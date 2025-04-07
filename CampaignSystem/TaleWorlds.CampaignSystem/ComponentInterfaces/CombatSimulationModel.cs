using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000177 RID: 375
	public abstract class CombatSimulationModel : GameModel
	{
		// Token: 0x0600196A RID: 6506
		public abstract int SimulateHit(CharacterObject strikerTroop, CharacterObject struckTroop, PartyBase strikerParty, PartyBase struckParty, float strikerAdvantage, MapEvent battle);

		// Token: 0x0600196B RID: 6507
		[return: TupleElementNames(new string[]
		{
			"defenderRounds",
			"attackerRounds"
		})]
		public abstract ValueTuple<int, int> GetSimulationRoundsForBattle(MapEvent mapEvent, int numDefenders, int numAttackers);

		// Token: 0x0600196C RID: 6508
		public abstract int GetNumberOfEquipmentsBuilt(Settlement settlement);

		// Token: 0x0600196D RID: 6509
		public abstract float GetMaximumSiegeEquipmentProgress(Settlement settlement);

		// Token: 0x0600196E RID: 6510
		public abstract float GetSettlementAdvantage(Settlement settlement);

		// Token: 0x0600196F RID: 6511
		[return: TupleElementNames(new string[]
		{
			"defenderAdvantage",
			"attackerAdvantage"
		})]
		public abstract ValueTuple<float, float> GetBattleAdvantage(PartyBase defenderParty, PartyBase attackerParty, MapEvent.BattleTypes mapEventType, Settlement settlement);
	}
}
