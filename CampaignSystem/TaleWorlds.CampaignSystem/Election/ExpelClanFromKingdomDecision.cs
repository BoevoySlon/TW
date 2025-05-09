﻿using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.Election
{
	// Token: 0x0200026D RID: 621
	public class ExpelClanFromKingdomDecision : KingdomDecision
	{
		// Token: 0x06002086 RID: 8326 RVA: 0x0008AF74 File Offset: 0x00089174
		internal static void AutoGeneratedStaticCollectObjectsExpelClanFromKingdomDecision(object o, List<object> collectedObjects)
		{
			((ExpelClanFromKingdomDecision)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0008AF82 File Offset: 0x00089182
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			collectedObjects.Add(this.ClanToExpel);
			collectedObjects.Add(this.OldKingdom);
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x0008AFA3 File Offset: 0x000891A3
		internal static object AutoGeneratedGetMemberValueClanToExpel(object o)
		{
			return ((ExpelClanFromKingdomDecision)o).ClanToExpel;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x0008AFB0 File Offset: 0x000891B0
		internal static object AutoGeneratedGetMemberValueOldKingdom(object o)
		{
			return ((ExpelClanFromKingdomDecision)o).OldKingdom;
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x0008AFBD File Offset: 0x000891BD
		public ExpelClanFromKingdomDecision(Clan proposerClan, Clan clan) : base(proposerClan)
		{
			this.ClanToExpel = clan;
			this.OldKingdom = clan.Kingdom;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x0008AFD9 File Offset: 0x000891D9
		public override bool IsAllowed()
		{
			return Campaign.Current.Models.KingdomDecisionPermissionModel.IsExpulsionDecisionAllowed(this.ClanToExpel);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x0008AFF5 File Offset: 0x000891F5
		public override int GetProposalInfluenceCost()
		{
			return Campaign.Current.Models.DiplomacyModel.GetInfluenceCostOfExpellingClan(base.ProposerClan);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x0008B011 File Offset: 0x00089211
		public override TextObject GetGeneralTitle()
		{
			TextObject textObject = new TextObject("{=pF92DagG}Expel {CLAN_NAME} from {KINGDOM_NAME}", null);
			textObject.SetTextVariable("CLAN_NAME", this.ClanToExpel.Name);
			textObject.SetTextVariable("KINGDOM_NAME", this.OldKingdom.Name);
			return textObject;
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0008B04C File Offset: 0x0008924C
		public override TextObject GetSupportTitle()
		{
			TextObject textObject = new TextObject("{=ZwpWX8Zx}Vote for expelling {CLAN_NAME} from the kingdom", null);
			textObject.SetTextVariable("CLAN_NAME", this.ClanToExpel.Name);
			return textObject;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x0008B070 File Offset: 0x00089270
		public override TextObject GetChooseTitle()
		{
			TextObject textObject = new TextObject("{=pF92DagG}Expel {CLAN_NAME} from {KINGDOM_NAME}", null);
			textObject.SetTextVariable("CLAN_NAME", this.ClanToExpel.Name);
			textObject.SetTextVariable("KINGDOM_NAME", this.OldKingdom.Name);
			return textObject;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x0008B0AC File Offset: 0x000892AC
		public override TextObject GetSupportDescription()
		{
			TextObject textObject = new TextObject("{=eTr0XHas}{FACTION_LEADER} will decide if {CLAN_NAME} will be expelled from {KINGDOM_NAME}. You can pick your stance regarding this decision.", null);
			textObject.SetTextVariable("FACTION_LEADER", this.DetermineChooser().Leader.Name);
			textObject.SetTextVariable("CLAN_NAME", this.ClanToExpel.Name);
			textObject.SetTextVariable("KINGDOM_NAME", this.OldKingdom.Name);
			return textObject;
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x0008B110 File Offset: 0x00089310
		public override TextObject GetChooseDescription()
		{
			TextObject textObject = new TextObject("{=J8brFxIW}As {?IS_FEMALE}queen{?}king{\\?} you must decide if {CLAN_NAME} will be expelled from kingdom.", null);
			textObject.SetTextVariable("IS_FEMALE", this.DetermineChooser().Leader.IsFemale ? 1 : 0);
			textObject.SetTextVariable("CLAN_NAME", this.ClanToExpel.Name);
			return textObject;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x0008B161 File Offset: 0x00089361
		public override IEnumerable<DecisionOutcome> DetermineInitialCandidates()
		{
			yield return new ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome(true);
			yield return new ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome(false);
			yield break;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x0008B16A File Offset: 0x0008936A
		public override Clan DetermineChooser()
		{
			return this.OldKingdom.RulingClan;
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x0008B177 File Offset: 0x00089377
		protected override bool ShouldBeCancelledInternal()
		{
			return !base.Kingdom.Clans.Contains(this.ClanToExpel);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x0008B194 File Offset: 0x00089394
		public override float DetermineSupport(Clan clan, DecisionOutcome possibleOutcome)
		{
			bool shouldBeExpelled = ((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)possibleOutcome).ShouldBeExpelled;
			float num = 3.5f;
			float num2 = (float)FactionManager.GetRelationBetweenClans(this.ClanToExpel, clan) * num;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 10000f;
			foreach (Settlement settlement in this.ClanToExpel.Settlements)
			{
				num3 += settlement.GetSettlementValueForFaction(this.OldKingdom) * 0.005f;
			}
			if (clan.Leader.GetTraitLevel(DefaultTraits.Calculating) > 0)
			{
				num5 = this.ClanToExpel.Influence * 0.05f + this.ClanToExpel.Renown * 0.02f;
			}
			if (clan.Leader.GetTraitLevel(DefaultTraits.Commander) > 0)
			{
				foreach (WarPartyComponent warPartyComponent in this.ClanToExpel.WarPartyComponents)
				{
					num4 += (float)warPartyComponent.MobileParty.MemberRoster.TotalManCount * 0.01f;
				}
			}
			float num7 = num6 + num2 + num3 + num4 + num5;
			float result;
			if (shouldBeExpelled)
			{
				result = -num7;
			}
			else
			{
				result = num7;
			}
			return result;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x0008B310 File Offset: 0x00089510
		public override void DetermineSponsors(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
		{
			foreach (DecisionOutcome decisionOutcome in possibleOutcomes)
			{
				if (((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)decisionOutcome).ShouldBeExpelled)
				{
					decisionOutcome.SetSponsor(base.ProposerClan);
				}
				else
				{
					base.AssignDefaultSponsor(decisionOutcome);
				}
			}
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x0008B37C File Offset: 0x0008957C
		public override void ApplyChosenOutcome(DecisionOutcome chosenOutcome)
		{
			if (((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)chosenOutcome).ShouldBeExpelled)
			{
				int relationCostOfExpellingClanFromKingdom = Campaign.Current.Models.DiplomacyModel.GetRelationCostOfExpellingClanFromKingdom();
				foreach (Supporter supporter in chosenOutcome.SupporterList)
				{
					if (((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)chosenOutcome).ShouldBeExpelled && this.ClanToExpel.Leader != supporter.Clan.Leader)
					{
						ChangeRelationAction.ApplyRelationChangeBetweenHeroes(this.ClanToExpel.Leader, supporter.Clan.Leader, relationCostOfExpellingClanFromKingdom, true);
					}
				}
				ChangeKingdomAction.ApplyByLeaveKingdom(this.ClanToExpel, true);
			}
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x0008B43C File Offset: 0x0008963C
		public override TextObject GetSecondaryEffects()
		{
			return new TextObject("{=fJY9uosa}All supporters gain some relations with each other and lose a large amount of relations with the expelled clan.", null);
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x0008B449 File Offset: 0x00089649
		public override void ApplySecondaryEffects(MBReadOnlyList<DecisionOutcome> possibleOutcomes, DecisionOutcome chosenOutcome)
		{
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x0008B44C File Offset: 0x0008964C
		public override TextObject GetChosenOutcomeText(DecisionOutcome chosenOutcome, KingdomDecision.SupportStatus supportStatus, bool isShortVersion = false)
		{
			TextObject textObject;
			if (((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)chosenOutcome).ShouldBeExpelled)
			{
				if (base.IsSingleClanDecision())
				{
					textObject = new TextObject("{=h5eTEYON}{RULER.NAME} has expelled the {CLAN} clan from the {KINGDOM}.", null);
				}
				else if (supportStatus == KingdomDecision.SupportStatus.Majority)
				{
					textObject = new TextObject("{=rd229FYG}{RULER.NAME} has expelled the {CLAN} clan from the {KINGDOM} with the support of {?RULER.GENDER}her{?}his{\\?} council.", null);
				}
				else if (supportStatus == KingdomDecision.SupportStatus.Minority)
				{
					textObject = new TextObject("{=G3qGLAeQ}{RULER.NAME} has expelled the {CLAN} clan from the {KINGDOM} against the wishes of {?RULER.GENDER}her{?}his{\\?} council.", null);
				}
				else
				{
					textObject = new TextObject("{=m6OVl6Dg}{RULER.NAME} has expelled the {CLAN} clan from the {KINGDOM}, with {?RULER.GENDER}her{?}his{\\?} council evenly split on the matter.", null);
				}
			}
			else if (base.IsSingleClanDecision())
			{
				textObject = new TextObject("{=mvkKP6OE}{RULER.NAME} chose not to expel the {CLAN} clan from the {KINGDOM}.", null);
			}
			else if (supportStatus == KingdomDecision.SupportStatus.Majority)
			{
				textObject = new TextObject("{=yBL3TzXw}{RULER.NAME} chose not to expel the {CLAN} clan from the {KINGDOM} with the support of {?RULER.GENDER}her{?}his{\\?} council.", null);
			}
			else if (supportStatus == KingdomDecision.SupportStatus.Minority)
			{
				textObject = new TextObject("{=940TwBPs}{RULER.NAME} chose not to expel the {CLAN} clan from the {KINGDOM} over the objections of {?RULER.GENDER}her{?}his{\\?} council.", null);
			}
			else
			{
				textObject = new TextObject("{=Oe1NdVLe}{RULER.NAME} chose not to expel the {CLAN} clan from the {KINGDOM} with {?RULER.GENDER}her{?}his{\\?} council evenly split on the matter.", null);
			}
			textObject.SetTextVariable("CLAN", this.ClanToExpel.Name);
			textObject.SetTextVariable("KINGDOM", this.OldKingdom.Name);
			StringHelpers.SetCharacterProperties("RULER", this.OldKingdom.Leader.CharacterObject, textObject, false);
			return textObject;
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0008B540 File Offset: 0x00089740
		public override DecisionOutcome GetQueriedDecisionOutcome(MBReadOnlyList<DecisionOutcome> possibleOutcomes)
		{
			return possibleOutcomes.FirstOrDefault((DecisionOutcome t) => ((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)t).ShouldBeExpelled);
		}

		// Token: 0x04000A40 RID: 2624
		private const float ClanFiefModifier = 0.005f;

		// Token: 0x04000A41 RID: 2625
		[SaveableField(100)]
		public readonly Clan ClanToExpel;

		// Token: 0x04000A42 RID: 2626
		[SaveableField(102)]
		public readonly Kingdom OldKingdom;

		// Token: 0x02000576 RID: 1398
		public class ExpelClanDecisionOutcome : DecisionOutcome
		{
			// Token: 0x06004584 RID: 17796 RVA: 0x0014A568 File Offset: 0x00148768
			internal static void AutoGeneratedStaticCollectObjectsExpelClanDecisionOutcome(object o, List<object> collectedObjects)
			{
				((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004585 RID: 17797 RVA: 0x0014A576 File Offset: 0x00148776
			protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				base.AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06004586 RID: 17798 RVA: 0x0014A57F File Offset: 0x0014877F
			internal static object AutoGeneratedGetMemberValueShouldBeExpelled(object o)
			{
				return ((ExpelClanFromKingdomDecision.ExpelClanDecisionOutcome)o).ShouldBeExpelled;
			}

			// Token: 0x06004587 RID: 17799 RVA: 0x0014A591 File Offset: 0x00148791
			public ExpelClanDecisionOutcome(bool shouldBeExpelled)
			{
				this.ShouldBeExpelled = shouldBeExpelled;
			}

			// Token: 0x06004588 RID: 17800 RVA: 0x0014A5A0 File Offset: 0x001487A0
			public override TextObject GetDecisionTitle()
			{
				TextObject textObject = new TextObject("{=kakxnaN5}{?SUPPORT}Yes{?}No{\\?}", null);
				textObject.SetTextVariable("SUPPORT", this.ShouldBeExpelled ? 1 : 0);
				return textObject;
			}

			// Token: 0x06004589 RID: 17801 RVA: 0x0014A5C5 File Offset: 0x001487C5
			public override TextObject GetDecisionDescription()
			{
				if (this.ShouldBeExpelled)
				{
					return new TextObject("{=s8z5Ugvm}The clan should be expelled", null);
				}
				return new TextObject("{=b2InhEeP}We oppose expelling the clan", null);
			}

			// Token: 0x0600458A RID: 17802 RVA: 0x0014A5E6 File Offset: 0x001487E6
			public override string GetDecisionLink()
			{
				return null;
			}

			// Token: 0x0600458B RID: 17803 RVA: 0x0014A5E9 File Offset: 0x001487E9
			public override ImageIdentifier GetDecisionImageIdentifier()
			{
				return null;
			}

			// Token: 0x040016ED RID: 5869
			[SaveableField(100)]
			public readonly bool ShouldBeExpelled;
		}
	}
}
