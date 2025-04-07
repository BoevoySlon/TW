using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003A9 RID: 937
	public class KingdomDecisionProposalBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003840 RID: 14400 RVA: 0x000FE104 File Offset: 0x000FC304
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.SessionLaunched));
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnPeaceMade));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x000FE1B2 File Offset: 0x000FC3B2
		private void OnKingdomDestroyed(Kingdom kingdom)
		{
			this.UpdateKingdomDecisions(kingdom);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000FE1BC File Offset: 0x000FC3BC
		private void DailyTickClan(Clan clan)
		{
			if ((float)((int)Campaign.Current.CampaignStartTime.ElapsedDaysUntilNow) < 5f)
			{
				return;
			}
			if (clan.IsEliminated)
			{
				return;
			}
			if (clan == Clan.PlayerClan || clan.TotalStrength <= 0f)
			{
				return;
			}
			if (clan.IsBanditFaction)
			{
				return;
			}
			if (clan.Kingdom == null)
			{
				return;
			}
			if (clan.Influence < 100f)
			{
				return;
			}
			KingdomDecision kingdomDecision = null;
			float randomFloat = MBRandom.RandomFloat;
			int num = ((Kingdom)clan.MapFaction).Clans.Count((Clan x) => x.Influence > 100f);
			float num2 = MathF.Min(0.33f, 1f / ((float)num + 2f));
			num2 *= ((clan.Kingdom == Hero.MainHero.MapFaction && !Hero.MainHero.Clan.IsUnderMercenaryService) ? ((clan.Kingdom.Leader == Hero.MainHero) ? 0.5f : 0.75f) : 1f);
			DiplomacyModel diplomacyModel = Campaign.Current.Models.DiplomacyModel;
			if (randomFloat < num2 && clan.Influence > (float)diplomacyModel.GetInfluenceCostOfProposingPeace(clan))
			{
				kingdomDecision = this.GetRandomPeaceDecision(clan);
			}
			else if (randomFloat < num2 * 2f && clan.Influence > (float)diplomacyModel.GetInfluenceCostOfProposingWar(clan))
			{
				kingdomDecision = this.GetRandomWarDecision(clan);
			}
			else if (randomFloat < num2 * 2.5f && clan.Influence > (float)(diplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan) * 4))
			{
				kingdomDecision = this.GetRandomPolicyDecision(clan);
			}
			else if (randomFloat < num2 * 3f && clan.Influence > 700f)
			{
				kingdomDecision = this.GetRandomAnnexationDecision(clan);
			}
			if (kingdomDecision != null)
			{
				if (this._kingdomDecisionsList == null)
				{
					this._kingdomDecisionsList = new List<KingdomDecision>();
				}
				bool flag = false;
				if (kingdomDecision is MakePeaceKingdomDecision && ((MakePeaceKingdomDecision)kingdomDecision).FactionToMakePeaceWith == Hero.MainHero.MapFaction)
				{
					foreach (KingdomDecision kingdomDecision2 in this._kingdomDecisionsList)
					{
						if (kingdomDecision2 is MakePeaceKingdomDecision && kingdomDecision2.Kingdom == Hero.MainHero.MapFaction && ((MakePeaceKingdomDecision)kingdomDecision2).FactionToMakePeaceWith == clan.Kingdom && kingdomDecision2.TriggerTime.IsFuture)
						{
							flag = true;
						}
						if (kingdomDecision2 is MakePeaceKingdomDecision && kingdomDecision2.Kingdom == clan.Kingdom && ((MakePeaceKingdomDecision)kingdomDecision2).FactionToMakePeaceWith == Hero.MainHero.MapFaction && kingdomDecision2.TriggerTime.IsFuture)
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					bool flag2 = false;
					foreach (KingdomDecision kingdomDecision3 in this._kingdomDecisionsList)
					{
						DeclareWarDecision declareWarDecision;
						DeclareWarDecision declareWarDecision2;
						MakePeaceKingdomDecision makePeaceKingdomDecision;
						MakePeaceKingdomDecision makePeaceKingdomDecision2;
						if ((declareWarDecision = (kingdomDecision3 as DeclareWarDecision)) != null && (declareWarDecision2 = (kingdomDecision as DeclareWarDecision)) != null && declareWarDecision.FactionToDeclareWarOn == declareWarDecision2.FactionToDeclareWarOn && declareWarDecision.ProposerClan.MapFaction == declareWarDecision2.ProposerClan.MapFaction)
						{
							flag2 = true;
						}
						else if ((makePeaceKingdomDecision = (kingdomDecision3 as MakePeaceKingdomDecision)) != null && (makePeaceKingdomDecision2 = (kingdomDecision as MakePeaceKingdomDecision)) != null && makePeaceKingdomDecision.FactionToMakePeaceWith == makePeaceKingdomDecision2.FactionToMakePeaceWith && makePeaceKingdomDecision.ProposerClan.MapFaction == makePeaceKingdomDecision2.ProposerClan.MapFaction)
						{
							flag2 = true;
						}
					}
					if (!flag2)
					{
						this._kingdomDecisionsList.Add(kingdomDecision);
						new KingdomElection(kingdomDecision);
						clan.Kingdom.AddDecision(kingdomDecision, false);
						return;
					}
				}
			}
			else
			{
				this.UpdateKingdomDecisions(clan.Kingdom);
			}
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000FE580 File Offset: 0x000FC780
		private void HourlyTick()
		{
			if (Clan.PlayerClan.Kingdom != null)
			{
				this.UpdateKingdomDecisions(Clan.PlayerClan.Kingdom);
			}
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000FE5A0 File Offset: 0x000FC7A0
		private void DailyTick()
		{
			if (this._kingdomDecisionsList != null)
			{
				int count = this._kingdomDecisionsList.Count;
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					if (this._kingdomDecisionsList[i - num].TriggerTime.ElapsedDaysUntilNow > 15f)
					{
						this._kingdomDecisionsList.RemoveAt(i - num);
						num++;
					}
				}
			}
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000FE604 File Offset: 0x000FC804
		public void UpdateKingdomDecisions(Kingdom kingdom)
		{
			List<KingdomDecision> list = new List<KingdomDecision>();
			List<KingdomDecision> list2 = new List<KingdomDecision>();
			foreach (KingdomDecision kingdomDecision in kingdom.UnresolvedDecisions)
			{
				if (kingdomDecision.ShouldBeCancelled())
				{
					list.Add(kingdomDecision);
				}
				else if (kingdomDecision.TriggerTime.IsPast && !kingdomDecision.NeedsPlayerResolution)
				{
					list2.Add(kingdomDecision);
				}
			}
			foreach (KingdomDecision kingdomDecision2 in list)
			{
				kingdom.RemoveDecision(kingdomDecision2);
				bool flag;
				if (!kingdomDecision2.DetermineChooser().Leader.IsHumanPlayerCharacter)
				{
					flag = kingdomDecision2.DetermineSupporters().Any((Supporter x) => x.IsPlayer);
				}
				else
				{
					flag = true;
				}
				bool isPlayerInvolved = flag;
				CampaignEventDispatcher.Instance.OnKingdomDecisionCancelled(kingdomDecision2, isPlayerInvolved);
			}
			foreach (KingdomDecision decision in list2)
			{
				new KingdomElection(decision).StartElectionWithoutPlayer();
			}
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000FE75C File Offset: 0x000FC95C
		private void OnPeaceMade(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			this.HandleDiplomaticChangeBetweenFactions(side1Faction, side2Faction);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000FE766 File Offset: 0x000FC966
		private void OnWarDeclared(IFaction side1Faction, IFaction side2Faction, DeclareWarAction.DeclareWarDetail detail)
		{
			this.HandleDiplomaticChangeBetweenFactions(side1Faction, side2Faction);
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x000FE770 File Offset: 0x000FC970
		private void HandleDiplomaticChangeBetweenFactions(IFaction side1Faction, IFaction side2Faction)
		{
			if (side1Faction.IsKingdomFaction && side2Faction.IsKingdomFaction)
			{
				this.UpdateKingdomDecisions((Kingdom)side1Faction);
				this.UpdateKingdomDecisions((Kingdom)side2Faction);
			}
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x000FE79C File Offset: 0x000FC99C
		private KingdomDecision GetRandomWarDecision(Clan clan)
		{
			KingdomDecision result = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault((KingdomDecision x) => x is DeclareWarDecision) != null)
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate((Kingdom x) => !x.IsEliminated && x != kingdom && !x.IsAtWarWith(kingdom) && x.GetStanceWith(kingdom).PeaceDeclarationDate.ElapsedDaysUntilNow > 20f);
			if (randomElementWithPredicate != null && this.ConsiderWar(clan, kingdom, randomElementWithPredicate))
			{
				result = new DeclareWarDecision(clan, randomElementWithPredicate);
			}
			return result;
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x000FE824 File Offset: 0x000FCA24
		private KingdomDecision GetRandomPeaceDecision(Clan clan)
		{
			KingdomDecision result = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault((KingdomDecision x) => x is MakePeaceKingdomDecision) != null)
			{
				return null;
			}
			Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate((Kingdom x) => x.IsAtWarWith(kingdom));
			MakePeaceKingdomDecision makePeaceKingdomDecision;
			if (randomElementWithPredicate != null && this.ConsiderPeace(clan, randomElementWithPredicate.RulingClan, kingdom, randomElementWithPredicate, out makePeaceKingdomDecision))
			{
				result = makePeaceKingdomDecision;
			}
			return result;
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x000FE8B0 File Offset: 0x000FCAB0
		private bool ConsiderWar(Clan clan, Kingdom kingdom, IFaction otherFaction)
		{
			int num = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingWar(clan) / 2;
			if (clan.Influence < (float)num)
			{
				return false;
			}
			DeclareWarDecision declareWarDecision = new DeclareWarDecision(clan, otherFaction);
			if (declareWarDecision.CalculateSupport(clan) > 50f)
			{
				float kingdomSupportForDecision = this.GetKingdomSupportForDecision(declareWarDecision);
				if (MBRandom.RandomFloat < 1.4f * kingdomSupportForDecision - 0.55f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000FE916 File Offset: 0x000FCB16
		private float GetKingdomSupportForWar(Clan clan, Kingdom kingdom, IFaction otherFaction)
		{
			return new KingdomElection(new DeclareWarDecision(clan, otherFaction)).GetLikelihoodForSponsor(clan);
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000FE92C File Offset: 0x000FCB2C
		private bool ConsiderPeace(Clan clan, Clan otherClan, Kingdom kingdom, IFaction otherFaction, out MakePeaceKingdomDecision decision)
		{
			decision = null;
			int influenceCostOfProposingPeace = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingPeace(clan);
			if (clan.Influence < (float)influenceCostOfProposingPeace)
			{
				return false;
			}
			int num = new PeaceBarterable(clan.Leader, kingdom, otherFaction, CampaignTime.Years(1f)).GetValueForFaction(otherFaction);
			int num2 = -num;
			if (clan.MapFaction == Hero.MainHero.MapFaction && otherFaction is Kingdom)
			{
				foreach (Clan clan2 in ((Kingdom)otherFaction).Clans)
				{
					if (clan2.Leader != clan2.MapFaction.Leader)
					{
						int valueForFaction = new PeaceBarterable(clan2.Leader, kingdom, otherFaction, CampaignTime.Years(1f)).GetValueForFaction(clan2);
						if (valueForFaction < num)
						{
							num = valueForFaction;
						}
					}
				}
				num2 = -num;
			}
			else
			{
				num2 += 30000;
			}
			if (otherFaction is Clan && num2 < 0)
			{
				num2 = 0;
			}
			float num3 = 0.5f;
			if (otherFaction == Hero.MainHero.MapFaction)
			{
				PeaceBarterable peaceBarterable = new PeaceBarterable(clan.MapFaction.Leader, kingdom, otherFaction, CampaignTime.Years(1f));
				int num4 = peaceBarterable.GetValueForFaction(clan.MapFaction);
				int num5 = 0;
				int num6 = 1;
				if (clan.MapFaction is Kingdom)
				{
					foreach (Clan clan3 in ((Kingdom)clan.MapFaction).Clans)
					{
						if (clan3.Leader != clan3.MapFaction.Leader)
						{
							int valueForFaction2 = peaceBarterable.GetValueForFaction(clan3);
							if (valueForFaction2 < num4)
							{
								num4 = valueForFaction2;
							}
							num5 += valueForFaction2;
							num6++;
						}
					}
				}
				float num7 = (float)num5 / (float)num6;
				int num8 = (int)(0.65f * num7 + 0.35f * (float)num4);
				if (num8 > num2)
				{
					num2 = num8;
					num3 = 0.2f;
				}
			}
			int num9 = num2;
			if (num2 > -5000 && num2 < 5000)
			{
				num2 = 0;
			}
			int dailyTributeForValue = Campaign.Current.Models.DiplomacyModel.GetDailyTributeForValue(num2);
			decision = new MakePeaceKingdomDecision(clan, otherFaction, dailyTributeForValue, true);
			if (decision.CalculateSupport(clan) > 5f)
			{
				float kingdomSupportForDecision = this.GetKingdomSupportForDecision(decision);
				if (MBRandom.RandomFloat < 2f * (kingdomSupportForDecision - num3))
				{
					if (otherFaction == Hero.MainHero.MapFaction)
					{
						num2 = num9 + 15000;
						if (num2 > -5000 && num2 < 5000)
						{
							num2 = 0;
						}
						int dailyTributeForValue2 = Campaign.Current.Models.DiplomacyModel.GetDailyTributeForValue(num2);
						decision = new MakePeaceKingdomDecision(clan, otherFaction, dailyTributeForValue2, true);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000FEBFC File Offset: 0x000FCDFC
		private float GetKingdomSupportForPeace(Clan clan, Clan otherClan, Kingdom kingdom, IFaction otherFaction)
		{
			int num = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfProposingPeace(clan) / 2;
			int num2 = -new PeaceBarterable(clan.Leader, kingdom, otherFaction, CampaignTime.Years(1f)).GetValueForFaction(otherFaction);
			if (otherFaction is Clan && num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > -5000 && num2 < 5000)
			{
				num2 = 0;
			}
			int dailyTributeForValue = Campaign.Current.Models.DiplomacyModel.GetDailyTributeForValue(num2);
			return new KingdomElection(new MakePeaceKingdomDecision(clan, otherFaction, dailyTributeForValue, true)).GetLikelihoodForSponsor(clan);
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000FEC90 File Offset: 0x000FCE90
		private KingdomDecision GetRandomPolicyDecision(Clan clan)
		{
			KingdomDecision result = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault((KingdomDecision x) => x is KingdomPolicyDecision) != null)
			{
				return null;
			}
			if (clan.Influence < 200f)
			{
				return null;
			}
			PolicyObject randomElement = PolicyObject.All.GetRandomElement<PolicyObject>();
			bool flag = kingdom.ActivePolicies.Contains(randomElement);
			if (this.ConsiderPolicy(clan, kingdom, randomElement, flag))
			{
				result = new KingdomPolicyDecision(clan, randomElement, flag);
			}
			return result;
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000FED14 File Offset: 0x000FCF14
		private bool ConsiderPolicy(Clan clan, Kingdom kingdom, PolicyObject policy, bool invert)
		{
			int influenceCostOfPolicyProposalAndDisavowal = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan);
			if (clan.Influence < (float)influenceCostOfPolicyProposalAndDisavowal)
			{
				return false;
			}
			KingdomPolicyDecision kingdomPolicyDecision = new KingdomPolicyDecision(clan, policy, invert);
			if (kingdomPolicyDecision.CalculateSupport(clan) > 50f)
			{
				float kingdomSupportForDecision = this.GetKingdomSupportForDecision(kingdomPolicyDecision);
				if ((double)MBRandom.RandomFloat < (double)kingdomSupportForDecision - 0.55)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x000FED7A File Offset: 0x000FCF7A
		private float GetKingdomSupportForPolicy(Clan clan, Kingdom kingdom, PolicyObject policy, bool invert)
		{
			Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfPolicyProposalAndDisavowal(clan);
			return new KingdomElection(new KingdomPolicyDecision(clan, policy, invert)).GetLikelihoodForSponsor(clan);
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x000FEDA8 File Offset: 0x000FCFA8
		private KingdomDecision GetRandomAnnexationDecision(Clan clan)
		{
			KingdomDecision result = null;
			Kingdom kingdom = clan.Kingdom;
			if (kingdom.UnresolvedDecisions.FirstOrDefault((KingdomDecision x) => x is KingdomPolicyDecision) != null)
			{
				return null;
			}
			if (clan.Influence < 300f)
			{
				return null;
			}
			Clan randomElement = kingdom.Clans.GetRandomElement<Clan>();
			if (randomElement != null && randomElement != clan && randomElement.GetRelationWithClan(clan) < -25)
			{
				if (randomElement.Fiefs.Count == 0)
				{
					return null;
				}
				Town randomElement2 = randomElement.Fiefs.GetRandomElement<Town>();
				if (this.ConsiderAnnex(clan, randomElement2))
				{
					result = new SettlementClaimantPreliminaryDecision(clan, randomElement2.Settlement);
				}
			}
			return result;
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x000FEE4C File Offset: 0x000FD04C
		private bool ConsiderAnnex(Clan clan, Town targetSettlement)
		{
			int influenceCostOfAnnexation = Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfAnnexation(clan);
			if (clan.Influence < (float)influenceCostOfAnnexation)
			{
				return false;
			}
			SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision = new SettlementClaimantPreliminaryDecision(clan, targetSettlement.Settlement);
			if (settlementClaimantPreliminaryDecision.CalculateSupport(clan) > 50f)
			{
				float kingdomSupportForDecision = this.GetKingdomSupportForDecision(settlementClaimantPreliminaryDecision);
				if ((double)MBRandom.RandomFloat < (double)kingdomSupportForDecision - 0.6)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x000FEEB5 File Offset: 0x000FD0B5
		private float GetKingdomSupportForDecision(KingdomDecision decision)
		{
			return new KingdomElection(decision).GetLikelihoodForOutcome(0);
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x000FEEC3 File Offset: 0x000FD0C3
		private void SessionLaunched(CampaignGameStarter starter)
		{
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x000FEEC5 File Offset: 0x000FD0C5
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<KingdomDecision>>("_kingdomDecisionsList", ref this._kingdomDecisionsList);
		}

		// Token: 0x04001181 RID: 4481
		private const int KingdomDecisionProposalCooldownInDays = 1;

		// Token: 0x04001182 RID: 4482
		private const float ClanInterestModifier = 1f;

		// Token: 0x04001183 RID: 4483
		private const float DecisionSuccessChanceModifier = 1f;

		// Token: 0x04001184 RID: 4484
		private List<KingdomDecision> _kingdomDecisionsList;

		// Token: 0x02000707 RID: 1799
		// (Invoke) Token: 0x06005802 RID: 22530
		private delegate KingdomDecision KingdomDecisionCreatorDelegate(Clan sponsorClan);
	}
}
