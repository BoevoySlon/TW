using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003F0 RID: 1008
	public class CommentOnDefeatCharacterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E9B RID: 16027 RVA: 0x00133ABD File Offset: 0x00131CBD
		public override void RegisterEvents()
		{
			CampaignEvents.CharacterDefeated.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnCharacterDefeated));
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x00133AD6 File Offset: 0x00131CD6
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x00133AD8 File Offset: 0x00131CD8
		private void OnCharacterDefeated(Hero winner, Hero loser)
		{
			LogEntry.AddLogEntry(new DefeatCharacterLogEntry(winner, loser));
		}
	}
}
