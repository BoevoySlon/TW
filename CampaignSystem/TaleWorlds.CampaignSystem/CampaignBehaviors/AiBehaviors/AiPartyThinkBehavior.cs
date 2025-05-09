﻿using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000405 RID: 1029
	public class AiPartyThinkBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003EF9 RID: 16121 RVA: 0x00136520 File Offset: 0x00134720
		public override void RegisterEvents()
		{
			CampaignEvents.TickPartialHourlyAiEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.PartyHourlyAiTick));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x001365A0 File Offset: 0x001347A0
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x001365A4 File Offset: 0x001347A4
		private void PartyHourlyAiTick(MobileParty mobileParty)
		{
			if (mobileParty.Ai.IsDisabled || mobileParty.Ai.DoNotMakeNewDecisions)
			{
				return;
			}
			bool flag = mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty;
			int num = (flag || mobileParty.Ai.RethinkAtNextHourlyTick || (mobileParty.MapEvent != null && (mobileParty.MapEvent.IsRaid || mobileParty.MapEvent.IsSiegeAssault))) ? 1 : 6;
			if (flag && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == mobileParty && (mobileParty.CurrentSettlement != null || (mobileParty.LastVisitedSettlement != null && mobileParty.MapEvent == null && mobileParty.LastVisitedSettlement.Position2D.Distance(mobileParty.Position2D) < 1f)))
			{
				num = 6;
			}
			if (mobileParty.Ai.HourCounter % num == 0 && mobileParty != MobileParty.MainParty && (mobileParty.MapEvent == null || (mobileParty.Party == mobileParty.MapEvent.AttackerSide.LeaderParty && (mobileParty.MapEvent.IsRaid || mobileParty.MapEvent.IsSiegeAssault))))
			{
				mobileParty.Ai.HourCounter = 0;
				Army.AIBehaviorFlags aibehaviorFlags = flag ? mobileParty.Army.AIBehavior : Army.AIBehaviorFlags.Unassigned;
				IMapPoint mapPoint = flag ? mobileParty.Army.AiBehaviorObject : null;
				mobileParty.Ai.RethinkAtNextHourlyTick = false;
				PartyThinkParams thinkParamsCache = mobileParty.ThinkParamsCache;
				thinkParamsCache.Reset(mobileParty);
				CampaignEventDispatcher.Instance.AiHourlyTick(mobileParty, thinkParamsCache);
				AIBehaviorTuple aibehaviorTuple = new AIBehaviorTuple(null, AiBehavior.Hold, false);
				AIBehaviorTuple item = new AIBehaviorTuple(null, AiBehavior.Hold, false);
				float num2 = -1f;
				float num3 = -1f;
				foreach (ValueTuple<AIBehaviorTuple, float> valueTuple in thinkParamsCache.AIBehaviorScores)
				{
					float item2 = valueTuple.Item2;
					if (item2 > num2)
					{
						num2 = item2;
						aibehaviorTuple = valueTuple.Item1;
					}
					if (item2 > num3 && !valueTuple.Item1.WillGatherArmy)
					{
						num3 = item2;
						item = valueTuple.Item1;
					}
				}
				if (mobileParty.DefaultBehavior == AiBehavior.Hold || mobileParty.Ai.RethinkAtNextHourlyTick || (thinkParamsCache.CurrentObjectiveValue < 0.05f && (mobileParty.DefaultBehavior == AiBehavior.BesiegeSettlement || mobileParty.DefaultBehavior == AiBehavior.RaidSettlement || mobileParty.DefaultBehavior == AiBehavior.DefendSettlement)))
				{
					num2 = 1f;
				}
				double num4 = (aibehaviorTuple.AiBehavior == AiBehavior.PatrolAroundPoint || aibehaviorTuple.AiBehavior == AiBehavior.GoToSettlement) ? 0.03 : 0.1;
				num4 *= (double)(aibehaviorTuple.WillGatherArmy ? 2f : ((mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty) ? 0.33f : 1f));
				bool flag2 = mobileParty.Army != null;
				int num5 = 0;
				while (num5 < num && !flag2)
				{
					flag2 = (MBRandom.RandomFloat < num2);
					num5++;
				}
				if (((double)num2 > num4 && flag2) || (num2 > 0.01f && mobileParty.MapEvent == null && mobileParty.Army == null && mobileParty.DefaultBehavior == AiBehavior.Hold))
				{
					if (mobileParty.MapEvent != null && mobileParty.Party == mobileParty.MapEvent.AttackerSide.LeaderParty && !thinkParamsCache.DoNotChangeBehavior && (aibehaviorTuple.Party != mobileParty.MapEvent.MapEventSettlement || (aibehaviorTuple.AiBehavior != AiBehavior.RaidSettlement && aibehaviorTuple.AiBehavior != AiBehavior.BesiegeSettlement && aibehaviorTuple.AiBehavior != AiBehavior.AssaultSettlement)))
					{
						if (PlayerEncounter.Current != null && PlayerEncounter.Battle == mobileParty.MapEvent)
						{
							PlayerEncounter.Finish(true);
						}
						if (mobileParty.MapEvent != null)
						{
							mobileParty.MapEvent.FinalizeEvent();
						}
						if (mobileParty.SiegeEvent != null)
						{
							mobileParty.SiegeEvent.FinalizeSiegeEvent();
						}
						if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty)
						{
							foreach (MobileParty mobileParty2 in mobileParty.Army.Parties)
							{
								mobileParty2.Ai.SetMoveEscortParty(mobileParty);
							}
						}
					}
					if ((double)num2 <= num4)
					{
						aibehaviorTuple = item;
					}
					bool flag3 = aibehaviorTuple.AiBehavior == AiBehavior.RaidSettlement || aibehaviorTuple.AiBehavior == AiBehavior.BesiegeSettlement || aibehaviorTuple.AiBehavior == AiBehavior.DefendSettlement || aibehaviorTuple.AiBehavior == AiBehavior.PatrolAroundPoint;
					if (mobileParty.Army != null && mobileParty.Army.LeaderParty != mobileParty && aibehaviorTuple.AiBehavior != AiBehavior.EscortParty && (mobileParty.Army.LeaderParty.MapEvent == null || mobileParty.Army.LeaderParty.MapEvent.MapEventSettlement == null || aibehaviorTuple.Party != mobileParty.Army.LeaderParty.MapEvent.MapEventSettlement))
					{
						mobileParty.Army = null;
					}
					if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty && (mobileParty.CurrentSettlement == null || mobileParty.CurrentSettlement.SiegeEvent == null) && (aibehaviorTuple.AiBehavior != AiBehavior.GoAroundParty && aibehaviorTuple.AiBehavior != AiBehavior.PatrolAroundPoint && aibehaviorTuple.AiBehavior != AiBehavior.GoToSettlement && !flag3))
					{
						DisbandArmyAction.ApplyByUnknownReason(mobileParty.Army);
					}
					if (flag3 && mobileParty.Army == null && aibehaviorTuple.WillGatherArmy && !mobileParty.LeaderHero.Clan.IsUnderMercenaryService)
					{
						bool flag4 = MBRandom.RandomFloat < num2;
						if (aibehaviorTuple.AiBehavior == AiBehavior.DefendSettlement || flag4)
						{
							Army.ArmyTypes selectedArmyType = (aibehaviorTuple.AiBehavior == AiBehavior.BesiegeSettlement) ? Army.ArmyTypes.Besieger : ((aibehaviorTuple.AiBehavior == AiBehavior.RaidSettlement) ? Army.ArmyTypes.Raider : Army.ArmyTypes.Defender);
							((Kingdom)mobileParty.MapFaction).CreateArmy(mobileParty.LeaderHero, aibehaviorTuple.Party as Settlement, selectedArmyType);
						}
					}
					else if (!thinkParamsCache.DoNotChangeBehavior)
					{
						if (aibehaviorTuple.AiBehavior == AiBehavior.PatrolAroundPoint)
						{
							SetPartyAiAction.GetActionForPatrollingAroundSettlement(mobileParty, (Settlement)aibehaviorTuple.Party);
						}
						else if (aibehaviorTuple.AiBehavior == AiBehavior.GoToSettlement)
						{
							if (MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(mobileParty) != aibehaviorTuple.Party)
							{
								SetPartyAiAction.GetActionForVisitingSettlement(mobileParty, (Settlement)aibehaviorTuple.Party);
							}
						}
						else if (aibehaviorTuple.AiBehavior == AiBehavior.EscortParty)
						{
							SetPartyAiAction.GetActionForEscortingParty(mobileParty, (MobileParty)aibehaviorTuple.Party);
						}
						else if (aibehaviorTuple.AiBehavior == AiBehavior.RaidSettlement)
						{
							if (mobileParty.MapEvent == null || !mobileParty.MapEvent.IsRaid || mobileParty.MapEvent.MapEventSettlement != aibehaviorTuple.Party)
							{
								SetPartyAiAction.GetActionForRaidingSettlement(mobileParty, (Settlement)aibehaviorTuple.Party);
							}
						}
						else if (aibehaviorTuple.AiBehavior == AiBehavior.BesiegeSettlement)
						{
							SetPartyAiAction.GetActionForBesiegingSettlement(mobileParty, (Settlement)aibehaviorTuple.Party);
						}
						else if (aibehaviorTuple.AiBehavior == AiBehavior.DefendSettlement && mobileParty.CurrentSettlement != aibehaviorTuple.Party)
						{
							SetPartyAiAction.GetActionForDefendingSettlement(mobileParty, (Settlement)aibehaviorTuple.Party);
						}
						else if (aibehaviorTuple.AiBehavior == AiBehavior.GoAroundParty)
						{
							SetPartyAiAction.GetActionForGoingAroundParty(mobileParty, (MobileParty)aibehaviorTuple.Party);
						}
					}
				}
				else if (mobileParty.Army != null && mobileParty.Army.LeaderParty == mobileParty && mobileParty.Army.AIBehavior != Army.AIBehaviorFlags.Gathering && mobileParty.Army.AIBehavior != Army.AIBehaviorFlags.WaitingForArmyMembers)
				{
					DisbandArmyAction.ApplyByUnknownReason(mobileParty.Army);
				}
				else if (mobileParty.Army != null && mobileParty.CurrentSettlement == null && mobileParty != mobileParty.Army.LeaderParty && !thinkParamsCache.DoNotChangeBehavior)
				{
					SetPartyAiAction.GetActionForEscortingParty(mobileParty, mobileParty.Army.LeaderParty);
				}
				if (MobileParty.MainParty.Army != null)
				{
					Army army = MobileParty.MainParty.Army;
					if (mobileParty.Equals((army != null) ? army.LeaderParty : null) && (aibehaviorFlags != mobileParty.Army.AIBehavior || mobileParty.Army.AiBehaviorObject != mapPoint))
					{
						Army.ArmyLeaderThinkReason behaviorChangeExplanation = Army.GetBehaviorChangeExplanation(aibehaviorFlags, mobileParty.Army.AIBehavior);
						CampaignEventDispatcher.Instance.OnArmyLeaderThink(mobileParty.LeaderHero, behaviorChangeExplanation);
					}
				}
			}
			mobileParty.Ai.HourCounter++;
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x00136DCC File Offset: 0x00134FCC
		private void OnMakePeace(IFaction faction1, IFaction faction2, MakePeaceAction.MakePeaceDetail detail)
		{
			if (faction1.IsKingdomFaction && faction2.IsKingdomFaction)
			{
				FactionHelper.FinishAllRelatedHostileActions((Kingdom)faction1, (Kingdom)faction2);
				return;
			}
			if (!faction1.IsKingdomFaction && !faction2.IsKingdomFaction)
			{
				FactionHelper.FinishAllRelatedHostileActions((Clan)faction1, (Clan)faction2);
				return;
			}
			if (faction1.IsKingdomFaction)
			{
				FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction((Clan)faction2, (Kingdom)faction1);
				FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction((Kingdom)faction1, (Clan)faction2);
				return;
			}
			FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction((Clan)faction1, (Kingdom)faction2);
			FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction((Kingdom)faction2, (Clan)faction1);
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00136E6C File Offset: 0x0013506C
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				if (warPartyComponent.MobileParty.TargetSettlement != null)
				{
					this.CheckMobilePartyActionAccordingToSettlement(warPartyComponent.MobileParty, warPartyComponent.MobileParty.TargetSettlement);
				}
			}
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00136EDC File Offset: 0x001350DC
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			foreach (WarPartyComponent warPartyComponent in faction1.WarPartyComponents)
			{
				if (warPartyComponent.MobileParty.TargetSettlement != null)
				{
					this.CheckMobilePartyActionAccordingToSettlement(warPartyComponent.MobileParty, warPartyComponent.MobileParty.TargetSettlement);
				}
			}
			foreach (WarPartyComponent warPartyComponent2 in faction2.WarPartyComponents)
			{
				if (warPartyComponent2.MobileParty.TargetSettlement != null)
				{
					this.CheckMobilePartyActionAccordingToSettlement(warPartyComponent2.MobileParty, warPartyComponent2.MobileParty.TargetSettlement);
				}
			}
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x00136FAC File Offset: 0x001351AC
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			this.HandlePartyActionsAfterSettlementOwnerChange(settlement);
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x00136FB8 File Offset: 0x001351B8
		private void HandlePartyActionsAfterSettlementOwnerChange(Settlement settlement)
		{
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				this.CheckMobilePartyActionAccordingToSettlement(mobileParty, settlement);
			}
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x0013700C File Offset: 0x0013520C
		private void CheckMobilePartyActionAccordingToSettlement(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty.BesiegedSettlement != settlement)
			{
				if (mobileParty.Army == null)
				{
					Settlement targetSettlement = mobileParty.TargetSettlement;
					if (targetSettlement != null && (targetSettlement == settlement || (targetSettlement.IsVillage && targetSettlement.Village.Bound == settlement)))
					{
						if (mobileParty.MapEvent != null)
						{
							mobileParty.Ai.RethinkAtNextHourlyTick = true;
							return;
						}
						if (mobileParty.CurrentSettlement == null)
						{
							mobileParty.Ai.SetMoveModeHold();
							return;
						}
						mobileParty.Ai.SetMoveGoToSettlement(mobileParty.CurrentSettlement);
						mobileParty.Ai.RecalculateShortTermAi();
						return;
					}
				}
				else if (mobileParty.Army.LeaderParty == mobileParty)
				{
					Army army = mobileParty.Army;
					if (army.AiBehaviorObject == settlement || (army.AiBehaviorObject != null && ((Settlement)army.AiBehaviorObject).IsVillage && ((Settlement)army.AiBehaviorObject).Village.Bound == settlement))
					{
						army.AIBehavior = Army.AIBehaviorFlags.Unassigned;
						army.AiBehaviorObject = null;
						if (army.LeaderParty.MapEvent == null)
						{
							army.LeaderParty.Ai.SetMoveModeHold();
						}
						else
						{
							army.LeaderParty.Ai.RethinkAtNextHourlyTick = true;
						}
						army.FinishArmyObjective();
					}
				}
			}
		}
	}
}
