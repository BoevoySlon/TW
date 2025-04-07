﻿using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x02000278 RID: 632
	public class FightTournamentGame : TournamentGame
	{
		// Token: 0x060021B2 RID: 8626 RVA: 0x0008F194 File Offset: 0x0008D394
		internal static void AutoGeneratedStaticCollectObjectsFightTournamentGame(object o, List<object> collectedObjects)
		{
			((FightTournamentGame)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x0008F1A2 File Offset: 0x0008D3A2
		protected override void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			base.AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x0008F1AB File Offset: 0x0008D3AB
		public override int MaxTeamSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x0008F1AE File Offset: 0x0008D3AE
		public override int MaxTeamNumberPerMatch
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x0008F1B1 File Offset: 0x0008D3B1
		public override int RemoveTournamentAfterDays
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060021B7 RID: 8631 RVA: 0x0008F1B5 File Offset: 0x0008D3B5
		public override int MaximumParticipantCount
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x0008F1B9 File Offset: 0x0008D3B9
		public FightTournamentGame(Town town) : base(town, null)
		{
			base.Mode = TournamentGame.QualificationMode.TeamScore;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0008F1CC File Offset: 0x0008D3CC
		public override bool CanBeAParticipant(CharacterObject character, bool considerSkills)
		{
			if (!character.IsHero)
			{
				return character.Tier >= 3;
			}
			return !considerSkills || character.HeroObject.GetSkillValue(DefaultSkills.OneHanded) >= 100 || character.HeroObject.GetSkillValue(DefaultSkills.TwoHanded) >= 100;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0008F220 File Offset: 0x0008D420
		public override TextObject GetMenuText()
		{
			int num = this.GetParticipantCharacters(Settlement.CurrentSettlement, false).Count((CharacterObject p) => p.IsHero);
			TextObject textObject = new TextObject("{=!}{TOURNAMENT_EXPLANATION} {PRIZE_EXPLANATION}", null);
			textObject.SetTextVariable("TOURNAMENT_EXPLANATION", GameTexts.FindText("str_fighting_menu_text", null));
			TextObject textObject2;
			if (num > 0)
			{
				textObject2 = new TextObject("{=GuWWKgEm}As you approach the arena, you overhear gossip about the contestants and prizes. Apparently there {?(NOBLE_COUNT > 1)}are {NOBLE_COUNT} lords{?}is 1 lord{\\?} with renowned fighting skills present in the city who plan to enter the tournament. Given this turnout, the organizers are offering {.a} \"{.%}{TOURNAMENT_PRIZE}{.%}\" for the victor.", null);
				textObject2.SetTextVariable("NOBLE_COUNT", num);
				textObject2.SetTextVariable("TOURNAMENT_PRIZE", base.Prize.Name);
			}
			else
			{
				textObject2 = new TextObject("{=mnAdqeGu}As you approach the arena, you overhear gossip about the contestants and prizes. Apparently there are no lords who plan to compete, but the winner will still receive a {TOURNAMENT_PRIZE}.", null);
				textObject2.SetTextVariable("TOURNAMENT_PRIZE", base.Prize.Name);
			}
			textObject.SetTextVariable("PRIZE_EXPLANATION", textObject2);
			return textObject;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0008F2E8 File Offset: 0x0008D4E8
		public override void OpenMission(Settlement settlement, bool isPlayerParticipating)
		{
			int upgradeLevel = settlement.IsTown ? settlement.Town.GetWallLevel() : 1;
			SandBoxMission.OpenTournamentFightMission(LocationComplex.Current.GetScene("arena", upgradeLevel), this, settlement, settlement.Culture, isPlayerParticipating);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0008F32C File Offset: 0x0008D52C
		public override MBList<CharacterObject> GetParticipantCharacters(Settlement settlement, bool includePlayer = true)
		{
			MBList<CharacterObject> mblist = new MBList<CharacterObject>();
			if (includePlayer)
			{
				mblist.Add(CharacterObject.PlayerCharacter);
			}
			int num = 0;
			while (num < settlement.Parties.Count && mblist.Count < this.MaximumParticipantCount)
			{
				Hero leaderHero = settlement.Parties[num].LeaderHero;
				if (this.CanNpcJoinTournament(leaderHero, mblist, true))
				{
					if (leaderHero.CurrentSettlement != settlement)
					{
						Debug.Print(leaderHero.StringId + " is in settlement.Parties list but current settlement is not, tournament settlement: " + settlement.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
					}
					mblist.Add(leaderHero.CharacterObject);
				}
				num++;
			}
			int num2 = 0;
			while (num2 < settlement.HeroesWithoutParty.Count && mblist.Count < this.MaximumParticipantCount)
			{
				Hero hero = settlement.HeroesWithoutParty[num2];
				if (this.CanNpcJoinTournament(hero, mblist, true) && hero.IsLord)
				{
					if (hero.CurrentSettlement != settlement)
					{
						Debug.Print(hero.StringId + " is in settlement.HeroesWithoutParty list but current settlement is not, tournament settlement: " + settlement.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
					}
					mblist.Add(hero.CharacterObject);
				}
				num2++;
			}
			int num3 = 0;
			while (num3 < settlement.HeroesWithoutParty.Count && mblist.Count < this.MaximumParticipantCount)
			{
				Hero hero2 = settlement.HeroesWithoutParty[num3];
				if (this.CanNpcJoinTournament(hero2, mblist, true) && !hero2.IsLord)
				{
					if (hero2.CurrentSettlement != settlement)
					{
						Debug.Print(hero2.StringId + " is in settlement.HeroesWithoutParty list but current settlement is not, tournament settlement: " + settlement.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
					}
					mblist.Add(hero2.CharacterObject);
				}
				num3++;
			}
			int num4 = 0;
			while (num4 < settlement.Parties.Count && mblist.Count < this.MaximumParticipantCount)
			{
				foreach (TroopRosterElement troopRosterElement in settlement.Parties[num4].MemberRoster.GetTroopRoster())
				{
					if (mblist.Count >= this.MaximumParticipantCount)
					{
						break;
					}
					CharacterObject character = troopRosterElement.Character;
					if (character.IsHero && character.HeroObject.Clan == Clan.PlayerClan && this.CanNpcJoinTournament(character.HeroObject, mblist, true))
					{
						if (character.HeroObject.CurrentSettlement != settlement)
						{
							Debug.Print(character.HeroObject.StringId + " is in settlement.HeroesWithoutParty list but current settlement is not, tournament settlement: " + settlement.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
						}
						mblist.Add(character);
					}
				}
				num4++;
			}
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			if (mblist.Count < this.MaximumParticipantCount && settlement.Town.GarrisonParty != null)
			{
				int num8 = this.MaximumParticipantCount - mblist.Count;
				foreach (TroopRosterElement troopRosterElement2 in settlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					if (this.CanBeAParticipant(troopRosterElement2.Character, false))
					{
						if (!mblist.Contains(troopRosterElement2.Character) && troopRosterElement2.Character.Tier == 3 && (float)num8 * 0.4f > (float)num5)
						{
							mblist.Add(troopRosterElement2.Character);
							num5++;
						}
						else if (!mblist.Contains(troopRosterElement2.Character) && troopRosterElement2.Character.Tier == 4 && (float)num8 * 0.4f > (float)num6)
						{
							mblist.Add(troopRosterElement2.Character);
							num6++;
						}
						else if (!mblist.Contains(troopRosterElement2.Character) && troopRosterElement2.Character.Tier == 5 && (float)num8 * 0.2f > (float)num7)
						{
							mblist.Add(troopRosterElement2.Character);
							num7++;
						}
					}
					if (mblist.Count >= this.MaximumParticipantCount)
					{
						break;
					}
				}
			}
			if (mblist.Count < this.MaximumParticipantCount)
			{
				List<CharacterObject> list = new List<CharacterObject>();
				CharacterObject basicTroop = ((settlement != null) ? settlement.Culture : Game.Current.ObjectManager.GetObject<CultureObject>("empire")).BasicTroop;
				this.GetUpgradeTargets(basicTroop, ref list);
				int num9 = this.MaximumParticipantCount - mblist.Count;
				foreach (CharacterObject characterObject in list)
				{
					if (!mblist.Contains(characterObject) && characterObject.Tier == 3 && (float)num9 * 0.4f > (float)num5)
					{
						mblist.Add(characterObject);
						num5++;
					}
					else if (!mblist.Contains(characterObject) && characterObject.Tier == 4 && (float)num9 * 0.4f > (float)num6)
					{
						mblist.Add(characterObject);
						num6++;
					}
					else if (!mblist.Contains(characterObject) && characterObject.Tier == 5 && (float)num9 * 0.2f > (float)num7)
					{
						mblist.Add(characterObject);
						num7++;
					}
					if (mblist.Count >= this.MaximumParticipantCount)
					{
						break;
					}
				}
				if (!list.IsEmpty<CharacterObject>())
				{
					while (mblist.Count < this.MaximumParticipantCount)
					{
						list.Shuffle<CharacterObject>();
						int num10 = 0;
						while (num10 < list.Count && mblist.Count < this.MaximumParticipantCount)
						{
							mblist.Add(list[num10]);
							num10++;
						}
					}
				}
			}
			while (mblist.Count < this.MaximumParticipantCount)
			{
				CultureObject cultureObject = (settlement != null) ? settlement.Culture : Game.Current.ObjectManager.GetObject<CultureObject>("empire");
				CharacterObject item = (MBRandom.RandomFloat > 0.5f) ? cultureObject.BasicTroop : cultureObject.EliteBasicTroop;
				mblist.Add(item);
			}
			this.SortTournamentParticipants(mblist);
			return mblist;
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0008F950 File Offset: 0x0008DB50
		private bool CanNpcJoinTournament(Hero hero, MBList<CharacterObject> participantCharacters, bool considerSkills)
		{
			return hero != null && !hero.IsWounded && !hero.IsNoncombatant && !participantCharacters.Contains(hero.CharacterObject) && hero != Hero.MainHero && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && (hero.IsLord || hero.IsWanderer) && this.CanBeAParticipant(hero.CharacterObject, considerSkills);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x0008F9C4 File Offset: 0x0008DBC4
		private void GetUpgradeTargets(CharacterObject troop, ref List<CharacterObject> list)
		{
			if (!list.Contains(troop) && this.CanBeAParticipant(troop, false))
			{
				list.Add(troop);
			}
			foreach (CharacterObject troop2 in troop.UpgradeTargets)
			{
				this.GetUpgradeTargets(troop2, ref list);
			}
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0008FA10 File Offset: 0x0008DC10
		private void SortTournamentParticipants(MBList<CharacterObject> participantCharacters)
		{
			for (int i = 0; i < participantCharacters.Count - 1; i++)
			{
				for (int j = participantCharacters.Count - 1; j > i; j--)
				{
					if (this.GetTroopPriorityPointForTournament(participantCharacters[j]) > this.GetTroopPriorityPointForTournament(participantCharacters[i]))
					{
						CharacterObject value = participantCharacters[j];
						CharacterObject value2 = participantCharacters[i];
						participantCharacters[j] = value2;
						participantCharacters[i] = value;
					}
				}
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x0008FA80 File Offset: 0x0008DC80
		private int GetTroopPriorityPointForTournament(CharacterObject troop)
		{
			int num = 40000;
			if (troop == CharacterObject.PlayerCharacter)
			{
				num += 80000;
			}
			if (troop.IsHero)
			{
				num += 20000;
			}
			if (troop.IsHero && troop.HeroObject.IsPlayerCompanion)
			{
				num += 10000;
			}
			else
			{
				Hero heroObject = troop.HeroObject;
				if (((heroObject != null) ? heroObject.Clan : null) != null)
				{
					int num2 = num;
					Clan clan = troop.HeroObject.Clan;
					num = num2 + (int)((clan != null) ? new float?(clan.Renown) : null).Value;
				}
				else
				{
					num += troop.Level;
				}
			}
			return num;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x0008FB24 File Offset: 0x0008DD24
		protected override ItemObject GetTournamentPrize(bool includePlayer, int lastRecordedLordCountForTournamentPrize)
		{
			int num = this.GetParticipantCharacters(base.Town.Settlement, includePlayer).Count((CharacterObject p) => p.IsHero);
			if (lastRecordedLordCountForTournamentPrize == num && base.Prize != null)
			{
				return base.Prize;
			}
			this._lastRecordedLordCountForTournamentPrize = num;
			if (num >= 4)
			{
				if (this._possibleEliteRewardItemObjectsCache == null || this._possibleEliteRewardItemObjectsCache.IsEmpty<ItemObject>())
				{
					this.CachePossibleEliteRewardItems();
					this.CachePossibleBannerItems(true);
				}
				int minValue = 0;
				int maxValue = this._possibleEliteRewardItemObjectsCache.Count;
				if (num < 10)
				{
					maxValue = this._possibleEliteRewardItemObjectsCache.Count / 2;
				}
				else
				{
					minValue = this._possibleEliteRewardItemObjectsCache.Count / 2;
				}
				return this._possibleEliteRewardItemObjectsCache[MBRandom.RandomInt(minValue, maxValue)];
			}
			ItemObject itemObject = null;
			if (MBRandom.RandomFloat < 0.05f)
			{
				if (this._possibleBannerRewardItemObjectsCache == null || this._possibleBannerRewardItemObjectsCache.IsEmpty<ItemObject>())
				{
					this.CachePossibleBannerItems(false);
				}
				itemObject = this._possibleBannerRewardItemObjectsCache.GetRandomElement<ItemObject>();
			}
			if (itemObject == null)
			{
				if (this._possibleRegularRewardItemObjectsCache == null || this._possibleRegularRewardItemObjectsCache.IsEmpty<ItemObject>())
				{
					this.CachePossibleRegularRewardItems();
				}
				int num2 = this._possibleRegularRewardItemObjectsCache.Count / 4;
				int num3 = Math.Min(this._possibleRegularRewardItemObjectsCache.Count, num2 * (num + 1));
				int minValue2 = Math.Max(0, num3 - num2);
				itemObject = this._possibleRegularRewardItemObjectsCache[MBRandom.RandomInt(minValue2, num3)];
			}
			if (itemObject != null)
			{
				return itemObject;
			}
			if (this._possibleEliteRewardItemObjectsCache == null || this._possibleEliteRewardItemObjectsCache.IsEmpty<ItemObject>())
			{
				this.CachePossibleEliteRewardItems();
			}
			return this._possibleEliteRewardItemObjectsCache.GetRandomElement<ItemObject>();
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x0008FCC4 File Offset: 0x0008DEC4
		private void CachePossibleBannerItems(bool isElite)
		{
			if (this._possibleBannerRewardItemObjectsCache == null)
			{
				this._possibleBannerRewardItemObjectsCache = new MBList<ItemObject>();
			}
			foreach (ItemObject itemObject in Campaign.Current.Models.BannerItemModel.GetPossibleRewardBannerItems())
			{
				if (isElite)
				{
					if (itemObject.BannerComponent.BannerLevel == 3)
					{
						this._possibleEliteRewardItemObjectsCache.Add(itemObject);
					}
				}
				else if (itemObject.BannerComponent.BannerLevel == 1 || itemObject.BannerComponent.BannerLevel == 2)
				{
					this._possibleBannerRewardItemObjectsCache.Add(itemObject);
				}
			}
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x0008FD74 File Offset: 0x0008DF74
		private void CachePossibleRegularRewardItems()
		{
			if (this._possibleRegularRewardItemObjectsCache == null)
			{
				this._possibleRegularRewardItemObjectsCache = new MBList<ItemObject>();
			}
			MBList<ItemObject> mblist = new MBList<ItemObject>();
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.Value > 1600 && itemObject.Value < 5000 && !itemObject.NotMerchandise && (itemObject.IsCraftedWeapon || itemObject.IsMountable || itemObject.ArmorComponent != null) && !itemObject.IsCraftedByPlayer)
				{
					if (itemObject.Culture == base.Town.Culture)
					{
						this._possibleRegularRewardItemObjectsCache.Add(itemObject);
					}
					else
					{
						mblist.Add(itemObject);
					}
				}
			}
			if (this._possibleRegularRewardItemObjectsCache.IsEmpty<ItemObject>())
			{
				this._possibleRegularRewardItemObjectsCache.AddRange(mblist);
			}
			this._possibleRegularRewardItemObjectsCache.Sort((ItemObject x, ItemObject y) => x.Value.CompareTo(y.Value));
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x0008FE88 File Offset: 0x0008E088
		private void CachePossibleEliteRewardItems()
		{
			if (this._possibleEliteRewardItemObjectsCache == null)
			{
				this._possibleEliteRewardItemObjectsCache = new MBList<ItemObject>();
			}
			foreach (string objectName in new string[]
			{
				"winds_fury_sword_t3",
				"bone_crusher_mace_t3",
				"tyrhung_sword_t3",
				"pernach_mace_t3",
				"early_retirement_2hsword_t3",
				"black_heart_2haxe_t3",
				"knights_fall_mace_t3",
				"the_scalpel_sword_t3",
				"judgement_mace_t3",
				"dawnbreaker_sword_t3",
				"ambassador_sword_t3",
				"heavy_nasalhelm_over_imperial_mail",
				"sturgian_helmet_closed",
				"full_helm_over_laced_coif",
				"desert_mail_coif",
				"heavy_nasalhelm_over_imperial_mail",
				"plumed_nomad_helmet",
				"ridged_northernhelm",
				"noble_horse_southern",
				"noble_horse_imperial",
				"noble_horse_western",
				"noble_horse_eastern",
				"noble_horse_battania",
				"noble_horse_northern",
				"special_camel",
				"western_crowned_helmet",
				"northern_warlord_helmet",
				"battania_warlord_pauldrons",
				"aserai_armor_02_b",
				"white_coat_over_mail",
				"spiked_helmet_with_facemask"
			})
			{
				this._possibleEliteRewardItemObjectsCache.Add(Game.Current.ObjectManager.GetObject<ItemObject>(objectName));
			}
			this._possibleEliteRewardItemObjectsCache.Sort((ItemObject x, ItemObject y) => x.Value.CompareTo(y.Value));
		}

		// Token: 0x04000A68 RID: 2664
		private const int LordLimitForEliteReward = 4;

		// Token: 0x04000A69 RID: 2665
		private const int EliteRewardLordCountToDivideRewards = 10;

		// Token: 0x04000A6A RID: 2666
		private const int EliteRewardGroupCount = 2;

		// Token: 0x04000A6B RID: 2667
		private const int RegularRewardMinimumValue = 1600;

		// Token: 0x04000A6C RID: 2668
		private const int RegularRewardMaximumValue = 5000;

		// Token: 0x04000A6D RID: 2669
		private const int ParticipateTournamentMinimumSkillLimit = 100;

		// Token: 0x04000A6E RID: 2670
		private const float RegularBannerRewardChance = 0.05f;

		// Token: 0x04000A6F RID: 2671
		public const int ParticipantTroopMinimumTierLimit = 3;

		// Token: 0x04000A70 RID: 2672
		private MBList<ItemObject> _possibleBannerRewardItemObjectsCache;

		// Token: 0x04000A71 RID: 2673
		private MBList<ItemObject> _possibleRegularRewardItemObjectsCache;

		// Token: 0x04000A72 RID: 2674
		private MBList<ItemObject> _possibleEliteRewardItemObjectsCache;
	}
}
