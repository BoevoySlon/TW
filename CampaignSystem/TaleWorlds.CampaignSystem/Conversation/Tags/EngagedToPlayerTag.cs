using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200021B RID: 539
	public class EngagedToPlayerTag : ConversationTag
	{
		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06001EAE RID: 7854 RVA: 0x00088F39 File Offset: 0x00087139
		public override string StringId
		{
			get
			{
				return "EngagedToPlayerTag";
			}
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x00088F40 File Offset: 0x00087140
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Romance.GetRomanticLevel(character.HeroObject, Hero.MainHero) == Romance.RomanceLevelEnum.CoupleAgreedOnMarriage;
		}

		// Token: 0x040009A2 RID: 2466
		public const string Id = "EngagedToPlayerTag";
	}
}
