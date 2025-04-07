using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x0200027B RID: 635
	public class TournamentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060021EA RID: 8682 RVA: 0x00090440 File Offset: 0x0008E640
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.TownRebelliosStateChanged.AddNonSerializedListener(this, new Action<Town, bool>(this.OnTownRebelliousStateChanged));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0009051C File Offset: 0x0008E71C
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			Campaign.Current.TournamentManager.InitializeLeaderboardEntry(Hero.MainHero, 0);
			this.InitializeTournamentLeaderboard();
			for (int i = 0; i < 3; i++)
			{
				foreach (Town town in Town.AllTowns)
				{
					if (town.IsTown)
					{
						this.ConsiderStartOrEndTournament(town);
					}
				}
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000905A0 File Offset: 0x0008E7A0
		private void OnDailyTick()
		{
			Hero leaderBoardLeader = Campaign.Current.TournamentManager.GetLeaderBoardLeader();
			if (leaderBoardLeader != null && leaderBoardLeader.IsAlive && leaderBoardLeader.Clan != null)
			{
				leaderBoardLeader.Clan.AddRenown(1f, true);
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000905E4 File Offset: 0x0008E7E4
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			foreach (Town town in Town.AllTowns)
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(town);
				if (tournamentGame != null && tournamentGame.Prize != null && (tournamentGame.Prize == DefaultItems.Trash || !tournamentGame.Prize.IsReady))
				{
					tournamentGame.UpdateTournamentPrize(false, true);
				}
			}
			foreach (KeyValuePair<Town, CampaignTime> keyValuePair in this._lastCreatedTournamentDatesInTowns.ToList<KeyValuePair<Town, CampaignTime>>())
			{
				if (keyValuePair.Value.ElapsedDaysUntilNow >= 15f)
				{
					this._lastCreatedTournamentDatesInTowns.Remove(keyValuePair.Key);
				}
			}
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000906D8 File Offset: 0x0008E8D8
		private void OnTownRebelliousStateChanged(Town town, bool rebelliousState)
		{
			if (town.InRebelliousState)
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(town);
				if (tournamentGame != null)
				{
					Campaign.Current.TournamentManager.ResolveTournament(tournamentGame, town);
				}
			}
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x00090714 File Offset: 0x0008E914
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			Town town = siegeEvent.BesiegedSettlement.Town;
			if (town != null)
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(town);
				if (tournamentGame != null)
				{
					Campaign.Current.TournamentManager.ResolveTournament(tournamentGame, town);
				}
			}
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00090755 File Offset: 0x0008E955
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Town, CampaignTime>>("_lastCreatedTournamentTimesInTowns", ref this._lastCreatedTournamentDatesInTowns);
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00090769 File Offset: 0x0008E969
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			Campaign.Current.TournamentManager.DeleteLeaderboardEntry(victim);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0009077B File Offset: 0x0008E97B
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x0009078B File Offset: 0x0008E98B
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.IsTown)
			{
				this.ConsiderStartOrEndTournament(settlement.Town);
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000907A4 File Offset: 0x0008E9A4
		private void ConsiderStartOrEndTournament(Town town)
		{
			CampaignTime campaignTime;
			if (!this._lastCreatedTournamentDatesInTowns.TryGetValue(town, out campaignTime) || campaignTime.ElapsedDaysUntilNow >= 15f)
			{
				ITournamentManager tournamentManager = Campaign.Current.TournamentManager;
				TournamentGame tournamentGame = tournamentManager.GetTournamentGame(town);
				if (tournamentGame != null && tournamentGame.CreationTime.ElapsedDaysUntilNow >= (float)tournamentGame.RemoveTournamentAfterDays)
				{
					tournamentManager.ResolveTournament(tournamentGame, town);
				}
				if (tournamentGame == null)
				{
					if (MBRandom.RandomFloat < Campaign.Current.Models.TournamentModel.GetTournamentStartChance(town))
					{
						tournamentManager.AddTournament(Campaign.Current.Models.TournamentModel.CreateTournament(town));
						if (!this._lastCreatedTournamentDatesInTowns.ContainsKey(town))
						{
							this._lastCreatedTournamentDatesInTowns.Add(town, CampaignTime.Now);
							return;
						}
						this._lastCreatedTournamentDatesInTowns[town] = CampaignTime.Now;
						return;
					}
				}
				else if (tournamentGame.CreationTime.ElapsedDaysUntilNow < (float)tournamentGame.RemoveTournamentAfterDays && MBRandom.RandomFloat < Campaign.Current.Models.TournamentModel.GetTournamentEndChance(tournamentGame))
				{
					tournamentManager.ResolveTournament(tournamentGame, town);
				}
			}
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000908B4 File Offset: 0x0008EAB4
		private void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			if (winner.IsHero && winner.HeroObject.Clan != null)
			{
				winner.HeroObject.Clan.AddRenown((float)Campaign.Current.Models.TournamentModel.GetRenownReward(winner.HeroObject, town), true);
				GainKingdomInfluenceAction.ApplyForDefault(winner.HeroObject, (float)Campaign.Current.Models.TournamentModel.GetInfluenceReward(winner.HeroObject, town));
			}
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x0009092A File Offset: 0x0008EB2A
		private float GetTournamentSimulationScore(Hero hero)
		{
			return Campaign.Current.Models.TournamentModel.GetTournamentSimulationScore(hero.CharacterObject);
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00090948 File Offset: 0x0008EB48
		private void InitializeTournamentLeaderboard()
		{
			Hero[] array = (from x in Hero.AllAliveHeroes
			where x.IsLord && this.GetTournamentSimulationScore(x) > 1.5f
			select x).ToArray<Hero>();
			int numLeaderboardVictoriesAtGameStart = Campaign.Current.Models.TournamentModel.GetNumLeaderboardVictoriesAtGameStart();
			if (array.Length < 3)
			{
				return;
			}
			List<Hero> list = new List<Hero>();
			for (int i = 0; i < numLeaderboardVictoriesAtGameStart; i++)
			{
				list.Clear();
				for (int j = 0; j < 16; j++)
				{
					Hero item = array[MBRandom.RandomInt(array.Length)];
					list.Add(item);
				}
				Hero hero = null;
				float num = 0f;
				foreach (Hero hero2 in list)
				{
					float num2 = this.GetTournamentSimulationScore(hero2) * (0.8f + 0.2f * MBRandom.RandomFloat);
					if (num2 > num)
					{
						num = num2;
						hero = hero2;
					}
				}
				Campaign.Current.TournamentManager.AddLeaderboardEntry(hero);
				hero.Clan.AddRenown((float)Campaign.Current.Models.TournamentModel.GetRenownReward(hero, null), true);
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00090A78 File Offset: 0x0008EC78
		protected void AddDialogs(CampaignGameStarter campaignGameSystemStarter)
		{
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00090A7C File Offset: 0x0008EC7C
		protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "join_tournament", "{=LN09ZLXZ}Join the tournament", new GameMenuOption.OnConditionDelegate(this.game_menu_join_tournament_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("menu_town_tournament_join");
			}, false, 1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "mno_tournament_event_watch", "{=6bQIRaIl}Watch the tournament", new GameMenuOption.OnConditionDelegate(this.game_menu_tournament_watch_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_tournament_watch_current_game_on_consequence), false, 2, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "mno_see_tournament_leaderboard", "{=vGF5S2hE}Leaderboard", new GameMenuOption.OnConditionDelegate(TournamentCampaignBehavior.game_menu_town_arena_see_leaderboard_on_condition), null, false, 3, false, null);
			campaignGameSystemStarter.AddGameMenu("menu_town_tournament_join", "{=5Adr6toM}{MENU_TEXT}", new OnInitDelegate(this.game_menu_tournament_join_on_init), GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_town_tournament_join", "mno_tournament_event_1", "{=es0Y3Bxc}Join", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Mission;
				return true;
			}, new GameMenuOption.OnConsequenceDelegate(this.game_menu_tournament_join_current_game_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_town_tournament_join", "mno_tournament_leave", "{=3sRdGQou}Leave", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("town_arena");
			}, true, -1, false, null);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x00090BDE File Offset: 0x0008EDDE
		[GameMenuEventHandler("town_arena", "mno_see_tournament_leaderboard", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_ui_town_arena_see_leaderboard_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTournamentLeaderboards();
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00090BEC File Offset: 0x0008EDEC
		private bool game_menu_join_tournament_on_condition(MenuCallbackArgs args)
		{
			bool shouldBeDisabled;
			TextObject disabledText;
			bool canPlayerDo = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.JoinTournament, out shouldBeDisabled, out disabledText);
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return MenuHelper.SetOptionProperties(args, canPlayerDo, shouldBeDisabled, disabledText);
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x00090C29 File Offset: 0x0008EE29
		private static bool game_menu_town_arena_see_leaderboard_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leaderboard;
			return Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown;
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00090C48 File Offset: 0x0008EE48
		[GameMenuInitializationHandler("menu_town_tournament_join")]
		private static void game_menu_ui_town_ui_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Town.WaitMeshName);
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00090C74 File Offset: 0x0008EE74
		private void game_menu_tournament_join_on_init(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			tournamentGame.UpdateTournamentPrize(true, false);
			GameTexts.SetVariable("MENU_TEXT", tournamentGame.GetMenuText());
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00090CB4 File Offset: 0x0008EEB4
		private void game_menu_tournament_join_current_game_on_consequence(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			GameMenu.SwitchToMenu("town");
			tournamentGame.PrepareForTournamentGame(true);
			Campaign.Current.TournamentManager.OnPlayerJoinTournament(tournamentGame.GetType(), Settlement.CurrentSettlement);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00090D08 File Offset: 0x0008EF08
		private bool game_menu_tournament_watch_on_condition(MenuCallbackArgs args)
		{
			bool shouldBeDisabled;
			TextObject disabledText;
			bool canPlayerDo = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.WatchTournament, out shouldBeDisabled, out disabledText);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return MenuHelper.SetOptionProperties(args, canPlayerDo, shouldBeDisabled, disabledText);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00090D44 File Offset: 0x0008EF44
		private void game_menu_tournament_watch_current_game_on_consequence(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			GameMenu.SwitchToMenu("town");
			tournamentGame.PrepareForTournamentGame(false);
			Campaign.Current.TournamentManager.OnPlayerWatchTournament(tournamentGame.GetType(), Settlement.CurrentSettlement);
		}

		// Token: 0x04000A75 RID: 2677
		private const int TournamentCooldownDurationAsDays = 15;

		// Token: 0x04000A76 RID: 2678
		private Dictionary<Town, CampaignTime> _lastCreatedTournamentDatesInTowns = new Dictionary<Town, CampaignTime>();
	}
}
