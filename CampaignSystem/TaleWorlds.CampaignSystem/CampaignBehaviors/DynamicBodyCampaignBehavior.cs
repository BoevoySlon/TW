using System;
using Helpers;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200038A RID: 906
	public class DynamicBodyCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x060035C0 RID: 13760 RVA: 0x000E9BC8 File Offset: 0x000E7DC8
		// (set) Token: 0x060035C1 RID: 13761 RVA: 0x000E9BE7 File Offset: 0x000E7DE7
		private CampaignTime LastSettlementVisitTime
		{
			get
			{
				if (Hero.MainHero.CurrentSettlement != null)
				{
					this._lastSettlementVisitTime = CampaignTime.Now;
				}
				return this._lastSettlementVisitTime;
			}
			set
			{
				this._lastSettlementVisitTime = value;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x060035C2 RID: 13762 RVA: 0x000E9BF0 File Offset: 0x000E7DF0
		private float MaxPlayerWeight
		{
			get
			{
				return MathF.Min(1f, this._unmodifiedWeight * 1.3f);
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x000E9C08 File Offset: 0x000E7E08
		private float MinPlayerWeight
		{
			get
			{
				return MathF.Max(0f, this._unmodifiedWeight * 0.7f);
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x060035C4 RID: 13764 RVA: 0x000E9C20 File Offset: 0x000E7E20
		private float MaxPlayerBuild
		{
			get
			{
				return MathF.Min(1f, this._unmodifiedBuild * 1.3f);
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x000E9C38 File Offset: 0x000E7E38
		private float MinPlayerBuild
		{
			get
			{
				return MathF.Max(0f, this._unmodifiedBuild * 0.7f);
			}
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x000E9C50 File Offset: 0x000E7E50
		private void DailyTick()
		{
			bool flag = this.LastSettlementVisitTime.ElapsedDaysUntilNow < 1f;
			bool flag2 = Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Party.IsStarving;
			float num = (Hero.MainHero.CurrentSettlement == null && flag2) ? -0.1f : (flag ? 0.025f : -0.025f);
			Hero.MainHero.Weight = MBMath.ClampFloat(Hero.MainHero.Weight + num, this.MinPlayerWeight, this.MaxPlayerWeight);
			float num2 = (MapEvent.PlayerMapEvent != null || PlayerSiege.PlayerSiegeEvent != null || this._lastEncounterTime.ElapsedDaysUntilNow < 2f) ? 0.025f : -0.015f;
			Hero.MainHero.Build = MBMath.ClampFloat(Hero.MainHero.Build + num2, this.MinPlayerBuild, this.MaxPlayerBuild);
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x000E9D40 File Offset: 0x000E7F40
		private void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			if (party != null && party.IsMainParty)
			{
				this.LastSettlementVisitTime = CampaignTime.Now;
			}
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x000E9D58 File Offset: 0x000E7F58
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			if (mapEvent.IsPlayerMapEvent)
			{
				this._lastEncounterTime = CampaignTime.Now;
			}
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x000E9D6D File Offset: 0x000E7F6D
		private void OnPlayerBodyPropertiesChanged()
		{
			this._unmodifiedBuild = Hero.MainHero.Build;
			this._unmodifiedWeight = Hero.MainHero.Weight;
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x000E9D8F File Offset: 0x000E7F8F
		private void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
			this._unmodifiedBuild = newPlayer.Build;
			this._unmodifiedWeight = newPlayer.Weight;
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x000E9DAC File Offset: 0x000E7FAC
		private void OnHeroCreated(Hero hero, bool bornNaturally)
		{
			if (!bornNaturally)
			{
				DynamicBodyProperties dynamicBodyPropertiesBetweenMinMaxRange = CharacterHelper.GetDynamicBodyPropertiesBetweenMinMaxRange(hero.CharacterObject);
				hero.Weight = dynamicBodyPropertiesBetweenMinMaxRange.Weight;
				hero.Build = dynamicBodyPropertiesBetweenMinMaxRange.Build;
			}
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000E9DE0 File Offset: 0x000E7FE0
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			this.OnPlayerBodyPropertiesChanged();
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000E9DE8 File Offset: 0x000E7FE8
		public override void RegisterEvents()
		{
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.OnPlayerBodyPropertiesChangedEvent.AddNonSerializedListener(this, new Action(this.OnPlayerBodyPropertiesChanged));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action(this.OnPlayerBodyPropertiesChanged));
			CampaignEvents.OnPlayerCharacterChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero, MobileParty, bool>(this.OnPlayerCharacterChanged));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000E9EB0 File Offset: 0x000E80B0
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_lastSettlementVisitTime", ref this._lastSettlementVisitTime);
			dataStore.SyncData<CampaignTime>("_lastEncounterTime", ref this._lastEncounterTime);
			dataStore.SyncData<float>("_unmodifiedWeight", ref this._unmodifiedWeight);
			dataStore.SyncData<float>("_unmodifiedBuild", ref this._unmodifiedBuild);
		}

		// Token: 0x04001141 RID: 4417
		private const float DailyBuildDecrease = -0.015f;

		// Token: 0x04001142 RID: 4418
		private const float DailyBuildIncrease = 0.025f;

		// Token: 0x04001143 RID: 4419
		private const float DailyWeightDecreaseWhenStarving = -0.1f;

		// Token: 0x04001144 RID: 4420
		private const float DailyWeightDecreaseWhenNotStarving = -0.025f;

		// Token: 0x04001145 RID: 4421
		private const float DailyWeightIncrease = 0.025f;

		// Token: 0x04001146 RID: 4422
		private CampaignTime _lastSettlementVisitTime = CampaignTime.Now;

		// Token: 0x04001147 RID: 4423
		private CampaignTime _lastEncounterTime = CampaignTime.Now;

		// Token: 0x04001148 RID: 4424
		private float _unmodifiedWeight = -1f;

		// Token: 0x04001149 RID: 4425
		private float _unmodifiedBuild = -1f;
	}
}
