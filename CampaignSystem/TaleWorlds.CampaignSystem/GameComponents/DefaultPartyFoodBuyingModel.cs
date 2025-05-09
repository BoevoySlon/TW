﻿using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011D RID: 285
	public class DefaultPartyFoodBuyingModel : PartyFoodBuyingModel
	{
		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x0006D1EF File Offset: 0x0006B3EF
		public override float MinimumDaysFoodToLastWhileBuyingFoodFromTown
		{
			get
			{
				return 30f;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x0600168B RID: 5771 RVA: 0x0006D1F6 File Offset: 0x0006B3F6
		public override float MinimumDaysFoodToLastWhileBuyingFoodFromVillage
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x0006D1FD File Offset: 0x0006B3FD
		public override float LowCostFoodPriceAverage
		{
			get
			{
				return 30f;
			}
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0006D204 File Offset: 0x0006B404
		public override void FindItemToBuy(MobileParty mobileParty, Settlement settlement, out ItemRosterElement itemElement, out float itemElementsPrice)
		{
			itemElement = ItemRosterElement.Invalid;
			itemElementsPrice = 0f;
			float num = 0f;
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			int num2 = -1;
			for (int i = 0; i < settlement.ItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = settlement.ItemRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Amount > 0)
				{
					bool flag = elementCopyAtIndex.EquipmentElement.Item.HasHorseComponent && elementCopyAtIndex.EquipmentElement.Item.HorseComponent.IsLiveStock;
					if (elementCopyAtIndex.EquipmentElement.Item.IsFood || flag)
					{
						int itemPrice = settlementComponent.GetItemPrice(elementCopyAtIndex.EquipmentElement, mobileParty, false);
						int itemValue = elementCopyAtIndex.EquipmentElement.ItemValue;
						if ((itemPrice < 120 || flag) && mobileParty.LeaderHero.Gold >= itemPrice)
						{
							object obj = flag ? ((120f - (float)(itemPrice / elementCopyAtIndex.EquipmentElement.Item.HorseComponent.MeatCount)) * 0.0083f) : ((float)(120 - itemPrice) * 0.0083f);
							float num3 = flag ? ((100f - (float)(itemValue / elementCopyAtIndex.EquipmentElement.Item.HorseComponent.MeatCount)) * 0.01f) : ((float)(100 - itemValue) * 0.01f);
							object obj2 = obj;
							float num4 = obj2 * obj2 * num3 * num3;
							if (num4 > 0f)
							{
								if (MBRandom.RandomFloat * (num + num4) >= num)
								{
									num2 = i;
									itemElementsPrice = (float)itemPrice;
								}
								num += num4;
							}
						}
					}
				}
			}
			if (num2 != -1)
			{
				itemElement = settlement.ItemRoster.GetElementCopyAtIndex(num2);
			}
		}
	}
}
