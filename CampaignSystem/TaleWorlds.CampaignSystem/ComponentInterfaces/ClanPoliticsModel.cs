using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A4 RID: 420
	public abstract class ClanPoliticsModel : GameModel
	{
		// Token: 0x06001AE2 RID: 6882
		public abstract ExplainedNumber CalculateInfluenceChange(Clan clan, bool includeDescriptions = false);

		// Token: 0x06001AE3 RID: 6883
		public abstract float CalculateSupportForPolicyInClan(Clan clan, PolicyObject policy);

		// Token: 0x06001AE4 RID: 6884
		public abstract float CalculateRelationshipChangeWithSponsor(Clan clan, Clan sponsorClan);

		// Token: 0x06001AE5 RID: 6885
		public abstract int GetInfluenceRequiredToOverrideKingdomDecision(DecisionOutcome popularOption, DecisionOutcome overridingOption, KingdomDecision decision);

		// Token: 0x06001AE6 RID: 6886
		public abstract bool CanHeroBeGovernor(Hero hero);
	}
}
