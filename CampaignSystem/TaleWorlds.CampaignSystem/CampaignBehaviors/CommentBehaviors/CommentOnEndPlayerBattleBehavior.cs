using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x020003F2 RID: 1010
	public class CommentOnEndPlayerBattleBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003EA3 RID: 16035 RVA: 0x00133B7A File Offset: 0x00131D7A
		public override void RegisterEvents()
		{
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnded));
		}

		// Token: 0x06003EA4 RID: 16036 RVA: 0x00133B93 File Offset: 0x00131D93
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003EA5 RID: 16037 RVA: 0x00133B95 File Offset: 0x00131D95
		private void OnPlayerBattleEnded(MapEvent mapEvent)
		{
			if (!mapEvent.IsHideoutBattle || mapEvent.BattleState != BattleState.None)
			{
				LogEntry.AddLogEntry(new PlayerBattleEndedLogEntry(mapEvent));
			}
		}
	}
}
