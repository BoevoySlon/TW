using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003E9 RID: 1001
	public class CommentOnChangeRomanticStateBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E7E RID: 15998 RVA: 0x00133728 File Offset: 0x00131928
		public override void RegisterEvents()
		{
			CampaignEvents.RomanticStateChanged.AddNonSerializedListener(this, new Action<Hero, Hero, Romance.RomanceLevelEnum>(this.OnRomanticStateChanged));
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x00133741 File Offset: 0x00131941
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00133743 File Offset: 0x00131943
		private void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum level)
		{
			if (hero1 == Hero.MainHero || hero2 == Hero.MainHero || hero1.Clan.Leader == hero1 || hero2.Clan.Leader == hero2)
			{
				LogEntry.AddLogEntry(new ChangeRomanticStateLogEntry(hero1, hero2, level));
			}
		}
	}
}
