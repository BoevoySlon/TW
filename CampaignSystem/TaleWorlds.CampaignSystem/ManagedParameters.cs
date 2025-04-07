using System;
using System.IO;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000095 RID: 149
	public sealed class ManagedParameters : IManagedParametersInitializer
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x0004C37A File Offset: 0x0004A57A
		public static ManagedParameters Instance { get; } = new ManagedParameters();

		// Token: 0x06001135 RID: 4405 RVA: 0x0004C384 File Offset: 0x0004A584
		public void Initialize(string relativeXmlPath)
		{
			XmlDocument doc = ManagedParameters.LoadXmlFile(relativeXmlPath);
			this.LoadFromXml(doc);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x0004C3A0 File Offset: 0x0004A5A0
		private void LoadFromXml(XmlNode doc)
		{
			XmlNode xmlNode = null;
			if (doc.ChildNodes.get_ItemOf(1).ChildNodes.get_ItemOf(0).Name == "managed_campaign_parameters")
			{
				xmlNode = doc.ChildNodes.get_ItemOf(1).ChildNodes.get_ItemOf(0).ChildNodes.get_ItemOf(0);
			}
			while (xmlNode != null)
			{
				ManagedParametersEnum managedParametersEnum;
				if (xmlNode.Name == "managed_campaign_parameter" && xmlNode.NodeType != 8 && Enum.TryParse<ManagedParametersEnum>(xmlNode.Attributes.get_ItemOf("id").Value, true, out managedParametersEnum))
				{
					this._managedParametersArray[(int)managedParametersEnum] = bool.Parse(xmlNode.Attributes.get_ItemOf("value").Value);
				}
				xmlNode = xmlNode.NextSibling;
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x0004C468 File Offset: 0x0004A668
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

		// Token: 0x06001138 RID: 4408 RVA: 0x0004C4B1 File Offset: 0x0004A6B1
		public bool GetManagedParameter(ManagedParametersEnum _managedParametersEnum)
		{
			return this._managedParametersArray[(int)_managedParametersEnum];
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x0004C4BC File Offset: 0x0004A6BC
		public bool SetManagedParameter(ManagedParametersEnum _managedParametersEnum, bool value)
		{
			this._managedParametersArray[(int)_managedParametersEnum] = value;
			return value;
		}

		// Token: 0x040005DD RID: 1501
		private readonly bool[] _managedParametersArray = new bool[2];
	}
}
