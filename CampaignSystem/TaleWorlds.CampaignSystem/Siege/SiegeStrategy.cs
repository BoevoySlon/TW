﻿using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x02000282 RID: 642
	public class SiegeStrategy : MBObjectBase
	{
		// Token: 0x06002267 RID: 8807 RVA: 0x000928FE File Offset: 0x00090AFE
		internal static void AutoGeneratedStaticCollectObjectsSiegeStrategy(object o, List<object> collectedObjects)
		{
			((SiegeStrategy)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x0009290C File Offset: 0x00090B0C
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x00092915 File Offset: 0x00090B15
		public static MBReadOnlyList<SiegeStrategy> All
		{
			get
			{
				return Campaign.Current.AllSiegeStrategies;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x0600226A RID: 8810 RVA: 0x00092921 File Offset: 0x00090B21
		// (set) Token: 0x0600226B RID: 8811 RVA: 0x00092929 File Offset: 0x00090B29
		public TextObject Name { get; private set; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x0600226C RID: 8812 RVA: 0x00092932 File Offset: 0x00090B32
		// (set) Token: 0x0600226D RID: 8813 RVA: 0x0009293A File Offset: 0x00090B3A
		public TextObject Description { get; private set; }

		// Token: 0x0600226E RID: 8814 RVA: 0x00092943 File Offset: 0x00090B43
		public SiegeStrategy(string stringId) : base(stringId)
		{
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x0009294C File Offset: 0x00090B4C
		public void Initialize(TextObject name, TextObject description)
		{
			base.Initialize();
			this.Name = name;
			this.Description = description;
			base.AfterInitialized();
		}
	}
}
