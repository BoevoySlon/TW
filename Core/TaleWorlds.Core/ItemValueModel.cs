using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006D RID: 109
	public abstract class ItemValueModel : GameModel
	{
		// Token: 0x0600072F RID: 1839
		public abstract float GetEquipmentValueFromTier(float itemTierf);

		// Token: 0x06000730 RID: 1840
		public abstract float CalculateTier(ItemObject item);

		// Token: 0x06000731 RID: 1841
		public abstract int CalculateValue(ItemObject item);
	}
}
