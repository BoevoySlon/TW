using System;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x020001E3 RID: 483
	public static class CampaignMapConversation
	{
		// Token: 0x06001D75 RID: 7541 RVA: 0x00085040 File Offset: 0x00083240
		public static void OpenConversation(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			Campaign.Current.ConversationManager.OpenMapConversation(playerCharacterData, conversationPartnerData);
		}
	}
}
