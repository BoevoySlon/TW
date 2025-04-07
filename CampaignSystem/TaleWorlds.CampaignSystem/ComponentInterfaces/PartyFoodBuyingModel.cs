using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000182 RID: 386
	public abstract class PartyFoodBuyingModel : GameModel
	{
		// Token: 0x060019E4 RID: 6628
		public abstract void FindItemToBuy(MobileParty mobileParty, Settlement settlement, out ItemRosterElement itemRosterElement, out float itemElementsPrice);

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060019E5 RID: 6629
		public abstract float MinimumDaysFoodToLastWhileBuyingFoodFromTown { get; }

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060019E6 RID: 6630
		public abstract float MinimumDaysFoodToLastWhileBuyingFoodFromVillage { get; }

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060019E7 RID: 6631
		public abstract float LowCostFoodPriceAverage { get; }
	}
}
