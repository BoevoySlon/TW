﻿using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.MapNotificationTypes
{
	// Token: 0x02000267 RID: 615
	public class VassalOfferMapNotification : InformationData
	{
		// Token: 0x0600203E RID: 8254 RVA: 0x0008AB3F File Offset: 0x00088D3F
		internal static void AutoGeneratedStaticCollectObjectsVassalOfferMapNotification(object o, List<object> collectedObjects)
		{
			((VassalOfferMapNotification)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0008AB4D File Offset: 0x00088D4D
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.OfferedKingdom);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0008AB62 File Offset: 0x00088D62
		internal static object AutoGeneratedGetMemberValueOfferedKingdom(object o)
		{
			return ((VassalOfferMapNotification)o).OfferedKingdom;
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x0008AB6F File Offset: 0x00088D6F
		public override TextObject TitleText
		{
			get
			{
				return new TextObject("{=MaDRGj4M}Vassalage Call", null);
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x0008AB7C File Offset: 0x00088D7C
		public override string SoundEventPath
		{
			get
			{
				return "event:/ui/notification/kingdom_decision";
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x0008AB83 File Offset: 0x00088D83
		// (set) Token: 0x06002044 RID: 8260 RVA: 0x0008AB8B File Offset: 0x00088D8B
		[SaveableProperty(1)]
		public Kingdom OfferedKingdom { get; private set; }

		// Token: 0x06002045 RID: 8261 RVA: 0x0008AB94 File Offset: 0x00088D94
		public VassalOfferMapNotification(Kingdom offeredKingdom, TextObject descriptionText) : base(descriptionText)
		{
			this.OfferedKingdom = offeredKingdom;
		}
	}
}
