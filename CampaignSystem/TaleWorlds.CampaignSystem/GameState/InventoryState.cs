using System;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000338 RID: 824
	public class InventoryState : PlayerGameState
	{
		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x000C201E File Offset: 0x000C021E
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x000C2021 File Offset: 0x000C0221
		// (set) Token: 0x06002EAF RID: 11951 RVA: 0x000C2029 File Offset: 0x000C0229
		public InventoryLogic InventoryLogic { get; private set; }

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06002EB0 RID: 11952 RVA: 0x000C2032 File Offset: 0x000C0232
		// (set) Token: 0x06002EB1 RID: 11953 RVA: 0x000C203A File Offset: 0x000C023A
		public IInventoryStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000C2043 File Offset: 0x000C0243
		public void InitializeLogic(InventoryLogic inventoryLogic)
		{
			this.InventoryLogic = inventoryLogic;
		}

		// Token: 0x04000DF1 RID: 3569
		private IInventoryStateHandler _handler;
	}
}
