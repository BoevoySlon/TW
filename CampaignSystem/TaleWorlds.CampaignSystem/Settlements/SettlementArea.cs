﻿using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x0200035B RID: 859
	public abstract class SettlementArea
	{
		// Token: 0x060031C0 RID: 12736 RVA: 0x000D0D08 File Offset: 0x000CEF08
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x060031C1 RID: 12737
		public abstract Settlement Settlement { get; }

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x060031C2 RID: 12738
		public abstract TextObject Name { get; }

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x060031C3 RID: 12739
		public abstract string Tag { get; }

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x060031C4 RID: 12740
		public abstract Hero Owner { get; }
	}
}
