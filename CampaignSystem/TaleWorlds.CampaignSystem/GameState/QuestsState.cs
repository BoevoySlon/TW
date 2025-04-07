using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000342 RID: 834
	public class QuestsState : GameState
	{
		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x06002F19 RID: 12057 RVA: 0x000C263C File Offset: 0x000C083C
		// (set) Token: 0x06002F1A RID: 12058 RVA: 0x000C2644 File Offset: 0x000C0844
		public IssueBase InitialSelectedIssue { get; private set; }

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x06002F1B RID: 12059 RVA: 0x000C264D File Offset: 0x000C084D
		// (set) Token: 0x06002F1C RID: 12060 RVA: 0x000C2655 File Offset: 0x000C0855
		public QuestBase InitialSelectedQuest { get; private set; }

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x06002F1D RID: 12061 RVA: 0x000C265E File Offset: 0x000C085E
		// (set) Token: 0x06002F1E RID: 12062 RVA: 0x000C2666 File Offset: 0x000C0866
		public JournalLogEntry InitialSelectedLog { get; private set; }

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06002F1F RID: 12063 RVA: 0x000C266F File Offset: 0x000C086F
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000C2672 File Offset: 0x000C0872
		// (set) Token: 0x06002F21 RID: 12065 RVA: 0x000C267A File Offset: 0x000C087A
		public IQuestsStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000C2683 File Offset: 0x000C0883
		public QuestsState()
		{
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000C268B File Offset: 0x000C088B
		public QuestsState(IssueBase initialSelectedIssue)
		{
			this.InitialSelectedIssue = initialSelectedIssue;
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000C269A File Offset: 0x000C089A
		public QuestsState(QuestBase initialSelectedQuest)
		{
			this.InitialSelectedQuest = initialSelectedQuest;
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000C26A9 File Offset: 0x000C08A9
		public QuestsState(JournalLogEntry initialSelectedLog)
		{
			this.InitialSelectedLog = initialSelectedLog;
		}

		// Token: 0x04000E03 RID: 3587
		private IQuestsStateHandler _handler;
	}
}
