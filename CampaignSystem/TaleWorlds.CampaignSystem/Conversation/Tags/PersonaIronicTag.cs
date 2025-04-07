using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200023E RID: 574
	public class PersonaIronicTag : ConversationTag
	{
		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06001F17 RID: 7959 RVA: 0x000895B1 File Offset: 0x000877B1
		public override string StringId
		{
			get
			{
				return "PersonaIronicTag";
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000895B8 File Offset: 0x000877B8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaIronic;
		}

		// Token: 0x040009C5 RID: 2501
		public const string Id = "PersonaIronicTag";
	}
}
