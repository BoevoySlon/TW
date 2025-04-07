using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003F7 RID: 1015
	public class CommentPregnancyBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003EB7 RID: 16055 RVA: 0x00133CE3 File Offset: 0x00131EE3
		public override void RegisterEvents()
		{
			CampaignEvents.OnChildConceivedEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnChildConceived));
		}

		// Token: 0x06003EB8 RID: 16056 RVA: 0x00133CFC File Offset: 0x00131EFC
		private void OnChildConceived(Hero mother)
		{
			LogEntry.AddLogEntry(new PregnancyLogEntry(mother));
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x00133D09 File Offset: 0x00131F09
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
