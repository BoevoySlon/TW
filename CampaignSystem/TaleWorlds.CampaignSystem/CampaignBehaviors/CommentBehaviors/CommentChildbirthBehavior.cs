using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003E8 RID: 1000
	public class CommentChildbirthBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E7A RID: 15994 RVA: 0x00133632 File Offset: 0x00131832
		public override void RegisterEvents()
		{
			CampaignEvents.OnGivenBirthEvent.AddNonSerializedListener(this, new Action<Hero, List<Hero>, int>(this.OnGivenBirthEvent));
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x0013364B File Offset: 0x0013184B
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x00133650 File Offset: 0x00131850
		private void OnGivenBirthEvent(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
			if (mother.IsHumanPlayerCharacter || mother.Clan == Hero.MainHero.Clan)
			{
				for (int i = 0; i < stillbornCount; i++)
				{
					ChildbirthLogEntry childbirthLogEntry = new ChildbirthLogEntry(mother, null);
					LogEntry.AddLogEntry(childbirthLogEntry);
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ChildBornMapNotification(null, childbirthLogEntry.GetEncyclopediaText(), CampaignTime.Now));
				}
				foreach (Hero newbornHero in aliveChildren)
				{
					ChildbirthLogEntry childbirthLogEntry2 = new ChildbirthLogEntry(mother, newbornHero);
					LogEntry.AddLogEntry(childbirthLogEntry2);
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new ChildBornMapNotification(newbornHero, childbirthLogEntry2.GetEncyclopediaText(), CampaignTime.Now));
				}
			}
		}
	}
}
