using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000213 RID: 531
	public class ChivalrousTag : ConversationTag
	{
		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06001E96 RID: 7830 RVA: 0x00088CBA File Offset: 0x00086EBA
		public override string StringId
		{
			get
			{
				return "ChivalrousTag";
			}
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x00088CC1 File Offset: 0x00086EC1
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetTraitLevel(DefaultTraits.Honor) + character.GetTraitLevel(DefaultTraits.Valor) > 0;
		}

		// Token: 0x04000999 RID: 2457
		public const string Id = "ChivalrousTag";
	}
}
