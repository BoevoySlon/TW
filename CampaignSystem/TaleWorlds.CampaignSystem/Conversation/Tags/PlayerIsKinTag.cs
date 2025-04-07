using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000204 RID: 516
	public class PlayerIsKinTag : ConversationTag
	{
		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x000888AB File Offset: 0x00086AAB
		public override string StringId
		{
			get
			{
				return "PlayerIsKinTag";
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000888B4 File Offset: 0x00086AB4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && (character.HeroObject.Siblings.Contains(Hero.MainHero) || character.HeroObject.Mother == Hero.MainHero || character.HeroObject.Father == Hero.MainHero || character.HeroObject.Spouse == Hero.MainHero);
		}

		// Token: 0x0400098A RID: 2442
		public const string Id = "PlayerIsKinTag";
	}
}
