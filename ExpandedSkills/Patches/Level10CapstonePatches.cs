using System.Reflection;
using ExpandedSkills.Framework;
using Il2CppTLD.Gear;
using Il2CppTLD.IntBackedUnit;

namespace ExpandedSkills.Patches
{
    internal static class CookingStarvationDrain
    {
        private const float MinimumAppliedDelta = 0.0002f;
        private static Condition _owner;
        private static float _pendingHealthDelta;

        internal static bool Filter(Condition condition, ref float healthDelta, DamageSource source)
        {
            if (source != DamageSource.Starving || healthDelta >= 0f) return true;

            if (!ExpandedSkillProgression.IsLevel10(SkillType.Cooking))
            {
                if (_owner == condition)
                {
                    _owner = null;
                    _pendingHealthDelta = 0f;
                }

                SkillActionDiagnostics.LogAction(SkillType.Cooking, "Condition.AddHealth(Starving)", $"Actual health delta passed to Condition={SkillActionDiagnostics.Number(healthDelta)}");
                return true;
            }

            if (_owner != condition)
            {
                _owner = condition;
                _pendingHealthDelta = 0f;
            }

            _pendingHealthDelta += healthDelta * 0.5f;
            if (Mathf.Abs(_pendingHealthDelta) < MinimumAppliedDelta)
            {
                SkillActionDiagnostics.LogAction(SkillType.Cooking, "Condition.AddHealth(Starving)", "Health tick accumulated; no Condition delta applied on this call.");
                return false;
            }

            healthDelta = _pendingHealthDelta;
            _pendingHealthDelta = 0f;
            SkillActionDiagnostics.LogAction(SkillType.Cooking, "Condition.AddHealth(Starving)", $"Actual reduced health delta passed to Condition={SkillActionDiagnostics.Number(healthDelta)}");
            return true;
        }
    }

    [HarmonyPatch(typeof(Condition), nameof(Condition.AddHealth), new[] { typeof(float), typeof(DamageSource), typeof(bool) })]
    internal static class CookingStarvationDrainPatch
    {
        private static bool Prefix(Condition __instance, ref float __0, DamageSource __1)
        {
            if (!Core.IsGameplayActive) return true;
            return CookingStarvationDrain.Filter(__instance, ref __0, __1);
        }
    }

    internal static class FirestartingTorchContext
    {
        private static int _depth;
        private static GearItem _createdTorch;

        internal static bool Begin()
        {
            if (!ExpandedSkillProgression.IsLevel10(SkillType.Firestarting)) return false;
            if (_depth++ == 0) _createdTorch = null;
            return true;
        }

        internal static void Capture(GearItem gear)
        {
            if (_depth <= 0 || gear == null || gear.m_TorchItem == null) return;
            _createdTorch = gear;
        }

        internal static Exception Complete(Exception exception, bool state)
        {
            if (!state) return exception;

            _depth = Math.Max(0, _depth - 1);
            if (_depth > 0) return exception;

            GearItem torch = _createdTorch;
            _createdTorch = null;
            if (torch == null || torch.m_TorchItem == null) return exception;

            float originalCondition = torch.GetNormalizedCondition();
            float improvedCondition = Mathf.Clamp01(originalCondition + 0.3f);
            torch.SetNormalizedHP(improvedCondition, false);
            torch.m_TorchItem.m_ElapsedBurnMinutes = Mathf.Clamp01(1f - improvedCondition)
                * torch.m_TorchItem.GetModifiedBurnLifetimeMinutes();
            SkillActionDiagnostics.LogAction(SkillType.Firestarting, "Panel_FeedFire.OnTakeTorch", $"Actual created torch condition={SkillActionDiagnostics.Percent(originalCondition * 100f)} -> {SkillActionDiagnostics.Percent(improvedCondition * 100f)}");
            return exception;
        }
    }

    [HarmonyPatch(typeof(Panel_FeedFire), nameof(Panel_FeedFire.OnTakeTorch))]
    internal static class FirestartingTakeTorchContextPatch
    {
        private static void Prefix(ref bool __state)
        {
            if (!Core.IsGameplayActive) return;
            __state = FirestartingTorchContext.Begin();
        }

        private static Exception Finalizer(Exception __exception, bool __state)
        {
            return FirestartingTorchContext.Complete(__exception, __state);
        }
    }

    [HarmonyPatch]
    internal static class FirestartingCreatedTorchCapturePatch
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (MethodInfo method in AccessTools.GetDeclaredMethods(typeof(PlayerManager)))
            {
                if (method.Name == nameof(PlayerManager.InstantiateItemInPlayerInventory)
                    && method.ReturnType == typeof(GearItem))
                {
                    yield return method;
                }
            }
        }

        private static void Postfix(GearItem __result)
        {
            if (!Core.IsGameplayActive) return;
            FirestartingTorchContext.Capture(__result);
        }
    }

    [HarmonyPatch(typeof(BodyHarvest), nameof(BodyHarvest.InitializeResourcesAndConditions))]
    internal static class CarcassMeatAvailabilityPatch
    {
        private static void Prefix(BodyHarvest __instance, ref bool __state)
        {
            if (!Core.IsGameplayActive) return;
            __state = __instance != null
                && !__instance.m_HasInitialized
                && ExpandedSkillProgression.IsLevel10(SkillType.CarcassHarvesting);
        }

        private static void Postfix(BodyHarvest __instance, bool __state)
        {
            if (!Core.IsGameplayActive) return;
            if (!__state || __instance == null) return;

            float kilograms = __instance.m_MeatAvailableKG.ToQuantity(1f);
            if (kilograms <= 0f) return;

            float improvedKilograms = kilograms * 1.1f;
            __instance.m_MeatAvailableKG = ItemWeight.FromKilograms(improvedKilograms);
            SkillActionDiagnostics.LogAction(SkillType.CarcassHarvesting, "BodyHarvest.InitializeResourcesAndConditions", $"Actual initial meat availability={SkillActionDiagnostics.Number(kilograms)} kg -> {SkillActionDiagnostics.Number(improvedKilograms)} kg");
        }
    }


    internal static class RuinedClothingRepair
    {
        internal sealed class PreviewState
        {
            internal GearItem Gear;
            internal bool Ended;
        }

        private static GearItem _previewGear;
        private static GearItem _activeRepairGear;
        private static float _originalHP;
        private static bool _originalWornOut;
        private static int _previewDepth;

        internal static bool IsEligible(GearItem gear)
        {
            return ExpandedSkillProgression.IsLevel10(SkillType.ClothingRepair)
                && gear != null
                && gear.m_ClothingItem != null
                && gear.m_Repairable != null
                && (gear.m_WornOut || gear.CurrentHP <= 0.001f || gear.GetRoundedCondition() <= 0);
        }

        internal static PreviewState BeginPreview(Panel_Inventory_Examine panel)
        {
            GearItem gear = panel?.m_GearItem;
            if (!IsEligible(gear)) return null;

            if (_previewDepth > 0 && _previewGear != gear) RestorePreview();

            PreviewState state = new PreviewState { Gear = gear };
            if (_previewDepth++ > 0) return state;

            _previewGear = gear;
            _originalHP = gear.CurrentHP;
            _originalWornOut = gear.m_WornOut;

            gear.CurrentHP = Mathf.Max(0.01f, gear.CurrentHP);
            gear.m_WornOut = false;
            return state;
        }

        internal static void EndPreview(PreviewState state)
        {
            if (state == null || state.Ended) return;
            state.Ended = true;

            if (_previewGear != state.Gear) return;
            _previewDepth = Math.Max(0, _previewDepth - 1);
            if (_previewDepth > 0) return;

            RestorePreview();
        }

        internal static void RefreshDisplayedCondition(Panel_Inventory_Examine panel, PreviewState state)
        {
            if (state?.Gear == null || panel?.m_GearItem != state.Gear) return;

            if (panel.m_ConditionLabel != null) panel.m_ConditionLabel.text = "0%";
            if (panel.m_ConditionSprite != null) panel.m_ConditionSprite.fillAmount = 0f;
        }

        internal static bool ShouldOverrideConditionIncrease(Repairable repairable)
        {
            GearItem gear = repairable?.m_GearItem;
            if (gear == null) gear = repairable?.GetComponent<GearItem>();

            return gear != null
                && (gear == _previewGear || gear == _activeRepairGear || IsEligible(gear));
        }

        internal static void BeginRepair(Panel_Inventory_Examine panel)
        {
            GearItem gear = panel?.m_GearItem;
            _activeRepairGear = IsEligible(gear) ? gear : null;
        }

        internal static void ValidateRepairStarted(Panel_Inventory_Examine panel)
        {
            if (panel?.m_GearItem == _activeRepairGear && !panel.m_RepairInProgress) _activeRepairGear = null;
        }

        internal static GearItem CaptureSuccessfulRepair(Panel_Inventory_Examine panel)
        {
            GearItem gear = panel?.m_GearItem;
            return gear != null && gear == _activeRepairGear ? gear : null;
        }

        internal static void CompleteRepair(GearItem gear)
        {
            if (gear == null) return;

            gear.SetNormalizedHP(0.1f, false);
            gear.m_WornOut = false;
            gear.UpdateDamageShader();

            if (_activeRepairGear == gear) _activeRepairGear = null;
        }

        internal static void CancelRepair(Panel_Inventory_Examine panel)
        {
            if (panel?.m_GearItem == _activeRepairGear) _activeRepairGear = null;
        }

        internal static void Reset()
        {
            RestorePreview();
            _activeRepairGear = null;
        }

        private static void RestorePreview()
        {
            GearItem gear = _previewGear;
            _previewGear = null;
            _previewDepth = 0;

            if (gear == null) return;
            gear.CurrentHP = _originalHP;
            gear.m_WornOut = _originalWornOut;
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.Enable), new[] { typeof(bool) })]
    internal static class MendingRuinedClothingEnableSimplePatch
    {
        private static void Prefix(Panel_Inventory_Examine __instance, bool __0, ref RuinedClothingRepair.PreviewState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__0) __state = RuinedClothingRepair.BeginPreview(__instance);
            else RuinedClothingRepair.Reset();
        }

        private static void Postfix(Panel_Inventory_Examine __instance, RuinedClothingRepair.PreviewState __state)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.EndPreview(__state);
            RuinedClothingRepair.RefreshDisplayedCondition(__instance, __state);
        }

        private static Exception Finalizer(Exception __exception, RuinedClothingRepair.PreviewState __state)
        {
            RuinedClothingRepair.EndPreview(__state);
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.Enable), new[] { typeof(bool), typeof(ComingFromScreenCategory) })]
    internal static class MendingRuinedClothingEnablePatch
    {
        private static void Prefix(Panel_Inventory_Examine __instance, bool __0, ref RuinedClothingRepair.PreviewState __state)
        {
            if (!Core.IsGameplayActive) return;
            if (__0) __state = RuinedClothingRepair.BeginPreview(__instance);
            else RuinedClothingRepair.Reset();
        }

        private static void Postfix(Panel_Inventory_Examine __instance, RuinedClothingRepair.PreviewState __state)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.EndPreview(__state);
            RuinedClothingRepair.RefreshDisplayedCondition(__instance, __state);
        }

        private static Exception Finalizer(Exception __exception, RuinedClothingRepair.PreviewState __state)
        {
            RuinedClothingRepair.EndPreview(__state);
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RefreshRepairPanel))]
    internal static class MendingRuinedClothingRefreshPatch
    {
        private static void Prefix(Panel_Inventory_Examine __instance, ref RuinedClothingRepair.PreviewState __state)
        {
            if (!Core.IsGameplayActive) return;
            __state = RuinedClothingRepair.BeginPreview(__instance);
        }

        private static void Postfix(Panel_Inventory_Examine __instance, RuinedClothingRepair.PreviewState __state)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.EndPreview(__state);
            RuinedClothingRepair.RefreshDisplayedCondition(__instance, __state);
        }

        private static Exception Finalizer(Exception __exception, RuinedClothingRepair.PreviewState __state)
        {
            RuinedClothingRepair.EndPreview(__state);
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Repairable), nameof(Repairable.GetConditionIncreaseFromRepair))]
    internal static class MendingRuinedClothingConditionPatch
    {
        private static bool Prefix(Repairable __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!RuinedClothingRepair.ShouldOverrideConditionIncrease(__instance)) return true;

            __result = 10f;
            return false;
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnSelectRepairTool))]
    internal static class MendingRuinedClothingStartPatch
    {
        private static void Prefix(Panel_Inventory_Examine __instance)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.BeginRepair(__instance);
        }

        private static void Postfix(Panel_Inventory_Examine __instance)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.ValidateRepairStarted(__instance);
        }

        private static Exception Finalizer(Exception __exception, Panel_Inventory_Examine __instance)
        {
            if (__exception != null) RuinedClothingRepair.CancelRepair(__instance);
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RepairSuccessful))]
    internal static class MendingRuinedClothingSuccessfulPatch
    {
        private static void Prefix(Panel_Inventory_Examine __instance, ref GearItem __state)
        {
            if (!Core.IsGameplayActive) return;
            __state = RuinedClothingRepair.CaptureSuccessfulRepair(__instance);
        }

        private static void Postfix(GearItem __state)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.CompleteRepair(__state);
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.RepairFailed))]
    internal static class MendingRuinedClothingFailedPatch
    {
        private static void Postfix(Panel_Inventory_Examine __instance)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.CancelRepair(__instance);
        }
    }

    [HarmonyPatch(typeof(Panel_Inventory_Examine), nameof(Panel_Inventory_Examine.OnProgressBarCancel))]
    internal static class MendingRuinedClothingCancelPatch
    {
        private static void Postfix(Panel_Inventory_Examine __instance)
        {
            if (!Core.IsGameplayActive) return;
            RuinedClothingRepair.CancelRepair(__instance);
        }
    }

    internal static class WeaponAnimationLevel10
    {
        internal const float GunTimeScale = 0.75f;
        internal const float BowTimeScale = 0.7f;
        internal const float GunFasterPlayback = 1f / GunTimeScale;
        internal const float BowFasterPlayback = 1f / BowTimeScale;

        private static PlayerAnimation _bowAnimation;
        private static bool _bowActionActive;
        private static int _bowActionStartFrame;

        private static PlayerAnimation _reloadAnimation;
        private static GunItem _reloadGun;
        private static float _originalReloadMultiplier;
        private static bool _gunReloadActive;
        private static int _gunReloadStartFrame;

        internal static void BeginBowAction(PlayerAnimation animation)
        {
            if (animation == null || !IsBowLevel10Equipped()) return;

            _bowAnimation = animation;
            _bowActionActive = true;
            _bowActionStartFrame = Time.frameCount;
            ApplyBowSpeed(animation);
            SkillActionDiagnostics.LogAction(SkillType.Archery, "PlayerAnimation bow action", $"Actual bow playback multiplier applied={SkillActionDiagnostics.Number(BowFasterPlayback)}");
        }

        internal static void BeginGunReload(PlayerAnimation animation, GunItem gun)
        {
            if (animation == null || gun == null || !HasGunReloadBonus(gun)) return;

            if (_gunReloadActive)
            {
                if (_reloadGun == gun)
                {
                    _reloadAnimation = animation;
                    ApplyGunReloadSpeed(animation);
                    return;
                }

                ResetGunReload();
            }

            _reloadAnimation = animation;
            _reloadGun = gun;
            _originalReloadMultiplier = gun.m_MultiplierReload;
            _gunReloadActive = true;
            _gunReloadStartFrame = Time.frameCount;
            ApplyGunReloadSpeed(animation);
            SkillActionDiagnostics.LogAction(SkillType.Rifle, "PlayerAnimation gun reload", $"Actual reload multiplier={SkillActionDiagnostics.Number(_originalReloadMultiplier)} -> {SkillActionDiagnostics.Number(gun.m_MultiplierReload)}");
        }

        internal static void Update()
        {
            UpdateBowAction();
            UpdateGunReload();
        }

        internal static void Reset()
        {
            ResetBowAction();
            ResetGunReload();
        }

        private static void UpdateBowAction()
        {
            if (!_bowActionActive) return;

            PlayerAnimation animation = _bowAnimation ?? GameManager.GetPlayerAnimationComponent();
            if (animation == null || !IsBowLevel10Equipped())
            {
                ResetBowAction();
                return;
            }

            PlayerAnimation.State state = animation.GetState();
            bool actionState = state == PlayerAnimation.State.ToAiming || state == PlayerAnimation.State.Reloading;
            if (!actionState && Time.frameCount > _bowActionStartFrame + 2)
            {
                ResetBowAction();
                return;
            }

            ApplyBowSpeed(animation);
        }

        private static void UpdateGunReload()
        {
            if (!_gunReloadActive) return;

            PlayerAnimation animation = _reloadAnimation ?? GameManager.GetPlayerAnimationComponent();
            GearItem item = GameManager.GetPlayerManagerComponent()?.m_ItemInHands;
            bool sameGun = item != null && item.m_GunItem == _reloadGun;
            bool stillReloading = animation != null && animation.GetState() == PlayerAnimation.State.Reloading;

            if (!sameGun || !HasGunReloadBonus(_reloadGun) || (!stillReloading && Time.frameCount > _gunReloadStartFrame + 2))
            {
                ResetGunReload();
                return;
            }

            ApplyGunReloadSpeed(animation);
        }

        private static void ApplyBowSpeed(PlayerAnimation animation)
        {
            animation.SetParameterBowSpeedModifier(BowFasterPlayback);
            SetPlaybackSpeed(animation, animation.m_DefaultPlaybackSpeedMultiplier * BowFasterPlayback);
        }

        private static void ApplyGunReloadSpeed(PlayerAnimation animation)
        {
            if (_reloadGun == null || animation == null) return;

            float playback = _originalReloadMultiplier * GunFasterPlayback;
            _reloadGun.m_MultiplierReload = playback;
            SetPlaybackSpeed(animation, playback);
        }

        private static void SetPlaybackSpeed(PlayerAnimation animation, float speed)
        {
            if (animation == null) return;

            animation.SetFloat(animation.m_AnimParameter_PlaybackSpeedMultiplier, speed);

            if (animation.m_EquippedFirstPersonWeaponLeftHand != null && animation.m_EquippedFirstPersonWeaponLeftHand.m_Animator != null)
                animation.m_EquippedFirstPersonWeaponLeftHand.m_Animator.speed = speed;

            if (animation.m_EquippedFirstPersonWeaponRightHand != null && animation.m_EquippedFirstPersonWeaponRightHand.m_Animator != null)
                animation.m_EquippedFirstPersonWeaponRightHand.m_Animator.speed = speed;

            if (animation.m_EquippedFirstPersonWeaponShoulder != null && animation.m_EquippedFirstPersonWeaponShoulder.m_Animator != null)
                animation.m_EquippedFirstPersonWeaponShoulder.m_Animator.speed = speed;
        }

        private static bool IsBowLevel10Equipped()
        {
            GearItem item = GameManager.GetPlayerManagerComponent()?.m_ItemInHands;
            return item != null && item.m_BowItem != null && ExpandedSkillProgression.IsLevel10(SkillType.Archery);
        }

        private static bool HasGunReloadBonus(GunItem gun)
        {
            if (gun == null) return false;

            return gun.m_GunType == GunType.Rifle && ExpandedSkillProgression.IsLevel10(SkillType.Rifle);
        }

        private static void ResetBowAction()
        {
            if (!_bowActionActive) return;

            PlayerAnimation animation = _bowAnimation ?? GameManager.GetPlayerAnimationComponent();
            if (animation != null)
            {
                animation.SetParameterBowSpeedModifier(1f);
                SetPlaybackSpeed(animation, animation.m_DefaultPlaybackSpeedMultiplier);
            }

            _bowAnimation = null;
            _bowActionActive = false;
            _bowActionStartFrame = 0;
        }

        private static void ResetGunReload()
        {
            if (!_gunReloadActive) return;

            if (_reloadGun != null) _reloadGun.m_MultiplierReload = _originalReloadMultiplier;
            PlayerAnimation animation = _reloadAnimation ?? GameManager.GetPlayerAnimationComponent();
            if (animation != null) SetPlaybackSpeed(animation, animation.m_DefaultPlaybackSpeedMultiplier);

            _reloadAnimation = null;
            _reloadGun = null;
            _originalReloadMultiplier = 0f;
            _gunReloadActive = false;
            _gunReloadStartFrame = 0;
        }
    }

    [HarmonyPatch(typeof(PlayerAnimation), nameof(PlayerAnimation.Trigger_Generic_Aim), new[] { typeof(PlayerAnimation.OnAnimationEvent) })]
    internal static class ArcheryDrawSpeedPatch
    {
        private static void Prefix(PlayerAnimation __instance)
        {
            if (!Core.IsGameplayActive) return;
            WeaponAnimationLevel10.BeginBowAction(__instance);
        }
    }

    [HarmonyPatch(typeof(PlayerAnimation), nameof(PlayerAnimation.Trigger_Generic_Reload), new[]
    {
        typeof(int),
        typeof(int),
        typeof(bool),
        typeof(PlayerAnimation.OnAnimationEvent),
        typeof(PlayerAnimation.OnAnimationEvent),
        typeof(PlayerAnimation.OnAnimationEvent),
        typeof(PlayerAnimation.OnAnimationEvent)
    })]
    internal static class ReloadAndBowNockSpeedPatch
    {
        private static void Prefix(PlayerAnimation __instance)
        {
            if (!Core.IsGameplayActive) return;
            GearItem item = GameManager.GetPlayerManagerComponent()?.m_ItemInHands;
            if (item == null) return;

            if (item.m_BowItem != null)
            {
                WeaponAnimationLevel10.BeginBowAction(__instance);
                return;
            }

            if (item.m_GunItem == null || item.m_GunItem.m_GunType != GunType.Rifle) return;
            WeaponAnimationLevel10.BeginGunReload(__instance, item.m_GunItem);
        }

        private static Exception Finalizer(Exception __exception)
        {
            if (__exception != null) WeaponAnimationLevel10.Reset();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(vp_FPSPlayer), nameof(vp_FPSPlayer.Update))]
    internal static class RevolverLevel10AimingMovementPatch
    {
        private static void Postfix(vp_FPSPlayer __instance)
        {
            if (!Core.IsGameplayActive) return;
            if (__instance == null
                || __instance.Controller == null
                || GameManager.m_IsPaused
                || InterfaceManager.IsOverlayActiveCached()
                || GameManager.ControlsLocked()
                || !GameManager.IsMoveInputUnblocked()
                || !ExpandedSkillProgression.IsLevel10(SkillType.Revolver))
                return;

            PlayerManager player = GameManager.GetPlayerManagerComponent();
            GearItem held = player?.m_ItemInHands;
            if (player == null
                || player.GetControlMode() != PlayerControlMode.AimRevolver
                || held?.m_GunItem == null
                || held.m_GunItem.m_GunType != GunType.Revolver)
                return;

            Vector2 movementInput = __instance.GetMovementInput();
            __instance.Controller.Move(movementInput);
            __instance.m_MovementInputLastFrame = movementInput.magnitude > 0.1f;
        }
    }

    internal static class GunsmithingCraftingTime
    {
        internal static bool IsEligible(BlueprintData blueprint)
        {
            return ExpandedSkillProgression.IsLevel10(SkillType.Gunsmithing)
                && blueprint != null
                && blueprint.m_RequiredCraftingLocation == CraftingLocation.AmmoWorkbench
                && (blueprint.m_AppliedSkill == SkillType.Gunsmithing
                    || blueprint.m_ImprovedSkill == SkillType.Gunsmithing);
        }

        internal static bool TryGetPanel(CraftingRequirementTimeSelect timeSelect, out Panel_Crafting panel)
        {
            panel = InterfaceManager.GetPanel<Panel_Crafting>();
            return panel != null
                && IsEligible(panel.SelectedBPI)
                && panel.m_RequirementContainer != null
                && panel.m_RequirementContainer.m_TimeSelect == timeSelect;
        }
    }

    [HarmonyPatch(typeof(Panel_Crafting), nameof(Panel_Crafting.GetFinalCraftingTimeWithAllModifiers))]
    internal static class GunsmithingFinalCraftingTimePatch
    {
        private static void Postfix(Panel_Crafting __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return;
            if (!GunsmithingCraftingTime.IsEligible(__instance?.SelectedBPI)) return;
            __result = Math.Max(1, Mathf.RoundToInt(__result * 0.5f));
        }
    }


    [HarmonyPatch(typeof(CraftingOperation), nameof(CraftingOperation.ApplyCraftingProgress), new Type[] { typeof(float) })]
    internal static class GunsmithingCraftingProgressPatch
    {
        private static void Prefix(CraftingOperation __instance, ref float __0)
        {
            if (!Core.IsGameplayActive) return;
            if (!GunsmithingCraftingTime.IsEligible(__instance?.Blueprint)) return;
            __0 /= 0.5f;
        }
    }

    [HarmonyPatch(typeof(CraftingOperation), nameof(CraftingOperation.ConsumeMaterialsUsedForCrafting), new Type[] { typeof(float) })]
    internal static class GunsmithingCraftingMaterialsPatch
    {
        private static void Prefix(CraftingOperation __instance, ref float __0)
        {
            if (!Core.IsGameplayActive) return;
            if (!GunsmithingCraftingTime.IsEligible(__instance?.Blueprint)) return;
            __0 /= 0.5f;
        }
    }

    [HarmonyPatch(typeof(CraftingRequirementTimeSelect), nameof(CraftingRequirementTimeSelect.Enable))]
    internal static class GunsmithingTimeSelectEnablePatch
    {
        private static void Postfix(CraftingRequirementTimeSelect __instance)
        {
            if (!Core.IsGameplayActive) return;
            if (!GunsmithingCraftingTime.TryGetPanel(__instance, out _)) return;
            if (__instance.m_TotalCraftingTime <= 0) return;

            __instance.m_DisplayedCraftingTime = __instance.m_TotalCraftingTime;
            __instance.Refresh();
        }
    }

}