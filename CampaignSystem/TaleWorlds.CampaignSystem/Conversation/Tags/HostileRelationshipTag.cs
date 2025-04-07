using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200020F RID: 527
	public class HostileRelationshipTag : ConversationTag
	{
		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x00088B98 File Offset: 0x00086D98
		public override string StringId
		{
			get
			{
				return "HostileRelationshipTag";
			}
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x00088BA0 File Offset: 0x00086DA0
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
			return (num + num2 + num3 < -1 && unmodifiedClanLeaderRelationshipWithPlayer <= -5f) || unmodifiedClanLeaderRelationshipWithPlayer <= -20f;
		}

		// Token: 0x04000995 RID: 2453
		public const string Id = "HostileRelationshipTag";
	}
}
