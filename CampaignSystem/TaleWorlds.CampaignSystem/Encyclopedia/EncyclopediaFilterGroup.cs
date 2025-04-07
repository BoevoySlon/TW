using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000155 RID: 341
	public class EncyclopediaFilterGroup : ViewModel
	{
		// Token: 0x0600187D RID: 6269 RVA: 0x0007C97D File Offset: 0x0007AB7D
		public EncyclopediaFilterGroup(List<EncyclopediaFilterItem> filters, TextObject name)
		{
			this.Filters = filters;
			this.Name = name;
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x0007C993 File Offset: 0x0007AB93
		public Predicate<object> Predicate
		{
			get
			{
				return delegate(object item)
				{
					if (!this.Filters.Any((EncyclopediaFilterItem f) => f.IsActive))
					{
						return true;
					}
					foreach (EncyclopediaFilterItem encyclopediaFilterItem in this.Filters)
					{
						if (encyclopediaFilterItem.IsActive && encyclopediaFilterItem.Predicate(item))
						{
							return true;
						}
					}
					return false;
				};
			}
		}

		// Token: 0x04000890 RID: 2192
		public readonly List<EncyclopediaFilterItem> Filters;

		// Token: 0x04000891 RID: 2193
		public readonly TextObject Name;
	}
}
