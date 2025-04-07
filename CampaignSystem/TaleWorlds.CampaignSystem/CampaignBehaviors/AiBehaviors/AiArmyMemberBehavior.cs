using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000400 RID: 1024
	public class AiArmyMemberBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003EE1 RID: 16097 RVA: 0x00134CC4 File Offset: 0x00132EC4
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x00134CF4 File Offset: 0x00132EF4
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x00134CF8 File Offset: 0x00132EF8
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			for (int i = 0; i < siegeEvent.BesiegedSettlement.Parties.Count; i++)
			{
				if (siegeEvent.BesiegedSettlement.Parties[i].IsLordParty)
				{
					siegeEvent.BesiegedSettlement.Parties[i].Ai.SetMoveModeHold();
				}
			}
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x00134D54 File Offset: 0x00132F54
		public void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty)
			{
				return;
			}
			if (mobileParty.AttachedTo == null)
			{
				if (mobileParty.Army.LeaderParty.CurrentSettlement != null && mobileParty.Army.LeaderParty.CurrentSettlement.IsUnderSiege)
				{
					return;
				}
				if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsUnderSiege)
				{
					return;
				}
			}
			AIBehaviorTuple item = new AIBehaviorTuple(mobileParty.Army.LeaderParty, AiBehavior.EscortParty, false);
			ValueTuple<AIBehaviorTuple, float> valueTuple = new ValueTuple<AIBehaviorTuple, float>(item, 0.25f);
			p.AddBehaviorScore(valueTuple);
		}

		// Token: 0x0400125F RID: 4703
		private const float FollowingArmyLeaderDefaultScore = 0.25f;
	}
}
