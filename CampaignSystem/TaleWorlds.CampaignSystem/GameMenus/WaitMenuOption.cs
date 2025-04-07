using System;
using System.Reflection;
using System.Xml;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameMenus
{
	// Token: 0x020000E5 RID: 229
	public class WaitMenuOption
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001435 RID: 5173 RVA: 0x00059D81 File Offset: 0x00057F81
		// (set) Token: 0x06001436 RID: 5174 RVA: 0x00059D89 File Offset: 0x00057F89
		public int Priority { get; private set; }

		// Token: 0x06001437 RID: 5175 RVA: 0x00059D92 File Offset: 0x00057F92
		internal WaitMenuOption()
		{
			this.Priority = 100;
			this._text = TextObject.Empty;
			this._tooltip = "";
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00059DB8 File Offset: 0x00057FB8
		internal WaitMenuOption(string idString, TextObject text, WaitMenuOption.OnConditionDelegate condition, WaitMenuOption.OnConsequenceDelegate consequence, int priority = 100, string tooltip = "")
		{
			this._idstring = idString;
			this._text = text;
			this.OnCondition = condition;
			this.OnConsequence = consequence;
			this.Priority = priority;
			this._tooltip = tooltip;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00059DF0 File Offset: 0x00057FF0
		public bool GetConditionsHold(Game game, MapState mapState)
		{
			if (this.OnCondition != null)
			{
				MenuCallbackArgs args = new MenuCallbackArgs(mapState, this.Text);
				return this.OnCondition(args);
			}
			return true;
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00059E20 File Offset: 0x00058020
		public TextObject Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x00059E28 File Offset: 0x00058028
		public string IdString
		{
			get
			{
				return this._idstring;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00059E30 File Offset: 0x00058030
		public string Tooltip
		{
			get
			{
				return this._tooltip;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00059E38 File Offset: 0x00058038
		public bool IsLeave
		{
			get
			{
				return this._isLeave;
			}
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x00059E40 File Offset: 0x00058040
		public void RunConsequence(Game game, MapState mapState)
		{
			if (this.OnConsequence != null)
			{
				MenuCallbackArgs args = new MenuCallbackArgs(mapState, this.Text);
				this.OnConsequence(args);
			}
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x00059E70 File Offset: 0x00058070
		public void Deserialize(XmlNode node, Type typeOfWaitMenusCallbacks)
		{
			if (node.Attributes == null)
			{
				throw new TWXmlLoadException("node.Attributes != null");
			}
			this._idstring = node.Attributes.get_ItemOf("id").Value;
			XmlNode xmlNode = node.Attributes.get_ItemOf("text");
			if (xmlNode != null)
			{
				this._text = new TextObject(xmlNode.InnerText, null);
			}
			if (node.Attributes.get_ItemOf("is_leave") != null)
			{
				this._isLeave = true;
			}
			XmlNode xmlNode2 = node.Attributes.get_ItemOf("on_condition");
			if (xmlNode2 != null)
			{
				string innerText = xmlNode2.InnerText;
				this._methodOnCondition = typeOfWaitMenusCallbacks.GetMethod(innerText);
				if (this._methodOnCondition == null)
				{
					throw new MBNotFoundException("Can not find WaitMenuOption condition:" + innerText);
				}
				this.OnCondition = (WaitMenuOption.OnConditionDelegate)Delegate.CreateDelegate(typeof(WaitMenuOption.OnConditionDelegate), null, this._methodOnCondition);
			}
			XmlNode xmlNode3 = node.Attributes.get_ItemOf("on_consequence");
			if (xmlNode3 != null)
			{
				string innerText2 = xmlNode3.InnerText;
				this._methodOnConsequence = typeOfWaitMenusCallbacks.GetMethod(innerText2);
				if (this._methodOnConsequence == null)
				{
					throw new MBNotFoundException("Can not find WaitMenuOption consequence:" + innerText2);
				}
				this.OnConsequence = (WaitMenuOption.OnConsequenceDelegate)Delegate.CreateDelegate(typeof(WaitMenuOption.OnConsequenceDelegate), null, this._methodOnConsequence);
			}
		}

		// Token: 0x040006FA RID: 1786
		private string _idstring;

		// Token: 0x040006FB RID: 1787
		private TextObject _text;

		// Token: 0x040006FC RID: 1788
		private string _tooltip;

		// Token: 0x040006FD RID: 1789
		private MethodInfo _methodOnCondition;

		// Token: 0x040006FE RID: 1790
		public WaitMenuOption.OnConditionDelegate OnCondition;

		// Token: 0x040006FF RID: 1791
		private MethodInfo _methodOnConsequence;

		// Token: 0x04000700 RID: 1792
		public WaitMenuOption.OnConsequenceDelegate OnConsequence;

		// Token: 0x04000701 RID: 1793
		private bool _isLeave;

		// Token: 0x020004F3 RID: 1267
		// (Invoke) Token: 0x06004390 RID: 17296
		public delegate bool OnConditionDelegate(MenuCallbackArgs args);

		// Token: 0x020004F4 RID: 1268
		// (Invoke) Token: 0x06004394 RID: 17300
		public delegate void OnConsequenceDelegate(MenuCallbackArgs args);
	}
}
