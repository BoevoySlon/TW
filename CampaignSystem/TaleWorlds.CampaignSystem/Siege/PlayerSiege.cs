using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x02000287 RID: 647
	public static class PlayerSiege
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x00093B1C File Offset: 0x00091D1C
		public static SiegeEvent PlayerSiegeEvent
		{
			get
			{
				SiegeEvent siegeEvent;
				if ((siegeEvent = MobileParty.MainParty.SiegeEvent) == null)
				{
					Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
					if (currentSettlement == null)
					{
						return null;
					}
					siegeEvent = currentSettlement.SiegeEvent;
				}
				return siegeEvent;
			}
		}

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x00093B41 File Offset: 0x00091D41
		public static Settlement BesiegedSettlement
		{
			get
			{
				SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
				if (playerSiegeEvent == null)
				{
					return null;
				}
				return playerSiegeEvent.BesiegedSettlement;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x00093B53 File Offset: 0x00091D53
		public static BattleSideEnum PlayerSide
		{
			get
			{
				if (MobileParty.MainParty.BesiegerCamp == null)
				{
					return BattleSideEnum.Defender;
				}
				return BattleSideEnum.Attacker;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x00093B64 File Offset: 0x00091D64
		public static bool IsRebellion
		{
			get
			{
				return PlayerSiege.BesiegedSettlement != null && PlayerSiege.BesiegedSettlement.IsUnderRebellionAttack();
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00093B79 File Offset: 0x00091D79
		private static void SetPlayerSiegeEvent()
		{
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x00093B7B File Offset: 0x00091D7B
		public static void StartSiegePreparation()
		{
			if (Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.ExitToLast();
			}
			GameMenu.ActivateGameMenu("menu_siege_strategies");
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x00093B98 File Offset: 0x00091D98
		public static void OnSiegeEventFinalized(bool besiegerPartyDefeated)
		{
			MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
			if (PlayerSiege.IsRebellion)
			{
				if (mapState != null && mapState.AtMenu)
				{
					GameMenu.ExitToLast();
					return;
				}
			}
			else if (PlayerSiege.PlayerSide == BattleSideEnum.Defender && !PlayerSiege.IsRebellion)
			{
				if (Settlement.CurrentSettlement != null)
				{
					if (mapState != null && !mapState.AtMenu)
					{
						GameMenu.ActivateGameMenu(besiegerPartyDefeated ? "siege_attacker_defeated" : "siege_attacker_left");
						return;
					}
					GameMenu.SwitchToMenu(besiegerPartyDefeated ? "siege_attacker_defeated" : "siege_attacker_left");
					return;
				}
			}
			else if (Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Army != null && Hero.MainHero.PartyBelongedTo.Army.LeaderParty != MobileParty.MainParty)
			{
				if (MobileParty.MainParty.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
				}
				if (PlayerEncounter.Battle == null)
				{
					if (mapState != null)
					{
						if (mapState.AtMenu)
						{
							GameMenu.SwitchToMenu("army_wait");
							return;
						}
						GameMenu.ActivateGameMenu("army_wait");
						return;
					}
					else
					{
						Campaign.Current.GameMenuManager.SetNextMenu("army_wait");
					}
				}
			}
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x00093CB0 File Offset: 0x00091EB0
		public static void StartPlayerSiege(BattleSideEnum playerSide, bool isSimulation = false, Settlement settlement = null)
		{
			if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				MobileParty.MainParty.Ai.SetMoveModeHold();
			}
			PlayerSiege.SetPlayerSiegeEvent();
			if (!isSimulation)
			{
				GameState gameState = Game.Current.GameStateManager.GameStates.FirstOrDefault((GameState s) => s is MapState);
				if (gameState != null)
				{
					MapState mapState = gameState as MapState;
					if (mapState != null)
					{
						mapState.OnPlayerSiegeActivated();
					}
				}
			}
			CampaignEventDispatcher.Instance.OnPlayerSiegeStarted();
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x00093D48 File Offset: 0x00091F48
		public static void ClosePlayerSiege()
		{
			if (PlayerSiege.PlayerSiegeEvent == null)
			{
				return;
			}
			PlayerSiege.BesiegedSettlement.Party.SetVisualAsDirty();
			Campaign.Current.autoEnterTown = null;
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				MobileParty.MainParty.Army.AiBehaviorObject = null;
			}
			MobileParty.MainParty.Ai.SetMoveModeHold();
			GameState gameState = Game.Current.GameStateManager.GameStates.FirstOrDefault((GameState s) => s is MapState);
			if (gameState != null)
			{
				MapState mapState = gameState as MapState;
				if (mapState == null)
				{
					return;
				}
				mapState.OnPlayerSiegeDeactivated();
			}
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x00093E00 File Offset: 0x00092000
		public static void StartSiegeMission(Settlement settlement = null)
		{
			Settlement besiegedSettlement = PlayerSiege.BesiegedSettlement;
			Settlement.SiegeState currentSiegeState = besiegedSettlement.CurrentSiegeState;
			if (currentSiegeState == Settlement.SiegeState.OnTheWalls)
			{
				List<MissionSiegeWeapon> preparedAndActiveSiegeEngines = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker));
				List<MissionSiegeWeapon> preparedAndActiveSiegeEngines2 = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender));
				bool hasAnySiegeTower = preparedAndActiveSiegeEngines.Exists((MissionSiegeWeapon data) => data.Type == DefaultSiegeEngineTypes.SiegeTower);
				int wallLevel = besiegedSettlement.Town.GetWallLevel();
				CampaignMission.OpenSiegeMissionWithDeployment(besiegedSettlement.LocationComplex.GetLocationWithId("center").GetSceneName(wallLevel), besiegedSettlement.SettlementWallSectionHitPointsRatioList.ToArray(), hasAnySiegeTower, preparedAndActiveSiegeEngines, preparedAndActiveSiegeEngines2, PlayerEncounter.Current.PlayerSide == BattleSideEnum.Attacker, wallLevel, false, false);
				return;
			}
			if (currentSiegeState != Settlement.SiegeState.Invalid)
			{
				return;
			}
			Debug.FailedAssert("Siege state is invalid!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Siege\\PlayerSiege.cs", "StartSiegeMission", 189);
		}
	}
}
