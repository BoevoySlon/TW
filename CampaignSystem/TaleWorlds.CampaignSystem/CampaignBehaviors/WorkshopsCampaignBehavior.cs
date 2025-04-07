using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E6 RID: 998
	public class WorkshopsCampaignBehavior : CampaignBehaviorBase, IWorkshopWarehouseCampaignBehavior
	{
		// Token: 0x06003E32 RID: 15922 RVA: 0x001310A4 File Offset: 0x0012F2A4
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.OnAfterSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterSessionLaunched));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, new Action<Town>(this.DailyTickTown));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
			CampaignEvents.WorkshopOwnerChangedEvent.AddNonSerializedListener(this, new Action<Workshop, Hero>(this.OnWorkshopOwnerChanged));
			CampaignEvents.WorkshopTypeChangedEvent.AddNonSerializedListener(this, new Action<Workshop>(this.OnWorkshopTypeChanged));
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00131197 File Offset: 0x0012F397
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<KeyValuePair<Settlement, ItemRoster>[]>("_warehouseRosterPerSettlement", ref this._warehouseRosterPerSettlement);
			dataStore.SyncData<WorkshopsCampaignBehavior.WorkshopData[]>("_workshopData", ref this._workshopData);
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x001311BD File Offset: 0x0012F3BD
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i >= 10)
			{
				if (i == 10)
				{
					this.InitializeBehaviorData();
					this.FillItemsInAllCategories();
					this.InitializeWorkshops();
					this.BuildWorkshopsAtGameStart();
				}
				if (i % 20 == 0)
				{
					this.RunTownShopsAtGameStart();
				}
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x001311F0 File Offset: 0x0012F3F0
		private void InitializeBehaviorData()
		{
			if (this._workshopData == null)
			{
				this._workshopData = new WorkshopsCampaignBehavior.WorkshopData[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
			}
			if (this._warehouseRosterPerSettlement == null)
			{
				this._warehouseRosterPerSettlement = new KeyValuePair<Settlement, ItemRoster>[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
			}
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x0013124C File Offset: 0x0012F44C
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeBehaviorData();
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 66233))
			{
				foreach (Workshop workshop in Hero.MainHero.OwnedWorkshops)
				{
					this.AddNewWorkshopData(workshop);
					this.AddNewWarehouseDataIfNeeded(workshop.Settlement);
				}
			}
			this.EnsureBehaviorDataSize();
			this.FillItemsInAllCategories();
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x001312E4 File Offset: 0x0012F4E4
		private void EnsureBehaviorDataSize()
		{
			if (this._workshopData.Length < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave)
			{
				WorkshopsCampaignBehavior.WorkshopData[] array = new WorkshopsCampaignBehavior.WorkshopData[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
				for (int i = 0; i < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave; i++)
				{
					if (i < this._workshopData.Length)
					{
						array[i] = this._workshopData[i];
					}
					else
					{
						array[i] = null;
					}
				}
				this._workshopData = array;
			}
			if (this._warehouseRosterPerSettlement.Length < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave)
			{
				KeyValuePair<Settlement, ItemRoster>[] array2 = new KeyValuePair<Settlement, ItemRoster>[Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave];
				for (int j = 0; j < Campaign.Current.Models.WorkshopModel.MaximumWorkshopsPlayerCanHave; j++)
				{
					if (j < this._warehouseRosterPerSettlement.Length && this._warehouseRosterPerSettlement[j].Key != null && this._warehouseRosterPerSettlement[j].Value != null)
					{
						array2[j] = this._warehouseRosterPerSettlement[j];
					}
				}
				this._warehouseRosterPerSettlement = array2;
			}
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x00131410 File Offset: 0x0012F610
		private void OnWorkshopTypeChanged(Workshop workshop)
		{
			if (workshop.Owner == Hero.MainHero)
			{
				this.RemoveWorkshopData(workshop);
				this.AddNewWorkshopData(workshop);
			}
		}

		// Token: 0x06003E39 RID: 15929 RVA: 0x00131430 File Offset: 0x0012F630
		private void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
			Hero owner = workshop.Owner;
			if (owner == Hero.MainHero)
			{
				this.AddNewWarehouseDataIfNeeded(workshop.Settlement);
				this.AddNewWorkshopData(workshop);
				return;
			}
			if (oldOwner == Hero.MainHero && Clan.PlayerClan.Leader != owner)
			{
				if (Hero.MainHero.OwnedWorkshops.All((Workshop x) => x.Settlement != workshop.Settlement))
				{
					if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement == workshop.Settlement)
					{
						this.TransferWarehouseToPlayerParty(Settlement.CurrentSettlement);
					}
					this.RemoveWarehouseData(workshop.Settlement);
				}
				this.RemoveWorkshopData(workshop);
			}
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x001314F0 File Offset: 0x0012F6F0
		private void DailyTickTown(Town town)
		{
			if (!town.InRebelliousState)
			{
				foreach (Workshop workshop in town.Workshops)
				{
					this.RunTownWorkshop(town, workshop);
					this.HandleDailyExpense(workshop);
				}
			}
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x00131530 File Offset: 0x0012F730
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (!victim.IsHumanPlayerCharacter)
			{
				foreach (Workshop workshop in victim.OwnedWorkshops.ToList<Workshop>())
				{
					Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
					ChangeOwnerOfWorkshopAction.ApplyByDeath(workshop, notableOwnerForWorkshop);
				}
			}
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x001315A8 File Offset: 0x0012F7A8
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			this.TransferPlayerWorkshopsIfNeeded();
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x001315B0 File Offset: 0x0012F7B0
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			this.TransferPlayerWorkshopsIfNeeded();
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x001315B8 File Offset: 0x0012F7B8
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newSettlementOwner, Hero oldSettlementOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.IsTown)
			{
				foreach (Workshop workshop in settlement.Town.Workshops)
				{
					if (workshop.Owner != null && workshop.Owner.MapFaction.IsAtWarWith(newSettlementOwner.MapFaction) && workshop.Owner.GetPerkValue(DefaultPerks.Trade.RapidDevelopment))
					{
						GiveGoldAction.ApplyBetweenCharacters(null, workshop.Owner, MathF.Round(DefaultPerks.Trade.RapidDevelopment.PrimaryBonus), false);
					}
				}
				this.TransferPlayerWorkshopsIfNeeded();
			}
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x0013163F File Offset: 0x0012F83F
		private void OnAfterSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeGameMenus(campaignGameStarter);
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x00131648 File Offset: 0x0012F848
		protected void InitializeGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenuOption("town", "manage_warehouse", "{=LK4kNZkb}Enter the warehouse", new GameMenuOption.OnConditionDelegate(this.warehouse_manage_on_condition), new GameMenuOption.OnConsequenceDelegate(this.warehouse_manage_on_consequence), false, 8, false, null);
			campaignGameStarter.AddPlayerLine("workshop_worker_manage_warehouse", "player_options", "warehouse", "{=mBnoWa8R}I would like to access the Warehouse.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("workshop_worker_manage_warehouse_answer", "warehouse", "player_options", "{=Y4LhmAdi}Sure, boss. Go ahead.", null, new ConversationSentence.OnConsequenceDelegate(this.warehouse_manage_on_consequence), 100, null);
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x001316D4 File Offset: 0x0012F8D4
		private void warehouse_manage_on_consequence()
		{
			InventoryLogic.CapacityData otherSideCapacity = new InventoryLogic.CapacityData(new Func<int>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityDelegate|20_0), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededWarningDelegate|20_1), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededHintDelegate|20_2));
			InventoryManager.OpenScreenAsWarehouse(this.GetWarehouseRoster(Settlement.CurrentSettlement), otherSideCapacity);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00131738 File Offset: 0x0012F938
		private void warehouse_manage_on_consequence(MenuCallbackArgs args)
		{
			InventoryLogic.CapacityData otherSideCapacity = new InventoryLogic.CapacityData(new Func<int>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityDelegate|21_0), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededWarningDelegate|21_1), new Func<TextObject>(WorkshopsCampaignBehavior.<>c.<>9.<warehouse_manage_on_consequence>g__CapacityExceededHintDelegate|21_2));
			InventoryManager.OpenScreenAsWarehouse(this.GetWarehouseRoster(Settlement.CurrentSettlement), otherSideCapacity);
		}

		// Token: 0x06003E43 RID: 15939 RVA: 0x0013178C File Offset: 0x0012F98C
		private bool warehouse_manage_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Warehouse;
			return this.GetWarehouseRoster(Settlement.CurrentSettlement) != null;
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x001317A4 File Offset: 0x0012F9A4
		bool IWorkshopWarehouseCampaignBehavior.IsGettingInputsFromWarehouse(Workshop workshop)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			return dataOfWorkshop != null && dataOfWorkshop.IsGettingInputsFromWarehouse;
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x001317C4 File Offset: 0x0012F9C4
		void IWorkshopWarehouseCampaignBehavior.SetIsGettingInputsFromWarehouse(Workshop workshop, bool isActive)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			if (dataOfWorkshop != null)
			{
				dataOfWorkshop.IsGettingInputsFromWarehouse = isActive;
			}
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x001317E4 File Offset: 0x0012F9E4
		float IWorkshopWarehouseCampaignBehavior.GetStockProductionInWarehouseRatio(Workshop workshop)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			if (dataOfWorkshop != null)
			{
				return dataOfWorkshop.StockProductionInWarehouseRatio;
			}
			return 0f;
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x00131808 File Offset: 0x0012FA08
		void IWorkshopWarehouseCampaignBehavior.SetStockProductionInWarehouseRatio(Workshop workshop, float ratio)
		{
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			if (dataOfWorkshop != null)
			{
				dataOfWorkshop.StockProductionInWarehouseRatio = ratio;
			}
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x00131828 File Offset: 0x0012FA28
		int IWorkshopWarehouseCampaignBehavior.GetInputCount(Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			int num = 0;
			List<ItemCategory> list = new List<ItemCategory>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
				{
					ItemCategory item = valueTuple.Item1;
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			foreach (ItemCategory itemCategory in list)
			{
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == itemCategory)
					{
						num += itemRosterElement.Amount;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00131978 File Offset: 0x0012FB78
		ExplainedNumber IWorkshopWarehouseCampaignBehavior.GetInputDailyChange(Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			ExplainedNumber result = new ExplainedNumber(0f, true, null);
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
				{
					float num = Campaign.Current.Models.WorkshopModel.GetEffectiveConversionSpeedOfProduction(workshop, production.ConversionSpeed, false).ResultNumber * (float)valueTuple.Item2;
					ItemCategory item = valueTuple.Item1;
					float num2;
					if (!dictionary.TryGetValue(item, out num2))
					{
						dictionary.Add(item, num);
					}
					else
					{
						dictionary[item] = num2 + num;
					}
				}
			}
			foreach (KeyValuePair<ItemCategory, float> keyValuePair in dictionary)
			{
				int num3 = 0;
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == keyValuePair.Key)
					{
						num3 += itemRosterElement.Amount;
					}
				}
				if (num3 > 0)
				{
					TextObject textObject = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
					textObject.SetTextVariable("RANK", keyValuePair.Key.GetName());
					textObject.SetTextVariable("NUMBER", num3);
					result.Add(-keyValuePair.Value, textObject, null);
				}
			}
			return result;
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00131B7C File Offset: 0x0012FD7C
		int IWorkshopWarehouseCampaignBehavior.GetOutputCount(Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			int num = 0;
			List<ItemCategory> list = new List<ItemCategory>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Outputs)
				{
					ItemCategory item = valueTuple.Item1;
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			foreach (ItemCategory itemCategory in list)
			{
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == itemCategory)
					{
						num += itemRosterElement.Amount;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x00131CCC File Offset: 0x0012FECC
		ExplainedNumber IWorkshopWarehouseCampaignBehavior.GetOutputDailyChange(Workshop workshop)
		{
			ExplainedNumber result = new ExplainedNumber(0f, true, null);
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			Dictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			foreach (WorkshopType.Production production in workshop.WorkshopType.Productions)
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Outputs)
				{
					ItemCategory item = valueTuple.Item1;
					if (item.IsTradeGood)
					{
						int item2 = valueTuple.Item2;
						float num = Campaign.Current.Models.WorkshopModel.GetEffectiveConversionSpeedOfProduction(workshop, production.ConversionSpeed, false).ResultNumber * (float)item2 * this.GetDataOfWorkshop(workshop).StockProductionInWarehouseRatio;
						float num2;
						if (!dictionary.TryGetValue(item, out num2))
						{
							dictionary.Add(item, num);
						}
						else
						{
							dictionary[item] = num2 + num;
						}
					}
				}
			}
			foreach (KeyValuePair<ItemCategory, float> keyValuePair in dictionary)
			{
				int num3 = 0;
				foreach (ItemRosterElement itemRosterElement in warehouseRoster)
				{
					if (itemRosterElement.EquipmentElement.Item.ItemCategory == keyValuePair.Key)
					{
						num3 += itemRosterElement.Amount;
					}
				}
				TextObject textObject = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null);
				textObject.SetTextVariable("RANK", keyValuePair.Key.GetName());
				textObject.SetTextVariable("NUMBER", num3);
				result.Add(keyValuePair.Value, textObject, null);
			}
			return result;
		}

		// Token: 0x06003E4C RID: 15948 RVA: 0x00131EEC File Offset: 0x001300EC
		bool IWorkshopWarehouseCampaignBehavior.IsRawMaterialsSufficientInTownMarket(Workshop workshop)
		{
			for (int i = 0; i < workshop.WorkshopType.Productions.Count; i++)
			{
				WorkshopType.Production production = workshop.WorkshopType.Productions[i];
				int num;
				if (this.DetermineItemRosterHasSufficientInputs(production, workshop.Settlement.Town.Owner.ItemRoster, workshop.Settlement.Town, out num))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x00131F54 File Offset: 0x00130154
		public float GetWarehouseItemRosterWeight(Settlement settlement)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(settlement);
			float num = 0f;
			foreach (ItemRosterElement itemRosterElement in warehouseRoster)
			{
				num += itemRosterElement.GetRosterElementWeight();
			}
			return num;
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x00131FAC File Offset: 0x001301AC
		private bool TickOneProductionCycleForPlayerWorkshop(WorkshopType.Production production, Workshop workshop)
		{
			bool flag = false;
			int inputMaterialCost = 0;
			Town town = workshop.Settlement.Town;
			WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop = this.GetDataOfWorkshop(workshop);
			bool flag2 = dataOfWorkshop.IsGettingInputsFromWarehouse;
			if (flag2)
			{
				flag = this.DetermineItemRosterHasSufficientInputs(production, this.GetWarehouseRoster(workshop.Settlement), town, out inputMaterialCost);
				if (flag)
				{
					inputMaterialCost = 0;
				}
				else
				{
					flag2 = false;
				}
			}
			if (!flag)
			{
				flag = this.DetermineItemRosterHasSufficientInputs(production, town.Owner.ItemRoster, town, out inputMaterialCost);
			}
			if (flag)
			{
				int outputIncome;
				List<EquipmentElement> itemsToProduce = this.GetItemsToProduce(production, workshop, out outputIncome);
				bool flag3;
				if (!production.Inputs.Any((ValueTuple<ItemCategory, int> x) => !x.Item1.IsTradeGood))
				{
					flag3 = !production.Outputs.Any((ValueTuple<ItemCategory, int> x) => !x.Item1.IsTradeGood);
				}
				else
				{
					flag3 = false;
				}
				bool effectCapital = flag3;
				float num = dataOfWorkshop.StockProductionInWarehouseRatio;
				bool allOutputsWillBeSentToWarehouse = num.ApproximatelyEqualsTo(1f, 1E-05f);
				if (this.CanPlayerWorkshopProduceThisCycle(production, workshop, inputMaterialCost, outputIncome, effectCapital, allOutputsWillBeSentToWarehouse))
				{
					Dictionary<ItemObject, int> dictionary = new Dictionary<ItemObject, int>();
					foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
					{
						if (flag2)
						{
							this.ConsumeInputFromWarehouse(valueTuple.Item1, valueTuple.Item2, workshop);
						}
						else
						{
							this.ConsumeInputFromTownMarket(valueTuple.Item1, valueTuple.Item2, town, workshop, effectCapital);
						}
					}
					foreach (EquipmentElement equipmentElement in itemsToProduce)
					{
						WorkshopsCampaignBehavior.WorkshopData dataOfWorkshop2 = this.GetDataOfWorkshop(workshop);
						if (equipmentElement.Item.IsTradeGood && this.CanItemFitInWarehouse(workshop.Settlement, equipmentElement))
						{
							this.AddOutputProgressForWarehouse(workshop, num);
							if (dataOfWorkshop2.ProductionProgressForWarehouse >= 1f)
							{
								this.ProduceAnOutputToWarehouse(equipmentElement, workshop);
								SkillLevelingManager.OnProductionProducedToWarehouse(equipmentElement);
								this.AddOutputProgressForWarehouse(workshop, -1f);
								if (dictionary.ContainsKey(equipmentElement.Item))
								{
									Dictionary<ItemObject, int> dictionary2 = dictionary;
									ItemObject item = equipmentElement.Item;
									int num2 = dictionary2[item];
									dictionary2[item] = num2 + 1;
								}
								else
								{
									dictionary.Add(equipmentElement.Item, 1);
								}
							}
						}
						else
						{
							num = 0f;
						}
						this.AddOutputProgressForTown(workshop, 1f - num);
						if (dataOfWorkshop2.ProductionProgressForTown >= 1f)
						{
							this.ProduceAnOutputToTown(equipmentElement, workshop, effectCapital);
							this.AddOutputProgressForTown(workshop, -1f);
							if (dictionary.ContainsKey(equipmentElement.Item))
							{
								Dictionary<ItemObject, int> dictionary3 = dictionary;
								ItemObject item = equipmentElement.Item;
								int num2 = dictionary3[item];
								dictionary3[item] = num2 + 1;
							}
							else
							{
								dictionary.Add(equipmentElement.Item, 1);
							}
						}
					}
					foreach (KeyValuePair<ItemObject, int> keyValuePair in dictionary)
					{
						ItemObject key = keyValuePair.Key;
						int value = keyValuePair.Value;
						CampaignEventDispatcher.Instance.OnItemProduced(key, workshop.Settlement, value);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x00132318 File Offset: 0x00130518
		private void ProduceAnOutputToWarehouse(EquipmentElement outputItem, Workshop workshop)
		{
			this.GetWarehouseRoster(workshop.Settlement).AddToCounts(outputItem, 1);
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x00132330 File Offset: 0x00130530
		private void ConsumeInputFromWarehouse(ItemCategory productionInput, int inputCount, Workshop workshop)
		{
			ItemRoster warehouseRoster = this.GetWarehouseRoster(workshop.Settlement);
			int num = inputCount;
			for (int i = 0; i < warehouseRoster.Count; i++)
			{
				if (num == 0)
				{
					return;
				}
				ItemObject itemAtIndex = warehouseRoster.GetItemAtIndex(i);
				if (itemAtIndex.ItemCategory == productionInput)
				{
					int elementNumber = warehouseRoster.GetElementNumber(i);
					int num2 = MathF.Min(num, elementNumber);
					num -= num2;
					warehouseRoster.AddToCounts(itemAtIndex, -inputCount);
					CampaignEventDispatcher.Instance.OnItemConsumed(itemAtIndex, workshop.Settlement, inputCount);
				}
			}
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x001323A8 File Offset: 0x001305A8
		private bool CanPlayerWorkshopProduceThisCycle(WorkshopType.Production production, Workshop workshop, int inputMaterialCost, int outputIncome, bool effectCapital, bool allOutputsWillBeSentToWarehouse)
		{
			float num = workshop.WorkshopType.IsHidden ? ((float)inputMaterialCost) : ((float)inputMaterialCost + 200f / production.ConversionSpeed);
			if (Campaign.Current.GameStarted && (float)outputIncome <= num)
			{
				return false;
			}
			if (workshop.Capital < inputMaterialCost)
			{
				return false;
			}
			if (effectCapital)
			{
				bool flag = workshop.Settlement.Town.Gold >= outputIncome;
				bool flag2 = !this.IsWarehouseAtLimit(workshop.Settlement);
				if (!flag && (!allOutputsWillBeSentToWarehouse || flag2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x00132430 File Offset: 0x00130630
		private void HandlePlayerWorkshopExpense(Workshop shop)
		{
			int expense = shop.Expense;
			if (shop.Capital > Campaign.Current.Models.WorkshopModel.CapitalLowLimit)
			{
				shop.ChangeGold(-expense);
				return;
			}
			if (shop.Owner.Gold >= expense)
			{
				shop.Owner.Gold -= expense;
				return;
			}
			if (shop.Capital >= expense)
			{
				shop.ChangeGold(-expense);
				return;
			}
			this.ChangeWorkshopOwnerByBankruptcy(shop);
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x001324A4 File Offset: 0x001306A4
		private bool TickOneProductionCycleForNotableWorkshop(WorkshopType.Production production, Workshop workshop)
		{
			Town town = workshop.Settlement.Town;
			int inputMaterialCost = 0;
			if (!this.DetermineItemRosterHasSufficientInputs(production, town.Owner.ItemRoster, town, out inputMaterialCost))
			{
				return false;
			}
			int outputIncome;
			List<EquipmentElement> itemsToProduce = this.GetItemsToProduce(production, workshop, out outputIncome);
			bool flag;
			if (!production.Inputs.Any((ValueTuple<ItemCategory, int> x) => !x.Item1.IsTradeGood))
			{
				flag = !production.Outputs.Any((ValueTuple<ItemCategory, int> x) => !x.Item1.IsTradeGood);
			}
			else
			{
				flag = false;
			}
			bool effectCapital = flag;
			if (this.CanNotableWorkshopProduceThisCycle(production, workshop, inputMaterialCost, outputIncome, effectCapital))
			{
				foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
				{
					this.ConsumeInputFromTownMarket(valueTuple.Item1, valueTuple.Item2, town, workshop, effectCapital);
				}
				foreach (EquipmentElement outputItem in itemsToProduce)
				{
					this.ProduceAnOutputToTown(outputItem, workshop, effectCapital);
					CampaignEventDispatcher.Instance.OnItemProduced(outputItem.Item, workshop.Settlement, 1);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x0013260C File Offset: 0x0013080C
		private bool CanNotableWorkshopProduceThisCycle(WorkshopType.Production production, Workshop workshop, int inputMaterialCost, int outputIncome, bool effectCapital)
		{
			float num = workshop.WorkshopType.IsHidden ? ((float)inputMaterialCost) : ((float)inputMaterialCost + 200f / production.ConversionSpeed);
			return (!Campaign.Current.GameStarted || (float)outputIncome > num) && (workshop.Settlement.Town.Gold >= outputIncome || !effectCapital) && workshop.Capital >= inputMaterialCost;
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x00132678 File Offset: 0x00130878
		private void HandleNotableWorkshopExpense(Workshop shop)
		{
			int expense = shop.Expense;
			if (shop.Capital >= expense)
			{
				shop.ChangeGold(-expense);
				return;
			}
			this.ChangeWorkshopOwnerByBankruptcy(shop);
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x001326A8 File Offset: 0x001308A8
		private WorkshopsCampaignBehavior.WorkshopData GetDataOfWorkshop(Workshop workshop)
		{
			for (int i = 0; i < this._workshopData.Length; i++)
			{
				WorkshopsCampaignBehavior.WorkshopData workshopData = this._workshopData[i];
				if (workshopData != null && workshopData.Workshop == workshop)
				{
					return workshopData;
				}
			}
			return null;
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x001326E0 File Offset: 0x001308E0
		private List<EquipmentElement> GetItemsToProduce(WorkshopType.Production production, Workshop workshop, out int income)
		{
			List<EquipmentElement> list = new List<EquipmentElement>();
			income = 0;
			for (int i = 0; i < production.Outputs.Count; i++)
			{
				int item = production.Outputs[i].Item2;
				for (int j = 0; j < item; j++)
				{
					EquipmentElement randomItem = this.GetRandomItem(production.Outputs[i].Item1, workshop.Settlement.Town);
					list.Add(randomItem);
					income += workshop.Settlement.Town.GetItemPrice(randomItem, null, true);
				}
			}
			return list;
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x00132774 File Offset: 0x00130974
		private void ProduceAnOutputToTown(EquipmentElement outputItem, Workshop workshop, bool effectCapital)
		{
			Town town = workshop.Settlement.Town;
			int itemPrice = town.GetItemPrice(outputItem, null, false);
			town.Owner.ItemRoster.AddToCounts(outputItem, 1);
			if (Campaign.Current.GameStarted && effectCapital)
			{
				int num = MathF.Min(1000, itemPrice);
				workshop.ChangeGold(num);
				town.ChangeGold(-num);
			}
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x001327D4 File Offset: 0x001309D4
		private void ConsumeInputFromTownMarket(ItemCategory productionInput, int productionInputCount, Town town, Workshop workshop, bool effectCapital)
		{
			ItemRoster itemRoster = town.Owner.ItemRoster;
			int num = itemRoster.FindIndex((ItemObject x) => x.ItemCategory == productionInput);
			if (num >= 0)
			{
				ItemObject itemAtIndex = itemRoster.GetItemAtIndex(num);
				if (Campaign.Current.GameStarted && effectCapital)
				{
					int itemPrice = town.GetItemPrice(itemAtIndex, null, false);
					workshop.ChangeGold(-itemPrice);
					town.ChangeGold(itemPrice);
				}
				itemRoster.AddToCounts(itemAtIndex, -productionInputCount);
				CampaignEventDispatcher.Instance.OnItemConsumed(itemAtIndex, town.Owner.Settlement, productionInputCount);
			}
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x00132866 File Offset: 0x00130A66
		private bool IsItemPreferredForTown(ItemObject item, Town townComponent)
		{
			return item.Culture == null || item.Culture.StringId == "neutral_culture" || item.Culture == townComponent.Culture;
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x00132898 File Offset: 0x00130A98
		private bool DetermineItemRosterHasSufficientInputs(WorkshopType.Production production, ItemRoster itemRoster, Town town, out int inputMaterialCost)
		{
			List<ValueTuple<ItemCategory, int>> inputs = production.Inputs;
			inputMaterialCost = 0;
			foreach (ValueTuple<ItemCategory, int> valueTuple in inputs)
			{
				ItemCategory item = valueTuple.Item1;
				int num = valueTuple.Item2;
				for (int i = 0; i < itemRoster.Count; i++)
				{
					ItemObject itemAtIndex = itemRoster.GetItemAtIndex(i);
					if (itemAtIndex.ItemCategory == item)
					{
						int elementNumber = itemRoster.GetElementNumber(i);
						int num2 = MathF.Min(num, elementNumber);
						num -= num2;
						inputMaterialCost += town.GetItemPrice(itemAtIndex, null, false) * num2;
					}
				}
				if (num > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x00132954 File Offset: 0x00130B54
		private void AddOutputProgressForWarehouse(Workshop workshop, float progressToAdd)
		{
			this.GetDataOfWorkshop(workshop).ProductionProgressForWarehouse += progressToAdd;
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x0013296A File Offset: 0x00130B6A
		private void AddOutputProgressForTown(Workshop workshop, float progressToAdd)
		{
			this.GetDataOfWorkshop(workshop).ProductionProgressForTown += progressToAdd;
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x00132980 File Offset: 0x00130B80
		private bool CanItemFitInWarehouse(Settlement settlement, EquipmentElement equipmentElement)
		{
			return this.GetWarehouseItemRosterWeight(settlement) + equipmentElement.Weight <= (float)Campaign.Current.Models.WorkshopModel.WarehouseCapacity;
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x001329AB File Offset: 0x00130BAB
		private bool IsWarehouseAtLimit(Settlement settlement)
		{
			return this.GetWarehouseItemRosterWeight(settlement) >= (float)Campaign.Current.Models.WorkshopModel.WarehouseCapacity;
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x001329D0 File Offset: 0x00130BD0
		private void AddNewWorkshopData(Workshop workshop)
		{
			for (int i = 0; i < this._workshopData.Length; i++)
			{
				if (this._workshopData[i] == null)
				{
					this._workshopData[i] = new WorkshopsCampaignBehavior.WorkshopData(workshop);
					return;
				}
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x00132A0C File Offset: 0x00130C0C
		private void RemoveWorkshopData(Workshop workshop)
		{
			for (int i = 0; i < this._workshopData.Length; i++)
			{
				WorkshopsCampaignBehavior.WorkshopData workshopData = this._workshopData[i];
				if (workshopData != null && workshopData.Workshop == workshop)
				{
					this._workshopData[i] = null;
					return;
				}
			}
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00132A4C File Offset: 0x00130C4C
		private ItemRoster GetWarehouseRoster(Settlement settlement)
		{
			foreach (KeyValuePair<Settlement, ItemRoster> keyValuePair in this._warehouseRosterPerSettlement)
			{
				if (keyValuePair.Key == settlement)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00132A8C File Offset: 0x00130C8C
		private void FillItemsInAllCategories()
		{
			foreach (ItemObject itemObject in Game.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (this.IsProducable(itemObject))
				{
					ItemCategory itemCategory = itemObject.ItemCategory;
					if (itemCategory != null)
					{
						List<ItemObject> list;
						if (!this._itemsInCategory.TryGetValue(itemCategory, out list))
						{
							list = new List<ItemObject>();
							this._itemsInCategory[itemCategory] = list;
						}
						list.Add(itemObject);
					}
				}
			}
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x00132B20 File Offset: 0x00130D20
		private bool IsProducable(ItemObject item)
		{
			return !item.MultiplayerItem && !item.NotMerchandise && !item.IsCraftedByPlayer;
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00132B40 File Offset: 0x00130D40
		private void RemoveWarehouseData(Settlement settlement)
		{
			for (int i = 0; i < this._warehouseRosterPerSettlement.Length; i++)
			{
				if (this._warehouseRosterPerSettlement[i].Key == settlement)
				{
					this._warehouseRosterPerSettlement[i] = new KeyValuePair<Settlement, ItemRoster>(null, null);
					return;
				}
			}
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00132B88 File Offset: 0x00130D88
		private void AddNewWarehouseDataIfNeeded(Settlement settlement)
		{
			bool flag = false;
			foreach (KeyValuePair<Settlement, ItemRoster> keyValuePair in this._warehouseRosterPerSettlement)
			{
				if (keyValuePair.Key == settlement)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < this._warehouseRosterPerSettlement.Length; j++)
				{
					KeyValuePair<Settlement, ItemRoster> keyValuePair2 = this._warehouseRosterPerSettlement[j];
					if (keyValuePair2.Value == null)
					{
						this._warehouseRosterPerSettlement[j] = new KeyValuePair<Settlement, ItemRoster>(settlement, new ItemRoster());
						return;
					}
				}
			}
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00132C10 File Offset: 0x00130E10
		private EquipmentElement GetRandomItem(ItemCategory itemGroupBase, Town townComponent)
		{
			EquipmentElement randomItemAux = this.GetRandomItemAux(itemGroupBase, townComponent);
			if (randomItemAux.Item != null)
			{
				return randomItemAux;
			}
			return this.GetRandomItemAux(itemGroupBase, null);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00132C3C File Offset: 0x00130E3C
		private EquipmentElement GetRandomItemAux(ItemCategory itemGroupBase, Town townComponent = null)
		{
			ItemObject itemObject = null;
			ItemModifier itemModifier = null;
			List<ValueTuple<ItemObject, float>> list = new List<ValueTuple<ItemObject, float>>();
			List<ItemObject> list2;
			if (this._itemsInCategory.TryGetValue(itemGroupBase, out list2))
			{
				foreach (ItemObject itemObject2 in list2)
				{
					if ((townComponent == null || this.IsItemPreferredForTown(itemObject2, townComponent)) && itemObject2.ItemCategory == itemGroupBase)
					{
						float item = 1f / ((float)MathF.Max(100, itemObject2.Value) + 100f);
						list.Add(new ValueTuple<ItemObject, float>(itemObject2, item));
					}
				}
				itemObject = MBRandom.ChooseWeighted<ItemObject>(list);
				ItemModifierGroup itemModifierGroup;
				if (itemObject == null)
				{
					itemModifierGroup = null;
				}
				else
				{
					ItemComponent itemComponent = itemObject.ItemComponent;
					itemModifierGroup = ((itemComponent != null) ? itemComponent.ItemModifierGroup : null);
				}
				ItemModifierGroup itemModifierGroup2 = itemModifierGroup;
				if (itemModifierGroup2 != null)
				{
					itemModifier = itemModifierGroup2.GetRandomItemModifierProductionScoreBased();
				}
			}
			return new EquipmentElement(itemObject, itemModifier, null, false);
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00132D1C File Offset: 0x00130F1C
		private void TransferPlayerWorkshopsIfNeeded()
		{
			int count = Hero.MainHero.OwnedWorkshops.Count;
			List<Workshop> list = Hero.MainHero.OwnedWorkshops.ToList<Workshop>();
			for (int i = 0; i < count; i++)
			{
				Workshop workshop = list[i];
				if (workshop.Settlement.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
					if (notableOwnerForWorkshop != null)
					{
						WorkshopType workshopType = this.DecideBestWorkshopType(workshop.Settlement, false, workshop.WorkshopType);
						ChangeOwnerOfWorkshopAction.ApplyByWar(workshop, notableOwnerForWorkshop, workshopType);
					}
				}
			}
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00132DB4 File Offset: 0x00130FB4
		private void ChangeWorkshopOwnerByBankruptcy(Workshop workshop)
		{
			int costForNotable = Campaign.Current.Models.WorkshopModel.GetCostForNotable(workshop);
			Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
			WorkshopType workshopType = this.DecideBestWorkshopType(workshop.Settlement, false, workshop.WorkshopType);
			ChangeOwnerOfWorkshopAction.ApplyByBankruptcy(workshop, notableOwnerForWorkshop, workshopType, costForNotable);
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x00132E0A File Offset: 0x0013100A
		private void HandleDailyExpense(Workshop shop)
		{
			if (!shop.WorkshopType.IsHidden)
			{
				if (shop.Owner != Hero.MainHero)
				{
					this.HandleNotableWorkshopExpense(shop);
					return;
				}
				this.HandlePlayerWorkshopExpense(shop);
			}
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x00132E38 File Offset: 0x00131038
		private float FindTotalInputDensityScore(Settlement settlement, WorkshopType workshop, IDictionary<ItemCategory, float> productionDict, bool atGameStart)
		{
			int num = 0;
			for (int i = 0; i < settlement.Town.Workshops.Length; i++)
			{
				if (settlement.Town.Workshops[i].WorkshopType == workshop)
				{
					num++;
				}
			}
			float num2 = 0.01f;
			float num3 = 0f;
			foreach (WorkshopType.Production production in workshop.Productions)
			{
				bool flag = false;
				using (List<ValueTuple<ItemCategory, int>>.Enumerator enumerator2 = production.Outputs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Item1.IsTradeGood)
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					foreach (ValueTuple<ItemCategory, int> valueTuple in production.Inputs)
					{
						ItemCategory item = valueTuple.Item1;
						int item2 = valueTuple.Item2;
						float num4;
						if (productionDict.TryGetValue(item, out num4))
						{
							num2 += num4 / (production.ConversionSpeed * (float)item2);
						}
						if (!atGameStart)
						{
							float priceFactor = settlement.Town.MarketData.GetPriceFactor(item);
							num3 += Math.Max(0f, 1f - priceFactor);
						}
					}
				}
			}
			float num5 = 1f + (float)(num * 6);
			num2 *= (float)workshop.Frequency * (1f / (float)Math.Pow((double)num5, 3.0));
			num2 += num3;
			num2 = MathF.Pow(num2, 0.6f);
			return num2;
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00132FFC File Offset: 0x001311FC
		private void BuildWorkshopForHeroAtGameStart(Hero ownerHero)
		{
			Settlement bornSettlement = ownerHero.BornSettlement;
			WorkshopType workshopType = this.DecideBestWorkshopType(bornSettlement, true, null);
			if (workshopType != null)
			{
				int num = -1;
				for (int i = 0; i < bornSettlement.Town.Workshops.Length; i++)
				{
					if (bornSettlement.Town.Workshops[i].WorkshopType == null)
					{
						num = i;
						break;
					}
				}
				if (num >= 0)
				{
					InitializeWorkshopAction.ApplyByNewGame(bornSettlement.Town.Workshops[num], ownerHero, workshopType);
				}
			}
		}

		// Token: 0x06003E6E RID: 15982 RVA: 0x00133070 File Offset: 0x00131270
		private WorkshopType DecideBestWorkshopType(Settlement currentSettlement, bool atGameStart, WorkshopType workshopToExclude = null)
		{
			IDictionary<ItemCategory, float> dictionary = new Dictionary<ItemCategory, float>();
			foreach (Village village in from x in Village.All
			where x.TradeBound == currentSettlement
			select x)
			{
				foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
				{
					ItemCategory itemCategory = valueTuple.Item1.ItemCategory;
					if (itemCategory != DefaultItemCategories.Grain || village.VillageType.PrimaryProduction == DefaultItems.Grain)
					{
						float item = valueTuple.Item2;
						if (itemCategory == DefaultItemCategories.Cow || itemCategory == DefaultItemCategories.Hog)
						{
							itemCategory = DefaultItemCategories.Hides;
						}
						if (itemCategory == DefaultItemCategories.Sheep)
						{
							itemCategory = DefaultItemCategories.Wool;
						}
						float num;
						if (dictionary.TryGetValue(itemCategory, out num))
						{
							dictionary[itemCategory] = num + item;
						}
						else
						{
							dictionary.Add(itemCategory, item);
						}
					}
				}
			}
			Dictionary<WorkshopType, float> dictionary2 = new Dictionary<WorkshopType, float>();
			float num2 = 0f;
			foreach (WorkshopType workshopType in WorkshopType.All)
			{
				if (!workshopType.IsHidden && (workshopToExclude == null || workshopToExclude != workshopType))
				{
					float num3 = this.FindTotalInputDensityScore(currentSettlement, workshopType, dictionary, atGameStart);
					dictionary2.Add(workshopType, num3);
					num2 += num3;
				}
			}
			float num4 = num2 * MBRandom.RandomFloat;
			WorkshopType workshopType2 = null;
			foreach (WorkshopType workshopType3 in WorkshopType.All)
			{
				if (!workshopType3.IsHidden && (workshopToExclude == null || workshopToExclude != workshopType3))
				{
					num4 -= dictionary2[workshopType3];
					if (num4 < 0f)
					{
						workshopType2 = workshopType3;
						break;
					}
				}
			}
			if (workshopType2 == null)
			{
				workshopType2 = WorkshopType.All[MBRandom.RandomInt(1, WorkshopType.All.Count)];
			}
			return workshopType2;
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x001332C0 File Offset: 0x001314C0
		private void InitializeWorkshops()
		{
			foreach (Town town in Town.AllTowns)
			{
				town.InitializeWorkshops(Campaign.Current.Models.WorkshopModel.DefaultWorkshopCountInSettlement);
			}
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00133324 File Offset: 0x00131524
		private void BuildWorkshopsAtGameStart()
		{
			foreach (Town town in Town.AllTowns)
			{
				this.BuildArtisanWorkshop(town);
				for (int i = 1; i < town.Workshops.Length; i++)
				{
					Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(town.Workshops[i]);
					this.BuildWorkshopForHeroAtGameStart(notableOwnerForWorkshop);
				}
			}
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x001333B0 File Offset: 0x001315B0
		private void BuildArtisanWorkshop(Town town)
		{
			Hero hero = town.Settlement.Notables.FirstOrDefault((Hero x) => x.IsArtisan);
			if (hero == null)
			{
				hero = town.Settlement.Notables.FirstOrDefault<Hero>();
			}
			if (hero != null)
			{
				WorkshopType type = WorkshopType.Find("artisans");
				town.Workshops[0].InitializeWorkshop(hero, type);
			}
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x00133420 File Offset: 0x00131620
		private void RunTownShopsAtGameStart()
		{
			foreach (Town town in Town.AllTowns)
			{
				foreach (Workshop workshop in town.Workshops)
				{
					this.RunTownWorkshop(town, workshop);
				}
			}
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x00133490 File Offset: 0x00131690
		private void RunTownWorkshop(Town townComponent, Workshop workshop)
		{
			WorkshopType workshopType = workshop.WorkshopType;
			bool flag = false;
			for (int i = 0; i < workshopType.Productions.Count; i++)
			{
				float num = workshop.GetProductionProgress(i);
				if (num > 1f)
				{
					num = 1f;
				}
				num += Campaign.Current.Models.WorkshopModel.GetEffectiveConversionSpeedOfProduction(workshop, workshopType.Productions[i].ConversionSpeed, false).ResultNumber;
				if (num >= 1f)
				{
					bool flag2 = true;
					while (flag2 && num >= 1f)
					{
						flag2 = ((workshop.Owner == Hero.MainHero) ? this.TickOneProductionCycleForPlayerWorkshop(workshopType.Productions[i], workshop) : this.TickOneProductionCycleForNotableWorkshop(workshopType.Productions[i], workshop));
						if (flag2)
						{
							flag = true;
						}
						num -= 1f;
					}
				}
				workshop.SetProgress(i, num);
			}
			if (flag)
			{
				workshop.UpdateLastRunTime();
			}
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x00133580 File Offset: 0x00131780
		public void TransferWarehouseToPlayerParty(Settlement settlement)
		{
			foreach (ItemRosterElement itemRosterElement in this.GetWarehouseRoster(settlement))
			{
				MobileParty.MainParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, itemRosterElement.Amount);
			}
			this.RemoveWarehouseData(settlement);
		}

		// Token: 0x0400125A RID: 4698
		private KeyValuePair<Settlement, ItemRoster>[] _warehouseRosterPerSettlement;

		// Token: 0x0400125B RID: 4699
		private WorkshopsCampaignBehavior.WorkshopData[] _workshopData;

		// Token: 0x0400125C RID: 4700
		private readonly Dictionary<ItemCategory, List<ItemObject>> _itemsInCategory = new Dictionary<ItemCategory, List<ItemObject>>();

		// Token: 0x02000754 RID: 1876
		public class WorkshopsCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060059C6 RID: 22982 RVA: 0x00184808 File Offset: 0x00182A08
			public WorkshopsCampaignBehaviorTypeDefiner() : base(155828)
			{
			}

			// Token: 0x060059C7 RID: 22983 RVA: 0x00184815 File Offset: 0x00182A15
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(WorkshopsCampaignBehavior.WorkshopData), 10, null);
			}

			// Token: 0x060059C8 RID: 22984 RVA: 0x0018482A File Offset: 0x00182A2A
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<Workshop, WorkshopsCampaignBehavior.WorkshopData>));
				base.ConstructContainerDefinition(typeof(WorkshopsCampaignBehavior.WorkshopData[]));
			}
		}

		// Token: 0x02000755 RID: 1877
		internal class WorkshopData
		{
			// Token: 0x060059C9 RID: 22985 RVA: 0x0018484C File Offset: 0x00182A4C
			public WorkshopData(Workshop workshop)
			{
				this.Workshop = workshop;
			}

			// Token: 0x060059CA RID: 22986 RVA: 0x0018485B File Offset: 0x00182A5B
			public override string ToString()
			{
				return this.Workshop.WorkshopType.ToString() + " in " + this.Workshop.Settlement.GetName();
			}

			// Token: 0x060059CB RID: 22987 RVA: 0x00184887 File Offset: 0x00182A87
			internal static void AutoGeneratedStaticCollectObjectsWorkshopData(object o, List<object> collectedObjects)
			{
				((WorkshopsCampaignBehavior.WorkshopData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060059CC RID: 22988 RVA: 0x00184895 File Offset: 0x00182A95
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Workshop);
			}

			// Token: 0x060059CD RID: 22989 RVA: 0x001848A3 File Offset: 0x00182AA3
			internal static object AutoGeneratedGetMemberValueWorkshop(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).Workshop;
			}

			// Token: 0x060059CE RID: 22990 RVA: 0x001848B0 File Offset: 0x00182AB0
			internal static object AutoGeneratedGetMemberValueIsGettingInputsFromWarehouse(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).IsGettingInputsFromWarehouse;
			}

			// Token: 0x060059CF RID: 22991 RVA: 0x001848C2 File Offset: 0x00182AC2
			internal static object AutoGeneratedGetMemberValueProductionProgressForWarehouse(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).ProductionProgressForWarehouse;
			}

			// Token: 0x060059D0 RID: 22992 RVA: 0x001848D4 File Offset: 0x00182AD4
			internal static object AutoGeneratedGetMemberValueProductionProgressForTown(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).ProductionProgressForTown;
			}

			// Token: 0x060059D1 RID: 22993 RVA: 0x001848E6 File Offset: 0x00182AE6
			internal static object AutoGeneratedGetMemberValueStockProductionInWarehouseRatio(object o)
			{
				return ((WorkshopsCampaignBehavior.WorkshopData)o).StockProductionInWarehouseRatio;
			}

			// Token: 0x04001ECC RID: 7884
			[SaveableField(1)]
			public Workshop Workshop;

			// Token: 0x04001ECD RID: 7885
			[SaveableField(2)]
			public bool IsGettingInputsFromWarehouse;

			// Token: 0x04001ECE RID: 7886
			[SaveableField(3)]
			public float ProductionProgressForWarehouse;

			// Token: 0x04001ECF RID: 7887
			[SaveableField(4)]
			public float ProductionProgressForTown;

			// Token: 0x04001ED0 RID: 7888
			[SaveableField(5)]
			public float StockProductionInWarehouseRatio;
		}
	}
}
