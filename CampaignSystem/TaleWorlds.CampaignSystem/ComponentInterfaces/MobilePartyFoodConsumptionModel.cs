using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200017B RID: 379
	public abstract class MobilePartyFoodConsumptionModel : GameModel
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x0600198A RID: 6538
		public abstract int NumberOfMenOnMapToEatOneFood { get; }

		// Token: 0x0600198B RID: 6539
		public abstract ExplainedNumber CalculateDailyBaseFoodConsumptionf(MobileParty party, bool includeDescription = false);

		// Token: 0x0600198C RID: 6540
		public abstract ExplainedNumber CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption);

		// Token: 0x0600198D RID: 6541
		public abstract bool DoesPartyConsumeFood(MobileParty mobileParty);
	}
}
