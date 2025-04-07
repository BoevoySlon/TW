using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B8 RID: 184
	public class MainHeroBattleDeathNotificationItem : SceneNotificationData
	{
		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001243 RID: 4675 RVA: 0x00054378 File Offset: 0x00052578
		public Hero DeadHero { get; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001244 RID: 4676 RVA: 0x00054380 File Offset: 0x00052580
		public CultureObject KillerCulture { get; }

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001245 RID: 4677 RVA: 0x00054388 File Offset: 0x00052588
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_main_hero_battle_death";
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x00054390 File Offset: 0x00052590
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

		// Token: 0x06001247 RID: 4679 RVA: 0x000543EC File Offset: 0x000525EC
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.DeadHero.BattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.DeadHero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 23; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture((this.KillerCulture != null && (float)i > 11.5f) ? this.KillerCulture : this.DeadHero.MapFaction.Culture);
				Equipment equipment = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000544B6 File Offset: 0x000526B6
		public MainHeroBattleDeathNotificationItem(Hero deadHero, CultureObject killerCulture = null)
		{
			this.DeadHero = deadHero;
			this.KillerCulture = killerCulture;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x04000643 RID: 1603
		private const int NumberOfCorpses = 23;

		// Token: 0x04000646 RID: 1606
		private readonly CampaignTime _creationCampaignTime;
	}
}
