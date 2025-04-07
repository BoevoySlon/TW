using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003EE RID: 1006
	public class CommentOnClanLeaderChangedBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E93 RID: 16019 RVA: 0x00133A14 File Offset: 0x00131C14
		public override void RegisterEvents()
		{
			CampaignEvents.OnClanLeaderChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(CommentOnClanLeaderChangedBehavior.OnClanLeaderChanged));
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x00133A2D File Offset: 0x00131C2D
		private static void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			LogEntry.AddLogEntry(new ClanLeaderChangedLogEntry(oldLeader, newLeader));
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x00133A3B File Offset: 0x00131C3B
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
