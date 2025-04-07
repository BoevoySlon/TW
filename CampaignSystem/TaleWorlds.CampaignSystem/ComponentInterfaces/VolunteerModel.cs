using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000186 RID: 390
	public abstract class VolunteerModel : GameModel
	{
		// Token: 0x060019FB RID: 6651
		public abstract int MaximumIndexHeroCanRecruitFromHero(Hero buyerHero, Hero sellerHero, int useValueAsRelation = -101);

		// Token: 0x060019FC RID: 6652
		public abstract float GetDailyVolunteerProductionProbability(Hero hero, int index, Settlement settlement);

		// Token: 0x060019FD RID: 6653
		public abstract CharacterObject GetBasicVolunteer(Hero hero);

		// Token: 0x060019FE RID: 6654
		public abstract bool CanHaveRecruits(Hero hero);

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060019FF RID: 6655
		public abstract int MaxVolunteerTier { get; }
	}
}
