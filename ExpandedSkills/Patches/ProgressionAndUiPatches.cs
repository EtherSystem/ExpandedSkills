using ExpandedSkills.Framework;
using ExpandedSkills.Persistence;
using ExpandedSkills.UI;

namespace ExpandedSkills.Patches
{
    [HarmonyPatch(typeof(SkillsManager), nameof(SkillsManager.Awake))]
    internal static class SkillsManagerAwakePatch
    {
        private static void Postfix()
        {
            if (!Core.IsGameplayActive) return;
            ExpandedSkillProgression.ApplyAllTierPointData();
        }
    }

    [HarmonyPatch(typeof(SkillsManager), nameof(SkillsManager.Deserialize))]
    internal static class SkillsManagerDeserializePatch
    {
        private static void Prefix()
        {
            SaveDataManager.BeginSkillsDeserialize();
            if (!Core.IsGameplayActive) return;
            ExpandedSkillProgression.ApplyAllTierPointData();
        }

        private static void Postfix(SkillsManager __instance, string __0)
        {
            SaveDataManager.OnSkillsDeserialized(__instance, __0);
            if (Core.IsGameplayActive)
            {
                ExpandedSkillProgression.ApplyAllTierPointData();
                SkillLevelTracker.NotifySkillsDeserialized();
            }

            SaveDataManager.EndSkillsDeserialize();
        }

        private static Exception Finalizer(Exception __exception)
        {
            if (__exception != null) SaveDataManager.EndSkillsDeserialize();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(SkillsManager), nameof(SkillsManager.Serialize))]
    internal static class SkillsManagerSerializePatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Prefix(out ExpandedSkillPointPersistence.CompatibilityPointScope __state)
        {
            __state = SaveDataManager.ApplyVanillaCompatibilityPointsForSerialization();
        }

        [HarmonyPriority(Priority.First)]
        private static void Postfix(ExpandedSkillPointPersistence.CompatibilityPointScope __state)
        {
            __state?.Restore();
        }

        private static Exception Finalizer(Exception __exception, ExpandedSkillPointPersistence.CompatibilityPointScope __state)
        {
            __state?.Restore();
            return __exception;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.SetPoints))]
    internal static class SkillSetPointsPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill __instance, ref int __0, SkillsManager.PointAssignmentMode __1)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            if (__1 == SkillsManager.PointAssignmentMode.AssignOnlyInSandbox && GameManager.IsStoryMode()) return false;

            int oldLevel = ExpandedSkillProgression.GetRealLevel(__instance);
            int[] tierPoints = ExpandedSkillProgression.GetTierPoints(__instance);
            __instance.m_CurrentPoints = Math.Max(0, Math.Min(__0, tierPoints[ExpandedSkillProgression.MaxTierIndex]));
            SaveDataManager.OnSkillPointsChanged(__instance);
            SkillLevelTracker.ObserveLevelChange(__instance, oldLevel);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.IncrementPoints))]
    internal static class SkillIncrementPointsPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill __instance, ref int __0, SkillsManager.PointAssignmentMode __1)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            if (__1 == SkillsManager.PointAssignmentMode.AssignOnlyInSandbox && GameManager.IsStoryMode()) return false;

            int oldLevel = ExpandedSkillProgression.GetRealLevel(__instance);
            int[] tierPoints = ExpandedSkillProgression.GetTierPoints(__instance);
            long targetPoints = (long)__instance.m_CurrentPoints + __0;
            __instance.m_CurrentPoints = (int)Math.Max(0L, Math.Min(targetPoints, tierPoints[ExpandedSkillProgression.MaxTierIndex]));
            SaveDataManager.OnSkillPointsChanged(__instance);
            SkillLevelTracker.ObserveLevelChange(__instance, oldLevel);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.SetTier))]
    internal static class SkillSetTierPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Skill __instance, int __0, SkillsManager.PointAssignmentMode __1)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;

            int[] tierPoints = ExpandedSkillProgression.GetTierPoints(__instance);
            if (__0 < 0 || __0 >= tierPoints.Length) return true;
            if (__1 == SkillsManager.PointAssignmentMode.AssignOnlyInSandbox && GameManager.IsStoryMode()) return false;

            int oldLevel = ExpandedSkillProgression.GetRealLevel(__instance);
            __instance.m_CurrentPoints = tierPoints[__0];
            SaveDataManager.OnSkillPointsChanged(__instance);
            SkillLevelTracker.ObserveLevelChange(__instance, oldLevel);
            return false;
        }
    }

    [HarmonyPatch(typeof(ResearchItem), nameof(ResearchItem.NoBenefitAtCurrentSkillLevel))]
    internal static class ResearchItemNoBenefitPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(ResearchItem __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance.m_SkillType, out _)) return true;

            Skill skill = ExpandedSkillRegistry.GetSkill(__instance.m_SkillType);
            if (skill == null) return true;

            int vanillaNoBenefitLevel = Math.Max(1, Math.Min(5, __instance.m_NoBenefitAtSkillLevel));
            int expandedNoBenefitLevel = Math.Min(ExpandedSkillProgression.MaxLevel, vanillaNoBenefitLevel * 2);
            __result = ExpandedSkillProgression.GetRealLevel(skill) >= expandedNoBenefitLevel;
            return false;
        }
    }

    [HarmonyPatch(typeof(Il2CppTLD.Cooking.RecipeData), nameof(Il2CppTLD.Cooking.RecipeData.HasCookingSkills))]
    internal static class RecipeDataHasCookingSkillsPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(Il2CppTLD.Cooking.RecipeData __instance, ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;

            Skill_Cooking skill = GameManager.GetSkillCooking();
            if (skill == null || !ExpandedSkillRegistry.TryGet(skill, out _)) return true;

            __result = ExpandedSkillProgression.GetRealLevel(skill) >= __instance.RequiredSkillLevel;
            return false;
        }
    }

    internal static class CookingRecipeSkillCompatibility
    {
        internal static void RemoveSatisfiedSkillRequirement(Il2CppTLD.Cooking.CookableItem item, ref Il2CppTLD.Cooking.CookableItem.Cookablility result)
        {
            if (!Core.IsGameplayActive || item.m_Recipe == null) return;

            Skill_Cooking skill = GameManager.GetSkillCooking();
            if (skill == null || !ExpandedSkillRegistry.TryGet(skill, out _)) return;
            if (ExpandedSkillProgression.GetRealLevel(skill) < item.m_Recipe.RequiredSkillLevel) return;

            result = (Il2CppTLD.Cooking.CookableItem.Cookablility)((int)result & ~(int)Il2CppTLD.Cooking.CookableItem.Cookablility.SkillTooLow);
        }
    }

    [HarmonyPatch(typeof(Il2CppTLD.Cooking.CookableItem), nameof(Il2CppTLD.Cooking.CookableItem.GetCookability))]
    internal static class CookableItemGetCookabilityPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(Il2CppTLD.Cooking.CookableItem __instance, ref Il2CppTLD.Cooking.CookableItem.Cookablility __result)
        {
            CookingRecipeSkillCompatibility.RemoveSatisfiedSkillRequirement(__instance, ref __result);
        }
    }

    [HarmonyPatch(typeof(Il2CppTLD.Cooking.CookableItem), nameof(Il2CppTLD.Cooking.CookableItem.GetPotCookability))]
    internal static class CookableItemGetPotCookabilityPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(Il2CppTLD.Cooking.CookableItem __instance, ref Il2CppTLD.Cooking.CookableItem.Cookablility __result)
        {
            CookingRecipeSkillCompatibility.RemoveSatisfiedSkillRequirement(__instance, ref __result);
        }
    }

    [HarmonyPatch(typeof(Il2CppTLD.Cooking.CookableItem), nameof(Il2CppTLD.Cooking.CookableItem.CanCookItem))]
    internal static class CookableItemCanCookItemPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(Il2CppTLD.Cooking.CookableItem __instance, CookingPotItem __0, Inventory __1, ref bool __result)
        {
            if (__result || !Core.IsGameplayActive || __instance.m_Recipe == null) return;

            Il2CppTLD.Cooking.CookableItem.Cookablility cookability = __instance.GetCookability(__0, __1);
            __result = cookability == Il2CppTLD.Cooking.CookableItem.Cookablility.Cookable;
        }
    }

    [HarmonyPatch(typeof(SkillsManager), nameof(SkillsManager.IncrementPointsAndNotify))]
    internal static class SkillsManagerIncrementPointsPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(SkillsManager __instance, SkillType __0, ref int __1, SkillsManager.PointAssignmentMode __2)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__0, out _)) return true;

            Skill skill = ExpandedSkillRegistry.GetSkill(__0);
            if (skill == null) return false;
            if (__2 == SkillsManager.PointAssignmentMode.AssignOnlyInSandbox && GameManager.IsStoryMode()) return false;

            ExpandedSkillProgression.ApplyAllTierPointData();
            int oldLevel = ExpandedSkillProgression.GetRealLevel(skill);
            int oldPoints = skill.GetPoints();
            int maxPoints = skill.GetMaxPoints();
            if (__1 > 0 && oldPoints >= maxPoints) return false;

            skill.IncrementPoints(__1, __2);
            int newPoints = skill.GetPoints();
            int newLevel = ExpandedSkillProgression.GetRealLevel(skill);
            SkillLevelTracker.ObserveLevelChange(skill, oldLevel);
            if (newPoints == oldPoints) return false;

            SkillNotify notify = GameManager.GetSkillNotify();
            if (notify != null) notify.MaybeShowPointIncrease(skill.m_SkillIcon);

            if (newLevel <= oldLevel) return false;

            AchievementManager achievements = GameManager.GetAchievementManagerComponent();
            if (achievements != null) achievements.UpdateAchievements();

            if (notify != null)
            {
                string tierName = __instance.GetTierName(ExpandedSkillProgression.GetRewardTierIndex(skill));
                notify.MaybeShowLevelUp(skill.m_SkillIconBackground, skill.m_DisplayName, tierName, newLevel);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(SkillsManager), nameof(SkillsManager.AllSkillsAtMaxmiumLevel))]
    internal static class SkillsManagerAllSkillsAtMaximumLevelPatch
    {
        [HarmonyPriority(Priority.Last)]
        private static bool Prefix(ref bool __result)
        {
            if (!Core.IsGameplayActive) return true;
            foreach (ExpandedSkillDefinition definition in ExpandedSkillRegistry.All)
            {
                Skill skill = definition.GetSkill();
                if (skill == null || skill.GetPoints() < ExpandedSkillProgression.GetPointsForLevel(skill, ExpandedSkillProgression.MaxLevel))
                {
                    __result = false;
                    return false;
                }
            }

            __result = true;
            return false;
        }
    }

    [HarmonyPatch(typeof(Panel_Log), nameof(Panel_Log.RefreshSkillsList))]
    internal static class PanelLogRefreshSkillsListPatch
    {
        private static void Postfix(Panel_Log __instance)
        {
            if (!Core.IsGameplayActive) return;
            ExpandedSkillProgression.RefreshSkillListItems(__instance);
        }
    }

    [HarmonyPatch(typeof(Panel_Log), nameof(Panel_Log.RefreshSelectedSkillDescriptionView))]
    internal static class PanelLogRefreshSelectedSkillDescriptionViewPatch
    {
        private static void Postfix(Panel_Log __instance)
        {
            if (!Core.IsGameplayActive) return;
            SkillPanelPresentationRenderer.RenderSelected(__instance);
        }
    }

    [HarmonyPatch(typeof(SkillsManager), nameof(SkillsManager.GetTierName))]
    internal static class SkillsManagerGetTierNamePatch
    {
        private static bool Prefix(SkillsManager __instance, int index, ref string __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (index < 5 || index > ExpandedSkillProgression.MaxTierIndex) return true;
            __result = __instance.GetTierName(ExpandedSkillProgression.GetRewardTierIndexFromRealTierIndex(index));
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetCurrentTierName))]
    internal static class SkillGetCurrentTierNamePatch
    {
        private static bool Prefix(Skill __instance, ref string __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            SkillsManager manager = GameManager.GetSkillsManager();
            __result = manager == null ? string.Empty : manager.GetTierName(ExpandedSkillProgression.GetRewardTierIndex(__instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetCurrentTierDescription))]
    internal static class SkillGetCurrentTierDescriptionPatch
    {
        private static bool Prefix(Skill __instance, ref string __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            __result = __instance.GetTierDescription(ExpandedSkillProgression.GetRewardTierIndex(__instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetCurrentTierBenefits))]
    internal static class SkillGetCurrentTierBenefitsPatch
    {
        private static bool Prefix(Skill __instance, ref string __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out ExpandedSkillDefinition definition)) return true;
            int level = ExpandedSkillProgression.GetRealLevel(__instance);
            __result = ExpandedSkillRewardData.GetBenefits(__instance, level - 1);
            if (level >= ExpandedSkillProgression.MaxLevel && definition.Level10Benefit.Length > 0) __result += "\n" + definition.Level10Benefit;
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetTierDescription))]
    internal static class SkillGetTierDescriptionPatch
    {
        private static bool Prefix(Skill __instance, int index, ref string __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _) || index < 5 || index > ExpandedSkillProgression.MaxTierIndex) return true;
            __result = __instance.GetTierDescription(ExpandedSkillProgression.GetRewardTierIndexFromRealTierIndex(index));
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetTierPoints))]
    internal static class SkillGetTierPointsPatch
    {
        private static bool Prefix(Skill __instance, int index, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;

            int[] tierPoints = ExpandedSkillProgression.GetTierPoints(__instance);
            if (index < 0 || index >= tierPoints.Length)
            {
                __result = 0;
                return false;
            }

            __result = tierPoints[index];
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetMaxPoints))]
    internal static class SkillGetMaxPointsPatch
    {
        private static bool Prefix(Skill __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            __result = ExpandedSkillProgression.GetTierPoints(__instance)[ExpandedSkillProgression.MaxTierIndex];
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetCurrentTierNumber))]
    internal static class SkillGetCurrentTierNumberPatch
    {
        private static bool Prefix(Skill __instance, ref int __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;

            __result = ExpandedSkillProgression.GetRewardTierIndex(__instance);
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetPointsAsNormalizedValue))]
    internal static class SkillGetPointsAsNormalizedValuePatch
    {
        private static bool Prefix(Skill __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            int max = ExpandedSkillProgression.GetTierPoints(__instance)[ExpandedSkillProgression.MaxTierIndex];
            __result = max <= 0 ? 1f : Math.Max(0f, Math.Min(1f, __instance.GetPoints() / (float)max));
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetPointsAsPercent))]
    internal static class SkillGetPointsAsPercentPatch
    {
        private static bool Prefix(Skill __instance, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            int max = ExpandedSkillProgression.GetTierPoints(__instance)[ExpandedSkillProgression.MaxTierIndex];
            __result = max <= 0 ? 100f : Math.Max(0f, Math.Min(100f, __instance.GetPoints() / (float)max * 100f));
            return false;
        }
    }

    [HarmonyPatch(typeof(Skill), nameof(Skill.GetProgressToNextLevelAsNormalizedValue))]
    internal static class SkillGetProgressToNextLevelAsNormalizedValuePatch
    {
        private static bool Prefix(Skill __instance, int addPoints, ref float __result)
        {
            if (!Core.IsGameplayActive) return true;
            if (!ExpandedSkillRegistry.TryGet(__instance, out _)) return true;
            __result = ExpandedSkillProgression.GetProgressToNextLevel(__instance, __instance.GetPoints(), addPoints);
            return false;
        }
    }

}