using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003A5 RID: 933
	public class ItemConsumptionBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003816 RID: 14358 RVA: 0x000FD634 File Offset: 0x000FB834
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedFollowUp));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedFollowUpEnd));
			CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, new Action<Town>(this.DailyTickTown));
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x000FD686 File Offset: 0x000FB886
		private void OnNewGameCreatedFollowUp(CampaignGameStarter starter, int i)
		{
			if (i < 2)
			{
				this.MakeConsumptionAllTowns();
			}
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x000FD694 File Offset: 0x000FB894
		private void OnNewGameCreatedFollowUpEnd(CampaignGameStarter starter)
		{
			Dictionary<ItemCategory, float> categoryBudget = new Dictionary<ItemCategory, float>();
			for (int i = 0; i < 10; i++)
			{
				foreach (Town town in Town.AllTowns)
				{
					ItemConsumptionBehavior.UpdateSupplyAndDemand(town);
					this.UpdateDemandShift(town, categoryBudget);
				}
			}
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x000FD700 File Offset: 0x000FB900
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x000FD704 File Offset: 0x000FB904
		private void DailyTickTown(Town town)
		{
			Dictionary<ItemCategory, int> saleLog = new Dictionary<ItemCategory, int>();
			this.MakeConsumptionInTown(town, saleLog);
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x000FD720 File Offset: 0x000FB920
		private void MakeConsumptionAllTowns()
		{
			foreach (Town town in Town.AllTowns)
			{
				this.DailyTickTown(town);
			}
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x000FD774 File Offset: 0x000FB974
		private void MakeConsumptionInTown(Town town, Dictionary<ItemCategory, int> saleLog)
		{
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			this.DeleteOverproducedItems(town);
			ItemConsumptionBehavior.UpdateSupplyAndDemand(town);
			this.UpdateDemandShift(town, dictionary);
			ItemConsumptionBehavior.MakeConsumption(town, dictionary, saleLog);
			ItemConsumptionBehavior.GetFoodFromMarket(town, saleLog);
			this.UpdateSellLog(town, saleLog);
			this.UpdateTownGold(town);
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x000FD7BC File Offset: 0x000FB9BC
		private void UpdateTownGold(Town town)
		{
			int townGoldChange = Campaign.Current.Models.SettlementConsumptionModel.GetTownGoldChange(town);
			town.ChangeGold(townGoldChange);
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x000FD7E8 File Offset: 0x000FB9E8
		private void DeleteOverproducedItems(Town town)
		{
			ItemRoster itemRoster = town.Owner.ItemRoster;
			for (int i = itemRoster.Count - 1; i >= 0; i--)
			{
				ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				int amount = elementCopyAtIndex.Amount;
				if (amount > 0 && (item.IsCraftedByPlayer || item.IsBannerItem))
				{
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -amount);
				}
				else if (elementCopyAtIndex.EquipmentElement.ItemModifier != null && MBRandom.RandomFloat < 0.5f)
				{
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -1);
				}
			}
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x000FD88C File Offset: 0x000FBA8C
		private static void GetFoodFromMarket(Town town, Dictionary<ItemCategory, int> saleLog)
		{
			float foodChange = town.FoodChange;
			ValueTuple<int, int> townFoodAndMarketStocks = TownHelpers.GetTownFoodAndMarketStocks(town);
			int item = townFoodAndMarketStocks.Item1;
			int item2 = townFoodAndMarketStocks.Item2;
			if (town.IsTown && town.IsUnderSiege && foodChange < 0f && item <= 0 && item2 > 0)
			{
				ItemConsumptionBehavior.GetFoodFromMarketInternal(town, Math.Abs(MathF.Floor(foodChange)), saleLog);
			}
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x000FD8E8 File Offset: 0x000FBAE8
		private void UpdateSellLog(Town town, Dictionary<ItemCategory, int> saleLog)
		{
			List<Town.SellLog> list = new List<Town.SellLog>();
			foreach (KeyValuePair<ItemCategory, int> keyValuePair in saleLog)
			{
				if (keyValuePair.Value > 0)
				{
					list.Add(new Town.SellLog(keyValuePair.Key, keyValuePair.Value));
				}
			}
			town.SetSoldItems(list);
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000FD960 File Offset: 0x000FBB60
		private static void GetFoodFromMarketInternal(Town town, int amount, Dictionary<ItemCategory, int> saleLog)
		{
			ItemRoster itemRoster = town.Owner.ItemRoster;
			int num = itemRoster.Count - 1;
			while (num >= 0 && amount > 0)
			{
				ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(num);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				if (item.ItemCategory.Properties == ItemCategory.Property.BonusToFoodStores)
				{
					int num2 = (elementCopyAtIndex.Amount >= amount) ? amount : elementCopyAtIndex.Amount;
					amount -= num2;
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -num2);
					int num3 = 0;
					saleLog.TryGetValue(item.ItemCategory, out num3);
					saleLog[item.ItemCategory] = num3 + num2;
				}
				num--;
			}
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000FDA0C File Offset: 0x000FBC0C
		private static void MakeConsumption(Town town, Dictionary<ItemCategory, float> categoryDemand, Dictionary<ItemCategory, int> saleLog)
		{
			saleLog.Clear();
			TownMarketData marketData = town.MarketData;
			ItemRoster itemRoster = town.Owner.ItemRoster;
			for (int i = itemRoster.Count - 1; i >= 0; i--)
			{
				ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				int amount = elementCopyAtIndex.Amount;
				ItemCategory itemCategory = item.GetItemCategory();
				float demand = categoryDemand[itemCategory];
				float num = ItemConsumptionBehavior.CalculateBudget(town, demand, itemCategory);
				if (num > 0.01f)
				{
					int price = marketData.GetPrice(item, null, false, null);
					float num2 = num / (float)price;
					if (num2 > (float)amount)
					{
						num2 = (float)amount;
					}
					int num3 = MBRandom.RoundRandomized(num2);
					if (num3 > amount)
					{
						num3 = amount;
					}
					itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement, -num3);
					categoryDemand[itemCategory] = num - num2 * (float)price;
					town.ChangeGold(num3 * price);
					int num4 = 0;
					saleLog.TryGetValue(itemCategory, out num4);
					saleLog[itemCategory] = num4 + num3;
				}
			}
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000FDB12 File Offset: 0x000FBD12
		private static float CalculateBudget(Town town, float demand, ItemCategory category)
		{
			return demand * MathF.Pow(town.GetItemCategoryPriceIndex(category), 0.3f);
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000FDB28 File Offset: 0x000FBD28
		private void UpdateDemandShift(Town town, Dictionary<ItemCategory, float> categoryBudget)
		{
			TownMarketData marketData = town.MarketData;
			foreach (ItemCategory itemCategory in ItemCategories.All)
			{
				categoryBudget[itemCategory] = Campaign.Current.Models.SettlementConsumptionModel.GetDailyDemandForCategory(town, itemCategory, 0);
			}
			foreach (ItemCategory itemCategory2 in ItemCategories.All)
			{
				if (itemCategory2.CanSubstitute != null)
				{
					ItemData categoryData = marketData.GetCategoryData(itemCategory2);
					ItemData categoryData2 = marketData.GetCategoryData(itemCategory2.CanSubstitute);
					if (categoryData.Supply / categoryData.Demand > categoryData2.Supply / categoryData2.Demand && categoryData2.Demand > categoryData.Demand)
					{
						float num = (categoryData2.Demand - categoryData.Demand) * itemCategory2.SubstitutionFactor;
						marketData.SetDemand(itemCategory2, categoryData.Demand + num);
						marketData.SetDemand(itemCategory2.CanSubstitute, categoryData2.Demand - num);
						float num2;
						float num3;
						if (categoryBudget.TryGetValue(itemCategory2, out num2) && categoryBudget.TryGetValue(itemCategory2.CanSubstitute, out num3))
						{
							categoryBudget[itemCategory2] = num2 + num;
							categoryBudget[itemCategory2.CanSubstitute] = num3 - num;
						}
					}
				}
			}
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000FDCA4 File Offset: 0x000FBEA4
		private static void UpdateSupplyAndDemand(Town town)
		{
			TownMarketData marketData = town.MarketData;
			SettlementEconomyModel settlementConsumptionModel = Campaign.Current.Models.SettlementConsumptionModel;
			foreach (ItemCategory itemCategory in ItemCategories.All)
			{
				ItemData categoryData = marketData.GetCategoryData(itemCategory);
				float estimatedDemandForCategory = settlementConsumptionModel.GetEstimatedDemandForCategory(town, categoryData, itemCategory);
				ValueTuple<float, float> supplyDemandForCategory = settlementConsumptionModel.GetSupplyDemandForCategory(town, itemCategory, (float)categoryData.InStoreValue, estimatedDemandForCategory, categoryData.Supply, categoryData.Demand);
				float item = supplyDemandForCategory.Item1;
				float item2 = supplyDemandForCategory.Item2;
				marketData.SetSupplyDemand(itemCategory, item, item2);
			}
		}
	}
}
