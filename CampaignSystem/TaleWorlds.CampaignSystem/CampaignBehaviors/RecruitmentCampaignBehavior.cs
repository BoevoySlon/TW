using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003CE RID: 974
	public class RecruitmentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003BD7 RID: 15319 RVA: 0x0011E310 File Offset: 0x0011C510
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, new Action<Town>(this.DailyTickTown));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
			CampaignEvents.MercenaryNumberChangedInTown.AddNonSerializedListener(this, new Action<Town, int, int>(this.OnMercenaryNumberChanged));
			CampaignEvents.MercenaryTroopChangedInTown.AddNonSerializedListener(this, new Action<Town, CharacterObject, CharacterObject>(this.OnMercenaryTroopChanged));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.OnUnitRecruitedEvent.AddNonSerializedListener(this, new Action<CharacterObject, int>(this.OnUnitRecruited));
			CampaignEvents.OnTroopRecruitedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, Hero, CharacterObject, int>(this.OnTroopRecruited));
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0011E41A File Offset: 0x0011C61A
		private void DailyTickSettlement(Settlement settlement)
		{
			this.UpdateVolunteersOfNotablesInSettlement(settlement);
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x0011E423 File Offset: 0x0011C623
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CharacterObject>("_selectedTroop", ref this._selectedTroop);
			dataStore.SyncData<Dictionary<Town, RecruitmentCampaignBehavior.TownMercenaryData>>("_townMercenaryData", ref this._townMercenaryData);
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x0011E44C File Offset: 0x0011C64C
		private RecruitmentCampaignBehavior.TownMercenaryData GetMercenaryData(Town town)
		{
			RecruitmentCampaignBehavior.TownMercenaryData townMercenaryData;
			if (!this._townMercenaryData.TryGetValue(town, out townMercenaryData))
			{
				townMercenaryData = new RecruitmentCampaignBehavior.TownMercenaryData(town);
				this._townMercenaryData.Add(town, townMercenaryData);
			}
			return townMercenaryData;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x0011E480 File Offset: 0x0011C680
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			foreach (Town town in Town.AllTowns)
			{
				this.UpdateCurrentMercenaryTroopAndCount(town, true);
			}
			foreach (Settlement settlement in Settlement.All)
			{
				this.UpdateVolunteersOfNotablesInSettlement(settlement);
			}
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x0011E514 File Offset: 0x0011C714
		private void OnTroopRecruited(Hero recruiter, Settlement settlement, Hero recruitmentSource, CharacterObject troop, int count)
		{
			if (recruiter != null && recruiter.PartyBelongedTo != null && recruiter.GetPerkValue(DefaultPerks.Leadership.FamousCommander))
			{
				recruiter.PartyBelongedTo.MemberRoster.AddXpToTroop((int)DefaultPerks.Leadership.FamousCommander.SecondaryBonus * count, troop);
			}
			SkillLevelingManager.OnTroopRecruited(recruiter, count, troop.Tier);
			if (recruiter != null && recruiter.PartyBelongedTo != null && troop.Occupation == Occupation.Bandit)
			{
				SkillLevelingManager.OnBanditsRecruited(recruiter.PartyBelongedTo, troop, count);
			}
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0011E590 File Offset: 0x0011C790
		private void OnUnitRecruited(CharacterObject troop, int count)
		{
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Leadership.FamousCommander))
			{
				MobileParty.MainParty.MemberRoster.AddXpToTroop((int)DefaultPerks.Leadership.FamousCommander.SecondaryBonus * count, troop);
			}
			SkillLevelingManager.OnTroopRecruited(Hero.MainHero, count, troop.Tier);
			if (troop.Occupation == Occupation.Bandit)
			{
				SkillLevelingManager.OnBanditsRecruited(MobileParty.MainParty, troop, count);
			}
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0011E5F4 File Offset: 0x0011C7F4
		private void DailyTickTown(Town town)
		{
			this.UpdateCurrentMercenaryTroopAndCount(town, (int)CampaignTime.Now.ToDays % 2 == 0);
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x0011E61B File Offset: 0x0011C81B
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x0011E62B File Offset: 0x0011C82B
		private void OnMercenaryNumberChanged(Town town, int oldNumber, int newNumber)
		{
			this.CheckIfMercenaryCharacterNeedsToRefresh(town.Owner.Settlement, this.GetMercenaryData(town).TroopType);
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x0011E64A File Offset: 0x0011C84A
		private void OnMercenaryTroopChanged(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
			this.CheckIfMercenaryCharacterNeedsToRefresh(town.Owner.Settlement, oldTroopType);
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x0011E660 File Offset: 0x0011C860
		private void UpdateVolunteersOfNotablesInSettlement(Settlement settlement)
		{
			if ((settlement.IsTown && !settlement.Town.InRebelliousState) || (settlement.IsVillage && !settlement.Village.Bound.Town.InRebelliousState))
			{
				foreach (Hero hero in settlement.Notables)
				{
					if (hero.CanHaveRecruits && hero.IsAlive)
					{
						bool flag = false;
						CharacterObject basicVolunteer = Campaign.Current.Models.VolunteerModel.GetBasicVolunteer(hero);
						for (int i = 0; i < 6; i++)
						{
							if (MBRandom.RandomFloat < Campaign.Current.Models.VolunteerModel.GetDailyVolunteerProductionProbability(hero, i, settlement))
							{
								CharacterObject characterObject = hero.VolunteerTypes[i];
								if (characterObject == null)
								{
									hero.VolunteerTypes[i] = basicVolunteer;
									flag = true;
								}
								else if (characterObject.UpgradeTargets.Length != 0 && characterObject.Tier < Campaign.Current.Models.VolunteerModel.MaxVolunteerTier)
								{
									float num = MathF.Log(hero.Power / (float)characterObject.Tier, 2f) * 0.01f;
									if (MBRandom.RandomFloat < num)
									{
										hero.VolunteerTypes[i] = characterObject.UpgradeTargets[MBRandom.RandomInt(characterObject.UpgradeTargets.Length)];
										flag = true;
									}
								}
							}
						}
						if (flag)
						{
							CharacterObject[] volunteerTypes = hero.VolunteerTypes;
							for (int j = 1; j < 6; j++)
							{
								CharacterObject characterObject2 = volunteerTypes[j];
								if (characterObject2 != null)
								{
									int num2 = 0;
									int num3 = j - 1;
									CharacterObject characterObject3 = volunteerTypes[num3];
									while (num3 >= 0 && (characterObject3 == null || (float)characterObject2.Level + (characterObject2.IsMounted ? 0.5f : 0f) < (float)characterObject3.Level + (characterObject3.IsMounted ? 0.5f : 0f)))
									{
										if (characterObject3 == null)
										{
											num3--;
											num2++;
											if (num3 >= 0)
											{
												characterObject3 = volunteerTypes[num3];
											}
										}
										else
										{
											volunteerTypes[num3 + 1 + num2] = characterObject3;
											num3--;
											num2 = 0;
											if (num3 >= 0)
											{
												characterObject3 = volunteerTypes[num3];
											}
										}
									}
									volunteerTypes[num3 + 1 + num2] = characterObject2;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x0011E8C8 File Offset: 0x0011CAC8
		public void HourlyTickParty(MobileParty mobileParty)
		{
			if ((mobileParty.IsCaravan || mobileParty.IsLordParty) && mobileParty.MapEvent == null && mobileParty != MobileParty.MainParty)
			{
				Settlement currentSettlementOfMobilePartyForAICalculation = MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(mobileParty);
				if (currentSettlementOfMobilePartyForAICalculation != null)
				{
					if ((currentSettlementOfMobilePartyForAICalculation.IsVillage && !currentSettlementOfMobilePartyForAICalculation.IsRaided && !currentSettlementOfMobilePartyForAICalculation.IsUnderRaid) || (currentSettlementOfMobilePartyForAICalculation.IsTown && !currentSettlementOfMobilePartyForAICalculation.IsUnderSiege))
					{
						this.CheckRecruiting(mobileParty, currentSettlementOfMobilePartyForAICalculation);
						return;
					}
				}
				else if (MBRandom.RandomFloat < 0.05f && mobileParty.LeaderHero != null && mobileParty.ActualClan != Clan.PlayerClan && !mobileParty.IsCaravan)
				{
					IFaction mapFaction = mobileParty.MapFaction;
					if (mapFaction != null && mapFaction.IsMinorFaction && MobileParty.MainParty.Position2D.DistanceSquared(mobileParty.Position2D) > (MobileParty.MainParty.SeeingRange + 5f) * (MobileParty.MainParty.SeeingRange + 5f))
					{
						int partySizeLimit = mobileParty.Party.PartySizeLimit;
						float num = (float)mobileParty.Party.NumberOfAllMembers / (float)partySizeLimit;
						float num2 = ((double)num < 0.2) ? 1000f : (((double)num < 0.3) ? 2000f : (((double)num < 0.4) ? 3000f : (((double)num < 0.55) ? 4000f : (((double)num < 0.7) ? 5000f : 7000f))));
						float num3 = ((float)mobileParty.LeaderHero.Gold > num2) ? 1f : MathF.Sqrt((float)mobileParty.LeaderHero.Gold / num2);
						if (MBRandom.RandomFloat < (1f - num) * num3)
						{
							CharacterObject basicTroop = mobileParty.ActualClan.BasicTroop;
							int num4 = MBRandom.RandomInt(3, 8);
							if (num4 + mobileParty.Party.NumberOfAllMembers > partySizeLimit)
							{
								num4 = partySizeLimit - mobileParty.Party.NumberOfAllMembers;
							}
							int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(basicTroop, mobileParty.LeaderHero, false);
							if (num4 * troopRecruitmentCost > mobileParty.LeaderHero.Gold)
							{
								num4 = mobileParty.LeaderHero.Gold / troopRecruitmentCost;
							}
							if (num4 > 0)
							{
								this.GetRecruitVolunteerFromMap(mobileParty, basicTroop, num4);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x0011EB18 File Offset: 0x0011CD18
		private void UpdateCurrentMercenaryTroopAndCount(Town town, bool forceUpdate = false)
		{
			RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = this.GetMercenaryData(town);
			if (!forceUpdate && mercenaryData.HasAvailableMercenary(Occupation.NotAssigned))
			{
				int difference = this.FindNumberOfMercenariesWillBeAdded(mercenaryData.TroopType, true);
				mercenaryData.ChangeMercenaryCount(difference);
				return;
			}
			if (MBRandom.RandomFloat < Campaign.Current.Models.TavernMercenaryTroopsModel.RegularMercenariesSpawnChance)
			{
				CharacterObject randomElementInefficiently = town.Culture.BasicMercenaryTroops.GetRandomElementInefficiently<CharacterObject>();
				this._selectedTroop = null;
				float num = this.FindTotalMercenaryProbability(randomElementInefficiently, 1f);
				float randomValueRemaining = MBRandom.RandomFloat * num;
				this.FindRandomMercenaryTroop(randomElementInefficiently, 1f, randomValueRemaining);
				int number = this.FindNumberOfMercenariesWillBeAdded(this._selectedTroop, false);
				mercenaryData.ChangeMercenaryType(this._selectedTroop, number);
				return;
			}
			CharacterObject caravanGuard = town.Culture.CaravanGuard;
			if (caravanGuard != null)
			{
				this._selectedTroop = null;
				float num2 = this.FindTotalMercenaryProbability(caravanGuard, 1f);
				float randomValueRemaining2 = MBRandom.RandomFloat * num2;
				this.FindRandomMercenaryTroop(caravanGuard, 1f, randomValueRemaining2);
				int number2 = this.FindNumberOfMercenariesWillBeAdded(this._selectedTroop, false);
				mercenaryData.ChangeMercenaryType(this._selectedTroop, number2);
			}
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x0011EC24 File Offset: 0x0011CE24
		private float FindTotalMercenaryProbability(CharacterObject mercenaryTroop, float probabilityOfTroop)
		{
			float num = probabilityOfTroop;
			foreach (CharacterObject mercenaryTroop2 in mercenaryTroop.UpgradeTargets)
			{
				num += this.FindTotalMercenaryProbability(mercenaryTroop2, probabilityOfTroop / 1.5f);
			}
			return num;
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x0011EC60 File Offset: 0x0011CE60
		private float FindRandomMercenaryTroop(CharacterObject mercenaryTroop, float probabilityOfTroop, float randomValueRemaining)
		{
			randomValueRemaining -= probabilityOfTroop;
			if (randomValueRemaining <= 1E-05f && this._selectedTroop == null)
			{
				this._selectedTroop = mercenaryTroop;
				return 1f;
			}
			float num = probabilityOfTroop;
			foreach (CharacterObject mercenaryTroop2 in mercenaryTroop.UpgradeTargets)
			{
				float num2 = this.FindRandomMercenaryTroop(mercenaryTroop2, probabilityOfTroop / 1.5f, randomValueRemaining);
				randomValueRemaining -= num2;
				num += num2;
			}
			return num;
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0011ECC8 File Offset: 0x0011CEC8
		private int FindNumberOfMercenariesWillBeAdded(CharacterObject character, bool dailyUpdate = false)
		{
			int tier = Campaign.Current.Models.CharacterStatsModel.GetTier(character);
			int maxCharacterTier = Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier;
			int num = (maxCharacterTier - tier) * 2;
			int num2 = (maxCharacterTier - tier) * 5;
			float randomFloat = MBRandom.RandomFloat;
			float randomFloat2 = MBRandom.RandomFloat;
			return MBRandom.RoundRandomized(MBMath.ClampFloat((randomFloat * randomFloat2 * (float)(num2 - num) + (float)num) * (dailyUpdate ? 0.1f : 1f), 1f, (float)num2));
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0011ED40 File Offset: 0x0011CF40
		private void CheckIfMercenaryCharacterNeedsToRefresh(Settlement settlement, CharacterObject oldTroopType)
		{
			if (settlement.IsTown && settlement == Settlement.CurrentSettlement && PlayerEncounter.LocationEncounter != null && settlement.LocationComplex != null && (CampaignMission.Current == null || GameStateManager.Current.ActiveState != CampaignMission.Current.State))
			{
				if (oldTroopType != null)
				{
					Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("tavern").RemoveAllCharacters((LocationCharacter x) => x.Character.Occupation == oldTroopType.Occupation);
				}
				this.AddMercenaryCharacterToTavern(settlement);
			}
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0011EDCC File Offset: 0x0011CFCC
		private void AddMercenaryCharacterToTavern(Settlement settlement)
		{
			if (settlement.LocationComplex != null && settlement.IsTown && this.GetMercenaryData(settlement.Town).HasAvailableMercenary(Occupation.NotAssigned))
			{
				Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("tavern");
				if (locationWithId != null)
				{
					locationWithId.AddLocationCharacters(new CreateLocationCharacterDelegate(this.CreateMercenary), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
				}
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0011EE30 File Offset: 0x0011D030
		private void CheckRecruiting(MobileParty mobileParty, Settlement settlement)
		{
			if (settlement.IsTown && mobileParty.IsCaravan)
			{
				RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = this.GetMercenaryData(settlement.Town);
				if (mercenaryData.HasAvailableMercenary(Occupation.CaravanGuard) || mercenaryData.HasAvailableMercenary(Occupation.Mercenary))
				{
					int partySizeLimit = mobileParty.Party.PartySizeLimit;
					if (mobileParty.Party.NumberOfAllMembers < partySizeLimit)
					{
						CharacterObject troopType = mercenaryData.TroopType;
						int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(troopType, mobileParty.LeaderHero, false);
						int num = mobileParty.IsCaravan ? 2000 : 0;
						if (mobileParty.PartyTradeGold > troopRecruitmentCost + num)
						{
							bool flag = true;
							double num2 = 0.0;
							for (int i = 0; i < mercenaryData.Number; i++)
							{
								if (flag)
								{
									int num3 = mobileParty.PartyTradeGold - (troopRecruitmentCost + num);
									double num4 = (double)MathF.Min(1f, MathF.Sqrt((float)num3 / (100f * (float)troopRecruitmentCost)));
									float num5 = (float)mobileParty.Party.NumberOfAllMembers / (float)partySizeLimit;
									float num6 = (MathF.Min(10f, 1f / num5) * MathF.Min(10f, 1f / num5) - 1f) * ((mobileParty.IsCaravan && mobileParty.Party.Owner == Hero.MainHero) ? 0.4f : 0.1f);
									num2 = num4 * (double)num6;
								}
								if ((double)MBRandom.RandomFloat < num2)
								{
									this.ApplyRecruitMercenary(mobileParty, settlement, troopType, 1);
									flag = true;
								}
								else
								{
									flag = false;
								}
							}
							return;
						}
					}
				}
			}
			else if (mobileParty.IsLordParty && !mobileParty.IsDisbanding && mobileParty.LeaderHero != null && !mobileParty.Party.IsStarving && (float)mobileParty.LeaderHero.Gold > HeroHelper.StartRecruitingMoneyLimit(mobileParty.LeaderHero) && (mobileParty.LeaderHero == mobileParty.LeaderHero.Clan.Leader || (float)mobileParty.LeaderHero.Clan.Gold > HeroHelper.StartRecruitingMoneyLimitForClanLeader(mobileParty.LeaderHero)) && ((float)mobileParty.Party.NumberOfAllMembers + 0.5f) / (float)mobileParty.LimitedPartySize <= 1f)
			{
				if (settlement.IsTown && this.GetMercenaryData(settlement.Town).HasAvailableMercenary(Occupation.Mercenary))
				{
					float num7 = (float)mobileParty.Party.NumberOfAllMembers / (float)mobileParty.LimitedPartySize;
					CharacterObject troopType2 = this.GetMercenaryData(settlement.Town).TroopType;
					if (troopType2 != null)
					{
						int troopRecruitmentCost2 = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(troopType2, mobileParty.LeaderHero, false);
						if (troopRecruitmentCost2 < 5000)
						{
							float num8 = MathF.Min(1f, (float)mobileParty.LeaderHero.Gold / ((troopRecruitmentCost2 <= 100) ? 100000f : ((float)((troopRecruitmentCost2 <= 200) ? 125000 : ((troopRecruitmentCost2 <= 400) ? 150000 : ((troopRecruitmentCost2 <= 700) ? 175000 : ((troopRecruitmentCost2 <= 1100) ? 200000 : ((troopRecruitmentCost2 <= 1600) ? 250000 : ((troopRecruitmentCost2 <= 2200) ? 300000 : 400000)))))))));
							float num9 = num8 * num8;
							float num10 = MathF.Max(1f, MathF.Min(10f, 1f / num7)) - 1f;
							float num11 = num9 * num10 * 0.25f;
							int number = this.GetMercenaryData(settlement.Town).Number;
							int num12 = 0;
							for (int j = 0; j < number; j++)
							{
								if (MBRandom.RandomFloat < num11)
								{
									num12++;
								}
							}
							num12 = MathF.Min(num12, mobileParty.LimitedPartySize - mobileParty.Party.NumberOfAllMembers);
							num12 = (((double)troopRecruitmentCost2 <= 0.1) ? num12 : MathF.Min(mobileParty.LeaderHero.Gold / troopRecruitmentCost2, num12));
							if (num12 > 0)
							{
								this.ApplyRecruitMercenary(mobileParty, settlement, troopType2, num12);
							}
						}
					}
				}
				if (mobileParty.Party.NumberOfAllMembers < mobileParty.LimitedPartySize && mobileParty.CanPayMoreWage())
				{
					this.RecruitVolunteersFromNotable(mobileParty, settlement);
				}
			}
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x0011F248 File Offset: 0x0011D448
		private void RecruitVolunteersFromNotable(MobileParty mobileParty, Settlement settlement)
		{
			if (((float)mobileParty.Party.NumberOfAllMembers + 0.5f) / (float)mobileParty.LimitedPartySize <= 1f)
			{
				foreach (Hero hero in settlement.Notables)
				{
					if (hero.IsAlive)
					{
						if (mobileParty.IsWageLimitExceeded())
						{
							break;
						}
						int num = MBRandom.RandomInt(6);
						int num2 = Campaign.Current.Models.VolunteerModel.MaximumIndexHeroCanRecruitFromHero(mobileParty.IsGarrison ? mobileParty.Party.Owner : mobileParty.LeaderHero, hero, -101);
						for (int i = num; i < num + 6; i++)
						{
							int num3 = i % 6;
							if (num3 >= num2)
							{
								break;
							}
							int num4 = (mobileParty.LeaderHero != null) ? ((int)MathF.Sqrt((float)mobileParty.LeaderHero.Gold / 10000f)) : 0;
							float num5 = MBRandom.RandomFloat;
							for (int j = 0; j < num4; j++)
							{
								float randomFloat = MBRandom.RandomFloat;
								if (randomFloat > num5)
								{
									num5 = randomFloat;
								}
							}
							if (mobileParty.Army != null)
							{
								float y = (mobileParty.Army.LeaderParty == mobileParty) ? 0.5f : 0.67f;
								num5 = MathF.Pow(num5, y);
							}
							float num6 = (float)mobileParty.Party.NumberOfAllMembers / (float)mobileParty.LimitedPartySize;
							if (num5 > num6 - 0.1f)
							{
								CharacterObject characterObject = hero.VolunteerTypes[num3];
								if (characterObject != null && mobileParty.LeaderHero.Gold > Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(characterObject, mobileParty.LeaderHero, false) && mobileParty.PaymentLimit >= mobileParty.TotalWage + Campaign.Current.Models.PartyWageModel.GetCharacterWage(characterObject))
								{
									this.GetRecruitVolunteerFromIndividual(mobileParty, characterObject, hero, num3);
									break;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x0011F450 File Offset: 0x0011D650
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (mobileParty != null && mobileParty.MapEvent == null)
			{
				if (!settlement.IsVillage)
				{
					Clan ownerClan = settlement.OwnerClan;
					if (ownerClan == null || ownerClan.IsAtWarWith(mobileParty.MapFaction))
					{
						return;
					}
				}
				if (!settlement.IsRaided && !settlement.IsUnderRaid)
				{
					int num = mobileParty.IsCaravan ? 1 : ((mobileParty.Army != null && mobileParty.Army == MobileParty.MainParty.Army) ? ((MobileParty.MainParty.PartySizeRatio < 0.6f) ? 1 : ((MobileParty.MainParty.PartySizeRatio < 0.9f) ? 2 : 3)) : 7);
					List<MobileParty> list = new List<MobileParty>();
					if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
					{
						using (List<MobileParty>.Enumerator enumerator = mobileParty.Army.Parties.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								MobileParty mobileParty2 = enumerator.Current;
								if ((mobileParty2 == mobileParty.Army.LeaderParty || mobileParty2.AttachedTo == mobileParty.Army.LeaderParty) && mobileParty2 != MobileParty.MainParty)
								{
									list.Add(mobileParty2);
								}
							}
							goto IL_138;
						}
					}
					if (mobileParty.AttachedTo == null && mobileParty != MobileParty.MainParty)
					{
						list.Add(mobileParty);
					}
					IL_138:
					for (int i = 0; i < num; i++)
					{
						foreach (MobileParty mobileParty3 in list)
						{
							this.CheckRecruiting(mobileParty3, settlement);
						}
					}
				}
			}
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x0011F5F8 File Offset: 0x0011D7F8
		private void ApplyInternal(MobileParty side1Party, Settlement settlement, Hero individual, CharacterObject troop, int number, int bitCode, RecruitmentCampaignBehavior.RecruitingDetail detail)
		{
			int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(troop, side1Party.LeaderHero, false);
			if (detail == RecruitmentCampaignBehavior.RecruitingDetail.MercenaryFromTavern)
			{
				if (side1Party.IsCaravan)
				{
					side1Party.PartyTradeGold -= number * troopRecruitmentCost;
					this.GetMercenaryData(settlement.Town).ChangeMercenaryCount(-number);
				}
				else
				{
					GiveGoldAction.ApplyBetweenCharacters(side1Party.LeaderHero, null, number * troopRecruitmentCost, true);
					this.GetMercenaryData(settlement.Town).ChangeMercenaryCount(-number);
				}
				side1Party.AddElementToMemberRoster(troop, number, false);
			}
			else if (detail == RecruitmentCampaignBehavior.RecruitingDetail.VolunteerFromIndividual)
			{
				GiveGoldAction.ApplyBetweenCharacters(side1Party.LeaderHero, null, troopRecruitmentCost, true);
				individual.VolunteerTypes[bitCode] = null;
				side1Party.AddElementToMemberRoster(troop, 1, false);
			}
			else if (detail == RecruitmentCampaignBehavior.RecruitingDetail.VolunteerFromMap)
			{
				GiveGoldAction.ApplyBetweenCharacters(side1Party.LeaderHero, null, number * troopRecruitmentCost, true);
				side1Party.AddElementToMemberRoster(troop, number, false);
			}
			else if (detail == RecruitmentCampaignBehavior.RecruitingDetail.VolunteerFromIndividualToGarrison)
			{
				individual.VolunteerTypes[bitCode] = null;
				side1Party.AddElementToMemberRoster(troop, 1, false);
			}
			CampaignEventDispatcher.Instance.OnTroopRecruited(side1Party.LeaderHero, settlement, individual, troop, number);
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x0011F707 File Offset: 0x0011D907
		private void ApplyRecruitMercenary(MobileParty side1Party, Settlement side2Party, CharacterObject subject, int number)
		{
			this.ApplyInternal(side1Party, side2Party, null, subject, number, -1, RecruitmentCampaignBehavior.RecruitingDetail.MercenaryFromTavern);
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x0011F717 File Offset: 0x0011D917
		private void GetRecruitVolunteerFromMap(MobileParty side1Party, CharacterObject subject, int number)
		{
			this.ApplyInternal(side1Party, null, null, subject, number, -1, RecruitmentCampaignBehavior.RecruitingDetail.VolunteerFromMap);
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x0011F726 File Offset: 0x0011D926
		private void GetRecruitVolunteerFromIndividual(MobileParty side1Party, CharacterObject subject, Hero individual, int bitCode)
		{
			this.ApplyInternal(side1Party, individual.CurrentSettlement, individual, subject, 1, bitCode, RecruitmentCampaignBehavior.RecruitingDetail.VolunteerFromIndividual);
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x0011F73C File Offset: 0x0011D93C
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			Location locationWithId = settlement.LocationComplex.GetLocationWithId("tavern");
			if (CampaignMission.Current.Location == locationWithId)
			{
				this.AddMercenaryCharacterToTavern(settlement);
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x0011F77C File Offset: 0x0011D97C
		private LocationCharacter CreateMercenary(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject troopType = this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).TroopType;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(troopType.Race, "_settlement");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(troopType, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).NoHorses(true), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "spawnpoint_mercenary", true, relation, null, false, false, null, false, false, true);
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0011F7FC File Offset: 0x0011D9FC
		protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenuOption("town_backstreet", "recruit_mercenaries", "{=NwO0CVzn}Recruit {MEN_COUNT} {MERCENARY_NAME} ({TOTAL_AMOUNT}{GOLD_ICON})", new GameMenuOption.OnConditionDelegate(this.buy_mercenaries_condition), delegate(MenuCallbackArgs x)
			{
				this.buy_mercenaries_on_consequence();
			}, false, 2, false, null);
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0011F83C File Offset: 0x0011DA3C
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("mercenary_recruit_start", "start", "mercenary_tavern_talk", "{=I0StkXlK}Do you have a need for fighters, {?PLAYER.GENDER}madam{?}sir{\\?}? Me and {?PLURAL}{MERCENARY_COUNT} of my mates{?}one of my mates{\\?} are looking for a master. You might call us mercenaries, like. We'll join you for {GOLD_AMOUNT}{GOLD_ICON}", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruit_plural_start_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("mercenary_recruit_start_single", "start", "mercenary_tavern_talk", "{=rJwExPKb}Do you have a need for fighters, {?PLAYER.GENDER}madam{?}sir{\\?}? I am looking for a master. I'll join you for {GOLD_AMOUNT}{GOLD_ICON}", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruit_single_start_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("mercenary_recruit_accept", "mercenary_tavern_talk", "mercenary_tavern_talk_hire", "{=PDLDvUfH}All right. I will hire {?PLURAL}all of you{?}you{\\?}. Here is {GOLD_AMOUNT}{GOLD_ICON}", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruit_accept_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_mercenary_recruit_accept_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("mercenary_recruit_accept_some", "mercenary_tavern_talk", "mercenary_tavern_talk_hire", "{=aTPc7AkY}All right. But I can only hire {MERCENARY_COUNT} of you. Here is {GOLD_AMOUNT}{GOLD_ICON}", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruit_accept_some_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_mercenary_recruit_accept_some_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("mercenary_recruit_reject_gold", "mercenary_tavern_talk", "close_window", "{=n5BGNLrc}That sounds good. But I can't afford any more men right now.", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruit_reject_gold_on_condition), null, 100, null, null);
			campaignGameStarter.AddPlayerLine("mercenary_recruit_reject", "mercenary_tavern_talk", "close_window", "{=I2thb8VU}Sorry. I don't need any other men right now.", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruit_dont_need_men_on_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("mercenary_recruit_end", "mercenary_tavern_talk_hire", "close_window", "{=vbxQoyN3}{RANDOM_HIRE_SENTENCE}", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruit_end_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("mercenary_recruit_start_2", "start", "close_window", "{=Jhj437BV}Don't worry, I'll be ready. Just having a last drink for the road.", new ConversationSentence.OnConditionDelegate(this.conversation_mercenary_recruited_on_condition), null, 100, null);
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0011F9BC File Offset: 0x0011DBBC
		private bool buy_mercenaries_condition(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement != null && MobileParty.MainParty.CurrentSettlement.IsTown && this.GetMercenaryData(MobileParty.MainParty.CurrentSettlement.Town).Number > 0)
			{
				RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = this.GetMercenaryData(MobileParty.MainParty.CurrentSettlement.Town);
				int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(mercenaryData.TroopType, Hero.MainHero, false);
				if (Hero.MainHero.Gold >= troopRecruitmentCost)
				{
					int num = MathF.Min(mercenaryData.Number, Hero.MainHero.Gold / troopRecruitmentCost);
					MBTextManager.SetTextVariable("MEN_COUNT", num);
					MBTextManager.SetTextVariable("MERCENARY_NAME", mercenaryData.TroopType.Name, false);
					MBTextManager.SetTextVariable("TOTAL_AMOUNT", num * troopRecruitmentCost);
				}
				else
				{
					args.Tooltip = GameTexts.FindText("str_decision_not_enough_gold", null);
					args.IsEnabled = false;
					int number = mercenaryData.Number;
					MBTextManager.SetTextVariable("MEN_COUNT", number);
					MBTextManager.SetTextVariable("MERCENARY_NAME", mercenaryData.TroopType.Name, false);
					MBTextManager.SetTextVariable("TOTAL_AMOUNT", number * troopRecruitmentCost);
				}
				args.optionLeaveType = GameMenuOption.LeaveType.Bribe;
				return true;
			}
			return false;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0011FAF0 File Offset: 0x0011DCF0
		private void buy_mercenaries_on_consequence()
		{
			if (MobileParty.MainParty.CurrentSettlement != null && MobileParty.MainParty.CurrentSettlement.IsTown && this.GetMercenaryData(MobileParty.MainParty.CurrentSettlement.Town).Number > 0)
			{
				RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = this.GetMercenaryData(MobileParty.MainParty.CurrentSettlement.Town);
				int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(mercenaryData.TroopType, Hero.MainHero, false);
				if (Hero.MainHero.Gold >= troopRecruitmentCost)
				{
					int num = MathF.Min(mercenaryData.Number, Hero.MainHero.Gold / troopRecruitmentCost);
					MobileParty.MainParty.MemberRoster.AddToCounts(mercenaryData.TroopType, num, false, 0, 0, true, -1);
					GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, -(num * troopRecruitmentCost), false);
					mercenaryData.ChangeMercenaryCount(-num);
					GameMenu.SwitchToMenu("town_backstreet");
				}
			}
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x0011FBDC File Offset: 0x0011DDDC
		private bool conversation_mercenary_recruit_plural_start_on_condition()
		{
			if (PlayerEncounter.EncounterSettlement == null || !PlayerEncounter.EncounterSettlement.IsTown)
			{
				return false;
			}
			RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town);
			bool flag = (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Mercenary || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.CaravanGuard || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Gangster) && PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.IsTown && mercenaryData.Number > 1;
			if (flag)
			{
				int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(mercenaryData.TroopType, Hero.MainHero, false);
				MBTextManager.SetTextVariable("PLURAL", (mercenaryData.Number - 1 > 1) ? 1 : 0);
				MBTextManager.SetTextVariable("MERCENARY_COUNT", mercenaryData.Number - 1);
				MBTextManager.SetTextVariable("GOLD_AMOUNT", troopRecruitmentCost * mercenaryData.Number);
			}
			return flag;
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x0011FCB8 File Offset: 0x0011DEB8
		private bool conversation_mercenary_recruit_single_start_on_condition()
		{
			if (PlayerEncounter.EncounterSettlement == null || !PlayerEncounter.EncounterSettlement.IsTown)
			{
				return false;
			}
			RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town);
			bool flag = (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Mercenary || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.CaravanGuard || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Gangster) && PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.IsTown && mercenaryData.Number == 1;
			if (flag)
			{
				int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(mercenaryData.TroopType, Hero.MainHero, false);
				MBTextManager.SetTextVariable("GOLD_AMOUNT", mercenaryData.Number * troopRecruitmentCost);
			}
			return flag;
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x0011FD6C File Offset: 0x0011DF6C
		private bool conversation_mercenary_recruit_accept_on_condition()
		{
			RecruitmentCampaignBehavior.TownMercenaryData mercenaryData = this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town);
			int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(mercenaryData.TroopType, Hero.MainHero, false);
			MBTextManager.SetTextVariable("PLURAL", (mercenaryData.Number > 1) ? 1 : 0);
			return Hero.MainHero.Gold >= mercenaryData.Number * troopRecruitmentCost;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0011FDD9 File Offset: 0x0011DFD9
		private bool conversation_mercenary_recruited_on_condition()
		{
			return (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Mercenary || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.CaravanGuard || CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Gangster) && PlayerEncounter.EncounterSettlement != null;
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x0011FE10 File Offset: 0x0011E010
		private void BuyMercenaries()
		{
			this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).ChangeMercenaryCount(-this._selectedMercenaryCount);
			int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).TroopType, Hero.MainHero, false);
			MobileParty.MainParty.AddElementToMemberRoster(CharacterObject.OneToOneConversationCharacter, this._selectedMercenaryCount, false);
			int amount = this._selectedMercenaryCount * troopRecruitmentCost;
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, amount, false);
			CampaignEventDispatcher.Instance.OnUnitRecruited(CharacterObject.OneToOneConversationCharacter, this._selectedMercenaryCount);
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x0011FEAB File Offset: 0x0011E0AB
		private void conversation_mercenary_recruit_accept_on_consequence()
		{
			this._selectedMercenaryCount = this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).Number;
			this.BuyMercenaries();
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x0011FED0 File Offset: 0x0011E0D0
		private bool conversation_mercenary_recruit_accept_some_on_condition()
		{
			int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).TroopType, Hero.MainHero, false);
			if (Hero.MainHero.Gold >= troopRecruitmentCost && Hero.MainHero.Gold < this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).Number * troopRecruitmentCost)
			{
				this._selectedMercenaryCount = 0;
				while (Hero.MainHero.Gold >= troopRecruitmentCost * (this._selectedMercenaryCount + 1))
				{
					this._selectedMercenaryCount++;
				}
				MBTextManager.SetTextVariable("MERCENARY_COUNT", this._selectedMercenaryCount);
				MBTextManager.SetTextVariable("GOLD_AMOUNT", troopRecruitmentCost * this._selectedMercenaryCount);
				return true;
			}
			return false;
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x0011FF8F File Offset: 0x0011E18F
		private void conversation_mercenary_recruit_accept_some_on_consequence()
		{
			this.BuyMercenaries();
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x0011FF98 File Offset: 0x0011E198
		private bool conversation_mercenary_recruit_reject_gold_on_condition()
		{
			int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).TroopType, Hero.MainHero, false);
			return Hero.MainHero.Gold < troopRecruitmentCost;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x0011FFE4 File Offset: 0x0011E1E4
		private bool conversation_mercenary_recruit_dont_need_men_on_condition()
		{
			int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(this.GetMercenaryData(PlayerEncounter.EncounterSettlement.Town).TroopType, Hero.MainHero, false);
			return Hero.MainHero.Gold >= troopRecruitmentCost;
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x00120034 File Offset: 0x0011E234
		private bool conversation_mercenary_recruit_end_on_condition()
		{
			MBTextManager.SetTextVariable("RANDOM_HIRE_SENTENCE", GameTexts.FindText("str_mercenary_tavern_talk_hire", MBRandom.RandomInt(4).ToString()), false);
			return true;
		}

		// Token: 0x040011F4 RID: 4596
		private Dictionary<Town, RecruitmentCampaignBehavior.TownMercenaryData> _townMercenaryData = new Dictionary<Town, RecruitmentCampaignBehavior.TownMercenaryData>();

		// Token: 0x040011F5 RID: 4597
		private int _selectedMercenaryCount;

		// Token: 0x040011F6 RID: 4598
		private CharacterObject _selectedTroop;

		// Token: 0x02000727 RID: 1831
		public class RecruitmentCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x06005930 RID: 22832 RVA: 0x00183BC4 File Offset: 0x00181DC4
			public RecruitmentCampaignBehaviorTypeDefiner() : base(881200)
			{
			}

			// Token: 0x06005931 RID: 22833 RVA: 0x00183BD1 File Offset: 0x00181DD1
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(RecruitmentCampaignBehavior.TownMercenaryData), 1, null);
			}

			// Token: 0x06005932 RID: 22834 RVA: 0x00183BE5 File Offset: 0x00181DE5
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<Town, RecruitmentCampaignBehavior.TownMercenaryData>));
			}
		}

		// Token: 0x02000728 RID: 1832
		internal class TownMercenaryData
		{
			// Token: 0x170013B5 RID: 5045
			// (get) Token: 0x06005933 RID: 22835 RVA: 0x00183BF7 File Offset: 0x00181DF7
			// (set) Token: 0x06005934 RID: 22836 RVA: 0x00183BFF File Offset: 0x00181DFF
			[SaveableProperty(202)]
			public CharacterObject TroopType { get; private set; }

			// Token: 0x170013B6 RID: 5046
			// (get) Token: 0x06005935 RID: 22837 RVA: 0x00183C08 File Offset: 0x00181E08
			// (set) Token: 0x06005936 RID: 22838 RVA: 0x00183C10 File Offset: 0x00181E10
			[SaveableProperty(203)]
			public int Number { get; private set; }

			// Token: 0x06005937 RID: 22839 RVA: 0x00183C19 File Offset: 0x00181E19
			public TownMercenaryData(Town currentTown)
			{
				this._currentTown = currentTown;
			}

			// Token: 0x06005938 RID: 22840 RVA: 0x00183C28 File Offset: 0x00181E28
			public void ChangeMercenaryType(CharacterObject troopType, int number)
			{
				if (troopType != this.TroopType)
				{
					CharacterObject troopType2 = this.TroopType;
					this.TroopType = troopType;
					this.Number = number;
					CampaignEventDispatcher.Instance.OnMercenaryTroopChangedInTown(this._currentTown, troopType2, this.TroopType);
					return;
				}
				if (this.Number != number)
				{
					int difference = number - this.Number;
					this.ChangeMercenaryCount(difference);
				}
			}

			// Token: 0x06005939 RID: 22841 RVA: 0x00183C84 File Offset: 0x00181E84
			public void ChangeMercenaryCount(int difference)
			{
				if (difference != 0)
				{
					int number = this.Number;
					this.Number += difference;
					CampaignEventDispatcher.Instance.OnMercenaryNumberChangedInTown(this._currentTown, number, this.Number);
				}
			}

			// Token: 0x0600593A RID: 22842 RVA: 0x00183CC0 File Offset: 0x00181EC0
			public bool HasAvailableMercenary(Occupation occupation = Occupation.NotAssigned)
			{
				return this.TroopType != null && this.Number > 0 && (occupation == Occupation.NotAssigned || this.TroopType.Occupation == occupation);
			}

			// Token: 0x0600593B RID: 22843 RVA: 0x00183CE8 File Offset: 0x00181EE8
			internal static void AutoGeneratedStaticCollectObjectsTownMercenaryData(object o, List<object> collectedObjects)
			{
				((RecruitmentCampaignBehavior.TownMercenaryData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x0600593C RID: 22844 RVA: 0x00183CF6 File Offset: 0x00181EF6
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this._currentTown);
				collectedObjects.Add(this.TroopType);
			}

			// Token: 0x0600593D RID: 22845 RVA: 0x00183D10 File Offset: 0x00181F10
			internal static object AutoGeneratedGetMemberValueTroopType(object o)
			{
				return ((RecruitmentCampaignBehavior.TownMercenaryData)o).TroopType;
			}

			// Token: 0x0600593E RID: 22846 RVA: 0x00183D1D File Offset: 0x00181F1D
			internal static object AutoGeneratedGetMemberValueNumber(object o)
			{
				return ((RecruitmentCampaignBehavior.TownMercenaryData)o).Number;
			}

			// Token: 0x0600593F RID: 22847 RVA: 0x00183D2F File Offset: 0x00181F2F
			internal static object AutoGeneratedGetMemberValue_currentTown(object o)
			{
				return ((RecruitmentCampaignBehavior.TownMercenaryData)o)._currentTown;
			}

			// Token: 0x04001E14 RID: 7700
			[SaveableField(204)]
			private readonly Town _currentTown;
		}

		// Token: 0x02000729 RID: 1833
		public enum RecruitingDetail
		{
			// Token: 0x04001E16 RID: 7702
			MercenaryFromTavern,
			// Token: 0x04001E17 RID: 7703
			VolunteerFromIndividual,
			// Token: 0x04001E18 RID: 7704
			VolunteerFromIndividualToGarrison,
			// Token: 0x04001E19 RID: 7705
			VolunteerFromMap
		}
	}
}
