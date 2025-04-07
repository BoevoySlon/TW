﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.MapNotificationTypes
{
	// Token: 0x0200026B RID: 619
	public class AlleyUnderAttackMapNotification : InformationData
	{
		// Token: 0x06002067 RID: 8295 RVA: 0x0008AD95 File Offset: 0x00088F95
		internal static void AutoGeneratedStaticCollectObjectsAlleyUnderAttackMapNotification(object o, List<object> collectedObjects)
		{
			((AlleyUnderAttackMapNotification)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x0008ADA3 File Offset: 0x00088FA3
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Alley);
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x0008ADB8 File Offset: 0x00088FB8
		internal static object AutoGeneratedGetMemberValueAlley(object o)
		{
			return ((AlleyUnderAttackMapNotification)o).Alley;
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x0008ADC5 File Offset: 0x00088FC5
		// (set) Token: 0x0600206B RID: 8299 RVA: 0x0008ADCD File Offset: 0x00088FCD
		[SaveableProperty(10)]
		public Alley Alley { get; private set; }

		// Token: 0x0600206C RID: 8300 RVA: 0x0008ADD6 File Offset: 0x00088FD6
		public AlleyUnderAttackMapNotification(Alley alley, TextObject description) : base(description)
		{
			this.Alley = alley;
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x0008ADE6 File Offset: 0x00088FE6
		public override TextObject TitleText
		{
			get
			{
				return new TextObject("{=emQQbbO9}Alley under attack!", null);
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x0008ADF3 File Offset: 0x00088FF3
		public override string SoundEventPath
		{
			get
			{
				return "event:/ui/notification/war_declared";
			}
		}
	}
}
