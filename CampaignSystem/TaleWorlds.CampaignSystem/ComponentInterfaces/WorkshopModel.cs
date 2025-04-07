using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BF RID: 447
	public abstract class WorkshopModel : GameModel
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06001B8D RID: 7053
		public abstract int DaysForPlayerSaveWorkshopFromBankruptcy { get; }

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06001B8E RID: 7054
		public abstract int CapitalLowLimit { get; }

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06001B8F RID: 7055
		public abstract int InitialCapital { get; }

		// Token: 0x06001B90 RID: 7056
		public abstract int GetMaxWorkshopCountForClanTier(int tier);

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06001B91 RID: 7057
		public abstract int DailyExpense { get; }

		// Token: 0x06001B92 RID: 7058
		public abstract int GetCostForPlayer(Workshop workshop);

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06001B93 RID: 7059
		public abstract int WarehouseCapacity { get; }

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001B94 RID: 7060
		public abstract int DefaultWorkshopCountInSettlement { get; }

		// Token: 0x06001B95 RID: 7061
		public abstract int GetCostForNotable(Workshop workshop);

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001B96 RID: 7062
		public abstract int MaximumWorkshopsPlayerCanHave { get; }

		// Token: 0x06001B97 RID: 7063
		public abstract ExplainedNumber GetEffectiveConversionSpeedOfProduction(Workshop workshop, float speed, bool includeDescriptions);

		// Token: 0x06001B98 RID: 7064
		public abstract Hero GetNotableOwnerForWorkshop(Workshop workshop);

		// Token: 0x06001B99 RID: 7065
		public abstract int GetConvertProductionCost(WorkshopType workshopType);

		// Token: 0x06001B9A RID: 7066
		public abstract bool CanPlayerSellWorkshop(Workshop workshop, out TextObject explanation);

		// Token: 0x06001B9B RID: 7067
		public abstract float GetTradeXpPerWarehouseProduction(EquipmentElement production);
	}
}
