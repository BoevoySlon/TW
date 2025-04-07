using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000146 RID: 326
	public class DefaultVassalRewardsModel : VassalRewardsModel
	{
		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06001839 RID: 6201 RVA: 0x0007B305 File Offset: 0x00079505
		public override int RelationRewardWithLeader
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x0007B309 File Offset: 0x00079509
		public override float InfluenceReward
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0007B310 File Offset: 0x00079510
		public override ItemRoster GetEquipmentRewardsForJoiningKingdom(Kingdom kingdom)
		{
			ItemRoster itemRoster = new ItemRoster();
			foreach (ItemObject item in kingdom.Culture.VassalRewardItems)
			{
				itemRoster.AddToCounts(item, 1);
			}
			ItemObject randomBannerAtLevel = this.GetRandomBannerAtLevel(2, kingdom.Culture);
			if (randomBannerAtLevel != null)
			{
				itemRoster.AddToCounts(randomBannerAtLevel, 1);
			}
			return itemRoster;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0007B38C File Offset: 0x0007958C
		private ItemObject GetRandomBannerAtLevel(int bannerLevel, CultureObject culture = null)
		{
			MBList<ItemObject> e = Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItems().ToMBList<ItemObject>();
			if (culture == null)
			{
				return e.GetRandomElementWithPredicate((ItemObject i) => (i.ItemComponent as BannerComponent).BannerLevel == bannerLevel);
			}
			return e.GetRandomElementWithPredicate((ItemObject i) => (i.ItemComponent as BannerComponent).BannerLevel == bannerLevel && i.Culture == culture);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0007B3F4 File Offset: 0x000795F4
		public override TroopRoster GetTroopRewardsForJoiningKingdom(Kingdom kingdom)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (PartyTemplateStack partyTemplateStack in kingdom.Culture.VassalRewardTroopsPartyTemplate.Stacks)
			{
				troopRoster.AddToCounts(partyTemplateStack.Character, partyTemplateStack.MaxValue, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x0400087F RID: 2175
		private const int VassalRewardBannerLevel = 2;
	}
}
