using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000406 RID: 1030
	public class AiPatrollingBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003F03 RID: 16131 RVA: 0x0013713F File Offset: 0x0013533F
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x0013716F File Offset: 0x0013536F
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this._disbandPartyCampaignBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x00137181 File Offset: 0x00135381
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x00137184 File Offset: 0x00135384
		private void CalculatePatrollingScoreForSettlement(Settlement settlement, PartyThinkParams p, float patrollingScoreAdjustment)
		{
			MobileParty mobilePartyOf = p.MobilePartyOf;
			AIBehaviorTuple item = new AIBehaviorTuple(settlement, AiBehavior.PatrolAroundPoint, false);
			float num = Campaign.Current.Models.TargetScoreCalculatingModel.CalculatePatrollingScoreForSettlement(settlement, mobilePartyOf);
			num *= patrollingScoreAdjustment;
			if (!mobilePartyOf.IsDisbanding)
			{
				IDisbandPartyCampaignBehavior disbandPartyCampaignBehavior = this._disbandPartyCampaignBehavior;
				if (disbandPartyCampaignBehavior == null || !disbandPartyCampaignBehavior.IsPartyWaitingForDisband(mobilePartyOf))
				{
					goto IL_52;
				}
			}
			num *= 0.25f;
			IL_52:
			float num2;
			if (p.TryGetBehaviorScore(item, out num2))
			{
				p.SetBehaviorScore(item, num2 + num);
				return;
			}
			ValueTuple<AIBehaviorTuple, float> valueTuple = new ValueTuple<AIBehaviorTuple, float>(item, num);
			p.AddBehaviorScore(valueTuple);
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0013720C File Offset: 0x0013540C
		private void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (mobileParty.IsMilitia || mobileParty.IsCaravan || mobileParty.IsVillager || mobileParty.IsBandit || mobileParty.IsDisbanding || (!mobileParty.MapFaction.IsMinorFaction && !mobileParty.MapFaction.IsKingdomFaction && !mobileParty.MapFaction.Leader.IsLord))
			{
				return;
			}
			Settlement currentSettlement = mobileParty.CurrentSettlement;
			if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null)
			{
				return;
			}
			float b;
			if (mobileParty.Army != null)
			{
				float num = 0f;
				foreach (MobileParty mobileParty2 in mobileParty.Army.Parties)
				{
					float num2 = PartyBaseHelper.FindPartySizeNormalLimit(mobileParty2);
					float num3 = mobileParty2.PartySizeRatio / num2;
					num += num3;
				}
				b = num / (float)mobileParty.Army.Parties.Count;
			}
			else
			{
				float num4 = PartyBaseHelper.FindPartySizeNormalLimit(mobileParty);
				b = mobileParty.PartySizeRatio / num4;
			}
			float num5 = MathF.Sqrt(MathF.Min(1f, b));
			float num6 = (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsFortification && mobileParty.CurrentSettlement.IsUnderSiege) ? 0f : 1f;
			num6 *= num5;
			if (mobileParty.Party.MapFaction.Settlements.Count > 0)
			{
				using (List<Settlement>.Enumerator enumerator2 = mobileParty.Party.MapFaction.Settlements.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Settlement settlement = enumerator2.Current;
						if (settlement.IsTown || settlement.IsVillage || settlement.MapFaction.IsMinorFaction)
						{
							this.CalculatePatrollingScoreForSettlement(settlement, p, num6);
						}
					}
					return;
				}
			}
			int num7 = -1;
			do
			{
				num7 = SettlementHelper.FindNextSettlementAroundMapPoint(mobileParty, Campaign.AverageDistanceBetweenTwoFortifications * 5f, num7);
				if (num7 >= 0 && Settlement.All[num7].IsTown)
				{
					this.CalculatePatrollingScoreForSettlement(Settlement.All[num7], p, num6);
				}
			}
			while (num7 >= 0);
		}

		// Token: 0x0400126B RID: 4715
		private IDisbandPartyCampaignBehavior _disbandPartyCampaignBehavior;
	}
}
