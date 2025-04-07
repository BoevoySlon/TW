using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200022B RID: 555
	public class EmpireTag : ConversationTag
	{
		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06001EDE RID: 7902 RVA: 0x00089283 File Offset: 0x00087483
		public override string StringId
		{
			get
			{
				return "EmpireTag";
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x0008928A File Offset: 0x0008748A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "empire";
		}

		// Token: 0x040009B2 RID: 2482
		public const string Id = "EmpireTag";
	}
}
