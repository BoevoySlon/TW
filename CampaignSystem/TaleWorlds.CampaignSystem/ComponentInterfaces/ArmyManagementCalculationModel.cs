using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018A RID: 394
	public abstract class ArmyManagementCalculationModel : GameModel
	{
		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06001A11 RID: 6673
		public abstract int InfluenceValuePerGold { get; }

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001A12 RID: 6674
		public abstract int AverageCallToArmyCost { get; }

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001A13 RID: 6675
		public abstract int CohesionThresholdForDispersion { get; }

		// Token: 0x06001A14 RID: 6676
		public abstract int CalculatePartyInfluenceCost(MobileParty armyLeaderParty, MobileParty party);

		// Token: 0x06001A15 RID: 6677
		public abstract float DailyBeingAtArmyInfluenceAward(MobileParty armyMemberParty);

		// Token: 0x06001A16 RID: 6678
		public abstract List<MobileParty> GetMobilePartiesToCallToArmy(MobileParty leaderParty);

		// Token: 0x06001A17 RID: 6679
		public abstract int CalculateTotalInfluenceCost(Army army, float percentage);

		// Token: 0x06001A18 RID: 6680
		public abstract float GetPartySizeScore(MobileParty party);

		// Token: 0x06001A19 RID: 6681
		public abstract bool CheckPartyEligibility(MobileParty party);

		// Token: 0x06001A1A RID: 6682
		public abstract int GetPartyRelation(Hero hero);

		// Token: 0x06001A1B RID: 6683
		public abstract ExplainedNumber CalculateDailyCohesionChange(Army army, bool includeDescriptions = false);

		// Token: 0x06001A1C RID: 6684
		public abstract int CalculateNewCohesion(Army army, PartyBase newParty, int calculatedCohesion, int sign);

		// Token: 0x06001A1D RID: 6685
		public abstract int GetCohesionBoostInfluenceCost(Army army, int percentageToBoost = 100);

		// Token: 0x06001A1E RID: 6686
		public abstract int GetCohesionBoostGoldCost(Army army, float percentageToBoost = 100f);

		// Token: 0x06001A1F RID: 6687
		public abstract int GetPartyStrength(PartyBase party);
	}
}
