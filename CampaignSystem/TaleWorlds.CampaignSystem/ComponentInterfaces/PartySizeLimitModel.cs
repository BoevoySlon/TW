using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000191 RID: 401
	public abstract class PartySizeLimitModel : GameModel
	{
		// Token: 0x06001A52 RID: 6738
		public abstract ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false);

		// Token: 0x06001A53 RID: 6739
		public abstract ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false);

		// Token: 0x06001A54 RID: 6740
		public abstract int GetTierPartySizeEffect(int tier);

		// Token: 0x06001A55 RID: 6741
		public abstract int GetAssumedPartySizeForLordParty(Hero leaderHero, IFaction partyMapFaction, Clan actualClan);
	}
}
