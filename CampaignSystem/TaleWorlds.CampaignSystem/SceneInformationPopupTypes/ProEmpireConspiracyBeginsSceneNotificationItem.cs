﻿using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000AE RID: 174
	public class ProEmpireConspiracyBeginsSceneNotificationItem : EmpireConspiracySupportsSceneNotificationItemBase
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060011F9 RID: 4601 RVA: 0x000532BC File Offset: 0x000514BC
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject = GameTexts.FindText("str_empire_conspiracy_supports_proempire", null);
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
				textObject.SetTextVariable("YEAR", CampaignTime.Now.GetYear);
				return textObject;
			}
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00053303 File Offset: 0x00051503
		public ProEmpireConspiracyBeginsSceneNotificationItem(Hero kingHero) : base(kingHero)
		{
		}
	}
}
