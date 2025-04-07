using System;
using TaleWorlds.CampaignSystem.MapEvents;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A2 RID: 162
	public class VisualCreator
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00051D44 File Offset: 0x0004FF44
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x00051D4C File Offset: 0x0004FF4C
		public IMapEventVisualCreator MapEventVisualCreator { get; set; }

		// Token: 0x060011AB RID: 4523 RVA: 0x00051D55 File Offset: 0x0004FF55
		public IMapEventVisual CreateMapEventVisual(MapEvent mapEvent)
		{
			IMapEventVisualCreator mapEventVisualCreator = this.MapEventVisualCreator;
			if (mapEventVisualCreator == null)
			{
				return null;
			}
			return mapEventVisualCreator.CreateMapEventVisual(mapEvent);
		}
	}
}
