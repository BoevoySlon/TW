using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003B9 RID: 953
	public class PartiesSellPrisonerCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A35 RID: 14901 RVA: 0x00112256 File Offset: 0x00110456
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x00112286 File Offset: 0x00110486
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x00112288 File Offset: 0x00110488
		private void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (settlement.IsFortification && mobileParty != null && mobileParty.PrisonRoster.Count > 0 && mobileParty.MapFaction != null && !mobileParty.IsMainParty && !mobileParty.IsDisbanding && !mobileParty.MapFaction.IsAtWarWith(settlement.MapFaction))
			{
				if (mobileParty.MapFaction.IsKingdomFaction && mobileParty.ActualClan != null)
				{
					TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
					foreach (TroopRosterElement troopRosterElement in mobileParty.PrisonRoster.GetTroopRoster())
					{
						if (!troopRosterElement.Character.IsHero || troopRosterElement.Character.HeroObject.MapFaction.IsAtWarWith(settlement.MapFaction))
						{
							troopRoster.Add(troopRosterElement);
						}
					}
					if (troopRoster.Count > 0)
					{
						SellPrisonersAction.ApplyForSelectedPrisoners(mobileParty.Party, settlement.Party, troopRoster);
						return;
					}
				}
				else
				{
					SellPrisonersAction.ApplyForAllPrisoners(mobileParty.Party, settlement.Party);
				}
			}
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x001123B4 File Offset: 0x001105B4
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.IsFortification)
			{
				TroopRoster prisonRoster = settlement.Party.PrisonRoster;
				if (prisonRoster.TotalRegulars > 0)
				{
					int num = (settlement.Owner == Hero.MainHero) ? (prisonRoster.TotalManCount - settlement.Party.PrisonerSizeLimit) : MBRandom.RoundRandomized((float)prisonRoster.TotalRegulars * 0.1f);
					if (num > 0)
					{
						TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
						IEnumerable<TroopRosterElement> enumerable;
						if (settlement.Owner != Hero.MainHero)
						{
							enumerable = prisonRoster.GetTroopRoster().AsEnumerable<TroopRosterElement>();
						}
						else
						{
							IEnumerable<TroopRosterElement> enumerable2 = from t in prisonRoster.GetTroopRoster()
							orderby t.Character.Tier
							select t;
							enumerable = enumerable2;
						}
						foreach (TroopRosterElement troopRosterElement in enumerable)
						{
							if (!troopRosterElement.Character.IsHero)
							{
								int num2 = Math.Min(num, troopRosterElement.Number);
								num -= num2;
								troopRoster.AddToCounts(troopRosterElement.Character, num2, false, 0, 0, true, -1);
								if (num <= 0)
								{
									break;
								}
							}
						}
						if (troopRoster.TotalManCount > 0)
						{
							SellPrisonersAction.ApplyForSelectedPrisoners(settlement.Party, null, troopRoster);
						}
					}
				}
			}
		}
	}
}
