using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003C0 RID: 960
	public class PerkResetCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003A81 RID: 14977 RVA: 0x001141F6 File Offset: 0x001123F6
		public int PerkResetCost
		{
			get
			{
				if (this._selectedSkillForReset == null)
				{
					return 0;
				}
				return this._heroForPerkReset.GetSkillValue(this._selectedSkillForReset) * 40;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06003A82 RID: 14978 RVA: 0x00114216 File Offset: 0x00112416
		public bool HasEnoughSkillValueForReset
		{
			get
			{
				return this._selectedSkillForReset != null && this._heroForPerkReset.GetSkillValue(this._selectedSkillForReset) >= 25;
			}
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x0011423A File Offset: 0x0011243A
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x0011426A File Offset: 0x0011246A
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_warningTime", ref this._warningTime);
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x0011427E File Offset: 0x0011247E
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x00114288 File Offset: 0x00112488
		protected void AddDialogs(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddDialogLine("arena_intro_7", "arena_intro_perk_reset", "arena_intro_4", "{=ocIutUyu}Also, here at the arena, we think a lot about the arts of war - and many other related skills as well. Often you pick up certain habits while learning your skills. If you need to change those, to practice one of your skills in a certain way, we can help you.", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_player_perk_reset", "arena_master_talk", "arena_master_ask_retrain", "{=Y7tz9D28}These teachers who help people hone their skills and learn new habits... Can you help me find one?", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_ask_retrain", "arena_master_ask_retrain", "arena_master_choose_hero", "{=NyWXSHH2}Of course. Was this for you, or someone else?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_player_perk_reset_2", "arena_master_choose_hero", "arena_master_reset_attribute", "{=3VxA6HaZ}This is for me.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_player_for_perk_reset_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_clan_member_perk_reset", "arena_master_choose_hero", "arena_master_reset_attribute", "{=1OKEl18y}This is for {COMPANION.NAME}", new ConversationSentence.OnConditionDelegate(this.conversation_player_has_single_clan_member_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_player_has_single_clan_member_on_consequence), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_ask_clan_member_perk_reset_2", "arena_master_choose_hero", "arena_master_retrain_ask_clan_members", "{=GvcotJmH}I would like you to help hone the skills of a member of my clan.", new ConversationSentence.OnConditionDelegate(this.conversation_player_has_multiple_clan_members_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_list_clan_members_on_condition), 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_retrain_ask_clan_member", "arena_master_retrain_ask_clan_members", "arena_master_select_clan_member", "{=WRwA0VVS}Which one of your clan members did you wish me to retrain?", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("arena_master_select_clan_member", "arena_master_select_clan_member", "arena_master_reset_attribute", "{=!}{CLAN_MEMBER.NAME}", "{=ElG1LnCA}I am thinking of someone else.", "arena_master_retrain_ask_clan_members", new ConversationSentence.OnConditionDelegate(this.conversation_arena_player_select_clan_member_multiple_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_clan_member_for_perk_reset_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_select_clan_member_cancel", "arena_master_select_clan_member", "arena_master_pre_talk", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_reset_attribute", "arena_master_reset_attribute", "arena_master_select_attribute", "{=95jXfam8}What kind of skill is this, speaking broadly? What trait would you say it reflects?", null, null, 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("arena_master_select_attribute", "arena_master_select_attribute", "arena_master_reset_perks", "{=!}{ATTRIBUTE_NAME}", "{=0G8Q3AZv}I am thinking of a different attribute.", "arena_master_reset_attribute", new ConversationSentence.OnConditionDelegate(this.conversation_arena_player_select_attribute_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_attribute_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_select_attribute_cancel", "arena_master_select_attribute", "arena_master_pre_talk", "{=g0JOQQl0}I don't want to do this right now.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_reset_perks", "arena_master_reset_perks", "arena_master_select_skill", "{=pGyO41lb}Yes, I can do that. What skill exactly do you have in mind?", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_set_skills_for_reset_on_consequence), 100, null);
			campaignGameStarter.AddRepeatablePlayerLine("arena_master_select_skill", "arena_master_select_skill", "arena_master_pay_for_reset", "{=8PV1oB9W}I wish to focus on {SKILL_NAME}.", "{=Z9pq58h4}I am thinking of a different skill.", "arena_master_reset_perks", new ConversationSentence.OnConditionDelegate(this.conversation_arena_player_select_skill_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_select_skill_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_select_skill_cancel", "arena_master_select_skill", "arena_master_reset_attribute", "{=CH7b5LaX}I have changed my mind.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_list_perks_on_condition), 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_pay_for_reset", "arena_master_pay_for_reset", "arena_master_accept_perk_reset", "{=q3J9Wb8N}If you can afford to pay {GOLD_AMOUNT} {GOLD_ICON} for it, I can teach you right now. Are you sure you want to go through with it?", new ConversationSentence.OnConditionDelegate(this.conversation_arena_ask_price_on_condition), null, 100, null);
			campaignGameStarter.AddDialogLine("arena_master_selected_skill_invalid", "arena_master_pay_for_reset", "arena_master_reset_attribute", "{=!}{NOT_ENOUGH_SKILL_TEXT}", new ConversationSentence.OnConditionDelegate(this.conversation_arena_skill_not_developed_enough_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_skill_not_developed_enough_on_consequence), 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_accept_perk_reset1", "arena_master_accept_perk_reset", "arena_master_perk_reset_closure", "{=Q0UjYw7V}Yes I am sure.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_player_accept_perk_reset_on_consequence), 100, new ConversationSentence.OnClickableConditionDelegate(this.conversation_arena_player_accept_price), null);
			campaignGameStarter.AddPlayerLine("arena_master_reject_perk_reset2", "arena_master_accept_perk_reset", "arena_master_pre_talk", "{=SLaJsVnq}Actually, I have changed my mind.", null, null, 100, null, null);
			campaignGameStarter.AddDialogLine("arena_master_perk_reset_closure", "arena_master_perk_reset_closure", "arena_master_perk_reset_final", "{=IsBVxopm}Excellent! Is there anything else I can help you with?", null, null, 100, null);
			campaignGameStarter.AddPlayerLine("arena_master_perk_reset_final1", "arena_master_perk_reset_final", "arena_master_reset_attribute", "{=aCGgBilx}I would like help fine-tuning another skill.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_train_another_skill_on_condition), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_perk_reset_final2", "arena_master_perk_reset_final", "arena_master_retrain_ask_clan_members", "{=c4tfVgqb}I would like you to help another member of my clan hone their skills.", new ConversationSentence.OnConditionDelegate(this.conversation_player_has_multiple_clan_members_on_condition), new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_train_another_clan_member_on_condition), 100, null, null);
			campaignGameStarter.AddPlayerLine("arena_master_perk_reset_final3", "arena_master_perk_reset_final", "arena_master_pre_talk", "{=Dz7E79QP}You have already helped enough. Thank you.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_arena_finish_perk_reset_dialogs_on_consequence), 100, null, null);
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x00114691 File Offset: 0x00112891
		private void conversation_player_has_single_clan_member_on_consequence()
		{
			this._heroForPerkReset = this.GetClanMembersInParty()[0];
			this.SetAttributesForDialog();
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001146AB File Offset: 0x001128AB
		private void conversation_arena_skill_not_developed_enough_on_consequence()
		{
			this.SetAttributesForDialog();
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001146B4 File Offset: 0x001128B4
		private bool conversation_arena_skill_not_developed_enough_on_condition()
		{
			TextObject textObject;
			if (this._heroForPerkReset == Hero.MainHero)
			{
				textObject = new TextObject("{=FN3xNnd1}You really don't have much experience in this skill, I can't help you much. Maybe we can work on something else?", null);
			}
			else
			{
				textObject = new TextObject("{=wGAmNQGE}{CHARACTER.NAME} does not have much experience in this skill, I can't help {?CHARACTER.GENDER}her{?}him{\\?} much. Maybe we can work on something else?", null);
				textObject.SetCharacterProperties("CHARACTER", this._heroForPerkReset.CharacterObject, false);
			}
			MBTextManager.SetTextVariable("NOT_ENOUGH_SKILL_TEXT", textObject, false);
			return !this.HasEnoughSkillValueForReset;
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x00114714 File Offset: 0x00112914
		private void conversation_arena_finish_perk_reset_dialogs_on_consequence()
		{
			this._heroForPerkReset = null;
			this._attributeForPerkReset = null;
			this._selectedSkillForReset = null;
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x0011472B File Offset: 0x0011292B
		private void conversation_arena_train_another_skill_on_condition()
		{
			this.SetAttributesForDialog();
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x00114733 File Offset: 0x00112933
		private void conversation_arena_train_another_clan_member_on_condition()
		{
			this.SetHeroesForDialog();
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x0011473B File Offset: 0x0011293B
		private void conversation_arena_player_accept_perk_reset_on_consequence()
		{
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, this.PerkResetCost, false);
			this.ResetPerkTreeForHero(this._heroForPerkReset, this._selectedSkillForReset);
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x00114764 File Offset: 0x00112964
		private bool conversation_arena_player_accept_price(out TextObject explanation)
		{
			if (Hero.MainHero.Gold < this.PerkResetCost)
			{
				explanation = new TextObject("{=QOWyEJrm}You don't have enough denars.", null);
				return false;
			}
			explanation = new TextObject("{=ePmSvu1s}{AMOUNT}{GOLD_ICON}", null);
			explanation.SetTextVariable("AMOUNT", this.PerkResetCost);
			return true;
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x001147B3 File Offset: 0x001129B3
		private void conversation_arena_player_select_skill_on_consequence()
		{
			this._selectedSkillForReset = (ConversationSentence.SelectedRepeatObject as SkillObject);
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x001147C5 File Offset: 0x001129C5
		private bool conversation_arena_ask_price_on_condition()
		{
			if (this.HasEnoughSkillValueForReset)
			{
				MBTextManager.SetTextVariable("GOLD_AMOUNT", this.PerkResetCost);
				MBTextManager.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">", false);
				return true;
			}
			return false;
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x001147F4 File Offset: 0x001129F4
		private bool conversation_arena_player_select_skill_on_condition()
		{
			SkillObject skillObject = ConversationSentence.CurrentProcessedRepeatObject as SkillObject;
			if (skillObject != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("SKILL_NAME", skillObject.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x00114828 File Offset: 0x00112A28
		private void conversation_arena_set_skills_for_reset_on_consequence()
		{
			this.SetSkillsForDialog();
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00114830 File Offset: 0x00112A30
		private void conversation_arena_list_perks_on_condition()
		{
			this.SetAttributesForDialog();
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x00114838 File Offset: 0x00112A38
		private void conversation_arena_player_select_attribute_on_consequence()
		{
			this._attributeForPerkReset = (ConversationSentence.SelectedRepeatObject as CharacterAttribute);
			this.SetSkillsForDialog();
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x00114850 File Offset: 0x00112A50
		private bool conversation_arena_player_select_attribute_on_condition()
		{
			CharacterAttribute characterAttribute = ConversationSentence.CurrentProcessedRepeatObject as CharacterAttribute;
			if (characterAttribute != null)
			{
				ConversationSentence.SelectedRepeatLine.SetTextVariable("ATTRIBUTE_NAME", characterAttribute.Name);
				return true;
			}
			return false;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x00114884 File Offset: 0x00112A84
		private void conversation_arena_player_select_clan_member_for_perk_reset_on_consequence()
		{
			this._heroForPerkReset = (ConversationSentence.SelectedRepeatObject as Hero);
			this.SetAttributesForDialog();
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x0011489C File Offset: 0x00112A9C
		private void conversation_arena_player_select_player_for_perk_reset_on_consequence()
		{
			this._heroForPerkReset = Hero.MainHero;
			this.SetAttributesForDialog();
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x001148AF File Offset: 0x00112AAF
		private void conversation_arena_list_clan_members_on_condition()
		{
			this.SetHeroesForDialog();
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x001148B8 File Offset: 0x00112AB8
		private bool conversation_arena_player_select_clan_member_multiple_on_condition()
		{
			Hero hero = ConversationSentence.CurrentProcessedRepeatObject as Hero;
			if (hero != null)
			{
				ConversationSentence.SelectedRepeatLine.SetCharacterProperties("CLAN_MEMBER", hero.CharacterObject, false);
				return true;
			}
			return false;
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x001148EC File Offset: 0x00112AEC
		private bool conversation_player_has_multiple_clan_members_on_condition()
		{
			return this.GetClanMembersInParty().Count > 1;
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x001148FC File Offset: 0x00112AFC
		private bool conversation_player_has_single_clan_member_on_condition()
		{
			List<Hero> clanMembersInParty = this.GetClanMembersInParty();
			if (clanMembersInParty.Count == 1)
			{
				StringHelpers.SetCharacterProperties("COMPANION", clanMembersInParty[0].CharacterObject, null, false);
				return true;
			}
			return false;
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x00114938 File Offset: 0x00112B38
		private void DailyTick()
		{
			if (Clan.PlayerClan.Companions.Count > Clan.PlayerClan.CompanionLimit)
			{
				if (!(this._warningTime != CampaignTime.Zero))
				{
					this.WarnPlayerAboutCompanionLimit();
					return;
				}
				if (this._warningTime.ElapsedDaysUntilNow > 6f)
				{
					this.RemoveACompanionFromPlayerParty();
					return;
				}
			}
			else
			{
				this._warningTime = CampaignTime.Zero;
			}
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x0011499D File Offset: 0x00112B9D
		private void SetHeroesForDialog()
		{
			ConversationSentence.SetObjectsToRepeatOver(this.GetClanMembersInParty(), 5);
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x001149AB File Offset: 0x00112BAB
		private void SetAttributesForDialog()
		{
			ConversationSentence.SetObjectsToRepeatOver(Attributes.All.ToList<CharacterAttribute>(), 5);
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x001149BD File Offset: 0x00112BBD
		private void SetSkillsForDialog()
		{
			ConversationSentence.SetObjectsToRepeatOver(this._attributeForPerkReset.Skills.ToList<SkillObject>(), 5);
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x001149D5 File Offset: 0x00112BD5
		private void ResetPerkTreeForHero(Hero hero, SkillObject skill)
		{
			this.ClearkPerksForSkill(hero, skill);
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x001149E0 File Offset: 0x00112BE0
		private void ClearPermanentBonusesIfExists(Hero hero, PerkObject perk)
		{
			if (!hero.GetPerkValue(perk))
			{
				return;
			}
			if (perk == DefaultPerks.Crafting.VigorousSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.StrongSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.EnduringSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.WeaponMasterSmith)
			{
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.OneHanded, 1);
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.TwoHanded, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Durable)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Steady)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Strong)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
			}
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x00114AC0 File Offset: 0x00112CC0
		private void ClearkPerksForSkill(Hero hero, SkillObject skill)
		{
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if (perkObject.Skill == skill)
				{
					this.ClearPermanentBonusesIfExists(hero, perkObject);
					hero.SetPerkValueInternal(perkObject, false);
				}
			}
			PartyBase.MainParty.MemberRoster.UpdateVersion();
			hero.HitPoints = MathF.Min(hero.HitPoints, hero.MaxHitPoints);
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00114B4C File Offset: 0x00112D4C
		private void RemoveACompanionFromPlayerParty()
		{
			int count = Clan.PlayerClan.Companions.Count;
			int num = MBRandom.RandomInt(count);
			for (int i = 0; i < count; i++)
			{
				int index = (i + num) % count;
				Hero hero = Clan.PlayerClan.Companions[index];
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
				{
					Settlement currentSettlement = hero.CurrentSettlement;
					if (((currentSettlement != null) ? currentSettlement.Party.MapEvent : null) == null && !Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
					{
						KillCharacterAction.ApplyByRemove(hero, true, true);
						return;
					}
				}
			}
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x00114BE5 File Offset: 0x00112DE5
		private void WarnPlayerAboutCompanionLimit()
		{
			MBInformationManager.AddQuickInformation(new TextObject("{=xDikJxbO}Your party is above your companion limits. Due to that some of the companions might leave soon.", null), 0, null, "event:/ui/notification/relation");
			this._warningTime = CampaignTime.Now;
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x00114C0C File Offset: 0x00112E0C
		private List<Hero> GetClanMembersInParty()
		{
			return (from m in PartyBase.MainParty.MemberRoster.GetTroopRoster()
			where m.Character.IsHero && m.Character.HeroObject.Clan == Clan.PlayerClan && !m.Character.HeroObject.IsHumanPlayerCharacter
			select m into t
			select t.Character.HeroObject).ToList<Hero>();
		}

		// Token: 0x040011C0 RID: 4544
		private Hero _heroForPerkReset;

		// Token: 0x040011C1 RID: 4545
		private CharacterAttribute _attributeForPerkReset;

		// Token: 0x040011C2 RID: 4546
		private SkillObject _selectedSkillForReset;

		// Token: 0x040011C3 RID: 4547
		private CampaignTime _warningTime;
	}
}
