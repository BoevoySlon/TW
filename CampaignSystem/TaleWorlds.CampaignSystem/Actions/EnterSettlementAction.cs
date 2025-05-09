﻿using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000441 RID: 1089
	public static class EnterSettlementAction
	{
		// Token: 0x060040B9 RID: 16569 RVA: 0x0013EF70 File Offset: 0x0013D170
		private static void ApplyInternal(Hero hero, MobileParty mobileParty, Settlement settlement, EnterSettlementAction.EnterSettlementDetail detail, object subject = null, bool isPlayerInvolved = false)
		{
			if (mobileParty != null && mobileParty.IsDisbanding && mobileParty.TargetSettlement == settlement)
			{
				DestroyPartyAction.ApplyForDisbanding(mobileParty, settlement);
			}
			else
			{
				CampaignEventDispatcher.Instance.OnSettlementEntered(mobileParty, settlement, hero);
				CampaignEventDispatcher.Instance.OnAfterSettlementEntered(mobileParty, settlement, hero);
				if (detail == EnterSettlementAction.EnterSettlementDetail.Prisoner)
				{
					if (hero != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, null, hero, false);
					}
					if (mobileParty != null)
					{
						CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(settlement, mobileParty.PrisonRoster.ToFlattenedRoster(), null, false);
					}
				}
				Hero hero2 = (mobileParty != null) ? mobileParty.LeaderHero : hero;
				if (hero2 != null)
				{
					float currentTime = Campaign.CurrentTime;
					if (hero2.Clan == settlement.OwnerClan && hero2.Clan.Leader == hero2)
					{
						settlement.LastVisitTimeOfOwner = currentTime;
					}
				}
				if (mobileParty == MobileParty.MainParty && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
				{
					foreach (MobileParty mobileParty2 in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
					{
						EnterSettlementAction.ApplyForParty(mobileParty2, settlement);
					}
				}
				if (hero != null && mobileParty == null && hero.PartyBelongedTo == null && hero.PartyBelongedToAsPrisoner == null && hero.Clan == Clan.PlayerClan && hero.GovernorOf == null)
				{
					CampaignEventDispatcher.Instance.OnHeroGetsBusy(hero, HeroGetsBusyReasons.BecomeEmissary);
				}
			}
			if (hero == Hero.MainHero || mobileParty == MobileParty.MainParty)
			{
				Debug.Print(string.Format("Player has entered {0}: {1}", settlement.StringId, settlement), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x0013F108 File Offset: 0x0013D308
		public static void ApplyForParty(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty != null && mobileParty.Army != null && mobileParty.Army.LeaderParty != null && mobileParty.Army.LeaderParty != mobileParty && mobileParty.Army.LeaderParty.CurrentSettlement == settlement && mobileParty.AttachedTo == null)
			{
				mobileParty.Army.AddPartyToMergedParties(mobileParty);
			}
			mobileParty.CurrentSettlement = settlement;
			settlement.SettlementComponent.OnPartyEntered(mobileParty);
			EnterSettlementAction.ApplyInternal(mobileParty.LeaderHero, mobileParty, settlement, EnterSettlementAction.EnterSettlementDetail.WarParty, null, false);
		}

		// Token: 0x060040BB RID: 16571 RVA: 0x0013F185 File Offset: 0x0013D385
		public static void ApplyForPartyEntersAlley(MobileParty party, Settlement settlement, Alley alley, bool isPlayerInvolved = false)
		{
			EnterSettlementAction.ApplyInternal(null, party, settlement, EnterSettlementAction.EnterSettlementDetail.PartyEntersAlley, alley, isPlayerInvolved);
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x0013F192 File Offset: 0x0013D392
		public static void ApplyForCharacterOnly(Hero hero, Settlement settlement)
		{
			hero.StayingInSettlement = settlement;
			EnterSettlementAction.ApplyInternal(hero, null, settlement, EnterSettlementAction.EnterSettlementDetail.Character, null, false);
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x0013F1A6 File Offset: 0x0013D3A6
		public static void ApplyForPrisoner(Hero hero, Settlement settlement)
		{
			hero.ChangeState(Hero.CharacterStates.Prisoner);
			EnterSettlementAction.ApplyInternal(hero, null, settlement, EnterSettlementAction.EnterSettlementDetail.Prisoner, null, false);
		}

		// Token: 0x02000770 RID: 1904
		private enum EnterSettlementDetail
		{
			// Token: 0x04001F1C RID: 7964
			WarParty,
			// Token: 0x04001F1D RID: 7965
			PartyEntersAlley,
			// Token: 0x04001F1E RID: 7966
			Character,
			// Token: 0x04001F1F RID: 7967
			Prisoner
		}
	}
}
