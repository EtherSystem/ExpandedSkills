using ExpandedSkills.Framework;

namespace ExpandedSkills.Skills.Gunsmithing
{
    internal static class GunsmithingSkillExpansion
    {
        internal const string Level10Benefit = "AMMUNITION CRAFTING TIME REDUCED BY 50%";

        internal static ExpandedSkillDefinition CreateDefinition()
        {
            return new ExpandedSkillDefinition(SkillType.Gunsmithing, () => GameManager.GetSkillGunsmithing(), Level10Benefit);
        }
    }
}
