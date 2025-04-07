using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C8 RID: 456
	public abstract class AlleyModel : GameModel
	{
		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001BC4 RID: 7108
		public abstract CampaignTime DestroyAlleyAfterDaysWhenLeaderIsDeath { get; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001BC5 RID: 7109
		public abstract int MinimumTroopCountInPlayerOwnedAlley { get; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001BC6 RID: 7110
		public abstract int MaximumTroopCountInPlayerOwnedAlley { get; }

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001BC7 RID: 7111
		public abstract float GetDailyCrimeRatingOfAlley { get; }

		// Token: 0x06001BC8 RID: 7112
		public abstract float GetDailyXpGainForAssignedClanMember(Hero assignedHero);

		// Token: 0x06001BC9 RID: 7113
		public abstract float GetDailyXpGainForMainHero();

		// Token: 0x06001BCA RID: 7114
		public abstract float GetInitialXpGainForMainHero();

		// Token: 0x06001BCB RID: 7115
		public abstract float GetXpGainAfterSuccessfulAlleyDefenseForMainHero();

		// Token: 0x06001BCC RID: 7116
		public abstract TroopRoster GetTroopsOfAIOwnedAlley(Alley alley);

		// Token: 0x06001BCD RID: 7117
		public abstract TroopRoster GetTroopsOfAlleyForBattleMission(Alley alley);

		// Token: 0x06001BCE RID: 7118
		public abstract int GetDailyIncomeOfAlley(Alley alley);

		// Token: 0x06001BCF RID: 7119
		public abstract List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(Alley alley);

		// Token: 0x06001BD0 RID: 7120
		public abstract TroopRoster GetTroopsToRecruitFromAlleyDependingOnAlleyRandom(Alley alley, float random);

		// Token: 0x06001BD1 RID: 7121
		public abstract TextObject GetDisabledReasonTextForHero(Hero hero, Alley alley, DefaultAlleyModel.AlleyMemberAvailabilityDetail detail);

		// Token: 0x06001BD2 RID: 7122
		public abstract float GetAlleyAttackResponseTimeInDays(TroopRoster troopRoster);
	}
}
