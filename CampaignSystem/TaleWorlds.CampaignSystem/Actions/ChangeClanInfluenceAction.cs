using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200042A RID: 1066
	public static class ChangeClanInfluenceAction
	{
		// Token: 0x06004055 RID: 16469 RVA: 0x0013D36F File Offset: 0x0013B56F
		private static void ApplyInternal(Clan clan, float amount)
		{
			clan.Influence += amount;
			CampaignEventDispatcher.Instance.OnClanInfluenceChanged(clan, amount);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0013D38B File Offset: 0x0013B58B
		public static void Apply(Clan clan, float amount)
		{
			ChangeClanInfluenceAction.ApplyInternal(clan, amount);
		}
	}
}
