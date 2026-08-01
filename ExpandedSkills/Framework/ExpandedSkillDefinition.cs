
namespace ExpandedSkills.Framework
{
    internal sealed class ExpandedSkillDefinition
    {
        internal ExpandedSkillDefinition(SkillType skillType, Func<Skill> skillAccessor, string level10Benefit)
        {
            SkillType = skillType;
            SkillAccessor = skillAccessor ?? throw new ArgumentNullException(nameof(skillAccessor));
            Level10Benefit = level10Benefit ?? string.Empty;
        }

        internal SkillType SkillType { get; }
        internal Func<Skill> SkillAccessor { get; }
        internal string Level10Benefit { get; }

        internal Skill GetSkill()
        {
            return Core.IsGameplayActive ? GetSkillForPersistence() : null;
        }

        internal Skill GetSkillForPersistence()
        {
            try
            {
                return SkillAccessor();
            }
            catch (Exception exception)
            {
                Core.LogExceptionOnce($"skill-accessor-{SkillType}", $"Failed to resolve the {SkillType} skill instance.", exception);
                return null;
            }
        }

        internal SkillPanelPresentation BuildPresentation(Skill skill)
        {
            if (!Core.IsGameplayActive) return null;

            try
            {
                return SkillPanelPresentationBuilder.Build(skill, SkillType);
            }
            catch (Exception exception)
            {
                Core.LogExceptionOnce($"skill-presentation-{SkillType}", $"Failed to build the {SkillType} skill-panel presentation.", exception);
                return null;
            }
        }
    }
}
