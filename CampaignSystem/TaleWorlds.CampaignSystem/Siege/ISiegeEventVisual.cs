using System;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x02000286 RID: 646
	public interface ISiegeEventVisual
	{
		// Token: 0x060022BE RID: 8894
		void Initialize();

		// Token: 0x060022BF RID: 8895
		void OnSiegeEventEnd();

		// Token: 0x060022C0 RID: 8896
		void Tick();
	}
}
