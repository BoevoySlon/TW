using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019A RID: 410
	public abstract class DisguiseDetectionModel : GameModel
	{
		// Token: 0x06001A82 RID: 6786
		public abstract float CalculateDisguiseDetectionProbability(Settlement settlement);
	}
}
