using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000248 RID: 584
	public class VoiceGroupPersonaIronicLowerTag : ConversationTag
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x00089731 File Offset: 0x00087931
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicLowerTag";
			}
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x00089738 File Offset: 0x00087938
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x040009CF RID: 2511
		public const string Id = "VoiceGroupPersonaIronicLowerTag";
	}
}
