using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000234 RID: 564
	public class UngratefulTag : ConversationTag
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x000893F1 File Offset: 0x000875F1
		public override string StringId
		{
			get
			{
				return "UngratefulTag";
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000893F8 File Offset: 0x000875F8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0;
		}

		// Token: 0x040009BB RID: 2491
		public const string Id = "UngratefulTag";
	}
}
