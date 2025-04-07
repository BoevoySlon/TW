using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000111 RID: 273
	public class DefaultKingdomDecisionPermissionModel : KingdomDecisionPermissionModel
	{
		// Token: 0x06001603 RID: 5635 RVA: 0x000690F3 File Offset: 0x000672F3
		public override bool IsPolicyDecisionAllowed(PolicyObject policy)
		{
			return true;
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x000690F6 File Offset: 0x000672F6
		public override bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = TextObject.Empty;
			return true;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00069100 File Offset: 0x00067300
		public override bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason)
		{
			reason = TextObject.Empty;
			return true;
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0006910A File Offset: 0x0006730A
		public override bool IsAnnexationDecisionAllowed(Settlement annexedSettlement)
		{
			return true;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0006910D File Offset: 0x0006730D
		public override bool IsExpulsionDecisionAllowed(Clan expelledClan)
		{
			return true;
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00069110 File Offset: 0x00067310
		public override bool IsKingSelectionDecisionAllowed(Kingdom kingdom)
		{
			return true;
		}
	}
}
