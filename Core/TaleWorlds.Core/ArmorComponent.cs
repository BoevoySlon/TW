using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000009 RID: 9
	public class ArmorComponent : ItemComponent
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000023CF File Offset: 0x000005CF
		// (set) Token: 0x06000043 RID: 67 RVA: 0x000023D7 File Offset: 0x000005D7
		public int HeadArmor { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000044 RID: 68 RVA: 0x000023E0 File Offset: 0x000005E0
		// (set) Token: 0x06000045 RID: 69 RVA: 0x000023E8 File Offset: 0x000005E8
		public int BodyArmor { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000023F1 File Offset: 0x000005F1
		// (set) Token: 0x06000047 RID: 71 RVA: 0x000023F9 File Offset: 0x000005F9
		public int LegArmor { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002402 File Offset: 0x00000602
		// (set) Token: 0x06000049 RID: 73 RVA: 0x0000240A File Offset: 0x0000060A
		public int ArmArmor { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002413 File Offset: 0x00000613
		// (set) Token: 0x0600004B RID: 75 RVA: 0x0000241B File Offset: 0x0000061B
		public int ManeuverBonus { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002424 File Offset: 0x00000624
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000242C File Offset: 0x0000062C
		public int SpeedBonus { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002435 File Offset: 0x00000635
		// (set) Token: 0x0600004F RID: 79 RVA: 0x0000243D File Offset: 0x0000063D
		public int ChargeBonus { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002446 File Offset: 0x00000646
		// (set) Token: 0x06000051 RID: 81 RVA: 0x0000244E File Offset: 0x0000064E
		public int FamilyType { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002457 File Offset: 0x00000657
		// (set) Token: 0x06000053 RID: 83 RVA: 0x0000245F File Offset: 0x0000065F
		public bool MultiMeshHasGenderVariations { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002468 File Offset: 0x00000668
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002470 File Offset: 0x00000670
		public ArmorComponent.ArmorMaterialTypes MaterialType { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002479 File Offset: 0x00000679
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002481 File Offset: 0x00000681
		public SkinMask MeshesMask { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000058 RID: 88 RVA: 0x0000248A File Offset: 0x0000068A
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002492 File Offset: 0x00000692
		public ArmorComponent.BodyMeshTypes BodyMeshType { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0000249B File Offset: 0x0000069B
		// (set) Token: 0x0600005B RID: 91 RVA: 0x000024A3 File Offset: 0x000006A3
		public ArmorComponent.BodyDeformTypes BodyDeformType { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005C RID: 92 RVA: 0x000024AC File Offset: 0x000006AC
		// (set) Token: 0x0600005D RID: 93 RVA: 0x000024B4 File Offset: 0x000006B4
		public ArmorComponent.HairCoverTypes HairCoverType { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000024BD File Offset: 0x000006BD
		// (set) Token: 0x0600005F RID: 95 RVA: 0x000024C5 File Offset: 0x000006C5
		public ArmorComponent.BeardCoverTypes BeardCoverType { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000024CE File Offset: 0x000006CE
		// (set) Token: 0x06000061 RID: 97 RVA: 0x000024D6 File Offset: 0x000006D6
		public ArmorComponent.HorseHarnessCoverTypes ManeCoverType { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000024DF File Offset: 0x000006DF
		// (set) Token: 0x06000063 RID: 99 RVA: 0x000024E7 File Offset: 0x000006E7
		public ArmorComponent.HorseTailCoverTypes TailCoverType { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000024F0 File Offset: 0x000006F0
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000024F8 File Offset: 0x000006F8
		public string ReinsMesh { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002501 File Offset: 0x00000701
		public string ReinsRopeMesh
		{
			get
			{
				return this.ReinsMesh + "_rope";
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002513 File Offset: 0x00000713
		public ArmorComponent(ItemObject item)
		{
			base.Item = item;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002524 File Offset: 0x00000724
		public override ItemComponent GetCopy()
		{
			return new ArmorComponent(base.Item)
			{
				HeadArmor = this.HeadArmor,
				BodyArmor = this.BodyArmor,
				LegArmor = this.LegArmor,
				ArmArmor = this.ArmArmor,
				MultiMeshHasGenderVariations = this.MultiMeshHasGenderVariations,
				MaterialType = this.MaterialType,
				MeshesMask = this.MeshesMask,
				BodyMeshType = this.BodyMeshType,
				HairCoverType = this.HairCoverType,
				BeardCoverType = this.BeardCoverType,
				ManeCoverType = this.ManeCoverType,
				TailCoverType = this.TailCoverType,
				BodyDeformType = this.BodyDeformType,
				ManeuverBonus = this.ManeuverBonus,
				SpeedBonus = this.SpeedBonus,
				ChargeBonus = this.ChargeBonus,
				FamilyType = this.FamilyType,
				ReinsMesh = this.ReinsMesh
			};
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002618 File Offset: 0x00000818
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.HeadArmor = ((node.Attributes.get_ItemOf("head_armor") != null) ? int.Parse(node.Attributes.get_ItemOf("head_armor").Value) : 0);
			this.BodyArmor = ((node.Attributes.get_ItemOf("body_armor") != null) ? int.Parse(node.Attributes.get_ItemOf("body_armor").Value) : 0);
			this.LegArmor = ((node.Attributes.get_ItemOf("leg_armor") != null) ? int.Parse(node.Attributes.get_ItemOf("leg_armor").Value) : 0);
			this.ArmArmor = ((node.Attributes.get_ItemOf("arm_armor") != null) ? int.Parse(node.Attributes.get_ItemOf("arm_armor").Value) : 0);
			this.FamilyType = ((node.Attributes.get_ItemOf("family_type") != null) ? int.Parse(node.Attributes.get_ItemOf("family_type").Value) : 0);
			this.ManeuverBonus = ((node.Attributes.get_ItemOf("maneuver_bonus") != null) ? int.Parse(node.Attributes.get_ItemOf("maneuver_bonus").Value) : 0);
			this.SpeedBonus = ((node.Attributes.get_ItemOf("speed_bonus") != null) ? int.Parse(node.Attributes.get_ItemOf("speed_bonus").Value) : 0);
			this.ChargeBonus = ((node.Attributes.get_ItemOf("charge_bonus") != null) ? int.Parse(node.Attributes.get_ItemOf("charge_bonus").Value) : 0);
			this.MaterialType = ((node.Attributes.get_ItemOf("material_type") != null) ? ((ArmorComponent.ArmorMaterialTypes)Enum.Parse(typeof(ArmorComponent.ArmorMaterialTypes), node.Attributes.get_ItemOf("material_type").Value)) : ArmorComponent.ArmorMaterialTypes.None);
			ArmorComponent.ArmorMaterialTypes materialType = this.MaterialType;
			this.MultiMeshHasGenderVariations = true;
			if (node.Attributes.get_ItemOf("has_gender_variations") != null)
			{
				this.MultiMeshHasGenderVariations = Convert.ToBoolean(node.Attributes.get_ItemOf("has_gender_variations").Value);
			}
			this.BodyMeshType = ArmorComponent.BodyMeshTypes.Normal;
			if (node.Attributes.get_ItemOf("body_mesh_type") != null)
			{
				string value = node.Attributes.get_ItemOf("body_mesh_type").Value;
				if (value == "upperbody")
				{
					this.BodyMeshType = ArmorComponent.BodyMeshTypes.Upperbody;
				}
				else if (value == "shoulders")
				{
					this.BodyMeshType = ArmorComponent.BodyMeshTypes.Shoulders;
				}
			}
			this.BodyDeformType = ArmorComponent.BodyDeformTypes.Medium;
			if (node.Attributes.get_ItemOf("body_deform_type") != null)
			{
				string value2 = node.Attributes.get_ItemOf("body_deform_type").Value;
				if (value2 == "large")
				{
					this.BodyDeformType = ArmorComponent.BodyDeformTypes.Large;
				}
				else if (value2 == "skinny")
				{
					this.BodyDeformType = ArmorComponent.BodyDeformTypes.Skinny;
				}
			}
			this.HairCoverType = ((node.Attributes.get_ItemOf("hair_cover_type") != null) ? ((ArmorComponent.HairCoverTypes)Enum.Parse(typeof(ArmorComponent.HairCoverTypes), node.Attributes.get_ItemOf("hair_cover_type").Value, true)) : ArmorComponent.HairCoverTypes.None);
			this.BeardCoverType = ((node.Attributes.get_ItemOf("beard_cover_type") != null) ? ((ArmorComponent.BeardCoverTypes)Enum.Parse(typeof(ArmorComponent.BeardCoverTypes), node.Attributes.get_ItemOf("beard_cover_type").Value, true)) : ArmorComponent.BeardCoverTypes.None);
			this.ManeCoverType = ((node.Attributes.get_ItemOf("mane_cover_type") != null) ? ((ArmorComponent.HorseHarnessCoverTypes)Enum.Parse(typeof(ArmorComponent.HorseHarnessCoverTypes), node.Attributes.get_ItemOf("mane_cover_type").Value, true)) : ArmorComponent.HorseHarnessCoverTypes.None);
			this.TailCoverType = ((node.Attributes.get_ItemOf("tail_cover_type") != null) ? ((ArmorComponent.HorseTailCoverTypes)Enum.Parse(typeof(ArmorComponent.HorseTailCoverTypes), node.Attributes.get_ItemOf("tail_cover_type").Value, true)) : ArmorComponent.HorseTailCoverTypes.None);
			this.ReinsMesh = ((node.Attributes.get_ItemOf("reins_mesh") != null) ? node.Attributes.get_ItemOf("reins_mesh").Value : "");
			bool flag = node.Attributes.get_ItemOf("covers_head") != null && Convert.ToBoolean(node.Attributes.get_ItemOf("covers_head").Value);
			bool flag2 = node.Attributes.get_ItemOf("covers_body") != null && Convert.ToBoolean(node.Attributes.get_ItemOf("covers_body").Value);
			bool flag3 = node.Attributes.get_ItemOf("covers_hands") != null && Convert.ToBoolean(node.Attributes.get_ItemOf("covers_hands").Value);
			bool flag4 = node.Attributes.get_ItemOf("covers_legs") != null && Convert.ToBoolean(node.Attributes.get_ItemOf("covers_legs").Value);
			if (!flag)
			{
				this.MeshesMask |= SkinMask.HeadVisible;
			}
			if (!flag2)
			{
				this.MeshesMask |= SkinMask.BodyVisible;
			}
			if (!flag3)
			{
				this.MeshesMask |= SkinMask.HandsVisible;
			}
			if (!flag4)
			{
				this.MeshesMask |= SkinMask.LegsVisible;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002B61 File Offset: 0x00000D61
		internal static void AutoGeneratedStaticCollectObjectsArmorComponent(object o, List<object> collectedObjects)
		{
			((ArmorComponent)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002B6F File Offset: 0x00000D6F
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x020000CC RID: 204
		public enum ArmorMaterialTypes : sbyte
		{
			// Token: 0x040005EC RID: 1516
			None,
			// Token: 0x040005ED RID: 1517
			Cloth,
			// Token: 0x040005EE RID: 1518
			Leather,
			// Token: 0x040005EF RID: 1519
			Chainmail,
			// Token: 0x040005F0 RID: 1520
			Plate
		}

		// Token: 0x020000CD RID: 205
		public enum HairCoverTypes
		{
			// Token: 0x040005F2 RID: 1522
			None,
			// Token: 0x040005F3 RID: 1523
			Type1,
			// Token: 0x040005F4 RID: 1524
			Type2,
			// Token: 0x040005F5 RID: 1525
			Type3,
			// Token: 0x040005F6 RID: 1526
			Type4,
			// Token: 0x040005F7 RID: 1527
			All,
			// Token: 0x040005F8 RID: 1528
			NumHairCoverTypes
		}

		// Token: 0x020000CE RID: 206
		public enum BeardCoverTypes
		{
			// Token: 0x040005FA RID: 1530
			None,
			// Token: 0x040005FB RID: 1531
			Type1,
			// Token: 0x040005FC RID: 1532
			Type2,
			// Token: 0x040005FD RID: 1533
			Type3,
			// Token: 0x040005FE RID: 1534
			Type4,
			// Token: 0x040005FF RID: 1535
			All,
			// Token: 0x04000600 RID: 1536
			NumBeardBoverTypes
		}

		// Token: 0x020000CF RID: 207
		public enum HorseHarnessCoverTypes
		{
			// Token: 0x04000602 RID: 1538
			None,
			// Token: 0x04000603 RID: 1539
			Type1,
			// Token: 0x04000604 RID: 1540
			Type2,
			// Token: 0x04000605 RID: 1541
			All,
			// Token: 0x04000606 RID: 1542
			HorseHarnessCoverTypes
		}

		// Token: 0x020000D0 RID: 208
		public enum HorseTailCoverTypes
		{
			// Token: 0x04000608 RID: 1544
			None,
			// Token: 0x04000609 RID: 1545
			All
		}

		// Token: 0x020000D1 RID: 209
		public enum BodyMeshTypes
		{
			// Token: 0x0400060B RID: 1547
			Normal,
			// Token: 0x0400060C RID: 1548
			Upperbody,
			// Token: 0x0400060D RID: 1549
			Shoulders,
			// Token: 0x0400060E RID: 1550
			BodyMeshTypesNum
		}

		// Token: 0x020000D2 RID: 210
		public enum BodyDeformTypes
		{
			// Token: 0x04000610 RID: 1552
			Medium,
			// Token: 0x04000611 RID: 1553
			Large,
			// Token: 0x04000612 RID: 1554
			Skinny,
			// Token: 0x04000613 RID: 1555
			BodyMeshTypesNum
		}
	}
}
