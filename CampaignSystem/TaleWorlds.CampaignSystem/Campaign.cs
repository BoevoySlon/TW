using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000028 RID: 40
	public class Campaign : GameType
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000F25C File Offset: 0x0000D45C
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000F263 File Offset: 0x0000D463
		public static float MapDiagonal { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000F26B File Offset: 0x0000D46B
		// (set) Token: 0x06000170 RID: 368 RVA: 0x0000F272 File Offset: 0x0000D472
		public static float MapDiagonalSquared { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000171 RID: 369 RVA: 0x0000F27A File Offset: 0x0000D47A
		// (set) Token: 0x06000172 RID: 370 RVA: 0x0000F281 File Offset: 0x0000D481
		public static float MaximumDistanceBetweenTwoSettlements { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000F289 File Offset: 0x0000D489
		// (set) Token: 0x06000174 RID: 372 RVA: 0x0000F290 File Offset: 0x0000D490
		public static Vec2 MapMinimumPosition { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000F298 File Offset: 0x0000D498
		// (set) Token: 0x06000176 RID: 374 RVA: 0x0000F29F File Offset: 0x0000D49F
		public static Vec2 MapMaximumPosition { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000F2A7 File Offset: 0x0000D4A7
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000F2AE File Offset: 0x0000D4AE
		public static float MapMaximumHeight { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000F2B6 File Offset: 0x0000D4B6
		// (set) Token: 0x0600017A RID: 378 RVA: 0x0000F2BD File Offset: 0x0000D4BD
		public static float AverageDistanceBetweenTwoFortifications { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000F2C5 File Offset: 0x0000D4C5
		// (set) Token: 0x0600017C RID: 380 RVA: 0x0000F2CD File Offset: 0x0000D4CD
		[CachedData]
		public float AverageWage { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000F2D6 File Offset: 0x0000D4D6
		public string NewGameVersion
		{
			get
			{
				return this._newGameVersion;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000F2DE File Offset: 0x0000D4DE
		public MBReadOnlyList<string> PreviouslyUsedModules
		{
			get
			{
				return this._previouslyUsedModules;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000F2E6 File Offset: 0x0000D4E6
		public MBReadOnlyList<string> UsedGameVersions
		{
			get
			{
				return this._usedGameVersions;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000F2EE File Offset: 0x0000D4EE
		// (set) Token: 0x06000181 RID: 385 RVA: 0x0000F2F6 File Offset: 0x0000D4F6
		[SaveableProperty(83)]
		public bool EnabledCheatsBefore { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000F2FF File Offset: 0x0000D4FF
		// (set) Token: 0x06000183 RID: 387 RVA: 0x0000F307 File Offset: 0x0000D507
		[SaveableProperty(82)]
		public string PlatformID { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0000F310 File Offset: 0x0000D510
		// (set) Token: 0x06000185 RID: 389 RVA: 0x0000F318 File Offset: 0x0000D518
		internal CampaignEventDispatcher CampaignEventDispatcher { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000F321 File Offset: 0x0000D521
		// (set) Token: 0x06000187 RID: 391 RVA: 0x0000F329 File Offset: 0x0000D529
		[SaveableProperty(80)]
		public string UniqueGameId { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000F332 File Offset: 0x0000D532
		// (set) Token: 0x06000189 RID: 393 RVA: 0x0000F33A File Offset: 0x0000D53A
		public SaveHandler SaveHandler { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000F343 File Offset: 0x0000D543
		public override bool SupportsSaving
		{
			get
			{
				return this.GameMode == CampaignGameMode.Campaign;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0000F34E File Offset: 0x0000D54E
		// (set) Token: 0x0600018C RID: 396 RVA: 0x0000F356 File Offset: 0x0000D556
		[SaveableProperty(211)]
		public CampaignObjectManager CampaignObjectManager { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000F35F File Offset: 0x0000D55F
		public override bool IsDevelopment
		{
			get
			{
				return this.GameMode == CampaignGameMode.Tutorial;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600018E RID: 398 RVA: 0x0000F36A File Offset: 0x0000D56A
		// (set) Token: 0x0600018F RID: 399 RVA: 0x0000F372 File Offset: 0x0000D572
		[SaveableProperty(3)]
		public bool IsCraftingEnabled { get; set; } = true;

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000F37B File Offset: 0x0000D57B
		// (set) Token: 0x06000191 RID: 401 RVA: 0x0000F383 File Offset: 0x0000D583
		[SaveableProperty(4)]
		public bool IsBannerEditorEnabled { get; set; } = true;

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000F38C File Offset: 0x0000D58C
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000F394 File Offset: 0x0000D594
		[SaveableProperty(5)]
		public bool IsFaceGenEnabled { get; set; } = true;

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000F39D File Offset: 0x0000D59D
		public ICampaignBehaviorManager CampaignBehaviorManager
		{
			get
			{
				return this._campaignBehaviorManager;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000F3A5 File Offset: 0x0000D5A5
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000F3AD File Offset: 0x0000D5AD
		[SaveableProperty(8)]
		public QuestManager QuestManager { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000F3B6 File Offset: 0x0000D5B6
		// (set) Token: 0x06000198 RID: 408 RVA: 0x0000F3BE File Offset: 0x0000D5BE
		[SaveableProperty(9)]
		public IssueManager IssueManager { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000F3C7 File Offset: 0x0000D5C7
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000F3CF File Offset: 0x0000D5CF
		[SaveableProperty(11)]
		public FactionManager FactionManager { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000F3D8 File Offset: 0x0000D5D8
		// (set) Token: 0x0600019C RID: 412 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
		[SaveableProperty(12)]
		public CharacterRelationManager CharacterRelationManager { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000F3E9 File Offset: 0x0000D5E9
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000F3F1 File Offset: 0x0000D5F1
		[SaveableProperty(14)]
		public Romance Romance { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000F3FA File Offset: 0x0000D5FA
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0000F402 File Offset: 0x0000D602
		[SaveableProperty(16)]
		public PlayerCaptivity PlayerCaptivity { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000F40B File Offset: 0x0000D60B
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x0000F413 File Offset: 0x0000D613
		[SaveableProperty(17)]
		internal Clan PlayerDefaultFaction { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000F41C File Offset: 0x0000D61C
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000F424 File Offset: 0x0000D624
		public CampaignMission.ICampaignMissionManager CampaignMissionManager { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000F42D File Offset: 0x0000D62D
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x0000F435 File Offset: 0x0000D635
		public ISkillLevelingManager SkillLevelingManager { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000F43E File Offset: 0x0000D63E
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000F446 File Offset: 0x0000D646
		public IMapSceneCreator MapSceneCreator { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000F44F File Offset: 0x0000D64F
		public override bool IsInventoryAccessibleAtMission
		{
			get
			{
				return this.GameMode == CampaignGameMode.Tutorial;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000F45A File Offset: 0x0000D65A
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000F462 File Offset: 0x0000D662
		public GameMenuCallbackManager GameMenuCallbackManager { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000F46B File Offset: 0x0000D66B
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000F473 File Offset: 0x0000D673
		public VisualCreator VisualCreator { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001AE RID: 430 RVA: 0x0000F47C File Offset: 0x0000D67C
		// (set) Token: 0x060001AF RID: 431 RVA: 0x0000F484 File Offset: 0x0000D684
		[SaveableProperty(28)]
		public MapStateData MapStateData { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000F48D File Offset: 0x0000D68D
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000F495 File Offset: 0x0000D695
		public DefaultPerks DefaultPerks { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000F49E File Offset: 0x0000D69E
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000F4A6 File Offset: 0x0000D6A6
		public DefaultTraits DefaultTraits { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000F4AF File Offset: 0x0000D6AF
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000F4B7 File Offset: 0x0000D6B7
		public DefaultPolicies DefaultPolicies { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000F4C0 File Offset: 0x0000D6C0
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000F4C8 File Offset: 0x0000D6C8
		public DefaultBuildingTypes DefaultBuildingTypes { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0000F4D1 File Offset: 0x0000D6D1
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x0000F4D9 File Offset: 0x0000D6D9
		public DefaultIssueEffects DefaultIssueEffects { get; private set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0000F4E2 File Offset: 0x0000D6E2
		// (set) Token: 0x060001BB RID: 443 RVA: 0x0000F4EA File Offset: 0x0000D6EA
		public DefaultItems DefaultItems { get; private set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000F4F3 File Offset: 0x0000D6F3
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000F4FB File Offset: 0x0000D6FB
		public DefaultSiegeStrategies DefaultSiegeStrategies { get; private set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000F504 File Offset: 0x0000D704
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000F50C File Offset: 0x0000D70C
		internal MBReadOnlyList<PerkObject> AllPerks { get; private set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000F515 File Offset: 0x0000D715
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000F51D File Offset: 0x0000D71D
		public DefaultSkillEffects DefaultSkillEffects { get; private set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000F526 File Offset: 0x0000D726
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x0000F52E File Offset: 0x0000D72E
		public DefaultVillageTypes DefaultVillageTypes { get; private set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000F537 File Offset: 0x0000D737
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x0000F53F File Offset: 0x0000D73F
		internal MBReadOnlyList<TraitObject> AllTraits { get; private set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000F548 File Offset: 0x0000D748
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000F550 File Offset: 0x0000D750
		internal MBReadOnlyList<MBEquipmentRoster> AllEquipmentRosters { get; private set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000F559 File Offset: 0x0000D759
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000F561 File Offset: 0x0000D761
		public DefaultCulturalFeats DefaultFeats { get; private set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000F56A File Offset: 0x0000D76A
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000F572 File Offset: 0x0000D772
		internal MBReadOnlyList<PolicyObject> AllPolicies { get; private set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000F57B File Offset: 0x0000D77B
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000F583 File Offset: 0x0000D783
		internal MBReadOnlyList<BuildingType> AllBuildingTypes { get; private set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000F58C File Offset: 0x0000D78C
		// (set) Token: 0x060001CF RID: 463 RVA: 0x0000F594 File Offset: 0x0000D794
		internal MBReadOnlyList<IssueEffect> AllIssueEffects { get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000F59D File Offset: 0x0000D79D
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000F5A5 File Offset: 0x0000D7A5
		internal MBReadOnlyList<SiegeStrategy> AllSiegeStrategies { get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000F5AE File Offset: 0x0000D7AE
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x0000F5B6 File Offset: 0x0000D7B6
		internal MBReadOnlyList<VillageType> AllVillageTypes { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000F5BF File Offset: 0x0000D7BF
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x0000F5C7 File Offset: 0x0000D7C7
		internal MBReadOnlyList<SkillEffect> AllSkillEffects { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
		internal MBReadOnlyList<FeatObject> AllFeats { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000F5E1 File Offset: 0x0000D7E1
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000F5E9 File Offset: 0x0000D7E9
		internal MBReadOnlyList<SkillObject> AllSkills { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000F5F2 File Offset: 0x0000D7F2
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000F5FA File Offset: 0x0000D7FA
		internal MBReadOnlyList<SiegeEngineType> AllSiegeEngineTypes { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000F603 File Offset: 0x0000D803
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000F60B File Offset: 0x0000D80B
		internal MBReadOnlyList<ItemCategory> AllItemCategories { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000F614 File Offset: 0x0000D814
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000F61C File Offset: 0x0000D81C
		internal MBReadOnlyList<CharacterAttribute> AllCharacterAttributes { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000F625 File Offset: 0x0000D825
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000F62D File Offset: 0x0000D82D
		internal MBReadOnlyList<ItemObject> AllItems { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000F636 File Offset: 0x0000D836
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x0000F63E File Offset: 0x0000D83E
		[SaveableProperty(100)]
		internal MapTimeTracker MapTimeTracker { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000F647 File Offset: 0x0000D847
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000F64F File Offset: 0x0000D84F
		public bool TimeControlModeLock { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000F658 File Offset: 0x0000D858
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x0000F660 File Offset: 0x0000D860
		public CampaignTimeControlMode TimeControlMode
		{
			get
			{
				return this._timeControlMode;
			}
			set
			{
				if (!this.TimeControlModeLock && value != this._timeControlMode)
				{
					this._timeControlMode = value;
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000F67A File Offset: 0x0000D87A
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000F682 File Offset: 0x0000D882
		public bool IsMapTooltipLongForm { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000F68B File Offset: 0x0000D88B
		// (set) Token: 0x060001EB RID: 491 RVA: 0x0000F693 File Offset: 0x0000D893
		public float SpeedUpMultiplier { get; set; } = 4f;

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000F69C File Offset: 0x0000D89C
		public float CampaignDt
		{
			get
			{
				return this._dt;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000F6A4 File Offset: 0x0000D8A4
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000F6AC File Offset: 0x0000D8AC
		public bool TrueSight { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000F6B5 File Offset: 0x0000D8B5
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0000F6BC File Offset: 0x0000D8BC
		public static Campaign Current { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000F6C4 File Offset: 0x0000D8C4
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000F6CC File Offset: 0x0000D8CC
		[SaveableProperty(36)]
		public CampaignTime CampaignStartTime { get; private set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000F6D5 File Offset: 0x0000D8D5
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000F6DD File Offset: 0x0000D8DD
		[SaveableProperty(37)]
		public CampaignGameMode GameMode { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000F6E6 File Offset: 0x0000D8E6
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x0000F6EE File Offset: 0x0000D8EE
		[SaveableProperty(38)]
		public float PlayerProgress { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000F6F7 File Offset: 0x0000D8F7
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x0000F6FF File Offset: 0x0000D8FF
		public GameMenuManager GameMenuManager { get; private set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000F708 File Offset: 0x0000D908
		public GameModels Models
		{
			get
			{
				return this._gameModels;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000F710 File Offset: 0x0000D910
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000F718 File Offset: 0x0000D918
		public SandBoxManager SandBoxManager { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000F721 File Offset: 0x0000D921
		public Campaign.GameLoadingType CampaignGameLoadingType
		{
			get
			{
				return this._gameLoadingType;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000F72C File Offset: 0x0000D92C
		public Campaign(CampaignGameMode gameMode)
		{
			this.GameMode = gameMode;
			this.Options = new CampaignOptions();
			this.MapTimeTracker = new MapTimeTracker(CampaignData.CampaignStartTime);
			this.CampaignStartTime = this.MapTimeTracker.Now;
			this.CampaignObjectManager = new CampaignObjectManager();
			this.CurrentConversationContext = ConversationContext.Default;
			this.QuestManager = new QuestManager();
			this.IssueManager = new IssueManager();
			this.FactionManager = new FactionManager();
			this.CharacterRelationManager = new CharacterRelationManager();
			this.Romance = new Romance();
			this.PlayerCaptivity = new PlayerCaptivity();
			this.BarterManager = new BarterManager();
			this.GameMenuCallbackManager = new GameMenuCallbackManager();
			this._campaignPeriodicEventManager = new CampaignPeriodicEventManager();
			this._tickData = new CampaignTickCacheDataStore();
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000F836 File Offset: 0x0000DA36
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000F83E File Offset: 0x0000DA3E
		[SaveableProperty(40)]
		public SiegeEventManager SiegeEventManager { get; internal set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000F847 File Offset: 0x0000DA47
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000F84F File Offset: 0x0000DA4F
		[SaveableProperty(41)]
		public MapEventManager MapEventManager { get; internal set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000F858 File Offset: 0x0000DA58
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000F860 File Offset: 0x0000DA60
		internal CampaignEvents CampaignEvents { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000F86C File Offset: 0x0000DA6C
		public MenuContext CurrentMenuContext
		{
			get
			{
				GameStateManager gameStateManager = base.CurrentGame.GameStateManager;
				TutorialState tutorialState = gameStateManager.ActiveState as TutorialState;
				if (tutorialState != null)
				{
					return tutorialState.MenuContext;
				}
				MapState mapState = gameStateManager.ActiveState as MapState;
				if (mapState != null)
				{
					return mapState.MenuContext;
				}
				GameState activeState = gameStateManager.ActiveState;
				MapState mapState2;
				if (((activeState != null) ? activeState.Predecessor : null) != null && (mapState2 = (gameStateManager.ActiveState.Predecessor as MapState)) != null)
				{
					return mapState2.MenuContext;
				}
				return null;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000F8E1 File Offset: 0x0000DAE1
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000F8E9 File Offset: 0x0000DAE9
		internal List<MBCampaignEvent> CustomPeriodicCampaignEvents { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000F8F2 File Offset: 0x0000DAF2
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000F8FA File Offset: 0x0000DAFA
		public bool IsMainPartyWaiting
		{
			get
			{
				return this._isMainPartyWaiting;
			}
			private set
			{
				this._isMainPartyWaiting = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000F903 File Offset: 0x0000DB03
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000F90B File Offset: 0x0000DB0B
		[SaveableProperty(45)]
		private int _curMapFrame { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000F914 File Offset: 0x0000DB14
		internal LocatorGrid<Settlement> SettlementLocator
		{
			get
			{
				LocatorGrid<Settlement> result;
				if ((result = this._settlementLocator) == null)
				{
					result = (this._settlementLocator = new LocatorGrid<Settlement>(5f, 32, 32));
				}
				return result;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000F944 File Offset: 0x0000DB44
		internal LocatorGrid<MobileParty> MobilePartyLocator
		{
			get
			{
				LocatorGrid<MobileParty> result;
				if ((result = this._mobilePartyLocator) == null)
				{
					result = (this._mobilePartyLocator = new LocatorGrid<MobileParty>(5f, 32, 32));
				}
				return result;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000F972 File Offset: 0x0000DB72
		public IMapScene MapSceneWrapper
		{
			get
			{
				return this._mapSceneWrapper;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000F97A File Offset: 0x0000DB7A
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000F982 File Offset: 0x0000DB82
		[SaveableProperty(54)]
		public PlayerEncounter PlayerEncounter { get; internal set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000F98B File Offset: 0x0000DB8B
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000F993 File Offset: 0x0000DB93
		[CachedData]
		internal LocationEncounter LocationEncounter { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000F99C File Offset: 0x0000DB9C
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		internal NameGenerator NameGenerator { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000F9AD File Offset: 0x0000DBAD
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000F9B5 File Offset: 0x0000DBB5
		[SaveableProperty(58)]
		public BarterManager BarterManager { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000F9BE File Offset: 0x0000DBBE
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000F9C6 File Offset: 0x0000DBC6
		[SaveableProperty(69)]
		public bool IsMainHeroDisguised { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000F9CF File Offset: 0x0000DBCF
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000F9D7 File Offset: 0x0000DBD7
		[SaveableProperty(70)]
		public bool DesertionEnabled { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000F9E0 File Offset: 0x0000DBE0
		public Vec2 DefaultStartingPosition
		{
			get
			{
				return new Vec2(685.3f, 410.9f);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000F9F1 File Offset: 0x0000DBF1
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0000F9F9 File Offset: 0x0000DBF9
		public Equipment DeadBattleEquipment { get; set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000FA02 File Offset: 0x0000DC02
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000FA0A File Offset: 0x0000DC0A
		public Equipment DeadCivilianEquipment { get; set; }

		// Token: 0x0600021F RID: 543 RVA: 0x0000FA14 File Offset: 0x0000DC14
		public void InitializeMainParty()
		{
			this.InitializeSinglePlayerReferences();
			this.MainParty.InitializeMobilePartyAtPosition(base.CurrentGame.ObjectManager.GetObject<PartyTemplateObject>("main_hero_party_template"), this.DefaultStartingPosition, -1);
			this.MainParty.ActualClan = Clan.PlayerClan;
			this.MainParty.PartyComponent = new LordPartyComponent(Hero.MainHero, Hero.MainHero);
			this.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 1);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000FA90 File Offset: 0x0000DC90
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData, ObjectLoadData objectLoadData)
		{
			this._campaignEntitySystem = new EntitySystem<CampaignEntityComponent>();
			this.PlayerFormationPreferences = this._playerFormationPreferences.GetReadOnlyDictionary<CharacterObject, FormationClass>();
			this.SpeedUpMultiplier = 4f;
			if (this.UniqueGameId == null && MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.2", 66233))
			{
				this.UniqueGameId = "oldSave";
			}
			if (this.BusyHideouts == null)
			{
				this.BusyHideouts = new List<Settlement>();
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000FB0C File Offset: 0x0000DD0C
		private void InitializeForSavedGame()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				settlement.Party.OnFinishLoadState();
			}
			foreach (MobileParty mobileParty in this.MobileParties.ToList<MobileParty>())
			{
				mobileParty.Party.OnFinishLoadState();
			}
			foreach (Settlement settlement2 in Settlement.All)
			{
				settlement2.OnFinishLoadState();
			}
			this.GameMenuCallbackManager = new GameMenuCallbackManager();
			this.GameMenuCallbackManager.OnGameLoad();
			this.IssueManager.InitializeForSavedGame();
			this.MinSettlementX = 1000f;
			this.MinSettlementY = 1000f;
			foreach (Settlement settlement3 in Settlement.All)
			{
				if (settlement3.Position2D.x < this.MinSettlementX)
				{
					this.MinSettlementX = settlement3.Position2D.x;
				}
				if (settlement3.Position2D.y < this.MinSettlementY)
				{
					this.MinSettlementY = settlement3.Position2D.y;
				}
				if (settlement3.Position2D.x > this.MaxSettlementX)
				{
					this.MaxSettlementX = settlement3.Position2D.x;
				}
				if (settlement3.Position2D.y > this.MaxSettlementY)
				{
					this.MaxSettlementY = settlement3.Position2D.y;
				}
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000FCF0 File Offset: 0x0000DEF0
		private void OnGameLoaded(CampaignGameStarter starter)
		{
			this._tickData = new CampaignTickCacheDataStore();
			base.ObjectManager.PreAfterLoad();
			this.CampaignObjectManager.PreAfterLoad();
			base.ObjectManager.AfterLoad();
			this.CampaignObjectManager.AfterLoad();
			this.CharacterRelationManager.AfterLoad();
			CampaignEventDispatcher.Instance.OnGameEarlyLoaded(starter);
			TroopRoster.CalculateCachedStatsOnLoad();
			CampaignEventDispatcher.Instance.OnGameLoaded(starter);
			this.InitializeForSavedGame();
			this._tickData.InitializeDataCache();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000FD6C File Offset: 0x0000DF6C
		private void OnDataLoadFinished(CampaignGameStarter starter)
		{
			this._towns = (from x in Settlement.All
			where x.IsTown
			select x.Town).ToMBList<Town>();
			this._castles = (from x in Settlement.All
			where x.IsCastle
			select x.Town).ToMBList<Town>();
			this._villages = (from x in Settlement.All
			where x.Village != null
			select x.Village).ToMBList<Village>();
			this._hideouts = (from x in Settlement.All
			where x.IsHideout
			select x.Hideout).ToMBList<Hideout>();
			this._campaignPeriodicEventManager.InitializeTickers();
			this.CreateCampaignEvents();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000FEEC File Offset: 0x0000E0EC
		private void OnSessionStart(CampaignGameStarter starter)
		{
			CampaignEventDispatcher.Instance.OnSessionStart(starter);
			CampaignEventDispatcher.Instance.OnAfterSessionStart(starter);
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			this.ConversationManager.Build();
			foreach (Settlement settlement in this.Settlements)
			{
				settlement.OnSessionStart();
			}
			this.IsCraftingEnabled = true;
			this.IsBannerEditorEnabled = true;
			this.IsFaceGenEnabled = true;
			this.MapEventManager.OnAfterLoad();
			this.KingdomManager.RegisterEvents();
			this.KingdomManager.OnSessionStart();
			this.CampaignInformationManager.RegisterEvents();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.IsVillage)
			{
				settlement.Village.DailyTick();
				return;
			}
			if (settlement.Town != null)
			{
				settlement.Town.DailyTick();
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
		private void GameInitTick()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				settlement.Party.UpdateVisibilityAndInspected(0f);
			}
			foreach (MobileParty mobileParty in this.MobileParties)
			{
				mobileParty.Party.UpdateVisibilityAndInspected(0f);
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00010088 File Offset: 0x0000E288
		internal void HourlyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			CampaignEventDispatcher.Instance.HourlyTick();
			MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
			if (mapState == null)
			{
				return;
			}
			mapState.OnHourlyTick();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000100B4 File Offset: 0x0000E2B4
		internal void DailyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			this.PlayerProgress = (this.PlayerProgress + Campaign.Current.Models.PlayerProgressionModel.GetPlayerProgress()) / 2f;
			Debug.Print("Before Daily Tick: " + CampaignTime.Now.ToString(), 0, Debug.DebugColor.White, 17592186044416UL);
			CampaignEventDispatcher.Instance.DailyTick();
			if ((int)this.CampaignStartTime.ElapsedDaysUntilNow % 7 == 0)
			{
				CampaignEventDispatcher.Instance.WeeklyTick();
				this.OnWeeklyTick();
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00010143 File Offset: 0x0000E343
		public void WaitAsyncTasks()
		{
			if (this.CampaignLateAITickTask != null)
			{
				this.CampaignLateAITickTask.Wait();
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00010158 File Offset: 0x0000E358
		private void OnWeeklyTick()
		{
			this.LogEntryHistory.DeleteOutdatedLogs();
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00010168 File Offset: 0x0000E368
		public CampaignTimeControlMode GetSimplifiedTimeControlMode()
		{
			switch (this.TimeControlMode)
			{
			case CampaignTimeControlMode.Stop:
				return CampaignTimeControlMode.Stop;
			case CampaignTimeControlMode.UnstoppablePlay:
				return CampaignTimeControlMode.UnstoppablePlay;
			case CampaignTimeControlMode.UnstoppableFastForward:
			case CampaignTimeControlMode.UnstoppableFastForwardForPartyWaitTime:
				return CampaignTimeControlMode.UnstoppableFastForward;
			case CampaignTimeControlMode.StoppablePlay:
				if (!this.IsMainPartyWaiting)
				{
					return CampaignTimeControlMode.StoppablePlay;
				}
				return CampaignTimeControlMode.Stop;
			case CampaignTimeControlMode.StoppableFastForward:
				if (!this.IsMainPartyWaiting)
				{
					return CampaignTimeControlMode.StoppableFastForward;
				}
				return CampaignTimeControlMode.Stop;
			default:
				return CampaignTimeControlMode.Stop;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000101BB File Offset: 0x0000E3BB
		private void CheckMainPartyNeedsUpdate()
		{
			MobileParty.MainParty.Ai.CheckPartyNeedsUpdate();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000101CC File Offset: 0x0000E3CC
		private void TickMapTime(float realDt)
		{
			float num = 0f;
			float speedUpMultiplier = this.SpeedUpMultiplier;
			float num2 = 0.25f * realDt;
			this.IsMainPartyWaiting = MobileParty.MainParty.ComputeIsWaiting();
			switch (this.TimeControlMode)
			{
			case CampaignTimeControlMode.Stop:
			case CampaignTimeControlMode.FastForwardStop:
				break;
			case CampaignTimeControlMode.UnstoppablePlay:
				num = num2;
				break;
			case CampaignTimeControlMode.UnstoppableFastForward:
			case CampaignTimeControlMode.UnstoppableFastForwardForPartyWaitTime:
				num = num2 * speedUpMultiplier;
				break;
			case CampaignTimeControlMode.StoppablePlay:
				if (!this.IsMainPartyWaiting)
				{
					num = num2;
				}
				break;
			case CampaignTimeControlMode.StoppableFastForward:
				if (!this.IsMainPartyWaiting)
				{
					num = num2 * speedUpMultiplier;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this._dt = num;
			this.MapTimeTracker.Tick(4320f * num);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0001026C File Offset: 0x0000E46C
		public void OnGameOver()
		{
			if (CampaignOptions.IsIronmanMode)
			{
				this.SaveHandler.QuickSaveCurrentGame();
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00010280 File Offset: 0x0000E480
		internal void RealTick(float realDt)
		{
			this.WaitAsyncTasks();
			this.CheckMainPartyNeedsUpdate();
			this.TickMapTime(realDt);
			foreach (CampaignEntityComponent campaignEntityComponent in this._campaignEntitySystem.GetComponents())
			{
				campaignEntityComponent.OnTick(realDt, this._dt);
			}
			if (!this.GameStarted)
			{
				this.GameStarted = true;
				this._tickData.InitializeDataCache();
				this.SiegeEventManager.Tick(this._dt);
			}
			this._tickData.RealTick(this._dt, realDt);
			this.SiegeEventManager.Tick(this._dt);
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00010340 File Offset: 0x0000E540
		public static float CurrentTime
		{
			get
			{
				return (float)CampaignTime.Now.ToHours;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001035C File Offset: 0x0000E55C
		public void SetTimeSpeed(int speed)
		{
			switch (speed)
			{
			case 0:
				if (this.TimeControlMode == CampaignTimeControlMode.UnstoppableFastForward || this.TimeControlMode == CampaignTimeControlMode.StoppableFastForward)
				{
					this.TimeControlMode = CampaignTimeControlMode.FastForwardStop;
					return;
				}
				if (this.TimeControlMode != CampaignTimeControlMode.FastForwardStop && this.TimeControlMode != CampaignTimeControlMode.Stop)
				{
					this.TimeControlMode = CampaignTimeControlMode.Stop;
					return;
				}
				break;
			case 1:
				if (((this.TimeControlMode == CampaignTimeControlMode.Stop || this.TimeControlMode == CampaignTimeControlMode.FastForwardStop) && this.MainParty.DefaultBehavior == AiBehavior.Hold) || this.IsMainPartyWaiting || (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty))
				{
					this.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
					return;
				}
				this.TimeControlMode = CampaignTimeControlMode.StoppablePlay;
				return;
			case 2:
				if (((this.TimeControlMode == CampaignTimeControlMode.Stop || this.TimeControlMode == CampaignTimeControlMode.FastForwardStop) && this.MainParty.DefaultBehavior == AiBehavior.Hold) || this.IsMainPartyWaiting || (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty))
				{
					this.TimeControlMode = CampaignTimeControlMode.UnstoppableFastForward;
					return;
				}
				this.TimeControlMode = CampaignTimeControlMode.StoppableFastForward;
				break;
			default:
				return;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00010464 File Offset: 0x0000E664
		public static void LateAITick()
		{
			Campaign.Current.LateAITickAux();
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00010470 File Offset: 0x0000E670
		internal void LateAITickAux()
		{
			if (this._dt > 0f || this._curSessionFrame < 3)
			{
				this.PartiesThink(this._dt);
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00010494 File Offset: 0x0000E694
		internal void Tick()
		{
			int curMapFrame = this._curMapFrame;
			this._curMapFrame = curMapFrame + 1;
			this._curSessionFrame++;
			if (this._dt > 0f || this._curSessionFrame < 3)
			{
				CampaignEventDispatcher.Instance.Tick(this._dt);
				this._campaignPeriodicEventManager.OnTick(this._dt);
				this.MapEventManager.Tick();
				this._lastNonZeroDtFrame = this._curMapFrame;
				this._campaignPeriodicEventManager.MobilePartyHourlyTick();
			}
			if (this._dt > 0f)
			{
				this._campaignPeriodicEventManager.TickPeriodicEvents();
			}
			this._tickData.Tick();
			Campaign.Current.PlayerCaptivity.Update(this._dt);
			if (this._dt > 0f || (MobileParty.MainParty.MapEvent == null && this._curMapFrame == this._lastNonZeroDtFrame + 1))
			{
				EncounterManager.Tick(this._dt);
				MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
				if (mapState != null && mapState.AtMenu && !mapState.MenuContext.GameMenu.IsWaitActive)
				{
					this._dt = 0f;
				}
			}
			if (this._dt > 0f || this._curSessionFrame < 3)
			{
				this._campaignPeriodicEventManager.TickPartialHourlyAi();
			}
			MapState mapState2;
			if ((mapState2 = (Game.Current.GameStateManager.ActiveState as MapState)) != null && !mapState2.AtMenu)
			{
				string genericStateMenu = this.Models.EncounterGameMenuModel.GetGenericStateMenu();
				if (!string.IsNullOrEmpty(genericStateMenu))
				{
					GameMenu.ActivateGameMenu(genericStateMenu);
				}
			}
			this.CampaignLateAITickTask.Invoke();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0001062C File Offset: 0x0000E82C
		private void CreateCampaignEvents()
		{
			long numTicks = (CampaignTime.Now - CampaignData.CampaignStartTime).NumTicks;
			CampaignTime initialWait = CampaignTime.Days(1f);
			if (numTicks % 864000000L != 0L)
			{
				initialWait = CampaignTime.Days((float)(numTicks % 864000000L) / 864000000f);
			}
			this._dailyTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Days(1f), initialWait);
			this._dailyTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.DailyTick));
			CampaignTime initialWait2 = CampaignTime.Hours(0.5f);
			if (numTicks % 36000000L != 0L)
			{
				initialWait2 = CampaignTime.Hours((float)(numTicks % 36000000L) / 36000000f);
			}
			this._hourlyTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Hours(1f), initialWait2);
			this._hourlyTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.HourlyTick));
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00010700 File Offset: 0x0000E900
		private void PartiesThink(float dt)
		{
			for (int i = 0; i < this.MobileParties.Count; i++)
			{
				this.MobileParties[i].Ai.Tick(dt);
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0001073C File Offset: 0x0000E93C
		public TComponent GetEntityComponent<TComponent>() where TComponent : CampaignEntityComponent
		{
			EntitySystem<CampaignEntityComponent> campaignEntitySystem = this._campaignEntitySystem;
			if (campaignEntitySystem == null)
			{
				return default(TComponent);
			}
			return campaignEntitySystem.GetComponent<TComponent>();
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00010762 File Offset: 0x0000E962
		public TComponent AddEntityComponent<TComponent>() where TComponent : CampaignEntityComponent, new()
		{
			return this._campaignEntitySystem.AddComponent<TComponent>();
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0001076F File Offset: 0x0000E96F
		public MBReadOnlyList<CampaignEntityComponent> CampaignEntityComponents
		{
			get
			{
				return this._campaignEntitySystem.Components;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0001077C File Offset: 0x0000E97C
		public T GetCampaignBehavior<T>()
		{
			return this._campaignBehaviorManager.GetBehavior<T>();
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00010789 File Offset: 0x0000E989
		public IEnumerable<T> GetCampaignBehaviors<T>()
		{
			return this._campaignBehaviorManager.GetBehaviors<T>();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00010796 File Offset: 0x0000E996
		public void AddCampaignBehaviorManager(ICampaignBehaviorManager manager)
		{
			this._campaignBehaviorManager = manager;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0001079F File Offset: 0x0000E99F
		public MBReadOnlyList<Hero> AliveHeroes
		{
			get
			{
				return this.CampaignObjectManager.AliveHeroes;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600023E RID: 574 RVA: 0x000107AC File Offset: 0x0000E9AC
		public MBReadOnlyList<Hero> DeadOrDisabledHeroes
		{
			get
			{
				return this.CampaignObjectManager.DeadOrDisabledHeroes;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600023F RID: 575 RVA: 0x000107B9 File Offset: 0x0000E9B9
		public MBReadOnlyList<MobileParty> MobileParties
		{
			get
			{
				return this.CampaignObjectManager.MobileParties;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000107C6 File Offset: 0x0000E9C6
		public MBReadOnlyList<MobileParty> CaravanParties
		{
			get
			{
				return this.CampaignObjectManager.CaravanParties;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000241 RID: 577 RVA: 0x000107D3 File Offset: 0x0000E9D3
		public MBReadOnlyList<MobileParty> VillagerParties
		{
			get
			{
				return this.CampaignObjectManager.VillagerParties;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000107E0 File Offset: 0x0000E9E0
		public MBReadOnlyList<MobileParty> MilitiaParties
		{
			get
			{
				return this.CampaignObjectManager.MilitiaParties;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000243 RID: 579 RVA: 0x000107ED File Offset: 0x0000E9ED
		public MBReadOnlyList<MobileParty> GarrisonParties
		{
			get
			{
				return this.CampaignObjectManager.GarrisonParties;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000107FA File Offset: 0x0000E9FA
		public MBReadOnlyList<MobileParty> CustomParties
		{
			get
			{
				return this.CampaignObjectManager.CustomParties;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00010807 File Offset: 0x0000EA07
		public MBReadOnlyList<MobileParty> LordParties
		{
			get
			{
				return this.CampaignObjectManager.LordParties;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00010814 File Offset: 0x0000EA14
		public MBReadOnlyList<MobileParty> BanditParties
		{
			get
			{
				return this.CampaignObjectManager.BanditParties;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000247 RID: 583 RVA: 0x00010821 File Offset: 0x0000EA21
		public MBReadOnlyList<MobileParty> PartiesWithoutPartyComponent
		{
			get
			{
				return this.CampaignObjectManager.PartiesWithoutPartyComponent;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0001082E File Offset: 0x0000EA2E
		public MBReadOnlyList<Settlement> Settlements
		{
			get
			{
				return this.CampaignObjectManager.Settlements;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0001083B File Offset: 0x0000EA3B
		public IEnumerable<IFaction> Factions
		{
			get
			{
				return this.CampaignObjectManager.Factions;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00010848 File Offset: 0x0000EA48
		public MBReadOnlyList<Kingdom> Kingdoms
		{
			get
			{
				return this.CampaignObjectManager.Kingdoms;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00010855 File Offset: 0x0000EA55
		public MBReadOnlyList<Clan> Clans
		{
			get
			{
				return this.CampaignObjectManager.Clans;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00010862 File Offset: 0x0000EA62
		public MBReadOnlyList<CharacterObject> Characters
		{
			get
			{
				return this._characters;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0001086A File Offset: 0x0000EA6A
		public MBReadOnlyList<WorkshopType> Workshops
		{
			get
			{
				return this._workshops;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00010872 File Offset: 0x0000EA72
		public MBReadOnlyList<ItemModifier> ItemModifiers
		{
			get
			{
				return this._itemModifiers;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0001087A File Offset: 0x0000EA7A
		public MBReadOnlyList<ItemModifierGroup> ItemModifierGroups
		{
			get
			{
				return this._itemModifierGroups;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00010882 File Offset: 0x0000EA82
		public MBReadOnlyList<Concept> Concepts
		{
			get
			{
				return this._concepts;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0001088A File Offset: 0x0000EA8A
		// (set) Token: 0x06000252 RID: 594 RVA: 0x00010892 File Offset: 0x0000EA92
		[SaveableProperty(60)]
		public MobileParty MainParty { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0001089B File Offset: 0x0000EA9B
		// (set) Token: 0x06000254 RID: 596 RVA: 0x000108A3 File Offset: 0x0000EAA3
		public PartyBase CameraFollowParty
		{
			get
			{
				return this._cameraFollowParty;
			}
			set
			{
				this._cameraFollowParty = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000255 RID: 597 RVA: 0x000108AC File Offset: 0x0000EAAC
		// (set) Token: 0x06000256 RID: 598 RVA: 0x000108B4 File Offset: 0x0000EAB4
		[SaveableProperty(62)]
		public CampaignInformationManager CampaignInformationManager { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000257 RID: 599 RVA: 0x000108BD File Offset: 0x0000EABD
		// (set) Token: 0x06000258 RID: 600 RVA: 0x000108C5 File Offset: 0x0000EAC5
		[SaveableProperty(63)]
		public VisualTrackerManager VisualTrackerManager { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000259 RID: 601 RVA: 0x000108CE File Offset: 0x0000EACE
		public LogEntryHistory LogEntryHistory
		{
			get
			{
				return this._logEntryHistory;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600025A RID: 602 RVA: 0x000108D6 File Offset: 0x0000EAD6
		// (set) Token: 0x0600025B RID: 603 RVA: 0x000108DE File Offset: 0x0000EADE
		public EncyclopediaManager EncyclopediaManager { get; private set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600025C RID: 604 RVA: 0x000108E7 File Offset: 0x0000EAE7
		// (set) Token: 0x0600025D RID: 605 RVA: 0x000108EF File Offset: 0x0000EAEF
		public InventoryManager InventoryManager { get; private set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600025E RID: 606 RVA: 0x000108F8 File Offset: 0x0000EAF8
		// (set) Token: 0x0600025F RID: 607 RVA: 0x00010900 File Offset: 0x0000EB00
		public PartyScreenManager PartyScreenManager { get; private set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00010909 File Offset: 0x0000EB09
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00010911 File Offset: 0x0000EB11
		public ConversationManager ConversationManager { get; private set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0001091A File Offset: 0x0000EB1A
		public bool IsDay
		{
			get
			{
				return !this.IsNight;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00010928 File Offset: 0x0000EB28
		public bool IsNight
		{
			get
			{
				return CampaignTime.Now.IsNightTime;
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00010942 File Offset: 0x0000EB42
		internal int GeneratePartyId(PartyBase party)
		{
			int lastPartyIndex = this._lastPartyIndex;
			this._lastPartyIndex++;
			return lastPartyIndex;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00010958 File Offset: 0x0000EB58
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00010960 File Offset: 0x0000EB60
		[SaveableProperty(68)]
		public HeroTraitDeveloper PlayerTraitDeveloper { get; private set; }

		// Token: 0x06000267 RID: 615 RVA: 0x0001096C File Offset: 0x0000EB6C
		private void LoadMapScene()
		{
			this._mapSceneWrapper = this.MapSceneCreator.CreateMapScene();
			this._mapSceneWrapper.SetSceneLevels(new List<string>
			{
				"level_1",
				"level_2",
				"level_3",
				"siege",
				"raid",
				"burned"
			});
			this._mapSceneWrapper.Load();
			Vec2 mapMinimumPosition;
			Vec2 mapMaximumPosition;
			float mapMaximumHeight;
			this._mapSceneWrapper.GetMapBorders(out mapMinimumPosition, out mapMaximumPosition, out mapMaximumHeight);
			Campaign.MapMinimumPosition = mapMinimumPosition;
			Campaign.MapMaximumPosition = mapMaximumPosition;
			Campaign.MapMaximumHeight = mapMaximumHeight;
			Campaign.MapDiagonal = Campaign.MapMinimumPosition.Distance(Campaign.MapMaximumPosition);
			Campaign.MapDiagonalSquared = Campaign.MapDiagonal * Campaign.MapDiagonal;
			this.UpdateMaximumDistanceBetweenTwoSettlements();
			MapWeatherModel mapWeatherModel = Campaign.Current.Models.MapWeatherModel;
			if (mapWeatherModel != null)
			{
				byte[] array = new byte[2097152];
				this._mapSceneWrapper.GetSnowAmountData(array);
				mapWeatherModel.InitializeSnowAndRainAmountData(array);
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00010A6C File Offset: 0x0000EC6C
		public void UpdateMaximumDistanceBetweenTwoSettlements()
		{
			Campaign.MaximumDistanceBetweenTwoSettlements = Campaign.Current.Models.MapDistanceModel.MaximumDistanceBetweenTwoSettlements;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00010A88 File Offset: 0x0000EC88
		private void InitializeCachedLists()
		{
			MBObjectManager objectManager = Game.Current.ObjectManager;
			this._characters = objectManager.GetObjectTypeList<CharacterObject>();
			this._workshops = objectManager.GetObjectTypeList<WorkshopType>();
			this._itemModifiers = objectManager.GetObjectTypeList<ItemModifier>();
			this._itemModifierGroups = objectManager.GetObjectTypeList<ItemModifierGroup>();
			this._concepts = objectManager.GetObjectTypeList<Concept>();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00010ADC File Offset: 0x0000ECDC
		private void InitializeDefaultEquipments()
		{
			this.DeadBattleEquipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("default_battle_equipment_roster_neutral").DefaultEquipment;
			this.DeadCivilianEquipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("default_civilian_equipment_roster_neutral").DefaultEquipment;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00010B1C File Offset: 0x0000ED1C
		public override void OnDestroy()
		{
			this.WaitAsyncTasks();
			GameTexts.ClearInstance();
			IMapScene mapSceneWrapper = this._mapSceneWrapper;
			if (mapSceneWrapper != null)
			{
				mapSceneWrapper.Destroy();
			}
			ConversationManager.Clear();
			MBTextManager.ClearAll();
			GameSceneDataManager.Destroy();
			this.CampaignInformationManager.DeRegisterEvents();
			MBSaveLoad.OnGameDestroy();
			Campaign.Current = null;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00010B6A File Offset: 0x0000ED6A
		public void InitializeSinglePlayerReferences()
		{
			this.IsInitializedSinglePlayerReferences = true;
			this.InitializeGamePlayReferences();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00010B7C File Offset: 0x0000ED7C
		private void CreateLists()
		{
			this.AllPerks = MBObjectManager.Instance.GetObjectTypeList<PerkObject>();
			this.AllTraits = MBObjectManager.Instance.GetObjectTypeList<TraitObject>();
			this.AllEquipmentRosters = MBObjectManager.Instance.GetObjectTypeList<MBEquipmentRoster>();
			this.AllPolicies = MBObjectManager.Instance.GetObjectTypeList<PolicyObject>();
			this.AllBuildingTypes = MBObjectManager.Instance.GetObjectTypeList<BuildingType>();
			this.AllIssueEffects = MBObjectManager.Instance.GetObjectTypeList<IssueEffect>();
			this.AllSiegeStrategies = MBObjectManager.Instance.GetObjectTypeList<SiegeStrategy>();
			this.AllVillageTypes = MBObjectManager.Instance.GetObjectTypeList<VillageType>();
			this.AllSkillEffects = MBObjectManager.Instance.GetObjectTypeList<SkillEffect>();
			this.AllFeats = MBObjectManager.Instance.GetObjectTypeList<FeatObject>();
			this.AllSkills = MBObjectManager.Instance.GetObjectTypeList<SkillObject>();
			this.AllSiegeEngineTypes = MBObjectManager.Instance.GetObjectTypeList<SiegeEngineType>();
			this.AllItemCategories = MBObjectManager.Instance.GetObjectTypeList<ItemCategory>();
			this.AllCharacterAttributes = MBObjectManager.Instance.GetObjectTypeList<CharacterAttribute>();
			this.AllItems = MBObjectManager.Instance.GetObjectTypeList<ItemObject>();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00010C79 File Offset: 0x0000EE79
		private void CalculateCachedValues()
		{
			this.CalculateAverageDistanceBetweenTowns();
			this.CalculateAverageWage();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00010C88 File Offset: 0x0000EE88
		private void CalculateAverageWage()
		{
			float num = 0f;
			float num2 = 0f;
			foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
			{
				if (cultureObject.IsMainCulture)
				{
					foreach (PartyTemplateStack partyTemplateStack in cultureObject.DefaultPartyTemplate.Stacks)
					{
						int troopWage = partyTemplateStack.Character.TroopWage;
						float num3 = (float)(partyTemplateStack.MaxValue + partyTemplateStack.MinValue) * 0.5f;
						num += (float)troopWage * num3;
						num2 += num3;
					}
				}
			}
			if (num2 > 0f)
			{
				this.AverageWage = num / num2;
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00010D78 File Offset: 0x0000EF78
		private void CalculateAverageDistanceBetweenTowns()
		{
			if (this.GameMode != CampaignGameMode.Tutorial)
			{
				float num = 0f;
				int num2 = 0;
				foreach (Town town in this.AllTowns)
				{
					float num3 = 5000f;
					foreach (Town town2 in this.AllTowns)
					{
						if (town != town2)
						{
							float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(town.Settlement, town2.Settlement);
							if (distance < num3)
							{
								num3 = distance;
							}
						}
					}
					num += num3;
					num2++;
				}
				Campaign.AverageDistanceBetweenTwoFortifications = num / (float)num2;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00010E64 File Offset: 0x0000F064
		public void InitializeGamePlayReferences()
		{
			base.CurrentGame.PlayerTroop = base.CurrentGame.ObjectManager.GetObject<CharacterObject>("main_hero");
			if (Hero.MainHero.Mother != null)
			{
				Hero.MainHero.Mother.SetHasMet();
			}
			if (Hero.MainHero.Father != null)
			{
				Hero.MainHero.Father.SetHasMet();
			}
			this.PlayerDefaultFaction = this.CampaignObjectManager.Find<Clan>("player_faction");
			GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 1000, true);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00010EF0 File Offset: 0x0000F0F0
		private void InitializeScenes()
		{
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetModules())
			{
				string str = ModuleHelper.GetModuleFullPath(moduleInfo.Id) + "ModuleData/";
				string text = str + "sp_battle_scenes.xml";
				string text2 = str + "conversation_scenes.xml";
				string text3 = str + "meeting_scenes.xml";
				if (File.Exists(text))
				{
					GameSceneDataManager.Instance.LoadSPBattleScenes(text);
				}
				if (File.Exists(text2))
				{
					GameSceneDataManager.Instance.LoadConversationScenes(text2);
				}
				if (File.Exists(text3))
				{
					GameSceneDataManager.Instance.LoadMeetingScenes(text3);
				}
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00010FA4 File Offset: 0x0000F1A4
		public void SetLoadingParameters(Campaign.GameLoadingType gameLoadingType)
		{
			Campaign.Current = this;
			this._gameLoadingType = gameLoadingType;
			if (gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				Campaign.Current.GameStarted = true;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00010FC2 File Offset: 0x0000F1C2
		public void AddCampaignEventReceiver(CampaignEventReceiver receiver)
		{
			this.CampaignEventDispatcher.AddCampaignEventReceiver(receiver);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00010FD0 File Offset: 0x0000F1D0
		protected override void OnInitialize()
		{
			this.CampaignEvents = new CampaignEvents();
			this.CustomPeriodicCampaignEvents = new List<MBCampaignEvent>();
			this.CampaignEventDispatcher = new CampaignEventDispatcher(new CampaignEventReceiver[]
			{
				this.CampaignEvents,
				this.IssueManager,
				this.QuestManager
			});
			this.SandBoxManager = Game.Current.AddGameHandler<SandBoxManager>();
			this.SaveHandler = new SaveHandler();
			this.VisualCreator = new VisualCreator();
			this.GameMenuManager = new GameMenuManager();
			if (this._gameLoadingType != Campaign.GameLoadingType.Editor)
			{
				this.CreateManagers();
			}
			CampaignGameStarter campaignGameStarter = new CampaignGameStarter(this.GameMenuManager, this.ConversationManager, base.CurrentGame.GameTextManager);
			this.SandBoxManager.Initialize(campaignGameStarter);
			base.GameManager.InitializeGameStarter(base.CurrentGame, campaignGameStarter);
			GameSceneDataManager.Initialize();
			if (this._gameLoadingType == Campaign.GameLoadingType.NewCampaign || this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				this.InitializeScenes();
			}
			base.GameManager.OnGameStart(base.CurrentGame, campaignGameStarter);
			base.CurrentGame.SetBasicModels(campaignGameStarter.Models);
			this._gameModels = base.CurrentGame.AddGameModelsManager<GameModels>(campaignGameStarter.Models);
			base.CurrentGame.CreateGameManager();
			if (this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				this.InitializeDefaultCampaignObjects();
			}
			base.GameManager.BeginGameStart(base.CurrentGame);
			if (this._gameLoadingType != Campaign.GameLoadingType.SavedCampaign)
			{
				this.OnNewCampaignStart();
			}
			this.CreateLists();
			this.InitializeBasicObjectXmls();
			if (this._gameLoadingType != Campaign.GameLoadingType.SavedCampaign)
			{
				base.GameManager.OnNewCampaignStart(base.CurrentGame, campaignGameStarter);
			}
			this.SandBoxManager.OnCampaignStart(campaignGameStarter, base.GameManager, this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign);
			if (this._gameLoadingType != Campaign.GameLoadingType.SavedCampaign)
			{
				this.AddCampaignBehaviorManager(new CampaignBehaviorManager(campaignGameStarter.CampaignBehaviors));
				base.GameManager.OnAfterCampaignStart(base.CurrentGame);
			}
			else
			{
				base.GameManager.OnGameLoaded(base.CurrentGame, campaignGameStarter);
				this._campaignBehaviorManager.InitializeCampaignBehaviors(campaignGameStarter.CampaignBehaviors);
				this._campaignBehaviorManager.LoadBehaviorData();
				this._campaignBehaviorManager.RegisterEvents();
			}
			ICraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			if (campaignBehavior != null)
			{
				campaignBehavior.InitializeCraftingElements();
			}
			campaignGameStarter.UnregisterNonReadyObjects();
			if (this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
			{
				this.InitializeCampaignObjectsOnAfterLoad();
			}
			else if (this._gameLoadingType == Campaign.GameLoadingType.NewCampaign || this._gameLoadingType == Campaign.GameLoadingType.Tutorial)
			{
				this.CampaignObjectManager.InitializeOnNewGame();
			}
			this.InitializeCachedLists();
			this.InitializeDefaultEquipments();
			this.NameGenerator.Initialize();
			base.CurrentGame.OnGameStart();
			base.GameManager.OnGameInitializationFinished(base.CurrentGame);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0001124E File Offset: 0x0000F44E
		private void CalculateCachedStatsOnLoad()
		{
			ItemRoster.CalculateCachedStatsOnLoad();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00011255 File Offset: 0x0000F455
		private void InitializeBasicObjectXmls()
		{
			base.ObjectManager.LoadXML("SPCultures", false);
			base.ObjectManager.LoadXML("Concepts", false);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0001127C File Offset: 0x0000F47C
		private void InitializeDefaultCampaignObjects()
		{
			base.CurrentGame.InitializeDefaultGameObjects();
			this.DefaultItems = new DefaultItems();
			base.CurrentGame.LoadBasicFiles();
			base.ObjectManager.LoadXML("Items", false);
			base.ObjectManager.LoadXML("EquipmentRosters", false);
			base.ObjectManager.LoadXML("partyTemplates", false);
			WeaponDescription @object = MBObjectManager.Instance.GetObject<WeaponDescription>("OneHandedBastardSwordAlternative");
			if (@object != null)
			{
				@object.IsHiddenFromUI = true;
			}
			WeaponDescription object2 = MBObjectManager.Instance.GetObject<WeaponDescription>("OneHandedBastardAxeAlternative");
			if (object2 != null)
			{
				object2.IsHiddenFromUI = true;
			}
			this.DefaultIssueEffects = new DefaultIssueEffects();
			this.DefaultTraits = new DefaultTraits();
			this.DefaultPolicies = new DefaultPolicies();
			this.DefaultPerks = new DefaultPerks();
			this.DefaultBuildingTypes = new DefaultBuildingTypes();
			this.DefaultVillageTypes = new DefaultVillageTypes();
			this.DefaultSiegeStrategies = new DefaultSiegeStrategies();
			this.DefaultSkillEffects = new DefaultSkillEffects();
			this.DefaultFeats = new DefaultCulturalFeats();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00011374 File Offset: 0x0000F574
		private void InitializeManagers()
		{
			this.KingdomManager = new KingdomManager();
			this.CampaignInformationManager = new CampaignInformationManager();
			this.VisualTrackerManager = new VisualTrackerManager();
			this.TournamentManager = new TournamentManager();
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000113A4 File Offset: 0x0000F5A4
		private void InitializeCampaignObjectsOnAfterLoad()
		{
			this.CampaignObjectManager.InitializeOnLoad();
			this.FactionManager.AfterLoad();
			List<PerkObject> collection = (from x in this.AllPerks
			where !x.IsTrash
			select x).ToList<PerkObject>();
			this.AllPerks = new MBReadOnlyList<PerkObject>(collection);
			this.LogEntryHistory.OnAfterLoad();
			foreach (Kingdom kingdom in this.Kingdoms)
			{
				foreach (Army army in kingdom.Armies)
				{
					army.OnAfterLoad();
				}
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0001148C File Offset: 0x0000F68C
		private void OnNewCampaignStart()
		{
			Game.Current.PlayerTroop = null;
			this.MapStateData = new MapStateData();
			this.InitializeDefaultCampaignObjects();
			this.MainParty = MBObjectManager.Instance.CreateObject<MobileParty>("player_party");
			this.MainParty.Ai.SetAsMainParty();
			this.InitializeManagers();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000114E0 File Offset: 0x0000F6E0
		protected override void BeforeRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<FeatObject>("Feat", "Feats", 0U, true, false);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000114F8 File Offset: 0x0000F6F8
		protected override void OnRegisterTypes(MBObjectManager objectManager)
		{
			objectManager.RegisterType<MobileParty>("MobileParty", "MobileParties", 14U, true, true);
			objectManager.RegisterType<CharacterObject>("NPCCharacter", "NPCCharacters", 16U, true, false);
			if (this.GameMode == CampaignGameMode.Tutorial)
			{
				objectManager.RegisterType<BasicCharacterObject>("NPCCharacter", "MPCharacters", 43U, true, false);
			}
			objectManager.RegisterType<CultureObject>("Culture", "SPCultures", 17U, true, false);
			objectManager.RegisterType<Clan>("Faction", "Factions", 18U, true, true);
			objectManager.RegisterType<PerkObject>("Perk", "Perks", 19U, true, false);
			objectManager.RegisterType<Kingdom>("Kingdom", "Kingdoms", 20U, true, true);
			objectManager.RegisterType<TraitObject>("Trait", "Traits", 21U, true, false);
			objectManager.RegisterType<VillageType>("VillageType", "VillageTypes", 22U, true, false);
			objectManager.RegisterType<BuildingType>("BuildingType", "BuildingTypes", 23U, true, false);
			objectManager.RegisterType<PartyTemplateObject>("PartyTemplate", "partyTemplates", 24U, true, false);
			objectManager.RegisterType<Settlement>("Settlement", "Settlements", 25U, true, false);
			objectManager.RegisterType<WorkshopType>("WorkshopType", "WorkshopTypes", 26U, true, false);
			objectManager.RegisterType<Village>("Village", "Components", 27U, true, false);
			objectManager.RegisterType<Hideout>("Hideout", "Components", 30U, true, false);
			objectManager.RegisterType<Town>("Town", "Components", 31U, true, false);
			objectManager.RegisterType<Hero>("Hero", "Heroes", 32U, true, true);
			objectManager.RegisterType<MenuContext>("MenuContext", "MenuContexts", 35U, true, false);
			objectManager.RegisterType<PolicyObject>("Policy", "Policies", 36U, true, false);
			objectManager.RegisterType<Concept>("Concept", "Concepts", 37U, true, false);
			objectManager.RegisterType<IssueEffect>("IssueEffect", "IssueEffects", 39U, true, false);
			objectManager.RegisterType<SiegeStrategy>("SiegeStrategy", "SiegeStrategies", 40U, true, false);
			objectManager.RegisterType<SkillEffect>("SkillEffect", "SkillEffects", 53U, true, false);
			objectManager.RegisterType<LocationComplexTemplate>("LocationComplexTemplate", "LocationComplexTemplates", 42U, true, false);
			objectManager.RegisterType<RetirementSettlementComponent>("RetirementSettlementComponent", "Components", 56U, true, false);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00011704 File Offset: 0x0000F904
		private void CreateManagers()
		{
			this.EncyclopediaManager = new EncyclopediaManager();
			this.InventoryManager = new InventoryManager();
			this.PartyScreenManager = new PartyScreenManager();
			this.ConversationManager = new ConversationManager();
			this.NameGenerator = new NameGenerator();
			this.SkillLevelingManager = new DefaultSkillLevelingManager();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00011753 File Offset: 0x0000F953
		private void OnNewGameCreated(CampaignGameStarter gameStarter)
		{
			this.OnNewGameCreatedInternal();
			GameManagerBase gameManager = base.GameManager;
			if (gameManager != null)
			{
				gameManager.OnNewGameCreated(base.CurrentGame, gameStarter);
			}
			CampaignEventDispatcher.Instance.OnNewGameCreated(gameStarter);
			this.OnAfterNewGameCreatedInternal();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00011784 File Offset: 0x0000F984
		private void OnNewGameCreatedInternal()
		{
			this.UniqueGameId = MiscHelper.GenerateCampaignId(12);
			this._newGameVersion = MBSaveLoad.CurrentVersion.ToString();
			this.PlatformID = ApplicationPlatform.CurrentPlatform.ToString();
			this.PlayerTraitDeveloper = new HeroTraitDeveloper(Hero.MainHero);
			this.TimeControlMode = CampaignTimeControlMode.Stop;
			this._campaignEntitySystem = new EntitySystem<CampaignEntityComponent>();
			this.SiegeEventManager = new SiegeEventManager();
			this.MapEventManager = new MapEventManager();
			this.autoEnterTown = null;
			this.MinSettlementX = 1000f;
			this.MinSettlementY = 1000f;
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.Position2D.x < this.MinSettlementX)
				{
					this.MinSettlementX = settlement.Position2D.x;
				}
				if (settlement.Position2D.y < this.MinSettlementY)
				{
					this.MinSettlementY = settlement.Position2D.y;
				}
				if (settlement.Position2D.x > this.MaxSettlementX)
				{
					this.MaxSettlementX = settlement.Position2D.x;
				}
				if (settlement.Position2D.y > this.MaxSettlementY)
				{
					this.MaxSettlementY = settlement.Position2D.y;
				}
			}
			this.CampaignBehaviorManager.RegisterEvents();
			this.CameraFollowParty = this.MainParty.Party;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00011918 File Offset: 0x0000FB18
		private void OnAfterNewGameCreatedInternal()
		{
			Hero.MainHero.Gold = 1000;
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, -Clan.PlayerClan.Influence);
			Hero.MainHero.ChangeState(Hero.CharacterStates.Active);
			this.GameInitTick();
			this._playerFormationPreferences = new Dictionary<CharacterObject, FormationClass>();
			this.PlayerFormationPreferences = this._playerFormationPreferences.GetReadOnlyDictionary<CharacterObject, FormationClass>();
			Campaign.Current.DesertionEnabled = true;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00011984 File Offset: 0x0000FB84
		protected override void DoLoadingForGameType(GameTypeLoadingStates gameTypeLoadingState, out GameTypeLoadingStates nextState)
		{
			nextState = GameTypeLoadingStates.None;
			switch (gameTypeLoadingState)
			{
			case GameTypeLoadingStates.InitializeFirstStep:
				base.CurrentGame.Initialize();
				nextState = GameTypeLoadingStates.WaitSecondStep;
				return;
			case GameTypeLoadingStates.WaitSecondStep:
				nextState = GameTypeLoadingStates.LoadVisualsThirdState;
				return;
			case GameTypeLoadingStates.LoadVisualsThirdState:
				if (this.GameMode == CampaignGameMode.Campaign)
				{
					this.LoadMapScene();
				}
				nextState = GameTypeLoadingStates.PostInitializeFourthState;
				return;
			case GameTypeLoadingStates.PostInitializeFourthState:
			{
				CampaignGameStarter gameStarter = this.SandBoxManager.GameStarter;
				if (this._gameLoadingType == Campaign.GameLoadingType.SavedCampaign)
				{
					this.OnDataLoadFinished(gameStarter);
					this.CalculateCachedValues();
					this.DeterminedSavedStats(this._gameLoadingType);
					foreach (Settlement settlement in Settlement.All)
					{
						settlement.Party.OnGameInitialized();
					}
					foreach (MobileParty mobileParty in this.MobileParties.ToList<MobileParty>())
					{
						mobileParty.Party.OnGameInitialized();
					}
					this.CalculateCachedStatsOnLoad();
					this.OnGameLoaded(gameStarter);
					this.OnSessionStart(gameStarter);
					foreach (Hero hero in Hero.AllAliveHeroes)
					{
						hero.CheckInvalidEquipmentsAndReplaceIfNeeded();
					}
					using (List<Hero>.Enumerator enumerator3 = Hero.DeadOrDisabledHeroes.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Hero hero2 = enumerator3.Current;
							hero2.CheckInvalidEquipmentsAndReplaceIfNeeded();
						}
						goto IL_297;
					}
				}
				if (this._gameLoadingType == Campaign.GameLoadingType.NewCampaign)
				{
					this.OnDataLoadFinished(gameStarter);
					this.CalculateCachedValues();
					MBSaveLoad.OnNewGame();
					this.InitializeMainParty();
					this.DeterminedSavedStats(this._gameLoadingType);
					foreach (Settlement settlement2 in Settlement.All)
					{
						settlement2.Party.OnGameInitialized();
					}
					foreach (MobileParty mobileParty2 in this.MobileParties.ToList<MobileParty>())
					{
						mobileParty2.Party.OnGameInitialized();
					}
					foreach (Settlement settlement3 in Settlement.All)
					{
						settlement3.OnGameCreated();
					}
					foreach (Clan clan in Clan.All)
					{
						clan.OnGameCreated();
					}
					MBObjectManager.Instance.RemoveTemporaryTypes();
					this.OnNewGameCreated(gameStarter);
					this.OnSessionStart(gameStarter);
					Debug.Print("Finished starting a new game.", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				IL_297:
				base.GameManager.OnAfterGameInitializationFinished(base.CurrentGame, gameStarter);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00011CA0 File Offset: 0x0000FEA0
		private void DeterminedSavedStats(Campaign.GameLoadingType gameLoadingType)
		{
			if (this._previouslyUsedModules == null)
			{
				this._previouslyUsedModules = new MBList<string>();
			}
			foreach (string item in SandBoxManager.Instance.ModuleManager.ModuleNames)
			{
				if (!this._previouslyUsedModules.Contains(item))
				{
					this._previouslyUsedModules.Add(item);
				}
			}
			if (this._usedGameVersions == null)
			{
				this._usedGameVersions = new MBList<string>();
			}
			string text = MBSaveLoad.LastLoadedGameVersion.ToString();
			if (this._usedGameVersions.Count <= 0 || !this._usedGameVersions[this._usedGameVersions.Count - 1].Equals(text))
			{
				this._usedGameVersions.Add(text);
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00011D5E File Offset: 0x0000FF5E
		public override void OnMissionIsStarting(string missionName, MissionInitializerRecord rec)
		{
			if (rec.PlayingInCampaignMode)
			{
				CampaignEventDispatcher.Instance.BeforeMissionOpened();
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00011D72 File Offset: 0x0000FF72
		public override void InitializeParameters()
		{
			ManagedParameters.Instance.Initialize(ModuleHelper.GetXmlPath("Native", "managed_campaign_parameters"));
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00011D8D File Offset: 0x0000FF8D
		public void SetTimeControlModeLock(bool isLocked)
		{
			this.TimeControlModeLock = isLocked;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00011D96 File Offset: 0x0000FF96
		public override bool IsPartyWindowAccessibleAtMission
		{
			get
			{
				return this.GameMode == CampaignGameMode.Tutorial;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00011DA1 File Offset: 0x0000FFA1
		internal MBReadOnlyList<Town> AllTowns
		{
			get
			{
				return this._towns;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00011DA9 File Offset: 0x0000FFA9
		internal MBReadOnlyList<Town> AllCastles
		{
			get
			{
				return this._castles;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600028A RID: 650 RVA: 0x00011DB1 File Offset: 0x0000FFB1
		internal MBReadOnlyList<Village> AllVillages
		{
			get
			{
				return this._villages;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600028B RID: 651 RVA: 0x00011DB9 File Offset: 0x0000FFB9
		internal MBReadOnlyList<Hideout> AllHideouts
		{
			get
			{
				return this._hideouts;
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00011DC4 File Offset: 0x0000FFC4
		public void OnPlayerCharacterChanged(out bool isMainPartyChanged)
		{
			isMainPartyChanged = false;
			if (MobileParty.MainParty != Hero.MainHero.PartyBelongedTo)
			{
				isMainPartyChanged = true;
			}
			this.MainParty = Hero.MainHero.PartyBelongedTo;
			if (Hero.MainHero.CurrentSettlement != null && !Hero.MainHero.IsPrisoner)
			{
				if (this.MainParty == null)
				{
					LeaveSettlementAction.ApplyForCharacterOnly(Hero.MainHero);
				}
				else
				{
					LeaveSettlementAction.ApplyForParty(this.MainParty);
				}
			}
			if (Hero.MainHero.IsFugitive)
			{
				Hero.MainHero.ChangeState(Hero.CharacterStates.Active);
			}
			this.PlayerTraitDeveloper = new HeroTraitDeveloper(Hero.MainHero);
			if (this.MainParty == null)
			{
				this.MainParty = MobileParty.CreateParty("player_party_" + Hero.MainHero.StringId, new LordPartyComponent(Hero.MainHero, Hero.MainHero), delegate(MobileParty mobileParty)
				{
					mobileParty.ActualClan = Clan.PlayerClan;
				});
				isMainPartyChanged = true;
				if (Hero.MainHero.IsPrisoner)
				{
					this.MainParty.InitializeMobilePartyAtPosition(base.CurrentGame.ObjectManager.GetObject<PartyTemplateObject>("main_hero_party_template"), Hero.MainHero.GetPosition().AsVec2, 0);
					this.MainParty.IsActive = false;
				}
				else
				{
					Vec2 vec;
					if (!(Hero.MainHero.GetPosition().AsVec2 != Vec2.Zero))
					{
						vec = SettlementHelper.FindRandomSettlement((Settlement s) => s.OwnerClan != null && !s.OwnerClan.IsAtWarWith(Clan.PlayerClan)).GetPosition2D;
					}
					else
					{
						vec = Hero.MainHero.GetPosition().AsVec2;
					}
					Vec2 position = vec;
					this.MainParty.InitializeMobilePartyAtPosition(base.CurrentGame.ObjectManager.GetObject<PartyTemplateObject>("main_hero_party_template"), position, 0);
					this.MainParty.IsActive = true;
					this.MainParty.MemberRoster.AddToCounts(Hero.MainHero.CharacterObject, 1, true, 0, 0, true, -1);
				}
			}
			Campaign.Current.MainParty.Ai.SetAsMainParty();
			PartyBase.MainParty.ItemRoster.UpdateVersion();
			PartyBase.MainParty.MemberRoster.UpdateVersion();
			if (MobileParty.MainParty.IsActive)
			{
				PartyBase.MainParty.SetAsCameraFollowParty();
			}
			PartyBase.MainParty.UpdateVisibilityAndInspected(0f);
			if (Hero.MainHero.Mother != null)
			{
				Hero.MainHero.Mother.SetHasMet();
			}
			if (Hero.MainHero.Father != null)
			{
				Hero.MainHero.Father.SetHasMet();
			}
			this.MainParty.SetWagePaymentLimit(Campaign.Current.Models.PartyWageModel.MaxWage);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00012058 File Offset: 0x00010258
		public void SetPlayerFormationPreference(CharacterObject character, FormationClass formation)
		{
			if (!this._playerFormationPreferences.ContainsKey(character))
			{
				this._playerFormationPreferences.Add(character, formation);
				return;
			}
			this._playerFormationPreferences[character] = formation;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00012083 File Offset: 0x00010283
		public override void OnStateChanged(GameState oldState)
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00012085 File Offset: 0x00010285
		internal static void AutoGeneratedStaticCollectObjectsCampaign(object o, List<object> collectedObjects)
		{
			((Campaign)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00012094 File Offset: 0x00010294
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Options);
			collectedObjects.Add(this.TournamentManager);
			collectedObjects.Add(this.autoEnterTown);
			collectedObjects.Add(this.BusyHideouts);
			collectedObjects.Add(this.KingdomManager);
			collectedObjects.Add(this._campaignPeriodicEventManager);
			collectedObjects.Add(this._previouslyUsedModules);
			collectedObjects.Add(this._usedGameVersions);
			collectedObjects.Add(this._playerFormationPreferences);
			collectedObjects.Add(this._campaignBehaviorManager);
			collectedObjects.Add(this._cameraFollowParty);
			collectedObjects.Add(this._logEntryHistory);
			collectedObjects.Add(this.CampaignObjectManager);
			collectedObjects.Add(this.QuestManager);
			collectedObjects.Add(this.IssueManager);
			collectedObjects.Add(this.FactionManager);
			collectedObjects.Add(this.CharacterRelationManager);
			collectedObjects.Add(this.Romance);
			collectedObjects.Add(this.PlayerCaptivity);
			collectedObjects.Add(this.PlayerDefaultFaction);
			collectedObjects.Add(this.MapStateData);
			collectedObjects.Add(this.MapTimeTracker);
			CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.CampaignStartTime, collectedObjects);
			collectedObjects.Add(this.SiegeEventManager);
			collectedObjects.Add(this.MapEventManager);
			collectedObjects.Add(this.PlayerEncounter);
			collectedObjects.Add(this.BarterManager);
			collectedObjects.Add(this.MainParty);
			collectedObjects.Add(this.CampaignInformationManager);
			collectedObjects.Add(this.VisualTrackerManager);
			collectedObjects.Add(this.PlayerTraitDeveloper);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00012221 File Offset: 0x00010421
		internal static object AutoGeneratedGetMemberValueEnabledCheatsBefore(object o)
		{
			return ((Campaign)o).EnabledCheatsBefore;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00012233 File Offset: 0x00010433
		internal static object AutoGeneratedGetMemberValuePlatformID(object o)
		{
			return ((Campaign)o).PlatformID;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00012240 File Offset: 0x00010440
		internal static object AutoGeneratedGetMemberValueUniqueGameId(object o)
		{
			return ((Campaign)o).UniqueGameId;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0001224D File Offset: 0x0001044D
		internal static object AutoGeneratedGetMemberValueCampaignObjectManager(object o)
		{
			return ((Campaign)o).CampaignObjectManager;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0001225A File Offset: 0x0001045A
		internal static object AutoGeneratedGetMemberValueIsCraftingEnabled(object o)
		{
			return ((Campaign)o).IsCraftingEnabled;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0001226C File Offset: 0x0001046C
		internal static object AutoGeneratedGetMemberValueIsBannerEditorEnabled(object o)
		{
			return ((Campaign)o).IsBannerEditorEnabled;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0001227E File Offset: 0x0001047E
		internal static object AutoGeneratedGetMemberValueIsFaceGenEnabled(object o)
		{
			return ((Campaign)o).IsFaceGenEnabled;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00012290 File Offset: 0x00010490
		internal static object AutoGeneratedGetMemberValueQuestManager(object o)
		{
			return ((Campaign)o).QuestManager;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0001229D File Offset: 0x0001049D
		internal static object AutoGeneratedGetMemberValueIssueManager(object o)
		{
			return ((Campaign)o).IssueManager;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000122AA File Offset: 0x000104AA
		internal static object AutoGeneratedGetMemberValueFactionManager(object o)
		{
			return ((Campaign)o).FactionManager;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000122B7 File Offset: 0x000104B7
		internal static object AutoGeneratedGetMemberValueCharacterRelationManager(object o)
		{
			return ((Campaign)o).CharacterRelationManager;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000122C4 File Offset: 0x000104C4
		internal static object AutoGeneratedGetMemberValueRomance(object o)
		{
			return ((Campaign)o).Romance;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000122D1 File Offset: 0x000104D1
		internal static object AutoGeneratedGetMemberValuePlayerCaptivity(object o)
		{
			return ((Campaign)o).PlayerCaptivity;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000122DE File Offset: 0x000104DE
		internal static object AutoGeneratedGetMemberValuePlayerDefaultFaction(object o)
		{
			return ((Campaign)o).PlayerDefaultFaction;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000122EB File Offset: 0x000104EB
		internal static object AutoGeneratedGetMemberValueMapStateData(object o)
		{
			return ((Campaign)o).MapStateData;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000122F8 File Offset: 0x000104F8
		internal static object AutoGeneratedGetMemberValueMapTimeTracker(object o)
		{
			return ((Campaign)o).MapTimeTracker;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00012305 File Offset: 0x00010505
		internal static object AutoGeneratedGetMemberValueCampaignStartTime(object o)
		{
			return ((Campaign)o).CampaignStartTime;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00012317 File Offset: 0x00010517
		internal static object AutoGeneratedGetMemberValueGameMode(object o)
		{
			return ((Campaign)o).GameMode;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00012329 File Offset: 0x00010529
		internal static object AutoGeneratedGetMemberValuePlayerProgress(object o)
		{
			return ((Campaign)o).PlayerProgress;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0001233B File Offset: 0x0001053B
		internal static object AutoGeneratedGetMemberValueSiegeEventManager(object o)
		{
			return ((Campaign)o).SiegeEventManager;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00012348 File Offset: 0x00010548
		internal static object AutoGeneratedGetMemberValueMapEventManager(object o)
		{
			return ((Campaign)o).MapEventManager;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00012355 File Offset: 0x00010555
		internal static object AutoGeneratedGetMemberValue_curMapFrame(object o)
		{
			return ((Campaign)o)._curMapFrame;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00012367 File Offset: 0x00010567
		internal static object AutoGeneratedGetMemberValuePlayerEncounter(object o)
		{
			return ((Campaign)o).PlayerEncounter;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00012374 File Offset: 0x00010574
		internal static object AutoGeneratedGetMemberValueBarterManager(object o)
		{
			return ((Campaign)o).BarterManager;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00012381 File Offset: 0x00010581
		internal static object AutoGeneratedGetMemberValueIsMainHeroDisguised(object o)
		{
			return ((Campaign)o).IsMainHeroDisguised;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00012393 File Offset: 0x00010593
		internal static object AutoGeneratedGetMemberValueDesertionEnabled(object o)
		{
			return ((Campaign)o).DesertionEnabled;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000123A5 File Offset: 0x000105A5
		internal static object AutoGeneratedGetMemberValueMainParty(object o)
		{
			return ((Campaign)o).MainParty;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000123B2 File Offset: 0x000105B2
		internal static object AutoGeneratedGetMemberValueCampaignInformationManager(object o)
		{
			return ((Campaign)o).CampaignInformationManager;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000123BF File Offset: 0x000105BF
		internal static object AutoGeneratedGetMemberValueVisualTrackerManager(object o)
		{
			return ((Campaign)o).VisualTrackerManager;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000123CC File Offset: 0x000105CC
		internal static object AutoGeneratedGetMemberValuePlayerTraitDeveloper(object o)
		{
			return ((Campaign)o).PlayerTraitDeveloper;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000123D9 File Offset: 0x000105D9
		internal static object AutoGeneratedGetMemberValueOptions(object o)
		{
			return ((Campaign)o).Options;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000123E6 File Offset: 0x000105E6
		internal static object AutoGeneratedGetMemberValueTournamentManager(object o)
		{
			return ((Campaign)o).TournamentManager;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000123F3 File Offset: 0x000105F3
		internal static object AutoGeneratedGetMemberValueIsInitializedSinglePlayerReferences(object o)
		{
			return ((Campaign)o).IsInitializedSinglePlayerReferences;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00012405 File Offset: 0x00010605
		internal static object AutoGeneratedGetMemberValueLastTimeControlMode(object o)
		{
			return ((Campaign)o).LastTimeControlMode;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00012417 File Offset: 0x00010617
		internal static object AutoGeneratedGetMemberValueautoEnterTown(object o)
		{
			return ((Campaign)o).autoEnterTown;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00012424 File Offset: 0x00010624
		internal static object AutoGeneratedGetMemberValueMainHeroIllDays(object o)
		{
			return ((Campaign)o).MainHeroIllDays;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00012436 File Offset: 0x00010636
		internal static object AutoGeneratedGetMemberValueBusyHideouts(object o)
		{
			return ((Campaign)o).BusyHideouts;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00012443 File Offset: 0x00010643
		internal static object AutoGeneratedGetMemberValueKingdomManager(object o)
		{
			return ((Campaign)o).KingdomManager;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00012450 File Offset: 0x00010650
		internal static object AutoGeneratedGetMemberValue_campaignPeriodicEventManager(object o)
		{
			return ((Campaign)o)._campaignPeriodicEventManager;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0001245D File Offset: 0x0001065D
		internal static object AutoGeneratedGetMemberValue_isMainPartyWaiting(object o)
		{
			return ((Campaign)o)._isMainPartyWaiting;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0001246F File Offset: 0x0001066F
		internal static object AutoGeneratedGetMemberValue_newGameVersion(object o)
		{
			return ((Campaign)o)._newGameVersion;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0001247C File Offset: 0x0001067C
		internal static object AutoGeneratedGetMemberValue_previouslyUsedModules(object o)
		{
			return ((Campaign)o)._previouslyUsedModules;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00012489 File Offset: 0x00010689
		internal static object AutoGeneratedGetMemberValue_usedGameVersions(object o)
		{
			return ((Campaign)o)._usedGameVersions;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00012496 File Offset: 0x00010696
		internal static object AutoGeneratedGetMemberValue_playerFormationPreferences(object o)
		{
			return ((Campaign)o)._playerFormationPreferences;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000124A3 File Offset: 0x000106A3
		internal static object AutoGeneratedGetMemberValue_campaignBehaviorManager(object o)
		{
			return ((Campaign)o)._campaignBehaviorManager;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000124B0 File Offset: 0x000106B0
		internal static object AutoGeneratedGetMemberValue_lastPartyIndex(object o)
		{
			return ((Campaign)o)._lastPartyIndex;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x000124C2 File Offset: 0x000106C2
		internal static object AutoGeneratedGetMemberValue_cameraFollowParty(object o)
		{
			return ((Campaign)o)._cameraFollowParty;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000124CF File Offset: 0x000106CF
		internal static object AutoGeneratedGetMemberValue_logEntryHistory(object o)
		{
			return ((Campaign)o)._logEntryHistory;
		}

		// Token: 0x0400004F RID: 79
		public const float ConfigTimeMultiplier = 0.25f;

		// Token: 0x04000050 RID: 80
		private EntitySystem<CampaignEntityComponent> _campaignEntitySystem;

		// Token: 0x04000057 RID: 87
		public ITask CampaignLateAITickTask;

		// Token: 0x04000058 RID: 88
		[SaveableField(210)]
		private CampaignPeriodicEventManager _campaignPeriodicEventManager;

		// Token: 0x0400005B RID: 91
		[SaveableField(53)]
		private bool _isMainPartyWaiting;

		// Token: 0x0400005C RID: 92
		[SaveableField(344)]
		private string _newGameVersion;

		// Token: 0x0400005D RID: 93
		[SaveableField(78)]
		private MBList<string> _previouslyUsedModules;

		// Token: 0x0400005E RID: 94
		[SaveableField(81)]
		private MBList<string> _usedGameVersions;

		// Token: 0x04000064 RID: 100
		[SaveableField(77)]
		private Dictionary<CharacterObject, FormationClass> _playerFormationPreferences;

		// Token: 0x04000065 RID: 101
		[SaveableField(7)]
		private ICampaignBehaviorManager _campaignBehaviorManager;

		// Token: 0x04000067 RID: 103
		private CampaignTickCacheDataStore _tickData;

		// Token: 0x04000068 RID: 104
		[SaveableField(2)]
		public readonly CampaignOptions Options;

		// Token: 0x04000069 RID: 105
		public MBReadOnlyDictionary<CharacterObject, FormationClass> PlayerFormationPreferences;

		// Token: 0x0400006A RID: 106
		[SaveableField(13)]
		public ITournamentManager TournamentManager;

		// Token: 0x0400006B RID: 107
		public float MinSettlementX;

		// Token: 0x0400006C RID: 108
		public float MaxSettlementX;

		// Token: 0x0400006D RID: 109
		public float MinSettlementY;

		// Token: 0x0400006E RID: 110
		public float MaxSettlementY;

		// Token: 0x0400006F RID: 111
		[SaveableField(27)]
		public bool IsInitializedSinglePlayerReferences;

		// Token: 0x04000070 RID: 112
		private LocatorGrid<MobileParty> _mobilePartyLocator;

		// Token: 0x04000071 RID: 113
		private LocatorGrid<Settlement> _settlementLocator;

		// Token: 0x04000072 RID: 114
		private GameModels _gameModels;

		// Token: 0x04000075 RID: 117
		[SaveableField(31)]
		public CampaignTimeControlMode LastTimeControlMode = CampaignTimeControlMode.UnstoppablePlay;

		// Token: 0x04000076 RID: 118
		private IMapScene _mapSceneWrapper;

		// Token: 0x04000077 RID: 119
		public bool GameStarted;

		// Token: 0x04000079 RID: 121
		[SaveableField(44)]
		public PartyBase autoEnterTown;

		// Token: 0x0400007A RID: 122
		private Campaign.GameLoadingType _gameLoadingType;

		// Token: 0x0400007B RID: 123
		public ConversationContext CurrentConversationContext;

		// Token: 0x0400007C RID: 124
		[CachedData]
		private float _dt;

		// Token: 0x0400007F RID: 127
		private CampaignTimeControlMode _timeControlMode;

		// Token: 0x04000080 RID: 128
		private int _curSessionFrame;

		// Token: 0x040000A5 RID: 165
		[SaveableField(30)]
		public int MainHeroIllDays = -1;

		// Token: 0x040000B5 RID: 181
		private MBCampaignEvent _dailyTickEvent;

		// Token: 0x040000B6 RID: 182
		private MBCampaignEvent _hourlyTickEvent;

		// Token: 0x040000B8 RID: 184
		[CachedData]
		private int _lastNonZeroDtFrame;

		// Token: 0x040000BF RID: 191
		[SaveableField(84)]
		public List<Settlement> BusyHideouts = new List<Settlement>();

		// Token: 0x040000C2 RID: 194
		private MBList<Town> _towns;

		// Token: 0x040000C3 RID: 195
		private MBList<Town> _castles;

		// Token: 0x040000C4 RID: 196
		private MBList<Village> _villages;

		// Token: 0x040000C5 RID: 197
		private MBList<Hideout> _hideouts;

		// Token: 0x040000C6 RID: 198
		private MBReadOnlyList<CharacterObject> _characters;

		// Token: 0x040000C7 RID: 199
		private MBReadOnlyList<WorkshopType> _workshops;

		// Token: 0x040000C8 RID: 200
		private MBReadOnlyList<ItemModifier> _itemModifiers;

		// Token: 0x040000C9 RID: 201
		private MBReadOnlyList<Concept> _concepts;

		// Token: 0x040000CA RID: 202
		private MBReadOnlyList<ItemModifierGroup> _itemModifierGroups;

		// Token: 0x040000CB RID: 203
		[SaveableField(79)]
		private int _lastPartyIndex;

		// Token: 0x040000CD RID: 205
		[SaveableField(61)]
		private PartyBase _cameraFollowParty;

		// Token: 0x040000D0 RID: 208
		[SaveableField(64)]
		private readonly LogEntryHistory _logEntryHistory = new LogEntryHistory();

		// Token: 0x040000D5 RID: 213
		[SaveableField(65)]
		public KingdomManager KingdomManager;

		// Token: 0x0200047C RID: 1148
		[Flags]
		public enum PartyRestFlags : uint
		{
			// Token: 0x0400137B RID: 4987
			None = 0U,
			// Token: 0x0400137C RID: 4988
			SafeMode = 1U
		}

		// Token: 0x0200047D RID: 1149
		public enum GameLoadingType
		{
			// Token: 0x0400137E RID: 4990
			Tutorial,
			// Token: 0x0400137F RID: 4991
			NewCampaign,
			// Token: 0x04001380 RID: 4992
			SavedCampaign,
			// Token: 0x04001381 RID: 4993
			Editor
		}
	}
}
