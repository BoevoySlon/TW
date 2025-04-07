using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B6 RID: 438
	public abstract class SiegeEventModel : GameModel
	{
		// Token: 0x06001B49 RID: 6985
		public abstract int GetSiegeEngineDestructionCasualties(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType destroyedSiegeEngine);

		// Token: 0x06001B4A RID: 6986
		public abstract float GetCasualtyChance(MobileParty siegeParty, SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001B4B RID: 6987
		public abstract int GetColleteralDamageCasualties(SiegeEngineType attackerSiegeEngine, MobileParty party);

		// Token: 0x06001B4C RID: 6988
		public abstract float GetSiegeEngineHitChance(SiegeEngineType siegeEngineType, BattleSideEnum battleSide, SiegeBombardTargets target, Town town);

		// Token: 0x06001B4D RID: 6989
		public abstract string GetSiegeEngineMapPrefabName(SiegeEngineType siegeEngineType, int wallLevel, BattleSideEnum side);

		// Token: 0x06001B4E RID: 6990
		public abstract string GetSiegeEngineMapProjectilePrefabName(SiegeEngineType siegeEngineType);

		// Token: 0x06001B4F RID: 6991
		public abstract string GetSiegeEngineMapReloadAnimationName(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001B50 RID: 6992
		public abstract string GetSiegeEngineMapFireAnimationName(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001B51 RID: 6993
		public abstract sbyte GetSiegeEngineMapProjectileBoneIndex(SiegeEngineType siegeEngineType, BattleSideEnum side);

		// Token: 0x06001B52 RID: 6994
		public abstract float GetSiegeStrategyScore(SiegeEvent siege, BattleSideEnum side, SiegeStrategy strategy);

		// Token: 0x06001B53 RID: 6995
		public abstract float GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side);

		// Token: 0x06001B54 RID: 6996
		public abstract MobileParty GetEffectiveSiegePartyForSide(SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001B55 RID: 6997
		public abstract float GetAvailableManDayPower(ISiegeEventSide side);

		// Token: 0x06001B56 RID: 6998
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerRangedSiegeEngines(PartyBase party);

		// Token: 0x06001B57 RID: 6999
		public abstract IEnumerable<SiegeEngineType> GetAvailableDefenderSiegeEngines(PartyBase party);

		// Token: 0x06001B58 RID: 7000
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerRamSiegeEngines(PartyBase party);

		// Token: 0x06001B59 RID: 7001
		public abstract IEnumerable<SiegeEngineType> GetAvailableAttackerTowerSiegeEngines(PartyBase party);

		// Token: 0x06001B5A RID: 7002
		public abstract IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement);

		// Token: 0x06001B5B RID: 7003
		public abstract IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp camp);

		// Token: 0x06001B5C RID: 7004
		public abstract float GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType siegeEngine, BattleSideEnum battleSide);

		// Token: 0x06001B5D RID: 7005
		public abstract int GetRangedSiegeEngineReloadTime(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine);

		// Token: 0x06001B5E RID: 7006
		public abstract float GetSiegeEngineDamage(SiegeEvent siegeEvent, BattleSideEnum battleSide, SiegeEngineType siegeEngine, SiegeBombardTargets target);

		// Token: 0x06001B5F RID: 7007
		public abstract FlattenedTroopRoster GetPriorityTroopsForSallyOutAmbush();
	}
}
