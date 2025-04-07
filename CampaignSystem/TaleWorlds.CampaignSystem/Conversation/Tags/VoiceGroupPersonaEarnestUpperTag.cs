using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000241 RID: 577
	public class VoiceGroupPersonaEarnestUpperTag : ConversationTag
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06001F20 RID: 7968 RVA: 0x00089627 File Offset: 0x00087827
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestUpperTag";
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0008962E File Offset: 0x0008782E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x040009C8 RID: 2504
		public const string Id = "VoiceGroupPersonaEarnestUpperTag";
	}
}
