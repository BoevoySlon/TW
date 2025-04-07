using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003C5 RID: 965
	public class PlayerVariablesBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003B4F RID: 15183 RVA: 0x0011A0E8 File Offset: 0x001182E8
		public override void RegisterEvents()
		{
			CampaignEvents.PlayerDesertedBattleEvent.AddNonSerializedListener(this, new Action<int>(this.OnPlayerDesertedBattle));
			CampaignEvents.VillageLooted.AddNonSerializedListener(this, new Action<Village>(this.OnVillageLooted));
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnd));
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x0011A13A File Offset: 0x0011833A
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x0011A13C File Offset: 0x0011833C
		private void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			SkillLevelingManager.OnTacticsUsed(MobileParty.MainParty, (float)(sacrificedMenCount * 50));
			TraitLevelingHelper.OnTroopsSacrificed();
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x0011A152 File Offset: 0x00118352
		private void OnVillageLooted(Village village)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.PlayerIsAttacker && PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.Village == village)
			{
				TraitLevelingHelper.OnVillageRaided();
			}
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x0011A17C File Offset: 0x0011837C
		private void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			float playerPartyContributionRate = (mapEvent.AttackerSide.IsMainPartyAmongParties() ? mapEvent.AttackerSide : mapEvent.DefenderSide).GetPlayerPartyContributionRate();
			TraitLevelingHelper.OnBattleWon(mapEvent, playerPartyContributionRate);
		}
	}
}
