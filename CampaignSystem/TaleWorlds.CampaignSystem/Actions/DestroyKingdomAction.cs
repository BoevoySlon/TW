using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200043A RID: 1082
	public static class DestroyKingdomAction
	{
		// Token: 0x0600409B RID: 16539 RVA: 0x0013E9F0 File Offset: 0x0013CBF0
		private static void ApplyInternal(Kingdom destroyedKingdom, bool isKingdomLeaderDeath = false)
		{
			destroyedKingdom.DeactivateKingdom();
			foreach (Clan clan in destroyedKingdom.Clans.ToList<Clan>())
			{
				if (!clan.IsEliminated)
				{
					if (isKingdomLeaderDeath)
					{
						DestroyClanAction.ApplyByClanLeaderDeath(clan);
					}
					else
					{
						DestroyClanAction.Apply(clan);
					}
					destroyedKingdom.RemoveClanInternal(clan);
				}
			}
			Campaign.Current.FactionManager.RemoveFactionsFromCampaignWars(destroyedKingdom);
			CampaignEventDispatcher.Instance.OnKingdomDestroyed(destroyedKingdom);
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0013EA84 File Offset: 0x0013CC84
		public static void Apply(Kingdom destroyedKingdom)
		{
			DestroyKingdomAction.ApplyInternal(destroyedKingdom, false);
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0013EA8D File Offset: 0x0013CC8D
		public static void ApplyByKingdomLeaderDeath(Kingdom destroyedKingdom)
		{
			DestroyKingdomAction.ApplyInternal(destroyedKingdom, true);
		}
	}
}
