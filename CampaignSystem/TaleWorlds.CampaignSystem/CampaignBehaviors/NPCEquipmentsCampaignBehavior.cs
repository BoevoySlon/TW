using System;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003B3 RID: 947
	public class NPCEquipmentsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A12 RID: 14866 RVA: 0x001115A4 File Offset: 0x0010F7A4
		public override void RegisterEvents()
		{
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x001115BD File Offset: 0x0010F7BD
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x001115C0 File Offset: 0x0010F7C0
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			foreach (CharacterObject characterObject in CharacterObject.All)
			{
				bool isTemplate = characterObject.IsTemplate;
			}
		}
	}
}
