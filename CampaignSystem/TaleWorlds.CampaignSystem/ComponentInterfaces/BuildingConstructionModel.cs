using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A8 RID: 424
	public abstract class BuildingConstructionModel : GameModel
	{
		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001AF9 RID: 6905
		public abstract int TownBoostCost { get; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06001AFA RID: 6906
		public abstract int TownBoostBonus { get; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06001AFB RID: 6907
		public abstract int CastleBoostCost { get; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06001AFC RID: 6908
		public abstract int CastleBoostBonus { get; }

		// Token: 0x06001AFD RID: 6909
		public abstract ExplainedNumber CalculateDailyConstructionPower(Town town, bool includeDescriptions = false);

		// Token: 0x06001AFE RID: 6910
		public abstract int CalculateDailyConstructionPowerWithoutBoost(Town town);

		// Token: 0x06001AFF RID: 6911
		public abstract int GetBoostCost(Town town);

		// Token: 0x06001B00 RID: 6912
		public abstract int GetBoostAmount(Town town);
	}
}
