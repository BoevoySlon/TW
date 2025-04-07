using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000078 RID: 120
	internal class DialogFlowLine
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x000482BD File Offset: 0x000464BD
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x000482B4 File Offset: 0x000464B4
		public List<KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>> Variations { get; private set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000F39 RID: 3897 RVA: 0x000482C5 File Offset: 0x000464C5
		public bool HasVariation
		{
			get
			{
				return this.Variations.Count > 0;
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000482D5 File Offset: 0x000464D5
		internal DialogFlowLine()
		{
			this.Variations = new List<KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>>();
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x000482E8 File Offset: 0x000464E8
		public void AddVariation(TextObject text, List<GameTextManager.ChoiceTag> list)
		{
			this.Variations.Add(new KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>(text, list));
		}

		// Token: 0x0400050F RID: 1295
		internal TextObject Text;

		// Token: 0x04000510 RID: 1296
		internal string InputToken;

		// Token: 0x04000511 RID: 1297
		internal string OutputToken;

		// Token: 0x04000512 RID: 1298
		internal bool ByPlayer;

		// Token: 0x04000513 RID: 1299
		internal ConversationSentence.OnConditionDelegate ConditionDelegate;

		// Token: 0x04000514 RID: 1300
		internal ConversationSentence.OnClickableConditionDelegate ClickableConditionDelegate;

		// Token: 0x04000515 RID: 1301
		internal ConversationSentence.OnConsequenceDelegate ConsequenceDelegate;

		// Token: 0x04000516 RID: 1302
		internal ConversationSentence.OnMultipleConversationConsequenceDelegate SpeakerDelegate;

		// Token: 0x04000517 RID: 1303
		internal ConversationSentence.OnMultipleConversationConsequenceDelegate ListenerDelegate;

		// Token: 0x04000518 RID: 1304
		internal bool IsRepeatable;

		// Token: 0x04000519 RID: 1305
		internal bool IsSpecialOption;
	}
}
