using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200020D RID: 525
	public class NpcIsNobleTag : ConversationTag
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x00088AAC File Offset: 0x00086CAC
		public override string StringId
		{
			get
			{
				return "NpcIsNobleTag";
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x00088AB4 File Offset: 0x00086CB4
		public override bool IsApplicableTo(CharacterObject character)
		{
			Hero heroObject = character.HeroObject;
			if (heroObject == null)
			{
				return false;
			}
			Clan clan = heroObject.Clan;
			bool? flag = (clan != null) ? new bool?(clan.IsNoble) : null;
			bool flag2 = true;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}

		// Token: 0x04000993 RID: 2451
		public const string Id = "NpcIsNobleTag";
	}
}
