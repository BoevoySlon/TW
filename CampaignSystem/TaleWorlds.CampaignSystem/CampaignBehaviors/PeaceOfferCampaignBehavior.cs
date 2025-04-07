using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003BE RID: 958
	public class PeaceOfferCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06003A6D RID: 14957 RVA: 0x00113BA4 File Offset: 0x00111DA4
		private static TextObject PeacePanelTitleText
		{
			get
			{
				return new TextObject("{=ho5EndaV}Decision", null);
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06003A6E RID: 14958 RVA: 0x00113BB1 File Offset: 0x00111DB1
		private static TextObject PeacePanelOkText
		{
			get
			{
				return new TextObject("{=oHaWR73d}Ok", null);
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003A6F RID: 14959 RVA: 0x00113BBE File Offset: 0x00111DBE
		private static TextObject PeacePanelAffirmativeText
		{
			get
			{
				return new TextObject("{=Y94H6XnK}Accept", null);
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003A70 RID: 14960 RVA: 0x00113BCB File Offset: 0x00111DCB
		private static TextObject PeacePanelNegativeText
		{
			get
			{
				return new TextObject("{=cOgmdp9e}Decline", null);
			}
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x00113BD8 File Offset: 0x00111DD8
		public override void RegisterEvents()
		{
			CampaignEvents.OnPeaceOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<IFaction, int>(this.OnPeaceOffered));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.OnPeaceOfferCancelledEvent.AddNonSerializedListener(this, new Action<IFaction>(this.OnPeaceOfferCancelled));
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x00113C41 File Offset: 0x00111E41
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("_currentPeaceOfferTributeAmount", ref this._currentPeaceOfferTributeAmount);
			dataStore.SyncData<IFaction>("_opponentFaction", ref this._opponentFaction);
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00113C67 File Offset: 0x00111E67
		public void SetCurrentTributeAmount(int tributeAmount)
		{
			this._currentPeaceOfferTributeAmount = tributeAmount;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00113C70 File Offset: 0x00111E70
		private void OnPeaceOffered(IFaction opponentFaction, int tributeAmount)
		{
			if (this._opponentFaction == null)
			{
				this._opponentFaction = opponentFaction;
				this._currentPeaceOfferTributeAmount = tributeAmount;
				TextObject textObject = (tributeAmount > 0) ? ((Hero.MainHero.MapFaction.Leader == Hero.MainHero) ? PeaceOfferCampaignBehavior.PeaceOfferTributePaidPanelDescriptionText : PeaceOfferCampaignBehavior.PeaceOfferTributePaidPanelPlayerIsVassalDescriptionText) : ((tributeAmount < 0) ? ((Hero.MainHero.MapFaction.Leader == Hero.MainHero) ? PeaceOfferCampaignBehavior.PeaceOfferTributeWantedPanelDescriptionText : PeaceOfferCampaignBehavior.PeaceOfferTributeWantedPanelPlayerIsVassalDescriptionText) : ((Hero.MainHero.MapFaction.Leader == Hero.MainHero) ? PeaceOfferCampaignBehavior.PeaceOfferDefaultPanelDescriptionText : PeaceOfferCampaignBehavior.PeaceOfferDefaultPanelPlayerIsVassalDescriptionText));
				textObject.SetTextVariable("MAP_FACTION_NAME", opponentFaction.InformalName);
				textObject.SetTextVariable("GOLD_AMOUNT", MathF.Abs(this._currentPeaceOfferTributeAmount));
				textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"8\">");
				TextObject peacePanelNegativeText = PeaceOfferCampaignBehavior.PeacePanelNegativeText;
				this._influenceCostOfDecline = 0;
				Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
				if (Hero.MainHero.MapFaction.Leader == Hero.MainHero)
				{
					InformationManager.ShowInquiry(new InquiryData(PeaceOfferCampaignBehavior.PeacePanelTitleText.ToString(), textObject.ToString(), true, (float)this._influenceCostOfDecline <= 0.1f || Hero.MainHero.Clan.Influence >= (float)this._influenceCostOfDecline, PeaceOfferCampaignBehavior.PeacePanelAffirmativeText.ToString(), peacePanelNegativeText.ToString(), new Action(this.AcceptPeaceOffer), new Action(this.DeclinePeaceOffer), "", 0f, null, null, null), true, false);
					this._hourCounter = 0;
					return;
				}
				InformationManager.ShowInquiry(new InquiryData(PeaceOfferCampaignBehavior.PeacePanelTitleText.ToString(), textObject.ToString(), false, true, PeaceOfferCampaignBehavior.PeacePanelOkText.ToString(), PeaceOfferCampaignBehavior.PeacePanelOkText.ToString(), new Action(this.OkPeaceOffer), new Action(this.OkPeaceOffer), "", 0f, null, null, null), true, false);
				this._hourCounter = 0;
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00113E58 File Offset: 0x00112058
		private void OnPeaceOfferCancelled(IFaction opponentFaction)
		{
			if (Hero.MainHero.MapFaction.Leader != Hero.MainHero)
			{
				this._opponentFaction = opponentFaction;
				this.OkPeaceOffer();
			}
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x00113E80 File Offset: 0x00112080
		public void HourlyTick()
		{
			if (this._opponentFaction != null)
			{
				this._hourCounter++;
				if (this._hourCounter == 24)
				{
					if (Hero.MainHero.MapFaction.Leader == Hero.MainHero)
					{
						CampaignEventDispatcher.Instance.OnPeaceOfferCancelled(this._opponentFaction);
						return;
					}
					CampaignEventDispatcher.Instance.OnPeaceOfferCancelled(this._opponentFaction);
				}
			}
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00113EE4 File Offset: 0x001120E4
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			if ((side1Faction == Hero.MainHero.MapFaction && side2Faction == this._opponentFaction) || (side2Faction == Hero.MainHero.MapFaction && side1Faction == this._opponentFaction))
			{
				this.DeclinePeaceOffer();
			}
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x00113F18 File Offset: 0x00112118
		private void OkPeaceOffer()
		{
			if (Clan.PlayerClan.IsUnderMercenaryService)
			{
				this.AcceptPeaceOffer();
				return;
			}
			MakePeaceKingdomDecision kingdomDecision = new MakePeaceKingdomDecision(Hero.MainHero.MapFaction.Leader.Clan, this._opponentFaction, -this._currentPeaceOfferTributeAmount, true);
			((Kingdom)Hero.MainHero.MapFaction).AddDecision(kingdomDecision, false);
			this._opponentFaction = null;
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x00113F7D File Offset: 0x0011217D
		private void AcceptPeaceOffer()
		{
			MakePeaceAction.Apply(this._opponentFaction, Hero.MainHero.MapFaction, this._currentPeaceOfferTributeAmount);
			this._opponentFaction = null;
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00113FA1 File Offset: 0x001121A1
		private void DeclinePeaceOffer()
		{
			CampaignEventDispatcher.Instance.OnPeaceOfferCancelled(this._opponentFaction);
			this._opponentFaction = null;
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)this._influenceCostOfDecline));
		}

		// Token: 0x040011B6 RID: 4534
		private static TextObject PeaceOfferDefaultPanelDescriptionText = new TextObject("{=IB1xsVEr}A courier has arrived from the {MAP_FACTION_NAME}. They offer you a white peace. Your vassals have left the decision with you.", null);

		// Token: 0x040011B7 RID: 4535
		private static TextObject PeaceOfferTributePaidPanelDescriptionText = new TextObject("{=JJQ0Hp4m}A courier has arrived from the {MAP_FACTION_NAME}. The {MAP_FACTION_NAME} will pay {GOLD_AMOUNT} {GOLD_ICON} in tribute each day to end the war between your realms. Your vassals have left the decision with you.", null);

		// Token: 0x040011B8 RID: 4536
		private static TextObject PeaceOfferTributeWantedPanelDescriptionText = new TextObject("{=Nd0Vhkxn}A courier has arrived from the {MAP_FACTION_NAME}. They offer you peace if you agree to pay a {GOLD_AMOUNT} {GOLD_ICON} daily tribute. Your vassals have left the decision with you.", null);

		// Token: 0x040011B9 RID: 4537
		private static TextObject PeaceOfferDefaultPanelPlayerIsVassalDescriptionText = new TextObject("{=gNf0ALKw}A courier has arrived from the {MAP_FACTION_NAME}. They offer you a white peace. Your kingdom will vote whether to accept the offer.", null);

		// Token: 0x040011BA RID: 4538
		private static TextObject PeaceOfferTributePaidPanelPlayerIsVassalDescriptionText = new TextObject("{=SR9FC5jH}A courier has arrived from the {MAP_FACTION_NAME} bearing a peace offer. The {MAP_FACTION_NAME} will pay {GOLD_AMOUNT} {GOLD_ICON} in tribute each day to end the war between your realms. Your kingdom will vote whether to accept the offer.", null);

		// Token: 0x040011BB RID: 4539
		private static TextObject PeaceOfferTributeWantedPanelPlayerIsVassalDescriptionText = new TextObject("{=sbFboHmV}A courier has arrived from the {MAP_FACTION_NAME}. They offer you peace if you agree to pay a {GOLD_AMOUNT} {GOLD_ICON} daily tribute. Your kingdom will vote whether to accept the offer.", null);

		// Token: 0x040011BC RID: 4540
		private IFaction _opponentFaction;

		// Token: 0x040011BD RID: 4541
		private int _currentPeaceOfferTributeAmount;

		// Token: 0x040011BE RID: 4542
		private int _influenceCostOfDecline;

		// Token: 0x040011BF RID: 4543
		private int _hourCounter;
	}
}
