using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200021E RID: 542
	public class OnTheRoadTag : ConversationTag
	{
		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x00088FCD File Offset: 0x000871CD
		public override string StringId
		{
			get
			{
				return "OnTheRoadTag";
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x00088FD4 File Offset: 0x000871D4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Settlement.CurrentSettlement == null;
		}

		// Token: 0x040009A5 RID: 2469
		public const string Id = "OnTheRoadTag";
	}
}
