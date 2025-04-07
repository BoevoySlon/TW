using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000198 RID: 408
	public abstract class SettlementEconomyModel : GameModel
	{
		// Token: 0x06001A72 RID: 6770
		public abstract float GetEstimatedDemandForCategory(Town town, ItemData itemData, ItemCategory category);

		// Token: 0x06001A73 RID: 6771
		public abstract float GetDailyDemandForCategory(Town town, ItemCategory category, int extraProsperity = 0);

		// Token: 0x06001A74 RID: 6772
		public abstract float GetDemandChangeFromValue(float purchaseValue);

		// Token: 0x06001A75 RID: 6773
		public abstract ValueTuple<float, float> GetSupplyDemandForCategory(Town town, ItemCategory category, float dailySupply, float dailyDemand, float oldSupply, float oldDemand);

		// Token: 0x06001A76 RID: 6774
		public abstract int GetTownGoldChange(Town town);
	}
}
