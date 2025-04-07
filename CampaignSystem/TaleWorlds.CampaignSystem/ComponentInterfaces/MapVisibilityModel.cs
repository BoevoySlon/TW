using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200016C RID: 364
	public abstract class MapVisibilityModel : GameModel
	{
		// Token: 0x06001928 RID: 6440
		public abstract ExplainedNumber GetPartySpottingRange(MobileParty party, bool includeDescriptions = false);

		// Token: 0x06001929 RID: 6441
		public abstract float GetPartyRelativeInspectionRange(IMapPoint party);

		// Token: 0x0600192A RID: 6442
		public abstract float GetPartySpottingDifficulty(MobileParty spotterParty, MobileParty party);

		// Token: 0x0600192B RID: 6443
		public abstract float GetHideoutSpottingDistance();
	}
}
