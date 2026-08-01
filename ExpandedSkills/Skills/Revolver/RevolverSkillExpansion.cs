using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.Revolver
{
    internal static class RevolverSkillExpansion
    {
        internal const string Level10Benefit = "CAN MOVE WHILE AIMING THE REVOLVER";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.Revolver, () => GameManager.GetSkillRevolver(), Level10Benefit);
        }
    }
}
