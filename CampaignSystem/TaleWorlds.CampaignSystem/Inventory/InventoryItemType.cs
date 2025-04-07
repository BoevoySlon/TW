using System;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000D6 RID: 214
	[Flags]
	public enum InventoryItemType
	{
		// Token: 0x040006AF RID: 1711
		None = 0,
		// Token: 0x040006B0 RID: 1712
		Weapon = 1,
		// Token: 0x040006B1 RID: 1713
		Shield = 2,
		// Token: 0x040006B2 RID: 1714
		HeadArmor = 4,
		// Token: 0x040006B3 RID: 1715
		BodyArmor = 8,
		// Token: 0x040006B4 RID: 1716
		LegArmor = 16,
		// Token: 0x040006B5 RID: 1717
		HandArmor = 32,
		// Token: 0x040006B6 RID: 1718
		Horse = 64,
		// Token: 0x040006B7 RID: 1719
		HorseHarness = 128,
		// Token: 0x040006B8 RID: 1720
		Goods = 256,
		// Token: 0x040006B9 RID: 1721
		Book = 512,
		// Token: 0x040006BA RID: 1722
		Animal = 1024,
		// Token: 0x040006BB RID: 1723
		Cape = 2048,
		// Token: 0x040006BC RID: 1724
		Banner = 4096,
		// Token: 0x040006BD RID: 1725
		HorseCategory = 192,
		// Token: 0x040006BE RID: 1726
		Armors = 2108,
		// Token: 0x040006BF RID: 1727
		Equipable = 6399,
		// Token: 0x040006C0 RID: 1728
		All = 4095
	}
}
