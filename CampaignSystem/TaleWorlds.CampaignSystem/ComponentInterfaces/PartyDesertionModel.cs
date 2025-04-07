using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000193 RID: 403
	public abstract class PartyDesertionModel : GameModel
	{
		// Token: 0x06001A5A RID: 6746
		public abstract int GetNumberOfDeserters(MobileParty mobileParty);

		// Token: 0x06001A5B RID: 6747
		public abstract int GetMoraleThresholdForTroopDesertion(MobileParty mobileParty);
	}
}
