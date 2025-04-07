using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000437 RID: 1079
	public static class ClaimSettlementAction
	{
		// Token: 0x0600408D RID: 16525 RVA: 0x0013E515 File Offset: 0x0013C715
		private static void ApplyInternal(Hero claimant, Settlement claimedSettlement)
		{
			ClaimSettlementAction.ImpactRelations(claimant, claimedSettlement);
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x0013E520 File Offset: 0x0013C720
		private static void ImpactRelations(Hero claimant, Settlement claimedSettlement)
		{
			if (claimedSettlement.OwnerClan.Leader != null)
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(claimant, claimedSettlement.OwnerClan.Leader, -50, false);
				if (!claimedSettlement.OwnerClan.IsMapFaction)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(claimant, claimedSettlement.OwnerClan.Leader, -20, false);
				}
			}
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x0013E56F File Offset: 0x0013C76F
		public static void Apply(Hero claimant, Settlement claimedSettlement)
		{
			ClaimSettlementAction.ApplyInternal(claimant, claimedSettlement);
		}
	}
}
