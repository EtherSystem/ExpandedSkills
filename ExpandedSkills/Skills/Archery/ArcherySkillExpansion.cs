using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.Archery
{
    internal static class ArcherySkillExpansion
    {
        internal const string Level10Benefit = "BOW NOCK AND DRAW TIME REDUCED BY 30%";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.Archery, () => GameManager.GetSkillArchery(), Level10Benefit);
        }
    }
}
