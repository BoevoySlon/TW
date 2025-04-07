using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200043B RID: 1083
	public static class DestroyPartyAction
	{
		// Token: 0x0600409E RID: 16542 RVA: 0x0013EA98 File Offset: 0x0013CC98
		private static void ApplyInternal(PartyBase destroyerParty, MobileParty destroyedParty)
		{
			if (destroyedParty != MobileParty.MainParty)
			{
				if (destroyedParty.IsCaravan && destroyedParty.Party.Owner != null && destroyedParty.Party.Owner.GetPerkValue(DefaultPerks.Trade.InsurancePlans))
				{
					GiveGoldAction.ApplyBetweenCharacters(null, destroyedParty.Party.Owner, (int)DefaultPerks.Trade.InsurancePlans.PrimaryBonus, false);
				}
				destroyedParty.RemoveParty();
				CampaignEventDispatcher.Instance.OnMobilePartyDestroyed(destroyedParty, destroyerParty);
			}
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0013EB08 File Offset: 0x0013CD08
		public static void Apply(PartyBase destroyerParty, MobileParty destroyedParty)
		{
			DestroyPartyAction.ApplyInternal(destroyerParty, destroyedParty);
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x0013EB11 File Offset: 0x0013CD11
		public static void ApplyForDisbanding(MobileParty disbandedParty, Settlement relatedSettlement)
		{
			if (disbandedParty.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForParty(disbandedParty);
			}
			CampaignEventDispatcher.Instance.OnPartyDisbanded(disbandedParty, relatedSettlement);
			DestroyPartyAction.ApplyInternal(null, disbandedParty);
		}
	}
}
