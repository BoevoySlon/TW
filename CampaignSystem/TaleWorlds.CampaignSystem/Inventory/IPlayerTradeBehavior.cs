using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000CC RID: 204
	public interface IPlayerTradeBehavior
	{
		// Token: 0x060012CA RID: 4810
		int GetProjectedProfit(ItemRosterElement itemRosterElement, int itemCost);
	}
}
