using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B0 RID: 432
	public abstract class PregnancyModel : GameModel
	{
		// Token: 0x06001B25 RID: 6949
		public abstract float GetDailyChanceOfPregnancyForHero(Hero hero);

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06001B26 RID: 6950
		public abstract float PregnancyDurationInDays { get; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06001B27 RID: 6951
		public abstract float MaternalMortalityProbabilityInLabor { get; }

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001B28 RID: 6952
		public abstract float StillbirthProbability { get; }

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06001B29 RID: 6953
		public abstract float DeliveringFemaleOffspringProbability { get; }

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06001B2A RID: 6954
		public abstract float DeliveringTwinsProbability { get; }
	}
}
