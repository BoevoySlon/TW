using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000237 RID: 567
	public class CalculatingTag : ConversationTag
	{
		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0008947B File Offset: 0x0008767B
		public override string StringId
		{
			get
			{
				return "CalculatingTag";
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x00089482 File Offset: 0x00087682
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) > 0;
		}

		// Token: 0x040009BE RID: 2494
		public const string Id = "CalculatingTag";
	}
}
