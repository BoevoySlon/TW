using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000079 RID: 121
	public interface IBattleObserver
	{
		// Token: 0x060007B1 RID: 1969
		void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberKilled = 0, int numberWounded = 0, int numberRouted = 0, int killCount = 0, int numberReadyToUpgrade = 0);

		// Token: 0x060007B2 RID: 1970
		void TroopSideChanged(BattleSideEnum prevSide, BattleSideEnum newSide, IBattleCombatant battleCombatant, BasicCharacterObject character);

		// Token: 0x060007B3 RID: 1971
		void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject skill);

		// Token: 0x060007B4 RID: 1972
		void BattleResultsReady();
	}
}
