namespace ExpandedSkills.Persistence
{
    internal sealed class ExpandedSkillsState
    {
        public int Version = SaveDataManager.CurrentVersion;
        public Dictionary<string, ExpandedSkillState> Skills = new(StringComparer.OrdinalIgnoreCase);
    }

    internal sealed class ExpandedSkillState
    {
        public int ExpandedPoints = 0;
        public int VanillaCompatibilityPoints = 0;
    }
}
