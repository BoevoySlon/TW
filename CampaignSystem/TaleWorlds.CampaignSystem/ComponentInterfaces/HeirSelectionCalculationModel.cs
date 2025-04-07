using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A6 RID: 422
	public abstract class HeirSelectionCalculationModel : GameModel
	{
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06001AF4 RID: 6900
		public abstract int HighestSkillPoint { get; }

		// Token: 0x06001AF5 RID: 6901
		public abstract int CalculateHeirSelectionPoint(Hero candidateHeir, Hero deadHero, ref Hero maxSkillHero);
	}
}
