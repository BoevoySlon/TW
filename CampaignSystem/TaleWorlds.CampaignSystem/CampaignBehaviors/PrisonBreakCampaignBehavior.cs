using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003C8 RID: 968
	public class PrisonBreakCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B67 RID: 15207 RVA: 0x0011A8EF File Offset: 0x00118AEF
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.CanHeroDieEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.CanHeroDie));
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0011A91F File Offset: 0x00118B1F
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Hero>("_prisonerHero", ref this._prisonerHero);
			dataStore.SyncData<Dictionary<Settlement, CampaignTime>>("_coolDownData", ref this._coolDownData);
			dataStore.SyncData<string>("_previousMenuId", ref this._previousMenuId);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x0011A958 File Offset: 0x00118B58
		private void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail detail, ref bool result)
		{
			if (detail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle && hero == Hero.MainHero && this._prisonerHero != null && CampaignMission.Current != null)
			{
				Location location = CampaignMission.Current.Location;
				Settlement currentSettlement = Settlement.CurrentSettlement;
				object obj;
				if (currentSettlement == null)
				{
					obj = null;
				}
				else
				{
					LocationComplex locationComplex = currentSettlement.LocationComplex;
					obj = ((locationComplex != null) ? locationComplex.GetLocationWithId("prison") : null);
				}
				if (location == obj)
				{
					result = false;
				}
			}
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0011A9B1 File Offset: 0x00118BB1
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0011A9C4 File Offset: 0x00118BC4
		private void AddGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenuOption("town_keep_dungeon", "town_prison_break", "{=lc0YIqby}Stage a prison break", new GameMenuOption.OnConditionDelegate(this.game_menu_stage_prison_break_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_prison_break_from_dungeon_on_consequence), false, 3, false, null);
			campaignGameStarter.AddGameMenuOption("castle_dungeon", "town_prison_break", "{=lc0YIqby}Stage a prison break", new GameMenuOption.OnConditionDelegate(this.game_menu_stage_prison_break_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_prison_break_from_castle_dungeon_on_consequence), false, 3, false, null);
			campaignGameStarter.AddGameMenuOption("town_enemy_town_keep", "town_prison_break", "{=lc0YIqby}Stage a prison break", new GameMenuOption.OnConditionDelegate(this.game_menu_stage_prison_break_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_castle_prison_break_from_enemy_keep_on_consequence), false, 0, false, null);
			campaignGameStarter.AddGameMenu("start_prison_break", "{=aZaujaHb}The guard accepts your offer. He is ready to help you break {PRISONER.NAME} out, if you're willing to pay.", new OnInitDelegate(this.start_prison_break_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("start_prison_break", "start", "{=N6UeziT8}Start ({COST}{GOLD_ICON})", new GameMenuOption.OnConditionDelegate(this.game_menu_castle_prison_break_on_condition), delegate(MenuCallbackArgs args)
			{
				this.OpenPrisonBreakMission();
			}, false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("start_prison_break", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_cancel_prison_break), true, -1, false, null);
			campaignGameStarter.AddGameMenu("prison_break_cool_down", "{=cGSXFJ3N}Because of a recent breakout attempt in this settlement it is on high alert. The guard won't even be seen talking to you.", null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("prison_break_cool_down", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_menu_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_cancel_prison_break), true, -1, false, null);
			campaignGameStarter.AddGameMenu("settlement_prison_break_success", "{=TazumJGN}You emerge into the streets. No one is yet aware of what happened in the dungeons, and you hustle {PRISONER.NAME} towards the gates.{newline}You may now leave the {?SETTLEMENT_TYPE}settlement{?}castle{\\?}.", new OnInitDelegate(this.settlement_prison_break_success_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("settlement_prison_break_success", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.game_menu_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.settlement_prison_break_success_continue_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenu("settlement_prison_break_fail_player_unconscious", "{=svuD2vBo}You were knocked unconscious while trying to break {PRISONER.NAME} out of the dungeon.{newline}The guards caught you both and threw you in a cell.", new OnInitDelegate(this.settlement_prison_break_fail_on_init), GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("settlement_prison_break_fail_player_unconscious", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.game_menu_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.settlement_prison_break_fail_player_unconscious_continue_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenu("settlement_prison_break_fail_prisoner_unconscious", "{=eKy1II3h}You made your way out but {PRISONER.NAME} was badly wounded during the escape. You had no choice but to leave {?PRISONER.GENDER}her{?}him{\\?} behind as you disappeared into the back streets and sneaked out the gate.{INFORMATION_IF_PRISONER_DEAD}", new OnInitDelegate(this.settlement_prison_break_fail_prisoner_injured_on_init), GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("settlement_prison_break_fail_prisoner_unconscious", "continue", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.game_menu_continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.settlement_prison_break_fail_prisoner_unconscious_continue_on_consequence), false, -1, false, null);
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0011AC1C File Offset: 0x00118E1C
		private void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("prison_break_start_1", "start", "prison_break_end_already_met", "{=5RDF3aZN}{SALUTATION}... You came for me!", new ConversationSentence.OnConditionDelegate(this.prison_break_end_with_success_clan_member), null, 120, null);
			campaignGameStarter.AddDialogLine("prison_break_start_2", "start", "prison_break_end_already_met", "{=PRadDFN5}{SALUTATION}... Well, I hadn't expected this, but I'm very grateful.", new ConversationSentence.OnConditionDelegate(this.prison_break_end_with_success_player_already_met), null, 120, null);
			campaignGameStarter.AddDialogLine("prison_break_start_3", "start", "prison_break_end_meet", "{=zbPRul7h}Well.. I don't know you, but I'm very grateful.", new ConversationSentence.OnConditionDelegate(this.prison_break_end_with_success_other_on_condition), null, 120, null);
			campaignGameStarter.AddPlayerLine("prison_break_player_ask", "prison_break_end_already_met", "prison_break_next_move", "{=qFoMsPIf}I'm glad we made it out safe. What will you do now?", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("prison_break_player_meet", "prison_break_end_meet", "prison_break_next_move", "{=nMn63bV1}I am {PLAYER.NAME}. All I ask is that you remember that name, and what I did.{newline}Tell me, what will you do now?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("prison_break_next_companion", "prison_break_next_move", "prison_break_next_move_player_companion", "{=aoJHP3Ud}I'm ready to rejoin you. I'm in your debt.", () => this._prisonerHero.CompanionOf == Clan.PlayerClan, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_commander", "prison_break_next_move", "prison_break_next_move_player", "{=xADZi2bK}I'll go and find my men. I will remember your help...", () => this._prisonerHero.IsCommander, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_noble", "prison_break_next_move", "prison_break_next_move_player", "{=W2vV5jzj}I'll go back to my family. I will remember your help...", () => this._prisonerHero.IsLord, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_notable", "prison_break_next_move", "prison_break_next_move_player", "{=efdCZPw4}I'll go back to my work. I will remember your help...", () => this._prisonerHero.IsNotable, null, 100, null);
			campaignGameStarter.AddDialogLine("prison_break_next_other", "prison_break_next_move", "prison_break_next_move_player_other", "{=TWZ4abt5}I'll keep wandering about, as I've done before. I can make a living. No need to worry.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("prison_break_end_dialog_3", "prison_break_next_move_player_companion", "close_window", "{=ncvB4XRL}You could join me.", null, new ConversationSentence.OnConsequenceDelegate(this.prison_break_end_with_success_companion), 100, null, null);
			campaignGameStarter.AddPlayerLine("prison_break_end_dialog_1", "prison_break_next_move_player", "close_window", "{=rlAec9CM}Very well. Keep safe.", null, new ConversationSentence.OnConsequenceDelegate(this.prison_break_end_with_success_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("prison_break_end_dialog_2", "prison_break_next_move_player_other", "close_window", "{=dzXaXKaC}Very well.", null, new ConversationSentence.OnConsequenceDelegate(this.prison_break_end_with_success_on_consequence), 100, null, null);
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x0011AE3C File Offset: 0x0011903C
		[GameMenuInitializationHandler("start_prison_break")]
		[GameMenuInitializationHandler("prison_break_cool_down")]
		[GameMenuInitializationHandler("settlement_prison_break_success")]
		[GameMenuInitializationHandler("settlement_prison_break_fail_player_unconscious")]
		[GameMenuInitializationHandler("settlement_prison_break_fail_prisoner_unconscious")]
		public static void game_menu_prison_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x0011AE58 File Offset: 0x00119058
		private bool prison_break_end_with_success_clan_member()
		{
			bool flag = this._prisonerHero != null && this._prisonerHero.CharacterObject == CharacterObject.OneToOneConversationCharacter && (this._prisonerHero.CompanionOf == Clan.PlayerClan || this._prisonerHero.Clan == Clan.PlayerClan);
			if (flag)
			{
				MBTextManager.SetTextVariable("SALUTATION", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_salutation", CharacterObject.OneToOneConversationCharacter), false);
			}
			return flag;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0011AED0 File Offset: 0x001190D0
		private bool prison_break_end_with_success_player_already_met()
		{
			bool flag = this._prisonerHero != null && this._prisonerHero.CharacterObject == CharacterObject.OneToOneConversationCharacter && this._prisonerHero.HasMet;
			if (flag)
			{
				MBTextManager.SetTextVariable("SALUTATION", Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_salutation", CharacterObject.OneToOneConversationCharacter), false);
			}
			return flag;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x0011AF2C File Offset: 0x0011912C
		private bool prison_break_end_with_success_other_on_condition()
		{
			return this._prisonerHero != null && this._prisonerHero.CharacterObject == CharacterObject.OneToOneConversationCharacter;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x0011AF4A File Offset: 0x0011914A
		private void PrisonBreakEndedInternal()
		{
			ChangeRelationAction.ApplyPlayerRelation(this._prisonerHero, Campaign.Current.Models.PrisonBreakModel.GetRelationRewardOnPrisonBreak(this._prisonerHero), true, true);
			SkillLevelingManager.OnPrisonBreakEnd(this._prisonerHero, true);
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x0011AF7F File Offset: 0x0011917F
		private void prison_break_end_with_success_on_consequence()
		{
			this.PrisonBreakEndedInternal();
			EndCaptivityAction.ApplyByEscape(this._prisonerHero, Hero.MainHero);
			this._prisonerHero = null;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x0011AF9E File Offset: 0x0011919E
		private void prison_break_end_with_success_companion()
		{
			this.PrisonBreakEndedInternal();
			EndCaptivityAction.ApplyByEscape(this._prisonerHero, Hero.MainHero);
			this._prisonerHero.ChangeState(Hero.CharacterStates.Active);
			AddHeroToPartyAction.Apply(this._prisonerHero, MobileParty.MainParty, true);
			this._prisonerHero = null;
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0011AFDA File Offset: 0x001191DA
		private bool game_menu_castle_prison_break_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			this._bribeCost = Campaign.Current.Models.PrisonBreakModel.GetPrisonBreakStartCost(this._prisonerHero);
			MBTextManager.SetTextVariable("COST", this._bribeCost);
			return true;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0011B014 File Offset: 0x00119214
		private void AddCoolDownForPrisonBreak(Settlement settlement)
		{
			CampaignTime value = CampaignTime.DaysFromNow(7f);
			if (this._coolDownData.ContainsKey(settlement))
			{
				this._coolDownData[settlement] = value;
				return;
			}
			this._coolDownData.Add(settlement, value);
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0011B058 File Offset: 0x00119258
		private bool CanPlayerStartPrisonBreak(Settlement settlement)
		{
			bool flag = true;
			CampaignTime campaignTime;
			if (this._coolDownData.TryGetValue(settlement, out campaignTime))
			{
				flag = campaignTime.IsPast;
				if (flag)
				{
					this._coolDownData.Remove(settlement);
				}
			}
			return flag;
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0011B090 File Offset: 0x00119290
		private bool game_menu_stage_prison_break_on_condition(MenuCallbackArgs args)
		{
			bool result = false;
			if (Campaign.Current.Models.PrisonBreakModel.CanPlayerStagePrisonBreak(Settlement.CurrentSettlement))
			{
				args.optionLeaveType = GameMenuOption.LeaveType.StagePrisonBreak;
				if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=yNMrF2QF}You are wounded", null);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0011B0E9 File Offset: 0x001192E9
		private void game_menu_castle_prison_break_from_dungeon_on_consequence(MenuCallbackArgs args)
		{
			this._previousMenuId = "town_keep_dungeon";
			this.game_menu_castle_prison_break_on_consequence(args);
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x0011B0FD File Offset: 0x001192FD
		private void game_menu_castle_prison_break_from_castle_dungeon_on_consequence(MenuCallbackArgs args)
		{
			this._previousMenuId = "castle_dungeon";
			this.game_menu_castle_prison_break_on_consequence(args);
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0011B111 File Offset: 0x00119311
		private void game_menu_castle_prison_break_from_enemy_keep_on_consequence(MenuCallbackArgs args)
		{
			this._previousMenuId = "town_enemy_town_keep";
			this.game_menu_castle_prison_break_on_consequence(args);
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0011B128 File Offset: 0x00119328
		private void game_menu_castle_prison_break_on_consequence(MenuCallbackArgs args)
		{
			if (this.CanPlayerStartPrisonBreak(Settlement.CurrentSettlement))
			{
				FlattenedTroopRoster flattenedTroopRoster = Settlement.CurrentSettlement.Party.PrisonRoster.ToFlattenedRoster();
				if (Settlement.CurrentSettlement.Town.GarrisonParty != null)
				{
					flattenedTroopRoster.Add(Settlement.CurrentSettlement.Town.GarrisonParty.PrisonRoster.GetTroopRoster());
				}
				flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => !x.Troop.IsHero);
				List<InquiryElement> list = new List<InquiryElement>();
				foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in flattenedTroopRoster)
				{
					TextObject textObject;
					TextObject textObject2;
					bool flag;
					if (FactionManager.IsAtWarAgainstFaction(Clan.PlayerClan.MapFaction, flattenedTroopRosterElement.Troop.HeroObject.MapFaction))
					{
						textObject = new TextObject("{=!}{HERO.NAME}", null);
						StringHelpers.SetCharacterProperties("HERO", flattenedTroopRosterElement.Troop, textObject, false);
						textObject2 = new TextObject("{=VM1SGrla}{HERO.NAME} is your enemy.", null);
						textObject2.SetCharacterProperties("HERO", flattenedTroopRosterElement.Troop, false);
						flag = true;
					}
					else
					{
						int prisonBreakStartCost = Campaign.Current.Models.PrisonBreakModel.GetPrisonBreakStartCost(flattenedTroopRosterElement.Troop.HeroObject);
						flag = (Hero.MainHero.Gold < prisonBreakStartCost);
						textObject = new TextObject("{=!}{HERO.NAME}", null);
						StringHelpers.SetCharacterProperties("HERO", flattenedTroopRosterElement.Troop, textObject, false);
						textObject2 = new TextObject("{=I4SjNT6Y}This will cost you {BRIBE_COST}{GOLD_ICON}.{?ENOUGH_GOLD}{?} You don't have enough money.{\\?}", null);
						textObject2.SetTextVariable("BRIBE_COST", prisonBreakStartCost);
						textObject2.SetTextVariable("ENOUGH_GOLD", flag ? 0 : 1);
					}
					list.Add(new InquiryElement(flattenedTroopRosterElement.Troop, textObject.ToString(), new ImageIdentifier(CharacterCode.CreateFrom(flattenedTroopRosterElement.Troop)), !flag, textObject2.ToString()));
				}
				MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=oQjsShmH}PRISONERS", null).ToString(), new TextObject("{=abpzOR0D}Choose a prisoner to break out", null).ToString(), list, true, 1, 1, GameTexts.FindText("str_done", null).ToString(), string.Empty, new Action<List<InquiryElement>>(this.StartPrisonBreak), null, "", false), false, false);
				return;
			}
			GameMenu.SwitchToMenu("prison_break_cool_down");
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x0011B390 File Offset: 0x00119590
		private void StartPrisonBreak(List<InquiryElement> prisonerList)
		{
			if (prisonerList.Count > 0)
			{
				this._prisonerHero = ((CharacterObject)prisonerList[0].Identifier).HeroObject;
				GameMenu.SwitchToMenu("start_prison_break");
				return;
			}
			this._prisonerHero = null;
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0011B3CC File Offset: 0x001195CC
		private void OpenPrisonBreakMission()
		{
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, this._bribeCost, false);
			this.AddCoolDownForPrisonBreak(Settlement.CurrentSettlement);
			Location locationWithId = LocationComplex.Current.GetLocationWithId("prison");
			CampaignMission.OpenPrisonBreakMission(locationWithId.GetSceneName(Settlement.CurrentSettlement.Town.GetWallLevel()), locationWithId, this._prisonerHero.CharacterObject, null);
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0011B42E File Offset: 0x0011962E
		private bool game_menu_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x0011B439 File Offset: 0x00119639
		private bool game_menu_continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0011B444 File Offset: 0x00119644
		private void game_menu_cancel_prison_break(MenuCallbackArgs args)
		{
			this._prisonerHero = null;
			GameMenu.SwitchToMenu(this._previousMenuId);
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0011B458 File Offset: 0x00119658
		private void start_prison_break_on_init(MenuCallbackArgs args)
		{
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0011B472 File Offset: 0x00119672
		private void settlement_prison_break_success_on_init(MenuCallbackArgs args)
		{
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
			MBTextManager.SetTextVariable("SETTLEMENT_TYPE", Settlement.CurrentSettlement.IsTown ? 1 : 0);
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0011B4A8 File Offset: 0x001196A8
		private void settlement_prison_break_success_continue_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, null, false, false, false, false, false, false), new ConversationCharacterData(this._prisonerHero.CharacterObject, null, false, false, false, false, false, false));
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0011B4F0 File Offset: 0x001196F0
		private void settlement_prison_break_fail_prisoner_injured_on_init(MenuCallbackArgs args)
		{
			if (this._prisonerHero.IsDead)
			{
				TextObject textObject = new TextObject("{=GkwOyJn9}{newline}You later learn that {?PRISONER.GENDER}she{?}he{\\?} died from {?PRISONER.GENDER}her{?}his{\\?} injuries.", null);
				StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, textObject, false);
				MBTextManager.SetTextVariable("INFORMATION_IF_PRISONER_DEAD", textObject, false);
			}
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0011B552 File Offset: 0x00119752
		private void settlement_prison_break_fail_on_init(MenuCallbackArgs args)
		{
			StringHelpers.SetCharacterProperties("PRISONER", this._prisonerHero.CharacterObject, null, false);
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0011B56C File Offset: 0x0011976C
		private void settlement_prison_break_fail_player_unconscious_continue_on_consequence(MenuCallbackArgs args)
		{
			SkillLevelingManager.OnPrisonBreakEnd(this._prisonerHero, false);
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			TakePrisonerAction.Apply(currentSettlement.Party, Hero.MainHero);
			this._prisonerHero = null;
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0011B5A0 File Offset: 0x001197A0
		private void settlement_prison_break_fail_prisoner_unconscious_continue_on_consequence(MenuCallbackArgs args)
		{
			SkillLevelingManager.OnPrisonBreakEnd(this._prisonerHero, false);
			this._prisonerHero = null;
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
		}

		// Token: 0x040011D0 RID: 4560
		private const int CoolDownInDays = 7;

		// Token: 0x040011D1 RID: 4561
		private const int PrisonBreakDialogPriority = 120;

		// Token: 0x040011D2 RID: 4562
		private Dictionary<Settlement, CampaignTime> _coolDownData = new Dictionary<Settlement, CampaignTime>();

		// Token: 0x040011D3 RID: 4563
		private Hero _prisonerHero;

		// Token: 0x040011D4 RID: 4564
		private int _bribeCost;

		// Token: 0x040011D5 RID: 4565
		private string _previousMenuId;
	}
}
