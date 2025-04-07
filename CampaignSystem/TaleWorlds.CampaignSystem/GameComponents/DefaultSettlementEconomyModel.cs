using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000131 RID: 305
	public class DefaultSettlementEconomyModel : SettlementEconomyModel
	{
		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06001731 RID: 5937 RVA: 0x00072EB8 File Offset: 0x000710B8
		private DefaultSettlementEconomyModel.CategoryValues CategoryValuesCache
		{
			get
			{
				if (this._categoryValues == null)
				{
					this._categoryValues = new DefaultSettlementEconomyModel.CategoryValues();
				}
				return this._categoryValues;
			}
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00072ED4 File Offset: 0x000710D4
		public override ValueTuple<float, float> GetSupplyDemandForCategory(Town town, ItemCategory category, float dailySupply, float dailyDemand, float oldSupply, float oldDemand)
		{
			float num = oldSupply * 0.85f + dailySupply * 0.15f;
			float item = oldDemand * 0.85f + dailyDemand * 0.15f;
			num = MathF.Max(0.1f, num);
			return new ValueTuple<float, float>(num, item);
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00072F18 File Offset: 0x00071118
		public override float GetDailyDemandForCategory(Town town, ItemCategory category, int extraProsperity)
		{
			float num = MathF.Max(0f, town.Prosperity + (float)extraProsperity);
			float num2 = MathF.Max(0f, town.Prosperity - 3000f);
			float num3 = category.BaseDemand * num;
			float num4 = category.LuxuryDemand * num2;
			float result = num3 + num4;
			if (category.BaseDemand < 1E-08f)
			{
				result = num * 0.01f;
			}
			return result;
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x00072F7C File Offset: 0x0007117C
		public override int GetTownGoldChange(Town town)
		{
			float num = 10000f + town.Prosperity * 12f - (float)town.Gold;
			return MathF.Round(0.25f * num);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x00072FB0 File Offset: 0x000711B0
		public override float GetDemandChangeFromValue(float purchaseValue)
		{
			return purchaseValue * 0.15f;
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x00072FB9 File Offset: 0x000711B9
		public override float GetEstimatedDemandForCategory(Town town, ItemData itemData, ItemCategory category)
		{
			return this.GetDailyDemandForCategory(town, category, 1000);
		}

		// Token: 0x04000817 RID: 2071
		private DefaultSettlementEconomyModel.CategoryValues _categoryValues;

		// Token: 0x04000818 RID: 2072
		private const int ProsperityLuxuryTreshold = 3000;

		// Token: 0x04000819 RID: 2073
		private const float dailyChangeFactor = 0.15f;

		// Token: 0x0400081A RID: 2074
		private const float oneMinusDailyChangeFactor = 0.85f;

		// Token: 0x0200050E RID: 1294
		private class CategoryValues
		{
			// Token: 0x060043E2 RID: 17378 RVA: 0x00146DF0 File Offset: 0x00144FF0
			public CategoryValues()
			{
				this.PriceDict = new Dictionary<ItemCategory, int>();
				foreach (ItemObject itemObject in Items.All)
				{
					this.PriceDict[itemObject.GetItemCategory()] = itemObject.Value;
				}
			}

			// Token: 0x060043E3 RID: 17379 RVA: 0x00146E64 File Offset: 0x00145064
			public int GetValueOfCategory(ItemCategory category)
			{
				int result = 1;
				this.PriceDict.TryGetValue(category, out result);
				return result;
			}

			// Token: 0x040015BB RID: 5563
			public Dictionary<ItemCategory, int> PriceDict;
		}
	}
}
