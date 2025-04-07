using System;
using System.Globalization;
using System.Xml;

namespace TaleWorlds.Core
{
	// Token: 0x020000CA RID: 202
	public static class XmlHelper
	{
		// Token: 0x060009E2 RID: 2530 RVA: 0x0002072C File Offset: 0x0001E92C
		public static int ReadInt(XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf(str);
			if (xmlAttribute == null)
			{
				return 0;
			}
			return int.Parse(xmlAttribute.Value);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00020758 File Offset: 0x0001E958
		public static void ReadInt(ref int val, XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf(str);
			if (xmlAttribute != null)
			{
				val = int.Parse(xmlAttribute.Value);
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00020784 File Offset: 0x0001E984
		public static float ReadFloat(XmlNode node, string str, float defaultValue = 0f)
		{
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf(str);
			if (xmlAttribute == null)
			{
				return defaultValue;
			}
			return float.Parse(xmlAttribute.Value);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x000207B0 File Offset: 0x0001E9B0
		public static string ReadString(XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf(str);
			if (xmlAttribute == null)
			{
				return "";
			}
			return xmlAttribute.Value;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000207DC File Offset: 0x0001E9DC
		public static void ReadHexCode(ref uint val, XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf(str);
			if (xmlAttribute != null)
			{
				string text = xmlAttribute.Value;
				text = text.Substring(2);
				val = uint.Parse(text, NumberStyles.HexNumber);
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00020818 File Offset: 0x0001EA18
		public static bool ReadBool(XmlNode node, string str)
		{
			XmlAttribute xmlAttribute = node.Attributes.get_ItemOf(str);
			return xmlAttribute != null && Convert.ToBoolean(xmlAttribute.InnerText);
		}
	}
}
