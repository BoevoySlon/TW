using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200045C RID: 1116
	public static class TransferPrisonerAction
	{
		// Token: 0x0600412C RID: 16684 RVA: 0x001420F4 File Offset: 0x001402F4
		private static void ApplyInternal(CharacterObject prisonerTroop, PartyBase prisonerOwnerParty, PartyBase newParty)
		{
			if (prisonerTroop.HeroObject == Hero.MainHero)
			{
				PlayerCaptivity.CaptorParty = newParty;
				return;
			}
			prisonerOwnerParty.PrisonRoster.AddToCounts(prisonerTroop, -1, false, 0, 0, true, -1);
			newParty.AddPrisoner(prisonerTroop, 1);
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x00142126 File Offset: 0x00140326
		public static void Apply(CharacterObject prisonerTroop, PartyBase prisonerOwnerParty, PartyBase newParty)
		{
			TransferPrisonerAction.ApplyInternal(prisonerTroop, prisonerOwnerParty, newParty);
		}
	}
}
