using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000A8 RID: 168
	public static class MBObjectManagerExtensions
	{
		// Token: 0x06000854 RID: 2132 RVA: 0x0001C34C File Offset: 0x0001A54C
		public static void LoadXML(this MBObjectManager objectManager, string id, bool skipXmlFilterForEditor = false)
		{
			Game game = Game.Current;
			bool isDevelopment = false;
			string gameType = "";
			if (game != null)
			{
				isDevelopment = game.GameType.IsDevelopment;
				gameType = game.GameType.GetType().Name;
			}
			objectManager.LoadXML(id, isDevelopment, gameType, skipXmlFilterForEditor);
		}
	}
}
