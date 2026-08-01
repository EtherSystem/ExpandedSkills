using ExpandedSkills.Framework;

namespace ExpandedSkills.UI
{
    internal static class SkillPanelPresentationRenderer
    {
        internal static void RenderSelected(Panel_Log panel)
        {
            if (!TryGetSelectedSkill(panel, out Skill skill)) return;
            if (!ExpandedSkillRegistry.TryGet(skill, out ExpandedSkillDefinition definition)) return;
            SkillPanelPresentation presentation = definition.BuildPresentation(skill);
            if (presentation == null) return;

            if (panel.m_SkillName != null) panel.m_SkillName.text = presentation.SkillName;
            if (panel.m_SkillLevelIconLargeLabel != null) panel.m_SkillLevelIconLargeLabel.text = presentation.DisplayLevel.ToString();
            if (panel.m_SkillLevelName != null) panel.m_SkillLevelName.text = presentation.RankName;
            if (panel.m_SkillDescription != null) panel.m_SkillDescription.text = presentation.Description;

            RenderBenefits(panel, presentation);
        }

        private static bool TryGetSelectedSkill(Panel_Log panel, out Skill skill)
        {
            skill = null;
            if (panel == null || panel.m_SkillsDisplayList == null) return false;
            if (panel.m_SkillListSelectedIndex < 0 || panel.m_SkillListSelectedIndex >= panel.m_SkillsDisplayList.Count) return false;

            SkillListItem item = panel.m_SkillsDisplayList[panel.m_SkillListSelectedIndex];
            if (item == null || item.m_Skill == null) return false;

            skill = item.m_Skill;
            return true;
        }

        private static void RenderBenefits(Panel_Log panel, SkillPanelPresentation presentation)
        {
            if (panel.m_SkillDescription == null || panel.m_SkillBenefitsStartDummy == null || panel.m_SkillBenefitPrefab == null || panel.m_SkillBenefitLines == null) return;

            Vector3 startPosition = panel.m_SkillDescription.transform.localPosition;
            startPosition.y -= panel.m_SkillDescription.printedSize.y;
            startPosition.y -= panel.m_DescriptionOffsetY;
            panel.m_SkillBenefitsStartDummy.transform.localPosition = startPosition;

            EnsureBenefitLineCount(panel, presentation.Benefits.Count);

            for (int i = 0; i < panel.m_SkillBenefitLines.Count; i++)
            {
                GameObject line = panel.m_SkillBenefitLines[i];
                if (line != null) line.SetActive(false);
            }

            SkillBenefitItem prefabItem = panel.m_SkillBenefitPrefab.GetComponent<SkillBenefitItem>();
            int baseHeight = prefabItem == null || prefabItem.m_Background == null ? 1 : prefabItem.m_Background.height;
            Vector3 linePosition = startPosition;

            for (int i = 0; i < presentation.Benefits.Count; i++)
            {
                GameObject line = panel.m_SkillBenefitLines[i];
                if (line == null) continue;

                SkillBenefitItem item = line.GetComponent<SkillBenefitItem>();
                if (item == null || item.m_LabelBenifit == null || item.m_Background == null) continue;

                line.SetActive(true);
                line.transform.localPosition = linePosition;
                line.transform.localScale = Vector3.one;
                item.m_LabelBenifit.text = presentation.Benefits[i];

                string processedText = item.m_LabelBenifit.processedText ?? string.Empty;
                int lineCount = processedText.Split('\n').Length;
                item.m_Background.height = Mathf.Max(1, lineCount) * baseHeight;

                Vector3 labelPosition = item.m_LabelBenifit.transform.localPosition;
                labelPosition.y = -item.m_Background.height / 2f + panel.m_SkillBenefitLabelOffsetY;
                item.m_LabelBenifit.transform.localPosition = labelPosition;

                linePosition.y -= item.m_Background.height + panel.m_SkillBenefitSpacingY;
            }
        }

        private static void EnsureBenefitLineCount(Panel_Log panel, int requiredCount)
        {
            while (panel.m_SkillBenefitLines.Count < requiredCount)
            {
                GameObject line = UnityEngine.Object.Instantiate<GameObject>(panel.m_SkillBenefitPrefab);
                line.transform.parent = panel.m_SkillBenefitsStartDummy.transform.parent;
                line.transform.localScale = Vector3.one;
                panel.m_SkillBenefitLines.Add(line);
            }
        }
    }
}