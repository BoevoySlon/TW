using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000176 RID: 374
	public abstract class BribeCalculationModel : GameModel
	{
		// Token: 0x06001965 RID: 6501
		public abstract int GetBribeToEnterLordsHall(Settlement settlement);

		// Token: 0x06001966 RID: 6502
		public abstract int GetBribeToEnterDungeon(Settlement settlement);

		// Token: 0x06001967 RID: 6503
		public abstract bool IsBribeNotNeededToEnterKeep(Settlement settlement);

		// Token: 0x06001968 RID: 6504
		public abstract bool IsBribeNotNeededToEnterDungeon(Settlement settlement);
	}
}
