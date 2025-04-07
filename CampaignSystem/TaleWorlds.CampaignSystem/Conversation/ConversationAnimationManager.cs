using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x020001E6 RID: 486
	public class ConversationAnimationManager
	{
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x0008509E File Offset: 0x0008329E
		// (set) Token: 0x06001D82 RID: 7554 RVA: 0x000850A6 File Offset: 0x000832A6
		public Dictionary<string, ConversationAnimData> ConversationAnims { get; private set; }

		// Token: 0x06001D83 RID: 7555 RVA: 0x000850AF File Offset: 0x000832AF
		public ConversationAnimationManager()
		{
			this.ConversationAnims = new Dictionary<string, ConversationAnimData>();
			this.LoadConversationAnimData(ModuleHelper.GetModuleFullPath("Sandbox") + "ModuleData/conversation_animations.xml");
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000850DC File Offset: 0x000832DC
		private void LoadConversationAnimData(string xmlPath)
		{
			XmlDocument doc = this.LoadXmlFile(xmlPath);
			this.LoadFromXml(doc);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000850F8 File Offset: 0x000832F8
		private XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00085144 File Offset: 0x00083344
		private void LoadFromXml(XmlDocument doc)
		{
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			if (doc.ChildNodes.get_ItemOf(1).Name != "ConversationAnimations")
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			foreach (object obj in doc.DocumentElement.SelectNodes("IdleAnim"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Attributes != null)
				{
					KeyValuePair<string, ConversationAnimData> keyValuePair = new KeyValuePair<string, ConversationAnimData>(xmlNode.Attributes.get_ItemOf("id").Value, new ConversationAnimData());
					keyValuePair.Value.IdleAnimStart = xmlNode.Attributes.get_ItemOf("action_id_1").Value;
					keyValuePair.Value.IdleAnimLoop = xmlNode.Attributes.get_ItemOf("action_id_2").Value;
					keyValuePair.Value.FamilyType = 0;
					XmlAttribute xmlAttribute = xmlNode.Attributes.get_ItemOf("family_type");
					int familyType;
					if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value) && int.TryParse(xmlAttribute.Value, out familyType))
					{
						keyValuePair.Value.FamilyType = familyType;
					}
					keyValuePair.Value.MountFamilyType = 0;
					XmlAttribute xmlAttribute2 = xmlNode.Attributes.get_ItemOf("mount_family_type");
					int mountFamilyType;
					if (xmlAttribute2 != null && !string.IsNullOrEmpty(xmlAttribute2.Value) && int.TryParse(xmlAttribute2.Value, out mountFamilyType))
					{
						keyValuePair.Value.MountFamilyType = mountFamilyType;
					}
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "Reactions")
						{
							foreach (object obj3 in xmlNode2.ChildNodes)
							{
								XmlNode xmlNode3 = (XmlNode)obj3;
								if (xmlNode3.Name == "Reaction" && xmlNode3.Attributes.get_ItemOf("id") != null && xmlNode3.Attributes.get_ItemOf("action_id") != null)
								{
									keyValuePair.Value.Reactions.Add(xmlNode3.Attributes.get_ItemOf("id").Value, xmlNode3.Attributes.get_ItemOf("action_id").Value);
								}
							}
						}
					}
					this.ConversationAnims.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
	}
}
