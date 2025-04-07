using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200023B RID: 571
	public class RogueSkillsTag : ConversationTag
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x00089533 File Offset: 0x00087733
		public override string StringId
		{
			get
			{
				return "RogueSkillsTag";
			}
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0008953A File Offset: 0x0008773A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.RogueSkills) > 0;
		}

		// Token: 0x040009C2 RID: 2498
		public const string Id = "RogueSkillsTag";
	}
}
