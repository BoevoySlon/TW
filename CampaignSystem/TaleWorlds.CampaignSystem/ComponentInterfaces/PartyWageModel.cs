using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000194 RID: 404
	public abstract class PartyWageModel : GameModel
	{
		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001A5D RID: 6749
		public abstract int MaxWage { get; }

		// Token: 0x06001A5E RID: 6750
		public abstract int GetCharacterWage(CharacterObject character);

		// Token: 0x06001A5F RID: 6751
		public abstract ExplainedNumber GetTotalWage(MobileParty mobileParty, bool includeDescriptions = false);

		// Token: 0x06001A60 RID: 6752
		public abstract int GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false);
	}
}
