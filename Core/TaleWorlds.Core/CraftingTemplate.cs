﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000027 RID: 39
	public class CraftingTemplate : MBObjectBase
	{
		// Token: 0x06000267 RID: 615 RVA: 0x0000ABC9 File Offset: 0x00008DC9
		internal static void AutoGeneratedStaticCollectObjectsCraftingTemplate(object o, List<object> collectedObjects)
		{
			((CraftingTemplate)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000ABD7 File Offset: 0x00008DD7
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000ABE0 File Offset: 0x00008DE0
		// (set) Token: 0x0600026A RID: 618 RVA: 0x0000ABE8 File Offset: 0x00008DE8
		public PieceData[] BuildOrders { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000ABF1 File Offset: 0x00008DF1
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000ABF9 File Offset: 0x00008DF9
		public WeaponDescription[] WeaponDescriptions { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000AC02 File Offset: 0x00008E02
		// (set) Token: 0x0600026E RID: 622 RVA: 0x0000AC0A File Offset: 0x00008E0A
		public List<CraftingPiece> Pieces { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000AC13 File Offset: 0x00008E13
		// (set) Token: 0x06000270 RID: 624 RVA: 0x0000AC1B File Offset: 0x00008E1B
		public ItemObject.ItemTypeEnum ItemType { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000AC24 File Offset: 0x00008E24
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public ItemModifierGroup ItemModifierGroup { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000AC35 File Offset: 0x00008E35
		// (set) Token: 0x06000274 RID: 628 RVA: 0x0000AC3D File Offset: 0x00008E3D
		public string[] ItemHolsters { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000AC46 File Offset: 0x00008E46
		// (set) Token: 0x06000276 RID: 630 RVA: 0x0000AC4E File Offset: 0x00008E4E
		public Vec3 ItemHolsterPositionShift { get; private set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000AC57 File Offset: 0x00008E57
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000AC5F File Offset: 0x00008E5F
		public bool UseWeaponAsHolsterMesh { get; private set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000AC68 File Offset: 0x00008E68
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000AC70 File Offset: 0x00008E70
		public bool AlwaysShowHolsterWithWeapon { get; private set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000AC79 File Offset: 0x00008E79
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000AC81 File Offset: 0x00008E81
		public bool RotateWeaponInHolster { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600027D RID: 637 RVA: 0x0000AC8A File Offset: 0x00008E8A
		// (set) Token: 0x0600027E RID: 638 RVA: 0x0000AC92 File Offset: 0x00008E92
		public CraftingPiece.PieceTypes PieceTypeToScaleHolsterWith { get; private set; }

		// Token: 0x06000280 RID: 640 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		public int GetIndexOfUsageDataWithId(string weaponDescriptionId)
		{
			int result = -1;
			for (int i = 0; i < this.WeaponDescriptions.Length; i++)
			{
				if (weaponDescriptionId == this.WeaponDescriptions[i].StringId)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000ACE0 File Offset: 0x00008EE0
		public bool IsPieceTypeHiddenOnHolster(CraftingPiece.PieceTypes pieceType)
		{
			return this._hiddenPieceTypesOnHolsteredMesh[(int)pieceType];
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000ACEA File Offset: 0x00008EEA
		public IEnumerable<KeyValuePair<CraftingTemplate.CraftingStatTypes, float>> GetStatDatas(int usageIndex, DamageTypes thrustDamageType, DamageTypes swingDamageType)
		{
			int num;
			for (int i = 0; i < this._statDataValues[usageIndex].Length; i = num + 1)
			{
				CraftingTemplate.CraftingStatTypes key = (CraftingTemplate.CraftingStatTypes)i;
				bool flag = false;
				switch (key)
				{
				case CraftingTemplate.CraftingStatTypes.ThrustSpeed:
				case CraftingTemplate.CraftingStatTypes.ThrustDamage:
					flag = (thrustDamageType == DamageTypes.Invalid);
					break;
				case CraftingTemplate.CraftingStatTypes.SwingSpeed:
				case CraftingTemplate.CraftingStatTypes.SwingDamage:
					flag = (swingDamageType == DamageTypes.Invalid);
					break;
				}
				if (!flag && this._statDataValues[usageIndex][i] >= 0f)
				{
					yield return new KeyValuePair<CraftingTemplate.CraftingStatTypes, float>(key, this._statDataValues[usageIndex][i]);
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000AD0F File Offset: 0x00008F0F
		public override string ToString()
		{
			return this.TemplateName.ToString();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000AD1C File Offset: 0x00008F1C
		public bool IsPieceTypeUsable(CraftingPiece.PieceTypes pieceType)
		{
			return this.BuildOrders.Any((PieceData bO) => bO.PieceType == pieceType);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000AD50 File Offset: 0x00008F50
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this._hiddenPieceTypesOnHolsteredMesh = new bool[4];
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf("modifier_group");
			string text = (xmlAttribute != null) ? xmlAttribute.Value : null;
			if (text != null)
			{
				this.ItemModifierGroup = Game.Current.ObjectManager.GetObject<ItemModifierGroup>(text);
			}
			this.ItemType = (ItemObject.ItemTypeEnum)Enum.Parse(typeof(ItemObject.ItemTypeEnum), node.Attributes.get_ItemOf("item_type").Value);
			this.ItemHolsters = node.Attributes.get_ItemOf("item_holsters").Value.Split(new char[]
			{
				':'
			});
			this.ItemHolsterPositionShift = Vec3.Parse(node.Attributes.get_ItemOf("default_item_holster_position_offset").Value);
			this.UseWeaponAsHolsterMesh = XmlHelper.ReadBool(node, "use_weapon_as_holster_mesh");
			this.AlwaysShowHolsterWithWeapon = XmlHelper.ReadBool(node, "always_show_holster_with_weapon");
			this.RotateWeaponInHolster = XmlHelper.ReadBool(node, "rotate_weapon_in_holster");
			XmlAttribute xmlAttribute2 = node.Attributes.get_ItemOf("piece_type_to_scale_holster_with");
			this.PieceTypeToScaleHolsterWith = ((xmlAttribute2 != null) ? ((CraftingPiece.PieceTypes)Enum.Parse(typeof(CraftingPiece.PieceTypes), xmlAttribute2.Value)) : CraftingPiece.PieceTypes.Invalid);
			XmlAttribute xmlAttribute3 = node.Attributes.get_ItemOf("hidden_piece_types_on_holster");
			if (xmlAttribute3 != null)
			{
				foreach (string value in xmlAttribute3.Value.Split(new char[]
				{
					':'
				}))
				{
					CraftingPiece.PieceTypes pieceTypes = (CraftingPiece.PieceTypes)Enum.Parse(typeof(CraftingPiece.PieceTypes), value);
					this._hiddenPieceTypesOnHolsteredMesh[(int)pieceTypes] = true;
				}
			}
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Attributes != null)
				{
					string name = xmlNode.Name;
					if (!(name == "PieceDatas"))
					{
						if (!(name == "WeaponDescriptions"))
						{
							if (!(name == "UsablePieces"))
							{
								if (!(name == "StatsData"))
								{
									continue;
								}
							}
							else
							{
								this.Pieces = new List<CraftingPiece>();
								using (IEnumerator enumerator2 = xmlNode.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj2 = enumerator2.Current;
										string value2 = ((XmlNode)obj2).Attributes.get_ItemOf("piece_id").Value;
										CraftingPiece @object = MBObjectManager.Instance.GetObject<CraftingPiece>(value2);
										if (@object != null)
										{
											this.Pieces.Add(@object);
										}
									}
									continue;
								}
							}
							XmlAttribute xmlAttribute4 = xmlNode.Attributes.get_ItemOf("weapon_description");
							float[] array2 = new float[11];
							for (int j = 0; j < array2.Length; j++)
							{
								array2[j] = float.MinValue;
							}
							foreach (object obj3 in xmlNode.ChildNodes)
							{
								XmlNode xmlNode2 = (XmlNode)obj3;
								if (xmlNode2.NodeType == 1)
								{
									XmlAttribute xmlAttribute5 = xmlNode2.Attributes.get_ItemOf("stat_type");
									XmlNode xmlNode3 = xmlNode2.Attributes.get_ItemOf("max_value");
									CraftingTemplate.CraftingStatTypes craftingStatTypes = (CraftingTemplate.CraftingStatTypes)Enum.Parse(typeof(CraftingTemplate.CraftingStatTypes), xmlAttribute5.Value);
									float num = float.Parse(xmlNode3.Value);
									array2[(int)craftingStatTypes] = num;
								}
							}
							if (xmlAttribute4 != null)
							{
								int indexOfUsageDataWithId = this.GetIndexOfUsageDataWithId(xmlAttribute4.Value);
								this._statDataValues[indexOfUsageDataWithId] = array2;
							}
							else
							{
								for (int k = 0; k < this._statDataValues.Length; k++)
								{
									this._statDataValues[k] = array2;
								}
							}
						}
						else
						{
							List<WeaponDescription> list = new List<WeaponDescription>();
							foreach (object obj4 in xmlNode.ChildNodes)
							{
								string value3 = ((XmlNode)obj4).Attributes.get_ItemOf("id").Value;
								WeaponDescription object2 = MBObjectManager.Instance.GetObject<WeaponDescription>(value3);
								if (object2 != null)
								{
									list.Add(object2);
								}
							}
							this.WeaponDescriptions = list.ToArray();
							this._statDataValues = new float[this.WeaponDescriptions.Length][];
						}
					}
					else
					{
						List<PieceData> list2 = new List<PieceData>();
						foreach (object obj5 in xmlNode.ChildNodes)
						{
							XmlNode xmlNode4 = (XmlNode)obj5;
							XmlAttribute xmlAttribute6 = xmlNode4.Attributes.get_ItemOf("piece_type");
							XmlNode xmlNode5 = xmlNode4.Attributes.get_ItemOf("build_order");
							CraftingPiece.PieceTypes pieceType = (CraftingPiece.PieceTypes)Enum.Parse(typeof(CraftingPiece.PieceTypes), xmlAttribute6.Value);
							int order = int.Parse(xmlNode5.Value);
							list2.Add(new PieceData(pieceType, order));
						}
						this.BuildOrders = list2.ToArray();
					}
				}
			}
			this.TemplateName = GameTexts.FindText("str_crafting_template", base.StringId);
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000B300 File Offset: 0x00009500
		public static MBReadOnlyList<CraftingTemplate> All
		{
			get
			{
				return MBObjectManager.Instance.GetObjectTypeList<CraftingTemplate>();
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000B30C File Offset: 0x0000950C
		public static CraftingTemplate GetTemplateFromId(string templateId)
		{
			return MBObjectManager.Instance.GetObject<CraftingTemplate>(templateId);
		}

		// Token: 0x040001A0 RID: 416
		public TextObject TemplateName;

		// Token: 0x040001A3 RID: 419
		private bool[] _hiddenPieceTypesOnHolsteredMesh;

		// Token: 0x040001A5 RID: 421
		private float[][] _statDataValues;

		// Token: 0x020000DB RID: 219
		public enum CraftingStatTypes
		{
			// Token: 0x04000631 RID: 1585
			Weight,
			// Token: 0x04000632 RID: 1586
			WeaponReach,
			// Token: 0x04000633 RID: 1587
			ThrustSpeed,
			// Token: 0x04000634 RID: 1588
			SwingSpeed,
			// Token: 0x04000635 RID: 1589
			ThrustDamage,
			// Token: 0x04000636 RID: 1590
			SwingDamage,
			// Token: 0x04000637 RID: 1591
			Handling,
			// Token: 0x04000638 RID: 1592
			MissileDamage,
			// Token: 0x04000639 RID: 1593
			MissileSpeed,
			// Token: 0x0400063A RID: 1594
			Accuracy,
			// Token: 0x0400063B RID: 1595
			StackAmount,
			// Token: 0x0400063C RID: 1596
			NumStatTypes
		}
	}
}
