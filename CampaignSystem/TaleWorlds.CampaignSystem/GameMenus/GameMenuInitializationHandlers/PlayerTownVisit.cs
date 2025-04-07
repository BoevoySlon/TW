using System;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.GameMenus.GameMenuInitializationHandlers
{
	// Token: 0x020000E7 RID: 231
	public class PlayerTownVisit
	{
		// Token: 0x06001450 RID: 5200 RVA: 0x0005AD08 File Offset: 0x00058F08
		[GameMenuInitializationHandler("town")]
		[GameMenuInitializationHandler("castle")]
		private static void game_menu_town_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Town.WaitMeshName);
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0005AD31 File Offset: 0x00058F31
		[GameMenuInitializationHandler("town_wait")]
		[GameMenuInitializationHandler("town_guard")]
		[GameMenuInitializationHandler("menu_tournament_withdraw_verify")]
		[GameMenuInitializationHandler("menu_tournament_bet_confirm")]
		public static void game_menu_town_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0005AD4D File Offset: 0x00058F4D
		[GameMenuEventHandler("town", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("town", "manage_production_cheat", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_town_manage_town_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0005AD5A File Offset: 0x00058F5A
		[GameMenuEventHandler("town_keep", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_town_castle_manage_town_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0005AD67 File Offset: 0x00058F67
		[GameMenuEventHandler("castle", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_castle_manage_castle_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0005AD74 File Offset: 0x00058F74
		[GameMenuEventHandler("tutorial", "mno_go_back_dot", GameMenuEventHandler.EventType.OnConsequence)]
		private static void mno_go_back_dot(MenuCallbackArgs args)
		{
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0005AD76 File Offset: 0x00058F76
		[GameMenuEventHandler("village", "buy_goods", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_village_buy_good_on_consequence(MenuCallbackArgs args)
		{
			InventoryManager.OpenScreenAsTrade(Settlement.CurrentSettlement.ItemRoster, Settlement.CurrentSettlement.Village, InventoryManager.InventoryCategoryType.None, null);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0005AD93 File Offset: 0x00058F93
		[GameMenuEventHandler("village", "manage_production", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_village_manage_village_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTownManagement();
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0005ADA0 File Offset: 0x00058FA0
		[GameMenuEventHandler("village", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("town_backstreet", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("town", "recruit_volunteers", GameMenuEventHandler.EventType.OnConsequence)]
		private static void game_menu_recruit_volunteers_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenRecruitVolunteers();
		}
	}
}
