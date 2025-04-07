using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003C9 RID: 969
	public class PrisonerCaptureCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B8E RID: 15246 RVA: 0x0011B618 File Offset: 0x00119818
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x0011B66A File Offset: 0x0011986A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0011B66C File Offset: 0x0011986C
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification)
		{
			foreach (Settlement settlement in from x in clan.Settlements
			where x.IsFortification
			select x)
			{
				this.HandleSettlementHeroes(settlement);
			}
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0011B6E0 File Offset: 0x001198E0
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail detail)
		{
			foreach (Settlement settlement in from x in faction1.Settlements
			where x.IsFortification
			select x)
			{
				this.HandleSettlementHeroes(settlement);
			}
			foreach (Settlement settlement2 in from x in faction2.Settlements
			where x.IsFortification
			select x)
			{
				this.HandleSettlementHeroes(settlement2);
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0011B7B4 File Offset: 0x001199B4
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			if (settlement.IsFortification)
			{
				this.HandleSettlementHeroes(settlement);
			}
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0011B7C8 File Offset: 0x001199C8
		private void HandleSettlementHeroes(Settlement settlement)
		{
			foreach (Hero hero in settlement.HeroesWithoutParty.Where(new Func<Hero, bool>(this.SettlementHeroCaptureCommonCondition)).ToList<Hero>())
			{
				TakePrisonerAction.Apply(hero.CurrentSettlement.Party, hero);
			}
			foreach (MobileParty mobileParty in (from x in settlement.Parties
			where x.IsLordParty && (x.Army == null || (x.Army != null && x.Army.LeaderParty == x && !x.Army.Parties.Contains(MobileParty.MainParty))) && x.MapEvent == null && this.SettlementHeroCaptureCommonCondition(x.LeaderHero)
			select x).ToList<MobileParty>())
			{
				LeaveSettlementAction.ApplyForParty(mobileParty);
				SetPartyAiAction.GetActionForPatrollingAroundSettlement(mobileParty, settlement);
			}
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0011B898 File Offset: 0x00119A98
		private bool SettlementHeroCaptureCommonCondition(Hero hero)
		{
			return hero != null && hero != Hero.MainHero && !hero.IsWanderer && !hero.IsNotable && hero.HeroState != Hero.CharacterStates.Prisoner && hero.HeroState != Hero.CharacterStates.Dead && hero.MapFaction != null && hero.CurrentSettlement != null && hero.MapFaction.IsAtWarWith(hero.CurrentSettlement.MapFaction);
		}
	}
}
