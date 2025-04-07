using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000021 RID: 33
	public struct BattleResultPartyData
	{
		// Token: 0x0600014C RID: 332 RVA: 0x0000EBA3 File Offset: 0x0000CDA3
		public BattleResultPartyData(PartyBase party)
		{
			this.Party = party;
			this.Characters = new List<CharacterObject>();
		}

		// Token: 0x04000028 RID: 40
		public readonly PartyBase Party;

		// Token: 0x04000029 RID: 41
		public readonly List<CharacterObject> Characters;
	}
}
