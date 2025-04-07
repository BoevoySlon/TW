﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x020002F9 RID: 761
	public class IssueManager : CampaignEventReceiver
	{
		// Token: 0x06002C8B RID: 11403 RVA: 0x000BA815 File Offset: 0x000B8A15
		internal static void AutoGeneratedStaticCollectObjectsIssueManager(object o, List<object> collectedObjects)
		{
			((IssueManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x000BA823 File Offset: 0x000B8A23
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._issues);
			collectedObjects.Add(this._issuesCoolDownData);
			collectedObjects.Add(this._issuesWaitingForPlayerCaptivity);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x000BA849 File Offset: 0x000B8A49
		internal static object AutoGeneratedGetMemberValue_nextIssueUniqueIndex(object o)
		{
			return ((IssueManager)o)._nextIssueUniqueIndex;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x000BA85B File Offset: 0x000B8A5B
		internal static object AutoGeneratedGetMemberValue_issues(object o)
		{
			return ((IssueManager)o)._issues;
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x000BA868 File Offset: 0x000B8A68
		internal static object AutoGeneratedGetMemberValue_issuesCoolDownData(object o)
		{
			return ((IssueManager)o)._issuesCoolDownData;
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x000BA875 File Offset: 0x000B8A75
		internal static object AutoGeneratedGetMemberValue_issuesWaitingForPlayerCaptivity(object o)
		{
			return ((IssueManager)o)._issuesWaitingForPlayerCaptivity;
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002C91 RID: 11409 RVA: 0x000BA882 File Offset: 0x000B8A82
		public IEnumerable<Hero> IssueSolvingCompanionList
		{
			get
			{
				foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
				{
					if (keyValuePair.Value.IsSolvingWithAlternative)
					{
						yield return keyValuePair.Value.AlternativeSolutionHero;
					}
				}
				Dictionary<Hero, IssueBase>.Enumerator enumerator = default(Dictionary<Hero, IssueBase>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06002C92 RID: 11410 RVA: 0x000BA892 File Offset: 0x000B8A92
		public IssueManager()
		{
			this._issues = new Dictionary<Hero, IssueBase>();
			this._issuesCoolDownData = new Dictionary<string, List<IssueCoolDownData>>();
			this._issueArgs = new Dictionary<Hero, List<PotentialIssueData>>();
			this.Initialize();
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x000BA8CC File Offset: 0x000B8ACC
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData)
		{
			this._issueArgs = new Dictionary<Hero, List<PotentialIssueData>>();
			this.Initialize();
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000BA8DF File Offset: 0x000B8ADF
		private void Initialize()
		{
			this.Issues = this._issues.GetReadOnlyDictionary<Hero, IssueBase>();
			this.AssignIssuesToHeroes();
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000BA8F8 File Offset: 0x000B8AF8
		private void AssignIssuesToHeroes()
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this._issues)
			{
				keyValuePair.Key.OnIssueCreatedForHero(keyValuePair.Value);
			}
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000BA958 File Offset: 0x000B8B58
		public void InitializeForSavedGame()
		{
			if (this._issuesWaitingForPlayerCaptivity == null)
			{
				this._issuesWaitingForPlayerCaptivity = new List<IssueBase>();
			}
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this._issues.ToList<KeyValuePair<Hero, IssueBase>>())
			{
				IssueBase value = keyValuePair.Value;
				if (value == null)
				{
					this._issues.Remove(keyValuePair.Key);
				}
				else
				{
					value.InitializeIssueBaseOnLoad();
					if (value.IssueOwner != keyValuePair.Key)
					{
						Debug.FailedAssert("Issue owner is not the same as key!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Issues\\IssueManager.cs", "InitializeForSavedGame", 106);
					}
				}
			}
			this.ExpireInvalidData();
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000BAA10 File Offset: 0x000B8C10
		public bool CreateNewIssue(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			IssueBase issueBase = potentialIssueData.OnStartIssue(pid, issueOwner);
			issueBase.StringId = "issue_" + this._nextIssueUniqueIndex;
			this._nextIssueUniqueIndex++;
			issueBase.AfterCreation();
			this._issues.Add(issueOwner, issueBase);
			issueOwner.OnIssueCreatedForHero(issueBase);
			if (issueOwner.PartyBelongedTo != null)
			{
				issueBase.AddTrackedObject(issueOwner.PartyBelongedTo);
			}
			CampaignEventDispatcher.Instance.OnNewIssueCreated(issueBase);
			return true;
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000BAA96 File Offset: 0x000B8C96
		public bool StartIssueQuest(Hero issueOwner)
		{
			if (this.Issues[issueOwner].StartIssueWithQuest())
			{
				return true;
			}
			this.Issues[issueOwner].CompleteIssueWithStayAliveConditionsFailed();
			return false;
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000BAAC0 File Offset: 0x000B8CC0
		public void DeactivateIssue(IssueBase issue)
		{
			if (issue.IssueQuest == null)
			{
				issue.IssueOwner.OnIssueDeactivatedForHero();
				Campaign.Current.ConversationManager.RemoveRelatedLines(issue);
				if (this.Issues.ContainsKey(issue.IssueOwner))
				{
					this._issues.Remove(issue.IssueOwner);
				}
				return;
			}
			QuestBase issueQuest = issue.IssueQuest;
			if (issueQuest == null)
			{
				return;
			}
			issueQuest.CompleteQuestWithCancel(null);
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000BAB28 File Offset: 0x000B8D28
		public void ChangeIssueOwner(IssueBase issue, Hero newOwner)
		{
			Hero issueOwner = issue.IssueOwner;
			issueOwner.OnIssueDeactivatedForHero();
			newOwner.OnIssueCreatedForHero(issue);
			issue.IssueOwner = newOwner;
			this._issues.Remove(issueOwner);
			this._issues.Add(newOwner, issue);
			CampaignEventDispatcher.Instance.OnIssueOwnerChanged(issue, issueOwner);
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000BAB78 File Offset: 0x000B8D78
		private void PrepareIssueArguments(Hero hero)
		{
			List<PotentialIssueData> list;
			if (this._issueArgs.TryGetValue(hero, out list))
			{
				list.Clear();
				return;
			}
			this._issueArgs.Add(hero, new List<PotentialIssueData>());
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000BABAD File Offset: 0x000B8DAD
		public void AddPotentialIssueData(Hero hero, PotentialIssueData issueData)
		{
			this._issueArgs[hero].Add(issueData);
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000BABC1 File Offset: 0x000B8DC1
		private List<PotentialIssueData> GetPotentialIssues(Hero hero)
		{
			return this._issueArgs[hero];
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x000BABCF File Offset: 0x000B8DCF
		public List<PotentialIssueData> CheckForIssues(Hero issueOwner)
		{
			this.PrepareIssueArguments(issueOwner);
			if (!this.Issues.ContainsKey(issueOwner))
			{
				CampaignEventDispatcher.Instance.OnCheckForIssue(issueOwner);
			}
			return this.GetPotentialIssues(issueOwner);
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x000BABF8 File Offset: 0x000B8DF8
		public override void DailyTick()
		{
			this.ExpireInvalidData();
			List<IssueBase> list = new List<IssueBase>();
			List<IssueBase> list2 = new List<IssueBase>();
			List<IssueBase> list3 = new List<IssueBase>();
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				IssueBase value = keyValuePair.Value;
				bool flag = false;
				if (value.IsSolvingWithAlternative)
				{
					if (value.AlternativeSolutionReturnTimeForTroops.IsPast)
					{
						if (!this._issuesWaitingForPlayerCaptivity.Contains(value))
						{
							if (Hero.MainHero.IsPrisoner)
							{
								this._issuesWaitingForPlayerCaptivity.Add(value);
							}
							else
							{
								list2.Add(value);
							}
						}
					}
					else
					{
						if ((int)value.AlternativeSolutionIssueEffectClearTime.ToDays == (int)CampaignTime.Now.ToDays)
						{
							value.OnAlternativeSolutionSolvedAndTroopsAreReturning();
						}
						JournalLog journalLog = value.JournalEntries[0];
						int progress = (journalLog.CurrentProgress + 1 > journalLog.Range) ? journalLog.Range : (journalLog.CurrentProgress + 1);
						journalLog.UpdateCurrentProgress(progress);
					}
				}
				if (value.IsOngoingWithoutQuest && !value.IssueStayAliveConditions())
				{
					list3.Add(value);
					flag = true;
				}
				if (value.IssueDueTime.IsPast && value.IsOngoingWithoutQuest && !flag && MBRandom.RandomFloat <= 0.2f)
				{
					list.Add(value);
				}
			}
			foreach (IssueBase issueBase in list2)
			{
				issueBase.CompleteIssueWithAlternativeSolution();
			}
			foreach (IssueBase issueBase2 in list)
			{
				issueBase2.CompleteIssueWithTimedOut();
			}
			foreach (IssueBase issueBase3 in list3)
			{
				issueBase3.CompleteIssueWithStayAliveConditionsFailed();
			}
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000BAE58 File Offset: 0x000B9058
		public override void HourlyTick()
		{
			if (!Hero.MainHero.IsPrisoner && this._issuesWaitingForPlayerCaptivity.Count > 0)
			{
				TextObject textObject = new TextObject("{=l0NTCps3}As you emerge from captivity, {COMPANION.NAME} is waiting outside and greets you. {?COMPANION.GENDER}She{?}He{\\?} says {?COMPANION.GENDER}she{?}he{\\?} has returned from {?COMPANION.GENDER}her{?}his{\\?} mission with {NUMBER} {?(NUMBER > 1)}troops{?}troop{\\?} and they are all ready to rejoin your party.", null);
				for (int i = this._issuesWaitingForPlayerCaptivity.Count - 1; i >= 0; i--)
				{
					IssueBase item = this._issuesWaitingForPlayerCaptivity[i];
					StringHelpers.SetCharacterProperties("COMPANION", item.AlternativeSolutionHero.CharacterObject, textObject, false);
					textObject.SetTextVariable("NUMBER", item.AlternativeSolutionSentTroops.TotalManCount);
					InformationManager.ShowInquiry(new InquiryData(string.Empty, textObject.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), null, delegate()
					{
						this._issuesWaitingForPlayerCaptivity.Remove(item);
						item.CompleteIssueWithAlternativeSolution();
					}, null, "", 0f, null, null, null), true, false);
				}
			}
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.HourlyTickWithIssueManager();
			}
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000BAF98 File Offset: 0x000B9198
		private void ExpireInvalidData()
		{
			foreach (KeyValuePair<string, List<IssueCoolDownData>> keyValuePair in this._issuesCoolDownData)
			{
				List<IssueCoolDownData> list = new List<IssueCoolDownData>();
				foreach (IssueCoolDownData issueCoolDownData in keyValuePair.Value)
				{
					if (!issueCoolDownData.IsValid())
					{
						list.Add(issueCoolDownData);
					}
				}
				foreach (IssueCoolDownData item in list)
				{
					keyValuePair.Value.Remove(item);
				}
			}
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000BB084 File Offset: 0x000B9284
		public bool IsThereActiveIssueWithTypeInSettlement(Type type, Settlement settlement)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				if (keyValuePair.Value.GetType() == type && keyValuePair.Value.IssueSettlement == settlement)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000BB0FC File Offset: 0x000B92FC
		public int GetNumOfAvailableIssuesInSettlement(Settlement settlement)
		{
			List<IssueBase> list = new List<IssueBase>();
			int num = 0;
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				int num2 = (keyValuePair.Value.IsSolvingWithAlternative || keyValuePair.Value.IsSolvingWithLordSolution || keyValuePair.Value.IsSolvingWithQuest) ? 1 : 0;
				bool flag = keyValuePair.Value.IssueSettlement == settlement || keyValuePair.Value.IssueOwner.CurrentSettlement == settlement;
				if (num2 == 0 && flag && keyValuePair.Value.IssueQuest == null)
				{
					if (keyValuePair.Value.IssueStayAliveConditions())
					{
						num++;
					}
					else if (keyValuePair.Value.IsOngoingWithoutQuest)
					{
						list.Add(keyValuePair.Value);
					}
				}
			}
			foreach (IssueBase issueBase in list)
			{
				issueBase.CompleteIssueWithStayAliveConditionsFailed();
			}
			return num;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000BB22C File Offset: 0x000B942C
		public int GetNumOfActiveIssuesInSettlement(Settlement settlement, bool includeQuests)
		{
			int num = 0;
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				bool flag = keyValuePair.Value.IsSolvingWithAlternative || keyValuePair.Value.IsSolvingWithLordSolution || keyValuePair.Value.IsSolvingWithQuest;
				bool flag2 = keyValuePair.Value.IssueSettlement == settlement || keyValuePair.Value.IssueOwner.CurrentSettlement == settlement;
				if (flag && flag2 && includeQuests == (keyValuePair.Value.IssueQuest != null))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000BB2E8 File Offset: 0x000B94E8
		private IEnumerable<Hero> GetHeroesThatHaveIssueForSettlement(Settlement settlement)
		{
			foreach (Hero hero in settlement.HeroesWithoutParty)
			{
				if (hero.Issue != null)
				{
					yield return hero;
				}
			}
			List<Hero>.Enumerator enumerator = default(List<Hero>.Enumerator);
			foreach (MobileParty mobileParty in Settlement.CurrentSettlement.Parties)
			{
				foreach (TroopRosterElement troopRosterElement in from x in mobileParty.MemberRoster.GetTroopRoster()
				where x.Character.IsHero
				select x)
				{
					if (troopRosterElement.Character.HeroObject.Issue != null)
					{
						yield return troopRosterElement.Character.HeroObject;
					}
				}
				IEnumerator<TroopRosterElement> enumerator3 = null;
			}
			List<MobileParty>.Enumerator enumerator2 = default(List<MobileParty>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000BB2F8 File Offset: 0x000B94F8
		public GameMenuOption.IssueQuestFlags CheckIssueForMenuLocations(List<Location> currentLocations, bool getIssuesWithoutAQuest = false)
		{
			GameMenuOption.IssueQuestFlags issueQuestFlags = GameMenuOption.IssueQuestFlags.None;
			if (Settlement.CurrentSettlement == null || !this.Issues.Any<KeyValuePair<Hero, IssueBase>>())
			{
				return issueQuestFlags;
			}
			foreach (Location location in currentLocations)
			{
				foreach (LocationCharacter locationCharacter in location.GetCharacterList())
				{
					Hero heroObject = locationCharacter.Character.HeroObject;
					if (heroObject != null && heroObject.Issue != null)
					{
						if (getIssuesWithoutAQuest)
						{
							IssueBase issue = heroObject.Issue;
							if (((issue != null) ? issue.IssueQuest : null) != null)
							{
								continue;
							}
						}
						if (!(location.StringId != "prison") || !heroObject.IsPrisoner)
						{
							QuestBase issueQuest = heroObject.Issue.IssueQuest;
							if ((issueQuest != null && issueQuest.IsOngoing) || heroObject.Issue.IsSolvingWithAlternative || heroObject.Issue.IsSolvingWithLordSolution)
							{
								issueQuestFlags |= GameMenuOption.IssueQuestFlags.ActiveIssue;
							}
							else
							{
								issueQuestFlags |= GameMenuOption.IssueQuestFlags.AvailableIssue;
							}
						}
					}
				}
			}
			return issueQuestFlags;
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000BB424 File Offset: 0x000B9624
		public override void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				if (keyValuePair.Value.IssueQuest == quest)
				{
					switch (detail)
					{
					case QuestBase.QuestCompleteDetails.Success:
						keyValuePair.Value.CompleteIssueWithQuest();
						return;
					case QuestBase.QuestCompleteDetails.Cancel:
						keyValuePair.Value.CompleteIssueWithCancel(null);
						return;
					case QuestBase.QuestCompleteDetails.Fail:
						keyValuePair.Value.CompleteIssueWithFail(null);
						return;
					case QuestBase.QuestCompleteDetails.Timeout:
						keyValuePair.Value.CompleteIssueWithTimedOut();
						return;
					case QuestBase.QuestCompleteDetails.FailWithBetrayal:
						keyValuePair.Value.CompleteIssueWithBetrayal();
						return;
					default:
						keyValuePair.Value.CompleteIssueWithQuest();
						return;
					}
				}
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000BB4FC File Offset: 0x000B96FC
		public override void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party == MobileParty.MainParty)
			{
				foreach (Hero hero2 in this.GetHeroesThatHaveIssueForSettlement(Settlement.CurrentSettlement))
				{
					if (hero2.Issue.IsOngoingWithoutQuest && !hero2.Issue.IssueStayAliveConditions())
					{
						hero2.Issue.CompleteIssueWithStayAliveConditionsFailed();
					}
				}
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000BB574 File Offset: 0x000B9774
		public override void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (this.IssueDeactivationCommonCondition(party.LeaderHero))
			{
				party.LeaderHero.Issue.CompleteIssueWithStayAliveConditionsFailed();
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x000BB594 File Offset: 0x000B9794
		public override void OnCharacterPortraitPopUpOpened(CharacterObject character)
		{
			if (this.IssueDeactivationCommonCondition(character.HeroObject))
			{
				character.HeroObject.Issue.CompleteIssueWithStayAliveConditionsFailed();
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000BB5B4 File Offset: 0x000B97B4
		private bool IssueDeactivationCommonCondition(Hero hero)
		{
			return hero != null && hero.Issue != null && hero.Issue.IsOngoingWithoutQuest && !hero.Issue.IssueStayAliveConditions();
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000BB5E0 File Offset: 0x000B97E0
		public override void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim.Issue != null)
			{
				if (victim.Issue.IssueQuest != null && victim.Issue.IssueQuest.IsOngoing)
				{
					TextObject textObject = new TextObject("{=rTvWdMXF}{DIED_HERO.LINK} died and your agreement with {?DIED_HERO.GENDER}her{?}him{\\?} canceled.", null);
					StringHelpers.SetCharacterProperties("DIED_HERO", victim.CharacterObject, textObject, false);
					victim.Issue.IssueQuest.CompleteQuestWithCancel(textObject);
					return;
				}
				TextObject textObject2;
				if (killer != null)
				{
					textObject2 = GameTexts.FindText("str_responsible_of_death_link_news", null);
					StringHelpers.SetCharacterProperties("HERO_1", killer.CharacterObject, textObject2, false);
					StringHelpers.SetCharacterProperties("HERO_2", victim.CharacterObject, textObject2, false);
				}
				else
				{
					textObject2 = GameTexts.FindText("str_murdered_passive_news", null);
					StringHelpers.SetCharacterProperties("HERO_2", victim.CharacterObject, textObject2, false);
				}
				victim.Issue.CompleteIssueWithCancel(textObject2);
			}
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000BB6AC File Offset: 0x000B98AC
		public override void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if ((newOwner != null && newOwner.Clan == Clan.PlayerClan) || (oldOwner != null && oldOwner.Clan == Clan.PlayerClan))
			{
				foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
				{
					if (keyValuePair.Key.IsNotable)
					{
						if (keyValuePair.Value.IssueSettlement == settlement)
						{
							keyValuePair.Value.InitializeIssueOnSettlementOwnerChange();
						}
						if (settlement.IsFortification && keyValuePair.Value.IssueSettlement.IsVillage && keyValuePair.Value.IssueSettlement.Village.Bound == settlement)
						{
							keyValuePair.Value.InitializeIssueOnSettlementOwnerChange();
						}
					}
				}
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000BB78C File Offset: 0x000B998C
		public void ToggleAllIssueTracks(bool enableTrack)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.ToggleTrackedObjects(enableTrack);
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000BB7E8 File Offset: 0x000B99E8
		public void AddIssueCoolDownData(Type type, IssueCoolDownData data)
		{
			string name = type.Name;
			if (!this._issuesCoolDownData.ContainsKey(name))
			{
				this._issuesCoolDownData.Add(name, new List<IssueCoolDownData>());
			}
			this._issuesCoolDownData[name].Add(data);
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000BB830 File Offset: 0x000B9A30
		public bool HasIssueCoolDown(Type type, Hero hero)
		{
			string name = type.Name;
			bool result = false;
			List<IssueCoolDownData> list;
			if (this._issuesCoolDownData.TryGetValue(name, out list))
			{
				foreach (IssueCoolDownData issueCoolDownData in list)
				{
					if (issueCoolDownData.IsValid() && issueCoolDownData.IsRelatedTo(hero))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000BB8AC File Offset: 0x000B9AAC
		public override void CanHaveQuestsOrIssues(Hero hero, ref bool result)
		{
			IssueBase issueBase;
			if (this.Issues.TryGetValue(hero, out issueBase))
			{
				result = false;
				return;
			}
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanHaveQuestOrIssueInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
				if ((keyValuePair.Value.IsSolvingWithAlternative && keyValuePair.Value.AlternativeSolutionHero == hero) || keyValuePair.Value.CounterOfferHero == hero)
				{
					result = false;
					break;
				}
			}
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000BB954 File Offset: 0x000B9B54
		public override void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanDieInfoIsRequested(hero, causeOfDeath, ref result);
				if (keyValuePair.Value.AlternativeSolutionHero == hero)
				{
					result = false;
				}
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000BB9C8 File Offset: 0x000B9BC8
		public override void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanBecomePrisonerInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000BBA28 File Offset: 0x000B9C28
		public override void CanHeroMarry(Hero hero, ref bool result)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanMarryInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000BBA88 File Offset: 0x000B9C88
		public override void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanBeSelectedInInventoryInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000BBAE8 File Offset: 0x000B9CE8
		public override void CanHeroLeadParty(Hero hero, ref bool result)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanLeadPartyInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000BBB48 File Offset: 0x000B9D48
		public override void CanMoveToSettlement(Hero hero, ref bool result)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanMoveToSettlementInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000BBBA8 File Offset: 0x000B9DA8
		public override void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in this.Issues)
			{
				keyValuePair.Value.OnHeroCanHavePartyRoleOrBeGovernorInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x000BBC08 File Offset: 0x000B9E08
		public static void FillIssueCountsPerSettlement(Dictionary<Settlement, int> issueCountPerSettlement)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
			{
				Settlement issueSettlement = keyValuePair.Value.IssueSettlement;
				if (issueSettlement != null)
				{
					if (!issueCountPerSettlement.ContainsKey(issueSettlement))
					{
						issueCountPerSettlement[issueSettlement] = 1;
					}
					else
					{
						Settlement key = issueSettlement;
						issueCountPerSettlement[key]++;
					}
				}
			}
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000BBC98 File Offset: 0x000B9E98
		public static IEnumerable<IssueBase> GetIssuesInSettlement(Settlement settlement, bool onlyNotables = true)
		{
			foreach (Hero hero in settlement.Notables)
			{
				if (hero.Issue != null)
				{
					yield return hero.Issue;
				}
			}
			List<Hero>.Enumerator enumerator = default(List<Hero>.Enumerator);
			if (!onlyNotables)
			{
				foreach (Hero hero2 in settlement.HeroesWithoutParty)
				{
					if (hero2.Issue != null && !hero2.IsNotable)
					{
						yield return hero2.Issue;
					}
				}
				enumerator = default(List<Hero>.Enumerator);
				foreach (MobileParty settlementParty in settlement.Parties)
				{
					int num;
					for (int i = 0; i < settlementParty.MemberRoster.Count; i = num + 1)
					{
						CharacterObject characterAtIndex = settlementParty.MemberRoster.GetCharacterAtIndex(i);
						Hero hero3 = (characterAtIndex != null) ? characterAtIndex.HeroObject : null;
						if (hero3 != null && hero3.Issue != null)
						{
							yield return hero3.Issue;
						}
						num = i;
					}
					settlementParty = null;
				}
				List<MobileParty>.Enumerator enumerator2 = default(List<MobileParty>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000BBCB0 File Offset: 0x000B9EB0
		public static IssueBase GetIssueOfQuest(QuestBase quest)
		{
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
			{
				if (keyValuePair.Value.IssueQuest == quest)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000BBD24 File Offset: 0x000B9F24
		public static void FillIssueCountsPerClan(Dictionary<Clan, int> issueCountPerClan, IEnumerable<Clan> clans)
		{
			foreach (Clan clan in clans)
			{
				int num = 0;
				using (List<Hero>.Enumerator enumerator2 = clan.Heroes.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Issue != null)
						{
							num++;
						}
					}
				}
				issueCountPerClan.Add(clan, num);
			}
		}

		// Token: 0x04000D61 RID: 3425
		[SaveableField(0)]
		private int _nextIssueUniqueIndex;

		// Token: 0x04000D62 RID: 3426
		public MBReadOnlyDictionary<Hero, IssueBase> Issues;

		// Token: 0x04000D63 RID: 3427
		[SaveableField(1)]
		private readonly Dictionary<Hero, IssueBase> _issues;

		// Token: 0x04000D64 RID: 3428
		[SaveableField(2)]
		private Dictionary<string, List<IssueCoolDownData>> _issuesCoolDownData;

		// Token: 0x04000D65 RID: 3429
		[CachedData]
		private Dictionary<Hero, List<PotentialIssueData>> _issueArgs;

		// Token: 0x04000D66 RID: 3430
		[SaveableField(3)]
		private List<IssueBase> _issuesWaitingForPlayerCaptivity = new List<IssueBase>();

		// Token: 0x04000D67 RID: 3431
		public const string IssueOfferToken = "issue_offer";

		// Token: 0x04000D68 RID: 3432
		public const string HeroMainOptionsToken = "hero_main_options";

		// Token: 0x04000D69 RID: 3433
		public const string IssueClassicQuestStartToken = "issue_classic_quest_start";

		// Token: 0x04000D6A RID: 3434
		public const string IssueDiscussAlternativeSolution = "issue_discuss_alternative_solution";

		// Token: 0x04000D6B RID: 3435
		private const float IssueCancelChance = 0.2f;
	}
}
