using System;
using Helpers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000450 RID: 1104
	public static class MarriageAction
	{
		// Token: 0x060040FA RID: 16634 RVA: 0x00140B70 File Offset: 0x0013ED70
		private static void ApplyInternal(Hero firstHero, Hero secondHero, bool showNotification)
		{
			if (!Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(firstHero, secondHero))
			{
				Debug.Print("MarriageAction.Apply() called for not suitable couple: " + firstHero.StringId + " and " + secondHero.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			firstHero.Spouse = secondHero;
			secondHero.Spouse = firstHero;
			ChangeRelationAction.ApplyRelationChangeBetweenHeroes(firstHero, secondHero, Campaign.Current.Models.MarriageModel.GetEffectiveRelationIncrease(firstHero, secondHero), false);
			Clan clanAfterMarriage = Campaign.Current.Models.MarriageModel.GetClanAfterMarriage(firstHero, secondHero);
			if (firstHero.Clan != clanAfterMarriage)
			{
				MarriageAction.HandleClanChangeAfterMarriageForHero(firstHero, clanAfterMarriage);
			}
			else
			{
				MarriageAction.HandleClanChangeAfterMarriageForHero(secondHero, clanAfterMarriage);
			}
			Romance.EndAllCourtships(firstHero);
			Romance.EndAllCourtships(secondHero);
			ChangeRomanticStateAction.Apply(firstHero, secondHero, Romance.RomanceLevelEnum.Marriage);
			CampaignEventDispatcher.Instance.OnHeroesMarried(firstHero, secondHero, showNotification);
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00140C40 File Offset: 0x0013EE40
		private static void HandleClanChangeAfterMarriageForHero(Hero hero, Clan clanAfterMarriage)
		{
			Clan clan = hero.Clan;
			if (hero.GovernorOf != null)
			{
				ChangeGovernorAction.RemoveGovernorOf(hero);
			}
			if (hero.PartyBelongedTo != null)
			{
				if (clan.Kingdom != clanAfterMarriage.Kingdom)
				{
					if (hero.PartyBelongedTo.Army != null)
					{
						if (hero.PartyBelongedTo.Army.LeaderParty == hero.PartyBelongedTo)
						{
							DisbandArmyAction.ApplyByUnknownReason(hero.PartyBelongedTo.Army);
						}
						else
						{
							hero.PartyBelongedTo.Army = null;
						}
					}
					IFaction kingdom = clanAfterMarriage.Kingdom;
					FactionHelper.FinishAllRelatedHostileActionsOfNobleToFaction(hero, kingdom ?? clanAfterMarriage);
				}
				MakeHeroFugitiveAction.Apply(hero);
			}
			hero.Clan = clanAfterMarriage;
			foreach (Hero hero2 in clan.Heroes)
			{
				hero2.UpdateHomeSettlement();
			}
			foreach (Hero hero3 in clanAfterMarriage.Heroes)
			{
				hero3.UpdateHomeSettlement();
			}
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x00140D60 File Offset: 0x0013EF60
		public static void Apply(Hero firstHero, Hero secondHero, bool showNotification = true)
		{
			MarriageAction.ApplyInternal(firstHero, secondHero, showNotification);
		}
	}
}
