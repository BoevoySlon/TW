using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200017E RID: 382
	public abstract class MinorFactionsModel : GameModel
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600199F RID: 6559
		public abstract float DailyMinorFactionHeroSpawnChance { get; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060019A0 RID: 6560
		public abstract int MinorFactionHeroLimit { get; }

		// Token: 0x060019A1 RID: 6561
		public abstract int GetMercenaryAwardFactorToJoinKingdom(Clan mercenaryClan, Kingdom kingdom, bool neededAmountForClanToJoinCalculation = false);
	}
}
