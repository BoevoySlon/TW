﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200003C RID: 60
	public class MBFastRandom
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x000111D5 File Offset: 0x0000F3D5
		internal static void AutoGeneratedStaticCollectObjectsMBFastRandom(object o, List<object> collectedObjects)
		{
			((MBFastRandom)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000111E3 File Offset: 0x0000F3E3
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x000111E5 File Offset: 0x0000F3E5
		internal static object AutoGeneratedGetMemberValue_x(object o)
		{
			return ((MBFastRandom)o)._x;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000111F7 File Offset: 0x0000F3F7
		internal static object AutoGeneratedGetMemberValue_y(object o)
		{
			return ((MBFastRandom)o)._y;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00011209 File Offset: 0x0000F409
		internal static object AutoGeneratedGetMemberValue_z(object o)
		{
			return ((MBFastRandom)o)._z;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001121B File Offset: 0x0000F41B
		internal static object AutoGeneratedGetMemberValue_w(object o)
		{
			return ((MBFastRandom)o)._w;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001122D File Offset: 0x0000F42D
		public MBFastRandom() : this((uint)Environment.TickCount)
		{
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001123A File Offset: 0x0000F43A
		public MBFastRandom(uint seed)
		{
			MBFastRandom.GenerateState(seed, out this._x, out this._y, out this._z, out this._w);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00011260 File Offset: 0x0000F460
		public void SetSeed(uint seed, uint seed2)
		{
			MBFastRandom.GenerateState(seed, seed2, out this._x, out this._y, out this._z, out this._w);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00011284 File Offset: 0x0000F484
		public int Next()
		{
			uint num;
			do
			{
				num = MBFastRandom.XorShift(ref this._x, ref this._y, ref this._z, ref this._w);
				num &= 2147483647U;
			}
			while (num == 2147483647U);
			return (int)num;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000112C0 File Offset: 0x0000F4C0
		public int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException("maxValue", "Maximum value must be non-negative.");
			}
			uint num = MBFastRandom.XorShift(ref this._x, ref this._y, ref this._z, ref this._w);
			return (int)(4.656612873077393E-10 * (double)(2147483647U & num) * (double)maxValue);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00011318 File Offset: 0x0000F518
		public int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("maxValue", maxValue, "Maximum value must be greater than or equal to minimum value");
			}
			uint num = MBFastRandom.XorShift(ref this._x, ref this._y, ref this._z, ref this._w);
			int num2 = maxValue - minValue;
			if (num2 < 0)
			{
				return minValue + (int)(2.3283064365386963E-10 * num * (double)((long)maxValue - (long)minValue));
			}
			return minValue + (int)(4.656612873077393E-10 * (double)(2147483647U & num) * (double)num2);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00011398 File Offset: 0x0000F598
		public double NextDouble()
		{
			uint num = MBFastRandom.XorShift(ref this._x, ref this._y, ref this._z, ref this._w);
			return 4.656612873077393E-10 * (double)(2147483647U & num);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000113D8 File Offset: 0x0000F5D8
		public float NextFloat()
		{
			uint num = MBFastRandom.XorShift(ref this._x, ref this._y, ref this._z, ref this._w);
			return 5.9604645E-08f * (float)(16777215U & num);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00011414 File Offset: 0x0000F614
		public void NextBytes(byte[] buffer)
		{
			int i = 0;
			int num = buffer.Length - 3;
			while (i < num)
			{
				uint num2 = MBFastRandom.XorShift(ref this._x, ref this._y, ref this._z, ref this._w);
				buffer[i++] = (byte)num2;
				buffer[i++] = (byte)(num2 >> 8);
				buffer[i++] = (byte)(num2 >> 16);
				buffer[i++] = (byte)(num2 >> 24);
			}
			if (i < buffer.Length)
			{
				uint num3 = MBFastRandom.XorShift(ref this._x, ref this._y, ref this._z, ref this._w);
				buffer[i++] = (byte)num3;
				if (i < buffer.Length)
				{
					buffer[i++] = (byte)(num3 >> 8);
					if (i < buffer.Length)
					{
						buffer[i++] = (byte)(num3 >> 16);
						if (i < buffer.Length)
						{
							buffer[i] = (byte)(num3 >> 24);
						}
					}
				}
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x000114D8 File Offset: 0x0000F6D8
		internal static int GetRandomInt(uint seed, uint seed2)
		{
			uint num;
			uint num2;
			uint num3;
			uint num4;
			MBFastRandom.GenerateState(seed, seed2, out num, out num2, out num3, out num4);
			uint num5;
			do
			{
				num5 = MBFastRandom.XorShift(ref num, ref num2, ref num3, ref num4);
				num5 &= 2147483647U;
			}
			while (num5 == 2147483647U);
			return (int)num5;
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00011518 File Offset: 0x0000F718
		internal static float GetRandomFloat(uint seed, uint seed2)
		{
			uint num;
			uint num2;
			uint num3;
			uint num4;
			MBFastRandom.GenerateState(seed, seed2, out num, out num2, out num3, out num4);
			uint num5 = MBFastRandom.XorShift(ref num, ref num2, ref num3, ref num4);
			return 5.9604645E-08f * (float)(16777215U & num5);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00011554 File Offset: 0x0000F754
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint XorShift(ref uint x, ref uint y, ref uint z, ref uint w)
		{
			uint num = x ^ x << 11;
			x = y;
			y = z;
			z = w;
			w = (w ^ w >> 19 ^ (num ^ num >> 8));
			return w;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00011588 File Offset: 0x0000F788
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void GenerateState(uint seed, out uint x, out uint y, out uint z, out uint w)
		{
			x = MBFastRandom.CalculateHashFromSeed(14695981039346656037UL, seed);
			y = MBFastRandom.CalculateHashFromSeed(14695981039346656037UL, x);
			z = MBFastRandom.CalculateHashFromSeed((ulong)x, y);
			w = MBFastRandom.CalculateHashFromSeed((ulong)y, z);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000115C8 File Offset: 0x0000F7C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void GenerateState(uint seed, uint seed2, out uint x, out uint y, out uint z, out uint w)
		{
			uint seed3 = MBFastRandom.CalculateHashFromSeed((ulong)MBFastRandom.CalculateHashFromSeed(14695981039346656037UL, seed), seed2);
			x = MBFastRandom.CalculateHashFromSeed(14695981039346656037UL, seed3);
			y = MBFastRandom.CalculateHashFromSeed(14695981039346656037UL, x);
			z = MBFastRandom.CalculateHashFromSeed((ulong)x, y);
			w = MBFastRandom.CalculateHashFromSeed((ulong)y, z);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001162A File Offset: 0x0000F82A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static uint CalculateHashFromSeed(ulong inputHash, uint seed)
		{
			return (uint)((inputHash ^ (ulong)seed) * 1099511628211UL >> 17);
		}

		// Token: 0x04000245 RID: 581
		private const ulong InitialHash = 14695981039346656037UL;

		// Token: 0x04000246 RID: 582
		private const ulong Prime = 1099511628211UL;

		// Token: 0x04000247 RID: 583
		private const double RealUnitInt = 4.656612873077393E-10;

		// Token: 0x04000248 RID: 584
		private const double RealUnitUint = 2.3283064365386963E-10;

		// Token: 0x04000249 RID: 585
		private const int FloatUnitRangeMax = 16777215;

		// Token: 0x0400024A RID: 586
		private const float FloatUnitInt = 5.9604645E-08f;

		// Token: 0x0400024B RID: 587
		[SaveableField(1)]
		private uint _x;

		// Token: 0x0400024C RID: 588
		[SaveableField(2)]
		private uint _y;

		// Token: 0x0400024D RID: 589
		[SaveableField(3)]
		private uint _z;

		// Token: 0x0400024E RID: 590
		[SaveableField(4)]
		private uint _w;
	}
}
