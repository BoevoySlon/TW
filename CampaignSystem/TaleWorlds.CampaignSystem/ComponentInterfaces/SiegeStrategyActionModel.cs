using System;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B5 RID: 437
	public abstract class SiegeStrategyActionModel : GameModel
	{
		// Token: 0x06001B47 RID: 6983
		public abstract void GetLogicalActionForStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex);

		// Token: 0x0200055D RID: 1373
		public enum SiegeAction
		{
			// Token: 0x040016AA RID: 5802
			ConstructNewSiegeEngine,
			// Token: 0x040016AB RID: 5803
			DeploySiegeEngineFromReserve,
			// Token: 0x040016AC RID: 5804
			MoveSiegeEngineToReserve,
			// Token: 0x040016AD RID: 5805
			RemoveDeployedSiegeEngine,
			// Token: 0x040016AE RID: 5806
			Hold
		}
	}
}
