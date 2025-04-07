using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x020000C8 RID: 200
	public struct LocatableSearchData<T>
	{
		// Token: 0x060012B7 RID: 4791 RVA: 0x0005581C File Offset: 0x00053A1C
		public LocatableSearchData(Vec2 position, float radius, int minX, int minY, int maxX, int maxY)
		{
			this.Position = position;
			this.RadiusSquared = radius * radius;
			this.MinY = minY;
			this.MaxXInclusive = maxX;
			this.MaxYInclusive = maxY;
			this.CurrentX = minX;
			this.CurrentY = minY - 1;
			this.CurrentLocatable = null;
		}

		// Token: 0x04000661 RID: 1633
		public readonly Vec2 Position;

		// Token: 0x04000662 RID: 1634
		public readonly float RadiusSquared;

		// Token: 0x04000663 RID: 1635
		public readonly int MinY;

		// Token: 0x04000664 RID: 1636
		public readonly int MaxXInclusive;

		// Token: 0x04000665 RID: 1637
		public readonly int MaxYInclusive;

		// Token: 0x04000666 RID: 1638
		public int CurrentX;

		// Token: 0x04000667 RID: 1639
		public int CurrentY;

		// Token: 0x04000668 RID: 1640
		internal ILocatable<T> CurrentLocatable;
	}
}
