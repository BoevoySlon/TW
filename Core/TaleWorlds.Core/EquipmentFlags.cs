using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000096 RID: 150
	[Flags]
	public enum EquipmentFlags : uint
	{
		// Token: 0x04000498 RID: 1176
		None = 0U,
		// Token: 0x04000499 RID: 1177
		IsWandererEquipment = 1U,
		// Token: 0x0400049A RID: 1178
		IsGentryEquipment = 2U,
		// Token: 0x0400049B RID: 1179
		IsRebelHeroEquipment = 4U,
		// Token: 0x0400049C RID: 1180
		IsNoncombatantTemplate = 8U,
		// Token: 0x0400049D RID: 1181
		IsCombatantTemplate = 16U,
		// Token: 0x0400049E RID: 1182
		IsCivilianTemplate = 32U,
		// Token: 0x0400049F RID: 1183
		IsNobleTemplate = 64U,
		// Token: 0x040004A0 RID: 1184
		IsFemaleTemplate = 128U,
		// Token: 0x040004A1 RID: 1185
		IsMediumTemplate = 256U,
		// Token: 0x040004A2 RID: 1186
		IsHeavyTemplate = 512U,
		// Token: 0x040004A3 RID: 1187
		IsFlamboyantTemplate = 1024U,
		// Token: 0x040004A4 RID: 1188
		IsStoicTemplate = 2048U,
		// Token: 0x040004A5 RID: 1189
		IsNomadTemplate = 4096U,
		// Token: 0x040004A6 RID: 1190
		IsWoodlandTemplate = 8192U,
		// Token: 0x040004A7 RID: 1191
		IsChildEquipmentTemplate = 16384U,
		// Token: 0x040004A8 RID: 1192
		IsTeenagerEquipmentTemplate = 32768U
	}
}
