using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010F RID: 271
	public class DefaultItemDiscardModel : ItemDiscardModel
	{
		// Token: 0x060015F7 RID: 5623 RVA: 0x00068DB8 File Offset: 0x00066FB8
		public override bool PlayerCanDonateItem(ItemObject item)
		{
			bool result = false;
			if (item.HasWeaponComponent)
			{
				result = MobileParty.MainParty.HasPerk(DefaultPerks.Steward.GivingHands, false);
			}
			else if (item.HasArmorComponent)
			{
				result = MobileParty.MainParty.HasPerk(DefaultPerks.Steward.PaidInPromise, true);
			}
			return result;
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x00068DFC File Offset: 0x00066FFC
		public override int GetXpBonusForDiscardingItem(ItemObject item, int amount = 1)
		{
			int num = 0;
			if (this.PlayerCanDonateItem(item))
			{
				switch (item.Tier)
				{
				case ItemObject.ItemTiers.Tier1:
					num = 75;
					break;
				case ItemObject.ItemTiers.Tier2:
					num = 150;
					break;
				case ItemObject.ItemTiers.Tier3:
					num = 250;
					break;
				case ItemObject.ItemTiers.Tier4:
				case ItemObject.ItemTiers.Tier5:
				case ItemObject.ItemTiers.Tier6:
					num = 300;
					break;
				default:
					num = 35;
					break;
				}
			}
			return num * amount;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00068E60 File Offset: 0x00067060
		public override int GetXpBonusForDiscardingItems(ItemRoster itemRoster)
		{
			float num = 0f;
			for (int i = 0; i < itemRoster.Count; i++)
			{
				ItemObject itemAtIndex = itemRoster.GetItemAtIndex(i);
				num += (float)this.GetXpBonusForDiscardingItem(itemAtIndex, itemRoster.GetElementNumber(i));
			}
			return MathF.Floor(num);
		}
	}
}
