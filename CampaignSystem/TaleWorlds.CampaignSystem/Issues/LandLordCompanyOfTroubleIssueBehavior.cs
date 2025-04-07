using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x0200030C RID: 780
	public class LandLordCompanyOfTroubleIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000BD634 File Offset: 0x000BB834
		private static LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest Instance
		{
			get
			{
				LandLordCompanyOfTroubleIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<LandLordCompanyOfTroubleIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest cachedQuest;
						if ((cachedQuest = (enumerator.Current as LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)) != null)
						{
							campaignBehavior._cachedQuest = cachedQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000BD6CC File Offset: 0x000BB8CC
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x000BD6FC File Offset: 0x000BB8FC
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("company_of_trouble_menu", "", new OnInitDelegate(this.company_of_trouble_menu_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x000BD720 File Offset: 0x000BB920
		private void company_of_trouble_menu_on_init(MenuCallbackArgs args)
		{
			if (LandLordCompanyOfTroubleIssueBehavior.Instance != null)
			{
				if (LandLordCompanyOfTroubleIssueBehavior.Instance._checkForBattleResults)
				{
					bool flag = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Battle.PlayerSide;
					PlayerEncounter.Finish(true);
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty != null && LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty.IsActive)
					{
						DestroyPartyAction.Apply(null, LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty);
					}
					LandLordCompanyOfTroubleIssueBehavior.Instance._checkForBattleResults = false;
					if (flag)
					{
						LandLordCompanyOfTroubleIssueBehavior.Instance.QuestSuccessWithPlayerDefeatedCompany();
						return;
					}
					LandLordCompanyOfTroubleIssueBehavior.Instance.QuestFailWithPlayerDefeatedAgainstCompany();
					return;
				}
				else
				{
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._triggerCompanyOfTroubleConversation)
					{
						CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(LandLordCompanyOfTroubleIssueBehavior.Instance._troubleCharacterObject, PartyBase.MainParty, false, false, false, false, false, false));
						LandLordCompanyOfTroubleIssueBehavior.Instance._triggerCompanyOfTroubleConversation = false;
						return;
					}
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._battleWillStart)
					{
						PlayerEncounter.Start();
						PlayerEncounter.Current.SetupFields(PartyBase.MainParty, LandLordCompanyOfTroubleIssueBehavior.Instance._companyOfTroubleParty.Party);
						PlayerEncounter.StartBattle();
						CampaignMission.OpenBattleMission(PlayerEncounter.GetBattleSceneForMapPatch(Campaign.Current.MapSceneWrapper.GetMapPatchAtPosition(MobileParty.MainParty.Position2D)), false);
						LandLordCompanyOfTroubleIssueBehavior.Instance._battleWillStart = false;
						LandLordCompanyOfTroubleIssueBehavior.Instance._checkForBattleResults = true;
						return;
					}
					if (LandLordCompanyOfTroubleIssueBehavior.Instance._companyLeftQuestWillFail)
					{
						LandLordCompanyOfTroubleIssueBehavior.Instance.CompanyLeftQuestFail();
					}
				}
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000BD884 File Offset: 0x000BBA84
		public void OnCheckForIssue(Hero hero)
		{
			if (hero.IsLord && hero.Clan != Clan.PlayerClan && hero.PartyBelongedTo != null && !hero.IsMinorFactionHero && hero.GetTraitLevel(DefaultTraits.Mercy) <= 0)
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue), IssueBase.IssueFrequency.Rare, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000BD912 File Offset: 0x000BBB12
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			return new LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue(issueOwner);
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x000BD91A File Offset: 0x000BBB1A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04000D8E RID: 3470
		private const IssueBase.IssueFrequency LandLordCompanyOfTroubleIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x04000D8F RID: 3471
		private LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest _cachedQuest;

		// Token: 0x04000D90 RID: 3472
		private const int IssueDuration = 25;

		// Token: 0x02000636 RID: 1590
		public class LandLordCompanyOfTroubleIssue : IssueBase
		{
			// Token: 0x06004DA7 RID: 19879 RVA: 0x00163835 File Offset: 0x00161A35
			internal static void AutoGeneratedStaticCollectObjectsLandLordCompanyOfTroubleIssue(object o, List<object> collectedObjects)
			{
				((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004DA8 RID: 19880 RVA: 0x00163843 File Offset: 0x00161A43
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x17000FF5 RID: 4085
			// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x0016384C File Offset: 0x00161A4C
			private int CompanyTroopCount
			{
				get
				{
					return 5 + (int)(base.IssueDifficultyMultiplier * 30f);
				}
			}

			// Token: 0x17000FF6 RID: 4086
			// (get) Token: 0x06004DAA RID: 19882 RVA: 0x0016385D File Offset: 0x00161A5D
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000FF7 RID: 4087
			// (get) Token: 0x06004DAB RID: 19883 RVA: 0x00163860 File Offset: 0x00161A60
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000FF8 RID: 4088
			// (get) Token: 0x06004DAC RID: 19884 RVA: 0x00163863 File Offset: 0x00161A63
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=wrpsJM2u}Yes... I hired a band of mercenaries for a campaign some time back. But... normally mercenaries have their own peculiar kind of honor. You pay them, they fight for you, you don't, they go somewhere else. But these ones have made it pretty clear that if I don't keep renewing the contract, they'll turn bandit. I can't afford that right now.[if:convo_thinking][ib:closed]", null);
				}
			}

			// Token: 0x17000FF9 RID: 4089
			// (get) Token: 0x06004DAD RID: 19885 RVA: 0x00163870 File Offset: 0x00161A70
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=VlbCFDWu}What do you want from me?", null);
				}
			}

			// Token: 0x17000FFA RID: 4090
			// (get) Token: 0x06004DAE RID: 19886 RVA: 0x0016387D File Offset: 0x00161A7D
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=wxDbPiNH}Well, you have the reputation of being able to manage ruffians. Maybe you can take them off my hands, find some other lord who has more need of them and more denars to pay them. I've paid their contract for a few months. I can give you a small reward and if you can find a buyer, you can transfer the rest of the contract to him and pocket the down payment.[if:convo_innocent_smile]", null);
				}
			}

			// Token: 0x17000FFB RID: 4091
			// (get) Token: 0x06004DAF RID: 19887 RVA: 0x0016388A File Offset: 0x00161A8A
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=6bvJSIqh}Yes. I can find a new lord to take them on.", null);
				}
			}

			// Token: 0x17000FFC RID: 4092
			// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x00163897 File Offset: 0x00161A97
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=PV7RHgUl}Company of Trouble", null);
				}
			}

			// Token: 0x17000FFD RID: 4093
			// (get) Token: 0x06004DB1 RID: 19889 RVA: 0x001638A4 File Offset: 0x00161AA4
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=zw7a9eIt}{ISSUE_GIVER.NAME} wants you to take {?ISSUE_GIVER.GENDER}her{?}his{\\?} mercenaries and transfer them to another lord before they cause any trouble.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17000FFE RID: 4094
			// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x001638D8 File Offset: 0x00161AD8
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=I022Z9Ub}Heh. {QUEST_GIVER.NAME} got in deeper than {?QUEST_GIVER.GENDER}she{?}he{\\?} could handle with those mercenaries.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06004DB3 RID: 19891 RVA: 0x0016390A File Offset: 0x00161B0A
			public LandLordCompanyOfTroubleIssue(Hero issueOwner) : base(issueOwner, CampaignTime.DaysFromNow(25f))
			{
			}

			// Token: 0x06004DB4 RID: 19892 RVA: 0x0016391D File Offset: 0x00161B1D
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.ClanInfluence)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x06004DB5 RID: 19893 RVA: 0x00163932 File Offset: 0x00161B32
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06004DB6 RID: 19894 RVA: 0x00163934 File Offset: 0x00161B34
			protected override void HourlyTick()
			{
			}

			// Token: 0x06004DB7 RID: 19895 RVA: 0x00163936 File Offset: 0x00161B36
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest(questId, base.IssueOwner, CampaignTime.Never, this.CompanyTroopCount);
			}

			// Token: 0x06004DB8 RID: 19896 RVA: 0x0016394F File Offset: 0x00161B4F
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x06004DB9 RID: 19897 RVA: 0x00163954 File Offset: 0x00161B54
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill)
			{
				skill = null;
				relationHero = null;
				flag = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (Clan.PlayerClan.Tier < 1)
				{
					flag |= IssueBase.PreconditionFlags.ClanTier;
				}
				if (MobileParty.MainParty.MemberRoster.TotalManCount < this.CompanyTroopCount)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (MobileParty.MainParty.MemberRoster.TotalManCount + this.CompanyTroopCount > PartyBase.MainParty.PartySizeLimit)
				{
					flag |= IssueBase.PreconditionFlags.PartySizeLimit;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06004DBA RID: 19898 RVA: 0x00163A08 File Offset: 0x00161C08
			public override bool IssueStayAliveConditions()
			{
				return base.IssueOwner.Clan != Clan.PlayerClan;
			}

			// Token: 0x06004DBB RID: 19899 RVA: 0x00163A1F File Offset: 0x00161C1F
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}
		}

		// Token: 0x02000637 RID: 1591
		public class LandLordCompanyOfTroubleIssueQuest : QuestBase
		{
			// Token: 0x06004DBC RID: 19900 RVA: 0x00163A21 File Offset: 0x00161C21
			internal static void AutoGeneratedStaticCollectObjectsLandLordCompanyOfTroubleIssueQuest(object o, List<object> collectedObjects)
			{
				((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004DBD RID: 19901 RVA: 0x00163A2F File Offset: 0x00161C2F
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._companyOfTroubleParty);
				collectedObjects.Add(this._persuationTriedHeroesList);
			}

			// Token: 0x06004DBE RID: 19902 RVA: 0x00163A50 File Offset: 0x00161C50
			internal static object AutoGeneratedGetMemberValue_companyOfTroubleParty(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._companyOfTroubleParty;
			}

			// Token: 0x06004DBF RID: 19903 RVA: 0x00163A5D File Offset: 0x00161C5D
			internal static object AutoGeneratedGetMemberValue_battleWillStart(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._battleWillStart;
			}

			// Token: 0x06004DC0 RID: 19904 RVA: 0x00163A6F File Offset: 0x00161C6F
			internal static object AutoGeneratedGetMemberValue_triggerCompanyOfTroubleConversation(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._triggerCompanyOfTroubleConversation;
			}

			// Token: 0x06004DC1 RID: 19905 RVA: 0x00163A81 File Offset: 0x00161C81
			internal static object AutoGeneratedGetMemberValue_thieveryCount(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._thieveryCount;
			}

			// Token: 0x06004DC2 RID: 19906 RVA: 0x00163A93 File Offset: 0x00161C93
			internal static object AutoGeneratedGetMemberValue_demandGold(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._demandGold;
			}

			// Token: 0x06004DC3 RID: 19907 RVA: 0x00163AA5 File Offset: 0x00161CA5
			internal static object AutoGeneratedGetMemberValue_persuationTriedHeroesList(object o)
			{
				return ((LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest)o)._persuationTriedHeroesList;
			}

			// Token: 0x17000FFF RID: 4095
			// (get) Token: 0x06004DC4 RID: 19908 RVA: 0x00163AB2 File Offset: 0x00161CB2
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001000 RID: 4096
			// (get) Token: 0x06004DC5 RID: 19909 RVA: 0x00163AB5 File Offset: 0x00161CB5
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=PV7RHgUl}Company of Trouble", null);
				}
			}

			// Token: 0x17001001 RID: 4097
			// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x00163AC4 File Offset: 0x00161CC4
			private TextObject _playerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=8nS3QgD7}{QUEST_GIVER.LINK} is a {?QUEST_GIVER.GENDER}lady{?}lord{\\?} who told you that {?QUEST_GIVER.GENDER}she{?}he{\\?} wants to sell {?QUEST_GIVER.GENDER}her{?}his{\\?} mercenaries to another lord's service. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you sell them for {?QUEST_GIVER.GENDER}her{?}him{\\?} without causing any trouble.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001002 RID: 4098
			// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x00163AF8 File Offset: 0x00161CF8
			private TextObject _questSuccessPlayerSoldCompany
			{
				get
				{
					TextObject textObject = new TextObject("{=34MdCd6u}You have sold the mercenaries to another lord as you promised. {QUEST_GIVER.LINK} is grateful and sends {?QUEST_GIVER.GENDER}her{?}his{\\?} regards.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001003 RID: 4099
			// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x00163B2C File Offset: 0x00161D2C
			private TextObject _allCompanyDiedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=RrTAX7QE}You got the troublesome mercenaries killed off. You get no extra money for the contract, but you did get rid of them as you promised. {QUEST_GIVER.LINK} is grateful and sends {?QUEST_GIVER.GENDER}her{?}his{\\?} regards.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001004 RID: 4100
			// (get) Token: 0x06004DC9 RID: 19913 RVA: 0x00163B5E File Offset: 0x00161D5E
			private TextObject _playerDefeatedAgainstCompany
			{
				get
				{
					return new TextObject("{=7naLQmq1}You have lost the battle against the mercenaries. You have failed to get rid of them as you promised. Now they've turned bandit and are starting to plunder the countryside", null);
				}
			}

			// Token: 0x17001005 RID: 4101
			// (get) Token: 0x06004DCA RID: 19914 RVA: 0x00163B6B File Offset: 0x00161D6B
			private TextObject _questFailCompanyLeft
			{
				get
				{
					return new TextObject("{=k9SksaXg}The mercenaries left your party, as you failed to get rid of them as you promised. Now the mercenaries have turned bandit and start to plunder countryside.", null);
				}
			}

			// Token: 0x17001006 RID: 4102
			// (get) Token: 0x06004DCB RID: 19915 RVA: 0x00163B78 File Offset: 0x00161D78
			private TextObject _questCanceledWarDeclared
			{
				get
				{
					TextObject textObject = new TextObject("{=ItueKmqd}Your clan is now at war with the {QUEST_GIVER_SETTLEMENT_FACTION}. You contract with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_SETTLEMENT_FACTION", base.QuestGiver.MapFaction.InformalName);
					return textObject;
				}
			}

			// Token: 0x17001007 RID: 4103
			// (get) Token: 0x06004DCC RID: 19916 RVA: 0x00163BC8 File Offset: 0x00161DC8
			private TextObject _playerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06004DCD RID: 19917 RVA: 0x00163BFC File Offset: 0x00161DFC
			public LandLordCompanyOfTroubleIssueQuest(string questId, Hero questGiver, CampaignTime duration, int companyTroopCount) : base(questId, questGiver, duration, 500)
			{
				this._troubleCharacterObject = MBObjectManager.Instance.GetObject<CharacterObject>("company_of_trouble_character");
				this._persuationTriedHeroesList = new List<Hero>();
				this._troubleCharacterObject.SetTransferableInPartyScreen(false);
				this._troubleCharacterObject.SetTransferableInHideouts(false);
				this._companyTroopCount = companyTroopCount;
				this._tasks = new PersuasionTask[3];
				this._battleWillStart = false;
				this._thieveryCount = 0;
				this._demandGold = 0;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06004DCE RID: 19918 RVA: 0x00163C84 File Offset: 0x00161E84
			protected override void InitializeQuestOnGameLoad()
			{
				this._troubleCharacterObject = MBObjectManager.Instance.GetObject<CharacterObject>("company_of_trouble_character");
				this._troubleCharacterObject.SetTransferableInPartyScreen(false);
				this._troubleCharacterObject.SetTransferableInHideouts(false);
				this._tasks = new PersuasionTask[3];
				this.UpdateCompanyTroopCount();
				this.SetDialogs();
			}

			// Token: 0x06004DCF RID: 19919 RVA: 0x00163CD8 File Offset: 0x00161ED8
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=T6d7wtJX}Very well. I'll tell them to join your party. Good luck.[if:convo_mocking_aristocratic][ib:hip]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=bWpLYiEg}Did you ever find a way to handle those mercenaries?[if:convo_astonished]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
				}).BeginPlayerOptions().PlayerOption(new TextObject("{=XzK4niIb}I'll find an employer soon.", null), null).NpcLine(new TextObject("{=rOBRabQz}Good. I'm waiting for your good news.[if:convo_mocking_aristocratic]", null), null, null).CloseDialog().PlayerOption(new TextObject("{=Zb3EdxDT}That kind of lord is hard to find.", null), null).NpcLine(new TextObject("{=yOfrb9Lu}Don't wait too long. These are dangerous men. Be careful.[if:convo_nonchalant]", null), null, null).CloseDialog().EndPlayerOptions().CloseDialog();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCompanyDialogFlow(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetOtherLordsDialogFlow(), this);
			}

			// Token: 0x06004DD0 RID: 19920 RVA: 0x00163E14 File Offset: 0x00162014
			private DialogFlow GetOtherLordsDialogFlow()
			{
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("hero_main_options", 700).BeginPlayerOptions().PlayerOption(new TextObject("{=2E7s4L9R}Do you need mercenaries? I have a contract that I can transfer to you for {DEMAND_GOLD} denars.", null), null).Condition(new ConversationSentence.OnConditionDelegate(this.PersuasionDialogForLordGeneralCondition)).BeginNpcOptions().NpcOption(new TextObject("{=ZR4RJdYS}Hmm, that sounds interesting...[if:convo_thinking]", null), new ConversationSentence.OnConditionDelegate(this.PersuasionDialogSpecialCondition), null, null).GotoDialogState("company_of_trouble_persuasion").NpcOption(new TextObject("{=pmrjUNEz}As it happens, I already have a mercenary contract that I wish to sell. So, no thank you.[if:convo_calm_friendly]", null), new ConversationSentence.OnConditionDelegate(this.HasSameIssue), null, null).GotoDialogState("hero_main_options").NpcOption(new TextObject("{=bw0hEPN6}You already bought their contract from our clan. Why would I want to buy them back?[if:convo_confused_normal]", null), new ConversationSentence.OnConditionDelegate(this.IsSameClanMember), null, null).GotoDialogState("hero_main_options").NpcOption(new TextObject("{=64bH4bUo}No, thank you. But perhaps one of the other lords of our clan would be interested.[if:convo_undecided_closed]", null), () => !this.HasMobileParty(), null, null).GotoDialogState("hero_main_options").NpcOption(new TextObject("{=Zs6L1aBL}I'm sorry. I don't need mercenaries right now.[if:convo_normal]", null), null, null, null).GotoDialogState("hero_main_options").EndNpcOptions().EndPlayerOptions();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x06004DD1 RID: 19921 RVA: 0x00163F2C File Offset: 0x0016212C
			private bool PersuasionDialogSpecialCondition()
			{
				return !this.IsSameClanMember() && !this.HasSameIssue() && this.HasMobileParty() && !this.InSameSettlement();
			}

			// Token: 0x06004DD2 RID: 19922 RVA: 0x00163F51 File Offset: 0x00162151
			private bool HasMobileParty()
			{
				return Hero.OneToOneConversationHero.PartyBelongedTo != null;
			}

			// Token: 0x06004DD3 RID: 19923 RVA: 0x00163F60 File Offset: 0x00162160
			private bool IsSameClanMember()
			{
				return Hero.OneToOneConversationHero.Clan == base.QuestGiver.Clan;
			}

			// Token: 0x06004DD4 RID: 19924 RVA: 0x00163F79 File Offset: 0x00162179
			private bool InSameSettlement()
			{
				return Hero.OneToOneConversationHero.CurrentSettlement != null && base.QuestGiver.CurrentSettlement != null && Hero.OneToOneConversationHero.CurrentSettlement == base.QuestGiver.CurrentSettlement;
			}

			// Token: 0x06004DD5 RID: 19925 RVA: 0x00163FAD File Offset: 0x001621AD
			private bool HasSameIssue()
			{
				IssueBase issue = Hero.OneToOneConversationHero.Issue;
				return ((issue != null) ? issue.GetType() : null) == base.GetType();
			}

			// Token: 0x06004DD6 RID: 19926 RVA: 0x00163FD0 File Offset: 0x001621D0
			private bool PersuasionDialogForLordGeneralCondition()
			{
				if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.IsLord && Hero.OneToOneConversationHero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && Hero.OneToOneConversationHero != base.QuestGiver && !Hero.OneToOneConversationHero.MapFaction.IsAtWarWith(base.QuestGiver.MapFaction) && Hero.OneToOneConversationHero.Clan != Clan.PlayerClan && !this._persuationTriedHeroesList.Contains(Hero.OneToOneConversationHero))
				{
					this.UpdateCompanyTroopCount();
					this._demandGold = 1000 + this._companyTroopCount * 150;
					MBTextManager.SetTextVariable("DEMAND_GOLD", this._demandGold);
					this._tasks[0] = this.GetPersuasionTask1();
					this._tasks[1] = this.GetPersuasionTask2();
					this._tasks[2] = this.GetPersuasionTask3();
					this._selectedTask = this._tasks.GetRandomElement<PersuasionTask>();
					return true;
				}
				return false;
			}

			// Token: 0x06004DD7 RID: 19927 RVA: 0x001640D0 File Offset: 0x001622D0
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				dialog.AddDialogLine("company_of_trouble_persuasion_check_accepted", "company_of_trouble_persuasion", "company_of_trouble_persuasion_start_reservation", "{=GCH6RgIQ}How tough are they?", new ConversationSentence.OnConditionDelegate(this.persuasion_start_with_company_of_trouble_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_start_with_company_of_trouble_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_rejected", "company_of_trouble_persuasion_start_reservation", "hero_main_options", "{=!}{FAILED_PERSUASION_LINE}", new ConversationSentence.OnConditionDelegate(this.persuasion_failed_with_company_of_trouble_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_rejected_with_company_of_trouble_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_attempt", "company_of_trouble_persuasion_start_reservation", "company_of_trouble_persuasion_select_option", "{=K0Qtl5RZ}Tell me about the details...", () => !this.persuasion_failed_with_company_of_trouble_on_condition(), null, this, 100, null, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_success", "company_of_trouble_persuasion_start_reservation", "close_window", "{=QlECaaHt}Hmm...They can be useful.", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.persuasion_complete_with_company_of_trouble_on_consequence), this, 200, null, null, null);
				string id = "company_of_trouble_persuasion_select_option_1";
				string inputToken = "company_of_trouble_persuasion_select_option";
				string outputToken = "company_of_trouble_persuasion_selected_option_response";
				string text = "{=0AUZvSAq}{COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate conditionDelegate = new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.company_of_trouble_persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.company_of_trouble_persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(id, inputToken, outputToken, text, conditionDelegate, consequenceDelegate, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				string id2 = "company_of_trouble_persuasion_select_option_2";
				string inputToken2 = "company_of_trouble_persuasion_select_option";
				string outputToken2 = "company_of_trouble_persuasion_selected_option_response";
				string text2 = "{=GG1W8qGd}{COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate conditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_select_option_2_on_consequence);
				persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.company_of_trouble_persuasion_setup_option_2);
				clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.company_of_trouble_persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(id2, inputToken2, outputToken2, text2, conditionDelegate2, consequenceDelegate2, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				string id3 = "company_of_trouble_persuasion_select_option_3";
				string inputToken3 = "company_of_trouble_persuasion_select_option";
				string outputToken3 = "company_of_trouble_persuasion_selected_option_response";
				string text3 = "{=kFs940kp}{COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate conditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_select_option_3_on_consequence);
				persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.company_of_trouble_persuasion_setup_option_3);
				clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.company_of_trouble_persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(id3, inputToken3, outputToken3, text3, conditionDelegate3, consequenceDelegate3, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				dialog.AddDialogLine("company_of_trouble_persuasion_select_option_reaction", "company_of_trouble_persuasion_selected_option_response", "company_of_trouble_persuasion_start_reservation", "{=D0xDRqvm}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.company_of_trouble_persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.company_of_trouble_persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x06004DD8 RID: 19928 RVA: 0x001642EE File Offset: 0x001624EE
			private void persuasion_start_with_company_of_trouble_on_consequence()
			{
				this._persuationTriedHeroesList.Add(Hero.OneToOneConversationHero);
				ConversationManager.StartPersuasion(2f, 1f, 0f, 2f, 2f, 0f, PersuasionDifficulty.Hard);
			}

			// Token: 0x06004DD9 RID: 19929 RVA: 0x00164324 File Offset: 0x00162524
			private bool persuasion_start_with_company_of_trouble_on_condition()
			{
				return !this._persuationTriedHeroesList.Contains(Hero.OneToOneConversationHero);
			}

			// Token: 0x06004DDA RID: 19930 RVA: 0x0016433C File Offset: 0x0016253C
			private PersuasionTask GetPersuasionTask1()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=1V9GeKr8}Fah...I don't need more men. Thank you.", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=EvAubSxs}What kind of troops do they make?", null);
				PersuasionOptionArgs option = new PersuasionOptionArgs(DefaultSkills.Trade, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Easy, false, new TextObject("{=sqMUtasn}Cheap, disposable and effective. What you say?", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option);
				PersuasionOptionArgs option2 = new PersuasionOptionArgs(DefaultSkills.Tactics, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=Pcgqs9aX}Here's a quick run down of their training...", null), null, true, false, false);
				persuasionTask.AddOptionToTask(option2);
				PersuasionOptionArgs option3 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Valor, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=WvQDatMJ}I won't kid you, they're mean bastards, but that's good if you can manage them.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option3);
				return persuasionTask;
			}

			// Token: 0x06004DDB RID: 19931 RVA: 0x00164400 File Offset: 0x00162600
			private PersuasionTask GetPersuasionTask2()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=UP0pMGDR}There are enough bandits around here already. I don't need more on retainer.", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=zR356YDY}I have to say, they seem more like bandits than soldiers.", null);
				PersuasionOptionArgs option = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Valor, TraitEffect.Positive, PersuasionArgumentStrength.Easy, false, new TextObject("{=JI6Q9pQ7}Bandits can kill as well as any other kind of troops, if used correctly.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option);
				PersuasionOptionArgs option2 = new PersuasionOptionArgs(DefaultSkills.Trade, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=SqceZdzH}Of course. That's why they're cheap. You get what you pay for. ", null), null, true, false, false);
				persuasionTask.AddOptionToTask(option2);
				PersuasionOptionArgs option3 = new PersuasionOptionArgs(DefaultSkills.Scouting, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=NWLH02KL}Bandits are good in the wilderness, having been both predator and prey.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option3);
				return persuasionTask;
			}

			// Token: 0x06004DDC RID: 19932 RVA: 0x001644C4 File Offset: 0x001626C4
			private PersuasionTask GetPersuasionTask3()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=97pacK2l}Fah... I don't need more men. Thank you.", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=A2ju7YTZ}I don't know... They look treacherous.", null);
				PersuasionOptionArgs option = new PersuasionOptionArgs(DefaultSkills.Tactics, DefaultTraits.Mercy, TraitEffect.Negative, PersuasionArgumentStrength.Easy, false, new TextObject("{=z1mdQhDB}Of course. Send them in ahead of your other troops. If they die, you don't need to pay them.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option);
				PersuasionOptionArgs option2 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=jWavM9AD}You've been around in the world. You know that mercenaries aren't saints.", null), null, true, false, false);
				persuasionTask.AddOptionToTask(option2);
				PersuasionOptionArgs option3 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Generosity, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=sLjGguGy}Sure, they're bastards. But they'll be loyal bastards if you treat them well.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option3);
				return persuasionTask;
			}

			// Token: 0x06004DDD RID: 19933 RVA: 0x00164588 File Offset: 0x00162788
			private bool company_of_trouble_persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				if (item == PersuasionOptionResult.CriticalFailure)
				{
					this._selectedTask.BlockAllOptions();
				}
				return true;
			}

			// Token: 0x06004DDE RID: 19934 RVA: 0x001645C8 File Offset: 0x001627C8
			private void company_of_trouble_persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.Hard);
				float moveToNextStageChance;
				float blockRandomOptionChance;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out moveToNextStageChance, out blockRandomOptionChance, difficulty);
				this._selectedTask.ApplyEffects(moveToNextStageChance, blockRandomOptionChance);
			}

			// Token: 0x06004DDF RID: 19935 RVA: 0x00164624 File Offset: 0x00162824
			private bool company_of_trouble_persuasion_select_option_1_on_condition()
			{
				if (this._selectedTask.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._selectedTask.Options.ElementAt(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._selectedTask.Options.ElementAt(0).Line);
					MBTextManager.SetTextVariable("COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06004DE0 RID: 19936 RVA: 0x001646A4 File Offset: 0x001628A4
			private bool company_of_trouble_persuasion_select_option_2_on_condition()
			{
				if (this._selectedTask.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._selectedTask.Options.ElementAt(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._selectedTask.Options.ElementAt(1).Line);
					MBTextManager.SetTextVariable("COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06004DE1 RID: 19937 RVA: 0x00164724 File Offset: 0x00162924
			private bool company_of_trouble_persuasion_select_option_3_on_condition()
			{
				if (this._selectedTask.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._selectedTask.Options.ElementAt(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._selectedTask.Options.ElementAt(2).Line);
					MBTextManager.SetTextVariable("COMPANY_OF_TROUBLE_PERSUADE_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06004DE2 RID: 19938 RVA: 0x001647A4 File Offset: 0x001629A4
			private void company_of_trouble_persuasion_select_option_1_on_consequence()
			{
				if (this._selectedTask.Options.Count > 0)
				{
					this._selectedTask.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x06004DE3 RID: 19939 RVA: 0x001647D0 File Offset: 0x001629D0
			private void company_of_trouble_persuasion_select_option_2_on_consequence()
			{
				if (this._selectedTask.Options.Count > 1)
				{
					this._selectedTask.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x06004DE4 RID: 19940 RVA: 0x001647FC File Offset: 0x001629FC
			private void company_of_trouble_persuasion_select_option_3_on_consequence()
			{
				if (this._selectedTask.Options.Count > 2)
				{
					this._selectedTask.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x06004DE5 RID: 19941 RVA: 0x00164828 File Offset: 0x00162A28
			private bool persuasion_failed_with_company_of_trouble_on_condition()
			{
				if (this._selectedTask.Options.All((PersuasionOptionArgs x) => x.IsBlocked) && !ConversationManager.GetPersuasionProgressSatisfied())
				{
					MBTextManager.SetTextVariable("FAILED_PERSUASION_LINE", this._selectedTask.FinalFailLine, false);
					return true;
				}
				return false;
			}

			// Token: 0x06004DE6 RID: 19942 RVA: 0x00164886 File Offset: 0x00162A86
			private PersuasionOptionArgs company_of_trouble_persuasion_setup_option_1()
			{
				return this._selectedTask.Options.ElementAt(0);
			}

			// Token: 0x06004DE7 RID: 19943 RVA: 0x00164899 File Offset: 0x00162A99
			private PersuasionOptionArgs company_of_trouble_persuasion_setup_option_2()
			{
				return this._selectedTask.Options.ElementAt(1);
			}

			// Token: 0x06004DE8 RID: 19944 RVA: 0x001648AC File Offset: 0x00162AAC
			private PersuasionOptionArgs company_of_trouble_persuasion_setup_option_3()
			{
				return this._selectedTask.Options.ElementAt(2);
			}

			// Token: 0x06004DE9 RID: 19945 RVA: 0x001648C0 File Offset: 0x00162AC0
			private bool company_of_trouble_persuasion_clickable_option_1_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._selectedTask.Options.Count > 0)
				{
					hintText = (this._selectedTask.Options.ElementAt(0).IsBlocked ? hintText : TextObject.Empty);
					return !this._selectedTask.Options.ElementAt(0).IsBlocked;
				}
				return false;
			}

			// Token: 0x06004DEA RID: 19946 RVA: 0x0016492C File Offset: 0x00162B2C
			private bool company_of_trouble_persuasion_clickable_option_2_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._selectedTask.Options.Count > 1)
				{
					hintText = (this._selectedTask.Options.ElementAt(1).IsBlocked ? hintText : TextObject.Empty);
					return !this._selectedTask.Options.ElementAt(1).IsBlocked;
				}
				return false;
			}

			// Token: 0x06004DEB RID: 19947 RVA: 0x00164998 File Offset: 0x00162B98
			private bool company_of_trouble_persuasion_clickable_option_3_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._selectedTask.Options.Count > 2)
				{
					hintText = (this._selectedTask.Options.ElementAt(2).IsBlocked ? hintText : TextObject.Empty);
					return !this._selectedTask.Options.ElementAt(2).IsBlocked;
				}
				return false;
			}

			// Token: 0x06004DEC RID: 19948 RVA: 0x00164A03 File Offset: 0x00162C03
			private void persuasion_rejected_with_company_of_trouble_on_consequence()
			{
				if (PlayerEncounter.Current != null)
				{
					PlayerEncounter.LeaveEncounter = true;
				}
				ConversationManager.EndPersuasion();
			}

			// Token: 0x06004DED RID: 19949 RVA: 0x00164A18 File Offset: 0x00162C18
			private void persuasion_complete_with_company_of_trouble_on_consequence()
			{
				if (PlayerEncounter.Current != null)
				{
					PlayerEncounter.LeaveEncounter = true;
				}
				ConversationManager.EndPersuasion();
				this.UpdateCompanyTroopCount();
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._demandGold, false);
				this.RelationshipChangeWithQuestGiver = 5;
				base.AddLog(this._questSuccessPlayerSoldCompany, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06004DEE RID: 19950 RVA: 0x00164A8C File Offset: 0x00162C8C
			private DialogFlow GetCompanyDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=8TCev3Qs}So, captain. We expect a bit of looting and plundering as compensation, in addition to the wages. You don't seem like you're going to provide it to us. So, farewell.[if:innocent_smile][ib:hip]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.CompanyDialogFromCondition)).BeginPlayerOptions().PlayerOption(new TextObject("{=1aaoSpNf}Your contract with the {QUEST_GIVER.NAME} is still in force. I can't let you go without {?QUEST_GIVER.GENDER}her{?}his{\\?} permission.", null), null).NpcLine(new TextObject("{=oI5H6Xo8}Don't think we won't fight you if you try and stop us.[if:convo_mocking_aristocratic]", null), null, null).BeginPlayerOptions().PlayerOption(new TextObject("{=hIFazIcK}So be it!", null), null).NpcLine(new TextObject("{=KKeRi477}All right, lads. Let's kill the boss.[if:convo_predatory][ib:aggressive]", null), null, null).Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += this.CreateCompanyEnemyParty;
				}).CloseDialog().PlayerOption(new TextObject("{=bm7UcuQj}No! There is no need to fight. I don't want any bloodshed... Just leave.", null), null).NpcLine(new TextObject("{=1vnaskLR}It was a pleasure to work with you, chief. Farewell...[if:convo_nonchalant][ib:normal2]", null), null, null).Consequence(delegate
				{
					this._companyLeftQuestWillFail = true;
				}).CloseDialog().EndPlayerOptions().CloseDialog().PlayerOption(new TextObject("{=hj4vfgxk}As you wish! Good luck. ", null), null).NpcLine(new TextObject("{=1vnaskLR}It was a pleasure to work with you, chief. Farewell...[if:convo_nonchalant][ib:normal2]", null), null, null).Consequence(delegate
				{
					this._companyLeftQuestWillFail = true;
				}).CloseDialog().EndPlayerOptions();
			}

			// Token: 0x06004DEF RID: 19951 RVA: 0x00164BAF File Offset: 0x00162DAF
			private bool CompanyDialogFromCondition()
			{
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
				return this._troubleCharacterObject == CharacterObject.OneToOneConversationCharacter;
			}

			// Token: 0x06004DF0 RID: 19952 RVA: 0x00164BD8 File Offset: 0x00162DD8
			private void CreateCompanyEnemyParty()
			{
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				Settlement settlement = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsHideout);
				this._companyOfTroubleParty = BanditPartyComponent.CreateBanditParty("company_of_trouble_" + base.StringId, settlement.OwnerClan, settlement.Hideout, false);
				TextObject customName = new TextObject("{=PV7RHgUl}Company of Trouble", null);
				this._companyOfTroubleParty.InitializeMobilePartyAtPosition(new TroopRoster(this._companyOfTroubleParty.Party), new TroopRoster(this._companyOfTroubleParty.Party), MobileParty.MainParty.Position2D);
				this._companyOfTroubleParty.SetCustomName(customName);
				this._companyOfTroubleParty.SetPartyUsedByQuest(true);
				this._companyOfTroubleParty.MemberRoster.AddToCounts(this._troubleCharacterObject, this._companyTroopCount, false, 0, 0, true, -1);
				this._battleWillStart = true;
			}

			// Token: 0x06004DF1 RID: 19953 RVA: 0x00164CD8 File Offset: 0x00162ED8
			internal void CompanyLeftQuestFail()
			{
				this.UpdateCompanyTroopCount();
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				base.AddLog(this._questFailCompanyLeft, false);
				base.CompleteQuestWithFail(null);
				this._companyLeftQuestWillFail = false;
				GameMenu.ExitToLast();
			}

			// Token: 0x06004DF2 RID: 19954 RVA: 0x00164D30 File Offset: 0x00162F30
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this._playerStartsQuestLogText, false);
				MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, this._companyTroopCount, false, 0, 0, true, -1);
				MBInformationManager.AddQuickInformation(new TextObject("{=jGIxKb99}Mercenaries have joined your party.", null), 0, null, "");
			}

			// Token: 0x06004DF3 RID: 19955 RVA: 0x00164D8C File Offset: 0x00162F8C
			protected override void RegisterEvents()
			{
				CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			}

			// Token: 0x06004DF4 RID: 19956 RVA: 0x00164E0C File Offset: 0x0016300C
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06004DF5 RID: 19957 RVA: 0x00164E1F File Offset: 0x0016301F
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this._questCanceledWarDeclared);
				}
			}

			// Token: 0x06004DF6 RID: 19958 RVA: 0x00164E49 File Offset: 0x00163049
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this._playerDeclaredWarQuestLogText, this._questCanceledWarDeclared, false);
			}

			// Token: 0x06004DF7 RID: 19959 RVA: 0x00164E64 File Offset: 0x00163064
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if ((mapEvent.IsPlayerMapEvent || mapEvent.IsPlayerSimulation) && !this._checkForBattleResults)
				{
					this.UpdateCompanyTroopCount();
					if (this._companyTroopCount == 0)
					{
						base.AddLog(this._allCompanyDiedLogText, false);
						this.RelationshipChangeWithQuestGiver = 5;
						GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
						base.CompleteQuestWithSuccess();
					}
				}
			}

			// Token: 0x06004DF8 RID: 19960 RVA: 0x00164EC4 File Offset: 0x001630C4
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					this.UpdateCompanyTroopCount();
					if (MobileParty.MainParty.MemberRoster.TotalManCount - this._companyTroopCount <= this._companyTroopCount && MapEvent.PlayerMapEvent == null && Settlement.CurrentSettlement == null && PlayerEncounter.Current == null && !Hero.MainHero.IsWounded)
					{
						this._triggerCompanyOfTroubleConversation = true;
						GameMenu.ActivateGameMenu("company_of_trouble_menu");
					}
				}
			}

			// Token: 0x06004DF9 RID: 19961 RVA: 0x00164F30 File Offset: 0x00163130
			private void TryToStealItemFromPlayer()
			{
				bool flag = false;
				for (int i = 0; i < MobileParty.MainParty.ItemRoster.Count; i++)
				{
					ItemRosterElement itemRosterElement = MobileParty.MainParty.ItemRoster[i];
					ItemObject item = itemRosterElement.EquipmentElement.Item;
					if (!itemRosterElement.IsEmpty && item.IsFood)
					{
						MobileParty.MainParty.ItemRoster.AddToCounts(item, -1);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (this._thieveryCount == 0 || this._thieveryCount == 1)
					{
						InformationManager.ShowInquiry(new InquiryData(this.Title.ToString(), (this._thieveryCount == 0) ? new TextObject("{=OKpwA8Az}Your men have noticed some of the goods in the baggage train are missing.", null).ToString() : new TextObject("{=acu1wTeq}Your men are sure of that some of the goods were stolen from the baggage train.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), true, false);
					}
					else
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=xlm8oYhM}Your men reported that some of the goods were stolen from the baggage train.", null), 0, null, "");
					}
					this._thieveryCount++;
				}
			}

			// Token: 0x06004DFA RID: 19962 RVA: 0x00165047 File Offset: 0x00163247
			public void DailyTick()
			{
				if (MBRandom.RandomFloat > 0.5f)
				{
					this.TryToStealItemFromPlayer();
				}
			}

			// Token: 0x06004DFB RID: 19963 RVA: 0x0016505C File Offset: 0x0016325C
			private void UpdateCompanyTroopCount()
			{
				bool flag = false;
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == this._troubleCharacterObject)
					{
						flag = true;
						this._companyTroopCount = troopRosterElement.Number;
						break;
					}
				}
				if (!flag)
				{
					this._companyTroopCount = 0;
				}
			}

			// Token: 0x06004DFC RID: 19964 RVA: 0x001650DC File Offset: 0x001632DC
			internal void QuestSuccessWithPlayerDefeatedCompany()
			{
				base.AddLog(this._allCompanyDiedLogText, false);
				this.RelationshipChangeWithQuestGiver = 5;
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06004DFD RID: 19965 RVA: 0x0016510B File Offset: 0x0016330B
			internal void QuestFailWithPlayerDefeatedAgainstCompany()
			{
				base.AddLog(this._playerDefeatedAgainstCompany, false);
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06004DFE RID: 19966 RVA: 0x00165122 File Offset: 0x00163322
			protected override void OnFinalize()
			{
				this.UpdateCompanyTroopCount();
				if (this._companyTroopCount > 0)
				{
					MobileParty.MainParty.MemberRoster.AddToCounts(this._troubleCharacterObject, -this._companyTroopCount, false, 0, 0, true, -1);
				}
			}

			// Token: 0x04001A03 RID: 6659
			private const string TroubleCharacterObjectStringId = "company_of_trouble_character";

			// Token: 0x04001A04 RID: 6660
			private int _companyTroopCount;

			// Token: 0x04001A05 RID: 6661
			[SaveableField(20)]
			internal MobileParty _companyOfTroubleParty;

			// Token: 0x04001A06 RID: 6662
			[SaveableField(30)]
			internal bool _battleWillStart;

			// Token: 0x04001A07 RID: 6663
			internal bool _checkForBattleResults;

			// Token: 0x04001A08 RID: 6664
			[SaveableField(40)]
			private int _thieveryCount;

			// Token: 0x04001A09 RID: 6665
			[SaveableField(80)]
			internal bool _triggerCompanyOfTroubleConversation;

			// Token: 0x04001A0A RID: 6666
			[SaveableField(50)]
			private int _demandGold;

			// Token: 0x04001A0B RID: 6667
			internal CharacterObject _troubleCharacterObject;

			// Token: 0x04001A0C RID: 6668
			private PersuasionTask[] _tasks;

			// Token: 0x04001A0D RID: 6669
			private PersuasionTask _selectedTask;

			// Token: 0x04001A0E RID: 6670
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.Hard;

			// Token: 0x04001A0F RID: 6671
			[SaveableField(70)]
			private List<Hero> _persuationTriedHeroesList;

			// Token: 0x04001A10 RID: 6672
			internal bool _companyLeftQuestWillFail;
		}

		// Token: 0x02000638 RID: 1592
		public class LandLordCompanyOfTroubleIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06004E06 RID: 19974 RVA: 0x001651B8 File Offset: 0x001633B8
			public LandLordCompanyOfTroubleIssueTypeDefiner() : base(4800000)
			{
			}

			// Token: 0x06004E07 RID: 19975 RVA: 0x001651C5 File Offset: 0x001633C5
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssue), 1, null);
				base.AddClassDefinition(typeof(LandLordCompanyOfTroubleIssueBehavior.LandLordCompanyOfTroubleIssueQuest), 2, null);
			}
		}
	}
}
