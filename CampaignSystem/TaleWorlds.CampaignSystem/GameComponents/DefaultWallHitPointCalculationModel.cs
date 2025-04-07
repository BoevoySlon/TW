using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014A RID: 330
	public class DefaultWallHitPointCalculationModel : WallHitPointCalculationModel
	{
		// Token: 0x0600184E RID: 6222 RVA: 0x0007C2E5 File Offset: 0x0007A4E5
		public override float CalculateMaximumWallHitPoint(Town town)
		{
			if (town == null)
			{
				return 0f;
			}
			return this.CalculateMaximumWallHitPointInternal(town);
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x0007C2F8 File Offset: 0x0007A4F8
		private float CalculateMaximumWallHitPointInternal(Town town)
		{
			float num = 0f;
			int wallLevel = town.GetWallLevel();
			if (wallLevel == 1)
			{
				num += 30000f;
			}
			else if (wallLevel == 2)
			{
				num += 50000f;
			}
			else if (wallLevel == 3)
			{
				num += 67000f;
			}
			else
			{
				Debug.FailedAssert("Settlement \"" + town.Name + "\" has a wrong wall level set.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameComponents\\DefaultWallHitPointCalculationModel.cs", "CalculateMaximumWallHitPointInternal", 35);
				num += -1f;
			}
			Hero governor = town.Governor;
			if (governor != null && governor.GetPerkValue(DefaultPerks.Engineering.EngineeringGuilds))
			{
				num += num * DefaultPerks.Engineering.EngineeringGuilds.SecondaryBonus;
			}
			return num;
		}
	}
}
