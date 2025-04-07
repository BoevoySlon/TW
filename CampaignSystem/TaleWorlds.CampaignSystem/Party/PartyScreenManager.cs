using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002AB RID: 683
	public class PartyScreenManager
	{
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x000A8D5C File Offset: 0x000A6F5C
		// (set) Token: 0x0600277F RID: 10111 RVA: 0x000A8D64 File Offset: 0x000A6F64
		public bool IsDonating { get; private set; }

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x000A8D6D File Offset: 0x000A6F6D
		public PartyScreenMode CurrentMode
		{
			get
			{
				return this._currentMode;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x000A8D75 File Offset: 0x000A6F75
		public static PartyScreenManager Instance
		{
			get
			{
				return Campaign.Current.PartyScreenManager;
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x000A8D81 File Offset: 0x000A6F81
		public static PartyScreenLogic PartyScreenLogic
		{
			get
			{
				return PartyScreenManager.Instance._partyScreenLogic;
			}
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000A8D90 File Offset: 0x000A6F90
		private void OpenPartyScreen()
		{
			Game game = Game.Current;
			this._partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData initializationData = new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = null,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = TroopRoster.CreateDummyTroopRoster(),
				LeftPrisonerRoster = TroopRoster.CreateDummyTroopRoster(),
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = null,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = 0,
				LeftPartyPrisonersSizeLimit = 0,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = null,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.TroopTransferableDelegate),
				PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.DefaultDoneHandler),
				PartyPresentationDoneButtonConditionDelegate = null,
				PartyPresentationCancelButtonActivateDelegate = null,
				PartyPresentationCancelButtonDelegate = null,
				IsDismissMode = true,
				IsTroopUpgradesDisabled = false,
				Header = null,
				PartyScreenClosedDelegate = null,
				TransferHealthiesGetWoundedsFirst = false,
				ShowProgressBar = false,
				MemberTransferState = PartyScreenLogic.TransferState.Transferable,
				PrisonerTransferState = PartyScreenLogic.TransferState.Transferable,
				AccompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable
			};
			this._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = game.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(this._partyScreenLogic);
			this._currentMode = PartyScreenMode.Normal;
			game.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000A8F26 File Offset: 0x000A7126
		public static void CloseScreen(bool isForced, bool fromCancel = false)
		{
			PartyScreenManager.Instance.ClosePartyPresentation(isForced, fromCancel);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000A8F34 File Offset: 0x000A7134
		private void ClosePartyPresentation(bool isForced, bool fromCancel)
		{
			if (this._partyScreenLogic == null)
			{
				Debug.FailedAssert("Trying to close party screen when it's already closed!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenManager.cs", "ClosePartyPresentation", 101);
				return;
			}
			bool flag = true;
			if (!fromCancel)
			{
				flag = this._partyScreenLogic.DoneLogic(isForced);
			}
			if (flag)
			{
				this._partyScreenLogic.OnPartyScreenClosed(fromCancel);
				this._partyScreenLogic = null;
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000A8F98 File Offset: 0x000A7198
		public static void OpenScreenAsCheat()
		{
			if (!Game.Current.CheatMode)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=!}Cheat mode is not enabled!", null), 0, null, "");
				return;
			}
			PartyScreenManager.Instance.IsDonating = false;
			Game game = Game.Current;
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData initializationData = new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = null,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = PartyScreenManager.GetRosterWithAllGameTroops(),
				LeftPrisonerRoster = TroopRoster.CreateDummyTroopRoster(),
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = null,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = 0,
				LeftPartyPrisonersSizeLimit = 0,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = null,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.TroopTransferableDelegate),
				PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.DefaultDoneHandler),
				PartyPresentationDoneButtonConditionDelegate = null,
				PartyPresentationCancelButtonActivateDelegate = null,
				PartyPresentationCancelButtonDelegate = null,
				IsDismissMode = true,
				IsTroopUpgradesDisabled = false,
				Header = null,
				PartyScreenClosedDelegate = null,
				TransferHealthiesGetWoundedsFirst = false,
				ShowProgressBar = false,
				MemberTransferState = PartyScreenLogic.TransferState.Transferable,
				PrisonerTransferState = PartyScreenLogic.TransferState.Transferable,
				AccompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable
			};
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = game.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			PartyScreenManager.Instance._currentMode = PartyScreenMode.Normal;
			game.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000A9170 File Offset: 0x000A7370
		private static TroopRoster GetRosterWithAllGameTroops()
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			List<CharacterObject> list = new List<CharacterObject>();
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(CharacterObject));
			for (int i = 0; i < CharacterObject.All.Count; i++)
			{
				CharacterObject characterObject = CharacterObject.All[i];
				if (pageOf.IsValidEncyclopediaItem(characterObject))
				{
					list.Add(characterObject);
				}
			}
			list.Sort((CharacterObject a, CharacterObject b) => a.Name.ToString().CompareTo(b.Name.ToString()));
			for (int j = 0; j < list.Count; j++)
			{
				CharacterObject character = list[j];
				troopRoster.AddToCounts(character, PartyScreenManager._countToAddForEachTroopCheatMode, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000A922F File Offset: 0x000A742F
		public static void OpenScreenAsNormal()
		{
			if (Game.Current.CheatMode)
			{
				PartyScreenManager.OpenScreenAsCheat();
				return;
			}
			PartyScreenManager.Instance.IsDonating = false;
			PartyScreenManager.Instance.OpenPartyScreen();
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000A9258 File Offset: 0x000A7458
		public static void OpenScreenAsRansom()
		{
			PartyScreenManager.Instance._currentMode = PartyScreenMode.Ransom;
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance.IsDonating = false;
			TroopRoster leftMemberRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster leftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.TransferableWithTrade;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.TroopTransferableDelegate);
			PartyBase leftOwnerParty = null;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.SellPrisonersDoneHandler);
			TextObject header = new TextObject("{=SvahUNo6}Ransom Prisoners", null);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, leftOwnerParty, GameTexts.FindText("str_ransom_broker", null), header, null, 0, 0, partyPresentationDoneButtonDelegate, null, null, null, null, false, false, false, false, 0);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000A9320 File Offset: 0x000A7520
		public static void OpenScreenAsLoot(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, TextObject leftPartyName, int leftPartySizeLimit, PartyScreenClosedDelegate partyScreenClosedDelegate = null)
		{
			PartyScreenManager.Instance._currentMode = PartyScreenMode.Loot;
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.TroopTransferableDelegate);
			PartyBase leftOwnerParty = null;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.DefaultDoneHandler);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, leftOwnerParty, leftPartyName, new TextObject("{=EOQcQa5l}Aftermath", null), null, leftPartySizeLimit, 0, partyPresentationDoneButtonDelegate, null, null, null, partyScreenClosedDelegate, false, false, false, false, 0);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyScreenManager.Instance.IsDonating = false;
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000A93D8 File Offset: 0x000A75D8
		public static void OpenScreenAsManageTroopsAndPrisoners(MobileParty leftParty, PartyScreenClosedDelegate onPartyScreenClosed = null)
		{
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.Normal;
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.ClanManageTroopAndPrisonerTransferableDelegate);
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.ManageTroopsAndPrisonersDoneHandler);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainPartyAndOther(leftParty, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, new TextObject("{=uQgNPJnc}Manage Troops", null), partyPresentationDoneButtonDelegate, null, null, null, onPartyScreenClosed, false, false, true, false);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyScreenManager.Instance.IsDonating = false;
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000A9484 File Offset: 0x000A7684
		public static void OpenScreenAsReceiveTroops(TroopRoster leftMemberParty, TextObject leftPartyName, PartyScreenClosedDelegate partyScreenClosedDelegate = null)
		{
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.TroopsManage;
			TroopRoster leftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.TroopTransferableDelegate);
			PartyBase leftOwnerParty = null;
			int totalManCount = leftMemberParty.TotalManCount;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.DefaultDoneHandler);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberParty, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, leftOwnerParty, leftPartyName, new TextObject("{=uQgNPJnc}Manage Troops", null), null, totalManCount, 0, partyPresentationDoneButtonDelegate, null, null, null, partyScreenClosedDelegate, false, false, false, false, 0);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyScreenManager.Instance.IsDonating = false;
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000A9544 File Offset: 0x000A7744
		public static void OpenScreenAsManageTroops(MobileParty leftParty)
		{
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.TroopsManage;
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainPartyAndOther(leftParty, PartyScreenLogic.TransferState.Transferable, PartyScreenLogic.TransferState.NotTransferable, PartyScreenLogic.TransferState.Transferable, new IsTroopTransferableDelegate(PartyScreenManager.ClanManageTroopTransferableDelegate), new TextObject("{=uQgNPJnc}Manage Troops", null), new PartyPresentationDoneButtonDelegate(PartyScreenManager.DefaultDoneHandler), null, null, null, null, false, false, true, false);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyScreenManager.Instance.IsDonating = false;
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000A95EC File Offset: 0x000A77EC
		public static void OpenScreenAsDonateTroops(MobileParty leftParty)
		{
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.TroopsManage;
			PartyScreenManager.Instance.IsDonating = (leftParty.Owner.Clan != Clan.PlayerClan);
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.DonateModeTroopTransferableDelegate);
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = new PartyPresentationDoneButtonConditionDelegate(PartyScreenManager.DonateDonePossibleDelegate);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainPartyAndOther(leftParty, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, new TextObject("{=4YfjgtO2}Donate Troops", null), null, partyPresentationDoneButtonConditionDelegate, null, null, null, false, false, true, false);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000A96AC File Offset: 0x000A78AC
		public static void OpenScreenAsDonateGarrisonWithCurrentSettlement()
		{
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.TroopsManage;
			PartyScreenManager.Instance.IsDonating = true;
			if (Hero.MainHero.CurrentSettlement.Town.GarrisonParty == null)
			{
				Hero.MainHero.CurrentSettlement.AddGarrisonParty(false);
			}
			MobileParty garrisonParty = Hero.MainHero.CurrentSettlement.Town.GarrisonParty;
			int num = Math.Max(garrisonParty.Party.PartySizeLimit - garrisonParty.Party.NumberOfAllMembers, 0);
			TroopRoster leftMemberRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster leftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.TroopTransferableDelegate);
			PartyBase leftOwnerParty = null;
			TextObject name = garrisonParty.Name;
			int leftPartyMembersSizeLimit = num;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.DonateGarrisonDoneHandler);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, leftOwnerParty, name, new TextObject("{=uQgNPJnc}Manage Troops", null), null, leftPartyMembersSizeLimit, 0, partyPresentationDoneButtonDelegate, null, null, null, null, false, false, false, false, 0);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000A97CC File Offset: 0x000A79CC
		public static void OpenScreenAsDonatePrisoners()
		{
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.PrisonerManage;
			PartyScreenManager.Instance.IsDonating = true;
			if (Hero.MainHero.CurrentSettlement.Town.GarrisonParty == null)
			{
				Hero.MainHero.CurrentSettlement.AddGarrisonParty(false);
			}
			TroopRoster prisonRoster = Hero.MainHero.CurrentSettlement.Party.PrisonRoster;
			int num = Math.Max(Hero.MainHero.CurrentSettlement.Party.PrisonerSizeLimit - prisonRoster.Count, 0);
			TextObject textObject = new TextObject("{=SDzIAtiA}Prisoners of {SETTLEMENT_NAME}", null);
			textObject.SetTextVariable("SETTLEMENT_NAME", Hero.MainHero.CurrentSettlement.Name);
			TroopRoster leftMemberRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster leftPrisonerRoster = prisonRoster;
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.DonatePrisonerTransferableDelegate);
			PartyBase leftOwnerParty = null;
			TextObject leftPartyName = textObject;
			int leftPartyPrisonersSizeLimit = num;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.DonatePrisonersDoneHandler);
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = new PartyPresentationDoneButtonConditionDelegate(PartyScreenManager.DonateDonePossibleDelegate);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, leftOwnerParty, leftPartyName, new TextObject("{=Z212GSiV}Leave Prisoners", null), null, 0, leftPartyPrisonersSizeLimit, partyPresentationDoneButtonDelegate, partyPresentationDoneButtonConditionDelegate, null, null, null, false, false, false, false, 0);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000A9920 File Offset: 0x000A7B20
		private static Tuple<bool, TextObject> DonateDonePossibleDelegate(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			if (PartyScreenManager.PartyScreenLogic.CurrentData.TransferredPrisonersHistory.Any((Tuple<CharacterObject, int> p) => p.Item2 > 0))
			{
				return new Tuple<bool, TextObject>(false, new TextObject("{=hI7eDbXs}You cannot take prisoners.", null));
			}
			if (PartyScreenManager.PartyScreenLogic.HaveRightSideGainedTroops())
			{
				return new Tuple<bool, TextObject>(false, new TextObject("{=pvkl6pZh}You cannot take troops.", null));
			}
			if ((PartyScreenManager.PartyScreenLogic.MemberTransferState != PartyScreenLogic.TransferState.NotTransferable || PartyScreenManager.PartyScreenLogic.AccompanyingTransferState != PartyScreenLogic.TransferState.NotTransferable) && PartyScreenManager.PartyScreenLogic.LeftPartyMembersSizeLimit < PartyScreenManager.PartyScreenLogic.MemberRosters[0].TotalManCount)
			{
				return new Tuple<bool, TextObject>(false, new TextObject("{=R7wiHjcL}Donated troops exceed party capacity.", null));
			}
			if (PartyScreenManager.PartyScreenLogic.PrisonerTransferState != PartyScreenLogic.TransferState.NotTransferable && PartyScreenManager.PartyScreenLogic.LeftPartyPrisonersSizeLimit < PartyScreenManager.PartyScreenLogic.PrisonerRosters[0].TotalManCount)
			{
				return new Tuple<bool, TextObject>(false, new TextObject("{=3nfPGbN0}Donated prisoners exceed party capacity.", null));
			}
			return new Tuple<bool, TextObject>(true, TextObject.Empty);
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000A9A1F File Offset: 0x000A7C1F
		public static bool DonatePrisonerTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return side == PartyScreenLogic.PartyRosterSide.Right && type == PartyScreenLogic.TroopType.Prisoner;
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000A9A2C File Offset: 0x000A7C2C
		public static void OpenScreenAsManagePrisoners()
		{
			Hero mainHero = Hero.MainHero;
			bool flag;
			if (mainHero == null)
			{
				flag = (null != null);
			}
			else
			{
				Settlement currentSettlement = mainHero.CurrentSettlement;
				flag = (((currentSettlement != null) ? currentSettlement.Party : null) != null);
			}
			if (!flag)
			{
				Debug.FailedAssert("Trying to open prisoner management in an invalid state", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenManager.cs", "OpenScreenAsManagePrisoners", 474);
				Debug.Print("Trying to open prisoner management in an invalid state", 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.PrisonerManage;
			TroopRoster prisonRoster = Hero.MainHero.CurrentSettlement.Party.PrisonRoster;
			TextObject textObject = new TextObject("{=SDzIAtiA}Prisoners of {SETTLEMENT_NAME}", null);
			textObject.SetTextVariable("SETTLEMENT_NAME", Hero.MainHero.CurrentSettlement.Name);
			TroopRoster leftMemberRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster leftPrisonerRoster = prisonRoster;
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable;
			IsTroopTransferableDelegate troopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenManager.TroopTransferableDelegate);
			PartyBase leftOwnerParty = null;
			TextObject leftPartyName = textObject;
			int prisonerSizeLimit = Hero.MainHero.CurrentSettlement.Party.PrisonerSizeLimit;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.ManageGarrisonDoneHandler);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, troopTransferableDelegate, leftOwnerParty, leftPartyName, new TextObject("{=aadTnAEg}Manage Prisoners", null), null, 0, prisonerSizeLimit, partyPresentationDoneButtonDelegate, null, null, null, null, false, false, false, false, 0);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyScreenManager.Instance.IsDonating = false;
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000A9B88 File Offset: 0x000A7D88
		public static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase leftOwnerParty)
		{
			Hero hero = (leftOwnerParty != null) ? leftOwnerParty.LeaderHero : null;
			bool flag;
			if ((hero == null || hero.Clan != Clan.PlayerClan) && (leftOwnerParty == null || !leftOwnerParty.IsMobile || !leftOwnerParty.MobileParty.IsCaravan || leftOwnerParty.Owner != Hero.MainHero))
			{
				if (leftOwnerParty != null && leftOwnerParty.IsMobile && leftOwnerParty.MobileParty.IsGarrison)
				{
					Settlement currentSettlement = leftOwnerParty.MobileParty.CurrentSettlement;
					flag = (((currentSettlement != null) ? currentSettlement.OwnerClan : null) == Clan.PlayerClan);
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			return !character.IsHero || (character.IsHero && character.HeroObject.Clan != Clan.PlayerClan && (!character.HeroObject.IsPlayerCompanion || (character.HeroObject.IsPlayerCompanion && flag2)));
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000A9C56 File Offset: 0x000A7E56
		public static bool ClanManageTroopAndPrisonerTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero || character.HeroObject.IsPrisoner;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000A9C6D File Offset: 0x000A7E6D
		public static bool ClanManageTroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero;
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000A9C78 File Offset: 0x000A7E78
		public static bool DonateModeTroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero && side == PartyScreenLogic.PartyRosterSide.Right;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000A9C88 File Offset: 0x000A7E88
		public static void OpenScreenWithCondition(IsTroopTransferableDelegate isTroopTransferable, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyPresentationDoneButtonDelegate onDoneClicked, PartyPresentationCancelButtonDelegate onCancelClicked, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, TextObject leftPartyName, int limit, bool showProgressBar, bool isDonating, PartyScreenMode screenMode = PartyScreenMode.Normal, TroopRoster memberRosterLeft = null, TroopRoster prisonerRosterLeft = null)
		{
			if (memberRosterLeft == null)
			{
				memberRosterLeft = TroopRoster.CreateDummyTroopRoster();
			}
			if (prisonerRosterLeft == null)
			{
				prisonerRosterLeft = TroopRoster.CreateDummyTroopRoster();
			}
			PartyScreenManager.Instance._currentMode = screenMode;
			PartyScreenManager.Instance.IsDonating = isDonating;
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(memberRosterLeft, prisonerRosterLeft, memberTransferState, prisonerTransferState, PartyScreenLogic.TransferState.NotTransferable, isTroopTransferable, null, leftPartyName, new TextObject("{=nZaeTlj8}Exchange Troops", null), null, limit, 0, onDoneClicked, doneButtonCondition, onCancelClicked, null, null, false, false, false, showProgressBar, 0);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000A9D5C File Offset: 0x000A7F5C
		public static void OpenScreenForManagingAlley(TroopRoster memberRosterLeft, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyPresentationDoneButtonDelegate onDoneClicked, TextObject leftPartyName, PartyPresentationCancelButtonDelegate onCancelButtonClicked)
		{
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(memberRosterLeft, TroopRoster.CreateDummyTroopRoster(), PartyScreenLogic.TransferState.Transferable, PartyScreenLogic.TransferState.NotTransferable, PartyScreenLogic.TransferState.NotTransferable, isTroopTransferable, null, leftPartyName, null, null, Campaign.Current.Models.AlleyModel.MaximumTroopCountInPlayerOwnedAlley + 1, 0, onDoneClicked, doneButtonCondition, onCancelButtonClicked, null, null, false, false, false, true, 0);
			PartyScreenManager.Instance._currentMode = PartyScreenMode.TroopsManage;
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000A9DFC File Offset: 0x000A7FFC
		public static void OpenScreenAsQuest(TroopRoster leftMemberRoster, TextObject leftPartyName, int leftPartySizeLimit, int questDaysMultiplier, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyScreenClosedDelegate onPartyScreenClosed, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null)
		{
			Debug.Print("PartyScreenManager::OpenScreenAsQuest", 0, Debug.DebugColor.White, 17592186044416UL);
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.QuestTroopManage;
			TroopRoster leftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState memberTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState prisonerTransferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState accompanyingTransferState = PartyScreenLogic.TransferState.Transferable;
			PartyBase leftOwnerParty = null;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.ManageTroopsAndPrisonersDoneHandler);
			PartyScreenLogicInitializationData initializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, leftPrisonerRoster, memberTransferState, prisonerTransferState, accompanyingTransferState, isTroopTransferable, leftOwnerParty, leftPartyName, new TextObject("{=nZaeTlj8}Exchange Troops", null), null, leftPartySizeLimit, 0, partyPresentationDoneButtonDelegate, doneButtonCondition, null, partyPresentationCancelButtonActivateDelegate, onPartyScreenClosed, false, true, false, true, questDaysMultiplier);
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyScreenManager.Instance.IsDonating = false;
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000A9ED0 File Offset: 0x000A80D0
		public static void OpenScreenWithDummyRoster(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonerRoster, TextObject leftPartyName, TextObject rightPartyName, int leftPartySizeLimit, int rightPartySizeLimit, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyScreenClosedDelegate onPartyScreenClosed, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null)
		{
			Debug.Print("PartyScreenManager::OpenScreenWithDummyRoster", 0, Debug.DebugColor.White, 17592186044416UL);
			PartyScreenManager.Instance._partyScreenLogic = new PartyScreenLogic();
			PartyScreenManager.Instance._currentMode = PartyScreenMode.TroopsManage;
			PartyScreenLogicInitializationData initializationData = new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = null,
				RightOwnerParty = MobileParty.MainParty.Party,
				LeftMemberRoster = leftMemberRoster,
				LeftPrisonerRoster = leftPrisonerRoster,
				RightMemberRoster = rightMemberRoster,
				RightPrisonerRoster = rightPrisonerRoster,
				LeftLeaderHero = null,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = leftPartySizeLimit,
				LeftPartyPrisonersSizeLimit = 0,
				RightPartyMembersSizeLimit = rightPartySizeLimit,
				RightPartyPrisonersSizeLimit = 0,
				LeftPartyName = leftPartyName,
				RightPartyName = rightPartyName,
				TroopTransferableDelegate = isTroopTransferable,
				PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenManager.ManageTroopsAndPrisonersDoneHandler),
				PartyPresentationDoneButtonConditionDelegate = doneButtonCondition,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = null,
				PartyScreenClosedDelegate = onPartyScreenClosed,
				IsDismissMode = true,
				IsTroopUpgradesDisabled = true,
				Header = null,
				TransferHealthiesGetWoundedsFirst = true,
				ShowProgressBar = false,
				MemberTransferState = PartyScreenLogic.TransferState.Transferable,
				PrisonerTransferState = PartyScreenLogic.TransferState.NotTransferable,
				AccompanyingTransferState = PartyScreenLogic.TransferState.Transferable
			};
			PartyScreenManager.Instance._partyScreenLogic.Initialize(initializationData);
			PartyScreenManager.Instance.IsDonating = false;
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.InitializeLogic(PartyScreenManager.Instance._partyScreenLogic);
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000AA068 File Offset: 0x000A8268
		public static void OpenScreenWithDummyRosterWithMainParty(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, TextObject leftPartyName, int leftPartySizeLimit, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyScreenClosedDelegate onPartyScreenClosed, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null)
		{
			Debug.Print("PartyScreenManager::OpenScreenWithDummyRosterWithMainParty", 0, Debug.DebugColor.White, 17592186044416UL);
			PartyScreenManager.OpenScreenWithDummyRoster(leftMemberRoster, leftPrisonerRoster, MobileParty.MainParty.MemberRoster, MobileParty.MainParty.PrisonRoster, leftPartyName, MobileParty.MainParty.Name, leftPartySizeLimit, MobileParty.MainParty.Party.PartySizeLimit, doneButtonCondition, onPartyScreenClosed, isTroopTransferable, partyPresentationCancelButtonActivateDelegate);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000AA0CC File Offset: 0x000A82CC
		public static void OpenScreenAsCreateClanPartyForHero(Hero hero, PartyScreenClosedDelegate onScreenClosed = null, IsTroopTransferableDelegate isTroopTransferable = null, bool isHeroRescued = false)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster leftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster2 = MobileParty.MainParty.MemberRoster.CloneRosterData();
			TroopRoster rightPrisonerRoster = MobileParty.MainParty.PrisonRoster.CloneRosterData();
			troopRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
			if (!isHeroRescued)
			{
				troopRoster2.AddToCounts(hero.CharacterObject, -1, false, 0, 0, true, -1);
			}
			TextObject textObject = GameTexts.FindText("str_lord_party_name", null);
			textObject.SetCharacterProperties("TROOP", hero.CharacterObject, false);
			PartyScreenManager.OpenScreenWithDummyRoster(troopRoster, leftPrisonerRoster, troopRoster2, rightPrisonerRoster, textObject, MobileParty.MainParty.Name, Campaign.Current.Models.PartySizeLimitModel.GetAssumedPartySizeForLordParty(hero, hero.Clan.MapFaction, hero.Clan), MobileParty.MainParty.LimitedPartySize, null, onScreenClosed ?? new PartyScreenClosedDelegate(PartyScreenManager.OpenScreenAsCreateClanPartyForHeroPartyScreenClosed), isTroopTransferable ?? new IsTroopTransferableDelegate(PartyScreenManager.OpenScreenAsCreateClanPartyForHeroTroopTransferableDelegate), null);
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000AA1B4 File Offset: 0x000A83B4
		private static void OpenScreenAsCreateClanPartyForHeroPartyScreenClosed(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			if (!fromCancel)
			{
				Hero hero = null;
				for (int i = 0; i < leftMemberRoster.data.Length; i++)
				{
					CharacterObject character = leftMemberRoster.data[i].Character;
					if (character != null && character.IsHero)
					{
						hero = leftMemberRoster.data[i].Character.HeroObject;
					}
				}
				MobileParty mobileParty = hero.Clan.CreateNewMobileParty(hero);
				foreach (TroopRosterElement troopRosterElement in leftMemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character != hero.CharacterObject)
					{
						mobileParty.MemberRoster.Add(troopRosterElement);
						rightOwnerParty.MemberRoster.AddToCounts(troopRosterElement.Character, -troopRosterElement.Number, false, -troopRosterElement.WoundedNumber, -troopRosterElement.Xp, true, -1);
					}
				}
				foreach (TroopRosterElement troopRosterElement2 in leftPrisonRoster.GetTroopRoster())
				{
					mobileParty.PrisonRoster.Add(troopRosterElement2);
					rightOwnerParty.PrisonRoster.AddToCounts(troopRosterElement2.Character, -troopRosterElement2.Number, false, -troopRosterElement2.WoundedNumber, -troopRosterElement2.Xp, true, -1);
				}
			}
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000AA324 File Offset: 0x000A8524
		private static bool OpenScreenAsCreateClanPartyForHeroTroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero;
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000AA32F File Offset: 0x000A852F
		private static bool SellPrisonersDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			SellPrisonersAction.ApplyByPartyScreen(leftPrisonRoster);
			return true;
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000AA338 File Offset: 0x000A8538
		private static bool DonateGarrisonDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
			MobileParty garrisonParty = currentSettlement.Town.GarrisonParty;
			if (garrisonParty == null)
			{
				currentSettlement.AddGarrisonParty(false);
				garrisonParty = currentSettlement.Town.GarrisonParty;
			}
			for (int i = 0; i < leftMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = leftMemberRoster.GetElementCopyAtIndex(i);
				garrisonParty.AddElementToMemberRoster(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false);
				if (elementCopyAtIndex.Character.IsHero)
				{
					EnterSettlementAction.ApplyForCharacterOnly(elementCopyAtIndex.Character.HeroObject, currentSettlement);
				}
			}
			return true;
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000AA3C0 File Offset: 0x000A85C0
		private static bool DonatePrisonersDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster leftSideTransferredPrisonerRoster, FlattenedTroopRoster rightSideTransferredPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			if (!rightSideTransferredPrisonerRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
				foreach (CharacterObject characterObject in rightSideTransferredPrisonerRoster.Troops)
				{
					if (characterObject.IsHero)
					{
						EnterSettlementAction.ApplyForPrisoner(characterObject.HeroObject, currentSettlement);
					}
				}
				CampaignEventDispatcher.Instance.OnPrisonerDonatedToSettlement(rightParty.MobileParty, rightSideTransferredPrisonerRoster, currentSettlement);
			}
			return true;
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000AA444 File Offset: 0x000A8644
		private static bool ManageGarrisonDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
			for (int i = 0; i < leftMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = leftMemberRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Character.IsHero)
				{
					EnterSettlementAction.ApplyForCharacterOnly(elementCopyAtIndex.Character.HeroObject, currentSettlement);
				}
			}
			for (int j = 0; j < leftPrisonRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = leftPrisonRoster.GetElementCopyAtIndex(j);
				if (elementCopyAtIndex2.Character.IsHero)
				{
					EnterSettlementAction.ApplyForPrisoner(elementCopyAtIndex2.Character.HeroObject, currentSettlement);
				}
			}
			return true;
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000AA4CE File Offset: 0x000A86CE
		private static bool ManageTroopsAndPrisonersDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			return true;
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000AA4D1 File Offset: 0x000A86D1
		private static bool DefaultDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			PartyScreenManager.HandleReleasedAndTakenPrisoners(takenPrisonerRoster, releasedPrisonerRoster);
			return true;
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000AA4DD File Offset: 0x000A86DD
		private static void HandleReleasedAndTakenPrisoners(FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster)
		{
			if (!releasedPrisonerRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				EndCaptivityAction.ApplyByReleasedByChoice(releasedPrisonerRoster);
			}
			if (!takenPrisonerRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				TakePrisonerAction.ApplyByTakenFromPartyScreen(takenPrisonerRoster);
			}
		}

		// Token: 0x04000C1F RID: 3103
		private PartyScreenMode _currentMode;

		// Token: 0x04000C20 RID: 3104
		private PartyScreenLogic _partyScreenLogic;

		// Token: 0x04000C21 RID: 3105
		private static readonly int _countToAddForEachTroopCheatMode = 10;
	}
}
