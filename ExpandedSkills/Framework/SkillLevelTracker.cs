using ExpandedSkills.Persistence;

namespace ExpandedSkills.Framework
{
    internal static class SkillLevelTracker
    {
        private static readonly Dictionary<SkillType, int> LastLevels = new();
        private static readonly Dictionary<SkillType, int> LastPoints = new();
        private static bool s_InGameplayScene;
        private static float s_NextLevelCheckAt;

        internal static void ResetForMainMenu()
        {
            s_InGameplayScene = false;
            s_NextLevelCheckAt = 0f;
            LastLevels.Clear();
            LastPoints.Clear();
        }

        internal static void SuspendForSceneTransition()
        {
            s_InGameplayScene = false;
        }

        internal static void NotifySkillsDeserialized()
        {
            LastLevels.Clear();
            LastPoints.Clear();
            s_NextLevelCheckAt = Time.realtimeSinceStartup + 0.25f;
        }

        internal static void ArmForGameplayScene()
        {
            s_InGameplayScene = true;
            s_NextLevelCheckAt = Time.realtimeSinceStartup + 0.25f;
        }

        internal static void Update()
        {
            if (!s_InGameplayScene || Time.realtimeSinceStartup < s_NextLevelCheckAt) return;
            s_NextLevelCheckAt = Time.realtimeSinceStartup + 0.25f;

            foreach (ExpandedSkillDefinition definition in ExpandedSkillRegistry.All)
            {
                Skill skill = definition.GetSkill();
                if (skill == null) continue;

                int currentLevel = ExpandedSkillProgression.GetRealLevel(skill);
                int currentPoints = skill.GetPoints();

                if (!LastLevels.TryGetValue(skill.m_SkillType, out int previousLevel))
                {
                    LastLevels[skill.m_SkillType] = currentLevel;
                    LastPoints[skill.m_SkillType] = currentPoints;
                    continue;
                }

                if (!LastPoints.TryGetValue(skill.m_SkillType, out int previousPoints) || currentPoints != previousPoints)
                {
                    LastPoints[skill.m_SkillType] = currentPoints;
                    SaveDataManager.OnSkillPointsChanged(skill);
                }

                if (currentLevel == previousLevel) continue;
                LastLevels[skill.m_SkillType] = currentLevel;
                LogLevelChange(skill, previousLevel, currentLevel);
            }
        }

        internal static void ObserveLevelChange(Skill skill, int previousLevel)
        {
            if (!s_InGameplayScene || skill == null || !ExpandedSkillRegistry.TryGet(skill, out _)) return;

            int currentLevel = ExpandedSkillProgression.GetRealLevel(skill);
            int lastLevel = LastLevels.TryGetValue(skill.m_SkillType, out int trackedLevel) ? trackedLevel : previousLevel;
            LastLevels[skill.m_SkillType] = currentLevel;
            LastPoints[skill.m_SkillType] = skill.GetPoints();
            SaveDataManager.OnSkillPointsChanged(skill);
            if (currentLevel != lastLevel) LogLevelChange(skill, lastLevel, currentLevel);
        }

        private static void LogLevelChange(Skill skill, int previousLevel, int currentLevel)
        {
            Core.Log($"[Skill diagnostics][{ExpandedSkillRegistry.GetLogName(skill.m_SkillType)}] Level changed: {previousLevel} -> {currentLevel} | Points={skill.GetPoints()}");
        }
    }
}