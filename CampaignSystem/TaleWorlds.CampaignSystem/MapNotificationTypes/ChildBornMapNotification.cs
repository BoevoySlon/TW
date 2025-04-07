﻿using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.MapNotificationTypes
{
	// Token: 0x02000257 RID: 599
	public class ChildBornMapNotification : InformationData
	{
		// Token: 0x06001F80 RID: 8064 RVA: 0x0008A021 File Offset: 0x00088221
		internal static void AutoGeneratedStaticCollectObjectsChildBornMapNotification(object o, List<object> collectedObjects)
		{
			((ChildBornMapNotification)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0008A02F File Offset: 0x0008822F
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.NewbornHero);
			CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this.CreationTime, collectedObjects);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0008A055 File Offset: 0x00088255
		internal static object AutoGeneratedGetMemberValueNewbornHero(object o)
		{
			return ((ChildBornMapNotification)o).NewbornHero;
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0008A062 File Offset: 0x00088262
		internal static object AutoGeneratedGetMemberValueCreationTime(object o)
		{
			return ((ChildBornMapNotification)o).CreationTime;
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06001F84 RID: 8068 RVA: 0x0008A074 File Offset: 0x00088274
		public override TextObject TitleText
		{
			get
			{
				return new TextObject("{=IEQrNbpH}Inheritance", null);
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x0008A081 File Offset: 0x00088281
		public override string SoundEventPath
		{
			get
			{
				return "event:/ui/notification/child_born";
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x0008A088 File Offset: 0x00088288
		// (set) Token: 0x06001F87 RID: 8071 RVA: 0x0008A090 File Offset: 0x00088290
		[SaveableProperty(2)]
		public Hero NewbornHero { get; private set; }

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x0008A099 File Offset: 0x00088299
		// (set) Token: 0x06001F89 RID: 8073 RVA: 0x0008A0A1 File Offset: 0x000882A1
		[SaveableProperty(3)]
		public CampaignTime CreationTime { get; private set; }

		// Token: 0x06001F8A RID: 8074 RVA: 0x0008A0AA File Offset: 0x000882AA
		public ChildBornMapNotification(Hero newbornHero, TextObject descriptionText, CampaignTime creationTime) : base(descriptionText)
		{
			this.NewbornHero = newbornHero;
			this.CreationTime = creationTime;
		}
	}
}
