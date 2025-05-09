﻿using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000147 RID: 327
	public class DefaultVillageProductionCalculatorModel : VillageProductionCalculatorModel
	{
		// Token: 0x0600183F RID: 6207 RVA: 0x0007B474 File Offset: 0x00079674
		public override float CalculateDailyProductionAmount(Village village, ItemObject item)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (village.VillageState == Village.VillageStates.Normal)
			{
				foreach (ValueTuple<ItemObject, float> valueTuple in village.VillageType.Productions)
				{
					ItemObject item2 = valueTuple.Item1;
					float num = valueTuple.Item2;
					if (item2 == item)
					{
						if (village.TradeBound != null)
						{
							float num2 = (float)(village.GetHearthLevel() + 1) * 0.5f;
							if (item.IsMountable && item.Tier == ItemObject.ItemTiers.Tier2 && PerkHelper.GetPerkValueForTown(DefaultPerks.Riding.Shepherd, village.TradeBound.Town) && MBRandom.RandomFloat < DefaultPerks.Riding.Shepherd.SecondaryBonus)
							{
								num += 1f;
							}
							explainedNumber.Add(num * num2, null, null);
							if (item.ItemCategory == DefaultItemCategories.Grain || item.ItemCategory == DefaultItemCategories.Olives || item.ItemCategory == DefaultItemCategories.Fish || item.ItemCategory == DefaultItemCategories.DateFruit)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.GranaryAccountant, village.TradeBound.Town, ref explainedNumber);
							}
							else if (item.ItemCategory == DefaultItemCategories.Clay || item.ItemCategory == DefaultItemCategories.Iron || item.ItemCategory == DefaultItemCategories.Cotton || item.ItemCategory == DefaultItemCategories.Silver)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.TradeyardForeman, village.TradeBound.Town, ref explainedNumber);
							}
							if (item.IsTradeGood)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Athletics.Steady, village.TradeBound.Town, ref explainedNumber);
							}
							if (item.IsAnimal)
							{
								PerkHelper.AddPerkBonusForTown(DefaultPerks.Medicine.PerfectHealth, village.TradeBound.Town, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusForTown(DefaultPerks.Riding.Breeder, village.TradeBound.Town, ref explainedNumber);
						}
						if (village.Settlement.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.KhuzaitAnimalProductionFeat) && (item.ItemCategory == DefaultItemCategories.Sheep || item.ItemCategory == DefaultItemCategories.Cow || item.ItemCategory == DefaultItemCategories.WarHorse || item.ItemCategory == DefaultItemCategories.Horse || item.ItemCategory == DefaultItemCategories.PackAnimal))
						{
							explainedNumber.AddFactor(DefaultCulturalFeats.KhuzaitAnimalProductionFeat.EffectBonus, GameTexts.FindText("str_culture", null));
						}
						if (village.Bound.IsCastle && village.Settlement.OwnerClan.Culture.HasFeat(DefaultCulturalFeats.VlandianCastleVillageProductionFeat))
						{
							explainedNumber.AddFactor(DefaultCulturalFeats.VlandianCastleVillageProductionFeat.EffectBonus, GameTexts.FindText("str_culture", null));
						}
					}
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0007B720 File Offset: 0x00079920
		public override float CalculateDailyFoodProductionAmount(Village village)
		{
			if (village.VillageState != Village.VillageStates.Normal)
			{
				return 0f;
			}
			float num = (float)(village.GetHearthLevel() + 1);
			float num2;
			if (this.GetIssueEffectOnFoodProduction(village.Settlement, out num2))
			{
				num *= num2;
			}
			return num;
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0007B75C File Offset: 0x0007995C
		private bool GetIssueEffectOnFoodProduction(Settlement settlement, out float issueEffect)
		{
			issueEffect = 1f;
			if (settlement.IsVillage)
			{
				foreach (Hero hero in SettlementHelper.GetAllHeroesOfSettlement(settlement, false))
				{
					if (hero.Issue != null && hero.MapFaction == settlement.MapFaction)
					{
						float activeIssueEffectAmount = hero.Issue.GetActiveIssueEffectAmount(DefaultIssueEffects.HalfVillageProduction);
						if (activeIssueEffectAmount != 0f)
						{
							issueEffect *= activeIssueEffectAmount;
						}
					}
				}
			}
			return !issueEffect.ApproximatelyEqualsTo(1f, 1E-05f);
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0007B7FC File Offset: 0x000799FC
		public override float CalculateProductionSpeedOfItemCategory(ItemCategory item)
		{
			float num = 0f;
			foreach (VillageType villageType in VillageType.All)
			{
				float productionPerDay = villageType.GetProductionPerDay(item);
				if (productionPerDay > num)
				{
					num = productionPerDay;
				}
			}
			return num;
		}
	}
}
