using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000183 RID: 387
	public abstract class PartyImpairmentModel : GameModel
	{
		// Token: 0x060019E9 RID: 6633
		public abstract float GetDisorganizedStateDuration(MobileParty party);

		// Token: 0x060019EA RID: 6634
		public abstract float GetVulnerabilityStateDuration(PartyBase party);

		// Token: 0x060019EB RID: 6635
		public abstract float GetSiegeExpectedVulnerabilityTime();

		// Token: 0x060019EC RID: 6636
		public abstract bool CanGetDisorganized(PartyBase partyBase);
	}
}
