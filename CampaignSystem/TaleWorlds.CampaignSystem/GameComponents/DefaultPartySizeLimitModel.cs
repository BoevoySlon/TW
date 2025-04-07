using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000121 RID: 289
	public class DefaultPartySizeLimitModel : PartySizeLimitModel
	{
		// Token: 0x060016AF RID: 5807 RVA: 0x0006E368 File Offset: 0x0006C568
		public DefaultPartySizeLimitModel()
		{
			this._quarterMasterText = GameTexts.FindText("str_clan_role", "quartermaster");
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0006E470 File Offset: 0x0006C670
		public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			if (!party.IsMobile)
			{
				return result;
			}
			if (party.MobileParty.IsGarrison)
			{
				return this.CalculateGarrisonPartySizeLimit(party.MobileParty, includeDescriptions);
			}
			return this.CalculateMobilePartyMemberSizeLimit(party.MobileParty, includeDescriptions);
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0006E4BD File Offset: 0x0006C6BD
		public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
		{
			if (party.IsSettlement)
			{
				return this.CalculateSettlementPartyPrisonerSizeLimitInternal(party.Settlement, includeDescriptions);
			}
			return this.CalculateMobilePartyPrisonerSizeLimitInternal(party, includeDescriptions);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x0006E4E0 File Offset: 0x0006C6E0
		private ExplainedNumber CalculateMobilePartyMemberSizeLimit(MobileParty party, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(20f, includeDescriptions, this._baseSizeText);
			if (party.LeaderHero != null && party.LeaderHero.Clan != null && !party.IsCaravan)
			{
				this.CalculateBaseMemberSize(party.LeaderHero, party.MapFaction, party.ActualClan, ref result);
				result.Add((float)(party.EffectiveQuartermaster.GetSkillValue(DefaultSkills.Steward) / 4), this._quarterMasterText, null);
				if (DefaultPartySizeLimitModel._addAdditionalPartySizeAsCheat && party.IsMainParty && Game.Current.CheatMode)
				{
					result.Add(5000f, new TextObject("{=!}Additional size from extra party cheat", null), null);
				}
			}
			else if (party.IsCaravan)
			{
				if (party.Party.Owner == Hero.MainHero)
				{
					result.Add(10f, this._randomSizeBonusTemporary, null);
				}
				else
				{
					Hero owner = party.Party.Owner;
					if (owner != null && owner.IsNotable)
					{
						result.Add((float)(10 * ((party.Party.Owner.Power < 100f) ? 1 : ((party.Party.Owner.Power < 200f) ? 2 : 3))), this._randomSizeBonusTemporary, null);
					}
				}
			}
			else if (party.IsVillager)
			{
				result.Add(40f, this._randomSizeBonusTemporary, null);
			}
			return result;
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0006E650 File Offset: 0x0006C850
		private ExplainedNumber CalculateGarrisonPartySizeLimit(MobileParty party, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(200f, includeDescriptions, this._baseSizeText);
			this.GetLeadershipSkillLevelEffect(party, DefaultPartySizeLimitModel.LimitType.GarrisonPartySizeLimit, ref result);
			result.Add((float)this.GetTownBonus(party), this._townBonusText, null);
			this.AddGarrisonOwnerPerkEffects(party.CurrentSettlement, ref result);
			this.AddSettlementProjectBonuses(party.Party, ref result);
			return result;
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0006E6B0 File Offset: 0x0006C8B0
		private ExplainedNumber CalculateSettlementPartyPrisonerSizeLimitInternal(Settlement settlement, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(60f, includeDescriptions, this._baseSizeText);
			Town town = settlement.Town;
			int num = (town != null) ? town.GetWallLevel() : 0;
			if (num > 0)
			{
				result.Add((float)(num * 40), this._wallLevelBonusText, null);
			}
			return result;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0006E6FC File Offset: 0x0006C8FC
		private ExplainedNumber CalculateMobilePartyPrisonerSizeLimitInternal(PartyBase party, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(10f, includeDescriptions, this._baseSizeText);
			result.Add((float)this.GetCurrentPartySizeEffect(party), this._currentPartySizeBonusText, null);
			this.AddMobilePartyLeaderPrisonerSizePerkEffects(party, ref result);
			if (DefaultPartySizeLimitModel._addAdditionalPrisonerSizeAsCheat && party.IsMobile && party.MobileParty.IsMainParty && Game.Current.CheatMode)
			{
				result.Add(5000f, new TextObject("{=!}Additional size from extra prisoner cheat", null), null);
			}
			return result;
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0006E77C File Offset: 0x0006C97C
		private void GetLeadershipSkillLevelEffect(MobileParty party, DefaultPartySizeLimitModel.LimitType type, ref ExplainedNumber partySizeBonus)
		{
			Hero hero;
			if (!party.IsGarrison)
			{
				hero = party.LeaderHero;
			}
			else if (party == null)
			{
				hero = null;
			}
			else
			{
				Settlement currentSettlement = party.CurrentSettlement;
				if (currentSettlement == null)
				{
					hero = null;
				}
				else
				{
					Clan ownerClan = currentSettlement.OwnerClan;
					hero = ((ownerClan != null) ? ownerClan.Leader : null);
				}
			}
			Hero hero2 = hero;
			if (hero2 != null && type == DefaultPartySizeLimitModel.LimitType.GarrisonPartySizeLimit)
			{
				SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Leadership, DefaultSkillEffects.LeadershipGarrisonSizeBonus, hero2.CharacterObject, ref partySizeBonus, -1, true, 0);
			}
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0006E7E0 File Offset: 0x0006C9E0
		private void AddMobilePartyLeaderPrisonerSizePerkEffects(PartyBase party, ref ExplainedNumber result)
		{
			if (party.LeaderHero != null)
			{
				if (party.LeaderHero.GetPerkValue(DefaultPerks.TwoHanded.Terror))
				{
					result.Add(DefaultPerks.TwoHanded.Terror.SecondaryBonus, DefaultPerks.TwoHanded.Terror.Name, null);
				}
				if (party.LeaderHero.GetPerkValue(DefaultPerks.Athletics.Stamina))
				{
					result.Add(DefaultPerks.Athletics.Stamina.SecondaryBonus, DefaultPerks.Athletics.Stamina.Name, null);
				}
				if (party.LeaderHero.GetPerkValue(DefaultPerks.Roguery.Manhunter))
				{
					result.Add(DefaultPerks.Roguery.Manhunter.SecondaryBonus, DefaultPerks.Roguery.Manhunter.Name, null);
				}
				if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Scouting.VantagePoint))
				{
					result.Add(DefaultPerks.Scouting.VantagePoint.SecondaryBonus, DefaultPerks.Scouting.VantagePoint.Name, null);
				}
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0006E8B4 File Offset: 0x0006CAB4
		private void AddGarrisonOwnerPerkEffects(Settlement currentSettlement, ref ExplainedNumber result)
		{
			if (currentSettlement != null && currentSettlement.IsTown)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.OneHanded.CorpsACorps, currentSettlement.Town, ref result);
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.VeteransRespect, currentSettlement.Town, ref result);
			}
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x0006E8E3 File Offset: 0x0006CAE3
		public override int GetTierPartySizeEffect(int tier)
		{
			if (tier >= 1)
			{
				return 15 * tier;
			}
			return 0;
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x0006E8F0 File Offset: 0x0006CAF0
		public override int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(20f, false, this._baseSizeText);
			if (leaderHero != null && leaderHero.Clan != null)
			{
				this.CalculateBaseMemberSize(leaderHero, partyMapFaction, actualClan, ref explainedNumber);
				explainedNumber.Add((float)(leaderHero.GetSkillValue(DefaultSkills.Steward) / 4), this._quarterMasterText, null);
			}
			return (int)explainedNumber.BaseNumber;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0006E94C File Offset: 0x0006CB4C
		private void AddSettlementProjectBonuses(PartyBase party, ref ExplainedNumber result)
		{
			if (party.Owner != null)
			{
				Settlement currentSettlement = party.MobileParty.CurrentSettlement;
				if (currentSettlement != null && (currentSettlement.IsTown || currentSettlement.IsCastle))
				{
					foreach (Building building in currentSettlement.Town.Buildings)
					{
						float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.GarrisonCapacity);
						if (buildingEffectAmount > 0f)
						{
							result.Add(buildingEffectAmount, building.Name, null);
						}
					}
				}
			}
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x0006E9E4 File Offset: 0x0006CBE4
		private int GetTownBonus(MobileParty party)
		{
			Settlement currentSettlement = party.CurrentSettlement;
			if (currentSettlement != null && currentSettlement.IsFortification && currentSettlement.IsTown)
			{
				return 200;
			}
			return 0;
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x0006EA12 File Offset: 0x0006CC12
		private int GetCurrentPartySizeEffect(PartyBase party)
		{
			return party.NumberOfHealthyMembers / 2;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x0006EA1C File Offset: 0x0006CC1C
		private void CalculateBaseMemberSize(Hero partyLeader, IFaction partyMapFaction, Clan actualClan, ref ExplainedNumber result)
		{
			if (partyMapFaction != null && partyMapFaction.IsKingdomFaction && partyLeader.MapFaction.Leader == partyLeader)
			{
				result.Add(20f, this._factionLeaderText, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.OneHanded.Prestige))
			{
				result.Add(DefaultPerks.OneHanded.Prestige.SecondaryBonus, DefaultPerks.OneHanded.Prestige.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.TwoHanded.Hope))
			{
				result.Add(DefaultPerks.TwoHanded.Hope.SecondaryBonus, DefaultPerks.TwoHanded.Hope.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.Athletics.ImposingStature))
			{
				result.Add(DefaultPerks.Athletics.ImposingStature.SecondaryBonus, DefaultPerks.Athletics.ImposingStature.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.Bow.MerryMen))
			{
				result.Add(DefaultPerks.Bow.MerryMen.PrimaryBonus, DefaultPerks.Bow.MerryMen.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.Tactics.HordeLeader))
			{
				result.Add(DefaultPerks.Tactics.HordeLeader.PrimaryBonus, DefaultPerks.Tactics.HordeLeader.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.Scouting.MountedScouts))
			{
				result.Add(DefaultPerks.Scouting.MountedScouts.SecondaryBonus, DefaultPerks.Scouting.MountedScouts.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.Leadership.Authority))
			{
				result.Add(DefaultPerks.Leadership.Authority.SecondaryBonus, DefaultPerks.Leadership.Authority.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.Leadership.UpliftingSpirit))
			{
				result.Add(DefaultPerks.Leadership.UpliftingSpirit.SecondaryBonus, DefaultPerks.Leadership.UpliftingSpirit.Name, null);
			}
			if (partyLeader.GetPerkValue(DefaultPerks.Leadership.TalentMagnet))
			{
				result.Add(DefaultPerks.Leadership.TalentMagnet.PrimaryBonus, DefaultPerks.Leadership.TalentMagnet.Name, null);
			}
			if (partyLeader.GetSkillValue(DefaultSkills.Leadership) > Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus && partyLeader.GetPerkValue(DefaultPerks.Leadership.UltimateLeader))
			{
				int num = partyLeader.GetSkillValue(DefaultSkills.Leadership) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
				result.Add((float)num * DefaultPerks.Leadership.UltimateLeader.PrimaryBonus, this._leadershipPerkUltimateLeaderBonusText, null);
			}
			if (actualClan != null)
			{
				Hero leader = actualClan.Leader;
				bool? flag = (leader != null) ? new bool?(leader.GetPerkValue(DefaultPerks.Leadership.LeaderOfMasses)) : null;
				bool flag2 = true;
				if (flag.GetValueOrDefault() == flag2 & flag != null)
				{
					int num2 = 0;
					using (List<Settlement>.Enumerator enumerator = actualClan.Settlements.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsTown)
							{
								num2++;
							}
						}
					}
					float num3 = (float)num2 * DefaultPerks.Leadership.LeaderOfMasses.PrimaryBonus;
					if (num3 > 0f)
					{
						result.Add(num3, DefaultPerks.Leadership.LeaderOfMasses.Name, null);
					}
				}
			}
			if (partyLeader.Clan.Leader == partyLeader)
			{
				if (partyLeader.Clan.Tier >= 5 && partyMapFaction.IsKingdomFaction && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.NobleRetinues))
				{
					result.Add(40f, DefaultPolicies.NobleRetinues.Name, null);
				}
				if (partyMapFaction.IsKingdomFaction && partyMapFaction.Leader == partyLeader && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.RoyalGuard))
				{
					result.Add(60f, DefaultPolicies.RoyalGuard.Name, null);
				}
				if (partyLeader.Clan.Tier > 0)
				{
					result.Add((float)(partyLeader.Clan.Tier * 25), this._clanTierText, null);
					return;
				}
			}
			else if (partyLeader.Clan.Tier > 0)
			{
				result.Add((float)(partyLeader.Clan.Tier * 15), this._clanTierText, null);
			}
		}

		// Token: 0x040007D2 RID: 2002
		private const int BaseMobilePartySize = 20;

		// Token: 0x040007D3 RID: 2003
		private const int BaseMobilePartyPrisonerSize = 10;

		// Token: 0x040007D4 RID: 2004
		private const int BaseSettlementPrisonerSize = 60;

		// Token: 0x040007D5 RID: 2005
		private const int SettlementPrisonerSizeBonusPerWallLevel = 40;

		// Token: 0x040007D6 RID: 2006
		private const int BaseGarrisonPartySize = 200;

		// Token: 0x040007D7 RID: 2007
		private const int TownGarrisonSizeBonus = 200;

		// Token: 0x040007D8 RID: 2008
		private const int AdditionalPartySizeForCheat = 5000;

		// Token: 0x040007D9 RID: 2009
		private readonly TextObject _leadershipSkillLevelBonusText = GameTexts.FindText("str_leadership_skill_level_bonus", null);

		// Token: 0x040007DA RID: 2010
		private readonly TextObject _leadershipPerkUltimateLeaderBonusText = GameTexts.FindText("str_leadership_perk_bonus", null);

		// Token: 0x040007DB RID: 2011
		private readonly TextObject _wallLevelBonusText = GameTexts.FindText("str_map_tooltip_wall_level", null);

		// Token: 0x040007DC RID: 2012
		private readonly TextObject _baseSizeText = GameTexts.FindText("str_base_size", null);

		// Token: 0x040007DD RID: 2013
		private readonly TextObject _clanTierText = GameTexts.FindText("str_clan_tier_bonus", null);

		// Token: 0x040007DE RID: 2014
		private readonly TextObject _renownText = GameTexts.FindText("str_renown_bonus", null);

		// Token: 0x040007DF RID: 2015
		private readonly TextObject _clanLeaderText = GameTexts.FindText("str_clan_leader_bonus", null);

		// Token: 0x040007E0 RID: 2016
		private readonly TextObject _factionLeaderText = GameTexts.FindText("str_faction_leader_bonus", null);

		// Token: 0x040007E1 RID: 2017
		private readonly TextObject _leaderLevelText = GameTexts.FindText("str_leader_level_bonus", null);

		// Token: 0x040007E2 RID: 2018
		private readonly TextObject _townBonusText = GameTexts.FindText("str_town_bonus", null);

		// Token: 0x040007E3 RID: 2019
		private readonly TextObject _minorFactionText = GameTexts.FindText("str_minor_faction_bonus", null);

		// Token: 0x040007E4 RID: 2020
		private readonly TextObject _currentPartySizeBonusText = GameTexts.FindText("str_current_party_size_bonus", null);

		// Token: 0x040007E5 RID: 2021
		private readonly TextObject _randomSizeBonusTemporary = new TextObject("{=hynFV8jC}Extra size bonus (Perk-like Effect)", null);

		// Token: 0x040007E6 RID: 2022
		private static bool _addAdditionalPartySizeAsCheat;

		// Token: 0x040007E7 RID: 2023
		private static bool _addAdditionalPrisonerSizeAsCheat;

		// Token: 0x040007E8 RID: 2024
		private TextObject _quarterMasterText;

		// Token: 0x0200050C RID: 1292
		private enum LimitType
		{
			// Token: 0x040015B6 RID: 5558
			MobilePartySizeLimit,
			// Token: 0x040015B7 RID: 5559
			GarrisonPartySizeLimit,
			// Token: 0x040015B8 RID: 5560
			PrisonerSizeLimit
		}
	}
}
