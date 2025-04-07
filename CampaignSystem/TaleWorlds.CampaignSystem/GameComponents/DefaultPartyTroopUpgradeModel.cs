using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000125 RID: 293
	public class DefaultPartyTroopUpgradeModel : PartyTroopUpgradeModel
	{
		// Token: 0x060016DE RID: 5854 RVA: 0x000700AC File Offset: 0x0006E2AC
		public override bool CanPartyUpgradeTroopToTarget(PartyBase upgradingParty, CharacterObject upgradeableCharacter, CharacterObject upgradeTarget)
		{
			bool flag = this.DoesPartyHaveRequiredItemsForUpgrade(upgradingParty, upgradeTarget);
			PerkObject perkObject;
			bool flag2 = this.DoesPartyHaveRequiredPerksForUpgrade(upgradingParty, upgradeableCharacter, upgradeTarget, out perkObject);
			return this.IsTroopUpgradeable(upgradingParty, upgradeableCharacter) && upgradeableCharacter.UpgradeTargets.Contains(upgradeTarget) && flag2 && flag;
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x000700EB File Offset: 0x0006E2EB
		public override bool IsTroopUpgradeable(PartyBase party, CharacterObject character)
		{
			return !character.IsHero && character.UpgradeTargets.Length != 0;
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x00070104 File Offset: 0x0006E304
		public override int GetXpCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
		{
			if (upgradeTarget != null && characterObject.UpgradeTargets.Contains(upgradeTarget))
			{
				int tier = upgradeTarget.Tier;
				int num = 0;
				for (int i = characterObject.Tier + 1; i <= tier; i++)
				{
					if (i <= 1)
					{
						num += 100;
					}
					else if (i == 2)
					{
						num += 300;
					}
					else if (i == 3)
					{
						num += 550;
					}
					else if (i == 4)
					{
						num += 900;
					}
					else if (i == 5)
					{
						num += 1300;
					}
					else if (i == 6)
					{
						num += 1700;
					}
					else if (i == 7)
					{
						num += 2100;
					}
					else
					{
						int num2 = upgradeTarget.Level + 4;
						num += (int)(1.333f * (float)num2 * (float)num2);
					}
				}
				return num;
			}
			return 100000000;
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x000701C4 File Offset: 0x0006E3C4
		public override int GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
		{
			PartyWageModel partyWageModel = Campaign.Current.Models.PartyWageModel;
			int troopRecruitmentCost = partyWageModel.GetTroopRecruitmentCost(upgradeTarget, null, true);
			int troopRecruitmentCost2 = partyWageModel.GetTroopRecruitmentCost(characterObject, null, true);
			bool flag = characterObject.Occupation == Occupation.Mercenary || characterObject.Occupation == Occupation.Gangster;
			ExplainedNumber explainedNumber = new ExplainedNumber((float)(troopRecruitmentCost - troopRecruitmentCost2) / ((!flag) ? 2f : 3f), false, null);
			if (party.MobileParty.HasPerk(DefaultPerks.Steward.SoundReserves, false))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.SoundReserves, party.MobileParty, true, ref explainedNumber);
			}
			if (characterObject.IsRanged && party.MobileParty.HasPerk(DefaultPerks.Bow.RenownedArcher, true))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.RenownedArcher, party.MobileParty, false, ref explainedNumber);
			}
			if (characterObject.IsInfantry && party.MobileParty.HasPerk(DefaultPerks.Throwing.ThrowingCompetitions, false))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.ThrowingCompetitions, party.MobileParty, true, ref explainedNumber);
			}
			if (characterObject.IsMounted && PartyBaseHelper.HasFeat(party, DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat))
			{
				explainedNumber.AddFactor(DefaultCulturalFeats.KhuzaitRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
			}
			else if (characterObject.IsInfantry && PartyBaseHelper.HasFeat(party, DefaultCulturalFeats.SturgianRecruitUpgradeFeat))
			{
				explainedNumber.AddFactor(DefaultCulturalFeats.SturgianRecruitUpgradeFeat.EffectBonus, GameTexts.FindText("str_culture", null));
			}
			if (flag && party.MobileParty.HasPerk(DefaultPerks.Steward.Contractors, false))
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Steward.Contractors, party.MobileParty, true, ref explainedNumber);
			}
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x0007033E File Offset: 0x0006E53E
		public override int GetSkillXpFromUpgradingTroops(PartyBase party, CharacterObject troop, int numberOfTroops)
		{
			return (troop.Level + 10) * numberOfTroops;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x0007034C File Offset: 0x0006E54C
		public override bool DoesPartyHaveRequiredItemsForUpgrade(PartyBase party, CharacterObject upgradeTarget)
		{
			ItemCategory upgradeRequiresItemFromCategory = upgradeTarget.UpgradeRequiresItemFromCategory;
			if (upgradeRequiresItemFromCategory != null)
			{
				int num = 0;
				for (int i = 0; i < party.ItemRoster.Count; i++)
				{
					ItemRosterElement itemRosterElement = party.ItemRoster[i];
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == upgradeRequiresItemFromCategory)
					{
						num += itemRosterElement.Amount;
					}
				}
				return num > 0;
			}
			return true;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x000703B0 File Offset: 0x0006E5B0
		public override bool DoesPartyHaveRequiredPerksForUpgrade(PartyBase party, CharacterObject character, CharacterObject upgradeTarget, out PerkObject requiredPerk)
		{
			requiredPerk = null;
			if (character.Culture.IsBandit && !upgradeTarget.Culture.IsBandit)
			{
				requiredPerk = DefaultPerks.Leadership.VeteransRespect;
				return party.MobileParty.HasPerk(requiredPerk, true);
			}
			return true;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x000703EC File Offset: 0x0006E5EC
		public override float GetUpgradeChanceForTroopUpgrade(PartyBase party, CharacterObject troop, int upgradeTargetIndex)
		{
			float result = 1f;
			int num = troop.UpgradeTargets.Length;
			if (num > 1 && upgradeTargetIndex >= 0 && upgradeTargetIndex < num)
			{
				if (party.LeaderHero != null && party.LeaderHero.PreferredUpgradeFormation != FormationClass.NumberOfAllFormations)
				{
					FormationClass preferredUpgradeFormation = party.LeaderHero.PreferredUpgradeFormation;
					if (CharacterHelper.SearchForFormationInTroopTree(troop.UpgradeTargets[upgradeTargetIndex], preferredUpgradeFormation))
					{
						result = 9999f;
					}
				}
				else
				{
					Hero leaderHero = party.LeaderHero;
					int num2 = (leaderHero != null) ? leaderHero.RandomValue : party.Id.GetHashCode();
					int deterministicHashCode = troop.StringId.GetDeterministicHashCode();
					uint num3 = (uint)(num2 >> (troop.Tier * 3 & 31) ^ deterministicHashCode);
					if ((long)upgradeTargetIndex == (long)((ulong)num3 % (ulong)((long)num)))
					{
						result = 9999f;
					}
				}
			}
			return result;
		}
	}
}
