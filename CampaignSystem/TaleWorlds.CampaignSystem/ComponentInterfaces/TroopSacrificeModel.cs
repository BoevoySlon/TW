using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AF RID: 431
	public abstract class TroopSacrificeModel : GameModel
	{
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06001B1F RID: 6943
		public abstract int BreakOutArmyLeaderRelationPenalty { get; }

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06001B20 RID: 6944
		public abstract int BreakOutArmyMemberRelationPenalty { get; }

		// Token: 0x06001B21 RID: 6945
		public abstract int GetLostTroopCountForBreakingInBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent);

		// Token: 0x06001B22 RID: 6946
		public abstract int GetLostTroopCountForBreakingOutOfBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent);

		// Token: 0x06001B23 RID: 6947
		public abstract int GetNumberOfTroopsSacrificedForTryingToGetAway(BattleSideEnum battleSide, MapEvent mapEvent);
	}
}
