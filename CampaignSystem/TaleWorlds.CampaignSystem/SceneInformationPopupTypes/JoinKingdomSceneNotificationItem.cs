using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000B5 RID: 181
	public class JoinKingdomSceneNotificationItem : SceneNotificationData
	{
		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x00053F24 File Offset: 0x00052124
		public Clan NewMemberClan { get; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600122E RID: 4654 RVA: 0x00053F2C File Offset: 0x0005212C
		public Kingdom KingdomToUse { get; }

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00053F34 File Offset: 0x00052134
		public override string SceneID
		{
			get
			{
				return "scn_cutscene_factionjoin";
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001230 RID: 4656 RVA: 0x00053F3B File Offset: 0x0005213B
		public override SceneNotificationData.RelevantContextType RelevantContext
		{
			get
			{
				return SceneNotificationData.RelevantContextType.Map;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x00053F40 File Offset: 0x00052140
		public override TextObject TitleText
		{
			get
			{
				GameTexts.SetVariable("CLAN_NAME", this.NewMemberClan.Name);
				GameTexts.SetVariable("DAY_OF_YEAR", CampaignSceneNotificationHelper.GetFormalDayAndSeasonText(this._creationCampaignTime));
				GameTexts.SetVariable("YEAR", this._creationCampaignTime.GetYear);
				GameTexts.SetVariable("KINGDOM_FORMALNAME", CampaignSceneNotificationHelper.GetFormalNameForKingdom(this.KingdomToUse));
				return GameTexts.FindText("str_new_faction_member", null);
			}
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00053FAF File Offset: 0x000521AF
		public override IEnumerable<Banner> GetBanners()
		{
			return new List<Banner>
			{
				this.KingdomToUse.Banner,
				this.KingdomToUse.Banner
			};
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00053FD8 File Offset: 0x000521D8
		public override IEnumerable<SceneNotificationData.SceneNotificationCharacter> GetSceneNotificationCharacters()
		{
			List<SceneNotificationData.SceneNotificationCharacter> list = new List<SceneNotificationData.SceneNotificationCharacter>();
			Hero leader = this.NewMemberClan.Leader;
			Equipment overridenEquipment = leader.CivilianEquipment.Clone(false);
			CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment, true, false);
			list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(leader, overridenEquipment, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			foreach (Hero hero in CampaignSceneNotificationHelper.GetMilitaryAudienceForKingdom(this.KingdomToUse, true).Take(5))
			{
				Equipment overridenEquipment2 = hero.CivilianEquipment.Clone(false);
				CampaignSceneNotificationHelper.RemoveWeaponsFromEquipment(ref overridenEquipment2, true, false);
				list.Add(CampaignSceneNotificationHelper.CreateNotificationCharacterFromHero(hero, overridenEquipment2, false, default(BodyProperties), uint.MaxValue, uint.MaxValue, false));
			}
			return list;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x000540A8 File Offset: 0x000522A8
		public JoinKingdomSceneNotificationItem(Clan newMember, Kingdom kingdom)
		{
			this.NewMemberClan = newMember;
			this.KingdomToUse = kingdom;
			this._creationCampaignTime = CampaignTime.Now;
		}

		// Token: 0x04000639 RID: 1593
		private const int NumberOfKingdomMembers = 5;

		// Token: 0x0400063C RID: 1596
		private readonly CampaignTime _creationCampaignTime;
	}
}
