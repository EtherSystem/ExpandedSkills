using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.Cooking
{
    internal static class CookingSkillExpansion
    {
        internal const string Level10Benefit = "STARVATION CONDITION LOSS REDUCED BY 50%";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.Cooking, () => GameManager.GetSkillCooking(), Level10Benefit);
        }
    }
}
