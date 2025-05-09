﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000395 RID: 917
	public class HeroSpawnCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003738 RID: 14136 RVA: 0x000F911C File Offset: 0x000F731C
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnNonBanditClanDailyTick));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroDailyTick));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x000F91E4 File Offset: 0x000F73E4
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i == 0)
			{
				int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
				foreach (Clan clan in Clan.All)
				{
					foreach (Hero hero in clan.Heroes)
					{
						if (hero.Age >= (float)heroComesOfAge && hero.IsAlive && !hero.IsDisabled)
						{
							hero.ChangeState(Hero.CharacterStates.Active);
						}
					}
				}
			}
			int num = Clan.NonBanditFactions.Count<Clan>();
			int num2 = num / 100 + ((num % 100 > i) ? 1 : 0);
			int num3 = num / 100;
			for (int j = 0; j < i; j++)
			{
				num3 += ((num % 100 > j) ? 1 : 0);
			}
			for (int k = 0; k < num2; k++)
			{
				this.OnNonBanditClanDailyTick(Clan.NonBanditFactions.ElementAt(num3 + k));
			}
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x000F9310 File Offset: 0x000F7510
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				if (!clan.IsEliminated && clan.IsMinorFaction && clan != Clan.PlayerClan)
				{
					HeroSpawnCampaignBehavior.SpawnMinorFactionHeroes(clan, true);
					HeroSpawnCampaignBehavior.CheckAndAssignClanLeader(clan);
					clan.UpdateHomeSettlement(null);
				}
			}
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000F9384 File Offset: 0x000F7584
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsActive)
				{
					this.OnHeroDailyTick(hero);
				}
			}
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000F93E0 File Offset: 0x000F75E0
		private void OnHeroComesOfAge(Hero hero)
		{
			if (!hero.IsDisabled && hero.HeroState != Hero.CharacterStates.Active && !hero.IsTraveling)
			{
				hero.ChangeState(Hero.CharacterStates.Active);
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, hero.HomeSettlement);
			}
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x000F940E File Offset: 0x000F760E
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x000F9410 File Offset: 0x000F7610
		private void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (!companion.IsFugitive && !companion.IsDead && detail != RemoveCompanionAction.RemoveCompanionDetail.ByTurningToLord && detail != RemoveCompanionAction.RemoveCompanionDetail.Death && companion.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
			{
				Settlement settlement = this.FindASuitableSettlementToTeleportForHero(companion, 0f);
				if (settlement == null)
				{
					settlement = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsTown);
				}
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(companion, settlement);
			}
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000F947C File Offset: 0x000F767C
		private void OnHeroDailyTick(Hero hero)
		{
			Settlement settlement = null;
			if (hero.IsFugitive || hero.IsReleased)
			{
				if (!hero.IsSpecial && (hero.IsPlayerCompanion || MBRandom.RandomFloat < 0.3f || (hero.CurrentSettlement != null && hero.CurrentSettlement.MapFaction.IsAtWarWith(hero.MapFaction))))
				{
					settlement = this.FindASuitableSettlementToTeleportForHero(hero, 0f);
				}
			}
			else if (hero.IsActive)
			{
				if (hero.CurrentSettlement == null && hero.PartyBelongedTo == null && !hero.IsSpecial && hero.GovernorOf == null)
				{
					if (MobileParty.MainParty.MemberRoster.Contains(hero.CharacterObject))
					{
						MobileParty.MainParty.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
						MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
					}
					else
					{
						settlement = this.FindASuitableSettlementToTeleportForHero(hero, 0f);
					}
				}
				else if (this.CanHeroMoveToAnotherSettlement(hero))
				{
					settlement = this.FindASuitableSettlementToTeleportForHero(hero, 10f);
				}
			}
			if (settlement != null)
			{
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(hero, settlement);
				if (!hero.IsActive)
				{
					hero.ChangeState(Hero.CharacterStates.Active);
				}
			}
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x000F95B0 File Offset: 0x000F77B0
		private void OnNonBanditClanDailyTick(Clan clan)
		{
			if (!clan.IsEliminated && clan != Clan.PlayerClan)
			{
				if (clan.IsMinorFaction)
				{
					HeroSpawnCampaignBehavior.SpawnMinorFactionHeroes(clan, false);
				}
				this.ConsiderSpawningLordParties(clan);
			}
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000F95D8 File Offset: 0x000F77D8
		private bool CanHeroMoveToAnotherSettlement(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan && !hero.IsTemplate && hero.IsAlive && !hero.IsNotable && !hero.IsHumanPlayerCharacter && !hero.IsPartyLeader && !hero.IsPrisoner && hero.HeroState != Hero.CharacterStates.Disabled && hero.GovernorOf == null && hero.PartyBelongedTo == null && !hero.IsWanderer && hero.PartyBelongedToAsPrisoner == null && hero.CharacterObject.Occupation != Occupation.Special && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				Settlement currentSettlement = hero.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town : null) == null || (!hero.CurrentSettlement.Town.HasTournament && !hero.CurrentSettlement.IsUnderSiege))
				{
					return hero.CanMoveToSettlement();
				}
			}
			return false;
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x000F96CC File Offset: 0x000F78CC
		private Settlement FindASuitableSettlementToTeleportForHero(Hero hero, float minimumScore = 0f)
		{
			Settlement settlement;
			if (hero.IsNotable)
			{
				settlement = hero.BornSettlement;
			}
			else
			{
				List<Settlement> list = (from x in hero.MapFaction.Settlements
				where x.IsTown
				select x).ToList<Settlement>();
				if (list.Count > 0)
				{
					List<ValueTuple<Settlement, float>> list2 = new List<ValueTuple<Settlement, float>>();
					foreach (Settlement settlement2 in list)
					{
						float moveScoreForHero = this.GetMoveScoreForHero(hero, settlement2.Town);
						list2.Add(new ValueTuple<Settlement, float>(settlement2, (moveScoreForHero >= minimumScore) ? moveScoreForHero : 0f));
					}
					settlement = MBRandom.ChooseWeighted<Settlement>(list2);
				}
				else
				{
					List<Settlement> list3 = new List<Settlement>();
					List<Settlement> list4 = new List<Settlement>();
					foreach (Town town in Town.AllTowns)
					{
						if (town.MapFaction.IsAtWarWith(hero.MapFaction))
						{
							list4.Add(town.Settlement);
						}
						else if (town.MapFaction != hero.MapFaction)
						{
							list3.Add(town.Settlement);
						}
					}
					List<ValueTuple<Settlement, float>> list5 = new List<ValueTuple<Settlement, float>>();
					foreach (Settlement settlement3 in list3)
					{
						float moveScoreForHero2 = this.GetMoveScoreForHero(hero, settlement3.Town);
						list5.Add(new ValueTuple<Settlement, float>(settlement3, (moveScoreForHero2 >= minimumScore) ? moveScoreForHero2 : 0f));
					}
					settlement = MBRandom.ChooseWeighted<Settlement>(list5);
					if (settlement == null)
					{
						list5 = new List<ValueTuple<Settlement, float>>();
						foreach (Settlement settlement4 in list4)
						{
							float moveScoreForHero3 = this.GetMoveScoreForHero(hero, settlement4.Town);
							list5.Add(new ValueTuple<Settlement, float>(settlement4, (moveScoreForHero3 >= minimumScore) ? moveScoreForHero3 : 0f));
						}
						settlement = MBRandom.ChooseWeighted<Settlement>(list5);
					}
				}
			}
			return settlement;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000F991C File Offset: 0x000F7B1C
		private float GetHeroPartyCommandScore(Hero hero)
		{
			return 3f * (float)hero.GetSkillValue(DefaultSkills.Tactics) + 2f * (float)hero.GetSkillValue(DefaultSkills.Leadership) + (float)hero.GetSkillValue(DefaultSkills.Scouting) + (float)hero.GetSkillValue(DefaultSkills.Steward) + (float)hero.GetSkillValue(DefaultSkills.OneHanded) + (float)hero.GetSkillValue(DefaultSkills.TwoHanded) + (float)hero.GetSkillValue(DefaultSkills.Polearm) + (float)hero.GetSkillValue(DefaultSkills.Riding) + ((hero.Clan.Leader == hero) ? 1000f : 0f) + ((hero.GovernorOf == null) ? 500f : 0f);
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x000F99CC File Offset: 0x000F7BCC
		private float GetMoveScoreForHero(Hero hero, Town fief)
		{
			Clan clan = hero.Clan;
			float num = 1E-06f;
			if (!fief.IsUnderSiege && !fief.MapFaction.IsAtWarWith(hero.MapFaction))
			{
				num = (FactionManager.IsAlliedWithFaction(fief.MapFaction, hero.MapFaction) ? 0.01f : 1E-05f);
				if (fief.MapFaction == hero.MapFaction)
				{
					num += 10f;
					if (fief.IsTown)
					{
						num += 100f;
					}
					if (fief.OwnerClan == clan)
					{
						num += (fief.IsTown ? 500f : 100f);
					}
					if (fief.HasTournament)
					{
						num += 400f;
					}
				}
				foreach (Hero hero2 in fief.Settlement.HeroesWithoutParty)
				{
					if (clan != null && hero2.Clan == clan)
					{
						num += (fief.IsTown ? 100f : 10f);
					}
				}
				if (fief.Settlement.IsStarving)
				{
					num *= 0.1f;
				}
				if (hero.CurrentSettlement == fief.Settlement)
				{
					num *= 3f;
				}
			}
			return num;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x000F9B10 File Offset: 0x000F7D10
		private void ConsiderSpawningLordParties(Clan clan)
		{
			int partyLimitForTier = Campaign.Current.Models.ClanTierModel.GetPartyLimitForTier(clan, clan.Tier);
			int count = clan.WarPartyComponents.Count;
			if (count >= partyLimitForTier)
			{
				return;
			}
			int num = partyLimitForTier - count;
			for (int i = 0; i < num; i++)
			{
				Hero bestAvailableCommander = this.GetBestAvailableCommander(clan);
				if (bestAvailableCommander == null)
				{
					break;
				}
				float num2 = this.CalculateScoreToCreateParty(clan);
				if (this.GetHeroPartyCommandScore(bestAvailableCommander) + num2 > 100f)
				{
					MobileParty mobileParty = this.SpawnLordParty(bestAvailableCommander);
					if (mobileParty != null)
					{
						this.GiveInitialItemsToParty(mobileParty);
					}
				}
			}
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x000F9B98 File Offset: 0x000F7D98
		private float CalculateScoreToCreateParty(Clan clan)
		{
			return (float)(clan.Fiefs.Count * 100 - clan.WarPartyComponents.Count * 100) + (float)clan.Gold * 0.01f + (clan.IsMinorFaction ? 200f : 0f) + ((clan.WarPartyComponents.Count > 0) ? 0f : 200f);
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x000F9C04 File Offset: 0x000F7E04
		private Hero GetBestAvailableCommander(Clan clan)
		{
			Hero hero = null;
			float num = 0f;
			foreach (Hero hero2 in clan.Heroes)
			{
				if (hero2.IsActive && hero2.IsAlive && hero2.PartyBelongedTo == null && hero2.PartyBelongedToAsPrisoner == null && hero2.CanLeadParty() && hero2.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && hero2.CharacterObject.Occupation == Occupation.Lord)
				{
					float heroPartyCommandScore = this.GetHeroPartyCommandScore(hero2);
					if (heroPartyCommandScore > num)
					{
						num = heroPartyCommandScore;
						hero = hero2;
					}
				}
			}
			if (hero != null)
			{
				return hero;
			}
			if (clan != Clan.PlayerClan)
			{
				foreach (Hero hero3 in clan.Heroes)
				{
					if (hero3.IsActive && hero3.IsAlive && hero3.PartyBelongedTo == null && hero3.PartyBelongedToAsPrisoner == null && hero3.Age > (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && hero3.CharacterObject.Occupation == Occupation.Lord)
					{
						float heroPartyCommandScore2 = this.GetHeroPartyCommandScore(hero3);
						if (heroPartyCommandScore2 > num)
						{
							num = heroPartyCommandScore2;
							hero = hero3;
						}
					}
				}
			}
			return hero;
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x000F9D74 File Offset: 0x000F7F74
		private MobileParty SpawnLordParty(Hero hero)
		{
			if (hero.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
			Settlement settlement = SettlementHelper.GetBestSettlementToSpawnAround(hero);
			MobileParty result;
			if (settlement != null && settlement.MapFaction == hero.MapFaction)
			{
				result = MobilePartyHelper.SpawnLordParty(hero, settlement);
			}
			else if (hero.MapFaction.InitialPosition.IsValid)
			{
				result = MobilePartyHelper.SpawnLordParty(hero, hero.MapFaction.InitialPosition, 30f);
			}
			else
			{
				foreach (Settlement settlement2 in Settlement.All)
				{
					if (settlement2.Culture == hero.Culture)
					{
						settlement = settlement2;
						break;
					}
				}
				if (settlement != null)
				{
					result = MobilePartyHelper.SpawnLordParty(hero, settlement);
				}
				else
				{
					result = MobilePartyHelper.SpawnLordParty(hero, Settlement.All.GetRandomElement<Settlement>());
				}
			}
			return result;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x000F9E54 File Offset: 0x000F8054
		private void GiveInitialItemsToParty(MobileParty heroParty)
		{
			float num = (254f + Campaign.AverageDistanceBetweenTwoFortifications * 4.54f) / 2f;
			foreach (Settlement settlement in Campaign.Current.Settlements)
			{
				if (settlement.IsVillage)
				{
					float num2 = heroParty.Position2D.Distance(settlement.Position2D);
					if (num2 < num)
					{
						foreach (ValueTuple<ItemObject, float> valueTuple in settlement.Village.VillageType.Productions)
						{
							ItemObject item = valueTuple.Item1;
							float item2 = valueTuple.Item2;
							float num3 = (item.ItemType == ItemObject.ItemTypeEnum.Horse && item.HorseComponent.IsRideable && !item.HorseComponent.IsPackAnimal) ? 7f : (item.IsFood ? 0.1f : 0f);
							float num4 = ((float)heroParty.MemberRoster.TotalManCount + 2f) / 200f;
							float num5 = 1f - num2 / num;
							int num6 = MBRandom.RoundRandomized(num3 * item2 * num5 * num4);
							if (num6 > 0)
							{
								heroParty.ItemRoster.AddToCounts(item, num6);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x000F9FE8 File Offset: 0x000F81E8
		private static void CheckAndAssignClanLeader(Clan clan)
		{
			if (clan.Leader == null || clan.Leader.IsDead)
			{
				Hero hero = clan.Lords.FirstOrDefaultQ((Hero x) => x.IsAlive);
				if (hero != null)
				{
					clan.SetLeader(hero);
					return;
				}
				Debug.FailedAssert("Cant find a lord to assign as leader to minor faction.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroSpawnCampaignBehavior.cs", "CheckAndAssignClanLeader", 598);
			}
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x000FA059 File Offset: 0x000F8259
		private static Hero CreateMinorFactionHeroFromTemplate(CharacterObject template, Clan faction)
		{
			Hero hero = HeroCreator.CreateSpecialHero(template, null, faction, null, Campaign.Current.GameStarted ? 19 : -1);
			hero.ChangeState(Campaign.Current.GameStarted ? Hero.CharacterStates.Active : Hero.CharacterStates.NotSpawned);
			hero.IsMinorFactionHero = true;
			return hero;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x000FA094 File Offset: 0x000F8294
		private static void SpawnMinorFactionHeroes(Clan clan, bool firstTime)
		{
			int num = Campaign.Current.Models.MinorFactionsModel.MinorFactionHeroLimit - clan.Lords.Count((Hero x) => x.IsAlive);
			if (num > 0)
			{
				if (firstTime)
				{
					int num2 = 0;
					while (num2 < clan.MinorFactionCharacterTemplates.Count && num > 0)
					{
						HeroSpawnCampaignBehavior.CreateMinorFactionHeroFromTemplate(clan.MinorFactionCharacterTemplates[num2], clan);
						num--;
						num2++;
					}
				}
				if (num > 0)
				{
					if (clan.MinorFactionCharacterTemplates == null || clan.MinorFactionCharacterTemplates.IsEmpty<CharacterObject>())
					{
						Debug.FailedAssert(string.Format("{0} templates are empty!", clan.Name), "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroSpawnCampaignBehavior.cs", "SpawnMinorFactionHeroes", 631);
						return;
					}
					for (int i = 0; i < num; i++)
					{
						if (MBRandom.RandomFloat < Campaign.Current.Models.MinorFactionsModel.DailyMinorFactionHeroSpawnChance)
						{
							HeroSpawnCampaignBehavior.CreateMinorFactionHeroFromTemplate(clan.MinorFactionCharacterTemplates.GetRandomElementInefficiently<CharacterObject>(), clan);
						}
					}
				}
			}
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x000FA194 File Offset: 0x000F8394
		public void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			if (oldGovernor != null && oldGovernor.Clan != null)
			{
				foreach (Hero hero in oldGovernor.Clan.Heroes)
				{
					hero.UpdateHomeSettlement();
				}
			}
			if (newGovernor != null && newGovernor.Clan != null && (oldGovernor == null || newGovernor.Clan != oldGovernor.Clan))
			{
				foreach (Hero hero2 in newGovernor.Clan.Heroes)
				{
					hero2.UpdateHomeSettlement();
				}
			}
		}

		// Token: 0x0400116E RID: 4462
		public const float DefaultHealingPercentage = 0.015f;

		// Token: 0x0400116F RID: 4463
		private const float MinimumScoreForSafeSettlement = 10f;

		// Token: 0x04001170 RID: 4464
		private const float CompanionMoveGoodEnoughScore = 1000f;
	}
}
