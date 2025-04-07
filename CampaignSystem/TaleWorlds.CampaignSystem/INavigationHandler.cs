using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008C RID: 140
	public interface INavigationHandler
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060010C8 RID: 4296
		bool PartyEnabled { get; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060010C9 RID: 4297
		bool InventoryEnabled { get; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060010CA RID: 4298
		bool QuestsEnabled { get; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060010CB RID: 4299
		bool CharacterDeveloperEnabled { get; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060010CC RID: 4300
		NavigationPermissionItem ClanPermission { get; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060010CD RID: 4301
		NavigationPermissionItem KingdomPermission { get; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060010CE RID: 4302
		bool EscapeMenuEnabled { get; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060010CF RID: 4303
		bool PartyActive { get; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060010D0 RID: 4304
		bool InventoryActive { get; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060010D1 RID: 4305
		bool QuestsActive { get; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060010D2 RID: 4306
		bool CharacterDeveloperActive { get; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060010D3 RID: 4307
		bool ClanActive { get; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060010D4 RID: 4308
		bool KingdomActive { get; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060010D5 RID: 4309
		bool EscapeMenuActive { get; }

		// Token: 0x060010D6 RID: 4310
		void OpenQuests();

		// Token: 0x060010D7 RID: 4311
		void OpenQuests(QuestBase quest);

		// Token: 0x060010D8 RID: 4312
		void OpenQuests(IssueBase issue);

		// Token: 0x060010D9 RID: 4313
		void OpenQuests(JournalLogEntry log);

		// Token: 0x060010DA RID: 4314
		void OpenInventory();

		// Token: 0x060010DB RID: 4315
		void OpenParty();

		// Token: 0x060010DC RID: 4316
		void OpenCharacterDeveloper();

		// Token: 0x060010DD RID: 4317
		void OpenCharacterDeveloper(Hero hero);

		// Token: 0x060010DE RID: 4318
		void OpenKingdom();

		// Token: 0x060010DF RID: 4319
		void OpenKingdom(Army army);

		// Token: 0x060010E0 RID: 4320
		void OpenKingdom(Settlement settlement);

		// Token: 0x060010E1 RID: 4321
		void OpenKingdom(Clan clan);

		// Token: 0x060010E2 RID: 4322
		void OpenKingdom(PolicyObject policy);

		// Token: 0x060010E3 RID: 4323
		void OpenKingdom(IFaction faction);

		// Token: 0x060010E4 RID: 4324
		void OpenKingdom(KingdomDecision decision);

		// Token: 0x060010E5 RID: 4325
		void OpenClan();

		// Token: 0x060010E6 RID: 4326
		void OpenClan(Hero hero);

		// Token: 0x060010E7 RID: 4327
		void OpenClan(PartyBase party);

		// Token: 0x060010E8 RID: 4328
		void OpenClan(Settlement settlement);

		// Token: 0x060010E9 RID: 4329
		void OpenClan(Workshop workshop);

		// Token: 0x060010EA RID: 4330
		void OpenClan(Alley alley);

		// Token: 0x060010EB RID: 4331
		void OpenEscapeMenu();
	}
}
