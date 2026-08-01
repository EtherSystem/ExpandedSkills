using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.CarcassHarvesting
{
    internal static class CarcassHarvestingSkillExpansion
    {
        internal const string Level10Benefit = "10% MORE MEAT RECOVERED FROM CARCASSES";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.CarcassHarvesting, () => GameManager.GetSkillCarcassHarvesting(), Level10Benefit);
        }
    }
}
