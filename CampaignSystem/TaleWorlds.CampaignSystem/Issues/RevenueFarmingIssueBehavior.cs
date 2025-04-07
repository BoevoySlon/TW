using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x0200031A RID: 794
	public class RevenueFarmingIssueBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x000BF336 File Offset: 0x000BD536
		private float IncidentChance
		{
			get
			{
				return (100f - RevenueFarmingIssueBehavior.Instance.TargetSettlement.Town.Loyalty * 0.8f) * 0.01f;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06002DAE RID: 11694 RVA: 0x000BF360 File Offset: 0x000BD560
		private static RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest Instance
		{
			get
			{
				RevenueFarmingIssueBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<RevenueFarmingIssueBehavior>();
				if (campaignBehavior._cachedQuest != null && campaignBehavior._cachedQuest.IsOngoing)
				{
					return campaignBehavior._cachedQuest;
				}
				using (List<QuestBase>.Enumerator enumerator = Campaign.Current.QuestManager.Quests.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest cachedQuest;
						if ((cachedQuest = (enumerator.Current as RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)) != null)
						{
							campaignBehavior._cachedQuest = cachedQuest;
							return campaignBehavior._cachedQuest;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000BF3F8 File Offset: 0x000BD5F8
		public override void RegisterEvents()
		{
			CampaignEvents.OnCheckForIssueEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnCheckForIssue));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnAfterSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterSessionLaunchedEvent));
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000BF44C File Offset: 0x000BD64C
		private void OnAfterSessionLaunchedEvent(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenuOption("town_guard", "talk_to_steward_for_revenue_town", "{=voXpzZdH}Hand over the revenue", new GameMenuOption.OnConditionDelegate(this.talk_to_steward_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_steward_on_consequence), false, 2, false, null);
			gameStarter.AddGameMenuOption("town", "talk_to_steward_for_revenue_town", "{=voXpzZdH}Hand over the revenue", new GameMenuOption.OnConditionDelegate(this.talk_to_steward_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_steward_on_consequence), false, 9, false, null);
			gameStarter.AddGameMenuOption("castle_guard", "talk_to_steward_for_revenue_castle", "{=voXpzZdH}Hand over the revenue", new GameMenuOption.OnConditionDelegate(this.talk_to_steward_on_condition), new GameMenuOption.OnConsequenceDelegate(this.talk_to_steward_on_consequence), false, 2, false, null);
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000BF4F0 File Offset: 0x000BD6F0
		private void OnSessionLaunched(CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenuOption("village", "revenue_farming_quest_collect_tax_menu_button", "{=mcrjFxDQ}Collect revenue", new GameMenuOption.OnConditionDelegate(this.collect_revenue_menu_condition), new GameMenuOption.OnConsequenceDelegate(this.collect_revenue_menu_consequence), false, 5, false, null);
			gameStarter.AddWaitGameMenu("village_collect_revenue", "{=p6swAFWn}Your men started collecting the revenues...", new OnInitDelegate(this.collecting_menu_on_init), null, null, new OnTickDelegate(this.collection_menu_on_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameOverlays.MenuOverlayType.None, 10f, GameMenu.MenuFlags.None, null);
			gameStarter.AddGameMenuOption("village_collect_revenue", "village_collect_revenue_back", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.leave_consequence), true, -1, false, null);
			this.AddVillageEvents(gameStarter);
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000BF59C File Offset: 0x000BD79C
		private bool talk_to_steward_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
			if (RevenueFarmingIssueBehavior.Instance != null)
			{
				if (Hero.MainHero.Gold < RevenueFarmingIssueBehavior.Instance._totalRequestedDenars)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=QOWyEJrm}You don't have enough denars.", null);
				}
				if (!RevenueFarmingIssueBehavior.Instance._allRevenuesAreCollected)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=QrAowQ5f}You have to collect the revenues first.", null);
				}
				return Settlement.CurrentSettlement.OwnerClan == RevenueFarmingIssueBehavior.Instance.QuestGiver.Clan;
			}
			return false;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000BF62E File Offset: 0x000BD82E
		private void talk_to_steward_on_consequence(MenuCallbackArgs args)
		{
			RevenueFarmingIssueBehavior.Instance.RevenuesAreDeliveredToSteward();
			if (Settlement.CurrentSettlement.IsCastle)
			{
				GameMenu.SwitchToMenu("castle");
				return;
			}
			GameMenu.SwitchToMenu("town");
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000BF65C File Offset: 0x000BD85C
		private void AddVillageEvents(CampaignGameStarter gameStarter)
		{
			this._villageEvents = new List<RevenueFarmingIssueBehavior.VillageEvent>();
			string mainEventText = "{=RabC7Wzm}The headman tells you that most of the villagers can't afford the rest of the tax. They offer crops and other goods as payment in kind.";
			TextObject mainLog = new TextObject("{=5hgc03yZ}While your men were collecting revenues, a headman came and told you that most of the villagers couldn't afford to pay what they owe. They offered to pay the rest with their products.", null);
			List<RevenueFarmingIssueBehavior.VillageEventOptionData> list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=XVzQ7MXQ}Refuse the offer, break into their homes and collect all rents and taxes by force.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 10)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				TraitLevelingHelper.OnIssueSolvedThroughQuest(RevenueFarmingIssueBehavior.Instance.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Mercy, -50)
				});
				int variable = MBRandom.RandomInt(2, 4);
				TextObject textObject = new TextObject("{=3vFxRKja}You refused his offer and decided to collect the rest of the revenues by force. Your action upset the village notables and made villagers angry. Some villagers tried to resist. In the brawl, {WOUNDED_COUNT} of your men got wounded.", null);
				textObject.SetTextVariable("WOUNDED_COUNT", variable);
				RevenueFarmingIssueBehavior.Instance.AddLog(textObject, false);
				this.ChangeRelationWithNotables(-5);
				int num = MBRandom.RandomInt(2, 4);
				MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(num);
				TextObject textObject2 = new TextObject("{=o27lTMD4}Some villagers tried to resist. In the brawl, {WOUNDED_NUMBER} of your men got wounded.", null);
				textObject2.SetTextVariable("WOUNDED_NUMBER", num);
				MBInformationManager.AddQuickInformation(textObject2, 0, null, "");
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=buKXELE3}Accept the offer.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Trade;
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
				int variable = (int)((float)revenueVillage.TargetAmount * 0.5f / (float)revenueVillage.Village.VillageType.PrimaryProduction.Value);
				TextObject textObject = new TextObject("{=wZfbYfoH}They will give you {PRODUCT_COUNT} {.%}{?(PRODUCT_COUNT > 1)}{PLURAL(PRODUCT)}{?}{PRODUCT}{\\?}{.%}.", null);
				textObject.SetTextVariable("PRODUCT", revenueVillage.Village.VillageType.PrimaryProduction.Name);
				textObject.SetTextVariable("PRODUCT_COUNT", variable);
				args.Tooltip = textObject;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				int variable;
				this.GiveVillageGoods(out variable);
				TextObject textObject = new TextObject("{=b5InObbq}You accepted the headman's offer. The village's notables and villagers were happy with your decision and they gave you {PRODUCT_COUNT} {.%}{?(PRODUCT_COUNT > 1)}{PLURAL(PRODUCT)}{?}{PRODUCT}{\\?}{.%}.", null);
				textObject.SetTextVariable("PRODUCT", RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().Village.VillageType.PrimaryProduction.Name);
				textObject.SetTextVariable("PRODUCT_COUNT", variable);
				RevenueFarmingIssueBehavior.Instance.AddLog(textObject, false);
				this.ChangeRelationWithNotables(1);
				this.CompleteCurrentRevenueCollection(true);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=jULnw6F1}Leave the village, telling the villagers that they are exempted from payment this year.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=a3WpsFTM}You decided to exempt the rest of the villagers from payment and left the village. The village's notables and farmers were grateful to you.", null), false);
				this.ChangeRelationWithNotables(3);
				this.CompleteCurrentRevenueCollection(true);
			}, false));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("offer_goods_and_troops", mainEventText, mainLog, list));
			mainEventText = "{=tVYLzFwu}Suddenly a brawl starts between your troops and some of the village youth.";
			mainLog = new TextObject("{=vKaeKPJ5}Revenue collection was interrupted by a sudden brawl between your troops and young men of the village.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=eJegI0iX}Order the rest of your troops to put the village youth to flight.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Mission;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 10)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=Zx1ZEl6Q}You ordered your troops to fight back. In the heat of the brawl, one of the young men was struck on the head and killed. His death greatly upset the villagers.", null), false);
				MBInformationManager.AddQuickInformation(new TextObject("{=xfEVlh7v}Your men beat some of youngsters to the death.", null), 0, null, "");
				this.ChangeRelationWithNotables(-5);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=Z6IoX4MH}Order your troops to try not to hurt the youth and try to separate the two sides.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 10)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				int num = MBRandom.RandomInt(6, 10);
				TextObject textObject = new TextObject("{=YRocrk78}You ordered your troops to disengage. When the dust settled, {WOUNDED} of them had been injured. But the village notables understood that you wanted a peaceful solution.", null);
				textObject.SetTextVariable("WOUNDED", num);
				RevenueFarmingIssueBehavior.Instance.AddLog(textObject, false);
				TextObject textObject2 = new TextObject("{=00Qvwelb}{WOUNDED_NUMBER} of your men got wounded while they were trying to separate the two sides.", null);
				textObject2.SetTextVariable("WOUNDED_NUMBER", num);
				MBInformationManager.AddQuickInformation(textObject2, 0, null, "");
				MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(num);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=Xl5JTBJE}Leave the village, telling them you've collected enough.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=T0feOigD}You decided to stop collecting revenues and leave the village. You told the villagers that they had paid enough, and they were grateful to you.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("brawl_breaks_out", mainEventText, mainLog, list));
			mainEventText = "{=cOlZvnal}A landlord says that his retainers, who help keep order in the village, have gone unpaid and are starting to get mutinous. He says that if you can't help him out with a small sum of money to pay them while you collect the revenues from the rest of the village, he can't guarantee that things will go peacefully.";
			mainLog = new TextObject("{=HK4pwetq}A few hours after the revenue collection started, a landlord came and told you that his retainers were getting mutinuous. He asked you for some money to pay them their back wages.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=0p0jXXIa}Reject the landlord's request for money and collect revenues as before.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				this.ChangeRelationWithNotables(-5);
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=bS7IAgJS}You told the notable that this was not your affair. A few hours later, the mutineers ambushed and killed some of your men on the outskirts of the village, and the revenues stolen.", null), false);
					GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().CollectedAmount, true);
					TextObject textObject = GameTexts.FindText("str_quest_collect_debt_quest_gold_removed", null);
					textObject.SetTextVariable("GOLD_AMOUNT", RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().CollectedAmount);
					InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), "event:/ui/notification/coins_negative"));
					this.CompleteCurrentRevenueCollection(false);
					int num = MBRandom.RandomInt(2, 5);
					TextObject textObject2 = new TextObject("{=mosHZG3b}The mutineers ambushed and killed {KILLED_NUMBER} of your men.", null);
					textObject2.SetTextVariable("KILLED_NUMBER", num);
					MBInformationManager.AddQuickInformation(textObject2, 0, null, "");
					MobileParty.MainParty.MemberRoster.KillNumberOfNonHeroTroopsRandomly(num);
					return;
				}
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=KQ8AU8Bz}You told the notable that this was not your affair. He did not like to hear this.", null), false);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=EmJDw5xP}Give the landlord a small bribe for his men, and continue to collect revenues with their help.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Trade;
				int num = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().TargetAmount / 3;
				if (Hero.MainHero.Gold < num)
				{
					args.Tooltip = new TextObject("{=m6uSOtE4}You don't have enough money.", null);
					args.IsEnabled = false;
				}
				else
				{
					TextObject textObject = new TextObject("{=hCavIm4G}You will pay {AMOUNT}{GOLD_ICON} denars.", null);
					textObject.SetTextVariable("AMOUNT", num);
					args.Tooltip = textObject;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=kp19y5Hh}You paid off the landlords' retainers to forestall a mutiny. The village notables were grateful to you.", null), false);
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().TargetAmount / 3, false);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=DhrjR8bs}Announce that the villagers have paid enough, and leave the village.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=1yCeyK4I}You declared that the village had paid enough, and departed.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("landlord_asks_for_money", mainEventText, mainLog, list));
			mainEventText = "{=pBII35Fa}As your man were collecting the tax, the headman shouted out to you across the fields that there has been an outbreak of the flux in the village. He warns you, for your own good, to stay away.";
			mainLog = new TextObject("{=fn59IIUf}As your man were collecting the tax, the headman shouted out to you that the village had seen an outbreak of the flux, and that you should stay away.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=CbapENnw}Tell your men that the headman is probably lying, and to go ahead and collect the revenues.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=9AyDNhQj}You told your men to ignore the warning. Several hours after you left, some of your men started experiencing chills, then diarrhea. This does not appear to be a particularly virulent strain, as no one died, but about half your men are incapacitated.", null), false);
					int num = MobileParty.MainParty.MemberRoster.TotalHealthyCount / 2;
					TextObject textObject = new TextObject("{=rmmZayCT}{WOUNDED_COUNT} of your men got wounded because of illness.", null);
					textObject.SetTextVariable("WOUNDED_COUNT", num);
					MBInformationManager.AddQuickInformation(textObject, 0, null, "");
					MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(num);
				}
				else
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=obhcbsQT}You told your men to ignore the warning.", null), false);
				}
				this.ChangeRelationWithNotables(-4);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=iE5vWYj2}Tell your men to be careful, and to touch nothing in a house where anyone has been sick.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=b3GrvocA}You told your men to go carefully, but still collect the revenues. The village notables seemed upset with your decision.", null), false);
				revenueVillage.SetAdditionalProgress(0.35f);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=YZZ4zjxU}Tell the villagers that, in light of their hardship, they are forgiven what they owe.", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=JSI0FVZ1}You decided to forgive the villagers' back payments. The headman thanked you, as the villagers were already suffering.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("village_is_under_quarantine", mainEventText, mainLog, list));
			mainEventText = "{=yPkHn74X}When you enter the village commons, you find a crowd of villagers has gathered to resist you. They call the rents and taxes owed 'unlawful' and refuse to pay them at all. They pelt your men with rotten vegetables.";
			mainLog = new TextObject("{=yPkHn74X}When you enter the village commons, you find a crowd of villagers has gathered to resist you. They call the rents and taxes owed 'unlawful' and refuse to pay them at all. They pelt your men with rotten vegetables.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=aZ9bME9C}Order your men to break up the crowd by force", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount <= 9)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=ztY2Nf0N}You ordered your men to break up the crowd. There was some scuffling, and some of your men were wounded.", null), false);
					int num = MBRandom.RandomInt(6, 8);
					TextObject textObject = new TextObject("{=xJwo7eBh}{WOUNDED_NUMBER} of your men got wounded while they were breaking up the crowd.", null);
					textObject.SetTextVariable("WOUNDED_NUMBER", num);
					MBInformationManager.AddQuickInformation(textObject, 0, null, "");
					MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(num);
				}
				else
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=ObYvBt0e}You ordered your men to break up the crowd. The attempt was successful and your men continued collecting taxes as usual.", null), false);
				}
				this.ChangeRelationWithNotables(-5);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=4MPhLYcT}Bargain with the group, agreeing to forgive the debts of the poorest villagers", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.DefendAction;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().SetAdditionalProgress(0.5f);
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=54RyKzPJ}After your attempts to bargain, a deal has been made to forgive the debts of the poorest villagers.", null), false);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=tZw45isr}Tell the villagers that they made their point and that you're leaving", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=6TYsIQav}After observing the villagers' hardships, you called back your men so as not put any more burden on them.", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("refuse_to_pay_what_they_owe", mainEventText, mainLog, list));
			mainEventText = "{=Tl4yagLi}The headman tells you that some households have suffered particularly hard this year from crop failures and bandit depredations. He asks you to forgive their back payments entirely. He hints that they are so desperate that they might resist by force.";
			mainLog = new TextObject("{=Tl4yagLi}The headman tells you that some households have suffered particularly hard this year from crop failures and bandit depredations. He asks you to forgive their back payments entirely. He hints that they are so desperate that they might resist by force.", null);
			list = new List<RevenueFarmingIssueBehavior.VillageEventOptionData>();
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=agMtRiru}Refuse to exempt anyone", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Continue;
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 5)
				{
					args.Tooltip = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
					args.IsEnabled = false;
				}
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				if (MBRandom.RandomFloat < this.IncidentChance)
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=VsriS0iI}You refused to exempt anyone, but the residents attacked and killed some of your troops who were separated from the rest, and then ran away.", null), false);
					int num = MBRandom.RandomInt(2, 4);
					TextObject textObject = new TextObject("{=MGD8Ka2o}The residents attacked and killed {KILLED_NUMBER} of your troops who were separated from the rest.", null);
					textObject.SetTextVariable("KILLED_NUMBER", num);
					MBInformationManager.AddQuickInformation(textObject, 0, null, "");
					MobileParty.MainParty.MemberRoster.KillNumberOfNonHeroTroopsRandomly(num);
				}
				else
				{
					RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=Rz1kkvbK}You refused to exempt anyone. Fortunately the villagers were sufficiently cowed by your men, and did not raise a hand.", null), false);
				}
				this.ChangeRelationWithNotables(-5);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=WDp5EAl3}Agree to exempt the poor households", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().SetAdditionalProgress(0.35f);
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=o70h6xqb}You showed mercy and exempted the poor households from the tax collection", null), false);
				this.ChangeRelationWithNotables(2);
				this.collect_revenue_menu_consequence(args);
			}, false));
			list.Add(new RevenueFarmingIssueBehavior.VillageEventOptionData("{=aMleZjlG}Tell the villagers that they have all paid enough, and depart", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				RevenueFarmingIssueBehavior.Instance.AddLog(new TextObject("{=rsQhD7sC}You thought that the villagers have paid enough, so departed from the settlement", null), false);
				this.ChangeRelationWithNotables(4);
				this.CompleteCurrentRevenueCollection(true);
			}, true));
			this._villageEvents.Add(new RevenueFarmingIssueBehavior.VillageEvent("relief_for_the_poorest", mainEventText, mainLog, list));
			foreach (RevenueFarmingIssueBehavior.VillageEvent villageEvent in this._villageEvents)
			{
				this.AddVillageEventMenus(villageEvent, gameStarter);
			}
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000BFC18 File Offset: 0x000BDE18
		private void AddVillageEventMenus(RevenueFarmingIssueBehavior.VillageEvent villageEvent, CampaignGameStarter gameStarter)
		{
			gameStarter.AddGameMenu(villageEvent.Id, villageEvent.MainEventText, null, GameOverlays.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			for (int i = 0; i < villageEvent.OptionConditionsAndConsequences.Count; i++)
			{
				RevenueFarmingIssueBehavior.VillageEventOptionData villageEventOptionData = villageEvent.OptionConditionsAndConsequences[i];
				gameStarter.AddGameMenuOption(villageEvent.Id, "Id_option" + i, villageEventOptionData.Text, villageEventOptionData.OnCondition, villageEventOptionData.OnConsequence, villageEventOptionData.IsLeave, -1, false, null);
			}
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000BFC98 File Offset: 0x000BDE98
		private bool collect_revenue_menu_condition(MenuCallbackArgs args)
		{
			if (RevenueFarmingIssueBehavior.Instance == null || !RevenueFarmingIssueBehavior.Instance.IsOngoing || (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty != MobileParty.MainParty))
			{
				return false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.Manage;
			args.OptionQuestData = GameMenuOption.IssueQuestFlags.ActiveIssue;
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.RevenueVillages.FirstOrDefault((RevenueFarmingIssueBehavior.RevenueVillage x) => x.Village == Settlement.CurrentSettlement.Village);
			if (revenueVillage != null && !revenueVillage.GetIsCompleted())
			{
				bool flag = MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 20;
				TextObject disabledText = new TextObject("{=CfCsGTfb}Villagers are not taking you seriously, as you do not have enough soldiers to carry out the process. At least 20 men are needed to continue.", null);
				return MenuHelper.SetOptionProperties(args, flag, !flag, disabledText);
			}
			return false;
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000BFD5C File Offset: 0x000BDF5C
		private void collecting_menu_on_init(MenuCallbackArgs args)
		{
			if (RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage().CollectedAmount == 0)
			{
				TextObject textObject = new TextObject("{=VktwHCN6}Your men have started to collect the tax of {VILLAGE}", null);
				textObject.SetTextVariable("VILLAGE", Settlement.CurrentSettlement.Name);
				MBInformationManager.AddQuickInformation(textObject, 0, null, "");
			}
			RevenueFarmingIssueBehavior.Instance.CollectingRevenues = true;
			args.MenuContext.GameMenu.StartWait();
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000BFDC2 File Offset: 0x000BDFC2
		private bool leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000BFDCD File Offset: 0x000BDFCD
		private void leave_consequence(MenuCallbackArgs args)
		{
			RevenueFarmingIssueBehavior.Instance.CollectingRevenues = false;
			GameMenu.SwitchToMenu("village");
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000BFDE4 File Offset: 0x000BDFE4
		private void collection_menu_on_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(revenueVillage.CollectProgress);
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000BFE12 File Offset: 0x000BE012
		private void collect_revenue_menu_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("village_collect_revenue");
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x000BFE29 File Offset: 0x000BE029
		[GameMenuInitializationHandler("village_collect_revenue")]
		private static void village_collect_revenue_game_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppablePlay;
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000BFE50 File Offset: 0x000BE050
		[GameMenuInitializationHandler("offer_goods_and_troops")]
		[GameMenuInitializationHandler("brawl_breaks_out")]
		[GameMenuInitializationHandler("landlord_asks_for_money")]
		[GameMenuInitializationHandler("village_is_under_quarantine")]
		[GameMenuInitializationHandler("refuse_to_pay_what_they_owe")]
		[GameMenuInitializationHandler("relief_for_the_poorest")]
		private static void village_event_common_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.SettlementComponent.WaitMeshName);
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000BFE6C File Offset: 0x000BE06C
		private void ChangeRelationWithNotables(int relation)
		{
			foreach (Hero hero in Settlement.CurrentSettlement.Notables)
			{
				hero.SetHasMet();
				ChangeRelationAction.ApplyPlayerRelation(hero, relation, false, false);
			}
			TextObject textObject = TextObject.Empty;
			if (relation > 0)
			{
				textObject = new TextObject("{=IwS1qeq9}Your relation is increased by {MAGNITUDE} with village notables.", null);
			}
			else
			{
				textObject = new TextObject("{=r5Netxy0}Your relation is decreased by {MAGNITUDE} with village notables.", null);
			}
			textObject.SetTextVariable("MAGNITUDE", relation);
			MBInformationManager.AddQuickInformation(textObject, 0, null, "");
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000BFF08 File Offset: 0x000BE108
		private void CompleteCurrentRevenueCollection(bool addLog = true)
		{
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
			RevenueFarmingIssueBehavior.Instance.SetVillageAsCompleted(revenueVillage, addLog);
			if (RevenueFarmingIssueBehavior.Instance.IsTracked(revenueVillage.Village.Settlement))
			{
				RevenueFarmingIssueBehavior.Instance.RemoveTrackedObject(revenueVillage.Village.Settlement);
			}
			RevenueFarmingIssueBehavior.Instance.CollectingRevenues = false;
			PlayerEncounter.Finish(true);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000BFF6C File Offset: 0x000BE16C
		private void GiveVillageGoods(out int amount)
		{
			RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = RevenueFarmingIssueBehavior.Instance.FindCurrentRevenueVillage();
			amount = (int)((float)revenueVillage.TargetAmount * 0.5f / (float)revenueVillage.Village.VillageType.PrimaryProduction.Value);
			MobileParty.MainParty.ItemRoster.AddToCounts(revenueVillage.Village.VillageType.PrimaryProduction, amount);
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000BFFD0 File Offset: 0x000BE1D0
		public void OnVillageEventWithIdSpawned(string Id)
		{
			RevenueFarmingIssueBehavior.VillageEvent villageEvent = this._villageEvents.FirstOrDefault((RevenueFarmingIssueBehavior.VillageEvent x) => x.Id == Id);
			RevenueFarmingIssueBehavior.Instance.AddLog(villageEvent.MainLog, false);
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000C0014 File Offset: 0x000BE214
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000C0018 File Offset: 0x000BE218
		private bool ConditionsHold(Hero issueGiver, out Settlement targetSettlement)
		{
			targetSettlement = null;
			if (issueGiver.IsLord && issueGiver.Clan.Leader == issueGiver && issueGiver.GetTraitLevel(DefaultTraits.Mercy) <= 0 && issueGiver.Clan.Settlements.Count > 0)
			{
				targetSettlement = (from x in issueGiver.Clan.Settlements
				where x.IsTown
				select x).GetRandomElementInefficiently<Settlement>();
			}
			return targetSettlement != null;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000C009C File Offset: 0x000BE29C
		public void OnCheckForIssue(Hero hero)
		{
			Settlement relatedObject;
			if (this.ConditionsHold(hero, out relatedObject))
			{
				Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(new PotentialIssueData.StartIssueDelegate(this.OnSelected), typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssue), IssueBase.IssueFrequency.VeryCommon, relatedObject));
				return;
			}
			Campaign.Current.IssueManager.AddPotentialIssueData(hero, new PotentialIssueData(typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssue), IssueBase.IssueFrequency.VeryCommon));
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000C0104 File Offset: 0x000BE304
		private IssueBase OnSelected(in PotentialIssueData pid, Hero issueOwner)
		{
			PotentialIssueData potentialIssueData = pid;
			return new RevenueFarmingIssueBehavior.RevenueFarmingIssue(issueOwner, potentialIssueData.RelatedObject as Settlement);
		}

		// Token: 0x04000DAC RID: 3500
		private const int CollectAllVillageTaxesAfterHours = 10;

		// Token: 0x04000DAD RID: 3501
		private const IssueBase.IssueFrequency RevenueFarmingIssueFrequency = IssueBase.IssueFrequency.VeryCommon;

		// Token: 0x04000DAE RID: 3502
		private List<RevenueFarmingIssueBehavior.VillageEvent> _villageEvents;

		// Token: 0x04000DAF RID: 3503
		private RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest _cachedQuest;

		// Token: 0x02000666 RID: 1638
		public class RevenueFarmingIssue : IssueBase
		{
			// Token: 0x0600529F RID: 21151 RVA: 0x00176EC1 File Offset: 0x001750C1
			internal static void AutoGeneratedStaticCollectObjectsRevenueFarmingIssue(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.RevenueFarmingIssue)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060052A0 RID: 21152 RVA: 0x00176ECF File Offset: 0x001750CF
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._targetSettlement);
			}

			// Token: 0x060052A1 RID: 21153 RVA: 0x00176EE4 File Offset: 0x001750E4
			internal static object AutoGeneratedGetMemberValue_targetSettlement(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssue)o)._targetSettlement;
			}

			// Token: 0x17001182 RID: 4482
			// (get) Token: 0x060052A2 RID: 21154 RVA: 0x00176EF1 File Offset: 0x001750F1
			protected override int RewardGold
			{
				get
				{
					return 0;
				}
			}

			// Token: 0x17001183 RID: 4483
			// (get) Token: 0x060052A3 RID: 21155 RVA: 0x00176EF4 File Offset: 0x001750F4
			protected int TotalRequestedDenars
			{
				get
				{
					int num = 0;
					foreach (Village village in this._targetSettlement.BoundVillages)
					{
						if (!village.Settlement.IsRaided && !village.Settlement.IsUnderRaid)
						{
							num += (int)(village.Hearth * 4f);
						}
					}
					return num / 3;
				}
			}

			// Token: 0x17001184 RID: 4484
			// (get) Token: 0x060052A4 RID: 21156 RVA: 0x00176F74 File Offset: 0x00175174
			public override TextObject IssueBriefByIssueGiver
			{
				get
				{
					return new TextObject("{=j5fS9zaa}Yes, there is something. I have been on campaign for much of this year, and I have not been able to go around to my estates collecting the rents that are owed to me and the taxes that are owed to the realm. I need some help collecting these revenues.[ib:confident3][if:convo_nonchalant]", null);
				}
			}

			// Token: 0x17001185 RID: 4485
			// (get) Token: 0x060052A5 RID: 21157 RVA: 0x00176F81 File Offset: 0x00175181
			public override TextObject IssueAcceptByPlayer
			{
				get
				{
					return new TextObject("{=AXy26AFb}Maybe I can help. What are the terms of agreement.", null);
				}
			}

			// Token: 0x17001186 RID: 4486
			// (get) Token: 0x060052A6 RID: 21158 RVA: 0x00176F8E File Offset: 0x0017518E
			public override TextObject IssueQuestSolutionExplanationByIssueGiver
			{
				get
				{
					TextObject textObject = new TextObject("{=F540oIed}I can designate you as my official revenue farmer, and give you a list of everyone's holdings and how much they owe. All you need to do is visit all my villages and collect what you can. I don't expect you to be able to get every denar. Some of the people around here are genuinely hard - up, but they'll all try to get out of paying. Let's just keep it simple: I will take {TOTAL_REQUESTED_DENARS}{GOLD_ICON} denars and you can keep whatever else you can squeeze out of them. Are you interested?[if:convo_calm_friendly]", null);
					textObject.SetTextVariable("TOTAL_REQUESTED_DENARS", this.TotalRequestedDenars);
					return textObject;
				}
			}

			// Token: 0x17001187 RID: 4487
			// (get) Token: 0x060052A7 RID: 21159 RVA: 0x00176FAD File Offset: 0x001751AD
			public override TextObject IssueQuestSolutionAcceptByPlayer
			{
				get
				{
					return new TextObject("{=dAmK7rKG}All right. I will visit your villages and collect your rent.", null);
				}
			}

			// Token: 0x17001188 RID: 4488
			// (get) Token: 0x060052A8 RID: 21160 RVA: 0x00176FBA File Offset: 0x001751BA
			public override bool IsThereAlternativeSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17001189 RID: 4489
			// (get) Token: 0x060052A9 RID: 21161 RVA: 0x00176FBD File Offset: 0x001751BD
			public override bool IsThereLordSolution
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700118A RID: 4490
			// (get) Token: 0x060052AA RID: 21162 RVA: 0x00176FC0 File Offset: 0x001751C0
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=zqrn2beP}Revenue Farming", null);
				}
			}

			// Token: 0x1700118B RID: 4491
			// (get) Token: 0x060052AB RID: 21163 RVA: 0x00176FCD File Offset: 0x001751CD
			public override TextObject Description
			{
				get
				{
					TextObject result = new TextObject("{=U8izV2lM}A {?ISSUE_GIVER.GENDER}lady{?}lord{\\?} is looking for someone to collect back rents that {?ISSUE_GIVER.GENDER}she{?}he{\\?} says are owed to {?ISSUE_GIVER.GENDER}her{?}him{\\?}.", null);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER", base.IssueOwner.CharacterObject, null, false);
					return result;
				}
			}

			// Token: 0x060052AC RID: 21164 RVA: 0x00176FF2 File Offset: 0x001751F2
			public RevenueFarmingIssue(Hero issueOwner, Settlement targetSettlement) : base(issueOwner, CampaignTime.DaysFromNow(20f))
			{
				this._targetSettlement = targetSettlement;
			}

			// Token: 0x060052AD RID: 21165 RVA: 0x0017700C File Offset: 0x0017520C
			protected override float GetIssueEffectAmountInternal(IssueEffect issueEffect)
			{
				if (issueEffect == DefaultIssueEffects.ClanInfluence)
				{
					return -0.2f;
				}
				return 0f;
			}

			// Token: 0x060052AE RID: 21166 RVA: 0x00177021 File Offset: 0x00175221
			public override IssueBase.IssueFrequency GetFrequency()
			{
				return IssueBase.IssueFrequency.VeryCommon;
			}

			// Token: 0x060052AF RID: 21167 RVA: 0x00177024 File Offset: 0x00175224
			public override bool IssueStayAliveConditions()
			{
				if (this._targetSettlement.OwnerClan == base.IssueOwner.Clan)
				{
					return this._targetSettlement.BoundVillages.Any((Village x) => !x.Settlement.IsRaided && !x.Settlement.IsUnderRaid);
				}
				return false;
			}

			// Token: 0x060052B0 RID: 21168 RVA: 0x0017707C File Offset: 0x0017527C
			protected override bool CanPlayerTakeQuestConditions(Hero issueGiver, out IssueBase.PreconditionFlags flags, out Hero relationHero, out SkillObject skill)
			{
				flags = IssueBase.PreconditionFlags.None;
				relationHero = null;
				skill = null;
				if (issueGiver.GetRelationWithPlayer() < -10f)
				{
					flags |= IssueBase.PreconditionFlags.Relation;
					relationHero = issueGiver;
				}
				if (FactionManager.IsAtWarAgainstFaction(issueGiver.MapFaction, Hero.MainHero.MapFaction))
				{
					flags |= IssueBase.PreconditionFlags.AtWar;
				}
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < 40)
				{
					flags |= IssueBase.PreconditionFlags.NotEnoughTroops;
				}
				return flags == IssueBase.PreconditionFlags.None;
			}

			// Token: 0x060052B1 RID: 21169 RVA: 0x001770E9 File Offset: 0x001752E9
			protected override void OnGameLoad()
			{
			}

			// Token: 0x060052B2 RID: 21170 RVA: 0x001770EB File Offset: 0x001752EB
			protected override void HourlyTick()
			{
			}

			// Token: 0x060052B3 RID: 21171 RVA: 0x001770F0 File Offset: 0x001752F0
			protected override QuestBase GenerateIssueQuest(string questId)
			{
				List<RevenueFarmingIssueBehavior.RevenueVillage> list = new List<RevenueFarmingIssueBehavior.RevenueVillage>();
				foreach (Village village in this._targetSettlement.BoundVillages)
				{
					if (!village.Settlement.IsUnderRaid && !village.Settlement.IsRaided)
					{
						list.Add(new RevenueFarmingIssueBehavior.RevenueVillage(village, (int)(village.Hearth * 4f)));
					}
				}
				return new RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest(questId, base.IssueOwner, CampaignTime.DaysFromNow(20f), list);
			}

			// Token: 0x060052B4 RID: 21172 RVA: 0x00177194 File Offset: 0x00175394
			protected override void CompleteIssueWithTimedOutConsequences()
			{
			}

			// Token: 0x04001AE4 RID: 6884
			private const int IssueAndQuestDuration = 20;

			// Token: 0x04001AE5 RID: 6885
			private const int MinimumRequiredMenCount = 40;

			// Token: 0x04001AE6 RID: 6886
			[SaveableField(1)]
			private Settlement _targetSettlement;
		}

		// Token: 0x02000667 RID: 1639
		public class RevenueFarmingIssueQuest : QuestBase
		{
			// Token: 0x060052B5 RID: 21173 RVA: 0x00177196 File Offset: 0x00175396
			internal static void AutoGeneratedStaticCollectObjectsRevenueFarmingIssueQuest(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060052B6 RID: 21174 RVA: 0x001771A4 File Offset: 0x001753A4
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
				collectedObjects.Add(this._revenueVillages);
				collectedObjects.Add(this._currentVillageEvents);
				collectedObjects.Add(this._questProgressLog);
			}

			// Token: 0x060052B7 RID: 21175 RVA: 0x001771D1 File Offset: 0x001753D1
			internal static object AutoGeneratedGetMemberValue_totalRequestedDenars(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._totalRequestedDenars;
			}

			// Token: 0x060052B8 RID: 21176 RVA: 0x001771E3 File Offset: 0x001753E3
			internal static object AutoGeneratedGetMemberValueCollectingRevenues(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o).CollectingRevenues;
			}

			// Token: 0x060052B9 RID: 21177 RVA: 0x001771F5 File Offset: 0x001753F5
			internal static object AutoGeneratedGetMemberValue_allRevenuesAreCollected(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._allRevenuesAreCollected;
			}

			// Token: 0x060052BA RID: 21178 RVA: 0x00177207 File Offset: 0x00175407
			internal static object AutoGeneratedGetMemberValue_revenueVillages(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._revenueVillages;
			}

			// Token: 0x060052BB RID: 21179 RVA: 0x00177214 File Offset: 0x00175414
			internal static object AutoGeneratedGetMemberValue_currentVillageEvents(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._currentVillageEvents;
			}

			// Token: 0x060052BC RID: 21180 RVA: 0x00177221 File Offset: 0x00175421
			internal static object AutoGeneratedGetMemberValue_questProgressLog(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest)o)._questProgressLog;
			}

			// Token: 0x1700118C RID: 4492
			// (get) Token: 0x060052BD RID: 21181 RVA: 0x0017722E File Offset: 0x0017542E
			public override TextObject Title
			{
				get
				{
					return new TextObject("{=zqrn2beP}Revenue Farming", null);
				}
			}

			// Token: 0x1700118D RID: 4493
			// (get) Token: 0x060052BE RID: 21182 RVA: 0x0017723B File Offset: 0x0017543B
			public override bool IsRemainingTimeHidden
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700118E RID: 4494
			// (get) Token: 0x060052BF RID: 21183 RVA: 0x0017723E File Offset: 0x0017543E
			public List<RevenueFarmingIssueBehavior.RevenueVillage> RevenueVillages
			{
				get
				{
					return this._revenueVillages;
				}
			}

			// Token: 0x1700118F RID: 4495
			// (get) Token: 0x060052C0 RID: 21184 RVA: 0x00177246 File Offset: 0x00175446
			// (set) Token: 0x060052C1 RID: 21185 RVA: 0x0017724E File Offset: 0x0017544E
			public Settlement TargetSettlement { get; private set; }

			// Token: 0x17001190 RID: 4496
			// (get) Token: 0x060052C2 RID: 21186 RVA: 0x00177258 File Offset: 0x00175458
			private TextObject QuestStartedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=b0WQfzNb}{QUEST_GIVER.LINK} the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} of {TARGET_SETTLEMENT} told you that {?QUEST_GIVER.GENDER}she{?}he{\\?} wanted to grant revenue collection rights to a commander of good reputation who has enough men to suppress any resistance. {?QUEST_GIVER.GENDER}She{?}He{\\?} asked you to visit all the villages that are bound to {TARGET_SETTLEMENT} and collect taxes and rents. You have agreed to collect the revenues after paying {QUEST_GIVER.LINK}'s share, {REQUESTED_DENARS}{GOLD_ICON} denars.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this.TargetSettlement.EncyclopediaLinkWithName);
					textObject.SetTextVariable("REQUESTED_DENARS", this._totalRequestedDenars);
					return textObject;
				}
			}

			// Token: 0x17001191 RID: 4497
			// (get) Token: 0x060052C3 RID: 21187 RVA: 0x001772B4 File Offset: 0x001754B4
			private TextObject QuestCanceledWarDeclaredLog
			{
				get
				{
					TextObject textObject = new TextObject("{=vW6kBki9}Your clan is now at war with {QUEST_GIVER.LINK}'s realm. Your agreement with {QUEST_GIVER.LINK} is canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001192 RID: 4498
			// (get) Token: 0x060052C4 RID: 21188 RVA: 0x001772E8 File Offset: 0x001754E8
			private TextObject PlayerDeclaredWarQuestLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=bqeWVVEE}Your actions have started a war with {QUEST_GIVER.LINK}'s faction. {?QUEST_GIVER.GENDER}She{?}He{\\?} cancels your agreement and the quest is a failure.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001193 RID: 4499
			// (get) Token: 0x060052C5 RID: 21189 RVA: 0x0017731C File Offset: 0x0017551C
			private TextObject QuestSuccessLog
			{
				get
				{
					TextObject textObject = new TextObject("{=CEQhyvzj}You have completed the collection of revenues and paid {QUEST_GIVER.LINK} a fix sum in advance, as agreed.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001194 RID: 4500
			// (get) Token: 0x060052C6 RID: 21190 RVA: 0x00177350 File Offset: 0x00175550
			private TextObject QuestBetrayedLog
			{
				get
				{
					TextObject textObject = new TextObject("{=5ky3voFY}You have rejected handing over the revenue to the {QUEST_GIVER.LINK}. The {?QUEST_GIVER.GENDER}lady{?}lord{\\?} is deeply disappointed in you.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001195 RID: 4501
			// (get) Token: 0x060052C7 RID: 21191 RVA: 0x00177384 File Offset: 0x00175584
			private TextObject QuestFailedWithTimeOutLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=RNdrvZJQ}You have failed to bring the revenues to the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} in time.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					return textObject;
				}
			}

			// Token: 0x17001196 RID: 4502
			// (get) Token: 0x060052C8 RID: 21192 RVA: 0x001773B8 File Offset: 0x001755B8
			private TextObject AllRevenuesAreCollectedLogText
			{
				get
				{
					TextObject textObject = new TextObject("{=ywlzjQfN}{QUEST_GIVER.LINK} wants {TOTAL_REQUESTED_DENARS}{GOLD_ICON} that you have collected from {?QUEST_GIVER.GENDER}her{?}his{\\?} fiefs. You can either give the denars to the {?QUEST_GIVER.GENDER}lady{?}lord{\\?} yourself, or hand them over to a steward of the {?QUEST_GIVER.GENDER}lady{?}lord{\\?}, which can be found in the castles and towns that belong to the {?QUEST_GIVER.GENDER}lady{?}lord{\\?}.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("TOTAL_REQUESTED_DENARS", this._totalRequestedDenars);
					return textObject;
				}
			}

			// Token: 0x060052C9 RID: 21193 RVA: 0x001773FC File Offset: 0x001755FC
			public RevenueFarmingIssueQuest(string questId, Hero giverHero, CampaignTime duration, List<RevenueFarmingIssueBehavior.RevenueVillage> revenueVillages) : base(questId, giverHero, duration, 0)
			{
				this._revenueVillages = revenueVillages;
				this.TargetSettlement = this._revenueVillages[0].Village.Bound;
				foreach (RevenueFarmingIssueBehavior.VillageEvent villageEvent in Campaign.Current.GetCampaignBehavior<RevenueFarmingIssueBehavior>()._villageEvents)
				{
					this._currentVillageEvents.Add(villageEvent.Id, false);
				}
				foreach (RevenueFarmingIssueBehavior.RevenueVillage revenueVillage in revenueVillages)
				{
					this._totalRequestedDenars += revenueVillage.TargetAmount / 3;
				}
				this.SetDialogs();
				base.InitializeQuestOnCreation();
			}

			// Token: 0x060052CA RID: 21194 RVA: 0x001774F4 File Offset: 0x001756F4
			private void QuestAcceptedConsequences()
			{
				base.StartQuest();
				this._questProgressLog = base.AddDiscreteLog(this.QuestStartedLogText, new TextObject("{=bC5aMfG0}Villages", null), 0, this._revenueVillages.Count, null, true);
				foreach (RevenueFarmingIssueBehavior.RevenueVillage revenueVillage in this._revenueVillages)
				{
					base.AddTrackedObject(revenueVillage.Village.Settlement);
				}
			}

			// Token: 0x060052CB RID: 21195 RVA: 0x00177584 File Offset: 0x00175784
			protected override void SetDialogs()
			{
				this.OfferDialogFlow = DialogFlow.CreateDialogFlow("issue_classic_quest_start", 100).NpcLine(new TextObject("{=PXigJyMs}Excellent. You are acting in my name now. Try to be polite but you have every right to use force if they don't cough up what's owed. Good luck.[ib:confident2][if:convo_bored2]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).Consequence(new ConversationSentence.OnConsequenceDelegate(this.QuestAcceptedConsequences)).CloseDialog();
				this.DiscussDialogFlow = DialogFlow.CreateDialogFlow("quest_discuss", 100).NpcLine(new TextObject("{=tthBNejU}Have you collected the revenues?[if:convo_undecided_open]", null), null, null).Condition(() => Hero.OneToOneConversationHero == base.QuestGiver).BeginPlayerOptions().PlayerOption(new TextObject("{=jQsr4vDO}I'm still working on it.", null), null).NpcLine(new TextObject("{=BI1UnHaB}Good, good. This takes time, I know, but don't keep me waiting too long.[if:convo_mocking_aristocratic]", null), null, null).Consequence(new ConversationSentence.OnConsequenceDelegate(MapEventHelper.OnConversationEnd)).CloseDialog().PlayerOption(new TextObject("{=ORl6qiOj}Yes, here is your share.", null), null).Condition(() => this._allRevenuesAreCollected).ClickableCondition(new ConversationSentence.OnClickableConditionDelegate(this.TurnQuestInClickableCondition)).NpcLine(new TextObject("{=MKYzHFKB}Thank you for your help.[if:convo_delighted]", null), null, null).Consequence(delegate
				{
					this.QuestCompletedWithSuccess();
					MapEventHelper.OnConversationEnd();
				}).CloseDialog().PlayerOption(new TextObject("{=kj3WQY1V}Maybe I should keep this to myself.", null), null).Condition(() => this._allRevenuesAreCollected).NpcLine(new TextObject("{=82aiVoV9}You will regret this in the long run...[ib:closed2][if:convo_angry]", null), null, null).Consequence(delegate
				{
					this.QuestCompletedWithBetray();
					MapEventHelper.OnConversationEnd();
				}).CloseDialog().PlayerOption(new TextObject("{=G5tyQj6N}Not yet.", null), null).NpcLine(new TextObject("{=UXCjNTjF}Hurry up. I don't have that much time.[if:convo_annoyed]", null), null, null).Consequence(new ConversationSentence.OnConsequenceDelegate(MapEventHelper.OnConversationEnd)).CloseDialog().EndPlayerOptions();
			}

			// Token: 0x060052CC RID: 21196 RVA: 0x00177732 File Offset: 0x00175932
			private bool TurnQuestInClickableCondition(out TextObject explanation)
			{
				explanation = TextObject.Empty;
				if (Hero.MainHero.Gold < RevenueFarmingIssueBehavior.Instance._totalRequestedDenars)
				{
					explanation = new TextObject("{=QOWyEJrm}You don't have enough denars.", null);
					return false;
				}
				return true;
			}

			// Token: 0x060052CD RID: 21197 RVA: 0x00177764 File Offset: 0x00175964
			protected override void OnBeforeTimedOut(ref bool completeWithSuccess, ref bool doNotResolveTheQuest)
			{
				this.RelationshipChangeWithQuestGiver = -5;
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -30)
				});
				if (Hero.MainHero.Gold >= this._totalRequestedDenars)
				{
					this.ShowQuestResolvePopUp();
					doNotResolveTheQuest = true;
				}
			}

			// Token: 0x060052CE RID: 21198 RVA: 0x001777B4 File Offset: 0x001759B4
			protected override void OnTimedOut()
			{
				base.AddLog(this.QuestFailedWithTimeOutLogText, false);
			}

			// Token: 0x060052CF RID: 21199 RVA: 0x001777C4 File Offset: 0x001759C4
			protected override void RegisterEvents()
			{
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.VillageBeingRaided.AddNonSerializedListener(this, new Action<Village>(this.OnVillageRaid));
				CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
				CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			}

			// Token: 0x060052D0 RID: 21200 RVA: 0x00177844 File Offset: 0x00175A44
			private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
			{
				if (settlement == this.TargetSettlement && oldOwner == base.QuestGiver)
				{
					TextObject textObject = new TextObject("{=1m68Nsze}{QUEST_GIVER.LINK} has lost {SETTLEMENT} and your agreement with {?QUEST_GIVER.GENDER}her{?}him{\\?} has been canceled.", null);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					textObject.SetTextVariable("SETTLEMENT", this.TargetSettlement.EncyclopediaLinkWithName);
					base.CompleteQuestWithCancel(textObject);
				}
			}

			// Token: 0x060052D1 RID: 21201 RVA: 0x001778A6 File Offset: 0x00175AA6
			private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
			{
				if (QuestHelper.CheckMinorMajorCoercion(this, mapEvent, attackerParty))
				{
					QuestHelper.ApplyGenericMinorMajorCoercionConsequences(this, mapEvent);
				}
			}

			// Token: 0x060052D2 RID: 21202 RVA: 0x001778BC File Offset: 0x00175ABC
			private void OnVillageRaid(Village village)
			{
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = this._revenueVillages.FirstOrDefault((RevenueFarmingIssueBehavior.RevenueVillage x) => x.Village.Id == village.Id);
				if (revenueVillage != null && !revenueVillage.IsRaided)
				{
					TextObject textObject = new TextObject("{=k8U0928J}{VILLAGE} has been raided. {QUEST_GIVER.LINK} asks you to exempt them, but still wants you to collect {AMOUNT}{GOLD_ICON} denars from rest of {?QUEST_GIVER.GENDER}her{?}his{\\?} villages.", null);
					textObject.SetTextVariable("VILLAGE", village.Settlement.EncyclopediaLinkWithName);
					StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
					this._totalRequestedDenars -= revenueVillage.TargetAmount / 3;
					textObject.SetTextVariable("AMOUNT", this._totalRequestedDenars);
					revenueVillage.SetDone();
					revenueVillage.IsRaided = true;
					base.AddLog(textObject, false);
					this._questProgressLog.UpdateCurrentProgress(this._questProgressLog.CurrentProgress + 1);
					if (this.CollectingRevenues)
					{
						this.CollectingRevenues = false;
					}
					if (this._revenueVillages.All((RevenueFarmingIssueBehavior.RevenueVillage x) => x.IsRaided))
					{
						TextObject cancelLog = new TextObject("{=44f1ff0q}All the villages of {QUEST_GIVER.LINK} has been raided and your agreement with {?QUEST_GIVER.GENDER}her{?}him{\\?} has been canceled.", null);
						base.CompleteQuestWithCancel(cancelLog);
					}
				}
			}

			// Token: 0x060052D3 RID: 21203 RVA: 0x001779E4 File Offset: 0x00175BE4
			protected override void HourlyTick()
			{
				if (base.IsOngoing)
				{
					if (!this._allRevenuesAreCollected)
					{
						if (this._revenueVillages.All((RevenueFarmingIssueBehavior.RevenueVillage x) => x.GetIsCompleted()))
						{
							this.OnAllRevenuesAreCollected();
						}
					}
					if (this.CollectingRevenues)
					{
						this.ProgressRevenueCollectionForVillage();
					}
				}
			}

			// Token: 0x060052D4 RID: 21204 RVA: 0x00177A41 File Offset: 0x00175C41
			private void OnAllRevenuesAreCollected()
			{
				this._allRevenuesAreCollected = true;
				base.AddLog(this.AllRevenuesAreCollectedLogText, false);
			}

			// Token: 0x060052D5 RID: 21205 RVA: 0x00177A58 File Offset: 0x00175C58
			public void RevenuesAreDeliveredToSteward()
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=RCa0DpAo}You have handed over the revenue to the steward", null), 0, null, "");
				this.QuestCompletedWithSuccess();
			}

			// Token: 0x060052D6 RID: 21206 RVA: 0x00177A78 File Offset: 0x00175C78
			private void ShowQuestResolvePopUp()
			{
				TextObject textObject = new TextObject("{=I9GYdYZx}{?QUEST_GIVER.GENDER}Lady{?}Lord{\\?} {QUEST_GIVER.NAME} wants {TOTAL_REQUESTED_DENARS}{GOLD_ICON} denars that you have collected from {?QUEST_GIVER.GENDER}her{?}his{\\?} fiefs. {?QUEST_GIVER.GENDER}She{?}He{\\?} has sent {?QUEST_GIVER.GENDER}her{?}his{\\?} steward to you to collect it. If you refuse this will be counted as a crime and {?QUEST_GIVER.GENDER}her{?}his{\\?} faction may declare war on you.", null);
				StringHelpers.SetCharacterProperties("QUEST_GIVER", base.QuestGiver.CharacterObject, textObject, false);
				textObject.SetTextVariable("TOTAL_REQUESTED_DENARS", this._totalRequestedDenars);
				InformationManager.ShowInquiry(new InquiryData(this.Title.ToString(), textObject.ToString(), true, true, new TextObject("{=plZVwdlL}Send the revenue", null).ToString(), new TextObject("{=asa9HaIQ}Keep the revenue", null).ToString(), new Action(this.QuestCompletedWithSuccess), new Action(this.QuestCompletedWithBetray), "", 0f, null, null, null), false, false);
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				if (this.CollectingRevenues)
				{
					this.CollectingRevenues = false;
				}
			}

			// Token: 0x060052D7 RID: 21207 RVA: 0x00177B3C File Offset: 0x00175D3C
			private void QuestCompletedWithSuccess()
			{
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, this._totalRequestedDenars, false);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, 30)
				});
				this.RelationshipChangeWithQuestGiver = 5;
				base.AddLog(this.QuestSuccessLog, false);
				base.CompleteQuestWithSuccess();
			}

			// Token: 0x060052D8 RID: 21208 RVA: 0x00177B98 File Offset: 0x00175D98
			private void QuestCompletedWithBetray()
			{
				ChangeCrimeRatingAction.Apply(base.QuestGiver.MapFaction, 45f, true);
				TraitLevelingHelper.OnIssueSolvedThroughQuest(base.QuestGiver, new Tuple<TraitObject, int>[]
				{
					new Tuple<TraitObject, int>(DefaultTraits.Honor, -100)
				});
				this.RelationshipChangeWithQuestGiver = -15;
				base.AddLog(this.QuestBetrayedLog, false);
				base.CompleteQuestWithBetrayal(null);
			}

			// Token: 0x060052D9 RID: 21209 RVA: 0x00177BF8 File Offset: 0x00175DF8
			private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
			{
				if (base.QuestGiver.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
				{
					base.CompleteQuestWithCancel(this.QuestCanceledWarDeclaredLog);
				}
			}

			// Token: 0x060052DA RID: 21210 RVA: 0x00177C22 File Offset: 0x00175E22
			private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
			{
				QuestHelper.CheckWarDeclarationAndFailOrCancelTheQuest(this, faction1, faction2, detail, this.PlayerDeclaredWarQuestLogText, this.QuestCanceledWarDeclaredLog, false);
			}

			// Token: 0x060052DB RID: 21211 RVA: 0x00177C3C File Offset: 0x00175E3C
			protected override void OnFinalize()
			{
				if (Campaign.Current.CurrentMenuContext != null)
				{
					if (this._currentVillageEvents.Any((KeyValuePair<string, bool> x) => x.Key == Campaign.Current.CurrentMenuContext.GameMenu.StringId) || Campaign.Current.CurrentMenuContext.GameMenu.StringId == "village_collect_revenue")
					{
						if (Game.Current.GameStateManager.ActiveState is MapState)
						{
							PlayerEncounter.Finish(true);
							return;
						}
						GameMenu.SwitchToMenu("village_outside");
					}
				}
			}

			// Token: 0x060052DC RID: 21212 RVA: 0x00177CC8 File Offset: 0x00175EC8
			protected override void InitializeQuestOnGameLoad()
			{
				this.TargetSettlement = this._revenueVillages[0].Village.Bound;
				this.SetDialogs();
			}

			// Token: 0x060052DD RID: 21213 RVA: 0x00177CEC File Offset: 0x00175EEC
			public RevenueFarmingIssueBehavior.RevenueVillage FindCurrentRevenueVillage()
			{
				foreach (RevenueFarmingIssueBehavior.RevenueVillage revenueVillage in this._revenueVillages)
				{
					if (revenueVillage.Village.Id == Settlement.CurrentSettlement.Village.Id)
					{
						return revenueVillage;
					}
				}
				return null;
			}

			// Token: 0x060052DE RID: 21214 RVA: 0x00177D60 File Offset: 0x00175F60
			private void ProgressRevenueCollectionForVillage()
			{
				RevenueFarmingIssueBehavior.RevenueVillage revenueVillage = this.FindCurrentRevenueVillage();
				if (!revenueVillage.EventOccurred && revenueVillage.CollectProgress >= 0.3f)
				{
					RevenueFarmingIssueBehavior behavior = Campaign.Current.GetCampaignBehavior<RevenueFarmingIssueBehavior>();
					KeyValuePair<string, bool> randomElementInefficiently = (from x in this._currentVillageEvents
					where !x.Value && behavior._villageEvents.Any((RevenueFarmingIssueBehavior.VillageEvent y) => y.Id == x.Key)
					select x).GetRandomElementInefficiently<KeyValuePair<string, bool>>();
					this._currentVillageEvents[randomElementInefficiently.Key] = true;
					behavior.OnVillageEventWithIdSpawned(randomElementInefficiently.Key);
					revenueVillage.EventOccurred = true;
					GameMenu.SwitchToMenu(randomElementInefficiently.Key);
					Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
					return;
				}
				revenueVillage.CollectedAmount += revenueVillage.HourlyGain;
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, revenueVillage.HourlyGain, false);
				if (revenueVillage.GetIsCompleted())
				{
					this.SetVillageAsCompleted(revenueVillage, true);
				}
			}

			// Token: 0x060052DF RID: 21215 RVA: 0x00177E38 File Offset: 0x00176038
			public void SetVillageAsCompleted(RevenueFarmingIssueBehavior.RevenueVillage village, bool addLog = true)
			{
				this.CollectingRevenues = false;
				village.SetDone();
				base.RemoveTrackedObject(village.Village.Settlement);
				GameMenu.SwitchToMenu("village");
				this._questProgressLog.UpdateCurrentProgress(this._questProgressLog.CurrentProgress + 1);
				if (addLog)
				{
					TextObject textObject = new TextObject("{=mQqN8Fg0}Your men have collected {TOTAL_COLLECTED_FROM_VILLAGE}{GOLD_ICON} denars and completed the revenue collection from {VILLAGE}.", null);
					textObject.SetTextVariable("TOTAL_COLLECTED_FROM_VILLAGE", village.CollectedAmount);
					textObject.SetTextVariable("VILLAGE", village.Village.Settlement.EncyclopediaLinkWithName);
					base.AddLog(textObject, false);
				}
				if (!this._allRevenuesAreCollected)
				{
					if (this._revenueVillages.All((RevenueFarmingIssueBehavior.RevenueVillage x) => x.GetIsCompleted()))
					{
						this.OnAllRevenuesAreCollected();
					}
				}
			}

			// Token: 0x04001AE7 RID: 6887
			[SaveableField(10)]
			internal int _totalRequestedDenars;

			// Token: 0x04001AE8 RID: 6888
			[SaveableField(20)]
			private readonly List<RevenueFarmingIssueBehavior.RevenueVillage> _revenueVillages;

			// Token: 0x04001AEA RID: 6890
			[SaveableField(30)]
			public bool CollectingRevenues;

			// Token: 0x04001AEB RID: 6891
			[SaveableField(40)]
			private readonly Dictionary<string, bool> _currentVillageEvents = new Dictionary<string, bool>();

			// Token: 0x04001AEC RID: 6892
			[SaveableField(50)]
			internal bool _allRevenuesAreCollected;

			// Token: 0x04001AED RID: 6893
			[SaveableField(60)]
			private JournalLog _questProgressLog;
		}

		// Token: 0x02000668 RID: 1640
		public class VillageEvent
		{
			// Token: 0x060052E6 RID: 21222 RVA: 0x00177F4D File Offset: 0x0017614D
			internal static void AutoGeneratedStaticCollectObjectsVillageEvent(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.VillageEvent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060052E7 RID: 21223 RVA: 0x00177F5B File Offset: 0x0017615B
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
			}

			// Token: 0x060052E8 RID: 21224 RVA: 0x00177F5D File Offset: 0x0017615D
			public VillageEvent(string id, string mainEventText, TextObject mainLog, List<RevenueFarmingIssueBehavior.VillageEventOptionData> optionConditionsAndConsequences)
			{
				this.Id = id;
				this.MainEventText = mainEventText;
				this.MainLog = mainLog;
				this.OptionConditionsAndConsequences = optionConditionsAndConsequences;
			}

			// Token: 0x04001AEE RID: 6894
			public readonly string Id;

			// Token: 0x04001AEF RID: 6895
			public readonly string MainEventText;

			// Token: 0x04001AF0 RID: 6896
			public TextObject MainLog;

			// Token: 0x04001AF1 RID: 6897
			public List<RevenueFarmingIssueBehavior.VillageEventOptionData> OptionConditionsAndConsequences;
		}

		// Token: 0x02000669 RID: 1641
		public class RevenueVillage
		{
			// Token: 0x060052E9 RID: 21225 RVA: 0x00177F82 File Offset: 0x00176182
			internal static void AutoGeneratedStaticCollectObjectsRevenueVillage(object o, List<object> collectedObjects)
			{
				((RevenueFarmingIssueBehavior.RevenueVillage)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x060052EA RID: 21226 RVA: 0x00177F90 File Offset: 0x00176190
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Village);
			}

			// Token: 0x060052EB RID: 21227 RVA: 0x00177F9E File Offset: 0x0017619E
			internal static object AutoGeneratedGetMemberValueVillage(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).Village;
			}

			// Token: 0x060052EC RID: 21228 RVA: 0x00177FAB File Offset: 0x001761AB
			internal static object AutoGeneratedGetMemberValueTargetAmount(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).TargetAmount;
			}

			// Token: 0x060052ED RID: 21229 RVA: 0x00177FBD File Offset: 0x001761BD
			internal static object AutoGeneratedGetMemberValueCollectedAmount(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).CollectedAmount;
			}

			// Token: 0x060052EE RID: 21230 RVA: 0x00177FCF File Offset: 0x001761CF
			internal static object AutoGeneratedGetMemberValueHourlyGain(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).HourlyGain;
			}

			// Token: 0x060052EF RID: 21231 RVA: 0x00177FE1 File Offset: 0x001761E1
			internal static object AutoGeneratedGetMemberValueEventOccurred(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).EventOccurred;
			}

			// Token: 0x060052F0 RID: 21232 RVA: 0x00177FF3 File Offset: 0x001761F3
			internal static object AutoGeneratedGetMemberValueIsRaided(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o).IsRaided;
			}

			// Token: 0x060052F1 RID: 21233 RVA: 0x00178005 File Offset: 0x00176205
			internal static object AutoGeneratedGetMemberValue_isDone(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o)._isDone;
			}

			// Token: 0x060052F2 RID: 21234 RVA: 0x00178017 File Offset: 0x00176217
			internal static object AutoGeneratedGetMemberValue_customProgress(object o)
			{
				return ((RevenueFarmingIssueBehavior.RevenueVillage)o)._customProgress;
			}

			// Token: 0x17001197 RID: 4503
			// (get) Token: 0x060052F3 RID: 21235 RVA: 0x00178029 File Offset: 0x00176229
			public float CollectProgress
			{
				get
				{
					return ((this.CollectedAmount == 0) ? 0f : ((float)this.CollectedAmount / (float)this.TargetAmount)) + this._customProgress;
				}
			}

			// Token: 0x060052F4 RID: 21236 RVA: 0x00178050 File Offset: 0x00176250
			public void SetDone()
			{
				this._isDone = true;
			}

			// Token: 0x060052F5 RID: 21237 RVA: 0x0017805C File Offset: 0x0017625C
			public RevenueVillage(Village village, int targetAmount)
			{
				this.Village = village;
				this.TargetAmount = targetAmount;
				this.CollectedAmount = 0;
				this.HourlyGain = targetAmount / 10;
				this._isDone = false;
				this.EventOccurred = false;
				this.IsRaided = false;
				this._customProgress = 0f;
			}

			// Token: 0x060052F6 RID: 21238 RVA: 0x001780AE File Offset: 0x001762AE
			public void SetAdditionalProgress(float progress)
			{
				this._customProgress = progress;
			}

			// Token: 0x060052F7 RID: 21239 RVA: 0x001780B7 File Offset: 0x001762B7
			public bool GetIsCompleted()
			{
				return this._isDone || this.CollectProgress >= 1f || this.CollectedAmount >= this.TargetAmount;
			}

			// Token: 0x04001AF2 RID: 6898
			[SaveableField(1)]
			public readonly Village Village;

			// Token: 0x04001AF3 RID: 6899
			[SaveableField(2)]
			public readonly int TargetAmount;

			// Token: 0x04001AF4 RID: 6900
			[SaveableField(3)]
			public int CollectedAmount;

			// Token: 0x04001AF5 RID: 6901
			[SaveableField(4)]
			public int HourlyGain;

			// Token: 0x04001AF6 RID: 6902
			[SaveableField(5)]
			private bool _isDone;

			// Token: 0x04001AF7 RID: 6903
			[SaveableField(6)]
			public bool EventOccurred;

			// Token: 0x04001AF8 RID: 6904
			[SaveableField(7)]
			public bool IsRaided;

			// Token: 0x04001AF9 RID: 6905
			[SaveableField(8)]
			private float _customProgress;
		}

		// Token: 0x0200066A RID: 1642
		public class RevenueFarmingIssueBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060052F8 RID: 21240 RVA: 0x001780E1 File Offset: 0x001762E1
			public RevenueFarmingIssueBehaviorTypeDefiner() : base(850000)
			{
			}

			// Token: 0x060052F9 RID: 21241 RVA: 0x001780F0 File Offset: 0x001762F0
			protected override void DefineClassTypes()
			{
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssue), 1, null);
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.RevenueFarmingIssueQuest), 2, null);
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.VillageEvent), 3, null);
				base.AddClassDefinition(typeof(RevenueFarmingIssueBehavior.RevenueVillage), 4, null);
			}

			// Token: 0x060052FA RID: 21242 RVA: 0x00178145 File Offset: 0x00176345
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(List<RevenueFarmingIssueBehavior.RevenueVillage>));
				base.ConstructContainerDefinition(typeof(List<RevenueFarmingIssueBehavior.VillageEvent>));
				base.ConstructContainerDefinition(typeof(Dictionary<string, bool>));
			}
		}

		// Token: 0x0200066B RID: 1643
		public struct VillageEventOptionData
		{
			// Token: 0x060052FB RID: 21243 RVA: 0x00178177 File Offset: 0x00176377
			public VillageEventOptionData(string text, GameMenuOption.OnConditionDelegate onCondition, GameMenuOption.OnConsequenceDelegate onConsequence, bool isLeave = false)
			{
				this.Text = text;
				this.OnCondition = onCondition;
				this.OnConsequence = onConsequence;
				this.IsLeave = isLeave;
			}

			// Token: 0x04001AFA RID: 6906
			public readonly string Text;

			// Token: 0x04001AFB RID: 6907
			public readonly GameMenuOption.OnConditionDelegate OnCondition;

			// Token: 0x04001AFC RID: 6908
			public readonly GameMenuOption.OnConsequenceDelegate OnConsequence;

			// Token: 0x04001AFD RID: 6909
			public readonly bool IsLeave;
		}
	}
}
