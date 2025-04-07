﻿using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.LogEntries
{
	// Token: 0x020002CD RID: 717
	public class CharacterBecameFugitiveLogEntry : LogEntry, IEncyclopediaLog
	{
		// Token: 0x06002A40 RID: 10816 RVA: 0x000B4AFB File Offset: 0x000B2CFB
		internal static void AutoGeneratedStaticCollectObjectsCharacterBecameFugitiveLogEntry(object o, List<object> collectedObjects)
		{
			((CharacterBecameFugitiveLogEntry)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000B4B09 File Offset: 0x000B2D09
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.Hero);
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000B4B1E File Offset: 0x000B2D1E
		internal static object AutoGeneratedGetMemberValueHero(object o)
		{
			return ((CharacterBecameFugitiveLogEntry)o).Hero;
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000B4B2B File Offset: 0x000B2D2B
		public CharacterBecameFugitiveLogEntry(Hero hero)
		{
			this.Hero = hero;
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000B4B3A File Offset: 0x000B2D3A
		public override string ToString()
		{
			return this.GetEncyclopediaText().ToString();
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000B4B47 File Offset: 0x000B2D47
		public bool IsVisibleInEncyclopediaPageOf<T>(T obj) where T : MBObjectBase
		{
			return obj == this.Hero;
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000B4B58 File Offset: 0x000B2D58
		public TextObject GetEncyclopediaText()
		{
			TextObject textObject = GameTexts.FindText("str_fugitive_news", null);
			StringHelpers.SetCharacterProperties("HERO", this.Hero.CharacterObject, textObject, false);
			return textObject;
		}

		// Token: 0x04000CB4 RID: 3252
		[SaveableField(90)]
		public readonly Hero Hero;
	}
}
