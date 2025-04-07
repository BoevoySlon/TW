using System;
using System.Collections.Generic;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000089 RID: 137
	public interface ICampaignBehaviorManager
	{
		// Token: 0x06001091 RID: 4241
		void RegisterEvents();

		// Token: 0x06001092 RID: 4242
		T GetBehavior<T>();

		// Token: 0x06001093 RID: 4243
		IEnumerable<T> GetBehaviors<T>();

		// Token: 0x06001094 RID: 4244
		void AddBehavior(CampaignBehaviorBase campaignBehavior);

		// Token: 0x06001095 RID: 4245
		void RemoveBehavior<T>() where T : CampaignBehaviorBase;

		// Token: 0x06001096 RID: 4246
		void ClearBehaviors();

		// Token: 0x06001097 RID: 4247
		void LoadBehaviorData();

		// Token: 0x06001098 RID: 4248
		void InitializeCampaignBehaviors(IEnumerable<CampaignBehaviorBase> inputComponents);
	}
}
