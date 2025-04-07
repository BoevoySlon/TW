using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000205 RID: 517
	public class PlayerIsFamousTag : ConversationTag
	{
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x00088922 File Offset: 0x00086B22
		public override string StringId
		{
			get
			{
				return "PlayerIsFamousTag";
			}
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x00088929 File Offset: 0x00086B29
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Clan.PlayerClan.Renown >= 50f;
		}

		// Token: 0x0400098B RID: 2443
		public const string Id = "PlayerIsFamousTag";
	}
}
