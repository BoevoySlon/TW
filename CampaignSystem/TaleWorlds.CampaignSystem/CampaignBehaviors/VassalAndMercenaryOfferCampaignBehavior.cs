﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003DF RID: 991
	public class VassalAndMercenaryOfferCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003D4F RID: 15695 RVA: 0x0012BA4A File Offset: 0x00129C4A
		private static TextObject DecisionPopUpTitleText
		{
			get
			{
				return new TextObject("{=ho5EndaV}Decision", null);
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003D50 RID: 15696 RVA: 0x0012BA57 File Offset: 0x00129C57
		private static TextObject DecisionPopUpAffirmativeText
		{
			get
			{
				return new TextObject("{=Y94H6XnK}Accept", null);
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003D51 RID: 15697 RVA: 0x0012BA64 File Offset: 0x00129C64
		private static TextObject DecisionPopUpNegativeText
		{
			get
			{
				return new TextObject("{=cOgmdp9e}Decline", null);
			}
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x0012BA74 File Offset: 0x00129C74
		public override void RegisterEvents()
		{
			if (!this._stopOffers)
			{
				CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
				CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
				CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
				CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
				CampaignEvents.OnVassalOrMercenaryServiceOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnVassalOrMercenaryServiceOfferedToPlayer));
				CampaignEvents.OnVassalOrMercenaryServiceOfferCanceledEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnVassalOrMercenaryServiceOfferCanceled));
				CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
				CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
				CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, new Action<Kingdom>(this.OnKingdomDestroyed));
				CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
			}
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0012BB72 File Offset: 0x00129D72
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Tuple<Kingdom, CampaignTime>>("_currentMercenaryOffer", ref this._currentMercenaryOffer);
			dataStore.SyncData<Dictionary<Kingdom, CampaignTime>>("_vassalOffers", ref this._vassalOffers);
			dataStore.SyncData<bool>("_stopOffers", ref this._stopOffers);
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x0012BBAA File Offset: 0x00129DAA
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddVassalDialogues(campaignGameStarter);
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x0012BBB4 File Offset: 0x00129DB4
		private void DailyTick()
		{
			if (!this._stopOffers && Clan.PlayerClan.Tier > Campaign.Current.Models.ClanTierModel.MinClanTier)
			{
				if (this._currentMercenaryOffer != null)
				{
					if (this._currentMercenaryOffer.Item2.ElapsedHoursUntilNow >= 48f || !this.MercenaryKingdomSelectionConditionsHold(this._currentMercenaryOffer.Item1))
					{
						CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(this._currentMercenaryOffer.Item1);
						return;
					}
				}
				else
				{
					float randomFloat = MBRandom.RandomFloat;
					if (randomFloat <= 0.02f && this.CanPlayerClanReceiveMercenaryOffer())
					{
						Kingdom randomElementWithPredicate = Kingdom.All.GetRandomElementWithPredicate(new Func<Kingdom, bool>(this.MercenaryKingdomSelectionConditionsHold));
						if (randomElementWithPredicate != null)
						{
							this.CreateMercenaryOffer(randomElementWithPredicate);
							return;
						}
					}
					else if (randomFloat <= 0.01f && this.CanPlayerClanReceiveVassalOffer())
					{
						Kingdom randomElementWithPredicate2 = Kingdom.All.GetRandomElementWithPredicate(new Func<Kingdom, bool>(this.VassalKingdomSelectionConditionsHold));
						if (randomElementWithPredicate2 != null)
						{
							this.CreateVassalOffer(randomElementWithPredicate2);
						}
					}
				}
			}
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x0012BCA4 File Offset: 0x00129EA4
		private bool VassalKingdomSelectionConditionsHold(Kingdom kingdom)
		{
			List<IFaction> list;
			List<IFaction> list2;
			return !this._vassalOffers.ContainsKey(kingdom) && FactionHelper.CanPlayerOfferVassalage(kingdom, out list, out list2);
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x0012BCCC File Offset: 0x00129ECC
		private bool MercenaryKingdomSelectionConditionsHold(Kingdom kingdom)
		{
			List<IFaction> list;
			List<IFaction> list2;
			return FactionHelper.CanPlayerOfferMercenaryService(kingdom, out list, out list2);
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x0012BCE3 File Offset: 0x00129EE3
		private void OnHeroPrisonerTaken(PartyBase captor, Hero prisoner)
		{
			if (prisoner == Hero.MainHero && this._currentMercenaryOffer != null)
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(this._currentMercenaryOffer.Item1);
			}
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0012BD0C File Offset: 0x00129F0C
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (clan == Clan.PlayerClan && newKingdom != null)
			{
				if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinAsMercenary && this._currentMercenaryOffer != null && this._currentMercenaryOffer.Item1 != newKingdom)
				{
					CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(this._currentMercenaryOffer.Item1);
					return;
				}
				if (detail == ChangeKingdomAction.ChangeKingdomActionDetail.JoinKingdom || detail == ChangeKingdomAction.ChangeKingdomActionDetail.CreateKingdom)
				{
					this._stopOffers = true;
					if (this._currentMercenaryOffer != null)
					{
						CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(this._currentMercenaryOffer.Item1);
					}
					foreach (KeyValuePair<Kingdom, CampaignTime> keyValuePair in this._vassalOffers.ToDictionary((KeyValuePair<Kingdom, CampaignTime> x) => x.Key, (KeyValuePair<Kingdom, CampaignTime> x) => x.Value))
					{
						CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(keyValuePair.Key);
					}
				}
			}
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x0012BE20 File Offset: 0x0012A020
		private void OnVassalOrMercenaryServiceOfferedToPlayer(Kingdom kingdom)
		{
			if (this._currentMercenaryOffer != null && this._currentMercenaryOffer.Item1 == kingdom)
			{
				this.CreateMercenaryOfferDecisionPopUp(kingdom);
			}
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x0012BE3F File Offset: 0x0012A03F
		private void OnVassalOrMercenaryServiceOfferCanceled(Kingdom kingdom)
		{
			this.ClearKingdomOffer(kingdom);
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x0012BE48 File Offset: 0x0012A048
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			if ((faction1 == Clan.PlayerClan || faction2 == Clan.PlayerClan) && this._currentMercenaryOffer != null && !this.MercenaryKingdomSelectionConditionsHold(this._currentMercenaryOffer.Item1))
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(this._currentMercenaryOffer.Item1);
			}
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x0012BE98 File Offset: 0x0012A098
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if ((effectiveHero == Hero.MainHero || effectiveHeroGainedRelationWith == Hero.MainHero) && this._currentMercenaryOffer != null && !this.MercenaryKingdomSelectionConditionsHold(this._currentMercenaryOffer.Item1))
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(this._currentMercenaryOffer.Item1);
			}
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x0012BEE5 File Offset: 0x0012A0E5
		private void OnKingdomDestroyed(Kingdom destroyedKingdom)
		{
			if ((this._currentMercenaryOffer != null && this._currentMercenaryOffer.Item1 == destroyedKingdom) || this._vassalOffers.ContainsKey(destroyedKingdom))
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(destroyedKingdom);
			}
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x0012BF18 File Offset: 0x0012A118
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
			if (this._currentMercenaryOffer != null)
			{
				CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(this._currentMercenaryOffer.Item1);
			}
			if (!this._vassalOffers.IsEmpty<KeyValuePair<Kingdom, CampaignTime>>())
			{
				foreach (Kingdom kingdom in Kingdom.All)
				{
					if (this._vassalOffers.ContainsKey(kingdom))
					{
						CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled(kingdom);
					}
				}
			}
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x0012BFA8 File Offset: 0x0012A1A8
		private void ClearKingdomOffer(Kingdom kingdom)
		{
			if (this._currentMercenaryOffer != null && this._currentMercenaryOffer.Item1 == kingdom)
			{
				this._currentMercenaryOffer = null;
				return;
			}
			if (this._vassalOffers.Count > 0)
			{
				this._vassalOffers.Clear();
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x0012BFE1 File Offset: 0x0012A1E1
		private bool CanPlayerClanReceiveMercenaryOffer()
		{
			return Clan.PlayerClan.Kingdom == null && Clan.PlayerClan.Tier == Campaign.Current.Models.ClanTierModel.MercenaryEligibleTier;
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x0012C014 File Offset: 0x0012A214
		public void CreateMercenaryOffer(Kingdom kingdom)
		{
			this._currentMercenaryOffer = new Tuple<Kingdom, CampaignTime>(kingdom, CampaignTime.Now);
			VassalAndMercenaryOfferCampaignBehavior.MercenaryOfferPanelNotificationText.SetCharacterProperties("OFFERED_KINGDOM_LEADER", kingdom.Leader.CharacterObject, false);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new MercenaryOfferMapNotification(kingdom, VassalAndMercenaryOfferCampaignBehavior.MercenaryOfferPanelNotificationText));
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x0012C068 File Offset: 0x0012A268
		private void CreateMercenaryOfferDecisionPopUp(Kingdom kingdom)
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			int mercenaryAwardFactorToJoinKingdom = Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(Clan.PlayerClan, kingdom, true);
			VassalAndMercenaryOfferCampaignBehavior.MercenaryOfferDecisionPopUpExplanationText.SetTextVariable("OFFERED_KINGDOM_NAME", kingdom.Name);
			VassalAndMercenaryOfferCampaignBehavior.MercenaryOfferDecisionPopUpExplanationText.SetTextVariable("GOLD_AMOUNT", mercenaryAwardFactorToJoinKingdom);
			InformationManager.ShowInquiry(new InquiryData(VassalAndMercenaryOfferCampaignBehavior.DecisionPopUpTitleText.ToString(), VassalAndMercenaryOfferCampaignBehavior.MercenaryOfferDecisionPopUpExplanationText.ToString(), true, true, VassalAndMercenaryOfferCampaignBehavior.DecisionPopUpAffirmativeText.ToString(), VassalAndMercenaryOfferCampaignBehavior.DecisionPopUpNegativeText.ToString(), new Action(this.MercenaryOfferAccepted), new Action(this.MercenaryOfferDeclined), "", 0f, null, null, null), false, false);
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x0012C120 File Offset: 0x0012A320
		private void MercenaryOfferAccepted()
		{
			Kingdom item = this._currentMercenaryOffer.Item1;
			this.ClearKingdomOffer(this._currentMercenaryOffer.Item1);
			int mercenaryAwardFactorToJoinKingdom = Campaign.Current.Models.MinorFactionsModel.GetMercenaryAwardFactorToJoinKingdom(Clan.PlayerClan, item, true);
			ChangeKingdomAction.ApplyByJoinFactionAsMercenary(Clan.PlayerClan, item, mercenaryAwardFactorToJoinKingdom, true);
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x0012C173 File Offset: 0x0012A373
		private void MercenaryOfferDeclined()
		{
			this.ClearKingdomOffer(this._currentMercenaryOffer.Item1);
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x0012C186 File Offset: 0x0012A386
		private bool CanPlayerClanReceiveVassalOffer()
		{
			return (Clan.PlayerClan.Kingdom == null || Clan.PlayerClan.IsUnderMercenaryService) && Clan.PlayerClan.Tier >= Campaign.Current.Models.ClanTierModel.VassalEligibleTier;
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x0012C1C8 File Offset: 0x0012A3C8
		public void CreateVassalOffer(Kingdom kingdom)
		{
			this._vassalOffers.Add(kingdom, CampaignTime.Now);
			VassalAndMercenaryOfferCampaignBehavior.VassalOfferPanelNotificationText.SetTextVariable("OFFERED_KINGDOM_NAME", kingdom.Name);
			VassalAndMercenaryOfferCampaignBehavior.VassalOfferPanelNotificationText.SetCharacterProperties("OFFERED_KINGDOM_LEADER", kingdom.Leader.CharacterObject, false);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new VassalOfferMapNotification(kingdom, VassalAndMercenaryOfferCampaignBehavior.VassalOfferPanelNotificationText));
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x0012C234 File Offset: 0x0012A434
		private void AddVassalDialogues(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("valid_vassal_offer_start", "start", "valid_vassal_offer_player_response", "{=aDABE6Md}Greetings, {PLAYER.NAME}. I am glad that you received my message. Are you interested in my offer?", new ConversationSentence.OnConditionDelegate(this.valid_vassal_offer_start_condition), null, int.MaxValue, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_accepts_response", "valid_vassal_offer_player_response", "vassal_offer_start_oath", "{=IHXqZSnt}Yes, I am ready to accept your offer.", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_declines_response", "valid_vassal_offer_player_response", "vassal_offer_king_response_to_decline", "{=FAuoq2gT}While I am honored, I must decline your offer.", null, new ConversationSentence.OnConsequenceDelegate(this.vassal_conversation_end_consequence), 100, null, null);
			campaignGameStarter.AddDialogLine("vassal_offer_king_response_to_accept_continue", "vassal_offer_start_oath", "vassal_offer_king_response_to_accept_start_oath_1_response", "{=54PbMkNw}Good. Then repeat the words of the oath with me: {OATH_LINE_1}", new ConversationSentence.OnConditionDelegate(this.conversation_set_oath_phrases_on_condition), null, 100, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_1", "vassal_offer_king_response_to_accept_start_oath_1_response", "vassal_offer_king_response_to_accept_start_oath_2", "{=!}{OATH_LINE_1}", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_1_decline", "vassal_offer_king_response_to_accept_start_oath_1_response", "vassal_offer_king_response_to_accept_start_oath_decline", "{=8bLwh9yy}Excuse me, {?CONVERSATION_NPC.GENDER}my lady{?}sir{\\?}. But I feel I need to think about this.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("vassal_offer_lord_oath_2", "vassal_offer_king_response_to_accept_start_oath_2", "vassal_offer_king_response_to_accept_start_oath_2_response", "{=!}{OATH_LINE_2}", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_2", "vassal_offer_king_response_to_accept_start_oath_2_response", "vassal_offer_king_response_to_accept_start_oath_3", "{=!}{OATH_LINE_2}", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_2_decline", "vassal_offer_king_response_to_accept_start_oath_2_response", "vassal_offer_king_response_to_accept_start_oath_decline", "{=LKdrCaTO}{?CONVERSATION_NPC.GENDER}My lady{?}Sir{\\?}, may I ask for some time to think about this?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("vassal_offer_lord_oath_3", "vassal_offer_king_response_to_accept_start_oath_3", "vassal_offer_king_response_to_accept_start_oath_3_response", "{=!}{OATH_LINE_3}", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_3", "vassal_offer_king_response_to_accept_start_oath_3_response", "vassal_offer_king_response_to_accept_start_oath_4", "{=!}{OATH_LINE_3}", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_3_decline", "vassal_offer_king_response_to_accept_start_oath_3_response", "vassal_offer_king_response_to_accept_start_oath_decline", "{=aa5F4vP5}My {?CONVERSATION_NPC.GENDER}lady{?}lord{\\?}, please give me more time to think about this.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("vassal_offer_lord_oath_4", "vassal_offer_king_response_to_accept_start_oath_4", "vassal_offer_king_response_to_accept_start_oath_4_response", "{=!}{OATH_LINE_4}", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_4", "vassal_offer_king_response_to_accept_start_oath_4_response", "lord_give_oath_10", "{=!}{OATH_LINE_4}", null, null, 100, null, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_oath_4_decline", "vassal_offer_king_response_to_accept_start_oath_4_response", "vassal_offer_king_response_to_accept_start_oath_decline", "{=aupbQveh}{?CONVERSATION_NPC.GENDER}Madame{?}Sir{\\?}, I must have more time to consider this.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("vassal_offer_king_response_to_decline_during_oath", "vassal_offer_king_response_to_accept_start_oath_decline", "lord_start", "{=vueZBBYB}Indeed. I am not sure why you didn't make up your mind before coming to speak with me.", null, new ConversationSentence.OnConsequenceDelegate(this.vassal_conversation_end_consequence), 100, null);
			campaignGameStarter.AddDialogLine("vassal_offer_king_response_to_decline_continue", "vassal_offer_king_response_to_decline", "lord_start", "{=Lo2kJuhK}I am sorry to hear that.", null, null, 100, null);
			campaignGameStarter.AddDialogLine("invalid_vassal_offer_start", "start", "invalid_vassal_offer_player_response", "{=!}{INVALID_REASON}[if:idle_angry][ib:closed]", new ConversationSentence.OnConditionDelegate(this.invalid_vassal_offer_start_condition), null, int.MaxValue, null);
			campaignGameStarter.AddPlayerLine("vassal_offer_player_accepts_response_2", "invalid_vassal_offer_player_response", "lord_start", "{=AmBEgOyq}I see...", null, new ConversationSentence.OnConsequenceDelegate(this.vassal_conversation_end_consequence), 100, null, null);
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x0012C4F4 File Offset: 0x0012A6F4
		private bool valid_vassal_offer_start_condition()
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.IsJoinedBattle)
			{
				return false;
			}
			if (Hero.OneToOneConversationHero != null)
			{
				IFaction mapFaction = Hero.OneToOneConversationHero.MapFaction;
				if ((mapFaction == null || mapFaction.IsKingdomFaction) && !Hero.OneToOneConversationHero.IsPrisoner)
				{
					KeyValuePair<Kingdom, CampaignTime> keyValuePair = this._vassalOffers.FirstOrDefault(delegate(KeyValuePair<Kingdom, CampaignTime> o)
					{
						IFaction key = o.Key;
						Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
						return key == ((oneToOneConversationHero != null) ? oneToOneConversationHero.MapFaction : null);
					});
					List<IFaction> list;
					List<IFaction> list2;
					bool flag = Hero.OneToOneConversationHero != null && keyValuePair.Key != null && Hero.OneToOneConversationHero == keyValuePair.Key.Leader && FactionHelper.CanPlayerOfferVassalage((Kingdom)Hero.OneToOneConversationHero.MapFaction, out list, out list2);
					if (flag)
					{
						StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
						Hero.OneToOneConversationHero.SetHasMet();
						float scoreOfKingdomToGetClan = Campaign.Current.Models.DiplomacyModel.GetScoreOfKingdomToGetClan((Kingdom)Hero.OneToOneConversationHero.MapFaction, Clan.PlayerClan);
						flag &= (scoreOfKingdomToGetClan > 0f);
					}
					return flag;
				}
			}
			return false;
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x0012C608 File Offset: 0x0012A808
		private bool conversation_set_oath_phrases_on_condition()
		{
			Hero leader = Hero.OneToOneConversationHero.MapFaction.Leader;
			string stringId = Hero.OneToOneConversationHero.Culture.StringId;
			MBTextManager.SetTextVariable("FACTION_TITLE", leader.IsFemale ? Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_liege_title_female", leader.CharacterObject) : Campaign.Current.ConversationManager.FindMatchingTextOrNull("str_liege_title", leader.CharacterObject), false);
			StringHelpers.SetCharacterProperties("LORD", CharacterObject.OneToOneConversationCharacter, null, false);
			if (stringId == "empire")
			{
				MBTextManager.SetTextVariable("OATH_LINE_1", "{=ya8VF98X}I swear by my ancestors that you are lawful {FACTION_TITLE}.", false);
			}
			else if (stringId == "khuzait")
			{
				MBTextManager.SetTextVariable("OATH_LINE_1", "{=PP8VeNiC}I swear that you are my {?LORD.GENDER}khatun{?}khan{\\?}, my {?LORD.GENDER}mother{?}father{\\?}, my protector...", false);
			}
			else
			{
				MBTextManager.SetTextVariable("OATH_LINE_1", "{=MqIg6Mh2}I swear homage to you as lawful {FACTION_TITLE}.", false);
			}
			if (stringId == "empire")
			{
				MBTextManager.SetTextVariable("OATH_LINE_2", "{=vuEyisBW}I affirm that you are executor of the will of the Senate and people...", false);
			}
			else if (stringId == "khuzait")
			{
				MBTextManager.SetTextVariable("OATH_LINE_2", "{=QSPMKz2R}You are the chosen of the Sky, and I shall follow your banner as long as my breath remains...", false);
			}
			else if (stringId == "battania")
			{
				MBTextManager.SetTextVariable("OATH_LINE_2", "{=OHJYAaW5}The powers of Heaven and of the Earth have entrusted to you the guardianship of this sacred land...", false);
			}
			else if (stringId == "aserai")
			{
				MBTextManager.SetTextVariable("OATH_LINE_2", "{=kc3tLqGy}You command the sons of Asera in war and govern them in peace...", false);
			}
			else if (stringId == "sturgia")
			{
				MBTextManager.SetTextVariable("OATH_LINE_2", "{=Qs7qs3b0}You are the shield of our people against the wolves of the forest, the steppe and the sea.", false);
			}
			else
			{
				MBTextManager.SetTextVariable("OATH_LINE_2", "{=PypPEj5Z}I will be your loyal {?PLAYER.GENDER}follower{?}man{\\?} as long as my breath remains...", false);
			}
			if (stringId == "empire")
			{
				MBTextManager.SetTextVariable("OATH_LINE_3", "{=LWFDXeQc}Furthermore, I accept induction into the army of Calradia, at the rank of archon.", false);
			}
			else if (stringId == "khuzait")
			{
				MBTextManager.SetTextVariable("OATH_LINE_3", "{=8lOCOcXw}Your word shall direct the strike of my sword and the flight of my arrow...", false);
			}
			else if (stringId == "aserai")
			{
				MBTextManager.SetTextVariable("OATH_LINE_3", "{=bue9AShm}I swear to fight your enemies and give shelter and water to your friends...", false);
			}
			else if (stringId == "sturgia")
			{
				MBTextManager.SetTextVariable("OATH_LINE_3", "{=U3u2D6Ze}I give you my word and bond, to stand by your banner in battle so long as my breath remains...", false);
			}
			else if (stringId == "battania")
			{
				MBTextManager.SetTextVariable("OATH_LINE_3", "{=UwbhGhGw}I shall stand by your side and not foresake you, and fight until my life leaves my body...", false);
			}
			else
			{
				MBTextManager.SetTextVariable("OATH_LINE_3", "{=2o7U1bNV}..and I will be at your side to fight your enemies should you need my sword.", false);
			}
			if (stringId == "empire")
			{
				MBTextManager.SetTextVariable("OATH_LINE_4", "{=EsF8sEaQ}And as such, that you are my commander, and I shall follow you wherever you lead.", false);
			}
			else if (stringId == "battania")
			{
				MBTextManager.SetTextVariable("OATH_LINE_4", "{=6KbDn1HS}I shall heed your judgements and pay you the tribute that is your due, so that this land may have a strong protector.", false);
			}
			else if (stringId == "khuzait")
			{
				MBTextManager.SetTextVariable("OATH_LINE_4", "{=xDzxaYed}Your word shall divide the spoils of victory and the bounties of peace.", false);
			}
			else if (stringId == "aserai")
			{
				MBTextManager.SetTextVariable("OATH_LINE_4", "{=qObicX7y}I swear to heed your judgements according to the laws of the Aserai, and ensure that my kinfolk heed them as well...", false);
			}
			else if (stringId == "sturgia")
			{
				MBTextManager.SetTextVariable("OATH_LINE_4", "{=HpWYfcgw}..and to uphold your rights under the laws of the Sturgians, and the rights of your kin, and to avenge their blood as thought it were my own.", false);
			}
			else
			{
				MBTextManager.SetTextVariable("OATH_LINE_4", "{=waoSd6tj}.. and I shall defend your rights and the rights of your legitimate heirs.", false);
			}
			StringHelpers.SetCharacterProperties("CONVERSATION_NPC", CharacterObject.OneToOneConversationCharacter, null, false);
			return true;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x0012C8FC File Offset: 0x0012AAFC
		private bool invalid_vassal_offer_start_condition()
		{
			if (Hero.OneToOneConversationHero != null)
			{
				IFaction mapFaction = Hero.OneToOneConversationHero.MapFaction;
				if ((mapFaction == null || mapFaction.IsKingdomFaction) && (PlayerEncounter.Current == null || (PlayerEncounter.Current.EncounterState != PlayerEncounterState.FreeHeroes && PlayerEncounter.Current.EncounterState != PlayerEncounterState.CaptureHeroes)))
				{
					Kingdom offerKingdom = (Kingdom)Hero.OneToOneConversationHero.MapFaction;
					KeyValuePair<Kingdom, CampaignTime> keyValuePair = this._vassalOffers.FirstOrDefault((KeyValuePair<Kingdom, CampaignTime> o) => o.Key == offerKingdom);
					List<IFaction> list = new List<IFaction>();
					List<IFaction> list2 = new List<IFaction>();
					bool flag = Hero.OneToOneConversationHero != null && keyValuePair.Key != null && Hero.OneToOneConversationHero == keyValuePair.Key.Leader && !FactionHelper.CanPlayerOfferVassalage(offerKingdom, out list, out list2);
					if (flag)
					{
						Hero.OneToOneConversationHero.SetHasMet();
						TextObject textObject = TextObject.Empty;
						if (offerKingdom.Leader.GetRelationWithPlayer() < (float)Campaign.Current.Models.DiplomacyModel.MinimumRelationWithConversationCharacterToJoinKingdom)
						{
							textObject = new TextObject("{=niWfuEeh}Well, {PLAYER.NAME}. Are you here about that offer I made? Seeing as what's happened between then and now, surely you realize that that offer no longer stands?", null);
						}
						else if (list.Contains(offerKingdom))
						{
							textObject = new TextObject("{=RACyH7N5}Greetings, {PLAYER.NAME}. I suppose that you're here because of that message I sent you. But we are at war now. I can no longer make that offer to you.", null);
						}
						else if (list2.Intersect(list).Count<IFaction>() != list.Count)
						{
							textObject = new TextObject("{=lynev8Lk}Greetings, {PLAYER.NAME}. I suppose that you're here because of that message I sent you. But the diplomatic situation has changed. You are at war with {WAR_KINGDOMS}, and we are at peace with them. Until that changes, I can no longer accept your fealty.", null);
							List<TextObject> list3 = new List<TextObject>();
							foreach (IFaction faction in list)
							{
								if (!list2.Contains(faction))
								{
									list3.Add(faction.Name);
								}
							}
							textObject.SetTextVariable("WAR_KINGDOMS", GameTexts.GameTextHelper.MergeTextObjectsWithComma(list3, true));
						}
						textObject.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, false);
						MBTextManager.SetTextVariable("INVALID_REASON", textObject, false);
					}
					return flag;
				}
			}
			return false;
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x0012CAEC File Offset: 0x0012ACEC
		private void vassal_conversation_end_consequence()
		{
			CampaignEventDispatcher.Instance.OnVassalOrMercenaryServiceOfferCanceled((Kingdom)Hero.OneToOneConversationHero.MapFaction);
		}

		// Token: 0x04001228 RID: 4648
		private const float MercenaryOfferCreationChance = 0.02f;

		// Token: 0x04001229 RID: 4649
		private const float VassalOfferCreationChance = 0.01f;

		// Token: 0x0400122A RID: 4650
		private const int MercenaryOfferCancelTimeInHours = 48;

		// Token: 0x0400122B RID: 4651
		private static readonly TextObject MercenaryOfferDecisionPopUpExplanationText = new TextObject("{=TENbJKpP}The {OFFERED_KINGDOM_NAME} is offering you work as a mercenary, paying {GOLD_AMOUNT}{GOLD_ICON} per influence point that you would gain from fighting on their behalf. Do you accept?", null);

		// Token: 0x0400122C RID: 4652
		private static readonly TextObject MercenaryOfferPanelNotificationText = new TextObject("{=FA2QZc7Q}A courier arrives, bearing a message from {OFFERED_KINGDOM_LEADER.NAME}. {?OFFERED_KINGDOM_LEADER.GENDER}She{?}He{\\?} is offering you a contract as a mercenary.", null);

		// Token: 0x0400122D RID: 4653
		private static readonly TextObject VassalOfferPanelNotificationText = new TextObject("{=7ouzFASf}A courier arrives, bearing a message from {OFFERED_KINGDOM_LEADER.NAME}. {?OFFERED_KINGDOM_LEADER.GENDER}She{?}He{\\?} remarks on your growing reputation, and asks if you would consider pledging yourself as a vassal of the {OFFERED_KINGDOM_NAME}. You should speak in person if you are interested.", null);

		// Token: 0x0400122E RID: 4654
		private Tuple<Kingdom, CampaignTime> _currentMercenaryOffer;

		// Token: 0x0400122F RID: 4655
		private Dictionary<Kingdom, CampaignTime> _vassalOffers = new Dictionary<Kingdom, CampaignTime>();

		// Token: 0x04001230 RID: 4656
		private bool _stopOffers;
	}
}
