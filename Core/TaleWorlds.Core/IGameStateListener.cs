using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000081 RID: 129
	public interface IGameStateListener
	{
		// Token: 0x060007D0 RID: 2000
		void OnActivate();

		// Token: 0x060007D1 RID: 2001
		void OnDeactivate();

		// Token: 0x060007D2 RID: 2002
		void OnInitialize();

		// Token: 0x060007D3 RID: 2003
		void OnFinalize();
	}
}
