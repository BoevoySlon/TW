using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013A RID: 314
	public class DefaultSiegeAftermathModel : SiegeAftermathModel
	{
		// Token: 0x060017BE RID: 6078 RVA: 0x000761A4 File Offset: 0x000743A4
		public override int GetSiegeAftermathTraitXpChangeForPlayer(TraitObject trait, Settlement devastatedSettlement, SiegeAftermathAction.SiegeAftermath aftermathType)
		{
			int result = 0;
			if (trait == DefaultTraits.Mercy)
			{
				if (aftermathType == SiegeAftermathAction.SiegeAftermath.Devastate)
				{
					if (devastatedSettlement.IsTown)
					{
						result = -50;
					}
					else
					{
						result = -30;
					}
				}
				else if (aftermathType == SiegeAftermathAction.SiegeAftermath.ShowMercy)
				{
					if (devastatedSettlement.IsTown)
					{
						result = 20;
					}
					else
					{
						result = 10;
					}
				}
			}
			return result;
		}
	}
}
