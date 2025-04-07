using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200008F RID: 143
	[Flags]
	public enum WeaponFlags : ulong
	{
		// Token: 0x0400041E RID: 1054
		MeleeWeapon = 1UL,
		// Token: 0x0400041F RID: 1055
		RangedWeapon = 2UL,
		// Token: 0x04000420 RID: 1056
		WeaponMask = 3UL,
		// Token: 0x04000421 RID: 1057
		FirearmAmmo = 4UL,
		// Token: 0x04000422 RID: 1058
		NotUsableWithOneHand = 16UL,
		// Token: 0x04000423 RID: 1059
		NotUsableWithTwoHand = 32UL,
		// Token: 0x04000424 RID: 1060
		HandUsageMask = 48UL,
		// Token: 0x04000425 RID: 1061
		WideGrip = 64UL,
		// Token: 0x04000426 RID: 1062
		AttachAmmoToVisual = 128UL,
		// Token: 0x04000427 RID: 1063
		Consumable = 256UL,
		// Token: 0x04000428 RID: 1064
		HasHitPoints = 512UL,
		// Token: 0x04000429 RID: 1065
		DataValueMask = 768UL,
		// Token: 0x0400042A RID: 1066
		HasString = 1024UL,
		// Token: 0x0400042B RID: 1067
		StringHeldByHand = 3072UL,
		// Token: 0x0400042C RID: 1068
		UnloadWhenSheathed = 4096UL,
		// Token: 0x0400042D RID: 1069
		AffectsArea = 8192UL,
		// Token: 0x0400042E RID: 1070
		AffectsAreaBig = 16384UL,
		// Token: 0x0400042F RID: 1071
		Burning = 32768UL,
		// Token: 0x04000430 RID: 1072
		BonusAgainstShield = 65536UL,
		// Token: 0x04000431 RID: 1073
		CanPenetrateShield = 131072UL,
		// Token: 0x04000432 RID: 1074
		CantReloadOnHorseback = 262144UL,
		// Token: 0x04000433 RID: 1075
		AutoReload = 524288UL,
		// Token: 0x04000434 RID: 1076
		CanKillEvenIfBlunt = 1048576UL,
		// Token: 0x04000435 RID: 1077
		TwoHandIdleOnMount = 2097152UL,
		// Token: 0x04000436 RID: 1078
		NoBlood = 4194304UL,
		// Token: 0x04000437 RID: 1079
		PenaltyWithShield = 8388608UL,
		// Token: 0x04000438 RID: 1080
		CanDismount = 16777216UL,
		// Token: 0x04000439 RID: 1081
		CanHook = 33554432UL,
		// Token: 0x0400043A RID: 1082
		CanKnockDown = 67108864UL,
		// Token: 0x0400043B RID: 1083
		CanCrushThrough = 134217728UL,
		// Token: 0x0400043C RID: 1084
		CanBlockRanged = 268435456UL,
		// Token: 0x0400043D RID: 1085
		MissileWithPhysics = 536870912UL,
		// Token: 0x0400043E RID: 1086
		MultiplePenetration = 1073741824UL,
		// Token: 0x0400043F RID: 1087
		LeavesTrail = 2147483648UL,
		// Token: 0x04000440 RID: 1088
		UseHandAsThrowBase = 4294967296UL,
		// Token: 0x04000441 RID: 1089
		AmmoBreaksOnBounceBack = 68719476736UL,
		// Token: 0x04000442 RID: 1090
		AmmoCanBreakOnBounceBack = 137438953472UL,
		// Token: 0x04000443 RID: 1091
		AmmoBreakOnBounceBackMask = 206158430208UL,
		// Token: 0x04000444 RID: 1092
		AmmoSticksWhenShot = 274877906944UL
	}
}
