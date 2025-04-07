using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000446 RID: 1094
	public static class GiveItemAction
	{
		// Token: 0x060040D8 RID: 16600 RVA: 0x0013F6C8 File Offset: 0x0013D8C8
		private static void ApplyInternal(Hero giver, Hero receiver, PartyBase giverParty, PartyBase receiverParty, in ItemRosterElement itemRosterElement)
		{
			bool flag = false;
			if (giver == null && receiver == null)
			{
				ItemRoster itemRoster = giverParty.ItemRoster;
				ItemRosterElement itemRosterElement2 = itemRosterElement;
				EquipmentElement equipmentElement = itemRosterElement2.EquipmentElement;
				itemRosterElement2 = itemRosterElement;
				itemRoster.AddToCounts(equipmentElement, -itemRosterElement2.Amount);
				ItemRoster itemRoster2 = receiverParty.ItemRoster;
				itemRosterElement2 = itemRosterElement;
				EquipmentElement equipmentElement2 = itemRosterElement2.EquipmentElement;
				itemRosterElement2 = itemRosterElement;
				itemRoster2.AddToCounts(equipmentElement2, itemRosterElement2.Amount);
				flag = true;
			}
			else if (giver.PartyBelongedTo != null && receiver.PartyBelongedTo != null)
			{
				ItemRoster itemRoster3 = giver.PartyBelongedTo.Party.ItemRoster;
				ItemRosterElement itemRosterElement2 = itemRosterElement;
				EquipmentElement equipmentElement3 = itemRosterElement2.EquipmentElement;
				itemRosterElement2 = itemRosterElement;
				itemRoster3.AddToCounts(equipmentElement3, -itemRosterElement2.Amount);
				ItemRoster itemRoster4 = receiver.PartyBelongedTo.Party.ItemRoster;
				itemRosterElement2 = itemRosterElement;
				EquipmentElement equipmentElement4 = itemRosterElement2.EquipmentElement;
				itemRosterElement2 = itemRosterElement;
				itemRoster4.AddToCounts(equipmentElement4, -itemRosterElement2.Amount);
				flag = true;
			}
			if (flag)
			{
				CampaignEventDispatcher.Instance.OnHeroOrPartyGaveItem(new ValueTuple<Hero, PartyBase>(giver, giverParty), new ValueTuple<Hero, PartyBase>(receiver, receiverParty), itemRosterElement, true);
			}
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x0013F7D5 File Offset: 0x0013D9D5
		public static void ApplyForHeroes(Hero giver, Hero receiver, in ItemRosterElement itemRosterElement)
		{
			GiveItemAction.ApplyInternal(giver, receiver, null, null, itemRosterElement);
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x0013F7E1 File Offset: 0x0013D9E1
		public static void ApplyForParties(PartyBase giverParty, PartyBase receiverParty, in ItemRosterElement itemRosterElement)
		{
			GiveItemAction.ApplyInternal(null, null, giverParty, receiverParty, itemRosterElement);
		}
	}
}
