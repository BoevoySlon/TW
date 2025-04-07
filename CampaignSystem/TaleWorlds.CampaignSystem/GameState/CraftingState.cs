using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000331 RID: 817
	public class CraftingState : GameState
	{
		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06002E89 RID: 11913 RVA: 0x000C1EC6 File Offset: 0x000C00C6
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06002E8A RID: 11914 RVA: 0x000C1EC9 File Offset: 0x000C00C9
		// (set) Token: 0x06002E8B RID: 11915 RVA: 0x000C1ED1 File Offset: 0x000C00D1
		public Crafting CraftingLogic { get; private set; }

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06002E8C RID: 11916 RVA: 0x000C1EDA File Offset: 0x000C00DA
		// (set) Token: 0x06002E8D RID: 11917 RVA: 0x000C1EE2 File Offset: 0x000C00E2
		public ICraftingStateHandler Handler
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

		// Token: 0x06002E8E RID: 11918 RVA: 0x000C1EEB File Offset: 0x000C00EB
		public void InitializeLogic(Crafting newCraftingLogic, bool isReplacingWeaponClass = false)
		{
			this.CraftingLogic = newCraftingLogic;
			if (this._handler != null)
			{
				if (isReplacingWeaponClass)
				{
					this._handler.OnCraftingLogicRefreshed();
					return;
				}
				this._handler.OnCraftingLogicInitialized();
			}
		}

		// Token: 0x04000DEB RID: 3563
		private ICraftingStateHandler _handler;
	}
}
