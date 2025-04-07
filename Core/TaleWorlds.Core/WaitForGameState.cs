using System;
using TaleWorlds.Network;

namespace TaleWorlds.Core
{
	// Token: 0x020000C7 RID: 199
	public class WaitForGameState : CoroutineState
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x0002059D File Offset: 0x0001E79D
		public WaitForGameState(Type stateType)
		{
			this._stateType = stateType;
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060009D7 RID: 2519 RVA: 0x000205AC File Offset: 0x0001E7AC
		protected override bool IsFinished
		{
			get
			{
				GameState gameState = (GameStateManager.Current != null) ? GameStateManager.Current.ActiveState : null;
				return gameState != null && this._stateType.IsInstanceOfType(gameState);
			}
		}

		// Token: 0x040005CC RID: 1484
		private Type _stateType;
	}
}
