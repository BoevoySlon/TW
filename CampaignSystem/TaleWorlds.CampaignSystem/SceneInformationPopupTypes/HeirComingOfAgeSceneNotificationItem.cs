using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B3 RID: 179
	public class HeirComingOfAgeSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001213 RID: 4627 RVA: 0x000536A6 File Offset: 0x000518A6
		public Hero MentorHero { get; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001214 RID: 4628 RVA: 0x000536AE File Offset: 0x000518AE
		public Hero HeroCameOfAge { get; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001215 RID: 4629 RVA: 0x000536B6 File Offset: 0x000518B6
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_heir_coming_of_age";
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x000536C0 File Offset: 0x000518C0
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("HERO_NAME", this.HeroCameOfAge.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_hero_came_of_age", null);
			}
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x0005371C File Offset: 0x0005191C
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.MentorHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MentorHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			string childStageEquipmentIDFromCulture = CampaignSceneNotificationHelper.GetChildStageEquipmentIDFromCulture(this.HeroCameOfAge.Culture);
			Equipment overridenEquipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>(childStageEquipmentIDFromCulture).DefaultEquipment.Clone(false);
			BodyProperties overriddenBodyProperties = new BodyProperties(new DynamicBodyProperties(6f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, overridenEquipment2, false, overriddenBodyProperties, uint.MaxValue, uint.MaxValue, false));
			BodyProperties overriddenBodyProperties2 = new BodyProperties(new DynamicBodyProperties(14f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, overridenEquipment2, false, overriddenBodyProperties2, uint.MaxValue, uint.MaxValue, false));
			Equipment overridenEquipment3 = this.HeroCameOfAge.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment3, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, overridenEquipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			return list;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00053861 File Offset: 0x00051A61
		public HeirComingOfAgeSceneNotificationItem(Hero mentorHero, Hero heroCameOfAge, CampaignTime creationTime)
		{
			this.MentorHero = mentorHero;
			this.HeroCameOfAge = heroCameOfAge;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x0400062C RID: 1580
		private readonly CampaignTime _creationCampaignTime;
	}
}
