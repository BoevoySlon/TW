using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x02000067 RID: 103
	public interface IGameStarter
	{
		// Token: 0x06000718 RID: 1816
		void AddModel(GameModel gameModel);

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000719 RID: 1817
		IEnumerable<GameModel> Models { get; }
	}
}
