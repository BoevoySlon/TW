using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200015C RID: 348
	public class EncyclopediaSortController
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x0007D18F File Offset: 0x0007B38F
		public TextObject Name { get; }

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x0007D197 File Offset: 0x0007B397
		public EncyclopediaListItemComparerBase Comparer { get; }

		// Token: 0x060018AF RID: 6319 RVA: 0x0007D19F File Offset: 0x0007B39F
		public EncyclopediaSortController(TextObject name, EncyclopediaListItemComparerBase comparer)
		{
			this.Name = name;
			this.Comparer = comparer;
		}
	}
}
