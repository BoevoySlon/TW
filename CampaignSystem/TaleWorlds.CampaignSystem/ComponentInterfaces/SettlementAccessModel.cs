using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000167 RID: 359
	public abstract class SettlementAccessModel : GameModel
	{
		// Token: 0x060018F4 RID: 6388
		public abstract void CanMainHeroEnterSettlement(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x060018F5 RID: 6389
		public abstract void CanMainHeroEnterLordsHall(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x060018F6 RID: 6390
		public abstract void CanMainHeroEnterDungeon(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x060018F7 RID: 6391
		public abstract bool CanMainHeroAccessLocation(Settlement settlement, string locationId, out bool disableOption, out TextObject disabledText);

		// Token: 0x060018F8 RID: 6392
		public abstract bool CanMainHeroDoSettlementAction(Settlement settlement, SettlementAccessModel.SettlementAction settlementAction, out bool disableOption, out TextObject disabledText);

		// Token: 0x060018F9 RID: 6393
		public abstract bool IsRequestMeetingOptionAvailable(Settlement settlement, out bool disableOption, out TextObject disabledText);

		// Token: 0x02000551 RID: 1361
		public enum AccessLevel
		{
			// Token: 0x0400166B RID: 5739
			NoAccess,
			// Token: 0x0400166C RID: 5740
			LimitedAccess,
			// Token: 0x0400166D RID: 5741
			FullAccess
		}

		// Token: 0x02000552 RID: 1362
		public enum AccessMethod
		{
			// Token: 0x0400166F RID: 5743
			None,
			// Token: 0x04001670 RID: 5744
			Direct,
			// Token: 0x04001671 RID: 5745
			ByRequest
		}

		// Token: 0x02000553 RID: 1363
		public enum AccessLimitationReason
		{
			// Token: 0x04001673 RID: 5747
			None,
			// Token: 0x04001674 RID: 5748
			HostileFaction,
			// Token: 0x04001675 RID: 5749
			RelationshipWithOwner,
			// Token: 0x04001676 RID: 5750
			CrimeRating,
			// Token: 0x04001677 RID: 5751
			VillageIsLooted,
			// Token: 0x04001678 RID: 5752
			Disguised,
			// Token: 0x04001679 RID: 5753
			ClanTier,
			// Token: 0x0400167A RID: 5754
			LocationEmpty
		}

		// Token: 0x02000554 RID: 1364
		public enum LimitedAccessSolution
		{
			// Token: 0x0400167C RID: 5756
			None,
			// Token: 0x0400167D RID: 5757
			Bribe,
			// Token: 0x0400167E RID: 5758
			Disguise
		}

		// Token: 0x02000555 RID: 1365
		public enum PreliminaryActionObligation
		{
			// Token: 0x04001680 RID: 5760
			None,
			// Token: 0x04001681 RID: 5761
			Optional
		}

		// Token: 0x02000556 RID: 1366
		public enum PreliminaryActionType
		{
			// Token: 0x04001683 RID: 5763
			None,
			// Token: 0x04001684 RID: 5764
			FaceCharges
		}

		// Token: 0x02000557 RID: 1367
		public enum SettlementAction
		{
			// Token: 0x04001686 RID: 5766
			RecruitTroops,
			// Token: 0x04001687 RID: 5767
			Craft,
			// Token: 0x04001688 RID: 5768
			WalkAroundTheArena,
			// Token: 0x04001689 RID: 5769
			JoinTournament,
			// Token: 0x0400168A RID: 5770
			WatchTournament,
			// Token: 0x0400168B RID: 5771
			Trade,
			// Token: 0x0400168C RID: 5772
			WaitInSettlement,
			// Token: 0x0400168D RID: 5773
			ManageTown
		}

		// Token: 0x02000558 RID: 1368
		public struct AccessDetails
		{
			// Token: 0x0400168E RID: 5774
			public SettlementAccessModel.AccessLevel AccessLevel;

			// Token: 0x0400168F RID: 5775
			public SettlementAccessModel.AccessMethod AccessMethod;

			// Token: 0x04001690 RID: 5776
			public SettlementAccessModel.AccessLimitationReason AccessLimitationReason;

			// Token: 0x04001691 RID: 5777
			public SettlementAccessModel.LimitedAccessSolution LimitedAccessSolution;

			// Token: 0x04001692 RID: 5778
			public SettlementAccessModel.PreliminaryActionObligation PreliminaryActionObligation;

			// Token: 0x04001693 RID: 5779
			public SettlementAccessModel.PreliminaryActionType PreliminaryActionType;
		}
	}
}
