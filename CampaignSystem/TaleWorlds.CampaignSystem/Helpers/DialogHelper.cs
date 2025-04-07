using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000014 RID: 20
	public static class DialogHelper
	{
		// Token: 0x060000CA RID: 202 RVA: 0x0000A9C3 File Offset: 0x00008BC3
		public static void SetDialogString(string stringVariable, string gameTextId)
		{
			MBTextManager.SetTextVariable(stringVariable, Campaign.Current.ConversationManager.FindMatchingTextOrNull(gameTextId, CharacterObject.OneToOneConversationCharacter), false);
		}
	}
}
