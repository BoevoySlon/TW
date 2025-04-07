using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x0200001D RID: 29
	public static class MiscHelper
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x0000C368 File Offset: 0x0000A568
		public static XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(path);
			string text = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text);
			streamReader.Close();
			return xmlDocument;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		public static string GenerateCampaignId(int length)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				string s = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
				byte[] bytes = Encoding.ASCII.GetBytes(s);
				byte[] array = md.ComputeHash(bytes);
				MBStringBuilder mbstringBuilder = default(MBStringBuilder);
				mbstringBuilder.Initialize(16, "GenerateCampaignId");
				int num = 0;
				while (num < array.Length && mbstringBuilder.Length < length)
				{
					mbstringBuilder.Append<string>(array[num].ToString("x2"));
					num++;
				}
				result = mbstringBuilder.ToStringAndRelease();
			}
			return result;
		}
	}
}
