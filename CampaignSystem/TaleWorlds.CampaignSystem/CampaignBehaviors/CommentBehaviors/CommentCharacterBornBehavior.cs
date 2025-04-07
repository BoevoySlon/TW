using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003E7 RID: 999
	public class CommentCharacterBornBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E76 RID: 15990 RVA: 0x001335FF File Offset: 0x001317FF
		public override void RegisterEvents()
		{
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.HeroCreated));
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00133618 File Offset: 0x00131818
		private void HeroCreated(Hero hero, bool isBornNaturally)
		{
			if (isBornNaturally)
			{
				LogEntry.AddLogEntry(new CharacterBornLogEntry(hero));
			}
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x00133628 File Offset: 0x00131828
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
