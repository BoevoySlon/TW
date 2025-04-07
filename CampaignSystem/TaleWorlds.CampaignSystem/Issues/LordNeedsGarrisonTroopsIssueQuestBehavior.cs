using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000312 RID: 786
	public class LordNeedsGarrisonTroopsIssueQuestBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06002D68 RID: 11624 RVA: 0x000BDF58 File Offset: 0x000BC158
		private static LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest Instance
		{
			get
			{
				LordNeedsGarrisonTroopsIssueQuestBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<LordNeedsGarrisonTroopsIssueQuestBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest cachedQuest;
						if ((cachedQuest = (enumerator.Current as LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)) != null)
						{
							campaignBehavior._cachedQuest = cachedQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x000BDFF0 File Offset: 0x000BC1F0
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000BE020 File Offset: 0x000BC220
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			string optionText = "{=FirEOQaI}Talk to the garrison commander";
			gameStarter.AddGameMenuOption("town", "talk_to_garrison_commander_town", optionText, new GameMenuOption.OnConditionDelegate(this.talk_to_garrison_commander_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_garrison_commander_on_consequence), false, 2, false, null);
			gameStarter.AddGameMenuOption("town_guard", "talk_to_garrison_commander_town", optionText, new GameMenuOption.OnConditionDelegate(this.talk_to_garrison_commander_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_garrison_commander_on_consequence), false, 2, false, null);
			gameStarter.AddGameMenuOption("castle_guard", "talk_to_garrison_commander_castle", optionText, new GameMenuOption.OnConditionDelegate(this.talk_to_garrison_commander_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_garrison_commander_on_consequence), false, 2, false, null);
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x000BE0BC File Offset: 0x000BC2BC
		private bool talk_to_garrison_commander_on_condition(MenuCallbackArgs args)
		{
			if (LordNeedsGarrisonTroopsIssueQuestBehavior.Instance != null)
			{
				if (Settlement.CurrentSettlement == LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement)
				{
					Town town = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.Town;
					if (((town != null) ? town.GarrisonParty : null) == null)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=JmoOJX4e}There is no one in the garrison to receive the troops requested. You should wait until someone arrives.", null);
					}
				}
				args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
				args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
				return Settlement.CurrentSettlement == LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement;
			}
			return false;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x000BE138 File Offset: 0x000BC338
		private void talk_to_garrison_commander_on_consequence(MenuCallbackArgs args)
		{
			CharacterObject characterObject = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.OwnerClan.Culture.EliteBasicTroop;
			foreach (TroopRosterElement troopRosterElement in LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsInfantry && characterObject.Level < troopRosterElement.Character.Level)
				{
					characterObject = troopRosterElement.Character;
				}
			}
			LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._selectedCharacterToTalk = characterObject;
			ConversationCharacterData playerCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			CharacterObject selectedCharacterToTalk = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._selectedCharacterToTalk;
			Town town = LordNeedsGarrisonTroopsIssueQuestBehavior.Instance._settlement.Town;
			CampaignMapConversation.OpenConversation(playerCharacterData, new ConversationCharacterData(selectedCharacterToTalk, (town != null) ? town.GarrisonParty.Party : null, false, false, false, false, false, false));
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000BE238 File Offset: 0x000BC438
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000BE23C File Offset: 0x000BC43C
		private bool ConditionsHold(Hero issueGiver, out Settlement selectedSettlement)
		{
			selectedSettlement = null;
			if (issueGiver.IsLord && issueGiver.Clan.Leader == issueGiver && !issueGiver.IsMinorFactionHero && issueGiver.Clan != Clan.PlayerClan)
			{
				foreach (Settlement settlement in issueGiver.Clan.Settlements)
				{
					if (settlement.IsCastle)
					{
						MobileParty garrisonParty = settlement.Town.GarrisonParty;
						if (garrisonParty != null && garrisonParty.MemberRoster.TotalHealthyCount < 120)
						{
							selectedSettlement = settlement;
							break;
						}
					}
					if (settlement.IsTown)
					{
						MobileParty garrisonParty2 = settlement.Town.GarrisonParty;
						if (garrisonParty2 != null && garrisonParty2.MemberRoster.TotalHealthyCount < 150)
						{
							selectedSettlement = settlement;
							break;
						}
					}
				}
				return selectedSettlement != null;
			}
			return false;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000BE330 File Offset: 0x000BC530
		public void OnCheckForIssue(Hero hero)
		{
			Settlement relatedObject;
			if (this.ConditionsHold(hero, out relatedObject))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue), IssueBase.IssueFrequency.Common, relatedObject));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue), IssueBase.IssueFrequency.Common));
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000BE398 File Offset: 0x000BC598
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue(issueOwner, potentialIssueData.RelatedObject as Settlement);
		}

		// Token: 0x04000D97 RID: 3479
		private const IssueBase.IssueFrequency LordNeedsGarrisonTroopsIssueFrequency = IssueBase.IssueFrequency.Common;

		// Token: 0x04000D98 RID: 3480
		private LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest _cachedQuest;

		// Token: 0x0200064A RID: 1610
		public class LordNeedsGarrisonTroopsIssue : IssueBase
		{
			// Token: 0x06004FF2 RID: 20466 RVA: 0x0016CD41 File Offset: 0x0016AF41
			internal static void AutoGeneratedStaticCollectObjectsLordNeedsGarrisonTroopsIssue(object o, List<object> collectedObjects)
			{
				((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004FF3 RID: 20467 RVA: 0x0016CD4F File Offset: 0x0016AF4F
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._settlement);
				collectedObjects.Add(this._neededTroopType);
			}

			// Token: 0x06004FF4 RID: 20468 RVA: 0x0016CD70 File Offset: 0x0016AF70
			internal static object AutoGeneratedGetMemberValue_settlement(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue)o)._settlement;
			}

			// Token: 0x06004FF5 RID: 20469 RVA: 0x0016CD7D File Offset: 0x0016AF7D
			internal static object AutoGeneratedGetMemberValue_neededTroopType(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue)o)._neededTroopType;
			}

			// Token: 0x170010AE RID: 4270
			// (get) Token: 0x06004FF6 RID: 20470 RVA: 0x0016CD8A File Offset: 0x0016AF8A
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x170010AF RID: 4271
			// (get) Token: 0x06004FF7 RID: 20471 RVA: 0x0016CD8D File Offset: 0x0016AF8D
			private int NumberOfTroopToBeRecruited
			{
				get
				{
					return 3 + (int)(base.IssueDifficultyMultiplier * 18f);
				}
			}

			// Token: 0x170010B0 RID: 4272
			// (get) Token: 0x06004FF8 RID: 20472 RVA: 0x0016CD9E File Offset: 0x0016AF9E
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 5 + MathF.Ceiling(8f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170010B1 RID: 4273
			// (get) Token: 0x06004FF9 RID: 20473 RVA: 0x0016CDB3 File Offset: 0x0016AFB3
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 3 + MathF.Ceiling(4f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170010B2 RID: 4274
			// (get) Token: 0x06004FFA RID: 20474 RVA: 0x0016CDC8 File Offset: 0x0016AFC8
			protected override int RewardGold
			{
				get
				{
					int num = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this._neededTroopType, Hero.MainHero, false) * this.NumberOfTroopToBeRecruited;
					return (int)(1500f + (float)num * 1.5f);
				}
			}

			// Token: 0x170010B3 RID: 4275
			// (get) Token: 0x06004FFB RID: 20475 RVA: 0x0016CE0C File Offset: 0x0016B00C
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=ZuTvTGsh}These wars have taken a toll on my men. The bravest often fall first, they say, and fewer and fewer families are willing to let their sons join my banner. But the wars don't stop because I have problems.[if:convo_undecided_closed][ib:closed]", null);
				}
			}

			// Token: 0x170010B4 RID: 4276
			// (get) Token: 0x06004FFC RID: 20476 RVA: 0x0016CE1C File Offset: 0x0016B01C
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=tTM6nPul}What can I do for you, {?ISSUE_OWNER.GENDER}madam{?}sir{\\?}?", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170010B5 RID: 4277
			// (get) Token: 0x06004FFD RID: 20477 RVA: 0x0016CE50 File Offset: 0x0016B050
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=driH06vI}I need more recruits in {SETTLEMENT}'s garrison. Since I'll be elsewhere... maybe you can recruit {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} and bring them to the garrison for me?[if:convo_undecided_open][ib:normal]", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					textObject.SetTextVariable("TROOP_TYPE", this._neededTroopType.EncyclopediaLinkWithName);
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this.NumberOfTroopToBeRecruited);
					return textObject;
				}
			}

			// Token: 0x170010B6 RID: 4278
			// (get) Token: 0x06004FFE RID: 20478 RVA: 0x0016CEA8 File Offset: 0x0016B0A8
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=igXcCqdo}One of your trusted companions who knows how to lead men can go around with {ALTERNATIVE_SOLUTION_MAN_COUNT} horsemen and pick some up. One way or the other I will pay {REWARD_GOLD}{GOLD_ICON} denars in return for your services. What do you say?[if:convo_thinking]", null);
					textObject.SetTextVariable("ALTERNATIVE_SOLUTION_MAN_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("REWARD_GOLD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					return textObject;
				}
			}

			// Token: 0x170010B7 RID: 4279
			// (get) Token: 0x06004FFF RID: 20479 RVA: 0x0016CEF5 File Offset: 0x0016B0F5
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=YHSm72Ln}I'll find your recruits and bring them to {SETTLEMENT} garrison.", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170010B8 RID: 4280
			// (get) Token: 0x06005000 RID: 20480 RVA: 0x0016CF1C File Offset: 0x0016B11C
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=JPclWyyr}My companion can handle it... So, {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} to {SETTLEMENT}.", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					textObject.SetTextVariable("TROOP_TYPE", this._neededTroopType.EncyclopediaLinkWithName);
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this.NumberOfTroopToBeRecruited);
					return textObject;
				}
			}

			// Token: 0x170010B9 RID: 4281
			// (get) Token: 0x06005001 RID: 20481 RVA: 0x0016CF74 File Offset: 0x0016B174
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					TextObject textObject = new TextObject("{=lWrmxsYR}I haven't heard any news from {SETTLEMENT}, but I realize it might take some time for your men to deliver the recruits.", null);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170010BA RID: 4282
			// (get) Token: 0x06005002 RID: 20482 RVA: 0x0016CF98 File Offset: 0x0016B198
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=WUWzyzWI}Thank you. Your help will be remembered.", null);
				}
			}

			// Token: 0x170010BB RID: 4283
			// (get) Token: 0x06005003 RID: 20483 RVA: 0x0016CFA8 File Offset: 0x0016B1A8
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=M560TDza}{ISSUE_OWNER.LINK}, the {?ISSUE_OWNER.GENDER}lady{?}lord{\\?} of {QUEST_SETTLEMENT}, told you that {?ISSUE_OWNER.GENDER}she{?}he{\\?} needs more troops in {?ISSUE_OWNER.GENDER}her{?}his{\\?} garrison. {?ISSUE_OWNER.GENDER}She{?}He{\\?} is willing to pay {REWARD}{GOLD_ICON} for your services. You asked your companion to deploy {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} troops to {QUEST_SETTLEMENT}'s garrison.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("TROOP_TYPE", this._neededTroopType.EncyclopediaLinkWithName);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this.NumberOfTroopToBeRecruited);
					return textObject;
				}
			}

			// Token: 0x170010BC RID: 4284
			// (get) Token: 0x06005004 RID: 20484 RVA: 0x0016D03D File Offset: 0x0016B23D
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170010BD RID: 4285
			// (get) Token: 0x06005005 RID: 20485 RVA: 0x0016D040 File Offset: 0x0016B240
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170010BE RID: 4286
			// (get) Token: 0x06005006 RID: 20486 RVA: 0x0016D044 File Offset: 0x0016B244
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=g6Ra6LUY}{ISSUE_OWNER.NAME} needs garrison troops in {SETTLEMENT}", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170010BF RID: 4287
			// (get) Token: 0x06005007 RID: 20487 RVA: 0x0016D090 File Offset: 0x0016B290
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=BOAaF6x5}{ISSUE_OWNER.NAME} asks for help to increase troop levels in {SETTLEMENT}", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170010C0 RID: 4288
			// (get) Token: 0x06005008 RID: 20488 RVA: 0x0016D0DC File Offset: 0x0016B2DC
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=sfFkYm0a}Your companion has successfully brought the troops {ISSUE_OWNER.LINK} requested. You received {REWARD}{GOLD_ICON}.", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					return textObject;
				}
			}

			// Token: 0x06005009 RID: 20489 RVA: 0x0016D134 File Offset: 0x0016B334
			public LordNeedsGarrisonTroopsIssue(Hero issueOwner, Settlement selectedSettlement) : base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._settlement = selectedSettlement;
				this._neededTroopType = CharacterHelper.GetTroopTree(base.IssueOwner.Culture.BasicTroop, 3f, 3f).GetRandomElementInefficiently<CharacterObject>();
			}

			// Token: 0x0600500A RID: 20490 RVA: 0x0016D183 File Offset: 0x0016B383
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -0.5f;
				}
				return 0f;
			}

			// Token: 0x0600500B RID: 20491 RVA: 0x0016D198 File Offset: 0x0016B398
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>((hero.GetSkillValue(DefaultSkills.Leadership) >= hero.GetSkillValue(DefaultSkills.Steward)) ? DefaultSkills.Leadership : DefaultSkills.Steward, 120);
			}

			// Token: 0x0600500C RID: 20492 RVA: 0x0016D1C5 File Offset: 0x0016B3C5
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 0, true);
			}

			// Token: 0x0600500D RID: 20493 RVA: 0x0016D1DD File Offset: 0x0016B3DD
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.IsMounted;
			}

			// Token: 0x0600500E RID: 20494 RVA: 0x0016D1E5 File Offset: 0x0016B3E5
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 0, true);
			}

			// Token: 0x170010C1 RID: 4289
			// (get) Token: 0x0600500F RID: 20495 RVA: 0x0016D206 File Offset: 0x0016B406
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(800f + 900f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06005010 RID: 20496 RVA: 0x0016D21B File Offset: 0x0016B41B
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				this.RelationshipChangeWithIssueOwner = 2;
			}

			// Token: 0x06005011 RID: 20497 RVA: 0x0016D234 File Offset: 0x0016B434
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Common;
			}

			// Token: 0x06005012 RID: 20498 RVA: 0x0016D238 File Offset: 0x0016B438
			public override bool IssueStayAliveConditions()
			{
				bool flag = false;
				if (this._settlement.IsTown)
				{
					MobileParty garrisonParty = this._settlement.Town.GarrisonParty;
					flag = (garrisonParty != null && garrisonParty.MemberRoster.TotalRegulars < 200);
				}
				else if (this._settlement.IsCastle)
				{
					MobileParty garrisonParty2 = this._settlement.Town.GarrisonParty;
					flag = (garrisonParty2 != null && garrisonParty2.MemberRoster.TotalRegulars < 160);
				}
				return this._settlement.OwnerClan == base.IssueOwner.Clan && flag && !base.IssueOwner.IsDead && base.IssueOwner.Clan != Clan.PlayerClan;
			}

			// Token: 0x06005013 RID: 20499 RVA: 0x0016D2F8 File Offset: 0x0016B4F8
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flags, out Hero relationHero, out SkillObject skill)
			{
				skill = null;
				relationHero = null;
				flags = IssueBase.PreconditionFlags.None;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flags |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (Hero.MainHero.IsKingdomLeader)
				{
					flags |= IssueBase.PreconditionFlags.MainHeroIsKingdomLeader;
				}
				if (issueGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					flags |= IssueBase.PreconditionFlags.AtWar;
				}
				return flags == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005014 RID: 20500 RVA: 0x0016D35E File Offset: 0x0016B55E
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005015 RID: 20501 RVA: 0x0016D360 File Offset: 0x0016B560
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005016 RID: 20502 RVA: 0x0016D362 File Offset: 0x0016B562
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(30f), this.RewardGold, this._settlement, this.NumberOfTroopToBeRecruited, this._neededTroopType);
			}

			// Token: 0x06005017 RID: 20503 RVA: 0x0016D392 File Offset: 0x0016B592
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
			}

			// Token: 0x06005018 RID: 20504 RVA: 0x0016D39C File Offset: 0x0016B59C
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001A66 RID: 6758
			private const int QuestDurationInDays = 30;

			// Token: 0x04001A67 RID: 6759
			private const int CompanionRequiredSkillLevel = 120;

			// Token: 0x04001A68 RID: 6760
			[SaveableField(60)]
			private Settlement _settlement;

			// Token: 0x04001A69 RID: 6761
			[SaveableField(30)]
			private CharacterObject _neededTroopType;
		}

		// Token: 0x0200064B RID: 1611
		public class LordNeedsGarrisonTroopsIssueQuest : QuestBase
		{
			// Token: 0x06005019 RID: 20505 RVA: 0x0016D39E File Offset: 0x0016B59E
			internal static void AutoGeneratedStaticCollectObjectsLordNeedsGarrisonTroopsIssueQuest(object o, List<object> collectedObjects)
			{
				((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600501A RID: 20506 RVA: 0x0016D3AC File Offset: 0x0016B5AC
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._requestedTroopType);
				collectedObjects.Add(this._playerStartsQuestLog);
			}

			// Token: 0x0600501B RID: 20507 RVA: 0x0016D3CD File Offset: 0x0016B5CD
			internal static object AutoGeneratedGetMemberValue_settlementStringID(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._settlementStringID;
			}

			// Token: 0x0600501C RID: 20508 RVA: 0x0016D3DA File Offset: 0x0016B5DA
			internal static object AutoGeneratedGetMemberValue_requestedTroopAmount(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._requestedTroopAmount;
			}

			// Token: 0x0600501D RID: 20509 RVA: 0x0016D3EC File Offset: 0x0016B5EC
			internal static object AutoGeneratedGetMemberValue_rewardGold(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._rewardGold;
			}

			// Token: 0x0600501E RID: 20510 RVA: 0x0016D3FE File Offset: 0x0016B5FE
			internal static object AutoGeneratedGetMemberValue_requestedTroopType(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._requestedTroopType;
			}

			// Token: 0x0600501F RID: 20511 RVA: 0x0016D40B File Offset: 0x0016B60B
			internal static object AutoGeneratedGetMemberValue_playerStartsQuestLog(object o)
			{
				return ((LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest)o)._playerStartsQuestLog;
			}

			// Token: 0x170010C2 RID: 4290
			// (get) Token: 0x06005020 RID: 20512 RVA: 0x0016D418 File Offset: 0x0016B618
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=g6Ra6LUY}{ISSUE_OWNER.NAME} needs garrison troops in {SETTLEMENT}", null);
					StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.Name);
					return textObject;
				}
			}

			// Token: 0x170010C3 RID: 4291
			// (get) Token: 0x06005021 RID: 20513 RVA: 0x0016D461 File Offset: 0x0016B661
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170010C4 RID: 4292
			// (get) Token: 0x06005022 RID: 20514 RVA: 0x0016D464 File Offset: 0x0016B664
			private TextObject _playerStartsQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=FViaQrbV}{QUEST_GIVER.LINK}, the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} of {QUEST_SETTLEMENT}, told you that {?QUEST_GIVER.GENDER}she{?}he{\\?} needs more troops in {?QUEST_GIVER.GENDER}her{?}his{\\?} garrison. {?QUEST_GIVER.GENDER}She{?}He{\\?} is willing to pay {REWARD}{GOLD_ICON} for your services. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to deliver {NUMBER_OF_TROOP_TO_BE_RECRUITED} {TROOP_TYPE} troops to garrison commander in {QUEST_SETTLEMENT}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TROOP_TYPE", this._requestedTroopType.Name);
					textObject.SetTextVariable("REWARD", this._rewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					textObject.SetTextVariable("NUMBER_OF_TROOP_TO_BE_RECRUITED", this._requestedTroopAmount);
					textObject.SetTextVariable("QUEST_SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170010C5 RID: 4293
			// (get) Token: 0x06005023 RID: 20515 RVA: 0x0016D4FC File Offset: 0x0016B6FC
			private TextObject _successQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=UEn466Y6}You have successfully brought the troops {QUEST_GIVER.LINK} requested. You received {REWARD} gold in return for your service.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this._rewardGold);
					return textObject;
				}
			}

			// Token: 0x170010C6 RID: 4294
			// (get) Token: 0x06005024 RID: 20516 RVA: 0x0016D540 File Offset: 0x0016B740
			private TextObject _questGiverLostTheSettlementLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=zS68eOsl}{QUEST_GIVER.LINK} has lost {SETTLEMENT} and your agreement with {?QUEST_GIVER.GENDER}her{?}his{\\?} canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170010C7 RID: 4295
			// (get) Token: 0x06005025 RID: 20517 RVA: 0x0016D58C File Offset: 0x0016B78C
			private TextObject _questFailedWarDeclaredLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=JIWVeTMD}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} was canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this._settlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170010C8 RID: 4296
			// (get) Token: 0x06005026 RID: 20518 RVA: 0x0016D5D8 File Offset: 0x0016B7D8
			private TextObject _playerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170010C9 RID: 4297
			// (get) Token: 0x06005027 RID: 20519 RVA: 0x0016D60A File Offset: 0x0016B80A
			private TextObject _timeOutLogText
			{
				get
				{
					return new TextObject("{=cnaxgN5b}You have failed to bring the troops in time.", null);
				}
			}

			// Token: 0x06005028 RID: 20520 RVA: 0x0016D618 File Offset: 0x0016B818
			public LordNeedsGarrisonTroopsIssueQuest(string questId, Hero giverHero, CampaignTime duration, int rewardGold, Settlement selectedSettlement, int requestedTroopAmount, CharacterObject requestedTroopType) : base(questId, giverHero, duration, rewardGold)
			{
				this._settlement = selectedSettlement;
				this._settlementStringID = selectedSettlement.StringId;
				this._requestedTroopAmount = requestedTroopAmount;
				this._collectedTroopAmount = 0;
				this._requestedTroopType = requestedTroopType;
				this._rewardGold = rewardGold;
				this.SetDialogs();
				base.AddTrackedObject(this._settlement);
				base.InitializeQuestOnCreation();
			}

			// Token: 0x06005029 RID: 20521 RVA: 0x0016D67C File Offset: 0x0016B87C
			private bool DialogCondition()
			{
				return Hero.OneToOneConversationHero == base.QuestGiver;
			}

			// Token: 0x0600502A RID: 20522 RVA: 0x0016D68C File Offset: 0x0016B88C
			protected override void SetDialogs()
			{
				Campaign.Current.ConversationManager.AddDialogFlow(this.GetGarrisonCommanderDialogFlow(), this);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=9iZg4vpz}Thank you. You will be rewarded when you are done.[if:convo_mocking_aristocratic]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition)).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=o6BunhbE}Have you brought my troops?[if:convo_undecided_open]", null), null, null).Condition(new ConversationSentence.OnConditionDelegate(this.DialogCondition)).Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
				}).BeginPlayerOptions().PlayerOption(new TextObject("{=eC4laxrj}I'm still out recruiting.", null), null).NpcLine(new TextObject("{=TxxbCbUc}Good. I have faith in you...[if:convo_mocking_aristocratic]", null), null, null).CloseDialog().PlayerOption(new TextObject("{=DbraLcwM}I need more time to find proper men.", null), null).NpcLine(new TextObject("{=Mw5bJ5Fb}Every day without a proper garrison is a day that we're vulnerable. Do hurry, if you can.[if:convo_normal]", null), null, null).CloseDialog().EndPlayerOptions();
			}

			// Token: 0x0600502B RID: 20523 RVA: 0x0016D7AD File Offset: 0x0016B9AD
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				this._playerStartsQuestLog = base.AddDiscreteLog(this._playerStartsQuestLogText, new TextObject("{=WIb9VvEM}Collected Troops", null), this._collectedTroopAmount, this._requestedTroopAmount, null, false);
			}

			// Token: 0x0600502C RID: 20524 RVA: 0x0016D7E0 File Offset: 0x0016B9E0
			private DialogFlow GetGarrisonCommanderDialogFlow()
			{
				TextObject textObject = new TextObject("{=abda9slW}We were waiting for you, {?PLAYER.GENDER}madam{?}sir{\\?}. Have you brought the troops that our {?ISSUE_OWNER.GENDER}lady{?}lord{\\?} requested?", null);
				StringHelpers.SetCharacterProperties("ISSUE_OWNER", base.QuestGiver.CharacterObject, textObject, false);
				return DialogFlow.CreateDialogFlow("start", 300).NpcLine(textObject, null, null).Condition(() => CharacterObject.OneToOneConversationCharacter == this._selectedCharacterToTalk).BeginPlayerOptions().PlayerOption(new TextObject("{=ooHbl6JU}Here are your men.", null), null).ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.PlayerGiveTroopsToGarrisonCommanderCondition)).NpcLine(new TextObject("{=Ouy4sN5b}Thank you.[if:convo_mocking_aristocratic]", null), null, null).Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += this.PlayerTransferredTroopsToGarrisonCommander;
				}).CloseDialog().PlayerOption(new TextObject("{=G5tyQj6N}Not yet.", null), null).NpcLine(new TextObject("{=yPOZd1wb}Very well. We'll keep waiting.[if:convo_normal]", null), null, null).CloseDialog().EndPlayerOptions();
			}

			// Token: 0x0600502D RID: 20525 RVA: 0x0016D8B8 File Offset: 0x0016BAB8
			private void PlayerTransferredTroopsToGarrisonCommander()
			{
				using (List<TroopRosterElement>.Enumerator enumerator = MobileParty.MainParty.MemberRoster.GetTroopRoster().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Character == this._requestedTroopType)
						{
							MobileParty.MainParty.MemberRoster.AddToCounts(this._requestedTroopType, -this._requestedTroopAmount, false, 0, 0, true, -1);
							break;
						}
					}
				}
				base.AddLog(this._successQuestLogText, false);
				this.RelationshipChangeWithQuestGiver = 2;
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._rewardGold, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x0600502E RID: 20526 RVA: 0x0016D96C File Offset: 0x0016BB6C
			private bool PlayerGiveTroopsToGarrisonCommanderCondition(out TextObject explanation)
			{
				int num = 0;
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == this._requestedTroopType)
					{
						num = troopRosterElement.Number;
						break;
					}
				}
				if (num < this._requestedTroopAmount)
				{
					explanation = new TextObject("{=VFO2aQ4l}You don't have enough men.", null);
					return false;
				}
				explanation = TextObject.Empty;
				return true;
			}

			// Token: 0x0600502F RID: 20527 RVA: 0x0016D9FC File Offset: 0x0016BBFC
			protected override void InitializeQuestOnGameLoad()
			{
				this._settlement = Settlement.Find(this._settlementStringID);
				this.CalculateTroopAmount();
				this.SetDialogs();
			}

			// Token: 0x06005030 RID: 20528 RVA: 0x0016DA1C File Offset: 0x0016BC1C
			protected override void RegisterEvents()
			{
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			}

			// Token: 0x06005031 RID: 20529 RVA: 0x0016DA85 File Offset: 0x0016BC85
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06005032 RID: 20530 RVA: 0x0016DA98 File Offset: 0x0016BC98
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement == this._settlement && this._settlement.OwnerClan != base.QuestGiver.Clan)
				{
					base.AddLog(this._questGiverLostTheSettlementLogText, false);
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06005033 RID: 20531 RVA: 0x0016DAD0 File Offset: 0x0016BCD0
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					this.CalculateTroopAmount();
					this._collectedTroopAmount = MBMath.ClampInt(this._collectedTroopAmount, 0, this._requestedTroopAmount);
					this._playerStartsQuestLog.UpdateCurrentProgress(this._collectedTroopAmount);
				}
			}

			// Token: 0x06005034 RID: 20532 RVA: 0x0016DB0C File Offset: 0x0016BD0C
			private void CalculateTroopAmount()
			{
				foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character == this._requestedTroopType)
					{
						this._collectedTroopAmount = MobileParty.MainParty.MemberRoster.GetTroopCount(troopRosterElement.Character);
						break;
					}
				}
			}

			// Token: 0x06005035 RID: 20533 RVA: 0x0016DB8C File Offset: 0x0016BD8C
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this._questFailedWarDeclaredLogText);
				}
			}

			// Token: 0x06005036 RID: 20534 RVA: 0x0016DBB6 File Offset: 0x0016BDB6
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this._playerDeclaredWarQuestLogText, this._questFailedWarDeclaredLogText, false);
			}

			// Token: 0x06005037 RID: 20535 RVA: 0x0016DBCE File Offset: 0x0016BDCE
			protected override void OnTimedOut()
			{
				base.AddLog(this._timeOutLogText, false);
				this.RelationshipChangeWithQuestGiver = -5;
			}

			// Token: 0x04001A6A RID: 6762
			internal Settlement _settlement;

			// Token: 0x04001A6B RID: 6763
			[SaveableField(10)]
			private string _settlementStringID;

			// Token: 0x04001A6C RID: 6764
			private int _collectedTroopAmount;

			// Token: 0x04001A6D RID: 6765
			[SaveableField(20)]
			private int _requestedTroopAmount;

			// Token: 0x04001A6E RID: 6766
			[SaveableField(30)]
			private int _rewardGold;

			// Token: 0x04001A6F RID: 6767
			[SaveableField(40)]
			private CharacterObject _requestedTroopType;

			// Token: 0x04001A70 RID: 6768
			internal CharacterObject _selectedCharacterToTalk;

			// Token: 0x04001A71 RID: 6769
			[SaveableField(50)]
			private JournalLog _playerStartsQuestLog;
		}

		// Token: 0x0200064C RID: 1612
		public class LordNeedsGarrisonTroopsIssueQuestTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600503A RID: 20538 RVA: 0x0016DC12 File Offset: 0x0016BE12
			public LordNeedsGarrisonTroopsIssueQuestTypeDefiner() : base(5080000)
			{
			}

			// Token: 0x0600503B RID: 20539 RVA: 0x0016DC1F File Offset: 0x0016BE1F
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssue), 1, null);
				base.AddClassDefinition(typeof(LordNeedsGarrisonTroopsIssueQuestBehavior.LordNeedsGarrisonTroopsIssueQuest), 2, null);
			}
		}
	}
}
