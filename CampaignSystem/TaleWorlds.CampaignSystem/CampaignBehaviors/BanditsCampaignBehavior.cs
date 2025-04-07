using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000376 RID: 886
	public class BanditsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x0600340A RID: 13322 RVA: 0x000D8D6C File Offset: 0x000D6F6C
		private int IdealBanditPartyCount
		{
			get
			{
				return this._numberOfMaxHideoutsAtEachBanditFaction * (this._numberOfMaxBanditPartiesAroundEachHideout + this._numberOfMaximumBanditPartiesInEachHideout) + this._numberOfMaximumLooterParties;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x0600340B RID: 13323 RVA: 0x000D8D89 File Offset: 0x000D6F89
		private int _numberOfMaximumLooterParties
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfMaximumLooterParties;
			}
		}

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x0600340C RID: 13324 RVA: 0x000D8D9F File Offset: 0x000D6F9F
		private float _radiusAroundPlayerPartySquared
		{
			get
			{
				return MobileParty.MainParty.SeeingRange * MobileParty.MainParty.SeeingRange * 1.25f;
			}
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x000D8DBC File Offset: 0x000D6FBC
		private float _numberOfMinimumBanditPartiesInAHideoutToInfestIt
		{
			get
			{
				return (float)Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
			}
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x0600340E RID: 13326 RVA: 0x000D8DD3 File Offset: 0x000D6FD3
		private int _numberOfMaxBanditPartiesAroundEachHideout
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesAroundEachHideout;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x0600340F RID: 13327 RVA: 0x000D8DE9 File Offset: 0x000D6FE9
		private int _numberOfMaxHideoutsAtEachBanditFaction
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfMaximumHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06003410 RID: 13328 RVA: 0x000D8DFF File Offset: 0x000D6FFF
		private int _numberOfInitialHideoutsAtEachBanditFaction
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfInitialHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06003411 RID: 13329 RVA: 0x000D8E15 File Offset: 0x000D7015
		private int _numberOfMaximumBanditPartiesInEachHideout
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesInEachHideout;
			}
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000D8E2C File Offset: 0x000D702C
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnPartyDestroyed));
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x000D8EF1 File Offset: 0x000D70F1
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x000D8EF3 File Offset: 0x000D70F3
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<MobileParty, BanditsCampaignBehavior.PlayerInteraction>>("_interactedBandits", ref this._interactedBandits);
			dataStore.SyncData<bool>("_hideoutsAndBanditsAreInited", ref this._hideoutsAndBanditsAreInitialized);
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x000D8F1C File Offset: 0x000D711C
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i == 0)
			{
				this.MakeBanditFactionsEnemyToKingdomFactions();
				this._hideoutsAndBanditsAreInitialized = false;
			}
			if (i < 10)
			{
				if (this._numberOfMaxHideoutsAtEachBanditFaction > 0)
				{
					int num = Clan.BanditFactions.Count((Clan x) => !BanditsCampaignBehavior.IsLooterFaction(x));
					int num2 = num / 10 + ((num % 10 > i) ? 1 : 0);
					int num3 = num / 10 * i;
					for (int j = 0; j < i; j++)
					{
						num3 += ((num % 10 > j) ? 1 : 0);
					}
					for (int k = 0; k < num2; k++)
					{
						this.SpawnHideoutsAndBanditsPartiallyOnNewGame(Clan.BanditFactions.ElementAt(num3 + k));
					}
					return;
				}
			}
			else
			{
				int num4 = i - 10;
				int idealBanditPartyCount = this.IdealBanditPartyCount;
				int num5 = idealBanditPartyCount / 90 + ((idealBanditPartyCount % 90 > num4) ? 1 : 0);
				int num6 = idealBanditPartyCount / 90 * num4;
				for (int l = 0; l < num4; l++)
				{
					num6 += ((idealBanditPartyCount % 90 > l) ? 1 : 0);
				}
				for (int m = 0; m < num5; m++)
				{
					this.SpawnBanditOrLooterPartiesAroundAHideoutOrSettlement(num6 + m);
				}
			}
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x000D903C File Offset: 0x000D723C
		private void SpawnHideoutsAndBanditsPartiallyOnNewGame(Clan banditClan)
		{
			if (!BanditsCampaignBehavior.IsLooterFaction(banditClan))
			{
				for (int i = 0; i < this._numberOfInitialHideoutsAtEachBanditFaction; i++)
				{
					this.FillANewHideoutWithBandits(banditClan);
				}
			}
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x000D906C File Offset: 0x000D726C
		private void MakeBanditFactionsEnemyToKingdomFactions()
		{
			foreach (Clan clan in Clan.BanditFactions)
			{
				if (clan.IsBanditFaction && !clan.IsMinorFaction)
				{
					foreach (Kingdom faction in Kingdom.All)
					{
						FactionManager.DeclareWar(faction, clan, false);
					}
					FactionManager.DeclareWar(Hero.MainHero.Clan, clan, false);
				}
			}
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x000D9114 File Offset: 0x000D7314
		private void OnPartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._interactedBandits.ContainsKey(mobileParty))
			{
				this._interactedBandits.Remove(mobileParty);
			}
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x000D9131 File Offset: 0x000D7331
		private void SetPlayerInteraction(MobileParty mobileParty, BanditsCampaignBehavior.PlayerInteraction interaction)
		{
			if (this._interactedBandits.ContainsKey(mobileParty))
			{
				this._interactedBandits[mobileParty] = interaction;
				return;
			}
			this._interactedBandits.Add(mobileParty, interaction);
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x000D915C File Offset: 0x000D735C
		private BanditsCampaignBehavior.PlayerInteraction GetPlayerInteraction(MobileParty mobileParty)
		{
			BanditsCampaignBehavior.PlayerInteraction result;
			if (this._interactedBandits.TryGetValue(mobileParty, out result))
			{
				return result;
			}
			return BanditsCampaignBehavior.PlayerInteraction.None;
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000D917C File Offset: 0x000D737C
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			this.CheckForSpawningBanditBoss(settlement, mobileParty);
			if (Campaign.Current.GameStarted && mobileParty != null && mobileParty.IsBandit && settlement.IsHideout)
			{
				if (!settlement.Hideout.IsSpotted && settlement.Hideout.IsInfested)
				{
					float lengthSquared = (MobileParty.MainParty.Position2D - settlement.Position2D).LengthSquared;
					float seeingRange = MobileParty.MainParty.SeeingRange;
					float num = seeingRange * seeingRange / lengthSquared;
					float partySpottingDifficulty = Campaign.Current.Models.MapVisibilityModel.GetPartySpottingDifficulty(MobileParty.MainParty, mobileParty);
					if (num / partySpottingDifficulty >= 1f)
					{
						settlement.Hideout.IsSpotted = true;
						settlement.Party.UpdateVisibilityAndInspected(0f);
						CampaignEventDispatcher.Instance.OnHideoutSpotted(MobileParty.MainParty.Party, settlement.Party);
					}
				}
				int num2 = 0;
				foreach (ItemRosterElement itemRosterElement in mobileParty.ItemRoster)
				{
					int num3 = itemRosterElement.EquipmentElement.Item.IsFood ? MBRandom.RoundRandomized((float)mobileParty.MemberRoster.TotalManCount * ((3f + 6f * MBRandom.RandomFloat) / (float)itemRosterElement.EquipmentElement.Item.Value)) : 0;
					if (itemRosterElement.Amount > num3)
					{
						int num4 = itemRosterElement.Amount - num3;
						num2 += num4 * itemRosterElement.EquipmentElement.Item.Value;
					}
				}
				if (num2 > 0)
				{
					if (mobileParty.IsPartyTradeActive)
					{
						mobileParty.PartyTradeGold += (int)(0.25f * (float)num2);
					}
					settlement.SettlementComponent.ChangeGold((int)(0.25f * (float)num2));
				}
			}
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000D9368 File Offset: 0x000D7568
		private void CheckForSpawningBanditBoss(Settlement settlement, MobileParty mobileParty)
		{
			if (settlement.IsHideout && settlement.Hideout.IsSpotted)
			{
				if (settlement.Parties.Any((MobileParty x) => x.IsBandit || x.IsBanditBossParty))
				{
					CultureObject culture = settlement.Culture;
					MobileParty mobileParty2 = settlement.Parties.FirstOrDefault((MobileParty x) => x.IsBanditBossParty);
					if (mobileParty2 == null)
					{
						this.AddBossParty(settlement, culture);
						return;
					}
					if (!mobileParty2.MemberRoster.Contains(culture.BanditBoss))
					{
						mobileParty2.MemberRoster.AddToCounts(culture.BanditBoss, 1, false, 0, 0, true, -1);
					}
				}
			}
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000D9428 File Offset: 0x000D7628
		private void AddBossParty(Settlement settlement, CultureObject culture)
		{
			PartyTemplateObject banditBossPartyTemplate = culture.BanditBossPartyTemplate;
			if (banditBossPartyTemplate != null)
			{
				this.AddBanditToHideout(settlement.Hideout, banditBossPartyTemplate, true).Ai.DisableAi();
			}
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x000D9458 File Offset: 0x000D7658
		public void DailyTick()
		{
			foreach (MobileParty mobileParty in MobileParty.AllBanditParties)
			{
				if (mobileParty.IsPartyTradeActive)
				{
					mobileParty.PartyTradeGold = (int)((double)mobileParty.PartyTradeGold * 0.95 + (double)(50f * (float)mobileParty.Party.MemberRoster.TotalManCount * 0.05f));
					if (MBRandom.RandomFloat < 0.03f && mobileParty.MapEvent != null)
					{
						foreach (ItemObject itemObject in Items.All)
						{
							if (itemObject.IsFood)
							{
								int num = BanditsCampaignBehavior.IsLooterFaction(mobileParty.MapFaction) ? 8 : 16;
								int num2 = MBRandom.RoundRandomized((float)mobileParty.MemberRoster.TotalManCount * (1f / (float)itemObject.Value) * (float)num * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat);
								if (num2 > 0)
								{
									mobileParty.ItemRoster.AddToCounts(itemObject, num2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x000D95C4 File Offset: 0x000D77C4
		private void TryToSpawnHideoutAndBanditHourly()
		{
			this._hideoutsAndBanditsAreInitialized = true;
			int num = 0;
			foreach (Clan faction in Clan.BanditFactions)
			{
				if (!BanditsCampaignBehavior.IsLooterFaction(faction))
				{
					for (int i = 0; i < this._numberOfInitialHideoutsAtEachBanditFaction; i++)
					{
						this.FillANewHideoutWithBandits(faction);
						num++;
					}
				}
			}
			int num2 = (int)(0.5f * (float)(this._numberOfMaxBanditPartiesAroundEachHideout * num + this._numberOfMaximumLooterParties));
			if (num2 > 0)
			{
				this.SpawnBanditOrLooterPartiesAroundAHideoutOrSettlement(num2);
			}
		}

		// Token: 0x06003420 RID: 13344 RVA: 0x000D9660 File Offset: 0x000D7860
		public void HourlyTick()
		{
			if (!this._hideoutsAndBanditsAreInitialized && this._numberOfMaxHideoutsAtEachBanditFaction > 0)
			{
				this.TryToSpawnHideoutAndBanditHourly();
			}
			if (Campaign.Current.IsNight)
			{
				int num = 0;
				foreach (Clan clan in Clan.BanditFactions)
				{
					num += clan.WarPartyComponents.Count;
				}
				int num2 = MBRandom.RoundRandomized((float)(this.IdealBanditPartyCount - num) * 0.01f);
				if (num2 > 0)
				{
					this.SpawnBanditOrLooterPartiesAroundAHideoutOrSettlement(num2);
				}
			}
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x000D96F8 File Offset: 0x000D78F8
		public void WeeklyTick()
		{
			this.AddNewHideouts();
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x000D9700 File Offset: 0x000D7900
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000D970C File Offset: 0x000D790C
		private void AddNewHideouts()
		{
			foreach (Clan clan in Clan.BanditFactions)
			{
				if (!BanditsCampaignBehavior.IsLooterFaction(clan))
				{
					int num = 0;
					foreach (Hideout hideout in Campaign.Current.AllHideouts)
					{
						if (hideout.IsInfested && hideout.Settlement.Culture == clan.Culture)
						{
							num++;
						}
					}
					float num2 = 0f;
					if ((float)num < (float)this._numberOfMaxHideoutsAtEachBanditFaction * 0.5f)
					{
						num2 = 1f - (float)num / (float)MathF.Ceiling((float)this._numberOfMaxHideoutsAtEachBanditFaction * 0.5f);
						num2 = MathF.Max(0f, num2 * num2);
					}
					if (MBRandom.RandomFloat < num2)
					{
						this.FillANewHideoutWithBandits(clan);
					}
				}
			}
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000D9818 File Offset: 0x000D7A18
		private void FillANewHideoutWithBandits(Clan faction)
		{
			Hideout hideout = this.SelectARandomHideout(faction, false, true, true);
			if (hideout != null)
			{
				int num = 0;
				while ((float)num < this._numberOfMinimumBanditPartiesInAHideoutToInfestIt)
				{
					this.AddBanditToHideout(hideout, null, false);
					num++;
				}
			}
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000D9850 File Offset: 0x000D7A50
		public MobileParty AddBanditToHideout(Hideout hideoutComponent, PartyTemplateObject overridenPartyTemplate = null, bool isBanditBossParty = false)
		{
			if (hideoutComponent.Owner.Settlement.Culture.IsBandit)
			{
				Clan clan = null;
				foreach (Clan clan2 in Clan.BanditFactions)
				{
					if (hideoutComponent.Owner.Settlement.Culture == clan2.Culture)
					{
						clan = clan2;
					}
				}
				PartyTemplateObject pt = overridenPartyTemplate ?? clan.DefaultPartyTemplate;
				MobileParty mobileParty = BanditPartyComponent.CreateBanditParty(clan.StringId + "_1", clan, hideoutComponent, isBanditBossParty);
				mobileParty.InitializeMobilePartyAtPosition(pt, hideoutComponent.Owner.Settlement.Position2D, -1);
				this.InitBanditParty(mobileParty, clan, hideoutComponent.Owner.Settlement);
				mobileParty.Ai.SetMoveGoToSettlement(hideoutComponent.Owner.Settlement);
				mobileParty.Ai.RecalculateShortTermAi();
				EnterSettlementAction.ApplyForParty(mobileParty, hideoutComponent.Owner.Settlement);
				return mobileParty;
			}
			return null;
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x000D9954 File Offset: 0x000D7B54
		private Hideout SelectARandomHideout(Clan faction, bool isInfestedHideoutNeeded, bool sameFactionIsNeeded, bool selectingFurtherToOthersNeeded = false)
		{
			float num = Campaign.AverageDistanceBetweenTwoFortifications * 0.33f * Campaign.AverageDistanceBetweenTwoFortifications * 0.33f;
			List<ValueTuple<Hideout, float>> list = new List<ValueTuple<Hideout, float>>();
			foreach (Hideout hideout in Hideout.All)
			{
				if ((!sameFactionIsNeeded || hideout.Settlement.Culture == faction.Culture) && (!isInfestedHideoutNeeded || hideout.IsInfested))
				{
					int num2 = 1;
					if (hideout.Settlement.LastThreatTime.ElapsedHoursUntilNow > 36f && selectingFurtherToOthersNeeded)
					{
						float num3 = Campaign.MapDiagonalSquared;
						float num4 = Campaign.MapDiagonalSquared;
						foreach (Hideout hideout2 in Hideout.All)
						{
							if (hideout != hideout2 && hideout2.IsInfested)
							{
								float num5 = hideout.Settlement.Position2D.DistanceSquared(hideout2.Settlement.Position2D);
								if (hideout.Settlement.Culture == hideout2.Settlement.Culture && num5 < num3)
								{
									num3 = num5;
								}
								if (num5 < num4)
								{
									num4 = num5;
								}
							}
						}
						num2 = (int)MathF.Max(1f, num3 / num + 5f * (num4 / num));
					}
					list.Add(new ValueTuple<Hideout, float>(hideout, (float)num2));
				}
			}
			return MBRandom.ChooseWeighted<Hideout>(list);
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x000D9B04 File Offset: 0x000D7D04
		private void SpawnBanditOrLooterPartiesAroundAHideoutOrSettlement(int numberOfBanditsWillBeSpawned)
		{
			List<Clan> list = Clan.BanditFactions.ToList<Clan>();
			Dictionary<Clan, int> dictionary = new Dictionary<Clan, int>(list.Count);
			foreach (Clan key in list)
			{
				dictionary.Add(key, 0);
			}
			foreach (Hideout hideout in Campaign.Current.AllHideouts)
			{
				if (hideout.IsInfested && hideout.MapFaction is Clan)
				{
					Dictionary<Clan, int> dictionary2 = dictionary;
					Clan key2 = hideout.MapFaction as Clan;
					int num = dictionary2[key2];
					dictionary2[key2] = num + 1;
				}
			}
			int num2 = this._numberOfMaxBanditPartiesAroundEachHideout + this._numberOfMaximumBanditPartiesInEachHideout + 1;
			int num3 = this._numberOfMaxHideoutsAtEachBanditFaction * num2;
			int num4 = 0;
			foreach (Clan clan in list)
			{
				num4 += clan.WarPartyComponents.Count;
			}
			numberOfBanditsWillBeSpawned = MathF.Max(0, MathF.Min(numberOfBanditsWillBeSpawned, list.Count((Clan f) => !BanditsCampaignBehavior.IsLooterFaction(f)) * num3 + this._numberOfMaximumLooterParties - num4));
			numberOfBanditsWillBeSpawned = MathF.Ceiling((float)numberOfBanditsWillBeSpawned * 0.667f) + MBRandom.RandomInt(numberOfBanditsWillBeSpawned / 3);
			for (int i = 0; i < numberOfBanditsWillBeSpawned; i++)
			{
				Clan clan2 = null;
				float num5 = 1f;
				for (int j = 0; j < list.Count; j++)
				{
					float num6 = 1f;
					if (BanditsCampaignBehavior.IsLooterFaction(list[j]))
					{
						num6 = (float)list[j].WarPartyComponents.Count / (float)this._numberOfMaximumLooterParties;
					}
					else
					{
						int num7 = dictionary[list[j]];
						if (num7 > 0)
						{
							num6 = (float)list[j].WarPartyComponents.Count / (float)(num7 * num2);
						}
					}
					if (num6 < 1f && (clan2 == null || num6 < num5))
					{
						clan2 = list[j];
						num5 = num6;
					}
				}
				if (clan2 == null)
				{
					return;
				}
				this.SpawnAPartyInFaction(clan2);
			}
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000D9D74 File Offset: 0x000D7F74
		private void SpawnAPartyInFaction(Clan selectedFaction)
		{
			PartyTemplateObject defaultPartyTemplate = selectedFaction.DefaultPartyTemplate;
			Settlement settlement;
			if (BanditsCampaignBehavior.IsLooterFaction(selectedFaction))
			{
				settlement = this.SelectARandomSettlementForLooterParty();
			}
			else
			{
				Hideout hideout = this.SelectARandomHideout(selectedFaction, true, true, false);
				settlement = ((hideout != null) ? hideout.Owner.Settlement : null);
				if (settlement == null)
				{
					hideout = this.SelectARandomHideout(selectedFaction, false, true, false);
					settlement = ((hideout != null) ? hideout.Owner.Settlement : null);
					if (settlement == null)
					{
						hideout = this.SelectARandomHideout(selectedFaction, false, false, false);
						settlement = ((hideout != null) ? hideout.Owner.Settlement : null);
					}
				}
			}
			MobileParty mobileParty = settlement.IsHideout ? BanditPartyComponent.CreateBanditParty(selectedFaction.StringId + "_1", selectedFaction, settlement.Hideout, false) : BanditPartyComponent.CreateLooterParty(selectedFaction.StringId + "_1", selectedFaction, settlement, false);
			if (settlement != null)
			{
				float num = 45f * (BanditsCampaignBehavior.IsLooterFaction(selectedFaction) ? 1.5f : 1f);
				mobileParty.InitializeMobilePartyAroundPosition(defaultPartyTemplate, settlement.GatePosition, num, 0f, -1);
				Vec2 vec = mobileParty.Position2D;
				float radiusAroundPlayerPartySquared = this._radiusAroundPlayerPartySquared;
				for (int i = 0; i < 15; i++)
				{
					Vec2 vec2 = MobilePartyHelper.FindReachablePointAroundPosition(vec, num, 0f);
					if (vec2.DistanceSquared(MobileParty.MainParty.Position2D) > radiusAroundPlayerPartySquared)
					{
						vec = vec2;
						break;
					}
				}
				if (vec != mobileParty.Position2D)
				{
					mobileParty.Position2D = vec;
				}
				this.InitBanditParty(mobileParty, selectedFaction, settlement);
				mobileParty.Aggressiveness = 1f - 0.2f * MBRandom.RandomFloat;
				mobileParty.Ai.SetMovePatrolAroundPoint(settlement.IsTown ? settlement.GatePosition : settlement.Position2D);
			}
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000D9F13 File Offset: 0x000D8113
		private static bool IsLooterFaction(IFaction faction)
		{
			return !faction.Culture.CanHaveSettlement;
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000D9F24 File Offset: 0x000D8124
		private Settlement SelectARandomSettlementForLooterParty()
		{
			int num = 0;
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown || settlement.IsVillage)
				{
					int num2 = BanditsCampaignBehavior.CalculateDistanceScore(settlement.Position2D.DistanceSquared(MobileParty.MainParty.Position2D));
					num += num2;
				}
			}
			int num3 = MBRandom.RandomInt(num);
			foreach (Settlement settlement2 in Settlement.All)
			{
				if (settlement2.IsTown || settlement2.IsVillage)
				{
					int num4 = BanditsCampaignBehavior.CalculateDistanceScore(settlement2.Position2D.DistanceSquared(MobileParty.MainParty.Position2D));
					num3 -= num4;
					if (num3 <= 0)
					{
						return settlement2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000DA030 File Offset: 0x000D8230
		private void InitBanditParty(MobileParty banditParty, Clan faction, Settlement homeSettlement)
		{
			banditParty.Party.SetVisualAsDirty();
			banditParty.ActualClan = faction;
			BanditsCampaignBehavior.CreatePartyTrade(banditParty);
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.IsFood)
				{
					int num = BanditsCampaignBehavior.IsLooterFaction(banditParty.MapFaction) ? 8 : 16;
					int num2 = MBRandom.RoundRandomized((float)banditParty.MemberRoster.TotalManCount * (1f / (float)itemObject.Value) * (float)num * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat);
					if (num2 > 0)
					{
						banditParty.ItemRoster.AddToCounts(itemObject, num2);
					}
				}
			}
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000DA0FC File Offset: 0x000D82FC
		private static void CreatePartyTrade(MobileParty banditParty)
		{
			int initialGold = (int)(10f * (float)banditParty.Party.MemberRoster.TotalManCount * (0.5f + 1f * MBRandom.RandomFloat));
			banditParty.InitializePartyTrade(initialGold);
		}

		// Token: 0x0600342D RID: 13357 RVA: 0x000DA13C File Offset: 0x000D833C
		private static int CalculateDistanceScore(float distance)
		{
			int result = 2;
			if (distance < 10000f)
			{
				result = 8;
			}
			else if (distance < 40000f)
			{
				result = 6;
			}
			else if (distance < 160000f)
			{
				result = 4;
			}
			else if (distance < 420000f)
			{
				result = 3;
			}
			return result;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x000DA17C File Offset: 0x000D837C
		protected void AddDialogs(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddDialogLine("bandit_start_defender", "start", "bandit_defender", "{=!}{ROBBERY_THREAT}", new ConversationSentence.OnConditionDelegate(this.bandit_start_defender_condition), null, 100, null);
			campaignGameSystemStarter.AddPlayerLine("bandit_start_defender_1", "bandit_defender", "bandit_start_fight", "{=DEnFOGhS}Fight me if you dare!", null, null, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("bandit_start_defender_2", "bandit_defender", "barter_with_bandit_prebarter", "{=aQYMefHU}Maybe we can work out something.", new ConversationSentence.OnConditionDelegate(this.bandit_start_barter_condition), null, 100, null, null);
			campaignGameSystemStarter.AddDialogLine("bandit_start_fight", "bandit_start_fight", "close_window", "{=!}{ROBBERY_START_FIGHT}[ib:aggressive]", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_bandit_set_hostile_on_consequence), 100, null);
			campaignGameSystemStarter.AddDialogLine("barter_with_bandit_prebarter", "barter_with_bandit_prebarter", "barter_with_bandit_screen", "{=!}{ROBBERY_PAY_AGREEMENT}", null, null, 100, null);
			campaignGameSystemStarter.AddDialogLine("barter_with_bandit_screen", "barter_with_bandit_screen", "barter_with_bandit_postbarter", "{=!}Barter screen goes here", null, new ConversationSentence.OnConsequenceDelegate(this.bandit_start_barter_consequence), 100, null);
			campaignGameSystemStarter.AddDialogLine("barter_with_bandit_postbarter_1", "barter_with_bandit_postbarter", "close_window", "{=!}{ROBBERY_CONCLUSION}", new ConversationSentence.OnConditionDelegate(this.bandit_barter_successful_condition), new ConversationSentence.OnConsequenceDelegate(this.bandit_barter_successful_on_consequence), 100, null);
			campaignGameSystemStarter.AddDialogLine("barter_with_bandit_postbarter_2", "barter_with_bandit_postbarter", "close_window", "{=!}{ROBBERY_START_FIGHT}", () => !this.bandit_barter_successful_condition(), new ConversationSentence.OnConsequenceDelegate(this.conversation_bandit_set_hostile_on_consequence), 100, null);
			campaignGameSystemStarter.AddDialogLine("bandit_start_attacker", "start", "bandit_attacker", "{=!}{BANDIT_NEUTRAL_GREETING}", new ConversationSentence.OnConditionDelegate(this.bandit_neutral_greet_on_condition), new ConversationSentence.OnConsequenceDelegate(this.bandit_neutral_greet_on_consequence), 100, null);
			campaignGameSystemStarter.AddPlayerLine("common_encounter_ultimatum", "bandit_attacker", "common_encounter_ultimatum_answer", "{=!}{BANDIT_ULTIMATUM}", null, null, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_encounter_fight", "bandit_attacker", "bandit_attacker_leave", "{=3W3eEIIZ}Never mind. You can go.", null, null, 100, null, null);
			campaignGameSystemStarter.AddDialogLine("common_encounter_ultimatum_we_can_join_you", "common_encounter_ultimatum_answer", "bandits_we_can_join_you", "{=B5UMlqHc}I'll be honest... We don't want to die. Would you take us on as hired fighters? That way everyone gets what they want.", new ConversationSentence.OnConditionDelegate(this.conversation_bandits_will_join_player_on_condition), null, 100, null);
			campaignGameSystemStarter.AddDialogLine("common_encounter_ultimatum_war", "common_encounter_ultimatum_answer", "close_window", "{=n99VA8KP}You'll never take us alive![if:convo_angry][ib:aggressive]", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_bandit_set_hostile_on_consequence), 100, null);
			campaignGameSystemStarter.AddPlayerLine("common_bandit_join_player_accepted", "bandits_we_can_join_you", "close_window", "{=XdKCuzg1}Very well. You may join us. But I'll be keeping an eye on you lot.", null, delegate
			{
				MobileParty party = MobileParty.ConversationParty;
				Campaign.Current.ConversationManager.ConversationEndOneShot += delegate()
				{
					this.conversation_bandits_join_player_party_on_consequence(party);
				};
			}, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_bandit_join_player_declined_1", "bandits_we_can_join_you", "player_do_not_let_bandits_to_join", "{=JZvywHNy}You think I'm daft? I'm not trusting you an inch.", null, null, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_bandit_join_player_declined_2", "bandits_we_can_join_you", "player_do_not_let_bandits_to_join", "{=z0WacPaW}No, justice demands you pay for your crimes.", null, new ConversationSentence.OnConsequenceDelegate(this.conversation_bandit_set_hostile_on_consequence), 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_bandit_join_player_leave", "bandits_we_can_join_you", "bandit_attacker_leave", "{=D33fIGQe}Never mind.", null, null, 100, null, null);
			campaignGameSystemStarter.AddDialogLine("common_encounter_declined_looters_to_join_war_surrender", "player_do_not_let_bandits_to_join", "close_window", "{=ji2eenPE}All right - we give up. We can't fight you. Maybe the likes of us don't deserve mercy, but... show what mercy you can.", new ConversationSentence.OnConditionDelegate(this.conversation_bandits_surrender_on_condition), delegate()
			{
				MobileParty party = MobileParty.ConversationParty;
				Campaign.Current.ConversationManager.ConversationEndOneShot += delegate()
				{
					this.conversation_bandits_surrender_on_consequence(party);
				};
			}, 100, null);
			campaignGameSystemStarter.AddDialogLine("common_encounter_ultimatum_war_2", "player_do_not_let_bandits_to_join", "close_window", "{=LDhU5urT}So that's how it is, is it? Right then - I'll make one of you bleed before I go down.[if:convo_angry][ib:aggressive]", null, null, 100, null);
			campaignGameSystemStarter.AddDialogLine("bandit_attacker_try_leave_success", "bandit_attacker_leave", "close_window", "{=IDdyHef9}We'll be on our way, then!", new ConversationSentence.OnConditionDelegate(this.bandit_attacker_try_leave_condition), delegate()
			{
				PlayerEncounter.LeaveEncounter = true;
			}, 100, null);
			campaignGameSystemStarter.AddDialogLine("bandit_attacker_try_leave_fail", "bandit_attacker_leave", "bandit_defender", "{=6Wc1XErN}Wait, wait... You're not going anywhere just yet.", () => !this.bandit_attacker_try_leave_condition(), null, 100, null);
			campaignGameSystemStarter.AddPlayerLine("common_encounter_cheat1", "bandit_attacker", "close_window", "{=4Wvdk30M}Cheat: Follow me", new ConversationSentence.OnConditionDelegate(this.bandit_cheat_conversations_condition), delegate
			{
				PlayerEncounter.EncounteredMobileParty.Ai.SetMoveEscortParty(MobileParty.MainParty);
				PlayerEncounter.EncounteredMobileParty.Ai.SetInitiative(0f, 0f, 48f);
				PlayerEncounter.LeaveEncounter = true;
			}, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_encounter_cheat2", "bandit_attacker", "close_window", "{=ORj5F5il}Cheat: Besiege Town", new ConversationSentence.OnConditionDelegate(this.bandit_cheat_conversations_condition), delegate
			{
				PlayerEncounter.EncounteredMobileParty.Ai.SetMoveBesiegeSettlement(Settlement.FindFirst((Settlement s) => s.IsTown && (double)s.Position2D.Distance(PlayerEncounter.EncounteredMobileParty.Position2D) < 80.0));
				PlayerEncounter.EncounteredMobileParty.Ai.SetInitiative(0f, 0f, 48f);
				PlayerEncounter.LeaveEncounter = true;
			}, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_encounter_cheat3", "bandit_attacker", "close_window", "{=RxuM5RzJ}Cheat: Raid Nearby Village", new ConversationSentence.OnConditionDelegate(this.bandit_cheat_conversations_condition), delegate
			{
				PlayerEncounter.EncounteredMobileParty.Ai.SetMoveRaidSettlement(Settlement.FindFirst((Settlement s) => s.IsVillage && (double)s.Position2D.Distance(PlayerEncounter.EncounteredMobileParty.Position2D) < 50.0));
				PlayerEncounter.LeaveEncounter = true;
			}, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_encounter_cheat4", "bandit_attacker", "close_window", "{=DIfTkzJJ}Cheat: Besiege Nearby Town", new ConversationSentence.OnConditionDelegate(this.bandit_cheat_conversations_condition), delegate
			{
				PlayerEncounter.EncounteredMobileParty.Ai.SetMoveBesiegeSettlement(Settlement.FindFirst((Settlement s) => s.IsTown && (double)s.Position2D.Distance(PlayerEncounter.EncounteredMobileParty.Position2D) < 50.0));
				PlayerEncounter.LeaveEncounter = true;
				PlayerEncounter.EncounteredMobileParty.Ai.SetInitiative(0f, 0f, 72f);
			}, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("common_encounter_cheat5", "bandit_attacker", "close_window", "{=eXaiRXF9}Cheat: Besiege Nearby Castle", new ConversationSentence.OnConditionDelegate(this.bandit_cheat_conversations_condition), delegate
			{
				PlayerEncounter.EncounteredMobileParty.Ai.SetMoveBesiegeSettlement(Settlement.FindFirst((Settlement s) => s.IsCastle && (double)s.Position2D.Distance(PlayerEncounter.EncounteredMobileParty.Position2D) < 50.0));
				PlayerEncounter.LeaveEncounter = true;
				PlayerEncounter.EncounteredMobileParty.Ai.SetInitiative(0f, 0f, 72f);
			}, 100, null, null);
			campaignGameSystemStarter.AddDialogLine("minor_faction_hostile", "start", "minor_faction_talk_hostile_response", "{=!}{MINOR_FACTION_ENCOUNTER}", new ConversationSentence.OnConditionDelegate(this.conversation_minor_faction_hostile_on_condition), null, 100, null);
			campaignGameSystemStarter.AddPlayerLine("minor_faction_talk_hostile_response_1", "minor_faction_talk_hostile_response", "close_window", "{=aaf5R99a}I'll give you nothing but cold steel, you scum!", null, null, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("minor_faction_talk_hostile_response_2", "minor_faction_talk_hostile_response", "minor_faction_talk_background", "{=EVLzPv1t}Hold - tell me more about yourselves.", null, null, 100, null, null);
			campaignGameSystemStarter.AddDialogLine("minor_faction_talk_background", "minor_faction_talk_background", "minor_faction_talk_background_next", "{=!}{MINOR_FACTION_SELFDESCRIPTION}", new ConversationSentence.OnConditionDelegate(this.conversation_minor_faction_set_selfdescription), null, 100, null);
			campaignGameSystemStarter.AddPlayerLine("minor_faction_talk_background_next_1", "minor_faction_talk_background_next", "minor_faction_talk_how_to_befriend", "{=vEsmC6M6}Is there any way we could not be enemies?", null, null, 100, null, null);
			campaignGameSystemStarter.AddPlayerLine("minor_faction_talk_background_next_2", "minor_faction_talk_background_next", "close_window", "{=p2WPU1CU}Very good then. Now I know whom I slay.", null, null, 100, null, null);
			campaignGameSystemStarter.AddDialogLine("minor_faction_talk_how_to_befriend", "minor_faction_talk_how_to_befriend", "minor_faction_talk_background_repeat_threat", "{=!}{MINOR_FACTION_HOWTOBEFRIEND}", new ConversationSentence.OnConditionDelegate(this.conversation_minor_faction_set_how_to_befriend), null, 100, null);
			campaignGameSystemStarter.AddDialogLine("minor_faction_talk_background_repeat_threat", "minor_faction_talk_background_repeat_threat", "minor_faction_talk_hostile_response", "{=ByOYHslS}That's enough talking for now. Make your choice.[if:convo_angry][[ib:aggressive]", null, null, 100, null);
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x000DA7AC File Offset: 0x000D89AC
		private bool bandit_barter_successful_condition()
		{
			return Campaign.Current.BarterManager.LastBarterIsAccepted;
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x000DA7BD File Offset: 0x000D89BD
		private bool bandit_cheat_conversations_condition()
		{
			return Game.Current.IsDevelopmentMode;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x000DA7CC File Offset: 0x000D89CC
		private bool conversation_bandits_will_join_player_on_condition()
		{
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.PartnersInCrime))
			{
				return true;
			}
			int num = PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.06f) ? 33 : 67;
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num = MathF.Round((float)num * (1f + DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return MobileParty.ConversationParty.Party.RandomIntWithSeed(3U, 100) <= 100 - num && PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.09f);
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x000DA864 File Offset: 0x000D8A64
		private bool conversation_bandits_surrender_on_condition()
		{
			int num = PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.04f) ? 33 : 67;
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Roguery.Scarface))
			{
				num = MathF.Round((float)num * (1f + DefaultPerks.Roguery.Scarface.PrimaryBonus));
			}
			return MobileParty.ConversationParty.Party.RandomIntWithSeed(4U, 100) <= 100 - num && PartyBaseHelper.DoesSurrenderIsLogicalForParty(MobileParty.ConversationParty, MobileParty.MainParty, 0.06f);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x000DA8E8 File Offset: 0x000D8AE8
		private bool bandit_neutral_greet_on_condition()
		{
			if (Campaign.Current.CurrentConversationContext == ConversationContext.PartyEncounter && PlayerEncounter.Current != null && PlayerEncounter.EncounteredMobileParty != null && PlayerEncounter.EncounteredMobileParty.MapFaction.IsBanditFaction && PlayerEncounter.PlayerIsAttacker && MobileParty.ConversationParty != null)
			{
				MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=ZPj0ZAO7}Yeah? What do you want with us?", false);
				MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=5zUIQtTa}I want you to surrender or die, brigand!", false);
				int num = MBRandom.RandomInt(8);
				BanditsCampaignBehavior.PlayerInteraction playerInteraction = this.GetPlayerInteraction(MobileParty.ConversationParty);
				if (playerInteraction == BanditsCampaignBehavior.PlayerInteraction.PaidOffParty)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=Bm7U7TgG}If you're going to keep pestering us, traveller, we might need to take a bit more coin from you.", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=KRfcro26}We're here to fight. Surrender or die!", false);
				}
				else if (playerInteraction != BanditsCampaignBehavior.PlayerInteraction.None)
				{
					if (PlayerEncounter.PlayerIsAttacker)
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=38DvG2ba}Yeah? What is it now?", false);
					}
					else
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=5laJ37D8}Back for more, are you?", false);
					}
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=KRfcro26}We're here to fight. Surrender or die!", false);
				}
				else if (num == 1)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=cO61R3va}We've got no quarrel with you.", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=oJ6lpXmp}But I have one with you, brigand! Give up now.", false);
				}
				else if (num == 2)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=6XdHP9Pv}We're not looking for a fight.", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=fiLWg11t}Neither am I, if you surrender. Otherwise...", false);
				}
				else if (num == 3)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=GUiT211X}You got a problem?", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=idwOxnX5}Not if you give up now. If not, prepare to fight!", false);
				}
				else if (num == 4)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=mHBHKacJ}We're just harmless travellers...", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=A5IJmN0X}I think not, brigand. Surrender or die!", false);
					if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "mountain_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=8rgH8CGc}We're just harmless shepherds...", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "forest_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=kRASveAC}We're just harmless foresters...", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "sea_raiders")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=k96R57KM}We're just harmless traders...", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "steppe_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=odzS6rhH}We're just harmless herdsmen...", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "desert_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=Vttb0P15}We're just harmless nomads...", false);
					}
				}
				else if (num == 5)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=wSwzyr6M}Mess with us and we'll sell our lives dearly.", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=GLqb67cg}I don't care, brigand. Surrender or die!", false);
				}
				else if (num == 6)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=xQ0aBavD}Back off, stranger, unless you want trouble.", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=BwIT8F0k}I don't mind, brigand. Surrender or die!", false);
				}
				else if (num == 7)
				{
					MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=8yPqbZmm}You best back off. There's dozens more of us hiding, just waiting for our signal.", false);
					MBTextManager.SetTextVariable("BANDIT_ULTIMATUM", "{=ASRpFaGF}Nice try, brigand. Surrender or die!", false);
					if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "mountain_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=TXzZwb7n}You best back off. Scores of our brothers are just over that ridge over there, waiting for our signal.", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "forest_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=lZj61xTm}You don't know who you're messing with. There are scores of our brothers hiding in the woods, just waiting for our signal to pepper you with arrows.", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "sea_raiders")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=7Sp6aNYo}You best let us be. There's dozens more of us hiding here, just waiting for our signal.", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "steppe_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=EUbdov2r}Back off, stranger. There's dozens more of us hiding in that gully over there, just waiting for our signal.", false);
					}
					else if (PlayerEncounter.EncounteredMobileParty.MapFaction.StringId == "desert_bandits")
					{
						MBTextManager.SetTextVariable("BANDIT_NEUTRAL_GREETING", "{=RWxYalkR}Be warned, stranger. There's dozens more of us hiding in that wadi over there, just waiting for our signal.", false);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x000DACD4 File Offset: 0x000D8ED4
		private void bandit_barter_successful_on_consequence()
		{
			this.SetPlayerInteraction(MobileParty.ConversationParty, BanditsCampaignBehavior.PlayerInteraction.PaidOffParty);
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x000DACE2 File Offset: 0x000D8EE2
		private void bandit_neutral_greet_on_consequence()
		{
			if (this.GetPlayerInteraction(MobileParty.ConversationParty) != BanditsCampaignBehavior.PlayerInteraction.PaidOffParty)
			{
				this.SetPlayerInteraction(MobileParty.ConversationParty, BanditsCampaignBehavior.PlayerInteraction.Friendly);
			}
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x000DACFE File Offset: 0x000D8EFE
		private void conversation_bandit_set_hostile_on_consequence()
		{
			this.SetPlayerInteraction(MobileParty.ConversationParty, BanditsCampaignBehavior.PlayerInteraction.Hostile);
		}

		// Token: 0x06003437 RID: 13367 RVA: 0x000DAD0C File Offset: 0x000D8F0C
		private void GetMemberAndPrisonerRostersFromParties(List<MobileParty> parties, ref TroopRoster troopsTakenAsMember, ref TroopRoster troopsTakenAsPrisoner, bool doBanditsJoinPlayerSide)
		{
			foreach (MobileParty mobileParty in parties)
			{
				for (int i = 0; i < mobileParty.MemberRoster.Count; i++)
				{
					if (!mobileParty.MemberRoster.GetCharacterAtIndex(i).IsHero)
					{
						if (doBanditsJoinPlayerSide)
						{
							troopsTakenAsMember.AddToCounts(mobileParty.MemberRoster.GetCharacterAtIndex(i), mobileParty.MemberRoster.GetElementNumber(i), false, 0, 0, true, -1);
						}
						else
						{
							troopsTakenAsPrisoner.AddToCounts(mobileParty.MemberRoster.GetCharacterAtIndex(i), mobileParty.MemberRoster.GetElementNumber(i), false, 0, 0, true, -1);
						}
					}
				}
				for (int j = mobileParty.PrisonRoster.Count - 1; j > -1; j--)
				{
					CharacterObject characterAtIndex = mobileParty.PrisonRoster.GetCharacterAtIndex(j);
					if (!characterAtIndex.IsHero)
					{
						troopsTakenAsMember.AddToCounts(mobileParty.PrisonRoster.GetCharacterAtIndex(j), mobileParty.PrisonRoster.GetElementNumber(j), false, 0, 0, true, -1);
					}
					else if (characterAtIndex.HeroObject.Clan == Clan.PlayerClan)
					{
						if (doBanditsJoinPlayerSide)
						{
							EndCaptivityAction.ApplyByPeace(characterAtIndex.HeroObject, null);
						}
						else
						{
							EndCaptivityAction.ApplyByReleasedAfterBattle(characterAtIndex.HeroObject);
						}
						characterAtIndex.HeroObject.ChangeState(Hero.CharacterStates.Active);
						AddHeroToPartyAction.Apply(characterAtIndex.HeroObject, MobileParty.MainParty, true);
					}
					else if (Clan.PlayerClan.IsAtWarWith(characterAtIndex.HeroObject.Clan))
					{
						TransferPrisonerAction.Apply(characterAtIndex, mobileParty.Party, PartyBase.MainParty);
					}
				}
			}
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000DAEB8 File Offset: 0x000D90B8
		private void OpenRosterScreenAfterBanditEncounter(MobileParty conversationParty, bool doBanditsJoinPlayerSide)
		{
			List<MobileParty> list = new List<MobileParty>
			{
				MobileParty.MainParty
			};
			List<MobileParty> list2 = new List<MobileParty>();
			if (PlayerEncounter.EncounteredMobileParty != null)
			{
				list2.Add(PlayerEncounter.EncounteredMobileParty);
			}
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(ref list, ref list2);
			}
			TroopRoster prisonerRosterLeft = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			this.GetMemberAndPrisonerRostersFromParties(list2, ref troopRoster, ref prisonerRosterLeft, doBanditsJoinPlayerSide);
			if (!doBanditsJoinPlayerSide)
			{
				Dictionary<PartyBase, ItemRoster> dictionary = new Dictionary<PartyBase, ItemRoster>();
				ItemRoster itemRoster = new ItemRoster();
				int num = 0;
				foreach (MobileParty mobileParty in list2)
				{
					num += mobileParty.PartyTradeGold;
					itemRoster.Add(mobileParty.ItemRoster);
				}
				GiveGoldAction.ApplyForPartyToCharacter(conversationParty.Party, Hero.MainHero, num, false);
				dictionary.Add(PartyBase.MainParty, itemRoster);
				if (itemRoster.Count > 0)
				{
					InventoryManager.OpenScreenAsLoot(dictionary);
					for (int i = 0; i < list2.Count - 1; i++)
					{
						list2[i].ItemRoster.Clear();
					}
				}
				PartyScreenManager.OpenScreenWithCondition(new IsTroopTransferableDelegate(this.IsTroopTransferable), new PartyPresentationDoneButtonConditionDelegate(this.DoneButtonCondition), new PartyPresentationDoneButtonDelegate(this.OnDoneClicked), null, PartyScreenLogic.TransferState.Transferable, PartyScreenLogic.TransferState.Transferable, PlayerEncounter.EncounteredParty.Name, troopRoster.TotalManCount, false, false, PartyScreenMode.Loot, troopRoster, prisonerRosterLeft);
				for (int j = list2.Count - 1; j >= 0; j--)
				{
					MobileParty destroyedParty = list2[j];
					DestroyPartyAction.Apply(MobileParty.MainParty.Party, destroyedParty);
				}
				return;
			}
			PartyScreenManager.OpenScreenWithCondition(new IsTroopTransferableDelegate(this.IsTroopTransferable), new PartyPresentationDoneButtonConditionDelegate(this.DoneButtonCondition), new PartyPresentationDoneButtonDelegate(this.OnDoneClicked), null, PartyScreenLogic.TransferState.Transferable, PartyScreenLogic.TransferState.Transferable, PlayerEncounter.EncounteredParty.Name, troopRoster.TotalManCount, false, false, PartyScreenMode.TroopsManage, troopRoster, null);
			for (int k = list2.Count - 1; k >= 0; k--)
			{
				MobileParty mobileParty2 = list2[k];
				CampaignEventDispatcher.Instance.OnBanditPartyRecruited(mobileParty2);
				DestroyPartyAction.Apply(MobileParty.MainParty.Party, mobileParty2);
			}
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x000DB0D8 File Offset: 0x000D92D8
		private void conversation_bandits_surrender_on_consequence(MobileParty conversationParty)
		{
			this.OpenRosterScreenAfterBanditEncounter(conversationParty, false);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x000DB0E8 File Offset: 0x000D92E8
		private void conversation_bandits_join_player_party_on_consequence(MobileParty conversationParty)
		{
			this.OpenRosterScreenAfterBanditEncounter(conversationParty, true);
			PlayerEncounter.LeaveEncounter = true;
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x000DB0F8 File Offset: 0x000D92F8
		private bool OnDoneClicked(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty, PartyBase rightParty)
		{
			return true;
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x000DB0FC File Offset: 0x000D92FC
		private Tuple<bool, TextObject> DoneButtonCondition(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			foreach (TroopRosterElement troopRosterElement in rightMemberRoster.GetTroopRoster())
			{
				if (troopRosterElement.Character.IsHero && troopRosterElement.Character.HeroObject.HeroState == Hero.CharacterStates.Fugitive)
				{
					troopRosterElement.Character.HeroObject.ChangeState(Hero.CharacterStates.Active);
				}
			}
			return new Tuple<bool, TextObject>(true, null);
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000DB180 File Offset: 0x000D9380
		private bool IsTroopTransferable(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return true;
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x000DB184 File Offset: 0x000D9384
		private bool bandit_start_defender_condition()
		{
			PartyBase encounteredParty = PlayerEncounter.EncounteredParty;
			if ((Hero.OneToOneConversationHero != null && Hero.OneToOneConversationHero.MapFaction != null && !Hero.OneToOneConversationHero.MapFaction.IsBanditFaction) || encounteredParty == null || !encounteredParty.IsMobile || !encounteredParty.MapFaction.IsBanditFaction)
			{
				return false;
			}
			List<TextObject> list = new List<TextObject>();
			List<TextObject> list2 = new List<TextObject>();
			List<TextObject> list3 = new List<TextObject>();
			List<TextObject> list4 = new List<TextObject>();
			for (int i = 1; i <= 6; i++)
			{
				TextObject item;
				if (GameTexts.TryGetText("str_robbery_threat", out item, i.ToString()))
				{
					list.Add(item);
					list2.Add(GameTexts.FindText("str_robbery_pay_agreement", i.ToString()));
					list3.Add(GameTexts.FindText("str_robbery_conclusion", i.ToString()));
					list4.Add(GameTexts.FindText("str_robbery_start_fight", i.ToString()));
				}
			}
			for (int j = 1; j <= 6; j++)
			{
				string variation = encounteredParty.MapFaction.StringId + "_" + j.ToString();
				TextObject item2;
				if (GameTexts.TryGetText("str_robbery_threat", out item2, variation))
				{
					for (int k = 0; k < 3; k++)
					{
						list.Add(item2);
						list2.Add(GameTexts.FindText("str_robbery_pay_agreement", variation));
						list3.Add(GameTexts.FindText("str_robbery_conclusion", variation));
						list4.Add(GameTexts.FindText("str_robbery_start_fight", variation));
					}
				}
			}
			int index = MBRandom.RandomInt(0, list.Count);
			MBTextManager.SetTextVariable("ROBBERY_THREAT", list[index], false);
			MBTextManager.SetTextVariable("ROBBERY_PAY_AGREEMENT", list2[index], false);
			MBTextManager.SetTextVariable("ROBBERY_CONCLUSION", list3[index], false);
			MBTextManager.SetTextVariable("ROBBERY_START_FIGHT", list4[index], false);
			List<MobileParty> list5 = new List<MobileParty>
			{
				MobileParty.MainParty
			};
			List<MobileParty> list6 = new List<MobileParty>();
			if (MobileParty.ConversationParty != null)
			{
				list6.Add(MobileParty.ConversationParty);
			}
			if (PlayerEncounter.Current != null)
			{
				PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(ref list5, ref list6);
			}
			float num = 0f;
			foreach (MobileParty mobileParty in list5)
			{
				num += mobileParty.Party.TotalStrength;
			}
			float num2 = 0f;
			foreach (MobileParty mobileParty2 in list6)
			{
				num2 += mobileParty2.Party.TotalStrength;
			}
			float num3 = (num2 + 1f) / (num + 1f);
			int num4 = Hero.MainHero.Gold / 100;
			double num5 = 2.0 * (double)MathF.Max(0f, MathF.Min(6f, num3 - 1f));
			float num6 = 0f;
			Settlement settlement = SettlementHelper.FindNearestSettlement((Settlement x) => x.IsTown || x.IsVillage, null);
			SettlementComponent settlementComponent;
			if (settlement.IsTown)
			{
				settlementComponent = settlement.Town;
			}
			else
			{
				settlementComponent = settlement.Village;
			}
			foreach (ItemRosterElement itemRosterElement in MobileParty.MainParty.ItemRoster)
			{
				num6 += (float)(settlementComponent.GetItemPrice(itemRosterElement.EquipmentElement, MobileParty.MainParty, true) * itemRosterElement.Amount);
			}
			float num7 = num6 / 100f;
			float num8 = 1f + 2f * MathF.Max(0f, MathF.Min(6f, num3 - 1f));
			BanditsCampaignBehavior._goldAmount = (int)((double)num4 * num5 + (double)(num7 * num8) + 100.0);
			MBTextManager.SetTextVariable("AMOUNT", BanditsCampaignBehavior._goldAmount.ToString(), false);
			return encounteredParty.IsMobile && encounteredParty.MapFaction.IsBanditFaction && PlayerEncounter.PlayerIsDefender;
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x000DB5B0 File Offset: 0x000D97B0
		private bool bandit_start_barter_condition()
		{
			return PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x000DB5C8 File Offset: 0x000D97C8
		private void bandit_start_barter_consequence()
		{
			BarterManager instance = BarterManager.Instance;
			Hero mainHero = Hero.MainHero;
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			PartyBase mainParty = PartyBase.MainParty;
			MobileParty conversationParty = MobileParty.ConversationParty;
			PartyBase otherParty = (conversationParty != null) ? conversationParty.Party : null;
			Hero beneficiaryOfOtherHero = null;
			BarterManager.BarterContextInitializer initContext = new BarterManager.BarterContextInitializer(BarterManager.Instance.InitializeSafePassageBarterContext);
			int persuasionCostReduction = 0;
			bool isAIBarter = false;
			Barterable[] array = new Barterable[1];
			int num = 0;
			Hero originalOwner = null;
			Hero mainHero2 = Hero.MainHero;
			MobileParty conversationParty2 = MobileParty.ConversationParty;
			array[num] = new SafePassageBarterable(originalOwner, mainHero2, (conversationParty2 != null) ? conversationParty2.Party : null, PartyBase.MainParty);
			instance.StartBarterOffer(mainHero, oneToOneConversationHero, mainParty, otherParty, beneficiaryOfOtherHero, initContext, persuasionCostReduction, isAIBarter, array);
		}

		// Token: 0x06003441 RID: 13377 RVA: 0x000DB63C File Offset: 0x000D983C
		private bool conversation_minor_faction_hostile_on_condition()
		{
			if (MapEvent.PlayerMapEvent != null)
			{
				foreach (PartyBase partyBase in MapEvent.PlayerMapEvent.InvolvedParties)
				{
					if (PartyBase.MainParty.Side == BattleSideEnum.Attacker && partyBase.IsMobile && partyBase.MobileParty.IsBandit && partyBase.MapFaction.IsMinorFaction)
					{
						string text = partyBase.MapFaction.StringId + "_encounter";
						if (FactionManager.IsAtWarAgainstFaction(partyBase.MapFaction, Hero.MainHero.MapFaction))
						{
							text += "_hostile";
						}
						else
						{
							text += "_neutral";
						}
						MBTextManager.SetTextVariable("MINOR_FACTION_ENCOUNTER", GameTexts.FindText(text, null), false);
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06003442 RID: 13378 RVA: 0x000DB728 File Offset: 0x000D9928
		private bool conversation_minor_faction_set_selfdescription()
		{
			foreach (PartyBase partyBase in MapEvent.PlayerMapEvent.InvolvedParties)
			{
				if (PartyBase.MainParty.Side == BattleSideEnum.Attacker && partyBase.IsMobile && partyBase.MobileParty.IsBandit && partyBase.MapFaction.IsMinorFaction)
				{
					string id = partyBase.MapFaction.StringId + "_selfdescription";
					MBTextManager.SetTextVariable("MINOR_FACTION_SELFDESCRIPTION", GameTexts.FindText(id, null), false);
					return true;
				}
			}
			return true;
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x000DB7D0 File Offset: 0x000D99D0
		private bool conversation_minor_faction_set_how_to_befriend()
		{
			foreach (PartyBase partyBase in MapEvent.PlayerMapEvent.InvolvedParties)
			{
				if (PartyBase.MainParty.Side == BattleSideEnum.Attacker && partyBase.IsMobile && partyBase.MobileParty.IsBandit && partyBase.MapFaction.IsMinorFaction)
				{
					string id = partyBase.MapFaction.StringId + "_how_to_befriend";
					MBTextManager.SetTextVariable("MINOR_FACTION_HOWTOBEFRIEND", GameTexts.FindText(id, null), false);
					return true;
				}
			}
			return true;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x000DB878 File Offset: 0x000D9A78
		private bool bandit_attacker_try_leave_condition()
		{
			return PlayerEncounter.EncounteredParty != null && (PlayerEncounter.EncounteredParty.TotalStrength <= PartyBase.MainParty.TotalStrength || this.GetPlayerInteraction(PlayerEncounter.EncounteredMobileParty) == BanditsCampaignBehavior.PlayerInteraction.PaidOffParty || this.GetPlayerInteraction(PlayerEncounter.EncounteredMobileParty) == BanditsCampaignBehavior.PlayerInteraction.Friendly);
		}

		// Token: 0x040010F2 RID: 4338
		private const float BanditSpawnRadius = 45f;

		// Token: 0x040010F3 RID: 4339
		private const float BanditStartGoldPerBandit = 10f;

		// Token: 0x040010F4 RID: 4340
		private const float BanditLongTermGoldPerBandit = 50f;

		// Token: 0x040010F5 RID: 4341
		private const int HideoutInfestCooldownAfterFightAsHours = 36;

		// Token: 0x040010F6 RID: 4342
		private bool _hideoutsAndBanditsAreInitialized;

		// Token: 0x040010F7 RID: 4343
		private Dictionary<MobileParty, BanditsCampaignBehavior.PlayerInteraction> _interactedBandits = new Dictionary<MobileParty, BanditsCampaignBehavior.PlayerInteraction>();

		// Token: 0x040010F8 RID: 4344
		private static int _goldAmount;

		// Token: 0x020006BA RID: 1722
		public class BanditsCampaignBehaviorTypeDefiner : SaveableTypeDefiner
		{
			// Token: 0x060056B6 RID: 22198 RVA: 0x0017FBE8 File Offset: 0x0017DDE8
			public BanditsCampaignBehaviorTypeDefiner() : base(70000)
			{
			}

			// Token: 0x060056B7 RID: 22199 RVA: 0x0017FBF5 File Offset: 0x0017DDF5
			protected override void DefineEnumTypes()
			{
				base.AddEnumDefinition(typeof(BanditsCampaignBehavior.PlayerInteraction), 1, null);
			}

			// Token: 0x060056B8 RID: 22200 RVA: 0x0017FC09 File Offset: 0x0017DE09
			protected override void DefineContainerDefinitions()
			{
				base.ConstructContainerDefinition(typeof(Dictionary<MobileParty, BanditsCampaignBehavior.PlayerInteraction>));
			}
		}

		// Token: 0x020006BB RID: 1723
		private enum PlayerInteraction
		{
			// Token: 0x04001BE6 RID: 7142
			None,
			// Token: 0x04001BE7 RID: 7143
			Friendly,
			// Token: 0x04001BE8 RID: 7144
			PaidOffParty,
			// Token: 0x04001BE9 RID: 7145
			Hostile
		}
	}
}
