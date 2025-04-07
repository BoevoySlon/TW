using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000EA RID: 234
	public class DefaultArmyManagementCalculationModel : ArmyManagementCalculationModel
	{
		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x0005BAA5 File Offset: 0x00059CA5
		public override int InfluenceValuePerGold
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0005BAA9 File Offset: 0x00059CA9
		public override int AverageCallToArmyCost
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0005BAAD File Offset: 0x00059CAD
		public override int CohesionThresholdForDispersion
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x0005BAB4 File Offset: 0x00059CB4
		public override float DailyBeingAtArmyInfluenceAward(MobileParty armyMemberParty)
		{
			float num = (armyMemberParty.Party.TotalStrength + 20f) / 200f;
			if (PartyBaseHelper.HasFeat(armyMemberParty.Party, DefaultCulturalFeats.EmpireArmyInfluenceFeat))
			{
				num += num * DefaultCulturalFeats.EmpireArmyInfluenceFeat.EffectBonus;
			}
			return num;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x0005BAFC File Offset: 0x00059CFC
		public override int CalculatePartyInfluenceCost(MobileParty armyLeaderParty, MobileParty party)
		{
			if (armyLeaderParty.LeaderHero != null && party.LeaderHero != null && armyLeaderParty.LeaderHero.Clan == party.LeaderHero.Clan)
			{
				return 0;
			}
			float num = (float)armyLeaderParty.LeaderHero.GetRelation(party.LeaderHero);
			float partySizeScore = this.GetPartySizeScore(party);
			float b = (float)MathF.Round(party.Party.TotalStrength);
			float num2 = (num < 0f) ? (1f + MathF.Sqrt(MathF.Abs(MathF.Max(-100f, num))) / 10f) : (1f - MathF.Sqrt(MathF.Abs(MathF.Min(100f, num))) / 20f);
			float num3 = 0.5f + MathF.Min(1000f, b) / 100f;
			float num4 = 0.5f + 1f * (1f - (partySizeScore - this._minimumPartySizeScoreNeeded) / (1f - this._minimumPartySizeScoreNeeded));
			float num5 = 1f + 1f * MathF.Pow(MathF.Min(Campaign.MapDiagonal * 10f, MathF.Max(1f, Campaign.Current.Models.MapDistanceModel.GetDistance(armyLeaderParty, party)) / Campaign.MapDiagonal), 0.67f);
			float num6 = (party.LeaderHero != null) ? party.LeaderHero.RandomFloat(0.75f, 1.25f) : 1f;
			float num7 = 1f;
			float num8 = 1f;
			float num9 = 1f;
			Hero leaderHero = armyLeaderParty.LeaderHero;
			if (((leaderHero != null) ? leaderHero.Clan.Kingdom : null) != null)
			{
				if (armyLeaderParty.LeaderHero.Clan.Tier >= 5 && armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Marshals))
				{
					num7 -= 0.1f;
				}
				if (armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.RoyalCommissions))
				{
					if (armyLeaderParty.LeaderHero == armyLeaderParty.LeaderHero.Clan.Kingdom.Leader)
					{
						num7 -= 0.3f;
					}
					else
					{
						num7 += 0.1f;
					}
				}
				if (party.LeaderHero != null)
				{
					if (armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.LordsPrivyCouncil) && party.LeaderHero.Clan.Tier <= 4)
					{
						num7 += 0.2f;
					}
					if (armyLeaderParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.Senate) && party.LeaderHero.Clan.Tier <= 2)
					{
						num7 += 0.1f;
					}
				}
				if (armyLeaderParty.LeaderHero.GetPerkValue(DefaultPerks.Leadership.InspiringLeader))
				{
					num8 += DefaultPerks.Leadership.InspiringLeader.PrimaryBonus;
				}
				if (armyLeaderParty.LeaderHero.GetPerkValue(DefaultPerks.Tactics.CallToArms))
				{
					num8 += DefaultPerks.Tactics.CallToArms.SecondaryBonus;
				}
			}
			if (PartyBaseHelper.HasFeat(armyLeaderParty.Party, DefaultCulturalFeats.VlandianArmyInfluenceFeat))
			{
				num9 += DefaultCulturalFeats.VlandianArmyInfluenceFeat.EffectBonus;
			}
			return (int)(0.65f * num2 * num3 * num6 * num5 * num4 * num7 * num8 * num9 * (float)this.AverageCallToArmyCost);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x0005BE34 File Offset: 0x0005A034
		public override List<MobileParty> GetMobilePartiesToCallToArmy(MobileParty leaderParty)
		{
			List<MobileParty> list = new List<MobileParty>();
			bool flag = false;
			bool flag2 = false;
			if (leaderParty.LeaderHero != null)
			{
				foreach (Settlement settlement in leaderParty.MapFaction.Settlements)
				{
					if (settlement.IsFortification && settlement.SiegeEvent != null)
					{
						flag = true;
						if (settlement.OwnerClan == leaderParty.LeaderHero.Clan)
						{
							flag2 = true;
						}
					}
				}
			}
			int b = (leaderParty.MapFaction.IsKingdomFaction && (Kingdom)leaderParty.MapFaction != null) ? ((Kingdom)leaderParty.MapFaction).Armies.Count : 0;
			float num = (0.55f - (float)MathF.Min(2, b) * 0.05f - ((Hero.MainHero.MapFaction == leaderParty.MapFaction) ? 0.05f : 0f)) * (1f - 0.5f * MathF.Sqrt(MathF.Min(leaderParty.LeaderHero.Clan.Influence, 900f)) * 0.033333335f);
			num *= (flag2 ? 1.25f : 1f);
			num *= (flag ? 1.125f : 1f);
			num *= leaderParty.LeaderHero.RandomFloat(0.85f, 1f);
			float num2 = MathF.Min(leaderParty.LeaderHero.Clan.Influence, 900f) * MathF.Min(1f, num);
			List<ValueTuple<MobileParty, float>> list2 = new List<ValueTuple<MobileParty, float>>();
			foreach (WarPartyComponent warPartyComponent in leaderParty.MapFaction.WarPartyComponents)
			{
				MobileParty mobileParty = warPartyComponent.MobileParty;
				Hero leaderHero = mobileParty.LeaderHero;
				if (mobileParty.IsLordParty && mobileParty.Army == null && mobileParty != leaderParty && leaderHero != null && !mobileParty.IsMainParty && leaderHero != leaderHero.MapFaction.Leader && !mobileParty.Ai.DoNotMakeNewDecisions)
				{
					Settlement currentSettlement = mobileParty.CurrentSettlement;
					if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) == null && !mobileParty.IsDisbanding && mobileParty.Food > -(mobileParty.FoodChange * 5f) && mobileParty.PartySizeRatio > 0.6f && leaderHero.CanLeadParty() && mobileParty.MapEvent == null && mobileParty.BesiegedSettlement == null)
					{
						IDisbandPartyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
						if (campaignBehavior == null || !campaignBehavior.IsPartyWaitingForDisband(mobileParty))
						{
							bool flag3 = false;
							using (List<ValueTuple<MobileParty, float>>.Enumerator enumerator3 = list2.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									if (enumerator3.Current.Item1 == mobileParty)
									{
										flag3 = true;
										break;
									}
								}
							}
							if (!flag3)
							{
								int num3 = Campaign.Current.Models.ArmyManagementCalculationModel.CalculatePartyInfluenceCost(leaderParty, mobileParty);
								float totalStrength = mobileParty.Party.TotalStrength;
								float num4 = 1f - (float)mobileParty.Party.MemberRoster.TotalWounded / (float)mobileParty.Party.MemberRoster.TotalManCount;
								float item = totalStrength / ((float)num3 + 0.1f) * num4;
								list2.Add(new ValueTuple<MobileParty, float>(mobileParty, item));
							}
						}
					}
				}
			}
			int num6;
			do
			{
				float num5 = 0.01f;
				num6 = -1;
				for (int i = 0; i < list2.Count; i++)
				{
					ValueTuple<MobileParty, float> valueTuple = list2[i];
					if (valueTuple.Item2 > num5)
					{
						num6 = i;
						num5 = valueTuple.Item2;
					}
				}
				if (num6 >= 0)
				{
					MobileParty item2 = list2[num6].Item1;
					int num7 = Campaign.Current.Models.ArmyManagementCalculationModel.CalculatePartyInfluenceCost(leaderParty, item2);
					list2[num6] = new ValueTuple<MobileParty, float>(item2, 0f);
					if (num2 > (float)num7)
					{
						num2 -= (float)num7;
						list.Add(item2);
					}
				}
			}
			while (num6 >= 0);
			return list;
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x0005C298 File Offset: 0x0005A498
		public override int CalculateTotalInfluenceCost(Army army, float percentage)
		{
			int num = 0;
			foreach (MobileParty party in from p in army.Parties
			where !p.IsMainParty
			select p)
			{
				num += this.CalculatePartyInfluenceCost(army.LeaderParty, party);
			}
			ExplainedNumber explainedNumber = new ExplainedNumber((float)num, false, null);
			if (army.LeaderParty.MapFaction.IsKingdomFaction && ((Kingdom)army.LeaderParty.MapFaction).ActivePolicies.Contains(DefaultPolicies.RoyalCommissions))
			{
				explainedNumber.AddFactor(-0.3f, null);
			}
			if (army.LeaderParty.LeaderHero.GetPerkValue(DefaultPerks.Tactics.Encirclement))
			{
				explainedNumber.AddFactor(DefaultPerks.Tactics.Encirclement.SecondaryBonus, null);
			}
			return MathF.Ceiling(explainedNumber.ResultNumber * percentage / 100f);
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0005C39C File Offset: 0x0005A59C
		public override float GetPartySizeScore(MobileParty party)
		{
			return MathF.Min(1f, party.PartySizeRatio);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x0005C3B0 File Offset: 0x0005A5B0
		public override ExplainedNumber CalculateDailyCohesionChange(Army army, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(-2f, includeDescriptions, null);
			this.CalculateCohesionChangeInternal(army, ref result);
			if (army.LeaderParty.HasPerk(DefaultPerks.Tactics.HordeLeader, true))
			{
				result.AddFactor(DefaultPerks.Tactics.HordeLeader.SecondaryBonus, DefaultPerks.Tactics.HordeLeader.Name);
			}
			SiegeEvent siegeEvent = army.LeaderParty.SiegeEvent;
			if (siegeEvent != null && siegeEvent.BesiegerCamp.IsBesiegerSideParty(army.LeaderParty) && army.LeaderParty.HasPerk(DefaultPerks.Engineering.CampBuilding, false))
			{
				result.AddFactor(DefaultPerks.Engineering.CampBuilding.PrimaryBonus, DefaultPerks.Engineering.CampBuilding.Name);
			}
			MobileParty leaderParty = army.LeaderParty;
			if (PartyBaseHelper.HasFeat((leaderParty != null) ? leaderParty.Party : null, DefaultCulturalFeats.SturgianArmyCohesionFeat))
			{
				result.AddFactor(DefaultCulturalFeats.SturgianArmyCohesionFeat.EffectBonus, GameTexts.FindText("str_culture", null));
			}
			return result;
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0005C490 File Offset: 0x0005A690
		private void CalculateCohesionChangeInternal(Army army, ref ExplainedNumber cohesionChange)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (MobileParty mobileParty in army.LeaderParty.AttachedParties)
			{
				if (mobileParty.Party.IsStarving)
				{
					num++;
				}
				if (mobileParty.Morale <= 25f)
				{
					num2++;
				}
				if (mobileParty.Party.NumberOfHealthyMembers <= 10)
				{
					num3++;
				}
				num4++;
			}
			cohesionChange.Add((float)(-(float)num4), DefaultArmyManagementCalculationModel._numberOfPartiesText, null);
			cohesionChange.Add((float)(-(float)((num + 1) / 2)), DefaultArmyManagementCalculationModel._numberOfStarvingPartiesText, null);
			cohesionChange.Add((float)(-(float)((num2 + 1) / 2)), DefaultArmyManagementCalculationModel._numberOfLowMoralePartiesText, null);
			cohesionChange.Add((float)(-(float)((num3 + 1) / 2)), DefaultArmyManagementCalculationModel._numberOfLessMemberPartiesText, null);
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0005C56C File Offset: 0x0005A76C
		public override int CalculateNewCohesion(Army army, PartyBase newParty, int calculatedCohesion, int sign)
		{
			if (army == null)
			{
				return calculatedCohesion;
			}
			sign = MathF.Sign(sign);
			int num = (sign == 1) ? (army.Parties.Count - 1) : army.Parties.Count;
			int num2 = (calculatedCohesion * num + 100 * sign) / (num + sign);
			if (num2 > 100)
			{
				return 100;
			}
			if (num2 >= 0)
			{
				return num2;
			}
			return 0;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x0005C5C5 File Offset: 0x0005A7C5
		public override int GetCohesionBoostInfluenceCost(Army army, int percentageToBoost = 100)
		{
			return this.CalculateTotalInfluenceCost(army, (float)percentageToBoost);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x0005C5D0 File Offset: 0x0005A7D0
		public override int GetCohesionBoostGoldCost(Army army, float percentageToBoost = 100f)
		{
			return this.CalculateTotalInfluenceCost(army, percentageToBoost) * this.InfluenceValuePerGold;
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0005C5E1 File Offset: 0x0005A7E1
		public override int GetPartyRelation(Hero hero)
		{
			if (hero == null)
			{
				return -101;
			}
			if (hero == Hero.MainHero)
			{
				return 101;
			}
			return Hero.MainHero.GetRelation(hero);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0005C5FF File Offset: 0x0005A7FF
		public override int GetPartyStrength(PartyBase party)
		{
			return MathF.Round(party.TotalStrength);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0005C60C File Offset: 0x0005A80C
		public override bool CheckPartyEligibility(MobileParty party)
		{
			return party.Army == null && this.GetPartySizeScore(party) > this._minimumPartySizeScoreNeeded && party.MapEvent == null && party.SiegeEvent == null;
		}

		// Token: 0x04000714 RID: 1812
		private const float MobilePartySizeRatioToCallToArmy = 0.6f;

		// Token: 0x04000715 RID: 1813
		private const float MinimumNeededFoodInDaysToCallToArmy = 5f;

		// Token: 0x04000716 RID: 1814
		private static readonly TextObject _numberOfPartiesText = GameTexts.FindText("str_number_of_parties", null);

		// Token: 0x04000717 RID: 1815
		private static readonly TextObject _numberOfStarvingPartiesText = GameTexts.FindText("str_number_of_starving_parties", null);

		// Token: 0x04000718 RID: 1816
		private static readonly TextObject _numberOfLowMoralePartiesText = GameTexts.FindText("str_number_of_low_morale_parties", null);

		// Token: 0x04000719 RID: 1817
		private static readonly TextObject _numberOfLessMemberPartiesText = GameTexts.FindText("str_number_of_less_member_parties", null);

		// Token: 0x0400071A RID: 1818
		private float _minimumPartySizeScoreNeeded = 0.4f;
	}
}
