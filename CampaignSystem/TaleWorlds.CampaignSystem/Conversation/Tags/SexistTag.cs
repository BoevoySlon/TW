using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000214 RID: 532
	public class SexistTag : ConversationTag
	{
		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06001E99 RID: 7833 RVA: 0x00088CE5 File Offset: 0x00086EE5
		public override string StringId
		{
			get
			{
				return "SexistTag";
			}
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00088CEC File Offset: 0x00086EEC
		public override bool IsApplicableTo(CharacterObject character)
		{
			bool flag = character.HeroObject.Clan.Heroes.Any((Hero x) => x.IsFemale && x.IsCommander);
			int num = character.GetTraitLevel(DefaultTraits.Calculating) + character.GetTraitLevel(DefaultTraits.Mercy);
			int num2 = character.GetTraitLevel(DefaultTraits.Valor) + character.GetTraitLevel(DefaultTraits.Generosity);
			return num < 0 && num2 <= 0 && !flag;
		}

		// Token: 0x0400099A RID: 2458
		public const string Id = "SexistTag";
	}
}
