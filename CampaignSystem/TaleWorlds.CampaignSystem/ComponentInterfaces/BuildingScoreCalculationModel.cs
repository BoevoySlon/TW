using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BA RID: 442
	public abstract class BuildingScoreCalculationModel : GameModel
	{
		// Token: 0x06001B6E RID: 7022
		public abstract Building GetNextBuilding(Town town);
	}
}
