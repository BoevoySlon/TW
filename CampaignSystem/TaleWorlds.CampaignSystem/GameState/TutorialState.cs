using System;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000344 RID: 836
	public class TutorialState : GameState
	{
		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x000C26B8 File Offset: 0x000C08B8
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000C26DE File Offset: 0x000C08DE
		protected override void OnActivate()
		{
			base.OnActivate();
			this.MenuContext.Refresh();
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000C26F1 File Offset: 0x000C08F1
		protected override void OnFinalize()
		{
			this.MenuContext.Destroy();
			this._objectManager.UnregisterObject(this.MenuContext);
			this.MenuContext = null;
			base.OnFinalize();
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000C271C File Offset: 0x000C091C
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.MenuContext.OnTick(dt);
		}

		// Token: 0x04000E04 RID: 3588
		private MBObjectManager _objectManager = MBObjectManager.Instance;

		// Token: 0x04000E05 RID: 3589
		public MenuContext MenuContext = MBObjectManager.Instance.CreateObject<MenuContext>();
	}
}
