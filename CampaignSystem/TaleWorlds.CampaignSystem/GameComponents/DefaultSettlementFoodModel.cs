using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000132 RID: 306
	public class DefaultSettlementFoodModel : SettlementFoodModel
	{
		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00072FD0 File Offset: 0x000711D0
		public override int FoodStocksUpperLimit
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x00072FD4 File Offset: 0x000711D4
		public override int NumberOfProsperityToEatOneFood
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x00072FD8 File Offset: 0x000711D8
		public override int NumberOfMenOnGarrisonToEatOneFood
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00072FDC File Offset: 0x000711DC
		public override int CastleFoodStockUpperLimitBonus
		{
			get
			{
				return 150;
			}
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x00072FE3 File Offset: 0x000711E3
		public override ExplainedNumber CalculateTownFoodStocksChange(Town town, bool includeMarketStocks = true, bool includeDescriptions = false)
		{
			return this.CalculateTownFoodChangeInternal(town, includeMarketStocks, includeDescriptions);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00072FF0 File Offset: 0x000711F0
		private ExplainedNumber CalculateTownFoodChangeInternal(Town town, bool includeMarketStocks, bool includeDescriptions)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			float num2;
			if (!town.IsUnderSiege)
			{
				int num = town.IsTown ? 15 : 10;
				result.Add((float)num, DefaultSettlementFoodModel.LandsAroundSettlementText, null);
				num2 = -town.Prosperity / (float)this.NumberOfProsperityToEatOneFood;
			}
			else
			{
				num2 = -town.Prosperity / (float)this.NumberOfProsperityToEatOneFood;
			}
			MobileParty garrisonParty = town.GarrisonParty;
			int num3 = (garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers : 0;
			num3 = -num3 / this.NumberOfMenOnGarrisonToEatOneFood;
			float num4 = 0f;
			float num5 = 0f;
			if (town.Governor != null)
			{
				if (town.IsUnderSiege)
				{
					if (town.Governor.GetPerkValue(DefaultPerks.Steward.Gourmet))
					{
						num5 += DefaultPerks.Steward.Gourmet.SecondaryBonus;
					}
					if (town.Governor.GetPerkValue(DefaultPerks.Medicine.TriageTent))
					{
						num4 += DefaultPerks.Medicine.TriageTent.SecondaryBonus;
					}
				}
				if (town.Governor.GetPerkValue(DefaultPerks.Steward.MasterOfWarcraft))
				{
					num4 += DefaultPerks.Steward.MasterOfWarcraft.SecondaryBonus;
				}
			}
			num2 += num2 * num4;
			num3 += (int)((float)num3 * (num4 + num5));
			result.Add(num2, DefaultSettlementFoodModel.ProsperityText, null);
			result.Add((float)num3, DefaultSettlementFoodModel.GarrisonText, null);
			Clan ownerClan = town.Settlement.OwnerClan;
			if (((ownerClan != null) ? ownerClan.Kingdom : null) != null && town.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.HuntingRights))
			{
				result.Add(2f, DefaultPolicies.HuntingRights.Name, null);
			}
			if (!town.IsUnderSiege)
			{
				foreach (Village village in town.Owner.Settlement.BoundVillages)
				{
					if (village.VillageState == Village.VillageStates.Normal)
					{
						int num6 = (village.GetHearthLevel() + 1) * 6;
						result.Add((float)num6, village.Name, null);
					}
					else
					{
						int num6 = 0;
						result.Add((float)num6, village.Name, null);
					}
				}
				float effectOfBuildings = town.GetEffectOfBuildings(BuildingEffectEnum.FoodProduction);
				if (effectOfBuildings > 0f)
				{
					result.Add(effectOfBuildings, includeDescriptions ? GameTexts.FindText("str_building_bonus", null) : TextObject.Empty, null);
				}
			}
			else if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Roguery.DirtyFighting))
			{
				result.Add(DefaultPerks.Roguery.DirtyFighting.SecondaryBonus, DefaultPerks.Roguery.DirtyFighting.Name, null);
			}
			else
			{
				result.Add(0f, DefaultSettlementFoodModel.VillagesUnderSiegeText, null);
			}
			if (includeMarketStocks)
			{
				foreach (Town.SellLog sellLog in town.SoldItems)
				{
					if (sellLog.Category.Properties == ItemCategory.Property.BonusToFoodStores)
					{
						result.Add((float)sellLog.Number, includeDescriptions ? sellLog.Category.GetName() : TextObject.Empty, null);
					}
				}
			}
			DefaultSettlementFoodModel.GetSettlementFoodChangeDueToIssues(town, ref result);
			return result;
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00073304 File Offset: 0x00071504
		private int CalculateFoodPurchasedFromMarket(Town town)
		{
			return town.SoldItems.Sum(delegate(Town.SellLog x)
			{
				if (x.Category.Properties != ItemCategory.Property.BonusToFoodStores)
				{
					return 0;
				}
				return x.Number;
			});
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00073330 File Offset: 0x00071530
		private static void GetSettlementFoodChangeDueToIssues(Town town, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementFood, town.Settlement, ref explainedNumber);
		}

		// Token: 0x0400081B RID: 2075
		private static readonly TextObject ProsperityText = GameTexts.FindText("str_prosperity", null);

		// Token: 0x0400081C RID: 2076
		private static readonly TextObject GarrisonText = GameTexts.FindText("str_garrison", null);

		// Token: 0x0400081D RID: 2077
		private static readonly TextObject LandsAroundSettlementText = GameTexts.FindText("str_lands_around_settlement", null);

		// Token: 0x0400081E RID: 2078
		private static readonly TextObject NormalVillagesText = GameTexts.FindText("str_normal_villages", null);

		// Token: 0x0400081F RID: 2079
		private static readonly TextObject RaidedVillagesText = GameTexts.FindText("str_raided_villages", null);

		// Token: 0x04000820 RID: 2080
		private static readonly TextObject VillagesUnderSiegeText = GameTexts.FindText("str_villages_under_siege", null);

		// Token: 0x04000821 RID: 2081
		private static readonly TextObject FoodBoughtByCiviliansText = GameTexts.FindText("str_food_bought_by_civilians", null);

		// Token: 0x04000822 RID: 2082
		private const int FoodProductionPerVillage = 10;
	}
}
