﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002CF RID: 719
	public class CharacterInsultedLogEntry : LogEntry, IEncyclopediaLog, IChatNotification
	{
		// Token: 0x06002A52 RID: 10834 RVA: 0x000B4C90 File Offset: 0x000B2E90
		internal static void AutoGeneratedStaticCollectObjectsCharacterInsultedLogEntry(object o, List<object> collectedObjects)
		{
			((CharacterInsultedLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000B4C9E File Offset: 0x000B2E9E
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Insultee);
			collectedObjects.Add(this.Insulter);
			collectedObjects.Add(this._overWhat);
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000B4CCB File Offset: 0x000B2ECB
		internal static object AutoGeneratedGetMemberValueInsultee(object o)
		{
			return ((CharacterInsultedLogEntry)o).Insultee;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000B4CD8 File Offset: 0x000B2ED8
		internal static object AutoGeneratedGetMemberValueInsulter(object o)
		{
			return ((CharacterInsultedLogEntry)o).Insulter;
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000B4CE5 File Offset: 0x000B2EE5
		internal static object AutoGeneratedGetMemberValue_overWhat(object o)
		{
			return ((CharacterInsultedLogEntry)o)._overWhat;
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000B4CF2 File Offset: 0x000B2EF2
		internal static object AutoGeneratedGetMemberValue_gameActionNote(object o)
		{
			return ((CharacterInsultedLogEntry)o)._gameActionNote;
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06002A58 RID: 10840 RVA: 0x000B4D04 File Offset: 0x000B2F04
		public override CampaignTime KeepInHistoryTime
		{
			get
			{
				return CampaignTime.Weeks(240f);
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06002A59 RID: 10841 RVA: 0x000B4D10 File Offset: 0x000B2F10
		public override ChatNotificationType NotificationType
		{
			get
			{
				return base.CivilianNotification(this.Insulter.Clan);
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06002A5A RID: 10842 RVA: 0x000B4D23 File Offset: 0x000B2F23
		public bool IsVisibleNotification
		{
			get
			{
				if (this._overWhat != null)
				{
					return this._overWhat.IsHero;
				}
				return this.Insultee.CharacterObject.IsHero;
			}
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000B4D49 File Offset: 0x000B2F49
		public CharacterInsultedLogEntry(Hero insultee, Hero insulter, CharacterObject overWhat, ActionNotes note)
		{
			this.Insultee = insultee;
			this.Insulter = insulter;
			this._overWhat = overWhat;
			this._gameActionNote = note;
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000B4D70 File Offset: 0x000B2F70
		public override TextObject GetHistoricComment(Hero talkTroop)
		{
			MBTextManager.SetTextVariable("SIDE1TROOP", this.Insultee.Name, false);
			MBTextManager.SetTextVariable("SIDE2TROOP", this.Insulter.Name, false);
			if (talkTroop == this.Insultee)
			{
				return GameTexts.FindText("str_description_insultcharacter_I_insulted_y", null);
			}
			if (talkTroop == this.Insulter)
			{
				return GameTexts.FindText("str_description_insultcharacter_x_insulted_me", null);
			}
			if (talkTroop == null)
			{
				return GameTexts.FindText("str_description_insultcharacter_x_insulted_y", null);
			}
			MBTextManager.SetTextVariable("SIDE1TROOP", ConversationHelper.HeroRefersToHero(talkTroop, this.Insultee, true), false);
			MBTextManager.SetTextVariable("SIDE2TROOP", ConversationHelper.HeroRefersToHero(talkTroop, this.Insulter, false), false);
			if (this._gameActionNote == ActionNotes.CourtshipQuarrel)
			{
				Hero heroObject = this._overWhat.HeroObject;
				MBTextManager.SetTextVariable("SUBJECT", ConversationHelper.HeroRefersToHero(talkTroop, heroObject, false), false);
				return GameTexts.FindText("str_description_insultcharacter_x_insulted_y_courtship", null);
			}
			return GameTexts.FindText("str_description_insultcharacter_x_insulted_y", null);
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000B4E51 File Offset: 0x000B3051
		public override int AsReasonForEnmity(Hero referenceHero1, Hero referenceHero2)
		{
			if (referenceHero1 == this.Insultee && referenceHero2 == this.Insulter)
			{
				return 5;
			}
			if (referenceHero2 == this.Insultee && referenceHero1 == this.Insulter)
			{
				return 5;
			}
			return 0;
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000B4E7C File Offset: 0x000B307C
		public override string ToString()
		{
			return this.GetEncyclopediaText().ToString();
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000B4E8C File Offset: 0x000B308C
		public TextObject GetNotificationText()
		{
			TextObject textObject = GameTexts.FindText("str_notification_quarrel", null);
			StringHelpers.SetCharacterProperties("LORD", this.Insultee.CharacterObject, textObject, false);
			StringHelpers.SetCharacterProperties("OTHER_LORD", this.Insulter.CharacterObject, textObject, false);
			return textObject;
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000B4ED6 File Offset: 0x000B30D6
		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase
		{
			return obj == this.Insultee || obj == this.Insulter;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000B4EF8 File Offset: 0x000B30F8
		public TextObject GetEncyclopediaText()
		{
			TextObject textObject = TextObject.Empty;
			if (this._gameActionNote == ActionNotes.CourtshipQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_courtship", null);
			}
			if (this._gameActionNote == ActionNotes.ValorStrategyQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_setback_valor", null);
			}
			if (this._gameActionNote == ActionNotes.CalculatingStrategyQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_setback_calculating", null);
			}
			if (this._gameActionNote == ActionNotes.ResponsibilityStrategyQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_setback_responsibility", null);
			}
			if (this._gameActionNote == ActionNotes.LandCheatingQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_land", null);
			}
			if (this._gameActionNote == ActionNotes.TroublemakerQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_troublemaker", null);
			}
			if (this._gameActionNote == ActionNotes.HereticQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_heretic", null);
			}
			if (this._gameActionNote == ActionNotes.RuthlessBusinessQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_ruthless_business", null);
			}
			if (this._gameActionNote == ActionNotes.DishonestBusinessQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_dishonest_business", null);
			}
			if (this._gameActionNote == ActionNotes.ExtortingQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_extorting", null);
			}
			if (this._gameActionNote == ActionNotes.VengeanceQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_vengeance", null);
			}
			if (this._gameActionNote == ActionNotes.FiefQuarrel)
			{
				textObject = GameTexts.FindText("str_insult_news_fief", null);
			}
			if (textObject != TextObject.Empty)
			{
				StringHelpers.SetCharacterProperties("INSULTER", this.Insultee.CharacterObject, textObject, false);
				StringHelpers.SetCharacterProperties("INSULTEE", this.Insulter.CharacterObject, textObject, false);
				return textObject;
			}
			textObject = new TextObject("{=v7sfiv5m}{INSULT_NEWS} {GAME_ACTION_NOTES}", null);
			textObject.SetTextVariable("INSULT_NEWS", GameTexts.FindText("str_insult_news", null));
			textObject.SetTextVariable("GAME_ACTION_NOTES", GameTexts.FindText("str_game_action_note", this._gameActionNote.ToString()));
			return textObject;
		}

		// Token: 0x04000CB6 RID: 3254
		[SaveableField(110)]
		public readonly Hero Insultee;

		// Token: 0x04000CB7 RID: 3255
		[SaveableField(111)]
		public readonly Hero Insulter;

		// Token: 0x04000CB8 RID: 3256
		[SaveableField(112)]
		private readonly CharacterObject _overWhat;

		// Token: 0x04000CB9 RID: 3257
		[SaveableField(113)]
		private readonly ActionNotes _gameActionNote;
	}
}
