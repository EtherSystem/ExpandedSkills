using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.FireStarting
{
    internal static class FireStartingSkillExpansion
    {
        internal const string Level10Benefit = "TORCHES PULLED FROM FIRES HAVE 30% ADDITIONAL CONDITION";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.Firestarting, () => GameManager.GetSkillFireStarting(), Level10Benefit);
        }
    }
}
