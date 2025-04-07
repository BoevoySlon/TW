using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B9 RID: 185
	public class MainHeroBattleVictoryDeathNotificationItem : SceneNotificationData
	{
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001249 RID: 4681 RVA: 0x000544D7 File Offset: 0x000526D7
		public Hero DeadHero { get; }

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x0600124A RID: 4682 RVA: 0x000544DF File Offset: 0x000526DF
		public List<CharacterObject> EncounterAllyCharacters { get; }

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x0600124B RID: 4683 RVA: 0x000544E7 File Offset: 0x000526E7
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_battle_victory_death";
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x000544F0 File Offset: 0x000526F0
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("NAME", this.DeadHero.Name);
				return GameTexts.FindText("str_main_hero_battle_death", null);
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0005454C File Offset: 0x0005274C
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.DeadHero.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 2; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(this.DeadHero.MapFaction.Culture);
				Equipment equipment = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			List<CharacterObject> encounterAllyCharacters = this.EncounterAllyCharacters;
			foreach (CharacterObject characterObject in ((encounterAllyCharacters != null) ? encounterAllyCharacters.Take(3) : null))
			{
				if (characterObject.IsHero)
				{
					Equipment overridenEquipment2 = characterObject.HeroObject.BattleEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment2, false, false);
					list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(characterObject.HeroObject, overridenEquipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
				}
				else
				{
					Equipment overriddenEquipment = characterObject.FirstBattleEquipment.Clone(false);
					CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overriddenEquipment, false, false);
					list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, overriddenEquipment, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
				}
			}
			return list;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000546CC File Offset: 0x000528CC
		public MainHeroBattleVictoryDeathNotificationItem(Hero deadHero, List<CharacterObject> encounterAllyCharacters)
		{
			this.DeadHero = deadHero;
			this.EncounterAllyCharacters = encounterAllyCharacters;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x04000647 RID: 1607
		private const int NumberOfCorpses = 2;

		// Token: 0x04000648 RID: 1608
		private const int NumberOfCompanions = 3;

		// Token: 0x0400064B RID: 1611
		private readonly CampaignTime _creationCampaignTime;
	}
}
