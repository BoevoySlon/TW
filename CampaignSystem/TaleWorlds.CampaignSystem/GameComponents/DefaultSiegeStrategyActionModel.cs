using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013D RID: 317
	public class DefaultSiegeStrategyActionModel : SiegeStrategyActionModel
	{
		// Token: 0x060017E2 RID: 6114 RVA: 0x00077410 File Offset: 0x00075610
		public override void GetLogicalActionForStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex)
		{
			siegeAction = SiegeStrategyActionModel.SiegeAction.Hold;
			siegeEngineType = null;
			deploymentIndex = -1;
			reserveIndex = -1;
			SiegeStrategy siegeStrategy = side.SiegeStrategy;
			if (siegeStrategy == DefaultSiegeStrategies.Custom)
			{
				return;
			}
			if (siegeStrategy == DefaultSiegeStrategies.PreserveStrength)
			{
				this.GetLogicalActionForPreserveStrengthStrategy(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex);
				return;
			}
			if (side.BattleSide == BattleSideEnum.Attacker)
			{
				if (siegeStrategy == DefaultSiegeStrategies.PrepareAssault)
				{
					this.GetLogicalActionForPrepareAssaultStrategy(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex);
					return;
				}
				if (siegeStrategy == DefaultSiegeStrategies.BreachWalls)
				{
					this.GetLogicalActionForBreachWallsStrategy(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex);
					return;
				}
				if (siegeStrategy == DefaultSiegeStrategies.WearOutDefenders)
				{
					this.GetLogicalActionForWearOutDefendersStrategy(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex);
					return;
				}
			}
			else if (side.BattleSide == BattleSideEnum.Defender)
			{
				if (siegeStrategy == DefaultSiegeStrategies.PrepareAgainstAssault)
				{
					this.GetLogicalActionForPrepareAgainstAssaultStrategy(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex);
					return;
				}
				if (siegeStrategy == DefaultSiegeStrategies.CounterBombardment)
				{
					this.GetLogicalActionForCounterBombardmentStrategy(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex);
				}
			}
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000774D0 File Offset: 0x000756D0
		private bool CheckIfStrategyListSatisfied(ISiegeEventSide side, List<ValueTuple<SiegeEngineType, int>> engineList)
		{
			SiegeEvent.SiegeEnginesContainer siegeEngines = side.SiegeEngines;
			foreach (ValueTuple<SiegeEngineType, int> valueTuple in engineList)
			{
				int num;
				siegeEngines.DeployedSiegeEngineTypesCount.TryGetValue(valueTuple.Item1, out num);
				if (num != valueTuple.Item2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00077544 File Offset: 0x00075744
		private void GetLogicalActionToCompleteEngineList(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex, List<ValueTuple<SiegeEngineType, int>> engineList)
		{
			siegeAction = SiegeStrategyActionModel.SiegeAction.Hold;
			siegeEngineType = null;
			deploymentIndex = -1;
			reserveIndex = -1;
			if (this.CheckIfStrategyListSatisfied(side, engineList))
			{
				return;
			}
			SiegeEvent.SiegeEnginesContainer siegeEngines = side.SiegeEngines;
			int num = -1;
			int num2 = -1;
			foreach (KeyValuePair<SiegeEngineType, int> keyValuePair in siegeEngines.DeployedSiegeEngineTypesCount)
			{
				int num3 = -1;
				foreach (ValueTuple<SiegeEngineType, int> valueTuple in engineList)
				{
					if (keyValuePair.Key == valueTuple.Item1)
					{
						num3 = valueTuple.Item2;
						break;
					}
				}
				if (num3 < 0 || num3 < keyValuePair.Value)
				{
					SiegeEngineType excessSiegeEngineType = keyValuePair.Key;
					Func<SiegeEvent.SiegeEngineConstructionProgress, bool> predicate = (SiegeEvent.SiegeEngineConstructionProgress engine) => engine != null && engine.SiegeEngine == excessSiegeEngineType && engine.IsActive && engine.Hitpoints > 0f;
					if (num2 == -1 && excessSiegeEngineType.IsRanged)
					{
						num2 = siegeEngines.DeployedRangedSiegeEngines.FindIndex(predicate);
					}
					else if (num == -1 && !excessSiegeEngineType.IsRanged)
					{
						num = siegeEngines.DeployedMeleeSiegeEngines.FindIndex(predicate);
					}
				}
			}
			int num4 = siegeEngines.DeployedMeleeSiegeEngines.FindIndex((SiegeEvent.SiegeEngineConstructionProgress engine) => engine == null);
			int num5 = siegeEngines.DeployedRangedSiegeEngines.FindIndex((SiegeEvent.SiegeEngineConstructionProgress engine) => engine == null);
			if (num4 == -1 && num5 == -1 && num == -1 && num2 == -1)
			{
				return;
			}
			int num6 = (num5 != -1) ? num5 : num2;
			int num7 = (num4 != -1) ? num4 : num;
			if (!siegeEngines.ReservedSiegeEngines.IsEmpty<SiegeEvent.SiegeEngineConstructionProgress>())
			{
				foreach (ValueTuple<SiegeEngineType, int> valueTuple2 in engineList)
				{
					int num8;
					siegeEngines.DeployedSiegeEngineTypesCount.TryGetValue(valueTuple2.Item1, out num8);
					if (num8 < valueTuple2.Item2)
					{
						SiegeEngineType slackEngineType = valueTuple2.Item1;
						int num9;
						siegeEngines.ReservedSiegeEngineTypesCount.TryGetValue(slackEngineType, out num9);
						if (num9 > 0)
						{
							int num10 = slackEngineType.IsRanged ? num6 : num7;
							if (num10 != -1)
							{
								siegeAction = SiegeStrategyActionModel.SiegeAction.DeploySiegeEngineFromReserve;
								siegeEngineType = slackEngineType;
								reserveIndex = siegeEngines.ReservedSiegeEngines.FindIndex((SiegeEvent.SiegeEngineConstructionProgress reservedEngine) => reservedEngine.SiegeEngine == slackEngineType);
								deploymentIndex = num10;
								return;
							}
						}
					}
				}
			}
			if (side.BattleSide == BattleSideEnum.Defender || (side as BesiegerCamp).IsPreparationComplete)
			{
				bool flag = false;
				using (List<SiegeEvent.SiegeEngineConstructionProgress>.Enumerator enumerator3 = siegeEngines.DeployedSiegeEngines.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.Progress < 1f)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					foreach (ValueTuple<SiegeEngineType, int> valueTuple3 in engineList)
					{
						int num11;
						siegeEngines.DeployedSiegeEngineTypesCount.TryGetValue(valueTuple3.Item1, out num11);
						if (num11 < valueTuple3.Item2)
						{
							SiegeEngineType item = valueTuple3.Item1;
							int num12 = item.IsRanged ? num6 : num7;
							if (num12 != -1)
							{
								siegeAction = SiegeStrategyActionModel.SiegeAction.ConstructNewSiegeEngine;
								siegeEngineType = item;
								deploymentIndex = num12;
								return;
							}
						}
					}
				}
			}
			if (num4 != -1)
			{
				int num13 = siegeEngines.ReservedSiegeEngines.FindIndex((SiegeEvent.SiegeEngineConstructionProgress engine) => !engine.SiegeEngine.IsRanged);
				if (num13 != -1)
				{
					siegeAction = SiegeStrategyActionModel.SiegeAction.DeploySiegeEngineFromReserve;
					siegeEngineType = siegeEngines.ReservedSiegeEngines[num13].SiegeEngine;
					reserveIndex = num13;
					deploymentIndex = num4;
					return;
				}
			}
			if (num5 != -1)
			{
				int num14 = siegeEngines.ReservedSiegeEngines.FindIndex((SiegeEvent.SiegeEngineConstructionProgress engine) => engine.SiegeEngine.IsRanged);
				if (num14 != -1)
				{
					siegeAction = SiegeStrategyActionModel.SiegeAction.DeploySiegeEngineFromReserve;
					siegeEngineType = siegeEngines.ReservedSiegeEngines[num14].SiegeEngine;
					reserveIndex = num14;
					deploymentIndex = num5;
					return;
				}
			}
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00077998 File Offset: 0x00075B98
		private void GetLogicalActionForPreserveStrengthStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex)
		{
			SiegeEvent.SiegeEnginesContainer siegeEngines = side.SiegeEngines;
			for (int i = 0; i < side.SiegeEngines.DeployedRangedSiegeEngines.Length; i++)
			{
				SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress = siegeEngines.DeployedRangedSiegeEngines[i];
				if (siegeEngineConstructionProgress != null && siegeEngineConstructionProgress.IsActive && siegeEngineConstructionProgress.Hitpoints > 0f)
				{
					siegeAction = SiegeStrategyActionModel.SiegeAction.MoveSiegeEngineToReserve;
					siegeEngineType = siegeEngineConstructionProgress.SiegeEngine;
					deploymentIndex = i;
					reserveIndex = -1;
					return;
				}
			}
			for (int j = 0; j < side.SiegeEngines.DeployedMeleeSiegeEngines.Length; j++)
			{
				SiegeEvent.SiegeEngineConstructionProgress siegeEngineConstructionProgress2 = siegeEngines.DeployedMeleeSiegeEngines[j];
				if (siegeEngineConstructionProgress2 != null && siegeEngineConstructionProgress2.IsActive && siegeEngineConstructionProgress2.Hitpoints > 0f)
				{
					siegeAction = SiegeStrategyActionModel.SiegeAction.MoveSiegeEngineToReserve;
					siegeEngineType = siegeEngineConstructionProgress2.SiegeEngine;
					deploymentIndex = j;
					reserveIndex = -1;
					return;
				}
			}
			siegeAction = SiegeStrategyActionModel.SiegeAction.Hold;
			siegeEngineType = null;
			deploymentIndex = -1;
			reserveIndex = -1;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00077A5C File Offset: 0x00075C5C
		private void GetLogicalActionForPrepareAssaultStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex)
		{
			if (this._prepareAssaultEngineList == null)
			{
				this._prepareAssaultEngineList = new List<ValueTuple<SiegeEngineType, int>>
				{
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Ram, 1),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.SiegeTower, 2),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Ballista, 2),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Onager, 2)
				};
			}
			this.GetLogicalActionToCompleteEngineList(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex, this._prepareAssaultEngineList);
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00077AD4 File Offset: 0x00075CD4
		private void GetLogicalActionForBreachWallsStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex)
		{
			if (this._breachWallsEngineList == null)
			{
				this._breachWallsEngineList = new List<ValueTuple<SiegeEngineType, int>>
				{
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Ram, 1),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.SiegeTower, 1),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Onager, 4)
				};
			}
			this.GetLogicalActionToCompleteEngineList(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex, this._breachWallsEngineList);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00077B3C File Offset: 0x00075D3C
		private void GetLogicalActionForWearOutDefendersStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex)
		{
			if (this._wearOutDefendersEngineList == null)
			{
				this._wearOutDefendersEngineList = new List<ValueTuple<SiegeEngineType, int>>
				{
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Ram, 1),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.SiegeTower, 1),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Trebuchet, 4)
				};
			}
			this.GetLogicalActionToCompleteEngineList(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex, this._wearOutDefendersEngineList);
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00077BA4 File Offset: 0x00075DA4
		private void GetLogicalActionForPrepareAgainstAssaultStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex)
		{
			if (this._prepareAgainstAssaultEngineList == null)
			{
				this._prepareAgainstAssaultEngineList = new List<ValueTuple<SiegeEngineType, int>>
				{
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.FireCatapult, 1),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Catapult, 2),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.FireBallista, 1)
				};
			}
			this.GetLogicalActionToCompleteEngineList(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex, this._prepareAgainstAssaultEngineList);
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00077C0C File Offset: 0x00075E0C
		private void GetLogicalActionForCounterBombardmentStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex)
		{
			if (this._counterBombardmentEngineList == null)
			{
				this._counterBombardmentEngineList = new List<ValueTuple<SiegeEngineType, int>>
				{
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.FireCatapult, 2),
					new ValueTuple<SiegeEngineType, int>(DefaultSiegeEngineTypes.Catapult, 2)
				};
			}
			this.GetLogicalActionToCompleteEngineList(side, out siegeAction, out siegeEngineType, out deploymentIndex, out reserveIndex, this._counterBombardmentEngineList);
		}

		// Token: 0x04000869 RID: 2153
		private List<ValueTuple<SiegeEngineType, int>> _prepareAssaultEngineList;

		// Token: 0x0400086A RID: 2154
		private List<ValueTuple<SiegeEngineType, int>> _breachWallsEngineList;

		// Token: 0x0400086B RID: 2155
		private List<ValueTuple<SiegeEngineType, int>> _wearOutDefendersEngineList;

		// Token: 0x0400086C RID: 2156
		private List<ValueTuple<SiegeEngineType, int>> _prepareAgainstAssaultEngineList;

		// Token: 0x0400086D RID: 2157
		private List<ValueTuple<SiegeEngineType, int>> _counterBombardmentEngineList;
	}
}
