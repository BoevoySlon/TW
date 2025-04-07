using System;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000FC RID: 252
	public class DefaultCrimeModel : CrimeModel
	{
		// Token: 0x06001540 RID: 5440 RVA: 0x00062681 File Offset: 0x00060881
		public override bool DoesPlayerHaveAnyCrimeRating(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 0f;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x00062690 File Offset: 0x00060890
		public override bool IsPlayerCrimeRatingSevere(IFaction faction)
		{
			return faction.MainHeroCrimeRating >= 65f;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x000626A2 File Offset: 0x000608A2
		public override bool IsPlayerCrimeRatingModerate(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 30f && faction.MainHeroCrimeRating <= 65f;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x000626C3 File Offset: 0x000608C3
		public override bool IsPlayerCrimeRatingMild(IFaction faction)
		{
			return faction.MainHeroCrimeRating > 0f && faction.MainHeroCrimeRating <= 30f;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x000626E4 File Offset: 0x000608E4
		public override float GetCost(IFaction faction, CrimeModel.PaymentMethod paymentMethod, float minimumCrimeRating)
		{
			float x = MathF.Max(0f, faction.MainHeroCrimeRating - minimumCrimeRating);
			if (paymentMethod == CrimeModel.PaymentMethod.Gold)
			{
				return (float)((int)(MathF.Pow(x, 1.2f) * 100f));
			}
			if (paymentMethod != CrimeModel.PaymentMethod.Influence)
			{
				return 0f;
			}
			return MathF.Pow(x, 1.2f);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00062734 File Offset: 0x00060934
		public override ExplainedNumber GetDailyCrimeRatingChange(IFaction faction, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			int num = faction.Settlements.Count(delegate(Settlement x)
			{
				if (x.IsTown)
				{
					return x.Alleys.Any((Alley y) => y.Owner == Hero.MainHero);
				}
				return false;
			});
			result.Add((float)num * Campaign.Current.Models.AlleyModel.GetDailyCrimeRatingOfAlley, new TextObject("{=t87T82jq}Owned alleys", null), null);
			if (faction.MainHeroCrimeRating.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return result;
			}
			Clan clan = faction as Clan;
			if (Hero.MainHero.Clan == faction)
			{
				result.Add(-5f, includeDescriptions ? new TextObject("{=eNtRt6F5}Your own Clan", null) : TextObject.Empty, null);
			}
			else if (faction.IsKingdomFaction && faction.Leader == Hero.MainHero)
			{
				result.Add(-5f, includeDescriptions ? new TextObject("{=xer2bta5}Your own Kingdom", null) : TextObject.Empty, null);
			}
			else if (Hero.MainHero.MapFaction == faction)
			{
				result.Add(-1.5f, includeDescriptions ? new TextObject("{=QRwaQIbm}Is in Kingdom", null) : TextObject.Empty, null);
			}
			else if (clan != null && Hero.MainHero.MapFaction == clan.Kingdom)
			{
				result.Add(-1.25f, includeDescriptions ? new TextObject("{=hXGByLG9}Sharing the same Kingdom", null) : TextObject.Empty, null);
			}
			else if (Hero.MainHero.Clan.IsAtWarWith(faction))
			{
				result.Add(-0.25f, includeDescriptions ? new TextObject("{=BYTrUJyj}In War", null) : TextObject.Empty, null);
			}
			else
			{
				result.Add(-1f, includeDescriptions ? new TextObject("{=basevalue}Base", null) : TextObject.Empty, null);
			}
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.WhiteLies, Hero.MainHero.CharacterObject, true, ref result);
			return result;
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00062911 File Offset: 0x00060B11
		public override int DeclareWarCrimeRatingThreshold
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x00062915 File Offset: 0x00060B15
		public override float GetMaxCrimeRating()
		{
			return 100f;
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0006291C File Offset: 0x00060B1C
		public override float GetMinAcceptableCrimeRating(IFaction faction)
		{
			if (faction != Hero.MainHero.MapFaction)
			{
				return 30f;
			}
			return 20f;
		}

		// Token: 0x0400076E RID: 1902
		private const float ModerateCrimeRatingThreshold = 30f;

		// Token: 0x0400076F RID: 1903
		private const float SevereCrimeRatingThreshold = 65f;
	}
}
