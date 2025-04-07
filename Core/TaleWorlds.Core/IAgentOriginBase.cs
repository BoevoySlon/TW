using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000077 RID: 119
	public interface IAgentOriginBase
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600079B RID: 1947
		bool IsUnderPlayersCommand { get; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600079C RID: 1948
		uint FactionColor { get; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600079D RID: 1949
		uint FactionColor2 { get; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x0600079E RID: 1950
		IBattleCombatant BattleCombatant { get; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600079F RID: 1951
		int UniqueSeed { get; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060007A0 RID: 1952
		int Seed { get; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060007A1 RID: 1953
		Banner Banner { get; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060007A2 RID: 1954
		BasicCharacterObject Troop { get; }

		// Token: 0x060007A3 RID: 1955
		void SetWounded();

		// Token: 0x060007A4 RID: 1956
		void SetKilled();

		// Token: 0x060007A5 RID: 1957
		void SetRouted();

		// Token: 0x060007A6 RID: 1958
		void OnAgentRemoved(float agentHealth);

		// Token: 0x060007A7 RID: 1959
		void OnScoreHit(BasicCharacterObject victim, BasicCharacterObject formationCaptain, int damage, bool isFatal, bool isTeamKill, WeaponComponentData attackerWeapon);

		// Token: 0x060007A8 RID: 1960
		void SetBanner(Banner banner);
	}
}
