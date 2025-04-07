using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010B RID: 267
	public class DefaultHeroDeathProbabilityCalculationModel : HeroDeathProbabilityCalculationModel
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x000684F4 File Offset: 0x000666F4
		public override float CalculateHeroDeathProbability(Hero hero)
		{
			return this.CalculateHeroDeathProbabilityInternal(hero);
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x00068500 File Offset: 0x00066700
		private float CalculateHeroDeathProbabilityInternal(Hero hero)
		{
			float num = 0f;
			if (!CampaignOptions.IsLifeDeathCycleDisabled)
			{
				int becomeOldAge = Campaign.Current.Models.AgeModel.BecomeOldAge;
				int num2 = Campaign.Current.Models.AgeModel.MaxAge - 1;
				if (hero.Age > (float)becomeOldAge)
				{
					if (hero.Age < (float)num2)
					{
						float num3 = 0.3f * ((hero.Age - (float)becomeOldAge) / (float)(Campaign.Current.Models.AgeModel.MaxAge - becomeOldAge));
						float num4 = 1f - MathF.Pow(1f - num3, 0.011904762f);
						num += num4;
					}
					else if (hero.Age >= (float)num2)
					{
						num += 1f;
					}
				}
			}
			return num;
		}
	}
}
