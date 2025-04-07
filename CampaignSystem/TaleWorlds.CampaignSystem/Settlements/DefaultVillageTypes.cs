using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x02000363 RID: 867
	public class DefaultVillageTypes
	{
		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x000D3212 File Offset: 0x000D1412
		private static DefaultVillageTypes Instance
		{
			get
			{
				return Campaign.Current.DefaultVillageTypes;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003298 RID: 12952 RVA: 0x000D321E File Offset: 0x000D141E
		// (set) Token: 0x06003299 RID: 12953 RVA: 0x000D3226 File Offset: 0x000D1426
		public IList<ItemObject> ConsumableRawItems { get; private set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x0600329A RID: 12954 RVA: 0x000D322F File Offset: 0x000D142F
		public static VillageType EuropeHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeEuropeHorseRanch;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x0600329B RID: 12955 RVA: 0x000D323B File Offset: 0x000D143B
		public static VillageType BattanianHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeBattanianHorseRanch;
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x0600329C RID: 12956 RVA: 0x000D3247 File Offset: 0x000D1447
		public static VillageType SturgianHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSturgianHorseRanch;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x0600329D RID: 12957 RVA: 0x000D3253 File Offset: 0x000D1453
		public static VillageType VlandianHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeVlandianHorseRanch;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x0600329E RID: 12958 RVA: 0x000D325F File Offset: 0x000D145F
		public static VillageType SteppeHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSteppeHorseRanch;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x0600329F RID: 12959 RVA: 0x000D326B File Offset: 0x000D146B
		public static VillageType DesertHorseRanch
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeDesertHorseRanch;
			}
		}

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x000D3277 File Offset: 0x000D1477
		public static VillageType WheatFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeWheatFarm;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x060032A1 RID: 12961 RVA: 0x000D3283 File Offset: 0x000D1483
		public static VillageType Lumberjack
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeLumberjack;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x000D328F File Offset: 0x000D148F
		public static VillageType ClayMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeClayMine;
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x000D329B File Offset: 0x000D149B
		public static VillageType SaltMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSaltMine;
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x000D32A7 File Offset: 0x000D14A7
		public static VillageType IronMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeIronMine;
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x000D32B3 File Offset: 0x000D14B3
		public static VillageType Fisherman
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeFisherman;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x060032A6 RID: 12966 RVA: 0x000D32BF File Offset: 0x000D14BF
		public static VillageType CattleRange
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeCattleRange;
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x000D32CB File Offset: 0x000D14CB
		public static VillageType SheepFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSheepFarm;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x000D32D7 File Offset: 0x000D14D7
		public static VillageType HogFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeHogFarm;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x060032A9 RID: 12969 RVA: 0x000D32E3 File Offset: 0x000D14E3
		public static VillageType VineYard
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeVineYard;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x060032AA RID: 12970 RVA: 0x000D32EF File Offset: 0x000D14EF
		public static VillageType FlaxPlant
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeFlaxPlant;
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x060032AB RID: 12971 RVA: 0x000D32FB File Offset: 0x000D14FB
		public static VillageType DateFarm
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeDateFarm;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x060032AC RID: 12972 RVA: 0x000D3307 File Offset: 0x000D1507
		public static VillageType OliveTrees
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeOliveTrees;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x060032AD RID: 12973 RVA: 0x000D3313 File Offset: 0x000D1513
		public static VillageType SilkPlant
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSilkPlant;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x000D331F File Offset: 0x000D151F
		public static VillageType SilverMine
		{
			get
			{
				return DefaultVillageTypes.Instance.VillageTypeSilverMine;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x000D332B File Offset: 0x000D152B
		// (set) Token: 0x060032B0 RID: 12976 RVA: 0x000D3333 File Offset: 0x000D1533
		internal VillageType VillageTypeEuropeHorseRanch { get; private set; }

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x000D333C File Offset: 0x000D153C
		// (set) Token: 0x060032B2 RID: 12978 RVA: 0x000D3344 File Offset: 0x000D1544
		internal VillageType VillageTypeBattanianHorseRanch { get; private set; }

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x000D334D File Offset: 0x000D154D
		// (set) Token: 0x060032B4 RID: 12980 RVA: 0x000D3355 File Offset: 0x000D1555
		internal VillageType VillageTypeSturgianHorseRanch { get; private set; }

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x000D335E File Offset: 0x000D155E
		// (set) Token: 0x060032B6 RID: 12982 RVA: 0x000D3366 File Offset: 0x000D1566
		internal VillageType VillageTypeVlandianHorseRanch { get; private set; }

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x060032B7 RID: 12983 RVA: 0x000D336F File Offset: 0x000D156F
		// (set) Token: 0x060032B8 RID: 12984 RVA: 0x000D3377 File Offset: 0x000D1577
		internal VillageType VillageTypeSteppeHorseRanch { get; private set; }

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x000D3380 File Offset: 0x000D1580
		// (set) Token: 0x060032BA RID: 12986 RVA: 0x000D3388 File Offset: 0x000D1588
		internal VillageType VillageTypeDesertHorseRanch { get; private set; }

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x000D3391 File Offset: 0x000D1591
		// (set) Token: 0x060032BC RID: 12988 RVA: 0x000D3399 File Offset: 0x000D1599
		internal VillageType VillageTypeWheatFarm { get; private set; }

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x060032BD RID: 12989 RVA: 0x000D33A2 File Offset: 0x000D15A2
		// (set) Token: 0x060032BE RID: 12990 RVA: 0x000D33AA File Offset: 0x000D15AA
		internal VillageType VillageTypeLumberjack { get; private set; }

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x060032BF RID: 12991 RVA: 0x000D33B3 File Offset: 0x000D15B3
		// (set) Token: 0x060032C0 RID: 12992 RVA: 0x000D33BB File Offset: 0x000D15BB
		internal VillageType VillageTypeClayMine { get; private set; }

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x060032C1 RID: 12993 RVA: 0x000D33C4 File Offset: 0x000D15C4
		// (set) Token: 0x060032C2 RID: 12994 RVA: 0x000D33CC File Offset: 0x000D15CC
		internal VillageType VillageTypeSaltMine { get; private set; }

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x060032C3 RID: 12995 RVA: 0x000D33D5 File Offset: 0x000D15D5
		// (set) Token: 0x060032C4 RID: 12996 RVA: 0x000D33DD File Offset: 0x000D15DD
		internal VillageType VillageTypeIronMine { get; private set; }

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x060032C5 RID: 12997 RVA: 0x000D33E6 File Offset: 0x000D15E6
		// (set) Token: 0x060032C6 RID: 12998 RVA: 0x000D33EE File Offset: 0x000D15EE
		internal VillageType VillageTypeFisherman { get; private set; }

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x060032C7 RID: 12999 RVA: 0x000D33F7 File Offset: 0x000D15F7
		// (set) Token: 0x060032C8 RID: 13000 RVA: 0x000D33FF File Offset: 0x000D15FF
		internal VillageType VillageTypeCattleRange { get; private set; }

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x060032C9 RID: 13001 RVA: 0x000D3408 File Offset: 0x000D1608
		// (set) Token: 0x060032CA RID: 13002 RVA: 0x000D3410 File Offset: 0x000D1610
		internal VillageType VillageTypeSheepFarm { get; private set; }

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x000D3419 File Offset: 0x000D1619
		// (set) Token: 0x060032CC RID: 13004 RVA: 0x000D3421 File Offset: 0x000D1621
		internal VillageType VillageTypeHogFarm { get; private set; }

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x000D342A File Offset: 0x000D162A
		// (set) Token: 0x060032CE RID: 13006 RVA: 0x000D3432 File Offset: 0x000D1632
		internal VillageType VillageTypeTrapper { get; private set; }

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x060032CF RID: 13007 RVA: 0x000D343B File Offset: 0x000D163B
		// (set) Token: 0x060032D0 RID: 13008 RVA: 0x000D3443 File Offset: 0x000D1643
		internal VillageType VillageTypeVineYard { get; private set; }

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x000D344C File Offset: 0x000D164C
		// (set) Token: 0x060032D2 RID: 13010 RVA: 0x000D3454 File Offset: 0x000D1654
		internal VillageType VillageTypeFlaxPlant { get; private set; }

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x060032D3 RID: 13011 RVA: 0x000D345D File Offset: 0x000D165D
		// (set) Token: 0x060032D4 RID: 13012 RVA: 0x000D3465 File Offset: 0x000D1665
		internal VillageType VillageTypeDateFarm { get; private set; }

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x060032D5 RID: 13013 RVA: 0x000D346E File Offset: 0x000D166E
		// (set) Token: 0x060032D6 RID: 13014 RVA: 0x000D3476 File Offset: 0x000D1676
		internal VillageType VillageTypeOliveTrees { get; private set; }

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x060032D7 RID: 13015 RVA: 0x000D347F File Offset: 0x000D167F
		// (set) Token: 0x060032D8 RID: 13016 RVA: 0x000D3487 File Offset: 0x000D1687
		internal VillageType VillageTypeSilkPlant { get; private set; }

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x000D3490 File Offset: 0x000D1690
		// (set) Token: 0x060032DA RID: 13018 RVA: 0x000D3498 File Offset: 0x000D1698
		internal VillageType VillageTypeSilverMine { get; private set; }

		// Token: 0x060032DB RID: 13019 RVA: 0x000D34A1 File Offset: 0x000D16A1
		public DefaultVillageTypes()
		{
			this.ConsumableRawItems = new List<ItemObject>();
			this.RegisterAll();
			this.AddProductions();
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x000D34C0 File Offset: 0x000D16C0
		private void RegisterAll()
		{
			this.VillageTypeWheatFarm = this.Create("wheat_farm");
			this.VillageTypeEuropeHorseRanch = this.Create("europe_horse_ranch");
			this.VillageTypeSteppeHorseRanch = this.Create("steppe_horse_ranch");
			this.VillageTypeDesertHorseRanch = this.Create("desert_horse_ranch");
			this.VillageTypeBattanianHorseRanch = this.Create("battanian_horse_ranch");
			this.VillageTypeSturgianHorseRanch = this.Create("sturgian_horse_ranch");
			this.VillageTypeVlandianHorseRanch = this.Create("vlandian_horse_ranch");
			this.VillageTypeLumberjack = this.Create("lumberjack");
			this.VillageTypeClayMine = this.Create("clay_mine");
			this.VillageTypeSaltMine = this.Create("salt_mine");
			this.VillageTypeIronMine = this.Create("iron_mine");
			this.VillageTypeFisherman = this.Create("fisherman");
			this.VillageTypeCattleRange = this.Create("cattle_farm");
			this.VillageTypeSheepFarm = this.Create("sheep_farm");
			this.VillageTypeHogFarm = this.Create("swine_farm");
			this.VillageTypeVineYard = this.Create("vineyard");
			this.VillageTypeFlaxPlant = this.Create("flax_plant");
			this.VillageTypeDateFarm = this.Create("date_farm");
			this.VillageTypeOliveTrees = this.Create("olive_trees");
			this.VillageTypeSilkPlant = this.Create("silk_plant");
			this.VillageTypeSilverMine = this.Create("silver_mine");
			this.VillageTypeTrapper = this.Create("trapper");
			this.InitializeAll();
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x000D3649 File Offset: 0x000D1849
		private VillageType Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<VillageType>(new VillageType(stringId));
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x000D3660 File Offset: 0x000D1860
		private void InitializeAll()
		{
			this.VillageTypeWheatFarm.Initialize(new TextObject("{=BPPG2XF7}Wheat Farm", null), "wheat_farm", "wheat_farm_ucon", "wheat_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 50f)
			});
			this.VillageTypeEuropeHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "europe_horse_ranch", "ranch_ucon", "europe_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSteppeHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "steppe_horse_ranch", "ranch_ucon", "steppe_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeDesertHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "desert_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeBattanianHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "battanian_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSturgianHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "sturgian_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeVlandianHorseRanch.Initialize(new TextObject("{=eEh752CZ}Horse Farm", null), "vlandian_horse_ranch", "ranch_ucon", "desert_horse_ranch_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeLumberjack.Initialize(new TextObject("{=YYl1W2jU}Forester", null), "lumberjack", "lumberjack_ucon", "lumberjack_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeClayMine.Initialize(new TextObject("{=myuzMhOn}Clay Pits", null), "clay_mine", "clay_mine_ucon", "clay_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSaltMine.Initialize(new TextObject("{=3aOIY6wl}Salt Mine", null), "salt_mine", "salt_mine_ucon", "salt_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeIronMine.Initialize(new TextObject("{=rHcVKSbA}Iron Mine", null), "iron_mine", "iron_mine_ucon", "iron_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeFisherman.Initialize(new TextObject("{=XpREJNHD}Fishers", null), "fisherman", "fisherman_ucon", "fisherman_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeCattleRange.Initialize(new TextObject("{=bW3csuSZ}Cattle Farms", null), "cattle_farm", "ranch_ucon", "cattle_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSheepFarm.Initialize(new TextObject("{=QbKbGu2h}Sheep Farms", null), "sheep_farm", "ranch_ucon", "sheep_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeHogFarm.Initialize(new TextObject("{=vqSHB7mJ}Swine Farm", null), "swine_farm", "swine_farm_ucon", "swine_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeVineYard.Initialize(new TextObject("{=ZtxWTS9V}Vineyard", null), "vineyard", "vineyard_ucon", "vineyard_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeFlaxPlant.Initialize(new TextObject("{=Z8ntYx0Y}Flax Field", null), "flax_plant", "flax_plant_ucon", "flax_plant_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeDateFarm.Initialize(new TextObject("{=2NR2E663}Palm Orchard", null), "date_farm", "date_farm_ucon", "date_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeOliveTrees.Initialize(new TextObject("{=ewrkbwI9}Olive Trees", null), "date_farm", "date_farm_ucon", "date_farm_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSilkPlant.Initialize(new TextObject("{=wTyq7LaM}Silkworm Farm", null), "silk_plant", "silk_plant_ucon", "silk_plant_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeSilverMine.Initialize(new TextObject("{=aJLQz9iZ}Silver Mine", null), "silver_mine", "silver_mine_ucon", "silver_mine_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
			this.VillageTypeTrapper.Initialize(new TextObject("{=RREyouKr}Trapper", null), "trapper", "trapper_ucon", "trapper_burned", new ValueTuple<ItemObject, float>[]
			{
				new ValueTuple<ItemObject, float>(DefaultItems.Grain, 3f)
			});
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x000D3C1C File Offset: 0x000D1E1C
		private void AddProductions()
		{
			this.AddProductions(this.VillageTypeWheatFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("cow", 0.2f),
				new ValueTuple<string, float>("sheep", 0.4f),
				new ValueTuple<string, float>("hog", 0.8f)
			});
			this.AddProductions(this.VillageTypeEuropeHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("empire_horse", 2.1f),
				new ValueTuple<string, float>("t2_empire_horse", 0.5f),
				new ValueTuple<string, float>("t3_empire_horse", 0.07f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeSturgianHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("sturgia_horse", 2.5f),
				new ValueTuple<string, float>("t2_sturgia_horse", 0.7f),
				new ValueTuple<string, float>("t3_sturgia_horse", 0.1f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeVlandianHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("vlandia_horse", 2.1f),
				new ValueTuple<string, float>("t2_vlandia_horse", 0.4f),
				new ValueTuple<string, float>("t3_vlandia_horse", 0.08f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeBattanianHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("battania_horse", 2.3f),
				new ValueTuple<string, float>("t2_battania_horse", 0.7f),
				new ValueTuple<string, float>("t3_battania_horse", 0.09f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f),
				new ValueTuple<string, float>("saddle_horse", 0.5f),
				new ValueTuple<string, float>("old_horse", 0.5f),
				new ValueTuple<string, float>("hunter", 0.2f),
				new ValueTuple<string, float>("charger", 0.2f)
			});
			this.AddProductions(this.VillageTypeSteppeHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("khuzait_horse", 1.8f),
				new ValueTuple<string, float>("t2_khuzait_horse", 0.4f),
				new ValueTuple<string, float>("t3_khuzait_horse", 0.05f),
				new ValueTuple<string, float>("sumpter_horse", 0.5f),
				new ValueTuple<string, float>("mule", 0.5f)
			});
			this.AddProductions(this.VillageTypeDesertHorseRanch, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("aserai_horse", 1.7f),
				new ValueTuple<string, float>("t2_aserai_horse", 0.3f),
				new ValueTuple<string, float>("t3_aserai_horse", 0.05f),
				new ValueTuple<string, float>("camel", 0.3f),
				new ValueTuple<string, float>("war_camel", 0.08f),
				new ValueTuple<string, float>("pack_camel", 0.3f),
				new ValueTuple<string, float>("sumpter_horse", 0.4f),
				new ValueTuple<string, float>("mule", 0.5f)
			});
			this.AddProductions(this.VillageTypeCattleRange, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("cow", 2f),
				new ValueTuple<string, float>("butter", 4f),
				new ValueTuple<string, float>("cheese", 4f)
			});
			this.AddProductions(this.VillageTypeSheepFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("sheep", 4f),
				new ValueTuple<string, float>("wool", 6f),
				new ValueTuple<string, float>("butter", 2f),
				new ValueTuple<string, float>("cheese", 2f)
			});
			this.AddProductions(this.VillageTypeHogFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("hog", 8f),
				new ValueTuple<string, float>("butter", 2f),
				new ValueTuple<string, float>("cheese", 2f)
			});
			this.AddProductions(this.VillageTypeLumberjack, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("hardwood", 18f)
			});
			this.AddProductions(this.VillageTypeClayMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("clay", 10f)
			});
			this.AddProductions(this.VillageTypeSaltMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("salt", 15f)
			});
			this.AddProductions(this.VillageTypeIronMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("iron", 10f)
			});
			this.AddProductions(this.VillageTypeFisherman, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("fish", 28f)
			});
			this.AddProductions(this.VillageTypeVineYard, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("grape", 11f)
			});
			this.AddProductions(this.VillageTypeFlaxPlant, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("flax", 18f)
			});
			this.AddProductions(this.VillageTypeDateFarm, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("date_fruit", 8f)
			});
			this.AddProductions(this.VillageTypeOliveTrees, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("olives", 12f)
			});
			this.AddProductions(this.VillageTypeSilkPlant, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("cotton", 8f)
			});
			this.AddProductions(this.VillageTypeSilverMine, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("silver", 3f)
			});
			this.AddProductions(this.VillageTypeTrapper, new ValueTuple<string, float>[]
			{
				new ValueTuple<string, float>("fur", 1.4f)
			});
			this.ConsumableRawItems.Add(Game.Current.ObjectManager.GetObject<ItemObject>("grain"));
			this.ConsumableRawItems.Add(Game.Current.ObjectManager.GetObject<ItemObject>("cheese"));
			this.ConsumableRawItems.Add(Game.Current.ObjectManager.GetObject<ItemObject>("butter"));
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x000D4472 File Offset: 0x000D2672
		private void AddProductions(VillageType villageType, ValueTuple<string, float>[] productions)
		{
			villageType.AddProductions(from p in productions
			select new ValueTuple<ItemObject, float>(Game.Current.ObjectManager.GetObject<ItemObject>(p.Item1), p.Item2));
		}
	}
}
