using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x02000352 RID: 850
	public interface ISkillLevelingManager
	{
		// Token: 0x06003053 RID: 12371
		void OnCombatHit(CharacterObject affectorCharacter, CharacterObject affectedCharacter, CharacterObject captain, Hero commander, float speedBonusFromMovement, float shotDifficulty, WeaponComponentData affectorWeapon, float hitPointRatio, CombatXpModel.MissionTypeEnum missionType, bool isAffectorMounted, bool isTeamKill, bool isAffectorUnderCommand, float damageAmount, bool isFatal, bool isSiegeEngineHit, bool isHorseCharge);

		// Token: 0x06003054 RID: 12372
		void OnSiegeEngineDestroyed(MobileParty party, SiegeEngineType destroyedSiegeEngine);

		// Token: 0x06003055 RID: 12373
		void OnSimulationCombatKill(CharacterObject affectorCharacter, CharacterObject affectedCharacter, PartyBase affectorParty, PartyBase commanderParty);

		// Token: 0x06003056 RID: 12374
		void OnTradeProfitMade(PartyBase party, int tradeProfit);

		// Token: 0x06003057 RID: 12375
		void OnTradeProfitMade(Hero hero, int tradeProfit);

		// Token: 0x06003058 RID: 12376
		void OnSettlementProjectFinished(Settlement settlement);

		// Token: 0x06003059 RID: 12377
		void OnSettlementGoverned(Hero governor, Settlement settlement);

		// Token: 0x0600305A RID: 12378
		void OnInfluenceSpent(Hero hero, float amountSpent);

		// Token: 0x0600305B RID: 12379
		void OnGainRelation(Hero hero, Hero gainedRelationWith, float relationChange, ChangeRelationAction.ChangeRelationDetail detail = ChangeRelationAction.ChangeRelationDetail.Default);

		// Token: 0x0600305C RID: 12380
		void OnTroopRecruited(Hero hero, int amount, int tier);

		// Token: 0x0600305D RID: 12381
		void OnBribeGiven(int amount);

		// Token: 0x0600305E RID: 12382
		void OnWarehouseProduction(EquipmentElement production);

		// Token: 0x0600305F RID: 12383
		void OnBanditsRecruited(MobileParty mobileParty, CharacterObject bandit, int count);

		// Token: 0x06003060 RID: 12384
		void OnMainHeroReleasedFromCaptivity(float captivityTime);

		// Token: 0x06003061 RID: 12385
		void OnMainHeroTortured();

		// Token: 0x06003062 RID: 12386
		void OnMainHeroDisguised(bool isNotCaught);

		// Token: 0x06003063 RID: 12387
		void OnRaid(MobileParty attackerParty, ItemRoster lootedItems);

		// Token: 0x06003064 RID: 12388
		void OnLoot(MobileParty attackerParty, MobileParty forcedParty, ItemRoster lootedItems, bool attacked);

		// Token: 0x06003065 RID: 12389
		void OnPrisonerSell(MobileParty mobileParty, in TroopRoster prisonerRoster);

		// Token: 0x06003066 RID: 12390
		void OnSurgeryApplied(MobileParty party, bool surgerySuccess, int troopTier);

		// Token: 0x06003067 RID: 12391
		void OnTacticsUsed(MobileParty party, float xp);

		// Token: 0x06003068 RID: 12392
		void OnHideoutSpotted(MobileParty party, PartyBase spottedParty);

		// Token: 0x06003069 RID: 12393
		void OnTrackDetected(Track track);

		// Token: 0x0600306A RID: 12394
		void OnTravelOnFoot(Hero hero, float speed);

		// Token: 0x0600306B RID: 12395
		void OnTravelOnHorse(Hero hero, float speed);

		// Token: 0x0600306C RID: 12396
		void OnHeroHealedWhileWaiting(Hero hero, int healingAmount);

		// Token: 0x0600306D RID: 12397
		void OnRegularTroopHealedWhileWaiting(MobileParty mobileParty, int healedTroopCount, float averageTier);

		// Token: 0x0600306E RID: 12398
		void OnLeadingArmy(MobileParty mobileParty);

		// Token: 0x0600306F RID: 12399
		void OnSieging(MobileParty mobileParty);

		// Token: 0x06003070 RID: 12400
		void OnSiegeEngineBuilt(MobileParty mobileParty, SiegeEngineType siegeEngine);

		// Token: 0x06003071 RID: 12401
		void OnUpgradeTroops(PartyBase party, CharacterObject troop, CharacterObject upgrade, int numberOfTroops);

		// Token: 0x06003072 RID: 12402
		void OnPersuasionSucceeded(Hero targetHero, SkillObject skill, PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient);

		// Token: 0x06003073 RID: 12403
		void OnPrisonBreakEnd(Hero prisonerHero, bool isSucceeded);

		// Token: 0x06003074 RID: 12404
		void OnWallBreached(MobileParty party);

		// Token: 0x06003075 RID: 12405
		void OnForceVolunteers(MobileParty attackerParty, PartyBase forcedParty);

		// Token: 0x06003076 RID: 12406
		void OnForceSupplies(MobileParty attackerParty, ItemRoster lootedItems, bool attacked);

		// Token: 0x06003077 RID: 12407
		void OnAIPartiesTravel(Hero hero, bool isCaravanParty, TerrainType currentTerrainType);

		// Token: 0x06003078 RID: 12408
		void OnTraverseTerrain(MobileParty mobileParty, TerrainType currentTerrainType);

		// Token: 0x06003079 RID: 12409
		void OnBattleEnd(PartyBase party, FlattenedTroopRoster flattenedTroopRoster);

		// Token: 0x0600307A RID: 12410
		void OnFoodConsumed(MobileParty mobileParty, bool wasStarving);

		// Token: 0x0600307B RID: 12411
		void OnAlleyCleared(Alley alley);

		// Token: 0x0600307C RID: 12412
		void OnDailyAlleyTick(Alley alley, Hero alleyLeader);

		// Token: 0x0600307D RID: 12413
		void OnBoardGameWonAgainstLord(Hero lord, BoardGameHelper.AIDifficulty difficulty, bool extraXpGain);
	}
}
