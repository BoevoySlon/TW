using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000A9 RID: 169
	public static class MBRandom
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0001C391 File Offset: 0x0001A591
		private static MBFastRandom Random
		{
			get
			{
				return Game.Current.RandomGenerator;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0001C39D File Offset: 0x0001A59D
		public static float RandomFloat
		{
			get
			{
				return MBRandom.Random.NextFloat();
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001C3A9 File Offset: 0x0001A5A9
		public static float RandomFloatRanged(float maxVal)
		{
			return MBRandom.RandomFloat * maxVal;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001C3B2 File Offset: 0x0001A5B2
		public static float RandomFloatRanged(float minVal, float maxVal)
		{
			return minVal + MBRandom.RandomFloat * (maxVal - minVal);
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0001C3C0 File Offset: 0x0001A5C0
		public static float RandomFloatNormal
		{
			get
			{
				int num = 4;
				float num2;
				float num4;
				do
				{
					num2 = 2f * MBRandom.RandomFloat - 1f;
					float num3 = 2f * MBRandom.RandomFloat - 1f;
					num4 = num2 * num2 + num3 * num3;
					num--;
				}
				while (num4 >= 1f || (num4 == 0f && num > 0));
				return num2 * num4 * 1f;
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001C41C File Offset: 0x0001A61C
		public static int RandomInt()
		{
			return MBRandom.Random.Next();
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001C428 File Offset: 0x0001A628
		public static int RandomInt(int maxValue)
		{
			return MBRandom.Random.Next(maxValue);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001C435 File Offset: 0x0001A635
		public static int RandomInt(int minValue, int maxValue)
		{
			return MBRandom.Random.Next(minValue, maxValue);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001C444 File Offset: 0x0001A644
		public static int RoundRandomized(float f)
		{
			int num = MathF.Floor(f);
			float num2 = f - (float)num;
			if (MBRandom.RandomFloat < num2)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001C46C File Offset: 0x0001A66C
		public static T ChooseWeighted<T>(IReadOnlyList<ValueTuple<T, float>> weightList)
		{
			int num;
			return MBRandom.ChooseWeighted<T>(weightList, out num);
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001C484 File Offset: 0x0001A684
		public static T ChooseWeighted<T>(IReadOnlyList<ValueTuple<T, float>> weightList, out int chosenIndex)
		{
			chosenIndex = -1;
			float num = weightList.Sum((ValueTuple<T, float> x) => x.Item2);
			float num2 = MBRandom.RandomFloat * num;
			for (int i = 0; i < weightList.Count; i++)
			{
				num2 -= weightList[i].Item2;
				if (num2 <= 0f)
				{
					chosenIndex = i;
					return weightList[i].Item1;
				}
			}
			if (weightList.Count > 0)
			{
				chosenIndex = 0;
				return weightList[0].Item1;
			}
			chosenIndex = -1;
			return default(T);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001C51F File Offset: 0x0001A71F
		public static void SetSeed(uint seed, uint seed2)
		{
			MBRandom.Random.SetSeed(seed, seed2);
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0001C52D File Offset: 0x0001A72D
		public static float NondeterministicRandomFloat
		{
			get
			{
				return MBRandom.NondeterministicRandom.NextFloat();
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0001C539 File Offset: 0x0001A739
		public static int NondeterministicRandomInt
		{
			get
			{
				return MBRandom.NondeterministicRandom.Next();
			}
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001C545 File Offset: 0x0001A745
		public static int RandomIntWithSeed(uint seed, uint seed2)
		{
			return MBFastRandom.GetRandomInt(seed, seed2);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001C54E File Offset: 0x0001A74E
		public static float RandomFloatWithSeed(uint seed, uint seed2)
		{
			return MBFastRandom.GetRandomFloat(seed, seed2);
		}

		// Token: 0x040004BD RID: 1213
		public const int MaxSeed = 2000;

		// Token: 0x040004BE RID: 1214
		private static readonly MBFastRandom NondeterministicRandom = new MBFastRandom();
	}
}
