using ExpandedSkills.Framework;
using Il2CppTLD.IntBackedUnit;
using Il2CppTLD.Gear;

namespace ExpandedSkills.Patches
{
    [HarmonyPatch(typeof(Panel_BodyHarvest), nameof(Panel_BodyHarvest.ShouldApplyInitialBarehandedHarvestLoss))]
    internal static class CarcassActionInitialLossPatch
    {
        private static void Postfix(Panel_BodyHarvest __instance, bool __result)
        {
            if (!Core.IsGameplayActive) return;
            BodyHarvest harvest = __instance?.m_BodyHarvest;
            float meat = harvest == null ? 0f : harvest.m_MeatAvailableKG.ToQuantity(1f);
            SkillActionDiagnostics.LogSample(SkillType.CarcassHarvesting, "Panel_BodyHarvest.ShouldApplyInitialBarehandedHarvestLoss", $"Meat currently available={SkillActionDiagnostics.Number(meat)} kg | Actual initial loss will apply={__result}");
        }
    }

    [HarmonyPatch(typeof(IceFishingHole), nameof(IceFishingHole.StartActiveFishing))]
    internal static class FishingActionTimePatch
    {
        private static void Prefix(float __0, float __1)
        {
            if (!Core.IsGameplayActive) return;
            SkillActionDiagnostics.LogAction(SkillType.IceFishing, "IceFishingHole.StartActiveFishing", $"Final real-time progress duration={SkillActionDiagnostics.Number(__0)} s | Final in-game fishing duration={SkillActionDiagnostics.Number(__1)} min");
        }
    }

    [HarmonyPatch(typeof(IceFishingHole), nameof(IceFishingHole.InstantiateFish))]
    internal static class FishingActionFishWeightPatch
    {
        private static void Postfix(GearItem __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__result == null) return;
            float calories = __result.m_FoodItem == null ? 0f : __result.m_FoodItem.m_CaloriesRemaining;
            SkillActionDiagnostics.LogAction(SkillType.IceFishing, "IceFishingHole.InstantiateFish", $"Fish={SkillActionDiagnostics.GearName(__result)} | Final weight={SkillActionDiagnostics.Weight(__result)} | Final calories={SkillActionDiagnostics.Number(calories)}");
        }
    }

    internal sealed class FishingCompletionActionState
    {
        internal GearItem Tackle;
        internal float TackleCondition;
        internal bool HadCaughtFish;
        internal bool ChanceContextActive;
    }

    [HarmonyPatch(typeof(IceFishingHole), nameof(IceFishingHole.OnActiveFishingComplete))]
    internal static class FishingActionCompletionPatch
    {
        private static void Prefix(IceFishingHole __instance, ref FishingCompletionActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (!SkillActionDiagnostics.Enabled || __instance == null) return;
            GearItem tackle = __instance.m_SelectedTackle;
            __state = new FishingCompletionActionState
            {
                Tackle = tackle,
                TackleCondition = tackle == null ? -1f : tackle.GetNormalizedCondition(),
                HadCaughtFish = __instance.HasCaughtFish,
                ChanceContextActive = SkillActionDiagnostics.BeginChanceContext(SkillType.IceFishing, "IceFishingHole.OnActiveFishingComplete")
            };
        }

        private static void Postfix(IceFishingHole __instance, bool __0, bool __1, float __2, FishingCompletionActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__instance == null || __state == null) return;
            GearItem afterTackle = __instance.m_SelectedTackle;
            string before = __state.Tackle == null ? "none" : SkillActionDiagnostics.Percent(__state.TackleCondition * 100f);
            string after = afterTackle == null ? "missing/consumed" : SkillActionDiagnostics.Condition(afterTackle);
            bool lineLost = __state.Tackle != null && afterTackle == null;
            SkillActionDiagnostics.LogAction(SkillType.IceFishing, "IceFishingHole.OnActiveFishingComplete", $"Operation success={__0} | Cancelled={__1} | Progress={SkillActionDiagnostics.Percent(__2 * 100f)} | Caught fish={__instance.HasCaughtFish || __state.HadCaughtFish} | Tackle condition={before} -> {after} | Line/tackle lost={lineLost}");
        }

        private static Exception Finalizer(Exception __exception, FishingCompletionActionState __state)
        {
            if (__state != null) SkillActionDiagnostics.EndChanceContext(__state.ChanceContextActive);
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Repairable), nameof(Repairable.GetChanceActionSuccess))]
    internal static class MendingActionChancePatch
    {
        private static void Postfix(Repairable __instance, ToolsItem __0, float __result)
        {
            if (!Core.IsGameplayActive) return;
            GearItem gear = __instance?.m_GearItem;
            GearItem tool = __0?.GetComponent<GearItem>();
            SkillActionDiagnostics.LogSample(SkillType.ClothingRepair, "Repairable.GetChanceActionSuccess", $"Item={SkillActionDiagnostics.GearName(gear)} | Tool={SkillActionDiagnostics.GearName(tool)} | Final repair chance={SkillActionDiagnostics.Percent(__result)}");
        }
    }

    [HarmonyPatch(typeof(Repairable), nameof(Repairable.GetConditionIncreaseFromRepair))]
    internal static class MendingActionConditionIncreasePatch
    {
        private static void Postfix(Repairable __instance, float __result)
        {
            if (!Core.IsGameplayActive) return;
            GearItem gear = __instance?.m_GearItem;
            SkillActionDiagnostics.LogSample(SkillType.ClothingRepair, "Repairable.GetConditionIncreaseFromRepair", $"Item={SkillActionDiagnostics.GearName(gear)} | Final condition restored={SkillActionDiagnostics.Percent(__result)}");
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.GetModifiedRepairDuration))]
    internal static class MendingActionDurationPatch
    {
        private static void Postfix(GearItem __0, int __1, float __2, int __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__0?.m_ClothingItem == null) return;
            SkillActionDiagnostics.LogSample(SkillType.ClothingRepair, "Panel_Inventory_Examine.GetModifiedRepairDuration", $"Item={SkillActionDiagnostics.GearName(__0)} | Base duration={__1} min | Requested restore={SkillActionDiagnostics.Percent(__2)} | Final repair duration={__result} min");
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RollForActionSuccess))]
    internal static class MendingActionRollPatch
    {
        private static void Postfix(Panel_Inventory_Examine __instance, float __0, bool __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__instance?.m_GearItem?.m_ClothingItem == null) return;
            SkillActionDiagnostics.LogAction(SkillType.ClothingRepair, "Panel_Inventory_Examine.RollForActionSuccess", $"Item={SkillActionDiagnostics.GearName(__instance.m_GearItem)} | Chance used={SkillActionDiagnostics.Percent(__0)} | Actual roll success={__result}");
        }
    }

    internal sealed class MendingRepairActionState
    {
        internal GearItem Gear;
        internal float ConditionBefore;
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RepairSuccessful))]
    internal static class MendingActionRepairResultPatch
    {
        [HarmonyPriority(Priority.First)]
        private static void Prefix(Panel_Inventory_Examine __instance, ref MendingRepairActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            GearItem gear = __instance?.m_GearItem;
            if (!SkillActionDiagnostics.Enabled || gear?.m_ClothingItem == null) return;
            __state = new MendingRepairActionState { Gear = gear, ConditionBefore = gear.GetNormalizedCondition() };
        }

        [HarmonyPriority(Priority.Last)]
        private static void Postfix(MendingRepairActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__state?.Gear == null) return;
            float after = __state.Gear.GetNormalizedCondition();
            SkillActionDiagnostics.LogAction(SkillType.ClothingRepair, "Panel_Inventory_Examine.RepairSuccessful", $"Item={SkillActionDiagnostics.GearName(__state.Gear)} | Actual condition={SkillActionDiagnostics.Percent(__state.ConditionBefore * 100f)} -> {SkillActionDiagnostics.Percent(after * 100f)} | Actual restored={SkillActionDiagnostics.Percent((after - __state.ConditionBefore) * 100f)}");
        }
    }

    internal sealed class MendingToolActionState
    {
        internal GearItem Tool;
        internal float ConditionBefore;
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.DegradeToolUsedForAction))]
    internal static class MendingActionToolWearPatch
    {
        private static void Prefix(Panel_Inventory_Examine __instance, ref MendingToolActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (!SkillActionDiagnostics.Enabled || __instance?.m_GearItem?.m_ClothingItem == null) return;
            Repairable repairable = __instance.m_GearItem.m_Repairable;
            if (repairable == null || repairable.m_FilteredRepairToolChoices == null) return;
            int index = __instance.m_SelectedToolIndex;
            if (index < 0 || index >= repairable.m_FilteredRepairToolChoices.Count) return;
            GearItem tool = repairable.m_FilteredRepairToolChoices[index]?.GetComponent<GearItem>();
            if (tool == null) return;
            __state = new MendingToolActionState { Tool = tool, ConditionBefore = tool.GetNormalizedCondition() };
        }

        private static void Postfix(MendingToolActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__state?.Tool == null) return;
            float after = __state.Tool.GetNormalizedCondition();
            SkillActionDiagnostics.LogAction(SkillType.ClothingRepair, "Panel_Inventory_Examine.DegradeToolUsedForAction", $"Tool={SkillActionDiagnostics.GearName(__state.Tool)} | Actual condition={SkillActionDiagnostics.Percent(__state.ConditionBefore * 100f)} -> {SkillActionDiagnostics.Percent(after * 100f)} | Actual wear={SkillActionDiagnostics.Percent((__state.ConditionBefore - after) * 100f)}");
        }
    }

    [HarmonyPatch(typeof(BowItem), nameof(BowItem.GetSwayIncreasePerSecond))]
    internal static class ArcheryActionSwayPatch
    {
        private static void Postfix(BowItem __instance, float __result)
        {
            if (!Core.IsGameplayActive) return;
            GearItem gear = __instance?.m_GearBow;
            SkillActionDiagnostics.LogSample(SkillType.Archery, "BowItem.GetSwayIncreasePerSecond", $"Bow={SkillActionDiagnostics.GearName(gear)} | Final sway increase={SkillActionDiagnostics.Number(__result)} per second");
        }
    }

    internal sealed class BowFireActionState
    {
        internal bool Crouched;
        internal BowState StateBefore;
    }

    [HarmonyPatch(typeof(BowItem), nameof(BowItem.PressFire))]
    internal static class ArcheryActionCrouchedFirePatch
    {
        private static void Prefix(BowItem __instance, ref BowFireActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (!SkillActionDiagnostics.Enabled || __instance == null) return;
            PlayerManager player = GameManager.GetPlayerManagerComponent();
            __state = new BowFireActionState { Crouched = player != null && player.PlayerIsCrouched(), StateBefore = __instance.m_BowState };
        }

        private static void Postfix(BowItem __instance, BowFireActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__instance == null || __state == null || !__state.Crouched) return;
            SkillActionDiagnostics.LogAction(SkillType.Archery, "BowItem.PressFire", $"Player crouched=True | Bow state={__state.StateBefore} -> {__instance.m_BowState} | Fire input accepted/state changed={__state.StateBefore != __instance.m_BowState}");
        }
    }

    [HarmonyPatch(typeof(BodyDamage), nameof(BodyDamage.GetDamageScale), new Type[] { typeof(BodyPart), typeof(WeaponSource) })]
    internal static class WeaponActionDamagePatch
    {
        private static void Postfix(BodyPart __0, WeaponSource __1, float __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__1 != WeaponSource.Arrow && __1 != WeaponSource.HardenedArrow) return;
            SkillActionDiagnostics.LogSample(SkillType.Archery, "BodyDamage.GetDamageScale", $"Weapon={__1} | Body part={__0} | Final damage scale={SkillActionDiagnostics.Number(__result)}");
        }
    }

    [HarmonyPatch(typeof(BodyDamage), nameof(BodyDamage.GetBleedOutMinutes), new Type[] { typeof(BodyPart), typeof(WeaponSource) })]
    internal static class WeaponActionBleedPatch
    {
        private static void Postfix(BodyPart __0, WeaponSource __1, float __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__1 != WeaponSource.Arrow) return;
            SkillActionDiagnostics.LogSample(SkillType.Archery, "BodyDamage.GetBleedOutMinutes", $"Weapon=Arrow | Body part={__0} | Final bleed-out time={SkillActionDiagnostics.Number(__result)} min");
        }
    }

    [HarmonyPatch(typeof(BodyDamage), nameof(BodyDamage.GetChanceKill), new Type[] { typeof(BodyPart), typeof(WeaponSource) })]
    internal static class WeaponActionCriticalPatch
    {
        private static void Postfix(BodyPart __0, WeaponSource __1, int __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__1 != WeaponSource.Arrow && __1 != WeaponSource.HardenedArrow) return;
            SkillActionDiagnostics.LogSample(SkillType.Archery, "BodyDamage.GetChanceKill", $"Weapon={__1} | Body part={__0} | Final instant-kill chance={SkillActionDiagnostics.Percent(__result)}");
        }
    }

    internal sealed class WeaponWearActionState
    {
        internal SkillType SkillType;
        internal float ConditionBefore;
    }

    [HarmonyPatch(typeof(GearItem), nameof(GearItem.DegradeOnUse))]
    internal static class WeaponActionWearPatch
    {
        private static void Prefix(GearItem __instance, ref WeaponWearActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (!SkillActionDiagnostics.Enabled || __instance == null) return;

            if (__instance.m_BowItem == null) return;
            __state = new WeaponWearActionState { SkillType = SkillType.Archery, ConditionBefore = __instance.GetNormalizedCondition() };
        }

        private static void Postfix(GearItem __instance, WeaponWearActionState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__instance == null || __state == null) return;
            float after = __instance.GetNormalizedCondition();
            SkillActionDiagnostics.LogAction(__state.SkillType, "GearItem.DegradeOnUse", $"Weapon={SkillActionDiagnostics.GearName(__instance)} | Actual condition={SkillActionDiagnostics.Percent(__state.ConditionBefore * 100f)} -> {SkillActionDiagnostics.Percent(after * 100f)} | Actual wear={SkillActionDiagnostics.Percent((__state.ConditionBefore - after) * 100f)}");
        }
    }

    [HarmonyPatch(typeof(BaseAi), nameof(BaseAi.ApplyDamage), new Type[] { typeof(float), typeof(float), typeof(DamageSource), typeof(string) })]
    internal static class WeaponActionFinalDamagePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(BaseAi __instance, float __0, float __1, DamageSource __2, string __3)
        {
            if (!Core.IsGameplayActive) return;
            if (!SkillActionDiagnostics.Enabled) return;
            GearItem held = GameManager.GetPlayerManagerComponent()?.m_ItemInHands;
            if (held?.m_BowItem == null) return;

            string target = __instance == null || string.IsNullOrEmpty(__instance.name) ? "<unknown animal>" : __instance.name;
            SkillActionDiagnostics.LogAction(SkillType.Archery, "BaseAi.ApplyDamage", $"Target={target} | Damage source={__2} | Final damage applied={SkillActionDiagnostics.Number(__0)} | Final bleed-out duration={SkillActionDiagnostics.Number(__1)} min | Weapon identifier={(__3 ?? "<none>")}");
        }
    }

    [HarmonyPatch(typeof(CraftingOperation), "ApplyCraftingProgress", new Type[] { typeof(float) })]
    internal static class GunsmithingActionCraftingContextPatch
    {
        internal static BlueprintData ActiveBlueprint;

        private static void Prefix(CraftingOperation __instance, ref bool __state)
        {
            if (!Core.IsGameplayActive) return;
            BlueprintData blueprint = __instance?.Blueprint;
            __state = SkillActionDiagnostics.Enabled
                && blueprint != null
                && (blueprint.m_AppliedSkill == SkillType.Gunsmithing || blueprint.m_ImprovedSkill == SkillType.Gunsmithing);
            if (!__state) return;

            if (SkillActionDiagnostics.GunsmithCraftingDepth == 0) ActiveBlueprint = blueprint;
            SkillActionDiagnostics.GunsmithCraftingDepth++;
        }

        private static void Postfix(CraftingOperation __instance, float __0, bool __state)
        {
            if (!Core.IsGameplayActive) return;
            if (!__state) return;
            BlueprintData blueprint = __instance?.Blueprint;
            SkillActionDiagnostics.LogAction(SkillType.Gunsmithing, "CraftingOperation.ApplyCraftingProgress", $"Blueprint={(blueprint == null || string.IsNullOrEmpty(blueprint.name) ? "<unknown>" : blueprint.name)} | Actual crafting hours credited to operation={SkillActionDiagnostics.Number(__0)} h");
        }

        private static Exception Finalizer(Exception __exception, bool __state)
        {
            if (!__state) return __exception;

            SkillActionDiagnostics.GunsmithCraftingDepth = Math.Max(0, SkillActionDiagnostics.GunsmithCraftingDepth - 1);
            if (SkillActionDiagnostics.GunsmithCraftingDepth == 0) ActiveBlueprint = null;
            return __exception;
        }
    }

    [HarmonyPatch(typeof(CraftingOperation), "ConsumeMaterialsUsedForCrafting", new Type[] { typeof(float) })]
    internal static class GunsmithingActionMaterialConsumptionPatch
    {
        private static void Postfix(CraftingOperation __instance, float __0)
        {
            if (!Core.IsGameplayActive) return;
            if (!SkillActionDiagnostics.Enabled) return;
            BlueprintData blueprint = __instance?.Blueprint;
            if (blueprint == null || (blueprint.m_AppliedSkill != SkillType.Gunsmithing && blueprint.m_ImprovedSkill != SkillType.Gunsmithing)) return;
            SkillActionDiagnostics.LogAction(SkillType.Gunsmithing, "CraftingOperation.ConsumeMaterialsUsedForCrafting", $"Blueprint={(string.IsNullOrEmpty(blueprint.name) ? "<unnamed>" : blueprint.name)} | Actual crafting hours used for proportional material consumption={SkillActionDiagnostics.Number(__0)} h");
        }
    }

    [HarmonyPatch(typeof(PlayerManager), nameof(PlayerManager.InstantiateCraftedItemInPlayerInventory))]
    internal static class GunsmithingActionAmmoConditionPatch
    {
        private static void Postfix(int __1, float __2)
        {
            if (!Core.IsGameplayActive) return;
            if (SkillActionDiagnostics.GunsmithCraftingDepth <= 0) return;

            BlueprintData blueprint = GunsmithingActionCraftingContextPatch.ActiveBlueprint;
            GearItem result = blueprint?.m_CraftedResultGear;
            SkillActionDiagnostics.LogAction(
                SkillType.Gunsmithing,
                "PlayerManager.InstantiateCraftedItemInPlayerInventory",
                $"Blueprint={(blueprint == null || string.IsNullOrEmpty(blueprint.name) ? "<unknown>" : blueprint.name)} | Crafted result={SkillActionDiagnostics.GearName(result)} | Units={__1} | Final normalized condition passed to result={SkillActionDiagnostics.Percent(__2 * 100f)}");
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.CheckForHarvestSuccess))]
    internal static class GunsmithingActionHarvestPatch
    {
        private static void Prefix(Panel_Inventory_Examine __instance, ref bool __state)
        {
            if (!Core.IsGameplayActive) return;
            GearItem gear = __instance?.m_GearItem;
            __state = gear != null
                && (gear.m_AmmoItem != null || gear.m_AmmoCasingItem != null)
                && SkillActionDiagnostics.BeginChanceContext(SkillType.Gunsmithing, "Panel_Inventory_Examine.CheckForHarvestSuccess");
        }

        private static void Postfix(Panel_Inventory_Examine __instance, bool __result)
        {
            if (!Core.IsGameplayActive) return;
            GearItem gear = __instance?.m_GearItem;
            if (gear == null || (gear.m_AmmoItem == null && gear.m_AmmoCasingItem == null)) return;
            SkillActionDiagnostics.LogAction(SkillType.Gunsmithing, "Panel_Inventory_Examine.CheckForHarvestSuccess", $"Ammunition={SkillActionDiagnostics.GearName(gear)} | Actual component recovery roll success={__result}");
        }

        private static Exception Finalizer(Exception __exception, bool __state)
        {
            SkillActionDiagnostics.EndChanceContext(__state);
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Panel_Milling), "DetermineConditionImprovement", new Type[] { typeof(SkillType) })]
    internal static class GunsmithingActionMillingConditionPatch
    {
        private static void Postfix(SkillType __0, float __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__0 != SkillType.Gunsmithing) return;
            SkillActionDiagnostics.LogSample(SkillType.Gunsmithing, "Panel_Milling.DetermineConditionImprovement", $"Final condition restored by milling={SkillActionDiagnostics.Percent(__result * 100f)}");
        }
    }

    [HarmonyPatch(typeof(Panel_Milling), "RollForRepairSuccess", new Type[] { typeof(SkillType) })]
    internal static class GunsmithingActionMillingSuccessPatch
    {
        private static void Prefix(SkillType __0, ref bool __state)
        {
            if (!Core.IsGameplayActive) return;
            __state = __0 == SkillType.Gunsmithing
                && SkillActionDiagnostics.BeginChanceContext(SkillType.Gunsmithing, "Panel_Milling.RollForRepairSuccess");
        }

        private static void Postfix(SkillType __0, bool __result)
        {
            if (!Core.IsGameplayActive) return;
            if (__0 != SkillType.Gunsmithing) return;
            SkillActionDiagnostics.LogAction(SkillType.Gunsmithing, "Panel_Milling.RollForRepairSuccess", $"Actual milling repair roll success={__result}");
        }

        private static Exception Finalizer(Exception __exception, bool __state)
        {
            SkillActionDiagnostics.EndChanceContext(__state);
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.GetFinalCraftingTimeWithAllModifiers))]
    internal static class GunsmithingActionCraftingTimePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(Panel_Crafting __instance, int __result)
        {
            if (!Core.IsGameplayActive) return;
            BlueprintData blueprint = __instance?.SelectedBPI;
            if (blueprint == null || (blueprint.m_AppliedSkill != SkillType.Gunsmithing && blueprint.m_ImprovedSkill != SkillType.Gunsmithing)) return;

            GearItem result = blueprint.m_CraftedResultGear;
            float combinedScale = blueprint.m_DurationMinutes > 0 ? (float)__result / blueprint.m_DurationMinutes : 0f;
            SkillActionDiagnostics.LogSample(
                SkillType.Gunsmithing,
                "Panel_Crafting.GetFinalCraftingTimeWithAllModifiers",
                $"Blueprint={(string.IsNullOrEmpty(blueprint.name) ? "<unnamed>" : blueprint.name)} | Crafted result={SkillActionDiagnostics.GearName(result)} | Blueprint base duration={blueprint.m_DurationMinutes} min | Final crafting time={__result} min | Combined final scale=x{SkillActionDiagnostics.Number(combinedScale)} | Applied skill={blueprint.m_AppliedSkill} | Improved skill={blueprint.m_ImprovedSkill} | Crafting location={blueprint.m_RequiredCraftingLocation}");
        }
    }

    [HarmonyPatch(typeof(Utils), nameof(Utils.RollChance), new Type[] { typeof(float) })]
    internal static class SkillActionActualChanceRollPatch
    {
        private static void Postfix(float __0, bool __result)
        {
            if (!Core.IsGameplayActive) return;
            SkillActionDiagnostics.LogChanceRoll(__0, __result);
        }
    }

}