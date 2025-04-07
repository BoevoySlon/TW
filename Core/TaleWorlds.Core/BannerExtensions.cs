using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200000C RID: 12
	public static class BannerExtensions
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00003C14 File Offset: 0x00001E14
		public static bool IsContentsSameWith(this Banner banner, Banner otherBanner)
		{
			if (banner == null && otherBanner == null)
			{
				return true;
			}
			if (banner == null || otherBanner == null)
			{
				return false;
			}
			if (banner.BannerDataList.Count != otherBanner.BannerDataList.Count)
			{
				return false;
			}
			for (int i = 0; i < banner.BannerDataList.Count; i++)
			{
				object obj = banner.BannerDataList[i];
				BannerData obj2 = otherBanner.BannerDataList[i];
				if (!obj.Equals(obj2))
				{
					return false;
				}
			}
			return true;
		}
	}
}
