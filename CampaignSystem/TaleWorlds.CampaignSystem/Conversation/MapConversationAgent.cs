using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x020001E4 RID: 484
	public class MapConversationAgent : IAgent
	{
		// Token: 0x06001D76 RID: 7542 RVA: 0x00085053 File Offset: 0x00083253
		public MapConversationAgent(CharacterObject characterObject)
		{
			this._characterObject = characterObject;
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x00085062 File Offset: 0x00083262
		public BasicCharacterObject Character
		{
			get
			{
				return this._characterObject;
			}
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x0008506A File Offset: 0x0008326A
		public bool IsEnemyOf(IAgent agent)
		{
			return false;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0008506D File Offset: 0x0008326D
		public bool IsFriendOf(IAgent agent)
		{
			return true;
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x00085070 File Offset: 0x00083270
		public AgentState State
		{
			get
			{
				return AgentState.Active;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x00085073 File Offset: 0x00083273
		public IMissionTeam Team
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x00085076 File Offset: 0x00083276
		public IAgentOriginBase Origin
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x00085079 File Offset: 0x00083279
		public float Age
		{
			get
			{
				return this.Character.Age;
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x00085086 File Offset: 0x00083286
		public bool IsActive()
		{
			return true;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x00085089 File Offset: 0x00083289
		public void SetAsConversationAgent(bool set)
		{
		}

		// Token: 0x0400090D RID: 2317
		private CharacterObject _characterObject;

		// Token: 0x0400090E RID: 2318
		public bool DeliveredLine;
	}
}
