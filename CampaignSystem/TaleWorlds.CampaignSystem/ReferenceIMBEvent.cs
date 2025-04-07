using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003D RID: 61
	public interface ReferenceIMBEvent<T1, T2> : IMbEventBase
	{
		// Token: 0x06000755 RID: 1877
		void AddNonSerializedListener(object owner, ReferenceAction<T1, T2> action);
	}
}
