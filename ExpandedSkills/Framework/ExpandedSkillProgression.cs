using ExpandedSkills.Persistence;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace ExpandedSkills.Framework
{
    internal static class ExpandedSkillProgression
    {
        internal const int MaxLevel = 10;
        internal const int MaxTierIndex = MaxLevel - 1;
        private static readonly int[] RewardTierByRealTier = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
        private static readonly Dictionary<SkillType, TierPointCache> TierPointsBySkill = new();

        private sealed class TierPointCache
        {
            internal readonly int[] Vanilla;
            internal readonly int[] Expanded;

            internal TierPointCache(int[] vanilla, int[] expanded)
            {
                Vanilla = vanilla;
                Expanded = expanded;
            }
        }

        internal static void ResetRuntimeCache()
        {
            TierPointsBySkill.Clear();
        }

        internal static int GetRewardTierIndexFromRealTierIndex(int realTierIndex)
        {
            return RewardTierByRealTier[ClampTierIndex(realTierIndex)];
        }

        internal static void ApplyAllTierPointData()
        {
            if (!Core.IsGameplayActive) return;

            foreach (ExpandedSkillDefinition definition in ExpandedSkillRegistry.All)
            {
                ApplyTierPointData(definition.GetSkill());
            }
        }

        private static void ApplyTierPointData(Skill skill)
        {
            if (skill == null) return;

            int[] tierPoints = GetTierPoints(skill);
            Il2CppStructArray<int> current = skill.m_TierPoints;
            bool matches = current != null && current.Length == tierPoints.Length;
            if (matches)
            {
                for (int i = 0; i < tierPoints.Length; i++)
                {
                    if (current[i] == tierPoints[i]) continue;
                    matches = false;
                    break;
                }
            }

            if (!matches) skill.m_TierPoints = new Il2CppStructArray<int>(tierPoints);
        }

        internal static int[] GetTierPoints(Skill skill)
        {
            if (skill == null) return Array.Empty<int>();
            if (TierPointsBySkill.TryGetValue(skill.m_SkillType, out TierPointCache cached) && SourceMatches(skill.m_TierPoints, cached.Vanilla)) return cached.Expanded;

            int[] vanilla = ReadVanillaTierPoints(skill);
            int[] expanded = ExpandTierPoints(vanilla);
            TierPointsBySkill[skill.m_SkillType] = new TierPointCache(vanilla, expanded);
            return expanded;
        }

        internal static int[] GetVanillaTierPoints(Skill skill)
        {
            if (skill == null) return Array.Empty<int>();
            GetTierPoints(skill);
            return TierPointsBySkill.TryGetValue(skill.m_SkillType, out TierPointCache cached) ? cached.Vanilla : Array.Empty<int>();
        }

        internal static int GetVanillaCompatibilityPoints(Skill skill, int expandedPoints)
        {
            int[] vanilla = GetVanillaTierPoints(skill);
            if (vanilla.Length < 5) return Math.Max(0, expandedPoints);
            return Math.Max(0, Math.Min(expandedPoints, vanilla[4]));
        }

        private static bool SourceMatches(Il2CppStructArray<int> source, int[] vanilla)
        {
            if (source == null || vanilla == null || source.Length < 5 || vanilla.Length < 5) return false;
            for (int i = 0; i < 5; i++)
            {
                if (source[i] != vanilla[i]) return false;
            }

            return true;
        }

        private static int[] ReadVanillaTierPoints(Skill skill)
        {
            int[] values = new int[5];
            Il2CppStructArray<int> source = skill.m_TierPoints;
            int count = source == null ? 0 : Math.Min(5, source.Length);
            for (int i = 0; i < count; i++) values[i] = source[i];

            if (count == 0)
            {
                values[0] = 0;
                values[1] = 25;
                values[2] = 50;
                values[3] = 100;
                values[4] = 200;
            }
            else
            {
                for (int i = Math.Max(1, count); i < values.Length; i++)
                {
                    int previous = values[i - 1];
                    int priorDelta = i > 1 ? Math.Max(1, values[i - 1] - values[i - 2]) : Math.Max(1, previous);
                    values[i] = previous + priorDelta;
                }
            }

            values[0] = 0;
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] <= values[i - 1]) values[i] = values[i - 1] + 1;
            }

            return values;
        }

        private static int[] ExpandTierPoints(int[] vanilla)
        {
            int[] expanded = new int[MaxLevel];
            Array.Copy(vanilla, expanded, Math.Min(vanilla.Length, 5));

            int lastDelta = Math.Max(1, expanded[4] - expanded[3]);
            int deltaGrowth = Math.Max(1, lastDelta / 2);
            for (int i = 5; i < expanded.Length; i++)
            {
                lastDelta += deltaGrowth;
                expanded[i] = expanded[i - 1] + lastDelta;
            }

            return expanded;
        }

        internal static int ClampTierIndex(int index)
        {
            return Math.Max(0, Math.Min(MaxTierIndex, index));
        }

        internal static int GetTierIndexFromPoints(Skill skill, int points)
        {
            int[] tierPoints = GetTierPoints(skill);
            int tierIndex = 0;
            for (int i = 0; i < tierPoints.Length; i++)
            {
                if (points >= tierPoints[i]) tierIndex = i;
            }
            return ClampTierIndex(tierIndex);
        }

        internal static int GetRealLevel(Skill skill)
        {
            if (!Core.IsGameplayActive) return 1;
            return skill == null ? 1 : GetTierIndexFromPoints(skill, skill.GetPoints()) + 1;
        }

        internal static int GetRewardTierIndex(Skill skill)
        {
            return GetRewardTierIndexFromRealTierIndex(GetRealLevel(skill) - 1);
        }

        internal static bool HasRewardLevel(Skill skill, int vanillaLevel)
        {
            if (!Core.IsGameplayActive) return false;
            if (vanillaLevel <= 1) return true;
            return GetRewardTierIndex(skill) + 1 >= vanillaLevel;
        }

        internal static bool IsLevel10(SkillType skillType)
        {
            if (!Core.IsGameplayActive) return false;
            Skill skill = ExpandedSkillRegistry.GetSkill(skillType);
            return skill != null && GetRealLevel(skill) >= MaxLevel;
        }

        internal static int GetPointsForLevel(Skill skill, int level)
        {
            int[] tierPoints = GetTierPoints(skill);
            return tierPoints[Math.Max(0, Math.Min(MaxTierIndex, level - 1))];
        }

        internal static float GetProgressToNextLevel(Skill skill, int points, int addPoints)
        {
            int[] tierPoints = GetTierPoints(skill);
            int projectedPoints = Math.Max(0, points + addPoints);
            int currentTier = GetTierIndexFromPoints(skill, projectedPoints);
            if (currentTier >= MaxTierIndex) return 1f;

            int current = tierPoints[currentTier];
            int next = tierPoints[currentTier + 1];
            return next <= current ? 1f : Math.Max(0f, Math.Min(1f, (projectedPoints - current) / (float)(next - current)));
        }

        internal static void SetPointsDirect(Skill skill, int points)
        {
            if (!Core.IsGameplayActive) return;
            if (skill == null || !ExpandedSkillRegistry.TryGet(skill, out _)) return;

            int oldLevel = GetRealLevel(skill);
            int[] tierPoints = GetTierPoints(skill);
            skill.m_CurrentPoints = Math.Max(0, Math.Min(points, tierPoints[MaxTierIndex]));
            SaveDataManager.OnSkillPointsChanged(skill);
            SkillLevelTracker.ObserveLevelChange(skill, oldLevel);
        }

        internal static void RefreshSkillListItems(Panel_Log panel)
        {
            if (!Core.IsGameplayActive) return;
            if (panel == null || panel.m_SkillsDisplayList == null) return;

            for (int i = 0; i < panel.m_SkillsDisplayList.Count; i++)
            {
                SkillListItem item = panel.m_SkillsDisplayList[i];
                if (item == null || item.m_Skill == null || !ExpandedSkillRegistry.TryGet(item.m_Skill, out _)) continue;

                Skill skill = item.m_Skill;
                int[] tierPoints = GetTierPoints(skill);
                int points = skill.GetPoints();
                int tierIndex = GetTierIndexFromPoints(skill, points);
                item.SetSkillLevel(tierIndex);

                if (tierIndex >= MaxTierIndex)
                {
                    item.SetSkillPoints(tierPoints[MaxTierIndex] + "/" + tierPoints[MaxTierIndex]);
                    item.EnableProgressBar(false);
                    continue;
                }

                item.SetSkillPoints(points + "/" + tierPoints[tierIndex + 1]);
                item.SetProgress(GetProgressToNextLevel(skill, points, 0));
                item.EnableProgressBar(true);
            }
        }
    }
}