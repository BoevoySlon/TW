using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003EC RID: 1004
	public class CommentOnCharacterKilledBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E8A RID: 16010 RVA: 0x00133885 File Offset: 0x00131A85
		public override void RegisterEvents()
		{
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x0013389E File Offset: 0x00131A9E
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x001338A0 File Offset: 0x00131AA0
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim.Clan != null && !Clan.BanditFactions.Contains(victim.Clan))
			{
				CharacterKilledLogEntry characterKilledLogEntry = new CharacterKilledLogEntry(victim, killer, detail);
				LogEntry.AddLogEntry(characterKilledLogEntry);
				if (this.IsRelatedToPlayer(victim))
				{
					Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new DeathMapNotification(victim, killer, characterKilledLogEntry.GetEncyclopediaText(), detail, CampaignTime.Now));
				}
			}
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x00133904 File Offset: 0x00131B04
		private bool IsRelatedToPlayer(Hero victim)
		{
			bool flag = victim == Hero.MainHero.Mother || victim == Hero.MainHero.Father || victim == Hero.MainHero.Spouse || victim == Hero.MainHero;
			if (!flag)
			{
				foreach (Hero hero in Hero.MainHero.Children)
				{
					if (victim == hero)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				foreach (Hero hero2 in Hero.MainHero.Siblings)
				{
					if (victim == hero2)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}
	}
}
