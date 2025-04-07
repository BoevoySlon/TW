using System;
using System.Collections;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000046 RID: 70
	public class WeaponDescription : MBObjectBase
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00013FB2 File Offset: 0x000121B2
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00013FBA File Offset: 0x000121BA
		public WeaponClass WeaponClass { get; private set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00013FC3 File Offset: 0x000121C3
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x00013FCB File Offset: 0x000121CB
		public WeaponFlags WeaponFlags { get; private set; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x00013FD4 File Offset: 0x000121D4
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x00013FDC File Offset: 0x000121DC
		public string ItemUsageFeatures { get; private set; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00013FE5 File Offset: 0x000121E5
		// (set) Token: 0x06000593 RID: 1427 RVA: 0x00013FED File Offset: 0x000121ED
		public bool RotatedInHand { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00013FF6 File Offset: 0x000121F6
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x00013FFE File Offset: 0x000121FE
		public bool IsHiddenFromUI { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00014007 File Offset: 0x00012207
		public MBReadOnlyList<CraftingPiece> AvailablePieces
		{
			get
			{
				return this._availablePieces;
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00014010 File Offset: 0x00012210
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.WeaponClass = ((node.Attributes.get_ItemOf("weapon_class") != null) ? ((WeaponClass)Enum.Parse(typeof(WeaponClass), node.Attributes.get_ItemOf("weapon_class").Value)) : WeaponClass.Undefined);
			this.ItemUsageFeatures = ((node.Attributes.get_ItemOf("item_usage_features") != null) ? node.Attributes.get_ItemOf("item_usage_features").Value : "");
			this.RotatedInHand = XmlHelper.ReadBool(node, "rotated_in_hand");
			this.UseCenterOfMassAsHandBase = XmlHelper.ReadBool(node, "use_center_of_mass_as_hand_base");
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "WeaponFlags")
				{
					using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							this.WeaponFlags |= (WeaponFlags)Enum.Parse(typeof(WeaponFlags), xmlNode2.Attributes.get_ItemOf("value").Value);
						}
						continue;
					}
				}
				if (xmlNode.Name == "AvailablePieces")
				{
					this._availablePieces = new MBList<CraftingPiece>();
					foreach (object obj3 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode3 = (XmlNode)obj3;
						if (xmlNode3.NodeType == 1)
						{
							string value = xmlNode3.Attributes.get_ItemOf("id").Value;
							CraftingPiece @object = MBObjectManager.Instance.GetObject<CraftingPiece>(value);
							if (@object != null)
							{
								this._availablePieces.Add(@object);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000295 RID: 661
		public bool UseCenterOfMassAsHandBase;

		// Token: 0x04000297 RID: 663
		private MBList<CraftingPiece> _availablePieces;
	}
}
