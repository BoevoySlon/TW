using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200016A RID: 362
	public abstract class LocationModel : GameModel
	{
		// Token: 0x0600191F RID: 6431
		public abstract int GetSettlementUpgradeLevel(LocationEncounter locationEncounter);

		// Token: 0x06001920 RID: 6432
		public abstract string GetCivilianSceneLevel(Settlement settlement);

		// Token: 0x06001921 RID: 6433
		public abstract string GetCivilianUpgradeLevelTag(int upgradeLevel);

		// Token: 0x06001922 RID: 6434
		public abstract string GetUpgradeLevelTag(int upgradeLevel);
	}
}
