using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200044C RID: 1100
	public static class LiftSiegeAction
	{
		// Token: 0x060040F0 RID: 16624 RVA: 0x00140947 File Offset: 0x0013EB47
		private static void ApplyInternal(MobileParty side1Party, Settlement settlement)
		{
			settlement.SiegeEvent.BesiegerCamp.RemoveAllSiegeParties();
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00140959 File Offset: 0x0013EB59
		public static void GetGameAction(MobileParty side1Party)
		{
			LiftSiegeAction.ApplyInternal(side1Party, side1Party.BesiegedSettlement);
		}
	}
}
