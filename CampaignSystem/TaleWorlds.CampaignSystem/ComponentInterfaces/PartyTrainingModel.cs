using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000170 RID: 368
	public abstract class PartyTrainingModel : GameModel
	{
		// Token: 0x06001941 RID: 6465
		public abstract int GenerateSharedXp(CharacterObject troop, int xp, MobileParty mobileParty);

		// Token: 0x06001942 RID: 6466
		public abstract int CalculateXpGainFromBattles(FlattenedTroopRosterElement troopRosterElement, PartyBase party);

		// Token: 0x06001943 RID: 6467
		public abstract int GetXpReward(CharacterObject character);

		// Token: 0x06001944 RID: 6468
		public abstract ExplainedNumber GetEffectiveDailyExperience(MobileParty party, TroopRosterElement troop);
	}
}
