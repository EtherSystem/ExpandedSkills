using System.Globalization;
using ExpandedSkills.Framework;
using Il2CppTLD.IntBackedUnit;

namespace ExpandedSkills
{
    internal static class SkillActionDiagnostics
    {
        private sealed class ChanceContext
        {
            internal SkillType SkillType;
            internal string Source;
            internal int RollIndex;
        }

        private static readonly HashSet<string> SeenSamples = new();
        private static readonly Stack<ChanceContext> ChanceContexts = new();

        internal static bool Enabled { get; private set; }
        internal static int GunsmithCraftingDepth { get; set; }

        internal static void SetEnabled(bool enabled)
        {
            Enabled = enabled;
            ResetSamples();
            Core.Log(enabled ? "[Skill action test] Enabled. Logs now report final values produced by gameplay action sources." : "[Skill action test] Disabled.");
        }

        internal static void Reset()
        {
            Enabled = false;
            GunsmithCraftingDepth = 0;
            SeenSamples.Clear();
            ChanceContexts.Clear();
        }

        internal static void ResetSamples()
        {
            GunsmithCraftingDepth = 0;
            SeenSamples.Clear();
            ChanceContexts.Clear();
            if (Enabled) Core.Log("[Skill action test] Recorded samples cleared.");
        }

        internal static bool BeginChanceContext(SkillType skillType, string source)
        {
            if (!Enabled) return false;
            ChanceContexts.Push(new ChanceContext { SkillType = skillType, Source = source, RollIndex = 0 });
            return true;
        }

        internal static void EndChanceContext(bool state)
        {
            if (!state || ChanceContexts.Count == 0) return;
            ChanceContexts.Pop();
        }

        internal static void LogChanceRoll(float chance, bool result)
        {
            if (!Enabled || ChanceContexts.Count == 0) return;
            ChanceContext context = ChanceContexts.Peek();
            context.RollIndex++;
            LogAction(context.SkillType, context.Source + ".Utils.RollChance", $"Roll #{context.RollIndex} | Chance actually used={Percent(chance)} | Result={result}");
        }

        internal static void LogSample(SkillType skillType, string source, string details)
        {
            if (!Enabled) return;

            Skill skill = ExpandedSkillRegistry.GetSkill(skillType);
            if (skill == null) return;

            int level = ExpandedSkillProgression.GetRealLevel(skill);
            string key = skillType + "|" + level + "|" + source + "|" + details;
            if (!SeenSamples.Add(key)) return;

            Core.Log($"[Skill action test][{ExpandedSkillRegistry.GetLogName(skillType)} L{level}] {source} | {details}");
        }

        internal static void LogAction(SkillType skillType, string source, string details)
        {
            if (!Enabled) return;

            Skill skill = ExpandedSkillRegistry.GetSkill(skillType);
            if (skill == null) return;

            int level = ExpandedSkillProgression.GetRealLevel(skill);
            Core.Log($"[Skill action test][{ExpandedSkillRegistry.GetLogName(skillType)} L{level}] {source} | {details}");
        }

        internal static string Number(float value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        internal static string Percent(float value)
        {
            return Number(value) + "%";
        }

        internal static string Condition(GearItem gear)
        {
            return gear == null ? "n/a" : Percent(gear.GetNormalizedCondition() * 100f);
        }

        internal static string Weight(GearItem gear)
        {
            return gear == null ? "n/a" : Number(gear.GetItemWeightKG().ToQuantity(1f)) + " kg";
        }

        internal static string GearName(GearItem gear)
        {
            if (gear == null) return "<none>";
            return string.IsNullOrEmpty(gear.name) ? "<unnamed gear>" : gear.name;
        }

    }
}