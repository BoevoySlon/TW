using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003AE RID: 942
	public class MarriageOfferCampaignBehavior : CampaignBehaviorBase, IMarriageOfferCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x060039D3 RID: 14803 RVA: 0x0010F6D5 File Offset: 0x0010D8D5
		internal bool IsThereActiveMarriageOffer
		{
			get
			{
				return this._currentOfferedPlayerClanHero != null && this._currentOfferedOtherClanHero != null;
			}
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x0010F6EC File Offset: 0x0010D8EC
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.DailyTickClan));
			CampaignEvents.OnMarriageOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnMarriageOfferedToPlayer));
			CampaignEvents.OnMarriageOfferCanceledEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnMarriageOfferCanceled));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.HeroesMarried.AddNonSerializedListener(this, new Action<Hero, Hero, bool>(this.OnHeroesMarried));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnArmyCreated));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			CampaignEvents.CharacterBecameFugitive.AddNonSerializedListener(this, new Action<Hero>(this.CharacterBecameFugitive));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.HeroRelationChanged.AddNonSerializedListener(this, new Action<Hero, Hero, int, bool, ChangeRelationAction.ChangeRelationDetail, Hero, Hero>(this.OnHeroRelationChanged));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangedKingdom));
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x0010F826 File Offset: 0x0010DA26
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Hero>("_currentOfferedPlayerClanHero", ref this._currentOfferedPlayerClanHero);
			dataStore.SyncData<Hero>("_currentOfferedOtherClanHero", ref this._currentOfferedOtherClanHero);
			dataStore.SyncData<CampaignTime>("_lastMarriageOfferTime", ref this._lastMarriageOfferTime);
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x0010F860 File Offset: 0x0010DA60
		public void CreateMarriageOffer(Hero currentOfferedPlayerClanHero, Hero currentOfferedOtherClanHero)
		{
			this._currentOfferedPlayerClanHero = currentOfferedPlayerClanHero;
			this._currentOfferedOtherClanHero = currentOfferedOtherClanHero;
			this._lastMarriageOfferTime = CampaignTime.Now;
			this.MarriageOfferPanelExplanationText.SetCharacterProperties("CLAN_MEMBER", this._currentOfferedPlayerClanHero.CharacterObject, false);
			this.MarriageOfferPanelExplanationText.SetTextVariable("OFFERING_CLAN_NAME", this._currentOfferedOtherClanHero.Clan.Name);
			Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new MarriageOfferMapNotification(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero, this.MarriageOfferPanelExplanationText));
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x0010F8EC File Offset: 0x0010DAEC
		public MBBindingList<TextObject> GetMarriageAcceptedConsequences()
		{
			MBBindingList<TextObject> mbbindingList = new MBBindingList<TextObject>();
			TextObject textObject = GameTexts.FindText("str_marriage_consequence_hero_join_clan", null);
			if (Campaign.Current.Models.MarriageModel.GetClanAfterMarriage(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero) == this._currentOfferedPlayerClanHero.Clan)
			{
				textObject.SetCharacterProperties("HERO", this._currentOfferedOtherClanHero.CharacterObject, false);
				textObject.SetTextVariable("CLAN_NAME", this._currentOfferedPlayerClanHero.Clan.Name);
			}
			else
			{
				textObject.SetCharacterProperties("HERO", this._currentOfferedPlayerClanHero.CharacterObject, false);
				textObject.SetTextVariable("CLAN_NAME", this._currentOfferedOtherClanHero.Clan.Name);
			}
			mbbindingList.Add(textObject);
			TextObject textObject2 = GameTexts.FindText("str_marriage_consequence_clan_relation", null);
			textObject2.SetTextVariable("CLAN_NAME", this._currentOfferedOtherClanHero.Clan.Name);
			textObject2.SetTextVariable("AMOUNT", 10.ToString("+0;-#"));
			mbbindingList.Add(textObject2);
			return mbbindingList;
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x0010F9F0 File Offset: 0x0010DBF0
		public void OnMarriageOfferAcceptedOnPopUp()
		{
			if (this._currentOfferedPlayerClanHero != Hero.MainHero)
			{
				Hero groomHero = this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero;
				Hero brideHero = this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero;
				MBInformationManager.ShowSceneNotification(new MarriageSceneNotificationItem(groomHero, brideHero, CampaignTime.Now, SceneNotificationData.RelevantContextType.Any));
			}
			ChangeRelationAction.ApplyPlayerRelation(this._currentOfferedOtherClanHero.Clan.Leader, 10, true, true);
			MarriageAction.Apply(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero, true);
			this.FinalizeMarriageOffer();
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x0010FA84 File Offset: 0x0010DC84
		public void OnMarriageOfferDeclinedOnPopUp()
		{
			CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x0010FAD1 File Offset: 0x0010DCD1
		public void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x0010FAD3 File Offset: 0x0010DCD3
		public void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
			this.FinalizeMarriageOffer();
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x0010FADC File Offset: 0x0010DCDC
		private void DailyTickClan(Clan consideringClan)
		{
			if (this.CanOfferMarriageForClan(consideringClan))
			{
				float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(Clan.PlayerClan.FactionMidSettlement, consideringClan.FactionMidSettlement);
				if (MBRandom.RandomFloat >= distance / Campaign.MaximumDistanceBetweenTwoSettlements - 0.5f)
				{
					foreach (Hero hero in Clan.PlayerClan.Heroes)
					{
						if (hero != Hero.MainHero && hero.CanMarry() && this.ConsiderMarriageForPlayerClanMember(hero, consideringClan))
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x0010FB90 File Offset: 0x0010DD90
		private void HourlyTick()
		{
			if (this.IsThereActiveMarriageOffer && this._lastMarriageOfferTime.ElapsedHoursUntilNow >= 48f)
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x0010FBF8 File Offset: 0x0010DDF8
		private void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			if (this.IsThereActiveMarriageOffer && (prisoner == Hero.MainHero || prisoner == this._currentOfferedPlayerClanHero || prisoner == this._currentOfferedOtherClanHero))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x0010FC68 File Offset: 0x0010DE68
		private void OnHeroesMarried(Hero hero1, Hero hero2, bool showNotification = true)
		{
			if (this.IsThereActiveMarriageOffer && ((hero1 == this._currentOfferedPlayerClanHero && hero2 == this._currentOfferedOtherClanHero) || (hero1 == this._currentOfferedOtherClanHero && hero2 == this._currentOfferedPlayerClanHero)))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x0010FCE4 File Offset: 0x0010DEE4
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			if (this.IsThereActiveMarriageOffer && (victim == Hero.MainHero || victim == this._currentOfferedPlayerClanHero || victim == this._currentOfferedOtherClanHero))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x0010FD54 File Offset: 0x0010DF54
		private void OnArmyCreated(Army army)
		{
			if (this.IsThereActiveMarriageOffer)
			{
				MobileParty partyBelongedTo = this._currentOfferedPlayerClanHero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.Army : null) == null)
				{
					MobileParty partyBelongedTo2 = this._currentOfferedOtherClanHero.PartyBelongedTo;
					if (((partyBelongedTo2 != null) ? partyBelongedTo2.Army : null) == null)
					{
						return;
					}
				}
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x0010FDDC File Offset: 0x0010DFDC
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (this.IsThereActiveMarriageOffer)
			{
				MobileParty partyBelongedTo = this._currentOfferedPlayerClanHero.PartyBelongedTo;
				if (((partyBelongedTo != null) ? partyBelongedTo.MapEvent : null) == null)
				{
					MobileParty partyBelongedTo2 = this._currentOfferedOtherClanHero.PartyBelongedTo;
					if (((partyBelongedTo2 != null) ? partyBelongedTo2.MapEvent : null) == null)
					{
						return;
					}
				}
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x0010FE64 File Offset: 0x0010E064
		private void CharacterBecameFugitive(Hero hero)
		{
			if (this.IsThereActiveMarriageOffer && (!this._currentOfferedPlayerClanHero.IsActive || !this._currentOfferedOtherClanHero.IsActive))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0010FED4 File Offset: 0x0010E0D4
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			if (this.IsThereActiveMarriageOffer && (!Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(this._currentOfferedPlayerClanHero, this._currentOfferedOtherClanHero) || !Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(Clan.PlayerClan, this._currentOfferedOtherClanHero.Clan)))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x0010FF74 File Offset: 0x0010E174
		private void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			if (this.IsThereActiveMarriageOffer && (effectiveHero.Clan == this._currentOfferedPlayerClanHero.Clan || effectiveHero.Clan == this._currentOfferedOtherClanHero.Clan) && (effectiveHeroGainedRelationWith.Clan == this._currentOfferedPlayerClanHero.Clan || effectiveHeroGainedRelationWith.Clan == this._currentOfferedOtherClanHero.Clan) && !Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(this._currentOfferedPlayerClanHero.Clan, this._currentOfferedOtherClanHero.Clan))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x00110048 File Offset: 0x0010E248
		private void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail detail, bool showNotification = true)
		{
			if (this.IsThereActiveMarriageOffer && (this._currentOfferedPlayerClanHero.Clan == clan || this._currentOfferedOtherClanHero.Clan == clan) && !Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(this._currentOfferedPlayerClanHero.Clan, this._currentOfferedOtherClanHero.Clan))
			{
				CampaignEventDispatcher.Instance.OnMarriageOfferCanceled(this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedOtherClanHero : this._currentOfferedPlayerClanHero, this._currentOfferedPlayerClanHero.IsFemale ? this._currentOfferedPlayerClanHero : this._currentOfferedOtherClanHero);
			}
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x001100E8 File Offset: 0x0010E2E8
		private bool CanOfferMarriageForClan(Clan consideringClan)
		{
			return !this.IsThereActiveMarriageOffer && this._lastMarriageOfferTime.ElapsedWeeksUntilNow >= 1f && !Hero.MainHero.IsPrisoner && consideringClan != Clan.PlayerClan && Campaign.Current.Models.MarriageModel.IsClanSuitableForMarriage(consideringClan) && Campaign.Current.Models.MarriageModel.ShouldNpcMarriageBetweenClansBeAllowed(Clan.PlayerClan, consideringClan);
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x00110158 File Offset: 0x0010E358
		private bool ConsiderMarriageForPlayerClanMember(Hero playerClanHero, Clan consideringClan)
		{
			MarriageModel marriageModel = Campaign.Current.Models.MarriageModel;
			foreach (Hero hero in consideringClan.Heroes)
			{
				float num = marriageModel.NpcCoupleMarriageChance(playerClanHero, hero);
				if (num > 0f && MBRandom.RandomFloat < num)
				{
					foreach (Romance.RomanticState romanticState in Romance.RomanticStateList)
					{
						if (romanticState.Level >= Romance.RomanceLevelEnum.MatchMadeByFamily && (romanticState.Person1 == playerClanHero || romanticState.Person2 == playerClanHero || romanticState.Person1 == hero || romanticState.Person2 == hero))
						{
							return false;
						}
					}
					this.CreateMarriageOffer(playerClanHero, hero);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x00110258 File Offset: 0x0010E458
		private void FinalizeMarriageOffer()
		{
			this._currentOfferedPlayerClanHero = null;
			this._currentOfferedOtherClanHero = null;
		}

		// Token: 0x040011A3 RID: 4515
		private const int MarriageOfferCooldownDurationAsWeeks = 1;

		// Token: 0x040011A4 RID: 4516
		private const int OfferRelationGainAmountWithTheMarriageClan = 10;

		// Token: 0x040011A5 RID: 4517
		private const float MapNotificationAutoDeclineDurationInHours = 48f;

		// Token: 0x040011A6 RID: 4518
		private readonly TextObject MarriageOfferPanelExplanationText = new TextObject("{=CZwrlJMJ}A courier with a marriage offer for {CLAN_MEMBER.NAME} from {OFFERING_CLAN_NAME} has arrived.", null);

		// Token: 0x040011A7 RID: 4519
		private Hero _currentOfferedPlayerClanHero;

		// Token: 0x040011A8 RID: 4520
		private Hero _currentOfferedOtherClanHero;

		// Token: 0x040011A9 RID: 4521
		private CampaignTime _lastMarriageOfferTime = CampaignTime.Zero;
	}
}
