using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000072 RID: 114
	public class GameTextManager
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x0001957D File Offset: 0x0001777D
		public GameTextManager()
		{
			this._gameTexts = new Dictionary<string, GameText>();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00019590 File Offset: 0x00017790
		public GameText GetGameText(string id)
		{
			GameText result;
			if (this._gameTexts.TryGetValue(id, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x000195B0 File Offset: 0x000177B0
		public GameText AddGameText(string id)
		{
			GameText gameText;
			if (!this._gameTexts.TryGetValue(id, out gameText))
			{
				gameText = new GameText(id);
				this._gameTexts.Add(gameText.Id, gameText);
			}
			return gameText;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x000195E8 File Offset: 0x000177E8
		public bool TryGetText(string id, string variation, out TextObject text)
		{
			text = null;
			GameText gameText;
			this._gameTexts.TryGetValue(id, out gameText);
			if (gameText != null)
			{
				if (variation == null)
				{
					text = gameText.DefaultText;
				}
				else
				{
					text = gameText.GetVariation(variation);
				}
				if (text != null)
				{
					text = text.CopyTextObject();
					text.AddIDToValue(id);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00019638 File Offset: 0x00017838
		public TextObject FindText(string id, string variation = null)
		{
			TextObject result;
			if (this.TryGetText(id, variation, out result))
			{
				return result;
			}
			TextObject result2;
			if (variation == null)
			{
				result2 = new TextObject("{=!}ERROR: Text with id " + id + " doesn't exist!", null);
			}
			else
			{
				result2 = new TextObject("{=!}ERROR: Text with id " + id + " doesn't exist! Variation: " + variation, null);
			}
			return result2;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00019688 File Offset: 0x00017888
		public IEnumerable<TextObject> FindAllTextVariations(string id)
		{
			GameText gameText;
			this._gameTexts.TryGetValue(id, out gameText);
			if (gameText != null)
			{
				foreach (GameText.GameTextVariation gameTextVariation in gameText.Variations)
				{
					yield return gameTextVariation.Text;
				}
				IEnumerator<GameText.GameTextVariation> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x000196A0 File Offset: 0x000178A0
		public void LoadGameTexts()
		{
			Game game = Game.Current;
			bool ignoreGameTypeInclusionCheck = false;
			string gameType = "";
			if (game != null)
			{
				ignoreGameTypeInclusionCheck = game.GameType.IsDevelopment;
				gameType = game.GameType.GetType().Name;
			}
			XmlDocument mergedXmlForManaged = MBObjectManager.GetMergedXmlForManaged("GameText", false, ignoreGameTypeInclusionCheck, gameType);
			try
			{
				this.LoadFromXML(mergedXmlForManaged);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00019708 File Offset: 0x00017908
		public void LoadDefaultTexts()
		{
			string text = ModuleHelper.GetModuleFullPath("Native") + "ModuleData/global_strings.xml";
			Debug.Print("opening " + text, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			StreamReader streamReader = new StreamReader(text);
			string text2 = streamReader.ReadToEnd();
			xmlDocument.LoadXml(text2);
			streamReader.Close();
			this.LoadFromXML(xmlDocument);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001976C File Offset: 0x0001796C
		private void LoadFromXML(XmlDocument doc)
		{
			XmlNode xmlNode = null;
			for (int i = 0; i < doc.ChildNodes.Count; i++)
			{
				XmlNode xmlNode2 = doc.ChildNodes.get_ItemOf(i);
				if (xmlNode2.NodeType != 8 && xmlNode2.Name == "strings" && xmlNode2.ChildNodes.Count > 0)
				{
					xmlNode = xmlNode2.ChildNodes.get_ItemOf(0);
					IL_1F8:
					while (xmlNode != null)
					{
						if (xmlNode.Name == "string" && xmlNode.NodeType != 8)
						{
							if (xmlNode.Attributes == null)
							{
								throw new TWXmlLoadException("Node attributes are null.");
							}
							string[] array = xmlNode.Attributes.get_ItemOf("id").Value.Split(new char[]
							{
								'.'
							});
							string id = array[0];
							GameText gameText = this.AddGameText(id);
							string variationId = "";
							if (array.Length > 1)
							{
								variationId = array[1];
							}
							TextObject textObject = new TextObject(xmlNode.Attributes.get_ItemOf("text").Value, null);
							List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
							foreach (object obj in xmlNode.ChildNodes)
							{
								XmlNode xmlNode3 = (XmlNode)obj;
								if (xmlNode3.Name == "tags")
								{
									XmlNodeList childNodes = xmlNode3.ChildNodes;
									for (int j = 0; j < childNodes.Count; j++)
									{
										XmlAttributeCollection attributes = childNodes.get_ItemOf(j).Attributes;
										if (attributes != null)
										{
											int weight = 1;
											if (attributes.get_ItemOf("weight") != null)
											{
												int.TryParse(attributes.get_ItemOf("weight").Value, out weight);
											}
											GameTextManager.ChoiceTag item = new GameTextManager.ChoiceTag(attributes.get_ItemOf("tag_name").Value, weight);
											list.Add(item);
										}
									}
								}
							}
							textObject.CacheTokens();
							gameText.AddVariationWithId(variationId, textObject, list);
						}
						xmlNode = xmlNode.NextSibling;
					}
					return;
				}
			}
			goto IL_1F8;
		}

		// Token: 0x040003BD RID: 957
		private readonly Dictionary<string, GameText> _gameTexts;

		// Token: 0x020000FD RID: 253
		public struct ChoiceTag
		{
			// Token: 0x1700035C RID: 860
			// (get) Token: 0x06000A47 RID: 2631 RVA: 0x000216FF File Offset: 0x0001F8FF
			// (set) Token: 0x06000A48 RID: 2632 RVA: 0x00021707 File Offset: 0x0001F907
			public string TagName { get; private set; }

			// Token: 0x1700035D RID: 861
			// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00021710 File Offset: 0x0001F910
			// (set) Token: 0x06000A4A RID: 2634 RVA: 0x00021718 File Offset: 0x0001F918
			public uint Weight { get; private set; }

			// Token: 0x1700035E RID: 862
			// (get) Token: 0x06000A4B RID: 2635 RVA: 0x00021721 File Offset: 0x0001F921
			// (set) Token: 0x06000A4C RID: 2636 RVA: 0x00021729 File Offset: 0x0001F929
			public bool IsTagReversed { get; private set; }

			// Token: 0x06000A4D RID: 2637 RVA: 0x00021732 File Offset: 0x0001F932
			public ChoiceTag(string tagName, int weight)
			{
				this = default(GameTextManager.ChoiceTag);
				this.TagName = tagName;
				this.Weight = (uint)MathF.Abs(weight);
				this.IsTagReversed = (weight < 0);
			}
		}
	}
}
