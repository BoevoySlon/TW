using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000012 RID: 18
	public class BannerIconGroup
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004307 File Offset: 0x00002507
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x0000430F File Offset: 0x0000250F
		public TextObject Name { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004318 File Offset: 0x00002518
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00004320 File Offset: 0x00002520
		public bool IsPattern { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004329 File Offset: 0x00002529
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00004331 File Offset: 0x00002531
		public int Id { get; private set; }

		// Token: 0x060000DA RID: 218 RVA: 0x0000433A File Offset: 0x0000253A
		internal BannerIconGroup()
		{
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004344 File Offset: 0x00002544
		public void Deserialize(XmlNode xmlNode, MBList<BannerIconGroup> previouslyAddedGroups)
		{
			this._allIcons = new Dictionary<int, BannerIconData>();
			this._availableIcons = new Dictionary<int, BannerIconData>();
			this._allBackgrounds = new Dictionary<int, string>();
			this.AllIcons = new MBReadOnlyDictionary<int, BannerIconData>(this._allIcons);
			this.AvailableIcons = new MBReadOnlyDictionary<int, BannerIconData>(this._availableIcons);
			this.AllBackgrounds = new MBReadOnlyDictionary<int, string>(this._allBackgrounds);
			this.Id = Convert.ToInt32(xmlNode.Attributes.get_ItemOf("id").Value);
			this.Name = new TextObject(xmlNode.Attributes.get_ItemOf("name").Value, null);
			this.IsPattern = Convert.ToBoolean(xmlNode.Attributes.get_ItemOf("is_pattern").Value);
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.Name == "Icon")
				{
					int id = Convert.ToInt32(xmlNode2.Attributes.get_ItemOf("id").Value);
					string value = xmlNode2.Attributes.get_ItemOf("material_name").Value;
					int textureIndex = int.Parse(xmlNode2.Attributes.get_ItemOf("texture_index").Value);
					if (!this._allIcons.ContainsKey(id) && !previouslyAddedGroups.Any((BannerIconGroup x) => x.AllIcons.ContainsKey(id)))
					{
						this._allIcons.Add(id, new BannerIconData(value, textureIndex));
						if (xmlNode2.Attributes.get_ItemOf("is_reserved") == null || !Convert.ToBoolean(xmlNode2.Attributes.get_ItemOf("is_reserved").Value))
						{
							this._availableIcons.Add(id, new BannerIconData(value, textureIndex));
						}
					}
				}
				else if (xmlNode2.Name == "Background")
				{
					int id = Convert.ToInt32(xmlNode2.Attributes.get_ItemOf("id").Value);
					string value2 = xmlNode2.Attributes.get_ItemOf("mesh_name").Value;
					if (xmlNode2.Attributes.get_ItemOf("is_base_background") != null && Convert.ToBoolean(xmlNode2.Attributes.get_ItemOf("is_base_background").Value))
					{
						BannerManager.Instance.SetBaseBackgroundId(id);
					}
					if (!this._allBackgrounds.ContainsKey(id) && !previouslyAddedGroups.Any((BannerIconGroup x) => x.AllBackgrounds.ContainsKey(id)))
					{
						this._allBackgrounds.Add(id, value2);
					}
				}
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004634 File Offset: 0x00002834
		public void Merge(BannerIconGroup otherGroup)
		{
			foreach (KeyValuePair<int, BannerIconData> keyValuePair in otherGroup._allIcons)
			{
				if (!this._allIcons.ContainsKey(keyValuePair.Key))
				{
					this._allIcons.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			foreach (KeyValuePair<int, string> keyValuePair2 in otherGroup._allBackgrounds)
			{
				if (!this._allBackgrounds.ContainsKey(keyValuePair2.Key))
				{
					this._allBackgrounds.Add(keyValuePair2.Key, keyValuePair2.Value);
				}
			}
			foreach (KeyValuePair<int, BannerIconData> keyValuePair3 in otherGroup._availableIcons)
			{
				if (!this._availableIcons.ContainsKey(keyValuePair3.Key))
				{
					this._availableIcons.Add(keyValuePair3.Key, keyValuePair3.Value);
				}
			}
		}

		// Token: 0x040000EF RID: 239
		public MBReadOnlyDictionary<int, BannerIconData> AllIcons;

		// Token: 0x040000F0 RID: 240
		public MBReadOnlyDictionary<int, string> AllBackgrounds;

		// Token: 0x040000F1 RID: 241
		public MBReadOnlyDictionary<int, BannerIconData> AvailableIcons;

		// Token: 0x040000F2 RID: 242
		private Dictionary<int, BannerIconData> _allIcons;

		// Token: 0x040000F3 RID: 243
		private Dictionary<int, string> _allBackgrounds;

		// Token: 0x040000F4 RID: 244
		private Dictionary<int, BannerIconData> _availableIcons;
	}
}
