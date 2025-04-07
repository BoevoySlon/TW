using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x0200033D RID: 829
	public interface IMapStateHandler
	{
		// Token: 0x06002EF0 RID: 12016
		void OnRefreshState();

		// Token: 0x06002EF1 RID: 12017
		void OnMainPartyEncounter();

		// Token: 0x06002EF2 RID: 12018
		void BeforeTick(float dt);

		// Token: 0x06002EF3 RID: 12019
		void Tick(float dt);

		// Token: 0x06002EF4 RID: 12020
		void AfterTick(float dt);

		// Token: 0x06002EF5 RID: 12021
		void AfterWaitTick(float dt);

		// Token: 0x06002EF6 RID: 12022
		void OnIdleTick(float dt);

		// Token: 0x06002EF7 RID: 12023
		void OnSignalPeriodicEvents();

		// Token: 0x06002EF8 RID: 12024
		void OnExit();

		// Token: 0x06002EF9 RID: 12025
		void ResetCamera(bool resetDistance, bool teleportToMainParty);

		// Token: 0x06002EFA RID: 12026
		void TeleportCameraToMainParty();

		// Token: 0x06002EFB RID: 12027
		void FastMoveCameraToMainParty();

		// Token: 0x06002EFC RID: 12028
		bool IsCameraLockedToPlayerParty();

		// Token: 0x06002EFD RID: 12029
		void StartCameraAnimation(Vec2 targetPosition, float animationStopDuration);

		// Token: 0x06002EFE RID: 12030
		void OnHourlyTick();

		// Token: 0x06002EFF RID: 12031
		void OnMenuModeTick(float dt);

		// Token: 0x06002F00 RID: 12032
		void OnEnteringMenuMode(MenuContext menuContext);

		// Token: 0x06002F01 RID: 12033
		void OnExitingMenuMode();

		// Token: 0x06002F02 RID: 12034
		void OnBattleSimulationStarted(BattleSimulation battleSimulation);

		// Token: 0x06002F03 RID: 12035
		void OnBattleSimulationEnded();

		// Token: 0x06002F04 RID: 12036
		void OnGameplayCheatsEnabled();

		// Token: 0x06002F05 RID: 12037
		void OnMapConversationStarts(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData);

		// Token: 0x06002F06 RID: 12038
		void OnMapConversationOver();

		// Token: 0x06002F07 RID: 12039
		void OnPlayerSiegeActivated();

		// Token: 0x06002F08 RID: 12040
		void OnPlayerSiegeDeactivated();

		// Token: 0x06002F09 RID: 12041
		void OnSiegeEngineClick(MatrixFrame siegeEngineFrame);
	}
}
