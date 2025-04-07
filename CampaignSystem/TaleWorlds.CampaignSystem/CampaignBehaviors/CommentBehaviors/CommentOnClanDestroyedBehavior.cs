using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003ED RID: 1005
	public class CommentOnClanDestroyedBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E8F RID: 16015 RVA: 0x001339E4 File Offset: 0x00131BE4
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, new Action<Clan>(this.OnClanDestroyed));
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x001339FD File Offset: 0x00131BFD
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x001339FF File Offset: 0x00131BFF
		private void OnClanDestroyed(Clan destroyedClan)
		{
			LogEntry.AddLogEntry(new ClanDestroyedLogEntry(destroyedClan));
		}
	}
}
