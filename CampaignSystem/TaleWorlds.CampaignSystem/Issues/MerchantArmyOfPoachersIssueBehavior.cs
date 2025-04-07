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
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000316 RID: 790
	public class MerchantArmyOfPoachersIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x06002D89 RID: 11657 RVA: 0x000BE996 File Offset: 0x000BCB96
		private void engage_poachers_consequence(MenuCallbackArgs args)
		{
			MerchantArmyOfPoachersIssueBehavior.Instance.StartQuestBattle();
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06002D8A RID: 11658 RVA: 0x000BE9A4 File Offset: 0x000BCBA4
		private static MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest Instance
		{
			get
			{
				MerchantArmyOfPoachersIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<MerchantArmyOfPoachersIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest cachedQuest;
						if ((cachedQuest = (enumerator.Current as MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)) != null)
						{
							campaignBehavior._cachedQuest = cachedQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000BEA3C File Offset: 0x000BCC3C
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000BEA6C File Offset: 0x000BCC6C
		private bool poachers_menu_back_condition(MenuCallbackArgs args)
		{
			return Hero.MainHero.IsWounded;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x000BEA78 File Offset: 0x000BCC78
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu("army_of_poachers_village", "{=eaQxeRh6}A boy runs out of the village and asks you to talk to the leader of the poachers. The villagers want to avoid a fight outside their homes.", new OnInitDelegate(this.army_of_poachers_village_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("army_of_poachers_village", "engage_the_poachers", "{=xF7he8fZ}Fight the poachers", new GameMenuOption.OnConditionDelegate(this.engage_poachers_condition), new GameMenuOption.OnConsequenceDelegate(this.engage_poachers_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("army_of_poachers_village", "talk_to_the_poachers", "{=wwJGE28v}Negotiate with the poachers", new GameMenuOption.OnConditionDelegate(this.talk_to_leader_of_poachers_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_leader_of_poachers_consequence), false, -1, false, null);
			gameStarter.AddGameMenuOption("army_of_poachers_village", "back_poachers", "{=E1OwmQFb}Back", new GameMenuOption.OnConditionDelegate(this.poachers_menu_back_condition), new GameMenuOption.OnConsequenceDelegate(this.poachers_menu_back_consequence), false, -1, false, null);
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000BEB38 File Offset: 0x000BCD38
		private void army_of_poachers_village_on_init(MenuCallbackArgs args)
		{
			if (MerchantArmyOfPoachersIssueBehavior.Instance != null && MerchantArmyOfPoachersIssueBehavior.Instance.IsOngoing)
			{
				args.MenuContext.SetBackgroundMeshName(MerchantArmyOfPoachersIssueBehavior.Instance._questVillage.Settlement.SettlementComponent.WaitMeshName);
				if (MerchantArmyOfPoachersIssueBehavior.Instance._poachersParty == null && !Hero.MainHero.IsWounded)
				{
					MerchantArmyOfPoachersIssueBehavior.Instance.CreatePoachersParty();
				}
				if (MerchantArmyOfPoachersIssueBehavior.Instance._isReadyToBeFinalized && PlayerEncounter.Current != null)
				{
					bool flag = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Battle.PlayerSide;
					PlayerEncounter.Update();
					if (PlayerEncounter.Current == null)
					{
						MerchantArmyOfPoachersIssueBehavior.Instance._isReadyToBeFinalized = false;
						if (flag)
						{
							MerchantArmyOfPoachersIssueBehavior.Instance.QuestSuccessWithPlayerDefeatedPoachers();
						}
						else
						{
							MerchantArmyOfPoachersIssueBehavior.Instance.QuestFailWithPlayerDefeatedAgainstPoachers();
						}
					}
					else if (PlayerEncounter.Battle.WinningSide == BattleSideEnum.None)
					{
						PlayerEncounter.LeaveEncounter = true;
						PlayerEncounter.Update();
						MerchantArmyOfPoachersIssueBehavior.Instance.QuestFailWithPlayerDefeatedAgainstPoachers();
					}
					else if (flag && PlayerEncounter.Current != null && Game.Current.GameStateManager.ActiveState is MapState)
					{
						PlayerEncounter.Finish(true);
						MerchantArmyOfPoachersIssueBehavior.Instance.QuestSuccessWithPlayerDefeatedPoachers();
					}
				}
				if (MerchantArmyOfPoachersIssueBehavior.Instance != null && MerchantArmyOfPoachersIssueBehavior.Instance._talkedToPoachersBattleWillStart)
				{
					MerchantArmyOfPoachersIssueBehavior.Instance.StartQuestBattle();
				}
			}
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000BEC77 File Offset: 0x000BCE77
		private bool engage_poachers_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			if (Hero.MainHero.IsWounded)
			{
				args.Tooltip = new TextObject("{=gEHEQazX}You're heavily wounded and not fit for the fight. Come back when you're ready.", null);
				args.IsEnabled = false;
			}
			return true;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000BECA5 File Offset: 0x000BCEA5
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000BECA7 File Offset: 0x000BCEA7
		private bool talk_to_leader_of_poachers_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			if (Hero.MainHero.IsWounded)
			{
				args.Tooltip = new TextObject("{=gEHEQazX}You're heavily wounded and not fit for the fight. Come back when you're ready.", null);
				args.IsEnabled = false;
			}
			return true;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000BECD6 File Offset: 0x000BCED6
		private void poachers_menu_back_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000BECE4 File Offset: 0x000BCEE4
		private bool ConditionsHold(Hero issueGiver, out Village questVillage)
		{
			questVillage = null;
			if (issueGiver.CurrentSettlement == null)
			{
				return false;
			}
			questVillage = issueGiver.CurrentSettlement.BoundVillages.GetRandomElementWithPredicate((Village x) => !x.Settlement.IsUnderRaid && !x.Settlement.IsRaided);
			if (questVillage != null && issueGiver.IsMerchant && issueGiver.GetTraitLevel(DefaultTraits.Mercy) + issueGiver.GetTraitLevel(DefaultTraits.Honor) < 0)
			{
				Town town = issueGiver.CurrentSettlement.Town;
				return town != null && town.Security <= (float)60;
			}
			return false;
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x000BED78 File Offset: 0x000BCF78
		private void talk_to_leader_of_poachers_consequence(MenuCallbackArgs args)
		{
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(MerchantArmyOfPoachersIssueBehavior.Instance._poachersParty.Party), MerchantArmyOfPoachersIssueBehavior.Instance._poachersParty.Party, false, false, false, false, false, false));
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000BEDD0 File Offset: 0x000BCFD0
		public void OnCheckForIssue(Hero hero)
		{
			Village relatedObject;
			if (this.ConditionsHold(hero, out relatedObject))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue), IssueBase.IssueFrequency.Common, relatedObject));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000BEE38 File Offset: 0x000BD038
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue(issueOwner, potentialIssueData.RelatedObject as Village);
		}

		// Token: 0x04000DA1 RID: 3489
		private const IssueBase.IssueFrequency ArmyOfPoachersIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x04000DA2 RID: 3490
		private MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest _cachedQuest;

		// Token: 0x02000657 RID: 1623
		public class MerchantArmyOfPoachersIssue : IssueBase
		{
			// Token: 0x06005144 RID: 20804 RVA: 0x00172436 File Offset: 0x00170636
			internal static void AutoGeneratedStaticCollectObjectsMerchantArmyOfPoachersIssue(object o, List<object> collectedObjects)
			{
				((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005145 RID: 20805 RVA: 0x00172444 File Offset: 0x00170644
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._questVillage);
			}

			// Token: 0x06005146 RID: 20806 RVA: 0x00172459 File Offset: 0x00170659
			internal static object AutoGeneratedGetMemberValue_questVillage(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue)o)._questVillage;
			}

			// Token: 0x17001116 RID: 4374
			// (get) Token: 0x06005147 RID: 20807 RVA: 0x00172466 File Offset: 0x00170666
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 12 + MathF.Ceiling(28f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17001117 RID: 4375
			// (get) Token: 0x06005148 RID: 20808 RVA: 0x0017247C File Offset: 0x0017067C
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 3 + MathF.Ceiling(5f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17001118 RID: 4376
			// (get) Token: 0x06005149 RID: 20809 RVA: 0x00172491 File Offset: 0x00170691
			protected override int RewardGold
			{
				get
				{
					return (int)(500f + 3000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17001119 RID: 4377
			// (get) Token: 0x0600514A RID: 20810 RVA: 0x001724A6 File Offset: 0x001706A6
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x1700111A RID: 4378
			// (get) Token: 0x0600514B RID: 20811 RVA: 0x001724AA File Offset: 0x001706AA
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=Jk3mDlU6}Yeah... I've got some problems. A few years ago, I needed hides for my tannery and I hired some hunters. I didn't ask too many questions about where they came by the skins they sold me. Well, that was a bit of mistake. Now they've banded together as a gang and are trying to muscle me out of the leather business.[ib:closed2][if:convo_thinking]", null);
				}
			}

			// Token: 0x1700111B RID: 4379
			// (get) Token: 0x0600514C RID: 20812 RVA: 0x001724B7 File Offset: 0x001706B7
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=apuNQC2W}What can I do for you?", null);
				}
			}

			// Token: 0x1700111C RID: 4380
			// (get) Token: 0x0600514D RID: 20813 RVA: 0x001724C4 File Offset: 0x001706C4
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=LbTETjZu}I want you to crush them. Go to {VILLAGE} and give them a lesson they won't forget.[ib:closed2][if:convo_grave]", null);
					textObject.SetTextVariable("VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700111D RID: 4381
			// (get) Token: 0x0600514E RID: 20814 RVA: 0x001724ED File Offset: 0x001706ED
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=2ELhox6C}If you don't want to get involved in this yourself, leave one of your capable companions and {NUMBER_OF_TROOPS} men for some days.[ib:closed][if:convo_grave]", null);
					textObject.SetTextVariable("NUMBER_OF_TROOPS", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x1700111E RID: 4382
			// (get) Token: 0x0600514F RID: 20815 RVA: 0x0017250C File Offset: 0x0017070C
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=b6naGx6H}I'll rid you of those poachers myself.", null);
				}
			}

			// Token: 0x1700111F RID: 4383
			// (get) Token: 0x06005150 RID: 20816 RVA: 0x00172519 File Offset: 0x00170719
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=lA14Ubal}I can send a companion to hunt these poachers.", null);
				}
			}

			// Token: 0x17001120 RID: 4384
			// (get) Token: 0x06005151 RID: 20817 RVA: 0x00172526 File Offset: 0x00170726
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=Xmtlrrmf}Thank you.[ib:normal][if:convo_normal]  Don't forget to warn your men. These poachers are not ordinary bandits. Good luck.", null);
				}
			}

			// Token: 0x17001121 RID: 4385
			// (get) Token: 0x06005152 RID: 20818 RVA: 0x00172533 File Offset: 0x00170733
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=51ahPi69}I understand that your men are still chasing those poachers. I realize that this mess might take a little time to clean up.[ib:normal2][if:convo_grave]", null);
				}
			}

			// Token: 0x17001122 RID: 4386
			// (get) Token: 0x06005153 RID: 20819 RVA: 0x00172540 File Offset: 0x00170740
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001123 RID: 4387
			// (get) Token: 0x06005154 RID: 20820 RVA: 0x00172543 File Offset: 0x00170743
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001124 RID: 4388
			// (get) Token: 0x06005155 RID: 20821 RVA: 0x00172548 File Offset: 0x00170748
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=428B377z}{ISSUE_GIVER.LINK}, a merchant of {QUEST_GIVER_SETTLEMENT}, told you that the poachers {?ISSUE_GIVER.GENDER}she{?}he{\\?} hired are now out of control. You asked {COMPANION.LINK} to take {NEEDED_MEN_COUNT} of your men to go to {QUEST_VILLAGE} and kill the poachers. They should rejoin your party in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_GIVER_SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("NEEDED_MEN_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("QUEST_VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17001125 RID: 4389
			// (get) Token: 0x06005156 RID: 20822 RVA: 0x001725F5 File Offset: 0x001707F5
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=iHFo2kjz}Army of Poachers", null);
				}
			}

			// Token: 0x17001126 RID: 4390
			// (get) Token: 0x06005157 RID: 20823 RVA: 0x00172602 File Offset: 0x00170802
			public override TextObject Description
			{
				get
				{
					TextObject result = new TextObject("{=NCC4VUOc}{ISSUE_GIVER.LINK} wants you to get rid of the poachers who once worked for {?ISSUE_GIVER.GENDER}her{?}him{\\?} but are now out of control.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, null, false);
					return result;
				}
			}

			// Token: 0x06005158 RID: 20824 RVA: 0x00172627 File Offset: 0x00170827
			public MerchantArmyOfPoachersIssue(Hero issueOwner, Village questVillage) : base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this._questVillage = questVillage;
			}

			// Token: 0x06005159 RID: 20825 RVA: 0x00172641 File Offset: 0x00170841
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementProsperity)
				{
					return 0.2f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -1f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementLoyalty)
				{
					return -0.2f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x0600515A RID: 20826 RVA: 0x00172680 File Offset: 0x00170880
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 2, false);
			}

			// Token: 0x0600515B RID: 20827 RVA: 0x00172698 File Offset: 0x00170898
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x0600515C RID: 20828 RVA: 0x001726A8 File Offset: 0x001708A8
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				int skillValue = hero.GetSkillValue(DefaultSkills.Bow);
				int skillValue2 = hero.GetSkillValue(DefaultSkills.Crossbow);
				int skillValue3 = hero.GetSkillValue(DefaultSkills.Throwing);
				if (skillValue >= skillValue2 && skillValue >= skillValue3)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.Bow, 150);
				}
				return new ValueTuple<SkillObject, int>((skillValue2 >= skillValue3) ? DefaultSkills.Crossbow : DefaultSkills.Throwing, 150);
			}

			// Token: 0x0600515D RID: 20829 RVA: 0x0017270B File Offset: 0x0017090B
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 2, false);
			}

			// Token: 0x0600515E RID: 20830 RVA: 0x0017272C File Offset: 0x0017092C
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x0600515F RID: 20831 RVA: 0x00172730 File Offset: 0x00170930
			public override bool IssueStayAliveConditions()
			{
				return !this._questVillage.Settlement.IsUnderRaid && !this._questVillage.Settlement.IsRaided && base.IssueOwner.CurrentSettlement.Town.Security <= 90f;
			}

			// Token: 0x06005160 RID: 20832 RVA: 0x00172784 File Offset: 0x00170984
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
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 15)
				{
					flag |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flag |= IssueBase.PreconditionFlags.AtWar;
				}
				return flag == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005161 RID: 20833 RVA: 0x001727F1 File Offset: 0x001709F1
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005162 RID: 20834 RVA: 0x001727F3 File Offset: 0x001709F3
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005163 RID: 20835 RVA: 0x001727F5 File Offset: 0x001709F5
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(20f), this._questVillage, base.IssueDifficultyMultiplier, this.RewardGold);
			}

			// Token: 0x06005164 RID: 20836 RVA: 0x0017281F File Offset: 0x00170A1F
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x17001127 RID: 4391
			// (get) Token: 0x06005165 RID: 20837 RVA: 0x00172821 File Offset: 0x00170A21
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(800f + 1000f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06005166 RID: 20838 RVA: 0x00172836 File Offset: 0x00170A36
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 5;
				base.IssueOwner.AddPower(30f);
				base.IssueOwner.CurrentSettlement.Town.Prosperity += 50f;
			}

			// Token: 0x06005167 RID: 20839 RVA: 0x00172870 File Offset: 0x00170A70
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
				base.IssueOwner.AddPower(-50f);
				base.IssueOwner.CurrentSettlement.Town.Prosperity -= 30f;
				base.IssueOwner.CurrentSettlement.Town.Security -= 5f;
				TraitLevelingHelper.OnIssueFailed(base.IssueOwner, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
			}

			// Token: 0x04001AA0 RID: 6816
			private const int AlternativeSolutionTroopTierRequirement = 2;

			// Token: 0x04001AA1 RID: 6817
			private const int CompanionRequiredSkillLevel = 150;

			// Token: 0x04001AA2 RID: 6818
			private const int MinimumRequiredMenCount = 15;

			// Token: 0x04001AA3 RID: 6819
			private const int IssueDuration = 15;

			// Token: 0x04001AA4 RID: 6820
			private const int QuestTimeLimit = 20;

			// Token: 0x04001AA5 RID: 6821
			[SaveableField(10)]
			private Village _questVillage;
		}

		// Token: 0x02000658 RID: 1624
		public class MerchantArmyOfPoachersIssueQuest : QuestBase
		{
			// Token: 0x06005168 RID: 20840 RVA: 0x001728F7 File Offset: 0x00170AF7
			internal static void AutoGeneratedStaticCollectObjectsMerchantArmyOfPoachersIssueQuest(object o, List<object> collectedObjects)
			{
				((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005169 RID: 20841 RVA: 0x00172905 File Offset: 0x00170B05
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._poachersParty);
				collectedObjects.Add(this._questVillage);
			}

			// Token: 0x0600516A RID: 20842 RVA: 0x00172926 File Offset: 0x00170B26
			internal static object AutoGeneratedGetMemberValue_poachersParty(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._poachersParty;
			}

			// Token: 0x0600516B RID: 20843 RVA: 0x00172933 File Offset: 0x00170B33
			internal static object AutoGeneratedGetMemberValue_questVillage(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._questVillage;
			}

			// Token: 0x0600516C RID: 20844 RVA: 0x00172940 File Offset: 0x00170B40
			internal static object AutoGeneratedGetMemberValue_talkedToPoachersBattleWillStart(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._talkedToPoachersBattleWillStart;
			}

			// Token: 0x0600516D RID: 20845 RVA: 0x00172952 File Offset: 0x00170B52
			internal static object AutoGeneratedGetMemberValue_isReadyToBeFinalized(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._isReadyToBeFinalized;
			}

			// Token: 0x0600516E RID: 20846 RVA: 0x00172964 File Offset: 0x00170B64
			internal static object AutoGeneratedGetMemberValue_persuasionTriedOnce(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._persuasionTriedOnce;
			}

			// Token: 0x0600516F RID: 20847 RVA: 0x00172976 File Offset: 0x00170B76
			internal static object AutoGeneratedGetMemberValue_difficultyMultiplier(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._difficultyMultiplier;
			}

			// Token: 0x06005170 RID: 20848 RVA: 0x00172988 File Offset: 0x00170B88
			internal static object AutoGeneratedGetMemberValue_rewardGold(object o)
			{
				return ((MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest)o)._rewardGold;
			}

			// Token: 0x17001128 RID: 4392
			// (get) Token: 0x06005171 RID: 20849 RVA: 0x0017299A File Offset: 0x00170B9A
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=iHFo2kjz}Army of Poachers", null);
				}
			}

			// Token: 0x17001129 RID: 4393
			// (get) Token: 0x06005172 RID: 20850 RVA: 0x001729A7 File Offset: 0x00170BA7
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700112A RID: 4394
			// (get) Token: 0x06005173 RID: 20851 RVA: 0x001729AC File Offset: 0x00170BAC
			private TextObject _questStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=fk4ewfQh}{QUEST_GIVER.LINK}, a merchant of {SETTLEMENT}, told you that the poachers {?QUEST_GIVER.GENDER}she{?}he{\\?} hired before are now out of control. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to go to {VILLAGE} around midnight and kill the poachers.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700112B RID: 4395
			// (get) Token: 0x06005174 RID: 20852 RVA: 0x00172A16 File Offset: 0x00170C16
			private TextObject _questCanceledTargetVillageRaided
			{
				get
				{
					TextObject textObject = new TextObject("{=etYq1Tky}{VILLAGE} was raided and the poachers scattered.", null);
					textObject.SetTextVariable("VILLAGE", this._questVillage.Settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700112C RID: 4396
			// (get) Token: 0x06005175 RID: 20853 RVA: 0x00172A40 File Offset: 0x00170C40
			private TextObject _questCanceledWarDeclared
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700112D RID: 4397
			// (get) Token: 0x06005176 RID: 20854 RVA: 0x00172A74 File Offset: 0x00170C74
			private TextObject _playerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700112E RID: 4398
			// (get) Token: 0x06005177 RID: 20855 RVA: 0x00172AA8 File Offset: 0x00170CA8
			private TextObject _questFailedAfterTalkingWithProachers
			{
				get
				{
					TextObject textObject = new TextObject("{=PIukmFYA}You decided not to get involved and left the village. You have failed to help {QUEST_GIVER.LINK} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700112F RID: 4399
			// (get) Token: 0x06005178 RID: 20856 RVA: 0x00172ADA File Offset: 0x00170CDA
			private TextObject _questSuccessPlayerComesToAnAgreementWithPoachers
			{
				get
				{
					return new TextObject("{=qPfJpwGa}You have persuaded the poachers to leave the district.", null);
				}
			}

			// Token: 0x17001130 RID: 4400
			// (get) Token: 0x06005179 RID: 20857 RVA: 0x00172AE8 File Offset: 0x00170CE8
			private TextObject _questFailWithPlayerDefeatedAgainstPoachers
			{
				get
				{
					TextObject textObject = new TextObject("{=p8Kfl5u6}You lost the battle against the poachers and failed to help {QUEST_GIVER.LINK} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001131 RID: 4401
			// (get) Token: 0x0600517A RID: 20858 RVA: 0x00172B1C File Offset: 0x00170D1C
			private TextObject _questSuccessWithPlayerDefeatedPoachers
			{
				get
				{
					TextObject textObject = new TextObject("{=8gNqLqFl}You have defeated the poachers and helped {QUEST_GIVER.LINK} as promised.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001132 RID: 4402
			// (get) Token: 0x0600517B RID: 20859 RVA: 0x00172B4E File Offset: 0x00170D4E
			private TextObject _questFailedWithTimeOutLogText
			{
				get
				{
					return new TextObject("{=HX7E09XJ}You failed to complete the quest in time.", null);
				}
			}

			// Token: 0x0600517C RID: 20860 RVA: 0x00172B5B File Offset: 0x00170D5B
			public MerchantArmyOfPoachersIssueQuest(string questId, Hero giverHero, CampaignTime duration, Village questVillage, float difficultyMultiplier, int rewardGold) : base(questId, giverHero, duration, rewardGold)
			{
				this._questVillage = questVillage;
				this._talkedToPoachersBattleWillStart = false;
				this._isReadyToBeFinalized = false;
				this._difficultyMultiplier = difficultyMultiplier;
				this._rewardGold = rewardGold;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x0600517D RID: 20861 RVA: 0x00172B9C File Offset: 0x00170D9C
			private bool SetStartDialogOnCondition()
			{
				if (this._poachersParty != null && CharacterObject.OneToOneConversationCharacter == ConversationHelper.GetConversationCharacterPartyLeader(this._poachersParty.Party))
				{
					MBTextManager.SetTextVariable("POACHER_PARTY_START_LINE", "{=j9MBwnWI}Well...Are you working for that merchant in the town ? So it's all fine when the rich folk trade in poached skins, but if we do it, armed men come to hunt us down.", false);
					if (this._persuasionTriedOnce)
					{
						MBTextManager.SetTextVariable("POACHER_PARTY_START_LINE", "{=Nn06TSq9}Anything else to say?", false);
					}
					return true;
				}
				return false;
			}

			// Token: 0x0600517E RID: 20862 RVA: 0x00172BF4 File Offset: 0x00170DF4
			private DialogFlow GetPoacherPartyDialogFlow()
			{
				DialogFlow dialogFlow = DialogFlow.CreateDialogFlow("start", 125).NpcLine("{=!}{POACHER_PARTY_START_LINE}", null, null).Condition(() => this.SetStartDialogOnCondition()).Consequence(delegate
				{
					this._task = this.GetPersuasionTask();
				}).BeginPlayerOptions().PlayerOption("{=afbLOXbb}Maybe we can come to an agreement.", null).Condition(() => !this._persuasionTriedOnce).Consequence(delegate
				{
					this._persuasionTriedOnce = true;
				}).GotoDialogState("start_poachers_persuasion").PlayerOption("{=mvw1ayGt}I'm here to do the job I agreed to do, outlaw. Give up or die.", null).NpcLine("{=hOVr77fd}You will never see the sunrise again![ib:warrior][if:convo_furious]", null, null).Consequence(delegate
				{
					this._talkedToPoachersBattleWillStart = true;
				}).CloseDialog().PlayerOption("{=VJYEoOAc}Well... You have a point. Go on. We won't bother you any more.", null).NpcLine("{=wglTyBbx}Thank you, friend. Go in peace.[ib:normal][if:convo_approving]", null, null).Consequence(delegate
				{
					Campaign.Current.GameMenuManager.SetNextMenu("village");
					Campaign.Current.ConversationManager.ConversationEndOneShot += this.QuestFailedAfterTalkingWithPoachers;
				}).CloseDialog().EndPlayerOptions().CloseDialog();
				this.AddPersuasionDialogs(dialogFlow);
				return dialogFlow;
			}

			// Token: 0x0600517F RID: 20863 RVA: 0x00172CE4 File Offset: 0x00170EE4
			private void AddPersuasionDialogs(DialogFlow dialog)
			{
				dialog.AddDialogLine("poachers_persuasion_check_accepted", "start_poachers_persuasion", "poachers_persuasion_start_reservation", "{=6P1ruzsC}Maybe...", new ConversationSentence.OnConditionDelegate(this.persuasion_start_with_poachers_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_start_with_poachers_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("poachers_persuasion_rejected", "poachers_persuasion_start_reservation", "start", "{=!}{FAILED_PERSUASION_LINE}", new ConversationSentence.OnConditionDelegate(this.persuasion_failed_with_poachers_on_condition), new ConversationSentence.OnConsequenceDelegate(this.persuasion_rejected_with_poachers_on_consequence), this, 100, null, null, null);
				dialog.AddDialogLine("poachers_persuasion_attempt", "poachers_persuasion_start_reservation", "poachers_persuasion_select_option", "{=wM77S68a}What's there to discuss?", () => !this.persuasion_failed_with_poachers_on_condition(), null, this, 100, null, null, null);
				dialog.AddDialogLine("poachers_persuasion_success", "poachers_persuasion_start_reservation", "close_window", "{=JQKCPllJ}You've made your point.", new ConversationSentence.OnConditionDelegate(ConversationManager.GetPersuasionProgressSatisfied), new ConversationSentence.OnConsequenceDelegate(this.persuasion_complete_with_poachers_on_consequence), this, 200, null, null, null);
				string id = "poachers_persuasion_select_option_1";
				string inputToken = "poachers_persuasion_select_option";
				string outputToken = "poachers_persuasion_selected_option_response";
				string text = "{=!}{POACHERS_PERSUADE_ATTEMPT_1}";
				ConversationSentence.OnConditionDelegate conditionDelegate = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_1_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_1_on_consequence);
				ConversationSentence.OnPersuasionOptionDelegate persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_1);
				ConversationSentence.OnClickableConditionDelegate clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_1_on_condition);
				dialog.AddPlayerLine(id, inputToken, outputToken, text, conditionDelegate, consequenceDelegate, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				string id2 = "poachers_persuasion_select_option_2";
				string inputToken2 = "poachers_persuasion_select_option";
				string outputToken2 = "poachers_persuasion_selected_option_response";
				string text2 = "{=!}{POACHERS_PERSUADE_ATTEMPT_2}";
				ConversationSentence.OnConditionDelegate conditionDelegate2 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_2_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate2 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_2_on_consequence);
				persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_2);
				clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_2_on_condition);
				dialog.AddPlayerLine(id2, inputToken2, outputToken2, text2, conditionDelegate2, consequenceDelegate2, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				string id3 = "poachers_persuasion_select_option_3";
				string inputToken3 = "poachers_persuasion_select_option";
				string outputToken3 = "poachers_persuasion_selected_option_response";
				string text3 = "{=!}{POACHERS_PERSUADE_ATTEMPT_3}";
				ConversationSentence.OnConditionDelegate conditionDelegate3 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_3_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate3 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_3_on_consequence);
				persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_3);
				clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_3_on_condition);
				dialog.AddPlayerLine(id3, inputToken3, outputToken3, text3, conditionDelegate3, consequenceDelegate3, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				string id4 = "poachers_persuasion_select_option_4";
				string inputToken4 = "poachers_persuasion_select_option";
				string outputToken4 = "poachers_persuasion_selected_option_response";
				string text4 = "{=!}{POACHERS_PERSUADE_ATTEMPT_4}";
				ConversationSentence.OnConditionDelegate conditionDelegate4 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_4_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate4 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_4_on_consequence);
				persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_4);
				clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_4_on_condition);
				dialog.AddPlayerLine(id4, inputToken4, outputToken4, text4, conditionDelegate4, consequenceDelegate4, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				string id5 = "poachers_persuasion_select_option_5";
				string inputToken5 = "poachers_persuasion_select_option";
				string outputToken5 = "poachers_persuasion_selected_option_response";
				string text5 = "{=!}{POACHERS_PERSUADE_ATTEMPT_5}";
				ConversationSentence.OnConditionDelegate conditionDelegate5 = new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_select_option_5_on_condition);
				ConversationSentence.OnConsequenceDelegate consequenceDelegate5 = new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_select_option_5_on_consequence);
				persuasionOptionDelegate = new ConversationSentence.OnPersuasionOptionDelegate(this.poachers_persuasion_setup_option_5);
				clickableConditionDelegate = new ConversationSentence.OnClickableConditionDelegate(this.poachers_persuasion_clickable_option_5_on_condition);
				dialog.AddPlayerLine(id5, inputToken5, outputToken5, text5, conditionDelegate5, consequenceDelegate5, this, 100, clickableConditionDelegate, persuasionOptionDelegate, null, null);
				dialog.AddDialogLine("poachers_persuasion_select_option_reaction", "poachers_persuasion_selected_option_response", "poachers_persuasion_start_reservation", "{=!}{PERSUASION_REACTION}", new ConversationSentence.OnConditionDelegate(this.poachers_persuasion_selected_option_response_on_condition), new ConversationSentence.OnConsequenceDelegate(this.poachers_persuasion_selected_option_response_on_consequence), this, 100, null, null, null);
			}

			// Token: 0x06005180 RID: 20864 RVA: 0x00172FAA File Offset: 0x001711AA
			private void persuasion_start_with_poachers_on_consequence()
			{
				ConversationManager.StartPersuasion(2f, 1f, 0f, 2f, 2f, 0f, PersuasionDifficulty.MediumHard);
			}

			// Token: 0x06005181 RID: 20865 RVA: 0x00172FD0 File Offset: 0x001711D0
			private bool persuasion_start_with_poachers_on_condition()
			{
				return this._poachersParty != null && CharacterObject.OneToOneConversationCharacter == ConversationHelper.GetConversationCharacterPartyLeader(this._poachersParty.Party);
			}

			// Token: 0x06005182 RID: 20866 RVA: 0x00172FF4 File Offset: 0x001711F4
			private PersuasionTask GetPersuasionTask()
			{
				PersuasionTask persuasionTask = new PersuasionTask(0);
				persuasionTask.FinalFailLine = new TextObject("{=l7Jt5tvt}This is how I earn my living, and all your clever talk doesn't make it any different. Leave now!", null);
				persuasionTask.TryLaterLine = new TextObject("{=!}TODO", null);
				persuasionTask.SpokenLine = new TextObject("{=wM77S68a}What's there to discuss?", null);
				PersuasionOptionArgs option = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Easy, false, new TextObject("{=cQCs72U7}You're not bad people. You can easily ply your trade somewhere else, somewhere safe.", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option);
				PersuasionOptionArgs option2 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Valor, TraitEffect.Positive, PersuasionArgumentStrength.ExtremelyHard, true, new TextObject("{=bioyMrUD}You are just a bunch of hunters. You don't stand a chance against us!", null), null, true, false, false);
				persuasionTask.AddOptionToTask(option2);
				PersuasionOptionArgs option3 = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Mercy, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, new TextObject("{=FO1oruNy}You talk about poor folk, but you think the people here like their village turned into a nest of outlaws?", null), null, false, false, false);
				persuasionTask.AddOptionToTask(option3);
				TextObject textObject = new TextObject("{=S0NeQdLp}You had an agreement with {QUEST_GIVER.NAME}. Your word is your bond, no matter which side of the law you're on.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				PersuasionOptionArgs option4 = new PersuasionOptionArgs(DefaultSkills.Charm, DefaultTraits.Honor, TraitEffect.Positive, PersuasionArgumentStrength.Normal, false, textObject, null, false, false, false);
				persuasionTask.AddOptionToTask(option4);
				TextObject line = new TextObject("{=brW4pjPQ}Flee while you can. An army is already on its way here to hang you all.", null);
				PersuasionOptionArgs option5 = new PersuasionOptionArgs(DefaultSkills.Roguery, DefaultTraits.Calculating, TraitEffect.Positive, PersuasionArgumentStrength.Hard, true, line, null, false, false, false);
				persuasionTask.AddOptionToTask(option5);
				return persuasionTask;
			}

			// Token: 0x06005183 RID: 20867 RVA: 0x0017312C File Offset: 0x0017132C
			private bool poachers_persuasion_selected_option_response_on_condition()
			{
				PersuasionOptionResult item = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>().Item2;
				MBTextManager.SetTextVariable("PERSUASION_REACTION", PersuasionHelper.GetDefaultPersuasionOptionReaction(item), false);
				if (item == PersuasionOptionResult.CriticalFailure)
				{
					this._task.BlockAllOptions();
				}
				return true;
			}

			// Token: 0x06005184 RID: 20868 RVA: 0x0017316C File Offset: 0x0017136C
			private void poachers_persuasion_selected_option_response_on_consequence()
			{
				Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = ConversationManager.GetPersuasionChosenOptions().Last<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
				float difficulty = Campaign.Current.Models.PersuasionModel.GetDifficulty(PersuasionDifficulty.MediumHard);
				float moveToNextStageChance;
				float blockRandomOptionChance;
				Campaign.Current.Models.PersuasionModel.GetEffectChances(tuple.Item1, out moveToNextStageChance, out blockRandomOptionChance, difficulty);
				this._task.ApplyEffects(moveToNextStageChance, blockRandomOptionChance);
			}

			// Token: 0x06005185 RID: 20869 RVA: 0x001731C8 File Offset: 0x001713C8
			private bool poachers_persuasion_select_option_1_on_condition()
			{
				if (this._task.Options.Count > 0)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt(0), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt(0).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_1", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005186 RID: 20870 RVA: 0x00173248 File Offset: 0x00171448
			private bool poachers_persuasion_select_option_2_on_condition()
			{
				if (this._task.Options.Count > 1)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt(1), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt(1).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_2", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005187 RID: 20871 RVA: 0x001732C8 File Offset: 0x001714C8
			private bool poachers_persuasion_select_option_3_on_condition()
			{
				if (this._task.Options.Count > 2)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt(2), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt(2).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_3", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005188 RID: 20872 RVA: 0x00173348 File Offset: 0x00171548
			private bool poachers_persuasion_select_option_4_on_condition()
			{
				if (this._task.Options.Count > 3)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt(3), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt(3).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_4", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005189 RID: 20873 RVA: 0x001733C8 File Offset: 0x001715C8
			private bool poachers_persuasion_select_option_5_on_condition()
			{
				if (this._task.Options.Count > 4)
				{
					TextObject textObject = new TextObject("{=bSo9hKwr}{PERSUASION_OPTION_LINE} {SUCCESS_CHANCE}", null);
					textObject.SetTextVariable("SUCCESS_CHANCE", PersuasionHelper.ShowSuccess(this._task.Options.ElementAt(4), false));
					textObject.SetTextVariable("PERSUASION_OPTION_LINE", this._task.Options.ElementAt(4).Line);
					MBTextManager.SetTextVariable("POACHERS_PERSUADE_ATTEMPT_5", textObject, false);
					return true;
				}
				return false;
			}

			// Token: 0x0600518A RID: 20874 RVA: 0x00173448 File Offset: 0x00171648
			private void poachers_persuasion_select_option_1_on_consequence()
			{
				if (this._task.Options.Count > 0)
				{
					this._task.Options[0].BlockTheOption(true);
				}
			}

			// Token: 0x0600518B RID: 20875 RVA: 0x00173474 File Offset: 0x00171674
			private void poachers_persuasion_select_option_2_on_consequence()
			{
				if (this._task.Options.Count > 1)
				{
					this._task.Options[1].BlockTheOption(true);
				}
			}

			// Token: 0x0600518C RID: 20876 RVA: 0x001734A0 File Offset: 0x001716A0
			private void poachers_persuasion_select_option_3_on_consequence()
			{
				if (this._task.Options.Count > 2)
				{
					this._task.Options[2].BlockTheOption(true);
				}
			}

			// Token: 0x0600518D RID: 20877 RVA: 0x001734CC File Offset: 0x001716CC
			private void poachers_persuasion_select_option_4_on_consequence()
			{
				if (this._task.Options.Count > 3)
				{
					this._task.Options[3].BlockTheOption(true);
				}
			}

			// Token: 0x0600518E RID: 20878 RVA: 0x001734F8 File Offset: 0x001716F8
			private void poachers_persuasion_select_option_5_on_consequence()
			{
				if (this._task.Options.Count > 4)
				{
					this._task.Options[4].BlockTheOption(true);
				}
			}

			// Token: 0x0600518F RID: 20879 RVA: 0x00173524 File Offset: 0x00171724
			private bool persuasion_failed_with_poachers_on_condition()
			{
				if (this._task.Options.All((PersuasionOptionArgs x) => x.IsBlocked) && !ConversationManager.GetPersuasionProgressSatisfied())
				{
					MBTextManager.SetTextVariable("FAILED_PERSUASION_LINE", this._task.FinalFailLine, false);
					return true;
				}
				return false;
			}

			// Token: 0x06005190 RID: 20880 RVA: 0x00173582 File Offset: 0x00171782
			private PersuasionOptionArgs poachers_persuasion_setup_option_1()
			{
				return this._task.Options.ElementAt(0);
			}

			// Token: 0x06005191 RID: 20881 RVA: 0x00173595 File Offset: 0x00171795
			private PersuasionOptionArgs poachers_persuasion_setup_option_2()
			{
				return this._task.Options.ElementAt(1);
			}

			// Token: 0x06005192 RID: 20882 RVA: 0x001735A8 File Offset: 0x001717A8
			private PersuasionOptionArgs poachers_persuasion_setup_option_3()
			{
				return this._task.Options.ElementAt(2);
			}

			// Token: 0x06005193 RID: 20883 RVA: 0x001735BB File Offset: 0x001717BB
			private PersuasionOptionArgs poachers_persuasion_setup_option_4()
			{
				return this._task.Options.ElementAt(3);
			}

			// Token: 0x06005194 RID: 20884 RVA: 0x001735CE File Offset: 0x001717CE
			private PersuasionOptionArgs poachers_persuasion_setup_option_5()
			{
				return this._task.Options.ElementAt(4);
			}

			// Token: 0x06005195 RID: 20885 RVA: 0x001735E4 File Offset: 0x001717E4
			private bool poachers_persuasion_clickable_option_1_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 0)
				{
					hintText = (this._task.Options.ElementAt(0).IsBlocked ? hintText : TextObject.Empty);
					return !this._task.Options.ElementAt(0).IsBlocked;
				}
				return false;
			}

			// Token: 0x06005196 RID: 20886 RVA: 0x00173650 File Offset: 0x00171850
			private bool poachers_persuasion_clickable_option_2_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 1)
				{
					hintText = (this._task.Options.ElementAt(1).IsBlocked ? hintText : TextObject.Empty);
					return !this._task.Options.ElementAt(1).IsBlocked;
				}
				return false;
			}

			// Token: 0x06005197 RID: 20887 RVA: 0x001736BC File Offset: 0x001718BC
			private bool poachers_persuasion_clickable_option_3_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 2)
				{
					hintText = (this._task.Options.ElementAt(2).IsBlocked ? hintText : TextObject.Empty);
					return !this._task.Options.ElementAt(2).IsBlocked;
				}
				return false;
			}

			// Token: 0x06005198 RID: 20888 RVA: 0x00173728 File Offset: 0x00171928
			private bool poachers_persuasion_clickable_option_4_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 3)
				{
					hintText = (this._task.Options.ElementAt(3).IsBlocked ? hintText : TextObject.Empty);
					return !this._task.Options.ElementAt(3).IsBlocked;
				}
				return false;
			}

			// Token: 0x06005199 RID: 20889 RVA: 0x00173794 File Offset: 0x00171994
			private bool poachers_persuasion_clickable_option_5_on_condition(out TextObject hintText)
			{
				hintText = new TextObject("{=9ACJsI6S}Blocked", null);
				if (this._task.Options.Count > 4)
				{
					hintText = (this._task.Options.ElementAt(4).IsBlocked ? hintText : TextObject.Empty);
					return !this._task.Options.ElementAt(4).IsBlocked;
				}
				return false;
			}

			// Token: 0x0600519A RID: 20890 RVA: 0x001737FF File Offset: 0x001719FF
			private void persuasion_rejected_with_poachers_on_consequence()
			{
				PlayerEncounter.LeaveEncounter = false;
				ConversationManager.EndPersuasion();
			}

			// Token: 0x0600519B RID: 20891 RVA: 0x0017380C File Offset: 0x00171A0C
			private void persuasion_complete_with_poachers_on_consequence()
			{
				PlayerEncounter.LeaveEncounter = true;
				ConversationManager.EndPersuasion();
				Campaign.Current.GameMenuManager.SetNextMenu("village");
				Campaign.Current.ConversationManager.ConversationEndOneShot += this.QuestSuccessPlayerComesToAnAgreementWithPoachers;
			}

			// Token: 0x0600519C RID: 20892 RVA: 0x00173848 File Offset: 0x00171A48
			internal void StartQuestBattle()
			{
				PlayerEncounter.RestartPlayerEncounter(PartyBase.MainParty, this._poachersParty.Party, false);
				PlayerEncounter.StartBattle();
				PlayerEncounter.Update();
				this._talkedToPoachersBattleWillStart = false;
				GameMenu.ActivateGameMenu("army_of_poachers_village");
				CampaignMission.OpenBattleMission(this._questVillage.Settlement.LocationComplex.GetScene("village_center", 1), false);
				this._isReadyToBeFinalized = true;
			}

			// Token: 0x0600519D RID: 20893 RVA: 0x001738B0 File Offset: 0x00171AB0
			private bool DialogCondition()
			{
				return Hero.OneToOneConversationHero == base.QuestGiver;
			}

			// Token: 0x0600519E RID: 20894 RVA: 0x001738C0 File Offset: 0x00171AC0
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=IefM6uAy}Thank you. You'll be paid well. Also you can keep their illegally obtained leather.[ib:normal2][if:convo_bemused]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition)).NpcLine(new TextObject("{=NC2VGafO}They skin their beasts in the woods, then go into the village after midnight to stash the hides. The villagers are terrified of them, I believe. If you go into the village late at night, you should be able to track them down.[ib:normal][if:convo_thinking]", null), null, null).NpcLine(new TextObject("{=3pkVKMnA}Most poachers would probably run if they were surprised by armed men. But these ones are bold and desperate. Be ready for a fight.[ib:normal2][if:convo_undecided_closed]", null), null, null).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=QNV1b5s5}Are those poachers still in business?[ib:normal2][if:convo_undecided_open]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition)).BeginPlayerOptions().PlayerOption(new TextObject("{=JhJBBWab}They will be gone soon.", null), null).NpcLine(new TextObject("{=gjGb044I}I hope they will be...[ib:normal2][if:convo_dismayed]", null), null, null).CloseDialog().PlayerOption(new TextObject("{=Gu3jF88V}Any night battle can easily go wrong. I need more time to prepare.", null), null).NpcLine(new TextObject("{=2EiC1YyZ}Well, if they get wind of what you're up to, things could go very wrong for me. Do be quick.[ib:nervous2][if:convo_dismayed]", null), null, null).CloseDialog().EndPlayerOptions();
				this.QuestCharacterDialogFlow = this.GetPoacherPartyDialogFlow();
			}

			// Token: 0x0600519F RID: 20895 RVA: 0x001739D8 File Offset: 0x00171BD8
			internal void CreatePoachersParty()
			{
				this._poachersParty = MobileParty.CreateParty("poachers_party", null, null);
				TextObject customName = new TextObject("{=WQa1R55u}Poachers Party", null);
				this._poachersParty.InitializeMobilePartyAtPosition(new TroopRoster(this._poachersParty.Party), new TroopRoster(this._poachersParty.Party), this._questVillage.Settlement.GetPosition2D);
				this._poachersParty.SetCustomName(customName);
				ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("leather");
				int num = MathF.Ceiling(this._difficultyMultiplier * 5f) + MBRandom.RandomInt(0, 2);
				this._poachersParty.ItemRoster.AddToCounts(@object, num * 2);
				CharacterObject character = CharacterObject.All.FirstOrDefault((CharacterObject t) => t.StringId == "poacher");
				int count = 10 + MathF.Ceiling(40f * this._difficultyMultiplier);
				this._poachersParty.MemberRoster.AddToCounts(character, count, false, 0, 0, true, -1);
				this._poachersParty.SetPartyUsedByQuest(true);
				this._poachersParty.Ai.DisableAi();
				Settlement closestHideout = SettlementHelper.FindNearestHideout((Settlement x) => x.IsActive, null);
				Clan actualClan = Clan.BanditFactions.FirstOrDefaultQ((Clan t) => t.Culture == closestHideout.Culture);
				this._poachersParty.ActualClan = actualClan;
				EnterSettlementAction.ApplyForParty(this._poachersParty, Settlement.CurrentSettlement);
			}

			// Token: 0x060051A0 RID: 20896 RVA: 0x00173B66 File Offset: 0x00171D66
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				base.AddLog(this._questStartedLogText, false);
				base.AddTrackedObject(this._questVillage.Settlement);
			}

			// Token: 0x060051A1 RID: 20897 RVA: 0x00173B90 File Offset: 0x00171D90
			internal void QuestFailedAfterTalkingWithPoachers()
			{
				base.AddLog(this._questFailedAfterTalkingWithProachers, false);
				TraitLevelingHelper.OnIssueFailed(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -50),
					new Tuple<TraitObject, int>(DefaultTraits.Mercy, 20)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-50f);
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 30f;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060051A2 RID: 20898 RVA: 0x00173C3C File Offset: 0x00171E3C
			internal void QuestSuccessPlayerComesToAnAgreementWithPoachers()
			{
				base.AddLog(this._questSuccessPlayerComesToAnAgreementWithPoachers, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 10),
					new Tuple<TraitObject, int>(DefaultTraits.Mercy, 50)
				});
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				this.RelationshipChangeWithQuestGiver = 5;
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				base.QuestGiver.AddPower(30f);
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
				base.QuestGiver.CurrentSettlement.Town.Prosperity += 50f;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060051A3 RID: 20899 RVA: 0x00173D08 File Offset: 0x00171F08
			internal void QuestFailWithPlayerDefeatedAgainstPoachers()
			{
				base.AddLog(this._questFailWithPlayerDefeatedAgainstPoachers, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-50f);
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 30f;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060051A4 RID: 20900 RVA: 0x00173DA4 File Offset: 0x00171FA4
			internal void QuestSuccessWithPlayerDefeatedPoachers()
			{
				base.AddLog(this._questSuccessWithPlayerDefeatedPoachers, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				this.RelationshipChangeWithQuestGiver = 5;
				base.QuestGiver.AddPower(30f);
				base.QuestGiver.CurrentSettlement.Town.Prosperity += 50f;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060051A5 RID: 20901 RVA: 0x00173E30 File Offset: 0x00172030
			protected override void OnTimedOut()
			{
				base.AddLog(this._questFailedWithTimeOutLogText, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-50f);
				base.QuestGiver.CurrentSettlement.Town.Prosperity -= 30f;
				base.QuestGiver.CurrentSettlement.Town.Security -= 5f;
			}

			// Token: 0x060051A6 RID: 20902 RVA: 0x00173EC5 File Offset: 0x001720C5
			private void QuestCanceledTargetVillageRaided()
			{
				base.AddLog(this._questCanceledTargetVillageRaided, false);
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x060051A7 RID: 20903 RVA: 0x00173EDC File Offset: 0x001720DC
			protected override void RegisterEvents()
			{
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventCheck));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.MapEventStarted));
				CampaignEvents.GameMenuOpened.AddNonSerializedListener(this, new Action<MenuCallbackArgs>(this.GameMenuOpened));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.CanHeroBecomePrisonerEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.OnCanHeroBecomePrisonerInfoIsRequested));
			}

			// Token: 0x060051A8 RID: 20904 RVA: 0x00173F73 File Offset: 0x00172173
			private void OnCanHeroBecomePrisonerInfoIsRequested(Hero hero, ref bool result)
			{
				if (hero == Hero.MainHero && this._isReadyToBeFinalized)
				{
					result = false;
				}
			}

			// Token: 0x060051A9 RID: 20905 RVA: 0x00173F88 File Offset: 0x00172188
			protected override void HourlyTick()
			{
				if (PlayerEncounter.Current != null && PlayerEncounter.Current.IsPlayerWaiting && PlayerEncounter.EncounterSettlement == this._questVillage.Settlement && CampaignTime.Now.IsNightTime && !this._isReadyToBeFinalized && base.IsOngoing)
				{
					EnterSettlementAction.ApplyForParty(MobileParty.MainParty, this._questVillage.Settlement);
					GameMenu.SwitchToMenu("army_of_poachers_village");
				}
			}

			// Token: 0x060051AA RID: 20906 RVA: 0x00173FF8 File Offset: 0x001721F8
			private void GameMenuOpened(MenuCallbackArgs obj)
			{
				if (obj.MenuContext.GameMenu.StringId == "village" && CampaignTime.Now.IsNightTime && Settlement.CurrentSettlement == this._questVillage.Settlement && !this._isReadyToBeFinalized)
				{
					GameMenu.SwitchToMenu("army_of_poachers_village");
				}
				if (obj.MenuContext.GameMenu.StringId == "army_of_poachers_village" && this._isReadyToBeFinalized && MapEvent.PlayerMapEvent != null && MapEvent.PlayerMapEvent.HasWinner && this._poachersParty != null)
				{
					this._poachersParty.IsVisible = false;
				}
			}

			// Token: 0x060051AB RID: 20907 RVA: 0x0017409F File Offset: 0x0017229F
			private void MapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				this.MapEventCheck(mapEvent);
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x060051AC RID: 20908 RVA: 0x001740B9 File Offset: 0x001722B9
			private void MapEventCheck(MapEvent mapEvent)
			{
				if (mapEvent.IsRaid && mapEvent.MapEventSettlement == this._questVillage.Settlement)
				{
					this.QuestCanceledTargetVillageRaided();
				}
			}

			// Token: 0x060051AD RID: 20909 RVA: 0x001740DC File Offset: 0x001722DC
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this._questCanceledWarDeclared);
				}
			}

			// Token: 0x060051AE RID: 20910 RVA: 0x0017410B File Offset: 0x0017230B
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this._playerDeclaredWarQuestLogText, this._questCanceledWarDeclared, false);
			}

			// Token: 0x060051AF RID: 20911 RVA: 0x00174123 File Offset: 0x00172323
			protected override void OnFinalize()
			{
				if (this._poachersParty != null && this._poachersParty.IsActive)
				{
					DestroyPartyAction.Apply(null, this._poachersParty);
				}
				if (Hero.MainHero.IsPrisoner)
				{
					EndCaptivityAction.ApplyByPeace(Hero.MainHero, null);
				}
			}

			// Token: 0x060051B0 RID: 20912 RVA: 0x0017415D File Offset: 0x0017235D
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x04001AA6 RID: 6822
			[SaveableField(10)]
			internal MobileParty _poachersParty;

			// Token: 0x04001AA7 RID: 6823
			[SaveableField(20)]
			internal Village _questVillage;

			// Token: 0x04001AA8 RID: 6824
			[SaveableField(30)]
			internal bool _talkedToPoachersBattleWillStart;

			// Token: 0x04001AA9 RID: 6825
			[SaveableField(40)]
			internal bool _isReadyToBeFinalized;

			// Token: 0x04001AAA RID: 6826
			[SaveableField(50)]
			internal bool _persuasionTriedOnce;

			// Token: 0x04001AAB RID: 6827
			[SaveableField(60)]
			internal float _difficultyMultiplier;

			// Token: 0x04001AAC RID: 6828
			[SaveableField(70)]
			internal int _rewardGold;

			// Token: 0x04001AAD RID: 6829
			private PersuasionTask _task;

			// Token: 0x04001AAE RID: 6830
			private const PersuasionDifficulty Difficulty = PersuasionDifficulty.MediumHard;
		}

		// Token: 0x02000659 RID: 1625
		public class MerchantArmyOfPoachersIssueBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060051B8 RID: 20920 RVA: 0x001741D4 File Offset: 0x001723D4
			public MerchantArmyOfPoachersIssueBehaviorTypeDefiner() : base(800000)
			{
			}

			// Token: 0x060051B9 RID: 20921 RVA: 0x001741E1 File Offset: 0x001723E1
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssue), 1, null);
				base.AddClassDefinition(typeof(MerchantArmyOfPoachersIssueBehavior.MerchantArmyOfPoachersIssueQuest), 2, null);
			}
		}
	}
}
