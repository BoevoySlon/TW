using System;
using System.Globalization;

namespace TaleWorlds.Core
{
	// Token: 0x020000B5 RID: 181
	public class MountCreationKey
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x0001EF12 File Offset: 0x0001D112
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x0001EF1A File Offset: 0x0001D11A
		public byte _leftFrontLegColorIndex { get; private set; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0001EF23 File Offset: 0x0001D123
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x0001EF2B File Offset: 0x0001D12B
		public byte _rightFrontLegColorIndex { get; private set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0001EF34 File Offset: 0x0001D134
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x0001EF3C File Offset: 0x0001D13C
		public byte _leftBackLegColorIndex { get; private set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0001EF45 File Offset: 0x0001D145
		// (set) Token: 0x06000956 RID: 2390 RVA: 0x0001EF4D File Offset: 0x0001D14D
		public byte _rightBackLegColorIndex { get; private set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0001EF56 File Offset: 0x0001D156
		// (set) Token: 0x06000958 RID: 2392 RVA: 0x0001EF5E File Offset: 0x0001D15E
		public byte MaterialIndex { get; private set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0001EF67 File Offset: 0x0001D167
		// (set) Token: 0x0600095A RID: 2394 RVA: 0x0001EF6F File Offset: 0x0001D16F
		public byte MeshMultiplierIndex { get; private set; }

		// Token: 0x0600095B RID: 2395 RVA: 0x0001EF78 File Offset: 0x0001D178
		public MountCreationKey(byte leftFrontLegColorIndex, byte rightFrontLegColorIndex, byte leftBackLegColorIndex, byte rightBackLegColorIndex, byte materialIndex, byte meshMultiplierIndex)
		{
			if (leftFrontLegColorIndex == 3 || rightFrontLegColorIndex == 3)
			{
				leftFrontLegColorIndex = 3;
				rightFrontLegColorIndex = 3;
			}
			this._leftFrontLegColorIndex = leftFrontLegColorIndex;
			this._rightFrontLegColorIndex = rightFrontLegColorIndex;
			this._leftBackLegColorIndex = leftBackLegColorIndex;
			this._rightBackLegColorIndex = rightBackLegColorIndex;
			this.MaterialIndex = materialIndex;
			this.MeshMultiplierIndex = meshMultiplierIndex;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0001EFC8 File Offset: 0x0001D1C8
		public static MountCreationKey FromString(string str)
		{
			if (str != null)
			{
				uint numericKey = uint.Parse(str, NumberStyles.HexNumber);
				int bitsFromKey = MountCreationKey.GetBitsFromKey(numericKey, 0, 2);
				int bitsFromKey2 = MountCreationKey.GetBitsFromKey(numericKey, 2, 2);
				int bitsFromKey3 = MountCreationKey.GetBitsFromKey(numericKey, 4, 2);
				int bitsFromKey4 = MountCreationKey.GetBitsFromKey(numericKey, 6, 2);
				int bitsFromKey5 = MountCreationKey.GetBitsFromKey(numericKey, 8, 2);
				int bitsFromKey6 = MountCreationKey.GetBitsFromKey(numericKey, 10, 2);
				return new MountCreationKey((byte)bitsFromKey, (byte)bitsFromKey2, (byte)bitsFromKey3, (byte)bitsFromKey4, (byte)bitsFromKey5, (byte)bitsFromKey6);
			}
			return new MountCreationKey(0, 0, 0, 0, 0, 0);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001F03C File Offset: 0x0001D23C
		public override string ToString()
		{
			uint num = 0U;
			this.SetBits(ref num, (int)this._leftFrontLegColorIndex, 0);
			this.SetBits(ref num, (int)this._rightFrontLegColorIndex, 2);
			this.SetBits(ref num, (int)this._leftBackLegColorIndex, 4);
			this.SetBits(ref num, (int)this._rightBackLegColorIndex, 6);
			this.SetBits(ref num, (int)this.MaterialIndex, 8);
			this.SetBits(ref num, (int)this.MeshMultiplierIndex, 10);
			return num.ToString("X");
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001F0B4 File Offset: 0x0001D2B4
		private static int GetBitsFromKey(uint numericKey, int startingBit, int numBits)
		{
			int num = (int)(numericKey >> startingBit);
			uint num2 = (uint)(numBits * numBits - 1);
			return num & (int)num2;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001F0D0 File Offset: 0x0001D2D0
		private void SetBits(ref uint numericKey, int value, int startingBit)
		{
			uint num = (uint)((uint)value << startingBit);
			numericKey |= num;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001F0EC File Offset: 0x0001D2EC
		public static string GetRandomMountKeyString(ItemObject mountItem, int randomSeed)
		{
			return MountCreationKey.GetRandomMountKey(mountItem, randomSeed).ToString();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001F0FC File Offset: 0x0001D2FC
		public static MountCreationKey GetRandomMountKey(ItemObject mountItem, int randomSeed)
		{
			MBFastRandom mbfastRandom = new MBFastRandom((uint)randomSeed);
			if (mountItem == null)
			{
				return new MountCreationKey((byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), 0, 0);
			}
			HorseComponent horseComponent = mountItem.HorseComponent;
			if (horseComponent.HorseMaterialNames != null && horseComponent.HorseMaterialNames.Count > 0)
			{
				int num = mbfastRandom.Next(horseComponent.HorseMaterialNames.Count);
				float num2 = mbfastRandom.NextFloat();
				int num3 = 0;
				float num4 = 0f;
				HorseComponent.MaterialProperty materialProperty = horseComponent.HorseMaterialNames[num];
				for (int i = 0; i < materialProperty.MeshMultiplier.Count; i++)
				{
					num4 += materialProperty.MeshMultiplier[i].Item2;
					if (num2 <= num4)
					{
						num3 = i;
						break;
					}
				}
				return new MountCreationKey((byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)num, (byte)num3);
			}
			return new MountCreationKey((byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), (byte)mbfastRandom.Next(4), 0, 0);
		}

		// Token: 0x0400055D RID: 1373
		private const int NumLegColors = 4;
	}
}
