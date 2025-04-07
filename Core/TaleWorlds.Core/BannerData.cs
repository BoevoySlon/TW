using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000010 RID: 16
	public class BannerData
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003E32 File Offset: 0x00002032
		public int LocalVersion
		{
			get
			{
				return this._localVersion;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003E3A File Offset: 0x0000203A
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003E42 File Offset: 0x00002042
		public int MeshId
		{
			get
			{
				return this._meshId;
			}
			set
			{
				if (value != this._meshId)
				{
					this._meshId = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003E62 File Offset: 0x00002062
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003E6A File Offset: 0x0000206A
		public int ColorId
		{
			get
			{
				return this._colorId;
			}
			set
			{
				if (value != this._colorId)
				{
					this._colorId = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00003E8A File Offset: 0x0000208A
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00003E92 File Offset: 0x00002092
		public int ColorId2
		{
			get
			{
				return this._colorId2;
			}
			set
			{
				if (value != this._colorId2)
				{
					this._colorId2 = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003EB2 File Offset: 0x000020B2
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00003EBA File Offset: 0x000020BA
		public Vec2 Size
		{
			get
			{
				return this._size;
			}
			set
			{
				if (value != this._size)
				{
					this._size = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00003EDF File Offset: 0x000020DF
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00003EE7 File Offset: 0x000020E7
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00003F0C File Offset: 0x0000210C
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00003F14 File Offset: 0x00002114
		public bool DrawStroke
		{
			get
			{
				return this._drawStroke;
			}
			set
			{
				if (value != this._drawStroke)
				{
					this._drawStroke = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003F34 File Offset: 0x00002134
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00003F3C File Offset: 0x0000213C
		public bool Mirror
		{
			get
			{
				return this._mirror;
			}
			set
			{
				if (value != this._mirror)
				{
					this._mirror = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00003F5C File Offset: 0x0000215C
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00003F64 File Offset: 0x00002164
		public float RotationValue
		{
			get
			{
				return this._rotationValue;
			}
			set
			{
				if (value != this._rotationValue)
				{
					this._rotationValue = value;
					this._localVersion++;
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003F84 File Offset: 0x00002184
		public float Rotation
		{
			get
			{
				return 6.2831855f * this.RotationValue;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003F94 File Offset: 0x00002194
		public BannerData(int meshId, int colorId, int colorId2, Vec2 size, Vec2 position, bool drawStroke, bool mirror, float rotationValue)
		{
			this.MeshId = meshId;
			this.ColorId = colorId;
			this.ColorId2 = colorId2;
			this.Size = size;
			this.Position = position;
			this.DrawStroke = drawStroke;
			this.Mirror = mirror;
			this.RotationValue = rotationValue;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003FE4 File Offset: 0x000021E4
		public BannerData(BannerData bannerData) : this(bannerData.MeshId, bannerData.ColorId, bannerData.ColorId2, bannerData.Size, bannerData.Position, bannerData.DrawStroke, bannerData.Mirror, bannerData.RotationValue)
		{
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004028 File Offset: 0x00002228
		public override bool Equals(object obj)
		{
			BannerData bannerData;
			return (bannerData = (obj as BannerData)) != null && bannerData.MeshId == this.MeshId && bannerData.ColorId == this.ColorId && bannerData.ColorId2 == this.ColorId2 && bannerData.Size.X == this.Size.X && bannerData.Size.Y == this.Size.Y && bannerData.Position.X == this.Position.X && bannerData.Position.Y == this.Position.Y && bannerData.DrawStroke == this.DrawStroke && bannerData.Mirror == this.Mirror && bannerData.RotationValue == this.RotationValue;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000411D File Offset: 0x0000231D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004125 File Offset: 0x00002325
		internal static void AutoGeneratedStaticCollectObjectsBannerData(object o, List<object> collectedObjects)
		{
			((BannerData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004133 File Offset: 0x00002333
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004135 File Offset: 0x00002335
		internal static object AutoGeneratedGetMemberValue_colorId2(object o)
		{
			return ((BannerData)o)._colorId2;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004147 File Offset: 0x00002347
		internal static object AutoGeneratedGetMemberValue_size(object o)
		{
			return ((BannerData)o)._size;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004159 File Offset: 0x00002359
		internal static object AutoGeneratedGetMemberValue_position(object o)
		{
			return ((BannerData)o)._position;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000416B File Offset: 0x0000236B
		internal static object AutoGeneratedGetMemberValue_drawStroke(object o)
		{
			return ((BannerData)o)._drawStroke;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000417D File Offset: 0x0000237D
		internal static object AutoGeneratedGetMemberValue_mirror(object o)
		{
			return ((BannerData)o)._mirror;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000418F File Offset: 0x0000238F
		internal static object AutoGeneratedGetMemberValue_rotationValue(object o)
		{
			return ((BannerData)o)._rotationValue;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000041A1 File Offset: 0x000023A1
		internal static object AutoGeneratedGetMemberValue_meshId(object o)
		{
			return ((BannerData)o)._meshId;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000041B3 File Offset: 0x000023B3
		internal static object AutoGeneratedGetMemberValue_colorId(object o)
		{
			return ((BannerData)o)._colorId;
		}

		// Token: 0x040000E2 RID: 226
		public const float RotationPrecision = 0.00278f;

		// Token: 0x040000E3 RID: 227
		[CachedData]
		private int _localVersion;

		// Token: 0x040000E4 RID: 228
		[SaveableField(1)]
		private int _meshId;

		// Token: 0x040000E5 RID: 229
		[SaveableField(2)]
		private int _colorId;

		// Token: 0x040000E6 RID: 230
		[SaveableField(3)]
		public int _colorId2;

		// Token: 0x040000E7 RID: 231
		[SaveableField(4)]
		public Vec2 _size;

		// Token: 0x040000E8 RID: 232
		[SaveableField(5)]
		public Vec2 _position;

		// Token: 0x040000E9 RID: 233
		[SaveableField(6)]
		public bool _drawStroke;

		// Token: 0x040000EA RID: 234
		[SaveableField(7)]
		public bool _mirror;

		// Token: 0x040000EB RID: 235
		[SaveableField(8)]
		public float _rotationValue;
	}
}
