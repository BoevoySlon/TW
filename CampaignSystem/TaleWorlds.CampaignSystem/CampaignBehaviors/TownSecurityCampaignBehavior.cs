using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003DB RID: 987
	public class TownSecurityCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003D27 RID: 15655 RVA: 0x0012A840 File Offset: 0x00128A40
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.MapEventEnded));
			CampaignEvents.OnSiegeEventEndedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.SiegeEventEnded));
			CampaignEvents.OnHideoutDeactivatedEvent.AddNonSerializedListener(this, new Action<Settlement>(this.OnHideoutDeactivated));
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x0012A894 File Offset: 0x00128A94
		private void OnHideoutDeactivated(Settlement hideout)
		{
			SettlementSecurityModel model = Campaign.Current.Models.SettlementSecurityModel;
			foreach (Settlement settlement in (from t in Settlement.All
			where t.IsTown && t.GatePosition.DistanceSquared(hideout.GatePosition) < model.HideoutClearedSecurityEffectRadius * model.HideoutClearedSecurityEffectRadius
			select t).ToList<Settlement>())
			{
				settlement.Town.Security += (float)model.HideoutClearedSecurityGain;
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x0012A934 File Offset: 0x00128B34
		private void MapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsFieldBattle && mapEvent.HasWinner)
			{
				SettlementSecurityModel model = Campaign.Current.Models.SettlementSecurityModel;
				using (List<Settlement>.Enumerator enumerator = (from t in Settlement.All
				where t.IsTown && t.GatePosition.DistanceSquared(mapEvent.Position) < model.MapEventSecurityEffectRadius * model.MapEventSecurityEffectRadius
				select t).ToList<Settlement>().GetEnumerator())
				{
					Func<PartyBase, bool> <>9__3;
					while (enumerator.MoveNext())
					{
						Settlement town = enumerator.Current;
						if (mapEvent.Winner.Parties.Any((MapEventParty party) => party.Party.IsMobile && party.Party.MobileParty.IsBandit) && mapEvent.InvolvedParties.Any((PartyBase party) => this.ValidCivilianPartyCondition(party, mapEvent, town.MapFaction)))
						{
							float sumOfAttackedPartyStrengths = mapEvent.StrengthOfSide[(int)mapEvent.DefeatedSide];
							town.Town.Security += model.GetLootedNearbyPartySecurityEffect(town.Town, sumOfAttackedPartyStrengths);
						}
						else
						{
							IEnumerable<PartyBase> involvedParties = mapEvent.InvolvedParties;
							Func<PartyBase, bool> predicate;
							if ((predicate = <>9__3) == null)
							{
								predicate = (<>9__3 = ((PartyBase party) => this.ValidBanditPartyCondition(party, mapEvent)));
							}
							if (involvedParties.Any(predicate))
							{
								float sumOfAttackedPartyStrengths2 = mapEvent.StrengthOfSide[(int)mapEvent.DefeatedSide];
								town.Town.Security += model.GetNearbyBanditPartyDefeatedSecurityEffect(town.Town, sumOfAttackedPartyStrengths2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x0012AB9C File Offset: 0x00128D9C
		private bool ValidCivilianPartyCondition(PartyBase party, MapEvent mapEvent, IFaction mapFaction)
		{
			return party.IsMobile && ((party.Side != mapEvent.WinningSide && party.MobileParty.IsVillager && FactionManager.IsAlliedWithFaction(party.MapFaction, mapFaction)) || (party.MobileParty.IsCaravan && !party.MapFaction.IsAtWarWith(mapFaction)));
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x0012ABFC File Offset: 0x00128DFC
		private bool ValidBanditPartyCondition(PartyBase party, MapEvent mapEvent)
		{
			if (party.Side != mapEvent.WinningSide)
			{
				MobileParty mobileParty = party.MobileParty;
				return mobileParty != null && mobileParty.IsBandit;
			}
			return false;
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x0012AC1F File Offset: 0x00128E1F
		private void SiegeEventEnded(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x0012AC21 File Offset: 0x00128E21
		public override void SyncData(IDataStore dataStore)
		{
		}
	}
}
