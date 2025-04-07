using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000041 RID: 65
	public interface IMbEvent<out T1, out T2> : IMbEventBase
	{
		// Token: 0x06000763 RID: 1891
		void AddNonSerializedListener(object owner, Action<T1, T2> action);
	}
}
