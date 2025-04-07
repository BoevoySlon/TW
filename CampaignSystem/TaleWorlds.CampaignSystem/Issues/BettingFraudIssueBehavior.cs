using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x020002FF RID: 767
	public class BettingFraudIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06002CDE RID: 11486 RVA: 0x000BC238 File Offset: 0x000BA438
		private static BettingFraudIssueBehavior.BettingFraudQuest Instance
		{
			get
			{
				BettingFraudIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<BettingFraudIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BettingFraudIssueBehavior.BettingFraudQuest cachedQuest;
						if ((cachedQuest = (enumerator.Current as BettingFraudIssueBehavior.BettingFraudQuest)) != null)
						{
							campaignBehavior._cachedQuest = cachedQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000BC2D0 File Offset: 0x000BA4D0
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.CheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000BC300 File Offset: 0x000BA500
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("menu_town_tournament_join_betting_fraud", "{=5Adr6toM}{MENU_TEXT}", new OnInitDelegate(this.game_menu_tournament_join_on_init), GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("menu_town_tournament_join_betting_fraud", "mno_tournament_event_1", "{=es0Y3Bxc}Join", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Mission;
				args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
				return true;
			}, new GameMenuOption.OnConsequenceDelegate(this.game_menu_tournament_join_current_game_on_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("menu_town_tournament_join_betting_fraud", "mno_tournament_leave", "{=3sRdGQou}Leave", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("town_arena");
			}, true, -1, false, null);
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000BC3C8 File Offset: 0x000BA5C8
		private void game_menu_tournament_join_on_init(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			tournamentGame.UpdateTournamentPrize(true, false);
			GameTexts.SetVariable("MENU_TEXT", tournamentGame.GetMenuText());
		}

		// Token: 0x06002CE2 RID: 11490 RVA: 0x000BC408 File Offset: 0x000BA608
		private void game_menu_tournament_join_current_game_on_consequence(MenuCallbackArgs args)
		{
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(BettingFraudIssueBehavior.Instance._thug, null, false, false, false, false, false, false));
		}

		// Token: 0x06002CE3 RID: 11491 RVA: 0x000BC444 File Offset: 0x000BA644
		[GameMenuInitializationHandler("menu_town_tournament_join_betting_fraud")]
		private static void game_menu_ui_town_ui_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Town.WaitMeshName);
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000BC46D File Offset: 0x000BA66D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000BC470 File Offset: 0x000BA670
		private void CheckForIssue(Hero hero)
		{
			if (this.ConditionsHold(hero))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(BettingFraudIssueBehavior.BettingFraudIssue), IssueBase.IssueFrequency.Rare, null));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(BettingFraudIssueBehavior.BettingFraudIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x06002CE6 RID: 11494 RVA: 0x000BC4D4 File Offset: 0x000BA6D4
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsGangLeader && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.Town != null && issueGiver.CurrentSettlement.Town.Security < 50f;
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000BC50C File Offset: 0x000BA70C
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			return new BettingFraudIssueBehavior.BettingFraudIssue(issueOwner);
		}

		// Token: 0x04000D74 RID: 3444
		private const IssueBase.IssueFrequency BettingFraudIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x04000D75 RID: 3445
		private const string JoinTournamentMenuId = "menu_town_tournament_join";

		// Token: 0x04000D76 RID: 3446
		private const string JoinTournamentForBettingFraudQuestMenuId = "menu_town_tournament_join_betting_fraud";

		// Token: 0x04000D77 RID: 3447
		private const int SettlementSecurityLimit = 50;

		// Token: 0x04000D78 RID: 3448
		private BettingFraudIssueBehavior.BettingFraudQuest _cachedQuest;

		// Token: 0x02000609 RID: 1545
		public class BettingFraudIssue : IssueBase
		{
			// Token: 0x06004920 RID: 18720 RVA: 0x00152818 File Offset: 0x00150A18
			internal static void AutoGeneratedStaticCollectObjectsBettingFraudIssue(object o, List<object> collectedObjects)
			{
				((BettingFraudIssueBehavior.BettingFraudIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004921 RID: 18721 RVA: 0x00152826 File Offset: 0x00150A26
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x17000E78 RID: 3704
			// (get) Token: 0x06004922 RID: 18722 RVA: 0x0015282F File Offset: 0x00150A2F
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=kru5Vpog}Yes. I'm glad to have the chance to talk to you. I keep an eye on the careers of champions like yourself for professional reasons, and I have a proposal that might interest a good fighter like you. Interested?[ib:confident3][if:convo_bemused]", null);
				}
			}

			// Token: 0x17000E79 RID: 3705
			// (get) Token: 0x06004923 RID: 18723 RVA: 0x0015283C File Offset: 0x00150A3C
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=YWXkgDSd}What kind of a partnership are we talking about?", null);
				}
			}

			// Token: 0x17000E7A RID: 3706
			// (get) Token: 0x06004924 RID: 18724 RVA: 0x00152849 File Offset: 0x00150A49
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=vLaoZhkF}I follow tournaments, you see, and like to both place and take bets. But of course I need someone who can not only win those tournaments but lose if necessary... if you understand what I mean. Not all the time. That would be too obvious. Here's what I propose. We enter into a partnership for five tournaments. Don't bother memorizing which ones you win and which ones you lose. Before each fight, an associate of my mine will let you know how you should place. Follow my instructions and I promise you will be rewarded handsomely. What do you say?[if:convo_bemused][ib:demure2]", null);
				}
			}

			// Token: 0x17000E7B RID: 3707
			// (get) Token: 0x06004925 RID: 18725 RVA: 0x00152856 File Offset: 0x00150A56
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=cL9BX7ph}As long as the payment is good, I agree.", null);
				}
			}

			// Token: 0x17000E7C RID: 3708
			// (get) Token: 0x06004926 RID: 18726 RVA: 0x00152863 File Offset: 0x00150A63
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000E7D RID: 3709
			// (get) Token: 0x06004927 RID: 18727 RVA: 0x00152866 File Offset: 0x00150A66
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000E7E RID: 3710
			// (get) Token: 0x06004928 RID: 18728 RVA: 0x00152869 File Offset: 0x00150A69
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=xhVrxgC4}Betting Fraud", null);
				}
			}

			// Token: 0x17000E7F RID: 3711
			// (get) Token: 0x06004929 RID: 18729 RVA: 0x00152876 File Offset: 0x00150A76
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=3j8pV58L}{ISSUE_GIVER.NAME} offers you a deal to fix {TOURNAMENT_COUNT} tournaments and share the profit from the bet winnings.", null);
					textObject.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, false);
					textObject.SetTextVariable("TOURNAMENT_COUNT", 5);
					return textObject;
				}
			}

			// Token: 0x0600492A RID: 18730 RVA: 0x001528A7 File Offset: 0x00150AA7
			public BettingFraudIssue(Hero issueOwner) : base(issueOwner, CampaignTime.DaysFromNow(45f))
			{
			}

			// Token: 0x0600492B RID: 18731 RVA: 0x001528BA File Offset: 0x00150ABA
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x0600492C RID: 18732 RVA: 0x001528CF File Offset: 0x00150ACF
			protected override void OnGameLoad()
			{
			}

			// Token: 0x0600492D RID: 18733 RVA: 0x001528D1 File Offset: 0x00150AD1
			protected override void HourlyTick()
			{
			}

			// Token: 0x0600492E RID: 18734 RVA: 0x001528D3 File Offset: 0x00150AD3
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new BettingFraudIssueBehavior.BettingFraudQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(45f), 0);
			}

			// Token: 0x0600492F RID: 18735 RVA: 0x001528EC File Offset: 0x00150AEC
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x06004930 RID: 18736 RVA: 0x001528F0 File Offset: 0x00150AF0
			protected override bool CanPlayerTakeQuestConditions(Hero issueOwner, out IssueBase.PreconditionFlags flag, out Hero relationHero, out SkillObject skill)
			{
				relationHero = null;
				skill = null;
				flag = IssueBase.PreconditionFlags.None;
				if (Clan.PlayerClan.Renown < 50f)
				{
					flag |= IssueBase.PreconditionFlags.Renown;
				}
				if (issueOwner.GetRelationWithPlayer() < -10f)
				{
					flag |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueOwner;
				}
				if (Hero.MainHero.GetSkillValue(DefaultSkills.OneHanded) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.TwoHanded) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Polearm) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Bow) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Crossbow) < 50 && Hero.MainHero.GetSkillValue(DefaultSkills.Throwing) < 50)
				{
					if (Hero.MainHero.GetSkillValue(DefaultSkills.OneHanded) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.OneHanded;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.TwoHanded) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.TwoHanded;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Polearm) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Polearm;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Bow) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Bow;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Crossbow) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Crossbow;
					}
					else if (Hero.MainHero.GetSkillValue(DefaultSkills.Throwing) < 50)
					{
						flag |= IssueBase.PreconditionFlags.Skill;
						skill = DefaultSkills.Throwing;
					}
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06004931 RID: 18737 RVA: 0x00152A8C File Offset: 0x00150C8C
			public override bool IssueStayAliveConditions()
			{
				return true;
			}

			// Token: 0x06004932 RID: 18738 RVA: 0x00152A8F File Offset: 0x00150C8F
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x040018EF RID: 6383
			private const int NeededTournamentCount = 5;

			// Token: 0x040018F0 RID: 6384
			private const int IssueDuration = 45;

			// Token: 0x040018F1 RID: 6385
			private const int MainHeroSkillLimit = 50;

			// Token: 0x040018F2 RID: 6386
			private const int MainClanRenownLimit = 50;

			// Token: 0x040018F3 RID: 6387
			private const int RelationLimitWithIssueOwner = -10;

			// Token: 0x040018F4 RID: 6388
			private const float IssueOwnerPowerPenaltyForIssueEffect = -0.2f;
		}

		// Token: 0x0200060A RID: 1546
		public class BettingFraudQuest : QuestBase
		{
			// Token: 0x06004933 RID: 18739 RVA: 0x00152A91 File Offset: 0x00150C91
			internal static void AutoGeneratedStaticCollectObjectsBettingFraudQuest(object o, List<object> collectedObjects)
			{
				((BettingFraudIssueBehavior.BettingFraudQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004934 RID: 18740 RVA: 0x00152A9F File Offset: 0x00150C9F
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._thug);
				collectedObjects.Add(this._startLog);
				collectedObjects.Add(this._counterOfferNotable);
			}

			// Token: 0x06004935 RID: 18741 RVA: 0x00152ACC File Offset: 0x00150CCC
			internal static object AutoGeneratedGetMemberValue_thug(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._thug;
			}

			// Token: 0x06004936 RID: 18742 RVA: 0x00152AD9 File Offset: 0x00150CD9
			internal static object AutoGeneratedGetMemberValue_startLog(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._startLog;
			}

			// Token: 0x06004937 RID: 18743 RVA: 0x00152AE6 File Offset: 0x00150CE6
			internal static object AutoGeneratedGetMemberValue_counterOfferNotable(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._counterOfferNotable;
			}

			// Token: 0x06004938 RID: 18744 RVA: 0x00152AF3 File Offset: 0x00150CF3
			internal static object AutoGeneratedGetMemberValue_fixedTournamentCount(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._fixedTournamentCount;
			}

			// Token: 0x06004939 RID: 18745 RVA: 0x00152B05 File Offset: 0x00150D05
			internal static object AutoGeneratedGetMemberValue_minorOffensiveCount(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._minorOffensiveCount;
			}

			// Token: 0x0600493A RID: 18746 RVA: 0x00152B17 File Offset: 0x00150D17
			internal static object AutoGeneratedGetMemberValue_counterOfferConversationDone(object o)
			{
				return ((BettingFraudIssueBehavior.BettingFraudQuest)o)._counterOfferConversationDone;
			}

			// Token: 0x17000E80 RID: 3712
			// (get) Token: 0x0600493B RID: 18747 RVA: 0x00152B29 File Offset: 0x00150D29
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=xhVrxgC4}Betting Fraud", null);
				}
			}

			// Token: 0x17000E81 RID: 3713
			// (get) Token: 0x0600493C RID: 18748 RVA: 0x00152B36 File Offset: 0x00150D36
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000E82 RID: 3714
			// (get) Token: 0x0600493D RID: 18749 RVA: 0x00152B39 File Offset: 0x00150D39
			private TextObject StartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=6rweIvZS}{QUEST_GIVER.LINK}, a gang leader from {SETTLEMENT} offers you to fix 5 tournaments together and share the profit.\n {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to enter 5 tournaments and follow the instructions given by {?QUEST_GIVER.GENDER}her{?}his{\\?} associate.", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x17000E83 RID: 3715
			// (get) Token: 0x0600493E RID: 18750 RVA: 0x00152B79 File Offset: 0x00150D79
			private TextObject CurrentDirectiveLog
			{
				get
				{
					TextObject textObject = new TextObject("{=dnZekyZI}Directive from {QUEST_GIVER.LINK}: {DIRECTIVE}", null);
					textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
					textObject.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
					return textObject;
				}
			}

			// Token: 0x17000E84 RID: 3716
			// (get) Token: 0x0600493F RID: 18751 RVA: 0x00152BB0 File Offset: 0x00150DB0
			private TextObject QuestFailedWithTimeOutLog
			{
				get
				{
					TextObject textObject = new TextObject("{=2brAaeFh}You failed to complete tournaments in time. {QUEST_GIVER.LINK} will certainly be disappointed.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x06004940 RID: 18752 RVA: 0x00152BE4 File Offset: 0x00150DE4
			public BettingFraudQuest(string questId, Hero questGiver, CampaignTime duration, int rewardGold) : base(questId, questGiver, duration, rewardGold)
			{
				this._counterOfferNotable = null;
				this._fixedTournamentCount = 0;
				this._minorOffensiveCount = 0;
				this._counterOfferAccepted = false;
				this._readyToStartTournament = false;
				this._startTournamentEndConversation = false;
				this._counterOfferConversationDone = false;
				this._currentDirective = BettingFraudIssueBehavior.BettingFraudQuest.Directives.None;
				this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.None;
				this._thug = MBObjectManager.Instance.GetObject<CharacterObject>((MBRandom.RandomFloat > 0.5f) ? "betting_fraud_thug_male" : "betting_fraud_thug_female");
				this._startLog = null;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06004941 RID: 18753 RVA: 0x00152C76 File Offset: 0x00150E76
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x06004942 RID: 18754 RVA: 0x00152C7E File Offset: 0x00150E7E
			protected override void HourlyTick()
			{
			}

			// Token: 0x06004943 RID: 18755 RVA: 0x00152C80 File Offset: 0x00150E80
			private void SelectCounterOfferNotable(Settlement settlement)
			{
				this._counterOfferNotable = settlement.Notables.GetRandomElement<Hero>();
			}

			// Token: 0x06004944 RID: 18756 RVA: 0x00152C93 File Offset: 0x00150E93
			private void IncreaseMinorOffensive()
			{
				this._minorOffensiveCount++;
				this._currentDirective = BettingFraudIssueBehavior.BettingFraudQuest.Directives.None;
				if (this._minorOffensiveCount >= 2)
				{
					this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SecondMinorOffense;
					return;
				}
				this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MinorOffense;
			}

			// Token: 0x06004945 RID: 18757 RVA: 0x00152CC2 File Offset: 0x00150EC2
			private void IncreaseFixedTournamentCount()
			{
				this._fixedTournamentCount++;
				this._startLog.UpdateCurrentProgress(this._fixedTournamentCount);
				this._currentDirective = BettingFraudIssueBehavior.BettingFraudQuest.Directives.None;
				if (this._fixedTournamentCount >= 5)
				{
					this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.BigReward;
					return;
				}
				this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SmallReward;
			}

			// Token: 0x06004946 RID: 18758 RVA: 0x00152D02 File Offset: 0x00150F02
			private void SetCurrentDirective()
			{
				this._currentDirective = ((MBRandom.RandomFloat <= 0.33f) ? BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt3RdRound : ((MBRandom.RandomFloat < 0.5f) ? BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt4ThRound : BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament));
				base.AddLog(this.CurrentDirectiveLog, false);
			}

			// Token: 0x06004947 RID: 18759 RVA: 0x00152D38 File Offset: 0x00150F38
			private void StartTournamentMission()
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
				GameMenu.SwitchToMenu("town");
				tournamentGame.PrepareForTournamentGame(true);
				Campaign.Current.TournamentManager.OnPlayerJoinTournament(tournamentGame.GetType(), Settlement.CurrentSettlement);
			}

			// Token: 0x06004948 RID: 18760 RVA: 0x00152D8C File Offset: 0x00150F8C
			protected override void RegisterEvents()
			{
				CampaignEvents.PlayerEliminatedFromTournament.AddNonSerializedListener(this, new Action<int, Town>(this.OnPlayerEliminatedFromTournament));
				CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
				CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.OnGameMenuOpened));
			}

			// Token: 0x06004949 RID: 18761 RVA: 0x00152DDE File Offset: 0x00150FDE
			private void OnPlayerEliminatedFromTournament(int round, Town town)
			{
				this._startTournamentEndConversation = true;
				if (round == (int)this._currentDirective)
				{
					this.IncreaseFixedTournamentCount();
					return;
				}
				if (round < (int)this._currentDirective)
				{
					this.IncreaseMinorOffensive();
					return;
				}
				if (round > (int)this._currentDirective)
				{
					this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense;
				}
			}

			// Token: 0x0600494A RID: 18762 RVA: 0x00152E18 File Offset: 0x00151018
			private void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
			{
				if (participants.Contains(CharacterObject.PlayerCharacter) && this._currentDirective != BettingFraudIssueBehavior.BettingFraudQuest.Directives.None)
				{
					this._startTournamentEndConversation = true;
					if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament)
					{
						if (winner == CharacterObject.PlayerCharacter)
						{
							this.IncreaseFixedTournamentCount();
							return;
						}
						this.IncreaseMinorOffensive();
						return;
					}
					else if (winner == CharacterObject.PlayerCharacter)
					{
						this._afterTournamentConversationState = BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense;
					}
				}
			}

			// Token: 0x0600494B RID: 18763 RVA: 0x00152E70 File Offset: 0x00151070
			private void OnGameMenuOpened(MenuCallbackArgs args)
			{
				if (args.MenuContext.GameMenu.StringId == "menu_town_tournament_join")
				{
					GameMenu.SwitchToMenu("menu_town_tournament_join_betting_fraud");
				}
				if (args.MenuContext.GameMenu.StringId == "menu_town_tournament_join_betting_fraud")
				{
					if (this._readyToStartTournament)
					{
						if (this._fixedTournamentCount == 4 && !this._counterOfferConversationDone && this._counterOfferNotable != null && this._currentDirective != BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament)
						{
							CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._counterOfferNotable.CharacterObject, null, false, false, false, false, false, false));
						}
						else
						{
							this.StartTournamentMission();
							this._readyToStartTournament = false;
						}
					}
					if (this._fixedTournamentCount == 4 && (this._counterOfferNotable == null || this._counterOfferNotable.CurrentSettlement != Settlement.CurrentSettlement))
					{
						this.SelectCounterOfferNotable(Settlement.CurrentSettlement);
					}
				}
				if (this._startTournamentEndConversation)
				{
					CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._thug, null, false, false, false, false, false, false));
				}
			}

			// Token: 0x0600494C RID: 18764 RVA: 0x00152F86 File Offset: 0x00151186
			protected override void OnTimedOut()
			{
				base.OnTimedOut();
				this.PlayerDidNotCompleteTournaments();
			}

			// Token: 0x0600494D RID: 18765 RVA: 0x00152F94 File Offset: 0x00151194
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = this.GetOfferDialogFlow();
				this.DiscussDialogFlow = this.GetDiscussDialogFlow();
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogWithThugStart(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetDialogWithThugEnd(), this);
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetCounterOfferDialog(), this);
			}

			// Token: 0x0600494E RID: 18766 RVA: 0x00152FFC File Offset: 0x001511FC
			private DialogFlow GetOfferDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=sp52g5AQ}Very good, very good. Try to enter five tournaments over the next 45 days or so. Right before the fight you'll hear from my associate how far I want you to go in the rankings before you lose.[if:convo_delighted][ib:hip]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).NpcLine(new TextObject("{=ADIYnC4u}Now, I know you can't win every fight, so if you underperform once or twice, I'd understand. But if you lose every time, or worse, if you overperform, well, then I'll be a bit angry.[if:convo_nonchalant][ib:normal2]", null), null, null).NpcLine(new TextObject("{=1hOPCf8I}But I'm sure you won't disappoint me. Enjoy your riches![if:convo_focused_happy][ib:confident]", null), null, null).Consequence(new ConversationSentence.OnConsequenceDelegate(this.OfferDialogFlowConsequence)).CloseDialog();
			}

			// Token: 0x0600494F RID: 18767 RVA: 0x00153072 File Offset: 0x00151272
			private void OfferDialogFlowConsequence()
			{
				base.StartQuest();
				this._startLog = base.AddDiscreteLog(this.StartLog, new TextObject("{=dLfWFa61}Fix 5 Tournaments", null), 0, 5, null, false);
			}

			// Token: 0x06004950 RID: 18768 RVA: 0x0015309C File Offset: 0x0015129C
			private DialogFlow GetDiscussDialogFlow()
			{
				return DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=!}{RESPONSE_TEXT}", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DiscussDialogCondition)).BeginPlayerOptions().PlayerOption(new TextObject("{=abLgPWzf}I will continue to honor our deal. Do not forget to do your end, that's all.", null), null).BeginNpcOptions().NpcOption(new TextObject("{=ZLPEsMUx}Well, there are tournament happening in {NEARBY_TOURNAMENTS_LIST} right now. You can go there and do the job. Your denars will be waiting for you.", null), new ConversationSentence.OnConditionDelegate(this.NpcTournamentLocationCondition), null, null).CloseDialog().NpcDefaultOption("{=sUfSCLQx}Sadly, I've heard no news of an upcoming tournament. I am sure one will be held before too long.").CloseDialog().EndNpcOptions().CloseDialog().PlayerOption(new TextObject("{=XUS5wNsD}I feel like I do all the job and you get your denars.", null), null).BeginNpcOptions().NpcOption(new TextObject("{=ZLPEsMUx}Well, there are tournament happening in {NEARBY_TOURNAMENTS_LIST} right now. You can go there and do the job. Your denars will be waiting for you.", null), new ConversationSentence.OnConditionDelegate(this.NpcTournamentLocationCondition), null, null).CloseDialog().NpcDefaultOption("{=sUfSCLQx}Sadly, I've heard no news of an upcoming tournament. I am sure one will be held before too long.").CloseDialog().EndNpcOptions().CloseDialog().EndPlayerOptions().CloseDialog();
			}

			// Token: 0x06004951 RID: 18769 RVA: 0x0015318C File Offset: 0x0015138C
			private bool DiscussDialogCondition()
			{
				bool flag = Hero.OneToOneConversationHero == base.QuestGiver;
				if (flag)
				{
					if (this._minorOffensiveCount > 0)
					{
						MBTextManager.SetTextVariable("RESPONSE_TEXT", new TextObject("{=7SPwGYvf}I had expected better of you. But even the best can fail sometimes. Just make sure it does not happen again.[if:convo_bored][ib:closed2] ", null), false);
						return flag;
					}
					MBTextManager.SetTextVariable("RESPONSE_TEXT", new TextObject("{=vo0uhUsZ}I have high hopes for you, friend. Just follow my directives and we will be rich.[if:convo_relaxed_happy][ib:demure2]", null), false);
				}
				return flag;
			}

			// Token: 0x06004952 RID: 18770 RVA: 0x001531E0 File Offset: 0x001513E0
			private bool NpcTournamentLocationCondition()
			{
				List<Town> list = (from x in Town.AllTowns
				where Campaign.Current.TournamentManager.GetTournamentGame(x) != null && x != Settlement.CurrentSettlement.Town
				select x).ToList<Town>();
				list = (from x in list
				orderby x.Settlement.Position2D.DistanceSquared(Settlement.CurrentSettlement.Position2D)
				select x).ToList<Town>();
				if (list.Count > 0)
				{
					MBTextManager.SetTextVariable("NEARBY_TOURNAMENTS_LIST", list[0].Name, false);
					return true;
				}
				return false;
			}

			// Token: 0x06004953 RID: 18771 RVA: 0x0015326C File Offset: 0x0015146C
			private DialogFlow GetDialogWithThugStart()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=!}{GREETING_LINE}", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogWithThugStartCondition)).BeginPlayerOptions().PlayerOption(new TextObject("{=!}{POSITIVE_OPTION}", null), null).Condition(new ConversationSentence.OnConditionDelegate(this.PositiveOptionCondition)).Consequence(new ConversationSentence.OnConsequenceDelegate(this.PositiveOptionConsequences)).CloseDialog().PlayerOption(new TextObject("{=!}{NEGATIVE_OPTION}", null), null).Condition(new ConversationSentence.OnConditionDelegate(this.NegativeOptionCondition)).Consequence(new ConversationSentence.OnConsequenceDelegate(this.NegativeOptionConsequence)).CloseDialog().EndPlayerOptions().CloseDialog();
			}

			// Token: 0x06004954 RID: 18772 RVA: 0x00153328 File Offset: 0x00151528
			private bool DialogWithThugStartCondition()
			{
				bool flag = CharacterObject.OneToOneConversationCharacter == this._thug && !this._startTournamentEndConversation;
				if (flag)
				{
					this.SetCurrentDirective();
					if (this._fixedTournamentCount < 2)
					{
						TextObject textObject = new TextObject("{=xYu4yVRU}Hey there friend. So... You don't need to know my name, but suffice to say that we're both friends of {QUEST_GIVER.LINK}. Here's {?QUEST_GIVER.GENDER}her{?}his{\\?} message for you: {DIRECTIVE}.[ib:confident][if:convo_nonchalant]", null);
						textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
						textObject.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
						MBTextManager.SetTextVariable("GREETING_LINE", textObject, false);
						return flag;
					}
					if (this._fixedTournamentCount < 4)
					{
						TextObject textObject2 = new TextObject("{=cQE9tQOy}My friend! Good to see you. You did very well in that last fight. People definitely won't be expecting you to \"{DIRECTIVE}\". What a surprise that would be. Well, I should not keep you from your tournament. You know what to do.[if:convo_happy][ib:closed2]", null);
						textObject2.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
						MBTextManager.SetTextVariable("GREETING_LINE", textObject2, false);
						return flag;
					}
					TextObject textObject3 = new TextObject("{=RVLPQ4rm}My friend. I am almost sad that these meetings are going to come to an end. Well, a deal is a deal. I won't beat around the bush. Here's your final message: {DIRECTIVE}. I wish you luck, right up until the moment that you have to go down.[if:convo_mocking_teasing][ib:closed]", null);
					textObject3.SetTextVariable("DIRECTIVE", this.GetDirectiveText());
					MBTextManager.SetTextVariable("GREETING_LINE", textObject3, false);
				}
				return flag;
			}

			// Token: 0x06004955 RID: 18773 RVA: 0x00153404 File Offset: 0x00151604
			private bool PositiveOptionCondition()
			{
				if (this._fixedTournamentCount < 2)
				{
					MBTextManager.SetTextVariable("POSITIVE_OPTION", new TextObject("{=PrUauabl}As long as the payment is as we talked, you got nothing to worry about.", null), false);
				}
				else if (this._fixedTournamentCount < 4)
				{
					MBTextManager.SetTextVariable("POSITIVE_OPTION", new TextObject("{=TKRsPVMU}Yes, I did. Be around when the tournament is over.", null), false);
				}
				else
				{
					MBTextManager.SetTextVariable("POSITIVE_OPTION", new TextObject("{=26XPQw2v}I will miss this little deal we had. See you at the end", null), false);
				}
				return true;
			}

			// Token: 0x06004956 RID: 18774 RVA: 0x0015346A File Offset: 0x0015166A
			private void PositiveOptionConsequences()
			{
				this._readyToStartTournament = true;
			}

			// Token: 0x06004957 RID: 18775 RVA: 0x00153473 File Offset: 0x00151673
			private bool NegativeOptionCondition()
			{
				bool flag = this._fixedTournamentCount >= 4;
				if (flag)
				{
					MBTextManager.SetTextVariable("NEGATIVE_OPTION", new TextObject("{=vapdvRQO}This deal was a mistake. We will not talk again after this last tournament.", null), false);
				}
				return flag;
			}

			// Token: 0x06004958 RID: 18776 RVA: 0x0015349A File Offset: 0x0015169A
			private void NegativeOptionConsequence()
			{
				this._readyToStartTournament = true;
			}

			// Token: 0x06004959 RID: 18777 RVA: 0x001534A4 File Offset: 0x001516A4
			private TextObject GetDirectiveText()
			{
				if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt3RdRound)
				{
					return new TextObject("{=aHlcBLYB}Lose this tournament at 3rd round", null);
				}
				if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.LoseAt4ThRound)
				{
					return new TextObject("{=hc1mnqOx}Lose this tournament at 4th round", null);
				}
				if (this._currentDirective == BettingFraudIssueBehavior.BettingFraudQuest.Directives.WinTheTournament)
				{
					return new TextObject("{=hl4pTsaO}Win this tournament", null);
				}
				return TextObject.Empty;
			}

			// Token: 0x0600495A RID: 18778 RVA: 0x001534F8 File Offset: 0x001516F8
			private DialogFlow GetDialogWithThugEnd()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=!}{GREETING_LINE}", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogWithThugEndCondition)).Consequence(new ConversationSentence.OnConsequenceDelegate(this.DialogWithThugEndConsequence)).CloseDialog();
			}

			// Token: 0x0600495B RID: 18779 RVA: 0x0015354C File Offset: 0x0015174C
			private bool DialogWithThugEndCondition()
			{
				bool flag = CharacterObject.OneToOneConversationCharacter == this._thug && this._startTournamentEndConversation;
				if (flag)
				{
					TextObject textObject = TextObject.Empty;
					switch (this._afterTournamentConversationState)
					{
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SmallReward:
						textObject = new TextObject("{=ZM8t4ZW2}We are very impressed, my friend. Here is the payment as promised. I hope we can continue this profitable partnership. See you at the next tournament.[if:convo_happy][ib:demure]", null);
						GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 250, false);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.BigReward:
						textObject = new TextObject("{=9vOZWY25}What an exciting result! I will definitely miss these tournaments. Well, maybe after some time goes by and memories get a little hazy we can continue. Here is the last payment. Very well deserved.[if:convo_happy][ib:demure]", null);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MinorOffense:
						textObject = new TextObject("{=d8bGHJnZ}This was not we were expecting. We lost some money. Well, Lady Fortune always casts her ballot too in these contests. But try to reassure us that this was her plan, and not yours, eh?[if:convo_grave][ib:closed2]", null);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SecondMinorOffense:
						textObject = new TextObject("{=bNAG2t8S}Well, my friend, either you're playing us false or you're just not very good at this. Either way, {QUEST_GIVER.LINK} wishes to tell you that {?QUEST_GIVER.GENDER}her{?}his{\\?} association with you is over.[if:convo_predatory][ib:closed2]", null);
						textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
						break;
					case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense:
						textObject = new TextObject("{=Lyqx3NYE}Well... What happened back there... That wasn't bad luck or incompetence. {QUEST_GIVER.LINK} trusted in you and {?QUEST_GIVER.GENDER}She{?}He{\\?} doesn't take well to betrayal.[if:convo_angry][ib:warrior]", null);
						textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
						break;
					default:
						Debug.FailedAssert("After tournament conversation state is not set!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Issues\\BettingFraudIssueBehavior.cs", "DialogWithThugEndCondition", 722);
						break;
					}
					MBTextManager.SetTextVariable("GREETING_LINE", textObject, false);
				}
				return flag;
			}

			// Token: 0x0600495C RID: 18780 RVA: 0x00153650 File Offset: 0x00151850
			private void DialogWithThugEndConsequence()
			{
				this._startTournamentEndConversation = false;
				switch (this._afterTournamentConversationState)
				{
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SmallReward:
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MinorOffense:
					break;
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.BigReward:
					this.MainHeroSuccessfullyFixedTournaments();
					return;
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.SecondMinorOffense:
					this.MainHeroFailToFixTournaments();
					return;
				case BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState.MajorOffense:
					if (this._counterOfferAccepted)
					{
						this.MainHeroAcceptsCounterOffer();
						return;
					}
					this.MainHeroChooseNotToFixTournaments();
					break;
				default:
					return;
				}
			}

			// Token: 0x0600495D RID: 18781 RVA: 0x001536AC File Offset: 0x001518AC
			private DialogFlow GetCounterOfferDialog()
			{
				return DialogFlow.CreateDialogFlow("start", 125).NpcLine(new TextObject("{=bUfBHSsz}Hold on a moment, friend. I need to talk to you.[ib:aggressive]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.CounterOfferConversationStartCondition)).Consequence(new ConversationSentence.OnConsequenceDelegate(this.CounterOfferConversationStartConsequence)).PlayerLine(new TextObject("{=PZfR7hEK}What do you want? I have a tournament to prepare for.", null), null).NpcLine(new TextObject("{=GN9F316V}Oh of course you do. {QUEST_GIVER.LINK}'s people have been running around placing bets - we know all about your arrangement, you see. And let me tell you something: as these arrangements go, {QUEST_GIVER.LINK} is getting you cheap. Do you want to see real money? Win this tournament and I will pay you what you're worth. And isn't it better to win than to lose?[if:convo_mocking_aristocratic][ib:confident2]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.AccusationCondition)).BeginPlayerOptions().PlayerOption(new TextObject("{=MacG8ikN}I will think about it.", null), null).Consequence(new ConversationSentence.OnConsequenceDelegate(this.CounterOfferAcceptedConsequence)).CloseDialog().PlayerOption(new TextObject("{=bT279pk9}I have no idea what you talking about. Be on your way, friend.", null), null).CloseDialog().EndPlayerOptions().CloseDialog();
			}

			// Token: 0x0600495E RID: 18782 RVA: 0x00153779 File Offset: 0x00151979
			private bool CounterOfferConversationStartCondition()
			{
				return this._counterOfferNotable != null && CharacterObject.OneToOneConversationCharacter == this._counterOfferNotable.CharacterObject;
			}

			// Token: 0x0600495F RID: 18783 RVA: 0x00153797 File Offset: 0x00151997
			private void CounterOfferConversationStartConsequence()
			{
				this._counterOfferConversationDone = true;
			}

			// Token: 0x06004960 RID: 18784 RVA: 0x001537A0 File Offset: 0x001519A0
			private bool AccusationCondition()
			{
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, null, false);
				return true;
			}

			// Token: 0x06004961 RID: 18785 RVA: 0x001537BB File Offset: 0x001519BB
			private void CounterOfferAcceptedConsequence()
			{
				this._counterOfferAccepted = true;
			}

			// Token: 0x06004962 RID: 18786 RVA: 0x001537C4 File Offset: 0x001519C4
			private void MainHeroSuccessfullyFixedTournaments()
			{
				TextObject textObject = new TextObject("{=aCA83avL}You have placed in the tournaments as {QUEST_GIVER.LINK} wished.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				base.AddLog(textObject, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 2500, false);
				Clan.PlayerClan.AddRenown(2f, true);
				base.QuestGiver.AddPower(10f);
				base.QuestGiver.CurrentSettlement.Town.Security += -20f;
				this.RelationshipChangeWithQuestGiver = 5;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06004963 RID: 18787 RVA: 0x0015385C File Offset: 0x00151A5C
			private void MainHeroFailToFixTournaments()
			{
				TextObject textObject = new TextObject("{=ETbToaZC}You have failed to place in the tournaments as {QUEST_GIVER.LINK} wished.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				base.AddLog(textObject, false);
				base.QuestGiver.AddPower(-10f);
				base.QuestGiver.CurrentSettlement.Town.Security += 10f;
				this.RelationshipChangeWithQuestGiver = -5;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06004964 RID: 18788 RVA: 0x001538D8 File Offset: 0x00151AD8
			private void MainHeroChooseNotToFixTournaments()
			{
				TextObject textObject = new TextObject("{=52smwnzz}You have chosen not to place in the tournaments as {QUEST_GIVER.LINK} wished.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				base.AddLog(textObject, false);
				base.QuestGiver.AddPower(-15f);
				base.QuestGiver.CurrentSettlement.Town.Security += 15f;
				this.RelationshipChangeWithQuestGiver = -10;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06004965 RID: 18789 RVA: 0x00153954 File Offset: 0x00151B54
			private void MainHeroAcceptsCounterOffer()
			{
				TextObject textObject = new TextObject("{=nb0wqaGA}You have made a deal with {NOTABLE.LINK} to betray {QUEST_GIVER.LINK}.", null);
				textObject.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, false);
				textObject.SetCharacterProperties("NOTABLE", this._counterOfferNotable.CharacterObject, false);
				base.AddLog(textObject, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 4500, false);
				base.QuestGiver.AddPower(-15f);
				base.QuestGiver.CurrentSettlement.Town.Security += 15f;
				ChangeRelationAction.ApplyPlayerRelation(this._counterOfferNotable, 2, true, true);
				this.RelationshipChangeWithQuestGiver = -10;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06004966 RID: 18790 RVA: 0x00153A03 File Offset: 0x00151C03
			private void PlayerDidNotCompleteTournaments()
			{
				base.AddLog(this.QuestFailedWithTimeOutLog, false);
				ChangeRelationAction.ApplyPlayerRelation(base.QuestGiver, -5, true, true);
			}

			// Token: 0x040018F5 RID: 6389
			private const int TournamentFixCount = 5;

			// Token: 0x040018F6 RID: 6390
			private const int MinorOffensiveLimit = 2;

			// Token: 0x040018F7 RID: 6391
			private const int SmallReward = 250;

			// Token: 0x040018F8 RID: 6392
			private const int BigReward = 2500;

			// Token: 0x040018F9 RID: 6393
			private const int CounterOfferReward = 4500;

			// Token: 0x040018FA RID: 6394
			private const string MaleThug = "betting_fraud_thug_male";

			// Token: 0x040018FB RID: 6395
			private const string FemaleThug = "betting_fraud_thug_female";

			// Token: 0x040018FC RID: 6396
			[SaveableField(100)]
			private JournalLog _startLog;

			// Token: 0x040018FD RID: 6397
			[SaveableField(1)]
			private Hero _counterOfferNotable;

			// Token: 0x040018FE RID: 6398
			[SaveableField(10)]
			internal readonly CharacterObject _thug;

			// Token: 0x040018FF RID: 6399
			[SaveableField(20)]
			private int _fixedTournamentCount;

			// Token: 0x04001900 RID: 6400
			[SaveableField(30)]
			private int _minorOffensiveCount;

			// Token: 0x04001901 RID: 6401
			private BettingFraudIssueBehavior.BettingFraudQuest.Directives _currentDirective;

			// Token: 0x04001902 RID: 6402
			private BettingFraudIssueBehavior.BettingFraudQuest.AfterTournamentConversationState _afterTournamentConversationState;

			// Token: 0x04001903 RID: 6403
			private bool _counterOfferAccepted;

			// Token: 0x04001904 RID: 6404
			private bool _readyToStartTournament;

			// Token: 0x04001905 RID: 6405
			private bool _startTournamentEndConversation;

			// Token: 0x04001906 RID: 6406
			[SaveableField(40)]
			private bool _counterOfferConversationDone;

			// Token: 0x0200079E RID: 1950
			private enum Directives
			{
				// Token: 0x04001F92 RID: 8082
				None,
				// Token: 0x04001F93 RID: 8083
				LoseAt3RdRound = 2,
				// Token: 0x04001F94 RID: 8084
				LoseAt4ThRound,
				// Token: 0x04001F95 RID: 8085
				WinTheTournament
			}

			// Token: 0x0200079F RID: 1951
			private enum AfterTournamentConversationState
			{
				// Token: 0x04001F97 RID: 8087
				None,
				// Token: 0x04001F98 RID: 8088
				SmallReward,
				// Token: 0x04001F99 RID: 8089
				BigReward,
				// Token: 0x04001F9A RID: 8090
				MinorOffense,
				// Token: 0x04001F9B RID: 8091
				SecondMinorOffense,
				// Token: 0x04001F9C RID: 8092
				MajorOffense
			}
		}

		// Token: 0x0200060B RID: 1547
		public class BettingFraudIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06004968 RID: 18792 RVA: 0x00153A31 File Offset: 0x00151C31
			public BettingFraudIssueTypeDefiner() : base(600327)
			{
			}

			// Token: 0x06004969 RID: 18793 RVA: 0x00153A3E File Offset: 0x00151C3E
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(BettingFraudIssueBehavior.BettingFraudIssue), 1, null);
				base.AddClassDefinition(typeof(BettingFraudIssueBehavior.BettingFraudQuest), 2, null);
			}
		}
	}
}
