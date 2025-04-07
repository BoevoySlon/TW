using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000174 RID: 372
	public abstract class RaidModel : GameModel
	{
		// Token: 0x06001959 RID: 6489
		public abstract MBReadOnlyList<ValueTuple<ItemObject, float>> GetCommonLootItemScores();

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x0600195A RID: 6490
		public abstract int GoldRewardForEachLostHearth { get; }

		// Token: 0x0600195B RID: 6491
		public abstract float CalculateHitDamage(MapEventSide attackerSide, float settlementHitPoints);
	}
}
