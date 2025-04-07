using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000116 RID: 278
	public class DefaultMapWeatherModel : MapWeatherModel
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001633 RID: 5683 RVA: 0x0006A821 File Offset: 0x00068A21
		public override CampaignTime WeatherUpdatePeriod
		{
			get
			{
				return CampaignTime.Hours(4f);
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001634 RID: 5684 RVA: 0x0006A830 File Offset: 0x00068A30
		public override CampaignTime WeatherUpdateFrequency
		{
			get
			{
				return new CampaignTime(this.WeatherUpdatePeriod.NumTicks / (long)(this.DefaultWeatherNodeDimension * this.DefaultWeatherNodeDimension));
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001635 RID: 5685 RVA: 0x0006A85F File Offset: 0x00068A5F
		public override int DefaultWeatherNodeDimension
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001636 RID: 5686 RVA: 0x0006A863 File Offset: 0x00068A63
		private CampaignTime PreviousRainDataCheckForWetness
		{
			get
			{
				return CampaignTime.Hours(24f);
			}
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0006A870 File Offset: 0x00068A70
		private uint GetSeed(CampaignTime campaignTime, Vec2 position)
		{
			campaignTime += new CampaignTime((long)Campaign.Current.UniqueGameId.GetHashCode());
			int num;
			int num2;
			this.GetNodePositionForWeather(position, out num, out num2);
			uint num3 = (uint)(campaignTime.ToHours / this.WeatherUpdatePeriod.ToHours);
			if (campaignTime.ToSeconds % this.WeatherUpdatePeriod.ToSeconds < this.WeatherUpdateFrequency.ToSeconds * (double)(num * this.DefaultWeatherNodeDimension + num2))
			{
				num3 -= 1U;
			}
			return num3;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0006A91D File Offset: 0x00068B1D
		public override AtmosphereState GetInterpolatedAtmosphereState(CampaignTime timeOfYear, Vec3 pos)
		{
			if (this._atmosphereGrid == null)
			{
				this._atmosphereGrid = new AtmosphereGrid();
				this._atmosphereGrid.Initialize();
			}
			return this._atmosphereGrid.GetInterpolatedStateInfo(pos);
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0006A94C File Offset: 0x00068B4C
		private Vec2 GetNodePositionForWeather(Vec2 pos, out int xIndex, out int yIndex)
		{
			if (Campaign.Current.MapSceneWrapper != null)
			{
				Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
				float num = terrainSize.X / (float)this.DefaultWeatherNodeDimension;
				float num2 = terrainSize.Y / (float)this.DefaultWeatherNodeDimension;
				xIndex = (int)(pos.x / num);
				yIndex = (int)(pos.y / num2);
				float a = (float)xIndex * num;
				float b = (float)yIndex * num2;
				return new Vec2(a, b);
			}
			xIndex = 0;
			yIndex = 0;
			return Vec2.Zero;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0006A9C8 File Offset: 0x00068BC8
		public override AtmosphereInfo GetAtmosphereModel(Vec3 position)
		{
			float hourOfDayNormalized = this.GetHourOfDayNormalized();
			float seasonFactor;
			float num;
			this.GetSeasonTimeFactorOfCampaignTime(CampaignTime.Now, out seasonFactor, out num, true);
			DefaultMapWeatherModel.SunPosition sunPosition = this.GetSunPosition(hourOfDayNormalized, seasonFactor);
			float environmentMultiplier = this.GetEnvironmentMultiplier(sunPosition);
			float num2 = this.GetModifiedEnvironmentMultiplier(environmentMultiplier);
			num2 = MathF.Max(MathF.Pow(num2, 1.5f), 0.001f);
			Vec3 sunColor = this.GetSunColor(environmentMultiplier);
			AtmosphereState interpolatedAtmosphereState = this.GetInterpolatedAtmosphereState(CampaignTime.Now, position);
			float temperature = this.GetTemperature(ref interpolatedAtmosphereState, seasonFactor);
			float humidity = this.GetHumidity(ref interpolatedAtmosphereState, seasonFactor);
			Campaign.Current.Models.MapWeatherModel.UpdateWeatherForPosition(position.AsVec2, CampaignTime.Now);
			ValueTuple<CampaignTime.Seasons, bool, float, float> seasonRainAndSnowDataForOpeningMission = this.GetSeasonRainAndSnowDataForOpeningMission(position.AsVec2);
			CampaignTime.Seasons item = seasonRainAndSnowDataForOpeningMission.Item1;
			bool item2 = seasonRainAndSnowDataForOpeningMission.Item2;
			float item3 = seasonRainAndSnowDataForOpeningMission.Item3;
			float item4 = seasonRainAndSnowDataForOpeningMission.Item4;
			string selectedAtmosphereId = this.GetSelectedAtmosphereId(item, item2, item4, item3);
			AtmosphereInfo result = default(AtmosphereInfo);
			result.SunInfo.Altitude = sunPosition.Altitude;
			result.SunInfo.Angle = sunPosition.Angle;
			result.SunInfo.Color = sunColor;
			result.SunInfo.Brightness = this.GetSunBrightness(environmentMultiplier, false);
			result.SunInfo.Size = this.GetSunSize(environmentMultiplier);
			result.SunInfo.RayStrength = this.GetSunRayStrength(environmentMultiplier);
			result.SunInfo.MaxBrightness = this.GetSunBrightness(1f, true);
			result.RainInfo.Density = item3;
			result.SnowInfo.Density = item4;
			result.AmbientInfo.EnvironmentMultiplier = MathF.Max(num2 * 0.5f, 0.001f);
			result.AmbientInfo.AmbientColor = this.GetAmbientFogColor(num2);
			result.AmbientInfo.MieScatterStrength = this.GetMieScatterStrength(environmentMultiplier);
			result.AmbientInfo.RayleighConstant = this.GetRayleighConstant(environmentMultiplier);
			result.SkyInfo.Brightness = this.GetSkyBrightness(hourOfDayNormalized, environmentMultiplier);
			result.FogInfo.Density = this.GetFogDensity(environmentMultiplier, position);
			result.FogInfo.Color = this.GetFogColor(num2);
			result.FogInfo.Falloff = 1.48f;
			result.TimeInfo.TimeOfDay = this.GetHourOfDay();
			result.TimeInfo.WinterTimeFactor = this.GetWinterTimeFactor(CampaignTime.Now);
			result.TimeInfo.DrynessFactor = this.GetDrynessFactor(CampaignTime.Now);
			result.TimeInfo.NightTimeFactor = this.GetNightTimeFactor();
			result.TimeInfo.Season = (int)item;
			result.AreaInfo.Temperature = temperature;
			result.AreaInfo.Humidity = humidity;
			result.PostProInfo.MinExposure = MBMath.Lerp(-3f, -2f, this.GetExposureCoefficientBetweenDayNight(), 1E-05f);
			result.PostProInfo.MaxExposure = MBMath.Lerp(2f, 0f, num2, 1E-05f);
			result.PostProInfo.BrightpassThreshold = MBMath.Lerp(0.7f, 0.9f, num2, 1E-05f);
			result.PostProInfo.MiddleGray = 0.1f;
			result.InterpolatedAtmosphereName = selectedAtmosphereId;
			return result;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0006AD03 File Offset: 0x00068F03
		public override void InitializeSnowAndRainAmountData(byte[] snowAndRainAmountData)
		{
			this._snowAndRainAmountData = snowAndRainAmountData;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0006AD0C File Offset: 0x00068F0C
		public override MapWeatherModel.WeatherEvent UpdateWeatherForPosition(Vec2 position, CampaignTime ct)
		{
			ValueTuple<float, float> snowAndRainDataFromTexture = this.GetSnowAndRainDataFromTexture(position, ct);
			float item = snowAndRainDataFromTexture.Item1;
			float item2 = snowAndRainDataFromTexture.Item2;
			if (item > 0.55f)
			{
				return this.SetIsBlizzardOrSnowFromFunction(item, ct, position);
			}
			return this.SetIsRainingOrWetFromFunction(item2, ct, position);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0006AD4C File Offset: 0x00068F4C
		private MapWeatherModel.WeatherEvent SetIsBlizzardOrSnowFromFunction(float snowValue, CampaignTime campaignTime, in Vec2 position)
		{
			int num;
			int num2;
			Vec2 nodePositionForWeather = this.GetNodePositionForWeather(position, out num, out num2);
			if (snowValue >= 0.65000004f)
			{
				float frequency = (snowValue - 0.55f) / 0.45f;
				uint seed = this.GetSeed(campaignTime, position);
				bool currentWeatherInAdjustedPosition = this.GetCurrentWeatherInAdjustedPosition(seed, frequency, 0.1f, nodePositionForWeather);
				this._weatherDataCache[num2 * 32 + num] = (currentWeatherInAdjustedPosition ? MapWeatherModel.WeatherEvent.Blizzard : MapWeatherModel.WeatherEvent.Snowy);
			}
			else
			{
				this._weatherDataCache[num2 * 32 + num] = ((snowValue > 0.55f) ? MapWeatherModel.WeatherEvent.Snowy : MapWeatherModel.WeatherEvent.Clear);
			}
			return this._weatherDataCache[num2 * 32 + num];
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x0006ADE0 File Offset: 0x00068FE0
		private MapWeatherModel.WeatherEvent SetIsRainingOrWetFromFunction(float rainValue, CampaignTime campaignTime, in Vec2 position)
		{
			int num;
			int num2;
			Vec2 nodePositionForWeather = this.GetNodePositionForWeather(position, out num, out num2);
			if (rainValue >= 0.6f)
			{
				float frequency = (rainValue - 0.6f) / 0.39999998f;
				uint seed = this.GetSeed(campaignTime, position);
				this._weatherDataCache[num2 * 32 + num] = MapWeatherModel.WeatherEvent.Clear;
				if (this.GetCurrentWeatherInAdjustedPosition(seed, frequency, 0.45f, nodePositionForWeather))
				{
					this._weatherDataCache[num2 * 32 + num] = MapWeatherModel.WeatherEvent.HeavyRain;
				}
				else
				{
					CampaignTime campaignTime2 = new CampaignTime(campaignTime.NumTicks - this.WeatherUpdatePeriod.NumTicks);
					uint seed2 = this.GetSeed(campaignTime2, position);
					float frequency2 = (this.GetSnowAndRainDataFromTexture(position, campaignTime2).Item2 - 0.6f) / 0.39999998f;
					while (campaignTime.NumTicks - campaignTime2.NumTicks < this.PreviousRainDataCheckForWetness.NumTicks)
					{
						if (this.GetCurrentWeatherInAdjustedPosition(seed2, frequency2, 0.45f, nodePositionForWeather))
						{
							this._weatherDataCache[num2 * 32 + num] = MapWeatherModel.WeatherEvent.LightRain;
							break;
						}
						campaignTime2 = new CampaignTime(campaignTime2.NumTicks - this.WeatherUpdatePeriod.NumTicks);
						seed2 = this.GetSeed(campaignTime2, position);
						frequency2 = (this.GetSnowAndRainDataFromTexture(position, campaignTime2).Item2 - 0.6f) / 0.39999998f;
					}
				}
			}
			else
			{
				this._weatherDataCache[num2 * 32 + num] = MapWeatherModel.WeatherEvent.Clear;
			}
			return this._weatherDataCache[num2 * 32 + num];
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0006AF60 File Offset: 0x00069160
		private bool GetCurrentWeatherInAdjustedPosition(uint seed, float frequency, float chanceModifier, in Vec2 adjustedPosition)
		{
			float num = frequency * chanceModifier;
			float mapDiagonal = Campaign.MapDiagonal;
			Vec2 vec = adjustedPosition;
			float num2 = mapDiagonal * vec.X;
			vec = adjustedPosition;
			return num > MBRandom.RandomFloatWithSeed(seed, (uint)(num2 + vec.Y));
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0006AFA0 File Offset: 0x000691A0
		private string GetSelectedAtmosphereId(CampaignTime.Seasons selectedSeason, bool isRaining, float snowValue, float rainValue)
		{
			string result = "semicloudy_field_battle";
			if (Settlement.CurrentSettlement != null && (Settlement.CurrentSettlement.IsFortification || Settlement.CurrentSettlement.IsVillage))
			{
				result = "semicloudy_" + Settlement.CurrentSettlement.Culture.StringId;
			}
			if (selectedSeason == CampaignTime.Seasons.Winter)
			{
				if (snowValue >= 0.85f)
				{
					result = "dense_snowy";
				}
				else
				{
					result = "semi_snowy";
				}
			}
			else
			{
				if (rainValue > 0.6f)
				{
					result = "wet";
				}
				if (isRaining)
				{
					if (rainValue >= 0.85f)
					{
						result = "dense_rainy";
					}
					else
					{
						result = "semi_rainy";
					}
				}
			}
			return result;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0006B034 File Offset: 0x00069234
		private ValueTuple<CampaignTime.Seasons, bool, float, float> GetSeasonRainAndSnowDataForOpeningMission(Vec2 position)
		{
			CampaignTime.Seasons seasons = CampaignTime.Now.GetSeasonOfYear;
			MapWeatherModel.WeatherEvent weatherEventInPosition = this.GetWeatherEventInPosition(position);
			float item = 0f;
			float item2 = 0.85f;
			bool item3 = false;
			switch (weatherEventInPosition)
			{
			case MapWeatherModel.WeatherEvent.Clear:
				if (seasons == CampaignTime.Seasons.Winter)
				{
					seasons = ((CampaignTime.Now.GetDayOfSeason > 10) ? CampaignTime.Seasons.Spring : CampaignTime.Seasons.Autumn);
				}
				break;
			case MapWeatherModel.WeatherEvent.LightRain:
				if (seasons == CampaignTime.Seasons.Winter)
				{
					seasons = ((CampaignTime.Now.GetDayOfSeason > 10) ? CampaignTime.Seasons.Spring : CampaignTime.Seasons.Autumn);
				}
				item = 0.7f;
				break;
			case MapWeatherModel.WeatherEvent.HeavyRain:
				if (seasons == CampaignTime.Seasons.Winter)
				{
					seasons = ((CampaignTime.Now.GetDayOfSeason > 10) ? CampaignTime.Seasons.Spring : CampaignTime.Seasons.Autumn);
				}
				item3 = true;
				item = 0.85f + MBRandom.RandomFloatRanged(0f, 0.14999998f);
				break;
			case MapWeatherModel.WeatherEvent.Snowy:
				seasons = CampaignTime.Seasons.Winter;
				item = 0.55f;
				item2 = 0.55f + MBRandom.RandomFloatRanged(0f, 0.3f);
				break;
			case MapWeatherModel.WeatherEvent.Blizzard:
				seasons = CampaignTime.Seasons.Winter;
				item = 0.85f;
				item2 = 0.85f;
				break;
			}
			return new ValueTuple<CampaignTime.Seasons, bool, float, float>(seasons, item3, item, item2);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0006B13C File Offset: 0x0006933C
		private DefaultMapWeatherModel.SunPosition GetSunPosition(float hourNorm, float seasonFactor)
		{
			float altitude;
			float angle;
			if (hourNorm >= 0.083333336f && hourNorm < 0.9166667f)
			{
				this._sunIsMoon = false;
				float amount = (hourNorm - 0.083333336f) / 0.8333334f;
				altitude = MBMath.Lerp(0f, 180f, amount, 1E-05f);
				angle = 50f * seasonFactor;
			}
			else
			{
				this._sunIsMoon = true;
				if (hourNorm >= 0.9166667f)
				{
					hourNorm -= 1f;
				}
				float num = (hourNorm - -0.08333331f) / 0.16666666f;
				num = ((num < 0f) ? 0f : ((num > 1f) ? 1f : num));
				altitude = MBMath.Lerp(180f, 0f, num, 1E-05f);
				angle = 50f * seasonFactor;
			}
			return new DefaultMapWeatherModel.SunPosition(angle, altitude);
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0006B1FC File Offset: 0x000693FC
		private Vec3 GetSunColor(float environmentMultiplier)
		{
			Vec3 vec;
			if (!this._sunIsMoon)
			{
				vec = new Vec3(1f, 1f - (1f - MathF.Pow(environmentMultiplier, 0.3f)) / 2f, 0.9f - (1f - MathF.Pow(environmentMultiplier, 0.3f)) / 2.5f, -1f);
			}
			else
			{
				vec = new Vec3(0.85f - MathF.Pow(environmentMultiplier, 0.4f), 0.8f - MathF.Pow(environmentMultiplier, 0.5f), 0.8f - MathF.Pow(environmentMultiplier, 0.8f), -1f);
				vec = Vec3.Vec3Max(vec, new Vec3(0.05f, 0.05f, 0.1f, -1f));
			}
			return vec;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0006B2C0 File Offset: 0x000694C0
		private float GetSunBrightness(float environmentMultiplier, bool forceDay = false)
		{
			float num;
			if (!this._sunIsMoon || forceDay)
			{
				num = MathF.Sin(MathF.Pow((environmentMultiplier - 0.001f) / 0.999f, 1.2f) * 1.5707964f) * 85f;
				num = MathF.Min(MathF.Max(num, 0.2f), 35f);
			}
			else
			{
				num = 0.2f;
			}
			return num;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x0006B322 File Offset: 0x00069522
		private float GetSunSize(float envMultiplier)
		{
			return 0.1f + (1f - envMultiplier) / 8f;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0006B338 File Offset: 0x00069538
		private float GetSunRayStrength(float envMultiplier)
		{
			return MathF.Min(MathF.Max(MathF.Sin(MathF.Pow((envMultiplier - 0.001f) / 0.999f, 0.4f) * 3.1415927f / 2f) - 0.15f, 0.01f), 0.5f);
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0006B388 File Offset: 0x00069588
		private float GetEnvironmentMultiplier(DefaultMapWeatherModel.SunPosition sunPos)
		{
			float num;
			if (this._sunIsMoon)
			{
				num = sunPos.Altitude / 180f * 2f;
			}
			else
			{
				num = sunPos.Altitude / 180f * 2f;
			}
			num = ((num > 1f) ? (2f - num) : num);
			num = MathF.Pow(num, 0.5f);
			float num2 = 1f - 0.011111111f * sunPos.Angle;
			float num3 = MBMath.ClampFloat(num * num2, 0f, 1f);
			return MBMath.ClampFloat(MathF.Min(MathF.Sin(num3 * num3) * 2f, 1f), 0f, 1f) * 0.999f + 0.001f;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0006B440 File Offset: 0x00069640
		private float GetModifiedEnvironmentMultiplier(float envMultiplier)
		{
			float num;
			if (!this._sunIsMoon)
			{
				num = (envMultiplier - 0.001f) / 0.999f;
				num = num * 0.999f + 0.001f;
			}
			else
			{
				num = (envMultiplier - 0.001f) / 0.999f;
				num = num * 0f + 0.001f;
			}
			return num;
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0006B490 File Offset: 0x00069690
		private float GetSkyBrightness(float hourNorm, float envMultiplier)
		{
			float x = (envMultiplier - 0.001f) / 0.999f;
			float num;
			if (!this._sunIsMoon)
			{
				num = MathF.Sin(MathF.Pow(x, 1.3f) * 1.5707964f) * 80f;
				num -= 1f;
				num = MathF.Min(MathF.Max(num, 0.055f), 25f);
			}
			else
			{
				num = 0.055f;
			}
			return num;
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0006B500 File Offset: 0x00069700
		private float GetFogDensity(float environmentMultiplier, Vec3 pos)
		{
			float num = this._sunIsMoon ? 0.5f : 0.4f;
			float num2 = 1f - environmentMultiplier;
			float num3 = 1f - MBMath.ClampFloat((pos.z - 30f) / 200f, 0f, 0.9f);
			return MathF.Min((0f + num * num2) * num3, 10f);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0006B568 File Offset: 0x00069768
		private Vec3 GetFogColor(float environmentMultiplier)
		{
			Vec3 vec;
			if (!this._sunIsMoon)
			{
				vec = new Vec3(1f - (1f - environmentMultiplier) / 7f, 0.75f - environmentMultiplier / 4f, 0.55f - environmentMultiplier / 5f, -1f);
			}
			else
			{
				vec = new Vec3(1f - environmentMultiplier * 10f, 0.75f + environmentMultiplier * 1.5f, 0.65f + environmentMultiplier * 2f, -1f);
				vec = Vec3.Vec3Max(vec, new Vec3(0.55f, 0.59f, 0.6f, -1f));
			}
			return vec;
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0006B60C File Offset: 0x0006980C
		private Vec3 GetAmbientFogColor(float moddedEnvMul)
		{
			return Vec3.Vec3Min(new Vec3(0.15f, 0.3f, 0.5f, -1f) + new Vec3(moddedEnvMul / 3f, moddedEnvMul / 2f, moddedEnvMul / 1.5f, -1f), new Vec3(1f, 1f, 1f, -1f));
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0006B674 File Offset: 0x00069874
		private float GetMieScatterStrength(float envMultiplier)
		{
			return (1f + (1f - envMultiplier)) * 10f;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0006B68C File Offset: 0x0006988C
		private float GetRayleighConstant(float envMultiplier)
		{
			float num = (envMultiplier - 0.001f) / 0.999f;
			return MathF.Min(MathF.Max(1f - MathF.Sin(MathF.Pow(num, 0.45f) * 3.1415927f / 2f) + (0.14f + num * 2f), 0.65f), 0.99f);
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0006B6EC File Offset: 0x000698EC
		private float GetHourOfDay()
		{
			return (float)(CampaignTime.Now.ToHours % 24.0);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0006B711 File Offset: 0x00069911
		private float GetHourOfDayNormalized()
		{
			return this.GetHourOfDay() / 24f;
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0006B720 File Offset: 0x00069920
		private float GetNightTimeFactor()
		{
			float num = this.GetHourOfDay() - 2f;
			for (num %= 24f; num < 0f; num += 24f)
			{
			}
			num = MathF.Max(num - 20f, 0f);
			return MathF.Min(num / 0.1f, 1f);
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0006B77C File Offset: 0x0006997C
		private float GetExposureCoefficientBetweenDayNight()
		{
			float hourOfDay = this.GetHourOfDay();
			float result = 0f;
			if (hourOfDay > 2f && hourOfDay < 4f)
			{
				result = 1f - (hourOfDay - 2f) / 2f;
			}
			if (hourOfDay < 22f && hourOfDay > 20f)
			{
				result = (hourOfDay - 20f) / 2f;
			}
			if (hourOfDay > 22f || hourOfDay < 2f)
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0006B7F0 File Offset: 0x000699F0
		private int GetTextureDataIndexForPosition(Vec2 position)
		{
			Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
			int num = MathF.Floor(position.x / terrainSize.X * 1024f);
			int value = MathF.Floor(position.y / terrainSize.Y * 1024f);
			num = MBMath.ClampIndex(num, 0, 1024);
			return MBMath.ClampIndex(value, 0, 1024) * 1024 + num;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0006B860 File Offset: 0x00069A60
		public ValueTuple<float, float> GetSnowAndRainDataFromTexture(Vec2 position, CampaignTime ct)
		{
			int num;
			int num2;
			Vec2 nodePositionForWeather = this.GetNodePositionForWeather(position, out num, out num2);
			int textureDataIndexForPosition = this.GetTextureDataIndexForPosition(position);
			int textureDataIndexForPosition2 = this.GetTextureDataIndexForPosition(nodePositionForWeather);
			byte b = this._snowAndRainAmountData[textureDataIndexForPosition * 2];
			float num3 = (float)this._snowAndRainAmountData[textureDataIndexForPosition2 * 2 + 1];
			float value = (float)b / 255f;
			float value2 = num3 / 255f;
			float amount;
			float amount2;
			Campaign.Current.Models.MapWeatherModel.GetSeasonTimeFactorOfCampaignTime(ct, out amount, out amount2, true);
			float num4 = MBMath.Lerp(0.55f, -0.1f, amount, 1E-05f);
			float num5 = MBMath.Lerp(0.7f, 0.3f, amount2, 1E-05f);
			float num6 = MBMath.SmoothStep(num4 - 0.65f, num4 + 0.65f, value);
			float item = MBMath.SmoothStep(num5 - 0.45f, num5 + 0.45f, value2);
			return new ValueTuple<float, float>(MBMath.Lerp(0f, num6, num6, 1E-05f), item);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0006B94C File Offset: 0x00069B4C
		public override MapWeatherModel.WeatherEvent GetWeatherEventInPosition(Vec2 pos)
		{
			int num;
			int num2;
			this.GetNodePositionForWeather(pos, out num, out num2);
			return this._weatherDataCache[num2 * 32 + num];
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0006B974 File Offset: 0x00069B74
		public override MapWeatherModel.WeatherEventEffectOnTerrain GetWeatherEffectOnTerrainForPosition(Vec2 pos)
		{
			switch (this.GetWeatherEventInPosition(pos))
			{
			case MapWeatherModel.WeatherEvent.Clear:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Default;
			case MapWeatherModel.WeatherEvent.LightRain:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			case MapWeatherModel.WeatherEvent.HeavyRain:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			case MapWeatherModel.WeatherEvent.Snowy:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			case MapWeatherModel.WeatherEvent.Blizzard:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			default:
				return MapWeatherModel.WeatherEventEffectOnTerrain.Default;
			}
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x0006B9B0 File Offset: 0x00069BB0
		private float GetWinterTimeFactor(CampaignTime timeOfYear)
		{
			float result = 0f;
			if (timeOfYear.GetSeasonOfYear == CampaignTime.Seasons.Winter)
			{
				float amount = MathF.Abs((float)Math.IEEERemainder(CampaignTime.Now.ToSeasons, 1.0));
				result = MBMath.SplitLerp(0f, 0.75f, 0f, 0.5f, amount, 1E-05f);
			}
			return result;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0006BA10 File Offset: 0x00069C10
		private float GetDrynessFactor(CampaignTime timeOfYear)
		{
			float result = 0f;
			float num = MathF.Abs((float)Math.IEEERemainder(CampaignTime.Now.ToSeasons, 1.0));
			switch (timeOfYear.GetSeasonOfYear)
			{
			case CampaignTime.Seasons.Summer:
			{
				float amount = MBMath.ClampFloat(num * 2f, 0f, 1f);
				result = MBMath.Lerp(0f, 1f, amount, 1E-05f);
				break;
			}
			case CampaignTime.Seasons.Autumn:
				result = 1f;
				break;
			case CampaignTime.Seasons.Winter:
				result = MBMath.Lerp(1f, 0f, num, 1E-05f);
				break;
			}
			return result;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0006BAB4 File Offset: 0x00069CB4
		public override void GetSeasonTimeFactorOfCampaignTime(CampaignTime ct, out float timeFactorForSnow, out float timeFactorForRain, bool snapCampaignTimeToWeatherPeriod = true)
		{
			if (snapCampaignTimeToWeatherPeriod)
			{
				ct = CampaignTime.Hours((float)((int)(ct.ToHours / this.WeatherUpdatePeriod.ToHours / 2.0) * (int)this.WeatherUpdatePeriod.ToHours * 2));
			}
			float yearProgress = (float)ct.ToSeasons % 4f;
			timeFactorForSnow = this.CalculateTimeFactorForSnow(yearProgress);
			timeFactorForRain = this.CalculateTimeFactorForRain(yearProgress);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0006BB24 File Offset: 0x00069D24
		private float CalculateTimeFactorForSnow(float yearProgress)
		{
			float result = 0f;
			if (yearProgress > 1.5f && (double)yearProgress <= 3.5)
			{
				result = MBMath.Map(yearProgress, 1.5f, 3.5f, 0f, 1f);
			}
			else if (yearProgress <= 1.5f)
			{
				result = MBMath.Map(yearProgress, 0f, 1.5f, 0.75f, 0f);
			}
			else if (yearProgress > 3.5f)
			{
				result = MBMath.Map(yearProgress, 3.5f, 4f, 1f, 0.75f);
			}
			return result;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0006BBB4 File Offset: 0x00069DB4
		private float CalculateTimeFactorForRain(float yearProgress)
		{
			float result = 0f;
			if (yearProgress > 1f && (double)yearProgress <= 2.5)
			{
				result = MBMath.Map(yearProgress, 1f, 2.5f, 0f, 1f);
			}
			else if (yearProgress <= 1f)
			{
				result = MBMath.Map(yearProgress, 0f, 1f, 1f, 0f);
			}
			else if (yearProgress > 2.5f)
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0006BC30 File Offset: 0x00069E30
		private float GetTemperature(ref AtmosphereState gridInfo, float seasonFactor)
		{
			if (gridInfo == null)
			{
				return 0f;
			}
			float temperatureAverage = gridInfo.TemperatureAverage;
			float num = (seasonFactor - 0.5f) * -2f;
			float num2 = gridInfo.TemperatureVariance * num;
			return temperatureAverage + num2;
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0006BC68 File Offset: 0x00069E68
		private float GetHumidity(ref AtmosphereState gridInfo, float seasonFactor)
		{
			if (gridInfo == null)
			{
				return 0f;
			}
			float humidityAverage = gridInfo.HumidityAverage;
			float num = (seasonFactor - 0.5f) * 2f;
			float num2 = gridInfo.HumidityVariance * num;
			return MBMath.ClampFloat(humidityAverage + num2, 0f, 100f);
		}

		// Token: 0x0400079D RID: 1949
		private const float SunRiseNorm = 0.083333336f;

		// Token: 0x0400079E RID: 1950
		private const float SunSetNorm = 0.9166667f;

		// Token: 0x0400079F RID: 1951
		private const float DayTime = 20f;

		// Token: 0x040007A0 RID: 1952
		private const float MinSunAngle = 0f;

		// Token: 0x040007A1 RID: 1953
		private const float MaxSunAngle = 50f;

		// Token: 0x040007A2 RID: 1954
		private const float MinEnvironmentMultiplier = 0.001f;

		// Token: 0x040007A3 RID: 1955
		private const float DayEnvironmentMultiplier = 1f;

		// Token: 0x040007A4 RID: 1956
		private const float NightEnvironmentMultiplier = 0.001f;

		// Token: 0x040007A5 RID: 1957
		private const float SnowStartThreshold = 0.55f;

		// Token: 0x040007A6 RID: 1958
		private const float DenseSnowStartThreshold = 0.85f;

		// Token: 0x040007A7 RID: 1959
		private const float NoSnowDelta = 0.1f;

		// Token: 0x040007A8 RID: 1960
		private const float WetThreshold = 0.6f;

		// Token: 0x040007A9 RID: 1961
		private const float WetThresholdForTexture = 0.3f;

		// Token: 0x040007AA RID: 1962
		private const float LightRainStartThreshold = 0.7f;

		// Token: 0x040007AB RID: 1963
		private const float DenseRainStartThreshold = 0.85f;

		// Token: 0x040007AC RID: 1964
		private const float SnowFrequencyModifier = 0.1f;

		// Token: 0x040007AD RID: 1965
		private const float RainFrequencyModifier = 0.45f;

		// Token: 0x040007AE RID: 1966
		private const float MaxSnowCoverage = 0.75f;

		// Token: 0x040007AF RID: 1967
		private const int SnowAndRainDataTextureDimension = 1024;

		// Token: 0x040007B0 RID: 1968
		private const int WeatherNodeDimension = 32;

		// Token: 0x040007B1 RID: 1969
		private MapWeatherModel.WeatherEvent[] _weatherDataCache = new MapWeatherModel.WeatherEvent[1024];

		// Token: 0x040007B2 RID: 1970
		private AtmosphereGrid _atmosphereGrid;

		// Token: 0x040007B3 RID: 1971
		private byte[] _snowAndRainAmountData = new byte[2097152];

		// Token: 0x040007B4 RID: 1972
		private bool _sunIsMoon;

		// Token: 0x02000509 RID: 1289
		private struct SunPosition
		{
			// Token: 0x17000DA4 RID: 3492
			// (get) Token: 0x060043D9 RID: 17369 RVA: 0x00146D83 File Offset: 0x00144F83
			// (set) Token: 0x060043DA RID: 17370 RVA: 0x00146D8B File Offset: 0x00144F8B
			public float Angle { get; private set; }

			// Token: 0x17000DA5 RID: 3493
			// (get) Token: 0x060043DB RID: 17371 RVA: 0x00146D94 File Offset: 0x00144F94
			// (set) Token: 0x060043DC RID: 17372 RVA: 0x00146D9C File Offset: 0x00144F9C
			public float Altitude { get; private set; }

			// Token: 0x060043DD RID: 17373 RVA: 0x00146DA5 File Offset: 0x00144FA5
			public SunPosition(float angle, float altitude)
			{
				this = default(DefaultMapWeatherModel.SunPosition);
				this.Angle = angle;
				this.Altitude = altitude;
			}
		}
	}
}
