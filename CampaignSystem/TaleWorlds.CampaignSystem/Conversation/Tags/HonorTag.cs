using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000235 RID: 565
	public class HonorTag : ConversationTag
	{
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06001EFC RID: 7932 RVA: 0x0008941F File Offset: 0x0008761F
		public override string StringId
		{
			get
			{
				return "HonorTag";
			}
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x00089426 File Offset: 0x00087626
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0;
		}

		// Token: 0x040009BC RID: 2492
		public const string Id = "HonorTag";
	}
}
