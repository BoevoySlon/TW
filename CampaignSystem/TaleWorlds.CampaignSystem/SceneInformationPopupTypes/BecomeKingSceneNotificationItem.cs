using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000A5 RID: 165
	public class BecomeKingSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x00052013 File Offset: 0x00050213
		public Hero NewLeaderHero { get; }

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x0005201B File Offset: 0x0005021B
		public override string SceneID
		{
			get
			{
				return "scn_become_king_notification";
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00052024 File Offset: 0x00050224
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject;
				if (this.NewLeaderHero.Clan.Kingdom.Culture.StringId.Equals("empire", StringComparison.InvariantCultureIgnoreCase))
				{
					textObject = GameTexts.FindText("str_become_king_empire", null);
				}
				else
				{
					TextObject variable = this.NewLeaderHero.IsFemale ? GameTexts.FindText("str_liege_title_female", this.NewLeaderHero.Clan.Kingdom.Culture.StringId) : GameTexts.FindText("str_liege_title", this.NewLeaderHero.Clan.Kingdom.Culture.StringId);
					textObject = GameTexts.FindText("str_become_king_nonempire", null);
					textObject.SetTextVariable("TITLE_NAME", variable);
				}
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				textObject.SetTextVariable("YEAR", this._creationCampaignTime.GetYear);
				textObject.SetTextVariable("KING_NAME", this.NewLeaderHero.Name);
				textObject.SetTextVariable("IS_KING_MALE", this.NewLeaderHero.IsFemale ? 0 : 1);
				return textObject;
			}
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00052140 File Offset: 0x00050340
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			Equipment overriddenEquipment = this.NewLeaderHero.CharacterObject.Equipment.Clone(true);
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			list.Add(new SceneNotificationData.SceneNotificationCharacter(this.NewLeaderHero.CharacterObject, overriddenEquipment, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false));
			for (int i = 0; i < 14; i++)
			{
				CharacterObject characterObject = this.IsAudienceFemale(i) ? this.NewLeaderHero.Clan.Kingdom.Culture.Townswoman : this.NewLeaderHero.Clan.Kingdom.Culture.Townsman;
				Equipment equipment = characterObject.FirstCivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
				uint color = BannerManager.Instance.ReadOnlyColorPalette.GetRandomElementInefficiently<KeyValuePair<int, BannerColor>>().Value.Color;
				uint color2 = BannerManager.Instance.ReadOnlyColorPalette.GetRandomElementInefficiently<KeyValuePair<int, BannerColor>>().Value.Color;
				list.Add(new SceneNotificationData.SceneNotificationCharacter(characterObject, equipment, characterObject.GetBodyProperties(equipment, MBRandom.RandomInt(100)), false, color, color2, false));
			}
			for (int j = 0; j < 2; j++)
			{
				list.Add(CampaignSceneNotificationHelper.GetBodyguardOfCulture(this.NewLeaderHero.Clan.Kingdom.MapFaction.Culture));
			}
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForHero(this.NewLeaderHero, false, false).Take(4))
			{
				Equipment overridenEquipment = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, false, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0005231C File Offset: 0x0005051C
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.NewLeaderHero.Clan.Kingdom.Banner,
				this.NewLeaderHero.Clan.Kingdom.Banner
			};
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x00052359 File Offset: 0x00050559
		public BecomeKingSceneNotificationItem(Hero newLeaderHero)
		{
			this.NewLeaderHero = newLeaderHero;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00052373 File Offset: 0x00050573
		private bool IsAudienceFemale(int indexOfAudience)
		{
			return indexOfAudience == 2 || indexOfAudience == 5 || indexOfAudience - 11 <= 2;
		}

		// Token: 0x04000607 RID: 1543
		private const int NumberOfAudience = 14;

		// Token: 0x04000608 RID: 1544
		private const int NumberOfGuards = 2;

		// Token: 0x04000609 RID: 1545
		private const int NumberOfCompanions = 4;

		// Token: 0x0400060B RID: 1547
		private readonly CampaignTime _creationCampaignTime;
	}
}
