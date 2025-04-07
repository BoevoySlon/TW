using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AA RID: 426
	public abstract class WallHitPointCalculationModel : GameModel
	{
		// Token: 0x06001B04 RID: 6916
		public abstract float CalculateMaximumWallHitPoint(Town town);
	}
}
