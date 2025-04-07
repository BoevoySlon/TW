using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019C RID: 412
	public abstract class SettlementLoyaltyModel : GameModel
	{
		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001A88 RID: 6792
		public abstract int SettlementLoyaltyChangeDueToSecurityThreshold { get; }

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001A89 RID: 6793
		public abstract int MaximumLoyaltyInSettlement { get; }

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001A8A RID: 6794
		public abstract int LoyaltyDriftMedium { get; }

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001A8B RID: 6795
		public abstract float HighLoyaltyProsperityEffect { get; }

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001A8C RID: 6796
		public abstract int LowLoyaltyProsperityEffect { get; }

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001A8D RID: 6797
		public abstract int MilitiaBoostPercentage { get; }

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001A8E RID: 6798
		public abstract float HighSecurityLoyaltyEffect { get; }

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001A8F RID: 6799
		public abstract float LowSecurityLoyaltyEffect { get; }

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001A90 RID: 6800
		public abstract float GovernorSameCultureLoyaltyEffect { get; }

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001A91 RID: 6801
		public abstract float GovernorDifferentCultureLoyaltyEffect { get; }

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001A92 RID: 6802
		public abstract float SettlementOwnerDifferentCultureLoyaltyEffect { get; }

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001A93 RID: 6803
		public abstract int ThresholdForTaxBoost { get; }

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06001A94 RID: 6804
		public abstract int RebellionStartLoyaltyThreshold { get; }

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06001A95 RID: 6805
		public abstract int ThresholdForTaxCorruption { get; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06001A96 RID: 6806
		public abstract int ThresholdForHigherTaxCorruption { get; }

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06001A97 RID: 6807
		public abstract int ThresholdForProsperityBoost { get; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06001A98 RID: 6808
		public abstract int ThresholdForProsperityPenalty { get; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06001A99 RID: 6809
		public abstract int AdditionalStarvationPenaltyStartDay { get; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06001A9A RID: 6810
		public abstract int AdditionalStarvationLoyaltyEffect { get; }

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001A9B RID: 6811
		public abstract int RebelliousStateStartLoyaltyThreshold { get; }

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06001A9C RID: 6812
		public abstract int LoyaltyBoostAfterRebellionStartValue { get; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001A9D RID: 6813
		public abstract float ThresholdForNotableRelationBonus { get; }

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06001A9E RID: 6814
		public abstract int DailyNotableRelationBonus { get; }

		// Token: 0x06001A9F RID: 6815
		public abstract ExplainedNumber CalculateLoyaltyChange(Town town, bool includeDescriptions = false);

		// Token: 0x06001AA0 RID: 6816
		public abstract void CalculateGoldGainDueToHighLoyalty(Town town, ref ExplainedNumber explainedNumber);

		// Token: 0x06001AA1 RID: 6817
		public abstract void CalculateGoldCutDueToLowLoyalty(Town town, ref ExplainedNumber explainedNumber);
	}
}
