using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000207 RID: 519
	public class PlayerIsNobleTag : ConversationTag
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x000889E1 File Offset: 0x00086BE1
		public override string StringId
		{
			get
			{
				return "PlayerIsNobleTag";
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000889E8 File Offset: 0x00086BE8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Settlement.All.Any((Settlement x) => x.OwnerClan == Hero.MainHero.Clan);
		}

		// Token: 0x0400098D RID: 2445
		public const string Id = "PlayerIsNobleTag";
	}
}
