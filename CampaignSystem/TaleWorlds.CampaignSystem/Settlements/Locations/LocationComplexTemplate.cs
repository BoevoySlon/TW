using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Settlements.Locations
{
	// Token: 0x0200036E RID: 878
	public sealed class LocationComplexTemplate : MBObjectBase
	{
		// Token: 0x060033AD RID: 13229 RVA: 0x000D6584 File Offset: 0x000D4784
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			List<string> list = new List<string>();
			base.Deserialize(objectManager, node);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Location")
				{
					if (xmlNode.Attributes == null)
					{
						throw new TWXmlLoadException("node.Attributes != null");
					}
					string value = xmlNode.Attributes.get_ItemOf("id").Value;
					TextObject textObject = new TextObject(xmlNode.Attributes.get_ItemOf("name").Value, null);
					string[] sceneNames = new string[]
					{
						(xmlNode.Attributes.get_ItemOf("scene_name") != null) ? xmlNode.Attributes.get_ItemOf("scene_name").Value : "",
						(xmlNode.Attributes.get_ItemOf("scene_name_1") != null) ? xmlNode.Attributes.get_ItemOf("scene_name_1").Value : "",
						(xmlNode.Attributes.get_ItemOf("scene_name_2") != null) ? xmlNode.Attributes.get_ItemOf("scene_name_2").Value : "",
						(xmlNode.Attributes.get_ItemOf("scene_name_3") != null) ? xmlNode.Attributes.get_ItemOf("scene_name_3").Value : ""
					};
					int prosperityMax = int.Parse(xmlNode.Attributes.get_ItemOf("max_prosperity").Value);
					bool isIndoor = bool.Parse(xmlNode.Attributes.get_ItemOf("indoor").Value);
					bool canBeReserved = xmlNode.Attributes.get_ItemOf("can_be_reserved") != null && bool.Parse(xmlNode.Attributes.get_ItemOf("can_be_reserved").Value);
					string innerText = xmlNode.Attributes.get_ItemOf("player_can_enter").InnerText;
					string innerText2 = xmlNode.Attributes.get_ItemOf("player_can_see").InnerText;
					string innerText3 = xmlNode.Attributes.get_ItemOf("ai_can_exit").InnerText;
					string innerText4 = xmlNode.Attributes.get_ItemOf("ai_can_enter").InnerText;
					list.Add(value);
					this.Locations.Add(new Location(value, textObject, textObject, prosperityMax, isIndoor, canBeReserved, innerText, innerText2, innerText3, innerText4, sceneNames, null));
				}
				if (xmlNode.Name == "Passages")
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "Passage")
						{
							if (xmlNode2.Attributes == null)
							{
								throw new TWXmlLoadException("node.Attributes != null");
							}
							string value2 = xmlNode2.Attributes.get_ItemOf("location_1").Value;
							if (!list.Contains(value2))
							{
								throw new TWXmlLoadException("Passage location does not exist with id :" + value2);
							}
							string value3 = xmlNode2.Attributes.get_ItemOf("location_2").Value;
							if (!list.Contains(value3))
							{
								throw new TWXmlLoadException("Passage location does not exist with id :" + value2);
							}
							this.Passages.Add(new KeyValuePair<string, string>(value2, value3));
						}
					}
				}
			}
		}

		// Token: 0x040010AB RID: 4267
		public List<Location> Locations = new List<Location>();

		// Token: 0x040010AC RID: 4268
		public List<KeyValuePair<string, string>> Passages = new List<KeyValuePair<string, string>>();
	}
}
