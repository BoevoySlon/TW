using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000018 RID: 24
	public static class BannerHelper
	{
		// Token: 0x060000DF RID: 223 RVA: 0x0000BA44 File Offset: 0x00009C44
		public static ItemObject GetRandomBannerItemForHero(Hero hero)
		{
			return Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItemsForHero(hero).GetRandomElementInefficiently<ItemObject>();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000BA60 File Offset: 0x00009C60
		public static void AddBannerBonusForBanner(BannerEffect bannerEffect, BannerComponent bannerComponent, ref ExplainedNumber bonuses)
		{
			if (bannerComponent != null && bannerComponent.BannerEffect == bannerEffect)
			{
				BannerHelper.AddBannerEffectToStat(ref bonuses, bannerEffect.IncrementType, bannerComponent.GetBannerEffectBonus(), bannerEffect.Name);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000BA86 File Offset: 0x00009C86
		private static void AddBannerEffectToStat(ref ExplainedNumber stat, BannerEffect.EffectIncrementType effectIncrementType, float number, TextObject effectName)
		{
			if (effectIncrementType == BannerEffect.EffectIncrementType.Add)
			{
				stat.Add(number, effectName, null);
				return;
			}
			if (effectIncrementType == BannerEffect.EffectIncrementType.AddFactor)
			{
				stat.AddFactor(number, effectName);
			}
		}
	}
}
