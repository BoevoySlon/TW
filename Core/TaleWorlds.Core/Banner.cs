using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200000B RID: 11
	public class Banner
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002C18 File Offset: 0x00000E18
		public MBReadOnlyList<BannerData> BannerDataList
		{
			get
			{
				return this._bannerDataList;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00002C20 File Offset: 0x00000E20
		public IBannerVisual BannerVisual
		{
			get
			{
				IBannerVisual result;
				if ((result = this._bannerVisual) == null)
				{
					result = (this._bannerVisual = Game.Current.CreateBannerVisual(this));
				}
				return result;
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002C4B File Offset: 0x00000E4B
		public Banner()
		{
			this._bannerDataList = new MBList<BannerData>();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002C60 File Offset: 0x00000E60
		public Banner(Banner banner)
		{
			this._bannerDataList = new MBList<BannerData>();
			foreach (BannerData bannerData in banner.BannerDataList)
			{
				this._bannerDataList.Add(new BannerData(bannerData));
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002CD0 File Offset: 0x00000ED0
		public Banner(string bannerKey)
		{
			this._bannerDataList = new MBList<BannerData>();
			this.Deserialize(bannerKey);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002CEA File Offset: 0x00000EEA
		public Banner(string bannerKey, uint color1, uint color2)
		{
			this._bannerDataList = new MBList<BannerData>();
			this.Deserialize(bannerKey);
			this.ChangePrimaryColor(color1);
			this.ChangeIconColors(color2);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002D12 File Offset: 0x00000F12
		public void SetBannerVisual(IBannerVisual visual)
		{
			this._bannerVisual = visual;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002D1C File Offset: 0x00000F1C
		public void ChangePrimaryColor(uint mainColor)
		{
			int colorId = BannerManager.GetColorId(mainColor);
			if (colorId < 0)
			{
				return;
			}
			this.BannerDataList[0].ColorId = colorId;
			this.BannerDataList[0].ColorId2 = colorId;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002D5C File Offset: 0x00000F5C
		public void ChangeBackgroundColor(uint primaryColor, uint secondaryColor)
		{
			int colorId = BannerManager.GetColorId(primaryColor);
			int colorId2 = BannerManager.GetColorId(secondaryColor);
			if (colorId < 0)
			{
				return;
			}
			if (colorId2 < 0)
			{
				return;
			}
			this.BannerDataList[0].ColorId = colorId;
			this.BannerDataList[0].ColorId2 = colorId2;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public void ChangeIconColors(uint color)
		{
			int colorId = BannerManager.GetColorId(color);
			if (colorId < 0)
			{
				return;
			}
			for (int i = 1; i < this.BannerDataList.Count; i++)
			{
				this.BannerDataList[i].ColorId = colorId;
				this.BannerDataList[i].ColorId2 = colorId;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002DFC File Offset: 0x00000FFC
		public void RotateBackgroundToRight()
		{
			this.BannerDataList[0].RotationValue -= 0.00278f;
			this.BannerDataList[0].RotationValue = ((this.BannerDataList[0].RotationValue < 0f) ? (this.BannerDataList[0].RotationValue + 1f) : this.BannerDataList[0].RotationValue);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002E7C File Offset: 0x0000107C
		public void RotateBackgroundToLeft()
		{
			this.BannerDataList[0].RotationValue += 0.00278f;
			this.BannerDataList[0].RotationValue = ((this.BannerDataList[0].RotationValue > 0f) ? (this.BannerDataList[0].RotationValue - 1f) : this.BannerDataList[0].RotationValue);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002EF9 File Offset: 0x000010F9
		public void SetBackgroundMeshId(int meshId)
		{
			this.BannerDataList[0].MeshId = meshId;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002F0D File Offset: 0x0000110D
		public string Serialize()
		{
			return Banner.GetBannerCodeFromBannerDataList(this._bannerDataList);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002F1A File Offset: 0x0000111A
		public void Deserialize(string message)
		{
			this._bannerVisual = null;
			this._bannerDataList.Clear();
			this._bannerDataList.AddRange(Banner.GetBannerDataFromBannerCode(message));
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002F40 File Offset: 0x00001140
		public void ClearAllIcons()
		{
			BannerData item = this._bannerDataList[0];
			this._bannerDataList.Clear();
			this._bannerDataList.Add(item);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00002F71 File Offset: 0x00001171
		public void AddIconData(BannerData iconData)
		{
			if (this._bannerDataList.Count < 33)
			{
				this._bannerDataList.Add(iconData);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002F8E File Offset: 0x0000118E
		public void AddIconData(BannerData iconData, int index)
		{
			if (this._bannerDataList.Count < 33 && index > 0 && index <= this._bannerDataList.Count)
			{
				this._bannerDataList.Insert(index, iconData);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002FBE File Offset: 0x000011BE
		public void RemoveIconDataAtIndex(int index)
		{
			if (index > 0 && index < this._bannerDataList.Count)
			{
				this._bannerDataList.RemoveAt(index);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00002FDE File Offset: 0x000011DE
		public static Banner CreateRandomClanBanner(int seed = -1)
		{
			return Banner.CreateRandomBannerInternal(seed, Banner.BannerIconOrientation.CentralPositionedOneIcon);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00002FE7 File Offset: 0x000011E7
		public static Banner CreateRandomBanner()
		{
			return Banner.CreateRandomBannerInternal(-1, Banner.BannerIconOrientation.None);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002FF0 File Offset: 0x000011F0
		private static Banner CreateRandomBannerInternal(int seed = -1, Banner.BannerIconOrientation orientation = Banner.BannerIconOrientation.None)
		{
			Game game = Game.Current;
			MBFastRandom mbfastRandom = (seed == -1) ? new MBFastRandom() : new MBFastRandom((uint)seed);
			Banner banner = new Banner();
			BannerData iconData = new BannerData(BannerManager.Instance.GetRandomBackgroundId(mbfastRandom), mbfastRandom.Next(BannerManager.ColorPalette.Count), mbfastRandom.Next(BannerManager.ColorPalette.Count), new Vec2(1528f, 1528f), new Vec2(764f, 764f), false, false, 0f);
			banner.AddIconData(iconData);
			switch ((orientation == Banner.BannerIconOrientation.None) ? mbfastRandom.Next(6) : ((int)orientation))
			{
			case 0:
				banner.CentralPositionedOneIcon(mbfastRandom);
				break;
			case 1:
				banner.CenteredTwoMirroredIcons(mbfastRandom);
				break;
			case 2:
				banner.DiagonalIcons(mbfastRandom);
				break;
			case 3:
				banner.HorizontalIcons(mbfastRandom);
				break;
			case 4:
				banner.VerticalIcons(mbfastRandom);
				break;
			case 5:
				banner.SquarePositionedFourIcons(mbfastRandom);
				break;
			}
			return banner;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000030DC File Offset: 0x000012DC
		public static Banner CreateOneColoredEmptyBanner(int colorIndex)
		{
			Banner banner = new Banner();
			BannerData iconData = new BannerData(BannerManager.Instance.GetRandomBackgroundId(new MBFastRandom()), colorIndex, colorIndex, new Vec2(1528f, 1528f), new Vec2(764f, 764f), false, false, 0f);
			banner.AddIconData(iconData);
			return banner;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003134 File Offset: 0x00001334
		public static Banner CreateOneColoredBannerWithOneIcon(uint backgroundColor, uint iconColor, int iconMeshId)
		{
			Banner banner = Banner.CreateOneColoredEmptyBanner(BannerManager.GetColorId(backgroundColor));
			if (iconMeshId == -1)
			{
				iconMeshId = BannerManager.Instance.GetRandomBannerIconId(new MBFastRandom());
			}
			banner.AddIconData(new BannerData(iconMeshId, BannerManager.GetColorId(iconColor), BannerManager.GetColorId(iconColor), new Vec2(512f, 512f), new Vec2(764f, 764f), false, false, 0f));
			return banner;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000031A0 File Offset: 0x000013A0
		private void CentralPositionedOneIcon(MBFastRandom random)
		{
			int randomBannerIconId = BannerManager.Instance.GetRandomBannerIconId(random);
			int colorId = random.Next(BannerManager.ColorPalette.Count);
			bool flag = random.NextFloat() < 0.5f;
			int randomColorIdForStroke = this.GetRandomColorIdForStroke(flag, random);
			bool mirror = random.Next(2) == 0;
			float num = random.NextFloat();
			float rotationValue = 0f;
			if (num > 0.9f)
			{
				rotationValue = 0.25f;
			}
			else if (num > 0.8f)
			{
				rotationValue = 0.5f;
			}
			else if (num > 0.7f)
			{
				rotationValue = 0.75f;
			}
			BannerData iconData = new BannerData(randomBannerIconId, colorId, randomColorIdForStroke, new Vec2(512f, 512f), new Vec2(764f, 764f), flag, mirror, rotationValue);
			this.AddIconData(iconData);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003264 File Offset: 0x00001464
		private void DiagonalIcons(MBFastRandom random)
		{
			int num = (random.NextFloat() < 0.5f) ? 2 : 3;
			bool flag = random.NextFloat() < 0.5f;
			int num2 = (512 - 20 * (num + 1)) / num;
			int num3 = BannerManager.Instance.GetRandomBannerIconId(random);
			int num4 = random.Next(BannerManager.ColorPalette.Count);
			bool flag2 = random.NextFloat() < 0.5f;
			int randomColorIdForStroke = this.GetRandomColorIdForStroke(flag2, random);
			int num5 = (512 - num * num2) / (num + 1);
			bool flag3 = random.NextFloat() < 0.3f;
			bool flag4 = flag3 || random.NextFloat() < 0.3f;
			for (int i = 0; i < num; i++)
			{
				num3 = (flag3 ? BannerManager.Instance.GetRandomBannerIconId(random) : num3);
				num4 = (flag4 ? random.Next(BannerManager.ColorPalette.Count) : num4);
				int num6 = i * (num2 + num5) + num5 + num2 / 2;
				int num7 = i * (num2 + num5) + num5 + num2 / 2;
				if (flag)
				{
					num7 = 512 - num7;
				}
				BannerData iconData = new BannerData(num3, num4, randomColorIdForStroke, new Vec2((float)num2, (float)num2), new Vec2((float)(num6 + 508), (float)(num7 + 508)), flag2, false, 0f);
				this.AddIconData(iconData);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000033B8 File Offset: 0x000015B8
		private void HorizontalIcons(MBFastRandom random)
		{
			int num = (random.NextFloat() < 0.5f) ? 2 : 3;
			int num2 = (512 - 20 * (num + 1)) / num;
			int num3 = BannerManager.Instance.GetRandomBannerIconId(random);
			int num4 = random.Next(BannerManager.ColorPalette.Count);
			bool flag = random.NextFloat() < 0.5f;
			int randomColorIdForStroke = this.GetRandomColorIdForStroke(flag, random);
			int num5 = (512 - num * num2) / (num + 1);
			bool flag2 = random.NextFloat() < 0.3f;
			bool flag3 = flag2 || random.NextFloat() < 0.3f;
			for (int i = 0; i < num; i++)
			{
				num3 = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num3);
				num4 = (flag3 ? random.Next(BannerManager.ColorPalette.Count) : num4);
				int num6 = i * (num2 + num5) + num5 + num2 / 2;
				BannerData iconData = new BannerData(num3, num4, randomColorIdForStroke, new Vec2((float)num2, (float)num2), new Vec2((float)(num6 + 508), 764f), flag, false, 0f);
				this.AddIconData(iconData);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000034D4 File Offset: 0x000016D4
		private void VerticalIcons(MBFastRandom random)
		{
			int num = (random.NextFloat() < 0.5f) ? 2 : 3;
			int num2 = (512 - 20 * (num + 1)) / num;
			int num3 = BannerManager.Instance.GetRandomBannerIconId(random);
			int num4 = random.Next(BannerManager.ColorPalette.Count);
			bool flag = random.NextFloat() < 0.5f;
			int randomColorIdForStroke = this.GetRandomColorIdForStroke(flag, random);
			int num5 = (512 - num * num2) / (num + 1);
			bool flag2 = random.NextFloat() < 0.3f;
			bool flag3 = flag2 || random.NextFloat() < 0.3f;
			for (int i = 0; i < num; i++)
			{
				num3 = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num3);
				num4 = (flag3 ? random.Next(BannerManager.ColorPalette.Count) : num4);
				int num6 = i * (num2 + num5) + num5 + num2 / 2;
				BannerData iconData = new BannerData(num3, num4, randomColorIdForStroke, new Vec2((float)num2, (float)num2), new Vec2(764f, (float)(num6 + 508)), flag, false, 0f);
				this.AddIconData(iconData);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000035F0 File Offset: 0x000017F0
		private void SquarePositionedFourIcons(MBFastRandom random)
		{
			bool flag = random.NextFloat() < 0.5f;
			bool flag2 = !flag && random.NextFloat() < 0.5f;
			bool flag3 = flag2 || random.NextFloat() < 0.5f;
			bool flag4 = random.NextFloat() < 0.5f;
			int num = BannerManager.Instance.GetRandomBannerIconId(random);
			int randomColorIdForStroke = this.GetRandomColorIdForStroke(flag4, random);
			int num2 = random.Next(BannerManager.ColorPalette.Count);
			BannerData iconData = new BannerData(num, num2, randomColorIdForStroke, new Vec2(220f, 220f), new Vec2(654f, 654f), flag4, false, 0f);
			this.AddIconData(iconData);
			num = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num);
			num2 = (flag3 ? random.Next(BannerManager.ColorPalette.Count) : num2);
			iconData = new BannerData(num, num2, randomColorIdForStroke, new Vec2(220f, 220f), new Vec2(874f, 654f), flag4, flag, 0f);
			this.AddIconData(iconData);
			num = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num);
			num2 = (flag3 ? random.Next(BannerManager.ColorPalette.Count) : num2);
			iconData = new BannerData(num, num2, randomColorIdForStroke, new Vec2(220f, 220f), new Vec2(654f, 874f), flag4, flag, flag ? 0.5f : 0f);
			this.AddIconData(iconData);
			num = (flag2 ? BannerManager.Instance.GetRandomBannerIconId(random) : num);
			num2 = (flag3 ? random.Next(BannerManager.ColorPalette.Count) : num2);
			iconData = new BannerData(num, num2, randomColorIdForStroke, new Vec2(220f, 220f), new Vec2(874f, 874f), flag4, false, flag ? 0.5f : 0f);
			this.AddIconData(iconData);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000037E0 File Offset: 0x000019E0
		private void CenteredTwoMirroredIcons(MBFastRandom random)
		{
			bool flag = random.NextFloat() < 0.5f;
			bool flag2 = random.NextFloat() < 0.5f;
			int randomBannerIconId = BannerManager.Instance.GetRandomBannerIconId(random);
			int randomColorIdForStroke = this.GetRandomColorIdForStroke(flag2, random);
			int num = random.Next(BannerManager.ColorPalette.Count);
			BannerData iconData = new BannerData(randomBannerIconId, num, randomColorIdForStroke, new Vec2(200f, 200f), new Vec2(664f, 764f), flag2, false, 0f);
			this.AddIconData(iconData);
			num = (flag ? random.Next(BannerManager.ColorPalette.Count) : num);
			iconData = new BannerData(randomBannerIconId, num, randomColorIdForStroke, new Vec2(200f, 200f), new Vec2(864f, 764f), flag2, true, 0f);
			this.AddIconData(iconData);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000038B2 File Offset: 0x00001AB2
		private int GetRandomColorIdForStroke(bool hasStroke, MBFastRandom random)
		{
			if (!hasStroke)
			{
				return BannerManager.ColorPalette.Count - 1;
			}
			return random.Next(BannerManager.ColorPalette.Count);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000038D4 File Offset: 0x00001AD4
		public uint GetPrimaryColor()
		{
			if (this.BannerDataList.Count <= 0)
			{
				return uint.MaxValue;
			}
			return BannerManager.GetColor(this.BannerDataList[0].ColorId);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000038FC File Offset: 0x00001AFC
		public uint GetSecondaryColor()
		{
			if (this.BannerDataList.Count <= 0)
			{
				return uint.MaxValue;
			}
			return BannerManager.GetColor(this.BannerDataList[0].ColorId2);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003924 File Offset: 0x00001B24
		public uint GetFirstIconColor()
		{
			if (this.BannerDataList.Count <= 1)
			{
				return uint.MaxValue;
			}
			return BannerManager.GetColor(this.BannerDataList[1].ColorId);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000394C File Offset: 0x00001B4C
		public int GetVersionNo()
		{
			int num = 0;
			for (int i = 0; i < this._bannerDataList.Count; i++)
			{
				num += this._bannerDataList[i].LocalVersion;
			}
			return num;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003988 File Offset: 0x00001B88
		public static string GetBannerCodeFromBannerDataList(MBList<BannerData> bannerDataList)
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "GetBannerCodeFromBannerDataList");
			bool flag = true;
			foreach (BannerData bannerData in bannerDataList)
			{
				if (!flag)
				{
					mbstringBuilder.Append('.');
				}
				flag = false;
				mbstringBuilder.Append(bannerData.MeshId);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.ColorId);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.ColorId2);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Size.x);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Size.y);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Position.x);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)bannerData.Position.y);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.DrawStroke ? 1 : 0);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append(bannerData.Mirror ? 1 : 0);
				mbstringBuilder.Append('.');
				mbstringBuilder.Append((int)(bannerData.RotationValue / 0.00278f));
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003B20 File Offset: 0x00001D20
		public static List<BannerData> GetBannerDataFromBannerCode(string bannerCode)
		{
			List<BannerData> list = new List<BannerData>();
			string[] array = bannerCode.Split(new char[]
			{
				'.'
			});
			int num = 0;
			while (num + 10 <= array.Length)
			{
				BannerData item = new BannerData(int.Parse(array[num]), int.Parse(array[num + 1]), int.Parse(array[num + 2]), new Vec2((float)int.Parse(array[num + 3]), (float)int.Parse(array[num + 4])), new Vec2((float)int.Parse(array[num + 5]), (float)int.Parse(array[num + 6])), int.Parse(array[num + 7]) == 1, int.Parse(array[num + 8]) == 1, (float)int.Parse(array[num + 9]) * 0.00278f);
				list.Add(item);
				num += 10;
			}
			return list;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003BE9 File Offset: 0x00001DE9
		internal static void AutoGeneratedStaticCollectObjectsBanner(object o, List<object> collectedObjects)
		{
			((Banner)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003BF7 File Offset: 0x00001DF7
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._bannerDataList);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003C05 File Offset: 0x00001E05
		internal static object AutoGeneratedGetMemberValue_bannerDataList(object o)
		{
			return ((Banner)o)._bannerDataList;
		}

		// Token: 0x040000D6 RID: 214
		public const int MaxSize = 8000;

		// Token: 0x040000D7 RID: 215
		public const int BannerFullSize = 1528;

		// Token: 0x040000D8 RID: 216
		public const int BannerEditableAreaSize = 512;

		// Token: 0x040000D9 RID: 217
		public const int MaxIconCount = 32;

		// Token: 0x040000DA RID: 218
		private const char Splitter = '.';

		// Token: 0x040000DB RID: 219
		public const int BackgroundDataIndex = 0;

		// Token: 0x040000DC RID: 220
		public const int BannerIconDataIndex = 1;

		// Token: 0x040000DD RID: 221
		[SaveableField(1)]
		private readonly MBList<BannerData> _bannerDataList;

		// Token: 0x040000DE RID: 222
		[CachedData]
		private IBannerVisual _bannerVisual;

		// Token: 0x020000D3 RID: 211
		private enum BannerIconOrientation
		{
			// Token: 0x04000615 RID: 1557
			None = -1,
			// Token: 0x04000616 RID: 1558
			CentralPositionedOneIcon,
			// Token: 0x04000617 RID: 1559
			CenteredTwoMirroredIcons,
			// Token: 0x04000618 RID: 1560
			DiagonalIcons,
			// Token: 0x04000619 RID: 1561
			HorizontalIcons,
			// Token: 0x0400061A RID: 1562
			VerticalIcons,
			// Token: 0x0400061B RID: 1563
			SquarePositionedFourIcons,
			// Token: 0x0400061C RID: 1564
			NumberOfOrientation
		}
	}
}
