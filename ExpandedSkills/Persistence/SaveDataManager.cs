using ExpandedSkills.Framework;
using ModData;
using System.Text.Json;

namespace ExpandedSkills.Persistence
{
    internal static class SaveDataManager
    {
        internal const int CurrentVersion = 1;

        private static readonly ModDataManager _manager = new("ExpandedSkills", false);
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true
        };
        private const string SUFFIX = "skilldata";

        private static readonly (SkillType SkillType, string Name)[] LoggedSkills =
        {
            (SkillType.CarcassHarvesting, "Carcass"),
            (SkillType.Cooking, "Cooking"),
            (SkillType.Firestarting, "Fire"),
            (SkillType.IceFishing, "Fishing"),
            (SkillType.Rifle, "Rifle"),
            (SkillType.Archery, "Archery"),
            (SkillType.ClothingRepair, "Mending"),
            (SkillType.Revolver, "Revolver"),
            (SkillType.Gunsmithing, "Gunsmithing")
        };

        private static bool _stateLoaded;
        private static bool _pendingVanillaReconciliation;
        private static bool _pendingFreshMigration;
        private static SkillsManager s_LastSkillsManager;
        private static string s_LastSerializedSkillsManagerData = string.Empty;
        private static string s_PendingLoadNote = string.Empty;

        internal static bool IsStateLoaded => _stateLoaded;

        internal static void BeginSlotLoad()
        {
            _stateLoaded = false;
            _pendingVanillaReconciliation = true;
            _pendingFreshMigration = false;
            s_LastSkillsManager = null;
            s_LastSerializedSkillsManagerData = string.Empty;
            s_PendingLoadNote = string.Empty;
            Core.State = new ExpandedSkillsState();
        }

        internal static void OnSkillsDeserialized(SkillsManager manager, string serializedSkillsManagerData)
        {
            s_LastSkillsManager = manager;
            s_LastSerializedSkillsManagerData = serializedSkillsManagerData ?? string.Empty;
            if (!_stateLoaded) return;

            if (_pendingFreshMigration)
            {
                FinalizePendingFreshMigration(manager, s_LastSerializedSkillsManagerData);
                return;
            }

            bool changed = ApplyLoadedState(manager, s_LastSerializedSkillsManagerData, _pendingVanillaReconciliation);
            _pendingVanillaReconciliation = false;
            if (changed) Core.Instance?.MarkDirty();
        }

        internal static void EnsureLoadedFromCurrentSkills()
        {
            if (_stateLoaded) return;

            SkillsManager manager = s_LastSkillsManager ?? GameManager.GetSkillsManager();
            LoadAndApply(manager, s_LastSerializedSkillsManagerData);
        }

        internal static void OnGameplaySceneInitialized()
        {
            SkillsManager manager = GameManager.GetSkillsManager();
            if (manager == null) return;

            if (!_stateLoaded)
            {
                LoadAndApply(manager, s_LastSerializedSkillsManagerData);
                return;
            }

            if (_pendingFreshMigration) return;

            EnsureState();
            bool changed = ApplyLoadedState(manager, string.Empty, reconcileVanillaProgress: false);
            if (changed) Core.Instance?.MarkDirty();
        }

        internal static void OnSkillPointsChanged(Skill skill)
        {
            if (!_stateLoaded || skill == null) return;
            if (!ExpandedSkillRegistry.TryGetForPersistence(skill.m_SkillType, out _)) return;
            if (_pendingFreshMigration)
            {
                Core.Instance?.MarkDirty();
                return;
            }

            EnsureState();
            ExpandedSkillState state = GetOrCreateSkillState(skill.m_SkillType);
            int expandedPoints = ClampExpandedPoints(skill, skill.m_CurrentPoints);
            int vanillaPoints = ExpandedSkillProgression.GetVanillaCompatibilityPoints(skill, expandedPoints);

            if (state.ExpandedPoints == expandedPoints && state.VanillaCompatibilityPoints == vanillaPoints) return;

            state.ExpandedPoints = expandedPoints;
            state.VanillaCompatibilityPoints = vanillaPoints;
            Core.Instance?.MarkDirty();
        }

        internal static void OnSave()
        {
            if (!_stateLoaded) EnsureLoadedFromCurrentSkills();
            if (!_stateLoaded) return;
            if (_pendingFreshMigration) FinalizePendingFreshMigration(s_LastSkillsManager ?? GameManager.GetSkillsManager(), s_LastSerializedSkillsManagerData);

            EnsureState();
            CaptureRuntimeState();

            try
            {
                string json = JsonSerializer.Serialize(Core.State, JsonOptions);
                if (!_manager.Save(json, SUFFIX))
                {
                    Core.Error("[ModData] Failed to save ExpandedSkills state.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Core.LogExceptionOnce("moddata-save", "[ModData] Failed to save ExpandedSkills state.", ex);
                return;
            }

            LogStateSnapshot("Saved");
        }

        internal static void OnNewGame()
        {
            _stateLoaded = true;
            _pendingVanillaReconciliation = false;
            _pendingFreshMigration = false;
            s_LastSkillsManager = null;
            s_LastSerializedSkillsManagerData = string.Empty;
            s_PendingLoadNote = string.Empty;
            Core.State = new ExpandedSkillsState();
            Core.Instance?.MarkDirty();
            Core.Log("[ModData] Clearing data for new game.");
        }

        internal static void ForgetLoadedState()
        {
            _stateLoaded = false;
            _pendingVanillaReconciliation = false;
            _pendingFreshMigration = false;
            s_LastSkillsManager = null;
            s_LastSerializedSkillsManagerData = string.Empty;
            s_PendingLoadNote = string.Empty;
            Core.State = new ExpandedSkillsState();
        }

        internal static ExpandedSkillPointPersistence.CompatibilityPointScope ApplyVanillaCompatibilityPointsForSerialization()
        {
            if (!_stateLoaded) return null;
            if (_pendingFreshMigration) FinalizePendingFreshMigration(s_LastSkillsManager ?? GameManager.GetSkillsManager(), s_LastSerializedSkillsManagerData);

            if (CaptureRuntimeState()) Core.Instance?.MarkDirty();
            return ExpandedSkillPointPersistence.ApplyVanillaCompatibilityPoints(Core.State);
        }

        private static void LoadAndApply(SkillsManager manager, string serializedSkillsManagerData)
        {
            string json = string.Empty;

            try
            {
                json = _manager.Load(SUFFIX) ?? string.Empty;
            }
            catch (Exception ex)
            {
                Core.LogExceptionOnce("moddata-load", "[ModData] Failed to load ExpandedSkills state.", ex);
            }

            bool freshState = string.IsNullOrEmpty(json);
            bool corruptedState = false;
            ExpandedSkillsState loaded = null;

            if (!freshState)
            {
                try
                {
                    loaded = JsonSerializer.Deserialize<ExpandedSkillsState>(json, JsonOptions);
                }
                catch
                {
                    corruptedState = true;
                }
            }

            Core.State = loaded ?? new ExpandedSkillsState();
            bool normalized = NormalizeState();
            _stateLoaded = true;

            string note = freshState
                ? "empty data / preserved current skill progression"
                : corruptedState
                    ? "corrupted data / rebuilt safely from current skill progression"
                    : null;

            bool deferFreshMigration = (freshState || corruptedState) && string.IsNullOrEmpty(serializedSkillsManagerData);
            if (deferFreshMigration)
            {
                _pendingFreshMigration = true;
                s_PendingLoadNote = note ?? "waiting for vanilla skill progression";
                if (freshState || corruptedState || normalized) Core.Instance?.MarkDirty();
                Core.Log("[ModData] Skill import deferred until the loaded save exposes its real skill points.");
                return;
            }

            bool hasSerializedData = !string.IsNullOrEmpty(serializedSkillsManagerData);
            bool changed = ApplyLoadedState(manager, serializedSkillsManagerData, reconcileVanillaProgress: hasSerializedData && !freshState && !corruptedState);
            if (hasSerializedData && manager != null) _pendingVanillaReconciliation = false;
            if (freshState || corruptedState || normalized || changed) Core.Instance?.MarkDirty();

            LogStateSnapshot("Loaded", note);
        }

        private static void FinalizePendingFreshMigration(SkillsManager manager, string serializedSkillsManagerData)
        {
            if (!_pendingFreshMigration || manager == null) return;

            Core.State = new ExpandedSkillsState();
            bool changed = ApplyLoadedState(manager, serializedSkillsManagerData, reconcileVanillaProgress: false);
            _pendingFreshMigration = false;
            _pendingVanillaReconciliation = false;
            if (changed) Core.Instance?.MarkDirty();

            string note = string.IsNullOrEmpty(s_PendingLoadNote)
                ? "preserved current skill progression"
                : s_PendingLoadNote;
            s_PendingLoadNote = string.Empty;
            LogStateSnapshot("Loaded", note);
        }

        private static bool ApplyLoadedState(SkillsManager manager, string serializedSkillsManagerData, bool reconcileVanillaProgress)
        {
            if (manager == null) return false;

            EnsureState();

            IReadOnlyDictionary<SkillType, int> serializedPoints = ExpandedSkillPointPersistence.ReadSerializedPoints(serializedSkillsManagerData);
            bool changed = false;

            foreach (ExpandedSkillDefinition definition in ExpandedSkillRegistry.All)
            {
                Skill skill = definition.GetSkillForPersistence();
                if (skill == null) continue;

                bool hadState = TryGetSkillState(skill.m_SkillType, out ExpandedSkillState state);
                state ??= GetOrCreateSkillState(skill.m_SkillType);

                int vanillaMaximum = ExpandedSkillProgression.GetVanillaTierPoints(skill)[4];
                int loadedRawPoints = serializedPoints.TryGetValue(skill.m_SkillType, out int rawPoints)
                    ? Math.Max(0, rawPoints)
                    : Math.Max(0, skill.m_CurrentPoints);
                int loadedVanillaPoints = Math.Min(loadedRawPoints, vanillaMaximum);
                int targetExpandedPoints;

                if (!hadState)
                {
                    targetExpandedPoints = ClampExpandedPoints(skill, loadedRawPoints);
                    state.ExpandedPoints = targetExpandedPoints;
                    state.VanillaCompatibilityPoints = ExpandedSkillProgression.GetVanillaCompatibilityPoints(skill, targetExpandedPoints);
                    changed = true;

                    Core.Log($"[ModData][{ExpandedSkillRegistry.GetLogName(skill.m_SkillType)}] Preserved loaded progression: LoadedPoints={loadedRawPoints} -> ExpandedPoints={targetExpandedPoints}.");
                }
                else
                {
                    int savedExpandedPoints = ClampExpandedPoints(skill, state.ExpandedPoints);
                    int savedVanillaPoints = Math.Max(0, Math.Min(state.VanillaCompatibilityPoints, vanillaMaximum));
                    targetExpandedPoints = savedExpandedPoints;

                    if (reconcileVanillaProgress)
                    {
                        int importedExpandedPoints = ClampExpandedPoints(skill, loadedRawPoints);
                        bool hasNewSerializedProgress = loadedVanillaPoints > savedVanillaPoints || loadedRawPoints > vanillaMaximum;
                        if (hasNewSerializedProgress && importedExpandedPoints > targetExpandedPoints)
                        {
                            Core.Log($"[ModData][{ExpandedSkillRegistry.GetLogName(skill.m_SkillType)}] Imported newer serialized progression: CompatibilityPoints={savedVanillaPoints}->{loadedVanillaPoints} | ExpandedPoints={targetExpandedPoints}->{importedExpandedPoints}.");
                            targetExpandedPoints = importedExpandedPoints;
                            changed = true;
                        }
                    }

                    int normalizedVanillaPoints = ExpandedSkillProgression.GetVanillaCompatibilityPoints(skill, targetExpandedPoints);
                    if (state.ExpandedPoints != targetExpandedPoints || state.VanillaCompatibilityPoints != normalizedVanillaPoints) changed = true;
                    state.ExpandedPoints = targetExpandedPoints;
                    state.VanillaCompatibilityPoints = normalizedVanillaPoints;
                }

                if (skill.m_CurrentPoints != targetExpandedPoints) skill.m_CurrentPoints = targetExpandedPoints;
            }

            return changed;
        }

        private static bool CaptureRuntimeState()
        {
            EnsureState();
            bool changed = false;

            foreach (ExpandedSkillDefinition definition in ExpandedSkillRegistry.All)
            {
                Skill skill = definition.GetSkillForPersistence();
                if (skill == null) continue;

                ExpandedSkillState state = GetOrCreateSkillState(skill.m_SkillType);
                int expandedPoints = ClampExpandedPoints(skill, skill.m_CurrentPoints);
                int vanillaPoints = ExpandedSkillProgression.GetVanillaCompatibilityPoints(skill, expandedPoints);
                if (state.ExpandedPoints != expandedPoints || state.VanillaCompatibilityPoints != vanillaPoints) changed = true;
                state.ExpandedPoints = expandedPoints;
                state.VanillaCompatibilityPoints = vanillaPoints;
            }

            return changed;
        }

        private static int ClampExpandedPoints(Skill skill, int points)
        {
            int[] expandedTierPoints = ExpandedSkillProgression.GetTierPoints(skill);
            int maximum = expandedTierPoints.Length == 0 ? 0 : expandedTierPoints[ExpandedSkillProgression.MaxTierIndex];
            return Math.Max(0, Math.Min(points, maximum));
        }

        private static bool NormalizeState()
        {
            EnsureState();
            bool changed = Core.State.Version != CurrentVersion;
            Core.State.Version = CurrentVersion;

            Dictionary<string, ExpandedSkillState> normalized = new(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, ExpandedSkillState> pair in Core.State.Skills)
            {
                if (string.IsNullOrWhiteSpace(pair.Key))
                {
                    changed = true;
                    continue;
                }

                ExpandedSkillState state = pair.Value ?? new ExpandedSkillState();
                int expandedPoints = Math.Max(0, state.ExpandedPoints);
                int vanillaPoints = Math.Max(0, state.VanillaCompatibilityPoints);
                if (pair.Value == null || expandedPoints != state.ExpandedPoints || vanillaPoints != state.VanillaCompatibilityPoints) changed = true;

                state.ExpandedPoints = expandedPoints;
                state.VanillaCompatibilityPoints = vanillaPoints;
                normalized[pair.Key] = state;
            }

            if (Core.State.Skills.Comparer != StringComparer.OrdinalIgnoreCase || normalized.Count != Core.State.Skills.Count) changed = true;
            Core.State.Skills = normalized;
            return changed;
        }

        private static void EnsureState()
        {
            Core.State ??= new ExpandedSkillsState();
            Core.State.Skills ??= new Dictionary<string, ExpandedSkillState>(StringComparer.OrdinalIgnoreCase);
        }

        private static bool TryGetSkillState(SkillType skillType, out ExpandedSkillState state)
        {
            EnsureState();
            return Core.State.Skills.TryGetValue(skillType.ToString(), out state);
        }

        private static ExpandedSkillState GetOrCreateSkillState(SkillType skillType)
        {
            EnsureState();
            string key = skillType.ToString();
            if (Core.State.Skills.TryGetValue(key, out ExpandedSkillState state) && state != null) return state;

            state = new ExpandedSkillState();
            Core.State.Skills[key] = state;
            return state;
        }

        private static void LogStateSnapshot(string title, string note = null)
        {
            EnsureState();

            Core.Log($"================== {title} ==================");
            if (!string.IsNullOrEmpty(note)) Core.Log($"Data : {note}");
            Core.Log($"ModData : Version:{Core.State.Version} | Skills:{Core.State.Skills.Count}");
            Core.Log("Skill progression :");

            bool runtimeReady = GameManager.GetSkillsManager() != null;
            foreach ((SkillType skillType, string name) in LoggedSkills)
            {
                Skill skill = runtimeReady ? ExpandedSkillRegistry.GetSkillForPersistence(skillType) : null;
                if (!TryGetSkillState(skillType, out ExpandedSkillState state) || state == null)
                {
                    Core.Log($"  {name,-12}: no data");
                    continue;
                }

                string level = skill == null ? "n/a" : (ExpandedSkillProgression.GetTierIndexFromPoints(skill, state.ExpandedPoints) + 1).ToString();
                Core.Log($"  {name,-12}: Level:{level} | ExpandedPoints:{state.ExpandedPoints} | VanillaCompatibility:{state.VanillaCompatibilityPoints}");
            }

            Core.Log("===========================================");
        }
    }

    [HarmonyPatch(typeof(SaveGameSlots), nameof(SaveGameSlots.WriteSlotToDisk), [typeof(SlotData), typeof(SaveGameSlots.Timestamp)])]
    internal static class ExpandedSkillsSavePatch
    {
        private static void Prefix()
        {
            Core.Instance?.SaveIfDirty();
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadSaveGameSlot), [typeof(string), typeof(int)])]
    internal static class ExpandedSkillsLoadPatch
    {
        private static void Prefix()
        {
            Core.Instance?.BeginSlotLoad();
        }

        private static void Postfix()
        {
            SaveDataManager.EnsureLoadedFromCurrentSkills();
        }
    }

    [HarmonyPatch(typeof(SaveGameSlots), nameof(SaveGameSlots.CreateSlot), [typeof(string), typeof(SaveSlotType), typeof(uint), typeof(Episode)])]
    internal static class ExpandedSkillsNewGamePatch
    {
        private static void Postfix()
        {
            Core.Instance?.ResetForNewGame();
        }
    }
}
