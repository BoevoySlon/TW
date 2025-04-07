using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024A RID: 586
	public class VoiceGroupPersonaSoftspokenUpperTag : ConversationTag
	{
		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x0008977D File Offset: 0x0008797D
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaSoftspokenUpperTag";
			}
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x00089784 File Offset: 0x00087984
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaSoftspoken && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x040009D1 RID: 2513
		public const string Id = "VoiceGroupPersonaSoftspokenUpperTag";
	}
}
