using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000050 RID: 80
	public static class CampaignMission
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00022D0D File Offset: 0x00020F0D
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x00022D14 File Offset: 0x00020F14
		public static ICampaignMission Current { get; set; }

		// Token: 0x060007C2 RID: 1986 RVA: 0x00022D1C File Offset: 0x00020F1C
		public static IMission OpenBattleMission(string scene, bool usesTownDecalAtlas)
		{
			return Campaign.Current.CampaignMissionManager.OpenBattleMission(scene, usesTownDecalAtlas);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00022D2F File Offset: 0x00020F2F
		public static IMission OpenAlleyFightMission(string scene, int upgradeLevel, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops)
		{
			return Campaign.Current.CampaignMissionManager.OpenAlleyFightMission(scene, upgradeLevel, location, playerSideTroops, rivalSideTroops);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00022D46 File Offset: 0x00020F46
		public static IMission OpenCombatMissionWithDialogue(string scene, CharacterObject characterToTalkTo, int upgradeLevel)
		{
			return Campaign.Current.CampaignMissionManager.OpenCombatMissionWithDialogue(scene, characterToTalkTo, upgradeLevel);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00022D5A File Offset: 0x00020F5A
		public static IMission OpenBattleMissionWhileEnteringSettlement(string scene, int upgradeLevel, int numberOfMaxTroopToBeSpawnedForPlayer, int numberOfMaxTroopToBeSpawnedForOpponent)
		{
			return Campaign.Current.CampaignMissionManager.OpenBattleMissionWhileEnteringSettlement(scene, upgradeLevel, numberOfMaxTroopToBeSpawnedForPlayer, numberOfMaxTroopToBeSpawnedForOpponent);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00022D6F File Offset: 0x00020F6F
		public static IMission OpenHideoutBattleMission(string scene, FlattenedTroopRoster playerTroops)
		{
			return Campaign.Current.CampaignMissionManager.OpenHideoutBattleMission(scene, playerTroops);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00022D84 File Offset: 0x00020F84
		public static IMission OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointsPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int upgradeLevel = 0, bool isSallyOut = false, bool isReliefForceAttack = false)
		{
			return Campaign.Current.CampaignMissionManager.OpenSiegeMissionWithDeployment(scene, wallHitPointsPercentages, hasAnySiegeTower, siegeWeaponsOfAttackers, siegeWeaponsOfDefenders, isPlayerAttacker, upgradeLevel, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00022DAE File Offset: 0x00020FAE
		public static IMission OpenSiegeMissionNoDeployment(string scene, bool isSallyOut = false, bool isReliefForceAttack = false)
		{
			return Campaign.Current.CampaignMissionManager.OpenSiegeMissionNoDeployment(scene, isSallyOut, isReliefForceAttack);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00022DC2 File Offset: 0x00020FC2
		public static IMission OpenSiegeLordsHallFightMission(string scene, FlattenedTroopRoster attackerPriorityList)
		{
			return Campaign.Current.CampaignMissionManager.OpenSiegeLordsHallFightMission(scene, attackerPriorityList);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00022DD5 File Offset: 0x00020FD5
		public static IMission OpenBattleMission(MissionInitializerRecord rec)
		{
			return Campaign.Current.CampaignMissionManager.OpenBattleMission(rec);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00022DE7 File Offset: 0x00020FE7
		public static IMission OpenCaravanBattleMission(MissionInitializerRecord rec, bool isCaravan)
		{
			return Campaign.Current.CampaignMissionManager.OpenCaravanBattleMission(rec, isCaravan);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00022DFA File Offset: 0x00020FFA
		public static IMission OpenTownCenterMission(string scene, Location location, CharacterObject talkToChar, int townUpgradeLevel, string playerSpawnTag)
		{
			return Campaign.Current.CampaignMissionManager.OpenTownCenterMission(scene, townUpgradeLevel, location, talkToChar, playerSpawnTag);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00022E11 File Offset: 0x00021011
		public static IMission OpenCastleCourtyardMission(string scene, Location location, CharacterObject talkToChar, int castleUpgradeLevel)
		{
			return Campaign.Current.CampaignMissionManager.OpenCastleCourtyardMission(scene, castleUpgradeLevel, location, talkToChar);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00022E26 File Offset: 0x00021026
		public static IMission OpenVillageMission(string scene, Location location, CharacterObject talkToChar)
		{
			return Campaign.Current.CampaignMissionManager.OpenVillageMission(scene, location, talkToChar);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00022E3A File Offset: 0x0002103A
		public static IMission OpenIndoorMission(string scene, int upgradeLevel, Location location, CharacterObject talkToChar)
		{
			return Campaign.Current.CampaignMissionManager.OpenIndoorMission(scene, upgradeLevel, location, talkToChar);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00022E4F File Offset: 0x0002104F
		public static IMission OpenPrisonBreakMission(string scene, Location location, CharacterObject prisonerCharacter, CharacterObject companionCharacter = null)
		{
			return Campaign.Current.CampaignMissionManager.OpenPrisonBreakMission(scene, location, prisonerCharacter, companionCharacter);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00022E64 File Offset: 0x00021064
		public static IMission OpenArenaStartMission(string scene, Location location, CharacterObject talkToChar)
		{
			return Campaign.Current.CampaignMissionManager.OpenArenaStartMission(scene, location, talkToChar);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00022E78 File Offset: 0x00021078
		public static IMission OpenArenaDuelMission(string scene, Location location, CharacterObject talkToChar, bool requireCivilianEquipment, bool spawnBothSidesWithHorse, Action<CharacterObject> onDuelEnd, float customAgentHealth)
		{
			return Campaign.Current.CampaignMissionManager.OpenArenaDuelMission(scene, location, talkToChar, requireCivilianEquipment, spawnBothSidesWithHorse, onDuelEnd, customAgentHealth);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00022E93 File Offset: 0x00021093
		public static IMission OpenConversationMission(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, string specialScene = "", string sceneLevels = "")
		{
			return Campaign.Current.CampaignMissionManager.OpenConversationMission(playerCharacterData, conversationPartnerData, specialScene, sceneLevels);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00022EA8 File Offset: 0x000210A8
		public static IMission OpenRetirementMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = null)
		{
			return Campaign.Current.CampaignMissionManager.OpenRetirementMission(scene, location, talkToChar, sceneLevels);
		}

		// Token: 0x02000498 RID: 1176
		public interface ICampaignMissionManager
		{
			// Token: 0x0600421E RID: 16926
			IMission OpenSiegeMissionWithDeployment(string scene, float[] wallHitPointsPercentages, bool hasAnySiegeTower, List<MissionSiegeWeapon> siegeWeaponsOfAttackers, List<MissionSiegeWeapon> siegeWeaponsOfDefenders, bool isPlayerAttacker, int upgradeLevel = 0, bool isSallyOut = false, bool isReliefForceAttack = false);

			// Token: 0x0600421F RID: 16927
			IMission OpenSiegeMissionNoDeployment(string scene, bool isSallyOut = false, bool isReliefForceAttack = false);

			// Token: 0x06004220 RID: 16928
			IMission OpenSiegeLordsHallFightMission(string scene, FlattenedTroopRoster attackerPriorityList);

			// Token: 0x06004221 RID: 16929
			IMission OpenBattleMission(MissionInitializerRecord rec);

			// Token: 0x06004222 RID: 16930
			IMission OpenCaravanBattleMission(MissionInitializerRecord rec, bool isCaravan);

			// Token: 0x06004223 RID: 16931
			IMission OpenBattleMission(string scene, bool usesTownDecalAtlas);

			// Token: 0x06004224 RID: 16932
			IMission OpenHideoutBattleMission(string scene, FlattenedTroopRoster playerTroops);

			// Token: 0x06004225 RID: 16933
			IMission OpenTownCenterMission(string scene, int townUpgradeLevel, Location location, CharacterObject talkToChar, string playerSpawnTag);

			// Token: 0x06004226 RID: 16934
			IMission OpenCastleCourtyardMission(string scene, int castleUpgradeLevel, Location location, CharacterObject talkToChar);

			// Token: 0x06004227 RID: 16935
			IMission OpenVillageMission(string scene, Location location, CharacterObject talkToChar);

			// Token: 0x06004228 RID: 16936
			IMission OpenIndoorMission(string scene, int upgradeLevel, Location location, CharacterObject talkToChar);

			// Token: 0x06004229 RID: 16937
			IMission OpenPrisonBreakMission(string scene, Location location, CharacterObject prisonerCharacter, CharacterObject companionCharacter = null);

			// Token: 0x0600422A RID: 16938
			IMission OpenArenaStartMission(string scene, Location location, CharacterObject talkToChar);

			// Token: 0x0600422B RID: 16939
			IMission OpenArenaDuelMission(string scene, Location location, CharacterObject duelCharacter, bool requireCivilianEquipment, bool spawnBOthSidesWithHorse, Action<CharacterObject> onDuelEndAction, float customAgentHealth);

			// Token: 0x0600422C RID: 16940
			IMission OpenConversationMission(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, string specialScene = "", string sceneLevels = "");

			// Token: 0x0600422D RID: 16941
			IMission OpenMeetingMission(string scene, CharacterObject character);

			// Token: 0x0600422E RID: 16942
			IMission OpenAlleyFightMission(string scene, int upgradeLevel, Location location, TroopRoster playerSideTroops, TroopRoster rivalSideTroops);

			// Token: 0x0600422F RID: 16943
			IMission OpenCombatMissionWithDialogue(string scene, CharacterObject characterToTalkTo, int upgradeLevel);

			// Token: 0x06004230 RID: 16944
			IMission OpenBattleMissionWhileEnteringSettlement(string scene, int upgradeLevel, int numberOfMaxTroopToBeSpawnedForPlayer, int numberOfMaxTroopToBeSpawnedForOpponent);

			// Token: 0x06004231 RID: 16945
			IMission OpenRetirementMission(string scene, Location location, CharacterObject talkToChar = null, string sceneLevels = null);
		}
	}
}
