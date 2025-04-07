using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018B RID: 395
	public abstract class RansomValueCalculationModel : GameModel
	{
		// Token: 0x06001A21 RID: 6689
		public abstract int PrisonerRansomValue(CharacterObject prisoner, Hero sellerHero = null);
	}
}
