using ExpandedSkills.Skills.Archery;
using ExpandedSkills.Skills.CarcassHarvesting;
using ExpandedSkills.Skills.Cooking;
using ExpandedSkills.Skills.FireStarting;
using ExpandedSkills.Skills.Gunsmithing;
using ExpandedSkills.Skills.IceFishing;
using ExpandedSkills.Skills.Mending;
using ExpandedSkills.Skills.Revolver;
using ExpandedSkills.Skills.Rifle;

namespace ExpandedSkills.Framework
{
    internal static class ExpandedSkillRegistry
    {
        private static readonly Dictionary<SkillType, ExpandedSkillDefinition> Definitions = new();
        private static readonly List<ExpandedSkillDefinition> OrderedDefinitions = new();

        internal static IReadOnlyList<ExpandedSkillDefinition> All => OrderedDefinitions;

        internal static void Initialize()
        {
            Definitions.Clear();
            OrderedDefinitions.Clear();

            Register(FireStartingSkillExpansion.CreateDefinition());
            Register(CarcassHarvestingSkillExpansion.CreateDefinition());
            Register(IceFishingSkillExpansion.CreateDefinition());
            Register(CookingSkillExpansion.CreateDefinition());
            Register(RifleSkillExpansion.CreateDefinition());
            Register(ArcherySkillExpansion.CreateDefinition());
            Register(MendingSkillExpansion.CreateDefinition());
            Register(RevolverSkillExpansion.CreateDefinition());
            Register(GunsmithingSkillExpansion.CreateDefinition());
        }

        private static void Register(ExpandedSkillDefinition definition)
        {
            Definitions[definition.SkillType] = definition;
            OrderedDefinitions.Add(definition);
        }

        internal static bool TryGet(SkillType skillType, out ExpandedSkillDefinition definition)
        {
            if (!Core.IsGameplayActive)
            {
                definition = null;
                return false;
            }

            return Definitions.TryGetValue(skillType, out definition);
        }

        internal static bool TryGet(Skill skill, out ExpandedSkillDefinition definition)
        {
            definition = null;
            return skill != null && TryGet(skill.m_SkillType, out definition);
        }

        internal static bool TryGetForPersistence(SkillType skillType, out ExpandedSkillDefinition definition)
        {
            return Definitions.TryGetValue(skillType, out definition);
        }

        internal static Skill GetSkill(SkillType skillType)
        {
            return TryGet(skillType, out ExpandedSkillDefinition definition) ? definition.GetSkill() : null;
        }

        internal static Skill GetSkillForPersistence(SkillType skillType)
        {
            return TryGetForPersistence(skillType, out ExpandedSkillDefinition definition) ? definition.GetSkillForPersistence() : null;
        }

        internal static string GetLogName(SkillType skillType)
        {
            return skillType switch
            {
                SkillType.CarcassHarvesting => "Carcass",
                SkillType.Cooking => "Cooking",
                SkillType.Firestarting => "Fire",
                SkillType.IceFishing => "Fishing",
                SkillType.Rifle => "Rifle",
                SkillType.Archery => "Archery",
                SkillType.ClothingRepair => "Mending",
                SkillType.Revolver => "Revolver",
                SkillType.Gunsmithing => "Gunsmithing",
                _ => skillType.ToString()
            };
        }
    }
}