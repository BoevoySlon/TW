using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000137 RID: 311
	public class DefaultSettlementSecurityModel : SettlementSecurityModel
	{
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x00075221 File Offset: 0x00073421
		public override int MaximumSecurityInSettlement
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x00075225 File Offset: 0x00073425
		public override int SecurityDriftMedium
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001785 RID: 6021 RVA: 0x00075229 File Offset: 0x00073429
		public override float MapEventSecurityEffectRadius
		{
			get
			{
				return 50f;
			}
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x00075230 File Offset: 0x00073430
		public override float HideoutClearedSecurityEffectRadius
		{
			get
			{
				return 100f;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x00075237 File Offset: 0x00073437
		public override int HideoutClearedSecurityGain
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x0007523A File Offset: 0x0007343A
		public override int ThresholdForTaxCorruption
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0007523E File Offset: 0x0007343E
		public override int ThresholdForHigherTaxCorruption
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x00075241 File Offset: 0x00073441
		public override int ThresholdForTaxBoost
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x00075245 File Offset: 0x00073445
		public override int SettlementTaxBoostPercentage
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x00075248 File Offset: 0x00073448
		public override int SettlementTaxPenaltyPercentage
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x0007524C File Offset: 0x0007344C
		public override int ThresholdForNotableRelationBonus
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x00075250 File Offset: 0x00073450
		public override int ThresholdForNotableRelationPenalty
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600178F RID: 6031 RVA: 0x00075254 File Offset: 0x00073454
		public override int DailyNotableRelationBonus
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x00075257 File Offset: 0x00073457
		public override int DailyNotableRelationPenalty
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x0007525A File Offset: 0x0007345A
		public override int DailyNotablePowerBonus
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001792 RID: 6034 RVA: 0x0007525D File Offset: 0x0007345D
		public override int DailyNotablePowerPenalty
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x00075260 File Offset: 0x00073460
		public override ExplainedNumber CalculateSecurityChange(Town town, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			this.CalculateInfestedHideoutEffectsOnSecurity(town, ref result);
			this.CalculateRaidedVillageEffectsOnSecurity(town, ref result);
			this.CalculateUnderSiegeEffectsOnSecurity(town, ref result);
			this.CalculateProsperityEffectOnSecurity(town, ref result);
			this.CalculateGarrisonEffectsOnSecurity(town, ref result);
			this.CalculatePolicyEffectsOnSecurity(town, ref result);
			this.CalculateGovernorEffectsOnSecurity(town, ref result);
			this.CalculateProjectEffectsOnSecurity(town, ref result);
			this.CalculateIssueEffectsOnSecurity(town, ref result);
			this.CalculatePerkEffectsOnSecurity(town, ref result);
			this.CalculateSecurityDrift(town, ref result);
			return result;
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000752DF File Offset: 0x000734DF
		private void CalculateProsperityEffectOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(MathF.Max(-5f, -0.0005f * town.Prosperity), DefaultSettlementSecurityModel.ProsperityText, null);
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x00075303 File Offset: 0x00073503
		private void CalculateUnderSiegeEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			if (town.Settlement.IsUnderSiege)
			{
				explainedNumber.Add(-3f, DefaultSettlementSecurityModel.UnderSiegeText, null);
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x00075324 File Offset: 0x00073524
		private void CalculateRaidedVillageEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = 0f;
			using (List<Village>.Enumerator enumerator = town.Settlement.BoundVillages.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.VillageState == Village.VillageStates.Looted)
					{
						num += -2f;
						break;
					}
				}
			}
			explainedNumber.Add(num, DefaultSettlementSecurityModel.LootedVillagesText, null);
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x0007539C File Offset: 0x0007359C
		private void CalculateInfestedHideoutEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = 40f;
			float num2 = num * num;
			int num3 = 0;
			foreach (Hideout hideout in Hideout.All)
			{
				if (hideout.IsInfested && town.Settlement.Position2D.DistanceSquared(hideout.Settlement.Position2D) < num2)
				{
					num3++;
					break;
				}
			}
			if (num3 > 0)
			{
				explainedNumber.Add(-2f, DefaultSettlementSecurityModel.NearbyHideoutText, null);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00075438 File Offset: 0x00073638
		private void CalculateSecurityDrift(Town town, ref ExplainedNumber explainedNumber)
		{
			explainedNumber.Add(-1f * (town.Security - (float)this.SecurityDriftMedium) / 15f, DefaultSettlementSecurityModel.SecurityDriftText, null);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00075460 File Offset: 0x00073660
		private void CalculatePolicyEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			Kingdom kingdom = town.Settlement.OwnerClan.Kingdom;
			if (kingdom != null)
			{
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Bailiffs))
				{
					explainedNumber.Add(1f, DefaultPolicies.Bailiffs.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Magistrates))
				{
					explainedNumber.Add(1f, DefaultPolicies.Magistrates.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.Serfdom) && town.IsTown)
				{
					explainedNumber.Add(1f, DefaultPolicies.Serfdom.Name, null);
				}
				if (kingdom.ActivePolicies.Contains(DefaultPolicies.TrialByJury))
				{
					explainedNumber.Add(-0.2f, DefaultPolicies.TrialByJury.Name, null);
				}
			}
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0007552C File Offset: 0x0007372C
		private void CalculateGovernorEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00075530 File Offset: 0x00073730
		private void CalculateGarrisonEffectsOnSecurity(Town town, ref ExplainedNumber result)
		{
			if (town.GarrisonParty != null && town.GarrisonParty.MemberRoster.Count != 0 && town.GarrisonParty.MemberRoster.TotalHealthyCount != 0)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(0.01f, false, null);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.StandUnited, town, ref explainedNumber);
				float num;
				float num2;
				float num3;
				this.CalculateStrengthOfGarrisonParty(town.GarrisonParty.Party, out num, out num2, out num3);
				float num4 = num * explainedNumber.ResultNumber;
				result.Add(num4, DefaultSettlementSecurityModel.GarrisonText, null);
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Leadership.Authority, town))
				{
					result.Add(num4 * DefaultPerks.Leadership.Authority.PrimaryBonus, DefaultPerks.Leadership.Authority.Name, null);
				}
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Riding.ReliefForce, town))
				{
					float num5 = num3 / num;
					result.Add(num4 * num5 * DefaultPerks.Riding.ReliefForce.SecondaryBonus, DefaultPerks.Riding.ReliefForce.Name, null);
				}
				float num6 = num2 / num;
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Bow.MountedArchery, town))
				{
					result.Add(num4 * num6 * DefaultPerks.Bow.MountedArchery.SecondaryBonus, DefaultPerks.Bow.MountedArchery.Name, null);
				}
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Bow.RangersSwiftness, town))
				{
					result.Add(num4 * num6 * DefaultPerks.Bow.RangersSwiftness.SecondaryBonus, DefaultPerks.Bow.RangersSwiftness.Name, null);
				}
				if (PerkHelper.GetPerkValueForTown(DefaultPerks.Crossbow.RenownMarksmen, town))
				{
					result.Add(num4 * num6 * DefaultPerks.Crossbow.RenownMarksmen.SecondaryBonus, DefaultPerks.Crossbow.RenownMarksmen.Name, null);
				}
			}
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x000756AC File Offset: 0x000738AC
		private void CalculateStrengthOfGarrisonParty(PartyBase party, out float totalStrength, out float archerStrength, out float cavalryStrength)
		{
			totalStrength = 0f;
			archerStrength = 0f;
			cavalryStrength = 0f;
			float leaderModifier = 0f;
			MapEvent.PowerCalculationContext context = MapEvent.PowerCalculationContext.Default;
			BattleSideEnum battleSideEnum = BattleSideEnum.Defender;
			if (party.MapEvent != null)
			{
				battleSideEnum = party.Side;
				leaderModifier = Campaign.Current.Models.MilitaryPowerModel.GetLeaderModifierInMapEvent(party.MapEvent, battleSideEnum);
				context = party.MapEvent.SimulationContext;
			}
			for (int i = 0; i < party.MemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Character != null)
				{
					float troopPower = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(elementCopyAtIndex.Character, battleSideEnum, context, leaderModifier);
					float num = (float)(elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber) * troopPower;
					if (elementCopyAtIndex.Character.IsMounted)
					{
						cavalryStrength += num;
					}
					if (elementCopyAtIndex.Character.IsRanged)
					{
						archerStrength += num;
					}
					totalStrength += num;
				}
			}
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x000757AC File Offset: 0x000739AC
		private void CalculatePerkEffectsOnSecurity(Town town, ref ExplainedNumber result)
		{
			float num = (float)town.Settlement.Parties.Where(delegate(MobileParty x)
			{
				Clan actualClan = x.ActualClan;
				if (actualClan != null && !actualClan.IsAtWarWith(town.MapFaction))
				{
					Hero leaderHero = x.LeaderHero;
					return leaderHero != null && leaderHero.GetPerkValue(DefaultPerks.Leadership.Presence);
				}
				return false;
			}).Count<MobileParty>() * DefaultPerks.Leadership.Presence.PrimaryBonus;
			if (num > 0f)
			{
				result.Add(num, DefaultPerks.Leadership.Presence.Name, null);
			}
			if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Roguery.KnowHow))
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Roguery.KnowHow, town, ref result);
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.ToBeBlunt, town, ref result);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Throwing.Focus, town, ref result);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.Skewer, town, ref result);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Tactics.Gensdarmes, town, ref result);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0007588C File Offset: 0x00073A8C
		private void CalculateProjectEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0007588E File Offset: 0x00073A8E
		private void CalculateIssueEffectsOnSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectsOfSettlement(DefaultIssueEffects.SettlementSecurity, town.Settlement, ref explainedNumber);
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000758B0 File Offset: 0x00073AB0
		public override float GetLootedNearbyPartySecurityEffect(Town town, float sumOfAttackedPartyStrengths)
		{
			return -1f * sumOfAttackedPartyStrengths * 0.005f;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x000758BF File Offset: 0x00073ABF
		public override float GetNearbyBanditPartyDefeatedSecurityEffect(Town town, float sumOfAttackedPartyStrengths)
		{
			return sumOfAttackedPartyStrengths * 0.005f;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000758C8 File Offset: 0x00073AC8
		public override void CalculateGoldGainDueToHighSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Security, (float)this.ThresholdForTaxBoost, (float)this.MaximumSecurityInSettlement, 0f, (float)this.SettlementTaxBoostPercentage);
			explainedNumber.AddFactor(num * 0.01f, DefaultSettlementSecurityModel.Security);
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00075910 File Offset: 0x00073B10
		public override void CalculateGoldCutDueToLowSecurity(Town town, ref ExplainedNumber explainedNumber)
		{
			float num = MBMath.Map(town.Security, (float)this.ThresholdForHigherTaxCorruption, (float)this.ThresholdForTaxCorruption, (float)this.SettlementTaxPenaltyPercentage, 0f);
			explainedNumber.AddFactor(-1f * num * 0.01f, DefaultSettlementSecurityModel.CorruptionText);
		}

		// Token: 0x0400084F RID: 2127
		private const float GarrisonHighSecurityGain = 3f;

		// Token: 0x04000850 RID: 2128
		private const float GarrisonLowSecurityPenalty = -3f;

		// Token: 0x04000851 RID: 2129
		private const float NearbyHideoutPenalty = -2f;

		// Token: 0x04000852 RID: 2130
		private const float VillageLootedSecurityEffect = -2f;

		// Token: 0x04000853 RID: 2131
		private const float UnderSiegeSecurityEffect = -3f;

		// Token: 0x04000854 RID: 2132
		private const float MaxProsperityEffect = -5f;

		// Token: 0x04000855 RID: 2133
		private const float PerProsperityEffect = -0.0005f;

		// Token: 0x04000856 RID: 2134
		private static readonly TextObject GarrisonText = GameTexts.FindText("str_garrison", null);

		// Token: 0x04000857 RID: 2135
		private static readonly TextObject LootedVillagesText = GameTexts.FindText("str_looted_villages", null);

		// Token: 0x04000858 RID: 2136
		private static readonly TextObject CorruptionText = GameTexts.FindText("str_corruption", null);

		// Token: 0x04000859 RID: 2137
		private static readonly TextObject NearbyHideoutText = GameTexts.FindText("str_nearby_hideout", null);

		// Token: 0x0400085A RID: 2138
		private static readonly TextObject UnderSiegeText = GameTexts.FindText("str_under_siege", null);

		// Token: 0x0400085B RID: 2139
		private static readonly TextObject ProsperityText = GameTexts.FindText("str_prosperity", null);

		// Token: 0x0400085C RID: 2140
		private static readonly TextObject Security = GameTexts.FindText("str_security", null);

		// Token: 0x0400085D RID: 2141
		private static readonly TextObject SecurityDriftText = GameTexts.FindText("str_security_drift", null);
	}
}
