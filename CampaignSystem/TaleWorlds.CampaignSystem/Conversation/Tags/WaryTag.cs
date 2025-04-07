using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020001F4 RID: 500
	public class WaryTag : ConversationTag
	{
		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x000884C3 File Offset: 0x000866C3
		public override string StringId
		{
			get
			{
				return "WaryTag";
			}
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000884CC File Offset: 0x000866CC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.MapFaction != Hero.MainHero.MapFaction && (Settlement.CurrentSettlement == null || Settlement.CurrentSettlement.SiegeEvent != null) && (Campaign.Current.ConversationManager.CurrentConversationIsFirst || FactionManager.IsAtWarAgainstFaction(character.HeroObject.MapFaction, Hero.MainHero.MapFaction));
		}

		// Token: 0x0400097A RID: 2426
		public const string Id = "WaryTag";
	}
}
