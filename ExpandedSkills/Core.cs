using ExpandedSkills.Framework;
using ExpandedSkills.Patches;
using ExpandedSkills.Persistence;

[assembly: MelonInfo(typeof(ExpandedSkills.Core), "ExpandedSkills", "1.1.2", "EtherSystem", null)]
[assembly: MelonGame("Hinterland", "TheLongDark")]

namespace ExpandedSkills
{
    public class Core : MelonMod
    {
        public static Core Instance { get; private set; }
        internal static ExpandedSkillsState State = new();

        private static readonly HashSet<string> ReportedExceptions = new(StringComparer.Ordinal);
        private static bool s_GameplayActive;
        private bool _dirty;

        internal static bool IsGameplayActive => s_GameplayActive;

        internal static void Log(string message)
        {
            Instance?.LoggerInstance.Msg(message);
        }

        internal static void Error(string message)
        {
            Instance?.LoggerInstance.Error(message);
        }

        internal static void LogExceptionOnce(string key, string context, Exception exception)
        {
            string normalizedKey = string.IsNullOrWhiteSpace(key) ? context : key;
            if (!ReportedExceptions.Add(normalizedKey)) return;

            string message = (exception.Message ?? string.Empty).Replace('\r', ' ').Replace('\n', ' ');
            Error($"{context} {exception.GetType().Name}: {message}");
        }

        internal void MarkDirty()
        {
            if (!SaveDataManager.IsStateLoaded) return;
            _dirty = true;
        }

        internal void SaveIfDirty()
        {
            if (!_dirty) return;

            SaveDataManager.OnSave();
            _dirty = false;
        }

        internal void BeginSlotLoad()
        {
            _dirty = false;
            SaveDataManager.BeginSlotLoad();
        }

        internal void ResetForNewGame()
        {
            _dirty = false;
            SaveDataManager.OnNewGame();
        }

        public override void OnInitializeMelon()
        {
            Instance = this;
            ExpandedSkillRegistry.Initialize();
            ExpandedSkillsSettingsManager.Initialize();
            RegisterConsoleCommands();
            Log("Initialized.");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (IsTransitionScene(sceneName))
            {
                SuspendGameplayRuntimeForTransition();
                return;
            }

            if (!IsGameplayScene(sceneName))
            {
                DeactivateGameplayRuntime();
                return;
            }

            s_GameplayActive = true;
            SkillLevelTracker.SuspendForSceneTransition();
            WeaponAnimationLevel10.Reset();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (IsTransitionScene(sceneName))
            {
                SuspendGameplayRuntimeForTransition();
                return;
            }

            if (!IsGameplayScene(sceneName))
            {
                DeactivateGameplayRuntime();
                return;
            }

            s_GameplayActive = true;
            ExpandedSkillProgression.ApplyAllTierPointData();
            SaveDataManager.OnGameplaySceneInitialized();
            ExpandedSkillProgression.ApplyAllTierPointData();
            SkillLevelTracker.ArmForGameplayScene();
        }

        public override void OnUpdate()
        {
            if (!s_GameplayActive || GameManager.m_Instance == null) return;

            WeaponAnimationLevel10.Update();
            SkillLevelTracker.Update();
        }

        private static void SuspendGameplayRuntimeForTransition()
        {
            s_GameplayActive = false;
            SkillLevelTracker.SuspendForSceneTransition();
            WeaponAnimationLevel10.Reset();
        }

        internal static void BeginMainMenuTransition()
        {
            SuspendGameplayRuntimeForTransition();
        }

        internal static void DeactivateGameplayRuntime()
        {
            s_GameplayActive = false;
            if (Instance != null) Instance._dirty = false;
            SkillLevelTracker.ResetForMainMenu();
            SkillActionDiagnostics.Reset();
            WeaponAnimationLevel10.Reset();
            ExpandedSkillProgression.ResetRuntimeCache();
            SaveDataManager.ForgetLoadedState();
        }

        private static void RegisterConsoleCommands()
        {
            RegisterSkillConsoleCommands(SkillType.Firestarting, "firestarting");
            RegisterSkillConsoleCommands(SkillType.CarcassHarvesting, "carcassharvesting");
            RegisterSkillConsoleCommands(SkillType.IceFishing, "icefishing");
            RegisterSkillConsoleCommands(SkillType.Cooking, "cooking");
            RegisterSkillConsoleCommands(SkillType.Rifle, "rifle");
            RegisterSkillConsoleCommands(SkillType.Archery, "archery");
            RegisterSkillConsoleCommands(SkillType.ClothingRepair, "mending");
            RegisterSkillConsoleCommands(SkillType.Revolver, "revolver");
            RegisterSkillConsoleCommands(SkillType.Gunsmithing, "gunsmithing");

            uConsole.RegisterCommand("xp_skill_level", new Action(() =>
            {
                var parameters = uConsole.GetAllParameters();
                if (parameters == null || parameters.Count < 2 || !TryParseSkillType(parameters[0], out SkillType skillType) || !int.TryParse(parameters[1], out int level))
                {
                    uConsole.Log("Usage: xp_skill_level [firestarting|carcass|icefishing|cooking|rifle|archery|mending|revolver|gunsmithing] [1-10]");
                    return;
                }

                SetSkillLevel(skillType, level);
            }));
            uConsole.RegisterCommand("xp_skill_points", new Action(() =>
            {
                var parameters = uConsole.GetAllParameters();
                if (parameters == null || parameters.Count < 2 || !TryParseSkillType(parameters[0], out SkillType skillType) || !int.TryParse(parameters[1], out int points))
                {
                    uConsole.Log("Usage: xp_skill_points [firestarting|carcass|icefishing|cooking|rifle|archery|mending|revolver|gunsmithing] [points]");
                    return;
                }

                SetSkillPoints(skillType, points);
            }));
            uConsole.RegisterCommand("es_skill_test", new Action(() =>
            {
                var parameters = uConsole.GetAllParameters();
                string mode = parameters != null && parameters.Count > 0 ? parameters[0].Trim().ToLowerInvariant() : "status";
                switch (mode)
                {
                    case "on":
                    case "enable":
                    case "1":
                        SkillActionDiagnostics.SetEnabled(true);
                        uConsole.Log("ExpandedSkills action-source diagnostics enabled. Perform gameplay actions at each level; use 'es_skill_test reset' before repeating a level.");
                        break;
                    case "off":
                    case "disable":
                    case "0":
                        SkillActionDiagnostics.SetEnabled(false);
                        uConsole.Log("ExpandedSkills action-source diagnostics disabled.");
                        break;
                    case "reset":
                        SkillActionDiagnostics.ResetSamples();
                        uConsole.Log("ExpandedSkills action-source diagnostic samples cleared.");
                        break;
                    case "status":
                        uConsole.Log("ExpandedSkills action-source diagnostics: " + (SkillActionDiagnostics.Enabled ? "ON" : "OFF"));
                        break;
                    default:
                        uConsole.Log("Usage: es_skill_test [on|off|reset|status]");
                        break;
                }
            }));
        }

        private static void RegisterSkillConsoleCommands(SkillType skillType, string commandName)
        {
            string levelCommand = "xp_" + commandName + "_level";
            string pointsCommand = "xp_" + commandName + "_points";
            uConsole.RegisterCommand(levelCommand, new Action(() => SetSkillLevelFromConsole(skillType, levelCommand + " [1-10]", 0)));
            uConsole.RegisterCommand(pointsCommand, new Action(() => SetSkillPointsFromConsole(skillType, pointsCommand + " [points]", 0)));
        }

        private static void SetSkillLevelFromConsole(SkillType skillType, string usage, int parameterIndex)
        {
            var parameters = uConsole.GetAllParameters();
            if (parameters == null || parameters.Count <= parameterIndex || !int.TryParse(parameters[parameterIndex], out int level))
            {
                uConsole.Log("Usage: " + usage);
                return;
            }

            SetSkillLevel(skillType, level);
        }

        private static void SetSkillPointsFromConsole(SkillType skillType, string usage, int parameterIndex)
        {
            var parameters = uConsole.GetAllParameters();
            if (parameters == null || parameters.Count <= parameterIndex || !int.TryParse(parameters[parameterIndex], out int points))
            {
                uConsole.Log("Usage: " + usage);
                return;
            }

            SetSkillPoints(skillType, points);
        }

        private static void SetSkillLevel(SkillType skillType, int level)
        {
            Skill skill = ExpandedSkillRegistry.GetSkill(skillType);
            if (skill == null)
            {
                uConsole.Log(skillType + " skill is not available.");
                return;
            }

            level = Math.Max(1, Math.Min(ExpandedSkillProgression.MaxLevel, level));
            ExpandedSkillProgression.ApplyAllTierPointData();
            ExpandedSkillProgression.SetPointsDirect(skill, ExpandedSkillProgression.GetPointsForLevel(skill, level));
            uConsole.Log($"{skill.m_DisplayName} set to level {ExpandedSkillProgression.GetRealLevel(skill)}, points {skill.GetPoints()}.");
        }

        private static void SetSkillPoints(SkillType skillType, int points)
        {
            Skill skill = ExpandedSkillRegistry.GetSkill(skillType);
            if (skill == null)
            {
                uConsole.Log(skillType + " skill is not available.");
                return;
            }

            ExpandedSkillProgression.ApplyAllTierPointData();
            ExpandedSkillProgression.SetPointsDirect(skill, points);
            uConsole.Log($"{skill.m_DisplayName} set to level {ExpandedSkillProgression.GetRealLevel(skill)}, points {skill.GetPoints()}.");
        }

        private static bool TryParseSkillType(string value, out SkillType skillType)
        {
            switch ((value ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "firestarting": case "fire": skillType = SkillType.Firestarting; return true;
                case "carcassharvesting": case "carcass": case "harvesting": skillType = SkillType.CarcassHarvesting; return true;
                case "icefishing": case "fishing": skillType = SkillType.IceFishing; return true;
                case "cooking": skillType = SkillType.Cooking; return true;
                case "rifle": skillType = SkillType.Rifle; return true;
                case "archery": case "bow": skillType = SkillType.Archery; return true;
                case "clothingrepair": case "mending": skillType = SkillType.ClothingRepair; return true;
                case "revolver": skillType = SkillType.Revolver; return true;
                case "gunsmithing": skillType = SkillType.Gunsmithing; return true;
                default: skillType = default; return false;
            }
        }

        internal static bool IsGameplayScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return false;

            string normalized = sceneName.ToLowerInvariant();
            if (normalized.StartsWith("boot", StringComparison.Ordinal)) return false;
            return !normalized.Contains("menu", StringComparison.Ordinal) && normalized != "empty";
        }

        private static bool IsTransitionScene(string sceneName)
        {
            return string.IsNullOrEmpty(sceneName) || sceneName.Equals("Empty", StringComparison.OrdinalIgnoreCase);
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.DoExitToMainMenu))]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.LoadMainMenu))]
    internal static class GameManagerMainMenuPatch
    {
        private static void Prefix() => Core.BeginMainMenuTransition();
        private static void Postfix() => Core.DeactivateGameplayRuntime();
    }
}