using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F3 RID: 243
	public class DefaultBuildingScoreCalculationModel : BuildingScoreCalculationModel
	{
		// Token: 0x060014CB RID: 5323 RVA: 0x0005DB64 File Offset: 0x0005BD64
		public override Building GetNextBuilding(Town town)
		{
			Building building = town.Buildings[MBRandom.RandomInt(0, town.Buildings.Count)];
			if (building.CurrentLevel != 3 && !town.BuildingsInProgress.Contains(building) && building.BuildingType.BuildingLocation != BuildingLocation.Daily)
			{
				return building;
			}
			return null;
		}
	}
}
