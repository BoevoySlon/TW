using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001EF RID: 495
	public abstract class ConversationTag
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001E29 RID: 7721
		public abstract string StringId { get; }

		// Token: 0x06001E2A RID: 7722
		public abstract bool IsApplicableTo(CharacterObject character);

		// Token: 0x06001E2B RID: 7723 RVA: 0x0008842C File Offset: 0x0008662C
		public override string ToString()
		{
			return this.StringId;
		}
	}
}
