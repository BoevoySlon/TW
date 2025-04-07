﻿using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x0200029C RID: 668
	public struct PartyTemplateStack
	{
		// Token: 0x060026BB RID: 9915 RVA: 0x000A49F8 File Offset: 0x000A2BF8
		public static void AutoGeneratedStaticCollectObjectsPartyTemplateStack(object o, List<object> collectedObjects)
		{
			((PartyTemplateStack)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000A4A14 File Offset: 0x000A2C14
		private void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this.Character);
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000A4A22 File Offset: 0x000A2C22
		internal static object AutoGeneratedGetMemberValueCharacter(object o)
		{
			return ((PartyTemplateStack)o).Character;
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000A4A2F File Offset: 0x000A2C2F
		internal static object AutoGeneratedGetMemberValueMinValue(object o)
		{
			return ((PartyTemplateStack)o).MinValue;
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000A4A41 File Offset: 0x000A2C41
		internal static object AutoGeneratedGetMemberValueMaxValue(object o)
		{
			return ((PartyTemplateStack)o).MaxValue;
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000A4A53 File Offset: 0x000A2C53
		public PartyTemplateStack(CharacterObject character, int minValue, int maxValue)
		{
			this.Character = character;
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}

		// Token: 0x04000B99 RID: 2969
		[SaveableField(0)]
		public CharacterObject Character;

		// Token: 0x04000B9A RID: 2970
		[SaveableField(1)]
		public int MinValue;

		// Token: 0x04000B9B RID: 2971
		[SaveableField(2)]
		public int MaxValue;
	}
}
