using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000CD RID: 205
	public abstract class InventoryListener
	{
		// Token: 0x060012CB RID: 4811
		public abstract int GetGold();

		// Token: 0x060012CC RID: 4812
		public abstract TextObject GetTraderName();

		// Token: 0x060012CD RID: 4813
		public abstract void SetGold(int gold);

		// Token: 0x060012CE RID: 4814
		public abstract PartyBase GetOppositeParty();

		// Token: 0x060012CF RID: 4815
		public abstract void OnTransaction();
	}
}
