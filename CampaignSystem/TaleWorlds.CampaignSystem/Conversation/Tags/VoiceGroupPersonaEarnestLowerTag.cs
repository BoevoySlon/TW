using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000242 RID: 578
	public class VoiceGroupPersonaEarnestLowerTag : ConversationTag
	{
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x0008964D File Offset: 0x0008784D
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestLowerTag";
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x00089654 File Offset: 0x00087854
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x040009C9 RID: 2505
		public const string Id = "VoiceGroupPersonaEarnestLowerTag";
	}
}
