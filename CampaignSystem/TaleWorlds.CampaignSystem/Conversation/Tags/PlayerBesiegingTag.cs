using System;
using System.Linq;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200021F RID: 543
	public class PlayerBesiegingTag : ConversationTag
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06001EBA RID: 7866 RVA: 0x00088FE6 File Offset: 0x000871E6
		public override string StringId
		{
			get
			{
				return "PlayerBesiegingTag";
			}
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x00088FF0 File Offset: 0x000871F0
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.SiegeEvent != null)
			{
				return Settlement.CurrentSettlement.SiegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege).Any((PartyBase party) => party.MobileParty == Hero.MainHero.PartyBelongedTo);
			}
			return false;
		}

		// Token: 0x040009A6 RID: 2470
		public const string Id = "PlayerBesiegingTag";
	}
}
