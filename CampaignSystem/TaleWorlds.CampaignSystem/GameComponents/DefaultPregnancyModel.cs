using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000129 RID: 297
	public class DefaultPregnancyModel : PregnancyModel
	{
		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060016FA RID: 5882 RVA: 0x00071710 File Offset: 0x0006F910
		public override float PregnancyDurationInDays
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x00071717 File Offset: 0x0006F917
		public override float MaternalMortalityProbabilityInLabor
		{
			get
			{
				return 0.015f;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x0007171E File Offset: 0x0006F91E
		public override float StillbirthProbability
		{
			get
			{
				return 0.01f;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00071725 File Offset: 0x0006F925
		public override float DeliveringFemaleOffspringProbability
		{
			get
			{
				return 0.51f;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x0007172C File Offset: 0x0006F92C
		public override float DeliveringTwinsProbability
		{
			get
			{
				return 0.03f;
			}
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00071733 File Offset: 0x0006F933
		private bool IsHeroAgeSuitableForPregnancy(Hero hero)
		{
			return hero.Age >= 18f && hero.Age <= 45f;
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00071754 File Offset: 0x0006F954
		public override float GetDailyChanceOfPregnancyForHero(Hero hero)
		{
			int num = hero.Children.Count + 1;
			float num2 = (float)(4 + 4 * hero.Clan.Tier);
			int num3 = hero.Clan.Lords.Count((Hero x) => x.IsAlive);
			float num4 = (hero != Hero.MainHero && hero.Spouse != Hero.MainHero) ? Math.Min(1f, (2f * num2 - (float)num3) / num2) : 1f;
			float num5 = (1.2f - (hero.Age - 18f) * 0.04f) / (float)(num * num) * 0.12f * num4;
			float baseNumber = (hero.Spouse != null && this.IsHeroAgeSuitableForPregnancy(hero)) ? num5 : 0f;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseNumber, false, null);
			if (hero.GetPerkValue(DefaultPerks.Charm.Virile) || hero.Spouse.GetPerkValue(DefaultPerks.Charm.Virile))
			{
				explainedNumber.AddFactor(DefaultPerks.Charm.Virile.PrimaryBonus, DefaultPerks.Charm.Virile.Name);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x04000813 RID: 2067
		private const int MinPregnancyAge = 18;

		// Token: 0x04000814 RID: 2068
		private const int MaxPregnancyAge = 45;
	}
}
