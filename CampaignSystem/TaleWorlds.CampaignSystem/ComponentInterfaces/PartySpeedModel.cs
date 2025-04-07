using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200016F RID: 367
	public abstract class PartySpeedModel : GameModel
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x0600193C RID: 6460
		public abstract float BaseSpeed { get; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600193D RID: 6461
		public abstract float MinimumSpeed { get; }

		// Token: 0x0600193E RID: 6462
		public abstract ExplainedNumber CalculateBaseSpeed(MobileParty party, bool includeDescriptions = false, int additionalTroopOnFootCount = 0, int additionalTroopOnHorseCount = 0);

		// Token: 0x0600193F RID: 6463
		public abstract ExplainedNumber CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber finalSpeed);
	}
}
