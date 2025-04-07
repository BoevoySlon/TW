using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200016B RID: 363
	public abstract class ItemDiscardModel : GameModel
	{
		// Token: 0x06001924 RID: 6436
		public abstract int GetXpBonusForDiscardingItems(ItemRoster itemRoster);

		// Token: 0x06001925 RID: 6437
		public abstract int GetXpBonusForDiscardingItem(ItemObject item, int amount = 1);

		// Token: 0x06001926 RID: 6438
		public abstract bool PlayerCanDonateItem(ItemObject item);
	}
}
