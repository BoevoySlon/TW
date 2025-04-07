using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000396 RID: 918
	public class HideoutCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600374F RID: 14159 RVA: 0x000FA25C File Offset: 0x000F845C
		public void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x000FA265 File Offset: 0x000F8465
		public void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x000FA270 File Offset: 0x000F8470
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnHideoutSpottedEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase>(this.OnHideoutSpotted));
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x000FA2D9 File Offset: 0x000F84D9
		private void OnHideoutSpotted(PartyBase party, PartyBase hideout)
		{
			SkillLevelingManager.OnHideoutSpotted(party.MobileParty, hideout);
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x000FA2E7 File Offset: 0x000F84E7
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<float>("_hideoutWaitProgressHours", ref this._hideoutWaitProgressHours);
			dataStore.SyncData<float>("_hideoutWaitTargetHours", ref this._hideoutWaitTargetHours);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x000FA310 File Offset: 0x000F8510
		public void HourlyTickSettlement(Settlement settlement)
		{
			if (settlement.IsHideout && settlement.Hideout.IsInfested && !settlement.Hideout.IsSpotted)
			{
				float hideoutSpottingDistance = Campaign.Current.Models.MapVisibilityModel.GetHideoutSpottingDistance();
				float num = MobileParty.MainParty.Position2D.DistanceSquared(settlement.Position2D);
				float num2 = 1f - num / (hideoutSpottingDistance * hideoutSpottingDistance);
				if (num2 > 0f && settlement.Parties.Count > 0 && MBRandom.RandomFloat < num2 && !settlement.Hideout.IsSpotted)
				{
					settlement.Hideout.IsSpotted = true;
					settlement.IsVisible = true;
					CampaignEventDispatcher.Instance.OnHideoutSpotted(MobileParty.MainParty.Party, settlement.Party);
				}
			}
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x000FA3DC File Offset: 0x000F85DC
		protected void AddGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenu("hideout_place", "{=!}{HIDEOUT_TEXT}", new OnInitDelegate(this.game_menu_hideout_place_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "wait", "{=4Sb0d8FY}Wait until nightfall to attack", new GameMenuOption.OnConditionDelegate(this.game_menu_wait_until_nightfall_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_wait_until_nightfall_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "attack", "{=zxMOqlhs}Attack", new GameMenuOption.OnConditionDelegate(this.game_menu_attack_hideout_parties_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_attack_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddWaitGameMenu("hideout_wait", "{=VLLAOXve}Waiting until nightfall to ambush", null, new OnConditionDelegate(this.hideout_wait_menu_on_condition), new OnConsequenceDelegate(this.hideout_wait_menu_on_consequence), new OnTickDelegate(this.hideout_wait_menu_on_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameOverlays.MenuOverlayType.None, this._hideoutWaitTargetHours, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_wait", "{=!}{HIDEOUT_TEXT}", new OnInitDelegate(this.hideout_after_wait_menu_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "attack", "{=zxMOqlhs}Attack", new GameMenuOption.OnConditionDelegate(this.game_menu_attack_hideout_parties_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_attack_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_defeated_and_saved", "{=1zLZf5rw}The rest of your men rushed to your help, dragging you out to safety and driving the bandits back into hiding.", new OnInitDelegate(this.game_menu_hideout_after_defeated_and_saved_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_defeated_and_saved", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x000FA5DC File Offset: 0x000F87DC
		private bool IsHideoutAttackableNow()
		{
			float currentHourInDay = CampaignTime.Now.CurrentHourInDay;
			return (this.CanAttackHideoutStart > this.CanAttackHideoutEnd && (currentHourInDay >= (float)this.CanAttackHideoutStart || currentHourInDay <= (float)this.CanAttackHideoutEnd)) || (this.CanAttackHideoutStart < this.CanAttackHideoutEnd && currentHourInDay >= (float)this.CanAttackHideoutStart && currentHourInDay <= (float)this.CanAttackHideoutEnd);
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000FA644 File Offset: 0x000F8844
		public bool hideout_wait_menu_on_condition(MenuCallbackArgs args)
		{
			return true;
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x000FA648 File Offset: 0x000F8848
		public void hideout_wait_menu_on_tick(MenuCallbackArgs args, CampaignTime campaignTime)
		{
			this._hideoutWaitProgressHours += (float)campaignTime.ToHours;
			if (this._hideoutWaitTargetHours.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this.CalculateHideoutAttackTime();
			}
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(this._hideoutWaitProgressHours / this._hideoutWaitTargetHours);
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x000FA6A4 File Offset: 0x000F88A4
		public void hideout_wait_menu_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_after_wait");
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000FA6B0 File Offset: 0x000F88B0
		private bool leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000FA6BC File Offset: 0x000F88BC
		[GameMenuInitializationHandler("hideout_wait")]
		[GameMenuInitializationHandler("hideout_after_wait")]
		[GameMenuInitializationHandler("hideout_after_defeated_and_saved")]
		private static void game_menu_hideout_ui_place_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Hideout.WaitMeshName);
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000FA6E8 File Offset: 0x000F88E8
		[GameMenuInitializationHandler("hideout_place")]
		private static void game_menu_hideout_sound_place_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_hideout");
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Hideout.WaitMeshName);
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000FA721 File Offset: 0x000F8921
		private void game_menu_hideout_after_defeated_and_saved_on_init(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.IsHideout)
			{
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x000FA744 File Offset: 0x000F8944
		private void game_menu_hideout_place_on_init(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.IsHideout)
			{
				return;
			}
			this._hideoutWaitProgressHours = 0f;
			if (!this.IsHideoutAttackableNow())
			{
				this.CalculateHideoutAttackTime();
			}
			else
			{
				this._hideoutWaitTargetHours = 0f;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			int num = 0;
			foreach (MobileParty mobileParty in currentSettlement.Parties)
			{
				num += mobileParty.MemberRoster.TotalManCount - mobileParty.MemberRoster.TotalWounded;
			}
			GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=DOmb81Mu}(Undefined hideout type)");
			if (currentSettlement.Culture.StringId.Equals("forest_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=cu2cLT5r}You spy though the trees what seems to be a clearing in the forest with what appears to be the outlines of a camp.");
			}
			if (currentSettlement.Culture.StringId.Equals("sea_raiders"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=bJ6ygV3P}As you travel along the coast, you see a sheltered cove with what appears to the outlines of a camp.");
			}
			if (currentSettlement.Culture.StringId.Equals("mountain_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=iyWUDSm8}Passing by the slopes of the mountains, you see an outcrop crowned with the ruins of an ancient fortress.");
			}
			if (currentSettlement.Culture.StringId.Equals("desert_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=b3iBOVXN}Passing by a wadi, you see what looks like a camouflaged well to tap the groundwater left behind by rare rainfalls.");
			}
			if (currentSettlement.Culture.StringId.Equals("steppe_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=5JaGVr0U}While traveling by a low range of hills, you see what appears to be the remains of a campsite in a stream gully.");
			}
			bool flag = !currentSettlement.Hideout.NextPossibleAttackTime.IsPast;
			if (flag)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=KLWn6yZQ}{HIDEOUT_DESCRIPTION} The remains of a fire suggest that it's been recently occupied, but its residents - whoever they are - are well-hidden for now.");
			}
			else if (num > 0)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=prcBBqMR}{HIDEOUT_DESCRIPTION} You see armed men moving about. As you listen quietly, you hear scraps of conversation about raids, ransoms, and the best places to waylay travellers.");
			}
			else
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=gywyEgZa}{HIDEOUT_DESCRIPTION} There seems to be no one inside.");
			}
			if (!flag && num > 0 && Hero.MainHero.IsWounded)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=fMekM2UH}{HIDEOUT_DESCRIPTION} You can not attack since your wounds do not allow you.");
			}
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			bool isInfested = Settlement.CurrentSettlement.Hideout.IsInfested;
			Settlement settlement = Settlement.CurrentSettlement.IsHideout ? Settlement.CurrentSettlement : null;
			if (PlayerEncounter.Battle != null)
			{
				bool flag2 = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Current.PlayerSide;
				PlayerEncounter.Update();
				if (flag2 && PlayerEncounter.Battle == null && settlement != null)
				{
					this.SetCleanHideoutRelations(settlement);
				}
			}
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x000FA994 File Offset: 0x000F8B94
		private void CalculateHideoutAttackTime()
		{
			this._hideoutWaitTargetHours = (((float)this.CanAttackHideoutStart > CampaignTime.Now.CurrentHourInDay) ? ((float)this.CanAttackHideoutStart - CampaignTime.Now.CurrentHourInDay) : (24f - CampaignTime.Now.CurrentHourInDay + (float)this.CanAttackHideoutStart));
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x000FA9F0 File Offset: 0x000F8BF0
		private void SetCleanHideoutRelations(Settlement hideout)
		{
			List<Settlement> list = new List<Settlement>();
			foreach (Village village in Campaign.Current.AllVillages)
			{
				if (village.Settlement.Position2D.DistanceSquared(hideout.Position2D) <= 1600f)
				{
					list.Add(village.Settlement);
				}
			}
			foreach (Settlement settlement in list)
			{
				if (settlement.Notables.Count > 0)
				{
					ChangeRelationAction.ApplyPlayerRelation(settlement.Notables.GetRandomElement<Hero>(), 2, true, false);
				}
			}
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
			{
				Town town = SettlementHelper.FindNearestTown(null, hideout).Town;
				Hero leader = town.OwnerClan.Leader;
				if (leader == Hero.MainHero)
				{
					town.Loyalty += 1f;
				}
				else
				{
					ChangeRelationAction.ApplyPlayerRelation(leader, (int)DefaultPerks.Charm.EffortForThePeople.PrimaryBonus, true, true);
				}
			}
			MBTextManager.SetTextVariable("RELATION_VALUE", (int)DefaultPerks.Charm.EffortForThePeople.PrimaryBonus);
			MBInformationManager.AddQuickInformation(new TextObject("{=o0qwDa0q}Your relation increased by {RELATION_VALUE} with nearby notables.", null), 0, null, "");
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x000FAB58 File Offset: 0x000F8D58
		private void hideout_after_wait_menu_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=VbU8Ue0O}After waiting for a while you find a good opportunity to close in undetected beneath the shroud of the night.", null);
			if (Hero.MainHero.IsWounded)
			{
				TextObject textObject2 = new TextObject("{=fMekM2UH}{HIDEOUT_DESCRIPTION}. You can not attack since your wounds do not allow you.", null);
				textObject2.SetTextVariable("HIDEOUT_DESCRIPTION", textObject);
				MBTextManager.SetTextVariable("HIDEOUT_TEXT", textObject2, false);
				return;
			}
			MBTextManager.SetTextVariable("HIDEOUT_TEXT", textObject, false);
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x000FABB0 File Offset: 0x000F8DB0
		private bool game_menu_attack_hideout_parties_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			Hideout hideout = Settlement.CurrentSettlement.Hideout;
			if (!Hero.MainHero.IsWounded && Settlement.CurrentSettlement.MapFaction != PartyBase.MainParty.MapFaction)
			{
				if (Settlement.CurrentSettlement.Parties.Any((MobileParty x) => x.IsBandit) && hideout.NextPossibleAttackTime.IsPast)
				{
					return this.IsHideoutAttackableNow();
				}
			}
			return false;
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x000FAC3C File Offset: 0x000F8E3C
		private void game_menu_encounter_attack_on_consequence(MenuCallbackArgs args)
		{
			int playerMaximumTroopCountForHideoutMission = Campaign.Current.Models.BanditDensityModel.GetPlayerMaximumTroopCountForHideoutMission(MobileParty.MainParty);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster strongestAndPriorTroops = MobilePartyHelper.GetStrongestAndPriorTroops(MobileParty.MainParty, playerMaximumTroopCountForHideoutMission, true);
			troopRoster.Add(strongestAndPriorTroops);
			Campaign campaign = Campaign.Current;
			int maxSelectableTroopCount = (campaign != null) ? campaign.Models.BanditDensityModel.GetPlayerMaximumTroopCountForHideoutMission(MobileParty.MainParty) : 0;
			args.MenuContext.OpenTroopSelection(MobileParty.MainParty.MemberRoster, troopRoster, new Func<CharacterObject, bool>(this.CanChangeStatusOfTroop), new Action<TroopRoster>(this.OnTroopRosterManageDone), maxSelectableTroopCount, 1);
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x000FACD0 File Offset: 0x000F8ED0
		private void ArrangeHideoutTroopCountsForMission()
		{
			int numberOfMinimumBanditTroopsInHideoutMission = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditTroopsInHideoutMission;
			int num = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumTroopCountForFirstFightInHideout + Campaign.Current.Models.BanditDensityModel.NumberOfMaximumTroopCountForBossFightInHideout;
			MBList<MobileParty> mblist = (from x in Settlement.CurrentSettlement.Parties
			where x.IsBandit || x.IsBanditBossParty
			select x).ToMBList<MobileParty>();
			int num2 = mblist.Sum((MobileParty x) => x.MemberRoster.TotalHealthyCount);
			if (num2 > num)
			{
				int i = num2 - num;
				mblist.RemoveAll((MobileParty x) => x.IsBanditBossParty || x.MemberRoster.TotalHealthyCount == 1);
				while (i > 0)
				{
					if (mblist.Count <= 0)
					{
						return;
					}
					MobileParty randomElement = mblist.GetRandomElement<MobileParty>();
					List<TroopRosterElement> troopRoster = randomElement.MemberRoster.GetTroopRoster();
					List<ValueTuple<TroopRosterElement, float>> list = new List<ValueTuple<TroopRosterElement, float>>();
					foreach (TroopRosterElement item in troopRoster)
					{
						list.Add(new ValueTuple<TroopRosterElement, float>(item, (float)(item.Number - item.WoundedNumber)));
					}
					TroopRosterElement troopRosterElement = MBRandom.ChooseWeighted<TroopRosterElement>(list);
					randomElement.MemberRoster.AddToCounts(troopRosterElement.Character, -1, false, 0, 0, true, -1);
					i--;
					if (randomElement.MemberRoster.TotalHealthyCount == 1)
					{
						mblist.Remove(randomElement);
					}
				}
			}
			else if (num2 < numberOfMinimumBanditTroopsInHideoutMission)
			{
				int num3 = numberOfMinimumBanditTroopsInHideoutMission - num2;
				mblist.RemoveAll((MobileParty x) => x.MemberRoster.GetTroopRoster().All((TroopRosterElement y) => y.Number == 0 || y.Character.Culture.BanditBoss == y.Character || y.Character.IsHero));
				while (num3 > 0 && mblist.Count > 0)
				{
					MobileParty randomElement2 = mblist.GetRandomElement<MobileParty>();
					List<TroopRosterElement> troopRoster2 = randomElement2.MemberRoster.GetTroopRoster();
					List<ValueTuple<TroopRosterElement, float>> list2 = new List<ValueTuple<TroopRosterElement, float>>();
					foreach (TroopRosterElement troopRosterElement2 in troopRoster2)
					{
						list2.Add(new ValueTuple<TroopRosterElement, float>(troopRosterElement2, (float)(troopRosterElement2.Number * ((troopRosterElement2.Character.Culture.BanditBoss == troopRosterElement2.Character || troopRosterElement2.Character.IsHero) ? 0 : 1))));
					}
					TroopRosterElement troopRosterElement3 = MBRandom.ChooseWeighted<TroopRosterElement>(list2);
					randomElement2.MemberRoster.AddToCounts(troopRosterElement3.Character, 1, false, 0, 0, true, -1);
					num3--;
				}
			}
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x000FAF80 File Offset: 0x000F9180
		private void OnTroopRosterManageDone(TroopRoster hideoutTroops)
		{
			this.ArrangeHideoutTroopCountsForMission();
			GameMenu.SwitchToMenu("hideout_place");
			Settlement.CurrentSettlement.Hideout.UpdateNextPossibleAttackTime();
			if (PlayerEncounter.IsActive)
			{
				PlayerEncounter.LeaveEncounter = false;
			}
			else
			{
				PlayerEncounter.Start();
				PlayerEncounter.Current.SetupFields(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			}
			if (PlayerEncounter.Battle == null)
			{
				PlayerEncounter.StartBattle();
				PlayerEncounter.Update();
			}
			CampaignMission.OpenHideoutBattleMission(Settlement.CurrentSettlement.Hideout.SceneName, (hideoutTroops != null) ? hideoutTroops.ToFlattenedRoster() : null);
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x000FB00C File Offset: 0x000F920C
		private bool CanChangeStatusOfTroop(CharacterObject character)
		{
			return !character.IsPlayerCharacter && !character.IsNotTransferableInHideouts;
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x000FB024 File Offset: 0x000F9224
		private bool game_menu_talk_to_leader_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			PartyBase party = Settlement.CurrentSettlement.Parties[0].Party;
			return party != null && party.LeaderHero != null && party.LeaderHero != Hero.MainHero;
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x000FB06C File Offset: 0x000F926C
		private void game_menu_talk_to_leader_on_consequence(MenuCallbackArgs args)
		{
			PartyBase party = Settlement.CurrentSettlement.Parties[0].Party;
			ConversationCharacterData playerCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			ConversationCharacterData conversationPartnerData = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(party), party, false, false, false, false, false, false);
			CampaignMission.OpenConversationMission(playerCharacterData, conversationPartnerData, "", "");
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x000FB0CC File Offset: 0x000F92CC
		private bool game_menu_wait_until_nightfall_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return Settlement.CurrentSettlement.Parties.Any((MobileParty t) => t != MobileParty.MainParty) && !this.IsHideoutAttackableNow();
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000FB11C File Offset: 0x000F931C
		private void game_menu_wait_until_nightfall_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_wait");
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000FB128 File Offset: 0x000F9328
		private void game_menu_hideout_leave_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (MobileParty.MainParty.CurrentSettlement != null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			PlayerEncounter.Finish(true);
		}

		// Token: 0x04001171 RID: 4465
		private const int MaxDistanceSquaredBetweenHideoutAndBoundVillage = 1600;

		// Token: 0x04001172 RID: 4466
		private const int HideoutClearRelationEffect = 2;

		// Token: 0x04001173 RID: 4467
		private readonly int CanAttackHideoutStart = 23;

		// Token: 0x04001174 RID: 4468
		private readonly int CanAttackHideoutEnd = 2;

		// Token: 0x04001175 RID: 4469
		private float _hideoutWaitProgressHours;

		// Token: 0x04001176 RID: 4470
		private float _hideoutWaitTargetHours;
	}
}
