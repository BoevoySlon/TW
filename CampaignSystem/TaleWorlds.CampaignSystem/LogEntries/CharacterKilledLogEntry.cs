﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002D0 RID: 720
	public class CharacterKilledLogEntry : LogEntry, IEncyclopediaLog, IChatNotification, IWarLog
	{
		// Token: 0x06002A62 RID: 10850 RVA: 0x000B5091 File Offset: 0x000B3291
		internal static void AutoGeneratedStaticCollectObjectsCharacterKilledLogEntry(object o, List<object> collectedObjects)
		{
			((CharacterKilledLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000B509F File Offset: 0x000B329F
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Victim);
			collectedObjects.Add(this.Killer);
			collectedObjects.Add(this.VictimMapFaction);
			collectedObjects.Add(this.KillerMapFaction);
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000B50D8 File Offset: 0x000B32D8
		internal static object AutoGeneratedGetMemberValueVictim(object o)
		{
			return ((CharacterKilledLogEntry)o).Victim;
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000B50E5 File Offset: 0x000B32E5
		internal static object AutoGeneratedGetMemberValueKiller(object o)
		{
			return ((CharacterKilledLogEntry)o).Killer;
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000B50F2 File Offset: 0x000B32F2
		internal static object AutoGeneratedGetMemberValue_actionDetail(object o)
		{
			return ((CharacterKilledLogEntry)o)._actionDetail;
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000B5104 File Offset: 0x000B3304
		internal static object AutoGeneratedGetMemberValueVictimMapFaction(object o)
		{
			return ((CharacterKilledLogEntry)o).VictimMapFaction;
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000B5111 File Offset: 0x000B3311
		internal static object AutoGeneratedGetMemberValueKillerMapFaction(object o)
		{
			return ((CharacterKilledLogEntry)o).KillerMapFaction;
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000B511E File Offset: 0x000B331E
		public bool IsVisibleNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06002A6A RID: 10858 RVA: 0x000B5121 File Offset: 0x000B3321
		public override ChatNotificationType NotificationType
		{
			get
			{
				return base.CivilianNotification(this.Victim.Clan);
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000B5134 File Offset: 0x000B3334
		public override CampaignTime KeepInHistoryTime
		{
			get
			{
				return CampaignTime.Never;
			}
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000B513B File Offset: 0x000B333B
		public CharacterKilledLogEntry(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail)
		{
			this.Victim = victim;
			this.Killer = killer;
			this.VictimMapFaction = victim.MapFaction;
			this.KillerMapFaction = ((killer != null) ? killer.MapFaction : null);
			this._actionDetail = detail;
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000B5176 File Offset: 0x000B3376
		public override int AsReasonForEnmity(Hero potentialKiller, Hero potentialRelative)
		{
			if (this.Killer != null && potentialKiller == this.Killer && !potentialRelative.Clan.IsMapFaction && potentialRelative.Clan == this.Victim.Clan)
			{
				return 10;
			}
			return 0;
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x000B51AD File Offset: 0x000B33AD
		public override string ToString()
		{
			return this.GetEncyclopediaText().ToString();
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000B51BC File Offset: 0x000B33BC
		public override TextObject GetHistoricComment(Hero talkTroop)
		{
			if (this.Killer == null)
			{
				return TextObject.Empty;
			}
			ConversationHelper.HeroRefersToHero(talkTroop, this.Victim, true);
			MBTextManager.SetTextVariable("HERO_1", this.Killer.Name, false);
			MBTextManager.SetTextVariable("HERO_2", this.Victim.Name, false);
			return GameTexts.FindText("str_responsible_of_death_news", null);
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000B521C File Offset: 0x000B341C
		public TextObject GetNotificationText()
		{
			TextObject textObject = TextObject.Empty;
			if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge)
			{
				textObject = new TextObject("{=5GSrvawr}{VICTIM.NAME} died of old age. {?VICTIM.GENDER}Her{?}His{\\?} family and friends will remember {?VICTIM.GENDER}her{?}him{\\?}.", null);
				StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (this.Killer != null)
				{
					textObject = GameTexts.FindText("str_responsible_of_death_link_news", null);
					StringHelpers.SetCharacterProperties("HERO_1", this.Killer.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("HERO_2", this.Victim.CharacterObject, textObject, false);
				}
				else
				{
					textObject = GameTexts.FindText("str_murdered_passive_news", null);
					StringHelpers.SetCharacterProperties("HERO_2", this.Victim.CharacterObject, textObject, false);
				}
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
			{
				textObject = new TextObject("{=BhDWm78v}{VICTIM.NAME} has died in battle.", null);
				StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor)
			{
				textObject = GameTexts.FindText("str_notification_maternal_death", null);
				StringHelpers.SetCharacterProperties("MOTHER", this.Victim.CharacterObject, textObject, false);
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.Executed)
			{
				if (this.Killer != null)
				{
					textObject = new TextObject("{=hB8CU9LP}{VICTIM.NAME} has been executed by {KILLER.NAME}.", null);
					StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("KILLER", this.Killer.CharacterObject, textObject, false);
				}
				else
				{
					textObject = new TextObject("{=mwbYdaJr}{VICTIM.NAME} has been executed.", null);
					StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
				}
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.Lost)
			{
				textObject = new TextObject("{=pVkchhqX}{VICTIM.NAME} was lost.", null);
				StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
			}
			return textObject;
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x000B53E3 File Offset: 0x000B35E3
		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase
		{
			return obj == this.Victim || obj == this.Killer;
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000B5404 File Offset: 0x000B3604
		public TextObject GetEncyclopediaText()
		{
			TextObject textObject = TextObject.Empty;
			if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge)
			{
				textObject = new TextObject("{=KWBwCq1Y}{VICTIM.LINK} died of old age.", null);
				StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.Murdered)
			{
				if (this.Killer != null)
				{
					textObject = GameTexts.FindText("str_responsible_of_death_link_news", null);
					StringHelpers.SetCharacterProperties("HERO_1", this.Killer.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("HERO_2", this.Victim.CharacterObject, textObject, false);
				}
				else
				{
					textObject = GameTexts.FindText("str_murdered_passive_news", null);
					StringHelpers.SetCharacterProperties("HERO_2", this.Victim.CharacterObject, textObject, false);
				}
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle)
			{
				if (this.KillerMapFaction != null)
				{
					textObject = new TextObject("{=kknvzzcG}{VICTIM.LINK} died in a battle against {FACTION_LINK}.", null);
					textObject.SetTextVariable("FACTION_LINK", this.KillerMapFaction.EncyclopediaLinkWithName);
				}
				else
				{
					textObject = new TextObject("{=mjSauU7P}{VICTIM.LINK} died in battle.", null);
				}
				StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.DiedInLabor)
			{
				textObject = GameTexts.FindText("str_notification_maternal_death", null);
				StringHelpers.SetCharacterProperties("MOTHER", this.Victim.CharacterObject, textObject, false);
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.Executed)
			{
				if (this.Killer != null)
				{
					textObject = new TextObject("{=b6Spbd9O}{VICTIM.LINK} has been executed by {KILLER.LINK}.", null);
					StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
					StringHelpers.SetCharacterProperties("KILLER", this.Killer.CharacterObject, textObject, false);
				}
				else
				{
					textObject = new TextObject("{=NacogXav}{VICTIM.LINK} has been executed.", null);
					StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
				}
			}
			else if (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.Lost)
			{
				textObject = new TextObject("{=NKTbhIoi}{VICTIM.LINK} was lost.", null);
				StringHelpers.SetCharacterProperties("VICTIM", this.Victim.CharacterObject, textObject, false);
			}
			return textObject;
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000B55F8 File Offset: 0x000B37F8
		public bool IsRelatedToWar(StanceLink stance, out IFaction effector, out IFaction effected)
		{
			IFaction faction = stance.Faction1;
			IFaction faction2 = stance.Faction2;
			effector = this.VictimMapFaction;
			effected = this.KillerMapFaction;
			return (this._actionDetail == KillCharacterAction.KillCharacterActionDetail.DiedInBattle || this._actionDetail == KillCharacterAction.KillCharacterActionDetail.Executed) && effector != null && effected != null && ((faction == this.VictimMapFaction && faction2 == this.KillerMapFaction) || (faction2 == this.VictimMapFaction && faction == this.KillerMapFaction));
		}

		// Token: 0x04000CBA RID: 3258
		[SaveableField(120)]
		public readonly Hero Victim;

		// Token: 0x04000CBB RID: 3259
		[SaveableField(121)]
		public readonly Hero Killer;

		// Token: 0x04000CBC RID: 3260
		[SaveableField(122)]
		private readonly KillCharacterAction.KillCharacterActionDetail _actionDetail;

		// Token: 0x04000CBD RID: 3261
		[SaveableField(124)]
		private readonly IFaction VictimMapFaction;

		// Token: 0x04000CBE RID: 3262
		[SaveableField(125)]
		private readonly IFaction KillerMapFaction;
	}
}
