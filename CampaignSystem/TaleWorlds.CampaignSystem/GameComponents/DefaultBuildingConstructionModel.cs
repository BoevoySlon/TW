using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F1 RID: 241
	public class DefaultBuildingConstructionModel : BuildingConstructionModel
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x0005D3F7 File Offset: 0x0005B5F7
		public override int TownBoostCost
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x0005D3FE File Offset: 0x0005B5FE
		public override int TownBoostBonus
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x060014C0 RID: 5312 RVA: 0x0005D402 File Offset: 0x0005B602
		public override int CastleBoostCost
		{
			get
			{
				return 250;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060014C1 RID: 5313 RVA: 0x0005D409 File Offset: 0x0005B609
		public override int CastleBoostBonus
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0005D410 File Offset: 0x0005B610
		public override ExplainedNumber CalculateDailyConstructionPower(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateDailyConstructionPowerInternal(town, ref result, false);
			return result;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0005D438 File Offset: 0x0005B638
		public override int CalculateDailyConstructionPowerWithoutBoost(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			return this.CalculateDailyConstructionPowerInternal(town, ref explainedNumber, true);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x0005D460 File Offset: 0x0005B660
		public override int GetBoostAmount(Town town)
		{
			object obj = town.IsCastle ? this.CastleBoostBonus : this.TownBoostBonus;
			float num = 0f;
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Steward.Relocation))
			{
				num += DefaultPerks.Steward.Relocation.SecondaryBonus;
			}
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Trade.SpringOfGold))
			{
				num += DefaultPerks.Trade.SpringOfGold.SecondaryBonus;
			}
			object obj2 = obj;
			return obj2 + (int)(obj2 * num);
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0005D4DD File Offset: 0x0005B6DD
		public override int GetBoostCost(Town town)
		{
			if (!town.IsCastle)
			{
				return this.TownBoostCost;
			}
			return this.CastleBoostCost;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0005D4F4 File Offset: 0x0005B6F4
		private int CalculateDailyConstructionPowerInternal(Town town, ref ExplainedNumber result, bool omitBoost = false)
		{
			float value = town.Prosperity * 0.01f;
			result.Add(value, GameTexts.FindText("str_prosperity", null), null);
			if (!omitBoost && town.BoostBuildingProcess > 0)
			{
				int num = town.IsCastle ? this.CastleBoostCost : this.TownBoostCost;
				int num2 = this.GetBoostAmount(town);
				float num3 = MathF.Min(1f, (float)town.BoostBuildingProcess / (float)num);
				float num4 = 0f;
				if (town.IsTown && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.Clockwork))
				{
					num4 += DefaultPerks.Engineering.Clockwork.SecondaryBonus;
				}
				num2 += MathF.Round((float)num2 * num4);
				result.Add((float)num2 * num3, DefaultBuildingConstructionModel.BoostText, null);
			}
			if (town.Governor != null)
			{
				Settlement currentSettlement = town.Governor.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == town)
				{
					SkillHelper.AddSkillBonusForTown(DefaultSkills.Engineering, DefaultSkillEffects.TownProjectBuildingBonus, town, ref result);
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.ForcedLabor, town, ref result);
				}
			}
			if (town.Governor != null)
			{
				Settlement currentSettlement2 = town.Governor.CurrentSettlement;
				if (((currentSettlement2 != null) ? currentSettlement2.Town : null) == town && !town.BuildingsInProgress.IsEmpty<Building>())
				{
					if (town.Governor.GetPerkValue(DefaultPerks.Steward.ForcedLabor) && town.Settlement.Party.PrisonRoster.TotalManCount > 0)
					{
						float value2 = MathF.Min(0.3f, (float)(town.Settlement.Party.PrisonRoster.TotalManCount / 3) * DefaultPerks.Steward.ForcedLabor.SecondaryBonus);
						result.AddFactor(value2, DefaultPerks.Steward.ForcedLabor.Name);
					}
					if (town.IsCastle && town.Governor.GetPerkValue(DefaultPerks.Engineering.MilitaryPlanner))
					{
						result.AddFactor(DefaultPerks.Engineering.MilitaryPlanner.SecondaryBonus, DefaultPerks.Engineering.MilitaryPlanner.Name);
					}
					else if (town.IsTown && town.Governor.GetPerkValue(DefaultPerks.Engineering.Carpenters))
					{
						result.AddFactor(DefaultPerks.Engineering.Carpenters.SecondaryBonus, DefaultPerks.Engineering.Carpenters.Name);
					}
					Building building = town.BuildingsInProgress.Peek();
					if ((building.BuildingType == DefaultBuildingTypes.Fortifications || building.BuildingType == DefaultBuildingTypes.CastleBarracks || building.BuildingType == DefaultBuildingTypes.CastleMilitiaBarracks || building.BuildingType == DefaultBuildingTypes.SettlementGarrisonBarracks || building.BuildingType == DefaultBuildingTypes.SettlementMilitiaBarracks || building.BuildingType == DefaultBuildingTypes.SettlementAquaducts) && town.Governor.GetPerkValue(DefaultPerks.Engineering.Stonecutters))
					{
						result.AddFactor(DefaultPerks.Engineering.Stonecutters.PrimaryBonus, DefaultPerks.Engineering.Stonecutters.Name);
					}
				}
			}
			SettlementLoyaltyModel settlementLoyaltyModel = Campaign.Current.Models.SettlementLoyaltyModel;
			int num5 = town.SoldItems.Sum(delegate(Town.SellLog x)
			{
				if (x.Category.Properties != ItemCategory.Property.BonusToProduction)
				{
					return 0;
				}
				return x.Number;
			});
			if (num5 > 0)
			{
				result.Add(0.25f * (float)num5, DefaultBuildingConstructionModel.ProductionFromMarketText, null);
			}
			BuildingType buildingType = town.BuildingsInProgress.IsEmpty<Building>() ? null : town.BuildingsInProgress.Peek().BuildingType;
			if (DefaultBuildingTypes.MilitaryBuildings.Contains(buildingType))
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Confidence, town, ref result);
			}
			if (buildingType == DefaultBuildingTypes.SettlementMarketplace || buildingType == DefaultBuildingTypes.SettlementAquaducts || buildingType == DefaultBuildingTypes.SettlementLimeKilns)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.SelfMadeMan, town, ref result);
			}
			float effectOfBuildings = town.GetEffectOfBuildings(BuildingEffectEnum.Construction);
			if (effectOfBuildings > 0f)
			{
				result.Add(effectOfBuildings, GameTexts.FindText("str_building_bonus", null), null);
			}
			if (town.Loyalty >= 75f)
			{
				float num6 = MBMath.Map(town.Loyalty, 75f, 100f, 0f, 20f);
				float value3 = result.ResultNumber * (num6 / 100f);
				result.Add(value3, DefaultBuildingConstructionModel.HighLoyaltyBonusText, null);
			}
			else if (town.Loyalty > 25f && town.Loyalty <= 50f)
			{
				float num7 = MBMath.Map(town.Loyalty, 25f, 50f, 50f, 0f);
				float num8 = result.ResultNumber * (num7 / 100f);
				result.Add(-num8, DefaultBuildingConstructionModel.LowLoyaltyPenaltyText, null);
			}
			else if (town.Loyalty <= 25f)
			{
				result.Add(-result.ResultNumber, DefaultBuildingConstructionModel.VeryLowLoyaltyPenaltyText, null);
				result.LimitMax(0f);
			}
			if (town.Loyalty > 25f && town.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.BattanianConstructionFeat))
			{
				result.AddFactor(DefaultCulturalFeats.BattanianConstructionFeat.EffectBonus, DefaultBuildingConstructionModel.CultureText);
			}
			result.LimitMin(0f);
			return (int)result.ResultNumber;
		}

		// Token: 0x04000720 RID: 1824
		private const float HammerMultiplier = 0.01f;

		// Token: 0x04000721 RID: 1825
		private const int VeryLowLoyaltyValue = 25;

		// Token: 0x04000722 RID: 1826
		private const float MediumLoyaltyValue = 50f;

		// Token: 0x04000723 RID: 1827
		private const float HighLoyaltyValue = 75f;

		// Token: 0x04000724 RID: 1828
		private const float HighestLoyaltyValue = 100f;

		// Token: 0x04000725 RID: 1829
		private static readonly TextObject ProductionFromMarketText = new TextObject("{=vaZDJGMx}Construction from Market", null);

		// Token: 0x04000726 RID: 1830
		private static readonly TextObject BoostText = new TextObject("{=yX1RycON}Boost from Reserve", null);

		// Token: 0x04000727 RID: 1831
		private static readonly TextObject HighLoyaltyBonusText = new TextObject("{=aSniKUJv}High Loyalty", null);

		// Token: 0x04000728 RID: 1832
		private static readonly TextObject LowLoyaltyPenaltyText = new TextObject("{=SJ2qsRdF}Low Loyalty", null);

		// Token: 0x04000729 RID: 1833
		private static readonly TextObject VeryLowLoyaltyPenaltyText = new TextObject("{=CcQzFnpN}Very Low Loyalty", null);

		// Token: 0x0400072A RID: 1834
		private static readonly TextObject CultureText = GameTexts.FindText("str_culture", null);
	}
}
