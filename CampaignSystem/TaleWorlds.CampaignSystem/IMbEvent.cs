using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000031 RID: 49
	public interface IMbEvent
	{
		// Token: 0x0600035A RID: 858
		void AddNonSerializedListener(object owner, Action action);

		// Token: 0x0600035B RID: 859
		void ClearListeners(object o);
	}
}
