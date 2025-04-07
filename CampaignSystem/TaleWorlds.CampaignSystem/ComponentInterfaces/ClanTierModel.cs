using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A3 RID: 419
	public abstract class ClanTierModel : GameModel
	{
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001AD3 RID: 6867
		public abstract int MinClanTier { get; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06001AD4 RID: 6868
		public abstract int MaxClanTier { get; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06001AD5 RID: 6869
		public abstract int MercenaryEligibleTier { get; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06001AD6 RID: 6870
		public abstract int VassalEligibleTier { get; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06001AD7 RID: 6871
		public abstract int BannerEligibleTier { get; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06001AD8 RID: 6872
		public abstract int RebelClanStartingTier { get; }

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06001AD9 RID: 6873
		public abstract int CompanionToLordClanStartingTier { get; }

		// Token: 0x06001ADA RID: 6874
		public abstract int CalculateInitialRenown(Clan clan);

		// Token: 0x06001ADB RID: 6875
		public abstract int CalculateInitialInfluence(Clan clan);

		// Token: 0x06001ADC RID: 6876
		public abstract int CalculateTier(Clan clan);

		// Token: 0x06001ADD RID: 6877
		public abstract ValueTuple<ExplainedNumber, bool> HasUpcomingTier(Clan clan, out TextObject extraExplanation, bool includeDescriptions = false);

		// Token: 0x06001ADE RID: 6878
		public abstract int GetRequiredRenownForTier(int tier);

		// Token: 0x06001ADF RID: 6879
		public abstract int GetPartyLimitForTier(Clan clan, int clanTierToCheck);

		// Token: 0x06001AE0 RID: 6880
		public abstract int GetCompanionLimit(Clan clan);
	}
}
