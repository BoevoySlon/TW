﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
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
	// Token: 0x020003E4 RID: 996
	public class VillagerCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003DEF RID: 15855 RVA: 0x0012EEE0 File Offset: 0x0012D0E0
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.DistributeLootToPartyEvent.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, Dictionary<PartyBase, ItemRoster>>(this.OnVillagerPartyLooted));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x0012EFBC File Offset: 0x0012D1BC
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			for (int i = 0; i < siegeEvent.BesiegedSettlement.Parties.Count; i++)
			{
				if (siegeEvent.BesiegedSettlement.Parties[i].IsVillager)
				{
					siegeEvent.BesiegedSettlement.Parties[i].Ai.SetMoveModeHold();
				}
			}
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x0012F018 File Offset: 0x0012D218
		private void OnVillagerPartyLooted(MapEvent mapEvent, PartyBase party, Dictionary<PartyBase, ItemRoster> loot)
		{
			foreach (PartyBase partyBase in loot.Keys)
			{
				if (partyBase.IsMobile && partyBase.MobileParty.IsVillager && party.IsMobile)
				{
					SkillLevelingManager.OnLoot(party.MobileParty, partyBase.MobileParty, loot[partyBase], true);
				}
			}
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x0012F09C File Offset: 0x0012D29C
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<float>("_collectFoodWaitHoursProgress", ref this._collectFoodWaitHoursProgress);
			dataStore.SyncData<float>("_collectVolunteerWaitHoursProgress", ref this._collectVolunteerWaitHoursProgress);
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_lootedVillagers", ref this._lootedVillagers);
			dataStore.SyncData<Dictionary<MobileParty, VillagerCampaignBehavior.PlayerInteraction>>("_interactedVillagers", ref this._interactedVillagers);
			dataStore.SyncData<Dictionary<Village, CampaignTime>>("_villageLastVillagerSendTime", ref this._villageLastVillagerSendTime);
			dataStore.SyncData<Dictionary<MobileParty, List<Settlement>>>("_previouslyChangedVillagerTargetsDueToEnemyOnWay", ref this._previouslyChangedVillagerTargetsDueToEnemyOnWay);
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0012F118 File Offset: 0x0012D318
		private void DeleteExpiredLootedVillagers()
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (KeyValuePair<MobileParty, CampaignTime> keyValuePair in this._lootedVillagers)
			{
				if (CampaignTime.Now - keyValuePair.Value >= CampaignTime.Days(10f))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (MobileParty key in list)
			{
				this._lootedVillagers.Remove(key);
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0012F1E0 File Offset: 0x0012D3E0
		public void DailyTick()
		{
			this.DeleteExpiredLootedVillagers();
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x0012F1E8 File Offset: 0x0012D3E8
		private void TickVillageThink(Settlement settlement)
		{
			Village village = settlement.Village;
			if (village != null && village.VillageState == Village.VillageStates.Normal && settlement.Party.MapEvent == null)
			{
				this.ThinkAboutSendingItemToTown(village);
			}
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x0012F21C File Offset: 0x0012D41C
		private void ThinkAboutSendingItemToTown(Village village)
		{
			if (MBRandom.RandomFloat < 0.15f)
			{
				VillagerPartyComponent villagerPartyComponent = village.VillagerPartyComponent;
				MobileParty mobileParty = (villagerPartyComponent != null) ? villagerPartyComponent.MobileParty : null;
				if ((mobileParty != null && (mobileParty.CurrentSettlement != village.Owner.Settlement || mobileParty.MapEvent != null)) || village.Owner.MapEvent != null)
				{
					return;
				}
				int num = 0;
				for (int i = 0; i < village.Owner.ItemRoster.Count; i++)
				{
					num += village.Owner.ItemRoster[i].Amount;
				}
				int werehouseCapacity = village.GetWerehouseCapacity();
				if (num >= werehouseCapacity && village.Owner.MapEvent == null)
				{
					if (mobileParty == null || (this._villageLastVillagerSendTime.ContainsKey(village) && this._villageLastVillagerSendTime[village].ElapsedDaysUntilNow > 7f && mobileParty.CurrentSettlement != village.Settlement))
					{
						if (village.Hearth > 12f)
						{
							this.CreateVillagerParty(village);
						}
					}
					else
					{
						int num2 = this.FindIdealPartySize(village);
						if (mobileParty.MemberRoster.TotalManCount < num2)
						{
							this.AddVillagersToParty(mobileParty, num2 - mobileParty.MemberRoster.TotalManCount);
						}
					}
					if (mobileParty != null)
					{
						this.LoadAndSendVillagerParty(village, mobileParty);
					}
				}
			}
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x0012F358 File Offset: 0x0012D558
		private void AddVillagersToParty(MobileParty villagerParty, int numberOfVillagersToAdd)
		{
			if (numberOfVillagersToAdd > (int)villagerParty.HomeSettlement.Village.Hearth)
			{
				numberOfVillagersToAdd = (int)villagerParty.HomeSettlement.Village.Hearth;
			}
			villagerParty.HomeSettlement.Village.Hearth -= (float)((numberOfVillagersToAdd + 1) / 2);
			CharacterObject character = villagerParty.HomeSettlement.Culture.VillagerPartyTemplate.Stacks.GetRandomElement<PartyTemplateStack>().Character;
			villagerParty.MemberRoster.AddToCounts(character, numberOfVillagersToAdd, false, 0, 0, true, -1);
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x0012F3DC File Offset: 0x0012D5DC
		private int FindIdealPartySize(Village village)
		{
			float num = 0f;
			foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
			{
				float num2 = Campaign.Current.Models.VillageProductionCalculatorModel.CalculateDailyProductionAmount(village, valueTuple.Item1);
				num += num2;
			}
			float num3 = (num > 10f) ? (40f * (1f - (MathF.Min(40f, num) - 10f) / 60f)) : 40f;
			return 12 + (int)(village.Hearth / num3);
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x0012F494 File Offset: 0x0012D694
		private void CreateVillagerParty(Village village)
		{
			EnterSettlementAction.ApplyForParty(VillagerPartyComponent.CreateVillagerParty(village.Settlement.Culture.VillagerPartyTemplate.StringId + "_1", village, this.FindIdealPartySize(village)), village.Settlement);
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x0012F4D0 File Offset: 0x0012D6D0
		private void LoadAndSendVillagerParty(Village village, MobileParty villagerParty)
		{
			if (!this._villageLastVillagerSendTime.ContainsKey(village))
			{
				this._villageLastVillagerSendTime.Add(village, CampaignTime.Now);
			}
			else
			{
				this._villageLastVillagerSendTime[village] = CampaignTime.Now;
			}
			VillagerCampaignBehavior.MoveItemsToVillagerParty(village, villagerParty);
			this.SendVillagerPartyToTradeBoundTown(villagerParty);
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x0012F520 File Offset: 0x0012D720
		private static void MoveItemsToVillagerParty(Village village, MobileParty villagerParty)
		{
			ItemRoster itemRoster = village.Settlement.ItemRoster;
			float num = (float)villagerParty.InventoryCapacity - villagerParty.ItemRoster.TotalWeight;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < itemRoster.Count; j++)
				{
					ItemRosterElement itemRosterElement = itemRoster[j];
					ItemObject item = itemRosterElement.EquipmentElement.Item;
					int num2 = MBRandom.RoundRandomized((float)itemRosterElement.Amount * 0.2f);
					if (num2 > 0)
					{
						if (!item.HasHorseComponent && item.Weight * (float)num2 > num)
						{
							num2 = MathF.Ceiling(num / item.Weight);
							if (num2 <= 0)
							{
								goto IL_D1;
							}
						}
						if (!item.HasHorseComponent)
						{
							num -= (float)num2 * item.Weight;
						}
						villagerParty.Party.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, num2);
						itemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num2);
					}
					IL_D1:;
				}
			}
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x0012F619 File Offset: 0x0012D819
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._interactedVillagers.ContainsKey(mobileParty))
			{
				this._interactedVillagers.Remove(mobileParty);
			}
			if (this._previouslyChangedVillagerTargetsDueToEnemyOnWay.ContainsKey(mobileParty))
			{
				this._previouslyChangedVillagerTargetsDueToEnemyOnWay.Remove(mobileParty);
			}
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x0012F651 File Offset: 0x0012D851
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			if (mobileParty.IsVillager)
			{
				this._previouslyChangedVillagerTargetsDueToEnemyOnWay.Add(mobileParty, new List<Settlement>());
			}
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x0012F66C File Offset: 0x0012D86C
		private void HourlyTickSettlement(Settlement settlement)
		{
			this.DestroyVillagerPartyIfMemberCountIsZero(settlement);
			this.ThinkAboutSendingInsideVillagersToTheirHomeVillage(settlement);
			this.TickVillageThink(settlement);
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0012F684 File Offset: 0x0012D884
		private void DestroyVillagerPartyIfMemberCountIsZero(Settlement settlement)
		{
			Village village = settlement.Village;
			if (village != null && village.VillagerPartyComponent != null && village.VillagerPartyComponent.MobileParty.MapEvent == null && village.VillagerPartyComponent.MobileParty.MemberRoster.TotalHealthyCount == 0)
			{
				DestroyPartyAction.Apply(null, village.VillagerPartyComponent.MobileParty);
			}
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0012F6E0 File Offset: 0x0012D8E0
		private void HourlyTickParty(MobileParty villagerParty)
		{
			if (!villagerParty.IsVillager || villagerParty.MapEvent != null)
			{
				return;
			}
			bool flag = false;
			if (villagerParty.CurrentSettlement != null)
			{
				if (villagerParty.HomeSettlement.Village.VillagerPartyComponent == null || villagerParty.HomeSettlement.Village.VillagerPartyComponent.MobileParty != villagerParty)
				{
					DestroyPartyAction.Apply(null, villagerParty);
				}
			}
			else if (villagerParty.DefaultBehavior == AiBehavior.GoToSettlement)
			{
				if (villagerParty.TargetSettlement.IsTown && (villagerParty.TargetSettlement == null || villagerParty.TargetSettlement.IsUnderSiege || villagerParty.Ai.NeedTargetReset || FactionManager.IsAtWarAgainstFaction(villagerParty.MapFaction, villagerParty.TargetSettlement.MapFaction)))
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (villagerParty.ItemRoster.Count > 1)
				{
					if (villagerParty.Ai.NeedTargetReset)
					{
						this._previouslyChangedVillagerTargetsDueToEnemyOnWay[villagerParty].Add(villagerParty.TargetSettlement);
					}
					this.SendVillagerPartyToTradeBoundTown(villagerParty);
					return;
				}
				this.SendVillagerPartyToVillage(villagerParty);
			}
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0012F7D5 File Offset: 0x0012D9D5
		private void SendVillagerPartyToVillage(MobileParty villagerParty)
		{
			villagerParty.Ai.SetMoveGoToSettlement(villagerParty.HomeSettlement);
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0012F7E8 File Offset: 0x0012D9E8
		private void SendVillagerPartyToTradeBoundTown(MobileParty villagerParty)
		{
			Settlement tradeBound = villagerParty.HomeSettlement.Village.TradeBound;
			if (tradeBound != null && !tradeBound.IsUnderSiege)
			{
				villagerParty.Ai.SetMoveGoToSettlement(tradeBound);
			}
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x0012F820 File Offset: 0x0012DA20
		private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (mobileParty != null && mobileParty.IsActive && mobileParty.IsVillager)
			{
				this._previouslyChangedVillagerTargetsDueToEnemyOnWay[mobileParty].Clear();
				if (settlement.IsTown)
				{
					SellGoodsForTradeAction.ApplyByVillagerTrade(settlement, mobileParty);
				}
				if (settlement.IsVillage)
				{
					int num = Campaign.Current.Models.SettlementTaxModel.CalculateVillageTaxFromIncome(mobileParty.HomeSettlement.Village, mobileParty.PartyTradeGold);
					mobileParty.PartyTradeGold = 0;
					mobileParty.HomeSettlement.Village.TradeTaxAccumulated += num;
				}
				if (settlement.IsTown && settlement.Town.Governor != null && settlement.Town.Governor.GetPerkValue(DefaultPerks.Trade.TravelingRumors))
				{
					settlement.Town.TradeTaxAccumulated += MathF.Round(DefaultPerks.Trade.TravelingRumors.SecondaryBonus);
				}
			}
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x0012F903 File Offset: 0x0012DB03
		private void SetPlayerInteraction(MobileParty mobileParty, VillagerCampaignBehavior.PlayerInteraction interaction)
		{
			if (this._interactedVillagers.ContainsKey(mobileParty))
			{
				this._interactedVillagers[mobileParty] = interaction;
				return;
			}
			this._interactedVillagers.Add(mobileParty, interaction);
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0012F930 File Offset: 0x0012DB30
		private VillagerCampaignBehavior.PlayerInteraction GetPlayerInteraction(MobileParty mobileParty)
		{
			VillagerCampaignBehavior.PlayerInteraction result;
			if (this._interactedVillagers.TryGetValue(mobileParty, out result))
			{
				return result;
			}
			return VillagerCampaignBehavior.PlayerInteraction.None;
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0012F950 File Offset: 0x0012DB50
		private void ThinkAboutSendingInsideVillagersToTheirHomeVillage(Settlement settlement)
		{
			if ((settlement.IsVillage || settlement.IsTown) && !settlement.IsUnderSiege && settlement.Party.MapEvent == null)
			{
				for (int i = 0; i < settlement.Parties.Count; i++)
				{
					MobileParty mobileParty = settlement.Parties[i];
					if (mobileParty.IsActive && mobileParty.IsVillager && MBRandom.RandomFloat < 0.2f)
					{
						if (settlement.IsTown)
						{
							mobileParty.Ai.SetMoveGoToSettlement(mobileParty.HomeSettlement);
						}
						else if (mobileParty.ItemRoster.Count > 1 && settlement != mobileParty.HomeSettlement)
						{
							this.SendVillagerPartyToTradeBoundTown(mobileParty);
						}
					}
				}
			}
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0012FA00 File Offset: 0x0012DC00
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
			this.AddMenus(campaignGameStarter);
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0012FA10 File Offset: 0x0012DC10
		protected void AddDialogs(CampaignGameStarter campaignGameSystemStarter)
		{
			this.AddVillageFarmerTradeAndLootDialogs(campaignGameSystemStarter);
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x0012FA19 File Offset: 0x0012DC19
		private void AddMenus(CampaignGameStarter campaignGameSystemStarter)
		{
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0012FA1B File Offset: 0x0012DC1B
		private void take_food_confirm_forget_it_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_hostile_action");
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0012FA28 File Offset: 0x0012DC28
		public bool taking_food_from_villagers_wait_on_condition(MenuCallbackArgs args)
		{
			int skillValue = MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Roguery).GetSkillValue(DefaultSkills.Roguery);
			this._collectFoodTotalWaitHours = (float)(12 - (int)((float)skillValue / 30f));
			args.MenuContext.GameMenu.SetTargetedWaitingTimeAndInitialProgress(this._collectFoodTotalWaitHours, this._collectFoodWaitHoursProgress / this._collectFoodTotalWaitHours);
			return true;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0012FA88 File Offset: 0x0012DC88
		public bool press_into_service_confirm_on_condition(MenuCallbackArgs args)
		{
			int skillValue = MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Roguery).GetSkillValue(DefaultSkills.Roguery);
			this._collectVolunteersTotalWaitHours = (float)(24 - (int)((float)skillValue / 15f));
			args.MenuContext.GameMenu.SetTargetedWaitingTimeAndInitialProgress(this._collectVolunteersTotalWaitHours, this._collectFoodWaitHoursProgress / this._collectFoodTotalWaitHours);
			return true;
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0012FAE6 File Offset: 0x0012DCE6
		public void taking_food_from_villagers_wait_on_tick(MenuCallbackArgs args, CampaignTime campaignTime)
		{
			this._collectFoodWaitHoursProgress += (float)campaignTime.ToHours;
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(this._collectFoodWaitHoursProgress / this._collectFoodTotalWaitHours);
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0012FB1A File Offset: 0x0012DD1A
		public void press_into_service_confirm_on_tick(MenuCallbackArgs args, CampaignTime campaignTime)
		{
			this._collectVolunteerWaitHoursProgress += (float)campaignTime.ToHours;
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(this._collectVolunteerWaitHoursProgress / this._collectVolunteersTotalWaitHours);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0012FB4E File Offset: 0x0012DD4E
		public void taking_food_from_villagers_wait_on_consequence(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			GameMenu.ActivateGameMenu("menu_village_take_food_success");
			ChangeVillageStateAction.ApplyBySettingToNormal(MobileParty.MainParty.CurrentSettlement);
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0012FB74 File Offset: 0x0012DD74
		private void press_into_service_confirm_on_consequence(MenuCallbackArgs args)
		{
			Village village = Settlement.CurrentSettlement.Village;
			GameMenu.ActivateGameMenu("menu_press_into_service_success");
			ChangeVillageStateAction.ApplyBySettingToNormal(MobileParty.MainParty.CurrentSettlement);
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0012FB9C File Offset: 0x0012DD9C
		private void AddVillageFarmerTradeAndLootDialogs(CampaignGameStarter starter)
		{
			starter.AddDialogLine("village_farmer_talk_start", "start", "village_farmer_talk", "{=ddymPMWg}{VILLAGER_GREETING}", new ConversationSentence.OnConditionDelegate(this.village_farmer_talk_start_on_condition), null, 100, null);
			starter.AddDialogLine("village_farmer_pretalk_start", "village_farmer_pretalk", "village_farmer_talk", "{=cZjaGL9R}Is there anything else I can do it for you?", null, null, 100, null);
			starter.AddPlayerLine("village_farmer_buy_products", "village_farmer_talk", "village_farmer_player_trade", "{=r46NWboa}I'm going to market too. What kind of products do you have?", new ConversationSentence.OnConditionDelegate(this.village_farmer_buy_products_on_condition), null, 100, null, null);
			starter.AddDialogLine("village_farmer_specify_products", "village_farmer_player_trade", "player_trade_decision", "{=BxazyNwY}We have {PRODUCTS}. We can let you have them for {TOTAL_PRICE}{GOLD_ICON}.", null, null, 100, null);
			starter.AddPlayerLine("player_decided_to_buy", "player_trade_decision", "close_window", "{=HQ6hyVNH}All right. Here is {TOTAL_PRICE}{GOLD_ICON}.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_player_decided_to_buy_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_not_to_buy", "player_trade_decision", "village_farmer_pretalk", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			starter.AddPlayerLine("village_farmer_loot", "village_farmer_talk", "village_farmer_loot_talk", "{=XaPMUJV0}Whatever you have, I'm taking it. Surrender or die!", new ConversationSentence.OnConditionDelegate(this.village_farmer_loot_on_condition), null, 100, new ConversationSentence.OnClickableConditionDelegate(this.village_farmer_loot_on_clickable_condition), null);
			starter.AddDialogLine("village_farmer_fight", "village_farmer_loot_talk", "village_farmer_do_not_bribe", "{=ctEEfvsk}What? We're not warriors, but I bet we can take you. If you want our goods, you'll have to fight us![rf:idle_angry][ib:aggressive]", new ConversationSentence.OnConditionDelegate(this.conversation_village_farmer_not_bribe_on_condition), null, 100, null);
			starter.AddPlayerLine("village_farmer_leave", "village_farmer_talk", "close_window", "{=1IJouNaM}Carry on, then. Farewell.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_leave_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_to_fight_villagers", "village_farmer_do_not_bribe", "close_window", "{=1r0tDsrR}Attack!", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_fight_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_to_not_fight_villagers_1", "village_farmer_do_not_bribe", "close_window", "{=D33fIGQe}Never mind.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("village_farmer_accepted_to_give_some_goods", "village_farmer_loot_talk", "village_farmer_give_some_goods", "{=dMc3SjOK}We can pay you. {TAKE_MONEY_AND_PRODUCT_STRING}[rf:idle_angry][ib:nervous]", new ConversationSentence.OnConditionDelegate(this.conversation_village_farmer_give_goods_on_condition), null, 100, null);
			starter.AddPlayerLine("player_decided_to_take_some_goods_villagers", "village_farmer_give_some_goods", "village_farmer_end_talk", "{=VT1hSCaw}All right.", null, null, 100, null, null);
			starter.AddPlayerLine("player_decided_to_take_everything_villagers", "village_farmer_give_some_goods", "player_wants_everything_villagers", "{=VpGjkNrF}I want everything.", null, null, 100, null, null);
			starter.AddPlayerLine("player_decided_to_not_fight_villagers_2", "village_farmer_give_some_goods", "close_window", "{=D33fIGQe}Never mind.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_leave_on_consequence), 100, null, null);
			starter.AddDialogLine("village_farmer_fight_no_surrender", "player_wants_everything_villagers", "close_window", "{=wAhXFoNH}You'll have to fight us first![rf:idle_angry][ib:aggressive]", new ConversationSentence.OnConditionDelegate(this.conversation_village_farmer_not_surrender_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_fight_on_consequence), 100, null);
			starter.AddDialogLine("village_farmer_accepted_to_give_everything", "player_wants_everything_villagers", "player_decision_to_take_prisoner_villagers", "{=33mKghKQ}Please don't kill us. We surrender.[rf:idle_angry][ib:nervous]", new ConversationSentence.OnConditionDelegate(this.conversation_village_farmer_give_goods_on_condition), null, 100, null);
			starter.AddPlayerLine("player_do_not_take_prisoner_villagers", "player_decision_to_take_prisoner_villagers", "village_farmer_end_talk_surrender", "{=6kaia5qP}Give me all your wares!", null, null, 100, null, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner_2", "player_decision_to_take_prisoner_villagers", "villager_taken_prisoner_warning", "{=g5G8AJ5n}You are my prisoner now.", null, null, 100, null, null);
			starter.AddDialogLine("villager_warn_player_to_take_prisoner", "villager_taken_prisoner_warning", "villager_taken_prisoner_warning_answer", "{=dPOOmYGQ}You think the lords and warriors of the {KINGDOM} won't just stand by idly when their people are kidnapped? You'd best let us go!", new ConversationSentence.OnConditionDelegate(this.conversation_warn_player_on_condition), null, 100, null);
			starter.AddDialogLine("villager_warn_player_to_take_prisoner_2", "villager_taken_prisoner_warning", "close_window", "{=BvytaDUJ}Heaven protect us from the likes of you.", null, delegate()
			{
				Campaign.Current.ConversationManager.ConversationEndOneShot += this.conversation_village_farmer_took_prisoner_on_consequence;
			}, 100, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner_continue_2", "villager_taken_prisoner_warning_answer", "close_window", "{=Dfl5WJfN}Enough talking. Now march.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_took_prisoner_on_consequence), 100, null, null);
			starter.AddPlayerLine("player_decided_to_take_prisoner_leave_2", "villager_taken_prisoner_warning_answer", "village_farmer_loot_talk", "{=BNb88lyN}Never mind. Go on your way.", null, null, 100, null, null);
			starter.AddDialogLine("village_farmer_bribery_leave", "village_farmer_end_talk", "close_window", "{=Pa1ZtapI}Okay. Okay then. We're going.", new ConversationSentence.OnConditionDelegate(this.conversation_village_farmer_looted_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_looted_leave_on_consequence), 100, null);
			starter.AddDialogLine("village_farmer_surrender_leave", "village_farmer_end_talk_surrender", "close_window", "{=Pa1ZtapI}Okay. Okay then. We're going.", new ConversationSentence.OnConditionDelegate(this.conversation_village_farmer_looted_leave_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_village_farmer_surrender_leave_on_consequence), 100, null);
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0012FFC0 File Offset: 0x0012E1C0
		private bool village_farmer_loot_on_clickable_condition(out TextObject explanation)
		{
			if (this._lootedVillagers.ContainsKey(MobileParty.ConversationParty))
			{
				explanation = new TextObject("{=PVPBqy1e}You just looted these people.", null);
				return false;
			}
			explanation = TextObject.Empty;
			int num;
			ItemRoster source;
			this.CalculateConversationPartyBribeAmount(out num, out source);
			bool flag = num > 0;
			bool flag2 = !source.IsEmpty<ItemRosterElement>();
			if (!flag && !flag2)
			{
				explanation = new TextObject("{=pbRwAjUN}They seem to have no valuable goods.", null);
				return false;
			}
			return true;
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x00130024 File Offset: 0x0012E224
		private bool village_farmer_talk_start_on_condition()
		{
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (PlayerEncounter.Current != null && Campaign.Current.CurrentConversationContext == ConversationContext.PartyEncounter && encounteredParty.IsMobile && encounteredParty.MobileParty.IsVillager)
			{
				VillagerCampaignBehavior.PlayerInteraction playerInteraction = this.GetPlayerInteraction(encounteredParty.MobileParty);
				if (playerInteraction == VillagerCampaignBehavior.PlayerInteraction.None)
				{
					MBTextManager.SetTextVariable("VILLAGE", encounteredParty.MobileParty.HomeSettlement.EncyclopediaLinkWithName, false);
					Settlement settlement;
					if (encounteredParty.MobileParty.HomeSettlement.Village.TradeBound != null)
					{
						settlement = encounteredParty.MobileParty.HomeSettlement.Village.TradeBound;
					}
					else if (encounteredParty.MobileParty.LastVisitedSettlement != null && encounteredParty.MobileParty.LastVisitedSettlement.IsTown)
					{
						settlement = encounteredParty.MobileParty.LastVisitedSettlement;
					}
					else
					{
						settlement = SettlementHelper.FindNearestTown(null, encounteredParty.MobileParty.HomeSettlement);
					}
					MBTextManager.SetTextVariable("TOWN", settlement.EncyclopediaLinkWithName, false);
					if (encounteredParty.MobileParty.DefaultBehavior == AiBehavior.GoToSettlement && encounteredParty.MobileParty.TargetSettlement.IsTown)
					{
						MBTextManager.SetTextVariable("VILLAGER_STATE", GameTexts.FindText("str_villager_goes_to_town", null), false);
					}
					else
					{
						MBTextManager.SetTextVariable("VILLAGER_STATE", (encounteredParty.MobileParty.PartyTradeGold > 0) ? GameTexts.FindText("str_villager_returns_to_village", null) : GameTexts.FindText("str_looted_villager_returns_to_village", null), false);
					}
					MBTextManager.SetTextVariable("VILLAGER_GREETING", "{=a7NrxcAD}Greetings, my {?PLAYER.GENDER}lady{?}lord{\\?}. We're farmers from the village of {VILLAGE}. {VILLAGER_STATE}".ToString(), false);
				}
				else if (playerInteraction == VillagerCampaignBehavior.PlayerInteraction.Hostile)
				{
					MBTextManager.SetTextVariable("VILLAGER_GREETING", "{=L7AN6ybY}What do you want with us now?", false);
				}
				else if (playerInteraction == VillagerCampaignBehavior.PlayerInteraction.Friendly)
				{
					MBTextManager.SetTextVariable("VILLAGER_GREETING", "{=5Mu1cdbc}Greetings, once again. How may we help you?", false);
				}
				if (playerInteraction == VillagerCampaignBehavior.PlayerInteraction.None)
				{
					this.SetPlayerInteraction(encounteredParty.MobileParty, VillagerCampaignBehavior.PlayerInteraction.Friendly);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x001301D6 File Offset: 0x0012E3D6
		private bool village_farmer_loot_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsVillager && MobileParty.ConversationParty.Party.MapFaction != Hero.MainHero.MapFaction;
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0013020B File Offset: 0x0012E40B
		private void conversation_village_farmer_leave_on_consequence()
		{
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x00130214 File Offset: 0x0012E414
		private bool village_farmer_buy_products_on_condition()
		{
			bool flag = true;
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if (!encounteredParty.MobileParty.IsVillager || encounteredParty.ItemRoster.IsEmpty<ItemRosterElement>())
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < encounteredParty.ItemRoster.Count; i++)
			{
				ItemRosterElement elementCopyAtIndex = encounteredParty.ItemRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.EquipmentElement.Item.ItemCategory != DefaultItemCategories.PackAnimal)
				{
					int num2 = encounteredParty.MobileParty.HomeSettlement.Village.GetItemPrice(elementCopyAtIndex.EquipmentElement, MobileParty.MainParty, true);
					int num3 = encounteredParty.MobileParty.HomeSettlement.Village.GetItemPrice(elementCopyAtIndex.EquipmentElement, MobileParty.MainParty, true);
					if (MobileParty.MainParty.HasPerk(DefaultPerks.Trade.SilverTongue, true))
					{
						num2 = MathF.Ceiling((float)num2 * (1f - DefaultPerks.Trade.SilverTongue.SecondaryBonus));
						num3 = MathF.Ceiling((float)num3 * (1f - DefaultPerks.Trade.SilverTongue.SecondaryBonus));
					}
					int elementNumber = encounteredParty.ItemRoster.GetElementNumber(i);
					num += num3 * elementNumber;
					MBTextManager.SetTextVariable("NUMBER_OF", elementNumber);
					MBTextManager.SetTextVariable("ITEM", elementCopyAtIndex.EquipmentElement.Item.Name, false);
					MBTextManager.SetTextVariable("AMOUNT", num2);
					if (flag)
					{
						MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_number_of_item_and_price", null).ToString(), false);
						flag = false;
					}
					else
					{
						MBTextManager.SetTextVariable("RIGHT", GameTexts.FindText("str_number_of_item_and_price", null).ToString(), false);
						MBTextManager.SetTextVariable("LEFT", GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString(), false);
					}
				}
			}
			if (Hero.MainHero.Gold >= num && num > 0)
			{
				MBTextManager.SetTextVariable("PRODUCTS", GameTexts.FindText("str_LEFT_ONLY", null).ToString(), false);
				MBTextManager.SetTextVariable("TOTAL_PRICE", num);
				return true;
			}
			return false;
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x00130408 File Offset: 0x0012E608
		private void conversation_player_decided_to_buy_on_consequence()
		{
			if (MobileParty.ConversationParty.IsVillager && MobileParty.ConversationParty.ItemRoster.Count > 0)
			{
				for (int i = MobileParty.ConversationParty.ItemRoster.Count - 1; i >= 0; i--)
				{
					ItemRosterElement elementCopyAtIndex = MobileParty.ConversationParty.ItemRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.EquipmentElement.Item.ItemCategory != DefaultItemCategories.PackAnimal)
					{
						int itemPrice = MobileParty.ConversationParty.HomeSettlement.Village.GetItemPrice(elementCopyAtIndex.EquipmentElement, MobileParty.MainParty, true);
						int elementNumber = MobileParty.ConversationParty.ItemRoster.GetElementNumber(i);
						int num = itemPrice * elementNumber;
						if (elementNumber > 0)
						{
							GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, num, false);
							MobileParty.ConversationParty.PartyTradeGold += num;
							PartyBase.MainParty.ItemRoster.AddToCounts(MobileParty.ConversationParty.ItemRoster.GetElementCopyAtIndex(i).EquipmentElement, elementNumber);
							MobileParty.ConversationParty.ItemRoster.AddToCounts(MobileParty.ConversationParty.ItemRoster.GetElementCopyAtIndex(i).EquipmentElement, -1 * elementNumber);
						}
					}
				}
			}
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x00130541 File Offset: 0x0012E741
		private bool conversation_village_farmer_not_bribe_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsVillager && !this.IsBribeFeasible(MobileParty.ConversationParty);
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x00130566 File Offset: 0x0012E766
		private bool conversation_village_farmer_not_surrender_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsVillager && !this.IsSurrenderFeasible(MobileParty.ConversationParty);
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x0013058B File Offset: 0x0012E78B
		private bool conversation_village_farmer_looted_leave_on_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsVillager;
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x001305A0 File Offset: 0x0012E7A0
		private bool conversation_warn_player_on_condition()
		{
			IFaction mapFaction = MobileParty.ConversationParty.MapFaction;
			MBTextManager.SetTextVariable("KINGDOM", mapFaction.InformalName, false);
			return !MobileParty.MainParty.MapFaction.IsAtWarWith(MobileParty.ConversationParty.MapFaction);
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x001305E8 File Offset: 0x0012E7E8
		private void conversation_village_farmer_took_prisoner_on_consequence()
		{
			ItemRoster itemRoster = new ItemRoster(PlayerEncounter.EncounteredParty.ItemRoster);
			if (itemRoster.Count > 0)
			{
				InventoryManager.OpenScreenAsLoot(new Dictionary<PartyBase, ItemRoster>
				{
					{
						PartyBase.MainParty,
						itemRoster
					}
				});
				PlayerEncounter.EncounteredParty.ItemRoster.Clear();
			}
			int partyTradeGold = PlayerEncounter.EncounteredParty.MobileParty.PartyTradeGold;
			if (partyTradeGold > 0)
			{
				GiveGoldAction.ApplyForPartyToCharacter(PlayerEncounter.EncounteredParty, Hero.MainHero, partyTradeGold, false);
			}
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, PlayerEncounter.EncounteredParty);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopRosterElement troopRosterElement in PlayerEncounter.EncounteredParty.MemberRoster.GetTroopRoster())
			{
				troopRoster.AddToCounts(troopRosterElement.Character, troopRosterElement.Number, false, 0, 0, true, -1);
			}
			PartyScreenManager.OpenScreenAsLoot(TroopRoster.CreateDummyTroopRoster(), troopRoster, PlayerEncounter.EncounteredParty.Name, troopRoster.TotalManCount, null);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, PlayerEncounter.EncounteredParty.MobileParty, itemRoster, false);
			DestroyPartyAction.Apply(MobileParty.MainParty.Party, PlayerEncounter.EncounteredParty.MobileParty);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x00130724 File Offset: 0x0012E924
		private void conversation_village_farmer_fight_on_consequence()
		{
			PlayerEncounter.Current.IsEnemy = true;
			this.SetPlayerInteraction(MobileParty.ConversationParty, VillagerCampaignBehavior.PlayerInteraction.Hostile);
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x00130740 File Offset: 0x0012E940
		private bool conversation_village_farmer_give_goods_on_condition()
		{
			int num;
			ItemRoster itemRoster;
			this.CalculateConversationPartyBribeAmount(out num, out itemRoster);
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
			return true;
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x00130A30 File Offset: 0x0012EC30
		private void conversation_village_farmer_looted_leave_on_consequence()
		{
			int amount;
			ItemRoster itemRoster;
			this.CalculateConversationPartyBribeAmount(out amount, out itemRoster);
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
			this.SetPlayerInteraction(MobileParty.ConversationParty, VillagerCampaignBehavior.PlayerInteraction.Hostile);
			this._lootedVillagers.Add(MobileParty.ConversationParty, CampaignTime.Now);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, MobileParty.ConversationParty, itemRoster, false);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x00130AEC File Offset: 0x0012ECEC
		private void conversation_village_farmer_surrender_leave_on_consequence()
		{
			ItemRoster itemRoster = new ItemRoster(MobileParty.ConversationParty.ItemRoster);
			if (itemRoster.Count > 0)
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
			int partyTradeGold = MobileParty.ConversationParty.PartyTradeGold;
			if (partyTradeGold > 0)
			{
				GiveGoldAction.ApplyForPartyToCharacter(MobileParty.ConversationParty.Party, Hero.MainHero, partyTradeGold, false);
			}
			BeHostileAction.ApplyMajorCoercionHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
			this._lootedVillagers.Add(MobileParty.ConversationParty, CampaignTime.Now);
			SkillLevelingManager.OnLoot(MobileParty.MainParty, MobileParty.ConversationParty, itemRoster, false);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x00130BA0 File Offset: 0x0012EDA0
		private bool IsBribeFeasible(MobileParty conversationParty)
		{
			int num = PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.05f) ? 33 : 67;
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num = MathF.Round((float)num * (1f + DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return conversationParty.Party.RandomIntWithSeed(3U, 100) <= 100 - num && PartyBaseHelper.DoesSurrenderIsLogicalForParty(conversationParty, MobileParty.MainParty, 0.4f);
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x00130C1C File Offset: 0x0012EE1C
		private bool IsSurrenderFeasible(MobileParty conversationParty)
		{
			int num = PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.05f) ? 33 : 67;
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num = MathF.Round((float)num * (1f + DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return conversationParty.Party.RandomIntWithSeed(4U, 100) <= 100 - num && PartyBaseHelper.DoesSurrenderIsLogicalForParty(conversationParty, MobileParty.MainParty, 0.1f);
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x00130C98 File Offset: 0x0012EE98
		private void CalculateConversationPartyBribeAmount(out int gold, out ItemRoster items)
		{
			int num = 0;
			ItemRoster itemRoster = new ItemRoster();
			bool flag = false;
			for (int i = 0; i < MobileParty.ConversationParty.ItemRoster.Count; i++)
			{
				num += MobileParty.ConversationParty.ItemRoster.GetElementUnitCost(i) * MobileParty.ConversationParty.ItemRoster.GetElementNumber(i);
				if (!flag && MobileParty.ConversationParty.ItemRoster.GetElementNumber(i) > 0)
				{
					flag = true;
				}
			}
			num += MobileParty.ConversationParty.PartyTradeGold;
			int num2 = MathF.Min((int)((float)num * 0.2f), 2000);
			if (MobileParty.MainParty.HasPerk(DefaultPerks.Roguery.SaltTheEarth, false))
			{
				num2 = MathF.Round((float)num2 * (1f + DefaultPerks.Roguery.SaltTheEarth.PrimaryBonus));
			}
			int num3 = MathF.Min(MobileParty.ConversationParty.PartyTradeGold, num2);
			if (num3 < num2 && flag)
			{
				ItemRoster itemRoster2 = new ItemRoster(MobileParty.ConversationParty.ItemRoster);
				int num4 = 0;
				while (num3 + num4 < num2)
				{
					ItemRosterElement randomElement = itemRoster2.GetRandomElement<ItemRosterElement>();
					num4 += randomElement.EquipmentElement.ItemValue;
					EquipmentElement rosterElement = new EquipmentElement(randomElement.EquipmentElement.Item, randomElement.EquipmentElement.ItemModifier, null, false);
					itemRoster.AddToCounts(rosterElement, 1);
					itemRoster2.AddToCounts(rosterElement, -1);
					if (itemRoster2.IsEmpty<ItemRosterElement>())
					{
						break;
					}
				}
			}
			gold = num3;
			items = itemRoster;
		}

		// Token: 0x0400124F RID: 4687
		private const int MinimumNumberOfVillagersAtVillagerParty = 12;

		// Token: 0x04001250 RID: 4688
		private const int OneVillagerPerHearth = 40;

		// Token: 0x04001251 RID: 4689
		private float _collectFoodTotalWaitHours;

		// Token: 0x04001252 RID: 4690
		private float _collectVolunteersTotalWaitHours;

		// Token: 0x04001253 RID: 4691
		private float _collectFoodWaitHoursProgress;

		// Token: 0x04001254 RID: 4692
		private float _collectVolunteerWaitHoursProgress;

		// Token: 0x04001255 RID: 4693
		private Dictionary<MobileParty, CampaignTime> _lootedVillagers = new Dictionary<MobileParty, CampaignTime>();

		// Token: 0x04001256 RID: 4694
		private Dictionary<MobileParty, VillagerCampaignBehavior.PlayerInteraction> _interactedVillagers = new Dictionary<MobileParty, VillagerCampaignBehavior.PlayerInteraction>();

		// Token: 0x04001257 RID: 4695
		private Dictionary<Village, CampaignTime> _villageLastVillagerSendTime = new Dictionary<Village, CampaignTime>();

		// Token: 0x04001258 RID: 4696
		private Dictionary<MobileParty, List<Settlement>> _previouslyChangedVillagerTargetsDueToEnemyOnWay = new Dictionary<MobileParty, List<Settlement>>();

		// Token: 0x02000751 RID: 1873
		public class VillagerCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060059C0 RID: 22976 RVA: 0x00184724 File Offset: 0x00182924
			public VillagerCampaignBehaviorTypeDefiner() : base(140000)
			{
			}

			// Token: 0x060059C1 RID: 22977 RVA: 0x00184731 File Offset: 0x00182931
			protected override void DefineEnumTypes()
			{
				base.AddEnumDefinition(typeof(VillagerCampaignBehavior.PlayerInteraction), 1, null);
			}

			// Token: 0x060059C2 RID: 22978 RVA: 0x00184745 File Offset: 0x00182945
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<MobileParty, VillagerCampaignBehavior.PlayerInteraction>));
			}
		}

		// Token: 0x02000752 RID: 1874
		private enum PlayerInteraction
		{
			// Token: 0x04001EC7 RID: 7879
			None,
			// Token: 0x04001EC8 RID: 7880
			Friendly,
			// Token: 0x04001EC9 RID: 7881
			TradedWith,
			// Token: 0x04001ECA RID: 7882
			Hostile
		}
	}
}
