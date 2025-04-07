using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200023C RID: 572
	public class PersonaEarnestTag : ConversationTag
	{
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06001F11 RID: 7953 RVA: 0x00089561 File Offset: 0x00087761
		public override string StringId
		{
			get
			{
				return "PersonaEarnestTag";
			}
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x00089568 File Offset: 0x00087768
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaEarnest;
		}

		// Token: 0x040009C3 RID: 2499
		public const string Id = "PersonaEarnestTag";
	}
}
