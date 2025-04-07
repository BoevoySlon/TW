using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000077 RID: 119
	public class DefaultSkillEffects
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x0004731B File Offset: 0x0004551B
		private static DefaultSkillEffects Instance
		{
			get
			{
				return Campaign.Current.DefaultSkillEffects;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x00047327 File Offset: 0x00045527
		public static SkillEffect OneHandedSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectOneHandedSpeed;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x00047333 File Offset: 0x00045533
		public static SkillEffect OneHandedDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectOneHandedDamage;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0004733F File Offset: 0x0004553F
		public static SkillEffect TwoHandedSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTwoHandedSpeed;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000F0A RID: 3850 RVA: 0x0004734B File Offset: 0x0004554B
		public static SkillEffect TwoHandedDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTwoHandedDamage;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00047357 File Offset: 0x00045557
		public static SkillEffect PolearmSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectPolearmSpeed;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000F0C RID: 3852 RVA: 0x00047363 File Offset: 0x00045563
		public static SkillEffect PolearmDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectPolearmDamage;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0004736F File Offset: 0x0004556F
		public static SkillEffect BowLevel
		{
			get
			{
				return DefaultSkillEffects.Instance._effectBowLevel;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0004737B File Offset: 0x0004557B
		public static SkillEffect BowDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectBowDamage;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x00047387 File Offset: 0x00045587
		public static SkillEffect BowAccuracy
		{
			get
			{
				return DefaultSkillEffects.Instance._effectBowAccuracy;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x00047393 File Offset: 0x00045593
		public static SkillEffect ThrowingSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectThrowingSpeed;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0004739F File Offset: 0x0004559F
		public static SkillEffect ThrowingDamage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectThrowingDamage;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x000473AB File Offset: 0x000455AB
		public static SkillEffect ThrowingAccuracy
		{
			get
			{
				return DefaultSkillEffects.Instance._effectThrowingAccuracy;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x000473B7 File Offset: 0x000455B7
		public static SkillEffect CrossbowReloadSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectCrossbowReloadSpeed;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x000473C3 File Offset: 0x000455C3
		public static SkillEffect CrossbowAccuracy
		{
			get
			{
				return DefaultSkillEffects.Instance._effectCrossbowAccuracy;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x000473CF File Offset: 0x000455CF
		public static SkillEffect HorseLevel
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHorseLevel;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000F16 RID: 3862 RVA: 0x000473DB File Offset: 0x000455DB
		public static SkillEffect HorseSpeed
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHorseSpeed;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x000473E7 File Offset: 0x000455E7
		public static SkillEffect HorseManeuver
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHorseManeuver;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000F18 RID: 3864 RVA: 0x000473F3 File Offset: 0x000455F3
		public static SkillEffect MountedWeaponDamagePenalty
		{
			get
			{
				return DefaultSkillEffects.Instance._effectMountedWeaponDamagePenalty;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x000473FF File Offset: 0x000455FF
		public static SkillEffect MountedWeaponSpeedPenalty
		{
			get
			{
				return DefaultSkillEffects.Instance._effectMountedWeaponSpeedPenalty;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x0004740B File Offset: 0x0004560B
		public static SkillEffect DismountResistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectDismountResistance;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x00047417 File Offset: 0x00045617
		public static SkillEffect AthleticsSpeedFactor
		{
			get
			{
				return DefaultSkillEffects.Instance._effectAthleticsSpeedFactor;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x00047423 File Offset: 0x00045623
		public static SkillEffect AthleticsWeightFactor
		{
			get
			{
				return DefaultSkillEffects.Instance._effectAthleticsWeightFactor;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0004742F File Offset: 0x0004562F
		public static SkillEffect KnockBackResistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectKnockBackResistance;
			}
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0004743B File Offset: 0x0004563B
		public static SkillEffect KnockDownResistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectKnockDownResistance;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00047447 File Offset: 0x00045647
		public static SkillEffect SmithingLevel
		{
			get
			{
				return DefaultSkillEffects.Instance._effectSmithingLevel;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x00047453 File Offset: 0x00045653
		public static SkillEffect TacticsAdvantage
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTacticsAdvantage;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0004745F File Offset: 0x0004565F
		public static SkillEffect TacticsTroopSacrificeReduction
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTacticsTroopSacrificeReduction;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0004746B File Offset: 0x0004566B
		public static SkillEffect TrackingRadius
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTrackingRadius;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x00047477 File Offset: 0x00045677
		public static SkillEffect TrackingLevel
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTrackingLevel;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x00047483 File Offset: 0x00045683
		public static SkillEffect TrackingSpottingDistance
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTrackingSpottingDistance;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0004748F File Offset: 0x0004568F
		public static SkillEffect TrackingTrackInformation
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTrackingTrackInformation;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0004749B File Offset: 0x0004569B
		public static SkillEffect RogueryLootBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectRogueryLootBonus;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x000474A7 File Offset: 0x000456A7
		public static SkillEffect CharmRelationBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectCharmRelationBonus;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x000474B3 File Offset: 0x000456B3
		public static SkillEffect TradePenaltyReduction
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTradePenaltyReduction;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x000474BF File Offset: 0x000456BF
		public static SkillEffect SurgeonSurvivalBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectSurgeonSurvivalBonus;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x000474CB File Offset: 0x000456CB
		public static SkillEffect SiegeEngineProductionBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectSiegeEngineProductionBonus;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x000474D7 File Offset: 0x000456D7
		public static SkillEffect TownProjectBuildingBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectTownProjectBuildingBonus;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x000474E3 File Offset: 0x000456E3
		public static SkillEffect HealingRateBonusForHeroes
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHealingRateBonusForHeroes;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x000474EF File Offset: 0x000456EF
		public static SkillEffect HealingRateBonusForRegulars
		{
			get
			{
				return DefaultSkillEffects.Instance._effectHealingRateBonusForRegulars;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x000474FB File Offset: 0x000456FB
		public static SkillEffect GovernorHealingRateBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectGovernorHealingRateBonus;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x00047507 File Offset: 0x00045707
		public static SkillEffect LeadershipMoraleBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectLeadershipMoraleBonus;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x00047513 File Offset: 0x00045713
		public static SkillEffect LeadershipGarrisonSizeBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectLeadershipGarrisonSizeBonus;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x0004751F File Offset: 0x0004571F
		public static SkillEffect StewardPartySizeBonus
		{
			get
			{
				return DefaultSkillEffects.Instance._effectStewardPartySizeBonus;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0004752B File Offset: 0x0004572B
		public static SkillEffect EngineerLevel
		{
			get
			{
				return DefaultSkillEffects.Instance._effectEngineerLevel;
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00047537 File Offset: 0x00045737
		public DefaultSkillEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00047548 File Offset: 0x00045748
		private void RegisterAll()
		{
			this._effectOneHandedSpeed = this.Create("OneHandedSpeed");
			this._effectOneHandedDamage = this.Create("OneHandedDamage");
			this._effectTwoHandedSpeed = this.Create("TwoHandedSpeed");
			this._effectTwoHandedDamage = this.Create("TwoHandedDamage");
			this._effectPolearmSpeed = this.Create("PolearmSpeed");
			this._effectPolearmDamage = this.Create("PolearmDamage");
			this._effectBowLevel = this.Create("BowLevel");
			this._effectBowDamage = this.Create("BowDamage");
			this._effectBowAccuracy = this.Create("BowAccuracy");
			this._effectThrowingSpeed = this.Create("ThrowingSpeed");
			this._effectThrowingDamage = this.Create("ThrowingDamage");
			this._effectThrowingAccuracy = this.Create("ThrowingAccuracy");
			this._effectCrossbowReloadSpeed = this.Create("CrossbowReloadSpeed");
			this._effectCrossbowAccuracy = this.Create("CrossbowAccuracy");
			this._effectHorseLevel = this.Create("HorseLevel");
			this._effectHorseSpeed = this.Create("HorseSpeed");
			this._effectHorseManeuver = this.Create("HorseManeuver");
			this._effectMountedWeaponDamagePenalty = this.Create("MountedWeaponDamagePenalty");
			this._effectMountedWeaponSpeedPenalty = this.Create("MountedWeaponSpeedPenalty");
			this._effectDismountResistance = this.Create("DismountResistance");
			this._effectAthleticsSpeedFactor = this.Create("AthleticsSpeedFactor");
			this._effectAthleticsWeightFactor = this.Create("AthleticsWeightFactor");
			this._effectKnockBackResistance = this.Create("KnockBackResistance");
			this._effectKnockDownResistance = this.Create("KnockDownResistance");
			this._effectSmithingLevel = this.Create("SmithingLevel");
			this._effectTacticsAdvantage = this.Create("TacticsAdvantage");
			this._effectTacticsTroopSacrificeReduction = this.Create("TacticsTroopSacrificeReduction");
			this._effectTrackingRadius = this.Create("TrackingRadius");
			this._effectTrackingLevel = this.Create("TrackingLevel");
			this._effectTrackingSpottingDistance = this.Create("TrackingSpottingDistance");
			this._effectTrackingTrackInformation = this.Create("TrackingTrackInformation");
			this._effectRogueryLootBonus = this.Create("RogueryLootBonus");
			this._effectCharmRelationBonus = this.Create("CharmRelationBonus");
			this._effectTradePenaltyReduction = this.Create("TradePenaltyReduction");
			this._effectLeadershipMoraleBonus = this.Create("LeadershipMoraleBonus");
			this._effectLeadershipGarrisonSizeBonus = this.Create("LeadershipGarrisonSizeBonus");
			this._effectSurgeonSurvivalBonus = this.Create("SurgeonSurvivalBonus");
			this._effectHealingRateBonusForHeroes = this.Create("HealingRateBonusForHeroes");
			this._effectHealingRateBonusForRegulars = this.Create("HealingRateBonusForRegulars");
			this._effectGovernorHealingRateBonus = this.Create("GovernorHealingRateBonus");
			this._effectSiegeEngineProductionBonus = this.Create("SiegeEngineProductionBonus");
			this._effectTownProjectBuildingBonus = this.Create("TownProjectBuildingBonus");
			this._effectStewardPartySizeBonus = this.Create("StewardPartySizeBonus");
			this._effectEngineerLevel = this.Create("EngineerLevel");
			this.InitializeAll();
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00047847 File Offset: 0x00045A47
		private SkillEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<SkillEffect>(new SkillEffect(stringId));
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00047860 File Offset: 0x00045A60
		private void InitializeAll()
		{
			this._effectOneHandedSpeed.Initialize(new TextObject("{=hjxRvb9l}One handed weapon speed: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.OneHanded
			}, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectOneHandedDamage.Initialize(new TextObject("{=baUFKAbd}One handed weapon damage: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.OneHanded
			}, SkillEffect.PerkRole.Personal, 0.15f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectTwoHandedSpeed.Initialize(new TextObject("{=Np94rYMz}Two handed weapon speed: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.TwoHanded
			}, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectTwoHandedDamage.Initialize(new TextObject("{=QkbbLb4v}Two handed weapon damage: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.TwoHanded
			}, SkillEffect.PerkRole.Personal, 0.16f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectPolearmSpeed.Initialize(new TextObject("{=2ATI9qVM}Polearm weapon speed: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Polearm
			}, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectPolearmDamage.Initialize(new TextObject("{=17cIGVQE}Polearm weapon damage: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Polearm
			}, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectBowLevel.Initialize(new TextObject("{=XN7BX0qP}Max usable bow difficulty: {a0}", null), new SkillObject[]
			{
				DefaultSkills.Bow
			}, SkillEffect.PerkRole.Personal, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectBowDamage.Initialize(new TextObject("{=RUZHJMQO}Bow Damage: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Bow
			}, SkillEffect.PerkRole.Personal, 0.11f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectBowAccuracy.Initialize(new TextObject("{=sQCS90Wq}Bow Accuracy: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Bow
			}, SkillEffect.PerkRole.Personal, 0.09f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectThrowingSpeed.Initialize(new TextObject("{=Z0CoeojG}Thrown weapon speed: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Throwing
			}, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectThrowingDamage.Initialize(new TextObject("{=TQMGppEk}Thrown weapon damage: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Throwing
			}, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectThrowingAccuracy.Initialize(new TextObject("{=SfKrjKuO}Thrown weapon accuracy: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Throwing
			}, SkillEffect.PerkRole.Personal, 0.06f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectCrossbowReloadSpeed.Initialize(new TextObject("{=W0Zu4iDz}Crossbow reload speed: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Crossbow
			}, SkillEffect.PerkRole.Personal, 0.07f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectCrossbowAccuracy.Initialize(new TextObject("{=JwWnpD40}Crossbow accuracy: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Crossbow
			}, SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectHorseLevel.Initialize(new TextObject("{=8uBbbwY9}Max mount difficulty: {a0}", null), new SkillObject[]
			{
				DefaultSkills.Riding
			}, SkillEffect.PerkRole.Personal, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectHorseSpeed.Initialize(new TextObject("{=Y07OcP1T}Horse speed: +{a0}", null), new SkillObject[]
			{
				DefaultSkills.Riding
			}, SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectHorseManeuver.Initialize(new TextObject("{=AahNTeXY}Horse maneuver: +{a0}", null), new SkillObject[]
			{
				DefaultSkills.Riding
			}, SkillEffect.PerkRole.Personal, 0.04f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectMountedWeaponDamagePenalty.Initialize(new TextObject("{=0dbwEczK}Mounted weapon damage penalty: {a0}%", null), new SkillObject[]
			{
				DefaultSkills.Riding
			}, SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 20f, 0f);
			this._effectMountedWeaponSpeedPenalty.Initialize(new TextObject("{=oE5etyy0}Mounted weapon speed & reload penalty: {a0}%", null), new SkillObject[]
			{
				DefaultSkills.Riding
			}, SkillEffect.PerkRole.Personal, -0.3f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 30f, 0f);
			this._effectDismountResistance.Initialize(new TextObject("{=kbHJVxAo}Dismount resistance: {a0}% of max. hitpoints", null), new SkillObject[]
			{
				DefaultSkills.Riding
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 40f, 0f);
			this._effectAthleticsSpeedFactor.Initialize(new TextObject("{=rgb6vdon}Running speed increased by {a0}%", null), new SkillObject[]
			{
				DefaultSkills.Athletics
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectAthleticsWeightFactor.Initialize(new TextObject("{=WaUuhxwv}Weight penalty reduced by: {a0}%", null), new SkillObject[]
			{
				DefaultSkills.Athletics
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectKnockBackResistance.Initialize(new TextObject("{=TyjDHQUv}Knock back resistance: {a0}% of max. hitpoints", null), new SkillObject[]
			{
				DefaultSkills.Athletics
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 15f, 0f);
			this._effectKnockDownResistance.Initialize(new TextObject("{=tlNZIH3l}Knock down resistance: {a0}% of max. hitpoints", null), new SkillObject[]
			{
				DefaultSkills.Athletics
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 40f, 0f);
			this._effectSmithingLevel.Initialize(new TextObject("{=ImN8Cfk6}Max difficulty of weapon that can be smithed without penalty: {a0}", null), new SkillObject[]
			{
				DefaultSkills.Crafting
			}, SkillEffect.PerkRole.Personal, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectTacticsAdvantage.Initialize(new TextObject("{=XO3SOlZx}Simulation advantage: +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Tactics
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectTacticsTroopSacrificeReduction.Initialize(new TextObject("{=VHdyQYKI}Decrease the sacrificed troop number when trying to get away +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Tactics
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectTrackingRadius.Initialize(new TextObject("{=kqJipMqc}Track detection radius +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Scouting
			}, SkillEffect.PerkRole.Scout, 0.1f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectTrackingLevel.Initialize(new TextObject("{=aGecGUub}Max track difficulty that can be detected: {a0}", null), new SkillObject[]
			{
				DefaultSkills.Scouting
			}, SkillEffect.PerkRole.Scout, 1f, SkillEffect.PerkRole.None, 1f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectTrackingSpottingDistance.Initialize(new TextObject("{=lbrOAvKj}Spotting distance +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Scouting
			}, SkillEffect.PerkRole.Scout, 0.06f, SkillEffect.PerkRole.None, 0.03f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectTrackingTrackInformation.Initialize(new TextObject("{=uNls3bOP}Track information level: {a0}", null), new SkillObject[]
			{
				DefaultSkills.Scouting
			}, SkillEffect.PerkRole.Scout, 0.04f, SkillEffect.PerkRole.None, 0.03f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectRogueryLootBonus.Initialize(new TextObject("{=bN3bLDb2}Battle Loot +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Roguery
			}, SkillEffect.PerkRole.PartyLeader, 0.25f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectCharmRelationBonus.Initialize(new TextObject("{=c5dsio8Q}Relation increase with NPCs +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Charm
			}, SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectTradePenaltyReduction.Initialize(new TextObject("{=uq7JwT1Z}Trade penalty Reduction +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Trade
			}, SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectLeadershipMoraleBonus.Initialize(new TextObject("{=n3bFiuVu}Increase morale of the parties under your command +{a0}", null), new SkillObject[]
			{
				DefaultSkills.Leadership
			}, SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectLeadershipGarrisonSizeBonus.Initialize(new TextObject("{=cSt26auo}Increase garrison size by +{a0}", null), new SkillObject[]
			{
				DefaultSkills.Leadership
			}, SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectSurgeonSurvivalBonus.Initialize(new TextObject("{=w4BzNJYl}Casualty survival chance +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Medicine
			}, SkillEffect.PerkRole.Surgeon, 0.01f, SkillEffect.PerkRole.None, 0.01f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectHealingRateBonusForHeroes.Initialize(new TextObject("{=fUvs4g40}Healing rate increase for heroes +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Medicine
			}, SkillEffect.PerkRole.Surgeon, 0.5f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectHealingRateBonusForRegulars.Initialize(new TextObject("{=A310vHqJ}Healing rate increase for troops +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Medicine
			}, SkillEffect.PerkRole.Surgeon, 1f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectGovernorHealingRateBonus.Initialize(new TextObject("{=6mQGst9s}Healing rate increase +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Medicine
			}, SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectSiegeEngineProductionBonus.Initialize(new TextObject("{=spbYlf0y}Faster siege engine production +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Engineering
			}, SkillEffect.PerkRole.Engineer, 0.1f, SkillEffect.PerkRole.None, 0.05f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectTownProjectBuildingBonus.Initialize(new TextObject("{=2paRqO8u}Faster building production +{a0}%", null), new SkillObject[]
			{
				DefaultSkills.Engineering
			}, SkillEffect.PerkRole.Governor, 0.25f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.AddFactor, 0f, 0f);
			this._effectStewardPartySizeBonus.Initialize(new TextObject("{=jNDUXetG}Increase party size by +{a0}", null), new SkillObject[]
			{
				DefaultSkills.Steward
			}, SkillEffect.PerkRole.Quartermaster, 0.25f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
			this._effectEngineerLevel.Initialize(new TextObject("{=aQduWCrg}Max difficulty of siege engine that can be built: {a0}", null), new SkillObject[]
			{
				DefaultSkills.Engineering
			}, SkillEffect.PerkRole.Engineer, 1f, SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Add, 0f, 0f);
		}

		// Token: 0x040004E3 RID: 1251
		private SkillEffect _effectOneHandedSpeed;

		// Token: 0x040004E4 RID: 1252
		private SkillEffect _effectOneHandedDamage;

		// Token: 0x040004E5 RID: 1253
		private SkillEffect _effectTwoHandedSpeed;

		// Token: 0x040004E6 RID: 1254
		private SkillEffect _effectTwoHandedDamage;

		// Token: 0x040004E7 RID: 1255
		private SkillEffect _effectPolearmSpeed;

		// Token: 0x040004E8 RID: 1256
		private SkillEffect _effectPolearmDamage;

		// Token: 0x040004E9 RID: 1257
		private SkillEffect _effectBowLevel;

		// Token: 0x040004EA RID: 1258
		private SkillEffect _effectBowDamage;

		// Token: 0x040004EB RID: 1259
		private SkillEffect _effectBowAccuracy;

		// Token: 0x040004EC RID: 1260
		private SkillEffect _effectThrowingSpeed;

		// Token: 0x040004ED RID: 1261
		private SkillEffect _effectThrowingDamage;

		// Token: 0x040004EE RID: 1262
		private SkillEffect _effectThrowingAccuracy;

		// Token: 0x040004EF RID: 1263
		private SkillEffect _effectCrossbowReloadSpeed;

		// Token: 0x040004F0 RID: 1264
		private SkillEffect _effectCrossbowAccuracy;

		// Token: 0x040004F1 RID: 1265
		private SkillEffect _effectHorseLevel;

		// Token: 0x040004F2 RID: 1266
		private SkillEffect _effectHorseSpeed;

		// Token: 0x040004F3 RID: 1267
		private SkillEffect _effectHorseManeuver;

		// Token: 0x040004F4 RID: 1268
		private SkillEffect _effectMountedWeaponDamagePenalty;

		// Token: 0x040004F5 RID: 1269
		private SkillEffect _effectMountedWeaponSpeedPenalty;

		// Token: 0x040004F6 RID: 1270
		private SkillEffect _effectDismountResistance;

		// Token: 0x040004F7 RID: 1271
		private SkillEffect _effectAthleticsSpeedFactor;

		// Token: 0x040004F8 RID: 1272
		private SkillEffect _effectAthleticsWeightFactor;

		// Token: 0x040004F9 RID: 1273
		private SkillEffect _effectKnockBackResistance;

		// Token: 0x040004FA RID: 1274
		private SkillEffect _effectKnockDownResistance;

		// Token: 0x040004FB RID: 1275
		private SkillEffect _effectSmithingLevel;

		// Token: 0x040004FC RID: 1276
		private SkillEffect _effectTacticsAdvantage;

		// Token: 0x040004FD RID: 1277
		private SkillEffect _effectTacticsTroopSacrificeReduction;

		// Token: 0x040004FE RID: 1278
		private SkillEffect _effectTrackingLevel;

		// Token: 0x040004FF RID: 1279
		private SkillEffect _effectTrackingRadius;

		// Token: 0x04000500 RID: 1280
		private SkillEffect _effectTrackingSpottingDistance;

		// Token: 0x04000501 RID: 1281
		private SkillEffect _effectTrackingTrackInformation;

		// Token: 0x04000502 RID: 1282
		private SkillEffect _effectRogueryLootBonus;

		// Token: 0x04000503 RID: 1283
		private SkillEffect _effectCharmRelationBonus;

		// Token: 0x04000504 RID: 1284
		private SkillEffect _effectTradePenaltyReduction;

		// Token: 0x04000505 RID: 1285
		private SkillEffect _effectSurgeonSurvivalBonus;

		// Token: 0x04000506 RID: 1286
		private SkillEffect _effectSiegeEngineProductionBonus;

		// Token: 0x04000507 RID: 1287
		private SkillEffect _effectTownProjectBuildingBonus;

		// Token: 0x04000508 RID: 1288
		private SkillEffect _effectHealingRateBonusForHeroes;

		// Token: 0x04000509 RID: 1289
		private SkillEffect _effectHealingRateBonusForRegulars;

		// Token: 0x0400050A RID: 1290
		private SkillEffect _effectGovernorHealingRateBonus;

		// Token: 0x0400050B RID: 1291
		private SkillEffect _effectLeadershipMoraleBonus;

		// Token: 0x0400050C RID: 1292
		private SkillEffect _effectLeadershipGarrisonSizeBonus;

		// Token: 0x0400050D RID: 1293
		private SkillEffect _effectStewardPartySizeBonus;

		// Token: 0x0400050E RID: 1294
		private SkillEffect _effectEngineerLevel;
	}
}
