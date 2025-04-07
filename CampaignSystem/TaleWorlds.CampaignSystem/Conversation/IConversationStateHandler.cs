using System;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x020001EB RID: 491
	public interface IConversationStateHandler
	{
		// Token: 0x06001DFB RID: 7675
		void OnConversationInstall();

		// Token: 0x06001DFC RID: 7676
		void OnConversationUninstall();

		// Token: 0x06001DFD RID: 7677
		void OnConversationActivate();

		// Token: 0x06001DFE RID: 7678
		void OnConversationDeactivate();

		// Token: 0x06001DFF RID: 7679
		void OnConversationContinue();

		// Token: 0x06001E00 RID: 7680
		void ExecuteConversationContinue();
	}
}
