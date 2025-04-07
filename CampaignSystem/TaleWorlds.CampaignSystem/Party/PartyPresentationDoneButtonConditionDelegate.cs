using System;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002A1 RID: 673
	// (Invoke) Token: 0x060026C6 RID: 9926
	public delegate Tuple<bool, TextObject> PartyPresentationDoneButtonConditionDelegate(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum);
}
