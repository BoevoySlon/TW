using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000424 RID: 1060
	public static class AddHeroToPartyAction
	{
		// Token: 0x06004042 RID: 16450 RVA: 0x0013C6EC File Offset: 0x0013A8EC
		private static void ApplyInternal(Hero hero, MobileParty newParty, bool showNotification = true)
		{
			PartyBase partyBase = (hero.PartyBelongedTo != null) ? hero.PartyBelongedTo.Party : ((hero.CurrentSettlement != null) ? hero.CurrentSettlement.Party : null);
			if (partyBase != null)
			{
				if (partyBase.IsSettlement && partyBase.Settlement.Notables.IndexOf(hero) >= 0)
				{
					hero.StayingInSettlement = null;
				}
				else
				{
					partyBase.MemberRoster.AddToCounts(hero.CharacterObject, -1, false, 0, 0, true, -1);
				}
			}
			if (hero.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
			newParty.AddElementToMemberRoster(hero.CharacterObject, 1, false);
			CampaignEventDispatcher.Instance.OnHeroJoinedParty(hero, newParty);
			if (showNotification && newParty == MobileParty.MainParty && hero.IsPlayerCompanion)
			{
				TextObject textObject = GameTexts.FindText("str_companion_added", null);
				StringHelpers.SetCharacterProperties("COMPANION", hero.CharacterObject, textObject, false);
				MBInformationManager.AddQuickInformation(textObject, 0, null, "");
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x0013C7CD File Offset: 0x0013A9CD
		public static void Apply(Hero hero, MobileParty party, bool showNotification = true)
		{
			AddHeroToPartyAction.ApplyInternal(hero, party, showNotification);
		}
	}
}
