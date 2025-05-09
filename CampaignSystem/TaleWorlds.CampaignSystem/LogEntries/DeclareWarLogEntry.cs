﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002D6 RID: 726
	public class DeclareWarLogEntry : LogEntry, IEncyclopediaLog, IChatNotification, IWarLog
	{
		// Token: 0x06002AB1 RID: 10929 RVA: 0x000B5DEF File Offset: 0x000B3FEF
		internal static void AutoGeneratedStaticCollectObjectsDeclareWarLogEntry(object o, List<object> collectedObjects)
		{
			((DeclareWarLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x000B5DFD File Offset: 0x000B3FFD
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Faction1);
			collectedObjects.Add(this.Faction2);
			collectedObjects.Add(this.Faction1Leader);
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x000B5E2A File Offset: 0x000B402A
		internal static object AutoGeneratedGetMemberValueFaction1(object o)
		{
			return ((DeclareWarLogEntry)o).Faction1;
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x000B5E37 File Offset: 0x000B4037
		internal static object AutoGeneratedGetMemberValueFaction2(object o)
		{
			return ((DeclareWarLogEntry)o).Faction2;
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x000B5E44 File Offset: 0x000B4044
		internal static object AutoGeneratedGetMemberValueFaction1Leader(object o)
		{
			return ((DeclareWarLogEntry)o).Faction1Leader;
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x000B5E51 File Offset: 0x000B4051
		public bool IsVisibleNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06002AB7 RID: 10935 RVA: 0x000B5E54 File Offset: 0x000B4054
		public override ChatNotificationType NotificationType
		{
			get
			{
				return base.AdversityNotification(this.Faction1, this.Faction2);
			}
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000B5E68 File Offset: 0x000B4068
		public DeclareWarLogEntry(IFaction faction1, IFaction faction2)
		{
			this.Faction1 = faction1;
			this.Faction2 = faction2;
			this.Faction1Leader = faction1.Leader;
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000B5E8C File Offset: 0x000B408C
		public bool IsRelatedToWar(StanceLink stance, out IFaction effector, out IFaction effected)
		{
			IFaction faction = stance.Faction1;
			IFaction faction2 = stance.Faction2;
			effector = faction;
			effected = faction2;
			return (faction == this.Faction1 && faction2 == this.Faction2) || (faction == this.Faction2 && faction2 == this.Faction1);
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000B5ED8 File Offset: 0x000B40D8
		public TextObject GetNotificationText()
		{
			Hero hero = this.Faction1Leader ?? this.Faction1.Leader;
			TextObject textObject;
			if (hero != null)
			{
				textObject = GameTexts.FindText("str_factions_declare_war_news", null);
				textObject.SetTextVariable("RULER_NAME", hero.Name);
			}
			else
			{
				textObject = GameTexts.FindText("str_factions_declare_war_news_direct", null);
			}
			textObject.SetTextVariable("FACTION1_NAME", this.Faction1.EncyclopediaLinkWithName);
			textObject.SetTextVariable("FACTION2_NAME", this.Faction2.EncyclopediaLinkWithName);
			return textObject;
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000B5F5C File Offset: 0x000B415C
		public override void GetConversationScoreAndComment(Hero talkTroop, bool findString, out string comment, out ImportanceEnum score)
		{
			score = ImportanceEnum.Zero;
			comment = "";
			if (!this.Faction1.IsEliminated && this.Faction1.Leader.Clan.IsRebelClan && talkTroop.Clan == this.Faction1)
			{
				score = ImportanceEnum.MatterOfLifeAndDeath;
				if (findString)
				{
					comment = "str_comment_we_have_rebelled";
				}
			}
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000B5FB8 File Offset: 0x000B41B8
		public override int GetAsRumor(Settlement talkSettlement, ref TextObject comment)
		{
			int result = 0;
			if (this.Faction1 == talkSettlement.MapFaction)
			{
				comment = new TextObject("{=mrmxEklL}So looks like it's war with {ENEMY_NAME}. Well, I don't deny they deserve it, but it will fall hardest on the poor folk like us.", null);
				comment.SetTextVariable("ENEMY_NAME", FactionHelper.GetTermUsedByOtherFaction(this.Faction2, talkSettlement.MapFaction, false));
				return 10;
			}
			if (this.Faction2 == talkSettlement.MapFaction)
			{
				comment = new TextObject("{=SVebFiHQ}So looks like {ENEMY_NAME} want war with us. Well, I say we show the bastards who we are!", null);
				comment.SetTextVariable("ENEMY_NAME", FactionHelper.GetTermUsedByOtherFaction(this.Faction1, talkSettlement.MapFaction, false));
				return 10;
			}
			return result;
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000B6042 File Offset: 0x000B4242
		public override string ToString()
		{
			return this.GetEncyclopediaText().ToString();
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000B604F File Offset: 0x000B424F
		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase
		{
			return obj == this.Faction1 || obj == this.Faction2;
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000B606F File Offset: 0x000B426F
		public TextObject GetEncyclopediaText()
		{
			return this.GetNotificationText();
		}

		// Token: 0x04000CCE RID: 3278
		[SaveableField(190)]
		public readonly IFaction Faction1;

		// Token: 0x04000CCF RID: 3279
		[SaveableField(191)]
		public readonly IFaction Faction2;

		// Token: 0x04000CD0 RID: 3280
		[SaveableField(192)]
		public readonly Hero Faction1Leader;
	}
}
