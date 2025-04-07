using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200020B RID: 523
	public class PlayerIsFemaleTag : ConversationTag
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06001E7E RID: 7806 RVA: 0x00088A73 File Offset: 0x00086C73
		public override string StringId
		{
			get
			{
				return "PlayerIsFemaleTag";
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x00088A7A File Offset: 0x00086C7A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.IsFemale;
		}

		// Token: 0x04000991 RID: 2449
		public const string Id = "PlayerIsFemaleTag";
	}
}
