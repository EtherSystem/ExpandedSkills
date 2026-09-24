using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace ExpandedSkills.Framework
{
    internal static class ExpandedSkillRewardData
    {
        private static int[] s_CookingLowConditionChance;
        private static int[] s_CookingConditionMax;
        internal static readonly int[] CookingCalorieBonus = { 0, 5, 10, 12, 15, 18, 20, 22, 25, 25 };
        internal static readonly int[] CookingTimeReduction = { 0, 0, 0, 5, 10, 15, 20, 25, 30, 30 };
        internal static readonly int[] CookingReadyTimeIncrease = { 0, 0, 0, 0, 0, 10, 20, 20, 20, 20 };

        internal static readonly int[] FirestartingSuccessChance = { 40, 48, 55, 60, 65, 70, 75, 82, 90, 90 };
        internal static readonly int[] FirestartingDurationIncrease = { 0, 5, 10, 10, 10, 18, 25, 38, 50, 50 };
        internal static readonly int[] FirestartingTimeReduction = { 0, 0, 0, 0, 0, 0, 0, 25, 50, 50 };

        internal static readonly int[] CarcassMeatTimeReduction = { 0, 5, 10, 18, 25, 28, 30, 40, 50, 50 };
        internal static readonly int[] CarcassHideGutTimeReduction = { 0, 0, 0, 5, 10, 15, 20, 25, 30, 30 };
        internal static readonly int[] CarcassFrozenThreshold = { 50, 50, 50, 50, 50, 60, 75, 90, 100, 100 };

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


        internal static void ApplyNativeRuntimeData(Skill skill)
        {
            if (skill == null) return;

            int levelIndex = GetLevelIndex(skill);

            switch (skill)
            {
                case Skill_Cooking cooking:
                    EnsureCookingConditionBaseline(cooking);
                    int conditionTier = ExpandedSkillProgression.GetRewardTierIndex(cooking);
                    cooking.m_LevelWhereNoCalorieLossFromSmashing = NativeThreshold(ExpandedSkillProgression.HasRewardLevel(cooking, 3));
                    cooking.m_LevelWhereNoParasitesOrFoodPoisoning = NativeThreshold(ExpandedSkillProgression.HasRewardLevel(cooking, 5));
                    Fill(cooking.m_CaloriePercentBonus, CookingCalorieBonus[levelIndex]);
                    Fill(cooking.m_CookingTimeReducePercent, CookingTimeReduction[levelIndex]);
                    Fill(cooking.m_ReadyTimeIncreasePercent, CookingReadyTimeIncrease[levelIndex]);
                    Fill(cooking.m_LowConditionChance, s_CookingLowConditionChance[conditionTier]);
                    Fill(cooking.m_ConditionMax, s_CookingConditionMax[conditionTier]);
                    break;
                case Skill_Firestarting firestarting:
                    firestarting.m_LevelWhereTinderNotRequired = NativeThreshold(ExpandedSkillProgression.HasRewardLevel(firestarting, 3));
                    Fill(firestarting.m_BaseSuccessChance, FirestartingSuccessChance[levelIndex]);
                    Fill(firestarting.m_DurationPercentIncrease, FirestartingDurationIncrease[levelIndex]);
                    Fill(firestarting.m_StartPercentIncrease, FirestartingTimeReduction[levelIndex]);
                    break;
                case Skill_CarcassHarvesting carcass:
                    carcass.m_LevelWhereNoInitialBarehandedHarvestLoss = NativeThreshold(ExpandedSkillProgression.HasRewardLevel(carcass, 3));
                    Fill(carcass.m_FrozenThresholdPercent, CarcassFrozenThreshold[levelIndex]);
                    Fill(carcass.m_MeatTimePercentDecrease, CarcassMeatTimeReduction[levelIndex]);
                    Fill(carcass.m_HideGutTimePercentDecrease, CarcassHideGutTimeReduction[levelIndex]);
                    break;
                case Skill_IceFishing iceFishing:
                    Fill(iceFishing.m_LineBreakOnCatchChance, Mathf.RoundToInt(IceFishingLineBreakChance[levelIndex]));
                    Fill(iceFishing.m_ReduceFishingTimePercent, IceFishingTimeReduction[levelIndex]);
                    Fill(iceFishing.m_IncreaseFishWeightPercent, IceFishingWeightIncrease[levelIndex]);
                    break;
                case Skill_ClothingRepair mending:
                    Fill(mending.m_BaseSuccessChance, MendingSuccessChance[levelIndex]);
                    Fill(mending.m_RepairTimePercentDecrease, MendingTimeReduction[levelIndex]);
                    Fill(mending.m_ItemConditionPercentIncrease, MendingConditionIncrease[levelIndex]);
                    Fill(mending.m_SewingToolDegradeDecrease, MendingToolWearReduction[levelIndex]);
                    break;
                case Skill_Archery archery:
                    archery.m_LevelWhereCanFireFromCrouch = NativeThreshold(ExpandedSkillProgression.HasRewardLevel(archery, 5));
                    Fill(archery.m_SwayReduction, ArcherySwayReduction[levelIndex]);
                    Fill(archery.m_DamageIncrease, ArcheryDamageIncrease[levelIndex]);
                    Fill(archery.m_CriticalHitChanceIncrease, ArcheryCriticalChanceIncrease[levelIndex]);
                    Fill(archery.m_BleedOutTimeReduction, ArcheryBleedTimeReduction[levelIndex]);
                    Fill(archery.m_ConditionDegradeOnUseReduction, ArcheryConditionWearReduction[levelIndex]);
                    break;
                case Skill_Rifle rifle:
                    Fill(rifle.m_CriticalHitChanceIncrease, RifleCriticalChanceIncrease[levelIndex]);
                    Fill(rifle.m_ConditionRepairBonus, RifleRepairBonus[levelIndex]);
                    Fill(rifle.m_AccuracyRangeIncrease, RifleAccuracyRangeIncrease[levelIndex]);
                    Fill(rifle.m_DamageIncrease, RifleDamageIncrease[levelIndex]);
                    Fill(rifle.m_StabilityBonus, RifleStabilityBonus[levelIndex]);
                    Fill(rifle.m_EffectiveRange, Mathf.RoundToInt(RifleEffectiveRange[levelIndex]));
                    Fill(rifle.m_ConditionDegradeOnUseReduction, RifleConditionWearReduction[levelIndex]);
                    Fill(rifle.m_AimAssistAngleDegrees, RifleAimAssistAngle[levelIndex]);
                    break;
                case Skill_Revolver revolver:
                    Fill(revolver.m_CriticalHitChanceIncrease, RevolverCriticalChanceIncrease[levelIndex]);
                    Fill(revolver.m_ConditionRepairBonus, RevolverRepairBonus[levelIndex]);
                    Fill(revolver.m_RecoilCompensation, RevolverRecoilCompensation[levelIndex]);
                    Fill(revolver.m_DamageIncrease, RevolverDamageIncrease[levelIndex]);
                    Fill(revolver.m_ConditionDegradeOnUseReduction, RevolverConditionWearReduction[levelIndex]);
                    Fill(revolver.m_AimAssistAngleDegrees, RevolverAimAssistAngle[levelIndex]);
                    Fill(revolver.m_StruggleBonus, RevolverStruggleBonus[levelIndex]);
                    break;
            }
        }

        private static void EnsureCookingConditionBaseline(Skill_Cooking cooking)
        {
            if (s_CookingLowConditionChance != null && s_CookingConditionMax != null) return;

            s_CookingLowConditionChance = ReadFive(cooking.m_LowConditionChance);
            s_CookingConditionMax = ReadFive(cooking.m_ConditionMax);
        }

        private static int[] ReadFive(Il2CppStructArray<int> values)
        {
            int[] result = new int[5];
            if (values == null || values.Length == 0) return result;

            int count = Math.Min(5, values.Length);
            for (int i = 0; i < count; i++) result[i] = values[i];
            for (int i = count; i < result.Length; i++) result[i] = result[Math.Max(0, count - 1)];
            return result;
        }

        private static int NativeThreshold(bool active) => active ? 1 : 6;

        private static void Fill(Il2CppStructArray<int> values, int value)
        {
            if (values == null) return;
            for (int i = 0; i < values.Length; i++) values[i] = value;
        }

        private static void Fill(Il2CppStructArray<float> values, float value)
        {
            if (values == null) return;
            for (int i = 0; i < values.Length; i++) values[i] = value;
        }

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
                if (lines[i].Contains(token, StringComparison.OrdinalIgnoreCase)) lines.RemoveAt(i);
            }
        }

        private static string ReplaceTokens(Skill skill, int index, string text)
        {
            return skill.m_SkillType switch
            {
                SkillType.Cooking => ReplaceCooking(text, index),
                SkillType.Firestarting => ReplaceFirestarting(text, index),
                SkillType.CarcassHarvesting => ReplaceCarcass(text, index),
                SkillType.IceFishing => ReplaceIceFishing(text, index),
                SkillType.ClothingRepair => ReplaceMending(text, index),
                SkillType.Archery => ReplaceArchery(text, index),
                SkillType.Rifle => ReplaceRifle(text, index),
                SkillType.Revolver => ReplaceRevolver(text, index),
                SkillType.Gunsmithing => ReplaceGunsmithing(text, index),
                _ => text
            };
        }

        private static string ReplaceCooking(string text, int index)
        {
            return text
                .Replace("{cal-bonus}", "+" + CookingCalorieBonus[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{time-bonus}", CookingTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{ready-bonus}", CookingReadyTimeIncrease[index] + "%", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceFirestarting(string text, int index)
        {
            return text
                .Replace("{roll-bonus}", FirestartingSuccessChance[index].ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{duration-bonus}", FirestartingDurationIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{speed-bonus}", FirestartingTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceCarcass(string text, int index)
        {
            return text
                .Replace("{duration-bonus}", CarcassMeatTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{meat-bonus}", CarcassMeatTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{hidegut-bonus}", CarcassHideGutTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{frozen-threshold}", CarcassFrozenThreshold[index] + "%", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceIceFishing(string text, int index)
        {
            return text
                .Replace("{chancebreak}", Number(IceFishingLineBreakChance[index]) + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{time-bonus}", IceFishingTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{weight-bonus}", IceFishingWeightIncrease[index] + "%", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceMending(string text, int index)
        {
            return text
                .Replace("{roll-bonus}", MendingSuccessChance[index].ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{time-bonus}", MendingTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{cond-bonus}", MendingConditionIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{sewing-bonus}", MendingToolWearReduction[index] + "%", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceArchery(string text, int index)
        {
            return text
                .Replace("{degrade-bonus}", ArcheryConditionWearReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{crit-bonus}", ArcheryCriticalChanceIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{damage-bonus}", ArcheryDamageIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{sway-bonus}", ArcherySwayReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{bleed-bonus}", ArcheryBleedTimeReduction[index] + "%", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceRifle(string text, int index)
        {
            return text
                .Replace("{degrade-bonus}", RifleConditionWearReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{crit-bonus}", RifleCriticalChanceIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{repair-bonus}", RifleRepairBonus[index].ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{accuracy-bonus}", RifleAccuracyRangeIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{damage-bonus}", RifleDamageIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{aim-bonus}", GetAimAssistText(RifleAimAssistAngle[index]), StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceRevolver(string text, int index)
        {
            return text
                .Replace("{degrade-bonus}", RevolverConditionWearReduction[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{crit-bonus}", RevolverCriticalChanceIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{repair-bonus}", RevolverRepairBonus[index].ToString(), StringComparison.OrdinalIgnoreCase)
                .Replace("{recoil-bonus}", RevolverRecoilCompensation[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{damage-bonus}", RevolverDamageIncrease[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{struggle-bonus}", RevolverStruggleBonus[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{aim-bonus}", GetAimAssistText(RevolverAimAssistAngle[index]), StringComparison.OrdinalIgnoreCase);
        }

        private static string ReplaceGunsmithing(string text, int index)
        {
            return text
                .Replace("{craft-condition}", GunsmithingAmmoCondition[index] + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{harvest-success}", Number(GunsmithingHarvestSuccess[index]) + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{milling-condition}", Number(GunsmithingMillingCondition[index]) + "%", StringComparison.OrdinalIgnoreCase)
                .Replace("{milling-success}", Number(GunsmithingMillingSuccess[index]) + "%", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetAimAssistText(float value)
        {
            return value <= 0f ? Localization.Get("GAMEPLAY_None") : Number(value) + "°";
        }

        private static string Number(float value) => value.ToString("0.##");
    }
}