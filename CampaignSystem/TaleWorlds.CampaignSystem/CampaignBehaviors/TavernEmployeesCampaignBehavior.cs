﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D8 RID: 984
	public class TavernEmployeesCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003CCF RID: 15567 RVA: 0x00127CD4 File Offset: 0x00125ED4
		public override void RegisterEvents()
		{
			CampaignEvents.OnMissionStartedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionStarted));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x00127D54 File Offset: 0x00125F54
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Settlement>("_orderedDrinkThisDayInSettlement", ref this._orderedDrinkThisDayInSettlement);
			dataStore.SyncData<bool>("_orderedDrinkThisVisit", ref this._orderedDrinkThisVisit);
			dataStore.SyncData<bool>("_hasMetWithRansomBroker", ref this._hasMetWithRansomBroker);
			dataStore.SyncData<bool>("_hasBoughtTunToParty", ref this._hasBoughtTunToParty);
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x00127DA9 File Offset: 0x00125FA9
		public void DailyTick()
		{
			this._orderedDrinkThisDayInSettlement = null;
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x00127DB2 File Offset: 0x00125FB2
		public void WeeklyTick()
		{
			this._hasBoughtTunToParty = false;
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x00127DBB File Offset: 0x00125FBB
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
			this._inquiryVariationIndex = MBRandom.NondeterministicRandomInt % 6;
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x00127DD4 File Offset: 0x00125FD4
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			if (settlement.IsTown && CampaignMission.Current != null)
			{
				Location location = CampaignMission.Current.Location;
				if (location != null && location.StringId == "tavern")
				{
					int num;
					if (unusedUsablePointCount.TryGetValue("spawnpoint_tavernkeeper", out num) && num > 0)
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernkeeper), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("sp_tavern_wench", out num) && num > 0)
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernWench), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("musician", out num) && num > 0)
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateMusician), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, num);
					}
					location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateRansomBroker), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					return;
				}
				if (location != null && location.StringId == "center" && !Campaign.Current.IsNight)
				{
					int count;
					if (unusedUsablePointCount.TryGetValue("spawnpoint_tavernkeeper", out count))
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernkeeper), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("sp_tavern_wench", out count))
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateTavernWench), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, 1);
					}
					if (unusedUsablePointCount.TryGetValue("musician", out count))
					{
						location.AddLocationCharacters(new CreateLocationCharacterDelegate(TavernEmployeesCampaignBehavior.CreateMusician), settlement.Culture, LocationCharacter.CharacterRelations.Neutral, count);
					}
				}
			}
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x00127F6B File Offset: 0x0012616B
		public void OnMissionStarted(IMission mission)
		{
			this._orderedDrinkThisVisit = false;
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x00127F74 File Offset: 0x00126174
		private static LocationCharacter CreateTavernWench(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject tavernWench = culture.TavernWench;
			int minValue;
			int maxValue;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(tavernWench, out minValue, out maxValue, "");
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(tavernWench.Race, "_settlement");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(tavernWench, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(minValue, maxValue));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "sp_tavern_wench", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_barmaid"), true, false, null, false, false, true)
			{
				PrefabNamesForBones = 
				{
					{
						agentData.AgentMonster.OffHandItemBoneIndex,
						"kitchen_pitcher_b_tavern"
					}
				}
			};
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x00128040 File Offset: 0x00126240
		private static LocationCharacter CreateTavernkeeper(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject tavernkeeper = culture.Tavernkeeper;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(tavernkeeper.Race, "_settlement");
			int minValue;
			int maxValue;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(tavernkeeper, out minValue, out maxValue, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(tavernkeeper, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(minValue, maxValue));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "spawnpoint_tavernkeeper", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_tavern_keeper"), true, false, null, false, false, true);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x001280F0 File Offset: 0x001262F0
		private static LocationCharacter CreateMusician(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject musician = culture.Musician;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(musician.Race, "_settlement");
			int minValue;
			int maxValue;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(musician, out minValue, out maxValue, "");
			AgentData agentData = new AgentData(new SimpleAgentOrigin(musician, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(minValue, maxValue));
			return new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), "musician", true, relation, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_musician"), true, false, null, false, false, true);
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x001281A0 File Offset: 0x001263A0
		private static LocationCharacter CreateRansomBroker(CultureObject culture, LocationCharacter.CharacterRelations relation)
		{
			CharacterObject ransomBroker = culture.RansomBroker;
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(ransomBroker.Race, "_settlement");
			int minValue;
			int maxValue;
			Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(ransomBroker, out minValue, out maxValue, "");
			return new LocationCharacter(new AgentData(new SimpleAgentOrigin(ransomBroker, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).Age(MBRandom.RandomInt(minValue, maxValue)), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddOutdoorWandererBehaviors), "npc_common", true, relation, null, true, false, null, false, false, true);
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x00128238 File Offset: 0x00126438
		protected void AddDialogs(CampaignGameStarter cgs)
		{
			cgs.AddDialogLine("talk_common_to_tavernkeeper", "start", "tavernkeeper_talk", "{=QCuxL92I}Good day, {?PLAYER.GENDER}madam{?}sir{\\?}. How can I help you?", () => CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Tavernkeeper, null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_talk_to_get_quest", "tavernkeeper_talk", "tavernkeeper_ask_quests", "{=A61ppTa6}Do you know of anyone who might have a task for someone like me?", null, null, 100, null, null);
			cgs.AddPlayerLine("tavernkeeper_get_clan_info_start", "tavernkeeper_talk", "tavernkeeper_offer_clan_info", "{=shXdvd5p}I'm looking for information about the owner of this town.", null, null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_get_clan_info_answer", "tavernkeeper_offer_clan_info", "player_offer_clan_info", "{=i96KTeph}I can sell you information about {OWNER_CLAN}, who are the owners of our town {SETTLEMENT} for {PRICE}{GOLD_ICON}.", new ConversationSentence.OnConditionDelegate(this.tavernkeeper_offer_clan_info_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_get_clan_info_player_answer_1", "player_offer_clan_info", "tavernkeeper_pretalk", "{=VaxbQby7}That sounds like a great deal.", null, new ConversationSentence.OnConsequenceDelegate(this.player_accepts_clan_info_offer_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.player_accepts_clan_info_offer_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_get_clan_info_player_answer_2", "player_offer_clan_info", "tavernkeeper_pretalk", "{=CH7b5LaX}I have changed my mind.", null, null, 100, null, null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_start", "tavernkeeper_talk", "tavernkeeper_companion_info_tavernkeeper_answer", "{=e9xd15Db}I am looking for some people to hire with specific skills. Would you happen to know anyone looking for work in the towns of the {FACTION_INFORMAL_NAME}? ", new ConversationSentence.OnConditionDelegate(this.tavernkeeper_talk_companion_on_condition), null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_answer", "tavernkeeper_companion_info_tavernkeeper_answer", "tavernkeeper_list_companion_types", "{=ASVkuYHG}I know a few. What kind of a person are you looking for?", null, null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_scout", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=joCSXAQQ}I would travel much faster if I had a good scout by my side.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Scout);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_engineer", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=NGcKLV88}A good engineer could help construct siege engines and new buildings in towns.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Engineer);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_surgeon", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=Y5ztM8zq}My men would feel safer with a good surgeon to aid them in their time of need.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Surgeon);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_quartermaster", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=88Fk9keT}I am sure I can do more with fewer supplies if I had a good quartermaster.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Quartermaster);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_caravan_leader", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=kePH44eg}I am planning to sponsor my own caravans, and someone who could run them would be perfect.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.CaravanLeader);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_leader", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=9z0Yz8za}I need a lieutenant to be my right hand and help me direct my troops in battle.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Leader);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_select_roguery", "tavernkeeper_list_companion_types", "player_selected_companion_type", "{=DMUsPekF}I need someone who knows a bit about the darker side of the world, who can handle villains and thieves.", null, delegate
			{
				this.FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Roguery);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_player_nevermind", "tavernkeeper_list_companion_types", "tavernkeeper_pretalk", "{=tdvnKIyS}Never mind.", null, null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_not_found_answer_1", "player_selected_companion_type", "tavernkeeper_talk_companion_end", "{=!}{CANNOT_THINK_OF_ANYONE_LINE}[rb:negative][rb:unsure]", () => !this.FoundCompanion(), new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddPlayerLine("tavernkeeper_companion_info_tavernkeeper_end", "tavernkeeper_talk_companion_end", "start", "{=PDf52VCf}Thank you for your time anyway.", null, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_inside_1", "player_selected_companion_type", "player_companion_response", "{=QdEGu0CW}A {?INQUIRY_COMPANION.GENDER}woman{?}man{\\?} called {INQUIRY_COMPANION.LINK} has passed through here on his way to {COMPANION_SETTLEMENT}. You may catch up to {?INQUIRY_COMPANION.GENDER}her{?}him{\\?} if you hurry.[rb:positive]", () => this._inquiryCurrentCompanion.CurrentSettlement != Settlement.CurrentSettlement && this._inquiryVariationIndex % 3 == 0, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_inside_2", "player_selected_companion_type", "player_companion_response", "{=WSAS3XLC}There was someone here not long ago, went by the name of {INQUIRY_COMPANION.LINK}. {?INQUIRY_COMPANION.GENDER}She{?}He{\\?} left for {COMPANION_SETTLEMENT}. Perhaps you can find them there, or on the road.[rb:positive]", () => this._inquiryCurrentCompanion.CurrentSettlement != Settlement.CurrentSettlement && this._inquiryVariationIndex % 3 == 1, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_inside_3", "player_selected_companion_type", "player_companion_response", "{=ahydRFKe}There was someone who called {?INQUIRY_COMPANION.GENDER}herself{?}himself{\\?} {INQUIRY_COMPANION.LINK}. Sounds like the kind of person who might interest you, no? {?INQUIRY_COMPANION.GENDER}She{?}He{\\?} was headed for {COMPANION_SETTLEMENT}.[rb:positive]", () => this._inquiryCurrentCompanion.CurrentSettlement != Settlement.CurrentSettlement && this._inquiryVariationIndex % 3 == 2, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_outside_1", "player_selected_companion_type", "player_companion_response", "{=gfmU4wiM}A {?INQUIRY_COMPANION.GENDER}woman{?}man{\\?} named {INQUIRY_COMPANION.LINK} is staying at my tavern. You may want to talk with him.", () => this._inquiryVariationIndex % 3 == 0, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_outside_2", "player_selected_companion_type", "player_companion_response", "{=qSy4Ns1N}There's someone who might meet your needs who is in here having a drink right now. Goes by the name of {INQUIRY_COMPANION.LINK}.", () => this._inquiryVariationIndex % 3 == 1, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddDialogLine("tavernkeeper_companion_info_tavernkeeper_found_answer_companion_is_outside_3", "player_selected_companion_type", "player_companion_response", "{=rLcRwkqK}You might want to look around here for a {?INQUIRY_COMPANION.GENDER}lady{?}fellow{\\?} named {INQUIRY_COMPANION.LINK}. I believe {?INQUIRY_COMPANION.GENDER}she{?}he{\\?} might possess that kind of skill.", () => this._inquiryVariationIndex % 3 == 2, new ConversationSentence.OnConsequenceDelegate(this.IncreaseVariationIndex), 100, null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper", "player_companion_response", "tavernkeeper_companion_info_tavernkeeper_answer", "{=SqrRaIU5}I would like to ask about someone with different expertise.", new ConversationSentence.OnConditionDelegate(this.FoundCompanion), null, 100, null, null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper_2", "player_companion_response", "player_selected_companion_type", "{=vx4ML2gX}Is there someone else other than {INQUIRY_COMPANION.LINK}?", new ConversationSentence.OnConditionDelegate(this.FoundCompanion), delegate
			{
				this.FindCompanionWithType(this._selectedCompanionType);
			}, 100, new ConversationSentence.OnClickableConditionDelegate(TavernEmployeesCampaignBehavior.companion_type_select_clickable_condition), null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper_found", "player_companion_response", "start", "{=3FzBTb3w}Thank you for this information. It was {COMPANION_INQUIRY_COST}{GOLD_ICON} well spent.", new ConversationSentence.OnConditionDelegate(this.FoundCompanion), null, 100, null, null);
			cgs.AddPlayerLine("player_companion_response_to_tavernkeeper_not_found", "player_companion_response", "start", "{=PDf52VCf}Thank you for your time anyway.", () => !this.FoundCompanion(), null, 100, null, null);
			cgs.AddPlayerLine("tavernkeeper_talk_to_leave", "tavernkeeper_talk", "close_window", "{=fF2BdOy9}I don't need anything now.", null, delegate
			{
				this._previouslyRecommendedCompanions.Clear();
			}, 100, null, null);
			cgs.AddDialogLine("1972", "tavernkeeper_pretalk", "tavernkeeper_talk", "{=ds294zxi}Anything else?", null, null, 100, null);
			cgs.AddDialogLine("talk_common_to_tavernmaid", "start", "tavernmaid_talk", "{=ddYWbO8b}The usual?", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_offers_usual_on_condition), null, 100, null);
			cgs.AddDialogLine("talk_common_to_tavernmaid_2", "start", "tavernmaid_talk", "{=x7k87vj3}What can I bring you, {?PLAYER.GENDER}madam{?}sir{\\?}? Would you like to taste our local speciality, {DRINK} with {FOOD}?", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_offers_food_on_condition), null, 100, null);
			cgs.AddDialogLine("talk_common_to_tavernmaid_3", "start", "tavernmaid_talk", "{=Tn9g83ry}Enjoying your drink, {?PLAYER.GENDER}madam{?}sir{\\?}?", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_gossips_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernmaid_order_food", "tavernmaid_talk", "tavernmaid_order_acknowledge", "{=E57VFXqU}I'll have that.", new ConversationSentence.OnConditionDelegate(this.conversation_player_can_order_food_on_condition), null, 100, null, null);
			cgs.AddDialogLine("tavernmain_order_acknowledge", "tavernmaid_order_acknowledge", "close_window", "{=3wb2dCfz}It'll be right up then, {?PLAYER.GENDER}ma'am{?}sir{\\?}.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_tavernmaid_delivers_food_on_consequence), 100, null);
			cgs.AddPlayerLine("tavernmaid_ask_tun", "tavernmaid_talk", "tavern_drink_morale_to_party", "{=oAaKaXEy}I really like this meal. I'd like it served to all my men.", new ConversationSentence.OnConditionDelegate(this.conversation_tavernmaid_buy_tun_on_condition), null, 100, null, null);
			cgs.AddPlayerLine("tavernmaid_leave", "tavernmaid_talk", "close_window", "{=Piq3oYmG}I'm fine, thank you.", null, null, 100, null, null);
			cgs.AddDialogLine("tavernmaid_give_tun_gold", "tavern_drink_morale_to_party", "tun_give_gold", "{=bjNuUqTx}With pleasure, {?PLAYER.GENDER}madam{?}sir{\\?}. That will cost you {COST}{GOLD_ICON}.", new ConversationSentence.OnConditionDelegate(TavernEmployeesCampaignBehavior.calculate_tun_cost_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernmaid_give_tun_gold_2", "tun_give_gold", "tavernmaid_enjoy", "{=nAM821Fb}Here you are.", null, new ConversationSentence.OnConsequenceDelegate(this.can_buy_tun_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.can_buy_tun_on_clickable_condition), null);
			cgs.AddPlayerLine("tavernmaid_not_give_tun_gold", "tun_give_gold", "start", "{=2PEKd3Sz}Actually, I changed my mind. Cancel the order...", null, null, 100, null, null);
			cgs.AddDialogLine("tavernmaid_best_wishes", "tavernmaid_enjoy", "close_window", "{=ZGMfmNe0}Very generous of you, my {?PLAYER.GENDER}lady{?}lord{\\?}. Good health and fortune to all of you.", null, null, 100, null);
			cgs.AddDialogLine("talk_bard", "start", "talk_bard_player", "{=b36QZ1aZ}{LYRIC_SCRAP}", new ConversationSentence.OnConditionDelegate(this.conversation_talk_bard_on_condition), null, 100, null);
			cgs.AddPlayerLine("talk_bard_player_leave", "talk_bard_player", "close_window", "{=sbczu2VI}Play on, good man.", null, null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_quest_info", "tavernkeeper_ask_quests", "tavernkeeper_has_quest", "{=uUkCLZEo}Let's see... {ISSUE_GIVER_LIST}.", new ConversationSentence.OnConditionDelegate(TavernEmployeesCampaignBehavior.tavenkeeper_has_quest_on_condition), null, 100, null);
			cgs.AddPlayerLine("tavernkeeper_player_thanks", "tavernkeeper_has_quest", "tavernkeeper_pretalk", "{=eALf5d30}Thanks!", null, null, 100, null, null);
			cgs.AddDialogLine("tavernkeeper_turndown", "tavernkeeper_doesnot_have_quests", "tavernkeeper_talk", "{=py6Y46sa}No, I didn't hear any...", null, null, 100, null);
			this.AddRansomBrokerDialogs(cgs);
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x00128A78 File Offset: 0x00126C78
		private void player_accepts_clan_info_offer_on_consequence()
		{
			foreach (Hero hero in Settlement.CurrentSettlement.OwnerClan.Heroes)
			{
				hero.IsKnownToPlayer = true;
			}
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, 500, false);
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x00128AE4 File Offset: 0x00126CE4
		private bool player_accepts_clan_info_offer_clickable_condition(out TextObject explanation)
		{
			bool flag = true;
			using (List<Hero>.Enumerator enumerator = Settlement.CurrentSettlement.OwnerClan.Heroes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsKnownToPlayer)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				explanation = new TextObject("{=LBiZ9Rie}You already possess this information.", null);
				return false;
			}
			explanation = new TextObject("{=!}{INFORMATION_COST}{GOLD_ICON}.", null);
			MBTextManager.SetTextVariable("INFORMATION_COST", 500);
			if (Hero.MainHero.Gold < 500)
			{
				explanation = new TextObject("{=xVZVYNan}You don't have enough{GOLD_ICON}.", null);
				return false;
			}
			return true;
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x00128B94 File Offset: 0x00126D94
		private bool tavernkeeper_offer_clan_info_on_condition()
		{
			MBTextManager.SetTextVariable("OWNER_CLAN", Settlement.CurrentSettlement.OwnerClan.EncyclopediaLinkWithName, false);
			MBTextManager.SetTextVariable("SETTLEMENT", Settlement.CurrentSettlement.EncyclopediaLinkWithName, false);
			MBTextManager.SetTextVariable("PRICE", 500);
			return true;
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00128BE0 File Offset: 0x00126DE0
		private void SetCannotThinkOfAnyoneLine(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType type)
		{
			TextObject text;
			if (this._previouslyRecommendedCompanions.ContainsKey(type))
			{
				text = new TextObject("{=eeIX6loR}I can't think of anyone else.", null);
			}
			else
			{
				text = ((this._inquiryVariationIndex % 2 == 0) ? new TextObject("{=BYpoxUEB}I haven't heard of someone with such skills looking for work for a while now.", null) : new TextObject("{=SbYlYGFA}Sorry. No one like that has passed through here for a while.", null));
			}
			MBTextManager.SetTextVariable("CANNOT_THINK_OF_ANYONE_LINE", text, false);
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x00128C38 File Offset: 0x00126E38
		private void IncreaseVariationIndex()
		{
			this._inquiryVariationIndex++;
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00128C48 File Offset: 0x00126E48
		private bool FoundCompanion()
		{
			return this._inquiryCurrentCompanion != null;
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00128C54 File Offset: 0x00126E54
		private void FindCompanionWithType(TavernEmployeesCampaignBehavior.TavernInquiryCompanionType companionType)
		{
			int num = 30;
			Hero inquiryCurrentCompanion = null;
			foreach (Town town in Settlement.CurrentSettlement.MapFaction.Fiefs)
			{
				if (town.IsTown)
				{
					foreach (Hero hero in town.Settlement.HeroesWithoutParty)
					{
						int num2 = 0;
						List<Hero> list;
						this._previouslyRecommendedCompanions.TryGetValue(companionType, out list);
						if (hero.IsWanderer && (list == null || !list.Contains(hero)))
						{
							switch (companionType)
							{
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Scout:
								num2 = hero.GetSkillValue(DefaultSkills.Scouting);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Engineer:
								num2 = hero.GetSkillValue(DefaultSkills.Engineering);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Surgeon:
								num2 = hero.GetSkillValue(DefaultSkills.Medicine);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Quartermaster:
								num2 = hero.GetSkillValue(DefaultSkills.Steward);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.CaravanLeader:
								num2 += hero.GetSkillValue(DefaultSkills.Trade);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Leader:
								num2 = hero.GetSkillValue(DefaultSkills.Leadership);
								num2 += hero.GetSkillValue(DefaultSkills.Tactics);
								break;
							case TavernEmployeesCampaignBehavior.TavernInquiryCompanionType.Roguery:
								num2 = hero.GetSkillValue(DefaultSkills.Roguery);
								break;
							}
						}
						if (num2 > num)
						{
							num = num2;
							inquiryCurrentCompanion = hero;
						}
					}
				}
			}
			this._inquiryCurrentCompanion = inquiryCurrentCompanion;
			this._selectedCompanionType = companionType;
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, 2, false);
			if (this._inquiryCurrentCompanion != null)
			{
				StringHelpers.SetCharacterProperties("INQUIRY_COMPANION", this._inquiryCurrentCompanion.CharacterObject, null, false);
				MBTextManager.SetTextVariable("COMPANION_SETTLEMENT", this._inquiryCurrentCompanion.CurrentSettlement.EncyclopediaLinkWithName, false);
				this._inquiryCurrentCompanion.IsKnownToPlayer = true;
				if (this._previouslyRecommendedCompanions.ContainsKey(this._selectedCompanionType))
				{
					this._previouslyRecommendedCompanions[this._selectedCompanionType].Add(this._inquiryCurrentCompanion);
				}
				else
				{
					this._previouslyRecommendedCompanions.Add(this._selectedCompanionType, new List<Hero>
					{
						this._inquiryCurrentCompanion
					});
				}
			}
			this.SetCannotThinkOfAnyoneLine(companionType);
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00128EC4 File Offset: 0x001270C4
		private bool conversation_talk_bard_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.Musician)
			{
				List<string> list = new List<string>();
				List<Settlement> list2 = (from x in Settlement.All
				where x.IsTown && x.Culture == Settlement.CurrentSettlement.Culture && x != Settlement.CurrentSettlement
				select x).ToList<Settlement>();
				Settlement settlement = list2[MBRandom.RandomInt(0, list2.Count)];
				MBTextManager.SetTextVariable("RANDOM_TOWN", settlement.Name, false);
				list.Add("{=3n1KRLpZ}'My love is far \n I know not where \n Perhaps the winds shall tell me'");
				list.Add("{=NQOQb0C9}'And many thousand bodies lay a-rotting in the sun \n But things like that must be you know for kingdoms to be won'");
				list.Add("{=bs8ayCGX}'A warrior brave you might surely be \n With your blade and your helm and your bold fiery steed \n But I'll give you a warning you'd be wise to heed \n Don't toy with the fishwives of {RANDOM_TOWN}'");
				list.Add("{=3n1KRLpZ}'My love is far \n I know not where \n Perhaps the winds shall tell me'");
				list.Add("{=YequZz6U}'Oh the maidens of {RANDOM_TOWN} are merry and fair \n Plotting their mischief with flowers in their hair \n Were I still a young man I sure would be there \n But now I'll take warmth over trouble'");
				list.Add("{=CM8Tr3lL}'Oh my pocket's been picked \n And my shirt's drenched with sick \n And my head feels like it's come a fit to bursting'");
				list.Add("{=DFkzQHRQ}'For all the silks of the Padishah \n For all the Emperor's gold \n For all the spice in the distance East...'");
				list.Add("{=2fbLBXtT}'O'er the whale-road she sped \n She were manned by the dead  \n And the clouds followed black in her wake'");
				string value = list[MBRandom.RandomInt(0, list.Count)];
				MBTextManager.SetTextVariable("LYRIC_SCRAP", new TextObject(value, null), false);
				return true;
			}
			return false;
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x00128FBB File Offset: 0x001271BB
		private static bool companion_type_select_clickable_condition(out TextObject explanation)
		{
			explanation = new TextObject("{=!}{COMPANION_INQUIRY_COST}{GOLD_ICON}.", null);
			MBTextManager.SetTextVariable("COMPANION_INQUIRY_COST", 2);
			if (Hero.MainHero.Gold < 2)
			{
				explanation = new TextObject("{=xVZVYNan}You don't have enough{GOLD_ICON}.", null);
				return false;
			}
			return true;
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00128FF4 File Offset: 0x001271F4
		private void AddRansomBrokerDialogs(CampaignGameStarter cgs)
		{
			cgs.AddDialogLine("ransom_broker_start", "start", "ransom_broker_intro", "{=!}{RANSOM_BROKER_INTRO}", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_start_on_condition), null, 100, null);
			cgs.AddPlayerLine("ransom_broker_intro", "ransom_broker_intro", "ransom_broker_2", "{=TGYJUUn0}Go on.", null, null, 100, null, null);
			cgs.AddDialogLine("ransom_broker_intro_2_di", "ransom_broker_2", "ransom_broker_3", "{=MFDb5duu}Splendid! I suspect that you may, in your line of work, occasionally acquire a few captives. I could possibly take them off your hands. I'd pay you, of course.", null, null, 100, null);
			cgs.AddPlayerLine("ransom_broker_intro_2", "ransom_broker_3", "ransom_broker_4", "{=bPqwLopK}Mm. Are you a slaver?", null, null, 100, null, null);
			cgs.AddDialogLine("ransom_broker_intro_3", "ransom_broker_4", "ransom_broker_5", "{=YCC0hPuC}Ah, no. Slavers are rare these days. It used to be that, when the Empire and its neighbors made war upon each other, the defeated were usually taken as slaves. That helped pay for the war, you see! But today, for better or for worse, that is not practical. What with the frontiers broken and raiders crossing this way and that, there are far too many opportunities for captives to escape. But that does not mean that war cannot be profitable! Indeed it still can!", null, null, 100, null);
			cgs.AddDialogLine("ransom_broker_intro_4", "ransom_broker_5", "ransom_broker_6", "{=8bfzW0np}Many captives are still taken, and their families will pay to have them back. Men such as I criss-cross the Empire and the outer kingdoms, acquiring prisoners, and contacting their kin for a suitable ransom.", null, null, 100, null);
			cgs.AddDialogLine("ransom_broker_intro_5", "ransom_broker_6", "ransom_broker_info_talk", "{=rLlIVqmY}So, were you to acquire a few prisoners in one of your victorious affrays, and wish to relieve yourself of their care and feeding, I or one of my colleagues would be happy to pay for them as a sort of speculative investment.", null, null, 100, null);
			cgs.AddPlayerLine("ransom_broker_info_talk_player_1", "ransom_broker_info_talk", "ransom_broker_families", "{=QHoCsSZX}What if their families can't pay?", null, null, 100, null, null);
			cgs.AddPlayerLine("ransom_broker_info_talk_player_2", "ransom_broker_info_talk", "ransom_broker_prices", "{=btA10FML}What can I get for a prisoner?", null, null, 100, null, null);
			cgs.AddPlayerLine("ransom_broker_info_talk_player_3", "ransom_broker_info_talk", "ransom_broker_ransom_me", "{=nwJPPIvn}Would you be able to ransom me if I were taken?", null, null, 100, null, null);
			cgs.AddPlayerLine("ransom_broker_info_talk_player_4", "ransom_broker_info_talk", "ransom_broker_pretalk", "{=TyultuzK}That's all I need to know. Thank you.", null, null, 100, null, null);
			cgs.AddDialogLine("ransom_broker_families", "ransom_broker_families", "ransom_broker_info_talk", "{=zxonBgY2}Oh, I suppose I could sell them to the republics of Geroia, to row their galleys, although even in Geroia they prefer free oarsmen these days... But it rarely comes to that. You'd be surprised what sorts of treasures a peasant can dig out of his cowshed or wheedle out of his cousins!", null, null, 100, null);
			cgs.AddDialogLine("ransom_broker_prices", "ransom_broker_prices", "ransom_broker_info_talk", "{=PLbxHyPu}It varies. I fancy that I have a fine eye for assessing a ransom. There are a dozen little things about a man that will tell you whether he goes to bed hungry, or spices his meat with pepper and cloves from the east. The real money of course is in the aristocracy, and if you ever want to do my job you'll want to learn about every landowning family or tribal chief in Calradia, their estates, their offspring both lawful and bastard, and, of course, their credit with the merchants.", null, null, 100, null);
			cgs.AddDialogLine("ransom_broker_ransom_me", "ransom_broker_ransom_me", "ransom_broker_info_talk", "{=4tY23HWb}Of course. I'm welcome in every court in Calradia. There's not many who can say that! So always be sure to keep a pot of denars buried somewhere, and a loyal servant who can find it in a hurry.", null, null, 100, null);
			cgs.AddDialogLine("ransom_broker_start_has_met", "start", "ransom_broker_talk", "{=w4yxgY3F}Greetings. If you have any prisoners, I will be happy to buy them from you.", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_start_has_met_on_condition), null, 100, null);
			cgs.AddDialogLine("ransom_broker_pretalk", "ransom_broker_pretalk", "ransom_broker_talk", "{=AQi1arUp}Anyway, if you have any prisoners, I will be happy to buy them from you.", null, null, 100, null);
			cgs.AddPlayerLine("ransom_broker_talk_player_1", "ransom_broker_talk", "ransom_broker_sell_prisoners", "{=cAVxYAdw}Then you'd better bring your purse. I have got prisoners to sell.", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_open_party_screen_on_condition), null, 100, null, null);
			cgs.AddPlayerLine("ransom_broker_talk_player_2", "ransom_broker_talk", "ransom_broker_2", "{=Yac7bSU3}Tell me about what you do again.", null, null, 100, null, null);
			cgs.AddPlayerLine("ransom_broker_talk_player_3", "ransom_broker_talk", "ransom_broker_ransom_companion", "{=cRVT5qHi}I wish to ransom one of my companions.", null, null, 100, null, null);
			cgs.AddPlayerLine("ransom_broker_talk_player_4", "ransom_broker_talk", "ransom_broker_no_prisoners", "{=CQMkh88h}I don't have any prisoners to sell, but that's good to know.", null, null, 100, null, null);
			cgs.AddDialogLine("ransom_broker_no_prisoners", "ransom_broker_no_prisoners", "close_window", "{=mEsaiLOR}Very well then. If you happen to have any more prisoners, you know where to find me.", null, null, 100, null);
			cgs.AddDialogLine("ransom_broker_ransom_companion", "ransom_broker_ransom_companion", "ransom_broker_ransom_companion_choose", "{=MHHdl7QC}Whom do you wish to ransom?", null, null, 100, null);
			cgs.AddRepeatablePlayerLine("ransom_broker_ransom_companion_choose", "ransom_broker_ransom_companion_choose", "ransom_broker_ransom_companion_name_sum", "{=!}{COMPANION.LINK}", "{=nomZx5Nw}I am thinking of a different companion.", "ransom_broker_ransom_companion", new ConversationSentence.OnConditionDelegate(this.conversation_ransom_broker_ransom_companion_choose_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_ransom_broker_ransom_companion_choose_on_consequence), 100, null);
			cgs.AddPlayerLine("ransom_broker_ransom_companion_choose_never_mind", "ransom_broker_ransom_companion_choose", "ransom_broker_pretalk", "{=tdvnKIyS}Never mind.", null, null, 100, null, null);
			cgs.AddDialogLine("ransom_broker_sell_prisoners", "ransom_broker_sell_prisoners", "ransom_broker_sell_prisoners_3", "{=xFmYRCHs}Let me see what you have...", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_ransom_broker_sell_prisoners_on_consequence), 100, null);
			cgs.AddDialogLine("ransom_broker_sell_prisoners_3", "ransom_broker_sell_prisoners_3", "ransom_broker_pretalk", "{=3BvfOe1y}Very well then. You catch some more and you want me to take them off of your hands, you know where to find me...", null, null, 100, null);
			cgs.AddDialogLine("ransom_broker_sell_prisoners_2", "ransom_broker_sell_prisoners_2", "close_window", "{=fQaPv0Xl}I will be staying here for a few days. Let me know if you need my services.", null, null, 100, null);
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x001293B8 File Offset: 0x001275B8
		private bool conversation_tavernmaid_offers_usual_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && !this._orderedDrinkThisVisit && this._orderedDrinkThisDayInSettlement == Settlement.CurrentSettlement;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x001293E0 File Offset: 0x001275E0
		private bool conversation_tavernmaid_offers_food_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && this._orderedDrinkThisDayInSettlement != Settlement.CurrentSettlement)
			{
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "vlandia")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=07qBenIW}a flagon of ale", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=uJceH1Dv}a Sunor sausage", null), false);
					if (MobileParty.MainParty.CurrentSettlement.Position2D.y < 150f)
					{
						MBTextManager.SetTextVariable("FOOD", new TextObject("{=07QlFlXK}a plate of herrings", null), false);
					}
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "empire")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=ybXgaKEv}a glass of local wine", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=IBhZGxxm}a plate of olives", null), false);
					if (MobileParty.MainParty.CurrentSettlement.Position2D.x < 300f)
					{
						MBTextManager.SetTextVariable("FOOD", new TextObject("{=d18Ul2Zl}a plate of sardines", null), false);
					}
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "khuzait")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=WZbrxhYm}a flask of kefir", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=0qc11kmz}a plate of mutton dumplings", null), false);
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "aserai")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=AULqrp7D}a glass of Calradian wine", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=GhPGpR90}a plate of dates", null), false);
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "sturgia")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=bZbFrIUr}a mug of kvass", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=LPBVTiV6}a strip of bacon", null), false);
				}
				if (MobileParty.MainParty.CurrentSettlement.Culture.StringId == "battania")
				{
					MBTextManager.SetTextVariable("DRINK", new TextObject("{=vEHaOSIT}a mug of sour beer", null), false);
					MBTextManager.SetTextVariable("FOOD", new TextObject("{=z4arML8E}a strip of dried venison", null), false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0012963B File Offset: 0x0012783B
		private void conversation_tavernmaid_delivers_food_on_consequence()
		{
			this._orderedDrinkThisDayInSettlement = Settlement.CurrentSettlement;
			this._orderedDrinkThisVisit = true;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0012964F File Offset: 0x0012784F
		private bool conversation_tavernmaid_gossips_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && this._orderedDrinkThisDayInSettlement == Settlement.CurrentSettlement;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x0012966E File Offset: 0x0012786E
		private bool conversation_player_can_order_food_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && !this._orderedDrinkThisVisit;
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0012968C File Offset: 0x0012788C
		private static bool tavenkeeper_has_quest_on_condition()
		{
			List<IssueBase> list = IssueManager.GetIssuesInSettlement(Hero.MainHero.CurrentSettlement, true).ToList<IssueBase>();
			if (list.Count > 0)
			{
				list.Shuffle<IssueBase>();
				if (list.Count == 1)
				{
					MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=roTCX8S8}{ISSUE_GIVER_1.LINK} mentioned something about needing someone to do some work.", false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_1", list[0].IssueOwner.CharacterObject, null, false);
				}
				else if (list.Count == 2)
				{
					MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=79XElnsg}{ISSUE_GIVER_1.LINK} mentioned something about needing someone to do some work. And I think {ISSUE_GIVER_2.LINK} was looking for someone.", false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_1", list[0].IssueOwner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_2", list[1].IssueOwner.CharacterObject, null, false);
				}
				else
				{
					MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=SIxE2LGn}{ISSUE_GIVER_1.LINK} mentioned something about needing someone to do some work. And I think {ISSUE_GIVER_2.LINK} and {ISSUE_GIVER_3.LINK} were looking for someone.", false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_1", list[0].IssueOwner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_2", list[1].IssueOwner.CharacterObject, null, false);
					StringHelpers.SetCharacterProperties("ISSUE_GIVER_3", list[2].IssueOwner.CharacterObject, null, false);
				}
				return true;
			}
			MBTextManager.SetTextVariable("ISSUE_GIVER_LIST", "{=RlP8aYVJ}Nobody is looking for help right now.", false);
			return true;
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x001297D1 File Offset: 0x001279D1
		private bool tavernkeeper_talk_companion_on_condition()
		{
			this._inquiryCurrentCompanion = null;
			if (Settlement.CurrentSettlement.OwnerClan.Kingdom != null)
			{
				MBTextManager.SetTextVariable("FACTION_INFORMAL_NAME", Settlement.CurrentSettlement.MapFaction.InformalName, false);
				return true;
			}
			return false;
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x00129808 File Offset: 0x00127A08
		private bool conversation_tavernmaid_buy_tun_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.TavernWench && this._orderedDrinkThisDayInSettlement == Settlement.CurrentSettlement && !this._hasBoughtTunToParty && PartyBase.MainParty.MemberRoster.Count > 1;
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x00129841 File Offset: 0x00127A41
		private bool can_buy_tun_on_clickable_condition(out TextObject explanation)
		{
			explanation = TextObject.Empty;
			if (Hero.MainHero.Gold < TavernEmployeesCampaignBehavior.get_tun_price())
			{
				explanation = new TextObject("{=xVZVYNan}You don't have enough{GOLD_ICON}.", null);
				return false;
			}
			return true;
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0012986C File Offset: 0x00127A6C
		private void can_buy_tun_on_consequence()
		{
			int tun_price = TavernEmployeesCampaignBehavior.get_tun_price();
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, tun_price, false);
			MobileParty.MainParty.RecentEventsMorale += 2f;
			this._hasBoughtTunToParty = true;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x001298AC File Offset: 0x00127AAC
		private static bool calculate_tun_cost_on_condition()
		{
			int tun_price = TavernEmployeesCampaignBehavior.get_tun_price();
			MBTextManager.SetTextVariable("COST", tun_price);
			return true;
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x001298CB File Offset: 0x00127ACB
		private static int get_tun_price()
		{
			return (int)(50f + (float)MobileParty.MainParty.MemberRoster.TotalHealthyCount * 0.2f);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x001298EC File Offset: 0x00127AEC
		private bool conversation_ransom_broker_start_on_condition()
		{
			if (CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.RansomBroker && !this._hasMetWithRansomBroker)
			{
				this._hasMetWithRansomBroker = true;
				MBTextManager.SetTextVariable("RANSOM_BROKER_INTRO", "{=Y7tozytM}Hello, {?PLAYER.GENDER}madam{?}sir{\\?}. You have the bearing of a warrior. Do you have a minute? We may have interests in common.", false);
				if (Settlement.CurrentSettlement.OwnerClan == Hero.MainHero.Clan || Settlement.CurrentSettlement.MapFaction.Leader == Hero.MainHero)
				{
					MBTextManager.SetTextVariable("RANSOM_BROKER_INTRO", "{=6zxo4AU9}This is quite the honor, your {?PLAYER.GENDER}ladyship{?}lordship{\\?}. Do you have a minute? I may be able to do you a service.", false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x00129965 File Offset: 0x00127B65
		private bool conversation_ransom_broker_open_party_screen_on_condition()
		{
			return MobileParty.MainParty.Party.NumberOfPrisoners > 0;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00129979 File Offset: 0x00127B79
		private bool conversation_ransom_broker_start_has_met_on_condition()
		{
			return CharacterObject.OneToOneConversationCharacter.Occupation == Occupation.RansomBroker && this._hasMetWithRansomBroker;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00129991 File Offset: 0x00127B91
		private void conversation_ransom_broker_sell_prisoners_on_consequence()
		{
			PartyScreenManager.OpenScreenAsRansom();
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x00129998 File Offset: 0x00127B98
		private bool conversation_ransom_broker_ransom_companion_choose_on_condition()
		{
			return false;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x0012999B File Offset: 0x00127B9B
		private void conversation_ransom_broker_ransom_companion_choose_on_consequence()
		{
		}

		// Token: 0x04001215 RID: 4629
		private const int TavernCompanionInquiryCost = 2;

		// Token: 0x04001216 RID: 4630
		private const int MinimumTavernCompanionInquirySkillLevel = 30;

		// Token: 0x04001217 RID: 4631
		private const int BaseTunPrice = 50;

		// Token: 0x04001218 RID: 4632
		private const int AskForClanInfoPrice = 500;

		// Token: 0x04001219 RID: 4633
		private Settlement _orderedDrinkThisDayInSettlement;

		// Token: 0x0400121A RID: 4634
		private bool _orderedDrinkThisVisit;

		// Token: 0x0400121B RID: 4635
		private bool _hasMetWithRansomBroker;

		// Token: 0x0400121C RID: 4636
		private bool _hasBoughtTunToParty;

		// Token: 0x0400121D RID: 4637
		private Hero _inquiryCurrentCompanion;

		// Token: 0x0400121E RID: 4638
		private TavernEmployeesCampaignBehavior.TavernInquiryCompanionType _selectedCompanionType;

		// Token: 0x0400121F RID: 4639
		private int _inquiryVariationIndex;

		// Token: 0x04001220 RID: 4640
		private readonly Dictionary<TavernEmployeesCampaignBehavior.TavernInquiryCompanionType, List<Hero>> _previouslyRecommendedCompanions = new Dictionary<TavernEmployeesCampaignBehavior.TavernInquiryCompanionType, List<Hero>>();

		// Token: 0x0200073B RID: 1851
		private enum TavernInquiryCompanionType
		{
			// Token: 0x04001E6E RID: 7790
			Scout,
			// Token: 0x04001E6F RID: 7791
			Engineer,
			// Token: 0x04001E70 RID: 7792
			Surgeon,
			// Token: 0x04001E71 RID: 7793
			Quartermaster,
			// Token: 0x04001E72 RID: 7794
			CaravanLeader,
			// Token: 0x04001E73 RID: 7795
			Leader,
			// Token: 0x04001E74 RID: 7796
			Roguery
		}
	}
}
