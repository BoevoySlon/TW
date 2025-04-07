using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000202 RID: 514
	public class PlayerIsBrotherTag : ConversationTag
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x00088833 File Offset: 0x00086A33
		public override string StringId
		{
			get
			{
				return "PlayerIsBrotherTag";
			}
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x0008883A File Offset: 0x00086A3A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Hero.MainHero.IsFemale && character.IsHero && character.HeroObject.Siblings.Contains(Hero.MainHero);
		}

		// Token: 0x04000988 RID: 2440
		public const string Id = "PlayerIsBrotherTag";
	}
}
