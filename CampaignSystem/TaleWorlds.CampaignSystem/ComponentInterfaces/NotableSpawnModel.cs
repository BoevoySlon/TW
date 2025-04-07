using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000189 RID: 393
	public abstract class NotableSpawnModel : GameModel
	{
		// Token: 0x06001A0F RID: 6671
		public abstract int GetTargetNotableCountForSettlement(Settlement settlement, Occupation occupation);
	}
}
