using ExpandedSkills.Framework;
using HarmonyLib;
using Il2Cpp;
using UnityEngine;

namespace ExpandedSkills.Patches
{
    [HarmonyPatch(typeof(Skill_Firestarting), nameof(Skill_Firestarting.TinderRequired))]
    internal static class FirestartingTinderRequiredPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill_Firestarting __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = !ExpandedSkillProgression.HasRewardLevel(__instance, 3);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_CarcassHarvesting), nameof(Skill_CarcassHarvesting.NoHarvestLossWhenDoingInitialHarvestingLoss))]
    internal static class CarcassInitialHarvestLossPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill_CarcassHarvesting __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillProgression.HasRewardLevel(__instance, 3);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Cooking), nameof(Skill_Cooking.NoCalorieLossWhenSmashingOpen))]
    internal static class CookingSmashingCanPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill_Cooking __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillProgression.HasRewardLevel(__instance, 3);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Cooking), nameof(Skill_Cooking.NoParasitesOrFoodPosioning))]
    internal static class CookingFoodSafetyPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill_Cooking __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillProgression.HasRewardLevel(__instance, 5);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Archery), nameof(Skill_Archery.CanFireBowWhileCrouched))]
    internal static class ArcheryCrouchedFirePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill_Archery __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillProgression.HasRewardLevel(__instance, 5);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Cooking), nameof(Skill_Cooking.GetCalorieScale))]
    internal static class CookingCaloriesPatch
    {
        private static bool Prefix(Skill_Cooking __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingCalorieBonus, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Cooking), nameof(Skill_Cooking.GetCookingTimeScale))]
    internal static class CookingTimePatch
    {
        private static bool Prefix(Skill_Cooking __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingTimeReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Cooking), nameof(Skill_Cooking.GetReadyTimeScale))]
    internal static class CookingReadyTimePatch
    {
        private static bool Prefix(Skill_Cooking __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingReadyTimeIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Cooking), nameof(Skill_Cooking.GetConditionLowConditionChance))]
    internal static class CookingLowConditionPatch
    {
        private static bool Prefix(Skill_Cooking __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingLowConditionChance, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Cooking), nameof(Skill_Cooking.GetConditionMaxScale))]
    internal static class CookingMaxConditionPatch
    {
        private static bool Prefix(Skill_Cooking __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CookingMaximumCondition, __instance) / 100f;
            return false;
        }
    }

    internal static class NativeFirestartingValueReadContext
    {
        [ThreadStatic]
        private static int _baseChanceReadDepth;

        internal static bool ReadingBaseChance => _baseChanceReadDepth > 0;

        internal static int ReadBaseChance(Skill_Firestarting skill)
        {
            if (skill == null) return 0;

            _baseChanceReadDepth++;
            try
            {
                return skill.GetBaseChanceSuccess();
            }
            finally
            {
                _baseChanceReadDepth = Math.Max(0, _baseChanceReadDepth - 1);
            }
        }
    }

    [HarmonyPatch(typeof(Skill_Firestarting), nameof(Skill_Firestarting.GetBaseChanceSuccess))]
    internal static class FirestartingSuccessPatch
    {
        private static bool Prefix(Skill_Firestarting __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (NativeFirestartingValueReadContext.ReadingBaseChance) return true;

            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingSuccessChance, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Firestarting), nameof(Skill_Firestarting.GetDurationScale))]
    internal static class FirestartingDurationPatch
    {
        private static bool Prefix(Skill_Firestarting __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingDurationIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Firestarting), nameof(Skill_Firestarting.GetStartTimeScale))]
    internal static class FirestartingTimePatch
    {
        private static bool Prefix(Skill_Firestarting __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.FirestartingTimeReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_CarcassHarvesting), nameof(Skill_CarcassHarvesting.GetFrozenThresholdPercent))]
    internal static class CarcassFrozenThresholdPatch
    {
        private static bool Prefix(Skill_CarcassHarvesting __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassBarehandedFrozenThreshold, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_CarcassHarvesting), nameof(Skill_CarcassHarvesting.GetBaseFrozenThresholdPercent))]
    internal static class CarcassBaseFrozenThresholdPatch
    {
        private static bool Prefix(ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 50;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_CarcassHarvesting), nameof(Skill_CarcassHarvesting.GetMeatHarvestTimeScale))]
    internal static class CarcassMeatTimePatch
    {
        private static bool Prefix(Skill_CarcassHarvesting __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassMeatTimeReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_CarcassHarvesting), nameof(Skill_CarcassHarvesting.GetHideGutHarvestTimeScale))]
    internal static class CarcassHideGutTimePatch
    {
        private static bool Prefix(Skill_CarcassHarvesting __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.CarcassHideGutTimeReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_IceFishing), nameof(Skill_IceFishing.GetLineBreakOnChancePercent))]
    internal static class IceFishingLineBreakPatch
    {
        private static bool Prefix(Skill_IceFishing __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.IceFishingLineBreakChance, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_IceFishing), nameof(Skill_IceFishing.GetBaseLineBreakOnChancePercent))]
    internal static class IceFishingBaseLineBreakPatch
    {
        private static bool Prefix(ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 12f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_IceFishing), nameof(Skill_IceFishing.ReduceFishingTimeScale))]
    internal static class IceFishingTimePatch
    {
        private static bool Prefix(Skill_IceFishing __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.IceFishingTimeReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_IceFishing), nameof(Skill_IceFishing.GetFishWeightScale))]
    internal static class IceFishingWeightPatch
    {
        private static bool Prefix(Skill_IceFishing __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.IceFishingWeightIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_ClothingRepair), nameof(Skill_ClothingRepair.GetBaseChanceSuccess))]
    internal static class MendingSuccessPatch
    {
        private static bool Prefix(Skill_ClothingRepair __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.MendingSuccessChance, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_ClothingRepair), nameof(Skill_ClothingRepair.GetRepairTimeScale))]
    internal static class MendingTimePatch
    {
        private static bool Prefix(Skill_ClothingRepair __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.MendingTimeReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_ClothingRepair), nameof(Skill_ClothingRepair.GetItemConditionScale))]
    internal static class MendingConditionPatch
    {
        private static bool Prefix(Skill_ClothingRepair __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.MendingConditionIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_ClothingRepair), nameof(Skill_ClothingRepair.GetSewingKitDegradeScale))]
    internal static class MendingSewingKitPatch
    {
        private static bool Prefix(Skill_ClothingRepair __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.MendingToolWearReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Archery), nameof(Skill_Archery.GetCriticalHitChanceScale))]
    internal static class ArcheryCritPatch
    {
        private static bool Prefix(Skill_Archery __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.ArcheryCriticalChanceIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Archery), nameof(Skill_Archery.GetArcheryDamageScale))]
    internal static class ArcheryDamagePatch
    {
        private static bool Prefix(Skill_Archery __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.ArcheryDamageIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Archery), nameof(Skill_Archery.GetConditionDegradeScale))]
    internal static class ArcheryConditionPatch
    {
        private static bool Prefix(Skill_Archery __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.ArcheryConditionWearReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Archery), nameof(Skill_Archery.GetSwayScale))]
    internal static class ArcherySwayPatch
    {
        private static bool Prefix(Skill_Archery __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(ExpandedSkillRewardData.Get(ExpandedSkillRewardData.ArcherySwayReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Archery), nameof(Skill_Archery.GetBleedOutTimeScale))]
    internal static class ArcheryBleedPatch
    {
        private static bool Prefix(Skill_Archery __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.ArcheryBleedTimeReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetCriticalHitChanceScale))]
    internal static class RifleCritPatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleCriticalChanceIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetConditionRepairBonus))]
    internal static class RifleRepairPatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleRepairBonus, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetAccuracyRangeScale))]
    internal static class RifleAccuracyPatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleAccuracyRangeIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetRifleDamageScale))]
    internal static class RifleDamagePatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleDamageIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetStabilityBonus))]
    internal static class RifleStabilityPatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleStabilityBonus, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetBestStabilityBonus))]
    internal static class RifleBestStabilityPatch
    {
        private static bool Prefix(ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.RifleStabilityBonus[ExpandedSkillProgression.MaxTierIndex] / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetEffectiveRange))]
    internal static class RifleRangePatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleEffectiveRange, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetConditionDegradeScale))]
    internal static class RifleConditionPatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleConditionWearReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Rifle), nameof(Skill_Rifle.GetAimAssistAngleDegrees))]
    internal static class RifleAimAssistPatch
    {
        private static bool Prefix(Skill_Rifle __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RifleAimAssistAngle, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Revolver), nameof(Skill_Revolver.GetCriticalHitChanceScale))]
    internal static class RevolverCritPatch
    {
        private static bool Prefix(Skill_Revolver __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RevolverCriticalChanceIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Revolver), nameof(Skill_Revolver.GetConditionRepairBonus))]
    internal static class RevolverRepairPatch
    {
        private static bool Prefix(Skill_Revolver __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RevolverRepairBonus, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Revolver), nameof(Skill_Revolver.GetRecoilScale))]
    internal static class RevolverRecoilPatch
    {
        private static bool Prefix(Skill_Revolver __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RevolverRecoilCompensation, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Revolver), nameof(Skill_Revolver.GetRevolverDamageScale))]
    internal static class RevolverDamagePatch
    {
        private static bool Prefix(Skill_Revolver __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = 1f + ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RevolverDamageIncrease, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Revolver), nameof(Skill_Revolver.GetConditionDegradeScale))]
    internal static class RevolverConditionPatch
    {
        private static bool Prefix(Skill_Revolver __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Mathf.Clamp01(1f - ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RevolverConditionWearReduction, __instance) / 100f);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Revolver), nameof(Skill_Revolver.GetAimAssistAngleDegrees))]
    internal static class RevolverAimAssistPatch
    {
        private static bool Prefix(Skill_Revolver __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RevolverAimAssistAngle, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Revolver), nameof(Skill_Revolver.GetStruggleBonus))]
    internal static class RevolverStrugglePatch
    {
        private static bool Prefix(Skill_Revolver __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.RevolverStruggleBonus, __instance) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Gunsmithing), nameof(Skill_Gunsmithing.GetAmmoCraftingCondition))]
    internal static class GunsmithingAmmoConditionPatch
    {
        private static bool Prefix(Skill_Gunsmithing __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingAmmoCondition, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Gunsmithing), nameof(Skill_Gunsmithing.GetMillingRepairCondition))]
    internal static class GunsmithingMillingConditionPatch
    {
        private static bool Prefix(Skill_Gunsmithing __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingMillingCondition, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Gunsmithing), nameof(Skill_Gunsmithing.GetMillingRepairSuccessChance))]
    internal static class GunsmithingMillingSuccessPatch
    {
        private static bool Prefix(Skill_Gunsmithing __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingMillingSuccess, __instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Gunsmithing), nameof(Skill_Gunsmithing.RollAmmoHarvestSuccess))]
    internal static class GunsmithingHarvestRollPatch
    {
        private static bool Prefix(Skill_Gunsmithing __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Utils.RollChance(ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingHarvestSuccess, __instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill_Gunsmithing), nameof(Skill_Gunsmithing.RollMillingRepairSuccess))]
    internal static class GunsmithingMillingRollPatch
    {
        private static bool Prefix(Skill_Gunsmithing __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            __result = Utils.RollChance(ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingMillingSuccess, __instance));
            return false;
        }
    }
}