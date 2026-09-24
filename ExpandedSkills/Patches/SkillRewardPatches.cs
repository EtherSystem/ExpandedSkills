using ExpandedSkills.Framework;
using Il2CppTLD.Gear;

namespace ExpandedSkills.Patches
{
    internal static class NativeFirestartingValueReadContext
    {
        internal static int ReadBaseChance(Skill_Firestarting skill)
        {
            return skill == null ? 0 : skill.GetBaseChanceSuccess();
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

    internal static class GunsmithingCraftingConditionContext
    {
        internal static BlueprintData ActiveBlueprint;
        internal static int Depth;
    }

    [HarmonyPatch(typeof(CraftingOperation), nameof(CraftingOperation.ApplyCraftingProgress), new Type[] { typeof(float) })]
    internal static class GunsmithingCraftingConditionContextPatch
    {
        private static void Prefix(CraftingOperation __instance, ref bool __state)
        {
            if (!Core.IsGameplayActive) return;

            BlueprintData blueprint = __instance?.Blueprint;
            __state = blueprint != null && blueprint.m_AppliedSkill == SkillType.Gunsmithing;
            if (!__state) return;

            if (GunsmithingCraftingConditionContext.Depth == 0) GunsmithingCraftingConditionContext.ActiveBlueprint = blueprint;
            GunsmithingCraftingConditionContext.Depth++;
        }

        private static Exception Finalizer(Exception __exception, bool __state)
        {
            if (!__state) return __exception;

            GunsmithingCraftingConditionContext.Depth = Math.Max(0, GunsmithingCraftingConditionContext.Depth - 1);
            if (GunsmithingCraftingConditionContext.Depth == 0) GunsmithingCraftingConditionContext.ActiveBlueprint = null;
            return __exception;
        }
    }

    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InstantiateCraftedItemInPlayerInventory))]
    internal static class GunsmithingAmmoConditionPatch
    {
        private static void Prefix(ref float __2)
        {
            if (!Core.IsGameplayActive) return;
            if (GunsmithingCraftingConditionContext.Depth <= 0) return;

            BlueprintData blueprint = GunsmithingCraftingConditionContext.ActiveBlueprint;
            if (blueprint == null || blueprint.m_AppliedSkill != SkillType.Gunsmithing) return;

            Skill_Gunsmithing skill = GameManager.GetSkillGunsmithing();
            if (skill == null) return;

            __2 = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingAmmoCondition, skill) / 100f;
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.CheckForHarvestSuccess))]
    internal static class GunsmithingHarvestSuccessPatch
    {
        [HarmonyPriority(Priority.Low)]
        private static bool Prefix(Harvest __0, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (__0 == null || __0.m_AppliedSkillType != SkillType.Gunsmithing) return true;

            Skill_Gunsmithing skill = GameManager.GetSkillGunsmithing();
            if (skill == null) return true;

            __result = Utils.RollChance(ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingHarvestSuccess, skill));
            return false;
        }
    }

    [HarmonyPatch(typeof(Panel_Milling), "DetermineConditionImprovement", new Type[] { typeof(SkillType) })]
    internal static class GunsmithingMillingConditionPatch
    {
        private static bool Prefix(SkillType __0, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (__0 != SkillType.Gunsmithing) return true;

            Skill_Gunsmithing skill = GameManager.GetSkillGunsmithing();
            if (skill == null) return true;

            __result = ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingMillingCondition, skill) / 100f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Panel_Milling), "RollForRepairSuccess", new Type[] { typeof(SkillType) })]
    internal static class GunsmithingMillingSuccessPatch
    {
        [HarmonyPriority(Priority.Low)]
        private static bool Prefix(SkillType __0, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (__0 != SkillType.Gunsmithing) return true;

            Skill_Gunsmithing skill = GameManager.GetSkillGunsmithing();
            if (skill == null) return true;

            __result = Utils.RollChance(ExpandedSkillRewardData.Get(ExpandedSkillRewardData.GunsmithingMillingSuccess, skill));
            return false;
        }
    }
}