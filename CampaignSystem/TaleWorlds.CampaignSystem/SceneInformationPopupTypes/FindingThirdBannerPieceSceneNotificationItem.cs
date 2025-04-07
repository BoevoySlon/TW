using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B1 RID: 177
	public class FindingThirdBannerPieceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x00053423 File Offset: 0x00051623
		public override string SceneID
		{
			get
			{
				return "scn_third_banner_piece_notification";
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0005342A File Offset: 0x0005162A
		public override bool IsAffirmativeOptionShown { get; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x00053434 File Offset: 0x00051634
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_third_banner_piece_found", null);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001208 RID: 4616 RVA: 0x00053479 File Offset: 0x00051679
		public override TextObject AffirmativeTitleText
		{
			get
			{
				return GameTexts.FindText("str_third_banner_piece_found_assembled", null);
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x00053486 File Offset: 0x00051686
		public override TextObject AffirmativeText
		{
			get
			{
				return new TextObject("{=6mgapvxb}Assemble", null);
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x0600120A RID: 4618 RVA: 0x00053493 File Offset: 0x00051693
		public override TextObject AffirmativeDescriptionText
		{
			get
			{
				return new TextObject("{=IRLB42FY}Assemble the dragon banner!", null);
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000534A0 File Offset: 0x000516A0
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				Hero.MainHero.ClanBanner
			};
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x000534B7 File Offset: 0x000516B7
		public FindingThirdBannerPieceSceneNotificationItem()
		{
			this.IsAffirmativeOptionShown = 1;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x04000626 RID: 1574
		private readonly CampaignTime _creationCampaignTime;
	}
}
