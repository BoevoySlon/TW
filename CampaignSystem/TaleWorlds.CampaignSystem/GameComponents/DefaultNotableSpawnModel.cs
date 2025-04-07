using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011B RID: 283
	public class DefaultNotableSpawnModel : NotableSpawnModel
	{
		// Token: 0x06001685 RID: 5765 RVA: 0x0006D0C8 File Offset: 0x0006B2C8
		public override int GetTargetNotableCountForSettlement(Settlement settlement, Occupation occupation)
		{
			int result = 0;
			if (settlement.IsTown)
			{
				if (occupation == Occupation.Merchant)
				{
					result = 2;
				}
				else if (occupation == Occupation.GangLeader)
				{
					result = 2;
				}
				else if (occupation == Occupation.Artisan)
				{
					result = 1;
				}
				else
				{
					result = 0;
				}
			}
			else if (settlement.IsVillage)
			{
				if (occupation == Occupation.Headman)
				{
					result = 1;
				}
				else if (occupation == Occupation.RuralNotable)
				{
					result = 2;
				}
			}
			return result;
		}
	}
}
