using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using ExpandedSkills.Persistence;

namespace ExpandedSkills.Framework
{
    internal static class ExpandedSkillPointPersistence
    {
        private static readonly Regex PointsPattern = new("\"m_Points\"\\s*:\\s*(-?\\d+)", RegexOptions.Compiled);

        private static readonly (string PropertyName, SkillType SkillType)[] SerializedSkillProperties =
        {
            ("m_Skill_FirestartingSerialized", SkillType.Firestarting),
            ("m_Skill_CarcassHarvestingSerialized", SkillType.CarcassHarvesting),
            ("m_Skill_CookingSerialized", SkillType.Cooking),
            ("m_Skill_IceFishingSerialized", SkillType.IceFishing),
            ("m_Skill_RifleSerialized", SkillType.Rifle),
            ("m_Skill_ArcherySerialized", SkillType.Archery),
            ("m_Skill_ClothingRepairSerialized", SkillType.ClothingRepair),
            ("m_Skill_RevolverSerialized", SkillType.Revolver),
            ("m_Skill_GunsmithingSerialized", SkillType.Gunsmithing)
        };

        internal static IReadOnlyDictionary<SkillType, int> ReadSerializedPoints(string text)
        {
            Dictionary<SkillType, int> pointsBySkill = new();
            if (string.IsNullOrEmpty(text)) return pointsBySkill;

            try
            {
                using JsonDocument document = JsonDocument.Parse(text);
                JsonElement root = document.RootElement;

                foreach ((string propertyName, SkillType skillType) in SerializedSkillProperties)
                {
                    if (!root.TryGetProperty(propertyName, out JsonElement property) || property.ValueKind != JsonValueKind.String) continue;
                    if (!TryReadPoints(property.GetString(), out int points)) continue;
                    pointsBySkill[skillType] = points;
                }
            }
            catch (Exception ex)
            {
                Core.LogExceptionOnce("skill-point-read", "[ModData] Failed to read serialized vanilla skill points.", ex);
            }

            return pointsBySkill;
        }

        internal static CompatibilityPointScope ApplyVanillaCompatibilityPoints(ExpandedSkillsState state)
        {
            if (state?.Skills == null) return null;

            CompatibilityPointScope scope = new();

            foreach (ExpandedSkillDefinition definition in ExpandedSkillRegistry.All)
            {
                Skill skill = definition.GetSkillForPersistence();
                if (skill == null) continue;
                if (!state.Skills.TryGetValue(skill.m_SkillType.ToString(), out ExpandedSkillState skillState) || skillState == null) continue;

                scope.Capture(skill);
                int vanillaMaximum = ExpandedSkillProgression.GetVanillaTierPoints(skill)[4];
                skill.m_CurrentPoints = Math.Max(0, Math.Min(skillState.VanillaCompatibilityPoints, vanillaMaximum));
            }

            return scope;
        }

        private static bool TryReadPoints(string serializedSkill, out int points)
        {
            points = 0;
            if (string.IsNullOrEmpty(serializedSkill)) return false;

            Match match = PointsPattern.Match(serializedSkill);
            return match.Success && int.TryParse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out points);
        }

        internal sealed class CompatibilityPointScope
        {
            private readonly List<CapturedSkillPoints> _captured = new();
            private bool _restored;

            internal void Capture(Skill skill)
            {
                _captured.Add(new CapturedSkillPoints(skill, skill.m_CurrentPoints));
            }

            internal void Restore()
            {
                if (_restored) return;
                _restored = true;

                foreach (CapturedSkillPoints captured in _captured)
                {
                    if (captured.Skill != null) captured.Skill.m_CurrentPoints = captured.Points;
                }
            }
        }

        private sealed class CapturedSkillPoints
        {
            internal CapturedSkillPoints(Skill skill, int points)
            {
                Skill = skill;
                Points = points;
            }

            internal Skill Skill { get; }
            internal int Points { get; }
        }
    }
}
