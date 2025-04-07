using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003D2 RID: 978
	public class SallyOutsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003C64 RID: 15460 RVA: 0x001240DD File Offset: 0x001222DD
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x0012410D File Offset: 0x0012230D
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (defenderParty.SiegeEvent != null)
			{
				this.CheckForSettlementSallyOut(defenderParty.SiegeEvent.BesiegedSettlement, false);
			}
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x00124129 File Offset: 0x00122329
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x0012412B File Offset: 0x0012232B
		public void HourlyTickSettlement(Settlement settlement)
		{
			this.CheckForSettlementSallyOut(settlement, false);
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x00124138 File Offset: 0x00122338
		private void CheckForSettlementSallyOut(Settlement settlement, bool forceForCheck = false)
		{
			if (settlement.IsFortification && settlement.SiegeEvent != null && settlement.Party.MapEvent == null && settlement.Town.GarrisonParty != null && settlement.Town.GarrisonParty.MapEvent == null)
			{
				bool flag = settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null && settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.IsSiegeOutside;
				if ((flag || MathF.Floor(CampaignTime.Now.ToHours) % 4 == 0) && (Hero.MainHero.CurrentSettlement != settlement || Campaign.Current.Models.EncounterModel.GetLeaderOfSiegeEvent(settlement.SiegeEvent, BattleSideEnum.Defender) != Hero.MainHero))
				{
					MobileParty leaderParty = settlement.SiegeEvent.BesiegerCamp.LeaderParty;
					float num = 0f;
					float num2 = 0f;
					float num3 = settlement.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.SallyOut).Sum((PartyBase x) => x.TotalStrength);
					LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(settlement.SiegeEvent.BesiegerCamp.LeaderParty.Position2D, 3f);
					for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
					{
						if (mobileParty.CurrentSettlement == null && mobileParty.Aggressiveness > 0f)
						{
							float num4 = (mobileParty.Aggressiveness > 0.5f) ? 1f : (mobileParty.Aggressiveness * 2f);
							if (mobileParty.MapFaction.IsAtWarWith(settlement.Party.MapFaction))
							{
								num += num4 * mobileParty.Party.TotalStrength;
							}
							else if (mobileParty.MapFaction == settlement.MapFaction)
							{
								num2 += num4 * mobileParty.Party.TotalStrength;
							}
						}
					}
					float num5 = num3 + num2;
					float num6 = flag ? 1.5f : 2f;
					if (num5 > num * num6)
					{
						if (flag)
						{
							using (IEnumerator<PartyBase> enumerator = settlement.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.SallyOut).GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									PartyBase partyBase = enumerator.Current;
									if (partyBase.IsMobile && !partyBase.MobileParty.IsMainParty && partyBase.MapEventSide == null)
									{
										partyBase.MapEventSide = settlement.SiegeEvent.BesiegerCamp.LeaderParty.MapEvent.AttackerSide;
									}
								}
								return;
							}
						}
						EncounterManager.StartPartyEncounter(settlement.Town.GarrisonParty.Party, leaderParty.Party);
					}
				}
			}
		}

		// Token: 0x04001202 RID: 4610
		private const int SallyOutCheckPeriodInHours = 4;

		// Token: 0x04001203 RID: 4611
		private const float SallyOutPowerRatioForHelpingReliefForce = 1.5f;

		// Token: 0x04001204 RID: 4612
		private const float SallyOutPowerRatio = 2f;
	}
}
