using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F1 RID: 497
	public class NpcIsLiegeTag : ConversationTag
	{
		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x0008844E File Offset: 0x0008664E
		public override string StringId
		{
			get
			{
				return "NpcIsLiegeTag";
			}
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x00088455 File Offset: 0x00086655
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsKingdomLeader;
		}

		// Token: 0x04000977 RID: 2423
		public const string Id = "NpcIsLiegeTag";
	}
}
