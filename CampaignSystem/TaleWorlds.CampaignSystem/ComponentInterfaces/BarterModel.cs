using System;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000173 RID: 371
	public abstract class BarterModel : GameModel
	{
		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06001954 RID: 6484
		public abstract int BarterCooldownWithHeroInDays { get; }

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06001955 RID: 6485
		public abstract float MaximumPercentageOfNpcGoldToSpendAtBarter { get; }

		// Token: 0x06001956 RID: 6486
		public abstract int CalculateOverpayRelationIncreaseCosts(Hero hero, float overpayAmount);

		// Token: 0x06001957 RID: 6487
		public abstract ExplainedNumber GetBarterPenalty(IFaction faction, ItemBarterable itemBarterable, Hero otherHero, PartyBase otherParty);
	}
}
