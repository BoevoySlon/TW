using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000135 RID: 309
	public class DefaultSettlementMilitiaModel : SettlementMilitiaModel
	{
		// Token: 0x06001772 RID: 6002 RVA: 0x00074359 File Offset: 0x00072559
		public override int MilitiaToSpawnAfterSiege(Town town)
		{
			return Math.Max(30, (int)(town.MilitiaChange * 20f));
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x0007436F File Offset: 0x0007256F
		public override ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false)
		{
			return DefaultSettlementMilitiaModel.CalculateMilitiaChangeInternal(settlement, includeDescriptions);
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00074378 File Offset: 0x00072578
		public override float CalculateEliteMilitiaSpawnChance(Settlement settlement)
		{
			float num = 0f;
			Hero hero = null;
			if (settlement.IsFortification && settlement.Town.Governor != null)
			{
				hero = settlement.Town.Governor;
			}
			else if (settlement.IsVillage)
			{
				Settlement tradeBound = settlement.Village.TradeBound;
				if (((tradeBound != null) ? tradeBound.Town.Governor : null) != null)
				{
					hero = settlement.Village.TradeBound.Town.Governor;
				}
			}
			if (hero != null && hero.GetPerkValue(DefaultPerks.Leadership.CitizenMilitia))
			{
				num += DefaultPerks.Leadership.CitizenMilitia.PrimaryBonus;
			}
			return num;
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0007440A File Offset: 0x0007260A
		public override void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate)
		{
			meleeTroopRate = 0.5f;
			rangedTroopRate = 1f - meleeTroopRate;
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00074420 File Offset: 0x00072620
		private static ExplainedNumber CalculateMilitiaChangeInternal(Settlement settlement, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			float militia = settlement.Militia;
			if (settlement.IsFortification)
			{
				result.Add(2f, DefaultSettlementMilitiaModel.BaseText, null);
			}
			float value = -militia * 0.025f;
			result.Add(value, DefaultSettlementMilitiaModel.RetiredText, null);
			if (settlement.IsVillage)
			{
				float value2 = settlement.Village.Hearth / 400f;
				result.Add(value2, DefaultSettlementMilitiaModel.FromHearthsText, null);
			}
			else if (settlement.IsFortification)
			{
				float num = settlement.Town.Prosperity / 1000f;
				result.Add(num, DefaultSettlementMilitiaModel.FromProsperityText, null);
				if (settlement.Town.InRebelliousState)
				{
					float num2 = MBMath.Map(settlement.Town.Loyalty, 0f, (float)Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold, (float)Campaign.Current.Models.SettlementLoyaltyModel.MilitiaBoostPercentage, 0f);
					float value3 = MathF.Abs(num * (num2 * 0.01f));
					result.Add(value3, DefaultSettlementMilitiaModel.LowLoyaltyText, null);
				}
			}
			if (settlement.IsTown)
			{
				int num3 = settlement.Town.SoldItems.Sum(delegate(Town.SellLog x)
				{
					if (x.Category.Properties != ItemCategory.Property.BonusToMilitia)
					{
						return 0;
					}
					return x.Number;
				});
				if (num3 > 0)
				{
					result.Add(0.2f * (float)num3, DefaultSettlementMilitiaModel.MilitiaFromMarketText, null);
				}
				if (settlement.OwnerClan.Kingdom != null)
				{
					if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom) && settlement.IsTown)
					{
						result.Add(-1f, DefaultPolicies.Serfdom.Name, null);
					}
					if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Cantons))
					{
						result.Add(1f, DefaultPolicies.Cantons.Name, null);
					}
				}
				if (settlement.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.BattanianMilitiaFeat))
				{
					result.Add(DefaultCulturalFeats.BattanianMilitiaFeat.EffectBonus, DefaultSettlementMilitiaModel.CultureText, null);
				}
			}
			if (settlement.IsCastle || settlement.IsTown)
			{
				if (settlement.Town.BuildingsInProgress.IsEmpty<Building>())
				{
					BuildingHelper.AddDefaultDailyBonus(settlement.Town, BuildingEffectEnum.MilitiaDaily, ref result);
				}
				foreach (Building building in settlement.Town.Buildings)
				{
					if (!building.BuildingType.IsDefaultProject)
					{
						float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.Militia);
						if (buildingEffectAmount > 0f)
						{
							result.Add(buildingEffectAmount, building.Name, null);
						}
					}
				}
				if (settlement.IsCastle && settlement.Town.InRebelliousState)
				{
					float resultNumber = result.ResultNumber;
					float num4 = 0f;
					foreach (Building building2 in settlement.Town.Buildings)
					{
						if (num4 < 1f && (!building2.BuildingType.IsDefaultProject || settlement.Town.CurrentBuilding == building2))
						{
							float buildingEffectAmount2 = building2.GetBuildingEffectAmount(BuildingEffectEnum.ReduceMilitia);
							if (buildingEffectAmount2 > 0f)
							{
								float num5 = buildingEffectAmount2 * 0.01f;
								num4 += num5;
								if (num4 > 1f)
								{
									num5 -= num4 - 1f;
								}
								float value4 = resultNumber * -num5;
								result.Add(value4, building2.Name, null);
							}
						}
					}
				}
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToPolicies(settlement, ref result);
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToPerks(settlement, ref result);
				DefaultSettlementMilitiaModel.GetSettlementMilitiaChangeDueToIssues(settlement, ref result);
			}
			return result;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x000747F0 File Offset: 0x000729F0
		private static void GetSettlementMilitiaChangeDueToPerks(Settlement settlement, ref ExplainedNumber result)
		{
			if (settlement.Town != null && settlement.Town.Governor != null)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.SwiftStrike, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.KeepAtBay, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.Drills, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.MerryMen, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Crossbow.LongShots, settlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Throwing.ThrowingCompetitions, settlement.Town, ref result);
				if (settlement.IsUnderSiege)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.ArmsDealer, settlement.Town, ref result);
				}
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.SevenVeterans, settlement.Town, ref result);
			}
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000748A8 File Offset: 0x00072AA8
		private static void GetSettlementMilitiaChangeDueToPolicies(Settlement settlement, ref ExplainedNumber result)
		{
			Kingdom kingdom = settlement.OwnerClan.Kingdom;
			if (kingdom != null && kingdom.ActivePolicies.Contains(DefaultPolicies.Citizenship))
			{
				result.Add(1f, DefaultPolicies.Citizenship.Name, null);
			}
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000748EC File Offset: 0x00072AEC
		private static void GetSettlementMilitiaChangeDueToIssues(Settlement settlement, ref ExplainedNumber result)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementMilitia, settlement, ref result);
		}

		// Token: 0x0400083F RID: 2111
		private static readonly TextObject BaseText = new TextObject("{=militarybase}Base", null);

		// Token: 0x04000840 RID: 2112
		private static readonly TextObject FromHearthsText = new TextObject("{=ecdZglky}From Hearths", null);

		// Token: 0x04000841 RID: 2113
		private static readonly TextObject FromProsperityText = new TextObject("{=cTmiNAlI}From Prosperity", null);

		// Token: 0x04000842 RID: 2114
		private static readonly TextObject RetiredText = new TextObject("{=gHnfFi1s}Retired", null);

		// Token: 0x04000843 RID: 2115
		private static readonly TextObject MilitiaFromMarketText = new TextObject("{=7ve3bQxg}Weapons From Market", null);

		// Token: 0x04000844 RID: 2116
		private static readonly TextObject FoodShortageText = new TextObject("{=qTFKvGSg}Food Shortage", null);

		// Token: 0x04000845 RID: 2117
		private static readonly TextObject LowLoyaltyText = new TextObject("{=SJ2qsRdF}Low Loyalty", null);

		// Token: 0x04000846 RID: 2118
		private static readonly TextObject CultureText = GameTexts.FindText("str_culture", null);

		// Token: 0x04000847 RID: 2119
		private const int AutoSpawnMilitiaDayMultiplierAfterSiege = 20;

		// Token: 0x04000848 RID: 2120
		private const int MinimumAutoSpawnedMilitiaAfterSiege = 30;
	}
}
