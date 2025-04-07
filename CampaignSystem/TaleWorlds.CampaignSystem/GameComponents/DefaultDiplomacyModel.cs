using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000101 RID: 257
	public class DefaultDiplomacyModel : DiplomacyModel
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600155B RID: 5467 RVA: 0x00062C72 File Offset: 0x00060E72
		public override int MinimumRelationWithConversationCharacterToJoinKingdom
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x00062C76 File Offset: 0x00060E76
		public override int GiftingTownRelationshipBonus
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600155D RID: 5469 RVA: 0x00062C7A File Offset: 0x00060E7A
		public override int GiftingCastleRelationshipBonus
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x00062C7E File Offset: 0x00060E7E
		public override int MaxRelationLimit
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600155F RID: 5471 RVA: 0x00062C82 File Offset: 0x00060E82
		public override int MinRelationLimit
		{
			get
			{
				return -100;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x00062C86 File Offset: 0x00060E86
		public override int MaxNeutralRelationLimit
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x00062C8A File Offset: 0x00060E8A
		public override int MinNeutralRelationLimit
		{
			get
			{
				return -10;
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00062C8E File Offset: 0x00060E8E
		public override float GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(Kingdom kingdomToJoin)
		{
			return kingdomToJoin.TotalStrength * 0.05f;
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00062C9C File Offset: 0x00060E9C
		public override float GetClanStrength(Clan clan)
		{
			float num = 0f;
			foreach (Hero hero in clan.Heroes)
			{
				num += this.GetHeroCommandingStrengthForClan(hero);
			}
			float num2 = 1.2f;
			float num3 = clan.Influence * num2;
			float num4 = 4f;
			float num5 = (float)clan.Settlements.Count * num4;
			return num + num3 + num5;
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00062D28 File Offset: 0x00060F28
		public override float GetHeroCommandingStrengthForClan(Hero hero)
		{
			if (!hero.IsAlive)
			{
				return 0f;
			}
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			float num4 = 1f;
			float num5 = 0.1f;
			float num6 = 5f;
			float num7 = (float)hero.GetSkillValue(DefaultSkills.Tactics) * num;
			float num8 = (float)hero.GetSkillValue(DefaultSkills.Steward) * num2;
			float num9 = (float)hero.GetSkillValue(DefaultSkills.Trade) * num3;
			float num10 = (float)hero.GetSkillValue(DefaultSkills.Leadership) * num4;
			float num11 = (float)((hero.GetTraitLevel(DefaultTraits.Commander) > 0) ? 300 : 0);
			float num12 = (float)hero.Gold * num5;
			float num13 = (hero.PartyBelongedTo != null) ? (num6 * hero.PartyBelongedTo.Party.TotalStrength) : 0f;
			float num14 = 0f;
			if (hero.Clan.Leader == hero)
			{
				num14 += 500f;
			}
			float num15 = 0f;
			if (hero.Father == hero.Clan.Leader || hero.Clan.Leader.Father == hero || hero.Mother == hero.Clan.Leader || hero.Clan.Leader.Mother == hero)
			{
				num15 += 100f;
			}
			float num16 = 0f;
			if (hero.IsNoncombatant)
			{
				num16 -= 250f;
			}
			float num17 = 0f;
			if (hero.GovernorOf != null)
			{
				num17 -= 250f;
			}
			float num18 = num11 + num7 + num8 + num9 + num10 + num12 + num13 + num14 + num15 + num16 + num17;
			if (num18 <= 0f)
			{
				return 0f;
			}
			return num18;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00062EDC File Offset: 0x000610DC
		public override float GetHeroGoverningStrengthForClan(Hero hero)
		{
			if (hero.IsAlive)
			{
				float num = 0.3f;
				float num2 = 0.9f;
				float num3 = 0.8f;
				float num4 = 1.2f;
				float num5 = 1f;
				float num6 = 0.005f;
				float num7 = 2f;
				float num8 = (float)hero.GetSkillValue(DefaultSkills.Tactics) * num;
				float num9 = (float)hero.GetSkillValue(DefaultSkills.Charm) * num2;
				float num10 = (float)hero.GetSkillValue(DefaultSkills.Engineering) * num3;
				float num11 = (float)hero.GetSkillValue(DefaultSkills.Steward) * num7;
				float num12 = (float)hero.GetSkillValue(DefaultSkills.Trade) * num4;
				float num13 = (float)hero.GetSkillValue(DefaultSkills.Leadership) * num5;
				float num14 = (float)((hero.GetTraitLevel(DefaultTraits.Honor) > 0) ? 100 : 0);
				float num15 = (float)MathF.Min(100000, hero.Gold) * num6;
				float num16 = 0f;
				if (hero.Spouse == hero.Clan.Leader)
				{
					num16 += 1000f;
				}
				if (hero.Father == hero.Clan.Leader || hero.Clan.Leader.Father == hero || hero.Mother == hero.Clan.Leader || hero.Clan.Leader.Mother == hero)
				{
					num16 += 750f;
				}
				if (hero.Siblings.Contains(hero.Clan.Leader))
				{
					num16 += 500f;
				}
				return num14 + num8 + num11 + num12 + num13 + num15 + num16 + num9 + num10;
			}
			return 0f;
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00063068 File Offset: 0x00061268
		public override float GetRelationIncreaseFactor(Hero hero1, Hero hero2, float relationChange)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(relationChange, false, null);
			Hero hero3;
			if (hero1.IsHumanPlayerCharacter || hero2.IsHumanPlayerCharacter)
			{
				hero3 = (hero1.IsHumanPlayerCharacter ? hero1 : hero2);
			}
			else
			{
				hero3 = ((MBRandom.RandomFloat < 0.5f) ? hero1 : hero2);
			}
			SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Charm, DefaultSkillEffects.CharmRelationBonus, hero3.CharacterObject, ref explainedNumber, -1, true, 0);
			if (hero1.IsFemale != hero2.IsFemale)
			{
				if (hero3.GetPerkValue(DefaultPerks.Charm.InBloom))
				{
					explainedNumber.AddFactor(DefaultPerks.Charm.InBloom.PrimaryBonus, null);
				}
			}
			else if (hero3.GetPerkValue(DefaultPerks.Charm.YoungAndRespectful))
			{
				explainedNumber.AddFactor(DefaultPerks.Charm.YoungAndRespectful.PrimaryBonus, null);
			}
			if (hero3.GetPerkValue(DefaultPerks.Charm.GoodNatured) && hero2.GetTraitLevel(DefaultTraits.Mercy) > 0)
			{
				explainedNumber.Add(DefaultPerks.Charm.GoodNatured.SecondaryBonus, DefaultPerks.Charm.GoodNatured.Name, null);
			}
			if (hero3.GetPerkValue(DefaultPerks.Charm.Tribute) && hero2.GetTraitLevel(DefaultTraits.Mercy) < 0)
			{
				explainedNumber.Add(DefaultPerks.Charm.Tribute.SecondaryBonus, DefaultPerks.Charm.Tribute.Name, null);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0006318C File Offset: 0x0006138C
		public override int GetInfluenceAwardForSettlementCapturer(Settlement settlement)
		{
			if (settlement.IsTown || settlement.IsCastle)
			{
				int num = settlement.IsTown ? 30 : 10;
				int num2 = 0;
				foreach (Village village in settlement.BoundVillages)
				{
					num2 += this.GetInfluenceAwardForSettlementCapturer(village.Settlement);
				}
				return num + num2;
			}
			return 10;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00063210 File Offset: 0x00061410
		public override float GetHourlyInfluenceAwardForBeingArmyMember(MobileParty mobileParty)
		{
			float totalStrength = mobileParty.Party.TotalStrength;
			float num = 0.0001f * (20f + totalStrength);
			if (mobileParty.BesiegedSettlement != null || mobileParty.MapEvent != null)
			{
				num *= 2f;
			}
			return num;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00063250 File Offset: 0x00061450
		public override float GetHourlyInfluenceAwardForRaidingEnemyVillage(MobileParty mobileParty)
		{
			int num = 0;
			foreach (MapEventParty mapEventParty in mobileParty.MapEvent.AttackerSide.Parties)
			{
				if (mapEventParty.Party.MobileParty != mobileParty)
				{
					MobileParty mobileParty2 = mapEventParty.Party.MobileParty;
					if (((mobileParty2 != null) ? mobileParty2.Army : null) == null || mapEventParty.Party.MobileParty.Army.LeaderParty != mobileParty)
					{
						continue;
					}
				}
				num += mapEventParty.Party.MemberRoster.TotalManCount;
			}
			return (MathF.Sqrt((float)num) + 2f) / 240f;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00063310 File Offset: 0x00061510
		public override float GetHourlyInfluenceAwardForBesiegingEnemyFortification(MobileParty mobileParty)
		{
			int num = 0;
			foreach (PartyBase partyBase in mobileParty.BesiegedSettlement.SiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
			{
				if (partyBase.MobileParty == mobileParty || (partyBase.MobileParty.Army != null && partyBase.MobileParty.Army.LeaderParty == mobileParty))
				{
					num += partyBase.MemberRoster.TotalManCount;
				}
			}
			return (MathF.Sqrt((float)num) + 2f) / 240f;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x000633B4 File Offset: 0x000615B4
		public override float GetScoreOfClanToJoinKingdom(Clan clan, Kingdom kingdom)
		{
			if (clan.Kingdom != null && clan.Kingdom.RulingClan == clan)
			{
				return -100000000f;
			}
			int relationBetweenClans = FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan);
			int num = 0;
			int num2 = 0;
			foreach (Clan clan2 in kingdom.Clans)
			{
				int relationBetweenClans2 = FactionManager.GetRelationBetweenClans(clan, clan2);
				num += relationBetweenClans2;
				num2++;
			}
			float num3 = (num2 > 0) ? ((float)num / (float)num2) : 0f;
			float num4 = MathF.Max(-100f, MathF.Min(100f, (float)relationBetweenClans + num3));
			float num5 = MathF.Min(2f, MathF.Max(0.33f, 1f + MathF.Sqrt(MathF.Abs(num4)) * ((num4 < 0f) ? -0.067f : 0.1f)));
			float num6 = 1f + ((kingdom.Culture == clan.Culture) ? 0.15f : ((kingdom.Leader == Hero.MainHero) ? 0f : -0.15f));
			float num7 = clan.CalculateTotalSettlementBaseValue();
			float num8 = clan.CalculateTotalSettlementValueForFaction(kingdom);
			int commanderLimit = clan.CommanderLimit;
			float num9 = 0f;
			float num10 = 0f;
			if (!clan.IsMinorFaction)
			{
				float num11 = 0f;
				foreach (Town town in kingdom.Fiefs)
				{
					num11 += town.Settlement.GetSettlementValueForFaction(kingdom);
				}
				int num12 = 0;
				foreach (Clan clan3 in kingdom.Clans)
				{
					if (!clan3.IsUnderMercenaryService && clan3 != clan)
					{
						num12 += clan3.CommanderLimit;
					}
				}
				num9 = num11 / (float)(num12 + commanderLimit);
				num10 = -((float)(num12 * num12) * 100f) + 10000f;
			}
			int gold = clan.Leader.Gold;
			float num13 = 0.5f * MathF.Min(1000000f, (clan.Kingdom != null) ? ((float)clan.Kingdom.KingdomBudgetWallet) : 0f) / ((clan.Kingdom != null) ? ((float)clan.Kingdom.Clans.Count + 1f) : 2f);
			float num14 = 0.15f;
			float num15 = num9 * MathF.Sqrt((float)commanderLimit) * num14 * 0.2f;
			num15 *= num5 * num6;
			num15 += (clan.MapFaction.IsAtWarWith(kingdom) ? (num8 - num7) : 0f);
			num15 += num10;
			if (clan.Kingdom != null && clan.Kingdom.Leader == Hero.MainHero && num15 > 0f)
			{
				num15 *= 0.2f;
			}
			return num15;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000636C0 File Offset: 0x000618C0
		public override float GetScoreOfClanToLeaveKingdom(Clan clan, Kingdom kingdom)
		{
			int relationBetweenClans = FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan);
			int num = 0;
			int num2 = 0;
			foreach (Clan clan2 in kingdom.Clans)
			{
				int relationBetweenClans2 = FactionManager.GetRelationBetweenClans(clan, clan2);
				num += relationBetweenClans2;
				num2++;
			}
			float num3 = (num2 > 0) ? ((float)num / (float)num2) : 0f;
			float num4 = MathF.Max(-100f, MathF.Min(100f, (float)relationBetweenClans + num3));
			float num5 = MathF.Min(2f, MathF.Max(0.33f, 1f + MathF.Sqrt(MathF.Abs(num4)) * ((num4 < 0f) ? -0.067f : 0.1f)));
			float num6 = 1f + ((kingdom.Culture == clan.Culture) ? 0.15f : ((kingdom.Leader == Hero.MainHero) ? 0f : -0.15f));
			float num7 = clan.CalculateTotalSettlementBaseValue();
			float num8 = clan.CalculateTotalSettlementValueForFaction(kingdom);
			int commanderLimit = clan.CommanderLimit;
			float num9 = 0f;
			if (!clan.IsMinorFaction)
			{
				float num10 = 0f;
				foreach (Town town in kingdom.Fiefs)
				{
					num10 += town.Settlement.GetSettlementValueForFaction(kingdom);
				}
				int num11 = 0;
				foreach (Clan clan3 in kingdom.Clans)
				{
					if (!clan3.IsUnderMercenaryService && clan3 != clan)
					{
						num11 += clan3.CommanderLimit;
					}
				}
				num9 = num10 / (float)(num11 + commanderLimit);
			}
			float num12 = HeroHelper.CalculateReliabilityConstant(clan.Leader, 1f);
			float b = (float)(CampaignTime.Now - clan.LastFactionChangeTime).ToDays;
			float num13 = 4000f * (15f - MathF.Sqrt(MathF.Min(225f, b)));
			int num14 = 0;
			int num15 = 0;
			using (List<Town>.Enumerator enumerator2 = clan.Fiefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.IsCastle)
					{
						num15++;
					}
					else
					{
						num14++;
					}
				}
			}
			float num16 = -70000f - (float)num15 * 10000f - (float)num14 * 30000f;
			int gold = clan.Leader.Gold;
			float num17 = 0.5f * (float)MathF.Min(1000000, (clan.Kingdom != null) ? clan.Kingdom.KingdomBudgetWallet : 0) / ((float)clan.Kingdom.Clans.Count + 1f);
			float num18 = 0.15f;
			num16 /= num18;
			float num19 = -num9 * MathF.Sqrt((float)commanderLimit) * num18 * 0.2f + num16 * num12 + -num13;
			num19 *= num5 * num6;
			if (num5 < 1f && num7 - num8 < 0f)
			{
				num19 += num5 * (num7 - num8);
			}
			else
			{
				num19 += num7 - num8;
			}
			if (num5 < 1f)
			{
				num19 += (1f - num5) * 200000f;
			}
			if (kingdom.Leader == Hero.MainHero)
			{
				if (num19 > 0f)
				{
					num19 *= 0.2f;
				}
				else
				{
					num19 *= 5f;
				}
			}
			return num19 + ((kingdom.Leader == Hero.MainHero) ? (-(1000000f * num5)) : 0f);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00063A94 File Offset: 0x00061C94
		public override float GetScoreOfKingdomToGetClan(Kingdom kingdom, Clan clan)
		{
			float num = MathF.Min(2f, MathF.Max(0.33f, 1f + 0.02f * (float)FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan)));
			float num2 = 1f + ((kingdom.Culture == clan.Culture) ? 1f : 0f);
			int commanderLimit = clan.CommanderLimit;
			float num3 = (clan.TotalStrength + 150f * (float)commanderLimit) * 20f;
			float powerRatioToEnemies = FactionHelper.GetPowerRatioToEnemies(kingdom);
			float num4 = HeroHelper.CalculateReliabilityConstant(clan.Leader, 1f);
			float num5 = 1f / MathF.Max(0.4f, MathF.Min(2.5f, MathF.Sqrt(powerRatioToEnemies)));
			num3 *= num5;
			return (clan.CalculateTotalSettlementValueForFaction(kingdom) * 0.1f + num3) * num * num2 * num4;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00063B68 File Offset: 0x00061D68
		public override float GetScoreOfKingdomToSackClan(Kingdom kingdom, Clan clan)
		{
			float num = MathF.Min(2f, MathF.Max(0.33f, 1f + 0.02f * (float)FactionManager.GetRelationBetweenClans(kingdom.RulingClan, clan)));
			float num2 = 1f + ((kingdom.Culture == clan.Culture) ? 1f : 0.5f);
			int commanderLimit = clan.CommanderLimit;
			float num3 = (clan.TotalStrength + 150f * (float)commanderLimit) * 20f;
			float num4 = clan.CalculateTotalSettlementValueForFaction(kingdom);
			return 10f - 1f * num3 * num2 * num - num4;
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00063C00 File Offset: 0x00061E00
		public override float GetScoreOfMercenaryToJoinKingdom(Clan mercenaryClan, Kingdom kingdom)
		{
			int num = (mercenaryClan.Kingdom == kingdom) ? mercenaryClan.MercenaryAwardMultiplier : Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(mercenaryClan, kingdom, false);
			float num2 = mercenaryClan.TotalStrength + (float)mercenaryClan.CommanderLimit * 50f;
			int mercenaryAwardFactorToJoinKingdom = Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(mercenaryClan, kingdom, true);
			if (kingdom.Leader == Hero.MainHero)
			{
				return 0f;
			}
			return (float)(num - mercenaryAwardFactorToJoinKingdom) * num2 * 0.5f;
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00063C84 File Offset: 0x00061E84
		public override float GetScoreOfMercenaryToLeaveKingdom(Clan mercenaryClan, Kingdom kingdom)
		{
			float num = 0.005f * MathF.Min(200f, mercenaryClan.LastFactionChangeTime.ElapsedDaysUntilNow);
			return 10000f * num - 5000f - this.GetScoreOfMercenaryToJoinKingdom(mercenaryClan, kingdom);
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00063CC8 File Offset: 0x00061EC8
		public override float GetScoreOfKingdomToHireMercenary(Kingdom kingdom, Clan mercenaryClan)
		{
			int num = 0;
			foreach (Clan clan in kingdom.Clans)
			{
				num += clan.CommanderLimit;
			}
			float num2 = (float)((num < 12) ? ((12 - num) * 100) : 0);
			int count = kingdom.Settlements.Count;
			int num3 = (count < 40) ? ((40 - count) * 30) : 0;
			return num2 + (float)num3;
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x00063D50 File Offset: 0x00061F50
		public override float GetScoreOfKingdomToSackMercenary(Kingdom kingdom, Clan mercenaryClan)
		{
			float b = ((float)kingdom.Leader.Gold > 20000f) ? (MathF.Sqrt((float)kingdom.Leader.Gold / 20000f) - 1f) : -1f;
			int relationBetweenClans = FactionManager.GetRelationBetweenClans(kingdom.RulingClan, mercenaryClan);
			float num = MathF.Min(5f, FactionHelper.GetPowerRatioToEnemies(kingdom));
			return (MathF.Min(2f + (float)relationBetweenClans / 100f - num, b) * -1f - 0.1f) * 50f * mercenaryClan.TotalStrength * 5f;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00063DE8 File Offset: 0x00061FE8
		public override float GetScoreOfDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, IFaction evaluatingClan, out TextObject peaceReason)
		{
			float num = -this.GetScoreOfWarInternal(factionDeclaresPeace, factionDeclaredPeace, evaluatingClan, true, out peaceReason);
			float num2 = 1f;
			if (num > 0f)
			{
				float num3 = (factionDeclaredPeace.Leader == Hero.MainHero) ? 0.12f : ((Hero.MainHero.MapFaction == factionDeclaredPeace) ? 0.24f : 0.36f);
				num2 *= num3 + (0.84f - num3) * (100f - factionDeclaredPeace.Aggressiveness) * 0.01f;
			}
			int num4 = (factionDeclaredPeace.Leader == Hero.MainHero || factionDeclaresPeace.Leader == Hero.MainHero) ? (MathF.Min(Hero.MainHero.Level + 1, 31) * 20) : 0;
			int num5 = -(int)MathF.Min(180000f, (MathF.Min(10000f, factionDeclaresPeace.TotalStrength) + 2000f + (float)num4) * (MathF.Min(10000f, factionDeclaredPeace.TotalStrength) + 2000f + (float)num4) * 0.00018f);
			return (float)((int)(num2 * num) + num5);
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00063EE4 File Offset: 0x000620E4
		private float GetWarFatiqueScoreNew(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingClan)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			foreach (Town town in factionDeclaresWar.Fiefs)
			{
				int num8 = 1;
				if (town.OwnerClan == evaluatingClan || (evaluatingClan.IsKingdomFaction && town.OwnerClan.Leader == evaluatingClan.Leader))
				{
					num8 = 3;
				}
				int num9 = town.Settlement.IsTown ? 2 : 1;
				num += ((town.Loyalty < 50f) ? ((50f - town.Loyalty) * MathF.Min(6000f, town.Prosperity) * (float)num8 * (float)num9 * 0.00166f) : 0f);
				num2 += (float)num9 * ((town.FoodStocks < 100f) ? ((100f - town.FoodStocks) * (float)num8) : 0f);
				num6 += num8 * num9;
				if (town.GarrisonParty == null)
				{
					num3 += 100f * (float)num8 * (float)num9;
				}
				else
				{
					float totalStrength = town.GarrisonParty.Party.TotalStrength;
					MilitiaPartyComponent militiaPartyComponent = town.Settlement.MilitiaPartyComponent;
					float num10 = totalStrength + ((militiaPartyComponent != null) ? militiaPartyComponent.MobileParty.Party.TotalStrength : 0f);
					num3 += ((num10 < (float)(200 * num9)) ? (0.25f * ((float)(200 * num9) - num10) * (float)num8 * (float)num9) : 0f);
				}
				if (town.IsUnderSiege && town.Settlement.SiegeEvent != null && town.Settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapFaction == factionDeclaredWar && (MobileParty.MainParty.SiegeEvent == null || MobileParty.MainParty.SiegeEvent.BesiegedSettlement != town.Settlement))
				{
					num7 += 100 * num8 * num9;
				}
				foreach (Village village in town.Villages)
				{
					float num11 = MathF.Max(0f, 400f - village.Hearth) * 0.2f;
					num11 += (float)((village.VillageState == Village.VillageStates.Looted) ? 20 : 0);
					num4 += num11 * (float)num8;
					num5 += num8;
				}
			}
			float num12 = 0f;
			float num13 = 0f;
			int num14 = 0;
			if (factionDeclaresWar.IsKingdomFaction)
			{
				foreach (Clan clan in ((Kingdom)factionDeclaresWar).Clans)
				{
					int num15 = 1;
					if (clan == evaluatingClan || (evaluatingClan.IsKingdomFaction && clan.Leader == evaluatingClan.Leader))
					{
						num15 = 3;
					}
					int partyLimitForTier = Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan, clan.Tier);
					if (partyLimitForTier > clan.WarPartyComponents.Count)
					{
						num12 += 100f * (float)(partyLimitForTier - clan.WarPartyComponents.Count * num15);
					}
					foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
					{
						if (warPartyComponent.MobileParty.PartySizeRatio < 0.9f)
						{
							num12 += 100f * (0.9f - warPartyComponent.MobileParty.PartySizeRatio) * (float)num15;
						}
						if (warPartyComponent.Party.TotalStrength > (float)warPartyComponent.Party.PartySizeLimit)
						{
							num13 += (warPartyComponent.Party.TotalStrength - (float)warPartyComponent.Party.PartySizeLimit) * (float)num15;
						}
					}
					num14 += partyLimitForTier * num15;
				}
			}
			int num16 = 0;
			int num17 = 0;
			int num18 = 0;
			int num19 = 0;
			foreach (StanceLink stanceLink in factionDeclaresWar.Stances)
			{
				if (stanceLink.Faction1 == factionDeclaresWar && stanceLink.Faction2 == factionDeclaredWar)
				{
					num16 = stanceLink.SuccessfulSieges2;
					num17 = stanceLink.SuccessfulRaids2;
					num18 = stanceLink.SuccessfulSieges1;
					num19 = stanceLink.SuccessfulRaids1;
				}
				else if (stanceLink.Faction1 == factionDeclaredWar && stanceLink.Faction2 == factionDeclaresWar)
				{
					num16 = stanceLink.SuccessfulSieges1;
					num17 = stanceLink.SuccessfulRaids1;
					num18 = stanceLink.SuccessfulSieges2;
					num19 = stanceLink.SuccessfulRaids2;
				}
			}
			int b = evaluatingClan.IsKingdomFaction ? 0 : evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Calculating);
			float num20 = 1f + 0.2f * (float)MathF.Min(2, MathF.Max(-2, b));
			int count = factionDeclaresWar.Fiefs.Count;
			int num21 = factionDeclaresWar.Fiefs.Count * 3;
			float num22 = MathF.Max(0f, (float)num16 - (float)num18 * 0.5f) / ((float)count + 5f) * DefaultDiplomacyModel.HappenedSiegesDifFactor * 100f * num20;
			float num23 = MathF.Max(0f, (float)num17 - (float)num19 * 0.5f) / ((float)num21 + 5f) * DefaultDiplomacyModel.HappenedRaidsDifFactor * 100f * num20;
			float num24 = num12 / (float)(num14 + 2) * DefaultDiplomacyModel.LordRiskValueFactor;
			float num25 = num13 / (float)(num14 + 2) * 0.5f * DefaultDiplomacyModel.LordRiskValueFactor;
			float num26 = num / (float)(num6 + 2) * DefaultDiplomacyModel.LoyalityRiskValueFactor;
			float num27 = num4 / (float)(num5 + 2) * DefaultDiplomacyModel.HearthRiskValueFactor;
			float num28 = num2 / (float)(num6 + 2) * DefaultDiplomacyModel.FoodRiskValueFactor;
			float num29 = num3 / (float)(num6 + 2) * DefaultDiplomacyModel.GarrisonRiskValueFactor;
			float num30 = (float)(num7 / (num6 + 2)) * DefaultDiplomacyModel.SiegeRiskValueFactor;
			return MathF.Min(300000f, num30 + num24 - num25 + num26 + num27 + num28 + num29 + num22 + num23);
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x0006455C File Offset: 0x0006275C
		private DefaultDiplomacyModel.WarStats CalculateWarStats(IFaction faction, IFaction targetFaction)
		{
			float num = faction.TotalStrength * 0.85f;
			float num2 = 0f;
			int num3 = 0;
			foreach (Town town in faction.Fiefs)
			{
				num3 += (town.IsCastle ? 1 : 2);
			}
			if (faction.IsKingdomFaction)
			{
				foreach (Clan clan in ((Kingdom)faction).Clans)
				{
					if (!clan.IsUnderMercenaryService)
					{
						int partyLimitForTier = Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan, clan.Tier);
						num2 += (float)partyLimitForTier * 80f * ((clan.Leader == clan.MapFaction.Leader) ? 1.25f : 1f);
					}
				}
			}
			num += num2;
			Clan rulingClan = faction.IsClan ? (faction as Clan) : (faction as Kingdom).RulingClan;
			float valueOfSettlements = faction.Fiefs.Sum((Town f) => (float)(f.IsTown ? 2000 : 1000) + f.Prosperity * 0.33f) * DefaultDiplomacyModel.ProsperityValueFactor;
			float num4 = 0f;
			float num5 = 0f;
			foreach (StanceLink stanceLink in faction.Stances)
			{
				if (stanceLink.IsAtWar && stanceLink.Faction1 != targetFaction && stanceLink.Faction2 != targetFaction && (!stanceLink.Faction2.IsMinorFaction || stanceLink.Faction2.Leader == Hero.MainHero))
				{
					IFaction faction2 = (stanceLink.Faction1 == faction) ? stanceLink.Faction2 : stanceLink.Faction1;
					if (faction2.IsKingdomFaction)
					{
						foreach (Clan clan2 in ((Kingdom)faction2).Clans)
						{
							if (!clan2.IsUnderMercenaryService)
							{
								num4 += (float)clan2.Tier * 80f * ((clan2.Leader == clan2.MapFaction.Leader) ? 1.5f : 1f);
							}
						}
					}
					num5 += faction2.TotalStrength;
				}
			}
			num5 += num4;
			num *= MathF.Sqrt(MathF.Sqrt((float)MathF.Min(num3 + 4, 40))) / 2.5f;
			return new DefaultDiplomacyModel.WarStats
			{
				RulingClan = rulingClan,
				Strength = num,
				ValueOfSettlements = valueOfSettlements,
				TotalStrengthOfEnemies = num5
			};
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x00064890 File Offset: 0x00062A90
		[return: TupleElementNames(new string[]
		{
			"kingdom1",
			"kingdom1Score",
			"kingdom2",
			"kingdom2Score"
		})]
		private ValueTuple<Kingdom, float, Kingdom, float> GetTopDogs()
		{
			ValueTuple<Kingdom, Kingdom, Kingdom> valueTuple = MBMath.MaxElements3<Kingdom>(Kingdom.All, (Kingdom k) => k.TotalStrength);
			Kingdom item = valueTuple.Item1;
			Kingdom item2 = valueTuple.Item2;
			Kingdom item3 = valueTuple.Item3;
			float num = (item != null) ? item.TotalStrength : 400f;
			float num2 = (item2 != null) ? item2.TotalStrength : 300f;
			float num3 = (item3 != null) ? item3.TotalStrength : ((item2 != null) ? item2.TotalStrength : 200f);
			if (num3 <= 3000f)
			{
				num3 = 3000f;
			}
			float item4 = num / num3;
			float item5 = num2 / num3;
			return new ValueTuple<Kingdom, float, Kingdom, float>(item, item4, item2, item5);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00064940 File Offset: 0x00062B40
		private float GetTopDogScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			float result = 0f;
			ValueTuple<Kingdom, float, Kingdom, float> topDogs = this.GetTopDogs();
			Kingdom item = topDogs.Item1;
			float item2 = topDogs.Item2;
			Kingdom item3 = topDogs.Item3;
			float item4 = topDogs.Item4;
			if (item == factionDeclaresWar)
			{
				return 0f;
			}
			if (factionDeclaredWar == item)
			{
				result = DefaultDiplomacyModel.StrengthValueFactor * 2f * (factionDeclaresWar.TotalStrength + 1f) * (0.3f * (item2 - 0.9f));
			}
			else if (factionDeclaredWar.IsAtWarWith(item))
			{
				result = -DefaultDiplomacyModel.StrengthValueFactor * 2f * (factionDeclaresWar.TotalStrength + 1f) * (0.2f * (item2 - 0.9f));
			}
			if (factionDeclaredWar == item3)
			{
				result = DefaultDiplomacyModel.StrengthValueFactor * 2f * (factionDeclaresWar.TotalStrength + 1f) * (0.3f * (item4 - 0.9f));
			}
			else if (factionDeclaredWar.IsAtWarWith(item))
			{
				result = -DefaultDiplomacyModel.StrengthValueFactor * 2f * (factionDeclaresWar.TotalStrength + 1f) * (0.2f * (item4 - 0.9f));
			}
			return result;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00064A40 File Offset: 0x00062C40
		private float GetBottomScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			float result = 0f;
			ValueTuple<Kingdom, float, Kingdom, float> topDogs = this.GetTopDogs();
			Kingdom item = topDogs.Item1;
			float item2 = topDogs.Item2;
			if (factionDeclaredWar == item)
			{
				result = DefaultDiplomacyModel.StrengthValueFactor * factionDeclaresWar.TotalStrength * (0.2f * item2);
			}
			return result;
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x00064A84 File Offset: 0x00062C84
		private float CalculateClanRiskScoreOfWar(float squareRootOfPowerRatio, IFaction factionDeclaredWar, IFaction evaluatingClan)
		{
			float num = 0f;
			if (squareRootOfPowerRatio > 0.5f)
			{
				foreach (Town town in evaluatingClan.Fiefs)
				{
					float num2 = Campaign.MapDiagonal * 2f;
					float num3 = Campaign.MapDiagonal * 2f;
					foreach (Town town2 in factionDeclaredWar.Fiefs)
					{
						if (town2.IsTown)
						{
							float length = (town.Settlement.GetPosition2D - town2.Settlement.GetPosition2D).Length;
							if (length < num2)
							{
								num3 = num2;
								num2 = length;
							}
							else if (length < num3)
							{
								num3 = length;
							}
						}
					}
					float num4 = (num2 + num3) / 2f;
					if (num4 < Campaign.AverageDistanceBetweenTwoFortifications * 3f)
					{
						float num5 = MathF.Min(Campaign.AverageDistanceBetweenTwoFortifications * 3f - num4, Campaign.AverageDistanceBetweenTwoFortifications * 2f) / (Campaign.AverageDistanceBetweenTwoFortifications * 2f);
						float num6 = MathF.Min(7.5f, (squareRootOfPowerRatio - 0.5f) * 5f);
						num6 += 0.5f;
						int num7 = town.IsTown ? 2 : 1;
						num += num5 * num6 * (float)num7 * (2000f + MathF.Min(8000f, town.Prosperity));
					}
				}
			}
			return num;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00064C3C File Offset: 0x00062E3C
		private float GetScoreOfWarInternal(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingClan, bool evaluatingPeace, out TextObject reason)
		{
			reason = TextObject.Empty;
			if (factionDeclaresWar.MapFaction == factionDeclaredWar.MapFaction)
			{
				return 0f;
			}
			DefaultDiplomacyModel.WarStats warStats = this.CalculateWarStats(factionDeclaresWar, factionDeclaredWar);
			DefaultDiplomacyModel.WarStats warStats2 = this.CalculateWarStats(factionDeclaredWar, factionDeclaresWar);
			float distance = this.GetDistance(factionDeclaresWar, factionDeclaredWar);
			float num = (483f + 8.63f * Campaign.AverageDistanceBetweenTwoFortifications) / 2f;
			float num2 = (factionDeclaresWar.Leader == Hero.MainHero || factionDeclaredWar.Leader == Hero.MainHero) ? -300000f : -400000f;
			float num3;
			if (distance - Campaign.AverageDistanceBetweenTwoFortifications * 1.5f > num)
			{
				num3 = num2;
			}
			else if (distance - Campaign.AverageDistanceBetweenTwoFortifications * 1.5f < 0f)
			{
				num3 = 0f;
			}
			else
			{
				float num4 = num - Campaign.AverageDistanceBetweenTwoFortifications * 1.5f;
				float num5 = -num2 / MathF.Pow(num4, 1.6f);
				float num6 = distance - Campaign.AverageDistanceBetweenTwoFortifications * 1.5f;
				num3 = num2 + num5 * MathF.Pow(MathF.Pow(num4 - num6, 8f), 0.2f);
				if (num3 > 0f)
				{
					num3 = 0f;
				}
			}
			float num7 = 1f - MathF.Pow(num3 / num2, 0.55f);
			num7 = 0.1f + num7 * 0.9f;
			float num8 = evaluatingClan.IsKingdomFaction ? 0f : evaluatingClan.Leader.RandomFloat(-20000f, 20000f);
			int valorLevelOfEvaluatingClan = MathF.Max(-2, MathF.Min(2, evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Valor)));
			float num9 = this.CalculateBenefitScore(ref warStats, ref warStats2, valorLevelOfEvaluatingClan, evaluatingPeace, distance, false);
			float num10 = this.CalculateBenefitScore(ref warStats2, ref warStats, valorLevelOfEvaluatingClan, evaluatingPeace, distance, true);
			float num11 = 0f;
			float squareRootOfPowerRatio = MathF.Min(2f, MathF.Sqrt((warStats2.Strength + 1000f) / (warStats.Strength + 1000f)));
			if (evaluatingClan.IsKingdomFaction)
			{
				int num12 = 0;
				foreach (Clan evaluatingClan2 in ((Kingdom)evaluatingClan).Clans)
				{
					num11 += this.CalculateClanRiskScoreOfWar(squareRootOfPowerRatio, factionDeclaredWar, evaluatingClan2);
					num12++;
				}
				if (num12 > 0)
				{
					num11 /= (float)num12;
				}
			}
			else
			{
				num11 = this.CalculateClanRiskScoreOfWar(squareRootOfPowerRatio, factionDeclaredWar, evaluatingClan);
			}
			num11 = MathF.Min(200000f, num11);
			float warFatiqueScoreNew = this.GetWarFatiqueScoreNew(factionDeclaresWar, factionDeclaredWar, evaluatingClan);
			float topDogScore = this.GetTopDogScore(factionDeclaresWar, factionDeclaredWar);
			float relationWithClan = (float)warStats.RulingClan.GetRelationWithClan(warStats2.RulingClan);
			int relationWithClan2 = evaluatingClan.Leader.Clan.GetRelationWithClan(warStats2.RulingClan);
			float num13 = (relationWithClan + (float)relationWithClan2) / 2f;
			num9 *= 0.7f + 0.3f * (100f - num13) * 0.01f;
			float num14 = (num13 < 0f) ? ((factionDeclaresWar.TotalStrength > factionDeclaredWar.TotalStrength * 2f) ? (-500f * num13) : (-500f * (factionDeclaresWar.TotalStrength / (2f * factionDeclaredWar.TotalStrength)) * (factionDeclaresWar.TotalStrength / (2f * factionDeclaredWar.TotalStrength)) * num13)) : 0f;
			num14 *= ((factionDeclaredWar.Leader == Hero.MainHero) ? 1.5f : 1f);
			float num15 = 1f + 0.002f * factionDeclaredWar.Aggressiveness * ((factionDeclaredWar.Leader == Hero.MainHero) ? 1.5f : 1f);
			num9 *= num15;
			if (factionDeclaredWar.Leader == Hero.MainHero && evaluatingPeace)
			{
				num10 /= num15;
			}
			float num16 = 0.3f * MathF.Min(100000f, factionDeclaredWar.Settlements.Sum(delegate(Settlement s)
			{
				if (s.Culture != factionDeclaresWar.Culture || !s.IsFortification)
				{
					return 0f;
				}
				return s.Town.Prosperity * 0.5f * DefaultDiplomacyModel.ProsperityValueFactor;
			}));
			int num17 = 0;
			foreach (Town town in factionDeclaresWar.Fiefs)
			{
				num17 += (town.IsTown ? 2 : 1);
			}
			if (num17 > 0)
			{
			}
			float num18 = 0.1f + 0.9f * MathF.Min(MathF.Min(num9, num10), 100000f) / 100000f;
			float num19 = num9 - num10;
			if (!evaluatingClan.IsKingdomFaction && evaluatingClan.Leader != evaluatingClan.MapFaction.Leader)
			{
				if (num19 > 0f && evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Mercy) != 0)
				{
					num19 *= 1f - 0.1f * (float)MathF.Min(2, MathF.Max(-2, evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Mercy)));
				}
				if (num19 < 0f && evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Valor) != 0)
				{
					num19 *= 1f - 0.1f * (float)MathF.Min(2, MathF.Max(-2, evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Valor)));
				}
			}
			float num20 = 0f;
			if (!evaluatingClan.IsKingdomFaction && warStats.Strength > warStats2.Strength && evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Mercy) > 0)
			{
				num20 -= MathF.Min((warStats.Strength + 500f) / (warStats2.Strength + 500f) - 1f, 2f) * 5000f * (float)evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Mercy);
			}
			if (!evaluatingClan.IsKingdomFaction && warStats.Strength < warStats2.Strength && evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Valor) > 0)
			{
				num20 += MathF.Min((warStats2.Strength + 500f) / (warStats.Strength + 500f) - 1f, 2f) * 5000f * (float)evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Valor);
			}
			float num21 = 0f;
			float num22 = 0f;
			StanceLink stanceWith = factionDeclaresWar.GetStanceWith(factionDeclaredWar);
			int num23 = 0;
			int num24 = 0;
			if (stanceWith.IsAtWar)
			{
				float elapsedDaysUntilNow = stanceWith.WarStartDate.ElapsedDaysUntilNow;
				int num25 = 60;
				float num26 = 5f;
				num21 = ((elapsedDaysUntilNow > (float)num25) ? ((elapsedDaysUntilNow - (float)num25) * -400f) : (((float)num25 - elapsedDaysUntilNow) * num26 * (400f + 0.2f * MathF.Min(6000f, MathF.Min(warStats.Strength, warStats2.Strength)))));
				if (num21 < 0f && !evaluatingClan.IsKingdomFaction)
				{
					int traitLevel = evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Valor);
					if (traitLevel < 0)
					{
						num21 *= 1f - (float)MathF.Max(traitLevel, -2) * 0.25f;
					}
					else if (traitLevel > 0)
					{
						num21 *= 1f - (float)MathF.Min(traitLevel, 2) * 0.175f;
					}
				}
				foreach (Hero hero in factionDeclaresWar.Heroes)
				{
					int num27 = (hero.Clan == evaluatingClan) ? 3 : 1;
					float num28 = (hero.Clan.Leader == hero) ? 1.5f : 1f;
					double num29 = (hero == hero.MapFaction.Leader) ? 1.5 : 1.0;
					if (hero.IsPrisoner && hero.IsLord && hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.MapFaction == factionDeclaredWar)
					{
						num23 += (int)(num29 * (double)num28 * (double)num27 * 3000.0);
					}
				}
				num24 = num23;
				using (List<Hero>.Enumerator enumerator3 = factionDeclaredWar.Heroes.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Hero hero2 = enumerator3.Current;
						double num30 = (hero2 == hero2.MapFaction.Leader) ? 1.5 : 1.0;
						float num31 = (hero2.Clan.Leader == hero2) ? 1.5f : 1f;
						if (hero2.IsPrisoner && hero2.IsLord && hero2.PartyBelongedToAsPrisoner != null && hero2.PartyBelongedToAsPrisoner.MapFaction == factionDeclaresWar)
						{
							num23 -= (int)(num30 * (double)num31 * 2500.0);
						}
					}
					goto IL_9C0;
				}
			}
			float elapsedDaysUntilNow2 = stanceWith.PeaceDeclarationDate.ElapsedDaysUntilNow;
			num22 = ((elapsedDaysUntilNow2 > 60f) ? 0f : ((60f - elapsedDaysUntilNow2) * -(400f + 0.2f * MathF.Min(6000f, MathF.Min(warStats.Strength, warStats2.Strength)))));
			if (num22 < 0f && !evaluatingClan.IsKingdomFaction)
			{
				int traitLevel2 = evaluatingClan.Leader.GetTraitLevel(DefaultTraits.Honor);
				if (traitLevel2 > 0)
				{
					num22 *= 1f + (float)MathF.Min(traitLevel2, 2) * 0.25f;
				}
				else if (traitLevel2 < 0)
				{
					num22 *= 1f + (float)MathF.Max(traitLevel2, -2) * 0.175f;
				}
			}
			IL_9C0:
			int num32 = factionDeclaresWar.IsKingdomFaction ? (((Kingdom)factionDeclaresWar).PoliticalStagnation * 1000) : 0;
			float num33 = num16 + num8 * num18 + num3 * num18 + num19 + num21 + num22 + (float)num32 * num18 - num11 * num18 + num14 - (float)num23 + num20 * num18;
			float num34 = DefaultDiplomacyModel.StrengthValueFactor * 0.5f * Kingdom.All.Sum(delegate(Kingdom k)
			{
				if (!k.IsAtWarWith(factionDeclaresWar) || !k.IsAtWarWith(factionDeclaredWar) || k.IsMinorFaction)
				{
					return 0f;
				}
				return MathF.Min(k.TotalStrength, factionDeclaredWar.TotalStrength);
			});
			float num35 = topDogScore - num34 - warFatiqueScoreNew;
			num35 *= num18;
			if (evaluatingPeace)
			{
				num33 += (float)((factionDeclaredWar.Leader == Hero.MainHero) ? 10000 : 0);
				if (num33 > 0f)
				{
					num33 += num35;
					if (num33 < 0f)
					{
						num33 *= 0.5f;
					}
				}
				else
				{
					num33 += num35 * 0.75f;
				}
			}
			else
			{
				num33 += num35;
			}
			if (evaluatingPeace)
			{
				float num36 = warFatiqueScoreNew;
				float num37 = -num19;
				float num38 = -num21 * 2f;
				int num39 = num23 * 3 + num24;
				float num40 = -num11 * 3f;
				float num41 = num33 * 0.5f;
				float num42 = -num3 * 0.5f;
				if (num41 > num40 && num41 > (float)num23 && num41 > warFatiqueScoreNew && num41 > num37 && num41 > num38 && num41 > num42)
				{
					reason = new TextObject("{=3JGFdaT7}The {ENEMY_KINGDOM_INFORMAL_NAME} are willing to pay considerable tribute.", null);
				}
				else if (num40 > (float)num39 && num40 > warFatiqueScoreNew && num40 > num37 && num40 > num38 && num40 > num42)
				{
					reason = new TextObject("{=eH0roDGM}Our clan's lands are vulnerable. I owe it to those under my protection to seek peace.", null);
				}
				else if ((float)num39 > num36 && (float)num39 > num37 && (float)num39 > num38 && (float)num23 > num42)
				{
					reason = new TextObject("{=TQmPcVRZ}Too many of our nobles are in captivity. We should make peace to free them.", null);
				}
				else if (num36 >= num37 && num36 >= num38 && warFatiqueScoreNew > num42)
				{
					reason = new TextObject("{=QQtJobYP}We need time to recover from the hardships of war.", null);
				}
				else if (num38 >= num37 && num38 > num42)
				{
					reason = new TextObject("{=lV0VOn99}This war has gone on too long.", null);
				}
				else if (num37 > num42)
				{
					if (warStats.TotalStrengthOfEnemies > 0f && warStats.Strength < warStats.TotalStrengthOfEnemies + warStats2.Strength)
					{
						reason = new TextObject("{=nuqv4GAA}We have too many enemies. We need to make peace with at least some of them.", null);
					}
					else if (warStats.Strength < warStats2.Strength)
					{
						reason = new TextObject("{=JOe3BC41}The {ENEMY_KINGDOM_INFORMAL_NAME} is currently more powerful than us. We need time to build up our strength.", null);
					}
					else if (warStats.ValueOfSettlements > warStats2.ValueOfSettlements)
					{
						reason = new TextObject("{=HqJSNG3M}Our realm is currently doing well, but we stand to lose this wealth if we go on fighting.", null);
					}
					else
					{
						reason = new TextObject("{=vwjs6EjJ}On balance, the gains we stand to make are not worth the costs and risks. ", null);
					}
				}
				else
				{
					reason = new TextObject("{=i0h0LKa0}Our borders are far from those of the enemy. It is too arduous to pursue this war.", null);
				}
				if (!TextObject.IsNullOrEmpty(reason))
				{
					reason.SetTextVariable("ENEMY_KINGDOM_INFORMAL_NAME", factionDeclaredWar.InformalName);
				}
			}
			else
			{
				float num43 = num19;
				int num44 = (relationWithClan2 < 0) ? (-relationWithClan2 * 1000) : 0;
				int num45 = (stanceWith.Faction1 == evaluatingClan.MapFaction) ? ((stanceWith.TotalTributePaidby1 != 0) ? stanceWith.TotalTributePaidby1 : (-stanceWith.TotalTributePaidby2)) : ((stanceWith.TotalTributePaidby1 != 0) ? stanceWith.TotalTributePaidby2 : (-stanceWith.TotalTributePaidby1));
				int num46 = num45 * 70;
				int num47 = num32;
				float num48 = topDogScore;
				float num49 = (100f - factionDeclaredWar.Aggressiveness) * 1000f;
				float num50 = 0f;
				if (factionDeclaredWar.Culture != factionDeclaresWar.Culture)
				{
					int num51 = 0;
					using (List<Town>.Enumerator enumerator2 = factionDeclaredWar.Fiefs.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Culture == factionDeclaresWar.Culture)
							{
								num51++;
							}
						}
					}
					num50 = MathF.Pow((float)num51, 0.7f) * 30000f;
				}
				if ((float)num46 > num43 && num46 > num44 && num46 > num47 && (float)num46 > num50 && (float)num46 > num48 && (float)num46 > num49)
				{
					if (num45 > 1000)
					{
						reason = new TextObject("{=Kt8tBtBG}We are paying too much tribute to the {ENEMY_KINGDOM_INFORMAL_NAME}.", null);
					}
					else
					{
						reason = new TextObject("{=qI4cicQz}It is a disgrace to keep paying tribute to the  {ENEMY_KINGDOM_INFORMAL_NAME}.", null);
					}
				}
				else if ((float)num44 > num43 && num44 > num47 && (float)num44 > num50 && (float)num44 > num48 && (float)num44 > num49)
				{
					reason = new TextObject("{=dov3iRlt}{ENEMY_RULER.NAME} of the {ENEMY_KINGDOM_INFORMAL_NAME} is vile and dangerous. We must deal with {?ENEMY_RULER.GENDER}her{?}him{\\?} before it is too late.", null);
				}
				else if (num43 > (float)num47 && num43 > num50 && num43 > num48)
				{
					if (warStats.TotalStrengthOfEnemies == 0f && warStats.Strength < warStats2.Strength)
					{
						reason = new TextObject("{=1aQAmENB}The  {ENEMY_KINGDOM_INFORMAL_NAME} may be strong but their lands are rich and ripe for the taking.", null);
					}
					else if (warStats.Strength > warStats2.Strength)
					{
						reason = new TextObject("{=az3K3j4C}Right now we are stronger than the {ENEMY_KINGDOM_INFORMAL_NAME}. We should strike while we can.", null);
					}
				}
				else if ((float)num47 > num50 && (float)num47 > num48 && (float)num47 > num49)
				{
					reason = new TextObject("{=pmg9KCqf}We have been at peace too long. Our men grow restless.", null);
				}
				else if (num50 > num48 && num50 > num49)
				{
					reason = new TextObject("{=79lEPn1u}The {ENEMY_KINGDOM_INFORMAL_NAME} have occupied our ancestral lands and they oppress our kinfolk.", null);
				}
				else if (num49 > num48)
				{
					reason = new TextObject("{=bHf8aMtt}The {ENEMY_KINGDOM_INFORMAL_NAME} have been acting aggressively. We should teach them a lesson.", null);
				}
				else
				{
					reason = new TextObject("{=gsmmoKNd}The {ENEMY_KINGDOM_INFORMAL_NAME} will devour all of Calradia if we do not stop them.", null);
				}
				if (!TextObject.IsNullOrEmpty(reason))
				{
					reason.SetTextVariable("ENEMY_KINGDOM_INFORMAL_NAME", factionDeclaredWar.InformalName);
					reason.SetCharacterProperties("ENEMY_RULER", factionDeclaredWar.Leader.CharacterObject, false);
				}
			}
			return num7 * num33;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00065BB8 File Offset: 0x00063DB8
		private float CalculateBenefitScore(ref DefaultDiplomacyModel.WarStats faction1Stats, ref DefaultDiplomacyModel.WarStats faction2Stats, int valorLevelOfEvaluatingClan, bool evaluatingPeace, float distanceToClosestEnemyFief, bool calculatingRisk = false)
		{
			float valueOfSettlements = faction2Stats.ValueOfSettlements;
			float num = MathF.Clamp((valueOfSettlements > DefaultDiplomacyModel._MeaningfulValue) ? ((valueOfSettlements - DefaultDiplomacyModel._MeaningfulValue) * 0.5f + DefaultDiplomacyModel._MinValue + DefaultDiplomacyModel._MeaningfulValue) : (valueOfSettlements + DefaultDiplomacyModel._MinValue), DefaultDiplomacyModel._MinValue, DefaultDiplomacyModel._MaxValue);
			float num2 = 100f;
			float num3 = (faction2Stats.Strength + num2) / (faction1Stats.Strength + num2);
			float num4 = 0f;
			float num5 = (num3 > 1f) ? num3 : (1f / num3);
			if (num5 > 3f)
			{
				num4 = MathF.Min(0.4f, (num5 / 3f - 1f) * 0.1f);
			}
			float num6 = MathF.Pow(num3, 1.1f + num4);
			if (!calculatingRisk)
			{
				float x = MathF.Min(1f, (MathF.Min(MathF.Max(faction2Stats.Strength, 10000f), faction1Stats.Strength) + num2) / (faction2Stats.Strength + faction1Stats.TotalStrengthOfEnemies + num2));
				num6 /= MathF.Pow(x, (0.5f - (float)valorLevelOfEvaluatingClan * 0.1f) * (evaluatingPeace ? 1.1f : 1f));
			}
			else
			{
				float x2 = MathF.Min(1f, (MathF.Min(MathF.Max(faction1Stats.Strength, 10000f), faction2Stats.Strength) + num2) / (faction1Stats.Strength + faction2Stats.TotalStrengthOfEnemies + num2));
				num6 *= MathF.Pow(x2, (0.4f - (float)valorLevelOfEvaluatingClan * 0.1f) * (evaluatingPeace ? 1.1f : 1f));
			}
			float num7 = 1f / (1f + num6);
			num7 = MathF.Clamp(num7, 0.01f, 0.99f);
			float num8 = num * num7;
			float b = Campaign.AverageDistanceBetweenTwoFortifications * 3f / (Campaign.AverageDistanceBetweenTwoFortifications * 3f + distanceToClosestEnemyFief + distanceToClosestEnemyFief * 0.25f);
			return num8 * MathF.Max(0.25f, b);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00065D94 File Offset: 0x00063F94
		private ValueTuple<Settlement, float>[] GetClosestSettlementsToOtherFactionsNearestSettlementToMidPoint(IFaction faction1, IFaction faction2)
		{
			Settlement toSettlement = null;
			float num = float.MaxValue;
			foreach (Town town in faction1.Fiefs)
			{
				float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, faction1.FactionMidSettlement);
				if (num > distance)
				{
					toSettlement = town.Settlement;
					num = distance;
				}
			}
			ValueTuple<Settlement, float>[] array = new ValueTuple<Settlement, float>[]
			{
				new ValueTuple<Settlement, float>(null, float.MaxValue),
				new ValueTuple<Settlement, float>(null, float.MaxValue),
				new ValueTuple<Settlement, float>(null, float.MaxValue)
			};
			foreach (Town town2 in faction2.Fiefs)
			{
				float distance2 = Campaign.Current.Models.MapDistanceModel.GetDistance(town2.Settlement, toSettlement);
				if (distance2 < array[2].Item2)
				{
					if (distance2 < array[1].Item2)
					{
						if (distance2 < array[0].Item2)
						{
							array[2] = array[1];
							array[1] = array[0];
							array[0].Item1 = town2.Settlement;
							array[0].Item2 = distance2;
						}
						else
						{
							array[2] = array[1];
							array[1].Item1 = town2.Settlement;
							array[1].Item2 = distance2;
						}
					}
					else
					{
						array[2].Item1 = town2.Settlement;
						array[2].Item2 = distance2;
					}
				}
			}
			return array;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00065F9C File Offset: 0x0006419C
		private float GetDistance(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			if (factionDeclaresWar.Fiefs.Count != 0 && factionDeclaredWar.Fiefs.Count != 0)
			{
				ValueTuple<Settlement, float>[] closestSettlementsToOtherFactionsNearestSettlementToMidPoint = this.GetClosestSettlementsToOtherFactionsNearestSettlementToMidPoint(factionDeclaredWar, factionDeclaresWar);
				ValueTuple<Settlement, float>[] closestSettlementsToOtherFactionsNearestSettlementToMidPoint2 = this.GetClosestSettlementsToOtherFactionsNearestSettlementToMidPoint(factionDeclaresWar, factionDeclaredWar);
				float[] array = new float[]
				{
					float.MaxValue,
					float.MaxValue,
					float.MaxValue
				};
				foreach (ValueTuple<Settlement, float> valueTuple in closestSettlementsToOtherFactionsNearestSettlementToMidPoint)
				{
					if (valueTuple.Item1 != null)
					{
						foreach (ValueTuple<Settlement, float> valueTuple2 in closestSettlementsToOtherFactionsNearestSettlementToMidPoint2)
						{
							if (valueTuple2.Item1 != null)
							{
								float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(valueTuple.Item1, valueTuple2.Item1);
								if (distance < array[2])
								{
									if (distance < array[1])
									{
										if (distance < array[0])
										{
											array[2] = array[1];
											array[1] = array[0];
											array[0] = distance;
										}
										else
										{
											array[2] = array[1];
											array[1] = distance;
										}
									}
									else
									{
										array[2] = distance;
									}
								}
							}
						}
					}
				}
				float num = array[0];
				float num2 = ((array[1] < float.MaxValue) ? array[1] : num) * 0.67f;
				float num3 = ((array[2] < float.MaxValue) ? array[2] : ((num2 < float.MaxValue) ? num2 : num)) * 0.33f;
				return (num + num2 + num3) / 2f;
			}
			if (factionDeclaresWar.Leader == Hero.MainHero || factionDeclaredWar.Leader == Hero.MainHero)
			{
				return 100f;
			}
			return 0.4f * (factionDeclaresWar.InitialPosition - factionDeclaredWar.InitialPosition).Length;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00066134 File Offset: 0x00064334
		public override float GetScoreOfDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingClan, out TextObject warReason)
		{
			float scoreOfWarInternal = this.GetScoreOfWarInternal(factionDeclaresWar, factionDeclaredWar, evaluatingClan, false, out warReason);
			StanceLink stanceWith = factionDeclaresWar.GetStanceWith(factionDeclaredWar);
			int num = 0;
			if (stanceWith.IsNeutral)
			{
				int dailyTributePaid = stanceWith.GetDailyTributePaid(factionDeclaredWar);
				float num2 = (float)evaluatingClan.Leader.Gold + (evaluatingClan.MapFaction.IsKingdomFaction ? (0.5f * ((float)((Kingdom)evaluatingClan.MapFaction).KingdomBudgetWallet / ((float)((Kingdom)evaluatingClan.MapFaction).Clans.Count + 1f))) : 0f);
				float num3 = (!evaluatingClan.IsKingdomFaction && evaluatingClan.Leader != null) ? ((num2 < 50000f) ? (1f + 0.5f * ((50000f - num2) / 50000f)) : ((num2 > 200000f) ? MathF.Max(0.5f, MathF.Sqrt(200000f / num2)) : 1f)) : 1f;
				num = this.GetValueOfDailyTribute(dailyTributePaid);
				num = (int)((float)num * num3);
			}
			int num4 = -(int)MathF.Min(120000f, (MathF.Min(10000f, factionDeclaresWar.TotalStrength) * 0.8f + 2000f) * (MathF.Min(10000f, factionDeclaredWar.TotalStrength) * 0.8f + 2000f) * 0.0012f);
			return scoreOfWarInternal + (float)num4 - (float)num;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00066288 File Offset: 0x00064488
		private static int GetWarFatiqueScore(IFaction factionDeclaresWar, IFaction factionDeclaredWar)
		{
			int num = 0;
			StanceLink stanceWith = factionDeclaresWar.GetStanceWith(factionDeclaredWar);
			float num2 = (float)(CampaignTime.Now - stanceWith.WarStartDate).ToDays;
			float num3 = (factionDeclaresWar.IsMinorFaction && factionDeclaresWar != MobileParty.MainParty.MapFaction) ? 40f : 60f;
			if (num2 < num3)
			{
				int num4 = ((factionDeclaredWar == MobileParty.MainParty.MapFaction && !factionDeclaresWar.IsMinorFaction) || (factionDeclaresWar == MobileParty.MainParty.MapFaction && !factionDeclaredWar.IsMinorFaction)) ? 2 : 1;
				float num5 = (factionDeclaresWar.IsMinorFaction && factionDeclaresWar != MobileParty.MainParty.MapFaction) ? 1000f : 2000f;
				num = (int)(MathF.Pow(num3 - num2, 1.3f) * num5 * (float)num4);
			}
			return num + 60000;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00066354 File Offset: 0x00064554
		public override float GetScoreOfLettingPartyGo(MobileParty party, MobileParty partyToLetGo)
		{
			float num = 0f;
			BattleSideEnum battleSideEnum = BattleSideEnum.Attacker;
			if (battleSideEnum == BattleSideEnum.Attacker)
			{
				num = 0.98f;
			}
			float num2 = 0f;
			for (int i = 0; i < partyToLetGo.ItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = partyToLetGo.ItemRoster.GetElementCopyAtIndex(i);
				num2 += (float)(elementCopyAtIndex.Amount * elementCopyAtIndex.EquipmentElement.GetBaseValue());
			}
			float num3 = 0f;
			for (int j = 0; j < party.ItemRoster.Count; j++)
			{
				ItemRosterElement elementCopyAtIndex2 = party.ItemRoster.GetElementCopyAtIndex(j);
				num3 += (float)(elementCopyAtIndex2.Amount * elementCopyAtIndex2.EquipmentElement.GetBaseValue());
			}
			float num4 = 0f;
			foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
			{
				num4 += MathF.Min(1000f, 10f * (float)troopRosterElement.Character.Level * MathF.Sqrt((float)troopRosterElement.Character.Level));
			}
			float num5 = 0f;
			foreach (TroopRosterElement troopRosterElement2 in partyToLetGo.MemberRoster.GetTroopRoster())
			{
				num5 += MathF.Min(1000f, 10f * (float)troopRosterElement2.Character.Level * MathF.Sqrt((float)troopRosterElement2.Character.Level));
			}
			float num6 = 0f;
			foreach (TroopRosterElement troopRosterElement3 in ((battleSideEnum == BattleSideEnum.Attacker) ? partyToLetGo : party).MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement3.Character.IsHero)
				{
					num6 += 500f;
				}
				num6 += (float)Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(troopRosterElement3.Character, (battleSideEnum == BattleSideEnum.Attacker) ? partyToLetGo.LeaderHero : party.LeaderHero) * 0.3f;
			}
			float num7 = party.IsPartyTradeActive ? ((float)party.PartyTradeGold) : 0f;
			num7 += ((party.LeaderHero != null) ? ((float)party.LeaderHero.Gold * 0.15f) : 0f);
			float num8 = partyToLetGo.IsPartyTradeActive ? ((float)partyToLetGo.PartyTradeGold) : 0f;
			num7 += ((partyToLetGo.LeaderHero != null) ? ((float)partyToLetGo.LeaderHero.Gold * 0.15f) : 0f);
			float num9 = num5 + 10000f;
			if (partyToLetGo.BesiegedSettlement != null)
			{
				num9 += 20000f;
			}
			return -1000f + (1f - num) * num4 - num * num9 - num * num8 + (1f - num) * num7 + num * num6 + (num3 * (1f - num) - num * num2);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0006667C File Offset: 0x0006487C
		public override float GetValueOfHeroForFaction(Hero examinedHero, IFaction targetFaction, bool forMarriage = false)
		{
			return this.GetHeroCommandingStrengthForClan(examinedHero) * 10f;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0006668B File Offset: 0x0006488B
		public override int GetRelationCostOfExpellingClanFromKingdom()
		{
			return -20;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0006668F File Offset: 0x0006488F
		public override int GetInfluenceCostOfSupportingClan()
		{
			return 50;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00066694 File Offset: 0x00064894
		public override int GetInfluenceCostOfExpellingClan(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(200f, false, null);
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x000666C4 File Offset: 0x000648C4
		public override int GetInfluenceCostOfProposingPeace(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(100f, false, null);
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x000666F4 File Offset: 0x000648F4
		public override int GetInfluenceCostOfProposingWar(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(200f, false, null);
			if (proposingClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.WarTax) && proposingClan == proposingClan.Kingdom.RulingClan)
			{
				explainedNumber.AddFactor(1f, null);
			}
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00066756 File Offset: 0x00064956
		public override int GetInfluenceValueOfSupportingClan()
		{
			return this.GetInfluenceCostOfSupportingClan() / 4;
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00066760 File Offset: 0x00064960
		public override int GetRelationValueOfSupportingClan()
		{
			return 1;
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00066764 File Offset: 0x00064964
		public override int GetInfluenceCostOfAnnexation(Clan proposingClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(200f, false, null);
			if (proposingClan.Kingdom != null)
			{
				if (proposingClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.FeudalInheritance))
				{
					explainedNumber.AddFactor(1f, null);
				}
				if (proposingClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.PrecarialLandTenure) && proposingClan == proposingClan.Kingdom.RulingClan)
				{
					explainedNumber.AddFactor(-0.5f, null);
				}
			}
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposingClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x000667F2 File Offset: 0x000649F2
		public override int GetInfluenceCostOfChangingLeaderOfArmy()
		{
			return 30;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x000667F8 File Offset: 0x000649F8
		public override int GetInfluenceCostOfDisbandingArmy()
		{
			int num = 30;
			if (Clan.PlayerClan.Kingdom != null && Clan.PlayerClan == Clan.PlayerClan.Kingdom.RulingClan)
			{
				num /= 2;
			}
			return num;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0006682F File Offset: 0x00064A2F
		public override int GetRelationCostOfDisbandingArmy(bool isLeaderParty)
		{
			if (!isLeaderParty)
			{
				return -1;
			}
			return -4;
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x00066838 File Offset: 0x00064A38
		public override int GetInfluenceCostOfPolicyProposalAndDisavowal(Clan proposerClan)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(100f, false, null);
			this.GetPerkEffectsOnKingdomDecisionInfluenceCost(proposerClan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x00066868 File Offset: 0x00064A68
		public override int GetInfluenceCostOfAbandoningArmy()
		{
			return 2;
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0006686B File Offset: 0x00064A6B
		private void GetPerkEffectsOnKingdomDecisionInfluenceCost(Clan proposingClan, ref ExplainedNumber cost)
		{
			if (proposingClan.Leader.GetPerkValue(DefaultPerks.Charm.Firebrand))
			{
				cost.AddFactor(DefaultPerks.Charm.Firebrand.PrimaryBonus, DefaultPerks.Charm.Firebrand.Name);
			}
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00066899 File Offset: 0x00064A99
		private int GetBaseRelationBetweenHeroes(Hero hero1, Hero hero2)
		{
			return CharacterRelationManager.GetHeroRelation(hero1, hero2);
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000668A2 File Offset: 0x00064AA2
		public override int GetBaseRelation(Hero hero1, Hero hero2)
		{
			return this.GetBaseRelationBetweenHeroes(hero1, hero2);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x000668AC File Offset: 0x00064AAC
		public override int GetEffectiveRelation(Hero hero1, Hero hero2)
		{
			Hero hero3;
			Hero hero4;
			this.GetHeroesForEffectiveRelation(hero1, hero2, out hero3, out hero4);
			if (hero3 == null || hero4 == null)
			{
				return 0;
			}
			int baseRelationBetweenHeroes = this.GetBaseRelationBetweenHeroes(hero3, hero4);
			this.GetPersonalityEffects(ref baseRelationBetweenHeroes, hero1, hero4);
			return MBMath.ClampInt(baseRelationBetweenHeroes, this.MinRelationLimit, this.MaxRelationLimit);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x000668F4 File Offset: 0x00064AF4
		public override void GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2)
		{
			effectiveHero1 = ((hero1.Clan != null) ? hero1.Clan.Leader : hero1);
			effectiveHero2 = ((hero2.Clan != null) ? hero2.Clan.Leader : hero2);
			if (effectiveHero1 == effectiveHero2 || (hero1.IsPlayerCompanion && hero2.IsHumanPlayerCharacter) || (hero1.IsPlayerCompanion && hero2.IsHumanPlayerCharacter))
			{
				effectiveHero1 = hero1;
				effectiveHero2 = hero2;
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00066960 File Offset: 0x00064B60
		public override int GetRelationChangeAfterClanLeaderIsDead(Hero deadLeader, Hero relationHero)
		{
			return (int)((float)CharacterRelationManager.GetHeroRelation(deadLeader, relationHero) * 0.7f);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00066974 File Offset: 0x00064B74
		public override int GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(Hero supporter, bool hasHeroVotedAgainstOwner)
		{
			int num;
			if (hasHeroVotedAgainstOwner)
			{
				num = -20;
				if (supporter.Culture.HasFeat(DefaultCulturalFeats.SturgianDecisionPenaltyFeat))
				{
					num += (int)((float)num * DefaultCulturalFeats.SturgianDecisionPenaltyFeat.EffectBonus);
				}
			}
			else
			{
				num = 5;
			}
			return num;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x000669AF File Offset: 0x00064BAF
		private void GetPersonalityEffects(ref int effectiveRelation, Hero hero1, Hero effectiveHero2)
		{
			this.GetTraitEffect(ref effectiveRelation, hero1, effectiveHero2, DefaultTraits.Honor, 2);
			this.GetTraitEffect(ref effectiveRelation, hero1, effectiveHero2, DefaultTraits.Valor, 1);
			this.GetTraitEffect(ref effectiveRelation, hero1, effectiveHero2, DefaultTraits.Mercy, 1);
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x000669E0 File Offset: 0x00064BE0
		private void GetTraitEffect(ref int effectiveRelation, Hero hero1, Hero effectiveHero2, TraitObject trait, int effectMagnitude)
		{
			int traitLevel = hero1.GetTraitLevel(trait);
			int traitLevel2 = effectiveHero2.GetTraitLevel(trait);
			int num = traitLevel * traitLevel2;
			if (num > 0)
			{
				effectiveRelation += effectMagnitude;
				return;
			}
			if (num < 0)
			{
				effectiveRelation -= effectMagnitude;
			}
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00066A18 File Offset: 0x00064C18
		public override int GetCharmExperienceFromRelationGain(Hero hero, float relationChange, ChangeRelationAction.ChangeRelationDetail detail)
		{
			float num = 20f;
			if (detail != ChangeRelationAction.ChangeRelationDetail.Emissary)
			{
				if (!hero.IsNotable)
				{
					if (hero.MapFaction != null && hero.MapFaction.Leader == hero)
					{
						num *= 30f;
					}
					else if (hero.Clan != null && hero.Clan.Leader == hero)
					{
						num *= 20f;
					}
				}
			}
			else if (!hero.IsNotable)
			{
				if (hero.MapFaction != null && hero.MapFaction.Leader == hero)
				{
					num *= 30f;
				}
				else if (hero.Clan != null && hero.Clan.Leader == hero)
				{
					num *= 20f;
				}
				else
				{
					num *= 10f;
				}
			}
			else
			{
				num *= 20f;
			}
			return MathF.Round(num * relationChange);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00066ADC File Offset: 0x00064CDC
		public override uint GetNotificationColor(ChatNotificationType notificationType)
		{
			switch (notificationType)
			{
			case ChatNotificationType.Default:
				return 10066329U;
			case ChatNotificationType.PlayerFactionPositive:
				return 2284902U;
			case ChatNotificationType.PlayerClanPositive:
				return 3407803U;
			case ChatNotificationType.PlayerFactionNegative:
				return 14509602U;
			case ChatNotificationType.PlayerClanNegative:
				return 16750899U;
			case ChatNotificationType.Civilian:
				return 10053324U;
			case ChatNotificationType.PlayerClanCivilian:
				return 15623935U;
			case ChatNotificationType.PlayerFactionCivilian:
				return 11163101U;
			case ChatNotificationType.Neutral:
				return 12303291U;
			case ChatNotificationType.PlayerFactionIndirectPositive:
				return 12298820U;
			case ChatNotificationType.PlayerFactionIndirectNegative:
				return 13382502U;
			case ChatNotificationType.PlayerClanPolitical:
				return 6745855U;
			case ChatNotificationType.PlayerFactionPolitical:
				return 5614301U;
			case ChatNotificationType.Political:
				return 6724044U;
			default:
				return 13369548U;
			}
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00066B82 File Offset: 0x00064D82
		public override float DenarsToInfluence()
		{
			return 0.002f;
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00066B89 File Offset: 0x00064D89
		public override bool CanSettlementBeGifted(Settlement settlementToGift)
		{
			return settlementToGift.Town != null && !settlementToGift.Town.IsOwnerUnassigned;
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00066BA3 File Offset: 0x00064DA3
		public override IEnumerable<BarterGroup> GetBarterGroups()
		{
			return new BarterGroup[]
			{
				new GoldBarterGroup(),
				new ItemBarterGroup(),
				new PrisonerBarterGroup(),
				new FiefBarterGroup(),
				new OtherBarterGroup(),
				new DefaultsBarterGroup()
			};
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00066BDB File Offset: 0x00064DDB
		public override int GetValueOfDailyTribute(int dailyTributeAmount)
		{
			return dailyTributeAmount * 70;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00066BE1 File Offset: 0x00064DE1
		public override int GetDailyTributeForValue(int value)
		{
			return value / 70 / 10 * 10;
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00066BED File Offset: 0x00064DED
		public override bool IsClanEligibleToBecomeRuler(Clan clan)
		{
			return !clan.IsEliminated && clan.Leader.IsAlive && !clan.IsUnderMercenaryService;
		}

		// Token: 0x04000771 RID: 1905
		private const int DailyValueFactorForTributes = 70;

		// Token: 0x04000772 RID: 1906
		private static float HearthRiskValueFactor = 500f;

		// Token: 0x04000773 RID: 1907
		private static float LordRiskValueFactor = 1000f;

		// Token: 0x04000774 RID: 1908
		private static float FoodRiskValueFactor = 750f;

		// Token: 0x04000775 RID: 1909
		private static float GarrisonRiskValueFactor = 2000f;

		// Token: 0x04000776 RID: 1910
		private static float SiegeRiskValueFactor = 3000f;

		// Token: 0x04000777 RID: 1911
		private static float LoyalityRiskValueFactor = 500f;

		// Token: 0x04000778 RID: 1912
		private static float ProsperityValueFactor = 50f;

		// Token: 0x04000779 RID: 1913
		private static float HappenedSiegesDifFactor = 1500f;

		// Token: 0x0400077A RID: 1914
		private static float HappenedRaidsDifFactor = 500f;

		// Token: 0x0400077B RID: 1915
		private static float StrengthValueFactor = 100f;

		// Token: 0x0400077C RID: 1916
		private static TextObject _personalityEffectText = new TextObject("{=HDBryERe}Personalities", null);

		// Token: 0x0400077D RID: 1917
		private const float strengthFactor = 50f;

		// Token: 0x0400077E RID: 1918
		private static float _MaxValue = 10000000f;

		// Token: 0x0400077F RID: 1919
		private static float _MeaningfulValue = 2000000f;

		// Token: 0x04000780 RID: 1920
		private static float _MinValue = 10000f;

		// Token: 0x02000501 RID: 1281
		private struct WarStats
		{
			// Token: 0x0400158C RID: 5516
			public Clan RulingClan;

			// Token: 0x0400158D RID: 5517
			public float Strength;

			// Token: 0x0400158E RID: 5518
			public float ValueOfSettlements;

			// Token: 0x0400158F RID: 5519
			public float TotalStrengthOfEnemies;
		}
	}
}
