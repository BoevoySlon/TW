using System;
using System.Collections.Generic;
using System.Linq;
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
	// Token: 0x02000034 RID: 52
	public class CampaignEventDispatcher : CampaignEventReceiver
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0001B59D File Offset: 0x0001979D
		public static CampaignEventDispatcher Instance
		{
			get
			{
				Campaign campaign = Campaign.Current;
				if (campaign == null)
				{
					return null;
				}
				return campaign.CampaignEventDispatcher;
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0001B5AF File Offset: 0x000197AF
		internal CampaignEventDispatcher(IEnumerable<CampaignEventReceiver> eventReceivers)
		{
			this._eventReceivers = eventReceivers.ToArray<CampaignEventReceiver>();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001B5C4 File Offset: 0x000197C4
		internal void AddCampaignEventReceiver(CampaignEventReceiver receiver)
		{
			CampaignEventReceiver[] array = new CampaignEventReceiver[this._eventReceivers.Length + 1];
			for (int i = 0; i < this._eventReceivers.Length; i++)
			{
				array[i] = this._eventReceivers[i];
			}
			array[this._eventReceivers.Length] = receiver;
			this._eventReceivers = array;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001B614 File Offset: 0x00019814
		public override void RemoveListeners(object o)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].RemoveListeners(o);
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0001B640 File Offset: 0x00019840
		public override void OnPlayerBodyPropertiesChanged()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerBodyPropertiesChanged();
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001B66C File Offset: 0x0001986C
		public override void OnHeroLevelledUp(Hero hero, bool shouldNotify = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroLevelledUp(hero, shouldNotify);
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001B698 File Offset: 0x00019898
		public override void OnCharacterCreationIsOver()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterCreationIsOver();
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001B6C4 File Offset: 0x000198C4
		public override void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroGainedSkill(hero, skill, change, shouldNotify);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001B6F4 File Offset: 0x000198F4
		public override void OnHeroWounded(Hero woundedHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroWounded(woundedHero);
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001B720 File Offset: 0x00019920
		public override void OnHeroRelationChanged(Hero effectiveHero, Hero effectiveHeroGainedRelationWith, int relationChange, bool showNotification, ChangeRelationAction.ChangeRelationDetail detail, Hero originalHero, Hero originalGainedRelationWith)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroRelationChanged(effectiveHero, effectiveHeroGainedRelationWith, relationChange, showNotification, detail, originalHero, originalGainedRelationWith);
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0001B758 File Offset: 0x00019958
		public override void OnLootDistributedToParty(MapEvent mapEvent, PartyBase winner, Dictionary<PartyBase, ItemRoster> loot)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnLootDistributedToParty(mapEvent, winner, loot);
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001B788 File Offset: 0x00019988
		public override void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroOccupationChanged(hero, oldOccupation);
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001B7B4 File Offset: 0x000199B4
		public override void OnBarterAccepted(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBarterAccepted(offererHero, otherHero, barters);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001B7E4 File Offset: 0x000199E4
		public override void OnBarterCanceled(Hero offererHero, Hero otherHero, List<Barterable> barters)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBarterCanceled(offererHero, otherHero, barters);
			}
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001B814 File Offset: 0x00019A14
		public override void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroCreated(hero, isBornNaturally);
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001B840 File Offset: 0x00019A40
		public override void OnQuestLogAdded(QuestBase quest, bool hideInformation)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnQuestLogAdded(quest, hideInformation);
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001B86C File Offset: 0x00019A6C
		public override void OnIssueLogAdded(IssueBase issue, bool hideInformation)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnIssueLogAdded(issue, hideInformation);
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001B898 File Offset: 0x00019A98
		public override void OnClanTierChanged(Clan clan, bool shouldNotify = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanTierChanged(clan, shouldNotify);
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001B8C4 File Offset: 0x00019AC4
		public override void OnClanChangedKingdom(Clan clan, Kingdom oldKingdom, Kingdom newKingdom, ChangeKingdomAction.ChangeKingdomActionDetail actionDetail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanChangedKingdom(clan, oldKingdom, newKingdom, actionDetail, showNotification);
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001B8F8 File Offset: 0x00019AF8
		public override void OnCompanionClanCreated(Clan clan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCompanionClanCreated(clan);
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001B924 File Offset: 0x00019B24
		public override void OnHeroJoinedParty(Hero hero, MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroJoinedParty(hero, party);
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001B950 File Offset: 0x00019B50
		public override void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDecisionAdded(decision, isPlayerInvolved);
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001B97C File Offset: 0x00019B7C
		public override void OnKingdomDecisionCancelled(KingdomDecision decision, bool isPlayerInvolved)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDecisionCancelled(decision, isPlayerInvolved);
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001B9A8 File Offset: 0x00019BA8
		public override void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome chosenOutcome, bool isPlayerInvolved)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDecisionConcluded(decision, chosenOutcome, isPlayerInvolved);
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001B9D8 File Offset: 0x00019BD8
		public override void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroOrPartyTradedGold(giver, recipient, goldAmount, showNotification);
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001BA08 File Offset: 0x00019C08
		public override void OnHeroOrPartyGaveItem(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> receiver, ItemRosterElement itemRosterElement, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroOrPartyGaveItem(giver, receiver, itemRosterElement, showNotification);
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001BA38 File Offset: 0x00019C38
		public override void OnBanditPartyRecruited(MobileParty banditParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBanditPartyRecruited(banditParty);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001BA64 File Offset: 0x00019C64
		public override void OnArmyCreated(Army army)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyCreated(army);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001BA90 File Offset: 0x00019C90
		public override void OnPartyAttachedAnotherParty(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyAttachedAnotherParty(mobileParty);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001BABC File Offset: 0x00019CBC
		public override void OnNearbyPartyAddedToPlayerMapEvent(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNearbyPartyAddedToPlayerMapEvent(mobileParty);
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001BAE8 File Offset: 0x00019CE8
		public override void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyDispersed(army, reason, isPlayersArmy);
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001BB18 File Offset: 0x00019D18
		public override void OnArmyGathered(Army army, Settlement gatheringSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyGathered(army, gatheringSettlement);
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0001BB44 File Offset: 0x00019D44
		public override void OnPerkOpened(Hero hero, PerkObject perk)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPerkOpened(hero, perk);
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001BB70 File Offset: 0x00019D70
		public override void OnPlayerTraitChanged(TraitObject trait, int previousLevel)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerTraitChanged(trait, previousLevel);
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001BB9C File Offset: 0x00019D9C
		public override void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageStateChanged(village, oldState, newState, raiderParty);
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001BBCC File Offset: 0x00019DCC
		public override void OnSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSettlementEntered(party, settlement, hero);
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001BBFC File Offset: 0x00019DFC
		public override void OnAfterSettlementEntered(MobileParty party, Settlement settlement, Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAfterSettlementEntered(party, settlement, hero);
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001BC2C File Offset: 0x00019E2C
		public override void OnMercenaryTroopChangedInTown(Town town, CharacterObject oldTroopType, CharacterObject newTroopType)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMercenaryTroopChangedInTown(town, oldTroopType, newTroopType);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001BC5C File Offset: 0x00019E5C
		public override void OnMercenaryNumberChangedInTown(Town town, int oldNumber, int newNumber)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMercenaryNumberChangedInTown(town, oldNumber, newNumber);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001BC8C File Offset: 0x00019E8C
		public override void OnAlleyOccupiedByPlayer(Alley alley, TroopRoster troops)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAlleyOccupiedByPlayer(alley, troops);
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001BCB8 File Offset: 0x00019EB8
		public override void OnAlleyOwnerChanged(Alley alley, Hero newOwner, Hero oldOwner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAlleyOwnerChanged(alley, newOwner, oldOwner);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0001BCE8 File Offset: 0x00019EE8
		public override void OnAlleyClearedByPlayer(Alley alley)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAlleyClearedByPlayer(alley);
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001BD14 File Offset: 0x00019F14
		public override void OnRomanticStateChanged(Hero hero1, Hero hero2, Romance.RomanceLevelEnum romanceLevel)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRomanticStateChanged(hero1, hero2, romanceLevel);
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001BD44 File Offset: 0x00019F44
		public override void OnHeroesMarried(Hero hero1, Hero hero2, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroesMarried(hero1, hero2, showNotification);
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001BD74 File Offset: 0x00019F74
		public override void OnPlayerEliminatedFromTournament(int round, Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerEliminatedFromTournament(round, town);
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001BDA0 File Offset: 0x00019FA0
		public override void OnPlayerStartedTournamentMatch(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerStartedTournamentMatch(town);
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001BDCC File Offset: 0x00019FCC
		public override void OnTournamentStarted(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTournamentStarted(town);
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001BDF8 File Offset: 0x00019FF8
		public override void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTournamentFinished(winner, participants, town, prize);
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001BE28 File Offset: 0x0001A028
		public override void OnTournamentCancelled(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTournamentCancelled(town);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001BE54 File Offset: 0x0001A054
		public override void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWarDeclared(faction1, faction2, declareWarDetail);
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001BE84 File Offset: 0x0001A084
		public override void OnRulingClanChanged(Kingdom kingdom, Clan newRulingClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRulingClanChanged(kingdom, newRulingClan);
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001BEB0 File Offset: 0x0001A0B0
		public override void OnStartBattle(PartyBase attackerParty, PartyBase defenderParty, object subject, bool showNotification)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnStartBattle(attackerParty, defenderParty, subject, showNotification);
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001BEE0 File Offset: 0x0001A0E0
		public override void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRebellionFinished(settlement, oldOwnerClan);
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001BF0C File Offset: 0x0001A10C
		public override void TownRebelliousStateChanged(Town town, bool rebelliousState)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TownRebelliousStateChanged(town, rebelliousState);
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0001BF38 File Offset: 0x0001A138
		public override void OnRebelliousClanDisbandedAtSettlement(Settlement settlement, Clan rebelliousClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRebelliousClanDisbandedAtSettlement(settlement, rebelliousClan);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001BF64 File Offset: 0x0001A164
		public override void OnItemsLooted(MobileParty mobileParty, ItemRoster items)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemsLooted(mobileParty, items);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0001BF90 File Offset: 0x0001A190
		public override void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyDestroyed(mobileParty, destroyerParty);
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001BFBC File Offset: 0x0001A1BC
		public override void OnMobilePartyCreated(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyCreated(party);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001BFE8 File Offset: 0x0001A1E8
		public override void OnMobilePartyQuestStatusChanged(MobileParty party, bool isUsedByQuest)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMobilePartyQuestStatusChanged(party, isUsedByQuest);
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001C014 File Offset: 0x0001A214
		public override void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroKilled(victim, killer, detail, showNotification);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001C044 File Offset: 0x0001A244
		public override void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeHeroKilled(victim, killer, detail, showNotification);
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001C074 File Offset: 0x0001A274
		public override void OnChildEducationCompleted(Hero hero, int age)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnChildEducationCompleted(hero, age);
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001C0A0 File Offset: 0x0001A2A0
		public override void OnHeroComesOfAge(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroComesOfAge(hero);
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001C0CC File Offset: 0x0001A2CC
		public override void OnHeroReachesTeenAge(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroReachesTeenAge(hero);
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
		public override void OnHeroGrowsOutOfInfancy(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroGrowsOutOfInfancy(hero);
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001C124 File Offset: 0x0001A324
		public override void OnCharacterDefeated(Hero winner, Hero loser)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterDefeated(winner, loser);
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001C150 File Offset: 0x0001A350
		public override void OnHeroPrisonerTaken(PartyBase capturer, Hero prisoner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroPrisonerTaken(capturer, prisoner);
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001C17C File Offset: 0x0001A37C
		public override void OnHeroPrisonerReleased(Hero prisoner, PartyBase party, IFaction capturerFaction, EndCaptivityDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroPrisonerReleased(prisoner, party, capturerFaction, detail);
			}
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001C1AC File Offset: 0x0001A3AC
		public override void OnCharacterBecameFugitive(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterBecameFugitive(hero);
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001C1D8 File Offset: 0x0001A3D8
		public override void OnPlayerLearnsAboutHero(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerLearnsAboutHero(hero);
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001C204 File Offset: 0x0001A404
		public override void OnPlayerMetHero(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerMetHero(hero);
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001C230 File Offset: 0x0001A430
		public override void OnRenownGained(Hero hero, int gainedRenown, bool doNotNotify)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRenownGained(hero, gainedRenown, doNotNotify);
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0001C260 File Offset: 0x0001A460
		public override void OnCrimeRatingChanged(IFaction kingdom, float deltaCrimeAmount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCrimeRatingChanged(kingdom, deltaCrimeAmount);
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001C28C File Offset: 0x0001A48C
		public override void OnNewCompanionAdded(Hero newCompanion)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewCompanionAdded(newCompanion);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001C2B8 File Offset: 0x0001A4B8
		public override void OnAfterMissionStarted(IMission iMission)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAfterMissionStarted(iMission);
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0001C2E4 File Offset: 0x0001A4E4
		public override void OnGameMenuOpened(MenuCallbackArgs args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameMenuOpened(args);
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001C310 File Offset: 0x0001A510
		public override void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMakePeace(side1Faction, side2Faction, detail);
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001C340 File Offset: 0x0001A540
		public override void OnKingdomDestroyed(Kingdom destroyedKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomDestroyed(destroyedKingdom);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001C36C File Offset: 0x0001A56C
		public override void CanKingdomBeDiscontinued(Kingdom kingdom, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanKingdomBeDiscontinued(kingdom, ref result);
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001C398 File Offset: 0x0001A598
		public override void OnKingdomCreated(Kingdom createdKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnKingdomCreated(createdKingdom);
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001C3C4 File Offset: 0x0001A5C4
		public override void OnVillageBecomeNormal(Village village)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageBecomeNormal(village);
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001C3F0 File Offset: 0x0001A5F0
		public override void OnVillageBeingRaided(Village village)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageBeingRaided(village);
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0001C41C File Offset: 0x0001A61C
		public override void OnVillageLooted(Village village)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVillageLooted(village);
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001C448 File Offset: 0x0001A648
		public override void OnConversationEnded(IEnumerable<CharacterObject> characters)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnConversationEnded(characters);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001C474 File Offset: 0x0001A674
		public override void OnAgentJoinedConversation(IAgent agent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAgentJoinedConversation(agent);
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
		public override void OnMapEventEnded(MapEvent mapEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapEventEnded(mapEvent);
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001C4CC File Offset: 0x0001A6CC
		public override void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapEventStarted(mapEvent, attackerParty, defenderParty);
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001C4FC File Offset: 0x0001A6FC
		public override void OnPrisonersChangeInSettlement(Settlement settlement, FlattenedTroopRoster prisonerRoster, Hero prisonerHero, bool takenFromDungeon)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonersChangeInSettlement(settlement, prisonerRoster, prisonerHero, takenFromDungeon);
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001C52C File Offset: 0x0001A72C
		public override void OnMissionStarted(IMission mission)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMissionStarted(mission);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001C558 File Offset: 0x0001A758
		public override void OnPlayerBoardGameOver(Hero opposingHero, BoardGameHelper.BoardGameState state)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerBoardGameOver(opposingHero, state);
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001C584 File Offset: 0x0001A784
		public override void OnRansomOfferedToPlayer(Hero captiveHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRansomOfferedToPlayer(captiveHero);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001C5B0 File Offset: 0x0001A7B0
		public override void OnRansomOfferCancelled(Hero captiveHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnRansomOfferCancelled(captiveHero);
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001C5DC File Offset: 0x0001A7DC
		public override void OnPeaceOfferedToPlayer(IFaction opponentFaction, int tributeAmount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPeaceOfferedToPlayer(opponentFaction, tributeAmount);
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001C608 File Offset: 0x0001A808
		public override void OnPeaceOfferCancelled(IFaction opponentFaction)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPeaceOfferCancelled(opponentFaction);
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001C634 File Offset: 0x0001A834
		public override void OnMarriageOfferedToPlayer(Hero suitor, Hero maiden)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMarriageOfferedToPlayer(suitor, maiden);
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001C660 File Offset: 0x0001A860
		public override void OnMarriageOfferCanceled(Hero suitor, Hero maiden)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMarriageOfferCanceled(suitor, maiden);
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001C68C File Offset: 0x0001A88C
		public override void OnVassalOrMercenaryServiceOfferedToPlayer(Kingdom offeredKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVassalOrMercenaryServiceOfferedToPlayer(offeredKingdom);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001C6B8 File Offset: 0x0001A8B8
		public override void OnCommonAreaStateChanged(Alley alley, Alley.AreaState oldState, Alley.AreaState newState)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCommonAreaStateChanged(alley, oldState, newState);
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001C6E8 File Offset: 0x0001A8E8
		public override void OnVassalOrMercenaryServiceOfferCanceled(Kingdom offeredKingdom)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnVassalOrMercenaryServiceOfferCanceled(offeredKingdom);
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001C714 File Offset: 0x0001A914
		public override void BeforeMissionOpened()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].BeforeMissionOpened();
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001C740 File Offset: 0x0001A940
		public override void OnPartyRemoved(PartyBase party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyRemoved(party);
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001C76C File Offset: 0x0001A96C
		public override void OnPartySizeChanged(PartyBase party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartySizeChanged(party);
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0001C798 File Offset: 0x0001A998
		public override void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSettlementOwnerChanged(settlement, openToClaim, newOwner, oldOwner, capturerHero, detail);
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001C7CC File Offset: 0x0001A9CC
		public override void OnGovernorChanged(Town fortification, Hero oldGovernor, Hero newGovernor)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGovernorChanged(fortification, oldGovernor, newGovernor);
			}
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001C7FC File Offset: 0x0001A9FC
		public override void OnSettlementLeft(MobileParty party, Settlement settlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSettlementLeft(party, settlement);
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001C828 File Offset: 0x0001AA28
		public override void Tick(float dt)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].Tick(dt);
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001C854 File Offset: 0x0001AA54
		public override void OnSessionStart(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSessionStart(campaignGameStarter);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0001C880 File Offset: 0x0001AA80
		public override void OnAfterSessionStart(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnAfterSessionStart(campaignGameStarter);
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0001C8AC File Offset: 0x0001AAAC
		public override void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewGameCreated(campaignGameStarter);
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001C8D8 File Offset: 0x0001AAD8
		public override void OnGameEarlyLoaded(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameEarlyLoaded(campaignGameStarter);
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001C904 File Offset: 0x0001AB04
		public override void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameLoaded(campaignGameStarter);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001C930 File Offset: 0x0001AB30
		public override void OnGameLoadFinished()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameLoadFinished();
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001C95C File Offset: 0x0001AB5C
		public override void OnPartyJoinedArmy(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyJoinedArmy(mobileParty);
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001C988 File Offset: 0x0001AB88
		public override void OnPartyRemovedFromArmy(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyRemovedFromArmy(mobileParty);
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001C9B4 File Offset: 0x0001ABB4
		public override void OnArmyLeaderThink(Hero hero, Army.ArmyLeaderThinkReason reason)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyLeaderThink(hero, reason);
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001C9E0 File Offset: 0x0001ABE0
		public override void OnArmyOverlaySetDirty()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnArmyOverlaySetDirty();
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001CA0C File Offset: 0x0001AC0C
		public override void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerDesertedBattle(sacrificedMenCount);
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001CA38 File Offset: 0x0001AC38
		public override void MissionTick(float dt)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].MissionTick(dt);
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001CA64 File Offset: 0x0001AC64
		public override void OnChildConceived(Hero mother)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnChildConceived(mother);
			}
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001CA90 File Offset: 0x0001AC90
		public override void OnGivenBirth(Hero mother, List<Hero> aliveChildren, int stillbornCount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGivenBirth(mother, aliveChildren, stillbornCount);
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001CAC0 File Offset: 0x0001ACC0
		public override void OnUnitRecruited(CharacterObject character, int amount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnUnitRecruited(character, amount);
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001CAEC File Offset: 0x0001ACEC
		public override void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerBattleEnd(mapEvent);
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001CB18 File Offset: 0x0001AD18
		public override void OnMissionEnded(IMission mission)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMissionEnded(mission);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001CB44 File Offset: 0x0001AD44
		public override void TickPartialHourlyAi(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TickPartialHourlyAi(party);
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001CB70 File Offset: 0x0001AD70
		public override void QuarterDailyPartyTick(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].QuarterDailyPartyTick(party);
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001CB9C File Offset: 0x0001AD9C
		public override void AiHourlyTick(MobileParty party, PartyThinkParams partyThinkParams)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].AiHourlyTick(party, partyThinkParams);
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001CBC8 File Offset: 0x0001ADC8
		public override void HourlyTick()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTick();
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001CBF4 File Offset: 0x0001ADF4
		public override void HourlyTickParty(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTickParty(mobileParty);
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001CC20 File Offset: 0x0001AE20
		public override void HourlyTickSettlement(Settlement settlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTickSettlement(settlement);
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001CC4C File Offset: 0x0001AE4C
		public override void HourlyTickClan(Clan clan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].HourlyTickClan(clan);
			}
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001CC78 File Offset: 0x0001AE78
		public override void DailyTick()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTick();
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001CCA4 File Offset: 0x0001AEA4
		public override void DailyTickParty(MobileParty mobileParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickParty(mobileParty);
			}
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001CCD0 File Offset: 0x0001AED0
		public override void DailyTickTown(Town town)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickTown(town);
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001CCFC File Offset: 0x0001AEFC
		public override void DailyTickSettlement(Settlement settlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickSettlement(settlement);
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001CD28 File Offset: 0x0001AF28
		public override void DailyTickHero(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickHero(hero);
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001CD54 File Offset: 0x0001AF54
		public override void DailyTickClan(Clan clan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].DailyTickClan(clan);
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001CD80 File Offset: 0x0001AF80
		public override void WeeklyTick()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].WeeklyTick();
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001CDAC File Offset: 0x0001AFAC
		public override void CollectAvailableTutorials(ref List<CampaignTutorial> tutorials)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CollectAvailableTutorials(ref tutorials);
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001CDD8 File Offset: 0x0001AFD8
		public override void OnTutorialCompleted(string tutorial)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTutorialCompleted(tutorial);
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001CE04 File Offset: 0x0001B004
		public override void BeforeGameMenuOpened(MenuCallbackArgs args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].BeforeGameMenuOpened(args);
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001CE30 File Offset: 0x0001B030
		public override void AfterGameMenuOpened(MenuCallbackArgs args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].AfterGameMenuOpened(args);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001CE5C File Offset: 0x0001B05C
		public override void OnBarterablesRequested(BarterData args)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBarterablesRequested(args);
			}
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001CE88 File Offset: 0x0001B088
		public override void OnPartyVisibilityChanged(PartyBase party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyVisibilityChanged(party);
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001CEB4 File Offset: 0x0001B0B4
		public override void OnCompanionRemoved(Hero companion, RemoveCompanionAction.RemoveCompanionDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCompanionRemoved(companion, detail);
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001CEE0 File Offset: 0x0001B0E0
		public override void TrackDetected(Track track)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TrackDetected(track);
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001CF0C File Offset: 0x0001B10C
		public override void TrackLost(Track track)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].TrackLost(track);
			}
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001CF38 File Offset: 0x0001B138
		public override void LocationCharactersAreReadyToSpawn(Dictionary<string, int> unusedUsablePointCount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].LocationCharactersAreReadyToSpawn(unusedUsablePointCount);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001CF64 File Offset: 0x0001B164
		public override void LocationCharactersSimulated()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].LocationCharactersSimulated();
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001CF90 File Offset: 0x0001B190
		public override void OnPlayerUpgradedTroops(CharacterObject upgradeFromTroop, CharacterObject upgradeToTroop, int number)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerUpgradedTroops(upgradeFromTroop, upgradeToTroop, number);
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001CFC0 File Offset: 0x0001B1C0
		public override void OnHeroCombatHit(CharacterObject attackerTroop, CharacterObject attackedTroop, PartyBase party, WeaponComponentData usedWeapon, bool isFatal, int xp)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroCombatHit(attackerTroop, attackedTroop, party, usedWeapon, isFatal, xp);
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001CFF4 File Offset: 0x0001B1F4
		public override void OnCharacterPortraitPopUpOpened(CharacterObject character)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterPortraitPopUpOpened(character);
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001D020 File Offset: 0x0001B220
		public override void OnCharacterPortraitPopUpClosed()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCharacterPortraitPopUpClosed();
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001D04C File Offset: 0x0001B24C
		public override void OnPlayerStartTalkFromMenu(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerStartTalkFromMenu(hero);
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001D078 File Offset: 0x0001B278
		public override void OnGameMenuOptionSelected(GameMenuOption gameMenuOption)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameMenuOptionSelected(gameMenuOption);
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001D0A4 File Offset: 0x0001B2A4
		public override void OnPlayerStartRecruitment(CharacterObject recruitTroopCharacter)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerStartRecruitment(recruitTroopCharacter);
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001D0D0 File Offset: 0x0001B2D0
		public override void OnBeforePlayerCharacterChanged(Hero oldPlayer, Hero newPlayer)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforePlayerCharacterChanged(oldPlayer, newPlayer);
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001D0FC File Offset: 0x0001B2FC
		public override void OnPlayerCharacterChanged(Hero oldPlayer, Hero newPlayer, MobileParty newPlayerParty, bool isMainPartyChanged)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerCharacterChanged(oldPlayer, newPlayer, newPlayerParty, isMainPartyChanged);
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001D12C File Offset: 0x0001B32C
		public override void OnClanLeaderChanged(Hero oldLeader, Hero newLeader)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanLeaderChanged(oldLeader, newLeader);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001D158 File Offset: 0x0001B358
		public override void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeEventStarted(siegeEvent);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001D184 File Offset: 0x0001B384
		public override void OnPlayerSiegeStarted()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerSiegeStarted();
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001D1B0 File Offset: 0x0001B3B0
		public override void OnSiegeEventEnded(SiegeEvent siegeEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeEventEnded(siegeEvent);
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001D1DC File Offset: 0x0001B3DC
		public override void OnSiegeAftermathApplied(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeAftermathApplied(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001D210 File Offset: 0x0001B410
		public override void OnSiegeBombardmentHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, SiegeBombardTargets target)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeBombardmentHit(besiegerParty, besiegedSettlement, side, weapon, target);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001D244 File Offset: 0x0001B444
		public override void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeBombardmentWallHit(besiegerParty, besiegedSettlement, side, weapon, isWallCracked);
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001D278 File Offset: 0x0001B478
		public override void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSiegeEngineDestroyed(besiegerParty, besiegedSettlement, side, destroyedEngine);
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001D2A8 File Offset: 0x0001B4A8
		public override void OnTradeRumorIsTaken(List<TradeRumor> newRumors, Settlement sourceSettlement = null)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTradeRumorIsTaken(newRumors, sourceSettlement);
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001D2D4 File Offset: 0x0001B4D4
		public override void OnCheckForIssue(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCheckForIssue(hero);
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001D300 File Offset: 0x0001B500
		public override void OnIssueUpdated(IssueBase issue, IssueBase.IssueUpdateDetails details, Hero issueSolver)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnIssueUpdated(issue, details, issueSolver);
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001D330 File Offset: 0x0001B530
		public override void OnTroopsDeserted(MobileParty mobileParty, TroopRoster desertedTroops)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTroopsDeserted(mobileParty, desertedTroops);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001D35C File Offset: 0x0001B55C
		public override void OnTroopRecruited(Hero recruiterHero, Settlement recruitmentSettlement, Hero recruitmentSource, CharacterObject troop, int amount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTroopRecruited(recruiterHero, recruitmentSettlement, recruitmentSource, troop, amount);
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001D390 File Offset: 0x0001B590
		public override void OnTroopGivenToSettlement(Hero giverHero, Settlement recipientSettlement, TroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnTroopGivenToSettlement(giverHero, recipientSettlement, roster);
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
		public override void OnItemSold(PartyBase receiverParty, PartyBase payerParty, ItemRosterElement itemRosterElement, int number, Settlement currentSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemSold(receiverParty, payerParty, itemRosterElement, number, currentSettlement);
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001D3F4 File Offset: 0x0001B5F4
		public override void OnCaravanTransactionCompleted(MobileParty caravanParty, Town town, List<ValueTuple<EquipmentElement, int>> itemRosterElements)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCaravanTransactionCompleted(caravanParty, town, itemRosterElements);
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001D424 File Offset: 0x0001B624
		public override void OnPrisonerSold(PartyBase sellerParty, PartyBase buyerParty, TroopRoster prisoners)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerSold(sellerParty, buyerParty, prisoners);
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001D454 File Offset: 0x0001B654
		public override void OnPartyDisbanded(MobileParty disbandParty, Settlement relatedSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyDisbanded(disbandParty, relatedSettlement);
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001D480 File Offset: 0x0001B680
		public override void OnPartyDisbandStarted(MobileParty disbandParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyDisbandStarted(disbandParty);
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001D4AC File Offset: 0x0001B6AC
		public override void OnPartyDisbandCanceled(MobileParty disbandParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyDisbandCanceled(disbandParty);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001D4D8 File Offset: 0x0001B6D8
		public override void OnBuildingLevelChanged(Town town, Building building, int levelChange)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBuildingLevelChanged(town, building, levelChange);
			}
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001D508 File Offset: 0x0001B708
		public override void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHideoutSpotted(party, hideoutParty);
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001D534 File Offset: 0x0001B734
		public override void OnHideoutDeactivated(Settlement hideout)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHideoutDeactivated(hideout);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001D560 File Offset: 0x0001B760
		public override void OnHeroSharedFoodWithAnother(Hero supporterHero, Hero supportedHero, float influence)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroSharedFoodWithAnother(supporterHero, supportedHero, influence);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001D590 File Offset: 0x0001B790
		public override void OnItemsDiscardedByPlayer(ItemRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemsDiscardedByPlayer(roster);
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001D5BC File Offset: 0x0001B7BC
		public override void OnPlayerInventoryExchange(List<ValueTuple<ItemRosterElement, int>> purchasedItems, List<ValueTuple<ItemRosterElement, int>> soldItems, bool isTrading)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerInventoryExchange(purchasedItems, soldItems, isTrading);
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001D5EC File Offset: 0x0001B7EC
		public override void OnPersuasionProgressCommitted(Tuple<PersuasionOptionArgs, PersuasionOptionResult> progress)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPersuasionProgressCommitted(progress);
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001D618 File Offset: 0x0001B818
		public override void OnQuestCompleted(QuestBase quest, QuestBase.QuestCompleteDetails detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnQuestCompleted(quest, detail);
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001D644 File Offset: 0x0001B844
		public override void OnQuestStarted(QuestBase quest)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnQuestStarted(quest);
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001D670 File Offset: 0x0001B870
		public override void OnItemProduced(ItemObject itemObject, Settlement settlement, int count)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemProduced(itemObject, settlement, count);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001D6A0 File Offset: 0x0001B8A0
		public override void OnItemConsumed(ItemObject itemObject, Settlement settlement, int count)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemConsumed(itemObject, settlement, count);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001D6D0 File Offset: 0x0001B8D0
		public override void OnPartyConsumedFood(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyConsumedFood(party);
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001D6FC File Offset: 0x0001B8FC
		public override void OnNewIssueCreated(IssueBase issue)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewIssueCreated(issue);
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001D728 File Offset: 0x0001B928
		public override void OnIssueOwnerChanged(IssueBase issue, Hero oldOwner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnIssueOwnerChanged(issue, oldOwner);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001D754 File Offset: 0x0001B954
		public override void OnBeforeMainCharacterDied(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeMainCharacterDied(victim, killer, detail, showNotification);
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001D784 File Offset: 0x0001B984
		public override void OnGameOver()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnGameOver();
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001D7B0 File Offset: 0x0001B9B0
		public override void SiegeCompleted(Settlement siegeSettlement, MobileParty attackerParty, bool isWin, MapEvent.BattleTypes battleType)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].SiegeCompleted(siegeSettlement, attackerParty, isWin, battleType);
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001D7E0 File Offset: 0x0001B9E0
		public override void SiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].SiegeEngineBuilt(siegeEvent, side, siegeEngine);
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001D810 File Offset: 0x0001BA10
		public override void RaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].RaidCompleted(winnerSide, raidEvent);
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001D83C File Offset: 0x0001BA3C
		public override void ForceSuppliesCompleted(BattleSideEnum winnerSide, ForceSuppliesEventComponent forceSuppliesEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].ForceSuppliesCompleted(winnerSide, forceSuppliesEvent);
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001D868 File Offset: 0x0001BA68
		public override void ForceVolunteersCompleted(BattleSideEnum winnerSide, ForceVolunteersEventComponent forceVolunteersEvent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].ForceVolunteersCompleted(winnerSide, forceVolunteersEvent);
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001D894 File Offset: 0x0001BA94
		public override void OnHideoutBattleCompleted(BattleSideEnum winnerSide, HideoutEventComponent hideoutEventComponent)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHideoutBattleCompleted(winnerSide, hideoutEventComponent);
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001D8C0 File Offset: 0x0001BAC0
		public override void OnClanDestroyed(Clan destroyedClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanDestroyed(destroyedClan);
			}
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001D8EC File Offset: 0x0001BAEC
		public override void OnNewItemCrafted(ItemObject itemObject, ItemModifier overriddenItemModifier, bool isCraftingOrderItem)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnNewItemCrafted(itemObject, overriddenItemModifier, isCraftingOrderItem);
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001D91C File Offset: 0x0001BB1C
		public override void OnWorkshopInitialized(Workshop workshop)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWorkshopInitialized(workshop);
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0001D948 File Offset: 0x0001BB48
		public override void OnWorkshopOwnerChanged(Workshop workshop, Hero oldOwner)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWorkshopOwnerChanged(workshop, oldOwner);
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001D974 File Offset: 0x0001BB74
		public override void OnWorkshopTypeChanged(Workshop workshop)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnWorkshopTypeChanged(workshop);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0001D9A0 File Offset: 0x0001BBA0
		public override void OnMainPartyPrisonerRecruited(FlattenedTroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMainPartyPrisonerRecruited(roster);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001D9CC File Offset: 0x0001BBCC
		public override void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerDonatedToSettlement(donatingParty, donatedPrisoners, donatedSettlement);
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001D9FC File Offset: 0x0001BBFC
		public override void OnEquipmentSmeltedByHero(Hero hero, EquipmentElement equipmentElement)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnEquipmentSmeltedByHero(hero, equipmentElement);
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001DA28 File Offset: 0x0001BC28
		public override void OnPrisonerTaken(FlattenedTroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerTaken(roster);
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001DA54 File Offset: 0x0001BC54
		public override void OnBeforeSave()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnBeforeSave();
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001DA80 File Offset: 0x0001BC80
		public override void OnSaveStarted()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSaveStarted();
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001DAAC File Offset: 0x0001BCAC
		public override void OnSaveOver(bool isSuccessful, string saveName)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnSaveOver(isSuccessful, saveName);
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001DAD8 File Offset: 0x0001BCD8
		public override void OnPrisonerReleased(FlattenedTroopRoster roster)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPrisonerReleased(roster);
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001DB04 File Offset: 0x0001BD04
		public override void OnHeroChangedClan(Hero hero, Clan oldClan)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroChangedClan(hero, oldClan);
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001DB30 File Offset: 0x0001BD30
		public override void OnHeroGetsBusy(Hero hero, HeroGetsBusyReasons heroGetsBusyReason)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroGetsBusy(hero, heroGetsBusyReason);
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001DB5C File Offset: 0x0001BD5C
		public override void OnPlayerTradeProfit(int profit)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerTradeProfit(profit);
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001DB88 File Offset: 0x0001BD88
		public override void CraftingPartUnlocked(CraftingPiece craftingPiece)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CraftingPartUnlocked(craftingPiece);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001DBB4 File Offset: 0x0001BDB4
		public override void CollectLoots(MapEvent mapEvent, PartyBase winner, Dictionary<PartyBase, ItemRoster> baseAndLootedItems, ItemRoster gainedLoot, MBList<TroopRosterElement> lootedCasualties, float lootAmount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CollectLoots(mapEvent, winner, baseAndLootedItems, gainedLoot, lootedCasualties, lootAmount);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
		public override void OnHeroTeleportationRequested(Hero hero, Settlement targetSettlement, MobileParty targetParty, TeleportHeroAction.TeleportationDetail detail)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroTeleportationRequested(hero, targetSettlement, targetParty, detail);
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001DC18 File Offset: 0x0001BE18
		public override void OnClanInfluenceChanged(Clan clan, float change)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnClanInfluenceChanged(clan, change);
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001DC44 File Offset: 0x0001BE44
		public override void OnPlayerPartyKnockedOrKilledTroop(CharacterObject strikedTroop)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerPartyKnockedOrKilledTroop(strikedTroop);
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001DC70 File Offset: 0x0001BE70
		public override void OnPlayerEarnedGoldFromAsset(DefaultClanFinanceModel.AssetIncomeType incomeType, int incomeAmount)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerEarnedGoldFromAsset(incomeType, incomeAmount);
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0001DC9C File Offset: 0x0001BE9C
		public override void OnPartyLeaderChangeOfferCanceled(MobileParty party)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPartyLeaderChangeOfferCanceled(party);
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001DCC8 File Offset: 0x0001BEC8
		public override void OnMainPartyStarving()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMainPartyStarving();
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001DCF4 File Offset: 0x0001BEF4
		public override void OnPlayerJoinedTournament(Town town, bool isParticipant)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnPlayerJoinedTournament(town, isParticipant);
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001DD20 File Offset: 0x0001BF20
		public override void OnCraftingOrderCompleted(Town town, CraftingOrder craftingOrder, ItemObject craftedItem, Hero completerHero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnCraftingOrderCompleted(town, craftingOrder, craftedItem, completerHero);
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001DD50 File Offset: 0x0001BF50
		public override void OnItemsRefined(Hero hero, Crafting.RefiningFormula refineFormula)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnItemsRefined(hero, refineFormula);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001DD7C File Offset: 0x0001BF7C
		public override void OnMapEventContinuityNeedsUpdate(IFaction faction)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnMapEventContinuityNeedsUpdate(faction);
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001DDA8 File Offset: 0x0001BFA8
		public override void CanHeroLeadParty(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroLeadParty(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001DDDC File Offset: 0x0001BFDC
		public override void CanHeroMarry(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroMarry(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001DE10 File Offset: 0x0001C010
		public override void CanHeroEquipmentBeChanged(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroEquipmentBeChanged(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001DE44 File Offset: 0x0001C044
		public override void CanBeGovernorOrHavePartyRole(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanBeGovernorOrHavePartyRole(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001DE78 File Offset: 0x0001C078
		public override void CanHeroDie(Hero hero, KillCharacterAction.KillCharacterActionDetail causeOfDeath, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroDie(hero, causeOfDeath, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001DEAC File Offset: 0x0001C0AC
		public override void CanHeroBecomePrisoner(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHeroBecomePrisoner(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001DEE0 File Offset: 0x0001C0E0
		public override void CanMoveToSettlement(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanMoveToSettlement(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001DF14 File Offset: 0x0001C114
		public override void CanHaveQuestsOrIssues(Hero hero, ref bool result)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].CanHaveQuestsOrIssues(hero, ref result);
				if (!result)
				{
					return;
				}
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001DF48 File Offset: 0x0001C148
		public override void OnHeroUnregistered(Hero hero)
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnHeroUnregistered(hero);
			}
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001DF74 File Offset: 0x0001C174
		public override void OnConfigChanged()
		{
			CampaignEventReceiver[] eventReceivers = this._eventReceivers;
			for (int i = 0; i < eventReceivers.Length; i++)
			{
				eventReceivers[i].OnConfigChanged();
			}
		}

		// Token: 0x0400017D RID: 381
		private CampaignEventReceiver[] _eventReceivers;
	}
}
