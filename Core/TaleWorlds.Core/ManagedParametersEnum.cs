using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000092 RID: 146
	public enum ManagedParametersEnum
	{
		// Token: 0x0400044D RID: 1101
		EnableCampaignTutorials,
		// Token: 0x0400044E RID: 1102
		ReducedMouseSensitivityMultiplier,
		// Token: 0x0400044F RID: 1103
		MeleeAddedElevationForCrosshair,
		// Token: 0x04000450 RID: 1104
		BipedalRadius,
		// Token: 0x04000451 RID: 1105
		QuadrupedalRadius,
		// Token: 0x04000452 RID: 1106
		BipedalCombatSpeedMinMultiplier,
		// Token: 0x04000453 RID: 1107
		BipedalCombatSpeedMaxMultiplier,
		// Token: 0x04000454 RID: 1108
		BipedalRangedReadySpeedMultiplier,
		// Token: 0x04000455 RID: 1109
		BipedalRangedReloadSpeedMultiplier,
		// Token: 0x04000456 RID: 1110
		DamageInterruptAttackThresholdPierce,
		// Token: 0x04000457 RID: 1111
		DamageInterruptAttackThresholdCut,
		// Token: 0x04000458 RID: 1112
		DamageInterruptAttackThresholdBlunt,
		// Token: 0x04000459 RID: 1113
		MakesRearAttackDamageThreshold,
		// Token: 0x0400045A RID: 1114
		MissileMinimumDamageToStick,
		// Token: 0x0400045B RID: 1115
		BreakableProjectileMinimumBreakSpeed,
		// Token: 0x0400045C RID: 1116
		FistFightDamageMultiplier,
		// Token: 0x0400045D RID: 1117
		FallDamageMultiplier,
		// Token: 0x0400045E RID: 1118
		FallDamageAbsorption,
		// Token: 0x0400045F RID: 1119
		FallSpeedReductionMultiplierForRiderDamage,
		// Token: 0x04000460 RID: 1120
		SwingHitWithArmDamageMultiplier,
		// Token: 0x04000461 RID: 1121
		ThrustHitWithArmDamageMultiplier,
		// Token: 0x04000462 RID: 1122
		NonTipThrustHitDamageMultiplier,
		// Token: 0x04000463 RID: 1123
		SwingCombatSpeedGraphZeroProgressValue,
		// Token: 0x04000464 RID: 1124
		SwingCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x04000465 RID: 1125
		SwingCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x04000466 RID: 1126
		SwingCombatSpeedGraphOneProgressValue,
		// Token: 0x04000467 RID: 1127
		OverSwingCombatSpeedGraphZeroProgressValue,
		// Token: 0x04000468 RID: 1128
		OverSwingCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x04000469 RID: 1129
		OverSwingCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x0400046A RID: 1130
		OverSwingCombatSpeedGraphOneProgressValue,
		// Token: 0x0400046B RID: 1131
		ThrustCombatSpeedGraphZeroProgressValue,
		// Token: 0x0400046C RID: 1132
		ThrustCombatSpeedGraphFirstMaximumPoint,
		// Token: 0x0400046D RID: 1133
		ThrustCombatSpeedGraphSecondMaximumPoint,
		// Token: 0x0400046E RID: 1134
		ThrustCombatSpeedGraphOneProgressValue,
		// Token: 0x0400046F RID: 1135
		StunPeriodAttackerSwing,
		// Token: 0x04000470 RID: 1136
		StunPeriodAttackerThrust,
		// Token: 0x04000471 RID: 1137
		StunDefendWeaponWeightOffsetShield,
		// Token: 0x04000472 RID: 1138
		StunDefendWeaponWeightMultiplierWeaponWeight,
		// Token: 0x04000473 RID: 1139
		StunDefendWeaponWeightBonusTwoHanded,
		// Token: 0x04000474 RID: 1140
		StunDefendWeaponWeightBonusPolearm,
		// Token: 0x04000475 RID: 1141
		StunMomentumTransferFactor,
		// Token: 0x04000476 RID: 1142
		StunDefendWeaponWeightParryMultiplier,
		// Token: 0x04000477 RID: 1143
		StunDefendWeaponWeightBonusRightStance,
		// Token: 0x04000478 RID: 1144
		StunDefendWeaponWeightBonusActiveBlocked,
		// Token: 0x04000479 RID: 1145
		StunDefendWeaponWeightBonusChamberBlocked,
		// Token: 0x0400047A RID: 1146
		StunPeriodAttackerFriendlyFire,
		// Token: 0x0400047B RID: 1147
		StunPeriodMax,
		// Token: 0x0400047C RID: 1148
		ProjectileMaxPenetrationSpeed,
		// Token: 0x0400047D RID: 1149
		ObjectMinPenetration,
		// Token: 0x0400047E RID: 1150
		ObjectMaxPenetration,
		// Token: 0x0400047F RID: 1151
		ProjectileMinPenetration,
		// Token: 0x04000480 RID: 1152
		ProjectileMaxPenetration,
		// Token: 0x04000481 RID: 1153
		RotatingProjectileMinPenetration,
		// Token: 0x04000482 RID: 1154
		RotatingProjectileMaxPenetration,
		// Token: 0x04000483 RID: 1155
		ShieldRightStanceBlockDamageMultiplier,
		// Token: 0x04000484 RID: 1156
		ShieldCorrectSideBlockDamageMultiplier,
		// Token: 0x04000485 RID: 1157
		AgentProjectileNormalWeight,
		// Token: 0x04000486 RID: 1158
		ProjectileNormalWeight,
		// Token: 0x04000487 RID: 1159
		ShieldPenetrationOffset,
		// Token: 0x04000488 RID: 1160
		ShieldPenetrationFactor,
		// Token: 0x04000489 RID: 1161
		AirFrictionJavelin,
		// Token: 0x0400048A RID: 1162
		AirFrictionArrow,
		// Token: 0x0400048B RID: 1163
		AirFrictionBallistaBolt,
		// Token: 0x0400048C RID: 1164
		AirFrictionBullet,
		// Token: 0x0400048D RID: 1165
		AirFrictionKnife,
		// Token: 0x0400048E RID: 1166
		AirFrictionAxe,
		// Token: 0x0400048F RID: 1167
		HeavyAttackMomentumMultiplier,
		// Token: 0x04000490 RID: 1168
		ActivateHeroTest,
		// Token: 0x04000491 RID: 1169
		Count
	}
}
