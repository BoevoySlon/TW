using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200033C RID: 828
	public class MapState : GameState
	{
		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x06002ECD RID: 11981 RVA: 0x000C2149 File Offset: 0x000C0349
		// (set) Token: 0x06002ECE RID: 11982 RVA: 0x000C2151 File Offset: 0x000C0351
		public MenuContext MenuContext
		{
			get
			{
				return this._menuContext;
			}
			private set
			{
				this._menuContext = value;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x06002ECF RID: 11983 RVA: 0x000C215A File Offset: 0x000C035A
		// (set) Token: 0x06002ED0 RID: 11984 RVA: 0x000C216B File Offset: 0x000C036B
		public string GameMenuId
		{
			get
			{
				return Campaign.Current.MapStateData.GameMenuId;
			}
			set
			{
				Campaign.Current.MapStateData.GameMenuId = value;
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06002ED1 RID: 11985 RVA: 0x000C217D File Offset: 0x000C037D
		public bool AtMenu
		{
			get
			{
				return this.MenuContext != null;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06002ED2 RID: 11986 RVA: 0x000C2188 File Offset: 0x000C0388
		public bool MapConversationActive
		{
			get
			{
				return this._mapConversationActive;
			}
		}

		// Token: 0x17000B3E RID: 2878
		// (get) Token: 0x06002ED3 RID: 11987 RVA: 0x000C2190 File Offset: 0x000C0390
		// (set) Token: 0x06002ED4 RID: 11988 RVA: 0x000C2198 File Offset: 0x000C0398
		public IMapStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x17000B3F RID: 2879
		// (get) Token: 0x06002ED5 RID: 11989 RVA: 0x000C21A1 File Offset: 0x000C03A1
		public bool IsSimulationActive
		{
			get
			{
				return this._battleSimulation != null;
			}
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x000C21AC File Offset: 0x000C03AC
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnIdleTick(dt);
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x000C21C6 File Offset: 0x000C03C6
		private void RefreshHandler()
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRefreshState();
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000C21D8 File Offset: 0x000C03D8
		public void OnJoinArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000C21E0 File Offset: 0x000C03E0
		public void OnLeaveArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x000C21E8 File Offset: 0x000C03E8
		public void OnDispersePlayerLeadedArmy()
		{
			this.RefreshHandler();
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000C21F0 File Offset: 0x000C03F0
		public void OnArmyCreated(MobileParty mobileParty)
		{
			this.RefreshHandler();
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000C21F8 File Offset: 0x000C03F8
		public void OnMainPartyEncounter()
		{
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMainPartyEncounter();
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000C220A File Offset: 0x000C040A
		public void ProcessTravel(Vec2 point)
		{
			MobileParty.MainParty.Ai.ForceAiNoPathMode = false;
			MobileParty.MainParty.Ai.SetMoveGoToPoint(point);
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000C222C File Offset: 0x000C042C
		public void ProcessTravel(PartyBase party)
		{
			if (party.IsMobile)
			{
				MobileParty.MainParty.Ai.SetMoveEngageParty(party.MobileParty);
			}
			else
			{
				MobileParty.MainParty.Ai.SetMoveGoToSettlement(party.Settlement);
			}
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.StoppablePlay;
			MobileParty.MainParty.Ai.ForceAiNoPathMode = false;
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x000C2288 File Offset: 0x000C0488
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (Campaign.Current.SaveHandler.IsSaving)
			{
				Campaign.Current.SaveHandler.SaveTick();
				return;
			}
			if (this._battleSimulation != null)
			{
				this._battleSimulation.Tick(dt);
			}
			else if (this.AtMenu)
			{
				this.OnMenuModeTick(dt);
			}
			this.OnMapModeTick(dt);
			if (!Campaign.Current.SaveHandler.IsSaving)
			{
				Campaign.Current.SaveHandler.CampaignTick();
			}
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x000C2309 File Offset: 0x000C0509
		private void OnMenuModeTick(float dt)
		{
			this.MenuContext.OnTick(dt);
			IMapStateHandler handler = this.Handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMenuModeTick(dt);
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x000C2328 File Offset: 0x000C0528
		private void OnMapModeTick(float dt)
		{
			if (this._closeScreenNextFrame)
			{
				Game.Current.GameStateManager.CleanStates(0);
				return;
			}
			if (this.Handler != null)
			{
				this.Handler.BeforeTick(dt);
			}
			if (Campaign.Current != null && base.GameStateManager.ActiveState == this)
			{
				Campaign.Current.RealTick(dt);
				IMapStateHandler handler = this.Handler;
				if (handler != null)
				{
					handler.Tick(dt);
				}
				IMapStateHandler handler2 = this.Handler;
				if (handler2 != null)
				{
					handler2.AfterTick(dt);
				}
				Campaign.Current.Tick();
				IMapStateHandler handler3 = this.Handler;
				if (handler3 == null)
				{
					return;
				}
				handler3.AfterWaitTick(dt);
			}
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000C23C4 File Offset: 0x000C05C4
		public void OnLoadingFinished()
		{
			if (!string.IsNullOrEmpty(this.GameMenuId))
			{
				this.EnterMenuMode();
			}
			this.RefreshHandler();
			if (Campaign.Current.CurrentMenuContext != null && Campaign.Current.CurrentMenuContext.GameMenu != null && Campaign.Current.CurrentMenuContext.GameMenu.IsWaitMenu)
			{
				Campaign.Current.CurrentMenuContext.GameMenu.StartWait();
			}
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000C243C File Offset: 0x000C063C
		public void OnMapConversationStarts(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			this._mapConversationActive = true;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMapConversationStarts(playerCharacterData, conversationPartnerData);
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000C2458 File Offset: 0x000C0658
		public void OnMapConversationOver()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnMapConversationOver();
			}
			this._mapConversationActive = false;
			if (Game.Current.GameStateManager.ActiveState is MapState)
			{
				MenuContext menuContext = this.MenuContext;
				if (menuContext != null)
				{
					menuContext.Refresh();
				}
			}
			this.RefreshHandler();
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000C24AA File Offset: 0x000C06AA
		internal void OnSignalPeriodicEvents()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSignalPeriodicEvents();
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x000C24BC File Offset: 0x000C06BC
		internal void OnHourlyTick()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnHourlyTick();
			}
			MenuContext menuContext = this.MenuContext;
			if (menuContext == null)
			{
				return;
			}
			menuContext.OnHourlyTick();
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x000C24DF File Offset: 0x000C06DF
		protected override void OnActivate()
		{
			base.OnActivate();
			MenuContext menuContext = this.MenuContext;
			if (menuContext != null)
			{
				menuContext.Refresh();
			}
			this.RefreshHandler();
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x000C24FE File Offset: 0x000C06FE
		public void EnterMenuMode()
		{
			this.MenuContext = MBObjectManager.Instance.CreateObject<MenuContext>();
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnEnteringMenuMode(this.MenuContext);
			}
			this.MenuContext.Refresh();
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000C2532 File Offset: 0x000C0732
		public void ExitMenuMode()
		{
			IMapStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnExitingMenuMode();
			}
			this.MenuContext.Destroy();
			MBObjectManager.Instance.UnregisterObject(this.MenuContext);
			this.MenuContext = null;
			this.GameMenuId = null;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000C256E File Offset: 0x000C076E
		public void StartBattleSimulation()
		{
			this._battleSimulation = PlayerEncounter.Current.BattleSimulation;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleSimulationStarted(this._battleSimulation);
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000C2596 File Offset: 0x000C0796
		public void EndBattleSimulation()
		{
			this._battleSimulation = null;
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleSimulationEnded();
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000C25AF File Offset: 0x000C07AF
		public void OnPlayerSiegeActivated()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSiegeActivated();
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x000C25C1 File Offset: 0x000C07C1
		public void OnPlayerSiegeDeactivated()
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSiegeDeactivated();
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x000C25D3 File Offset: 0x000C07D3
		public void OnSiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
			IMapStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSiegeEngineClick(siegeEngineFrame);
		}

		// Token: 0x04000DF9 RID: 3577
		private MenuContext _menuContext;

		// Token: 0x04000DFA RID: 3578
		private bool _mapConversationActive;

		// Token: 0x04000DFB RID: 3579
		private bool _closeScreenNextFrame;

		// Token: 0x04000DFC RID: 3580
		private IMapStateHandler _handler;

		// Token: 0x04000DFD RID: 3581
		private BattleSimulation _battleSimulation;
	}
}
