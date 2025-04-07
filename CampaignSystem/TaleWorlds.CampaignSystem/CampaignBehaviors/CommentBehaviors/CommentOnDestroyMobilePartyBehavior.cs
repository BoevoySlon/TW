using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003F1 RID: 1009
	public class CommentOnDestroyMobilePartyBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E9F RID: 16031 RVA: 0x00133AEE File Offset: 0x00131CEE
		public override void RegisterEvents()
		{
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x00133B07 File Offset: 0x00131D07
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x00133B0C File Offset: 0x00131D0C
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			Hero hero = (destroyerParty != null) ? destroyerParty.LeaderHero : null;
			IFaction faction = (destroyerParty != null) ? destroyerParty.MapFaction : null;
			if (hero == Hero.MainHero || mobileParty.LeaderHero == Hero.MainHero || (faction != null && mobileParty.MapFaction != null && faction.IsKingdomFaction && mobileParty.MapFaction.IsKingdomFaction))
			{
				LogEntry.AddLogEntry(new DestroyMobilePartyLogEntry(mobileParty, destroyerParty));
			}
		}
	}
}
