using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002A6 RID: 678
	public class PartyScreenLogic
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060026D9 RID: 9945 RVA: 0x000A4A6C File Offset: 0x000A2C6C
		// (remove) Token: 0x060026DA RID: 9946 RVA: 0x000A4AA4 File Offset: 0x000A2CA4
		public event PartyScreenLogic.PartyGoldDelegate PartyGoldChange;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060026DB RID: 9947 RVA: 0x000A4ADC File Offset: 0x000A2CDC
		// (remove) Token: 0x060026DC RID: 9948 RVA: 0x000A4B14 File Offset: 0x000A2D14
		public event PartyScreenLogic.PartyMoraleDelegate PartyMoraleChange;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060026DD RID: 9949 RVA: 0x000A4B4C File Offset: 0x000A2D4C
		// (remove) Token: 0x060026DE RID: 9950 RVA: 0x000A4B84 File Offset: 0x000A2D84
		public event PartyScreenLogic.PartyInfluenceDelegate PartyInfluenceChange;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060026DF RID: 9951 RVA: 0x000A4BBC File Offset: 0x000A2DBC
		// (remove) Token: 0x060026E0 RID: 9952 RVA: 0x000A4BF4 File Offset: 0x000A2DF4
		public event PartyScreenLogic.PartyHorseDelegate PartyHorseChange;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060026E1 RID: 9953 RVA: 0x000A4C2C File Offset: 0x000A2E2C
		// (remove) Token: 0x060026E2 RID: 9954 RVA: 0x000A4C64 File Offset: 0x000A2E64
		public event PartyScreenLogic.PresentationUpdate Update;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060026E3 RID: 9955 RVA: 0x000A4C9C File Offset: 0x000A2E9C
		// (remove) Token: 0x060026E4 RID: 9956 RVA: 0x000A4CD4 File Offset: 0x000A2ED4
		public event PartyScreenClosedDelegate PartyScreenClosedEvent;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060026E5 RID: 9957 RVA: 0x000A4D0C File Offset: 0x000A2F0C
		// (remove) Token: 0x060026E6 RID: 9958 RVA: 0x000A4D44 File Offset: 0x000A2F44
		public event PartyScreenLogic.AfterResetDelegate AfterReset;

		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x000A4D79 File Offset: 0x000A2F79
		// (set) Token: 0x060026E8 RID: 9960 RVA: 0x000A4D81 File Offset: 0x000A2F81
		public PartyScreenLogic.TroopSortType ActiveOtherPartySortType
		{
			get
			{
				return this._activeOtherPartySortType;
			}
			set
			{
				this._activeOtherPartySortType = value;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000A4D8A File Offset: 0x000A2F8A
		// (set) Token: 0x060026EA RID: 9962 RVA: 0x000A4D92 File Offset: 0x000A2F92
		public PartyScreenLogic.TroopSortType ActiveMainPartySortType
		{
			get
			{
				return this._activeMainPartySortType;
			}
			set
			{
				this._activeMainPartySortType = value;
			}
		}

		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x000A4D9B File Offset: 0x000A2F9B
		// (set) Token: 0x060026EC RID: 9964 RVA: 0x000A4DA3 File Offset: 0x000A2FA3
		public bool IsOtherPartySortAscending
		{
			get
			{
				return this._isOtherPartySortAscending;
			}
			set
			{
				this._isOtherPartySortAscending = value;
			}
		}

		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x000A4DAC File Offset: 0x000A2FAC
		// (set) Token: 0x060026EE RID: 9966 RVA: 0x000A4DB4 File Offset: 0x000A2FB4
		public bool IsMainPartySortAscending
		{
			get
			{
				return this._isMainPartySortAscending;
			}
			set
			{
				this._isMainPartySortAscending = value;
			}
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x000A4DBD File Offset: 0x000A2FBD
		// (set) Token: 0x060026F0 RID: 9968 RVA: 0x000A4DC5 File Offset: 0x000A2FC5
		public PartyScreenLogic.TransferState MemberTransferState { get; private set; }

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x000A4DCE File Offset: 0x000A2FCE
		// (set) Token: 0x060026F2 RID: 9970 RVA: 0x000A4DD6 File Offset: 0x000A2FD6
		public PartyScreenLogic.TransferState PrisonerTransferState { get; private set; }

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x000A4DDF File Offset: 0x000A2FDF
		// (set) Token: 0x060026F4 RID: 9972 RVA: 0x000A4DE7 File Offset: 0x000A2FE7
		public PartyScreenLogic.TransferState AccompanyingTransferState { get; private set; }

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x000A4DF0 File Offset: 0x000A2FF0
		// (set) Token: 0x060026F6 RID: 9974 RVA: 0x000A4DF8 File Offset: 0x000A2FF8
		public TextObject LeftPartyName { get; private set; }

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060026F7 RID: 9975 RVA: 0x000A4E01 File Offset: 0x000A3001
		// (set) Token: 0x060026F8 RID: 9976 RVA: 0x000A4E09 File Offset: 0x000A3009
		public TextObject RightPartyName { get; private set; }

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060026F9 RID: 9977 RVA: 0x000A4E12 File Offset: 0x000A3012
		// (set) Token: 0x060026FA RID: 9978 RVA: 0x000A4E1A File Offset: 0x000A301A
		public TextObject Header { get; private set; }

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060026FB RID: 9979 RVA: 0x000A4E23 File Offset: 0x000A3023
		// (set) Token: 0x060026FC RID: 9980 RVA: 0x000A4E2B File Offset: 0x000A302B
		public int LeftPartyMembersSizeLimit { get; private set; }

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060026FD RID: 9981 RVA: 0x000A4E34 File Offset: 0x000A3034
		// (set) Token: 0x060026FE RID: 9982 RVA: 0x000A4E3C File Offset: 0x000A303C
		public int LeftPartyPrisonersSizeLimit { get; private set; }

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x000A4E45 File Offset: 0x000A3045
		// (set) Token: 0x06002700 RID: 9984 RVA: 0x000A4E4D File Offset: 0x000A304D
		public int RightPartyMembersSizeLimit { get; private set; }

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06002701 RID: 9985 RVA: 0x000A4E56 File Offset: 0x000A3056
		// (set) Token: 0x06002702 RID: 9986 RVA: 0x000A4E5E File Offset: 0x000A305E
		public int RightPartyPrisonersSizeLimit { get; private set; }

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06002703 RID: 9987 RVA: 0x000A4E67 File Offset: 0x000A3067
		// (set) Token: 0x06002704 RID: 9988 RVA: 0x000A4E6F File Offset: 0x000A306F
		public bool ShowProgressBar { get; private set; }

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06002705 RID: 9989 RVA: 0x000A4E78 File Offset: 0x000A3078
		// (set) Token: 0x06002706 RID: 9990 RVA: 0x000A4E80 File Offset: 0x000A3080
		public string DoneReasonString { get; private set; }

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06002707 RID: 9991 RVA: 0x000A4E89 File Offset: 0x000A3089
		// (set) Token: 0x06002708 RID: 9992 RVA: 0x000A4E91 File Offset: 0x000A3091
		public bool IsTroopUpgradesDisabled { get; private set; }

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06002709 RID: 9993 RVA: 0x000A4E9A File Offset: 0x000A309A
		// (set) Token: 0x0600270A RID: 9994 RVA: 0x000A4EA2 File Offset: 0x000A30A2
		public CharacterObject RightPartyLeader { get; private set; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x000A4EAB File Offset: 0x000A30AB
		// (set) Token: 0x0600270C RID: 9996 RVA: 0x000A4EB3 File Offset: 0x000A30B3
		public CharacterObject LeftPartyLeader { get; private set; }

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x000A4EBC File Offset: 0x000A30BC
		// (set) Token: 0x0600270E RID: 9998 RVA: 0x000A4EC4 File Offset: 0x000A30C4
		public PartyBase LeftOwnerParty { get; private set; }

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x000A4ECD File Offset: 0x000A30CD
		// (set) Token: 0x06002710 RID: 10000 RVA: 0x000A4ED5 File Offset: 0x000A30D5
		public PartyBase RightOwnerParty { get; private set; }

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06002711 RID: 10001 RVA: 0x000A4EDE File Offset: 0x000A30DE
		// (set) Token: 0x06002712 RID: 10002 RVA: 0x000A4EE6 File Offset: 0x000A30E6
		public PartyScreenData CurrentData { get; private set; }

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06002713 RID: 10003 RVA: 0x000A4EEF File Offset: 0x000A30EF
		// (set) Token: 0x06002714 RID: 10004 RVA: 0x000A4EF7 File Offset: 0x000A30F7
		public bool TransferHealthiesGetWoundedsFirst { get; private set; }

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x000A4F00 File Offset: 0x000A3100
		// (set) Token: 0x06002716 RID: 10006 RVA: 0x000A4F08 File Offset: 0x000A3108
		public int QuestModeWageDaysMultiplier { get; private set; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x000A4F11 File Offset: 0x000A3111
		// (set) Token: 0x06002718 RID: 10008 RVA: 0x000A4F19 File Offset: 0x000A3119
		public Game Game
		{
			get
			{
				return this._game;
			}
			set
			{
				this._game = value;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06002719 RID: 10009 RVA: 0x000A4F22 File Offset: 0x000A3122
		private PartyScreenMode CurrentMode
		{
			get
			{
				return PartyScreenManager.Instance.CurrentMode;
			}
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000A4F30 File Offset: 0x000A3130
		public PartyScreenLogic()
		{
			this._game = Game.Current;
			this.MemberRosters = new TroopRoster[2];
			this.PrisonerRosters = new TroopRoster[2];
			this.CurrentData = new PartyScreenData();
			this._initialData = new PartyScreenData();
			this._defaultComparers = new Dictionary<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer>
			{
				{
					PartyScreenLogic.TroopSortType.Custom,
					new PartyScreenLogic.TroopDefaultComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Type,
					new PartyScreenLogic.TroopTypeComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Name,
					new PartyScreenLogic.TroopNameComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Count,
					new PartyScreenLogic.TroopCountComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Tier,
					new PartyScreenLogic.TroopTierComparer()
				}
			};
			this.IsTroopUpgradesDisabled = false;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000A4FCC File Offset: 0x000A31CC
		public void Initialize(PartyScreenLogicInitializationData initializationData)
		{
			this.MemberRosters[1] = initializationData.RightMemberRoster;
			this.PrisonerRosters[1] = initializationData.RightPrisonerRoster;
			this.MemberRosters[0] = initializationData.LeftMemberRoster;
			this.PrisonerRosters[0] = initializationData.LeftPrisonerRoster;
			Hero rightLeaderHero = initializationData.RightLeaderHero;
			this.RightPartyLeader = ((rightLeaderHero != null) ? rightLeaderHero.CharacterObject : null);
			Hero leftLeaderHero = initializationData.LeftLeaderHero;
			this.LeftPartyLeader = ((leftLeaderHero != null) ? leftLeaderHero.CharacterObject : null);
			this.RightOwnerParty = initializationData.RightOwnerParty;
			this.LeftOwnerParty = initializationData.LeftOwnerParty;
			this.RightPartyName = initializationData.RightPartyName;
			this.RightPartyMembersSizeLimit = initializationData.RightPartyMembersSizeLimit;
			this.RightPartyPrisonersSizeLimit = initializationData.RightPartyPrisonersSizeLimit;
			this.LeftPartyName = initializationData.LeftPartyName;
			this.LeftPartyMembersSizeLimit = initializationData.LeftPartyMembersSizeLimit;
			this.LeftPartyPrisonersSizeLimit = initializationData.LeftPartyPrisonersSizeLimit;
			this.Header = initializationData.Header;
			this.QuestModeWageDaysMultiplier = initializationData.QuestModeWageDaysMultiplier;
			this.TransferHealthiesGetWoundedsFirst = initializationData.TransferHealthiesGetWoundedsFirst;
			this.SetPartyGoldChangeAmount(0);
			this.SetHorseChangeAmount(0);
			this.SetInfluenceChangeAmount(0, 0, 0);
			this.SetMoraleChangeAmount(0);
			this.CurrentData.BindRostersFrom(this.MemberRosters[1], this.PrisonerRosters[1], this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty, this.LeftOwnerParty);
			this._initialData.InitializeCopyFrom(initializationData.RightOwnerParty, initializationData.LeftOwnerParty);
			this._initialData.CopyFromPartyAndRoster(this.MemberRosters[1], this.PrisonerRosters[1], this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty);
			if (initializationData.PartyPresentationDoneButtonDelegate == null)
			{
				Debug.FailedAssert("Done handler is given null for party screen!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "Initialize", 237);
				initializationData.PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenLogic.DefaultDoneHandler);
			}
			this.PartyPresentationDoneButtonDelegate = initializationData.PartyPresentationDoneButtonDelegate;
			this.PartyPresentationDoneButtonConditionDelegate = initializationData.PartyPresentationDoneButtonConditionDelegate;
			this.PartyPresentationCancelButtonActivateDelegate = initializationData.PartyPresentationCancelButtonActivateDelegate;
			this.PartyPresentationCancelButtonDelegate = initializationData.PartyPresentationCancelButtonDelegate;
			this.IsTroopUpgradesDisabled = (initializationData.IsTroopUpgradesDisabled || initializationData.RightOwnerParty == null);
			this.MemberTransferState = initializationData.MemberTransferState;
			this.PrisonerTransferState = initializationData.PrisonerTransferState;
			this.AccompanyingTransferState = initializationData.AccompanyingTransferState;
			this.IsTroopTransferableDelegate = initializationData.TroopTransferableDelegate;
			this.PartyPresentationCancelButtonActivateDelegate = initializationData.PartyPresentationCancelButtonActivateDelegate;
			this.PartyPresentationCancelButtonDelegate = initializationData.PartyPresentationCancelButtonDelegate;
			this.PartyScreenClosedEvent = initializationData.PartyScreenClosedDelegate;
			this.ShowProgressBar = initializationData.ShowProgressBar;
			if (this.CurrentMode == PartyScreenMode.QuestTroopManage)
			{
				int partyGoldChangeAmount = -this.MemberRosters[0].Sum((TroopRosterElement t) => t.Character.TroopWage * t.Number * this.QuestModeWageDaysMultiplier);
				this._initialData.PartyGoldChangeAmount = partyGoldChangeAmount;
				this.SetPartyGoldChangeAmount(partyGoldChangeAmount);
			}
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000A5277 File Offset: 0x000A3477
		private void SetPartyGoldChangeAmount(int newTotalAmount)
		{
			this.CurrentData.PartyGoldChangeAmount = newTotalAmount;
			PartyScreenLogic.PartyGoldDelegate partyGoldChange = this.PartyGoldChange;
			if (partyGoldChange == null)
			{
				return;
			}
			partyGoldChange();
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000A5295 File Offset: 0x000A3495
		private void SetMoraleChangeAmount(int newAmount)
		{
			this.CurrentData.PartyMoraleChangeAmount = newAmount;
			PartyScreenLogic.PartyMoraleDelegate partyMoraleChange = this.PartyMoraleChange;
			if (partyMoraleChange == null)
			{
				return;
			}
			partyMoraleChange();
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000A52B3 File Offset: 0x000A34B3
		private void SetHorseChangeAmount(int newAmount)
		{
			this.CurrentData.PartyHorseChangeAmount = newAmount;
			PartyScreenLogic.PartyHorseDelegate partyHorseChange = this.PartyHorseChange;
			if (partyHorseChange == null)
			{
				return;
			}
			partyHorseChange();
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000A52D1 File Offset: 0x000A34D1
		private void SetInfluenceChangeAmount(int heroInfluence, int troopInfluence, int prisonerInfluence)
		{
			this.CurrentData.PartyInfluenceChangeAmount = new ValueTuple<int, int, int>(heroInfluence, troopInfluence, prisonerInfluence);
			PartyScreenLogic.PartyInfluenceDelegate partyInfluenceChange = this.PartyInfluenceChange;
			if (partyInfluenceChange == null)
			{
				return;
			}
			partyInfluenceChange();
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000A52F8 File Offset: 0x000A34F8
		private void ProcessCommand(PartyScreenLogic.PartyCommand command)
		{
			switch (command.Code)
			{
			case PartyScreenLogic.PartyCommandCode.TransferTroop:
				this.TransferTroop(command, true);
				return;
			case PartyScreenLogic.PartyCommandCode.UpgradeTroop:
				this.UpgradeTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop:
				this.TransferPartyLeaderTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot:
				this.TransferTroopToLeaderSlot(command);
				return;
			case PartyScreenLogic.PartyCommandCode.ShiftTroop:
				this.ShiftTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.RecruitTroop:
				this.RecruitPrisoner(command);
				return;
			case PartyScreenLogic.PartyCommandCode.ExecuteTroop:
				this.ExecuteTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferAllTroops:
				this.TransferAllTroops(command);
				return;
			case PartyScreenLogic.PartyCommandCode.SortTroops:
				this.SortTroops(command);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002721 RID: 10017 RVA: 0x000A537F File Offset: 0x000A357F
		public void AddCommand(PartyScreenLogic.PartyCommand command)
		{
			this.ProcessCommand(command);
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x000A5388 File Offset: 0x000A3588
		public bool ValidateCommand(PartyScreenLogic.PartyCommand command)
		{
			if (command.Code == PartyScreenLogic.PartyCommandCode.TransferTroop || command.Code == PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot)
			{
				CharacterObject character = command.Character;
				if (character == CharacterObject.PlayerCharacter)
				{
					return false;
				}
				int num;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					bool flag = num != -1 && this.MemberRosters[(int)command.RosterSide].GetElementNumber(num) >= command.TotalNumber;
					bool flag2 = command.RosterSide != PartyScreenLogic.PartyRosterSide.Left || command.Index != 0;
					return flag && flag2;
				}
				num = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character);
				return num != -1 && this.PrisonerRosters[(int)command.RosterSide].GetElementNumber(num) >= command.TotalNumber;
			}
			else if (command.Code == PartyScreenLogic.PartyCommandCode.ShiftTroop)
			{
				CharacterObject character2 = command.Character;
				if (character2 == this.LeftPartyLeader || character2 == this.RightPartyLeader || ((command.RosterSide != PartyScreenLogic.PartyRosterSide.Left || (this.LeftPartyLeader != null && command.Index == 0)) && (command.RosterSide != PartyScreenLogic.PartyRosterSide.Right || (this.RightPartyLeader != null && command.Index == 0))))
				{
					return false;
				}
				int num2;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					num2 = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character2);
					return num2 != -1 && num2 != command.Index;
				}
				num2 = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character2);
				return num2 != -1 && num2 != command.Index;
			}
			else
			{
				if (command.Code == PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop)
				{
					CharacterObject character3 = command.Character;
					BasicCharacterObject playerTroop = this._game.PlayerTroop;
					return false;
				}
				if (command.Code == PartyScreenLogic.PartyCommandCode.UpgradeTroop)
				{
					CharacterObject character4 = command.Character;
					int num3 = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character4);
					if (num3 == -1 || this.MemberRosters[(int)command.RosterSide].GetElementNumber(num3) < command.TotalNumber || character4.UpgradeTargets.Length == 0)
					{
						return false;
					}
					if (command.UpgradeTarget >= character4.UpgradeTargets.Length)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=kaQ7DsW3}Character does not have upgrade target.", null), 0, null, "");
						return false;
					}
					CharacterObject characterObject = character4.UpgradeTargets[command.UpgradeTarget];
					int upgradeXpCost = character4.GetUpgradeXpCost(PartyBase.MainParty, command.UpgradeTarget);
					int upgradeGoldCost = character4.GetUpgradeGoldCost(PartyBase.MainParty, command.UpgradeTarget);
					if (this.MemberRosters[(int)command.RosterSide].GetElementXp(num3) < upgradeXpCost * command.TotalNumber)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=m1bIfPf1}Character does not have enough experience for upgrade.", null), 0, null, "");
						return false;
					}
					CharacterObject characterObject2 = (command.RosterSide == PartyScreenLogic.PartyRosterSide.Left) ? this.LeftPartyLeader : this.RightPartyLeader;
					int? num4 = ((characterObject2 != null) ? new int?(characterObject2.HeroObject.Gold) : null) + this.CurrentData.PartyGoldChangeAmount;
					int num5 = upgradeGoldCost * command.TotalNumber;
					if (!(num4.GetValueOrDefault() >= num5 & num4 != null))
					{
						MBTextManager.SetTextVariable("VALUE", upgradeGoldCost);
						MBInformationManager.AddQuickInformation(GameTexts.FindText("str_gold_needed_for_upgrade", null), 0, null, "");
						return false;
					}
					if (characterObject.UpgradeRequiresItemFromCategory == null)
					{
						return true;
					}
					foreach (ItemRosterElement itemRosterElement in this.RightOwnerParty.ItemRoster)
					{
						if (itemRosterElement.EquipmentElement.Item.ItemCategory == characterObject.UpgradeRequiresItemFromCategory)
						{
							return true;
						}
					}
					MBTextManager.SetTextVariable("REQUIRED_ITEM", characterObject.UpgradeRequiresItemFromCategory.GetName(), false);
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_item_needed_for_upgrade", null), 0, null, "");
					return false;
				}
				else
				{
					if (command.Code == PartyScreenLogic.PartyCommandCode.RecruitTroop)
					{
						return this.IsPrisonerRecruitable(command.Type, command.Character, command.RosterSide);
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.ExecuteTroop)
					{
						return this.IsExecutable(command.Type, command.Character, command.RosterSide);
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.TransferAllTroops)
					{
						return this.GetRoster(command.RosterSide, command.Type).Count != 0;
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.SortTroops)
					{
						return this.GetActiveSortTypeForSide(command.RosterSide) != command.SortType || this.GetIsAscendingSortForSide(command.RosterSide) != command.IsSortAscending;
					}
					throw new MBUnknownTypeException("Unknown command type in ValidateCommand.");
				}
			}
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x000A5838 File Offset: 0x000A3A38
		private void OnReset(bool fromCancel)
		{
			PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
			if (afterReset == null)
			{
				return;
			}
			afterReset(this, fromCancel);
		}

		// Token: 0x06002724 RID: 10020 RVA: 0x000A584C File Offset: 0x000A3A4C
		protected void TransferTroopToLeaderSlot(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					int index = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					TroopRosterElement elementCopyAtIndex = this.MemberRosters[(int)command.RosterSide].GetElementCopyAtIndex(index);
					int num = command.TotalNumber * (elementCopyAtIndex.Xp / elementCopyAtIndex.Number);
					this.MemberRosters[(int)command.RosterSide].AddToCounts(character, -command.TotalNumber, false, -command.WoundedNumber, 0, true, index);
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddToCounts(character, command.TotalNumber, false, command.WoundedNumber, 0, true, 0);
					if (elementCopyAtIndex.Number != command.TotalNumber)
					{
						this.MemberRosters[(int)command.RosterSide].AddXpToTroop(-num, character);
					}
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddXpToTroop(num, character);
				}
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002725 RID: 10021 RVA: 0x000A596C File Offset: 0x000A3B6C
		protected void TransferTroop(PartyScreenLogic.PartyCommand command, bool invokeUpdate)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject troop = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					int index = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(troop);
					TroopRosterElement elementCopyAtIndex = this.MemberRosters[(int)command.RosterSide].GetElementCopyAtIndex(index);
					int num = (troop.UpgradeTargets.Length != 0) ? troop.UpgradeTargets.Max((CharacterObject x) => Campaign.Current.Models.PartyTroopUpgradeModel.GetXpCostForUpgrade(PartyBase.MainParty, troop, x)) : 0;
					int num3;
					if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
					{
						int num2 = (elementCopyAtIndex.Number - command.TotalNumber) * num;
						num3 = ((elementCopyAtIndex.Xp >= num2 && num2 >= 0) ? (elementCopyAtIndex.Xp - num2) : 0);
					}
					else
					{
						int num4 = command.TotalNumber * num;
						num3 = ((elementCopyAtIndex.Xp > num4 && num4 >= 0) ? num4 : elementCopyAtIndex.Xp);
						this.MemberRosters[(int)command.RosterSide].AddXpToTroop(-num3, troop);
					}
					this.MemberRosters[(int)command.RosterSide].AddToCounts(troop, -command.TotalNumber, false, -command.WoundedNumber, 0, false, index);
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddToCounts(troop, command.TotalNumber, false, command.WoundedNumber, 0, false, command.Index);
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddXpToTroop(num3, troop);
				}
				else
				{
					int index2 = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(troop);
					TroopRosterElement elementCopyAtIndex2 = this.PrisonerRosters[(int)command.RosterSide].GetElementCopyAtIndex(index2);
					int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(elementCopyAtIndex2.Character);
					int num6;
					if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
					{
						this.UpdatePrisonerTransferHistory(troop, -command.TotalNumber);
						int num5 = (elementCopyAtIndex2.Number - command.TotalNumber) * conformityNeededToRecruitPrisoner;
						num6 = ((elementCopyAtIndex2.Xp >= num5 && num5 >= 0) ? (elementCopyAtIndex2.Xp - num5) : 0);
					}
					else
					{
						this.UpdatePrisonerTransferHistory(troop, command.TotalNumber);
						int num7 = command.TotalNumber * conformityNeededToRecruitPrisoner;
						num6 = ((elementCopyAtIndex2.Xp > num7 && num7 >= 0) ? num7 : elementCopyAtIndex2.Xp);
						this.PrisonerRosters[(int)command.RosterSide].AddXpToTroop(-num6, troop);
					}
					this.PrisonerRosters[(int)command.RosterSide].AddToCounts(troop, -command.TotalNumber, false, -command.WoundedNumber, 0, false, command.Index);
					this.PrisonerRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddToCounts(troop, command.TotalNumber, false, command.WoundedNumber, 0, false, command.Index);
					this.PrisonerRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddXpToTroop(num6, troop);
					if (this.CurrentData.RightRecruitableData.ContainsKey(troop))
					{
						this.CurrentData.RightRecruitableData[troop] = MathF.Max(MathF.Min(this.CurrentData.RightRecruitableData[troop], this.PrisonerRosters[1].GetElementNumber(troop)), Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, troop));
					}
				}
				flag = true;
			}
			if (flag)
			{
				if (this.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade && command.Type == PartyScreenLogic.TroopType.Prisoner)
				{
					int num8 = (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right) ? 1 : -1;
					this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount + Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(command.Character, Hero.MainHero) * command.TotalNumber * num8);
				}
				if (this.CurrentMode == PartyScreenMode.QuestTroopManage)
				{
					int num9 = (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right) ? -1 : 1;
					this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount + command.Character.TroopWage * command.TotalNumber * this.QuestModeWageDaysMultiplier * num9);
				}
				if (PartyScreenManager.Instance.IsDonating)
				{
					Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
					float num10 = 0f;
					float num11 = 0f;
					float num12 = 0f;
					foreach (TroopTradeDifference troopTradeDifference in this._initialData.GetTroopTradeDifferencesFromTo(this.CurrentData))
					{
						int num13 = troopTradeDifference.FromCount - troopTradeDifference.ToCount;
						if (num13 > 0)
						{
							if (!troopTradeDifference.IsPrisoner)
							{
								num11 += (float)num13 * Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterTroopDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
							else if (troopTradeDifference.Troop.IsHero)
							{
								num10 += Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterPrisonerDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
							else
							{
								num12 += (float)num13 * Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterPrisonerDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
						}
					}
					this.SetInfluenceChangeAmount((int)num10, (int)num11, (int)num12);
				}
				if (invokeUpdate)
				{
					PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
					if (updateDelegate != null)
					{
						updateDelegate(command);
					}
					PartyScreenLogic.PresentationUpdate update = this.Update;
					if (update == null)
					{
						return;
					}
					update(command);
				}
			}
		}

		// Token: 0x06002726 RID: 10022 RVA: 0x000A5F08 File Offset: 0x000A4108
		protected void ShiftTroop(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					int num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					int targetIndex = (num < command.Index) ? (command.Index - 1) : command.Index;
					this.MemberRosters[(int)command.RosterSide].ShiftTroopToIndex(num, targetIndex);
				}
				else
				{
					int num2 = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					this.PrisonerRosters[(int)command.RosterSide].GetElementCopyAtIndex(num2);
					int targetIndex2 = (num2 < command.Index) ? (command.Index - 1) : command.Index;
					this.PrisonerRosters[(int)command.RosterSide].ShiftTroopToIndex(num2, targetIndex2);
				}
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002727 RID: 10023 RVA: 0x000A5FFB File Offset: 0x000A41FB
		protected void TransferPartyLeaderTroop(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				PartyBase partyBase = (command.RosterSide == PartyScreenLogic.PartyRosterSide.Left) ? this.LeftOwnerParty : this.RightOwnerParty;
			}
		}

		// Token: 0x06002728 RID: 10024 RVA: 0x000A6020 File Offset: 0x000A4220
		protected void UpgradeTroop(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				CharacterObject characterObject = character.UpgradeTargets[command.UpgradeTarget];
				TroopRoster roster = this.GetRoster(command.RosterSide, command.Type);
				int index = roster.FindIndexOfTroop(character);
				int num = character.GetUpgradeXpCost(PartyBase.MainParty, command.UpgradeTarget) * command.TotalNumber;
				roster.SetElementXp(index, roster.GetElementXp(index) - num);
				List<ValueTuple<EquipmentElement, int>> usedHorses = null;
				this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount - character.GetUpgradeGoldCost(PartyBase.MainParty, command.UpgradeTarget) * command.TotalNumber);
				if (characterObject.UpgradeRequiresItemFromCategory != null)
				{
					usedHorses = this.RemoveItemFromItemRoster(characterObject.UpgradeRequiresItemFromCategory, command.TotalNumber);
				}
				int num2 = 0;
				foreach (TroopRosterElement troopRosterElement in roster.GetTroopRoster())
				{
					if (troopRosterElement.Character == character && command.TotalNumber > troopRosterElement.Number - troopRosterElement.WoundedNumber)
					{
						num2 = command.TotalNumber - (troopRosterElement.Number - troopRosterElement.WoundedNumber);
					}
				}
				roster.AddToCounts(character, -command.TotalNumber, false, -num2, 0, true, -1);
				roster.AddToCounts(characterObject, command.TotalNumber, false, num2, 0, true, command.Index);
				this.AddUpgradeToHistory(character, characterObject, command.TotalNumber);
				this.AddUsedHorsesToHistory(usedHorses);
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate == null)
				{
					return;
				}
				updateDelegate(command);
			}
		}

		// Token: 0x06002729 RID: 10025 RVA: 0x000A61B0 File Offset: 0x000A43B0
		protected void RecruitPrisoner(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				TroopRoster troopRoster = this.PrisonerRosters[(int)command.RosterSide];
				int num = MathF.Min(this.CurrentData.RightRecruitableData[character], command.TotalNumber);
				if (num > 0)
				{
					Dictionary<CharacterObject, int> rightRecruitableData = this.CurrentData.RightRecruitableData;
					CharacterObject key = character;
					rightRecruitableData[key] -= num;
					int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(character);
					troopRoster.AddXpToTroop(-conformityNeededToRecruitPrisoner * num, character);
					troopRoster.AddToCounts(character, -num, false, 0, 0, true, -1);
					this.MemberRosters[(int)command.RosterSide].AddToCounts(command.Character, num, false, 0, 0, true, command.Index);
					this.AddRecruitToHistory(character, num);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x000A62B4 File Offset: 0x000A44B4
		protected void ExecuteTroop(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				this.PrisonerRosters[(int)command.RosterSide].AddToCounts(character, -1, false, 0, 0, true, -1);
				KillCharacterAction.ApplyByExecution(character.HeroObject, Hero.MainHero, true, false);
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update != null)
				{
					update(command);
				}
				this._initialData.LeftPrisonerRoster.AddToCounts(command.Character, -1, false, 0, 0, true, -1);
				this._initialData.RightPrisonerRoster.AddToCounts(command.Character, -1, false, 0, 0, true, -1);
			}
		}

		// Token: 0x0600272B RID: 10027 RVA: 0x000A6364 File Offset: 0x000A4564
		protected void TransferAllTroops(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				PartyScreenLogic.PartyRosterSide side = PartyScreenLogic.PartyRosterSide.Right - command.RosterSide;
				TroopRoster roster = this.GetRoster(command.RosterSide, command.Type);
				List<TroopRosterElement> listFromRoster = this.GetListFromRoster(roster);
				int num = -1;
				if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
				{
					if (command.Type == PartyScreenLogic.TroopType.Prisoner)
					{
						num = this.LeftPartyPrisonersSizeLimit - this.PrisonerRosters[0].TotalManCount;
					}
					else
					{
						num = this.LeftPartyMembersSizeLimit - this.MemberRosters[0].TotalManCount;
					}
				}
				else if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Left)
				{
					if (command.Type == PartyScreenLogic.TroopType.Prisoner)
					{
						num = this.RightPartyPrisonersSizeLimit - this.PrisonerRosters[1].TotalManCount;
					}
					else
					{
						num = this.RightPartyMembersSizeLimit - this.MemberRosters[1].TotalManCount;
					}
				}
				if (num <= 0)
				{
					num = listFromRoster.Sum((TroopRosterElement x) => x.Number);
				}
				IEnumerable<string> source = (command.Type == PartyScreenLogic.TroopType.Member) ? Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyTroopLocks() : Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyPrisonerLocks();
				int num2 = 0;
				while (num2 < listFromRoster.Count && num > 0)
				{
					TroopRosterElement troopRosterElement = listFromRoster[num2];
					if ((command.RosterSide != PartyScreenLogic.PartyRosterSide.Right || !source.Contains(troopRosterElement.Character.StringId)) && this.IsTroopTransferable(command.Type, troopRosterElement.Character, (int)command.RosterSide))
					{
						PartyScreenLogic.PartyCommand partyCommand = new PartyScreenLogic.PartyCommand();
						int num3 = MBMath.ClampInt(troopRosterElement.Number, 0, num);
						partyCommand.FillForTransferTroop(command.RosterSide, command.Type, troopRosterElement.Character, num3, troopRosterElement.WoundedNumber, -1);
						this.TransferTroop(partyCommand, false);
						num -= num3;
					}
					num2++;
				}
				PartyScreenLogic.TroopSortType activeSortTypeForSide = this.GetActiveSortTypeForSide(side);
				if (activeSortTypeForSide != PartyScreenLogic.TroopSortType.Custom)
				{
					TroopRoster roster2 = this.GetRoster(side, command.Type);
					this.SortRoster(roster2, activeSortTypeForSide);
				}
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x000A6570 File Offset: 0x000A4770
		protected void SortTroops(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				this.SetActiveSortTypeForSide(command.RosterSide, command.SortType);
				this.SetIsAscendingForSide(command.RosterSide, command.IsSortAscending);
				this.UpdateComparersAscendingOrder(command.IsSortAscending);
				if (command.SortType != PartyScreenLogic.TroopSortType.Custom)
				{
					TroopRoster roster = this.GetRoster(command.RosterSide, PartyScreenLogic.TroopType.Member);
					TroopRoster roster2 = this.GetRoster(command.RosterSide, PartyScreenLogic.TroopType.Prisoner);
					this.SortRoster(roster, command.SortType);
					this.SortRoster(roster2, command.SortType);
				}
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000A661C File Offset: 0x000A481C
		public int GetIndexToInsertTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, TroopRosterElement troop)
		{
			PartyScreenLogic.TroopSortType activeSortTypeForSide = this.GetActiveSortTypeForSide(side);
			if (activeSortTypeForSide != PartyScreenLogic.TroopSortType.Custom)
			{
				return -1;
			}
			PartyScreenLogic.TroopComparer comparer = this.GetComparer(activeSortTypeForSide);
			TroopRoster roster = this.GetRoster(side, type);
			for (int i = 0; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i);
				if (!elementCopyAtIndex.Character.IsHero)
				{
					if (elementCopyAtIndex.Character.StringId == troop.Character.StringId)
					{
						return -1;
					}
					if (comparer.Compare(elementCopyAtIndex, troop) < 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x000A669E File Offset: 0x000A489E
		public PartyScreenLogic.TroopSortType GetActiveSortTypeForSide(PartyScreenLogic.PartyRosterSide side)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				return this.ActiveOtherPartySortType;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				return this.ActiveMainPartySortType;
			}
			return PartyScreenLogic.TroopSortType.Invalid;
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000A66B6 File Offset: 0x000A48B6
		private void SetActiveSortTypeForSide(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopSortType sortType)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				this.ActiveOtherPartySortType = sortType;
				return;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				this.ActiveMainPartySortType = sortType;
			}
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000A66CE File Offset: 0x000A48CE
		public bool GetIsAscendingSortForSide(PartyScreenLogic.PartyRosterSide side)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				return this.IsOtherPartySortAscending;
			}
			return side == PartyScreenLogic.PartyRosterSide.Right && this.IsMainPartySortAscending;
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000A66E6 File Offset: 0x000A48E6
		private void SetIsAscendingForSide(PartyScreenLogic.PartyRosterSide side, bool isAscending)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				this.IsOtherPartySortAscending = isAscending;
				return;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				this.IsMainPartySortAscending = isAscending;
			}
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x000A6700 File Offset: 0x000A4900
		private List<TroopRosterElement> GetListFromRoster(TroopRoster roster)
		{
			List<TroopRosterElement> list = new List<TroopRosterElement>();
			for (int i = 0; i < roster.Count; i++)
			{
				list.Add(roster.GetElementCopyAtIndex(i));
			}
			return list;
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000A6734 File Offset: 0x000A4934
		private void SyncRosterWithList(TroopRoster roster, List<TroopRosterElement> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				TroopRosterElement troopRosterElement = list[i];
				int firstIndex = roster.FindIndexOfTroop(troopRosterElement.Character);
				roster.SwapTroopsAtIndices(firstIndex, i);
			}
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000A6770 File Offset: 0x000A4970
		[Conditional("DEBUG")]
		private void EnsureRosterIsSyncedWithList(TroopRoster roster, List<TroopRosterElement> list)
		{
			if (roster.Count != list.Count)
			{
				Debug.FailedAssert("Roster count is not synced with the list count", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "EnsureRosterIsSyncedWithList", 1045);
				return;
			}
			for (int i = 0; i < roster.Count; i++)
			{
				if (roster.GetCharacterAtIndex(i).StringId != list[i].Character.StringId)
				{
					Debug.FailedAssert("Roster is not synced with the list at index: " + i, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "EnsureRosterIsSyncedWithList", 1055);
					return;
				}
			}
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000A6800 File Offset: 0x000A4A00
		private void SortRoster(TroopRoster originalRoster, PartyScreenLogic.TroopSortType sortType)
		{
			PartyScreenLogic.TroopComparer comparer = this._defaultComparers[sortType];
			if (!this.IsRosterOrdered(originalRoster, comparer))
			{
				List<TroopRosterElement> listFromRoster = this.GetListFromRoster(originalRoster);
				listFromRoster.Sort(this._defaultComparers[sortType]);
				this.SyncRosterWithList(originalRoster, listFromRoster);
			}
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000A6848 File Offset: 0x000A4A48
		private bool IsRosterOrdered(TroopRoster roster, PartyScreenLogic.TroopComparer comparer)
		{
			for (int i = 1; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i - 1);
				TroopRosterElement elementCopyAtIndex2 = roster.GetElementCopyAtIndex(i);
				if (comparer.Compare(elementCopyAtIndex, elementCopyAtIndex2) >= 1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000A6888 File Offset: 0x000A4A88
		public bool IsDoneActive()
		{
			object obj = Hero.MainHero.Gold < -this.CurrentData.PartyGoldChangeAmount && this.CurrentData.PartyGoldChangeAmount < 0;
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = this.PartyPresentationDoneButtonConditionDelegate;
			Tuple<bool, TextObject> tuple = (partyPresentationDoneButtonConditionDelegate != null) ? partyPresentationDoneButtonConditionDelegate(this.MemberRosters[0], this.PrisonerRosters[0], this.MemberRosters[1], this.PrisonerRosters[1], this.LeftPartyMembersSizeLimit, 0) : null;
			bool flag = this.PartyPresentationDoneButtonConditionDelegate == null || (tuple != null && tuple.Item1);
			this.DoneReasonString = null;
			object obj2 = obj;
			if (obj2 != null)
			{
				this.DoneReasonString = GameTexts.FindText("str_inventory_popup_player_not_enough_gold", null).ToString();
			}
			else
			{
				string text;
				if (tuple == null)
				{
					text = null;
				}
				else
				{
					TextObject item = tuple.Item2;
					text = ((item != null) ? item.ToString() : null);
				}
				this.DoneReasonString = (text ?? string.Empty);
			}
			return obj2 == 0 && flag;
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000A695E File Offset: 0x000A4B5E
		public bool IsCancelActive()
		{
			return this.PartyPresentationCancelButtonActivateDelegate == null || this.PartyPresentationCancelButtonActivateDelegate();
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000A6978 File Offset: 0x000A4B78
		public bool DoneLogic(bool isForced)
		{
			if (Hero.MainHero.Gold < -this.CurrentData.PartyGoldChangeAmount && this.CurrentData.PartyGoldChangeAmount < 0)
			{
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_inventory_popup_player_not_enough_gold", null), 0, null, "");
				return false;
			}
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			FlattenedTroopRoster flattenedTroopRoster2 = new FlattenedTroopRoster(4);
			foreach (Tuple<CharacterObject, int> tuple in this.CurrentData.TransferredPrisonersHistory)
			{
				int number = MathF.Abs(tuple.Item2);
				if (tuple.Item2 < 0)
				{
					flattenedTroopRoster.Add(tuple.Item1, number, 0);
				}
				else if (tuple.Item2 > 0)
				{
					flattenedTroopRoster2.Add(tuple.Item1, number, 0);
				}
			}
			if (Settlement.CurrentSettlement != null && !flattenedTroopRoster2.IsEmpty<FlattenedTroopRosterElement>())
			{
				CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(Settlement.CurrentSettlement, flattenedTroopRoster2, null, true);
			}
			bool flag = this.PartyPresentationDoneButtonDelegate(this.MemberRosters[0], this.PrisonerRosters[0], this.MemberRosters[1], this.PrisonerRosters[1], flattenedTroopRoster2, flattenedTroopRoster, isForced, this.LeftOwnerParty, this.RightOwnerParty);
			if (flag)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.CurrentData.PartyGoldChangeAmount, false);
				if (this.CurrentData.PartyInfluenceChangeAmount.Item2 != 0)
				{
					GainKingdomInfluenceAction.ApplyForLeavingTroopToGarrison(Hero.MainHero, (float)this.CurrentData.PartyInfluenceChangeAmount.Item2);
				}
				this.FireCampaignRelatedEvents();
				this.SetPartyGoldChangeAmount(0);
				this.SetHorseChangeAmount(0);
				this.SetInfluenceChangeAmount(0, 0, 0);
				this.SetMoraleChangeAmount(0);
				this.CurrentData.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>();
				this.CurrentData.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>();
				this.CurrentData.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>();
				this.CurrentData.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>();
				this._initialData.CopyFromScreenData(this.CurrentData);
			}
			return flag;
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000A6B70 File Offset: 0x000A4D70
		public void OnPartyScreenClosed(bool fromCancel)
		{
			if (fromCancel)
			{
				PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = this.PartyPresentationCancelButtonDelegate;
				if (partyPresentationCancelButtonDelegate != null)
				{
					partyPresentationCancelButtonDelegate();
				}
			}
			PartyScreenClosedDelegate partyScreenClosedEvent = this.PartyScreenClosedEvent;
			if (partyScreenClosedEvent == null)
			{
				return;
			}
			partyScreenClosedEvent(this.LeftOwnerParty, this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty, this.MemberRosters[1], this.PrisonerRosters[1], fromCancel);
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x000A6BD0 File Offset: 0x000A4DD0
		private void UpdateComparersAscendingOrder(bool isAscending)
		{
			foreach (KeyValuePair<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer> keyValuePair in this._defaultComparers)
			{
				keyValuePair.Value.SetIsAscending(isAscending);
			}
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000A6C2C File Offset: 0x000A4E2C
		private void FireCampaignRelatedEvents()
		{
			foreach (Tuple<CharacterObject, CharacterObject, int> tuple in this.CurrentData.UpgradedTroopsHistory)
			{
				CampaignEventDispatcher.Instance.OnPlayerUpgradedTroops(tuple.Item1, tuple.Item2, tuple.Item3);
			}
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			foreach (Tuple<CharacterObject, int> tuple2 in this.CurrentData.RecruitedPrisonersHistory)
			{
				flattenedTroopRoster.Add(tuple2.Item1, tuple2.Item2, 0);
			}
			if (!flattenedTroopRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				CampaignEventDispatcher.Instance.OnMainPartyPrisonerRecruited(flattenedTroopRoster);
			}
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000A6D0C File Offset: 0x000A4F0C
		public bool IsTroopTransferable(PartyScreenLogic.TroopType troopType, CharacterObject character, int side)
		{
			return this.IsTroopRosterTransferable(troopType) && !character.IsNotTransferableInPartyScreen && character != CharacterObject.PlayerCharacter && (this.IsTroopTransferableDelegate == null || this.IsTroopTransferableDelegate(character, troopType, (PartyScreenLogic.PartyRosterSide)side, this.LeftOwnerParty));
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000A6D48 File Offset: 0x000A4F48
		public bool IsTroopRosterTransferable(PartyScreenLogic.TroopType troopType)
		{
			if (troopType == PartyScreenLogic.TroopType.Prisoner)
			{
				return this.PrisonerTransferState == PartyScreenLogic.TransferState.Transferable || this.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade;
			}
			return troopType == PartyScreenLogic.TroopType.Member && (this.MemberTransferState == PartyScreenLogic.TransferState.Transferable || this.MemberTransferState == PartyScreenLogic.TransferState.TransferableWithTrade);
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000A6D7D File Offset: 0x000A4F7D
		public bool IsPrisonerRecruitable(PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
		{
			return side == PartyScreenLogic.PartyRosterSide.Right && troopType == PartyScreenLogic.TroopType.Prisoner && !character.IsHero && this.CurrentData.RightRecruitableData.ContainsKey(character) && this.CurrentData.RightRecruitableData[character] > 0;
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000A6DBC File Offset: 0x000A4FBC
		public string GetRecruitableReasonText(CharacterObject character, bool isRecruitable, int troopCount, string fiveStackShortcutKeyText, string entireStackShortcutKeyText)
		{
			GameTexts.SetVariable("newline", "\n");
			if (isRecruitable)
			{
				if (!string.IsNullOrEmpty(entireStackShortcutKeyText))
				{
					GameTexts.SetVariable("KEY_NAME", entireStackShortcutKeyText);
					string content = GameTexts.FindText("str_entire_stack_shortcut_recruit_units", null).ToString();
					GameTexts.SetVariable("STR1", content);
					GameTexts.SetVariable("STR2", "");
					if (troopCount >= 5 && !string.IsNullOrEmpty(fiveStackShortcutKeyText))
					{
						GameTexts.SetVariable("KEY_NAME", fiveStackShortcutKeyText);
						string content2 = GameTexts.FindText("str_five_stack_shortcut_recruit_units", null).ToString();
						GameTexts.SetVariable("STR2", content2);
					}
					string content3 = GameTexts.FindText("str_string_newline_string", null).ToString();
					GameTexts.SetVariable("STR2", content3);
				}
				if (this.RightOwnerParty.PartySizeLimit <= this.MemberRosters[1].TotalManCount)
				{
					GameTexts.SetVariable("STR1", GameTexts.FindText("str_recruit_party_size_limit", null));
					return GameTexts.FindText("str_string_newline_string", null).ToString();
				}
				GameTexts.SetVariable("STR1", GameTexts.FindText("str_recruit_prisoner", null));
				return GameTexts.FindText("str_string_newline_string", null).ToString();
			}
			else
			{
				if (character.IsHero)
				{
					return GameTexts.FindText("str_cannot_recruit_hero", null).ToString();
				}
				return GameTexts.FindText("str_cannot_recruit_prisoner", null).ToString();
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000A6F04 File Offset: 0x000A5104
		public bool IsExecutable(PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
		{
			return troopType == PartyScreenLogic.TroopType.Prisoner && side == PartyScreenLogic.PartyRosterSide.Right && character.IsHero && character.HeroObject.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && (PlayerEncounter.Current == null || PlayerEncounter.Current.EncounterState == PlayerEncounterState.Begin) && FaceGen.GetMaturityTypeWithAge(character.Age) > BodyMeshMaturityType.Tween;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000A6F66 File Offset: 0x000A5166
		public string GetExecutableReasonText(CharacterObject character, bool isExecutable)
		{
			if (isExecutable)
			{
				return GameTexts.FindText("str_execute_prisoner", null).ToString();
			}
			if (!character.IsHero)
			{
				return GameTexts.FindText("str_cannot_execute_nonhero", null).ToString();
			}
			return GameTexts.FindText("str_cannot_execute_hero", null).ToString();
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000A6FA5 File Offset: 0x000A51A5
		public int GetCurrentQuestCurrentCount()
		{
			return this.MemberRosters[0].Sum((TroopRosterElement item) => item.Number);
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000A6FD3 File Offset: 0x000A51D3
		public int GetCurrentQuestRequiredCount()
		{
			return this.LeftPartyMembersSizeLimit;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000A6FDB File Offset: 0x000A51DB
		private static bool DefaultDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			return true;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000A6FE0 File Offset: 0x000A51E0
		private void AddUpgradeToHistory(CharacterObject fromTroop, CharacterObject toTroop, int num)
		{
			Tuple<CharacterObject, CharacterObject, int> tuple = this.CurrentData.UpgradedTroopsHistory.Find((Tuple<CharacterObject, CharacterObject, int> t) => t.Item1 == fromTroop && t.Item2 == toTroop);
			if (tuple != null)
			{
				int item = tuple.Item3;
				this.CurrentData.UpgradedTroopsHistory.Remove(tuple);
				this.CurrentData.UpgradedTroopsHistory.Add(new Tuple<CharacterObject, CharacterObject, int>(fromTroop, toTroop, num + item));
				return;
			}
			this.CurrentData.UpgradedTroopsHistory.Add(new Tuple<CharacterObject, CharacterObject, int>(fromTroop, toTroop, num));
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x000A7084 File Offset: 0x000A5284
		private void AddUsedHorsesToHistory(List<ValueTuple<EquipmentElement, int>> usedHorses)
		{
			if (usedHorses != null)
			{
				using (List<ValueTuple<EquipmentElement, int>>.Enumerator enumerator = usedHorses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<EquipmentElement, int> usedHorse = enumerator.Current;
						Tuple<EquipmentElement, int> tuple = this.CurrentData.UsedUpgradeHorsesHistory.Find((Tuple<EquipmentElement, int> t) => t.Equals(usedHorse.Item1));
						if (tuple != null)
						{
							int item = tuple.Item2;
							this.CurrentData.UsedUpgradeHorsesHistory.Remove(tuple);
							this.CurrentData.UsedUpgradeHorsesHistory.Add(new Tuple<EquipmentElement, int>(usedHorse.Item1, item + usedHorse.Item2));
						}
						else
						{
							this.CurrentData.UsedUpgradeHorsesHistory.Add(new Tuple<EquipmentElement, int>(usedHorse.Item1, usedHorse.Item2));
						}
					}
				}
				PartyScreenData currentData = this.CurrentData;
				this.SetHorseChangeAmount(currentData.PartyHorseChangeAmount += usedHorses.Sum((ValueTuple<EquipmentElement, int> t) => t.Item2));
			}
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000A71B8 File Offset: 0x000A53B8
		private void UpdatePrisonerTransferHistory(CharacterObject troop, int amount)
		{
			Tuple<CharacterObject, int> tuple = this.CurrentData.TransferredPrisonersHistory.Find((Tuple<CharacterObject, int> t) => t.Item1 == troop);
			if (tuple != null)
			{
				int item = tuple.Item2;
				this.CurrentData.TransferredPrisonersHistory.Remove(tuple);
				this.CurrentData.TransferredPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount + item));
				return;
			}
			this.CurrentData.TransferredPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount));
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000A7248 File Offset: 0x000A5448
		private void AddRecruitToHistory(CharacterObject troop, int amount)
		{
			Tuple<CharacterObject, int> tuple = this.CurrentData.RecruitedPrisonersHistory.Find((Tuple<CharacterObject, int> t) => t.Item1 == troop);
			if (tuple != null)
			{
				int item = tuple.Item2;
				this.CurrentData.RecruitedPrisonersHistory.Remove(tuple);
				this.CurrentData.RecruitedPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount + item));
			}
			else
			{
				this.CurrentData.RecruitedPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount));
			}
			int prisonerRecruitmentMoraleEffect = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetPrisonerRecruitmentMoraleEffect(this.RightOwnerParty, troop, amount);
			this.SetMoraleChangeAmount(this.CurrentData.PartyMoraleChangeAmount + prisonerRecruitmentMoraleEffect);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000A730C File Offset: 0x000A550C
		private string GetItemLockStringID(EquipmentElement equipmentElement)
		{
			return equipmentElement.Item.StringId + ((equipmentElement.ItemModifier != null) ? equipmentElement.ItemModifier.StringId : "");
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000A733C File Offset: 0x000A553C
		private List<ValueTuple<EquipmentElement, int>> RemoveItemFromItemRoster(ItemCategory itemCategory, int numOfItemsLeftToRemove = 1)
		{
			List<ValueTuple<EquipmentElement, int>> list = new List<ValueTuple<EquipmentElement, int>>();
			IEnumerable<string> lockedItems = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetInventoryLocks();
			foreach (ItemRosterElement itemRosterElement in from x in this.RightOwnerParty.ItemRoster.Where(delegate(ItemRosterElement x)
			{
				ItemObject item = x.EquipmentElement.Item;
				return ((item != null) ? item.ItemCategory : null) == itemCategory;
			})
			orderby x.EquipmentElement.Item.Value
			orderby lockedItems.Contains(this.GetItemLockStringID(x.EquipmentElement))
			select x)
			{
				int num = MathF.Min(numOfItemsLeftToRemove, itemRosterElement.Amount);
				this.RightOwnerParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num);
				numOfItemsLeftToRemove -= num;
				list.Add(new ValueTuple<EquipmentElement, int>(itemRosterElement.EquipmentElement, num));
				if (numOfItemsLeftToRemove <= 0)
				{
					break;
				}
			}
			if (numOfItemsLeftToRemove > 0)
			{
				Debug.FailedAssert("Couldn't find enough upgrade req items in the inventory.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "RemoveItemFromItemRoster", 1478);
			}
			return list;
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000A7464 File Offset: 0x000A5664
		public void Reset(bool fromCancel)
		{
			this.ResetLogic(fromCancel);
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x000A746D File Offset: 0x000A566D
		private void ResetLogic(bool fromCancel)
		{
			if (this.CurrentData != this._initialData)
			{
				this.CurrentData.ResetUsing(this._initialData);
				PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
				if (afterReset == null)
				{
					return;
				}
				afterReset(this, fromCancel);
			}
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000A74A5 File Offset: 0x000A56A5
		public void SavePartyScreenData()
		{
			this._savedData = new PartyScreenData();
			this._savedData.InitializeCopyFrom(this.CurrentData.RightParty, this.CurrentData.LeftParty);
			this._savedData.CopyFromScreenData(this.CurrentData);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000A74E4 File Offset: 0x000A56E4
		public void ResetToLastSavedPartyScreenData(bool fromCancel)
		{
			if (this.CurrentData != this._savedData)
			{
				this.CurrentData.ResetUsing(this._savedData);
				PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
				if (afterReset == null)
				{
					return;
				}
				afterReset(this, fromCancel);
			}
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000A751C File Offset: 0x000A571C
		public void RemoveZeroCounts()
		{
			for (int i = 0; i < this.MemberRosters.Length; i++)
			{
				this.MemberRosters[i].RemoveZeroCounts();
			}
			for (int j = 0; j < this.PrisonerRosters.Length; j++)
			{
				this.PrisonerRosters[j].RemoveZeroCounts();
			}
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000A7569 File Offset: 0x000A5769
		public int GetTroopRecruitableAmount(CharacterObject troop)
		{
			if (!this.CurrentData.RightRecruitableData.ContainsKey(troop))
			{
				return 0;
			}
			return this.CurrentData.RightRecruitableData[troop];
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000A7591 File Offset: 0x000A5791
		public TroopRoster GetRoster(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType troopType)
		{
			if (troopType == PartyScreenLogic.TroopType.Member)
			{
				return this.MemberRosters[(int)side];
			}
			if (troopType == PartyScreenLogic.TroopType.Prisoner)
			{
				return this.PrisonerRosters[(int)side];
			}
			return null;
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000A75AE File Offset: 0x000A57AE
		internal void OnDoneEvent(List<TroopTradeDifference> freshlySellList)
		{
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000A75B0 File Offset: 0x000A57B0
		public bool IsThereAnyChanges()
		{
			return this._initialData.IsThereAnyTroopTradeDifferenceBetween(this.CurrentData);
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000A75C4 File Offset: 0x000A57C4
		public bool HaveRightSideGainedTroops()
		{
			foreach (TroopTradeDifference troopTradeDifference in this._initialData.GetTroopTradeDifferencesFromTo(this.CurrentData))
			{
				if (!troopTradeDifference.IsPrisoner && troopTradeDifference.FromCount < troopTradeDifference.ToCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000A763C File Offset: 0x000A583C
		public PartyScreenLogic.TroopComparer GetComparer(PartyScreenLogic.TroopSortType sortType)
		{
			return this._defaultComparers[sortType];
		}

		// Token: 0x04000BB7 RID: 2999
		public PartyPresentationDoneButtonDelegate PartyPresentationDoneButtonDelegate;

		// Token: 0x04000BB8 RID: 3000
		public PartyPresentationDoneButtonConditionDelegate PartyPresentationDoneButtonConditionDelegate;

		// Token: 0x04000BB9 RID: 3001
		public PartyPresentationCancelButtonActivateDelegate PartyPresentationCancelButtonActivateDelegate;

		// Token: 0x04000BBA RID: 3002
		public PartyPresentationCancelButtonDelegate PartyPresentationCancelButtonDelegate;

		// Token: 0x04000BBB RID: 3003
		public PartyScreenLogic.PresentationUpdate UpdateDelegate;

		// Token: 0x04000BBC RID: 3004
		public IsTroopTransferableDelegate IsTroopTransferableDelegate;

		// Token: 0x04000BC4 RID: 3012
		private PartyScreenLogic.TroopSortType _activeOtherPartySortType;

		// Token: 0x04000BC5 RID: 3013
		private PartyScreenLogic.TroopSortType _activeMainPartySortType;

		// Token: 0x04000BC6 RID: 3014
		private bool _isOtherPartySortAscending;

		// Token: 0x04000BC7 RID: 3015
		private bool _isMainPartySortAscending;

		// Token: 0x04000BDC RID: 3036
		public TroopRoster[] MemberRosters;

		// Token: 0x04000BDD RID: 3037
		public TroopRoster[] PrisonerRosters;

		// Token: 0x04000BDE RID: 3038
		public bool IsConsumablesChanges;

		// Token: 0x04000BDF RID: 3039
		private readonly Dictionary<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer> _defaultComparers;

		// Token: 0x04000BE0 RID: 3040
		private readonly PartyScreenData _initialData;

		// Token: 0x04000BE1 RID: 3041
		private PartyScreenData _savedData;

		// Token: 0x04000BE2 RID: 3042
		private Game _game;

		// Token: 0x020005B5 RID: 1461
		public enum TroopSortType
		{
			// Token: 0x040017C5 RID: 6085
			Invalid = -1,
			// Token: 0x040017C6 RID: 6086
			Custom,
			// Token: 0x040017C7 RID: 6087
			Type,
			// Token: 0x040017C8 RID: 6088
			Name,
			// Token: 0x040017C9 RID: 6089
			Count,
			// Token: 0x040017CA RID: 6090
			Tier
		}

		// Token: 0x020005B6 RID: 1462
		public enum PartyRosterSide : byte
		{
			// Token: 0x040017CC RID: 6092
			None = 99,
			// Token: 0x040017CD RID: 6093
			Right = 1,
			// Token: 0x040017CE RID: 6094
			Left = 0
		}

		// Token: 0x020005B7 RID: 1463
		[Flags]
		public enum TroopType
		{
			// Token: 0x040017D0 RID: 6096
			Member = 1,
			// Token: 0x040017D1 RID: 6097
			Prisoner = 2,
			// Token: 0x040017D2 RID: 6098
			None = 3
		}

		// Token: 0x020005B8 RID: 1464
		public enum PartyCommandCode
		{
			// Token: 0x040017D4 RID: 6100
			TransferTroop,
			// Token: 0x040017D5 RID: 6101
			UpgradeTroop,
			// Token: 0x040017D6 RID: 6102
			TransferPartyLeaderTroop,
			// Token: 0x040017D7 RID: 6103
			TransferTroopToLeaderSlot,
			// Token: 0x040017D8 RID: 6104
			ShiftTroop,
			// Token: 0x040017D9 RID: 6105
			RecruitTroop,
			// Token: 0x040017DA RID: 6106
			ExecuteTroop,
			// Token: 0x040017DB RID: 6107
			TransferAllTroops,
			// Token: 0x040017DC RID: 6108
			SortTroops
		}

		// Token: 0x020005B9 RID: 1465
		public enum TransferState
		{
			// Token: 0x040017DE RID: 6110
			NotTransferable,
			// Token: 0x040017DF RID: 6111
			Transferable,
			// Token: 0x040017E0 RID: 6112
			TransferableWithTrade
		}

		// Token: 0x020005BA RID: 1466
		// (Invoke) Token: 0x0600470A RID: 18186
		public delegate void PresentationUpdate(PartyScreenLogic.PartyCommand command);

		// Token: 0x020005BB RID: 1467
		// (Invoke) Token: 0x0600470E RID: 18190
		public delegate void PartyGoldDelegate();

		// Token: 0x020005BC RID: 1468
		// (Invoke) Token: 0x06004712 RID: 18194
		public delegate void PartyMoraleDelegate();

		// Token: 0x020005BD RID: 1469
		// (Invoke) Token: 0x06004716 RID: 18198
		public delegate void PartyInfluenceDelegate();

		// Token: 0x020005BE RID: 1470
		// (Invoke) Token: 0x0600471A RID: 18202
		public delegate void PartyHorseDelegate();

		// Token: 0x020005BF RID: 1471
		// (Invoke) Token: 0x0600471E RID: 18206
		public delegate void AfterResetDelegate(PartyScreenLogic partyScreenLogic, bool fromCancel);

		// Token: 0x020005C0 RID: 1472
		public class PartyCommand : ISerializableObject
		{
			// Token: 0x17000DF9 RID: 3577
			// (get) Token: 0x06004721 RID: 18209 RVA: 0x0014CBE7 File Offset: 0x0014ADE7
			// (set) Token: 0x06004722 RID: 18210 RVA: 0x0014CBEF File Offset: 0x0014ADEF
			public PartyScreenLogic.PartyCommandCode Code { get; private set; }

			// Token: 0x17000DFA RID: 3578
			// (get) Token: 0x06004723 RID: 18211 RVA: 0x0014CBF8 File Offset: 0x0014ADF8
			// (set) Token: 0x06004724 RID: 18212 RVA: 0x0014CC00 File Offset: 0x0014AE00
			public PartyScreenLogic.PartyRosterSide RosterSide { get; private set; }

			// Token: 0x17000DFB RID: 3579
			// (get) Token: 0x06004725 RID: 18213 RVA: 0x0014CC09 File Offset: 0x0014AE09
			// (set) Token: 0x06004726 RID: 18214 RVA: 0x0014CC11 File Offset: 0x0014AE11
			public CharacterObject Character { get; private set; }

			// Token: 0x17000DFC RID: 3580
			// (get) Token: 0x06004727 RID: 18215 RVA: 0x0014CC1A File Offset: 0x0014AE1A
			// (set) Token: 0x06004728 RID: 18216 RVA: 0x0014CC22 File Offset: 0x0014AE22
			public int TotalNumber { get; private set; }

			// Token: 0x17000DFD RID: 3581
			// (get) Token: 0x06004729 RID: 18217 RVA: 0x0014CC2B File Offset: 0x0014AE2B
			// (set) Token: 0x0600472A RID: 18218 RVA: 0x0014CC33 File Offset: 0x0014AE33
			public int WoundedNumber { get; private set; }

			// Token: 0x17000DFE RID: 3582
			// (get) Token: 0x0600472B RID: 18219 RVA: 0x0014CC3C File Offset: 0x0014AE3C
			// (set) Token: 0x0600472C RID: 18220 RVA: 0x0014CC44 File Offset: 0x0014AE44
			public int Index { get; private set; }

			// Token: 0x17000DFF RID: 3583
			// (get) Token: 0x0600472D RID: 18221 RVA: 0x0014CC4D File Offset: 0x0014AE4D
			// (set) Token: 0x0600472E RID: 18222 RVA: 0x0014CC55 File Offset: 0x0014AE55
			public int UpgradeTarget { get; private set; }

			// Token: 0x17000E00 RID: 3584
			// (get) Token: 0x0600472F RID: 18223 RVA: 0x0014CC5E File Offset: 0x0014AE5E
			// (set) Token: 0x06004730 RID: 18224 RVA: 0x0014CC66 File Offset: 0x0014AE66
			public PartyScreenLogic.TroopType Type { get; private set; }

			// Token: 0x17000E01 RID: 3585
			// (get) Token: 0x06004731 RID: 18225 RVA: 0x0014CC6F File Offset: 0x0014AE6F
			// (set) Token: 0x06004732 RID: 18226 RVA: 0x0014CC77 File Offset: 0x0014AE77
			public PartyScreenLogic.TroopSortType SortType { get; private set; }

			// Token: 0x17000E02 RID: 3586
			// (get) Token: 0x06004733 RID: 18227 RVA: 0x0014CC80 File Offset: 0x0014AE80
			// (set) Token: 0x06004734 RID: 18228 RVA: 0x0014CC88 File Offset: 0x0014AE88
			public bool IsSortAscending { get; private set; }

			// Token: 0x06004736 RID: 18230 RVA: 0x0014CC99 File Offset: 0x0014AE99
			public void FillForTransferTroop(PartyScreenLogic.PartyRosterSide fromSide, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber, int woundedNumber, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferTroop;
				this.RosterSide = fromSide;
				this.TotalNumber = totalNumber;
				this.WoundedNumber = woundedNumber;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x06004737 RID: 18231 RVA: 0x0014CCCF File Offset: 0x0014AECF
			public void FillForShiftTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.ShiftTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x06004738 RID: 18232 RVA: 0x0014CCF5 File Offset: 0x0014AEF5
			public void FillForTransferTroopToLeaderSlot(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber, int woundedNumber, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot;
				this.RosterSide = side;
				this.TotalNumber = totalNumber;
				this.WoundedNumber = woundedNumber;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x06004739 RID: 18233 RVA: 0x0014CD2B File Offset: 0x0014AF2B
			public void FillForTransferPartyLeaderTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop;
				this.RosterSide = side;
				this.TotalNumber = totalNumber;
				this.Character = character;
				this.Type = type;
			}

			// Token: 0x0600473A RID: 18234 RVA: 0x0014CD51 File Offset: 0x0014AF51
			public void FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int number, int upgradeTargetType, int index)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.UpgradeTroop;
				this.RosterSide = side;
				this.TotalNumber = number;
				this.Character = character;
				this.UpgradeTarget = upgradeTargetType;
				this.Type = type;
				this.Index = index;
			}

			// Token: 0x0600473B RID: 18235 RVA: 0x0014CD87 File Offset: 0x0014AF87
			public void FillForRecruitTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int number, int index)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.RecruitTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
				this.TotalNumber = number;
				this.Index = index;
			}

			// Token: 0x0600473C RID: 18236 RVA: 0x0014CDB5 File Offset: 0x0014AFB5
			public void FillForExecuteTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.ExecuteTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
			}

			// Token: 0x0600473D RID: 18237 RVA: 0x0014CDD3 File Offset: 0x0014AFD3
			public void FillForTransferAllTroops(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferAllTroops;
				this.RosterSide = side;
				this.Type = type;
			}

			// Token: 0x0600473E RID: 18238 RVA: 0x0014CDEA File Offset: 0x0014AFEA
			public void FillForSortTroops(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopSortType sortType, bool isAscending)
			{
				this.RosterSide = side;
				this.Code = PartyScreenLogic.PartyCommandCode.SortTroops;
				this.SortType = sortType;
				this.IsSortAscending = isAscending;
			}

			// Token: 0x0600473F RID: 18239 RVA: 0x0014CE08 File Offset: 0x0014B008
			void ISerializableObject.SerializeTo(IWriter writer)
			{
				writer.WriteByte((byte)this.Code);
				writer.WriteByte((byte)this.RosterSide);
				writer.WriteUInt(this.Character.Id.InternalValue);
				writer.WriteInt(this.TotalNumber);
				writer.WriteInt(this.WoundedNumber);
				writer.WriteInt(this.UpgradeTarget);
				writer.WriteByte((byte)this.Type);
			}

			// Token: 0x06004740 RID: 18240 RVA: 0x0014CE78 File Offset: 0x0014B078
			void ISerializableObject.DeserializeFrom(IReader reader)
			{
				this.Code = (PartyScreenLogic.PartyCommandCode)reader.ReadByte();
				this.RosterSide = (PartyScreenLogic.PartyRosterSide)reader.ReadByte();
				MBGUID objectId = new MBGUID(reader.ReadUInt());
				this.Character = (CharacterObject)MBObjectManager.Instance.GetObject(objectId);
				this.TotalNumber = reader.ReadInt();
				this.WoundedNumber = reader.ReadInt();
				this.UpgradeTarget = reader.ReadInt();
				this.Type = (PartyScreenLogic.TroopType)reader.ReadByte();
			}
		}

		// Token: 0x020005C1 RID: 1473
		public abstract class TroopComparer : IComparer<TroopRosterElement>
		{
			// Token: 0x06004741 RID: 18241 RVA: 0x0014CEF0 File Offset: 0x0014B0F0
			public void SetIsAscending(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06004742 RID: 18242 RVA: 0x0014CEF9 File Offset: 0x0014B0F9
			private int GetHeroComparisonResult(TroopRosterElement x, TroopRosterElement y)
			{
				if (x.Character.HeroObject != null)
				{
					if (x.Character.HeroObject == Hero.MainHero)
					{
						return -2;
					}
					if (y.Character.HeroObject == null)
					{
						return -1;
					}
				}
				return 0;
			}

			// Token: 0x06004743 RID: 18243 RVA: 0x0014CF30 File Offset: 0x0014B130
			public int Compare(TroopRosterElement x, TroopRosterElement y)
			{
				int num = this._isAscending ? 1 : -1;
				int heroComparisonResult = this.GetHeroComparisonResult(x, y);
				if (heroComparisonResult != 0)
				{
					return heroComparisonResult;
				}
				heroComparisonResult = this.GetHeroComparisonResult(y, x);
				if (heroComparisonResult != 0)
				{
					return heroComparisonResult * -1;
				}
				return this.CompareTroops(x, y) * num;
			}

			// Token: 0x06004744 RID: 18244
			protected abstract int CompareTroops(TroopRosterElement x, TroopRosterElement y);

			// Token: 0x040017EB RID: 6123
			private bool _isAscending;
		}

		// Token: 0x020005C2 RID: 1474
		private class TroopDefaultComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06004746 RID: 18246 RVA: 0x0014CF7A File Offset: 0x0014B17A
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return 0;
			}
		}

		// Token: 0x020005C3 RID: 1475
		private class TroopTypeComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06004748 RID: 18248 RVA: 0x0014CF88 File Offset: 0x0014B188
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				int defaultFormationClass = (int)x.Character.DefaultFormationClass;
				int defaultFormationClass2 = (int)y.Character.DefaultFormationClass;
				return defaultFormationClass.CompareTo(defaultFormationClass2);
			}
		}

		// Token: 0x020005C4 RID: 1476
		private class TroopNameComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x0600474A RID: 18250 RVA: 0x0014CFBD File Offset: 0x0014B1BD
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Character.Name.ToString().CompareTo(y.Character.Name.ToString());
			}
		}

		// Token: 0x020005C5 RID: 1477
		private class TroopCountComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x0600474C RID: 18252 RVA: 0x0014CFEC File Offset: 0x0014B1EC
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Number.CompareTo(y.Number);
			}
		}

		// Token: 0x020005C6 RID: 1478
		private class TroopTierComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x0600474E RID: 18254 RVA: 0x0014D018 File Offset: 0x0014B218
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Character.Tier.CompareTo(y.Character.Tier);
			}
		}
	}
}
