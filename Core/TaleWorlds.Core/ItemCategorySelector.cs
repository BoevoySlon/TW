using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006C RID: 108
	public abstract class ItemCategorySelector : GameModel
	{
		// Token: 0x0600072D RID: 1837
		public abstract ItemCategory GetItemCategoryForItem(ItemObject itemObject);
	}
}
