using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000050 RID: 80
	public class DefaultSiegeEngineTypes
	{
		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x00016050 File Offset: 0x00014250
		private static DefaultSiegeEngineTypes Instance
		{
			get
			{
				return Game.Current.DefaultSiegeEngineTypes;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001605C File Offset: 0x0001425C
		public static SiegeEngineType Preparations
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypePreparations;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00016068 File Offset: 0x00014268
		public static SiegeEngineType Ladder
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeLadder;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x00016074 File Offset: 0x00014274
		public static SiegeEngineType Ballista
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeBallista;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00016080 File Offset: 0x00014280
		public static SiegeEngineType FireBallista
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireBallista;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0001608C File Offset: 0x0001428C
		public static SiegeEngineType Ram
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeRam;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x00016098 File Offset: 0x00014298
		public static SiegeEngineType ImprovedRam
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeImprovedRam;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x000160A4 File Offset: 0x000142A4
		public static SiegeEngineType SiegeTower
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeSiegeTower;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x000160B0 File Offset: 0x000142B0
		public static SiegeEngineType HeavySiegeTower
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeHeavySiegeTower;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x000160BC File Offset: 0x000142BC
		public static SiegeEngineType Catapult
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeCatapult;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x000160C8 File Offset: 0x000142C8
		public static SiegeEngineType FireCatapult
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireCatapult;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x000160D4 File Offset: 0x000142D4
		public static SiegeEngineType Onager
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeOnager;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x000160E0 File Offset: 0x000142E0
		public static SiegeEngineType FireOnager
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeFireOnager;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x000160EC File Offset: 0x000142EC
		public static SiegeEngineType Bricole
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeBricole;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000160F8 File Offset: 0x000142F8
		public static SiegeEngineType Trebuchet
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeTrebuchet;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00016104 File Offset: 0x00014304
		public static SiegeEngineType FireTrebuchet
		{
			get
			{
				return DefaultSiegeEngineTypes.Instance._siegeEngineTypeTrebuchet;
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00016110 File Offset: 0x00014310
		public DefaultSiegeEngineTypes()
		{
			this.RegisterAll();
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00016120 File Offset: 0x00014320
		private void RegisterAll()
		{
			Game.Current.ObjectManager.LoadXML("SiegeEngines", false);
			this._siegeEngineTypePreparations = this.GetSiegeEngine("preparations");
			this._siegeEngineTypeLadder = this.GetSiegeEngine("ladder");
			this._siegeEngineTypeSiegeTower = this.GetSiegeEngine("siege_tower_level1");
			this._siegeEngineTypeHeavySiegeTower = this.GetSiegeEngine("siege_tower_level2");
			this._siegeEngineTypeBallista = this.GetSiegeEngine("ballista");
			this._siegeEngineTypeFireBallista = this.GetSiegeEngine("fire_ballista");
			this._siegeEngineTypeCatapult = this.GetSiegeEngine("catapult");
			this._siegeEngineTypeFireCatapult = this.GetSiegeEngine("fire_catapult");
			this._siegeEngineTypeOnager = this.GetSiegeEngine("onager");
			this._siegeEngineTypeFireOnager = this.GetSiegeEngine("fire_onager");
			this._siegeEngineTypeBricole = this.GetSiegeEngine("bricole");
			this._siegeEngineTypeTrebuchet = this.GetSiegeEngine("trebuchet");
			this._siegeEngineTypeFireTrebuchet = this.GetSiegeEngine("fire_trebuchet");
			this._siegeEngineTypeRam = this.GetSiegeEngine("ram");
			this._siegeEngineTypeImprovedRam = this.GetSiegeEngine("improved_ram");
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00016241 File Offset: 0x00014441
		private SiegeEngineType GetSiegeEngine(string id)
		{
			return Game.Current.ObjectManager.GetObject<SiegeEngineType>(id);
		}

		// Token: 0x040002F8 RID: 760
		private SiegeEngineType _siegeEngineTypePreparations;

		// Token: 0x040002F9 RID: 761
		private SiegeEngineType _siegeEngineTypeLadder;

		// Token: 0x040002FA RID: 762
		private SiegeEngineType _siegeEngineTypeBallista;

		// Token: 0x040002FB RID: 763
		private SiegeEngineType _siegeEngineTypeFireBallista;

		// Token: 0x040002FC RID: 764
		private SiegeEngineType _siegeEngineTypeRam;

		// Token: 0x040002FD RID: 765
		private SiegeEngineType _siegeEngineTypeImprovedRam;

		// Token: 0x040002FE RID: 766
		private SiegeEngineType _siegeEngineTypeSiegeTower;

		// Token: 0x040002FF RID: 767
		private SiegeEngineType _siegeEngineTypeHeavySiegeTower;

		// Token: 0x04000300 RID: 768
		private SiegeEngineType _siegeEngineTypeCatapult;

		// Token: 0x04000301 RID: 769
		private SiegeEngineType _siegeEngineTypeFireCatapult;

		// Token: 0x04000302 RID: 770
		private SiegeEngineType _siegeEngineTypeOnager;

		// Token: 0x04000303 RID: 771
		private SiegeEngineType _siegeEngineTypeFireOnager;

		// Token: 0x04000304 RID: 772
		private SiegeEngineType _siegeEngineTypeBricole;

		// Token: 0x04000305 RID: 773
		private SiegeEngineType _siegeEngineTypeTrebuchet;

		// Token: 0x04000306 RID: 774
		private SiegeEngineType _siegeEngineTypeFireTrebuchet;
	}
}
