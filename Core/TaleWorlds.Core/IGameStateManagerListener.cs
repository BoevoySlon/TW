using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000082 RID: 130
	public interface IGameStateManagerListener
	{
		// Token: 0x060007D4 RID: 2004
		void OnCreateState(GameState gameState);

		// Token: 0x060007D5 RID: 2005
		void OnPushState(GameState gameState, bool isTopGameState);

		// Token: 0x060007D6 RID: 2006
		void OnPopState(GameState gameState);

		// Token: 0x060007D7 RID: 2007
		void OnCleanStates();

		// Token: 0x060007D8 RID: 2008
		void OnSavedGameLoadFinished();
	}
}
