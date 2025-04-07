using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000230 RID: 560
	public class SturgianTag : ConversationTag
	{
		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x00089341 File Offset: 0x00087541
		public override string StringId
		{
			get
			{
				return "SturgianTag";
			}
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x00089348 File Offset: 0x00087548
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "sturgia";
		}

		// Token: 0x040009B7 RID: 2487
		public const string Id = "SturgianTag";
	}
}
