using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000223 RID: 547
	public class HeadmanNotableTypeTag : ConversationTag
	{
		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06001EC6 RID: 7878 RVA: 0x000890FF File Offset: 0x000872FF
		public override string StringId
		{
			get
			{
				return "HeadmanNotableTypeTag";
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00089106 File Offset: 0x00087306
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Headman;
		}

		// Token: 0x040009AA RID: 2474
		public const string Id = "HeadmanNotableTypeTag";
	}
}
