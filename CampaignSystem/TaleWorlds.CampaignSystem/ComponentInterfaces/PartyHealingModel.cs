using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000172 RID: 370
	public abstract class PartyHealingModel : GameModel
	{
		// Token: 0x0600194B RID: 6475
		public abstract float GetSurgeryChance(PartyBase party);

		// Token: 0x0600194C RID: 6476
		public abstract float GetSurvivalChance(PartyBase party, CharacterObject agentCharacter, DamageTypes damageType, bool canDamageKillEvenIfBlunt, PartyBase enemyParty = null);

		// Token: 0x0600194D RID: 6477
		public abstract int GetSkillXpFromHealingTroop(PartyBase party);

		// Token: 0x0600194E RID: 6478
		public abstract ExplainedNumber GetDailyHealingForRegulars(MobileParty party, bool includeDescriptions = false);

		// Token: 0x0600194F RID: 6479
		public abstract ExplainedNumber GetDailyHealingHpForHeroes(MobileParty party, bool includeDescriptions = false);

		// Token: 0x06001950 RID: 6480
		public abstract int GetHeroesEffectedHealingAmount(Hero hero, float healingRate);

		// Token: 0x06001951 RID: 6481
		public abstract float GetSiegeBombardmentHitSurgeryChance(PartyBase party);

		// Token: 0x06001952 RID: 6482
		public abstract int GetBattleEndHealingAmount(MobileParty party, Hero hero);
	}
}
