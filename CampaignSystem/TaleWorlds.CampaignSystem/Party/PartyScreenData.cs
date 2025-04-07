using System;
using System.Collections;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x020002A7 RID: 679
	public class PartyScreenData : IEnumerable<ValueTuple<TroopRosterElement, bool>>, IEnumerable
	{
		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06002758 RID: 10072 RVA: 0x000A7666 File Offset: 0x000A5866
		// (set) Token: 0x06002759 RID: 10073 RVA: 0x000A766E File Offset: 0x000A586E
		public PartyBase RightParty { get; private set; }

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x000A7677 File Offset: 0x000A5877
		// (set) Token: 0x0600275B RID: 10075 RVA: 0x000A767F File Offset: 0x000A587F
		public PartyBase LeftParty { get; private set; }

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x000A7688 File Offset: 0x000A5888
		// (set) Token: 0x0600275D RID: 10077 RVA: 0x000A7690 File Offset: 0x000A5890
		public Hero RightPartyLeaderHero { get; private set; }

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x000A7699 File Offset: 0x000A5899
		// (set) Token: 0x0600275F RID: 10079 RVA: 0x000A76A1 File Offset: 0x000A58A1
		public Hero LeftPartyLeaderHero { get; private set; }

		// Token: 0x06002760 RID: 10080 RVA: 0x000A76AA File Offset: 0x000A58AA
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000A76B4 File Offset: 0x000A58B4
		public PartyScreenData()
		{
			this.PartyGoldChangeAmount = 0;
			this.PartyInfluenceChangeAmount = new ValueTuple<int, int, int>(0, 0, 0);
			this.PartyMoraleChangeAmount = 0;
			this.PartyHorseChangeAmount = 0;
			this.RightRecruitableData = new Dictionary<CharacterObject, int>();
			this.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>();
			this.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>();
			this.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>();
			this.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>();
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000A7724 File Offset: 0x000A5924
		public void InitializeCopyFrom(PartyBase rightParty, PartyBase leftParty)
		{
			if (rightParty != null)
			{
				this.RightParty = rightParty;
				this.RightPartyLeaderHero = rightParty.LeaderHero;
			}
			if (leftParty != null)
			{
				this.LeftParty = leftParty;
				this.LeftPartyLeaderHero = leftParty.LeaderHero;
			}
			this.RightMemberRoster = TroopRoster.CreateDummyTroopRoster();
			this.LeftMemberRoster = TroopRoster.CreateDummyTroopRoster();
			this.RightPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			this.LeftPrisonerRoster = TroopRoster.CreateDummyTroopRoster();
			this.RightItemRoster = new ItemRoster();
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000A7794 File Offset: 0x000A5994
		public void CopyFromPartyAndRoster(TroopRoster rightPartyMemberRoster, TroopRoster rightPartyPrisonerRoster, TroopRoster leftPartyMemberRoster, TroopRoster leftPartyPrisonerRoster, PartyBase rightParty)
		{
			PrisonerRecruitmentCalculationModel prisonerRecruitmentCalculationModel = Campaign.Current.Models.PrisonerRecruitmentCalculationModel;
			for (int i = 0; i < rightPartyMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = rightPartyMemberRoster.GetElementCopyAtIndex(i);
				this.RightMemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, elementCopyAtIndex.Xp, true, -1);
			}
			for (int j = 0; j < leftPartyMemberRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = leftPartyMemberRoster.GetElementCopyAtIndex(j);
				this.LeftMemberRoster.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, elementCopyAtIndex2.Xp, true, -1);
			}
			this.RightRecruitableData.Clear();
			for (int k = 0; k < rightPartyPrisonerRoster.Count; k++)
			{
				TroopRosterElement elementCopyAtIndex3 = rightPartyPrisonerRoster.GetElementCopyAtIndex(k);
				this.RightPrisonerRoster.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, elementCopyAtIndex3.Xp, true, -1);
				if (rightParty != null)
				{
					MobileParty mobileParty = rightParty.MobileParty;
					bool? flag = (mobileParty != null) ? new bool?(mobileParty.IsMainParty) : null;
					bool flag2 = true;
					if (flag.GetValueOrDefault() == flag2 & flag != null)
					{
						int value = prisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, elementCopyAtIndex3.Character);
						if (!this.RightRecruitableData.ContainsKey(elementCopyAtIndex3.Character))
						{
							this.RightRecruitableData.Add(elementCopyAtIndex3.Character, value);
						}
					}
				}
			}
			for (int l = 0; l < leftPartyPrisonerRoster.Count; l++)
			{
				TroopRosterElement elementCopyAtIndex4 = leftPartyPrisonerRoster.GetElementCopyAtIndex(l);
				this.LeftPrisonerRoster.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, elementCopyAtIndex4.Xp, true, -1);
			}
			if (rightParty != null)
			{
				for (int m = 0; m < rightParty.ItemRoster.Count; m++)
				{
					ItemRosterElement elementCopyAtIndex5 = rightParty.ItemRoster.GetElementCopyAtIndex(m);
					this.RightItemRoster.AddToCounts(elementCopyAtIndex5.EquipmentElement, elementCopyAtIndex5.Amount);
				}
			}
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000A79A4 File Offset: 0x000A5BA4
		public void CopyFromScreenData(PartyScreenData data)
		{
			this.RightMemberRoster.Clear();
			for (int i = 0; i < data.RightMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = data.RightMemberRoster.GetElementCopyAtIndex(i);
				this.RightMemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, elementCopyAtIndex.Xp, true, -1);
			}
			this.RightPrisonerRoster.Clear();
			for (int j = 0; j < data.RightPrisonerRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = data.RightPrisonerRoster.GetElementCopyAtIndex(j);
				this.RightPrisonerRoster.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, elementCopyAtIndex2.Xp, true, -1);
			}
			this.RightItemRoster.Clear();
			if (data.RightItemRoster != null)
			{
				for (int k = 0; k < data.RightItemRoster.Count; k++)
				{
					ItemRosterElement elementCopyAtIndex3 = data.RightItemRoster.GetElementCopyAtIndex(k);
					this.RightItemRoster.AddToCounts(elementCopyAtIndex3.EquipmentElement, elementCopyAtIndex3.Amount);
				}
			}
			this.LeftMemberRoster.Clear();
			for (int l = 0; l < data.LeftMemberRoster.Count; l++)
			{
				TroopRosterElement elementCopyAtIndex4 = data.LeftMemberRoster.GetElementCopyAtIndex(l);
				this.LeftMemberRoster.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, elementCopyAtIndex4.Xp, true, -1);
			}
			this.LeftPrisonerRoster.Clear();
			for (int m = 0; m < data.LeftPrisonerRoster.Count; m++)
			{
				TroopRosterElement elementCopyAtIndex5 = data.LeftPrisonerRoster.GetElementCopyAtIndex(m);
				this.LeftPrisonerRoster.AddToCounts(elementCopyAtIndex5.Character, elementCopyAtIndex5.Number, false, elementCopyAtIndex5.WoundedNumber, elementCopyAtIndex5.Xp, true, -1);
			}
			this.PartyGoldChangeAmount = data.PartyGoldChangeAmount;
			this.PartyInfluenceChangeAmount = data.PartyInfluenceChangeAmount;
			this.PartyMoraleChangeAmount = data.PartyMoraleChangeAmount;
			this.PartyHorseChangeAmount = data.PartyHorseChangeAmount;
			this.RightRecruitableData = new Dictionary<CharacterObject, int>(data.RightRecruitableData);
			this.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>(data.UpgradedTroopsHistory);
			this.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>(data.TransferredPrisonersHistory);
			this.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>(data.RecruitedPrisonersHistory);
			this.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>(data.UsedUpgradeHorsesHistory);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000A7BFC File Offset: 0x000A5DFC
		public void BindRostersFrom(TroopRoster rightPartyMemberRoster, TroopRoster rightPartyPrisonerRoster, TroopRoster leftPartyMemberRoster, TroopRoster leftPartyPrisonerRoster, PartyBase rightParty, PartyBase leftParty)
		{
			this.RightParty = rightParty;
			this.LeftParty = leftParty;
			if (rightParty != null)
			{
				this.RightItemRoster = rightParty.ItemRoster;
				this.RightPartyLeaderHero = rightParty.LeaderHero;
			}
			if (leftParty != null)
			{
				this.LeftPartyLeaderHero = leftParty.LeaderHero;
			}
			this.RightMemberRoster = rightPartyMemberRoster;
			this.LeftMemberRoster = leftPartyMemberRoster;
			this.RightPrisonerRoster = rightPartyPrisonerRoster;
			this.LeftPrisonerRoster = leftPartyPrisonerRoster;
			if (rightParty != null)
			{
				MobileParty mobileParty = rightParty.MobileParty;
				bool? flag = (mobileParty != null) ? new bool?(mobileParty.IsMainParty) : null;
				bool flag2 = true;
				if (flag.GetValueOrDefault() == flag2 & flag != null)
				{
					this.RightRecruitableData = new Dictionary<CharacterObject, int>();
					PrisonerRecruitmentCalculationModel prisonerRecruitmentCalculationModel = Campaign.Current.Models.PrisonerRecruitmentCalculationModel;
					foreach (TroopRosterElement troopRosterElement in rightParty.PrisonRoster.GetTroopRoster())
					{
						int value = prisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, troopRosterElement.Character);
						if (!this.RightRecruitableData.ContainsKey(troopRosterElement.Character))
						{
							this.RightRecruitableData.Add(troopRosterElement.Character, value);
						}
					}
				}
			}
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000A7D44 File Offset: 0x000A5F44
		private List<Tuple<Hero, SkillEffect.PerkRole>> GetPartyHeroesWithPerks(TroopRoster roster)
		{
			MobileParty mobileParty;
			if (roster == null)
			{
				mobileParty = null;
			}
			else
			{
				PartyBase ownerParty = roster.OwnerParty;
				mobileParty = ((ownerParty != null) ? ownerParty.MobileParty : null);
			}
			MobileParty mobileParty2 = mobileParty;
			if (mobileParty2 == null)
			{
				return null;
			}
			List<Tuple<Hero, SkillEffect.PerkRole>> list = new List<Tuple<Hero, SkillEffect.PerkRole>>();
			for (int i = 0; i < roster.Count; i++)
			{
				CharacterObject characterAtIndex = roster.GetCharacterAtIndex(i);
				Hero hero = (characterAtIndex != null) ? characterAtIndex.HeroObject : null;
				if (hero != null)
				{
					SkillEffect.PerkRole heroPerkRole = mobileParty2.GetHeroPerkRole(hero);
					if (heroPerkRole != SkillEffect.PerkRole.None)
					{
						list.Add(new Tuple<Hero, SkillEffect.PerkRole>(hero, heroPerkRole));
					}
				}
			}
			return list;
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000A7DBC File Offset: 0x000A5FBC
		public void ResetUsing(PartyScreenData partyScreenData)
		{
			List<Tuple<Hero, SkillEffect.PerkRole>> partyHeroesWithPerks = this.GetPartyHeroesWithPerks(this.LeftMemberRoster);
			List<Tuple<Hero, SkillEffect.PerkRole>> partyHeroesWithPerks2 = this.GetPartyHeroesWithPerks(this.RightMemberRoster);
			this.RightMemberRoster.Clear();
			this.RightMemberRoster.RemoveZeroCounts();
			for (int i = 0; i < partyScreenData.RightMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = partyScreenData.RightMemberRoster.GetElementCopyAtIndex(i);
				this.RightMemberRoster.AddToCounts(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false, elementCopyAtIndex.WoundedNumber, elementCopyAtIndex.Xp, true, -1);
			}
			PartyBase rightParty = this.RightParty;
			if (((rightParty != null) ? rightParty.MobileParty : null) != null)
			{
				this.RightParty.MobileParty.ChangePartyLeader(partyScreenData.RightPartyLeaderHero);
			}
			this.LeftMemberRoster.Clear();
			this.LeftMemberRoster.RemoveZeroCounts();
			for (int j = 0; j < partyScreenData.LeftMemberRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = partyScreenData.LeftMemberRoster.GetElementCopyAtIndex(j);
				this.LeftMemberRoster.AddToCounts(elementCopyAtIndex2.Character, elementCopyAtIndex2.Number, false, elementCopyAtIndex2.WoundedNumber, elementCopyAtIndex2.Xp, true, -1);
			}
			PartyBase leftParty = this.LeftParty;
			if (((leftParty != null) ? leftParty.MobileParty : null) != null)
			{
				this.LeftParty.MobileParty.ChangePartyLeader(partyScreenData.LeftPartyLeaderHero);
			}
			this.RightPrisonerRoster.Clear();
			this.LeftPrisonerRoster.Clear();
			this.RightPrisonerRoster.RemoveZeroCounts();
			for (int k = 0; k < partyScreenData.RightPrisonerRoster.Count; k++)
			{
				TroopRosterElement elementCopyAtIndex3 = partyScreenData.RightPrisonerRoster.GetElementCopyAtIndex(k);
				this.RightPrisonerRoster.AddToCounts(elementCopyAtIndex3.Character, elementCopyAtIndex3.Number, false, elementCopyAtIndex3.WoundedNumber, elementCopyAtIndex3.Xp, true, -1);
			}
			this.LeftPrisonerRoster.RemoveZeroCounts();
			for (int l = 0; l < partyScreenData.LeftPrisonerRoster.Count; l++)
			{
				TroopRosterElement elementCopyAtIndex4 = partyScreenData.LeftPrisonerRoster.GetElementCopyAtIndex(l);
				this.LeftPrisonerRoster.AddToCounts(elementCopyAtIndex4.Character, elementCopyAtIndex4.Number, false, elementCopyAtIndex4.WoundedNumber, elementCopyAtIndex4.Xp, true, -1);
			}
			if (this.RightItemRoster != null)
			{
				this.RightItemRoster.Clear();
				for (int m = 0; m < partyScreenData.RightItemRoster.Count; m++)
				{
					ItemRosterElement elementCopyAtIndex5 = partyScreenData.RightItemRoster.GetElementCopyAtIndex(m);
					this.RightItemRoster.AddToCounts(elementCopyAtIndex5.EquipmentElement, elementCopyAtIndex5.Amount);
				}
			}
			this.PartyGoldChangeAmount = partyScreenData.PartyGoldChangeAmount;
			this.PartyInfluenceChangeAmount = partyScreenData.PartyInfluenceChangeAmount;
			this.PartyMoraleChangeAmount = partyScreenData.PartyMoraleChangeAmount;
			this.PartyHorseChangeAmount = partyScreenData.PartyHorseChangeAmount;
			this.RightRecruitableData = new Dictionary<CharacterObject, int>(partyScreenData.RightRecruitableData);
			this.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>(partyScreenData.UpgradedTroopsHistory);
			this.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>(partyScreenData.TransferredPrisonersHistory);
			this.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>(partyScreenData.RecruitedPrisonersHistory);
			this.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>(partyScreenData.UsedUpgradeHorsesHistory);
			if (partyHeroesWithPerks != null)
			{
				PartyBase leftParty2 = this.LeftParty;
				if (((leftParty2 != null) ? leftParty2.MobileParty : null) != null)
				{
					for (int n = 0; n < partyHeroesWithPerks.Count; n++)
					{
						this.LeftParty.MobileParty.SetHeroPerkRole(partyHeroesWithPerks[n].Item1, partyHeroesWithPerks[n].Item2);
					}
				}
			}
			if (partyHeroesWithPerks2 != null)
			{
				PartyBase rightParty2 = this.RightParty;
				if (((rightParty2 != null) ? rightParty2.MobileParty : null) != null)
				{
					for (int num = 0; num < partyHeroesWithPerks2.Count; num++)
					{
						this.RightParty.MobileParty.SetHeroPerkRole(partyHeroesWithPerks2[num].Item1, partyHeroesWithPerks2[num].Item2);
					}
				}
			}
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000A8160 File Offset: 0x000A6360
		public bool IsThereAnyTroopTradeDifferenceBetween(PartyScreenData other)
		{
			MBList<TroopRosterElement> troopRoster = this.RightMemberRoster.GetTroopRoster();
			MBList<TroopRosterElement> troopRoster2 = other.RightMemberRoster.GetTroopRoster();
			if (troopRoster.Count != troopRoster2.Count)
			{
				return true;
			}
			using (List<TroopRosterElement>.Enumerator enumerator = troopRoster.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TroopRosterElement elem = enumerator.Current;
					if (troopRoster2.FindIndex((TroopRosterElement x) => x.Character == elem.Character && x.Number == elem.Number) == -1)
					{
						return true;
					}
				}
			}
			MBList<TroopRosterElement> troopRoster3 = this.RightPrisonerRoster.GetTroopRoster();
			MBList<TroopRosterElement> troopRoster4 = other.RightPrisonerRoster.GetTroopRoster();
			if (troopRoster3.Count != troopRoster4.Count)
			{
				return true;
			}
			using (List<TroopRosterElement>.Enumerator enumerator = troopRoster3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TroopRosterElement elem = enumerator.Current;
					if (troopRoster4.FindIndex((TroopRosterElement x) => x.Character == elem.Character && x.Number == elem.Number) == -1)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000A8288 File Offset: 0x000A6488
		public List<TroopTradeDifference> GetTroopTradeDifferencesFromTo(PartyScreenData toPartyScreenData)
		{
			List<TroopTradeDifference> list = new List<TroopTradeDifference>();
			string str = "Current settlement: ";
			Settlement currentSettlement = Settlement.CurrentSettlement;
			Debug.Print(str + ((currentSettlement != null) ? currentSettlement.StringId : null), 0, Debug.DebugColor.White, 17592186044416UL);
			string str2 = "Left party id: ";
			PartyBase leftParty = toPartyScreenData.LeftParty;
			string str3;
			if (leftParty == null)
			{
				str3 = null;
			}
			else
			{
				MobileParty mobileParty = leftParty.MobileParty;
				str3 = ((mobileParty != null) ? mobileParty.StringId : null);
			}
			Debug.Print(str2 + str3, 0, Debug.DebugColor.White, 17592186044416UL);
			string str4 = "Right party id: ";
			PartyBase rightParty = toPartyScreenData.RightParty;
			string str5;
			if (rightParty == null)
			{
				str5 = null;
			}
			else
			{
				MobileParty mobileParty2 = rightParty.MobileParty;
				str5 = ((mobileParty2 != null) ? mobileParty2.StringId : null);
			}
			Debug.Print(str4 + str5, 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (ValueTuple<TroopRosterElement, bool> valueTuple in this)
			{
				TroopRosterElement item = valueTuple.Item1;
				int number = item.Number;
				int num = 0;
				foreach (ValueTuple<TroopRosterElement, bool> valueTuple2 in toPartyScreenData)
				{
					if (valueTuple2.Item1.Character == valueTuple.Item1.Character && valueTuple2.Item2 == valueTuple.Item2)
					{
						int num2 = num;
						item = valueTuple2.Item1;
						num = num2 + item.Number;
					}
				}
				if (number != num)
				{
					TroopTradeDifference item2 = new TroopTradeDifference
					{
						Troop = valueTuple.Item1.Character,
						ToCount = num,
						FromCount = number,
						IsPrisoner = valueTuple.Item2
					};
					list.Add(item2);
				}
				Debug.Print(string.Concat(new object[]
				{
					"currently owned: ",
					number,
					", previously owned: ",
					num,
					" name: ",
					valueTuple.Item1.Character.StringId
				}), 0, Debug.DebugColor.White, 17592186044416UL);
			}
			foreach (ValueTuple<TroopRosterElement, bool> valueTuple3 in toPartyScreenData)
			{
				TroopRosterElement item = valueTuple3.Item1;
				int number2 = item.Number;
				int num3 = 0;
				foreach (ValueTuple<TroopRosterElement, bool> valueTuple4 in this)
				{
					if (valueTuple3.Item1.Character == valueTuple4.Item1.Character && valueTuple3.Item2 == valueTuple4.Item2)
					{
						int num4 = num3;
						item = valueTuple3.Item1;
						num3 = num4 + item.Number;
					}
				}
				if (num3 != number2)
				{
					TroopTradeDifference item3 = new TroopTradeDifference
					{
						Troop = valueTuple3.Item1.Character,
						ToCount = number2,
						FromCount = num3,
						IsPrisoner = valueTuple3.Item2
					};
					if (!list.Contains(item3))
					{
						list.Add(item3);
						Debug.Print(string.Concat(new object[]
						{
							"currently owned: ",
							num3,
							", previously owned: ",
							number2,
							" name: ",
							valueTuple3.Item1.Character.StringId
						}), 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
			}
			return list;
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000A8644 File Offset: 0x000A6844
		private IEnumerator<ValueTuple<TroopRosterElement, bool>> EnumerateElements()
		{
			int num;
			for (int i = 0; i < this.RightMemberRoster.Count; i = num + 1)
			{
				TroopRosterElement elementCopyAtIndex = this.RightMemberRoster.GetElementCopyAtIndex(i);
				yield return new ValueTuple<TroopRosterElement, bool>(elementCopyAtIndex, false);
				num = i;
			}
			for (int i = 0; i < this.RightPrisonerRoster.Count; i = num + 1)
			{
				TroopRosterElement elementCopyAtIndex2 = this.RightPrisonerRoster.GetElementCopyAtIndex(i);
				yield return new ValueTuple<TroopRosterElement, bool>(elementCopyAtIndex2, true);
				num = i;
			}
			yield break;
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000A8653 File Offset: 0x000A6853
		public IEnumerator<ValueTuple<TroopRosterElement, bool>> GetEnumerator()
		{
			return this.EnumerateElements();
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000A865B File Offset: 0x000A685B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnumerateElements();
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000A8663 File Offset: 0x000A6863
		public override bool Equals(object obj)
		{
			return this == obj;
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000A866C File Offset: 0x000A686C
		public static bool operator ==(PartyScreenData a, PartyScreenData b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.PartyGoldChangeAmount != b.PartyGoldChangeAmount || a.PartyInfluenceChangeAmount.Item1 != b.PartyInfluenceChangeAmount.Item1 || a.PartyInfluenceChangeAmount.Item2 != b.PartyInfluenceChangeAmount.Item2 || a.PartyInfluenceChangeAmount.Item3 != b.PartyInfluenceChangeAmount.Item3 || a.PartyMoraleChangeAmount != b.PartyMoraleChangeAmount || a.PartyHorseChangeAmount != b.PartyHorseChangeAmount)
			{
				return false;
			}
			if (a.RightMemberRoster.Count != b.RightMemberRoster.Count || a.RightPrisonerRoster.Count != b.RightPrisonerRoster.Count || a.RightRecruitableData.Count != b.RightRecruitableData.Count || a.UpgradedTroopsHistory.Count != b.UpgradedTroopsHistory.Count || a.TransferredPrisonersHistory.Count != b.TransferredPrisonersHistory.Count || a.RecruitedPrisonersHistory.Count != b.RecruitedPrisonersHistory.Count || a.UsedUpgradeHorsesHistory.Count != b.UsedUpgradeHorsesHistory.Count)
			{
				return false;
			}
			if (a.RightMemberRoster != b.RightMemberRoster)
			{
				return false;
			}
			if (a.RightPrisonerRoster != b.RightPrisonerRoster)
			{
				return false;
			}
			foreach (CharacterObject key in a.RightRecruitableData.Keys)
			{
				if (!b.RightRecruitableData.ContainsKey(key) || a.RightRecruitableData[key] != b.RightRecruitableData[key])
				{
					return false;
				}
			}
			for (int i = 0; i < a.UpgradedTroopsHistory.Count; i++)
			{
				if (a.UpgradedTroopsHistory[i].Item1 != b.UpgradedTroopsHistory[i].Item1 || a.UpgradedTroopsHistory[i].Item2 != b.UpgradedTroopsHistory[i].Item2 || a.UpgradedTroopsHistory[i].Item3 != b.UpgradedTroopsHistory[i].Item3)
				{
					return false;
				}
			}
			for (int j = 0; j < a.TransferredPrisonersHistory.Count; j++)
			{
				if (a.TransferredPrisonersHistory[j].Item1 != b.TransferredPrisonersHistory[j].Item1 || a.TransferredPrisonersHistory[j].Item2 != b.TransferredPrisonersHistory[j].Item2)
				{
					return false;
				}
			}
			for (int k = 0; k < a.RecruitedPrisonersHistory.Count; k++)
			{
				if (a.RecruitedPrisonersHistory[k].Item1 != b.RecruitedPrisonersHistory[k].Item1 || a.RecruitedPrisonersHistory[k].Item2 != b.RecruitedPrisonersHistory[k].Item2)
				{
					return false;
				}
			}
			for (int l = 0; l < a.UsedUpgradeHorsesHistory.Count; l++)
			{
				if (a.UsedUpgradeHorsesHistory[l].Item1.Item != b.UsedUpgradeHorsesHistory[l].Item1.Item || a.UsedUpgradeHorsesHistory[l].Item2 != b.UsedUpgradeHorsesHistory[l].Item2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000A8A18 File Offset: 0x000A6C18
		public static bool operator !=(PartyScreenData first, PartyScreenData second)
		{
			return !(first == second);
		}

		// Token: 0x04000BE7 RID: 3047
		public TroopRoster RightMemberRoster;

		// Token: 0x04000BE8 RID: 3048
		public TroopRoster LeftMemberRoster;

		// Token: 0x04000BE9 RID: 3049
		public TroopRoster RightPrisonerRoster;

		// Token: 0x04000BEA RID: 3050
		public TroopRoster LeftPrisonerRoster;

		// Token: 0x04000BEB RID: 3051
		public ItemRoster RightItemRoster;

		// Token: 0x04000BEC RID: 3052
		public Dictionary<CharacterObject, int> RightRecruitableData;

		// Token: 0x04000BED RID: 3053
		public int PartyGoldChangeAmount;

		// Token: 0x04000BEE RID: 3054
		public ValueTuple<int, int, int> PartyInfluenceChangeAmount;

		// Token: 0x04000BEF RID: 3055
		public int PartyMoraleChangeAmount;

		// Token: 0x04000BF0 RID: 3056
		public int PartyHorseChangeAmount;

		// Token: 0x04000BF1 RID: 3057
		public List<Tuple<CharacterObject, CharacterObject, int>> UpgradedTroopsHistory;

		// Token: 0x04000BF2 RID: 3058
		public List<Tuple<CharacterObject, int>> TransferredPrisonersHistory;

		// Token: 0x04000BF3 RID: 3059
		public List<Tuple<CharacterObject, int>> RecruitedPrisonersHistory;

		// Token: 0x04000BF4 RID: 3060
		public List<Tuple<EquipmentElement, int>> UsedUpgradeHorsesHistory;
	}
}
