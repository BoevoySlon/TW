using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003C6 RID: 966
	public class PoliticalStagnationAndBorderIncidentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B55 RID: 15189 RVA: 0x0011A1B9 File Offset: 0x001183B9
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x0011A1E9 File Offset: 0x001183E9
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x0011A1EC File Offset: 0x001183EC
		public void HourlyTickSettlement(Settlement settlement)
		{
			if (settlement.IsFortification || settlement.IsVillage)
			{
				LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(settlement.Position2D, 10f);
				for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
				{
					if (!mobileParty.IsGarrison && !mobileParty.IsMilitia && mobileParty.Aggressiveness > 0f)
					{
						if (mobileParty.MapFaction == settlement.MapFaction && (mobileParty.IsCaravan || mobileParty.IsVillager) && mobileParty.Ai.IsAlerted)
						{
							settlement.NumberOfEnemiesSpottedAround += 0.2f;
						}
						if (mobileParty.CurrentSettlement == null && FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, settlement.MapFaction))
						{
							float num = mobileParty.Party.TotalStrength;
							if (mobileParty == MobileParty.MainParty)
							{
								num *= 2f;
								num += 50f;
							}
							float num2 = MathF.Min(1f, num / 500f * MathF.Min(1f, mobileParty.Aggressiveness));
							if (!mobileParty.IsLordParty)
							{
								num2 *= 0.5f;
							}
							if (mobileParty.MapEvent != null && mobileParty.MapEvent.IsFieldBattle)
							{
								num2 = 3f * num2;
							}
							settlement.NumberOfEnemiesSpottedAround += num2;
						}
						else if (mobileParty.MapFaction == settlement.MapFaction)
						{
							float num3 = MathF.Min(1f, mobileParty.Party.TotalStrength / 500f * MathF.Min(1f, mobileParty.Aggressiveness));
							settlement.NumberOfAlliesSpottedAround += num3;
						}
					}
				}
				settlement.NumberOfEnemiesSpottedAround *= 0.95f;
				settlement.NumberOfAlliesSpottedAround *= 0.8f;
			}
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0011A3B0 File Offset: 0x001185B0
		public void DailyTick()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				PoliticalStagnationAndBorderIncidentCampaignBehavior.UpdatePoliticallyStagnation(kingdom);
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0011A400 File Offset: 0x00118600
		private static void UpdatePoliticallyStagnation(Kingdom kingdom)
		{
			float num = 1f + (float)MathF.Min(60, kingdom.Fiefs.Count) * 0.2f;
			float num2 = 2f + (float)MathF.Min(60, kingdom.Fiefs.Count) * 0.6f;
			int num3 = 1;
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (FactionManager.IsAtWarAgainstFaction(kingdom, kingdom2))
				{
					if ((float)kingdom2.Fiefs.Count >= num2)
					{
						num3 = -2;
						break;
					}
					if ((float)kingdom2.Fiefs.Count >= num)
					{
						num3 = -1;
					}
				}
			}
			kingdom.PoliticalStagnation += num3;
			if (kingdom.PoliticalStagnation < 0)
			{
				kingdom.PoliticalStagnation = 0;
				return;
			}
			if (kingdom.PoliticalStagnation > 300)
			{
				kingdom.PoliticalStagnation = 300;
			}
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0011A4F8 File Offset: 0x001186F8
		private void BorderIncidents()
		{
		}
	}
}
