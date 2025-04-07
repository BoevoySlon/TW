using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x02000151 RID: 337
	public static class ItemObjectExtensions
	{
		// Token: 0x06001868 RID: 6248 RVA: 0x0007C662 File Offset: 0x0007A862
		public static ItemCategory GetItemCategory(this ItemObject item)
		{
			return item.ItemCategory;
		}
	}
}
