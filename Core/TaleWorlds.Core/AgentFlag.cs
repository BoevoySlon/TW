using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000007 RID: 7
	[Flags]
	public enum AgentFlag : uint
	{
		// Token: 0x0400009A RID: 154
		None = 0U,
		// Token: 0x0400009B RID: 155
		Mountable = 1U,
		// Token: 0x0400009C RID: 156
		CanJump = 2U,
		// Token: 0x0400009D RID: 157
		CanRear = 4U,
		// Token: 0x0400009E RID: 158
		CanAttack = 8U,
		// Token: 0x0400009F RID: 159
		CanDefend = 16U,
		// Token: 0x040000A0 RID: 160
		RunsAwayWhenHit = 32U,
		// Token: 0x040000A1 RID: 161
		CanCharge = 64U,
		// Token: 0x040000A2 RID: 162
		CanBeCharged = 128U,
		// Token: 0x040000A3 RID: 163
		CanClimbLadders = 256U,
		// Token: 0x040000A4 RID: 164
		CanBeInGroup = 512U,
		// Token: 0x040000A5 RID: 165
		CanSprint = 1024U,
		// Token: 0x040000A6 RID: 166
		IsHumanoid = 2048U,
		// Token: 0x040000A7 RID: 167
		CanGetScared = 4096U,
		// Token: 0x040000A8 RID: 168
		CanRide = 8192U,
		// Token: 0x040000A9 RID: 169
		CanWieldWeapon = 16384U,
		// Token: 0x040000AA RID: 170
		CanCrouch = 32768U,
		// Token: 0x040000AB RID: 171
		CanGetAlarmed = 65536U,
		// Token: 0x040000AC RID: 172
		CanWander = 131072U,
		// Token: 0x040000AD RID: 173
		CanKick = 524288U,
		// Token: 0x040000AE RID: 174
		CanRetreat = 1048576U,
		// Token: 0x040000AF RID: 175
		MoveAsHerd = 2097152U,
		// Token: 0x040000B0 RID: 176
		MoveForwardOnly = 4194304U,
		// Token: 0x040000B1 RID: 177
		IsUnique = 8388608U,
		// Token: 0x040000B2 RID: 178
		CanUseAllBowsMounted = 16777216U,
		// Token: 0x040000B3 RID: 179
		CanReloadAllXBowsMounted = 33554432U,
		// Token: 0x040000B4 RID: 180
		CanDeflectArrowsWith2HSword = 67108864U
	}
}
