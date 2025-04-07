using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000212 RID: 530
	public class AmoralTag : ConversationTag
	{
		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x00088C8F File Offset: 0x00086E8F
		public override string StringId
		{
			get
			{
				return "AmoralTag";
			}
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x00088C96 File Offset: 0x00086E96
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetTraitLevel(DefaultTraits.Honor) + character.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x04000998 RID: 2456
		public const string Id = "AmoralTag";
	}
}
