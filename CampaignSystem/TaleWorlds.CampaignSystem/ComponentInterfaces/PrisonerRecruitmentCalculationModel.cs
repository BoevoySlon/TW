using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BC RID: 444
	public abstract class PrisonerRecruitmentCalculationModel : GameModel
	{
		// Token: 0x06001B7B RID: 7035
		public abstract int GetConformityNeededToRecruitPrisoner(CharacterObject character);

		// Token: 0x06001B7C RID: 7036
		public abstract int GetConformityChangePerHour(PartyBase party, CharacterObject character);

		// Token: 0x06001B7D RID: 7037
		public abstract int GetPrisonerRecruitmentMoraleEffect(PartyBase party, CharacterObject character, int num);

		// Token: 0x06001B7E RID: 7038
		public abstract bool IsPrisonerRecruitable(PartyBase party, CharacterObject character, out int conformityNeeded);

		// Token: 0x06001B7F RID: 7039
		public abstract bool ShouldPartyRecruitPrisoners(PartyBase party);

		// Token: 0x06001B80 RID: 7040
		public abstract int CalculateRecruitableNumber(PartyBase party, CharacterObject character);
	}
}
