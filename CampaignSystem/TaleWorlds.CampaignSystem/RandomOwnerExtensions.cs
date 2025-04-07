using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008F RID: 143
	public static class RandomOwnerExtensions
	{
		// Token: 0x060010F2 RID: 4338 RVA: 0x0004C26A File Offset: 0x0004A46A
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed)
		{
			return MBRandom.RandomIntWithSeed((uint)obj.RandomValue, seed);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0004C278 File Offset: 0x0004A478
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed, int max)
		{
			return obj.RandomIntWithSeed(seed, 0, max);
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0004C283 File Offset: 0x0004A483
		public static int RandomIntWithSeed(this IRandomOwner obj, uint seed, int min, int max)
		{
			return RandomOwnerExtensions.Random(obj.RandomIntWithSeed(seed), min, max);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0004C293 File Offset: 0x0004A493
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed)
		{
			return MBRandom.RandomFloatWithSeed((uint)obj.RandomValue, seed);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0004C2A1 File Offset: 0x0004A4A1
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed, float max)
		{
			return obj.RandomFloatWithSeed(seed, 0f, max);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0004C2B0 File Offset: 0x0004A4B0
		public static float RandomFloatWithSeed(this IRandomOwner obj, uint seed, float min, float max)
		{
			return RandomOwnerExtensions.Random(obj.RandomFloatWithSeed(seed), min, max);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0004C2C0 File Offset: 0x0004A4C0
		public static int RandomInt(this IRandomOwner obj)
		{
			return obj.RandomValue;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0004C2C8 File Offset: 0x0004A4C8
		public static int RandomInt(this IRandomOwner obj, int max)
		{
			return obj.RandomInt(0, max);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0004C2D2 File Offset: 0x0004A4D2
		public static int RandomInt(this IRandomOwner obj, int min, int max)
		{
			return RandomOwnerExtensions.Random(obj.RandomInt(), min, max);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004C2E1 File Offset: 0x0004A4E1
		public static float RandomFloat(this IRandomOwner obj)
		{
			return (float)obj.RandomValue / 2.1474836E+09f;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0004C2F0 File Offset: 0x0004A4F0
		public static float RandomFloat(this IRandomOwner obj, float max)
		{
			return obj.RandomFloat(0f, max);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0004C2FE File Offset: 0x0004A4FE
		public static float RandomFloat(this IRandomOwner obj, float min, float max)
		{
			return RandomOwnerExtensions.Random(obj.RandomFloat(), min, max);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0004C310 File Offset: 0x0004A510
		private static int Random(int randomValue, int min, int max)
		{
			int num = max - min;
			if (num == 0)
			{
				Debug.FailedAssert("invalid Random parameters", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\IRandomOwner.cs", "Random", 78);
				return 0;
			}
			return min + randomValue % num;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0004C344 File Offset: 0x0004A544
		private static float Random(float randomValue, float min, float max)
		{
			float num = max - min;
			if (num <= 1E-45f)
			{
				Debug.FailedAssert("invalid Random parameters", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\IRandomOwner.cs", "Random", 90);
				return min;
			}
			return min + randomValue * num;
		}
	}
}
