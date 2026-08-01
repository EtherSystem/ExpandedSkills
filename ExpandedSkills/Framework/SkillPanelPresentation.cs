namespace ExpandedSkills.Framework
{
    internal sealed class SkillPanelPresentation
    {
        internal SkillPanelPresentation(string skillName, int displayLevel, string rankName, string description, IReadOnlyList<string> benefits)
        {
            SkillName = skillName ?? string.Empty;
            DisplayLevel = displayLevel;
            RankName = rankName ?? string.Empty;
            Description = description ?? string.Empty;
            Benefits = benefits ?? new List<string>();
        }

        internal string SkillName { get; }
        internal int DisplayLevel { get; }
        internal string RankName { get; }
        internal string Description { get; }
        internal IReadOnlyList<string> Benefits { get; }
    }
}