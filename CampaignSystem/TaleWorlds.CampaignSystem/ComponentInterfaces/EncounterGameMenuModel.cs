using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018F RID: 399
	public abstract class EncounterGameMenuModel : GameModel
	{
		// Token: 0x06001A49 RID: 6729
		public abstract string GetEncounterMenu(PartyBase attackerParty, PartyBase defenderParty, out bool startBattle, out bool joinBattle);

		// Token: 0x06001A4A RID: 6730
		public abstract string GetRaidCompleteMenu();

		// Token: 0x06001A4B RID: 6731
		public abstract string GetNewPartyJoinMenu(MobileParty newParty);

		// Token: 0x06001A4C RID: 6732
		public abstract string GetGenericStateMenu();
	}
}
