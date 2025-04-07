using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000083 RID: 131
	public interface IGameStateManagerOwner
	{
		// Token: 0x060007D9 RID: 2009
		void OnStateStackEmpty();

		// Token: 0x060007DA RID: 2010
		void OnStateChanged(GameState oldState);
	}
}
