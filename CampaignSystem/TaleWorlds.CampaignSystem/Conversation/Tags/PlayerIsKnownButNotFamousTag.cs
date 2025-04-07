using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000206 RID: 518
	public class PlayerIsKnownButNotFamousTag : ConversationTag
	{
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x00088947 File Offset: 0x00086B47
		public override string StringId
		{
			get
			{
				return "PlayerIsKnownButNotFamousTag";
			}
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x00088950 File Offset: 0x00086B50
		public override bool IsApplicableTo(CharacterObject character)
		{
			int baseRelation = Campaign.Current.Models.DiplomacyModel.GetBaseRelation(Hero.MainHero, Hero.OneToOneConversationHero);
			if (Hero.OneToOneConversationHero.Clan != null && baseRelation == 0)
			{
				baseRelation = Campaign.Current.Models.DiplomacyModel.GetBaseRelation(Hero.MainHero, Hero.OneToOneConversationHero.Clan.Leader);
			}
			return baseRelation != 0 && Clan.PlayerClan.Renown < 50f && Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x0400098C RID: 2444
		public const string Id = "PlayerIsKnownButNotFamousTag";
	}
}
