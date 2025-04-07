using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B6 RID: 182
	public class KingdomCreatedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x000540C9 File Offset: 0x000522C9
		public Kingdom NewKingdom { get; }

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001236 RID: 4662 RVA: 0x000540D1 File Offset: 0x000522D1
		public override string SceneID
		{
			get
			{
				return "scn_kingdom_made";
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x000540D8 File Offset: 0x000522D8
		public override bool PauseActiveState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x000540DC File Offset: 0x000522DC
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("KINGDOM_NAME", this.NewKingdom.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("LEADER_NAME", this.NewKingdom.Leader.Name);
				return GameTexts.FindText("str_kingdom_created", null);
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x00054150 File Offset: 0x00052350
		public override TextObject AffirmativeText
		{
			get
			{
				return GameTexts.FindText("str_ok", null);
			}
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0005415D File Offset: 0x0005235D
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.NewKingdom.Banner,
				this.NewKingdom.Banner
			};
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00054188 File Offset: 0x00052388
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Hero leader = this.NewKingdom.Leader;
			Equipment overridenEquipment = leader.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(leader, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForKingdom(this.NewKingdom, false).Take(5))
			{
				Equipment overridenEquipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment2, true, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, overridenEquipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00054258 File Offset: 0x00052458
		public KingdomCreatedSceneNotificationItem(Kingdom newKingdom)
		{
			this.NewKingdom = newKingdom;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x0400063D RID: 1597
		private const int NumberOfKingdomMemberAudience = 5;

		// Token: 0x0400063F RID: 1599
		private readonly CampaignTime _creationCampaignTime;
	}
}
