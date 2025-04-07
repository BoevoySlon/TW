using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000432 RID: 1074
	public static class ChangeProductionTypeOfWorkshopAction
	{
		// Token: 0x0600407D RID: 16509 RVA: 0x0013E34C File Offset: 0x0013C54C
		public static void Apply(Workshop workshop, WorkshopType newWorkshopType, bool ignoreCost = false)
		{
			int num = ignoreCost ? 0 : Campaign.Current.Models.WorkshopModel.GetConvertProductionCost(newWorkshopType);
			workshop.ChangeWorkshopProduction(newWorkshopType);
			if (num > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(workshop.Owner, null, num, false);
			}
			CampaignEventDispatcher.Instance.OnWorkshopTypeChanged(workshop);
		}
	}
}
