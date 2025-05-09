﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x02000359 RID: 857
	public class TownMarketData : IMarketData
	{
		// Token: 0x060030ED RID: 12525 RVA: 0x000CE96A File Offset: 0x000CCB6A
		internal static void AutoGeneratedStaticCollectObjectsTownMarketData(object o, List<object> collectedObjects)
		{
			((TownMarketData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x000CE978 File Offset: 0x000CCB78
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._itemDict);
			collectedObjects.Add(this._town);
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000CE992 File Offset: 0x000CCB92
		internal static object AutoGeneratedGetMemberValue_itemDict(object o)
		{
			return ((TownMarketData)o)._itemDict;
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x000CE99F File Offset: 0x000CCB9F
		internal static object AutoGeneratedGetMemberValue_town(object o)
		{
			return ((TownMarketData)o)._town;
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000CE9AC File Offset: 0x000CCBAC
		public TownMarketData(Town town)
		{
			this._town = town;
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000CE9C8 File Offset: 0x000CCBC8
		public ItemData GetCategoryData(ItemCategory itemCategory)
		{
			ItemData result;
			if (!this._itemDict.TryGetValue(itemCategory, out result))
			{
				result = default(ItemData);
			}
			return result;
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000CE9F0 File Offset: 0x000CCBF0
		public int GetItemCountOfCategory(ItemCategory itemCategory)
		{
			ItemData itemData;
			if (!this._itemDict.TryGetValue(itemCategory, out itemData))
			{
				return 0;
			}
			return itemData.InStore;
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000CEA15 File Offset: 0x000CCC15
		private void SetItemData(ItemCategory itemCategory, ItemData itemData)
		{
			this._itemDict[itemCategory] = itemData;
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x000CEA24 File Offset: 0x000CCC24
		public void OnTownInventoryUpdated(ItemRosterElement item, int count)
		{
			if (item.EquipmentElement.Item == null)
			{
				this.ClearStores();
				return;
			}
			this.AddNumberInStore(item.EquipmentElement.Item.GetItemCategory(), count, item.EquipmentElement.Item.Value);
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000CEA78 File Offset: 0x000CCC78
		public void AddDemand(ItemCategory itemCategory, float demandAmount)
		{
			SettlementEconomyModel settlementConsumptionModel = Campaign.Current.Models.SettlementConsumptionModel;
			this.SetItemData(itemCategory, this.GetCategoryData(itemCategory).AddDemand(settlementConsumptionModel.GetDemandChangeFromValue(demandAmount)));
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000CEAB4 File Offset: 0x000CCCB4
		public void AddSupply(ItemCategory itemCategory, float supplyAmount)
		{
			this.SetItemData(itemCategory, this.GetCategoryData(itemCategory).AddSupply(supplyAmount));
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000CEAD8 File Offset: 0x000CCCD8
		public void AddNumberInStore(ItemCategory itemCategory, int number, int value)
		{
			this.SetItemData(itemCategory, this.GetCategoryData(itemCategory).AddInStore(number, value));
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x000CEB00 File Offset: 0x000CCD00
		public void SetSupplyDemand(ItemCategory itemCategory, float supply, float demand)
		{
			ItemData categoryData = this.GetCategoryData(itemCategory);
			this.SetItemData(itemCategory, new ItemData(supply, demand, categoryData.InStore, categoryData.InStoreValue));
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x000CEB30 File Offset: 0x000CCD30
		public void SetDemand(ItemCategory itemCategory, float demand)
		{
			ItemData categoryData = this.GetCategoryData(itemCategory);
			this.SetItemData(itemCategory, new ItemData(categoryData.Supply, demand, categoryData.InStore, categoryData.InStoreValue));
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x000CEB64 File Offset: 0x000CCD64
		public float GetDemand(ItemCategory itemCategory)
		{
			return this.GetCategoryData(itemCategory).Demand;
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000CEB72 File Offset: 0x000CCD72
		public float GetSupply(ItemCategory itemCategory)
		{
			return this.GetCategoryData(itemCategory).Supply;
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000CEB80 File Offset: 0x000CCD80
		public float GetPriceFactor(ItemCategory itemCategory)
		{
			ItemData categoryData = this.GetCategoryData(itemCategory);
			return Campaign.Current.Models.TradeItemPriceFactorModel.GetBasePriceFactor(itemCategory, (float)categoryData.InStoreValue, categoryData.Supply, categoryData.Demand, false, 0);
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x000CEBBF File Offset: 0x000CCDBF
		public int GetPrice(ItemObject item, MobileParty tradingParty = null, bool isSelling = false, PartyBase merchantParty = null)
		{
			return this.GetPrice(new EquipmentElement(item, null, null, false), tradingParty, isSelling, null);
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x000CEBD4 File Offset: 0x000CCDD4
		public int GetPrice(EquipmentElement itemRosterElement, MobileParty tradingParty = null, bool isSelling = false, PartyBase merchantParty = null)
		{
			ItemData categoryData = this.GetCategoryData(itemRosterElement.Item.GetItemCategory());
			return Campaign.Current.Models.TradeItemPriceFactorModel.GetPrice(itemRosterElement, tradingParty, merchantParty, isSelling, (float)categoryData.InStoreValue, categoryData.Supply, categoryData.Demand);
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x000CEC20 File Offset: 0x000CCE20
		public void UpdateStores()
		{
			this.ClearStores();
			ItemRoster itemRoster = this._town.Owner.ItemRoster;
			for (int i = 0; i < itemRoster.Count; i++)
			{
				ItemRosterElement itemRosterElement = itemRoster[i];
				if (itemRosterElement.EquipmentElement.Item.ItemCategory != null)
				{
					ItemData categoryData = this.GetCategoryData(itemRosterElement.EquipmentElement.Item.GetItemCategory());
					this.SetItemData(itemRosterElement.EquipmentElement.Item.GetItemCategory(), categoryData.AddInStore(itemRosterElement.Amount, itemRosterElement.EquipmentElement.Item.Value));
				}
			}
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x000CECD0 File Offset: 0x000CCED0
		private void ClearStores()
		{
			foreach (ItemCategory itemCategory in ItemCategories.All)
			{
				ItemData categoryData = this.GetCategoryData(itemCategory);
				this.SetItemData(itemCategory, new ItemData(categoryData.Supply, categoryData.Demand, 0, 0));
			}
		}

		// Token: 0x04000FED RID: 4077
		[SaveableField(1)]
		private Dictionary<ItemCategory, ItemData> _itemDict = new Dictionary<ItemCategory, ItemData>();

		// Token: 0x04000FEE RID: 4078
		[SaveableField(2)]
		private Town _town;
	}
}
