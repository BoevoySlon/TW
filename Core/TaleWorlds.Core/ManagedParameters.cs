using System;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000093 RID: 147
	public sealed class ManagedParameters : IManagedParametersInitializer
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001B3A5 File Offset: 0x000195A5
		public static ManagedParameters Instance { get; } = new ManagedParameters();

		// Token: 0x060007FB RID: 2043 RVA: 0x0001B3AC File Offset: 0x000195AC
		public static float GetParameter(ManagedParametersEnum managedParameterType)
		{
			return ManagedParameters.Instance._managedParametersArray[(int)managedParameterType];
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001B3BA File Offset: 0x000195BA
		public static void SetParameter(ManagedParametersEnum managedParameterType, float newValue)
		{
			ManagedParameters.Instance._managedParametersArray[(int)managedParameterType] = newValue;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001B3CC File Offset: 0x000195CC
		public void Initialize(string relativeXmlPath)
		{
			XmlDocument doc = ManagedParameters.LoadXmlFile(relativeXmlPath);
			this.LoadFromXml(doc);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001B3E7 File Offset: 0x000195E7
		private ManagedParameters()
		{
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001B3FC File Offset: 0x000195FC
		private static XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001B448 File Offset: 0x00019648
		private void LoadFromXml(XmlNode doc)
		{
			Debug.Print("loading managed_core_parameters.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			if (doc.ChildNodes.get_ItemOf(1).Name != "base")
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			if (doc.ChildNodes.get_ItemOf(1).ChildNodes.get_ItemOf(0).Name != "managed_core_parameters")
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			XmlNode xmlNode = null;
			if (doc.ChildNodes.get_ItemOf(1).ChildNodes.get_ItemOf(0).Name == "managed_core_parameters")
			{
				xmlNode = doc.ChildNodes.get_ItemOf(1).ChildNodes.get_ItemOf(0).ChildNodes.get_ItemOf(0);
			}
			while (xmlNode != null)
			{
				ManagedParametersEnum managedParametersEnum;
				if (xmlNode.Name == "managed_core_parameter" && xmlNode.NodeType != 8 && Enum.TryParse<ManagedParametersEnum>(xmlNode.Attributes.get_ItemOf("id").Value, true, out managedParametersEnum))
				{
					this._managedParametersArray[(int)managedParametersEnum] = float.Parse(xmlNode.Attributes.get_ItemOf("value").Value);
				}
				xmlNode = xmlNode.NextSibling;
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001B597 File Offset: 0x00019797
		public float GetManagedParameter(ManagedParametersEnum managedParameterEnum)
		{
			return this._managedParametersArray[(int)managedParameterEnum];
		}

		// Token: 0x04000492 RID: 1170
		private readonly float[] _managedParametersArray = new float[68];
	}
}
