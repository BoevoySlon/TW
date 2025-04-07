using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000458 RID: 1112
	public static class SiegeAftermathAction
	{
		// Token: 0x06004119 RID: 16665 RVA: 0x001419F7 File Offset: 0x0013FBF7
		private static void ApplyInternal(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			CampaignEventDispatcher.Instance.OnSiegeAftermathApplied(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x00141A09 File Offset: 0x0013FC09
		public static void ApplyAftermath(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			SiegeAftermathAction.ApplyInternal(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
		}

		// Token: 0x0200077C RID: 1916
		public enum SiegeAftermath
		{
			// Token: 0x04001F55 RID: 8021
			Devastate,
			// Token: 0x04001F56 RID: 8022
			Pillage,
			// Token: 0x04001F57 RID: 8023
			ShowMercy
		}
	}
}
