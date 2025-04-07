using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C1 RID: 449
	public abstract class PrisonBreakModel : GameModel
	{
		// Token: 0x06001BA5 RID: 7077
		public abstract bool CanPlayerStagePrisonBreak(Settlement settlement);

		// Token: 0x06001BA6 RID: 7078
		public abstract int GetPrisonBreakStartCost(Hero prisonerHero);

		// Token: 0x06001BA7 RID: 7079
		public abstract int GetRelationRewardOnPrisonBreak(Hero prisonerHero);

		// Token: 0x06001BA8 RID: 7080
		public abstract float GetRogueryRewardOnPrisonBreak(Hero prisonerHero, bool isSuccess);
	}
}
