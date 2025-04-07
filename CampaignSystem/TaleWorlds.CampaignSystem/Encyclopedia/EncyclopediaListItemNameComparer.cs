using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200015A RID: 346
	internal class EncyclopediaListItemNameComparer : EncyclopediaListItemComparerBase
	{
		// Token: 0x060018A1 RID: 6305 RVA: 0x0007D105 File Offset: 0x0007B305
		public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
		{
			return base.ResolveEquality(x, y);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0007D10F File Offset: 0x0007B30F
		public override string GetComparedValueText(EncyclopediaListItem item)
		{
			return "";
		}
	}
}
