using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x020000C4 RID: 196
	public interface IMapPoint
	{
		// Token: 0x06001288 RID: 4744
		void OnGameInitialized();

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001289 RID: 4745
		TextObject Name { get; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x0600128A RID: 4746
		Vec2 Position2D { get; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x0600128B RID: 4747
		PathFaceRecord CurrentNavigationFace { get; }

		// Token: 0x0600128C RID: 4748
		Vec3 GetLogicalPosition();

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x0600128D RID: 4749
		IFaction MapFaction { get; }

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x0600128E RID: 4750
		bool IsInspected { get; }

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x0600128F RID: 4751
		bool IsVisible { get; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001290 RID: 4752
		// (set) Token: 0x06001291 RID: 4753
		bool IsActive { get; set; }
	}
}
