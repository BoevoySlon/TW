using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C7 RID: 455
	public abstract class CutsceneSelectionModel : GameModel
	{
		// Token: 0x06001BC2 RID: 7106
		public abstract SceneNotificationData GetKingdomDestroyedSceneNotification(Kingdom kingdom);
	}
}
