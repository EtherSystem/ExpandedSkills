namespace ExpandedSkills.Framework
{
    internal static class SkillPanelPresentationBuilder
    {
        internal static SkillPanelPresentation Build(Skill skill, SkillType expectedSkillType)
        {
            if (skill == null || skill.m_SkillType != expectedSkillType) return null;
            if (!ExpandedSkillRegistry.TryGet(skill, out ExpandedSkillDefinition definition)) return null;

            int displayLevel = ExpandedSkillProgression.GetRealLevel(skill);
            int rewardTierIndex = ExpandedSkillProgression.GetRewardTierIndex(skill);
            SkillsManager skillsManager = GameManager.GetSkillsManager();

            string rankName = skillsManager == null ? string.Empty : skillsManager.GetTierName(rewardTierIndex);
            string description = skill.GetTierDescription(rewardTierIndex) ?? string.Empty;
            List<string> benefits = SplitBenefits(ExpandedSkillRewardData.GetBenefits(skill, displayLevel - 1));
            if (displayLevel >= ExpandedSkillProgression.MaxLevel && definition.Level10Benefit.Length > 0) benefits.Add(definition.Level10Benefit);

            return new SkillPanelPresentation(skill.m_DisplayName, displayLevel, rankName, description, benefits);
        }

        private static List<string> SplitBenefits(string text)
        {
            List<string> benefits = new();
            if (string.IsNullOrEmpty(text)) return benefits;

            string[] lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length > 0) benefits.Add(line);
            }

            return benefits;
        }
    }
}