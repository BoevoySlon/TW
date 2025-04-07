using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200005C RID: 92
	[Flags]
	public enum TroopUsageFlags : ushort
	{
		// Token: 0x0400035E RID: 862
		None = 0,
		// Token: 0x0400035F RID: 863
		OnFoot = 1,
		// Token: 0x04000360 RID: 864
		Mounted = 2,
		// Token: 0x04000361 RID: 865
		Melee = 4,
		// Token: 0x04000362 RID: 866
		Ranged = 8,
		// Token: 0x04000363 RID: 867
		OneHandedUser = 16,
		// Token: 0x04000364 RID: 868
		ShieldUser = 32,
		// Token: 0x04000365 RID: 869
		TwoHandedUser = 64,
		// Token: 0x04000366 RID: 870
		PolearmUser = 128,
		// Token: 0x04000367 RID: 871
		BowUser = 256,
		// Token: 0x04000368 RID: 872
		ThrownUser = 512,
		// Token: 0x04000369 RID: 873
		CrossbowUser = 1024,
		// Token: 0x0400036A RID: 874
		Undefined = 65535
	}
}
