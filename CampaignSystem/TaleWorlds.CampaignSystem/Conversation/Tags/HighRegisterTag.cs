using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F7 RID: 503
	public class HighRegisterTag : ConversationTag
	{
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001E42 RID: 7746 RVA: 0x0008861A File Offset: 0x0008681A
		public override string StringId
		{
			get
			{
				return "HighRegisterTag";
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x00088621 File Offset: 0x00086821
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x0400097D RID: 2429
		public const string Id = "HighRegisterTag";
	}
}
