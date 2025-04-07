using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000180 RID: 384
	public abstract class EmissaryModel : GameModel
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060019DA RID: 6618
		public abstract int EmissaryRelationBonusForMainClan { get; }

		// Token: 0x060019DB RID: 6619
		public abstract bool IsEmissary(Hero hero);
	}
}
