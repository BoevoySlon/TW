using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000401 RID: 1025
	public class AiBanditPatrollingBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003EE6 RID: 16102 RVA: 0x00134DF1 File Offset: 0x00132FF1
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x00134E0A File Offset: 0x0013300A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x00134E0C File Offset: 0x0013300C
		public void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (!mobileParty.IsBandit)
			{
				return;
			}
			if (mobileParty.IsBanditBossParty)
			{
				return;
			}
			int num = 0;
			if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsHideout)
			{
				if (mobileParty.CurrentSettlement.Parties.CountQ((MobileParty x) => x.IsBandit && !x.IsBanditBossParty) <= Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt + 1)
				{
					return;
				}
			}
			if (mobileParty.MapFaction.Culture.CanHaveSettlement && (mobileParty.Ai.NeedTargetReset || (mobileParty.HomeSettlement.IsHideout && !mobileParty.HomeSettlement.Hideout.IsInfested)))
			{
				Settlement settlement = SettlementHelper.FindNearestHideout((Settlement x) => x.Culture == mobileParty.MapFaction.Culture && x.Hideout.IsInfested, null);
				if (settlement != null)
				{
					mobileParty.BanditPartyComponent.SetHomeHideout(settlement.Hideout);
				}
			}
			AIBehaviorTuple item = new AIBehaviorTuple(mobileParty.HomeSettlement, AiBehavior.PatrolAroundPoint, false);
			float num2 = 1f;
			if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsHideout && (mobileParty.CurrentSettlement.MapFaction == mobileParty.MapFaction || mobileParty.CurrentSettlement.Hideout.IsInfested))
			{
				int numberOfMinimumBanditPartiesInAHideoutToInfestIt = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
				int numberOfMaximumBanditPartiesInEachHideout = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesInEachHideout;
				num2 = (float)(num - numberOfMinimumBanditPartiesInAHideoutToInfestIt) / (float)(numberOfMaximumBanditPartiesInEachHideout - numberOfMinimumBanditPartiesInAHideoutToInfestIt);
			}
			float num3 = (mobileParty.CurrentSettlement != null) ? (MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat) : 0.5f;
			float item2 = 0.5f * num2 * num3;
			ValueTuple<AIBehaviorTuple, float> valueTuple = new ValueTuple<AIBehaviorTuple, float>(item, item2);
			p.AddBehaviorScore(valueTuple);
		}
	}
}
