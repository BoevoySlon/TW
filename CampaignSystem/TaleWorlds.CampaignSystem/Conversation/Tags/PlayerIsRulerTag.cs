using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000208 RID: 520
	public class PlayerIsRulerTag : ConversationTag
	{
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06001E75 RID: 7797 RVA: 0x00088A1B File Offset: 0x00086C1B
		public override string StringId
		{
			get
			{
				return "PlayerIsRulerTag";
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00088A22 File Offset: 0x00086C22
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.Clan.Leader == Hero.MainHero;
		}

		// Token: 0x0400098E RID: 2446
		public const string Id = "PlayerIsRulerTag";
	}
}
