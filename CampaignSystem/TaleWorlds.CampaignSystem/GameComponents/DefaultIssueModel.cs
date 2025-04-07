using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010E RID: 270
	public class DefaultIssueModel : IssueModel
	{
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x000689E9 File Offset: 0x00066BE9
		public override int IssueOwnerCoolDownInDays
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x000689ED File Offset: 0x00066BED
		public override float GetIssueDifficultyMultiplier()
		{
			return MBMath.ClampFloat(Campaign.Current.PlayerProgress, 0.1f, 1f);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x00068A08 File Offset: 0x00066C08
		public override void GetIssueEffectsOfSettlement(IssueEffect issueEffect, Settlement settlement, ref ExplainedNumber explainedNumber)
		{
			Hero leader = settlement.OwnerClan.Leader;
			if (leader != null && leader.IsAlive && leader.Issue != null)
			{
				this.GetIssueEffectOfHeroInternal(issueEffect, leader, ref explainedNumber, DefaultIssueModel.SettlementIssuesText);
			}
			foreach (Hero hero in settlement.HeroesWithoutParty)
			{
				if (hero.Issue != null)
				{
					this.GetIssueEffectOfHeroInternal(issueEffect, hero, ref explainedNumber, DefaultIssueModel.SettlementIssuesText);
				}
			}
			if (settlement.IsTown || settlement.IsCastle)
			{
				foreach (Village village in settlement.BoundVillages)
				{
					foreach (Hero hero2 in village.Settlement.Notables)
					{
						if (hero2.Issue != null)
						{
							this.GetIssueEffectOfHeroInternal(issueEffect, hero2, ref explainedNumber, DefaultIssueModel.RelatedSettlementIssuesText);
						}
					}
				}
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x00068B3C File Offset: 0x00066D3C
		public override void GetIssueEffectOfHero(IssueEffect issueEffect, Hero hero, ref ExplainedNumber explainedNumber)
		{
			this.GetIssueEffectOfHeroInternal(issueEffect, hero, ref explainedNumber, DefaultIssueModel.HeroIssueText);
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x00068B4C File Offset: 0x00066D4C
		public override void GetIssueEffectOfClan(IssueEffect issueEffect, Clan clan, ref ExplainedNumber explainedNumber)
		{
			float num = 0f;
			foreach (Hero hero in clan.Lords)
			{
				if (hero.Issue != null)
				{
					IssueBase issue = hero.Issue;
					num += issue.GetActiveIssueEffectAmount(issueEffect);
				}
			}
			explainedNumber.Add(num, DefaultIssueModel.ClanIssuesText, null);
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x00068BC4 File Offset: 0x00066DC4
		public override ValueTuple<int, int> GetCausalityForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			int skillValue = alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1);
			float num = 0.8f;
			if (skillValue != 0)
			{
				num = (float)(issueAlternativeSolutionSkill.Item2 / skillValue) * 0.1f;
			}
			num = MBMath.ClampFloat(num, 0.2f, 0.8f);
			int num2 = MathF.Ceiling((float)issue.GetTotalAlternativeSolutionNeededMenCount() * num);
			return new ValueTuple<int, int>(MBMath.ClampInt(2 * (num2 / 3), 1, num2), num2);
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x00068C34 File Offset: 0x00066E34
		public override float GetFailureRiskForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			return MBMath.ClampFloat((float)(issueAlternativeSolutionSkill.Item2 - alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1)) * 0.5f / 100f, 0f, 0.9f);
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x00068C7C File Offset: 0x00066E7C
		public override CampaignTime GetDurationOfResolutionForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			int skillValue = alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1);
			float num = 10f;
			if (skillValue != 0)
			{
				num = MBMath.ClampFloat((float)(issueAlternativeSolutionSkill.Item2 / skillValue), 0f, 10f);
			}
			return CampaignTime.Days((float)issue.GetBaseAlternativeSolutionDurationInDays() + 2f * num);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00068CD8 File Offset: 0x00066ED8
		public override int GetTroopsRequiredForHero(Hero alternativeSolutionHero, IssueBase issue)
		{
			ValueTuple<SkillObject, int> issueAlternativeSolutionSkill = this.GetIssueAlternativeSolutionSkill(alternativeSolutionHero, issue);
			int skillValue = alternativeSolutionHero.GetSkillValue(issueAlternativeSolutionSkill.Item1);
			float num = 1.2f;
			if (skillValue != 0)
			{
				num = (float)issueAlternativeSolutionSkill.Item2 / (float)skillValue;
			}
			num = MBMath.ClampFloat(num, 0.2f, 1.2f);
			return (int)((float)issue.AlternativeSolutionBaseNeededMenCount * num);
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x00068D2A File Offset: 0x00066F2A
		public override ValueTuple<SkillObject, int> GetIssueAlternativeSolutionSkill(Hero hero, IssueBase issue)
		{
			return issue.GetAlternativeSolutionSkill(hero);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00068D34 File Offset: 0x00066F34
		private void GetIssueEffectOfHeroInternal(IssueEffect issueEffect, Hero hero, ref ExplainedNumber explainedNumber, TextObject customText)
		{
			float activeIssueEffectAmount = hero.Issue.GetActiveIssueEffectAmount(issueEffect);
			if (activeIssueEffectAmount != 0f)
			{
				explainedNumber.Add(activeIssueEffectAmount, customText, null);
			}
		}

		// Token: 0x04000791 RID: 1937
		private static readonly TextObject SettlementIssuesText = new TextObject("{=EQLgVYk0}Settlement Issues", null);

		// Token: 0x04000792 RID: 1938
		private static readonly TextObject HeroIssueText = GameTexts.FindText("str_issues", null);

		// Token: 0x04000793 RID: 1939
		private static readonly TextObject RelatedSettlementIssuesText = new TextObject("{=umNyHc3A}Bound Village Issues", null);

		// Token: 0x04000794 RID: 1940
		private static readonly TextObject ClanIssuesText = new TextObject("{=jdl8G8JS}Clan Issues", null);
	}
}
