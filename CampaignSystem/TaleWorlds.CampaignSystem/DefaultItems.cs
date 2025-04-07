using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000075 RID: 117
	public class DefaultItems
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0004627D File Offset: 0x0004447D
		private static DefaultItems Instance
		{
			get
			{
				return Campaign.Current.DefaultItems;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00046289 File Offset: 0x00044489
		public static ItemObject Grain
		{
			get
			{
				return DefaultItems.Instance._itemGrain;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00046295 File Offset: 0x00044495
		public static ItemObject Meat
		{
			get
			{
				return DefaultItems.Instance._itemMeat;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x000462A1 File Offset: 0x000444A1
		public static ItemObject Hides
		{
			get
			{
				return DefaultItems.Instance._itemHides;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x000462AD File Offset: 0x000444AD
		public static ItemObject Tools
		{
			get
			{
				return DefaultItems.Instance._itemTools;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x000462B9 File Offset: 0x000444B9
		public static ItemObject IronOre
		{
			get
			{
				return DefaultItems.Instance._itemIronOre;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x000462C5 File Offset: 0x000444C5
		public static ItemObject HardWood
		{
			get
			{
				return DefaultItems.Instance._itemHardwood;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x000462D1 File Offset: 0x000444D1
		public static ItemObject Charcoal
		{
			get
			{
				return DefaultItems.Instance._itemCharcoal;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x000462DD File Offset: 0x000444DD
		public static ItemObject IronIngot1
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot1;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x000462E9 File Offset: 0x000444E9
		public static ItemObject IronIngot2
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot2;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x000462F5 File Offset: 0x000444F5
		public static ItemObject IronIngot3
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot3;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00046301 File Offset: 0x00044501
		public static ItemObject IronIngot4
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot4;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0004630D File Offset: 0x0004450D
		public static ItemObject IronIngot5
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot5;
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x00046319 File Offset: 0x00044519
		public static ItemObject IronIngot6
		{
			get
			{
				return DefaultItems.Instance._itemIronIngot6;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00046325 File Offset: 0x00044525
		public static ItemObject Trash
		{
			get
			{
				return DefaultItems.Instance._itemTrash;
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x00046331 File Offset: 0x00044531
		public DefaultItems()
		{
			this.RegisterAll();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00046340 File Offset: 0x00044540
		private void RegisterAll()
		{
			this._itemGrain = this.Create("grain");
			this._itemMeat = this.Create("meat");
			this._itemHides = this.Create("hides");
			this._itemTools = this.Create("tools");
			this._itemIronOre = this.Create("iron");
			this._itemHardwood = this.Create("hardwood");
			this._itemCharcoal = this.Create("charcoal");
			this._itemIronIngot1 = this.Create("ironIngot1");
			this._itemIronIngot2 = this.Create("ironIngot2");
			this._itemIronIngot3 = this.Create("ironIngot3");
			this._itemIronIngot4 = this.Create("ironIngot4");
			this._itemIronIngot5 = this.Create("ironIngot5");
			this._itemIronIngot6 = this.Create("ironIngot6");
			this._itemTrash = this.Create("trash");
			this.InitializeAll();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00046441 File Offset: 0x00044641
		private ItemObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<ItemObject>(new ItemObject(stringId));
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00046458 File Offset: 0x00044658
		private void InitializeAll()
		{
			ItemObject.InitializeTradeGood(this._itemGrain, new TextObject("{=Itv3fgJm}Grain{@Plural}loads of grain{\\@}", null), "merchandise_grain", DefaultItemCategories.Grain, 10, 10f, ItemObject.ItemTypeEnum.Goods, true);
			ItemObject.InitializeTradeGood(this._itemMeat, new TextObject("{=LmwhFv5p}Meat{@Plural}loads of meat{\\@}", null), "merchandise_meat", DefaultItemCategories.Meat, 30, 10f, ItemObject.ItemTypeEnum.Goods, true);
			ItemObject.InitializeTradeGood(this._itemHides, new TextObject("{=4kvKQuXM}Hides{@Plural}loads of hide{\\@}", null), "merchandise_hides_b", DefaultItemCategories.Hides, 50, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemTools, new TextObject("{=n3cjEB0X}Tools{@Plural}loads of tools{\\@}", null), "bd_pickaxe_b", DefaultItemCategories.Tools, 200, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronOre, new TextObject("{=Kw6BkhIf}Iron Ore{@Plural}loads of iron ore{\\@}", null), "iron_ore", DefaultItemCategories.Iron, 50, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemHardwood, new TextObject("{=ExjMoUiT}Hardwood{@Plural}hardwood logs{\\@}", null), "hardwood", DefaultItemCategories.Wood, 25, 10f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemCharcoal, new TextObject("{=iQadPYNe}Charcoal{@Plural}loads of charcoal{\\@}", null), "charcoal", DefaultItemCategories.Wood, 50, 5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot1, new TextObject("{=gOpodlt1}Crude Iron{@Plural}loads of crude iron{\\@}", null), "crude_iron", DefaultItemCategories.Iron, 20, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot2, new TextObject("{=7HvtT8bm}Wrought Iron{@Plural}loads of wrought iron{\\@}", null), "wrought_iron", DefaultItemCategories.Iron, 30, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot3, new TextObject("{=XHmmbnbB}Iron{@Plural}loads of iron{\\@}", null), "iron_a", DefaultItemCategories.Iron, 60, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot4, new TextObject("{=UfuLKuaI}Steel{@Plural}loads of steel{\\@}", null), "steel", DefaultItemCategories.Iron, 100, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot5, new TextObject("{=azjMBa86}Fine Steel{@Plural}loads of fine steel{\\@}", null), "fine_steel", DefaultItemCategories.Iron, 160, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemIronIngot6, new TextObject("{=vLVAfcta}Thamaskene Steel{@Plural}loads of thamaskene steel{\\@}", null), "thamaskene_steel", DefaultItemCategories.Iron, 260, 0.5f, ItemObject.ItemTypeEnum.Goods, false);
			ItemObject.InitializeTradeGood(this._itemTrash, new TextObject("{=ZvZN6UkU}Trash Item", null), "iron_ore", DefaultItemCategories.Unassigned, 1, 1f, ItemObject.ItemTypeEnum.Goods, false);
		}

		// Token: 0x040004B0 RID: 1200
		private const float TradeGoodWeight = 10f;

		// Token: 0x040004B1 RID: 1201
		private const float HalfWeight = 5f;

		// Token: 0x040004B2 RID: 1202
		private const float IngotWeight = 0.5f;

		// Token: 0x040004B3 RID: 1203
		private const float TrashWeight = 1f;

		// Token: 0x040004B4 RID: 1204
		private const int IngotValue = 20;

		// Token: 0x040004B5 RID: 1205
		private const int TrashValue = 1;

		// Token: 0x040004B6 RID: 1206
		private ItemObject _itemGrain;

		// Token: 0x040004B7 RID: 1207
		private ItemObject _itemMeat;

		// Token: 0x040004B8 RID: 1208
		private ItemObject _itemHides;

		// Token: 0x040004B9 RID: 1209
		private ItemObject _itemTools;

		// Token: 0x040004BA RID: 1210
		private ItemObject _itemIronOre;

		// Token: 0x040004BB RID: 1211
		private ItemObject _itemHardwood;

		// Token: 0x040004BC RID: 1212
		private ItemObject _itemCharcoal;

		// Token: 0x040004BD RID: 1213
		private ItemObject _itemIronIngot1;

		// Token: 0x040004BE RID: 1214
		private ItemObject _itemIronIngot2;

		// Token: 0x040004BF RID: 1215
		private ItemObject _itemIronIngot3;

		// Token: 0x040004C0 RID: 1216
		private ItemObject _itemIronIngot4;

		// Token: 0x040004C1 RID: 1217
		private ItemObject _itemIronIngot5;

		// Token: 0x040004C2 RID: 1218
		private ItemObject _itemIronIngot6;

		// Token: 0x040004C3 RID: 1219
		private ItemObject _itemTrash;
	}
}
