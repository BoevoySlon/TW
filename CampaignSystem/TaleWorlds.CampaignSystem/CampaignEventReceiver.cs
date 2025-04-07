using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000035 RID: 53
	public abstract class CampaignEventReceiver
	{
		// Token: 0x0600045C RID: 1116 RVA: 0x0001DF9E File Offset: 0x0001C19E
		public virtual void RemoveListeners(object o)
		{
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001DFA0 File Offset: 0x0001C1A0
		public virtual void OnCharacterCreationIsOver()
		{
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001DFA2 File Offset: 0x0001C1A2
		public virtual void OnHeroLevelledUp(Hero hero, bool shouldNotify = true)
		{
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001DFA4 File Offset: 0x0001C1A4
		public virtual void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001DFA6 File Offset: 0x0001C1A6
		public virtual void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001DFA8 File Offset: 0x0001C1A8
		public virtual void OnHeroWounded(Hero woundedHero)
		{
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001DFAA File Offset: 0x0001C1AA
		public virtual void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001DFAC File Offset: 0x0001C1AC
		public virtual void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001DFAE File Offset: 0x0001C1AE
		public virtual void OnIssueLogAdded(IssueBase issue, bool hideInformation)
		{
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001DFB0 File Offset: 0x0001C1B0
		public virtual void OnClanTierChanged(Clan clan, bool shouldNotify = true)
		{
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001DFB2 File Offset: 0x0001C1B2
		public virtual void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail actionDetail, bool showNotification = true)
		{
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		public virtual void OnCompanionClanCreated(Clan clan)
		{
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001DFB6 File Offset: 0x0001C1B6
		public virtual void OnHeroJoinedParty(Hero hero, MobileParty mobileParty)
		{
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001DFB8 File Offset: 0x0001C1B8
		public virtual void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001DFBA File Offset: 0x0001C1BA
		public virtual void OnKingdomDecisionCancelled(KingdomDecision decision, bool isPlayerInvolved)
		{
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001DFBC File Offset: 0x0001C1BC
		public virtual void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001DFBE File Offset: 0x0001C1BE
		public virtual void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001DFC0 File Offset: 0x0001C1C0
		public virtual void OnHeroOrPartyGaveItem(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> receiver, ItemRosterElement itemRosterElement, bool showNotification)
		{
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001DFC2 File Offset: 0x0001C1C2
		public virtual void OnBanditPartyRecruited(MobileParty banditParty)
		{
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001DFC4 File Offset: 0x0001C1C4
		public virtual void OnArmyCreated(Army army)
		{
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001DFC6 File Offset: 0x0001C1C6
		public virtual void OnPartyAttachedAnotherParty(MobileParty mobileParty)
		{
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001DFC8 File Offset: 0x0001C1C8
		public virtual void OnNearbyPartyAddedToPlayerMapEvent(MobileParty mobileParty)
		{
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001DFCA File Offset: 0x0001C1CA
		public virtual void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001DFCC File Offset: 0x0001C1CC
		public virtual void OnArmyGathered(Army army, Settlement gatheringSettlement)
		{
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001DFCE File Offset: 0x0001C1CE
		public virtual void OnPerkOpened(Hero hero, PerkObject perk)
		{
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001DFD0 File Offset: 0x0001C1D0
		public virtual void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001DFD2 File Offset: 0x0001C1D2
		public virtual void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001DFD4 File Offset: 0x0001C1D4
		public virtual void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001DFD6 File Offset: 0x0001C1D6
		public virtual void OnAfterSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001DFD8 File Offset: 0x0001C1D8
		public virtual void OnMercenaryTroopChangedInTown(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001DFDA File Offset: 0x0001C1DA
		public virtual void OnMercenaryNumberChangedInTown(Town town, int oldNumber, int newNumber)
		{
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001DFDC File Offset: 0x0001C1DC
		public virtual void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001DFDE File Offset: 0x0001C1DE
		public virtual void OnAlleyClearedByPlayer(Alley alley)
		{
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001DFE0 File Offset: 0x0001C1E0
		public virtual void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
		{
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001DFE2 File Offset: 0x0001C1E2
		public virtual void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum romanceLevel)
		{
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001DFE4 File Offset: 0x0001C1E4
		public virtual void OnHeroesMarried(Hero hero1, Hero hero2, bool showNotification = true)
		{
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001DFE6 File Offset: 0x0001C1E6
		public virtual void OnPlayerEliminatedFromTournament(int round, Town town)
		{
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001DFE8 File Offset: 0x0001C1E8
		public virtual void OnPlayerStartedTournamentMatch(Town town)
		{
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001DFEA File Offset: 0x0001C1EA
		public virtual void OnTournamentStarted(Town town)
		{
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001DFEC File Offset: 0x0001C1EC
		public virtual void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001DFEE File Offset: 0x0001C1EE
		public virtual void OnTournamentCancelled(Town town)
		{
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001DFF0 File Offset: 0x0001C1F0
		public virtual void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0001DFF2 File Offset: 0x0001C1F2
		public virtual void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001DFF4 File Offset: 0x0001C1F4
		public virtual void OnKingdomCreated(Kingdom createdKingdom)
		{
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001DFF6 File Offset: 0x0001C1F6
		public virtual void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001DFF8 File Offset: 0x0001C1F8
		public virtual void OnKingdomDestroyed(Kingdom kingdom)
		{
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001DFFA File Offset: 0x0001C1FA
		public virtual void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001DFFC File Offset: 0x0001C1FC
		public virtual void OnBarterAccepted(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0001DFFE File Offset: 0x0001C1FE
		public virtual void OnBarterCanceled(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0001E000 File Offset: 0x0001C200
		public virtual void OnStartBattle(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0001E002 File Offset: 0x0001C202
		public virtual void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001E004 File Offset: 0x0001C204
		public virtual void TownRebelliousStateChanged(Town town, bool rebelliousState)
		{
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001E006 File Offset: 0x0001C206
		public virtual void OnRebelliousClanDisbandedAtSettlement(Settlement settlement, Clan clan)
		{
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001E008 File Offset: 0x0001C208
		public virtual void OnItemsLooted(MobileParty mobileParty, ItemRoster items)
		{
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001E00A File Offset: 0x0001C20A
		public virtual void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0001E00C File Offset: 0x0001C20C
		public virtual void OnMobilePartyCreated(MobileParty party)
		{
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001E00E File Offset: 0x0001C20E
		public virtual void OnMobilePartyQuestStatusChanged(MobileParty party, bool isUsedByQuest)
		{
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001E010 File Offset: 0x0001C210
		public virtual void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001E012 File Offset: 0x0001C212
		public virtual void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001E014 File Offset: 0x0001C214
		public virtual void OnChildEducationCompleted(Hero hero, int age)
		{
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001E016 File Offset: 0x0001C216
		public virtual void OnHeroComesOfAge(Hero hero)
		{
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001E018 File Offset: 0x0001C218
		public virtual void OnHeroReachesTeenAge(Hero hero)
		{
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001E01A File Offset: 0x0001C21A
		public virtual void OnHeroGrowsOutOfInfancy(Hero hero)
		{
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001E01C File Offset: 0x0001C21C
		public virtual void OnCharacterDefeated(Hero winner, Hero loser)
		{
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001E01E File Offset: 0x0001C21E
		public virtual void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001E020 File Offset: 0x0001C220
		public virtual void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail)
		{
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001E022 File Offset: 0x0001C222
		public virtual void OnCharacterBecameFugitive(Hero hero)
		{
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001E024 File Offset: 0x0001C224
		public virtual void OnPlayerMetHero(Hero hero)
		{
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001E026 File Offset: 0x0001C226
		public virtual void OnPlayerLearnsAboutHero(Hero hero)
		{
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001E028 File Offset: 0x0001C228
		public virtual void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotify)
		{
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001E02A File Offset: 0x0001C22A
		public virtual void OnCrimeRatingChanged(IFaction kingdom, float deltaCrimeAmount)
		{
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001E02C File Offset: 0x0001C22C
		public virtual void OnNewCompanionAdded(Hero newCompanion)
		{
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001E02E File Offset: 0x0001C22E
		public virtual void OnAfterMissionStarted(IMission iMission)
		{
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001E030 File Offset: 0x0001C230
		public virtual void OnGameMenuOpened(MenuCallbackArgs args)
		{
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001E032 File Offset: 0x0001C232
		public virtual void OnVillageBecomeNormal(Village village)
		{
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0001E034 File Offset: 0x0001C234
		public virtual void OnVillageBeingRaided(Village village)
		{
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001E036 File Offset: 0x0001C236
		public virtual void OnVillageLooted(Village village)
		{
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0001E038 File Offset: 0x0001C238
		public virtual void OnAgentJoinedConversation(IAgent agent)
		{
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001E03A File Offset: 0x0001C23A
		public virtual void OnConversationEnded(IEnumerable<CharacterObject> characters)
		{
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001E03C File Offset: 0x0001C23C
		public virtual void OnMapEventEnded(MapEvent mapEvent)
		{
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001E03E File Offset: 0x0001C23E
		public virtual void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001E040 File Offset: 0x0001C240
		public virtual void OnRansomOfferedToPlayer(Hero captiveHero)
		{
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001E042 File Offset: 0x0001C242
		public virtual void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001E044 File Offset: 0x0001C244
		public virtual void OnMissionStarted(IMission mission)
		{
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001E046 File Offset: 0x0001C246
		public virtual void OnRansomOfferCancelled(Hero captiveHero)
		{
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001E048 File Offset: 0x0001C248
		public virtual void OnPeaceOfferedToPlayer(IFaction opponentFaction, int tributeAmount)
		{
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001E04A File Offset: 0x0001C24A
		public virtual void OnPeaceOfferCancelled(IFaction opponentFaction)
		{
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001E04C File Offset: 0x0001C24C
		public virtual void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001E04E File Offset: 0x0001C24E
		public virtual void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001E050 File Offset: 0x0001C250
		public virtual void OnVassalOrMercenaryServiceOfferedToPlayer(Kingdom offeredKingdom)
		{
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001E052 File Offset: 0x0001C252
		public virtual void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001E054 File Offset: 0x0001C254
		public virtual void OnPlayerBoardGameOver(Hero opposingHero, BoardGameHelper.BoardGameState state)
		{
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001E056 File Offset: 0x0001C256
		public virtual void OnCommonAreaStateChanged(Alley alley, Alley.AreaState oldState, Alley.AreaState newState)
		{
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001E058 File Offset: 0x0001C258
		public virtual void BeforeMissionOpened()
		{
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001E05A File Offset: 0x0001C25A
		public virtual void OnPartyRemoved(PartyBase party)
		{
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001E05C File Offset: 0x0001C25C
		public virtual void OnPartySizeChanged(PartyBase party)
		{
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001E05E File Offset: 0x0001C25E
		public virtual void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001E060 File Offset: 0x0001C260
		public virtual void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001E062 File Offset: 0x0001C262
		public virtual void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001E064 File Offset: 0x0001C264
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001E066 File Offset: 0x0001C266
		public virtual void OnSessionStart(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001E068 File Offset: 0x0001C268
		public virtual void OnAfterSessionStart(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001E06A File Offset: 0x0001C26A
		public virtual void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001E06C File Offset: 0x0001C26C
		public virtual void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001E06E File Offset: 0x0001C26E
		public virtual void OnGameEarlyLoaded(CampaignGameStarter campaignGameStarter)
		{
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001E070 File Offset: 0x0001C270
		public virtual void OnPlayerTradeProfit(int profit)
		{
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001E072 File Offset: 0x0001C272
		public virtual void OnRulingClanChanged(Kingdom kingdom, Clan newRulingClan)
		{
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001E074 File Offset: 0x0001C274
		public virtual void OnPrisonerReleased(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001E076 File Offset: 0x0001C276
		public virtual void OnGameLoadFinished()
		{
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001E078 File Offset: 0x0001C278
		public virtual void OnPartyJoinedArmy(MobileParty mobileParty)
		{
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001E07A File Offset: 0x0001C27A
		public virtual void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001E07C File Offset: 0x0001C27C
		public virtual void OnArmyLeaderThink(Hero hero, Army.ArmyLeaderThinkReason reason)
		{
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001E07E File Offset: 0x0001C27E
		public virtual void OnArmyOverlaySetDirty()
		{
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001E080 File Offset: 0x0001C280
		public virtual void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001E082 File Offset: 0x0001C282
		public virtual void MissionTick(float dt)
		{
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001E084 File Offset: 0x0001C284
		public virtual void OnChildConceived(Hero mother)
		{
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001E086 File Offset: 0x0001C286
		public virtual void OnGivenBirth(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001E088 File Offset: 0x0001C288
		public virtual void OnUnitRecruited(CharacterObject character, int amount)
		{
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001E08A File Offset: 0x0001C28A
		public virtual void OnPlayerBattleEnd(MapEvent mapEvent)
		{
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001E08C File Offset: 0x0001C28C
		public virtual void OnMissionEnded(IMission mission)
		{
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001E08E File Offset: 0x0001C28E
		public virtual void TickPartialHourlyAi(MobileParty party)
		{
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001E090 File Offset: 0x0001C290
		public virtual void QuarterDailyPartyTick(MobileParty party)
		{
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001E092 File Offset: 0x0001C292
		public virtual void AiHourlyTick(MobileParty party, PartyThinkParams partyThinkParams)
		{
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001E094 File Offset: 0x0001C294
		public virtual void HourlyTick()
		{
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001E096 File Offset: 0x0001C296
		public virtual void HourlyTickParty(MobileParty mobileParty)
		{
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001E098 File Offset: 0x0001C298
		public virtual void HourlyTickSettlement(Settlement settlement)
		{
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001E09A File Offset: 0x0001C29A
		public virtual void HourlyTickClan(Clan clan)
		{
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001E09C File Offset: 0x0001C29C
		public virtual void DailyTick()
		{
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001E09E File Offset: 0x0001C29E
		public virtual void DailyTickParty(MobileParty mobileParty)
		{
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001E0A0 File Offset: 0x0001C2A0
		public virtual void DailyTickTown(Town town)
		{
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001E0A2 File Offset: 0x0001C2A2
		public virtual void DailyTickSettlement(Settlement settlement)
		{
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001E0A4 File Offset: 0x0001C2A4
		public virtual void DailyTickClan(Clan clan)
		{
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001E0A6 File Offset: 0x0001C2A6
		public virtual void OnPlayerBodyPropertiesChanged()
		{
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0001E0A8 File Offset: 0x0001C2A8
		public virtual void WeeklyTick()
		{
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001E0AA File Offset: 0x0001C2AA
		public virtual void CollectAvailableTutorials(ref List<CampaignTutorial> tutorials)
		{
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001E0AC File Offset: 0x0001C2AC
		public virtual void DailyTickHero(Hero hero)
		{
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001E0AE File Offset: 0x0001C2AE
		public virtual void OnTutorialCompleted(string tutorial)
		{
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001E0B0 File Offset: 0x0001C2B0
		public virtual void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001E0B2 File Offset: 0x0001C2B2
		public virtual void BeforeGameMenuOpened(MenuCallbackArgs args)
		{
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001E0B4 File Offset: 0x0001C2B4
		public virtual void AfterGameMenuOpened(MenuCallbackArgs args)
		{
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001E0B6 File Offset: 0x0001C2B6
		public virtual void OnBarterablesRequested(BarterData args)
		{
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001E0B8 File Offset: 0x0001C2B8
		public virtual void OnPartyVisibilityChanged(PartyBase party)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001E0BA File Offset: 0x0001C2BA
		public virtual void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001E0BC File Offset: 0x0001C2BC
		public virtual void TrackDetected(Track track)
		{
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001E0BE File Offset: 0x0001C2BE
		public virtual void TrackLost(Track track)
		{
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001E0C0 File Offset: 0x0001C2C0
		public virtual void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001E0C2 File Offset: 0x0001C2C2
		public virtual void LocationCharactersSimulated()
		{
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001E0C4 File Offset: 0x0001C2C4
		public virtual void OnPlayerUpgradedTroops(CharacterObject upgradeFromTroop, CharacterObject upgradeToTroop, int number)
		{
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001E0C6 File Offset: 0x0001C2C6
		public virtual void OnHeroCombatHit(CharacterObject attackerTroop, CharacterObject attackedTroop, PartyBase party, WeaponComponentData usedWeapon, bool isFatal, int xp)
		{
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001E0C8 File Offset: 0x0001C2C8
		public virtual void OnCharacterPortraitPopUpOpened(CharacterObject character)
		{
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001E0CA File Offset: 0x0001C2CA
		public virtual void OnCharacterPortraitPopUpClosed()
		{
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001E0CC File Offset: 0x0001C2CC
		public virtual void OnPlayerStartTalkFromMenu(Hero hero)
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001E0CE File Offset: 0x0001C2CE
		public virtual void OnGameMenuOptionSelected(GameMenuOption gameMenuOption)
		{
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001E0D0 File Offset: 0x0001C2D0
		public virtual void OnPlayerStartRecruitment(CharacterObject recruitTroopCharacter)
		{
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001E0D2 File Offset: 0x0001C2D2
		public virtual void OnBeforePlayerCharacterChanged(Hero oldPlayer, Hero newPlayer)
		{
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001E0D4 File Offset: 0x0001C2D4
		public virtual void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newMainParty, bool isMainPartyChanged)
		{
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001E0D6 File Offset: 0x0001C2D6
		public virtual void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001E0D8 File Offset: 0x0001C2D8
		public virtual void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001E0DA File Offset: 0x0001C2DA
		public virtual void OnPlayerSiegeStarted()
		{
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001E0DC File Offset: 0x0001C2DC
		public virtual void OnSiegeEventEnded(SiegeEvent siegeEvent)
		{
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001E0DE File Offset: 0x0001C2DE
		public virtual void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001E0E0 File Offset: 0x0001C2E0
		public virtual void OnSiegeBombardmentHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, SiegeBombardTargets target)
		{
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001E0E2 File Offset: 0x0001C2E2
		public virtual void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001E0E4 File Offset: 0x0001C2E4
		public virtual void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001E0E6 File Offset: 0x0001C2E6
		public virtual void OnTradeRumorIsTaken(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001E0E8 File Offset: 0x0001C2E8
		public virtual void OnCheckForIssue(Hero hero)
		{
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001E0EA File Offset: 0x0001C2EA
		public virtual void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver)
		{
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		public virtual void OnTroopsDeserted(MobileParty mobileParty, TroopRoster desertedTroops)
		{
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001E0EE File Offset: 0x0001C2EE
		public virtual void OnTroopRecruited(Hero recruiterHero, Settlement recruitmentSettlement, Hero recruitmentSource, CharacterObject troop, int amount)
		{
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001E0F0 File Offset: 0x0001C2F0
		public virtual void OnTroopGivenToSettlement(Hero giverHero, Settlement recipientSettlement, TroopRoster roster)
		{
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001E0F2 File Offset: 0x0001C2F2
		public virtual void OnItemSold(PartyBase receiverParty, PartyBase payerParty, ItemRosterElement itemRosterElement, int number, Settlement currentSettlement)
		{
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001E0F4 File Offset: 0x0001C2F4
		public virtual void OnCaravanTransactionCompleted(MobileParty caravanParty, Town town, List<ValueTuple<EquipmentElement, int>> itemRosterElements)
		{
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001E0F6 File Offset: 0x0001C2F6
		public virtual void OnPrisonerSold(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001E0F8 File Offset: 0x0001C2F8
		public virtual void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001E0FA File Offset: 0x0001C2FA
		public virtual void OnPartyDisbandStarted(MobileParty disbandParty)
		{
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001E0FC File Offset: 0x0001C2FC
		public virtual void OnPartyDisbandCanceled(MobileParty disbandParty)
		{
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001E0FE File Offset: 0x0001C2FE
		public virtual void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001E100 File Offset: 0x0001C300
		public virtual void OnHideoutDeactivated(Settlement hideout)
		{
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001E102 File Offset: 0x0001C302
		public virtual void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001E104 File Offset: 0x0001C304
		public virtual void OnItemsDiscardedByPlayer(ItemRoster roster)
		{
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001E106 File Offset: 0x0001C306
		public virtual void OnPersuasionProgressCommitted(Tuple<PersuasionOptionArgs, PersuasionOptionResult> progress)
		{
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001E108 File Offset: 0x0001C308
		public virtual void OnHeroSharedFoodWithAnother(Hero supporterHero, Hero supportedHero, float influence)
		{
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001E10A File Offset: 0x0001C30A
		public virtual void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001E10C File Offset: 0x0001C30C
		public virtual void OnQuestStarted(QuestBase quest)
		{
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001E10E File Offset: 0x0001C30E
		public virtual void OnItemProduced(ItemObject itemObject, Settlement settlement, int count)
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001E110 File Offset: 0x0001C310
		public virtual void OnItemConsumed(ItemObject itemObject, Settlement settlement, int count)
		{
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001E112 File Offset: 0x0001C312
		public virtual void OnPartyConsumedFood(MobileParty party)
		{
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001E114 File Offset: 0x0001C314
		public virtual void SiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001E116 File Offset: 0x0001C316
		public virtual void SiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001E118 File Offset: 0x0001C318
		public virtual void RaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001E11A File Offset: 0x0001C31A
		public virtual void ForceSuppliesCompleted(BattleSideEnum winnerSide, ForceSuppliesEventComponent forceSuppliesEvent)
		{
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001E11C File Offset: 0x0001C31C
		public virtual void ForceVolunteersCompleted(BattleSideEnum winnerSide, ForceVolunteersEventComponent forceVolunteersEvent)
		{
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001E11E File Offset: 0x0001C31E
		public virtual void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001E120 File Offset: 0x0001C320
		public virtual void OnGameOver()
		{
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001E122 File Offset: 0x0001C322
		public virtual void OnClanDestroyed(Clan destroyedClan)
		{
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001E124 File Offset: 0x0001C324
		public virtual void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent)
		{
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001E126 File Offset: 0x0001C326
		public virtual void OnNewIssueCreated(IssueBase issue)
		{
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001E128 File Offset: 0x0001C328
		public virtual void OnIssueOwnerChanged(IssueBase issue, Hero oldOwner)
		{
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001E12A File Offset: 0x0001C32A
		public virtual void OnNewItemCrafted(ItemObject itemObject)
		{
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001E12C File Offset: 0x0001C32C
		public virtual void OnWorkshopInitialized(Workshop workshop)
		{
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001E12E File Offset: 0x0001C32E
		public virtual void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001E130 File Offset: 0x0001C330
		public virtual void OnWorkshopTypeChanged(Workshop workshop)
		{
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001E132 File Offset: 0x0001C332
		public virtual void OnEquipmentSmeltedByHero(Hero hero, EquipmentElement equipmentElement)
		{
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001E134 File Offset: 0x0001C334
		public virtual void CraftingPartUnlocked(CraftingPiece craftingPiece)
		{
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001E136 File Offset: 0x0001C336
		public virtual void OnPrisonerTaken(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001E138 File Offset: 0x0001C338
		public virtual void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001E13A File Offset: 0x0001C33A
		public virtual void OnBeforeSave()
		{
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001E13C File Offset: 0x0001C33C
		public virtual void OnMainPartyPrisonerRecruited(FlattenedTroopRoster roster)
		{
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001E13E File Offset: 0x0001C33E
		public virtual void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001E140 File Offset: 0x0001C340
		public virtual void CanMoveToSettlement(Hero hero, ref bool result)
		{
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001E142 File Offset: 0x0001C342
		public virtual void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001E144 File Offset: 0x0001C344
		public virtual void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001E146 File Offset: 0x0001C346
		public virtual void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001E148 File Offset: 0x0001C348
		public virtual void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001E14A File Offset: 0x0001C34A
		public virtual void OnSaveOver(bool isSuccessful, string saveName)
		{
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001E14C File Offset: 0x0001C34C
		public virtual void OnSaveStarted()
		{
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001E14E File Offset: 0x0001C34E
		public virtual void CanHeroMarry(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001E150 File Offset: 0x0001C350
		public virtual void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001E152 File Offset: 0x0001C352
		public virtual void OnPartyLeaderChangeOfferCanceled(MobileParty party)
		{
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001E154 File Offset: 0x0001C354
		public virtual void OnClanInfluenceChanged(Clan clan, float change)
		{
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001E156 File Offset: 0x0001C356
		public virtual void OnPlayerPartyKnockedOrKilledTroop(CharacterObject strikedTroop)
		{
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001E158 File Offset: 0x0001C358
		public virtual void OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType incomeType, int incomeAmount)
		{
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001E15A File Offset: 0x0001C35A
		public virtual void CollectLoots(MapEvent mapEvent, PartyBase party, Dictionary<PartyBase, ItemRoster> loot, ItemRoster gainedLoot, MBList<TroopRosterElement> lootedCasualties, float lootAmount)
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001E15C File Offset: 0x0001C35C
		public virtual void OnLootDistributedToParty(MapEvent mapEvent, PartyBase party, Dictionary<PartyBase, ItemRoster> loot)
		{
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001E15E File Offset: 0x0001C35E
		public virtual void OnPlayerJoinedTournament(Town town, bool isParticipant)
		{
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001E160 File Offset: 0x0001C360
		public virtual void OnHeroUnregistered(Hero hero)
		{
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001E162 File Offset: 0x0001C362
		public virtual void OnConfigChanged()
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001E164 File Offset: 0x0001C364
		public virtual void OnCraftingOrderCompleted(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero)
		{
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001E166 File Offset: 0x0001C366
		public virtual void OnItemsRefined(Hero hero, Crafting.RefiningFormula refineFormula)
		{
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001E168 File Offset: 0x0001C368
		public virtual void OnMapEventContinuityNeedsUpdate(IFaction faction)
		{
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001E16A File Offset: 0x0001C36A
		public virtual void CanHeroLeadParty(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001E16C File Offset: 0x0001C36C
		public virtual void OnMainPartyStarving()
		{
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001E16E File Offset: 0x0001C36E
		public virtual void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001E170 File Offset: 0x0001C370
		public virtual void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001E172 File Offset: 0x0001C372
		public virtual void CanHaveQuestsOrIssues(Hero hero, ref bool result)
		{
		}
	}
}
