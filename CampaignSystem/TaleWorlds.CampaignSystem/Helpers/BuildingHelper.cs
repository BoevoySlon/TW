using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x0200001A RID: 26
	public static class BuildingHelper
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000BE04 File Offset: 0x0000A004
		public static void ChangeCurrentBuilding(BuildingType buildingType, Town town)
		{
			Queue<Building> queue = new Queue<Building>();
			foreach (Building building in town.Buildings)
			{
				if (building.BuildingType == buildingType)
				{
					queue.Enqueue(building);
					break;
				}
			}
			foreach (Building item in town.BuildingsInProgress)
			{
				queue.Enqueue(item);
			}
			town.BuildingsInProgress = queue;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000BEB4 File Offset: 0x0000A0B4
		public static void ChangeDefaultBuilding(Building newDefault, Town town)
		{
			foreach (Building building in town.Buildings)
			{
				if (building.IsCurrentlyDefault)
				{
					building.IsCurrentlyDefault = false;
				}
				if (building == newDefault)
				{
					building.IsCurrentlyDefault = true;
				}
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000BF1C File Offset: 0x0000A11C
		public static void ChangeCurrentBuildingQueue(List<Building> buildings, Town town)
		{
			town.BuildingsInProgress = new Queue<Building>();
			foreach (Building building in buildings)
			{
				if (!building.BuildingType.IsDefaultProject)
				{
					town.BuildingsInProgress.Enqueue(building);
				}
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000BF88 File Offset: 0x0000A188
		public static float GetProgressOfBuilding(Building building, Town town)
		{
			using (List<Building>.Enumerator enumerator = town.Buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == building)
					{
						return building.BuildingProgress / (float)building.GetConstructionCost();
					}
				}
			}
			Debug.FailedAssert(building.Name + "is not a project of" + town.Name, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetProgressOfBuilding", 6252);
			return 0f;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000C018 File Offset: 0x0000A218
		public static int GetDaysToComplete(Building building, Town town)
		{
			BuildingConstructionModel buildingConstructionModel = Campaign.Current.Models.BuildingConstructionModel;
			using (List<Building>.Enumerator enumerator = town.Buildings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == building)
					{
						float num = (float)building.GetConstructionCost() - building.BuildingProgress;
						int num2 = (int)town.Construction;
						if (num2 != 0)
						{
							int num3 = (int)(num / (float)num2);
							int num4 = town.IsCastle ? buildingConstructionModel.CastleBoostCost : buildingConstructionModel.TownBoostCost;
							if (town.BoostBuildingProcess >= num4)
							{
								int num5 = town.BoostBuildingProcess / num4;
								if (num3 > num5)
								{
									int num6 = num5 * num2;
									int num7 = Campaign.Current.Models.BuildingConstructionModel.CalculateDailyConstructionPowerWithoutBoost(town);
									return num5 + MathF.Max((int)((num - (float)num6) / (float)num7), 1);
								}
							}
							return MathF.Max(num3, 1);
						}
						return -1;
					}
				}
			}
			Debug.FailedAssert(building.Name + "is not a project of" + town.Name, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetDaysToComplete", 6294);
			return 0;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000C148 File Offset: 0x0000A348
		public static int GetTierOfBuilding(BuildingType buildingType, Town town)
		{
			foreach (Building building in town.Buildings)
			{
				if (building.BuildingType == buildingType)
				{
					return building.CurrentLevel;
				}
			}
			Debug.FailedAssert(buildingType.Name + "is not a project of" + town.Name, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetTierOfBuilding", 6308);
			return 0;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000C1D4 File Offset: 0x0000A3D4
		public static void BoostBuildingProcessWithGold(int gold, Town town)
		{
			if (gold < town.BoostBuildingProcess)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, town.BoostBuildingProcess - gold, false);
			}
			else if (gold > town.BoostBuildingProcess)
			{
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, gold - town.BoostBuildingProcess, false);
			}
			town.BoostBuildingProcess = gold;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000C224 File Offset: 0x0000A424
		public static void AddDefaultDailyBonus(Town fortification, BuildingEffectEnum effect, ref ExplainedNumber result)
		{
			float buildingEffectAmount = Campaign.Current.Models.BuildingEffectModel.GetBuildingEffectAmount(fortification.CurrentDefaultBuilding, effect);
			result.Add(buildingEffectAmount, fortification.CurrentDefaultBuilding.BuildingType.Name, null);
		}
	}
}
