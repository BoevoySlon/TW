using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F0 RID: 496
	public class DefaultTag : ConversationTag
	{
		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x0008843C File Offset: 0x0008663C
		public override string StringId
		{
			get
			{
				return "DefaultTag";
			}
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00088443 File Offset: 0x00086643
		public override bool IsApplicableTo(CharacterObject character)
		{
			return true;
		}

		// Token: 0x04000976 RID: 2422
		public const string Id = "DefaultTag";
	}
}
