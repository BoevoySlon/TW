using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FE RID: 254
	public class DefaultDailyTroopXpBonusModel : DailyTroopXpBonusModel
	{
		// Token: 0x0600154C RID: 5452 RVA: 0x00062953 File Offset: 0x00060B53
		public override int CalculateDailyTroopXpBonus(Town town)
		{
			return this.CalculateTroopXpBonusInternal(town);
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0006295C File Offset: 0x00060B5C
		private int CalculateTroopXpBonusInternal(Town town)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			foreach (Building building in town.Buildings)
			{
				float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.Experience);
				if (buildingEffectAmount > 0f)
				{
					explainedNumber.Add(buildingEffectAmount, building.Name, null);
				}
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Leadership.RaiseTheMeek, town, ref explainedNumber);
			PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.ProjectileDeflection, town, ref explainedNumber);
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x000629F8 File Offset: 0x00060BF8
		public override float CalculateGarrisonXpBonusMultiplier(Town town)
		{
			return 1f;
		}
	}
}
