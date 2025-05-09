﻿using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus.GameMenuInitializationHandlers
{
	// Token: 0x020000E6 RID: 230
	public class DefaultEncounter
	{
		// Token: 0x06001440 RID: 5184 RVA: 0x00059FBD File Offset: 0x000581BD
		[GameMenuInitializationHandler("taken_prisoner")]
		[GameMenuInitializationHandler("menu_captivity_end_no_more_enemies")]
		[GameMenuInitializationHandler("menu_captivity_end_by_ally_party_saved")]
		[GameMenuInitializationHandler("menu_captivity_end_by_party_removed")]
		[GameMenuInitializationHandler("menu_captivity_end_wilderness_escape")]
		[GameMenuInitializationHandler("menu_escape_captivity_during_battle")]
		[GameMenuInitializationHandler("menu_released_after_battle")]
		[GameMenuInitializationHandler("menu_captivity_end_propose_ransom_wilderness")]
		public static void game_menu_taken_prisoner_ui_on_init(MenuCallbackArgs args)
		{
			if (Hero.MainHero.IsFemale)
			{
				args.MenuContext.SetBackgroundMeshName("wait_captive_female");
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_captive_male");
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00059FEC File Offset: 0x000581EC
		[GameMenuInitializationHandler("defeated_and_taken_prisoner")]
		public static void game_menu_defeat_and_taken_prisoner_ui_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("encounter_lose");
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00059FFE File Offset: 0x000581FE
		[GameMenuInitializationHandler("menu_captivity_transfer_to_town")]
		[GameMenuInitializationHandler("menu_captivity_end_exchanged_with_prisoner")]
		[GameMenuInitializationHandler("menu_captivity_end_propose_ransom_in_prison")]
		[GameMenuInitializationHandler("menu_captivity_castle_remain")]
		[GameMenuInitializationHandler("menu_captivity_end_propose_ransom_out")]
		[GameMenuInitializationHandler("menu_captivity_end_prison_escape")]
		public static void game_menu_taken_prisoner_town_ui_on_init(MenuCallbackArgs args)
		{
			if (Hero.MainHero.IsFemale)
			{
				args.MenuContext.SetBackgroundMeshName("wait_prisoner_female");
				return;
			}
			args.MenuContext.SetBackgroundMeshName("wait_prisoner_male");
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x0005A02D File Offset: 0x0005822D
		[GameMenuInitializationHandler("e3_action_menu")]
		private static void E3ActionMenuOnInit(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("gui_bg_lord_khuzait");
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x0005A040 File Offset: 0x00058240
		[GameMenuInitializationHandler("join_encounter")]
		private static void game_menu_join_encounter_on_init(MenuCallbackArgs args)
		{
			MobileParty mobileParty = PlayerEncounter.EncounteredParty.MobileParty;
			if (mobileParty != null && mobileParty.IsCaravan)
			{
				args.MenuContext.SetBackgroundMeshName("encounter_caravan");
				return;
			}
			args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounteredParty.MapFaction.Culture.EncounterBackgroundMesh);
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x0005A098 File Offset: 0x00058298
		[GameMenuInitializationHandler("encounter")]
		[GameMenuInitializationHandler("try_to_get_away")]
		[GameMenuInitializationHandler("try_to_get_away_debrief")]
		private static void game_menu_encounter_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			bool flag = false;
			if (PlayerEncounter.Battle != null && PlayerEncounter.Current.FirstInit)
			{
				PlayerEncounter.Current.FirstInit = false;
			}
			if (currentSettlement != null && currentSettlement.IsVillage && PlayerEncounter.Battle != null)
			{
				args.MenuContext.SetBackgroundMeshName("wait_ambush");
			}
			else if (PlayerEncounter.EncounteredParty != null && PlayerEncounter.EncounteredParty.IsMobile)
			{
				if (PlayerEncounter.EncounteredParty.MobileParty.IsVillager)
				{
					args.MenuContext.SetBackgroundMeshName("encounter_peasant");
				}
				else if (PlayerEncounter.EncounteredParty.MobileParty.IsCaravan)
				{
					args.MenuContext.SetBackgroundMeshName("encounter_caravan");
				}
				else if (PlayerEncounter.EncounteredParty.MapFaction == null)
				{
					CultureObject @object = Game.Current.ObjectManager.GetObject<CultureObject>("neutral_culture");
					args.MenuContext.SetBackgroundMeshName(@object.EncounterBackgroundMesh);
				}
				else
				{
					args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounteredParty.MapFaction.Culture.EncounterBackgroundMesh);
				}
			}
			if (PartyBase.MainParty.Side == BattleSideEnum.Defender && PartyBase.MainParty.NumberOfHealthyMembers == 0)
			{
				int num = 0;
				foreach (MapEventParty mapEventParty in PartyBase.MainParty.MapEvent.PartiesOnSide(PartyBase.MainParty.Side))
				{
					if (mapEventParty.Party != PartyBase.MainParty)
					{
						num += mapEventParty.Party.NumberOfHealthyMembers;
					}
				}
				if (num > 0)
				{
					MBTextManager.SetTextVariable("ENCOUNTER_TEXT", GameTexts.FindText("str_you_have_encountered_no_health_men_but_allies_has", null), true);
				}
				else
				{
					MBTextManager.SetTextVariable("ENCOUNTER_TEXT", (PartyBase.MainParty.MemberRoster.Count == 1) ? GameTexts.FindText("str_you_have_encountered_no_health_alone", null) : GameTexts.FindText("str_you_have_encountered_no_health_men", null), true);
				}
			}
			else if (currentSettlement != null)
			{
				if (currentSettlement.IsHideout)
				{
					MBTextManager.SetTextVariable("ENCOUNTER_TEXT", GameTexts.FindText("str_there_are_bandits_inside", null), true);
				}
				else if (currentSettlement.IsUnderRebellionAttack())
				{
					MBTextManager.SetTextVariable("ENCOUNTER_TEXT", GameTexts.FindText("str_there_are_rebels_inside", null), true);
				}
				else if (currentSettlement.IsVillage && PlayerEncounter.Battle != null)
				{
					int num2 = (from party in PlayerEncounter.Battle.InvolvedParties
					where party.Side != PartyBase.MainParty.Side
					select party).Sum((PartyBase party) => party.NumberOfHealthyMembers);
					MBTextManager.SetTextVariable("SETTLEMENT", currentSettlement.Name, false);
					TextObject textObject;
					if (PlayerEncounter.Battle.IsRaid && num2 == 0)
					{
						if (!MobileParty.MainParty.MapFaction.IsAtWarWith(currentSettlement.MapFaction))
						{
							textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_raid_no_resisting_on_peace", null);
						}
						else
						{
							textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_raid_no_resisting_on_war", null);
						}
					}
					else if (PlayerEncounter.Battle.IsForcingSupplies)
					{
						if (!MobileParty.MainParty.MapFaction.IsAtWarWith(currentSettlement.MapFaction))
						{
							textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_force_supplies_with_resisting_on_peace", null);
						}
						else
						{
							textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_force_supplies_with_resisting_on_war", null);
						}
					}
					else if (PlayerEncounter.Battle.IsForcingVolunteers)
					{
						if (!MobileParty.MainParty.MapFaction.IsAtWarWith(currentSettlement.MapFaction))
						{
							textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_force_volunteers_with_resisting_on_peace", null);
						}
						else
						{
							textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_force_volunteers_with_resisting_on_war", null);
						}
					}
					else
					{
						if (MobileParty.MainParty.MapFaction == currentSettlement.MapFaction)
						{
							textObject = new TextObject("{=HMt8Xo5p}{PARTY} is raiding {SETTLEMENT}. You decide to...", null);
						}
						else if (MobileParty.MainParty.MapFaction != PlayerEncounter.Battle.AttackerSide.MapFaction)
						{
							if (MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerEncounter.Battle.AttackerSide.MapFaction))
							{
								textObject = GameTexts.FindText("str_you_have_encountered_enemy_party_while_raiding", null);
							}
							else if (!MobileParty.MainParty.MapFaction.IsAtWarWith(currentSettlement.MapFaction))
							{
								textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_raid_with_resisting_on_peace", null);
							}
							else
							{
								textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_raid_with_resisting_on_war", null);
							}
						}
						else
						{
							textObject = GameTexts.FindText("str_you_have_encountered_settlement_to_raid_with_resisting_on_war", null);
						}
						textObject.SetTextVariable("PARTY", PlayerEncounter.Battle.AttackerSide.LeaderParty.Name);
					}
					textObject.SetTextVariable("SETTLEMENT", currentSettlement.Name);
					textObject.SetTextVariable("KINGDOM", currentSettlement.MapFaction.IsKingdomFaction ? ((Kingdom)currentSettlement.MapFaction).EncyclopediaTitle : currentSettlement.MapFaction.Name);
					MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject, true);
				}
				else if (currentSettlement.IsFortification)
				{
					if (PlayerEncounter.Battle != null)
					{
						if (PlayerEncounter.Battle.IsSiegeAssault)
						{
							if (PlayerEncounter.EncounterSettlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
							{
								if (PlayerEncounter.CampaignBattleResult != null && PlayerEncounter.CampaignBattleResult.EnemyPulledBack && currentSettlement.CurrentSiegeState == Settlement.SiegeState.InTheLordsHall)
								{
									TextObject text = GameTexts.FindText("str_you_have_encountered_settlement_to_siege_defenders_pulled_back", null);
									MBTextManager.SetTextVariable("ENCOUNTER_TEXT", text, true);
								}
								else if (currentSettlement.CurrentSiegeState == Settlement.SiegeState.InTheLordsHall)
								{
									TextObject text2 = GameTexts.FindText("str_you_have_encountered_settlement_to_siege_defenders_pulled_back_2", null);
									MBTextManager.SetTextVariable("ENCOUNTER_TEXT", text2, true);
								}
								else
								{
									TextObject textObject2 = GameTexts.FindText("str_you_have_encountered_settlement_to_siege", null);
									textObject2.SetTextVariable("SETTLEMENT", currentSettlement.Name);
									MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject2, true);
								}
							}
							else
							{
								TextObject textObject3 = GameTexts.FindText("str_you_have_encountered_settlement_to_help_defenders_inside_settlement", null);
								textObject3.SetTextVariable("SETTLEMENT", currentSettlement.Name);
								MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject3, true);
							}
						}
						else if (PlayerEncounter.Battle.IsSiegeOutside)
						{
							TextObject textObject4 = GameTexts.FindText("str_you_have_encountered_PARTY", null);
							textObject4.SetTextVariable("PARTY", PlayerEncounter.Battle.GetLeaderParty(PartyBase.MainParty.OpponentSide).Name);
							MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject4, true);
						}
						else if (PlayerEncounter.Battle.IsSallyOut && PlayerEncounter.Battle.PlayerSide == BattleSideEnum.Attacker)
						{
							TextObject textObject5 = GameTexts.FindText("str_you_have_encountered_PARTY_sally_out", null);
							textObject5.SetTextVariable("PARTY", PlayerEncounter.Battle.GetLeaderParty(PartyBase.MainParty.OpponentSide).Name);
							MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject5, true);
						}
						else
						{
							TextObject textObject6 = GameTexts.FindText("str_you_have_encountered_PARTY", null);
							textObject6.SetTextVariable("PARTY", PlayerEncounter.Battle.GetLeaderParty(PartyBase.MainParty.OpponentSide).Name);
							MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject6, true);
						}
					}
					else
					{
						Debug.FailedAssert("settlement encounter, but there is no MapEvent, menu text will be wrong", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameMenus\\GameMenuInitializationHandlers\\DefaultEncounter.cs", "game_menu_encounter_on_init", 291);
						TextObject textObject7 = GameTexts.FindText("str_you_have_encountered_settlement_to_siege", null);
						textObject7.SetTextVariable("SETTLEMENT", currentSettlement.Name);
						MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject7, true);
					}
				}
				else
				{
					MBTextManager.SetTextVariable("ENCOUNTER_TEXT", GameTexts.FindText("str_you_are_trapped_by_enemies", null), true);
				}
			}
			else if (MobileParty.MainParty.MapEvent != null && PlayerEncounter.CheckIfLeadingAvaliable() && PlayerEncounter.GetLeadingHero() != Hero.MainHero)
			{
				Hero leadingHero = PlayerEncounter.GetLeadingHero();
				TextObject textObject8 = GameTexts.FindText("str_army_leader_encounter", null);
				textObject8.SetTextVariable("PARTY", leadingHero.PartyBelongedTo.Name);
				textObject8.SetTextVariable("ARMY_COMMANDER_GENDER", leadingHero.IsFemale ? 1 : 0);
				MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject8, true);
			}
			else
			{
				TextObject textObject9;
				if (Settlement.CurrentSettlement != null && PlayerEncounter.EncounteredMobileParty.BesiegedSettlement == Settlement.CurrentSettlement)
				{
					textObject9 = new TextObject("{=dGoEWaeX}The enemy has begun their assault!", null);
					flag = true;
				}
				else if (PlayerEncounter.EncounteredMobileParty.Army != null)
				{
					if (PlayerEncounter.EncounteredMobileParty.Army.LeaderParty == PlayerEncounter.EncounteredMobileParty)
					{
						textObject9 = GameTexts.FindText("str_you_have_encountered_ARMY", null);
						textObject9.SetTextVariable("ARMY", PlayerEncounter.EncounteredMobileParty.Army.Name);
					}
					else
					{
						textObject9 = GameTexts.FindText("str_you_have_encountered_PARTY", null);
						textObject9.SetTextVariable("PARTY", MapEvent.PlayerMapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide).Name);
					}
				}
				else if (PlayerEncounter.EncounteredMobileParty.MapFaction != MobileParty.MainParty.MapFaction && PlayerEncounter.EncounteredMobileParty.MapFaction != null && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredMobileParty.MapFaction) && !Campaign.Current.Models.EncounterModel.IsEncounterExemptFromHostileActions(PartyBase.MainParty, PlayerEncounter.EncounteredParty))
				{
					textObject9 = GameTexts.FindText("str_you_have_encountered_PARTY_on_peace", null);
					IFaction mapFaction = PlayerEncounter.EncounteredMobileParty.MapFaction;
					textObject9.SetTextVariable("KINGDOM", mapFaction.IsKingdomFaction ? ((Kingdom)mapFaction).EncyclopediaTitle : mapFaction.Name);
					textObject9.SetTextVariable("PARTY", PlayerEncounter.EncounteredMobileParty.Name);
				}
				else if (PlayerEncounter.Battle != null && PlayerEncounter.Battle.IsSallyOut && MobileParty.MainParty.BesiegedSettlement != null)
				{
					if (PlayerEncounter.EncounteredMobileParty.IsGarrison)
					{
						textObject9 = new TextObject("{=xYeMbApi}{PARTY} has sallied out to attack you!", null);
						textObject9.SetTextVariable("PARTY", PlayerEncounter.Battle.GetLeaderParty(PartyBase.MainParty.OpponentSide).Name);
					}
					else
					{
						textObject9 = new TextObject("{=zmLD6wIj}{RELIEF_PARTY} has come to support {SETTLEMENT}. {FURTHER_EXPLANATION}.", null);
						textObject9.SetTextVariable("RELIEF_PARTY", PlayerEncounter.Battle.GetLeaderParty(PartyBase.MainParty.OpponentSide).Name);
						textObject9.SetTextVariable("SETTLEMENT", MobileParty.MainParty.SiegeEvent.BesiegedSettlement.Name);
						if (MobileParty.MainParty.SiegeEvent.BesiegedSettlement.IsCastle)
						{
							textObject9.SetTextVariable("FURTHER_EXPLANATION", "{=urOywsiw}Castle garrison decided to sally out");
						}
						else
						{
							textObject9.SetTextVariable("FURTHER_EXPLANATION", "{=xdtwRyfB}Town garrison decided to sally out");
						}
					}
					MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject9, true);
				}
				else
				{
					textObject9 = GameTexts.FindText("str_you_have_encountered_PARTY", null);
					textObject9.SetTextVariable("PARTY", PlayerEncounter.EncounteredMobileParty.Name);
				}
				MBTextManager.SetTextVariable("ENCOUNTER_TEXT", textObject9, true);
			}
			if (Settlement.CurrentSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
			}
			MBTextManager.SetTextVariable("ATTACK_TEXT", flag ? new TextObject("{=Ky03jg94}Fight", null) : new TextObject("{=zxMOqlhs}Attack", null), false);
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x0005AB34 File Offset: 0x00058D34
		[GameMenuInitializationHandler("join_siege_event")]
		[GameMenuInitializationHandler("join_sally_out")]
		private static void game_menu_join_siege_event_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounteredParty.MapFaction.Culture.EncounterBackgroundMesh);
			MobileParty leaderParty = Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty;
			if (((leaderParty == MobileParty.MainParty) ? 1 : 0) == 1)
			{
				if (leaderParty.MapEvent != null)
				{
					MBTextManager.SetTextVariable("DEFENDER", leaderParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender).Name, false);
					MBTextManager.SetTextVariable("ATTACKER", leaderParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker).Name, false);
					MBTextManager.SetTextVariable("JOIN_SIEGE_TEXT", new TextObject("{=Qx8LaNhC}{DEFENDER} are fighting against {ATTACKER}.", null), false);
					return;
				}
				if (leaderParty.IsMainParty)
				{
					MBTextManager.SetTextVariable("JOIN_SIEGE_TEXT", new TextObject("{=jZ8Eqxbf}You are besieging the settlement.", null), false);
					return;
				}
			}
			else
			{
				MBTextManager.SetTextVariable("JOIN_SIEGE_TEXT", new TextObject("{=arCGUuR5}The settlement is under siege.", null), false);
			}
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x0005AC10 File Offset: 0x00058E10
		[GameMenuInitializationHandler("village_loot_complete")]
		private static void game_menu_village_loot_complete_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.Village.WaitMeshName);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0005AC2C File Offset: 0x00058E2C
		[GameMenuInitializationHandler("village_start_attack")]
		public static void game_menu_village_start_attack_on_init(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			args.MenuContext.SetBackgroundMeshName(village.WaitMeshName);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x0005AC58 File Offset: 0x00058E58
		[GameMenuInitializationHandler("town_wait")]
		[GameMenuInitializationHandler("town_guard")]
		[GameMenuInitializationHandler("menu_tournament_withdraw_verify")]
		[GameMenuInitializationHandler("menu_tournament_bet_confirm")]
		[GameMenuInitializationHandler("siege_attacker_defeated")]
		public static void game_menu_town_menu_on_init(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
				return;
			}
			if (PlayerSiege.BesiegedSettlement != null)
			{
				args.MenuContext.SetBackgroundMeshName(PlayerSiege.BesiegedSettlement.SettlementComponent.WaitMeshName);
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0005ACA8 File Offset: 0x00058EA8
		[GameMenuInitializationHandler("siege_attacker_left")]
		public static void game_menu_attackers_left_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_besieging");
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x0005ACBA File Offset: 0x00058EBA
		[GameMenuInitializationHandler("new_game_begin")]
		public static void game_menu_new_game_begin_on_init(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
			Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.EncyclopediaLink.ToString());
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0005ACDF File Offset: 0x00058EDF
		[GameMenuEventHandler("kingdom", "mno_call_to_arms", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_kingdom_mno_call_to_arms_on_consequence(MenuCallbackArgs args)
		{
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0005ACE1 File Offset: 0x00058EE1
		[GameMenuEventHandler("kingdom", "encyclopedia", GameMenuEventHandler.EventType.OnConsequence)]
		[GameMenuEventHandler("reports", "encyclopedia", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_encyclopedia_on_consequence(MenuCallbackArgs args)
		{
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0005ACE3 File Offset: 0x00058EE3
		[GameMenuInitializationHandler("request_meeting")]
		[GameMenuInitializationHandler("request_meeting_with_besiegers")]
		public static void game_menu_town_menu_request_meeting_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(PlayerEncounter.EncounterSettlement.SettlementComponent.WaitMeshName);
		}
	}
}
