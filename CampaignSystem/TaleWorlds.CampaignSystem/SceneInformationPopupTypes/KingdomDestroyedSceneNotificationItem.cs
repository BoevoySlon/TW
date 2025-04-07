using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B7 RID: 183
	public class KingdomDestroyedSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x00054272 File Offset: 0x00052472
		public Kingdom DestroyedKingdom { get; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600123E RID: 4670 RVA: 0x0005427A File Offset: 0x0005247A
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_enemykingdom_destroyed";
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00054284 File Offset: 0x00052484
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("FORMAL_NAME", CampaignSceneNotificationHelper.GetFormalNameForKingdom(this.DestroyedKingdom));
				return GameTexts.FindText("str_kingdom_destroyed_scene_notification", null);
			}
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x000542DE File Offset: 0x000524DE
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.DestroyedKingdom.Banner
			};
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000542F8 File Offset: 0x000524F8
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			for (int i = 0; i < 2; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(this.DestroyedKingdom.Culture);
				Equipment equipment = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00054362 File Offset: 0x00052562
		public KingdomDestroyedSceneNotificationItem(Kingdom destroyedKingdom, CampaignTime creationTime)
		{
			this.DestroyedKingdom = destroyedKingdom;
			this._creationCampaignTime = creationTime;
		}

		// Token: 0x04000640 RID: 1600
		private const int NumberOfDeadTroops = 2;

		// Token: 0x04000642 RID: 1602
		private readonly CampaignTime _creationCampaignTime;
	}
}
