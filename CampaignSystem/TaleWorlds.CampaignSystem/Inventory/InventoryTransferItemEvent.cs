using System;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D2 RID: 210
	public class InventoryTransferItemEvent : EventBase
	{
		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x000579AE File Offset: 0x00055BAE
		// (set) Token: 0x06001359 RID: 4953 RVA: 0x000579B6 File Offset: 0x00055BB6
		public ItemObject Item { get; private set; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x000579BF File Offset: 0x00055BBF
		// (set) Token: 0x0600135B RID: 4955 RVA: 0x000579C7 File Offset: 0x00055BC7
		public bool IsBuyForPlayer { get; private set; }

		// Token: 0x0600135C RID: 4956 RVA: 0x000579D0 File Offset: 0x00055BD0
		public InventoryTransferItemEvent(ItemObject item, bool isBuyForPlayer)
		{
			this.Item = item;
			this.IsBuyForPlayer = isBuyForPlayer;
		}
	}
}
