using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000A7 RID: 167
	public class ClanMemberPeaceDeathSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x0005278C File Offset: 0x0005098C
		public Hero DeadHero { get; }

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00052794 File Offset: 0x00050994
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_family_member_death";
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x0005279C File Offset: 0x0005099C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_family_member_death", null);
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000527F8 File Offset: 0x000509F8
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			Equipment overridenEquipment = this.DeadHero.CivilianEquipment.Clone(false);
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForHero(this.DeadHero, true, false).Take(5))
			{
				Equipment overridenEquipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment2, false, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, overridenEquipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list;
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x000528C4 File Offset: 0x00050AC4
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.DeadHero.ClanBanner
			};
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x000528DC File Offset: 0x00050ADC
		public ClanMemberPeaceDeathSceneNotificationItem(Hero deadHero, CampaignTime creationTime)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x0400060C RID: 1548
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x0400060E RID: 1550
		private readonly CampaignTime _creationCampaignTime;
	}
}
