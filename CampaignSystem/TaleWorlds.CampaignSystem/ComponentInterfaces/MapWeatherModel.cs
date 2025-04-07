using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018D RID: 397
	public abstract class MapWeatherModel : GameModel
	{
		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001A32 RID: 6706
		public abstract CampaignTime WeatherUpdateFrequency { get; }

		// Token: 0x06001A33 RID: 6707
		public abstract AtmosphereState GetInterpolatedAtmosphereState(CampaignTime timeOfYear, Vec3 pos);

		// Token: 0x06001A34 RID: 6708
		public abstract AtmosphereInfo GetAtmosphereModel(Vec3 position);

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001A35 RID: 6709
		public abstract CampaignTime WeatherUpdatePeriod { get; }

		// Token: 0x06001A36 RID: 6710
		public abstract void GetSeasonTimeFactorOfCampaignTime(CampaignTime ct, out float timeFactorForSnow, out float timeFactorForRain, bool snapCampaignTimeToWeatherPeriod = true);

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001A37 RID: 6711
		public abstract int DefaultWeatherNodeDimension { get; }

		// Token: 0x06001A38 RID: 6712
		public abstract MapWeatherModel.WeatherEvent UpdateWeatherForPosition(Vec2 position, CampaignTime ct);

		// Token: 0x06001A39 RID: 6713
		public abstract MapWeatherModel.WeatherEvent GetWeatherEventInPosition(Vec2 pos);

		// Token: 0x06001A3A RID: 6714
		public abstract MapWeatherModel.WeatherEventEffectOnTerrain GetWeatherEffectOnTerrainForPosition(Vec2 pos);

		// Token: 0x06001A3B RID: 6715
		public abstract void InitializeSnowAndRainAmountData(byte[] bytes);

		// Token: 0x0200055A RID: 1370
		public enum WeatherEvent
		{
			// Token: 0x0400169B RID: 5787
			Clear,
			// Token: 0x0400169C RID: 5788
			LightRain,
			// Token: 0x0400169D RID: 5789
			HeavyRain,
			// Token: 0x0400169E RID: 5790
			Snowy,
			// Token: 0x0400169F RID: 5791
			Blizzard
		}

		// Token: 0x0200055B RID: 1371
		public enum WeatherEventEffectOnTerrain
		{
			// Token: 0x040016A1 RID: 5793
			Default,
			// Token: 0x040016A2 RID: 5794
			Wet
		}
	}
}
