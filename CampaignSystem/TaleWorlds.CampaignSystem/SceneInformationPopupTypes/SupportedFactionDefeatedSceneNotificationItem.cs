using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BF RID: 191
	public class SupportedFactionDefeatedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x0005563E File Offset: 0x0005383E
		public Kingdom Faction { get; }

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x00055646 File Offset: 0x00053846
		public bool PlayerWantsRestore { get; }

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x0005564E File Offset: 0x0005384E
		public override string SceneID
		{
			get
			{
				return "scn_supported_faction_defeated_notification";
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x00055658 File Offset: 0x00053858
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("FORMAL_NAME", CampaignSceneNotificationHelper.GetFormalNameForKingdom(this.Faction));
				GameTexts.SetVariable("PLAYER_WANTS_RESTORE", this.PlayerWantsRestore ? 1 : 0);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_supported_faction_defeated", null);
			}
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x000556C8 File Offset: 0x000538C8
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.Faction.Banner,
				this.Faction.Banner
			};
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x000556F1 File Offset: 0x000538F1
		public SupportedFactionDefeatedSceneNotificationItem(Kingdom faction, bool playerWantsRestore)
		{
			this.Faction = faction;
			this.PlayerWantsRestore = playerWantsRestore;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x04000660 RID: 1632
		private readonly CampaignTime _creationCampaignTime;
	}
}
