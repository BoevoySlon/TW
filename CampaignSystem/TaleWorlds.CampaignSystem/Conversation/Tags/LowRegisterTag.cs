using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F8 RID: 504
	public class LowRegisterTag : ConversationTag
	{
		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06001E45 RID: 7749 RVA: 0x0008863B File Offset: 0x0008683B
		public override string StringId
		{
			get
			{
				return "LowRegisterTag";
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x00088642 File Offset: 0x00086842
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && !ConversationTagHelper.UsesHighRegister(character) && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x0400097E RID: 2430
		public const string Id = "LowRegisterTag";
	}
}
