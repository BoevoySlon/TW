using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200021C RID: 540
	public class OldTag : ConversationTag
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x00088F67 File Offset: 0x00087167
		public override string StringId
		{
			get
			{
				return "OldTag";
			}
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x00088F6E File Offset: 0x0008716E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Age > 49f;
		}

		// Token: 0x040009A3 RID: 2467
		public const string Id = "OldTag";
	}
}
