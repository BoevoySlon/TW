using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200007B RID: 123
	public static class EncounterManager
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x000487CE File Offset: 0x000469CE
		public static EncounterModel EncounterModel
		{
			get
			{
				return Campaign.Current.Models.EncounterModel;
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x000487DF File Offset: 0x000469DF
		public static void Tick(float dt)
		{
			EncounterManager.HandleEncounters(dt);
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x000487E8 File Offset: 0x000469E8
		private static void HandleEncounters(float dt)
		{
			if (Campaign.Current.TimeControlMode != CampaignTimeControlMode.Stop)
			{
				for (int i = 0; i < Campaign.Current.MobileParties.Count; i++)
				{
					EncounterManager.HandleEncounterForMobileParty(Campaign.Current.MobileParties[i], dt);
				}
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00048834 File Offset: 0x00046A34
		public static void HandleEncounterForMobileParty(MobileParty mobileParty, float dt)
		{
			if (mobileParty.IsActive && mobileParty.AttachedTo == null && mobileParty.MapEventSide == null && (mobileParty.CurrentSettlement == null || mobileParty.IsGarrison) && (mobileParty.BesiegedSettlement == null || mobileParty.ShortTermBehavior == AiBehavior.AssaultSettlement) && (mobileParty.IsCurrentlyEngagingParty || mobileParty.IsCurrentlyEngagingSettlement || (mobileParty.Ai.AiBehaviorMapEntity != null && mobileParty.ShortTermBehavior == AiBehavior.GoToPoint && !(mobileParty.Ai.AiBehaviorMapEntity is Settlement) && !(mobileParty.Ai.AiBehaviorMapEntity is MobileParty) && (mobileParty.Party != PartyBase.MainParty || PlayerEncounter.Current == null))))
			{
				if (mobileParty.IsCurrentlyEngagingSettlement && mobileParty.ShortTermTargetSettlement != null && mobileParty.ShortTermTargetSettlement == mobileParty.CurrentSettlement)
				{
					return;
				}
				if (mobileParty.IsCurrentlyEngagingParty && (!mobileParty.ShortTermTargetParty.IsActive || (mobileParty.ShortTermTargetParty.CurrentSettlement != null && (mobileParty.ShortTermTargetParty.MapEvent == null || (mobileParty.ShortTermTargetParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker).MapFaction != mobileParty.MapFaction && mobileParty.ShortTermTargetParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender).MapFaction != mobileParty.MapFaction)))))
				{
					return;
				}
				Vec2 v;
				float num;
				EncounterManager.GetEncounterTargetPoint(dt, mobileParty, out v, out num);
				float length = (mobileParty.Position2D - v).Length;
				if ((mobileParty.BesiegedSettlement != null && mobileParty.BesiegedSettlement == mobileParty.TargetSettlement) || length < num)
				{
					mobileParty.Ai.AiBehaviorMapEntity.OnPartyInteraction(mobileParty);
				}
			}
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x000489D0 File Offset: 0x00046BD0
		public static void StartPartyEncounter(PartyBase attackerParty, PartyBase defenderParty)
		{
			bool flag = PartyBase.MainParty.MapEvent != null && (PartyBase.MainParty.MapEvent.InvolvedParties.Contains(attackerParty) || PartyBase.MainParty.MapEvent.InvolvedParties.Contains(defenderParty));
			if (defenderParty == PartyBase.MainParty && PlayerSiege.PlayerSiegeEvent != null)
			{
				Debug.Print("\nPlayerSiege is interrupted\n", 0, Debug.DebugColor.DarkGreen, 64UL);
			}
			if (attackerParty == PartyBase.MainParty || defenderParty == PartyBase.MainParty || flag)
			{
				if (PartyBase.MainParty.MapEvent != null && PlayerEncounter.IsActive && ((PartyBase.MainParty.MapEvent.AttackerSide.TroopCount > 0 && PartyBase.MainParty.MapEvent.DefenderSide.TroopCount > 0) || PartyBase.MainParty.MapEvent.PartiesOnSide(PlayerEncounter.Current.OpponentSide).FindIndex((MapEventParty party) => party.Party == defenderParty) >= 0 || (PartyBase.MainParty.MapEvent.AttackerSide.LeaderParty != MobileParty.MainParty.Party && PartyBase.MainParty.MapEvent.DefenderSide.LeaderParty != MobileParty.MainParty.Party)))
				{
					PlayerEncounter.Current.OnPartyJoinEncounter(attackerParty.MobileParty);
				}
				else if (((attackerParty == PartyBase.MainParty || defenderParty == PartyBase.MainParty) && !PlayerEncounter.IsActive) || (PlayerEncounter.EncounterSettlement != null && Settlement.CurrentSettlement != null && PlayerEncounter.EncounterSettlement == Settlement.CurrentSettlement))
				{
					EncounterManager.RestartPlayerEncounter(attackerParty, defenderParty);
				}
			}
			else if (attackerParty.IsActive && defenderParty.IsActive)
			{
				if (attackerParty.MobileParty.Army != null && defenderParty == PartyBase.MainParty)
				{
					MergePartiesAction.Apply(defenderParty, attackerParty);
				}
				else
				{
					StartBattleAction.Apply(attackerParty, defenderParty);
				}
			}
			if (defenderParty.SiegeEvent != null && defenderParty != PartyBase.MainParty && defenderParty.SiegeEvent.BesiegerCamp != null && defenderParty.SiegeEvent.BesiegerCamp.HasInvolvedPartyForEventType(PartyBase.MainParty, MapEvent.BattleTypes.Siege) && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty))
			{
				EncounterManager.StartPartyEncounter(PartyBase.MainParty, attackerParty);
			}
			if (attackerParty != PartyBase.MainParty && attackerParty.MapEvent != null && attackerParty.MapEvent.IsSallyOut && attackerParty.MapEvent.MapEventSettlement == MobileParty.MainParty.CurrentSettlement && MobileParty.MainParty.Army == null)
			{
				GameMenu.SwitchToMenu("join_sally_out");
			}
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00048C8C File Offset: 0x00046E8C
		public static void StartSettlementEncounter(MobileParty attackerParty, Settlement settlement)
		{
			if (attackerParty.DefaultBehavior == AiBehavior.BesiegeSettlement && attackerParty.TargetSettlement == settlement && attackerParty.ShortTermBehavior != AiBehavior.AssaultSettlement)
			{
				if (attackerParty.BesiegedSettlement == null)
				{
					if (settlement.SiegeEvent == null)
					{
						Campaign.Current.SiegeEventManager.StartSiegeEvent(settlement, attackerParty);
					}
					else
					{
						MapEventSide mapEventSide = settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEventSide;
						attackerParty.BesiegerCamp = settlement.SiegeEvent.BesiegerCamp;
						if (mapEventSide != null)
						{
							attackerParty.MapEventSide = mapEventSide;
						}
					}
				}
				if (settlement.Party.MapEvent == null)
				{
					return;
				}
			}
			if (!attackerParty.IsVillager && attackerParty != MobileParty.MainParty && settlement.IsVillage && settlement.Village.VillageState == Village.VillageStates.Looted)
			{
				attackerParty.Ai.SetMoveModeHold();
				return;
			}
			if (attackerParty == MobileParty.MainParty)
			{
				PlayerEncounter.Start();
				PlayerEncounter.Current.Init(attackerParty.Party, settlement.Party, settlement);
				return;
			}
			if (attackerParty.Aggressiveness > 0.01f && PartyBase.MainParty.MapEvent != null && PartyBase.MainParty.MapEvent.MapEventSettlement == settlement)
			{
				if (PlayerEncounter.IsActive)
				{
					if (attackerParty.MapFaction == MobileParty.MainParty.MapFaction || (PartyBase.MainParty.MapEvent.AttackerSide.LeaderParty != PartyBase.MainParty && PartyBase.MainParty.MapEvent.DefenderSide.LeaderParty != PartyBase.MainParty))
					{
						PlayerEncounter.Current.OnPartyJoinEncounter(attackerParty);
					}
					else
					{
						if (PlayerEncounter.IsActive)
						{
							PlayerEncounter.Finish(true);
						}
						EncounterManager.RestartPlayerEncounter(attackerParty.Party, PartyBase.MainParty);
					}
				}
			}
			else
			{
				bool flag = MobileParty.MainParty.CurrentSettlement == settlement;
				MapEvent mapEvent = settlement.Party.MapEvent;
				if (mapEvent != null && !mapEvent.IsFinalized && (mapEvent.AttackerSide.MapFaction == attackerParty.MapFaction || mapEvent.DefenderSide.MapFaction == attackerParty.MapFaction))
				{
					if (flag && attackerParty.AttachedTo == null)
					{
						PlayerEncounter.Finish(true);
					}
					settlement.Party.MapEventSide = ((mapEvent.AttackerSide.MapFaction == attackerParty.MapFaction) ? mapEvent.DefenderSide : mapEvent.AttackerSide);
				}
				else if (settlement.Party.MapEvent == null && attackerParty != MobileParty.MainParty && attackerParty.ShortTermBehavior == AiBehavior.RaidSettlement && attackerParty.ShortTermTargetSettlement == settlement && FactionManager.IsAtWarAgainstFaction(attackerParty.MapFaction, settlement.MapFaction))
				{
					if (flag)
					{
						PlayerEncounter.Finish(false);
					}
					if (settlement.SettlementHitPoints > 0.001f)
					{
						StartBattleAction.ApplyStartRaid(attackerParty, settlement);
					}
					if (flag)
					{
						if (MobileParty.MainParty.MapFaction == settlement.MapFaction)
						{
							PlayerEncounter.Start();
							PlayerEncounter.Current.Init(attackerParty.Party, settlement.Party, settlement);
						}
						else
						{
							LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
						}
					}
				}
				else if (attackerParty != MobileParty.MainParty && attackerParty.ShortTermBehavior == AiBehavior.AssaultSettlement && attackerParty.ShortTermTargetSettlement == settlement && FactionManager.IsAtWarAgainstFaction(attackerParty.MapFaction, settlement.MapFaction))
				{
					if (flag)
					{
						PlayerEncounter.Finish(false);
					}
					bool flag2 = settlement.Party.MapEvent == null;
					StartBattleAction.ApplyStartAssaultAgainstWalls(attackerParty, settlement);
					if (attackerParty.MapEvent.DefenderSide.TroopCount == 0 && (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender || MobileParty.MainParty.CurrentSettlement != settlement))
					{
						bool flag3 = MobileParty.MainParty.BesiegedSettlement == settlement;
						if (flag3 && PlayerEncounter.Current == null)
						{
							EncounterManager.StartSettlementEncounter((MobileParty.MainParty.Army != null) ? MobileParty.MainParty.Army.LeaderParty : MobileParty.MainParty, settlement);
						}
						attackerParty.MapEvent.SetOverrideWinner(BattleSideEnum.Attacker);
						attackerParty.MapEvent.FinalizeEvent();
						if (flag3)
						{
							GameMenu.SwitchToMenu("menu_settlement_taken");
						}
						return;
					}
					if (attackerParty.ShortTermBehavior == AiBehavior.AssaultSettlement && flag2 && attackerParty != MobileParty.MainParty && PlayerEncounter.Current != null && PlayerEncounter.EncounterSettlement == settlement && MobileParty.MainParty.CurrentSettlement == null)
					{
						PlayerEncounter.Finish(true);
					}
					if (MobileParty.MainParty.BesiegedSettlement == settlement && (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty))
					{
						EncounterManager.StartSettlementEncounter(MobileParty.MainParty, settlement);
					}
					else if (flag)
					{
						if (attackerParty.MapEvent.CanPartyJoinBattle(PartyBase.MainParty, settlement.BattleSide))
						{
							PlayerEncounter.Start();
							PlayerEncounter.Current.Init(attackerParty.Party, settlement.Party, settlement);
						}
						else
						{
							LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
						}
					}
				}
				else if ((attackerParty.ShortTermBehavior == AiBehavior.GoToSettlement && attackerParty.ShortTermTargetSettlement == settlement) || attackerParty.Ai.IsDisabled || (attackerParty.Army != null && attackerParty.Army.LeaderParty.AttachedParties.Contains(attackerParty) && attackerParty.Army.LeaderParty.CurrentSettlement == settlement))
				{
					EnterSettlementAction.ApplyForParty(attackerParty, settlement);
				}
			}
			bool flag4 = attackerParty != null && (attackerParty.Army == null || attackerParty.Army.LeaderParty == attackerParty) && attackerParty.CurrentSettlement == settlement && !attackerParty.IsVillager && !attackerParty.IsMilitia && attackerParty != MobileParty.MainParty && attackerParty.MapEvent == null && settlement != null && settlement.IsVillage;
			if (attackerParty.Army != null && attackerParty.Army.LeaderParty == attackerParty && attackerParty != MobileParty.MainParty && !flag4)
			{
				foreach (MobileParty mobileParty in attackerParty.Army.LeaderParty.AttachedParties)
				{
					if (mobileParty.MapEvent == null)
					{
						EncounterManager.StartSettlementEncounter(mobileParty, settlement);
					}
				}
			}
			if (flag4)
			{
				LeaveSettlementAction.ApplyForParty(attackerParty);
				attackerParty.Ai.SetMoveModeHold();
				if (attackerParty != MobileParty.MainParty && (MobileParty.MainParty.Army == null || attackerParty != MobileParty.MainParty.Army.LeaderParty))
				{
					attackerParty.Ai.RethinkAtNextHourlyTick = true;
				}
			}
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0004926C File Offset: 0x0004746C
		private static void GetEncounterTargetPoint(float dt, MobileParty mobileParty, out Vec2 targetPoint, out float neededMaximumDistanceForEncountering)
		{
			if (mobileParty.Army != null)
			{
				neededMaximumDistanceForEncountering = MathF.Clamp(EncounterManager.EncounterModel.NeededMaximumDistanceForEncounteringMobileParty * MathF.Sqrt((float)(mobileParty.Army.LeaderParty.AttachedParties.Count + 1)), MathF.Max(EncounterManager.EncounterModel.NeededMaximumDistanceForEncounteringMobileParty, dt * EncounterManager.EncounterModel.EstimatedMaximumMobilePartySpeedExceptPlayer), MathF.Max(EncounterManager.EncounterModel.MaximumAllowedDistanceForEncounteringMobilePartyInArmy, dt * (EncounterManager.EncounterModel.EstimatedMaximumMobilePartySpeedExceptPlayer + 0.01f)));
			}
			else
			{
				neededMaximumDistanceForEncountering = MathF.Max(EncounterManager.EncounterModel.NeededMaximumDistanceForEncounteringMobileParty, dt * EncounterManager.EncounterModel.EstimatedMaximumMobilePartySpeedExceptPlayer);
			}
			if (mobileParty.IsCurrentlyEngagingSettlement)
			{
				targetPoint = mobileParty.ShortTermTargetSettlement.GatePosition;
				neededMaximumDistanceForEncountering = (mobileParty.ShortTermTargetSettlement.IsTown ? EncounterManager.EncounterModel.NeededMaximumDistanceForEncounteringTown : EncounterManager.EncounterModel.NeededMaximumDistanceForEncounteringVillage);
				return;
			}
			if (mobileParty.Army != null && mobileParty.Army.LeaderParty != mobileParty && mobileParty.ShortTermTargetParty.MapEvent != null && mobileParty.ShortTermTargetParty.MapEvent == mobileParty.Army.LeaderParty.MapEvent && mobileParty.Army.LeaderParty.AttachedParties.Contains(mobileParty))
			{
				targetPoint = mobileParty.Position2D;
				return;
			}
			if (mobileParty.CurrentSettlement != null && mobileParty.ShortTermTargetParty.BesiegedSettlement == mobileParty.CurrentSettlement)
			{
				targetPoint = mobileParty.CurrentSettlement.GatePosition;
				return;
			}
			targetPoint = mobileParty.Ai.AiBehaviorMapEntity.InteractionPosition;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x000493F4 File Offset: 0x000475F4
		private static void RestartPlayerEncounter(PartyBase attackerParty, PartyBase defenderParty)
		{
			Settlement settlement = null;
			if (MobileParty.MainParty.MapEvent != null && MobileParty.MainParty.MapEvent.IsRaid)
			{
				settlement = MobileParty.MainParty.MapEvent.MapEventSettlement;
			}
			if (PlayerEncounter.Current != null && (PlayerEncounter.EncounteredParty != attackerParty || PartyBase.MainParty != defenderParty) && (PlayerEncounter.EncounteredParty != defenderParty || PartyBase.MainParty != attackerParty))
			{
				PlayerEncounter.Finish(false);
			}
			if (PlayerEncounter.Current == null)
			{
				PlayerEncounter.Start();
			}
			if (attackerParty == PartyBase.MainParty && defenderParty.IsMobile && defenderParty.MobileParty.IsCurrentlyEngagingParty && defenderParty.MobileParty.ShortTermTargetParty == MobileParty.MainParty)
			{
				attackerParty = defenderParty;
				defenderParty = PartyBase.MainParty;
			}
			PlayerEncounter.Current.Init(attackerParty, defenderParty, settlement);
		}
	}
}
