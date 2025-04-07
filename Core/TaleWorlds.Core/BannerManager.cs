using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000014 RID: 20
	public class BannerManager
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000047B2 File Offset: 0x000029B2
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x000047B9 File Offset: 0x000029B9
		public static BannerManager Instance { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000047C1 File Offset: 0x000029C1
		public MBReadOnlyList<BannerIconGroup> BannerIconGroups
		{
			get
			{
				return this._bannerIconGroups;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000047C9 File Offset: 0x000029C9
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x000047D1 File Offset: 0x000029D1
		public int BaseBackgroundId { get; private set; }

		// Token: 0x060000E7 RID: 231 RVA: 0x000047DA File Offset: 0x000029DA
		private BannerManager()
		{
			this._bannerIconGroups = new MBList<BannerIconGroup>();
			this._colorPalette = new Dictionary<int, BannerColor>();
			this.ReadOnlyColorPalette = this._colorPalette.GetReadOnlyDictionary<int, BannerColor>();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004809 File Offset: 0x00002A09
		public static void Initialize()
		{
			if (BannerManager.Instance == null)
			{
				BannerManager.Instance = new BannerManager();
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x0000481C File Offset: 0x00002A1C
		public static MBReadOnlyDictionary<int, BannerColor> ColorPalette
		{
			get
			{
				return BannerManager.Instance.ReadOnlyColorPalette;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004828 File Offset: 0x00002A28
		public static uint GetColor(int id)
		{
			if (BannerManager.ColorPalette.ContainsKey(id))
			{
				return BannerManager.ColorPalette[id].Color;
			}
			return 3735928559U;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000485C File Offset: 0x00002A5C
		public static int GetColorId(uint color)
		{
			foreach (KeyValuePair<int, BannerColor> keyValuePair in BannerManager.ColorPalette)
			{
				if (keyValuePair.Value.Color == color)
				{
					return keyValuePair.Key;
				}
			}
			return -1;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000048C8 File Offset: 0x00002AC8
		public BannerIconData GetIconDataFromIconId(int id)
		{
			using (List<BannerIconGroup>.Enumerator enumerator = this._bannerIconGroups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BannerIconData result;
					if (enumerator.Current.AllIcons.TryGetValue(id, out result))
					{
						return result;
					}
				}
			}
			return default(BannerIconData);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004934 File Offset: 0x00002B34
		public int GetRandomBackgroundId(MBFastRandom random)
		{
			int num = random.Next(0, this._availablePatternCount);
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (bannerIconGroup.IsPattern)
				{
					if (num < bannerIconGroup.AllBackgrounds.Count)
					{
						return bannerIconGroup.AllBackgrounds.ElementAt(num).Key;
					}
					num -= bannerIconGroup.AllBackgrounds.Count;
				}
			}
			return -1;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000049D0 File Offset: 0x00002BD0
		public int GetRandomBannerIconId(MBFastRandom random)
		{
			int num = random.Next(0, this._availableIconCount);
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern)
				{
					if (num < bannerIconGroup.AvailableIcons.Count)
					{
						return bannerIconGroup.AvailableIcons.ElementAt(num).Key;
					}
					num -= bannerIconGroup.AvailableIcons.Count;
				}
			}
			return -1;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004A6C File Offset: 0x00002C6C
		public string GetBackgroundMeshName(int id)
		{
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (bannerIconGroup.IsPattern && bannerIconGroup.AllBackgrounds.ContainsKey(id))
				{
					return bannerIconGroup.AllBackgrounds[id];
				}
			}
			return null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public string GetIconSourceTextureName(int id)
		{
			foreach (BannerIconGroup bannerIconGroup in this.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern && bannerIconGroup.AllBackgrounds.ContainsKey(id))
				{
					return bannerIconGroup.AllBackgrounds[id];
				}
			}
			return null;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004B54 File Offset: 0x00002D54
		public void SetBaseBackgroundId(int id)
		{
			this.BaseBackgroundId = id;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004B60 File Offset: 0x00002D60
		public void LoadBannerIcons(string xmlPath)
		{
			XmlDocument doc = this.LoadXmlFile(xmlPath);
			this.LoadFromXml(doc);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004B7C File Offset: 0x00002D7C
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

		// Token: 0x060000F4 RID: 244 RVA: 0x00004BC8 File Offset: 0x00002DC8
		private void LoadFromXml(XmlDocument doc)
		{
			Debug.Print("loading banner_icons.xml:", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			if (doc.ChildNodes.get_ItemOf(1).Name != "base")
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			XmlNode xmlNode = doc.ChildNodes.get_ItemOf(1).ChildNodes.get_ItemOf(0);
			if (xmlNode.Name != "BannerIconData")
			{
				throw new TWXmlLoadException("Incorrect XML document format.");
			}
			if (xmlNode.Name == "BannerIconData")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.Name == "BannerIconGroup")
					{
						BannerIconGroup bannerIconGroup = new BannerIconGroup();
						bannerIconGroup.Deserialize(xmlNode2, this._bannerIconGroups);
						BannerIconGroup bannerIconGroup3 = this._bannerIconGroups.FirstOrDefault((BannerIconGroup x) => x.Id == bannerIconGroup.Id);
						if (bannerIconGroup3 == null)
						{
							this._bannerIconGroups.Add(bannerIconGroup);
						}
						else
						{
							bannerIconGroup3.Merge(bannerIconGroup);
						}
					}
					if (xmlNode2.Name == "BannerColors")
					{
						foreach (object obj2 in xmlNode2.ChildNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.Name == "Color")
							{
								int key = Convert.ToInt32(xmlNode3.Attributes.get_ItemOf("id").Value);
								if (!this._colorPalette.ContainsKey(key))
								{
									uint color = Convert.ToUInt32(xmlNode3.Attributes.get_ItemOf("hex").Value, 16);
									XmlAttribute xmlAttribute = xmlNode3.Attributes.get_ItemOf("player_can_choose_for_sigil");
									bool playerCanChooseForSigil = Convert.ToBoolean(((xmlAttribute != null) ? xmlAttribute.Value : null) ?? "false");
									XmlAttribute xmlAttribute2 = xmlNode3.Attributes.get_ItemOf("player_can_choose_for_background");
									bool playerCanChooseForBackground = Convert.ToBoolean(((xmlAttribute2 != null) ? xmlAttribute2.Value : null) ?? "false");
									this._colorPalette.Add(key, new BannerColor(color, playerCanChooseForSigil, playerCanChooseForBackground));
								}
							}
						}
					}
				}
			}
			this._availablePatternCount = 0;
			this._availableIconCount = 0;
			foreach (BannerIconGroup bannerIconGroup2 in this._bannerIconGroups)
			{
				if (bannerIconGroup2.IsPattern)
				{
					this._availablePatternCount += bannerIconGroup2.AllBackgrounds.Count;
				}
				else
				{
					this._availableIconCount += bannerIconGroup2.AvailableIcons.Count;
				}
			}
		}

		// Token: 0x040000F9 RID: 249
		public const int DarkRed = 1;

		// Token: 0x040000FA RID: 250
		public const int Green = 120;

		// Token: 0x040000FB RID: 251
		public const int Blue = 119;

		// Token: 0x040000FC RID: 252
		public const int Purple = 4;

		// Token: 0x040000FD RID: 253
		public const int DarkPurple = 6;

		// Token: 0x040000FE RID: 254
		public const int Orange = 9;

		// Token: 0x040000FF RID: 255
		public const int DarkBlue = 12;

		// Token: 0x04000100 RID: 256
		public const int Red = 118;

		// Token: 0x04000101 RID: 257
		public const int Yellow = 121;

		// Token: 0x04000103 RID: 259
		public MBReadOnlyDictionary<int, BannerColor> ReadOnlyColorPalette;

		// Token: 0x04000104 RID: 260
		private Dictionary<int, BannerColor> _colorPalette;

		// Token: 0x04000105 RID: 261
		private MBList<BannerIconGroup> _bannerIconGroups;

		// Token: 0x04000107 RID: 263
		private int _availablePatternCount;

		// Token: 0x04000108 RID: 264
		private int _availableIconCount;
	}
}
