using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011E RID: 286
	public class DefaultPartyHealingModel : PartyHealingModel
	{
		// Token: 0x0600168F RID: 5775 RVA: 0x0006D3C8 File Offset: 0x0006B5C8
		public override float GetSurgeryChance(PartyBase party)
		{
			MobileParty mobileParty = party.MobileParty;
			int? num;
			if (mobileParty == null)
			{
				num = null;
			}
			else
			{
				Hero effectiveSurgeon = mobileParty.EffectiveSurgeon;
				num = ((effectiveSurgeon != null) ? new int?(effectiveSurgeon.GetSkillValue(DefaultSkills.Medicine)) : null);
			}
			int num2 = num ?? 0;
			return 0.0015f * (float)num2;
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0006D42C File Offset: 0x0006B62C
		public override float GetSiegeBombardmentHitSurgeryChance(PartyBase party)
		{
			float num = 0f;
			if (party != null && party.IsMobile && party.MobileParty.HasPerk(DefaultPerks.Medicine.SiegeMedic, false))
			{
				num += DefaultPerks.Medicine.SiegeMedic.PrimaryBonus;
			}
			return num;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0006D46C File Offset: 0x0006B66C
		public override float GetSurvivalChance(PartyBase party, CharacterObject character, DamageTypes damageType, bool canDamageKillEvenIfBlunt, PartyBase enemyParty = null)
		{
			if ((damageType == DamageTypes.Blunt && !canDamageKillEvenIfBlunt) || (character.IsHero && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.VeryEasy) || (character.IsPlayerCharacter && CampaignOptions.BattleDeath == CampaignOptions.Difficulty.Easy))
			{
				return 1f;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			float result;
			if (((party != null) ? party.MobileParty : null) != null)
			{
				MobileParty mobileParty = party.MobileParty;
				SkillHelper.AddSkillBonusForParty(DefaultSkills.Medicine, DefaultSkillEffects.SurgeonSurvivalBonus, mobileParty, ref explainedNumber);
				if (((enemyParty != null) ? enemyParty.MobileParty : null) != null && enemyParty.MobileParty.HasPerk(DefaultPerks.Medicine.DoctorsOath, false))
				{
					SkillHelper.AddSkillBonusForParty(DefaultSkills.Medicine, DefaultSkillEffects.SurgeonSurvivalBonus, enemyParty.MobileParty, ref explainedNumber);
					SkillLevelingManager.OnSurgeryApplied(enemyParty.MobileParty, false, character.Tier);
				}
				explainedNumber.Add((float)character.Level * 0.02f, null, null);
				if (!character.IsHero && party.MapEvent != null && character.Tier < 3)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PhysicianOfPeople, party.MobileParty, false, ref explainedNumber);
				}
				if (character.IsHero)
				{
					explainedNumber.Add(character.GetTotalArmorSum(false) * 0.01f, null, null);
					explainedNumber.Add(character.Age * -0.01f, null, null);
					explainedNumber.AddFactor(50f, null);
				}
				ExplainedNumber explainedNumber2 = new ExplainedNumber(1f / explainedNumber.ResultNumber, false, null);
				if (character.IsHero)
				{
					if (party.IsMobile && party.MobileParty.HasPerk(DefaultPerks.Medicine.CheatDeath, true))
					{
						explainedNumber2.AddFactor(DefaultPerks.Medicine.CheatDeath.SecondaryBonus, DefaultPerks.Medicine.CheatDeath.Name);
					}
					if (character.HeroObject.Clan == Clan.PlayerClan)
					{
						float clanMemberDeathChanceMultiplier = Campaign.Current.Models.DifficultyModel.GetClanMemberDeathChanceMultiplier();
						if (!clanMemberDeathChanceMultiplier.ApproximatelyEqualsTo(0f, 1E-05f))
						{
							explainedNumber2.AddFactor(clanMemberDeathChanceMultiplier, GameTexts.FindText("str_game_difficulty", null));
						}
					}
				}
				result = 1f - MBMath.ClampFloat(explainedNumber2.ResultNumber, 0f, 1f);
			}
			else if (character.IsHero && character.HeroObject.IsPrisoner)
			{
				result = 1f - character.Age * 0.0035f;
			}
			else if (explainedNumber.ResultNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				result = 0f;
			}
			else
			{
				result = 1f - 1f / explainedNumber.ResultNumber;
			}
			return result;
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0006D6D3 File Offset: 0x0006B8D3
		public override int GetSkillXpFromHealingTroop(PartyBase party)
		{
			return 5;
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0006D6D8 File Offset: 0x0006B8D8
		public override ExplainedNumber GetDailyHealingForRegulars(MobileParty party, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			if (party.Party.IsStarving || (party.IsGarrison && party.CurrentSettlement.IsStarving))
			{
				if (party.IsGarrison)
				{
					if (SettlementHelper.IsGarrisonStarving(party.CurrentSettlement))
					{
						int num = MBRandom.RoundRandomized((float)party.MemberRoster.TotalRegulars * 0.1f);
						result.Add((float)(-(float)num), DefaultPartyHealingModel._starvingText, null);
					}
				}
				else
				{
					int totalRegulars = party.MemberRoster.TotalRegulars;
					result.Add((float)(-(float)totalRegulars) * 0.25f, DefaultPartyHealingModel._starvingText, null);
				}
			}
			else
			{
				result = new ExplainedNumber(5f, includeDescriptions, null);
				if (party.IsGarrison)
				{
					if (party.CurrentSettlement.IsTown)
					{
						SkillHelper.AddSkillBonusForTown(DefaultSkills.Medicine, DefaultSkillEffects.GovernorHealingRateBonus, party.CurrentSettlement.Town, ref result);
					}
				}
				else
				{
					SkillHelper.AddSkillBonusForParty(DefaultSkills.Medicine, DefaultSkillEffects.HealingRateBonusForRegulars, party, ref result);
				}
				if (!party.IsGarrison && !party.IsMilitia)
				{
					if (!party.IsMoving)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.TriageTent, party, true, ref result);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.AGoodDaysRest, party, true, ref result);
					}
					else
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.WalkItOff, party, true, ref result);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WalkItOff, party, true, ref result);
					}
				}
				if (party.Morale >= Campaign.Current.Models.PartyMoraleModel.HighMoraleValue)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BestMedicine, party, true, ref result);
				}
				if (party.CurrentSettlement != null && !party.CurrentSettlement.IsHideout)
				{
					if (party.CurrentSettlement.IsFortification)
					{
						result.Add(10f, DefaultPartyHealingModel._settlementText, null);
					}
					if (party.SiegeEvent == null && !party.CurrentSettlement.IsUnderSiege && !party.CurrentSettlement.IsRaided && !party.CurrentSettlement.IsUnderRaid)
					{
						if (party.CurrentSettlement.IsTown)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PristineStreets, party, false, ref result);
						}
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.GoodLogdings, party, true, ref result);
					}
				}
				else if (!party.IsMoving && party.LastVisitedSettlement != null && party.LastVisitedSettlement.IsVillage && party.LastVisitedSettlement.Position2D.DistanceSquared(party.Position2D) < 2f && !party.LastVisitedSettlement.IsUnderRaid && !party.LastVisitedSettlement.IsRaided)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BushDoctor, party, false, ref result);
				}
				if (party.Army != null)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Rearguard, party, true, ref result);
				}
				if (party.ItemRoster.FoodVariety > 0 && party.HasPerk(DefaultPerks.Medicine.PerfectHealth, false))
				{
					result.AddFactor((float)party.ItemRoster.FoodVariety * DefaultPerks.Medicine.PerfectHealth.PrimaryBonus, DefaultPerks.Medicine.PerfectHealth.Name);
				}
				if (party.HasPerk(DefaultPerks.Medicine.HelpingHands, false))
				{
					float value = (float)MathF.Floor((float)party.MemberRoster.TotalManCount / 10f) * DefaultPerks.Medicine.HelpingHands.PrimaryBonus;
					result.AddFactor(value, DefaultPerks.Medicine.HelpingHands.Name);
				}
			}
			return result;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0006D9F8 File Offset: 0x0006BBF8
		public override ExplainedNumber GetDailyHealingHpForHeroes(MobileParty party, bool includeDescriptions = false)
		{
			if (party.Party.IsStarving && party.CurrentSettlement == null)
			{
				return new ExplainedNumber(-19f, includeDescriptions, DefaultPartyHealingModel._starvingText);
			}
			ExplainedNumber result = new ExplainedNumber(11f, includeDescriptions, null);
			if (!party.IsGarrison && !party.IsMilitia)
			{
				if (!party.IsMoving)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.TriageTent, party, true, ref result);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.AGoodDaysRest, party, true, ref result);
				}
				else
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.WalkItOff, party, true, ref result);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WalkItOff, party, true, ref result);
				}
			}
			if (party.Morale >= Campaign.Current.Models.PartyMoraleModel.HighMoraleValue)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BestMedicine, party, true, ref result);
			}
			if (party.CurrentSettlement != null && !party.CurrentSettlement.IsHideout)
			{
				if (party.CurrentSettlement.IsFortification)
				{
					result.Add(8f, DefaultPartyHealingModel._settlementText, null);
				}
				if (party.CurrentSettlement.IsTown)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.PristineStreets, party, false, ref result);
				}
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.GoodLogdings, party, true, ref result);
			}
			else if (!party.IsMoving && party.LastVisitedSettlement != null && party.LastVisitedSettlement.IsVillage && party.LastVisitedSettlement.Position2D.DistanceSquared(party.Position2D) < 2f && !party.LastVisitedSettlement.IsUnderRaid && !party.LastVisitedSettlement.IsRaided)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.BushDoctor, party, false, ref result);
			}
			SkillHelper.AddSkillBonusForParty(DefaultSkills.Medicine, DefaultSkillEffects.HealingRateBonusForHeroes, party, ref result);
			return result;
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x0006DB8C File Offset: 0x0006BD8C
		public override int GetHeroesEffectedHealingAmount(Hero hero, float healingRate)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(healingRate, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.SelfMedication, hero.CharacterObject, true, ref explainedNumber);
			float resultNumber = explainedNumber.ResultNumber;
			if (resultNumber - (float)((int)resultNumber) > MBRandom.RandomFloat)
			{
				return (int)resultNumber + 1;
			}
			return (int)resultNumber;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x0006DBD4 File Offset: 0x0006BDD4
		public override int GetBattleEndHealingAmount(MobileParty party, Hero hero)
		{
			float num = 0f;
			if (hero.GetPerkValue(DefaultPerks.Medicine.PreventiveMedicine))
			{
				num += (float)(hero.MaxHitPoints - hero.HitPoints) * DefaultPerks.Medicine.PreventiveMedicine.SecondaryBonus;
			}
			if (party.MapEventSide == party.MapEvent.AttackerSide && hero.GetPerkValue(DefaultPerks.Medicine.WalkItOff))
			{
				num += DefaultPerks.Medicine.WalkItOff.SecondaryBonus;
			}
			return MathF.Round(num);
		}

		// Token: 0x040007C0 RID: 1984
		private const int StarvingEffectHeroes = -19;

		// Token: 0x040007C1 RID: 1985
		private const int FortificationEffectForHeroes = 8;

		// Token: 0x040007C2 RID: 1986
		private const int FortificationEffectForRegulars = 10;

		// Token: 0x040007C3 RID: 1987
		private const int BaseDailyHealingForHeroes = 11;

		// Token: 0x040007C4 RID: 1988
		private const int BaseDailyHealingForTroops = 5;

		// Token: 0x040007C5 RID: 1989
		private const int SkillEXPFromHealingTroops = 5;

		// Token: 0x040007C6 RID: 1990
		private const float StarvingWoundedEffectRatio = 0.25f;

		// Token: 0x040007C7 RID: 1991
		private const float StarvingWoundedEffectRatioForGarrison = 0.1f;

		// Token: 0x040007C8 RID: 1992
		private static readonly TextObject _starvingText = new TextObject("{=jZYUdkXF}Starving", null);

		// Token: 0x040007C9 RID: 1993
		private static readonly TextObject _settlementText = new TextObject("{=M0Gpl0dH}In Settlement", null);
	}
}
