using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000231 RID: 561
	public class MercyTag : ConversationTag
	{
		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x00089367 File Offset: 0x00087567
		public override string StringId
		{
			get
			{
				return "MercyTag";
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0008936E File Offset: 0x0008756E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) > 0;
		}

		// Token: 0x040009B8 RID: 2488
		public const string Id = "MercyTag";
	}
}
