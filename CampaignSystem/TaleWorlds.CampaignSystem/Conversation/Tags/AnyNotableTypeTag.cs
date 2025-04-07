using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000227 RID: 551
	public class AnyNotableTypeTag : ConversationTag
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06001ED2 RID: 7890 RVA: 0x00089193 File Offset: 0x00087393
		public override string StringId
		{
			get
			{
				return "AnyNotableTypeTag";
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0008919A File Offset: 0x0008739A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsNotable;
		}

		// Token: 0x040009AE RID: 2478
		public const string Id = "AnyNotableTypeTag";
	}
}
