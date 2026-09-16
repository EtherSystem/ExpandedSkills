using System.Reflection;
using ExpandedSkills.Framework;
using ModSettings;

namespace ExpandedSkills
{
    internal sealed class ExpandedSkillsSettings : JsonModSettings
    {
        [Section("XP Requirements")]

        [Name("Fire Starting")]
        [Description("Vanilla reference: level 4 -> 5 requires 100 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowFireStartingXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 50 XP extra, 150 XP total.")]
        [Slider(0, 100, 101, NumberFormat = "{0:0} XP")]
        public int FireStartingLevel6ExtraXp = 50;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 100 XP extra, 200 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int FireStartingLevel7ExtraXp = 100;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 150 XP extra, 250 XP total.")]
        [Slider(0, 300, 301, NumberFormat = "{0:0} XP")]
        public int FireStartingLevel8ExtraXp = 150;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 200 XP extra, 300 XP total.")]
        [Slider(0, 400, 401, NumberFormat = "{0:0} XP")]
        public int FireStartingLevel9ExtraXp = 200;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 250 XP extra, 350 XP total.")]
        [Slider(0, 500, 501, NumberFormat = "{0:0} XP")]
        public int FireStartingLevel10ExtraXp = 250;

        [Name("Carcass Harvesting")]
        [Description("Vanilla reference: level 4 -> 5 requires 50 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowCarcassHarvestingXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 25 XP extra, 75 XP total.")]
        [Slider(0, 50, 51, NumberFormat = "{0:0} XP")]
        public int CarcassHarvestingLevel6ExtraXp = 25;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 50 XP extra, 100 XP total.")]
        [Slider(0, 100, 101, NumberFormat = "{0:0} XP")]
        public int CarcassHarvestingLevel7ExtraXp = 50;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 75 XP extra, 125 XP total.")]
        [Slider(0, 150, 151, NumberFormat = "{0:0} XP")]
        public int CarcassHarvestingLevel8ExtraXp = 75;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 100 XP extra, 150 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int CarcassHarvestingLevel9ExtraXp = 100;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 125 XP extra, 175 XP total.")]
        [Slider(0, 250, 251, NumberFormat = "{0:0} XP")]
        public int CarcassHarvestingLevel10ExtraXp = 125;

        [Name("Ice Fishing")]
        [Description("Vanilla reference: level 4 -> 5 requires 100 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowIceFishingXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 50 XP extra, 150 XP total.")]
        [Slider(0, 100, 101, NumberFormat = "{0:0} XP")]
        public int IceFishingLevel6ExtraXp = 50;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 100 XP extra, 200 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int IceFishingLevel7ExtraXp = 100;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 150 XP extra, 250 XP total.")]
        [Slider(0, 300, 301, NumberFormat = "{0:0} XP")]
        public int IceFishingLevel8ExtraXp = 150;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 200 XP extra, 300 XP total.")]
        [Slider(0, 400, 401, NumberFormat = "{0:0} XP")]
        public int IceFishingLevel9ExtraXp = 200;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 100 XP and is used as the baseline. Default: 250 XP extra, 350 XP total.")]
        [Slider(0, 500, 501, NumberFormat = "{0:0} XP")]
        public int IceFishingLevel10ExtraXp = 250;

        [Name("Cooking")]
        [Description("Vanilla reference: level 4 -> 5 requires 200 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowCookingXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 100 XP extra, 300 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int CookingLevel6ExtraXp = 100;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 200 XP extra, 400 XP total.")]
        [Slider(0, 400, 401, NumberFormat = "{0:0} XP")]
        public int CookingLevel7ExtraXp = 200;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 300 XP extra, 500 XP total.")]
        [Slider(0, 600, 601, NumberFormat = "{0:0} XP")]
        public int CookingLevel8ExtraXp = 300;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 400 XP extra, 600 XP total.")]
        [Slider(0, 800, 801, NumberFormat = "{0:0} XP")]
        public int CookingLevel9ExtraXp = 400;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 500 XP extra, 700 XP total.")]
        [Slider(0, 1000, 1001, NumberFormat = "{0:0} XP")]
        public int CookingLevel10ExtraXp = 500;

        [Name("Rifle Firearm")]
        [Description("Vanilla reference: level 4 -> 5 requires 50 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowRifleXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 25 XP extra, 75 XP total.")]
        [Slider(0, 50, 51, NumberFormat = "{0:0} XP")]
        public int RifleLevel6ExtraXp = 25;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 50 XP extra, 100 XP total.")]
        [Slider(0, 100, 101, NumberFormat = "{0:0} XP")]
        public int RifleLevel7ExtraXp = 50;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 75 XP extra, 125 XP total.")]
        [Slider(0, 150, 151, NumberFormat = "{0:0} XP")]
        public int RifleLevel8ExtraXp = 75;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 100 XP extra, 150 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int RifleLevel9ExtraXp = 100;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 125 XP extra, 175 XP total.")]
        [Slider(0, 250, 251, NumberFormat = "{0:0} XP")]
        public int RifleLevel10ExtraXp = 125;

        [Name("Archery")]
        [Description("Vanilla reference: level 4 -> 5 requires 50 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowArcheryXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 25 XP extra, 75 XP total.")]
        [Slider(0, 50, 51, NumberFormat = "{0:0} XP")]
        public int ArcheryLevel6ExtraXp = 25;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 50 XP extra, 100 XP total.")]
        [Slider(0, 100, 101, NumberFormat = "{0:0} XP")]
        public int ArcheryLevel7ExtraXp = 50;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 75 XP extra, 125 XP total.")]
        [Slider(0, 150, 151, NumberFormat = "{0:0} XP")]
        public int ArcheryLevel8ExtraXp = 75;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 100 XP extra, 150 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int ArcheryLevel9ExtraXp = 100;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 125 XP extra, 175 XP total.")]
        [Slider(0, 250, 251, NumberFormat = "{0:0} XP")]
        public int ArcheryLevel10ExtraXp = 125;

        [Name("Mending")]
        [Description("Vanilla reference: level 4 -> 5 requires 200 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowMendingXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 100 XP extra, 300 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int MendingLevel6ExtraXp = 100;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 200 XP extra, 400 XP total.")]
        [Slider(0, 400, 401, NumberFormat = "{0:0} XP")]
        public int MendingLevel7ExtraXp = 200;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 300 XP extra, 500 XP total.")]
        [Slider(0, 600, 601, NumberFormat = "{0:0} XP")]
        public int MendingLevel8ExtraXp = 300;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 400 XP extra, 600 XP total.")]
        [Slider(0, 800, 801, NumberFormat = "{0:0} XP")]
        public int MendingLevel9ExtraXp = 400;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 200 XP and is used as the baseline. Default: 500 XP extra, 700 XP total.")]
        [Slider(0, 1000, 1001, NumberFormat = "{0:0} XP")]
        public int MendingLevel10ExtraXp = 500;

        [Name("Revolver Firearm")]
        [Description("Vanilla reference: level 4 -> 5 requires 50 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowRevolverXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 25 XP extra, 75 XP total.")]
        [Slider(0, 50, 51, NumberFormat = "{0:0} XP")]
        public int RevolverLevel6ExtraXp = 25;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 50 XP extra, 100 XP total.")]
        [Slider(0, 100, 101, NumberFormat = "{0:0} XP")]
        public int RevolverLevel7ExtraXp = 50;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 75 XP extra, 125 XP total.")]
        [Slider(0, 150, 151, NumberFormat = "{0:0} XP")]
        public int RevolverLevel8ExtraXp = 75;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 100 XP extra, 150 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int RevolverLevel9ExtraXp = 100;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 125 XP extra, 175 XP total.")]
        [Slider(0, 250, 251, NumberFormat = "{0:0} XP")]
        public int RevolverLevel10ExtraXp = 125;

        [Name("Gunsmithing")]
        [Description("Vanilla reference: level 4 -> 5 requires 50 XP. Expand to customize the additional XP required for levels 6-10.")]
        [Choice("+", "-")]
        public bool ShowGunsmithingXp = false;

        [Name("Level 6")]
        [Description("Sets the additional XP for level 5 -> 6. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 25 XP extra, 75 XP total.")]
        [Slider(0, 50, 51, NumberFormat = "{0:0} XP")]
        public int GunsmithingLevel6ExtraXp = 25;

        [Name("Level 7")]
        [Description("Sets the additional XP for level 6 -> 7. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 50 XP extra, 100 XP total.")]
        [Slider(0, 100, 101, NumberFormat = "{0:0} XP")]
        public int GunsmithingLevel7ExtraXp = 50;

        [Name("Level 8")]
        [Description("Sets the additional XP for level 7 -> 8. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 75 XP extra, 125 XP total.")]
        [Slider(0, 150, 151, NumberFormat = "{0:0} XP")]
        public int GunsmithingLevel8ExtraXp = 75;

        [Name("Level 9")]
        [Description("Sets the additional XP for level 8 -> 9. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 100 XP extra, 150 XP total.")]
        [Slider(0, 200, 201, NumberFormat = "{0:0} XP")]
        public int GunsmithingLevel9ExtraXp = 100;

        [Name("Level 10")]
        [Description("Sets the additional XP for level 9 -> 10. The vanilla level 4 -> 5 requirement is 50 XP and is used as the baseline. Default: 125 XP extra, 175 XP total.")]
        [Slider(0, 250, 251, NumberFormat = "{0:0} XP")]
        public int GunsmithingLevel10ExtraXp = 125;

        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            ExpandedSkillsSettingsManager.RefreshVisibility();
        }

        protected override void OnConfirm()
        {
            base.OnConfirm();
            ExpandedSkillsSettingsManager.ApplyConfirmedSettings();
        }
    }

    internal static class ExpandedSkillsSettingsManager
    {
        internal static readonly ExpandedSkillsSettings Options = new();
        private static readonly Dictionary<SkillType, int[]> AppliedExtraXp = new();

        internal static void Initialize()
        {
            Options.AddToModSettings("Expanded Skills");
            CaptureConfirmedSettings();
            RefreshVisibility();
        }

        internal static int GetExtraXp(SkillType skillType, int level)
        {
            if (level < 6 || level > ExpandedSkillProgression.MaxLevel) return 0;
            if (!AppliedExtraXp.TryGetValue(skillType, out int[] values) || values == null || values.Length < 5) return GetDefaultExtraXp(skillType, level);
            return Math.Max(0, values[level - 6]);
        }

        internal static int GetVanillaLevelFourToFiveXp(SkillType skillType)
        {
            return skillType switch
            {
                SkillType.Firestarting => 100,
                SkillType.CarcassHarvesting => 50,
                SkillType.IceFishing => 100,
                SkillType.Cooking => 200,
                SkillType.Rifle => 50,
                SkillType.Archery => 50,
                SkillType.ClothingRepair => 200,
                SkillType.Revolver => 50,
                SkillType.Gunsmithing => 50,
                _ => 1
            };
        }

        internal static void ApplyConfirmedSettings()
        {
            CaptureConfirmedSettings();
            ExpandedSkillProgression.ResetRuntimeCache();
            if (!Core.IsGameplayActive) return;

            ExpandedSkillProgression.ApplyAllTierPointData();
        }

        internal static void RefreshVisibility()
        {
            Options.SetFieldVisible(nameof(Options.FireStartingLevel6ExtraXp), Options.ShowFireStartingXp);
            Options.SetFieldVisible(nameof(Options.FireStartingLevel7ExtraXp), Options.ShowFireStartingXp);
            Options.SetFieldVisible(nameof(Options.FireStartingLevel8ExtraXp), Options.ShowFireStartingXp);
            Options.SetFieldVisible(nameof(Options.FireStartingLevel9ExtraXp), Options.ShowFireStartingXp);
            Options.SetFieldVisible(nameof(Options.FireStartingLevel10ExtraXp), Options.ShowFireStartingXp);

            Options.SetFieldVisible(nameof(Options.CarcassHarvestingLevel6ExtraXp), Options.ShowCarcassHarvestingXp);
            Options.SetFieldVisible(nameof(Options.CarcassHarvestingLevel7ExtraXp), Options.ShowCarcassHarvestingXp);
            Options.SetFieldVisible(nameof(Options.CarcassHarvestingLevel8ExtraXp), Options.ShowCarcassHarvestingXp);
            Options.SetFieldVisible(nameof(Options.CarcassHarvestingLevel9ExtraXp), Options.ShowCarcassHarvestingXp);
            Options.SetFieldVisible(nameof(Options.CarcassHarvestingLevel10ExtraXp), Options.ShowCarcassHarvestingXp);

            Options.SetFieldVisible(nameof(Options.IceFishingLevel6ExtraXp), Options.ShowIceFishingXp);
            Options.SetFieldVisible(nameof(Options.IceFishingLevel7ExtraXp), Options.ShowIceFishingXp);
            Options.SetFieldVisible(nameof(Options.IceFishingLevel8ExtraXp), Options.ShowIceFishingXp);
            Options.SetFieldVisible(nameof(Options.IceFishingLevel9ExtraXp), Options.ShowIceFishingXp);
            Options.SetFieldVisible(nameof(Options.IceFishingLevel10ExtraXp), Options.ShowIceFishingXp);

            Options.SetFieldVisible(nameof(Options.CookingLevel6ExtraXp), Options.ShowCookingXp);
            Options.SetFieldVisible(nameof(Options.CookingLevel7ExtraXp), Options.ShowCookingXp);
            Options.SetFieldVisible(nameof(Options.CookingLevel8ExtraXp), Options.ShowCookingXp);
            Options.SetFieldVisible(nameof(Options.CookingLevel9ExtraXp), Options.ShowCookingXp);
            Options.SetFieldVisible(nameof(Options.CookingLevel10ExtraXp), Options.ShowCookingXp);

            Options.SetFieldVisible(nameof(Options.RifleLevel6ExtraXp), Options.ShowRifleXp);
            Options.SetFieldVisible(nameof(Options.RifleLevel7ExtraXp), Options.ShowRifleXp);
            Options.SetFieldVisible(nameof(Options.RifleLevel8ExtraXp), Options.ShowRifleXp);
            Options.SetFieldVisible(nameof(Options.RifleLevel9ExtraXp), Options.ShowRifleXp);
            Options.SetFieldVisible(nameof(Options.RifleLevel10ExtraXp), Options.ShowRifleXp);

            Options.SetFieldVisible(nameof(Options.ArcheryLevel6ExtraXp), Options.ShowArcheryXp);
            Options.SetFieldVisible(nameof(Options.ArcheryLevel7ExtraXp), Options.ShowArcheryXp);
            Options.SetFieldVisible(nameof(Options.ArcheryLevel8ExtraXp), Options.ShowArcheryXp);
            Options.SetFieldVisible(nameof(Options.ArcheryLevel9ExtraXp), Options.ShowArcheryXp);
            Options.SetFieldVisible(nameof(Options.ArcheryLevel10ExtraXp), Options.ShowArcheryXp);

            Options.SetFieldVisible(nameof(Options.MendingLevel6ExtraXp), Options.ShowMendingXp);
            Options.SetFieldVisible(nameof(Options.MendingLevel7ExtraXp), Options.ShowMendingXp);
            Options.SetFieldVisible(nameof(Options.MendingLevel8ExtraXp), Options.ShowMendingXp);
            Options.SetFieldVisible(nameof(Options.MendingLevel9ExtraXp), Options.ShowMendingXp);
            Options.SetFieldVisible(nameof(Options.MendingLevel10ExtraXp), Options.ShowMendingXp);

            Options.SetFieldVisible(nameof(Options.RevolverLevel6ExtraXp), Options.ShowRevolverXp);
            Options.SetFieldVisible(nameof(Options.RevolverLevel7ExtraXp), Options.ShowRevolverXp);
            Options.SetFieldVisible(nameof(Options.RevolverLevel8ExtraXp), Options.ShowRevolverXp);
            Options.SetFieldVisible(nameof(Options.RevolverLevel9ExtraXp), Options.ShowRevolverXp);
            Options.SetFieldVisible(nameof(Options.RevolverLevel10ExtraXp), Options.ShowRevolverXp);

            Options.SetFieldVisible(nameof(Options.GunsmithingLevel6ExtraXp), Options.ShowGunsmithingXp);
            Options.SetFieldVisible(nameof(Options.GunsmithingLevel7ExtraXp), Options.ShowGunsmithingXp);
            Options.SetFieldVisible(nameof(Options.GunsmithingLevel8ExtraXp), Options.ShowGunsmithingXp);
            Options.SetFieldVisible(nameof(Options.GunsmithingLevel9ExtraXp), Options.ShowGunsmithingXp);
            Options.SetFieldVisible(nameof(Options.GunsmithingLevel10ExtraXp), Options.ShowGunsmithingXp);

        }

        private static void CaptureConfirmedSettings()
        {
            AppliedExtraXp.Clear();
            AppliedExtraXp[SkillType.Firestarting] = new[] { Options.FireStartingLevel6ExtraXp, Options.FireStartingLevel7ExtraXp, Options.FireStartingLevel8ExtraXp, Options.FireStartingLevel9ExtraXp, Options.FireStartingLevel10ExtraXp };
            AppliedExtraXp[SkillType.CarcassHarvesting] = new[] { Options.CarcassHarvestingLevel6ExtraXp, Options.CarcassHarvestingLevel7ExtraXp, Options.CarcassHarvestingLevel8ExtraXp, Options.CarcassHarvestingLevel9ExtraXp, Options.CarcassHarvestingLevel10ExtraXp };
            AppliedExtraXp[SkillType.IceFishing] = new[] { Options.IceFishingLevel6ExtraXp, Options.IceFishingLevel7ExtraXp, Options.IceFishingLevel8ExtraXp, Options.IceFishingLevel9ExtraXp, Options.IceFishingLevel10ExtraXp };
            AppliedExtraXp[SkillType.Cooking] = new[] { Options.CookingLevel6ExtraXp, Options.CookingLevel7ExtraXp, Options.CookingLevel8ExtraXp, Options.CookingLevel9ExtraXp, Options.CookingLevel10ExtraXp };
            AppliedExtraXp[SkillType.Rifle] = new[] { Options.RifleLevel6ExtraXp, Options.RifleLevel7ExtraXp, Options.RifleLevel8ExtraXp, Options.RifleLevel9ExtraXp, Options.RifleLevel10ExtraXp };
            AppliedExtraXp[SkillType.Archery] = new[] { Options.ArcheryLevel6ExtraXp, Options.ArcheryLevel7ExtraXp, Options.ArcheryLevel8ExtraXp, Options.ArcheryLevel9ExtraXp, Options.ArcheryLevel10ExtraXp };
            AppliedExtraXp[SkillType.ClothingRepair] = new[] { Options.MendingLevel6ExtraXp, Options.MendingLevel7ExtraXp, Options.MendingLevel8ExtraXp, Options.MendingLevel9ExtraXp, Options.MendingLevel10ExtraXp };
            AppliedExtraXp[SkillType.Revolver] = new[] { Options.RevolverLevel6ExtraXp, Options.RevolverLevel7ExtraXp, Options.RevolverLevel8ExtraXp, Options.RevolverLevel9ExtraXp, Options.RevolverLevel10ExtraXp };
            AppliedExtraXp[SkillType.Gunsmithing] = new[] { Options.GunsmithingLevel6ExtraXp, Options.GunsmithingLevel7ExtraXp, Options.GunsmithingLevel8ExtraXp, Options.GunsmithingLevel9ExtraXp, Options.GunsmithingLevel10ExtraXp };
        }

        private static int GetDefaultExtraXp(SkillType skillType, int level)
        {
            int vanilla = GetVanillaLevelFourToFiveXp(skillType);
            return vanilla * (level - 5) / 2;
        }

    }
}
