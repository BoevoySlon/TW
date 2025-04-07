using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000249 RID: 585
	public class VoiceGroupPersonaSoftspokenTribalTag : ConversationTag
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x00089757 File Offset: 0x00087957
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaSoftspokenTribalTag";
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0008975E File Offset: 0x0008795E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaSoftspoken && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x040009D0 RID: 2512
		public const string Id = "VoiceGroupPersonaSoftspokenTribalTag";
	}
}
