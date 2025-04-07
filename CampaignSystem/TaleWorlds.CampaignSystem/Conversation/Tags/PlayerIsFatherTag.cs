using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000200 RID: 512
	public class PlayerIsFatherTag : ConversationTag
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x000887D9 File Offset: 0x000869D9
		public override string StringId
		{
			get
			{
				return "PlayerIsFatherTag";
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000887E0 File Offset: 0x000869E0
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.Father == Hero.MainHero;
		}

		// Token: 0x04000986 RID: 2438
		public const string Id = "PlayerIsFatherTag";
	}
}
