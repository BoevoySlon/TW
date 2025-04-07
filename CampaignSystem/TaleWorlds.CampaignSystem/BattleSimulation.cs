using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000024 RID: 36
	public class BattleSimulation : IBattleObserver
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000EC1A File Offset: 0x0000CE1A
		// (set) Token: 0x06000156 RID: 342 RVA: 0x0000EC22 File Offset: 0x0000CE22
		public bool IsSimulationPaused { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000EC2B File Offset: 0x0000CE2B
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000EC33 File Offset: 0x0000CE33
		public bool IsSimulationFinished { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000159 RID: 345 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		private bool IsPlayerJoinedBattle
		{
			get
			{
				return PlayerEncounter.Current.IsJoinedBattle;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600015A RID: 346 RVA: 0x0000EC48 File Offset: 0x0000CE48
		public MapEvent MapEvent
		{
			get
			{
				return this._mapEvent;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600015B RID: 347 RVA: 0x0000EC50 File Offset: 0x0000CE50
		// (set) Token: 0x0600015C RID: 348 RVA: 0x0000EC58 File Offset: 0x0000CE58
		public IBattleObserver BattleObserver
		{
			get
			{
				return this._battleObserver;
			}
			set
			{
				this._battleObserver = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000EC61 File Offset: 0x0000CE61
		// (set) Token: 0x0600015E RID: 350 RVA: 0x0000EC69 File Offset: 0x0000CE69
		public List<List<BattleResultPartyData>> Teams { get; private set; }

		// Token: 0x0600015F RID: 351 RVA: 0x0000EC74 File Offset: 0x0000CE74
		public BattleSimulation(FlattenedTroopRoster selectedTroopsForPlayerSide, FlattenedTroopRoster selectedTroopsForOtherSide)
		{
			this.IsSimulationPaused = true;
			float applicationTime = Game.Current.ApplicationTime;
			this._timer = new Timer(applicationTime, 1f, true);
			this._mapEvent = (PlayerEncounter.Battle ?? PlayerEncounter.StartBattle());
			this._mapEvent.IsPlayerSimulation = true;
			this._mapEvent.BattleObserver = this;
			this.SelectedTroops[(int)this._mapEvent.PlayerSide] = selectedTroopsForPlayerSide;
			this.SelectedTroops[(int)this._mapEvent.GetOtherSide(this._mapEvent.PlayerSide)] = selectedTroopsForOtherSide;
			this._mapEvent.GetNumberOfInvolvedMen();
			if (this._mapEvent.IsSiegeAssault)
			{
				PlayerSiege.StartPlayerSiege(MobileParty.MainParty.Party.Side, true, this._mapEvent.MapEventSettlement);
			}
			List<List<BattleResultPartyData>> list = new List<List<BattleResultPartyData>>
			{
				new List<BattleResultPartyData>(),
				new List<BattleResultPartyData>()
			};
			foreach (PartyBase partyBase in this._mapEvent.InvolvedParties)
			{
				BattleResultPartyData battleResultPartyData = default(BattleResultPartyData);
				bool flag = false;
				foreach (BattleResultPartyData battleResultPartyData2 in list[(int)partyBase.Side])
				{
					if (battleResultPartyData2.Party == partyBase)
					{
						flag = true;
						battleResultPartyData = battleResultPartyData2;
						break;
					}
				}
				if (!flag)
				{
					battleResultPartyData = new BattleResultPartyData(partyBase);
					list[(int)partyBase.Side].Add(battleResultPartyData);
				}
				for (int i = 0; i < partyBase.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = partyBase.MemberRoster.GetElementCopyAtIndex(i);
					if (!battleResultPartyData.Characters.Contains(elementCopyAtIndex.Character))
					{
						battleResultPartyData.Characters.Add(elementCopyAtIndex.Character);
					}
				}
			}
			this.Teams = list;
			this.tempRosterRefs = new List<TroopRoster>();
			foreach (PartyBase partyBase2 in this._mapEvent.InvolvedParties)
			{
				this.tempRosterRefs.Add(partyBase2.MemberRoster);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000EEE0 File Offset: 0x0000D0E0
		public void Play()
		{
			this._simulationState = BattleSimulation.SimulationState.Play;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000EEE9 File Offset: 0x0000D0E9
		public void FastForward()
		{
			this._simulationState = BattleSimulation.SimulationState.FastForward;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000EEF2 File Offset: 0x0000D0F2
		public void Skip()
		{
			this._simulationState = BattleSimulation.SimulationState.Skip;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		public void OnReturn()
		{
			foreach (PartyBase partyBase in this._mapEvent.InvolvedParties)
			{
				partyBase.MemberRoster.RemoveZeroCounts();
			}
			GameMenu.ActivateGameMenu("encounter");
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000EF5C File Offset: 0x0000D15C
		private void BattleEndLogic()
		{
			if (PlayerEncounter.Battle == null)
			{
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			BattleSideEnum side = PartyBase.MainParty.Side;
			if (PlayerEncounter.Battle.GetMapEventSide(side).NumRemainingSimulationTroops > 0)
			{
				GameMenu.SwitchToMenu("encounter");
				return;
			}
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000EFAA File Offset: 0x0000D1AA
		private void TickSimulateBattle()
		{
			BattleSimulation.SimulateBattleFromUi();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000EFB4 File Offset: 0x0000D1B4
		public void Tick(float dt)
		{
			if (this.IsSimulationFinished)
			{
				return;
			}
			if (PlayerEncounter.Current == null)
			{
				Debug.FailedAssert("PlayerEncounter.Current == null", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BattleSimulation.cs", "Tick", 222);
				this.IsSimulationFinished = true;
				return;
			}
			int num = 0;
			if (PlayerEncounter.BattleState == BattleState.None)
			{
				foreach (PartyBase partyBase in this.MapEvent.InvolvedParties)
				{
					if (partyBase.Side == MobileParty.MainParty.Party.Side && partyBase != MobileParty.MainParty.Party)
					{
						num += partyBase.NumberOfHealthyMembers;
					}
				}
			}
			if ((MobileParty.MainParty.MapEvent == this.MapEvent && MobileParty.MainParty.Party.NumberOfHealthyMembers == 1 && !Hero.MainHero.IsWounded && num == 0) || PlayerEncounter.BattleState == BattleState.AttackerVictory || PlayerEncounter.BattleState == BattleState.DefenderVictory)
			{
				this.IsSimulationFinished = true;
				return;
			}
			if (this._simulationState == BattleSimulation.SimulationState.Skip)
			{
				while (PlayerEncounter.BattleState == BattleState.None || PlayerEncounter.BattleState == BattleState.DefenderPullBack)
				{
					this.TickSimulateBattle();
					num = 0;
					if (PlayerEncounter.BattleState == BattleState.None || PlayerEncounter.BattleState == BattleState.DefenderPullBack)
					{
						foreach (PartyBase partyBase2 in this.MapEvent.InvolvedParties)
						{
							if (partyBase2.Side == MobileParty.MainParty.Party.Side && partyBase2 != MobileParty.MainParty.Party)
							{
								num += partyBase2.NumberOfHealthyMembers;
							}
						}
					}
					if (MobileParty.MainParty.MapEvent == this.MapEvent && MobileParty.MainParty.Party.NumberOfHealthyMembers <= 1 && num == 0)
					{
						return;
					}
				}
				return;
			}
			this._numTicks += dt;
			if (this._simulationState == BattleSimulation.SimulationState.FastForward)
			{
				this._numTicks *= 3f;
			}
			while (this._numTicks >= 1f)
			{
				this.TickSimulateBattle();
				this._numTicks -= 1f;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
		public static void SimulateBattleFromUi()
		{
			PlayerEncounter.SimulateBattle();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000F1D7 File Offset: 0x0000D3D7
		public void ResetSimulation()
		{
			this.MapEvent.SimulateBattleSetup(PlayerEncounter.CurrentBattleSimulation.SelectedTroops);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000F1F0 File Offset: 0x0000D3F0
		public void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberKilled = 0, int numberWounded = 0, int numberRouted = 0, int killCount = 0, int numberReadyToUpgrade = 0)
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.TroopNumberChanged(side, battleCombatant, character, number, numberKilled, numberWounded, numberRouted, killCount, numberReadyToUpgrade);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000F21C File Offset: 0x0000D41C
		public void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject skill)
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.HeroSkillIncreased(side, battleCombatant, heroCharacter, skill);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000F233 File Offset: 0x0000D433
		public void BattleResultsReady()
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.BattleResultsReady();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000F245 File Offset: 0x0000D445
		public void TroopSideChanged(BattleSideEnum prevSide, BattleSideEnum newSide, IBattleCombatant battleCombatant, BasicCharacterObject character)
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.TroopSideChanged(prevSide, newSide, battleCombatant, character);
		}

		// Token: 0x04000030 RID: 48
		private readonly MapEvent _mapEvent;

		// Token: 0x04000031 RID: 49
		public List<TroopRoster> tempRosterRefs;

		// Token: 0x04000032 RID: 50
		private IBattleObserver _battleObserver;

		// Token: 0x04000033 RID: 51
		private Timer _timer;

		// Token: 0x04000035 RID: 53
		public readonly FlattenedTroopRoster[] SelectedTroops = new FlattenedTroopRoster[2];

		// Token: 0x04000036 RID: 54
		private BattleSimulation.SimulationState _simulationState;

		// Token: 0x04000037 RID: 55
		private float _numTicks;

		// Token: 0x0200047B RID: 1147
		private enum SimulationState
		{
			// Token: 0x04001377 RID: 4983
			Play,
			// Token: 0x04001378 RID: 4984
			FastForward,
			// Token: 0x04001379 RID: 4985
			Skip
		}
	}
}
