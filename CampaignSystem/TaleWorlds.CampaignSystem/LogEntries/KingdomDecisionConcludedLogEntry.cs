﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002DA RID: 730
	public class KingdomDecisionConcludedLogEntry : LogEntry, IChatNotification
	{
		// Token: 0x06002ADC RID: 10972 RVA: 0x000B64FD File Offset: 0x000B46FD
		internal static void AutoGeneratedStaticCollectObjectsKingdomDecisionConcludedLogEntry(object o, List<object> collectedObjects)
		{
			((KingdomDecisionConcludedLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000B650B File Offset: 0x000B470B
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Kingdom);
			collectedObjects.Add(this._notificationText);
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000B652C File Offset: 0x000B472C
		internal static object AutoGeneratedGetMemberValueKingdom(object o)
		{
			return ((KingdomDecisionConcludedLogEntry)o).Kingdom;
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000B6539 File Offset: 0x000B4739
		internal static object AutoGeneratedGetMemberValue_isVisibleNotification(object o)
		{
			return ((KingdomDecisionConcludedLogEntry)o)._isVisibleNotification;
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000B654B File Offset: 0x000B474B
		internal static object AutoGeneratedGetMemberValue_notificationText(object o)
		{
			return ((KingdomDecisionConcludedLogEntry)o)._notificationText;
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06002AE1 RID: 10977 RVA: 0x000B6558 File Offset: 0x000B4758
		public override CampaignTime KeepInHistoryTime
		{
			get
			{
				return CampaignTime.Weeks(1f);
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06002AE2 RID: 10978 RVA: 0x000B6564 File Offset: 0x000B4764
		public bool IsVisibleNotification
		{
			get
			{
				return this._isVisibleNotification;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06002AE3 RID: 10979 RVA: 0x000B656C File Offset: 0x000B476C
		public override ChatNotificationType NotificationType
		{
			get
			{
				return base.PoliticalNotification(this.Kingdom);
			}
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x000B657A File Offset: 0x000B477A
		public KingdomDecisionConcludedLogEntry(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			this.Kingdom = decision.Kingdom;
			this._isVisibleNotification = !isPlayerInvolved;
			this._notificationText = decision.GetChosenOutcomeText(chosenOutcome, decision.SupportStatusOfFinalDecision, true);
		}

		// Token: 0x06002AE5 RID: 10981 RVA: 0x000B65AC File Offset: 0x000B47AC
		public override string ToString()
		{
			return this.GetNotificationText().ToString();
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x000B65B9 File Offset: 0x000B47B9
		public TextObject GetNotificationText()
		{
			return this._notificationText;
		}

		// Token: 0x04000CDB RID: 3291
		[SaveableField(1)]
		public readonly Kingdom Kingdom;

		// Token: 0x04000CDC RID: 3292
		[SaveableField(3)]
		private readonly bool _isVisibleNotification;

		// Token: 0x04000CDD RID: 3293
		[SaveableField(4)]
		private readonly TextObject _notificationText;
	}
}
