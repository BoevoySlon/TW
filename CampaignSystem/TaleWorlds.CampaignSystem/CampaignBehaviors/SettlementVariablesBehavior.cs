using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D4 RID: 980
	public class SettlementVariablesBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003C6E RID: 15470 RVA: 0x00124704 File Offset: 0x00122904
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x0012471D File Offset: 0x0012291D
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x00124720 File Offset: 0x00122920
		private void HourlyTickSettlement(Settlement settlement)
		{
			if ((settlement.IsVillage || settlement.IsFortification || settlement.IsHideout) && settlement.LastAttackerParty != null && settlement.LastThreatTime.ElapsedHoursUntilNow > 24f)
			{
				settlement.LastAttackerParty = null;
			}
		}
	}
}
