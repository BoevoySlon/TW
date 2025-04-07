using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200042E RID: 1070
	public static class ChangeKingdomAction
	{
		// Token: 0x06004061 RID: 16481 RVA: 0x0013D744 File Offset: 0x0013B944
		private static void ApplyInternal(Clan clan, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, int awardMultiplier = 0, bool byRebellion = false, bool showNotification = true)
		{
			Kingdom kingdom = clan.Kingdom;
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			MapEvent mapEvent = MobileParty.MainParty.MapEvent;
			PlayerEncounter playerEncounter = PlayerEncounter.Current;
			clan.DebtToKingdom = 0;
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdomByDefection)
			{
				FactionHelper.AdjustFactionStancesForClanJoiningKingdom(clan, newKingdom);
			}
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdomByDefection)
			{
				if (clan.IsUnderMercenaryService)
				{
					clan.EndMercenaryService(false);
				}
				if (kingdom != null)
				{
					clan.ClanLeaveKingdom(!byRebellion);
				}
				clan.Kingdom = newKingdom;
				if (newKingdom != null && detail == ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom)
				{
					ChangeRulingClanAction.Apply(newKingdom, clan);
				}
			}
			else if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary)
			{
				if (clan.IsUnderMercenaryService)
				{
					clan.EndMercenaryService(true);
				}
				clan.MercenaryAwardMultiplier = awardMultiplier;
				clan.Kingdom = newKingdom;
				clan.StartMercenaryService();
				if (clan == Clan.PlayerClan)
				{
					Campaign.Current.KingdomManager.PlayerMercenaryServiceNextRenewDay = Campaign.CurrentTime + 720f;
				}
			}
			else if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByClanDestruction || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction)
			{
				clan.Kingdom = null;
				if (clan.IsUnderMercenaryService)
				{
					clan.EndMercenaryService(true);
				}
				if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion)
				{
					if (clan == Clan.PlayerClan)
					{
						foreach (Clan clan2 in kingdom.Clans)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(clan.Leader, clan2.Leader, -40, true);
						}
						DeclareWarAction.ApplyByRebellion(kingdom, clan);
					}
				}
				else
				{
					if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom)
					{
						if (clan == Clan.PlayerClan && !clan.IsEliminated)
						{
							foreach (Clan clan3 in kingdom.Clans)
							{
								ChangeRelationAction.ApplyRelationChangeBetweenHeroes(clan.Leader, clan3.Leader, -20, true);
							}
						}
						using (List<Settlement>.Enumerator enumerator2 = new List<Settlement>(clan.Settlements).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Settlement settlement = enumerator2.Current;
								ChangeOwnerOfSettlementAction.ApplyByLeaveFaction(kingdom.Leader, settlement);
								foreach (Hero hero in new List<Hero>(settlement.HeroesWithoutParty))
								{
									if (hero.CurrentSettlement != null && hero.Clan == clan)
									{
										if (hero.PartyBelongedTo != null)
										{
											LeaveSettlementAction.ApplyForParty(hero.PartyBelongedTo);
											EnterSettlementAction.ApplyForParty(hero.PartyBelongedTo, clan.Leader.HomeSettlement);
										}
										else
										{
											LeaveSettlementAction.ApplyForCharacterOnly(hero);
											EnterSettlementAction.ApplyForCharacterOnly(hero, clan.Leader.HomeSettlement);
										}
									}
								}
							}
							goto IL_30F;
						}
					}
					if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction)
					{
						foreach (StanceLink stanceLink in kingdom.Stances)
						{
							if (stanceLink.IsAtWar && !stanceLink.IsAtConstantWar)
							{
								IFaction faction = (stanceLink.Faction1 == kingdom) ? stanceLink.Faction2 : stanceLink.Faction1;
								if (faction != clan && !clan.GetStanceWith(faction).IsAtWar)
								{
									DeclareWarAction.ApplyByDefault(clan, faction);
								}
							}
						}
					}
				}
			}
			IL_30F:
			if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion)
			{
				foreach (StanceLink stanceLink2 in new List<StanceLink>(clan.Stances))
				{
					if (stanceLink2.IsAtWar && !stanceLink2.IsAtConstantWar)
					{
						IFaction faction2 = (stanceLink2.Faction1 == clan) ? stanceLink2.Faction2 : stanceLink2.Faction1;
						if (detail != ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion || clan != Clan.PlayerClan || faction2 != kingdom)
						{
							MakePeaceAction.Apply(clan, faction2, 0);
							FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(clan, faction2);
							FactionHelper.FinishAllRelatedHostileActionsOfFactionToFaction(faction2, clan);
						}
					}
				}
				ChangeKingdomAction.CheckIfPartyIconIsDirty(clan, kingdom);
			}
			foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
			{
				if (warPartyComponent.MobileParty.MapEvent == null)
				{
					warPartyComponent.MobileParty.Ai.SetMoveModeHold();
				}
			}
			CampaignEventDispatcher.Instance.OnClanChangedKingdom(clan, kingdom, newKingdom, detail, showNotification);
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0013DBC4 File Offset: 0x0013BDC4
		public static void ApplyByJoinToKingdom(Clan clan, Kingdom newKingdom, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom, 0, false, showNotification);
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0013DBD1 File Offset: 0x0013BDD1
		public static void ApplyByJoinToKingdomByDefection(Clan clan, Kingdom newKingdom, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdomByDefection, 0, false, showNotification);
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0013DBDE File Offset: 0x0013BDDE
		public static void ApplyByCreateKingdom(Clan clan, Kingdom newKingdom, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom, 0, false, showNotification);
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x0013DBEB File Offset: 0x0013BDEB
		public static void ApplyByLeaveByKingdomDestruction(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByKingdomDestruction, 0, false, showNotification);
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x0013DBF8 File Offset: 0x0013BDF8
		public static void ApplyByLeaveKingdom(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveKingdom, 0, false, showNotification);
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0013DC05 File Offset: 0x0013BE05
		public static void ApplyByLeaveWithRebellionAgainstKingdom(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveWithRebellion, 0, false, showNotification);
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0013DC12 File Offset: 0x0013BE12
		public static void ApplyByJoinFactionAsMercenary(Clan clan, Kingdom newKingdom, int awardMultiplier = 50, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary, awardMultiplier, false, showNotification);
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0013DC1F File Offset: 0x0013BE1F
		public static void ApplyByLeaveKingdomAsMercenary(Clan mercenaryClan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(mercenaryClan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveAsMercenary, 0, false, showNotification);
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x0013DC2C File Offset: 0x0013BE2C
		public static void ApplyByLeaveKingdomByClanDestruction(Clan clan, bool showNotification = true)
		{
			ChangeKingdomAction.ApplyInternal(clan, null, ChangeKingdomAction.ChangeKingdomActionDetail.LeaveByClanDestruction, 0, false, showNotification);
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x0013DC3C File Offset: 0x0013BE3C
		private static void CheckIfPartyIconIsDirty(Clan clan, Kingdom oldKingdom)
		{
			IFaction faction;
			if (clan.Kingdom == null)
			{
				faction = clan;
			}
			else
			{
				IFaction kingdom = clan.Kingdom;
				faction = kingdom;
			}
			IFaction faction2 = faction;
			IFaction faction3 = oldKingdom ?? clan;
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				if (mobileParty.IsVisible && ((mobileParty.Party.Owner != null && mobileParty.Party.Owner.Clan == clan) || (clan == Clan.PlayerClan && ((!FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction2) && FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction3)) || (FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction2) && !FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, faction3))))))
				{
					mobileParty.Party.SetVisualAsDirty();
				}
			}
			foreach (Settlement settlement in clan.Settlements)
			{
				settlement.Party.SetVisualAsDirty();
			}
		}

		// Token: 0x040012BF RID: 4799
		public const float PotentialSettlementsPerNobleEffect = 0.2f;

		// Token: 0x040012C0 RID: 4800
		public const float NewGainedFiefsValueForKingdomConstant = 0.1f;

		// Token: 0x040012C1 RID: 4801
		public const float LordsUnitStrengthValue = 20f;

		// Token: 0x040012C2 RID: 4802
		public const float MercenaryUnitStrengthValue = 5f;

		// Token: 0x040012C3 RID: 4803
		public const float MinimumNeededGoldForRecruitingMercenaries = 20000f;

		// Token: 0x02000769 RID: 1897
		public enum ChangeKingdomActionDetail
		{
			// Token: 0x04001EF4 RID: 7924
			JoinAsMercenary,
			// Token: 0x04001EF5 RID: 7925
			JoinKingdom,
			// Token: 0x04001EF6 RID: 7926
			JoinKingdomByDefection,
			// Token: 0x04001EF7 RID: 7927
			LeaveKingdom,
			// Token: 0x04001EF8 RID: 7928
			LeaveWithRebellion,
			// Token: 0x04001EF9 RID: 7929
			LeaveAsMercenary,
			// Token: 0x04001EFA RID: 7930
			LeaveByClanDestruction,
			// Token: 0x04001EFB RID: 7931
			CreateKingdom,
			// Token: 0x04001EFC RID: 7932
			LeaveByKingdomDestruction
		}
	}
}
