using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003C4 RID: 964
	public class PlayerTrackCompanionBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B44 RID: 15172 RVA: 0x00119DE0 File Offset: 0x00117FE0
		public override void RegisterEvents()
		{
			CampaignEvents.CharacterBecameFugitive.AddNonSerializedListener(this, new Action<Hero>(this.HeroBecameFugitive));
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.CompanionRemoved));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.SettlementEntered));
			CampaignEvents.NewCompanionAdded.AddNonSerializedListener(this, new Action<Hero>(this.CompanionAdded));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail>(this.OnHeroPrisonerReleased));
			CampaignEvents.CanBeGovernorOrHavePartyRoleEvent.AddNonSerializedListener(this, new ReferenceAction<Hero, bool>(this.CanBeGovernorOrHavePartyRole));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x00119E90 File Offset: 0x00118090
		private void OnGameLoadFinished()
		{
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.2.9.35637", 66233)))
			{
				foreach (Hero hero in this.ScatteredCompanions.Keys.ToList<Hero>())
				{
					if (hero.PartyBelongedTo != null || hero.GovernorOf != null || Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
					{
						this.ScatteredCompanions.Remove(hero);
					}
				}
			}
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x00119F44 File Offset: 0x00118144
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, CampaignTime>>("ScatteredCompanions", ref this.ScatteredCompanions);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x00119F58 File Offset: 0x00118158
		private void CanBeGovernorOrHavePartyRole(Hero hero, ref bool canBeGovernorOrHavePartyRole)
		{
			if (this.ScatteredCompanions.ContainsKey(hero))
			{
				canBeGovernorOrHavePartyRole = false;
			}
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x00119F6B File Offset: 0x0011816B
		private void AddHeroToScatteredCompanions(Hero hero)
		{
			if (hero.IsPlayerCompanion)
			{
				if (!this.ScatteredCompanions.ContainsKey(hero))
				{
					this.ScatteredCompanions.Add(hero, CampaignTime.Now);
					return;
				}
				this.ScatteredCompanions[hero] = CampaignTime.Now;
			}
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x00119FA6 File Offset: 0x001181A6
		private void HeroBecameFugitive(Hero hero)
		{
			this.AddHeroToScatteredCompanions(hero);
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x00119FAF File Offset: 0x001181AF
		private void OnHeroPrisonerReleased(Hero releasedHero, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail)
		{
			this.AddHeroToScatteredCompanions(releasedHero);
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x00119FB8 File Offset: 0x001181B8
		private void SettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			if (party == MobileParty.MainParty)
			{
				foreach (Hero hero2 in settlement.HeroesWithoutParty)
				{
					if (this.ScatteredCompanions.ContainsKey(hero2))
					{
						TextObject textObject = new TextObject("{=ahpSGaow}You hear that your companion {NOTABLE.LINK}, who was separated from you after a battle, is currently in this settlement.", null);
						StringHelpers.SetCharacterProperties("NOTABLE", hero2.CharacterObject, textObject, false);
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=dx0hmeH6}Tracking", null).ToString(), textObject.ToString(), true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
						this.ScatteredCompanions.Remove(hero2);
					}
				}
			}
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x0011A098 File Offset: 0x00118298
		private void CompanionAdded(Hero companion)
		{
			if (this.ScatteredCompanions.ContainsKey(companion))
			{
				this.ScatteredCompanions.Remove(companion);
			}
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x0011A0B5 File Offset: 0x001182B5
		private void CompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			if (this.ScatteredCompanions.ContainsKey(companion))
			{
				this.ScatteredCompanions.Remove(companion);
			}
		}

		// Token: 0x040011CE RID: 4558
		private Dictionary<Hero, CampaignTime> ScatteredCompanions = new Dictionary<Hero, CampaignTime>();
	}
}
