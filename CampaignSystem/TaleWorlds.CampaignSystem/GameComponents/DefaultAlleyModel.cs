using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000E9 RID: 233
	public class DefaultAlleyModel : AlleyModel
	{
		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x0005B110 File Offset: 0x00059310
		private CharacterObject _thug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_1");
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0005B121 File Offset: 0x00059321
		private CharacterObject _expertThug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_2");
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0005B132 File Offset: 0x00059332
		private CharacterObject _masterThug
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CharacterObject>("gangster_3");
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0005B143 File Offset: 0x00059343
		public override CampaignTime DestroyAlleyAfterDaysWhenLeaderIsDeath
		{
			get
			{
				return CampaignTime.Days(4f);
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0005B14F File Offset: 0x0005934F
		public override int MinimumTroopCountInPlayerOwnedAlley
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0005B152 File Offset: 0x00059352
		public override int MaximumTroopCountInPlayerOwnedAlley
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x0005B156 File Offset: 0x00059356
		public override float GetDailyCrimeRatingOfAlley
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0005B15D File Offset: 0x0005935D
		public override float GetDailyXpGainForAssignedClanMember(Hero assignedHero)
		{
			return 200f;
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0005B164 File Offset: 0x00059364
		public override float GetDailyXpGainForMainHero()
		{
			return 40f;
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0005B16B File Offset: 0x0005936B
		public override float GetInitialXpGainForMainHero()
		{
			return 1500f;
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0005B172 File Offset: 0x00059372
		public override float GetXpGainAfterSuccessfulAlleyDefenseForMainHero()
		{
			return 6000f;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0005B179 File Offset: 0x00059379
		public override TroopRoster GetTroopsOfAIOwnedAlley(Alley alley)
		{
			return this.GetTroopsOfAlleyInternal(alley);
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0005B184 File Offset: 0x00059384
		public override TroopRoster GetTroopsOfAlleyForBattleMission(Alley alley)
		{
			TroopRoster troopsOfAlleyInternal = this.GetTroopsOfAlleyInternal(alley);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			foreach (TroopRosterElement troopRosterElement in troopsOfAlleyInternal.GetTroopRoster())
			{
				troopRoster.AddToCounts(troopRosterElement.Character, troopRosterElement.Number * 2, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0005B1F8 File Offset: 0x000593F8
		private TroopRoster GetTroopsOfAlleyInternal(Alley alley)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			Hero owner = alley.Owner;
			if (owner.Power <= 100f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 1, false, 0, 0, true, -1);
				}
			}
			else if (owner.Power <= 200f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
			}
			else if (owner.Power <= 300f)
			{
				if ((float)owner.RandomValue > 0.5f)
				{
					troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 2, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 2, false, 0, 0, true, -1);
				}
				else
				{
					troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._expertThug, 3, false, 0, 0, true, -1);
					troopRoster.AddToCounts(this._masterThug, 3, false, 0, 0, true, -1);
				}
			}
			else if ((float)owner.RandomValue > 0.5f)
			{
				troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._expertThug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._masterThug, 3, false, 0, 0, true, -1);
			}
			else
			{
				troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._expertThug, 4, false, 0, 0, true, -1);
				troopRoster.AddToCounts(this._masterThug, 4, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0005B428 File Offset: 0x00059628
		public override List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(Alley alley)
		{
			List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> list = new List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>>();
			foreach (Hero hero in Clan.PlayerClan.Lords)
			{
				if (hero != Hero.MainHero && !hero.IsDead)
				{
					list.Add(new ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>(hero, this.GetAvailability(alley, hero)));
				}
			}
			foreach (Hero hero2 in Clan.PlayerClan.Companions)
			{
				if (hero2 != Hero.MainHero && !hero2.IsDead)
				{
					list.Add(new ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>(hero2, this.GetAvailability(alley, hero2)));
				}
			}
			return list;
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0005B508 File Offset: 0x00059708
		public override TroopRoster GetTroopsToRecruitFromAlleyDependingOnAlleyRandom(Alley alley, float random)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			if (random >= 0.5f)
			{
				return troopRoster;
			}
			Clan relatedBanditClanDependingOnAlleySettlementFaction = this.GetRelatedBanditClanDependingOnAlleySettlementFaction(alley);
			if (random > 0.3f)
			{
				troopRoster.AddToCounts(this._thug, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 1, false, 0, 0, true, -1);
			}
			else if (random > 0.15f)
			{
				troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 1, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 1, false, 0, 0, true, -1);
			}
			else if (random > 0.05f)
			{
				troopRoster.AddToCounts(this._thug, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 1, false, 0, 0, true, -1);
			}
			else
			{
				troopRoster.AddToCounts(this._thug, 2, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop, 3, false, 0, 0, true, -1);
				troopRoster.AddToCounts(relatedBanditClanDependingOnAlleySettlementFaction.BasicTroop.UpgradeTargets[0], 3, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0005B638 File Offset: 0x00059838
		public override TextObject GetDisabledReasonTextForHero(Hero hero, Alley alley, DefaultAlleyModel.AlleyMemberAvailabilityDetail detail)
		{
			switch (detail)
			{
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available:
				return TextObject.Empty;
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay:
			{
				TextObject textObject = new TextObject("{=dgUF5awO}It will take {HOURS} {?HOURS > 1}hours{?}hour{\\?} for this clan member to arrive.", null);
				textObject.SetTextVariable("HOURS", (int)Math.Ceiling((double)Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, alley.Settlement.Party).ResultNumber));
				return textObject;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughRoguerySkill:
			{
				TextObject textObject2 = GameTexts.FindText("str_character_role_disabled_tooltip", null);
				textObject2.SetTextVariable("SKILL_NAME", DefaultSkills.Roguery.Name.ToString());
				textObject2.SetTextVariable("MIN_SKILL_AMOUNT", 30);
				return textObject2;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughMercyTrait:
			{
				TextObject textObject3 = GameTexts.FindText("str_hero_needs_trait_tooltip", null);
				textObject3.SetTextVariable("TRAIT_NAME", DefaultTraits.Mercy.Name.ToString());
				textObject3.SetTextVariable("MAX_TRAIT_AMOUNT", 0);
				return textObject3;
			}
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.CanNotLeadParty:
				return new TextObject("{=qClVr2ka}This hero cannot lead a party.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlreadyAlleyLeader:
				return GameTexts.FindText("str_hero_is_already_alley_leader", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Prisoner:
				return new TextObject("{=qhRC8XWU}This hero is currently prisoner.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.SolvingIssue:
				return new TextObject("{=nT6EQGf9}This hero is currently solving an issue.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Traveling:
				return new TextObject("{=WECWpVSw}This hero is currently traveling.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy:
				return new TextObject("{=c9iu5lcc}This hero is currently busy.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Fugutive:
				return new TextObject("{=eZYtkDff}This hero is currently fugutive.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.Governor:
				return new TextObject("{=8NI4wrqU}This hero is currently assigned as a governor.", null);
			case DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlleyUnderAttack:
				return new TextObject("{=pdqi2qz1}You can not do this action while your alley is under attack.", null);
			default:
				return TextObject.Empty;
			}
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0005B7AC File Offset: 0x000599AC
		public override float GetAlleyAttackResponseTimeInDays(TroopRoster troopRoster)
		{
			float num = 0f;
			foreach (TroopRosterElement troopRosterElement in troopRoster.GetTroopRoster())
			{
				num += (((float)troopRosterElement.Character.Tier > 4f) ? 4f : ((float)troopRosterElement.Character.Tier)) * (float)troopRosterElement.Number;
			}
			return (float)Math.Min(10, 5 + (int)(num / 8f));
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0005B844 File Offset: 0x00059A44
		private Clan GetRelatedBanditClanDependingOnAlleySettlementFaction(Alley alley)
		{
			string stringId = alley.Settlement.Culture.StringId;
			Clan result = null;
			if (stringId == "khuzait")
			{
				result = Clan.BanditFactions.FirstOrDefault((Clan x) => x.StringId == "steppe_bandits");
			}
			else if (stringId == "vlandia" || stringId.Contains("empire"))
			{
				result = Clan.BanditFactions.FirstOrDefault((Clan x) => x.StringId == "mountain_bandits");
			}
			else if (stringId == "aserai")
			{
				result = Clan.BanditFactions.FirstOrDefault((Clan x) => x.StringId == "desert_bandits");
			}
			else if (stringId == "battania")
			{
				result = Clan.BanditFactions.FirstOrDefault((Clan x) => x.StringId == "forest_bandits");
			}
			else if (stringId == "sturgia")
			{
				result = Clan.BanditFactions.FirstOrDefault((Clan x) => x.StringId == "sea_raiders");
			}
			return result;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0005B994 File Offset: 0x00059B94
		private DefaultAlleyModel.AlleyMemberAvailabilityDetail GetAvailability(Alley alley, Hero hero)
		{
			IAlleyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>();
			if (campaignBehavior != null && campaignBehavior.GetIsAlleyUnderAttack(alley))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlleyUnderAttack;
			}
			if (hero.GetSkillValue(DefaultSkills.Roguery) < 30)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughRoguerySkill;
			}
			if (hero.GetTraitLevel(DefaultTraits.Mercy) > 0)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.NotEnoughMercyTrait;
			}
			if (campaignBehavior != null && campaignBehavior.GetAllAssignedClanMembersForOwnedAlleys().Contains(hero))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AlreadyAlleyLeader;
			}
			if (hero.GovernorOf != null)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Governor;
			}
			if (!hero.CanLeadParty())
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.CanNotLeadParty;
			}
			if (Campaign.Current.IssueManager.IssueSolvingCompanionList.Contains(hero))
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.SolvingIssue;
			}
			if (hero.IsFugitive)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Fugutive;
			}
			if (hero.IsTraveling)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Traveling;
			}
			if (hero.IsPrisoner)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Prisoner;
			}
			if (!hero.IsActive)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy;
			}
			if (hero.IsPartyLeader)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Busy;
			}
			if (Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, alley.Settlement.Party).BaseNumber > 0f)
			{
				return DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay;
			}
			return DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0005BA84 File Offset: 0x00059C84
		public override int GetDailyIncomeOfAlley(Alley alley)
		{
			return (int)(alley.Settlement.Town.Prosperity / 50f);
		}

		// Token: 0x04000710 RID: 1808
		private const int BaseResponseTimeInDays = 5;

		// Token: 0x04000711 RID: 1809
		private const int MaxResponseTimeInDays = 10;

		// Token: 0x04000712 RID: 1810
		public const int MinimumRoguerySkillNeededForLeadingAnAlley = 30;

		// Token: 0x04000713 RID: 1811
		public const int MaximumMercyTraitNeededForLeadingAnAlley = 0;

		// Token: 0x020004F6 RID: 1270
		public enum AlleyMemberAvailabilityDetail
		{
			// Token: 0x0400155D RID: 5469
			Available,
			// Token: 0x0400155E RID: 5470
			AvailableWithDelay,
			// Token: 0x0400155F RID: 5471
			NotEnoughRoguerySkill,
			// Token: 0x04001560 RID: 5472
			NotEnoughMercyTrait,
			// Token: 0x04001561 RID: 5473
			CanNotLeadParty,
			// Token: 0x04001562 RID: 5474
			AlreadyAlleyLeader,
			// Token: 0x04001563 RID: 5475
			Prisoner,
			// Token: 0x04001564 RID: 5476
			SolvingIssue,
			// Token: 0x04001565 RID: 5477
			Traveling,
			// Token: 0x04001566 RID: 5478
			Busy,
			// Token: 0x04001567 RID: 5479
			Fugutive,
			// Token: 0x04001568 RID: 5480
			Governor,
			// Token: 0x04001569 RID: 5481
			AlleyUnderAttack
		}
	}
}
