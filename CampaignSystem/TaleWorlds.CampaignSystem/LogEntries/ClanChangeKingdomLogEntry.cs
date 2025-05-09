﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002D3 RID: 723
	public class ClanChangeKingdomLogEntry : LogEntry, IChatNotification, IWarLog
	{
		// Token: 0x06002A92 RID: 10898 RVA: 0x000B58CC File Offset: 0x000B3ACC
		internal static void AutoGeneratedStaticCollectObjectsClanChangeKingdomLogEntry(object o, List<object> collectedObjects)
		{
			((ClanChangeKingdomLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000B58DA File Offset: 0x000B3ADA
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Clan);
			collectedObjects.Add(this.OldKingdom);
			collectedObjects.Add(this.NewKingdom);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000B5907 File Offset: 0x000B3B07
		internal static object AutoGeneratedGetMemberValueClan(object o)
		{
			return ((ClanChangeKingdomLogEntry)o).Clan;
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000B5914 File Offset: 0x000B3B14
		internal static object AutoGeneratedGetMemberValueOldKingdom(object o)
		{
			return ((ClanChangeKingdomLogEntry)o).OldKingdom;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000B5921 File Offset: 0x000B3B21
		internal static object AutoGeneratedGetMemberValueNewKingdom(object o)
		{
			return ((ClanChangeKingdomLogEntry)o).NewKingdom;
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000B592E File Offset: 0x000B3B2E
		internal static object AutoGeneratedGetMemberValue_byRebellion(object o)
		{
			return ((ClanChangeKingdomLogEntry)o)._byRebellion;
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06002A98 RID: 10904 RVA: 0x000B5940 File Offset: 0x000B3B40
		public bool IsVisibleNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06002A99 RID: 10905 RVA: 0x000B5943 File Offset: 0x000B3B43
		public override ChatNotificationType NotificationType
		{
			get
			{
				return base.MilitaryNotification(this.NewKingdom, this.OldKingdom);
			}
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000B5957 File Offset: 0x000B3B57
		public ClanChangeKingdomLogEntry(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, bool byRebellion)
		{
			this.Clan = clan;
			this.OldKingdom = oldKingdom;
			this.NewKingdom = newKingdom;
			this._byRebellion = byRebellion;
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000B597C File Offset: 0x000B3B7C
		public bool IsRelatedToWar(StanceLink stance, out IFaction effector, out IFaction effected)
		{
			IFaction faction = stance.Faction1;
			IFaction faction2 = stance.Faction2;
			Kingdom newKingdom = this.NewKingdom;
			effector = ((newKingdom != null) ? newKingdom.MapFaction : null);
			Kingdom oldKingdom = this.OldKingdom;
			effected = ((oldKingdom != null) ? oldKingdom.MapFaction : null);
			return (this.OldKingdom == faction && this.NewKingdom == faction2) || (this.OldKingdom == faction2 && this.NewKingdom == faction);
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000B59E8 File Offset: 0x000B3BE8
		public TextObject GetNotificationText()
		{
			if (this.NewKingdom != null)
			{
				TextObject textObject = GameTexts.FindText("str_notification_change_faction", null);
				textObject.SetTextVariable("FACTION", this.NewKingdom.InformalName);
				textObject.SetTextVariable("CLAN_NAME", this.Clan.Name);
				return textObject;
			}
			return TextObject.Empty;
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000B5A3C File Offset: 0x000B3C3C
		public override ImportanceEnum GetImportanceForClan(Clan clan)
		{
			if (clan == this.Clan)
			{
				return ImportanceEnum.ReasonablyImportant;
			}
			if (clan.Kingdom != null && (clan.Kingdom == this.OldKingdom || clan.Kingdom == this.NewKingdom))
			{
				return ImportanceEnum.SomewhatImportant;
			}
			return ImportanceEnum.Zero;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000B5A70 File Offset: 0x000B3C70
		public override void GetConversationScoreAndComment(Hero talkTroop, bool findString, out string comment, out ImportanceEnum score)
		{
			score = ImportanceEnum.Zero;
			comment = "";
			if (this.Clan == Clan.PlayerClan && talkTroop.IsLord)
			{
				StringHelpers.SetCharacterProperties("TT_LIEGE", talkTroop.MapFaction.Leader.CharacterObject, null, false);
				if (talkTroop.MapFaction == this.NewKingdom && Hero.MainHero.MapFaction == talkTroop.MapFaction && Hero.MainHero != this.NewKingdom.Leader && talkTroop != this.NewKingdom.Leader)
				{
					score = ImportanceEnum.ReasonablyImportant;
					if (findString)
					{
						if (Hero.MainHero.Clan.IsUnderMercenaryService)
						{
							comment = "str_comment_changeherofaction_you_joined_us_as_mercenary";
							return;
						}
						this.NewKingdom.Leader.SetTextVariables();
						comment = "str_comment_changeherofaction_you_joined_us";
						return;
					}
				}
				else if (talkTroop.MapFaction == this.OldKingdom && Hero.MainHero.MapFaction != talkTroop.MapFaction && this._byRebellion)
				{
					score = ImportanceEnum.QuiteImportant;
					if (findString)
					{
						this.OldKingdom.Leader.SetTextVariables();
						if (this.OldKingdom.Leader == talkTroop)
						{
							comment = "str_comment_changeherofaction_you_rebelled_against_me";
							return;
						}
						comment = "str_comment_changeherofaction_you_rebelled_against_us";
						score = ImportanceEnum.VeryImportant;
						return;
					}
				}
				else if (talkTroop.MapFaction == this.OldKingdom && Hero.MainHero.MapFaction != talkTroop.MapFaction && talkTroop != this.OldKingdom.Leader)
				{
					score = ImportanceEnum.ReasonablyImportant;
					if (findString)
					{
						comment = "str_comment_changeherofaction_you_renounced_your_allegiance";
					}
				}
			}
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000B5BD8 File Offset: 0x000B3DD8
		public override string ToString()
		{
			return this.GetNotificationText().ToString();
		}

		// Token: 0x04000CC5 RID: 3269
		[SaveableField(150)]
		public readonly Clan Clan;

		// Token: 0x04000CC6 RID: 3270
		[SaveableField(151)]
		public readonly Kingdom OldKingdom;

		// Token: 0x04000CC7 RID: 3271
		[SaveableField(152)]
		public readonly Kingdom NewKingdom;

		// Token: 0x04000CC8 RID: 3272
		[SaveableField(153)]
		private readonly bool _byRebellion;
	}
}
