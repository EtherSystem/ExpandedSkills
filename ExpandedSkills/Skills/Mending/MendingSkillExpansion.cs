using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.Mending
{
    internal static class MendingSkillExpansion
    {
        internal const string Level10Benefit = "RUINED CLOTHING CAN BE RESTORED TO 10% CONDITION";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.ClothingRepair, () => GameManager.GetSkillClothingRepair(), Level10Benefit);
        }
    }
}
