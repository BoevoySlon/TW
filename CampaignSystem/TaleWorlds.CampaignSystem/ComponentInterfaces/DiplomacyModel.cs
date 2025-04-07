using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200017F RID: 383
	public abstract class DiplomacyModel : GameModel
	{
		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060019A3 RID: 6563
		public abstract int MaxRelationLimit { get; }

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060019A4 RID: 6564
		public abstract int MinRelationLimit { get; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060019A5 RID: 6565
		public abstract int MaxNeutralRelationLimit { get; }

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060019A6 RID: 6566
		public abstract int MinNeutralRelationLimit { get; }

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060019A7 RID: 6567
		public abstract int MinimumRelationWithConversationCharacterToJoinKingdom { get; }

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060019A8 RID: 6568
		public abstract int GiftingTownRelationshipBonus { get; }

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060019A9 RID: 6569
		public abstract int GiftingCastleRelationshipBonus { get; }

		// Token: 0x060019AA RID: 6570
		public abstract float GetStrengthThresholdForNonMutualWarsToBeIgnoredToJoinKingdom(Kingdom kingdomToJoin);

		// Token: 0x060019AB RID: 6571
		public abstract float GetRelationIncreaseFactor(Hero hero1, Hero hero2, float relationValue);

		// Token: 0x060019AC RID: 6572
		public abstract int GetInfluenceAwardForSettlementCapturer(Settlement settlement);

		// Token: 0x060019AD RID: 6573
		public abstract float GetHourlyInfluenceAwardForRaidingEnemyVillage(MobileParty mobileParty);

		// Token: 0x060019AE RID: 6574
		public abstract float GetHourlyInfluenceAwardForBesiegingEnemyFortification(MobileParty mobileParty);

		// Token: 0x060019AF RID: 6575
		public abstract float GetHourlyInfluenceAwardForBeingArmyMember(MobileParty mobileParty);

		// Token: 0x060019B0 RID: 6576
		public abstract float GetScoreOfClanToJoinKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x060019B1 RID: 6577
		public abstract float GetScoreOfClanToLeaveKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x060019B2 RID: 6578
		public abstract float GetScoreOfKingdomToGetClan(Kingdom kingdom, Clan clan);

		// Token: 0x060019B3 RID: 6579
		public abstract float GetScoreOfKingdomToSackClan(Kingdom kingdom, Clan clan);

		// Token: 0x060019B4 RID: 6580
		public abstract float GetScoreOfMercenaryToJoinKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x060019B5 RID: 6581
		public abstract float GetScoreOfMercenaryToLeaveKingdom(Clan clan, Kingdom kingdom);

		// Token: 0x060019B6 RID: 6582
		public abstract float GetScoreOfKingdomToHireMercenary(Kingdom kingdom, Clan mercenaryClan);

		// Token: 0x060019B7 RID: 6583
		public abstract float GetScoreOfKingdomToSackMercenary(Kingdom kingdom, Clan mercenaryClan);

		// Token: 0x060019B8 RID: 6584
		public abstract float GetScoreOfDeclaringPeace(IFaction factionDeclaresPeace, IFaction factionDeclaredPeace, IFaction evaluatingFaction, out TextObject reason);

		// Token: 0x060019B9 RID: 6585
		public abstract float GetScoreOfDeclaringWar(IFaction factionDeclaresWar, IFaction factionDeclaredWar, IFaction evaluatingFaction, out TextObject reason);

		// Token: 0x060019BA RID: 6586
		public abstract float GetScoreOfLettingPartyGo(MobileParty party, MobileParty partyToLetGo);

		// Token: 0x060019BB RID: 6587
		public abstract float GetValueOfHeroForFaction(Hero examinedHero, IFaction targetFaction, bool forMarriage = false);

		// Token: 0x060019BC RID: 6588
		public abstract int GetRelationCostOfExpellingClanFromKingdom();

		// Token: 0x060019BD RID: 6589
		public abstract int GetInfluenceCostOfSupportingClan();

		// Token: 0x060019BE RID: 6590
		public abstract int GetInfluenceCostOfExpellingClan(Clan proposingClan);

		// Token: 0x060019BF RID: 6591
		public abstract int GetInfluenceCostOfProposingPeace(Clan proposingClan);

		// Token: 0x060019C0 RID: 6592
		public abstract int GetInfluenceCostOfProposingWar(Clan proposingClan);

		// Token: 0x060019C1 RID: 6593
		public abstract int GetInfluenceValueOfSupportingClan();

		// Token: 0x060019C2 RID: 6594
		public abstract int GetRelationValueOfSupportingClan();

		// Token: 0x060019C3 RID: 6595
		public abstract int GetInfluenceCostOfAnnexation(Clan proposingClan);

		// Token: 0x060019C4 RID: 6596
		public abstract int GetInfluenceCostOfChangingLeaderOfArmy();

		// Token: 0x060019C5 RID: 6597
		public abstract int GetInfluenceCostOfDisbandingArmy();

		// Token: 0x060019C6 RID: 6598
		public abstract int GetRelationCostOfDisbandingArmy(bool isLeaderParty);

		// Token: 0x060019C7 RID: 6599
		public abstract int GetInfluenceCostOfPolicyProposalAndDisavowal(Clan proposingClan);

		// Token: 0x060019C8 RID: 6600
		public abstract int GetInfluenceCostOfAbandoningArmy();

		// Token: 0x060019C9 RID: 6601
		public abstract int GetEffectiveRelation(Hero hero, Hero hero1);

		// Token: 0x060019CA RID: 6602
		public abstract int GetBaseRelation(Hero hero, Hero hero1);

		// Token: 0x060019CB RID: 6603
		public abstract void GetHeroesForEffectiveRelation(Hero hero1, Hero hero2, out Hero effectiveHero1, out Hero effectiveHero2);

		// Token: 0x060019CC RID: 6604
		public abstract int GetRelationChangeAfterClanLeaderIsDead(Hero deadLeader, Hero relationHero);

		// Token: 0x060019CD RID: 6605
		public abstract int GetRelationChangeAfterVotingInSettlementOwnerPreliminaryDecision(Hero supporter, bool hasHeroVotedAgainstOwner);

		// Token: 0x060019CE RID: 6606
		public abstract float GetClanStrength(Clan clan);

		// Token: 0x060019CF RID: 6607
		public abstract float GetHeroCommandingStrengthForClan(Hero hero);

		// Token: 0x060019D0 RID: 6608
		public abstract float GetHeroGoverningStrengthForClan(Hero hero);

		// Token: 0x060019D1 RID: 6609
		public abstract uint GetNotificationColor(ChatNotificationType notificationType);

		// Token: 0x060019D2 RID: 6610
		public abstract int GetValueOfDailyTribute(int dailyTributeAmount);

		// Token: 0x060019D3 RID: 6611
		public abstract int GetDailyTributeForValue(int value);

		// Token: 0x060019D4 RID: 6612
		public abstract bool CanSettlementBeGifted(Settlement settlement);

		// Token: 0x060019D5 RID: 6613
		public abstract bool IsClanEligibleToBecomeRuler(Clan clan);

		// Token: 0x060019D6 RID: 6614
		public abstract IEnumerable<BarterGroup> GetBarterGroups();

		// Token: 0x060019D7 RID: 6615
		public abstract int GetCharmExperienceFromRelationGain(Hero hero, float relationChange, ChangeRelationAction.ChangeRelationDetail detail);

		// Token: 0x060019D8 RID: 6616
		public abstract float DenarsToInfluence();
	}
}
