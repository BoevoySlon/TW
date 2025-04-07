using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000188 RID: 392
	public abstract class BanditDensityModel : GameModel
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001A03 RID: 6659
		public abstract int NumberOfMaximumLooterParties { get; }

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001A04 RID: 6660
		public abstract int NumberOfMinimumBanditPartiesInAHideoutToInfestIt { get; }

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001A05 RID: 6661
		public abstract int NumberOfMaximumBanditPartiesInEachHideout { get; }

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001A06 RID: 6662
		public abstract int NumberOfMaximumBanditPartiesAroundEachHideout { get; }

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001A07 RID: 6663
		public abstract int NumberOfMaximumHideoutsAtEachBanditFaction { get; }

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001A08 RID: 6664
		public abstract int NumberOfInitialHideoutsAtEachBanditFaction { get; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001A09 RID: 6665
		public abstract int NumberOfMinimumBanditTroopsInHideoutMission { get; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001A0A RID: 6666
		public abstract int NumberOfMaximumTroopCountForFirstFightInHideout { get; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001A0B RID: 6667
		public abstract int NumberOfMaximumTroopCountForBossFightInHideout { get; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06001A0C RID: 6668
		public abstract float SpawnPercentageForFirstFightInHideoutMission { get; }

		// Token: 0x06001A0D RID: 6669
		public abstract int GetPlayerMaximumTroopCountForHideoutMission(MobileParty party);
	}
}
