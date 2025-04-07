using System;
using Helpers;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000426 RID: 1062
	public static class ApplyHeirSelectionAction
	{
		// Token: 0x06004046 RID: 16454 RVA: 0x0013C810 File Offset: 0x0013AA10
		private static void ApplyInternal(Hero heir, bool isRetirement = false)
		{
			if (heir.PartyBelongedTo != null && heir.PartyBelongedTo.IsCaravan)
			{
				Settlement settlement = SettlementHelper.FindNearestSettlement((Settlement s) => (s.IsTown || s.IsCastle) && !FactionManager.IsAtWarAgainstFaction(s.MapFaction, heir.MapFaction), null);
				if (settlement == null)
				{
					settlement = SettlementHelper.FindNearestSettlement((Settlement s) => s.IsVillage || (!s.IsHideout && !s.IsFortification), null);
				}
				DestroyPartyAction.Apply(null, heir.PartyBelongedTo);
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(heir, settlement);
			}
			ApplyHeirSelectionAction.TransferCaravanOwnerships(heir);
			ChangeClanLeaderAction.ApplyWithSelectedNewLeader(Clan.PlayerClan, heir);
			if (isRetirement)
			{
				DisableHeroAction.Apply(Hero.MainHero);
				if (heir.PartyBelongedTo != MobileParty.MainParty)
				{
					MobileParty.MainParty.MemberRoster.RemoveTroop(CharacterObject.PlayerCharacter, 1, default(UniqueTroopDescriptor), 0);
				}
				LogEntry.AddLogEntry(new PlayerRetiredLogEntry(Hero.MainHero));
				TextObject textObject = new TextObject("{=0MTzaxau}{?CHARACTER.GENDER}She{?}He{\\?} retired from adventuring, and was last seen with a group of mountain hermits living a life of quiet contemplation.", null);
				textObject.SetCharacterProperties("CHARACTER", Hero.MainHero.CharacterObject, false);
				Hero.MainHero.EncyclopediaText = textObject;
			}
			else
			{
				KillCharacterAction.ApplyByDeathMarkForced(Hero.MainHero, true);
			}
			if (heir.CurrentSettlement != null && heir.PartyBelongedTo != null)
			{
				LeaveSettlementAction.ApplyForCharacterOnly(heir);
				LeaveSettlementAction.ApplyForParty(heir.PartyBelongedTo);
			}
			for (int i = Hero.MainHero.OwnedWorkshops.Count - 1; i >= 0; i--)
			{
				ChangeOwnerOfWorkshopAction.ApplyByDeath(Hero.MainHero.OwnedWorkshops[i], heir);
			}
			if (heir.PartyBelongedTo != MobileParty.MainParty)
			{
				for (int j = MobileParty.MainParty.MemberRoster.Count - 1; j >= 0; j--)
				{
					TroopRosterElement elementCopyAtIndex = MobileParty.MainParty.MemberRoster.GetElementCopyAtIndex(j);
					if (elementCopyAtIndex.Character.IsHero && elementCopyAtIndex.Character.HeroObject != Hero.MainHero)
					{
						MakeHeroFugitiveAction.Apply(elementCopyAtIndex.Character.HeroObject);
					}
				}
			}
			if (MobileParty.MainParty.Army != null)
			{
				DisbandArmyAction.ApplyByUnknownReason(MobileParty.MainParty.Army);
			}
			ChangePlayerCharacterAction.Apply(heir);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x0013CA5D File Offset: 0x0013AC5D
		public static void ApplyByDeath(Hero heir)
		{
			ApplyHeirSelectionAction.ApplyInternal(heir, false);
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0013CA66 File Offset: 0x0013AC66
		public static void ApplyByRetirement(Hero heir)
		{
			ApplyHeirSelectionAction.ApplyInternal(heir, true);
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0013CA70 File Offset: 0x0013AC70
		private static void TransferCaravanOwnerships(Hero newLeader)
		{
			foreach (Hero hero in Clan.PlayerClan.Heroes)
			{
				if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.IsCaravan)
				{
					CaravanPartyComponent.TransferCaravanOwnership(hero.PartyBelongedTo, newLeader, hero.PartyBelongedTo.HomeSettlement);
				}
			}
		}
	}
}
