using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Encounters
{
	// Token: 0x02000297 RID: 663
	public class VillageEncounter : LocationEncounter
	{
		// Token: 0x0600246D RID: 9325 RVA: 0x0009AE38 File Offset: 0x00099038
		public VillageEncounter(Settlement settlement) : base(settlement)
		{
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0009AE44 File Offset: 0x00099044
		public override IMission CreateAndOpenMissionController(Location nextLocation, Location previousLocation = null, CharacterObject talkToChar = null, string playerSpecialSpawnTag = null)
		{
			IMission result = null;
			if (nextLocation.StringId == "village_center")
			{
				result = CampaignMission.OpenVillageMission(nextLocation.GetSceneName(1), nextLocation, talkToChar);
			}
			return result;
		}
	}
}
