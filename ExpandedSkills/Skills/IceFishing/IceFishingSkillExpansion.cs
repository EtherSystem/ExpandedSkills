using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.IceFishing
{
    internal static class IceFishingSkillExpansion
    {
        internal const string Level10Benefit = "FISHING LINES NEVER BREAK ON A CATCH";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.IceFishing, () => GameManager.GetSkillIceFishing(), Level10Benefit);
        }
    }
}
