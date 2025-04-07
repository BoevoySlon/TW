using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000443 RID: 1091
	public static class GainRenownAction
	{
		// Token: 0x060040CA RID: 16586 RVA: 0x0013F3CE File Offset: 0x0013D5CE
		private static void ApplyInternal(Hero hero, float gainedRenown, bool doNotNotify)
		{
			if (gainedRenown > 0f)
			{
				hero.Clan.AddRenown(gainedRenown, true);
				CampaignEventDispatcher.Instance.OnRenownGained(hero, (int)gainedRenown, doNotNotify);
			}
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x0013F3F3 File Offset: 0x0013D5F3
		public static void Apply(Hero hero, float renownValue, bool doNotNotify = false)
		{
			GainRenownAction.ApplyInternal(hero, renownValue, doNotNotify);
		}
	}
}
