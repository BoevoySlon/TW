using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200004A RID: 74
	public class DefaultBannerEffects
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00014270 File Offset: 0x00012470
		private static DefaultBannerEffects Instance
		{
			get
			{
				return Game.Current.DefaultBannerEffects;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0001427C File Offset: 0x0001247C
		public static BannerEffect IncreasedMeleeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMeleeDamage;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00014288 File Offset: 0x00012488
		public static BannerEffect IncreasedMeleeDamageAgainstMountedTroops
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMeleeDamageAgainstMountedTroops;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00014294 File Offset: 0x00012494
		public static BannerEffect IncreasedRangedDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedRangedDamage;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x000142A0 File Offset: 0x000124A0
		public static BannerEffect IncreasedChargeDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedChargeDamage;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x000142AC File Offset: 0x000124AC
		public static BannerEffect DecreasedRangedAccuracyPenalty
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedRangedAccuracyPenalty;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x000142B8 File Offset: 0x000124B8
		public static BannerEffect DecreasedMoraleShock
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedMoraleShock;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x000142C4 File Offset: 0x000124C4
		public static BannerEffect DecreasedMeleeAttackDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedMeleeAttackDamage;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x000142D0 File Offset: 0x000124D0
		public static BannerEffect DecreasedRangedAttackDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedRangedAttackDamage;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x000142DC File Offset: 0x000124DC
		public static BannerEffect DecreasedShieldDamage
		{
			get
			{
				return DefaultBannerEffects.Instance._decreasedShieldDamage;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x000142E8 File Offset: 0x000124E8
		public static BannerEffect IncreasedTroopMovementSpeed
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedTroopMovementSpeed;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x000142F4 File Offset: 0x000124F4
		public static BannerEffect IncreasedMountMovementSpeed
		{
			get
			{
				return DefaultBannerEffects.Instance._increasedMountMovementSpeed;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00014300 File Offset: 0x00012500
		public DefaultBannerEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00014310 File Offset: 0x00012510
		private void RegisterAll()
		{
			this._increasedMeleeDamage = this.Create("IncreasedMeleeDamage");
			this._increasedMeleeDamageAgainstMountedTroops = this.Create("IncreasedMeleeDamageAgainstMountedTroops");
			this._increasedRangedDamage = this.Create("IncreasedRangedDamage");
			this._increasedChargeDamage = this.Create("IncreasedChargeDamage");
			this._decreasedRangedAccuracyPenalty = this.Create("DecreasedRangedAccuracyPenalty");
			this._decreasedMoraleShock = this.Create("DecreasedMoraleShock");
			this._decreasedMeleeAttackDamage = this.Create("DecreasedMeleeAttackDamage");
			this._decreasedRangedAttackDamage = this.Create("DecreasedRangedAttackDamage");
			this._decreasedShieldDamage = this.Create("DecreasedShieldDamage");
			this._increasedTroopMovementSpeed = this.Create("IncreasedTroopMovementSpeed");
			this._increasedMountMovementSpeed = this.Create("IncreasedMountMovementSpeed");
			this.InitializeAll();
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000143DE File Offset: 0x000125DE
		private BannerEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BannerEffect>(new BannerEffect(stringId));
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000143F8 File Offset: 0x000125F8
		private void InitializeAll()
		{
			this._increasedMeleeDamage.Initialize("{=unaWKloT}Increased Melee Damage", "{=8ZNOgT8Z}{BONUS_AMOUNT}% melee damage to troops in your formation.", 0.05f, 0.1f, 0.15f, BannerEffect.EffectIncrementType.AddFactor);
			this._increasedMeleeDamageAgainstMountedTroops.Initialize("{=t0Qzb7CY}Increased Melee Damage Against Mounted Troops", "{=sxGmF0tC}{BONUS_AMOUNT}% melee damage by troops in your formation against cavalry.", 0.1f, 0.2f, 0.3f, BannerEffect.EffectIncrementType.AddFactor);
			this._increasedRangedDamage.Initialize("{=Ch5NpCd0}Increased Ranged Damage", "{=labbKop6}{BONUS_AMOUNT}% ranged damage to troops in your formation.", 0.04f, 0.06f, 0.08f, BannerEffect.EffectIncrementType.AddFactor);
			this._increasedChargeDamage.Initialize("{=O2oBC9sH}Increased Charge Damage", "{=Z2xgnrDa}{BONUS_AMOUNT}% charge damage to mounted troops in your formation.", 0.1f, 0.2f, 0.3f, BannerEffect.EffectIncrementType.AddFactor);
			this._decreasedRangedAccuracyPenalty.Initialize("{=MkBPRCuF}Decreased Ranged Accuracy Penalty", "{=Gu0Wxxul}{BONUS_AMOUNT}% accuracy penalty for ranged troops in your formation.", -0.04f, -0.06f, -0.08f, BannerEffect.EffectIncrementType.AddFactor);
			this._decreasedMoraleShock.Initialize("{=nOMT0Cw6}Decreased Morale Shock", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.1f, -0.2f, -0.3f, BannerEffect.EffectIncrementType.AddFactor);
			this._decreasedMeleeAttackDamage.Initialize("{=a3Vc59WV}Decreased Taken Melee Attack Damage", "{=ORFrCYSn}{BONUS_AMOUNT}% damage by melee attacks to troops in your formation.", -0.05f, -0.1f, -0.15f, BannerEffect.EffectIncrementType.AddFactor);
			this._decreasedRangedAttackDamage.Initialize("{=p0JFbL7G}Decreased Taken Ranged Attack Damage", "{=W0agPHes}{BONUS_AMOUNT}% morale penalty from casualties to troops in your formation.", -0.05f, -0.1f, -0.15f, BannerEffect.EffectIncrementType.AddFactor);
			this._decreasedShieldDamage.Initialize("{=T79exjaP}Decreased Taken Shield Damage", "{=klGEDUmw}{BONUS_AMOUNT}% damage to shields of troops in your formation.", -0.15f, -0.25f, -0.3f, BannerEffect.EffectIncrementType.AddFactor);
			this._increasedTroopMovementSpeed.Initialize("{=PbJAOKKZ}Increased Troop Movement Speed", "{=nqWulUTP}{BONUS_AMOUNT}% movement speed to infantry in your formation.", 0.15f, 0.25f, 0.3f, BannerEffect.EffectIncrementType.AddFactor);
			this._increasedMountMovementSpeed.Initialize("{=nMfxbc0Y}Increased Mount Movement Speed", "{=g0l7W5xQ}{BONUS_AMOUNT}% movement speed to mounts in your formation.", 0.05f, 0.08f, 0.1f, BannerEffect.EffectIncrementType.AddFactor);
		}

		// Token: 0x040002A8 RID: 680
		private BannerEffect _increasedMeleeDamage;

		// Token: 0x040002A9 RID: 681
		private BannerEffect _increasedMeleeDamageAgainstMountedTroops;

		// Token: 0x040002AA RID: 682
		private BannerEffect _increasedRangedDamage;

		// Token: 0x040002AB RID: 683
		private BannerEffect _increasedChargeDamage;

		// Token: 0x040002AC RID: 684
		private BannerEffect _decreasedRangedAccuracyPenalty;

		// Token: 0x040002AD RID: 685
		private BannerEffect _decreasedMoraleShock;

		// Token: 0x040002AE RID: 686
		private BannerEffect _decreasedMeleeAttackDamage;

		// Token: 0x040002AF RID: 687
		private BannerEffect _decreasedRangedAttackDamage;

		// Token: 0x040002B0 RID: 688
		private BannerEffect _decreasedShieldDamage;

		// Token: 0x040002B1 RID: 689
		private BannerEffect _increasedTroopMovementSpeed;

		// Token: 0x040002B2 RID: 690
		private BannerEffect _increasedMountMovementSpeed;
	}
}
