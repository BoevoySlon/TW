using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000083 RID: 131
	public class GameSceneDataManager
	{
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x0004A63C File Offset: 0x0004883C
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x0004A643 File Offset: 0x00048843
		public static GameSceneDataManager Instance { get; private set; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x0004A64B File Offset: 0x0004884B
		public MBReadOnlyList<SingleplayerBattleSceneData> SingleplayerBattleScenes
		{
			get
			{
				return this._singleplayerBattleScenes;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0004A653 File Offset: 0x00048853
		public MBReadOnlyList<ConversationSceneData> ConversationScenes
		{
			get
			{
				return this._conversationScenes;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x0004A65B File Offset: 0x0004885B
		public MBReadOnlyList<MeetingSceneData> MeetingScenes
		{
			get
			{
				return this._meetingScenes;
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0004A663 File Offset: 0x00048863
		public GameSceneDataManager()
		{
			this._singleplayerBattleScenes = new MBList<SingleplayerBattleSceneData>();
			this._conversationScenes = new MBList<ConversationSceneData>();
			this._meetingScenes = new MBList<MeetingSceneData>();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0004A68C File Offset: 0x0004888C
		internal static void Initialize()
		{
			GameSceneDataManager.Instance = new GameSceneDataManager();
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0004A698 File Offset: 0x00048898
		internal static void Destroy()
		{
			GameSceneDataManager.Instance = null;
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0004A6A0 File Offset: 0x000488A0
		public void LoadSPBattleScenes(string path)
		{
			XmlDocument doc = this.LoadXmlFile(path);
			this.LoadSPBattleScenes(doc);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0004A6BC File Offset: 0x000488BC
		public void LoadConversationScenes(string path)
		{
			XmlDocument doc = this.LoadXmlFile(path);
			this.LoadConversationScenes(doc);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0004A6D8 File Offset: 0x000488D8
		public void LoadMeetingScenes(string path)
		{
			XmlDocument doc = this.LoadXmlFile(path);
			this.LoadMeetingScenes(doc);
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0004A6F4 File Offset: 0x000488F4
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

		// Token: 0x0600106C RID: 4204 RVA: 0x0004A740 File Offset: 0x00048940
		private void LoadSPBattleScenes(XmlDocument doc)
		{
			this._singleplayerBattleScenes.Clear();
			Debug.Print("loading sp_battles.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes.get_ItemOf(1);
			if (xmlNode.Name != "SPBattleScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be SPBattleScenes.");
			}
			if (xmlNode.Name == "SPBattleScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != 8)
					{
						string sceneID = null;
						List<int> list = new List<int>();
						TerrainType terrain = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes.get_ItemOf(i).Name == "id")
							{
								sceneID = xmlNode2.Attributes.get_ItemOf(i).InnerText;
							}
							else if (xmlNode2.Attributes.get_ItemOf(i).Name == "map_indices")
							{
								foreach (string s in xmlNode2.Attributes.get_ItemOf(i).InnerText.Replace(" ", "").Split(new char[]
								{
									','
								}))
								{
									list.Add(int.Parse(s));
								}
							}
							else if (xmlNode2.Attributes.get_ItemOf(i).Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes.get_ItemOf(i).InnerText, out terrain))
								{
									terrain = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes.get_ItemOf(i).Name == "forest_density")
							{
								char[] array2 = xmlNode2.Attributes.get_ItemOf(i).InnerText.ToLower().ToCharArray();
								array2[0] = char.ToUpper(array2[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array2), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
						}
						XmlNodeList childNodes = xmlNode2.ChildNodes;
						List<TerrainType> list2 = new List<TerrainType>();
						foreach (object obj2 in childNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != 8 && xmlNode3.Name == "TerrainTypes")
							{
								foreach (object obj3 in xmlNode3.ChildNodes)
								{
									XmlNode xmlNode4 = (XmlNode)obj3;
									TerrainType item;
									if (xmlNode4.Name == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes.get_ItemOf("name").InnerText, out item) && !list2.Contains(item))
									{
										list2.Add(item);
									}
								}
							}
						}
						this._singleplayerBattleScenes.Add(new SingleplayerBattleSceneData(sceneID, terrain, list2, forestDensity, list));
					}
				}
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0004AAE4 File Offset: 0x00048CE4
		private void LoadConversationScenes(XmlDocument doc)
		{
			Debug.Print("loading conversation_scenes.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes.get_ItemOf(1);
			if (xmlNode.Name != "ConversationScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be ConversationScenes.");
			}
			if (xmlNode.Name == "ConversationScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != 8)
					{
						string sceneID = null;
						TerrainType terrain = TerrainType.Plain;
						ForestDensity forestDensity = ForestDensity.None;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes.get_ItemOf(i).Name == "id")
							{
								sceneID = xmlNode2.Attributes.get_ItemOf(i).InnerText;
							}
							else if (xmlNode2.Attributes.get_ItemOf(i).Name == "terrain")
							{
								if (!Enum.TryParse<TerrainType>(xmlNode2.Attributes.get_ItemOf(i).InnerText, out terrain))
								{
									terrain = TerrainType.Plain;
								}
							}
							else if (xmlNode2.Attributes.get_ItemOf(i).Name == "forest_density")
							{
								char[] array = xmlNode2.Attributes.get_ItemOf(i).InnerText.ToLower().ToCharArray();
								array[0] = char.ToUpper(array[0]);
								if (!Enum.TryParse<ForestDensity>(new string(array), out forestDensity))
								{
									forestDensity = ForestDensity.None;
								}
							}
						}
						XmlNodeList childNodes = xmlNode2.ChildNodes;
						List<TerrainType> list = new List<TerrainType>();
						foreach (object obj2 in childNodes)
						{
							XmlNode xmlNode3 = (XmlNode)obj2;
							if (xmlNode3.NodeType != 8 && xmlNode3.Name == "flags")
							{
								foreach (object obj3 in xmlNode3.ChildNodes)
								{
									XmlNode xmlNode4 = (XmlNode)obj3;
									TerrainType item;
									if (xmlNode4.NodeType != 8 && xmlNode4.Attributes.get_ItemOf("name").InnerText == "TerrainType" && Enum.TryParse<TerrainType>(xmlNode4.Attributes.get_ItemOf("value").InnerText, out item) && !list.Contains(item))
									{
										list.Add(item);
									}
								}
							}
						}
						this._conversationScenes.Add(new ConversationSceneData(sceneID, terrain, list, forestDensity));
					}
				}
			}
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0004AE10 File Offset: 0x00049010
		private void LoadMeetingScenes(XmlDocument doc)
		{
			Debug.Print("loading meeting_scenes.xml", 0, Debug.DebugColor.White, 17592186044416UL);
			if (doc.ChildNodes.Count <= 1)
			{
				throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least 2 child nodes.");
			}
			XmlNode xmlNode = doc.ChildNodes.get_ItemOf(1);
			if (xmlNode.Name != "MeetingScenes")
			{
				throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be MeetingScenes.");
			}
			if (xmlNode.Name == "MeetingScenes")
			{
				foreach (object obj in xmlNode.ChildNodes)
				{
					XmlNode xmlNode2 = (XmlNode)obj;
					if (xmlNode2.NodeType != 8)
					{
						string sceneID = null;
						string cultureString = null;
						for (int i = 0; i < xmlNode2.Attributes.Count; i++)
						{
							if (xmlNode2.Attributes.get_ItemOf(i).Name == "id")
							{
								sceneID = xmlNode2.Attributes.get_ItemOf(i).InnerText;
							}
							if (xmlNode2.Attributes.get_ItemOf(i).Name == "culture")
							{
								cultureString = xmlNode2.Attributes.get_ItemOf(i).InnerText.Split(new char[]
								{
									'.'
								})[1];
							}
						}
						this._meetingScenes.Add(new MeetingSceneData(sceneID, cultureString));
					}
				}
			}
		}

		// Token: 0x040005BC RID: 1468
		private MBList<SingleplayerBattleSceneData> _singleplayerBattleScenes;

		// Token: 0x040005BD RID: 1469
		private MBList<ConversationSceneData> _conversationScenes;

		// Token: 0x040005BE RID: 1470
		private MBList<MeetingSceneData> _meetingScenes;

		// Token: 0x040005BF RID: 1471
		private const TerrainType DefaultTerrain = TerrainType.Plain;

		// Token: 0x040005C0 RID: 1472
		private const ForestDensity DefaultForestDensity = ForestDensity.None;
	}
}
