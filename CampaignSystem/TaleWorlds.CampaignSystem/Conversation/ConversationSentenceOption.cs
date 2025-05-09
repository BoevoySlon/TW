﻿using System;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x020001EC RID: 492
	public struct ConversationSentenceOption
	{
		// Token: 0x04000950 RID: 2384
		public int SentenceNo;

		// Token: 0x04000951 RID: 2385
		public string Id;

		// Token: 0x04000952 RID: 2386
		public object RepeatObject;

		// Token: 0x04000953 RID: 2387
		public TextObject Text;

		// Token: 0x04000954 RID: 2388
		public string DebugInfo;

		// Token: 0x04000955 RID: 2389
		public bool IsClickable;

		// Token: 0x04000956 RID: 2390
		public bool HasPersuasion;

		// Token: 0x04000957 RID: 2391
		public string SkillName;

		// Token: 0x04000958 RID: 2392
		public string TraitName;

		// Token: 0x04000959 RID: 2393
		public bool IsSpecial;

		// Token: 0x0400095A RID: 2394
		public TextObject HintText;

		// Token: 0x0400095B RID: 2395
		public PersuasionOptionArgs PersuationOptionArgs;
	}
}
