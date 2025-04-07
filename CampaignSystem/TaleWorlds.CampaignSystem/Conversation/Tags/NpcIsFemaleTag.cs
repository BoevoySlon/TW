using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000209 RID: 521
	public class NpcIsFemaleTag : ConversationTag
	{
		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x00088A42 File Offset: 0x00086C42
		public override string StringId
		{
			get
			{
				return "NpcIsFemaleTag";
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x00088A49 File Offset: 0x00086C49
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsFemale;
		}

		// Token: 0x0400098F RID: 2447
		public const string Id = "NpcIsFemaleTag";
	}
}
