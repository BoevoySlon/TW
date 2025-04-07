using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000A9 RID: 169
	public class DeathOldAgeSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00052A5A File Offset: 0x00050C5A
		public Hero DeadHero { get; }

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x00052A62 File Offset: 0x00050C62
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_death_old_age";
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x00052A6C File Offset: 0x00050C6C
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_died_of_old_age", null);
			}
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00052AC6 File Offset: 0x00050CC6
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.DeadHero.ClanBanner
			};
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00052AE0 File Offset: 0x00050CE0
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

		// Token: 0x060011E0 RID: 4576 RVA: 0x00052BAC File Offset: 0x00050DAC
		public DeathOldAgeSceneNotificationItem(Hero deadHero)
		{
			this.DeadHero = deadHero;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x04000612 RID: 1554
		private const int NumberOfAudienceHeroes = 5;

		// Token: 0x04000614 RID: 1556
		private readonly CampaignTime _creationCampaignTime;
	}
}
