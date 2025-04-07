using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BD RID: 189
	public class NewBornSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001265 RID: 4709 RVA: 0x000551CC File Offset: 0x000533CC
		public Hero MaleHero { get; }

		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x000551D4 File Offset: 0x000533D4
		public Hero FemaleHero { get; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001267 RID: 4711 RVA: 0x000551DC File Offset: 0x000533DC
		public override string SceneID
		{
			get
			{
				return "scn_born_baby";
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x000551E4 File Offset: 0x000533E4
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("FATHER_NAME", this.MaleHero.Name);
				GameTexts.SetVariable("MOTHER_NAME", this.FemaleHero.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_baby_born", null);
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00055254 File Offset: 0x00053454
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

		// Token: 0x0600126A RID: 4714 RVA: 0x00055337 File Offset: 0x00053537
		public NewBornSceneNotificationItem(Hero maleHero, Hero femaleHero, CampaignTime creationTime)
		{
			this.MaleHero = maleHero;
			this.FemaleHero = femaleHero;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x04000659 RID: 1625
		private readonly CampaignTime _creationCampaignTime;
	}
}
