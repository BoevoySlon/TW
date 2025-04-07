using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000023 RID: 35
	public class BattleSimulationResultArgs
	{
		// Token: 0x06000154 RID: 340 RVA: 0x0000EC07 File Offset: 0x0000CE07
		public BattleSimulationResultArgs()
		{
			this.RoundResults = new List<BattleSimulationResult>();
		}

		// Token: 0x0400002D RID: 45
		public List<BattleSimulationResult> RoundResults;
	}
}
