using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BC RID: 188
	public class NewBornFemaleHeroSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x00055059 File Offset: 0x00053259
		public Hero MaleHero { get; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x00055061 File Offset: 0x00053261
		public Hero FemaleHero { get; }

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x00055069 File Offset: 0x00053269
		public override string SceneID
		{
			get
			{
				return "scn_born_baby_female_hero";
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x00055070 File Offset: 0x00053270
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_baby_born_only_mother", null);
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000550CC File Offset: 0x000532CC
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			CharacterObject characterObject = CharacterObject.All.First((CharacterObject h) => h.StringId == "cutscene_midwife");
			Equipment overridenEquipment = this.MaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, true, false);
			Equipment overridenEquipment2 = this.FemaleHero.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment2, true, true);
			Equipment overriddenEquipment = characterObject.FirstCivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overriddenEquipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.MaleHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.FemaleHero, overridenEquipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, overriddenEquipment, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			return list;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x000551AF File Offset: 0x000533AF
		public NewBornFemaleHeroSceneNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x04000656 RID: 1622
		private readonly CampaignTime _creationCampaignTime;
	}
}
