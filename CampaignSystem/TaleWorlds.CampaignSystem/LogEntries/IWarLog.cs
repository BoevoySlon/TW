using System;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002F5 RID: 757
	public interface IWarLog
	{
		// Token: 0x06002BEC RID: 11244
		bool IsRelatedToWar(StanceLink stance, out IFaction effector, out IFaction effected);
	}
}
