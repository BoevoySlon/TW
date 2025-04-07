using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000054 RID: 84
	public interface IEntityComponent
	{
		// Token: 0x0600064D RID: 1613
		void OnInitialize();

		// Token: 0x0600064E RID: 1614
		void OnFinalize();
	}
}
