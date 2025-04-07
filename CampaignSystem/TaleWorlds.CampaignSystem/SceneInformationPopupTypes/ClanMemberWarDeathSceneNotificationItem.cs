using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000A8 RID: 168
	public class ClanMemberWarDeathSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x000528F2 File Offset: 0x00050AF2
		public Hero DeadHero { get; }

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000528FA File Offset: 0x00050AFA
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_family_member_death_war";
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00052904 File Offset: 0x00050B04
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_family_member_death_war", null);
			}
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x0005295E File Offset: 0x00050B5E
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.DeadHero.ClanBanner
			};
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00052978 File Offset: 0x00050B78
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.DeadHero.CivilianEquipment.Clone(false);
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

		// Token: 0x060011DA RID: 4570 RVA: 0x00052A44 File Offset: 0x00050C44
		public ClanMemberWarDeathSceneNotificationItem(Hero deadHero, CampaignTime creationTime)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x0400060F RID: 1551
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x04000611 RID: 1553
		private readonly CampaignTime _creationCampaignTime;
	}
}
