﻿using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000039 RID: 57
	public class WeaponComponent : ItemComponent
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x0000FF98 File Offset: 0x0000E198
		internal static void AutoGeneratedStaticCollectObjectsWeaponComponent(object o, List<object> collectedObjects)
		{
			((WeaponComponent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000FFA6 File Offset: 0x0000E1A6
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0000FFAF File Offset: 0x0000E1AF
		public MBReadOnlyList<WeaponComponentData> Weapons
		{
			get
			{
				return this._weaponList;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000FFB7 File Offset: 0x0000E1B7
		public WeaponComponentData PrimaryWeapon
		{
			get
			{
				return this._weaponList[0];
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000FFC5 File Offset: 0x0000E1C5
		public void AddWeapon(WeaponComponentData weaponComponentData, ItemModifierGroup itemModifierGroup)
		{
			base.ItemModifierGroup = itemModifierGroup;
			this._weaponList.Add(weaponComponentData);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000FFDA File Offset: 0x0000E1DA
		public override ItemComponent GetCopy()
		{
			return new WeaponComponent(base.Item);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000FFE7 File Offset: 0x0000E1E7
		public WeaponComponent(ItemObject item)
		{
			base.Item = item;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00010001 File Offset: 0x0000E201
		public ItemObject.ItemTypeEnum GetItemType()
		{
			return WeaponComponentData.GetItemTypeFromWeaponClass(this._weaponList[0].WeaponClass);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001001C File Offset: 0x0000E21C
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf("modifier_group");
			if (xmlAttribute != null)
			{
				string value = xmlAttribute.Value;
			}
			WeaponComponentData weaponComponentData = new WeaponComponentData(base.Item, WeaponClass.Undefined, (WeaponFlags)0UL);
			weaponComponentData.Deserialize(base.Item, node);
			this._weaponList.Add(weaponComponentData);
		}

		// Token: 0x04000221 RID: 545
		private readonly MBList<WeaponComponentData> _weaponList = new MBList<WeaponComponentData>();
	}
}
