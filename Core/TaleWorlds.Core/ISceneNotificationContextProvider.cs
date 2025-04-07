using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000BC RID: 188
	public interface ISceneNotificationContextProvider
	{
		// Token: 0x06000992 RID: 2450
		bool IsContextAllowed(SceneNotificationData.RelevantContextType relevantType);
	}
}
