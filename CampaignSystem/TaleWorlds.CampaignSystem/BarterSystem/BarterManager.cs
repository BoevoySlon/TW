using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.BarterSystem
{
	// Token: 0x02000410 RID: 1040
	public class BarterManager
	{
		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003F30 RID: 16176 RVA: 0x00139206 File Offset: 0x00137406
		public static BarterManager Instance
		{
			get
			{
				return Campaign.Current.BarterManager;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003F31 RID: 16177 RVA: 0x00139212 File Offset: 0x00137412
		// (set) Token: 0x06003F32 RID: 16178 RVA: 0x0013921A File Offset: 0x0013741A
		[SaveableProperty(1)]
		public bool LastBarterIsAccepted { get; internal set; }

		// Token: 0x06003F33 RID: 16179 RVA: 0x00139223 File Offset: 0x00137423
		public BarterManager()
		{
			this._barteredHeroes = new Dictionary<Hero, CampaignTime>();
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x00139236 File Offset: 0x00137436
		public void BeginPlayerBarter(BarterData args)
		{
			if (this.BarterBegin != null)
			{
				this.BarterBegin(args);
			}
			ICampaignMission campaignMission = CampaignMission.Current;
			if (campaignMission == null)
			{
				return;
			}
			campaignMission.SetMissionMode(MissionMode.Barter, false);
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x00139260 File Offset: 0x00137460
		private void AddBaseBarterables(BarterData args, IEnumerable<Barterable> defaultBarterables)
		{
			if (defaultBarterables != null)
			{
				bool flag = false;
				foreach (Barterable barterable in defaultBarterables)
				{
					if (!flag)
					{
						args.AddBarterGroup(new DefaultsBarterGroup());
						flag = true;
					}
					barterable.SetIsOffered(true);
					args.AddBarterable<OtherBarterGroup>(barterable, true);
					barterable.SetIsOffered(true);
				}
			}
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x001392CC File Offset: 0x001374CC
		public void StartBarterOffer(Hero offerer, Hero other, PartyBase offererParty, PartyBase otherParty, Hero beneficiaryOfOtherHero = null, BarterManager.BarterContextInitializer InitContext = null, int persuasionCostReduction = 0, bool isAIBarter = false, IEnumerable<Barterable> defaultBarterables = null)
		{
			this.LastBarterIsAccepted = false;
			if (offerer == Hero.MainHero && other != null && InitContext == null)
			{
				if (!this.CanPlayerBarterWithHero(other))
				{
					Debug.FailedAssert("Barter with the hero is on cooldown.", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BarterSystem\\BarterManager.cs", "StartBarterOffer", 83);
					return;
				}
				this.ClearHeroCooldowns();
			}
			BarterData args = new BarterData(offerer, beneficiaryOfOtherHero ?? other, offererParty, otherParty, InitContext, persuasionCostReduction, false);
			this.AddBaseBarterables(args, defaultBarterables);
			CampaignEventDispatcher.Instance.OnBarterablesRequested(args);
			if (!isAIBarter)
			{
				Campaign.Current.BarterManager.BeginPlayerBarter(args);
			}
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00139354 File Offset: 0x00137554
		public void ExecuteAiBarter(IFaction faction1, IFaction faction2, Hero faction1Hero, Hero faction2Hero, Barterable barterable)
		{
			this.ExecuteAiBarter(faction1, faction2, faction1Hero, faction2Hero, new Barterable[]
			{
				barterable
			});
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x00139378 File Offset: 0x00137578
		public void ExecuteAiBarter(IFaction faction1, IFaction faction2, Hero faction1Hero, Hero faction2Hero, IEnumerable<Barterable> baseBarterables)
		{
			BarterData barterData = new BarterData(faction1.Leader, faction2.Leader, null, null, null, 0, true);
			barterData.AddBarterGroup(new DefaultsBarterGroup());
			foreach (Barterable barterable in baseBarterables)
			{
				barterable.SetIsOffered(true);
				barterData.AddBarterable<DefaultsBarterGroup>(barterable, true);
			}
			CampaignEventDispatcher.Instance.OnBarterablesRequested(barterData);
			Campaign.Current.BarterManager.ExecuteAIBarter(barterData, faction1, faction2, faction1Hero, faction2Hero);
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x0013940C File Offset: 0x0013760C
		public void ExecuteAIBarter(BarterData barterData, IFaction faction1, IFaction faction2, Hero faction1Hero, Hero faction2Hero)
		{
			this.MakeBalanced(barterData, faction1, faction2, faction2Hero, 1f);
			this.MakeBalanced(barterData, faction2, faction1, faction1Hero, 1f);
			float offerValueForFaction = this.GetOfferValueForFaction(barterData, faction1);
			float offerValueForFaction2 = this.GetOfferValueForFaction(barterData, faction2);
			if (offerValueForFaction >= 0f && offerValueForFaction2 >= 0f)
			{
				this.ApplyBarterOffer(barterData.OffererHero, barterData.OtherHero, barterData.GetOfferedBarterables());
			}
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x00139474 File Offset: 0x00137674
		private void MakeBalanced(BarterData args, IFaction faction1, IFaction faction2, Hero faction2Hero, float fulfillRatio)
		{
			foreach (ValueTuple<Barterable, int> valueTuple in BarterHelper.GetAutoBalanceBarterablesAdd(args, faction1, faction2, faction2Hero, fulfillRatio))
			{
				Barterable item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (!item.IsOffered)
				{
					item.SetIsOffered(true);
					item.CurrentAmount = 0;
				}
				item.CurrentAmount += item2;
			}
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x001394F0 File Offset: 0x001376F0
		public void Close()
		{
			if (CampaignMission.Current != null)
			{
				CampaignMission.Current.SetMissionMode(MissionMode.Conversation, false);
			}
			if (this.Closed != null)
			{
				this.Closed();
			}
		}

		// Token: 0x06003F3C RID: 16188 RVA: 0x00139518 File Offset: 0x00137718
		public bool IsOfferAcceptable(BarterData args, Hero hero, PartyBase party)
		{
			return this.GetOfferValue(hero, party, args.OffererParty, args.GetOfferedBarterables()) > -0.01f;
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x00139538 File Offset: 0x00137738
		public float GetOfferValueForFaction(BarterData barterData, IFaction faction)
		{
			int num = 0;
			foreach (Barterable barterable in barterData.GetOfferedBarterables())
			{
				num += barterable.GetValueForFaction(faction);
			}
			return (float)num;
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x00139594 File Offset: 0x00137794
		public float GetOfferValue(Hero selfHero, PartyBase selfParty, PartyBase offererParty, IEnumerable<Barterable> offeredBarters)
		{
			float num = 0f;
			IFaction faction;
			if (((selfHero != null) ? selfHero.Clan : null) != null)
			{
				IFaction clan = selfHero.Clan;
				faction = clan;
			}
			else
			{
				faction = selfParty.MapFaction;
			}
			IFaction faction2 = faction;
			foreach (Barterable barterable in offeredBarters)
			{
				num += (float)barterable.GetValueForFaction(faction2);
			}
			this._overpayAmount = ((num > 0f) ? num : 0f);
			return num;
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x00139620 File Offset: 0x00137820
		public void ApplyAndFinalizePlayerBarter(Hero offererHero, Hero otherHero, BarterData barterData)
		{
			this.LastBarterIsAccepted = true;
			this.ApplyBarterOffer(offererHero, otherHero, barterData.GetOfferedBarterables());
			if (otherHero != null)
			{
				this.HandleHeroCooldown(otherHero);
			}
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x00139641 File Offset: 0x00137841
		public void CancelAndFinalizePlayerBarter(Hero offererHero, Hero otherHero, BarterData barterData)
		{
			this.CancelBarter(offererHero, otherHero, barterData.GetOfferedBarterables());
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x00139654 File Offset: 0x00137854
		private void ApplyBarterOffer(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			foreach (Barterable barterable in barters)
			{
				barterable.Apply();
			}
			CampaignEventDispatcher.Instance.OnBarterAccepted(offererHero, otherHero, barters);
			if (offererHero == Hero.MainHero)
			{
				if (this._overpayAmount > 0f && otherHero != null)
				{
					this.ApplyOverpayBonus(otherHero);
				}
				this.Close();
				if (Campaign.Current.ConversationManager.IsConversationInProgress)
				{
					Campaign.Current.ConversationManager.ContinueConversation();
				}
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_offer_accepted", null), 0, null, "");
			}
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x00139708 File Offset: 0x00137908
		private void CancelBarter(Hero offererHero, Hero otherHero, List<Barterable> offeredBarters)
		{
			this.Close();
			MBInformationManager.AddQuickInformation(GameTexts.FindText("str_offer_rejected", null), 0, null, "");
			CampaignEventDispatcher.Instance.OnBarterCanceled(offererHero, otherHero, offeredBarters);
			Campaign.Current.ConversationManager.ContinueConversation();
		}

		// Token: 0x06003F43 RID: 16195 RVA: 0x00139744 File Offset: 0x00137944
		private void ApplyOverpayBonus(Hero otherHero)
		{
			if (otherHero.MapFaction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				return;
			}
			int num = Campaign.Current.Models.BarterModel.CalculateOverpayRelationIncreaseCosts(otherHero, this._overpayAmount);
			if (num > 0)
			{
				ChangeRelationAction.ApplyRelationChangeBetweenHeroes(Hero.MainHero, otherHero, num, true);
			}
		}

		// Token: 0x06003F44 RID: 16196 RVA: 0x00139798 File Offset: 0x00137998
		public bool CanPlayerBarterWithHero(Hero hero)
		{
			CampaignTime campaignTime;
			return !this._barteredHeroes.TryGetValue(hero, out campaignTime) || campaignTime.IsPast;
		}

		// Token: 0x06003F45 RID: 16197 RVA: 0x001397C0 File Offset: 0x001379C0
		private void HandleHeroCooldown(Hero hero)
		{
			CampaignTime value = CampaignTime.Now + CampaignTime.Days((float)Campaign.Current.Models.BarterModel.BarterCooldownWithHeroInDays);
			if (!this._barteredHeroes.ContainsKey(hero))
			{
				this._barteredHeroes.Add(hero, value);
				return;
			}
			this._barteredHeroes[hero] = value;
		}

		// Token: 0x06003F46 RID: 16198 RVA: 0x0013981C File Offset: 0x00137A1C
		private void ClearHeroCooldowns()
		{
			foreach (KeyValuePair<Hero, CampaignTime> keyValuePair in new Dictionary<Hero, CampaignTime>(this._barteredHeroes))
			{
				if (keyValuePair.Value.IsPast)
				{
					this._barteredHeroes.Remove(keyValuePair.Key);
				}
			}
		}

		// Token: 0x06003F47 RID: 16199 RVA: 0x00139894 File Offset: 0x00137A94
		public bool InitializeMarriageBarterContext(Barterable barterable, BarterData args, object obj)
		{
			Hero hero = null;
			Hero hero2 = null;
			if (obj != null)
			{
				Tuple<Hero, Hero> tuple = obj as Tuple<Hero, Hero>;
				if (tuple != null)
				{
					hero = tuple.Item1;
					hero2 = tuple.Item2;
				}
			}
			MarriageBarterable marriageBarterable = barterable as MarriageBarterable;
			return marriageBarterable != null && hero != null && hero2 != null && marriageBarterable.ProposingHero == hero2 && marriageBarterable.HeroBeingProposedTo == hero;
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x001398E4 File Offset: 0x00137AE4
		public bool InitializeJoinFactionBarterContext(Barterable barterable, BarterData args, object obj)
		{
			return barterable.GetType() == typeof(JoinKingdomAsClanBarterable) && barterable.OriginalOwner == Hero.OneToOneConversationHero;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0013990C File Offset: 0x00137B0C
		public bool InitializeMakePeaceBarterContext(Barterable barterable, BarterData args, object obj)
		{
			return barterable.GetType() == typeof(PeaceBarterable) && barterable.OriginalOwner == args.OtherHero;
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x00139935 File Offset: 0x00137B35
		public bool InitializeSafePassageBarterContext(Barterable barterable, BarterData args, object obj)
		{
			if (barterable.GetType() == typeof(SafePassageBarterable))
			{
				PartyBase originalParty = barterable.OriginalParty;
				MobileParty conversationParty = MobileParty.ConversationParty;
				return originalParty == ((conversationParty != null) ? conversationParty.Party : null);
			}
			return false;
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x00139969 File Offset: 0x00137B69
		internal static void AutoGeneratedStaticCollectObjectsBarterManager(object o, List<object> collectedObjects)
		{
			((BarterManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00139977 File Offset: 0x00137B77
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._barteredHeroes);
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x00139985 File Offset: 0x00137B85
		internal static object AutoGeneratedGetMemberValueLastBarterIsAccepted(object o)
		{
			return ((BarterManager)o).LastBarterIsAccepted;
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x00139997 File Offset: 0x00137B97
		internal static object AutoGeneratedGetMemberValue_barteredHeroes(object o)
		{
			return ((BarterManager)o)._barteredHeroes;
		}

		// Token: 0x04001280 RID: 4736
		public BarterManager.BarterCloseEventDelegate Closed;

		// Token: 0x04001281 RID: 4737
		public BarterManager.BarterBeginEventDelegate BarterBegin;

		// Token: 0x04001282 RID: 4738
		[SaveableField(2)]
		private readonly Dictionary<Hero, CampaignTime> _barteredHeroes;

		// Token: 0x04001283 RID: 4739
		private float _overpayAmount;

		// Token: 0x02000760 RID: 1888
		// (Invoke) Token: 0x060059F8 RID: 23032
		public delegate bool BarterContextInitializer(Barterable barterable, BarterData args, object obj = null);

		// Token: 0x02000761 RID: 1889
		// (Invoke) Token: 0x060059FC RID: 23036
		public delegate void BarterCloseEventDelegate();

		// Token: 0x02000762 RID: 1890
		// (Invoke) Token: 0x06005A00 RID: 23040
		public delegate void BarterBeginEventDelegate(BarterData args);
	}
}
