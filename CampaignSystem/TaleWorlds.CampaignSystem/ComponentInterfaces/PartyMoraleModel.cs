using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200017C RID: 380
	public abstract class PartyMoraleModel : GameModel
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x0600198F RID: 6543
		public abstract float HighMoraleValue { get; }

		// Token: 0x06001990 RID: 6544
		public abstract int GetDailyStarvationMoralePenalty(PartyBase party);

		// Token: 0x06001991 RID: 6545
		public abstract int GetDailyNoWageMoralePenalty(MobileParty party);

		// Token: 0x06001992 RID: 6546
		public abstract float GetStandardBaseMorale(PartyBase party);

		// Token: 0x06001993 RID: 6547
		public abstract float GetVictoryMoraleChange(PartyBase party);

		// Token: 0x06001994 RID: 6548
		public abstract float GetDefeatMoraleChange(PartyBase party);

		// Token: 0x06001995 RID: 6549
		public abstract ExplainedNumber GetEffectivePartyMorale(MobileParty party, bool includeDescription = false);
	}
}
