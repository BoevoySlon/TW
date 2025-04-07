using System;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000078 RID: 120
	public interface IBattleCombatant
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060007A9 RID: 1961
		TextObject Name { get; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060007AA RID: 1962
		BattleSideEnum Side { get; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060007AB RID: 1963
		BasicCultureObject BasicCulture { get; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060007AC RID: 1964
		BasicCharacterObject General { get; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060007AD RID: 1965
		Tuple<uint, uint> PrimaryColorPair { get; }

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060007AE RID: 1966
		Tuple<uint, uint> AlternativeColorPair { get; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060007AF RID: 1967
		Banner Banner { get; }

		// Token: 0x060007B0 RID: 1968
		int GetTacticsSkillAmount();
	}
}
