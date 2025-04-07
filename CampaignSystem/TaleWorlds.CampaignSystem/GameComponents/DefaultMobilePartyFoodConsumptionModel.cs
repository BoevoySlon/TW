using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000108 RID: 264
	public class DefaultMobilePartyFoodConsumptionModel : MobilePartyFoodConsumptionModel
	{
		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x00067EFF File Offset: 0x000660FF
		public override int NumberOfMenOnMapToEatOneFood
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x00067F04 File Offset: 0x00066104
		public override ExplainedNumber CalculateDailyBaseFoodConsumptionf(MobileParty party, bool includeDescription = false)
		{
			int num = party.Party.NumberOfAllMembers + party.Party.NumberOfPrisoners / 2;
			num = ((num < 1) ? 1 : num);
			return new ExplainedNumber(-(float)num / (float)this.NumberOfMenOnMapToEatOneFood, includeDescription, null);
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x00067F46 File Offset: 0x00066146
		public override ExplainedNumber CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption)
		{
			this.CalculatePerkEffects(party, ref baseConsumption);
			baseConsumption.LimitMax(0f);
			return baseConsumption;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x00067F60 File Offset: 0x00066160
		private void CalculatePerkEffects(MobileParty party, ref ExplainedNumber result)
		{
			int num = 0;
			for (int i = 0; i < party.MemberRoster.Count; i++)
			{
				if (party.MemberRoster.GetCharacterAtIndex(i).Culture.IsBandit)
				{
					num += party.MemberRoster.GetElementNumber(i);
				}
			}
			for (int j = 0; j < party.PrisonRoster.Count; j++)
			{
				if (party.PrisonRoster.GetCharacterAtIndex(j).Culture.IsBandit)
				{
					num += party.PrisonRoster.GetElementNumber(j);
				}
			}
			if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Roguery.Promises) && num > 0)
			{
				float value = (float)num / (float)party.MemberRoster.TotalManCount * DefaultPerks.Roguery.Promises.PrimaryBonus;
				result.AddFactor(value, DefaultPerks.Roguery.Promises.Name);
			}
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.Spartan, party, false, ref result);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.WarriorsDiet, party, true, ref result);
			if (party.EffectiveQuartermaster != null)
			{
				PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Steward.PriceOfLoyalty, party.EffectiveQuartermaster.CharacterObject, DefaultSkills.Steward, true, ref result, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus);
			}
			TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace);
			if (faceTerrainType == TerrainType.Forest || faceTerrainType == TerrainType.Steppe)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Foragers, party, true, ref result);
			}
			if (party.IsGarrison && party.CurrentSettlement != null && party.CurrentSettlement.Town.IsUnderSiege)
			{
				PerkHelper.AddPerkBonusForTown(DefaultPerks.Athletics.StrongLegs, party.CurrentSettlement.Town, ref result);
			}
			if (party.Army != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.StiffUpperLip, party, true, ref result);
			}
			SiegeEvent siegeEvent = party.SiegeEvent;
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp : null) != null)
			{
				if (party.HasPerk(DefaultPerks.Steward.SoundReserves, true))
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.SoundReserves, party, false, ref result);
				}
				if (party.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(party.Party, MapEvent.BattleTypes.Siege) && party.HasPerk(DefaultPerks.Steward.MasterOfPlanning, false))
				{
					result.AddFactor(DefaultPerks.Steward.MasterOfPlanning.PrimaryBonus, DefaultPerks.Steward.MasterOfPlanning.Name);
				}
			}
		}

		// Token: 0x060015D6 RID: 5590 RVA: 0x00068170 File Offset: 0x00066370
		public override bool DoesPartyConsumeFood(MobileParty mobileParty)
		{
			return mobileParty.IsActive && (mobileParty.LeaderHero == null || mobileParty.LeaderHero.IsLord || mobileParty.LeaderHero.Clan == Clan.PlayerClan || mobileParty.LeaderHero.IsMinorFactionHero) && !mobileParty.IsGarrison && !mobileParty.IsCaravan && !mobileParty.IsBandit && !mobileParty.IsMilitia && !mobileParty.IsVillager;
		}

		// Token: 0x04000781 RID: 1921
		private static readonly TextObject _partyConsumption = new TextObject("{=UrFzdy4z}Daily Consumption", null);
	}
}
