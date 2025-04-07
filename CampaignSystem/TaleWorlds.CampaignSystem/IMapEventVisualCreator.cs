using System;
using TaleWorlds.CampaignSystem.MapEvents;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A3 RID: 163
	public interface IMapEventVisualCreator
	{
		// Token: 0x060011AD RID: 4525
		IMapEventVisual CreateMapEventVisual(MapEvent mapEvent);
	}
}
