using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000195 RID: 405
	public abstract class TradeItemPriceFactorModel : GameModel
	{
		// Token: 0x06001A62 RID: 6754
		public abstract float GetTradePenalty(ItemObject item, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStore, float supply, float demand);

		// Token: 0x06001A63 RID: 6755
		public abstract float GetBasePriceFactor(ItemCategory itemCategory, float inStoreValue, float supply, float demand, bool isSelling, int transferValue);

		// Token: 0x06001A64 RID: 6756
		public abstract int GetPrice(EquipmentElement itemRosterElement, MobileParty clientParty, PartyBase merchant, bool isSelling, float inStoreValue, float supply, float demand);

		// Token: 0x06001A65 RID: 6757
		public abstract int GetTheoreticalMaxItemMarketValue(ItemObject item);
	}
}
