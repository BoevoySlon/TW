using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019D RID: 413
	public abstract class SettlementSecurityModel : GameModel
	{
		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06001AA3 RID: 6819
		public abstract int MaximumSecurityInSettlement { get; }

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06001AA4 RID: 6820
		public abstract int SecurityDriftMedium { get; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06001AA5 RID: 6821
		public abstract float MapEventSecurityEffectRadius { get; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001AA6 RID: 6822
		public abstract float HideoutClearedSecurityEffectRadius { get; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001AA7 RID: 6823
		public abstract int HideoutClearedSecurityGain { get; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001AA8 RID: 6824
		public abstract int ThresholdForTaxCorruption { get; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001AA9 RID: 6825
		public abstract int ThresholdForHigherTaxCorruption { get; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001AAA RID: 6826
		public abstract int ThresholdForTaxBoost { get; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001AAB RID: 6827
		public abstract int SettlementTaxBoostPercentage { get; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001AAC RID: 6828
		public abstract int SettlementTaxPenaltyPercentage { get; }

		// Token: 0x06001AAD RID: 6829
		public abstract float GetLootedNearbyPartySecurityEffect(Town town, float sumOfAttackedPartyStrengths);

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001AAE RID: 6830
		public abstract int ThresholdForNotableRelationBonus { get; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001AAF RID: 6831
		public abstract int ThresholdForNotableRelationPenalty { get; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001AB0 RID: 6832
		public abstract int DailyNotableRelationBonus { get; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06001AB1 RID: 6833
		public abstract int DailyNotableRelationPenalty { get; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06001AB2 RID: 6834
		public abstract int DailyNotablePowerBonus { get; }

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06001AB3 RID: 6835
		public abstract int DailyNotablePowerPenalty { get; }

		// Token: 0x06001AB4 RID: 6836
		public abstract ExplainedNumber CalculateSecurityChange(Town town, bool includeDescriptions = false);

		// Token: 0x06001AB5 RID: 6837
		public abstract float GetNearbyBanditPartyDefeatedSecurityEffect(Town town, float sumOfAttackedPartyStrengths);

		// Token: 0x06001AB6 RID: 6838
		public abstract void CalculateGoldGainDueToHighSecurity(Town town, ref ExplainedNumber explainedNumber);

		// Token: 0x06001AB7 RID: 6839
		public abstract void CalculateGoldCutDueToLowSecurity(Town town, ref ExplainedNumber explainedNumber);
	}
}
