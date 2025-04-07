using System;
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
	// Token: 0x02000138 RID: 312
	public class DefaultSettlementTaxModel : SettlementTaxModel
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060017A6 RID: 6054 RVA: 0x000759F1 File Offset: 0x00073BF1
		public override float SettlementCommissionRateTown
		{
			get
			{
				return 0.06f;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x000759F8 File Offset: 0x00073BF8
		public override float SettlementCommissionRateVillage
		{
			get
			{
				return 0.7f;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x000759FF File Offset: 0x00073BFF
		public override int SettlementCommissionDecreaseSecurityThreshold
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x00075A03 File Offset: 0x00073C03
		public override int MaximumDecreaseBasedOnSecuritySecurity
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00075A08 File Offset: 0x00073C08
		public override float GetTownTaxRatio(Town town)
		{
			float num = 1f;
			if (town.Settlement.OwnerClan.Kingdom != null && town.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CrownDuty))
			{
				num += 0.05f;
			}
			return this.SettlementCommissionRateTown * num;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00075A5E File Offset: 0x00073C5E
		public override float GetVillageTaxRatio()
		{
			return this.SettlementCommissionRateVillage;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00075A68 File Offset: 0x00073C68
		public override float GetTownCommissionChangeBasedOnSecurity(Town town, float commission)
		{
			if (town.Security < (float)this.SettlementCommissionDecreaseSecurityThreshold)
			{
				float num = MBMath.Map((float)this.SettlementCommissionDecreaseSecurityThreshold - town.Security, 0f, (float)this.SettlementCommissionDecreaseSecurityThreshold, (float)this.MaximumDecreaseBasedOnSecuritySecurity, 0f);
				commission -= commission * (num * 0.01f);
				return commission;
			}
			return commission;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00075AC0 File Offset: 0x00073CC0
		public override ExplainedNumber CalculateTownTax(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateDailyTaxInternal(town, ref result);
			return result;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00075AE8 File Offset: 0x00073CE8
		private float CalculateDailyTax(Town town, ref ExplainedNumber explainedNumber)
		{
			float prosperity = town.Prosperity;
			float num = 1f;
			if (town.Settlement.OwnerClan.Kingdom != null && town.Settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.CouncilOfTheCommons))
			{
				num -= 0.05f;
			}
			float num2 = 0.35f;
			float value = prosperity * num2 * num;
			explainedNumber.Add(value, DefaultSettlementTaxModel.ProsperityText, null);
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00075B5C File Offset: 0x00073D5C
		private void CalculateDailyTaxInternal(Town town, ref ExplainedNumber result)
		{
			float rawTax = this.CalculateDailyTax(town, ref result);
			this.CalculatePolicyGoldCut(town, rawTax, ref result);
			if (town.IsTown || town.IsCastle)
			{
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Bow.QuickDraw, town))
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.QuickDraw, town, ref result);
				}
				Hero governor = town.Governor;
				if (governor != null && governor.GetPerkValue(DefaultPerks.Steward.Logistician))
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.Logistician, town, ref result);
				}
			}
			if (town.IsTown)
			{
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Scouting.DesertBorn, town))
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Scouting.DesertBorn, town, ref result);
				}
				if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Steward.PriceOfLoyalty))
				{
					int num = town.Governor.GetSkillValue(DefaultSkills.Steward) - Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus;
					result.AddFactor(DefaultPerks.Steward.PriceOfLoyalty.SecondaryBonus * (float)num, DefaultPerks.Steward.PriceOfLoyalty.Name);
				}
				if (town.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.KhuzaitDecreasedTaxFeat))
				{
					result.AddFactor(DefaultCulturalFeats.KhuzaitDecreasedTaxFeat.EffectBonus, GameTexts.FindText("str_culture", null));
				}
			}
			this.GetSettlementTaxChangeDueToIssues(town, ref result);
			this.CalculateSettlementTaxDueToSecurity(town, ref result);
			this.CalculateSettlementTaxDueToLoyalty(town, ref result);
			this.CalculateSettlementTaxDueToBuildings(town, ref result);
			result.Clamp(0f, float.MaxValue);
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00075CAC File Offset: 0x00073EAC
		private void CalculateSettlementTaxDueToSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			SettlementSecurityModel settlementSecurityModel = Campaign.Current.Models.SettlementSecurityModel;
			if (town.Security >= (float)settlementSecurityModel.ThresholdForTaxBoost)
			{
				settlementSecurityModel.CalculateGoldGainDueToHighSecurity(town, ref explainedNumber);
				return;
			}
			if (town.Security >= (float)settlementSecurityModel.ThresholdForHigherTaxCorruption && town.Security < (float)settlementSecurityModel.ThresholdForTaxCorruption)
			{
				settlementSecurityModel.CalculateGoldCutDueToLowSecurity(town, ref explainedNumber);
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00075D08 File Offset: 0x00073F08
		private void CalculateSettlementTaxDueToLoyalty(Town town, ref ExplainedNumber explainedNumber)
		{
			SettlementLoyaltyModel settlementLoyaltyModel = Campaign.Current.Models.SettlementLoyaltyModel;
			if (town.Loyalty >= (float)settlementLoyaltyModel.ThresholdForTaxBoost)
			{
				settlementLoyaltyModel.CalculateGoldGainDueToHighLoyalty(town, ref explainedNumber);
				return;
			}
			if (town.Loyalty >= (float)settlementLoyaltyModel.ThresholdForHigherTaxCorruption && town.Loyalty <= (float)settlementLoyaltyModel.ThresholdForTaxCorruption)
			{
				settlementLoyaltyModel.CalculateGoldCutDueToLowLoyalty(town, ref explainedNumber);
				return;
			}
			if (town.Loyalty < (float)settlementLoyaltyModel.ThresholdForHigherTaxCorruption)
			{
				explainedNumber.AddFactor(-1f, DefaultSettlementTaxModel.VeryLowLoyalty);
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00075D84 File Offset: 0x00073F84
		private void CalculateSettlementTaxDueToBuildings(Town town, ref ExplainedNumber result)
		{
			if (town.IsTown || town.IsCastle)
			{
				foreach (Building building in town.Buildings)
				{
					float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.Tax);
					result.AddFactor(buildingEffectAmount * 0.01f, building.Name);
				}
			}
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00075DFC File Offset: 0x00073FFC
		private void CalculatePolicyGoldCut(Town town, float rawTax, ref ExplainedNumber explainedNumber)
		{
			if (town.MapFaction.IsKingdomFaction)
			{
				Kingdom kingdom = (Kingdom)town.MapFaction;
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Magistrates))
				{
					explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.Magistrates.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Cantons))
				{
					explainedNumber.Add(-0.1f * rawTax, DefaultPolicies.Cantons.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Bailiffs))
				{
					explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.Bailiffs.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.TribunesOfThePeople))
				{
					explainedNumber.Add(-0.05f * rawTax, DefaultPolicies.TribunesOfThePeople.Name, null);
				}
			}
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00075ECB File Offset: 0x000740CB
		private void GetSettlementTaxChangeDueToIssues(Town center, ref ExplainedNumber result)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementTax, center.Owner.Settlement, ref result);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00075EF2 File Offset: 0x000740F2
		public override int CalculateVillageTaxFromIncome(Village village, int marketIncome)
		{
			return (int)((float)marketIncome * this.GetVillageTaxRatio());
		}

		// Token: 0x0400085E RID: 2142
		private static readonly TextObject ProsperityText = GameTexts.FindText("str_prosperity", null);

		// Token: 0x0400085F RID: 2143
		private static readonly TextObject VeryLowSecurity = new TextObject("{=IaJ4lhzx}Very Low Security", null);

		// Token: 0x04000860 RID: 2144
		private static readonly TextObject VeryLowLoyalty = new TextObject("{=CcQzFnpN}Very Low Loyalty", null);
	}
}
