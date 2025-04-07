using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x020001D0 RID: 464
	public class CharacterCreationCategory
	{
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001BF2 RID: 7154 RVA: 0x0007ED5C File Offset: 0x0007CF5C
		// (set) Token: 0x06001BF3 RID: 7155 RVA: 0x0007ED64 File Offset: 0x0007CF64
		public CharacterCreationOnCondition CategoryCondition { get; private set; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001BF4 RID: 7156 RVA: 0x0007ED6D File Offset: 0x0007CF6D
		// (set) Token: 0x06001BF5 RID: 7157 RVA: 0x0007ED75 File Offset: 0x0007CF75
		public List<CharacterCreationOption> CharacterCreationOptions { get; private set; }

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0007ED7E File Offset: 0x0007CF7E
		public CharacterCreationCategory(CharacterCreationOnCondition condition = null)
		{
			this.CategoryCondition = condition;
			this.CharacterCreationOptions = new List<CharacterCreationOption>();
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x0007ED98 File Offset: 0x0007CF98
		public void AddCategoryOption(TextObject text, MBList<SkillObject> effectedSkills, CharacterAttribute effectedAttribute, int focusToAdd, int skillLevelToAdd, int attributeLevelToAdd, CharacterCreationOnCondition optionCondition, CharacterCreationOnSelect onSelect, CharacterCreationApplyFinalEffects onApply, TextObject descriptionText = null, MBList<TraitObject> effectedTraits = null, int traitLevelToAdd = 0, int renownToAdd = 0, int goldToAdd = 0, int unspentFocusPoint = 0, int unspentAttributePoint = 0)
		{
			CharacterCreationOption item = new CharacterCreationOption(this.CharacterCreationOptions.Count + 1, effectedSkills, effectedAttribute, focusToAdd, skillLevelToAdd, attributeLevelToAdd, text, optionCondition, onSelect, onApply, descriptionText, effectedTraits, traitLevelToAdd, renownToAdd, goldToAdd, unspentFocusPoint, unspentAttributePoint);
			this.CharacterCreationOptions.Add(item);
		}
	}
}
