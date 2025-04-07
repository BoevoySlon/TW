using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200015D RID: 349
	public abstract class EncyclopediaModelBase : Attribute
	{
		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060018B0 RID: 6320 RVA: 0x0007D1B5 File Offset: 0x0007B3B5
		// (set) Token: 0x060018B1 RID: 6321 RVA: 0x0007D1BD File Offset: 0x0007B3BD
		public Type[] PageTargetTypes { get; private set; }

		// Token: 0x060018B2 RID: 6322 RVA: 0x0007D1C6 File Offset: 0x0007B3C6
		public EncyclopediaModelBase(Type[] pageTargetTypes)
		{
			this.PageTargetTypes = pageTargetTypes;
		}
	}
}
