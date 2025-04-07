using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000006 RID: 6
	public enum DrivenProperty
	{
		// Token: 0x04000042 RID: 66
		None = -1,
		// Token: 0x04000043 RID: 67
		AiRangedHorsebackMissileRange,
		// Token: 0x04000044 RID: 68
		AiFacingMissileWatch,
		// Token: 0x04000045 RID: 69
		AiFlyingMissileCheckRadius,
		// Token: 0x04000046 RID: 70
		AiShootFreq,
		// Token: 0x04000047 RID: 71
		AiWaitBeforeShootFactor,
		// Token: 0x04000048 RID: 72
		AIBlockOnDecideAbility,
		// Token: 0x04000049 RID: 73
		AIParryOnDecideAbility,
		// Token: 0x0400004A RID: 74
		AiTryChamberAttackOnDecide,
		// Token: 0x0400004B RID: 75
		AIAttackOnParryChance,
		// Token: 0x0400004C RID: 76
		AiAttackOnParryTiming,
		// Token: 0x0400004D RID: 77
		AIDecideOnAttackChance,
		// Token: 0x0400004E RID: 78
		AIParryOnAttackAbility,
		// Token: 0x0400004F RID: 79
		AiKick,
		// Token: 0x04000050 RID: 80
		AiAttackCalculationMaxTimeFactor,
		// Token: 0x04000051 RID: 81
		AiDecideOnAttackWhenReceiveHitTiming,
		// Token: 0x04000052 RID: 82
		AiDecideOnAttackContinueAction,
		// Token: 0x04000053 RID: 83
		AiDecideOnAttackingContinue,
		// Token: 0x04000054 RID: 84
		AIParryOnAttackingContinueAbility,
		// Token: 0x04000055 RID: 85
		AIDecideOnRealizeEnemyBlockingAttackAbility,
		// Token: 0x04000056 RID: 86
		AIRealizeBlockingFromIncorrectSideAbility,
		// Token: 0x04000057 RID: 87
		AiAttackingShieldDefenseChance,
		// Token: 0x04000058 RID: 88
		AiAttackingShieldDefenseTimer,
		// Token: 0x04000059 RID: 89
		AiCheckMovementIntervalFactor,
		// Token: 0x0400005A RID: 90
		AiMovementDelayFactor,
		// Token: 0x0400005B RID: 91
		AiParryDecisionChangeValue,
		// Token: 0x0400005C RID: 92
		AiDefendWithShieldDecisionChanceValue,
		// Token: 0x0400005D RID: 93
		AiMoveEnemySideTimeValue,
		// Token: 0x0400005E RID: 94
		AiMinimumDistanceToContinueFactor,
		// Token: 0x0400005F RID: 95
		AiHearingDistanceFactor,
		// Token: 0x04000060 RID: 96
		AiChargeHorsebackTargetDistFactor,
		// Token: 0x04000061 RID: 97
		AiRangerLeadErrorMin,
		// Token: 0x04000062 RID: 98
		AiRangerLeadErrorMax,
		// Token: 0x04000063 RID: 99
		AiRangerVerticalErrorMultiplier,
		// Token: 0x04000064 RID: 100
		AiRangerHorizontalErrorMultiplier,
		// Token: 0x04000065 RID: 101
		AIAttackOnDecideChance,
		// Token: 0x04000066 RID: 102
		AiRaiseShieldDelayTimeBase,
		// Token: 0x04000067 RID: 103
		AiUseShieldAgainstEnemyMissileProbability,
		// Token: 0x04000068 RID: 104
		AiSpeciesIndex,
		// Token: 0x04000069 RID: 105
		AiRandomizedDefendDirectionChance,
		// Token: 0x0400006A RID: 106
		AiShooterError,
		// Token: 0x0400006B RID: 107
		AISetNoAttackTimerAfterBeingHitAbility,
		// Token: 0x0400006C RID: 108
		AISetNoAttackTimerAfterBeingParriedAbility,
		// Token: 0x0400006D RID: 109
		AISetNoDefendTimerAfterHittingAbility,
		// Token: 0x0400006E RID: 110
		AISetNoDefendTimerAfterParryingAbility,
		// Token: 0x0400006F RID: 111
		AIEstimateStunDurationPrecision,
		// Token: 0x04000070 RID: 112
		AIHoldingReadyMaxDuration,
		// Token: 0x04000071 RID: 113
		AIHoldingReadyVariationPercentage,
		// Token: 0x04000072 RID: 114
		MountChargeDamage,
		// Token: 0x04000073 RID: 115
		MountDifficulty,
		// Token: 0x04000074 RID: 116
		ArmorEncumbrance,
		// Token: 0x04000075 RID: 117
		ArmorHead,
		// Token: 0x04000076 RID: 118
		ArmorTorso,
		// Token: 0x04000077 RID: 119
		ArmorLegs,
		// Token: 0x04000078 RID: 120
		ArmorArms,
		// Token: 0x04000079 RID: 121
		UseRealisticBlocking,
		// Token: 0x0400007A RID: 122
		WeaponsEncumbrance,
		// Token: 0x0400007B RID: 123
		SwingSpeedMultiplier,
		// Token: 0x0400007C RID: 124
		ThrustOrRangedReadySpeedMultiplier,
		// Token: 0x0400007D RID: 125
		HandlingMultiplier,
		// Token: 0x0400007E RID: 126
		ReloadSpeed,
		// Token: 0x0400007F RID: 127
		MissileSpeedMultiplier,
		// Token: 0x04000080 RID: 128
		WeaponInaccuracy,
		// Token: 0x04000081 RID: 129
		WeaponWorstMobileAccuracyPenalty,
		// Token: 0x04000082 RID: 130
		WeaponWorstUnsteadyAccuracyPenalty,
		// Token: 0x04000083 RID: 131
		WeaponBestAccuracyWaitTime,
		// Token: 0x04000084 RID: 132
		WeaponUnsteadyBeginTime,
		// Token: 0x04000085 RID: 133
		WeaponUnsteadyEndTime,
		// Token: 0x04000086 RID: 134
		WeaponRotationalAccuracyPenaltyInRadians,
		// Token: 0x04000087 RID: 135
		AttributeRiding,
		// Token: 0x04000088 RID: 136
		AttributeShield,
		// Token: 0x04000089 RID: 137
		AttributeShieldMissileCollisionBodySizeAdder,
		// Token: 0x0400008A RID: 138
		ShieldBashStunDurationMultiplier,
		// Token: 0x0400008B RID: 139
		KickStunDurationMultiplier,
		// Token: 0x0400008C RID: 140
		ReloadMovementPenaltyFactor,
		// Token: 0x0400008D RID: 141
		TopSpeedReachDuration,
		// Token: 0x0400008E RID: 142
		MaxSpeedMultiplier,
		// Token: 0x0400008F RID: 143
		CombatMaxSpeedMultiplier,
		// Token: 0x04000090 RID: 144
		AttributeHorseArchery,
		// Token: 0x04000091 RID: 145
		AttributeCourage,
		// Token: 0x04000092 RID: 146
		MountManeuver,
		// Token: 0x04000093 RID: 147
		MountSpeed,
		// Token: 0x04000094 RID: 148
		MountDashAccelerationMultiplier,
		// Token: 0x04000095 RID: 149
		BipedalRangedReadySpeedMultiplier,
		// Token: 0x04000096 RID: 150
		BipedalRangedReloadSpeedMultiplier,
		// Token: 0x04000097 RID: 151
		Count,
		// Token: 0x04000098 RID: 152
		DrivenPropertiesCalculatedAtSpawnEnd = 55
	}
}
