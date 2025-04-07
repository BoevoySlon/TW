﻿using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x0200000F RID: 15
	public class BannerComponent : WeaponComponent
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003D39 File Offset: 0x00001F39
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00003D41 File Offset: 0x00001F41
		public int BannerLevel { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003D4A File Offset: 0x00001F4A
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00003D52 File Offset: 0x00001F52
		public BannerEffect BannerEffect { get; private set; }

		// Token: 0x060000A6 RID: 166 RVA: 0x00003D5B File Offset: 0x00001F5B
		public BannerComponent(ItemObject item) : base(item)
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00003D64 File Offset: 0x00001F64
		public override ItemComponent GetCopy()
		{
			return new BannerComponent(this.Item)
			{
				BannerLevel = this.BannerLevel,
				BannerEffect = this.BannerEffect
			};
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00003D96 File Offset: 0x00001F96
		public float GetBannerEffectBonus()
		{
			return this.BannerEffect.GetBonusAtLevel(this.BannerLevel);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00003DAC File Offset: 0x00001FAC
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.BannerLevel = ((node.Attributes.get_ItemOf("banner_level") != null) ? int.Parse(node.Attributes.get_ItemOf("banner_level").Value) : 1);
			this.BannerEffect = MBObjectManager.Instance.GetObject<BannerEffect>(node.Attributes.get_ItemOf("effect").Value);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003E1B File Offset: 0x0000201B
		internal static void AutoGeneratedStaticCollectObjectsBannerComponent(object o, List<object> collectedObjects)
		{
			((BannerComponent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003E29 File Offset: 0x00002029
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}
	}
}
