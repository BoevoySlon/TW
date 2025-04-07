using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A0 RID: 416
	public abstract class SettlementTaxModel : GameModel
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001AC1 RID: 6849
		public abstract float SettlementCommissionRateTown { get; }

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001AC2 RID: 6850
		public abstract float SettlementCommissionRateVillage { get; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001AC3 RID: 6851
		public abstract int SettlementCommissionDecreaseSecurityThreshold { get; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001AC4 RID: 6852
		public abstract int MaximumDecreaseBasedOnSecuritySecurity { get; }

		// Token: 0x06001AC5 RID: 6853
		public abstract float GetTownTaxRatio(Town town);

		// Token: 0x06001AC6 RID: 6854
		public abstract float GetVillageTaxRatio();

		// Token: 0x06001AC7 RID: 6855
		public abstract float GetTownCommissionChangeBasedOnSecurity(Town town, float commission);

		// Token: 0x06001AC8 RID: 6856
		public abstract ExplainedNumber CalculateTownTax(Town town, bool includeDescriptions = false);

		// Token: 0x06001AC9 RID: 6857
		public abstract int CalculateVillageTaxFromIncome(Village village, int marketIncome);
	}
}
