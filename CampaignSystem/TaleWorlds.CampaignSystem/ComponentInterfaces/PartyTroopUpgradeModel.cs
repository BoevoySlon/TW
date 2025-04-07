using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BE RID: 446
	public abstract class PartyTroopUpgradeModel : GameModel
	{
		// Token: 0x06001B84 RID: 7044
		public abstract bool CanPartyUpgradeTroopToTarget(PartyBase party, CharacterObject character, CharacterObject target);

		// Token: 0x06001B85 RID: 7045
		public abstract bool IsTroopUpgradeable(PartyBase party, CharacterObject character);

		// Token: 0x06001B86 RID: 7046
		public abstract bool DoesPartyHaveRequiredItemsForUpgrade(PartyBase party, CharacterObject upgradeTarget);

		// Token: 0x06001B87 RID: 7047
		public abstract bool DoesPartyHaveRequiredPerksForUpgrade(PartyBase party, CharacterObject character, CharacterObject upgradeTarget, out PerkObject requiredPerk);

		// Token: 0x06001B88 RID: 7048
		public abstract int GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget);

		// Token: 0x06001B89 RID: 7049
		public abstract int GetXpCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget);

		// Token: 0x06001B8A RID: 7050
		public abstract int GetSkillXpFromUpgradingTroops(PartyBase party, CharacterObject troop, int numberOfTroops);

		// Token: 0x06001B8B RID: 7051
		public abstract float GetUpgradeChanceForTroopUpgrade(PartyBase party, CharacterObject troop, int upgradeTargetIndex);
	}
}
