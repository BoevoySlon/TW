using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200004F RID: 79
	public interface ICampaignMission
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007AB RID: 1963
		GameState State { get; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007AC RID: 1964
		IMissionTroopSupplier AgentSupplier { get; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007AD RID: 1965
		// (set) Token: 0x060007AE RID: 1966
		Location Location { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007AF RID: 1967
		// (set) Token: 0x060007B0 RID: 1968
		Alley LastVisitedAlley { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007B1 RID: 1969
		MissionMode Mode { get; }

		// Token: 0x060007B2 RID: 1970
		void SetMissionMode(MissionMode newMode, bool atStart);

		// Token: 0x060007B3 RID: 1971
		void OnCloseEncounterMenu();

		// Token: 0x060007B4 RID: 1972
		bool AgentLookingAtAgent(IAgent agent1, IAgent agent2);

		// Token: 0x060007B5 RID: 1973
		void OnCharacterLocationChanged(LocationCharacter locationCharacter, Location fromLocation, Location toLocation);

		// Token: 0x060007B6 RID: 1974
		void OnProcessSentence();

		// Token: 0x060007B7 RID: 1975
		void OnConversationContinue();

		// Token: 0x060007B8 RID: 1976
		bool CheckIfAgentCanFollow(IAgent agent);

		// Token: 0x060007B9 RID: 1977
		void AddAgentFollowing(IAgent agent);

		// Token: 0x060007BA RID: 1978
		bool CheckIfAgentCanUnFollow(IAgent agent);

		// Token: 0x060007BB RID: 1979
		void RemoveAgentFollowing(IAgent agent);

		// Token: 0x060007BC RID: 1980
		void OnConversationPlay(string idleActionId, string idleFaceAnimId, string reactionId, string reactionFaceAnimId, string soundPath);

		// Token: 0x060007BD RID: 1981
		void OnConversationStart(IAgent agent, bool setActionsInstantly);

		// Token: 0x060007BE RID: 1982
		void OnConversationEnd(IAgent agent);

		// Token: 0x060007BF RID: 1983
		void EndMission();
	}
}
