using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C4 RID: 452
	public abstract class BannerItemModel : GameModel
	{
		// Token: 0x06001BB8 RID: 7096
		public abstract IEnumerable<ItemObject> GetPossibleRewardBannerItems();

		// Token: 0x06001BB9 RID: 7097
		public abstract IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero);

		// Token: 0x06001BBA RID: 7098
		public abstract int GetBannerItemLevelForHero(Hero hero);

		// Token: 0x06001BBB RID: 7099
		public abstract bool CanBannerBeUpdated(ItemObject item);
	}
}
