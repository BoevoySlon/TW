using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B9 RID: 441
	public abstract class CompanionHiringPriceCalculationModel : GameModel
	{
		// Token: 0x06001B6C RID: 7020
		public abstract int GetCompanionHiringPrice(Hero companion);
	}
}
