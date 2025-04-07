﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002F1 RID: 753
	public class PlayerRetiredLogEntry : LogEntry, IEncyclopediaLog
	{
		// Token: 0x06002BDF RID: 11231 RVA: 0x000B90C0 File Offset: 0x000B72C0
		internal static void AutoGeneratedStaticCollectObjectsPlayerRetiredLogEntry(object o, List<object> collectedObjects)
		{
			((PlayerRetiredLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000B90CE File Offset: 0x000B72CE
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Retiree);
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000B90E3 File Offset: 0x000B72E3
		internal static object AutoGeneratedGetMemberValueRetiree(object o)
		{
			return ((PlayerRetiredLogEntry)o).Retiree;
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000B90F0 File Offset: 0x000B72F0
		public PlayerRetiredLogEntry(Hero retireeHero)
		{
			this.Retiree = retireeHero;
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000B90FF File Offset: 0x000B72FF
		public override string ToString()
		{
			return this.GetEncyclopediaText().ToString();
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000B910C File Offset: 0x000B730C
		public TextObject GetEncyclopediaText()
		{
			TextObject textObject = new TextObject("{=mg0yAzIb}{RETIREE.LINK} has retired.", null);
			StringHelpers.SetCharacterProperties("RETIREE", this.Retiree.CharacterObject, textObject, false);
			return textObject;
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000B913E File Offset: 0x000B733E
		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase
		{
			return obj == this.Retiree;
		}

		// Token: 0x04000D27 RID: 3367
		[SaveableField(10)]
		public readonly Hero Retiree;
	}
}
