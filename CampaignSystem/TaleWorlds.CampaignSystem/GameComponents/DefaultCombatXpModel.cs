using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FA RID: 250
	public class DefaultCombatXpModel : CombatXpModel
	{
		// Token: 0x06001538 RID: 5432 RVA: 0x00062140 File Offset: 0x00060340
		public override SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit)
		{
			SkillObject result = DefaultSkills.Athletics;
			if (isSiegeEngineHit)
			{
				result = DefaultSkills.Engineering;
			}
			else if (weapon != null)
			{
				result = weapon.RelevantSkill;
			}
			return result;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0006216C File Offset: 0x0006036C
		public override void GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase party, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType, out int xpAmount)
		{
			int num = attackedTroop.MaxHitPoints();
			MilitaryPowerModel militaryPowerModel = Campaign.Current.Models.MilitaryPowerModel;
			float defaultTroopPower = militaryPowerModel.GetDefaultTroopPower(attackedTroop);
			float defaultTroopPower2 = militaryPowerModel.GetDefaultTroopPower(attackerTroop);
			float leaderModifier = 0f;
			float contextModifier = 0f;
			if (((party != null) ? party.MapEvent : null) != null)
			{
				contextModifier = militaryPowerModel.GetContextModifier(attackedTroop, party.Side, party.MapEvent.SimulationContext);
				leaderModifier = party.MapEventSide.LeaderSimulationModifier;
			}
			float troopPower = militaryPowerModel.GetTroopPower(defaultTroopPower, leaderModifier, contextModifier);
			float troopPower2 = militaryPowerModel.GetTroopPower(defaultTroopPower2, leaderModifier, contextModifier);
			float num2 = 0.4f * (troopPower2 + 0.5f) * (troopPower + 0.5f) * (float)(MathF.Min(damage, num) + (isFatal ? num : 0));
			num2 *= ((missionType == CombatXpModel.MissionTypeEnum.NoXp) ? 0f : ((missionType == CombatXpModel.MissionTypeEnum.PracticeFight) ? 0.0625f : ((missionType == CombatXpModel.MissionTypeEnum.Tournament) ? 0.33f : ((missionType == CombatXpModel.MissionTypeEnum.SimulationBattle) ? 0.9f : ((missionType == CombatXpModel.MissionTypeEnum.Battle) ? 1f : 1f)))));
			ExplainedNumber explainedNumber = new ExplainedNumber(num2, false, null);
			if (party != null)
			{
				this.GetBattleXpBonusFromPerks(party, ref explainedNumber, attackerTroop);
			}
			if (captain != null && captain.IsHero && captain.GetPerkValue(DefaultPerks.Leadership.InspiringLeader))
			{
				explainedNumber.AddFactor(DefaultPerks.Leadership.InspiringLeader.SecondaryBonus, DefaultPerks.Leadership.InspiringLeader.Name);
			}
			xpAmount = MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x000622CF File Offset: 0x000604CF
		public override float GetXpMultiplierFromShotDifficulty(float shotDifficulty)
		{
			if (shotDifficulty > 14.4f)
			{
				shotDifficulty = 14.4f;
			}
			return MBMath.Lerp(0f, 2f, (shotDifficulty - 1f) / 13.4f, 1E-05f);
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x00062301 File Offset: 0x00060501
		public override float CaptainRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00062308 File Offset: 0x00060508
		private void GetBattleXpBonusFromPerks(PartyBase party, ref ExplainedNumber xpToGain, CharacterObject troop)
		{
			if (party.IsMobile && party.MobileParty.LeaderHero != null)
			{
				if (!troop.IsRanged && party.MobileParty.HasPerk(DefaultPerks.OneHanded.Trainer, true))
				{
					xpToGain.AddFactor(DefaultPerks.OneHanded.Trainer.SecondaryBonus, DefaultPerks.OneHanded.Trainer.Name);
				}
				if (troop.HasThrowingWeapon() && party.MobileParty.HasPerk(DefaultPerks.Throwing.Resourceful, true))
				{
					xpToGain.AddFactor(DefaultPerks.Throwing.Resourceful.SecondaryBonus, DefaultPerks.Throwing.Resourceful.Name);
				}
				if (troop.IsInfantry)
				{
					if (party.MobileParty.HasPerk(DefaultPerks.OneHanded.CorpsACorps, false))
					{
						xpToGain.AddFactor(DefaultPerks.OneHanded.CorpsACorps.PrimaryBonus, DefaultPerks.OneHanded.CorpsACorps.Name);
					}
					if (party.MobileParty.HasPerk(DefaultPerks.TwoHanded.BaptisedInBlood, true))
					{
						xpToGain.AddFactor(DefaultPerks.TwoHanded.BaptisedInBlood.SecondaryBonus, DefaultPerks.TwoHanded.BaptisedInBlood.Name);
					}
				}
				if (party.MobileParty.HasPerk(DefaultPerks.OneHanded.LeadByExample, false))
				{
					xpToGain.AddFactor(DefaultPerks.OneHanded.LeadByExample.PrimaryBonus, DefaultPerks.OneHanded.LeadByExample.Name);
				}
				if (troop.IsRanged)
				{
					if (party.MobileParty.HasPerk(DefaultPerks.Crossbow.MountedCrossbowman, true))
					{
						xpToGain.AddFactor(DefaultPerks.Crossbow.MountedCrossbowman.SecondaryBonus, DefaultPerks.Crossbow.MountedCrossbowman.Name);
					}
					if (party.MobileParty.HasPerk(DefaultPerks.Bow.BullsEye, false))
					{
						xpToGain.AddFactor(DefaultPerks.Bow.BullsEye.PrimaryBonus, DefaultPerks.Bow.BullsEye.Name);
					}
				}
				if (troop.Culture.IsBandit && party.MobileParty.HasPerk(DefaultPerks.Roguery.NoRestForTheWicked, false))
				{
					xpToGain.AddFactor(DefaultPerks.Roguery.NoRestForTheWicked.PrimaryBonus, DefaultPerks.Roguery.NoRestForTheWicked.Name);
				}
			}
			if (party.IsMobile && party.MobileParty.IsGarrison)
			{
				Settlement currentSettlement = party.MobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town.Governor : null) != null)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.ProjectileDeflection, party.MobileParty.CurrentSettlement.Town, ref xpToGain);
					if (troop.IsMounted)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.Guards, party.MobileParty.CurrentSettlement.Town, ref xpToGain);
					}
				}
			}
		}
	}
}
