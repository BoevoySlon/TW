using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200042D RID: 1069
	public static class ChangeGovernorAction
	{
		// Token: 0x0600405C RID: 16476 RVA: 0x0013D664 File Offset: 0x0013B864
		private static void ApplyInternal(Town fortification, Hero governor)
		{
			Hero governor2 = fortification.Governor;
			if (governor == null)
			{
				fortification.Governor = null;
			}
			else if (governor.CurrentSettlement == fortification.Settlement && !governor.IsPrisoner)
			{
				fortification.Governor = governor;
				TeleportHeroAction.ApplyImmediateTeleportToSettlement(governor, fortification.Settlement);
			}
			else
			{
				fortification.Governor = null;
				TeleportHeroAction.ApplyDelayedTeleportToSettlementAsGovernor(governor, fortification.Settlement);
			}
			if (governor2 != null)
			{
				governor2.GovernorOf = null;
			}
			CampaignEventDispatcher.Instance.OnGovernorChanged(fortification, governor2, governor);
			if (governor != null)
			{
				CampaignEventDispatcher.Instance.OnHeroGetsBusy(governor, HeroGetsBusyReasons.BecomeGovernor);
			}
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x0013D6EC File Offset: 0x0013B8EC
		private static void ApplyGiveUpInternal(Hero governor)
		{
			Town governorOf = governor.GovernorOf;
			governorOf.Governor = null;
			governor.GovernorOf = null;
			CampaignEventDispatcher.Instance.OnGovernorChanged(governorOf, governor, null);
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x0013D71B File Offset: 0x0013B91B
		public static void Apply(Town fortification, Hero governor)
		{
			ChangeGovernorAction.ApplyInternal(fortification, governor);
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x0013D724 File Offset: 0x0013B924
		public static void RemoveGovernorOf(Hero governor)
		{
			ChangeGovernorAction.ApplyGiveUpInternal(governor);
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x0013D72C File Offset: 0x0013B92C
		public static void RemoveGovernorOfIfExists(Town town)
		{
			if (town.Governor != null)
			{
				ChangeGovernorAction.ApplyGiveUpInternal(town.Governor);
			}
		}
	}
}
