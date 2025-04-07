using System;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000156 RID: 342
	public class EncyclopediaFilterItem
	{
		// Token: 0x06001880 RID: 6272 RVA: 0x0007CA3C File Offset: 0x0007AC3C
		public EncyclopediaFilterItem(TextObject name, Predicate<object> predicate)
		{
			this.Name = name;
			this.Predicate = predicate;
			this.IsActive = false;
		}

		// Token: 0x04000892 RID: 2194
		public readonly TextObject Name;

		// Token: 0x04000893 RID: 2195
		public readonly Predicate<object> Predicate;

		// Token: 0x04000894 RID: 2196
		public bool IsActive;
	}
}
