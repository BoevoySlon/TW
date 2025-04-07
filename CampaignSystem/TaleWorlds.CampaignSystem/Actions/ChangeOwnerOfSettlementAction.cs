using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200042F RID: 1071
	public static class ChangeOwnerOfSettlementAction
	{
		// Token: 0x0600406C RID: 16492 RVA: 0x0013DD70 File Offset: 0x0013BF70
		private static void ApplyInternal(Settlement settlement, Hero newOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			Clan ownerClan = settlement.OwnerClan;
			Hero oldOwner = (ownerClan != null) ? ownerClan.Leader : null;
			if (settlement.IsFortification)
			{
				settlement.Town.OwnerClan = newOwner.Clan;
			}
			if (settlement.IsFortification)
			{
				if (settlement.Town.GarrisonParty == null)
				{
					settlement.AddGarrisonParty(false);
				}
				ChangeGovernorAction.RemoveGovernorOfIfExists(settlement.Town);
			}
			settlement.Party.SetVisualAsDirty();
			foreach (Village village in settlement.BoundVillages)
			{
				village.Settlement.Party.SetVisualAsDirty();
				if (village.VillagerPartyComponent != null && newOwner != null)
				{
					foreach (MobileParty mobileParty in MobileParty.All)
					{
						if (mobileParty.MapEvent == null && mobileParty != MobileParty.MainParty && mobileParty.ShortTermTargetParty == village.VillagerPartyComponent.MobileParty && !mobileParty.MapFaction.IsAtWarWith(newOwner.MapFaction))
						{
							mobileParty.Ai.SetMoveModeHold();
						}
					}
				}
			}
			bool openToClaim = (detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege || detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByRevolt || detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByClanDestruction || detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByLeaveFaction) && settlement.IsFortification;
			if (newOwner != null)
			{
				IFaction mapFaction = newOwner.MapFaction;
				if (settlement.Party.MapEvent != null && !settlement.Party.MapEvent.AttackerSide.LeaderParty.MapFaction.IsAtWarWith(mapFaction) && settlement.Party.MapEvent.Winner == null)
				{
					settlement.Party.MapEvent.DiplomaticallyFinished = true;
					foreach (WarPartyComponent warPartyComponent in settlement.MapFaction.WarPartyComponents)
					{
						MobileParty mobileParty2 = warPartyComponent.MobileParty;
						if (mobileParty2.DefaultBehavior == AiBehavior.DefendSettlement && mobileParty2.TargetSettlement == settlement && mobileParty2.CurrentSettlement == null)
						{
							mobileParty2.Ai.SetMoveModeHold();
						}
					}
					settlement.Party.MapEvent.Update();
				}
				foreach (Clan clan in Clan.NonBanditFactions)
				{
					if (mapFaction == null || (clan.Kingdom == null && !clan.IsAtWarWith(mapFaction)) || (clan.Kingdom != null && !clan.Kingdom.IsAtWarWith(mapFaction)))
					{
						foreach (WarPartyComponent warPartyComponent2 in clan.WarPartyComponents)
						{
							MobileParty mobileParty3 = warPartyComponent2.MobileParty;
							if (mobileParty3.BesiegedSettlement != settlement && (mobileParty3.DefaultBehavior == AiBehavior.RaidSettlement || mobileParty3.DefaultBehavior == AiBehavior.BesiegeSettlement || mobileParty3.DefaultBehavior == AiBehavior.AssaultSettlement) && mobileParty3.TargetSettlement == settlement)
							{
								Army army = mobileParty3.Army;
								if (army != null)
								{
									army.FinishArmyObjective();
								}
								mobileParty3.Ai.SetMoveModeHold();
							}
						}
					}
				}
			}
			CampaignEventDispatcher.Instance.OnSettlementOwnerChanged(settlement, openToClaim, newOwner, oldOwner, capturerHero, detail);
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0013E0DC File Offset: 0x0013C2DC
		public static void ApplyByDefault(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.Default);
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0013E0E7 File Offset: 0x0013C2E7
		public static void ApplyByKingDecision(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByKingDecision);
			if (settlement.Town != null)
			{
				settlement.Town.IsOwnerUnassigned = false;
			}
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x0013E106 File Offset: 0x0013C306
		public static void ApplyBySiege(Hero newOwner, Hero capturerHero, Settlement settlement)
		{
			if (settlement.Town != null)
			{
				settlement.Town.LastCapturedBy = capturerHero.Clan;
			}
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, newOwner, capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege);
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x0013E12A File Offset: 0x0013C32A
		public static void ApplyByLeaveFaction(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByLeaveFaction);
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x0013E135 File Offset: 0x0013C335
		public static void ApplyByBarter(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByBarter);
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0013E140 File Offset: 0x0013C340
		public static void ApplyByRebellion(Hero hero, Settlement settlement)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, hero, hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByRebellion);
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0013E14B File Offset: 0x0013C34B
		public static void ApplyByDestroyClan(Settlement settlement, Hero newOwner)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, newOwner, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByClanDestruction);
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0013E156 File Offset: 0x0013C356
		public static void ApplyByGift(Settlement settlement, Hero newOwner)
		{
			ChangeOwnerOfSettlementAction.ApplyInternal(settlement, newOwner, null, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.ByGift);
		}

		// Token: 0x0200076A RID: 1898
		public enum ChangeOwnerOfSettlementDetail
		{
			// Token: 0x04001EFE RID: 7934
			Default,
			// Token: 0x04001EFF RID: 7935
			BySiege,
			// Token: 0x04001F00 RID: 7936
			ByBarter,
			// Token: 0x04001F01 RID: 7937
			ByRevolt,
			// Token: 0x04001F02 RID: 7938
			ByLeaveFaction,
			// Token: 0x04001F03 RID: 7939
			ByKingDecision,
			// Token: 0x04001F04 RID: 7940
			ByGift,
			// Token: 0x04001F05 RID: 7941
			ByRebellion,
			// Token: 0x04001F06 RID: 7942
			ByClanDestruction
		}
	}
}
