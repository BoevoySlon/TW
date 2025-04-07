using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Extensions
{
	// Token: 0x0200014D RID: 333
	public static class Skills
	{
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x0007C625 File Offset: 0x0007A825
		public static MBReadOnlyList<SkillObject> All
		{
			get
			{
				return Campaign.Current.AllSkills;
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0007C631 File Offset: 0x0007A831
		public static SkillObject GetSkill(int i)
		{
			return Skills.All[i];
		}
	}
}
