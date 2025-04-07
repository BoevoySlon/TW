using System;
using System.Collections.Generic;
using System.Threading;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200006E RID: 110
	public class CampaignTickCacheDataStore
	{
		// Token: 0x06000EBE RID: 3774 RVA: 0x00045BF0 File Offset: 0x00043DF0
		internal CampaignTickCacheDataStore()
		{
			this._mobilePartyComparer = new CampaignTickCacheDataStore.MobilePartyComparer();
			this._parallelInitializeCachedPartyVariablesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelInitializeCachedPartyVariables);
			this._parallelCacheTargetPartyVariablesAtFrameStartPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelCacheTargetPartyVariablesAtFrameStart);
			this._parallelArrangePartyIndicesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelArrangePartyIndices);
			this._parallelTickArmiesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickArmies);
			this._parallelTickMovingPartiesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickMovingParties);
			this._parallelTickStationaryPartiesPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelTickStationaryParties);
			this._parallelCheckExitingSettlementsPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelCheckExitingSettlements);
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00045C8C File Offset: 0x00043E8C
		internal void ValidateMobilePartyTickDataCache(int currentTotalMobilePartyCount)
		{
			if (this._currentTotalMobilePartyCapacity <= currentTotalMobilePartyCount)
			{
				this.InitializeCacheArrays();
			}
			this._currentFrameMovingPartyCount = -1;
			this._currentFrameStationaryPartyCount = -1;
			this._currentFrameMovingArmyLeaderCount = -1;
			this._gridChangeCount = -1;
			this._exitingSettlementCount = -1;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00045CC0 File Offset: 0x00043EC0
		private void InitializeCacheArrays()
		{
			int num = (int)((float)this._currentTotalMobilePartyCapacity * 2f);
			this._cacheData = new CampaignTickCacheDataStore.PartyTickCachePerParty[num];
			this._gridChangeMobilePartyList = new MobileParty[num];
			this._exitingSettlementMobilePartyList = new MobileParty[num];
			this._currentTotalMobilePartyCapacity = num;
			this._movingPartyIndices = new int[num];
			this._stationaryPartyIndices = new int[num];
			this._movingArmyLeaderPartyIndices = new int[num];
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00045D2C File Offset: 0x00043F2C
		internal void InitializeDataCache()
		{
			this._currentFrameMovingArmyLeaderCount = Campaign.Current.MobileParties.Count;
			this._currentTotalMobilePartyCapacity = Campaign.Current.MobileParties.Count;
			this._currentFrameStationaryPartyCount = Campaign.Current.MobileParties.Count;
			this.InitializeCacheArrays();
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00045D80 File Offset: 0x00043F80
		private void ParallelCheckExitingSettlements(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				Campaign.Current.MobileParties[i].CheckExitingSettlementParallel(ref this._exitingSettlementCount, ref this._exitingSettlementMobilePartyList);
			}
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00045DBC File Offset: 0x00043FBC
		private void ParallelInitializeCachedPartyVariables(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				MobileParty mobileParty = Campaign.Current.MobileParties[i];
				this._cacheData[i].MobileParty = mobileParty;
				mobileParty.InitializeCachedPartyVariables(ref this._cacheData[i].LocalVariables);
			}
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00045E10 File Offset: 0x00044010
		private void ParallelCacheTargetPartyVariablesAtFrameStart(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._cacheData[i].MobileParty.Ai.CacheTargetPartyVariablesAtFrameStart(ref this._cacheData[i].LocalVariables);
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00045E58 File Offset: 0x00044058
		private void ParallelArrangePartyIndices(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				MobileParty.CachedPartyVariables localVariables = this._cacheData[i].LocalVariables;
				if (localVariables.IsMoving)
				{
					if (localVariables.IsArmyLeader)
					{
						int num = Interlocked.Increment(ref this._currentFrameMovingArmyLeaderCount);
						this._movingArmyLeaderPartyIndices[num] = i;
					}
					else
					{
						int num2 = Interlocked.Increment(ref this._currentFrameMovingPartyCount);
						this._movingPartyIndices[num2] = i;
					}
				}
				else
				{
					int num3 = Interlocked.Increment(ref this._currentFrameStationaryPartyCount);
					this._stationaryPartyIndices[num3] = i;
				}
			}
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00045ED8 File Offset: 0x000440D8
		private void ParallelTickArmies(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._movingArmyLeaderPartyIndices[i];
				CampaignTickCacheDataStore.PartyTickCachePerParty partyTickCachePerParty = this._cacheData[num];
				MobileParty mobileParty = partyTickCachePerParty.MobileParty;
				MobileParty.CachedPartyVariables localVariables = partyTickCachePerParty.LocalVariables;
				mobileParty.TickForMovingArmyLeader(ref localVariables, this._currentDt, this._currentRealDt);
				mobileParty.TickForMobileParty2(ref localVariables, this._currentRealDt, ref this._gridChangeCount, ref this._gridChangeMobilePartyList);
				mobileParty.ValidateSpeed();
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00045F48 File Offset: 0x00044148
		private void ParallelTickMovingParties(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._movingPartyIndices[i];
				CampaignTickCacheDataStore.PartyTickCachePerParty partyTickCachePerParty = this._cacheData[num];
				MobileParty mobileParty = partyTickCachePerParty.MobileParty;
				MobileParty.CachedPartyVariables localVariables = partyTickCachePerParty.LocalVariables;
				mobileParty.TickForMovingMobileParty(ref localVariables, this._currentDt, this._currentRealDt);
				mobileParty.TickForMobileParty2(ref localVariables, this._currentRealDt, ref this._gridChangeCount, ref this._gridChangeMobilePartyList);
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00045FB4 File Offset: 0x000441B4
		private void ParallelTickStationaryParties(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				int num = this._stationaryPartyIndices[i];
				CampaignTickCacheDataStore.PartyTickCachePerParty partyTickCachePerParty = this._cacheData[num];
				MobileParty mobileParty = partyTickCachePerParty.MobileParty;
				MobileParty.CachedPartyVariables localVariables = partyTickCachePerParty.LocalVariables;
				mobileParty.TickForStationaryMobileParty(ref localVariables, this._currentDt, this._currentRealDt);
				mobileParty.TickForMobileParty2(ref localVariables, this._currentRealDt, ref this._gridChangeCount, ref this._gridChangeMobilePartyList);
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00046020 File Offset: 0x00044220
		internal void Tick()
		{
			TWParallel.For(0, Campaign.Current.MobileParties.Count, this._parallelCheckExitingSettlementsPredicate, 16);
			Array.Sort<MobileParty>(this._exitingSettlementMobilePartyList, 0, this._exitingSettlementCount + 1, this._mobilePartyComparer);
			for (int i = 0; i < this._exitingSettlementCount + 1; i++)
			{
				LeaveSettlementAction.ApplyForParty(this._exitingSettlementMobilePartyList[i]);
			}
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00046084 File Offset: 0x00044284
		internal void RealTick(float dt, float realDt)
		{
			this._currentDt = dt;
			this._currentRealDt = realDt;
			this.ValidateMobilePartyTickDataCache(Campaign.Current.MobileParties.Count);
			int count = Campaign.Current.MobileParties.Count;
			TWParallel.For(0, count, this._parallelInitializeCachedPartyVariablesPredicate, 16);
			TWParallel.For(0, count, this._parallelCacheTargetPartyVariablesAtFrameStartPredicate, 16);
			TWParallel.For(0, count, this._parallelArrangePartyIndicesPredicate, 16);
			TWParallel.For(0, this._currentFrameMovingArmyLeaderCount + 1, this._parallelTickArmiesPredicate, 16);
			TWParallel.For(0, this._currentFrameMovingPartyCount + 1, this._parallelTickMovingPartiesPredicate, 16);
			TWParallel.For(0, this._currentFrameStationaryPartyCount + 1, this._parallelTickStationaryPartiesPredicate, 16);
			this.UpdateVisibilitiesAroundMainParty();
			Array.Sort<MobileParty>(this._gridChangeMobilePartyList, 0, this._gridChangeCount + 1, this._mobilePartyComparer);
			Campaign campaign = Campaign.Current;
			for (int i = 0; i < this._gridChangeCount + 1; i++)
			{
				campaign.MobilePartyLocator.UpdateLocator(this._gridChangeMobilePartyList[i]);
			}
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00046180 File Offset: 0x00044380
		private void UpdateVisibilitiesAroundMainParty()
		{
			if (MobileParty.MainParty.CurrentNavigationFace.IsValid() && Campaign.Current.GetSimplifiedTimeControlMode() != CampaignTimeControlMode.Stop)
			{
				float seeingRange = MobileParty.MainParty.SeeingRange;
				LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(MobileParty.MainParty.Position2D, seeingRange + 25f);
				for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
				{
					if (!mobileParty.IsMilitia && !mobileParty.IsGarrison)
					{
						mobileParty.Party.UpdateVisibilityAndInspected(seeingRange);
					}
				}
				LocatableSearchData<Settlement> locatableSearchData2 = Settlement.StartFindingLocatablesAroundPosition(MobileParty.MainParty.Position2D, seeingRange + 25f);
				for (Settlement settlement = Settlement.FindNextLocatable(ref locatableSearchData2); settlement != null; settlement = Settlement.FindNextLocatable(ref locatableSearchData2))
				{
					settlement.Party.UpdateVisibilityAndInspected(seeingRange);
				}
			}
		}

		// Token: 0x0400043A RID: 1082
		private CampaignTickCacheDataStore.PartyTickCachePerParty[] _cacheData;

		// Token: 0x0400043B RID: 1083
		private MobileParty[] _gridChangeMobilePartyList;

		// Token: 0x0400043C RID: 1084
		private MobileParty[] _exitingSettlementMobilePartyList;

		// Token: 0x0400043D RID: 1085
		private int[] _movingPartyIndices;

		// Token: 0x0400043E RID: 1086
		private int _currentFrameMovingPartyCount;

		// Token: 0x0400043F RID: 1087
		private int[] _stationaryPartyIndices;

		// Token: 0x04000440 RID: 1088
		private int _currentFrameStationaryPartyCount;

		// Token: 0x04000441 RID: 1089
		private int[] _movingArmyLeaderPartyIndices;

		// Token: 0x04000442 RID: 1090
		private int _currentFrameMovingArmyLeaderCount;

		// Token: 0x04000443 RID: 1091
		private int _currentTotalMobilePartyCapacity;

		// Token: 0x04000444 RID: 1092
		private int _gridChangeCount;

		// Token: 0x04000445 RID: 1093
		private int _exitingSettlementCount;

		// Token: 0x04000446 RID: 1094
		private float _currentDt;

		// Token: 0x04000447 RID: 1095
		private float _currentRealDt;

		// Token: 0x04000448 RID: 1096
		private readonly TWParallel.ParallelForAuxPredicate _parallelInitializeCachedPartyVariablesPredicate;

		// Token: 0x04000449 RID: 1097
		private readonly TWParallel.ParallelForAuxPredicate _parallelCacheTargetPartyVariablesAtFrameStartPredicate;

		// Token: 0x0400044A RID: 1098
		private readonly TWParallel.ParallelForAuxPredicate _parallelArrangePartyIndicesPredicate;

		// Token: 0x0400044B RID: 1099
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickArmiesPredicate;

		// Token: 0x0400044C RID: 1100
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickMovingPartiesPredicate;

		// Token: 0x0400044D RID: 1101
		private readonly TWParallel.ParallelForAuxPredicate _parallelTickStationaryPartiesPredicate;

		// Token: 0x0400044E RID: 1102
		private readonly TWParallel.ParallelForAuxPredicate _parallelCheckExitingSettlementsPredicate;

		// Token: 0x0400044F RID: 1103
		private readonly CampaignTickCacheDataStore.MobilePartyComparer _mobilePartyComparer;

		// Token: 0x020004C0 RID: 1216
		private struct PartyTickCachePerParty
		{
			// Token: 0x04001491 RID: 5265
			internal MobileParty MobileParty;

			// Token: 0x04001492 RID: 5266
			internal MobileParty.CachedPartyVariables LocalVariables;
		}

		// Token: 0x020004C1 RID: 1217
		private class MobilePartyComparer : IComparer<MobileParty>
		{
			// Token: 0x060042EE RID: 17134 RVA: 0x00145328 File Offset: 0x00143528
			public int Compare(MobileParty x, MobileParty y)
			{
				return x.Id.InternalValue.CompareTo(y.Id.InternalValue);
			}
		}
	}
}
