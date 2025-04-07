using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000145 RID: 325
	public class DefaultValuationModel : ValuationModel
	{
		// Token: 0x06001835 RID: 6197 RVA: 0x0007B273 File Offset: 0x00079473
		public override float GetMilitaryValueOfParty(MobileParty party)
		{
			return party.Party.TotalStrength * 15f;
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x0007B286 File Offset: 0x00079486
		public override float GetValueOfTroop(CharacterObject troop)
		{
			return troop.GetPower() * 15f;
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x0007B294 File Offset: 0x00079494
		public override float GetValueOfHero(Hero hero)
		{
			if (hero.Clan != null)
			{
				return ((float)hero.Clan.Gold * 0.15f + (float)((1 + hero.Clan.Tier * hero.Clan.Tier) * 500)) * ((hero.Clan.Leader == hero) ? 4f : 1f);
			}
			return 500f;
		}
	}
}
