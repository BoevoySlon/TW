using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011A RID: 282
	public class DefaultNotablePowerModel : NotablePowerModel
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x0006CCEC File Offset: 0x0006AEEC
		public override int NotableDisappearPowerLimit
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x0006CCF0 File Offset: 0x0006AEF0
		public override ExplainedNumber CalculateDailyPowerChangeForHero(Hero hero, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			if (!hero.IsActive)
			{
				return result;
			}
			if (hero.Power > (float)this.RegularNotableMaxPowerLevel)
			{
				this.CalculateDailyPowerChangeForInfluentialNotables(hero, ref result);
			}
			this.CalculateDailyPowerChangePerPropertyOwned(hero, ref result);
			if (hero.Issue != null)
			{
				this.CalculatePowerChangeFromIssues(hero, ref result);
			}
			if (hero.IsArtisan)
			{
				result.Add(-0.1f, this._propertyEffect, null);
			}
			if (hero.IsGangLeader)
			{
				result.Add(-0.4f, this._propertyEffect, null);
			}
			if (hero.IsRuralNotable)
			{
				result.Add(0.1f, this._propertyEffect, null);
			}
			if (hero.IsHeadman)
			{
				result.Add(0.1f, this._propertyEffect, null);
			}
			if (hero.IsMerchant)
			{
				result.Add(0.2f, this._propertyEffect, null);
			}
			if (hero.CurrentSettlement != null)
			{
				if (hero.CurrentSettlement.IsVillage && hero.CurrentSettlement.Village.Bound.IsCastle)
				{
					result.Add(0.1f, this._propertyEffect, null);
				}
				if (hero.SupporterOf == hero.CurrentSettlement.OwnerClan)
				{
					this.CalculateDailyPowerChangeForAffiliationWithRulerClan(ref result);
				}
			}
			return result;
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x0006CE29 File Offset: 0x0006B029
		public override int RegularNotableMaxPowerLevel
		{
			get
			{
				return this.NotablePowerRanks[1].MinPowerValue;
			}
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x0006CE3C File Offset: 0x0006B03C
		private void CalculateDailyPowerChangePerPropertyOwned(Hero hero, ref ExplainedNumber explainedNumber)
		{
			float num = 0.1f;
			int count = hero.OwnedAlleys.Count;
			explainedNumber.Add(num * (float)count, this._propertyEffect, null);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0006CE6C File Offset: 0x0006B06C
		private void CalculateDailyPowerChangeForAffiliationWithRulerClan(ref ExplainedNumber explainedNumber)
		{
			float value = 0.2f;
			explainedNumber.Add(value, this._rulerClanEffect, null);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x0006CE90 File Offset: 0x0006B090
		private void CalculateDailyPowerChangeForInfluentialNotables(Hero hero, ref ExplainedNumber explainedNumber)
		{
			float value = -1f * ((hero.Power - (float)this.RegularNotableMaxPowerLevel) / 500f);
			explainedNumber.Add(value, this._currentRankEffect, null);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x0006CEC6 File Offset: 0x0006B0C6
		private void CalculatePowerChangeFromIssues(Hero hero, ref ExplainedNumber explainedNumber)
		{
			Campaign.Current.Models.IssueModel.GetIssueEffectOfHero(DefaultIssueEffects.IssueOwnerPower, hero, ref explainedNumber);
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x0006CEE3 File Offset: 0x0006B0E3
		public override TextObject GetPowerRankName(Hero hero)
		{
			return this.GetPowerRank(hero).Name;
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x0006CEF1 File Offset: 0x0006B0F1
		public override float GetInfluenceBonusToClan(Hero hero)
		{
			return this.GetPowerRank(hero).InfluenceBonus;
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x0006CF00 File Offset: 0x0006B100
		private DefaultNotablePowerModel.NotablePowerRank GetPowerRank(Hero hero)
		{
			int num = 0;
			for (int i = 0; i < this.NotablePowerRanks.Length; i++)
			{
				if (hero.Power > (float)this.NotablePowerRanks[i].MinPowerValue)
				{
					num = i;
				}
			}
			return this.NotablePowerRanks[num];
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x0006CF4C File Offset: 0x0006B14C
		public override int GetInitialPower()
		{
			float randomFloat = MBRandom.RandomFloat;
			if (randomFloat < 0.2f)
			{
				return MBRandom.RandomInt((int)((float)(this.NotablePowerRanks[0].MinPowerValue + this.NotablePowerRanks[1].MinPowerValue) * 0.5f), this.NotablePowerRanks[1].MinPowerValue);
			}
			if (randomFloat >= 0.8f)
			{
				return MBRandom.RandomInt(this.NotablePowerRanks[2].MinPowerValue, (int)((float)this.NotablePowerRanks[2].MinPowerValue * 2f));
			}
			return MBRandom.RandomInt(this.NotablePowerRanks[1].MinPowerValue, this.NotablePowerRanks[2].MinPowerValue);
		}

		// Token: 0x040007BB RID: 1979
		private DefaultNotablePowerModel.NotablePowerRank[] NotablePowerRanks = new DefaultNotablePowerModel.NotablePowerRank[]
		{
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=aTeuX4L0}Regular", null), 0, 0.05f),
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=nTETQEmy}Influential", null), 100, 0.1f),
			new DefaultNotablePowerModel.NotablePowerRank(new TextObject("{=UCpyo9hw}Powerful", null), 200, 0.15f)
		};

		// Token: 0x040007BC RID: 1980
		private TextObject _currentRankEffect = new TextObject("{=7j9uHxLM}Current Rank Effect", null);

		// Token: 0x040007BD RID: 1981
		private TextObject _militiaEffect = new TextObject("{=R1MaIgOb}Militia Effect", null);

		// Token: 0x040007BE RID: 1982
		private TextObject _rulerClanEffect = new TextObject("{=JE3RTqx5}Ruler Clan Effect", null);

		// Token: 0x040007BF RID: 1983
		private TextObject _propertyEffect = new TextObject("{=yDomN9L2}Property Effect", null);

		// Token: 0x0200050B RID: 1291
		private struct NotablePowerRank
		{
			// Token: 0x060043DE RID: 17374 RVA: 0x00146DBC File Offset: 0x00144FBC
			public NotablePowerRank(TextObject name, int minPowerValue, float influenceBonus)
			{
				this.Name = name;
				this.MinPowerValue = minPowerValue;
				this.InfluenceBonus = influenceBonus;
			}

			// Token: 0x040015B2 RID: 5554
			public readonly TextObject Name;

			// Token: 0x040015B3 RID: 5555
			public readonly int MinPowerValue;

			// Token: 0x040015B4 RID: 5556
			public readonly float InfluenceBonus;
		}
	}
}
