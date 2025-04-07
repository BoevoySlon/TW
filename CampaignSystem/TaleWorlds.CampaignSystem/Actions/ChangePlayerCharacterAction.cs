using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000431 RID: 1073
	public class ChangePlayerCharacterAction
	{
		// Token: 0x0600407B RID: 16507 RVA: 0x0013E298 File Offset: 0x0013C498
		public static void Apply(Hero hero)
		{
			Hero mainHero = Hero.MainHero;
			MobileParty mainParty = MobileParty.MainParty;
			Game.Current.PlayerTroop = hero.CharacterObject;
			CampaignEventDispatcher.Instance.OnBeforePlayerCharacterChanged(mainHero, hero);
			bool isMainPartyChanged;
			Campaign.Current.OnPlayerCharacterChanged(out isMainPartyChanged);
			if (mainParty != MobileParty.MainParty && mainParty.IsActive)
			{
				if (mainParty.MemberRoster.TotalManCount == 0)
				{
					DestroyPartyAction.Apply(null, mainParty);
				}
				else
				{
					mainParty.LordPartyComponent.ChangePartyOwner(Hero.MainHero);
				}
			}
			if (hero.IsPrisoner)
			{
				PlayerCaptivity.OnPlayerCharacterChanged();
			}
			CampaignEventDispatcher.Instance.OnPlayerCharacterChanged(mainHero, hero, MobileParty.MainParty, isMainPartyChanged);
			PartyBase.MainParty.SetVisualAsDirty();
			Campaign.Current.MainHeroIllDays = -1;
		}
	}
}
