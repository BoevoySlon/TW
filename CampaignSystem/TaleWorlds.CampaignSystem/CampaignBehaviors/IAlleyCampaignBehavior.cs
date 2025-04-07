using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000397 RID: 919
	public interface IAlleyCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x0600376D RID: 14189
		bool GetIsAlleyUnderAttack(Alley alley);

		// Token: 0x0600376E RID: 14190
		int GetPlayerOwnedAlleyTroopCount(Alley alley);

		// Token: 0x0600376F RID: 14191
		int GetResponseTimeLeftForAttackInDays(Alley alley);

		// Token: 0x06003770 RID: 14192
		void AbandonAlleyFromClanMenu(Alley alley);

		// Token: 0x06003771 RID: 14193
		Hero GetAssignedClanMemberOfAlley(Alley alley);

		// Token: 0x06003772 RID: 14194
		bool IsHeroAlleyLeaderOfAnyPlayerAlley(Hero hero);

		// Token: 0x06003773 RID: 14195
		List<Hero> GetAllAssignedClanMembersForOwnedAlleys();

		// Token: 0x06003774 RID: 14196
		void ChangeAlleyMember(Alley alley, Hero newAlleyLead);

		// Token: 0x06003775 RID: 14197
		void OnPlayerRetreatedFromMission();

		// Token: 0x06003776 RID: 14198
		void OnPlayerDiedInMission();
	}
}
