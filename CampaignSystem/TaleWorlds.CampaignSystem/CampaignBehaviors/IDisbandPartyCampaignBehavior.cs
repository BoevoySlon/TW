using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200039A RID: 922
	public interface IDisbandPartyCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x0600378C RID: 14220
		bool IsPartyWaitingForDisband(MobileParty party);
	}
}
