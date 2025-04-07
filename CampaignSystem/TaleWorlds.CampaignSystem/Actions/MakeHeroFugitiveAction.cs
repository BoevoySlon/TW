using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x0200044D RID: 1101
	public static class MakeHeroFugitiveAction
	{
		// Token: 0x060040F2 RID: 16626 RVA: 0x00140968 File Offset: 0x0013EB68
		private static void ApplyInternal(Hero fugitive)
		{
			if (fugitive.IsAlive)
			{
				if (fugitive.PartyBelongedTo != null)
				{
					if (fugitive.PartyBelongedTo.LeaderHero == fugitive)
					{
						DestroyPartyAction.Apply(null, fugitive.PartyBelongedTo);
					}
					else
					{
						fugitive.PartyBelongedTo.MemberRoster.RemoveTroop(fugitive.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					}
				}
				if (fugitive.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForCharacterOnly(fugitive);
				}
				fugitive.ChangeState(Hero.CharacterStates.Fugitive);
				CampaignEventDispatcher.Instance.OnCharacterBecameFugitive(fugitive);
			}
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x001409E2 File Offset: 0x0013EBE2
		public static void Apply(Hero fugitive)
		{
			MakeHeroFugitiveAction.ApplyInternal(fugitive);
		}
	}
}
