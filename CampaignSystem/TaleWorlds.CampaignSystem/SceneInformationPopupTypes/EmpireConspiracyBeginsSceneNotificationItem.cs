using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000AB RID: 171
	public class EmpireConspiracyBeginsSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00052E89 File Offset: 0x00051089
		public Hero PlayerHero { get; }

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00052E91 File Offset: 0x00051091
		public Kingdom Empire { get; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00052E99 File Offset: 0x00051099
		public bool IsConspiracyAgainstEmpire { get; }

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x00052EA1 File Offset: 0x000510A1
		public override string SceneID
		{
			get
			{
				return "scn_empire_conspiracy_start_notification";
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00052EA8 File Offset: 0x000510A8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				if (this.IsConspiracyAgainstEmpire)
				{
					return GameTexts.FindText("str_empire_conspiracy_begins_antiempire", null);
				}
				return GameTexts.FindText("str_empire_conspiracy_begins_proempire", null);
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00052F01 File Offset: 0x00051101
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.Empire.Banner
			};
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00052F1C File Offset: 0x0005111C
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			for (int i = 0; i < 8; i++)
			{
				Equipment equipment = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("conspirator_cutscene_template").DefaultEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
				CharacterObject facePropertiesFromAudienceIndex = this.GetFacePropertiesFromAudienceIndex(false, i);
				BodyProperties bodyProperties = facePropertiesFromAudienceIndex.GetBodyProperties(equipment, MBRandom.RandomInt(100));
				uint customColor = this._audienceColors[MBRandom.RandomInt(this._audienceColors.Length)];
				uint customColor2 = this._audienceColors[MBRandom.RandomInt(this._audienceColors.Length)];
				list.Add(new SceneNotificationData.SceneNotificationCharacter(facePropertiesFromAudienceIndex, equipment, bodyProperties, false, customColor, customColor2, false));
			}
			return list;
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00052FC0 File Offset: 0x000511C0
		public EmpireConspiracyBeginsSceneNotificationItem(Hero playerHero, Kingdom empire, bool isConspiracyAgainstEmpire)
		{
			this.PlayerHero = playerHero;
			this.Empire = empire;
			this.IsConspiracyAgainstEmpire = isConspiracyAgainstEmpire;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00053000 File Offset: 0x00051200
		private CharacterObject GetFacePropertiesFromAudienceIndex(bool playerWantsRestore, int audienceMemberIndex)
		{
			if (!playerWantsRestore)
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("villager_empire");
			}
			string objectName;
			switch (audienceMemberIndex % 8)
			{
			case 0:
				objectName = "villager_battania";
				break;
			case 1:
				objectName = "villager_khuzait";
				break;
			case 2:
				objectName = "villager_vlandia";
				break;
			case 3:
				objectName = "villager_aserai";
				break;
			case 4:
				objectName = "villager_battania";
				break;
			case 5:
				objectName = "villager_sturgia";
				break;
			default:
				objectName = "villager_battania";
				break;
			}
			return MBObjectManager.Instance.GetObject<CharacterObject>(objectName);
		}

		// Token: 0x04000618 RID: 1560
		private const int AudienceNumber = 8;

		// Token: 0x04000619 RID: 1561
		private readonly uint[] _audienceColors = new uint[]
		{
			4278914065U,
			4284308292U,
			4281543757U,
			4282199842U
		};

		// Token: 0x0400061D RID: 1565
		private readonly CampaignTime _creationCampaignTime;
	}
}
