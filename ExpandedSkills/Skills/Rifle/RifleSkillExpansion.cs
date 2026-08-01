using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.Rifle
{
    internal static class RifleSkillExpansion
    {
        internal const string Level10Benefit = "RIFLE RELOAD TIME REDUCED BY 25%";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.Rifle, () => GameManager.GetSkillRifle(), Level10Benefit);
        }
    }
}
