using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B3 RID: 435
	public abstract class MilitaryPowerModel : GameModel
	{
		// Token: 0x06001B37 RID: 6967
		public abstract float GetTroopPower(float defaultTroopPower, float leaderModifier = 0f, float contextModifier = 0f);

		// Token: 0x06001B38 RID: 6968
		public abstract float GetTroopPower(CharacterObject troop, BattleSideEnum side, MapEvent.PowerCalculationContext context, float leaderModifier);

		// Token: 0x06001B39 RID: 6969
		public abstract float GetContextModifier(CharacterObject troop, BattleSideEnum battleSideEnum, MapEvent.PowerCalculationContext context);

		// Token: 0x06001B3A RID: 6970
		public abstract float GetLeaderModifierInMapEvent(MapEvent mapEvent, BattleSideEnum battleSideEnum);

		// Token: 0x06001B3B RID: 6971
		public abstract float GetDefaultTroopPower(CharacterObject troop);
	}
}
