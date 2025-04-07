using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x020000C5 RID: 197
	public interface IMapScene
	{
		// Token: 0x06001292 RID: 4754
		void Load();

		// Token: 0x06001293 RID: 4755
		void Destroy();

		// Token: 0x06001294 RID: 4756
		PathFaceRecord GetFaceIndex(Vec2 position);

		// Token: 0x06001295 RID: 4757
		bool AreFacesOnSameIsland(PathFaceRecord startingFace, PathFaceRecord endFace, bool ignoreDisabled);

		// Token: 0x06001296 RID: 4758
		TerrainType GetTerrainTypeAtPosition(Vec2 position);

		// Token: 0x06001297 RID: 4759
		List<TerrainType> GetEnvironmentTerrainTypes(Vec2 position);

		// Token: 0x06001298 RID: 4760
		List<TerrainType> GetEnvironmentTerrainTypesCount(Vec2 position, out TerrainType currentPositionTerrainType);

		// Token: 0x06001299 RID: 4761
		MapPatchData GetMapPatchAtPosition(Vec2 position);

		// Token: 0x0600129A RID: 4762
		TerrainType GetFaceTerrainType(PathFaceRecord faceIndex);

		// Token: 0x0600129B RID: 4763
		Vec2 GetAccessiblePointNearPosition(Vec2 position, float radius);

		// Token: 0x0600129C RID: 4764
		bool GetPathBetweenAIFaces(PathFaceRecord startingFace, PathFaceRecord endingFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, NavigationPath path, int[] excludedFaceIds = null);

		// Token: 0x0600129D RID: 4765
		bool GetPathDistanceBetweenAIFaces(PathFaceRecord startingAiFace, PathFaceRecord endingAiFace, Vec2 startingPosition, Vec2 endingPosition, float agentRadius, float distanceLimit, out float distance);

		// Token: 0x0600129E RID: 4766
		bool IsLineToPointClear(PathFaceRecord startingFace, Vec2 position, Vec2 destination, float agentRadius);

		// Token: 0x0600129F RID: 4767
		Vec2 GetLastPointOnNavigationMeshFromPositionToDestination(PathFaceRecord startingFace, Vec2 position, Vec2 destination);

		// Token: 0x060012A0 RID: 4768
		Vec2 GetNavigationMeshCenterPosition(PathFaceRecord face);

		// Token: 0x060012A1 RID: 4769
		int GetNumberOfNavigationMeshFaces();

		// Token: 0x060012A2 RID: 4770
		bool GetHeightAtPoint(Vec2 point, ref float height);

		// Token: 0x060012A3 RID: 4771
		float GetWinterTimeFactor();

		// Token: 0x060012A4 RID: 4772
		void GetTerrainHeightAndNormal(Vec2 position, out float height, out Vec3 normal);

		// Token: 0x060012A5 RID: 4773
		float GetFaceVertexZ(PathFaceRecord navMeshFace);

		// Token: 0x060012A6 RID: 4774
		Vec3 GetGroundNormal(Vec2 position);

		// Token: 0x060012A7 RID: 4775
		string GetTerrainTypeName(TerrainType type);

		// Token: 0x060012A8 RID: 4776
		Vec2 GetTerrainSize();

		// Token: 0x060012A9 RID: 4777
		uint GetSceneLevel(string name);

		// Token: 0x060012AA RID: 4778
		void SetSceneLevels(List<string> levels);

		// Token: 0x060012AB RID: 4779
		List<AtmosphereState> GetAtmosphereStates();

		// Token: 0x060012AC RID: 4780
		void SetAtmosphereColorgrade(TerrainType terrainType);

		// Token: 0x060012AD RID: 4781
		void AddNewEntityToMapScene(string entityId, Vec2 position);

		// Token: 0x060012AE RID: 4782
		void GetFaceIndexForMultiplePositions(int movedPartyCount, float[] positionArray, PathFaceRecord[] resultArray);

		// Token: 0x060012AF RID: 4783
		void GetMapBorders(out Vec2 minimumPosition, out Vec2 maximumPosition, out float maximumHeight);

		// Token: 0x060012B0 RID: 4784
		void GetSnowAmountData(byte[] snowData);
	}
}
