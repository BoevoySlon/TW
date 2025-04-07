using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002A9 RID: 681
	public struct PartyScreenLogicInitializationData
	{
		// Token: 0x0600277C RID: 10108 RVA: 0x000A8AA8 File Offset: 0x000A6CA8
		public static PartyScreenLogicInitializationData CreateBasicInitDataWithMainParty(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, PartyScreenLogic.TransferState accompanyingTransferState, IsTroopTransferableDelegate troopTransferableDelegate, PartyBase leftOwnerParty = null, TextObject leftPartyName = null, TextObject header = null, Hero leftLeaderHero = null, int leftPartyMembersSizeLimit = 0, int leftPartyPrisonersSizeLimit = 0, PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = null, PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = null, PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = null, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null, PartyScreenClosedDelegate partyScreenClosedDelegate = null, bool isDismissMode = false, bool transferHealthiesGetWoundedsFirst = false, bool isTroopUpgradesDisabled = false, bool showProgressBar = false, int questModeWageDaysMultiplier = 0)
		{
			return new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = leftOwnerParty,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = leftMemberRoster,
				LeftPrisonerRoster = leftPrisonerRoster,
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = leftLeaderHero,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = leftPartyMembersSizeLimit,
				LeftPartyPrisonersSizeLimit = leftPartyPrisonersSizeLimit,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = leftPartyName,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = troopTransferableDelegate,
				PartyPresentationDoneButtonDelegate = partyPresentationDoneButtonDelegate,
				PartyPresentationDoneButtonConditionDelegate = partyPresentationDoneButtonConditionDelegate,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = partyPresentationCancelButtonDelegate,
				IsDismissMode = isDismissMode,
				IsTroopUpgradesDisabled = isTroopUpgradesDisabled,
				Header = header,
				PartyScreenClosedDelegate = partyScreenClosedDelegate,
				TransferHealthiesGetWoundedsFirst = transferHealthiesGetWoundedsFirst,
				ShowProgressBar = showProgressBar,
				MemberTransferState = memberTransferState,
				PrisonerTransferState = prisonerTransferState,
				AccompanyingTransferState = accompanyingTransferState,
				QuestModeWageDaysMultiplier = questModeWageDaysMultiplier
			};
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000A8BF4 File Offset: 0x000A6DF4
		public static PartyScreenLogicInitializationData CreateBasicInitDataWithMainPartyAndOther(MobileParty party, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, PartyScreenLogic.TransferState accompanyingTransferState, IsTroopTransferableDelegate troopTransferableDelegate, TextObject header = null, PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = null, PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = null, PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = null, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null, PartyScreenClosedDelegate partyScreenClosedDelegate = null, bool isDismissMode = false, bool transferHealthiesGetWoundedsFirst = false, bool isTroopUpgradesDisabled = true, bool showProgressBar = false)
		{
			return new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = party.Party,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = party.MemberRoster,
				LeftPrisonerRoster = party.PrisonRoster,
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = party.LeaderHero,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = party.Party.PartySizeLimit,
				LeftPartyPrisonersSizeLimit = party.Party.PrisonerSizeLimit,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = party.Name,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = troopTransferableDelegate,
				PartyPresentationDoneButtonDelegate = partyPresentationDoneButtonDelegate,
				PartyPresentationDoneButtonConditionDelegate = partyPresentationDoneButtonConditionDelegate,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = partyPresentationCancelButtonDelegate,
				IsDismissMode = isDismissMode,
				IsTroopUpgradesDisabled = isTroopUpgradesDisabled,
				Header = header,
				PartyScreenClosedDelegate = partyScreenClosedDelegate,
				TransferHealthiesGetWoundedsFirst = transferHealthiesGetWoundedsFirst,
				ShowProgressBar = showProgressBar,
				MemberTransferState = memberTransferState,
				PrisonerTransferState = prisonerTransferState,
				AccompanyingTransferState = accompanyingTransferState
			};
		}

		// Token: 0x04000BFA RID: 3066
		public TroopRoster LeftMemberRoster;

		// Token: 0x04000BFB RID: 3067
		public TroopRoster LeftPrisonerRoster;

		// Token: 0x04000BFC RID: 3068
		public TroopRoster RightMemberRoster;

		// Token: 0x04000BFD RID: 3069
		public TroopRoster RightPrisonerRoster;

		// Token: 0x04000BFE RID: 3070
		public PartyBase LeftOwnerParty;

		// Token: 0x04000BFF RID: 3071
		public PartyBase RightOwnerParty;

		// Token: 0x04000C00 RID: 3072
		public TextObject LeftPartyName;

		// Token: 0x04000C01 RID: 3073
		public TextObject RightPartyName;

		// Token: 0x04000C02 RID: 3074
		public TextObject Header;

		// Token: 0x04000C03 RID: 3075
		public Hero LeftLeaderHero;

		// Token: 0x04000C04 RID: 3076
		public Hero RightLeaderHero;

		// Token: 0x04000C05 RID: 3077
		public int LeftPartyMembersSizeLimit;

		// Token: 0x04000C06 RID: 3078
		public int LeftPartyPrisonersSizeLimit;

		// Token: 0x04000C07 RID: 3079
		public int RightPartyMembersSizeLimit;

		// Token: 0x04000C08 RID: 3080
		public int RightPartyPrisonersSizeLimit;

		// Token: 0x04000C09 RID: 3081
		public PartyPresentationDoneButtonDelegate PartyPresentationDoneButtonDelegate;

		// Token: 0x04000C0A RID: 3082
		public PartyPresentationDoneButtonConditionDelegate PartyPresentationDoneButtonConditionDelegate;

		// Token: 0x04000C0B RID: 3083
		public PartyPresentationCancelButtonActivateDelegate PartyPresentationCancelButtonActivateDelegate;

		// Token: 0x04000C0C RID: 3084
		public IsTroopTransferableDelegate TroopTransferableDelegate;

		// Token: 0x04000C0D RID: 3085
		public PartyPresentationCancelButtonDelegate PartyPresentationCancelButtonDelegate;

		// Token: 0x04000C0E RID: 3086
		public PartyScreenClosedDelegate PartyScreenClosedDelegate;

		// Token: 0x04000C0F RID: 3087
		public bool IsDismissMode;

		// Token: 0x04000C10 RID: 3088
		public bool TransferHealthiesGetWoundedsFirst;

		// Token: 0x04000C11 RID: 3089
		public bool IsTroopUpgradesDisabled;

		// Token: 0x04000C12 RID: 3090
		public bool ShowProgressBar;

		// Token: 0x04000C13 RID: 3091
		public int QuestModeWageDaysMultiplier;

		// Token: 0x04000C14 RID: 3092
		public PartyScreenLogic.TransferState MemberTransferState;

		// Token: 0x04000C15 RID: 3093
		public PartyScreenLogic.TransferState PrisonerTransferState;

		// Token: 0x04000C16 RID: 3094
		public PartyScreenLogic.TransferState AccompanyingTransferState;
	}
}
