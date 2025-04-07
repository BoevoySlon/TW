using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003F6 RID: 1014
	public class CommentOnPlayerMeetLordBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003EB3 RID: 16051 RVA: 0x00133C99 File Offset: 0x00131E99
		public override void RegisterEvents()
		{
			CampaignEvents.OnPlayerMetHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnPlayerMetCharacter));
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x00133CB2 File Offset: 0x00131EB2
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x00133CB4 File Offset: 0x00131EB4
		private void OnPlayerMetCharacter(Hero hero)
		{
			if (hero.Mother != Hero.MainHero && hero.Father != Hero.MainHero)
			{
				LogEntry.AddLogEntry(new PlayerMeetLordLogEntry(hero));
			}
		}
	}
}
