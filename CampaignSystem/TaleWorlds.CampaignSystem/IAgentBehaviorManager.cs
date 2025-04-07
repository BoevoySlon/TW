using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000086 RID: 134
	public interface IAgentBehaviorManager
	{
		// Token: 0x06001079 RID: 4217
		void AddQuestCharacterBehaviors(IAgent agent);

		// Token: 0x0600107A RID: 4218
		void AddWandererBehaviors(IAgent agent);

		// Token: 0x0600107B RID: 4219
		void AddOutdoorWandererBehaviors(IAgent agent);

		// Token: 0x0600107C RID: 4220
		void AddIndoorWandererBehaviors(IAgent agent);

		// Token: 0x0600107D RID: 4221
		void AddFixedCharacterBehaviors(IAgent agent);

		// Token: 0x0600107E RID: 4222
		void AddAmbushPlayerBehaviors(IAgent agent);

		// Token: 0x0600107F RID: 4223
		void AddStandGuardBehaviors(IAgent agent);

		// Token: 0x06001080 RID: 4224
		void AddPatrollingGuardBehaviors(IAgent agent);

		// Token: 0x06001081 RID: 4225
		void AddCompanionBehaviors(IAgent agent);

		// Token: 0x06001082 RID: 4226
		void AddBodyguardBehaviors(IAgent agent);

		// Token: 0x06001083 RID: 4227
		void AddFirstCompanionBehavior(IAgent agent);
	}
}
