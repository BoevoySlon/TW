using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D7 RID: 983
	public class SiegeEventCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06003CA5 RID: 15525 RVA: 0x001268B0 File Offset: 0x00124AB0
		private TextObject _currentSiegeDescription
		{
			get
			{
				if (PlayerSiege.PlayerSiegeEvent == null)
				{
					return TextObject.Empty;
				}
				TextObject textObject = (PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? this._attackerSummaryText : this._defenderSummaryText;
				Settlement settlement = PlayerEncounter.EncounterSettlement ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				textObject.SetTextVariable("SETTLEMENT", settlement.Name);
				Hero leaderOfSiegeEvent = Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(PlayerSiege.PlayerSiegeEvent, PlayerSiege.PlayerSide);
				if (leaderOfSiegeEvent == Hero.MainHero)
				{
					TextObject variable = (PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? new TextObject("{=0DpoSNky}You are commanding the besiegers.", null) : new TextObject("{=W0FR7yy0}You are commanding the defenders.", null);
					textObject.SetTextVariable("FURTHER_EXPLANATION", variable);
				}
				else if (leaderOfSiegeEvent != null)
				{
					TextObject textObject2 = (PlayerSiege.PlayerSide == BattleSideEnum.Attacker) ? new TextObject("{=d2spYiHG}{LEADER.NAME} is commanding the besiegers.", null) : new TextObject("{=Ja8dMYHi}{LEADER.NAME} is commanding the defenders.", null);
					StringHelpers.SetCharacterProperties("LEADER", leaderOfSiegeEvent.CharacterObject, textObject2, false);
					textObject.SetTextVariable("FURTHER_EXPLANATION", textObject2);
				}
				else
				{
					textObject.SetTextVariable("FURTHER_EXPLANATION", TextObject.Empty);
				}
				return textObject;
			}
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x001269BC File Offset: 0x00124BBC
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
			CampaignEvents.SiegeEngineBuiltEvent.AddNonSerializedListener(this, new Action<SiegeEvent, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineBuilt));
			CampaignEvents.OnSiegeEngineDestroyedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineDestroyed));
			CampaignEvents.OnSiegeBombardmentHitEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, SiegeBombardTargets>(this.OnSiegeEngineHit));
			CampaignEvents.OnSiegeBombardmentWallHitEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, bool>(this.OnSiegeBombardmentWallHit));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceDeclared));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x00126A84 File Offset: 0x00124C84
		private void OnPeaceDeclared(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if (Campaign.Current.CurrentMenuContext != null && Game.Current.GameStateManager.ActiveState != null && Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu() == "menu_siege_strategies")
			{
				Campaign.Current.CurrentMenuContext.Refresh();
			}
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00126ADD File Offset: 0x00124CDD
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (settlement.SiegeEvent != null && party == MobileParty.MainParty)
			{
				this.SetDefaultTactics(settlement.SiegeEvent, BattleSideEnum.Defender);
			}
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00126AFC File Offset: 0x00124CFC
		private void OnSiegeBombardmentWallHit(MobileParty party, Settlement settlement, BattleSideEnum battleSide, SiegeEngineType siegeEngine, bool isWallCracked)
		{
			if (isWallCracked && party != null)
			{
				SkillLevelingManager.OnWallBreached(party);
			}
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00126B0B File Offset: 0x00124D0B
		private void OnSiegeEngineHit(MobileParty party, Settlement settlement, BattleSideEnum side, SiegeEngineType engine, SiegeBombardTargets target)
		{
			if (target == SiegeBombardTargets.RangedEngines)
			{
				this.BombardHitEngineCasualties(settlement.SiegeEvent.GetSiegeEventSide(side), engine);
			}
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x00126B28 File Offset: 0x00124D28
		private void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum lostSide, SiegeEngineType siegeEngine)
		{
			SiegeEventModel siegeEventModel = Campaign.Current.Models.SiegeEventModel;
			SiegeEvent siegeEvent = besiegedSettlement.SiegeEvent;
			MobileParty effectiveSiegePartyForSide = siegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, lostSide);
			MobileParty effectiveSiegePartyForSide2 = siegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, lostSide.GetOppositeSide());
			if (effectiveSiegePartyForSide2 != null)
			{
				SkillLevelingManager.OnSiegeEngineDestroyed(effectiveSiegePartyForSide2, siegeEngine);
			}
			float casualtyChance = Campaign.Current.Models.SiegeEventModel.GetCasualtyChance(effectiveSiegePartyForSide, siegeEvent, lostSide);
			if (MBRandom.RandomFloat <= casualtyChance)
			{
				ISiegeEventSide siegeEventSide = siegeEvent.GetSiegeEventSide(lostSide);
				int num = siegeEventModel.GetSiegeEngineDestructionCasualties(siegeEvent, siegeEventSide.BattleSide, siegeEngine);
				BattleSideEnum oppositeSide = siegeEventSide.BattleSide.GetOppositeSide();
				if (effectiveSiegePartyForSide2 != null && oppositeSide == BattleSideEnum.Attacker && effectiveSiegePartyForSide2.HasPerk(DefaultPerks.Tactics.PickThemOfTheWalls, false) && MBRandom.RandomFloat < DefaultPerks.Tactics.PickThemOfTheWalls.PrimaryBonus)
				{
					num *= 2;
				}
				if (oppositeSide == BattleSideEnum.Defender)
				{
					Hero governor = siegeEvent.BesiegedSettlement.Town.Governor;
					if (governor != null && governor.GetPerkValue(DefaultPerks.Tactics.PickThemOfTheWalls) && MBRandom.RandomFloat < DefaultPerks.Tactics.PickThemOfTheWalls.SecondaryBonus)
					{
						num *= 2;
					}
				}
				this.KillRandomTroopsOfEnemy(siegeEventSide, num);
			}
		}

		// Token: 0x06003CAC RID: 15532 RVA: 0x00126C34 File Offset: 0x00124E34
		private void OnSiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngineType)
		{
			MobileParty effectiveSiegePartyForSide = Campaign.Current.Models.SiegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, side);
			if (effectiveSiegePartyForSide != null)
			{
				SkillLevelingManager.OnSiegeEngineBuilt(effectiveSiegePartyForSide, siegeEngineType);
				if (effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Apprenticeship, false))
				{
					for (int i = 0; i < effectiveSiegePartyForSide.MemberRoster.Count; i++)
					{
						CharacterObject characterAtIndex = effectiveSiegePartyForSide.MemberRoster.GetCharacterAtIndex(i);
						if (!characterAtIndex.IsHero)
						{
							int elementNumber = effectiveSiegePartyForSide.MemberRoster.GetElementNumber(i);
							effectiveSiegePartyForSide.MemberRoster.AddXpToTroop(elementNumber * (int)DefaultPerks.Engineering.Apprenticeship.PrimaryBonus, characterAtIndex);
						}
					}
				}
			}
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x00126CC4 File Offset: 0x00124EC4
		private int KillRandomTroopsOfEnemy(ISiegeEventSide siegeEventSide, int count)
		{
			SiegeEvent siegeEvent = siegeEventSide.SiegeEvent;
			IEnumerable<PartyBase> involvedPartiesForEventType = siegeEventSide.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege);
			int num = involvedPartiesForEventType.Sum((PartyBase p) => p.NumberOfRegularMembers);
			if (num == 0 || count == 0)
			{
				return 0;
			}
			int num2 = 0;
			int num3 = MBRandom.RandomInt(involvedPartiesForEventType.Count<PartyBase>() - 1);
			for (int i = 0; i < involvedPartiesForEventType.Count<PartyBase>(); i++)
			{
				PartyBase partyBase = involvedPartiesForEventType.ElementAt((i + num3) % involvedPartiesForEventType.Count<PartyBase>());
				float siegeBombardmentHitSurgeryChance = Campaign.Current.Models.PartyHealingModel.GetSiegeBombardmentHitSurgeryChance(partyBase);
				float num4 = (float)partyBase.NumberOfRegularMembers / (float)num;
				float randomFloat = MBRandom.RandomFloat;
				int num5 = MathF.Min(MBRandom.RoundRandomized((float)(count - num2) * (num4 + randomFloat)), count);
				if (num5 > 0)
				{
					int num6 = MathF.Round((float)num5 * siegeBombardmentHitSurgeryChance);
					num2 += num5;
					num5 -= num6;
					siegeEventSide.OnTroopsKilledOnSide(num5);
					partyBase.MemberRoster.KillNumberOfNonHeroTroopsRandomly(num5);
					if (num6 > 0)
					{
						partyBase.MemberRoster.WoundNumberOfTroopsRandomly(num6);
					}
				}
				if (num2 >= count)
				{
					break;
				}
			}
			return num2;
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x00126DDC File Offset: 0x00124FDC
		private void BombardHitEngineCasualties(ISiegeEventSide siegeEventSide, SiegeEngineType attackerEngineType)
		{
			SiegeEvent siegeEvent = siegeEventSide.SiegeEvent;
			Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
			BesiegerCamp besiegerCamp = siegeEvent.BesiegerCamp;
			MobileParty effectiveSiegePartyForSide = Campaign.Current.Models.SiegeEventModel.GetEffectiveSiegePartyForSide(siegeEvent, siegeEventSide.BattleSide);
			ISiegeEventSide siegeEventSide2 = siegeEvent.GetSiegeEventSide(siegeEventSide.BattleSide.GetOppositeSide());
			float siegeEngineHitChance = Campaign.Current.Models.SiegeEventModel.GetSiegeEngineHitChance(attackerEngineType, siegeEventSide.BattleSide, SiegeBombardTargets.People, besiegedSettlement.Town);
			if (MBRandom.RandomFloat < siegeEngineHitChance)
			{
				int colleteralDamageCasualties = Campaign.Current.Models.SiegeEventModel.GetColleteralDamageCasualties(attackerEngineType, effectiveSiegePartyForSide);
				if (this.KillRandomTroopsOfEnemy(siegeEventSide2, colleteralDamageCasualties) > 0)
				{
					CampaignEventDispatcher.Instance.OnSiegeBombardmentHit(besiegerCamp.LeaderParty, besiegedSettlement, siegeEventSide.BattleSide, attackerEngineType, SiegeBombardTargets.People);
				}
			}
		}

		// Token: 0x06003CAF RID: 15535 RVA: 0x00126E98 File Offset: 0x00125098
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003CB0 RID: 15536 RVA: 0x00126E9A File Offset: 0x0012509A
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x00126EA3 File Offset: 0x001250A3
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			this.SetDefaultTactics(siegeEvent, BattleSideEnum.Attacker);
			this.SetDefaultTactics(siegeEvent, BattleSideEnum.Defender);
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x00126EB8 File Offset: 0x001250B8
		protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddWaitGameMenu("menu_siege_strategies", "{=!}{CURRENT_STRATEGY}", new OnInitDelegate(this.game_menu_siege_strategies_on_init), null, null, new OnTickDelegate(this.game_menu_siege_strategies_on_tick), GameMenu.MenuAndOptionType.WaitMenuHideProgressAndHoursOption, GameOverlays.MenuOverlayType.Encounter, 0f, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_lead_assault", "{=mjOcwUSA}Lead an assault", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.game_menu_siege_strategies_lead_assault_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_siege_strategies_lead_assault_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_order_troops", "{=TtGJqRI5}Send troops", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.game_menu_siege_strategies_order_assault_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_order_an_assault_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_request_parley", "{=2xVbLS5r}Request a parley", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.menu_defender_side_request_audience_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_defender_side_request_audience_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_break_out", "{=dFcgXnQq}Break out", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.menu_defender_siege_break_out_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_defender_siege_break_out_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_sally_out", "{=!}{SALLY_OUT_BUTTON_TEXT}", new GameMenuOption.OnConditionDelegate(SiegeEventCampaignBehavior.menu_sally_out_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_sally_out_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.menu_siege_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_siege_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_leave_army", "{=hSdJ0UUv}Leave Army", new GameMenuOption.OnConditionDelegate(this.menu_siege_strategies_passive_wait_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_siege_strategies_passive_wait_leave_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("menu_siege_strategies_break_siege", "{=!}{SIEGE_LEAVE_TEXT}", new OnInitDelegate(this.menu_break_siege_on_init), GameOverlays.MenuOverlayType.Encounter, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies_break_siege", "menu_siege_strategies_break_siege_return", "{=25ifdWOy}Return to siege", new GameMenuOption.OnConditionDelegate(this.return_siege_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("menu_siege_strategies");
			}, false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies_break_siege", "menu_siege_strategies_break_siege_go_on", "{=TGYJUUn0}Go on.", new GameMenuOption.OnConditionDelegate(this.leave_siege_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_end_besieging_on_consequence), true, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("menu_siege_safe_passage_accepted", "Besiegers have agreed to allow safe passage for your party.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_safe_passage_accepted", "menu_siege_safe_passage_accepted_leave", "Leave", new GameMenuOption.OnConditionDelegate(this.leave_siege_on_condition), new GameMenuOption.OnConsequenceDelegate(SiegeEventCampaignBehavior.menu_siege_leave_on_consequence), true, -1, false, null);
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x00127128 File Offset: 0x00125328
		private void game_menu_siege_strategies_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			string genericStateMenu = Campaign.Current.Models.EncounterGameMenuModel.GetGenericStateMenu();
			if (!(genericStateMenu != "menu_siege_strategies"))
			{
				args.MenuContext.GameMenu.GetText().SetTextVariable("CURRENT_STRATEGY", this._currentSiegeDescription);
				Campaign.Current.GameMenuManager.RefreshMenuOptions(args.MenuContext);
				return;
			}
			if (!string.IsNullOrEmpty(genericStateMenu))
			{
				GameMenu.SwitchToMenu(genericStateMenu);
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x001271A2 File Offset: 0x001253A2
		private void game_menu_siege_strategies_on_init(MenuCallbackArgs args)
		{
			MBTextManager.SetTextVariable("CURRENT_STRATEGY", this._currentSiegeDescription, false);
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x001271B5 File Offset: 0x001253B5
		private static void menu_siege_strategies_lead_assault_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.IsActive)
			{
				PlayerEncounter.LeaveEncounter = false;
			}
			else
			{
				EncounterManager.StartSettlementEncounter(MobileParty.MainParty, PlayerSiege.PlayerSiegeEvent.BesiegedSettlement);
			}
			GameMenu.SwitchToMenu("assault_town");
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x001271E4 File Offset: 0x001253E4
		private static void menu_order_an_assault_on_consequence(MenuCallbackArgs args)
		{
			if (PlayerEncounter.IsActive)
			{
				PlayerEncounter.LeaveEncounter = false;
			}
			else
			{
				PlayerEncounter.Start();
				PlayerEncounter.Current.SetupFields(PartyBase.MainParty, PlayerSiege.PlayerSiegeEvent.BesiegedSettlement.Party);
			}
			GameMenu.SwitchToMenu("assault_town_order_attack");
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x00127222 File Offset: 0x00125422
		private bool menu_siege_strategies_order_troops_on_condition(MenuCallbackArgs args)
		{
			args.IsEnabled = (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty);
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x00127258 File Offset: 0x00125458
		private bool menu_siege_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty) && ((PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapFaction)) || (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Attacker));
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x001272D8 File Offset: 0x001254D8
		private bool menu_siege_strategies_passive_wait_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Attacker && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00127328 File Offset: 0x00125528
		private void menu_break_siege_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.SiegeEvent.BesiegerCamp.LeaderParty == MobileParty.MainParty)
			{
				MBTextManager.SetTextVariable("SIEGE_LEAVE_TEXT", this._removeSiegeCompletelyText, false);
			}
			else
			{
				MBTextManager.SetTextVariable("SIEGE_LEAVE_TEXT", this._leaveSiegeText, false);
			}
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x0012737F File Offset: 0x0012557F
		private bool return_siege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x0012738A File Offset: 0x0012558A
		private bool leave_siege_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x00127395 File Offset: 0x00125595
		private void menu_siege_strategies_passive_wait_leave_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				PlayerSiege.ClosePlayerSiege();
			}
			MobileParty.MainParty.BesiegerCamp = null;
			MobileParty.MainParty.Army = null;
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x001273C0 File Offset: 0x001255C0
		private static bool game_menu_siege_strategies_order_assault_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.OrderTroopsToAttack;
			if (MobileParty.MainParty.BesiegedSettlement == null)
			{
				return false;
			}
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(MobileParty.MainParty.BesiegedSettlement.SiegeEvent, PlayerSiege.PlayerSide) == Hero.MainHero)
			{
				Settlement settlement = (PlayerEncounter.EncounteredParty != null) ? PlayerEncounter.EncounteredParty.Settlement : PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				if (PlayerSiege.PlayerSide == BattleSideEnum.Attacker && !settlement.SiegeEvent.BesiegerCamp.IsPreparationComplete)
				{
					args.IsEnabled = false;
					args.Tooltip = SiegeEventCampaignBehavior._waitSiegeEquipmentsText;
				}
				else
				{
					bool flag = MobileParty.MainParty.MemberRoster.GetTroopRoster().Any(delegate(TroopRosterElement x)
					{
						if (x.Character.IsHero)
						{
							return x.Character != CharacterObject.PlayerCharacter && !x.Character.HeroObject.IsWounded;
						}
						return x.Number > x.WoundedNumber;
					});
					if (!flag && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
					{
						foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
						{
							flag = mobileParty.MemberRoster.GetTroopRoster().Any(delegate(TroopRosterElement x)
							{
								if (x.Character.IsHero)
								{
									return x.Character != CharacterObject.PlayerCharacter && !x.Character.HeroObject.IsWounded;
								}
								return x.Number > x.WoundedNumber;
							});
							if (flag)
							{
								break;
							}
						}
					}
					args.Tooltip = TooltipHelper.GetSendTroopsPowerContextTooltipForSiege();
					if (!flag)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=ao9bhAhf}You are not leading any troops", null);
					}
				}
			}
			else
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=1Hd19nq5}You are not in command of this siege.", null);
			}
			return true;
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x00127584 File Offset: 0x00125784
		private static bool game_menu_siege_strategies_lead_assault_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.LeadAssault;
			if (MobileParty.MainParty.BesiegedSettlement == null)
			{
				return false;
			}
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(MobileParty.MainParty.BesiegedSettlement.SiegeEvent, PlayerSiege.PlayerSide) == Hero.MainHero)
			{
				Settlement settlement = (PlayerEncounter.EncounteredParty != null) ? PlayerEncounter.EncounteredParty.Settlement : PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
				if (PlayerSiege.PlayerSide == BattleSideEnum.Attacker && !settlement.SiegeEvent.BesiegerCamp.IsPreparationComplete)
				{
					args.IsEnabled = false;
					args.Tooltip = SiegeEventCampaignBehavior._waitSiegeEquipmentsText;
				}
				else if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = SiegeEventCampaignBehavior._woundedAssaultText;
				}
			}
			else
			{
				args.IsEnabled = false;
				args.Tooltip = SiegeEventCampaignBehavior._noCommandText;
			}
			return true;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x00127658 File Offset: 0x00125858
		private static void LeaveSiege()
		{
			MobileParty.MainParty.BesiegerCamp = null;
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				MobileParty.MainParty.Army.AIBehavior = Army.AIBehaviorFlags.Unassigned;
			}
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Finish(true);
				return;
			}
			GameMenu.ExitToLast();
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x001276B8 File Offset: 0x001258B8
		private static void menu_siege_leave_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.BesiegerCamp == null)
			{
				if (PlayerEncounter.Current != null && MobileParty.MainParty.CurrentSettlement != null)
				{
					if (MobileParty.MainParty.Army != null)
					{
						MobileParty.MainParty.Army = null;
					}
					PlayerSiege.ClosePlayerSiege();
					PlayerEncounter.LeaveSettlement();
					PlayerEncounter.Finish(true);
					return;
				}
				GameMenu.ExitToLast();
				return;
			}
			else
			{
				if (MobileParty.MainParty.BesiegerCamp.LeaderParty == MobileParty.MainParty)
				{
					GameMenu.SwitchToMenu("menu_siege_strategies_break_siege");
					return;
				}
				SiegeEventCampaignBehavior.LeaveSiege();
				return;
			}
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x00127738 File Offset: 0x00125938
		private static void menu_end_besieging_on_consequence(MenuCallbackArgs args)
		{
			SiegeEventCampaignBehavior.LeaveSiege();
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x00127740 File Offset: 0x00125940
		private static bool menu_defender_side_request_audience_on_condition(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender)
			{
				return false;
			}
			if (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapFaction))
			{
				return false;
			}
			Settlement settlement = Settlement.CurrentSettlement ?? PlayerSiege.PlayerSiegeEvent.BesiegedSettlement;
			if (settlement.SiegeEvent != null)
			{
				if (!settlement.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Any((PartyBase party) => party.LeaderHero != null && party.MobileParty.IsLordParty))
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=rO704KOG}There is no one with the authority to talk to you.", null);
				}
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null)
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=1UO0yMBr}You can not parley during an ongoing battle.", null);
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			return true;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x00127834 File Offset: 0x00125A34
		private static void menu_defender_side_request_audience_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("request_meeting_with_besiegers");
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x00127840 File Offset: 0x00125A40
		private static bool menu_sally_out_on_condition(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender)
			{
				return false;
			}
			if (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapFaction))
			{
				args.IsEnabled = false;
				args.Tooltip = new TextObject("{=UqaNs3ck}You are not at war with the besiegers.", null);
			}
			if (Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(PlayerSiege.PlayerSiegeEvent, PlayerSiege.PlayerSide) != Hero.MainHero && (PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent == null || !PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsSallyOut))
			{
				args.IsEnabled = false;
				TextObject tooltip = new TextObject("{=OmGHXuZB}You are not in command of the defenders.", null);
				args.Tooltip = tooltip;
			}
			if (PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null && PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsSallyOut)
			{
				args.Text.SetTextVariable("SALLY_OUT_BUTTON_TEXT", new TextObject("{=fyNNCOFK}Join the sally out", null));
			}
			else
			{
				args.Text.SetTextVariable("SALLY_OUT_BUTTON_TEXT", new TextObject("{=KKB2vNFr}Sally out", null));
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return true;
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0012798C File Offset: 0x00125B8C
		private static void menu_sally_out_on_consequence(MenuCallbackArgs args)
		{
			MobileParty leaderParty = Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.LeaderParty;
			if (leaderParty.Party.MapEvent != null)
			{
				leaderParty.Party.MapEvent.FinalizeEvent();
			}
			EncounterManager.StartPartyEncounter(MobileParty.MainParty.Party, leaderParty.Party);
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x001279E0 File Offset: 0x00125BE0
		private static bool menu_defender_siege_break_out_on_condition(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender)
			{
				return false;
			}
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty)
			{
				args.IsEnabled = true;
				TextObject textObject = new TextObject("{=!}If you break out from the siege, you will also leave the army. This is a dishonorable act and you will lose relations with all army member lords.{newline}• Army Leader: {ARMY_LEADER_RELATION_PENALTY}{newline}• Army Members: {ARMY_MEMBER_RELATION_PENALTY}", null);
				textObject.SetTextVariable("ARMY_LEADER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyLeaderRelationPenalty);
				textObject.SetTextVariable("ARMY_MEMBER_RELATION_PENALTY", Campaign.Current.Models.TroopSacrificeModel.BreakOutArmyMemberRelationPenalty);
				args.Tooltip = textObject;
			}
			if (PlayerSiege.PlayerSiegeEvent != null && PlayerSiege.PlayerSide == BattleSideEnum.Defender && !MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerSiege.PlayerSiegeEvent.BesiegerCamp.LeaderParty.MapFaction))
			{
				return false;
			}
			MobileParty mainParty = MobileParty.MainParty;
			SiegeEvent siegeEvent = Settlement.CurrentSettlement.SiegeEvent;
			int lostTroopCountForBreakingOutOfBesiegedSettlement = Campaign.Current.Models.TroopSacrificeModel.GetLostTroopCountForBreakingOutOfBesiegedSettlement(mainParty, siegeEvent);
			int num = (mainParty.Army != null && mainParty.Army.LeaderParty == mainParty) ? mainParty.Army.TotalRegularCount : mainParty.MemberRoster.TotalRegulars;
			if (lostTroopCountForBreakingOutOfBesiegedSettlement > num)
			{
				args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				args.IsEnabled = false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.LeaveTroopsAndFlee;
			return Hero.MainHero.MapFaction != siegeEvent.BesiegerCamp.LeaderParty.MapFaction;
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x00127B46 File Offset: 0x00125D46
		private static void menu_defender_siege_break_out_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("break_out_menu");
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00127B52 File Offset: 0x00125D52
		private void menu_siege_select_strategy_leave_on_consequence(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			GameMenu.SwitchToMenu("menu_siege_strategies");
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x00127B66 File Offset: 0x00125D66
		private void SetTactic(SiegeEvent siegeEvent, BattleSideEnum side, SiegeStrategy strategy)
		{
			siegeEvent.GetSiegeEventSide(side).SetSiegeStrategy(strategy);
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x00127B78 File Offset: 0x00125D78
		private void SetDefaultTactics(SiegeEvent siegeEvent, BattleSideEnum side)
		{
			Hero leaderOfSiegeEvent = Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(siegeEvent, side);
			SiegeStrategy strategy = null;
			if (leaderOfSiegeEvent == Hero.MainHero)
			{
				strategy = DefaultSiegeStrategies.Custom;
			}
			else
			{
				IEnumerable<SiegeStrategy> enumerable = (side == BattleSideEnum.Attacker) ? DefaultSiegeStrategies.AllAttackerStrategies : DefaultSiegeStrategies.AllDefenderStrategies;
				float num = float.MinValue;
				foreach (SiegeStrategy siegeStrategy in enumerable)
				{
					float num2 = Campaign.Current.Models.SiegeEventModel.GetSiegeStrategyScore(siegeEvent, side, siegeStrategy) * (0.5f + 0.5f * MBRandom.RandomFloat);
					if (num2 > num)
					{
						num = num2;
						strategy = siegeStrategy;
					}
				}
			}
			this.SetTactic(siegeEvent, side, strategy);
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x00127C34 File Offset: 0x00125E34
		[GameMenuInitializationHandler("menu_siege_strategies")]
		private static void game_menu_siege_strategies_background_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName("wait_besieging");
		}

		// Token: 0x0400120E RID: 4622
		private readonly TextObject _attackerSummaryText = new TextObject("{=sbmWGPYG}You are besieging {SETTLEMENT}. {FURTHER_EXPLANATION}", null);

		// Token: 0x0400120F RID: 4623
		private readonly TextObject _defenderSummaryText = new TextObject("{=l5YipTe3}{SETTLEMENT} is under siege. {FURTHER_EXPLANATION}", null);

		// Token: 0x04001210 RID: 4624
		private readonly TextObject _removeSiegeCompletelyText = new TextObject("{=5ZDCnrDQ}This will end the siege. You cannot take your siege engines with you, and they will be destroyed.", null);

		// Token: 0x04001211 RID: 4625
		private readonly TextObject _leaveSiegeText = new TextObject("{=176K8dcb}You will end the siege if you leave. Are you sure?", null);

		// Token: 0x04001212 RID: 4626
		private static readonly TextObject _waitSiegeEquipmentsText = new TextObject("{=bCuxzp1N}You need to wait for the siege equipment to be prepared.", null);

		// Token: 0x04001213 RID: 4627
		private static readonly TextObject _woundedAssaultText = new TextObject("{=gzYuWR28}You are wounded, and in no condition to lead an assault.", null);

		// Token: 0x04001214 RID: 4628
		private static readonly TextObject _noCommandText = new TextObject("{=1Hd19nq5}You are not in command of this siege.", null);
	}
}
