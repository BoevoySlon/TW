using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FB RID: 251
	public class DefaultCompanionHiringPriceCalculationModel : CompanionHiringPriceCalculationModel
	{
		// Token: 0x0600153E RID: 5438 RVA: 0x00062540 File Offset: 0x00060740
		public override int GetCompanionHiringPrice(Hero companion)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			Settlement currentSettlement = companion.CurrentSettlement;
			Town town = (currentSettlement != null) ? currentSettlement.Town : null;
			if (town == null)
			{
				town = SettlementHelper.FindNearestTown(null, null).Town;
			}
			float num = 0f;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				EquipmentElement itemRosterElement = companion.CharacterObject.Equipment[equipmentIndex];
				if (itemRosterElement.Item != null)
				{
					num += (float)town.GetItemPrice(itemRosterElement, null, false);
				}
			}
			for (EquipmentIndex equipmentIndex2 = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex2 < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex2++)
			{
				EquipmentElement itemRosterElement2 = companion.CharacterObject.FirstCivilianEquipment[equipmentIndex2];
				if (itemRosterElement2.Item != null)
				{
					num += (float)town.GetItemPrice(itemRosterElement2, null, false);
				}
			}
			explainedNumber.Add(num / 2f, null, null);
			explainedNumber.Add((float)(companion.CharacterObject.Level * 10), null, null);
			if (Hero.MainHero.IsPartyLeader && Hero.MainHero.GetPerkValue(DefaultPerks.Steward.PaidInPromise))
			{
				explainedNumber.AddFactor(DefaultPerks.Steward.PaidInPromise.PrimaryBonus, null);
			}
			if (Hero.MainHero.PartyBelongedTo != null)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Trade.GreatInvestor, Hero.MainHero.PartyBelongedTo, false, ref explainedNumber);
			}
			return (int)explainedNumber.ResultNumber;
		}
	}
}
