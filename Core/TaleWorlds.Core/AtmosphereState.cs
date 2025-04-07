using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x0200000A RID: 10
	public class AtmosphereState
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00002B78 File Offset: 0x00000D78
		public AtmosphereState()
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002BAC File Offset: 0x00000DAC
		public AtmosphereState(Vec3 position, float tempAv, float tempVar, float humAv, float humVar, string colorGradeTexture)
		{
			this.Position = position;
			this.TemperatureAverage = tempAv;
			this.TemperatureVariance = tempVar;
			this.HumidityAverage = humAv;
			this.HumidityVariance = humVar;
			this.ColorGradeTexture = colorGradeTexture;
		}

		// Token: 0x040000CE RID: 206
		public Vec3 Position = Vec3.Zero;

		// Token: 0x040000CF RID: 207
		public float TemperatureAverage;

		// Token: 0x040000D0 RID: 208
		public float TemperatureVariance;

		// Token: 0x040000D1 RID: 209
		public float HumidityAverage;

		// Token: 0x040000D2 RID: 210
		public float HumidityVariance;

		// Token: 0x040000D3 RID: 211
		public float distanceForMaxWeight = 1f;

		// Token: 0x040000D4 RID: 212
		public float distanceForMinWeight = 1f;

		// Token: 0x040000D5 RID: 213
		public string ColorGradeTexture = "";
	}
}
