using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000168 RID: 360
	public abstract class EncounterModel : GameModel
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060018FB RID: 6395
		public abstract float EstimatedMaximumMobilePartySpeedExceptPlayer { get; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060018FC RID: 6396
		public abstract float NeededMaximumDistanceForEncounteringMobileParty { get; }

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060018FD RID: 6397
		public abstract float MaximumAllowedDistanceForEncounteringMobilePartyInArmy { get; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060018FE RID: 6398
		public abstract float NeededMaximumDistanceForEncounteringTown { get; }

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060018FF RID: 6399
		public abstract float NeededMaximumDistanceForEncounteringVillage { get; }

		// Token: 0x06001900 RID: 6400
		public abstract bool IsEncounterExemptFromHostileActions(PartyBase side1, PartyBase side2);

		// Token: 0x06001901 RID: 6401
		public abstract Hero GetLeaderOfSiegeEvent(SiegeEvent siegeEvent, BattleSideEnum side);

		// Token: 0x06001902 RID: 6402
		public abstract Hero GetLeaderOfMapEvent(MapEvent mapEvent, BattleSideEnum side);

		// Token: 0x06001903 RID: 6403
		public abstract int GetCharacterSergeantScore(Hero hero);

		// Token: 0x06001904 RID: 6404
		public abstract IEnumerable<PartyBase> GetDefenderPartiesOfSettlement(Settlement settlement, MapEvent.BattleTypes mapEventType);

		// Token: 0x06001905 RID: 6405
		public abstract PartyBase GetNextDefenderPartyOfSettlement(Settlement settlement, ref int partyIndex, MapEvent.BattleTypes mapEventType);

		// Token: 0x06001906 RID: 6406
		public abstract MapEventComponent CreateMapEventComponentForEncounter(PartyBase attackerParty, PartyBase defenderParty, MapEvent.BattleTypes battleType);
	}
}
