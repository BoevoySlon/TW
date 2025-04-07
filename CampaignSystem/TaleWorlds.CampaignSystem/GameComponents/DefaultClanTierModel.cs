using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F8 RID: 248
	public class DefaultClanTierModel : ClanTierModel
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x00061265 File Offset: 0x0005F465
		public override int MinClanTier
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x00061268 File Offset: 0x0005F468
		public override int MaxClanTier
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x0006126B File Offset: 0x0005F46B
		public override int MercenaryEligibleTier
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x0006126E File Offset: 0x0005F46E
		public override int VassalEligibleTier
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x00061271 File Offset: 0x0005F471
		public override int BannerEligibleTier
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x00061274 File Offset: 0x0005F474
		public override int RebelClanStartingTier
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x00061277 File Offset: 0x0005F477
		public override int CompanionToLordClanStartingTier
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x0006127A File Offset: 0x0005F47A
		private int KingdomEligibleTier
		{
			get
			{
				return Campaign.Current.Models.KingdomCreationModel.MinimumClanTierToCreateKingdom;
			}
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00061290 File Offset: 0x0005F490
		public override int CalculateInitialRenown(Clan clan)
		{
			int num = DefaultClanTierModel.TierLowerRenownLimits[clan.Tier];
			int num2 = (clan.Tier >= this.MaxClanTier) ? (DefaultClanTierModel.TierLowerRenownLimits[this.MaxClanTier] + 1500) : DefaultClanTierModel.TierLowerRenownLimits[clan.Tier + 1];
			int maxValue = (int)((float)num2 - (float)(num2 - num) * 0.4f);
			return MBRandom.RandomInt(num, maxValue);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x000612F1 File Offset: 0x0005F4F1
		public override int CalculateInitialInfluence(Clan clan)
		{
			return (int)(150f + (float)MBRandom.RandomInt((int)((float)this.CalculateInitialRenown(clan) / 15f)) + (float)MBRandom.RandomInt(MBRandom.RandomInt(MBRandom.RandomInt(400))));
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00061328 File Offset: 0x0005F528
		public override int CalculateTier(Clan clan)
		{
			int result = this.MinClanTier;
			for (int i = this.MinClanTier + 1; i <= this.MaxClanTier; i++)
			{
				if (clan.Renown >= (float)DefaultClanTierModel.TierLowerRenownLimits[i])
				{
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x00061368 File Offset: 0x0005F568
		public override ValueTuple<ExplainedNumber, bool> HasUpcomingTier(Clan clan, out TextObject extraExplanation, bool includeDescriptions = false)
		{
			bool flag = clan.Tier < this.MaxClanTier;
			ExplainedNumber item = new ExplainedNumber(0f, includeDescriptions, null);
			extraExplanation = TextObject.Empty;
			if (flag)
			{
				int num = this.GetPartyLimitForTier(clan, clan.Tier + 1) - this.GetPartyLimitForTier(clan, clan.Tier);
				if (num != 0)
				{
					item.Add((float)num, this._partyLimitBonusText, null);
				}
				int num2 = this.GetCompanionLimitFromTier(clan.Tier + 1) - this.GetCompanionLimitFromTier(clan.Tier);
				if (num2 != 0)
				{
					item.Add((float)num2, this._companionLimitBonusText, null);
				}
				int num3 = Campaign.Current.Models.PartySizeLimitModel.GetTierPartySizeEffect(clan.Tier + 1) - Campaign.Current.Models.PartySizeLimitModel.GetTierPartySizeEffect(clan.Tier);
				if (num3 > 0)
				{
					item.Add((float)num3, this._additionalCurrentPartySizeBonus, null);
				}
				int num4 = Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(clan.Tier + 1) - Campaign.Current.Models.WorkshopModel.GetMaxWorkshopCountForClanTier(clan.Tier);
				if (num4 > 0)
				{
					item.Add((float)num4, this._additionalWorkshopCountBonus, null);
				}
				if (clan.Tier + 1 == this.MercenaryEligibleTier)
				{
					extraExplanation = this._mercenaryEligibleText;
				}
				else if (clan.Tier + 1 == this.VassalEligibleTier)
				{
					extraExplanation = this._vassalEligibleText;
				}
				else if (clan.Tier + 1 == this.KingdomEligibleTier)
				{
					extraExplanation = this._kingdomEligibleText;
				}
			}
			return new ValueTuple<ExplainedNumber, bool>(item, flag);
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x000614F0 File Offset: 0x0005F6F0
		public override int GetRequiredRenownForTier(int tier)
		{
			return DefaultClanTierModel.TierLowerRenownLimits[tier];
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x000614FC File Offset: 0x0005F6FC
		public override int GetPartyLimitForTier(Clan clan, int clanTierToCheck)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			if (!clan.IsMinorFaction)
			{
				if (clanTierToCheck < 3)
				{
					explainedNumber.Add(1f, null, null);
				}
				else if (clanTierToCheck < 5)
				{
					explainedNumber.Add(2f, null, null);
				}
				else
				{
					explainedNumber.Add(3f, null, null);
				}
			}
			else
			{
				explainedNumber.Add(MathF.Clamp((float)clanTierToCheck, 1f, 4f), null, null);
			}
			this.AddPartyLimitPerkEffects(clan, ref explainedNumber);
			return MathF.Round(explainedNumber.ResultNumber);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00061586 File Offset: 0x0005F786
		private void AddPartyLimitPerkEffects(Clan clan, ref ExplainedNumber result)
		{
			if (clan.Leader != null && clan.Leader.GetPerkValue(DefaultPerks.Leadership.TalentMagnet))
			{
				result.Add(DefaultPerks.Leadership.TalentMagnet.SecondaryBonus, DefaultPerks.Leadership.TalentMagnet.Name, null);
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000615C0 File Offset: 0x0005F7C0
		public override int GetCompanionLimit(Clan clan)
		{
			int num = this.GetCompanionLimitFromTier(clan.Tier);
			if (clan.Leader.GetPerkValue(DefaultPerks.Leadership.WePledgeOurSwords))
			{
				num += (int)DefaultPerks.Leadership.WePledgeOurSwords.PrimaryBonus;
			}
			if (clan.Leader.GetPerkValue(DefaultPerks.Charm.Camaraderie))
			{
				num += (int)DefaultPerks.Charm.Camaraderie.SecondaryBonus;
			}
			return num;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0006161B File Offset: 0x0005F81B
		private int GetCompanionLimitFromTier(int clanTier)
		{
			return clanTier + 3;
		}

		// Token: 0x04000766 RID: 1894
		private static readonly int[] TierLowerRenownLimits = new int[]
		{
			0,
			50,
			150,
			350,
			900,
			2350,
			6150
		};

		// Token: 0x04000767 RID: 1895
		private readonly TextObject _partyLimitBonusText = GameTexts.FindText("str_clan_tier_party_limit_bonus", null);

		// Token: 0x04000768 RID: 1896
		private readonly TextObject _companionLimitBonusText = GameTexts.FindText("str_clan_tier_companion_limit_bonus", null);

		// Token: 0x04000769 RID: 1897
		private readonly TextObject _mercenaryEligibleText = GameTexts.FindText("str_clan_tier_mercenary_eligible", null);

		// Token: 0x0400076A RID: 1898
		private readonly TextObject _vassalEligibleText = GameTexts.FindText("str_clan_tier_vassal_eligible", null);

		// Token: 0x0400076B RID: 1899
		private readonly TextObject _additionalCurrentPartySizeBonus = GameTexts.FindText("str_clan_tier_party_size_bonus", null);

		// Token: 0x0400076C RID: 1900
		private readonly TextObject _additionalWorkshopCountBonus = GameTexts.FindText("str_clan_tier_workshop_count_bonus", null);

		// Token: 0x0400076D RID: 1901
		private readonly TextObject _kingdomEligibleText = GameTexts.FindText("str_clan_tier_kingdom_eligible", null);
	}
}
