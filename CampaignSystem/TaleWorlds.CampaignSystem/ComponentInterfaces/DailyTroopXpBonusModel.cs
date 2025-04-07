using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AB RID: 427
	public abstract class DailyTroopXpBonusModel : GameModel
	{
		// Token: 0x06001B06 RID: 6918
		public abstract int CalculateDailyTroopXpBonus(Town town);

		// Token: 0x06001B07 RID: 6919
		public abstract float CalculateGarrisonXpBonusMultiplier(Town town);
	}
}
