using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AE RID: 430
	public abstract class AgeModel : GameModel
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06001B16 RID: 6934
		public abstract int BecomeInfantAge { get; }

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06001B17 RID: 6935
		public abstract int BecomeChildAge { get; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06001B18 RID: 6936
		public abstract int BecomeTeenagerAge { get; }

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001B19 RID: 6937
		public abstract int HeroComesOfAge { get; }

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001B1A RID: 6938
		public abstract int BecomeOldAge { get; }

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001B1B RID: 6939
		public abstract int MaxAge { get; }

		// Token: 0x06001B1C RID: 6940
		public abstract void GetAgeLimitForLocation(CharacterObject character, out int minimumAge, out int maximumAge, string additionalTags = "");

		// Token: 0x06001B1D RID: 6941
		public abstract float GetSkillScalingModifierForAge(Hero hero, SkillObject skill, bool isByNaturalGrowth);
	}
}
