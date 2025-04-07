using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200038D RID: 909
	public class EncounterGameMenuBehavior : CampaignBehaviorBase
	{
		// Token: 0x060035F6 RID: 13814 RVA: 0x000F01AC File Offset: 0x000EE3AC
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<TroopRoster>("_breakInOutCasualties", ref this._breakInOutCasualties);
			dataStore.SyncData<int>("_intEncounterVariable", ref this._intEncounterVariable);
			dataStore.SyncData<int>("_breakInOutArmyCasualties", ref this._breakInOutArmyCasualties);
			dataStore.SyncData<TroopRoster>("_getAwayCasualties", ref this._getAwayCasualties);
			dataStore.SyncData<ItemRoster>("_getAwayLostBaggage", ref this._getAwayLostBaggage);
			dataStore.SyncData<bool>("_playerIsAlreadyInCastle", ref this._playerIsAlreadyInCastle);
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000F0228 File Offset: 0x000EE428
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x000F0294 File Offset: 0x000EE494
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			IFaction mapFaction = siegeEvent.BesiegerCamp.LeaderParty.MapFaction;
			if (siegeEvent.IsPlayerSiegeEvent && mapFaction != null && mapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				mapFaction.NotAttackableByPlayerUntilTime = CampaignTime.Zero;
			}
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x000F02D8 File Offset: 0x000EE4D8
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (mapEvent.IsPlayerMapEvent && attackerParty.MapFaction != null && attackerParty.MapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				attackerParty.MapFaction.NotAttackableByPlayerUntilTime = CampaignTime.Zero;
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x000F031A File Offset: 0x000EE51A
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party == MobileParty.MainParty)
			{
				this._playerIsAlreadyInCastle = false;
			}
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000F032B File Offset: 0x000EE52B
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeAccessDetails();
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000F033C File Offset: 0x000EE53C
		private void InitializeAccessDetails()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement != null && (currentSettlement.IsFortification || currentSettlement.IsVillage))
			{
				Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(Settlement.CurrentSettlement, out this._accessDetails);
			}
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x000F0384 File Offset: 0x000EE584
		private void AddGameMenus(CampaignGameStarter gameSystemInitializer)
		{
			gameSystemInitializer.AddGameMenu("taken_prisoner", "{=ezClQMBj}Your enemies take you as a prisoner.", new OnInitDelegate(this.game_menu_taken_prisoner_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("taken_prisoner", "taken_prisoner_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_taken_prisoner_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_taken_prisoner_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("defeated_and_taken_prisoner", "{=ezClQMBj}Your enemies take you as a prisoner.", new OnInitDelegate(this.game_menu_taken_prisoner_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("defeated_and_taken_prisoner", "taken_prisoner_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_taken_prisoner_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_taken_prisoner_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_meeting", "{=!}.", new OnInitDelegate(this.game_menu_encounter_meeting_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("join_encounter", "{=jKWJpIES}{JOIN_ENCOUNTER_TEXT}. You decide to...", new OnInitDelegate(this.game_menu_join_encounter_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_help_attackers", "{=h3yEHb4U}Help {ATTACKER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_attackers_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_attackers_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_help_defenders", "{=FwIgakj8}Help {DEFENDER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_defenders_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_defenders_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_abandon", "{=Nr49hlfC}Abandon army.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_abandon_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_abandon_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_encounter", "join_encounter_leave", "{=!}{LEAVE_TEXT}", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_leave_no_army_on_condition), delegate(MenuCallbackArgs args)
			{
				if (MobileParty.MainParty.SiegeEvent != null && MobileParty.MainParty.SiegeEvent.BesiegerCamp != null && MobileParty.MainParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(PartyBase.MainParty, MapEvent.BattleTypes.Siege))
				{
					MobileParty.MainParty.BesiegerCamp = null;
				}
				PlayerEncounter.Finish(true);
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("join_sally_out", "{=CcNVobQU}Garrison of the settlement you are in decided to sally out. You decide to...", new OnInitDelegate(this.game_menu_join_sally_out_on_init), GameOverlays.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("join_sally_out", "join_siege_event", "{=IaA8sbY2}Join the sally out", new GameMenuOption.OnConditionDelegate(this.game_menu_join_sally_out_event_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_sally_out_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_sally_out", "join_siege_event_break_in", "{=z1RHDsOG}Stay in settlement", new GameMenuOption.OnConditionDelegate(this.game_menu_stay_in_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_stay_in_settlement_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("join_siege_event", "{=xNyKVMHx}{JOIN_SIEGE_TEXT} You decide to...", new OnInitDelegate(this.game_menu_join_siege_event_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "join_siege_event", "{=ZVsJf5Ff}Join the continuing siege.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_siege_event_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_siege_event_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "attack_besiegers", "{=CVg3P07C}Assault the siege camp.", new GameMenuOption.OnConditionDelegate(this.attack_besieger_side_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_defenders_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "join_siege_event_break_in", "{=XAvwP3Ce}Break in to help the defenders", new GameMenuOption.OnConditionDelegate(this.break_in_to_help_defender_side_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_siege_event_on_defender_side_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("join_siege_event", "join_encounter_leave", "{=ebUwP3Q3}Don't get involved.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				PlayerEncounter.Finish(true);
				if (Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Army != null && Hero.MainHero.PartyBelongedTo.Army.LeaderParty != MobileParty.MainParty)
				{
					Hero.MainHero.PartyBelongedTo.Army = null;
					MobileParty.MainParty.Ai.SetMoveModeHold();
				}
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("siege_attacker_left", "{=LR6Y57Rq}Attackers abandoned the siege.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_left", "siege_attacker_left_return_to_settlement", "{=j7bZRFxc}Return to {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_left", "siege_attacker_left_leave", "{=mfAP8Wlq}Leave settlement.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_left_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				PlayerEncounter.Finish(true);
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("siege_attacker_defeated", "{=njbpMLdJ}Attackers have been defeated.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_defeated", "siege_attacker_defeated_return_to_settlement", "{=j7bZRFxc}Return to {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_siege_attacker_left_return_to_settlement_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("siege_attacker_defeated", "siege_attacker_defeated_leave", "{=mfAP8Wlq}Leave settlement.", new GameMenuOption.OnConditionDelegate(this.game_menu_siege_attacker_defeated_leave_on_condition), delegate(MenuCallbackArgs args)
			{
				PlayerEncounter.Finish(true);
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter", "{=!}{ENCOUNTER_TEXT}", new OnInitDelegate(this.game_menu_encounter_on_init), GameOverlays.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "continue_preparations", "{=FOoMM4AU}Continue siege preparations.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_besiege_continue_siege_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_besiege_continue_siege_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "village_raid_action", "{=lvttCRi8}Plunder the village, then raze it.", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_raid_no_resist_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "village_force_volunteer_action", "{=9YHjPkb8}Force notables to give you recruits.", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_force_volunteers_no_resist_loot_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "village_force_supplies_action", "{=JMzyh6Gl}Force people to give you supplies.", new GameMenuOption.OnConditionDelegate(this.game_menu_village_hostile_action_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_force_supplies_no_resist_loot_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "attack", "{=o1pZHZOF}{ATTACK_TEXT}!", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "capture_the_enemy", "{=27yneDGL}Capture the enemy.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_capture_the_enemy_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_capture_the_enemy_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "str_order_attack", "{=!}{SEND_TROOPS_TEXT}", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_order_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_order_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "leave_soldiers_behind", "{=qNgGoqmI}Try to get away.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_leave_your_soldiers_behind_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("try_to_get_away");
			}, false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "surrender", "{=3nT5wWzb}Surrender.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_surrender_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "abandon_army", "{=Nr49hlfC}Abandon army.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_abandon_army_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_abandon_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter", "go_back_to_settlement", "{=j7bZRFxc}Return to {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_sally_out_go_back_to_settlement_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_sally_out_go_back_to_settlement_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("army_encounter", "{=!}{ARMY_ENCOUNTER_TEXT}", new OnInitDelegate(this.game_menu_army_encounter_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_talk_to_leader", "{=tYVW8iQN}Talk to army leader", new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_leader_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_leader_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_talk_to_other_members", "{=b7APCGY2}Talk to other members", new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_other_members_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_other_members_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_join_army", "{=N4Qa0WsT}Join army", new GameMenuOption.OnConditionDelegate(this.game_menu_army_join_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_join_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_attack_army", "{=0URijoc0}Attack army", new GameMenuOption.OnConditionDelegate(this.game_menu_army_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("army_encounter", "army_leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_army_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.army_encounter_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("game_menu_army_talk_to_other_members", "{=yYTotiqW}Talk to...", new OnInitDelegate(this.game_menu_army_talk_to_other_members_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("game_menu_army_talk_to_other_members", "game_menu_army_talk_to_other_members_item", "{=!}{CHAR_NAME}", new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_other_members_item_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_other_members_item_on_consequence), false, -1, true, null);
			gameSystemInitializer.AddGameMenuOption("game_menu_army_talk_to_other_members", "game_menu_army_talk_to_other_members_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_army_talk_to_other_members_back_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_talk_to_other_members_back_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("try_to_get_away", "{=bwuawgrW}As the highest tactics skilled member {HIGHEST_TACTICS_SKILLED_MEMBER} ({HIGHEST_TACTICS_SKILL}) devise a plan to disperse into the wilderness to break away from your enemies. You and most men may escape with your lives, but as many as {NEEEDED_MEN_COUNT} {SOLDIER_OR_SOLDIERS} may be lost and part of your baggage could be captured.", new OnInitDelegate(this.game_menu_leave_soldiers_behind_on_init), GameOverlays.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("try_to_get_away", "try_to_get_away_accept", "{=DbOv36TA}Go ahead with that.", new GameMenuOption.OnConditionDelegate(this.game_menu_try_to_get_away_accept_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_leave_your_soldiers_behind_accept_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("try_to_get_away", "try_to_get_away_reject", "{=f1etg9oL}Think of something else.", new GameMenuOption.OnConditionDelegate(this.game_menu_try_to_get_away_reject_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("encounter");
			}, true, -1, false, null);
			gameSystemInitializer.AddGameMenu("try_to_get_away_debrief", "{=ruU70rFl}You disperse into the shrubs and bushes. The enemies halt and seem to hesitate for a while before resuming their pursuit. {CASUALTIES_AND_LOST_BAGGAGE}", new OnInitDelegate(this.game_menu_get_away_debrief_on_init), GameOverlays.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("try_to_get_away_debrief", "try_to_get_away_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.game_menu_try_to_get_away_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_try_to_get_away_end), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("assault_town", "", new OnInitDelegate(this.game_menu_town_assault_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("assault_town_order_attack", "", new OnInitDelegate(this.game_menu_town_assault_order_attack_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("town_outside", "{=!}{TOWN_TEXT}", new OnInitDelegate(this.game_menu_town_outside_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "approach_gates", "{=XlbDnuJx}Approach the gates and hail the guard.", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_approach_gates_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_outside_approach_gates_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_disguise_yourself", "{=VCREeAF1}Disguise yourself and sneak through the gate. ({SNEAK_CHANCE}%)", new GameMenuOption.OnConditionDelegate(this.game_menu_town_disguise_yourself_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_disguise_yourself_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_besiege", "{=WdIGdHuL}Besiege the town.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_town_besiege_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_town_besiege_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_enter_cheat", "{=!}Enter town (Cheat).", new GameMenuOption.OnConditionDelegate(this.game_menu_town_outside_cheat_enter_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_outside_enter_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_outside", "town_outside_leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_outside_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_sneak_into_town_succeeded", "{=pSSDfAjR}Disguised in the garments of a poor pilgrim, you fool the guards and make your way into the town.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_sneak_into_town_succeeded", "str_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.menu_sneak_into_town_succeeded_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.menu_sneak_into_town_succeeded_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_sneak_into_town_caught", "{=u7yLV7Vr}As you try to sneak in, one of the guards recognizes you and raises the alarm! Another quickly slams the gate shut behind you, and you have no choice but to give up.", new OnInitDelegate(EncounterGameMenuBehavior.game_menu_sneak_into_town_caught_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_sneak_into_town_caught", "mno_sneak_caught_surrender", "{=3nT5wWzb}Surrender.", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.mno_sneak_caught_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.mno_sneak_caught_surrender_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_captivity_castle_taken_prisoner", "{=AFJ3BvTH}You are quickly surrounded by guards who take away your weapons. With curses and insults, they throw you into the dungeon where you must while away the miserable days of your captivity.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_captivity_castle_taken_prisoner", "mno_sneak_caught_surrender", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.game_menu_captivity_castle_taken_prisoner_cont_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_menu_captivity_castle_taken_prisoner_cont_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_captivity_castle_taken_prisoner", "cheat_continue", "{=!}Cheat : Leave.", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.game_menu_captivity_taken_prisoner_cheat_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_menu_captivity_taken_prisoner_cheat_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("fortification_crime_rating", "{=!}{FORTIFICATION_CRIME_RATING_TEXT}", new OnInitDelegate(this.game_menu_fortification_high_crime_rating_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("fortification_crime_rating", "fortification_crime_rating_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_fortification_high_crime_rating_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_fortification_high_crime_rating_continue_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("army_left_settlement_due_to_war_declaration", "{=!}{ARMY_LEFT_SETTLEMENT_DUE_TO_WAR_TEXT}", new OnInitDelegate(this.game_menu_army_left_settlement_due_to_war_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("army_left_settlement_due_to_war_declaration", "army_left_settlement_due_to_war_declaration_continue", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_army_left_settlement_due_to_war_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_army_left_settlement_due_to_war_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("castle_outside", "{=!}{TOWN_TEXT}", new OnInitDelegate(this.game_menu_castle_outside_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("castle_outside", "approach_gates", "{=XlbDnuJx}Approach the gates and hail the guard.", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_approach_gates_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_outside_approach_gates_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_outside", "town_besiege", "{=UzMYZgoE}Besiege the castle.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_town_besiege_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_town_besiege_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_outside", "town_outside_leave", "{=2YYRyrOO}Leave...", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_outside_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("town_guard", "{=SxkaQbSa}You approach the gate. The men on the walls watch you closely.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("town_guard", "request_meeting_commander", "{=RSQbOjub}Request a meeting with someone.", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_someone_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_someone_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_guard", "guard_discuss_criminal_surrender", "{=ACvQdkG8}Discuss the terms of your surrender", new GameMenuOption.OnConditionDelegate(this.outside_menu_criminal_on_condition), new GameMenuOption.OnConsequenceDelegate(this.outside_menu_criminal_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_guard", "guard_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_guard_back_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("castle_guard", "{=SxkaQbSa}You approach the gate. The men on the walls watch you closely.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("castle_guard", "request_shelter", "{=mG9jW8Fp}Request entry to the castle.", new GameMenuOption.OnConditionDelegate(this.game_menu_town_guard_request_shelter_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_entry_to_castle_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_guard", "request_meeting_commander", "{=RSQbOjub}Request a meeting with someone.", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_someone_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_someone_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("castle_guard", "guard_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(this.game_menu_castle_outside_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_town_guard_back_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_castle_entry_granted", "{=Mg1PotzO}After a brief wait, the guards open the gates for you and allow your party inside.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_castle_entry_granted", "str_continue", "{=bLNocKd1}Continue..", new GameMenuOption.OnConditionDelegate(EncounterGameMenuBehavior.game_request_entry_to_castle_approved_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_request_entry_to_castle_approved_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_castle_entry_denied", "{=QpQQJjD6}The lord of this castle has forbidden you from coming inside these walls, and the guard sergeant informs you that his men will fire if you attempt to come any closer.", new OnInitDelegate(EncounterGameMenuBehavior.menu_castle_entry_denied_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_castle_entry_denied", "str_continue", "{=veWOovVv}Continue...", null, new GameMenuOption.OnConsequenceDelegate(EncounterGameMenuBehavior.game_request_entry_to_castle_rejected_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("request_meeting", "{=pBAx7jTM}With whom do you want to meet?", new OnInitDelegate(this.game_menu_town_menu_request_meeting_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting", "request_meeting_with", "{=!}{HERO_TO_MEET.LINK}", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_with_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_with_on_consequence), false, -1, true, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting", "meeting_town_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_town_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting", "meeting_castle_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_castle_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_castle_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("request_meeting_with_besiegers", "{=pBAx7jTM}With whom do you want to meet?", new OnInitDelegate(this.game_menu_town_menu_request_meeting_with_besiegers_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting_with_besiegers", "request_meeting_with", "{=!}{PARTY_LEADER.LINK}", new GameMenuOption.OnConditionDelegate(this.game_menu_request_meeting_with_besiegers_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_with_besiegers_on_consequence), false, -1, true, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting_with_besiegers", "request_meeting_town_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_town_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_town_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("request_meeting_with_besiegers", "request_meeting_castle_leave", "{=3nbuRBJK}Forget it.", new GameMenuOption.OnConditionDelegate(this.game_meeting_castle_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_request_meeting_castle_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("village_outside", "{=!}.", new OnInitDelegate(this.VillageOutsideOnInit), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("village_loot_complete", "{=qt5bkw8l}On your orders your troops sack the village, pillaging everything of any value, and then put the buildings to the torch. From the coins and valuables that are found, you get your share.", new OnInitDelegate(this.game_menu_village_loot_complete_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("village_loot_complete", "continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.game_menu_village_loot_complete_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_village_loot_complete_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("raid_interrupted", "{=KW7amS8c}While your troops are pillaging the countryside, you receive news that the enemy is approaching. You quickly gather up your soldiers and prepare for battle.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("raid_interrupted", "continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.game_menu_raid_interrupted_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_raid_interrupted_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_interrupted", "{=lKWflUid}While you are waiting in {DEFENDER}, {ATTACKER} started an attack on it.", new OnInitDelegate(this.game_menu_encounter_interrupted_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted", "encounter_interrupted_help_attackers", "{=h3yEHb4U}Help {ATTACKER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_attackers_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_attackers_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted", "encounter_interrupted_help_defenders", "{=FwIgakj8}Help {DEFENDER}.", new GameMenuOption.OnConditionDelegate(this.game_menu_join_encounter_help_defenders_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_join_encounter_help_defenders_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted", "leave", "{=UgfmaQgx}Leave {DEFENDER}", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_interrupted_siege_preparations", "{=ABeCWcLi}While you are resting, you hear news that a force led by {ATTACKER} has arrived outside the walls of {DEFENDER} and is beginning preparations for a siege.", new OnInitDelegate(this.game_menu_encounter_interrupted_siege_preparations_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_siege_preparations", "encounter_interrupted_siege_preparations_join_defend", "{=Lxx97yNh}Join the defense of {SETTLEMENT}", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_siege_preparations_join_defend_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_siege_preparations_join_defend_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_siege_preparations", "encounter_interrupted_siege_preparations_break_out_of_town", "{=ybzBF59f}Break out of {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_siege_preparations_break_out_of_town_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_break_out_of_town_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_siege_preparations", "encounter_interrupted_siege_preparations_leave_town", "{=FILG5eZD}Leave {SETTLEMENT}.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_siege_preparations_leave_town_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_leave_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("encounter_interrupted_raid_started", "{=7o4AfEhN}While you are resting, you hear news that a force led by {ATTACKER} has arrived outside of {DEFENDER} to raid it.", new OnInitDelegate(this.game_menu_encounter_interrupted_by_raid_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("encounter_interrupted_raid_started", "encounter_interrupted_raid_started_leave", "{=WVkc4UgX}Continue.", new GameMenuOption.OnConditionDelegate(this.game_menu_encounter_interrupted_by_raid_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_interrupted_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("continue_siege_after_attack", "{=CVp0j9al}You have defeated the enemies outside the walls. Now you decide to...", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("continue_siege_after_attack", "continue_siege", "{=zeKvSEpN}Continue the siege", new GameMenuOption.OnConditionDelegate(this.continue_siege_after_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.continue_siege_after_attack_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("continue_siege_after_attack", "leave_siege", "{=b7UHp4J9}Leave the siege", new GameMenuOption.OnConditionDelegate(this.leave_siege_after_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.leave_siege_after_attack_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("continue_siege_after_attack", "leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(this.leave_army_after_attack_on_condition), new GameMenuOption.OnConsequenceDelegate(this.leave_army_after_attack_on_consequence), true, -1, false, null);
			gameSystemInitializer.AddGameMenu("town_caught_by_guards", "{=gVuF84RZ}Guards caught you", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("town_caught_by_guards", "town_caught_by_guards_criminal_outside_menu_give_yourself_up", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.outside_menu_criminal_on_condition), new GameMenuOption.OnConsequenceDelegate(this.caught_outside_menu_criminal_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("town_caught_by_guards", "town_caught_by_guards_enemy_outside_menu_give_yourself_up", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.caught_outside_menu_enemy_on_condition), new GameMenuOption.OnConsequenceDelegate(this.caught_outside_menu_enemy_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_in_menu", "{=sd15CQHI}You devised a plan to distract the besiegers so you can rush the fortress gates, expecting the defenders to let you in. You and most of your men may get through, but as many as {POSSIBLE_CASUALTIES} {?PLURAL}troops{?}troop{\\?} may be lost.", new OnInitDelegate(this.break_in_out_menu_on_init), GameOverlays.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_in_menu", "break_in_menu_accept", "{=DbOv36TA}Go ahead with that.", new GameMenuOption.OnConditionDelegate(this.break_in_menu_accept_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_menu_accept_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("break_in_menu", "break_in_menu_reject", "{=f1etg9oL}Think of something else.", new GameMenuOption.OnConditionDelegate(this.break_in_menu_reject_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_menu_reject_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_in_debrief_menu", "{=PHe0oco1}You fought your way through the attackers to reach the gates. The defenders open them quickly to let you through. When the gates are safely closed behind you, you take a quick tally only to see you have lost the following: {CASUALTIES}.{OTHER_CASUALTIES}", new OnInitDelegate(this.break_in_out_debrief_menu_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_in_debrief_menu", "break_in_debrief_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.break_in_debrief_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_in_debrief_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_out_menu", "{=J1aEaygO}You devised a plan to fight your way through the attackers to escape from the fortress. You and most of your men may survive, but as many as {POSSIBLE_CASUALTIES} {?PLURAL}troops{?}troop{\\?} may be lost.", new OnInitDelegate(this.break_in_out_menu_on_init), GameOverlays.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_out_menu", "break_out_menu_accept", "{=DbOv36TA}Go ahead with that.", new GameMenuOption.OnConditionDelegate(this.break_out_menu_accept_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_out_menu_accept_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("break_out_menu", "break_out_menu_reject", "{=f1etg9oL}Think of something else.", new GameMenuOption.OnConditionDelegate(this.break_out_menu_reject_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_out_menu_reject_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("break_out_debrief_menu", "{=OzyrsZZK}You fought your way through the attackers to escape from the settlement. When, after some time your forces regroup, you take a quick tally only to see you have lost the following: {CASUALTIES}.{OTHER_CASUALTIES}", new OnInitDelegate(this.break_in_out_debrief_menu_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("break_out_debrief_menu", "break_out_debrief_continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.break_out_debrief_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.break_out_debrief_continue_on_consequence), false, -1, false, null);
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x000F18FC File Offset: 0x000EFAFC
		private bool game_menu_encounter_army_lead_inf_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.Infantry));
			}
			return false;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x000F199C File Offset: 0x000EFB9C
		private void game_menu_encounter_army_lead_inf_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000F19A8 File Offset: 0x000EFBA8
		private bool game_menu_encounter_army_lead_arc_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.Ranged));
			}
			return false;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000F1A48 File Offset: 0x000EFC48
		private void game_menu_encounter_army_lead_arc_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x000F1A54 File Offset: 0x000EFC54
		private bool game_menu_encounter_army_lead_cav_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.Cavalry));
			}
			return false;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000F1AF4 File Offset: 0x000EFCF4
		private void game_menu_encounter_army_lead_cav_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x000F1AFD File Offset: 0x000EFCFD
		public static void game_menu_captivity_taken_prisoner_cheat_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000F1B08 File Offset: 0x000EFD08
		private bool game_menu_encounter_army_lead_har_on_condition(MenuCallbackArgs args)
		{
			bool flag = MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == 0;
			if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && !flag)
			{
				return MobileParty.MainParty.MapEvent.PartiesOnSide(MobileParty.MainParty.MapEvent.PlayerSide).Any((MapEventParty party) => party.Party.MemberRoster.GetTroopRoster().Any((TroopRosterElement tr) => tr.Character != null && tr.Character.GetFormationClass() == FormationClass.HorseArcher));
			}
			return false;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000F1BA8 File Offset: 0x000EFDA8
		private void game_menu_encounter_army_lead_har_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x000F1BB4 File Offset: 0x000EFDB4
		private void game_menu_join_encounter_on_init(MenuCallbackArgs args)
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 66233) && PlayerEncounter.Current == null)
			{
				GameMenu.ExitToLast();
				return;
			}
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			PartyBase leaderParty = encounteredBattle.GetLeaderParty(BattleSideEnum.Attacker);
			PartyBase leaderParty2 = encounteredBattle.GetLeaderParty(BattleSideEnum.Defender);
			if (leaderParty.IsMobile && leaderParty.MobileParty.Army != null)
			{
				MBTextManager.SetTextVariable("ATTACKER", leaderParty.MobileParty.ArmyName, false);
			}
			else
			{
				MBTextManager.SetTextVariable("ATTACKER", leaderParty.Name, false);
			}
			if (leaderParty2.IsMobile && leaderParty2.MobileParty.Army != null)
			{
				MBTextManager.SetTextVariable("DEFENDER", leaderParty2.MobileParty.ArmyName, false);
			}
			else
			{
				MBTextManager.SetTextVariable("DEFENDER", leaderParty2.Name, false);
			}
			if (encounteredBattle.IsSallyOut)
			{
				MBTextManager.SetTextVariable("JOIN_ENCOUNTER_TEXT", GameTexts.FindText("str_defenders_make_sally_out", null), false);
				StringHelpers.SetCharacterProperties("BESIEGER_LEADER", Campaign.Current.Models.EncounterModel.GetLeaderOfMapEvent(encounteredBattle, BattleSideEnum.Defender).CharacterObject, null, false);
				return;
			}
			if (leaderParty2.IsSettlement)
			{
				TextObject text = new TextObject("{=kDiN9iYw}{ATTACKER} is besieging the walls of {DEFENDER}", null);
				if (encounteredBattle.IsSiegeAssault)
				{
					Settlement.SiegeState currentSiegeState = leaderParty2.Settlement.CurrentSiegeState;
					if (currentSiegeState != Settlement.SiegeState.OnTheWalls && currentSiegeState == Settlement.SiegeState.InTheLordsHall)
					{
						text = new TextObject("{=oXY2wnic}{ATTACKER} is fighting inside the lord's hall of {DEFENDER}", null);
					}
				}
				else if (encounteredBattle.IsRaid)
				{
					if (encounteredBattle.DefenderSide.TroopCount > 0)
					{
						text = new TextObject("{=kvNQLcCb}{ATTACKER} is fighting in {DEFENDER}", null);
					}
					else
					{
						text = new TextObject("{=BExNNwm0}{ATTACKER} is raiding {DEFENDER}", null);
					}
				}
				MBTextManager.SetTextVariable("JOIN_ENCOUNTER_TEXT", text, false);
				return;
			}
			MBTextManager.SetTextVariable("JOIN_ENCOUNTER_TEXT", GameTexts.FindText("str_come_across_battle", null), false);
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x000F1D5C File Offset: 0x000EFF5C
		private bool game_menu_join_encounter_help_attackers_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			IFaction mapFaction = encounteredBattle.GetLeaderParty(BattleSideEnum.Defender).MapFaction;
			this.CheckFactionAttackableHonorably(args, mapFaction);
			return encounteredBattle.CanPartyJoinBattle(PartyBase.MainParty, BattleSideEnum.Attacker);
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x000F1D98 File Offset: 0x000EFF98
		private void game_menu_join_encounter_help_attackers_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.InsideSettlement && PlayerEncounter.EncounterSettlement.IsUnderSiege)
			{
				PlayerEncounter.LeaveSettlement();
			}
			PlayerEncounter.JoinBattle(BattleSideEnum.Attacker);
			if (PlayerEncounter.Battle.DefenderSide.TroopCount > 0)
			{
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			if (MobileParty.MainParty.Army != null)
			{
				if (MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
				{
					if (!MobileParty.MainParty.Army.LeaderParty.AttachedParties.Contains(MobileParty.MainParty))
					{
						MobileParty.MainParty.Army.AddPartyToMergedParties(MobileParty.MainParty);
						Campaign.Current.CameraFollowParty = MobileParty.MainParty.Army.LeaderParty.Party;
						CampaignEventDispatcher.Instance.OnArmyOverlaySetDirty();
					}
					if (PlayerEncounter.Battle.IsRaid)
					{
						GameMenu.SwitchToMenu("raiding_village");
						return;
					}
					GameMenu.SwitchToMenu("army_wait");
					return;
				}
				else
				{
					if (PlayerEncounter.Battle.IsRaid)
					{
						GameMenu.SwitchToMenu("raiding_village");
						MobileParty.MainParty.Ai.SetMoveModeHold();
						return;
					}
					GameMenu.SwitchToMenu("encounter");
					return;
				}
			}
			else
			{
				if (PlayerEncounter.Battle.IsRaid)
				{
					GameMenu.SwitchToMenu("raiding_village");
					MobileParty.MainParty.Ai.SetMoveModeHold();
					return;
				}
				GameMenu.SwitchToMenu("menu_siege_strategies");
				MobileParty.MainParty.Ai.SetMoveModeHold();
				return;
			}
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x000F1EF3 File Offset: 0x000F00F3
		private bool game_menu_join_encounter_abandon_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x000F1F24 File Offset: 0x000F0124
		private bool game_menu_join_encounter_help_defenders_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			IFaction mapFaction = encounteredBattle.GetLeaderParty(BattleSideEnum.Attacker).MapFaction;
			this.CheckFactionAttackableHonorably(args, mapFaction);
			return encounteredBattle.CanPartyJoinBattle(PartyBase.MainParty, BattleSideEnum.Defender);
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x000F1F5E File Offset: 0x000F015E
		public static bool game_menu_captivity_castle_taken_prisoner_cont_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000F1F6C File Offset: 0x000F016C
		private void game_menu_join_encounter_help_defenders_on_consequence(MenuCallbackArgs args)
		{
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (((encounteredParty != null) ? encounteredParty.MapEvent : null) != null)
			{
				PlayerEncounter.JoinBattle(BattleSideEnum.Defender);
				GameMenu.ActivateGameMenu("encounter");
				return;
			}
			if (PlayerEncounter.Current != null)
			{
				if (PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.SiegeEvent != null && !PlayerEncounter.EncounterSettlement.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction))
				{
					PlayerEncounter.RestartPlayerEncounter(PlayerEncounter.EncounterSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party, PartyBase.MainParty, false);
				}
				GameMenu.ActivateGameMenu("encounter");
			}
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000F2000 File Offset: 0x000F0200
		private void game_menu_join_siege_event_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement.Party.SiegeEvent == null)
			{
				PlayerEncounter.Finish(true);
			}
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000F2020 File Offset: 0x000F0220
		private void game_menu_join_sally_out_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000F2034 File Offset: 0x000F0234
		private bool game_menu_join_siege_event_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			TextObject tooltip;
			if (DiplomacyHelper.DidMainHeroSwornNotToAttackFaction(Settlement.CurrentSettlement.MapFaction, out tooltip))
			{
				args.IsEnabled = false;
				args.Tooltip = tooltip;
			}
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			return siegeEvent != null && siegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Attacker);
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000F208C File Offset: 0x000F028C
		private void game_menu_join_siege_event_on_consequence(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.IsMainParty && !Settlement.CurrentSettlement.SiegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Attacker))
			{
				Debug.FailedAssert("Player should not be able to join this siege.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\EncounterGameMenuBehavior.cs", "game_menu_join_siege_event_on_consequence", 670);
				return;
			}
			MobileParty leaderParty = Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty;
			if (!leaderParty.IsMainParty)
			{
				MobileParty.MainParty.Ai.SetMoveEscortParty(leaderParty);
			}
			if (Settlement.CurrentSettlement.Party.MapEvent != null)
			{
				PlayerEncounter.JoinBattle(BattleSideEnum.Attacker);
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (Hero.MainHero.CurrentSettlement != null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.BesiegerCamp = currentSettlement.SiegeEvent.BesiegerCamp;
			PlayerSiege.StartPlayerSiege(BattleSideEnum.Attacker, false, currentSettlement);
			PlayerSiege.StartSiegePreparation();
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000F2186 File Offset: 0x000F0386
		private bool game_menu_join_sally_out_event_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return true;
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000F2191 File Offset: 0x000F0391
		private bool game_menu_stay_in_settlement_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x000F219C File Offset: 0x000F039C
		private void game_menu_join_sally_out_on_consequence(MenuCallbackArgs args)
		{
			PartyBase sallyOutDefenderLeader = MapEventHelper.GetSallyOutDefenderLeader();
			EncounterManager.StartPartyEncounter(MobileParty.MainParty.Party, sallyOutDefenderLeader);
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x000F21BF File Offset: 0x000F03BF
		private void game_menu_stay_in_settlement_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("menu_siege_strategies");
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x000F21DD File Offset: 0x000F03DD
		private bool break_in_to_help_defender_side_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			return this.common_join_siege_event_button_condition(args);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000F21F0 File Offset: 0x000F03F0
		private bool common_join_siege_event_button_condition(MenuCallbackArgs args)
		{
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			if (siegeEvent != null)
			{
				MobileParty mainParty = MobileParty.MainParty;
				int lostTroopCountForBreakingInBesiegedSettlement = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(mainParty, siegeEvent);
				Army army = mainParty.Army;
				int num = (army != null) ? army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars;
				TextObject tooltip;
				if (DiplomacyHelper.DidMainHeroSwornNotToAttackFaction(siegeEvent.BesiegerCamp.LeaderParty.MapFaction, out tooltip))
				{
					args.IsEnabled = false;
					args.Tooltip = tooltip;
				}
				else if (lostTroopCountForBreakingInBesiegedSettlement > num)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				}
				return siegeEvent.CanPartyJoinSide(MobileParty.MainParty.Party, BattleSideEnum.Defender);
			}
			return false;
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000F22A1 File Offset: 0x000F04A1
		private bool attack_besieger_side_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return this.common_join_siege_event_button_condition(args);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000F22B2 File Offset: 0x000F04B2
		private void game_menu_join_siege_event_on_defender_side_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("break_in_menu");
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x000F22C0 File Offset: 0x000F04C0
		private bool game_menu_join_encounter_leave_no_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			MBTextManager.SetTextVariable("LEAVE_TEXT", "{=ebUwP3Q3}Don't get involved.", false);
			if (MobileParty.MainParty.Army != null)
			{
				Army army = MobileParty.MainParty.Army;
				return ((army != null) ? army.LeaderParty : null) == MobileParty.MainParty;
			}
			return true;
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x000F2310 File Offset: 0x000F0510
		private bool game_menu_join_encounter_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			MobileParty mobileParty = (Settlement.CurrentSettlement.SiegeEvent != null) ? Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty : null;
			return mobileParty == null || !mobileParty.IsMainParty;
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x000F2357 File Offset: 0x000F0557
		private bool game_menu_siege_attacker_left_return_to_settlement_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			GameTexts.SetVariable("SETTLEMENT", MobileParty.MainParty.LastVisitedSettlement.Name);
			return true;
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x000F237C File Offset: 0x000F057C
		private void game_menu_siege_attacker_left_return_to_settlement_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(false);
			}
			if (MobileParty.MainParty.AttachedTo == null)
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, MobileParty.MainParty.LastVisitedSettlement);
			}
			else
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty.AttachedTo, MobileParty.MainParty.LastVisitedSettlement);
			}
			if (PlayerEncounter.Current != null && PlayerEncounter.LocationEncounter == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (string.IsNullOrEmpty(genericStateMenu))
			{
				GameMenu.ExitToLast();
				return;
			}
			GameMenu.SwitchToMenu(genericStateMenu);
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x000F240D File Offset: 0x000F060D
		private bool game_menu_siege_attacker_left_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x000F2418 File Offset: 0x000F0618
		private bool game_menu_siege_attacker_defeated_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x000F2423 File Offset: 0x000F0623
		private bool game_menu_encounter_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.CheatMode;
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x000F2430 File Offset: 0x000F0630
		private void game_menu_encounter_interrupted_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PartyBase leaderParty = PlayerEncounter.EncounteredBattle.GetLeaderParty(BattleSideEnum.Attacker);
			MBTextManager.SetTextVariable("ATTACKER", leaderParty.Name, false);
			MBTextManager.SetTextVariable("DEFENDER", currentSettlement.Name, false);
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x000F2474 File Offset: 0x000F0674
		private void game_menu_encounter_interrupted_siege_preparations_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			TextObject name = Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Name;
			TextObject text = args.MenuContext.GameMenu.GetText();
			text.SetTextVariable("ATTACKER", name);
			text.SetTextVariable("DEFENDER", currentSettlement.Name);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x000F24E4 File Offset: 0x000F06E4
		private bool game_menu_encounter_interrupted_siege_preparations_leave_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			return !FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, Hero.MainHero.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.MapFaction);
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x000F2550 File Offset: 0x000F0750
		private void game_menu_encounter_interrupted_by_raid_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			TextObject name = currentSettlement.Party.MapEvent.GetLeaderParty(currentSettlement.Party.OpponentSide).Name;
			TextObject text = args.MenuContext.GameMenu.GetText();
			text.SetTextVariable("ATTACKER", name);
			text.SetTextVariable("DEFENDER", currentSettlement.Name);
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.IsPlayerWaiting = false;
			}
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000F25C4 File Offset: 0x000F07C4
		private bool game_menu_encounter_interrupted_by_raid_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000F25D0 File Offset: 0x000F07D0
		private void game_menu_settlement_hide_and_wait_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			SiegeEvent siegeEvent = currentSettlement.SiegeEvent;
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp.LeaderParty : null) != null)
			{
				GameMenu.SwitchToMenu("encounter_interrupted_siege_preparations");
				return;
			}
			if (currentSettlement.IsTown)
			{
				GameMenu.SwitchToMenu("town");
				return;
			}
			if (currentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle");
			}
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x000F262C File Offset: 0x000F082C
		private bool game_menu_settlement_hide_and_wait_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x000F2637 File Offset: 0x000F0837
		private bool wait_menu_settlement_hide_and_wait_on_condition(MenuCallbackArgs args)
		{
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.StoppableFastForward;
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return true;
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x000F2674 File Offset: 0x000F0874
		private bool game_menu_encounter_interrupted_siege_preparations_break_out_of_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int lostTroopCountForBreakingOutOfBesiegedSettlement = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent);
			int num = (mainParty.Army != null && mainParty.Army.LeaderParty == mainParty) ? mainParty.Army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars;
			if (mainParty.Army != null && mainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				args.IsEnabled = true;
				TextObject textObject = new TextObject("{=!}If you break out from the siege, you will also leave the army. This is a dishonorable act and you will lose relations with all army member lords.{newline}• Army Leader: {ARMY_LEADER_RELATION_PENALTY}{newline}• Army Members: {ARMY_MEMBER_RELATION_PENALTY}", null);
				textObject.SetTextVariable("ARMY_LEADER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyLeaderRelationPenalty);
				textObject.SetTextVariable("ARMY_MEMBER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyMemberRelationPenalty);
				args.Tooltip = textObject;
			}
			if (lostTroopCountForBreakingOutOfBesiegedSettlement > num)
			{
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				args.IsEnabled = false;
			}
			return FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, siegeEvent.BesiegerCamp.LeaderParty.MapFaction);
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x000F27B4 File Offset: 0x000F09B4
		private bool game_menu_encounter_interrupted_siege_preparations_hide_in_town_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			IFaction mapFaction = Hero.MainHero.MapFaction;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			IFaction mapFaction2 = Settlement.CurrentSettlement.MapFaction;
			return mapFaction != siegeEvent.BesiegerCamp.LeaderParty.MapFaction && (FactionManager.IsAtWarAgainstFaction(mapFaction2, mapFaction) || FactionManager.IsNeutralWithFaction(mapFaction2, mapFaction));
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x000F2811 File Offset: 0x000F0A11
		private void game_menu_encounter_interrupted_break_out_of_town_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("break_out_menu");
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000F2820 File Offset: 0x000F0A20
		private void game_menu_encounter_interrupted_siege_preparations_join_defend_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PlayerSiege.StartPlayerSiege(BattleSideEnum.Defender, false, null);
			MobileParty.MainParty.Ai.SetMoveDefendSettlement(currentSettlement);
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000F2850 File Offset: 0x000F0A50
		private bool game_menu_encounter_interrupted_siege_preparations_join_defend_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
			GameTexts.SetVariable("SETTLEMENT", Settlement.CurrentSettlement.Name);
			return Settlement.CurrentSettlement.SiegeEvent.CanPartyJoinSide(PartyBase.MainParty, BattleSideEnum.Defender);
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000F2883 File Offset: 0x000F0A83
		private void game_menu_encounter_interrupted_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000F288B File Offset: 0x000F0A8B
		public static void menu_sneak_into_town_succeeded_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town");
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000F2897 File Offset: 0x000F0A97
		public static bool menu_sneak_into_town_succeeded_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000F28A2 File Offset: 0x000F0AA2
		public static void game_menu_sneak_into_town_caught_on_init(MenuCallbackArgs args)
		{
			ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, 10f, true);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000F28B9 File Offset: 0x000F0AB9
		public static void mno_sneak_caught_surrender_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("menu_captivity_castle_taken_prisoner");
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000F28C5 File Offset: 0x000F0AC5
		public static bool mno_sneak_caught_surrender_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return true;
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000F28D0 File Offset: 0x000F0AD0
		private void game_menu_encounter_interrupted_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("join_encounter");
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000F28DC File Offset: 0x000F0ADC
		private bool game_menu_encounter_interrupted_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000F28E7 File Offset: 0x000F0AE7
		private void game_menu_town_assault_on_init(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
			this.game_menu_encounter_attack_on_consequence(args);
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x000F28FA File Offset: 0x000F0AFA
		private void game_menu_town_assault_order_attack_on_init(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
			this.game_menu_encounter_order_attack_on_consequence(args);
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000F2910 File Offset: 0x000F0B10
		private void game_menu_army_encounter_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.LeaveEncounter)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			if ((PlayerEncounter.Battle != null && PlayerEncounter.Battle.AttackerSide.LeaderParty != PartyBase.MainParty && PlayerEncounter.Battle.DefenderSide.LeaderParty != PartyBase.MainParty) || PlayerEncounter.MeetingDone)
			{
				if (PlayerEncounter.Battle == null)
				{
					PlayerEncounter.StartBattle();
				}
				if (PlayerEncounter.BattleChallenge)
				{
					GameMenu.SwitchToMenu("duel_starter_menu");
					return;
				}
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			else
			{
				if (PlayerEncounter.EncounteredMobileParty.SiegeEvent != null && Settlement.CurrentSettlement != null)
				{
					GameMenu.SwitchToMenu("join_siege_event");
					return;
				}
				if (PlayerEncounter.EncounteredMobileParty != null && PlayerEncounter.EncounteredMobileParty.Army != null)
				{
					MBTextManager.SetTextVariable("ARMY", PlayerEncounter.EncounteredMobileParty.Army.Name, false);
					MBTextManager.SetTextVariable("ARMY_ENCOUNTER_TEXT", GameTexts.FindText("str_you_have_encountered_ARMY", null), true);
				}
				return;
			}
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000F29F0 File Offset: 0x000F0BF0
		private void game_menu_encounter_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/battle/slide_in");
			if (PlayerEncounter.Battle == null)
			{
				if (MobileParty.MainParty.MapEvent != null)
				{
					PlayerEncounter.Init();
				}
				else
				{
					PlayerEncounter.StartBattle();
				}
			}
			PlayerEncounter.Update();
			this.UpdateVillageHostileActionEncounter(args);
			if (PlayerEncounter.Current == null)
			{
				Campaign.Current.SaveHandler.SignalAutoSave();
			}
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000F2A50 File Offset: 0x000F0C50
		private void UpdateVillageHostileActionEncounter(MenuCallbackArgs args)
		{
			MapEvent battle = PlayerEncounter.Battle;
			if (Game.Current.GameStateManager.ActiveState is MapState && ((battle != null) ? battle.MapEventSettlement : null) != null && battle.MapEventSettlement.IsVillage && battle.DefenderSide.LeaderParty.IsSettlement && battle.AttackerSide == battle.GetMapEventSide(battle.PlayerSide))
			{
				bool flag = battle.DefenderSide.Parties.All((MapEventParty x) => x.Party.MemberRoster.TotalHealthyCount == 0);
				bool flag2 = this.ConsiderMilitiaSurrenderPossibility();
				if (flag || flag2)
				{
					if (!flag)
					{
						for (int i = battle.DefenderSide.Parties.Count - 1; i >= 0; i--)
						{
							if (battle.DefenderSide.Parties[i].Party.IsMobile)
							{
								battle.DefenderSide.Parties[i].Party.MapEventSide = null;
							}
						}
						if (!battle.IsRaid)
						{
							battle.SetOverrideWinner(BattleSideEnum.Attacker);
						}
					}
					if (battle.IsRaid)
					{
						this.game_menu_village_raid_no_resist_on_consequence(args);
						return;
					}
					if (battle.IsForcingSupplies)
					{
						this.game_menu_village_force_supplies_no_resist_loot_on_consequence(args);
						return;
					}
					if (battle.IsForcingVolunteers)
					{
						this.game_menu_village_force_volunteers_no_resist_loot_on_consequence(args);
					}
				}
			}
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000F2B9F File Offset: 0x000F0D9F
		public static bool game_menu_captivity_taken_prisoner_cheat_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000F2BAC File Offset: 0x000F0DAC
		private bool ConsiderMilitiaSurrenderPossibility()
		{
			bool result = false;
			MapEvent battle = PlayerEncounter.Battle;
			if ((battle.IsRaid || battle.IsForcingSupplies || battle.IsForcingVolunteers) && battle.MapEventSettlement.IsVillage)
			{
				Settlement mapEventSettlement = battle.MapEventSettlement;
				float num = 0f;
				bool flag = false;
				foreach (MapEventParty mapEventParty in battle.DefenderSide.Parties)
				{
					num += mapEventParty.Party.TotalStrength;
					if (mapEventParty.Party.IsMobile && mapEventParty.Party.MobileParty.IsLordParty)
					{
						flag = true;
					}
				}
				float num2 = 0f;
				foreach (MapEventParty mapEventParty2 in battle.AttackerSide.Parties)
				{
					if (!mapEventParty2.Party.IsMobile || mapEventParty2.Party.MobileParty.Army == null)
					{
						num2 += mapEventParty2.Party.TotalStrength;
					}
					else if (mapEventParty2.Party.IsMobile && mapEventParty2.Party.MobileParty.Army != null && mapEventParty2.Party.MobileParty.Army.LeaderParty == mapEventParty2.Party.MobileParty)
					{
						foreach (MobileParty mobileParty in mapEventParty2.Party.MobileParty.Army.LeaderParty.AttachedParties)
						{
							num2 += mobileParty.Party.TotalStrength;
						}
					}
				}
				Clan ownerClan = mapEventSettlement.OwnerClan;
				bool flag2;
				if (ownerClan == null)
				{
					flag2 = (null != null);
				}
				else
				{
					Hero leader = ownerClan.Leader;
					flag2 = (((leader != null) ? leader.PartyBelongedTo : null) != null);
				}
				float num3 = flag2 ? mapEventSettlement.OwnerClan.Leader.PartyBelongedTo.Party.RandomFloatWithSeed(1U, 0.05f, 0.15f) : 0.1f;
				result = (!flag && num2 * num3 > num);
			}
			return result;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000F2E08 File Offset: 0x000F1008
		private bool game_menu_encounter_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && MapEventHelper.CanLeaveBattle(MobileParty.MainParty) && (!MobileParty.MainParty.MapEvent.IsSallyOut || MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Defender || MobileParty.MainParty.CurrentSettlement == null);
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x000F2E80 File Offset: 0x000F1080
		private bool game_menu_sally_out_go_back_to_settlement_on_condition(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.IsSallyOut && MobileParty.MainParty.MapEvent.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.CurrentSettlement != null)
			{
				bool flag = Campaign.Current.Models.EncounterModel.GetLeaderOfMapEvent(MobileParty.MainParty.MapEvent, MobileParty.MainParty.MapEvent.PlayerSide) == Hero.MainHero;
				if ((MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty || flag) && (MobileParty.MainParty.SiegeEvent == null || !MobileParty.MainParty.SiegeEvent.BesiegerCamp.IsBesiegerSideParty(MobileParty.MainParty)) && !PlayerEncounter.Current.CheckIfBattleShouldContinueAfterBattleMission())
				{
					args.optionLeaveType = GameMenuOption.LeaveType.Leave;
					GameTexts.SetVariable("SETTLEMENT", MobileParty.MainParty.LastVisitedSettlement.Name);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x000F2F8C File Offset: 0x000F118C
		private void game_menu_sally_out_go_back_to_settlement_consequence(MenuCallbackArgs args)
		{
			MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
			playerMapEvent.BeginWait();
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfMapEvent(playerMapEvent, playerMapEvent.PlayerSide) == Hero.MainHero)
			{
				PlayerEncounter.Current.FinalizeBattle();
				PlayerEncounter.Current.SetupFields(Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party, PartyBase.MainParty);
			}
			else
			{
				PlayerEncounter.LeaveBattle();
			}
			GameMenu.SwitchToMenu("menu_siege_strategies");
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000F300C File Offset: 0x000F120C
		private bool game_menu_encounter_abandon_army_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty && !MobileParty.MainParty.MapEvent.IsSallyOut && MapEventHelper.CanLeaveBattle(MobileParty.MainParty);
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000F3060 File Offset: 0x000F1260
		private void game_menu_army_talk_to_leader_on_consequence(MenuCallbackArgs args)
		{
			Campaign.Current.CurrentConversationContext = ConversationContext.PartyEncounter;
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			ConversationCharacterData playerCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			ConversationCharacterData conversationPartnerData = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(PlayerEncounter.EncounteredParty), PlayerEncounter.EncounteredParty, false, false, false, false, false, false);
			PlayerEncounter.SetMeetingDone();
			CampaignMapConversation.OpenConversation(playerCharacterData, conversationPartnerData);
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000F30BC File Offset: 0x000F12BC
		private bool game_menu_army_talk_to_leader_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (((encounteredParty != null) ? encounteredParty.LeaderHero : null) != null)
			{
				MenuHelper.SetIssueAndQuestDataForHero(args, PlayerEncounter.EncounteredParty.LeaderHero);
			}
			return true;
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000F30EA File Offset: 0x000F12EA
		public static void game_menu_captivity_castle_taken_prisoner_cont_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
			PartyBase.MainParty.AddElementToMemberRoster(CharacterObject.PlayerCharacter, -1, true);
			TakePrisonerAction.Apply(Settlement.CurrentSettlement.Party, Hero.MainHero);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000F3118 File Offset: 0x000F1318
		private bool game_menu_army_talk_to_other_members_on_condition(MenuCallbackArgs args)
		{
			foreach (MobileParty mobileParty in PlayerEncounter.EncounteredMobileParty.Army.LeaderParty.AttachedParties)
			{
				Hero leaderHero = mobileParty.LeaderHero;
				if (leaderHero != null)
				{
					MenuHelper.SetIssueAndQuestDataForHero(args, leaderHero);
				}
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return !FactionManager.IsAtWarAgainstFaction(MobileParty.MainParty.MapFaction, PlayerEncounter.EncounteredMobileParty.MapFaction) && PlayerEncounter.EncounteredMobileParty.Army.LeaderParty.AttachedParties.Count > 0;
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000F31C4 File Offset: 0x000F13C4
		private void game_menu_army_talk_to_other_members_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("game_menu_army_talk_to_other_members");
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000F31D0 File Offset: 0x000F13D0
		private void game_menu_army_talk_to_other_members_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.LeaveEncounter)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			args.MenuContext.SetRepeatObjectList(PlayerEncounter.EncounteredMobileParty.Army.LeaderParty.AttachedParties.ToList<MobileParty>());
			if (PlayerEncounter.EncounteredMobileParty.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction))
			{
				GameMenu.SwitchToMenu("encounter");
			}
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000F3234 File Offset: 0x000F1434
		private bool game_menu_army_talk_to_other_members_item_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			MobileParty mobileParty = args.MenuContext.GetCurrentRepeatableObject() as MobileParty;
			MBTextManager.SetTextVariable("CHAR_NAME", (mobileParty != null) ? mobileParty.LeaderHero.Name : null, false);
			if (mobileParty != null && mobileParty.LeaderHero != null)
			{
				MenuHelper.SetIssueAndQuestDataForHero(args, mobileParty.LeaderHero);
			}
			return true;
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x000F3290 File Offset: 0x000F1490
		private void game_menu_army_talk_to_other_members_item_on_consequence(MenuCallbackArgs args)
		{
			MobileParty mobileParty = args.MenuContext.GetSelectedObject() as MobileParty;
			Campaign.Current.CurrentConversationContext = ConversationContext.PartyEncounter;
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(mobileParty.Party), mobileParty.Party, false, false, false, false, false, false));
		}

		// Token: 0x06003649 RID: 13897 RVA: 0x000F32EF File Offset: 0x000F14EF
		private bool game_menu_army_talk_to_other_members_back_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x000F32FA File Offset: 0x000F14FA
		private void game_menu_army_talk_to_other_members_back_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("army_encounter");
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x000F3306 File Offset: 0x000F1506
		private bool game_menu_army_attack_on_condition(MenuCallbackArgs args)
		{
			this.CheckEnemyAttackableHonorably(args);
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredMobileParty.MapFaction);
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000F3330 File Offset: 0x000F1530
		private void CheckEnemyAttackableHonorably(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				return;
			}
			if (PlayerEncounter.PlayerIsDefender)
			{
				return;
			}
			IFaction mapFaction = PlayerEncounter.EncounteredParty.MapFaction;
			if (mapFaction != null && mapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = EncounterGameMenuBehavior.EnemyNotAttackableTooltip;
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000F3398 File Offset: 0x000F1598
		private void CheckFactionAttackableHonorably(MenuCallbackArgs args, IFaction faction)
		{
			if (faction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = EncounterGameMenuBehavior.EnemyNotAttackableTooltip;
			}
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x000F33C8 File Offset: 0x000F15C8
		private void CheckFortificationAttackableHonorably(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				return;
			}
			IFaction mapFaction = PlayerEncounter.EncounterSettlement.MapFaction;
			if (mapFaction != null && mapFaction.NotAttackableByPlayerUntilTime.IsFuture)
			{
				args.IsEnabled = false;
				args.Tooltip = EncounterGameMenuBehavior.EnemyNotAttackableTooltip;
			}
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x000F342A File Offset: 0x000F162A
		private bool game_menu_army_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000F3435 File Offset: 0x000F1635
		private void game_menu_army_attack_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000F3444 File Offset: 0x000F1644
		private bool game_menu_army_join_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			if (PlayerEncounter.EncounteredMobileParty.MapFaction != MobileParty.MainParty.MapFaction)
			{
				return false;
			}
			if (PlayerEncounter.EncounteredMobileParty.Army == MobileParty.MainParty.Army)
			{
				return false;
			}
			if (PlayerEncounter.EncounteredMobileParty.MapFaction != null)
			{
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (kingdom.IsAtWarWith(Clan.PlayerClan.MapFaction) && kingdom.NotAttackableByPlayerUntilTime.IsFuture)
					{
						args.IsEnabled = false;
						args.Tooltip = GameTexts.FindText("str_cant_join_army_safe_passage", null);
					}
				}
			}
			return MobileParty.MainParty.Army == null && PlayerEncounter.EncounteredMobileParty.MapFaction == MobileParty.MainParty.MapFaction;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000F3534 File Offset: 0x000F1734
		private void game_menu_army_join_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.Army = PlayerEncounter.EncounteredMobileParty.Army;
			MobileParty.MainParty.Army.AddPartyToMergedParties(MobileParty.MainParty);
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000F3564 File Offset: 0x000F1764
		private void army_encounter_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000F356C File Offset: 0x000F176C
		private void game_menu_encounter_leave_on_consequence(MenuCallbackArgs args)
		{
			Settlement besiegedSettlement = MobileParty.MainParty.BesiegedSettlement;
			if (besiegedSettlement != null && besiegedSettlement.CurrentSiegeState == Settlement.SiegeState.InTheLordsHall)
			{
				TextObject textObject = new TextObject("{=h3YuHSRb}Are you sure you want to abandon the siege?", null);
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_decision", null).ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate()
				{
					MenuHelper.EncounterLeaveConsequence();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			MenuHelper.EncounterLeaveConsequence();
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x000F3618 File Offset: 0x000F1818
		private void game_menu_encounter_abandon_on_consequence(MenuCallbackArgs args)
		{
			((PlayerEncounter.Battle != null) ? PlayerEncounter.Battle : PlayerEncounter.EncounteredBattle).BeginWait();
			MobileParty.MainParty.Ai.SetMoveModeHold();
			Hero.MainHero.PartyBelongedTo.Army = null;
			PlayerEncounter.Finish(true);
			if (MobileParty.MainParty.BesiegerCamp != null)
			{
				MobileParty.MainParty.BesiegerCamp = null;
			}
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x000F367C File Offset: 0x000F187C
		private bool game_menu_encounter_surrender_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return MobileParty.MainParty.MapEvent != null && !MapEventHelper.CanLeaveBattle(MobileParty.MainParty) && PartyBase.MainParty.Side == BattleSideEnum.Defender && MobileParty.MainParty.MapEvent.DefenderSide.TroopCount == MobileParty.MainParty.Party.NumberOfHealthyMembers;
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x000F36DD File Offset: 0x000F18DD
		private void game_menu_encounter_surrender_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.PlayerSurrender = true;
			PlayerEncounter.Update();
			if (!Hero.MainHero.CanBecomePrisoner())
			{
				GameMenu.ActivateGameMenu("menu_captivity_end_no_more_enemies");
			}
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x000F3700 File Offset: 0x000F1900
		private bool game_menu_encounter_attack_on_condition(MenuCallbackArgs args)
		{
			this.CheckEnemyAttackableHonorably(args);
			return MenuHelper.EncounterAttackCondition(args);
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x000F370F File Offset: 0x000F190F
		private bool game_menu_encounter_capture_the_enemy_on_condition(MenuCallbackArgs args)
		{
			return MenuHelper.EncounterCaptureEnemyCondition(args);
		}

		// Token: 0x0600365A RID: 13914 RVA: 0x000F3717 File Offset: 0x000F1917
		private void game_menu_encounter_attack_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterAttackConsequence(args);
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000F371F File Offset: 0x000F191F
		private void game_menu_capture_the_enemy_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterCaptureTheEnemyOnConsequence(args);
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000F3728 File Offset: 0x000F1928
		private bool game_menu_encounter_order_attack_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.OrderTroopsToAttack;
			MapEvent playerMapEvent = MapEvent.PlayerMapEvent;
			if (playerMapEvent != null)
			{
				this.CheckEnemyAttackableHonorably(args);
				int num = 0;
				foreach (MapEventParty mapEventParty in playerMapEvent.PartiesOnSide(MobileParty.MainParty.Party.Side))
				{
					num += mapEventParty.Party.MemberRoster.Sum(delegate(TroopRosterElement x)
					{
						if (!x.Character.IsHero)
						{
							return x.Number - x.WoundedNumber;
						}
						if (x.Character == CharacterObject.PlayerCharacter)
						{
							return 0;
						}
						if (!x.Character.HeroObject.IsWounded)
						{
							return 1;
						}
						return 0;
					});
				}
				if (playerMapEvent.HasTroopsOnBothSides() && playerMapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide) != null && num > 0)
				{
					if (MobileParty.MainParty.MemberRoster.Sum(delegate(TroopRosterElement x)
					{
						if (!x.Character.IsHero)
						{
							return x.Number - x.WoundedNumber;
						}
						if (x.Character == CharacterObject.PlayerCharacter)
						{
							return 0;
						}
						if (!x.Character.HeroObject.IsWounded)
						{
							return 1;
						}
						return 0;
					}) > 0)
					{
						MBTextManager.SetTextVariable("SEND_TROOPS_TEXT", "{=QfMeoKOm}Send troops.", false);
					}
					else
					{
						MBTextManager.SetTextVariable("SEND_TROOPS_TEXT", "{=jo3UHKMD}Leave it to the others.", false);
					}
					if (playerMapEvent.IsInvulnerable)
					{
						playerMapEvent.IsInvulnerable = false;
					}
					IFaction mapFaction = PlayerEncounter.EncounteredParty.MapFaction;
					if (mapFaction == null || mapFaction.NotAttackableByPlayerUntilTime.IsPast)
					{
						args.Tooltip = TooltipHelper.GetSendTroopsPowerContextTooltipForMapEvent();
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000F388C File Offset: 0x000F1A8C
		private void game_menu_encounter_order_attack_on_consequence(MenuCallbackArgs args)
		{
			MenuHelper.EncounterOrderAttackConsequence(args);
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000F3894 File Offset: 0x000F1A94
		private bool game_menu_encounter_leave_your_soldiers_behind_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			if (PartyBase.MainParty.Side == BattleSideEnum.Defender && PlayerEncounter.Battle.DefenderSide.LeaderParty == PartyBase.MainParty && !MobileParty.MainParty.MapEvent.HasWinner)
			{
				int numberOfTroopsSacrificedForTryingToGetAway = Campaign.Current.Models.TroopSacrificeModel.GetNumberOfTroopsSacrificedForTryingToGetAway(PlayerEncounter.Current.PlayerSide, PlayerEncounter.Battle);
				this._intEncounterVariable = numberOfTroopsSacrificedForTryingToGetAway;
				int num = PartyBase.MainParty.NumberOfRegularMembers;
				if (MobileParty.MainParty.Army != null)
				{
					foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
					{
						num += mobileParty.Party.NumberOfRegularMembers;
					}
				}
				if (numberOfTroopsSacrificedForTryingToGetAway < 0 || num < numberOfTroopsSacrificedForTryingToGetAway)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000F39A8 File Offset: 0x000F1BA8
		private void game_menu_leave_soldiers_behind_on_init(MenuCallbackArgs args)
		{
			Hero heroWithHighestSkill = MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Tactics);
			int content = (heroWithHighestSkill != null) ? heroWithHighestSkill.GetSkillValue(DefaultSkills.Tactics) : 0;
			MBTextManager.SetTextVariable("HIGHEST_TACTICS_SKILL", content);
			MBTextManager.SetTextVariable("HIGHEST_TACTICS_SKILLED_MEMBER", (heroWithHighestSkill != null && heroWithHighestSkill != Hero.MainHero) ? heroWithHighestSkill.Name : GameTexts.FindText("str_you", null), false);
			MBTextManager.SetTextVariable("NEEEDED_MEN_COUNT", this._intEncounterVariable);
			MBTextManager.SetTextVariable("SOLDIER_OR_SOLDIERS", (this._intEncounterVariable <= 1) ? GameTexts.FindText("str_soldier", null) : GameTexts.FindText("str_soldiers", null), false);
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000F3A47 File Offset: 0x000F1C47
		public static void game_request_entry_to_castle_approved_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("castle");
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000F3A53 File Offset: 0x000F1C53
		public static bool game_request_entry_to_castle_approved_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000F3A5E File Offset: 0x000F1C5E
		public static void game_request_entry_to_castle_rejected_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("castle_outside");
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000F3A6A File Offset: 0x000F1C6A
		public static void menu_castle_entry_denied_on_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x000F3A6C File Offset: 0x000F1C6C
		private void game_menu_encounter_leave_your_soldiers_behind_accept_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.SacrificeTroops(this._intEncounterVariable, out this._getAwayCasualties, out this._getAwayLostBaggage);
			CampaignEventDispatcher.Instance.OnPlayerDesertedBattle(this._intEncounterVariable);
			if (MobileParty.MainParty.BesiegerCamp != null)
			{
				MobileParty.MainParty.BesiegerCamp = null;
			}
			if (Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.SwitchToMenu("try_to_get_away_debrief");
				return;
			}
			GameMenu.ActivateGameMenu("try_to_get_away_debrief");
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000F3AD8 File Offset: 0x000F1CD8
		private void game_menu_get_away_debrief_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = GameTexts.FindText("str_STR1_space_STR2", null);
			if (this._getAwayCasualties != null)
			{
				TextObject textObject2;
				if (this._getAwayCasualties.Count > 0)
				{
					textObject2 = new TextObject("{=NhHzgs5e}When, after some time your forces regroup, you take a quick tally of your troops only to see you have lost: {CASUALTIES}.", null);
					textObject2.SetTextVariable("CASUALTIES", PartyBaseHelper.PrintRegularTroopCategories(this._getAwayCasualties));
				}
				else
				{
					textObject2 = new TextObject("{=bXzAluln}When, after some time your forces regroup, you take a quick tally of your troops to see that your forces are intact.", null);
				}
				textObject.SetTextVariable("STR1", textObject2);
			}
			if (this._getAwayLostBaggage != null)
			{
				TextObject textObject3 = TextObject.Empty;
				if (this._getAwayLostBaggage.Count > 0)
				{
					textObject3 = new TextObject("{=mrjz1ka4}And in your lost baggage you had: {LOST_BAGGAGE}.", null);
					textObject3.SetTextVariable("LOST_BAGGAGE", PartyBaseHelper.PrintSummarisedItemRoster(this._getAwayLostBaggage));
				}
				textObject.SetTextVariable("STR2", textObject3);
			}
			MBTextManager.SetTextVariable("CASUALTIES_AND_LOST_BAGGAGE", textObject, false);
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000F3BA3 File Offset: 0x000F1DA3
		private bool game_menu_try_to_get_away_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000F3BAE File Offset: 0x000F1DAE
		private bool game_menu_try_to_get_away_reject_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000F3BB9 File Offset: 0x000F1DB9
		private bool game_menu_try_to_get_away_accept_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x000F3BC4 File Offset: 0x000F1DC4
		private void game_menu_try_to_get_away_end(MenuCallbackArgs args)
		{
			foreach (MapEventParty mapEventParty in PlayerEncounter.Battle.PartiesOnSide(BattleSideEnum.Defender))
			{
				if (mapEventParty.Party.MobileParty != null)
				{
					if (mapEventParty.Party.MobileParty.BesiegerCamp != null)
					{
						mapEventParty.Party.MobileParty.BesiegerCamp = null;
					}
					if (mapEventParty.Party.MobileParty.CurrentSettlement != null && mapEventParty.Party == PartyBase.MainParty)
					{
						LeaveSettlementAction.ApplyForParty(mapEventParty.Party.MobileParty);
					}
				}
			}
			PlayerEncounter.Battle.DiplomaticallyFinished = true;
			PlayerEncounter.ProtectPlayerSide(1f);
			PlayerEncounter.Finish(true);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x000F3C90 File Offset: 0x000F1E90
		private bool game_menu_town_besiege_continue_siege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (encounteredParty == null)
			{
				return false;
			}
			MapEvent encounteredBattle = PlayerEncounter.EncounteredBattle;
			return encounteredBattle != null && encounteredBattle.GetLeaderParty(PartyBase.MainParty.Side) == PartyBase.MainParty && encounteredParty.IsSettlement && encounteredParty.Settlement.IsFortification && FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, encounteredParty.MapFaction) && encounteredParty.Settlement.IsUnderSiege && encounteredParty.Settlement.CurrentSiegeState == Settlement.SiegeState.OnTheWalls;
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x000F3D19 File Offset: 0x000F1F19
		private void game_menu_town_besiege_continue_siege_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Battle != null)
			{
				PlayerEncounter.Finish(true);
			}
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x000F3D30 File Offset: 0x000F1F30
		private bool game_menu_village_hostile_action_on_condition(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement == null || !Settlement.CurrentSettlement.IsVillage)
			{
				return false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Raid;
			MapEvent battle = PlayerEncounter.Battle;
			if (PartyBase.MainParty.Side == BattleSideEnum.Attacker)
			{
				return !battle.PartiesOnSide(BattleSideEnum.Defender).Any((MapEventParty party) => party.Party.NumberOfHealthyMembers > 0);
			}
			return false;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000F3D9D File Offset: 0x000F1F9D
		private void game_menu_village_raid_no_resist_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			if (PlayerEncounter.Current != null)
			{
				if (PlayerEncounter.InsideSettlement)
				{
					PlayerEncounter.LeaveSettlement();
				}
				GameMenu.ActivateGameMenu("raiding_village");
			}
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x000F3DD0 File Offset: 0x000F1FD0
		private void game_menu_village_force_supplies_no_resist_loot_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyMinorCoercionHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			GameMenu.ActivateGameMenu("force_supplies_village");
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x000F3DF0 File Offset: 0x000F1FF0
		private void game_menu_village_force_volunteers_no_resist_loot_on_consequence(MenuCallbackArgs args)
		{
			BeHostileAction.ApplyMajorCoercionHostileAction(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			GameMenu.ActivateGameMenu("force_volunteers_village");
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x000F3E10 File Offset: 0x000F2010
		private void game_menu_taken_prisoner_on_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x000F3E12 File Offset: 0x000F2012
		private bool game_menu_taken_prisoner_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000F3E1D File Offset: 0x000F201D
		private void game_menu_taken_prisoner_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x000F3E24 File Offset: 0x000F2024
		private void game_menu_encounter_meeting_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current == null || ((PlayerEncounter.Battle == null || PlayerEncounter.Battle.AttackerSide.LeaderParty == PartyBase.MainParty || PlayerEncounter.Battle.DefenderSide.LeaderParty == PartyBase.MainParty) && !PlayerEncounter.MeetingDone))
			{
				PlayerEncounter.DoMeeting();
				return;
			}
			if (PlayerEncounter.LeaveEncounter)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			if (PlayerEncounter.Battle == null)
			{
				PlayerEncounter.StartBattle();
			}
			if (PlayerEncounter.BattleChallenge)
			{
				GameMenu.SwitchToMenu("duel_starter_menu");
				return;
			}
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000F3EAF File Offset: 0x000F20AF
		private void VillageOutsideOnInit(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000F3EBC File Offset: 0x000F20BC
		private void game_menu_town_outside_on_init(MenuCallbackArgs args)
		{
			Settlement encounterSettlement = PlayerEncounter.EncounterSettlement;
			args.MenuTitle = encounterSettlement.Name;
			TextObject textObject = TextObject.Empty;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(encounterSettlement, out this._accessDetails);
			SettlementAccessModel.AccessLevel accessLevel = this._accessDetails.AccessLevel;
			int num = (int)accessLevel;
			if (num != 0)
			{
				if (num == 1)
				{
					if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
					{
						textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
						textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
						goto IL_146;
					}
				}
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.HostileFaction)
			{
				if (encounterSettlement.InRebelliousState)
				{
					textObject = GameTexts.FindText("str_gate_down_enemy_text_castle_low_loyalty", null);
					textObject.SetTextVariable("FACTION_INFORMAL_NAME", encounterSettlement.MapFaction.InformalName);
					goto IL_146;
				}
				textObject = GameTexts.FindText("str_gate_down_enemy_text_castle", null);
				goto IL_146;
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
			{
				textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
				textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
				goto IL_146;
			}
			if (encounterSettlement.InRebelliousState)
			{
				textObject = GameTexts.FindText("str_settlement_not_allowed_text_low_loyalty", null);
				textObject.SetTextVariable("FACTION_INFORMAL_NAME", encounterSettlement.MapFaction.InformalName);
			}
			else
			{
				textObject = GameTexts.FindText("str_settlement_not_allowed_text", null);
			}
			IL_146:
			textObject.SetTextVariable("SETTLEMENT_NAME", encounterSettlement.EncyclopediaLinkWithName);
			textObject.SetTextVariable("FACTION_TERM", encounterSettlement.MapFaction.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("TOWN_TEXT", textObject, false);
			if (this._accessDetails.PreliminaryActionObligation == SettlementAccessModel.PreliminaryActionObligation.Optional && this._accessDetails.PreliminaryActionType == SettlementAccessModel.PreliminaryActionType.FaceCharges)
			{
				GameMenu.SwitchToMenu("town_inside_criminal");
				return;
			}
			if (this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess && this._accessDetails.AccessMethod == SettlementAccessModel.AccessMethod.Direct)
			{
				GameMenu.SwitchToMenu("town");
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x000F4094 File Offset: 0x000F2294
		private void game_menu_fortification_high_crime_rating_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=DdeIg5hz}As you move through the streets, you hear whispers of an upcoming war between your faction and {SETTLEMENT_FACTION}. Upon hearing this, you slink away without attracting any suspicion.", null);
			textObject.SetTextVariable("SETTLEMENT_FACTION", Settlement.CurrentSettlement.MapFaction.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("FORTIFICATION_CRIME_RATING_TEXT", textObject, false);
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x000F40D4 File Offset: 0x000F22D4
		private bool game_menu_fortification_high_crime_rating_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x000F40E0 File Offset: 0x000F22E0
		private void game_menu_army_left_settlement_due_to_war_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=Nsb6SD4y}After receiving word of an upcoming war against {ENEMY_FACTION}, {ARMY_NAME} decided to leave {SETTLEMENT_NAME}.", null);
			textObject.SetTextVariable("ENEMY_FACTION", Settlement.CurrentSettlement.MapFaction.EncyclopediaLinkWithName);
			textObject.SetTextVariable("ARMY_NAME", MobileParty.MainParty.Army.Name);
			textObject.SetTextVariable("SETTLEMENT_NAME", Settlement.CurrentSettlement.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("ARMY_LEFT_SETTLEMENT_DUE_TO_WAR_TEXT", textObject, false);
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x000F4151 File Offset: 0x000F2351
		private bool game_menu_army_left_settlement_due_to_war_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x000F415C File Offset: 0x000F235C
		private void game_menu_castle_outside_on_init(MenuCallbackArgs args)
		{
			Settlement encounterSettlement = PlayerEncounter.EncounterSettlement;
			args.MenuTitle = encounterSettlement.Name;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterSettlement(encounterSettlement, out this._accessDetails);
			TextObject textObject = TextObject.Empty;
			SettlementAccessModel.AccessLevel accessLevel = this._accessDetails.AccessLevel;
			int num = (int)accessLevel;
			if (num != 0)
			{
				if (num == 1)
				{
					if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
					{
						textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
						textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
						goto IL_114;
					}
				}
				if (encounterSettlement.OwnerClan == Hero.MainHero.Clan)
				{
					textObject = GameTexts.FindText("str_castle_text_yours", null);
				}
				else
				{
					textObject = GameTexts.FindText("str_castle_text_1", null);
				}
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.HostileFaction)
			{
				textObject = GameTexts.FindText("str_gate_down_enemy_text_castle", null);
			}
			else if (this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
			{
				textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.MapFaction.Name);
				textObject = GameTexts.FindText("str_gate_down_criminal_text", null);
			}
			else
			{
				textObject = GameTexts.FindText("str_settlement_not_allowed_text", null);
			}
			IL_114:
			encounterSettlement.OwnerClan.Leader.SetPropertiesToTextObject(textObject, "LORD");
			textObject.SetTextVariable("FACTION_TERM", encounterSettlement.MapFaction.EncyclopediaLinkWithName);
			textObject.SetTextVariable("SETTLEMENT_NAME", encounterSettlement.EncyclopediaLinkWithName);
			MBTextManager.SetTextVariable("TOWN_TEXT", textObject, false);
			if (this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess && (this._accessDetails.AccessMethod == SettlementAccessModel.AccessMethod.Direct || (this._playerIsAlreadyInCastle && this._accessDetails.AccessMethod == SettlementAccessModel.AccessMethod.ByRequest)))
			{
				GameMenu.SwitchToMenu("castle");
				return;
			}
			this._playerIsAlreadyInCastle = false;
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000F430C File Offset: 0x000F250C
		private void game_menu_army_left_settlement_due_to_war_on_consequence(MenuCallbackArgs args)
		{
			MobileParty leaderParty = MobileParty.MainParty.Army.LeaderParty;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			LeaveSettlementAction.ApplyForParty(leaderParty);
			SetPartyAiAction.GetActionForPatrollingAroundSettlement(leaderParty, currentSettlement);
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x000F433A File Offset: 0x000F253A
		private void game_menu_town_outside_approach_gates_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town_guard");
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x000F4346 File Offset: 0x000F2546
		private bool game_menu_castle_outside_approach_gates_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x000F4350 File Offset: 0x000F2550
		private void game_menu_castle_outside_approach_gates_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("castle_guard");
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x000F435C File Offset: 0x000F255C
		private void game_menu_fortification_high_crime_rating_continue_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x000F4364 File Offset: 0x000F2564
		private bool outside_menu_criminal_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000F438B File Offset: 0x000F258B
		private void outside_menu_criminal_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town_discuss_criminal_surrender");
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000F4397 File Offset: 0x000F2597
		private void caught_outside_menu_criminal_on_consequence(MenuCallbackArgs args)
		{
			ChangeCrimeRatingAction.Apply(Settlement.CurrentSettlement.MapFaction, 10f, true);
			GameMenu.SwitchToMenu("town_inside_criminal");
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x000F43B8 File Offset: 0x000F25B8
		private bool caught_outside_menu_enemy_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return Hero.MainHero.MapFaction.IsAtWarWith(Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x000F43DB File Offset: 0x000F25DB
		private void caught_outside_menu_enemy_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("taken_prisoner");
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x000F43E8 File Offset: 0x000F25E8
		private bool game_menu_town_disguise_yourself_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.SneakIn;
			MBTextManager.SetTextVariable("SNEAK_CHANCE", MathF.Round(Campaign.Current.Models.DisguiseDetectionModel.CalculateDisguiseDetectionProbability(Settlement.CurrentSettlement) * 100f));
			return this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && this._accessDetails.LimitedAccessSolution == SettlementAccessModel.LimitedAccessSolution.Disguise;
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x000F444C File Offset: 0x000F264C
		private void game_menu_town_disguise_yourself_on_consequence(MenuCallbackArgs args)
		{
			bool flag = Campaign.Current.Models.DisguiseDetectionModel.CalculateDisguiseDetectionProbability(Settlement.CurrentSettlement) > MBRandom.RandomFloat;
			SkillLevelingManager.OnMainHeroDisguised(flag);
			Campaign.Current.IsMainHeroDisguised = true;
			if (flag)
			{
				GameMenu.SwitchToMenu("menu_sneak_into_town_succeeded");
				return;
			}
			GameMenu.SwitchToMenu("menu_sneak_into_town_caught");
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x000F44A4 File Offset: 0x000F26A4
		private void game_menu_castle_town_sneak_grappling_hook_on_consequence(MenuCallbackArgs args)
		{
			bool flag = false;
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>("grappling_hook");
			for (int i = 0; i < PartyBase.MainParty.ItemRoster.Count; i++)
			{
				if (PartyBase.MainParty.ItemRoster.GetElementCopyAtIndex(i).EquipmentElement.Item == @object)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Campaign.Current.GameMenuManager.SetNextMenu("town_stealth");
				PlayerEncounter.LocationEncounter.CreateAndOpenMissionController(LocationComplex.Current.GetLocationWithId("center"), null, null, null);
				return;
			}
			MBInformationManager.AddQuickInformation(new TextObject("{=!}TODO You have not any grappling hook!", null), 0, null, "");
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x000F4551 File Offset: 0x000F2751
		private bool game_menu_town_sneak_grappling_hook_on_condition(MenuCallbackArgs args)
		{
			return Campaign.Current.IsNight;
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x000F4560 File Offset: 0x000F2760
		private bool game_menu_town_town_besiege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.BesiegeTown;
			this.CheckFortificationAttackableHonorably(args);
			return FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, Settlement.CurrentSettlement.MapFaction) && PartyBase.MainParty.NumberOfHealthyMembers > 0 && !Settlement.CurrentSettlement.IsUnderSiege;
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x000F45B3 File Offset: 0x000F27B3
		private void leave_siege_after_attack_on_consequence(MenuCallbackArgs args)
		{
			MobileParty.MainParty.BesiegerCamp = null;
			GameMenu.ExitToLast();
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x000F45C5 File Offset: 0x000F27C5
		private bool leave_siege_after_attack_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x000F45F3 File Offset: 0x000F27F3
		private bool leave_army_after_attack_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x000F4624 File Offset: 0x000F2824
		private void leave_army_after_attack_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
			}
			else
			{
				GameMenu.ExitToLast();
			}
			if (Settlement.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
				PartyBase.MainParty.SetVisualAsDirty();
			}
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x000F4660 File Offset: 0x000F2860
		private void game_menu_town_town_besiege_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
			}
			Campaign.Current.SiegeEventManager.StartSiegeEvent(currentSettlement, MobileParty.MainParty);
			PlayerSiege.StartPlayerSiege(BattleSideEnum.Attacker, false, null);
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x000F46A3 File Offset: 0x000F28A3
		private void continue_siege_after_attack_on_consequence(MenuCallbackArgs args)
		{
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000F46AA File Offset: 0x000F28AA
		private bool continue_siege_after_attack_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x000F46B5 File Offset: 0x000F28B5
		private bool game_menu_town_outside_cheat_enter_on_condition(MenuCallbackArgs args)
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x000F46C1 File Offset: 0x000F28C1
		private void game_menu_town_outside_enter_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("town");
			PlayerEncounter.LocationEncounter.IsInsideOfASettlement = true;
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x000F46D8 File Offset: 0x000F28D8
		private bool game_menu_castle_outside_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x000F46E3 File Offset: 0x000F28E3
		private void game_menu_castle_outside_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x000F46EC File Offset: 0x000F28EC
		private bool game_menu_town_guard_request_shelter_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			if (this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.NoAccess && this._accessDetails.AccessLimitationReason == SettlementAccessModel.AccessLimitationReason.CrimeRating)
			{
				args.Tooltip = new TextObject("{=03DZpTYi}You are a wanted criminal.", null);
				args.IsEnabled = false;
			}
			List<Location> locations = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall" || x == "prison").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, locations);
			return true;
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x000F476F File Offset: 0x000F296F
		private void game_menu_request_entry_to_castle_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (this._accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess)
			{
				this._playerIsAlreadyInCastle = true;
				GameMenu.SwitchToMenu("menu_castle_entry_granted");
				return;
			}
			GameMenu.SwitchToMenu("menu_castle_entry_denied");
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x000F47A4 File Offset: 0x000F29A4
		private bool game_menu_request_meeting_someone_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			List<Location> locations = Settlement.CurrentSettlement.LocationComplex.FindAll((string x) => x == "lordshall").ToList<Location>();
			MenuHelper.SetIssueAndQuestDataForLocations(args, locations);
			bool flag;
			TextObject tooltip;
			bool result = Campaign.Current.Models.SettlementAccessModel.IsRequestMeetingOptionAvailable(Settlement.CurrentSettlement, out flag, out tooltip);
			args.Tooltip = tooltip;
			args.IsEnabled = !flag;
			return result;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x000F4822 File Offset: 0x000F2A22
		private void game_menu_request_meeting_someone_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("request_meeting");
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x000F482E File Offset: 0x000F2A2E
		private void game_menu_town_guard_back_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x000F4858 File Offset: 0x000F2A58
		private void game_menu_town_menu_request_meeting_on_init(MenuCallbackArgs args)
		{
			List<Hero> heroesToMeetInTown = TownHelpers.GetHeroesToMeetInTown(Settlement.CurrentSettlement);
			args.MenuContext.SetRepeatObjectList(heroesToMeetInTown);
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000F487C File Offset: 0x000F2A7C
		private bool game_menu_request_meeting_with_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			Hero hero = args.MenuContext.GetCurrentRepeatableObject() as Hero;
			if (hero != null)
			{
				StringHelpers.SetCharacterProperties("HERO_TO_MEET", hero.CharacterObject, null, false);
				MenuHelper.SetIssueAndQuestDataForHero(args, hero);
				return true;
			}
			return false;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000F48C4 File Offset: 0x000F2AC4
		private void game_menu_town_menu_request_meeting_with_besiegers_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement.SiegeEvent == null)
			{
				if (MobileParty.MainParty.BesiegerCamp == null)
				{
					PlayerSiege.ClosePlayerSiege();
				}
				if (currentSettlement.IsTown)
				{
					GameMenu.SwitchToMenu("town");
					return;
				}
				if (currentSettlement.IsCastle)
				{
					GameMenu.SwitchToMenu("castle");
					return;
				}
				Debug.FailedAssert("non-fortification under siege", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\EncounterGameMenuBehavior.cs", "game_menu_town_menu_request_meeting_with_besiegers_on_init", 2444);
			}
			List<MobileParty> list = new List<MobileParty>();
			if (currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army != null)
			{
				list.Add(currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army.LeaderParty);
			}
			else
			{
				list.Add(currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty);
			}
			args.MenuContext.SetRepeatObjectList(list.AsReadOnly());
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x000F4998 File Offset: 0x000F2B98
		private bool game_menu_request_meeting_with_besiegers_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement.SiegeEvent != null)
			{
				MobileParty mobileParty = (currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army != null) ? currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Army.LeaderParty : currentSettlement.SiegeEvent.BesiegerCamp.LeaderParty;
				StringHelpers.SetCharacterProperties("PARTY_LEADER", mobileParty.LeaderHero.CharacterObject, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000F4A20 File Offset: 0x000F2C20
		private string GetMeetingScene(out string sceneLevel)
		{
			string sceneID = GameSceneDataManager.Instance.MeetingScenes.GetRandomElementWithPredicate((MeetingSceneData x) => x.Culture == Settlement.CurrentSettlement.Culture).SceneID;
			if (string.IsNullOrEmpty(sceneID))
			{
				sceneID = GameSceneDataManager.Instance.MeetingScenes.GetRandomElement<MeetingSceneData>().SceneID;
			}
			sceneLevel = "";
			if (Settlement.CurrentSettlement.IsFortification)
			{
				sceneLevel = Campaign.Current.Models.LocationModel.GetUpgradeLevelTag(Settlement.CurrentSettlement.Town.GetWallLevel());
			}
			return sceneID;
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x000F4AC0 File Offset: 0x000F2CC0
		private void game_menu_request_meeting_with_besiegers_on_consequence(MenuCallbackArgs args)
		{
			string sceneLevels;
			string meetingScene = this.GetMeetingScene(out sceneLevels);
			MobileParty mobileParty = (MobileParty)args.MenuContext.GetSelectedObject();
			ConversationCharacterData playerCharacterData = new ConversationCharacterData(Hero.MainHero.CharacterObject, PartyBase.MainParty, true, false, false, false, false, false);
			ConversationCharacterData conversationPartnerData = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(mobileParty.Party), mobileParty.Party, false, false, false, false, false, false);
			CampaignMission.OpenConversationMission(playerCharacterData, conversationPartnerData, meetingScene, sceneLevels);
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x000F4B28 File Offset: 0x000F2D28
		private void game_menu_request_meeting_with_on_consequence(MenuCallbackArgs args)
		{
			string sceneLevels;
			string meetingScene = this.GetMeetingScene(out sceneLevels);
			Hero hero = (Hero)args.MenuContext.GetSelectedObject();
			ConversationCharacterData playerCharacterData = new ConversationCharacterData(Hero.MainHero.CharacterObject, PartyBase.MainParty, false, false, false, false, false, false);
			CharacterObject characterObject = hero.CharacterObject;
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			ConversationCharacterData conversationPartnerData = new ConversationCharacterData(characterObject, (partyBelongedTo != null) ? partyBelongedTo.Party : null, true, false, false, false, false, false);
			CampaignMission.OpenConversationMission(playerCharacterData, conversationPartnerData, meetingScene, sceneLevels);
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000F4B97 File Offset: 0x000F2D97
		private bool game_meeting_town_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return Settlement.CurrentSettlement.IsTown;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x000F4BAB File Offset: 0x000F2DAB
		private bool game_meeting_castle_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return Settlement.CurrentSettlement.IsCastle;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x000F4BC0 File Offset: 0x000F2DC0
		private void game_menu_request_meeting_town_leave_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSiegeEvent.BesiegedSettlement != Settlement.CurrentSettlement)
			{
				GameMenu.SwitchToMenu("town_guard");
				return;
			}
			GameMenu.ExitToLast();
			PlayerEncounter.LeaveEncounter = false;
			if (Hero.MainHero.CurrentSettlement != null && PlayerSiege.PlayerSiegeEvent == null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegedSettlement.SiegeEvent != null)
			{
				PlayerSiege.StartSiegePreparation();
				return;
			}
			PlayerEncounter.Finish(true);
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x000F4C30 File Offset: 0x000F2E30
		private void game_menu_request_meeting_castle_leave_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSiegeEvent.BesiegedSettlement != Settlement.CurrentSettlement)
			{
				GameMenu.SwitchToMenu("castle_guard");
				return;
			}
			GameMenu.ExitToLast();
			PlayerEncounter.LeaveEncounter = false;
			if (Hero.MainHero.CurrentSettlement != null && PlayerSiege.PlayerSiegeEvent == null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegedSettlement.SiegeEvent != null)
			{
				PlayerSiege.StartSiegePreparation();
				return;
			}
			PlayerEncounter.Finish(true);
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x000F4CA0 File Offset: 0x000F2EA0
		private void game_menu_village_loot_complete_on_init(MenuCallbackArgs args)
		{
			PlayerEncounter.Update();
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x000F4CA7 File Offset: 0x000F2EA7
		private void game_menu_village_loot_complete_continue_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x000F4CAF File Offset: 0x000F2EAF
		private bool game_menu_village_loot_complete_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000F4CBA File Offset: 0x000F2EBA
		private void game_menu_raid_interrupted_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000F4CC6 File Offset: 0x000F2EC6
		private bool game_menu_raid_interrupted_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000F4CD1 File Offset: 0x000F2ED1
		private void break_out_menu_accept_on_consequence(MenuCallbackArgs args)
		{
			BreakInOutBesiegedSettlementAction.ApplyBreakOut(out this._breakInOutCasualties, out this._breakInOutArmyCasualties);
			GameMenu.SwitchToMenu("break_out_debrief_menu");
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000F4CF0 File Offset: 0x000F2EF0
		private bool break_out_menu_accept_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int lostTroopCountForBreakingOutOfBesiegedSettlement = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent);
			int num = (mainParty.Army != null && mainParty.Army.LeaderParty == mainParty) ? mainParty.Army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars;
			if (lostTroopCountForBreakingOutOfBesiegedSettlement > num)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
			}
			return true;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000F4D77 File Offset: 0x000F2F77
		private void break_in_menu_accept_on_consequence(MenuCallbackArgs args)
		{
			BreakInOutBesiegedSettlementAction.ApplyBreakIn(out this._breakInOutCasualties, out this._breakInOutArmyCasualties);
			GameMenu.SwitchToMenu("break_in_debrief_menu");
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000F4D94 File Offset: 0x000F2F94
		private bool break_in_menu_accept_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int num = (siegeEvent != null) ? Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(mainParty, siegeEvent) : 0;
			Army army = mainParty.Army;
			int num2 = (army != null) ? army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars;
			if (num > num2)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
			}
			return true;
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000F4E0F File Offset: 0x000F300F
		private void break_out_menu_reject_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				GameMenu.SwitchToMenu("menu_siege_strategies");
				return;
			}
			GameMenu.SwitchToMenu("encounter_interrupted_siege_preparations");
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000F4E2D File Offset: 0x000F302D
		private bool break_out_menu_reject_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x000F4E37 File Offset: 0x000F3037
		private void break_in_menu_reject_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("join_siege_event");
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000F4E43 File Offset: 0x000F3043
		private bool break_in_menu_reject_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x000F4E50 File Offset: 0x000F3050
		private void break_in_out_menu_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement.Party.SiegeEvent == null)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int lostTroopCountForBreakingInBesiegedSettlement = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingInBesiegedSettlement(mainParty, siegeEvent);
			TextObject text = args.MenuContext.GameMenu.GetText();
			text.SetTextVariable("POSSIBLE_CASUALTIES", lostTroopCountForBreakingInBesiegedSettlement);
			text.SetTextVariable("PLURAL", (lostTroopCountForBreakingInBesiegedSettlement > 1) ? 1 : 0);
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x000F4ED4 File Offset: 0x000F30D4
		private void break_in_out_debrief_menu_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement.Party.SiegeEvent == null)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			TextObject text = args.MenuContext.GameMenu.GetText();
			text.SetTextVariable("CASUALTIES", PartyBaseHelper.PrintRegularTroopCategories(this._breakInOutCasualties));
			if (this._breakInOutArmyCasualties > 0)
			{
				TextObject textObject = new TextObject("{=hxnCr8bm} Other parties of your army lost {NUMBER} {?PLURAL}troops{?}troop{\\?}.", null);
				textObject.SetTextVariable("NUMBER", this._breakInOutArmyCasualties);
				textObject.SetTextVariable("PLURAL", (this._breakInOutArmyCasualties > 1) ? 1 : 0);
				text.SetTextVariable("OTHER_CASUALTIES", textObject);
				return;
			}
			text.SetTextVariable("OTHER_CASUALTIES", TextObject.Empty);
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000F4F87 File Offset: 0x000F3187
		private void break_out_debrief_continue_on_consequence(MenuCallbackArgs args)
		{
			Settlement besiegedSettlement = PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
			PlayerEncounter.Finish(true);
			besiegedSettlement.Party.SetVisualAsDirty();
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				PlayerSiege.ClosePlayerSiege();
			}
			PlayerEncounter.ProtectPlayerSide(1f);
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000F4FB9 File Offset: 0x000F31B9
		private bool break_out_debrief_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000F4FC4 File Offset: 0x000F31C4
		private bool break_in_debrief_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000F4FD0 File Offset: 0x000F31D0
		private void break_in_debrief_continue_on_consequence(MenuCallbackArgs args)
		{
			if (Hero.MainHero.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			if (PlayerSiege.PlayerSiegeEvent == null)
			{
				PlayerSiege.StartPlayerSiege(BattleSideEnum.Defender, false, null);
			}
			if (Hero.MainHero.CurrentSettlement.Party.MapEvent != null)
			{
				GameMenu.SwitchToMenu("join_encounter");
				return;
			}
			PlayerSiege.StartSiegePreparation();
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000F5024 File Offset: 0x000F3224
		[GameMenuInitializationHandler("army_encounter")]
		[GameMenuInitializationHandler("game_menu_army_talk_to_other_members")]
		private static void army_encounter_background_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.EncounteredMobileParty != null && PlayerEncounter.EncounteredMobileParty.Army != null)
			{
				args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounteredMobileParty.Army.Kingdom.Culture.EncounterBackgroundMesh);
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_fallback");
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x000F507C File Offset: 0x000F327C
		[GameMenuInitializationHandler("castle_outside")]
		[GameMenuInitializationHandler("town_outside")]
		[GameMenuInitializationHandler("fortification_crime_rating")]
		[GameMenuInitializationHandler("village_outside")]
		[GameMenuInitializationHandler("menu_sneak_into_town_succeeded")]
		private static void encounter_menu_ui_castle_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x000F50A5 File Offset: 0x000F32A5
		[GameMenuInitializationHandler("menu_castle_taken")]
		[GameMenuInitializationHandler("menu_settlement_taken")]
		private static void encounter_menu_settlement_taken_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x000F50B7 File Offset: 0x000F32B7
		[GameMenuInitializationHandler("encounter_meeting")]
		private static void game_menu_encounter_meeting_background_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounteredParty.MapFaction.Culture.EncounterBackgroundMesh);
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x000F50D8 File Offset: 0x000F32D8
		[GameMenuInitializationHandler("menu_castle_entry_denied")]
		private static void game_menu_castle_guard_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("encounter_guards");
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x000F50EA File Offset: 0x000F32EA
		[GameMenuInitializationHandler("break_in_menu")]
		[GameMenuInitializationHandler("break_in_debrief_menu")]
		[GameMenuInitializationHandler("break_out_menu")]
		[GameMenuInitializationHandler("break_out_debrief_menu")]
		[GameMenuInitializationHandler("continue_siege_after_attack")]
		[GameMenuInitializationHandler("siege_attacker_defeated")]
		[GameMenuInitializationHandler("siege_attacker_left")]
		private static void game_menu_siege_background_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_besieging");
		}

		// Token: 0x0400115C RID: 4444
		private static readonly TextObject EnemyNotAttackableTooltip = GameTexts.FindText("str_enemy_not_attackable_tooltip", null);

		// Token: 0x0400115D RID: 4445
		private int _intEncounterVariable;

		// Token: 0x0400115E RID: 4446
		private TroopRoster _breakInOutCasualties;

		// Token: 0x0400115F RID: 4447
		private int _breakInOutArmyCasualties;

		// Token: 0x04001160 RID: 4448
		private SettlementAccessModel.AccessDetails _accessDetails;

		// Token: 0x04001161 RID: 4449
		private TroopRoster _getAwayCasualties;

		// Token: 0x04001162 RID: 4450
		private ItemRoster _getAwayLostBaggage;

		// Token: 0x04001163 RID: 4451
		private bool _playerIsAlreadyInCastle;

		// Token: 0x04001164 RID: 4452
		private const float SmugglingCrimeRate = 10f;
	}
}
