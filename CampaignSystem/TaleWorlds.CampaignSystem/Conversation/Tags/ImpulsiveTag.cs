using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000238 RID: 568
	public class ImpulsiveTag : ConversationTag
	{
		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x000894A9 File Offset: 0x000876A9
		public override string StringId
		{
			get
			{
				return "ImpulsiveTag";
			}
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x000894B0 File Offset: 0x000876B0
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0;
		}

		// Token: 0x040009BF RID: 2495
		public const string Id = "ImpulsiveTag";
	}
}
