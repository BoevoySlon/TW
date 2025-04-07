using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003CC RID: 972
	public class RansomOfferCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003BAF RID: 15279 RVA: 0x0011C9C4 File Offset: 0x0011ABC4
		private static TextObject RansomPanelTitleText
		{
			get
			{
				return new TextObject("{=ho5EndaV}Decision", null);
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x0011C9D1 File Offset: 0x0011ABD1
		private static TextObject RansomPanelAffirmativeText
		{
			get
			{
				return new TextObject("{=Y94H6XnK}Accept", null);
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06003BB1 RID: 15281 RVA: 0x0011C9DE File Offset: 0x0011ABDE
		private static TextObject RansomPanelNegativeText
		{
			get
			{
				return new TextObject("{=cOgmdp9e}Decline", null);
			}
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x0011C9EC File Offset: 0x0011ABEC
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnRansomOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnRansomOffered));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, new Action<Hero, PartyBase, IFaction, EndCaptivityDetail>(this.OnHeroPrisonerReleased));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.PrisonersChangeInSettlement.AddNonSerializedListener(this, new Action<Settlement, FlattenedTroopRoster, Hero, bool>(this.OnPrisonersChangeInSettlement));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x0011CA9A File Offset: 0x0011AC9A
		private void OnHeroPrisonerTaken(PartyBase party, Hero hero)
		{
			this.HandleDeclineRansomOffer(hero);
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x0011CAA4 File Offset: 0x0011ACA4
		private void DailyTickHero(Hero hero)
		{
			if (hero.IsPrisoner && hero.Clan != null && hero.PartyBelongedToAsPrisoner != null && hero.PartyBelongedToAsPrisoner.MapFaction != null && !hero.PartyBelongedToAsPrisoner.MapFaction.IsBanditFaction && hero != Hero.MainHero && hero.Clan.Lords.Count > 1)
			{
				this.ConsiderRansomPrisoner(hero);
			}
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x0011CB0C File Offset: 0x0011AD0C
		private void ConsiderRansomPrisoner(Hero hero)
		{
			Clan captorClanOfPrisoner = this.GetCaptorClanOfPrisoner(hero);
			if (captorClanOfPrisoner != null)
			{
				Hero hero2 = (hero.Clan.Leader != hero) ? hero.Clan.Leader : (from t in hero.Clan.Lords
				where t != hero.Clan.Leader
				select t).GetRandomElementInefficiently<Hero>();
				if (hero2 != Hero.MainHero || !hero2.IsPrisoner)
				{
					if (captorClanOfPrisoner == Clan.PlayerClan || hero.Clan == Clan.PlayerClan)
					{
						if (this._currentRansomHero == null)
						{
							float num = (!this._heroesWithDeclinedRansomOffers.Contains(hero)) ? 0.2f : 0.12f;
							if (MBRandom.RandomFloat < num)
							{
								float num2 = (float)new SetPrisonerFreeBarterable(hero, captorClanOfPrisoner.Leader, hero.PartyBelongedToAsPrisoner, hero2).GetUnitValueForFaction(hero.Clan) * 1.1f;
								if (num2 > 1E-05f && MBRandom.RandomFloat < num && (float)(hero2.Gold + 1000) >= num2)
								{
									this.SetCurrentRansomHero(hero, hero2);
									StringHelpers.SetCharacterProperties("CAPTIVE_HERO", hero.CharacterObject, RansomOfferCampaignBehavior.RansomOfferDescriptionText, false);
									Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new RansomOfferMapNotification(hero, RansomOfferCampaignBehavior.RansomOfferDescriptionText));
									return;
								}
							}
						}
					}
					else if (MBRandom.RandomFloat < 0.1f)
					{
						SetPrisonerFreeBarterable setPrisonerFreeBarterable = new SetPrisonerFreeBarterable(hero, captorClanOfPrisoner.Leader, hero.PartyBelongedToAsPrisoner, hero2);
						if (setPrisonerFreeBarterable.GetValueForFaction(captorClanOfPrisoner) + setPrisonerFreeBarterable.GetValueForFaction(hero.Clan) > 0)
						{
							Campaign.Current.BarterManager.ExecuteAiBarter(captorClanOfPrisoner, hero.Clan, captorClanOfPrisoner.Leader, hero2, setPrisonerFreeBarterable);
						}
					}
				}
			}
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x0011CD08 File Offset: 0x0011AF08
		private Clan GetCaptorClanOfPrisoner(Hero hero)
		{
			Clan result;
			if (hero.PartyBelongedToAsPrisoner.IsMobile)
			{
				if ((hero.PartyBelongedToAsPrisoner.MobileParty.IsMilitia || hero.PartyBelongedToAsPrisoner.MobileParty.IsGarrison || hero.PartyBelongedToAsPrisoner.MobileParty.IsCaravan || hero.PartyBelongedToAsPrisoner.MobileParty.IsVillager) && hero.PartyBelongedToAsPrisoner.Owner != null)
				{
					if (hero.PartyBelongedToAsPrisoner.Owner.IsNotable)
					{
						result = hero.PartyBelongedToAsPrisoner.Owner.CurrentSettlement.OwnerClan;
					}
					else
					{
						result = hero.PartyBelongedToAsPrisoner.Owner.Clan;
					}
				}
				else
				{
					result = hero.PartyBelongedToAsPrisoner.MobileParty.ActualClan;
				}
			}
			else
			{
				result = hero.PartyBelongedToAsPrisoner.Settlement.OwnerClan;
			}
			return result;
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x0011CDDE File Offset: 0x0011AFDE
		public void SetCurrentRansomHero(Hero hero, Hero ransomPayer = null)
		{
			this._currentRansomHero = hero;
			this._currentRansomPayer = ransomPayer;
			this._currentRansomOfferDate = ((hero != null) ? CampaignTime.Now : CampaignTime.Never);
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x0011CE04 File Offset: 0x0011B004
		private void OnRansomOffered(Hero captiveHero)
		{
			Clan captorClanOfPrisoner = this.GetCaptorClanOfPrisoner(captiveHero);
			Clan clan = (captiveHero.Clan == Clan.PlayerClan) ? captorClanOfPrisoner : captiveHero.Clan;
			Hero hero = (captiveHero.Clan.Leader != captiveHero) ? captiveHero.Clan.Leader : (from t in captiveHero.Clan.Lords
			where t != captiveHero.Clan.Leader
			select t).GetRandomElementInefficiently<Hero>();
			int ransomPrice = (int)Math.Min((float)hero.Gold, (float)new SetPrisonerFreeBarterable(captiveHero, captorClanOfPrisoner.Leader, captiveHero.PartyBelongedToAsPrisoner, hero).GetUnitValueForFaction(captiveHero.Clan) * 1.1f);
			TextObject textObject = (captorClanOfPrisoner == Clan.PlayerClan) ? RansomOfferCampaignBehavior.RansomPanelDescriptionPlayerHeldPrisonerText : RansomOfferCampaignBehavior.RansomPanelDescriptionNpcHeldPrisonerText;
			textObject.SetTextVariable("CLAN_NAME", clan.Name);
			textObject.SetTextVariable("GOLD_AMOUNT", ransomPrice);
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
			StringHelpers.SetCharacterProperties("CAPTIVE_HERO", captiveHero.CharacterObject, textObject, false);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			InformationManager.ShowInquiry(new InquiryData(RansomOfferCampaignBehavior.RansomPanelTitleText.ToString(), textObject.ToString(), true, true, RansomOfferCampaignBehavior.RansomPanelAffirmativeText.ToString(), RansomOfferCampaignBehavior.RansomPanelNegativeText.ToString(), delegate()
			{
				this.AcceptRansomOffer(ransomPrice);
			}, new Action(this.DeclineRansomOffer), "", 0f, null, null, null), true, false);
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x0011CFB8 File Offset: 0x0011B1B8
		private void AcceptRansomOffer(int ransomPrice)
		{
			if (this._heroesWithDeclinedRansomOffers.Contains(this._currentRansomHero))
			{
				this._heroesWithDeclinedRansomOffers.Remove(this._currentRansomHero);
			}
			GiveGoldAction.ApplyBetweenCharacters(this._currentRansomPayer, this.GetCaptorClanOfPrisoner(this._currentRansomHero).Leader, ransomPrice, false);
			EndCaptivityAction.ApplyByRansom(this._currentRansomHero, this._currentRansomHero.Clan.Leader);
			IStatisticsCampaignBehavior behavior = Campaign.Current.CampaignBehaviorManager.GetBehavior<IStatisticsCampaignBehavior>();
			if (behavior != null)
			{
				behavior.OnPlayerAcceptedRansomOffer(ransomPrice);
			}
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x0011D040 File Offset: 0x0011B240
		private void DeclineRansomOffer()
		{
			if (this._currentRansomHero.IsPrisoner && this._currentRansomHero.IsAlive && !this._heroesWithDeclinedRansomOffers.Contains(this._currentRansomHero))
			{
				this._heroesWithDeclinedRansomOffers.Add(this._currentRansomHero);
			}
			this.SetCurrentRansomHero(null, null);
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x0011D093 File Offset: 0x0011B293
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.HandleDeclineRansomOffer(victim);
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0011D09C File Offset: 0x0011B29C
		private void HandleDeclineRansomOffer(Hero victim)
		{
			if (this._currentRansomHero != null && (victim == this._currentRansomHero || victim == Hero.MainHero))
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
				this.DeclineRansomOffer();
			}
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x0011D0D0 File Offset: 0x0011B2D0
		private void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster roster, Hero prisoner, bool takenFromDungeon)
		{
			if (!takenFromDungeon && this._currentRansomHero != null)
			{
				if (prisoner == this._currentRansomHero)
				{
					CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
					this.DeclineRansomOffer();
					return;
				}
				if (roster != null)
				{
					foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in roster)
					{
						if (flattenedTroopRosterElement.Troop.IsHero && flattenedTroopRosterElement.Troop.HeroObject == this._currentRansomHero)
						{
							CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
							this.DeclineRansomOffer();
							break;
						}
					}
				}
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x0011D180 File Offset: 0x0011B380
		private void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail)
		{
			this.HandleDeclineRansomOffer(prisoner);
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x0011D189 File Offset: 0x0011B389
		private void HourlyTick()
		{
			if (this._currentRansomHero != null && this._currentRansomOfferDate.ElapsedHoursUntilNow >= 48f)
			{
				CampaignEventDispatcher.Instance.OnRansomOfferCancelled(this._currentRansomHero);
				this.DeclineRansomOffer();
			}
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x0011D1BC File Offset: 0x0011B3BC
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Hero>>("_heroesWithDeclinedRansomOffers", ref this._heroesWithDeclinedRansomOffers);
			dataStore.SyncData<Hero>("_currentRansomHero", ref this._currentRansomHero);
			dataStore.SyncData<Hero>("_currentRansomPayer", ref this._currentRansomPayer);
			dataStore.SyncData<CampaignTime>("_currentRansomOfferDate", ref this._currentRansomOfferDate);
		}

		// Token: 0x040011D7 RID: 4567
		private const float RansomOfferInitialChance = 0.2f;

		// Token: 0x040011D8 RID: 4568
		private const float RansomOfferChanceAfterRefusal = 0.12f;

		// Token: 0x040011D9 RID: 4569
		private const float RansomOfferChanceForPrisonersKeptByAI = 0.1f;

		// Token: 0x040011DA RID: 4570
		private const float MapNotificationAutoDeclineDurationInHours = 48f;

		// Token: 0x040011DB RID: 4571
		private const int AmountOfGoldLeftAfterRansom = 1000;

		// Token: 0x040011DC RID: 4572
		private static TextObject RansomOfferDescriptionText = new TextObject("{=ZqJ92UN4}A courier with a ransom offer for the freedom of {CAPTIVE_HERO.NAME} has arrived.", null);

		// Token: 0x040011DD RID: 4573
		private static TextObject RansomPanelDescriptionNpcHeldPrisonerText = new TextObject("{=4fXpOe4N}A courier arrives from the {CLAN_NAME}. They hold {CAPTIVE_HERO.NAME} and are demanding {GOLD_AMOUNT}{GOLD_ICON} in ransom.", null);

		// Token: 0x040011DE RID: 4574
		private static TextObject RansomPanelDescriptionPlayerHeldPrisonerText = new TextObject("{=PutoRsWp}A courier arrives from the {CLAN_NAME}. They offer you {GOLD_AMOUNT}{GOLD_ICON} in ransom if you will free {CAPTIVE_HERO.NAME}.", null);

		// Token: 0x040011DF RID: 4575
		private List<Hero> _heroesWithDeclinedRansomOffers = new List<Hero>();

		// Token: 0x040011E0 RID: 4576
		private Hero _currentRansomHero;

		// Token: 0x040011E1 RID: 4577
		private Hero _currentRansomPayer;

		// Token: 0x040011E2 RID: 4578
		private CampaignTime _currentRansomOfferDate;
	}
}
