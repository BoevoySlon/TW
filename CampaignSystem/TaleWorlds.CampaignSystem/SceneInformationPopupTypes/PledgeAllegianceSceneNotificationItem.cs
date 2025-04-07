using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000BE RID: 190
	public class PledgeAllegianceSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x00055354 File Offset: 0x00053554
		public Hero PlayerHero { get; }

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x0600126C RID: 4716 RVA: 0x0005535C File Offset: 0x0005355C
		public bool PlayerWantsToRestore { get; }

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600126D RID: 4717 RVA: 0x00055364 File Offset: 0x00053564
		public override string SceneID
		{
			get
			{
				return "scn_pledge_allegiance_notification";
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x0005536C File Offset: 0x0005356C
		public override TextObject TitleText
		{
			get
			{
				TextObject textObject = GameTexts.FindText("str_pledge_notification_title", null);
				textObject.SetCharacterProperties("RULER", this.PlayerHero.Clan.Kingdom.Leader.CharacterObject, false);
				textObject.SetTextVariable("PLAYER_WANTS_RESTORE", this.PlayerWantsToRestore ? 1 : 0);
				textObject.SetTextVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				textObject.SetTextVariable("YEAR", this._creationCampaignTime.GetYear);
				return textObject;
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x000553F4 File Offset: 0x000535F4
		public override IEnumerable<Banner> GetBanners()
		{
			List<Banner> list = new List<Banner>();
			list.Add(Hero.MainHero.ClanBanner);
			Clan clan = this.PlayerHero.Clan.Kingdom.Leader.Clan;
			list.Add(((clan != null) ? clan.Kingdom.Banner : null) ?? this.PlayerHero.Clan.Kingdom.Leader.ClanBanner);
			return list;
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00055468 File Offset: 0x00053668
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			ItemObject itemObject = null;
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Equipment equipment = this.PlayerHero.BattleEquipment.Clone(false);
			if (equipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				itemObject = CampaignSceneNotificationHelper.GetDefaultHorseItem();
				equipment[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(itemObject, null, null, false);
			}
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.PlayerHero, equipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, true));
			Equipment equipment2 = this.PlayerHero.Clan.Kingdom.Leader.BattleEquipment.Clone(false);
			if (equipment2[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
			{
				if (itemObject == null)
				{
					itemObject = CampaignSceneNotificationHelper.GetDefaultHorseItem();
				}
				equipment2[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(itemObject, null, null, false);
			}
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment2, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(this.PlayerHero.Clan.Kingdom.Leader, equipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, true));
			IFaction mapFaction = this.PlayerHero.Clan.Kingdom.Leader.MapFaction;
			CultureObject culture = (((mapFaction != null) ? mapFaction.Culture : null) != null) ? this.PlayerHero.Clan.Kingdom.Leader.MapFaction.Culture : this.PlayerHero.MapFaction.Culture;
			for (int i = 0; i < 24; i++)
			{
				CharacterObject randomTroopForCulture = CampaignSceneNotificationHelper.GetRandomTroopForCulture(culture);
				Equipment equipment3 = randomTroopForCulture.FirstBattleEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref equipment3, false, false);
				BodyProperties bodyProperties = randomTroopForCulture.GetBodyProperties(equipment3, MBRandom.RandomInt(100));
				list.Add(new SceneNotificationData.SceneNotificationCharacter(randomTroopForCulture, equipment3, bodyProperties, false, uint.MaxValue, uint.MaxValue, false));
			}
			return list;
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0005561D File Offset: 0x0005381D
		public PledgeAllegianceSceneNotificationItem(Hero playerHero, bool playerWantsToRestore)
		{
			this.PlayerHero = playerHero;
			this.PlayerWantsToRestore = playerWantsToRestore;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x0400065A RID: 1626
		private const int NumberOfTroops = 24;

		// Token: 0x0400065D RID: 1629
		private readonly CampaignTime _creationCampaignTime;
	}
}
