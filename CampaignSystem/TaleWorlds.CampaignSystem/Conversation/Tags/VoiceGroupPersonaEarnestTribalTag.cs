using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000240 RID: 576
	public class VoiceGroupPersonaEarnestTribalTag : ConversationTag
	{
		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06001F1D RID: 7965 RVA: 0x00089601 File Offset: 0x00087801
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestTribalTag";
			}
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x00089608 File Offset: 0x00087808
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x040009C7 RID: 2503
		public const string Id = "VoiceGroupPersonaEarnestTribalTag";
	}
}
