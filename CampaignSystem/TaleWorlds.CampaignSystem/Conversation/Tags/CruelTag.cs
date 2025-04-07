using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000232 RID: 562
	public class CruelTag : ConversationTag
	{
		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x00089395 File Offset: 0x00087595
		public override string StringId
		{
			get
			{
				return "CruelTag";
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0008939C File Offset: 0x0008759C
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x040009B9 RID: 2489
		public const string Id = "CruelTag";
	}
}
