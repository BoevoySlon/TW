using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AC RID: 428
	public abstract class MarriageModel : GameModel
	{
		// Token: 0x06001B09 RID: 6921
		public abstract bool IsCoupleSuitableForMarriage(Hero firstHero, Hero secondHero);

		// Token: 0x06001B0A RID: 6922
		public abstract int GetEffectiveRelationIncrease(Hero firstHero, Hero secondHero);

		// Token: 0x06001B0B RID: 6923
		public abstract Clan GetClanAfterMarriage(Hero firstHero, Hero secondHero);

		// Token: 0x06001B0C RID: 6924
		public abstract bool IsSuitableForMarriage(Hero hero);

		// Token: 0x06001B0D RID: 6925
		public abstract bool IsClanSuitableForMarriage(Clan clan);

		// Token: 0x06001B0E RID: 6926
		public abstract float NpcCoupleMarriageChance(Hero firstHero, Hero secondHero);

		// Token: 0x06001B0F RID: 6927
		public abstract bool ShouldNpcMarriageBetweenClansBeAllowed(Clan consideringClan, Clan targetClan);

		// Token: 0x06001B10 RID: 6928
		public abstract List<Hero> GetAdultChildrenSuitableForMarriage(Hero hero);

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001B11 RID: 6929
		public abstract int MinimumMarriageAgeMale { get; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001B12 RID: 6930
		public abstract int MinimumMarriageAgeFemale { get; }
	}
}
