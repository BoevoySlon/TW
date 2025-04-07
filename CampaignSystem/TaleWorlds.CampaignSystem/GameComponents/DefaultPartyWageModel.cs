using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000126 RID: 294
	public class DefaultPartyWageModel : PartyWageModel
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x000704A9 File Offset: 0x0006E6A9
		public override int MaxWage
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x000704B0 File Offset: 0x0006E6B0
		public override int GetCharacterWage(CharacterObject character)
		{
			int num;
			switch (character.Tier)
			{
			case 0:
				num = 1;
				break;
			case 1:
				num = 2;
				break;
			case 2:
				num = 3;
				break;
			case 3:
				num = 5;
				break;
			case 4:
				num = 8;
				break;
			case 5:
				num = 12;
				break;
			case 6:
				num = 17;
				break;
			default:
				num = 23;
				break;
			}
			if (character.Occupation == Occupation.Mercenary)
			{
				num = (int)((float)num * 1.5f);
			}
			return num;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x00070520 File Offset: 0x0006E720
		public override ExplainedNumber GetTotalWage(MobileParty mobileParty, bool includeDescriptions = false)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			bool flag = !mobileParty.HasPerk(DefaultPerks.Steward.AidCorps, false);
			int num12 = 0;
			int num13 = 0;
			for (int i = 0; i < mobileParty.MemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = mobileParty.MemberRoster.GetElementCopyAtIndex(i);
				CharacterObject character = elementCopyAtIndex.Character;
				int num14 = flag ? elementCopyAtIndex.Number : (elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber);
				if (character.IsHero)
				{
					Hero heroObject = elementCopyAtIndex.Character.HeroObject;
					Clan clan = character.HeroObject.Clan;
					if (heroObject != ((clan != null) ? clan.Leader : null))
					{
						if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Steward.PaidInPromise))
						{
							num3 += MathF.Round((float)elementCopyAtIndex.Character.TroopWage * (1f + DefaultPerks.Steward.PaidInPromise.PrimaryBonus));
						}
						else
						{
							num3 += elementCopyAtIndex.Character.TroopWage;
						}
					}
				}
				else
				{
					if (character.Tier < 4)
					{
						if (character.Culture.IsBandit)
						{
							num9 += elementCopyAtIndex.Character.TroopWage * elementCopyAtIndex.Number;
						}
						num += elementCopyAtIndex.Character.TroopWage * num14;
					}
					else if (character.Tier == 4)
					{
						if (character.Culture.IsBandit)
						{
							num10 += elementCopyAtIndex.Character.TroopWage * elementCopyAtIndex.Number;
						}
						num2 += elementCopyAtIndex.Character.TroopWage * num14;
					}
					else if (character.Tier > 4)
					{
						if (character.Culture.IsBandit)
						{
							num11 += elementCopyAtIndex.Character.TroopWage * elementCopyAtIndex.Number;
						}
						num3 += elementCopyAtIndex.Character.TroopWage * num14;
					}
					if (character.IsInfantry)
					{
						num4 += num14;
					}
					if (character.IsMounted)
					{
						num5 += num14;
					}
					if (character.Occupation == Occupation.CaravanGuard)
					{
						num12 += elementCopyAtIndex.Number;
					}
					if (character.Occupation == Occupation.Mercenary)
					{
						num13 += elementCopyAtIndex.Number;
					}
					if (character.IsRanged)
					{
						num6 += num14;
						if (character.Tier >= 4)
						{
							num7 += num14;
							num8 += elementCopyAtIndex.Character.TroopWage * elementCopyAtIndex.Number;
						}
					}
				}
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Roguery.DeepPockets))
			{
				num -= num9;
				num2 -= num10;
				num3 -= num11;
				int num15 = num9 + num10 + num11;
				explainedNumber.Add((float)num15, null, null);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.DeepPockets, mobileParty.LeaderHero.CharacterObject, false, ref explainedNumber);
			}
			int num16 = num + num2 + num3;
			if (mobileParty.HasPerk(DefaultPerks.Crossbow.PickedShots, false) && num7 > 0)
			{
				float num17 = (float)num8 * DefaultPerks.Crossbow.PickedShots.PrimaryBonus;
				num16 += (int)num17;
			}
			ExplainedNumber result = new ExplainedNumber((float)num16, includeDescriptions, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(1f, false, null);
			if (mobileParty.IsGarrison)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) != null)
				{
					if (mobileParty.CurrentSettlement.IsTown)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.MilitaryTradition, mobileParty.CurrentSettlement.Town, ref result);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Berserker, mobileParty.CurrentSettlement.Town, ref result);
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Bow.HunterClan, mobileParty.CurrentSettlement.Town, ref result);
						float troopRatio = (float)num4 / (float)mobileParty.MemberRoster.TotalRegulars;
						this.CalculatePartialGarrisonWageReduction(troopRatio, mobileParty, DefaultPerks.Polearm.StandardBearer, ref result, true);
						float troopRatio2 = (float)num5 / (float)mobileParty.MemberRoster.TotalRegulars;
						this.CalculatePartialGarrisonWageReduction(troopRatio2, mobileParty, DefaultPerks.Riding.CavalryTactics, ref result, true);
						float troopRatio3 = (float)num6 / (float)mobileParty.MemberRoster.TotalRegulars;
						this.CalculatePartialGarrisonWageReduction(troopRatio3, mobileParty, DefaultPerks.Crossbow.PeasantLeader, ref result, true);
					}
					else if (mobileParty.CurrentSettlement.IsCastle)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.StiffUpperLip, mobileParty.CurrentSettlement.Town, ref result);
					}
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.DrillSergant, mobileParty.CurrentSettlement.Town, ref result);
					if (mobileParty.CurrentSettlement.Culture.HasFeat(DefaultCulturalFeats.EmpireGarrisonWageFeat))
					{
						result.AddFactor(DefaultCulturalFeats.EmpireGarrisonWageFeat.EffectBonus, GameTexts.FindText("str_culture", null));
					}
					foreach (Building building in mobileParty.CurrentSettlement.Town.Buildings)
					{
						float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.GarrisonWageReduce);
						if (buildingEffectAmount > 0f)
						{
							explainedNumber2.AddFactor(-(buildingEffectAmount / 100f), building.Name);
						}
					}
				}
			}
			result.Add(explainedNumber.ResultNumber, null, null);
			float value = (mobileParty.LeaderHero != null && mobileParty.LeaderHero.Clan.Kingdom != null && !mobileParty.LeaderHero.Clan.IsUnderMercenaryService && mobileParty.LeaderHero.Clan.Kingdom.ActivePolicies.Contains(DefaultPolicies.MilitaryCoronae)) ? 0.1f : 0f;
			if (mobileParty.HasPerk(DefaultPerks.Trade.SwordForBarter, true))
			{
				float num18 = (float)num12 / (float)mobileParty.MemberRoster.TotalRegulars;
				if (num18 > 0f)
				{
					float value2 = DefaultPerks.Trade.SwordForBarter.SecondaryBonus * num18;
					result.AddFactor(value2, DefaultPerks.Trade.SwordForBarter.Name);
				}
			}
			if (mobileParty.HasPerk(DefaultPerks.Steward.Contractors, false))
			{
				float num19 = (float)num13 / (float)mobileParty.MemberRoster.TotalRegulars;
				if (num19 > 0f)
				{
					float value3 = DefaultPerks.Steward.Contractors.PrimaryBonus * num19;
					result.AddFactor(value3, DefaultPerks.Steward.Contractors.Name);
				}
			}
			if (mobileParty.HasPerk(DefaultPerks.Trade.MercenaryConnections, true))
			{
				float num20 = (float)num13 / (float)mobileParty.MemberRoster.TotalRegulars;
				if (num20 > 0f)
				{
					float value4 = DefaultPerks.Trade.MercenaryConnections.SecondaryBonus * num20;
					result.AddFactor(value4, DefaultPerks.Trade.MercenaryConnections.Name);
				}
			}
			result.AddFactor(value, DefaultPolicies.MilitaryCoronae.Name);
			result.AddFactor(explainedNumber2.ResultNumber - 1f, DefaultPartyWageModel._buildingEffects);
			if (PartyBaseHelper.HasFeat(mobileParty.Party, DefaultCulturalFeats.AseraiIncreasedWageFeat))
			{
				result.AddFactor(DefaultCulturalFeats.AseraiIncreasedWageFeat.EffectBonus, DefaultPartyWageModel._cultureText);
			}
			if (mobileParty.HasPerk(DefaultPerks.Steward.Frugal, false))
			{
				result.AddFactor(DefaultPerks.Steward.Frugal.PrimaryBonus, DefaultPerks.Steward.Frugal.Name);
			}
			if (mobileParty.Army != null && mobileParty.HasPerk(DefaultPerks.Steward.EfficientCampaigner, true))
			{
				result.AddFactor(DefaultPerks.Steward.EfficientCampaigner.SecondaryBonus, DefaultPerks.Steward.EfficientCampaigner.Name);
			}
			if (mobileParty.SiegeEvent != null && mobileParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(mobileParty.Party, MapEvent.BattleTypes.Siege) && mobileParty.HasPerk(DefaultPerks.Steward.MasterOfWarcraft, false))
			{
				result.AddFactor(DefaultPerks.Steward.MasterOfWarcraft.PrimaryBonus, DefaultPerks.Steward.MasterOfWarcraft.Name);
			}
			if (mobileParty.EffectiveQuartermaster != null)
			{
				PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, mobileParty.EffectiveQuartermaster.CharacterObject, DefaultSkills.Steward, true, ref result, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
			}
			if (mobileParty.CurrentSettlement != null && mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Trade.ContentTrades))
			{
				result.AddFactor(DefaultPerks.Trade.ContentTrades.SecondaryBonus, DefaultPerks.Trade.ContentTrades.Name);
			}
			return result;
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00070CD0 File Offset: 0x0006EED0
		private void CalculatePartialGarrisonWageReduction(float troopRatio, MobileParty mobileParty, PerkObject perk, ref ExplainedNumber garrisonWageReductionMultiplier, bool isSecondaryEffect)
		{
			if (troopRatio > 0f && mobileParty.CurrentSettlement.Town.Governor != null && PerkHelper.GetPerkValueForTown(perk, mobileParty.CurrentSettlement.Town))
			{
				garrisonWageReductionMultiplier.AddFactor(isSecondaryEffect ? (perk.SecondaryBonus * troopRatio) : (perk.PrimaryBonus * troopRatio), perk.Name);
			}
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00070D30 File Offset: 0x0006EF30
		public override int GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
		{
			int num = 10 * MathF.Round((float)troop.Level * MathF.Pow((float)troop.Level, 0.65f) * 0.2f);
			if (troop.Level <= 1)
			{
				num = 10;
			}
			else if (troop.Level <= 6)
			{
				num = 20;
			}
			else if (troop.Level <= 11)
			{
				num = 50;
			}
			else if (troop.Level <= 16)
			{
				num = 100;
			}
			else if (troop.Level <= 21)
			{
				num = 200;
			}
			else if (troop.Level <= 26)
			{
				num = 400;
			}
			else if (troop.Level <= 31)
			{
				num = 600;
			}
			else if (troop.Level <= 36)
			{
				num = 1000;
			}
			else
			{
				num = 1500;
			}
			if (troop.Equipment.Horse.Item != null && !withoutItemCost)
			{
				if (troop.Level < 26)
				{
					num += 150;
				}
				else
				{
					num += 500;
				}
			}
			bool flag = troop.Occupation == Occupation.Mercenary || troop.Occupation == Occupation.Gangster || troop.Occupation == Occupation.CaravanGuard;
			if (flag)
			{
				num = MathF.Round((float)num * 2f);
			}
			if (buyerHero != null)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
				if (troop.Tier >= 2 && buyerHero.GetPerkValue(DefaultPerks.Throwing.HeadHunter))
				{
					explainedNumber.AddFactor(DefaultPerks.Throwing.HeadHunter.SecondaryBonus, null);
				}
				if (troop.IsInfantry)
				{
					if (buyerHero.GetPerkValue(DefaultPerks.OneHanded.ChinkInTheArmor))
					{
						explainedNumber.AddFactor(DefaultPerks.OneHanded.ChinkInTheArmor.SecondaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.TwoHanded.ShowOfStrength))
					{
						explainedNumber.AddFactor(DefaultPerks.TwoHanded.ShowOfStrength.SecondaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.Polearm.HardyFrontline))
					{
						explainedNumber.AddFactor(DefaultPerks.Polearm.HardyFrontline.SecondaryBonus, null);
					}
					if (buyerHero.Culture.HasFeat(DefaultCulturalFeats.SturgianRecruitUpgradeFeat))
					{
						explainedNumber.AddFactor(DefaultCulturalFeats.SturgianRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
					}
				}
				else if (troop.IsRanged)
				{
					if (buyerHero.GetPerkValue(DefaultPerks.Bow.RenownedArcher))
					{
						explainedNumber.AddFactor(DefaultPerks.Bow.RenownedArcher.SecondaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.Crossbow.Piercer))
					{
						explainedNumber.AddFactor(DefaultPerks.Crossbow.Piercer.SecondaryBonus, null);
					}
				}
				if (troop.IsMounted && buyerHero.Culture.HasFeat(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat))
				{
					explainedNumber.AddFactor(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
				}
				if (buyerHero.IsPartyLeader && buyerHero.GetPerkValue(DefaultPerks.Steward.Frugal))
				{
					explainedNumber.AddFactor(DefaultPerks.Steward.Frugal.SecondaryBonus, null);
				}
				if (flag)
				{
					if (buyerHero.GetPerkValue(DefaultPerks.Trade.SwordForBarter))
					{
						explainedNumber.AddFactor(DefaultPerks.Trade.SwordForBarter.PrimaryBonus, null);
					}
					if (buyerHero.GetPerkValue(DefaultPerks.Charm.SlickNegotiator))
					{
						explainedNumber.AddFactor(DefaultPerks.Charm.SlickNegotiator.PrimaryBonus, null);
					}
				}
				num = MathF.Max(1, MathF.Round((float)num * explainedNumber.ResultNumber));
			}
			return num;
		}

		// Token: 0x04000810 RID: 2064
		private static readonly TextObject _cultureText = GameTexts.FindText("str_culture", null);

		// Token: 0x04000811 RID: 2065
		private static readonly TextObject _buildingEffects = GameTexts.FindText("str_building_effects", null);

		// Token: 0x04000812 RID: 2066
		private const float MercenaryWageFactor = 1.5f;
	}
}
