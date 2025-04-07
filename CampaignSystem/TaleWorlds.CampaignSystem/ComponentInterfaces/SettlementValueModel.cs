using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019B RID: 411
	public abstract class SettlementValueModel : GameModel
	{
		// Token: 0x06001A84 RID: 6788
		public abstract float CalculateSettlementValueForFaction(Settlement settlement, IFaction faction);

		// Token: 0x06001A85 RID: 6789
		public abstract float CalculateSettlementBaseValue(Settlement settlement);

		// Token: 0x06001A86 RID: 6790
		public abstract float CalculateSettlementValueForEnemyHero(Settlement settlement, Hero hero);
	}
}
