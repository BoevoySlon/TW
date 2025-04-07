using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E0 RID: 992
	public class ViewDataTrackerCampaignBehavior : CampaignBehaviorBase, IViewDataTracker
	{
		// Token: 0x06003D6F RID: 15727 RVA: 0x0012CB4C File Offset: 0x0012AD4C
		public ViewDataTrackerCampaignBehavior()
		{
			this._inventoryItemLocks = new List<string>();
			this._partyPrisonerLocks = new List<string>();
			this._partyTroopLocks = new List<string>();
			this._encyclopediaBookmarkedClans = new List<Clan>();
			this._encyclopediaBookmarkedConcepts = new List<Concept>();
			this._encyclopediaBookmarkedHeroes = new List<Hero>();
			this._encyclopediaBookmarkedKingdoms = new List<Kingdom>();
			this._encyclopediaBookmarkedSettlements = new List<Settlement>();
			this._encyclopediaBookmarkedUnits = new List<CharacterObject>();
			this._inventorySortPreferences = new Dictionary<int, Tuple<int, int>>();
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003D70 RID: 15728 RVA: 0x0012CC21 File Offset: 0x0012AE21
		// (set) Token: 0x06003D71 RID: 15729 RVA: 0x0012CC29 File Offset: 0x0012AE29
		public bool IsPartyNotificationActive { get; private set; }

		// Token: 0x06003D72 RID: 15730 RVA: 0x0012CC32 File Offset: 0x0012AE32
		public string GetPartyNotificationText()
		{
			this._recruitNotificationText.SetTextVariable("NUMBER", this._numOfRecruitablePrisoners);
			return this._recruitNotificationText.ToString();
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x0012CC56 File Offset: 0x0012AE56
		public void ClearPartyNotification()
		{
			this.IsPartyNotificationActive = false;
			this._numOfRecruitablePrisoners = 0;
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x0012CC66 File Offset: 0x0012AE66
		public void UpdatePartyNotification()
		{
			this.UpdatePrisonerRecruitValue();
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0012CC70 File Offset: 0x0012AE70
		private void UpdatePrisonerRecruitValue()
		{
			Dictionary<CharacterObject, int> dictionary = new Dictionary<CharacterObject, int>();
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.PrisonRoster.GetTroopRoster())
			{
				int num = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, troopRosterElement.Character);
				int num2;
				if (this._examinedPrisonerCharacterList.TryGetValue(troopRosterElement.Character, out num2))
				{
					if (num2 != num)
					{
						this._examinedPrisonerCharacterList[troopRosterElement.Character] = num;
						if (num2 < num)
						{
							this.IsPartyNotificationActive = true;
							this._numOfRecruitablePrisoners += num - num2;
						}
					}
				}
				else
				{
					this._examinedPrisonerCharacterList.Add(troopRosterElement.Character, num);
					if (num > 0)
					{
						this.IsPartyNotificationActive = true;
						this._numOfRecruitablePrisoners += num;
					}
				}
				dictionary.Add(troopRosterElement.Character, num);
			}
			this._examinedPrisonerCharacterList = dictionary;
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003D76 RID: 15734 RVA: 0x0012CD7C File Offset: 0x0012AF7C
		public bool IsQuestNotificationActive
		{
			get
			{
				return this._unExaminedQuestLogs.Count > 0;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003D77 RID: 15735 RVA: 0x0012CD8C File Offset: 0x0012AF8C
		public List<JournalLog> UnExaminedQuestLogs
		{
			get
			{
				return this._unExaminedQuestLogs;
			}
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x0012CD94 File Offset: 0x0012AF94
		public string GetQuestNotificationText()
		{
			this._questNotificationText.SetTextVariable("NUMBER", this._unExaminedQuestLogs.Count);
			return this._questNotificationText.ToString();
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x0012CDBD File Offset: 0x0012AFBD
		public void OnQuestLogExamined(JournalLog log)
		{
			if (this._unExaminedQuestLogs.Contains(log))
			{
				this._unExaminedQuestLogs.Remove(log);
			}
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x0012CDDA File Offset: 0x0012AFDA
		private void OnQuestLogAdded(QuestBase obj, bool hideInformation)
		{
			this._unExaminedQuestLogs.Add(obj.JournalEntries[obj.JournalEntries.Count - 1]);
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x0012CDFF File Offset: 0x0012AFFF
		private void OnIssueLogAdded(IssueBase obj, bool hideInformation)
		{
			this._unExaminedQuestLogs.Add(obj.JournalEntries[obj.JournalEntries.Count - 1]);
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x0012CE24 File Offset: 0x0012B024
		public List<Army> UnExaminedArmies
		{
			get
			{
				return this._unExaminedArmies;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003D7D RID: 15741 RVA: 0x0012CE2C File Offset: 0x0012B02C
		public int NumOfKingdomArmyNotifications
		{
			get
			{
				return this.UnExaminedArmies.Count;
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x0012CE39 File Offset: 0x0012B039
		public void OnArmyExamined(Army army)
		{
			this._unExaminedArmies.Remove(army);
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x0012CE48 File Offset: 0x0012B048
		private void OnArmyDispersed(Army arg1, Army.ArmyDispersionReason arg2, bool isPlayersArmy)
		{
			Army item;
			if (isPlayersArmy && (item = this._unExaminedArmies.SingleOrDefault((Army a) => a == arg1)) != null)
			{
				this._unExaminedArmies.Remove(item);
			}
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x0012CE8D File Offset: 0x0012B08D
		private void OnNewArmyCreated(Army army)
		{
			if (army.Kingdom == Hero.MainHero.MapFaction && army.LeaderParty != MobileParty.MainParty)
			{
				this._unExaminedArmies.Add(army);
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003D81 RID: 15745 RVA: 0x0012CEBA File Offset: 0x0012B0BA
		public bool IsCharacterNotificationActive
		{
			get
			{
				return this._isCharacterNotificationActive;
			}
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0012CEC2 File Offset: 0x0012B0C2
		public void ClearCharacterNotification()
		{
			this._isCharacterNotificationActive = false;
			this._numOfPerks = 0;
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x0012CED2 File Offset: 0x0012B0D2
		public string GetCharacterNotificationText()
		{
			this._characterNotificationText.SetTextVariable("NUMBER", this._numOfPerks);
			return this._characterNotificationText.ToString();
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x0012CEF6 File Offset: 0x0012B0F6
		private void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
			if ((hero == Hero.MainHero || hero.Clan == Clan.PlayerClan) && PerkHelper.AvailablePerkCountOfHero(hero) > 0)
			{
				this._isCharacterNotificationActive = true;
				this._numOfPerks++;
			}
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0012CF2B File Offset: 0x0012B12B
		private void OnHeroLevelledUp(Hero hero, bool shouldNotify)
		{
			if (hero == Hero.MainHero)
			{
				this._isCharacterNotificationActive = true;
			}
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x0012CF3C File Offset: 0x0012B13C
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.UpdatePartyNotification();
			this.UpdatePrisonerRecruitValue();
		}

		// Token: 0x06003D87 RID: 15751 RVA: 0x0012CF4A File Offset: 0x0012B14A
		public bool GetMapBarExtendedState()
		{
			return this._isMapBarExtended;
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x0012CF52 File Offset: 0x0012B152
		public void SetMapBarExtendedState(bool isExtended)
		{
			this._isMapBarExtended = isExtended;
		}

		// Token: 0x06003D89 RID: 15753 RVA: 0x0012CF5B File Offset: 0x0012B15B
		public void SetInventoryLocks(IEnumerable<string> locks)
		{
			this._inventoryItemLocks = locks.ToList<string>();
		}

		// Token: 0x06003D8A RID: 15754 RVA: 0x0012CF69 File Offset: 0x0012B169
		public IEnumerable<string> GetInventoryLocks()
		{
			return this._inventoryItemLocks;
		}

		// Token: 0x06003D8B RID: 15755 RVA: 0x0012CF71 File Offset: 0x0012B171
		public void InventorySetSortPreference(int inventoryMode, int sortOption, int sortState)
		{
			this._inventorySortPreferences[inventoryMode] = new Tuple<int, int>(sortOption, sortState);
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x0012CF88 File Offset: 0x0012B188
		public Tuple<int, int> InventoryGetSortPreference(int inventoryMode)
		{
			Tuple<int, int> result;
			if (this._inventorySortPreferences.TryGetValue(inventoryMode, out result))
			{
				return result;
			}
			return new Tuple<int, int>(0, 0);
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x0012CFAE File Offset: 0x0012B1AE
		public void SetPartyTroopLocks(IEnumerable<string> locks)
		{
			this._partyTroopLocks = locks.ToList<string>();
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x0012CFBC File Offset: 0x0012B1BC
		public void SetPartyPrisonerLocks(IEnumerable<string> locks)
		{
			this._partyPrisonerLocks = locks.ToList<string>();
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x0012CFCA File Offset: 0x0012B1CA
		public void SetPartySortType(int sortType)
		{
			this._partySortType = sortType;
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x0012CFD3 File Offset: 0x0012B1D3
		public void SetIsPartySortAscending(bool isAscending)
		{
			this._isPartySortAscending = isAscending;
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x0012CFDC File Offset: 0x0012B1DC
		public IEnumerable<string> GetPartyTroopLocks()
		{
			return this._partyTroopLocks;
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x0012CFE4 File Offset: 0x0012B1E4
		public IEnumerable<string> GetPartyPrisonerLocks()
		{
			return this._partyPrisonerLocks;
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x0012CFEC File Offset: 0x0012B1EC
		public int GetPartySortType()
		{
			return this._partySortType;
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0012CFF4 File Offset: 0x0012B1F4
		public bool GetIsPartySortAscending()
		{
			return this._isPartySortAscending;
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0012CFFC File Offset: 0x0012B1FC
		public void AddEncyclopediaBookmarkToItem(Hero item)
		{
			this._encyclopediaBookmarkedHeroes.Add(item);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x0012D00A File Offset: 0x0012B20A
		public void AddEncyclopediaBookmarkToItem(Clan clan)
		{
			this._encyclopediaBookmarkedClans.Add(clan);
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x0012D018 File Offset: 0x0012B218
		public void AddEncyclopediaBookmarkToItem(Concept concept)
		{
			this._encyclopediaBookmarkedConcepts.Add(concept);
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x0012D026 File Offset: 0x0012B226
		public void AddEncyclopediaBookmarkToItem(Kingdom kingdom)
		{
			this._encyclopediaBookmarkedKingdoms.Add(kingdom);
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x0012D034 File Offset: 0x0012B234
		public void AddEncyclopediaBookmarkToItem(Settlement settlement)
		{
			this._encyclopediaBookmarkedSettlements.Add(settlement);
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0012D042 File Offset: 0x0012B242
		public void AddEncyclopediaBookmarkToItem(CharacterObject unit)
		{
			this._encyclopediaBookmarkedUnits.Add(unit);
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x0012D050 File Offset: 0x0012B250
		public void RemoveEncyclopediaBookmarkFromItem(Hero hero)
		{
			this._encyclopediaBookmarkedHeroes.Remove(hero);
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x0012D05F File Offset: 0x0012B25F
		public void RemoveEncyclopediaBookmarkFromItem(Clan clan)
		{
			this._encyclopediaBookmarkedClans.Remove(clan);
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0012D06E File Offset: 0x0012B26E
		public void RemoveEncyclopediaBookmarkFromItem(Concept concept)
		{
			this._encyclopediaBookmarkedConcepts.Remove(concept);
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x0012D07D File Offset: 0x0012B27D
		public void RemoveEncyclopediaBookmarkFromItem(Kingdom kingdom)
		{
			this._encyclopediaBookmarkedKingdoms.Remove(kingdom);
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x0012D08C File Offset: 0x0012B28C
		public void RemoveEncyclopediaBookmarkFromItem(Settlement settlement)
		{
			this._encyclopediaBookmarkedSettlements.Remove(settlement);
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x0012D09B File Offset: 0x0012B29B
		public void RemoveEncyclopediaBookmarkFromItem(CharacterObject unit)
		{
			this._encyclopediaBookmarkedUnits.Remove(unit);
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x0012D0AA File Offset: 0x0012B2AA
		public bool IsEncyclopediaBookmarked(Hero hero)
		{
			return this._encyclopediaBookmarkedHeroes.Contains(hero);
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x0012D0B8 File Offset: 0x0012B2B8
		public bool IsEncyclopediaBookmarked(Clan clan)
		{
			return this._encyclopediaBookmarkedClans.Contains(clan);
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x0012D0C6 File Offset: 0x0012B2C6
		public bool IsEncyclopediaBookmarked(Concept concept)
		{
			return this._encyclopediaBookmarkedConcepts.Contains(concept);
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x0012D0D4 File Offset: 0x0012B2D4
		public bool IsEncyclopediaBookmarked(Kingdom kingdom)
		{
			return this._encyclopediaBookmarkedKingdoms.Contains(kingdom);
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x0012D0E2 File Offset: 0x0012B2E2
		public bool IsEncyclopediaBookmarked(Settlement settlement)
		{
			return this._encyclopediaBookmarkedSettlements.Contains(settlement);
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x0012D0F0 File Offset: 0x0012B2F0
		public bool IsEncyclopediaBookmarked(CharacterObject unit)
		{
			return this._encyclopediaBookmarkedUnits.Contains(unit);
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x0012D0FE File Offset: 0x0012B2FE
		public void SetQuestSelection(QuestBase selection)
		{
			this._questSelection = selection;
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x0012D107 File Offset: 0x0012B307
		public QuestBase GetQuestSelection()
		{
			return this._questSelection;
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x0012D110 File Offset: 0x0012B310
		public override void RegisterEvents()
		{
			CampaignEvents.HeroGainedSkill.AddNonSerializedListener(this, new Action<Hero, SkillObject, int, bool>(this.OnHeroGainedSkill));
			CampaignEvents.HeroLevelledUp.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroLevelledUp));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnNewArmyCreated));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.QuestLogAddedEvent.AddNonSerializedListener(this, new Action<QuestBase, bool>(this.OnQuestLogAdded));
			CampaignEvents.IssueLogAddedEvent.AddNonSerializedListener(this, new Action<IssueBase, bool>(this.OnIssueLogAdded));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x0012D1BE File Offset: 0x0012B3BE
		public void SetQuestSortTypeSelection(int questSortTypeSelection)
		{
			this._questSortTypeSelection = questSortTypeSelection;
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x0012D1C7 File Offset: 0x0012B3C7
		public int GetQuestSortTypeSelection()
		{
			return this._questSortTypeSelection;
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x0012D1D0 File Offset: 0x0012B3D0
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<bool>("_isMapBarExtended", ref this._isMapBarExtended);
			dataStore.SyncData<List<string>>("_inventoryItemLocks", ref this._inventoryItemLocks);
			dataStore.SyncData<Dictionary<int, Tuple<int, int>>>("_inventorySortPreferences", ref this._inventorySortPreferences);
			dataStore.SyncData<int>("_partySortType", ref this._partySortType);
			dataStore.SyncData<bool>("_isPartySortAscending", ref this._isPartySortAscending);
			dataStore.SyncData<List<string>>("_partyTroopLocks", ref this._partyTroopLocks);
			dataStore.SyncData<List<string>>("_partyPrisonerLocks", ref this._partyPrisonerLocks);
			dataStore.SyncData<List<Hero>>("_encyclopediaBookmarkedHeroes", ref this._encyclopediaBookmarkedHeroes);
			dataStore.SyncData<List<Clan>>("_encyclopediaBookmarkedClans", ref this._encyclopediaBookmarkedClans);
			dataStore.SyncData<List<Concept>>("_encyclopediaBookmarkedConcepts", ref this._encyclopediaBookmarkedConcepts);
			dataStore.SyncData<List<Kingdom>>("_encyclopediaBookmarkedKingdoms", ref this._encyclopediaBookmarkedKingdoms);
			dataStore.SyncData<List<Settlement>>("_encyclopediaBookmarkedSettlements", ref this._encyclopediaBookmarkedSettlements);
			dataStore.SyncData<List<CharacterObject>>("_encyclopediaBookmarkedUnits", ref this._encyclopediaBookmarkedUnits);
			dataStore.SyncData<QuestBase>("_questSelection", ref this._questSelection);
			dataStore.SyncData<List<JournalLog>>("_unExaminedQuestLogs", ref this._unExaminedQuestLogs);
			dataStore.SyncData<List<Army>>("_unExaminedArmies", ref this._unExaminedArmies);
			dataStore.SyncData<bool>("_isCharacterNotificationActive", ref this._isCharacterNotificationActive);
			dataStore.SyncData<int>("_numOfPerks", ref this._numOfPerks);
			dataStore.SyncData<Dictionary<CharacterObject, int>>("_examinedPrisonerCharacterList", ref this._examinedPrisonerCharacterList);
		}

		// Token: 0x04001231 RID: 4657
		private readonly TextObject _characterNotificationText = new TextObject("{=rlqjkZ9Q}You have {NUMBER} new perks available for selection.", null);

		// Token: 0x04001232 RID: 4658
		private readonly TextObject _questNotificationText = new TextObject("{=FAIYN0vN}You have {NUMBER} new updates to your quests.", null);

		// Token: 0x04001233 RID: 4659
		private readonly TextObject _recruitNotificationText = new TextObject("{=PJMbfSPJ}You have {NUMBER} new prisoners to recruit.", null);

		// Token: 0x04001235 RID: 4661
		private Dictionary<CharacterObject, int> _examinedPrisonerCharacterList = new Dictionary<CharacterObject, int>();

		// Token: 0x04001236 RID: 4662
		private int _numOfRecruitablePrisoners;

		// Token: 0x04001237 RID: 4663
		private List<JournalLog> _unExaminedQuestLogs = new List<JournalLog>();

		// Token: 0x04001238 RID: 4664
		private List<Army> _unExaminedArmies = new List<Army>();

		// Token: 0x04001239 RID: 4665
		private bool _isCharacterNotificationActive;

		// Token: 0x0400123A RID: 4666
		private int _numOfPerks;

		// Token: 0x0400123B RID: 4667
		private bool _isMapBarExtended;

		// Token: 0x0400123C RID: 4668
		private List<string> _inventoryItemLocks;

		// Token: 0x0400123D RID: 4669
		[SaveableField(21)]
		private Dictionary<int, Tuple<int, int>> _inventorySortPreferences;

		// Token: 0x0400123E RID: 4670
		private int _partySortType;

		// Token: 0x0400123F RID: 4671
		private bool _isPartySortAscending;

		// Token: 0x04001240 RID: 4672
		private List<string> _partyTroopLocks;

		// Token: 0x04001241 RID: 4673
		private List<string> _partyPrisonerLocks;

		// Token: 0x04001242 RID: 4674
		private List<Hero> _encyclopediaBookmarkedHeroes;

		// Token: 0x04001243 RID: 4675
		private List<Clan> _encyclopediaBookmarkedClans;

		// Token: 0x04001244 RID: 4676
		private List<Concept> _encyclopediaBookmarkedConcepts;

		// Token: 0x04001245 RID: 4677
		private List<Kingdom> _encyclopediaBookmarkedKingdoms;

		// Token: 0x04001246 RID: 4678
		private List<Settlement> _encyclopediaBookmarkedSettlements;

		// Token: 0x04001247 RID: 4679
		private List<CharacterObject> _encyclopediaBookmarkedUnits;

		// Token: 0x04001248 RID: 4680
		private QuestBase _questSelection;

		// Token: 0x04001249 RID: 4681
		[SaveableField(51)]
		private int _questSortTypeSelection;
	}
}
