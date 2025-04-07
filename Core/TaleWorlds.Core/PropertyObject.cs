﻿using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x02000033 RID: 51
	public class PropertyObject : MBObjectBase
	{
		// Token: 0x060003EB RID: 1003 RVA: 0x0000F339 File Offset: 0x0000D539
		internal static void AutoGeneratedStaticCollectObjectsPropertyObject(object o, List<object> collectedObjects)
		{
			((PropertyObject)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000F347 File Offset: 0x0000D547
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000F350 File Offset: 0x0000D550
		public TextObject Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000F358 File Offset: 0x0000D558
		public TextObject Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000F360 File Offset: 0x0000D560
		public PropertyObject(string stringId) : base(stringId)
		{
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000F369 File Offset: 0x0000D569
		public void Initialize(TextObject name, TextObject description)
		{
			base.Initialize();
			this._name = name;
			this._description = description;
		}

		// Token: 0x0400020A RID: 522
		private TextObject _name;

		// Token: 0x0400020B RID: 523
		private TextObject _description;
	}
}
