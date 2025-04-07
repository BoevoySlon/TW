using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200003B RID: 59
	public interface IMbEvent<out T> : IMbEventBase
	{
		// Token: 0x0600074E RID: 1870
		void AddNonSerializedListener(object owner, Action<T> action);
	}
}
