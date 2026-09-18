using ExpandedSkills.Framework;

namespace ExpandedSkills.Patches
{
    internal static class FinalConsumerCorrection
    {
        internal static int RewardIndex(Skill skill)
        {
            return skill == null ? 0 : ExpandedSkillProgression.GetRewardTierIndex(skill);
        }

        internal static float GetReductionScale(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<int> values, int index)
        {
            if (values == null || values.Length == 0) return 1f;
            index = Mathf.Clamp(index, 0, values.Length - 1);
            return Mathf.Clamp01(1f - values[index] / 100f);
        }

        internal static float GetIncreaseScale(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<int> values, int index)
        {
            if (values == null || values.Length == 0) return 1f;
            index = Mathf.Clamp(index, 0, values.Length - 1);
            return 1f + values[index] / 100f;
        }

        internal static float DetectReductionScale(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<int> values, float observedScale, int fallbackIndex, float expandedScale)
        {
            return DetectScale(values, observedScale, fallbackIndex, expandedScale, true);
        }

        internal static float DetectIncreaseScale(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<int> values, float observedScale, int fallbackIndex, float expandedScale)
        {
            return DetectScale(values, observedScale, fallbackIndex, expandedScale, false);
        }

        internal static float CorrectByScale(float value, float observedScale, float expandedScale)
        {
            return observedScale <= 0.0001f ? value : value * expandedScale / observedScale;
        }

        internal static bool Approximately(float first, float second)
        {
            return Mathf.Abs(first - second) <= 0.0025f;
        }

        private static float DetectScale(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<int> values, float observedScale, int fallbackIndex, float expandedScale, bool reduction)
        {
            if (observedScale <= 0f) return 1f;
            if (Approximately(observedScale, expandedScale)) return expandedScale;
            if (values == null || values.Length == 0) return observedScale;

            int count = Math.Min(5, values.Length);
            float fallback = reduction
                ? Mathf.Clamp01(1f - values[Mathf.Clamp(fallbackIndex, 0, count - 1)] / 100f)
                : 1f + values[Mathf.Clamp(fallbackIndex, 0, count - 1)] / 100f;
            float closest = fallback;
            float closestDistance = Mathf.Abs(observedScale - closest);
            for (int i = 0; i < count; i++)
            {
                float candidate = reduction ? Mathf.Clamp01(1f - values[i] / 100f) : 1f + values[i] / 100f;
                float distance = Mathf.Abs(observedScale - candidate);
                if (distance >= closestDistance) continue;
                closest = candidate;
                closestDistance = distance;
            }

            return closest;
        }
    }

    [HarmonyPatch(typeof(CookingPotItem), nameof(CookingPotItem.GetTotalCookMultiplier))]
    internal static class CookingFinalCookMultiplierConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(CookingPotItem __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Cooking skill = GameManager.GetSkillCooking();
            if (__instance == null || skill == null) return;

            float skillScale = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingTimeReduction, skill) / 100f);
            __result = __instance.m_CookingTimeMultiplier * skillScale;
            SkillActionDiagnostics.LogSample(
                SkillType.Cooking,
                "CookingPotItem.GetTotalCookMultiplier",
                $"Surface multiplier=x{SkillActionDiagnostics.Number(__instance.m_CookingTimeMultiplier)} | Configured ES skill scale=x{SkillActionDiagnostics.Number(skillScale)} | Final total cook multiplier=x{SkillActionDiagnostics.Number(__result)}");
        }
    }

    [HarmonyPatch(typeof(CookingPotItem), nameof(CookingPotItem.GetTotalBoilMultiplier))]
    internal static class CookingFinalBoilMultiplierConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(CookingPotItem __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Cooking skill = GameManager.GetSkillCooking();
            if (__instance == null || skill == null) return;

            float skillScale = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingTimeReduction, skill) / 100f);
            __result = __instance.m_BoilingTimeMultiplier * skillScale;
            SkillActionDiagnostics.LogSample(
                SkillType.Cooking,
                "CookingPotItem.GetTotalBoilMultiplier",
                $"Surface multiplier=x{SkillActionDiagnostics.Number(__instance.m_BoilingTimeMultiplier)} | Configured ES skill scale=x{SkillActionDiagnostics.Number(skillScale)} | Final total boil multiplier=x{SkillActionDiagnostics.Number(__result)}");
        }
    }

    [HarmonyPatch(typeof(CookingPotItem), nameof(CookingPotItem.GetTotalReadyMultiplier))]
    internal static class CookingFinalReadyMultiplierConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(CookingPotItem __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Cooking skill = GameManager.GetSkillCooking();
            if (__instance == null || skill == null) return;

            float skillScale = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingReadyTimeIncrease, skill) / 100f;
            __result = __instance.m_ReadyTimeMultiplier * skillScale;
            SkillActionDiagnostics.LogSample(
                SkillType.Cooking,
                "CookingPotItem.GetTotalReadyMultiplier",
                $"Surface multiplier=x{SkillActionDiagnostics.Number(__instance.m_ReadyTimeMultiplier)} | Configured ES skill scale=x{SkillActionDiagnostics.Number(skillScale)} | Final total ready multiplier=x{SkillActionDiagnostics.Number(__result)}");
        }
    }

    [HarmonyPatch(typeof(FireManager), nameof(FireManager.CalculateFireStartSuccess))]
    internal static class FireFinalSuccessConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(FireStarterItem __0, FuelSourceItem __1, FireStarterItem __2, ref float __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Firestarting skill = GameManager.GetSkillFireStarting();
            if (skill == null || __0 == null || __1 == null) return;

            int nativeContribution = NativeFirestartingValueReadContext.ReadBaseChance(skill);
            int expandedContribution = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingSuccessChance, skill);
            __result = Mathf.Clamp(__result - nativeContribution + expandedContribution, 0f, 100f);
        }
    }

    [HarmonyPatch(typeof(Panel_FireStart), nameof(Panel_FireStart.Refresh))]
    internal static class FirestartingBaseSkillDisplayPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(Panel_FireStart __instance)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Firestarting skill = GameManager.GetSkillFireStarting();
            if (__instance == null || __instance.m_Label_BaseSkill == null || skill == null) return;

            string expectedText = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingSuccessChance, skill) + "%";
            if (__instance.m_Label_BaseSkill.text != expectedText) __instance.m_Label_BaseSkill.text = expectedText;
        }
    }

    [HarmonyPatch(typeof(FuelSourceItem), nameof(FuelSourceItem.GetModifiedBurnDurationHours))]
    internal static class FireFinalDurationConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(FuelSourceItem __instance, float __0, ref float __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Firestarting skill = GameManager.GetSkillFireStarting();
            if (__instance == null || skill == null) return;

            int rewardIndex = FinalConsumerCorrection.RewardIndex(skill);
            float expandedScale = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingDurationIncrease, skill) / 100f;
            float observedScale = FinalConsumerCorrection.GetIncreaseScale(skill.m_DurationPercentIncrease, rewardIndex);
            float baseDuration = __instance.m_BurnDurationHours * __0;
            if (baseDuration > 0.0001f)
            {
                float observed = __result / baseDuration;
                observedScale = FinalConsumerCorrection.DetectIncreaseScale(skill.m_DurationPercentIncrease, observed, rewardIndex, expandedScale);
            }

            __result = FinalConsumerCorrection.CorrectByScale(__result, observedScale, expandedScale);
        }
    }

    [HarmonyPatch(typeof(FireManager), nameof(FireManager.PlayerCalculateFireStartTime))]
    internal static class FireFinalStartTimeConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(FireManager __instance, FireStarterItem __0, FuelSourceItem __1, FireStarterItem __2, ref float __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Firestarting skill = GameManager.GetSkillFireStarting();
            if (__instance == null || __0 == null || __1 == null || skill == null) return;

            int rewardIndex = FinalConsumerCorrection.RewardIndex(skill);
            float expandedScale = 1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingTimeReduction, skill) / 100f;
            float observedScale = FinalConsumerCorrection.GetReductionScale(skill.m_StartPercentIncrease, rewardIndex);
            float accelerantModifier = __2 == null ? 0f : __2.m_FireStartDurationModifier;
            float unscaledDuration = (__1.m_IsWet ? __instance.m_StartFireTimeSecondsWet : __instance.m_StartFireTimeSeconds) + __0.m_SecondsToIgniteTinder;
            float minimumDuration = __0.m_SecondsToIgniteTinder + 0.5f;
            if (__result > minimumDuration + 0.0001f && unscaledDuration > 0.0001f)
            {
                float observed = Mathf.Max(0f, __result - accelerantModifier) / unscaledDuration;
                observedScale = FinalConsumerCorrection.DetectReductionScale(skill.m_StartPercentIncrease, observed, rewardIndex, expandedScale);
                __result = FinalConsumerCorrection.CorrectByScale(Mathf.Max(0f, __result - accelerantModifier), observedScale, expandedScale) + accelerantModifier;
                __result = Mathf.Max(__result, minimumDuration);
            }
        }
    }

    [HarmonyPatch(typeof(FireManager), nameof(FireManager.PlayerHasMaterialsToStartFire))]
    internal static class FireFinalTinderConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(FireManager __instance, ref bool __result, ref FireStarterItem __0, ref FuelSourceItem __1, ref FuelSourceItem __2, ref FireStarterItem __3)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Firestarting skill = GameManager.GetSkillFireStarting();
            if (__result || __instance == null || skill == null || ExpandedSkillProgression.GetRealLevel(skill) < 5) return;

            FireStarterItem starter = __instance.PlayerGetFirestarterChoice();
            FuelSourceItem fuel = __instance.PlayerGetFuelChoice();
            if (starter == null || fuel == null) return;

            __0 = starter;
            __1 = null;
            __2 = fuel;
            __3 = __instance.PlayerGetAccelerantChoice();
            __result = true;
        }
    }
}