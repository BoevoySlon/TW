using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200037D RID: 893
	public class CaravansCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06003484 RID: 13444 RVA: 0x000DD0F4 File Offset: 0x000DB2F4
		private float DistanceScoreDivider
		{
			get
			{
				return (636f + 11.36f * Campaign.AverageDistanceBetweenTwoFortifications) / 2f;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x000DD10D File Offset: 0x000DB30D
		private float DistanceLimitVeryFar
		{
			get
			{
				return (508f + 9f * Campaign.AverageDistanceBetweenTwoFortifications) / 2f;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06003486 RID: 13446 RVA: 0x000DD126 File Offset: 0x000DB326
		private float DistanceLimitFar
		{
			get
			{
				return (381f + 6.75f * Campaign.AverageDistanceBetweenTwoFortifications) / 2f;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x000DD13F File Offset: 0x000DB33F
		private float DistanceLimitMedium
		{
			get
			{
				return (254f + 4.5f * Campaign.AverageDistanceBetweenTwoFortifications) / 2f;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06003488 RID: 13448 RVA: 0x000DD158 File Offset: 0x000DB358
		private float DistanceLimitClose
		{
			get
			{
				return (127f + 2.25f * Campaign.AverageDistanceBetweenTwoFortifications) / 2f;
			}
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x000DD174 File Offset: 0x000DB374
		public CaravansCampaignBehavior()
		{
			this._tradeActionLogPool = new CaravansCampaignBehavior.TradeActionLogPool(4096);
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x000DD20C File Offset: 0x000DB40C
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUpEvent));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEndEvent));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.DistributeLootToPartyEvent.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, Dictionary<PartyBase, ItemRoster>>(this.OnLootCaravanParties));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000DD344 File Offset: 0x000DB544
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			for (int i = 0; i < siegeEvent.BesiegedSettlement.Parties.Count; i++)
			{
				if (siegeEvent.BesiegedSettlement.Parties[i].IsCaravan)
				{
					siegeEvent.BesiegedSettlement.Parties[i].Ai.SetMoveModeHold();
				}
			}
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x000DD3A0 File Offset: 0x000DB5A0
		private void OnLootCaravanParties(MapEvent mapEvent, PartyBase party, Dictionary<PartyBase, ItemRoster> loot)
		{
			foreach (PartyBase partyBase in loot.Keys)
			{
				if (partyBase.IsMobile && partyBase.MobileParty.IsCaravan && party.IsMobile)
				{
					SkillLevelingManager.OnLoot(party.MobileParty, partyBase.MobileParty, loot[partyBase], true);
				}
			}
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x000DD424 File Offset: 0x000DB624
		public void OnNewGameCreatedPartialFollowUpEvent(CampaignGameStarter starter, int i)
		{
			List<Hero> list = new List<Hero>();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.Clan != Clan.PlayerClan && this.ShouldHaveCaravan(hero))
				{
					list.Add(hero);
				}
			}
			int count = list.Count;
			int num = count / 100 + ((count % 100 > i) ? 1 : 0);
			int num2 = count / 100 * i;
			for (int j = 0; j < i; j++)
			{
				num2 += ((count % 100 > j) ? 1 : 0);
			}
			for (int k = 0; k < num; k++)
			{
				this.SpawnCaravan(list[num2 + k], true);
			}
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x000DD4F8 File Offset: 0x000DB6F8
		private void OnNewGameCreatedPartialFollowUpEndEvent(CampaignGameStarter obj)
		{
			for (int i = 0; i < 2; i++)
			{
				this.UpdateAverageValues();
				this.DoInitialTradeRuns();
			}
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x000DD520 File Offset: 0x000DB720
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_tradeRumorTakenCaravans", ref this._tradeRumorTakenCaravans);
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_lootedCaravans", ref this._lootedCaravans);
			dataStore.SyncData<Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction>>("_interactedCaravans", ref this._interactedCaravans);
			dataStore.SyncData<Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>>>("_tradeActionLogs", ref this._tradeActionLogs);
			dataStore.SyncData<Dictionary<MobileParty, List<Settlement>>>("_previouslyChangedCaravanTargetsDueToEnemyOnWay", ref this._previouslyChangedCaravanTargetsDueToEnemyOnWay);
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x000DD588 File Offset: 0x000DB788
		private void DoInitialTradeRuns()
		{
			foreach (MobileParty mobileParty in MobileParty.AllCaravanParties)
			{
				Town town = null;
				Town town2 = null;
				float num = 0f;
				foreach (Town town3 in Town.AllTowns)
				{
					float num2 = mobileParty.Position2D.Distance(town3.Settlement.GatePosition);
					if (num2 > 1f)
					{
						num += 1f / MathF.Pow(num2, 1.5f);
					}
					else
					{
						town2 = town3;
					}
				}
				float num3 = MBRandom.RandomFloat * num;
				foreach (Town town4 in Town.AllTowns)
				{
					float num4 = mobileParty.Position2D.Distance(town4.Settlement.GatePosition);
					if (num4 > 1f)
					{
						num3 -= 1f / MathF.Pow(num4, 1.5f);
						if (num3 <= 0f)
						{
							town = town4;
							break;
						}
					}
				}
				if (town != null && town2 != null)
				{
					this.CreatePriceDataCache();
					if (MBRandom.RandomFloat < 0.5f)
					{
						this.SellGoods(mobileParty, town2, 0.7f, false);
						this.BuyGoods(mobileParty, town2);
						this.SellGoods(mobileParty, town, 0.7f, false);
					}
					else
					{
						this.SellGoods(mobileParty, town, 0.7f, false);
						this.BuyGoods(mobileParty, town);
						this.SellGoods(mobileParty, town2, 0.7f, false);
					}
				}
			}
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x000DD77C File Offset: 0x000DB97C
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
			this.UpdateAverageValues();
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x000DD78C File Offset: 0x000DB98C
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			foreach (PartyBase partyBase in mapEvent.InvolvedParties)
			{
				if (partyBase.IsMobile && partyBase.MobileParty.IsCaravan && mapEvent.IsWinnerSide(partyBase.Side))
				{
					MobileParty mobileParty = partyBase.MobileParty;
					int numberOfPackAnimals = mobileParty.ItemRoster.NumberOfPackAnimals;
					int numberOfLivestockAnimals = mobileParty.ItemRoster.NumberOfLivestockAnimals;
					int numberOfMounts = mobileParty.ItemRoster.NumberOfMounts;
					int totalManCount = mobileParty.MemberRoster.TotalManCount;
					if ((float)(numberOfPackAnimals + numberOfLivestockAnimals + numberOfMounts) > (float)totalManCount * 1.2f)
					{
						int num2;
						for (int i = numberOfPackAnimals + numberOfLivestockAnimals + numberOfMounts; i > totalManCount; i -= num2)
						{
							int num = 10000;
							ItemRosterElement itemRosterElement = partyBase.MobileParty.ItemRoster[0];
							foreach (ItemRosterElement itemRosterElement2 in partyBase.MobileParty.ItemRoster)
							{
								if (itemRosterElement2.EquipmentElement.Item.IsMountable || itemRosterElement2.EquipmentElement.Item.ItemCategory == DefaultItemCategories.PackAnimal || itemRosterElement2.EquipmentElement.Item.IsAnimal)
								{
									int itemValue = itemRosterElement2.EquipmentElement.ItemValue;
									if (itemValue < num)
									{
										num = itemValue;
										itemRosterElement = itemRosterElement2;
									}
								}
							}
							num2 = MathF.Min(itemRosterElement.Amount, MathF.Max(1, i - totalManCount));
							mobileParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num2);
						}
					}
					int inventoryCapacity = mobileParty.InventoryCapacity;
					float totalWeight = mobileParty.ItemRoster.TotalWeight;
					float num3 = 0f;
					if (totalWeight - num3 > (float)inventoryCapacity)
					{
						while (totalWeight - num3 > (float)inventoryCapacity)
						{
							int num4 = 10000;
							ItemRosterElement itemRosterElement3 = partyBase.MobileParty.ItemRoster[0];
							foreach (ItemRosterElement itemRosterElement4 in partyBase.MobileParty.ItemRoster)
							{
								if (!itemRosterElement4.EquipmentElement.Item.IsMountable)
								{
									int itemValue2 = itemRosterElement4.EquipmentElement.ItemValue;
									if (itemValue2 < num4)
									{
										num4 = itemValue2;
										itemRosterElement3 = itemRosterElement4;
									}
								}
							}
							int val = MathF.Ceiling((totalWeight - num3 - (float)inventoryCapacity) / itemRosterElement3.EquipmentElement.Weight);
							int num5 = Math.Max(1, Math.Min(itemRosterElement3.Amount, val));
							float weight = itemRosterElement3.EquipmentElement.Weight;
							mobileParty.ItemRoster.AddToCounts(itemRosterElement3.EquipmentElement, -num5);
							num3 += weight * (float)num5;
						}
					}
				}
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x000DDAC0 File Offset: 0x000DBCC0
		public bool ShouldHaveCaravan(Hero hero)
		{
			return hero.PartyBelongedTo == null && hero.IsMerchant && (hero.IsFugitive || hero.IsReleased || hero.IsNotSpawned || hero.IsActive) && !hero.IsTemplate && hero.CanLeadParty();
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x000DDB10 File Offset: 0x000DBD10
		public void SpawnCaravan(Hero hero, bool initialSpawn = false)
		{
			if (hero.OwnedCaravans.Count <= 0)
			{
				Settlement settlement = hero.HomeSettlement ?? hero.BornSettlement;
				Settlement spawnSettlement;
				if (settlement == null)
				{
					spawnSettlement = Town.AllTowns.GetRandomElement<Town>().Settlement;
				}
				else if (settlement.IsTown)
				{
					spawnSettlement = settlement;
				}
				else if (settlement.IsVillage)
				{
					spawnSettlement = (settlement.Village.TradeBound ?? Town.AllTowns.GetRandomElement<Town>().Settlement);
				}
				else
				{
					spawnSettlement = Town.AllTowns.GetRandomElement<Town>().Settlement;
				}
				bool isElite = false;
				if (hero.Power >= 112f)
				{
					float num = hero.Power * 0.0045f - 0.5f;
					isElite = (hero.RandomFloat() < num);
				}
				CaravanPartyComponent.CreateCaravanParty(hero, spawnSettlement, initialSpawn, null, null, 0, isElite);
				if (!initialSpawn && hero.Power >= 50f)
				{
					hero.AddPower(-30f);
				}
			}
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000DDBF0 File Offset: 0x000DBDF0
		private void UpdateAverageValues()
		{
			Dictionary<ItemCategory, ValueTuple<float, int>> dictionary = new Dictionary<ItemCategory, ValueTuple<float, int>>();
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.IsReady)
				{
					ValueTuple<float, int> valueTuple;
					dictionary.TryGetValue(itemObject.ItemCategory, out valueTuple);
					dictionary[itemObject.ItemCategory] = new ValueTuple<float, int>(valueTuple.Item1 + (float)MathF.Min(500, itemObject.Value), valueTuple.Item2 + 1);
				}
			}
			this._packAnimalCategoryIndex = -1;
			for (int i = 0; i < ItemCategories.All.Count; i++)
			{
				ItemCategory itemCategory = ItemCategories.All[i];
				ValueTuple<float, int> valueTuple2;
				bool flag = dictionary.TryGetValue(itemCategory, out valueTuple2);
				this._averageValuesCached[itemCategory] = (flag ? (valueTuple2.Item1 / (float)valueTuple2.Item2) : 1f);
				if (itemCategory == DefaultItemCategories.PackAnimal)
				{
					this._packAnimalCategoryIndex = i;
				}
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000DDCFC File Offset: 0x000DBEFC
		private void CreatePriceDataCache()
		{
			foreach (ItemCategory itemCategory in ItemCategories.All)
			{
				float num = 0f;
				float num2 = 1000f;
				foreach (Town town in Town.AllTowns)
				{
					float itemCategoryPriceIndex = town.GetItemCategoryPriceIndex(itemCategory);
					num += itemCategoryPriceIndex;
					if (itemCategoryPriceIndex < num2)
					{
						num2 = itemCategoryPriceIndex;
					}
				}
				float averageBuySellPriceIndex = num / (float)Town.AllTowns.Count;
				this._priceDictionary[itemCategory] = new CaravansCampaignBehavior.PriceIndexData(averageBuySellPriceIndex, num2);
			}
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000DDDC8 File Offset: 0x000DBFC8
		public void DailyTick()
		{
			this.DeleteExpiredTradeRumorTakenCaravans();
			this.DeleteExpiredLootedCaravans();
			this.CreatePriceDataCache();
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000DDDDC File Offset: 0x000DBFDC
		private void DailyTickHero(Hero hero)
		{
			if (hero != Hero.MainHero && this.ShouldHaveCaravan(hero))
			{
				this.SpawnCaravan(hero, false);
			}
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000DDDF8 File Offset: 0x000DBFF8
		private void DeleteExpiredTradeRumorTakenCaravans()
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (KeyValuePair<MobileParty, CampaignTime> keyValuePair in this._tradeRumorTakenCaravans)
			{
				if (CampaignTime.Now - keyValuePair.Value >= CampaignTime.Days(1f))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (MobileParty key in list)
			{
				this._tradeRumorTakenCaravans.Remove(key);
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000DDEC0 File Offset: 0x000DC0C0
		private void DeleteExpiredLootedCaravans()
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (KeyValuePair<MobileParty, CampaignTime> keyValuePair in this._lootedCaravans)
			{
				if (CampaignTime.Now - keyValuePair.Value >= CampaignTime.Days(10f))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (MobileParty key in list)
			{
				this._lootedCaravans.Remove(key);
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000DDF88 File Offset: 0x000DC188
		private Town GetDestinationForMobileParty(MobileParty party)
		{
			Settlement targetSettlement = party.TargetSettlement;
			if (targetSettlement == null)
			{
				return null;
			}
			return targetSettlement.Town;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000DDF9C File Offset: 0x000DC19C
		public void HourlyTickParty(MobileParty caravanParty)
		{
			if (!Campaign.Current.GameStarted)
			{
				return;
			}
			if (caravanParty.IsCaravan)
			{
				bool flag = false;
				float randomFloat = MBRandom.RandomFloat;
				if (caravanParty.MapEvent == null && caravanParty.IsPartyTradeActive && !caravanParty.Ai.DoNotMakeNewDecisions)
				{
					if (caravanParty.CurrentSettlement != null && caravanParty.CurrentSettlement.IsTown)
					{
						if (!caravanParty.CurrentSettlement.IsUnderSiege && caravanParty.ShortTermBehavior != AiBehavior.FleeToPoint && !caravanParty.Ai.IsAlerted && (caravanParty.IsCurrentlyUsedByAQuest || randomFloat < 0.33333334f))
						{
							float num = (caravanParty.MemberRoster.TotalManCount > 0) ? ((float)caravanParty.MemberRoster.TotalWounded / (float)caravanParty.MemberRoster.TotalManCount) : 1f;
							float num2 = 1f;
							if ((double)num > 0.4)
							{
								num2 = 0f;
							}
							else if ((double)num > 0.2)
							{
								num2 = 0.1f;
							}
							else if ((double)num > 0.1)
							{
								num2 = 0.2f;
							}
							else if ((double)num > 0.05)
							{
								num2 = 0.3f;
							}
							else if ((double)num > 0.025)
							{
								num2 = 0.4f;
							}
							float randomFloat2 = MBRandom.RandomFloat;
							if (num2 > randomFloat2)
							{
								flag = true;
							}
						}
					}
					else
					{
						Town destinationForMobileParty = this.GetDestinationForMobileParty(caravanParty);
						flag = (destinationForMobileParty == null || destinationForMobileParty.IsUnderSiege || caravanParty.MapFaction.IsAtWarWith(destinationForMobileParty.MapFaction) || caravanParty.Ai.NeedTargetReset || (!caravanParty.IsCurrentlyUsedByAQuest && randomFloat < 0.01f));
					}
					if (flag)
					{
						if (caravanParty.CurrentSettlement != null && caravanParty.CurrentSettlement.IsTown)
						{
							Town town = caravanParty.CurrentSettlement.Town;
							this.BuyGoods(caravanParty, town);
						}
						if (!this._previouslyChangedCaravanTargetsDueToEnemyOnWay.ContainsKey(caravanParty))
						{
							this._previouslyChangedCaravanTargetsDueToEnemyOnWay.Add(caravanParty, new List<Settlement>());
						}
						if (caravanParty.Ai.NeedTargetReset && caravanParty.TargetSettlement != null)
						{
							this._previouslyChangedCaravanTargetsDueToEnemyOnWay[caravanParty].Add(caravanParty.TargetSettlement);
						}
						Town town2 = this.ThinkNextDestination(caravanParty);
						if (town2 != null)
						{
							caravanParty.Ai.SetMoveGoToSettlement(town2.Settlement);
						}
					}
					Town destinationForMobileParty2 = this.GetDestinationForMobileParty(caravanParty);
					if (caravanParty.CurrentSettlement == null && destinationForMobileParty2 != null && caravanParty.TargetSettlement != destinationForMobileParty2.Settlement)
					{
						caravanParty.Ai.SetMoveGoToSettlement(destinationForMobileParty2.Settlement);
					}
				}
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000DE218 File Offset: 0x000DC418
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			Town town = settlement.Town;
			if (Campaign.Current.GameStarted && mobileParty != null && town != null && mobileParty.IsCaravan && mobileParty.IsPartyTradeActive && mobileParty.IsActive)
			{
				if (this._previouslyChangedCaravanTargetsDueToEnemyOnWay.ContainsKey(mobileParty))
				{
					this._previouslyChangedCaravanTargetsDueToEnemyOnWay[mobileParty].Clear();
				}
				else
				{
					this._previouslyChangedCaravanTargetsDueToEnemyOnWay.Add(mobileParty, new List<Settlement>());
				}
				if (Campaign.Current.GameStarted)
				{
					List<CaravansCampaignBehavior.TradeActionLog> list;
					if (this._tradeActionLogs.TryGetValue(mobileParty, out list))
					{
						for (int i = list.Count - 1; i >= 0; i--)
						{
							CaravansCampaignBehavior.TradeActionLog tradeActionLog = list[i];
							if (tradeActionLog.BoughtTime.ElapsedDaysUntilNow > 7f)
							{
								list.RemoveAt(i);
								this._tradeActionLogPool.ReleaseLog(tradeActionLog);
							}
						}
					}
					this.SellGoods(mobileParty, town, 1.1f, false);
				}
				if (mobileParty.HomeSettlement == settlement)
				{
					this._caravanLastHomeTownVisitTime[mobileParty] = CampaignTime.Now;
				}
			}
			if (mobileParty != null && mobileParty.IsCaravan && settlement.IsTown && settlement.Town.Governor != null && settlement.Town.Governor.GetPerkValue(DefaultPerks.Trade.Tollgates))
			{
				settlement.Town.TradeTaxAccumulated += MathF.Round(DefaultPerks.Trade.Tollgates.SecondaryBonus);
			}
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000DE378 File Offset: 0x000DC578
		public void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty != null && mobileParty != MobileParty.MainParty && (mobileParty.IsCaravan || mobileParty.IsLordParty))
			{
				int inventoryCapacity = mobileParty.InventoryCapacity;
				float totalWeight = mobileParty.ItemRoster.TotalWeight;
				Town town = settlement.IsTown ? settlement.Town : (settlement.IsVillage ? settlement.Village.Bound.Town : null);
				if (town != null)
				{
					float num = 1.1f;
					while (totalWeight > (float)inventoryCapacity)
					{
						this.SellGoods(mobileParty, town, num, true);
						num -= 0.02f;
						if (num < 0.75f)
						{
							break;
						}
						inventoryCapacity = mobileParty.InventoryCapacity;
						totalWeight = mobileParty.ItemRoster.TotalWeight;
					}
				}
			}
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000DE424 File Offset: 0x000DC624
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._interactedCaravans.ContainsKey(mobileParty))
			{
				this._interactedCaravans.Remove(mobileParty);
			}
			List<CaravansCampaignBehavior.TradeActionLog> list;
			if (this._tradeActionLogs.TryGetValue(mobileParty, out list))
			{
				this._tradeActionLogs.Remove(mobileParty);
				for (int i = 0; i < list.Count; i++)
				{
					CaravansCampaignBehavior.TradeActionLog log = list[i];
					this._tradeActionLogPool.ReleaseLog(log);
				}
			}
			if (this._previouslyChangedCaravanTargetsDueToEnemyOnWay.ContainsKey(mobileParty))
			{
				this._previouslyChangedCaravanTargetsDueToEnemyOnWay.Remove(mobileParty);
			}
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x000DE4A9 File Offset: 0x000DC6A9
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			if (mobileParty.IsCaravan)
			{
				this._previouslyChangedCaravanTargetsDueToEnemyOnWay.Add(mobileParty, new List<Settlement>());
			}
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000DE4C4 File Offset: 0x000DC6C4
		private Town ThinkNextDestination(MobileParty caravanParty)
		{
			this.RefreshTotalValueOfItemsAtCategoryForParty(caravanParty);
			return this.FindNextDestinationForCaravan(caravanParty, true) ?? this.FindNextDestinationForCaravan(caravanParty, false);
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000DE4E4 File Offset: 0x000DC6E4
		private Town FindNextDestinationForCaravan(MobileParty caravanParty, bool distanceCut)
		{
			float num = 0f;
			Town result = null;
			float caravanFullness = caravanParty.ItemRoster.TotalWeight / (float)caravanParty.InventoryCapacity;
			CampaignTime lastHomeVisitTimeOfCaravan;
			this._caravanLastHomeTownVisitTime.TryGetValue(caravanParty, out lastHomeVisitTimeOfCaravan);
			foreach (Town town in Town.AllTowns)
			{
				if (town.Owner.Settlement != caravanParty.CurrentSettlement && !town.IsUnderSiege && !town.MapFaction.IsAtWarWith(caravanParty.MapFaction) && (!town.Settlement.Parties.Contains(MobileParty.MainParty) || !MobileParty.MainParty.MapFaction.IsAtWarWith(caravanParty.MapFaction)) && !this._previouslyChangedCaravanTargetsDueToEnemyOnWay[caravanParty].Contains(town.Settlement))
				{
					float tradeScoreForTown = this.GetTradeScoreForTown(caravanParty, town, lastHomeVisitTimeOfCaravan, caravanFullness, distanceCut);
					if (tradeScoreForTown > num)
					{
						num = tradeScoreForTown;
						result = town;
					}
				}
			}
			return result;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000DE5F8 File Offset: 0x000DC7F8
		private void AdjustVeryFarAddition(float distance, float minimumAddition, ref float veryFarAddition)
		{
			if (distance > this.DistanceLimitVeryFar)
			{
				veryFarAddition += (distance - this.DistanceLimitVeryFar) * minimumAddition * 4f;
			}
			if (distance > this.DistanceLimitFar)
			{
				veryFarAddition += (distance - this.DistanceLimitFar) * minimumAddition * 3f;
			}
			if (distance > this.DistanceLimitMedium)
			{
				veryFarAddition += (distance - this.DistanceLimitMedium) * minimumAddition * 2f;
			}
			if (distance > this.DistanceLimitClose)
			{
				veryFarAddition += (distance - this.DistanceLimitClose) * minimumAddition;
			}
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000DE678 File Offset: 0x000DC878
		private float GetTradeScoreForTown(MobileParty caravanParty, Town town, CampaignTime lastHomeVisitTimeOfCaravan, float caravanFullness, bool distanceCut)
		{
			float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(caravanParty, town.Owner.Settlement);
			float num = 0f;
			this.AdjustVeryFarAddition(distance, 0.15f, ref num);
			float elapsedDaysUntilNow = lastHomeVisitTimeOfCaravan.ElapsedDaysUntilNow;
			bool flag = elapsedDaysUntilNow > 2f;
			if (flag)
			{
				float distance2 = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Owner.Settlement, caravanParty.HomeSettlement);
				this.AdjustVeryFarAddition(distance2, ((elapsedDaysUntilNow - 1f) * MathF.Sqrt(elapsedDaysUntilNow - 1f) - 1f) * 0.008f, ref num);
			}
			float num2 = 1f / (distance + num + 8f);
			if (distanceCut && (town.Owner.Settlement != caravanParty.HomeSettlement || !flag) && num2 < 1f / this.DistanceScoreDivider)
			{
				return -1f;
			}
			float num3 = 1f;
			if (caravanParty.HomeSettlement == town.Owner.Settlement)
			{
				num3 = 1f + elapsedDaysUntilNow * 0.1f * (elapsedDaysUntilNow * 0.1f);
			}
			TownMarketData marketData = town.MarketData;
			float num4 = 0f;
			for (int i = 0; i < caravanParty.Party.ItemRoster.Count; i++)
			{
				ItemObject item = caravanParty.ItemRoster.GetElementCopyAtIndex(i).EquipmentElement.Item;
				float limitValue = 1.1f - MathF.Sqrt((float)MathF.Min(this._totalValueOfItemsAtCategory[item.ItemCategory], 5000) / 5000f) * 0.2f;
				num4 += this.CalculateTownSellScoreForCategory(caravanParty, marketData, i, limitValue);
			}
			num4 *= 0.3f + caravanFullness;
			float num5 = 0f;
			for (int j = 0; j < ItemCategories.All.Count; j++)
			{
				ItemCategory itemCategory = ItemCategories.All[j];
				if (itemCategory.IsTradeGood || itemCategory.IsAnimal)
				{
					num5 += this.CalculateTownBuyScoreForCategory(marketData, j);
				}
			}
			num5 *= MathF.Max(0.1f, 1f - (caravanFullness - 0.2f * MathF.Min(num4, 1000f) / 1000f));
			num5 = MathF.Min(num5, (float)((int)(0.5f * (float)caravanParty.PartyTradeGold)));
			float num6 = (caravanParty.Ai.NeedTargetReset && caravanParty.TargetSettlement == town.Settlement) ? 0.1f : 1f;
			float num7 = (caravanParty.IsCurrentlyUsedByAQuest && town.Settlement == caravanParty.HomeSettlement && caravanParty.Position2D.Distance(caravanParty.HomeSettlement.GatePosition) < 3f) ? 0.1f : 1f;
			return (num4 + num5) * num6 * num2 * num3 * num7;
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000DE950 File Offset: 0x000DCB50
		private float CalculateTownSellScoreForCategory(MobileParty party, TownMarketData marketData, int i, float limitValue)
		{
			ItemRosterElement itemRosterElement = party.Party.ItemRoster[i];
			ItemCategory itemCategory = itemRosterElement.EquipmentElement.Item.ItemCategory;
			CaravansCampaignBehavior.PriceIndexData categoryPriceData = this.GetCategoryPriceData(itemCategory);
			float num = marketData.GetPriceFactor(itemCategory) - categoryPriceData.AverageBuySellPriceIndex * limitValue;
			if (num > 0f)
			{
				int num2 = (itemRosterElement.EquipmentElement.Item.ItemCategory != DefaultItemCategories.PackAnimal) ? itemRosterElement.Amount : MathF.Max(0, itemRosterElement.Amount - party.MemberRoster.TotalManCount);
				float num3 = (itemCategory.Properties == ItemCategory.Property.BonusToFoodStores) ? 1.1f : 1f;
				return num * num3 * (float)MathF.Min(4000, itemRosterElement.EquipmentElement.Item.Value * num2);
			}
			return 0f;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000DEA2F File Offset: 0x000DCC2F
		private void SetPlayerInteraction(MobileParty mobileParty, CaravansCampaignBehavior.PlayerInteraction interaction)
		{
			if (this._interactedCaravans.ContainsKey(mobileParty))
			{
				this._interactedCaravans[mobileParty] = interaction;
				return;
			}
			this._interactedCaravans.Add(mobileParty, interaction);
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000DEA5C File Offset: 0x000DCC5C
		private CaravansCampaignBehavior.PlayerInteraction GetPlayerInteraction(MobileParty mobileParty)
		{
			CaravansCampaignBehavior.PlayerInteraction result;
			if (this._interactedCaravans.TryGetValue(mobileParty, out result))
			{
				return result;
			}
			return CaravansCampaignBehavior.PlayerInteraction.None;
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000DEA7C File Offset: 0x000DCC7C
		private float CalculateTownBuyScoreForCategory(TownMarketData marketData, int categoryIndex)
		{
			ItemCategory itemCategory = ItemCategories.All[categoryIndex];
			ref CaravansCampaignBehavior.PriceIndexData categoryPriceData = this.GetCategoryPriceData(itemCategory);
			float priceFactor = marketData.GetPriceFactor(itemCategory);
			float num = categoryPriceData.AverageBuySellPriceIndex / priceFactor;
			float num2 = num * num - 1.1f;
			if (num2 > 0f)
			{
				return MathF.Min(MathF.Sqrt(this._averageValuesCached[itemCategory]) * 3f * num2, 0.3f * (float)marketData.GetCategoryData(itemCategory).InStoreValue);
			}
			return 0f;
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000DEAF4 File Offset: 0x000DCCF4
		private CaravansCampaignBehavior.PriceIndexData GetCategoryPriceData(ItemCategory category)
		{
			CaravansCampaignBehavior.PriceIndexData result;
			if (!this._priceDictionary.TryGetValue(category, out result))
			{
				return new CaravansCampaignBehavior.PriceIndexData(1f, 1f);
			}
			return result;
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000DEB24 File Offset: 0x000DCD24
		private void RefreshTotalValueOfItemsAtCategoryForParty(MobileParty caravanParty)
		{
			this._totalValueOfItemsAtCategory.Clear();
			for (int i = 0; i < caravanParty.ItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = caravanParty.ItemRoster.GetElementCopyAtIndex(i);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				int num = elementCopyAtIndex.Amount * (item.Value + 10);
				int num2;
				if (this._totalValueOfItemsAtCategory.TryGetValue(item.ItemCategory, out num2))
				{
					this._totalValueOfItemsAtCategory[item.ItemCategory] = num2 + num;
				}
				else
				{
					this._totalValueOfItemsAtCategory.Add(item.ItemCategory, num);
				}
			}
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000DEBC0 File Offset: 0x000DCDC0
		private void SellGoods(MobileParty caravanParty, Town town, float priceIndexSellLimit = 1.1f, bool toLoseWeight = false)
		{
			int gold = town.Gold;
			int num = (int)((float)caravanParty.ItemRoster.NumberOfPackAnimals - (float)caravanParty.Party.NumberOfAllMembers * 0.6f);
			int num2 = (int)((float)caravanParty.ItemRoster.NumberOfLivestockAnimals - (float)caravanParty.Party.NumberOfAllMembers * 0.6f);
			int itemAverageWeight = Campaign.Current.Models.InventoryCapacityModel.GetItemAverageWeight();
			this.RefreshTotalValueOfItemsAtCategoryForParty(caravanParty);
			List<ValueTuple<EquipmentElement, int>> list = new List<ValueTuple<EquipmentElement, int>>();
			for (int i = 0; i < 2; i++)
			{
				for (int j = caravanParty.ItemRoster.Count - 1; j >= 0; j--)
				{
					ItemRosterElement elementCopyAtIndex = caravanParty.ItemRoster.GetElementCopyAtIndex(j);
					ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
					CaravansCampaignBehavior.PriceIndexData priceIndexData;
					if (this._priceDictionary.TryGetValue(item.GetItemCategory(), out priceIndexData) && (i != 0 || (!item.HasHorseComponent && item.ItemCategory != DefaultItemCategories.PackAnimal)) && (i != 1 || item.HasHorseComponent || item.ItemCategory == DefaultItemCategories.PackAnimal) && (!toLoseWeight || !item.HasHorseComponent))
					{
						bool flag = item.ItemCategory == DefaultItemCategories.PackAnimal;
						if (!flag || num > 0)
						{
							bool flag2 = item.HorseComponent != null && item.HorseComponent.IsLiveStock;
							float priceFactor = town.MarketData.GetPriceFactor(elementCopyAtIndex.EquipmentElement.Item.GetItemCategory());
							float demand = town.MarketData.GetDemand(elementCopyAtIndex.EquipmentElement.Item.GetItemCategory());
							float num3 = priceFactor / priceIndexData.AverageBuySellPriceIndex;
							float num4 = Campaign.Current.GameStarted ? (MathF.Sqrt((float)MathF.Min(this._totalValueOfItemsAtCategory[item.ItemCategory], 5000) / 5000f) * 0.4f) : 0f;
							float num5 = priceIndexSellLimit - num4;
							if (num3 >= num5 || (num2 > 0 && flag2) || (num > 0 && flag))
							{
								float num6 = 0.8f * priceIndexData.AverageBuySellPriceIndex + 0.2f * priceIndexData.MinBuySellPriceIndex;
								if (priceFactor >= num6 * num5 || (num2 > 0 && flag2) || (num > 0 && flag))
								{
									float num7 = priceFactor - num6 * num5;
									float num8 = num7 * (float)item.Value;
									float num9 = num7 * 200f;
									float num10 = num8 + num9;
									int itemPrice = town.GetItemPrice(item, caravanParty, true);
									float num11 = (item.ItemCategory.Properties == ItemCategory.Property.BonusToFoodStores) ? 1.1f : 1f;
									float num12 = (item.ItemCategory == DefaultItemCategories.PackAnimal) ? 1.5f : 1f;
									float num13 = ((num3 > 1f) ? MathF.Pow(num3, 0.67f) : num3) * num10 * num11 * num12 * 3f;
									if (num13 > demand * 20f)
									{
										num13 = demand * 20f;
									}
									if (num13 > 0f || (num2 > 0 && flag2) || (num > 0 && flag))
									{
										int num14 = (num > 0 && flag) ? num : ((num2 > 0 && flag2) ? num2 : MBRandom.RoundRandomized(num13 / (float)itemPrice));
										int amount = elementCopyAtIndex.Amount;
										if (num14 > amount)
										{
											num14 = amount;
										}
										if (num14 * itemPrice > gold)
										{
											num14 = gold / itemPrice;
										}
										if (toLoseWeight && caravanParty.ItemRoster.TotalWeight - (float)(num14 * itemAverageWeight) < (float)caravanParty.InventoryCapacity)
										{
											num14 = (int)((caravanParty.ItemRoster.TotalWeight - (float)caravanParty.InventoryCapacity) / (float)itemAverageWeight + 0.99f);
										}
										if (num14 > elementCopyAtIndex.Amount)
										{
											num14 = elementCopyAtIndex.Amount;
										}
										if (num14 * itemPrice > gold)
										{
											num14 = gold / itemPrice;
										}
										if (num14 > 0)
										{
											list.Add(new ValueTuple<EquipmentElement, int>(elementCopyAtIndex.EquipmentElement, num14));
											if (Campaign.Current.GameStarted)
											{
												this.OnSellItems(caravanParty, elementCopyAtIndex, town);
											}
											SellItemsAction.Apply(caravanParty.Party, town.Owner, elementCopyAtIndex, num14, town.Owner.Settlement);
											num = (int)((float)caravanParty.ItemRoster.NumberOfPackAnimals - (float)caravanParty.Party.NumberOfAllMembers * 0.6f);
											num2 = (int)((float)caravanParty.ItemRoster.NumberOfLivestockAnimals - (float)caravanParty.Party.NumberOfAllMembers * 0.6f);
										}
									}
								}
							}
						}
					}
				}
			}
			if (!list.IsEmpty<ValueTuple<EquipmentElement, int>>() && caravanParty.IsCaravan)
			{
				CampaignEventDispatcher.Instance.OnCaravanTransactionCompleted(caravanParty, town, list);
			}
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000DF040 File Offset: 0x000DD240
		private void OnSellItems(MobileParty caravanParty, ItemRosterElement rosterElement, Town soldTown)
		{
			int itemPrice = soldTown.GetItemPrice(rosterElement.EquipmentElement.Item, caravanParty, true);
			List<CaravansCampaignBehavior.TradeActionLog> list;
			if (this._tradeActionLogs.TryGetValue(caravanParty, out list))
			{
				foreach (CaravansCampaignBehavior.TradeActionLog tradeActionLog in list)
				{
					if (tradeActionLog.ItemRosterElement.EquipmentElement.Item == rosterElement.EquipmentElement.Item && itemPrice > tradeActionLog.SellPrice)
					{
						tradeActionLog.OnSellAction(soldTown.Settlement, itemPrice);
					}
				}
			}
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000DF0F0 File Offset: 0x000DD2F0
		private void BuyGoods(MobileParty caravanParty, Town town)
		{
			CaravansCampaignBehavior.<>c__DisplayClass68_0 CS$<>8__locals1 = new CaravansCampaignBehavior.<>c__DisplayClass68_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.town = town;
			this.CaravanTotalValue(caravanParty);
			List<ValueTuple<EquipmentElement, int>> list = new List<ValueTuple<EquipmentElement, int>>();
			float capacityFactor = this.CalculateCapacityFactor(caravanParty);
			float budgetFactor = this.CalculateBudgetFactor(caravanParty);
			this.RefreshTotalValueOfItemsAtCategoryForParty(caravanParty);
			ValueTuple<ItemCategory, ItemCategory, ItemCategory, ItemCategory, ItemCategory> valueTuple = MBMath.MaxElements5<ItemCategory>(ItemCategories.All, (ItemCategory x) => CS$<>8__locals1.<>4__this.CalculateBuyValue(x, CS$<>8__locals1.town, budgetFactor, capacityFactor));
			ItemCategory item = valueTuple.Item1;
			ItemCategory item2 = valueTuple.Item2;
			ItemCategory item3 = valueTuple.Item3;
			ItemCategory item4 = valueTuple.Item4;
			ItemCategory item5 = valueTuple.Item5;
			if (item != null)
			{
				this.BuyCategory(caravanParty, CS$<>8__locals1.town, item, budgetFactor, capacityFactor, list);
			}
			if (item2 != null)
			{
				this.BuyCategory(caravanParty, CS$<>8__locals1.town, item2, budgetFactor, capacityFactor, list);
			}
			if (item3 != null)
			{
				this.BuyCategory(caravanParty, CS$<>8__locals1.town, item3, budgetFactor, capacityFactor, list);
			}
			if (item4 != null)
			{
				this.BuyCategory(caravanParty, CS$<>8__locals1.town, item4, budgetFactor, capacityFactor, list);
			}
			if (item5 != null)
			{
				this.BuyCategory(caravanParty, CS$<>8__locals1.town, item5, budgetFactor, capacityFactor, list);
			}
			if ((float)(caravanParty.ItemRoster.NumberOfPackAnimals + caravanParty.ItemRoster.NumberOfLivestockAnimals) < (float)caravanParty.Party.NumberOfAllMembers * 2f && caravanParty.ItemRoster.NumberOfPackAnimals < caravanParty.Party.NumberOfAllMembers && this._packAnimalCategoryIndex >= 0 && caravanParty.PartyTradeGold > 1000)
			{
				this.BuyCategory(caravanParty, CS$<>8__locals1.town, DefaultItemCategories.PackAnimal, budgetFactor, capacityFactor, list);
			}
			if (!list.IsEmpty<ValueTuple<EquipmentElement, int>>())
			{
				CampaignEventDispatcher.Instance.OnCaravanTransactionCompleted(caravanParty, CS$<>8__locals1.town, list);
			}
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000DF2DD File Offset: 0x000DD4DD
		private float CalculateBudgetFactor(MobileParty caravanParty)
		{
			return 0.1f + MathF.Clamp((float)caravanParty.PartyTradeGold / this.ReferenceBudgetValue, 0f, 1f);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000DF304 File Offset: 0x000DD504
		private float CalculateCapacityFactor(MobileParty caravanParty)
		{
			float value = caravanParty.Party.ItemRoster.TotalWeight / ((float)caravanParty.InventoryCapacity + 1f);
			return 1.1f - MathF.Clamp(value, 0f, 1f);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000DF348 File Offset: 0x000DD548
		private void BuyCategory(MobileParty caravanParty, Town town, ItemCategory category, float budgetFactor, float capacityFactor, List<ValueTuple<EquipmentElement, int>> boughtItems)
		{
			float num = this.CalculateBuyValue(category, town, budgetFactor, capacityFactor);
			if (num < 7f)
			{
				return;
			}
			if (caravanParty.TotalWeightCarried / (float)caravanParty.InventoryCapacity > 0.8f && !category.IsAnimal)
			{
				return;
			}
			if (town.MarketData.GetCategoryData(category).InStore == 0)
			{
				return;
			}
			float num2 = MathF.Min((float)caravanParty.PartyTradeGold * 0.5f, num * 1.5f);
			if (num2 > 1500f)
			{
				num2 = 1500f;
			}
			if (!Campaign.Current.GameStarted)
			{
				num2 *= 0.5f;
			}
			float num3 = num2;
			Predicate<ItemObject> <>9__0;
			for (;;)
			{
				int num4 = 0;
				int x2 = (int)(MBRandom.RandomFloat * (float)town.Owner.ItemRoster.Count);
				ItemRoster itemRoster = town.Owner.ItemRoster;
				Predicate<ItemObject> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ItemObject x) => x.ItemCategory == category));
				}
				int num5 = itemRoster.FindIndexFirstAfterXthElement(predicate, x2);
				if (num5 < 0)
				{
					break;
				}
				ItemRosterElement elementCopyAtIndex = town.Owner.ItemRoster.GetElementCopyAtIndex(num5);
				ItemObject item = elementCopyAtIndex.EquipmentElement.Item;
				int itemPrice = town.GetItemPrice(item, caravanParty, false);
				int num6 = MBRandom.RoundRandomized(num3 / (float)itemPrice);
				if (num6 > elementCopyAtIndex.Amount)
				{
					num6 = elementCopyAtIndex.Amount;
				}
				if (num6 > 100)
				{
					num6 = 100;
				}
				if (!category.IsAnimal && caravanParty.TotalWeightCarried + (float)num6 * item.Weight > (float)caravanParty.InventoryCapacity)
				{
					num6 = (int)(((float)caravanParty.InventoryCapacity * 0.8f - caravanParty.TotalWeightCarried) / item.Weight);
				}
				if (elementCopyAtIndex.EquipmentElement.Item.HorseComponent != null && (elementCopyAtIndex.EquipmentElement.Item.HorseComponent.IsLiveStock || elementCopyAtIndex.EquipmentElement.Item.HorseComponent.IsPackAnimal))
				{
					int numberOfPackAnimals = caravanParty.ItemRoster.NumberOfPackAnimals;
					int numberOfLivestockAnimals = caravanParty.ItemRoster.NumberOfLivestockAnimals;
					if (elementCopyAtIndex.EquipmentElement.Item.HorseComponent.IsLiveStock && (float)(numberOfLivestockAnimals + num6) > (float)caravanParty.Party.NumberOfAllMembers * 0.6f)
					{
						num6 = (int)((float)caravanParty.Party.NumberOfAllMembers * 0.6f) - numberOfLivestockAnimals;
					}
					else if (elementCopyAtIndex.EquipmentElement.Item.HorseComponent.IsPackAnimal && numberOfPackAnimals + num6 > caravanParty.Party.NumberOfAllMembers)
					{
						num6 = caravanParty.Party.NumberOfAllMembers - numberOfPackAnimals;
					}
				}
				if (num6 > 0)
				{
					SellItemsAction.Apply(town.Owner, caravanParty.Party, elementCopyAtIndex, num6, town.Owner.Settlement);
					boughtItems.Add(new ValueTuple<EquipmentElement, int>(elementCopyAtIndex.EquipmentElement, -num6));
					num4 = num6;
					num3 -= (float)((num6 + 1) * itemPrice);
					Town destinationForMobileParty = this.GetDestinationForMobileParty(caravanParty);
					if (caravanParty.LastVisitedSettlement != null && destinationForMobileParty != null && Campaign.Current.GameStarted)
					{
						List<CaravansCampaignBehavior.TradeActionLog> list;
						if (!this._tradeActionLogs.TryGetValue(caravanParty, out list))
						{
							list = new List<CaravansCampaignBehavior.TradeActionLog>();
							this._tradeActionLogs.Add(caravanParty, list);
						}
						int itemPrice2 = town.GetItemPrice(elementCopyAtIndex.EquipmentElement, caravanParty, false);
						list.Add(this._tradeActionLogPool.CreateNewLog(town.Settlement, itemPrice2, elementCopyAtIndex));
					}
				}
				if (num3 <= 0f || num4 <= 0 || num4 >= 100)
				{
					return;
				}
			}
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000DF6C8 File Offset: 0x000DD8C8
		private int CaravanTotalValue(MobileParty caravanParty)
		{
			float num = 0f;
			for (int i = 0; i < caravanParty.ItemRoster.Count; i++)
			{
				ItemRosterElement itemRosterElement = caravanParty.ItemRoster[i];
				num += this.GetGlobalItemSellPrice(itemRosterElement.EquipmentElement.Item) * (float)itemRosterElement.Amount;
			}
			return (int)num + caravanParty.PartyTradeGold;
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000DF728 File Offset: 0x000DD928
		private float CalculateBuyValue(ItemCategory category, Town town, float budgetFactor, float capacityFactor)
		{
			if (!category.IsTradeGood && !category.IsAnimal)
			{
				return 0f;
			}
			CaravansCampaignBehavior.PriceIndexData priceIndexData;
			if (!this._priceDictionary.TryGetValue(category, out priceIndexData))
			{
				return 0f;
			}
			if (town.MarketData.GetItemCountOfCategory(category) == 0)
			{
				return 0f;
			}
			float num = 0f;
			if (Campaign.Current.GameStarted && this._totalValueOfItemsAtCategory.ContainsKey(category))
			{
				num = MathF.Sqrt((float)MathF.Min(this._totalValueOfItemsAtCategory[category], 5000) / 5000f) * 0.4f;
			}
			float itemCategoryPriceIndex = town.GetItemCategoryPriceIndex(category);
			float averageBuySellPriceIndex = priceIndexData.AverageBuySellPriceIndex;
			float num2 = averageBuySellPriceIndex * (1f - num) - itemCategoryPriceIndex;
			float demand = town.MarketData.GetDemand(category);
			float num3 = 0.1f * MathF.Pow(demand, 0.5f);
			if (num2 < 0f)
			{
				return 0f;
			}
			float num4 = num2 * this._averageValuesCached[category];
			float num5 = num2 * 200f;
			float num6 = averageBuySellPriceIndex / itemCategoryPriceIndex;
			float num7 = (category.Properties == ItemCategory.Property.BonusToFoodStores) ? 1.1f : 1f;
			return ((category == DefaultItemCategories.PackAnimal) ? 1.5f : 1f) * num7 * num6 * num3 * (num4 * budgetFactor + num5 * capacityFactor);
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000DF86C File Offset: 0x000DDA6C
		private float GetGlobalItemSellPrice(ItemObject item)
		{
			CaravansCampaignBehavior.PriceIndexData priceIndexData;
			if (!this._priceDictionary.TryGetValue(item.ItemCategory, out priceIndexData))
			{
				return 1f;
			}
			return priceIndexData.AverageBuySellPriceIndex * (float)item.Value;
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x000DF8A4 File Offset: 0x000DDAA4
		protected void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddPlayerLine("caravan_companion_talk_start", "hero_main_options", "caravan_companion_talk_start", "{=q0RY0dQG}We need to talk business.", new ConversationSentence.OnConditionDelegate(this.companion_is_caravan_leader_on_condition), null, 100, null, null);
			starter.AddDialogLine("caravan_companion_talk_start_reply", "caravan_companion_talk_start", "caravan_companion_talk_start_reply", "{=9RiXgPc1}Certainly. What do you need to know?", null, null, 100, null);
			starter.AddPlayerLine("caravan_companion_trade_rumors", "caravan_companion_talk_start_reply", "caravan_companion_ask_trade_rumors", "{=oMuxr3X6}What news of the markets? Any good deals to be had?", null, null, 100, null, null);
			starter.AddDialogLine("caravan_companion_ask_trade_rumors", "caravan_companion_ask_trade_rumors", "caravan_companion_anything_else", "{=sC4ZLZ8x}{COMMENT}", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_ask_trade_rumors_on_consequence), 100, null);
			starter.AddDialogLine("caravan_companion_talk_player_thank", "caravan_companion_anything_else", "caravan_companion_talk_end", "{=DQBaaC0e}Is there anything else?", null, null, 100, null);
			starter.AddPlayerLine("caravan_companion_talk_not_leave", "caravan_companion_talk_end", "lord_pretalk", "{=i2FwKPmC}Yes, I wanted to talk about something else..", null, null, 100, null, null);
			starter.AddPlayerLine("caravan_companion_talk_leave", "caravan_companion_talk_end", "close_window", "{=1IJouNaM}Carry on, then. Farewell.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_player_leave_encounter_on_consequence), 100, null, null);
			starter.AddPlayerLine("caravan_companion_nevermind", "caravan_companion_talk_start_reply", "lord_pretalk", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			starter.AddDialogLine("player_caravan_talk_start", "start", "player_caravan_talk_start", "{=BsVXQEhj}How may I help you?", new ConversationSentence.OnConditionDelegate(this.player_caravan_talk_start_on_condition), null, 100, null);
			starter.AddPlayerLine("player_caravan_trade_rumors", "player_caravan_talk_start", "player_caravan_ask_trade_rumors", "{=shNl2Npf}What news of the markets?", null, null, 100, null, null);
			starter.AddDialogLine("player_caravan_ask_trade_rumors", "player_caravan_ask_trade_rumors", "player_caravan_anything_else", "{=sC4ZLZ8x}{COMMENT}", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_ask_trade_rumors_on_consequence), 100, null);
			starter.AddDialogLine("player_caravan_talk_player_thank", "player_caravan_anything_else", "player_caravan_talk_end", "{=DQBaaC0e}Is there anything else?", null, null, 100, null);
			starter.AddPlayerLine("player_caravan_talk_not_leave", "player_caravan_talk_end", "start", "{=i2FwKPmC}Yes, I wanted to talk about something else..", null, null, 100, null, null);
			starter.AddPlayerLine("player_caravan_talk_leave", "player_caravan_talk_end", "close_window", "{=1IJouNaM}Carry on, then. Farewell.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_player_leave_encounter_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_caravan_nevermind", "player_caravan_talk_start", "close_window", "{=D33fIGQe}Never mind.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_player_leave_encounter_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_hero_leader_talk_start", "start", "caravan_talk", "{=!}{CARAVAN_GREETING}", new ConversationSentence.OnConditionDelegate(this.caravan_start_talk_on_condition), null, 100, null);
			starter.AddDialogLine("caravan_pretalk", "caravan_pretalk", "caravan_talk", "{=3cBfSJOI}Is there anything else?[ib:normal]", null, null, 100, null);
			starter.AddPlayerLine("caravan_buy_products", "caravan_talk", "caravan_player_trade", "{=t0UGXPV4}I'm interested in trading. What kind of products do you have?", new ConversationSentence.OnConditionDelegate(this.caravan_buy_products_on_condition), null, 100, null, null);
			starter.AddPlayerLine("caravan_trade_rumors", "caravan_talk", "caravan_ask_trade_rumors", "{=b5Ucatkb}Tell me about your journeys. What news of the markets?", null, null, 100, null, null);
			starter.AddDialogLine("caravan_ask_trade_rumors", "caravan_ask_trade_rumors", "caravan_trade_rumors_player_answer", "{=sC4ZLZ8x}{COMMENT}", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_ask_trade_rumors_on_consequence), 100, null);
			starter.AddPlayerLine("caravan_trade_rumors_player_answer", "caravan_trade_rumors_player_answer", "caravan_talk_player_thank", "{=ha7EmrU9}Thank you for that information.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_talk_player_thank", "caravan_talk_player_thank", "caravan_talk", "{=BQuVWKvq}You're welcome. Is there anything we need to discuss?", null, null, 100, null);
			starter.AddPlayerLine("caravan_loot", "caravan_talk", "caravan_loot_talk", "{=WOBy5UfY}Hand over your goods, or die!", new ConversationSentence.OnConditionDelegate(this.caravan_loot_on_condition), null, 100, new ConversationSentence.OnClickableConditionDelegate(this.caravan_loot_on_clickable_condition), null);
			starter.AddPlayerLine("caravan_talk_leave", "caravan_talk", "close_window", "{=1IJouNaM}Carry on, then. Farewell.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_player_trade_end", "caravan_player_trade", "caravan_pretalk", "{=tlLDHAIu}Very well. A pleasure doing business with you.[rf:convo_relaxed_happy][ib:demure]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_player_trade_end_on_condition), null, 100, null);
			starter.AddDialogLine("caravan_player_trade_end_response", "caravan_player_trade_response", "close_window", "{=2g2FhKb5}Farewell.", null, null, 100, null);
			starter.AddDialogLine("caravan_fight", "caravan_loot_talk", "caravan_do_not_bribe", "{=QNaKmkt9}We're paid to guard this caravan. If you want to rob it, it's going to be over our dead bodies![rf:idle_angry][ib:aggressive]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_not_bribe_on_condition), null, 100, null);
			starter.AddPlayerLine("player_decided_to_fight", "caravan_do_not_bribe", "close_window", "{=EhxS7NQ4}So be it. Attack!", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_fight_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_to_not_fight_1", "caravan_do_not_bribe", "close_window", "{=bfPsE9M1}You must have misunderstood me. Go in peace.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_accepted_to_give_some_goods", "caravan_loot_talk", "caravan_give_some_goods", "{=dMc3SjOK}We can pay you. {TAKE_MONEY_AND_PRODUCT_STRING}[rf:idle_angry][ib:nervous]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_give_goods_on_condition), null, 100, null);
			starter.AddPlayerLine("player_decided_to_take_some_goods", "caravan_give_some_goods", "caravan_end_talk_bribe", "{=0Pd84h4W}I'll accept that.", null, null, 100, null, null);
			starter.AddPlayerLine("player_decided_to_take_everything", "caravan_give_some_goods", "player_wants_everything", "{=QZ6IcCIm}I want everything you've got.", null, null, 100, null, null);
			starter.AddPlayerLine("player_decided_to_not_fight_2", "caravan_give_some_goods", "close_window", "{=bfPsE9M1}You must have misunderstood me. Go in peace.", null, new ConversationSentence.OnConsequenceDelegate(this.caravan_talk_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("caravan_fight_no_surrender", "player_wants_everything", "close_window", "{=3JfCwL31}You will have to fight us first![rf:idle_angry][ib:aggressive]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_not_surrender_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_fight_on_consequence), 100, null);
			starter.AddDialogLine("caravan_accepted_to_give_everything", "player_wants_everything", "player_decision_to_take_prisoners", "{=hbtbSag8}We can't fight you. We surrender. Please don't hurt us. Take what you want.[if:idle_angry][ib:nervous]", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_give_goods_on_condition), null, 100, null);
			starter.AddPlayerLine("player_do_not_take_prisoners", "player_decision_to_take_prisoners", "caravan_end_talk_surrender", "{=6kaia5qP}Give me all your wares!", null, null, 100, null, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner", "player_decision_to_take_prisoners", "caravan_taken_prisoner_warning_check", "{=1gv0AVUN}You are my prisoners now.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_warn_player_to_take_prisoner", "caravan_taken_prisoner_warning_check", "caravan_taken_prisoner_warning_answer", "{=NuYzgBZB}You are going too far. The {KINGDOM} won't stand for the destruction of its caravans.", new ConversationSentence.OnConditionDelegate(this.conversation_warn_player_on_condition), null, 100, null);
			starter.AddDialogLine("caravan_do_not_warn_player", "caravan_taken_prisoner_warning_check", "close_window", "{=BvytaDUJ}Heaven protect us from the likes of you.", null, delegate()
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += this.player_take_prisoner_consequence;
			}, 100, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner_continue", "caravan_taken_prisoner_warning_answer", "close_window", "{=WVkc4UgX}Continue.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_took_prisoner_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner_leave", "caravan_taken_prisoner_warning_answer", "caravan_loot_talk", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			starter.AddDialogLine("caravan_bribery_leave", "caravan_end_talk_bribe", "close_window", "{=uPwKhAps}Can we leave now?", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_looted_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_looted_leave_on_consequence), 100, null);
			starter.AddDialogLine("caravan_surrender_leave", "caravan_end_talk_surrender", "close_window", "{=uPwKhAps}Can we leave now?", new ConversationSentence.OnConditionDelegate(this.conversation_caravan_looted_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_caravan_surrender_leave_on_consequence), 100, null);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000DFF68 File Offset: 0x000DE168
		private bool companion_is_caravan_leader_on_condition()
		{
			return Hero.OneToOneConversationHero != null && MobileParty.ConversationParty != null && MobileParty.ConversationParty.Party.Owner == Hero.MainHero && MobileParty.ConversationParty.IsCaravan && (Hero.OneToOneConversationHero.IsPlayerCompanion || Hero.OneToOneConversationHero.Clan == Clan.PlayerClan);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000DFFC8 File Offset: 0x000DE1C8
		private bool player_caravan_talk_start_on_condition()
		{
			return Hero.OneToOneConversationHero == null && MobileParty.ConversationParty != null && MobileParty.ConversationParty.Party.Owner == Hero.MainHero && MobileParty.ConversationParty.IsCaravan && PartyBase.MainParty.Side == BattleSideEnum.Attacker;
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x000E0014 File Offset: 0x000DE214
		private void player_take_prisoner_consequence()
		{
			if (MobileParty.MainParty.MapFaction.IsAtWarWith(PlayerEncounter.EncounteredMobileParty.MapFaction))
			{
				this.conversation_caravan_took_prisoner_on_consequence();
			}
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000E0038 File Offset: 0x000DE238
		private bool conversation_warn_player_on_condition()
		{
			IFaction mapFaction = MobileParty.ConversationParty.MapFaction;
			MBTextManager.SetTextVariable("KINGDOM", mapFaction.IsKingdomFaction ? ((Kingdom)mapFaction).EncyclopediaTitle : mapFaction.Name, false);
			return PlayerEncounter.Current != null && !PlayerEncounter.LeaveEncounter && !MobileParty.MainParty.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction);
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000E00A4 File Offset: 0x000DE2A4
		private bool caravan_start_talk_on_condition()
		{
			if (MobileParty.ConversationParty == null || !MobileParty.ConversationParty.IsCaravan)
			{
				return false;
			}
			CaravansCampaignBehavior.PlayerInteraction playerInteraction = this.GetPlayerInteraction(MobileParty.ConversationParty);
			this.SetPlayerInteraction(MobileParty.ConversationParty, CaravansCampaignBehavior.PlayerInteraction.Friendly);
			if (playerInteraction == CaravansCampaignBehavior.PlayerInteraction.Hostile)
			{
				MBTextManager.SetTextVariable("CARAVAN_GREETING", "{=L7AN6ybY}What do you want with us now?", false);
			}
			else if (playerInteraction != CaravansCampaignBehavior.PlayerInteraction.None)
			{
				MBTextManager.SetTextVariable("CARAVAN_GREETING", "{=Z5kqbeyu}Greetings, once again. Is there anything else?", false);
			}
			else if (CharacterObject.OneToOneConversationCharacter.IsHero && PartyBase.MainParty.Side == BattleSideEnum.Attacker && MobileParty.ConversationParty.Party.Owner != Hero.MainHero)
			{
				StringHelpers.SetCharacterProperties("LEADER", CharacterObject.OneToOneConversationCharacter, null, false);
				MBTextManager.SetTextVariable("CARAVAN_GREETING", "{=afVsbikp}Greetings, traveller. How may we help you?", false);
			}
			else
			{
				MBTextManager.SetTextVariable("HOMETOWN", MobileParty.ConversationParty.HomeSettlement.EncyclopediaLinkWithName, false);
				StringHelpers.SetCharacterProperties("MERCHANT", MobileParty.ConversationParty.Party.Owner.CharacterObject, null, false);
				StringHelpers.SetCharacterProperties("PROTECTOR", MobileParty.ConversationParty.HomeSettlement.OwnerClan.Leader.CharacterObject, null, false);
				TextObject text = new TextObject("{=FpUybbSk}Greetings. This caravan is owned by {MERCHANT.LINK}. We trade under the protection of {PROTECTOR.LINK}, master of {HOMETOWN}. How may we help you?[if:convo_normal]", null);
				MBTextManager.SetTextVariable("CARAVAN_GREETING", text, false);
			}
			return true;
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000E01DC File Offset: 0x000DE3DC
		private bool caravan_loot_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan && MobileParty.ConversationParty.Party.MapFaction != Hero.MainHero.MapFaction && MobileParty.ConversationParty.Party.Owner != Hero.MainHero;
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000E0234 File Offset: 0x000DE434
		private bool caravan_loot_on_clickable_condition(out TextObject explanation)
		{
			if (this._lootedCaravans.ContainsKey(MobileParty.ConversationParty))
			{
				explanation = new TextObject("{=il2khBNl}You just looted this party.", null);
				return false;
			}
			explanation = TextObject.Empty;
			int num;
			ItemRoster itemRoster;
			this.BribeAmount(MobileParty.ConversationParty.Party, out num, out itemRoster);
			bool flag = num > 0;
			bool flag2 = !itemRoster.IsEmpty<ItemRosterElement>();
			if (flag)
			{
				if (flag2)
				{
					TextObject textObject = (itemRoster.Count == 1) ? GameTexts.FindText("str_LEFT_RIGHT", null) : GameTexts.FindText("str_LEFT_comma_RIGHT", null);
					TextObject textObject2 = GameTexts.FindText("str_looted_party_have_money", null);
					textObject2.SetTextVariable("MONEY", num);
					textObject2.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					textObject2.SetTextVariable("ITEM_LIST", textObject);
					for (int i = 0; i < itemRoster.Count; i++)
					{
						ItemRosterElement elementCopyAtIndex = itemRoster.GetElementCopyAtIndex(i);
						TextObject textObject3 = GameTexts.FindText("str_offered_item_list", null);
						textObject3.SetTextVariable("COUNT", elementCopyAtIndex.Amount);
						textObject3.SetTextVariable("ITEM", elementCopyAtIndex.EquipmentElement.Item.Name);
						textObject.SetTextVariable("LEFT", textObject3);
						if (itemRoster.Count == 1)
						{
							textObject.SetTextVariable("RIGHT", TextObject.Empty);
						}
						else if (itemRoster.Count - 2 > i)
						{
							TextObject textObject4 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
							textObject.SetTextVariable("RIGHT", textObject4);
							textObject = textObject4;
						}
						else
						{
							TextObject textObject5 = GameTexts.FindText("str_LEFT_ONLY", null);
							textObject.SetTextVariable("RIGHT", textObject5);
							textObject = textObject5;
						}
					}
					MBTextManager.SetTextVariable("TAKE_MONEY_AND_PRODUCT_STRING", textObject2, false);
				}
				else
				{
					TextObject textObject6 = GameTexts.FindText("str_looted_party_have_money_but_no_item", null);
					textObject6.SetTextVariable("MONEY", num);
					textObject6.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
					MBTextManager.SetTextVariable("TAKE_MONEY_AND_PRODUCT_STRING", textObject6, false);
				}
			}
			else if (flag2)
			{
				TextObject textObject7 = (itemRoster.Count == 1) ? GameTexts.FindText("str_LEFT_RIGHT", null) : GameTexts.FindText("str_LEFT_comma_RIGHT", null);
				TextObject textObject8 = GameTexts.FindText("str_looted_party_have_no_money", null);
				textObject8.SetTextVariable("ITEM_LIST", textObject7);
				for (int j = 0; j < itemRoster.Count; j++)
				{
					ItemRosterElement elementCopyAtIndex2 = itemRoster.GetElementCopyAtIndex(j);
					TextObject textObject9 = GameTexts.FindText("str_offered_item_list", null);
					textObject9.SetTextVariable("COUNT", elementCopyAtIndex2.Amount);
					textObject9.SetTextVariable("ITEM", elementCopyAtIndex2.EquipmentElement.Item.Name);
					textObject7.SetTextVariable("LEFT", textObject9);
					if (itemRoster.Count == 1)
					{
						textObject7.SetTextVariable("RIGHT", TextObject.Empty);
					}
					else if (itemRoster.Count - 2 > j)
					{
						TextObject textObject10 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
						textObject7.SetTextVariable("RIGHT", textObject10);
						textObject7 = textObject10;
					}
					else
					{
						TextObject textObject11 = GameTexts.FindText("str_LEFT_ONLY", null);
						textObject7.SetTextVariable("RIGHT", textObject11);
						textObject7 = textObject11;
					}
				}
				MBTextManager.SetTextVariable("TAKE_MONEY_AND_PRODUCT_STRING", textObject8, false);
			}
			if (!flag && !flag2)
			{
				explanation = new TextObject("{=pbRwAjUN}They seem to have no valuable goods.", null);
				return false;
			}
			return true;
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000E0574 File Offset: 0x000DE774
		private bool caravan_buy_products_on_condition()
		{
			if (MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan)
			{
				for (int i = 0; i < MobileParty.ConversationParty.ItemRoster.Count; i++)
				{
					if (MobileParty.ConversationParty.ItemRoster.GetElementNumber(i) > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000E05C4 File Offset: 0x000DE7C4
		private void caravan_player_leave_encounter_on_consequence()
		{
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000E05CC File Offset: 0x000DE7CC
		private void caravan_ask_trade_rumors_on_consequence()
		{
			Town destinationForMobileParty = this.GetDestinationForMobileParty(MobileParty.ConversationParty);
			if (MobileParty.ConversationParty.LastVisitedSettlement != null && destinationForMobileParty != null && MobileParty.ConversationParty.LastVisitedSettlement != destinationForMobileParty.Settlement)
			{
				List<ValueTuple<CaravansCampaignBehavior.TradeActionLog, float>> list = new List<ValueTuple<CaravansCampaignBehavior.TradeActionLog, float>>();
				List<CaravansCampaignBehavior.TradeActionLog> list2;
				if (this._tradeActionLogs.TryGetValue(MobileParty.ConversationParty, out list2))
				{
					foreach (CaravansCampaignBehavior.TradeActionLog tradeActionLog in list2)
					{
						float profitRate = tradeActionLog.ProfitRate;
						if (profitRate > 1.2f && tradeActionLog.SoldSettlement != null && tradeActionLog.SoldSettlement != tradeActionLog.BoughtSettlement)
						{
							list.Add(new ValueTuple<CaravansCampaignBehavior.TradeActionLog, float>(tradeActionLog, profitRate));
						}
					}
				}
				if (list.Count <= 0)
				{
					MBTextManager.SetTextVariable("COMMENT", GameTexts.FindText("str_caravan_trade_comment_no_profit", null), false);
					return;
				}
				CaravansCampaignBehavior.TradeActionLog tradeActionLog2 = MBRandom.ChooseWeighted<CaravansCampaignBehavior.TradeActionLog>(list);
				MBTextManager.SetTextVariable("ITEM_NAME", tradeActionLog2.ItemRosterElement.EquipmentElement.Item.Name, false);
				MBTextManager.SetTextVariable("SETTLEMENT", tradeActionLog2.BoughtSettlement.EncyclopediaLinkWithName, false);
				MBTextManager.SetTextVariable("DESTINATION", tradeActionLog2.SoldSettlement.EncyclopediaLinkWithName, false);
				MBTextManager.SetTextVariable("BUY_COST", tradeActionLog2.BuyPrice);
				MBTextManager.SetTextVariable("SELL_COST", tradeActionLog2.SellPrice);
				MBTextManager.SetTextVariable("COMMENT", GameTexts.FindText("str_caravan_trade_comment", null), false);
				if (!this._tradeRumorTakenCaravans.ContainsKey(MobileParty.ConversationParty) || (this._tradeRumorTakenCaravans.ContainsKey(MobileParty.ConversationParty) && CampaignTime.Now - this._tradeRumorTakenCaravans[MobileParty.ConversationParty] >= CampaignTime.Days(1f)))
				{
					List<TradeRumor> list3 = new List<TradeRumor>();
					list3.Add(new TradeRumor(destinationForMobileParty.Owner.Settlement, tradeActionLog2.ItemRosterElement.EquipmentElement.Item, destinationForMobileParty.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, false), destinationForMobileParty.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, true), 10));
					Town town = MobileParty.ConversationParty.LastVisitedSettlement.Town;
					if (town != null)
					{
						list3.Add(new TradeRumor(town.Owner.Settlement, tradeActionLog2.ItemRosterElement.EquipmentElement.Item, town.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, false), town.GetItemPrice(tradeActionLog2.ItemRosterElement.EquipmentElement.Item, null, true), 10));
					}
					if (list3.Count > 0)
					{
						CampaignEventDispatcher.Instance.OnTradeRumorIsTaken(list3, null);
						if (this._tradeRumorTakenCaravans.ContainsKey(MobileParty.ConversationParty) && CampaignTime.Now - this._tradeRumorTakenCaravans[MobileParty.ConversationParty] >= CampaignTime.Days(1f))
						{
							this._tradeRumorTakenCaravans[MobileParty.ConversationParty] = CampaignTime.Now;
							return;
						}
						this._tradeRumorTakenCaravans.Add(MobileParty.ConversationParty, CampaignTime.Now);
						return;
					}
				}
			}
			else
			{
				MBTextManager.SetTextVariable("COMMENT", new TextObject("{=TEUVTPIa}Well, we've been resting in town for a while, so our information is probably quite out of date.", null), false);
			}
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000E0934 File Offset: 0x000DEB34
		private void caravan_talk_leave_on_consequence()
		{
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.LeaveEncounter = true;
			}
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000E0943 File Offset: 0x000DEB43
		private bool conversation_caravan_player_trade_end_on_condition()
		{
			if (MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan)
			{
				InventoryManager.OpenTradeWithCaravanOrAlleyParty(MobileParty.ConversationParty, InventoryManager.InventoryCategoryType.None);
			}
			return true;
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000E0964 File Offset: 0x000DEB64
		private bool conversation_caravan_not_bribe_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan && !this.IsBribeFeasible();
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x000E0984 File Offset: 0x000DEB84
		private bool conversation_caravan_not_surrender_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan && !this.IsSurrenderFeasible(MobileParty.ConversationParty, MobileParty.MainParty);
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000E09AE File Offset: 0x000DEBAE
		private void conversation_caravan_fight_on_consequence()
		{
			this.SetPlayerInteraction(MobileParty.ConversationParty, CaravansCampaignBehavior.PlayerInteraction.Hostile);
			PlayerEncounter.Current.IsEnemy = true;
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000E09C7 File Offset: 0x000DEBC7
		private bool conversation_caravan_give_goods_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x000E09DC File Offset: 0x000DEBDC
		private bool conversation_caravan_looted_leave_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsCaravan;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x000E09F4 File Offset: 0x000DEBF4
		private void conversation_caravan_looted_leave_on_consequence()
		{
			int amount;
			ItemRoster itemRoster;
			this.BribeAmount(MobileParty.ConversationParty.Party, out amount, out itemRoster);
			GiveGoldAction.ApplyForPartyToCharacter(MobileParty.ConversationParty.Party, Hero.MainHero, amount, false);
			if (!itemRoster.IsEmpty<ItemRosterElement>())
			{
				for (int i = itemRoster.Count - 1; i >= 0; i--)
				{
					PartyBase party = MobileParty.ConversationParty.Party;
					PartyBase party2 = Hero.MainHero.PartyBelongedTo.Party;
					ItemRosterElement itemRosterElement = itemRoster[i];
					GiveItemAction.ApplyForParties(party, party2, itemRosterElement);
				}
			}
			BeHostileAction.ApplyMinorCoercionHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
			this._lootedCaravans.Add(MobileParty.ConversationParty, CampaignTime.Now);
			this.SetPlayerInteraction(MobileParty.ConversationParty, CaravansCampaignBehavior.PlayerInteraction.Hostile);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, MobileParty.ConversationParty, itemRoster, false);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x000E0ABC File Offset: 0x000DECBC
		private void conversation_caravan_surrender_leave_on_consequence()
		{
			ItemRoster itemRoster = new ItemRoster(MobileParty.ConversationParty.ItemRoster);
			bool flag = false;
			for (int i = 0; i < itemRoster.Count; i++)
			{
				if (itemRoster.GetElementNumber(i) > 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				InventoryManager.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster>
				{
					{
						PartyBase.MainParty,
						itemRoster
					}
				});
				MobileParty.ConversationParty.ItemRoster.Clear();
			}
			int num = MathF.Max(MobileParty.ConversationParty.PartyTradeGold, 0);
			if (num > 0)
			{
				GiveGoldAction.ApplyForPartyToCharacter(MobileParty.ConversationParty.Party, Hero.MainHero, num, false);
			}
			BeHostileAction.ApplyMajorCoercionHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
			this._lootedCaravans.Add(MobileParty.ConversationParty, CampaignTime.Now);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, MobileParty.ConversationParty, itemRoster, false);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x000E0B90 File Offset: 0x000DED90
		private void conversation_caravan_took_prisoner_on_consequence()
		{
			MobileParty encounteredMobileParty = PlayerEncounter.EncounteredMobileParty;
			ItemRoster itemRoster = new ItemRoster(encounteredMobileParty.ItemRoster);
			bool flag = false;
			for (int i = 0; i < itemRoster.Count; i++)
			{
				if (itemRoster.GetElementNumber(i) > 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				InventoryManager.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster>
				{
					{
						PartyBase.MainParty,
						itemRoster
					}
				});
				encounteredMobileParty.ItemRoster.Clear();
			}
			int num = MathF.Max(encounteredMobileParty.PartyTradeGold, 0);
			if (num > 0)
			{
				GiveGoldAction.ApplyForPartyToCharacter(encounteredMobileParty.Party, Hero.MainHero, num, false);
			}
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, encounteredMobileParty.Party);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopRosterElement troopRosterElement in encounteredMobileParty.MemberRoster.GetTroopRoster())
			{
				troopRoster.AddToCounts(troopRosterElement.Character, troopRosterElement.Number, false, 0, 0, true, -1);
			}
			PartyScreenManager.OpenScreenAsLoot(TroopRoster.CreateDummyTroopRoster(), troopRoster, encounteredMobileParty.Name, troopRoster.TotalManCount, null);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, encounteredMobileParty, itemRoster, false);
			DestroyPartyAction.Apply(MobileParty.MainParty.Party, encounteredMobileParty);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x000E0CD4 File Offset: 0x000DEED4
		private bool IsBribeFeasible()
		{
			int num = PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.1f) ? 33 : 67;
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num = MathF.Round((float)num * (1f - DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return MobileParty.ConversationParty.Party.RandomIntWithSeed(5U, 100) <= 100 - num && PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.6f);
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x000E0D58 File Offset: 0x000DEF58
		private bool IsSurrenderFeasible(MobileParty conversationParty, MobileParty mainParty)
		{
			int num = PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.1f) ? 33 : 67;
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num = MathF.Round((float)num * (1f - DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return conversationParty.Party.RandomIntWithSeed(7U, 100) <= 100 - num && PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.1f);
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x000E0DD8 File Offset: 0x000DEFD8
		private void BribeAmount(PartyBase party, out int gold, out ItemRoster items)
		{
			int num = 0;
			ItemRoster itemRoster = new ItemRoster();
			bool flag = false;
			for (int i = 0; i < MobileParty.ConversationParty.ItemRoster.Count; i++)
			{
				num += MobileParty.ConversationParty.ItemRoster.GetElementUnitCost(i) * MobileParty.ConversationParty.ItemRoster.GetElementNumber(i);
				flag = true;
			}
			num += MobileParty.ConversationParty.PartyTradeGold;
			int num2 = MathF.Min((int)((float)num * 0.05f), 2000);
			int num3 = MathF.Min(MobileParty.ConversationParty.PartyTradeGold, num2);
			if (num3 < num2 && flag)
			{
				for (int j = 0; j < MobileParty.ConversationParty.ItemRoster.Count; j++)
				{
					ItemRosterElement elementCopyAtIndex = MobileParty.ConversationParty.ItemRoster.GetElementCopyAtIndex(j);
					if (elementCopyAtIndex.EquipmentElement.ItemValue * elementCopyAtIndex.Amount >= num2 - num3)
					{
						if (elementCopyAtIndex.EquipmentElement.Item.Type == ItemObject.ItemTypeEnum.Goods)
						{
							if (!itemRoster.IsEmpty<ItemRosterElement>())
							{
								itemRoster.Clear();
							}
							itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement.Item, elementCopyAtIndex.Amount);
							break;
						}
						if (itemRoster.IsEmpty<ItemRosterElement>())
						{
							itemRoster.AddToCounts(elementCopyAtIndex.EquipmentElement.Item, elementCopyAtIndex.Amount);
						}
					}
				}
				if (itemRoster.IsEmpty<ItemRosterElement>())
				{
					int num4 = num2 - num3;
					int num5 = 0;
					while (num5 < MobileParty.ConversationParty.ItemRoster.Count && num4 > 0)
					{
						ItemRosterElement randomElement = MobileParty.ConversationParty.ItemRoster.GetRandomElement<ItemRosterElement>();
						num4 -= randomElement.Amount * randomElement.EquipmentElement.ItemValue;
						itemRoster.AddToCounts(randomElement.EquipmentElement.Item, randomElement.Amount);
						num5++;
					}
				}
			}
			gold = num3;
			items = itemRoster;
		}

		// Token: 0x04001100 RID: 4352
		private const float AverageCaravanWaitAtSettlement = 3f;

		// Token: 0x04001101 RID: 4353
		private const int MaxMoneyToSpendOnSingleCategory = 1500;

		// Token: 0x04001102 RID: 4354
		private const int MaxNumberOfItemsToBuyFromSingleCategory = 100;

		// Token: 0x04001103 RID: 4355
		public const int InitialCaravanGold = 10000;

		// Token: 0x04001104 RID: 4356
		private const float ProfitRateRumorThreshold = 1.2f;

		// Token: 0x04001105 RID: 4357
		private float ReferenceBudgetValue = 5000f;

		// Token: 0x04001106 RID: 4358
		private Dictionary<MobileParty, CampaignTime> _tradeRumorTakenCaravans = new Dictionary<MobileParty, CampaignTime>();

		// Token: 0x04001107 RID: 4359
		private Dictionary<MobileParty, CampaignTime> _caravanLastHomeTownVisitTime = new Dictionary<MobileParty, CampaignTime>();

		// Token: 0x04001108 RID: 4360
		private Dictionary<MobileParty, CampaignTime> _lootedCaravans = new Dictionary<MobileParty, CampaignTime>();

		// Token: 0x04001109 RID: 4361
		private Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction> _interactedCaravans = new Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction>();

		// Token: 0x0400110A RID: 4362
		private Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>> _tradeActionLogs = new Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>>();

		// Token: 0x0400110B RID: 4363
		private Dictionary<MobileParty, List<Settlement>> _previouslyChangedCaravanTargetsDueToEnemyOnWay = new Dictionary<MobileParty, List<Settlement>>();

		// Token: 0x0400110C RID: 4364
		private CaravansCampaignBehavior.TradeActionLogPool _tradeActionLogPool;

		// Token: 0x0400110D RID: 4365
		private int _packAnimalCategoryIndex = -1;

		// Token: 0x0400110E RID: 4366
		private readonly Dictionary<ItemCategory, float> _averageValuesCached = new Dictionary<ItemCategory, float>();

		// Token: 0x0400110F RID: 4367
		private readonly Dictionary<ItemCategory, CaravansCampaignBehavior.PriceIndexData> _priceDictionary = new Dictionary<ItemCategory, CaravansCampaignBehavior.PriceIndexData>();

		// Token: 0x04001110 RID: 4368
		private readonly Dictionary<ItemCategory, int> _totalValueOfItemsAtCategory = new Dictionary<ItemCategory, int>();

		// Token: 0x020006C3 RID: 1731
		public class CaravansCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060056D9 RID: 22233 RVA: 0x00180013 File Offset: 0x0017E213
			public CaravansCampaignBehaviorTypeDefiner() : base(60000)
			{
			}

			// Token: 0x060056DA RID: 22234 RVA: 0x00180020 File Offset: 0x0017E220
			protected override void DefineEnumTypes()
			{
				base.AddEnumDefinition(typeof(CaravansCampaignBehavior.PlayerInteraction), 1, null);
			}

			// Token: 0x060056DB RID: 22235 RVA: 0x00180034 File Offset: 0x0017E234
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<MobileParty, CaravansCampaignBehavior.PlayerInteraction>));
				base.ConstructContainerDefinition(typeof(List<CaravansCampaignBehavior.TradeActionLog>));
				base.ConstructContainerDefinition(typeof(Dictionary<MobileParty, List<CaravansCampaignBehavior.TradeActionLog>>));
			}

			// Token: 0x060056DC RID: 22236 RVA: 0x00180066 File Offset: 0x0017E266
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(CaravansCampaignBehavior.TradeActionLog), 2, null);
			}
		}

		// Token: 0x020006C4 RID: 1732
		private enum PlayerInteraction
		{
			// Token: 0x04001C06 RID: 7174
			None,
			// Token: 0x04001C07 RID: 7175
			Friendly,
			// Token: 0x04001C08 RID: 7176
			TradedWith,
			// Token: 0x04001C09 RID: 7177
			Hostile
		}

		// Token: 0x020006C5 RID: 1733
		private struct PriceIndexData
		{
			// Token: 0x060056DD RID: 22237 RVA: 0x0018007A File Offset: 0x0017E27A
			public PriceIndexData(float averageBuySellPriceIndex, float minBuySellPriceIndex)
			{
				this.AverageBuySellPriceIndex = averageBuySellPriceIndex;
				this.MinBuySellPriceIndex = minBuySellPriceIndex;
			}

			// Token: 0x04001C0A RID: 7178
			internal readonly float AverageBuySellPriceIndex;

			// Token: 0x04001C0B RID: 7179
			internal readonly float MinBuySellPriceIndex;
		}

		// Token: 0x020006C6 RID: 1734
		internal class TradeActionLog
		{
			// Token: 0x170013AC RID: 5036
			// (get) Token: 0x060056DE RID: 22238 RVA: 0x0018008A File Offset: 0x0017E28A
			public float ProfitRate
			{
				get
				{
					return (float)this.SellPrice / (float)this.BuyPrice;
				}
			}

			// Token: 0x060056DF RID: 22239 RVA: 0x0018009B File Offset: 0x0017E29B
			public void OnSellAction(Settlement soldSettlement, int sellPrice)
			{
				this.SellPrice = sellPrice;
				this.SoldSettlement = soldSettlement;
			}

			// Token: 0x060056E0 RID: 22240 RVA: 0x001800AB File Offset: 0x0017E2AB
			public void Reset()
			{
				this.BoughtSettlement = null;
				this.SoldSettlement = null;
				this.SellPrice = 0;
				this.BuyPrice = 0;
			}

			// Token: 0x060056E1 RID: 22241 RVA: 0x001800C9 File Offset: 0x0017E2C9
			internal static void AutoGeneratedStaticCollectObjectsTradeActionLog(object o, List<object> collectedObjects)
			{
				((CaravansCampaignBehavior.TradeActionLog)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060056E2 RID: 22242 RVA: 0x001800D7 File Offset: 0x0017E2D7
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.BoughtSettlement);
				ItemRosterElement.AutoGeneratedStaticCollectObjectsItemRosterElement(this.ItemRosterElement, collectedObjects);
				collectedObjects.Add(this.SoldSettlement);
				CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.BoughtTime, collectedObjects);
			}

			// Token: 0x060056E3 RID: 22243 RVA: 0x00180113 File Offset: 0x0017E313
			internal static object AutoGeneratedGetMemberValueBoughtSettlement(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).BoughtSettlement;
			}

			// Token: 0x060056E4 RID: 22244 RVA: 0x00180120 File Offset: 0x0017E320
			internal static object AutoGeneratedGetMemberValueBuyPrice(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).BuyPrice;
			}

			// Token: 0x060056E5 RID: 22245 RVA: 0x00180132 File Offset: 0x0017E332
			internal static object AutoGeneratedGetMemberValueSellPrice(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).SellPrice;
			}

			// Token: 0x060056E6 RID: 22246 RVA: 0x00180144 File Offset: 0x0017E344
			internal static object AutoGeneratedGetMemberValueItemRosterElement(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).ItemRosterElement;
			}

			// Token: 0x060056E7 RID: 22247 RVA: 0x00180156 File Offset: 0x0017E356
			internal static object AutoGeneratedGetMemberValueSoldSettlement(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).SoldSettlement;
			}

			// Token: 0x060056E8 RID: 22248 RVA: 0x00180163 File Offset: 0x0017E363
			internal static object AutoGeneratedGetMemberValueBoughtTime(object o)
			{
				return ((CaravansCampaignBehavior.TradeActionLog)o).BoughtTime;
			}

			// Token: 0x04001C0C RID: 7180
			[SaveableField(0)]
			public Settlement BoughtSettlement;

			// Token: 0x04001C0D RID: 7181
			[SaveableField(1)]
			public int BuyPrice;

			// Token: 0x04001C0E RID: 7182
			[SaveableField(2)]
			public int SellPrice;

			// Token: 0x04001C0F RID: 7183
			[SaveableField(3)]
			public ItemRosterElement ItemRosterElement;

			// Token: 0x04001C10 RID: 7184
			[SaveableField(4)]
			public Settlement SoldSettlement;

			// Token: 0x04001C11 RID: 7185
			[SaveableField(5)]
			public CampaignTime BoughtTime;
		}

		// Token: 0x020006C7 RID: 1735
		internal class TradeActionLogPool
		{
			// Token: 0x170013AD RID: 5037
			// (get) Token: 0x060056EA RID: 22250 RVA: 0x0018017D File Offset: 0x0017E37D
			public int Size
			{
				get
				{
					Stack<CaravansCampaignBehavior.TradeActionLog> stack = this._stack;
					if (stack == null)
					{
						return 0;
					}
					return stack.Count;
				}
			}

			// Token: 0x170013AE RID: 5038
			// (get) Token: 0x060056EB RID: 22251 RVA: 0x00180190 File Offset: 0x0017E390
			private int MaxSize { get; }

			// Token: 0x060056EC RID: 22252 RVA: 0x00180198 File Offset: 0x0017E398
			public TradeActionLogPool(int size)
			{
				this.MaxSize = size;
				this._stack = new Stack<CaravansCampaignBehavior.TradeActionLog>(size);
				for (int i = 0; i < size; i++)
				{
					this._stack.Push(new CaravansCampaignBehavior.TradeActionLog());
				}
			}

			// Token: 0x060056ED RID: 22253 RVA: 0x001801DC File Offset: 0x0017E3DC
			public CaravansCampaignBehavior.TradeActionLog CreateNewLog(Settlement boughtSettlement, int buyPrice, ItemRosterElement itemRosterElement)
			{
				CaravansCampaignBehavior.TradeActionLog tradeActionLog = (this._stack.Count > 0) ? this._stack.Pop() : new CaravansCampaignBehavior.TradeActionLog();
				tradeActionLog.BoughtSettlement = boughtSettlement;
				tradeActionLog.BuyPrice = buyPrice;
				tradeActionLog.ItemRosterElement = itemRosterElement;
				tradeActionLog.BoughtTime = CampaignTime.Now;
				return tradeActionLog;
			}

			// Token: 0x060056EE RID: 22254 RVA: 0x00180229 File Offset: 0x0017E429
			public void ReleaseLog(CaravansCampaignBehavior.TradeActionLog log)
			{
				log.Reset();
				if (this._stack.Count < this.MaxSize)
				{
					this._stack.Push(log);
				}
			}

			// Token: 0x060056EF RID: 22255 RVA: 0x00180250 File Offset: 0x0017E450
			public override string ToString()
			{
				return string.Format("TrackPool: {0}", this.Size);
			}

			// Token: 0x04001C13 RID: 7187
			private Stack<CaravansCampaignBehavior.TradeActionLog> _stack;
		}
	}
}
