﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000064 RID: 100
	public class QuestManager : CampaignEventReceiver
	{
		// Token: 0x06000DA0 RID: 3488 RVA: 0x00043967 File Offset: 0x00041B67
		internal static void AutoGeneratedStaticCollectObjectsQuestManager(object o, List<object> collectedObjects)
		{
			((QuestManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00043975 File Offset: 0x00041B75
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._quests);
			collectedObjects.Add(this._trackedObjects);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0004398F File Offset: 0x00041B8F
		internal static object AutoGeneratedGetMemberValue_quests(object o)
		{
			return ((QuestManager)o)._quests;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0004399C File Offset: 0x00041B9C
		internal static object AutoGeneratedGetMemberValue_trackedObjects(object o)
		{
			return ((QuestManager)o)._trackedObjects;
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000439A9 File Offset: 0x00041BA9
		public MBReadOnlyList<QuestBase> Quests
		{
			get
			{
				return this._quests;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x000439B1 File Offset: 0x00041BB1
		// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x000439B9 File Offset: 0x00041BB9
		public bool QuestDebugMode { get; set; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x000439C2 File Offset: 0x00041BC2
		// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x000439CA File Offset: 0x00041BCA
		public MBReadOnlyDictionary<ITrackableCampaignObject, List<QuestBase>> TrackedObjects { get; private set; }

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000439D3 File Offset: 0x00041BD3
		public QuestManager()
		{
			this._quests = new MBList<QuestBase>();
			this._trackedObjects = new Dictionary<ITrackableCampaignObject, List<QuestBase>>();
			this._currentHourlyTickQuestsToTimeout = new MBList<QuestBase>();
			this.Initialize();
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00043A02 File Offset: 0x00041C02
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData)
		{
			this.Initialize();
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00043A0A File Offset: 0x00041C0A
		private void Initialize()
		{
			this.TrackedObjects = this._trackedObjects.GetReadOnlyDictionary<ITrackableCampaignObject, List<QuestBase>>();
			this._currentHourlyTickQuestsToTimeout = new MBList<QuestBase>();
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00043A28 File Offset: 0x00041C28
		public override void OnQuestStarted(QuestBase quest)
		{
			this._quests.Add(quest);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00043A38 File Offset: 0x00041C38
		public bool IsThereActiveQuestWithType(Type type)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				if (questBase.IsOngoing && (type == questBase.GetType() || questBase.GetType().IsSubclassOf(type)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00043AB0 File Offset: 0x00041CB0
		public bool IsQuestGiver(Hero offeringHero)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				if (questBase.IsOngoing && questBase.QuestGiver == offeringHero)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00043B14 File Offset: 0x00041D14
		public override void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			List<QuestBase> list = new List<QuestBase>();
			for (int i = this.Quests.Count - 1; i >= 0; i--)
			{
				QuestBase questBase = this.Quests[i];
				if (questBase == null)
				{
					this._quests.Remove(questBase);
				}
				else if (!questBase.IsFinalized)
				{
					bool flag = false;
					foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
					{
						IssueBase value = keyValuePair.Value;
						if (((value != null) ? value.IssueQuest : null) == questBase)
						{
							flag = true;
							break;
						}
					}
					if (flag || questBase.IsSpecialQuest)
					{
						questBase.InitializeQuestOnLoadWithQuestManager();
						using (List<QuestTaskBase>.Enumerator enumerator2 = questBase.TaskList.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								QuestTaskBase questTaskBase = enumerator2.Current;
								if (questTaskBase.IsActive)
								{
									questTaskBase.SetReferences();
									questTaskBase.AddTaskDialogs();
								}
							}
							goto IL_142;
						}
					}
					list.Add(questBase);
					Debug.FailedAssert(string.Concat(new object[]
					{
						"There is not active issue for quest: ",
						questBase.Title,
						" string id: ",
						questBase.StringId,
						". Quest will be canceled."
					}), "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\QuestManager.cs", "OnGameLoaded", 127);
				}
				IL_142:;
			}
			foreach (QuestBase questBase2 in list)
			{
				questBase2.CompleteQuestWithCancel(null);
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00043CC8 File Offset: 0x00041EC8
		public override void OnSessionStart(CampaignGameStarter campaignGameStarter)
		{
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00043CE4 File Offset: 0x00041EE4
		public override void HourlyTick()
		{
			for (int i = this.Quests.Count - 1; i >= 0; i--)
			{
				QuestBase questBase = this.Quests[i];
				if (questBase.IsOngoing && questBase.QuestDueTime.IsPast)
				{
					this._currentHourlyTickQuestsToTimeout.Add(questBase);
				}
			}
			foreach (QuestBase questBase2 in this._currentHourlyTickQuestsToTimeout)
			{
				if (!questBase2.IsFinalized)
				{
					questBase2.CompleteQuestWithTimeOut(null);
				}
			}
			this._currentHourlyTickQuestsToTimeout.Clear();
			for (int j = this.Quests.Count - 1; j >= 0; j--)
			{
				this.Quests[j].HourlyTickWithQuestManager();
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00043DC4 File Offset: 0x00041FC4
		public override void DailyTick()
		{
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00043DC8 File Offset: 0x00041FC8
		public GameMenuOption.IssueQuestFlags CheckQuestForMenuLocations(List<Location> currentLocations)
		{
			GameMenuOption.IssueQuestFlags issueQuestFlags = GameMenuOption.IssueQuestFlags.None;
			foreach (Location location in currentLocations)
			{
				foreach (LocationCharacter locationCharacter in location.GetCharacterList())
				{
					CharacterObject character = locationCharacter.Character;
					Hero hero = (character != null) ? character.HeroObject : null;
					if (hero != null)
					{
						foreach (QuestBase questBase in this.Quests)
						{
							if (questBase != null && questBase.IsOngoing)
							{
								if (questBase.QuestGiver == hero)
								{
									issueQuestFlags |= (questBase.IsSpecialQuest ? GameMenuOption.IssueQuestFlags.ActiveStoryQuest : GameMenuOption.IssueQuestFlags.ActiveIssue);
								}
								else if (questBase.IsTracked(hero))
								{
									issueQuestFlags |= (questBase.IsSpecialQuest ? GameMenuOption.IssueQuestFlags.TrackedStoryQuest : GameMenuOption.IssueQuestFlags.TrackedIssue);
								}
							}
						}
					}
				}
			}
			foreach (Location location2 in currentLocations)
			{
				issueQuestFlags |= this.IsLocationsTracked(location2);
			}
			return issueQuestFlags;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00043F28 File Offset: 0x00042128
		private GameMenuOption.IssueQuestFlags IsLocationsTracked(Location location)
		{
			GameMenuOption.IssueQuestFlags issueQuestFlags = GameMenuOption.IssueQuestFlags.None;
			foreach (QuestBase questBase in this.Quests)
			{
				issueQuestFlags |= questBase.IsLocationTrackedByQuest(location);
			}
			return issueQuestFlags;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00043F84 File Offset: 0x00042184
		public void OnQuestFinalized(QuestBase quest)
		{
			this._quests.Remove(quest);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00043F94 File Offset: 0x00042194
		public override void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newPlayerParty, bool isMainPartyChanged)
		{
			for (int i = this.Quests.Count - 1; i >= 0; i--)
			{
				QuestBase questBase = this.Quests[i];
				if (questBase.IsOngoing && !questBase.IsSpecialQuest)
				{
					questBase.CompleteQuestWithFail(null);
				}
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00043FE0 File Offset: 0x000421E0
		public override void CanHaveQuestsOrIssues(Hero hero, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				if (questBase.IsOngoing && questBase.QuestGiver == hero)
				{
					result = false;
					break;
				}
				questBase.OnHeroCanHaveQuestOrIssueInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00044050 File Offset: 0x00042250
		public override void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				questBase.OnHeroCanDieInfoIsRequested(hero, causeOfDeath, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000440AC File Offset: 0x000422AC
		public override void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				questBase.OnHeroCanBecomePrisonerInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00044104 File Offset: 0x00042304
		public override void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				questBase.OnHeroCanBeSelectedInInventoryInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0004415C File Offset: 0x0004235C
		public override void CanHeroLeadParty(Hero hero, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				questBase.OnHeroCanLeadPartyInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x000441B4 File Offset: 0x000423B4
		public override void CanHeroMarry(Hero hero, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				questBase.OnHeroCanMarryInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0004420C File Offset: 0x0004240C
		public override void CanMoveToSettlement(Hero hero, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				questBase.OnHeroCanMoveToSettlementInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00044264 File Offset: 0x00042464
		public override void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				questBase.OnHeroCanHavePartyRoleOrBeGovernorInfoIsRequested(hero, ref result);
				if (!result)
				{
					break;
				}
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000442BC File Offset: 0x000424BC
		public void AddTrackedObjectForQuest(ITrackableCampaignObject trackedObject, QuestBase relatedQuest)
		{
			List<QuestBase> list;
			if (!this._trackedObjects.TryGetValue(trackedObject, out list))
			{
				this._trackedObjects.Add(trackedObject, new List<QuestBase>
				{
					relatedQuest
				});
				return;
			}
			if (!list.Contains(relatedQuest))
			{
				list.Add(relatedQuest);
				return;
			}
			Debug.FailedAssert(trackedObject.GetName() + " already contains quest: " + relatedQuest.Title, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\QuestManager.cs", "AddTrackedObjectForQuest", 362);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00044330 File Offset: 0x00042530
		public void RemoveTrackedObjectForQuest(ITrackableCampaignObject trackedObject, QuestBase relatedQuest)
		{
			List<QuestBase> list;
			if (this._trackedObjects.TryGetValue(trackedObject, out list))
			{
				if (!list.Contains(relatedQuest))
				{
					Debug.FailedAssert(trackedObject.GetName() + " is not tracked by quest: " + relatedQuest.Title, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\QuestManager.cs", "RemoveTrackedObjectForQuest", 386);
					return;
				}
				list.Remove(relatedQuest);
				if (list.Count == 0)
				{
					this._trackedObjects.Remove(trackedObject);
					Campaign.Current.VisualTrackerManager.RemoveTrackedObject(trackedObject, false);
					return;
				}
			}
			else
			{
				Debug.FailedAssert(trackedObject.GetName() + " does not track any quests.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\QuestManager.cs", "RemoveTrackedObjectForQuest", 391);
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x000443D4 File Offset: 0x000425D4
		public void RemoveAllTrackedObjectsForQuest(QuestBase quest)
		{
			List<ITrackableCampaignObject> list = new List<ITrackableCampaignObject>();
			foreach (KeyValuePair<ITrackableCampaignObject, List<QuestBase>> keyValuePair in this.TrackedObjects)
			{
				if (keyValuePair.Value.Contains(quest))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (ITrackableCampaignObject trackedObject in list)
			{
				this.RemoveTrackedObjectForQuest(trackedObject, quest);
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00044484 File Offset: 0x00042684
		public List<ITrackableCampaignObject> GetAllTrackedObjectsOfAQuest(QuestBase quest)
		{
			List<ITrackableCampaignObject> list = new List<ITrackableCampaignObject>();
			foreach (KeyValuePair<ITrackableCampaignObject, List<QuestBase>> keyValuePair in this.TrackedObjects)
			{
				if (keyValuePair.Value.Contains(quest))
				{
					list.Add(keyValuePair.Key);
				}
			}
			return list;
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x000444F4 File Offset: 0x000426F4
		public IEnumerable<QuestBase> GetQuestGiverQuests(Hero hero)
		{
			foreach (QuestBase questBase in this.Quests)
			{
				if (questBase.IsOngoing && questBase.QuestGiver == hero)
				{
					yield return questBase;
				}
			}
			List<QuestBase>.Enumerator enumerator = default(List<QuestBase>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0004450C File Offset: 0x0004270C
		public static bool QuestExistInSettlementNotables(QuestBase questBase, Settlement settlement)
		{
			foreach (Hero hero in settlement.Notables)
			{
				if (questBase.QuestGiver == hero)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00044568 File Offset: 0x00042768
		public static bool QuestExistInClan(QuestBase questBase, Clan clan)
		{
			foreach (Hero hero in clan.Lords)
			{
				if (questBase.QuestGiver == hero)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040003E8 RID: 1000
		public const string QuestOfferToken = "quest_offer";

		// Token: 0x040003E9 RID: 1001
		public const string QuestDiscussToken = "quest_discuss";

		// Token: 0x040003EA RID: 1002
		public const string HeroMainOptionsToken = "hero_main_options";

		// Token: 0x040003EB RID: 1003
		public const string NpcLordStartToken = "lord_start";

		// Token: 0x040003EC RID: 1004
		public const string CharacterTalkToken = "start";

		// Token: 0x040003ED RID: 1005
		public static string PriorQuestName;

		// Token: 0x040003EE RID: 1006
		private MBList<QuestBase> _currentHourlyTickQuestsToTimeout;

		// Token: 0x040003EF RID: 1007
		[SaveableField(0)]
		private readonly MBList<QuestBase> _quests;

		// Token: 0x040003F1 RID: 1009
		[SaveableField(10)]
		private readonly Dictionary<ITrackableCampaignObject, List<QuestBase>> _trackedObjects;
	}
}
