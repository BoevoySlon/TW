using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000AD RID: 173
	public class AntiEmpireConspiracyBeginsSceneNotificationItem : EmpireConspiracySupportsSceneNotificationItemBase
	{
		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x00053200 File Offset: 0x00051400
		public override TextObject TitleText
		{
			get
			{
				List<TextObject> list = new List<TextObject>();
				foreach (Kingdom kingdom in this._antiEmpireFactions)
				{
					list.Add(kingdom.InformalName);
				}
				TextObject textObject = GameTexts.FindText("str_empire_conspiracy_supports_antiempire", null);
				textObject.SetTextVariable("FACTION_NAMES", GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, true));
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(CampaignTime.Now));
				textObject.SetTextVariable("YEAR", CampaignTime.Now.GetYear);
				return textObject;
			}
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000532AC File Offset: 0x000514AC
		public AntiEmpireConspiracyBeginsSceneNotificationItem(Hero kingHero, List<Kingdom> antiEmpireFactions) : base(kingHero)
		{
			this._antiEmpireFactions = antiEmpireFactions;
		}

		// Token: 0x0400061F RID: 1567
		private readonly List<Kingdom> _antiEmpireFactions;
	}
}
