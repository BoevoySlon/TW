using System;
using System.Runtime.CompilerServices;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003A3 RID: 931
	public interface IStatisticsCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x060037EA RID: 14314
		void OnDefectionPersuasionSucess();

		// Token: 0x060037EB RID: 14315
		void OnPlayerAcceptedRansomOffer(int ransomPrice);

		// Token: 0x060037EC RID: 14316
		int GetHighestTournamentRank();

		// Token: 0x060037ED RID: 14317
		int GetNumberOfTournamentWins();

		// Token: 0x060037EE RID: 14318
		int GetNumberOfChildrenBorn();

		// Token: 0x060037EF RID: 14319
		int GetNumberOfPrisonersRecruited();

		// Token: 0x060037F0 RID: 14320
		int GetNumberOfTroopsRecruited();

		// Token: 0x060037F1 RID: 14321
		int GetNumberOfClansDefected();

		// Token: 0x060037F2 RID: 14322
		int GetNumberOfIssuesSolved();

		// Token: 0x060037F3 RID: 14323
		int GetTotalInfluenceEarned();

		// Token: 0x060037F4 RID: 14324
		int GetTotalCrimeRatingGained();

		// Token: 0x060037F5 RID: 14325
		int GetNumberOfBattlesWon();

		// Token: 0x060037F6 RID: 14326
		int GetNumberOfBattlesLost();

		// Token: 0x060037F7 RID: 14327
		int GetLargestBattleWonAsLeader();

		// Token: 0x060037F8 RID: 14328
		int GetLargestArmyFormedByPlayer();

		// Token: 0x060037F9 RID: 14329
		int GetNumberOfEnemyClansDestroyed();

		// Token: 0x060037FA RID: 14330
		int GetNumberOfHeroesKilledInBattle();

		// Token: 0x060037FB RID: 14331
		int GetNumberOfTroopsKnockedOrKilledAsParty();

		// Token: 0x060037FC RID: 14332
		int GetNumberOfTroopsKnockedOrKilledByPlayer();

		// Token: 0x060037FD RID: 14333
		int GetNumberOfHeroPrisonersTaken();

		// Token: 0x060037FE RID: 14334
		int GetNumberOfTroopPrisonersTaken();

		// Token: 0x060037FF RID: 14335
		int GetNumberOfTownsCaptured();

		// Token: 0x06003800 RID: 14336
		int GetNumberOfHideoutsCleared();

		// Token: 0x06003801 RID: 14337
		int GetNumberOfCastlesCaptured();

		// Token: 0x06003802 RID: 14338
		int GetNumberOfVillagesRaided();

		// Token: 0x06003803 RID: 14339
		int GetNumberOfCraftingPartsUnlocked();

		// Token: 0x06003804 RID: 14340
		int GetNumberOfWeaponsCrafted();

		// Token: 0x06003805 RID: 14341
		int GetNumberOfCraftingOrdersCompleted();

		// Token: 0x06003806 RID: 14342
		int GetNumberOfCompanionsHired();

		// Token: 0x06003807 RID: 14343
		ulong GetTotalTimePlayedInSeconds();

		// Token: 0x06003808 RID: 14344
		ulong GetTotalDenarsEarned();

		// Token: 0x06003809 RID: 14345
		ulong GetDenarsEarnedFromCaravans();

		// Token: 0x0600380A RID: 14346
		ulong GetDenarsEarnedFromWorkshops();

		// Token: 0x0600380B RID: 14347
		ulong GetDenarsEarnedFromRansoms();

		// Token: 0x0600380C RID: 14348
		ulong GetDenarsEarnedFromTaxes();

		// Token: 0x0600380D RID: 14349
		ulong GetDenarsEarnedFromTributes();

		// Token: 0x0600380E RID: 14350
		ulong GetDenarsPaidAsTributes();

		// Token: 0x0600380F RID: 14351
		CampaignTime GetTotalTimePlayed();

		// Token: 0x06003810 RID: 14352
		CampaignTime GetTimeSpentAsPrisoner();

		// Token: 0x06003811 RID: 14353
		ValueTuple<string, int> GetMostExpensiveItemCrafted();

		// Token: 0x06003812 RID: 14354
		[return: TupleElementNames(new string[]
		{
			"name",
			"value"
		})]
		ValueTuple<string, int> GetCompanionWithMostKills();

		// Token: 0x06003813 RID: 14355
		[return: TupleElementNames(new string[]
		{
			"name",
			"value"
		})]
		ValueTuple<string, int> GetCompanionWithMostIssuesSolved();
	}
}
