using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000AF RID: 175
	public class FindingFirstBannerPieceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060011FB RID: 4603 RVA: 0x0005330C File Offset: 0x0005150C
		public Hero PlayerHero { get; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060011FC RID: 4604 RVA: 0x00053314 File Offset: 0x00051514
		public override string SceneID
		{
			get
			{
				return "scn_first_banner_piece_notification";
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060011FD RID: 4605 RVA: 0x0005331C File Offset: 0x0005151C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_first_banner_piece_found", null);
			}
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00053361 File Offset: 0x00051561
		public override void OnCloseAction()
		{
			base.OnCloseAction();
			Action onCloseAction = this._onCloseAction;
			if (onCloseAction == null)
			{
				return;
			}
			onCloseAction();
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00053379 File Offset: 0x00051579
		public FindingFirstBannerPieceSceneNotificationItem(Hero playerHero, Action onCloseAction = null)
		{
			this.PlayerHero = playerHero;
			this._creationCampaignTime = CampaignTime.Now;
			this._onCloseAction = onCloseAction;
		}

		// Token: 0x04000621 RID: 1569
		private readonly Action _onCloseAction;

		// Token: 0x04000622 RID: 1570
		private readonly CampaignTime _creationCampaignTime;
	}
}
