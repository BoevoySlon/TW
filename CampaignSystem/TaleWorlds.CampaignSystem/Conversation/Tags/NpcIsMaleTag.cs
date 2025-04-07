using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200020A RID: 522
	public class NpcIsMaleTag : ConversationTag
	{
		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06001E7B RID: 7803 RVA: 0x00088A59 File Offset: 0x00086C59
		public override string StringId
		{
			get
			{
				return "NpcIsMaleTag";
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x00088A60 File Offset: 0x00086C60
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !character.IsFemale;
		}

		// Token: 0x04000990 RID: 2448
		public const string Id = "NpcIsMaleTag";
	}
}
