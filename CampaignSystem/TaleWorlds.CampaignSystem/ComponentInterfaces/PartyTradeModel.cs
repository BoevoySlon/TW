using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000171 RID: 369
	public abstract class PartyTradeModel : GameModel
	{
		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06001946 RID: 6470
		public abstract int CaravanTransactionHighestValueItemCount { get; }

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06001947 RID: 6471
		public abstract int SmallCaravanFormingCostForPlayer { get; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06001948 RID: 6472
		public abstract int LargeCaravanFormingCostForPlayer { get; }

		// Token: 0x06001949 RID: 6473
		public abstract float GetTradePenaltyFactor(MobileParty party);
	}
}
