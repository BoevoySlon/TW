using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000224 RID: 548
	public class GangLeaderNotableTypeTag : ConversationTag
	{
		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06001EC9 RID: 7881 RVA: 0x00089124 File Offset: 0x00087324
		public override string StringId
		{
			get
			{
				return "GangLeaderNotableTypeTag";
			}
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0008912B File Offset: 0x0008732B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.GangLeader;
		}

		// Token: 0x040009AB RID: 2475
		public const string Id = "GangLeaderNotableTypeTag";
	}
}
