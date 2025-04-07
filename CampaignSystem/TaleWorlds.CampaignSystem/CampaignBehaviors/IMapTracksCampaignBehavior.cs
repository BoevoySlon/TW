using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200039D RID: 925
	public interface IMapTracksCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06003793 RID: 14227
		MBReadOnlyList<Track> DetectedTracks { get; }

		// Token: 0x06003794 RID: 14228
		void AddTrack(MobileParty target, Vec2 trackPosition, Vec2 trackDirection);

		// Token: 0x06003795 RID: 14229
		void AddMapArrow(TextObject pointerName, Vec2 trackPosition, Vec2 trackDirection, float life);
	}
}
