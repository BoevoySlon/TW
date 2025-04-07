using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000197 RID: 407
	public abstract class SettlementFoodModel : GameModel
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001A6C RID: 6764
		public abstract int FoodStocksUpperLimit { get; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001A6D RID: 6765
		public abstract int NumberOfProsperityToEatOneFood { get; }

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001A6E RID: 6766
		public abstract int NumberOfMenOnGarrisonToEatOneFood { get; }

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001A6F RID: 6767
		public abstract int CastleFoodStockUpperLimitBonus { get; }

		// Token: 0x06001A70 RID: 6768
		public abstract ExplainedNumber CalculateTownFoodStocksChange(Town town, bool includeMarketStocks = true, bool includeDescriptions = false);
	}
}
