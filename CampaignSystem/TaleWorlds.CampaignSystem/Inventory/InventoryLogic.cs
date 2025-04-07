using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000CF RID: 207
	public class InventoryLogic
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060012D7 RID: 4823 RVA: 0x00055D22 File Offset: 0x00053F22
		// (set) Token: 0x060012D8 RID: 4824 RVA: 0x00055D2A File Offset: 0x00053F2A
		public bool DisableNetwork { get; set; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060012D9 RID: 4825 RVA: 0x00055D33 File Offset: 0x00053F33
		// (set) Token: 0x060012DA RID: 4826 RVA: 0x00055D3B File Offset: 0x00053F3B
		public Action<int> TotalAmountChange { get; set; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x00055D44 File Offset: 0x00053F44
		// (set) Token: 0x060012DC RID: 4828 RVA: 0x00055D4C File Offset: 0x00053F4C
		public Action DonationXpChange { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060012DD RID: 4829 RVA: 0x00055D58 File Offset: 0x00053F58
		// (remove) Token: 0x060012DE RID: 4830 RVA: 0x00055D90 File Offset: 0x00053F90
		public event InventoryLogic.AfterResetDelegate AfterReset;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060012DF RID: 4831 RVA: 0x00055DC8 File Offset: 0x00053FC8
		// (remove) Token: 0x060012E0 RID: 4832 RVA: 0x00055E00 File Offset: 0x00054000
		public event InventoryLogic.ProcessResultListDelegate AfterTransfer;

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x00055E35 File Offset: 0x00054035
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x00055E3D File Offset: 0x0005403D
		public TroopRoster RightMemberRoster { get; private set; }

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060012E3 RID: 4835 RVA: 0x00055E46 File Offset: 0x00054046
		// (set) Token: 0x060012E4 RID: 4836 RVA: 0x00055E4E File Offset: 0x0005404E
		public TroopRoster LeftMemberRoster { get; private set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x00055E57 File Offset: 0x00054057
		// (set) Token: 0x060012E6 RID: 4838 RVA: 0x00055E5F File Offset: 0x0005405F
		public CharacterObject InitialEquipmentCharacter { get; private set; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x00055E68 File Offset: 0x00054068
		// (set) Token: 0x060012E8 RID: 4840 RVA: 0x00055E70 File Offset: 0x00054070
		public bool IsTrading { get; private set; }

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x00055E79 File Offset: 0x00054079
		// (set) Token: 0x060012EA RID: 4842 RVA: 0x00055E81 File Offset: 0x00054081
		public bool IsSpecialActionsPermitted { get; private set; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x00055E8A File Offset: 0x0005408A
		// (set) Token: 0x060012EC RID: 4844 RVA: 0x00055E92 File Offset: 0x00054092
		public CharacterObject OwnerCharacter { get; private set; }

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x00055E9B File Offset: 0x0005409B
		// (set) Token: 0x060012EE RID: 4846 RVA: 0x00055EA3 File Offset: 0x000540A3
		public MobileParty OwnerParty { get; private set; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x00055EAC File Offset: 0x000540AC
		// (set) Token: 0x060012F0 RID: 4848 RVA: 0x00055EB4 File Offset: 0x000540B4
		public PartyBase OtherParty { get; private set; }

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x00055EBD File Offset: 0x000540BD
		// (set) Token: 0x060012F2 RID: 4850 RVA: 0x00055EC5 File Offset: 0x000540C5
		public IMarketData MarketData { get; private set; }

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060012F3 RID: 4851 RVA: 0x00055ECE File Offset: 0x000540CE
		// (set) Token: 0x060012F4 RID: 4852 RVA: 0x00055ED6 File Offset: 0x000540D6
		public InventoryLogic.CapacityData OtherSideCapacityData { get; private set; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00055EDF File Offset: 0x000540DF
		// (set) Token: 0x060012F6 RID: 4854 RVA: 0x00055EE7 File Offset: 0x000540E7
		public TextObject LeftRosterName { get; private set; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00055EF0 File Offset: 0x000540F0
		// (set) Token: 0x060012F8 RID: 4856 RVA: 0x00055EF8 File Offset: 0x000540F8
		public bool IsDiscardDonating { get; private set; }

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060012F9 RID: 4857 RVA: 0x00055F01 File Offset: 0x00054101
		// (set) Token: 0x060012FA RID: 4858 RVA: 0x00055F09 File Offset: 0x00054109
		public bool IsOtherPartyFromPlayerClan { get; private set; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x00055F12 File Offset: 0x00054112
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x00055F1A File Offset: 0x0005411A
		public InventoryListener InventoryListener { get; private set; }

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x00055F23 File Offset: 0x00054123
		public int TotalAmount
		{
			get
			{
				return this.TransactionDebt;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060012FE RID: 4862 RVA: 0x00055F2B File Offset: 0x0005412B
		public PartyBase OppositePartyFromListener
		{
			get
			{
				return this.InventoryListener.GetOppositeParty();
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x00055F38 File Offset: 0x00054138
		public SettlementComponent CurrentSettlementComponent
		{
			get
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				if (currentSettlement == null)
				{
					return null;
				}
				return currentSettlement.SettlementComponent;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x00055F4C File Offset: 0x0005414C
		public MobileParty CurrentMobileParty
		{
			get
			{
				if (PlayerEncounter.Current != null)
				{
					return PlayerEncounter.EncounteredParty.MobileParty;
				}
				MapEvent mapEvent = PartyBase.MainParty.MapEvent;
				bool flag;
				if (mapEvent == null)
				{
					flag = (null != null);
				}
				else
				{
					PartyBase leaderParty = mapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide);
					flag = (((leaderParty != null) ? leaderParty.MobileParty : null) != null);
				}
				if (flag)
				{
					return PartyBase.MainParty.MapEvent.GetLeaderParty(PartyBase.MainParty.OpponentSide).MobileParty;
				}
				return null;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x00055FB9 File Offset: 0x000541B9
		// (set) Token: 0x06001302 RID: 4866 RVA: 0x00055FC1 File Offset: 0x000541C1
		public int TransactionDebt
		{
			get
			{
				return this._transactionDebt;
			}
			private set
			{
				if (value != this._transactionDebt)
				{
					this._transactionDebt = value;
					this.TotalAmountChange(this._transactionDebt);
				}
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x00055FE4 File Offset: 0x000541E4
		// (set) Token: 0x06001304 RID: 4868 RVA: 0x00055FEC File Offset: 0x000541EC
		public float XpGainFromDonations
		{
			get
			{
				return this._xpGainFromDonations;
			}
			private set
			{
				if (value != this._xpGainFromDonations)
				{
					this._xpGainFromDonations = value;
					if (this._xpGainFromDonations < 0f)
					{
						this._xpGainFromDonations = 0f;
					}
					Action donationXpChange = this.DonationXpChange;
					if (donationXpChange == null)
					{
						return;
					}
					donationXpChange();
				}
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x00056028 File Offset: 0x00054228
		public InventoryLogic(MobileParty ownerParty, CharacterObject ownerCharacter, PartyBase merchantParty)
		{
			this._rosters = new ItemRoster[2];
			this._rostersBackup = new ItemRoster[2];
			this.OwnerParty = ownerParty;
			this.OwnerCharacter = ownerCharacter;
			this.OtherParty = merchantParty;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x00056085 File Offset: 0x00054285
		public InventoryLogic(PartyBase merchantParty) : this(MobileParty.MainParty, CharacterObject.PlayerCharacter, merchantParty)
		{
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00056098 File Offset: 0x00054298
		public void Initialize(ItemRoster leftItemRoster, MobileParty party, bool isTrading, bool isSpecialActionsPermitted, CharacterObject initialCharacterOfRightRoster, InventoryManager.InventoryCategoryType merchantItemType, IMarketData marketData, bool useBasePrices, TextObject leftRosterName = null, TroopRoster leftMemberRoster = null, InventoryLogic.CapacityData otherSideCapacityData = null)
		{
			this.Initialize(leftItemRoster, party.ItemRoster, party.MemberRoster, isTrading, isSpecialActionsPermitted, initialCharacterOfRightRoster, merchantItemType, marketData, useBasePrices, leftRosterName, leftMemberRoster, otherSideCapacityData);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x000560CC File Offset: 0x000542CC
		public void Initialize(ItemRoster leftItemRoster, ItemRoster rightItemRoster, TroopRoster rightMemberRoster, bool isTrading, bool isSpecialActionsPermitted, CharacterObject initialCharacterOfRightRoster, InventoryManager.InventoryCategoryType merchantItemType, IMarketData marketData, bool useBasePrices, TextObject leftRosterName = null, TroopRoster leftMemberRoster = null, InventoryLogic.CapacityData otherSideCapacityData = null)
		{
			this.OtherSideCapacityData = otherSideCapacityData;
			this.MarketData = marketData;
			this.TransactionDebt = 0;
			this.MerchantItemType = merchantItemType;
			this.InventoryListener = new FakeInventoryListener();
			this._useBasePrices = useBasePrices;
			this.LeftRosterName = leftRosterName;
			this.IsTrading = isTrading;
			this.IsSpecialActionsPermitted = isSpecialActionsPermitted;
			this.InitializeRosters(leftItemRoster, rightItemRoster, rightMemberRoster, initialCharacterOfRightRoster, leftMemberRoster);
			this._transactionHistory.Clear();
			this.InitializeCategoryAverages();
			this.IsDiscardDonating = ((InventoryManager.Instance.CurrentMode == InventoryMode.Default && !Game.Current.CheatMode) || InventoryManager.Instance.CurrentMode == InventoryMode.Loot);
			this.InitializeXpGainFromDonations();
			PartyBase otherParty = this.OtherParty;
			Clan clan;
			if (otherParty == null)
			{
				clan = null;
			}
			else
			{
				MobileParty mobileParty = otherParty.MobileParty;
				clan = ((mobileParty != null) ? mobileParty.ActualClan : null);
			}
			if (clan == Hero.MainHero.Clan)
			{
				this.IsOtherPartyFromPlayerClan = true;
			}
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x000561A6 File Offset: 0x000543A6
		private void InitializeRosters(ItemRoster leftItemRoster, ItemRoster rightItemRoster, TroopRoster rightMemberRoster, CharacterObject initialCharacterOfRightRoster, TroopRoster leftMemberRoster = null)
		{
			this._rosters[0] = leftItemRoster;
			this._rosters[1] = rightItemRoster;
			this.RightMemberRoster = rightMemberRoster;
			this.LeftMemberRoster = leftMemberRoster;
			this.InitialEquipmentCharacter = initialCharacterOfRightRoster;
			this.SetCurrentStateAsInitial();
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000561D7 File Offset: 0x000543D7
		public int GetItemTotalPrice(ItemRosterElement itemRosterElement, int absStockChange, out int lastPrice, bool isBuying)
		{
			lastPrice = this.GetItemPrice(itemRosterElement.EquipmentElement, isBuying);
			return lastPrice;
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000561EC File Offset: 0x000543EC
		public void SetPlayerAcceptTraderOffer()
		{
			this._playerAcceptsTraderOffer = true;
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x000561F8 File Offset: 0x000543F8
		public bool DoneLogic()
		{
			if (this.IsPreviewingItem)
			{
				return false;
			}
			SettlementComponent currentSettlementComponent = this.CurrentSettlementComponent;
			MobileParty currentMobileParty = this.CurrentMobileParty;
			PartyBase partyBase = null;
			if (currentMobileParty != null)
			{
				partyBase = currentMobileParty.Party;
			}
			else if (currentSettlementComponent != null)
			{
				partyBase = currentSettlementComponent.Owner;
			}
			if (!this._playerAcceptsTraderOffer)
			{
				InventoryListener inventoryListener = this.InventoryListener;
				int? num = ((inventoryListener != null) ? new int?(inventoryListener.GetGold()) : null) + this.TotalAmount;
				int num2 = 0;
				bool flag = num.GetValueOrDefault() < num2 & num != null;
			}
			if (this.InventoryListener != null && this.IsTrading && this.OwnerCharacter.HeroObject.Gold - this.TotalAmount < 0)
			{
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_warning_you_dont_have_enough_money", null), 0, null, "");
				return false;
			}
			if (this._playerAcceptsTraderOffer)
			{
				this._playerAcceptsTraderOffer = false;
				if (this.InventoryListener != null)
				{
					int gold = this.InventoryListener.GetGold();
					this.TransactionDebt = -gold;
				}
			}
			if (this.OwnerCharacter != null && this.OwnerCharacter.HeroObject != null && this.IsTrading)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, this.OwnerCharacter.HeroObject, MathF.Min(-this.TotalAmount, this.InventoryListener.GetGold()), false);
				if (currentSettlementComponent != null && currentSettlementComponent.IsTown && this.OwnerCharacter.GetPerkValue(DefaultPerks.Trade.TrickleDown))
				{
					int num3 = 0;
					List<ValueTuple<ItemRosterElement, int>> boughtItems = this._transactionHistory.GetBoughtItems();
					int num4 = 0;
					while (boughtItems != null && num4 < boughtItems.Count)
					{
						ItemObject item = boughtItems[num4].Item1.EquipmentElement.Item;
						if (item != null && item.IsTradeGood)
						{
							num3 += boughtItems[num4].Item2;
						}
						num4++;
					}
					if (num3 >= 10000)
					{
						for (int i = 0; i < currentSettlementComponent.Settlement.Notables.Count; i++)
						{
							if (currentSettlementComponent.Settlement.Notables[i].IsMerchant)
							{
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(currentSettlementComponent.Settlement.Notables[i], this.OwnerCharacter.HeroObject, MathF.Floor(DefaultPerks.Trade.TrickleDown.PrimaryBonus), true);
							}
						}
					}
				}
			}
			if (this.IsDiscardDonating)
			{
				CampaignEventDispatcher.Instance.OnItemsDiscardedByPlayer(this._rosters[0]);
			}
			CampaignEventDispatcher.Instance.OnPlayerInventoryExchange(this._transactionHistory.GetBoughtItems(), this._transactionHistory.GetSoldItems(), this.IsTrading);
			if (currentSettlementComponent != null && this.InventoryListener != null && this.IsTrading)
			{
				this.InventoryListener.SetGold(this.InventoryListener.GetGold() + this.TotalAmount);
			}
			else if (((currentMobileParty != null) ? currentMobileParty.Party.LeaderHero : null) != null && this.IsTrading)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, this.CurrentMobileParty.Party.LeaderHero, this.TotalAmount, false);
				if (this.CurrentMobileParty.Party.LeaderHero.CompanionOf != null)
				{
					this.CurrentMobileParty.AddTaxGold((int)((float)this.TotalAmount * 0.1f));
				}
			}
			else if (partyBase != null && partyBase.LeaderHero == null && this.IsTrading)
			{
				GiveGoldAction.ApplyForCharacterToParty(null, partyBase, this.TotalAmount, false);
			}
			this._partyInitialEquipment = new InventoryLogic.PartyEquipment(this.OwnerParty);
			return true;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x00056578 File Offset: 0x00054778
		public List<ValueTuple<ItemRosterElement, int>> GetBoughtItems()
		{
			return this._transactionHistory.GetBoughtItems();
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x00056585 File Offset: 0x00054785
		public List<ValueTuple<ItemRosterElement, int>> GetSoldItems()
		{
			return this._transactionHistory.GetSoldItems();
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x00056592 File Offset: 0x00054792
		public bool CanInventoryCapacityIncrease(InventoryLogic.InventorySide side)
		{
			return InventoryManager.Instance.CurrentMode != InventoryMode.Warehouse || side > InventoryLogic.InventorySide.OtherInventory;
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x000565A7 File Offset: 0x000547A7
		public bool GetCanItemIncreaseInventoryCapacity(ItemObject item)
		{
			return item.HasHorseComponent;
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x000565B0 File Offset: 0x000547B0
		private void InitializeCategoryAverages()
		{
			if (Campaign.Current != null && Settlement.CurrentSettlement != null)
			{
				Town town = Settlement.CurrentSettlement.IsVillage ? Settlement.CurrentSettlement.Village.Bound.Town : Settlement.CurrentSettlement.Town;
				foreach (ItemCategory itemCategory in ItemCategories.All)
				{
					float num = 0f;
					for (int i = 0; i < Town.AllTowns.Count; i++)
					{
						if (Town.AllTowns[i] != town)
						{
							num += Town.AllTowns[i].MarketData.GetPriceFactor(itemCategory);
						}
					}
					float num2 = num / (float)(Town.AllTowns.Count - 1);
					this._itemCategoryAverages.Add(itemCategory, num2);
					Debug.Print(string.Format("Average value of {0} : {1}", itemCategory.GetName(), num2), 0, Debug.DebugColor.White, 17592186044416UL);
				}
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x000566D4 File Offset: 0x000548D4
		private void InitializeXpGainFromDonations()
		{
			this.XpGainFromDonations = 0f;
			bool flag = PerkHelper.PlayerHasAnyItemDonationPerk();
			bool flag2 = InventoryManager.Instance.CurrentMode == InventoryMode.Loot;
			if (flag && flag2)
			{
				this.XpGainFromDonations = (float)Campaign.Current.Models.ItemDiscardModel.GetXpBonusForDiscardingItems(this._rosters[0]);
			}
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00056728 File Offset: 0x00054928
		private void HandleDonationOnTransferItem(ItemRosterElement rosterElement, int amount, bool isBuying, bool isSelling)
		{
			ItemObject item = rosterElement.EquipmentElement.Item;
			ItemDiscardModel itemDiscardModel = Campaign.Current.Models.ItemDiscardModel;
			if (this.IsDiscardDonating && (isSelling || isBuying) && item != null)
			{
				this.XpGainFromDonations += (float)(itemDiscardModel.GetXpBonusForDiscardingItem(item, amount) * (isSelling ? 1 : -1));
			}
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x00056785 File Offset: 0x00054985
		public float GetAveragePriceFactorItemCategory(ItemCategory category)
		{
			if (this._itemCategoryAverages.ContainsKey(category))
			{
				return this._itemCategoryAverages[category];
			}
			return -99f;
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000567A7 File Offset: 0x000549A7
		public bool IsThereAnyChanges()
		{
			return this.IsThereAnyChangeBetweenRosters(this._rosters[1], this._rostersBackup[1]) || !this._partyInitialEquipment.IsEqual(new InventoryLogic.PartyEquipment(this.OwnerParty));
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x000567DC File Offset: 0x000549DC
		private bool IsThereAnyChangeBetweenRosters(ItemRoster roster1, ItemRoster roster2)
		{
			if (roster1.Count != roster2.Count)
			{
				return true;
			}
			using (IEnumerator<ItemRosterElement> enumerator = roster1.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ItemRosterElement item = enumerator.Current;
					if (!roster2.Any((ItemRosterElement e) => e.IsEqualTo(item)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00056854 File Offset: 0x00054A54
		public void Reset(bool fromCancel)
		{
			this.ResetLogic(fromCancel);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00056860 File Offset: 0x00054A60
		private void ResetLogic(bool fromCancel)
		{
			Debug.Print("InventoryLogic::Reset", 0, Debug.DebugColor.White, 17592186044416UL);
			for (int i = 0; i < 2; i++)
			{
				this._rosters[i].Clear();
				this._rosters[i].Add(this._rostersBackup[i]);
			}
			this.TransactionDebt = 0;
			this._transactionHistory.Clear();
			this.InitializeXpGainFromDonations();
			this._partyInitialEquipment.ResetEquipment(this.OwnerParty);
			InventoryLogic.AfterResetDelegate afterReset = this.AfterReset;
			if (afterReset != null)
			{
				afterReset(this, fromCancel);
			}
			List<TransferCommandResult> resultList = new List<TransferCommandResult>();
			if (!fromCancel)
			{
				this.OnAfterTransfer(resultList);
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00056900 File Offset: 0x00054B00
		public bool CanPlayerCompleteTransaction()
		{
			InventoryLogic.CapacityData otherSideCapacityData = this.OtherSideCapacityData;
			int num = (otherSideCapacityData != null) ? otherSideCapacityData.GetCapacity() : -1;
			if (num != -1)
			{
				if (this._rosters[0].Sum((ItemRosterElement x) => x.GetRosterElementWeight()) > (float)num)
				{
					return false;
				}
			}
			return !this.IsPreviewingItem || !this.IsTrading || this.TotalAmount <= 0 || (this.TotalAmount >= 0 && this.OwnerCharacter.HeroObject.Gold - this.TotalAmount >= 0);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x0005699C File Offset: 0x00054B9C
		public bool CanSlaughterItem(ItemRosterElement element, InventoryLogic.InventorySide sideOfItem)
		{
			return (!this.IsTrading || this._transactionHistory.IsEmpty) && (this.IsSpecialActionsPermitted && this.IsSlaughterable(element.EquipmentElement.Item) && sideOfItem == InventoryLogic.InventorySide.PlayerInventory && element.Amount > 0) && !this._transactionHistory.GetBoughtItems().Any((ValueTuple<ItemRosterElement, int> i) => i.Item1.EquipmentElement.Item == element.EquipmentElement.Item);
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00056A23 File Offset: 0x00054C23
		public bool IsSlaughterable(ItemObject item)
		{
			return item.Type == ItemObject.ItemTypeEnum.Animal || item.Type == ItemObject.ItemTypeEnum.Horse;
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00056A3C File Offset: 0x00054C3C
		public bool CanDonateItem(ItemRosterElement element, InventoryLogic.InventorySide sideOfItem)
		{
			return Game.Current.IsDevelopmentMode && this.IsSpecialActionsPermitted && element.Amount > 0 && this.IsDonatable(element.EquipmentElement.Item) && sideOfItem == InventoryLogic.InventorySide.PlayerInventory;
		}

		// Token: 0x0600131D RID: 4893 RVA: 0x00056A84 File Offset: 0x00054C84
		public bool IsDonatable(ItemObject item)
		{
			return item.Type == ItemObject.ItemTypeEnum.Arrows || item.Type == ItemObject.ItemTypeEnum.BodyArmor || item.Type == ItemObject.ItemTypeEnum.Bolts || item.Type == ItemObject.ItemTypeEnum.Bow || item.Type == ItemObject.ItemTypeEnum.Bullets || item.Type == ItemObject.ItemTypeEnum.Cape || item.Type == ItemObject.ItemTypeEnum.ChestArmor || item.Type == ItemObject.ItemTypeEnum.Crossbow || item.Type == ItemObject.ItemTypeEnum.HandArmor || item.Type == ItemObject.ItemTypeEnum.HeadArmor || item.Type == ItemObject.ItemTypeEnum.HorseHarness || item.Type == ItemObject.ItemTypeEnum.LegArmor || item.Type == ItemObject.ItemTypeEnum.Musket || item.Type == ItemObject.ItemTypeEnum.OneHandedWeapon || item.Type == ItemObject.ItemTypeEnum.Pistol || item.Type == ItemObject.ItemTypeEnum.Polearm || item.Type == ItemObject.ItemTypeEnum.Shield || item.Type == ItemObject.ItemTypeEnum.Thrown || item.Type == ItemObject.ItemTypeEnum.TwoHandedWeapon;
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x00056B59 File Offset: 0x00054D59
		public void SetInventoryListener(InventoryListener inventoryListener)
		{
			this.InventoryListener = inventoryListener;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00056B64 File Offset: 0x00054D64
		public int GetItemPrice(EquipmentElement equipmentElement, bool isBuying = false)
		{
			bool flag = !isBuying;
			bool flag2 = false;
			int result = 0;
			int num;
			bool flag3;
			if (this._transactionHistory.GetLastTransfer(equipmentElement, out num, out flag3) && flag3 != flag)
			{
				flag2 = true;
				result = num;
			}
			if (this._useBasePrices)
			{
				return equipmentElement.GetBaseValue();
			}
			if (flag2)
			{
				return result;
			}
			return this.MarketData.GetPrice(equipmentElement, this.OwnerParty, flag, this.OtherParty);
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00056BC4 File Offset: 0x00054DC4
		public int GetCostOfItemRosterElement(ItemRosterElement itemRosterElement, InventoryLogic.InventorySide side)
		{
			bool isBuying = side == InventoryLogic.InventorySide.OtherInventory && this.IsTrading;
			return this.GetItemPrice(itemRosterElement.EquipmentElement, isBuying);
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00056BEC File Offset: 0x00054DEC
		private void OnAfterTransfer(List<TransferCommandResult> resultList)
		{
			InventoryLogic.ProcessResultListDelegate afterTransfer = this.AfterTransfer;
			if (afterTransfer != null)
			{
				afterTransfer(this, resultList);
			}
			foreach (TransferCommandResult transferCommandResult in resultList)
			{
				if (transferCommandResult.EffectedNumber > 0)
				{
					Game.Current.EventManager.TriggerEvent<InventoryTransferItemEvent>(new InventoryTransferItemEvent(transferCommandResult.EffectedItemRosterElement.EquipmentElement.Item, transferCommandResult.ResultSide == InventoryLogic.InventorySide.PlayerInventory));
				}
			}
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00056C84 File Offset: 0x00054E84
		public void AddTransferCommand(TransferCommand command)
		{
			this.ProcessTransferCommand(command);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x00056C90 File Offset: 0x00054E90
		public void AddTransferCommands(IEnumerable<TransferCommand> commands)
		{
			foreach (TransferCommand command in commands)
			{
				this.ProcessTransferCommand(command);
			}
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x00056CD8 File Offset: 0x00054ED8
		public bool CheckItemRosterHasElement(InventoryLogic.InventorySide side, ItemRosterElement rosterElement, int number)
		{
			int num = this._rosters[(int)side].FindIndexOfElement(rosterElement.EquipmentElement);
			return num != -1 && this._rosters[(int)side].GetElementCopyAtIndex(num).Amount >= number;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x00056D1C File Offset: 0x00054F1C
		private void ProcessTransferCommand(TransferCommand command)
		{
			List<TransferCommandResult> resultList = this.TransferItem(ref command);
			this.OnAfterTransfer(resultList);
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x00056D3C File Offset: 0x00054F3C
		private List<TransferCommandResult> TransferItem(ref TransferCommand transferCommand)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			Debug.Print(string.Format("TransferItem Name: {0} | From: {1} To: {2} | Amount: {3}", new object[]
			{
				transferCommand.ElementToTransfer.EquipmentElement.Item.Name.ToString(),
				transferCommand.FromSide,
				transferCommand.ToSide,
				transferCommand.Amount
			}), 0, Debug.DebugColor.White, 17592186044416UL);
			if (transferCommand.ElementToTransfer.EquipmentElement.Item != null && InventoryLogic.TransferIsMovementValid(ref transferCommand) && this.DoesTransferItemExist(ref transferCommand))
			{
				int num = 0;
				bool flag = false;
				if (transferCommand.FromSide != InventoryLogic.InventorySide.Equipment && transferCommand.FromSide != InventoryLogic.InventorySide.None)
				{
					int index = this._rosters[(int)transferCommand.FromSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
					ItemRosterElement elementCopyAtIndex = this._rosters[(int)transferCommand.FromSide].GetElementCopyAtIndex(index);
					flag = (transferCommand.Amount == elementCopyAtIndex.Amount);
				}
				bool flag2 = this.IsSell(transferCommand.FromSide, transferCommand.ToSide);
				bool flag3 = this.IsBuy(transferCommand.FromSide, transferCommand.ToSide);
				for (int i = 0; i < transferCommand.Amount; i++)
				{
					if (transferCommand.ToSide == InventoryLogic.InventorySide.Equipment && transferCommand.CharacterEquipment[(int)transferCommand.ToEquipmentIndex].Item != null)
					{
						TransferCommand transferCommand2 = TransferCommand.Transfer(1, InventoryLogic.InventorySide.Equipment, InventoryLogic.InventorySide.PlayerInventory, new ItemRosterElement(transferCommand.CharacterEquipment[(int)transferCommand.ToEquipmentIndex], 1), transferCommand.ToEquipmentIndex, EquipmentIndex.None, transferCommand.Character, transferCommand.IsCivilianEquipment);
						list.AddRange(this.TransferItem(ref transferCommand2));
					}
					EquipmentElement equipmentElement = transferCommand.ElementToTransfer.EquipmentElement;
					int itemPrice = this.GetItemPrice(equipmentElement, flag3);
					if (flag3 || flag2)
					{
						this._transactionHistory.RecordTransaction(equipmentElement, flag2, itemPrice);
					}
					if (this.IsTrading)
					{
						if (flag3)
						{
							num += itemPrice;
						}
						else if (flag2)
						{
							num -= itemPrice;
						}
					}
					if (transferCommand.FromSide == InventoryLogic.InventorySide.Equipment)
					{
						ItemRosterElement itemRosterElement = new ItemRosterElement(transferCommand.CharacterEquipment[(int)transferCommand.FromEquipmentIndex], transferCommand.Amount);
						itemRosterElement.Amount--;
						transferCommand.CharacterEquipment[(int)transferCommand.FromEquipmentIndex] = itemRosterElement.EquipmentElement;
					}
					else if (transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory)
					{
						this._rosters[(int)transferCommand.FromSide].AddToCounts(transferCommand.ElementToTransfer.EquipmentElement, -1);
					}
					if (transferCommand.ToSide == InventoryLogic.InventorySide.Equipment)
					{
						ItemRosterElement elementToTransfer = transferCommand.ElementToTransfer;
						elementToTransfer.Amount = 1;
						transferCommand.CharacterEquipment[(int)transferCommand.ToEquipmentIndex] = elementToTransfer.EquipmentElement;
					}
					else if (transferCommand.ToSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.ToSide == InventoryLogic.InventorySide.OtherInventory)
					{
						this._rosters[(int)transferCommand.ToSide].AddToCounts(transferCommand.ElementToTransfer.EquipmentElement, 1);
					}
				}
				if (transferCommand.FromSide == InventoryLogic.InventorySide.Equipment)
				{
					ItemRosterElement effectedItemRosterElement = new ItemRosterElement(transferCommand.CharacterEquipment[(int)transferCommand.FromEquipmentIndex], transferCommand.Amount);
					int amount = effectedItemRosterElement.Amount;
					effectedItemRosterElement.Amount = amount - 1;
					list.Add(new TransferCommandResult(transferCommand.FromSide, effectedItemRosterElement, -transferCommand.Amount, effectedItemRosterElement.Amount, transferCommand.FromEquipmentIndex, transferCommand.Character, transferCommand.IsCivilianEquipment));
				}
				else if (transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory)
				{
					if (flag)
					{
						list.Add(new TransferCommandResult(transferCommand.FromSide, new ItemRosterElement(transferCommand.ElementToTransfer.EquipmentElement, 0), -transferCommand.Amount, 0, transferCommand.FromEquipmentIndex, transferCommand.Character, transferCommand.IsCivilianEquipment));
					}
					else
					{
						int index2 = this._rosters[(int)transferCommand.FromSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
						ItemRosterElement elementCopyAtIndex2 = this._rosters[(int)transferCommand.FromSide].GetElementCopyAtIndex(index2);
						list.Add(new TransferCommandResult(transferCommand.FromSide, elementCopyAtIndex2, -transferCommand.Amount, elementCopyAtIndex2.Amount, transferCommand.FromEquipmentIndex, transferCommand.Character, transferCommand.IsCivilianEquipment));
					}
				}
				if (transferCommand.ToSide == InventoryLogic.InventorySide.Equipment)
				{
					ItemRosterElement elementToTransfer2 = transferCommand.ElementToTransfer;
					elementToTransfer2.Amount = 1;
					list.Add(new TransferCommandResult(transferCommand.ToSide, elementToTransfer2, 1, 1, transferCommand.ToEquipmentIndex, transferCommand.Character, transferCommand.IsCivilianEquipment));
				}
				else if (transferCommand.ToSide == InventoryLogic.InventorySide.PlayerInventory || transferCommand.ToSide == InventoryLogic.InventorySide.OtherInventory)
				{
					int index3 = this._rosters[(int)transferCommand.ToSide].FindIndexOfElement(transferCommand.ElementToTransfer.EquipmentElement);
					ItemRosterElement elementCopyAtIndex3 = this._rosters[(int)transferCommand.ToSide].GetElementCopyAtIndex(index3);
					list.Add(new TransferCommandResult(transferCommand.ToSide, elementCopyAtIndex3, transferCommand.Amount, elementCopyAtIndex3.Amount, transferCommand.ToEquipmentIndex, transferCommand.Character, transferCommand.IsCivilianEquipment));
				}
				this.HandleDonationOnTransferItem(transferCommand.ElementToTransfer, transferCommand.Amount, flag3, flag2);
				this.TransactionDebt += num;
			}
			return list;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x00057249 File Offset: 0x00055449
		private bool IsSell(InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide)
		{
			return toSide == InventoryLogic.InventorySide.OtherInventory && (fromSide == InventoryLogic.InventorySide.Equipment || fromSide == InventoryLogic.InventorySide.PlayerInventory);
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0005725A File Offset: 0x0005545A
		private bool IsBuy(InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide)
		{
			return fromSide == InventoryLogic.InventorySide.OtherInventory && (toSide == InventoryLogic.InventorySide.Equipment || toSide == InventoryLogic.InventorySide.PlayerInventory);
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0005726C File Offset: 0x0005546C
		public void SlaughterItem(ItemRosterElement itemRosterElement)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			EquipmentElement equipmentElement = itemRosterElement.EquipmentElement;
			int meatCount = equipmentElement.Item.HorseComponent.MeatCount;
			int hideCount = equipmentElement.Item.HorseComponent.HideCount;
			int index = this._rosters[1].AddToCounts(DefaultItems.Meat, meatCount);
			ItemRosterElement elementCopyAtIndex = this._rosters[1].GetElementCopyAtIndex(index);
			bool flag = itemRosterElement.Amount == 1;
			int index2 = this._rosters[1].AddToCounts(itemRosterElement.EquipmentElement, -1);
			if (flag)
			{
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, new ItemRosterElement(equipmentElement, 0), -1, 0, EquipmentIndex.None, null, equipmentElement.Item.IsCivilian));
			}
			else
			{
				ItemRosterElement elementCopyAtIndex2 = this._rosters[1].GetElementCopyAtIndex(index2);
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex2, -1, elementCopyAtIndex2.Amount, EquipmentIndex.None, null, elementCopyAtIndex2.EquipmentElement.Item.IsCivilian));
			}
			list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex, meatCount, elementCopyAtIndex.Amount, EquipmentIndex.None, null, elementCopyAtIndex.EquipmentElement.Item.IsCivilian));
			if (hideCount > 0)
			{
				int index3 = this._rosters[1].AddToCounts(DefaultItems.Hides, hideCount);
				ItemRosterElement elementCopyAtIndex3 = this._rosters[1].GetElementCopyAtIndex(index3);
				list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex3, hideCount, elementCopyAtIndex3.Amount, EquipmentIndex.None, null, elementCopyAtIndex3.EquipmentElement.Item.IsCivilian));
			}
			this.SetCurrentStateAsInitial();
			this.OnAfterTransfer(list);
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x000573E8 File Offset: 0x000555E8
		public void DonateItem(ItemRosterElement itemRosterElement)
		{
			List<TransferCommandResult> list = new List<TransferCommandResult>();
			int tier = (int)itemRosterElement.EquipmentElement.Item.Tier;
			int num = 100 * (tier + 1);
			InventoryLogic.InventorySide inventorySide = InventoryLogic.InventorySide.PlayerInventory;
			int index = this._rosters[(int)inventorySide].AddToCounts(itemRosterElement.EquipmentElement, -1);
			ItemRosterElement elementCopyAtIndex = this._rosters[(int)inventorySide].GetElementCopyAtIndex(index);
			list.Add(new TransferCommandResult(InventoryLogic.InventorySide.PlayerInventory, elementCopyAtIndex, -1, elementCopyAtIndex.Amount, EquipmentIndex.None, null, elementCopyAtIndex.EquipmentElement.Item.IsCivilian));
			if (num > 0)
			{
				TroopRosterElement randomElementWithPredicate = PartyBase.MainParty.MemberRoster.GetTroopRoster().GetRandomElementWithPredicate((TroopRosterElement m) => !m.Character.IsHero && m.Character.UpgradeTargets.Length != 0);
				if (randomElementWithPredicate.Character != null)
				{
					PartyBase.MainParty.MemberRoster.AddXpToTroop(num, randomElementWithPredicate.Character);
					TextObject textObject = new TextObject("{=Kwja0a4s}Added {XPAMOUNT} amount of xp to {TROOPNAME}", null);
					textObject.SetTextVariable("XPAMOUNT", num);
					textObject.SetTextVariable("TROOPNAME", randomElementWithPredicate.Character.Name.ToString());
					Debug.Print(textObject.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
					MBInformationManager.AddQuickInformation(textObject, 0, null, "");
				}
			}
			this.SetCurrentStateAsInitial();
			this.OnAfterTransfer(list);
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00057534 File Offset: 0x00055734
		private static bool TransferIsMovementValid(ref TransferCommand transferCommand)
		{
			if (transferCommand.ElementToTransfer.EquipmentElement.IsQuestItem)
			{
				BannerComponent bannerComponent = transferCommand.ElementToTransfer.EquipmentElement.Item.BannerComponent;
				if (((bannerComponent != null) ? bannerComponent.BannerEffect : null) == null || ((transferCommand.FromSide != InventoryLogic.InventorySide.PlayerInventory || transferCommand.ToSide != InventoryLogic.InventorySide.Equipment) && (transferCommand.FromSide != InventoryLogic.InventorySide.Equipment || transferCommand.ToSide != InventoryLogic.InventorySide.PlayerInventory)))
				{
					return false;
				}
			}
			bool result = false;
			if (transferCommand.ToSide == InventoryLogic.InventorySide.Equipment)
			{
				InventoryItemType inventoryItemTypeOfItem = InventoryManager.GetInventoryItemTypeOfItem(transferCommand.ElementToTransfer.EquipmentElement.Item);
				switch (transferCommand.ToEquipmentIndex)
				{
				case EquipmentIndex.WeaponItemBeginSlot:
				case EquipmentIndex.Weapon1:
				case EquipmentIndex.Weapon2:
				case EquipmentIndex.Weapon3:
					result = (inventoryItemTypeOfItem == InventoryItemType.Weapon || inventoryItemTypeOfItem == InventoryItemType.Shield);
					break;
				case EquipmentIndex.ExtraWeaponSlot:
					result = (inventoryItemTypeOfItem == InventoryItemType.Banner);
					break;
				case EquipmentIndex.NumAllWeaponSlots:
					result = (inventoryItemTypeOfItem == InventoryItemType.HeadArmor);
					break;
				case EquipmentIndex.Body:
					result = (inventoryItemTypeOfItem == InventoryItemType.BodyArmor);
					break;
				case EquipmentIndex.Leg:
					result = (inventoryItemTypeOfItem == InventoryItemType.LegArmor);
					break;
				case EquipmentIndex.Gloves:
					result = (inventoryItemTypeOfItem == InventoryItemType.HandArmor);
					break;
				case EquipmentIndex.Cape:
					result = (inventoryItemTypeOfItem == InventoryItemType.Cape);
					break;
				case EquipmentIndex.ArmorItemEndSlot:
					result = (inventoryItemTypeOfItem == InventoryItemType.Horse);
					break;
				case EquipmentIndex.HorseHarness:
					result = (inventoryItemTypeOfItem == InventoryItemType.HorseHarness);
					break;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0005766C File Offset: 0x0005586C
		private bool DoesTransferItemExist(ref TransferCommand transferCommand)
		{
			if (transferCommand.FromSide == InventoryLogic.InventorySide.OtherInventory || transferCommand.FromSide == InventoryLogic.InventorySide.PlayerInventory)
			{
				return this.CheckItemRosterHasElement(transferCommand.FromSide, transferCommand.ElementToTransfer, transferCommand.Amount);
			}
			return transferCommand.FromSide == InventoryLogic.InventorySide.Equipment && transferCommand.CharacterEquipment[(int)transferCommand.FromEquipmentIndex].Item != null && transferCommand.ElementToTransfer.EquipmentElement.IsEqualTo(transferCommand.CharacterEquipment[(int)transferCommand.FromEquipmentIndex]);
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x000576F2 File Offset: 0x000558F2
		public void TransferOne(ItemRosterElement itemRosterElement)
		{
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x000576F4 File Offset: 0x000558F4
		public int GetElementCountOnSide(InventoryLogic.InventorySide side)
		{
			return this._rosters[(int)side].Count;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00057703 File Offset: 0x00055903
		public IEnumerable<ItemRosterElement> GetElementsInInitialRoster(InventoryLogic.InventorySide side)
		{
			return this._rostersBackup[(int)side];
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0005770D File Offset: 0x0005590D
		public IEnumerable<ItemRosterElement> GetElementsInRoster(InventoryLogic.InventorySide side)
		{
			return this._rosters[(int)side];
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x00057718 File Offset: 0x00055918
		private void SetCurrentStateAsInitial()
		{
			for (int i = 0; i < this._rostersBackup.Length; i++)
			{
				this._rostersBackup[i] = new ItemRoster(this._rosters[i]);
			}
			this._partyInitialEquipment = new InventoryLogic.PartyEquipment(this.OwnerParty);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00057760 File Offset: 0x00055960
		public ItemRosterElement? FindItemFromSide(InventoryLogic.InventorySide side, EquipmentElement item)
		{
			int num = this._rosters[(int)side].FindIndexOfElement(item);
			if (num >= 0)
			{
				return new ItemRosterElement?(this._rosters[(int)side].ElementAt(num));
			}
			return null;
		}

		// Token: 0x0400067A RID: 1658
		private ItemRoster[] _rosters;

		// Token: 0x0400067B RID: 1659
		private ItemRoster[] _rostersBackup;

		// Token: 0x04000681 RID: 1665
		public bool IsPreviewingItem;

		// Token: 0x04000682 RID: 1666
		private InventoryLogic.PartyEquipment _partyInitialEquipment;

		// Token: 0x04000688 RID: 1672
		private float _xpGainFromDonations;

		// Token: 0x04000689 RID: 1673
		private int _transactionDebt;

		// Token: 0x0400068A RID: 1674
		private bool _playerAcceptsTraderOffer;

		// Token: 0x0400068B RID: 1675
		private InventoryLogic.TransactionHistory _transactionHistory = new InventoryLogic.TransactionHistory();

		// Token: 0x0400068C RID: 1676
		private Dictionary<ItemCategory, float> _itemCategoryAverages = new Dictionary<ItemCategory, float>();

		// Token: 0x0400068D RID: 1677
		private bool _useBasePrices;

		// Token: 0x0400068E RID: 1678
		public InventoryManager.InventoryCategoryType MerchantItemType = InventoryManager.InventoryCategoryType.None;

		// Token: 0x020004DB RID: 1243
		public enum TransferType
		{
			// Token: 0x040014FC RID: 5372
			Neutral,
			// Token: 0x040014FD RID: 5373
			Sell,
			// Token: 0x040014FE RID: 5374
			Buy
		}

		// Token: 0x020004DC RID: 1244
		public enum InventorySide
		{
			// Token: 0x04001500 RID: 5376
			OtherInventory,
			// Token: 0x04001501 RID: 5377
			PlayerInventory,
			// Token: 0x04001502 RID: 5378
			Equipment,
			// Token: 0x04001503 RID: 5379
			None = -1
		}

		// Token: 0x020004DD RID: 1245
		// (Invoke) Token: 0x06004348 RID: 17224
		public delegate void AfterResetDelegate(InventoryLogic inventoryLogic, bool fromCancel);

		// Token: 0x020004DE RID: 1246
		// (Invoke) Token: 0x0600434C RID: 17228
		public delegate void TotalAmountChangeDelegate(int newTotalAmount);

		// Token: 0x020004DF RID: 1247
		// (Invoke) Token: 0x06004350 RID: 17232
		public delegate void ProcessResultListDelegate(InventoryLogic inventoryLogic, List<TransferCommandResult> results);

		// Token: 0x020004E0 RID: 1248
		private class PartyEquipment
		{
			// Token: 0x17000D9E RID: 3486
			// (get) Token: 0x06004353 RID: 17235 RVA: 0x00146144 File Offset: 0x00144344
			// (set) Token: 0x06004354 RID: 17236 RVA: 0x0014614C File Offset: 0x0014434C
			public Dictionary<CharacterObject, Equipment[]> CharacterEquipments { get; private set; }

			// Token: 0x06004355 RID: 17237 RVA: 0x00146155 File Offset: 0x00144355
			public PartyEquipment(MobileParty party)
			{
				this.CharacterEquipments = new Dictionary<CharacterObject, Equipment[]>();
				this.InitializeCopyFrom(party);
			}

			// Token: 0x06004356 RID: 17238 RVA: 0x00146170 File Offset: 0x00144370
			public void InitializeCopyFrom(MobileParty party)
			{
				this.CharacterEquipments = new Dictionary<CharacterObject, Equipment[]>();
				for (int i = 0; i < party.MemberRoster.Count; i++)
				{
					CharacterObject character = party.MemberRoster.GetElementCopyAtIndex(i).Character;
					if (character.IsHero)
					{
						this.CharacterEquipments.Add(character, new Equipment[]
						{
							new Equipment(character.Equipment),
							new Equipment(character.FirstCivilianEquipment)
						});
					}
				}
			}

			// Token: 0x06004357 RID: 17239 RVA: 0x001461E8 File Offset: 0x001443E8
			internal void ResetEquipment(MobileParty ownerParty)
			{
				foreach (KeyValuePair<CharacterObject, Equipment[]> keyValuePair in this.CharacterEquipments)
				{
					keyValuePair.Key.Equipment.FillFrom(keyValuePair.Value[0], true);
					keyValuePair.Key.FirstCivilianEquipment.FillFrom(keyValuePair.Value[1], true);
				}
			}

			// Token: 0x06004358 RID: 17240 RVA: 0x0014626C File Offset: 0x0014446C
			public void SetReference(InventoryLogic.PartyEquipment partyEquipment)
			{
				this.CharacterEquipments.Clear();
				this.CharacterEquipments = partyEquipment.CharacterEquipments;
			}

			// Token: 0x06004359 RID: 17241 RVA: 0x00146288 File Offset: 0x00144488
			public bool IsEqual(InventoryLogic.PartyEquipment partyEquipment)
			{
				if (partyEquipment.CharacterEquipments.Keys.Count != this.CharacterEquipments.Keys.Count)
				{
					return false;
				}
				foreach (CharacterObject characterObject in partyEquipment.CharacterEquipments.Keys)
				{
					if (!this.CharacterEquipments.Keys.Contains(characterObject))
					{
						return false;
					}
					Equipment[] array;
					if (!this.CharacterEquipments.TryGetValue(characterObject, out array))
					{
						return false;
					}
					Equipment[] array2;
					if (!partyEquipment.CharacterEquipments.TryGetValue(characterObject, out array2) || array2.Length != array.Length)
					{
						return false;
					}
					for (int i = 0; i < array.Length; i++)
					{
						if (!array[i].IsEquipmentEqualTo(array2[i]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x020004E1 RID: 1249
		private class ItemLog : IReadOnlyCollection<int>, IEnumerable<int>, IEnumerable
		{
			// Token: 0x17000D9F RID: 3487
			// (get) Token: 0x0600435A RID: 17242 RVA: 0x00146374 File Offset: 0x00144574
			public bool IsSelling
			{
				get
				{
					return this._isSelling;
				}
			}

			// Token: 0x17000DA0 RID: 3488
			// (get) Token: 0x0600435B RID: 17243 RVA: 0x0014637C File Offset: 0x0014457C
			public int Count
			{
				get
				{
					return ((IReadOnlyCollection<int>)this._transactions).Count;
				}
			}

			// Token: 0x0600435C RID: 17244 RVA: 0x00146389 File Offset: 0x00144589
			private void AddTransaction(int price, bool isSelling)
			{
				if (this._transactions.IsEmpty<int>())
				{
					this._isSelling = isSelling;
				}
				this._transactions.Add(price);
			}

			// Token: 0x0600435D RID: 17245 RVA: 0x001463AC File Offset: 0x001445AC
			private void RemoveLastTransaction()
			{
				if (!this._transactions.IsEmpty<int>())
				{
					this._transactions.RemoveAt(this._transactions.Count - 1);
					return;
				}
				Debug.FailedAssert("false", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Inventory\\InventoryLogic.cs", "RemoveLastTransaction", 1177);
			}

			// Token: 0x0600435E RID: 17246 RVA: 0x001463F8 File Offset: 0x001445F8
			public void RecordTransaction(int price, bool isSelling)
			{
				if (!this._transactions.IsEmpty<int>() && isSelling != this._isSelling)
				{
					this.RemoveLastTransaction();
					return;
				}
				this.AddTransaction(price, isSelling);
			}

			// Token: 0x0600435F RID: 17247 RVA: 0x0014641F File Offset: 0x0014461F
			public bool GetLastTransaction(out int price, out bool isSelling)
			{
				if (this._transactions.IsEmpty<int>())
				{
					price = 0;
					isSelling = false;
					return false;
				}
				price = this._transactions[this._transactions.Count - 1];
				isSelling = this._isSelling;
				return true;
			}

			// Token: 0x06004360 RID: 17248 RVA: 0x00146459 File Offset: 0x00144659
			public IEnumerator<int> GetEnumerator()
			{
				return ((IEnumerable<int>)this._transactions).GetEnumerator();
			}

			// Token: 0x06004361 RID: 17249 RVA: 0x00146466 File Offset: 0x00144666
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<int>)this._transactions).GetEnumerator();
			}

			// Token: 0x04001505 RID: 5381
			private List<int> _transactions = new List<int>();

			// Token: 0x04001506 RID: 5382
			private bool _isSelling;
		}

		// Token: 0x020004E2 RID: 1250
		public class CapacityData
		{
			// Token: 0x06004363 RID: 17251 RVA: 0x00146486 File Offset: 0x00144686
			public CapacityData(Func<int> getCapacity, Func<TextObject> getCapacityExceededWarningText, Func<TextObject> getCapacityExceededHintText)
			{
				this._getCapacity = getCapacity;
				this._getCapacityExceededWarningText = getCapacityExceededHintText;
				this._getCapacityExceededHintText = getCapacityExceededWarningText;
			}

			// Token: 0x06004364 RID: 17252 RVA: 0x001464A3 File Offset: 0x001446A3
			public int GetCapacity()
			{
				Func<int> getCapacity = this._getCapacity;
				if (getCapacity == null)
				{
					return -1;
				}
				return getCapacity();
			}

			// Token: 0x06004365 RID: 17253 RVA: 0x001464B6 File Offset: 0x001446B6
			public TextObject GetCapacityExceededWarningText()
			{
				Func<TextObject> getCapacityExceededWarningText = this._getCapacityExceededWarningText;
				if (getCapacityExceededWarningText == null)
				{
					return null;
				}
				return getCapacityExceededWarningText();
			}

			// Token: 0x06004366 RID: 17254 RVA: 0x001464C9 File Offset: 0x001446C9
			public TextObject GetCapacityExceededHintText()
			{
				Func<TextObject> getCapacityExceededHintText = this._getCapacityExceededHintText;
				if (getCapacityExceededHintText == null)
				{
					return null;
				}
				return getCapacityExceededHintText();
			}

			// Token: 0x04001507 RID: 5383
			private readonly Func<int> _getCapacity;

			// Token: 0x04001508 RID: 5384
			private readonly Func<TextObject> _getCapacityExceededWarningText;

			// Token: 0x04001509 RID: 5385
			private readonly Func<TextObject> _getCapacityExceededHintText;
		}

		// Token: 0x020004E3 RID: 1251
		private class TransactionHistory
		{
			// Token: 0x06004367 RID: 17255 RVA: 0x001464DC File Offset: 0x001446DC
			internal void RecordTransaction(EquipmentElement elementToTransfer, bool isSelling, int price)
			{
				InventoryLogic.ItemLog itemLog;
				if (!this._transactionLogs.TryGetValue(elementToTransfer, out itemLog))
				{
					itemLog = new InventoryLogic.ItemLog();
					this._transactionLogs[elementToTransfer] = itemLog;
				}
				itemLog.RecordTransaction(price, isSelling);
			}

			// Token: 0x17000DA1 RID: 3489
			// (get) Token: 0x06004368 RID: 17256 RVA: 0x00146514 File Offset: 0x00144714
			public bool IsEmpty
			{
				get
				{
					return this._transactionLogs.IsEmpty<KeyValuePair<EquipmentElement, InventoryLogic.ItemLog>>();
				}
			}

			// Token: 0x06004369 RID: 17257 RVA: 0x00146521 File Offset: 0x00144721
			public void Clear()
			{
				this._transactionLogs.Clear();
			}

			// Token: 0x0600436A RID: 17258 RVA: 0x00146530 File Offset: 0x00144730
			public bool GetLastTransfer(EquipmentElement equipmentElement, out int lastPrice, out bool lastIsSelling)
			{
				InventoryLogic.ItemLog itemLog;
				bool flag = this._transactionLogs.TryGetValue(equipmentElement, out itemLog);
				lastPrice = 0;
				lastIsSelling = false;
				return flag && itemLog.GetLastTransaction(out lastPrice, out lastIsSelling);
			}

			// Token: 0x0600436B RID: 17259 RVA: 0x00146560 File Offset: 0x00144760
			internal List<ValueTuple<ItemRosterElement, int>> GetTransferredItems(bool isSelling)
			{
				List<ValueTuple<ItemRosterElement, int>> list = new List<ValueTuple<ItemRosterElement, int>>();
				foreach (KeyValuePair<EquipmentElement, InventoryLogic.ItemLog> keyValuePair in this._transactionLogs)
				{
					if (keyValuePair.Value.Count > 0 && !keyValuePair.Value.IsSelling == isSelling)
					{
						int item = keyValuePair.Value.Sum();
						list.Add(new ValueTuple<ItemRosterElement, int>(new ItemRosterElement(keyValuePair.Key.Item, keyValuePair.Value.Count, keyValuePair.Key.ItemModifier), item));
					}
				}
				return list;
			}

			// Token: 0x0600436C RID: 17260 RVA: 0x00146620 File Offset: 0x00144820
			internal List<ValueTuple<ItemRosterElement, int>> GetBoughtItems()
			{
				return this.GetTransferredItems(true);
			}

			// Token: 0x0600436D RID: 17261 RVA: 0x00146629 File Offset: 0x00144829
			internal List<ValueTuple<ItemRosterElement, int>> GetSoldItems()
			{
				return this.GetTransferredItems(false);
			}

			// Token: 0x0400150A RID: 5386
			private Dictionary<EquipmentElement, InventoryLogic.ItemLog> _transactionLogs = new Dictionary<EquipmentElement, InventoryLogic.ItemLog>();
		}
	}
}
