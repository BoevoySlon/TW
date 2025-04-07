using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000052 RID: 82
	public class CampaignObjectManager
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00022EDC File Offset: 0x000210DC
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x00022EE4 File Offset: 0x000210E4
		[SaveableProperty(80)]
		public MBReadOnlyList<Settlement> Settlements { get; private set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x00022EED File Offset: 0x000210ED
		public MBReadOnlyList<MobileParty> MobileParties
		{
			get
			{
				return this._mobileParties;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00022EF5 File Offset: 0x000210F5
		public MBReadOnlyList<MobileParty> CaravanParties
		{
			get
			{
				return this._caravanParties;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00022EFD File Offset: 0x000210FD
		public MBReadOnlyList<MobileParty> MilitiaParties
		{
			get
			{
				return this._militiaParties;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00022F05 File Offset: 0x00021105
		public MBReadOnlyList<MobileParty> GarrisonParties
		{
			get
			{
				return this._garrisonParties;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00022F0D File Offset: 0x0002110D
		public MBReadOnlyList<MobileParty> BanditParties
		{
			get
			{
				return this._banditParties;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00022F15 File Offset: 0x00021115
		public MBReadOnlyList<MobileParty> VillagerParties
		{
			get
			{
				return this._villagerParties;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x00022F1D File Offset: 0x0002111D
		public MBReadOnlyList<MobileParty> LordParties
		{
			get
			{
				return this._lordParties;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00022F25 File Offset: 0x00021125
		public MBReadOnlyList<MobileParty> CustomParties
		{
			get
			{
				return this._customParties;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00022F2D File Offset: 0x0002112D
		public MBReadOnlyList<MobileParty> PartiesWithoutPartyComponent
		{
			get
			{
				return this._partiesWithoutPartyComponent;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x00022F35 File Offset: 0x00021135
		public MBReadOnlyList<Hero> AliveHeroes
		{
			get
			{
				return this._aliveHeroes;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00022F3D File Offset: 0x0002113D
		public MBReadOnlyList<Hero> DeadOrDisabledHeroes
		{
			get
			{
				return this._deadOrDisabledHeroes;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00022F45 File Offset: 0x00021145
		public MBReadOnlyList<Clan> Clans
		{
			get
			{
				return this._clans;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x00022F4D File Offset: 0x0002114D
		public MBReadOnlyList<Kingdom> Kingdoms
		{
			get
			{
				return this._kingdoms;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00022F55 File Offset: 0x00021155
		public MBReadOnlyList<IFaction> Factions
		{
			get
			{
				return this._factions;
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00022F60 File Offset: 0x00021160
		public CampaignObjectManager()
		{
			this._objects = new CampaignObjectManager.ICampaignObjectType[5];
			this._mobileParties = new MBList<MobileParty>();
			this._caravanParties = new MBList<MobileParty>();
			this._militiaParties = new MBList<MobileParty>();
			this._garrisonParties = new MBList<MobileParty>();
			this._customParties = new MBList<MobileParty>();
			this._banditParties = new MBList<MobileParty>();
			this._villagerParties = new MBList<MobileParty>();
			this._lordParties = new MBList<MobileParty>();
			this._partiesWithoutPartyComponent = new MBList<MobileParty>();
			this._deadOrDisabledHeroes = new MBList<Hero>();
			this._aliveHeroes = new MBList<Hero>();
			this._clans = new MBList<Clan>();
			this._kingdoms = new MBList<Kingdom>();
			this._factions = new MBList<IFaction>();
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002301C File Offset: 0x0002121C
		private void InitializeManagerObjectLists()
		{
			this._objects[4] = new CampaignObjectManager.CampaignObjectType<MobileParty>(this._mobileParties);
			this._objects[0] = new CampaignObjectManager.CampaignObjectType<Hero>(this._deadOrDisabledHeroes);
			this._objects[1] = new CampaignObjectManager.CampaignObjectType<Hero>(this._aliveHeroes);
			this._objects[2] = new CampaignObjectManager.CampaignObjectType<Clan>(this._clans);
			this._objects[3] = new CampaignObjectManager.CampaignObjectType<Kingdom>(this._kingdoms);
			this._objectTypesAndNextIds = new Dictionary<Type, uint>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				uint maxObjectSubId = campaignObjectType.GetMaxObjectSubId();
				uint num;
				if (this._objectTypesAndNextIds.TryGetValue(campaignObjectType.ObjectClass, out num))
				{
					if (num <= maxObjectSubId)
					{
						this._objectTypesAndNextIds[campaignObjectType.ObjectClass] = maxObjectSubId + 1U;
					}
				}
				else
				{
					this._objectTypesAndNextIds.Add(campaignObjectType.ObjectClass, maxObjectSubId + 1U);
				}
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000230F8 File Offset: 0x000212F8
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData, ObjectLoadData objectLoadData)
		{
			this._objects = new CampaignObjectManager.ICampaignObjectType[5];
			this._factions = new MBList<IFaction>();
			this._caravanParties = new MBList<MobileParty>();
			this._militiaParties = new MBList<MobileParty>();
			this._garrisonParties = new MBList<MobileParty>();
			this._customParties = new MBList<MobileParty>();
			this._banditParties = new MBList<MobileParty>();
			this._villagerParties = new MBList<MobileParty>();
			this._lordParties = new MBList<MobileParty>();
			this._partiesWithoutPartyComponent = new MBList<MobileParty>();
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00023174 File Offset: 0x00021374
		internal void PreAfterLoad()
		{
			CampaignObjectManager.ICampaignObjectType[] objects = this._objects;
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].PreAfterLoad();
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000231A0 File Offset: 0x000213A0
		internal void AfterLoad()
		{
			CampaignObjectManager.ICampaignObjectType[] objects = this._objects;
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].AfterLoad();
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000231CC File Offset: 0x000213CC
		internal void InitializeOnLoad()
		{
			this.Settlements = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			foreach (Clan item in this._clans)
			{
				if (!this._factions.Contains(item))
				{
					this._factions.Add(item);
				}
			}
			foreach (Kingdom item2 in this._kingdoms)
			{
				if (!this._factions.Contains(item2))
				{
					this._factions.Add(item2);
				}
			}
			foreach (MobileParty mobileParty in this._mobileParties)
			{
				mobileParty.UpdatePartyComponentFlags();
				this.AddPartyToAppropriateList(mobileParty);
			}
			this.InitializeManagerObjectLists();
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000232EC File Offset: 0x000214EC
		internal void InitializeOnNewGame()
		{
			List<Hero> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<Hero>();
			MBReadOnlyList<MobileParty> objectTypeList2 = MBObjectManager.Instance.GetObjectTypeList<MobileParty>();
			MBReadOnlyList<Clan> objectTypeList3 = MBObjectManager.Instance.GetObjectTypeList<Clan>();
			MBReadOnlyList<Kingdom> objectTypeList4 = MBObjectManager.Instance.GetObjectTypeList<Kingdom>();
			this.Settlements = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			foreach (Hero hero in objectTypeList)
			{
				if (hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled)
				{
					if (!this._deadOrDisabledHeroes.Contains(hero))
					{
						this._deadOrDisabledHeroes.Add(hero);
					}
				}
				else if (!this._aliveHeroes.Contains(hero))
				{
					this._aliveHeroes.Add(hero);
				}
			}
			foreach (Clan item in objectTypeList3)
			{
				if (!this._clans.Contains(item))
				{
					this._clans.Add(item);
				}
				if (!this._factions.Contains(item))
				{
					this._factions.Add(item);
				}
			}
			foreach (Kingdom item2 in objectTypeList4)
			{
				if (!this._kingdoms.Contains(item2))
				{
					this._kingdoms.Add(item2);
				}
				if (!this._factions.Contains(item2))
				{
					this._factions.Add(item2);
				}
			}
			foreach (MobileParty mobileParty in objectTypeList2)
			{
				this._mobileParties.Add(mobileParty);
				this.AddPartyToAppropriateList(mobileParty);
			}
			this.InitializeManagerObjectLists();
			this.InitializeCachedData();
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000234F8 File Offset: 0x000216F8
		private void InitializeCachedData()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsVillage)
				{
					settlement.OwnerClan.OnBoundVillageAdded(settlement.Village);
				}
			}
			foreach (Clan clan in Clan.All)
			{
				if (clan.Kingdom != null)
				{
					foreach (Hero hero in clan.Heroes)
					{
						clan.Kingdom.OnHeroAdded(hero);
					}
				}
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000235E8 File Offset: 0x000217E8
		internal void AddMobileParty(MobileParty party)
		{
			party.Id = new MBGUID(14U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<MobileParty>());
			this._mobileParties.Add(party);
			this.OnItemAdded<MobileParty>(CampaignObjectManager.CampaignObjects.MobileParty, party);
			this.AddPartyToAppropriateList(party);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00023621 File Offset: 0x00021821
		internal void RemoveMobileParty(MobileParty party)
		{
			this._mobileParties.Remove(party);
			this.OnItemRemoved<MobileParty>(CampaignObjectManager.CampaignObjects.MobileParty, party);
			this.RemovePartyFromAppropriateList(party);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0002363F File Offset: 0x0002183F
		internal void BeforePartyComponentChanged(MobileParty party)
		{
			this.RemovePartyFromAppropriateList(party);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00023648 File Offset: 0x00021848
		internal void AfterPartyComponentChanged(MobileParty party)
		{
			this.AddPartyToAppropriateList(party);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00023651 File Offset: 0x00021851
		internal void AddHero(Hero hero)
		{
			hero.Id = new MBGUID(32U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Hero>());
			this.OnHeroAdded(hero);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00023676 File Offset: 0x00021876
		internal void UnregisterDeadHero(Hero hero)
		{
			this._deadOrDisabledHeroes.Remove(hero);
			this.OnItemRemoved<Hero>(CampaignObjectManager.CampaignObjects.DeadOrDisabledHeroes, hero);
			CampaignEventDispatcher.Instance.OnHeroUnregistered(hero);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00023698 File Offset: 0x00021898
		private void OnHeroAdded(Hero hero)
		{
			if (hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled)
			{
				this._deadOrDisabledHeroes.Add(hero);
				this.OnItemAdded<Hero>(CampaignObjectManager.CampaignObjects.DeadOrDisabledHeroes, hero);
				return;
			}
			this._aliveHeroes.Add(hero);
			this.OnItemAdded<Hero>(CampaignObjectManager.CampaignObjects.AliveHeroes, hero);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x000236D8 File Offset: 0x000218D8
		internal void HeroStateChanged(Hero hero, Hero.CharacterStates oldState)
		{
			bool flag = oldState == Hero.CharacterStates.Dead || oldState == Hero.CharacterStates.Disabled;
			bool flag2 = hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled;
			if (flag != flag2)
			{
				if (flag2)
				{
					if (this._aliveHeroes.Contains(hero))
					{
						this._aliveHeroes.Remove(hero);
					}
				}
				else if (this._deadOrDisabledHeroes.Contains(hero))
				{
					this._deadOrDisabledHeroes.Remove(hero);
				}
				this.OnHeroAdded(hero);
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0002374B File Offset: 0x0002194B
		internal void AddClan(Clan clan)
		{
			clan.Id = new MBGUID(18U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Clan>());
			this._clans.Add(clan);
			this.OnItemAdded<Clan>(CampaignObjectManager.CampaignObjects.Clans, clan);
			this._factions.Add(clan);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00023789 File Offset: 0x00021989
		internal void RemoveClan(Clan clan)
		{
			if (this._clans.Contains(clan))
			{
				this._clans.Remove(clan);
				this.OnItemRemoved<Clan>(CampaignObjectManager.CampaignObjects.Clans, clan);
			}
			if (this._factions.Contains(clan))
			{
				this._factions.Remove(clan);
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x000237C9 File Offset: 0x000219C9
		internal void AddKingdom(Kingdom kingdom)
		{
			kingdom.Id = new MBGUID(20U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Kingdom>());
			this._kingdoms.Add(kingdom);
			this.OnItemAdded<Kingdom>(CampaignObjectManager.CampaignObjects.Kingdoms, kingdom);
			this._factions.Add(kingdom);
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x00023808 File Offset: 0x00021A08
		private void AddPartyToAppropriateList(MobileParty party)
		{
			if (party.IsBandit)
			{
				this._banditParties.Add(party);
				return;
			}
			if (party.IsCaravan)
			{
				this._caravanParties.Add(party);
				return;
			}
			if (party.IsLordParty)
			{
				this._lordParties.Add(party);
				return;
			}
			if (party.IsMilitia)
			{
				this._militiaParties.Add(party);
				return;
			}
			if (party.IsVillager)
			{
				this._villagerParties.Add(party);
				return;
			}
			if (party.IsCustomParty)
			{
				this._customParties.Add(party);
				return;
			}
			if (party.IsGarrison)
			{
				this._garrisonParties.Add(party);
				return;
			}
			this._partiesWithoutPartyComponent.Add(party);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000238B4 File Offset: 0x00021AB4
		private void RemovePartyFromAppropriateList(MobileParty party)
		{
			if (party.IsBandit)
			{
				this._banditParties.Remove(party);
				return;
			}
			if (party.IsCaravan)
			{
				this._caravanParties.Remove(party);
				return;
			}
			if (party.IsLordParty)
			{
				this._lordParties.Remove(party);
				return;
			}
			if (party.IsMilitia)
			{
				this._militiaParties.Remove(party);
				return;
			}
			if (party.IsVillager)
			{
				this._villagerParties.Remove(party);
				return;
			}
			if (party.IsCustomParty)
			{
				this._customParties.Remove(party);
				return;
			}
			if (party.IsGarrison)
			{
				this._garrisonParties.Remove(party);
				return;
			}
			this._partiesWithoutPartyComponent.Remove(party);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00023968 File Offset: 0x00021B68
		private void OnItemAdded<T>(CampaignObjectManager.CampaignObjects targetList, T obj) where T : MBObjectBase
		{
			CampaignObjectManager.CampaignObjectType<T> campaignObjectType = (CampaignObjectManager.CampaignObjectType<T>)this._objects[(int)targetList];
			if (campaignObjectType != null)
			{
				campaignObjectType.OnItemAdded(obj);
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00023990 File Offset: 0x00021B90
		private void OnItemRemoved<T>(CampaignObjectManager.CampaignObjects targetList, T obj) where T : MBObjectBase
		{
			CampaignObjectManager.CampaignObjectType<T> campaignObjectType = (CampaignObjectManager.CampaignObjectType<T>)this._objects[(int)targetList];
			if (campaignObjectType != null)
			{
				campaignObjectType.UnregisterItem(obj);
			}
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x000239B8 File Offset: 0x00021BB8
		public T Find<T>(Predicate<T> predicate) where T : MBObjectBase
		{
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (typeof(T) == campaignObjectType.ObjectClass)
				{
					T t = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).Find(predicate);
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00023A18 File Offset: 0x00021C18
		private uint GetNextUniqueObjectIdOfType<T>() where T : MBObjectBase
		{
			uint num;
			if (this._objectTypesAndNextIds.TryGetValue(typeof(T), out num))
			{
				this._objectTypesAndNextIds[typeof(T)] = num + 1U;
			}
			return num;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00023A58 File Offset: 0x00021C58
		public T Find<T>(string id) where T : MBObjectBase
		{
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (campaignObjectType != null && typeof(T) == campaignObjectType.ObjectClass)
				{
					T t = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).Find(id);
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00023ABC File Offset: 0x00021CBC
		public string FindNextUniqueStringId<T>(string id) where T : MBObjectBase
		{
			List<CampaignObjectManager.CampaignObjectType<T>> list = new List<CampaignObjectManager.CampaignObjectType<T>>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (campaignObjectType != null && typeof(T) == campaignObjectType.ObjectClass)
				{
					list.Add(campaignObjectType as CampaignObjectManager.CampaignObjectType<T>);
				}
			}
			return CampaignObjectManager.CampaignObjectType<T>.FindNextUniqueStringId(list, id);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00023B15 File Offset: 0x00021D15
		internal static void AutoGeneratedStaticCollectObjectsCampaignObjectManager(object o, List<object> collectedObjects)
		{
			((CampaignObjectManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00023B24 File Offset: 0x00021D24
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._deadOrDisabledHeroes);
			collectedObjects.Add(this._aliveHeroes);
			collectedObjects.Add(this._clans);
			collectedObjects.Add(this._kingdoms);
			collectedObjects.Add(this._mobileParties);
			collectedObjects.Add(this.Settlements);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00023B79 File Offset: 0x00021D79
		internal static object AutoGeneratedGetMemberValueSettlements(object o)
		{
			return ((CampaignObjectManager)o).Settlements;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00023B86 File Offset: 0x00021D86
		internal static object AutoGeneratedGetMemberValue_deadOrDisabledHeroes(object o)
		{
			return ((CampaignObjectManager)o)._deadOrDisabledHeroes;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00023B93 File Offset: 0x00021D93
		internal static object AutoGeneratedGetMemberValue_aliveHeroes(object o)
		{
			return ((CampaignObjectManager)o)._aliveHeroes;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00023BA0 File Offset: 0x00021DA0
		internal static object AutoGeneratedGetMemberValue_clans(object o)
		{
			return ((CampaignObjectManager)o)._clans;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00023BAD File Offset: 0x00021DAD
		internal static object AutoGeneratedGetMemberValue_kingdoms(object o)
		{
			return ((CampaignObjectManager)o)._kingdoms;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00023BBA File Offset: 0x00021DBA
		internal static object AutoGeneratedGetMemberValue_mobileParties(object o)
		{
			return ((CampaignObjectManager)o)._mobileParties;
		}

		// Token: 0x04000291 RID: 657
		internal const uint HeroObjectManagerTypeID = 32U;

		// Token: 0x04000292 RID: 658
		internal const uint MobilePartyObjectManagerTypeID = 14U;

		// Token: 0x04000293 RID: 659
		internal const uint ClanObjectManagerTypeID = 18U;

		// Token: 0x04000294 RID: 660
		internal const uint KingdomObjectManagerTypeID = 20U;

		// Token: 0x04000295 RID: 661
		private CampaignObjectManager.ICampaignObjectType[] _objects;

		// Token: 0x04000296 RID: 662
		private Dictionary<Type, uint> _objectTypesAndNextIds;

		// Token: 0x04000297 RID: 663
		[SaveableField(20)]
		private readonly MBList<Hero> _deadOrDisabledHeroes;

		// Token: 0x04000298 RID: 664
		[SaveableField(30)]
		private readonly MBList<Hero> _aliveHeroes;

		// Token: 0x04000299 RID: 665
		[SaveableField(40)]
		private readonly MBList<Clan> _clans;

		// Token: 0x0400029A RID: 666
		[SaveableField(50)]
		private readonly MBList<Kingdom> _kingdoms;

		// Token: 0x0400029B RID: 667
		private MBList<IFaction> _factions;

		// Token: 0x0400029C RID: 668
		[SaveableField(71)]
		private MBList<MobileParty> _mobileParties;

		// Token: 0x0400029D RID: 669
		private MBList<MobileParty> _caravanParties;

		// Token: 0x0400029E RID: 670
		private MBList<MobileParty> _militiaParties;

		// Token: 0x0400029F RID: 671
		private MBList<MobileParty> _garrisonParties;

		// Token: 0x040002A0 RID: 672
		private MBList<MobileParty> _banditParties;

		// Token: 0x040002A1 RID: 673
		private MBList<MobileParty> _villagerParties;

		// Token: 0x040002A2 RID: 674
		private MBList<MobileParty> _customParties;

		// Token: 0x040002A3 RID: 675
		private MBList<MobileParty> _lordParties;

		// Token: 0x040002A4 RID: 676
		private MBList<MobileParty> _partiesWithoutPartyComponent;

		// Token: 0x02000499 RID: 1177
		private interface ICampaignObjectType : IEnumerable
		{
			// Token: 0x17000D7B RID: 3451
			// (get) Token: 0x06004232 RID: 16946
			Type ObjectClass { get; }

			// Token: 0x06004233 RID: 16947
			void PreAfterLoad();

			// Token: 0x06004234 RID: 16948
			void AfterLoad();

			// Token: 0x06004235 RID: 16949
			uint GetMaxObjectSubId();
		}

		// Token: 0x0200049A RID: 1178
		private class CampaignObjectType<T> : CampaignObjectManager.ICampaignObjectType, IEnumerable, IEnumerable<T> where T : MBObjectBase
		{
			// Token: 0x17000D7C RID: 3452
			// (get) Token: 0x06004236 RID: 16950 RVA: 0x00143579 File Offset: 0x00141779
			// (set) Token: 0x06004237 RID: 16951 RVA: 0x00143581 File Offset: 0x00141781
			public uint MaxCreatedPostfixIndex { get; private set; }

			// Token: 0x06004238 RID: 16952 RVA: 0x0014358C File Offset: 0x0014178C
			public CampaignObjectType(IEnumerable<T> registeredObjects)
			{
				this._registeredObjects = registeredObjects;
				foreach (T t in this._registeredObjects)
				{
					ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(t.StringId);
					if (idParts.Item2 > this.MaxCreatedPostfixIndex)
					{
						this.MaxCreatedPostfixIndex = idParts.Item2;
					}
				}
			}

			// Token: 0x17000D7D RID: 3453
			// (get) Token: 0x06004239 RID: 16953 RVA: 0x00143608 File Offset: 0x00141808
			Type CampaignObjectManager.ICampaignObjectType.ObjectClass
			{
				get
				{
					return typeof(T);
				}
			}

			// Token: 0x0600423A RID: 16954 RVA: 0x00143614 File Offset: 0x00141814
			public void PreAfterLoad()
			{
				foreach (T t in this._registeredObjects.ToList<T>())
				{
					t.PreAfterLoadInternal();
				}
			}

			// Token: 0x0600423B RID: 16955 RVA: 0x00143670 File Offset: 0x00141870
			public void AfterLoad()
			{
				foreach (T t in this._registeredObjects.ToList<T>())
				{
					t.IsReady = true;
					t.AfterLoadInternal();
				}
			}

			// Token: 0x0600423C RID: 16956 RVA: 0x001436D8 File Offset: 0x001418D8
			IEnumerator<T> IEnumerable<!0>.GetEnumerator()
			{
				return this._registeredObjects.GetEnumerator();
			}

			// Token: 0x0600423D RID: 16957 RVA: 0x001436E5 File Offset: 0x001418E5
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._registeredObjects.GetEnumerator();
			}

			// Token: 0x0600423E RID: 16958 RVA: 0x001436F4 File Offset: 0x001418F4
			public uint GetMaxObjectSubId()
			{
				uint num = 0U;
				foreach (T t in this._registeredObjects)
				{
					if (t.Id.SubId > num)
					{
						num = t.Id.SubId;
					}
				}
				return num;
			}

			// Token: 0x0600423F RID: 16959 RVA: 0x00143768 File Offset: 0x00141968
			public void OnItemAdded(T item)
			{
				ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(item.StringId);
				if (idParts.Item2 > this.MaxCreatedPostfixIndex)
				{
					this.MaxCreatedPostfixIndex = idParts.Item2;
				}
				this.RegisterItem(item);
			}

			// Token: 0x06004240 RID: 16960 RVA: 0x001437A7 File Offset: 0x001419A7
			private void RegisterItem(T item)
			{
				item.IsReady = true;
			}

			// Token: 0x06004241 RID: 16961 RVA: 0x001437B5 File Offset: 0x001419B5
			public void UnregisterItem(T item)
			{
				item.IsReady = false;
			}

			// Token: 0x06004242 RID: 16962 RVA: 0x001437C4 File Offset: 0x001419C4
			public T Find(string id)
			{
				foreach (T t in this._registeredObjects)
				{
					if (t.StringId == id)
					{
						return t;
					}
				}
				return default(T);
			}

			// Token: 0x06004243 RID: 16963 RVA: 0x0014382C File Offset: 0x00141A2C
			public T Find(Predicate<T> predicate)
			{
				foreach (T t in this._registeredObjects)
				{
					if (predicate(t))
					{
						return t;
					}
				}
				return default(T);
			}

			// Token: 0x06004244 RID: 16964 RVA: 0x0014388C File Offset: 0x00141A8C
			public static string FindNextUniqueStringId(List<CampaignObjectManager.CampaignObjectType<T>> lists, string id)
			{
				if (!CampaignObjectManager.CampaignObjectType<T>.Exist(lists, id))
				{
					return id;
				}
				ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(id);
				string item = idParts.Item1;
				uint num = idParts.Item2;
				num = MathF.Max(num, lists.Max((CampaignObjectManager.CampaignObjectType<T> x) => x.MaxCreatedPostfixIndex));
				num += 1U;
				return item + num;
			}

			// Token: 0x06004245 RID: 16965 RVA: 0x001438F4 File Offset: 0x00141AF4
			[return: TupleElementNames(new string[]
			{
				"str",
				"number"
			})]
			private static ValueTuple<string, uint> GetIdParts(string stringId)
			{
				int num = stringId.Length - 1;
				while (num > 0 && char.IsDigit(stringId[num]))
				{
					num--;
				}
				string item = stringId.Substring(0, num + 1);
				uint item2 = 0U;
				if (num < stringId.Length - 1)
				{
					uint.TryParse(stringId.Substring(num + 1, stringId.Length - num - 1), out item2);
				}
				return new ValueTuple<string, uint>(item, item2);
			}

			// Token: 0x06004246 RID: 16966 RVA: 0x0014395C File Offset: 0x00141B5C
			private static bool Exist(List<CampaignObjectManager.CampaignObjectType<T>> lists, string id)
			{
				using (List<CampaignObjectManager.CampaignObjectType<T>>.Enumerator enumerator = lists.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Find(id) != null)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x040013E7 RID: 5095
			private readonly IEnumerable<T> _registeredObjects;
		}

		// Token: 0x0200049B RID: 1179
		private enum CampaignObjects
		{
			// Token: 0x040013EA RID: 5098
			DeadOrDisabledHeroes,
			// Token: 0x040013EB RID: 5099
			AliveHeroes,
			// Token: 0x040013EC RID: 5100
			Clans,
			// Token: 0x040013ED RID: 5101
			Kingdoms,
			// Token: 0x040013EE RID: 5102
			MobileParty,
			// Token: 0x040013EF RID: 5103
			ObjectCount
		}
	}
}
