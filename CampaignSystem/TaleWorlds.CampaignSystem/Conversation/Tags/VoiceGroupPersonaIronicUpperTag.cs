using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000247 RID: 583
	public class VoiceGroupPersonaIronicUpperTag : ConversationTag
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06001F32 RID: 7986 RVA: 0x0008970B File Offset: 0x0008790B
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicUpperTag";
			}
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x00089712 File Offset: 0x00087912
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x040009CE RID: 2510
		public const string Id = "VoiceGroupPersonaIronicUpperTag";
	}
}
