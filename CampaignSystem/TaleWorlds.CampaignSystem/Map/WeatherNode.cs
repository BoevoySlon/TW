using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x020000CB RID: 203
	public class WeatherNode
	{
		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060012C5 RID: 4805 RVA: 0x00055CC8 File Offset: 0x00053EC8
		// (set) Token: 0x060012C6 RID: 4806 RVA: 0x00055CD0 File Offset: 0x00053ED0
		public bool IsVisuallyDirty { get; private set; }

		// Token: 0x060012C7 RID: 4807 RVA: 0x00055CD9 File Offset: 0x00053ED9
		public WeatherNode(Vec2 position)
		{
			this.Position = position;
			this.CurrentWeatherEvent = MapWeatherModel.WeatherEvent.Clear;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00055CEF File Offset: 0x00053EEF
		public void SetVisualDirty()
		{
			this.IsVisuallyDirty = true;
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00055CF8 File Offset: 0x00053EF8
		public void OnVisualUpdated()
		{
			this.IsVisuallyDirty = false;
		}

		// Token: 0x04000672 RID: 1650
		public Vec2 Position;

		// Token: 0x04000674 RID: 1652
		public MapWeatherModel.WeatherEvent CurrentWeatherEvent;
	}
}
