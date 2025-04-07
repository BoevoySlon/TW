using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000211 RID: 529
	public class UncharitableTag : ConversationTag
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06001E90 RID: 7824 RVA: 0x00088C64 File Offset: 0x00086E64
		public override string StringId
		{
			get
			{
				return "UncharitableTag";
			}
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x00088C6B File Offset: 0x00086E6B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetTraitLevel(DefaultTraits.Generosity) + character.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x04000997 RID: 2455
		public const string Id = "UncharitableTag";
	}
}
