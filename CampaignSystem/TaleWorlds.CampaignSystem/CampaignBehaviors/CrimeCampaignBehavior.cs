using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000384 RID: 900
	public class CrimeCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003558 RID: 13656 RVA: 0x000E702C File Offset: 0x000E522C
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterGameCreated));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroDeath));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000E70AC File Offset: 0x000E52AC
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000E70B0 File Offset: 0x000E52B0
		private void OnDailyTick()
		{
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				float dailyCrimeRatingChange = clan.DailyCrimeRatingChange;
				if (!clan.IsEliminated && !dailyCrimeRatingChange.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					ChangeCrimeRatingAction.Apply(clan, dailyCrimeRatingChange, false);
				}
			}
			foreach (Kingdom kingdom in Kingdom.All)
			{
				float dailyCrimeRatingChange2 = kingdom.DailyCrimeRatingChange;
				if (!kingdom.IsEliminated && !dailyCrimeRatingChange2.ApproximatelyEqualsTo(0f, 1E-05f))
				{
					ChangeCrimeRatingAction.Apply(kingdom, dailyCrimeRatingChange2, false);
				}
			}
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000E7188 File Offset: 0x000E5388
		private void OnAfterGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000E7194 File Offset: 0x000E5394
		private void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenu("town_inside_criminal", "{=XgA2JgVR}You are brought to the town square to face judgment.", new OnInitDelegate(CrimeCampaignBehavior.town_inside_criminal_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_punishment", "{=8iDpmu0L}Accept corporal punishment", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_give_punishment_and_money", "{=Xi1wpR2L}Accept corporal punishment and pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_give_your_life", "{=bVi0JKSx}You will be executed. You must face it as bravely as you can", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_influence", "{=1cMS6415}Pay {FINE}{INFLUENCE_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_pay_by_money", "{=870ZCp1J}Pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_inside_criminal", "criminal_inside_menu_ignore_charges", "{=UQhRKJb9}Ignore the charges", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_ignore_charges_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_ignore_charges_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenu("town_discuss_criminal_surrender", "{=lwVwe4qU}You are discussing the terms of your surrender.", new OnInitDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_on_init), GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_punishment", "{=8iDpmu0L}Accept corporal punishment", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_pay_by_punishment_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_give_punishment_and_money", "{=Xi1wpR2L}Accept corporal punishment and pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_punishment_and_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_give_your_life", "{=VSzwMDJ2}You will be put to death", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_your_life_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_influence", "{=1cMS6415}Pay {FINE}{INFLUENCE_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_influence_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_pay_by_money", "{=870ZCp1J}Pay {FINE}{GOLD_ICON}", new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.criminal_inside_menu_give_money_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_discuss_criminal_surrender", "town_discuss_criminal_surrender_back", GameTexts.FindText("str_back", null).ToString(), new GameMenuOption.OnConditionDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_on_condition), new GameMenuOption.OnConsequenceDelegate(CrimeCampaignBehavior.town_discuss_criminal_surrender_back_on_consequence), true, -1, false, null);
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000E7438 File Offset: 0x000E5638
		private void OnHeroDeath(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (victim == Hero.MainHero)
			{
				foreach (Clan clan in Clan.NonBanditFactions)
				{
					if (!clan.IsEliminated)
					{
						ChangeCrimeRatingAction.Apply(clan, -clan.MainHeroCrimeRating, true);
					}
				}
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (!kingdom.IsEliminated)
					{
						ChangeCrimeRatingAction.Apply(kingdom, -kingdom.MainHeroCrimeRating, true);
					}
				}
			}
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000E74F0 File Offset: 0x000E56F0
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			if (side1Faction == Hero.MainHero.MapFaction || side2Faction == Hero.MainHero.MapFaction)
			{
				IFaction faction = (side1Faction == Hero.MainHero.MapFaction) ? side2Faction : side1Faction;
				float num = (float)Campaign.Current.Models.CrimeModel.DeclareWarCrimeRatingThreshold * 0.5f;
				if (faction.MainHeroCrimeRating > num)
				{
					ChangeCrimeRatingAction.Apply(faction, num - faction.MainHeroCrimeRating, true);
				}
			}
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000E7560 File Offset: 0x000E5760
		private static bool CanPayCriminalRatingValueWith(IFaction faction, CrimeModel.PaymentMethod paymentMethod)
		{
			if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingModerate(Settlement.CurrentSettlement.MapFaction))
			{
				if (paymentMethod == CrimeModel.PaymentMethod.Gold)
				{
					return true;
				}
				if (CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(faction))
				{
					if (paymentMethod == CrimeModel.PaymentMethod.Influence)
					{
						return true;
					}
				}
				else if (paymentMethod == CrimeModel.PaymentMethod.Punishment)
				{
					return true;
				}
			}
			else if (Campaign.Current.Models.CrimeModel.IsPlayerCrimeRatingSevere(Settlement.CurrentSettlement.MapFaction))
			{
				if (CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(faction))
				{
					if (paymentMethod == CrimeModel.PaymentMethod.Gold)
					{
						return true;
					}
					if (paymentMethod == CrimeModel.PaymentMethod.Influence)
					{
						return true;
					}
				}
				else
				{
					if (paymentMethod.HasAnyFlag(CrimeModel.PaymentMethod.Execution))
					{
						return Hero.MainHero.Gold < (int)PayForCrimeAction.GetClearCrimeCost(faction, CrimeModel.PaymentMethod.Gold);
					}
					if (paymentMethod.HasAllFlags(CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000E7608 File Offset: 0x000E5808
		private static bool IsCriminalPlayerInSameKingdomOf(IFaction faction)
		{
			Clan clan = faction as Clan;
			return Hero.MainHero.Clan == faction || Hero.MainHero.Clan.Kingdom == faction || (clan != null && Hero.MainHero.Clan.Kingdom == clan.Kingdom);
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000E7659 File Offset: 0x000E5859
		[GameMenuInitializationHandler("town_discuss_criminal_surrender")]
		[GameMenuInitializationHandler("town_inside_criminal")]
		public static void game_menu_town_criminal_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.Town.WaitMeshName);
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000E7675 File Offset: 0x000E5875
		public static void town_inside_criminal_on_init(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000E7688 File Offset: 0x000E5888
		public static void town_discuss_criminal_surrender_on_init(MenuCallbackArgs args)
		{
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000E768A File Offset: 0x000E588A
		public static bool criminal_inside_menu_pay_by_punishment_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Punishment);
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000E76A4 File Offset: 0x000E58A4
		public static void criminal_inside_menu_pay_by_punishment_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Punishment);
			if (Hero.MainHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (Settlement.CurrentSettlement.IsCastle)
					{
						GameMenu.SwitchToMenu("castle_outside");
						return;
					}
					GameMenu.SwitchToMenu("town_outside");
					return;
				}
				else
				{
					PlayerEncounter.Finish(true);
				}
			}
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x000E7704 File Offset: 0x000E5904
		public static bool criminal_inside_menu_give_money_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Trade;
			int num = (int)PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			args.Text.SetTextVariable("FINE", num);
			args.Text.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
			if (Hero.MainHero.Gold < num)
			{
				args.Tooltip = new TextObject("{=d0kbtGYn}You don't have enough gold.", null);
				args.IsEnabled = false;
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000E7788 File Offset: 0x000E5988
		public static void criminal_inside_menu_give_money_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000E77BC File Offset: 0x000E59BC
		public static bool criminal_inside_menu_give_influence_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Bribe;
			float clearCrimeCost = PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
			args.Text.SetTextVariable("FINE", clearCrimeCost.ToString("F1"));
			args.Text.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"7\">");
			if (Clan.PlayerClan.Influence < clearCrimeCost)
			{
				args.Tooltip = new TextObject("{=rMagXCrI}You don't have enough influence to get the charges dropped.", null);
				args.IsEnabled = false;
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000E7849 File Offset: 0x000E5A49
		public static void criminal_inside_menu_give_influence_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Influence);
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_outside");
				return;
			}
			GameMenu.SwitchToMenu("town_outside");
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000E787C File Offset: 0x000E5A7C
		public static bool criminal_inside_menu_give_punishment_and_money_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			int num = (int)PayForCrimeAction.GetClearCrimeCost(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold);
			args.Text.SetTextVariable("FINE", num);
			args.Text.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
			if (Hero.MainHero.Gold < num)
			{
				args.Tooltip = new TextObject("{=ETKyjOkJ}You don't have enough denars to pay the fine.", null);
				args.IsEnabled = false;
			}
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment);
		}

		// Token: 0x0600356B RID: 13675 RVA: 0x000E7900 File Offset: 0x000E5B00
		public static void criminal_inside_menu_give_punishment_and_money_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Gold | CrimeModel.PaymentMethod.Punishment);
			if (Hero.MainHero.DeathMark != KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (Settlement.CurrentSettlement.IsCastle)
					{
						GameMenu.SwitchToMenu("castle_outside");
						return;
					}
					GameMenu.SwitchToMenu("town_outside");
					return;
				}
				else
				{
					PlayerEncounter.Finish(true);
				}
			}
		}

		// Token: 0x0600356C RID: 13676 RVA: 0x000E795E File Offset: 0x000E5B5E
		public static bool criminal_inside_menu_give_your_life_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Surrender;
			return CrimeCampaignBehavior.CanPayCriminalRatingValueWith(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Execution);
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x000E7978 File Offset: 0x000E5B78
		public static void criminal_inside_menu_give_your_life_on_consequence(MenuCallbackArgs args)
		{
			PayForCrimeAction.Apply(Settlement.CurrentSettlement.MapFaction, CrimeModel.PaymentMethod.Execution);
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x000E798A File Offset: 0x000E5B8A
		public static bool criminal_inside_menu_ignore_charges_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return CrimeCampaignBehavior.IsCriminalPlayerInSameKingdomOf(Settlement.CurrentSettlement.MapFaction);
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x000E79A3 File Offset: 0x000E5BA3
		public static void criminal_inside_menu_ignore_charges_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle");
				return;
			}
			GameMenu.SwitchToMenu("town");
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x000E79C6 File Offset: 0x000E5BC6
		public static void town_discuss_criminal_surrender_back_on_consequence(MenuCallbackArgs args)
		{
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle_guard");
				return;
			}
			GameMenu.SwitchToMenu("town_guard");
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000E79E9 File Offset: 0x000E5BE9
		public static bool town_discuss_criminal_surrender_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Submenu;
			return true;
		}
	}
}
