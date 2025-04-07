using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003B6 RID: 950
	public class PartiesBuyFoodCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A20 RID: 14880 RVA: 0x001117C6 File Offset: 0x0010F9C6
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x001117F6 File Offset: 0x0010F9F6
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x001117F8 File Offset: 0x0010F9F8
		private void TryBuyingFood(MobileParty mobileParty, Settlement settlement)
		{
			if (Campaign.Current.GameStarted && mobileParty.LeaderHero != null && (settlement.IsTown || settlement.IsVillage) && Campaign.Current.Models.MobilePartyFoodConsumptionModel.DoesPartyConsumeFood(mobileParty) && (mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty) && (settlement.IsVillage || (mobileParty.MapFaction != null && !mobileParty.MapFaction.IsAtWarWith(settlement.MapFaction))) && settlement.ItemRoster.TotalFood > 0)
			{
				PartyFoodBuyingModel partyFoodBuyingModel = Campaign.Current.Models.PartyFoodBuyingModel;
				float minimumDaysToLast = settlement.IsVillage ? partyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromVillage : partyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromTown;
				if (mobileParty.Army == null)
				{
					this.BuyFoodInternal(mobileParty, settlement, this.CalculateFoodCountToBuy(mobileParty, minimumDaysToLast));
					return;
				}
				this.BuyFoodForArmy(mobileParty, settlement, minimumDaysToLast);
			}
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x001118E8 File Offset: 0x0010FAE8
		private int CalculateFoodCountToBuy(MobileParty mobileParty, float minimumDaysToLast)
		{
			float num = (float)mobileParty.TotalFoodAtInventory / -mobileParty.FoodChange;
			float num2 = minimumDaysToLast - num;
			if (num2 > 0f)
			{
				return (int)(-mobileParty.FoodChange * num2);
			}
			return 0;
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00111920 File Offset: 0x0010FB20
		private void BuyFoodInternal(MobileParty mobileParty, Settlement settlement, int numberOfFoodItemsNeededToBuy)
		{
			if (!mobileParty.IsMainParty)
			{
				for (int i = 0; i < numberOfFoodItemsNeededToBuy; i++)
				{
					ItemRosterElement subject;
					float num;
					Campaign.Current.Models.PartyFoodBuyingModel.FindItemToBuy(mobileParty, settlement, out subject, out num);
					if (subject.EquipmentElement.Item == null)
					{
						break;
					}
					if (num <= (float)mobileParty.LeaderHero.Gold)
					{
						SellItemsAction.Apply(settlement.Party, mobileParty.Party, subject, 1, null);
					}
					if (subject.EquipmentElement.Item.HasHorseComponent && subject.EquipmentElement.Item.HorseComponent.IsLiveStock)
					{
						i += subject.EquipmentElement.Item.HorseComponent.MeatCount - 1;
					}
				}
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x001119E8 File Offset: 0x0010FBE8
		private void BuyFoodForArmy(MobileParty mobileParty, Settlement settlement, float minimumDaysToLast)
		{
			float num = mobileParty.Army.LeaderParty.FoodChange;
			foreach (MobileParty mobileParty2 in mobileParty.Army.LeaderParty.AttachedParties)
			{
				num += mobileParty2.FoodChange;
			}
			List<ValueTuple<int, int>> list = new List<ValueTuple<int, int>>(mobileParty.Army.Parties.Count);
			float num2 = mobileParty.Army.LeaderParty.FoodChange / num;
			int num3 = this.CalculateFoodCountToBuy(mobileParty.Army.LeaderParty, minimumDaysToLast);
			list.Add(new ValueTuple<int, int>((int)((float)settlement.ItemRoster.TotalFood * num2), num3));
			int num4 = num3;
			foreach (MobileParty mobileParty3 in mobileParty.Army.LeaderParty.AttachedParties)
			{
				num2 = mobileParty3.FoodChange / num;
				num3 = this.CalculateFoodCountToBuy(mobileParty3, minimumDaysToLast);
				list.Add(new ValueTuple<int, int>((int)((float)settlement.ItemRoster.TotalFood * num2), num3));
				num4 += num3;
			}
			bool flag = settlement.ItemRoster.TotalFood < num4;
			int num5 = 0;
			foreach (ValueTuple<int, int> valueTuple in list)
			{
				int numberOfFoodItemsNeededToBuy = flag ? valueTuple.Item1 : valueTuple.Item2;
				MobileParty mobileParty4 = (num5 == 0) ? mobileParty.Army.LeaderParty : mobileParty.Army.LeaderParty.AttachedParties[num5 - 1];
				if (!mobileParty4.IsMainParty)
				{
					this.BuyFoodInternal(mobileParty4, settlement, numberOfFoodItemsNeededToBuy);
				}
				num5++;
			}
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00111BDC File Offset: 0x0010FDDC
		public void HourlyTickParty(MobileParty mobileParty)
		{
			Settlement currentSettlementOfMobilePartyForAICalculation = MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(mobileParty);
			if (currentSettlementOfMobilePartyForAICalculation != null)
			{
				this.TryBuyingFood(mobileParty, currentSettlementOfMobilePartyForAICalculation);
			}
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x00111BFB File Offset: 0x0010FDFB
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (mobileParty != null)
			{
				this.TryBuyingFood(mobileParty, settlement);
			}
		}
	}
}
