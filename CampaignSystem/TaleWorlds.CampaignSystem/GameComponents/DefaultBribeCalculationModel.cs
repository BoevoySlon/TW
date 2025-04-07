using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F0 RID: 240
	public class DefaultBribeCalculationModel : BribeCalculationModel
	{
		// Token: 0x060014B5 RID: 5301 RVA: 0x0005D1EC File Offset: 0x0005B3EC
		public override bool IsBribeNotNeededToEnterKeep(Settlement settlement)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterLordsHall(settlement, out accessDetails);
			return accessDetails.AccessLevel != SettlementAccessModel.AccessLevel.LimitedAccess || (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution != SettlementAccessModel.LimitedAccessSolution.Bribe);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0005D234 File Offset: 0x0005B434
		public override bool IsBribeNotNeededToEnterDungeon(Settlement settlement)
		{
			SettlementAccessModel.AccessDetails accessDetails;
			Campaign.Current.Models.SettlementAccessModel.CanMainHeroEnterDungeon(settlement, out accessDetails);
			return accessDetails.AccessLevel != SettlementAccessModel.AccessLevel.LimitedAccess || (accessDetails.AccessLevel == SettlementAccessModel.AccessLevel.LimitedAccess && accessDetails.LimitedAccessSolution != SettlementAccessModel.LimitedAccessSolution.Bribe);
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x0005D27A File Offset: 0x0005B47A
		private float GetSkillFactor()
		{
			return (1f - (float)Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) / 300f) * 0.65f + 0.35f;
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0005D2A4 File Offset: 0x0005B4A4
		private int GetBribeForCriminalRating(IFaction faction)
		{
			return MathF.Round(Campaign.Current.Models.CrimeModel.GetCost(faction, CrimeModel.PaymentMethod.Gold, 0f)) / 5;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0005D2C8 File Offset: 0x0005B4C8
		private int GetBaseBribeValue(IFaction faction)
		{
			if (faction.IsAtWarWith(Clan.PlayerClan))
			{
				return 5000;
			}
			if (faction.IsAtWarWith(Hero.MainHero.MapFaction))
			{
				return 3000;
			}
			if (FactionManager.IsNeutralWithFaction(faction, Clan.PlayerClan))
			{
				return 100;
			}
			if (Hero.MainHero.Clan == faction)
			{
				return 0;
			}
			if (Hero.MainHero.MapFaction == faction)
			{
				return 0;
			}
			if (faction is Clan)
			{
				IFaction mapFaction = Hero.MainHero.MapFaction;
				Kingdom kingdom = (faction as Clan).Kingdom;
				return 0;
			}
			return 0;
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0005D350 File Offset: 0x0005B550
		public override int GetBribeToEnterLordsHall(Settlement settlement)
		{
			if (this.IsBribeNotNeededToEnterKeep(settlement))
			{
				return 0;
			}
			return this.GetBribeInternal(settlement);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0005D364 File Offset: 0x0005B564
		public override int GetBribeToEnterDungeon(Settlement settlement)
		{
			return this.GetBribeToEnterLordsHall(settlement);
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0005D370 File Offset: 0x0005B570
		private int GetBribeInternal(Settlement settlement)
		{
			int num = this.GetBaseBribeValue(settlement.MapFaction);
			num += this.GetBribeForCriminalRating(settlement.MapFaction);
			if (Clan.PlayerClan.Renown < 500f)
			{
				num += (500 - (int)Clan.PlayerClan.Renown) * 15 / 10;
				num = MathF.Max(num, 50);
			}
			num = (int)((float)num * this.GetSkillFactor() / 25f) * 25;
			return MathF.Max(num - settlement.BribePaid, 0);
		}
	}
}
