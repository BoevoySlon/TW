using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000457 RID: 1111
	public static class SetPartyAiAction
	{
		// Token: 0x0600410F RID: 16655 RVA: 0x0014172C File Offset: 0x0013F92C
		private static void ApplyInternal(MobileParty owner, IMapPoint subject, SetPartyAiAction.SetPartyAiActionDetail detail)
		{
			if (detail == SetPartyAiAction.SetPartyAiActionDetail.GoToSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.GoToSettlement || owner.TargetSettlement != subject)
				{
					owner.Ai.SetMoveGoToSettlement((Settlement)subject);
				}
				if (owner.Army != null)
				{
					owner.Army.ArmyType = Army.ArmyTypes.Patrolling;
					owner.Army.AIBehavior = Army.AIBehaviorFlags.GoToSettlement;
					owner.Army.AiBehaviorObject = subject;
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.PatrolAroundSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.PatrolAroundPoint || owner.TargetSettlement != subject)
				{
					owner.Ai.SetMovePatrolAroundSettlement((Settlement)subject);
				}
				if (owner.Army != null)
				{
					owner.Army.ArmyType = Army.ArmyTypes.Patrolling;
					owner.Army.AIBehavior = Army.AIBehaviorFlags.Patrolling;
					owner.Army.AiBehaviorObject = subject;
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.RaidSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.RaidSettlement || owner.TargetSettlement != subject)
				{
					owner.Ai.SetMoveRaidSettlement((Settlement)subject);
					if (owner.Army != null)
					{
						owner.Army.AIBehavior = Army.AIBehaviorFlags.TravellingToAssignment;
						owner.Army.ArmyType = Army.ArmyTypes.Raider;
						owner.Army.AiBehaviorObject = subject;
						return;
					}
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.BesiegeSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.BesiegeSettlement || owner.TargetSettlement != subject)
				{
					owner.Ai.SetMoveBesiegeSettlement((Settlement)subject);
					if (owner.Army != null)
					{
						owner.Army.AIBehavior = Army.AIBehaviorFlags.TravellingToAssignment;
						owner.Army.ArmyType = Army.ArmyTypes.Besieger;
						owner.Army.AiBehaviorObject = subject;
						return;
					}
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.GoAroundParty)
			{
				if (owner.DefaultBehavior != AiBehavior.GoAroundParty || owner != subject)
				{
					owner.Ai.SetMoveGoAroundParty((MobileParty)subject);
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.EngageParty)
			{
				if (owner.DefaultBehavior != AiBehavior.EngageParty || owner != subject)
				{
					owner.Ai.SetMoveEngageParty((MobileParty)subject);
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.DefendParty)
			{
				if (owner.DefaultBehavior != AiBehavior.DefendSettlement || owner != subject)
				{
					owner.Ai.SetMoveDefendSettlement((Settlement)subject);
					if (owner.Army != null)
					{
						owner.Army.AIBehavior = Army.AIBehaviorFlags.Defending;
						owner.Army.ArmyType = Army.ArmyTypes.Defender;
						owner.Army.AiBehaviorObject = subject;
						return;
					}
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.EscortParty && (owner.DefaultBehavior != AiBehavior.EscortParty || owner.TargetParty != subject))
			{
				MobileParty mobileParty = (MobileParty)subject;
				owner.Ai.SetMoveEscortParty(mobileParty);
				if (owner.IsLordParty && mobileParty.IsLordParty && owner != MobileParty.MainParty && owner.Army == null && mobileParty.Army != null)
				{
					owner.Army = mobileParty.Army;
				}
			}
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x0014199D File Offset: 0x0013FB9D
		public static void GetAction(MobileParty owner, Settlement settlement)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, SetPartyAiAction.SetPartyAiActionDetail.GoToSettlement);
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x001419A7 File Offset: 0x0013FBA7
		public static void GetActionForVisitingSettlement(MobileParty owner, Settlement settlement)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, SetPartyAiAction.SetPartyAiActionDetail.GoToSettlement);
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x001419B1 File Offset: 0x0013FBB1
		public static void GetActionForPatrollingAroundSettlement(MobileParty owner, Settlement settlement)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, SetPartyAiAction.SetPartyAiActionDetail.PatrolAroundSettlement);
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x001419BB File Offset: 0x0013FBBB
		public static void GetActionForRaidingSettlement(MobileParty owner, Settlement settlement)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, SetPartyAiAction.SetPartyAiActionDetail.RaidSettlement);
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x001419C5 File Offset: 0x0013FBC5
		public static void GetActionForBesiegingSettlement(MobileParty owner, Settlement settlement)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, SetPartyAiAction.SetPartyAiActionDetail.BesiegeSettlement);
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x001419CF File Offset: 0x0013FBCF
		public static void GetActionForEngagingParty(MobileParty owner, MobileParty mobileParty)
		{
			SetPartyAiAction.ApplyInternal(owner, mobileParty, SetPartyAiAction.SetPartyAiActionDetail.EngageParty);
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x001419D9 File Offset: 0x0013FBD9
		public static void GetActionForGoingAroundParty(MobileParty owner, MobileParty mobileParty)
		{
			SetPartyAiAction.ApplyInternal(owner, mobileParty, SetPartyAiAction.SetPartyAiActionDetail.GoAroundParty);
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x001419E3 File Offset: 0x0013FBE3
		public static void GetActionForDefendingSettlement(MobileParty owner, Settlement settlement)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, SetPartyAiAction.SetPartyAiActionDetail.DefendParty);
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x001419ED File Offset: 0x0013FBED
		public static void GetActionForEscortingParty(MobileParty owner, MobileParty mobileParty)
		{
			SetPartyAiAction.ApplyInternal(owner, mobileParty, SetPartyAiAction.SetPartyAiActionDetail.EscortParty);
		}

		// Token: 0x0200077B RID: 1915
		private enum SetPartyAiActionDetail
		{
			// Token: 0x04001F4C RID: 8012
			GoToSettlement,
			// Token: 0x04001F4D RID: 8013
			PatrolAroundSettlement,
			// Token: 0x04001F4E RID: 8014
			RaidSettlement,
			// Token: 0x04001F4F RID: 8015
			BesiegeSettlement,
			// Token: 0x04001F50 RID: 8016
			EngageParty,
			// Token: 0x04001F51 RID: 8017
			GoAroundParty,
			// Token: 0x04001F52 RID: 8018
			DefendParty,
			// Token: 0x04001F53 RID: 8019
			EscortParty
		}
	}
}
