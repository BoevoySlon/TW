using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x020000C7 RID: 199
	internal interface ILocatable<T>
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060012B2 RID: 4786
		// (set) Token: 0x060012B3 RID: 4787
		[CachedData]
		int LocatorNodeIndex { get; set; }

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060012B4 RID: 4788
		// (set) Token: 0x060012B5 RID: 4789
		[CachedData]
		T NextLocatable { get; set; }

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060012B6 RID: 4790
		[CachedData]
		Vec2 GetPosition2D { get; }
	}
}
