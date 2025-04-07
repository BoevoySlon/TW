using System;
using TaleWorlds.CampaignSystem.Settlements.Workshops;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000430 RID: 1072
	public static class ChangeOwnerOfWorkshopAction
	{
		// Token: 0x06004075 RID: 16501 RVA: 0x0013E164 File Offset: 0x0013C364
		private static void ApplyInternal(Workshop workshop, Hero newOwner, WorkshopType workshopType, int capital, int cost)
		{
			Hero owner = workshop.Owner;
			workshop.ChangeOwnerOfWorkshop(newOwner, workshopType, capital);
			if (newOwner == Hero.MainHero)
			{
				GiveGoldAction.ApplyBetweenCharacters(newOwner, owner, cost, false);
			}
			if (owner == Hero.MainHero)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, cost, false);
			}
			CampaignEventDispatcher.Instance.OnWorkshopOwnerChanged(workshop, owner);
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x0013E1B5 File Offset: 0x0013C3B5
		public static void ApplyByBankruptcy(Workshop workshop, Hero newOwner, WorkshopType workshopType, int cost)
		{
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, cost);
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0013E1D4 File Offset: 0x0013C3D4
		public static void ApplyByPlayerBuying(Workshop workshop)
		{
			int costForPlayer = Campaign.Current.Models.WorkshopModel.GetCostForPlayer(workshop);
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, Hero.MainHero, workshop.WorkshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, costForPlayer);
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0013E220 File Offset: 0x0013C420
		public static void ApplyByPlayerSelling(Workshop workshop, Hero newOwner, WorkshopType workshopType)
		{
			int costForNotable = Campaign.Current.Models.WorkshopModel.GetCostForNotable(workshop);
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, costForNotable);
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x0013E260 File Offset: 0x0013C460
		public static void ApplyByDeath(Workshop workshop, Hero newOwner)
		{
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshop.WorkshopType, workshop.Capital, 0);
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0013E276 File Offset: 0x0013C476
		public static void ApplyByWar(Workshop workshop, Hero newOwner, WorkshopType workshopType)
		{
			ChangeOwnerOfWorkshopAction.ApplyInternal(workshop, newOwner, workshopType, Campaign.Current.Models.WorkshopModel.InitialCapital, 0);
		}
	}
}
