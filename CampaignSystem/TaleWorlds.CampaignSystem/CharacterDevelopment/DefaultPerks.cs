using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterDevelopment
{
	// Token: 0x0200034D RID: 845
	public class DefaultPerks
	{
		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x000C44B1 File Offset: 0x000C26B1
		private static DefaultPerks Instance
		{
			get
			{
				return Campaign.Current.DefaultPerks;
			}
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000C44BD File Offset: 0x000C26BD
		public DefaultPerks()
		{
			this.RegisterAll();
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x000C44CC File Offset: 0x000C26CC
		private void RegisterAll()
		{
			this._oneHandedWrappedHandles = this.Create("OneHandedWrappedHandles");
			this._oneHandedBasher = this.Create("OneHandedBasher");
			this._oneHandedToBeBlunt = this.Create("OneHandedToBeBlunt");
			this._oneHandedSwiftStrike = this.Create("OneHandedSwiftStrike");
			this._oneHandedCavalry = this.Create("OneHandedCavalry");
			this._oneHandedShieldBearer = this.Create("OneHandedShieldBearer");
			this._oneHandedTrainer = this.Create("OneHandedTrainer");
			this._oneHandedDuelist = this.Create("OneHandedDuelist");
			this._oneHandedShieldWall = this.Create("OneHandedShieldWall");
			this._oneHandedArrowCatcher = this.Create("OneHandedArrowCatcher");
			this._oneHandedMilitaryTradition = this.Create("OneHandedMilitaryTradition");
			this._oneHandedCorpsACorps = this.Create("OneHandedCorpsACorps");
			this._oneHandedStandUnited = this.Create("OneHandedStandUnited");
			this._oneHandedLeadByExample = this.Create("OneHandedLeadByExample");
			this._oneHandedSteelCoreShields = this.Create("OneHandedSteelCoreShields");
			this._oneHandedFleetOfFoot = this.Create("OneHandedFleetOfFoot");
			this._oneHandedDeadlyPurpose = this.Create("OneHandedDeadlyPurpose");
			this._oneHandedUnwaveringDefense = this.Create("OneHandedUnwaveringDefense");
			this._oneHandedPrestige = this.Create("OneHandedPrestige");
			this._oneHandedChinkInTheArmor = this.Create("OneHandedChinkInTheArmor");
			this._oneHandedWayOfTheSword = this.Create("OneHandedWayOfTheSword");
			this._twoHandedStrongGrip = this.Create("TwoHandedStrongGrip");
			this._twoHandedWoodChopper = this.Create("TwoHandedWoodChopper");
			this._twoHandedOnTheEdge = this.Create("TwoHandedOnTheEdge");
			this._twoHandedHeadBasher = this.Create("TwoHandedHeadBasher");
			this._twoHandedShowOfStrength = this.Create("TwoHandedShowOfStrength");
			this._twoHandedBaptisedInBlood = this.Create("TwoHandedBaptisedInBlood");
			this._twoHandedBeastSlayer = this.Create("TwoHandedBeastSlayer");
			this._twoHandedShieldBreaker = this.Create("TwoHandedShieldBreaker");
			this._twoHandedBerserker = this.Create("TwoHandedBerserker");
			this._twoHandedConfidence = this.Create("TwoHandedConfidence");
			this._twoHandedProjectileDeflection = this.Create("TwoHandedProjectileDeflection");
			this._twoHandedTerror = this.Create("TwoHandedTerror");
			this._twoHandedHope = this.Create("TwoHandedHope");
			this._twoHandedRecklessCharge = this.Create("TwoHandedRecklessCharge");
			this._twoHandedThickHides = this.Create("TwoHandedThickHides");
			this._twoHandedBladeMaster = this.Create("TwoHandedBladeMaster");
			this._twoHandedVandal = this.Create("TwoHandedVandal");
			this._twoHandedWayOfTheGreatAxe = this.Create("TwoHandedWayOfTheGreatAxe");
			this._polearmPikeman = this.Create("PolearmPikeman");
			this._polearmCavalry = this.Create("PolearmCavalry");
			this._polearmBraced = this.Create("PolearmBraced");
			this._polearmKeepAtBay = this.Create("PolearmKeepAtBay");
			this._polearmSwiftSwing = this.Create("PolearmSwiftSwing");
			this._polearmCleanThrust = this.Create("PolearmCleanThrust");
			this._polearmFootwork = this.Create("PolearmFootwork");
			this._polearmHardKnock = this.Create("PolearmHardKnock");
			this._polearmSteedKiller = this.Create("PolearmSteadKiller");
			this._polearmLancer = this.Create("PolearmLancer");
			this._polearmSkewer = this.Create("PolearmSkewer");
			this._polearmGuards = this.Create("PolearmGuards");
			this._polearmStandardBearer = this.Create("PolearmStandardBearer");
			this._polearmPhalanx = this.Create("PolearmPhalanx");
			this._polearmHardyFrontline = this.Create("PolearmHardyFrontline");
			this._polearmDrills = this.Create("PolearmDrills");
			this._polearmSureFooted = this.Create("PolearmSureFooted");
			this._polearmUnstoppableForce = this.Create("PolearmUnstoppableForce");
			this._polearmCounterweight = this.Create("PolearmCounterweight");
			this._polearmSharpenTheTip = this.Create("PolearmSharpenTheTip");
			this._polearmWayOfTheSpear = this.Create("PolearmWayOfTheSpear");
			this._bowBowControl = this.Create("BowBowControl");
			this._bowDeadAim = this.Create("BowDeadAim");
			this._bowBodkin = this.Create("BowBodkin");
			this._bowRangersSwiftness = this.Create("BowRangersSwiftness");
			this._bowRapidFire = this.Create("BowRapidFire");
			this._bowQuickAdjustments = this.Create("BowQuickAdjustments");
			this._bowMerryMen = this.Create("BowMerryMen");
			this._bowMountedArchery = this.Create("BowMountedArchery");
			this._bowTrainer = this.Create("BowTrainer");
			this._bowStrongBows = this.Create("BowStrongBows");
			this._bowDiscipline = this.Create("BowDiscipline");
			this._bowHunterClan = this.Create("BowHunterClan");
			this._bowSkirmishPhaseMaster = this.Create("BowSkirmishPhaseMaster");
			this._bowEagleEye = this.Create("BowEagleEye");
			this._bowBullsEye = this.Create("BowBullsEye");
			this._bowRenownedArcher = this.Create("BowRenownedArcher");
			this._bowHorseMaster = this.Create("BowHorseMaster");
			this._bowDeepQuivers = this.Create("BowDeepQuivers");
			this._bowQuickDraw = this.Create("BowQuickDraw");
			this._bowNockingPoint = this.Create("BowNockingPoint");
			this._bowDeadshot = this.Create("BowDeadshot");
			this._crossbowPiercer = this.Create("CrossbowPiercer");
			this._crossbowMarksmen = this.Create("CrossbowMarksmen");
			this._crossbowUnhorser = this.Create("CrossbowUnhorser");
			this._crossbowWindWinder = this.Create("CrossbowWindWinder");
			this._crossbowDonkeysSwiftness = this.Create("CrossbowDonkeysSwiftness");
			this._crossbowSheriff = this.Create("CrossbowSheriff");
			this._crossbowPeasantLeader = this.Create("CrossbowPeasantLeader");
			this._crossbowRenownMarksmen = this.Create("CrossbowRenownMarksmen");
			this._crossbowFletcher = this.Create("CrossbowFletcher");
			this._crossbowPuncture = this.Create("CrossbowPuncture");
			this._crossbowLooseAndMove = this.Create("CrossbowLooseAndMove");
			this._crossbowDeftHands = this.Create("CrossbowDeftHands");
			this._crossbowCounterFire = this.Create("CrossbowCounterFire");
			this._crossbowMountedCrossbowman = this.Create("CrossbowMountedCrossbowman");
			this._crossbowSteady = this.Create("CrossbowSteady");
			this._crossbowLongShots = this.Create("CrossbowLongShots");
			this._crossbowHammerBolts = this.Create("CrossbowHammerBolts");
			this._crossbowPavise = this.Create("CrossbowPavise");
			this._crossbowTerror = this.Create("CrossbowTerror");
			this._crossbowPickedShots = this.Create("CrossbowBoltenGuard");
			this._crossbowMightyPull = this.Create("CrossbowMightyPull");
			this._throwingQuickDraw = this.Create("ThrowingQuickDraw");
			this._throwingShieldBreaker = this.Create("ThrowingShieldBreaker");
			this._throwingHunter = this.Create("ThrowingHunter");
			this._throwingFlexibleFighter = this.Create("ThrowingFlexibleFighter");
			this._throwingMountedSkirmisher = this.Create("ThrowingMountedSkirmisher");
			this._throwingPerfectTechnique = this.Create("ThrowingPerfectTechnique");
			this._throwingRunningThrow = this.Create("ThrowingRunningThrow");
			this._throwingKnockOff = this.Create("ThrowingKnockOff");
			this._throwingSkirmisher = this.Create("ThrowingSkirmisher");
			this._throwingWellPrepared = this.Create("ThrowingWellPrepared");
			this._throwingFocus = this.Create("ThrowingFocus");
			this._throwingLastHit = this.Create("ThrowingLastHit");
			this._throwingHeadHunter = this.Create("ThrowingHeadHunter");
			this._throwingThrowingCompetitions = this.Create("ThrowingThrowingCompetitions");
			this._throwingSaddlebags = this.Create("ThrowingSaddlebags");
			this._throwingSplinters = this.Create("ThrowingSplinters");
			this._throwingResourceful = this.Create("ThrowingResourceful");
			this._throwingLongReach = this.Create("ThrowingLongReach");
			this._throwingWeakSpot = this.Create("ThrowingWeakSpot");
			this._throwingImpale = this.Create("ThrowingImpale");
			this._throwingUnstoppableForce = this.Create("ThrowingUnstoppableForce");
			this._ridingFullSpeed = this.Create("RidingFullSpeed");
			this._ridingNimbleSteed = this.Create("RidingNimbleStead");
			this._ridingWellStraped = this.Create("RidingWellStraped");
			this._ridingVeterinary = this.Create("RidingVeterinary");
			this._ridingNomadicTraditions = this.Create("RidingNomadicTraditions");
			this._ridingDeeperSacks = this.Create("RidingDeeperSacks");
			this._ridingSagittarius = this.Create("RidingSagittarius");
			this._ridingSweepingWind = this.Create("RidingSweepingWind");
			this._ridingReliefForce = this.Create("RidingReliefForce");
			this._ridingMountedWarrior = this.Create("RidingMountedWarrior");
			this._ridingHorseArcher = this.Create("RidingHorseArcher");
			this._ridingShepherd = this.Create("RidingShepherd");
			this._ridingBreeder = this.Create("RidingBreeder");
			this._ridingThunderousCharge = this.Create("RidingThunderousCharge");
			this._ridingAnnoyingBuzz = this.Create("RidingAnnoyingBuzz");
			this._ridingMountedPatrols = this.Create("RidingMountedPatrols");
			this._ridingCavalryTactics = this.Create("RidingCavalryTactics");
			this._ridingDauntlessSteed = this.Create("RidingDauntlessSteed");
			this._ridingToughSteed = this.Create("RidingToughSteed");
			this._ridingTheWayOfTheSaddle = this.Create("RidingTheWayOfTheSaddle");
			this._athleticsMorningExercise = this.Create("AthleticsMorningExercise");
			this._athleticsWellBuilt = this.Create("AthleticsWellBuilt");
			this._athleticsFury = this.Create("AthleticsFury");
			this._athleticsFormFittingArmor = this.Create("AthleticsFormFittingArmor");
			this._athleticsImposingStature = this.Create("AthleticsImposingStature");
			this._athleticsStamina = this.Create("AthleticsStamina");
			this._athleticsSprint = this.Create("AthleticsSprint");
			this._athleticsPowerful = this.Create("AthleticsPowerful");
			this._athleticsSurgingBlow = this.Create("AthleticsSurgingBlow");
			this._athleticsBraced = this.Create("AthleticsBraced");
			this._athleticsWalkItOff = this.Create("AthleticsWalkItOff");
			this._athleticsAGoodDaysRest = this.Create("AthleticsAGoodDaysRest");
			this._athleticsDurable = this.Create("AthleticsDurable");
			this._athleticsEnergetic = this.Create("AthleticsEnergetic");
			this._athleticsSteady = this.Create("AthleticsSteady");
			this._athleticsStrong = this.Create("AthleticsStrong");
			this._athleticsStrongLegs = this.Create("AthleticsStrongLegs");
			this._athleticsStrongArms = this.Create("AthleticsStrongArms");
			this._athleticsSpartan = this.Create("AthleticsSpartan");
			this._athleticsIgnorePain = this.Create("AthleticsIgnorePain");
			this._athleticsMightyBlow = this.Create("AthleticsMightyBlow");
			this._craftingSharpenedEdge = this.Create("CraftingSharpenedEdge");
			this._craftingSharpenedTip = this.Create("CraftingSharpenedTip");
			this._craftingIronMaker = this.Create("IronYield");
			this._craftingCharcoalMaker = this.Create("CharcoalYield");
			this._craftingSteelMaker = this.Create("SteelMaker");
			this._craftingSteelMaker2 = this.Create("SteelMaker2");
			this._craftingSteelMaker3 = this.Create("SteelMaker3");
			this._craftingCuriousSmelter = this.Create("CuriousSmelter");
			this._craftingCuriousSmith = this.Create("CuriousSmith");
			this._craftingPracticalRefiner = this.Create("PracticalRefiner");
			this._craftingPracticalSmelter = this.Create("PracticalSmelter");
			this._craftingPracticalSmith = this.Create("PracticalSmith");
			this._craftingArtisanSmith = this.Create("ArtisanSmith");
			this._craftingExperiencedSmith = this.Create("ExperiencedSmith");
			this._craftingMasterSmith = this.Create("MasterSmith");
			this._craftingLegendarySmith = this.Create("LegendarySmith");
			this._craftingVigorousSmith = this.Create("VigorousSmith");
			this._craftingStrongSmith = this.Create("StrongSmith");
			this._craftingEnduringSmith = this.Create("EnduringSmith");
			this._craftingFencerSmith = this.Create("WeaponMasterSmith");
			this._tacticsTightFormations = this.Create("TacticsTightFormations");
			this._tacticsLooseFormations = this.Create("TacticsLooseFormations");
			this._tacticsExtendedSkirmish = this.Create("TacticsExtendedSkirmish");
			this._tacticsDecisiveBattle = this.Create("TacticsDecisiveBattle");
			this._tacticsSmallUnitTactics = this.Create("TacticsSmallUnitTactics");
			this._tacticsHordeLeader = this.Create("TacticsHordeLeader");
			this._tacticsLawKeeper = this.Create("TacticsLawkeeper");
			this._tacticsCoaching = this.Create("TacticsCoaching");
			this._tacticsSwiftRegroup = this.Create("TacticsSwiftRegroup");
			this._tacticsImproviser = this.Create("TacticsImproviser");
			this._tacticsOnTheMarch = this.Create("TacticsOnTheMarch");
			this._tacticsCallToArms = this.Create("TacticsCallToArms");
			this._tacticsPickThemOfTheWalls = this.Create("TacticsPickThemOfTheWalls");
			this._tacticsMakeThemPay = this.Create("TacticsMakeThemPay");
			this._tacticsEliteReserves = this.Create("TacticsEliteReserves");
			this._tacticsEncirclement = this.Create("TacticsEncirclement");
			this._tacticsPreBattleManeuvers = this.Create("TacticsPreBattleManeuvers");
			this._tacticsBesieged = this.Create("TacticsBesieged");
			this._tacticsCounteroffensive = this.Create("TacticsCounteroffensive");
			this._tacticsGensdarmes = this.Create("TacticsGensdarmes");
			this._tacticsTacticalMastery = this.Create("TacticsTacticalMastery");
			this._scoutingDayTraveler = this.Create("ScoutingDayTraveler");
			this._scoutingNightRunner = this.Create("ScoutingNightRunner");
			this._scoutingPathfinder = this.Create("ScoutingPathfinder");
			this._scoutingWaterDiviner = this.Create("ScoutingWaterDiviner");
			this._scoutingForestKin = this.Create("ScoutingForestKin");
			this._scoutingDesertBorn = this.Create("ScoutingDesertBorn");
			this._scoutingForcedMarch = this.Create("ScoutingForcedMarch");
			this._scoutingUnburdened = this.Create("ScoutingUnburdened");
			this._scoutingTracker = this.Create("ScoutingTracker");
			this._scoutingRanger = this.Create("ScoutingRanger");
			this._scoutingMountedScouts = this.Create("ScoutingMountedScouts");
			this._scoutingPatrols = this.Create("ScoutingPatrols");
			this._scoutingForagers = this.Create("ScoutingForagers");
			this._scoutingBeastWhisperer = this.Create("ScoutingBeastWhisperer");
			this._scoutingVillageNetwork = this.Create("ScoutingVillageNetwork");
			this._scoutingRumourNetwork = this.Create("ScoutingRumourNetwork");
			this._scoutingVantagePoint = this.Create("ScoutingVantagePoint");
			this._scoutingKeenSight = this.Create("ScoutingKeenSight");
			this._scoutingVanguard = this.Create("ScoutingVanguard");
			this._scoutingRearguard = this.Create("ScoutingRearguard");
			this._scoutingUncannyInsight = this.Create("ScoutingUncannyInsight");
			this._rogueryNoRestForTheWicked = this.Create("RogueryNoRestForTheWicked");
			this._roguerySweetTalker = this.Create("RoguerySweetTalker");
			this._rogueryTwoFaced = this.Create("RogueryTwoFaced");
			this._rogueryDeepPockets = this.Create("RogueryDeepPockets");
			this._rogueryInBestLight = this.Create("RogueryInBestLight");
			this._rogueryKnowHow = this.Create("RogueryKnowHow");
			this._rogueryPromises = this.Create("RogueryPromises");
			this._rogueryManhunter = this.Create("RogueryManhunter");
			this._rogueryScarface = this.Create("RogueryScarface");
			this._rogueryWhiteLies = this.Create("RogueryWhiteLies");
			this._roguerySmugglerConnections = this.Create("RoguerySmugglerConnections");
			this._rogueryPartnersInCrime = this.Create("RogueryPartnersInCrime");
			this._rogueryOneOfTheFamily = this.Create("RogueryOneOfTheFamily");
			this._roguerySaltTheEarth = this.Create("RoguerySaltTheEarth");
			this._rogueryCarver = this.Create("RogueryCarver");
			this._rogueryRansomBroker = this.Create("RogueryRansomBroker");
			this._rogueryArmsDealer = this.Create("RogueryArmsDealer");
			this._rogueryDirtyFighting = this.Create("RogueryDirtyFighting");
			this._rogueryDashAndSlash = this.Create("RogueryDashAndSlash");
			this._rogueryFleetFooted = this.Create("RogueryFleetFooted");
			this._rogueryRogueExtraordinaire = this.Create("RogueryRogueExtraordinaire");
			this._leadershipCombatTips = this.Create("LeadershipCombatTips");
			this._leadershipRaiseTheMeek = this.Create("LeadershipRaiseTheMeek");
			this._leadershipFerventAttacker = this.Create("LeadershipFerventAttacker");
			this._leadershipStoutDefender = this.Create("LeadershipStoutDefender");
			this._leadershipAuthority = this.Create("LeadershipAuthority");
			this._leadershipHeroicLeader = this.Create("LeadershipHeroicLeader");
			this._leadershipLoyaltyAndHonor = this.Create("LeadershipLoyaltyAndHonor");
			this._leadershipFamousCommander = this.Create("LeadershipFamousCommander");
			this._leadershipPresence = this.Create("LeadershipPresence");
			this._leadershipLeaderOfTheMasses = this.Create("LeadershipLeaderOfMasses");
			this._leadershipVeteransRespect = this.Create("LeadershipVeteransRespect");
			this._leadershipCitizenMilitia = this.Create("LeadershipCitizenMilitia");
			this._leadershipInspiringLeader = this.Create("LeadershipInspiringLeader");
			this._leadershipUpliftingSpirit = this.Create("LeadershipUpliftingSpirit");
			this._leadershipMakeADifference = this.Create("LeadershipMakeADifference");
			this._leadershipLeadByExample = this.Create("LeadershipLeadByExample");
			this._leadershipTrustedCommander = this.Create("LeadershipTrustedCommander");
			this._leadershipGreatLeader = this.Create("LeadershipGreatLeader");
			this._leadershipWePledgeOurSwords = this.Create("LeadershipWePledgeOurSwords");
			this._leadershipTalentMagnet = this.Create("LeadershipTalentMagnet");
			this._leadershipUltimateLeader = this.Create("LeadershipUltimateLeader");
			this._charmVirile = this.Create("CharmVirile");
			this._charmSelfPromoter = this.Create("CharmSelfPromoter");
			this._charmOratory = this.Create("CharmOratory");
			this._charmWarlord = this.Create("CharmWarlord");
			this._charmForgivableGrievances = this.Create("CharmForgivableGrievances");
			this._charmMeaningfulFavors = this.Create("CharmMeaningfulFavors");
			this._charmInBloom = this.Create("CharmInBloom");
			this._charmYoungAndRespectful = this.Create("CharmYoungAndRespectful");
			this._charmFirebrand = this.Create("CharmFirebrand");
			this._charmFlexibleEthics = this.Create("CharmFlexibleEthics");
			this._charmEffortForThePeople = this.Create("CharmEffortForThePeople");
			this._charmSlickNegotiator = this.Create("CharmSlickNegotiator");
			this._charmGoodNatured = this.Create("CharmGoodNatured");
			this._charmTribute = this.Create("CharmTribute");
			this._charmMoralLeader = this.Create("CharmMoralLeader");
			this._charmNaturalLeader = this.Create("CharmNaturalLeader");
			this._charmPublicSpeaker = this.Create("CharmPublicSpeaker");
			this._charmParade = this.Create("CharmParade");
			this._charmCamaraderie = this.Create("CharmCamaraderie");
			this._charmImmortalCharm = this.Create("CharmImmortalCharm");
			this._tradeAppraiser = this.Create("TradeAppraiser");
			this._tradeWholeSeller = this.Create("TradeWholeSeller");
			this._tradeCaravanMaster = this.Create("TradeCaravanMaster");
			this._tradeMarketDealer = this.Create("TradeMarketDealer");
			this._tradeTravelingRumors = this.Create("TradeTravelingRumors");
			this._tradeLocalConnection = this.Create("TradeLocalConnection");
			this._tradeDistributedGoods = this.Create("TradeDistributedGoods");
			this._tradeTollgates = this.Create("TradeTollgates");
			this._tradeArtisanCommunity = this.Create("TradeArtisanCommunity");
			this._tradeGreatInvestor = this.Create("TradeGreatInvestor");
			this._tradeMercenaryConnections = this.Create("TradeMercenaryConnections");
			this._tradeContentTrades = this.Create("TradeContentTrades");
			this._tradeInsurancePlans = this.Create("TradeInsurancePlans");
			this._tradeRapidDevelopment = this.Create("TradeRapidDevelopment");
			this._tradeGranaryAccountant = this.Create("TradeGranaryAccountant");
			this._tradeTradeyardForeman = this.Create("TradeTradeyardForeman");
			this._tradeSwordForBarter = this.Create("TradeSwordForBarter");
			this._tradeSelfMadeMan = this.Create("TradeSelfMadeMan");
			this._tradeSilverTongue = this.Create("TradeSilverTongue");
			this._tradeSpringOfGold = this.Create("TradeSpringOfGold");
			this._tradeManOfMeans = this.Create("TradeManOfMeans");
			this._tradeTrickleDown = this.Create("TradeTrickleDown");
			this._tradeEverythingHasAPrice = this.Create("TradeEverythingHasAPrice");
			this._stewardWarriorsDiet = this.Create("StewardWarriorsDiet");
			this._stewardFrugal = this.Create("StewardFrugal");
			this._stewardSevenVeterans = this.Create("StewardSevenVeterans");
			this._stewardDrillSergant = this.Create("StewardDrillSergant");
			this._stewardSweatshops = this.Create("StewardSweatshops");
			this._stewardStiffUpperLip = this.Create("StewardStiffUpperLip");
			this._stewardPaidInPromise = this.Create("StewardPaidInPromise");
			this._stewardEfficientCampaigner = this.Create("StewardEfficientCampaigner");
			this._stewardGivingHands = this.Create("StewardForeseeableFuture");
			this._stewardLogistician = this.Create("StewardLogistician");
			this._stewardRelocation = this.Create("StewardRelocation");
			this._stewardAidCorps = this.Create("StewardAidCorps");
			this._stewardGourmet = this.Create("StewardGourmet");
			this._stewardSoundReserves = this.Create("StewardSoundReserves");
			this._stewardForcedLabor = this.Create("StewardForcedLabor");
			this._stewardContractors = this.Create("StewardContractors");
			this._stewardArenicosMules = this.Create("StewardArenicosMules");
			this._stewardArenicosHorses = this.Create("StewardArenicosHorses");
			this._stewardMasterOfPlanning = this.Create("StewardMasterOfPlanning");
			this._stewardMasterOfWarcraft = this.Create("StewardMasterOfWarcraft");
			this._stewardPriceOfLoyalty = this.Create("StewardPriceOfLoyalty");
			this._medicineSelfMedication = this.Create("MedicineSelfMedication");
			this._medicinePreventiveMedicine = this.Create("MedicinePreventiveMedicine");
			this._medicineTriageTent = this.Create("MedicineTriageTent");
			this._medicineWalkItOff = this.Create("MedicineWalkItOff");
			this._medicineSledges = this.Create("MedicineSledges");
			this._medicineDoctorsOath = this.Create("MedicineDoctorsOath");
			this._medicineBestMedicine = this.Create("MedicineBestMedicine");
			this._medicineGoodLodging = this.Create("MedicineGoodLodging");
			this._medicineSiegeMedic = this.Create("MedicineSiegeMedic");
			this._medicineVeterinarian = this.Create("MedicineVeterinarian");
			this._medicinePristineStreets = this.Create("MedicinePristineStreets");
			this._medicineBushDoctor = this.Create("MedicineBushDoctor");
			this._medicinePerfectHealth = this.Create("MedicinePerfectHealth");
			this._medicineHealthAdvise = this.Create("MedicineHealthAdvise");
			this._medicinePhysicianOfPeople = this.Create("MedicinePhysicianOfPeople");
			this._medicineCleanInfrastructure = this.Create("MedicineCleanInfrastructure");
			this._medicineCheatDeath = this.Create("MedicineCheatDeath");
			this._medicineFortitudeTonic = this.Create("MedicineFortitudeTonic");
			this._medicineHelpingHands = this.Create("MedicineHelpingHands");
			this._medicineBattleHardened = this.Create("MedicineBattleHardened");
			this._medicineMinisterOfHealth = this.Create("MedicineMinisterOfHealth");
			this._engineeringScaffolds = this.Create("EngineeringScaffolds");
			this._engineeringTorsionEngines = this.Create("EngineeringTorsionEngines");
			this._engineeringSiegeWorks = this.Create("EngineeringSiegeWorks");
			this._engineeringDungeonArchitect = this.Create("EngineeringDungeonArchitect");
			this._engineeringCarpenters = this.Create("EngineeringCarpenters");
			this._engineeringMilitaryPlanner = this.Create("EngineeringMilitaryPlanner");
			this._engineeringWallBreaker = this.Create("EngineeringWallBreaker");
			this._engineeringDreadfulSieger = this.Create("EngineeringDreadfulSieger");
			this._engineeringSalvager = this.Create("EngineeringSalvager");
			this._engineeringForeman = this.Create("EngineeringForeman");
			this._engineeringStonecutters = this.Create("EngineeringStonecutters");
			this._engineeringSiegeEngineer = this.Create("EngineeringSiegeEngineer");
			this._engineeringCampBuilding = this.Create("EngineeringCampBuilding");
			this._engineeringBattlements = this.Create("EngineeringBattlements");
			this._engineeringEngineeringGuilds = this.Create("EngineeringEngineeringGuilds");
			this._engineeringApprenticeship = this.Create("EngineeringApprenticeship");
			this._engineeringMetallurgy = this.Create("EngineeringMetallurgy");
			this._engineeringImprovedTools = this.Create("EngineeringImprovedTools");
			this._engineeringClockwork = this.Create("EngineeringClockwork");
			this._engineeringArchitecturalCommisions = this.Create("EngineeringArchitecturalCommissions");
			this._engineeringMasterwork = this.Create("EngineeringMasterwork");
			this.InitializeAll();
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x000C5DB8 File Offset: 0x000C3FB8
		private void InitializeAll()
		{
			this._oneHandedWrappedHandles.Initialize("{=looKU9Gl}Wrapped Handles", DefaultSkills.OneHanded, this.GetTierCost(1), this._oneHandedBasher, "{=dY3GOmTN}{VALUE}% handling to one handed weapons.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=0mBHB7mA}{VALUE} one handed skill to infantry troops in your formation.", SkillEffect.PerkRole.Captain, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.OneHandedUser);
			this._oneHandedBasher.Initialize("{=6yEeYNRu}Basher", DefaultSkills.OneHanded, this.GetTierCost(1), this._oneHandedWrappedHandles, "{=fFNNeqxu}{VALUE}% damage and longer stun duration with shield bashes.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=goOE8oiI}{VALUE}% damage taken by infantry while in shield wall formation.", SkillEffect.PerkRole.Captain, -0.04f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._oneHandedToBeBlunt.Initialize("{=SJ69EYuI}To Be Blunt", DefaultSkills.OneHanded, this.GetTierCost(2), this._oneHandedSwiftStrike, "{=mzUa3duw}{VALUE}% damage with one handed axes and maces.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=Ib9RYpMO}{VALUE} daily security to governed settlement.", SkillEffect.PerkRole.Governor, 0.5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedSwiftStrike.Initialize("{=ciELES5v}Swift Strike", DefaultSkills.OneHanded, this.GetTierCost(2), this._oneHandedToBeBlunt, "{=bW7DT97A}{VALUE}% swing speed with one handed weapons.", SkillEffect.PerkRole.Personal, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=xwA6Om0Y}{VALUE} daily militia recruitment in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedCavalry.Initialize("{=YVGtcLHF}Cavalry", DefaultSkills.OneHanded, this.GetTierCost(3), this._oneHandedShieldBearer, "{=D3k7UbmZ}{VALUE}% damage with one handed weapons while mounted.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=aj2R3vnb}{VALUE}% melee damage by cavalry troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.Melee);
			this._oneHandedShieldBearer.Initialize("{=vnG1q18y}Shield Bearer", DefaultSkills.OneHanded, this.GetTierCost(3), this._oneHandedCavalry, "{=hMJVRJdw}Removed movement speed penalty of wielding shields.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Invalid, "{=1QsZq9UW}{VALUE}% movement speed to infantry in your formation.", SkillEffect.PerkRole.Captain, 0.03f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._oneHandedTrainer.Initialize("{=3xuwVbfs}Trainer", DefaultSkills.OneHanded, this.GetTierCost(4), this._oneHandedDuelist, "{=Ti9auMiO}{VALUE} hit points.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.Add, "{=rXb91Rwi}{VALUE}% experience to melee troops in your party after every battle.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedDuelist.Initialize("{=XphY9cNV}Duelist", DefaultSkills.OneHanded, this.GetTierCost(4), this._oneHandedTrainer, "{=uRZgz63l}{VALUE}% damage while wielding a one handed weapon without a shield.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=uKTgBX4S}Double the amount of renown gained from tournaments.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedShieldWall.Initialize("{=nSwkI97I}Shieldwall", DefaultSkills.OneHanded, this.GetTierCost(5), this._oneHandedArrowCatcher, "{=DiFIyniQ}{VALUE}% damage to your shield while blocking in wrong direction.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=EdDRYFoL}Larger shield protection area against projectiles to troops in your formation while in shield wall formation.", SkillEffect.PerkRole.Captain, 0.01f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.ShieldUser);
			this._oneHandedArrowCatcher.Initialize("{=a94mkNNk}Arrow Catcher", DefaultSkills.OneHanded, this.GetTierCost(5), this._oneHandedShieldWall, "{=dcsschkC}Larger shield protection area against projectiles.", SkillEffect.PerkRole.Personal, 0.01f, SkillEffect.EffectIncrementType.Add, "{=uz7KxUlP}Larger shield protection area against projectiles for troops in your formation.", SkillEffect.PerkRole.Captain, 0.01f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.ShieldUser);
			this._oneHandedMilitaryTradition.Initialize("{=Fc7OsyZ8}Military Tradition", DefaultSkills.OneHanded, this.GetTierCost(6), this._oneHandedCorpsACorps, "{=0A6BUASZ}{VALUE} daily experience to infantry in your party.", SkillEffect.PerkRole.PartyLeader, 2f, SkillEffect.EffectIncrementType.Add, "{=B2msxAju}{VALUE}% garrison wages in the governed settlement.", SkillEffect.PerkRole.Governor, -0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedCorpsACorps.Initialize("{=M3aNEkBJ}Corps-a-corps", DefaultSkills.OneHanded, this.GetTierCost(6), this._oneHandedMilitaryTradition, "{=8jHJeh8z}{VALUE}% of the total experience gained as a bonus to infantry after battles.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=wBgpln4f}{VALUE} garrison limit in the governed settlement.", SkillEffect.PerkRole.Governor, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedStandUnited.Initialize("{=d8qjwKza}Stand United", DefaultSkills.OneHanded, this.GetTierCost(7), this._oneHandedLeadByExample, "{=JZ8ihtoa}{VALUE} starting battle morale to troops in your party if you are outnumbered.", SkillEffect.PerkRole.PartyLeader, 8f, SkillEffect.EffectIncrementType.Add, "{=5aVPqukr}{VALUE}% security provided by troops in the garrison of the governed settlement.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedLeadByExample.Initialize("{=bOhbWapX}Lead by example", DefaultSkills.OneHanded, this.GetTierCost(7), this._oneHandedStandUnited, "{=V97vqbIb}{VALUE}% experience to troops in your party after battle.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=g5nnybjz}{VALUE} starting battle morale to troops in your party.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedSteelCoreShields.Initialize("{=rSATpMpq}Steel Core Shields", DefaultSkills.OneHanded, this.GetTierCost(8), this._oneHandedFleetOfFoot, "{=q2gybZYy}{VALUE}% damage to your shields.", SkillEffect.PerkRole.Personal, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=Bb4L971j}{VALUE}% damage to shields of infantry troops in your formation.", SkillEffect.PerkRole.Captain, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ShieldUser);
			this._oneHandedFleetOfFoot.Initialize("{=OtdkOGur}Fleet of Foot", DefaultSkills.OneHanded, this.GetTierCost(8), this._oneHandedSteelCoreShields, "{=V53EYEXx}{VALUE}% combat movement speed.", SkillEffect.PerkRole.Personal, 0.04f, SkillEffect.EffectIncrementType.AddFactor, "{=1QsZq9UW}{VALUE}% movement speed to infantry in your formation.", SkillEffect.PerkRole.Captain, 0.04f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._oneHandedDeadlyPurpose.Initialize("{=xpGoduJq}Deadly Purpose", DefaultSkills.OneHanded, this.GetTierCost(9), this._oneHandedUnwaveringDefense, "{=CTqO5MBf}{VALUE}% damage with one handed weapons.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=fcmt2U5u}{VALUE}% melee weapon damage by infantry in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee);
			this._oneHandedUnwaveringDefense.Initialize("{=yFbEDUyb}Unwavering Defense", DefaultSkills.OneHanded, this.GetTierCost(9), this._oneHandedDeadlyPurpose, "{=Ti9auMiO}{VALUE} hit points.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "{=aeNhsyD7}{VALUE} hit points to infantry in your party.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedPrestige.Initialize("{=DSKtsYPi}Prestige", DefaultSkills.OneHanded, this.GetTierCost(10), this._oneHandedChinkInTheArmor, "{=LjeYTgX7}{VALUE}% damage against shields with one handed weapons.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=qxbBsB1a}{VALUE} party limit.", SkillEffect.PerkRole.PartyLeader, 15f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedChinkInTheArmor.Initialize("{=bBa0LB1D}Chink in the Armor", DefaultSkills.OneHanded, this.GetTierCost(10), this._oneHandedPrestige, "{=KKsCor3D}{VALUE}% armor penetration with melee attacks.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=3a6tmImq}{VALUE}% recruitment cost of infantry.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._oneHandedWayOfTheSword.Initialize("{=nThmB3yB}Way of the Sword", DefaultSkills.OneHanded, this.GetTierCost(11), null, "{=jan55Git}{VALUE}% attack speed with one handed weapons for every skill point above 250.", SkillEffect.PerkRole.Personal, 0.002f, SkillEffect.EffectIncrementType.AddFactor, "{=hr0TfX6o}{VALUE}% damage with one handed weapons for every skill point above 250.", SkillEffect.PerkRole.Personal, 0.005f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedStrongGrip.Initialize("{=xDQTgPf0}Strong Grip", DefaultSkills.TwoHanded, this.GetTierCost(1), this._twoHandedWoodChopper, "{=OpVRgL9n}{VALUE}% handling to two handed weapons.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=TmYKKarv}{VALUE} two handed skill to infantry troops in your formation.", SkillEffect.PerkRole.Captain, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot | TroopUsageFlags.TwoHandedUser);
			this._twoHandedWoodChopper.Initialize("{=J7oh7Vin}Wood Chopper", DefaultSkills.TwoHanded, this.GetTierCost(1), this._twoHandedStrongGrip, "{=impj5bAM}{VALUE}% damage to shields with two handed weapons.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=4u69jBeE}{VALUE}% damage against shields by troops in your formation.", SkillEffect.PerkRole.Captain, 0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._twoHandedOnTheEdge.Initialize("{=rkuAgPSA}On the Edge", DefaultSkills.TwoHanded, this.GetTierCost(2), this._twoHandedHeadBasher, "{=R8Lnif8l}{VALUE}% swing speed with two handed weapons.", SkillEffect.PerkRole.Personal, 0.03f, SkillEffect.EffectIncrementType.AddFactor, "{=z5DZXHF7}{VALUE}% swing speed to infantry in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee);
			this._twoHandedHeadBasher.Initialize("{=E5bgLJcs}Head Basher", DefaultSkills.TwoHanded, this.GetTierCost(2), this._twoHandedOnTheEdge, "{=qJBhadGi}{VALUE}% damage with two handed axes and maces.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=c86V0dch}{VALUE}% damage by infantry in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._twoHandedShowOfStrength.Initialize("{=RlMqzqbS}Show of Strength", DefaultSkills.TwoHanded, this.GetTierCost(3), this._twoHandedBaptisedInBlood, "{=7Nudvd8S}{VALUE}% chance of knocking the enemy down with a heavy hit.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=3a6tmImq}{VALUE}% recruitment cost of infantry.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedBaptisedInBlood.Initialize("{=miMZavW3}Baptised in Blood", DefaultSkills.TwoHanded, this.GetTierCost(3), this._twoHandedShowOfStrength, "{=TR4ORD1T}{VALUE} experience to infantry in your party for each enemy you kill with a two handed weapon.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "{=rXb91Rwi}{VALUE}% experience to melee troops in your party after every battle.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedBeastSlayer.Initialize("{=NDtlE6PY}Beast Slayer", DefaultSkills.TwoHanded, this.GetTierCost(4), this._twoHandedShieldBreaker, "{=fxTRlxBD}{VALUE}% damage to mounts with two handed weapons.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=lekpGqEA}{VALUE}% damage to mounts by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._twoHandedShieldBreaker.Initialize("{=bM9VX881}Shield breaker", DefaultSkills.TwoHanded, this.GetTierCost(4), this._twoHandedBeastSlayer, "{=impj5bAM}{VALUE}% damage to shields with two handed weapons.", SkillEffect.PerkRole.Personal, 0.4f, SkillEffect.EffectIncrementType.AddFactor, "{=ciCQnAj6}{VALUE}% damage against shields by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._twoHandedBerserker.Initialize("{=RssJTUpL}Berserker", DefaultSkills.TwoHanded, this.GetTierCost(5), this._twoHandedConfidence, "{=D5RqqHIm}{VALUE}% damage with two handed weapons while you have less than half of your hit points.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=B2msxAju}{VALUE}% garrison wages in the governed settlement.", SkillEffect.PerkRole.Governor, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedConfidence.Initialize("{=2jnsxsv5}Confidence", DefaultSkills.TwoHanded, this.GetTierCost(5), this._twoHandedBerserker, "{=QUXbhsxb}{VALUE}% damage with two handed weapons while you have more than 90% of your hit points.", SkillEffect.PerkRole.Personal, 0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=FX0GjiNa}{VALUE}% build speed to military projects in the governed settlement.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedProjectileDeflection.Initialize("{=rCCG4mSJ}Projectile Deflection", DefaultSkills.TwoHanded, this.GetTierCost(6), null, "{=YP8tN7nl}You can deflect projectiles with two handed swords by blocking.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Invalid, "{=FdSPC05Q}{VALUE}% experience to garrison troops in the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedTerror.Initialize("{=nAlCj2m0}Terror", DefaultSkills.TwoHanded, this.GetTierCost(7), this._twoHandedHope, "{=thGHcZMB}{VALUE}% battle morale effect to enemy troops with your two handed kills.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=POp8DAZD}{VALUE} prisoner limit.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedHope.Initialize("{=lPuk6bao}Hope", DefaultSkills.TwoHanded, this.GetTierCost(7), this._twoHandedTerror, "{=2zNrVsDj}{VALUE}% battle morale effect to friendly troops with your two handed kills.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=qxbBsB1a}{VALUE} party limit.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedRecklessCharge.Initialize("{=ZGovl01w}Reckless Charge", DefaultSkills.TwoHanded, this.GetTierCost(8), this._twoHandedThickHides, "{=1PC4D2fx}{VALUE}% damage bonus from speed with two handed weapons while on foot.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=b5l18lo7}{VALUE}% damage and movement speed to infantry in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._twoHandedThickHides.Initialize("{=j9OIuxxY}Thick Hides", DefaultSkills.TwoHanded, this.GetTierCost(8), this._twoHandedRecklessCharge, "{=Ti9auMiO}{VALUE} hit points.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "{=ucHrYWaz}{VALUE} hit points to troops in your party.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._twoHandedBladeMaster.Initialize("{=TtwAoHfw}Blade Master", DefaultSkills.TwoHanded, this.GetTierCost(9), this._twoHandedVandal, "{=Pq0bhBUL}{VALUE}% damage with two handed weapons.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=l0ZGaUuI}{VALUE}% attack speed to infantry in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._twoHandedVandal.Initialize("{=czCRxHZy}Vandal", DefaultSkills.TwoHanded, this.GetTierCost(9), this._twoHandedBladeMaster, "{=u57OuUZR}{VALUE}% armor penetration with your attacks.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=8q4vzfbH}{VALUE}% damage against destructible objects by troops in your formation.", SkillEffect.PerkRole.Captain, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._twoHandedWayOfTheGreatAxe.Initialize("{=dbEb7iak}Way Of The Great Axe", DefaultSkills.TwoHanded, this.GetTierCost(10), null, "{=yRvF2Li4}{VALUE}% attack speed with two handed weapons for every skill point above 250.", SkillEffect.PerkRole.Personal, 0.002f, SkillEffect.EffectIncrementType.AddFactor, "{=IM05Jahy}{VALUE}% damage with two handed weapons for every skill point above 250.", SkillEffect.PerkRole.Personal, 0.005f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._polearmPikeman.Initialize("{=IFqtwpb0}Pikeman", DefaultSkills.Polearm, this.GetTierCost(1), this._polearmCavalry, "{=NtzmIh0g}{VALUE}% damage with polearms on foot.", SkillEffect.PerkRole.Personal, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=Yu5hTuIN}{VALUE}% damage by infantry troops in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._polearmCavalry.Initialize("{=YVGtcLHF}Cavalry", DefaultSkills.Polearm, this.GetTierCost(1), this._polearmPikeman, "{=IaBTfvfc}{VALUE}% damage with polearms while mounted.", SkillEffect.PerkRole.Personal, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=ywc8frAo}{VALUE}% damage by cavalry troops in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted);
			this._polearmBraced.Initialize("{=dU7haWkI}Braced", DefaultSkills.Polearm, this.GetTierCost(2), this._polearmKeepAtBay, "{=mmFrvHRt}{VALUE}% chance of dismounting enemy cavalry with a heavy hit.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=OWXECmbt}{VALUE}% damage by infantry in your formation against cavalry.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._polearmKeepAtBay.Initialize("{=TaucWWCB}Keep at Bay", DefaultSkills.Polearm, this.GetTierCost(2), this._polearmBraced, "{=ObFtbGZl}{VALUE}% chance of knocking enemies back with thrust attacks made with polearms.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._polearmSwiftSwing.Initialize("{=xM5aawCj}Swift Swing", DefaultSkills.Polearm, this.GetTierCost(3), this._polearmCleanThrust, "{=7tdcIxYN}{VALUE}% swing speed with polearms.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=Tq0E9sSW}{VALUE}% swing speed to infantry in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee);
			this._polearmCleanThrust.Initialize("{=PeaiNrSu}Clean Thrust", DefaultSkills.Polearm, this.GetTierCost(3), this._polearmSwiftSwing, "{=xEp10rIa}{VALUE}% thrust damage with polearms.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=apgpk6j1}{VALUE} polearm skill to infantry in your formation.", SkillEffect.PerkRole.Captain, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot | TroopUsageFlags.PolearmUser);
			this._polearmFootwork.Initialize("{=Yvk8a2tb}Footwork", DefaultSkills.Polearm, this.GetTierCost(4), this._polearmHardKnock, "{=eDzl7r8u}{VALUE}% combat movement speed with polearms.", SkillEffect.PerkRole.Personal, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=1QsZq9UW}{VALUE}% movement speed to infantry in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._polearmHardKnock.Initialize("{=8DTKXbKw}Hard Knock", DefaultSkills.Polearm, this.GetTierCost(4), this._polearmFootwork, "{=7Nudvd8S}{VALUE}% chance of knocking the enemy down with a heavy hit.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=aeNhsyD7}{VALUE} hit points to infantry in your party.", SkillEffect.PerkRole.PartyLeader, 3f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._polearmSteedKiller.Initialize("{=8POWjrr6}Steed Killer", DefaultSkills.Polearm, this.GetTierCost(5), this._polearmLancer, "{=5aE8sEnj}{VALUE}% damage to mounts with polearms.", SkillEffect.PerkRole.Personal, 0.7f, SkillEffect.EffectIncrementType.AddFactor, "{=JGGdnIRO}{VALUE}% damage to mounts with polearms by infantry in your formation.", SkillEffect.PerkRole.Captain, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot | TroopUsageFlags.PolearmUser);
			this._polearmLancer.Initialize("{=hchDYAKL}Lancer", DefaultSkills.Polearm, this.GetTierCost(5), this._polearmSteedKiller, "{=I0hqrQuD}{VALUE}% damage bonus from speed with polearms while mounted.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=00mulBcs}{VALUE}% damage bonus from speed with polearms by troops in your formation.", SkillEffect.PerkRole.Captain, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.PolearmUser);
			this._polearmSkewer.Initialize("{=o57z0zB9}Skewer", DefaultSkills.Polearm, this.GetTierCost(6), this._polearmGuards, "{=dcVuMs9r}{VALUE}% chance of your lance staying couched after a kill.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=buFin46y}{VALUE} daily security in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._polearmGuards.Initialize("{=vquApOWo}Guards", DefaultSkills.Polearm, this.GetTierCost(6), this._polearmSkewer, "{=VB0GJijE}{VALUE}% damage when you hit an enemy in the head with a polearm.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=Ux90sIph}{VALUE}% experience gain to garrisoned cavalry in the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._polearmStandardBearer.Initialize("{=Vv81gkWN}Standard Bearer", DefaultSkills.Polearm, this.GetTierCost(7), this._polearmPhalanx, "{=RbDAfDsF}{VALUE}% battle morale loss to troops in your formation.", SkillEffect.PerkRole.Captain, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=V2v4ZMDT}{VALUE}% wages to garrisoned infantry in the governed settlement.", SkillEffect.PerkRole.Governor, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.None, TroopUsageFlags.Undefined);
			this._polearmPhalanx.Initialize("{=5vs3qlQ8}Phalanx", DefaultSkills.Polearm, this.GetTierCost(7), this._polearmStandardBearer, "{=3zzWzQcO}{VALUE} melee weapon skills to troops in your party while in shield wall formation.", SkillEffect.PerkRole.PartyLeader, 30f, SkillEffect.EffectIncrementType.Add, "{=yank0KD9}{VALUE}% damage with polearms by troops in your formation.", SkillEffect.PerkRole.Captain, 0.03f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.PolearmUser);
			this._polearmHardyFrontline.Initialize("{=NtMEk0lA}Hardy Frontline", DefaultSkills.Polearm, this.GetTierCost(8), this._polearmDrills, "{=ucHrYWaz}{VALUE} hit points to troops in your party.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, "{=3a6tmImq}{VALUE}% recruitment cost of infantry.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._polearmDrills.Initialize("{=JpiQagYa}Drills", DefaultSkills.Polearm, this.GetTierCost(8), this._polearmHardyFrontline, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, "{=x3SJTtDj}{VALUE} bonus daily experience to troops in your party.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._polearmSureFooted.Initialize("{=bdzt4TcN}Sure Footed", DefaultSkills.Polearm, this.GetTierCost(9), this._polearmUnstoppableForce, "{=QqVLsf0N}{VALUE}% charge damage taken.", SkillEffect.PerkRole.Personal, -0.4f, SkillEffect.EffectIncrementType.AddFactor, "{=Dilnx8Es}{VALUE}% charge damage taken by troops in your formation.", SkillEffect.PerkRole.Captain, -0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._polearmUnstoppableForce.Initialize("{=Jat5GFDi}Unstoppable Force", DefaultSkills.Polearm, this.GetTierCost(9), this._polearmSureFooted, "{=cUaUTwx5}Triple couch lance damage against shields.", SkillEffect.PerkRole.Personal, 3f, SkillEffect.EffectIncrementType.AddFactor, "{=jaLOtaRh}{VALUE}% damage bonus from speed with polearms to cavalry in your formation.", SkillEffect.PerkRole.Captain, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.PolearmUser);
			this._polearmCounterweight.Initialize("{=BazrgEOj}Counterweight", DefaultSkills.Polearm, this.GetTierCost(10), this._polearmSharpenTheTip, "{=02IdNzbt}{VALUE}% handling of swingable polearms.", SkillEffect.PerkRole.Personal, 0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=uZPweUQg}{VALUE} polearm skill to troops in your formation.", SkillEffect.PerkRole.Captain, 20f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.PolearmUser);
			this._polearmSharpenTheTip.Initialize("{=ljduhdzj}Sharpen the Tip", DefaultSkills.Polearm, this.GetTierCost(10), this._polearmCounterweight, "{=sbkrplXi}{VALUE}% damage with thrust attacks made with polearms.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=wLrF0mzr}{VALUE}% damage with thrust attacks by infantry troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot | TroopUsageFlags.Melee);
			this._polearmWayOfTheSpear.Initialize("{=YnimoRlg}Way of the Spear", DefaultSkills.Polearm, this.GetTierCost(11), null, "{=x1T8wWNU}{VALUE}% attack speed with polearms for every skill point above 250.", SkillEffect.PerkRole.Personal, 0.002f, SkillEffect.EffectIncrementType.AddFactor, "{=UB67Ye3r}{VALUE}% damage with polearms for every skill point above 250.", SkillEffect.PerkRole.Personal, 0.005f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowBowControl.Initialize("{=1zteHA7R}Bow Control", DefaultSkills.Bow, this.GetTierCost(1), this._bowDeadAim, "{=4PdKPMNc}{VALUE}% accuracy penalty while moving.", SkillEffect.PerkRole.Personal, -0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=0DaxFvnw}{VALUE}% damage with bows by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.BowUser);
			this._bowDeadAim.Initialize("{=FVLymWqW}Dead Aim", DefaultSkills.Bow, this.GetTierCost(1), this._bowBowControl, "{=hmbeQW4v}{VALUE}% headshot damage with bows.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=QbWK6sWo}{VALUE} Bow skill to troops in your formation.", SkillEffect.PerkRole.Captain, 20f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.BowUser);
			this._bowBodkin.Initialize("{=PDM8MzCA}Bodkin", DefaultSkills.Bow, this.GetTierCost(2), this._bowNockingPoint, "{=EU3No7XM}{VALUE}% armor penetration with bows.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=KfLZ8Hbw}{VALUE}% armor penetration with bows by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.BowUser);
			this._bowNockingPoint.Initialize("{=bS8alS24}Nocking Point", DefaultSkills.Bow, this.GetTierCost(2), this._bowBodkin, "{=zady0hI7}{VALUE}% movement speed penalty while reloading.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=kaJ6SJeI}{VALUE}% movement speed to archers in your formation.", SkillEffect.PerkRole.Captain, 0.03f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.BowUser);
			this._bowRapidFire.Initialize("{=Kc9oatmM}Rapid Fire", DefaultSkills.Bow, this.GetTierCost(3), this._bowQuickAdjustments, "{=0vqPUXfr}{VALUE}% reload speed with bows.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=KOlw0Na1}{VALUE}% reload speed to troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Ranged);
			this._bowQuickAdjustments.Initialize("{=nOZerIfl}Quick Adjustments", DefaultSkills.Bow, this.GetTierCost(3), this._bowRapidFire, "{=LAxaYQzv}{VALUE}% accuracy penalty while rotating.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=qC298I3g}{VALUE}% accuracy penalty to archers in your formation.", SkillEffect.PerkRole.Captain, -0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.BowUser);
			this._bowMerryMen.Initialize("{=ssljPTUr}Merry Men", DefaultSkills.Bow, this.GetTierCost(4), this._bowMountedArchery, "{=NouDSrXE}{VALUE} party size.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowMountedArchery.Initialize("{=WEUSMkCp}Mounted Archery", DefaultSkills.Bow, this.GetTierCost(4), this._bowMerryMen, "{=XITAY0KG}{VALUE}% accuracy penalty using bows while mounted.", SkillEffect.PerkRole.Personal, -0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=6XDcZUsb}{VALUE}% security provided by archers in the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowTrainer.Initialize("{=UE2X5rAy}Trainer", DefaultSkills.Bow, this.GetTierCost(5), this._bowStrongBows, "{=xoVR3Xr3}Daily Bow skill experience bonus to the party member with the lowest bow skill.", SkillEffect.PerkRole.PartyLeader, 6f, SkillEffect.EffectIncrementType.Add, "{=TcMme3Da}{VALUE} daily experience to archers in your party.", SkillEffect.PerkRole.PartyLeader, 3f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowStrongBows.Initialize("{=dqbbT5DK}Strong bows", DefaultSkills.Bow, this.GetTierCost(5), this._bowTrainer, "{=FXWn934b}{VALUE}% damage with bows.", SkillEffect.PerkRole.Personal, 0.08f, SkillEffect.EffectIncrementType.AddFactor, "{=yppPCR1z}{VALUE}% damage with bows by tier 3+ troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.BowUser);
			this._bowDiscipline.Initialize("{=D867vF71}Discipline", DefaultSkills.Bow, this.GetTierCost(6), this._bowHunterClan, "{=sHiIwnOb}{VALUE}% aiming duration without losing accuracy.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=F7bbkYx4}{VALUE} loyalty per day in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowHunterClan.Initialize("{=AAy1oX7z}Hunter Clan", DefaultSkills.Bow, this.GetTierCost(6), this._bowDiscipline, "{=kLVpYR0z}{VALUE}% damage with bows to mounts.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=1FPpHasQ}{VALUE}% garrison wages in the governed castle.", SkillEffect.PerkRole.Governor, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowSkirmishPhaseMaster.Initialize("{=oVdoauUE}Skirmish Phase Master", DefaultSkills.Bow, this.GetTierCost(7), this._bowEagleEye, "{=R93r6aV7}{VALUE}% damage taken from projectiles.", SkillEffect.PerkRole.Personal, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=pHdIgnYu}{VALUE}% damage taken from projectiles by ranged troops in your formation.", SkillEffect.PerkRole.Captain, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Ranged);
			this._bowEagleEye.Initialize("{=lq67KjSY}Eagle Eye", DefaultSkills.Bow, this.GetTierCost(7), this._bowSkirmishPhaseMaster, "{=xTDnna2J}{VALUE}% zoom with bows.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=1Z8oWbo7}{VALUE}% visual range on the campaign map.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowBullsEye.Initialize("{=QH77Weyq}Bulls Eye", DefaultSkills.Bow, this.GetTierCost(8), this._bowRenownedArcher, "{=OFMYfDPZ}{VALUE}% bonus experience to ranged troops in your party after every battle.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=mmH70R4H}{VALUE} daily experience to garrison troops in the governed settlement.", SkillEffect.PerkRole.Governor, 3f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Ranged, TroopUsageFlags.Undefined);
			this._bowRenownedArcher.Initialize("{=aIKVpGvE}Renowned Archer", DefaultSkills.Bow, this.GetTierCost(8), this._bowBullsEye, "{=kmdxvkEV}{VALUE}% starting battle morale to ranged troops in your party.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=bnnWpLbk}{VALUE}% recruitment and upgrade cost to ranged troops.", SkillEffect.PerkRole.PartyLeader, -0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowHorseMaster.Initialize("{=dbUybDTG}Horse Master", DefaultSkills.Bow, this.GetTierCost(9), this._bowDeepQuivers, "{=LiaRnWJZ}You can now use all bows on horseback.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Invalid, "{=0J9dgA7j}{VALUE} bow skill to horse archers in your formation", SkillEffect.PerkRole.Captain, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.BowUser);
			this._bowDeepQuivers.Initialize("{=h83ZU95t}Deep Quivers", DefaultSkills.Bow, this.GetTierCost(9), this._bowHorseMaster, "{=YOQQR1bJ}{VALUE} extra arrows per quiver.", SkillEffect.PerkRole.Personal, 3f, SkillEffect.EffectIncrementType.Add, "{=CBVfPRRe}{VALUE} extra arrow per quiver to troops in your party.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowQuickDraw.Initialize("{=vnJndBgT}Quick Draw", DefaultSkills.Bow, this.GetTierCost(10), this._bowRangersSwiftness, "{=jU084S5S}{VALUE}% aiming speed with bows.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=tsh4RXNa}{VALUE}% tax gain in the governed settlement.", SkillEffect.PerkRole.Governor, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowRangersSwiftness.Initialize("{=p12tSfCC}Ranger's Swiftness", DefaultSkills.Bow, this.GetTierCost(10), this._bowQuickDraw, "{=RQd005zy}Equipped bows do not slow you down.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Invalid, "{=6XDcZUsb}{VALUE}% security provided by archers in the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._bowDeadshot.Initialize("{=rsKhbZpJ}Deadshot", DefaultSkills.Bow, this.GetTierCost(11), null, "{=HCqd0IOt}{VALUE}% reload speed with bows for every skill point above 200.", SkillEffect.PerkRole.Personal, 0.002f, SkillEffect.EffectIncrementType.AddFactor, "{=hiFadSiC}{VALUE}% damage with bows for every skill point above 200.", SkillEffect.PerkRole.Personal, 0.005f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowPiercer.Initialize("{=v8RwzwqD}Piercer", DefaultSkills.Crossbow, this.GetTierCost(1), this._crossbowMarksmen, "{=j3J0Hbax}Your crossbow attacks ignore armors below 20.", SkillEffect.PerkRole.Personal, 20f, SkillEffect.EffectIncrementType.Add, "{=CLBXxPdh}{VALUE}% recruitment cost of ranged troops.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowMarksmen.Initialize("{=IUGVdu64}Marksmen", DefaultSkills.Crossbow, this.GetTierCost(1), this._crossbowPiercer, "{=LCsu8rXa}{VALUE}% faster aiming with crossbows.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=kmdxvkEV}{VALUE}% starting battle morale to ranged troops in your party.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowUnhorser.Initialize("{=75vJc53f}Unhorser", DefaultSkills.Crossbow, this.GetTierCost(2), this._crossbowWindWinder, "{=97nYJcKO}{VALUE}% crossbow damage to mounts.", SkillEffect.PerkRole.Personal, 0.4f, SkillEffect.EffectIncrementType.AddFactor, "{=laWxqjBP}{VALUE}% damage against mounts to crossbow troops in your formation.", SkillEffect.PerkRole.Captain, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._crossbowWindWinder.Initialize("{=1bw2uw8H}Wind Winder", DefaultSkills.Crossbow, this.GetTierCost(2), this._crossbowUnhorser, "{=3cBX5x0x}{VALUE}% reload speed with crossbows.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=YHjdf1uO}{VALUE}% crossbow reload speed to troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._crossbowDonkeysSwiftness.Initialize("{=uANbQUxg}Donkey's Swiftness", DefaultSkills.Crossbow, this.GetTierCost(3), this._crossbowSheriff, "{=Af7zOV2l}{VALUE}% accuracy loss while moving.", SkillEffect.PerkRole.Personal, -0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=aIyRxlCf}{VALUE} crossbow skill to troops in your formation.", SkillEffect.PerkRole.Captain, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._crossbowSheriff.Initialize("{=leaowE4D}Sheriff", DefaultSkills.Crossbow, this.GetTierCost(3), this._crossbowDonkeysSwiftness, "{=W7PaF7Lr}{VALUE}% headshot damage with crossbows.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=HB2wwuj6}{VALUE}% crossbow damage to infantry by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._crossbowPeasantLeader.Initialize("{=2rPMYl7Y}Peasant Leader", DefaultSkills.Crossbow, this.GetTierCost(4), this._crossbowRenownMarksmen, "{=4CSaYB8H}{VALUE}% battle morale to tier 1 to 3 troops", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=xuUbaa9f}{VALUE}% garrisoned ranged troop wages in the governed settlement.", SkillEffect.PerkRole.Governor, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowRenownMarksmen.Initialize("{=ICkvftaM}Renowned Marksmen", DefaultSkills.Crossbow, this.GetTierCost(4), this._crossbowPeasantLeader, "{=uj52xbdr}{VALUE} daily experience to ranged troops in your party.", SkillEffect.PerkRole.PartyLeader, 2f, SkillEffect.EffectIncrementType.Add, "{=i4GboakR}{VALUE}% security provided by ranged troops in the garrison of the governed settlement.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowFletcher.Initialize("{=FA5QlTvm}Fletcher", DefaultSkills.Crossbow, this.GetTierCost(5), this._crossbowPuncture, "{=wvQbeHpp}{VALUE} bolts per quiver.", SkillEffect.PerkRole.Personal, 4f, SkillEffect.EffectIncrementType.Add, "{=j3Hcp9RQ}{VALUE} bolts per quiver to troops in your party.", SkillEffect.PerkRole.PartyLeader, 2f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowPuncture.Initialize("{=jjkJyKoy}Puncture", DefaultSkills.Crossbow, this.GetTierCost(5), this._crossbowFletcher, "{=bVUQyN8t}{VALUE}% armor penetration with crossbows.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=KhCU9FZU}{VALUE}% armor penetration with crossbows by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._crossbowLooseAndMove.Initialize("{=SKUPHeAw}Loose and Move", DefaultSkills.Crossbow, this.GetTierCost(6), this._crossbowDeftHands, "{=BbaidhT4}Equipped crossbows do not slow you down.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "{=loVfqss6}{VALUE}% movement speed to ranged troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Ranged);
			this._crossbowDeftHands.Initialize("{=NYHeygaj}Deft Hands", DefaultSkills.Crossbow, this.GetTierCost(6), this._crossbowLooseAndMove, "{=VY7WQSgu}{VALUE}% resistance to getting staggered while reloading your crossbow.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=wUov3khT}{VALUE}% resistance to getting staggered while reloading crossbows to troops in your formation.", SkillEffect.PerkRole.Captain, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._crossbowCounterFire.Initialize("{=grgnisK4}Counter Fire", DefaultSkills.Crossbow, this.GetTierCost(7), this._crossbowMountedCrossbowman, "{=8ieLwTyG}{VALUE}% projectile damage taken while equipped with a crossbow.", SkillEffect.PerkRole.Personal, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=zJHhHRBw}{VALUE}% damage taken from projectiles by your troops.", SkillEffect.PerkRole.Captain, -0.03f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._crossbowMountedCrossbowman.Initialize("{=UZHvbYTr}Mounted Crossbowman", DefaultSkills.Crossbow, this.GetTierCost(7), this._crossbowCounterFire, "{=ZTt5Es7q}You can reload any crossbow on horseback.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "{=i36Gg6mW}{VALUE}% experience gained to ranged troops in your party.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowSteady.Initialize("{=Ye9lbBr3}Steady", DefaultSkills.Crossbow, this.GetTierCost(8), this._crossbowLongShots, "{=wFWdhNCN}{VALUE}% accuracy penalty with crossbows while mounted.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=q5IMLou4}{VALUE}% tariff gain in the governed settlement.", SkillEffect.PerkRole.Governor, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowLongShots.Initialize("{=Y5KXWHJY}Long Shots", DefaultSkills.Crossbow, this.GetTierCost(8), this._crossbowSteady, "{=Stykk4VR}{VALUE}% more zoom with crossbows.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.AddFactor, "{=xwA6Om0Y}{VALUE} daily militia recruitment in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowHammerBolts.Initialize("{=FjMS9Mbz}Hammer Bolts", DefaultSkills.Crossbow, this.GetTierCost(9), this._crossbowPavise, "{=K8VhWN85}{VALUE}% chance of dismounting enemy cavalry with a heavy hit from your crossbow.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=yz8xogMc}{VALUE}% damage with crossbows by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._crossbowPavise.Initialize("{=2667CwaK}Pavise", DefaultSkills.Crossbow, this.GetTierCost(9), this._crossbowHammerBolts, "{=pr5vaFNc}{VALUE}% chance of blocking projectiles from behind with a shield on your back.", SkillEffect.PerkRole.Personal, 0.75f, SkillEffect.EffectIncrementType.AddFactor, "{=Q8LSfvIO}{VALUE}% accuracy to ballistas in the governed settlement.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowTerror.Initialize("{=nAlCj2m0}Terror", DefaultSkills.Crossbow, this.GetTierCost(10), this._crossbowPickedShots, "{=NGFbn4Qx}{VALUE}% chance of increasing the siege bombardment casualties per hit by 1.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=sUFw8cDt}{VALUE}% morale loss to enemy due to crossbow kills by troops in your formation.", SkillEffect.PerkRole.Captain, 0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._crossbowPickedShots.Initialize("{=nGWmyZCs}Picked Shots", DefaultSkills.Crossbow, this.GetTierCost(10), this._crossbowTerror, "{=YG7HavAk}{VALUE}% wages of tier 4+ ranged troops.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=Yxchzh3a}{VALUE} hit points to ranged troops in your party.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._crossbowMightyPull.Initialize("{=ZFtyxzT5}Mighty Pull", DefaultSkills.Crossbow, this.GetTierCost(11), null, "{=Jtx8Czql}{VALUE}% reload speed with crossbows for every skill point above 200.", SkillEffect.PerkRole.Personal, 0.002f, SkillEffect.EffectIncrementType.AddFactor, "{=WUaSub02}{VALUE}% damage with crossbows for every skill point above 200.", SkillEffect.PerkRole.Personal, 0.005f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingQuickDraw.Initialize("{=vnJndBgT}Quick Draw", DefaultSkills.Throwing, this.GetTierCost(1), this._throwingShieldBreaker, "{=Fnbf4ShY}{VALUE}% draw speed with throwing weapons.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=UkADS8nQ}{VALUE}% draw speed with throwing weapons to troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingShieldBreaker.Initialize("{=DeWp2GjP}Shield Breaker", DefaultSkills.Throwing, this.GetTierCost(1), this._throwingQuickDraw, "{=wPwbyBra}{VALUE}% damage to shields with throwing weapons.", SkillEffect.PerkRole.Personal, 0.4f, SkillEffect.EffectIncrementType.AddFactor, "{=inFSdSiC}{VALUE}% damage to shields with throwing weapons by troops in your formation.", SkillEffect.PerkRole.Captain, 0.08f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingHunter.Initialize("{=xnDWqYKW}Hunter", DefaultSkills.Throwing, this.GetTierCost(2), this._throwingFlexibleFighter, "{=FPdjh976}{VALUE}% damage to mounts with throwing weapons.", SkillEffect.PerkRole.Personal, 0.4f, SkillEffect.EffectIncrementType.AddFactor, "{=ZgvRAR0u}{VALUE}% damage to mounts with throwing weapons by troops in your formation.", SkillEffect.PerkRole.Captain, 0.08f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingFlexibleFighter.Initialize("{=mPPWRjCZ}Flexible Fighter", DefaultSkills.Throwing, this.GetTierCost(2), this._throwingHunter, "{=6rEsV6SZ}{VALUE}% damage while using throwing weapons as melee.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=SSm1kkaB}{VALUE} Control skills of infantry, {VALUE} Vigor skills of archers in your formation.", SkillEffect.PerkRole.Captain, 15f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._throwingMountedSkirmisher.Initialize("{=l1I748Fn}Mounted Skirmisher", DefaultSkills.Throwing, this.GetTierCost(3), this._throwingWellPrepared, "{=JsdkJbDL}{VALUE}% accuracy penalty with throwing weapons while mounted.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=0L96iq1b}{VALUE}% damage with throwing weapons by mounted troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.ThrownUser);
			this._throwingWellPrepared.Initialize("{=bloxcikL}Well Prepared", DefaultSkills.Throwing, this.GetTierCost(3), this._throwingMountedSkirmisher, "{=nKw4eb22}{VALUE} ammunition for throwing weapons.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "{=1lEckrPh}{VALUE} ammunition for throwing weapons to troops in your party.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingRunningThrow.Initialize("{=OcaW12fJ}Running Throw", DefaultSkills.Throwing, this.GetTierCost(4), this._throwingKnockOff, "{=Z4maWcyl}{VALUE}% damage bonus from speed with throwing weapons.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.Add, "{=a5CWbHsd}{VALUE} throwing skill to troops in your formation.", SkillEffect.PerkRole.Captain, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingKnockOff.Initialize("{=Gn3KBN8L}Knock Off", DefaultSkills.Throwing, this.GetTierCost(4), this._throwingRunningThrow, "{=JqGLpG2L}{VALUE}% chance of dismounting enemy cavalry with a heavy hit with your throwing weapons.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=cJEbenVQ}{VALUE}% throwing weapon damage to cavalry by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.ThrownUser);
			this._throwingSkirmisher.Initialize("{=s9oED1IR}Skirmisher", DefaultSkills.Throwing, this.GetTierCost(5), this._throwingSaddlebags, "{=O6UPQskm}{VALUE}% damage taken by ranged attacks while holding a throwing weapon.", SkillEffect.PerkRole.Personal, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=ZUYOXMFo}{VALUE}% damage taken by ranged attacks to troops in your formation.", SkillEffect.PerkRole.Captain, -0.03f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._throwingSaddlebags.Initialize("{=VUxFbEiW}Saddlebags", DefaultSkills.Throwing, this.GetTierCost(5), this._throwingSkirmisher, "{=bFNFpd2d}{VALUE} ammunition for throwing weapons when you start a battle mounted.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.Add, "{=0jbhAPub}{VALUE} daily experience to infantry troops in your party.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingFocus.Initialize("{=throwingskillfocus}Focus", DefaultSkills.Throwing, this.GetTierCost(6), this._throwingLastHit, "{=hJdHb0G7}{VALUE}% zoom with throwing weapons.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=buFin46y}{VALUE} daily security in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingLastHit.Initialize("{=IsHyjvSq}Last Hit", DefaultSkills.Throwing, this.GetTierCost(6), this._throwingFocus, "{=PleZrXuO}{VALUE}% damage to enemies with less than half of their hit points left.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=g5nnybjz}{VALUE} starting battle morale to troops in your party.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingHeadHunter.Initialize("{=iARYMyuq}Head Hunter", DefaultSkills.Throwing, this.GetTierCost(7), this._throwingThrowingCompetitions, "{=UsD0y74h}{VALUE}% headshot damage with thrown weapons.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=j7inOz04}{VALUE}% recruitment cost of tier 2+ troops.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingThrowingCompetitions.Initialize("{=cC8iTtg5}Throwing Competitions", DefaultSkills.Throwing, this.GetTierCost(7), this._throwingHeadHunter, "{=W0cfTJQv}{VALUE}% upgrade cost of infantry troops.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingResourceful.Initialize("{=w53LSPJ1}Resourceful", DefaultSkills.Throwing, this.GetTierCost(8), this._throwingSplinters, "{=nKw4eb22}{VALUE} ammunition for throwing weapons.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.Add, "{=P0iCmQAf}{VALUE}% experience from battles to troops in your party equipped with throwing weapons.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingSplinters.Initialize("{=b6W74uyR}Splinters", DefaultSkills.Throwing, this.GetTierCost(8), this._throwingResourceful, "{=ymKzbcfB}Triple damage against shields with throwing axes.", SkillEffect.PerkRole.Personal, 3f, SkillEffect.EffectIncrementType.AddFactor, "{=inFSdSiC}{VALUE}% damage to shields with throwing weapons by troops in your formation.", SkillEffect.PerkRole.Captain, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingPerfectTechnique.Initialize("{=BCoQgZvG}Perfect Technique", DefaultSkills.Throwing, this.GetTierCost(9), this._throwingLongReach, "{=cr1AipGT}{VALUE}% travel speed to your throwing weapons.", SkillEffect.PerkRole.Personal, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=rkHnKmSK}{VALUE}% travel speed to throwing weapons of troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingLongReach.Initialize("{=9iLyu1kp}Long Reach", DefaultSkills.Throwing, this.GetTierCost(9), this._throwingPerfectTechnique, "{=lEi1hIIt}You can pick up items from the ground while mounted.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=VgkFpMxF}{VALUE}% morale and renown gained from battles won.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._throwingWeakSpot.Initialize("{=cPPLAz8l}Weak Spot", DefaultSkills.Throwing, this.GetTierCost(10), this._throwingImpale, "{=z4zrwc9K}{VALUE}% armor penetration with throwing weapons.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=b97khG1u}{VALUE}% armor penetration with throwing weapons by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingImpale.Initialize("{=tYAYIRjr}Impale", DefaultSkills.Throwing, this.GetTierCost(10), this._throwingWeakSpot, "{=D9coiXFt}Javelins you throw can penetrate shields.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=xlddWniu}{VALUE}% damage with throwing weapons by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._throwingUnstoppableForce.Initialize("{=Jat5GFDi}Unstoppable Force", DefaultSkills.Throwing, this.GetTierCost(11), null, "{=4MPzgKqE}{VALUE}% travel speed to your throwing weapons for every skill point above 200.", SkillEffect.PerkRole.Personal, 0.002f, SkillEffect.EffectIncrementType.AddFactor, "{=pDvv90Th}{VALUE}% damage with throwing weapons for every skill point above 200.", SkillEffect.PerkRole.Personal, 0.005f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingFullSpeed.Initialize("{=kzy9Iduz}Full Speed", DefaultSkills.Riding, this.GetTierCost(1), this._ridingNimbleSteed, "{=wKSA8Qob}{VALUE}% charge damage dealt.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=DS8fM8Op}{VALUE}% charge damage dealt by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted);
			this._ridingNimbleSteed.Initialize("{=cXlnH1Jp}Nimble Steed", DefaultSkills.Riding, this.GetTierCost(1), this._ridingFullSpeed, "{=f8R0Hkxa}{VALUE}% maneuvering.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=zjctOvv8}{VALUE} riding skill to troops in your formation.", SkillEffect.PerkRole.Captain, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted);
			this._ridingWellStraped.Initialize("{=3lfS4iCZ}Well Strapped", DefaultSkills.Riding, this.GetTierCost(2), this._ridingVeterinary, "{=oKWft2IH}{VALUE}% chance of your mount dying or becoming lame after it falls in battle.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=IkhQPr3Z}{VALUE} daily loyalty to the governed settlement.", SkillEffect.PerkRole.Governor, 0.5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingVeterinary.Initialize("{=ZaSmz64G}Veterinary", DefaultSkills.Riding, this.GetTierCost(2), this._ridingWellStraped, "{=tvRYz5lr}{VALUE}% hit points to your mount.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=b0w3Fruf}{VALUE}% hit points to mounts of troops in your party.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingNomadicTraditions.Initialize("{=PB5iowxh}Nomadic Traditions", DefaultSkills.Riding, this.GetTierCost(3), this._ridingDeeperSacks, "{=Wrmqdoz8}{VALUE}% party speed bonus from footmen on horses.", SkillEffect.PerkRole.PartyLeader, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=fPB1WdEy}{VALUE}% melee damage bonus from speed to mounted troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.Melee);
			this._ridingDeeperSacks.Initialize("{=VWYrJCje}Deeper Sacks", DefaultSkills.Riding, this.GetTierCost(3), this._ridingNomadicTraditions, "{=Yp4zv2ib}{VALUE}% carrying capacity for pack animals in your party.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=UC6JdOXk}{VALUE}% trade penalty for mounts.", SkillEffect.PerkRole.PartyLeader, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingSagittarius.Initialize("{=jbPZTSP4}Sagittarius", DefaultSkills.Riding, this.GetTierCost(4), this._ridingSweepingWind, "{=nc3carw2}{VALUE}% accuracy penalty while mounted.", SkillEffect.PerkRole.Personal, -0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=r0epmJJJ}{VALUE}% accuracy penalty to mounted troops in your formation.", SkillEffect.PerkRole.Captain, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.Ranged);
			this._ridingSweepingWind.Initialize("{=gL7Ltjpc}Sweeping Wind", DefaultSkills.Riding, this.GetTierCost(4), this._ridingSagittarius, "{=lTafHBwZ}{VALUE}% top speed to your mount.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=Q74nUiFJ}{VALUE}% party speed.", SkillEffect.PerkRole.PartyLeader, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingReliefForce.Initialize("{=g5I4qyjw}Relief Force", DefaultSkills.Riding, this.GetTierCost(5), null, "{=tx37EgiO}{VALUE} starting battle morale when you join an ongoing battle of your allies.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, "{=RVNPXS46}{VALUE}% security provided by mounted troops in the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingMountedWarrior.Initialize("{=ixqTFMVA}Mounted Warrior", DefaultSkills.Riding, this.GetTierCost(6), this._ridingHorseArcher, "{=1GwI0hcG}{VALUE}% mounted melee damage.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=f6sgEuS0}{VALUE}% mounted melee damage by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.Melee);
			this._ridingHorseArcher.Initialize("{=ugJfuabA}Horse Archer", DefaultSkills.Riding, this.GetTierCost(6), this._ridingMountedWarrior, "{=G4xCqSNG}{VALUE}% ranged damage while mounted.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=DFMsbFrB}{VALUE}% damage by mounted archers in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.BowUser);
			this._ridingShepherd.Initialize("{=I5LyCJzj}Shepherd", DefaultSkills.Riding, this.GetTierCost(7), this._ridingBreeder, "{=aiIPozp6}{VALUE}% herding speed penalty.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=YhZj58ut}{VALUE}% chance of producing tier 2 horses in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingBreeder.Initialize("{=4Pbfs4rV}Breeder", DefaultSkills.Riding, this.GetTierCost(7), this._ridingShepherd, "{=Cpaw42pv}{VALUE}% daily chance of animals in your party reproducing.", SkillEffect.PerkRole.PartyLeader, 0.01f, SkillEffect.EffectIncrementType.AddFactor, "{=665JbYIC}{VALUE}% production rate to villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingThunderousCharge.Initialize("{=3MLtqFPt}Thunderous Charge", DefaultSkills.Riding, this.GetTierCost(8), this._ridingAnnoyingBuzz, "{=qvjCYY61}{VALUE}% battle morale penalty to enemies with mounted melee kills.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=fK9GFdM8}{VALUE}% battle morale penalty to enemies with mounted melee kills by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.Melee);
			this._ridingAnnoyingBuzz.Initialize("{=Okibjv5n}Annoying Buzz", DefaultSkills.Riding, this.GetTierCost(8), this._ridingThunderousCharge, "{=nbbQfbli}{VALUE}% battle morale penalty to enemies with mounted ranged kills.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=wtdwqO8i}{VALUE}% battle morale penalty to enemies with mounted ranged kills by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted | TroopUsageFlags.Ranged);
			this._ridingMountedPatrols.Initialize("{=1z3oRPQu}Mounted Patrols", DefaultSkills.Riding, this.GetTierCost(9), this._ridingCavalryTactics, "{=pAkHwm3k}{VALUE}% escape chance to prisoners in your party.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=mNbAR4uk}{VALUE}% escape chance to prisoners in the governed settlement.", SkillEffect.PerkRole.Governor, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingCavalryTactics.Initialize("{=ZMxAGDKU}Cavalry Tactics", DefaultSkills.Riding, this.GetTierCost(9), this._ridingMountedPatrols, "{=oboqflX9}{VALUE}% volunteering rate of cavalry troops in the settlements governed by your clan.", SkillEffect.PerkRole.ClanLeader, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=mXGozqqL}{VALUE}% wages of mounted troops in the governed settlement.", SkillEffect.PerkRole.Governor, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._ridingDauntlessSteed.Initialize("{=eYzTvFEH}Dauntless Steed", DefaultSkills.Riding, this.GetTierCost(10), this._ridingToughSteed, "{=7uhottjU}{VALUE}% resistance to getting staggered while mounted.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=MEr2aoeC}{VALUE} armor to all equipped armor pieces of mounted troops in your formation.", SkillEffect.PerkRole.Captain, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted);
			this._ridingToughSteed.Initialize("{=vDNbHDfq}Tough Steed", DefaultSkills.Riding, this.GetTierCost(10), this._ridingDauntlessSteed, "{=svkQsokb}{VALUE}% armor to your mount.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=Ful5cXFa}{VALUE} armor to mounts of troops in your formation.", SkillEffect.PerkRole.Captain, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Mounted);
			this._ridingTheWayOfTheSaddle.Initialize("{=HvYgMtXO}The Way Of The Saddle", DefaultSkills.Riding, this.GetTierCost(11), null, "{=nXZktHa6}{VALUE} charge damage and maneuvering for every skill point above 250.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsMorningExercise.Initialize("{=ipwU1JT3}Morning Exercise", DefaultSkills.Athletics, this.GetTierCost(1), this._athleticsWellBuilt, "{=V53EYEXx}{VALUE}% combat movement speed.", SkillEffect.PerkRole.Personal, 0.03f, SkillEffect.EffectIncrementType.AddFactor, "{=nRvR1Rpc}{VALUE}% combat movement speed to troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._athleticsWellBuilt.Initialize("{=bigS7KHi}Well Built", DefaultSkills.Athletics, this.GetTierCost(1), this._athleticsMorningExercise, "{=Ti9auMiO}{VALUE} hit points.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "{=V4zyUiai}{VALUE} hit points to foot troops in your party.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsFury.Initialize("{=b0ak3yGV}Fury", DefaultSkills.Athletics, this.GetTierCost(2), this._athleticsFormFittingArmor, "{=Epwmv89M}{VALUE}% weapon handling while on foot.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=LGFsDic7}{VALUE}% weapon handling to foot troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._athleticsFormFittingArmor.Initialize("{=tp3p7R8E}Form Fitting Armor", DefaultSkills.Athletics, this.GetTierCost(2), this._athleticsFury, "{=86R9Ttgx}{VALUE}% armor weight.", SkillEffect.PerkRole.Personal, -0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=WpCx75Pc}{VALUE}% combat movement speed to tier 3+ foot troops in your formation.", SkillEffect.PerkRole.Captain, 0.04f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._athleticsImposingStature.Initialize("{=3hffzsoK}Imposing Stature", DefaultSkills.Athletics, this.GetTierCost(3), this._athleticsStamina, "{=qCaIau4o}{VALUE}% persuasion chance.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=NouDSrXE}{VALUE} party size.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsStamina.Initialize("{=2lCLp5eo}Stamina", DefaultSkills.Athletics, this.GetTierCost(3), this._athleticsImposingStature, "{=Lrm17UNm}{VALUE}% crafting stamina recovery rate.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=PNB9bHJd}{VALUE} prisoner limit and -10% escape chance to your prisoners.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsSprint.Initialize("{=864bKdc6}Sprint", DefaultSkills.Athletics, this.GetTierCost(4), this._athleticsPowerful, "{=mWezTaa1}{VALUE}% combat movement speed when you have no shields and no ranged weapons equipped.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=zoNKoZDZ}{VALUE}% combat movement speed to infantry troops in your formation.", SkillEffect.PerkRole.Captain, 0.03f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._athleticsPowerful.Initialize("{=UCpyo9hw}Powerful", DefaultSkills.Athletics, this.GetTierCost(4), this._athleticsSprint, "{=CglYgfiY}{VALUE}% damage with melee weapons.", SkillEffect.PerkRole.Personal, 0.04f, SkillEffect.EffectIncrementType.AddFactor, "{=eBmaa49a}{VALUE}% melee damage by troops in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Melee);
			this._athleticsSurgingBlow.Initialize("{=zrYFYDfj}Surging Blow", DefaultSkills.Athletics, this.GetTierCost(5), this._athleticsBraced, "{=QiZfTNWJ}{VALUE}% damage bonus from speed while on foot.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=m6RcG1bD}{VALUE}% damage bonus from speed to troops in your formation.", SkillEffect.PerkRole.Captain, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._athleticsBraced.Initialize("{=dU7haWkI}Braced", DefaultSkills.Athletics, this.GetTierCost(5), this._athleticsSurgingBlow, "{=QqVLsf0N}{VALUE}% charge damage taken.", SkillEffect.PerkRole.Personal, -0.4f, SkillEffect.EffectIncrementType.AddFactor, "{=Dilnx8Es}{VALUE}% charge damage taken by troops in your formation.", SkillEffect.PerkRole.Captain, -0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._athleticsWalkItOff.Initialize("{=0pyLfrGZ}Walk It Off", DefaultSkills.Athletics, this.GetTierCost(6), this._athleticsAGoodDaysRest, "{=65b6daHW}{VALUE}% hit point regeneration while traveling.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=9Hv0q2lg}{VALUE} daily experience to foot troops while traveling.", SkillEffect.PerkRole.PartyLeader, 3f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsAGoodDaysRest.Initialize("{=B7HwvV6L}A Good Days Rest", DefaultSkills.Athletics, this.GetTierCost(6), this._athleticsWalkItOff, "{=cCXt1jce}{VALUE}% hit point regeneration while waiting in settlements.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=fyibGRUQ}{VALUE} daily experience to foot troops while waiting in settlements.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsDurable.Initialize("{=8AKmJwv7}Durable", DefaultSkills.Athletics, this.GetTierCost(7), this._athleticsEnergetic, "{=4uqDestM}{VALUE} Endurance attribute.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "{=m993aVvX}{VALUE} daily loyalty in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsEnergetic.Initialize("{=1YxFYg3s}Energetic", DefaultSkills.Athletics, this.GetTierCost(7), this._athleticsDurable, "{=qPpN2wW8}{VALUE}% overburdened speed penalty.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=ULY7byYc}{VALUE}% hearth growth in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsSteady.Initialize("{=Ye9lbBr3}Steady", DefaultSkills.Athletics, this.GetTierCost(8), this._athleticsStrong, "{=C8LhGtUJ}{VALUE} Control attribute.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "{=rkQptw1O}{VALUE}% production in farms, mines, lumber camps and clay pits bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsStrong.Initialize("{=d5aK6Sv0}Strong", DefaultSkills.Athletics, this.GetTierCost(8), this._athleticsSteady, "{=gtlygHIk}{VALUE} Vigor attribute.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "{=yXaozMwY}{VALUE}% party speed by foot troops in your party.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsStrongLegs.Initialize("{=guZWnzaV}Strong Legs", DefaultSkills.Athletics, this.GetTierCost(9), this._athleticsStrongArms, "{=QIDr1cTd}{VALUE}% fall damage taken and +100% kick damage dealt.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=O3sh2iws}{VALUE}% food consumption in the governed settlement while under siege.", SkillEffect.PerkRole.Governor, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsStrongArms.Initialize("{=qBKmIyYx}Strong Arms", DefaultSkills.Athletics, this.GetTierCost(9), this._athleticsStrongLegs, "{=Ztezot02}{VALUE}% damage with throwing weapons.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=a5CWbHsd}{VALUE} throwing skill to troops in your formation.", SkillEffect.PerkRole.Captain, 20f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.ThrownUser);
			this._athleticsSpartan.Initialize("{=PX0Xufmr}Spartan", DefaultSkills.Athletics, this.GetTierCost(10), this._athleticsIgnorePain, "{=NmGcIg3j}{VALUE}% resistance to getting staggered while on foot.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=6NHvsrrx}{VALUE}% food consumption in your party.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._athleticsIgnorePain.Initialize("{=AHtFRv5T}Ignore Pain", DefaultSkills.Athletics, this.GetTierCost(10), this._athleticsSpartan, "{=1be7OEQB}{VALUE}% armor while on foot.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=F2H2lZJ4}{VALUE} armor to all equipped armor pieces of foot troops in your formation.", SkillEffect.PerkRole.Captain, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.OnFoot);
			this._athleticsMightyBlow.Initialize("{=lbGa4ihC}Mighty Blow ", DefaultSkills.Athletics, this.GetTierCost(11), null, "{=cqUXbafi}You stun your enemies longer after they block your attack.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=LItNgwiF}{VALUE} hit points for every skill point above 250.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingIronMaker.Initialize("{=i3eT3Zjb}Efficient Iron Maker", DefaultSkills.Crafting, this.GetTierCost(1), this._craftingCharcoalMaker, "{=6btajdpT}You can produce crude iron more efficiently by obtaining three units of crude iron from one unit of iron ore.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingCharcoalMaker.Initialize("{=u5zNNZKa}Efficient Charcoal Maker", DefaultSkills.Crafting, this.GetTierCost(1), this._craftingIronMaker, "{=wbwoVfSq}You can use a more efficient method of charcoal production that produces three units of charcoal from two units of hardwood.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingSteelMaker.Initialize("{=pKquYFTX}Steel Maker", DefaultSkills.Crafting, this.GetTierCost(2), this._craftingCuriousSmelter, "{=qZpIdBib}You can refine two units of iron into one unit of steel, and one unit of crude iron as by-product.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingCuriousSmelter.Initialize("{=Tu1Sd2qg}Curious Smelter", DefaultSkills.Crafting, this.GetTierCost(2), this._craftingSteelMaker, "{=1dS5OjLQ}{VALUE}% learning rate of new part designs when smelting.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingSteelMaker2.Initialize("{=EerNV0aM}Steel Maker 2", DefaultSkills.Crafting, this.GetTierCost(3), this._craftingCuriousSmith, "{=mm5ZzOOV}You can refine two units of steel into one unit of fine steel, and one unit of crude iron as by-product.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingCuriousSmith.Initialize("{=J1GSW0yk}Curious Smith", DefaultSkills.Crafting, this.GetTierCost(3), this._craftingSteelMaker2, "{=vWt9bvOz}{VALUE}% learning rate of new part designs when smithing.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingExperiencedSmith.Initialize("{=dwtIc9AG}Experienced Smith", DefaultSkills.Crafting, this.GetTierCost(4), this._craftingSteelMaker3, "{=w1K8XDls}{VALUE}% greater chance of creating Fine weapons.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=qPJJnxM1}Successful crafting orders of notables increase your relation by {VALUE} with them.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingSteelMaker3.Initialize("{=c5GOJIhU}Steel Maker 3", DefaultSkills.Crafting, this.GetTierCost(4), this._craftingExperiencedSmith, "{=fxGdAlI2}You can refine two units of fine steel into one unit of Thamaskene steel,{newline}and one unit of crude iron as by-product.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "{=3b4sjuMu}{VALUE} relationships with lords and ladies for successful crafting orders.", SkillEffect.PerkRole.Personal, 4f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingPracticalRefiner.Initialize("{=OrcSQyOb}Practical Refiner", DefaultSkills.Crafting, this.GetTierCost(5), this._craftingPracticalSmelter, "{=hmrUcvwz}{VALUE}% stamina spent while refining.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingPracticalSmelter.Initialize("{=KFpnAWwr}Practical Smelter", DefaultSkills.Crafting, this.GetTierCost(5), this._craftingPracticalRefiner, "{=NzlwbSIj}{VALUE}% stamina spent while smelting.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingVigorousSmith.Initialize("{=8hhS659w}Vigorous Smith", DefaultSkills.Crafting, this.GetTierCost(6), this._craftingStrongSmith, "{=gtlygHIk}{VALUE} Vigor attribute.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingStrongSmith.Initialize("{=83iwPPVH}Controlled Smith", DefaultSkills.Crafting, this.GetTierCost(6), this._craftingVigorousSmith, "{=C8LhGtUJ}{VALUE} Control attribute.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingPracticalSmith.Initialize("{=rR8iTDPI}Practical Smith", DefaultSkills.Crafting, this.GetTierCost(7), this._craftingArtisanSmith, "{=FqmS9wcP}{VALUE}% stamina spent while smithing.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingArtisanSmith.Initialize("{=bnVCX24q}Artisan Smith", DefaultSkills.Crafting, this.GetTierCost(7), this._craftingPracticalSmith, "{=W9pOfMAE}{VALUE}% trade penalty when selling smithing weapons.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingMasterSmith.Initialize("{=ivH8RWyb}Master Smith", DefaultSkills.Crafting, this.GetTierCost(8), null, "{=SBTTId7I}{VALUE}% greater chance of creating masterwork weapons.", SkillEffect.PerkRole.Personal, 0.075f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingFencerSmith.Initialize("{=SSdYsV4R}Fencer Smith", DefaultSkills.Crafting, this.GetTierCost(9), this._craftingEnduringSmith, "{=j3QNVqP5}{VALUE} Focus Point to One Handed and Two Handed.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingEnduringSmith.Initialize("{=RWMACSag}Enduring Smith", DefaultSkills.Crafting, this.GetTierCost(9), this._craftingFencerSmith, "{=4uqDestM}{VALUE} Endurance attribute.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingSharpenedEdge.Initialize("{=knWgaYdk}Sharpened Edge", DefaultSkills.Crafting, this.GetTierCost(10), this._craftingSharpenedTip, "{=S7BMf2Wa}{VALUE}% swing damage of crafted weapons.", SkillEffect.PerkRole.Personal, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingSharpenedTip.Initialize("{=aO2JSbSq}Sharpened Tip", DefaultSkills.Crafting, this.GetTierCost(10), this._craftingSharpenedEdge, "{=KabSHyf0}{VALUE}% thrust damage of crafted weapons.", SkillEffect.PerkRole.Personal, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._craftingLegendarySmith.Initialize("{=f4lnEplc}Legendary Smith", DefaultSkills.Crafting, this.GetTierCost(11), null, "{=wc15ZSpO}{VALUE}% greater chance of creating Legendary weapons, chance increases by 1% for every 5 skill points above 275.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingDayTraveler.Initialize("{=6PSgX2BP}Day Traveler", DefaultSkills.Scouting, this.GetTierCost(1), this._scoutingNightRunner, "{=86nHAJs9}{VALUE}% travel speed during daytime.", SkillEffect.PerkRole.Scout, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=RUEfCZBG}{VALUE}% sight range during daytime in campaign map.", SkillEffect.PerkRole.Scout, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingNightRunner.Initialize("{=B8Gq2ylh}Night Runner", DefaultSkills.Scouting, this.GetTierCost(1), this._scoutingDayTraveler, "{=QmaIRD7P}{VALUE}% travel speed during nighttime", SkillEffect.PerkRole.Scout, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=6CteaPKm}{VALUE}% sight range during nighttime in campaign map.", SkillEffect.PerkRole.Scout, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingPathfinder.Initialize("{=d2qGHXyx}Pathfinder", DefaultSkills.Scouting, this.GetTierCost(2), this._scoutingWaterDiviner, "{=ETiOGIvT}{VALUE}% travel speed on steppes and plains.", SkillEffect.PerkRole.Scout, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=sAv1co78}{VALUE}% daily chance to increase relation with a notable by 1 when you enter a town.", SkillEffect.PerkRole.PartyLeader, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingWaterDiviner.Initialize("{=gsz9DMNq}Water Diviner", DefaultSkills.Scouting, this.GetTierCost(2), this._scoutingPathfinder, "{=8EtK0F1K}{VALUE}% sight range while traveling on steppes and plains.", SkillEffect.PerkRole.Scout, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=aW1qO2dN}{VALUE}% daily chance to increase relation with a notable by 1 when you enter a village.", SkillEffect.PerkRole.PartyLeader, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingForestKin.Initialize("{=0XuFh3cX}Forest Kin", DefaultSkills.Scouting, this.GetTierCost(3), this._scoutingDesertBorn, "{=cpbKNtlZ}{VALUE}% travel speed penalty from forests if your party is composed of 75% or more infantry units.", SkillEffect.PerkRole.Scout, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=xq9wJPKI}{VALUE}% tax income from villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingDesertBorn.Initialize("{=TbBmjK8M}Desert Born", DefaultSkills.Scouting, this.GetTierCost(3), this._scoutingForestKin, "{=k9WaJ396}{VALUE}% travel speed on deserts and dunes.", SkillEffect.PerkRole.Scout, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=nUJfb5VX}{VALUE}% tax income from the governed settlement.", SkillEffect.PerkRole.Governor, 0.025f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingForcedMarch.Initialize("{=jhZe9Mfo}Forced March", DefaultSkills.Scouting, this.GetTierCost(4), this._scoutingUnburdened, "{=zky6r5Ax}{VALUE}% travel speed when the party morale is higher than 75.", SkillEffect.PerkRole.Scout, 0.025f, SkillEffect.EffectIncrementType.AddFactor, "{=hLbn3SBE}{VALUE} experience per day to all troops while traveling with party morale higher than 75.", SkillEffect.PerkRole.PartyLeader, 2f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingUnburdened.Initialize("{=sA2OrT6l}Unburdened", DefaultSkills.Scouting, this.GetTierCost(4), this._scoutingForcedMarch, "{=N5jFSdGR}{VALUE}% overburden penalty.", SkillEffect.PerkRole.Scout, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=OJ9QCJh8}{VALUE} experience per day to all troops when traveling while overburdened.", SkillEffect.PerkRole.PartyLeader, 2f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingTracker.Initialize("{=AoaabumE}Tracker", DefaultSkills.Scouting, this.GetTierCost(5), this._scoutingRanger, "{=mTHliJuT}{VALUE}% track visibility duration.", SkillEffect.PerkRole.Scout, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=pnAq0a40}{VALUE}% travel speed while following a hostile party.", SkillEffect.PerkRole.Scout, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingRanger.Initialize("{=09gOOa0h}Ranger", DefaultSkills.Scouting, this.GetTierCost(5), this._scoutingTracker, "{=boXP9vkF}{VALUE}% track spotting distance.", SkillEffect.PerkRole.Scout, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=aeK3ykbL}{VALUE}% track detection chance.", SkillEffect.PerkRole.Scout, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingMountedScouts.Initialize("{=K9Nb117q}Mounted Scouts", DefaultSkills.Scouting, this.GetTierCost(6), this._scoutingPatrols, "{=DHZxUm6I}{VALUE}% sight range when your party is composed of more than %50 cavalry troops.", SkillEffect.PerkRole.Scout, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=rLs30aPf}{VALUE} party size limit.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingPatrols.Initialize("{=uKc4le8Q}Patrols", DefaultSkills.Scouting, this.GetTierCost(6), this._scoutingMountedScouts, "{=2ljMER8Z}{VALUE} battle morale against bandit parties.", SkillEffect.PerkRole.Scout, 5f, SkillEffect.EffectIncrementType.Add, "{=7K0BqbFG}{VALUE}% advantage against bandits when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingForagers.Initialize("{=LPxEDIk7}Foragers", DefaultSkills.Scouting, this.GetTierCost(7), this._scoutingBeastWhisperer, "{=FepLiMeY}{VALUE}% food consumption while traveling through steppes and forests.", SkillEffect.PerkRole.Scout, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=kjn1D5Td}{VALUE}% disorganized state duration.", SkillEffect.PerkRole.PartyLeader, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingBeastWhisperer.Initialize("{=mrtDAhtL}Beast Whisperer", DefaultSkills.Scouting, this.GetTierCost(7), this._scoutingForagers, "{=jGAe89hM}{VALUE}% chance to find a mount when traveling through steppes and plains.", SkillEffect.PerkRole.Scout, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=Yp4zv2ib}{VALUE}% carrying capacity for pack animals in your party.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingVillageNetwork.Initialize("{=lYQAuYaH}Village Network", DefaultSkills.Scouting, this.GetTierCost(8), this._scoutingRumourNetwork, "{=zj4Sz28B}{VALUE}% trade penalty with villages of your own culture.", SkillEffect.PerkRole.PartyLeader, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=j9KDLaDo}{VALUE}% villager party size of villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingRumourNetwork.Initialize("{=LwWyc6ou}Rumor Network", DefaultSkills.Scouting, this.GetTierCost(8), this._scoutingVillageNetwork, "{=c7V0ayuX}{VALUE}% trade penalty within cities of your own kingdom.", SkillEffect.PerkRole.PartyLeader, -0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=JrTtFFfe}{VALUE}% hideout detection range.", SkillEffect.PerkRole.Scout, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingVantagePoint.Initialize("{=EC2n5DBl}Vantage Point", DefaultSkills.Scouting, this.GetTierCost(9), this._scoutingKeenSight, "{=Y1lC59hw}{VALUE}% sight range when stationary for at least an hour.", SkillEffect.PerkRole.Scout, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=POp8DAZD}{VALUE} prisoner limit.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingKeenSight.Initialize("{=3yVPrhXt}Keen Sight", DefaultSkills.Scouting, this.GetTierCost(9), this._scoutingVantagePoint, "{=dt1xXqbD}{VALUE}% sight penalty for traveling in forests.", SkillEffect.PerkRole.Scout, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=Lr7TZOFL}{VALUE}% chance of prisoner lords escaping from your party.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingVanguard.Initialize("{=Cp7dI87a}Vanguard", DefaultSkills.Scouting, this.GetTierCost(10), this._scoutingRearguard, "{=9yN8Fpv6}{VALUE}% damage by your troops when they are sent as attackers.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=Bzoxobzn}{VALUE}% damage by your troops when they are sent to sally out.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingRearguard.Initialize("{=e4QAc5A6}Rearguard", DefaultSkills.Scouting, this.GetTierCost(10), this._scoutingVanguard, "{=WlAAsJNK}{VALUE}% wounded troop recovery speed while in an army.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=dlnOcIyj}{VALUE}% damage by your troops when defending at your siege camp.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._scoutingUncannyInsight.Initialize("{=M9vC9mio}Uncanny Insight", DefaultSkills.Scouting, this.GetTierCost(11), null, "{=4Onw6Gxa}{VALUE}% party speed for every skill point above 200 scouting skill.", SkillEffect.PerkRole.Scout, 0.001f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsTightFormations.Initialize("{=EX5cZDLH}Tight Formations", DefaultSkills.Tactics, this.GetTierCost(1), this._tacticsLooseFormations, "{=eJ8AN9Au}{VALUE}% damage by your infantry to cavalry when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=gJJ2F3iL}{VALUE}% morale penalty when troops in your formation use shield wall, square, skein, column formations.", SkillEffect.PerkRole.Captain, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsLooseFormations.Initialize("{=9y3X0MQg}Loose Formations", DefaultSkills.Tactics, this.GetTierCost(1), this._tacticsTightFormations, "{=Xykn90RV}{VALUE}% damage to your infantry from ranged troops when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=jZzVlDlf}{VALUE}% morale penalty when troops in your formation use line, loose, circle or scatter formations.", SkillEffect.PerkRole.Captain, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsExtendedSkirmish.Initialize("{=EsYYcvcA}Extended Skirmish", DefaultSkills.Tactics, this.GetTierCost(2), this._tacticsDecisiveBattle, "{=Jm0GA3ak}{VALUE}% damage in snowy and forest terrains when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=U3B7zaQb}{VALUE}% movement speed to troops in your formation in snowy and forest terrains.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsDecisiveBattle.Initialize("{=4ElA7gRS}Decisive Battle", DefaultSkills.Tactics, this.GetTierCost(2), this._tacticsExtendedSkirmish, "{=CcggbEVk}{VALUE}% damage in plains, steppes and deserts when your troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=7yOCFsG5}{VALUE}% movement speed to troops in your formation in plains, steppes and deserts.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsSmallUnitTactics.Initialize("{=30hNRt3x}Small Unit Tactics", DefaultSkills.Tactics, this.GetTierCost(3), this._tacticsHordeLeader, "{=3mJMAX0Y}{VALUE} troop for the hideout crew", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, "{=GDSQyMaG}{VALUE}% movement speed to troops in your formation when there are less than 15 soldiers.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsHordeLeader.Initialize("{=Vp8Pwou8}Horde Leader", DefaultSkills.Tactics, this.GetTierCost(3), this._tacticsSmallUnitTactics, "{=NouDSrXE}{VALUE} party size.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, "{=y52Zz7U9}{VALUE}% army cohesion loss to commanded armies.", SkillEffect.PerkRole.ArmyCommander, -0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsLawKeeper.Initialize("{=zUK9JOlb}Law Keeper", DefaultSkills.Tactics, this.GetTierCost(4), this._tacticsCoaching, "{=QOMr1QS7}{VALUE}% damage against bandits when your troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=yfRAX2Qv}{VALUE}% damage against bandits by troops in your formation.", SkillEffect.PerkRole.Captain, 0.04f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsCoaching.Initialize("{=afaCdojS}Coaching", DefaultSkills.Tactics, this.GetTierCost(4), this._tacticsLawKeeper, "{=KSWdxKPJ}{VALUE}% damage when your troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.03f, SkillEffect.EffectIncrementType.AddFactor, "{=9CjoaJwe}{VALUE}% damage by troops in your formation.", SkillEffect.PerkRole.Captain, 0.01f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsSwiftRegroup.Initialize("{=nmJe4wN1}Swift Regroup", DefaultSkills.Tactics, this.GetTierCost(5), this._tacticsImproviser, "{=9f16mDn0}{VALUE}% disorganized state duration when a raid or siege is broken.", SkillEffect.PerkRole.PartyMember, -0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=0pW4fcjt}{VALUE}% troops left behind when escaping from battles.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsImproviser.Initialize("{=qAn93jVN}Improviser", DefaultSkills.Tactics, this.GetTierCost(5), this._tacticsSwiftRegroup, "{=pFSWDNaF}No morale penalty for disorganized state in battles, in sally out or when being attacked.", SkillEffect.PerkRole.PartyMember, 0f, SkillEffect.EffectIncrementType.Add, "{=4V8CS018}{VALUE}% loss of troops when breaking into or out of a settlement under siege.", SkillEffect.PerkRole.PartyLeader, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsOnTheMarch.Initialize("{=kolBffjD}On The March", DefaultSkills.Tactics, this.GetTierCost(6), this._tacticsCallToArms, "{=C6rYWvrb}{VALUE}% fortification bonus to enemies when troops are sent to confront the enemy.", SkillEffect.PerkRole.ArmyCommander, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=09npcQY0}{VALUE}% fortification bonus to the governed settlement", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsCallToArms.Initialize("{=mUubYb7v}Call To Arms", DefaultSkills.Tactics, this.GetTierCost(6), this._tacticsOnTheMarch, "{=3UB3qhjk}{VALUE}% movement speed to parties called to your army.", SkillEffect.PerkRole.ArmyCommander, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=mAKqS7Rk}{VALUE}% influence required to call parties to your army", SkillEffect.PerkRole.ArmyCommander, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsPickThemOfTheWalls.Initialize("{=XQkY7jkL}Pick Them Off The Walls", DefaultSkills.Tactics, this.GetTierCost(7), this._tacticsMakeThemPay, "{=mmRmG5AY}{VALUE}% chance for dealing double damage to siege defender troops in siege bombardment", SkillEffect.PerkRole.Engineer, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=bBRA2jJp}{VALUE}% chance for dealing double damage to besieging troops in siege bombardment of the governed settlement.", SkillEffect.PerkRole.Governor, 0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsMakeThemPay.Initialize("{=8xxeNK0o}Make Them Pay", DefaultSkills.Tactics, this.GetTierCost(7), this._tacticsPickThemOfTheWalls, "{=e2N77Ufi}{VALUE}% damage to defender siege engines.", SkillEffect.PerkRole.Engineer, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=hDvZTHbq}{VALUE}% damage to besieging siege engines.", SkillEffect.PerkRole.Governor, 0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsEliteReserves.Initialize("{=luDtfSN7}Elite Reserves", DefaultSkills.Tactics, this.GetTierCost(8), this._tacticsEncirclement, "{=zVEvl8WQ}{VALUE}% less damage to tier 3+ units when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=ldD4sVOi}{VALUE}% damage taken by troops in your formation.", SkillEffect.PerkRole.Captain, -0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._tacticsEncirclement.Initialize("{=EhaMPtRX}Encirclement", DefaultSkills.Tactics, this.GetTierCost(8), this._tacticsEliteReserves, "{=seiduCHq}{VALUE}% damage to outnumbered enemies when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=mtB1tUIb}{VALUE}% influence cost to boost army cohesion.", SkillEffect.PerkRole.ArmyCommander, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsPreBattleManeuvers.Initialize("{=cHgLxbbc}Pre Battle Maneuvers", DefaultSkills.Tactics, this.GetTierCost(9), this._tacticsBesieged, "{=dPo5goLo}{VALUE}% influence gain from winning battles.", SkillEffect.PerkRole.PartyMember, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=70PZ5mFx}{VALUE}% damage per 100 skill difference with the enemy when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.01f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsBesieged.Initialize("{=ALC3Kzv9}Besieged", DefaultSkills.Tactics, this.GetTierCost(9), this._tacticsPreBattleManeuvers, "{=gjkWXuwC}{VALUE}% damage while besieged when troops are sent to confront the enemy.", SkillEffect.PerkRole.PartyMember, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=SIMwGiJF}{VALUE}% influence gain from winning sieges.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsCounteroffensive.Initialize("{=mn5tQhyp}Counter Offensive", DefaultSkills.Tactics, this.GetTierCost(10), this._tacticsGensdarmes, "{=FQppujVl}{VALUE}% damage when troops are sent to confront the attacking enemy in a field battle.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=4Xb1xtbF}{VALUE}% damage when troops are sent to confront the enemy while outnumbered.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tacticsGensdarmes.Initialize("{=CTEuBfU0}Gens d'armes", DefaultSkills.Tactics, this.GetTierCost(10), this._tacticsCounteroffensive, "{=cPvszBhr}{VALUE}% damage to infantry by cavalry troops in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=buFin46y}{VALUE} daily security in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Mounted, TroopUsageFlags.Undefined);
			this._tacticsTacticalMastery.Initialize("{=8rvpcb4k}Tactical Mastery", DefaultSkills.Tactics, this.GetTierCost(11), null, "{=ClrLzkvx}{VALUE}% damage for every skill point above 200 tactics skill when troops are sent to confront the enemy.", SkillEffect.PerkRole.ArmyCommander, 0.005f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryNoRestForTheWicked.Initialize("{=RyfFWmDs}No Rest for the Wicked", DefaultSkills.Roguery, this.GetTierCost(1), this._roguerySweetTalker, "{=yZarNiMq}{VALUE}% experience gain for bandits in your party.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=IqRNTls2}{VALUE}% raid speed.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._roguerySweetTalker.Initialize("{=570wiYEe}Sweet Talker", DefaultSkills.Roguery, this.GetTierCost(1), this._rogueryNoRestForTheWicked, "{=P3d4nn88}{VALUE}% chance for convincing bandits to leave in peace with barter.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=icyzOJZf}{VALUE}% prisoner escape chance in the governed settlement.", SkillEffect.PerkRole.Governor, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryTwoFaced.Initialize("{=kg4Mx9j4}Two Faced", DefaultSkills.Roguery, this.GetTierCost(2), this._rogueryDeepPockets, "{=uDRb7FmU}{VALUE}% increased chance for sneaking into towns", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=PznnUlI3}No morale loss from converting bandit prisoners.", SkillEffect.PerkRole.PartyLeader, 0f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryDeepPockets.Initialize("{=by1b61pn}Deep Pockets", DefaultSkills.Roguery, this.GetTierCost(2), this._rogueryTwoFaced, "{=ixiL39S4}Double the amount of betting allowed in tournaments.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.AddFactor, "{=xSKhyecU}{VALUE}% bandit troop wages.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryInBestLight.Initialize("{=xoARIHde}In Best Light", DefaultSkills.Roguery, this.GetTierCost(3), this._rogueryKnowHow, "{=fcraUMzb}{VALUE} extra troop from village notables when successfully forced for volunteers.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, "{=UYYntLzb}{VALUE}% faster recovery from raids for your villages.", SkillEffect.PerkRole.ClanLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryKnowHow.Initialize("{=tvoN5ynt}Know-How", DefaultSkills.Roguery, this.GetTierCost(3), this._rogueryInBestLight, "{=swgcsLOA}{VALUE}% more loot from defeated villagers and caravans.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=XwmYu5rH}{VALUE} security per day in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryPromises.Initialize("{=XZOtTuxA}Promises", DefaultSkills.Roguery, this.GetTierCost(4), this._rogueryManhunter, "{=jKUmtH7z}{VALUE}% food consumption for bandit units in your party.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=zHQuHeIg}{VALUE}% recruitment rate for bandit prisoners in your party.", SkillEffect.PerkRole.PartyLeader, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryManhunter.Initialize("{=GeB42ygg}Manhunter", DefaultSkills.Roguery, this.GetTierCost(4), this._rogueryPromises, "{=pcys1RSF}{VALUE}% better deals with ransom broker for regular troops.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=POp8DAZD}{VALUE} prisoner limit.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryScarface.Initialize("{=XqSn5Uo0}Scarface", DefaultSkills.Roguery, this.GetTierCost(5), this._rogueryWhiteLies, "{=FaGc9xR4}{VALUE}% chance for bandits, villagers and caravans to surrender.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=1IYP1wHc}{VALUE}% chance per day to increase relation with a notable by 1 in the governed settlement.", SkillEffect.PerkRole.Governor, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryWhiteLies.Initialize("{=F51HfzZj}White Lies", DefaultSkills.Roguery, this.GetTierCost(5), this._rogueryScarface, "{=mseUsbjg}{VALUE}% crime rating decrease rate.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=R8vLC6j0}{VALUE}% chance to get 1 relation per day with a random notable in the governed settlement.", SkillEffect.PerkRole.Governor, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._roguerySmugglerConnections.Initialize("{=E8a2joMO}Smuggler Connections", DefaultSkills.Roguery, this.GetTierCost(6), this._rogueryPartnersInCrime, "{=XZe7YPLJ}{VALUE} armor points to equipped civilian body armors.", SkillEffect.PerkRole.Personal, 10f, SkillEffect.EffectIncrementType.Add, "{=gXmzmJbg}{VALUE}% trade penalty when you have crime rating.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryPartnersInCrime.Initialize("{=2PVm7NON}Partners in Crime", DefaultSkills.Roguery, this.GetTierCost(6), this._roguerySmugglerConnections, "{=zFfkR2uK}Surrendering bandit parties always offer to join you.", SkillEffect.PerkRole.PartyLeader, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=mNleBavO}{VALUE}% damage by bandit troops in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._rogueryOneOfTheFamily.Initialize("{=oumTabhS}One of the Family", DefaultSkills.Roguery, this.GetTierCost(7), this._roguerySaltTheEarth, "{=w0LOgr9e}{VALUE} bonus Vigor and Control skills to bandit units in your party", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, "{=Dn0yNCn8}{VALUE} recruitment slot when recruiting from gang leaders.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._roguerySaltTheEarth.Initialize("{=tuv1O7ig}Salt the Earth", DefaultSkills.Roguery, this.GetTierCost(7), this._rogueryOneOfTheFamily, "{=MesU0nGW}{VALUE}% more loot when villagers comply to your hostile actions.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=YbioVwyr}{VALUE}% tariff revenue in the governed settlement.", SkillEffect.PerkRole.Governor, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryCarver.Initialize("{=7gZo2SY4}Carver", DefaultSkills.Roguery, this.GetTierCost(8), this._rogueryRansomBroker, "{=g2Zy1Bso}{VALUE}% damage with civilian weapons.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=hiH9dVhH}{VALUE}% one handed damage by troops under your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.OneHandedUser);
			this._rogueryRansomBroker.Initialize("{=W2WXkiAh}Ransom Broker", DefaultSkills.Roguery, this.GetTierCost(8), this._rogueryCarver, "{=7gabTf4P}{VALUE}% better deals for heroes from ransom brokers.", SkillEffect.PerkRole.PartyLeader, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=8aajPkKG}{VALUE}% escape chance for hero prisoners.", SkillEffect.PerkRole.PartyLeader, -0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryArmsDealer.Initialize("{=5bmlZ26b}Arms Dealer", DefaultSkills.Roguery, this.GetTierCost(9), this._rogueryDirtyFighting, "{=o5rp0ViP}{VALUE}% sell price penalty for weapons.", SkillEffect.PerkRole.PartyLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=nmTai1Vw}{VALUE}% militia per day in the besieged governed settlement.", SkillEffect.PerkRole.Governor, 2f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryDirtyFighting.Initialize("{=bb1hS9I4}Dirty Fighting", DefaultSkills.Roguery, this.GetTierCost(9), this._rogueryArmsDealer, "{=bm3eSbBD}{VALUE}% stun duration for kicking.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=iuCYTaMJ}{VALUE} random food item will be smuggled to the besieged governed settlement.", SkillEffect.PerkRole.Governor, 2f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryDashAndSlash.Initialize("{=w1B71sNj}Dash and Slash", DefaultSkills.Roguery, this.GetTierCost(10), this._rogueryFleetFooted, "{=QiZfTNWJ}{VALUE}% damage bonus from speed while on foot.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=hRCvgbQ5}{VALUE}% two handed weapon damage by troops in your formation.", SkillEffect.PerkRole.Captain, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.TwoHandedUser);
			this._rogueryFleetFooted.Initialize("{=yY5iDvAb}Fleet Footed", DefaultSkills.Roguery, this.GetTierCost(10), this._rogueryDashAndSlash, "{=93Z7G161}{VALUE}% combat movement speed while no weapons or shields are equipped.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=lSebD5Fa}{VALUE}% escape chance when imprisoned by mobile parties.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._rogueryRogueExtraordinaire.Initialize("{=U3cgqyUE}Rogue Extraordinaire", DefaultSkills.Roguery, this.GetTierCost(11), null, "{=ClrwacPi}{VALUE}% loot amount for every skill point above 200.", SkillEffect.PerkRole.Personal, 0.01f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmVirile.Initialize("{=mbqoZ4WH}Virile", DefaultSkills.Charm, this.GetTierCost(1), this._charmSelfPromoter, "{=pdQbJrr4}{VALUE}% more likely to have children.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=29R5VkXa}{VALUE}% daily chance to get +1 relation with a random notable in the governed settlement while a continuous project is active.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmSelfPromoter.Initialize("{=hkG9ATZy}Self Promoter", DefaultSkills.Charm, this.GetTierCost(1), this._charmVirile, "{=qARDRFqO}{VALUE} renown when a tournament is won.", SkillEffect.PerkRole.Personal, 3f, SkillEffect.EffectIncrementType.Add, "{=PSvarWWW}{VALUE} morale while defending in a besieged settlement.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmOratory.Initialize("{=OZXEMb2C}Oratory", DefaultSkills.Charm, this.GetTierCost(2), this._charmWarlord, "{=qRoQuHe4}{VALUE} renown and influence for each issue resolved", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "{=YBmzuIbm}{VALUE} relationship with a random notable of your kingdom when an enemy lord is defeated.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmWarlord.Initialize("{=jiWr5Rlz}Warlord", DefaultSkills.Charm, this.GetTierCost(2), this._charmOratory, "{=IbQlvyY5}{VALUE}% influence gain from battles.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=231BaeH9}{VALUE} relationship with a random lord of your kingdom when an enemy lord is defeated.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmForgivableGrievances.Initialize("{=l863hIyN}Forgivable Grievances", DefaultSkills.Charm, this.GetTierCost(3), this._charmMeaningfulFavors, "{=BCB08mNZ}{VALUE}% chance of avoiding critical failure on persuasion.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=bFMDTiLE}{VALUE}% daily chance to increase relations with a random lord or notable with negative relations with you when you are in a settlement.", SkillEffect.PerkRole.Personal, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmMeaningfulFavors.Initialize("{=4hUEryJ6}Meaningful Favors", DefaultSkills.Charm, this.GetTierCost(3), this._charmForgivableGrievances, "{=T1N2w4uK}{VALUE}% chance for double persuasion success.", SkillEffect.PerkRole.Personal, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=6WP4OkKt}{VALUE}% daily chance to increase relations with powerful notables in the governed settlement.", SkillEffect.PerkRole.Governor, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmInBloom.Initialize("{=ZlXSlx0p}In Bloom", DefaultSkills.Charm, this.GetTierCost(4), this._charmYoungAndRespectful, "{=aVWb6aoQ}{VALUE}% relationship gain with the opposing gender.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=SimMOKbW}{VALUE}% daily chance to increase relations with a random notable of opposed sex in the governed settlement.", SkillEffect.PerkRole.Governor, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmYoungAndRespectful.Initialize("{=TpzZgFsA}Young And Respectful", DefaultSkills.Charm, this.GetTierCost(4), this._charmInBloom, "{=3MOJjS7A}{VALUE}% relationship gain with the same gender.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=7e397ieb}{VALUE}% daily chance to increase relations with a random notable of same sex in the governed settlement.", SkillEffect.PerkRole.Governor, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmFirebrand.Initialize("{=EbKP7Xx5}Firebrand", DefaultSkills.Charm, this.GetTierCost(5), this._charmFlexibleEthics, "{=vYj0z0zr}{VALUE}% influence cost to initiate kingdom decisions.", SkillEffect.PerkRole.ClanLeader, -0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=4ajo4jvp}{VALUE} recruitment slot from rural notables.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmFlexibleEthics.Initialize("{=58Imsasy}Flexible Ethics", DefaultSkills.Charm, this.GetTierCost(5), this._charmFirebrand, "{=HkOatwqw}{VALUE}% influence cost when voting for kingdom proposals made by others.", SkillEffect.PerkRole.Personal, -0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=FbAGhzbI}{VALUE} recruitment slot from urban notables.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmEffortForThePeople.Initialize("{=RIiVDdi0}Effort For The People", DefaultSkills.Charm, this.GetTierCost(6), this._charmSlickNegotiator, "{=P2eOw2sQ}{VALUE} relation with the nearest settlement owner clan when you clear a hideout. +1 town loyalty if it is your clan.", SkillEffect.PerkRole.Personal, 3f, SkillEffect.EffectIncrementType.Add, "{=FpleMw35}{VALUE}% barter penalty with lords of same culture.", SkillEffect.PerkRole.Personal, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmSlickNegotiator.Initialize("{=WOqxWM67}Slick Negotiator", DefaultSkills.Charm, this.GetTierCost(6), this._charmEffortForThePeople, "{=AqpEXxNy}{VALUE}% hiring costs of mercenary troops.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=6ex96ekx}{VALUE}% barter penalty with lords of different cultures.", SkillEffect.PerkRole.Personal, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmGoodNatured.Initialize("{=2y7gahYi}Good Natured", DefaultSkills.Charm, this.GetTierCost(7), this._charmTribute, "{=aitgGIog}{VALUE}% influence return when a supported proposal fails to pass.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.AddFactor, "{=fpaeONmG}{VALUE} extra relationship when you increase relationship with merciful lords.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmTribute.Initialize("{=dSBbSHkM}Tribute", DefaultSkills.Charm, this.GetTierCost(7), this._charmGoodNatured, "{=nJu03DL9}{VALUE}% relationship bonus when you pay more than minimum amount in barters.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=iqJQd4D8}{VALUE} extra relationship when you increase relationship with cruel lords.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmMoralLeader.Initialize("{=zUXUrGWa}Moral Leader", DefaultSkills.Charm, this.GetTierCost(8), this._charmNaturalLeader, "{=9mlBbzLx}{VALUE} persuasion success required against characters of your own culture.", SkillEffect.PerkRole.Personal, -1f, SkillEffect.EffectIncrementType.Add, "{=Cm0OcbsV}{VALUE} relation with settlement notables when a project is completed in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmNaturalLeader.Initialize("{=qaZDUknZ}Natural Leader", DefaultSkills.Charm, this.GetTierCost(8), this._charmMoralLeader, "{=dyVvsBMs}{VALUE} persuasion success required against characters of different cultures.", SkillEffect.PerkRole.Personal, -1f, SkillEffect.EffectIncrementType.Add, "{=30eSZeZd}{VALUE}% experience gain for companions.", SkillEffect.PerkRole.ClanLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmPublicSpeaker.Initialize("{=16fxd9fN}Public Speaker", DefaultSkills.Charm, this.GetTierCost(9), this._charmParade, "{=z4naITkR}{VALUE}% renown gain from battles.", SkillEffect.PerkRole.PartyLeader, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=J7JaXZm8}{VALUE}% effect from forums, marketplaces and festivals.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmParade.Initialize("{=DTnaWgAv}Parade", DefaultSkills.Charm, this.GetTierCost(9), this._charmPublicSpeaker, "{=yA2P7w9N}{VALUE} loyalty bonus to settlement while waiting in the settlement.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "{=rHtwp8ag}{VALUE}% daily chance to gain +1 relationship with a random lord in the same army.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmCamaraderie.Initialize("{=p2zZGkZw}Camaraderie", DefaultSkills.Charm, this.GetTierCost(10), null, "{=l2ZKUJQY}Double the relation gain for helping lords in battle.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.AddFactor, "{=XmwIHIMN}{VALUE} companion limit", SkillEffect.PerkRole.ClanLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._charmImmortalCharm.Initialize("{=9XWiXokY}Immortal Charm", DefaultSkills.Charm, this.GetTierCost(11), null, "{=BjLYCHMD}{VALUE} influence per day.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipCombatTips.Initialize("{=Cb5s9HlD}Combat Tips", DefaultSkills.Leadership, this.GetTierCost(1), this._leadershipRaiseTheMeek, "{=76TOkicW}{VALUE} experience per day to all troops in party.", SkillEffect.PerkRole.PartyLeader, 2f, SkillEffect.EffectIncrementType.Add, "{=z3OU7vrn}{VALUE} to troop tiers when recruiting from same culture.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipRaiseTheMeek.Initialize("{=JGCtv8om}Raise The Meek", DefaultSkills.Leadership, this.GetTierCost(1), this._leadershipCombatTips, "{=Ra2poaEh}{VALUE} experience per day to tier 1 and 2 troops.", SkillEffect.PerkRole.PartyLeader, 4f, SkillEffect.EffectIncrementType.Add, "{=CjLuIEgh}{VALUE} experience per day to each troop in garrison in the governed settlement.", SkillEffect.PerkRole.Governor, 3f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipFerventAttacker.Initialize("{=MhRF64eR}Fervent Attacker", DefaultSkills.Leadership, this.GetTierCost(2), this._leadershipStoutDefender, "{=o7xn0ybm}{VALUE} starting battle morale when attacking.", SkillEffect.PerkRole.PartyLeader, 4f, SkillEffect.EffectIncrementType.Add, "{=AbulTQm9}{VALUE}% recruitment rate of tier 1, 2 and 3 prisoners.", SkillEffect.PerkRole.PartyLeader, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipStoutDefender.Initialize("{=YogcurDJ}Stout Defender", DefaultSkills.Leadership, this.GetTierCost(2), this._leadershipFerventAttacker, "{=RIMXqF1d}{VALUE} battle morale at the beginning of a battle when defending.", SkillEffect.PerkRole.PartyLeader, 8f, SkillEffect.EffectIncrementType.Add, "{=qItLTWR2}{VALUE}% recruitment rate of tier 4+ prisoners.", SkillEffect.PerkRole.PartyLeader, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipAuthority.Initialize("{=CeCAMvkX}Authority", DefaultSkills.Leadership, this.GetTierCost(3), this._leadershipHeroicLeader, "{=bezXAM92}{VALUE}% security bonus from the town garrison in the governing settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=rLs30aPf}{VALUE} party size limit.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipHeroicLeader.Initialize("{=7aX2eh5x}Heroic Leader", DefaultSkills.Leadership, this.GetTierCost(3), this._leadershipAuthority, "{=m993aVvX}{VALUE} daily loyalty in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, "{=yvyVugUN}{VALUE}% battle morale penalty to enemies when troops in your formation kill an enemy.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._leadershipLoyaltyAndHonor.Initialize("{=UJYaonYM}Loyalty and Honor", DefaultSkills.Leadership, this.GetTierCost(4), this._leadershipFamousCommander, "{=wBURlfzR}Tier 3+ troops in your party no longer retreat due to low morale", SkillEffect.PerkRole.PartyLeader, 3f, SkillEffect.EffectIncrementType.Add, "{=kuu7M6aQ}{VALUE}% faster non-bandit prisoner recruitment.", SkillEffect.PerkRole.PartyLeader, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipFamousCommander.Initialize("{=FQkHkMhw}Famous Commander", DefaultSkills.Leadership, this.GetTierCost(4), this._leadershipLoyaltyAndHonor, "{=z4naITkR}{VALUE}% renown gain from battles.", SkillEffect.PerkRole.Personal, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=CkJarFvq}{VALUE} experience to troops on recruitment.", SkillEffect.PerkRole.Personal, 200f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipPresence.Initialize("{=6RckjM4S}Presence", DefaultSkills.Leadership, this.GetTierCost(5), this._leadershipLeaderOfTheMasses, "{=UgRGBWhn}{VALUE} security per day while waiting in a town.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "{=9JN5bc7f}No morale penalty for recruiting prisoners of your faction.", SkillEffect.PerkRole.PartyLeader, 0f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipLeaderOfTheMasses.Initialize("{=T5rM9XgO}Leader of the Masses", DefaultSkills.Leadership, this.GetTierCost(5), this._leadershipPresence, "{=VUma8oHz}{VALUE} party size for each town you control.", SkillEffect.PerkRole.ClanLeader, 5f, SkillEffect.EffectIncrementType.Add, "{=ptmYmT6B}{VALUE}% experience from battles shared with the troops in your party.", SkillEffect.PerkRole.PartyLeader, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipVeteransRespect.Initialize("{=vWGQNcu5}Veteran's Respect", DefaultSkills.Leadership, this.GetTierCost(6), this._leadershipCitizenMilitia, "{=wSLO8VgG}{VALUE} garrison size in the governed settlement.", SkillEffect.PerkRole.Governor, 20f, SkillEffect.EffectIncrementType.Add, "{=lsnrQCB8}Bandits can be converted into regular troops.", SkillEffect.PerkRole.PartyLeader, 0f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipCitizenMilitia.Initialize("{=vZtLm43v}Citizen Militia", DefaultSkills.Leadership, this.GetTierCost(6), this._leadershipVeteransRespect, "{=kVd6nlXo}{VALUE}% chance of militias to spawn as elite troops in the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=QPfj9Dbj}{VALUE}% morale from victories.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipInspiringLeader.Initialize("{=kaEzJUTW}Inspiring Leader", DefaultSkills.Leadership, this.GetTierCost(7), this._leadershipUpliftingSpirit, "{=M04V0cmt}{VALUE}% influence cost for calling parties to an army.", SkillEffect.PerkRole.ArmyCommander, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=je77ZaN7}{VALUE}% experience to troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._leadershipUpliftingSpirit.Initialize("{=EbROfVJJ}Uplifting Spirit", DefaultSkills.Leadership, this.GetTierCost(7), this._leadershipInspiringLeader, "{=FZ06ALO6}{VALUE} battle morale in siege battles.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, "{=rLs30aPf}{VALUE} party size limit.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipTrustedCommander.Initialize("{=6ETg3maz}Trusted Commander", DefaultSkills.Leadership, this.GetTierCost(8), this._leadershipLeadByExample, "{=dAS81esi}{VALUE}% recruitment rate for ranged prisoners.", SkillEffect.PerkRole.PartyLeader, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=lutYwHwt}{VALUE}% experience for troops, when they are sent to confront the enemy.", SkillEffect.PerkRole.PartyLeader, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipLeadByExample.Initialize("{=WFFlp3Qi}Lead by Example", DefaultSkills.Leadership, this.GetTierCost(8), this._leadershipTrustedCommander, "{=tEsgNQOZ}{VALUE}% recruitment rate for infantry prisoners.", SkillEffect.PerkRole.PartyLeader, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=aeceOwWb}{VALUE}% shared experience for cavalry troops.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipMakeADifference.Initialize("{=5uW9zKTN}Make a Difference", DefaultSkills.Leadership, this.GetTierCost(9), this._leadershipGreatLeader, "{=YaPOTaMJ}{VALUE}% battle morale to troops when you kill an enemy in battle.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.AddFactor, "{=MMMuSlOW}{VALUE}% shared experience for archers.", SkillEffect.PerkRole.PartyLeader, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipGreatLeader.Initialize("{=3hzSmrMw}Great Leader", DefaultSkills.Leadership, this.GetTierCost(9), this._leadershipMakeADifference, "{=p8pviWlQ}{VALUE} battle morale to troops at the beginning of a battle.", SkillEffect.PerkRole.ArmyCommander, 5f, SkillEffect.EffectIncrementType.Add, "{=LGH67bOj}{VALUE} battle morale to troops that are of same culture as you.", SkillEffect.PerkRole.PartyLeader, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipWePledgeOurSwords.Initialize("{=3GHIb7YX}We Pledge our Swords", DefaultSkills.Leadership, this.GetTierCost(10), this._leadershipTalentMagnet, "{=0AUYrhGw}{VALUE} companion limit.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "{=FkkVHjBP}{VALUE} battle morale at the beginning of the battle for each tier 6 troop in the party up to 10 morale.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipTalentMagnet.Initialize("{=pFfqWRnf}Talent Magnet", DefaultSkills.Leadership, this.GetTierCost(10), this._leadershipWePledgeOurSwords, "{=rLs30aPf}{VALUE} party size limit.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, "{=gqboke7l}{VALUE} clan party limit.", SkillEffect.PerkRole.ClanLeader, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._leadershipUltimateLeader.Initialize("{=FK3W0SKk}Ultimate Leader", DefaultSkills.Leadership, this.GetTierCost(11), null, "{=Q72PJYtf}{VALUE} party size for each leadership point above 250.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeAppraiser.Initialize("{=b3PsxeiB}Appraiser", DefaultSkills.Trade, this.GetTierCost(1), this._tradeWholeSeller, "{=wki8aFec}{VALUE}% price penalty while selling equipment.", SkillEffect.PerkRole.PartyLeader, -0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=gHUQfWlg}Your profits are marked.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeWholeSeller.Initialize("{=lTNpxGoh}Whole Seller", DefaultSkills.Trade, this.GetTierCost(1), this._tradeAppraiser, "{=9Y4rMcYj}{VALUE}% price penalty while selling trade goods.", SkillEffect.PerkRole.PartyLeader, -0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=gHUQfWlg}Your profits are marked.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeCaravanMaster.Initialize("{=5acLha5Q}Caravan Master", DefaultSkills.Trade, this.GetTierCost(2), this._tradeMarketDealer, "{=SPs04fam}{VALUE}% carrying capacity for your party.", SkillEffect.PerkRole.Quartermaster, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=QUYwIYEi}Item prices are marked relative to the average price.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeMarketDealer.Initialize("{=InLGoUbB}Market Dealer", DefaultSkills.Trade, this.GetTierCost(2), this._tradeCaravanMaster, "{=Si3QiLW4}{VALUE}% cost of bartering for safe passage.", SkillEffect.PerkRole.ClanLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=QUYwIYEi}Item prices are marked relative to the average price.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeDistributedGoods.Initialize("{=nxkNY4YG}Distributed Goods", DefaultSkills.Trade, this.GetTierCost(3), this._tradeLocalConnection, "{=we6jYdRD}Double the relationship gain by resolved issues with artisans.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.AddFactor, "{=RYkPTHv1}{VALUE}% price penalty while buying from villages.", SkillEffect.PerkRole.Quartermaster, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeLocalConnection.Initialize("{=mznjEwjC}Local Connection", DefaultSkills.Trade, this.GetTierCost(3), this._tradeDistributedGoods, "{=ORencCvQ}Double the relationship gain by resolved issues with merchants.", SkillEffect.PerkRole.Personal, 2f, SkillEffect.EffectIncrementType.Add, "{=AAYplFKi}{VALUE}% price penalty while selling animals.", SkillEffect.PerkRole.Quartermaster, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeTravelingRumors.Initialize("{=3j6Ec63l}Traveling Rumors", DefaultSkills.Trade, this.GetTierCost(4), this._tradeTollgates, "{=DV2kW53e}Your caravans gather trade rumors.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "{=D2nbscmg}{VALUE} gold for each villager party visiting the governed settlement.", SkillEffect.PerkRole.Governor, 20f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeTollgates.Initialize("{=JnSh4Fmz}Toll Gates", DefaultSkills.Trade, this.GetTierCost(4), this._tradeTravelingRumors, "{=SOHgkGKy}Your workshops gather trade rumors.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "{=bteVVFh0}{VALUE} gold for each caravan visiting the governed settlement.", SkillEffect.PerkRole.Governor, 30f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeArtisanCommunity.Initialize("{=8f8UGq46}Artisan Community", DefaultSkills.Trade, this.GetTierCost(5), this._tradeGreatInvestor, "{=CBSDuOmp}{VALUE} daily renown from every profiting workshop.", SkillEffect.PerkRole.ClanLeader, 1f, SkillEffect.EffectIncrementType.Add, "{=amA9OfPU}{VALUE} recruitment slot when recruiting from merchant notables. ", SkillEffect.PerkRole.Quartermaster, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeGreatInvestor.Initialize("{=g9qLrEb4}Great Investor", DefaultSkills.Trade, this.GetTierCost(5), this._tradeArtisanCommunity, "{=aYpbyTfA}{VALUE} daily renown from every profiting caravan.", SkillEffect.PerkRole.ClanLeader, 1f, SkillEffect.EffectIncrementType.Add, "{=m41r7FPw}{VALUE}% companion recruitment cost.", SkillEffect.PerkRole.Quartermaster, -0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeMercenaryConnections.Initialize("{=vivNLdHp}Mercenary Connections", DefaultSkills.Trade, this.GetTierCost(6), this._tradeContentTrades, "{=HrTFr1ox}{VALUE}% workshop production rate.", SkillEffect.PerkRole.Governor, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=GNtTFR0j}{VALUE}% mercenary troop wages in your party.", SkillEffect.PerkRole.PartyLeader, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeContentTrades.Initialize("{=FV4SWLQx}Content Trades", DefaultSkills.Trade, this.GetTierCost(6), this._tradeMercenaryConnections, "{=Eo958e7R}{VALUE}% tariff income in the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=Oq1K7oDW}{VALUE}% wages paid while waiting in settlements.", SkillEffect.PerkRole.PartyLeader, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeInsurancePlans.Initialize("{=aYQybo4E}Insurance Plans", DefaultSkills.Trade, this.GetTierCost(7), this._tradeRapidDevelopment, "{=NMnpGic4}{VALUE} denar return when one of your caravans is destroyed.", SkillEffect.PerkRole.ClanLeader, 5000f, SkillEffect.EffectIncrementType.Add, "{=xe0dX5QQ}{VALUE}% price penalty while buying food items.", SkillEffect.PerkRole.Quartermaster, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeRapidDevelopment.Initialize("{=u9oONz9o}Rapid Development", DefaultSkills.Trade, this.GetTierCost(7), this._tradeInsurancePlans, "{=EdCkK2c4}{VALUE} denar return for each workshop when workshop's town is captured by an enemy.", SkillEffect.PerkRole.ClanLeader, 5000f, SkillEffect.EffectIncrementType.Add, "{=4ORpHfu2}{VALUE}% price penalty while buying clay, iron, cotton and silver.", SkillEffect.PerkRole.Quartermaster, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeGranaryAccountant.Initialize("{=TFy2VYtM}Granary Accountant", DefaultSkills.Trade, this.GetTierCost(8), this._tradeTradeyardForeman, "{=SyxQF0tM}{VALUE}% price penalty while selling food items.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=JnQcDyAz}{VALUE}% production rate to grain, olives, fish, date in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeTradeyardForeman.Initialize("{=QqKNxmeF}Tradeyard Foreman", DefaultSkills.Trade, this.GetTierCost(8), this._tradeGranaryAccountant, "{=KgrnmR73}{VALUE}% price penalty while selling pottery, tools, cotton and jewelry.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=mN3fLgtx}{VALUE}% production rate to clay, iron, cotton and silver in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeSwordForBarter.Initialize("{=AIsDxCeG}Sword For Barter", DefaultSkills.Trade, this.GetTierCost(9), this._tradeSelfMadeMan, "{=AqpEXxNy}{VALUE}% hiring costs of mercenary troops.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=68ye0NpS}{VALUE}% caravan guard wages.", SkillEffect.PerkRole.Quartermaster, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeSelfMadeMan.Initialize("{=uHJltZ5D}Self-made Man", DefaultSkills.Trade, this.GetTierCost(9), this._tradeSwordForBarter, "{=rTbVn6sJ}{VALUE}% barter penalty for items.", SkillEffect.PerkRole.Personal, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=Q9VCvUTg}{VALUE}% build speed for marketplace, kiln and aqueduct projects.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeSilverTongue.Initialize("{=5rDdJpJo}Silver Tongue", DefaultSkills.Trade, this.GetTierCost(10), this._tradeSpringOfGold, "{=UzKyyfbF}{VALUE}% gold required while persuading lords to defect to your faction.", SkillEffect.PerkRole.Personal, -0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=Kb9uC4gQ}{VALUE}% better trade deals from caravans and villagers", SkillEffect.PerkRole.Quartermaster, 0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeSpringOfGold.Initialize("{=K0SRwH6E}Spring of Gold", DefaultSkills.Trade, this.GetTierCost(10), this._tradeSilverTongue, "{=gu7EN92A}{VALUE}% denars of interest income per day based on your current denars up to 1000 denars.", SkillEffect.PerkRole.ClanLeader, 0.001f, SkillEffect.EffectIncrementType.AddFactor, "{=XmqJb7RN}{VALUE}% effect from boosting projects in the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeManOfMeans.Initialize("{=Jy2ap8L1}Man of Means", DefaultSkills.Trade, this.GetTierCost(11), this._tradeTrickleDown, "{=7QadTbWs}{VALUE}% costs of recruiting minor faction clans into your clan.", SkillEffect.PerkRole.ClanLeader, -0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=lA0eEkGP}{VALUE}% ransom cost for your freedom.", SkillEffect.PerkRole.Personal, -0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeTrickleDown.Initialize("{=L4fz3Jdr}Trickle Down", DefaultSkills.Trade, this.GetTierCost(11), this._tradeManOfMeans, "{=ANhbaAhL}{VALUE} relationship with merchants if 10.000 or more denars are spent on a single deal.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, "{=REZyGJGH}{VALUE} daily prosperity while building a project in the governed settlement.", SkillEffect.PerkRole.Governor, 2f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._tradeEverythingHasAPrice.Initialize("{=cRwNeSzb}Everything Has a Price", DefaultSkills.Trade, this.GetTierCost(12), null, "{=HeefccTC}You can now trade settlements in barter.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Invalid, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardWarriorsDiet.Initialize("{=mIDsxe1O}Warrior's Diet", DefaultSkills.Steward, this.GetTierCost(1), this._stewardFrugal, "{=6NHvsrrx}{VALUE}% food consumption in your party.", SkillEffect.PerkRole.Quartermaster, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=mSvfxXVW}No morale penalty from having single type of food.", SkillEffect.PerkRole.PartyLeader, 0f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardFrugal.Initialize("{=eJIbMa8P}Frugal", DefaultSkills.Steward, this.GetTierCost(1), this._stewardWarriorsDiet, "{=CJB5HCsI}{VALUE}% wages in your party.", SkillEffect.PerkRole.Quartermaster, -0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=OTyYJ2Bt}{VALUE}% recruitment costs.", SkillEffect.PerkRole.PartyLeader, -0.15f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardSevenVeterans.Initialize("{=2ryLuN2i}Seven Veterans", DefaultSkills.Steward, this.GetTierCost(2), this._stewardDrillSergant, "{=gX0edfpK}{VALUE} daily experience for tier 4+ troops in your party.", SkillEffect.PerkRole.Quartermaster, 4f, SkillEffect.EffectIncrementType.Add, "{=g9gTYB8u}{VALUE} militia recruitment in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardDrillSergant.Initialize("{=L9k4bovO}Drill Sergeant", DefaultSkills.Steward, this.GetTierCost(2), this._stewardSevenVeterans, "{=UYhJZya5}{VALUE} daily experience to troops in your party.", SkillEffect.PerkRole.Quartermaster, 2f, SkillEffect.EffectIncrementType.Add, "{=B2msxAju}{VALUE}% garrison wages in the governed settlement.", SkillEffect.PerkRole.Governor, -0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardSweatshops.Initialize("{=jbAtOsIy}Sweatshops", DefaultSkills.Steward, this.GetTierCost(3), this._stewardStiffUpperLip, "{=6wqJA77K}{VALUE}% production rate to owned workshops.", SkillEffect.PerkRole.Personal, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=rA9nzrAr}{VALUE}% siege engine build rate in your party.", SkillEffect.PerkRole.Quartermaster, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardStiffUpperLip.Initialize("{=QUeJ4gc3}Stiff Upper Lip", DefaultSkills.Steward, this.GetTierCost(3), this._stewardSweatshops, "{=y9AsEMnV}{VALUE}% food consumption in your party while it is part of an army.", SkillEffect.PerkRole.Quartermaster, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=1FPpHasQ}{VALUE}% garrison wages in the governed castle.", SkillEffect.PerkRole.Governor, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardPaidInPromise.Initialize("{=CPxbG7Zp}Paid in Promise", DefaultSkills.Steward, this.GetTierCost(4), this._stewardEfficientCampaigner, "{=H9tQfeBr}{VALUE}% companion wages and recruitment fees.", SkillEffect.PerkRole.PartyLeader, -0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=1eKRHLur}Discarded armors are donated to troops for increased experience.", SkillEffect.PerkRole.Quartermaster, 0f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardEfficientCampaigner.Initialize("{=sC53NYcA}Efficient Campaigner", DefaultSkills.Steward, this.GetTierCost(4), this._stewardPaidInPromise, "{=5t6cveXT}{VALUE} extra food for each food taken during village raids for your party.", SkillEffect.PerkRole.PartyLeader, 1f, SkillEffect.EffectIncrementType.Add, "{=JhFCoWbE}{VALUE}% troop wages in your party while it is part of an army.", SkillEffect.PerkRole.Quartermaster, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardGivingHands.Initialize("{=VsqyzWYY}Giving Hands", DefaultSkills.Steward, this.GetTierCost(5), this._stewardLogistician, "{=WaGKvsfc}Discarded weapons are donated to troops for increased experience.", SkillEffect.PerkRole.Quartermaster, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=Eo958e7R}{VALUE}% tariff income in the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardLogistician.Initialize("{=U2buPiec}Logistician", DefaultSkills.Steward, this.GetTierCost(5), this._stewardGivingHands, "{=sG9WGOeN}{VALUE} party morale when number of mounts is greater than number of foot troops in your party.", SkillEffect.PerkRole.Quartermaster, 4f, SkillEffect.EffectIncrementType.Add, "{=Z1n0w5Kc}{VALUE}% tax income.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardRelocation.Initialize("{=R6dnhblo}Relocation", DefaultSkills.Steward, this.GetTierCost(6), this._stewardAidCorps, "{=urSSNtUD}{VALUE}% influence gain from donating troops.", SkillEffect.PerkRole.Quartermaster, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=XmqJb7RN}{VALUE}% effect from boosting projects in the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardAidCorps.Initialize("{=4FdtVyj1}Aid Corps", DefaultSkills.Steward, this.GetTierCost(6), this._stewardRelocation, "{=ZLbCqt23}Wounded troops in your party are no longer paid wages.", SkillEffect.PerkRole.Quartermaster, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=ULY7byYc}{VALUE}% hearth growth in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardGourmet.Initialize("{=63lHFDSG}Gourmet", DefaultSkills.Steward, this.GetTierCost(7), this._stewardSoundReserves, "{=KDtcsKUs}Double the morale bonus from having diverse food in your party.", SkillEffect.PerkRole.Quartermaster, 2f, SkillEffect.EffectIncrementType.AddFactor, "{=q2ZDAm2v}{VALUE}% garrison food consumption during sieges in the governed settlement.", SkillEffect.PerkRole.Governor, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardSoundReserves.Initialize("{=O5dgeoss}Sound Reserves", DefaultSkills.Steward, this.GetTierCost(7), this._stewardGourmet, "{=RkYL5eaP}{VALUE}% troop upgrade costs.", SkillEffect.PerkRole.Quartermaster, -0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=P10E5o9l}{VALUE}% food consumption during sieges in your party.", SkillEffect.PerkRole.Quartermaster, -0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardForcedLabor.Initialize("{=cWyqiNrf}Forced Labor", DefaultSkills.Steward, this.GetTierCost(8), this._stewardContractors, "{=HrOTTjgo}Prisoners in your party provide carry capacity as if they are standard troops.", SkillEffect.PerkRole.Quartermaster, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=T9Viygs8}{VALUE}% construction speed per every 3 prisoners.", SkillEffect.PerkRole.Governor, 0.01f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardContractors.Initialize("{=Pg5enC8c}Contractors", DefaultSkills.Steward, this.GetTierCost(8), this._stewardForcedLabor, "{=4220dQ4j}{VALUE}% wages and upgrade costs of the mercenary troops in your party.", SkillEffect.PerkRole.Quartermaster, -0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=xiTD2qUv}{VALUE}% town project effects in the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardArenicosMules.Initialize("{=qBx8UbUt}Arenicos' Mules", DefaultSkills.Steward, this.GetTierCost(9), this._stewardArenicosHorses, "{=Yp4zv2ib}{VALUE}% carrying capacity for pack animals in your party.", SkillEffect.PerkRole.Quartermaster, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=fswrp38u}{VALUE}% trade penalty for trading pack animals.", SkillEffect.PerkRole.Quartermaster, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardArenicosHorses.Initialize("{=tbQ5bUzD}Arenicos' Horses", DefaultSkills.Steward, this.GetTierCost(9), this._stewardArenicosMules, "{=G9OTNRs4}{VALUE}% carrying capacity for troops in your party.", SkillEffect.PerkRole.Quartermaster, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=xm4eEbQY}{VALUE}% trade penalty for trading mounts.", SkillEffect.PerkRole.Personal, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardMasterOfPlanning.Initialize("{=n5aT1Y7s}Master of Planning", DefaultSkills.Steward, this.GetTierCost(10), this._stewardMasterOfWarcraft, "{=KMmAG5bk}{VALUE}% food consumption while your party is in a siege camp.", SkillEffect.PerkRole.Quartermaster, -0.4f, SkillEffect.EffectIncrementType.AddFactor, "{=P5OjioRl}{VALUE}% effectiveness to continuous projects in the governed settlement. ", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardMasterOfWarcraft.Initialize("{=MM0ARhGh}Master of Warcraft", DefaultSkills.Steward, this.GetTierCost(10), this._stewardMasterOfPlanning, "{=StzVsQ2P}{VALUE}% troop wages while your party is in a siege camp.", SkillEffect.PerkRole.Quartermaster, -0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=ya7alenH}{VALUE}% food consumption of town population in the governed settlement.", SkillEffect.PerkRole.Governor, -0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._stewardPriceOfLoyalty.Initialize("{=eVTnUmSB}Price of Loyalty", DefaultSkills.Steward, this.GetTierCost(11), null, "{=sYrG8rNy}{VALUE}% to food consumption, wages and combat related morale loss for each steward point above 250 in your party.", SkillEffect.PerkRole.Quartermaster, -0.005f, SkillEffect.EffectIncrementType.AddFactor, "{=lwp50FuF}{VALUE}% tax income for each skill point above 200 in the governed settlement", SkillEffect.PerkRole.Governor, 0.005f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineSelfMedication.Initialize("{=TLGvIdJB}Self Medication", DefaultSkills.Medicine, this.GetTierCost(1), this._medicinePreventiveMedicine, "{=bLAw2di4}{VALUE}% healing rate.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=V53EYEXx}{VALUE}% combat movement speed.", SkillEffect.PerkRole.Personal, 0.02f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicinePreventiveMedicine.Initialize("{=wI393cla}Preventive Medicine", DefaultSkills.Medicine, this.GetTierCost(1), this._medicineSelfMedication, "{=Ti9auMiO}{VALUE} hit points.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, "{=10cVZTTm}{VALUE}% recovery of lost hit points after each battle.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineTriageTent.Initialize("{=EU4JjLqV}Triage Tent", DefaultSkills.Medicine, this.GetTierCost(2), this._medicineWalkItOff, "{=ZMPhsLdx}{VALUE}% healing rate when stationary on the campaign map.", SkillEffect.PerkRole.Surgeon, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=Mn714dPH}{VALUE}% food consumption for besieged governed settlement.", SkillEffect.PerkRole.Governor, -0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineWalkItOff.Initialize("{=0pyLfrGZ}Walk It Off", DefaultSkills.Medicine, this.GetTierCost(2), this._medicineTriageTent, "{=NtCBRiLH}{VALUE}% healing rate when moving on the campaign map.", SkillEffect.PerkRole.Surgeon, 0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=4YNqWPEu}{VALUE} hit points recovery after each offensive battle.", SkillEffect.PerkRole.Personal, 10f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineSledges.Initialize("{=TyB6y5bh}Sledges", DefaultSkills.Medicine, this.GetTierCost(3), this._medicineDoctorsOath, "{=bFOfZmwC}{VALUE}% party speed penalty from the wounded.", SkillEffect.PerkRole.Surgeon, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=dfULyKsz}{VALUE} hit points to mounts in your party.", SkillEffect.PerkRole.PartyLeader, 15f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineDoctorsOath.Initialize("{=PAwDV08b}Doctor's Oath", DefaultSkills.Medicine, this.GetTierCost(3), this._medicineSledges, "{=XPB1iBkh}Your medicine skill also applies to enemy casualties, increasing potential prisoners.", SkillEffect.PerkRole.Surgeon, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=Ti9auMiO}{VALUE} hit points.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineBestMedicine.Initialize("{=ei1JSeco}Best Medicine", DefaultSkills.Medicine, this.GetTierCost(4), this._medicineGoodLodging, "{=L3kTYA2p}{VALUE}% healing rate while party morale is above 70.", SkillEffect.PerkRole.Surgeon, 0.15f, SkillEffect.EffectIncrementType.AddFactor, "{=At6b9vHF}{VALUE} relationship per day with a random notable over age 40 when party is in a town.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineGoodLodging.Initialize("{=RXo3edjn}Good Lodging", DefaultSkills.Medicine, this.GetTierCost(4), this._medicineBestMedicine, "{=NjMR2ypH}{VALUE}% healing rate while resting in settlements.", SkillEffect.PerkRole.Surgeon, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=ZH3U43xW}{VALUE} relationship per day with a random noble over age 40 when party is in a town.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineSiegeMedic.Initialize("{=ObwbbEqE}Siege Medic", DefaultSkills.Medicine, this.GetTierCost(5), this._medicineVeterinarian, "{=Gyy4rwnD}{VALUE}% chance of troops getting wounded instead of getting killed during siege bombardment.", SkillEffect.PerkRole.Surgeon, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=Nxh6aX2E}{VALUE}% chance to recover from lethal wounds during siege bombardment.", SkillEffect.PerkRole.Surgeon, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineVeterinarian.Initialize("{=DNPbZZPQ}Veterinarian", DefaultSkills.Medicine, this.GetTierCost(5), this._medicineSiegeMedic, "{=PZb8JrMH}{VALUE}% daily chance to recover a lame horse.", SkillEffect.PerkRole.Surgeon, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=GJRcFc0V}{VALUE}% chance to recover mounts of dead cavalry troops in battles.", SkillEffect.PerkRole.Surgeon, 0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicinePristineStreets.Initialize("{=72tbUfrz}Pristine Streets", DefaultSkills.Medicine, this.GetTierCost(6), this._medicineBushDoctor, "{=JMMVcpA0}{VALUE} settlement prosperity every day in governed settlements.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, "{=R9O0Y64L}{VALUE}% party healing rate while waiting in towns.", SkillEffect.PerkRole.Surgeon, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineBushDoctor.Initialize("{=HGrsb7k2}Bush Doctor", DefaultSkills.Medicine, this.GetTierCost(6), this._medicinePristineStreets, "{=ULY7byYc}{VALUE}% hearth growth in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=UaKTuz1l}{VALUE}% party healing rate while waiting in villages.", SkillEffect.PerkRole.Surgeon, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicinePerfectHealth.Initialize("{=cGuPMx4p}Perfect Health", DefaultSkills.Medicine, this.GetTierCost(7), this._medicineHealthAdvise, "{=1yqMERf2}{VALUE}% recovery rate for each type of food in party inventory.", SkillEffect.PerkRole.Surgeon, 0.05f, SkillEffect.EffectIncrementType.AddFactor, "{=QsMEML5E}{VALUE}% animal production rate in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineHealthAdvise.Initialize("{=NxcvQlAk}Health Advice", DefaultSkills.Medicine, this.GetTierCost(7), this._medicinePerfectHealth, "{=uRvym4tq}Chance of recovery from death due to old age for every clan member.", SkillEffect.PerkRole.ClanLeader, 0f, SkillEffect.EffectIncrementType.AddFactor, "{=ioYR1Grc}Wounded troops do not decrease morale in battles.", SkillEffect.PerkRole.Surgeon, 0f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicinePhysicianOfPeople.Initialize("{=5o6pSbCx}Physician of People", DefaultSkills.Medicine, this.GetTierCost(8), this._medicineCleanInfrastructure, "{=F7bbkYx4}{VALUE} loyalty per day in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, "{=bNsaUb42}{VALUE}% chance to recover from lethal wounds for tier 1 and 2 troops", SkillEffect.PerkRole.Surgeon, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineCleanInfrastructure.Initialize("{=CZ4y5NAf}Clean Infrastructure", DefaultSkills.Medicine, this.GetTierCost(8), this._medicinePhysicianOfPeople, "{=S9XsuYap}{VALUE} prosperity bonus from civilian projects in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, "{=dYyFWmGB}{VALUE}% recovery rate from raids in villages bound to the governed settlement.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineCheatDeath.Initialize("{=cpg0oHZJ}Cheat Death", DefaultSkills.Medicine, this.GetTierCost(9), this._medicineFortitudeTonic, "{=n2xL3okw}Cheat death due to old age once.", SkillEffect.PerkRole.Personal, 0f, SkillEffect.EffectIncrementType.Add, "{=b1IKTI8t}{VALUE}% chance to die when you fall unconscious in battle.", SkillEffect.PerkRole.Surgeon, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineFortitudeTonic.Initialize("{=ib2SMG9b}Fortitude Tonic", DefaultSkills.Medicine, this.GetTierCost(9), this._medicineCheatDeath, "{=v9NohO6l}{VALUE} hit points to other heroes in your party.", SkillEffect.PerkRole.PartyLeader, 10f, SkillEffect.EffectIncrementType.Add, "{=Ti9auMiO}{VALUE} hit points.", SkillEffect.PerkRole.Personal, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineHelpingHands.Initialize("{=KavZKNaa}Helping Hands", DefaultSkills.Medicine, this.GetTierCost(10), this._medicineBattleHardened, "{=6NOzUcGN}{VALUE}% troop recovery rate for every 10 troop in your party.", SkillEffect.PerkRole.Surgeon, 0.02f, SkillEffect.EffectIncrementType.AddFactor, "{=iHuzmdm2}{VALUE}% prosperity loss from starvation.", SkillEffect.PerkRole.Governor, -0.5f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineBattleHardened.Initialize("{=oSbRD72H}Battle Hardened", DefaultSkills.Medicine, this.GetTierCost(10), this._medicineHelpingHands, "{=qWpabhp6}{VALUE} experience to wounded units at the end of the battle.", SkillEffect.PerkRole.Surgeon, 25f, SkillEffect.EffectIncrementType.Add, "{=3tLU4AG7}{VALUE}% siege attrition loss in the governed settlement.", SkillEffect.PerkRole.Governor, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._medicineMinisterOfHealth.Initialize("{=rtTjuJTc}Minister of Health", DefaultSkills.Medicine, this.GetTierCost(11), null, "{=cwFyqrfv}{VALUE} hit point to troops for every skill point above 250.", SkillEffect.PerkRole.Personal, 1f, SkillEffect.EffectIncrementType.Add, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringScaffolds.Initialize("{=ekavTnTp}Scaffolds", DefaultSkills.Engineering, this.GetTierCost(1), this._engineeringTorsionEngines, "{=2WC42D5D}{VALUE}% build speed to non-ranged siege engines.", SkillEffect.PerkRole.Engineer, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=F1CJo2wX}{VALUE}% shield hitpoints.", SkillEffect.PerkRole.Personal, 0.3f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringTorsionEngines.Initialize("{=57TDG2Ta}Torsion Engines", DefaultSkills.Engineering, this.GetTierCost(1), this._engineeringScaffolds, "{=hv18SprX}{VALUE}% build speed to ranged siege engines.", SkillEffect.PerkRole.Engineer, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=aA8T7AsY}{VALUE} damage to equipped crossbows.", SkillEffect.PerkRole.Personal, 3f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringSiegeWorks.Initialize("{=Nr1GPYSr}Siegeworks", DefaultSkills.Engineering, this.GetTierCost(2), this._engineeringDungeonArchitect, "{=oOZH3v9Y}{VALUE}% hit points to ranged siege engines.", SkillEffect.PerkRole.Engineer, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=pIFOcikU}{VALUE} prebuilt catapult to the settlement when a siege starts in the governed settlement.", SkillEffect.PerkRole.Governor, 1f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringDungeonArchitect.Initialize("{=aPbpBJq5}Dungeon Architect", DefaultSkills.Engineering, this.GetTierCost(2), this._engineeringSiegeWorks, "{=KK3DAGej}{VALUE}% chance of ranged siege engines getting hit while under bombardment.", SkillEffect.PerkRole.Engineer, -0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=ako4Xbvk}{VALUE}% escape chance to prisoners in dungeons of governed settlements.", SkillEffect.PerkRole.Governor, -0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringCarpenters.Initialize("{=YwhAlz5n}Carpenters", DefaultSkills.Engineering, this.GetTierCost(3), this._engineeringMilitaryPlanner, "{=cXCbpPqS}{VALUE}% hit points to rams and siege-towers.", SkillEffect.PerkRole.Engineer, 0.33f, SkillEffect.EffectIncrementType.AddFactor, "{=lVp2bwR9}{VALUE}% build speed for projects in the governed town.", SkillEffect.PerkRole.Governor, 0.12f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringMilitaryPlanner.Initialize("{=mzDsT7lV}Military Planner", DefaultSkills.Engineering, this.GetTierCost(3), this._engineeringCarpenters, "{=zU6gKebE}{VALUE}% ammunition to ranged troops when besieging.", SkillEffect.PerkRole.Engineer, 0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=xZqVL9wN}{VALUE}% build speed for projects in the governed castle.", SkillEffect.PerkRole.Governor, 0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringWallBreaker.Initialize("{=0wlWgIeL}Wall Breaker", DefaultSkills.Engineering, this.GetTierCost(4), this._engineeringDreadfulSieger, "{=JBa4DO2u}{VALUE}% damage dealt to walls during siege bombardment.", SkillEffect.PerkRole.Engineer, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=g3SKNcMV}{VALUE}% damage dealt to shields by troops in your formation.", SkillEffect.PerkRole.Captain, 0.1f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._engineeringDreadfulSieger.Initialize("{=bIS4kqmf}Dreadful Besieger", DefaultSkills.Engineering, this.GetTierCost(4), this._engineeringWallBreaker, "{=zUzfRYzf}{VALUE}% accuracy to your siege engines during siege bombardments in the governed settlement.", SkillEffect.PerkRole.Governor, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=cD8a5zbZ}{VALUE}% crossbow damage by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.CrossbowUser);
			this._engineeringSalvager.Initialize("{=AgJAfEEZ}Salvager", DefaultSkills.Engineering, this.GetTierCost(5), this._engineeringForeman, "{=mtb8vJ4o}{VALUE}% accuracy to ballistas during siege bombardment.", SkillEffect.PerkRole.Engineer, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=qfjgKCty}{VALUE}% siege engine build speed increase for each militia.", SkillEffect.PerkRole.Governor, 0.001f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringForeman.Initialize("{=3ML4EkWY}Foreman", DefaultSkills.Engineering, this.GetTierCost(5), this._engineeringSalvager, "{=M4IaRQJy}{VALUE}% mangonel and trebuchet accuracy during siege bombardment.", SkillEffect.PerkRole.Engineer, 0.1f, SkillEffect.EffectIncrementType.AddFactor, "{=ivrmsCFC}{VALUE} prosperity when a project is finished in the governed settlement.", SkillEffect.PerkRole.Governor, 100f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringStonecutters.Initialize("{=auIRGa2V}Stonecutters", DefaultSkills.Engineering, this.GetTierCost(6), this._engineeringSiegeEngineer, "{=uohYIaSw}{VALUE}% build speed for fortifications, aqueducts and barrack projects in the governed settlement.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=uakMSJY6}Fire versions of siege engines can be constructed.", SkillEffect.PerkRole.Engineer, 0f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringSiegeEngineer.Initialize("{=pFGhJxyN}Siege Engineer", DefaultSkills.Engineering, this.GetTierCost(6), this._engineeringStonecutters, "{=cRfa2IaT}{VALUE}% hit points to defensive siege engines in the governed settlement.", SkillEffect.PerkRole.Governor, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=uakMSJY6}Fire versions of siege engines can be constructed.", SkillEffect.PerkRole.Engineer, 0f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringCampBuilding.Initialize("{=Lv2pbg8c}Camp Building", DefaultSkills.Engineering, this.GetTierCost(7), this._engineeringBattlements, "{=fDSyE0eE}{VALUE}% cohesion loss of armies when besieging.", SkillEffect.PerkRole.ArmyCommander, -0.5f, SkillEffect.EffectIncrementType.AddFactor, "{=0T7AKmVS}{VALUE}% casualty chance from siege bombardments.", SkillEffect.PerkRole.Engineer, -0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringBattlements.Initialize("{=hHHEW1HN}Battlements", DefaultSkills.Engineering, this.GetTierCost(7), this._engineeringCampBuilding, "{=Ix98dg08}{VALUE} prebuilt ballista when you set up a siege camp.", SkillEffect.PerkRole.Engineer, 1f, SkillEffect.EffectIncrementType.Add, "{=hXqSlJM7}{VALUE} maximum granary capacity in the governed settlement.", SkillEffect.PerkRole.Governor, 100f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringEngineeringGuilds.Initialize("{=elKQc0O6}Engineering Guilds", DefaultSkills.Engineering, this.GetTierCost(8), this._engineeringApprenticeship, "{=KAozuVLa}{VALUE} recruitment slot when recruiting from artisan notables.", SkillEffect.PerkRole.Engineer, 1f, SkillEffect.EffectIncrementType.Add, "{=EIkzYco9}{VALUE}% wall hit points in the governed settlement.", SkillEffect.PerkRole.Governor, 0.25f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringApprenticeship.Initialize("{=yzybG5rl}Apprenticeship", DefaultSkills.Engineering, this.GetTierCost(8), this._engineeringEngineeringGuilds, "{=3m2tQF9F}{VALUE} experience to troops when a siege engine is built.", SkillEffect.PerkRole.Engineer, 5f, SkillEffect.EffectIncrementType.Add, "{=AeTSNsRu}{VALUE}% prosperity gain for each unique project in the governed settlement.", SkillEffect.PerkRole.Governor, 0.01f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringMetallurgy.Initialize("{=qjvDsu8u}Metallurgy", DefaultSkills.Engineering, this.GetTierCost(9), this._engineeringImprovedTools, "{=ZMVo5TTq}{VALUE}% chance to remove negative modifiers on looted items.", SkillEffect.PerkRole.Engineer, 0.3f, SkillEffect.EffectIncrementType.AddFactor, "{=XWPcZgM9}{VALUE} armor to all equipped armor pieces of troops in your formation.", SkillEffect.PerkRole.Captain, 5f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.None);
			this._engineeringImprovedTools.Initialize("{=XixNAaD5}Improved Tools", DefaultSkills.Engineering, this.GetTierCost(9), this._engineeringMetallurgy, "{=5ATpHJag}{VALUE}% siege camp preparation speed.", SkillEffect.PerkRole.Engineer, 0.2f, SkillEffect.EffectIncrementType.AddFactor, "{=eBmaa49a}{VALUE}% melee damage by troops in your formation.", SkillEffect.PerkRole.Captain, 0.05f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Melee);
			this._engineeringClockwork.Initialize("{=Z9Rey6LC}Clockwork", DefaultSkills.Engineering, this.GetTierCost(10), this._engineeringArchitecturalCommisions, "{=yn9GhVK4}{VALUE}% reload speed to ballistas during siege bombardment.", SkillEffect.PerkRole.Engineer, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=Jlmtufb3}{VALUE}% effect from boosting projects in the governed town.", SkillEffect.PerkRole.Governor, 0.2f, SkillEffect.EffectIncrementType.AddFactor, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringArchitecturalCommisions.Initialize("{=KODafKT7}Architectural Commissions", DefaultSkills.Engineering, this.GetTierCost(10), this._engineeringClockwork, "{=0aMHHQL4}{VALUE}% reload speed to mangonels and trebuchets in siege bombardment.", SkillEffect.PerkRole.Engineer, 0.25f, SkillEffect.EffectIncrementType.AddFactor, "{=e3ykBSpR}{VALUE} gold per day for continuous projects in the governed settlement.", SkillEffect.PerkRole.Governor, 20f, SkillEffect.EffectIncrementType.Add, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
			this._engineeringMasterwork.Initialize("{=SNsAlN4R}Masterwork", DefaultSkills.Engineering, this.GetTierCost(11), null, "{=RP2Jn3J4}{VALUE}% damage for each engineering skill point over 250 for siege engines in siege bombardment.", SkillEffect.PerkRole.Engineer, 0.01f, SkillEffect.EffectIncrementType.AddFactor, "", SkillEffect.PerkRole.None, 0f, SkillEffect.EffectIncrementType.Invalid, TroopUsageFlags.Undefined, TroopUsageFlags.Undefined);
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000CC18E File Offset: 0x000CA38E
		private int GetTierCost(int tierIndex)
		{
			return DefaultPerks.TierSkillRequirements[tierIndex - 1];
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000CC199 File Offset: 0x000CA399
		private PerkObject Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<PerkObject>(new PerkObject(stringId));
		}

		// Token: 0x04000E2F RID: 3631
		private static readonly int[] TierSkillRequirements = new int[]
		{
			25,
			50,
			75,
			100,
			125,
			150,
			175,
			200,
			225,
			250,
			275,
			300
		};

		// Token: 0x04000E30 RID: 3632
		private PerkObject _oneHandedBasher;

		// Token: 0x04000E31 RID: 3633
		private PerkObject _oneHandedToBeBlunt;

		// Token: 0x04000E32 RID: 3634
		private PerkObject _oneHandedSteelCoreShields;

		// Token: 0x04000E33 RID: 3635
		private PerkObject _oneHandedFleetOfFoot;

		// Token: 0x04000E34 RID: 3636
		private PerkObject _oneHandedDeadlyPurpose;

		// Token: 0x04000E35 RID: 3637
		private PerkObject _oneHandedUnwaveringDefense;

		// Token: 0x04000E36 RID: 3638
		private PerkObject _oneHandedWrappedHandles;

		// Token: 0x04000E37 RID: 3639
		private PerkObject _oneHandedWayOfTheSword;

		// Token: 0x04000E38 RID: 3640
		private PerkObject _oneHandedPrestige;

		// Token: 0x04000E39 RID: 3641
		private PerkObject _oneHandedChinkInTheArmor;

		// Token: 0x04000E3A RID: 3642
		private PerkObject _oneHandedStandUnited;

		// Token: 0x04000E3B RID: 3643
		private PerkObject _oneHandedLeadByExample;

		// Token: 0x04000E3C RID: 3644
		private PerkObject _oneHandedMilitaryTradition;

		// Token: 0x04000E3D RID: 3645
		private PerkObject _oneHandedCorpsACorps;

		// Token: 0x04000E3E RID: 3646
		private PerkObject _oneHandedShieldWall;

		// Token: 0x04000E3F RID: 3647
		private PerkObject _oneHandedArrowCatcher;

		// Token: 0x04000E40 RID: 3648
		private PerkObject _oneHandedShieldBearer;

		// Token: 0x04000E41 RID: 3649
		private PerkObject _oneHandedTrainer;

		// Token: 0x04000E42 RID: 3650
		private PerkObject _oneHandedDuelist;

		// Token: 0x04000E43 RID: 3651
		private PerkObject _oneHandedSwiftStrike;

		// Token: 0x04000E44 RID: 3652
		private PerkObject _oneHandedCavalry;

		// Token: 0x04000E45 RID: 3653
		private PerkObject _twoHandedWoodChopper;

		// Token: 0x04000E46 RID: 3654
		private PerkObject _twoHandedWayOfTheGreatAxe;

		// Token: 0x04000E47 RID: 3655
		private PerkObject _twoHandedStrongGrip;

		// Token: 0x04000E48 RID: 3656
		private PerkObject _twoHandedOnTheEdge;

		// Token: 0x04000E49 RID: 3657
		private PerkObject _twoHandedHeadBasher;

		// Token: 0x04000E4A RID: 3658
		private PerkObject _twoHandedShowOfStrength;

		// Token: 0x04000E4B RID: 3659
		private PerkObject _twoHandedBeastSlayer;

		// Token: 0x04000E4C RID: 3660
		private PerkObject _twoHandedBaptisedInBlood;

		// Token: 0x04000E4D RID: 3661
		private PerkObject _twoHandedShieldBreaker;

		// Token: 0x04000E4E RID: 3662
		private PerkObject _twoHandedConfidence;

		// Token: 0x04000E4F RID: 3663
		private PerkObject _twoHandedBerserker;

		// Token: 0x04000E50 RID: 3664
		private PerkObject _twoHandedProjectileDeflection;

		// Token: 0x04000E51 RID: 3665
		private PerkObject _twoHandedTerror;

		// Token: 0x04000E52 RID: 3666
		private PerkObject _twoHandedHope;

		// Token: 0x04000E53 RID: 3667
		private PerkObject _twoHandedThickHides;

		// Token: 0x04000E54 RID: 3668
		private PerkObject _twoHandedRecklessCharge;

		// Token: 0x04000E55 RID: 3669
		private PerkObject _twoHandedBladeMaster;

		// Token: 0x04000E56 RID: 3670
		private PerkObject _twoHandedVandal;

		// Token: 0x04000E57 RID: 3671
		public PerkObject _polearmPikeman;

		// Token: 0x04000E58 RID: 3672
		public PerkObject _polearmCavalry;

		// Token: 0x04000E59 RID: 3673
		public PerkObject _polearmBraced;

		// Token: 0x04000E5A RID: 3674
		public PerkObject _polearmKeepAtBay;

		// Token: 0x04000E5B RID: 3675
		public PerkObject _polearmSwiftSwing;

		// Token: 0x04000E5C RID: 3676
		public PerkObject _polearmCleanThrust;

		// Token: 0x04000E5D RID: 3677
		public PerkObject _polearmFootwork;

		// Token: 0x04000E5E RID: 3678
		public PerkObject _polearmHardKnock;

		// Token: 0x04000E5F RID: 3679
		public PerkObject _polearmSteedKiller;

		// Token: 0x04000E60 RID: 3680
		public PerkObject _polearmLancer;

		// Token: 0x04000E61 RID: 3681
		public PerkObject _polearmGuards;

		// Token: 0x04000E62 RID: 3682
		public PerkObject _polearmSkewer;

		// Token: 0x04000E63 RID: 3683
		public PerkObject _polearmStandardBearer;

		// Token: 0x04000E64 RID: 3684
		public PerkObject _polearmPhalanx;

		// Token: 0x04000E65 RID: 3685
		public PerkObject _polearmHardyFrontline;

		// Token: 0x04000E66 RID: 3686
		public PerkObject _polearmDrills;

		// Token: 0x04000E67 RID: 3687
		public PerkObject _polearmSureFooted;

		// Token: 0x04000E68 RID: 3688
		public PerkObject _polearmUnstoppableForce;

		// Token: 0x04000E69 RID: 3689
		public PerkObject _polearmCounterweight;

		// Token: 0x04000E6A RID: 3690
		public PerkObject _polearmWayOfTheSpear;

		// Token: 0x04000E6B RID: 3691
		public PerkObject _polearmSharpenTheTip;

		// Token: 0x04000E6C RID: 3692
		public PerkObject _bowDeadAim;

		// Token: 0x04000E6D RID: 3693
		public PerkObject _bowBodkin;

		// Token: 0x04000E6E RID: 3694
		public PerkObject _bowRangersSwiftness;

		// Token: 0x04000E6F RID: 3695
		public PerkObject _bowRapidFire;

		// Token: 0x04000E70 RID: 3696
		public PerkObject _bowQuickAdjustments;

		// Token: 0x04000E71 RID: 3697
		public PerkObject _bowMerryMen;

		// Token: 0x04000E72 RID: 3698
		public PerkObject _bowMountedArchery;

		// Token: 0x04000E73 RID: 3699
		public PerkObject _bowTrainer;

		// Token: 0x04000E74 RID: 3700
		public PerkObject _bowStrongBows;

		// Token: 0x04000E75 RID: 3701
		public PerkObject _bowDiscipline;

		// Token: 0x04000E76 RID: 3702
		public PerkObject _bowHunterClan;

		// Token: 0x04000E77 RID: 3703
		public PerkObject _bowSkirmishPhaseMaster;

		// Token: 0x04000E78 RID: 3704
		public PerkObject _bowEagleEye;

		// Token: 0x04000E79 RID: 3705
		public PerkObject _bowBullsEye;

		// Token: 0x04000E7A RID: 3706
		public PerkObject _bowRenownedArcher;

		// Token: 0x04000E7B RID: 3707
		public PerkObject _bowHorseMaster;

		// Token: 0x04000E7C RID: 3708
		public PerkObject _bowDeepQuivers;

		// Token: 0x04000E7D RID: 3709
		public PerkObject _bowQuickDraw;

		// Token: 0x04000E7E RID: 3710
		public PerkObject _bowNockingPoint;

		// Token: 0x04000E7F RID: 3711
		public PerkObject _bowBowControl;

		// Token: 0x04000E80 RID: 3712
		public PerkObject _bowDeadshot;

		// Token: 0x04000E81 RID: 3713
		public PerkObject _crossbowMarksmen;

		// Token: 0x04000E82 RID: 3714
		public PerkObject _crossbowUnhorser;

		// Token: 0x04000E83 RID: 3715
		public PerkObject _crossbowWindWinder;

		// Token: 0x04000E84 RID: 3716
		public PerkObject _crossbowDonkeysSwiftness;

		// Token: 0x04000E85 RID: 3717
		public PerkObject _crossbowSheriff;

		// Token: 0x04000E86 RID: 3718
		public PerkObject _crossbowPeasantLeader;

		// Token: 0x04000E87 RID: 3719
		public PerkObject _crossbowRenownMarksmen;

		// Token: 0x04000E88 RID: 3720
		public PerkObject _crossbowFletcher;

		// Token: 0x04000E89 RID: 3721
		public PerkObject _crossbowPuncture;

		// Token: 0x04000E8A RID: 3722
		public PerkObject _crossbowLooseAndMove;

		// Token: 0x04000E8B RID: 3723
		public PerkObject _crossbowDeftHands;

		// Token: 0x04000E8C RID: 3724
		public PerkObject _crossbowCounterFire;

		// Token: 0x04000E8D RID: 3725
		public PerkObject _crossbowMountedCrossbowman;

		// Token: 0x04000E8E RID: 3726
		public PerkObject _crossbowSteady;

		// Token: 0x04000E8F RID: 3727
		public PerkObject _crossbowLongShots;

		// Token: 0x04000E90 RID: 3728
		public PerkObject _crossbowHammerBolts;

		// Token: 0x04000E91 RID: 3729
		public PerkObject _crossbowPavise;

		// Token: 0x04000E92 RID: 3730
		public PerkObject _crossbowTerror;

		// Token: 0x04000E93 RID: 3731
		public PerkObject _crossbowPickedShots;

		// Token: 0x04000E94 RID: 3732
		public PerkObject _crossbowPiercer;

		// Token: 0x04000E95 RID: 3733
		public PerkObject _crossbowMightyPull;

		// Token: 0x04000E96 RID: 3734
		private PerkObject _throwingShieldBreaker;

		// Token: 0x04000E97 RID: 3735
		private PerkObject _throwingHunter;

		// Token: 0x04000E98 RID: 3736
		private PerkObject _throwingFlexibleFighter;

		// Token: 0x04000E99 RID: 3737
		private PerkObject _throwingMountedSkirmisher;

		// Token: 0x04000E9A RID: 3738
		private PerkObject _throwingPerfectTechnique;

		// Token: 0x04000E9B RID: 3739
		private PerkObject _throwingRunningThrow;

		// Token: 0x04000E9C RID: 3740
		private PerkObject _throwingKnockOff;

		// Token: 0x04000E9D RID: 3741
		private PerkObject _throwingWellPrepared;

		// Token: 0x04000E9E RID: 3742
		private PerkObject _throwingSkirmisher;

		// Token: 0x04000E9F RID: 3743
		private PerkObject _throwingFocus;

		// Token: 0x04000EA0 RID: 3744
		private PerkObject _throwingLastHit;

		// Token: 0x04000EA1 RID: 3745
		private PerkObject _throwingHeadHunter;

		// Token: 0x04000EA2 RID: 3746
		private PerkObject _throwingThrowingCompetitions;

		// Token: 0x04000EA3 RID: 3747
		private PerkObject _throwingSaddlebags;

		// Token: 0x04000EA4 RID: 3748
		private PerkObject _throwingSplinters;

		// Token: 0x04000EA5 RID: 3749
		private PerkObject _throwingResourceful;

		// Token: 0x04000EA6 RID: 3750
		private PerkObject _throwingLongReach;

		// Token: 0x04000EA7 RID: 3751
		private PerkObject _throwingWeakSpot;

		// Token: 0x04000EA8 RID: 3752
		private PerkObject _throwingQuickDraw;

		// Token: 0x04000EA9 RID: 3753
		private PerkObject _throwingImpale;

		// Token: 0x04000EAA RID: 3754
		private PerkObject _throwingUnstoppableForce;

		// Token: 0x04000EAB RID: 3755
		private PerkObject _ridingNimbleSteed;

		// Token: 0x04000EAC RID: 3756
		private PerkObject _ridingWellStraped;

		// Token: 0x04000EAD RID: 3757
		private PerkObject _ridingVeterinary;

		// Token: 0x04000EAE RID: 3758
		private PerkObject _ridingNomadicTraditions;

		// Token: 0x04000EAF RID: 3759
		private PerkObject _ridingDeeperSacks;

		// Token: 0x04000EB0 RID: 3760
		private PerkObject _ridingSagittarius;

		// Token: 0x04000EB1 RID: 3761
		private PerkObject _ridingSweepingWind;

		// Token: 0x04000EB2 RID: 3762
		private PerkObject _ridingReliefForce;

		// Token: 0x04000EB3 RID: 3763
		private PerkObject _ridingMountedWarrior;

		// Token: 0x04000EB4 RID: 3764
		private PerkObject _ridingHorseArcher;

		// Token: 0x04000EB5 RID: 3765
		private PerkObject _ridingShepherd;

		// Token: 0x04000EB6 RID: 3766
		private PerkObject _ridingBreeder;

		// Token: 0x04000EB7 RID: 3767
		private PerkObject _ridingThunderousCharge;

		// Token: 0x04000EB8 RID: 3768
		private PerkObject _ridingAnnoyingBuzz;

		// Token: 0x04000EB9 RID: 3769
		private PerkObject _ridingMountedPatrols;

		// Token: 0x04000EBA RID: 3770
		private PerkObject _ridingCavalryTactics;

		// Token: 0x04000EBB RID: 3771
		private PerkObject _ridingDauntlessSteed;

		// Token: 0x04000EBC RID: 3772
		private PerkObject _ridingToughSteed;

		// Token: 0x04000EBD RID: 3773
		private PerkObject _ridingFullSpeed;

		// Token: 0x04000EBE RID: 3774
		private PerkObject _ridingTheWayOfTheSaddle;

		// Token: 0x04000EBF RID: 3775
		private PerkObject _athleticsFormFittingArmor;

		// Token: 0x04000EC0 RID: 3776
		private PerkObject _athleticsImposingStature;

		// Token: 0x04000EC1 RID: 3777
		private PerkObject _athleticsStamina;

		// Token: 0x04000EC2 RID: 3778
		private PerkObject _athleticsSprint;

		// Token: 0x04000EC3 RID: 3779
		private PerkObject _athleticsPowerful;

		// Token: 0x04000EC4 RID: 3780
		private PerkObject _athleticsSurgingBlow;

		// Token: 0x04000EC5 RID: 3781
		private PerkObject _athleticsWellBuilt;

		// Token: 0x04000EC6 RID: 3782
		private PerkObject _athleticsFury;

		// Token: 0x04000EC7 RID: 3783
		private PerkObject _athleticsBraced;

		// Token: 0x04000EC8 RID: 3784
		private PerkObject _athleticsAGoodDaysRest;

		// Token: 0x04000EC9 RID: 3785
		private PerkObject _athleticsDurable;

		// Token: 0x04000ECA RID: 3786
		private PerkObject _athleticsEnergetic;

		// Token: 0x04000ECB RID: 3787
		private PerkObject _athleticsSteady;

		// Token: 0x04000ECC RID: 3788
		private PerkObject _athleticsStrong;

		// Token: 0x04000ECD RID: 3789
		private PerkObject _athleticsStrongLegs;

		// Token: 0x04000ECE RID: 3790
		private PerkObject _athleticsStrongArms;

		// Token: 0x04000ECF RID: 3791
		private PerkObject _athleticsSpartan;

		// Token: 0x04000ED0 RID: 3792
		private PerkObject _athleticsMorningExercise;

		// Token: 0x04000ED1 RID: 3793
		private PerkObject _athleticsIgnorePain;

		// Token: 0x04000ED2 RID: 3794
		private PerkObject _athleticsWalkItOff;

		// Token: 0x04000ED3 RID: 3795
		private PerkObject _athleticsMightyBlow;

		// Token: 0x04000ED4 RID: 3796
		private PerkObject _craftingSteelMaker2;

		// Token: 0x04000ED5 RID: 3797
		private PerkObject _craftingSteelMaker3;

		// Token: 0x04000ED6 RID: 3798
		private PerkObject _craftingCharcoalMaker;

		// Token: 0x04000ED7 RID: 3799
		private PerkObject _craftingSteelMaker;

		// Token: 0x04000ED8 RID: 3800
		private PerkObject _craftingCuriousSmelter;

		// Token: 0x04000ED9 RID: 3801
		private PerkObject _craftingCuriousSmith;

		// Token: 0x04000EDA RID: 3802
		private PerkObject _craftingPracticalSmelter;

		// Token: 0x04000EDB RID: 3803
		private PerkObject _craftingPracticalRefiner;

		// Token: 0x04000EDC RID: 3804
		private PerkObject _craftingPracticalSmith;

		// Token: 0x04000EDD RID: 3805
		private PerkObject _craftingArtisanSmith;

		// Token: 0x04000EDE RID: 3806
		private PerkObject _craftingExperiencedSmith;

		// Token: 0x04000EDF RID: 3807
		private PerkObject _craftingMasterSmith;

		// Token: 0x04000EE0 RID: 3808
		private PerkObject _craftingLegendarySmith;

		// Token: 0x04000EE1 RID: 3809
		private PerkObject _craftingVigorousSmith;

		// Token: 0x04000EE2 RID: 3810
		private PerkObject _craftingStrongSmith;

		// Token: 0x04000EE3 RID: 3811
		private PerkObject _craftingEnduringSmith;

		// Token: 0x04000EE4 RID: 3812
		private PerkObject _craftingIronMaker;

		// Token: 0x04000EE5 RID: 3813
		private PerkObject _craftingFencerSmith;

		// Token: 0x04000EE6 RID: 3814
		private PerkObject _craftingSharpenedEdge;

		// Token: 0x04000EE7 RID: 3815
		private PerkObject _craftingSharpenedTip;

		// Token: 0x04000EE8 RID: 3816
		private PerkObject _tacticsSmallUnitTactics;

		// Token: 0x04000EE9 RID: 3817
		private PerkObject _tacticsHordeLeader;

		// Token: 0x04000EEA RID: 3818
		private PerkObject _tacticsLawKeeper;

		// Token: 0x04000EEB RID: 3819
		private PerkObject _tacticsLooseFormations;

		// Token: 0x04000EEC RID: 3820
		private PerkObject _tacticsSwiftRegroup;

		// Token: 0x04000EED RID: 3821
		private PerkObject _tacticsExtendedSkirmish;

		// Token: 0x04000EEE RID: 3822
		private PerkObject _tacticsDecisiveBattle;

		// Token: 0x04000EEF RID: 3823
		private PerkObject _tacticsCoaching;

		// Token: 0x04000EF0 RID: 3824
		private PerkObject _tacticsImproviser;

		// Token: 0x04000EF1 RID: 3825
		private PerkObject _tacticsOnTheMarch;

		// Token: 0x04000EF2 RID: 3826
		private PerkObject _tacticsCallToArms;

		// Token: 0x04000EF3 RID: 3827
		private PerkObject _tacticsPickThemOfTheWalls;

		// Token: 0x04000EF4 RID: 3828
		private PerkObject _tacticsMakeThemPay;

		// Token: 0x04000EF5 RID: 3829
		private PerkObject _tacticsEliteReserves;

		// Token: 0x04000EF6 RID: 3830
		private PerkObject _tacticsEncirclement;

		// Token: 0x04000EF7 RID: 3831
		private PerkObject _tacticsPreBattleManeuvers;

		// Token: 0x04000EF8 RID: 3832
		private PerkObject _tacticsBesieged;

		// Token: 0x04000EF9 RID: 3833
		private PerkObject _tacticsCounteroffensive;

		// Token: 0x04000EFA RID: 3834
		private PerkObject _tacticsGensdarmes;

		// Token: 0x04000EFB RID: 3835
		private PerkObject _tacticsTightFormations;

		// Token: 0x04000EFC RID: 3836
		private PerkObject _tacticsTacticalMastery;

		// Token: 0x04000EFD RID: 3837
		private PerkObject _scoutingNightRunner;

		// Token: 0x04000EFE RID: 3838
		private PerkObject _scoutingWaterDiviner;

		// Token: 0x04000EFF RID: 3839
		private PerkObject _scoutingForestKin;

		// Token: 0x04000F00 RID: 3840
		private PerkObject _scoutingForcedMarch;

		// Token: 0x04000F01 RID: 3841
		private PerkObject _scoutingDesertBorn;

		// Token: 0x04000F02 RID: 3842
		private PerkObject _scoutingPathfinder;

		// Token: 0x04000F03 RID: 3843
		private PerkObject _scoutingUnburdened;

		// Token: 0x04000F04 RID: 3844
		private PerkObject _scoutingTracker;

		// Token: 0x04000F05 RID: 3845
		private PerkObject _scoutingRanger;

		// Token: 0x04000F06 RID: 3846
		private PerkObject _scoutingMountedScouts;

		// Token: 0x04000F07 RID: 3847
		private PerkObject _scoutingPatrols;

		// Token: 0x04000F08 RID: 3848
		private PerkObject _scoutingForagers;

		// Token: 0x04000F09 RID: 3849
		private PerkObject _scoutingBeastWhisperer;

		// Token: 0x04000F0A RID: 3850
		private PerkObject _scoutingVillageNetwork;

		// Token: 0x04000F0B RID: 3851
		private PerkObject _scoutingRumourNetwork;

		// Token: 0x04000F0C RID: 3852
		private PerkObject _scoutingVantagePoint;

		// Token: 0x04000F0D RID: 3853
		private PerkObject _scoutingKeenSight;

		// Token: 0x04000F0E RID: 3854
		private PerkObject _scoutingVanguard;

		// Token: 0x04000F0F RID: 3855
		private PerkObject _scoutingRearguard;

		// Token: 0x04000F10 RID: 3856
		private PerkObject _scoutingDayTraveler;

		// Token: 0x04000F11 RID: 3857
		private PerkObject _scoutingUncannyInsight;

		// Token: 0x04000F12 RID: 3858
		private PerkObject _rogueryTwoFaced;

		// Token: 0x04000F13 RID: 3859
		private PerkObject _rogueryDeepPockets;

		// Token: 0x04000F14 RID: 3860
		private PerkObject _rogueryInBestLight;

		// Token: 0x04000F15 RID: 3861
		private PerkObject _roguerySweetTalker;

		// Token: 0x04000F16 RID: 3862
		private PerkObject _rogueryKnowHow;

		// Token: 0x04000F17 RID: 3863
		private PerkObject _rogueryManhunter;

		// Token: 0x04000F18 RID: 3864
		private PerkObject _rogueryPromises;

		// Token: 0x04000F19 RID: 3865
		private PerkObject _rogueryScarface;

		// Token: 0x04000F1A RID: 3866
		private PerkObject _rogueryWhiteLies;

		// Token: 0x04000F1B RID: 3867
		private PerkObject _roguerySmugglerConnections;

		// Token: 0x04000F1C RID: 3868
		private PerkObject _rogueryPartnersInCrime;

		// Token: 0x04000F1D RID: 3869
		private PerkObject _rogueryOneOfTheFamily;

		// Token: 0x04000F1E RID: 3870
		private PerkObject _roguerySaltTheEarth;

		// Token: 0x04000F1F RID: 3871
		private PerkObject _rogueryCarver;

		// Token: 0x04000F20 RID: 3872
		private PerkObject _rogueryRansomBroker;

		// Token: 0x04000F21 RID: 3873
		private PerkObject _rogueryArmsDealer;

		// Token: 0x04000F22 RID: 3874
		private PerkObject _rogueryDirtyFighting;

		// Token: 0x04000F23 RID: 3875
		private PerkObject _rogueryDashAndSlash;

		// Token: 0x04000F24 RID: 3876
		private PerkObject _rogueryFleetFooted;

		// Token: 0x04000F25 RID: 3877
		private PerkObject _rogueryNoRestForTheWicked;

		// Token: 0x04000F26 RID: 3878
		private PerkObject _rogueryRogueExtraordinaire;

		// Token: 0x04000F27 RID: 3879
		private PerkObject _leadershipFerventAttacker;

		// Token: 0x04000F28 RID: 3880
		private PerkObject _leadershipStoutDefender;

		// Token: 0x04000F29 RID: 3881
		private PerkObject _leadershipAuthority;

		// Token: 0x04000F2A RID: 3882
		private PerkObject _leadershipHeroicLeader;

		// Token: 0x04000F2B RID: 3883
		private PerkObject _leadershipLoyaltyAndHonor;

		// Token: 0x04000F2C RID: 3884
		private PerkObject _leadershipFamousCommander;

		// Token: 0x04000F2D RID: 3885
		private PerkObject _leadershipRaiseTheMeek;

		// Token: 0x04000F2E RID: 3886
		private PerkObject _leadershipPresence;

		// Token: 0x04000F2F RID: 3887
		private PerkObject _leadershipVeteransRespect;

		// Token: 0x04000F30 RID: 3888
		private PerkObject _leadershipLeaderOfTheMasses;

		// Token: 0x04000F31 RID: 3889
		private PerkObject _leadershipInspiringLeader;

		// Token: 0x04000F32 RID: 3890
		private PerkObject _leadershipUpliftingSpirit;

		// Token: 0x04000F33 RID: 3891
		private PerkObject _leadershipMakeADifference;

		// Token: 0x04000F34 RID: 3892
		private PerkObject _leadershipLeadByExample;

		// Token: 0x04000F35 RID: 3893
		private PerkObject _leadershipTrustedCommander;

		// Token: 0x04000F36 RID: 3894
		private PerkObject _leadershipGreatLeader;

		// Token: 0x04000F37 RID: 3895
		private PerkObject _leadershipWePledgeOurSwords;

		// Token: 0x04000F38 RID: 3896
		private PerkObject _leadershipUltimateLeader;

		// Token: 0x04000F39 RID: 3897
		private PerkObject _leadershipTalentMagnet;

		// Token: 0x04000F3A RID: 3898
		private PerkObject _leadershipCitizenMilitia;

		// Token: 0x04000F3B RID: 3899
		private PerkObject _leadershipCombatTips;

		// Token: 0x04000F3C RID: 3900
		private PerkObject _charmVirile;

		// Token: 0x04000F3D RID: 3901
		private PerkObject _charmSelfPromoter;

		// Token: 0x04000F3E RID: 3902
		private PerkObject _charmOratory;

		// Token: 0x04000F3F RID: 3903
		private PerkObject _charmWarlord;

		// Token: 0x04000F40 RID: 3904
		private PerkObject _charmForgivableGrievances;

		// Token: 0x04000F41 RID: 3905
		private PerkObject _charmMeaningfulFavors;

		// Token: 0x04000F42 RID: 3906
		private PerkObject _charmInBloom;

		// Token: 0x04000F43 RID: 3907
		private PerkObject _charmYoungAndRespectful;

		// Token: 0x04000F44 RID: 3908
		private PerkObject _charmFirebrand;

		// Token: 0x04000F45 RID: 3909
		private PerkObject _charmFlexibleEthics;

		// Token: 0x04000F46 RID: 3910
		private PerkObject _charmEffortForThePeople;

		// Token: 0x04000F47 RID: 3911
		private PerkObject _charmSlickNegotiator;

		// Token: 0x04000F48 RID: 3912
		private PerkObject _charmGoodNatured;

		// Token: 0x04000F49 RID: 3913
		private PerkObject _charmTribute;

		// Token: 0x04000F4A RID: 3914
		private PerkObject _charmMoralLeader;

		// Token: 0x04000F4B RID: 3915
		private PerkObject _charmNaturalLeader;

		// Token: 0x04000F4C RID: 3916
		private PerkObject _charmPublicSpeaker;

		// Token: 0x04000F4D RID: 3917
		private PerkObject _charmParade;

		// Token: 0x04000F4E RID: 3918
		private PerkObject _charmCamaraderie;

		// Token: 0x04000F4F RID: 3919
		private PerkObject _charmImmortalCharm;

		// Token: 0x04000F50 RID: 3920
		private PerkObject _tradeTravelingRumors;

		// Token: 0x04000F51 RID: 3921
		private PerkObject _tradeLocalConnection;

		// Token: 0x04000F52 RID: 3922
		private PerkObject _tradeDistributedGoods;

		// Token: 0x04000F53 RID: 3923
		private PerkObject _tradeTollgates;

		// Token: 0x04000F54 RID: 3924
		private PerkObject _tradeArtisanCommunity;

		// Token: 0x04000F55 RID: 3925
		private PerkObject _tradeGreatInvestor;

		// Token: 0x04000F56 RID: 3926
		private PerkObject _tradeMercenaryConnections;

		// Token: 0x04000F57 RID: 3927
		private PerkObject _tradeContentTrades;

		// Token: 0x04000F58 RID: 3928
		private PerkObject _tradeInsurancePlans;

		// Token: 0x04000F59 RID: 3929
		private PerkObject _tradeRapidDevelopment;

		// Token: 0x04000F5A RID: 3930
		private PerkObject _tradeGranaryAccountant;

		// Token: 0x04000F5B RID: 3931
		private PerkObject _tradeTradeyardForeman;

		// Token: 0x04000F5C RID: 3932
		private PerkObject _tradeWholeSeller;

		// Token: 0x04000F5D RID: 3933
		private PerkObject _tradeCaravanMaster;

		// Token: 0x04000F5E RID: 3934
		private PerkObject _tradeMarketDealer;

		// Token: 0x04000F5F RID: 3935
		private PerkObject _tradeSwordForBarter;

		// Token: 0x04000F60 RID: 3936
		private PerkObject _tradeTrickleDown;

		// Token: 0x04000F61 RID: 3937
		private PerkObject _tradeManOfMeans;

		// Token: 0x04000F62 RID: 3938
		private PerkObject _tradeSpringOfGold;

		// Token: 0x04000F63 RID: 3939
		private PerkObject _tradeSilverTongue;

		// Token: 0x04000F64 RID: 3940
		private PerkObject _tradeSelfMadeMan;

		// Token: 0x04000F65 RID: 3941
		private PerkObject _tradeAppraiser;

		// Token: 0x04000F66 RID: 3942
		private PerkObject _tradeEverythingHasAPrice;

		// Token: 0x04000F67 RID: 3943
		private PerkObject _medicinePreventiveMedicine;

		// Token: 0x04000F68 RID: 3944
		private PerkObject _medicineTriageTent;

		// Token: 0x04000F69 RID: 3945
		private PerkObject _medicineWalkItOff;

		// Token: 0x04000F6A RID: 3946
		private PerkObject _medicineSledges;

		// Token: 0x04000F6B RID: 3947
		private PerkObject _medicineDoctorsOath;

		// Token: 0x04000F6C RID: 3948
		private PerkObject _medicineBestMedicine;

		// Token: 0x04000F6D RID: 3949
		private PerkObject _medicineGoodLodging;

		// Token: 0x04000F6E RID: 3950
		private PerkObject _medicineSiegeMedic;

		// Token: 0x04000F6F RID: 3951
		private PerkObject _medicineVeterinarian;

		// Token: 0x04000F70 RID: 3952
		private PerkObject _medicinePristineStreets;

		// Token: 0x04000F71 RID: 3953
		private PerkObject _medicineBushDoctor;

		// Token: 0x04000F72 RID: 3954
		private PerkObject _medicinePerfectHealth;

		// Token: 0x04000F73 RID: 3955
		private PerkObject _medicineHealthAdvise;

		// Token: 0x04000F74 RID: 3956
		private PerkObject _medicinePhysicianOfPeople;

		// Token: 0x04000F75 RID: 3957
		private PerkObject _medicineCleanInfrastructure;

		// Token: 0x04000F76 RID: 3958
		private PerkObject _medicineCheatDeath;

		// Token: 0x04000F77 RID: 3959
		private PerkObject _medicineHelpingHands;

		// Token: 0x04000F78 RID: 3960
		private PerkObject _medicineFortitudeTonic;

		// Token: 0x04000F79 RID: 3961
		private PerkObject _medicineBattleHardened;

		// Token: 0x04000F7A RID: 3962
		private PerkObject _medicineMinisterOfHealth;

		// Token: 0x04000F7B RID: 3963
		private PerkObject _medicineSelfMedication;

		// Token: 0x04000F7C RID: 3964
		private PerkObject _stewardFrugal;

		// Token: 0x04000F7D RID: 3965
		private PerkObject _stewardSevenVeterans;

		// Token: 0x04000F7E RID: 3966
		private PerkObject _stewardDrillSergant;

		// Token: 0x04000F7F RID: 3967
		private PerkObject _stewardSweatshops;

		// Token: 0x04000F80 RID: 3968
		private PerkObject _stewardEfficientCampaigner;

		// Token: 0x04000F81 RID: 3969
		private PerkObject _stewardGivingHands;

		// Token: 0x04000F82 RID: 3970
		private PerkObject _stewardLogistician;

		// Token: 0x04000F83 RID: 3971
		private PerkObject _stewardStiffUpperLip;

		// Token: 0x04000F84 RID: 3972
		private PerkObject _stewardPaidInPromise;

		// Token: 0x04000F85 RID: 3973
		private PerkObject _stewardRelocation;

		// Token: 0x04000F86 RID: 3974
		private PerkObject _stewardAidCorps;

		// Token: 0x04000F87 RID: 3975
		private PerkObject _stewardGourmet;

		// Token: 0x04000F88 RID: 3976
		private PerkObject _stewardSoundReserves;

		// Token: 0x04000F89 RID: 3977
		private PerkObject _stewardArenicosMules;

		// Token: 0x04000F8A RID: 3978
		private PerkObject _stewardForcedLabor;

		// Token: 0x04000F8B RID: 3979
		private PerkObject _stewardPriceOfLoyalty;

		// Token: 0x04000F8C RID: 3980
		private PerkObject _stewardContractors;

		// Token: 0x04000F8D RID: 3981
		private PerkObject _stewardMasterOfWarcraft;

		// Token: 0x04000F8E RID: 3982
		private PerkObject _stewardMasterOfPlanning;

		// Token: 0x04000F8F RID: 3983
		private PerkObject _stewardWarriorsDiet;

		// Token: 0x04000F90 RID: 3984
		private PerkObject _stewardArenicosHorses;

		// Token: 0x04000F91 RID: 3985
		private PerkObject _engineeringSiegeWorks;

		// Token: 0x04000F92 RID: 3986
		private PerkObject _engineeringCarpenters;

		// Token: 0x04000F93 RID: 3987
		private PerkObject _engineeringDungeonArchitect;

		// Token: 0x04000F94 RID: 3988
		private PerkObject _engineeringMilitaryPlanner;

		// Token: 0x04000F95 RID: 3989
		private PerkObject _engineeringDreadfulSieger;

		// Token: 0x04000F96 RID: 3990
		private PerkObject _engineeringTorsionEngines;

		// Token: 0x04000F97 RID: 3991
		private PerkObject _engineeringSalvager;

		// Token: 0x04000F98 RID: 3992
		private PerkObject _engineeringForeman;

		// Token: 0x04000F99 RID: 3993
		private PerkObject _engineeringWallBreaker;

		// Token: 0x04000F9A RID: 3994
		private PerkObject _engineeringStonecutters;

		// Token: 0x04000F9B RID: 3995
		private PerkObject _engineeringSiegeEngineer;

		// Token: 0x04000F9C RID: 3996
		private PerkObject _engineeringCampBuilding;

		// Token: 0x04000F9D RID: 3997
		private PerkObject _engineeringBattlements;

		// Token: 0x04000F9E RID: 3998
		private PerkObject _engineeringEngineeringGuilds;

		// Token: 0x04000F9F RID: 3999
		private PerkObject _engineeringApprenticeship;

		// Token: 0x04000FA0 RID: 4000
		private PerkObject _engineeringMetallurgy;

		// Token: 0x04000FA1 RID: 4001
		private PerkObject _engineeringImprovedTools;

		// Token: 0x04000FA2 RID: 4002
		private PerkObject _engineeringClockwork;

		// Token: 0x04000FA3 RID: 4003
		private PerkObject _engineeringArchitecturalCommisions;

		// Token: 0x04000FA4 RID: 4004
		private PerkObject _engineeringScaffolds;

		// Token: 0x04000FA5 RID: 4005
		private PerkObject _engineeringMasterwork;

		// Token: 0x02000687 RID: 1671
		public static class OneHanded
		{
			// Token: 0x1700121D RID: 4637
			// (get) Token: 0x060054B3 RID: 21683 RVA: 0x0017D6AA File Offset: 0x0017B8AA
			public static PerkObject WrappedHandles
			{
				get
				{
					return DefaultPerks.Instance._oneHandedWrappedHandles;
				}
			}

			// Token: 0x1700121E RID: 4638
			// (get) Token: 0x060054B4 RID: 21684 RVA: 0x0017D6B6 File Offset: 0x0017B8B6
			public static PerkObject Basher
			{
				get
				{
					return DefaultPerks.Instance._oneHandedBasher;
				}
			}

			// Token: 0x1700121F RID: 4639
			// (get) Token: 0x060054B5 RID: 21685 RVA: 0x0017D6C2 File Offset: 0x0017B8C2
			public static PerkObject ToBeBlunt
			{
				get
				{
					return DefaultPerks.Instance._oneHandedToBeBlunt;
				}
			}

			// Token: 0x17001220 RID: 4640
			// (get) Token: 0x060054B6 RID: 21686 RVA: 0x0017D6CE File Offset: 0x0017B8CE
			public static PerkObject SwiftStrike
			{
				get
				{
					return DefaultPerks.Instance._oneHandedSwiftStrike;
				}
			}

			// Token: 0x17001221 RID: 4641
			// (get) Token: 0x060054B7 RID: 21687 RVA: 0x0017D6DA File Offset: 0x0017B8DA
			public static PerkObject Cavalry
			{
				get
				{
					return DefaultPerks.Instance._oneHandedCavalry;
				}
			}

			// Token: 0x17001222 RID: 4642
			// (get) Token: 0x060054B8 RID: 21688 RVA: 0x0017D6E6 File Offset: 0x0017B8E6
			public static PerkObject ShieldBearer
			{
				get
				{
					return DefaultPerks.Instance._oneHandedShieldBearer;
				}
			}

			// Token: 0x17001223 RID: 4643
			// (get) Token: 0x060054B9 RID: 21689 RVA: 0x0017D6F2 File Offset: 0x0017B8F2
			public static PerkObject Trainer
			{
				get
				{
					return DefaultPerks.Instance._oneHandedTrainer;
				}
			}

			// Token: 0x17001224 RID: 4644
			// (get) Token: 0x060054BA RID: 21690 RVA: 0x0017D6FE File Offset: 0x0017B8FE
			public static PerkObject Duelist
			{
				get
				{
					return DefaultPerks.Instance._oneHandedDuelist;
				}
			}

			// Token: 0x17001225 RID: 4645
			// (get) Token: 0x060054BB RID: 21691 RVA: 0x0017D70A File Offset: 0x0017B90A
			public static PerkObject ShieldWall
			{
				get
				{
					return DefaultPerks.Instance._oneHandedShieldWall;
				}
			}

			// Token: 0x17001226 RID: 4646
			// (get) Token: 0x060054BC RID: 21692 RVA: 0x0017D716 File Offset: 0x0017B916
			public static PerkObject ArrowCatcher
			{
				get
				{
					return DefaultPerks.Instance._oneHandedArrowCatcher;
				}
			}

			// Token: 0x17001227 RID: 4647
			// (get) Token: 0x060054BD RID: 21693 RVA: 0x0017D722 File Offset: 0x0017B922
			public static PerkObject MilitaryTradition
			{
				get
				{
					return DefaultPerks.Instance._oneHandedMilitaryTradition;
				}
			}

			// Token: 0x17001228 RID: 4648
			// (get) Token: 0x060054BE RID: 21694 RVA: 0x0017D72E File Offset: 0x0017B92E
			public static PerkObject CorpsACorps
			{
				get
				{
					return DefaultPerks.Instance._oneHandedCorpsACorps;
				}
			}

			// Token: 0x17001229 RID: 4649
			// (get) Token: 0x060054BF RID: 21695 RVA: 0x0017D73A File Offset: 0x0017B93A
			public static PerkObject StandUnited
			{
				get
				{
					return DefaultPerks.Instance._oneHandedStandUnited;
				}
			}

			// Token: 0x1700122A RID: 4650
			// (get) Token: 0x060054C0 RID: 21696 RVA: 0x0017D746 File Offset: 0x0017B946
			public static PerkObject LeadByExample
			{
				get
				{
					return DefaultPerks.Instance._oneHandedLeadByExample;
				}
			}

			// Token: 0x1700122B RID: 4651
			// (get) Token: 0x060054C1 RID: 21697 RVA: 0x0017D752 File Offset: 0x0017B952
			public static PerkObject SteelCoreShields
			{
				get
				{
					return DefaultPerks.Instance._oneHandedSteelCoreShields;
				}
			}

			// Token: 0x1700122C RID: 4652
			// (get) Token: 0x060054C2 RID: 21698 RVA: 0x0017D75E File Offset: 0x0017B95E
			public static PerkObject FleetOfFoot
			{
				get
				{
					return DefaultPerks.Instance._oneHandedFleetOfFoot;
				}
			}

			// Token: 0x1700122D RID: 4653
			// (get) Token: 0x060054C3 RID: 21699 RVA: 0x0017D76A File Offset: 0x0017B96A
			public static PerkObject DeadlyPurpose
			{
				get
				{
					return DefaultPerks.Instance._oneHandedDeadlyPurpose;
				}
			}

			// Token: 0x1700122E RID: 4654
			// (get) Token: 0x060054C4 RID: 21700 RVA: 0x0017D776 File Offset: 0x0017B976
			public static PerkObject UnwaveringDefense
			{
				get
				{
					return DefaultPerks.Instance._oneHandedUnwaveringDefense;
				}
			}

			// Token: 0x1700122F RID: 4655
			// (get) Token: 0x060054C5 RID: 21701 RVA: 0x0017D782 File Offset: 0x0017B982
			public static PerkObject Prestige
			{
				get
				{
					return DefaultPerks.Instance._oneHandedPrestige;
				}
			}

			// Token: 0x17001230 RID: 4656
			// (get) Token: 0x060054C6 RID: 21702 RVA: 0x0017D78E File Offset: 0x0017B98E
			public static PerkObject WayOfTheSword
			{
				get
				{
					return DefaultPerks.Instance._oneHandedWayOfTheSword;
				}
			}

			// Token: 0x17001231 RID: 4657
			// (get) Token: 0x060054C7 RID: 21703 RVA: 0x0017D79A File Offset: 0x0017B99A
			public static PerkObject ChinkInTheArmor
			{
				get
				{
					return DefaultPerks.Instance._oneHandedChinkInTheArmor;
				}
			}
		}

		// Token: 0x02000688 RID: 1672
		public static class TwoHanded
		{
			// Token: 0x17001232 RID: 4658
			// (get) Token: 0x060054C8 RID: 21704 RVA: 0x0017D7A6 File Offset: 0x0017B9A6
			public static PerkObject StrongGrip
			{
				get
				{
					return DefaultPerks.Instance._twoHandedStrongGrip;
				}
			}

			// Token: 0x17001233 RID: 4659
			// (get) Token: 0x060054C9 RID: 21705 RVA: 0x0017D7B2 File Offset: 0x0017B9B2
			public static PerkObject WoodChopper
			{
				get
				{
					return DefaultPerks.Instance._twoHandedWoodChopper;
				}
			}

			// Token: 0x17001234 RID: 4660
			// (get) Token: 0x060054CA RID: 21706 RVA: 0x0017D7BE File Offset: 0x0017B9BE
			public static PerkObject OnTheEdge
			{
				get
				{
					return DefaultPerks.Instance._twoHandedOnTheEdge;
				}
			}

			// Token: 0x17001235 RID: 4661
			// (get) Token: 0x060054CB RID: 21707 RVA: 0x0017D7CA File Offset: 0x0017B9CA
			public static PerkObject HeadBasher
			{
				get
				{
					return DefaultPerks.Instance._twoHandedHeadBasher;
				}
			}

			// Token: 0x17001236 RID: 4662
			// (get) Token: 0x060054CC RID: 21708 RVA: 0x0017D7D6 File Offset: 0x0017B9D6
			public static PerkObject ShowOfStrength
			{
				get
				{
					return DefaultPerks.Instance._twoHandedShowOfStrength;
				}
			}

			// Token: 0x17001237 RID: 4663
			// (get) Token: 0x060054CD RID: 21709 RVA: 0x0017D7E2 File Offset: 0x0017B9E2
			public static PerkObject BaptisedInBlood
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBaptisedInBlood;
				}
			}

			// Token: 0x17001238 RID: 4664
			// (get) Token: 0x060054CE RID: 21710 RVA: 0x0017D7EE File Offset: 0x0017B9EE
			public static PerkObject BeastSlayer
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBeastSlayer;
				}
			}

			// Token: 0x17001239 RID: 4665
			// (get) Token: 0x060054CF RID: 21711 RVA: 0x0017D7FA File Offset: 0x0017B9FA
			public static PerkObject ShieldBreaker
			{
				get
				{
					return DefaultPerks.Instance._twoHandedShieldBreaker;
				}
			}

			// Token: 0x1700123A RID: 4666
			// (get) Token: 0x060054D0 RID: 21712 RVA: 0x0017D806 File Offset: 0x0017BA06
			public static PerkObject Confidence
			{
				get
				{
					return DefaultPerks.Instance._twoHandedConfidence;
				}
			}

			// Token: 0x1700123B RID: 4667
			// (get) Token: 0x060054D1 RID: 21713 RVA: 0x0017D812 File Offset: 0x0017BA12
			public static PerkObject Berserker
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBerserker;
				}
			}

			// Token: 0x1700123C RID: 4668
			// (get) Token: 0x060054D2 RID: 21714 RVA: 0x0017D81E File Offset: 0x0017BA1E
			public static PerkObject ProjectileDeflection
			{
				get
				{
					return DefaultPerks.Instance._twoHandedProjectileDeflection;
				}
			}

			// Token: 0x1700123D RID: 4669
			// (get) Token: 0x060054D3 RID: 21715 RVA: 0x0017D82A File Offset: 0x0017BA2A
			public static PerkObject Terror
			{
				get
				{
					return DefaultPerks.Instance._twoHandedTerror;
				}
			}

			// Token: 0x1700123E RID: 4670
			// (get) Token: 0x060054D4 RID: 21716 RVA: 0x0017D836 File Offset: 0x0017BA36
			public static PerkObject Hope
			{
				get
				{
					return DefaultPerks.Instance._twoHandedHope;
				}
			}

			// Token: 0x1700123F RID: 4671
			// (get) Token: 0x060054D5 RID: 21717 RVA: 0x0017D842 File Offset: 0x0017BA42
			public static PerkObject RecklessCharge
			{
				get
				{
					return DefaultPerks.Instance._twoHandedRecklessCharge;
				}
			}

			// Token: 0x17001240 RID: 4672
			// (get) Token: 0x060054D6 RID: 21718 RVA: 0x0017D84E File Offset: 0x0017BA4E
			public static PerkObject ThickHides
			{
				get
				{
					return DefaultPerks.Instance._twoHandedThickHides;
				}
			}

			// Token: 0x17001241 RID: 4673
			// (get) Token: 0x060054D7 RID: 21719 RVA: 0x0017D85A File Offset: 0x0017BA5A
			public static PerkObject BladeMaster
			{
				get
				{
					return DefaultPerks.Instance._twoHandedBladeMaster;
				}
			}

			// Token: 0x17001242 RID: 4674
			// (get) Token: 0x060054D8 RID: 21720 RVA: 0x0017D866 File Offset: 0x0017BA66
			public static PerkObject Vandal
			{
				get
				{
					return DefaultPerks.Instance._twoHandedVandal;
				}
			}

			// Token: 0x17001243 RID: 4675
			// (get) Token: 0x060054D9 RID: 21721 RVA: 0x0017D872 File Offset: 0x0017BA72
			public static PerkObject WayOfTheGreatAxe
			{
				get
				{
					return DefaultPerks.Instance._twoHandedWayOfTheGreatAxe;
				}
			}
		}

		// Token: 0x02000689 RID: 1673
		public static class Polearm
		{
			// Token: 0x17001244 RID: 4676
			// (get) Token: 0x060054DA RID: 21722 RVA: 0x0017D87E File Offset: 0x0017BA7E
			public static PerkObject Pikeman
			{
				get
				{
					return DefaultPerks.Instance._polearmPikeman;
				}
			}

			// Token: 0x17001245 RID: 4677
			// (get) Token: 0x060054DB RID: 21723 RVA: 0x0017D88A File Offset: 0x0017BA8A
			public static PerkObject Cavalry
			{
				get
				{
					return DefaultPerks.Instance._polearmCavalry;
				}
			}

			// Token: 0x17001246 RID: 4678
			// (get) Token: 0x060054DC RID: 21724 RVA: 0x0017D896 File Offset: 0x0017BA96
			public static PerkObject Braced
			{
				get
				{
					return DefaultPerks.Instance._polearmBraced;
				}
			}

			// Token: 0x17001247 RID: 4679
			// (get) Token: 0x060054DD RID: 21725 RVA: 0x0017D8A2 File Offset: 0x0017BAA2
			public static PerkObject KeepAtBay
			{
				get
				{
					return DefaultPerks.Instance._polearmKeepAtBay;
				}
			}

			// Token: 0x17001248 RID: 4680
			// (get) Token: 0x060054DE RID: 21726 RVA: 0x0017D8AE File Offset: 0x0017BAAE
			public static PerkObject SwiftSwing
			{
				get
				{
					return DefaultPerks.Instance._polearmSwiftSwing;
				}
			}

			// Token: 0x17001249 RID: 4681
			// (get) Token: 0x060054DF RID: 21727 RVA: 0x0017D8BA File Offset: 0x0017BABA
			public static PerkObject CleanThrust
			{
				get
				{
					return DefaultPerks.Instance._polearmCleanThrust;
				}
			}

			// Token: 0x1700124A RID: 4682
			// (get) Token: 0x060054E0 RID: 21728 RVA: 0x0017D8C6 File Offset: 0x0017BAC6
			public static PerkObject Footwork
			{
				get
				{
					return DefaultPerks.Instance._polearmFootwork;
				}
			}

			// Token: 0x1700124B RID: 4683
			// (get) Token: 0x060054E1 RID: 21729 RVA: 0x0017D8D2 File Offset: 0x0017BAD2
			public static PerkObject HardKnock
			{
				get
				{
					return DefaultPerks.Instance._polearmHardKnock;
				}
			}

			// Token: 0x1700124C RID: 4684
			// (get) Token: 0x060054E2 RID: 21730 RVA: 0x0017D8DE File Offset: 0x0017BADE
			public static PerkObject SteedKiller
			{
				get
				{
					return DefaultPerks.Instance._polearmSteedKiller;
				}
			}

			// Token: 0x1700124D RID: 4685
			// (get) Token: 0x060054E3 RID: 21731 RVA: 0x0017D8EA File Offset: 0x0017BAEA
			public static PerkObject Lancer
			{
				get
				{
					return DefaultPerks.Instance._polearmLancer;
				}
			}

			// Token: 0x1700124E RID: 4686
			// (get) Token: 0x060054E4 RID: 21732 RVA: 0x0017D8F6 File Offset: 0x0017BAF6
			public static PerkObject Skewer
			{
				get
				{
					return DefaultPerks.Instance._polearmSkewer;
				}
			}

			// Token: 0x1700124F RID: 4687
			// (get) Token: 0x060054E5 RID: 21733 RVA: 0x0017D902 File Offset: 0x0017BB02
			public static PerkObject Guards
			{
				get
				{
					return DefaultPerks.Instance._polearmGuards;
				}
			}

			// Token: 0x17001250 RID: 4688
			// (get) Token: 0x060054E6 RID: 21734 RVA: 0x0017D90E File Offset: 0x0017BB0E
			public static PerkObject StandardBearer
			{
				get
				{
					return DefaultPerks.Instance._polearmStandardBearer;
				}
			}

			// Token: 0x17001251 RID: 4689
			// (get) Token: 0x060054E7 RID: 21735 RVA: 0x0017D91A File Offset: 0x0017BB1A
			public static PerkObject Phalanx
			{
				get
				{
					return DefaultPerks.Instance._polearmPhalanx;
				}
			}

			// Token: 0x17001252 RID: 4690
			// (get) Token: 0x060054E8 RID: 21736 RVA: 0x0017D926 File Offset: 0x0017BB26
			public static PerkObject HardyFrontline
			{
				get
				{
					return DefaultPerks.Instance._polearmHardyFrontline;
				}
			}

			// Token: 0x17001253 RID: 4691
			// (get) Token: 0x060054E9 RID: 21737 RVA: 0x0017D932 File Offset: 0x0017BB32
			public static PerkObject Drills
			{
				get
				{
					return DefaultPerks.Instance._polearmDrills;
				}
			}

			// Token: 0x17001254 RID: 4692
			// (get) Token: 0x060054EA RID: 21738 RVA: 0x0017D93E File Offset: 0x0017BB3E
			public static PerkObject SureFooted
			{
				get
				{
					return DefaultPerks.Instance._polearmSureFooted;
				}
			}

			// Token: 0x17001255 RID: 4693
			// (get) Token: 0x060054EB RID: 21739 RVA: 0x0017D94A File Offset: 0x0017BB4A
			public static PerkObject UnstoppableForce
			{
				get
				{
					return DefaultPerks.Instance._polearmUnstoppableForce;
				}
			}

			// Token: 0x17001256 RID: 4694
			// (get) Token: 0x060054EC RID: 21740 RVA: 0x0017D956 File Offset: 0x0017BB56
			public static PerkObject CounterWeight
			{
				get
				{
					return DefaultPerks.Instance._polearmCounterweight;
				}
			}

			// Token: 0x17001257 RID: 4695
			// (get) Token: 0x060054ED RID: 21741 RVA: 0x0017D962 File Offset: 0x0017BB62
			public static PerkObject SharpenTheTip
			{
				get
				{
					return DefaultPerks.Instance._polearmSharpenTheTip;
				}
			}

			// Token: 0x17001258 RID: 4696
			// (get) Token: 0x060054EE RID: 21742 RVA: 0x0017D96E File Offset: 0x0017BB6E
			public static PerkObject WayOfTheSpear
			{
				get
				{
					return DefaultPerks.Instance._polearmWayOfTheSpear;
				}
			}
		}

		// Token: 0x0200068A RID: 1674
		public static class Bow
		{
			// Token: 0x17001259 RID: 4697
			// (get) Token: 0x060054EF RID: 21743 RVA: 0x0017D97A File Offset: 0x0017BB7A
			public static PerkObject BowControl
			{
				get
				{
					return DefaultPerks.Instance._bowBowControl;
				}
			}

			// Token: 0x1700125A RID: 4698
			// (get) Token: 0x060054F0 RID: 21744 RVA: 0x0017D986 File Offset: 0x0017BB86
			public static PerkObject DeadAim
			{
				get
				{
					return DefaultPerks.Instance._bowDeadAim;
				}
			}

			// Token: 0x1700125B RID: 4699
			// (get) Token: 0x060054F1 RID: 21745 RVA: 0x0017D992 File Offset: 0x0017BB92
			public static PerkObject Bodkin
			{
				get
				{
					return DefaultPerks.Instance._bowBodkin;
				}
			}

			// Token: 0x1700125C RID: 4700
			// (get) Token: 0x060054F2 RID: 21746 RVA: 0x0017D99E File Offset: 0x0017BB9E
			public static PerkObject RangersSwiftness
			{
				get
				{
					return DefaultPerks.Instance._bowRangersSwiftness;
				}
			}

			// Token: 0x1700125D RID: 4701
			// (get) Token: 0x060054F3 RID: 21747 RVA: 0x0017D9AA File Offset: 0x0017BBAA
			public static PerkObject RapidFire
			{
				get
				{
					return DefaultPerks.Instance._bowRapidFire;
				}
			}

			// Token: 0x1700125E RID: 4702
			// (get) Token: 0x060054F4 RID: 21748 RVA: 0x0017D9B6 File Offset: 0x0017BBB6
			public static PerkObject QuickAdjustments
			{
				get
				{
					return DefaultPerks.Instance._bowQuickAdjustments;
				}
			}

			// Token: 0x1700125F RID: 4703
			// (get) Token: 0x060054F5 RID: 21749 RVA: 0x0017D9C2 File Offset: 0x0017BBC2
			public static PerkObject MerryMen
			{
				get
				{
					return DefaultPerks.Instance._bowMerryMen;
				}
			}

			// Token: 0x17001260 RID: 4704
			// (get) Token: 0x060054F6 RID: 21750 RVA: 0x0017D9CE File Offset: 0x0017BBCE
			public static PerkObject MountedArchery
			{
				get
				{
					return DefaultPerks.Instance._bowMountedArchery;
				}
			}

			// Token: 0x17001261 RID: 4705
			// (get) Token: 0x060054F7 RID: 21751 RVA: 0x0017D9DA File Offset: 0x0017BBDA
			public static PerkObject Trainer
			{
				get
				{
					return DefaultPerks.Instance._bowTrainer;
				}
			}

			// Token: 0x17001262 RID: 4706
			// (get) Token: 0x060054F8 RID: 21752 RVA: 0x0017D9E6 File Offset: 0x0017BBE6
			public static PerkObject StrongBows
			{
				get
				{
					return DefaultPerks.Instance._bowStrongBows;
				}
			}

			// Token: 0x17001263 RID: 4707
			// (get) Token: 0x060054F9 RID: 21753 RVA: 0x0017D9F2 File Offset: 0x0017BBF2
			public static PerkObject Discipline
			{
				get
				{
					return DefaultPerks.Instance._bowDiscipline;
				}
			}

			// Token: 0x17001264 RID: 4708
			// (get) Token: 0x060054FA RID: 21754 RVA: 0x0017D9FE File Offset: 0x0017BBFE
			public static PerkObject HunterClan
			{
				get
				{
					return DefaultPerks.Instance._bowHunterClan;
				}
			}

			// Token: 0x17001265 RID: 4709
			// (get) Token: 0x060054FB RID: 21755 RVA: 0x0017DA0A File Offset: 0x0017BC0A
			public static PerkObject SkirmishPhaseMaster
			{
				get
				{
					return DefaultPerks.Instance._bowSkirmishPhaseMaster;
				}
			}

			// Token: 0x17001266 RID: 4710
			// (get) Token: 0x060054FC RID: 21756 RVA: 0x0017DA16 File Offset: 0x0017BC16
			public static PerkObject EagleEye
			{
				get
				{
					return DefaultPerks.Instance._bowEagleEye;
				}
			}

			// Token: 0x17001267 RID: 4711
			// (get) Token: 0x060054FD RID: 21757 RVA: 0x0017DA22 File Offset: 0x0017BC22
			public static PerkObject BullsEye
			{
				get
				{
					return DefaultPerks.Instance._bowBullsEye;
				}
			}

			// Token: 0x17001268 RID: 4712
			// (get) Token: 0x060054FE RID: 21758 RVA: 0x0017DA2E File Offset: 0x0017BC2E
			public static PerkObject RenownedArcher
			{
				get
				{
					return DefaultPerks.Instance._bowRenownedArcher;
				}
			}

			// Token: 0x17001269 RID: 4713
			// (get) Token: 0x060054FF RID: 21759 RVA: 0x0017DA3A File Offset: 0x0017BC3A
			public static PerkObject HorseMaster
			{
				get
				{
					return DefaultPerks.Instance._bowHorseMaster;
				}
			}

			// Token: 0x1700126A RID: 4714
			// (get) Token: 0x06005500 RID: 21760 RVA: 0x0017DA46 File Offset: 0x0017BC46
			public static PerkObject DeepQuivers
			{
				get
				{
					return DefaultPerks.Instance._bowDeepQuivers;
				}
			}

			// Token: 0x1700126B RID: 4715
			// (get) Token: 0x06005501 RID: 21761 RVA: 0x0017DA52 File Offset: 0x0017BC52
			public static PerkObject QuickDraw
			{
				get
				{
					return DefaultPerks.Instance._bowQuickDraw;
				}
			}

			// Token: 0x1700126C RID: 4716
			// (get) Token: 0x06005502 RID: 21762 RVA: 0x0017DA5E File Offset: 0x0017BC5E
			public static PerkObject NockingPoint
			{
				get
				{
					return DefaultPerks.Instance._bowNockingPoint;
				}
			}

			// Token: 0x1700126D RID: 4717
			// (get) Token: 0x06005503 RID: 21763 RVA: 0x0017DA6A File Offset: 0x0017BC6A
			public static PerkObject Deadshot
			{
				get
				{
					return DefaultPerks.Instance._bowDeadshot;
				}
			}
		}

		// Token: 0x0200068B RID: 1675
		public static class Crossbow
		{
			// Token: 0x1700126E RID: 4718
			// (get) Token: 0x06005504 RID: 21764 RVA: 0x0017DA76 File Offset: 0x0017BC76
			public static PerkObject Piercer
			{
				get
				{
					return DefaultPerks.Instance._crossbowPiercer;
				}
			}

			// Token: 0x1700126F RID: 4719
			// (get) Token: 0x06005505 RID: 21765 RVA: 0x0017DA82 File Offset: 0x0017BC82
			public static PerkObject Marksmen
			{
				get
				{
					return DefaultPerks.Instance._crossbowMarksmen;
				}
			}

			// Token: 0x17001270 RID: 4720
			// (get) Token: 0x06005506 RID: 21766 RVA: 0x0017DA8E File Offset: 0x0017BC8E
			public static PerkObject Unhorser
			{
				get
				{
					return DefaultPerks.Instance._crossbowUnhorser;
				}
			}

			// Token: 0x17001271 RID: 4721
			// (get) Token: 0x06005507 RID: 21767 RVA: 0x0017DA9A File Offset: 0x0017BC9A
			public static PerkObject WindWinder
			{
				get
				{
					return DefaultPerks.Instance._crossbowWindWinder;
				}
			}

			// Token: 0x17001272 RID: 4722
			// (get) Token: 0x06005508 RID: 21768 RVA: 0x0017DAA6 File Offset: 0x0017BCA6
			public static PerkObject DonkeysSwiftness
			{
				get
				{
					return DefaultPerks.Instance._crossbowDonkeysSwiftness;
				}
			}

			// Token: 0x17001273 RID: 4723
			// (get) Token: 0x06005509 RID: 21769 RVA: 0x0017DAB2 File Offset: 0x0017BCB2
			public static PerkObject Sheriff
			{
				get
				{
					return DefaultPerks.Instance._crossbowSheriff;
				}
			}

			// Token: 0x17001274 RID: 4724
			// (get) Token: 0x0600550A RID: 21770 RVA: 0x0017DABE File Offset: 0x0017BCBE
			public static PerkObject PeasantLeader
			{
				get
				{
					return DefaultPerks.Instance._crossbowPeasantLeader;
				}
			}

			// Token: 0x17001275 RID: 4725
			// (get) Token: 0x0600550B RID: 21771 RVA: 0x0017DACA File Offset: 0x0017BCCA
			public static PerkObject RenownMarksmen
			{
				get
				{
					return DefaultPerks.Instance._crossbowRenownMarksmen;
				}
			}

			// Token: 0x17001276 RID: 4726
			// (get) Token: 0x0600550C RID: 21772 RVA: 0x0017DAD6 File Offset: 0x0017BCD6
			public static PerkObject Fletcher
			{
				get
				{
					return DefaultPerks.Instance._crossbowFletcher;
				}
			}

			// Token: 0x17001277 RID: 4727
			// (get) Token: 0x0600550D RID: 21773 RVA: 0x0017DAE2 File Offset: 0x0017BCE2
			public static PerkObject Puncture
			{
				get
				{
					return DefaultPerks.Instance._crossbowPuncture;
				}
			}

			// Token: 0x17001278 RID: 4728
			// (get) Token: 0x0600550E RID: 21774 RVA: 0x0017DAEE File Offset: 0x0017BCEE
			public static PerkObject LooseAndMove
			{
				get
				{
					return DefaultPerks.Instance._crossbowLooseAndMove;
				}
			}

			// Token: 0x17001279 RID: 4729
			// (get) Token: 0x0600550F RID: 21775 RVA: 0x0017DAFA File Offset: 0x0017BCFA
			public static PerkObject DeftHands
			{
				get
				{
					return DefaultPerks.Instance._crossbowDeftHands;
				}
			}

			// Token: 0x1700127A RID: 4730
			// (get) Token: 0x06005510 RID: 21776 RVA: 0x0017DB06 File Offset: 0x0017BD06
			public static PerkObject CounterFire
			{
				get
				{
					return DefaultPerks.Instance._crossbowCounterFire;
				}
			}

			// Token: 0x1700127B RID: 4731
			// (get) Token: 0x06005511 RID: 21777 RVA: 0x0017DB12 File Offset: 0x0017BD12
			public static PerkObject MountedCrossbowman
			{
				get
				{
					return DefaultPerks.Instance._crossbowMountedCrossbowman;
				}
			}

			// Token: 0x1700127C RID: 4732
			// (get) Token: 0x06005512 RID: 21778 RVA: 0x0017DB1E File Offset: 0x0017BD1E
			public static PerkObject Steady
			{
				get
				{
					return DefaultPerks.Instance._crossbowSteady;
				}
			}

			// Token: 0x1700127D RID: 4733
			// (get) Token: 0x06005513 RID: 21779 RVA: 0x0017DB2A File Offset: 0x0017BD2A
			public static PerkObject LongShots
			{
				get
				{
					return DefaultPerks.Instance._crossbowLongShots;
				}
			}

			// Token: 0x1700127E RID: 4734
			// (get) Token: 0x06005514 RID: 21780 RVA: 0x0017DB36 File Offset: 0x0017BD36
			public static PerkObject HammerBolts
			{
				get
				{
					return DefaultPerks.Instance._crossbowHammerBolts;
				}
			}

			// Token: 0x1700127F RID: 4735
			// (get) Token: 0x06005515 RID: 21781 RVA: 0x0017DB42 File Offset: 0x0017BD42
			public static PerkObject Pavise
			{
				get
				{
					return DefaultPerks.Instance._crossbowPavise;
				}
			}

			// Token: 0x17001280 RID: 4736
			// (get) Token: 0x06005516 RID: 21782 RVA: 0x0017DB4E File Offset: 0x0017BD4E
			public static PerkObject Terror
			{
				get
				{
					return DefaultPerks.Instance._crossbowTerror;
				}
			}

			// Token: 0x17001281 RID: 4737
			// (get) Token: 0x06005517 RID: 21783 RVA: 0x0017DB5A File Offset: 0x0017BD5A
			public static PerkObject PickedShots
			{
				get
				{
					return DefaultPerks.Instance._crossbowPickedShots;
				}
			}

			// Token: 0x17001282 RID: 4738
			// (get) Token: 0x06005518 RID: 21784 RVA: 0x0017DB66 File Offset: 0x0017BD66
			public static PerkObject MightyPull
			{
				get
				{
					return DefaultPerks.Instance._crossbowMightyPull;
				}
			}
		}

		// Token: 0x0200068C RID: 1676
		public static class Throwing
		{
			// Token: 0x17001283 RID: 4739
			// (get) Token: 0x06005519 RID: 21785 RVA: 0x0017DB72 File Offset: 0x0017BD72
			public static PerkObject QuickDraw
			{
				get
				{
					return DefaultPerks.Instance._throwingQuickDraw;
				}
			}

			// Token: 0x17001284 RID: 4740
			// (get) Token: 0x0600551A RID: 21786 RVA: 0x0017DB7E File Offset: 0x0017BD7E
			public static PerkObject ShieldBreaker
			{
				get
				{
					return DefaultPerks.Instance._throwingShieldBreaker;
				}
			}

			// Token: 0x17001285 RID: 4741
			// (get) Token: 0x0600551B RID: 21787 RVA: 0x0017DB8A File Offset: 0x0017BD8A
			public static PerkObject Hunter
			{
				get
				{
					return DefaultPerks.Instance._throwingHunter;
				}
			}

			// Token: 0x17001286 RID: 4742
			// (get) Token: 0x0600551C RID: 21788 RVA: 0x0017DB96 File Offset: 0x0017BD96
			public static PerkObject FlexibleFighter
			{
				get
				{
					return DefaultPerks.Instance._throwingFlexibleFighter;
				}
			}

			// Token: 0x17001287 RID: 4743
			// (get) Token: 0x0600551D RID: 21789 RVA: 0x0017DBA2 File Offset: 0x0017BDA2
			public static PerkObject MountedSkirmisher
			{
				get
				{
					return DefaultPerks.Instance._throwingMountedSkirmisher;
				}
			}

			// Token: 0x17001288 RID: 4744
			// (get) Token: 0x0600551E RID: 21790 RVA: 0x0017DBAE File Offset: 0x0017BDAE
			public static PerkObject PerfectTechnique
			{
				get
				{
					return DefaultPerks.Instance._throwingPerfectTechnique;
				}
			}

			// Token: 0x17001289 RID: 4745
			// (get) Token: 0x0600551F RID: 21791 RVA: 0x0017DBBA File Offset: 0x0017BDBA
			public static PerkObject RunningThrow
			{
				get
				{
					return DefaultPerks.Instance._throwingRunningThrow;
				}
			}

			// Token: 0x1700128A RID: 4746
			// (get) Token: 0x06005520 RID: 21792 RVA: 0x0017DBC6 File Offset: 0x0017BDC6
			public static PerkObject KnockOff
			{
				get
				{
					return DefaultPerks.Instance._throwingKnockOff;
				}
			}

			// Token: 0x1700128B RID: 4747
			// (get) Token: 0x06005521 RID: 21793 RVA: 0x0017DBD2 File Offset: 0x0017BDD2
			public static PerkObject WellPrepared
			{
				get
				{
					return DefaultPerks.Instance._throwingWellPrepared;
				}
			}

			// Token: 0x1700128C RID: 4748
			// (get) Token: 0x06005522 RID: 21794 RVA: 0x0017DBDE File Offset: 0x0017BDDE
			public static PerkObject Skirmisher
			{
				get
				{
					return DefaultPerks.Instance._throwingSkirmisher;
				}
			}

			// Token: 0x1700128D RID: 4749
			// (get) Token: 0x06005523 RID: 21795 RVA: 0x0017DBEA File Offset: 0x0017BDEA
			public static PerkObject Focus
			{
				get
				{
					return DefaultPerks.Instance._throwingFocus;
				}
			}

			// Token: 0x1700128E RID: 4750
			// (get) Token: 0x06005524 RID: 21796 RVA: 0x0017DBF6 File Offset: 0x0017BDF6
			public static PerkObject LastHit
			{
				get
				{
					return DefaultPerks.Instance._throwingLastHit;
				}
			}

			// Token: 0x1700128F RID: 4751
			// (get) Token: 0x06005525 RID: 21797 RVA: 0x0017DC02 File Offset: 0x0017BE02
			public static PerkObject HeadHunter
			{
				get
				{
					return DefaultPerks.Instance._throwingHeadHunter;
				}
			}

			// Token: 0x17001290 RID: 4752
			// (get) Token: 0x06005526 RID: 21798 RVA: 0x0017DC0E File Offset: 0x0017BE0E
			public static PerkObject ThrowingCompetitions
			{
				get
				{
					return DefaultPerks.Instance._throwingThrowingCompetitions;
				}
			}

			// Token: 0x17001291 RID: 4753
			// (get) Token: 0x06005527 RID: 21799 RVA: 0x0017DC1A File Offset: 0x0017BE1A
			public static PerkObject Saddlebags
			{
				get
				{
					return DefaultPerks.Instance._throwingSaddlebags;
				}
			}

			// Token: 0x17001292 RID: 4754
			// (get) Token: 0x06005528 RID: 21800 RVA: 0x0017DC26 File Offset: 0x0017BE26
			public static PerkObject Splinters
			{
				get
				{
					return DefaultPerks.Instance._throwingSplinters;
				}
			}

			// Token: 0x17001293 RID: 4755
			// (get) Token: 0x06005529 RID: 21801 RVA: 0x0017DC32 File Offset: 0x0017BE32
			public static PerkObject Resourceful
			{
				get
				{
					return DefaultPerks.Instance._throwingResourceful;
				}
			}

			// Token: 0x17001294 RID: 4756
			// (get) Token: 0x0600552A RID: 21802 RVA: 0x0017DC3E File Offset: 0x0017BE3E
			public static PerkObject LongReach
			{
				get
				{
					return DefaultPerks.Instance._throwingLongReach;
				}
			}

			// Token: 0x17001295 RID: 4757
			// (get) Token: 0x0600552B RID: 21803 RVA: 0x0017DC4A File Offset: 0x0017BE4A
			public static PerkObject WeakSpot
			{
				get
				{
					return DefaultPerks.Instance._throwingWeakSpot;
				}
			}

			// Token: 0x17001296 RID: 4758
			// (get) Token: 0x0600552C RID: 21804 RVA: 0x0017DC56 File Offset: 0x0017BE56
			public static PerkObject Impale
			{
				get
				{
					return DefaultPerks.Instance._throwingImpale;
				}
			}

			// Token: 0x17001297 RID: 4759
			// (get) Token: 0x0600552D RID: 21805 RVA: 0x0017DC62 File Offset: 0x0017BE62
			public static PerkObject UnstoppableForce
			{
				get
				{
					return DefaultPerks.Instance._throwingUnstoppableForce;
				}
			}
		}

		// Token: 0x0200068D RID: 1677
		public static class Riding
		{
			// Token: 0x17001298 RID: 4760
			// (get) Token: 0x0600552E RID: 21806 RVA: 0x0017DC6E File Offset: 0x0017BE6E
			public static PerkObject FullSpeed
			{
				get
				{
					return DefaultPerks.Instance._ridingFullSpeed;
				}
			}

			// Token: 0x17001299 RID: 4761
			// (get) Token: 0x0600552F RID: 21807 RVA: 0x0017DC7A File Offset: 0x0017BE7A
			public static PerkObject NimbleSteed
			{
				get
				{
					return DefaultPerks.Instance._ridingNimbleSteed;
				}
			}

			// Token: 0x1700129A RID: 4762
			// (get) Token: 0x06005530 RID: 21808 RVA: 0x0017DC86 File Offset: 0x0017BE86
			public static PerkObject WellStraped
			{
				get
				{
					return DefaultPerks.Instance._ridingWellStraped;
				}
			}

			// Token: 0x1700129B RID: 4763
			// (get) Token: 0x06005531 RID: 21809 RVA: 0x0017DC92 File Offset: 0x0017BE92
			public static PerkObject Veterinary
			{
				get
				{
					return DefaultPerks.Instance._ridingVeterinary;
				}
			}

			// Token: 0x1700129C RID: 4764
			// (get) Token: 0x06005532 RID: 21810 RVA: 0x0017DC9E File Offset: 0x0017BE9E
			public static PerkObject NomadicTraditions
			{
				get
				{
					return DefaultPerks.Instance._ridingNomadicTraditions;
				}
			}

			// Token: 0x1700129D RID: 4765
			// (get) Token: 0x06005533 RID: 21811 RVA: 0x0017DCAA File Offset: 0x0017BEAA
			public static PerkObject DeeperSacks
			{
				get
				{
					return DefaultPerks.Instance._ridingDeeperSacks;
				}
			}

			// Token: 0x1700129E RID: 4766
			// (get) Token: 0x06005534 RID: 21812 RVA: 0x0017DCB6 File Offset: 0x0017BEB6
			public static PerkObject Sagittarius
			{
				get
				{
					return DefaultPerks.Instance._ridingSagittarius;
				}
			}

			// Token: 0x1700129F RID: 4767
			// (get) Token: 0x06005535 RID: 21813 RVA: 0x0017DCC2 File Offset: 0x0017BEC2
			public static PerkObject SweepingWind
			{
				get
				{
					return DefaultPerks.Instance._ridingSweepingWind;
				}
			}

			// Token: 0x170012A0 RID: 4768
			// (get) Token: 0x06005536 RID: 21814 RVA: 0x0017DCCE File Offset: 0x0017BECE
			public static PerkObject ReliefForce
			{
				get
				{
					return DefaultPerks.Instance._ridingReliefForce;
				}
			}

			// Token: 0x170012A1 RID: 4769
			// (get) Token: 0x06005537 RID: 21815 RVA: 0x0017DCDA File Offset: 0x0017BEDA
			public static PerkObject MountedWarrior
			{
				get
				{
					return DefaultPerks.Instance._ridingMountedWarrior;
				}
			}

			// Token: 0x170012A2 RID: 4770
			// (get) Token: 0x06005538 RID: 21816 RVA: 0x0017DCE6 File Offset: 0x0017BEE6
			public static PerkObject HorseArcher
			{
				get
				{
					return DefaultPerks.Instance._ridingHorseArcher;
				}
			}

			// Token: 0x170012A3 RID: 4771
			// (get) Token: 0x06005539 RID: 21817 RVA: 0x0017DCF2 File Offset: 0x0017BEF2
			public static PerkObject Shepherd
			{
				get
				{
					return DefaultPerks.Instance._ridingShepherd;
				}
			}

			// Token: 0x170012A4 RID: 4772
			// (get) Token: 0x0600553A RID: 21818 RVA: 0x0017DCFE File Offset: 0x0017BEFE
			public static PerkObject Breeder
			{
				get
				{
					return DefaultPerks.Instance._ridingBreeder;
				}
			}

			// Token: 0x170012A5 RID: 4773
			// (get) Token: 0x0600553B RID: 21819 RVA: 0x0017DD0A File Offset: 0x0017BF0A
			public static PerkObject ThunderousCharge
			{
				get
				{
					return DefaultPerks.Instance._ridingThunderousCharge;
				}
			}

			// Token: 0x170012A6 RID: 4774
			// (get) Token: 0x0600553C RID: 21820 RVA: 0x0017DD16 File Offset: 0x0017BF16
			public static PerkObject AnnoyingBuzz
			{
				get
				{
					return DefaultPerks.Instance._ridingAnnoyingBuzz;
				}
			}

			// Token: 0x170012A7 RID: 4775
			// (get) Token: 0x0600553D RID: 21821 RVA: 0x0017DD22 File Offset: 0x0017BF22
			public static PerkObject MountedPatrols
			{
				get
				{
					return DefaultPerks.Instance._ridingMountedPatrols;
				}
			}

			// Token: 0x170012A8 RID: 4776
			// (get) Token: 0x0600553E RID: 21822 RVA: 0x0017DD2E File Offset: 0x0017BF2E
			public static PerkObject CavalryTactics
			{
				get
				{
					return DefaultPerks.Instance._ridingCavalryTactics;
				}
			}

			// Token: 0x170012A9 RID: 4777
			// (get) Token: 0x0600553F RID: 21823 RVA: 0x0017DD3A File Offset: 0x0017BF3A
			public static PerkObject DauntlessSteed
			{
				get
				{
					return DefaultPerks.Instance._ridingDauntlessSteed;
				}
			}

			// Token: 0x170012AA RID: 4778
			// (get) Token: 0x06005540 RID: 21824 RVA: 0x0017DD46 File Offset: 0x0017BF46
			public static PerkObject ToughSteed
			{
				get
				{
					return DefaultPerks.Instance._ridingToughSteed;
				}
			}

			// Token: 0x170012AB RID: 4779
			// (get) Token: 0x06005541 RID: 21825 RVA: 0x0017DD52 File Offset: 0x0017BF52
			public static PerkObject TheWayOfTheSaddle
			{
				get
				{
					return DefaultPerks.Instance._ridingTheWayOfTheSaddle;
				}
			}
		}

		// Token: 0x0200068E RID: 1678
		public static class Athletics
		{
			// Token: 0x170012AC RID: 4780
			// (get) Token: 0x06005542 RID: 21826 RVA: 0x0017DD5E File Offset: 0x0017BF5E
			public static PerkObject MorningExercise
			{
				get
				{
					return DefaultPerks.Instance._athleticsMorningExercise;
				}
			}

			// Token: 0x170012AD RID: 4781
			// (get) Token: 0x06005543 RID: 21827 RVA: 0x0017DD6A File Offset: 0x0017BF6A
			public static PerkObject WellBuilt
			{
				get
				{
					return DefaultPerks.Instance._athleticsWellBuilt;
				}
			}

			// Token: 0x170012AE RID: 4782
			// (get) Token: 0x06005544 RID: 21828 RVA: 0x0017DD76 File Offset: 0x0017BF76
			public static PerkObject Fury
			{
				get
				{
					return DefaultPerks.Instance._athleticsFury;
				}
			}

			// Token: 0x170012AF RID: 4783
			// (get) Token: 0x06005545 RID: 21829 RVA: 0x0017DD82 File Offset: 0x0017BF82
			public static PerkObject FormFittingArmor
			{
				get
				{
					return DefaultPerks.Instance._athleticsFormFittingArmor;
				}
			}

			// Token: 0x170012B0 RID: 4784
			// (get) Token: 0x06005546 RID: 21830 RVA: 0x0017DD8E File Offset: 0x0017BF8E
			public static PerkObject ImposingStature
			{
				get
				{
					return DefaultPerks.Instance._athleticsImposingStature;
				}
			}

			// Token: 0x170012B1 RID: 4785
			// (get) Token: 0x06005547 RID: 21831 RVA: 0x0017DD9A File Offset: 0x0017BF9A
			public static PerkObject Stamina
			{
				get
				{
					return DefaultPerks.Instance._athleticsStamina;
				}
			}

			// Token: 0x170012B2 RID: 4786
			// (get) Token: 0x06005548 RID: 21832 RVA: 0x0017DDA6 File Offset: 0x0017BFA6
			public static PerkObject Sprint
			{
				get
				{
					return DefaultPerks.Instance._athleticsSprint;
				}
			}

			// Token: 0x170012B3 RID: 4787
			// (get) Token: 0x06005549 RID: 21833 RVA: 0x0017DDB2 File Offset: 0x0017BFB2
			public static PerkObject Powerful
			{
				get
				{
					return DefaultPerks.Instance._athleticsPowerful;
				}
			}

			// Token: 0x170012B4 RID: 4788
			// (get) Token: 0x0600554A RID: 21834 RVA: 0x0017DDBE File Offset: 0x0017BFBE
			public static PerkObject SurgingBlow
			{
				get
				{
					return DefaultPerks.Instance._athleticsSurgingBlow;
				}
			}

			// Token: 0x170012B5 RID: 4789
			// (get) Token: 0x0600554B RID: 21835 RVA: 0x0017DDCA File Offset: 0x0017BFCA
			public static PerkObject Braced
			{
				get
				{
					return DefaultPerks.Instance._athleticsBraced;
				}
			}

			// Token: 0x170012B6 RID: 4790
			// (get) Token: 0x0600554C RID: 21836 RVA: 0x0017DDD6 File Offset: 0x0017BFD6
			public static PerkObject WalkItOff
			{
				get
				{
					return DefaultPerks.Instance._athleticsWalkItOff;
				}
			}

			// Token: 0x170012B7 RID: 4791
			// (get) Token: 0x0600554D RID: 21837 RVA: 0x0017DDE2 File Offset: 0x0017BFE2
			public static PerkObject AGoodDaysRest
			{
				get
				{
					return DefaultPerks.Instance._athleticsAGoodDaysRest;
				}
			}

			// Token: 0x170012B8 RID: 4792
			// (get) Token: 0x0600554E RID: 21838 RVA: 0x0017DDEE File Offset: 0x0017BFEE
			public static PerkObject Durable
			{
				get
				{
					return DefaultPerks.Instance._athleticsDurable;
				}
			}

			// Token: 0x170012B9 RID: 4793
			// (get) Token: 0x0600554F RID: 21839 RVA: 0x0017DDFA File Offset: 0x0017BFFA
			public static PerkObject Energetic
			{
				get
				{
					return DefaultPerks.Instance._athleticsEnergetic;
				}
			}

			// Token: 0x170012BA RID: 4794
			// (get) Token: 0x06005550 RID: 21840 RVA: 0x0017DE06 File Offset: 0x0017C006
			public static PerkObject Steady
			{
				get
				{
					return DefaultPerks.Instance._athleticsSteady;
				}
			}

			// Token: 0x170012BB RID: 4795
			// (get) Token: 0x06005551 RID: 21841 RVA: 0x0017DE12 File Offset: 0x0017C012
			public static PerkObject Strong
			{
				get
				{
					return DefaultPerks.Instance._athleticsStrong;
				}
			}

			// Token: 0x170012BC RID: 4796
			// (get) Token: 0x06005552 RID: 21842 RVA: 0x0017DE1E File Offset: 0x0017C01E
			public static PerkObject StrongLegs
			{
				get
				{
					return DefaultPerks.Instance._athleticsStrongLegs;
				}
			}

			// Token: 0x170012BD RID: 4797
			// (get) Token: 0x06005553 RID: 21843 RVA: 0x0017DE2A File Offset: 0x0017C02A
			public static PerkObject StrongArms
			{
				get
				{
					return DefaultPerks.Instance._athleticsStrongArms;
				}
			}

			// Token: 0x170012BE RID: 4798
			// (get) Token: 0x06005554 RID: 21844 RVA: 0x0017DE36 File Offset: 0x0017C036
			public static PerkObject Spartan
			{
				get
				{
					return DefaultPerks.Instance._athleticsSpartan;
				}
			}

			// Token: 0x170012BF RID: 4799
			// (get) Token: 0x06005555 RID: 21845 RVA: 0x0017DE42 File Offset: 0x0017C042
			public static PerkObject IgnorePain
			{
				get
				{
					return DefaultPerks.Instance._athleticsIgnorePain;
				}
			}

			// Token: 0x170012C0 RID: 4800
			// (get) Token: 0x06005556 RID: 21846 RVA: 0x0017DE4E File Offset: 0x0017C04E
			public static PerkObject MightyBlow
			{
				get
				{
					return DefaultPerks.Instance._athleticsMightyBlow;
				}
			}
		}

		// Token: 0x0200068F RID: 1679
		public static class Crafting
		{
			// Token: 0x170012C1 RID: 4801
			// (get) Token: 0x06005557 RID: 21847 RVA: 0x0017DE5A File Offset: 0x0017C05A
			public static PerkObject IronMaker
			{
				get
				{
					return DefaultPerks.Instance._craftingIronMaker;
				}
			}

			// Token: 0x170012C2 RID: 4802
			// (get) Token: 0x06005558 RID: 21848 RVA: 0x0017DE66 File Offset: 0x0017C066
			public static PerkObject CharcoalMaker
			{
				get
				{
					return DefaultPerks.Instance._craftingCharcoalMaker;
				}
			}

			// Token: 0x170012C3 RID: 4803
			// (get) Token: 0x06005559 RID: 21849 RVA: 0x0017DE72 File Offset: 0x0017C072
			public static PerkObject SteelMaker
			{
				get
				{
					return DefaultPerks.Instance._craftingSteelMaker;
				}
			}

			// Token: 0x170012C4 RID: 4804
			// (get) Token: 0x0600555A RID: 21850 RVA: 0x0017DE7E File Offset: 0x0017C07E
			public static PerkObject SteelMaker2
			{
				get
				{
					return DefaultPerks.Instance._craftingSteelMaker2;
				}
			}

			// Token: 0x170012C5 RID: 4805
			// (get) Token: 0x0600555B RID: 21851 RVA: 0x0017DE8A File Offset: 0x0017C08A
			public static PerkObject SteelMaker3
			{
				get
				{
					return DefaultPerks.Instance._craftingSteelMaker3;
				}
			}

			// Token: 0x170012C6 RID: 4806
			// (get) Token: 0x0600555C RID: 21852 RVA: 0x0017DE96 File Offset: 0x0017C096
			public static PerkObject CuriousSmelter
			{
				get
				{
					return DefaultPerks.Instance._craftingCuriousSmelter;
				}
			}

			// Token: 0x170012C7 RID: 4807
			// (get) Token: 0x0600555D RID: 21853 RVA: 0x0017DEA2 File Offset: 0x0017C0A2
			public static PerkObject CuriousSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingCuriousSmith;
				}
			}

			// Token: 0x170012C8 RID: 4808
			// (get) Token: 0x0600555E RID: 21854 RVA: 0x0017DEAE File Offset: 0x0017C0AE
			public static PerkObject PracticalRefiner
			{
				get
				{
					return DefaultPerks.Instance._craftingPracticalRefiner;
				}
			}

			// Token: 0x170012C9 RID: 4809
			// (get) Token: 0x0600555F RID: 21855 RVA: 0x0017DEBA File Offset: 0x0017C0BA
			public static PerkObject PracticalSmelter
			{
				get
				{
					return DefaultPerks.Instance._craftingPracticalSmelter;
				}
			}

			// Token: 0x170012CA RID: 4810
			// (get) Token: 0x06005560 RID: 21856 RVA: 0x0017DEC6 File Offset: 0x0017C0C6
			public static PerkObject PracticalSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingPracticalSmith;
				}
			}

			// Token: 0x170012CB RID: 4811
			// (get) Token: 0x06005561 RID: 21857 RVA: 0x0017DED2 File Offset: 0x0017C0D2
			public static PerkObject ArtisanSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingArtisanSmith;
				}
			}

			// Token: 0x170012CC RID: 4812
			// (get) Token: 0x06005562 RID: 21858 RVA: 0x0017DEDE File Offset: 0x0017C0DE
			public static PerkObject ExperiencedSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingExperiencedSmith;
				}
			}

			// Token: 0x170012CD RID: 4813
			// (get) Token: 0x06005563 RID: 21859 RVA: 0x0017DEEA File Offset: 0x0017C0EA
			public static PerkObject MasterSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingMasterSmith;
				}
			}

			// Token: 0x170012CE RID: 4814
			// (get) Token: 0x06005564 RID: 21860 RVA: 0x0017DEF6 File Offset: 0x0017C0F6
			public static PerkObject LegendarySmith
			{
				get
				{
					return DefaultPerks.Instance._craftingLegendarySmith;
				}
			}

			// Token: 0x170012CF RID: 4815
			// (get) Token: 0x06005565 RID: 21861 RVA: 0x0017DF02 File Offset: 0x0017C102
			public static PerkObject VigorousSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingVigorousSmith;
				}
			}

			// Token: 0x170012D0 RID: 4816
			// (get) Token: 0x06005566 RID: 21862 RVA: 0x0017DF0E File Offset: 0x0017C10E
			public static PerkObject StrongSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingStrongSmith;
				}
			}

			// Token: 0x170012D1 RID: 4817
			// (get) Token: 0x06005567 RID: 21863 RVA: 0x0017DF1A File Offset: 0x0017C11A
			public static PerkObject EnduringSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingEnduringSmith;
				}
			}

			// Token: 0x170012D2 RID: 4818
			// (get) Token: 0x06005568 RID: 21864 RVA: 0x0017DF26 File Offset: 0x0017C126
			public static PerkObject WeaponMasterSmith
			{
				get
				{
					return DefaultPerks.Instance._craftingFencerSmith;
				}
			}

			// Token: 0x170012D3 RID: 4819
			// (get) Token: 0x06005569 RID: 21865 RVA: 0x0017DF32 File Offset: 0x0017C132
			public static PerkObject SharpenedEdge
			{
				get
				{
					return DefaultPerks.Instance._craftingSharpenedEdge;
				}
			}

			// Token: 0x170012D4 RID: 4820
			// (get) Token: 0x0600556A RID: 21866 RVA: 0x0017DF3E File Offset: 0x0017C13E
			public static PerkObject SharpenedTip
			{
				get
				{
					return DefaultPerks.Instance._craftingSharpenedTip;
				}
			}
		}

		// Token: 0x02000690 RID: 1680
		public static class Scouting
		{
			// Token: 0x170012D5 RID: 4821
			// (get) Token: 0x0600556B RID: 21867 RVA: 0x0017DF4A File Offset: 0x0017C14A
			public static PerkObject DayTraveler
			{
				get
				{
					return DefaultPerks.Instance._scoutingDayTraveler;
				}
			}

			// Token: 0x170012D6 RID: 4822
			// (get) Token: 0x0600556C RID: 21868 RVA: 0x0017DF56 File Offset: 0x0017C156
			public static PerkObject Pathfinder
			{
				get
				{
					return DefaultPerks.Instance._scoutingPathfinder;
				}
			}

			// Token: 0x170012D7 RID: 4823
			// (get) Token: 0x0600556D RID: 21869 RVA: 0x0017DF62 File Offset: 0x0017C162
			public static PerkObject NightRunner
			{
				get
				{
					return DefaultPerks.Instance._scoutingNightRunner;
				}
			}

			// Token: 0x170012D8 RID: 4824
			// (get) Token: 0x0600556E RID: 21870 RVA: 0x0017DF6E File Offset: 0x0017C16E
			public static PerkObject WaterDiviner
			{
				get
				{
					return DefaultPerks.Instance._scoutingWaterDiviner;
				}
			}

			// Token: 0x170012D9 RID: 4825
			// (get) Token: 0x0600556F RID: 21871 RVA: 0x0017DF7A File Offset: 0x0017C17A
			public static PerkObject ForestKin
			{
				get
				{
					return DefaultPerks.Instance._scoutingForestKin;
				}
			}

			// Token: 0x170012DA RID: 4826
			// (get) Token: 0x06005570 RID: 21872 RVA: 0x0017DF86 File Offset: 0x0017C186
			public static PerkObject DesertBorn
			{
				get
				{
					return DefaultPerks.Instance._scoutingDesertBorn;
				}
			}

			// Token: 0x170012DB RID: 4827
			// (get) Token: 0x06005571 RID: 21873 RVA: 0x0017DF92 File Offset: 0x0017C192
			public static PerkObject ForcedMarch
			{
				get
				{
					return DefaultPerks.Instance._scoutingForcedMarch;
				}
			}

			// Token: 0x170012DC RID: 4828
			// (get) Token: 0x06005572 RID: 21874 RVA: 0x0017DF9E File Offset: 0x0017C19E
			public static PerkObject Unburdened
			{
				get
				{
					return DefaultPerks.Instance._scoutingUnburdened;
				}
			}

			// Token: 0x170012DD RID: 4829
			// (get) Token: 0x06005573 RID: 21875 RVA: 0x0017DFAA File Offset: 0x0017C1AA
			public static PerkObject Tracker
			{
				get
				{
					return DefaultPerks.Instance._scoutingTracker;
				}
			}

			// Token: 0x170012DE RID: 4830
			// (get) Token: 0x06005574 RID: 21876 RVA: 0x0017DFB6 File Offset: 0x0017C1B6
			public static PerkObject Ranger
			{
				get
				{
					return DefaultPerks.Instance._scoutingRanger;
				}
			}

			// Token: 0x170012DF RID: 4831
			// (get) Token: 0x06005575 RID: 21877 RVA: 0x0017DFC2 File Offset: 0x0017C1C2
			public static PerkObject MountedScouts
			{
				get
				{
					return DefaultPerks.Instance._scoutingMountedScouts;
				}
			}

			// Token: 0x170012E0 RID: 4832
			// (get) Token: 0x06005576 RID: 21878 RVA: 0x0017DFCE File Offset: 0x0017C1CE
			public static PerkObject Patrols
			{
				get
				{
					return DefaultPerks.Instance._scoutingPatrols;
				}
			}

			// Token: 0x170012E1 RID: 4833
			// (get) Token: 0x06005577 RID: 21879 RVA: 0x0017DFDA File Offset: 0x0017C1DA
			public static PerkObject Foragers
			{
				get
				{
					return DefaultPerks.Instance._scoutingForagers;
				}
			}

			// Token: 0x170012E2 RID: 4834
			// (get) Token: 0x06005578 RID: 21880 RVA: 0x0017DFE6 File Offset: 0x0017C1E6
			public static PerkObject BeastWhisperer
			{
				get
				{
					return DefaultPerks.Instance._scoutingBeastWhisperer;
				}
			}

			// Token: 0x170012E3 RID: 4835
			// (get) Token: 0x06005579 RID: 21881 RVA: 0x0017DFF2 File Offset: 0x0017C1F2
			public static PerkObject VillageNetwork
			{
				get
				{
					return DefaultPerks.Instance._scoutingVillageNetwork;
				}
			}

			// Token: 0x170012E4 RID: 4836
			// (get) Token: 0x0600557A RID: 21882 RVA: 0x0017DFFE File Offset: 0x0017C1FE
			public static PerkObject RumourNetwork
			{
				get
				{
					return DefaultPerks.Instance._scoutingRumourNetwork;
				}
			}

			// Token: 0x170012E5 RID: 4837
			// (get) Token: 0x0600557B RID: 21883 RVA: 0x0017E00A File Offset: 0x0017C20A
			public static PerkObject VantagePoint
			{
				get
				{
					return DefaultPerks.Instance._scoutingVantagePoint;
				}
			}

			// Token: 0x170012E6 RID: 4838
			// (get) Token: 0x0600557C RID: 21884 RVA: 0x0017E016 File Offset: 0x0017C216
			public static PerkObject KeenSight
			{
				get
				{
					return DefaultPerks.Instance._scoutingKeenSight;
				}
			}

			// Token: 0x170012E7 RID: 4839
			// (get) Token: 0x0600557D RID: 21885 RVA: 0x0017E022 File Offset: 0x0017C222
			public static PerkObject Vanguard
			{
				get
				{
					return DefaultPerks.Instance._scoutingVanguard;
				}
			}

			// Token: 0x170012E8 RID: 4840
			// (get) Token: 0x0600557E RID: 21886 RVA: 0x0017E02E File Offset: 0x0017C22E
			public static PerkObject Rearguard
			{
				get
				{
					return DefaultPerks.Instance._scoutingRearguard;
				}
			}

			// Token: 0x170012E9 RID: 4841
			// (get) Token: 0x0600557F RID: 21887 RVA: 0x0017E03A File Offset: 0x0017C23A
			public static PerkObject UncannyInsight
			{
				get
				{
					return DefaultPerks.Instance._scoutingUncannyInsight;
				}
			}
		}

		// Token: 0x02000691 RID: 1681
		public static class Tactics
		{
			// Token: 0x170012EA RID: 4842
			// (get) Token: 0x06005580 RID: 21888 RVA: 0x0017E046 File Offset: 0x0017C246
			public static PerkObject TightFormations
			{
				get
				{
					return DefaultPerks.Instance._tacticsTightFormations;
				}
			}

			// Token: 0x170012EB RID: 4843
			// (get) Token: 0x06005581 RID: 21889 RVA: 0x0017E052 File Offset: 0x0017C252
			public static PerkObject LooseFormations
			{
				get
				{
					return DefaultPerks.Instance._tacticsLooseFormations;
				}
			}

			// Token: 0x170012EC RID: 4844
			// (get) Token: 0x06005582 RID: 21890 RVA: 0x0017E05E File Offset: 0x0017C25E
			public static PerkObject ExtendedSkirmish
			{
				get
				{
					return DefaultPerks.Instance._tacticsExtendedSkirmish;
				}
			}

			// Token: 0x170012ED RID: 4845
			// (get) Token: 0x06005583 RID: 21891 RVA: 0x0017E06A File Offset: 0x0017C26A
			public static PerkObject DecisiveBattle
			{
				get
				{
					return DefaultPerks.Instance._tacticsDecisiveBattle;
				}
			}

			// Token: 0x170012EE RID: 4846
			// (get) Token: 0x06005584 RID: 21892 RVA: 0x0017E076 File Offset: 0x0017C276
			public static PerkObject SmallUnitTactics
			{
				get
				{
					return DefaultPerks.Instance._tacticsSmallUnitTactics;
				}
			}

			// Token: 0x170012EF RID: 4847
			// (get) Token: 0x06005585 RID: 21893 RVA: 0x0017E082 File Offset: 0x0017C282
			public static PerkObject HordeLeader
			{
				get
				{
					return DefaultPerks.Instance._tacticsHordeLeader;
				}
			}

			// Token: 0x170012F0 RID: 4848
			// (get) Token: 0x06005586 RID: 21894 RVA: 0x0017E08E File Offset: 0x0017C28E
			public static PerkObject LawKeeper
			{
				get
				{
					return DefaultPerks.Instance._tacticsLawKeeper;
				}
			}

			// Token: 0x170012F1 RID: 4849
			// (get) Token: 0x06005587 RID: 21895 RVA: 0x0017E09A File Offset: 0x0017C29A
			public static PerkObject Coaching
			{
				get
				{
					return DefaultPerks.Instance._tacticsCoaching;
				}
			}

			// Token: 0x170012F2 RID: 4850
			// (get) Token: 0x06005588 RID: 21896 RVA: 0x0017E0A6 File Offset: 0x0017C2A6
			public static PerkObject SwiftRegroup
			{
				get
				{
					return DefaultPerks.Instance._tacticsSwiftRegroup;
				}
			}

			// Token: 0x170012F3 RID: 4851
			// (get) Token: 0x06005589 RID: 21897 RVA: 0x0017E0B2 File Offset: 0x0017C2B2
			public static PerkObject Improviser
			{
				get
				{
					return DefaultPerks.Instance._tacticsImproviser;
				}
			}

			// Token: 0x170012F4 RID: 4852
			// (get) Token: 0x0600558A RID: 21898 RVA: 0x0017E0BE File Offset: 0x0017C2BE
			public static PerkObject OnTheMarch
			{
				get
				{
					return DefaultPerks.Instance._tacticsOnTheMarch;
				}
			}

			// Token: 0x170012F5 RID: 4853
			// (get) Token: 0x0600558B RID: 21899 RVA: 0x0017E0CA File Offset: 0x0017C2CA
			public static PerkObject CallToArms
			{
				get
				{
					return DefaultPerks.Instance._tacticsCallToArms;
				}
			}

			// Token: 0x170012F6 RID: 4854
			// (get) Token: 0x0600558C RID: 21900 RVA: 0x0017E0D6 File Offset: 0x0017C2D6
			public static PerkObject PickThemOfTheWalls
			{
				get
				{
					return DefaultPerks.Instance._tacticsPickThemOfTheWalls;
				}
			}

			// Token: 0x170012F7 RID: 4855
			// (get) Token: 0x0600558D RID: 21901 RVA: 0x0017E0E2 File Offset: 0x0017C2E2
			public static PerkObject MakeThemPay
			{
				get
				{
					return DefaultPerks.Instance._tacticsMakeThemPay;
				}
			}

			// Token: 0x170012F8 RID: 4856
			// (get) Token: 0x0600558E RID: 21902 RVA: 0x0017E0EE File Offset: 0x0017C2EE
			public static PerkObject EliteReserves
			{
				get
				{
					return DefaultPerks.Instance._tacticsEliteReserves;
				}
			}

			// Token: 0x170012F9 RID: 4857
			// (get) Token: 0x0600558F RID: 21903 RVA: 0x0017E0FA File Offset: 0x0017C2FA
			public static PerkObject Encirclement
			{
				get
				{
					return DefaultPerks.Instance._tacticsEncirclement;
				}
			}

			// Token: 0x170012FA RID: 4858
			// (get) Token: 0x06005590 RID: 21904 RVA: 0x0017E106 File Offset: 0x0017C306
			public static PerkObject PreBattleManeuvers
			{
				get
				{
					return DefaultPerks.Instance._tacticsPreBattleManeuvers;
				}
			}

			// Token: 0x170012FB RID: 4859
			// (get) Token: 0x06005591 RID: 21905 RVA: 0x0017E112 File Offset: 0x0017C312
			public static PerkObject Besieged
			{
				get
				{
					return DefaultPerks.Instance._tacticsBesieged;
				}
			}

			// Token: 0x170012FC RID: 4860
			// (get) Token: 0x06005592 RID: 21906 RVA: 0x0017E11E File Offset: 0x0017C31E
			public static PerkObject Counteroffensive
			{
				get
				{
					return DefaultPerks.Instance._tacticsCounteroffensive;
				}
			}

			// Token: 0x170012FD RID: 4861
			// (get) Token: 0x06005593 RID: 21907 RVA: 0x0017E12A File Offset: 0x0017C32A
			public static PerkObject Gensdarmes
			{
				get
				{
					return DefaultPerks.Instance._tacticsGensdarmes;
				}
			}

			// Token: 0x170012FE RID: 4862
			// (get) Token: 0x06005594 RID: 21908 RVA: 0x0017E136 File Offset: 0x0017C336
			public static PerkObject TacticalMastery
			{
				get
				{
					return DefaultPerks.Instance._tacticsTacticalMastery;
				}
			}
		}

		// Token: 0x02000692 RID: 1682
		public static class Roguery
		{
			// Token: 0x170012FF RID: 4863
			// (get) Token: 0x06005595 RID: 21909 RVA: 0x0017E142 File Offset: 0x0017C342
			public static PerkObject NoRestForTheWicked
			{
				get
				{
					return DefaultPerks.Instance._rogueryNoRestForTheWicked;
				}
			}

			// Token: 0x17001300 RID: 4864
			// (get) Token: 0x06005596 RID: 21910 RVA: 0x0017E14E File Offset: 0x0017C34E
			public static PerkObject SweetTalker
			{
				get
				{
					return DefaultPerks.Instance._roguerySweetTalker;
				}
			}

			// Token: 0x17001301 RID: 4865
			// (get) Token: 0x06005597 RID: 21911 RVA: 0x0017E15A File Offset: 0x0017C35A
			public static PerkObject TwoFaced
			{
				get
				{
					return DefaultPerks.Instance._rogueryTwoFaced;
				}
			}

			// Token: 0x17001302 RID: 4866
			// (get) Token: 0x06005598 RID: 21912 RVA: 0x0017E166 File Offset: 0x0017C366
			public static PerkObject DeepPockets
			{
				get
				{
					return DefaultPerks.Instance._rogueryDeepPockets;
				}
			}

			// Token: 0x17001303 RID: 4867
			// (get) Token: 0x06005599 RID: 21913 RVA: 0x0017E172 File Offset: 0x0017C372
			public static PerkObject InBestLight
			{
				get
				{
					return DefaultPerks.Instance._rogueryInBestLight;
				}
			}

			// Token: 0x17001304 RID: 4868
			// (get) Token: 0x0600559A RID: 21914 RVA: 0x0017E17E File Offset: 0x0017C37E
			public static PerkObject KnowHow
			{
				get
				{
					return DefaultPerks.Instance._rogueryKnowHow;
				}
			}

			// Token: 0x17001305 RID: 4869
			// (get) Token: 0x0600559B RID: 21915 RVA: 0x0017E18A File Offset: 0x0017C38A
			public static PerkObject Promises
			{
				get
				{
					return DefaultPerks.Instance._rogueryPromises;
				}
			}

			// Token: 0x17001306 RID: 4870
			// (get) Token: 0x0600559C RID: 21916 RVA: 0x0017E196 File Offset: 0x0017C396
			public static PerkObject Manhunter
			{
				get
				{
					return DefaultPerks.Instance._rogueryManhunter;
				}
			}

			// Token: 0x17001307 RID: 4871
			// (get) Token: 0x0600559D RID: 21917 RVA: 0x0017E1A2 File Offset: 0x0017C3A2
			public static PerkObject Scarface
			{
				get
				{
					return DefaultPerks.Instance._rogueryScarface;
				}
			}

			// Token: 0x17001308 RID: 4872
			// (get) Token: 0x0600559E RID: 21918 RVA: 0x0017E1AE File Offset: 0x0017C3AE
			public static PerkObject WhiteLies
			{
				get
				{
					return DefaultPerks.Instance._rogueryWhiteLies;
				}
			}

			// Token: 0x17001309 RID: 4873
			// (get) Token: 0x0600559F RID: 21919 RVA: 0x0017E1BA File Offset: 0x0017C3BA
			public static PerkObject SmugglerConnections
			{
				get
				{
					return DefaultPerks.Instance._roguerySmugglerConnections;
				}
			}

			// Token: 0x1700130A RID: 4874
			// (get) Token: 0x060055A0 RID: 21920 RVA: 0x0017E1C6 File Offset: 0x0017C3C6
			public static PerkObject PartnersInCrime
			{
				get
				{
					return DefaultPerks.Instance._rogueryPartnersInCrime;
				}
			}

			// Token: 0x1700130B RID: 4875
			// (get) Token: 0x060055A1 RID: 21921 RVA: 0x0017E1D2 File Offset: 0x0017C3D2
			public static PerkObject OneOfTheFamily
			{
				get
				{
					return DefaultPerks.Instance._rogueryOneOfTheFamily;
				}
			}

			// Token: 0x1700130C RID: 4876
			// (get) Token: 0x060055A2 RID: 21922 RVA: 0x0017E1DE File Offset: 0x0017C3DE
			public static PerkObject SaltTheEarth
			{
				get
				{
					return DefaultPerks.Instance._roguerySaltTheEarth;
				}
			}

			// Token: 0x1700130D RID: 4877
			// (get) Token: 0x060055A3 RID: 21923 RVA: 0x0017E1EA File Offset: 0x0017C3EA
			public static PerkObject Carver
			{
				get
				{
					return DefaultPerks.Instance._rogueryCarver;
				}
			}

			// Token: 0x1700130E RID: 4878
			// (get) Token: 0x060055A4 RID: 21924 RVA: 0x0017E1F6 File Offset: 0x0017C3F6
			public static PerkObject RansomBroker
			{
				get
				{
					return DefaultPerks.Instance._rogueryRansomBroker;
				}
			}

			// Token: 0x1700130F RID: 4879
			// (get) Token: 0x060055A5 RID: 21925 RVA: 0x0017E202 File Offset: 0x0017C402
			public static PerkObject ArmsDealer
			{
				get
				{
					return DefaultPerks.Instance._rogueryArmsDealer;
				}
			}

			// Token: 0x17001310 RID: 4880
			// (get) Token: 0x060055A6 RID: 21926 RVA: 0x0017E20E File Offset: 0x0017C40E
			public static PerkObject DirtyFighting
			{
				get
				{
					return DefaultPerks.Instance._rogueryDirtyFighting;
				}
			}

			// Token: 0x17001311 RID: 4881
			// (get) Token: 0x060055A7 RID: 21927 RVA: 0x0017E21A File Offset: 0x0017C41A
			public static PerkObject DashAndSlash
			{
				get
				{
					return DefaultPerks.Instance._rogueryDashAndSlash;
				}
			}

			// Token: 0x17001312 RID: 4882
			// (get) Token: 0x060055A8 RID: 21928 RVA: 0x0017E226 File Offset: 0x0017C426
			public static PerkObject FleetFooted
			{
				get
				{
					return DefaultPerks.Instance._rogueryFleetFooted;
				}
			}

			// Token: 0x17001313 RID: 4883
			// (get) Token: 0x060055A9 RID: 21929 RVA: 0x0017E232 File Offset: 0x0017C432
			public static PerkObject RogueExtraordinaire
			{
				get
				{
					return DefaultPerks.Instance._rogueryRogueExtraordinaire;
				}
			}
		}

		// Token: 0x02000693 RID: 1683
		public static class Charm
		{
			// Token: 0x17001314 RID: 4884
			// (get) Token: 0x060055AA RID: 21930 RVA: 0x0017E23E File Offset: 0x0017C43E
			public static PerkObject Virile
			{
				get
				{
					return DefaultPerks.Instance._charmVirile;
				}
			}

			// Token: 0x17001315 RID: 4885
			// (get) Token: 0x060055AB RID: 21931 RVA: 0x0017E24A File Offset: 0x0017C44A
			public static PerkObject SelfPromoter
			{
				get
				{
					return DefaultPerks.Instance._charmSelfPromoter;
				}
			}

			// Token: 0x17001316 RID: 4886
			// (get) Token: 0x060055AC RID: 21932 RVA: 0x0017E256 File Offset: 0x0017C456
			public static PerkObject Oratory
			{
				get
				{
					return DefaultPerks.Instance._charmOratory;
				}
			}

			// Token: 0x17001317 RID: 4887
			// (get) Token: 0x060055AD RID: 21933 RVA: 0x0017E262 File Offset: 0x0017C462
			public static PerkObject Warlord
			{
				get
				{
					return DefaultPerks.Instance._charmWarlord;
				}
			}

			// Token: 0x17001318 RID: 4888
			// (get) Token: 0x060055AE RID: 21934 RVA: 0x0017E26E File Offset: 0x0017C46E
			public static PerkObject ForgivableGrievances
			{
				get
				{
					return DefaultPerks.Instance._charmForgivableGrievances;
				}
			}

			// Token: 0x17001319 RID: 4889
			// (get) Token: 0x060055AF RID: 21935 RVA: 0x0017E27A File Offset: 0x0017C47A
			public static PerkObject MeaningfulFavors
			{
				get
				{
					return DefaultPerks.Instance._charmMeaningfulFavors;
				}
			}

			// Token: 0x1700131A RID: 4890
			// (get) Token: 0x060055B0 RID: 21936 RVA: 0x0017E286 File Offset: 0x0017C486
			public static PerkObject InBloom
			{
				get
				{
					return DefaultPerks.Instance._charmInBloom;
				}
			}

			// Token: 0x1700131B RID: 4891
			// (get) Token: 0x060055B1 RID: 21937 RVA: 0x0017E292 File Offset: 0x0017C492
			public static PerkObject YoungAndRespectful
			{
				get
				{
					return DefaultPerks.Instance._charmYoungAndRespectful;
				}
			}

			// Token: 0x1700131C RID: 4892
			// (get) Token: 0x060055B2 RID: 21938 RVA: 0x0017E29E File Offset: 0x0017C49E
			public static PerkObject Firebrand
			{
				get
				{
					return DefaultPerks.Instance._charmFirebrand;
				}
			}

			// Token: 0x1700131D RID: 4893
			// (get) Token: 0x060055B3 RID: 21939 RVA: 0x0017E2AA File Offset: 0x0017C4AA
			public static PerkObject FlexibleEthics
			{
				get
				{
					return DefaultPerks.Instance._charmFlexibleEthics;
				}
			}

			// Token: 0x1700131E RID: 4894
			// (get) Token: 0x060055B4 RID: 21940 RVA: 0x0017E2B6 File Offset: 0x0017C4B6
			public static PerkObject EffortForThePeople
			{
				get
				{
					return DefaultPerks.Instance._charmEffortForThePeople;
				}
			}

			// Token: 0x1700131F RID: 4895
			// (get) Token: 0x060055B5 RID: 21941 RVA: 0x0017E2C2 File Offset: 0x0017C4C2
			public static PerkObject SlickNegotiator
			{
				get
				{
					return DefaultPerks.Instance._charmSlickNegotiator;
				}
			}

			// Token: 0x17001320 RID: 4896
			// (get) Token: 0x060055B6 RID: 21942 RVA: 0x0017E2CE File Offset: 0x0017C4CE
			public static PerkObject GoodNatured
			{
				get
				{
					return DefaultPerks.Instance._charmGoodNatured;
				}
			}

			// Token: 0x17001321 RID: 4897
			// (get) Token: 0x060055B7 RID: 21943 RVA: 0x0017E2DA File Offset: 0x0017C4DA
			public static PerkObject Tribute
			{
				get
				{
					return DefaultPerks.Instance._charmTribute;
				}
			}

			// Token: 0x17001322 RID: 4898
			// (get) Token: 0x060055B8 RID: 21944 RVA: 0x0017E2E6 File Offset: 0x0017C4E6
			public static PerkObject MoralLeader
			{
				get
				{
					return DefaultPerks.Instance._charmMoralLeader;
				}
			}

			// Token: 0x17001323 RID: 4899
			// (get) Token: 0x060055B9 RID: 21945 RVA: 0x0017E2F2 File Offset: 0x0017C4F2
			public static PerkObject NaturalLeader
			{
				get
				{
					return DefaultPerks.Instance._charmNaturalLeader;
				}
			}

			// Token: 0x17001324 RID: 4900
			// (get) Token: 0x060055BA RID: 21946 RVA: 0x0017E2FE File Offset: 0x0017C4FE
			public static PerkObject PublicSpeaker
			{
				get
				{
					return DefaultPerks.Instance._charmPublicSpeaker;
				}
			}

			// Token: 0x17001325 RID: 4901
			// (get) Token: 0x060055BB RID: 21947 RVA: 0x0017E30A File Offset: 0x0017C50A
			public static PerkObject Parade
			{
				get
				{
					return DefaultPerks.Instance._charmParade;
				}
			}

			// Token: 0x17001326 RID: 4902
			// (get) Token: 0x060055BC RID: 21948 RVA: 0x0017E316 File Offset: 0x0017C516
			public static PerkObject Camaraderie
			{
				get
				{
					return DefaultPerks.Instance._charmCamaraderie;
				}
			}

			// Token: 0x17001327 RID: 4903
			// (get) Token: 0x060055BD RID: 21949 RVA: 0x0017E322 File Offset: 0x0017C522
			public static PerkObject ImmortalCharm
			{
				get
				{
					return DefaultPerks.Instance._charmImmortalCharm;
				}
			}
		}

		// Token: 0x02000694 RID: 1684
		public static class Leadership
		{
			// Token: 0x17001328 RID: 4904
			// (get) Token: 0x060055BE RID: 21950 RVA: 0x0017E32E File Offset: 0x0017C52E
			public static PerkObject CombatTips
			{
				get
				{
					return DefaultPerks.Instance._leadershipCombatTips;
				}
			}

			// Token: 0x17001329 RID: 4905
			// (get) Token: 0x060055BF RID: 21951 RVA: 0x0017E33A File Offset: 0x0017C53A
			public static PerkObject RaiseTheMeek
			{
				get
				{
					return DefaultPerks.Instance._leadershipRaiseTheMeek;
				}
			}

			// Token: 0x1700132A RID: 4906
			// (get) Token: 0x060055C0 RID: 21952 RVA: 0x0017E346 File Offset: 0x0017C546
			public static PerkObject FerventAttacker
			{
				get
				{
					return DefaultPerks.Instance._leadershipFerventAttacker;
				}
			}

			// Token: 0x1700132B RID: 4907
			// (get) Token: 0x060055C1 RID: 21953 RVA: 0x0017E352 File Offset: 0x0017C552
			public static PerkObject StoutDefender
			{
				get
				{
					return DefaultPerks.Instance._leadershipStoutDefender;
				}
			}

			// Token: 0x1700132C RID: 4908
			// (get) Token: 0x060055C2 RID: 21954 RVA: 0x0017E35E File Offset: 0x0017C55E
			public static PerkObject Authority
			{
				get
				{
					return DefaultPerks.Instance._leadershipAuthority;
				}
			}

			// Token: 0x1700132D RID: 4909
			// (get) Token: 0x060055C3 RID: 21955 RVA: 0x0017E36A File Offset: 0x0017C56A
			public static PerkObject HeroicLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipHeroicLeader;
				}
			}

			// Token: 0x1700132E RID: 4910
			// (get) Token: 0x060055C4 RID: 21956 RVA: 0x0017E376 File Offset: 0x0017C576
			public static PerkObject LoyaltyAndHonor
			{
				get
				{
					return DefaultPerks.Instance._leadershipLoyaltyAndHonor;
				}
			}

			// Token: 0x1700132F RID: 4911
			// (get) Token: 0x060055C5 RID: 21957 RVA: 0x0017E382 File Offset: 0x0017C582
			public static PerkObject Presence
			{
				get
				{
					return DefaultPerks.Instance._leadershipPresence;
				}
			}

			// Token: 0x17001330 RID: 4912
			// (get) Token: 0x060055C6 RID: 21958 RVA: 0x0017E38E File Offset: 0x0017C58E
			public static PerkObject FamousCommander
			{
				get
				{
					return DefaultPerks.Instance._leadershipFamousCommander;
				}
			}

			// Token: 0x17001331 RID: 4913
			// (get) Token: 0x060055C7 RID: 21959 RVA: 0x0017E39A File Offset: 0x0017C59A
			public static PerkObject LeaderOfMasses
			{
				get
				{
					return DefaultPerks.Instance._leadershipLeaderOfTheMasses;
				}
			}

			// Token: 0x17001332 RID: 4914
			// (get) Token: 0x060055C8 RID: 21960 RVA: 0x0017E3A6 File Offset: 0x0017C5A6
			public static PerkObject VeteransRespect
			{
				get
				{
					return DefaultPerks.Instance._leadershipVeteransRespect;
				}
			}

			// Token: 0x17001333 RID: 4915
			// (get) Token: 0x060055C9 RID: 21961 RVA: 0x0017E3B2 File Offset: 0x0017C5B2
			public static PerkObject CitizenMilitia
			{
				get
				{
					return DefaultPerks.Instance._leadershipCitizenMilitia;
				}
			}

			// Token: 0x17001334 RID: 4916
			// (get) Token: 0x060055CA RID: 21962 RVA: 0x0017E3BE File Offset: 0x0017C5BE
			public static PerkObject InspiringLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipInspiringLeader;
				}
			}

			// Token: 0x17001335 RID: 4917
			// (get) Token: 0x060055CB RID: 21963 RVA: 0x0017E3CA File Offset: 0x0017C5CA
			public static PerkObject UpliftingSpirit
			{
				get
				{
					return DefaultPerks.Instance._leadershipUpliftingSpirit;
				}
			}

			// Token: 0x17001336 RID: 4918
			// (get) Token: 0x060055CC RID: 21964 RVA: 0x0017E3D6 File Offset: 0x0017C5D6
			public static PerkObject MakeADifference
			{
				get
				{
					return DefaultPerks.Instance._leadershipMakeADifference;
				}
			}

			// Token: 0x17001337 RID: 4919
			// (get) Token: 0x060055CD RID: 21965 RVA: 0x0017E3E2 File Offset: 0x0017C5E2
			public static PerkObject LeadByExample
			{
				get
				{
					return DefaultPerks.Instance._leadershipLeadByExample;
				}
			}

			// Token: 0x17001338 RID: 4920
			// (get) Token: 0x060055CE RID: 21966 RVA: 0x0017E3EE File Offset: 0x0017C5EE
			public static PerkObject TrustedCommander
			{
				get
				{
					return DefaultPerks.Instance._leadershipTrustedCommander;
				}
			}

			// Token: 0x17001339 RID: 4921
			// (get) Token: 0x060055CF RID: 21967 RVA: 0x0017E3FA File Offset: 0x0017C5FA
			public static PerkObject GreatLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipGreatLeader;
				}
			}

			// Token: 0x1700133A RID: 4922
			// (get) Token: 0x060055D0 RID: 21968 RVA: 0x0017E406 File Offset: 0x0017C606
			public static PerkObject WePledgeOurSwords
			{
				get
				{
					return DefaultPerks.Instance._leadershipWePledgeOurSwords;
				}
			}

			// Token: 0x1700133B RID: 4923
			// (get) Token: 0x060055D1 RID: 21969 RVA: 0x0017E412 File Offset: 0x0017C612
			public static PerkObject TalentMagnet
			{
				get
				{
					return DefaultPerks.Instance._leadershipTalentMagnet;
				}
			}

			// Token: 0x1700133C RID: 4924
			// (get) Token: 0x060055D2 RID: 21970 RVA: 0x0017E41E File Offset: 0x0017C61E
			public static PerkObject UltimateLeader
			{
				get
				{
					return DefaultPerks.Instance._leadershipUltimateLeader;
				}
			}
		}

		// Token: 0x02000695 RID: 1685
		public static class Trade
		{
			// Token: 0x1700133D RID: 4925
			// (get) Token: 0x060055D3 RID: 21971 RVA: 0x0017E42A File Offset: 0x0017C62A
			public static PerkObject Appraiser
			{
				get
				{
					return DefaultPerks.Instance._tradeAppraiser;
				}
			}

			// Token: 0x1700133E RID: 4926
			// (get) Token: 0x060055D4 RID: 21972 RVA: 0x0017E436 File Offset: 0x0017C636
			public static PerkObject WholeSeller
			{
				get
				{
					return DefaultPerks.Instance._tradeWholeSeller;
				}
			}

			// Token: 0x1700133F RID: 4927
			// (get) Token: 0x060055D5 RID: 21973 RVA: 0x0017E442 File Offset: 0x0017C642
			public static PerkObject CaravanMaster
			{
				get
				{
					return DefaultPerks.Instance._tradeCaravanMaster;
				}
			}

			// Token: 0x17001340 RID: 4928
			// (get) Token: 0x060055D6 RID: 21974 RVA: 0x0017E44E File Offset: 0x0017C64E
			public static PerkObject MarketDealer
			{
				get
				{
					return DefaultPerks.Instance._tradeMarketDealer;
				}
			}

			// Token: 0x17001341 RID: 4929
			// (get) Token: 0x060055D7 RID: 21975 RVA: 0x0017E45A File Offset: 0x0017C65A
			public static PerkObject TravelingRumors
			{
				get
				{
					return DefaultPerks.Instance._tradeTravelingRumors;
				}
			}

			// Token: 0x17001342 RID: 4930
			// (get) Token: 0x060055D8 RID: 21976 RVA: 0x0017E466 File Offset: 0x0017C666
			public static PerkObject LocalConnection
			{
				get
				{
					return DefaultPerks.Instance._tradeLocalConnection;
				}
			}

			// Token: 0x17001343 RID: 4931
			// (get) Token: 0x060055D9 RID: 21977 RVA: 0x0017E472 File Offset: 0x0017C672
			public static PerkObject DistributedGoods
			{
				get
				{
					return DefaultPerks.Instance._tradeDistributedGoods;
				}
			}

			// Token: 0x17001344 RID: 4932
			// (get) Token: 0x060055DA RID: 21978 RVA: 0x0017E47E File Offset: 0x0017C67E
			public static PerkObject Tollgates
			{
				get
				{
					return DefaultPerks.Instance._tradeTollgates;
				}
			}

			// Token: 0x17001345 RID: 4933
			// (get) Token: 0x060055DB RID: 21979 RVA: 0x0017E48A File Offset: 0x0017C68A
			public static PerkObject ArtisanCommunity
			{
				get
				{
					return DefaultPerks.Instance._tradeArtisanCommunity;
				}
			}

			// Token: 0x17001346 RID: 4934
			// (get) Token: 0x060055DC RID: 21980 RVA: 0x0017E496 File Offset: 0x0017C696
			public static PerkObject GreatInvestor
			{
				get
				{
					return DefaultPerks.Instance._tradeGreatInvestor;
				}
			}

			// Token: 0x17001347 RID: 4935
			// (get) Token: 0x060055DD RID: 21981 RVA: 0x0017E4A2 File Offset: 0x0017C6A2
			public static PerkObject MercenaryConnections
			{
				get
				{
					return DefaultPerks.Instance._tradeMercenaryConnections;
				}
			}

			// Token: 0x17001348 RID: 4936
			// (get) Token: 0x060055DE RID: 21982 RVA: 0x0017E4AE File Offset: 0x0017C6AE
			public static PerkObject ContentTrades
			{
				get
				{
					return DefaultPerks.Instance._tradeContentTrades;
				}
			}

			// Token: 0x17001349 RID: 4937
			// (get) Token: 0x060055DF RID: 21983 RVA: 0x0017E4BA File Offset: 0x0017C6BA
			public static PerkObject InsurancePlans
			{
				get
				{
					return DefaultPerks.Instance._tradeInsurancePlans;
				}
			}

			// Token: 0x1700134A RID: 4938
			// (get) Token: 0x060055E0 RID: 21984 RVA: 0x0017E4C6 File Offset: 0x0017C6C6
			public static PerkObject RapidDevelopment
			{
				get
				{
					return DefaultPerks.Instance._tradeRapidDevelopment;
				}
			}

			// Token: 0x1700134B RID: 4939
			// (get) Token: 0x060055E1 RID: 21985 RVA: 0x0017E4D2 File Offset: 0x0017C6D2
			public static PerkObject GranaryAccountant
			{
				get
				{
					return DefaultPerks.Instance._tradeGranaryAccountant;
				}
			}

			// Token: 0x1700134C RID: 4940
			// (get) Token: 0x060055E2 RID: 21986 RVA: 0x0017E4DE File Offset: 0x0017C6DE
			public static PerkObject TradeyardForeman
			{
				get
				{
					return DefaultPerks.Instance._tradeTradeyardForeman;
				}
			}

			// Token: 0x1700134D RID: 4941
			// (get) Token: 0x060055E3 RID: 21987 RVA: 0x0017E4EA File Offset: 0x0017C6EA
			public static PerkObject SwordForBarter
			{
				get
				{
					return DefaultPerks.Instance._tradeSwordForBarter;
				}
			}

			// Token: 0x1700134E RID: 4942
			// (get) Token: 0x060055E4 RID: 21988 RVA: 0x0017E4F6 File Offset: 0x0017C6F6
			public static PerkObject SelfMadeMan
			{
				get
				{
					return DefaultPerks.Instance._tradeSelfMadeMan;
				}
			}

			// Token: 0x1700134F RID: 4943
			// (get) Token: 0x060055E5 RID: 21989 RVA: 0x0017E502 File Offset: 0x0017C702
			public static PerkObject SilverTongue
			{
				get
				{
					return DefaultPerks.Instance._tradeSilverTongue;
				}
			}

			// Token: 0x17001350 RID: 4944
			// (get) Token: 0x060055E6 RID: 21990 RVA: 0x0017E50E File Offset: 0x0017C70E
			public static PerkObject SpringOfGold
			{
				get
				{
					return DefaultPerks.Instance._tradeSpringOfGold;
				}
			}

			// Token: 0x17001351 RID: 4945
			// (get) Token: 0x060055E7 RID: 21991 RVA: 0x0017E51A File Offset: 0x0017C71A
			public static PerkObject ManOfMeans
			{
				get
				{
					return DefaultPerks.Instance._tradeManOfMeans;
				}
			}

			// Token: 0x17001352 RID: 4946
			// (get) Token: 0x060055E8 RID: 21992 RVA: 0x0017E526 File Offset: 0x0017C726
			public static PerkObject TrickleDown
			{
				get
				{
					return DefaultPerks.Instance._tradeTrickleDown;
				}
			}

			// Token: 0x17001353 RID: 4947
			// (get) Token: 0x060055E9 RID: 21993 RVA: 0x0017E532 File Offset: 0x0017C732
			public static PerkObject EverythingHasAPrice
			{
				get
				{
					return DefaultPerks.Instance._tradeEverythingHasAPrice;
				}
			}
		}

		// Token: 0x02000696 RID: 1686
		public static class Steward
		{
			// Token: 0x17001354 RID: 4948
			// (get) Token: 0x060055EA RID: 21994 RVA: 0x0017E53E File Offset: 0x0017C73E
			public static PerkObject WarriorsDiet
			{
				get
				{
					return DefaultPerks.Instance._stewardWarriorsDiet;
				}
			}

			// Token: 0x17001355 RID: 4949
			// (get) Token: 0x060055EB RID: 21995 RVA: 0x0017E54A File Offset: 0x0017C74A
			public static PerkObject Frugal
			{
				get
				{
					return DefaultPerks.Instance._stewardFrugal;
				}
			}

			// Token: 0x17001356 RID: 4950
			// (get) Token: 0x060055EC RID: 21996 RVA: 0x0017E556 File Offset: 0x0017C756
			public static PerkObject SevenVeterans
			{
				get
				{
					return DefaultPerks.Instance._stewardSevenVeterans;
				}
			}

			// Token: 0x17001357 RID: 4951
			// (get) Token: 0x060055ED RID: 21997 RVA: 0x0017E562 File Offset: 0x0017C762
			public static PerkObject DrillSergant
			{
				get
				{
					return DefaultPerks.Instance._stewardDrillSergant;
				}
			}

			// Token: 0x17001358 RID: 4952
			// (get) Token: 0x060055EE RID: 21998 RVA: 0x0017E56E File Offset: 0x0017C76E
			public static PerkObject Sweatshops
			{
				get
				{
					return DefaultPerks.Instance._stewardSweatshops;
				}
			}

			// Token: 0x17001359 RID: 4953
			// (get) Token: 0x060055EF RID: 21999 RVA: 0x0017E57A File Offset: 0x0017C77A
			public static PerkObject StiffUpperLip
			{
				get
				{
					return DefaultPerks.Instance._stewardStiffUpperLip;
				}
			}

			// Token: 0x1700135A RID: 4954
			// (get) Token: 0x060055F0 RID: 22000 RVA: 0x0017E586 File Offset: 0x0017C786
			public static PerkObject PaidInPromise
			{
				get
				{
					return DefaultPerks.Instance._stewardPaidInPromise;
				}
			}

			// Token: 0x1700135B RID: 4955
			// (get) Token: 0x060055F1 RID: 22001 RVA: 0x0017E592 File Offset: 0x0017C792
			public static PerkObject EfficientCampaigner
			{
				get
				{
					return DefaultPerks.Instance._stewardEfficientCampaigner;
				}
			}

			// Token: 0x1700135C RID: 4956
			// (get) Token: 0x060055F2 RID: 22002 RVA: 0x0017E59E File Offset: 0x0017C79E
			public static PerkObject GivingHands
			{
				get
				{
					return DefaultPerks.Instance._stewardGivingHands;
				}
			}

			// Token: 0x1700135D RID: 4957
			// (get) Token: 0x060055F3 RID: 22003 RVA: 0x0017E5AA File Offset: 0x0017C7AA
			public static PerkObject Logistician
			{
				get
				{
					return DefaultPerks.Instance._stewardLogistician;
				}
			}

			// Token: 0x1700135E RID: 4958
			// (get) Token: 0x060055F4 RID: 22004 RVA: 0x0017E5B6 File Offset: 0x0017C7B6
			public static PerkObject Relocation
			{
				get
				{
					return DefaultPerks.Instance._stewardRelocation;
				}
			}

			// Token: 0x1700135F RID: 4959
			// (get) Token: 0x060055F5 RID: 22005 RVA: 0x0017E5C2 File Offset: 0x0017C7C2
			public static PerkObject AidCorps
			{
				get
				{
					return DefaultPerks.Instance._stewardAidCorps;
				}
			}

			// Token: 0x17001360 RID: 4960
			// (get) Token: 0x060055F6 RID: 22006 RVA: 0x0017E5CE File Offset: 0x0017C7CE
			public static PerkObject Gourmet
			{
				get
				{
					return DefaultPerks.Instance._stewardGourmet;
				}
			}

			// Token: 0x17001361 RID: 4961
			// (get) Token: 0x060055F7 RID: 22007 RVA: 0x0017E5DA File Offset: 0x0017C7DA
			public static PerkObject SoundReserves
			{
				get
				{
					return DefaultPerks.Instance._stewardSoundReserves;
				}
			}

			// Token: 0x17001362 RID: 4962
			// (get) Token: 0x060055F8 RID: 22008 RVA: 0x0017E5E6 File Offset: 0x0017C7E6
			public static PerkObject ForcedLabor
			{
				get
				{
					return DefaultPerks.Instance._stewardForcedLabor;
				}
			}

			// Token: 0x17001363 RID: 4963
			// (get) Token: 0x060055F9 RID: 22009 RVA: 0x0017E5F2 File Offset: 0x0017C7F2
			public static PerkObject Contractors
			{
				get
				{
					return DefaultPerks.Instance._stewardContractors;
				}
			}

			// Token: 0x17001364 RID: 4964
			// (get) Token: 0x060055FA RID: 22010 RVA: 0x0017E5FE File Offset: 0x0017C7FE
			public static PerkObject ArenicosMules
			{
				get
				{
					return DefaultPerks.Instance._stewardArenicosMules;
				}
			}

			// Token: 0x17001365 RID: 4965
			// (get) Token: 0x060055FB RID: 22011 RVA: 0x0017E60A File Offset: 0x0017C80A
			public static PerkObject ArenicosHorses
			{
				get
				{
					return DefaultPerks.Instance._stewardArenicosHorses;
				}
			}

			// Token: 0x17001366 RID: 4966
			// (get) Token: 0x060055FC RID: 22012 RVA: 0x0017E616 File Offset: 0x0017C816
			public static PerkObject MasterOfPlanning
			{
				get
				{
					return DefaultPerks.Instance._stewardMasterOfPlanning;
				}
			}

			// Token: 0x17001367 RID: 4967
			// (get) Token: 0x060055FD RID: 22013 RVA: 0x0017E622 File Offset: 0x0017C822
			public static PerkObject MasterOfWarcraft
			{
				get
				{
					return DefaultPerks.Instance._stewardMasterOfWarcraft;
				}
			}

			// Token: 0x17001368 RID: 4968
			// (get) Token: 0x060055FE RID: 22014 RVA: 0x0017E62E File Offset: 0x0017C82E
			public static PerkObject PriceOfLoyalty
			{
				get
				{
					return DefaultPerks.Instance._stewardPriceOfLoyalty;
				}
			}
		}

		// Token: 0x02000697 RID: 1687
		public static class Medicine
		{
			// Token: 0x17001369 RID: 4969
			// (get) Token: 0x060055FF RID: 22015 RVA: 0x0017E63A File Offset: 0x0017C83A
			public static PerkObject SelfMedication
			{
				get
				{
					return DefaultPerks.Instance._medicineSelfMedication;
				}
			}

			// Token: 0x1700136A RID: 4970
			// (get) Token: 0x06005600 RID: 22016 RVA: 0x0017E646 File Offset: 0x0017C846
			public static PerkObject PreventiveMedicine
			{
				get
				{
					return DefaultPerks.Instance._medicinePreventiveMedicine;
				}
			}

			// Token: 0x1700136B RID: 4971
			// (get) Token: 0x06005601 RID: 22017 RVA: 0x0017E652 File Offset: 0x0017C852
			public static PerkObject TriageTent
			{
				get
				{
					return DefaultPerks.Instance._medicineTriageTent;
				}
			}

			// Token: 0x1700136C RID: 4972
			// (get) Token: 0x06005602 RID: 22018 RVA: 0x0017E65E File Offset: 0x0017C85E
			public static PerkObject WalkItOff
			{
				get
				{
					return DefaultPerks.Instance._medicineWalkItOff;
				}
			}

			// Token: 0x1700136D RID: 4973
			// (get) Token: 0x06005603 RID: 22019 RVA: 0x0017E66A File Offset: 0x0017C86A
			public static PerkObject Sledges
			{
				get
				{
					return DefaultPerks.Instance._medicineSledges;
				}
			}

			// Token: 0x1700136E RID: 4974
			// (get) Token: 0x06005604 RID: 22020 RVA: 0x0017E676 File Offset: 0x0017C876
			public static PerkObject DoctorsOath
			{
				get
				{
					return DefaultPerks.Instance._medicineDoctorsOath;
				}
			}

			// Token: 0x1700136F RID: 4975
			// (get) Token: 0x06005605 RID: 22021 RVA: 0x0017E682 File Offset: 0x0017C882
			public static PerkObject BestMedicine
			{
				get
				{
					return DefaultPerks.Instance._medicineBestMedicine;
				}
			}

			// Token: 0x17001370 RID: 4976
			// (get) Token: 0x06005606 RID: 22022 RVA: 0x0017E68E File Offset: 0x0017C88E
			public static PerkObject GoodLogdings
			{
				get
				{
					return DefaultPerks.Instance._medicineGoodLodging;
				}
			}

			// Token: 0x17001371 RID: 4977
			// (get) Token: 0x06005607 RID: 22023 RVA: 0x0017E69A File Offset: 0x0017C89A
			public static PerkObject SiegeMedic
			{
				get
				{
					return DefaultPerks.Instance._medicineSiegeMedic;
				}
			}

			// Token: 0x17001372 RID: 4978
			// (get) Token: 0x06005608 RID: 22024 RVA: 0x0017E6A6 File Offset: 0x0017C8A6
			public static PerkObject Veterinarian
			{
				get
				{
					return DefaultPerks.Instance._medicineVeterinarian;
				}
			}

			// Token: 0x17001373 RID: 4979
			// (get) Token: 0x06005609 RID: 22025 RVA: 0x0017E6B2 File Offset: 0x0017C8B2
			public static PerkObject PristineStreets
			{
				get
				{
					return DefaultPerks.Instance._medicinePristineStreets;
				}
			}

			// Token: 0x17001374 RID: 4980
			// (get) Token: 0x0600560A RID: 22026 RVA: 0x0017E6BE File Offset: 0x0017C8BE
			public static PerkObject BushDoctor
			{
				get
				{
					return DefaultPerks.Instance._medicineBushDoctor;
				}
			}

			// Token: 0x17001375 RID: 4981
			// (get) Token: 0x0600560B RID: 22027 RVA: 0x0017E6CA File Offset: 0x0017C8CA
			public static PerkObject PerfectHealth
			{
				get
				{
					return DefaultPerks.Instance._medicinePerfectHealth;
				}
			}

			// Token: 0x17001376 RID: 4982
			// (get) Token: 0x0600560C RID: 22028 RVA: 0x0017E6D6 File Offset: 0x0017C8D6
			public static PerkObject HealthAdvise
			{
				get
				{
					return DefaultPerks.Instance._medicineHealthAdvise;
				}
			}

			// Token: 0x17001377 RID: 4983
			// (get) Token: 0x0600560D RID: 22029 RVA: 0x0017E6E2 File Offset: 0x0017C8E2
			public static PerkObject PhysicianOfPeople
			{
				get
				{
					return DefaultPerks.Instance._medicinePhysicianOfPeople;
				}
			}

			// Token: 0x17001378 RID: 4984
			// (get) Token: 0x0600560E RID: 22030 RVA: 0x0017E6EE File Offset: 0x0017C8EE
			public static PerkObject CleanInfrastructure
			{
				get
				{
					return DefaultPerks.Instance._medicineCleanInfrastructure;
				}
			}

			// Token: 0x17001379 RID: 4985
			// (get) Token: 0x0600560F RID: 22031 RVA: 0x0017E6FA File Offset: 0x0017C8FA
			public static PerkObject CheatDeath
			{
				get
				{
					return DefaultPerks.Instance._medicineCheatDeath;
				}
			}

			// Token: 0x1700137A RID: 4986
			// (get) Token: 0x06005610 RID: 22032 RVA: 0x0017E706 File Offset: 0x0017C906
			public static PerkObject FortitudeTonic
			{
				get
				{
					return DefaultPerks.Instance._medicineFortitudeTonic;
				}
			}

			// Token: 0x1700137B RID: 4987
			// (get) Token: 0x06005611 RID: 22033 RVA: 0x0017E712 File Offset: 0x0017C912
			public static PerkObject HelpingHands
			{
				get
				{
					return DefaultPerks.Instance._medicineHelpingHands;
				}
			}

			// Token: 0x1700137C RID: 4988
			// (get) Token: 0x06005612 RID: 22034 RVA: 0x0017E71E File Offset: 0x0017C91E
			public static PerkObject BattleHardened
			{
				get
				{
					return DefaultPerks.Instance._medicineBattleHardened;
				}
			}

			// Token: 0x1700137D RID: 4989
			// (get) Token: 0x06005613 RID: 22035 RVA: 0x0017E72A File Offset: 0x0017C92A
			public static PerkObject MinisterOfHealth
			{
				get
				{
					return DefaultPerks.Instance._medicineMinisterOfHealth;
				}
			}
		}

		// Token: 0x02000698 RID: 1688
		public static class Engineering
		{
			// Token: 0x1700137E RID: 4990
			// (get) Token: 0x06005614 RID: 22036 RVA: 0x0017E736 File Offset: 0x0017C936
			public static PerkObject Scaffolds
			{
				get
				{
					return DefaultPerks.Instance._engineeringScaffolds;
				}
			}

			// Token: 0x1700137F RID: 4991
			// (get) Token: 0x06005615 RID: 22037 RVA: 0x0017E742 File Offset: 0x0017C942
			public static PerkObject TorsionEngines
			{
				get
				{
					return DefaultPerks.Instance._engineeringTorsionEngines;
				}
			}

			// Token: 0x17001380 RID: 4992
			// (get) Token: 0x06005616 RID: 22038 RVA: 0x0017E74E File Offset: 0x0017C94E
			public static PerkObject SiegeWorks
			{
				get
				{
					return DefaultPerks.Instance._engineeringSiegeWorks;
				}
			}

			// Token: 0x17001381 RID: 4993
			// (get) Token: 0x06005617 RID: 22039 RVA: 0x0017E75A File Offset: 0x0017C95A
			public static PerkObject DungeonArchitect
			{
				get
				{
					return DefaultPerks.Instance._engineeringDungeonArchitect;
				}
			}

			// Token: 0x17001382 RID: 4994
			// (get) Token: 0x06005618 RID: 22040 RVA: 0x0017E766 File Offset: 0x0017C966
			public static PerkObject Carpenters
			{
				get
				{
					return DefaultPerks.Instance._engineeringCarpenters;
				}
			}

			// Token: 0x17001383 RID: 4995
			// (get) Token: 0x06005619 RID: 22041 RVA: 0x0017E772 File Offset: 0x0017C972
			public static PerkObject MilitaryPlanner
			{
				get
				{
					return DefaultPerks.Instance._engineeringMilitaryPlanner;
				}
			}

			// Token: 0x17001384 RID: 4996
			// (get) Token: 0x0600561A RID: 22042 RVA: 0x0017E77E File Offset: 0x0017C97E
			public static PerkObject WallBreaker
			{
				get
				{
					return DefaultPerks.Instance._engineeringWallBreaker;
				}
			}

			// Token: 0x17001385 RID: 4997
			// (get) Token: 0x0600561B RID: 22043 RVA: 0x0017E78A File Offset: 0x0017C98A
			public static PerkObject DreadfulSieger
			{
				get
				{
					return DefaultPerks.Instance._engineeringDreadfulSieger;
				}
			}

			// Token: 0x17001386 RID: 4998
			// (get) Token: 0x0600561C RID: 22044 RVA: 0x0017E796 File Offset: 0x0017C996
			public static PerkObject Salvager
			{
				get
				{
					return DefaultPerks.Instance._engineeringSalvager;
				}
			}

			// Token: 0x17001387 RID: 4999
			// (get) Token: 0x0600561D RID: 22045 RVA: 0x0017E7A2 File Offset: 0x0017C9A2
			public static PerkObject Foreman
			{
				get
				{
					return DefaultPerks.Instance._engineeringForeman;
				}
			}

			// Token: 0x17001388 RID: 5000
			// (get) Token: 0x0600561E RID: 22046 RVA: 0x0017E7AE File Offset: 0x0017C9AE
			public static PerkObject Stonecutters
			{
				get
				{
					return DefaultPerks.Instance._engineeringStonecutters;
				}
			}

			// Token: 0x17001389 RID: 5001
			// (get) Token: 0x0600561F RID: 22047 RVA: 0x0017E7BA File Offset: 0x0017C9BA
			public static PerkObject SiegeEngineer
			{
				get
				{
					return DefaultPerks.Instance._engineeringSiegeEngineer;
				}
			}

			// Token: 0x1700138A RID: 5002
			// (get) Token: 0x06005620 RID: 22048 RVA: 0x0017E7C6 File Offset: 0x0017C9C6
			public static PerkObject CampBuilding
			{
				get
				{
					return DefaultPerks.Instance._engineeringCampBuilding;
				}
			}

			// Token: 0x1700138B RID: 5003
			// (get) Token: 0x06005621 RID: 22049 RVA: 0x0017E7D2 File Offset: 0x0017C9D2
			public static PerkObject Battlements
			{
				get
				{
					return DefaultPerks.Instance._engineeringBattlements;
				}
			}

			// Token: 0x1700138C RID: 5004
			// (get) Token: 0x06005622 RID: 22050 RVA: 0x0017E7DE File Offset: 0x0017C9DE
			public static PerkObject EngineeringGuilds
			{
				get
				{
					return DefaultPerks.Instance._engineeringEngineeringGuilds;
				}
			}

			// Token: 0x1700138D RID: 5005
			// (get) Token: 0x06005623 RID: 22051 RVA: 0x0017E7EA File Offset: 0x0017C9EA
			public static PerkObject Apprenticeship
			{
				get
				{
					return DefaultPerks.Instance._engineeringApprenticeship;
				}
			}

			// Token: 0x1700138E RID: 5006
			// (get) Token: 0x06005624 RID: 22052 RVA: 0x0017E7F6 File Offset: 0x0017C9F6
			public static PerkObject Metallurgy
			{
				get
				{
					return DefaultPerks.Instance._engineeringMetallurgy;
				}
			}

			// Token: 0x1700138F RID: 5007
			// (get) Token: 0x06005625 RID: 22053 RVA: 0x0017E802 File Offset: 0x0017CA02
			public static PerkObject ImprovedTools
			{
				get
				{
					return DefaultPerks.Instance._engineeringImprovedTools;
				}
			}

			// Token: 0x17001390 RID: 5008
			// (get) Token: 0x06005626 RID: 22054 RVA: 0x0017E80E File Offset: 0x0017CA0E
			public static PerkObject Clockwork
			{
				get
				{
					return DefaultPerks.Instance._engineeringClockwork;
				}
			}

			// Token: 0x17001391 RID: 5009
			// (get) Token: 0x06005627 RID: 22055 RVA: 0x0017E81A File Offset: 0x0017CA1A
			public static PerkObject ArchitecturalCommisions
			{
				get
				{
					return DefaultPerks.Instance._engineeringArchitecturalCommisions;
				}
			}

			// Token: 0x17001392 RID: 5010
			// (get) Token: 0x06005628 RID: 22056 RVA: 0x0017E826 File Offset: 0x0017CA26
			public static PerkObject Masterwork
			{
				get
				{
					return DefaultPerks.Instance._engineeringMasterwork;
				}
			}
		}
	}
}
