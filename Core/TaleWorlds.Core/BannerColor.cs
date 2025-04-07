using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000015 RID: 21
	public struct BannerColor
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00004F1C File Offset: 0x0000311C
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00004F24 File Offset: 0x00003124
		public uint Color { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00004F2D File Offset: 0x0000312D
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00004F35 File Offset: 0x00003135
		public bool PlayerCanChooseForSigil { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00004F3E File Offset: 0x0000313E
		// (set) Token: 0x060000FA RID: 250 RVA: 0x00004F46 File Offset: 0x00003146
		public bool PlayerCanChooseForBackground { get; private set; }

		// Token: 0x060000FB RID: 251 RVA: 0x00004F4F File Offset: 0x0000314F
		public BannerColor(uint color, bool playerCanChooseForSigil, bool playerCanChooseForBackground)
		{
			this.Color = color;
			this.PlayerCanChooseForSigil = playerCanChooseForSigil;
			this.PlayerCanChooseForBackground = playerCanChooseForBackground;
		}
	}
}
