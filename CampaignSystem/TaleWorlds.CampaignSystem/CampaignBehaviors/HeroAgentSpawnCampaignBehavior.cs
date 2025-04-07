using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000393 RID: 915
	public class HeroAgentSpawnCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x000F6EEC File Offset: 0x000F50EC
		private static Location Prison
		{
			get
			{
				return LocationComplex.Current.GetLocationWithId("prison");
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003703 RID: 14083 RVA: 0x000F6EFD File Offset: 0x000F50FD
		private static Location LordsHall
		{
			get
			{
				return LocationComplex.Current.GetLocationWithId("lordshall");
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x000F6F0E File Offset: 0x000F510E
		private static Location Tavern
		{
			get
			{
				return LocationComplex.Current.GetLocationWithId("tavern");
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003705 RID: 14085 RVA: 0x000F6F1F File Offset: 0x000F511F
		private static Location Alley
		{
			get
			{
				return LocationComplex.Current.GetLocationWithId("alley");
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x000F6F30 File Offset: 0x000F5130
		private static Location Center
		{
			get
			{
				return LocationComplex.Current.GetLocationWithId("center");
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x000F6F41 File Offset: 0x000F5141
		private static Location VillageCenter
		{
			get
			{
				return LocationComplex.Current.GetLocationWithId("village_center");
			}
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x000F6F54 File Offset: 0x000F5154
		public override void RegisterEvents()
		{
			CampaignEvents.PrisonersChangeInSettlement.AddNonSerializedListener(this, new Action<Settlement, FlattenedTroopRoster, Hero, bool>(this.OnPrisonersChangeInSettlement));
			CampaignEvents.OnGovernorChangedEvent.AddNonSerializedListener(this, new Action<Town, Hero, Hero>(this.OnGovernorChanged));
			CampaignEvents.OnSettlementLeftEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement>(this.OnSettlementLeft));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTaken));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.LocationCharactersAreReadyToSpawnEvent.AddNonSerializedListener(this, new Action<Dictionary<string, int>>(this.LocationCharactersAreReadyToSpawn));
			CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x000F7019 File Offset: 0x000F5219
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x000F701C File Offset: 0x000F521C
		private void OnGovernorChanged(Town town, Hero oldGovernor, Hero newGovernor)
		{
			if (oldGovernor != null && oldGovernor.IsAlive)
			{
				LocationCharacter locationCharacterOfHero = town.Settlement.LocationComplex.GetLocationCharacterOfHero(oldGovernor);
				if (locationCharacterOfHero != null)
				{
					SettlementAccessModel.AccessDetails accessDetails;
					Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(town.Settlement, out accessDetails);
					Location locationOfCharacter = town.Settlement.LocationComplex.GetLocationOfCharacter(oldGovernor);
					if (LocationComplex.Current != null)
					{
						Location location = (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess) ? HeroAgentSpawnCampaignBehavior.LordsHall : (town.IsTown ? HeroAgentSpawnCampaignBehavior.Tavern : HeroAgentSpawnCampaignBehavior.Center);
						if (location != locationOfCharacter)
						{
							town.Settlement.LocationComplex.ChangeLocation(locationCharacterOfHero, locationOfCharacter, location);
						}
					}
					else
					{
						Debug.Print("LocationComplex is null", 0, Debug.DebugColor.White, 17592186044416UL);
						Debug.FailedAssert("LocationComplex is null", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroAgentSpawnCampaignBehavior.cs", "OnGovernorChanged", 67);
					}
				}
			}
			if (newGovernor != null)
			{
				LocationCharacter locationCharacterOfHero2 = town.Settlement.LocationComplex.GetLocationCharacterOfHero(newGovernor);
				if (locationCharacterOfHero2 != null)
				{
					Location locationOfCharacter2 = town.Settlement.LocationComplex.GetLocationOfCharacter(newGovernor);
					if (LocationComplex.Current != null)
					{
						if (locationOfCharacter2 != HeroAgentSpawnCampaignBehavior.LordsHall)
						{
							town.Settlement.LocationComplex.ChangeLocation(locationCharacterOfHero2, locationOfCharacter2, HeroAgentSpawnCampaignBehavior.LordsHall);
							return;
						}
					}
					else
					{
						Debug.Print("LocationComplex is null", 0, Debug.DebugColor.White, 17592186044416UL);
						Debug.FailedAssert("LocationComplex is null", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\HeroAgentSpawnCampaignBehavior.cs", "OnGovernorChanged", 88);
					}
				}
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x000F7178 File Offset: 0x000F5378
		private void OnMissionEnded(IMission mission)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && Settlement.CurrentSettlement != null && !Hero.MainHero.IsPrisoner && !Settlement.CurrentSettlement.IsUnderSiege)
			{
				this.AddSettlementLocationCharacters(Settlement.CurrentSettlement);
				this._addNotableHelperCharacters = true;
			}
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x000F71C4 File Offset: 0x000F53C4
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			if (LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null)
			{
				if (mobileParty != null)
				{
					if (mobileParty == MobileParty.MainParty)
					{
						this.AddSettlementLocationCharacters(settlement);
						this._addNotableHelperCharacters = true;
						return;
					}
					if (MobileParty.MainParty.CurrentSettlement == settlement && (settlement.IsFortification || settlement.IsVillage))
					{
						this.AddPartyHero(mobileParty, settlement);
						return;
					}
				}
				else if (MobileParty.MainParty.CurrentSettlement == settlement && hero != null)
				{
					if (hero.IsNotable)
					{
						this.AddNotableLocationCharacter(hero, settlement);
						return;
					}
					if (hero.IsWanderer)
					{
						this.AddWandererLocationCharacter(hero, settlement);
					}
				}
			}
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x000F7250 File Offset: 0x000F5450
		public void OnSettlementLeft(MobileParty mobileParty, Settlement settlement)
		{
			if (mobileParty != MobileParty.MainParty && MobileParty.MainParty.CurrentSettlement == settlement && mobileParty.LeaderHero != null && LocationComplex.Current != null)
			{
				Location locationOfCharacter = LocationComplex.Current.GetLocationOfCharacter(mobileParty.LeaderHero);
				if (locationOfCharacter != null)
				{
					locationOfCharacter.RemoveCharacter(mobileParty.LeaderHero);
				}
			}
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x000F72A1 File Offset: 0x000F54A1
		private void OnGameLoadFinished()
		{
			if (Settlement.CurrentSettlement != null && !Hero.MainHero.IsPrisoner && LocationComplex.Current != null && PlayerEncounter.LocationEncounter != null && !Settlement.CurrentSettlement.IsUnderSiege)
			{
				this.AddSettlementLocationCharacters(Settlement.CurrentSettlement);
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x000F72DC File Offset: 0x000F54DC
		private void AddSettlementLocationCharacters(Settlement settlement)
		{
			if (settlement.IsFortification || settlement.IsVillage)
			{
				List<MobileParty> list = Settlement.CurrentSettlement.Parties.ToList<MobileParty>();
				if (settlement.IsFortification)
				{
					this.AddLordsHallCharacters(settlement, ref list);
					this.RefreshPrisonCharacters(settlement);
					this.AddCompanionsAndClanMembersToSettlement(settlement);
					if (settlement.IsFortification)
					{
						this.AddNotablesAndWanderers(settlement);
					}
				}
				else if (settlement.IsVillage)
				{
					this.AddHeroesWithoutPartyCharactersToVillage(settlement);
					this.AddCompanionsAndClanMembersToSettlement(settlement);
				}
				foreach (MobileParty mobileParty in list)
				{
					this.AddPartyHero(mobileParty, settlement);
				}
			}
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x000F7394 File Offset: 0x000F5594
		private void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			if (this._addNotableHelperCharacters && (CampaignMission.Current.Location == HeroAgentSpawnCampaignBehavior.Center || CampaignMission.Current.Location == HeroAgentSpawnCampaignBehavior.VillageCenter))
			{
				this.SpawnNotableHelperCharacters(settlement);
				this._addNotableHelperCharacters = false;
			}
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x000F73E4 File Offset: 0x000F55E4
		private void AddCompanionsAndClanMembersToSettlement(Settlement settlement)
		{
			if (settlement.IsFortification || settlement.IsVillage)
			{
				foreach (Hero hero in Clan.PlayerClan.Lords)
				{
					int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
					if (hero != Hero.MainHero && hero.Age >= (float)heroComesOfAge && !hero.IsPrisoner && hero.CurrentSettlement == settlement && (hero.GovernorOf == null || hero.GovernorOf != settlement.Town))
					{
						Location location;
						if (settlement.IsFortification)
						{
							SettlementAccessModel.AccessDetails accessDetails;
							Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(settlement, out accessDetails);
							location = ((accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.FullAccess) ? HeroAgentSpawnCampaignBehavior.LordsHall : (settlement.IsTown ? HeroAgentSpawnCampaignBehavior.Tavern : HeroAgentSpawnCampaignBehavior.Center));
						}
						else
						{
							location = HeroAgentSpawnCampaignBehavior.VillageCenter;
						}
						IFaction mapFaction = hero.MapFaction;
						uint color = (mapFaction != null) ? mapFaction.Color : 4291609515U;
						IFaction mapFaction2 = hero.MapFaction;
						uint color2 = (mapFaction2 != null) ? mapFaction2.Color : 4291609515U;
						Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(hero.CharacterObject.Race);
						AgentData agentData = new AgentData(new PartyAgentOrigin(PartyBase.MainParty, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(baseMonsterFromRace).NoHorses(true).ClothingColor1(color).ClothingColor2(color2);
						location.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddCompanionBehaviors), "sp_notable", true, LocationCharacter.CharacterRelations.Friendly, null, !PlayerEncounter.LocationEncounter.Settlement.IsVillage, false, null, false, true, true));
					}
				}
				using (IEnumerator<Hero> enumerator2 = Hero.MainHero.CompanionsInParty.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Hero companion = enumerator2.Current;
						if (!companion.IsWounded && !PlayerEncounter.LocationEncounter.CharactersAccompanyingPlayer.Exists((AccompanyingCharacter x) => x.LocationCharacter.Character.HeroObject == companion))
						{
							IFaction mapFaction3 = companion.MapFaction;
							uint color3 = (mapFaction3 != null) ? mapFaction3.Color : 4291609515U;
							IFaction mapFaction4 = companion.MapFaction;
							uint color4 = (mapFaction4 != null) ? mapFaction4.Color : 4291609515U;
							Monster baseMonsterFromRace2 = FaceGen.GetBaseMonsterFromRace(companion.CharacterObject.Race);
							Location location = settlement.IsFortification ? HeroAgentSpawnCampaignBehavior.Center : HeroAgentSpawnCampaignBehavior.VillageCenter;
							AgentData agentData2 = new AgentData(new PartyAgentOrigin(PartyBase.MainParty, companion.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(baseMonsterFromRace2).NoHorses(true).ClothingColor1(color3).ClothingColor2(color4);
							location.AddCharacter(new LocationCharacter(agentData2, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddCompanionBehaviors), "sp_notable", true, LocationCharacter.CharacterRelations.Friendly, null, !PlayerEncounter.LocationEncounter.Settlement.IsVillage, false, null, false, true, true));
						}
					}
				}
			}
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x000F7744 File Offset: 0x000F5944
		private void AddPartyHero(MobileParty mobileParty, Settlement settlement)
		{
			Hero leaderHero = mobileParty.LeaderHero;
			if (leaderHero == null || leaderHero == Hero.MainHero)
			{
				return;
			}
			IFaction mapFaction = leaderHero.MapFaction;
			uint color = (mapFaction != null) ? mapFaction.Color : 4291609515U;
			IFaction mapFaction2 = leaderHero.MapFaction;
			uint color2 = (mapFaction2 != null) ? mapFaction2.Color : 4291609515U;
			Tuple<string, Monster> actionSetAndMonster = HeroAgentSpawnCampaignBehavior.GetActionSetAndMonster(leaderHero.CharacterObject);
			AgentData agentData = new AgentData(new PartyAgentOrigin(mobileParty.Party, leaderHero.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(actionSetAndMonster.Item2).NoHorses(true).ClothingColor1(color).ClothingColor2(color2);
			string spawnTag = "sp_notable";
			Location location;
			if (settlement.IsFortification)
			{
				location = HeroAgentSpawnCampaignBehavior.LordsHall;
			}
			else if (settlement.IsVillage)
			{
				location = HeroAgentSpawnCampaignBehavior.VillageCenter;
			}
			else
			{
				location = HeroAgentSpawnCampaignBehavior.Center;
			}
			if (location != null)
			{
				location.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), spawnTag, true, LocationCharacter.CharacterRelations.Neutral, actionSetAndMonster.Item1, !settlement.IsVillage, false, null, false, false, true));
			}
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000F784F File Offset: 0x000F5A4F
		private void OnHeroPrisonerTaken(PartyBase capturerParty, Hero prisoner)
		{
			if (capturerParty.IsSettlement)
			{
				this.OnPrisonersChangeInSettlement(capturerParty.Settlement, null, prisoner, false);
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000F7868 File Offset: 0x000F5A68
		public void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
			if (settlement != null && settlement.IsFortification && LocationComplex.Current == settlement.LocationComplex)
			{
				if (prisonerHero != null)
				{
					this.SendPrisonerHeroToNextLocation(settlement, prisonerHero, takenFromDungeon);
				}
				if (prisonerRoster != null)
				{
					foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in prisonerRoster)
					{
						if (flattenedTroopRosterElement.Troop.IsHero)
						{
							this.SendPrisonerHeroToNextLocation(settlement, flattenedTroopRosterElement.Troop.HeroObject, takenFromDungeon);
						}
					}
				}
			}
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x000F78F4 File Offset: 0x000F5AF4
		private void SendPrisonerHeroToNextLocation(Settlement settlement, Hero hero, bool takenFromDungeon)
		{
			Location locationOfCharacter = LocationComplex.Current.GetLocationOfCharacter(hero);
			Location location = this.DecideNewLocationOnPrisonerChange(settlement, hero, takenFromDungeon);
			LocationCharacter locationCharacterOfHero = LocationComplex.Current.GetLocationCharacterOfHero(hero);
			if (locationCharacterOfHero == null)
			{
				if (location != null)
				{
					this.AddHeroToDecidedLocation(location, hero, settlement);
					return;
				}
			}
			else if (locationOfCharacter != location)
			{
				LocationComplex.Current.ChangeLocation(locationCharacterOfHero, locationOfCharacter, location);
			}
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000F7944 File Offset: 0x000F5B44
		private Location DecideNewLocationOnPrisonerChange(Settlement settlement, Hero hero, bool takenFromDungeon)
		{
			if (hero.IsPrisoner)
			{
				if (!takenFromDungeon)
				{
					return HeroAgentSpawnCampaignBehavior.Prison;
				}
				return null;
			}
			else
			{
				if (!settlement.IsFortification)
				{
					return HeroAgentSpawnCampaignBehavior.VillageCenter;
				}
				if (hero.IsWanderer && settlement.IsTown)
				{
					return HeroAgentSpawnCampaignBehavior.Tavern;
				}
				if (hero.CharacterObject.Occupation == Occupation.Lord)
				{
					return HeroAgentSpawnCampaignBehavior.LordsHall;
				}
				return HeroAgentSpawnCampaignBehavior.Center;
			}
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x000F79A4 File Offset: 0x000F5BA4
		private void AddHeroToDecidedLocation(Location location, Hero hero, Settlement settlement)
		{
			if (location == HeroAgentSpawnCampaignBehavior.Prison)
			{
				Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(hero.CharacterObject.Race, "_settlement");
				AgentData agentData = new AgentData(new SimpleAgentOrigin(hero.CharacterObject, -1, null, default(UniqueTroopDescriptor))).NoWeapons(true).Monster(monsterWithSuffix).NoHorses(true);
				location.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "sp_prisoner", true, LocationCharacter.CharacterRelations.Neutral, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_villager"), true, false, null, false, false, true));
				return;
			}
			if (location == HeroAgentSpawnCampaignBehavior.VillageCenter)
			{
				Monster monsterWithSuffix2 = FaceGen.GetMonsterWithSuffix(hero.CharacterObject.Race, "_settlement");
				AgentData agentData2 = new AgentData(new PartyAgentOrigin(null, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(monsterWithSuffix2);
				location.AddCharacter(new LocationCharacter(agentData2, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "sp_notable_rural_notable", false, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true));
				return;
			}
			if (location == HeroAgentSpawnCampaignBehavior.Tavern)
			{
				Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(hero.CharacterObject.Race);
				AgentData agentData3 = new AgentData(new PartyAgentOrigin(PartyBase.MainParty, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(baseMonsterFromRace).NoHorses(true);
				location.AddCharacter(new LocationCharacter(agentData3, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddCompanionBehaviors), null, true, LocationCharacter.CharacterRelations.Friendly, null, !PlayerEncounter.LocationEncounter.Settlement.IsVillage, false, null, false, true, true));
				return;
			}
			if (location == HeroAgentSpawnCampaignBehavior.LordsHall)
			{
				Tuple<string, Monster> actionSetAndMonster = HeroAgentSpawnCampaignBehavior.GetActionSetAndMonster(hero.CharacterObject);
				AgentData agentData4 = new AgentData(new SimpleAgentOrigin(hero.CharacterObject, -1, null, default(UniqueTroopDescriptor))).Monster(actionSetAndMonster.Item2).NoHorses(true);
				location.AddCharacter(new LocationCharacter(agentData4, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), null, true, LocationCharacter.CharacterRelations.Neutral, actionSetAndMonster.Item1, true, false, null, false, false, true));
				return;
			}
			if (location == HeroAgentSpawnCampaignBehavior.Center)
			{
				if (hero.IsNotable)
				{
					this.AddNotableLocationCharacter(hero, settlement);
					return;
				}
				Monster monsterWithSuffix3 = FaceGen.GetMonsterWithSuffix(hero.CharacterObject.Race, "_settlement");
				AgentData agentData5 = new AgentData(new PartyAgentOrigin(null, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(monsterWithSuffix3);
				location.AddCharacter(new LocationCharacter(agentData5, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "sp_notable_rural_notable", false, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true));
			}
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x000F7C44 File Offset: 0x000F5E44
		private void AddLordsHallCharacters(Settlement settlement, ref List<MobileParty> partiesToBeSpawn)
		{
			Hero hero = null;
			Hero hero2 = null;
			if (settlement.MapFaction.IsKingdomFaction)
			{
				Hero leader = ((Kingdom)settlement.MapFaction).Leader;
				if (leader.CurrentSettlement == settlement)
				{
					hero = leader;
				}
				if (leader.Spouse != null && leader.Spouse.CurrentSettlement == settlement)
				{
					hero2 = leader.Spouse;
				}
			}
			if (hero == null && settlement.OwnerClan.Leader.CurrentSettlement == settlement)
			{
				hero = settlement.OwnerClan.Leader;
			}
			if (hero2 == null && settlement.OwnerClan.Leader.Spouse != null && settlement.OwnerClan.Leader.Spouse.CurrentSettlement == settlement)
			{
				hero2 = settlement.OwnerClan.Leader.Spouse;
			}
			bool flag = false;
			if (hero != null && hero != Hero.MainHero)
			{
				IFaction mapFaction = hero.MapFaction;
				uint color = (mapFaction != null) ? mapFaction.Color : 4291609515U;
				IFaction mapFaction2 = hero.MapFaction;
				uint color2 = (mapFaction2 != null) ? mapFaction2.Color : 4291609515U;
				flag = true;
				Tuple<string, Monster> actionSetAndMonster = HeroAgentSpawnCampaignBehavior.GetActionSetAndMonster(hero.CharacterObject);
				AgentData agentData = new AgentData(new PartyAgentOrigin(null, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(actionSetAndMonster.Item2).NoHorses(true).ClothingColor1(color).ClothingColor2(color2);
				HeroAgentSpawnCampaignBehavior.LordsHall.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "sp_throne", true, LocationCharacter.CharacterRelations.Neutral, actionSetAndMonster.Item1, true, false, null, false, false, true));
				if (hero.PartyBelongedTo != null && partiesToBeSpawn.Contains(hero.PartyBelongedTo))
				{
					partiesToBeSpawn.Remove(hero.PartyBelongedTo);
				}
			}
			if (hero2 != null && hero2 != Hero.MainHero)
			{
				IFaction mapFaction3 = hero2.MapFaction;
				uint color3 = (mapFaction3 != null) ? mapFaction3.Color : 4291609515U;
				IFaction mapFaction4 = hero2.MapFaction;
				uint color4 = (mapFaction4 != null) ? mapFaction4.Color : 4291609515U;
				Tuple<string, Monster> actionSetAndMonster2 = HeroAgentSpawnCampaignBehavior.GetActionSetAndMonster(hero2.CharacterObject);
				AgentData agentData2 = new AgentData(new PartyAgentOrigin(null, hero2.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(actionSetAndMonster2.Item2).NoHorses(true).ClothingColor1(color3).ClothingColor2(color4);
				HeroAgentSpawnCampaignBehavior.LordsHall.AddCharacter(new LocationCharacter(agentData2, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), flag ? "sp_notable" : "sp_throne", true, LocationCharacter.CharacterRelations.Neutral, actionSetAndMonster2.Item1, true, false, null, false, false, true));
				if (hero2.PartyBelongedTo != null && partiesToBeSpawn.Contains(hero2.PartyBelongedTo))
				{
					partiesToBeSpawn.Remove(hero2.PartyBelongedTo);
				}
			}
			int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
			foreach (Hero hero3 in settlement.HeroesWithoutParty)
			{
				if (hero3 != hero && hero3 != hero2 && hero3.Age >= (float)heroComesOfAge && !hero3.IsPrisoner && (hero3.Clan != Clan.PlayerClan || (hero3.GovernorOf != null && hero3.GovernorOf == settlement.Town)))
				{
					Tuple<string, Monster> actionSetAndMonster3 = HeroAgentSpawnCampaignBehavior.GetActionSetAndMonster(hero3.CharacterObject);
					IFaction mapFaction5 = hero3.MapFaction;
					uint color5 = (mapFaction5 != null) ? mapFaction5.Color : 4291609515U;
					IFaction mapFaction6 = hero3.MapFaction;
					uint color6 = (mapFaction6 != null) ? mapFaction6.Color : 4291609515U;
					AgentData agentData3 = new AgentData(new SimpleAgentOrigin(hero3.CharacterObject, -1, null, default(UniqueTroopDescriptor))).Monster(actionSetAndMonster3.Item2).NoHorses(true).ClothingColor1(color5).ClothingColor2(color6);
					HeroAgentSpawnCampaignBehavior.LordsHall.AddCharacter(new LocationCharacter(agentData3, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "sp_notable", true, LocationCharacter.CharacterRelations.Neutral, actionSetAndMonster3.Item1, true, false, null, false, false, true));
				}
			}
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000F8064 File Offset: 0x000F6264
		private void RefreshPrisonCharacters(Settlement settlement)
		{
			HeroAgentSpawnCampaignBehavior.Prison.RemoveAllHeroCharactersFromPrison();
			List<CharacterObject> prisonerHeroes = settlement.SettlementComponent.GetPrisonerHeroes();
			if (Campaign.Current.GameMode != CampaignGameMode.Campaign)
			{
				for (int i = 0; i < 5; i++)
				{
					prisonerHeroes.Add(Game.Current.ObjectManager.GetObject<CharacterObject>("townsman_empire"));
				}
			}
			foreach (CharacterObject characterObject in prisonerHeroes)
			{
				Hero heroObject = characterObject.HeroObject;
				uint? num;
				if (heroObject == null)
				{
					num = null;
				}
				else
				{
					IFaction mapFaction = heroObject.MapFaction;
					num = ((mapFaction != null) ? new uint?(mapFaction.Color) : null);
				}
				uint color = num ?? 4291609515U;
				Hero heroObject2 = characterObject.HeroObject;
				uint? num2;
				if (heroObject2 == null)
				{
					num2 = null;
				}
				else
				{
					IFaction mapFaction2 = heroObject2.MapFaction;
					num2 = ((mapFaction2 != null) ? new uint?(mapFaction2.Color) : null);
				}
				uint color2 = num2 ?? 4291609515U;
				Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(characterObject.Race, "_settlement");
				AgentData agentData = new AgentData(new SimpleAgentOrigin(characterObject, -1, null, default(UniqueTroopDescriptor))).NoWeapons(true).Monster(monsterWithSuffix).NoHorses(true).ClothingColor1(color).ClothingColor2(color2);
				HeroAgentSpawnCampaignBehavior.Prison.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "sp_prisoner", true, LocationCharacter.CharacterRelations.Neutral, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, "_villager"), true, false, null, false, false, true));
			}
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000F823C File Offset: 0x000F643C
		private void AddNotablesAndWanderers(Settlement settlement)
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				foreach (Hero notable in settlement.Notables)
				{
					this.AddNotableLocationCharacter(notable, settlement);
				}
				foreach (Hero hero in from x in settlement.HeroesWithoutParty
				where x.IsWanderer || x.IsPlayerCompanion
				select x)
				{
					if (hero.GovernorOf == null || hero.GovernorOf != settlement.Town)
					{
						this.AddWandererLocationCharacter(hero, settlement);
					}
				}
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000F8318 File Offset: 0x000F6518
		private void AddWandererLocationCharacter(Hero wanderer, Settlement settlement)
		{
			bool flag = settlement.Culture.StringId.ToLower() == "aserai" || settlement.Culture.StringId.ToLower() == "khuzait";
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(wanderer.CharacterObject.Race, "_settlement");
			string actionSetCode = flag ? ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, wanderer.IsFemale, "_warrior_in_aserai_tavern") : ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, wanderer.IsFemale, "_warrior_in_tavern");
			LocationCharacter locationCharacter = new LocationCharacter(new AgentData(new PartyAgentOrigin(null, wanderer.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(monsterWithSuffix).NoHorses(true), new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "npc_common", true, LocationCharacter.CharacterRelations.Neutral, actionSetCode, true, false, null, false, false, true);
			if (settlement.IsCastle)
			{
				HeroAgentSpawnCampaignBehavior.Center.AddCharacter(locationCharacter);
				return;
			}
			if (settlement.IsTown)
			{
				IAlleyCampaignBehavior campaignBehavior = CampaignBehaviorBase.GetCampaignBehavior<IAlleyCampaignBehavior>();
				Location location;
				if (campaignBehavior != null && campaignBehavior.IsHeroAlleyLeaderOfAnyPlayerAlley(wanderer))
				{
					location = HeroAgentSpawnCampaignBehavior.Alley;
				}
				else
				{
					location = HeroAgentSpawnCampaignBehavior.Tavern;
				}
				location.AddCharacter(locationCharacter);
				return;
			}
			HeroAgentSpawnCampaignBehavior.VillageCenter.AddCharacter(locationCharacter);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000F8448 File Offset: 0x000F6648
		private void AddNotableLocationCharacter(Hero notable, Settlement settlement)
		{
			string suffix = notable.IsArtisan ? "_villager_artisan" : (notable.IsMerchant ? "_villager_merchant" : (notable.IsPreacher ? "_villager_preacher" : (notable.IsGangLeader ? "_villager_gangleader" : (notable.IsRuralNotable ? "_villager_ruralnotable" : (notable.IsFemale ? "_lord" : "_villager_merchant")))));
			string text = notable.IsArtisan ? "sp_notable_artisan" : (notable.IsMerchant ? "sp_notable_merchant" : (notable.IsPreacher ? "sp_notable_preacher" : (notable.IsGangLeader ? "sp_notable_gangleader" : (notable.IsRuralNotable ? "sp_notable_rural_notable" : ((notable.GovernorOf == notable.CurrentSettlement.Town) ? "sp_governor" : "sp_notable")))));
			MBReadOnlyList<Workshop> ownedWorkshops = notable.OwnedWorkshops;
			if (ownedWorkshops.Count != 0)
			{
				for (int i = 0; i < ownedWorkshops.Count; i++)
				{
					if (!ownedWorkshops[i].WorkshopType.IsHidden)
					{
						text = text + "_" + ownedWorkshops[i].Tag;
						break;
					}
				}
			}
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(notable.CharacterObject.Race, "_settlement");
			AgentData agentData = new AgentData(new PartyAgentOrigin(null, notable.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(monsterWithSuffix).NoHorses(true);
			LocationCharacter locationCharacter = new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), text, true, LocationCharacter.CharacterRelations.Neutral, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, notable.IsFemale, suffix), true, false, null, false, false, true);
			if (settlement.IsVillage)
			{
				HeroAgentSpawnCampaignBehavior.VillageCenter.AddCharacter(locationCharacter);
				return;
			}
			HeroAgentSpawnCampaignBehavior.Center.AddCharacter(locationCharacter);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000F8614 File Offset: 0x000F6814
		private void AddHeroesWithoutPartyCharactersToVillage(Settlement settlement)
		{
			foreach (Hero hero in settlement.HeroesWithoutParty)
			{
				Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(hero.CharacterObject.Race, "_settlement");
				AgentData agentData = new AgentData(new PartyAgentOrigin(null, hero.CharacterObject, -1, default(UniqueTroopDescriptor), false)).Monster(monsterWithSuffix);
				HeroAgentSpawnCampaignBehavior.VillageCenter.AddCharacter(new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFixedCharacterBehaviors), "sp_notable_rural_notable", false, LocationCharacter.CharacterRelations.Neutral, null, true, false, null, false, false, true));
			}
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000F86D0 File Offset: 0x000F68D0
		private void SpawnNotableHelperCharacters(Settlement settlement)
		{
			int num = settlement.Notables.Count((Hero x) => x.IsGangLeader);
			int characterToSpawnCount = settlement.Notables.Count((Hero x) => x.IsPreacher);
			int characterToSpawnCount2 = settlement.Notables.Count((Hero x) => x.IsArtisan);
			int characterToSpawnCount3 = settlement.Notables.Count((Hero x) => x.IsRuralNotable || x.IsHeadman);
			int characterToSpawnCount4 = settlement.Notables.Count((Hero x) => x.IsMerchant);
			this.SpawnNotableHelperCharacter(settlement.Culture.GangleaderBodyguard, "_gangleader_bodyguard", "sp_gangleader_bodyguard", num * 2);
			this.SpawnNotableHelperCharacter(settlement.Culture.PreacherNotary, "_merchant_notary", "sp_preacher_notary", characterToSpawnCount);
			this.SpawnNotableHelperCharacter(settlement.Culture.ArtisanNotary, "_merchant_notary", "sp_artisan_notary", characterToSpawnCount2);
			this.SpawnNotableHelperCharacter(settlement.Culture.RuralNotableNotary, "_merchant_notary", "sp_rural_notable_notary", characterToSpawnCount3);
			this.SpawnNotableHelperCharacter(settlement.Culture.MerchantNotary, "_merchant_notary", "sp_merchant_notary", characterToSpawnCount4);
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x000F8844 File Offset: 0x000F6A44
		private void SpawnNotableHelperCharacter(CharacterObject character, string actionSetSuffix, string tag, int characterToSpawnCount)
		{
			Location location = HeroAgentSpawnCampaignBehavior.Center ?? HeroAgentSpawnCampaignBehavior.VillageCenter;
			while (characterToSpawnCount > 0)
			{
				Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(character.Race, "_settlement");
				int minValue;
				int maxValue;
				Campaign.Current.Models.AgeModel.GetAgeLimitForLocation(character, out minValue, out maxValue, "Notary");
				AgentData agentData = new AgentData(new SimpleAgentOrigin(character, -1, null, default(UniqueTroopDescriptor))).Monster(monsterWithSuffix).NoHorses(true).Age(MBRandom.RandomInt(minValue, maxValue));
				LocationCharacter locationCharacter = new LocationCharacter(agentData, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddWandererBehaviors), tag, true, LocationCharacter.CharacterRelations.Neutral, ActionSetCode.GenerateActionSetNameWithSuffix(agentData.AgentMonster, agentData.AgentIsFemale, actionSetSuffix), true, false, null, false, false, true);
				location.AddCharacter(locationCharacter);
				characterToSpawnCount--;
			}
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x000F8918 File Offset: 0x000F6B18
		private static Tuple<string, Monster> GetActionSetAndMonster(CharacterObject character)
		{
			Monster monsterWithSuffix = FaceGen.GetMonsterWithSuffix(character.Race, "_settlement");
			return new Tuple<string, Monster>(ActionSetCode.GenerateActionSetNameWithSuffix(monsterWithSuffix, character.IsFemale, "_lord"), monsterWithSuffix);
		}

		// Token: 0x0400116D RID: 4461
		[NonSerialized]
		private bool _addNotableHelperCharacters;
	}
}
