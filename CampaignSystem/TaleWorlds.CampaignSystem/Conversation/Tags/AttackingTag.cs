using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200021D RID: 541
	public class AttackingTag : ConversationTag
	{
		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06001EB4 RID: 7860 RVA: 0x00088F85 File Offset: 0x00087185
		public override string StringId
		{
			get
			{
				return "AttackingTag";
			}
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x00088F8C File Offset: 0x0008718C
		public override bool IsApplicableTo(CharacterObject character)
		{
			return HeroHelper.WillLordAttack() || (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.SiegeEvent != null && Settlement.CurrentSettlement.Parties.Contains(Hero.MainHero.PartyBelongedTo));
		}

		// Token: 0x040009A4 RID: 2468
		public const string Id = "AttackingTag";
	}
}
