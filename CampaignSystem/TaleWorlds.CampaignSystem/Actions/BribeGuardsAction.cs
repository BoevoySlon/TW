using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000429 RID: 1065
	public static class BribeGuardsAction
	{
		// Token: 0x06004053 RID: 16467 RVA: 0x0013D330 File Offset: 0x0013B530
		private static void ApplyInternal(Settlement settlement, int gold)
		{
			if (gold > 0)
			{
				if (MBRandom.RandomFloat < (float)gold / 1000f)
				{
					SkillLevelingManager.OnBribeGiven(gold);
				}
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, gold, false);
				settlement.BribePaid += gold;
			}
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0013D366 File Offset: 0x0013B566
		public static void Apply(Settlement settlement, int gold)
		{
			BribeGuardsAction.ApplyInternal(settlement, gold);
		}
	}
}
