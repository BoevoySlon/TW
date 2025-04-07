using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000218 RID: 536
	public class MetBeforeTag : ConversationTag
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06001EA5 RID: 7845 RVA: 0x00088E5C File Offset: 0x0008705C
		public override string StringId
		{
			get
			{
				return "MetBeforeTag";
			}
		}

		// Token: 0x06001EA6 RID: 7846 RVA: 0x00088E63 File Offset: 0x00087063
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x0400099E RID: 2462
		public const string Id = "MetBeforeTag";
	}
}
