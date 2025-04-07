using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000233 RID: 563
	public class GenerosityTag : ConversationTag
	{
		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06001EF6 RID: 7926 RVA: 0x000893C3 File Offset: 0x000875C3
		public override string StringId
		{
			get
			{
				return "GenerosityTag";
			}
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x000893CA File Offset: 0x000875CA
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) > 0;
		}

		// Token: 0x040009BA RID: 2490
		public const string Id = "GenerosityTag";
	}
}
