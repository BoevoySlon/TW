using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FD RID: 253
	public class DefaultCutsceneSelectionModel : CutsceneSelectionModel
	{
		// Token: 0x0600154A RID: 5450 RVA: 0x0006293E File Offset: 0x00060B3E
		public override SceneNotificationData GetKingdomDestroyedSceneNotification(Kingdom kingdom)
		{
			return new KingdomDestroyedSceneNotificationItem(kingdom, CampaignTime.Now);
		}
	}
}
