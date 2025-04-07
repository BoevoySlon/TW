using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x02000019 RID: 25
	public class BasicGameModels : GameModelsManager
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00006331 File Offset: 0x00004531
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00006339 File Offset: 0x00004539
		public RidingModel RidingModel { get; private set; }

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006342 File Offset: 0x00004542
		// (set) Token: 0x0600016A RID: 362 RVA: 0x0000634A File Offset: 0x0000454A
		public BattleSurvivalModel BattleSurvivalModel { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006353 File Offset: 0x00004553
		// (set) Token: 0x0600016C RID: 364 RVA: 0x0000635B File Offset: 0x0000455B
		public ItemCategorySelector ItemCategorySelector { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00006364 File Offset: 0x00004564
		// (set) Token: 0x0600016E RID: 366 RVA: 0x0000636C File Offset: 0x0000456C
		public ItemValueModel ItemValueModel { get; private set; }

		// Token: 0x0600016F RID: 367 RVA: 0x00006375 File Offset: 0x00004575
		public BasicGameModels(IEnumerable<GameModel> inputComponents) : base(inputComponents)
		{
			this.RidingModel = base.GetGameModel<RidingModel>();
			this.ItemCategorySelector = base.GetGameModel<ItemCategorySelector>();
			this.ItemValueModel = base.GetGameModel<ItemValueModel>();
		}
	}
}
