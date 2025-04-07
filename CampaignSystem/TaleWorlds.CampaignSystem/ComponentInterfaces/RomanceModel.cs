using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000187 RID: 391
	public abstract class RomanceModel : GameModel
	{
		// Token: 0x06001A01 RID: 6657
		public abstract int GetAttractionValuePercentage(Hero potentiallyInterestedCharacter, Hero heroOfInterest);
	}
}
