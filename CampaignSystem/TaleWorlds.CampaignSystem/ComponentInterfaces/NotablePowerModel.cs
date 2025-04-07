using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B1 RID: 433
	public abstract class NotablePowerModel : GameModel
	{
		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001B2C RID: 6956
		public abstract int RegularNotableMaxPowerLevel { get; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001B2D RID: 6957
		public abstract int NotableDisappearPowerLimit { get; }

		// Token: 0x06001B2E RID: 6958
		public abstract ExplainedNumber CalculateDailyPowerChangeForHero(Hero hero, bool includeDescriptions = false);

		// Token: 0x06001B2F RID: 6959
		public abstract TextObject GetPowerRankName(Hero hero);

		// Token: 0x06001B30 RID: 6960
		public abstract float GetInfluenceBonusToClan(Hero hero);

		// Token: 0x06001B31 RID: 6961
		public abstract int GetInitialPower();
	}
}
