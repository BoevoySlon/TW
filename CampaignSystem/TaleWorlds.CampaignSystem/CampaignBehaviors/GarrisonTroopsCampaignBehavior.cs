using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000391 RID: 913
	public class GarrisonTroopsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060036DC RID: 14044 RVA: 0x000F60A9 File Offset: 0x000F42A9
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUpEvent));
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x000F60D9 File Offset: 0x000F42D9
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x000F60DC File Offset: 0x000F42DC
		public void OnNewGameCreatedPartialFollowUpEvent(CampaignGameStarter starter, int i)
		{
			List<Settlement> list = Campaign.Current.Settlements.WhereQ((Settlement x) => x.IsFortification).ToList<Settlement>();
			int count = list.Count;
			int num = count / 100 + ((count % 100 > i) ? 1 : 0);
			int num2 = count / 100 * i;
			for (int j = 0; j < i; j++)
			{
				num2 += ((count % 100 > j) ? 1 : 0);
			}
			for (int k = 0; k < num; k++)
			{
				list[num2 + k].AddGarrisonParty(true);
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x000F617C File Offset: 0x000F437C
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (!Campaign.Current.GameStarted)
			{
				return;
			}
			if (mobileParty != null && mobileParty.IsLordParty && !mobileParty.IsDisbanding && mobileParty.LeaderHero != null && settlement.IsFortification && FactionManager.IsAlliedWithFaction(mobileParty.MapFaction, settlement.MapFaction) && (settlement.OwnerClan != Clan.PlayerClan || settlement.Town.IsOwnerUnassigned))
			{
				if (mobileParty.Army != null)
				{
					if (mobileParty.Army.LeaderParty == mobileParty)
					{
						this.TryLeaveOrTakeTroopsFromGarrisonForArmy(mobileParty);
						return;
					}
				}
				else if (!mobileParty.IsMainParty)
				{
					ValueTuple<int, int> garrisonLeaveOrTakeDataOfParty = this.GetGarrisonLeaveOrTakeDataOfParty(mobileParty);
					this.ApplyTroopLeaveOrTakeData(mobileParty, garrisonLeaveOrTakeDataOfParty.Item1, garrisonLeaveOrTakeDataOfParty.Item2);
				}
			}
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x000F6230 File Offset: 0x000F4430
		private void TryLeaveOrTakeTroopsFromGarrisonForArmy(MobileParty mobileParty)
		{
			List<ValueTuple<MobileParty, int, int>> list = new List<ValueTuple<MobileParty, int, int>>();
			ValueTuple<int, int> garrisonLeaveOrTakeDataOfParty = this.GetGarrisonLeaveOrTakeDataOfParty(mobileParty);
			list.Add(new ValueTuple<MobileParty, int, int>(mobileParty, garrisonLeaveOrTakeDataOfParty.Item1, garrisonLeaveOrTakeDataOfParty.Item2));
			foreach (MobileParty mobileParty2 in mobileParty.AttachedParties)
			{
				ValueTuple<int, int> garrisonLeaveOrTakeDataOfParty2 = this.GetGarrisonLeaveOrTakeDataOfParty(mobileParty2);
				list.Add(new ValueTuple<MobileParty, int, int>(mobileParty2, garrisonLeaveOrTakeDataOfParty2.Item1, garrisonLeaveOrTakeDataOfParty2.Item2));
			}
			foreach (ValueTuple<MobileParty, int, int> valueTuple in list)
			{
				MobileParty item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				if (item != MobileParty.MainParty)
				{
					this.ApplyTroopLeaveOrTakeData(item, item2, item3);
				}
			}
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000F6328 File Offset: 0x000F4528
		private ValueTuple<int, int> GetGarrisonLeaveOrTakeDataOfParty(MobileParty mobileParty)
		{
			Settlement currentSettlement = mobileParty.CurrentSettlement;
			int num = Campaign.Current.Models.SettlementGarrisonModel.FindNumberOfTroopsToLeaveToGarrison(mobileParty, currentSettlement);
			int item = 0;
			if (num <= 0 && mobileParty.LeaderHero.Clan == currentSettlement.OwnerClan && !mobileParty.IsWageLimitExceeded())
			{
				item = Campaign.Current.Models.SettlementGarrisonModel.FindNumberOfTroopsToTakeFromGarrison(mobileParty, mobileParty.CurrentSettlement, 0f);
			}
			return new ValueTuple<int, int>(num, item);
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x000F639A File Offset: 0x000F459A
		private void ApplyTroopLeaveOrTakeData(MobileParty party, int numberOfTroopsToLeave, int numberOfTroopToTake)
		{
			if (numberOfTroopsToLeave > 0)
			{
				LeaveTroopsToSettlementAction.Apply(party, party.CurrentSettlement, numberOfTroopsToLeave, true);
				return;
			}
			if (numberOfTroopToTake > 0)
			{
				LeaveTroopsToSettlementAction.Apply(party, party.CurrentSettlement, -numberOfTroopToTake, false);
			}
		}
	}
}
