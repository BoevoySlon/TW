using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000120 RID: 288
	public class DefaultPartyMoraleModel : PartyMoraleModel
	{
		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x0006DE03 File Offset: 0x0006C003
		public override float HighMoraleValue
		{
			get
			{
				return 70f;
			}
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0006DE0A File Offset: 0x0006C00A
		public override int GetDailyStarvationMoralePenalty(PartyBase party)
		{
			return -5;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x0006DE0E File Offset: 0x0006C00E
		public override int GetDailyNoWageMoralePenalty(MobileParty party)
		{
			return -3;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0006DE12 File Offset: 0x0006C012
		private int GetStarvationMoralePenalty(MobileParty party)
		{
			return -30;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0006DE16 File Offset: 0x0006C016
		private int GetNoWageMoralePenalty(MobileParty party)
		{
			return -20;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x0006DE1A File Offset: 0x0006C01A
		public override float GetStandardBaseMorale(PartyBase party)
		{
			return 50f;
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x0006DE21 File Offset: 0x0006C021
		public override float GetVictoryMoraleChange(PartyBase party)
		{
			return 20f;
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x0006DE28 File Offset: 0x0006C028
		public override float GetDefeatMoraleChange(PartyBase party)
		{
			return -20f;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0006DE30 File Offset: 0x0006C030
		private void CalculateFoodVarietyMoraleBonus(MobileParty party, ref ExplainedNumber result)
		{
			if (!party.Party.IsStarving)
			{
				float num;
				switch (party.ItemRoster.FoodVariety)
				{
				case 0:
				case 1:
					num = -2f;
					break;
				case 2:
					num = -1f;
					break;
				case 3:
					num = 0f;
					break;
				case 4:
					num = 1f;
					break;
				case 5:
					num = 2f;
					break;
				case 6:
					num = 3f;
					break;
				case 7:
					num = 5f;
					break;
				case 8:
					num = 6f;
					break;
				case 9:
					num = 7f;
					break;
				case 10:
					num = 8f;
					break;
				case 11:
					num = 9f;
					break;
				case 12:
					num = 10f;
					break;
				default:
					num = 10f;
					break;
				}
				if (num < 0f && party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Steward.WarriorsDiet))
				{
					num = 0f;
				}
				if (num != 0f)
				{
					result.Add(num, this._foodBonusMoraleText, null);
					if (num > 0f && party.HasPerk(DefaultPerks.Steward.Gourmet, false))
					{
						result.Add(num, DefaultPerks.Steward.Gourmet.Name, null);
					}
				}
			}
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x0006DF64 File Offset: 0x0006C164
		private void GetPartySizeMoraleEffect(MobileParty mobileParty, ref ExplainedNumber result)
		{
			if (!mobileParty.IsMilitia && !mobileParty.IsVillager)
			{
				int num = mobileParty.Party.NumberOfAllMembers - mobileParty.Party.PartySizeLimit;
				if (num > 0)
				{
					result.Add(-1f * MathF.Sqrt((float)num), this._partySizeMoraleText, null);
				}
			}
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x0006DFB8 File Offset: 0x0006C1B8
		private static void CheckPerkEffectOnPartyMorale(MobileParty party, PerkObject perk, bool isInfoNeeded, TextObject newInfo, int perkEffect, out TextObject outNewInfo, out int outPerkEffect)
		{
			outNewInfo = newInfo;
			outPerkEffect = perkEffect;
			if (party.LeaderHero != null && party.LeaderHero.GetPerkValue(perk))
			{
				if (isInfoNeeded)
				{
					MBTextManager.SetTextVariable("EFFECT_NAME", perk.Name, false);
					MBTextManager.SetTextVariable("NUM", 10);
					MBTextManager.SetTextVariable("STR1", newInfo, false);
					MBTextManager.SetTextVariable("STR2", GameTexts.FindText("str_party_effect", null), false);
					outNewInfo = GameTexts.FindText("str_new_item_line", null);
				}
				outPerkEffect += 10;
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0006E040 File Offset: 0x0006C240
		private void GetMoraleEffectsFromPerks(MobileParty party, ref ExplainedNumber bonus)
		{
			if (party.HasPerk(DefaultPerks.Crossbow.PeasantLeader, false))
			{
				float num = this.CalculateTroopTierRatio(party);
				bonus.AddFactor(DefaultPerks.Crossbow.PeasantLeader.PrimaryBonus * num, DefaultPerks.Crossbow.PeasantLeader.Name);
			}
			Settlement currentSettlement = party.CurrentSettlement;
			if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null && party.HasPerk(DefaultPerks.Charm.SelfPromoter, true))
			{
				bonus.Add(DefaultPerks.Charm.SelfPromoter.SecondaryBonus, DefaultPerks.Charm.SelfPromoter.Name, null);
			}
			if (party.HasPerk(DefaultPerks.Steward.Logistician, false))
			{
				int num2 = 0;
				for (int i = 0; i < party.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Character.IsMounted)
					{
						num2 += elementCopyAtIndex.Number;
					}
				}
				if (party.Party.NumberOfMounts > party.MemberRoster.TotalManCount - num2)
				{
					bonus.Add(DefaultPerks.Steward.Logistician.PrimaryBonus, DefaultPerks.Steward.Logistician.Name, null);
				}
			}
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0006E13C File Offset: 0x0006C33C
		private float CalculateTroopTierRatio(MobileParty party)
		{
			int totalManCount = party.MemberRoster.TotalManCount;
			float num = 0f;
			foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.Tier <= 3)
				{
					num += (float)troopRosterElement.Number;
				}
			}
			return num / (float)totalManCount;
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0006E1BC File Offset: 0x0006C3BC
		private void GetMoraleEffectsFromSkill(MobileParty party, ref ExplainedNumber bonus)
		{
			CharacterObject effectivePartyLeaderForSkill = SkillHelper.GetEffectivePartyLeaderForSkill(party.Party);
			if (effectivePartyLeaderForSkill != null && effectivePartyLeaderForSkill.GetSkillValue(DefaultSkills.Leadership) > 0)
			{
				SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Leadership, DefaultSkillEffects.LeadershipMoraleBonus, effectivePartyLeaderForSkill, ref bonus, -1, true, 0);
			}
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0006E1FC File Offset: 0x0006C3FC
		public override ExplainedNumber GetEffectivePartyMorale(MobileParty mobileParty, bool includeDescription = false)
		{
			ExplainedNumber result = new ExplainedNumber(50f, includeDescription, null);
			result.Add(mobileParty.RecentEventsMorale, this._recentEventsText, null);
			this.GetMoraleEffectsFromSkill(mobileParty, ref result);
			if (mobileParty.IsMilitia || mobileParty.IsGarrison)
			{
				if (mobileParty.IsMilitia)
				{
					if (mobileParty.HomeSettlement.IsStarving)
					{
						result.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
					}
				}
				else if (SettlementHelper.IsGarrisonStarving(mobileParty.CurrentSettlement))
				{
					result.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
				}
			}
			else if (mobileParty.Party.IsStarving)
			{
				result.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
			}
			if (mobileParty.HasUnpaidWages > 0f)
			{
				result.Add(mobileParty.HasUnpaidWages * (float)this.GetNoWageMoralePenalty(mobileParty), this._noWageMoraleText, null);
			}
			this.GetMoraleEffectsFromPerks(mobileParty, ref result);
			this.CalculateFoodVarietyMoraleBonus(mobileParty, ref result);
			this.GetPartySizeMoraleEffect(mobileParty, ref result);
			return result;
		}

		// Token: 0x040007CC RID: 1996
		private const float BaseMoraleValue = 50f;

		// Token: 0x040007CD RID: 1997
		private readonly TextObject _recentEventsText = GameTexts.FindText("str_recent_events", null);

		// Token: 0x040007CE RID: 1998
		private readonly TextObject _starvationMoraleText = GameTexts.FindText("str_starvation_morale", null);

		// Token: 0x040007CF RID: 1999
		private readonly TextObject _noWageMoraleText = GameTexts.FindText("str_no_wage_morale", null);

		// Token: 0x040007D0 RID: 2000
		private readonly TextObject _foodBonusMoraleText = GameTexts.FindText("str_food_bonus_morale", null);

		// Token: 0x040007D1 RID: 2001
		private readonly TextObject _partySizeMoraleText = GameTexts.FindText("str_party_size_morale", null);
	}
}
