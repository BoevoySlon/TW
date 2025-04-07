using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000439 RID: 1081
	public static class DestroyClanAction
	{
		// Token: 0x06004097 RID: 16535 RVA: 0x0013E7B0 File Offset: 0x0013C9B0
		private static void ApplyInternal(Clan destroyedClan, DestroyClanAction.DestroyClanActionDetails details)
		{
			destroyedClan.DeactivateClan();
			foreach (WarPartyComponent warPartyComponent in destroyedClan.WarPartyComponents.ToList<WarPartyComponent>())
			{
				PartyBase destroyerParty = null;
				if (warPartyComponent.MobileParty.MapEvent != null)
				{
					destroyerParty = warPartyComponent.MobileParty.MapEventSide.OtherSide.LeaderParty;
					if (warPartyComponent.MobileParty.MapEvent != MobileParty.MainParty.MapEvent)
					{
						warPartyComponent.MobileParty.MapEventSide = null;
					}
				}
				DestroyPartyAction.Apply(destroyerParty, warPartyComponent.MobileParty);
			}
			List<Hero> list = (from x in destroyedClan.Heroes
			where x.IsAlive
			select x).ToList<Hero>();
			for (int i = 0; i < list.Count; i++)
			{
				Hero hero = list[i];
				if (details != DestroyClanAction.DestroyClanActionDetails.ClanLeaderDeath || hero != destroyedClan.Leader)
				{
					KillCharacterAction.ApplyByRemove(hero, false, true);
				}
			}
			if (details != DestroyClanAction.DestroyClanActionDetails.ClanLeaderDeath && destroyedClan.Leader != null && destroyedClan.Leader.IsAlive && destroyedClan.Leader.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
			{
				KillCharacterAction.ApplyByRemove(destroyedClan.Leader, false, true);
			}
			if (!destroyedClan.Settlements.IsEmpty<Settlement>())
			{
				Clan clan = FactionHelper.ChooseHeirClanForFiefs(destroyedClan);
				foreach (Settlement settlement in destroyedClan.Settlements.ToList<Settlement>())
				{
					if (settlement.IsTown || settlement.IsCastle)
					{
						Hero randomElementWithPredicate = clan.Lords.GetRandomElementWithPredicate((Hero x) => !x.IsChild && x.IsAlive);
						ChangeOwnerOfSettlementAction.ApplyByDestroyClan(settlement, randomElementWithPredicate);
					}
				}
			}
			FactionManager.Instance.RemoveFactionsFromCampaignWars(destroyedClan);
			if (destroyedClan.Kingdom != null)
			{
				ChangeKingdomAction.ApplyByLeaveKingdomByClanDestruction(destroyedClan, true);
			}
			if (destroyedClan.IsRebelClan)
			{
				Campaign.Current.CampaignObjectManager.RemoveClan(destroyedClan);
			}
			CampaignEventDispatcher.Instance.OnClanDestroyed(destroyedClan);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x0013E9D4 File Offset: 0x0013CBD4
		public static void Apply(Clan destroyedClan)
		{
			DestroyClanAction.ApplyInternal(destroyedClan, DestroyClanAction.DestroyClanActionDetails.Default);
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x0013E9DD File Offset: 0x0013CBDD
		public static void ApplyByFailedRebellion(Clan failedRebellionClan)
		{
			DestroyClanAction.ApplyInternal(failedRebellionClan, DestroyClanAction.DestroyClanActionDetails.RebellionFailure);
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0013E9E6 File Offset: 0x0013CBE6
		public static void ApplyByClanLeaderDeath(Clan destroyedClan)
		{
			DestroyClanAction.ApplyInternal(destroyedClan, DestroyClanAction.DestroyClanActionDetails.ClanLeaderDeath);
		}

		// Token: 0x0200076E RID: 1902
		private enum DestroyClanActionDetails
		{
			// Token: 0x04001F15 RID: 7957
			Default,
			// Token: 0x04001F16 RID: 7958
			RebellionFailure,
			// Token: 0x04001F17 RID: 7959
			ClanLeaderDeath
		}
	}
}
