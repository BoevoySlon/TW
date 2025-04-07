using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019F RID: 415
	public abstract class SettlementGarrisonModel : GameModel
	{
		// Token: 0x06001ABC RID: 6844
		public abstract ExplainedNumber CalculateGarrisonChange(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001ABD RID: 6845
		public abstract ExplainedNumber CalculateGarrisonChangeAutoRecruitment(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001ABE RID: 6846
		public abstract int FindNumberOfTroopsToTakeFromGarrison(MobileParty mobileParty, Settlement settlement, float idealGarrisonStrengthPerWalledCenter = 0f);

		// Token: 0x06001ABF RID: 6847
		public abstract int FindNumberOfTroopsToLeaveToGarrison(MobileParty mobileParty, Settlement settlement);
	}
}
