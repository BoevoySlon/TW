using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BB RID: 187
	public class NewBornFemaleHeroSceneAlternateNotificationItem : SceneNotificationData
	{
		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x00054F09 File Offset: 0x00053109
		public Hero MaleHero { get; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x00054F11 File Offset: 0x00053111
		public Hero FemaleHero { get; }

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x00054F19 File Offset: 0x00053119
		public override string SceneID
		{
			get
			{
				return "scn_born_baby_female_hero2";
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x00054F20 File Offset: 0x00053120
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				return GameTexts.FindText("str_baby_born_only_mother", null);
			}
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00054F7C File Offset: 0x0005317C
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.FemaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, true, true);
			CharacterObject characterObject = CharacterObject.All.First((CharacterObject h) => h.StringId == "cutscene_midwife");
			Equipment overriddenEquipment = characterObject.FirstCivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overriddenEquipment, false, false);
			list.Add(new SceneNotificationData.SceneNotificationCharacter(null, null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.FemaleHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, overriddenEquipment, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			return list;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x0005503C File Offset: 0x0005323C
		public NewBornFemaleHeroSceneAlternateNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x04000653 RID: 1619
		private readonly CampaignTime _creationCampaignTime;
	}
}
