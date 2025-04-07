using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200044F RID: 1103
	public static class MakePregnantAction
	{
		// Token: 0x060040F8 RID: 16632 RVA: 0x00140B52 File Offset: 0x0013ED52
		private static void ApplyInternal(Hero mother)
		{
			mother.IsPregnant = true;
			CampaignEventDispatcher.Instance.OnChildConceived(mother);
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x00140B66 File Offset: 0x0013ED66
		public static void Apply(Hero mother)
		{
			MakePregnantAction.ApplyInternal(mother);
		}
	}
}
