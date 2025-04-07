using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000222 RID: 546
	public class PreacherNotableTypeTag : ConversationTag
	{
		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x000890DA File Offset: 0x000872DA
		public override string StringId
		{
			get
			{
				return "PreacherNotableTypeTag";
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000890E1 File Offset: 0x000872E1
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Preacher;
		}

		// Token: 0x040009A9 RID: 2473
		public const string Id = "PreacherNotableTypeTag";
	}
}
