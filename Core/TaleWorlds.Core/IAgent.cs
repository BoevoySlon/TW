using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000076 RID: 118
	public interface IAgent
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000792 RID: 1938
		BasicCharacterObject Character { get; }

		// Token: 0x06000793 RID: 1939
		bool IsEnemyOf(IAgent agent);

		// Token: 0x06000794 RID: 1940
		bool IsFriendOf(IAgent agent);

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000795 RID: 1941
		AgentState State { get; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000796 RID: 1942
		IMissionTeam Team { get; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000797 RID: 1943
		IAgentOriginBase Origin { get; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000798 RID: 1944
		float Age { get; }

		// Token: 0x06000799 RID: 1945
		bool IsActive();

		// Token: 0x0600079A RID: 1946
		void SetAsConversationAgent(bool set);
	}
}
