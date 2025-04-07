using System;
using Helpers;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000215 RID: 533
	public class UnderCommandTag : ConversationTag
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06001E9C RID: 7836 RVA: 0x00088D72 File Offset: 0x00086F72
		public override string StringId
		{
			get
			{
				return "UnderCommandTag";
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x00088D79 File Offset: 0x00086F79
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.Spouse != Hero.MainHero && HeroHelper.UnderPlayerCommand(character.HeroObject);
		}

		// Token: 0x0400099B RID: 2459
		public const string Id = "UnderCommandTag";
	}
}
