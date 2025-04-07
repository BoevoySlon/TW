using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A2 RID: 418
	public abstract class VassalRewardsModel : GameModel
	{
		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001ACE RID: 6862
		public abstract float InfluenceReward { get; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001ACF RID: 6863
		public abstract int RelationRewardWithLeader { get; }

		// Token: 0x06001AD0 RID: 6864
		public abstract TroopRoster GetTroopRewardsForJoiningKingdom(Kingdom kingdom);

		// Token: 0x06001AD1 RID: 6865
		public abstract ItemRoster GetEquipmentRewardsForJoiningKingdom(Kingdom kingdom);
	}
}
