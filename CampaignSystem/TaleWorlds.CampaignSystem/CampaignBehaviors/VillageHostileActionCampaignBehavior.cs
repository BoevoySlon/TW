using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E3 RID: 995
	public class VillageHostileActionCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003DBB RID: 15803 RVA: 0x0012DCAD File Offset: 0x0012BEAD
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<string, CampaignTime>>("_villageLastHostileActionTimeDictionary", ref this._villageLastHostileActionTimeDictionary);
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x0012DCC1 File Offset: 0x0012BEC1
		public override void RegisterEvents()
		{
			CampaignEvents.OnAfterSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterSessionLaunched));
			CampaignEvents.ItemsLooted.AddNonSerializedListener(this, new Action<MobileParty, ItemRoster>(this.OnItemsLooted));
		}

		// Token: 0x06003DBD RID: 15805 RVA: 0x0012DCF1 File Offset: 0x0012BEF1
		private void OnItemsLooted(MobileParty mobileParty, ItemRoster lootedItems)
		{
			SkillLevelingManager.OnRaid(mobileParty, lootedItems);
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x0012DCFA File Offset: 0x0012BEFA
		private void OnAfterSessionLaunched(CampaignGameStarter campaignGameSystemStarter)
		{
			this.AddGameMenus(campaignGameSystemStarter);
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x0012DD04 File Offset: 0x0012BF04
		public void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenuOption("village", "hostile_action", "{=GM3tAYMr}Take a hostile action", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_on_consequence), false, 2, false, null);
			campaignGameSystemStarter.AddGameMenu("village_hostile_action", "{=YVNZaVCA}What action do you have in mind?", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_menu_on_init), GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "raid_village", "{=CTi0ml5F}Raid the village", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_raid_village_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_hostile_action_raid_village_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "force_peasants_to_give_volunteers", "{=RL8z99Dt}Force notables to give you recruits", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_force_volunteers_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_hostile_action_force_volunteers_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "force_peasants_to_give_supplies", "{=eAzwpqE1}Force peasants to give you goods", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_take_food_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_hostile_action_take_food_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("village_hostile_action", "forget_it", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.back_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_hostile_action_forget_it_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddWaitGameMenu("raiding_village", "{=hWwr3mrC}You are raiding {VILLAGE_NAME}.", new OnInitDelegate(VillageHostileActionCampaignBehavior.hostile_action_village_on_init), new OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_start_raiding_on_condition), new OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_on_consequence), new OnTickDelegate(VillageHostileActionCampaignBehavior.wait_menu_raiding_village_on_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameOverlays.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("raiding_village", "raiding_village_end", "{=M7CcfbIx}End Raiding", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("raiding_village", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_leaving_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_leaving_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("raiding_village", "abandon_army", "{=0vnegjxf}Abandon Army", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_abandoning_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.wait_menu_end_raiding_at_army_by_abandoning_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("raid_village_no_resist_warn_player", "{=lOwjyUi5}Raiding this village will cause a war with {KINGDOM}.", new OnInitDelegate(VillageHostileActionCampaignBehavior.game_menu_warn_player_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("raid_village_no_resist_warn_player", "raid_village_warn_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.game_menu_village_hostile_action_raid_village_warn_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_hostile_action_raid_village_warn_continue_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("raid_village_no_resist_warn_player", "raid_village_warn_leave", "{=sP9ohQTs}Forget it", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.back_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_hostile_action_raid_village_warn_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_supplies_village", "{=EqFbNha8}The villagers grudgingly bring out what they have for you.", new OnInitDelegate(VillageHostileActionCampaignBehavior.hostile_action_village_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_supplies_village", "force_supplies_village_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.village_force_supplies_ended_successfully_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("force_volunteers_village", "{=BqkD4YWr}You manage to round up some men from the village who look like they might make decent recruits.", new OnInitDelegate(VillageHostileActionCampaignBehavior.hostile_action_village_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("force_volunteers_village", "force_supplies_village_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.village_force_volunteers_ended_successfully_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village_looted", "{=NxcXfUxu}The village has been looted. A handful of souls scatter as you pass through the burnt-out houses.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_looted", "leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.back_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.game_menu_settlement_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village_player_raid_ended", "{=m1rzHfxI}{VILLAGE_ENCOUNTER_RESULT}", new OnInitDelegate(VillageHostileActionCampaignBehavior.village_player_raid_ended_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_player_raid_ended", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.village_player_raid_ended_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("village_raid_ended_leaded_by_someone_else", "{=m1rzHfxI}{VILLAGE_ENCOUNTER_RESULT}", new OnInitDelegate(VillageHostileActionCampaignBehavior.village_raid_ended_leaded_by_someone_else_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("village_raid_ended_leaded_by_someone_else", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(VillageHostileActionCampaignBehavior.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(VillageHostileActionCampaignBehavior.village_raid_ended_leaded_by_someone_else_on_consequence), true, -1, false, null);
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x0012E107 File Offset: 0x0012C307
		private static bool wait_menu_end_raiding_on_condition(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}
			return false;
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x0012E136 File Offset: 0x0012C336
		private static bool wait_menu_end_raiding_at_army_by_leaving_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && MobileParty.MainParty.MapEvent == null;
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x0012E174 File Offset: 0x0012C374
		private void village_force_supplies_ended_successfully_on_consequence(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			GameMenu.SwitchToMenu("village");
			ItemRoster itemRoster = new ItemRoster();
			int num = MathF.Max((int)(Settlement.CurrentSettlement.Village.Hearth * 0.15f), 20);
			GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, num * Campaign.Current.Models.RaidModel.GoldRewardForEachLostHearth, false);
			for (int i = 0; i < Settlement.CurrentSettlement.Village.VillageType.Productions.Count; i++)
			{
				ValueTuple<ItemObject, float> valueTuple = Settlement.CurrentSettlement.Village.VillageType.Productions[i];
				ItemObject item = valueTuple.Item1;
				int num2 = (int)(valueTuple.Item2 / 60f * (float)num);
				if (num2 > 0)
				{
					itemRoster.AddToCounts(item, num2);
				}
			}
			if (!this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				this._villageLastHostileActionTimeDictionary.Add(Settlement.CurrentSettlement.StringId, CampaignTime.Now);
			}
			else
			{
				this._villageLastHostileActionTimeDictionary[Settlement.CurrentSettlement.StringId] = CampaignTime.Now;
			}
			Settlement.CurrentSettlement.SettlementHitPoints -= Settlement.CurrentSettlement.SettlementHitPoints * 0.8f;
			InventoryManager.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster>
			{
				{
					PartyBase.MainParty,
					itemRoster
				}
			});
			bool attacked = MapEvent.PlayerMapEvent == null;
			SkillLevelingManager.OnForceSupplies(MobileParty.MainParty, itemRoster, attacked);
			PlayerEncounter.Current.ForceSupplies = false;
			PlayerEncounter.Current.FinalizeBattle();
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x0012E2F0 File Offset: 0x0012C4F0
		private void village_force_volunteers_ended_successfully_on_consequence(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			GameMenu.SwitchToMenu("village");
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			int num = (int)Math.Ceiling((double)(Settlement.CurrentSettlement.Village.Hearth / 30f));
			if (MobileParty.MainParty.HasPerk(DefaultPerks.Roguery.InBestLight, false))
			{
				num += Settlement.CurrentSettlement.Notables.Count;
			}
			troopRoster.AddToCounts(Settlement.CurrentSettlement.Culture.BasicTroop, num, false, 0, 0, true, -1);
			if (!this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				this._villageLastHostileActionTimeDictionary.Add(Settlement.CurrentSettlement.StringId, CampaignTime.Now);
			}
			else
			{
				this._villageLastHostileActionTimeDictionary[Settlement.CurrentSettlement.StringId] = CampaignTime.Now;
			}
			Settlement.CurrentSettlement.SettlementHitPoints -= Settlement.CurrentSettlement.SettlementHitPoints * 0.8f;
			Settlement.CurrentSettlement.Village.Hearth -= (float)(num / 2);
			PartyScreenManager.OpenScreenAsLoot(troopRoster, TroopRoster.CreateDummyTroopRoster(), MobileParty.MainParty.CurrentSettlement.Name, troopRoster.TotalManCount, null);
			PlayerEncounter.Current.ForceVolunteers = false;
			SkillLevelingManager.OnForceVolunteers(MobileParty.MainParty, Settlement.CurrentSettlement.Party);
			PlayerEncounter.Current.FinalizeBattle();
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x0012E444 File Offset: 0x0012C644
		private static bool game_menu_village_hostile_action_raid_village_warn_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Raid;
			return true;
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x0012E450 File Offset: 0x0012C650
		private static bool wait_menu_end_raiding_at_army_by_abandoning_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty || MobileParty.MainParty.MapEvent == null)
			{
				return false;
			}
			args.Tooltip = GameTexts.FindText("str_abandon_army", null);
			args.Tooltip.SetTextVariable("INFLUENCE_COST", Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy());
			return true;
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x0012E4D2 File Offset: 0x0012C6D2
		private static void wait_menu_end_raiding_at_army_by_leaving_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Current.ForceRaid = false;
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x0012E4F0 File Offset: 0x0012C6F0
		private static void wait_menu_end_raiding_at_army_by_abandoning_on_consequence(MenuCallbackArgs args)
		{
			Clan.PlayerClan.Influence -= (float)Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAbandoningArmy();
			PlayerEncounter.Current.ForceRaid = false;
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x0012E53F File Offset: 0x0012C73F
		private static void village_player_raid_ended_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x0012E548 File Offset: 0x0012C748
		private static void village_raid_ended_leaded_by_someone_else_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army == null)
			{
				MobileParty shortTermTargetParty = MobileParty.MainParty.ShortTermTargetParty;
				if (((shortTermTargetParty != null) ? shortTermTargetParty.Ai.AiBehaviorPartyBase : null) != null && MobileParty.MainParty.ShortTermTargetParty.Ai.AiBehaviorPartyBase.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					goto IL_8F;
				}
			}
			if (MobileParty.MainParty.Ai.AiBehaviorPartyBase == null || !MobileParty.MainParty.Ai.AiBehaviorPartyBase.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				if (MobileParty.MainParty.Army == null || (Settlement)MobileParty.MainParty.Army.AiBehaviorObject == null || ((Settlement)MobileParty.MainParty.Army.AiBehaviorObject).MapFaction == null)
				{
					MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=3OW1QQNx}The raid was ended by the battle outside of the village.", null), false);
					return;
				}
				if (((Settlement)MobileParty.MainParty.Army.AiBehaviorObject).MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=iyJaisRb}Village is successfully raided by the army you are following.", null), false);
					return;
				}
				MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=JNGUIIQ1}Village is saved by the army you are following.", null), false);
				return;
			}
			IL_8F:
			MobileParty shortTermTargetParty2 = MobileParty.MainParty.ShortTermTargetParty;
			if ((((shortTermTargetParty2 != null) ? shortTermTargetParty2.Ai.AiBehaviorPartyBase : null) != null) ? MobileParty.MainParty.ShortTermTargetParty.Ai.AiBehaviorPartyBase.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction) : MobileParty.MainParty.Ai.AiBehaviorPartyBase.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=04tBEafz}Village is successfully raided by your help.", null), false);
				return;
			}
			MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", new TextObject("{=2Ixb5OKD}Village is successfully saved by your help.", null), false);
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x0012E72C File Offset: 0x0012C92C
		private static void village_player_raid_ended_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.LastVisitedSettlement != null && MobileParty.MainParty.LastVisitedSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", "{=4YuFTXxC}You successfully raided the village.", false);
				return;
			}
			MBTextManager.SetTextVariable("VILLAGE_ENCOUNTER_RESULT", "{=aih1Y62W}You have saved the village.", false);
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x0012E786 File Offset: 0x0012C986
		private static void village_raid_ended_leaded_by_someone_else_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				GameMenu.SwitchToMenu("army_wait");
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0012E7BC File Offset: 0x0012C9BC
		private static void game_menu_warn_player_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MBTextManager.SetTextVariable("KINGDOM", currentSettlement.MapFaction.IsKingdomFaction ? ((Kingdom)currentSettlement.MapFaction).EncyclopediaTitle : currentSettlement.MapFaction.Name, false);
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x0012E804 File Offset: 0x0012CA04
		private static void game_menu_village_hostile_menu_on_init(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveEncounter = false;
			if (Campaign.Current.GameMenuManager.NextLocation != null)
			{
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(Campaign.Current.GameMenuManager.NextLocation, Campaign.Current.GameMenuManager.PreviousLocation, null, null);
				Campaign.Current.GameMenuManager.NextLocation = null;
				Campaign.Current.GameMenuManager.PreviousLocation = null;
				return;
			}
			if (Settlement.CurrentSettlement.SettlementHitPoints <= 0f)
			{
				GameMenu.ActivateGameMenu("RaidCompleted");
			}
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x0012E890 File Offset: 0x0012CA90
		private static bool game_menu_village_hostile_action_on_condition(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && village != null && Hero.MainHero.MapFaction != village.Owner.MapFaction && village.VillageState == Village.VillageStates.Normal;
		}

		// Token: 0x06003DCF RID: 15823 RVA: 0x0012E8F8 File Offset: 0x0012CAF8
		private static void game_menu_village_hostile_action_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_hostile_action");
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x0012E904 File Offset: 0x0012CB04
		private bool game_menu_village_hostile_action_take_food_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveGoods;
			this.CheckVillageAttackableHonorably(args);
			if (this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				if (this._villageLastHostileActionTimeDictionary[Settlement.CurrentSettlement.StringId].ElapsedDaysUntilNow <= 10f)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=mvhyI8Hb}You have already done hostile action in this village recently.", null);
				}
				else
				{
					this._villageLastHostileActionTimeDictionary.Remove(Settlement.CurrentSettlement.StringId);
				}
			}
			return true;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x0012E98B File Offset: 0x0012CB8B
		private void game_menu_village_hostile_action_forget_it_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x0012E997 File Offset: 0x0012CB97
		private void game_menu_village_hostile_action_take_food_on_consequence(MenuCallbackArgs args)
		{
			this._lastSelectedHostileAction = VillageHostileActionCampaignBehavior.HostileAction.TakeSupplies;
			PlayerEncounter.Current.ForceSupplies = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0012E9B8 File Offset: 0x0012CBB8
		private bool game_menu_village_hostile_action_force_volunteers_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.ForceToGiveTroops;
			this.CheckVillageAttackableHonorably(args);
			CampaignTime campaignTime;
			if (this._villageLastHostileActionTimeDictionary.TryGetValue(Settlement.CurrentSettlement.StringId, out campaignTime) && campaignTime.ElapsedDaysUntilNow <= 10f)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=mvhyI8Hb}You have already done hostile action in this village recently.", null);
			}
			else if (this._villageLastHostileActionTimeDictionary.ContainsKey(Settlement.CurrentSettlement.StringId))
			{
				this._villageLastHostileActionTimeDictionary.Remove(Settlement.CurrentSettlement.StringId);
			}
			return true;
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x0012EA4A File Offset: 0x0012CC4A
		private void game_menu_village_hostile_action_force_volunteers_on_consequence(MenuCallbackArgs args)
		{
			this._lastSelectedHostileAction = VillageHostileActionCampaignBehavior.HostileAction.GetVolunteers;
			PlayerEncounter.Current.ForceVolunteers = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x0012EA68 File Offset: 0x0012CC68
		private bool game_menu_village_hostile_action_raid_village_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			this.CheckVillageAttackableHonorably(args);
			return !FactionManager.IsAlliedWithFaction(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x0012EA94 File Offset: 0x0012CC94
		private void game_menu_village_hostile_action_raid_village_warn_continue_on_consequence(MenuCallbackArgs args)
		{
			this._lastSelectedHostileAction = VillageHostileActionCampaignBehavior.HostileAction.RaidTheVillage;
			PlayerEncounter.Current.ForceRaid = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0012EAB2 File Offset: 0x0012CCB2
		private void game_menu_village_hostile_action_raid_village_warn_leave_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_hostile_action");
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x0012EAC4 File Offset: 0x0012CCC4
		private void game_menu_village_hostile_action_raid_village_on_consequence(MenuCallbackArgs args)
		{
			if (!FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.MapFaction))
			{
				GameMenu.SwitchToMenu("raid_village_no_resist_warn_player");
				return;
			}
			this._lastSelectedHostileAction = VillageHostileActionCampaignBehavior.HostileAction.RaidTheVillage;
			PlayerEncounter.Current.ForceRaid = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x0012EB13 File Offset: 0x0012CD13
		private void game_menu_villagers_resist_attack_resistance_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x0012EB20 File Offset: 0x0012CD20
		private void CheckVillageAttackableHonorably(MenuCallbackArgs args)
		{
			Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
			IFaction faction = (currentSettlement != null) ? currentSettlement.MapFaction : null;
			this.CheckFactionAttackableHonorably(args, faction);
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x0012EB4C File Offset: 0x0012CD4C
		private void CheckFactionAttackableHonorably(MenuCallbackArgs args, IFaction faction)
		{
			if (faction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = this.EnemyNotAttackableTooltip;
			}
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x0012EB7C File Offset: 0x0012CD7C
		private bool game_menu_no_resist_plunder_village_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Raid;
			return !this.IsThereAnyDefence() && !FactionManager.IsAlliedWithFaction(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x0012EBAC File Offset: 0x0012CDAC
		private void game_menu_villagers_resist_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			Village village = currentSettlement.Village;
			if (PlayerEncounter.Battle != null)
			{
				PlayerEncounter.Update();
			}
			else if (currentSettlement.SettlementHitPoints <= 0f)
			{
				GameMenu.SwitchToMenu("village_looted");
				return;
			}
			if (!this.game_menu_villagers_resist_attack_resistance_on_condition(args))
			{
				if (this._lastSelectedHostileAction == VillageHostileActionCampaignBehavior.HostileAction.TakeSupplies)
				{
					GameMenu.SwitchToMenu("village_take_food_confirm");
				}
				else if (this._lastSelectedHostileAction == VillageHostileActionCampaignBehavior.HostileAction.GetVolunteers)
				{
					GameMenu.SwitchToMenu("village_press_into_service_confirm");
				}
			}
			MBTextManager.SetTextVariable("STATE", GameTexts.FindText(this.IsThereAnyDefence() ? "str_raid_resist" : "str_village_raid_villagers_are_nonresistant", null), false);
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x0012EC44 File Offset: 0x0012CE44
		private static void game_menu_village_start_attack_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (PlayerEncounter.Battle != null)
			{
				PlayerEncounter.Update();
			}
			else if (currentSettlement.SettlementHitPoints <= 0f)
			{
				GameMenu.SwitchToMenu("village_looted");
			}
			float lastDemandSatisfiedTime = currentSettlement.Village.LastDemandSatisfiedTime;
			if (lastDemandSatisfiedTime > 0f && (Campaign.CurrentTime - lastDemandSatisfiedTime) / 24f < 7f)
			{
				GameTexts.SetVariable("STATE", GameTexts.FindText("str_villiger_recently_satisfied_demands", null));
				return;
			}
			GameTexts.SetVariable("STATE", GameTexts.FindText("str_villigers_grab_their_tools", null));
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x0012ECCF File Offset: 0x0012CECF
		private static bool game_menu_menu_village_take_food_success_take_supplies_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return true;
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x0012ECDA File Offset: 0x0012CEDA
		private bool game_menu_villagers_resist_attack_resistance_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			return this.IsThereAnyDefence();
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x0012ECF0 File Offset: 0x0012CEF0
		private bool IsThereAnyDefence()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement != null)
			{
				foreach (MobileParty mobileParty in currentSettlement.Parties)
				{
					if (!mobileParty.IsMainParty && mobileParty.MapFaction == currentSettlement.MapFaction && mobileParty.Party.NumberOfHealthyMembers > 0)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x0012ED70 File Offset: 0x0012CF70
		public static void game_menu_menu_village_take_food_success_let_them_keep_it_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x0012ED7C File Offset: 0x0012CF7C
		public static bool game_menu_menu_village_take_food_success_let_them_keep_it_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x0012ED86 File Offset: 0x0012CF86
		public static void hostile_action_village_on_init(MenuCallbackArgs args)
		{
			MBTextManager.SetTextVariable("VILLAGE_NAME", PlayerEncounter.EncounterSettlement.Name, false);
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0012ED9D File Offset: 0x0012CF9D
		public static void wait_menu_raiding_village_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(1f - PlayerEncounter.Battle.MapEventSettlement.SettlementHitPoints);
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0012EDC4 File Offset: 0x0012CFC4
		public static bool wait_menu_start_raiding_on_condition(MenuCallbackArgs args)
		{
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", PlayerEncounter.Battle.MapEventSettlement.Name, false);
			return true;
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x0012EDE1 File Offset: 0x0012CFE1
		public static void wait_menu_end_raiding_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Current.ForceRaid = false;
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x0012EDF4 File Offset: 0x0012CFF4
		private static void game_menu_settlement_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			Campaign.Current.SaveHandler.SignalAutoSave();
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0012EE10 File Offset: 0x0012D010
		[GameMenuInitializationHandler("village_player_raid_ended")]
		public static void game_menu_village_raid_ended_menu_sound_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_raiding_village");
			if (MobileParty.MainParty.LastVisitedSettlement != null && MobileParty.MainParty.LastVisitedSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				args.MenuContext.SetAmbientSound("event:/map/ambient/node/settlements/2d/village_raided");
			}
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0012EE69 File Offset: 0x0012D069
		[GameMenuInitializationHandler("village_looted")]
		[GameMenuInitializationHandler("village_raid_ended_leaded_by_someone_else")]
		[GameMenuInitializationHandler("raiding_village")]
		private static void game_menu_ui_village_hostile_raid_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_raiding_village");
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x0012EE7C File Offset: 0x0012D07C
		[GameMenuInitializationHandler("village_hostile_action")]
		[GameMenuInitializationHandler("force_volunteers_village")]
		[GameMenuInitializationHandler("force_supplies_village")]
		[GameMenuInitializationHandler("raid_village_no_resist_warn_player")]
		[GameMenuInitializationHandler("raid_village_resisted")]
		[GameMenuInitializationHandler("village_loot_no_resist")]
		[GameMenuInitializationHandler("village_take_food_confirm")]
		[GameMenuInitializationHandler("village_press_into_service_confirm")]
		[GameMenuInitializationHandler("menu_press_into_service_success")]
		[GameMenuInitializationHandler("menu_village_take_food_success")]
		public static void game_menu_village_menu_on_init(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x0012EEA5 File Offset: 0x0012D0A5
		private static bool continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x0012EEB0 File Offset: 0x0012D0B0
		private static bool back_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x0400124B RID: 4683
		private readonly TextObject EnemyNotAttackableTooltip = GameTexts.FindText("str_enemy_not_attackable_tooltip", null);

		// Token: 0x0400124C RID: 4684
		private VillageHostileActionCampaignBehavior.HostileAction _lastSelectedHostileAction;

		// Token: 0x0400124D RID: 4685
		private Dictionary<string, CampaignTime> _villageLastHostileActionTimeDictionary = new Dictionary<string, CampaignTime>();

		// Token: 0x0400124E RID: 4686
		private const float IntervalForHostileActionAsDay = 10f;

		// Token: 0x02000750 RID: 1872
		private enum HostileAction
		{
			// Token: 0x04001EC3 RID: 7875
			RaidTheVillage,
			// Token: 0x04001EC4 RID: 7876
			TakeSupplies,
			// Token: 0x04001EC5 RID: 7877
			GetVolunteers
		}
	}
}
