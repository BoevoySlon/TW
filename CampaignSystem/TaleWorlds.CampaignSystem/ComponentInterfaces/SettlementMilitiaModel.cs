using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000196 RID: 406
	public abstract class SettlementMilitiaModel : GameModel
	{
		// Token: 0x06001A67 RID: 6759
		public abstract int MilitiaToSpawnAfterSiege(Town town);

		// Token: 0x06001A68 RID: 6760
		public abstract ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001A69 RID: 6761
		public abstract float CalculateEliteMilitiaSpawnChance(Settlement settlement);

		// Token: 0x06001A6A RID: 6762
		public abstract void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate);
	}
}
