using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000057 RID: 87
	public static class FaceGen
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x000170E8 File Offset: 0x000152E8
		public static void SetInstance(IFaceGen faceGen)
		{
			FaceGen._instance = faceGen;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000170F0 File Offset: 0x000152F0
		public static BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tatooTags)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetRandomBodyProperties(race, isFemale, bodyPropertiesMin, bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tatooTags);
			}
			return bodyPropertiesMin;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001711E File Offset: 0x0001531E
		public static int GetRaceCount()
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return 0;
			}
			return instance.GetRaceCount();
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00017130 File Offset: 0x00015330
		public static int GetRaceOrDefault(string raceId)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return 0;
			}
			return instance.GetRaceOrDefault(raceId);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00017143 File Offset: 0x00015343
		public static string GetBaseMonsterNameFromRace(int race)
		{
			IFaceGen instance = FaceGen._instance;
			return ((instance != null) ? instance.GetBaseMonsterNameFromRace(race) : null) ?? null;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001715C File Offset: 0x0001535C
		public static string[] GetRaceNames()
		{
			IFaceGen instance = FaceGen._instance;
			return ((instance != null) ? instance.GetRaceNames() : null) ?? null;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00017174 File Offset: 0x00015374
		public static Monster GetMonster(string monsterID)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetMonster(monsterID);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00017187 File Offset: 0x00015387
		public static Monster GetMonsterWithSuffix(int race, string suffix)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetMonsterWithSuffix(race, suffix);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001719B File Offset: 0x0001539B
		public static Monster GetBaseMonsterFromRace(int race)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return null;
			}
			return instance.GetBaseMonsterFromRace(race);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x000171AE File Offset: 0x000153AE
		public static void GenerateParentKey(BodyProperties childBodyProperties, int race, ref BodyProperties motherBodyProperties, ref BodyProperties fatherBodyProperties)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.GenerateParentBody(childBodyProperties, race, ref motherBodyProperties, ref fatherBodyProperties);
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x000171C3 File Offset: 0x000153C3
		public static void SetHair(ref BodyProperties bodyProperties, int hair, int beard, int tattoo)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetHair(ref bodyProperties, hair, beard, tattoo);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000171D8 File Offset: 0x000153D8
		public static void SetBody(ref BodyProperties bodyProperties, int build, int weight)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetBody(ref bodyProperties, build, weight);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000171EC File Offset: 0x000153EC
		public static void SetPigmentation(ref BodyProperties bodyProperties, int skinColor, int hairColor, int eyeColor)
		{
			IFaceGen instance = FaceGen._instance;
			if (instance == null)
			{
				return;
			}
			instance.SetPigmentation(ref bodyProperties, skinColor, hairColor, eyeColor);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00017201 File Offset: 0x00015401
		public static BodyProperties GetBodyPropertiesWithAge(ref BodyProperties originalBodyProperties, float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetBodyPropertiesWithAge(ref originalBodyProperties, age);
			}
			return originalBodyProperties;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001721D File Offset: 0x0001541D
		public static BodyMeshMaturityType GetMaturityTypeWithAge(float age)
		{
			if (FaceGen._instance != null)
			{
				return FaceGen._instance.GetMaturityTypeWithAge(age);
			}
			return BodyMeshMaturityType.Child;
		}

		// Token: 0x04000337 RID: 823
		public const string MonsterSuffixSettlement = "_settlement";

		// Token: 0x04000338 RID: 824
		public const string MonsterSuffixSettlementSlow = "_settlement_slow";

		// Token: 0x04000339 RID: 825
		public const string MonsterSuffixSettlementFast = "_settlement_fast";

		// Token: 0x0400033A RID: 826
		public const string MonsterSuffixChild = "_child";

		// Token: 0x0400033B RID: 827
		public static bool ShowDebugValues;

		// Token: 0x0400033C RID: 828
		public static bool UpdateDeformKeys;

		// Token: 0x0400033D RID: 829
		private static IFaceGen _instance;
	}
}
