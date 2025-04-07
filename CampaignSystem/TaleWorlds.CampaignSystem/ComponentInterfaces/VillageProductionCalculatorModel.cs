using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000184 RID: 388
	public abstract class VillageProductionCalculatorModel : GameModel
	{
		// Token: 0x060019EE RID: 6638
		public abstract float CalculateProductionSpeedOfItemCategory(ItemCategory item);

		// Token: 0x060019EF RID: 6639
		public abstract float CalculateDailyProductionAmount(Village village, ItemObject item);

		// Token: 0x060019F0 RID: 6640
		public abstract float CalculateDailyFoodProductionAmount(Village village);
	}
}
