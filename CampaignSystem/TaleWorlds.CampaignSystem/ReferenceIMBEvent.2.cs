using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003F RID: 63
	public interface ReferenceIMBEvent<T1, T2, T3> : IMbEventBase
	{
		// Token: 0x0600075C RID: 1884
		void AddNonSerializedListener(object owner, ReferenceAction<T1, T2, T3> action);
	}
}
