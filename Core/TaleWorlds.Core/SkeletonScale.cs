using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000BD RID: 189
	public sealed class SkeletonScale : MBObjectBase
	{
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0001FBFC File Offset: 0x0001DDFC
		// (set) Token: 0x06000994 RID: 2452 RVA: 0x0001FC04 File Offset: 0x0001DE04
		public string SkeletonModel { get; private set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0001FC0D File Offset: 0x0001DE0D
		// (set) Token: 0x06000996 RID: 2454 RVA: 0x0001FC15 File Offset: 0x0001DE15
		public Vec3 MountSitBoneScale { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000997 RID: 2455 RVA: 0x0001FC1E File Offset: 0x0001DE1E
		// (set) Token: 0x06000998 RID: 2456 RVA: 0x0001FC26 File Offset: 0x0001DE26
		public float MountRadiusAdder { get; private set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0001FC2F File Offset: 0x0001DE2F
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0001FC37 File Offset: 0x0001DE37
		public Vec3[] Scales { get; private set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0001FC40 File Offset: 0x0001DE40
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0001FC48 File Offset: 0x0001DE48
		public List<string> BoneNames { get; private set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0001FC51 File Offset: 0x0001DE51
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0001FC59 File Offset: 0x0001DE59
		public sbyte[] BoneIndices { get; private set; }

		// Token: 0x0600099F RID: 2463 RVA: 0x0001FC62 File Offset: 0x0001DE62
		public SkeletonScale()
		{
			this.BoneNames = null;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001FC74 File Offset: 0x0001DE74
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.SkeletonModel = node.Attributes.get_ItemOf("skeleton").InnerText;
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf("mount_sit_bone_scale");
			Vec3 mountSitBoneScale = new Vec3(1f, 1f, 1f, -1f);
			if (xmlAttribute != null)
			{
				string[] array = xmlAttribute.Value.Split(new char[]
				{
					','
				});
				if (array.Length == 3)
				{
					float.TryParse(array[0], out mountSitBoneScale.x);
					float.TryParse(array[1], out mountSitBoneScale.y);
					float.TryParse(array[2], out mountSitBoneScale.z);
				}
			}
			this.MountSitBoneScale = mountSitBoneScale;
			XmlAttribute xmlAttribute2 = node.Attributes.get_ItemOf("mount_radius_adder");
			if (xmlAttribute2 != null)
			{
				this.MountRadiusAdder = float.Parse(xmlAttribute2.Value);
			}
			this.BoneNames = new List<string>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string name = xmlNode.Name;
				if (name == "BoneScales")
				{
					List<Vec3> list = new List<Vec3>();
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Attributes != null)
						{
							name = xmlNode2.Name;
							if (name == "BoneScale")
							{
								XmlAttribute xmlAttribute3 = xmlNode2.Attributes.get_ItemOf("scale");
								Vec3 item = default(Vec3);
								if (xmlAttribute3 != null)
								{
									string[] array2 = xmlAttribute3.Value.Split(new char[]
									{
										','
									});
									if (array2.Length == 3)
									{
										float.TryParse(array2[0], out item.x);
										float.TryParse(array2[1], out item.y);
										float.TryParse(array2[2], out item.z);
									}
								}
								this.BoneNames.Add(xmlNode2.Attributes.get_ItemOf("bone_name").InnerText);
								list.Add(item);
							}
						}
					}
					this.Scales = list.ToArray();
				}
			}
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001FF04 File Offset: 0x0001E104
		public void SetBoneIndices(sbyte[] boneIndices)
		{
			this.BoneIndices = boneIndices;
			this.BoneNames = null;
		}
	}
}
