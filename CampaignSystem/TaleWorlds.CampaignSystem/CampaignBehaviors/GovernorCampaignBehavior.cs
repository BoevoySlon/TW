using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000392 RID: 914
	public class GovernorCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060036E4 RID: 14052 RVA: 0x000F63CC File Offset: 0x000F45CC
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.OnHeroChangedClanEvent.AddNonSerializedListener(this, new Action<Hero, Clan>(this.OnHeroChangedClan));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x000F644C File Offset: 0x000F464C
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.2.0", 66233))
			{
				foreach (Town town in Town.AllFiefs)
				{
					if (town.Governor != null && town != town.Governor.GovernorOf)
					{
						town.Governor = null;
					}
				}
			}
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000F64D0 File Offset: 0x000F46D0
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x000F64DC File Offset: 0x000F46DC
		private void DailyTickSettlement(Settlement settlement)
		{
			if ((settlement.IsTown || settlement.IsCastle) && settlement.Town.Governor != null)
			{
				Hero governor = settlement.Town.Governor;
				if (governor.GetPerkValue(DefaultPerks.Charm.InBloom) && MBRandom.RandomFloat <= DefaultPerks.Charm.InBloom.SecondaryBonus)
				{
					Hero randomElementWithPredicate = settlement.Notables.GetRandomElementWithPredicate((Hero x) => x.IsFemale != governor.IsFemale);
					if (randomElementWithPredicate != null)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(governor.Clan.Leader, randomElementWithPredicate, 1, true);
					}
				}
				if (governor.GetPerkValue(DefaultPerks.Charm.YoungAndRespectful) && MBRandom.RandomFloat <= DefaultPerks.Charm.YoungAndRespectful.SecondaryBonus)
				{
					Hero randomElementWithPredicate2 = settlement.Notables.GetRandomElementWithPredicate((Hero x) => x.IsFemale == governor.IsFemale);
					if (randomElementWithPredicate2 != null)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(governor.Clan.Leader, randomElementWithPredicate2, 1, true);
					}
				}
				if (governor.GetPerkValue(DefaultPerks.Charm.MeaningfulFavors) && MBRandom.RandomFloat <= DefaultPerks.Charm.MeaningfulFavors.SecondaryBonus)
				{
					foreach (Hero hero in settlement.Notables)
					{
						if (hero.Power >= 200f)
						{
							ChangeRelationAction.ApplyRelationChangeBetweenHeroes(settlement.OwnerClan.Leader, hero, 1, true);
						}
					}
				}
				SkillLevelingManager.OnSettlementGoverned(governor, settlement);
			}
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x000F6660 File Offset: 0x000F4860
		private void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			if (hero.GovernorOf != null && hero.GovernorOf.OwnerClan != hero.Clan)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x000F6683 File Offset: 0x000F4883
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x000F6688 File Offset: 0x000F4888
		private void AddDialogs(CampaignGameStarter starter)
		{
			starter.AddPlayerLine("governor_talk_start", "hero_main_options", "governor_talk_start_reply", "{=zBo78JQb}How are things doing here in {GOVERNOR_SETTLEMENT}?", new ConversationSentence.OnConditionDelegate(this.governor_talk_start_on_condition), null, 100, null, null);
			starter.AddDialogLine("governor_talk_start_reply", "governor_talk_start_reply", "lord_pretalk", "{=!}{SETTLEMENT_DESCRIPTION}", new ConversationSentence.OnConditionDelegate(this.governor_talk_start_reply_on_condition), null, 200, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_start", "hero_main_options", "governor_kingdom_creation_reply", "{=EKuB6Ohf}It is time to take a momentous step... It is time to proclaim a new kingdom.", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_start_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_start_on_consequence), 200, new ConversationSentence.OnClickableConditionDelegate(this.governor_talk_kingdom_creation_start_clickable_condition), null);
			starter.AddDialogLine("governor_talk_kingdom_creation_reply", "governor_kingdom_creation_reply", "governor_kingdom_creation_culture_selection", "{=ZyNjXUHc}I am at your command.", null, null, 100, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_culture_selection", "governor_kingdom_creation_culture_selection", "governor_kingdom_creation_culture_selection_options", "{=jxEVSu98}The language of our documents, and our customary laws... Whose should we use?", null, null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_option", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selected", "{CULTURE_OPTION_0}", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_option_0_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_option_0_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_option_2", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selected", "{CULTURE_OPTION_1}", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_option_1_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_option_1_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_option_3", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selected", "{CULTURE_OPTION_2}", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_option_2_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_option_2_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_other", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_culture_selection", "{=kcuNzSvf}I have another people in mind.", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_culture_other_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_culture_other_on_consequence), 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selection_cancel", "governor_kingdom_creation_culture_selection_options", "governor_kingdom_creation_exit", "{=hbzs5tLd}On second thought, perhaps now is not the right time.", null, null, 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_exit_reply", "governor_kingdom_creation_exit", "close_window", "{=ppi6eVos}As you wish.", null, null, 100, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_culture_selected", "governor_kingdom_creation_culture_selected", "governor_kingdom_creation_culture_selected_confirmation", "{=VOtKthQU}Yes. A kingdom using {CULTURE_ADJECTIVE} law would institute the following: {INITIAL_POLICY_NAMES}.", new ConversationSentence.OnConditionDelegate(this.governor_kingdom_creation_culture_selected_on_condition), null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selected_player_reply", "governor_kingdom_creation_culture_selected_confirmation", "governor_kingdom_creation_name_selection", "{=dzXaXKaC}Very well.", null, null, 100, null, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_culture_selected_player_reply_2", "governor_kingdom_creation_culture_selected_confirmation", "governor_kingdom_creation_culture_selection", "{=kTjsx8gN}Perhaps we should choose another set of laws and customs.", null, null, 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_name_selection", "governor_kingdom_creation_name_selection", "governor_kingdom_creation_name_selection_response", "{=wT1ducZX}Now. What will the kingdom be called?", null, null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_name_selection_player", "governor_kingdom_creation_name_selection_response", "governor_kingdom_creation_name_selection_prompted", "{=XRoG766S}I'll name it...", null, new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_name_selection_on_consequence), 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_name_selection_response", "governor_kingdom_creation_name_selection_prompted", "governor_kingdom_creation_name_selected", "{=shf5aY3l}I'm listening...", null, null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_name_selection_cancel", "governor_kingdom_creation_name_selection_response", "governor_kingdom_creation_exit", "{=7HpfrmIU}On a second thought... Now is not the right time to do this.", null, null, 100, null, null);
			starter.AddDialogLine("governor_talk_kingdom_creation_name_selection_final_response", "governor_kingdom_creation_name_selected", "governor_kingdom_creation_finalization", "{=CzJZ5zhT}So it shall be proclaimed throughout your domain. May {KINGDOM_NAME} forever be victorious!", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_finalization_on_condition), null, 100, null);
			starter.AddPlayerLine("governor_talk_kingdom_creation_finalization", "governor_kingdom_creation_finalization", "close_window", "{=VRbbIWNf}So it shall be.", new ConversationSentence.OnConditionDelegate(this.governor_talk_kingdom_creation_finalization_on_condition), new ConversationSentence.OnConsequenceDelegate(this.governor_talk_kingdom_creation_finalization_on_consequence), 100, null, null);
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x000F69ED File Offset: 0x000F4BED
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (victim.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(victim);
			}
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x000F6A00 File Offset: 0x000F4C00
		private bool governor_talk_start_on_condition()
		{
			if (Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.GovernorOf != null && Hero.OneToOneConversationHero.CurrentSettlement != null && Hero.OneToOneConversationHero.CurrentSettlement.IsTown && Hero.OneToOneConversationHero.CurrentSettlement.Town == Hero.OneToOneConversationHero.GovernorOf && Hero.OneToOneConversationHero.GovernorOf.Owner.Owner == Hero.MainHero)
			{
				MBTextManager.SetTextVariable("GOVERNOR_SETTLEMENT", Hero.OneToOneConversationHero.CurrentSettlement.Name, false);
				return true;
			}
			return false;
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x000F6A90 File Offset: 0x000F4C90
		private bool governor_talk_start_reply_on_condition()
		{
			Settlement currentSettlement = Hero.OneToOneConversationHero.CurrentSettlement;
			TextObject textObject = TextObject.Empty;
			switch (currentSettlement.Town.GetProsperityLevel())
			{
			case SettlementComponent.ProsperityLevel.Low:
				textObject = new TextObject("{=rbJEuVKg}Things could certainly be better, my {?HERO.GENDER}lady{?}lord{\\?}. The merchants say business is slow, and the people complain that goods are expensive and in short supply.", null);
				break;
			case SettlementComponent.ProsperityLevel.Mid:
				textObject = new TextObject("{=HgdbSrq9}Things are all right, my {?HERO.GENDER}lady{?}lord{\\?}. The merchants say that they are breaking even, for the most part. Some prices are high, but most of what the people need is available.", null);
				break;
			case SettlementComponent.ProsperityLevel.High:
				textObject = new TextObject("{=8G94SlPD}We are doing well, my {?HERO.GENDER}lady{?}lord{\\?}. The merchants say business is brisk, and everything the people need appears to be in good supply.", null);
				break;
			}
			StringHelpers.SetCharacterProperties("HERO", CharacterObject.PlayerCharacter, textObject, false);
			MBTextManager.SetTextVariable("SETTLEMENT_DESCRIPTION", textObject.ToString(), false);
			return true;
		}

		// Token: 0x060036EE RID: 14062 RVA: 0x000F6B18 File Offset: 0x000F4D18
		private bool governor_talk_kingdom_creation_start_on_condition()
		{
			return Clan.PlayerClan.Kingdom == null && Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.GovernorOf != null && Hero.OneToOneConversationHero.GovernorOf.Settlement.MapFaction == Hero.MainHero.MapFaction;
		}

		// Token: 0x060036EF RID: 14063 RVA: 0x000F6B66 File Offset: 0x000F4D66
		private void governor_talk_kingdom_creation_start_on_consequence()
		{
			this._availablePlayerKingdomCultures.Clear();
			this._availablePlayerKingdomCultures = Campaign.Current.Models.KingdomCreationModel.GetAvailablePlayerKingdomCultures().ToList<CultureObject>();
			this._kingdomCreationCurrentCulturePageIndex = 0;
		}

		// Token: 0x060036F0 RID: 14064 RVA: 0x000F6B9C File Offset: 0x000F4D9C
		private bool governor_talk_kingdom_creation_start_clickable_condition(out TextObject explanation)
		{
			List<TextObject> list;
			bool result = Campaign.Current.Models.KingdomCreationModel.IsPlayerKingdomCreationPossible(out list);
			string text = "";
			foreach (TextObject textObject in list)
			{
				text += textObject;
				if (textObject != list[list.Count - 1])
				{
					text += "\n";
				}
			}
			explanation = new TextObject(text, null);
			return result;
		}

		// Token: 0x060036F1 RID: 14065 RVA: 0x000F6C34 File Offset: 0x000F4E34
		private bool governor_talk_kingdom_creation_culture_option_0_on_condition()
		{
			return this.HandleAvailableCultureConditionAndText(0);
		}

		// Token: 0x060036F2 RID: 14066 RVA: 0x000F6C3D File Offset: 0x000F4E3D
		private bool governor_talk_kingdom_creation_culture_option_1_on_condition()
		{
			return this.HandleAvailableCultureConditionAndText(1);
		}

		// Token: 0x060036F3 RID: 14067 RVA: 0x000F6C46 File Offset: 0x000F4E46
		private bool governor_talk_kingdom_creation_culture_option_2_on_condition()
		{
			return this.HandleAvailableCultureConditionAndText(2);
		}

		// Token: 0x060036F4 RID: 14068 RVA: 0x000F6C50 File Offset: 0x000F4E50
		private bool HandleAvailableCultureConditionAndText(int index)
		{
			int cultureIndex = this.GetCultureIndex(index);
			if (this._availablePlayerKingdomCultures.Count > cultureIndex)
			{
				TextObject textObject = new TextObject("{=mY6DbVfc}The language and laws of {CULTURE_NAME}.", null);
				textObject.SetTextVariable("CULTURE_NAME", FactionHelper.GetInformalNameForFactionCulture(this._availablePlayerKingdomCultures[cultureIndex]));
				MBTextManager.SetTextVariable("CULTURE_OPTION_" + index.ToString(), textObject, false);
				return true;
			}
			return false;
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x000F6CB7 File Offset: 0x000F4EB7
		private int GetCultureIndex(int optionIndex)
		{
			return this._kingdomCreationCurrentCulturePageIndex * 3 + optionIndex;
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x000F6CC3 File Offset: 0x000F4EC3
		private void governor_talk_kingdom_creation_culture_option_0_on_consequence()
		{
			this._kingdomCreationChosenCulture = this._availablePlayerKingdomCultures[this.GetCultureIndex(0)];
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x000F6CDD File Offset: 0x000F4EDD
		private void governor_talk_kingdom_creation_culture_option_1_on_consequence()
		{
			this._kingdomCreationChosenCulture = this._availablePlayerKingdomCultures[this.GetCultureIndex(1)];
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x000F6CF7 File Offset: 0x000F4EF7
		private void governor_talk_kingdom_creation_culture_option_2_on_consequence()
		{
			this._kingdomCreationChosenCulture = this._availablePlayerKingdomCultures[this.GetCultureIndex(2)];
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x000F6D11 File Offset: 0x000F4F11
		private bool governor_talk_kingdom_creation_culture_other_on_condition()
		{
			return this._availablePlayerKingdomCultures.Count > 3;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x000F6D21 File Offset: 0x000F4F21
		private void governor_talk_kingdom_creation_culture_other_on_consequence()
		{
			this._kingdomCreationCurrentCulturePageIndex++;
			if (this._kingdomCreationCurrentCulturePageIndex > MathF.Ceiling((float)this._availablePlayerKingdomCultures.Count / 3f) - 1)
			{
				this._kingdomCreationCurrentCulturePageIndex = 0;
			}
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x000F6D5C File Offset: 0x000F4F5C
		private bool governor_kingdom_creation_culture_selected_on_condition()
		{
			TextObject text = GameTexts.GameTextHelper.MergeTextObjectsWithComma((from t in this._kingdomCreationChosenCulture.DefaultPolicyList
			select t.Name).ToList<TextObject>(), true);
			MBTextManager.SetTextVariable("INITIAL_POLICY_NAMES", text, false);
			MBTextManager.SetTextVariable("CULTURE_ADJECTIVE", FactionHelper.GetAdjectiveForFactionCulture(this._kingdomCreationChosenCulture), false);
			return true;
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x000F6DC8 File Offset: 0x000F4FC8
		private void governor_talk_kingdom_creation_name_selection_on_consequence()
		{
			this._kingdomCreationChosenName = TextObject.Empty;
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=RuaA8t97}Kingdom Name", null).ToString(), string.Empty, true, true, GameTexts.FindText("str_done", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action<string>(this.OnKingdomNameSelectionDone), new Action(this.OnKingdomNameSelectionCancel), false, new Func<string, Tuple<bool, string>>(FactionHelper.IsKingdomNameApplicable), "", ""), false, false);
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x000F6E52 File Offset: 0x000F5052
		private void OnKingdomNameSelectionDone(string chosenName)
		{
			this._kingdomCreationChosenName = new TextObject(chosenName, null);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x000F6E70 File Offset: 0x000F5070
		private void OnKingdomNameSelectionCancel()
		{
			Campaign.Current.ConversationManager.EndConversation();
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x000F6E81 File Offset: 0x000F5081
		private bool governor_talk_kingdom_creation_finalization_on_condition()
		{
			MBTextManager.SetTextVariable("KINGDOM_NAME", this._kingdomCreationChosenName, false);
			return true;
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x000F6E98 File Offset: 0x000F5098
		private void governor_talk_kingdom_creation_finalization_on_consequence()
		{
			Campaign.Current.KingdomManager.CreateKingdom(this._kingdomCreationChosenName, this._kingdomCreationChosenName, this._kingdomCreationChosenCulture, Clan.PlayerClan, this._kingdomCreationChosenCulture.DefaultPolicyList, null, null, null);
		}

		// Token: 0x04001168 RID: 4456
		private const int CultureDialogueOptionCount = 3;

		// Token: 0x04001169 RID: 4457
		private List<CultureObject> _availablePlayerKingdomCultures = new List<CultureObject>();

		// Token: 0x0400116A RID: 4458
		private int _kingdomCreationCurrentCulturePageIndex;

		// Token: 0x0400116B RID: 4459
		private CultureObject _kingdomCreationChosenCulture;

		// Token: 0x0400116C RID: 4460
		private TextObject _kingdomCreationChosenName;
	}
}
