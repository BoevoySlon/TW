using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x02000295 RID: 661
	public class RetirementEncounter : LocationEncounter
	{
		// Token: 0x06002469 RID: 9321 RVA: 0x0009AD4A File Offset: 0x00098F4A
		public RetirementEncounter(Settlement settlement) : base(settlement)
		{
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0009AD54 File Offset: 0x00098F54
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			IMission result = null;
			if (Settlement.CurrentSettlement.SettlementComponent is RetirementSettlementComponent)
			{
				int upgradeLevel = Settlement.CurrentSettlement.IsTown ? Settlement.CurrentSettlement.Town.GetWallLevel() : 1;
				result = CampaignMission.OpenRetirementMission(nextLocation.GetSceneName(upgradeLevel), nextLocation, null, null);
			}
			return result;
		}
	}
}
