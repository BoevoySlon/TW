using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B2 RID: 178
	public class HeirComingOfAgeFemaleSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x000534D1 File Offset: 0x000516D1
		public Hero MentorHero { get; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x000534D9 File Offset: 0x000516D9
		public Hero HeroCameOfAge { get; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x0600120F RID: 4623 RVA: 0x000534E1 File Offset: 0x000516E1
		public override string SceneID
		{
			get
			{
				return "scn_hero_come_of_age_female";
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x000534E8 File Offset: 0x000516E8
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

		// Token: 0x06001211 RID: 4625 RVA: 0x00053544 File Offset: 0x00051744
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.MentorHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MentorHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			string childStageEquipmentIDFromCulture = CampaignSceneNotificationHelper.GetChildStageEquipmentIDFromCulture(this.HeroCameOfAge.Culture);
			Equipment overridenEquipment2 = MBObjectManager.Instance.GetObject<MBEquipmentRoster>(childStageEquipmentIDFromCulture).DefaultEquipment.Clone(false);
			BodyProperties overriddenBodyProperties = new BodyProperties(new DynamicBodyProperties(6f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, overridenEquipment2, false, overriddenBodyProperties, uint.MaxValue, uint.MaxValue, false));
			BodyProperties overriddenBodyProperties2 = new BodyProperties(new DynamicBodyProperties(14f, this.HeroCameOfAge.Weight, this.HeroCameOfAge.Build), this.HeroCameOfAge.StaticBodyProperties);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, overridenEquipment2, false, overriddenBodyProperties2, uint.MaxValue, uint.MaxValue, false));
			Equipment overridenEquipment3 = this.HeroCameOfAge.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment3, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.HeroCameOfAge, overridenEquipment3, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			return list;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00053689 File Offset: 0x00051889
		public HeirComingOfAgeFemaleSceneNotificationItem(Hero mentorHero, Hero heroCameOfAge, CampaignTime creationTime)
		{
			this.MentorHero = mentorHero;
			this.HeroCameOfAge = heroCameOfAge;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x04000629 RID: 1577
		private readonly CampaignTime _creationCampaignTime;
	}
}
