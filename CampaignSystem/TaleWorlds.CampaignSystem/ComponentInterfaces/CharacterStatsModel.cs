using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000166 RID: 358
	public abstract class CharacterStatsModel : GameModel
	{
		// Token: 0x060018F0 RID: 6384
		public abstract ExplainedNumber MaxHitpoints(CharacterObject character, bool includeDescriptions = false);

		// Token: 0x060018F1 RID: 6385
		public abstract int GetTier(CharacterObject character);

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060018F2 RID: 6386
		public abstract int MaxCharacterTier { get; }
	}
}
