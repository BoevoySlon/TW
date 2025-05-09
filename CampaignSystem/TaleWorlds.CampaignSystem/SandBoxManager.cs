﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000099 RID: 153
	public class SandBoxManager : GameHandler
	{
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x0004E096 File Offset: 0x0004C296
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x0004E09E File Offset: 0x0004C29E
		public ISandBoxMissionManager SandBoxMissionManager { get; set; }

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x0004E0A7 File Offset: 0x0004C2A7
		// (set) Token: 0x06001157 RID: 4439 RVA: 0x0004E0AF File Offset: 0x0004C2AF
		public IAgentBehaviorManager AgentBehaviorManager { get; set; }

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x0004E0B8 File Offset: 0x0004C2B8
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x0004E0C0 File Offset: 0x0004C2C0
		public IModuleManager ModuleManager { get; set; }

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x0004E0C9 File Offset: 0x0004C2C9
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x0004E0D1 File Offset: 0x0004C2D1
		public ISaveManager SandBoxSaveManager { get; set; }

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x0004E0DA File Offset: 0x0004C2DA
		public static SandBoxManager Instance
		{
			get
			{
				return Game.Current.GetGameHandler<SandBoxManager>();
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x0004E0E6 File Offset: 0x0004C2E6
		// (set) Token: 0x0600115E RID: 4446 RVA: 0x0004E0EE File Offset: 0x0004C2EE
		public CampaignGameStarter GameStarter { get; private set; }

		// Token: 0x0600115F RID: 4447 RVA: 0x0004E0F8 File Offset: 0x0004C2F8
		public void Initialize(CampaignGameStarter gameStarter)
		{
			this.GameStarter = gameStarter;
			gameStarter.AddBehavior(new PartyUpgraderCampaignBehavior());
			gameStarter.AddBehavior(new EncounterGameMenuBehavior());
			gameStarter.AddBehavior(new PlayerCaptivityCampaignBehavior());
			gameStarter.AddBehavior(new BackstoryCampaignBehavior());
			gameStarter.AddBehavior(new TradeCampaignBehavior());
			gameStarter.AddBehavior(new BanditsCampaignBehavior());
			gameStarter.AddBehavior(new CharacterRelationCampaignBehavior());
			gameStarter.AddBehavior(new DesertionCampaignBehavior());
			gameStarter.AddBehavior(new FoodConsumptionBehavior());
			gameStarter.AddBehavior(new FindingItemOnMapBehavior());
			gameStarter.AddBehavior(new BuildingsCampaignBehavior());
			gameStarter.AddBehavior(new ItemConsumptionBehavior());
			gameStarter.AddBehavior(new GarrisonTroopsCampaignBehavior());
			gameStarter.AddBehavior(new CaravansCampaignBehavior());
			gameStarter.AddBehavior(new GovernorCampaignBehavior());
			gameStarter.AddBehavior(new HideoutCampaignBehavior());
			gameStarter.AddBehavior(new PartiesBuyFoodCampaignBehavior());
			gameStarter.AddBehavior(new PartiesBuyHorseCampaignBehavior());
			gameStarter.AddBehavior(new PoliticalStagnationAndBorderIncidentCampaignBehavior());
			gameStarter.AddBehavior(new PrisonerReleaseCampaignBehavior());
			gameStarter.AddBehavior(new PrisonerCaptureCampaignBehavior());
			gameStarter.AddBehavior(new PrisonerRecruitCampaignBehavior());
			gameStarter.AddBehavior(new RomanceCampaignBehavior());
			gameStarter.AddBehavior(new LordDefectionCampaignBehavior());
			gameStarter.AddBehavior(new PartiesSellPrisonerCampaignBehavior());
			gameStarter.AddBehavior(new PartiesSellLootCampaignBehavior());
			gameStarter.AddBehavior(new SettlementVariablesBehavior());
			gameStarter.AddBehavior(new MilitiasCampaignBehavior());
			gameStarter.AddBehavior(new SettlementClaimantCampaignBehavior());
			gameStarter.AddBehavior(new TradeRumorsCampaignBehavior());
			gameStarter.AddBehavior(new NotablesCampaignBehavior());
			gameStarter.AddBehavior(new LordConversationsCampaignBehavior());
			gameStarter.AddBehavior(new CompanionsCampaignBehavior());
			gameStarter.AddBehavior(new RetrainOutlawPartyMembersBehavior());
			gameStarter.AddBehavior(new RecruitPrisonersCampaignBehavior());
			gameStarter.AddBehavior(new HeroSpawnCampaignBehavior());
			gameStarter.AddBehavior(new TournamentCampaignBehavior());
			gameStarter.AddBehavior(new CraftingCampaignBehavior());
			gameStarter.AddBehavior(new MapTracksCampaignBehavior());
			gameStarter.AddBehavior(new HeroAgentSpawnCampaignBehavior());
			gameStarter.AddBehavior(new CharacterDevelopmentCampaignBehavior());
			gameStarter.AddBehavior(new TradeSkillCampaingBehavior());
			gameStarter.AddBehavior(new TavernEmployeesCampaignBehavior());
			gameStarter.AddBehavior(new RecruitmentCampaignBehavior());
			gameStarter.AddBehavior(new VillageHostileActionCampaignBehavior());
			gameStarter.AddBehavior(new PlayerTownVisitCampaignBehavior());
			gameStarter.AddBehavior(new PrisonBreakCampaignBehavior());
			gameStarter.AddBehavior(new DynamicBodyCampaignBehavior());
			gameStarter.AddBehavior(new TownMerchantsCampaignBehavior());
			gameStarter.AddBehavior(new OutlawClansCampaignBehavior());
			gameStarter.AddBehavior(new VillageTradeBoundCampaignBehavior());
			gameStarter.AddBehavior(new VillageGoodProductionCampaignBehavior());
			gameStarter.AddBehavior(new SiegeAftermathCampaignBehavior());
			gameStarter.AddBehavior(new NPCEquipmentsCampaignBehavior());
			gameStarter.AddBehavior(new VillagerCampaignBehavior());
			gameStarter.AddBehavior(new VillageHealCampaignBehavior());
			gameStarter.AddBehavior(new PlayerVariablesBehavior());
			gameStarter.AddBehavior(new MobilePartyTrainingBehavior());
			gameStarter.AddBehavior(new EducationCampaignBehavior());
			gameStarter.AddBehavior(new RansomOfferCampaignBehavior());
			gameStarter.AddBehavior(new PeaceOfferCampaignBehavior());
			gameStarter.AddBehavior(new MarriageOfferCampaignBehavior());
			gameStarter.AddBehavior(new VassalAndMercenaryOfferCampaignBehavior());
			gameStarter.AddBehavior(new CommentOnLeaveFactionBehavior());
			gameStarter.AddBehavior(new CommentOnChangeRomanticStateBehavior());
			gameStarter.AddBehavior(new CommentOnChangeSettlementOwnerBehavior());
			gameStarter.AddBehavior(new CommentOnPlayerMeetLordBehavior());
			gameStarter.AddBehavior(new CommentOnEndPlayerBattleBehavior());
			gameStarter.AddBehavior(new CommentOnDefeatCharacterBehavior());
			gameStarter.AddBehavior(new CommentOnCharacterKilledBehavior());
			gameStarter.AddBehavior(new CommentOnChangeVillageStateBehavior());
			gameStarter.AddBehavior(new CommentOnDestroyMobilePartyBehavior());
			gameStarter.AddBehavior(new CommentOnMakePeaceBehavior());
			gameStarter.AddBehavior(new CommentOnDeclareWarBehavior());
			gameStarter.AddBehavior(new CommentOnKingdomDestroyedBehavior());
			gameStarter.AddBehavior(new CommentOnClanDestroyedBehavior());
			gameStarter.AddBehavior(new CommentOnClanLeaderChangedBehavior());
			gameStarter.AddBehavior(new CommentPregnancyBehavior());
			gameStarter.AddBehavior(new CommentChildbirthBehavior());
			gameStarter.AddBehavior(new CommentCharacterBornBehavior());
			gameStarter.AddBehavior(new DefaultLogsCampaignBehavior());
			gameStarter.AddBehavior(new JournalLogsCampaignBehavior());
			gameStarter.AddBehavior(new ViewDataTrackerCampaignBehavior());
			gameStarter.AddBehavior(new AiArmyMemberBehavior());
			gameStarter.AddBehavior(new AiMilitaryBehavior());
			gameStarter.AddBehavior(new AiPatrollingBehavior());
			gameStarter.AddBehavior(new AiEngagePartyBehavior());
			gameStarter.AddBehavior(new AiBanditPatrollingBehavior());
			gameStarter.AddBehavior(new AiVisitSettlementBehavior());
			gameStarter.AddBehavior(new AiPartyThinkBehavior());
			gameStarter.AddBehavior(new DiplomaticBartersBehavior());
			gameStarter.AddBehavior(new SetPrisonerFreeBarterBehavior());
			gameStarter.AddBehavior(new FiefBarterBehavior());
			gameStarter.AddBehavior(new ItemBarterBehavior());
			gameStarter.AddBehavior(new GoldBarterBehavior());
			gameStarter.AddBehavior(new TransferPrisonerBarterBehavior());
			gameStarter.AddBehavior(new CompanionGrievanceBehavior());
			gameStarter.AddBehavior(new PlayerTrackCompanionBehavior());
			gameStarter.AddBehavior(new RebellionsCampaignBehavior());
			gameStarter.AddBehavior(new SallyOutsCampaignBehavior());
			gameStarter.AddBehavior(new CrimeCampaignBehavior());
			gameStarter.AddBehavior(new PlayerArmyWaitBehavior());
			gameStarter.AddBehavior(new ClanVariablesCampaignBehavior());
			gameStarter.AddBehavior(new FactionDiscontinuationCampaignBehavior());
			gameStarter.AddBehavior(new AgingCampaignBehavior());
			gameStarter.AddBehavior(new BattleCampaignBehavior());
			gameStarter.AddBehavior(new WorkshopsCampaignBehavior());
			gameStarter.AddBehavior(new PregnancyCampaignBehavior());
			gameStarter.AddBehavior(new InitialChildGenerationCampaignBehavior());
			gameStarter.AddBehavior(new NotablePowerManagementBehavior());
			gameStarter.AddBehavior(new PerkActivationHandlerCampaignBehavior());
			gameStarter.AddBehavior(new TownSecurityCampaignBehavior());
			gameStarter.AddBehavior(new HeroKnownInformationCampaignBehavior());
			gameStarter.AddBehavior(new DisbandPartyCampaignBehavior());
			gameStarter.AddBehavior(new PartyHealCampaignBehavior());
			gameStarter.AddBehavior(new CampaignBattleRecoveryBehavior());
			gameStarter.AddBehavior(new CampaignWarManagerBehavior());
			gameStarter.AddBehavior(new KingdomDecisionProposalBehavior());
			gameStarter.AddBehavior(new PartyRolesCampaignBehavior());
			gameStarter.AddBehavior(new EmissarySystemCampaignBehavior());
			gameStarter.AddBehavior(new SiegeEventCampaignBehavior());
			gameStarter.AddBehavior(new IssuesCampaignBehavior());
			gameStarter.AddBehavior(new InfluenceGainCampaignBehavior());
			gameStarter.AddBehavior(new BannerCampaignBehavior());
			gameStarter.AddBehavior(new TeleportationCampaignBehavior());
			gameStarter.AddBehavior(new ArmyNeedsSuppliesIssueBehavior());
			gameStarter.AddBehavior(new ArtisanCantSellProductsAtAFairPriceIssueBehavior());
			gameStarter.AddBehavior(new ArtisanOverpricedGoodsIssueBehavior());
			gameStarter.AddBehavior(new CapturedByBountyHuntersIssueBehavior());
			gameStarter.AddBehavior(new CaravanAmbushIssueBehavior());
			gameStarter.AddBehavior(new EscortMerchantCaravanIssueBehavior());
			gameStarter.AddBehavior(new ExtortionByDesertersIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsToOffloadStolenGoodsIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsWeaponsIssueQuestBehavior());
			gameStarter.AddBehavior(new RevenueFarmingIssueBehavior());
			gameStarter.AddBehavior(new HeadmanNeedsGrainIssueBehavior());
			gameStarter.AddBehavior(new HeadmanNeedsToDeliverAHerdIssueBehavior());
			gameStarter.AddBehavior(new HeadmanVillageNeedsDraughtAnimalsIssueBehavior());
			gameStarter.AddBehavior(new LadysKnightOutIssueBehavior());
			gameStarter.AddBehavior(new LandLordCompanyOfTroubleIssueBehavior());
			gameStarter.AddBehavior(new LandLordTheArtOfTheTradeIssueBehavior());
			gameStarter.AddBehavior(new LandlordNeedsAccessToVillageCommonsIssueBehavior());
			gameStarter.AddBehavior(new LandLordNeedsManualLaborersIssueBehavior());
			gameStarter.AddBehavior(new LandlordTrainingForRetainersIssueBehavior());
			gameStarter.AddBehavior(new LordNeedsGarrisonTroopsIssueQuestBehavior());
			gameStarter.AddBehavior(new TheConquestOfSettlementIssueBehavior());
			gameStarter.AddBehavior(new VillageNeedsCraftingMaterialsIssueBehavior());
			gameStarter.AddBehavior(new SmugglersIssueBehavior());
			gameStarter.AddBehavior(new LordNeedsHorsesIssueBehavior());
			gameStarter.AddBehavior(new LordsNeedsTutorIssueBehavior());
			gameStarter.AddBehavior(new LordWantsRivalCapturedIssueBehavior());
			gameStarter.AddBehavior(new MerchantArmyOfPoachersIssueBehavior());
			gameStarter.AddBehavior(new MerchantNeedsHelpWithOutlawsIssueQuestBehavior());
			gameStarter.AddBehavior(new NearbyBanditBaseIssueBehavior());
			gameStarter.AddBehavior(new RaidAnEnemyTerritoryIssueBehavior());
			gameStarter.AddBehavior(new ScoutEnemyGarrisonsIssueBehavior());
			gameStarter.AddBehavior(new VillageNeedsToolsIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsRecruitsIssueBehavior());
			gameStarter.AddBehavior(new GangLeaderNeedsSpecialWeaponsIssueBehavior());
			gameStarter.AddBehavior(new LesserNobleRevoltIssueBehavior());
			gameStarter.AddBehavior(new BettingFraudIssueBehavior());
			gameStarter.AddBehavior(new DiscardItemsCampaignBehavior());
			gameStarter.AddBehavior(new OrderOfBattleCampaignBehavior());
			gameStarter.AddBehavior(new DisorganizedStateCampaignBehavior());
			gameStarter.AddBehavior(new PerkResetCampaignBehavior());
			gameStarter.AddBehavior(new SiegeAmbushCampaignBehavior());
			gameStarter.AddBehavior(new MapWeatherCampaignBehavior());
			gameStarter.AddBehavior(new PartyDiplomaticHandlerCampaignBehavior());
			gameStarter.AddModel(new DefaultCharacterDevelopmentModel());
			gameStarter.AddModel(new DefaultValuationModel());
			gameStarter.AddModel(new DefaultItemDiscardModel());
			gameStarter.AddModel(new DefaultMapVisibilityModel());
			gameStarter.AddModel(new DefaultInformationRestrictionModel());
			gameStarter.AddModel(new DefaultMapDistanceModel());
			gameStarter.AddModel(new DefaultPartyHealingModel());
			gameStarter.AddModel(new DefaultPartyTrainingModel());
			gameStarter.AddModel(new DefaultPartyTradeModel());
			gameStarter.AddModel(new DefaultRansomValueCalculationModel());
			gameStarter.AddModel(new DefaultRaidModel());
			gameStarter.AddModel(new DefaultCombatSimulationModel());
			gameStarter.AddModel(new DefaultCombatXpModel());
			gameStarter.AddModel(new DefaultGenericXpModel());
			gameStarter.AddModel(new DefaultSmithingModel());
			gameStarter.AddModel(new DefaultPartySpeedCalculatingModel());
			gameStarter.AddModel(new DefaultPartyImpairmentModel());
			gameStarter.AddModel(new DefaultCharacterStatsModel());
			gameStarter.AddModel(new DefaultEncounterModel());
			gameStarter.AddModel(new DefaultMobilePartyFoodConsumptionModel());
			gameStarter.AddModel(new DefaultPartyFoodBuyingModel());
			gameStarter.AddModel(new DefaultPartyMoraleModel());
			gameStarter.AddModel(new DefaultDiplomacyModel());
			gameStarter.AddModel(new DefaultKingdomCreationModel());
			gameStarter.AddModel(new DefaultVillageProductionCalculatorModel());
			gameStarter.AddModel(new DefaultVolunteerModel());
			gameStarter.AddModel(new DefaultArmyManagementCalculationModel());
			gameStarter.AddModel(new DefaultBanditDensityModel());
			gameStarter.AddModel(new DefaultNotableSpawnModel());
			gameStarter.AddModel(new DefaultEncounterGameMenuModel());
			gameStarter.AddModel(new DefaultBattleRewardModel());
			gameStarter.AddModel(new DefaultRomanceModel());
			gameStarter.AddModel(new DefaultMapTrackModel());
			gameStarter.AddModel(new DefaultMapWeatherModel());
			gameStarter.AddModel(new DefaultRidingModel());
			gameStarter.AddModel(new DefaultTargetScoreCalculatingModel());
			gameStarter.AddModel(new DefaultCrimeModel());
			gameStarter.AddModel(new DefaultDisguiseDetectionModel());
			gameStarter.AddModel(new DefaultBribeCalculationModel());
			gameStarter.AddModel(new DefaultTroopSacrificeModel());
			gameStarter.AddModel(new DefaultSettlementAccessModel());
			gameStarter.AddModel(new DefaultKingdomDecisionPermissionModel());
			gameStarter.AddModel(new DefaultEmissaryModel());
			gameStarter.AddModel(new DefaultMilitaryPowerModel());
			gameStarter.AddModel(new DefaultPartySizeLimitModel());
			gameStarter.AddModel(new DefaultPartyWageModel());
			gameStarter.AddModel(new DefaultPartyDesertionModel());
			gameStarter.AddModel(new DefaultInventoryCapacityModel());
			gameStarter.AddModel(new DefaultItemCategorySelector());
			gameStarter.AddModel(new DefaultItemValueModel());
			gameStarter.AddModel(new DefaultTradeItemPriceFactorModel());
			gameStarter.AddModel(new DefaultSettlementValueModel());
			gameStarter.AddModel(new DefaultSettlementMilitiaModel());
			gameStarter.AddModel(new DefaultSettlementEconomyModel());
			gameStarter.AddModel(new DefaultSettlementFoodModel());
			gameStarter.AddModel(new DefaultSettlementLoyaltyModel());
			gameStarter.AddModel(new DefaultSettlementSecurityModel());
			gameStarter.AddModel(new DefaultSettlementProsperityModel());
			gameStarter.AddModel(new DefaultSettlementGarrisonModel());
			gameStarter.AddModel(new DefaultSettlementTaxModel());
			gameStarter.AddModel(new DefaultBarterModel());
			gameStarter.AddModel(new DefaultPersuasionModel());
			gameStarter.AddModel(new DefaultClanTierModel());
			gameStarter.AddModel(new DefaultMinorFactionsModel());
			gameStarter.AddModel(new DefaultClanPoliticsModel());
			gameStarter.AddModel(new DefaultVassalRewardsModel());
			gameStarter.AddModel(new DefaultClanFinanceModel());
			gameStarter.AddModel(new DefaultHeirSelectionCalculationModel());
			gameStarter.AddModel(new DefaultHeroDeathProbabilityCalculationModel());
			gameStarter.AddModel(new DefaultBuildingConstructionModel());
			gameStarter.AddModel(new DefaultBuildingEffectModel());
			gameStarter.AddModel(new DefaultWallHitPointCalculationModel());
			gameStarter.AddModel(new DefaultMarriageModel());
			gameStarter.AddModel(new DefaultAgeModel());
			gameStarter.AddModel(new DefaultPlayerProgressionModel());
			gameStarter.AddModel(new DefaultDailyTroopXpBonusModel());
			gameStarter.AddModel(new DefaultPregnancyModel());
			gameStarter.AddModel(new DefaultNotablePowerModel());
			gameStarter.AddModel(new DefaultTournamentModel());
			gameStarter.AddModel(new DefaultSiegeStrategyActionModel());
			gameStarter.AddModel(new DefaultSiegeEventModel());
			gameStarter.AddModel(new DefaultSiegeAftermathModel());
			gameStarter.AddModel(new DefaultSiegeLordsHallFightModel());
			gameStarter.AddModel(new DefaultCompanionHiringPriceCalculationModel());
			gameStarter.AddModel(new DefaultBuildingScoreCalculationModel());
			gameStarter.AddModel(new DefaultIssueModel());
			gameStarter.AddModel(new DefaultPrisonerRecruitmentCalculationModel());
			gameStarter.AddModel(new DefaultPartyTroopUpgradeModel());
			gameStarter.AddModel(new DefaultTavernMercenaryTroopsModel());
			gameStarter.AddModel(new DefaultWorkshopModel());
			gameStarter.AddModel(new DefaultDifficultyModel());
			gameStarter.AddModel(new DefaultLocationModel());
			gameStarter.AddModel(new DefaultPrisonerDonationModel());
			gameStarter.AddModel(new DefaultPrisonBreakModel());
			gameStarter.AddModel(new DefaultBattleCaptainModel());
			gameStarter.AddModel(new DefaultExecutionRelationModel());
			gameStarter.AddModel(new DefaultBannerItemModel());
			gameStarter.AddModel(new DefaultDelayedTeleportationModel());
			gameStarter.AddModel(new DefaultTroopSupplierProbabilityModel());
			gameStarter.AddModel(new DefaultCutsceneSelectionModel());
			gameStarter.AddModel(new DefaultEquipmentSelectionModel());
			gameStarter.AddModel(new DefaultAlleyModel());
			gameStarter.AddModel(new DefaultVoiceOverModel());
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0004ECB1 File Offset: 0x0004CEB1
		public void OnCampaignStart(CampaignGameStarter gameInitializer, GameManagerBase gameManager, bool isSavedCampaign)
		{
			gameManager.RegisterSubModuleObjects(isSavedCampaign);
			gameManager.AfterRegisterSubModuleObjects(isSavedCampaign);
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && isSavedCampaign)
			{
				MBObjectManager.Instance.RemoveTemporaryTypes();
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0004ECDC File Offset: 0x0004CEDC
		public void InitializeSandboxXMLs(bool isSavedCampaign)
		{
			MBObjectManager.Instance.LoadXML("NPCCharacters", false);
			if (!isSavedCampaign)
			{
				MBObjectManager.Instance.LoadXML("Heroes", false);
			}
			if (Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				MBObjectManager.Instance.LoadXML("MPCharacters", false);
			}
			if (!isSavedCampaign)
			{
				MBObjectManager.Instance.LoadXML("Kingdoms", false);
				MBObjectManager.Instance.LoadXML("Factions", false);
			}
			MBObjectManager.Instance.LoadXML("WorkshopTypes", false);
			MBObjectManager.Instance.LoadXML("LocationComplexTemplates", false);
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && !Game.Current.IsEditModeOn)
			{
				MBObjectManager.Instance.LoadXML("Settlements", false);
			}
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0004ED98 File Offset: 0x0004CF98
		public void InitializeCharactersAfterLoad(bool isSavedCampaign)
		{
			if (isSavedCampaign)
			{
				foreach (Hero hero in Campaign.Current.AliveHeroes)
				{
					if (!hero.CharacterObject.IsOriginalCharacter)
					{
						hero.CharacterObject.InitializeHeroCharacterOnAfterLoad();
					}
				}
				foreach (Hero hero2 in Campaign.Current.DeadOrDisabledHeroes)
				{
					if (!hero2.CharacterObject.IsOriginalCharacter)
					{
						hero2.CharacterObject.InitializeHeroCharacterOnAfterLoad();
					}
				}
				List<CharacterObject> list = new List<CharacterObject>();
				foreach (CharacterObject characterObject in Campaign.Current.ObjectManager.GetObjectTypeList<CharacterObject>())
				{
					if (!characterObject.IsReady && !characterObject.IsOriginalCharacter)
					{
						if (characterObject.HeroObject != null)
						{
							characterObject.InitializeHeroCharacterOnAfterLoad();
						}
						else
						{
							Debug.FailedAssert("saved a characterobject but not its heroobject", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SandBoxManager.cs", "InitializeCharactersAfterLoad", 436);
							list.Add(characterObject);
						}
					}
				}
				foreach (CharacterObject obj in list)
				{
					Campaign.Current.ObjectManager.UnregisterObject(obj);
				}
			}
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0004EF38 File Offset: 0x0004D138
		protected override void OnTick(float dt)
		{
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0004EF3A File Offset: 0x0004D13A
		public override void OnBeforeSave()
		{
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x0004EF3C File Offset: 0x0004D13C
		public override void OnAfterSave()
		{
		}
	}
}
