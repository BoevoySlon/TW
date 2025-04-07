using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A5 RID: 421
	public abstract class ClanFinanceModel : GameModel
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06001AE8 RID: 6888
		public abstract int PartyGoldLowerThreshold { get; }

		// Token: 0x06001AE9 RID: 6889
		public abstract ExplainedNumber CalculateClanGoldChange(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001AEA RID: 6890
		public abstract ExplainedNumber CalculateClanIncome(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001AEB RID: 6891
		public abstract ExplainedNumber CalculateClanExpenses(Clan clan, bool includeDescriptions = false, bool applyWithdrawals = false, bool includeDetails = false);

		// Token: 0x06001AEC RID: 6892
		public abstract ExplainedNumber CalculateTownIncomeFromTariffs(Clan clan, Town town, bool applyWithdrawals = false);

		// Token: 0x06001AED RID: 6893
		public abstract int CalculateTownIncomeFromProjects(Town town);

		// Token: 0x06001AEE RID: 6894
		public abstract int CalculateVillageIncome(Clan clan, Village village, bool applyWithdrawals = false);

		// Token: 0x06001AEF RID: 6895
		public abstract int CalculateNotableDailyGoldChange(Hero hero, bool applyWithdrawals);

		// Token: 0x06001AF0 RID: 6896
		public abstract int CalculateOwnerIncomeFromCaravan(MobileParty caravan);

		// Token: 0x06001AF1 RID: 6897
		public abstract int CalculateOwnerIncomeFromWorkshop(Workshop workshop);

		// Token: 0x06001AF2 RID: 6898
		public abstract float RevenueSmoothenFraction();
	}
}
