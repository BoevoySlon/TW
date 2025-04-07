using System;
using TaleWorlds.CampaignSystem.Roster;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000337 RID: 823
	public interface IMenuContextHandler
	{
		// Token: 0x06002EA3 RID: 11939
		void OnBackgroundMeshNameSet(string name);

		// Token: 0x06002EA4 RID: 11940
		void OnOpenTownManagement();

		// Token: 0x06002EA5 RID: 11941
		void OnOpenRecruitVolunteers();

		// Token: 0x06002EA6 RID: 11942
		void OnOpenTournamentLeaderboard();

		// Token: 0x06002EA7 RID: 11943
		void OnOpenTroopSelection(TroopRoster fullRoster, TroopRoster initialSelections, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster> onDone, int maxSelectableTroopCount, int minSelectableTroopCount);

		// Token: 0x06002EA8 RID: 11944
		void OnMenuCreate();

		// Token: 0x06002EA9 RID: 11945
		void OnMenuActivate();

		// Token: 0x06002EAA RID: 11946
		void OnHourlyTick();

		// Token: 0x06002EAB RID: 11947
		void OnPanelSoundIDSet(string panelSoundID);

		// Token: 0x06002EAC RID: 11948
		void OnAmbientSoundIDSet(string ambientSoundID);
	}
}
