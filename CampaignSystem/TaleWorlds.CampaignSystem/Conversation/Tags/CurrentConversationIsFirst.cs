using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000217 RID: 535
	public class CurrentConversationIsFirst : ConversationTag
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06001EA2 RID: 7842 RVA: 0x00088E3C File Offset: 0x0008703C
		public override string StringId
		{
			get
			{
				return "CurrentConversationIsFirst";
			}
		}

		// Token: 0x06001EA3 RID: 7843 RVA: 0x00088E43 File Offset: 0x00087043
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x0400099D RID: 2461
		public const string Id = "CurrentConversationIsFirst";
	}
}
