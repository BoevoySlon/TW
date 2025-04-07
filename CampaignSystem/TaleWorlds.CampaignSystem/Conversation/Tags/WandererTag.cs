using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000228 RID: 552
	public class WandererTag : ConversationTag
	{
		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06001ED5 RID: 7893 RVA: 0x000891B9 File Offset: 0x000873B9
		public override string StringId
		{
			get
			{
				return "WandererTag";
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000891C0 File Offset: 0x000873C0
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsWanderer;
		}

		// Token: 0x040009AF RID: 2479
		public const string Id = "WandererTag";
	}
}
