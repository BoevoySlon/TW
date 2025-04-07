using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D5 RID: 981
	public class SiegeAftermathCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003C72 RID: 15474 RVA: 0x00124774 File Offset: 0x00122974
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnSiegeAftermathAppliedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, SiegeAftermathAction.SiegeAftermath, Clan, Dictionary<MobileParty, float>>(this.OnSiegeAftermathApplied));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnBuildingLevelChangedEvent.AddNonSerializedListener(this, new Action<Town, Building, int>(this.OnBuildingLevelChanged));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x001247F4 File Offset: 0x001229F4
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x00124800 File Offset: 0x00122A00
		private void AddGameMenus(CampaignGameStarter gameSystemInitializer)
		{
			gameSystemInitializer.AddGameMenu("menu_settlement_taken", "", new OnInitDelegate(this.menu_settlement_taken_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenu("menu_settlement_taken_player_leader", "{=!}{SETTLEMENT_TAKEN_TEXT}", new OnInitDelegate(this.menu_settlement_taken_player_leader_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_leader", "menu_settlement_taken_devastate", "{=v0mZi3Zd}Devastate {DEVASTATE_INFLUENCE_COST_TEXT}", new GameMenuOption.OnConditionDelegate(this.menu_settlement_taken_devastate_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_devastate_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_leader", "menu_settlement_taken_pillage", "{=tZCLAkGZ}Pillage", new GameMenuOption.OnConditionDelegate(this.menu_settlement_taken_pillage_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_pillage_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_leader", "menu_settlement_taken_show_mercy", "{=EuwtMZGZ}Show Mercy {SHOW_MERCY_INFLUENCE_COST_TEXT}", new GameMenuOption.OnConditionDelegate(this.menu_settlement_taken_show_mercy_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_show_mercy_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_settlement_taken_player_army_member", "{=!}{LEADER_DECISION_TEXT}", new OnInitDelegate(this.menu_settlement_taken_player_army_member_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_army_member", "menu_settlement_taken_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("menu_settlement_taken_player_participant", "{=!}{PLAYER_PARTICIPANT_TEXT}", new OnInitDelegate(this.menu_settlement_taken_player_participant_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("menu_settlement_taken_player_participant", "menu_settlement_taken_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_continue_on_consequence), false, -1, false, null);
			gameSystemInitializer.AddGameMenu("siege_aftermath_contextual_summary", "{=!}{START_OF_EXPLANATION}{newline} {newline}{CONTEXTUAL_SUMMARY_TEXT}{newline} {newline}{END_OF_EXPLANATION}", new OnInitDelegate(this.siege_aftermath_contextual_summary_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			gameSystemInitializer.AddGameMenuOption("siege_aftermath_contextual_summary", "menu_settlement_taken_continue", "{=veWOovVv}Continue...", new GameMenuOption.OnConditionDelegate(this.continue_on_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_settlement_taken_continue_on_consequence), false, -1, false, null);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x001249D0 File Offset: 0x00122BD0
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if ((mapEvent.IsSiegeAssault || mapEvent.IsSiegeOutside) && mapEvent.WinningSide == BattleSideEnum.Attacker && mapEvent.MapEventSettlement != null)
			{
				this._siegeEventPartyContributions.Clear();
				foreach (MapEventParty mapEventParty in mapEvent.PartiesOnSide(BattleSideEnum.Attacker))
				{
					float num;
					float num2;
					float num3;
					float num4;
					float value;
					mapEvent.GetBattleRewards(mapEventParty.Party, out num, out num2, out num3, out num4, out value);
					if (mapEventParty.Party.IsMobile && !this._siegeEventPartyContributions.ContainsKey(mapEventParty.Party.MobileParty))
					{
						this._siegeEventPartyContributions.Add(mapEventParty.Party.MobileParty, value);
					}
				}
				if (mapEvent.AttackerSide.IsMainPartyAmongParties())
				{
					this._playerEncounterAftermathDamagedBuildings.Clear();
					this._wasPlayerArmyMember = false;
					this._besiegerParty = mapEvent.AttackerSide.LeaderParty.MobileParty;
					this._prevSettlementOwnerClan = mapEvent.MapEventSettlement.OwnerClan;
					if (this._besiegerParty != MobileParty.MainParty)
					{
						if (this._besiegerParty.Army != null && this._besiegerParty.Army.Parties.Contains(MobileParty.MainParty))
						{
							this._wasPlayerArmyMember = true;
						}
						this._playerEncounterAftermath = this.DetermineAISiegeAftermath(this._besiegerParty, mapEvent.MapEventSettlement);
						SiegeAftermathAction.ApplyAftermath(this._besiegerParty, mapEvent.MapEventSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
						return;
					}
				}
				else
				{
					if (mapEvent.MapEventSettlement.SiegeEvent != null)
					{
						MobileParty leaderParty = mapEvent.MapEventSettlement.SiegeEvent.BesiegerCamp.LeaderParty;
						SiegeAftermathAction.SiegeAftermath aftermathType = this.DetermineAISiegeAftermath(leaderParty, mapEvent.MapEventSettlement);
						SiegeAftermathAction.ApplyAftermath(leaderParty, mapEvent.MapEventSettlement, aftermathType, mapEvent.MapEventSettlement.OwnerClan, this._siegeEventPartyContributions);
						return;
					}
					Debug.FailedAssert("Siege event is null in siege aftermath", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "OnMapEventEnded", 116);
				}
			}
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x00124BD8 File Offset: 0x00122DD8
		private void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			float siegeAftermathInfluenceCost = this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, aftermathType);
			if (siegeAftermathInfluenceCost > 0f)
			{
				ChangeClanInfluenceAction.Apply(attackerParty.ActualClan, -siegeAftermathInfluenceCost);
			}
			this._settlementProsperityCache = settlement.Town.Prosperity;
			settlement.Town.Prosperity += this.GetSiegeAftermathProsperityPenalty(attackerParty, settlement, aftermathType);
			if (aftermathType != SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				int siegeAftermathProjectsLoss = this.GetSiegeAftermathProjectsLoss(attackerParty, aftermathType);
				for (int i = 0; i < siegeAftermathProjectsLoss; i++)
				{
					settlement.Town.Buildings.GetRandomElementWithPredicate((Building x) => !x.BuildingType.IsDefaultProject).LevelDown();
				}
				settlement.Town.Loyalty += this.GetSiegeAftermathLoyaltyPenalty(aftermathType);
				if (settlement.IsTown)
				{
					foreach (Hero hero in settlement.Notables)
					{
						hero.AddPower(hero.Power * this.GetSiegeAftermathNotablePowerModifierForAftermath(aftermathType));
					}
				}
				if (previousSettlementOwner.Leader == null)
				{
					Debug.Print(string.Format("{0}: {1} leader was null", previousSettlementOwner.StringId, previousSettlementOwner), 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert(string.Format("{0}: {1} leader was null", previousSettlementOwner.StringId, previousSettlementOwner), "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "OnSiegeAftermathApplied", 158);
				}
				if (attackerParty.LeaderHero != null)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(attackerParty.LeaderHero, previousSettlementOwner.Leader, this.GetSiegeAftermathRelationPenaltyWithSettlementOwner(aftermathType), true);
				}
			}
			float totalArmyGoldGain = (float)this.GetSiegeAftermathArmyGoldGain(attackerParty, settlement, aftermathType);
			foreach (KeyValuePair<MobileParty, float> keyValuePair in partyContributions)
			{
				MobileParty key = keyValuePair.Key;
				int siegeAftermathPartyGoldGain = this.GetSiegeAftermathPartyGoldGain(totalArmyGoldGain, keyValuePair.Value);
				if (key.LeaderHero != null)
				{
					GiveGoldAction.ApplyForPartyToCharacter(null, key.LeaderHero, siegeAftermathPartyGoldGain, false);
				}
				else
				{
					GiveGoldAction.ApplyForCharacterToParty(null, key.Party, siegeAftermathPartyGoldGain, false);
				}
				key.RecentEventsMorale += this.GetSiegeAftermathPartyMoraleBonus(attackerParty, settlement, aftermathType);
				if (attackerParty == MobileParty.MainParty && key != attackerParty && key.LeaderHero != null && aftermathType != SiegeAftermathAction.SiegeAftermath.Pillage && attackerParty.MapFaction.Culture != settlement.Culture)
				{
					int siegeAftermathRelationChangeWithLord = this.GetSiegeAftermathRelationChangeWithLord(key.LeaderHero, aftermathType);
					if (siegeAftermathRelationChangeWithLord != 0)
					{
						ChangeRelationAction.ApplyPlayerRelation(key.LeaderHero, siegeAftermathRelationChangeWithLord, true, true);
					}
				}
			}
			if (attackerParty == MobileParty.MainParty && aftermathType != SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				TraitLevelingHelper.OnSiegeAftermathApplied(settlement, aftermathType, new TraitObject[]
				{
					DefaultTraits.Mercy
				});
			}
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00124E80 File Offset: 0x00123080
		private SiegeAftermathAction.SiegeAftermath DetermineSiegeAftermathOnEncounterLeaderDeath(MobileParty attackerParty, Settlement settlement)
		{
			if (attackerParty.MapFaction.Culture != settlement.Culture)
			{
				return SiegeAftermathAction.SiegeAftermath.Devastate;
			}
			return SiegeAftermathAction.SiegeAftermath.ShowMercy;
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x00124E98 File Offset: 0x00123098
		private bool IsMobilePartyLeaderAliveForSiegeAftermath(MobileParty attackerParty)
		{
			return attackerParty.LeaderHero != null && attackerParty.LeaderHero.IsAlive && attackerParty.LeaderHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.DiedInBattle && attackerParty.LeaderHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.WoundedInBattle;
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00124ED0 File Offset: 0x001230D0
		private SiegeAftermathAction.SiegeAftermath DetermineAISiegeAftermath(MobileParty attackerParty, Settlement settlement)
		{
			if (!this.IsMobilePartyLeaderAliveForSiegeAftermath(attackerParty))
			{
				return this.DetermineSiegeAftermathOnEncounterLeaderDeath(attackerParty, settlement);
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			float num = (attackerParty.Army != null) ? attackerParty.Army.Morale : attackerParty.Morale;
			if (attackerParty.MapFaction.Culture == settlement.Culture || (attackerParty.ActualClan.Influence > 2f * this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, SiegeAftermathAction.SiegeAftermath.ShowMercy) && num > 60f))
			{
				flag = true;
			}
			if (attackerParty.MapFaction.Culture != settlement.Culture)
			{
				flag2 = true;
			}
			if (attackerParty.MapFaction.Culture != settlement.Culture && attackerParty.ActualClan.Influence > 2f * this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, SiegeAftermathAction.SiegeAftermath.Devastate) && num < 90f)
			{
				flag3 = true;
			}
			int traitLevel = attackerParty.LeaderHero.GetTraitLevel(DefaultTraits.Mercy);
			float num2;
			float num3;
			float num4;
			if (traitLevel > 0)
			{
				num2 = 0.4f + 0.2f * (float)traitLevel;
				num3 = 1f - num2;
				num4 = 0f;
			}
			else if (traitLevel < 0)
			{
				num4 = 0.4f + 0.2f * (float)MathF.Abs(traitLevel);
				num3 = 1f - num4;
				num2 = 0f;
			}
			else
			{
				num2 = 0.2f;
				num3 = 0.6f;
				num4 = 0.2f;
			}
			if (!flag)
			{
				num3 += num2;
				num2 = 0f;
			}
			if (!flag3)
			{
				num3 += num4;
				num4 = 0f;
			}
			if (!flag2)
			{
				num2 += num3;
				num3 = 0f;
			}
			return MBRandom.ChooseWeighted<SiegeAftermathAction.SiegeAftermath>(new List<ValueTuple<SiegeAftermathAction.SiegeAftermath, float>>
			{
				new ValueTuple<SiegeAftermathAction.SiegeAftermath, float>(SiegeAftermathAction.SiegeAftermath.ShowMercy, num2),
				new ValueTuple<SiegeAftermathAction.SiegeAftermath, float>(SiegeAftermathAction.SiegeAftermath.Pillage, num3),
				new ValueTuple<SiegeAftermathAction.SiegeAftermath, float>(SiegeAftermathAction.SiegeAftermath.Devastate, num4)
			});
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x00125094 File Offset: 0x00123294
		private void OnBuildingLevelChanged(Town town, Building building, int level)
		{
			if (town.Settlement == PlayerEncounter.EncounterSettlement && level < 0)
			{
				if (!this._playerEncounterAftermathDamagedBuildings.ContainsKey(building))
				{
					this._playerEncounterAftermathDamagedBuildings.Add(building, 0);
				}
				Dictionary<Building, int> playerEncounterAftermathDamagedBuildings = this._playerEncounterAftermathDamagedBuildings;
				playerEncounterAftermathDamagedBuildings[building] += -1;
			}
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x001250E8 File Offset: 0x001232E8
		private void HandlePlayerDeathDuringSiegeAftermath()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			SiegeAftermathAction.SiegeAftermath aftermathType = this.DetermineSiegeAftermathOnEncounterLeaderDeath(MobileParty.MainParty, currentSettlement);
			SiegeAftermathAction.ApplyAftermath(MobileParty.MainParty, currentSettlement, aftermathType, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0012512C File Offset: 0x0012332C
		private void menu_settlement_taken_on_init(MenuCallbackArgs args)
		{
			MobileParty besiegerParty = this._besiegerParty;
			if (besiegerParty == MobileParty.MainParty)
			{
				if (this.IsMobilePartyLeaderAliveForSiegeAftermath(besiegerParty))
				{
					GameMenu.SwitchToMenu("menu_settlement_taken_player_leader");
					return;
				}
				this.HandlePlayerDeathDuringSiegeAftermath();
				return;
			}
			else
			{
				if (this._wasPlayerArmyMember)
				{
					GameMenu.SwitchToMenu("menu_settlement_taken_player_army_member");
					return;
				}
				GameMenu.SwitchToMenu("menu_settlement_taken_player_participant");
				return;
			}
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x00125180 File Offset: 0x00123380
		private void menu_settlement_taken_player_leader_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.GameMenu.GetText().SetTextVariable("SETTLEMENT_TAKEN_TEXT", new TextObject("{=QvyFYn1b}The defenders are routed, and it's clear that {TOWN_NAME} is yours. It's time for you to determine the fate of the {?IS_CITY}city{?}fortress{\\?}.", null));
			MBTextManager.SetTextVariable("TOWN_NAME", Settlement.CurrentSettlement.Name, false);
			MBTextManager.SetTextVariable("IS_CITY", Settlement.CurrentSettlement.IsTown ? 1 : 0);
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x001251F4 File Offset: 0x001233F4
		private void menu_settlement_taken_player_army_member_on_init(MenuCallbackArgs args)
		{
			bool flag = this._besiegerParty.Army != null && this._besiegerParty.Army.Parties.Contains(MobileParty.MainParty);
			TextObject textObject = TextObject.Empty;
			if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				if (flag)
				{
					textObject = new TextObject("{=peHCARhM}{DEFAULT_TEXT}{ARMY_LEADER.LINK} has ordered that {SETTLEMENT} to be laid waste. {?ARMY_LEADER.GENDER}Her{?}His{\\?} troops sweep through the {?IS_CITY}city{?}fortress{\\?} taking whatever loot they like and setting fire to the rest.", null);
				}
				else if (this._wasPlayerArmyMember)
				{
					textObject = new TextObject("{=qeRRWMfU}{DEFAULT_TEXT}{ARMY_LEADER.LINK} fell during the fighting. {?ARMY_LEADER.GENDER}Her{?}His{\\?} vengeful troops sweep through the {?IS_CITY}city{?}fortress{\\?} taking whatever loot they like and setting fire to the rest.", null);
				}
				textObject.SetTextVariable("IS_CITY", Settlement.CurrentSettlement.IsTown ? 1 : 0);
			}
			else if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				if (flag)
				{
					textObject = new TextObject("{=BXw5MwX7}{DEFAULT_TEXT}{ARMY_LEADER.LINK} grants {?ARMY_LEADER.GENDER}her{?}his{\\?} men their customary right of pillage after a successful siege. {?ARMY_LEADER.GENDER}She{?}He{\\?} tells them they may take property but must spare the townsfolk's lives.", null);
				}
				else if (this._wasPlayerArmyMember)
				{
					Debug.FailedAssert("_wasPlayerArmyMember", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "menu_settlement_taken_player_army_member_on_init", 401);
					textObject = new TextObject("{=99v8GTTe}{DEFAULT_TEXT}Before {?ARMY_LEADER.GENDER}she{?}he{\\?} fell, {ARMY_LEADER.LINK} granted {?ARMY_LEADER.GENDER}her{?}his{\\?} men their customary right of pillage after a successful siege. They may take property but must spare the townsfolk's lives.", null);
				}
			}
			else if (this._besiegerParty.MapFaction.Culture == Settlement.CurrentSettlement.Culture)
			{
				if (flag)
				{
					textObject = new TextObject("{=Wmq47pvL}{DEFAULT_TEXT}{ARMY_LEADER.LINK} had to show mercy to the people of {SETTLEMENT} who were originally descendants of {FACTION}.", null);
					textObject.SetTextVariable("FACTION", Settlement.CurrentSettlement.Culture.GetName());
				}
				else if (this._wasPlayerArmyMember)
				{
					textObject = new TextObject("{=F5Xc0m5O}{DEFAULT_TEXT}{ARMY_LEADER.LINK} fell during the fighting. {?ARMY_LEADER.GENDER}Her{?}His{\\?} troops, reluctant to harm their {CULTURE_ADJ} kinfolk, forego their traditional right of pillage.", null);
					textObject.SetTextVariable("CULTURE_ADJ", FactionHelper.GetAdjectiveForFaction(this._besiegerParty.MapFaction));
				}
			}
			else if (flag)
			{
				textObject = new TextObject("{=Bp0ZQbfp}{DEFAULT_TEXT}{ARMY_LEADER.LINK} has decided to show mercy to the people of {SETTLEMENT}. You can hear disgruntled murmuring among the troops, who have been denied their customary right of pillage.", null);
			}
			else if (this._wasPlayerArmyMember)
			{
				Debug.FailedAssert("_wasPlayerArmyMember", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\SiegeAftermathCampaignBehavior.cs", "menu_settlement_taken_player_army_member_on_init", 428);
				textObject = new TextObject("{=ULtzLvXi}{DEFAULT_TEXT}Before {?ARMY_LEADER.GENDER}she{?}he{\\?} fell, {ARMY_LEADER.LINK} gave orders that mercy should be shown to the people of {SETTLEMENT}.", null);
			}
			TextObject text = args.MenuContext.GameMenu.GetText();
			TextObject textObject2 = new TextObject("{=hvQUqRSb}{SETTLEMENT} has been taken by an army of which you are a member. ", null);
			textObject2.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.GetName());
			textObject.SetTextVariable("DEFAULT_TEXT", textObject2);
			StringHelpers.SetCharacterProperties("ARMY_LEADER", this._besiegerParty.LordPartyComponent.Owner.CharacterObject, textObject, false);
			textObject.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.GetName());
			text.SetTextVariable("LEADER_DECISION_TEXT", textObject);
			text.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.GetName());
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x00125434 File Offset: 0x00123634
		private void menu_settlement_taken_player_participant_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = new TextObject("{=C2KeQd0a}{ENCOUNTER_LEADER.LINK} thanks you for helping in the siege of {SETTLEMENT}. You were able to loot your fallen foes, but you do not participate in the sack of the {?IS_TOWN}town{?}castle{\\?} as you are not part of the army that took it.", null);
			StringHelpers.SetCharacterProperties("ENCOUNTER_LEADER", this._besiegerParty.LordPartyComponent.Owner.CharacterObject, textObject, false);
			textObject.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.GetName());
			textObject.SetTextVariable("IS_TOWN", Settlement.CurrentSettlement.IsTown ? 1 : 0);
			args.MenuContext.GameMenu.GetText().SetTextVariable("PLAYER_PARTICIPANT_TEXT", textObject);
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x001254CD File Offset: 0x001236CD
		private void menu_settlement_taken_continue_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.ExitToLast();
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x001254D4 File Offset: 0x001236D4
		private bool menu_settlement_taken_devastate_on_condition(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty attackerParty = this._besiegerParty;
			Army army = attackerParty.Army;
			MBList<MobileParty> mblist = ((army != null) ? army.Parties.WhereQ((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) > 0).ToMBList<MobileParty>() : null) ?? new MBList<MobileParty>();
			int num = (int)this.GetSiegeAftermathInfluenceCost(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Devastate);
			bool flag = (float)num > 0f;
			bool flag2 = currentSettlement.Culture == attackerParty.MapFaction.Culture;
			TextObject textObject = new TextObject("{=FPPb7ur6}({INFLUENCE_AMOUNT}{INFLUENCE_ICON})", null);
			textObject.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"7\">");
			MBTextManager.SetTextVariable("DEVASTATE_INFLUENCE_COST_TEXT", flag ? textObject : TextObject.Empty, false);
			TextObject textObject2 = new TextObject("{=0FxtNPvV}You cannot devastate a settlement that has your faction culture.{newline}", null);
			TextObject textObject3 = new TextObject("{=Q9RXyDBz}{newline} • {HERO.NAME} must use {INFLUENCE_AMOUNT} influence to convince {MERCIFUL_LORD_COUNT} merciful leaders of this action:{newline} {MERCIFUL_LORDS}", null);
			string tag = "HERO";
			Hero leaderHero = attackerParty.LeaderHero;
			StringHelpers.SetCharacterProperties(tag, (leaderHero != null) ? leaderHero.CharacterObject : null, textObject3, false);
			textObject3.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject3.SetTextVariable("MERCIFUL_LORD_COUNT", mblist.Count);
			List<TextObject> list = new List<TextObject>();
			foreach (MobileParty mobileParty in mblist)
			{
				list.Add(mobileParty.LeaderHero.Name);
			}
			textObject3.SetTextVariable("MERCIFUL_LORDS", GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(list, new TextObject("{=!}{newline}", null), null));
			textObject3.SetTextVariable("INFLUENCE_AMOUNT", num);
			TextObject textObject4 = new TextObject("{=!}{CULTURE_CONDITION_TEXT}{STATIC_CONDITIONS_TEXT}{INFLUENCE_CONDITION_TEXT}", null);
			textObject4.SetTextVariable("CULTURE_CONDITION_TEXT", flag2 ? textObject2 : TextObject.Empty);
			textObject4.SetTextVariable("STATIC_CONDITIONS_TEXT", this.GetSiegeAftermathConsequencesText(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Devastate, true));
			textObject4.SetTextVariable("INFLUENCE_CONDITION_TEXT", flag ? textObject3 : TextObject.Empty);
			args.IsEnabled = this.IsSiegeAftermathPossible(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Devastate);
			args.Tooltip = textObject4;
			args.optionLeaveType = GameMenuOption.LeaveType.Devastate;
			return true;
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x00125710 File Offset: 0x00123910
		private void menu_settlement_taken_devastate_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			this._playerEncounterAftermath = SiegeAftermathAction.SiegeAftermath.Devastate;
			SiegeAftermathAction.ApplyAftermath(besiegerParty, currentSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x00125752 File Offset: 0x00123952
		private bool continue_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x00125760 File Offset: 0x00123960
		private bool menu_settlement_taken_pillage_on_condition(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			bool flag = currentSettlement.Culture == besiegerParty.MapFaction.Culture;
			TextObject textObject = new TextObject("{=!}{CULTURE_CONDITION_TEXT}{STATIC_CONDITIONS_TEXT}", null);
			TextObject textObject2 = new TextObject("{=uwmHjy7z}You cannot pillage a settlement that has your faction culture.{newline}", null);
			textObject.SetTextVariable("CULTURE_CONDITION_TEXT", flag ? textObject2 : TextObject.Empty);
			textObject.SetTextVariable("STATIC_CONDITIONS_TEXT", this.GetSiegeAftermathConsequencesText(besiegerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Pillage, true));
			args.IsEnabled = this.IsSiegeAftermathPossible(besiegerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.Pillage);
			args.Tooltip = textObject;
			args.optionLeaveType = GameMenuOption.LeaveType.Pillage;
			return true;
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x001257F4 File Offset: 0x001239F4
		private void menu_settlement_taken_pillage_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			this._playerEncounterAftermath = SiegeAftermathAction.SiegeAftermath.Pillage;
			SiegeAftermathAction.ApplyAftermath(besiegerParty, currentSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00125838 File Offset: 0x00123A38
		private bool menu_settlement_taken_show_mercy_on_condition(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty attackerParty = this._besiegerParty;
			Army army = attackerParty.Army;
			MBList<MobileParty> mblist = ((army != null) ? army.Parties.WhereQ((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) < 0).ToMBList<MobileParty>() : null) ?? new MBList<MobileParty>();
			int num = (int)this.GetSiegeAftermathInfluenceCost(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.ShowMercy);
			bool flag = currentSettlement.Culture == attackerParty.MapFaction.Culture;
			bool flag2 = (float)num > 0f;
			TextObject textObject = new TextObject("{=FPPb7ur6}({INFLUENCE_AMOUNT}{INFLUENCE_ICON})", null);
			textObject.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"7\">");
			MBTextManager.SetTextVariable("SHOW_MERCY_INFLUENCE_COST_TEXT", flag2 ? textObject : TextObject.Empty, false);
			TextObject textObject2 = new TextObject("{=aXFYyBEQ}Showing mercy to a settlement that shares your faction's culture requires no influence.{newline}", null);
			TextObject textObject3 = new TextObject("{=bn5fpYx3}{newline} • {HERO.NAME} must use {INFLUENCE_AMOUNT} influence to convince {CRUEL_LORD_COUNT} non-merciful leaders of this action:{newline} {CRUEL_LORDS}", null);
			string tag = "HERO";
			Hero leaderHero = attackerParty.LeaderHero;
			StringHelpers.SetCharacterProperties(tag, (leaderHero != null) ? leaderHero.CharacterObject : null, textObject3, false);
			textObject3.SetTextVariable("INFLUENCE_AMOUNT", num);
			textObject3.SetTextVariable("CRUEL_LORD_COUNT", mblist.Count);
			List<TextObject> list = new List<TextObject>();
			foreach (MobileParty mobileParty in mblist)
			{
				list.Add(mobileParty.LeaderHero.Name);
			}
			textObject3.SetTextVariable("CRUEL_LORDS", GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(list, new TextObject("{=!}{newline}", null), null));
			TextObject textObject4 = new TextObject("{=!}{CULTURE_CONDITION_TEXT}{STATIC_CONDITIONS_TEXT}{INFLUENCE_CONDITION_TEXT}", null);
			textObject4.SetTextVariable("CULTURE_CONDITION_TEXT", flag ? textObject2 : TextObject.Empty);
			textObject4.SetTextVariable("STATIC_CONDITIONS_TEXT", this.GetSiegeAftermathConsequencesText(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.ShowMercy, true));
			textObject4.SetTextVariable("INFLUENCE_CONDITION_TEXT", flag2 ? textObject3 : TextObject.Empty);
			args.IsEnabled = this.IsSiegeAftermathPossible(attackerParty, currentSettlement, SiegeAftermathAction.SiegeAftermath.ShowMercy);
			args.Tooltip = textObject4;
			args.optionLeaveType = GameMenuOption.LeaveType.ShowMercy;
			return true;
		}

		// Token: 0x06003C87 RID: 15495 RVA: 0x00125A68 File Offset: 0x00123C68
		private void menu_settlement_taken_show_mercy_on_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			this._playerEncounterAftermath = SiegeAftermathAction.SiegeAftermath.ShowMercy;
			SiegeAftermathAction.ApplyAftermath(besiegerParty, currentSettlement, this._playerEncounterAftermath, this._prevSettlementOwnerClan, this._siegeEventPartyContributions);
			GameMenu.SwitchToMenu("siege_aftermath_contextual_summary");
		}

		// Token: 0x06003C88 RID: 15496 RVA: 0x00125AAC File Offset: 0x00123CAC
		private TextObject GetSiegeAftermathConsequencesText(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermath, bool isTooltip)
		{
			TextObject textObject = new TextObject("{=!}{PROSPERITY_TEXT}{TOWN_PROJECTS_TEXT}{LOYALTY_TEXT}{NOTABLE_POWER_TEXT}{PARTY_MORALE_TEXT}{ARMY_GOLD_TEXT}{PARTY_GOLD_TEXT}{OWNER_RELATION_TEXT}", null);
			TextObject textObject2 = new TextObject("{=ERh2DVEa} • Prosperity Lost: {PROSPERITY_LOST_AMOUNT}", null);
			textObject2.SetTextVariable("PROSPERITY_LOST_AMOUNT", -1 * (int)this.GetSiegeAftermathProsperityPenalty(attackerParty, settlement, aftermath));
			textObject.SetTextVariable("PROSPERITY_TEXT", textObject2);
			TextObject textObject3 = new TextObject("{=HtHcEv7N}{newline} • Party Morale Change : {MORALE_CHANGE}", null);
			textObject3.SetTextVariable("MORALE_CHANGE", this.GetSiegeAftermathPartyMoraleBonus(attackerParty, settlement, aftermath));
			textObject.SetTextVariable("PARTY_MORALE_TEXT", textObject3);
			if (aftermath != SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				TextObject textObject4 = TextObject.Empty;
				if (isTooltip)
				{
					textObject4 = new TextObject("{=tF1G5YLe}{newline} • Building Levels Reduced: {LEVELS_LOST}", null);
					textObject4.SetTextVariable("LEVELS_LOST", this.GetSiegeAftermathProjectsLoss(attackerParty, aftermath));
					textObject.SetTextVariable("TOWN_PROJECTS_TEXT", textObject4);
				}
				else
				{
					textObject4 = new TextObject("{=WDRTZ8se}{newline} • Town Projects Razed: {LEVELS_LOST}{PROJECTS_DESTROYED}", null);
					TextObject textObject5 = new TextObject("{=W1KJEvit}{newline}    Levels Lost: {BUILDINGS_LOST_LEVEL}", null);
					TextObject textObject6 = new TextObject("{=n1bQHmCk}{newline}    Projects Destroyed: {BUILDINGS_DESTROYED}", null);
					TextObject textObject7 = new TextObject("{=HDNedIxl}{newline}        {BUILDING_NAME}: {LEVEL_LOST}", null);
					TextObject textObject8 = new TextObject("{=jZmBbA5M}{newline}        {BUILDING_NAME}", null);
					List<KeyValuePair<Building, int>> list = new List<KeyValuePair<Building, int>>(from t in this._playerEncounterAftermathDamagedBuildings
					where t.Key.CurrentLevel > 0
					select t);
					List<Building> list2 = new List<Building>(from t in this._playerEncounterAftermathDamagedBuildings
					where t.Key.CurrentLevel <= 0
					select t.Key);
					string text = "";
					foreach (KeyValuePair<Building, int> keyValuePair in list)
					{
						TextObject textObject9 = textObject7.CopyTextObject();
						textObject9.SetTextVariable("BUILDING_NAME", keyValuePair.Key.Name);
						textObject9.SetTextVariable("LEVEL_LOST", keyValuePair.Value);
						text += textObject9.ToString();
					}
					string text2 = "";
					foreach (Building building in list2)
					{
						TextObject textObject10 = textObject8.CopyTextObject();
						textObject10.SetTextVariable("BUILDING_NAME", building.Name);
						text2 += textObject10.ToString();
					}
					textObject5.SetTextVariable("BUILDINGS_LOST_LEVEL", text);
					textObject6.SetTextVariable("BUILDINGS_DESTROYED", text2);
					textObject4.SetTextVariable("LEVELS_LOST", (!list.IsEmpty<KeyValuePair<Building, int>>()) ? textObject5.ToString() : "");
					textObject4.SetTextVariable("PROJECTS_DESTROYED", (!list2.IsEmpty<Building>()) ? textObject6.ToString() : "");
					textObject.SetTextVariable("TOWN_PROJECTS_TEXT", (!list2.IsEmpty<Building>() || !list.IsEmpty<KeyValuePair<Building, int>>()) ? textObject4.ToString() : "");
				}
				TextObject textObject11 = new TextObject("{=EVxxKXmW}{newline} • Loyalty in {SETTLEMENT} : {LOYALTY_LOST_AMOUNT}", null);
				textObject11.SetTextVariable("LOYALTY_LOST_AMOUNT", this.GetSiegeAftermathLoyaltyPenalty(aftermath));
				textObject11.SetTextVariable("SETTLEMENT", settlement.GetName());
				textObject.SetTextVariable("LOYALTY_TEXT", textObject11);
				if (settlement.Notables.Count > 0)
				{
					TextObject textObject12 = new TextObject("{=38dcXWzq}{newline} • Notable Powers: {NOTABLE_POWER_LOST_AMOUNT}%", null);
					textObject12.SetTextVariable("NOTABLE_POWER_LOST_AMOUNT", this.GetSiegeAftermathNotablePowerModifierForAftermath(aftermath) * 100f);
					textObject.SetTextVariable("NOTABLE_POWER_TEXT", textObject12);
				}
				TextObject textObject13 = new TextObject("{=RO3Zv0K4}{newline} • Relation with Settlement Owner : {OWNER.LINK} : {RELATION_CHANGE}", null);
				textObject13.SetTextVariable("RELATION_CHANGE", this.GetSiegeAftermathRelationPenaltyWithSettlementOwner(aftermath));
				StringHelpers.SetCharacterProperties("OWNER", this._prevSettlementOwnerClan.Leader.CharacterObject, textObject13, false);
				textObject.SetTextVariable("OWNER_RELATION_TEXT", textObject13);
				int siegeAftermathArmyGoldGain = this.GetSiegeAftermathArmyGoldGain(attackerParty, settlement, aftermath);
				TextObject textObject14 = new TextObject("{=2wwAyZdL}{newline} • Army Gold Gain : {ARMY_GOLD_GAIN}", null);
				textObject14.SetTextVariable("ARMY_GOLD_GAIN", siegeAftermathArmyGoldGain);
				textObject.SetTextVariable("ARMY_GOLD_TEXT", textObject14);
				TextObject textObject15 = new TextObject("{=RmW8Wf83}{newline} • Party Gold Gain : {PARTY_GOLD_GAIN}", null);
				textObject15.SetTextVariable("PARTY_GOLD_GAIN", this.GetSiegeAftermathPartyGoldGain((float)siegeAftermathArmyGoldGain, this._siegeEventPartyContributions[attackerParty]));
				textObject.SetTextVariable("PARTY_GOLD_TEXT", textObject15);
			}
			else
			{
				textObject.SetTextVariable("TOWN_PROJECTS_TEXT", "");
				textObject.SetTextVariable("LOYALTY_TEXT", "");
				textObject.SetTextVariable("NOTABLE_POWER_TEXT", "");
				textObject.SetTextVariable("OWNER_RELATION_TEXT", "");
				textObject.SetTextVariable("ARMY_GOLD_TEXT", "");
				textObject.SetTextVariable("PARTY_GOLD_TEXT", "");
			}
			return textObject;
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x00125F60 File Offset: 0x00124160
		private void siege_aftermath_contextual_summary_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			MobileParty besiegerParty = this._besiegerParty;
			if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				TextObject textObject = new TextObject("{=VFVqjZwY}Your troops sweep through the {?IS_CITY}city{?}fortress{\\?}, taking whatever loot they like and setting fire to the rest.", null);
				textObject.SetTextVariable("IS_CITY", Settlement.CurrentSettlement.IsTown ? 1 : 0);
				MBTextManager.SetTextVariable("START_OF_EXPLANATION", textObject, false);
			}
			else if (this._playerEncounterAftermath == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				MBTextManager.SetTextVariable("START_OF_EXPLANATION", new TextObject("{=oJUxWEwp}You grant your men their customary right of pillage after a successful siege. You tell them they may take property but must spare the townsfolk's lives.", null), false);
			}
			else if (currentSettlement.Culture != besiegerParty.MapFaction.Culture)
			{
				TextObject textObject2 = new TextObject("{=x2dvXNQ0}You have decided to show mercy to the people of {SETTLEMENT_NAME}.{newline}You can hear disgruntled murmuring among the troops, who have been denied their customary right of pillage.", null);
				textObject2.SetTextVariable("SETTLEMENT_NAME", currentSettlement.Name);
				MBTextManager.SetTextVariable("START_OF_EXPLANATION", textObject2, false);
			}
			else
			{
				TextObject textObject3 = new TextObject("{=bXN2fbcv}Your men treat the residents of {SETTLEMENT_NAME} as wayward subjects of the {SETTLEMENT_CULTURE_NAME} rather than foes, and treat them relatively well.", null);
				textObject3.SetTextVariable("SETTLEMENT_NAME", currentSettlement.Name);
				textObject3.SetTextVariable("SETTLEMENT_CULTURE_NAME", FactionHelper.GetFormalNameForFactionCulture(currentSettlement.Culture));
				MBTextManager.SetTextVariable("START_OF_EXPLANATION", textObject3, false);
			}
			MBTextManager.SetTextVariable("CONTEXTUAL_SUMMARY_TEXT", this.GetSiegeAftermathConsequencesText(besiegerParty, currentSettlement, this._playerEncounterAftermath, false), false);
			TextObject text = new TextObject("{=I0ZG4tIj}{TOWN_NAME} has fallen to your troops. You may station a garrison here to defend it against enemies who may try to recapture it.", null);
			MBTextManager.SetTextVariable("TOWN_NAME", Settlement.CurrentSettlement.Name, false);
			MBTextManager.SetTextVariable("END_OF_EXPLANATION", text, false);
			args.MenuContext.SetBackgroundMeshName("encounter_win");
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x001260C0 File Offset: 0x001242C0
		private float GetSiegeAftermathProsperityPenalty(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int num = attackerParty.MemberRoster.TotalHealthyCount;
			if (attackerParty.Army != null)
			{
				num = attackerParty.Army.TotalHealthyMembers;
			}
			float num2 = -1f * ((MathF.Log((float)num * 0.04f + 2f, 2f) * 2.5f + 2.5f) * 0.01f * ((this._settlementProsperityCache < 0f) ? settlement.Town.Prosperity : this._settlementProsperityCache));
			float result = num2;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				result = 1.5f * num2;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				result = 0.5f * num2;
			}
			return result;
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x0012615C File Offset: 0x0012435C
		private int GetSiegeAftermathProjectsLoss(MobileParty attackerParty, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int num = attackerParty.MemberRoster.TotalHealthyCount;
			if (attackerParty.Army != null)
			{
				num = attackerParty.Army.TotalHealthyMembers;
			}
			int num2 = MathF.Floor(MathF.Log((float)num * 0.02f + 2f, 2f));
			int result = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				result = MathF.Ceiling(1.5f * (float)num2);
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				result = num2;
			}
			return result;
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x001261C4 File Offset: 0x001243C4
		private float GetSiegeAftermathLoyaltyPenalty(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float result = 0f;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				result = -30f;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				result = -15f;
			}
			return result;
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x001261F0 File Offset: 0x001243F0
		private float GetSiegeAftermathNotablePowerModifierForAftermath(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float result = 0f;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				result = -0.5f;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				result = -0.25f;
			}
			return result;
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x0012621C File Offset: 0x0012441C
		private float GetSiegeAftermathPartyMoraleBonus(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int num = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				num = 20;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				num = 10;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy && attackerParty.MapFaction.Culture != settlement.Culture)
			{
				num = -15;
			}
			return (float)num;
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x00126258 File Offset: 0x00124458
		private int GetSiegeAftermathArmyGoldGain(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float num = -1f * this.GetSiegeAftermathProsperityPenalty(attackerParty, settlement, aftermathType);
			float f = 0f;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate || aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				f = num * 15f;
			}
			return MathF.Floor(f);
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00126290 File Offset: 0x00124490
		private int GetSiegeAftermathPartyGoldGain(float totalArmyGoldGain, float partyContributionPercentage)
		{
			return MathF.Floor(totalArmyGoldGain * partyContributionPercentage / 100f);
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x001262A0 File Offset: 0x001244A0
		private int GetSiegeAftermathRelationChangeWithLord(Hero hero, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			if (hero.GetTraitLevel(DefaultTraits.Mercy) > 0)
			{
				return this.GetSiegeAftermathRelationChangeWithMercifulLord(aftermathType);
			}
			if (hero.GetTraitLevel(DefaultTraits.Mercy) < 0)
			{
				return -1 * this.GetSiegeAftermathRelationChangeWithMercifulLord(aftermathType);
			}
			return 0;
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x001262D4 File Offset: 0x001244D4
		private int GetSiegeAftermathRelationChangeWithMercifulLord(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int result = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				result = -10;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy)
			{
				result = 10;
			}
			return result;
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x001262F4 File Offset: 0x001244F4
		private int GetSiegeAftermathRelationPenaltyWithSettlementOwner(SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int result = 0;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				result = -30;
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				result = -15;
			}
			return result;
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x00126314 File Offset: 0x00124514
		private float GetSiegeAftermathInfluenceCost(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float result = 0f;
			if (attackerParty.Army != null && aftermathType != SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				int num = attackerParty.Army.Parties.Count((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) > 0);
				int num2 = attackerParty.Army.Parties.Count((MobileParty t) => t != attackerParty && t.LeaderHero != null && t.LeaderHero.GetTraitLevel(DefaultTraits.Mercy) < 0);
				if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
				{
					result = settlement.Town.Prosperity / 400f * (float)num;
				}
				else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy && attackerParty.MapFaction.Culture != settlement.Culture)
				{
					result = settlement.Town.Prosperity / 400f * (float)num2;
				}
			}
			return result;
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x001263DC File Offset: 0x001245DC
		private bool IsSiegeAftermathPossible(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			float siegeAftermathInfluenceCost = this.GetSiegeAftermathInfluenceCost(attackerParty, settlement, aftermathType);
			bool flag = attackerParty.ActualClan.Influence >= siegeAftermathInfluenceCost;
			bool flag2 = settlement.Culture == attackerParty.MapFaction.Culture;
			bool result;
			if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
			{
				result = (flag && !flag2);
			}
			else if (aftermathType == SiegeAftermathAction.SiegeAftermath.Pillage)
			{
				result = !flag2;
			}
			else
			{
				result = (flag || flag2);
			}
			return result;
		}

		// Token: 0x06003C96 RID: 15510 RVA: 0x0012643C File Offset: 0x0012463C
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.IsFortification && detail == ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail.BySiege && capturerHero != null && settlement.OwnerClan != null && settlement.OwnerClan != Clan.PlayerClan && !oldOwner.IsDead)
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(oldOwner, capturerHero, -10, capturerHero == Hero.MainHero);
				if (capturerHero.MapFaction.Leader != capturerHero && settlement.OwnerClan.Leader != capturerHero.MapFaction.Leader)
				{
					ChangeRelationAction.ApplyRelationChangeBetweenHeroes(oldOwner, capturerHero.MapFaction.Leader, -6, capturerHero.MapFaction.Leader == Hero.MainHero);
				}
			}
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x001264E8 File Offset: 0x001246E8
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<MobileParty>("_besiegerParty", ref this._besiegerParty);
			dataStore.SyncData<Clan>("_prevSettlementOwnerClan", ref this._prevSettlementOwnerClan);
			dataStore.SyncData<SiegeAftermathAction.SiegeAftermath>("_playerEncounterAftermath", ref this._playerEncounterAftermath);
			dataStore.SyncData<Dictionary<MobileParty, float>>("_siegeEventPartyContributions", ref this._siegeEventPartyContributions);
			dataStore.SyncData<bool>("_wasPlayerArmyMember", ref this._wasPlayerArmyMember);
			dataStore.SyncData<Dictionary<Building, int>>("_playerEncounterAftermathDamagedBuildings", ref this._playerEncounterAftermathDamagedBuildings);
		}

		// Token: 0x04001205 RID: 4613
		private MobileParty _besiegerParty;

		// Token: 0x04001206 RID: 4614
		private Clan _prevSettlementOwnerClan;

		// Token: 0x04001207 RID: 4615
		private SiegeAftermathAction.SiegeAftermath _playerEncounterAftermath;

		// Token: 0x04001208 RID: 4616
		private Dictionary<MobileParty, float> _siegeEventPartyContributions = new Dictionary<MobileParty, float>();

		// Token: 0x04001209 RID: 4617
		private Dictionary<Building, int> _playerEncounterAftermathDamagedBuildings = new Dictionary<Building, int>();

		// Token: 0x0400120A RID: 4618
		private bool _wasPlayerArmyMember;

		// Token: 0x0400120B RID: 4619
		private float _settlementProsperityCache = -1f;
	}
}
