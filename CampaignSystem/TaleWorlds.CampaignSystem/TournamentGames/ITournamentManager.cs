using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x0200027A RID: 634
	public interface ITournamentManager
	{
		// Token: 0x060021DB RID: 8667
		void AddTournament(TournamentGame game);

		// Token: 0x060021DC RID: 8668
		TournamentGame GetTournamentGame(Town town);

		// Token: 0x060021DD RID: 8669
		void OnPlayerJoinMatch(Type gameType);

		// Token: 0x060021DE RID: 8670
		void OnPlayerJoinTournament(Type gameType, Settlement settlement);

		// Token: 0x060021DF RID: 8671
		void OnPlayerWatchTournament(Type gameType, Settlement settlement);

		// Token: 0x060021E0 RID: 8672
		void OnPlayerWinMatch(Type gameType);

		// Token: 0x060021E1 RID: 8673
		void OnPlayerWinTournament(Type gameType);

		// Token: 0x060021E2 RID: 8674
		void InitializeLeaderboardEntry(Hero hero, int initialVictories = 0);

		// Token: 0x060021E3 RID: 8675
		void AddLeaderboardEntry(Hero hero);

		// Token: 0x060021E4 RID: 8676
		void GivePrizeToWinner(TournamentGame tournament, Hero winner, bool isPlayerParticipated);

		// Token: 0x060021E5 RID: 8677
		void DeleteLeaderboardEntry(Hero hero);

		// Token: 0x060021E6 RID: 8678
		List<KeyValuePair<Hero, int>> GetLeaderboard();

		// Token: 0x060021E7 RID: 8679
		int GetLeaderBoardRank(Hero hero);

		// Token: 0x060021E8 RID: 8680
		Hero GetLeaderBoardLeader();

		// Token: 0x060021E9 RID: 8681
		void ResolveTournament(TournamentGame tournament, Town town);
	}
}
