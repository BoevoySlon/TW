using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200016D RID: 365
	public abstract class InformationRestrictionModel : GameModel
	{
		// Token: 0x0600192D RID: 6445
		public abstract bool DoesPlayerKnowDetailsOf(Settlement settlement);

		// Token: 0x0600192E RID: 6446
		public abstract bool DoesPlayerKnowDetailsOf(Hero hero);
	}
}
