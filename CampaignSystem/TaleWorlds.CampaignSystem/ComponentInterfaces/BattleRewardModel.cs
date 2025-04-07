using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018C RID: 396
	public abstract class BattleRewardModel : GameModel
	{
		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001A23 RID: 6691
		public abstract float DestroyHideoutBannerLootChance { get; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001A24 RID: 6692
		public abstract float CaptureSettlementBannerLootChance { get; }

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001A25 RID: 6693
		public abstract float DefeatRegularHeroBannerLootChance { get; }

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001A26 RID: 6694
		public abstract float DefeatClanLeaderBannerLootChance { get; }

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001A27 RID: 6695
		public abstract float DefeatKingdomRulerBannerLootChance { get; }

		// Token: 0x06001A28 RID: 6696
		public abstract int GetPlayerGainedRelationAmount(MapEvent mapEvent, Hero hero);

		// Token: 0x06001A29 RID: 6697
		public abstract ExplainedNumber CalculateRenownGain(PartyBase party, float renownValueOfBattle, float contributionShare);

		// Token: 0x06001A2A RID: 6698
		public abstract ExplainedNumber CalculateInfluenceGain(PartyBase party, float influenceValueOfBattle, float contributionShare);

		// Token: 0x06001A2B RID: 6699
		public abstract ExplainedNumber CalculateMoraleGainVictory(PartyBase party, float renownValueOfBattle, float contributionShare);

		// Token: 0x06001A2C RID: 6700
		public abstract int CalculateGoldLossAfterDefeat(Hero partyLeaderHero);

		// Token: 0x06001A2D RID: 6701
		public abstract EquipmentElement GetLootedItemFromTroop(CharacterObject character, float targetValue);

		// Token: 0x06001A2E RID: 6702
		public abstract float GetPartySavePrisonerAsMemberShareProbability(PartyBase winnerParty, float lootAmount);

		// Token: 0x06001A2F RID: 6703
		public abstract float GetExpectedLootedItemValue(CharacterObject character);

		// Token: 0x06001A30 RID: 6704
		public abstract float GetAITradePenalty();
	}
}
