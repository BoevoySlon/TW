﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002E4 RID: 740
	public class TakePrisonerLogEntry : LogEntry, IEncyclopediaLog, IChatNotification, IWarLog
	{
		// Token: 0x06002B50 RID: 11088 RVA: 0x000B7AA8 File Offset: 0x000B5CA8
		internal static void AutoGeneratedStaticCollectObjectsTakePrisonerLogEntry(object o, List<object> collectedObjects)
		{
			((TakePrisonerLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x000B7AB8 File Offset: 0x000B5CB8
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.CapturerPartyMapFaction);
			collectedObjects.Add(this.Prisoner);
			collectedObjects.Add(this.CapturerSettlement);
			collectedObjects.Add(this.CapturerMobilePartyLeader);
			collectedObjects.Add(this.CapturerHero);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x000B7B08 File Offset: 0x000B5D08
		internal static object AutoGeneratedGetMemberValueCapturerPartyMapFaction(object o)
		{
			return ((TakePrisonerLogEntry)o).CapturerPartyMapFaction;
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000B7B15 File Offset: 0x000B5D15
		internal static object AutoGeneratedGetMemberValuePrisoner(object o)
		{
			return ((TakePrisonerLogEntry)o).Prisoner;
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000B7B22 File Offset: 0x000B5D22
		internal static object AutoGeneratedGetMemberValueCapturerSettlement(object o)
		{
			return ((TakePrisonerLogEntry)o).CapturerSettlement;
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000B7B2F File Offset: 0x000B5D2F
		internal static object AutoGeneratedGetMemberValueCapturerMobilePartyLeader(object o)
		{
			return ((TakePrisonerLogEntry)o).CapturerMobilePartyLeader;
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000B7B3C File Offset: 0x000B5D3C
		internal static object AutoGeneratedGetMemberValueCapturerHero(object o)
		{
			return ((TakePrisonerLogEntry)o).CapturerHero;
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002B57 RID: 11095 RVA: 0x000B7B49 File Offset: 0x000B5D49
		public override CampaignTime KeepInHistoryTime
		{
			get
			{
				return CampaignTime.Weeks(12f);
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000B7B55 File Offset: 0x000B5D55
		public bool IsVisibleNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06002B59 RID: 11097 RVA: 0x000B7B58 File Offset: 0x000B5D58
		public override ChatNotificationType NotificationType
		{
			get
			{
				Hero capturerHero = this.CapturerHero;
				IFaction faction = (capturerHero != null) ? capturerHero.Clan : null;
				return base.MilitaryNotification(faction ?? this.CapturerPartyMapFaction, this.Prisoner.Clan);
			}
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000B7B94 File Offset: 0x000B5D94
		public TakePrisonerLogEntry(PartyBase capturerParty, Hero prisoner)
		{
			this.CapturerPartyMapFaction = capturerParty.MapFaction;
			this.CapturerHero = capturerParty.LeaderHero;
			MobileParty mobileParty = capturerParty.MobileParty;
			this.CapturerMobilePartyLeader = ((mobileParty != null) ? mobileParty.LeaderHero : null);
			this.CapturerSettlement = capturerParty.Settlement;
			this.Prisoner = prisoner;
		}

		// Token: 0x06002B5B RID: 11099 RVA: 0x000B7BEC File Offset: 0x000B5DEC
		public bool IsRelatedToWar(StanceLink stance, out IFaction effector, out IFaction effected)
		{
			IFaction faction = stance.Faction1;
			IFaction faction2 = stance.Faction2;
			effector = this.CapturerPartyMapFaction.MapFaction;
			effected = this.Prisoner.MapFaction;
			return (this.CapturerPartyMapFaction == faction && this.Prisoner.MapFaction == faction2) || (this.CapturerPartyMapFaction == faction2 && this.Prisoner.MapFaction == faction);
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000B7C53 File Offset: 0x000B5E53
		public override string ToString()
		{
			return this.GetNotificationText().ToString();
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000B7C60 File Offset: 0x000B5E60
		public TextObject GetNotificationText()
		{
			TextObject textObject = new TextObject("{=QRJQ9Wgv}{PRISONER_LORD.LINK}{?PRISONER_LORD_HAS_FACTION_LINK} of the {PRISONER_LORD_FACTION_LINK}{?}{\\?} has been taken prisoner by the {CAPTOR_FACTION}.", null);
			if (this.CapturerHero != null)
			{
				textObject = new TextObject("{=Ebb7aH3T}{PRISONER_LORD.LINK}{?PRISONER_LORD_HAS_FACTION_LINK} of the {PRISONER_LORD_FACTION_LINK}{?}{\\?} has been taken prisoner by {CAPTURER_LORD.LINK}{?CAPTURER_LORD_HAS_FACTION_LINK} of the {CAPTURER_LORD_FACTION_LINK}{?}{\\?}.", null);
				StringHelpers.SetCharacterProperties("CAPTURER_LORD", this.CapturerHero.CharacterObject, textObject, false);
				Clan clan = this.CapturerHero.Clan;
				if (clan != null && !clan.IsMinorFaction)
				{
					textObject.SetTextVariable("CAPTURER_LORD_FACTION_LINK", this.CapturerHero.MapFaction.EncyclopediaLinkWithName);
					textObject.SetTextVariable("CAPTURER_LORD_HAS_FACTION_LINK", 1);
				}
			}
			textObject.SetTextVariable("CAPTOR_FACTION", this.CapturerPartyMapFaction.InformalName);
			StringHelpers.SetCharacterProperties("PRISONER_LORD", this.Prisoner.CharacterObject, textObject, false);
			Clan clan2 = this.Prisoner.Clan;
			if (clan2 != null && !clan2.IsMinorFaction)
			{
				textObject.SetTextVariable("PRISONER_LORD_FACTION_LINK", this.Prisoner.MapFaction.EncyclopediaLinkWithName);
				textObject.SetTextVariable("PRISONER_LORD_HAS_FACTION_LINK", 1);
			}
			return textObject;
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x000B7D5F File Offset: 0x000B5F5F
		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase
		{
			return obj == this.Prisoner || (this.CapturerSettlement != null && obj == this.CapturerSettlement) || (this.CapturerMobilePartyLeader != null && obj == this.CapturerMobilePartyLeader);
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000B7D9F File Offset: 0x000B5F9F
		public TextObject GetEncyclopediaText()
		{
			return this.GetNotificationText();
		}

		// Token: 0x04000CFC RID: 3324
		[SaveableField(330)]
		public readonly IFaction CapturerPartyMapFaction;

		// Token: 0x04000CFD RID: 3325
		[SaveableField(331)]
		public readonly Hero Prisoner;

		// Token: 0x04000CFE RID: 3326
		[SaveableField(332)]
		public readonly Settlement CapturerSettlement;

		// Token: 0x04000CFF RID: 3327
		[SaveableField(333)]
		public readonly Hero CapturerMobilePartyLeader;

		// Token: 0x04000D00 RID: 3328
		[SaveableField(334)]
		public readonly Hero CapturerHero;
	}
}
