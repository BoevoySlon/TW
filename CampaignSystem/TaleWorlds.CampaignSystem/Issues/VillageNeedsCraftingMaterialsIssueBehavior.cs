﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Extensions;
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
	// Token: 0x0200031E RID: 798
	public class VillageNeedsCraftingMaterialsIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x06002DEC RID: 11756 RVA: 0x000C0D0C File Offset: 0x000BEF0C
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000C0D28 File Offset: 0x000BEF28
		private void OnCheckForIssue(Hero hero)
		{
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, this.ConditionsHold(hero) ? new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnStartIssue), typeof(VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssue), IssueBase.IssueFrequency.Rare, null) : new PotentialIssueData(typeof(VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssue), IssueBase.IssueFrequency.Rare));
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000C0D7D File Offset: 0x000BEF7D
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsRuralNotable && !issueGiver.MapFaction.IsAtWarWith(Clan.PlayerClan);
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000C0D9C File Offset: 0x000BEF9C
		private IssueBase OnStartIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			return new VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssue(issueOwner);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000C0DA4 File Offset: 0x000BEFA4
		private static ItemObject SelectCraftingMaterial()
		{
			int num = MBRandom.RandomInt(0, 2);
			if (num == 0)
			{
				return DefaultItems.IronIngot1;
			}
			if (num != 1)
			{
				return DefaultItems.IronIngot1;
			}
			return DefaultItems.IronIngot2;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000C0DD3 File Offset: 0x000BEFD3
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04000DB4 RID: 3508
		private const IssueBase.IssueFrequency VillageNeedsCraftingMaterialsIssueFrequency = IssueBase.IssueFrequency.Rare;

		// Token: 0x0200067B RID: 1659
		public class VillageNeedsCraftingMaterialsIssue : IssueBase
		{
			// Token: 0x06005405 RID: 21509 RVA: 0x0017BA45 File Offset: 0x00179C45
			internal static void AutoGeneratedStaticCollectObjectsVillageNeedsCraftingMaterialsIssue(object o, List<object> collectedObjects)
			{
				((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005406 RID: 21510 RVA: 0x0017BA53 File Offset: 0x00179C53
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._requestedItem);
			}

			// Token: 0x06005407 RID: 21511 RVA: 0x0017BA68 File Offset: 0x00179C68
			internal static object AutoGeneratedGetMemberValue_requestedItem(object o)
			{
				return ((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssue)o)._requestedItem;
			}

			// Token: 0x06005408 RID: 21512 RVA: 0x0017BA75 File Offset: 0x00179C75
			internal static object AutoGeneratedGetMemberValue_promisedPayment(object o)
			{
				return ((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssue)o)._promisedPayment;
			}

			// Token: 0x170011E3 RID: 4579
			// (get) Token: 0x06005409 RID: 21513 RVA: 0x0017BA87 File Offset: 0x00179C87
			private int _numberOfRequestedItem
			{
				get
				{
					return MathF.Round((float)((int)(750f / (float)this._requestedItem.Value)) * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170011E4 RID: 4580
			// (get) Token: 0x0600540A RID: 21514 RVA: 0x0017BAA9 File Offset: 0x00179CA9
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return 500 + (int)(700f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x170011E5 RID: 4581
			// (get) Token: 0x0600540B RID: 21515 RVA: 0x0017BABE File Offset: 0x00179CBE
			protected override bool IssueQuestCanBeDuplicated
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170011E6 RID: 4582
			// (get) Token: 0x0600540C RID: 21516 RVA: 0x0017BAC1 File Offset: 0x00179CC1
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 4;
				}
			}

			// Token: 0x170011E7 RID: 4583
			// (get) Token: 0x0600540D RID: 21517 RVA: 0x0017BAC4 File Offset: 0x00179CC4
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return (int)(2f + 4f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x0600540E RID: 21518 RVA: 0x0017BAD9 File Offset: 0x00179CD9
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.VillageHearth)
				{
					return -0.2f;
				}
				if (issueEffect == DefaultIssueEffects.IssueOwnerPower)
				{
					return -0.1f;
				}
				return 0f;
			}

			// Token: 0x0600540F RID: 21519 RVA: 0x0017BAFC File Offset: 0x00179CFC
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				return new ValueTuple<SkillObject, int>(DefaultSkills.Crafting, 120);
			}

			// Token: 0x06005410 RID: 21520 RVA: 0x0017BB0A File Offset: 0x00179D0A
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 0, false);
			}

			// Token: 0x06005411 RID: 21521 RVA: 0x0017BB2C File Offset: 0x00179D2C
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.GetPayment(), false);
				this.RelationshipChangeWithIssueOwner = 5;
				base.IssueSettlement.Village.Hearth += 60f;
				base.IssueOwner.AddPower(10f);
			}

			// Token: 0x06005412 RID: 21522 RVA: 0x0017BB7E File Offset: 0x00179D7E
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 0, false);
			}

			// Token: 0x170011E8 RID: 4584
			// (get) Token: 0x06005413 RID: 21523 RVA: 0x0017BB96 File Offset: 0x00179D96
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=eR7P1cVA}{VILLAGE} Needs Crafting Materials", null);
					textObject.SetTextVariable("VILLAGE", base.IssueOwner.CurrentSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170011E9 RID: 4585
			// (get) Token: 0x06005414 RID: 21524 RVA: 0x0017BBBF File Offset: 0x00179DBF
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=5CJrR0X3}{ISSUE_GIVER.LINK} in the village requested crafting materials for their ongoing project.", null);
					textObject.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, false);
					return textObject;
				}
			}

			// Token: 0x170011EA RID: 4586
			// (get) Token: 0x06005415 RID: 21525 RVA: 0x0017BBE3 File Offset: 0x00179DE3
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=095beaQ5}Yes, there's a lot of work we need to do around the village, and we're short on the materials that our smith needs to make us tools and fittings. Do you think you could get us some? We'll pay well.[ib:demure][if:convo_dismayed]", null);
				}
			}

			// Token: 0x170011EB RID: 4587
			// (get) Token: 0x06005416 RID: 21526 RVA: 0x0017BBF0 File Offset: 0x00179DF0
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=xmu89biL}Maybe I can help. What do you need exactly?", null);
				}
			}

			// Token: 0x170011EC RID: 4588
			// (get) Token: 0x06005417 RID: 21527 RVA: 0x0017BC00 File Offset: 0x00179E00
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=PftlaE0x}We need {REQUESTED_ITEM_COUNT} {?(REQUESTED_ITEM_COUNT > 1)}{PLURAL(REQUESTED_ITEM)}{?}{REQUESTED_ITEM}{\\?} in {NUMBER_OF_DAYS} days. We need to repair some roofs before the next big storms. I can offer {PAYMENT}{GOLD_ICON}. What do you say?", null);
					textObject.SetTextVariable("PAYMENT", this.GetPayment());
					textObject.SetTextVariable("REQUESTED_ITEM", this._requestedItem.Name);
					textObject.SetTextVariable("REQUESTED_ITEM_COUNT", this._numberOfRequestedItem);
					textObject.SetTextVariable("NUMBER_OF_DAYS", 30);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					return textObject;
				}
			}

			// Token: 0x170011ED RID: 4589
			// (get) Token: 0x06005418 RID: 21528 RVA: 0x0017BC72 File Offset: 0x00179E72
			public override TextObject IssuePlayerResponseAfterAlternativeExplanation
			{
				get
				{
					return new TextObject("{=i96OaGH3}Is there anything else I could do to help?", null);
				}
			}

			// Token: 0x170011EE RID: 4590
			// (get) Token: 0x06005419 RID: 21529 RVA: 0x0017BC7F File Offset: 0x00179E7F
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=WzdhPF7M}Well, if we had some extra skilled labor, we could probably melt down old tools and reforge them. That's too much work for just our smith by himself, but maybe he could do it with someone proficient in crafting to help him.[ib:demure2][if:convo_thinking]", null);
				}
			}

			// Token: 0x170011EF RID: 4591
			// (get) Token: 0x0600541A RID: 21530 RVA: 0x0017BC8C File Offset: 0x00179E8C
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=WsmH9Cfd}I will provide what you need.", null);
				}
			}

			// Token: 0x170011F0 RID: 4592
			// (get) Token: 0x0600541B RID: 21531 RVA: 0x0017BC99 File Offset: 0x00179E99
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=8DWTTnpP}My comrade will help your smith to produce what you need.", null);
				}
			}

			// Token: 0x170011F1 RID: 4593
			// (get) Token: 0x0600541C RID: 21532 RVA: 0x0017BCA6 File Offset: 0x00179EA6
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					return new TextObject("{=xlagNKZ2}Thank you. With their help, we should be able to make what we need.[if:convo_astonished]", null);
				}
			}

			// Token: 0x170011F2 RID: 4594
			// (get) Token: 0x0600541D RID: 21533 RVA: 0x0017BCB3 File Offset: 0x00179EB3
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=P3Uu0Ham}Your companion is still working with our smith. I hope they will finish the order in time.[if:convo_approving]", null);
				}
			}

			// Token: 0x170011F3 RID: 4595
			// (get) Token: 0x0600541E RID: 21534 RVA: 0x0017BCC0 File Offset: 0x00179EC0
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170011F4 RID: 4596
			// (get) Token: 0x0600541F RID: 21535 RVA: 0x0017BCC3 File Offset: 0x00179EC3
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170011F5 RID: 4597
			// (get) Token: 0x06005420 RID: 21536 RVA: 0x0017BCC6 File Offset: 0x00179EC6
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Duration;
				}
			}

			// Token: 0x170011F6 RID: 4598
			// (get) Token: 0x06005421 RID: 21537 RVA: 0x0017BCCC File Offset: 0x00179ECC
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=1XuYGQcT}{ISSUE_GIVER.LINK} told you that {?QUEST_GIVER.GENDER}her{?}his{\\?} local smith needs {REQUESTED_ITEM} to forge more tools. You asked your companion {COMPANION.LINK} to help the local smith and craft {REQUESTED_ITEM_COUNT} {?(REQUESTED_ITEM_COUNT > 1)}{PLURAL(REQUESTED_ITEM)}{?}{REQUESTED_ITEM}{\\?} for the village. Your companion will rejoin your party in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", base.IssueOwner.CurrentSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("REQUESTED_ITEM", this._requestedItem.Name);
					textObject.SetTextVariable("REQUESTED_ITEM_COUNT", this._numberOfRequestedItem);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x170011F7 RID: 4599
			// (get) Token: 0x06005422 RID: 21538 RVA: 0x0017BD70 File Offset: 0x00179F70
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=n86jgG3m}Your companion {COMPANION.LINK} has helped the local smith and produced {REQUESTED_AMOUNT} {?(REQUESTED_AMOUNT > 1)}{PLURAL(REQUESTED_GOOD)}{?}{REQUESTED_GOOD}{\\?} as you promised.", null);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("REQUESTED_AMOUNT", this._numberOfRequestedItem);
					textObject.SetTextVariable("REQUESTED_GOOD", this._requestedItem.Name);
					return textObject;
				}
			}

			// Token: 0x06005423 RID: 21539 RVA: 0x0017BDCB File Offset: 0x00179FCB
			protected override void OnGameLoad()
			{
			}

			// Token: 0x06005424 RID: 21540 RVA: 0x0017BDCD File Offset: 0x00179FCD
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005425 RID: 21541 RVA: 0x0017BDCF File Offset: 0x00179FCF
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(30f), this.GetPayment(), this._requestedItem, this._numberOfRequestedItem);
			}

			// Token: 0x06005426 RID: 21542 RVA: 0x0017BDF9 File Offset: 0x00179FF9
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.Rare;
			}

			// Token: 0x06005427 RID: 21543 RVA: 0x0017BDFC File Offset: 0x00179FFC
			public override void AlternativeSolutionStartConsequence()
			{
				this._promisedPayment = this.GetPayment();
			}

			// Token: 0x06005428 RID: 21544 RVA: 0x0017BE0C File Offset: 0x0017A00C
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flags, out Hero relationHero, out SkillObject skill)
			{
				flags = IssueBase.PreconditionFlags.None;
				relationHero = null;
				skill = null;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flags |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (FactionManager.IsAtWarAgainstFaction(issueGiver.MapFaction, Hero.MainHero.MapFaction))
				{
					flags |= IssueBase.PreconditionFlags.AtWar;
				}
				return flags == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x06005429 RID: 21545 RVA: 0x0017BE5C File Offset: 0x0017A05C
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x0600542A RID: 21546 RVA: 0x0017BE5E File Offset: 0x0017A05E
			public override bool IssueStayAliveConditions()
			{
				return !base.IssueOwner.CurrentSettlement.IsRaided && !base.IssueOwner.CurrentSettlement.IsUnderRaid;
			}

			// Token: 0x0600542B RID: 21547 RVA: 0x0017BE87 File Offset: 0x0017A087
			public VillageNeedsCraftingMaterialsIssue(Hero issueOwner) : base(issueOwner, CampaignTime.DaysFromNow(30f))
			{
				this._requestedItem = VillageNeedsCraftingMaterialsIssueBehavior.SelectCraftingMaterial();
			}

			// Token: 0x0600542C RID: 21548 RVA: 0x0017BEA8 File Offset: 0x0017A0A8
			private int GetPayment()
			{
				if (this._promisedPayment != 0)
				{
					return this._promisedPayment;
				}
				return 750 + (base.IssueSettlement.Village.Bound.Town.MarketData.GetPrice(this._requestedItem, null, false, null) + QuestHelper.GetAveragePriceOfItemInTheWorld(this._requestedItem) / 2) * this._numberOfRequestedItem;
			}

			// Token: 0x04001B46 RID: 6982
			private const int TimeLimit = 30;

			// Token: 0x04001B47 RID: 6983
			private const int PowerChangeForQuestGiver = 10;

			// Token: 0x04001B48 RID: 6984
			private const int RelationWithIssueOwnerRewardOnSuccess = 5;

			// Token: 0x04001B49 RID: 6985
			private const int VillageHeartChangeOnAlternativeSuccess = 60;

			// Token: 0x04001B4A RID: 6986
			private const int RequiredSkillValueForAlternativeSolution = 120;

			// Token: 0x04001B4B RID: 6987
			[SaveableField(1)]
			private readonly ItemObject _requestedItem;

			// Token: 0x04001B4C RID: 6988
			[SaveableField(4)]
			private int _promisedPayment;
		}

		// Token: 0x0200067C RID: 1660
		public class VillageNeedsCraftingMaterialsIssueQuest : QuestBase
		{
			// Token: 0x0600542D RID: 21549 RVA: 0x0017BF07 File Offset: 0x0017A107
			internal static void AutoGeneratedStaticCollectObjectsVillageNeedsCraftingMaterialsIssueQuest(object o, List<object> collectedObjects)
			{
				((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600542E RID: 21550 RVA: 0x0017BF15 File Offset: 0x0017A115
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._requestedItem);
				collectedObjects.Add(this._playerAcceptedQuestLog);
				collectedObjects.Add(this._playerHasNeededItemsLog);
			}

			// Token: 0x0600542F RID: 21551 RVA: 0x0017BF42 File Offset: 0x0017A142
			internal static object AutoGeneratedGetMemberValue_requestedItemAmount(object o)
			{
				return ((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssueQuest)o)._requestedItemAmount;
			}

			// Token: 0x06005430 RID: 21552 RVA: 0x0017BF54 File Offset: 0x0017A154
			internal static object AutoGeneratedGetMemberValue_requestedItem(object o)
			{
				return ((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssueQuest)o)._requestedItem;
			}

			// Token: 0x06005431 RID: 21553 RVA: 0x0017BF61 File Offset: 0x0017A161
			internal static object AutoGeneratedGetMemberValue_playerAcceptedQuestLog(object o)
			{
				return ((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssueQuest)o)._playerAcceptedQuestLog;
			}

			// Token: 0x06005432 RID: 21554 RVA: 0x0017BF6E File Offset: 0x0017A16E
			internal static object AutoGeneratedGetMemberValue_playerHasNeededItemsLog(object o)
			{
				return ((VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssueQuest)o)._playerHasNeededItemsLog;
			}

			// Token: 0x170011F8 RID: 4600
			// (get) Token: 0x06005433 RID: 21555 RVA: 0x0017BF7C File Offset: 0x0017A17C
			private TextObject QuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=YZeKScP5}{QUEST_GIVER.LINK} told you that {?QUEST_GIVER.GENDER}her{?}his{\\?} local smith needs {REQUESTED_ITEM} to forge more tools. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to bring {REQUESTED_ITEM_AMOUNT} {?(REQUESTED_ITEM_AMOUNT > 1)}{PLURAL(REQUESTED_ITEM)}{?}{REQUESTED_ITEM}{\\?} to {?QUEST_GIVER.GENDER}her{?}him{\\?}.", null);
					textObject.SetTextVariable("REQUESTED_ITEM_AMOUNT", this._requestedItemAmount);
					textObject.SetTextVariable("REQUESTED_ITEM", this._requestedItem.Name);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011F9 RID: 4601
			// (get) Token: 0x06005434 RID: 21556 RVA: 0x0017BFD8 File Offset: 0x0017A1D8
			private TextObject QuestSuccessLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=LiDSTrvV}You brought {REQUESTED_ITEM_AMOUNT} {?(REQUESTED_ITEM_AMOUNT > 1)}{PLURAL(REQUESTED_ITEM)}{?}{REQUESTED_ITEM}{\\?} to {?QUEST_GIVER.GENDER}her{?}him{\\?} as promised.", null);
					textObject.SetTextVariable("REQUESTED_ITEM_AMOUNT", this._requestedItemAmount);
					textObject.SetTextVariable("REQUESTED_ITEM", this._requestedItem.Name);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011FA RID: 4602
			// (get) Token: 0x06005435 RID: 21557 RVA: 0x0017C034 File Offset: 0x0017A234
			private TextObject QuestCanceledWarDeclaredLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x170011FB RID: 4603
			// (get) Token: 0x06005436 RID: 21558 RVA: 0x0017C068 File Offset: 0x0017A268
			private TextObject QuestGiverVillageRaidedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=gJG0xmAq}{QUEST_GIVER.LINK}'s village {QUEST_SETTLEMENT} was raided. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.QuestGiver.CurrentSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x170011FC RID: 4604
			// (get) Token: 0x06005437 RID: 21559 RVA: 0x0017C0B8 File Offset: 0x0017A2B8
			private TextObject QuestFailedWithTimeOutLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=nmz1ky2D}You failed to deliver {REQUESTED_ITEM_AMOUNT} {?(REQUESTED_ITEM_AMOUNT > 1)}{PLURAL(REQUESTED_ITEM)}{?}{REQUESTED_ITEM}{\\?} to {QUEST_GIVER.LINK} in time.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REQUESTED_ITEM_AMOUNT", this._requestedItemAmount);
					textObject.SetTextVariable("REQUESTED_ITEM", this._requestedItem.Name);
					return textObject;
				}
			}

			// Token: 0x170011FD RID: 4605
			// (get) Token: 0x06005438 RID: 21560 RVA: 0x0017C113 File Offset: 0x0017A313
			private TextObject PlayerHasNeededItemsLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=MxpPkytG}You now have enough {ITEM} to complete the quest. Return to {QUEST_SETTLEMENT} to hand them over.", null);
					textObject.SetTextVariable("ITEM", this._requestedItem.Name);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.QuestGiver.CurrentSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x06005439 RID: 21561 RVA: 0x0017C153 File Offset: 0x0017A353
			public VillageNeedsCraftingMaterialsIssueQuest(string questId, Hero questGiver, CampaignTime duration, int rewardGold, ItemObject requestedItem, int requestedItemAmount) : base(questId, questGiver, duration, rewardGold)
			{
				this._requestedItem = requestedItem;
				this._requestedItemAmount = requestedItemAmount;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x170011FE RID: 4606
			// (get) Token: 0x0600543A RID: 21562 RVA: 0x0017C17C File Offset: 0x0017A37C
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=LgiRMbgE}{ISSUE_SETTLEMENT} Needs Crafting Materials", null);
					textObject.SetTextVariable("ISSUE_SETTLEMENT", base.QuestGiver.CurrentSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x170011FF RID: 4607
			// (get) Token: 0x0600543B RID: 21563 RVA: 0x0017C1A5 File Offset: 0x0017A3A5
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600543C RID: 21564 RVA: 0x0017C1A8 File Offset: 0x0017A3A8
			protected override void SetDialogs()
			{
				TextObject npcText = new TextObject("{=UbUokDyI}Thank you. We'd appreciate it if you got the goods to us as quickly as possible. Good luck![ib:nervous2][if:convo_excited]", null);
				TextObject textObject = new TextObject("{=4c9ySfVj}Did you find what we needed, {?PLAYER.GENDER}madam{?}sir{\\?}?", null);
				TextObject textObject2 = new TextObject("{=nEGe8rUd}Thank you for your help, {?PLAYER.GENDER}madam{?}sir{\\?}. Here is what we promised.", null);
				TextObject npcText2 = new TextObject("{=sTfr1C8H}Thank you. But if the storms come before you find them, well, that would be bad for us.[ib:nervous2][if:convo_nervous]", null);
				textObject.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, false);
				textObject2.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, false);
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(npcText, null, null).Condition(() => CharacterObject.OneToOneConversationCharacter == base.QuestGiver.CharacterObject).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(textObject, null, null).Condition(() => CharacterObject.OneToOneConversationCharacter == base.QuestGiver.CharacterObject).BeginPlayerOptions().PlayerOption(new TextObject("{=bLRGix1b}Yes, I have them with me.", null), null).ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.CompleteQuestClickableConditions)).NpcLine(textObject2, null, null).Consequence(delegate
				{
					Campaign.Current.ConversationManager.ConversationEndOneShot += this.Success;
				}).CloseDialog().PlayerOption(new TextObject("{=D8KFcE2i}Not yet, I am still working on it.", null), null).NpcLine(npcText2, null, null).CloseDialog().EndPlayerOptions().CloseDialog();
			}

			// Token: 0x0600543D RID: 21565 RVA: 0x0017C2EC File Offset: 0x0017A4EC
			private bool CompleteQuestClickableConditions(out TextObject explanation)
			{
				if (this._playerAcceptedQuestLog.CurrentProgress >= this._requestedItemAmount)
				{
					explanation = TextObject.Empty;
					return true;
				}
				explanation = new TextObject("{=EmBla2xa}You don't have enough {ITEM}", null);
				explanation.SetTextVariable("ITEM", this._requestedItem.Name);
				return false;
			}

			// Token: 0x0600543E RID: 21566 RVA: 0x0017C33B File Offset: 0x0017A53B
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x0600543F RID: 21567 RVA: 0x0017C343 File Offset: 0x0017A543
			protected override void HourlyTick()
			{
			}

			// Token: 0x06005440 RID: 21568 RVA: 0x0017C348 File Offset: 0x0017A548
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				int requiredItemCountOnPlayer = this.GetRequiredItemCountOnPlayer();
				TextObject textObject = new TextObject("{=nAEhfGJk}Collect {ITEM}", null);
				textObject.SetTextVariable("ITEM", this._requestedItem.Name);
				this._playerAcceptedQuestLog = base.AddDiscreteLog(this.QuestStartedLogText, textObject, requiredItemCountOnPlayer, this._requestedItemAmount, null, false);
			}

			// Token: 0x06005441 RID: 21569 RVA: 0x0017C3A1 File Offset: 0x0017A5A1
			protected override void OnTimedOut()
			{
				this.Fail();
			}

			// Token: 0x06005442 RID: 21570 RVA: 0x0017C3AC File Offset: 0x0017A5AC
			private void Success()
			{
				base.AddLog(this.QuestSuccessLogText, false);
				ItemRosterElement itemRosterElement = new ItemRosterElement(this._requestedItem, this._requestedItemAmount, null);
				GiveItemAction.ApplyForParties(PartyBase.MainParty, Settlement.CurrentSettlement.Party, itemRosterElement);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(Hero.MainHero, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 30)
				});
				base.QuestGiver.AddPower(10f);
				this.RelationshipChangeWithQuestGiver = 5;
				base.QuestGiver.CurrentSettlement.Village.Hearth += 30f;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06005443 RID: 21571 RVA: 0x0017C460 File Offset: 0x0017A660
			private void Fail()
			{
				base.AddLog(this.QuestFailedWithTimeOutLogText, false);
				base.QuestGiver.AddPower(-10f);
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.CurrentSettlement.Village.Hearth += -40f;
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06005444 RID: 21572 RVA: 0x0017C4BC File Offset: 0x0017A6BC
			private int GetRequiredItemCountOnPlayer()
			{
				int itemNumber = PartyBase.MainParty.ItemRoster.GetItemNumber(this._requestedItem);
				if (itemNumber >= this._requestedItemAmount)
				{
					TextObject textObject = new TextObject("{=MTCrXEvj}You have enough {ITEM} to complete the quest. Return to {QUEST_SETTLEMENT} to hand it over.", null);
					textObject.SetTextVariable("QUEST_SETTLEMENT", base.QuestGiver.CurrentSettlement.Name);
					textObject.SetTextVariable("ITEM", this._requestedItem.Name);
					MBInformationManager.AddQuickInformation(textObject, 0, null, "");
				}
				if (itemNumber <= this._requestedItemAmount)
				{
					return itemNumber;
				}
				return this._requestedItemAmount;
			}

			// Token: 0x06005445 RID: 21573 RVA: 0x0017C544 File Offset: 0x0017A744
			protected override void RegisterEvents()
			{
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
				CampaignEvents.PlayerInventoryExchangeEvent.AddNonSerializedListener(this, new Action<List<ValueTuple<ItemRosterElement, int>>, List<ValueTuple<ItemRosterElement, int>>, bool>(this.OnPlayerInventoryExchange));
				CampaignEvents.OnNewItemCraftedEvent.AddNonSerializedListener(this, new Action<ItemObject, ItemModifier, bool>(this.OnItemCrafted));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.OnEquipmentSmeltedByHeroEvent.AddNonSerializedListener(this, new Action<Hero, EquipmentElement>(this.OnEquipmentSmeltedByHero));
				CampaignEvents.OnItemsRefinedEvent.AddNonSerializedListener(this, new Action<Hero, Crafting.RefiningFormula>(this.OnItemsRefined));
			}

			// Token: 0x06005446 RID: 21574 RVA: 0x0017C609 File Offset: 0x0017A809
			private void OnItemsRefined(Hero hero, Crafting.RefiningFormula refiningFormula)
			{
				if (hero == Hero.MainHero)
				{
					this.UpdateQuestLog();
				}
			}

			// Token: 0x06005447 RID: 21575 RVA: 0x0017C619 File Offset: 0x0017A819
			private void OnEquipmentSmeltedByHero(Hero hero, EquipmentElement equipmentElement)
			{
				if (hero == Hero.MainHero)
				{
					this.UpdateQuestLog();
				}
			}

			// Token: 0x06005448 RID: 21576 RVA: 0x0017C629 File Offset: 0x0017A829
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.QuestCanceledWarDeclaredLogText, this.QuestCanceledWarDeclaredLogText, false);
			}

			// Token: 0x06005449 RID: 21577 RVA: 0x0017C641 File Offset: 0x0017A841
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x0600544A RID: 21578 RVA: 0x0017C654 File Offset: 0x0017A854
			private void OnItemCrafted(ItemObject item, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
			{
				this.UpdateQuestLog();
			}

			// Token: 0x0600544B RID: 21579 RVA: 0x0017C65C File Offset: 0x0017A85C
			private void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
			{
				this.UpdateQuestLog();
			}

			// Token: 0x0600544C RID: 21580 RVA: 0x0017C664 File Offset: 0x0017A864
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.CurrentSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclaredLogText);
				}
			}

			// Token: 0x0600544D RID: 21581 RVA: 0x0017C693 File Offset: 0x0017A893
			private void OnRaidCompleted(BattleSideEnum battleSide, RaidEventComponent mapEvent)
			{
				if (mapEvent.MapEventSettlement == base.QuestGiver.CurrentSettlement)
				{
					base.CompleteQuestWithCancel(this.QuestGiverVillageRaidedLogText);
				}
			}

			// Token: 0x0600544E RID: 21582 RVA: 0x0017C6B4 File Offset: 0x0017A8B4
			private void CheckIfPlayerReadyToReturnItems()
			{
				if (this._playerHasNeededItemsLog == null && this._playerAcceptedQuestLog.CurrentProgress >= this._requestedItemAmount)
				{
					this._playerHasNeededItemsLog = base.AddLog(this.PlayerHasNeededItemsLogText, false);
					return;
				}
				if (this._playerHasNeededItemsLog != null && this._playerAcceptedQuestLog.CurrentProgress < this._requestedItemAmount)
				{
					base.RemoveLog(this._playerHasNeededItemsLog);
					this._playerHasNeededItemsLog = null;
				}
			}

			// Token: 0x0600544F RID: 21583 RVA: 0x0017C71E File Offset: 0x0017A91E
			private void UpdateQuestLog()
			{
				this._playerAcceptedQuestLog.UpdateCurrentProgress(this.GetRequiredItemCountOnPlayer());
				this.CheckIfPlayerReadyToReturnItems();
			}

			// Token: 0x04001B4D RID: 6989
			[SaveableField(10)]
			private readonly int _requestedItemAmount;

			// Token: 0x04001B4E RID: 6990
			[SaveableField(20)]
			private readonly ItemObject _requestedItem;

			// Token: 0x04001B4F RID: 6991
			[SaveableField(30)]
			private JournalLog _playerAcceptedQuestLog;

			// Token: 0x04001B50 RID: 6992
			[SaveableField(40)]
			private JournalLog _playerHasNeededItemsLog;

			// Token: 0x04001B51 RID: 6993
			private const int SuccessRelationBonus = 5;

			// Token: 0x04001B52 RID: 6994
			private const int FailRelationPenalty = -5;

			// Token: 0x04001B53 RID: 6995
			private const int SuccessPowerBonus = 10;

			// Token: 0x04001B54 RID: 6996
			private const int FailPowerPenalty = -10;

			// Token: 0x04001B55 RID: 6997
			private const int SuccessHonorBonus = 30;

			// Token: 0x04001B56 RID: 6998
			private const int FailWithCrimeHonorPenalty = -50;

			// Token: 0x04001B57 RID: 6999
			private const int SuccessHearthBonus = 30;

			// Token: 0x04001B58 RID: 7000
			private const int FailToDeliverInTimeHearthPenalty = -40;
		}

		// Token: 0x0200067D RID: 1661
		public class VillageNeedsCraftingMaterialsIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005453 RID: 21587 RVA: 0x0017C77C File Offset: 0x0017A97C
			public VillageNeedsCraftingMaterialsIssueTypeDefiner() : base(601000)
			{
			}

			// Token: 0x06005454 RID: 21588 RVA: 0x0017C789 File Offset: 0x0017A989
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssue), 1, null);
				base.AddClassDefinition(typeof(VillageNeedsCraftingMaterialsIssueBehavior.VillageNeedsCraftingMaterialsIssueQuest), 2, null);
			}
		}
	}
}
