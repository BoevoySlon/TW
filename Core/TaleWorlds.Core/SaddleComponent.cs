using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000B9 RID: 185
	public class SaddleComponent : ItemComponent
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x0001F42C File Offset: 0x0001D62C
		public SaddleComponent(SaddleComponent saddleComponent)
		{
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001F434 File Offset: 0x0001D634
		public override ItemComponent GetCopy()
		{
			return new SaddleComponent(this);
		}
	}
}
