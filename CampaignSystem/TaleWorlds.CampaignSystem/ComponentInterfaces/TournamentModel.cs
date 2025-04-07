using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B4 RID: 436
	public abstract class TournamentModel : GameModel
	{
		// Token: 0x06001B3D RID: 6973
		public abstract float GetTournamentStartChance(Town town);

		// Token: 0x06001B3E RID: 6974
		public abstract TournamentGame CreateTournament(Town town);

		// Token: 0x06001B3F RID: 6975
		public abstract float GetTournamentEndChance(TournamentGame tournament);

		// Token: 0x06001B40 RID: 6976
		public abstract int GetNumLeaderboardVictoriesAtGameStart();

		// Token: 0x06001B41 RID: 6977
		public abstract float GetTournamentSimulationScore(CharacterObject character);

		// Token: 0x06001B42 RID: 6978
		public abstract int GetRenownReward(Hero winner, Town town);

		// Token: 0x06001B43 RID: 6979
		public abstract int GetInfluenceReward(Hero winner, Town town);

		// Token: 0x06001B44 RID: 6980
		[return: TupleElementNames(new string[]
		{
			"skill",
			"xp"
		})]
		public abstract ValueTuple<SkillObject, int> GetSkillXpGainFromTournament(Town town);

		// Token: 0x06001B45 RID: 6981
		public abstract Equipment GetParticipantArmor(CharacterObject participant);
	}
}
