using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003EB RID: 1003
	public class CommentOnChangeVillageStateBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E86 RID: 16006 RVA: 0x001337F3 File Offset: 0x001319F3
		public override void RegisterEvents()
		{
			CampaignEvents.VillageStateChanged.AddNonSerializedListener(this, new Action<Village, Village.VillageStates, Village.VillageStates, MobileParty>(this.OnVillageStateChanged));
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x0013380C File Offset: 0x00131A0C
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x00133810 File Offset: 0x00131A10
		private void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
			if (newState != Village.VillageStates.Normal && raiderParty != null && (raiderParty.LeaderHero == Hero.MainHero || village.Owner.Settlement.OwnerClan.Leader == Hero.MainHero || village.Settlement.MapFaction.IsKingdomFaction || raiderParty.MapFaction.IsKingdomFaction))
			{
				LogEntry.AddLogEntry(new VillageStateChangedLogEntry(village, oldState, newState, raiderParty));
			}
		}
	}
}
