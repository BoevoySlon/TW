using System;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000044 RID: 68
	public sealed class BladeData : MBObjectBase
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00013BD6 File Offset: 0x00011DD6
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00013BDE File Offset: 0x00011DDE
		public DamageTypes ThrustDamageType { get; private set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00013BE7 File Offset: 0x00011DE7
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x00013BEF File Offset: 0x00011DEF
		public float ThrustDamageFactor { get; private set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00013BF8 File Offset: 0x00011DF8
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00013C00 File Offset: 0x00011E00
		public DamageTypes SwingDamageType { get; private set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00013C09 File Offset: 0x00011E09
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00013C11 File Offset: 0x00011E11
		public float SwingDamageFactor { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00013C1A File Offset: 0x00011E1A
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00013C22 File Offset: 0x00011E22
		public float BladeLength { get; private set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00013C2B File Offset: 0x00011E2B
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00013C33 File Offset: 0x00011E33
		public float BladeWidth { get; private set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00013C3C File Offset: 0x00011E3C
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x00013C44 File Offset: 0x00011E44
		public short StackAmount { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00013C4D File Offset: 0x00011E4D
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00013C55 File Offset: 0x00011E55
		public string PhysicsMaterial { get; private set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00013C5E File Offset: 0x00011E5E
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00013C66 File Offset: 0x00011E66
		public string BodyName { get; private set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00013C6F File Offset: 0x00011E6F
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00013C77 File Offset: 0x00011E77
		public string HolsterMeshName { get; private set; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00013C80 File Offset: 0x00011E80
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00013C88 File Offset: 0x00011E88
		public string HolsterBodyName { get; private set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00013C91 File Offset: 0x00011E91
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00013C99 File Offset: 0x00011E99
		public float HolsterMeshLength { get; private set; }

		// Token: 0x06000585 RID: 1413 RVA: 0x00013CA2 File Offset: 0x00011EA2
		public BladeData(CraftingPiece.PieceTypes pieceType, float bladeLength)
		{
			this.PieceType = pieceType;
			this.BladeLength = bladeLength;
			this.ThrustDamageType = DamageTypes.Invalid;
			this.SwingDamageType = DamageTypes.Invalid;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00013CC8 File Offset: 0x00011EC8
		public override void Deserialize(MBObjectManager objectManager, XmlNode childNode)
		{
			this.Initialize();
			XmlAttribute xmlAttribute = childNode.Attributes.get_ItemOf("stack_amount");
			XmlAttribute xmlAttribute2 = childNode.Attributes.get_ItemOf("blade_length");
			XmlAttribute xmlAttribute3 = childNode.Attributes.get_ItemOf("blade_width");
			XmlAttribute xmlAttribute4 = childNode.Attributes.get_ItemOf("physics_material");
			XmlAttribute xmlAttribute5 = childNode.Attributes.get_ItemOf("body_name");
			XmlAttribute xmlAttribute6 = childNode.Attributes.get_ItemOf("holster_mesh");
			XmlAttribute xmlAttribute7 = childNode.Attributes.get_ItemOf("holster_body_name");
			XmlAttribute xmlAttribute8 = childNode.Attributes.get_ItemOf("holster_mesh_length");
			this.StackAmount = ((xmlAttribute != null) ? short.Parse(xmlAttribute.Value) : 1);
			this.BladeLength = ((xmlAttribute2 != null) ? (0.01f * float.Parse(xmlAttribute2.Value)) : this.BladeLength);
			this.BladeWidth = ((xmlAttribute3 != null) ? (0.01f * float.Parse(xmlAttribute3.Value)) : (0.15f + this.BladeLength * 0.3f));
			this.PhysicsMaterial = ((xmlAttribute4 != null) ? xmlAttribute4.InnerText : null);
			this.BodyName = ((xmlAttribute5 != null) ? xmlAttribute5.InnerText : null);
			this.HolsterMeshName = ((xmlAttribute6 != null) ? xmlAttribute6.InnerText : null);
			this.HolsterBodyName = ((xmlAttribute7 != null) ? xmlAttribute7.InnerText : null);
			this.HolsterMeshLength = 0.01f * ((xmlAttribute8 != null) ? float.Parse(xmlAttribute8.Value) : 0f);
			foreach (object obj in childNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string name = xmlNode.Name;
				if (!(name == "Thrust"))
				{
					if (name == "Swing")
					{
						XmlAttribute xmlAttribute9 = xmlNode.Attributes.get_ItemOf("damage_type");
						XmlAttribute xmlAttribute10 = xmlNode.Attributes.get_ItemOf("damage_factor");
						this.SwingDamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), xmlAttribute9.Value, true);
						this.SwingDamageFactor = float.Parse(xmlAttribute10.Value);
					}
				}
				else
				{
					XmlAttribute xmlAttribute11 = xmlNode.Attributes.get_ItemOf("damage_type");
					XmlAttribute xmlAttribute12 = xmlNode.Attributes.get_ItemOf("damage_factor");
					this.ThrustDamageType = (DamageTypes)Enum.Parse(typeof(DamageTypes), xmlAttribute11.Value, true);
					this.ThrustDamageFactor = float.Parse(xmlAttribute12.Value);
				}
			}
		}

		// Token: 0x04000282 RID: 642
		public readonly CraftingPiece.PieceTypes PieceType;
	}
}
