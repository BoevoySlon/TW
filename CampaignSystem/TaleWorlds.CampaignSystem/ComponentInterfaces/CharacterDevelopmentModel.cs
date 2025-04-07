using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000169 RID: 361
	public abstract class CharacterDevelopmentModel : GameModel
	{
		// Token: 0x06001908 RID: 6408
		public abstract List<Tuple<SkillObject, int>> GetSkillsDerivedFromTraits(Hero hero = null, CharacterObject templateCharacter = null, bool isByNaturalGrowth = false);

		// Token: 0x06001909 RID: 6409
		public abstract int SkillsRequiredForLevel(int level);

		// Token: 0x0600190A RID: 6410
		public abstract int GetMaxSkillPoint();

		// Token: 0x0600190B RID: 6411
		public abstract int GetXpRequiredForSkillLevel(int skillLevel);

		// Token: 0x0600190C RID: 6412
		public abstract int GetSkillLevelChange(Hero hero, SkillObject skill, float skillXp);

		// Token: 0x0600190D RID: 6413
		public abstract int GetXpAmountForSkillLevelChange(Hero hero, SkillObject skill, int skillLevelChange);

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x0600190E RID: 6414
		public abstract int MaxAttribute { get; }

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x0600190F RID: 6415
		public abstract int MaxFocusPerSkill { get; }

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06001910 RID: 6416
		public abstract int MaxSkillRequiredForEpicPerkBonus { get; }

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06001911 RID: 6417
		public abstract int MinSkillRequiredForEpicPerkBonus { get; }

		// Token: 0x06001912 RID: 6418
		public abstract void GetTraitLevelForTraitXp(Hero hero, TraitObject trait, int newValue, out int traitLevel, out int traitXp);

		// Token: 0x06001913 RID: 6419
		public abstract int GetTraitXpRequiredForTraitLevel(TraitObject trait, int traitLevel);

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06001914 RID: 6420
		public abstract int FocusPointsPerLevel { get; }

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06001915 RID: 6421
		public abstract int FocusPointsAtStart { get; }

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06001916 RID: 6422
		public abstract int AttributePointsAtStart { get; }

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06001917 RID: 6423
		public abstract int LevelsPerAttributePoint { get; }

		// Token: 0x06001918 RID: 6424
		public abstract ExplainedNumber CalculateLearningLimit(int attributeValue, int focusValue, TextObject attributeName, bool includeDescriptions = false);

		// Token: 0x06001919 RID: 6425
		public abstract float CalculateLearningRate(Hero hero, SkillObject skill);

		// Token: 0x0600191A RID: 6426
		public abstract ExplainedNumber CalculateLearningRate(int attributeValue, int focusValue, int skillValue, int characterLevel, TextObject attributeName, bool includeDescriptions = false);

		// Token: 0x0600191B RID: 6427
		public abstract SkillObject GetNextSkillToAddFocus(Hero hero);

		// Token: 0x0600191C RID: 6428
		public abstract CharacterAttribute GetNextAttributeToUpgrade(Hero hero);

		// Token: 0x0600191D RID: 6429
		public abstract PerkObject GetNextPerkToChoose(Hero hero, PerkObject perk);
	}
}
