using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000134 RID: 308
	public class DefaultSettlementLoyaltyModel : SettlementLoyaltyModel
	{
		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x00073AE9 File Offset: 0x00071CE9
		public override float HighLoyaltyProsperityEffect
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00073AF0 File Offset: 0x00071CF0
		public override int LowLoyaltyProsperityEffect
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x00073AF3 File Offset: 0x00071CF3
		public override int ThresholdForTaxBoost
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x00073AF7 File Offset: 0x00071CF7
		public override int ThresholdForTaxCorruption
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x00073AFB File Offset: 0x00071CFB
		public override int ThresholdForHigherTaxCorruption
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x00073AFF File Offset: 0x00071CFF
		public override int ThresholdForProsperityBoost
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x00073B03 File Offset: 0x00071D03
		public override int ThresholdForProsperityPenalty
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x00073B07 File Offset: 0x00071D07
		public override int AdditionalStarvationPenaltyStartDay
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001753 RID: 5971 RVA: 0x00073B0B File Offset: 0x00071D0B
		public override int AdditionalStarvationLoyaltyEffect
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x00073B0E File Offset: 0x00071D0E
		public override int RebellionStartLoyaltyThreshold
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x00073B12 File Offset: 0x00071D12
		public override int RebelliousStateStartLoyaltyThreshold
		{
			get
			{
				return 25;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x00073B16 File Offset: 0x00071D16
		public override int LoyaltyBoostAfterRebellionStartValue
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x00073B19 File Offset: 0x00071D19
		public override int MilitiaBoostPercentage
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x00073B20 File Offset: 0x00071D20
		public override float ThresholdForNotableRelationBonus
		{
			get
			{
				return 75f;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001759 RID: 5977 RVA: 0x00073B27 File Offset: 0x00071D27
		public override int DailyNotableRelationBonus
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x00073B2A File Offset: 0x00071D2A
		public override int SettlementLoyaltyChangeDueToSecurityThreshold
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x00073B2E File Offset: 0x00071D2E
		public override int MaximumLoyaltyInSettlement
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x00073B32 File Offset: 0x00071D32
		public override int LoyaltyDriftMedium
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x0600175D RID: 5981 RVA: 0x00073B36 File Offset: 0x00071D36
		public override float HighSecurityLoyaltyEffect
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x00073B3D File Offset: 0x00071D3D
		public override float LowSecurityLoyaltyEffect
		{
			get
			{
				return -2f;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x0600175F RID: 5983 RVA: 0x00073B44 File Offset: 0x00071D44
		public override float GovernorSameCultureLoyaltyEffect
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x00073B4B File Offset: 0x00071D4B
		public override float GovernorDifferentCultureLoyaltyEffect
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001761 RID: 5985 RVA: 0x00073B52 File Offset: 0x00071D52
		public override float SettlementOwnerDifferentCultureLoyaltyEffect
		{
			get
			{
				return -3f;
			}
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00073B59 File Offset: 0x00071D59
		public override ExplainedNumber CalculateLoyaltyChange(Town town, bool includeDescriptions = false)
		{
			return this.CalculateLoyaltyChangeInternal(town, includeDescriptions);
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x00073B64 File Offset: 0x00071D64
		public override void CalculateGoldGainDueToHighLoyalty(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Loyalty, (float)this.ThresholdForTaxBoost, 100f, 0f, 20f);
			explainedNumber.AddFactor(num * 0.01f, DefaultSettlementLoyaltyModel.LoyaltyText);
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00073BA8 File Offset: 0x00071DA8
		public override void CalculateGoldCutDueToLowLoyalty(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Loyalty, (float)this.ThresholdForHigherTaxCorruption, (float)this.ThresholdForTaxCorruption, 50f, 0f);
			explainedNumber.AddFactor(-1f * num * 0.01f, DefaultSettlementLoyaltyModel.CorruptionText);
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00073BF4 File Offset: 0x00071DF4
		private ExplainedNumber CalculateLoyaltyChangeInternal(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			this.GetSettlementLoyaltyChangeDueToFoodStocks(town, ref result);
			this.GetSettlementLoyaltyChangeDueToGovernorCulture(town, ref result);
			this.GetSettlementLoyaltyChangeDueToOwnerCulture(town, ref result);
			this.GetSettlementLoyaltyChangeDueToPolicies(town, ref result);
			this.GetSettlementLoyaltyChangeDueToProjects(town, ref result);
			this.GetSettlementLoyaltyChangeDueToIssues(town, ref result);
			this.GetSettlementLoyaltyChangeDueToSecurity(town, ref result);
			this.GetSettlementLoyaltyChangeDueToNotableRelations(town, ref result);
			this.GetSettlementLoyaltyChangeDueToGovernorPerks(town, ref result);
			this.GetSettlementLoyaltyChangeDueToLoyaltyDrift(town, ref result);
			return result;
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00073C6C File Offset: 0x00071E6C
		private void GetSettlementLoyaltyChangeDueToGovernorPerks(Town town, ref ExplainedNumber explainedNumber)
		{
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.HeroicLeader, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.PhysicianOfPeople, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Athletics.Durable, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.Discipline, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Riding.WellStraped, town, ref explainedNumber);
			float num = 0f;
			for (int i = 0; i < town.Settlement.Parties.Count; i++)
			{
				MobileParty mobileParty = town.Settlement.Parties[i];
				if (mobileParty.ActualClan == town.OwnerClan)
				{
					if (mobileParty.IsMainParty)
					{
						for (int j = 0; j < mobileParty.MemberRoster.Count; j++)
						{
							CharacterObject characterAtIndex = mobileParty.MemberRoster.GetCharacterAtIndex(j);
							if (characterAtIndex.IsHero && characterAtIndex.HeroObject.GetPerkValue(DefaultPerks.Charm.Parade))
							{
								num += DefaultPerks.Charm.Parade.PrimaryBonus;
							}
						}
					}
					else if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Charm.Parade))
					{
						num += DefaultPerks.Charm.Parade.PrimaryBonus;
					}
				}
			}
			foreach (Hero hero in town.Settlement.HeroesWithoutParty)
			{
				if (hero.Clan == town.OwnerClan && hero.GetPerkValue(DefaultPerks.Charm.Parade))
				{
					num += DefaultPerks.Charm.Parade.PrimaryBonus;
				}
			}
			if (num > 0f)
			{
				explainedNumber.Add(num, DefaultPerks.Charm.Parade.Name, null);
			}
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00073E04 File Offset: 0x00072004
		private void GetSettlementLoyaltyChangeDueToNotableRelations(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = 0f;
			foreach (Hero hero in town.Settlement.Notables)
			{
				if (hero.SupporterOf != null)
				{
					if (hero.SupporterOf == town.Settlement.OwnerClan)
					{
						num += 0.5f;
					}
					else if (town.Settlement.OwnerClan.IsAtWarWith(hero.SupporterOf))
					{
						num += -0.5f;
					}
				}
			}
			if (!num.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				explainedNumber.Add(num, DefaultSettlementLoyaltyModel.NotableText, null);
			}
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00073EC0 File Offset: 0x000720C0
		private void GetSettlementLoyaltyChangeDueToOwnerCulture(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.Settlement.OwnerClan.Culture != town.Settlement.Culture)
			{
				explainedNumber.Add(this.SettlementOwnerDifferentCultureLoyaltyEffect, DefaultSettlementLoyaltyModel.CultureText, null);
			}
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00073EF4 File Offset: 0x000720F4
		private void GetSettlementLoyaltyChangeDueToPolicies(Town town, ref ExplainedNumber explainedNumber)
		{
			Kingdom kingdom = town.Owner.Settlement.OwnerClan.Kingdom;
			if (kingdom != null)
			{
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Citizenship))
				{
					if (town.Settlement.OwnerClan.Culture == town.Settlement.Culture)
					{
						explainedNumber.Add(0.5f, DefaultPolicies.Citizenship.Name, null);
					}
					else
					{
						explainedNumber.Add(-0.5f, DefaultPolicies.Citizenship.Name, null);
					}
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.HuntingRights))
				{
					explainedNumber.Add(-0.2f, DefaultPolicies.HuntingRights.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.GrazingRights))
				{
					explainedNumber.Add(0.5f, DefaultPolicies.GrazingRights.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.TrialByJury))
				{
					explainedNumber.Add(0.5f, DefaultPolicies.TrialByJury.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.ImperialTowns))
				{
					if (kingdom.RulingClan == town.Settlement.OwnerClan)
					{
						explainedNumber.Add(1f, DefaultPolicies.ImperialTowns.Name, null);
					}
					else
					{
						explainedNumber.Add(-0.3f, DefaultPolicies.ImperialTowns.Name, null);
					}
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.ForgivenessOfDebts))
				{
					explainedNumber.Add(2f, DefaultPolicies.ForgivenessOfDebts.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.TribunesOfThePeople))
				{
					explainedNumber.Add(1f, DefaultPolicies.TribunesOfThePeople.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.DebasementOfTheCurrency))
				{
					explainedNumber.Add(-1f, DefaultPolicies.DebasementOfTheCurrency.Name, null);
				}
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x000740BD File Offset: 0x000722BD
		private void GetSettlementLoyaltyChangeDueToGovernorCulture(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.Governor != null)
			{
				explainedNumber.Add((town.Governor.Culture == town.Culture) ? this.GovernorSameCultureLoyaltyEffect : this.GovernorDifferentCultureLoyaltyEffect, DefaultSettlementLoyaltyModel.GovernorCultureText, null);
			}
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x000740F4 File Offset: 0x000722F4
		private void GetSettlementLoyaltyChangeDueToFoodStocks(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.Settlement.IsStarving)
			{
				float num = -1f;
				if (town.Settlement.Party.DaysStarving > 14f)
				{
					num += -1f;
				}
				explainedNumber.Add(num, DefaultSettlementLoyaltyModel.StarvingText, null);
			}
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00074140 File Offset: 0x00072340
		private void GetSettlementLoyaltyChangeDueToSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float value = (town.Security > (float)this.SettlementLoyaltyChangeDueToSecurityThreshold) ? MBMath.Map(town.Security, (float)this.SettlementLoyaltyChangeDueToSecurityThreshold, (float)this.MaximumLoyaltyInSettlement, 0f, this.HighSecurityLoyaltyEffect) : MBMath.Map(town.Security, 0f, (float)this.SettlementLoyaltyChangeDueToSecurityThreshold, this.LowSecurityLoyaltyEffect, 0f);
			explainedNumber.Add(value, DefaultSettlementLoyaltyModel.SecurityText, null);
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x000741B8 File Offset: 0x000723B8
		private void GetSettlementLoyaltyChangeDueToProjects(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.BuildingsInProgress.IsEmpty<Building>())
			{
				BuildingHelper.AddDefaultDailyBonus(town, BuildingEffectEnum.LoyaltyDaily, ref explainedNumber);
			}
			foreach (Building building in town.Buildings)
			{
				float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.Loyalty);
				if (!building.BuildingType.IsDefaultProject && buildingEffectAmount > 0f)
				{
					explainedNumber.Add(buildingEffectAmount, building.Name, null);
				}
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00074248 File Offset: 0x00072448
		private void GetSettlementLoyaltyChangeDueToIssues(Town town, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementLoyalty, town.Settlement, ref explainedNumber);
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0007426A File Offset: 0x0007246A
		private void GetSettlementLoyaltyChangeDueToLoyaltyDrift(Town town, ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(-1f * (town.Loyalty - (float)this.LoyaltyDriftMedium) * 0.1f, DefaultSettlementLoyaltyModel.LoyaltyDriftText, null);
		}

		// Token: 0x04000830 RID: 2096
		private const float StarvationLoyaltyEffect = -1f;

		// Token: 0x04000831 RID: 2097
		private const int AdditionalStarvationLoyaltyEffectAfterDays = 14;

		// Token: 0x04000832 RID: 2098
		private const float NotableSupportsOwnerLoyaltyEffect = 0.5f;

		// Token: 0x04000833 RID: 2099
		private const float NotableSupportsEnemyLoyaltyEffect = -0.5f;

		// Token: 0x04000834 RID: 2100
		private static readonly TextObject StarvingText = GameTexts.FindText("str_starving", null);

		// Token: 0x04000835 RID: 2101
		private static readonly TextObject CultureText = new TextObject("{=YjoXyFDX}Owner Culture", null);

		// Token: 0x04000836 RID: 2102
		private static readonly TextObject NotableText = GameTexts.FindText("str_notable_relations", null);

		// Token: 0x04000837 RID: 2103
		private static readonly TextObject CrimeText = GameTexts.FindText("str_governor_criminal", null);

		// Token: 0x04000838 RID: 2104
		private static readonly TextObject GovernorText = GameTexts.FindText("str_notable_governor", null);

		// Token: 0x04000839 RID: 2105
		private static readonly TextObject GovernorCultureText = new TextObject("{=5Vo8dJub}Governor's Culture", null);

		// Token: 0x0400083A RID: 2106
		private static readonly TextObject NoGovernorText = new TextObject("{=NH5N3kP5}No governor", null);

		// Token: 0x0400083B RID: 2107
		private static readonly TextObject SecurityText = GameTexts.FindText("str_security", null);

		// Token: 0x0400083C RID: 2108
		private static readonly TextObject LoyaltyText = GameTexts.FindText("str_loyalty", null);

		// Token: 0x0400083D RID: 2109
		private static readonly TextObject LoyaltyDriftText = GameTexts.FindText("str_loyalty_drift", null);

		// Token: 0x0400083E RID: 2110
		private static readonly TextObject CorruptionText = GameTexts.FindText("str_corruption", null);
	}
}
