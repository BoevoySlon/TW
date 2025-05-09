﻿using System;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E2 RID: 994
	public class VillageHealCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003DB7 RID: 15799 RVA: 0x0012DB70 File Offset: 0x0012BD70
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x0012DB8C File Offset: 0x0012BD8C
		private void DailyTickSettlement(Settlement settlement)
		{
			if ((settlement.IsVillage || settlement.IsTown) && settlement.SettlementHitPoints < 1f && settlement.Party.MapEvent == null && settlement.Party.SiegeEvent == null)
			{
				float num = (7000f - MathF.Min(7000f, MathF.Max(1000f, settlement.MapFaction.TotalStrength))) / 100000f;
				ExplainedNumber explainedNumber = new ExplainedNumber(0.06f + num, false, null);
				if (settlement.IsVillage && settlement.Village.TradeBound != null && PerkHelper.GetPerkValueForTown(DefaultPerks.Medicine.CleanInfrastructure, settlement.Village.TradeBound.Town))
				{
					explainedNumber.AddFactor(DefaultPerks.Medicine.CleanInfrastructure.SecondaryBonus, DefaultPerks.Medicine.CleanInfrastructure.Name);
				}
				if (settlement.OwnerClan.Leader.GetPerkValue(DefaultPerks.Roguery.InBestLight))
				{
					explainedNumber.AddFactor(DefaultPerks.Roguery.InBestLight.SecondaryBonus, DefaultPerks.Roguery.InBestLight.Name);
				}
				IncreaseSettlementHealthAction.Apply(settlement, explainedNumber.ResultNumber);
			}
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x0012DCA3 File Offset: 0x0012BEA3
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
