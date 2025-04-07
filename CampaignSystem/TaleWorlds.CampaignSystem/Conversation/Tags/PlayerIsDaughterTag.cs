using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001FE RID: 510
	public class PlayerIsDaughterTag : ConversationTag
	{
		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x0008873F File Offset: 0x0008693F
		public override string StringId
		{
			get
			{
				return "PlayerIsDaughterTag";
			}
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x00088746 File Offset: 0x00086946
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Hero.MainHero.IsFemale && (Hero.MainHero.Father == character.HeroObject || Hero.MainHero.Mother == character.HeroObject);
		}

		// Token: 0x04000984 RID: 2436
		public const string Id = "PlayerIsDaughterTag";
	}
}
