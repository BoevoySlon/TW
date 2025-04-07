using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000AC RID: 172
	public abstract class EmpireConspiracySupportsSceneNotificationItemBase : SceneNotificationData
	{
		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x00053085 File Offset: 0x00051285
		public Hero King { get; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x0005308D File Offset: 0x0005128D
		public override string SceneID
		{
			get
			{
				return "scn_empire_conspiracy_supports_notification";
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00053094 File Offset: 0x00051294
		public override TextObject AffirmativeText
		{
			get
			{
				return GameTexts.FindText("str_ok", null);
			}
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x000530A1 File Offset: 0x000512A1
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.King.MapFaction.Banner,
				this.King.MapFaction.Banner
			};
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x000530D4 File Offset: 0x000512D4
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment overridenEquipment = this.King.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, false, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.King, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			CharacterObject @object = MBObjectManager.Instance.GetObject<CharacterObject>("villager_battania");
			Equipment equipment = MBObjectManager.Instance.GetObject<MBEquipmentRoster>("conspirator_cutscene_template").DefaultEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, false, false);
			BodyProperties bodyProperties = @object.GetBodyProperties(equipment, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment, bodyProperties, false, 0U, 0U, false));
			bodyProperties = @object.GetBodyProperties(equipment, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment, bodyProperties, false, 0U, 0U, false));
			bodyProperties = @object.GetBodyProperties(equipment, MBRandom.RandomInt(100));
			list.Add(new SceneNotificationData.SceneNotificationCharacter(@object, equipment, bodyProperties, false, 0U, 0U, false));
			list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.King.MapFaction.Culture));
			list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.King.MapFaction.Culture));
			return list;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x000531EE File Offset: 0x000513EE
		protected EmpireConspiracySupportsSceneNotificationItemBase(Hero kingHero)
		{
			this.King = kingHero;
		}
	}
}
