using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200012A RID: 298
	public class DefaultPrisonBreakModel : PrisonBreakModel
	{
		// Token: 0x06001702 RID: 5890 RVA: 0x0007187C File Offset: 0x0006FA7C
		public override bool CanPlayerStagePrisonBreak(Settlement settlement)
		{
			bool result = false;
			if (settlement.IsFortification)
			{
				MobileParty garrisonParty = settlement.Town.GarrisonParty;
				bool flag = (garrisonParty != null && garrisonParty.PrisonRoster.TotalHeroes > 0) || settlement.Party.PrisonRoster.TotalHeroes > 0;
				result = (settlement.MapFaction != Clan.PlayerClan.MapFaction && !FactionManager.IsAlliedWithFaction(settlement.MapFaction, Clan.PlayerClan.MapFaction) && flag);
			}
			return result;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x000718FC File Offset: 0x0006FAFC
		public override int GetPrisonBreakStartCost(Hero prisonerHero)
		{
			int num = MathF.Ceiling((float)Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(prisonerHero.CharacterObject, null) / 2000f * prisonerHero.CurrentSettlement.Town.Security * 35f - (float)(Hero.MainHero.GetSkillValue(DefaultSkills.Roguery) * 10));
			num = ((num < 100) ? 0 : (num / 100 * 100));
			return num + 1000;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00071973 File Offset: 0x0006FB73
		public override int GetRelationRewardOnPrisonBreak(Hero prisonerHero)
		{
			return 15;
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00071977 File Offset: 0x0006FB77
		public override float GetRogueryRewardOnPrisonBreak(Hero prisonerHero, bool isSuccess)
		{
			return (float)(isSuccess ? MBRandom.RandomInt(3500, 6000) : MBRandom.RandomInt(1000, 2500));
		}

		// Token: 0x04000815 RID: 2069
		private const int BasePrisonBreakCost = 1000;
	}
}
