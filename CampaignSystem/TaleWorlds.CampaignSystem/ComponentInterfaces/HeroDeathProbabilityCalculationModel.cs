using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A7 RID: 423
	public abstract class HeroDeathProbabilityCalculationModel : GameModel
	{
		// Token: 0x06001AF7 RID: 6903
		public abstract float CalculateHeroDeathProbability(Hero hero);
	}
}
