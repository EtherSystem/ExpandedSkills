namespace ExpandedSkills.Framework
{
    internal static class ExpandedSkillRewardData
    {
        internal static readonly int[] CookingCalorieBonus = { 0, 5, 10, 12, 15, 18, 20, 22, 25, 25 };
        internal static readonly int[] CookingTimeReduction = { 0, 0, 0, 5, 10, 15, 20, 25, 30, 30 };
        internal static readonly int[] CookingReadyTimeIncrease = { 0, 0, 0, 0, 0, 10, 20, 20, 20, 20 };
        internal static readonly float[] CookingLowConditionChance = { 20f, 18f, 15f, 12f, 10f, 8f, 5f, 2f, 0f, 0f };
        internal static readonly float[] CookingMaximumCondition = { 75f, 75f, 75f, 80f, 85f, 85f, 85f, 95f, 100f, 100f };

        internal static readonly int[] FirestartingSuccessChance = { 40, 48, 55, 60, 65, 70, 75, 82, 90, 90 };
        internal static readonly int[] FirestartingDurationIncrease = { 0, 5, 10, 10, 10, 18, 25, 38, 50, 50 };
        internal static readonly int[] FirestartingTimeReduction = { 0, 0, 0, 0, 0, 0, 0, 25, 50, 50 };

        internal static readonly int[] CarcassMeatTimeReduction = { 0, 5, 10, 18, 25, 28, 30, 40, 50, 50 };
        internal static readonly int[] CarcassHideGutTimeReduction = { 0, 0, 0, 5, 10, 15, 20, 25, 30, 30 };
        internal static readonly int[] CarcassBarehandedFrozenThreshold = { 0, 0, 0, 0, 50, 60, 75, 90, 100, 100 };

        internal static readonly float[] IceFishingLineBreakChance = { 12f, 10f, 8f, 6f, 5f, 4f, 3f, 2f, 1f, 0f };
        internal static readonly int[] IceFishingTimeReduction = { 0, 2, 5, 7, 10, 15, 20, 25, 30, 30 };
        internal static readonly int[] IceFishingWeightIncrease = { 0, 0, 0, 0, 0, 5, 10, 18, 25, 25 };

        internal static readonly int[] MendingSuccessChance = { 50, 60, 65, 70, 75, 80, 85, 92, 100, 100 };
        internal static readonly int[] MendingTimeReduction = { 0, 5, 10, 12, 15, 20, 25, 32, 40, 40 };
        internal static readonly int[] MendingConditionIncrease = { 0, 0, 0, 5, 10, 12, 15, 20, 25, 25 };
        internal static readonly int[] MendingToolWearReduction = { 0, 0, 0, 0, 0, 12, 25, 30, 35, 35 };

        internal static readonly int[] ArcherySwayReduction = { 0, 10, 25, 40, 50, 60, 75, 75, 75, 75 };
        internal static readonly int[] ArcheryDamageIncrease = { 0, 5, 10, 10, 10, 10, 10, 18, 25, 25 };
        internal static readonly int[] ArcheryCriticalChanceIncrease = { 0, 0, 0, 8, 15, 20, 25, 38, 50, 50 };
        internal static readonly int[] ArcheryBleedTimeReduction = { 0, 0, 0, 0, 0, 10, 25, 40, 50, 50 };
        internal static readonly int[] ArcheryConditionWearReduction = { 0, 0, 0, 0, 0, 25, 50, 50, 50, 50 };

        internal static readonly int[] RifleCriticalChanceIncrease = { 0, 5, 10, 12, 15, 18, 20, 25, 30, 30 };
        internal static readonly int[] RifleRepairBonus = { 0, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        internal static readonly int[] RifleAccuracyRangeIncrease = { 0, 0, 0, 10, 20, 25, 30, 35, 40, 40 };
        internal static readonly int[] RifleDamageIncrease = { 0, 0, 0, 0, 0, 5, 10, 15, 20, 20 };
        internal static readonly int[] RifleStabilityBonus = { 0, 5, 10, 15, 20, 25, 30, 40, 50, 50 };
        internal static readonly float[] RifleEffectiveRange = { 75f, 80f, 90f, 100f, 110f, 130f, 150f, 200f, 250f, 250f };
        internal static readonly float[] RifleAimAssistAngle = { 0f, 0.1f, 0.2f, 0.3f, 0.35f, 0.4f, 0.5f, 0.55f, 0.6f, 0.6f };
        internal static readonly int[] RifleConditionWearReduction = { 0, 0, 0, 0, 0, 0, 0, 25, 50, 50 };

        internal static readonly int[] RevolverCriticalChanceIncrease = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        internal static readonly int[] RevolverRepairBonus = { 0, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        internal static readonly int[] RevolverRecoilCompensation = { 0, 10, 25, 30, 35, 40, 50, 60, 70, 70 };
        internal static readonly int[] RevolverDamageIncrease = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        internal static readonly int[] RevolverConditionWearReduction = { 0, 0, 0, 0, 0, 0, 0, 25, 50, 50 };
        internal static readonly float[] RevolverAimAssistAngle = { 0f, 0f, 0f, 0f, 0f, 0.5f, 1f, 1.5f, 2f, 2f };
        internal static readonly int[] RevolverStruggleBonus = { 0, 0, 0, 5, 10, 15, 20, 25, 30, 30 };

        internal static readonly int[] GunsmithingAmmoCondition = { 20, 30, 40, 50, 60, 70, 80, 90, 100, 100 };
        internal static readonly float[] GunsmithingHarvestSuccess = { 50f, 55f, 60f, 65f, 70f, 75f, 80f, 90f, 100f, 100f };
        internal static readonly float[] GunsmithingMillingSuccess = { 50f, 55f, 60f, 68f, 75f, 82f, 90f, 95f, 100f, 100f };
        internal static readonly float[] GunsmithingMillingCondition = { 40f, 50f, 60f, 70f, 80f, 85f, 90f, 95f, 100f, 100f };

        internal static int Get(int[] values, Skill skill) => values[GetLevelIndex(skill)];
        internal static float Get(float[] values, Skill skill) => values[GetLevelIndex(skill)];

        internal static int GetLevelIndex(Skill skill)
        {
            return Math.Max(0, Math.Min(ExpandedSkillProgression.MaxTierIndex, ExpandedSkillProgression.GetRealLevel(skill) - 1));
        }

        internal static string GetBenefits(Skill skill, int realTierIndex)
        {
            if (skill == null) return string.Empty;

            int index = Math.Max(0, Math.Min(ExpandedSkillProgression.MaxTierIndex, realTierIndex));
            int templateIndex = Math.Min(4, (index + 1) / 2);
            string template = GetVanillaTemplate(skill, templateIndex);
            if (string.IsNullOrEmpty(template)) return string.Empty;

            List<string> lines = SplitLines(template);
            if ((index & 1) == 1 && index < ExpandedSkillProgression.MaxTierIndex) RemoveNewTokenlessLines(skill, templateIndex, lines);
            if (skill.m_SkillType == SkillType.CarcassHarvesting && CarcassBarehandedFrozenThreshold[index] <= 0) RemoveLinesContaining(lines, "{frozen-threshold}");
            if (skill.m_SkillType == SkillType.IceFishing && index >= ExpandedSkillProgression.MaxTierIndex) RemoveLinesContaining(lines, "{chancebreak}");

            string text = string.Join("\n", lines);
            return ReplaceTokens(skill, index, text);
        }

        private static string GetVanillaTemplate(Skill skill, int templateIndex)
        {
            if (skill.m_TierLocalizedBenefits == null || templateIndex < 0 || templateIndex >= skill.m_TierLocalizedBenefits.Length) return string.Empty;
            return skill.m_TierLocalizedBenefits[templateIndex].Text() ?? string.Empty;
        }

        private static List<string> SplitLines(string text)
        {
            List<string> lines = new();
            string[] parts = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                string line = parts[i].Trim();
                if (line.Length > 0) lines.Add(line);
            }
            return lines;
        }

        private static void RemoveNewTokenlessLines(Skill skill, int templateIndex, List<string> lines)
        {
            if (templateIndex <= 0) return;

            HashSet<string> previousLines = new(SplitLines(GetVanillaTemplate(skill, templateIndex - 1)), StringComparer.Ordinal);
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                string line = lines[i];
                if (line.Contains('{') || previousLines.Contains(line)) continue;
                lines.RemoveAt(i);
            }
        }

        private static void RemoveLinesContaining(List<string> lines, string token)
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].Contains(token, StringComparison.Ordinal)) lines.RemoveAt(i);
            }
        }

        private static string ReplaceTokens(Skill skill, int index, string text)
        {
            return skill switch
            {
                Skill_Cooking cooking => ReplaceCooking(text, index),
                Skill_Firestarting firestarting => ReplaceFirestarting(text, index),
                Skill_CarcassHarvesting carcass => ReplaceCarcass(text, index),
                Skill_IceFishing iceFishing => ReplaceIceFishing(text, index),
                Skill_ClothingRepair mending => ReplaceMending(text, index),
                Skill_Archery archery => ReplaceArchery(text, index),
                Skill_Rifle rifle => ReplaceRifle(rifle, text, index),
                Skill_Revolver revolver => ReplaceRevolver(revolver, text, index),
                Skill_Gunsmithing gunsmithing => ReplaceGunsmithing(text, index),
                _ => text
            };
        }

        private static string ReplaceCooking(string text, int index)
        {
            return text
                .Replace("{cal-bonus}", "+" + CookingCalorieBonus[index] + "%")
                .Replace("{time-bonus}", CookingTimeReduction[index] + "%")
                .Replace("{ready-bonus}", CookingReadyTimeIncrease[index] + "%");
        }

        private static string ReplaceFirestarting(string text, int index)
        {
            return text
                .Replace("{roll-bonus}", FirestartingSuccessChance[index].ToString())
                .Replace("{duration-bonus}", FirestartingDurationIncrease[index] + "%")
                .Replace("{speed-bonus}", FirestartingTimeReduction[index] + "%");
        }

        private static string ReplaceCarcass(string text, int index)
        {
            return text
                .Replace("{duration-bonus}", CarcassMeatTimeReduction[index] + "%")
                .Replace("{meat-bonus}", CarcassMeatTimeReduction[index] + "%")
                .Replace("{hidegut-bonus}", CarcassHideGutTimeReduction[index] + "%")
                .Replace("{frozen-threshold}", CarcassBarehandedFrozenThreshold[index] + "%");
        }

        private static string ReplaceIceFishing(string text, int index)
        {
            return text
                .Replace("{chancebreak}", Number(IceFishingLineBreakChance[index]) + "%")
                .Replace("{time-bonus}", IceFishingTimeReduction[index] + "%")
                .Replace("{weight-bonus}", IceFishingWeightIncrease[index] + "%");
        }

        private static string ReplaceMending(string text, int index)
        {
            return text
                .Replace("{roll-bonus}", MendingSuccessChance[index].ToString())
                .Replace("{time-bonus}", MendingTimeReduction[index] + "%")
                .Replace("{cond-bonus}", MendingConditionIncrease[index] + "%")
                .Replace("{sewing-bonus}", MendingToolWearReduction[index] + "%");
        }

        private static string ReplaceArchery(string text, int index)
        {
            return text
                .Replace("{degrade-bonus}", ArcheryConditionWearReduction[index] + "%")
                .Replace("{crit-bonus}", ArcheryCriticalChanceIncrease[index] + "%")
                .Replace("{damage-bonus}", ArcheryDamageIncrease[index] + "%")
                .Replace("{sway-bonus}", ArcherySwayReduction[index] + "%")
                .Replace("{bleed-bonus}", ArcheryBleedTimeReduction[index] + "%");
        }

        private static string ReplaceRifle(Skill_Rifle skill, string text, int index)
        {
            return text
                .Replace("{degrade-bonus}", RifleConditionWearReduction[index] + "%")
                .Replace("{crit-bonus}", RifleCriticalChanceIncrease[index] + "%")
                .Replace("{repair-bonus}", RifleRepairBonus[index].ToString())
                .Replace("{accuracy-bonus}", RifleAccuracyRangeIncrease[index] + "%")
                .Replace("{damage-bonus}", RifleDamageIncrease[index] + "%")
                .Replace("{aim-bonus}", GetClosestAimAssistText(skill, RifleAimAssistAngle[index]));
        }

        private static string ReplaceRevolver(Skill_Revolver skill, string text, int index)
        {
            return text
                .Replace("{degrade-bonus}", RevolverConditionWearReduction[index] + "%")
                .Replace("{crit-bonus}", RevolverCriticalChanceIncrease[index] + "%")
                .Replace("{repair-bonus}", RevolverRepairBonus[index].ToString())
                .Replace("{recoil-bonus}", RevolverRecoilCompensation[index] + "%")
                .Replace("{damage-bonus}", RevolverDamageIncrease[index] + "%")
                .Replace("{struggle-bonus}", RevolverStruggleBonus[index] + "%")
                .Replace("{aim-bonus}", GetClosestAimAssistText(skill, RevolverAimAssistAngle[index]));
        }

        private static string ReplaceGunsmithing(string text, int index)
        {
            return text
                .Replace("{craft-condition}", GunsmithingAmmoCondition[index] + "%")
                .Replace("{harvest-success}", Number(GunsmithingHarvestSuccess[index]) + "%")
                .Replace("{milling-condition}", Number(GunsmithingMillingCondition[index]) + "%")
                .Replace("{milling-success}", Number(GunsmithingMillingSuccess[index]) + "%");
        }

        private static string GetClosestAimAssistText(Skill_Rifle skill, float value)
        {
            if (skill.m_AimAssistAngleDegrees == null || skill.m_AimAssistTierText == null) return Localization.Get("GAMEPLAY_None");

            int count = Math.Min(skill.m_AimAssistAngleDegrees.Length, skill.m_AimAssistTierText.Length);
            if (count <= 0) return Localization.Get("GAMEPLAY_None");

            int closest = 0;
            float closestDistance = Math.Abs(skill.m_AimAssistAngleDegrees[0] - value);
            for (int i = 1; i < count; i++)
            {
                float distance = Math.Abs(skill.m_AimAssistAngleDegrees[i] - value);
                if (distance < closestDistance || Math.Abs(distance - closestDistance) < 0.0001f && skill.m_AimAssistAngleDegrees[i] > skill.m_AimAssistAngleDegrees[closest])
                {
                    closest = i;
                    closestDistance = distance;
                }
            }

            return GetLocalizedAimAssist(skill.m_AimAssistTierText[closest]);
        }

        private static string GetClosestAimAssistText(Skill_Revolver skill, float value)
        {
            if (skill.m_AimAssistAngleDegrees == null || skill.m_AimAssistTierText == null) return Localization.Get("GAMEPLAY_None");

            int count = Math.Min(skill.m_AimAssistAngleDegrees.Length, skill.m_AimAssistTierText.Length);
            if (count <= 0) return Localization.Get("GAMEPLAY_None");

            int closest = 0;
            float closestDistance = Math.Abs(skill.m_AimAssistAngleDegrees[0] - value);
            for (int i = 1; i < count; i++)
            {
                float distance = Math.Abs(skill.m_AimAssistAngleDegrees[i] - value);
                if (distance < closestDistance || Math.Abs(distance - closestDistance) < 0.0001f && skill.m_AimAssistAngleDegrees[i] > skill.m_AimAssistAngleDegrees[closest])
                {
                    closest = i;
                    closestDistance = distance;
                }
            }

            return GetLocalizedAimAssist(skill.m_AimAssistTierText[closest]);
        }

        private static string GetLocalizedAimAssist(string localizationId)
        {
            string value = Localization.Get(localizationId);
            return string.IsNullOrEmpty(value) ? Localization.Get("GAMEPLAY_None") : value;
        }

        private static string Number(float value) => value.ToString("0.##");
    }
}