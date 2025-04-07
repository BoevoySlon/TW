using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000181 RID: 385
	public abstract class KingdomDecisionPermissionModel : GameModel
	{
		// Token: 0x060019DD RID: 6621
		public abstract bool IsPolicyDecisionAllowed(PolicyObject policy);

		// Token: 0x060019DE RID: 6622
		public abstract bool IsWarDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x060019DF RID: 6623
		public abstract bool IsPeaceDecisionAllowedBetweenKingdoms(Kingdom kingdom1, Kingdom kingdom2, out TextObject reason);

		// Token: 0x060019E0 RID: 6624
		public abstract bool IsAnnexationDecisionAllowed(Settlement annexedSettlement);

		// Token: 0x060019E1 RID: 6625
		public abstract bool IsExpulsionDecisionAllowed(Clan expelledClan);

		// Token: 0x060019E2 RID: 6626
		public abstract bool IsKingSelectionDecisionAllowed(Kingdom kingdom);
	}
}
