using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200039C RID: 924
	public interface IFacegenCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x06003792 RID: 14226
		IFaceGeneratorCustomFilter GetFaceGenFilter();
	}
}
