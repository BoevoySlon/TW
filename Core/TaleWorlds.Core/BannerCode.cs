using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200000E RID: 14
	public class BannerCode
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003C85 File Offset: 0x00001E85
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00003C8D File Offset: 0x00001E8D
		public string Code { get; private set; }

		// Token: 0x0600009A RID: 154 RVA: 0x00003C96 File Offset: 0x00001E96
		public Banner CalculateBanner()
		{
			return new Banner(this.Code);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003CA4 File Offset: 0x00001EA4
		public static BannerCode CreateFrom(Banner banner)
		{
			BannerCode bannerCode = new BannerCode();
			if (banner != null)
			{
				bannerCode.Code = banner.Serialize();
			}
			return bannerCode;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003CC7 File Offset: 0x00001EC7
		public static BannerCode CreateFrom(string bannerCodeCode)
		{
			return new BannerCode
			{
				Code = bannerCodeCode
			};
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003CD5 File Offset: 0x00001ED5
		public override int GetHashCode()
		{
			return this.Code.GetHashCode();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003CE2 File Offset: 0x00001EE2
		public override bool Equals(object obj)
		{
			return obj != null && obj is BannerCode && !(((BannerCode)obj).Code != this.Code);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003D0E File Offset: 0x00001F0E
		public static bool operator ==(BannerCode a, BannerCode b)
		{
			return a == b || (a != null && b != null && a.Equals(b));
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003D25 File Offset: 0x00001F25
		public static bool operator !=(BannerCode a, BannerCode b)
		{
			return !(a == b);
		}
	}
}
