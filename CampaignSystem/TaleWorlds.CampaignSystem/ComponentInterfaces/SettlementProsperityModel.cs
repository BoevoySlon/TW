using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019E RID: 414
	public abstract class SettlementProsperityModel : GameModel
	{
		// Token: 0x06001AB9 RID: 6841
		public abstract ExplainedNumber CalculateProsperityChange(Town fortification, bool includeDescriptions = false);

		// Token: 0x06001ABA RID: 6842
		public abstract ExplainedNumber CalculateHearthChange(Village village, bool includeDescriptions = false);
	}
}
