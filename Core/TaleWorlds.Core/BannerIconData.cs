using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000013 RID: 19
	public struct BannerIconData
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00004780 File Offset: 0x00002980
		// (set) Token: 0x060000DE RID: 222 RVA: 0x00004788 File Offset: 0x00002988
		public string MaterialName { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004791 File Offset: 0x00002991
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004799 File Offset: 0x00002999
		public int TextureIndex { get; private set; }

		// Token: 0x060000E1 RID: 225 RVA: 0x000047A2 File Offset: 0x000029A2
		public BannerIconData(string materialName, int textureIndex)
		{
			this.MaterialName = materialName;
			this.TextureIndex = textureIndex;
		}
	}
}
