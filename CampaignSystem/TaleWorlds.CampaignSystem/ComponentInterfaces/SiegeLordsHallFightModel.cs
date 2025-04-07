using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B8 RID: 440
	public abstract class SiegeLordsHallFightModel : GameModel
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001B63 RID: 7011
		public abstract float AreaLostRatio { get; }

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001B64 RID: 7012
		public abstract float AttackerDefenderTroopCountRatio { get; }

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001B65 RID: 7013
		public abstract int DefenderTroopNumberForSuccessfulPullBack { get; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001B66 RID: 7014
		public abstract float DefenderMaxArcherRatio { get; }

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001B67 RID: 7015
		public abstract int MaxDefenderSideTroopCount { get; }

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001B68 RID: 7016
		public abstract int MaxDefenderArcherCount { get; }

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06001B69 RID: 7017
		public abstract int MaxAttackerSideTroopCount { get; }

		// Token: 0x06001B6A RID: 7018
		public abstract FlattenedTroopRoster GetPriorityListForLordsHallFightMission(MapEvent playerMapEvent, BattleSideEnum side, int troopCount);
	}
}
