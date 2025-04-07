using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200020E RID: 526
	public class FriendlyRelationshipTag : ConversationTag
	{
		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x00088B08 File Offset: 0x00086D08
		public override string StringId
		{
			get
			{
				return "FriendlyRelationshipTag";
			}
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x00088B10 File Offset: 0x00086D10
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (!character.IsHero)
			{
				return false;
			}
			float unmodifiedClanLeaderRelationshipWithPlayer = character.HeroObject.GetUnmodifiedClanLeaderRelationshipWithPlayer();
			int num = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Mercy);
			int num2 = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Honor);
			int num3 = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Valor);
			return (num + num2 + num3 > 0 && unmodifiedClanLeaderRelationshipWithPlayer >= 5f) || unmodifiedClanLeaderRelationshipWithPlayer >= 20f;
		}

		// Token: 0x04000994 RID: 2452
		public const string Id = "FriendlyRelationshipTag";
	}
}
