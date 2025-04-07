using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F6 RID: 502
	public class OutlawSympathyTag : ConversationTag
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x000885DF File Offset: 0x000867DF
		public override string StringId
		{
			get
			{
				return "OutlawSympathyTag";
			}
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000885E6 File Offset: 0x000867E6
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsWanderer && character.HeroObject.GetTraitLevel(DefaultTraits.RogueSkills) > 0;
		}

		// Token: 0x0400097C RID: 2428
		public const string Id = "OutlawSympathyTag";
	}
}
