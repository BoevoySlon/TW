using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BB RID: 443
	public abstract class IssueModel : GameModel
	{
		// Token: 0x06001B70 RID: 7024
		public abstract float GetIssueDifficultyMultiplier();

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06001B71 RID: 7025
		public abstract int IssueOwnerCoolDownInDays { get; }

		// Token: 0x06001B72 RID: 7026
		public abstract void GetIssueEffectsOfSettlement(IssueEffect issueEffect, Settlement settlement, ref ExplainedNumber explainedNumber);

		// Token: 0x06001B73 RID: 7027
		public abstract void GetIssueEffectOfHero(IssueEffect issueEffect, Hero hero, ref ExplainedNumber explainedNumber);

		// Token: 0x06001B74 RID: 7028
		public abstract void GetIssueEffectOfClan(IssueEffect issueEffect, Clan clan, ref ExplainedNumber explainedNumber);

		// Token: 0x06001B75 RID: 7029
		public abstract ValueTuple<int, int> GetCausalityForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001B76 RID: 7030
		public abstract float GetFailureRiskForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001B77 RID: 7031
		public abstract CampaignTime GetDurationOfResolutionForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001B78 RID: 7032
		public abstract int GetTroopsRequiredForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001B79 RID: 7033
		public abstract ValueTuple<SkillObject, int> GetIssueAlternativeSolutionSkill(Hero hero, IssueBase issue);
	}
}
