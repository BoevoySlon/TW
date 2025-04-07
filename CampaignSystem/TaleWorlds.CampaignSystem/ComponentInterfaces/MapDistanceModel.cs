using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200016E RID: 366
	public abstract class MapDistanceModel : GameModel
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06001930 RID: 6448
		// (set) Token: 0x06001931 RID: 6449
		public abstract float MaximumDistanceBetweenTwoSettlements { get; set; }

		// Token: 0x06001932 RID: 6450
		public abstract float GetDistance(Settlement fromSettlement, Settlement toSettlement);

		// Token: 0x06001933 RID: 6451
		public abstract float GetDistance(MobileParty fromParty, Settlement toSettlement);

		// Token: 0x06001934 RID: 6452
		public abstract float GetDistance(MobileParty fromParty, MobileParty toParty);

		// Token: 0x06001935 RID: 6453
		public abstract bool GetDistance(Settlement fromSettlement, Settlement toSettlement, float maximumDistance, out float distance);

		// Token: 0x06001936 RID: 6454
		public abstract bool GetDistance(MobileParty fromParty, Settlement toSettlement, float maximumDistance, out float distance);

		// Token: 0x06001937 RID: 6455
		public abstract bool GetDistance(IMapPoint fromMapPoint, MobileParty toParty, float maximumDistance, out float distance);

		// Token: 0x06001938 RID: 6456
		public abstract bool GetDistance(IMapPoint fromMapPoint, Settlement toSettlement, float maximumDistance, out float distance);

		// Token: 0x06001939 RID: 6457
		public abstract bool GetDistance(IMapPoint fromMapPoint, in Vec2 toPoint, float maximumDistance, out float distance);

		// Token: 0x0600193A RID: 6458
		public abstract Settlement GetClosestSettlementForNavigationMesh(PathFaceRecord face);
	}
}
