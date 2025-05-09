﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
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
	// Token: 0x02000318 RID: 792
	public class NearbyBanditBaseIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x06002D9E RID: 11678 RVA: 0x000BEF9C File Offset: 0x000BD19C
		private Settlement FindSuitableHideout(Hero issueOwner)
		{
			Settlement result = null;
			float num = float.MaxValue;
			foreach (Hideout hideout in from t in Campaign.Current.AllHideouts
			where t.IsInfested
			select t)
			{
				float num2 = hideout.Settlement.GatePosition.DistanceSquared(issueOwner.GetMapPoint().Position2D);
				if (num2 <= 1225f && num2 < num)
				{
					num = num2;
					result = hideout.Settlement;
				}
			}
			return result;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x000BF04C File Offset: 0x000BD24C
		private void OnCheckForIssue(Hero hero)
		{
			if (hero.IsNotable)
			{
				Settlement settlement = this.FindSuitableHideout(hero);
				if (this.ConditionsHold(hero) && settlement != null)
				{
					Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnIssueSelected), typeof(NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue), IssueBase.IssueFrequency.VeryCommon, settlement));
					return;
				}
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue), IssueBase.IssueFrequency.VeryCommon));
			}
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x000BF0C4 File Offset: 0x000BD2C4
		private IssueBase OnIssueSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue(issueOwner, potentialIssueData.RelatedObject as Settlement);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x000BF0EA File Offset: 0x000BD2EA
		private bool ConditionsHold(Hero issueGiver)
		{
			return issueGiver.IsHeadman && issueGiver.CurrentSettlement != null && issueGiver.CurrentSettlement.Village.Bound.Town.Security <= 50f;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x000BF124 File Offset: 0x000BD324
		private void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver = null)
		{
			NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue nearbyBanditBaseIssue;
			if ((nearbyBanditBaseIssue = (issue as NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue)) != null && details == IssueBase.IssueUpdateDetails.IssueFinishedByAILord)
			{
				foreach (MobileParty mobileParty in nearbyBanditBaseIssue.TargetHideout.Parties)
				{
					mobileParty.Ai.SetMovePatrolAroundSettlement(nearbyBanditBaseIssue.TargetHideout);
				}
			}
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000BF194 File Offset: 0x000BD394
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000BF1C4 File Offset: 0x000BD3C4
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x04000DA6 RID: 3494
		private const int NearbyHideoutMaxRange = 35;

		// Token: 0x04000DA7 RID: 3495
		private const IssueBase.IssueFrequency NearbyHideoutIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x0200065F RID: 1631
		public class NearbyBanditBaseIssue : IssueBase
		{
			// Token: 0x06005215 RID: 21013 RVA: 0x001756EF File Offset: 0x001738EF
			internal static void AutoGeneratedStaticCollectObjectsNearbyBanditBaseIssue(object o, List<object> collectedObjects)
			{
				((NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005216 RID: 21014 RVA: 0x001756FD File Offset: 0x001738FD
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetHideout);
				collectedObjects.Add(this._issueSettlement);
			}

			// Token: 0x06005217 RID: 21015 RVA: 0x0017571E File Offset: 0x0017391E
			internal static object AutoGeneratedGetMemberValue_targetHideout(object o)
			{
				return ((NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue)o)._targetHideout;
			}

			// Token: 0x06005218 RID: 21016 RVA: 0x0017572B File Offset: 0x0017392B
			internal static object AutoGeneratedGetMemberValue_issueSettlement(object o)
			{
				return ((NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue)o)._issueSettlement;
			}

			// Token: 0x17001152 RID: 4434
			// (get) Token: 0x06005219 RID: 21017 RVA: 0x00175738 File Offset: 0x00173938
			public override IssueBase.AlternativeSolutionScaleFlag AlternativeSolutionScaleFlags
			{
				get
				{
					return IssueBase.AlternativeSolutionScaleFlag.Casualties | IssueBase.AlternativeSolutionScaleFlag.FailureRisk;
				}
			}

			// Token: 0x17001153 RID: 4435
			// (get) Token: 0x0600521A RID: 21018 RVA: 0x0017573C File Offset: 0x0017393C
			public override int AlternativeSolutionBaseNeededMenCount
			{
				get
				{
					return 10;
				}
			}

			// Token: 0x17001154 RID: 4436
			// (get) Token: 0x0600521B RID: 21019 RVA: 0x00175740 File Offset: 0x00173940
			protected override int AlternativeSolutionBaseDurationInDaysInternal
			{
				get
				{
					return 4 + MathF.Ceiling(6f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x17001155 RID: 4437
			// (get) Token: 0x0600521C RID: 21020 RVA: 0x00175755 File Offset: 0x00173955
			protected override int RewardGold
			{
				get
				{
					return 3000;
				}
			}

			// Token: 0x17001156 RID: 4438
			// (get) Token: 0x0600521D RID: 21021 RVA: 0x0017575C File Offset: 0x0017395C
			internal Settlement TargetHideout
			{
				get
				{
					return this._targetHideout;
				}
			}

			// Token: 0x17001157 RID: 4439
			// (get) Token: 0x0600521E RID: 21022 RVA: 0x00175764 File Offset: 0x00173964
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=vw2Q9jJH}Yes... There's this old ruin, a place that offers a good view of the roads, and is yet hard to reach. Needless to say, it attracts bandits. A new gang has moved in and they have been giving hell to the caravans and travellers passing by.[ib:closed][if:convo_undecided_open]", null);
				}
			}

			// Token: 0x17001158 RID: 4440
			// (get) Token: 0x0600521F RID: 21023 RVA: 0x00175771 File Offset: 0x00173971
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=IqH0jFdK}So you need someone to deal with these bastards?", null);
				}
			}

			// Token: 0x17001159 RID: 4441
			// (get) Token: 0x06005220 RID: 21024 RVA: 0x0017577E File Offset: 0x0017397E
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					return new TextObject("{=zstiYI49}Any bandits there can easily spot and evade a large army moving against them, but if you can enter the hideout with a small group of determined warriors you can catch them unaware.[ib:closed][if:convo_thinking]", null);
				}
			}

			// Token: 0x1700115A RID: 4442
			// (get) Token: 0x06005221 RID: 21025 RVA: 0x0017578B File Offset: 0x0017398B
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=uhYprSnG}I will go to the hideout myself and ambush the bandits.", null);
				}
			}

			// Token: 0x1700115B RID: 4443
			// (get) Token: 0x06005222 RID: 21026 RVA: 0x00175798 File Offset: 0x00173998
			protected override int CompanionSkillRewardXP
			{
				get
				{
					return (int)(1000f + 1250f * base.IssueDifficultyMultiplier);
				}
			}

			// Token: 0x06005223 RID: 21027 RVA: 0x001757AD File Offset: 0x001739AD
			public override bool CanBeCompletedByAI()
			{
				return Hero.MainHero.PartyBelongedToAsPrisoner != this._targetHideout.Party;
			}

			// Token: 0x1700115C RID: 4444
			// (get) Token: 0x06005224 RID: 21028 RVA: 0x001757C9 File Offset: 0x001739C9
			public override TextObject IssueAlternativeSolutionAcceptByPlayer
			{
				get
				{
					TextObject textObject = new TextObject("{=IFasMslv}I will assign a companion with {TROOP_COUNT} good men for {RETURN_DAYS} days.", null);
					textObject.SetTextVariable("TROOP_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x1700115D RID: 4445
			// (get) Token: 0x06005225 RID: 21029 RVA: 0x001757FA File Offset: 0x001739FA
			public override TextObject IssueDiscussAlternativeSolution
			{
				get
				{
					return new TextObject("{=DgVU7owN}I pray for your warriors. The people here will be very glad to hear of their success.[ib:hip][if:convo_excited]", null);
				}
			}

			// Token: 0x1700115E RID: 4446
			// (get) Token: 0x06005226 RID: 21030 RVA: 0x00175808 File Offset: 0x00173A08
			public override TextObject IssueAlternativeSolutionResponseByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=aXOgAKfj}Thank you, {?PLAYER.GENDER}madam{?}sir{\\?}. I hope your people will be successful.[ib:hip][if:convo_excited]", null);
					StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700115F RID: 4447
			// (get) Token: 0x06005227 RID: 21031 RVA: 0x00175839 File Offset: 0x00173A39
			public override TextObject IssueAlternativeSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=VNXgZ8mt}Alternatively, if you can assign a companion with {TROOP_COUNT} or so men to this task, they can do the job.[ib:closed][if:convo_undecided_open]", null);
					textObject.SetTextVariable("TROOP_COUNT", base.GetTotalAlternativeSolutionNeededMenCount());
					return textObject;
				}
			}

			// Token: 0x17001160 RID: 4448
			// (get) Token: 0x06005228 RID: 21032 RVA: 0x00175858 File Offset: 0x00173A58
			public override TextObject IssueAsRumorInSettlement
			{
				get
				{
					TextObject textObject = new TextObject("{=ctgihUte}I hope {QUEST_GIVER.NAME} has a plan to get rid of those bandits.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001161 RID: 4449
			// (get) Token: 0x06005229 RID: 21033 RVA: 0x0017588A File Offset: 0x00173A8A
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17001162 RID: 4450
			// (get) Token: 0x0600522A RID: 21034 RVA: 0x00175890 File Offset: 0x00173A90
			protected override TextObject AlternativeSolutionStartLog
			{
				get
				{
					TextObject textObject = new TextObject("{=G4kpabSf}{ISSUE_GIVER.LINK}, a headman from {ISSUE_SETTLEMENT}, has told you about recent bandit attacks on local villagers and asked you to clear out the outlaws' hideout. You asked {COMPANION.LINK} to take {TROOP_COUNT} of your best men to go and take care of it. They should report back to you in {RETURN_DAYS} days.", null);
					StringHelpers.SetCharacterProperties("PLAYER", Hero.MainHero.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("COMPANION", base.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("ISSUE_SETTLEMENT", this._issueSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("TROOP_COUNT", this.AlternativeSolutionSentTroops.TotalManCount - 1);
					textObject.SetTextVariable("RETURN_DAYS", base.GetTotalAlternativeSolutionDurationInDays());
					return textObject;
				}
			}

			// Token: 0x17001163 RID: 4451
			// (get) Token: 0x0600522B RID: 21035 RVA: 0x00175933 File Offset: 0x00173B33
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001164 RID: 4452
			// (get) Token: 0x0600522C RID: 21036 RVA: 0x00175936 File Offset: 0x00173B36
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=ENYbLO8r}Bandit Base Near {SETTLEMENT}", null);
					textObject.SetTextVariable("SETTLEMENT", this._issueSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x17001165 RID: 4453
			// (get) Token: 0x0600522D RID: 21037 RVA: 0x0017595C File Offset: 0x00173B5C
			public override TextObject Description
			{
				get
				{
					TextObject textObject = new TextObject("{=vZ01a4cG}{QUEST_GIVER.LINK} wants you to clear the hideout that attracts more bandits to {?QUEST_GIVER.GENDER}her{?}his{\\?} region.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001166 RID: 4454
			// (get) Token: 0x0600522E RID: 21038 RVA: 0x00175990 File Offset: 0x00173B90
			public override TextObject IssueAlternativeSolutionSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=SN3pjZiK}You received a message from {QUEST_GIVER.LINK}.\n\"Thank you for clearing out that bandits' nest. Please accept these {REWARD}{GOLD_ICON} denars with our gratitude.\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					return textObject;
				}
			}

			// Token: 0x17001167 RID: 4455
			// (get) Token: 0x0600522F RID: 21039 RVA: 0x001759E8 File Offset: 0x00173BE8
			public override TextObject IssueAlternativeSolutionFailLog
			{
				get
				{
					TextObject textObject = new TextObject("{=qsMnnfQ3}You failed to clear the hideout in time to prevent further attacks. {QUEST_GIVER.LINK} is disappointed.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.IssueOwner.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001168 RID: 4456
			// (get) Token: 0x06005230 RID: 21040 RVA: 0x00175A1A File Offset: 0x00173C1A
			protected override bool IssueQuestCanBeDuplicated
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06005231 RID: 21041 RVA: 0x00175A1D File Offset: 0x00173C1D
			public NearbyBanditBaseIssue(Hero issueOwner, Settlement targetHideout) : base(issueOwner, CampaignTime.DaysFromNow(15f))
			{
				this._targetHideout = targetHideout;
			}

			// Token: 0x06005232 RID: 21042 RVA: 0x00175A37 File Offset: 0x00173C37
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.SettlementProsperity)
				{
					return -0.2f;
				}
				if (issueEffect == DefaultIssueEffects.SettlementSecurity)
				{
					return -1f;
				}
				return 0f;
			}

			// Token: 0x06005233 RID: 21043 RVA: 0x00175A5C File Offset: 0x00173C5C
			public override ValueTuple<SkillObject, int> GetAlternativeSolutionSkill(Hero hero)
			{
				int skillValue = hero.GetSkillValue(DefaultSkills.OneHanded);
				int skillValue2 = hero.GetSkillValue(DefaultSkills.TwoHanded);
				int skillValue3 = hero.GetSkillValue(DefaultSkills.Polearm);
				if (skillValue >= skillValue2 && skillValue >= skillValue3)
				{
					return new ValueTuple<SkillObject, int>(DefaultSkills.OneHanded, 120);
				}
				return new ValueTuple<SkillObject, int>((skillValue2 >= skillValue3) ? DefaultSkills.TwoHanded : DefaultSkills.Polearm, 120);
			}

			// Token: 0x06005234 RID: 21044 RVA: 0x00175AB9 File Offset: 0x00173CB9
			protected override void AfterIssueCreation()
			{
				this._issueSettlement = base.IssueOwner.CurrentSettlement;
			}

			// Token: 0x06005235 RID: 21045 RVA: 0x00175ACC File Offset: 0x00173CCC
			public override bool DoTroopsSatisfyAlternativeSolution(TroopRoster troopRoster, out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(troopRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 2, false);
			}

			// Token: 0x06005236 RID: 21046 RVA: 0x00175AE4 File Offset: 0x00173CE4
			public override bool IsTroopTypeNeededByAlternativeSolution(CharacterObject character)
			{
				return character.Tier >= 2;
			}

			// Token: 0x06005237 RID: 21047 RVA: 0x00175AF2 File Offset: 0x00173CF2
			public override bool AlternativeSolutionCondition(out TextObject explanation)
			{
				explanation = TextObject.Empty;
				return QuestHelper.CheckRosterForAlternativeSolution(MobileParty.MainParty.MemberRoster, base.GetTotalAlternativeSolutionNeededMenCount(), ref explanation, 2, false);
			}

			// Token: 0x06005238 RID: 21048 RVA: 0x00175B14 File Offset: 0x00173D14
			protected override void AlternativeSolutionEndWithSuccessConsequence()
			{
				this.RelationshipChangeWithIssueOwner = 5;
				base.IssueOwner.AddPower(5f);
				this._issueSettlement.Village.Bound.Town.Prosperity += 10f;
				TraitLevelingHelper.OnIssueSolvedThroughAlternativeSolution(base.IssueOwner, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
			}

			// Token: 0x06005239 RID: 21049 RVA: 0x00175B8E File Offset: 0x00173D8E
			protected override void AlternativeSolutionEndWithFailureConsequence()
			{
				this.RelationshipChangeWithIssueOwner = -5;
				base.IssueOwner.AddPower(-5f);
				this._issueSettlement.Village.Bound.Town.Prosperity += -10f;
			}

			// Token: 0x0600523A RID: 21050 RVA: 0x00175BCE File Offset: 0x00173DCE
			protected override void OnGameLoad()
			{
			}

			// Token: 0x0600523B RID: 21051 RVA: 0x00175BD0 File Offset: 0x00173DD0
			protected override void HourlyTick()
			{
			}

			// Token: 0x0600523C RID: 21052 RVA: 0x00175BD2 File Offset: 0x00173DD2
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				return new NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssueQuest(questId, base.IssueOwner, this._targetHideout, this._issueSettlement, this.RewardGold, CampaignTime.DaysFromNow(30f));
			}

			// Token: 0x0600523D RID: 21053 RVA: 0x00175BFC File Offset: 0x00173DFC
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x0600523E RID: 21054 RVA: 0x00175C00 File Offset: 0x00173E00
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

			// Token: 0x0600523F RID: 21055 RVA: 0x00175C50 File Offset: 0x00173E50
			public override bool IssueStayAliveConditions()
			{
				return this._targetHideout.Hideout.IsInfested && base.IssueOwner.CurrentSettlement.IsVillage && !base.IssueOwner.CurrentSettlement.IsRaided && !base.IssueOwner.CurrentSettlement.IsUnderRaid && base.IssueOwner.CurrentSettlement.Village.Bound.Town.Security <= 80f;
			}

			// Token: 0x06005240 RID: 21056 RVA: 0x00175CD0 File Offset: 0x00173ED0
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001ABF RID: 6847
			private const int AlternativeSolutionFinalMenCount = 10;

			// Token: 0x04001AC0 RID: 6848
			private const int AlternativeSolutionMinimumTroopTier = 2;

			// Token: 0x04001AC1 RID: 6849
			private const int AlternativeSolutionCompanionSkillThreshold = 120;

			// Token: 0x04001AC2 RID: 6850
			private const int AlternativeSolutionRelationRewardOnSuccess = 5;

			// Token: 0x04001AC3 RID: 6851
			private const int AlternativeSolutionRelationPenaltyOnFail = -5;

			// Token: 0x04001AC4 RID: 6852
			private const int IssueOwnerPowerBonusOnSuccess = 5;

			// Token: 0x04001AC5 RID: 6853
			private const int IssueOwnerPowerPenaltyOnFail = -5;

			// Token: 0x04001AC6 RID: 6854
			private const int SettlementProsperityBonusOnSuccess = 10;

			// Token: 0x04001AC7 RID: 6855
			private const int SettlementProsperityPenaltyOnFail = -10;

			// Token: 0x04001AC8 RID: 6856
			private const int IssueDuration = 15;

			// Token: 0x04001AC9 RID: 6857
			private const int QuestTimeLimit = 30;

			// Token: 0x04001ACA RID: 6858
			[SaveableField(100)]
			private readonly Settlement _targetHideout;

			// Token: 0x04001ACB RID: 6859
			[SaveableField(101)]
			private Settlement _issueSettlement;
		}

		// Token: 0x02000660 RID: 1632
		public class NearbyBanditBaseIssueQuest : QuestBase
		{
			// Token: 0x06005241 RID: 21057 RVA: 0x00175CD2 File Offset: 0x00173ED2
			internal static void AutoGeneratedStaticCollectObjectsNearbyBanditBaseIssueQuest(object o, List<object> collectedObjects)
			{
				((NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06005242 RID: 21058 RVA: 0x00175CE0 File Offset: 0x00173EE0
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetHideout);
				collectedObjects.Add(this._questSettlement);
			}

			// Token: 0x06005243 RID: 21059 RVA: 0x00175D01 File Offset: 0x00173F01
			internal static object AutoGeneratedGetMemberValue_targetHideout(object o)
			{
				return ((NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssueQuest)o)._targetHideout;
			}

			// Token: 0x06005244 RID: 21060 RVA: 0x00175D0E File Offset: 0x00173F0E
			internal static object AutoGeneratedGetMemberValue_questSettlement(object o)
			{
				return ((NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssueQuest)o)._questSettlement;
			}

			// Token: 0x17001169 RID: 4457
			// (get) Token: 0x06005245 RID: 21061 RVA: 0x00175D1B File Offset: 0x00173F1B
			public override TextObject Title
			{
				get
				{
					TextObject textObject = new TextObject("{=ENYbLO8r}Bandit Base Near {SETTLEMENT}", null);
					textObject.SetTextVariable("SETTLEMENT", this._questSettlement.Name);
					return textObject;
				}
			}

			// Token: 0x1700116A RID: 4458
			// (get) Token: 0x06005246 RID: 21062 RVA: 0x00175D3F File Offset: 0x00173F3F
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700116B RID: 4459
			// (get) Token: 0x06005247 RID: 21063 RVA: 0x00175D44 File Offset: 0x00173F44
			private TextObject _onQuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=ogsh3V6G}{QUEST_GIVER.LINK}, a headman from {QUEST_SETTLEMENT}, has told you about the hideout of some bandits who have recently been attacking local villagers. You told {?QUEST_GIVER.GENDER}her{?}him{\\?} that you will take care of the situation yourself. {QUEST_GIVER.LINK} also marked the location of the hideout on your map.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("QUEST_SETTLEMENT", this._questSettlement.EncyclopediaLinkWithName);
					return textObject;
				}
			}

			// Token: 0x1700116C RID: 4460
			// (get) Token: 0x06005248 RID: 21064 RVA: 0x00175D90 File Offset: 0x00173F90
			private TextObject _onQuestSucceededLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=SN3pjZiK}You received a message from {QUEST_GIVER.LINK}.\n\"Thank you for clearing out that bandits' nest. Please accept these {REWARD}{GOLD_ICON} denars with our gratitude.\"", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("REWARD", this.RewardGold);
					textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					return textObject;
				}
			}

			// Token: 0x1700116D RID: 4461
			// (get) Token: 0x06005249 RID: 21065 RVA: 0x00175DE8 File Offset: 0x00173FE8
			private TextObject _onQuestFailedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=qsMnnfQ3}You failed to clear the hideout in time to prevent further attacks. {QUEST_GIVER.LINK} is disappointed.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x1700116E RID: 4462
			// (get) Token: 0x0600524A RID: 21066 RVA: 0x00175E1C File Offset: 0x0017401C
			private TextObject _onQuestCanceledLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=4Bub0GY6}Hideout was cleared by someone else. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x0600524B RID: 21067 RVA: 0x00175E4E File Offset: 0x0017404E
			public NearbyBanditBaseIssueQuest(string questId, Hero questGiver, Settlement targetHideout, Settlement questSettlement, int rewardGold, CampaignTime duration) : base(questId, questGiver, duration, rewardGold)
			{
				this._targetHideout = targetHideout;
				this._questSettlement = questSettlement;
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x0600524C RID: 21068 RVA: 0x00175E77 File Offset: 0x00174077
			protected override void InitializeQuestOnGameLoad()
			{
				this.SetDialogs();
			}

			// Token: 0x0600524D RID: 21069 RVA: 0x00175E7F File Offset: 0x0017407F
			protected override void HourlyTick()
			{
			}

			// Token: 0x0600524E RID: 21070 RVA: 0x00175E84 File Offset: 0x00174084
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine("{=spj8bYVo}Good! I'll mark the hideout for you on a map.[if:convo_excited]", null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).Consequence(new ConversationSentence.OnConsequenceDelegate(this.OnQuestAccepted)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine("{=l9wYpIuV}Any news? Have you managed to clear out the hideout yet?[if:convo_astonished]", null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).BeginPlayerOptions().PlayerOption("{=wErSpkjy}I'm still working on it.", null).NpcLine("{=XTt6gZ7h}Do make haste, if you can. As long as those bandits are up there, no traveller is safe![if:convo_grave]", null, null).CloseDialog().PlayerOption("{=I8raOMRH}Sorry. No progress yet.", null).NpcLine("{=kWruAXaF}Well... You know as long as those bandits remain there, no traveller is safe.[if:convo_grave]", null, null).CloseDialog().EndPlayerOptions().CloseDialog();
			}

			// Token: 0x0600524F RID: 21071 RVA: 0x00175F4C File Offset: 0x0017414C
			private void OnQuestAccepted()
			{
				base.StartQuest();
				this._targetHideout.Hideout.IsSpotted = true;
				this._targetHideout.IsVisible = true;
				base.AddTrackedObject(this._targetHideout);
				QuestHelper.AddMapArrowFromPointToTarget(new TextObject("{=xpsQyPaV}Direction to Bandits", null), this._questSettlement.Position2D, this._targetHideout.Position2D, 5f, 0.1f);
				TextObject textObject = new TextObject("{=XGa8MkbJ}{QUEST_GIVER.NAME} has marked the hideout on your map", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, "");
				base.AddLog(this._onQuestStartedLogText, false);
			}

			// Token: 0x06005250 RID: 21072 RVA: 0x00175FF8 File Offset: 0x001741F8
			private void OnQuestSucceeded()
			{
				base.AddLog(this._onQuestSucceededLogText, false);
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.RewardGold, false);
				GainRenownAction.Apply(Hero.MainHero, 1f, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 50)
				});
				base.QuestGiver.AddPower(5f);
				this.RelationshipChangeWithQuestGiver = 5;
				this._questSettlement.Village.Bound.Town.Prosperity += 10f;
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x06005251 RID: 21073 RVA: 0x00176098 File Offset: 0x00174298
			private void OnQuestFailed(bool isTimedOut)
			{
				base.AddLog(this._onQuestFailedLogText, false);
				this.RelationshipChangeWithQuestGiver = -5;
				base.QuestGiver.AddPower(-5f);
				this._questSettlement.Village.Bound.Town.Prosperity += -10f;
				this._questSettlement.Village.Bound.Town.Security += -5f;
				if (!isTimedOut)
				{
					base.CompleteQuestWithFail(null);
				}
			}

			// Token: 0x06005252 RID: 21074 RVA: 0x00176121 File Offset: 0x00174321
			private void OnQuestCanceled()
			{
				base.AddLog(this._onQuestCanceledLogText, false);
				base.CompleteQuestWithFail(null);
			}

			// Token: 0x06005253 RID: 21075 RVA: 0x00176138 File Offset: 0x00174338
			protected override void OnTimedOut()
			{
				this.OnQuestFailed(true);
			}

			// Token: 0x06005254 RID: 21076 RVA: 0x00176144 File Offset: 0x00174344
			protected override void RegisterEvents()
			{
				CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
				CampaignEvents.OnHideoutDeactivatedEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnHideoutCleared));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			}

			// Token: 0x06005255 RID: 21077 RVA: 0x00176196 File Offset: 0x00174396
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x06005256 RID: 21078 RVA: 0x001761A9 File Offset: 0x001743A9
			private void OnHideoutCleared(Settlement hideout)
			{
				if (this._targetHideout == hideout)
				{
					base.CompleteQuestWithCancel(null);
				}
			}

			// Token: 0x06005257 RID: 21079 RVA: 0x001761BC File Offset: 0x001743BC
			private void OnMapEventEnded(MapEvent mapEvent)
			{
				if (mapEvent.IsHideoutBattle && mapEvent.MapEventSettlement == this._targetHideout)
				{
					if (mapEvent.InvolvedParties.Contains(PartyBase.MainParty))
					{
						if (mapEvent.BattleState == BattleState.DefenderVictory)
						{
							this.OnQuestFailed(false);
							return;
						}
						if (mapEvent.BattleState == BattleState.AttackerVictory)
						{
							this.OnQuestSucceeded();
							return;
						}
					}
					else if (mapEvent.BattleState == BattleState.AttackerVictory)
					{
						this.OnQuestCanceled();
					}
				}
			}

			// Token: 0x04001ACC RID: 6860
			private const int QuestGiverRelationBonus = 5;

			// Token: 0x04001ACD RID: 6861
			private const int QuestGiverRelationPenalty = -5;

			// Token: 0x04001ACE RID: 6862
			private const int QuestGiverPowerBonus = 5;

			// Token: 0x04001ACF RID: 6863
			private const int QuestGiverPowerPenalty = -5;

			// Token: 0x04001AD0 RID: 6864
			private const int TownProsperityBonus = 10;

			// Token: 0x04001AD1 RID: 6865
			private const int TownProsperityPenalty = -10;

			// Token: 0x04001AD2 RID: 6866
			private const int TownSecurityPenalty = -5;

			// Token: 0x04001AD3 RID: 6867
			private const int QuestGuid = 1056731;

			// Token: 0x04001AD4 RID: 6868
			[SaveableField(100)]
			private readonly Settlement _targetHideout;

			// Token: 0x04001AD5 RID: 6869
			[SaveableField(101)]
			private readonly Settlement _questSettlement;
		}

		// Token: 0x02000661 RID: 1633
		public class NearbyBanditBaseIssueTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x0600525A RID: 21082 RVA: 0x0017623F File Offset: 0x0017443F
			public NearbyBanditBaseIssueTypeDefiner() : base(400000)
			{
			}

			// Token: 0x0600525B RID: 21083 RVA: 0x0017624C File Offset: 0x0017444C
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssue), 1, null);
				base.AddClassDefinition(typeof(NearbyBanditBaseIssueBehavior.NearbyBanditBaseIssueQuest), 2, null);
			}
		}
	}
}
