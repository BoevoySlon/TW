using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200014F RID: 335
	public static class ItemCategories
	{
		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06001866 RID: 6246 RVA: 0x0007C64A File Offset: 0x0007A84A
		public static MBReadOnlyList<ItemCategory> All
		{
			get
			{
				return Campaign.Current.AllItemCategories;
			}
		}
	}
}
