using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000092 RID: 146
	public interface IViewDataTracker
	{
		// Token: 0x06001102 RID: 4354
		void SetInventoryLocks(IEnumerable<string> locks);

		// Token: 0x06001103 RID: 4355
		IEnumerable<string> GetInventoryLocks();

		// Token: 0x06001104 RID: 4356
		bool GetMapBarExtendedState();

		// Token: 0x06001105 RID: 4357
		void SetMapBarExtendedState(bool value);

		// Token: 0x06001106 RID: 4358
		void SetPartyTroopLocks(IEnumerable<string> locks);

		// Token: 0x06001107 RID: 4359
		void SetPartyPrisonerLocks(IEnumerable<string> locks);

		// Token: 0x06001108 RID: 4360
		void SetPartySortType(int sortType);

		// Token: 0x06001109 RID: 4361
		void SetIsPartySortAscending(bool isAscending);

		// Token: 0x0600110A RID: 4362
		IEnumerable<string> GetPartyTroopLocks();

		// Token: 0x0600110B RID: 4363
		IEnumerable<string> GetPartyPrisonerLocks();

		// Token: 0x0600110C RID: 4364
		int GetPartySortType();

		// Token: 0x0600110D RID: 4365
		bool GetIsPartySortAscending();

		// Token: 0x0600110E RID: 4366
		void AddEncyclopediaBookmarkToItem(Concept concept);

		// Token: 0x0600110F RID: 4367
		void AddEncyclopediaBookmarkToItem(Kingdom kingdom);

		// Token: 0x06001110 RID: 4368
		void AddEncyclopediaBookmarkToItem(Settlement settlement);

		// Token: 0x06001111 RID: 4369
		void AddEncyclopediaBookmarkToItem(CharacterObject unit);

		// Token: 0x06001112 RID: 4370
		void AddEncyclopediaBookmarkToItem(Hero item);

		// Token: 0x06001113 RID: 4371
		void AddEncyclopediaBookmarkToItem(Clan clan);

		// Token: 0x06001114 RID: 4372
		void RemoveEncyclopediaBookmarkFromItem(Hero hero);

		// Token: 0x06001115 RID: 4373
		void RemoveEncyclopediaBookmarkFromItem(Clan clan);

		// Token: 0x06001116 RID: 4374
		void RemoveEncyclopediaBookmarkFromItem(Concept concept);

		// Token: 0x06001117 RID: 4375
		void RemoveEncyclopediaBookmarkFromItem(Kingdom kingdom);

		// Token: 0x06001118 RID: 4376
		void RemoveEncyclopediaBookmarkFromItem(Settlement settlement);

		// Token: 0x06001119 RID: 4377
		void RemoveEncyclopediaBookmarkFromItem(CharacterObject unit);

		// Token: 0x0600111A RID: 4378
		bool IsEncyclopediaBookmarked(Hero hero);

		// Token: 0x0600111B RID: 4379
		bool IsEncyclopediaBookmarked(Clan clan);

		// Token: 0x0600111C RID: 4380
		bool IsEncyclopediaBookmarked(Concept concept);

		// Token: 0x0600111D RID: 4381
		bool IsEncyclopediaBookmarked(Kingdom kingdom);

		// Token: 0x0600111E RID: 4382
		bool IsEncyclopediaBookmarked(Settlement settlement);

		// Token: 0x0600111F RID: 4383
		bool IsEncyclopediaBookmarked(CharacterObject unit);

		// Token: 0x06001120 RID: 4384
		void SetQuestSelection(QuestBase selection);

		// Token: 0x06001121 RID: 4385
		QuestBase GetQuestSelection();

		// Token: 0x06001122 RID: 4386
		void SetQuestSortTypeSelection(int questSortTypeSelection);

		// Token: 0x06001123 RID: 4387
		int GetQuestSortTypeSelection();

		// Token: 0x06001124 RID: 4388
		void InventorySetSortPreference(int inventoryMode, int sortOption, int sortState);

		// Token: 0x06001125 RID: 4389
		Tuple<int, int> InventoryGetSortPreference(int inventoryMode);

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001126 RID: 4390
		bool IsPartyNotificationActive { get; }

		// Token: 0x06001127 RID: 4391
		string GetPartyNotificationText();

		// Token: 0x06001128 RID: 4392
		void ClearPartyNotification();

		// Token: 0x06001129 RID: 4393
		void UpdatePartyNotification();

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600112A RID: 4394
		bool IsQuestNotificationActive { get; }

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600112B RID: 4395
		List<JournalLog> UnExaminedQuestLogs { get; }

		// Token: 0x0600112C RID: 4396
		string GetQuestNotificationText();

		// Token: 0x0600112D RID: 4397
		void OnQuestLogExamined(JournalLog log);

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600112E RID: 4398
		List<Army> UnExaminedArmies { get; }

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600112F RID: 4399
		int NumOfKingdomArmyNotifications { get; }

		// Token: 0x06001130 RID: 4400
		void OnArmyExamined(Army army);

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001131 RID: 4401
		bool IsCharacterNotificationActive { get; }

		// Token: 0x06001132 RID: 4402
		void ClearCharacterNotification();

		// Token: 0x06001133 RID: 4403
		string GetCharacterNotificationText();
	}
}
