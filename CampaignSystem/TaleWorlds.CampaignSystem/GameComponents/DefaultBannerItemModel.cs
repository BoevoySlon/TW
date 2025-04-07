using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000EC RID: 236
	public class DefaultBannerItemModel : BannerItemModel
	{
		// Token: 0x06001497 RID: 5271 RVA: 0x0005C738 File Offset: 0x0005A938
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItems()
		{
			return Items.All.WhereQ((ItemObject i) => i.IsBannerItem && i.StringId != "campaign_banner_small");
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0005C764 File Offset: 0x0005A964
		public override IEnumerable<ItemObject> GetPossibleRewardBannerItemsForHero(Hero hero)
		{
			IEnumerable<ItemObject> possibleRewardBannerItems = this.GetPossibleRewardBannerItems();
			int bannerItemLevelForHero = this.GetBannerItemLevelForHero(hero);
			List<ItemObject> list = new List<ItemObject>();
			foreach (ItemObject itemObject in possibleRewardBannerItems)
			{
				if ((itemObject.Culture == null || itemObject.Culture == hero.Culture) && (itemObject.ItemComponent as BannerComponent).BannerLevel == bannerItemLevelForHero)
				{
					list.Add(itemObject);
				}
			}
			return list;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0005C7EC File Offset: 0x0005A9EC
		public override int GetBannerItemLevelForHero(Hero hero)
		{
			if (hero.Clan == null || hero.Clan.Leader != hero)
			{
				return 1;
			}
			if (hero.MapFaction.IsKingdomFaction && hero.Clan.Kingdom.RulingClan == hero.Clan)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0005C839 File Offset: 0x0005AA39
		public override bool CanBannerBeUpdated(ItemObject item)
		{
			return true;
		}

		// Token: 0x0400071B RID: 1819
		public const int BannerLevel1 = 1;

		// Token: 0x0400071C RID: 1820
		public const int BannerLevel2 = 2;

		// Token: 0x0400071D RID: 1821
		public const int BannerLevel3 = 3;

		// Token: 0x0400071E RID: 1822
		private const string MapBannerId = "campaign_banner_small";
	}
}
