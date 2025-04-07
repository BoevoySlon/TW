using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200022E RID: 558
	public class KhuzaitTag : ConversationTag
	{
		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000892F5 File Offset: 0x000874F5
		public override string StringId
		{
			get
			{
				return "KhuzaitTag";
			}
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000892FC File Offset: 0x000874FC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "khuzait";
		}

		// Token: 0x040009B5 RID: 2485
		public const string Id = "KhuzaitTag";
	}
}
