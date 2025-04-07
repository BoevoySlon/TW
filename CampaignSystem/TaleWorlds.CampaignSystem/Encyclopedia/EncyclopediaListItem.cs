using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000157 RID: 343
	public struct EncyclopediaListItem
	{
		// Token: 0x06001881 RID: 6273 RVA: 0x0007CA59 File Offset: 0x0007AC59
		public EncyclopediaListItem(object obj, string name, string description, string id, string typeName, bool playerCanSeeValues, Action onShowTooltip = null)
		{
			this.Object = obj;
			this.Name = name;
			this.Description = description;
			this.Id = id;
			this.TypeName = typeName;
			this.PlayerCanSeeValues = playerCanSeeValues;
			this.OnShowTooltip = onShowTooltip;
		}

		// Token: 0x04000895 RID: 2197
		public readonly object Object;

		// Token: 0x04000896 RID: 2198
		public readonly string Name;

		// Token: 0x04000897 RID: 2199
		public readonly string Description;

		// Token: 0x04000898 RID: 2200
		public readonly string Id;

		// Token: 0x04000899 RID: 2201
		public readonly string TypeName;

		// Token: 0x0400089A RID: 2202
		public readonly bool PlayerCanSeeValues;

		// Token: 0x0400089B RID: 2203
		public readonly Action OnShowTooltip;
	}
}
