using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000124 RID: 292
	public class DefaultPartyTrainingModel : PartyTrainingModel
	{
		// Token: 0x060016D8 RID: 5848 RVA: 0x0006FB2E File Offset: 0x0006DD2E
		public override int GetXpReward(CharacterObject character)
		{
			int num = character.Level + 6;
			return num * num / 3;
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x0006FB3C File Offset: 0x0006DD3C
		public override ExplainedNumber GetEffectiveDailyExperience(MobileParty mobileParty, TroopRosterElement troop)
		{
			ExplainedNumber result = default(ExplainedNumber);
			if (mobileParty.IsLordParty && !troop.Character.IsHero && (mobileParty.Army == null || mobileParty.Army.LeaderParty != MobileParty.MainParty) && mobileParty.MapEvent == null && (mobileParty.Party.Owner == null || mobileParty.Party.Owner.Clan != Clan.PlayerClan))
			{
				if (mobileParty.LeaderHero != null && mobileParty.LeaderHero == mobileParty.ActualClan.Leader)
				{
					result.Add(15f + (float)troop.Character.Tier * 3f, null, null);
				}
				else
				{
					result.Add(10f + (float)troop.Character.Tier * 2f, null, null);
				}
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Leadership.CombatTips, false))
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Leadership.CombatTips), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Leadership.RaiseTheMeek, false) && troop.Character.Tier < 3)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Leadership.RaiseTheMeek), null, null);
			}
			if (mobileParty.IsGarrison)
			{
				Settlement currentSettlement = mobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town.Governor : null) != null && mobileParty.CurrentSettlement.Town.Governor.GetPerkValue(DefaultPerks.Bow.BullsEye))
				{
					result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Bow.BullsEye), null, null);
				}
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Polearm.Drills, true))
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Polearm.Drills), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.OneHanded.MilitaryTradition, false) && troop.Character.IsInfantry)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.OneHanded.MilitaryTradition), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Athletics.WalkItOff, true) && !troop.Character.IsMounted && mobileParty.IsMoving)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Athletics.WalkItOff), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Throwing.Saddlebags, true) && troop.Character.IsInfantry)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Throwing.Saddlebags), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Athletics.AGoodDaysRest, true) && !troop.Character.IsMounted && !mobileParty.IsMoving && mobileParty.CurrentSettlement != null)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Athletics.AGoodDaysRest), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Bow.Trainer, true) && troop.Character.IsRanged)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Bow.Trainer), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Crossbow.RenownMarksmen, false) && troop.Character.IsRanged)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Crossbow.RenownMarksmen), null, null);
			}
			if (mobileParty.IsActive && mobileParty.IsMoving)
			{
				if (mobileParty.Morale > 75f)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.ForcedMarch, mobileParty, false, ref result);
				}
				if (mobileParty.ItemRoster.TotalWeight > (float)mobileParty.InventoryCapacity)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Unburdened, mobileParty, false, ref result);
				}
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Steward.SevenVeterans, false) && troop.Character.Tier >= 4)
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Steward.SevenVeterans), null, null);
			}
			if (mobileParty.IsActive && mobileParty.HasPerk(DefaultPerks.Steward.DrillSergant, false))
			{
				result.Add((float)this.GetPerkExperiencesForTroops(DefaultPerks.Steward.DrillSergant), null, null);
			}
			if (troop.Character.Culture.IsBandit)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.NoRestForTheWicked, mobileParty, true, ref result);
			}
			return result;
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x0006FF34 File Offset: 0x0006E134
		private int GetPerkExperiencesForTroops(PerkObject perk)
		{
			if (perk == DefaultPerks.Leadership.CombatTips || perk == DefaultPerks.Leadership.RaiseTheMeek || perk == DefaultPerks.OneHanded.MilitaryTradition || perk == DefaultPerks.Crossbow.RenownMarksmen || perk == DefaultPerks.Steward.SevenVeterans || perk == DefaultPerks.Steward.DrillSergant)
			{
				return MathF.Round(perk.PrimaryBonus);
			}
			if (perk == DefaultPerks.Polearm.Drills || perk == DefaultPerks.Athletics.WalkItOff || perk == DefaultPerks.Athletics.AGoodDaysRest || perk == DefaultPerks.Bow.Trainer || perk == DefaultPerks.Bow.BullsEye || perk == DefaultPerks.Throwing.Saddlebags)
			{
				return MathF.Round(perk.SecondaryBonus);
			}
			return 0;
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x0006FFBC File Offset: 0x0006E1BC
		public override int GenerateSharedXp(CharacterObject troop, int xp, MobileParty mobileParty)
		{
			float num = (float)xp * DefaultPerks.Leadership.LeaderOfMasses.SecondaryBonus;
			if (troop.IsHero && !mobileParty.HasPerk(DefaultPerks.Leadership.LeaderOfMasses, true))
			{
				return 0;
			}
			if (troop.IsRanged && troop.IsRegular && mobileParty.HasPerk(DefaultPerks.Leadership.MakeADifference, true))
			{
				num += num * DefaultPerks.Leadership.MakeADifference.SecondaryBonus;
			}
			if (troop.IsMounted && troop.IsRegular && mobileParty.HasPerk(DefaultPerks.Leadership.LeadByExample, true))
			{
				num += num * DefaultPerks.Leadership.LeadByExample.SecondaryBonus;
			}
			return (int)num;
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x0007004C File Offset: 0x0006E24C
		public override int CalculateXpGainFromBattles(FlattenedTroopRosterElement troopRosterElement, PartyBase party)
		{
			int num = troopRosterElement.XpGained;
			if ((party.MapEvent.IsPlayerSimulation || !party.MapEvent.IsPlayerMapEvent) && party.MobileParty.HasPerk(DefaultPerks.Leadership.TrustedCommander, true))
			{
				num += MathF.Round((float)num * DefaultPerks.Leadership.TrustedCommander.SecondaryBonus);
			}
			return num;
		}
	}
}
