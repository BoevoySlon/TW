using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200017D RID: 381
	public abstract class KingdomCreationModel : GameModel
	{
		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001997 RID: 6551
		public abstract int MinimumClanTierToCreateKingdom { get; }

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001998 RID: 6552
		public abstract int MinimumNumberOfSettlementsOwnedToCreateKingdom { get; }

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06001999 RID: 6553
		public abstract int MinimumTroopCountToCreateKingdom { get; }

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600199A RID: 6554
		public abstract int MaximumNumberOfInitialPolicies { get; }

		// Token: 0x0600199B RID: 6555
		public abstract bool IsPlayerKingdomCreationPossible(out List<TextObject> explanations);

		// Token: 0x0600199C RID: 6556
		public abstract bool IsPlayerKingdomAbdicationPossible(out List<TextObject> explanations);

		// Token: 0x0600199D RID: 6557
		public abstract IEnumerable<CultureObject> GetAvailablePlayerKingdomCultures();
	}
}
