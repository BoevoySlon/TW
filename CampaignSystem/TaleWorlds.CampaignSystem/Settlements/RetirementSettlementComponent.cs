﻿using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Settlements
{
	// Token: 0x02000361 RID: 865
	public class RetirementSettlementComponent : SettlementComponent, ISpottable
	{
		// Token: 0x06003283 RID: 12931 RVA: 0x000D2FE0 File Offset: 0x000D11E0
		internal static void AutoGeneratedStaticCollectObjectsRetirementSettlementComponent(object o, List<object> collectedObjects)
		{
			((RetirementSettlementComponent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x000D2FEE File Offset: 0x000D11EE
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x000D2FF7 File Offset: 0x000D11F7
		internal static object AutoGeneratedGetMemberValue_isSpotted(object o)
		{
			return ((RetirementSettlementComponent)o)._isSpotted;
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06003286 RID: 12934 RVA: 0x000D3009 File Offset: 0x000D1209
		// (set) Token: 0x06003287 RID: 12935 RVA: 0x000D3011 File Offset: 0x000D1211
		public bool IsSpotted
		{
			get
			{
				return this._isSpotted;
			}
			set
			{
				this._isSpotted = value;
			}
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x000D3024 File Offset: 0x000D1224
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			if (node.Attributes.get_ItemOf("background_crop_position") != null)
			{
				base.BackgroundCropPosition = float.Parse(node.Attributes.get_ItemOf("background_crop_position").Value);
			}
			if (node.Attributes.get_ItemOf("background_mesh") != null)
			{
				base.BackgroundMeshName = node.Attributes.get_ItemOf("background_mesh").Value;
			}
			if (node.Attributes.get_ItemOf("wait_mesh") != null)
			{
				base.WaitMeshName = node.Attributes.get_ItemOf("wait_mesh").Value;
			}
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x000D30C5 File Offset: 0x000D12C5
		protected override void OnInventoryUpdated(ItemRosterElement item, int count)
		{
		}

		// Token: 0x04001050 RID: 4176
		[SaveableField(10)]
		private bool _isSpotted;
	}
}
