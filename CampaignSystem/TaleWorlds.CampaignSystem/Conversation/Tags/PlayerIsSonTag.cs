using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001FF RID: 511
	public class PlayerIsSonTag : ConversationTag
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x0008878C File Offset: 0x0008698C
		public override string StringId
		{
			get
			{
				return "PlayerIsSonTag";
			}
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x00088793 File Offset: 0x00086993
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && !Hero.MainHero.IsFemale && (Hero.MainHero.Father == character.HeroObject || Hero.MainHero.Mother == character.HeroObject);
		}

		// Token: 0x04000985 RID: 2437
		public const string Id = "PlayerIsSonTag";
	}
}
