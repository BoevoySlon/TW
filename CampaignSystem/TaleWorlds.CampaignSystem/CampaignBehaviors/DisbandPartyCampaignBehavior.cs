using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000387 RID: 903
	public class DisbandPartyCampaignBehavior : CampaignBehaviorBase, IDisbandPartyCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x06003591 RID: 13713 RVA: 0x000E8448 File Offset: 0x000E6648
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnPartyDisbandStartedEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyDisbandStarted));
			CampaignEvents.OnPartyDisbandCanceledEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyDisbandCanceled));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.OnPartyDisbandedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnPartyDisbanded));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.HourlyTickParty));
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000E8569 File Offset: 0x000E6769
		public bool IsPartyWaitingForDisband(MobileParty party)
		{
			return this._partiesThatWaitingToDisband.ContainsKey(party);
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000E8577 File Offset: 0x000E6777
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<MobileParty, CampaignTime>>("_partiesThatWaitingToDisband", ref this._partiesThatWaitingToDisband);
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000E858C File Offset: 0x000E678C
		private void OnGameLoadFinished()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				for (int i = kingdom.Armies.Count - 1; i >= 0; i--)
				{
					Army army = kingdom.Armies[i];
					for (int j = army.Parties.Count - 1; j >= 0; j--)
					{
						MobileParty mobileParty = army.Parties[j];
						if (army.LeaderParty != mobileParty && mobileParty.LeaderHero == null)
						{
							DisbandPartyAction.StartDisband(mobileParty);
							mobileParty.Army = null;
						}
					}
					if (army.LeaderParty.LeaderHero == null)
					{
						DisbandPartyAction.StartDisband(army.LeaderParty);
					}
				}
			}
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000E8668 File Offset: 0x000E6868
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000E8674 File Offset: 0x000E6874
		private void OnPartyDisbandStarted(MobileParty party)
		{
			if (party.ActualClan == Clan.PlayerClan || party.MemberRoster.Count < 10)
			{
				if (party.IsCaravan && party.ActualClan == Clan.PlayerClan)
				{
					party.Ai.SetDoNotMakeNewDecisions(true);
					Settlement settlement;
					this.GetTargetSettlementForDisbandingPlayerClanCaravan(party, out settlement);
					if (settlement != null)
					{
						party.Ai.SetMoveGoToSettlement(settlement);
					}
				}
				this._partiesThatWaitingToDisband.Add(party, CampaignTime.HoursFromNow(24f));
				return;
			}
			Hero hero = null;
			foreach (Hero hero2 in party.ActualClan.Heroes)
			{
				if (hero2.PartyBelongedTo == null && hero2.IsActive && hero2.DeathMark == KillCharacterAction.KillCharacterActionDetail.None && hero2.CurrentSettlement != null && hero2.GovernorOf == null && (!hero2.CurrentSettlement.IsUnderSiege || !hero2.CurrentSettlement.IsUnderRaid))
				{
					hero = hero2;
					break;
				}
			}
			if (hero != null)
			{
				TeleportHeroAction.ApplyDelayedTeleportToPartyAsPartyLeader(hero, party);
				return;
			}
			this._partiesThatWaitingToDisband.Add(party, CampaignTime.HoursFromNow(24f));
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000E87A0 File Offset: 0x000E69A0
		private void OnPartyDisbandCanceled(MobileParty party)
		{
			if (this._partiesThatWaitingToDisband.ContainsKey(party))
			{
				this._partiesThatWaitingToDisband.Remove(party);
			}
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000E87C0 File Offset: 0x000E69C0
		private void HourlyTick()
		{
			List<MobileParty> list = new List<MobileParty>();
			foreach (KeyValuePair<MobileParty, CampaignTime> keyValuePair in this._partiesThatWaitingToDisband)
			{
				if (keyValuePair.Value.IsPast)
				{
					keyValuePair.Key.IsDisbanding = true;
					list.Add(keyValuePair.Key);
				}
			}
			foreach (MobileParty key in list)
			{
				this._partiesThatWaitingToDisband.Remove(key);
			}
			list = null;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x000E8888 File Offset: 0x000E6A88
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._partiesThatWaitingToDisband.ContainsKey(mobileParty))
			{
				this._partiesThatWaitingToDisband.Remove(mobileParty);
			}
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x000E88A5 File Offset: 0x000E6AA5
		private void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			if (targetParty != null && detail == TeleportHeroAction.TeleportationDetail.DelayedTeleportToPartyAsPartyLeader && this._partiesThatWaitingToDisband.ContainsKey(targetParty))
			{
				this._partiesThatWaitingToDisband.Remove(targetParty);
			}
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000E88CC File Offset: 0x000E6ACC
		private void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			if (prisoner == Hero.MainHero)
			{
				foreach (WarPartyComponent warPartyComponent in Clan.PlayerClan.WarPartyComponents)
				{
					if (warPartyComponent.MobileParty != null && warPartyComponent.MobileParty.LeaderHero == null)
					{
						CampaignEventDispatcher.Instance.OnPartyLeaderChangeOfferCanceled(warPartyComponent.MobileParty);
					}
				}
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000E894C File Offset: 0x000E6B4C
		private void DailyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.IsDisbanding && mobileParty.MapEvent == null && mobileParty.IsActive && mobileParty.TargetSettlement != null)
			{
				this.CheckDisbandedPartyDaily(mobileParty, mobileParty.TargetSettlement);
			}
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000E897C File Offset: 0x000E6B7C
		private void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty.IsCaravan && mobileParty.ActualClan == Clan.PlayerClan && !mobileParty.IsDisbanding && this._partiesThatWaitingToDisband.ContainsKey(mobileParty) && mobileParty.CurrentSettlement == null && mobileParty.TargetSettlement != null)
			{
				Settlement moveGoToSettlement;
				this.GetTargetSettlementForDisbandingPlayerClanCaravan(mobileParty, out moveGoToSettlement);
				mobileParty.Ai.SetMoveGoToSettlement(moveGoToSettlement);
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000E89DC File Offset: 0x000E6BDC
		private void HourlyTickParty(MobileParty party)
		{
			if (party.Ai.DefaultBehavior == AiBehavior.Hold && party.Ai.DoNotMakeNewDecisions && (party.IsDisbanding || this._partiesThatWaitingToDisband.ContainsKey(party)))
			{
				Settlement settlement;
				this.GetTargetSettlementForDisbandingPlayerClanCaravan(party, out settlement);
				if (settlement != null)
				{
					party.Ai.SetMoveGoToSettlement(settlement);
				}
			}
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000E8A34 File Offset: 0x000E6C34
		private void GetTargetSettlementForDisbandingPlayerClanCaravan(MobileParty mobileParty, out Settlement targetSettlement)
		{
			float num = 0f;
			targetSettlement = null;
			foreach (Settlement settlement in mobileParty.MapFaction.Settlements)
			{
				if (settlement.IsFortification)
				{
					float num2 = DisbandPartyCampaignBehavior.CalculateTargetSettlementScore(mobileParty, settlement);
					if (num2 > num)
					{
						targetSettlement = settlement;
						num = num2;
					}
				}
			}
			if (targetSettlement != null)
			{
				return;
			}
			int num3 = -1;
			Func<Settlement, bool> <>9__0;
			do
			{
				num3 = SettlementHelper.FindNextSettlementAroundMapPoint(mobileParty, 40f, num3);
				if (num3 >= 0)
				{
					Settlement settlement2 = Settlement.All[num3];
					if (settlement2.OwnerClan != null && !settlement2.OwnerClan.IsAtWarWith(mobileParty.MapFaction) && settlement2.IsFortification)
					{
						float num4 = DisbandPartyCampaignBehavior.CalculateTargetSettlementScore(mobileParty, settlement2);
						if (num4 > num)
						{
							targetSettlement = settlement2;
							num = num4;
						}
					}
					if (targetSettlement == null)
					{
						Func<Settlement, bool> condition;
						if ((condition = <>9__0) == null)
						{
							condition = (<>9__0 = ((Settlement s) => s.OwnerClan != null && !s.OwnerClan.IsAtWarWith(mobileParty.MapFaction)));
						}
						targetSettlement = SettlementHelper.FindNearestFortification(condition, null);
					}
				}
			}
			while (num3 >= 0);
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000E8B6C File Offset: 0x000E6D6C
		private static float CalculateTargetSettlementScore(MobileParty disbandParty, Settlement settlement)
		{
			float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(disbandParty, settlement);
			float num = MathF.Pow(1f - 0.95f * (MathF.Min(Campaign.MapDiagonal, distance) / Campaign.MapDiagonal), 3f);
			Hero owner = disbandParty.Party.Owner;
			float num2;
			if (((owner != null) ? owner.Clan : null) != settlement.OwnerClan)
			{
				Hero owner2 = disbandParty.Party.Owner;
				num2 = ((((owner2 != null) ? owner2.MapFaction : null) == settlement.MapFaction) ? 0.1f : 0.01f);
			}
			else
			{
				num2 = 1f;
			}
			float num3 = num2;
			float num4 = (disbandParty.DefaultBehavior == AiBehavior.GoToSettlement && disbandParty.TargetSettlement == settlement) ? 1f : 0.3f;
			return num * num3 * num4;
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000E8C2C File Offset: 0x000E6E2C
		private void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
			if (relatedSettlement != null)
			{
				if (relatedSettlement.IsFortification)
				{
					if (relatedSettlement.Town.GarrisonParty != null && relatedSettlement.Town.GarrisonParty.MapEvent == null)
					{
						this.MergeDisbandPartyToFortification(disbandParty, relatedSettlement.Town.GarrisonParty.Party, relatedSettlement);
						return;
					}
				}
				else if (relatedSettlement.IsVillage && relatedSettlement.Village.VillageState != Village.VillageStates.BeingRaided)
				{
					this.MergeDisbandPartyToVillage(disbandParty, relatedSettlement);
				}
			}
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000E8C9C File Offset: 0x000E6E9C
		private void MergeDisbandPartyToFortification(MobileParty disbandParty, PartyBase mergeToParty, Settlement settlement)
		{
			foreach (TroopRosterElement troopRosterElement in disbandParty.PrisonRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsHero)
				{
					if (!mergeToParty.MapFaction.IsAtWarWith(troopRosterElement.Character.HeroObject.MapFaction))
					{
						EndCaptivityAction.ApplyByPeace(troopRosterElement.Character.HeroObject, null);
					}
					else
					{
						TransferPrisonerAction.Apply(troopRosterElement.Character, disbandParty.Party, settlement.Party);
					}
				}
				else
				{
					mergeToParty.AddPrisoner(troopRosterElement.Character, troopRosterElement.Number);
				}
			}
			foreach (TroopRosterElement troopRosterElement2 in disbandParty.MemberRoster.GetTroopRoster())
			{
				disbandParty.MemberRoster.RemoveTroop(troopRosterElement2.Character, troopRosterElement2.Number, default(UniqueTroopDescriptor), 0);
				if (troopRosterElement2.Character.IsHero && troopRosterElement2.Character.HeroObject.IsAlive)
				{
					EnterSettlementAction.ApplyForCharacterOnly(troopRosterElement2.Character.HeroObject, settlement);
				}
				else
				{
					mergeToParty.MemberRoster.AddToCounts(troopRosterElement2.Character, troopRosterElement2.Number, false, troopRosterElement2.WoundedNumber, troopRosterElement2.Xp, true, -1);
				}
			}
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000E8E1C File Offset: 0x000E701C
		private void MergeDisbandPartyToVillage(MobileParty disbandParty, Settlement settlement)
		{
			foreach (TroopRosterElement troopRosterElement in disbandParty.PrisonRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsHero)
				{
					EndCaptivityAction.ApplyByEscape(troopRosterElement.Character.HeroObject, null);
				}
			}
			foreach (TroopRosterElement troopRosterElement2 in disbandParty.MemberRoster.GetTroopRoster())
			{
				if (troopRosterElement2.Character.IsHero && troopRosterElement2.Character.HeroObject.IsAlive)
				{
					disbandParty.MemberRoster.RemoveTroop(troopRosterElement2.Character, 1, default(UniqueTroopDescriptor), 0);
					EnterSettlementAction.ApplyForCharacterOnly(troopRosterElement2.Character.HeroObject, settlement);
				}
			}
			float num = (float)disbandParty.MemberRoster.TotalManCount * 0.5f;
			settlement.Militia += num;
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x000E8F3C File Offset: 0x000E713C
		private void CheckDisbandedPartyDaily(MobileParty disbandParty, Settlement settlement)
		{
			if (disbandParty.MemberRoster.Count == 0)
			{
				DestroyPartyAction.Apply(null, disbandParty);
				return;
			}
			if (settlement == null && disbandParty.StationaryStartTime.ElapsedHoursUntilNow >= 3f)
			{
				foreach (TroopRosterElement troopRosterElement in disbandParty.MemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character.IsHero && !troopRosterElement.Character.IsPlayerCharacter && !troopRosterElement.Character.HeroObject.IsDead)
					{
						MakeHeroFugitiveAction.Apply(troopRosterElement.Character.HeroObject);
					}
				}
				DestroyPartyAction.Apply(null, disbandParty);
				return;
			}
			if (settlement != null && settlement == disbandParty.CurrentSettlement)
			{
				DestroyPartyAction.ApplyForDisbanding(disbandParty, settlement);
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000E9014 File Offset: 0x000E7214
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("disbanding_leaderless_party_start", "start", "disbanding_leaderless_party_start_response", "{=!}{EXPLANATION}", new ConversationSentence.OnConditionDelegate(this.disbanding_leaderless_party_start_on_condition), null, 500, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_take_party", "disbanding_leaderless_party_start_response", "close_window", "{=eyZo8ZTk}Let me inspect the party troops.", new ConversationSentence.OnConditionDelegate(this.disbanding_leaderless_party_join_main_party_answer_condition), new ConversationSentence.OnConsequenceDelegate(this.disbanding_leaderless_party_join_main_party_answer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_neutral", "disbanding_leaderless_party_start_response", "attack_disbanding_party_neutral_response", "{=SXgm2b1M}You're not going anywhere. Not with your valuables, anyway.", new ConversationSentence.OnConditionDelegate(this.attack_neutral_disbanding_party_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("disbanding_leaderless_party_answer_attack_neutral_di", "attack_disbanding_party_neutral_response", "attack_disbanding_party_neutral_player_response", "{=CgS44dOE}Are you mad? We're not your enemy.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_neutral_2", "attack_disbanding_party_neutral_player_response", "close_window", "{=Mt5F4wE2}No, you're my prey. Prepare to fight!", null, new ConversationSentence.OnConsequenceDelegate(this.attack_disbanding_party_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_neutral_3", "attack_disbanding_party_neutral_player_response", "close_window", "{=XrQBTVis}I don't know what I was thinking. Go on, then...", null, new ConversationSentence.OnConsequenceDelegate(this.disbanding_leaderless_party_answer_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_attack_enemy", "disbanding_leaderless_party_start_response", "attack_disbanding_enemy_response", "{=WwLy9Src}You know we're at war. I can't just let you go.", new ConversationSentence.OnConditionDelegate(this.attack_enemy_disbanding_party_condition), null, 100, null, null);
			campaignGameStarter.AddDialogLine("disbanding_leaderless_party_answer", "attack_disbanding_enemy_response", "close_window", "{=jBN2LlgF}We'll fight to our last drop of blood!", null, new ConversationSentence.OnConsequenceDelegate(this.attack_disbanding_party_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("disbanding_leaderless_party_answer_2", "disbanding_leaderless_party_start_response", "close_window", "{=disband_party_campaign_behaviorbdisbanding_leaderless_party_answer}Well... Go on, then.", null, new ConversationSentence.OnConsequenceDelegate(this.disbanding_leaderless_party_answer_on_consequence), 100, null, null);
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000E91B0 File Offset: 0x000E73B0
		private bool disbanding_leaderless_party_start_on_condition()
		{
			bool flag = MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsLordParty && (MobileParty.ConversationParty.LeaderHero == null || MobileParty.ConversationParty.IsDisbanding || this.IsPartyWaitingForDisband(MobileParty.ConversationParty));
			if (flag)
			{
				if (MobileParty.ConversationParty.LeaderHero == null)
				{
					if (MobileParty.ConversationParty.TargetSettlement != null)
					{
						TextObject textObject = new TextObject("{=9IwzVbJf}We recently lost our leader, now we are traveling to {TARGET_SETTLEMENT}. We will rejoin the garrison unless we are assigned a new leader.", null);
						textObject.SetTextVariable("TARGET_SETTLEMENT", MobileParty.ConversationParty.TargetSettlement.EncyclopediaLinkWithName);
						MBTextManager.SetTextVariable("EXPLANATION", textObject, false);
						return flag;
					}
					MBTextManager.SetTextVariable("EXPLANATION", new TextObject("{=COEifaao}We recently lost our leader. We are now waiting for new orders.", null), false);
					return flag;
				}
				else
				{
					if (MobileParty.ConversationParty.TargetSettlement != null)
					{
						TextObject textObject2 = new TextObject("{=uZIlfFa2}We're disbanding. We're all going to {TARGET_SETTLEMENT_LINK}, then we're going our separate ways.", null);
						textObject2.SetTextVariable("TARGET_SETTLEMENT_LINK", MobileParty.ConversationParty.TargetSettlement.EncyclopediaLinkWithName);
						MBTextManager.SetTextVariable("EXPLANATION", textObject2, false);
						return flag;
					}
					MBTextManager.SetTextVariable("EXPLANATION", new TextObject("{=G1PN6ku4}We're disbanding.", null), false);
				}
			}
			return flag;
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000E92B8 File Offset: 0x000E74B8
		private bool disbanding_leaderless_party_join_main_party_answer_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.Party.Owner != null && MobileParty.ConversationParty.Party.Owner.Clan == Clan.PlayerClan;
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000E92EF File Offset: 0x000E74EF
		private void disbanding_leaderless_party_join_main_party_answer_on_consequence()
		{
			PlayerEncounter.LeaveEncounter = true;
			PartyScreenManager.OpenScreenAsManageTroopsAndPrisoners(MobileParty.ConversationParty, new PartyScreenClosedDelegate(this.OnPartyScreenClosed));
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000E930D File Offset: 0x000E750D
		private void OnPartyScreenClosed(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			if (leftOwnerParty.MemberRoster.TotalManCount <= 0)
			{
				DestroyPartyAction.Apply(null, leftOwnerParty.MobileParty);
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000E9329 File Offset: 0x000E7529
		private void disbanding_leaderless_party_answer_on_consequence()
		{
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x000E9331 File Offset: 0x000E7531
		private bool attack_neutral_disbanding_party_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.MapFaction != Clan.PlayerClan.MapFaction && !FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, MobileParty.ConversationParty.MapFaction);
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000E936E File Offset: 0x000E756E
		private bool attack_enemy_disbanding_party_condition()
		{
			return MobileParty.ConversationParty != null && MobileParty.ConversationParty.MapFaction != Clan.PlayerClan.MapFaction && FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, MobileParty.ConversationParty.MapFaction);
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000E93A8 File Offset: 0x000E75A8
		private void attack_disbanding_party_consequence()
		{
			PlayerEncounter.Current.IsEnemy = true;
			BeHostileAction.ApplyEncounterHostileAction(PartyBase.MainParty, MobileParty.ConversationParty.Party);
		}

		// Token: 0x0400113C RID: 4412
		private const int DisbandDelayTimeAsHours = 24;

		// Token: 0x0400113D RID: 4413
		private const int RemoveDisbandingPartyAfterHoldForHours = 3;

		// Token: 0x0400113E RID: 4414
		private const int DisbandPartySizeLimitForAIParties = 10;

		// Token: 0x0400113F RID: 4415
		private Dictionary<MobileParty, CampaignTime> _partiesThatWaitingToDisband = new Dictionary<MobileParty, CampaignTime>();
	}
}
