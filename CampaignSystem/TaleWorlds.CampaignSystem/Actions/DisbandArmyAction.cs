using System;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200043D RID: 1085
	public static class DisbandArmyAction
	{
		// Token: 0x060040A3 RID: 16547 RVA: 0x0013EBD0 File Offset: 0x0013CDD0
		private static void ApplyInternal(Army army, Army.ArmyDispersionReason reason)
		{
			if (reason == Army.ArmyDispersionReason.DismissalRequestedWithInfluence)
			{
				DiplomacyModel diplomacyModel = Campaign.Current.Models.DiplomacyModel;
				ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)diplomacyModel.GetInfluenceCostOfDisbandingArmy()));
				foreach (MobileParty mobileParty in army.Parties.ToList<MobileParty>())
				{
					if (mobileParty != MobileParty.MainParty && mobileParty.LeaderHero != null)
					{
						ChangeRelationAction.ApplyPlayerRelation(mobileParty.LeaderHero, diplomacyModel.GetRelationCostOfDisbandingArmy(mobileParty == mobileParty.Army.LeaderParty), true, true);
					}
				}
			}
			army.DisperseInternal(reason);
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0013EC84 File Offset: 0x0013CE84
		public static void ApplyByReleasedByPlayerAfterBattle(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.DismissalRequestedWithInfluence);
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0013EC8D File Offset: 0x0013CE8D
		public static void ApplyByArmyLeaderIsDead(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.ArmyLeaderIsDead);
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x0013EC97 File Offset: 0x0013CE97
		public static void ApplyByNotEnoughParty(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.NotEnoughParty);
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x0013ECA0 File Offset: 0x0013CEA0
		public static void ApplyByObjectiveFinished(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.ObjectiveFinished);
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x0013ECA9 File Offset: 0x0013CEA9
		public static void ApplyByPlayerTakenPrisoner(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.PlayerTakenPrisoner);
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x0013ECB2 File Offset: 0x0013CEB2
		public static void ApplyByFoodProblem(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.FoodProblem);
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x0013ECBC File Offset: 0x0013CEBC
		public static void ApplyByCohesionDepleted(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.CohesionDepleted);
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0013ECC5 File Offset: 0x0013CEC5
		public static void ApplyByNoActiveWar(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.NoActiveWar);
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0013ECCF File Offset: 0x0013CECF
		public static void ApplyByUnknownReason(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.Unknown);
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0013ECD8 File Offset: 0x0013CED8
		public static void ApplyByLeaderPartyRemoved(Army army)
		{
			DisbandArmyAction.ApplyInternal(army, Army.ArmyDispersionReason.LeaderPartyRemoved);
		}
	}
}
