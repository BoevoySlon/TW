using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200015B RID: 347
	public abstract class EncyclopediaListItemComparerBase : IComparer<EncyclopediaListItem>
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x0007D11E File Offset: 0x0007B31E
		// (set) Token: 0x060018A5 RID: 6309 RVA: 0x0007D126 File Offset: 0x0007B326
		public bool IsAscending { get; private set; }

		// Token: 0x060018A6 RID: 6310 RVA: 0x0007D12F File Offset: 0x0007B32F
		public void SetSortOrder(bool isAscending)
		{
			this.IsAscending = isAscending;
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0007D138 File Offset: 0x0007B338
		public void SwitchSortOrder()
		{
			this.IsAscending = !this.IsAscending;
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0007D149 File Offset: 0x0007B349
		public void SetDefaultSortOrder()
		{
			this.IsAscending = false;
		}

		// Token: 0x060018A9 RID: 6313
		public abstract int Compare(EncyclopediaListItem x, EncyclopediaListItem y);

		// Token: 0x060018AA RID: 6314
		public abstract string GetComparedValueText(EncyclopediaListItem item);

		// Token: 0x060018AB RID: 6315 RVA: 0x0007D152 File Offset: 0x0007B352
		protected int ResolveEquality(EncyclopediaListItem x, EncyclopediaListItem y)
		{
			return x.Name.CompareTo(y.Name);
		}

		// Token: 0x040008AA RID: 2218
		protected readonly TextObject _emptyValue = new TextObject("{=4NaOKslb}-", null);

		// Token: 0x040008AB RID: 2219
		protected readonly TextObject _missingValue = new TextObject("{=keqS2dGa}???", null);
	}
}
