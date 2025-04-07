using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011C RID: 284
	public class DefaultPartyDesertionModel : PartyDesertionModel
	{
		// Token: 0x06001687 RID: 5767 RVA: 0x0006D11F File Offset: 0x0006B31F
		public override int GetMoraleThresholdForTroopDesertion(MobileParty party)
		{
			return 10;
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x0006D124 File Offset: 0x0006B324
		public override int GetNumberOfDeserters(MobileParty mobileParty)
		{
			bool flag = mobileParty.IsWageLimitExceeded();
			bool flag2 = mobileParty.Party.NumberOfAllMembers > mobileParty.LimitedPartySize;
			int result = 0;
			if (flag)
			{
				int num = mobileParty.TotalWage - mobileParty.PaymentLimit;
				result = MathF.Min(20, MathF.Max(1, (int)((float)num / Campaign.Current.AverageWage * 0.25f)));
			}
			else if (flag2)
			{
				if (mobileParty.IsGarrison)
				{
					result = MathF.Ceiling((float)(mobileParty.Party.NumberOfAllMembers - mobileParty.LimitedPartySize) * 0.25f);
				}
				else
				{
					result = ((mobileParty.Party.NumberOfAllMembers > mobileParty.LimitedPartySize) ? MathF.Max(1, (int)((float)(mobileParty.Party.NumberOfAllMembers - mobileParty.LimitedPartySize) * 0.25f)) : 0);
				}
			}
			return result;
		}
	}
}
