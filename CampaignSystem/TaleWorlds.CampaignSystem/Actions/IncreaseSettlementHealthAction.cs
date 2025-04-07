using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000447 RID: 1095
	public static class IncreaseSettlementHealthAction
	{
		// Token: 0x060040DB RID: 16603 RVA: 0x0013F7F0 File Offset: 0x0013D9F0
		private static void ApplyInternal(Settlement settlement, float percentage)
		{
			settlement.SettlementHitPoints += percentage;
			settlement.SettlementHitPoints = ((settlement.SettlementHitPoints > 1f) ? 1f : settlement.SettlementHitPoints);
			if (settlement.SettlementHitPoints >= 1f && settlement.IsVillage && settlement.Village.VillageState != Village.VillageStates.Normal)
			{
				ChangeVillageStateAction.ApplyBySettingToNormal(settlement);
				settlement.Militia += 20f;
			}
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x0013F865 File Offset: 0x0013DA65
		public static void Apply(Settlement settlement, float percentage)
		{
			IncreaseSettlementHealthAction.ApplyInternal(settlement, percentage);
		}
	}
}
