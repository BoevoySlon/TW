using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000440 RID: 1088
	public static class EndCaptivityAction
	{
		// Token: 0x060040B0 RID: 16560 RVA: 0x0013EDC0 File Offset: 0x0013CFC0
		private static void ApplyInternal(Hero prisoner, EndCaptivityDetail detail, Hero facilitatior = null)
		{
			PartyBase partyBelongedToAsPrisoner = prisoner.PartyBelongedToAsPrisoner;
			IFaction capturerFaction = (partyBelongedToAsPrisoner != null) ? partyBelongedToAsPrisoner.MapFaction : null;
			if (prisoner == Hero.MainHero)
			{
				PlayerCaptivity.EndCaptivity();
				if (facilitatior != null && detail != EndCaptivityDetail.Death)
				{
					StringHelpers.SetCharacterProperties("FACILITATOR", facilitatior.CharacterObject, null, false);
					MBInformationManager.AddQuickInformation(new TextObject("{=xPuSASof}{FACILITATOR.NAME} paid a ransom and freed you from captivity.", null), 0, null, "");
				}
				CampaignEventDispatcher.Instance.OnHeroPrisonerReleased(prisoner, partyBelongedToAsPrisoner, capturerFaction, detail);
				return;
			}
			if (detail == EndCaptivityDetail.Death)
			{
				prisoner.StayingInSettlement = null;
			}
			if (partyBelongedToAsPrisoner != null && partyBelongedToAsPrisoner.PrisonRoster.Contains(prisoner.CharacterObject))
			{
				partyBelongedToAsPrisoner.PrisonRoster.RemoveTroop(prisoner.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
			}
			if (detail != EndCaptivityDetail.Death)
			{
				if (detail <= EndCaptivityDetail.ReleasedByChoice || detail == EndCaptivityDetail.ReleasedByCompensation)
				{
					prisoner.ChangeState(Hero.CharacterStates.Released);
					if (prisoner.IsPlayerCompanion && detail != EndCaptivityDetail.Ransom)
					{
						MakeHeroFugitiveAction.Apply(prisoner);
					}
				}
				else
				{
					MakeHeroFugitiveAction.Apply(prisoner);
				}
				Settlement currentSettlement = prisoner.CurrentSettlement;
				if (currentSettlement != null)
				{
					currentSettlement.AddHeroWithoutParty(prisoner);
				}
				CampaignEventDispatcher.Instance.OnHeroPrisonerReleased(prisoner, partyBelongedToAsPrisoner, capturerFaction, detail);
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0013EEB7 File Offset: 0x0013D0B7
		public static void ApplyByReleasedAfterBattle(Hero character)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedAfterBattle, null);
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0013EEC1 File Offset: 0x0013D0C1
		public static void ApplyByRansom(Hero character, Hero facilitator)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.Ransom, facilitator);
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0013EECB File Offset: 0x0013D0CB
		public static void ApplyByPeace(Hero character, Hero facilitator = null)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedAfterPeace, facilitator);
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0013EED5 File Offset: 0x0013D0D5
		public static void ApplyByEscape(Hero character, Hero facilitator = null)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedAfterEscape, facilitator);
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0013EEDF File Offset: 0x0013D0DF
		public static void ApplyByDeath(Hero character)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.Death, null);
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0013EEEC File Offset: 0x0013D0EC
		public static void ApplyByReleasedByChoice(FlattenedTroopRoster troopRoster)
		{
			foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in troopRoster)
			{
				if (flattenedTroopRosterElement.Troop.IsHero)
				{
					EndCaptivityAction.ApplyInternal(flattenedTroopRosterElement.Troop.HeroObject, EndCaptivityDetail.ReleasedByChoice, null);
				}
			}
			CampaignEventDispatcher.Instance.OnPrisonerReleased(troopRoster);
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x0013EF5C File Offset: 0x0013D15C
		public static void ApplyByReleasedByChoice(Hero character, Hero facilitator = null)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedByChoice, facilitator);
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x0013EF66 File Offset: 0x0013D166
		public static void ApplyByReleasedByCompensation(Hero character)
		{
			EndCaptivityAction.ApplyInternal(character, EndCaptivityDetail.ReleasedByCompensation, null);
		}
	}
}
