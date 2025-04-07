using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018E RID: 398
	public abstract class MapTrackModel : GameModel
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001A3D RID: 6717
		public abstract float MaxTrackLife { get; }

		// Token: 0x06001A3E RID: 6718
		public abstract float GetSkipTrackChance(MobileParty mobileParty);

		// Token: 0x06001A3F RID: 6719
		public abstract float GetMaxTrackSpottingDistanceForMainParty();

		// Token: 0x06001A40 RID: 6720
		public abstract bool CanPartyLeaveTrack(MobileParty mobileParty);

		// Token: 0x06001A41 RID: 6721
		public abstract float GetTrackDetectionDifficultyForMainParty(Track track, float trackSpottingDistance);

		// Token: 0x06001A42 RID: 6722
		public abstract float GetSkillFromTrackDetected(Track track);

		// Token: 0x06001A43 RID: 6723
		public abstract int GetTrackLife(MobileParty mobileParty);

		// Token: 0x06001A44 RID: 6724
		public abstract TextObject TrackTitle(Track track);

		// Token: 0x06001A45 RID: 6725
		public abstract IEnumerable<ValueTuple<TextObject, string>> GetTrackDescription(Track track);

		// Token: 0x06001A46 RID: 6726
		public abstract uint GetTrackColor(Track track);

		// Token: 0x06001A47 RID: 6727
		public abstract float GetTrackScale(Track track);
	}
}
