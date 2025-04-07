using System;
using System.Xml;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000094 RID: 148
	public class MBBodyProperty : MBObjectBase
	{
		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0001B5AD File Offset: 0x000197AD
		public BodyProperties BodyPropertyMin
		{
			get
			{
				return this._bodyPropertyMin;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x0001B5B5 File Offset: 0x000197B5
		public BodyProperties BodyPropertyMax
		{
			get
			{
				return this._bodyPropertyMax;
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001B5BD File Offset: 0x000197BD
		public MBBodyProperty(string stringId) : base(stringId)
		{
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001B5C6 File Offset: 0x000197C6
		public MBBodyProperty()
		{
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001B5D0 File Offset: 0x000197D0
		public void Init(BodyProperties bodyPropertyMin, BodyProperties bodyPropertyMax)
		{
			base.Initialize();
			this._bodyPropertyMin = bodyPropertyMin;
			this._bodyPropertyMax = bodyPropertyMax;
			if (this._bodyPropertyMax.Age <= 0f)
			{
				this._bodyPropertyMax = this._bodyPropertyMin;
			}
			if (this._bodyPropertyMin.Age <= 0f)
			{
				this._bodyPropertyMin = this._bodyPropertyMax;
			}
			base.AfterInitialized();
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001B634 File Offset: 0x00019834
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "BodyPropertiesMin")
				{
					BodyProperties.FromXmlNode(xmlNode, out this._bodyPropertyMin);
				}
				else if (xmlNode.Name == "BodyPropertiesMax")
				{
					BodyProperties.FromXmlNode(xmlNode, out this._bodyPropertyMax);
				}
			}
			if (this._bodyPropertyMax.Age <= 0f)
			{
				this._bodyPropertyMax = this._bodyPropertyMin;
			}
			if (this._bodyPropertyMin.Age <= 0f)
			{
				this._bodyPropertyMin = this._bodyPropertyMax;
			}
		}

		// Token: 0x04000494 RID: 1172
		private BodyProperties _bodyPropertyMin;

		// Token: 0x04000495 RID: 1173
		private BodyProperties _bodyPropertyMax;
	}
}
