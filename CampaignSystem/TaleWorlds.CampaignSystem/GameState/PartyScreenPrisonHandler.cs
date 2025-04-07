using System;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x02000341 RID: 833
	public interface PartyScreenPrisonHandler
	{
		// Token: 0x06002F15 RID: 12053
		void ExecuteTakeAllPrisonersScript();

		// Token: 0x06002F16 RID: 12054
		void ExecuteDoneScript();

		// Token: 0x06002F17 RID: 12055
		void ExecuteResetScript();

		// Token: 0x06002F18 RID: 12056
		void ExecuteSellAllPrisoners();
	}
}
