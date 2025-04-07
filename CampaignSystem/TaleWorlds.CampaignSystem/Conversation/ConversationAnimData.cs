using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x020001E5 RID: 485
	public class ConversationAnimData
	{
		// Token: 0x06001D80 RID: 7552 RVA: 0x0008508B File Offset: 0x0008328B
		public ConversationAnimData()
		{
			this.Reactions = new Dictionary<string, string>();
		}

		// Token: 0x0400090F RID: 2319
		[SaveableField(0)]
		public string IdleAnimStart;

		// Token: 0x04000910 RID: 2320
		[SaveableField(1)]
		public string IdleAnimLoop;

		// Token: 0x04000911 RID: 2321
		[SaveableField(2)]
		public int FamilyType;

		// Token: 0x04000912 RID: 2322
		[SaveableField(3)]
		public int MountFamilyType;

		// Token: 0x04000913 RID: 2323
		[SaveableField(4)]
		public Dictionary<string, string> Reactions;
	}
}
