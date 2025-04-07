using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x02000365 RID: 869
	public interface IMarketData
	{
		// Token: 0x060032E3 RID: 13027
		int GetPrice(ItemObject item, MobileParty tradingParty, bool isSelling, PartyBase merchantParty);

		// Token: 0x060032E4 RID: 13028
		int GetPrice(EquipmentElement itemRosterElement, MobileParty tradingParty, bool isSelling, PartyBase merchantParty);
	}
}
