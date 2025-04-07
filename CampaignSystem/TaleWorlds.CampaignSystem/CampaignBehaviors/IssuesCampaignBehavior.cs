using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003A2 RID: 930
	public class IssuesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060037A5 RID: 14245 RVA: 0x000FB540 File Offset: 0x000F9740
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.OnIssueUpdatedEvent.AddNonSerializedListener(this, new Action<IssueBase, IssueBase.IssueUpdateDetails, Hero>(this.OnIssueUpdated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x000FB5D8 File Offset: 0x000F97D8
		private Settlement CurrentTickSettlement
		{
			get
			{
				CampaignTime campaignTime = new CampaignTime(CampaignTime.Days(1f).NumTicks / (long)this._settlements.Length);
				int num = (int)(CampaignTime.Now.NumTicks / campaignTime.NumTicks) % this._settlements.Length;
				return this._settlements[num];
			}
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x000FB630 File Offset: 0x000F9830
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			Settlement[] array = (from x in Village.All
			select x.Settlement).ToArray<Settlement>();
			int num = MathF.Ceiling(0.7f * (float)array.Length);
			Settlement[] array2 = (from x in Town.AllTowns
			select x.Settlement).ToArray<Settlement>();
			int num2 = MathF.Ceiling(0.8f * (float)array2.Length);
			int num3 = Hero.AllAliveHeroes.Count((Hero x) => x.IsLord && x.Clan != null && !x.Clan.IsBanditFaction && !x.IsChild);
			int num4 = MathF.Ceiling(0.120000005f * (float)num3);
			int totalDesiredIssueCount = num + num2 + num4;
			Campaign.Current.ConversationManager.DisableSentenceSort();
			this._additionalFrequencyScore = -0.4f;
			array.Shuffle<Settlement>();
			this.CreateRandomSettlementIssues(array, 2, num, totalDesiredIssueCount);
			array2.Shuffle<Settlement>();
			this.CreateRandomSettlementIssues(array2, 3, num2, totalDesiredIssueCount);
			Clan[] array3 = (from x in Clan.NonBanditFactions
			where x.Heroes.Count != 0
			select x).ToArray<Clan>();
			array3.Shuffle<Clan>();
			this.CreateRandomClanIssues(array3, num4, totalDesiredIssueCount);
			this._additionalFrequencyScore = 0.2f;
			Campaign.Current.ConversationManager.EnableSentenceSort();
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x000FB798 File Offset: 0x000F9998
		private void OnSettlementTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			Settlement currentTickSettlement = this.CurrentTickSettlement;
			int num = Campaign.Current.IssueManager.Issues.Count((KeyValuePair<Hero, IssueBase> x) => !x.Value.IsTriedToSolveBefore);
			int num2 = currentTickSettlement.HeroesWithoutParty.Count((Hero x) => x.Issue != null);
			int num3 = currentTickSettlement.IsTown ? 1 : 1;
			int num4 = currentTickSettlement.IsTown ? 3 : 2;
			if (num4 > 0 && num2 < num4 && (num2 < num3 || MBRandom.RandomFloat < this.GetIssueGenerationChance(num2, num4)))
			{
				this.CreateAnIssueForSettlementNotables(currentTickSettlement, num + 1);
			}
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x000FB850 File Offset: 0x000F9A50
		private void DailyTickClan(Clan clan)
		{
			if (this.IsClanSuitableForIssueCreation(clan))
			{
				int num = Campaign.Current.IssueManager.Issues.Count((KeyValuePair<Hero, IssueBase> x) => !x.Value.IsTriedToSolveBefore);
				int num2 = clan.Heroes.Count((Hero x) => x.Issue != null);
				int num3 = clan.Heroes.Count((Hero x) => x.IsAlive && !x.IsChild && x.IsLord);
				int num4 = MathF.Ceiling((float)num3 * 0.1f);
				int num5 = MathF.Floor((float)num3 * 0.2f);
				if (num5 > 0 && num2 < num5 && (num2 < num4 || MBRandom.RandomFloat < this.GetIssueGenerationChance(num2, num5)))
				{
					this.CreateAnIssueForClanNobles(clan, num + 1);
				}
			}
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x000FB933 File Offset: 0x000F9B33
		private bool IsClanSuitableForIssueCreation(Clan clan)
		{
			return clan.Heroes.Count > 0 && !clan.IsBanditFaction;
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x000FB950 File Offset: 0x000F9B50
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			this._additionalFrequencyScore = 0.2f;
			List<IssueBase> list = new List<IssueBase>();
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
			{
				if (keyValuePair.Key.IsNotable && keyValuePair.Key.CurrentSettlement == null)
				{
					list.Add(keyValuePair.Value);
				}
			}
			foreach (IssueBase issueBase in list)
			{
				issueBase.CompleteIssueWithCancel(null);
			}
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x000FBA1C File Offset: 0x000F9C1C
		private float GetIssueGenerationChance(int currentIssueCount, int maxIssueCount)
		{
			float num = 1f - (float)currentIssueCount / (float)maxIssueCount;
			return 0.3f * num * num;
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x000FBA40 File Offset: 0x000F9C40
		private void CreateRandomSettlementIssues(Settlement[] shuffledSettlementArray, int maxIssueCountPerSettlement, int desiredIssueCount, int totalDesiredIssueCount)
		{
			int num = shuffledSettlementArray.Length;
			int[] array = new int[num];
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			while (num2 < num && num4 < desiredIssueCount)
			{
				int num6 = (num4 + num2 + num3) % num;
				if (array[num6] < num5)
				{
					num3++;
				}
				else if (array[num6] < maxIssueCountPerSettlement && this.CreateAnIssueForSettlementNotables(shuffledSettlementArray[num6], totalDesiredIssueCount))
				{
					num4++;
					array[num6]++;
				}
				else
				{
					num2++;
				}
			}
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x000FBAB4 File Offset: 0x000F9CB4
		private void CreateRandomClanIssues(Clan[] shuffledClanArray, int desiredIssueCount, int totalDesiredIssueCount)
		{
			int num = shuffledClanArray.Length;
			int num2 = 0;
			int num3 = 0;
			while (num2 < num && num3 < desiredIssueCount)
			{
				if (this.CreateAnIssueForClanNobles(shuffledClanArray[(num3 + num2) % num], totalDesiredIssueCount))
				{
					num3++;
				}
				else
				{
					num2++;
				}
			}
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x000FBAF0 File Offset: 0x000F9CF0
		private bool CreateAnIssueForSettlementNotables(Settlement settlement, int totalDesiredIssueCount)
		{
			List<IssuesCampaignBehavior.IssueData> list = new List<IssuesCampaignBehavior.IssueData>();
			IssueManager issueManager = Campaign.Current.IssueManager;
			foreach (Hero hero in settlement.Notables)
			{
				if (hero.Issue == null && hero.CanHaveQuestsOrIssues())
				{
					List<PotentialIssueData> list2 = Campaign.Current.IssueManager.CheckForIssues(hero);
					int totalFrequencyScore = list2.SumQ((PotentialIssueData x) => this.GetFrequencyScore(x.Frequency));
					foreach (PotentialIssueData issueData in list2)
					{
						if (issueData.IsValid)
						{
							float num = this.CalculateIssueScoreForNotable(issueData, settlement, totalDesiredIssueCount, totalFrequencyScore);
							if (num > 0f && !issueManager.HasIssueCoolDown(issueData.IssueType, hero))
							{
								list.Add(new IssuesCampaignBehavior.IssueData(issueData, hero, num));
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				List<ValueTuple<IssuesCampaignBehavior.IssueData, float>> list3 = new List<ValueTuple<IssuesCampaignBehavior.IssueData, float>>();
				foreach (IssuesCampaignBehavior.IssueData issueData2 in list)
				{
					list3.Add(new ValueTuple<IssuesCampaignBehavior.IssueData, float>(issueData2, issueData2.Score));
				}
				IssuesCampaignBehavior.IssueData issueData3 = MBRandom.ChooseWeighted<IssuesCampaignBehavior.IssueData>(list3);
				Campaign.Current.IssueManager.CreateNewIssue(issueData3.PotentialIssueData, issueData3.Hero);
				return true;
			}
			return false;
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x000FBC8C File Offset: 0x000F9E8C
		private bool CreateAnIssueForClanNobles(Clan clan, int totalDesiredIssueCount)
		{
			List<IssuesCampaignBehavior.IssueData> list = new List<IssuesCampaignBehavior.IssueData>();
			IssueManager issueManager = Campaign.Current.IssueManager;
			foreach (Hero hero in clan.Lords)
			{
				if (hero.Clan != Clan.PlayerClan && hero.CanHaveQuestsOrIssues() && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && (hero.IsActive || hero.IsPrisoner) && hero.Issue == null)
				{
					List<PotentialIssueData> list2 = Campaign.Current.IssueManager.CheckForIssues(hero);
					int totalFrequencyScore = list2.SumQ((PotentialIssueData x) => this.GetFrequencyScore(x.Frequency));
					foreach (PotentialIssueData issueData in list2)
					{
						if (issueData.IsValid)
						{
							float num = this.CalculateIssueScoreForClan(issueData, clan, totalDesiredIssueCount, totalFrequencyScore);
							if (num > 0f && !issueManager.HasIssueCoolDown(issueData.IssueType, hero))
							{
								list.Add(new IssuesCampaignBehavior.IssueData(issueData, hero, num));
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				IssuesCampaignBehavior.IssueData issueData2 = (from x in list
				orderby x.Score descending
				select x).First<IssuesCampaignBehavior.IssueData>();
				Campaign.Current.IssueManager.CreateNewIssue(issueData2.PotentialIssueData, issueData2.Hero);
				return true;
			}
			return false;
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x000FBE54 File Offset: 0x000FA054
		private float CalculateIssueScoreForClan(in PotentialIssueData pid, Clan clan, int totalDesiredIssueCount, int totalFrequencyScore)
		{
			foreach (Hero hero in clan.Heroes)
			{
				if (hero.Issue != null)
				{
					Type type = hero.Issue.GetType();
					PotentialIssueData potentialIssueData = pid;
					if (type == potentialIssueData.IssueType)
					{
						return 0f;
					}
				}
			}
			return this.CalculateIssueScoreInternal(pid, totalDesiredIssueCount, totalFrequencyScore);
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x000FBEDC File Offset: 0x000FA0DC
		private float CalculateIssueScoreForNotable(in PotentialIssueData pid, Settlement settlement, int totalDesiredIssueCount, int totalFrequencyScore)
		{
			foreach (Hero hero in settlement.Notables)
			{
				if (hero.Issue != null)
				{
					Type type = hero.Issue.GetType();
					PotentialIssueData potentialIssueData = pid;
					if (type == potentialIssueData.IssueType)
					{
						return 0f;
					}
				}
			}
			return this.CalculateIssueScoreInternal(pid, totalDesiredIssueCount, totalFrequencyScore);
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x000FBF64 File Offset: 0x000FA164
		private float CalculateIssueScoreInternal(in PotentialIssueData pid, int totalDesiredIssueCount, int totalFrequencyScore)
		{
			PotentialIssueData potentialIssueData = pid;
			float num = (float)this.GetFrequencyScore(potentialIssueData.Frequency) / (float)totalFrequencyScore;
			float num2;
			if (totalDesiredIssueCount == 0)
			{
				num2 = 1f;
			}
			else
			{
				int num3 = 0;
				foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
				{
					Type type = keyValuePair.Value.GetType();
					potentialIssueData = pid;
					if (type == potentialIssueData.IssueType)
					{
						num3++;
					}
				}
				num2 = (float)num3 / (float)totalDesiredIssueCount;
			}
			float num4 = 1f + this._additionalFrequencyScore - num2 / num;
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			else if (num4 < this._additionalFrequencyScore)
			{
				num4 *= 0.01f;
			}
			else if (num4 < this._additionalFrequencyScore + 0.4f)
			{
				num4 *= 0.1f;
			}
			return num * num4;
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x000FC060 File Offset: 0x000FA260
		private int GetFrequencyScore(IssueBase.IssueFrequency frequency)
		{
			int result = 0;
			switch (frequency)
			{
			case IssueBase.IssueFrequency.VeryCommon:
				result = 6;
				break;
			case IssueBase.IssueFrequency.Common:
				result = 3;
				break;
			case IssueBase.IssueFrequency.Rare:
				result = 1;
				break;
			}
			return result;
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x000FC090 File Offset: 0x000FA290
		private void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CharacterObject characterObject;
			if (party == null)
			{
				characterObject = hero.CharacterObject;
			}
			else
			{
				Hero leaderHero = party.LeaderHero;
				characterObject = ((leaderHero != null) ? leaderHero.CharacterObject : null);
			}
			CharacterObject characterObject2 = characterObject;
			if (characterObject2 != null && !characterObject2.IsPlayerCharacter && ((party != null) ? party.Army : null) == null && Campaign.Current.GameStarted)
			{
				MBList<IssueBase> mblist = IssueManager.GetIssuesInSettlement(settlement, true).ToMBList<IssueBase>();
				float num = (settlement.OwnerClan == characterObject2.HeroObject.Clan) ? 0.05f : 0.01f;
				if (mblist.Count > 0 && MBRandom.RandomFloat < num)
				{
					IssueBase randomElement = mblist.GetRandomElement<IssueBase>();
					if (randomElement.CanBeCompletedByAI() && randomElement.IsOngoingWithoutQuest)
					{
						randomElement.CompleteIssueWithAiLord(characterObject2.HeroObject);
					}
				}
			}
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x000FC144 File Offset: 0x000FA344
		private void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver = null)
		{
			if (details == IssueBase.IssueUpdateDetails.IssueFinishedWithSuccess && issueSolver != null && issueSolver.GetPerkValue(DefaultPerks.Charm.Oratory))
			{
				GainRenownAction.Apply(issueSolver, (float)MathF.Round(DefaultPerks.Charm.Oratory.PrimaryBonus), false);
				GainKingdomInfluenceAction.ApplyForDefault(issueSolver, (float)MathF.Round(DefaultPerks.Charm.Oratory.PrimaryBonus));
			}
			if ((details == IssueBase.IssueUpdateDetails.IssueFail || details == IssueBase.IssueUpdateDetails.IssueFinishedWithSuccess || details == IssueBase.IssueUpdateDetails.IssueFinishedWithBetrayal || details == IssueBase.IssueUpdateDetails.IssueTimedOut || details == IssueBase.IssueUpdateDetails.SentTroopsFinishedQuest) && issueSolver != null && issue.IssueOwner != null)
			{
				int num = issue.IsSolvingWithQuest ? issue.IssueQuest.RelationshipChangeWithQuestGiver : issue.RelationshipChangeWithIssueOwner;
				if (num > 0)
				{
					if (issueSolver.GetPerkValue(DefaultPerks.Trade.DistributedGoods) && issue.IssueOwner.IsArtisan)
					{
						num *= (int)DefaultPerks.Trade.DistributedGoods.PrimaryBonus;
					}
					if (issueSolver.GetPerkValue(DefaultPerks.Trade.LocalConnection) && issue.IssueOwner.IsMerchant)
					{
						num *= (int)DefaultPerks.Trade.LocalConnection.PrimaryBonus;
					}
					ChangeRelationAction.ApplyPlayerRelation(issue.IsSolvingWithQuest ? issue.IssueQuest.QuestGiver : issue.IssueOwner, num, true, true);
				}
				else if (num < 0)
				{
					ChangeRelationAction.ApplyPlayerRelation(issue.IsSolvingWithQuest ? issue.IssueQuest.QuestGiver : issue.IssueOwner, num, true, true);
				}
			}
			if (details == IssueBase.IssueUpdateDetails.IssueCancel || details == IssueBase.IssueUpdateDetails.IssueFail || details == IssueBase.IssueUpdateDetails.IssueFinishedWithSuccess || details == IssueBase.IssueUpdateDetails.IssueFinishedWithBetrayal || details == IssueBase.IssueUpdateDetails.IssueTimedOut || details == IssueBase.IssueUpdateDetails.SentTroopsFinishedQuest || details == IssueBase.IssueUpdateDetails.SentTroopsFailedQuest || details == IssueBase.IssueUpdateDetails.IssueFinishedByAILord)
			{
				Campaign.Current.IssueManager.AddIssueCoolDownData(issue.GetType(), new HeroRelatedIssueCoolDownData(issue.IssueOwner, CampaignTime.DaysFromNow((float)Campaign.Current.Models.IssueModel.IssueOwnerCoolDownInDays)));
			}
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x000FC2D3 File Offset: 0x000FA4D3
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x000FC2D8 File Offset: 0x000FA4D8
		private void OnSessionLaunched(CampaignGameStarter starter)
		{
			List<Settlement> list = (from x in Settlement.All
			where x.IsTown || x.IsVillage
			select x).ToList<Settlement>();
			this.DeterministicShuffle(list);
			this._settlements = list.ToArray();
			CampaignTime campaignTime = new CampaignTime(CampaignTime.Days(1f).NumTicks / (long)this._settlements.Length);
			CampaignTime initialWait = campaignTime - new CampaignTime(CampaignTime.Now.NumTicks % campaignTime.NumTicks);
			this._periodicEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(campaignTime, initialWait);
			this._periodicEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.OnSettlementTick));
			this.AddDialogues(starter);
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x000FC398 File Offset: 0x000FA598
		private void DeterministicShuffle(List<Settlement> settlements)
		{
			Random random = new Random(53);
			for (int i = 0; i < settlements.Count; i++)
			{
				int index = random.Next() % settlements.Count;
				Settlement value = settlements[i];
				settlements[i] = settlements[index];
				settlements[index] = value;
			}
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x000FC3EC File Offset: 0x000FA5EC
		private void AddDialogues(CampaignGameStarter starter)
		{
			starter.AddDialogLine("issue_not_offered", "issue_offer", "hero_main_options", "{=!}{ISSUE_NOT_OFFERED_EXPLANATION}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_not_offered_condition), new ConversationSentence.OnConsequenceDelegate(this.leave_on_conversation_end_consequence), 100, null);
			starter.AddDialogLine("issue_explanation", "issue_offer", "issue_explanation_player_response", "{=!}{IssueBriefByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offered_begin_condition), new ConversationSentence.OnConsequenceDelegate(this.leave_on_conversation_end_consequence), 100, null);
			starter.AddPlayerLine("issue_explanation_player_response_pre_lord_solution", "issue_explanation_player_response", "issue_lord_solution_brief", "{=!}{IssueAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(this.issue_explanation_player_response_pre_lord_solution_condition), null, 100, null, null);
			starter.AddPlayerLine("issue_explanation_player_response_pre_quest_solution", "issue_explanation_player_response", "issue_quest_solution_brief", "{=!}{IssueAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(this.issue_explanation_player_response_pre_quest_solution_condition), null, 100, null, null);
			starter.AddDialogLine("issue_lord_solution_brief", "issue_lord_solution_brief", "issue_lord_solution_player_response", "{=!}{IssueLordSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_lord_solution_brief_condition), null, 100, null);
			starter.AddPlayerLine("issue_lord_solution_player_response", "issue_lord_solution_player_response", "issue_quest_solution_brief", "{=!}{IssuePlayerResponseAfterLordExplanationText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_lord_solution_player_response_condition), null, 100, null, null);
			starter.AddDialogLine("issue_quest_solution_brief_pre_alternative_solution", "issue_quest_solution_brief", "issue_alternative_solution_player_response", "{=!}{IssueQuestSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_quest_solution_brief_pre_alternative_solution_condition), null, 100, null);
			starter.AddDialogLine("issue_quest_solution_brief_pre_player_response", "issue_quest_solution_brief", "issue_offer_player_response", "{=!}{IssueQuestSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_quest_solution_brief_pre_player_response_condition), null, 100, null);
			starter.AddPlayerLine("issue_alternative_solution_player_response", "issue_alternative_solution_player_response", "issue_alternative_solution_brief", "{=!}{IssuePlayerResponseAfterAlternativeExplanationText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_alternative_solution_player_response_condition), null, 100, null, null);
			starter.AddDialogLine("issue_alternative_solution_brief", "issue_alternative_solution_brief", "issue_offer_player_response", "{=!}{IssueAlternativeSolutionExplanationByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_alternative_solution_brief_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_2_consequence), 100, null);
			starter.AddPlayerLine("issue_offer_player_accept_quest", "issue_offer_player_response", "issue_classic_quest_start", "{=!}{IssueQuestSolutionAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_quest_condition), delegate
			{
				Campaign.Current.IssueManager.StartIssueQuest(Hero.OneToOneConversationHero);
			}, 100, null, null);
			starter.AddPlayerLine("issue_offer_player_accept_alternative", "issue_offer_player_response", "issue_offer_player_accept_alternative_2", "{=!}{IssueAlternativeSolutionAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_condition), null, 100, new ConversationSentence.OnClickableConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_clickable_condition), null);
			starter.AddPlayerLine("issue_offer_player_accept_lord", "issue_offer_player_response", "issue_offer_player_accept_lord_2", "{=!}{IssueLordSolutionAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(this.issue_offer_player_accept_lord_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_lord_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_lord_clickable_condition), null);
			starter.AddPlayerLine("issue_offer_player_response_reject", "issue_offer_player_response", "issue_offer_hero_response_reject", "{=l549ODcw}Sorry. I can't do that right now.", null, null, 100, null, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_2", "issue_offer_player_accept_alternative_2", "issue_offer_player_accept_alternative_3", "{=X4ITSQOl}Which of your people can help us?", null, null, 100, null);
			starter.AddRepeatablePlayerLine("issue_offer_player_accept_alternative_3", "issue_offer_player_accept_alternative_3", "issue_offer_player_accept_alternative_4", "{=C2ZGNwwh}{COMPANION.NAME} {COMPANION_SCALED_PARAMETERS}", "{=nomZx5Nw}I am thinking of a different companion.", "issue_offer_player_accept_alternative_2", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_3_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_3_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_3_clickable_condition));
			starter.AddPlayerLine("issue_offer_player_accept_go_back", "issue_offer_player_accept_alternative_3", "issue_offer_hero_response_reject", "{=OymJQD7M}Actually, I don't have any available men right now...", null, null, 100, null, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_4", "issue_offer_player_accept_alternative_4", "issue_offer_player_accept_alternative_5", "{=!}Party screen goes here", null, new ConversationSentence.OnConsequenceDelegate(this.issue_offer_player_accept_alternative_4_consequence), 100, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_5_a", "issue_offer_player_accept_alternative_5", "close_window", "{=!}{IssueAlternativeSolutionResponseByIssueGiverText}", new ConversationSentence.OnConditionDelegate(this.issue_offer_player_accept_alternative_5_a_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_5_a_consequence), 100, null);
			starter.AddDialogLine("issue_offer_player_accept_alternative_5_b", "issue_offer_player_accept_alternative_5", "issue_offer_player_response", "{=!}{IssueGiverResponseToRejection}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_hero_response_reject_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_offer_player_accept_alternative_5_b_consequence), 100, null);
			starter.AddPlayerLine("issue_offer_player_back", "issue_offer_player_accept_alternative_5", "issue_offer_player_response", GameTexts.FindText("str_back", null).ToString(), null, null, 100, null, null);
			starter.AddDialogLine("issue_offer_player_accept_lord_2", "issue_offer_player_accept_lord_2", "hero_main_options", "{=!}{IssueLordSolutionResponseByIssueGiverText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_player_accept_lord_2_condition), null, 100, null);
			starter.AddDialogLine("issue_offer_hero_response_reject", "issue_offer_hero_response_reject", "hero_main_options", "{=!}{IssueGiverResponseToRejection}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_offer_hero_response_reject_condition), null, 100, null);
			starter.AddDialogLine("issue_counter_offer_1", "start", "issue_counter_offer_2", "{=!}{IssueLordSolutionCounterOfferBriefByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_start_condition), null, int.MaxValue, null);
			starter.AddDialogLine("issue_counter_offer_2", "issue_counter_offer_2", "issue_counter_offer_player_response", "{=!}{IssueLordSolutionCounterOfferExplanationByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_2_condition), null, 100, null);
			starter.AddPlayerLine("issue_counter_offer_player_accept", "issue_counter_offer_player_response", "issue_counter_offer_accepted", "{=!}{IssueLordSolutionCounterOfferAcceptByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_player_accept_condition), null, 100, null, null);
			starter.AddDialogLine("issue_counter_offer_accepted", "issue_counter_offer_accepted", "close_window", "{=!}{IssueLordSolutionCounterOfferAcceptResponseByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_accepted_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_counter_offer_accepted_consequence), 100, null);
			starter.AddPlayerLine("issue_counter_offer_player_reject", "issue_counter_offer_player_response", "issue_counter_offer_reject", "{=!}{IssueLordSolutionCounterOfferDeclineByPlayerText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_player_reject_condition), null, 100, null, null);
			starter.AddDialogLine("issue_counter_offer_reject", "issue_counter_offer_reject", "close_window", "{=!}{IssueLordSolutionCounterOfferDeclineResponseByOtherNpcText}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_counter_offer_reject_condition), new ConversationSentence.OnConsequenceDelegate(IssuesCampaignBehavior.issue_counter_offer_reject_consequence), 100, null);
			starter.AddDialogLine("issue_alternative_solution_discuss", "issue_discuss_alternative_solution", "close_window", "{=!}{IssueDiscussAlternativeSolution}", new ConversationSentence.OnConditionDelegate(IssuesCampaignBehavior.issue_alternative_solution_discussion_condition), new ConversationSentence.OnConsequenceDelegate(this.issue_alternative_solution_discussion_consequence), int.MaxValue, null);
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000FC9AC File Offset: 0x000FABAC
		private static bool issue_alternative_solution_discussion_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (issueOwnersIssue != null && issueOwnersIssue.IsThereAlternativeSolution && issueOwnersIssue.IsSolvingWithAlternative)
			{
				MBTextManager.SetTextVariable("IssueDiscussAlternativeSolution", issueOwnersIssue.IssueDiscussAlternativeSolution, false);
				return true;
			}
			return false;
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x000FC9E6 File Offset: 0x000FABE6
		private void issue_alternative_solution_discussion_consequence()
		{
			if (PlayerEncounter.Current != null && Campaign.Current.ConversationManager.ConversationParty == PlayerEncounter.EncounteredMobileParty)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x000FCA0C File Offset: 0x000FAC0C
		private static void issue_counter_offer_reject_consequence()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			Campaign.Current.ConversationManager.ConversationEndOneShot += counterOfferersIssue.CompleteIssueWithLordSolutionWithRefuseCounterOffer;
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x000FCA3C File Offset: 0x000FAC3C
		private static bool issue_counter_offer_reject_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferDeclineResponseByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferDeclineResponseByOtherNpc, false);
			return true;
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x000FCA64 File Offset: 0x000FAC64
		private static bool issue_counter_offer_player_reject_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferDeclineByPlayerText", counterOfferersIssue.IssueLordSolutionCounterOfferDeclineByPlayer, false);
			return true;
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x000FCA8C File Offset: 0x000FAC8C
		private static void issue_counter_offer_accepted_consequence()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			Campaign.Current.ConversationManager.ConversationEndOneShot += counterOfferersIssue.CompleteIssueWithLordSolutionWithAcceptCounterOffer;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x000FCABC File Offset: 0x000FACBC
		private static bool issue_counter_offer_accepted_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferAcceptResponseByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferAcceptResponseByOtherNpc, false);
			return true;
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x000FCAE4 File Offset: 0x000FACE4
		private static bool issue_counter_offer_player_accept_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferAcceptByPlayerText", counterOfferersIssue.IssueLordSolutionCounterOfferAcceptByPlayer, false);
			return true;
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x000FCB0C File Offset: 0x000FAD0C
		private static bool issue_counter_offer_2_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferExplanationByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferExplanationByOtherNpc, false);
			return true;
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000FCB34 File Offset: 0x000FAD34
		private static bool issue_counter_offer_start_condition()
		{
			IssueBase counterOfferersIssue = IssuesCampaignBehavior.GetCounterOfferersIssue();
			if (counterOfferersIssue != null)
			{
				MBTextManager.SetTextVariable("IssueLordSolutionCounterOfferBriefByOtherNpcText", counterOfferersIssue.IssueLordSolutionCounterOfferBriefByOtherNpc, false);
				return true;
			}
			return false;
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x000FCB60 File Offset: 0x000FAD60
		private static bool issue_offer_player_accept_lord_2_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionResponseByIssueGiverText", issueOwnersIssue.IssueLordSolutionResponseByIssueGiver, false);
			return true;
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x000FCB88 File Offset: 0x000FAD88
		private void issue_offer_player_accept_alternative_4_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			int totalAlternativeSolutionNeededMenCount = issueOwnersIssue.GetTotalAlternativeSolutionNeededMenCount();
			if (totalAlternativeSolutionNeededMenCount > 1)
			{
				PartyScreenManager.OpenScreenAsQuest(issueOwnersIssue.AlternativeSolutionSentTroops, new TextObject("{=FbLOFO88}Select troops for mission", null), totalAlternativeSolutionNeededMenCount + 1, issueOwnersIssue.GetTotalAlternativeSolutionDurationInDays(), new PartyPresentationDoneButtonConditionDelegate(this.PartyScreenDoneCondition), new PartyScreenClosedDelegate(IssuesCampaignBehavior.PartyScreenDoneClicked), new IsTroopTransferableDelegate(IssuesCampaignBehavior.TroopTransferableDelegate), null);
				return;
			}
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x000FCBFC File Offset: 0x000FADFC
		private static void issue_offer_player_accept_alternative_5_b_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MobileParty.MainParty.MemberRoster.Add(issueOwnersIssue.AlternativeSolutionSentTroops);
			issueOwnersIssue.AlternativeSolutionSentTroops.Clear();
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000FCC2F File Offset: 0x000FAE2F
		private static void issue_offer_player_accept_alternative_5_a_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			issueOwnersIssue.AlternativeSolutionStartConsequence();
			issueOwnersIssue.StartIssueWithAlternativeSolution();
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000FCC44 File Offset: 0x000FAE44
		private bool issue_offer_player_accept_alternative_5_a_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAlternativeSolutionResponseByIssueGiverText", issueOwnersIssue.IssueAlternativeSolutionResponseByIssueGiver, false);
			TextObject textObject;
			return IssuesCampaignBehavior.DoTroopsSatisfyAlternativeSolutionInternal(issueOwnersIssue.AlternativeSolutionSentTroops, out textObject);
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x000FCC78 File Offset: 0x000FAE78
		private static bool issue_offer_player_accept_alternative_3_clickable_condition(out TextObject explanation)
		{
			bool result = true;
			explanation = TextObject.Empty;
			Hero hero = ConversationSentence.CurrentProcessedRepeatObject as Hero;
			if (hero == null || hero.PartyBelongedTo != MobileParty.MainParty)
			{
				result = false;
			}
			else if (!hero.CanHaveQuestsOrIssues())
			{
				explanation = new TextObject("{=DBabgrcC}This hero is not available right now.", null);
				result = false;
			}
			else if (hero.IsWounded)
			{
				explanation = new TextObject("{=CyrOuz4h}This hero is wounded.", null);
				result = false;
			}
			else if (hero.IsPregnant)
			{
				explanation = new TextObject("{=BaKOWJb6}This hero is pregnant.", null);
				result = false;
			}
			return result;
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x000FCCF8 File Offset: 0x000FAEF8
		private static void issue_offer_player_accept_alternative_3_consequence()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			Hero hero = ConversationSentence.SelectedRepeatObject as Hero;
			if (hero != null)
			{
				MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, -1, false, 0, 0, true, -1);
				issueOwnersIssue.AlternativeSolutionSentTroops.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
				CampaignEventDispatcher.Instance.OnHeroGetsBusy(hero, HeroGetsBusyReasons.SolvesIssue);
			}
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x000FCD5C File Offset: 0x000FAF5C
		private static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase leftOwnerParty)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			return !character.IsHero && !character.IsNotTransferableInPartyScreen && type != PartyScreenLogic.TroopType.Prisoner && issueOwnersIssue.IsTroopTypeNeededByAlternativeSolution(character);
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000FCD8C File Offset: 0x000FAF8C
		private static void PartyScreenDoneClicked(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000FCDA0 File Offset: 0x000FAFA0
		private Tuple<bool, TextObject> PartyScreenDoneCondition(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			TextObject item;
			return new Tuple<bool, TextObject>(IssuesCampaignBehavior.DoTroopsSatisfyAlternativeSolutionInternal(leftMemberRoster, out item), item);
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000FCDBC File Offset: 0x000FAFBC
		private static bool DoTroopsSatisfyAlternativeSolutionInternal(TroopRoster troopRoster, out TextObject explanation)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			explanation = TextObject.Empty;
			int totalAlternativeSolutionNeededMenCount = issueOwnersIssue.GetTotalAlternativeSolutionNeededMenCount();
			if (troopRoster.TotalRegulars >= totalAlternativeSolutionNeededMenCount && troopRoster.TotalRegulars - troopRoster.TotalWoundedRegulars < totalAlternativeSolutionNeededMenCount)
			{
				explanation = new TextObject("{=fjmGXcLW}You have to send healthy troops to this quest.", null);
				return false;
			}
			return issueOwnersIssue.DoTroopsSatisfyAlternativeSolution(troopRoster, out explanation);
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000FCE10 File Offset: 0x000FB010
		private static bool issue_offer_player_accept_alternative_3_condition()
		{
			Hero hero = ConversationSentence.CurrentProcessedRepeatObject as Hero;
			if (hero != null)
			{
				StringHelpers.SetRepeatableCharacterProperties("COMPANION", hero.CharacterObject, false);
			}
			List<TextObject> list = new List<TextObject>();
			IssueModel issueModel = Campaign.Current.Models.IssueModel;
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			bool flag = false;
			if (issueOwnersIssue.AlternativeSolutionHasCasualties)
			{
				ValueTuple<int, int> causalityForHero = issueModel.GetCausalityForHero(hero, issueOwnersIssue);
				if (causalityForHero.Item2 > 0)
				{
					TextObject textObject = TextObject.Empty;
					if (causalityForHero.Item1 == causalityForHero.Item2)
					{
						textObject = new TextObject("{=zPlFvCRm}{NUMBER_OF_TROOPS} troop loss", null);
						textObject.SetTextVariable("NUMBER_OF_TROOPS", causalityForHero.Item1);
					}
					else
					{
						textObject = new TextObject("{=bdlomGZ1}{MIN_NUMBER_OF_TROOPS} - {MAX_NUMBER_OF_TROOPS_LOST} troop loss", null);
						textObject.SetTextVariable("MIN_NUMBER_OF_TROOPS", causalityForHero.Item1);
						textObject.SetTextVariable("MAX_NUMBER_OF_TROOPS_LOST", causalityForHero.Item2);
					}
					flag = true;
					list.Add(textObject);
				}
			}
			if (issueOwnersIssue.AlternativeSolutionHasFailureRisk)
			{
				float num = issueModel.GetFailureRiskForHero(hero, issueOwnersIssue);
				if (num > 0f)
				{
					num = (float)((int)(num * 100f));
					TextObject textObject2 = new TextObject("{=9tLYXGGc}{FAILURE_RISK}% risk of failure", null);
					textObject2.SetTextVariable("FAILURE_RISK", num);
					list.Add(textObject2);
					flag = true;
				}
				else
				{
					TextObject item = new TextObject("{=way8jWK8}no risk of failure", null);
					list.Add(item);
				}
			}
			if (issueOwnersIssue.AlternativeSolutionHasScaledRequiredTroops)
			{
				int troopsRequiredForHero = issueModel.GetTroopsRequiredForHero(hero, issueOwnersIssue);
				if (troopsRequiredForHero > 0)
				{
					TextObject textObject3 = new TextObject("{=b3bJXMt2}{NUMBER_OF_TROOPS} required troops", null);
					textObject3.SetTextVariable("NUMBER_OF_TROOPS", troopsRequiredForHero);
					list.Add(textObject3);
					flag = true;
				}
			}
			if (issueOwnersIssue.AlternativeSolutionHasScaledDuration)
			{
				CampaignTime durationOfResolutionForHero = issueModel.GetDurationOfResolutionForHero(hero, issueOwnersIssue);
				if (durationOfResolutionForHero > CampaignTime.Days(0f))
				{
					TextObject textObject4 = new TextObject("{=ImatoO4Y}{DURATION_IN_DAYS} required days to complete", null);
					textObject4.SetTextVariable("DURATION_IN_DAYS", (float)durationOfResolutionForHero.ToDays);
					list.Add(textObject4);
					flag = true;
				}
			}
			if (flag)
			{
				ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = issueModel.GetIssueAlternativeSolutionSkill(hero, issueOwnersIssue);
				if (issueAlternativeSolutionSkill.Item1 != null)
				{
					TextObject textObject5 = new TextObject("{=!}{SKILL}: {NUMBER}", null);
					textObject5.SetTextVariable("SKILL", issueAlternativeSolutionSkill.Item1.Name);
					textObject5.SetTextVariable("NUMBER", hero.GetSkillValue(issueAlternativeSolutionSkill.Item1));
					list.Add(textObject5);
				}
			}
			if (list.IsEmpty<TextObject>())
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("COMPANION_SCALED_PARAMETERS", TextObject.Empty);
			}
			else
			{
				TextObject variable = GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, false);
				TextObject textObject6 = GameTexts.FindText("str_STR_in_parentheses", null);
				textObject6.SetTextVariable("STR", variable);
				ConversationSentence.SelectedRepeatLine.SetTextVariable("COMPANION_SCALED_PARAMETERS", textObject6);
			}
			return true;
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000FD0A0 File Offset: 0x000FB2A0
		private static void issue_offer_player_accept_alternative_2_consequence()
		{
			List<Hero> list = new List<Hero>();
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsHero && !troopRosterElement.Character.IsPlayerCharacter && troopRosterElement.Character.HeroObject.CanHaveQuestsOrIssues())
				{
					list.Add(troopRosterElement.Character.HeroObject);
				}
			}
			ConversationSentence.SetObjectsToRepeatOver(list, 5);
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000FD140 File Offset: 0x000FB340
		private static bool issue_offer_hero_response_reject_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.GetPersona() == DefaultTraits.PersonaCurt)
			{
				MBTextManager.SetTextVariable("IssueGiverResponseToRejection", new TextObject("{=h2Wle7ZI}Well. That's a pity.", null), false);
			}
			else if (CharacterObject.OneToOneConversationCharacter.GetPersona() == DefaultTraits.PersonaIronic)
			{
				MBTextManager.SetTextVariable("IssueGiverResponseToRejection", new TextObject("{=wbLnJrJA}Ah, well. I can look elsewhere for help, I suppose.", null), false);
			}
			else
			{
				MBTextManager.SetTextVariable("IssueGiverResponseToRejection", new TextObject("{=Uoy2tTZJ}Very well. But perhaps you will reconsider later.", null), false);
			}
			return true;
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000FD1B8 File Offset: 0x000FB3B8
		private static bool issue_offer_player_accept_lord_clickable_condition(out TextObject explanation)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (!issueOwnersIssue.LordSolutionCondition(out explanation))
			{
				return false;
			}
			if (Clan.PlayerClan.Influence < (float)issueOwnersIssue.NeededInfluenceForLordSolution)
			{
				explanation = new TextObject("{=hRdhfSs0}You don't have enough influence for this solution. ({NEEDED_INFLUENCE}{INFLUENCE_ICON})", null);
				explanation.SetTextVariable("NEEDED_INFLUENCE", issueOwnersIssue.NeededInfluenceForLordSolution);
				explanation.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"7\">");
				return false;
			}
			explanation = new TextObject("{=xbvgc8Sp}This solution will cost {INFLUENCE} influence.", null);
			explanation.SetTextVariable("INFLUENCE", issueOwnersIssue.NeededInfluenceForLordSolution);
			return true;
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000FD23E File Offset: 0x000FB43E
		private static void issue_offer_player_accept_lord_consequence()
		{
			Hero.OneToOneConversationHero.Issue.StartIssueWithLordSolution();
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x000FD250 File Offset: 0x000FB450
		private bool issue_offer_player_accept_lord_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (issueOwnersIssue.IsThereLordSolution)
			{
				MBTextManager.SetTextVariable("IssueLordSolutionAcceptByPlayerText", issueOwnersIssue.IssueLordSolutionAcceptByPlayer, false);
				return IssuesCampaignBehavior.IssueLordSolutionCondition();
			}
			return false;
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x000FD284 File Offset: 0x000FB484
		private static bool issue_offer_player_accept_alternative_clickable_condition(out TextObject explanation)
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if ((from m in MobileParty.MainParty.MemberRoster.GetTroopRoster()
			where m.Character.IsHero && !m.Character.IsPlayerCharacter && m.Character.HeroObject.CanHaveQuestsOrIssues()
			select m).IsEmpty<TroopRosterElement>())
			{
				explanation = new TextObject("{=qjpNREwg}You don't have any companions or family members.", null);
				return false;
			}
			if (!issueOwnersIssue.AlternativeSolutionCondition(out explanation))
			{
				return false;
			}
			explanation = TextObject.Empty;
			return true;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x000FD2F4 File Offset: 0x000FB4F4
		private static bool issue_offer_player_accept_alternative_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			if (issueOwnersIssue.IsThereAlternativeSolution)
			{
				MBTextManager.SetTextVariable("IssueAlternativeSolutionAcceptByPlayerText", issueOwnersIssue.IssueAlternativeSolutionAcceptByPlayer, false);
				return true;
			}
			return false;
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x000FD324 File Offset: 0x000FB524
		private static bool issue_offer_player_accept_quest_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueQuestSolutionAcceptByPlayerText", issueOwnersIssue.IssueQuestSolutionAcceptByPlayer, false);
			return true;
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x000FD34C File Offset: 0x000FB54C
		private static bool issue_alternative_solution_brief_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAlternativeSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueAlternativeSolutionExplanationByIssueGiver, false);
			return true;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000FD374 File Offset: 0x000FB574
		private static bool issue_alternative_solution_player_response_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssuePlayerResponseAfterAlternativeExplanationText", issueOwnersIssue.IssuePlayerResponseAfterAlternativeExplanation, false);
			return issueOwnersIssue.IsThereAlternativeSolution;
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x000FD3A0 File Offset: 0x000FB5A0
		private static bool issue_quest_solution_brief_pre_player_response_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueQuestSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueQuestSolutionExplanationByIssueGiver, false);
			return !issueOwnersIssue.IsThereAlternativeSolution;
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000FD3D0 File Offset: 0x000FB5D0
		private static bool issue_quest_solution_brief_pre_alternative_solution_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueQuestSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueQuestSolutionExplanationByIssueGiver, false);
			return issueOwnersIssue.IsThereAlternativeSolution;
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000FD3FC File Offset: 0x000FB5FC
		private static bool issue_lord_solution_player_response_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssuePlayerResponseAfterLordExplanationText", issueOwnersIssue.IssuePlayerResponseAfterLordExplanation, false);
			return true;
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000FD424 File Offset: 0x000FB624
		private static bool issue_lord_solution_brief_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueLordSolutionExplanationByIssueGiverText", issueOwnersIssue.IssueLordSolutionExplanationByIssueGiver, false);
			return true;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x000FD44C File Offset: 0x000FB64C
		private bool issue_explanation_player_response_pre_quest_solution_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAcceptByPlayerText", issueOwnersIssue.IssueAcceptByPlayer, false);
			return !issueOwnersIssue.IsThereLordSolution || !IssuesCampaignBehavior.IssueLordSolutionCondition();
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x000FD484 File Offset: 0x000FB684
		private bool issue_explanation_player_response_pre_lord_solution_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			MBTextManager.SetTextVariable("IssueAcceptByPlayerText", issueOwnersIssue.IssueAcceptByPlayer, false);
			return issueOwnersIssue.IsThereLordSolution && IssuesCampaignBehavior.IssueLordSolutionCondition();
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x000FD4B8 File Offset: 0x000FB6B8
		private static bool IssueLordSolutionCondition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			return issueOwnersIssue.IssueOwner.CurrentSettlement != null && issueOwnersIssue.IssueOwner.CurrentSettlement.OwnerClan == Clan.PlayerClan;
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x000FD4F4 File Offset: 0x000FB6F4
		private static bool issue_offered_begin_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			TextObject textObject;
			if (issueOwnersIssue != null && issueOwnersIssue.CheckPreconditions(Hero.OneToOneConversationHero, out textObject))
			{
				MBTextManager.SetTextVariable("IssueBriefByIssueGiverText", issueOwnersIssue.IssueBriefByIssueGiver, false);
				return true;
			}
			return false;
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000FD530 File Offset: 0x000FB730
		private static bool issue_not_offered_condition()
		{
			IssueBase issueOwnersIssue = IssuesCampaignBehavior.GetIssueOwnersIssue();
			TextObject text;
			if (issueOwnersIssue != null && !issueOwnersIssue.CheckPreconditions(Hero.OneToOneConversationHero, out text))
			{
				MBTextManager.SetTextVariable("ISSUE_NOT_OFFERED_EXPLANATION", text, false);
				return true;
			}
			return false;
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000FD564 File Offset: 0x000FB764
		private void leave_on_conversation_end_consequence()
		{
			Campaign.Current.ConversationManager.ConversationEndOneShot += MapEventHelper.OnConversationEnd;
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x000FD581 File Offset: 0x000FB781
		private static IssueBase GetIssueOwnersIssue()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			if (oneToOneConversationHero == null)
			{
				return null;
			}
			return oneToOneConversationHero.Issue;
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x000FD594 File Offset: 0x000FB794
		private static IssueBase GetCounterOfferersIssue()
		{
			if (Hero.OneToOneConversationHero != null)
			{
				foreach (IssueBase issueBase in Campaign.Current.IssueManager.Issues.Values)
				{
					if (issueBase.CounterOfferHero == Hero.OneToOneConversationHero && issueBase.IsSolvingWithLordSolution)
					{
						return issueBase;
					}
				}
			}
			return null;
		}

		// Token: 0x04001178 RID: 4472
		private const int MinNotableIssueCountForTowns = 1;

		// Token: 0x04001179 RID: 4473
		private const int MaxNotableIssueCountForTowns = 3;

		// Token: 0x0400117A RID: 4474
		private const int MinNotableIssueCountForVillages = 1;

		// Token: 0x0400117B RID: 4475
		private const int MaxNotableIssueCountForVillages = 2;

		// Token: 0x0400117C RID: 4476
		private const float MinIssuePercentageForClanHeroes = 0.1f;

		// Token: 0x0400117D RID: 4477
		private const float MaxIssuePercentageForClanHeroes = 0.2f;

		// Token: 0x0400117E RID: 4478
		private float _additionalFrequencyScore;

		// Token: 0x0400117F RID: 4479
		private Settlement[] _settlements;

		// Token: 0x04001180 RID: 4480
		private MBCampaignEvent _periodicEvent;

		// Token: 0x02000703 RID: 1795
		private struct IssueData
		{
			// Token: 0x060057ED RID: 22509 RVA: 0x00182605 File Offset: 0x00180805
			public IssueData(PotentialIssueData issueData, Hero hero, float score)
			{
				this.PotentialIssueData = issueData;
				this.Hero = hero;
				this.Score = score;
			}

			// Token: 0x04001CEF RID: 7407
			public readonly PotentialIssueData PotentialIssueData;

			// Token: 0x04001CF0 RID: 7408
			public readonly Hero Hero;

			// Token: 0x04001CF1 RID: 7409
			public readonly float Score;
		}
	}
}
