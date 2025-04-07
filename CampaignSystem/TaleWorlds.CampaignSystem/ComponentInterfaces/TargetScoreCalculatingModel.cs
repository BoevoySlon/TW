using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000185 RID: 389
	public abstract class TargetScoreCalculatingModel : GameModel
	{
		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060019F2 RID: 6642
		public abstract float TravelingToAssignmentFactor { get; }

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060019F3 RID: 6643
		public abstract float BesiegingFactor { get; }

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060019F4 RID: 6644
		public abstract float AssaultingTownFactor { get; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060019F5 RID: 6645
		public abstract float RaidingFactor { get; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060019F6 RID: 6646
		public abstract float DefendingFactor { get; }

		// Token: 0x060019F7 RID: 6647
		public abstract float GetTargetScoreForFaction(Settlement targetSettlement, Army.ArmyTypes missionType, MobileParty mobileParty, float ourStrength, int numberOfEnemyFactionSettlements = -1, float totalEnemyMobilePartyStrength = -1f);

		// Token: 0x060019F8 RID: 6648
		public abstract float CalculatePatrollingScoreForSettlement(Settlement targetSettlement, MobileParty mobileParty);

		// Token: 0x060019F9 RID: 6649
		public abstract float CurrentObjectiveValue(MobileParty mobileParty);
	}
}
