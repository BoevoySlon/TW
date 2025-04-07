using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x020001D3 RID: 467
	public class FaceGenMount
	{
		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001C07 RID: 7175 RVA: 0x0007F056 File Offset: 0x0007D256
		// (set) Token: 0x06001C08 RID: 7176 RVA: 0x0007F05E File Offset: 0x0007D25E
		public MountCreationKey MountKey { get; private set; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001C09 RID: 7177 RVA: 0x0007F067 File Offset: 0x0007D267
		// (set) Token: 0x06001C0A RID: 7178 RVA: 0x0007F06F File Offset: 0x0007D26F
		public ItemObject HorseItem { get; private set; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001C0B RID: 7179 RVA: 0x0007F078 File Offset: 0x0007D278
		// (set) Token: 0x06001C0C RID: 7180 RVA: 0x0007F080 File Offset: 0x0007D280
		public ItemObject HarnessItem { get; private set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001C0D RID: 7181 RVA: 0x0007F089 File Offset: 0x0007D289
		// (set) Token: 0x06001C0E RID: 7182 RVA: 0x0007F091 File Offset: 0x0007D291
		public string ActionName { get; set; }

		// Token: 0x06001C0F RID: 7183 RVA: 0x0007F09A File Offset: 0x0007D29A
		public FaceGenMount(MountCreationKey mountKey, ItemObject horseItem, ItemObject harnessItem, string actionName = "act_inventory_idle_start")
		{
			this.MountKey = mountKey;
			this.HorseItem = horseItem;
			this.HarnessItem = harnessItem;
			this.ActionName = actionName;
		}
	}
}
