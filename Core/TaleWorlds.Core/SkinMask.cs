using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000BE RID: 190
	[Flags]
	public enum SkinMask
	{
		// Token: 0x04000589 RID: 1417
		NoneVisible = 0,
		// Token: 0x0400058A RID: 1418
		HeadVisible = 1,
		// Token: 0x0400058B RID: 1419
		BodyVisible = 32,
		// Token: 0x0400058C RID: 1420
		UnderwearVisible = 64,
		// Token: 0x0400058D RID: 1421
		HandsVisible = 128,
		// Token: 0x0400058E RID: 1422
		LegsVisible = 256,
		// Token: 0x0400058F RID: 1423
		AllVisible = 481
	}
}
