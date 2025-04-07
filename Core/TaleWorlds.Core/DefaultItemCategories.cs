using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200004C RID: 76
	public class DefaultItemCategories
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060005B4 RID: 1460 RVA: 0x000147A9 File Offset: 0x000129A9
		private static DefaultItemCategories Instance
		{
			get
			{
				return Game.Current.DefaultItemCategories;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x000147B5 File Offset: 0x000129B5
		public static ItemCategory Grain
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryGrain;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x000147C1 File Offset: 0x000129C1
		public static ItemCategory Wood
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryWood;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x000147CD File Offset: 0x000129CD
		public static ItemCategory Meat
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryMeat;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x000147D9 File Offset: 0x000129D9
		public static ItemCategory Wool
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryWool;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000147E5 File Offset: 0x000129E5
		public static ItemCategory Cheese
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryCheese;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000147F1 File Offset: 0x000129F1
		public static ItemCategory Iron
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryIron;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x000147FD File Offset: 0x000129FD
		public static ItemCategory Salt
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategorySalt;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00014809 File Offset: 0x00012A09
		public static ItemCategory Silver
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategorySilver;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00014815 File Offset: 0x00012A15
		public static ItemCategory Cotton
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryCotton;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00014821 File Offset: 0x00012A21
		public static ItemCategory Fish
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryFish;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0001482D File Offset: 0x00012A2D
		public static ItemCategory Flax
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryFlax;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060005C0 RID: 1472 RVA: 0x00014839 File Offset: 0x00012A39
		public static ItemCategory Grape
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryGrape;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00014845 File Offset: 0x00012A45
		public static ItemCategory Hides
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHides;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00014851 File Offset: 0x00012A51
		public static ItemCategory Clay
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryClay;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0001485D File Offset: 0x00012A5D
		public static ItemCategory DateFruit
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryDateFruit;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00014869 File Offset: 0x00012A69
		public static ItemCategory Olives
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryOlives;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x00014875 File Offset: 0x00012A75
		public static ItemCategory Beer
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryBeer;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x00014881 File Offset: 0x00012A81
		public static ItemCategory Wine
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryWine;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001488D File Offset: 0x00012A8D
		public static ItemCategory Oil
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryOil;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00014899 File Offset: 0x00012A99
		public static ItemCategory Tools
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryTools;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x000148A5 File Offset: 0x00012AA5
		public static ItemCategory Pottery
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryPottery;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x000148B1 File Offset: 0x00012AB1
		public static ItemCategory Cloth
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryCloth;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x000148BD File Offset: 0x00012ABD
		public static ItemCategory Linen
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryLinen;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x000148C9 File Offset: 0x00012AC9
		public static ItemCategory Leather
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryLeather;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x000148D5 File Offset: 0x00012AD5
		public static ItemCategory Velvet
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryVelvet;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x000148E1 File Offset: 0x00012AE1
		public static ItemCategory Sheep
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategorySheep;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000148ED File Offset: 0x00012AED
		public static ItemCategory Cow
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryCow;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x000148F9 File Offset: 0x00012AF9
		public static ItemCategory Hog
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHog;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00014905 File Offset: 0x00012B05
		public static ItemCategory Horse
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHorse;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x00014911 File Offset: 0x00012B11
		public static ItemCategory WarHorse
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryWarHorse;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x0001491D File Offset: 0x00012B1D
		public static ItemCategory NobleHorse
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryNobleHorse;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x00014929 File Offset: 0x00012B29
		public static ItemCategory Butter
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryButter;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00014935 File Offset: 0x00012B35
		public static ItemCategory Fur
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryFur;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x00014941 File Offset: 0x00012B41
		public static ItemCategory Jewelry
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryJewelry;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001494D File Offset: 0x00012B4D
		public static ItemCategory PackAnimal
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryPackAnimal;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00014959 File Offset: 0x00012B59
		public static ItemCategory MeleeWeapons1
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryMeleeWeapons1;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00014965 File Offset: 0x00012B65
		public static ItemCategory MeleeWeapons2
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryMeleeWeapons2;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x00014971 File Offset: 0x00012B71
		public static ItemCategory MeleeWeapons3
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryMeleeWeapons3;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0001497D File Offset: 0x00012B7D
		public static ItemCategory MeleeWeapons4
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryMeleeWeapons4;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x00014989 File Offset: 0x00012B89
		public static ItemCategory MeleeWeapons5
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryMeleeWeapons5;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x00014995 File Offset: 0x00012B95
		public static ItemCategory RangedWeapons1
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryRangedWeapons1;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x000149A1 File Offset: 0x00012BA1
		public static ItemCategory RangedWeapons2
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryRangedWeapons2;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x000149AD File Offset: 0x00012BAD
		public static ItemCategory RangedWeapons3
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryRangedWeapons3;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x000149B9 File Offset: 0x00012BB9
		public static ItemCategory RangedWeapons4
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryRangedWeapons4;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x000149C5 File Offset: 0x00012BC5
		public static ItemCategory RangedWeapons5
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryRangedWeapons5;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x000149D1 File Offset: 0x00012BD1
		public static ItemCategory Shield1
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryShield;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000149DD File Offset: 0x00012BDD
		public static ItemCategory Shield2
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryShield2;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x000149E9 File Offset: 0x00012BE9
		public static ItemCategory Shield3
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryShield3;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x000149F5 File Offset: 0x00012BF5
		public static ItemCategory Shield4
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryShield4;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00014A01 File Offset: 0x00012C01
		public static ItemCategory Shield5
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryShield5;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00014A0D File Offset: 0x00012C0D
		public static ItemCategory Garment
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryGarment;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00014A19 File Offset: 0x00012C19
		public static ItemCategory LightArmor
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryLightArmor;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00014A25 File Offset: 0x00012C25
		public static ItemCategory MediumArmor
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryMediumArmor;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00014A31 File Offset: 0x00012C31
		public static ItemCategory HeavyArmor
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHeavyArmor;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x00014A3D File Offset: 0x00012C3D
		public static ItemCategory UltraArmor
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryUltraArmor;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00014A49 File Offset: 0x00012C49
		public static ItemCategory HorseEquipment
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHorseEquipment;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00014A55 File Offset: 0x00012C55
		public static ItemCategory HorseEquipment2
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHorseEquipment2;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00014A61 File Offset: 0x00012C61
		public static ItemCategory HorseEquipment3
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHorseEquipment3;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00014A6D File Offset: 0x00012C6D
		public static ItemCategory HorseEquipment4
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHorseEquipment4;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x00014A79 File Offset: 0x00012C79
		public static ItemCategory HorseEquipment5
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryHorseEquipment5;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00014A85 File Offset: 0x00012C85
		public static ItemCategory Arrows
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryArrows;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00014A91 File Offset: 0x00012C91
		public static ItemCategory Banners
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryBanners;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00014A9D File Offset: 0x00012C9D
		public static ItemCategory Unassigned
		{
			get
			{
				return DefaultItemCategories.Instance._itemCategoryUnassigned;
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00014AA9 File Offset: 0x00012CA9
		private ItemCategory Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<ItemCategory>(new ItemCategory(stringId));
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00014AC0 File Offset: 0x00012CC0
		public DefaultItemCategories()
		{
			this.RegisterAll();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00014AD0 File Offset: 0x00012CD0
		private void RegisterAll()
		{
			this._itemCategoryGrain = this.Create("grain");
			this._itemCategoryFish = this.Create("fish");
			this._itemCategoryMeat = this.Create("meat");
			this._itemCategoryCheese = this.Create("cheese");
			this._itemCategoryGrape = this.Create("grape");
			this._itemCategoryDateFruit = this.Create("date_fruit");
			this._itemCategoryOlives = this.Create("olives");
			this._itemCategoryButter = this.Create("butter");
			this._itemCategoryOil = this.Create("oil");
			this._itemCategoryFlax = this.Create("flax");
			this._itemCategoryLinen = this.Create("linen");
			this._itemCategoryWool = this.Create("wool");
			this._itemCategoryCloth = this.Create("cloth");
			this._itemCategoryCotton = this.Create("cotton");
			this._itemCategoryVelvet = this.Create("velvet");
			this._itemCategoryWood = this.Create("hardwood");
			this._itemCategoryIron = this.Create("iron");
			this._itemCategorySalt = this.Create("salt");
			this._itemCategorySilver = this.Create("silver");
			this._itemCategoryHides = this.Create("hides");
			this._itemCategoryClay = this.Create("clay");
			this._itemCategoryBeer = this.Create("beer");
			this._itemCategoryWine = this.Create("wine");
			this._itemCategoryTools = this.Create("tools");
			this._itemCategoryPottery = this.Create("pottery");
			this._itemCategoryLeather = this.Create("leather");
			this._itemCategoryFur = this.Create("fur");
			this._itemCategoryJewelry = this.Create("jewelry");
			this._itemCategoryArrows = this.Create("arrows");
			this._itemCategorySheep = this.Create("sheep");
			this._itemCategoryCow = this.Create("cow");
			this._itemCategoryHog = this.Create("hog");
			this._itemCategoryPackAnimal = this.Create("sumpter_horse");
			this._itemCategoryHorse = this.Create("horse");
			this._itemCategoryWarHorse = this.Create("war_horse");
			this._itemCategoryNobleHorse = this.Create("noble_horse");
			this._itemCategoryHorseEquipment = this.Create("horse_equipment");
			this._itemCategoryHorseEquipment2 = this.Create("horse_equipment_2");
			this._itemCategoryHorseEquipment3 = this.Create("horse_equipment_3");
			this._itemCategoryHorseEquipment4 = this.Create("horse_equipment_4");
			this._itemCategoryHorseEquipment5 = this.Create("horse_equipment_5");
			this._itemCategoryMeleeWeapons1 = this.Create("melee_weapons");
			this._itemCategoryMeleeWeapons2 = this.Create("melee_weapons_2");
			this._itemCategoryMeleeWeapons3 = this.Create("melee_weapons_3");
			this._itemCategoryMeleeWeapons4 = this.Create("melee_weapons_4");
			this._itemCategoryMeleeWeapons5 = this.Create("melee_weapons_5");
			this._itemCategoryRangedWeapons1 = this.Create("ranged_weapons");
			this._itemCategoryRangedWeapons2 = this.Create("ranged_weapons_2");
			this._itemCategoryRangedWeapons3 = this.Create("ranged_weapons_3");
			this._itemCategoryRangedWeapons4 = this.Create("ranged_weapons_4");
			this._itemCategoryRangedWeapons5 = this.Create("ranged_weapons_5");
			this._itemCategoryShield = this.Create("shield");
			this._itemCategoryShield2 = this.Create("shield_2");
			this._itemCategoryShield3 = this.Create("shield_3");
			this._itemCategoryShield4 = this.Create("shield_4");
			this._itemCategoryShield5 = this.Create("shield_5");
			this._itemCategoryGarment = this.Create("garment");
			this._itemCategoryLightArmor = this.Create("light_armor");
			this._itemCategoryMediumArmor = this.Create("medium_armor");
			this._itemCategoryHeavyArmor = this.Create("heavy_armor");
			this._itemCategoryUltraArmor = this.Create("ultra_armor");
			this._itemCategoryBanners = this.Create("banner");
			this._itemCategoryUnassigned = this.Create("unassigned");
			this.InitializeAll();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00014F14 File Offset: 0x00013114
		private void InitializeAll()
		{
			this._itemCategoryGrain.InitializeObject(true, 140, 0, ItemCategory.Property.BonusToFoodStores, null, 0f, false, true);
			this._itemCategoryFish.InitializeObject(true, 15, 15, ItemCategory.Property.BonusToFoodStores, this._itemCategoryGrain, 0.1f, false, true);
			this._itemCategoryMeat.InitializeObject(true, 19, 50, ItemCategory.Property.BonusToFoodStores, this._itemCategoryFish, 0.1f, false, true);
			this._itemCategoryCheese.InitializeObject(true, 10, 20, ItemCategory.Property.BonusToFoodStores, this._itemCategoryGrain, 0.01f, false, true);
			this._itemCategoryButter.InitializeObject(true, 10, 25, ItemCategory.Property.BonusToFoodStores, null, 0f, false, true);
			this._itemCategoryGrape.InitializeObject(true, 5, 20, ItemCategory.Property.BonusToFoodStores, this._itemCategoryGrain, 0.1f, false, true);
			this._itemCategoryOlives.InitializeObject(true, 5, 20, ItemCategory.Property.BonusToFoodStores, this._itemCategoryGrain, 0.01f, false, true);
			this._itemCategoryDateFruit.InitializeObject(true, 7, 32, ItemCategory.Property.BonusToFoodStores, this._itemCategoryGrape, 0.01f, false, true);
			this._itemCategoryOil.InitializeObject(true, 17, 30, ItemCategory.Property.None, this._itemCategoryButter, 0.1f, false, true);
			this._itemCategoryFlax.InitializeObject(true, 10, 20, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryLinen.InitializeObject(true, 20, 25, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryWool.InitializeObject(true, 12, 0, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryCloth.InitializeObject(false, 12, 6, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryCotton.InitializeObject(true, 10, 3, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryVelvet.InitializeObject(true, 15, 32, ItemCategory.Property.BonusToLoyalty, null, 0f, false, true);
			this._itemCategoryWood.InitializeObject(true, 10, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryIron.InitializeObject(true, 10, 20, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategorySalt.InitializeObject(true, 25, 25, ItemCategory.Property.BonusToTax, null, 0f, false, true);
			this._itemCategorySilver.InitializeObject(true, 10, 20, ItemCategory.Property.BonusToTax, null, 0f, false, true);
			this._itemCategoryHides.InitializeObject(true, 30, 15, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryClay.InitializeObject(true, 8, 5, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryBeer.InitializeObject(true, 23, 20, ItemCategory.Property.BonusToFoodStores, null, 0f, false, true);
			this._itemCategoryWine.InitializeObject(true, 15, 30, ItemCategory.Property.BonusToLoyalty, null, 0f, false, true);
			this._itemCategoryTools.InitializeObject(true, 30, 30, ItemCategory.Property.BonusToProduction, null, 0f, false, true);
			this._itemCategoryPottery.InitializeObject(true, 22, 20, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryLeather.InitializeObject(true, 25, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryFur.InitializeObject(true, 10, 38, ItemCategory.Property.BonusToProsperity, null, 0f, false, true);
			this._itemCategoryJewelry.InitializeObject(true, 15, 32, ItemCategory.Property.BonusToLoyalty, null, 0f, false, true);
			this._itemCategorySheep.InitializeObject(true, 8, 0, ItemCategory.Property.None, null, 0f, true, true);
			this._itemCategoryCow.InitializeObject(true, 8, 0, ItemCategory.Property.None, null, 0f, true, true);
			this._itemCategoryHog.InitializeObject(true, 6, 0, ItemCategory.Property.None, null, 0f, true, true);
			this._itemCategoryPackAnimal.InitializeObject(true, 20, 3, ItemCategory.Property.BonusToProsperity, null, 0f, true, true);
			this._itemCategoryHorse.InitializeObject(true, 140, 0, ItemCategory.Property.BonusToProsperity, null, 0f, true, true);
			this._itemCategoryWarHorse.InitializeObject(true, 120, 20, ItemCategory.Property.BonusToGarrison, null, 0f, true, true);
			this._itemCategoryNobleHorse.InitializeObject(false, 120, 50, ItemCategory.Property.BonusToGarrison, null, 0f, true, true);
			this._itemCategoryArrows.InitializeObject(false, 30, 30, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryHorseEquipment.InitializeObject(false, 9, 5, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryHorseEquipment2.InitializeObject(false, 7, 6, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryHorseEquipment3.InitializeObject(false, 5, 7, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryHorseEquipment4.InitializeObject(false, 5, 8, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryHorseEquipment5.InitializeObject(false, 5, 9, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryMeleeWeapons1.InitializeObject(false, 9, 7, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryMeleeWeapons2.InitializeObject(false, 7, 7, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryMeleeWeapons3.InitializeObject(false, 5, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryMeleeWeapons4.InitializeObject(false, 4, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryMeleeWeapons5.InitializeObject(false, 4, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryRangedWeapons1.InitializeObject(false, 9, 7, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryRangedWeapons2.InitializeObject(false, 7, 7, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryRangedWeapons3.InitializeObject(false, 5, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryRangedWeapons4.InitializeObject(false, 4, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryRangedWeapons5.InitializeObject(false, 4, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryShield.InitializeObject(false, 9, 7, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryShield2.InitializeObject(false, 7, 7, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryShield3.InitializeObject(false, 5, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryShield4.InitializeObject(false, 4, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryShield5.InitializeObject(false, 4, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryGarment.InitializeObject(false, 9, 15, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryLightArmor.InitializeObject(false, 7, 16, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryMediumArmor.InitializeObject(false, 5, 17, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryHeavyArmor.InitializeObject(false, 4, 17, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryUltraArmor.InitializeObject(false, 3, 10, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryBanners.InitializeObject(false, 0, 0, ItemCategory.Property.None, null, 0f, false, true);
			this._itemCategoryUnassigned.InitializeObject(false, 0, 0, ItemCategory.Property.None, null, 0f, false, false);
		}

		// Token: 0x040002B9 RID: 697
		private ItemCategory _itemCategoryGrain;

		// Token: 0x040002BA RID: 698
		private ItemCategory _itemCategoryWood;

		// Token: 0x040002BB RID: 699
		private ItemCategory _itemCategoryMeat;

		// Token: 0x040002BC RID: 700
		private ItemCategory _itemCategoryLinen;

		// Token: 0x040002BD RID: 701
		private ItemCategory _itemCategoryHorseEquipment;

		// Token: 0x040002BE RID: 702
		private ItemCategory _itemCategoryHorseEquipment2;

		// Token: 0x040002BF RID: 703
		private ItemCategory _itemCategoryJewelry;

		// Token: 0x040002C0 RID: 704
		private ItemCategory _itemCategoryHorseEquipment3;

		// Token: 0x040002C1 RID: 705
		private ItemCategory _itemCategoryFur;

		// Token: 0x040002C2 RID: 706
		private ItemCategory _itemCategoryHorseEquipment4;

		// Token: 0x040002C3 RID: 707
		private ItemCategory _itemCategoryButter;

		// Token: 0x040002C4 RID: 708
		private ItemCategory _itemCategoryHorseEquipment5;

		// Token: 0x040002C5 RID: 709
		private ItemCategory _itemCategoryUnassigned;

		// Token: 0x040002C6 RID: 710
		private ItemCategory _itemCategoryArrows;

		// Token: 0x040002C7 RID: 711
		private ItemCategory _itemCategoryUltraArmor;

		// Token: 0x040002C8 RID: 712
		private ItemCategory _itemCategoryHeavyArmor;

		// Token: 0x040002C9 RID: 713
		private ItemCategory _itemCategoryMediumArmor;

		// Token: 0x040002CA RID: 714
		private ItemCategory _itemCategoryLightArmor;

		// Token: 0x040002CB RID: 715
		private ItemCategory _itemCategoryGarment;

		// Token: 0x040002CC RID: 716
		private ItemCategory _itemCategoryShield5;

		// Token: 0x040002CD RID: 717
		private ItemCategory _itemCategoryShield4;

		// Token: 0x040002CE RID: 718
		private ItemCategory _itemCategoryShield3;

		// Token: 0x040002CF RID: 719
		private ItemCategory _itemCategoryShield2;

		// Token: 0x040002D0 RID: 720
		private ItemCategory _itemCategoryShield;

		// Token: 0x040002D1 RID: 721
		private ItemCategory _itemCategoryRangedWeapons5;

		// Token: 0x040002D2 RID: 722
		private ItemCategory _itemCategoryRangedWeapons4;

		// Token: 0x040002D3 RID: 723
		private ItemCategory _itemCategoryRangedWeapons3;

		// Token: 0x040002D4 RID: 724
		private ItemCategory _itemCategoryRangedWeapons2;

		// Token: 0x040002D5 RID: 725
		private ItemCategory _itemCategoryRangedWeapons1;

		// Token: 0x040002D6 RID: 726
		private ItemCategory _itemCategoryMeleeWeapons5;

		// Token: 0x040002D7 RID: 727
		private ItemCategory _itemCategoryMeleeWeapons4;

		// Token: 0x040002D8 RID: 728
		private ItemCategory _itemCategoryMeleeWeapons3;

		// Token: 0x040002D9 RID: 729
		private ItemCategory _itemCategoryMeleeWeapons2;

		// Token: 0x040002DA RID: 730
		private ItemCategory _itemCategoryMeleeWeapons1;

		// Token: 0x040002DB RID: 731
		private ItemCategory _itemCategoryWarHorse;

		// Token: 0x040002DC RID: 732
		private ItemCategory _itemCategoryNobleHorse;

		// Token: 0x040002DD RID: 733
		private ItemCategory _itemCategoryPackAnimal;

		// Token: 0x040002DE RID: 734
		private ItemCategory _itemCategoryHorse;

		// Token: 0x040002DF RID: 735
		private ItemCategory _itemCategoryHog;

		// Token: 0x040002E0 RID: 736
		private ItemCategory _itemCategoryCow;

		// Token: 0x040002E1 RID: 737
		private ItemCategory _itemCategorySheep;

		// Token: 0x040002E2 RID: 738
		private ItemCategory _itemCategoryVelvet;

		// Token: 0x040002E3 RID: 739
		private ItemCategory _itemCategoryLeather;

		// Token: 0x040002E4 RID: 740
		private ItemCategory _itemCategoryCloth;

		// Token: 0x040002E5 RID: 741
		private ItemCategory _itemCategoryPottery;

		// Token: 0x040002E6 RID: 742
		private ItemCategory _itemCategoryTools;

		// Token: 0x040002E7 RID: 743
		private ItemCategory _itemCategoryOil;

		// Token: 0x040002E8 RID: 744
		private ItemCategory _itemCategoryWine;

		// Token: 0x040002E9 RID: 745
		private ItemCategory _itemCategoryBeer;

		// Token: 0x040002EA RID: 746
		private ItemCategory _itemCategoryOlives;

		// Token: 0x040002EB RID: 747
		private ItemCategory _itemCategoryDateFruit;

		// Token: 0x040002EC RID: 748
		private ItemCategory _itemCategoryClay;

		// Token: 0x040002ED RID: 749
		private ItemCategory _itemCategoryHides;

		// Token: 0x040002EE RID: 750
		private ItemCategory _itemCategoryGrape;

		// Token: 0x040002EF RID: 751
		private ItemCategory _itemCategoryFlax;

		// Token: 0x040002F0 RID: 752
		private ItemCategory _itemCategoryFish;

		// Token: 0x040002F1 RID: 753
		private ItemCategory _itemCategoryCotton;

		// Token: 0x040002F2 RID: 754
		private ItemCategory _itemCategorySilver;

		// Token: 0x040002F3 RID: 755
		private ItemCategory _itemCategorySalt;

		// Token: 0x040002F4 RID: 756
		private ItemCategory _itemCategoryIron;

		// Token: 0x040002F5 RID: 757
		private ItemCategory _itemCategoryCheese;

		// Token: 0x040002F6 RID: 758
		private ItemCategory _itemCategoryWool;

		// Token: 0x040002F7 RID: 759
		private ItemCategory _itemCategoryBanners;
	}
}
