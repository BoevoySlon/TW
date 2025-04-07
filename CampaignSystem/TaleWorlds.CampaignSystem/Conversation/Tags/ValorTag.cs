using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000239 RID: 569
	public class ValorTag : ConversationTag
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06001F08 RID: 7944 RVA: 0x000894D7 File Offset: 0x000876D7
		public override string StringId
		{
			get
			{
				return "ValorTag";
			}
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x000894DE File Offset: 0x000876DE
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Valor) > 0;
		}

		// Token: 0x040009C0 RID: 2496
		public const string Id = "ValorTag";
	}
}
