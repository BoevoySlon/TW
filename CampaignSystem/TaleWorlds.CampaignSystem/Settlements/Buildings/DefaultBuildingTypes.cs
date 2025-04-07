using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Settlements.Buildings
{
	// Token: 0x02000373 RID: 883
	public class DefaultBuildingTypes
	{
		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x060033D2 RID: 13266 RVA: 0x000D7009 File Offset: 0x000D5209
		public static IEnumerable<BuildingType> MilitaryBuildings
		{
			get
			{
				yield return DefaultBuildingTypes.Fortifications;
				yield return DefaultBuildingTypes.SettlementGarrisonBarracks;
				yield return DefaultBuildingTypes.SettlementTrainingFields;
				yield return DefaultBuildingTypes.SettlementWorkshop;
				yield return DefaultBuildingTypes.SettlementMilitiaBarracks;
				yield return DefaultBuildingTypes.SettlementSiegeWorkshop;
				yield return DefaultBuildingTypes.Wall;
				yield return DefaultBuildingTypes.CastleBarracks;
				yield return DefaultBuildingTypes.CastleTrainingFields;
				yield return DefaultBuildingTypes.CastleCastallansOffice;
				yield return DefaultBuildingTypes.CastleWorkshop;
				yield return DefaultBuildingTypes.CastleSiegeWorkshop;
				yield return DefaultBuildingTypes.CastleMilitiaBarracks;
				yield return DefaultBuildingTypes.TrainMilitiaDaily;
				yield break;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x000D7012 File Offset: 0x000D5212
		private static DefaultBuildingTypes Instance
		{
			get
			{
				return Campaign.Current.DefaultBuildingTypes;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x000D701E File Offset: 0x000D521E
		public static BuildingType Fortifications
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingFortifications;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060033D5 RID: 13269 RVA: 0x000D702A File Offset: 0x000D522A
		public static BuildingType SettlementGarrisonBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementGarrisonBarracks;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x000D7036 File Offset: 0x000D5236
		public static BuildingType SettlementTrainingFields
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementTrainingFields;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060033D7 RID: 13271 RVA: 0x000D7042 File Offset: 0x000D5242
		public static BuildingType SettlementFairgrounds
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementFairgrounds;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060033D8 RID: 13272 RVA: 0x000D704E File Offset: 0x000D524E
		public static BuildingType SettlementMarketplace
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementMarketplace;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x000D705A File Offset: 0x000D525A
		public static BuildingType SettlementAquaducts
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementAquaducts;
			}
		}

		// Token: 0x17000CB6 RID: 3254
		// (get) Token: 0x060033DA RID: 13274 RVA: 0x000D7066 File Offset: 0x000D5266
		public static BuildingType SettlementForum
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementForum;
			}
		}

		// Token: 0x17000CB7 RID: 3255
		// (get) Token: 0x060033DB RID: 13275 RVA: 0x000D7072 File Offset: 0x000D5272
		public static BuildingType SettlementGranary
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementGranary;
			}
		}

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x060033DC RID: 13276 RVA: 0x000D707E File Offset: 0x000D527E
		public static BuildingType SettlementWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementOrchard;
			}
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x000D708A File Offset: 0x000D528A
		public static BuildingType SettlementMilitiaBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementMilitiaBarracks;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x060033DE RID: 13278 RVA: 0x000D7096 File Offset: 0x000D5296
		public static BuildingType SettlementSiegeWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementSiegeWorkshop;
			}
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x060033DF RID: 13279 RVA: 0x000D70A2 File Offset: 0x000D52A2
		public static BuildingType SettlementLimeKilns
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingSettlementLimeKilns;
			}
		}

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x060033E0 RID: 13280 RVA: 0x000D70AE File Offset: 0x000D52AE
		public static BuildingType Wall
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingWall;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060033E1 RID: 13281 RVA: 0x000D70BA File Offset: 0x000D52BA
		public static BuildingType CastleBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleBarracks;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060033E2 RID: 13282 RVA: 0x000D70C6 File Offset: 0x000D52C6
		public static BuildingType CastleTrainingFields
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleTrainingFields;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060033E3 RID: 13283 RVA: 0x000D70D2 File Offset: 0x000D52D2
		public static BuildingType CastleGranary
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleGranary;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x060033E4 RID: 13284 RVA: 0x000D70DE File Offset: 0x000D52DE
		public static BuildingType CastleGardens
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleGardens;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060033E5 RID: 13285 RVA: 0x000D70EA File Offset: 0x000D52EA
		public static BuildingType CastleCastallansOffice
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleCastallansOffice;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060033E6 RID: 13286 RVA: 0x000D70F6 File Offset: 0x000D52F6
		public static BuildingType CastleWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleWorkshop;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060033E7 RID: 13287 RVA: 0x000D7102 File Offset: 0x000D5302
		public static BuildingType CastleFairgrounds
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleFairgrounds;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060033E8 RID: 13288 RVA: 0x000D710E File Offset: 0x000D530E
		public static BuildingType CastleSiegeWorkshop
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleSiegeWorkshop;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060033E9 RID: 13289 RVA: 0x000D711A File Offset: 0x000D531A
		public static BuildingType CastleMilitiaBarracks
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleMilitiaBarracks;
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x000D7126 File Offset: 0x000D5326
		public static BuildingType CastleTollCollector
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingCastleTollCollector;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x000D7132 File Offset: 0x000D5332
		public static BuildingType BuildHouseDaily
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingDailyBuildHouse;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060033EC RID: 13292 RVA: 0x000D713E File Offset: 0x000D533E
		public static BuildingType TrainMilitiaDaily
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingDailyTrainMilitia;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060033ED RID: 13293 RVA: 0x000D714A File Offset: 0x000D534A
		public static BuildingType FestivalsAndGamesDaily
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingDailyFestivalsAndGames;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060033EE RID: 13294 RVA: 0x000D7156 File Offset: 0x000D5356
		public static BuildingType IrrigationDaily
		{
			get
			{
				return DefaultBuildingTypes.Instance._buildingDailyIrrigation;
			}
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x000D7162 File Offset: 0x000D5362
		public DefaultBuildingTypes()
		{
			this.RegisterAll();
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x000D7170 File Offset: 0x000D5370
		private void RegisterAll()
		{
			this._buildingFortifications = this.Create("building_fortifications");
			this._buildingSettlementGarrisonBarracks = this.Create("building_settlement_garrison_barracks");
			this._buildingSettlementTrainingFields = this.Create("building_settlement_training_fields");
			this._buildingSettlementFairgrounds = this.Create("building_settlement_fairgrounds");
			this._buildingSettlementMarketplace = this.Create("building_settlement_marketplace");
			this._buildingSettlementAquaducts = this.Create("building_settlement_aquaducts");
			this._buildingSettlementForum = this.Create("building_settlement_forum");
			this._buildingSettlementGranary = this.Create("building_settlement_granary");
			this._buildingSettlementOrchard = this.Create("building_settlement_lime_kilns");
			this._buildingSettlementMilitiaBarracks = this.Create("building_settlement_militia_barracks");
			this._buildingSettlementSiegeWorkshop = this.Create("building_siege_workshop");
			this._buildingSettlementLimeKilns = this.Create("building_settlement_workshop");
			this._buildingWall = this.Create("building_wall");
			this._buildingCastleBarracks = this.Create("building_castle_barracks");
			this._buildingCastleTrainingFields = this.Create("building_castle_training_fields");
			this._buildingCastleGranary = this.Create("building_castle_granary");
			this._buildingCastleGardens = this.Create("building_castle_gardens");
			this._buildingCastleCastallansOffice = this.Create("building_castle_castallans_office");
			this._buildingCastleWorkshop = this.Create("building_castle_workshops");
			this._buildingCastleFairgrounds = this.Create("building_castle_fairgrounds");
			this._buildingCastleSiegeWorkshop = this.Create("building_castle_siege_workshop");
			this._buildingCastleMilitiaBarracks = this.Create("building_castle_militia_barracks");
			this._buildingCastleTollCollector = this.Create("building_castle_lime_kilns");
			this._buildingDailyBuildHouse = this.Create("building_daily_build_house");
			this._buildingDailyTrainMilitia = this.Create("building_daily_train_militia");
			this._buildingDailyFestivalsAndGames = this.Create("building_festivals_and_games");
			this._buildingDailyIrrigation = this.Create("building_irrigation");
			this.InitializeAll();
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x000D734E File Offset: 0x000D554E
		private BuildingType Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<BuildingType>(new BuildingType(stringId));
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x000D7368 File Offset: 0x000D5568
		private void InitializeAll()
		{
			this._buildingFortifications.Initialize(new TextObject("{=CVdK1ax1}Fortifications", null), new TextObject("{=dIM6xa2O}Better fortifications and higher walls around town, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[]
			{
				0,
				8000,
				16000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 25f, 50f, 100f)
			}, 1);
			this._buildingSettlementGarrisonBarracks.Initialize(new TextObject("{=54vkRuHo}Garrison Barracks", null), new TextObject("{=DHm1MBsj}Lodging for the garrisoned troops. Each level increases the garrison capacity of the stronghold.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 30f, 60f, 100f)
			}, 0);
			this._buildingSettlementTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=otWlERkc}A field for military drills that increases the daily experience gain of all garrisoned units.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Experience, 1f, 2f, 3f)
			}, 0);
			this._buildingSettlementFairgrounds.Initialize(new TextObject("{=ixHqTrX5}Fairgrounds", null), new TextObject("{=0B91pZ2R}A permanent space that hosts fairs. Citizens can gather, drink dance and socialize,  increasing the daily morale of the settlement.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Loyalty, 0.5f, 1f, 1.5f)
			}, 0);
			this._buildingSettlementMarketplace.Initialize(new TextObject("{=zLdXCpne}Marketplace", null), new TextObject("{=Z9LWA6A3}Scheduled market days lure folks from surrounding villages to the settlement and of course the local ruler takes a handsome cut of any sales. Increases the wealth and tax yield of the settlement.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Tax, 5f, 10f, 15f)
			}, 0);
			this._buildingSettlementAquaducts.Initialize(new TextObject("{=f5jHMbOq}Aqueducts", null), new TextObject("{=UojHRjdG}Access to clean water provides room for growth with healthy citizens and a clean infrastructure. Increases daily Prosperity change.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Prosperity, 0.3f, 0.6f, 1f)
			}, 0);
			this._buildingSettlementForum.Initialize(new TextObject("{=paelEWj1}Forum", null), new TextObject("{=wTBtu1t5}An open square in the settlement where people can meet, spend time, and share their ideas. Increases influence of the settlement owner.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Influence, 0.5f, 1f, 1.5f)
			}, 0);
			this._buildingSettlementGranary.Initialize(new TextObject("{=PstO2f5I}Granary", null), new TextObject("{=aK23T43P}Keeps stockpiles of food so that the settlement has more food supply. Each level increases the local food supply.", null), new int[]
			{
				1000,
				1500,
				2000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Foodstock, 200f, 400f, 600f)
			}, 0);
			this._buildingSettlementLimeKilns.Initialize(new TextObject("{=NbgeKwVr}Workshops", null), new TextObject("{=qR9bEE6g}A building which provides the means required for the manufacture or repair of buildings. Improves project development speed. Also stonemasons reinforce the walls.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Construction, 3f, 6f, 9f)
			}, 0);
			this._buildingSettlementMilitiaBarracks.Initialize(new TextObject("{=l91xAgmU}Militia Grounds", null), new TextObject("{=RliyRJKl}Provides weapons training for citizens. Increases daily militia recruitment.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Militia, 0.5f, 1f, 1.5f)
			}, 0);
			this._buildingSettlementSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=MharAceZ}A workshop dedicated to sieges. Contains tools and materials to repair walls, build and repair siege engines.", null), new int[]
			{
				1000,
				1500,
				2000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.WallRepairSpeed, 50f, 50f, 50f),
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, 30f, 60f, 100f)
			}, 0);
			this._buildingSettlementOrchard.Initialize(new TextObject("{=AkbiPIij}Orchards", null), new TextObject("{=ZCLVOXgM}Fruit trees and vegetable gardens outside the walls provide food as long as there is no siege.", null), new int[]
			{
				2000,
				3000,
				4000
			}, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.FoodProduction, 6f, 12f, 18f)
			}, 0);
			this._buildingWall.Initialize(new TextObject("{=6pNrNj93}Wall", null), new TextObject("{=oS5Nesmi}Better fortifications and higher walls around the keep, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[]
			{
				0,
				2500,
				5000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 25f, 50f, 100f)
			}, 1);
			this._buildingCastleBarracks.Initialize(new TextObject("{=x2B0OjhI}Barracks", null), new TextObject("{=HJ1is924}Lodgings for the garrisoned troops. Increases the garrison capacity of the stronghold.", null), new int[]
			{
				500,
				1000,
				1500
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 30f, 60f, 100f)
			}, 0);
			this._buildingCastleTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=otWlERkc}A field for military drills that increases the daily experience gain of all garrisoned units.", null), new int[]
			{
				500,
				1000,
				1500
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Experience, 1f, 2f, 3f)
			}, 0);
			this._buildingCastleGranary.Initialize(new TextObject("{=PstO2f5I}Granary", null), new TextObject("{=iazij7fO}Keeps stockpiles of food so that the settlement has more food supply. Increases the local food supply.", null), new int[]
			{
				500,
				1000,
				1500
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Foodstock, 100f, 150f, 200f)
			}, 0);
			this._buildingCastleGardens.Initialize(new TextObject("{=yT6XN4Mr}Gardens", null), new TextObject("{=ZCLVOXgM}Fruit trees and vegetable gardens outside the walls provide food as long as there is no siege.", null), new int[]
			{
				500,
				750,
				1000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.FoodProduction, 3f, 6f, 9f)
			}, 0);
			this._buildingCastleCastallansOffice.Initialize(new TextObject("{=kLNnFMR9}Castellan's Office", null), new TextObject("{=GDsI6daq}Provides a warden for the castle who maintains discipline and upholds the law.", null), new int[]
			{
				500,
				750,
				1000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonWageReduce, 10f, 20f, 30f)
			}, 0);
			this._buildingCastleWorkshop.Initialize(new TextObject("{=NbgeKwVr}Workshops", null), new TextObject("{=qR9bEE6g}A building which provides the means required for the manufacture or repair of buildings. Improves project development speed. Also stonemasons reinforce the walls.", null), new int[]
			{
				500,
				750,
				1000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Construction, 1f, 2f, 3f)
			}, 0);
			this._buildingCastleFairgrounds.Initialize(new TextObject("{=ixHqTrX5}Fairgrounds", null), new TextObject("{=QHZeCDJy}A permanent space that hosts fairs. Citizens can gather, drink dance and socialize, increasing the daily morale of the settlement.", null), new int[]
			{
				500,
				750,
				1000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Loyalty, 0.5f, 1f, 1.5f)
			}, 0);
			this._buildingCastleSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=MharAceZ}A workshop dedicated to sieges. Contains tools and materials to repair walls, build and repair siege engines.", null), new int[]
			{
				500,
				750,
				1000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.WallRepairSpeed, 50f, 50f, 50f),
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, 30f, 60f, 100f)
			}, 0);
			this._buildingCastleMilitiaBarracks.Initialize(new TextObject("{=l91xAgmU}Militia Grounds", null), new TextObject("{=YRrx8bAK}Provides weapons training for citizens. Each level increases daily militia recruitment.", null), new int[]
			{
				500,
				750,
				1000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Militia, 1f, 2f, 3f)
			}, 0);
			this._buildingCastleTollCollector.Initialize(new TextObject("{=VawDQKLl}Toll Collector", null), new TextObject("{=ac8PkfhG}Increases tax income from the region", null), new int[]
			{
				500,
				750,
				1000
			}, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Tax, 10f, 20f, 30f)
			}, 0);
			this._buildingDailyBuildHouse.Initialize(new TextObject("{=F4V7oaVx}Housing", null), new TextObject("{=yWXtcxqb}Construct housing so that more folks can settle, increasing population.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.ProsperityDaily, 1f, 1f, 1f)
			}, 0);
			this._buildingDailyTrainMilitia.Initialize(new TextObject("{=p1Y3EU5O}Train Militia", null), new TextObject("{=61J1wa6k}Schedule drills for commoners, increasing militia recruitment.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.MilitiaDaily, 3f, 3f, 3f)
			}, 0);
			this._buildingDailyFestivalsAndGames.Initialize(new TextObject("{=aEmYZadz}Festival and Games", null), new TextObject("{=ovDbQIo9}Organize festivals and games in the settlement, increasing morale.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.LoyaltyDaily, 3f, 3f, 3f)
			}, 0);
			this._buildingDailyIrrigation.Initialize(new TextObject("{=O4cknzhW}Irrigation", null), new TextObject("{=CU9g49fo}Provide irrigation, increasing growth in bound villages.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[]
			{
				new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.VillageDevelopmentDaily, 1f, 1f, 1f)
			}, 0);
		}

		// Token: 0x040010D3 RID: 4307
		public const int MaxBuildingLevel = 3;

		// Token: 0x040010D4 RID: 4308
		private BuildingType _buildingFortifications;

		// Token: 0x040010D5 RID: 4309
		private BuildingType _buildingSettlementGarrisonBarracks;

		// Token: 0x040010D6 RID: 4310
		private BuildingType _buildingSettlementTrainingFields;

		// Token: 0x040010D7 RID: 4311
		private BuildingType _buildingSettlementFairgrounds;

		// Token: 0x040010D8 RID: 4312
		private BuildingType _buildingSettlementMarketplace;

		// Token: 0x040010D9 RID: 4313
		private BuildingType _buildingSettlementAquaducts;

		// Token: 0x040010DA RID: 4314
		private BuildingType _buildingSettlementForum;

		// Token: 0x040010DB RID: 4315
		private BuildingType _buildingSettlementGranary;

		// Token: 0x040010DC RID: 4316
		private BuildingType _buildingSettlementOrchard;

		// Token: 0x040010DD RID: 4317
		private BuildingType _buildingSettlementMilitiaBarracks;

		// Token: 0x040010DE RID: 4318
		private BuildingType _buildingSettlementSiegeWorkshop;

		// Token: 0x040010DF RID: 4319
		private BuildingType _buildingSettlementLimeKilns;

		// Token: 0x040010E0 RID: 4320
		private BuildingType _buildingWall;

		// Token: 0x040010E1 RID: 4321
		private BuildingType _buildingCastleBarracks;

		// Token: 0x040010E2 RID: 4322
		private BuildingType _buildingCastleTrainingFields;

		// Token: 0x040010E3 RID: 4323
		private BuildingType _buildingCastleGranary;

		// Token: 0x040010E4 RID: 4324
		private BuildingType _buildingCastleGardens;

		// Token: 0x040010E5 RID: 4325
		private BuildingType _buildingCastleCastallansOffice;

		// Token: 0x040010E6 RID: 4326
		private BuildingType _buildingCastleWorkshop;

		// Token: 0x040010E7 RID: 4327
		private BuildingType _buildingCastleFairgrounds;

		// Token: 0x040010E8 RID: 4328
		private BuildingType _buildingCastleSiegeWorkshop;

		// Token: 0x040010E9 RID: 4329
		private BuildingType _buildingCastleMilitiaBarracks;

		// Token: 0x040010EA RID: 4330
		private BuildingType _buildingCastleTollCollector;

		// Token: 0x040010EB RID: 4331
		private BuildingType _buildingDailyBuildHouse;

		// Token: 0x040010EC RID: 4332
		private BuildingType _buildingDailyTrainMilitia;

		// Token: 0x040010ED RID: 4333
		private BuildingType _buildingDailyFestivalsAndGames;

		// Token: 0x040010EE RID: 4334
		private BuildingType _buildingDailyIrrigation;
	}
}
