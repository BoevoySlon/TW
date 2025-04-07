using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000246 RID: 582
	public class VoiceGroupPersonaIronicTribalTag : ConversationTag
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x000896E5 File Offset: 0x000878E5
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicTribalTag";
			}
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000896EC File Offset: 0x000878EC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x040009CD RID: 2509
		public const string Id = "VoiceGroupPersonaIronicTribalTag";
	}
}
