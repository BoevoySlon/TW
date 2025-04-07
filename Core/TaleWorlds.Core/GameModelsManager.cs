using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x0200006A RID: 106
	public abstract class GameModelsManager
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x0001873B File Offset: 0x0001693B
		protected GameModelsManager(IEnumerable<GameModel> inputComponents)
		{
			this._gameModels = inputComponents.ToMBList<GameModel>();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00018750 File Offset: 0x00016950
		protected T GetGameModel<T>() where T : GameModel
		{
			for (int i = this._gameModels.Count - 1; i >= 0; i--)
			{
				T result;
				if ((result = (this._gameModels[i] as T)) != null)
				{
					return result;
				}
			}
			return default(T);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001879F File Offset: 0x0001699F
		public MBReadOnlyList<GameModel> GetGameModels()
		{
			return this._gameModels;
		}

		// Token: 0x040003AD RID: 941
		private readonly MBList<GameModel> _gameModels;
	}
}
