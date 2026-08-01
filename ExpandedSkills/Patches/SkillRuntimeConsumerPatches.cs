using ExpandedSkills.Framework;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppTLD.Gear;
using Il2CppTLD.IntBackedUnit;

namespace ExpandedSkills.Patches
{
    internal static class FinalConsumerCorrection
    {
        internal static int RewardIndex(Skill skill)
        {
            return skill == null ? 0 : ExpandedSkillProgression.GetRewardTierIndex(skill);
        }

        internal static float GetReductionScale(Il2CppStructArray<int> values, int index)
        {
            if (values == null || values.Length == 0) return 1f;
            index = Mathf.Clamp(index, 0, values.Length - 1);
            return Mathf.Clamp01(1f - values[index] / 100f);
        }

        internal static float GetIncreaseScale(Il2CppStructArray<int> values, int index)
        {
            if (values == null || values.Length == 0) return 1f;
            index = Mathf.Clamp(index, 0, values.Length - 1);
            return 1f + values[index] / 100f;
        }

        internal static float DetectReductionScale(Il2CppStructArray<int> values, float observedScale, int fallbackIndex, float expandedScale)
        {
            return DetectScale(values, observedScale, fallbackIndex, expandedScale, true);
        }

        internal static float DetectIncreaseScale(Il2CppStructArray<int> values, float observedScale, int fallbackIndex, float expandedScale)
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

        private static float DetectScale(Il2CppStructArray<int> values, float observedScale, int fallbackIndex, float expandedScale, bool reduction)
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

            float skillScale = 1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingTimeReduction, skill) / 100f;
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

            float skillScale = 1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingTimeReduction, skill) / 100f;
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

    internal sealed class CookingFinalResultState
    {
        internal float CaloriesBeforeSkillAndPot;
    }

    [HarmonyPatch(typeof(CookingPotItem), "SetCookedGearProperties", new Type[] { typeof(GearItem), typeof(GearItem) })]
    internal static class CookingFinalResultConsumerPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Prefix(CookingPotItem __instance, GearItem __0, GearItem __1, ref CookingFinalResultState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__0 == null || __1?.m_FoodItem == null) return;

            float calories = __1.m_FoodItem.m_CaloriesTotal;
            if (__0.m_FoodWeight != null && __1.m_FoodWeight != null)
            {
                float rawMaximumWeight = __0.m_FoodWeight.m_MaxWeight.ToQuantity(1f);
                float cookedMaximumWeight = __1.m_FoodWeight.m_MaxWeight.ToQuantity(1f);
                if (rawMaximumWeight > 0.0001f)
                {
                    float rawWeightFraction = __0.GetItemWeightKG().ToQuantity(1f) / rawMaximumWeight;
                    calories = __1.m_FoodWeight.m_CaloriesPerKG * rawWeightFraction * cookedMaximumWeight;
                }
            }

            __state = new CookingFinalResultState
            {
                CaloriesBeforeSkillAndPot = calories * (__instance == null ? 1f : __instance.m_CookedCalorieMultiplier)
            };
        }

        [HarmonyPriority(Priority.First)]
        private static void Postfix(GearItem __0, GearItem __1, CookingFinalResultState __state)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Cooking skill = GameManager.GetSkillCooking();
            if (skill == null || __0 == null || __1 == null) return;

            int rewardIndex = FinalConsumerCorrection.RewardIndex(skill);
            float expandedCalorieScale = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingCalorieBonus, skill) / 100f;
            float observedCalorieScale = FinalConsumerCorrection.GetIncreaseScale(skill.m_CaloriePercentBonus, rewardIndex);
            float caloriesBefore = __1.m_FoodItem == null ? 0f : __1.m_FoodItem.m_CaloriesTotal;
            if (__1.m_FoodItem != null)
            {
                if (__state != null && __state.CaloriesBeforeSkillAndPot > 0.0001f)
                {
                    float observed = caloriesBefore / __state.CaloriesBeforeSkillAndPot;
                    observedCalorieScale = FinalConsumerCorrection.DetectIncreaseScale(skill.m_CaloriePercentBonus, observed, rewardIndex, expandedCalorieScale);
                }

                float ratio = observedCalorieScale <= 0.0001f ? 1f : expandedCalorieScale / observedCalorieScale;
                __1.m_FoodItem.m_CaloriesTotal *= ratio;
                __1.m_FoodItem.m_CaloriesRemaining *= ratio;
            }

            float conditionBefore = __1.GetNormalizedCondition();
            float configuredLowConditionChance = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingLowConditionChance, skill);
            float configuredMaximumCondition = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingMaximumCondition, skill) / 100f;
            bool lowConditionRoll = configuredLowConditionChance > 0f && Utils.RollChance(configuredLowConditionChance);
            float conditionAfter = lowConditionRoll
                ? UnityEngine.Random.Range(skill.m_LowConditionMin, skill.m_LowConditionMax)
                : Mathf.Min(Mathf.Clamp01(__0.GetNormalizedCondition() + 0.5f), configuredMaximumCondition);
            __1.SetNormalizedHP(Mathf.Clamp01(conditionAfter), false);

            SkillActionDiagnostics.LogAction(
                SkillType.Cooking,
                "CookingPotItem.SetCookedGearProperties",
                $"Configured ES calorie scale=x{SkillActionDiagnostics.Number(expandedCalorieScale)} | Consumer calorie scale actually observed=x{SkillActionDiagnostics.Number(observedCalorieScale)} | Calories result before ES correction={SkillActionDiagnostics.Number(caloriesBefore)} | Calories result after ES correction={SkillActionDiagnostics.Number(__1.m_FoodItem == null ? 0f : __1.m_FoodItem.m_CaloriesTotal)} | Configured ES low-condition chance={SkillActionDiagnostics.Percent(configuredLowConditionChance)} | Configured ES maximum condition={SkillActionDiagnostics.Percent(configuredMaximumCondition * 100f)} | Vanilla condition result before ES correction={SkillActionDiagnostics.Percent(conditionBefore * 100f)} | Condition result after ES correction={SkillActionDiagnostics.Condition(__1)} | ES low-condition roll={lowConditionRoll}");
        }
    }

    internal sealed class CookingSmashProtectionState
    {
        internal GearItem Gear;
        internal float Calories;
    }

    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.OnSmashComplete), new Type[] { typeof(bool), typeof(bool), typeof(float) })]
    internal static class CookingSmashProtectionPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Prefix(PlayerManager __instance, bool __0, bool __1, ref CookingSmashProtectionState __state)
        {
            if (!Core.IsGameplayActive) return;
            Skill_Cooking skill = GameManager.GetSkillCooking();
            GearItem gear = __instance?.m_SmashableItemUsed;
            if (!__0 || __1 || skill == null || ExpandedSkillProgression.GetRealLevel(skill) < 5 || gear?.m_FoodItem == null) return;
            __state = new CookingSmashProtectionState { Gear = gear, Calories = gear.m_FoodItem.m_CaloriesRemaining };
        }

        [HarmonyPriority(Priority.First)]
        private static void Postfix(CookingSmashProtectionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__state?.Gear?.m_FoodItem == null) return;
            float beforeCorrection = __state.Gear.m_FoodItem.m_CaloriesRemaining;
            __state.Gear.m_FoodItem.m_CaloriesRemaining = __state.Calories;
            SkillActionDiagnostics.LogAction(SkillType.Cooking, "PlayerManager.OnSmashComplete", $"Configured ES no-calorie-loss reward=True | Vanilla result before ES correction={SkillActionDiagnostics.Number(beforeCorrection)} calories | Result after ES correction={SkillActionDiagnostics.Number(__state.Calories)} calories");
        }
    }

    internal static class CookingFoodSafetyContext
    {
        private static int _depth;

        internal static bool Active => _depth > 0;

        internal static bool Begin(PlayerManager player, bool success, bool cancelled)
        {
            Skill_Cooking skill = GameManager.GetSkillCooking();
            if (!success || cancelled || player?.m_FoodItemEaten?.m_FoodItem == null || skill == null || ExpandedSkillProgression.GetRealLevel(skill) < 9) return false;
            _depth++;
            return true;
        }

        internal static Exception End(Exception exception, bool state)
        {
            if (state) _depth = Math.Max(0, _depth - 1);
            return exception;
        }
    }

    [HarmonyPatch(typeof(PlayerManager), "EatingComplete_Internal", new Type[] { typeof(bool), typeof(bool), typeof(float) })]
    internal static class CookingFoodSafetyContextPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Prefix(PlayerManager __instance, bool __0, bool __1, ref bool __state)
        {
            if (!Core.IsGameplayActive) return;
            __state = CookingFoodSafetyContext.Begin(__instance, __0, __1);
        }

        private static Exception Finalizer(Exception __exception, bool __state)
        {
            return CookingFoodSafetyContext.End(__exception, __state);
        }
    }

    [HarmonyPatch(typeof(GearItem), nameof(GearItem.RollForFoodPoisoning), new Type[] { typeof(float) })]
    internal static class CookingFoodPoisoningDecisionPatch
    {
        [HarmonyPriority(Priority.First)]
        private static bool Prefix(ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!CookingFoodSafetyContext.Active) return true;
            __result = false;
            SkillActionDiagnostics.LogAction(SkillType.Cooking, "GearItem.RollForFoodPoisoning", "Configured ES protection=True | Vanilla decision skipped before roll | Result after ES correction=False");
            return false;
        }
    }

    [HarmonyPatch(typeof(IntestinalParasites), nameof(IntestinalParasites.AddRiskPercent), new Type[] { typeof(Il2CppStructArray<float>), typeof(bool) })]
    internal static class CookingParasiteDecisionPatch
    {
        [HarmonyPriority(Priority.First)]
        private static bool Prefix(IntestinalParasites __instance)
        {
            if (!Core.IsGameplayActive) return true;
            if (!CookingFoodSafetyContext.Active) return true;
            float before = __instance == null ? 0f : __instance.m_CurrentInfectionChance;
            SkillActionDiagnostics.LogAction(SkillType.Cooking, "IntestinalParasites.AddRiskPercent", $"Configured ES protection=True | Vanilla risk application skipped | Risk before ES correction={SkillActionDiagnostics.Percent(before)} | Risk after ES correction={SkillActionDiagnostics.Percent(before)}");
            return false;
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

            float itemModifiers = __0.m_FireStartSkillModifier + __1.m_FireStartSkillModifier + (__2 == null ? 0f : __2.m_FireStartSkillModifier);
            int nativeContribution = NativeFirestartingValueReadContext.ReadBaseChance(skill);
            int expandedContribution = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingSuccessChance, skill);
            float before = __result;
            __result = Mathf.Clamp(before - nativeContribution + expandedContribution, 0f, 100f);
            SkillActionDiagnostics.LogSample(
                SkillType.Firestarting,
                "FireManager.CalculateFireStartSuccess",
                $"Configured ES skill contribution={expandedContribution}% | Vanilla skill contribution actually used={nativeContribution}% | Starter/fuel/accelerant item contributions present={SkillActionDiagnostics.Number(itemModifiers)}% | Result before ES correction={SkillActionDiagnostics.Percent(before)} | Result after ES correction={SkillActionDiagnostics.Percent(__result)} | Starter={__0.name} | Fuel={__1.name} | Accelerant={(__2 == null ? "none" : __2.name)}");
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

            int displayedChance = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingSuccessChance, skill);
            string expectedText = displayedChance + "%";
            if (__instance.m_Label_BaseSkill.text != expectedText) __instance.m_Label_BaseSkill.text = expectedText;
            SkillActionDiagnostics.LogSample(SkillType.Firestarting, "Panel_FireStart.Refresh", $"Base skill label forced after full panel refresh={expectedText}");
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

            float before = __result;
            __result = FinalConsumerCorrection.CorrectByScale(before, observedScale, expandedScale);
            SkillActionDiagnostics.LogSample(SkillType.Firestarting, "FuelSourceItem.GetModifiedBurnDurationHours", $"Configured ES duration scale=x{SkillActionDiagnostics.Number(expandedScale)} | Consumer duration scale actually observed=x{SkillActionDiagnostics.Number(observedScale)} | Result before ES correction={SkillActionDiagnostics.Number(before)} h | Result after ES correction={SkillActionDiagnostics.Number(__result)} h | Fuel={__instance.name} | Condition={SkillActionDiagnostics.Percent(__0 * 100f)}");
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
            float before = __result;
            if (before > minimumDuration + 0.0001f && unscaledDuration > 0.0001f)
            {
                float observed = Mathf.Max(0f, before - accelerantModifier) / unscaledDuration;
                observedScale = FinalConsumerCorrection.DetectReductionScale(skill.m_StartPercentIncrease, observed, rewardIndex, expandedScale);
                __result = FinalConsumerCorrection.CorrectByScale(Mathf.Max(0f, before - accelerantModifier), observedScale, expandedScale) + accelerantModifier;
                __result = Mathf.Max(__result, minimumDuration);
            }

            SkillActionDiagnostics.LogSample(SkillType.Firestarting, "FireManager.PlayerCalculateFireStartTime", $"Configured ES start-time scale=x{SkillActionDiagnostics.Number(expandedScale)} | Consumer start-time scale actually observed=x{SkillActionDiagnostics.Number(observedScale)} | Result before ES correction={SkillActionDiagnostics.Number(before)} s | Result after ES correction={SkillActionDiagnostics.Number(__result)} s | Accelerant modifier preserved={SkillActionDiagnostics.Number(accelerantModifier)} s | Minimum duration preserved={SkillActionDiagnostics.Number(minimumDuration)} s");
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
            SkillActionDiagnostics.LogAction(SkillType.Firestarting, "FireManager.PlayerHasMaterialsToStartFire", "Configured ES tinder requirement=False | Vanilla result before ES correction=False | Result after ES correction=True | Starter and fuel were both available");
        }
    }

    [HarmonyPatch(typeof(BodyHarvest), nameof(BodyHarvest.IsTooFrozenToHarvestWithBareHands))]
    internal static class CarcassFinalFrozenConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(BodyHarvest __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_CarcassHarvesting skill = GameManager.GetSkillCarcassHarvesting();
            if (__instance == null || skill == null || ExpandedSkillProgression.GetRealLevel(skill) < 5) return;

            int frozen = __instance.GetPercentFrozen();
            int threshold = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassBarehandedFrozenThreshold, skill);
            bool intactFullyFrozenRequiresTool = frozen >= 100 && !__instance.IsGearItem() && __instance.IsAFreshBody();
            __result = frozen > threshold || intactFullyFrozenRequiresTool;
            SkillActionDiagnostics.LogSample(
                SkillType.CarcassHarvesting,
                "BodyHarvest.IsTooFrozenToHarvestWithBareHands",
                $"Frozen={frozen}% | Fully frozen flag={__instance.m_Frozen} | ES threshold={threshold}% | Fresh intact 100% carcass requires tool={intactFullyFrozenRequiresTool} | Too frozen for bare hands={__result}");
        }
    }

    [HarmonyPatch(typeof(BodyHarvest), nameof(BodyHarvest.CanHarvestWithBareHands))]
    internal static class CarcassFinalBareHandsConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static bool Prefix(BodyHarvest __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            Skill_CarcassHarvesting skill = GameManager.GetSkillCarcassHarvesting();
            if (__instance == null || skill == null || ExpandedSkillProgression.GetRealLevel(skill) < 5) return true;

            int frozen = __instance.GetPercentFrozen();
            int threshold = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassBarehandedFrozenThreshold, skill);
            bool intactFullyFrozenRequiresTool = frozen >= 100 && !__instance.IsGearItem() && __instance.IsAFreshBody();
            bool resourcesAvailable = __instance.AreResourcesAvailable();
            __result = resourcesAvailable && frozen <= threshold && !intactFullyFrozenRequiresTool;

            SkillActionDiagnostics.LogSample(
                SkillType.CarcassHarvesting,
                "BodyHarvest.CanHarvestWithBareHands",
                $"Frozen={frozen}% | Fully frozen flag={__instance.m_Frozen} | ES threshold={threshold}% | Resources available={resourcesAvailable} | Fresh intact 100% carcass requires tool={intactFullyFrozenRequiresTool} | Bare hands allowed={__result}");
            return false;
        }
    }

    [HarmonyPatch(typeof(Panel_BodyHarvest), nameof(Panel_BodyHarvest.GetHarvestDurationMinutes))]
    internal static class CarcassFinalDurationConsumerPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Postfix(Panel_BodyHarvest __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return;
            Skill_CarcassHarvesting skill = GameManager.GetSkillCarcassHarvesting();
            BodyHarvest harvest = __instance?.m_BodyHarvest;
            if (__instance == null || __instance.m_Settings == null || harvest == null || skill == null) return;

            GearItem tool = null;
            if (__instance.m_Tools != null && __instance.m_Tools.Count > 0)
            {
                int index = Mathf.Clamp(__instance.m_SelectedToolItemIndex, 0, __instance.m_Tools.Count - 1);
                tool = __instance.m_Tools[index];
            }

            float meatRate = __instance.m_Settings.m_HarvestMeatMinutesPerKG;
            float frozenMeatRate = __instance.m_Settings.m_HarvestFrozenMeatMinutesPerKG;
            float hideRate = __instance.m_Settings.m_HarvestHideMinutesPerUnit;
            float gutRate = __instance.m_Settings.m_HarvestGutMinutesPerUnit;
            if (tool?.m_BodyHarvestItem != null)
            {
                meatRate = tool.m_BodyHarvestItem.m_HarvestMeatMinutesPerKG;
                frozenMeatRate = tool.m_BodyHarvestItem.m_HarvestFrozenMeatMinutesPerKG;
                hideRate = tool.m_BodyHarvestItem.m_HarvestHideMinutesPerUnit;
                gutRate = tool.m_BodyHarvestItem.m_HarvestGutMinutesPerUnit;
            }

            float meatAmount = __instance.m_MenuItem_Meat == null ? 0f : __instance.m_MenuItem_Meat.HarvestAmount.ToQuantity(1f);
            int hideAmount = __instance.m_MenuItem_Hide == null ? 0 : __instance.m_MenuItem_Hide.HarvestUnits;
            int gutAmount = __instance.m_MenuItem_Gut == null ? 0 : __instance.m_MenuItem_Gut.HarvestUnits;
            float meatScale = 1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassMeatTimeReduction, skill) / 100f;
            float hideGutScale = 1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassHideGutTimeReduction, skill) / 100f;
            int frozen = harvest.GetPercentFrozen();
            int frozenThreshold = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassBarehandedFrozenThreshold, skill);
            bool intactFullyFrozenRequiresTool = frozen >= 100 && !harvest.IsGearItem() && harvest.IsAFreshBody();
            bool useFrozenMeatRate = frozen > frozenThreshold || intactFullyFrozenRequiresTool;
            float selectedMeatRate = useFrozenMeatRate ? frozenMeatRate : meatRate;

            __result = Mathf.Max(0f,
                meatAmount * selectedMeatRate * meatScale
                + hideAmount * hideRate * hideGutScale
                + gutAmount * gutRate * hideGutScale);

            SkillActionDiagnostics.LogSample(
                SkillType.CarcassHarvesting,
                "Panel_BodyHarvest.GetHarvestDurationMinutes",
                $"Meat={SkillActionDiagnostics.Number(meatAmount)} kg | Hide={hideAmount} | Gut={gutAmount} | Tool={SkillActionDiagnostics.GearName(tool)} | Frozen={frozen}% | Fully frozen flag={harvest.m_Frozen} | ES threshold={frozenThreshold}% | Fresh intact 100% carcass requires tool={intactFullyFrozenRequiresTool} | Meat rate={(useFrozenMeatRate ? "frozen" : "normal")} | Meat scale=x{SkillActionDiagnostics.Number(meatScale)} | Hide/gut scale=x{SkillActionDiagnostics.Number(hideGutScale)} | Final duration={SkillActionDiagnostics.Number(__result)} min");
        }
    }

}