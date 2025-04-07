using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000449 RID: 1097
	public static class KillCharacterAction
	{
		// Token: 0x060040DE RID: 16606 RVA: 0x0013F8A8 File Offset: 0x0013DAA8
		private static void ApplyInternal(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail actionDetail, bool showNotification, bool isForced = false)
		{
			if (!victim.CanDie(actionDetail) && !isForced)
			{
				return;
			}
			if (!victim.IsAlive)
			{
				Debug.FailedAssert("Victim: " + victim.Name + " is already dead!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\KillCharacterAction.cs", "ApplyInternal", 40);
				return;
			}
			if (victim.IsNotable)
			{
				IssueBase issue = victim.Issue;
				if (((issue != null) ? issue.IssueQuest : null) != null)
				{
					Debug.FailedAssert("Trying to kill a notable that has quest!", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Actions\\KillCharacterAction.cs", "ApplyInternal", 47);
				}
			}
			MobileParty partyBelongedTo = victim.PartyBelongedTo;
			if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
			{
				MobileParty partyBelongedTo2 = victim.PartyBelongedTo;
				if (((partyBelongedTo2 != null) ? partyBelongedTo2.SiegeEvent : null) == null)
				{
					goto IL_E2;
				}
			}
			if (victim.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
			{
				victim.AddDeathMark(killer, actionDetail);
				return;
			}
			IL_E2:
			CampaignEventDispatcher.Instance.OnBeforeHeroKilled(victim, killer, actionDetail, showNotification);
			if (victim.IsHumanPlayerCharacter && !isForced)
			{
				CampaignEventDispatcher.Instance.OnBeforeMainCharacterDied(victim, killer, actionDetail, showNotification);
				return;
			}
			victim.AddDeathMark(killer, actionDetail);
			victim.EncyclopediaText = KillCharacterAction.CreateObituary(victim, actionDetail);
			if (victim.Clan != null && (victim.Clan.Leader == victim || victim == Hero.MainHero))
			{
				if (!victim.Clan.IsEliminated && victim != Hero.MainHero && victim.Clan.Heroes.Any((Hero x) => !x.IsChild && x != victim && x.IsAlive && x.IsLord))
				{
					ChangeClanLeaderAction.ApplyWithoutSelectedNewLeader(victim.Clan);
				}
				if (victim.Clan.Kingdom != null && victim.Clan.Kingdom.RulingClan == victim.Clan)
				{
					List<Clan> list = (from t in victim.Clan.Kingdom.Clans
					where !t.IsEliminated && t.Leader != victim && !t.IsUnderMercenaryService
					select t).ToList<Clan>();
					if (list.IsEmpty<Clan>())
					{
						if (!victim.Clan.Kingdom.IsEliminated)
						{
							DestroyKingdomAction.ApplyByKingdomLeaderDeath(victim.Clan.Kingdom);
						}
					}
					else if (!victim.Clan.Kingdom.IsEliminated)
					{
						if (list.Count > 1)
						{
							Clan clanToExclude = (victim.Clan.Leader == victim || victim.Clan.Leader == null) ? victim.Clan : null;
							victim.Clan.Kingdom.AddDecision(new KingSelectionKingdomDecision(victim.Clan, clanToExclude), true);
							if (clanToExclude != null)
							{
								Clan randomElementWithPredicate = victim.Clan.Kingdom.Clans.GetRandomElementWithPredicate((Clan t) => t != clanToExclude && Campaign.Current.Models.DiplomacyModel.IsClanEligibleToBecomeRuler(t));
								ChangeRulingClanAction.Apply(victim.Clan.Kingdom, randomElementWithPredicate);
							}
						}
						else
						{
							ChangeRulingClanAction.Apply(victim.Clan.Kingdom, list[0]);
						}
					}
				}
			}
			if (victim.PartyBelongedTo != null && (victim.PartyBelongedTo.LeaderHero == victim || victim == Hero.MainHero))
			{
				MobileParty partyBelongedTo3 = victim.PartyBelongedTo;
				if (victim.PartyBelongedTo.Army != null)
				{
					if (victim.PartyBelongedTo.Army.LeaderParty == victim.PartyBelongedTo)
					{
						DisbandArmyAction.ApplyByArmyLeaderIsDead(victim.PartyBelongedTo.Army);
					}
					else
					{
						victim.PartyBelongedTo.Army = null;
					}
				}
				if (partyBelongedTo3 != MobileParty.MainParty)
				{
					partyBelongedTo3.Ai.SetMoveModeHold();
					if (victim.Clan != null && victim.Clan.IsRebelClan)
					{
						DestroyPartyAction.Apply(null, partyBelongedTo3);
					}
				}
			}
			KillCharacterAction.MakeDead(victim, true);
			if (victim.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(victim);
			}
			if (actionDetail == KillCharacterAction.KillCharacterActionDetail.Executed && killer == Hero.MainHero && victim.Clan != null)
			{
				if (victim.GetTraitLevel(DefaultTraits.Honor) >= 0)
				{
					TraitLevelingHelper.OnLordExecuted();
				}
				foreach (Clan clan in Clan.All)
				{
					if (!clan.IsEliminated && !clan.IsBanditFaction && clan != Clan.PlayerClan)
					{
						bool affectRelatives;
						int relationChangeForExecutingHero = Campaign.Current.Models.ExecutionRelationModel.GetRelationChangeForExecutingHero(victim, clan.Leader, out affectRelatives);
						if (relationChangeForExecutingHero != 0)
						{
							ChangeRelationAction.ApplyPlayerRelation(clan.Leader, relationChangeForExecutingHero, affectRelatives, true);
						}
					}
				}
			}
			if (victim.Clan != null && !victim.Clan.IsEliminated && !victim.Clan.IsBanditFaction && victim.Clan != Clan.PlayerClan)
			{
				if (victim.Clan.Leader == victim)
				{
					DestroyClanAction.ApplyByClanLeaderDeath(victim.Clan);
				}
				else if (victim.Clan.Leader == null)
				{
					DestroyClanAction.Apply(victim.Clan);
				}
			}
			CampaignEventDispatcher.Instance.OnHeroKilled(victim, killer, actionDetail, showNotification);
			if (victim.Spouse != null)
			{
				victim.Spouse = null;
			}
			if (victim.CompanionOf != null)
			{
				RemoveCompanionAction.ApplyByDeath(victim.CompanionOf, victim);
			}
			if (victim.CurrentSettlement != null)
			{
				if (victim.CurrentSettlement == Settlement.CurrentSettlement)
				{
					LocationComplex locationComplex = LocationComplex.Current;
					if (locationComplex != null)
					{
						locationComplex.RemoveCharacterIfExists(victim);
					}
				}
				if (victim.StayingInSettlement != null)
				{
					victim.StayingInSettlement = null;
				}
			}
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x0013FF18 File Offset: 0x0013E118
		public static void ApplyByOldAge(Hero victim, bool showNotification = true)
		{
			KillCharacterAction.ApplyInternal(victim, null, KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge, showNotification, false);
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x0013FF24 File Offset: 0x0013E124
		public static void ApplyByWounds(Hero victim, bool showNotification = true)
		{
			KillCharacterAction.ApplyInternal(victim, null, KillCharacterAction.KillCharacterActionDetail.WoundedInBattle, showNotification, false);
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x0013FF30 File Offset: 0x0013E130
		public static void ApplyByBattle(Hero victim, Hero killer, bool showNotification = true)
		{
			KillCharacterAction.ApplyInternal(victim, killer, KillCharacterAction.KillCharacterActionDetail.DiedInBattle, showNotification, false);
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x0013FF3C File Offset: 0x0013E13C
		public static void ApplyByMurder(Hero victim, Hero killer = null, bool showNotification = true)
		{
			KillCharacterAction.ApplyInternal(victim, killer, KillCharacterAction.KillCharacterActionDetail.Murdered, showNotification, false);
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x0013FF48 File Offset: 0x0013E148
		public static void ApplyInLabor(Hero lostMother, bool showNotification = true)
		{
			KillCharacterAction.ApplyInternal(lostMother, null, KillCharacterAction.KillCharacterActionDetail.DiedInLabor, showNotification, false);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x0013FF54 File Offset: 0x0013E154
		public static void ApplyByExecution(Hero victim, Hero executer, bool showNotification = true, bool isForced = false)
		{
			KillCharacterAction.ApplyInternal(victim, executer, KillCharacterAction.KillCharacterActionDetail.Executed, showNotification, isForced);
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x0013FF60 File Offset: 0x0013E160
		public static void ApplyByRemove(Hero victim, bool showNotification = false, bool isForced = true)
		{
			KillCharacterAction.ApplyInternal(victim, null, KillCharacterAction.KillCharacterActionDetail.Lost, showNotification, isForced);
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x0013FF6C File Offset: 0x0013E16C
		public static void ApplyByDeathMark(Hero victim, bool showNotification = false)
		{
			KillCharacterAction.ApplyInternal(victim, victim.DeathMarkKillerHero, victim.DeathMark, showNotification, false);
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x0013FF82 File Offset: 0x0013E182
		public static void ApplyByDeathMarkForced(Hero victim, bool showNotification = false)
		{
			KillCharacterAction.ApplyInternal(victim, victim.DeathMarkKillerHero, victim.DeathMark, showNotification, true);
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x0013FF98 File Offset: 0x0013E198
		public static void ApplyByPlayerIllness()
		{
			KillCharacterAction.ApplyInternal(Hero.MainHero, null, KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge, true, true);
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x0013FFA8 File Offset: 0x0013E1A8
		private static void MakeDead(Hero victim, bool disbandVictimParty = true)
		{
			victim.ChangeState(Hero.CharacterStates.Dead);
			victim.DeathDay = CampaignTime.Now;
			if (!victim.IsHumanPlayerCharacter)
			{
				victim.OnDeath();
			}
			if (victim.PartyBelongedToAsPrisoner != null)
			{
				EndCaptivityAction.ApplyByDeath(victim);
			}
			if (victim.PartyBelongedTo != null)
			{
				MobileParty partyBelongedTo = victim.PartyBelongedTo;
				if (partyBelongedTo.LeaderHero == victim)
				{
					bool flag = false;
					if (!partyBelongedTo.IsMainParty)
					{
						foreach (TroopRosterElement troopRosterElement in partyBelongedTo.MemberRoster.GetTroopRoster())
						{
							if (troopRosterElement.Character.IsHero && troopRosterElement.Character != victim.CharacterObject)
							{
								partyBelongedTo.ChangePartyLeader(troopRosterElement.Character.HeroObject);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						if (!partyBelongedTo.IsMainParty)
						{
							partyBelongedTo.RemovePartyLeader();
						}
						if (partyBelongedTo.IsActive && disbandVictimParty)
						{
							Hero owner = partyBelongedTo.Party.Owner;
							if (((owner != null) ? owner.CompanionOf : null) == Clan.PlayerClan)
							{
								partyBelongedTo.Party.SetCustomOwner(Hero.MainHero);
							}
							partyBelongedTo.MemberRoster.RemoveTroop(victim.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
							DisbandPartyAction.StartDisband(partyBelongedTo);
						}
					}
				}
				if (victim.PartyBelongedTo != null)
				{
					partyBelongedTo.MemberRoster.RemoveTroop(victim.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
				}
				if (partyBelongedTo.IsActive && partyBelongedTo.MemberRoster.TotalManCount == 0)
				{
					DestroyPartyAction.Apply(null, partyBelongedTo);
					return;
				}
			}
			else if (victim.IsHumanPlayerCharacter && !MobileParty.MainParty.IsActive)
			{
				DestroyPartyAction.Apply(null, MobileParty.MainParty);
			}
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x0014014C File Offset: 0x0013E34C
		private static Clan SelectHeirClanForKingdom(Kingdom kingdom, bool exceptRulingClan)
		{
			Clan rulingClan = kingdom.RulingClan;
			Clan result = null;
			float num = 0f;
			IEnumerable<Clan> clans = kingdom.Clans;
			Func<Clan, bool> <>9__0;
			Func<Clan, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((Clan t) => t.Heroes.Any((Hero h) => h.IsAlive) && !t.IsMinorFaction && t != rulingClan));
			}
			foreach (Clan clan in clans.Where(predicate))
			{
				float clanStrength = Campaign.Current.Models.DiplomacyModel.GetClanStrength(clan);
				if (num <= clanStrength)
				{
					num = clanStrength;
					result = clan;
				}
			}
			return result;
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x001401F8 File Offset: 0x0013E3F8
		private static TextObject CreateObituary(Hero hero, KillCharacterAction.KillCharacterActionDetail detail)
		{
			TextObject textObject;
			if (hero.IsLord)
			{
				if (hero.Clan != null && hero.Clan.IsMinorFaction)
				{
					textObject = new TextObject("{=L7qd6qfv}{CHARACTER.FIRSTNAME} was a member of the {CHARACTER.FACTION}. {FURTHER_DETAILS}.", null);
				}
				else
				{
					textObject = new TextObject("{=mfYzCeGR}{CHARACTER.NAME} was {TITLE} of the {CHARACTER_FACTION_SHORT}. {FURTHER_DETAILS}.", null);
					textObject.SetTextVariable("CHARACTER_FACTION_SHORT", hero.MapFaction.InformalName);
					textObject.SetTextVariable("TITLE", HeroHelper.GetTitleInIndefiniteCase(hero));
				}
			}
			else if (hero.HomeSettlement != null)
			{
				textObject = new TextObject("{=YNXK352h}{CHARACTER.NAME} was a prominent {.%}{PROFESSION}{.%} from {HOMETOWN}. {FURTHER_DETAILS}.", null);
				textObject.SetTextVariable("PROFESSION", HeroHelper.GetCharacterTypeName(hero));
				textObject.SetTextVariable("HOMETOWN", hero.HomeSettlement.Name);
			}
			else
			{
				textObject = new TextObject("{=!}{FURTHER_DETAILS}.", null);
			}
			StringHelpers.SetCharacterProperties("CHARACTER", hero.CharacterObject, textObject, true);
			TextObject textObject2 = TextObject.Empty;
			if (detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
			{
				textObject2 = new TextObject("{=6pCABUme}{?CHARACTER.GENDER}She{?}He{\\?} died in battle in {YEAR} at the age of {CHARACTER.AGE}. {?CHARACTER.GENDER}She{?}He{\\?} was reputed to be {REPUTATION}", null);
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor)
			{
				textObject2 = new TextObject("{=7Vw6iYNI}{?CHARACTER.GENDER}She{?}He{\\?} died in childbirth in {YEAR} at the age of {CHARACTER.AGE}. {?CHARACTER.GENDER}She{?}He{\\?} was reputed to be {REPUTATION}", null);
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.Executed)
			{
				textObject2 = new TextObject("{=9Tq3IAiz}{?CHARACTER.GENDER}She{?}He{\\?} was executed in {YEAR} at the age of {CHARACTER.AGE}. {?CHARACTER.GENDER}She{?}He{\\?} was reputed to be {REPUTATION}", null);
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.Lost)
			{
				textObject2 = new TextObject("{=SausWqM5}{?CHARACTER.GENDER}She{?}He{\\?} disappeared in {YEAR} at the age of {CHARACTER.AGE}. {?CHARACTER.GENDER}She{?}He{\\?} was reputed to be {REPUTATION}", null);
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				textObject2 = new TextObject("{=TUDAvcTR}{?CHARACTER.GENDER}She{?}He{\\?} was assassinated in {YEAR} at the age of {CHARACTER.AGE}. {?CHARACTER.GENDER}She{?}He{\\?} was reputed to be {REPUTATION}", null);
			}
			else if (detail == KillCharacterAction.KillCharacterActionDetail.WoundedInBattle)
			{
				textObject2 = new TextObject("{=LsBCQtVX}{?CHARACTER.GENDER}She{?}He{\\?} died of war-wounds in {YEAR} at the age of {CHARACTER.AGE}. {?CHARACTER.GENDER}She{?}He{\\?} was reputed to be {REPUTATION}", null);
			}
			else
			{
				textObject2 = new TextObject("{=HU5n5KTW}{?CHARACTER.GENDER}She{?}He{\\?} died of natural causes in {YEAR} at the age of {CHARACTER.AGE}. {?CHARACTER.GENDER}She{?}He{\\?} was reputed to be {REPUTATION}", null);
			}
			StringHelpers.SetCharacterProperties("CHARACTER", hero.CharacterObject, textObject2, true);
			textObject2.SetTextVariable("REPUTATION", CharacterHelper.GetReputationDescription(hero.CharacterObject));
			textObject2.SetTextVariable("YEAR", CampaignTime.Now.GetYear.ToString());
			textObject.SetTextVariable("FURTHER_DETAILS", textObject2);
			return textObject;
		}

		// Token: 0x02000772 RID: 1906
		public enum KillCharacterActionDetail
		{
			// Token: 0x04001F2F RID: 7983
			None,
			// Token: 0x04001F30 RID: 7984
			Murdered,
			// Token: 0x04001F31 RID: 7985
			DiedInLabor,
			// Token: 0x04001F32 RID: 7986
			DiedOfOldAge,
			// Token: 0x04001F33 RID: 7987
			DiedInBattle,
			// Token: 0x04001F34 RID: 7988
			WoundedInBattle,
			// Token: 0x04001F35 RID: 7989
			Executed,
			// Token: 0x04001F36 RID: 7990
			Lost
		}
	}
}
