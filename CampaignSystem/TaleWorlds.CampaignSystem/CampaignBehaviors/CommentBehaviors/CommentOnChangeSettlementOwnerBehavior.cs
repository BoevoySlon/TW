using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003EA RID: 1002
	public class CommentOnChangeSettlementOwnerBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003E82 RID: 16002 RVA: 0x00133786 File Offset: 0x00131986
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x0013379F File Offset: 0x0013199F
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x001337A4 File Offset: 0x001319A4
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero previousOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			ChangeSettlementOwnerLogEntry changeSettlementOwnerLogEntry = new ChangeSettlementOwnerLogEntry(settlement, newOwner, previousOwner, false);
			LogEntry.AddLogEntry(changeSettlementOwnerLogEntry);
			if (newOwner != null && newOwner.IsHumanPlayerCharacter)
			{
				Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new SettlementOwnerChangedMapNotification(settlement, newOwner, previousOwner, changeSettlementOwnerLogEntry.GetEncyclopediaText()));
			}
		}
	}
}
