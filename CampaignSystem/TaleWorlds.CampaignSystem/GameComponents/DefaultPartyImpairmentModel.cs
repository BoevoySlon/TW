using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011F RID: 287
	public class DefaultPartyImpairmentModel : PartyImpairmentModel
	{
		// Token: 0x06001699 RID: 5785 RVA: 0x0006DC70 File Offset: 0x0006BE70
		public override float GetSiegeExpectedVulnerabilityTime()
		{
			float num = (2f + MBRandom.RandomFloatNormal + 24f - CampaignTime.Now.CurrentHourInDay) % 24f;
			float num2 = MathF.Pow(MBRandom.RandomFloat, 6f);
			return (((MBRandom.RandomFloatNormal > 0f) ? num2 : (1f - num2)) * 24f + num) % 24f;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x0006DCD8 File Offset: 0x0006BED8
		public override float GetDisorganizedStateDuration(MobileParty party)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(6f, false, null);
			if (party.MapEvent != null && (party.MapEvent.IsRaid || party.MapEvent.IsSiegeAssault) && party.HasPerk(DefaultPerks.Tactics.SwiftRegroup, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Tactics.SwiftRegroup.PrimaryBonus, DefaultPerks.Tactics.SwiftRegroup.Description);
			}
			if (party.HasPerk(DefaultPerks.Scouting.Foragers, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Scouting.Foragers.SecondaryBonus, DefaultPerks.Scouting.Foragers.Description);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0006DD74 File Offset: 0x0006BF74
		public override bool CanGetDisorganized(PartyBase party)
		{
			return party.IsActive && party.IsMobile && party.MobileParty.MemberRoster.TotalManCount >= 10 && (party.MobileParty.Army == null || party.MobileParty == party.MobileParty.Army.LeaderParty || party.MobileParty.AttachedTo != null);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x0006DDDC File Offset: 0x0006BFDC
		public override float GetVulnerabilityStateDuration(PartyBase party)
		{
			return MBRandom.RandomFloatNormal + 4f;
		}

		// Token: 0x040007CA RID: 1994
		private const float BaseDisorganizedStateDuration = 6f;

		// Token: 0x040007CB RID: 1995
		private static readonly TextObject _settlementInvolvedMapEvent = new TextObject("{=KVlPhPSD}Settlement involved map event", null);
	}
}
