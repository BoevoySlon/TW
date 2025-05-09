﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000017 RID: 23
	public static class PerkHelper
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x0000B06C File Offset: 0x0000926C
		public static IEnumerable<PerkObject> GetCaptainPerksForTroopUsages(TroopUsageFlags troopUsageFlags)
		{
			List<PerkObject> list = new List<PerkObject>();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				bool flag = perkObject.PrimaryTroopUsageMask != TroopUsageFlags.Undefined && troopUsageFlags.HasAllFlags(perkObject.PrimaryTroopUsageMask);
				bool flag2 = perkObject.SecondaryTroopUsageMask != TroopUsageFlags.Undefined && troopUsageFlags.HasAllFlags(perkObject.SecondaryTroopUsageMask);
				if (flag || flag2)
				{
					list.Add(perkObject);
				}
			}
			return list;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000B104 File Offset: 0x00009304
		public static bool PlayerHasAnyItemDonationPerk()
		{
			return MobileParty.MainParty.HasPerk(DefaultPerks.Steward.GivingHands, false) || MobileParty.MainParty.HasPerk(DefaultPerks.Steward.PaidInPromise, true);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000B12C File Offset: 0x0000932C
		public static void AddPerkBonusForParty(PerkObject perk, MobileParty party, bool isPrimaryBonus, ref ExplainedNumber stat)
		{
			Hero hero = (party != null) ? party.LeaderHero : null;
			if (hero != null)
			{
				bool flag = isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.PartyLeader;
				bool flag2 = !isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.PartyLeader;
				if ((flag || flag2) && hero.GetPerkValue(perk))
				{
					float num = flag ? perk.PrimaryBonus : perk.SecondaryBonus;
					if (hero.Clan != Clan.PlayerClan)
					{
						num *= 1.8f;
					}
					if (flag)
					{
						PerkHelper.AddToStat(ref stat, perk.PrimaryIncrementType, num, perk.Name);
					}
					else
					{
						PerkHelper.AddToStat(ref stat, perk.SecondaryIncrementType, num, perk.Name);
					}
				}
				flag = (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.ClanLeader);
				flag2 = (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.ClanLeader);
				if ((flag || flag2) && hero.Clan.Leader != null && hero.Clan.Leader.GetPerkValue(perk))
				{
					if (flag)
					{
						PerkHelper.AddToStat(ref stat, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
					}
					else
					{
						PerkHelper.AddToStat(ref stat, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
					}
				}
				flag = (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.PartyMember);
				flag2 = (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.PartyMember);
				if (flag || flag2)
				{
					if (hero.Clan != Clan.PlayerClan)
					{
						if (hero.GetPerkValue(perk))
						{
							PerkHelper.AddToStat(ref stat, flag ? perk.PrimaryIncrementType : perk.SecondaryIncrementType, flag ? perk.PrimaryBonus : perk.SecondaryBonus, perk.Name);
						}
					}
					else
					{
						foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
						{
							if (troopRosterElement.Character.IsHero && troopRosterElement.Character.GetPerkValue(perk))
							{
								PerkHelper.AddToStat(ref stat, flag ? perk.PrimaryIncrementType : perk.SecondaryIncrementType, flag ? perk.PrimaryBonus : perk.SecondaryBonus, perk.Name);
							}
						}
					}
				}
				if (hero.Clan == Clan.PlayerClan)
				{
					flag = (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.Engineer);
					flag2 = (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.Engineer);
					if (flag || flag2)
					{
						Hero effectiveEngineer = party.EffectiveEngineer;
						if (effectiveEngineer != null && effectiveEngineer.GetPerkValue(perk))
						{
							if (flag)
							{
								PerkHelper.AddToStat(ref stat, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
							}
							else
							{
								PerkHelper.AddToStat(ref stat, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
							}
						}
					}
					flag = (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.Scout);
					flag2 = (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.Scout);
					if (flag || flag2)
					{
						Hero effectiveScout = party.EffectiveScout;
						if (effectiveScout != null && effectiveScout.GetPerkValue(perk))
						{
							if (flag)
							{
								PerkHelper.AddToStat(ref stat, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
							}
							else
							{
								PerkHelper.AddToStat(ref stat, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
							}
						}
					}
					flag = (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.Surgeon);
					flag2 = (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.Surgeon);
					if (flag || flag2)
					{
						Hero effectiveSurgeon = party.EffectiveSurgeon;
						if (effectiveSurgeon != null && effectiveSurgeon.GetPerkValue(perk))
						{
							if (flag)
							{
								PerkHelper.AddToStat(ref stat, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
							}
							else
							{
								PerkHelper.AddToStat(ref stat, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
							}
						}
					}
					flag = (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.Quartermaster);
					flag2 = (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.Quartermaster);
					if (flag || flag2)
					{
						Hero effectiveQuartermaster = party.EffectiveQuartermaster;
						if (effectiveQuartermaster != null && effectiveQuartermaster.GetPerkValue(perk))
						{
							if (flag)
							{
								PerkHelper.AddToStat(ref stat, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
								return;
							}
							PerkHelper.AddToStat(ref stat, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
						}
					}
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000B520 File Offset: 0x00009720
		private static void AddToStat(ref ExplainedNumber stat, SkillEffect.EffectIncrementType effectIncrementType, float number, TextObject text)
		{
			if (effectIncrementType == SkillEffect.EffectIncrementType.Add)
			{
				stat.Add(number, text, null);
				return;
			}
			if (effectIncrementType == SkillEffect.EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number, text);
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000B53C File Offset: 0x0000973C
		public static void AddPerkBonusForCharacter(PerkObject perk, CharacterObject character, bool isPrimaryBonus, ref ExplainedNumber bonuses)
		{
			if (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.Personal)
			{
				if (character.GetPerkValue(perk))
				{
					PerkHelper.AddToStat(ref bonuses, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
				}
			}
			else if (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.Personal && character.GetPerkValue(perk))
			{
				PerkHelper.AddToStat(ref bonuses, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
			}
			if (isPrimaryBonus && perk.PrimaryRole == SkillEffect.PerkRole.ClanLeader)
			{
				if (character.IsHero)
				{
					Clan clan = character.HeroObject.Clan;
					if (((clan != null) ? clan.Leader : null) != null && character.HeroObject.Clan.Leader.GetPerkValue(perk))
					{
						PerkHelper.AddToStat(ref bonuses, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
						return;
					}
				}
			}
			else if (!isPrimaryBonus && perk.SecondaryRole == SkillEffect.PerkRole.ClanLeader && character.IsHero && character.HeroObject.Clan.Leader != null && character.HeroObject.Clan.Leader.GetPerkValue(perk))
			{
				PerkHelper.AddToStat(ref bonuses, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000B664 File Offset: 0x00009864
		public static void AddEpicPerkBonusForCharacter(PerkObject perk, CharacterObject character, SkillObject skillType, bool applyPrimaryBonus, ref ExplainedNumber bonuses, int skillRequired)
		{
			if (character.GetPerkValue(perk))
			{
				int skillValue = character.GetSkillValue(skillType);
				if (skillValue > skillRequired)
				{
					if (applyPrimaryBonus)
					{
						PerkHelper.AddToStat(ref bonuses, perk.PrimaryIncrementType, perk.PrimaryBonus * (float)(skillValue - skillRequired), perk.Name);
						return;
					}
					PerkHelper.AddToStat(ref bonuses, perk.SecondaryIncrementType, perk.SecondaryBonus * (float)(skillValue - skillRequired), perk.Name);
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000B6CC File Offset: 0x000098CC
		public static void AddPerkBonusFromCaptain(PerkObject perk, CharacterObject captainCharacter, ref ExplainedNumber bonuses)
		{
			if (perk.PrimaryRole == SkillEffect.PerkRole.Captain)
			{
				if (captainCharacter != null && captainCharacter.GetPerkValue(perk))
				{
					PerkHelper.AddToStat(ref bonuses, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
					return;
				}
			}
			else if (perk.SecondaryRole == SkillEffect.PerkRole.Captain && captainCharacter != null && captainCharacter.GetPerkValue(perk))
			{
				PerkHelper.AddToStat(ref bonuses, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000B738 File Offset: 0x00009938
		public static void AddPerkBonusForTown(PerkObject perk, Town town, ref ExplainedNumber bonuses)
		{
			bool flag = perk.PrimaryRole == SkillEffect.PerkRole.Governor;
			bool flag2 = perk.SecondaryRole == SkillEffect.PerkRole.Governor;
			if (flag || flag2)
			{
				Hero governor = town.Governor;
				if (governor != null && governor.GetPerkValue(perk) && governor.CurrentSettlement != null && governor.CurrentSettlement == town.Settlement)
				{
					if (flag)
					{
						PerkHelper.AddToStat(ref bonuses, perk.PrimaryIncrementType, perk.PrimaryBonus, perk.Name);
						return;
					}
					PerkHelper.AddToStat(ref bonuses, perk.SecondaryIncrementType, perk.SecondaryBonus, perk.Name);
				}
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000B7BC File Offset: 0x000099BC
		public static bool GetPerkValueForTown(PerkObject perk, Town town)
		{
			if (perk.PrimaryRole == SkillEffect.PerkRole.ClanLeader || perk.SecondaryRole == SkillEffect.PerkRole.ClanLeader)
			{
				Clan ownerClan = town.Owner.Settlement.OwnerClan;
				Hero hero = (ownerClan != null) ? ownerClan.Leader : null;
				if (hero != null && hero.GetPerkValue(perk))
				{
					return true;
				}
			}
			if (perk.PrimaryRole == SkillEffect.PerkRole.Governor || perk.SecondaryRole == SkillEffect.PerkRole.Governor)
			{
				Hero governor = town.Governor;
				if (governor != null && governor.GetPerkValue(perk) && governor.CurrentSettlement != null && governor.CurrentSettlement == town.Settlement)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000B844 File Offset: 0x00009A44
		public static List<PerkObject> GetGovernorPerksForHero(Hero hero)
		{
			List<PerkObject> list = new List<PerkObject>();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if ((perkObject.PrimaryRole == SkillEffect.PerkRole.Governor || perkObject.SecondaryRole == SkillEffect.PerkRole.Governor) && hero.GetPerkValue(perkObject))
				{
					list.Add(perkObject);
				}
			}
			return list;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000B8B8 File Offset: 0x00009AB8
		public static ValueTuple<TextObject, TextObject> GetGovernorEngineeringSkillEffectForHero(Hero governor)
		{
			if (governor != null && governor.GetSkillValue(DefaultSkills.Engineering) > 0)
			{
				SkillEffect townProjectBuildingBonus = DefaultSkillEffects.TownProjectBuildingBonus;
				TextObject description = townProjectBuildingBonus.Description;
				float num = (townProjectBuildingBonus.PrimaryRole == SkillEffect.PerkRole.Governor) ? townProjectBuildingBonus.PrimaryBonus : townProjectBuildingBonus.SecondaryBonus;
				description.SetTextVariable("a0", (float)governor.GetSkillValue(DefaultSkills.Engineering) * num);
				return new ValueTuple<TextObject, TextObject>(DefaultSkills.Engineering.Name, description);
			}
			return new ValueTuple<TextObject, TextObject>(TextObject.Empty, new TextObject("{=0rBsbw1T}No effect", null));
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000B93C File Offset: 0x00009B3C
		public static void SetDescriptionTextVariable(TextObject description, float bonus, SkillEffect.EffectIncrementType effectIncrementType)
		{
			float num = (effectIncrementType == SkillEffect.EffectIncrementType.AddFactor) ? (bonus * 100f) : bonus;
			string text = string.Format("{0:0.#}", num);
			if (bonus > 0f)
			{
				description.SetTextVariable("VALUE", "+" + text);
				return;
			}
			description.SetTextVariable("VALUE", text ?? "");
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		public static int AvailablePerkCountOfHero(Hero hero)
		{
			MBList<PerkObject> mblist = new MBList<PerkObject>();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				SkillObject skill = perkObject.Skill;
				if ((float)hero.GetSkillValue(skill) >= perkObject.RequiredSkillValue && !hero.GetPerkValue(perkObject) && (perkObject.AlternativePerk == null || !hero.GetPerkValue(perkObject.AlternativePerk)) && !mblist.Contains(perkObject.AlternativePerk))
				{
					mblist.Add(perkObject);
				}
			}
			return mblist.Count;
		}
	}
}
