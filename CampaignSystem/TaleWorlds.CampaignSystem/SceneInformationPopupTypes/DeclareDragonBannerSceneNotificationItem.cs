using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000AA RID: 170
	public class DeclareDragonBannerSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x00052BC6 File Offset: 0x00050DC6
		public bool PlayerWantsToRestore { get; }

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x00052BCE File Offset: 0x00050DCE
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_declare_dragon_banner";
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x00052BD8 File Offset: 0x00050DD8
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("PLAYER_WANTS_RESTORE", this.PlayerWantsToRestore ? 1 : 0);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				return GameTexts.FindText("str_declare_dragon_banner", null);
			}
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x00052C34 File Offset: 0x00050E34
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			IOrderedEnumerable<Hero> clanHeroesPool = from h in Hero.MainHero.Clan.Heroes
			where !h.IsChild && h.IsAlive && h != Hero.MainHero
			orderby h.Level
			select h;
			for (int i = 0; i < 17; i++)
			{
				SceneNotificationData.SceneNotificationCharacter characterAtIndex = this.GetCharacterAtIndex(i, clanHeroesPool);
				list.Add(characterAtIndex);
			}
			return list;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x00052CBD File Offset: 0x00050EBD
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				Hero.MainHero.ClanBanner
			};
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00052CD4 File Offset: 0x00050ED4
		public DeclareDragonBannerSceneNotificationItem(bool playerWantsToRestore)
		{
			this.PlayerWantsToRestore = playerWantsToRestore;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x00052CF0 File Offset: 0x00050EF0
		private SceneNotificationData.SceneNotificationCharacter GetCharacterAtIndex(int index, IOrderedEnumerable<Hero> clanHeroesPool)
		{
			bool flag = false;
			int num = -1;
			string objectName = string.Empty;
			switch (index)
			{
			case 0:
				objectName = "battanian_picked_warrior";
				num = 0;
				break;
			case 1:
				objectName = "imperial_infantryman";
				break;
			case 2:
				objectName = "imperial_veteran_infantryman";
				break;
			case 3:
				objectName = "sturgian_warrior";
				num = 1;
				break;
			case 4:
				objectName = "imperial_menavliaton";
				break;
			case 5:
				objectName = "sturgian_ulfhednar";
				num = 2;
				break;
			case 6:
				objectName = "aserai_recruit";
				break;
			case 7:
				objectName = "aserai_skirmisher";
				break;
			case 8:
				objectName = "aserai_veteran_faris";
				break;
			case 9:
				objectName = "imperial_legionary";
				num = 3;
				break;
			case 10:
				objectName = "mountain_bandits_bandit";
				break;
			case 11:
				objectName = "mountain_bandits_chief";
				break;
			case 12:
				objectName = "forest_people_tier_3";
				num = 4;
				break;
			case 13:
				objectName = "mountain_bandits_raider";
				break;
			case 14:
				flag = true;
				break;
			case 15:
				objectName = "vlandian_pikeman";
				break;
			case 16:
				objectName = "vlandian_voulgier";
				break;
			}
			uint customColor = uint.MaxValue;
			uint customColor2 = uint.MaxValue;
			CharacterObject characterObject;
			if (flag)
			{
				characterObject = CharacterObject.PlayerCharacter;
				customColor = Hero.MainHero.MapFaction.Color;
				customColor2 = Hero.MainHero.MapFaction.Color2;
			}
			else if (num != -1 && clanHeroesPool.ElementAtOrDefault(num) != null)
			{
				Hero hero = clanHeroesPool.ElementAtOrDefault(num);
				characterObject = hero.CharacterObject;
				customColor = hero.MapFaction.Color;
				customColor2 = hero.MapFaction.Color2;
			}
			else
			{
				characterObject = MBObjectManager.Instance.GetObject<CharacterObject>(objectName);
			}
			Equipment overriddenEquipment = characterObject.FirstBattleEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overriddenEquipment, true, false);
			return new SceneNotificationData.SceneNotificationCharacter(characterObject, overriddenEquipment, default(BodyProperties), false, customColor, customColor2, false);
		}

		// Token: 0x04000615 RID: 1557
		private const int NumberOfCharacters = 17;

		// Token: 0x04000617 RID: 1559
		private readonly CampaignTime _creationCampaignTime;
	}
}
