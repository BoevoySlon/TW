using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003B2 RID: 946
	public class NotablesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060039FE RID: 14846 RVA: 0x0011092E File Offset: 0x0010EB2E
		public NotablesCampaignBehavior()
		{
			this._settlementPassedDaysForWeeklyTick = new Dictionary<Settlement, int>();
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x00110944 File Offset: 0x0010EB44
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x001109F4 File Offset: 0x0010EBF4
		private void WeeklyTick()
		{
			foreach (Hero hero in Hero.DeadOrDisabledHeroes.ToList<Hero>())
			{
				if (hero.IsDead && hero.IsNotable && hero.DeathDay.ElapsedDaysUntilNow >= 7f)
				{
					Campaign.Current.CampaignObjectManager.UnregisterDeadHero(hero);
				}
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00110A7C File Offset: 0x0010EC7C
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.WeeklyTick();
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x00110A84 File Offset: 0x0010EC84
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Settlement, int>>("_settlementPassedDaysForWeeklyTick", ref this._settlementPassedDaysForWeeklyTick);
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x00110A98 File Offset: 0x0010EC98
		public void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.SpawnNotablesAtGameStart();
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x00110AA0 File Offset: 0x0010ECA0
		private void DetermineRelation(Hero hero1, Hero hero2, float randomValue, float chanceOfConflict)
		{
			float num = 0.3f;
			if (randomValue < num)
			{
				int num2 = (int)((num - randomValue) * (num - randomValue) / (num * num) * 100f);
				if (num2 > 0)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero1, hero2, num2, true);
					return;
				}
			}
			else if (randomValue > 1f - chanceOfConflict)
			{
				int num3 = -(int)((randomValue - (1f - chanceOfConflict)) * (randomValue - (1f - chanceOfConflict)) / (chanceOfConflict * chanceOfConflict) * 100f);
				if (num3 < 0)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(hero1, hero2, num3, true);
				}
			}
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x00110B14 File Offset: 0x0010ED14
		public void SetInitialRelationsBetweenNotablesAndLords()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				for (int i = 0; i < settlement.Notables.Count; i++)
				{
					Hero hero = settlement.Notables[i];
					foreach (Hero hero2 in settlement.MapFaction.Lords)
					{
						if (hero2 != hero && hero2 == hero2.Clan.Leader && hero2.MapFaction == settlement.MapFaction)
						{
							float chanceOfConflict = (float)HeroHelper.NPCPersonalityClashWithNPC(hero, hero2) * 0.01f * 2.5f;
							float num = MBRandom.RandomFloat;
							float num2 = Campaign.MapDiagonal;
							foreach (Settlement settlement2 in hero2.Clan.Settlements)
							{
								float num3 = (settlement == settlement2) ? 0f : settlement2.Position2D.Distance(settlement.Position2D);
								if (num3 < num2)
								{
									num2 = num3;
								}
							}
							float num4 = (num2 < 100f) ? (1f - num2 / 100f) : 0f;
							float num5 = num4 * MBRandom.RandomFloat + (1f - num4);
							if (MBRandom.RandomFloat < 0.2f)
							{
								num5 = 1f / (0.5f + 0.5f * num5);
							}
							num *= num5;
							if (num > 1f)
							{
								num = 1f;
							}
							this.DetermineRelation(hero, hero2, num, chanceOfConflict);
						}
						for (int j = i + 1; j < settlement.Notables.Count; j++)
						{
							Hero hero3 = settlement.Notables[j];
							float chanceOfConflict2 = (float)HeroHelper.NPCPersonalityClashWithNPC(hero, hero3) * 0.01f * 2.5f;
							float randomValue = MBRandom.RandomFloat;
							if (hero.CharacterObject.Occupation == hero3.CharacterObject.Occupation)
							{
								randomValue = 1f - 0.25f * MBRandom.RandomFloat;
							}
							this.DetermineRelation(hero, hero3, randomValue, chanceOfConflict2);
						}
					}
				}
			}
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00110DB8 File Offset: 0x0010EFB8
		public void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i == 1)
			{
				this.SetInitialRelationsBetweenNotablesAndLords();
				int num = 50;
				for (int j = 0; j < num; j++)
				{
					foreach (Hero hero in Hero.AllAliveHeroes)
					{
						if (hero.IsNotable)
						{
							this.UpdateNotableSupport(hero);
						}
					}
				}
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00110E2C File Offset: 0x0010F02C
		private void DailyTickSettlement(Settlement settlement)
		{
			if (this._settlementPassedDaysForWeeklyTick.ContainsKey(settlement))
			{
				Dictionary<Settlement, int> settlementPassedDaysForWeeklyTick = this._settlementPassedDaysForWeeklyTick;
				int num = settlementPassedDaysForWeeklyTick[settlement];
				settlementPassedDaysForWeeklyTick[settlement] = num + 1;
				if (this._settlementPassedDaysForWeeklyTick[settlement] == 7)
				{
					SettlementHelper.SpawnNotablesIfNeeded(settlement);
					this._settlementPassedDaysForWeeklyTick[settlement] = 0;
					return;
				}
			}
			else
			{
				this._settlementPassedDaysForWeeklyTick.Add(settlement, 0);
			}
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x00110E90 File Offset: 0x0010F090
		private void UpdateNotableRelations(Hero notable)
		{
			foreach (Clan clan in Clan.All)
			{
				if (clan != Clan.PlayerClan && clan.Leader != null && !clan.IsEliminated)
				{
					int relation = notable.GetRelation(clan.Leader);
					if (relation > 0)
					{
						float num = (float)relation / 1000f;
						if (MBRandom.RandomFloat < num)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(notable, clan.Leader, -20, true);
						}
					}
					else if (relation < 0)
					{
						float num2 = (float)(-(float)relation) / 1000f;
						if (MBRandom.RandomFloat < num2)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(notable, clan.Leader, 20, true);
						}
					}
				}
			}
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00110F50 File Offset: 0x0010F150
		private void UpdateNotableSupport(Hero notable)
		{
			if (notable.SupporterOf == null)
			{
				using (IEnumerator<Clan> enumerator = Clan.NonBanditFactions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Clan clan = enumerator.Current;
						if (clan.Leader != null)
						{
							int relation = notable.GetRelation(clan.Leader);
							if (relation > 50)
							{
								float num = (float)(relation - 50) / 2000f;
								if (MBRandom.RandomFloat < num)
								{
									notable.SupporterOf = clan;
								}
							}
						}
					}
					return;
				}
			}
			int relation2 = notable.GetRelation(notable.SupporterOf.Leader);
			if (relation2 < 0)
			{
				notable.SupporterOf = null;
				return;
			}
			if (relation2 < 50)
			{
				float num2 = (float)(50 - relation2) / 500f;
				if (MBRandom.RandomFloat < num2)
				{
					notable.SupporterOf = null;
				}
			}
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x00111018 File Offset: 0x0010F218
		private void BalanceGoldAndPowerOfNotable(Hero notable)
		{
			if (notable.Gold > 10500)
			{
				int num = (notable.Gold - 10000) / 500;
				GiveGoldAction.ApplyBetweenCharacters(notable, null, num * 500, true);
				notable.AddPower((float)num);
				return;
			}
			if (notable.Gold < 4500 && notable.Power > 0f)
			{
				int num2 = (5000 - notable.Gold) / 500;
				GiveGoldAction.ApplyBetweenCharacters(null, notable, num2 * 500, true);
				notable.AddPower((float)(-(float)num2));
			}
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x001110A4 File Offset: 0x0010F2A4
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsNotable && hero.CurrentSettlement != null)
			{
				if (MBRandom.RandomFloat < 0.01f)
				{
					this.UpdateNotableRelations(hero);
				}
				this.UpdateNotableSupport(hero);
				this.BalanceGoldAndPowerOfNotable(hero);
				this.ManageCaravanExpensesOfNotable(hero);
				this.CheckAndMakeNotableDisappear(hero);
			}
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x001110F0 File Offset: 0x0010F2F0
		private void CheckAndMakeNotableDisappear(Hero notable)
		{
			if (notable.OwnedWorkshops.IsEmpty<Workshop>() && notable.OwnedCaravans.IsEmpty<CaravanPartyComponent>() && notable.OwnedAlleys.IsEmpty<Alley>() && notable.CanDie(KillCharacterAction.KillCharacterActionDetail.Lost) && notable.CanHaveQuestsOrIssues() && notable.Power < (float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit)
			{
				float randomFloat = MBRandom.RandomFloat;
				float notableDisappearProbability = this.GetNotableDisappearProbability(notable);
				if (randomFloat < notableDisappearProbability)
				{
					KillCharacterAction.ApplyByRemove(notable, false, true);
					IssueBase issue = notable.Issue;
					if (issue == null)
					{
						return;
					}
					issue.CompleteIssueWithAiLord(notable.CurrentSettlement.OwnerClan.Leader);
				}
			}
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x00111190 File Offset: 0x0010F390
		private void ManageCaravanExpensesOfNotable(Hero notable)
		{
			for (int i = notable.OwnedCaravans.Count - 1; i >= 0; i--)
			{
				CaravanPartyComponent caravanPartyComponent = notable.OwnedCaravans[i];
				int totalWage = caravanPartyComponent.MobileParty.TotalWage;
				if (caravanPartyComponent.MobileParty.PartyTradeGold >= totalWage)
				{
					caravanPartyComponent.MobileParty.PartyTradeGold -= totalWage;
				}
				else
				{
					int num = MathF.Min(totalWage, notable.Gold);
					notable.Gold -= num;
				}
				if (caravanPartyComponent.MobileParty.PartyTradeGold < 5000)
				{
					int num2 = MathF.Min(5000 - caravanPartyComponent.MobileParty.PartyTradeGold, notable.Gold);
					caravanPartyComponent.MobileParty.PartyTradeGold += num2;
					notable.Gold -= num2;
				}
			}
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x0011126A File Offset: 0x0010F46A
		private float GetNotableDisappearProbability(Hero hero)
		{
			return ((float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit - hero.Power) / (float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit * 0.02f;
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x001112A4 File Offset: 0x0010F4A4
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim.IsNotable)
			{
				if (victim.Power >= (float)Campaign.Current.Models.NotablePowerModel.NotableDisappearPowerLimit)
				{
					Hero hero = HeroCreator.CreateRelativeNotableHero(victim);
					if (victim.CurrentSettlement != null)
					{
						this.ChangeDeadNotable(victim, hero, victim.CurrentSettlement);
					}
					using (List<CaravanPartyComponent>.Enumerator enumerator = victim.OwnedCaravans.ToList<CaravanPartyComponent>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CaravanPartyComponent caravanPartyComponent = enumerator.Current;
							CaravanPartyComponent.TransferCaravanOwnership(caravanPartyComponent.MobileParty, hero, hero.CurrentSettlement);
						}
						return;
					}
				}
				foreach (CaravanPartyComponent caravanPartyComponent2 in victim.OwnedCaravans.ToList<CaravanPartyComponent>())
				{
					DestroyPartyAction.Apply(null, caravanPartyComponent2.MobileParty);
				}
			}
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x00111398 File Offset: 0x0010F598
		private void ChangeDeadNotable(Hero deadNotable, Hero newNotable, Settlement notableSettlement)
		{
			EnterSettlementAction.ApplyForCharacterOnly(newNotable, notableSettlement);
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (newNotable != hero)
				{
					int relation = deadNotable.GetRelation(hero);
					if (Math.Abs(relation) >= 20 || (relation != 0 && hero.CurrentSettlement == notableSettlement))
					{
						newNotable.SetPersonalRelation(hero, relation);
					}
				}
			}
			if (deadNotable.Issue != null)
			{
				Campaign.Current.IssueManager.ChangeIssueOwner(deadNotable.Issue, newNotable);
			}
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x00111434 File Offset: 0x0010F634
		private void SpawnNotablesAtGameStart()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown)
				{
					int targetNotableCountForSettlement = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Artisan);
					for (int i = 0; i < targetNotableCountForSettlement; i++)
					{
						HeroCreator.CreateHeroAtOccupation(Occupation.Artisan, settlement);
					}
					int targetNotableCountForSettlement2 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Merchant);
					for (int j = 0; j < targetNotableCountForSettlement2; j++)
					{
						HeroCreator.CreateHeroAtOccupation(Occupation.Merchant, settlement);
					}
					int targetNotableCountForSettlement3 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.GangLeader);
					for (int k = 0; k < targetNotableCountForSettlement3; k++)
					{
						HeroCreator.CreateHeroAtOccupation(Occupation.GangLeader, settlement);
					}
				}
				else if (settlement.IsVillage)
				{
					int targetNotableCountForSettlement4 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.RuralNotable);
					for (int l = 0; l < targetNotableCountForSettlement4; l++)
					{
						HeroCreator.CreateHeroAtOccupation(Occupation.RuralNotable, settlement);
					}
					int targetNotableCountForSettlement5 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Headman);
					for (int m = 0; m < targetNotableCountForSettlement5; m++)
					{
						HeroCreator.CreateHeroAtOccupation(Occupation.Headman, settlement);
					}
				}
			}
		}

		// Token: 0x040011AA RID: 4522
		private const int GoldLimitForNotablesToStartGainingPower = 10000;

		// Token: 0x040011AB RID: 4523
		private const int GoldLimitForNotablesToStartLosingPower = 5000;

		// Token: 0x040011AC RID: 4524
		private const int GoldNeededToGainOnePower = 500;

		// Token: 0x040011AD RID: 4525
		private const int CaravanGoldLowLimit = 5000;

		// Token: 0x040011AE RID: 4526
		private const int RemoveNotableCharacterAfterDays = 7;

		// Token: 0x040011AF RID: 4527
		private Dictionary<Settlement, int> _settlementPassedDaysForWeeklyTick;
	}
}
