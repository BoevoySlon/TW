using System;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x0200040D RID: 1037
	public class PrisonerBarterGroup : BarterGroup
	{
		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003F2A RID: 16170 RVA: 0x001391D9 File Offset: 0x001373D9
		public override float AIDecisionWeight
		{
			get
			{
				return 0.7f;
			}
		}
	}
}
