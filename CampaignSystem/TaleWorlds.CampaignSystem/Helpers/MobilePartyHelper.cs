using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Helpers
{
	// Token: 0x0200000E RID: 14
	public static class MobilePartyHelper
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00006A0E File Offset: 0x00004C0E
		public static MobileParty SpawnLordParty(Hero hero, Settlement spawnSettlement)
		{
			return MobilePartyHelper.SpawnLordPartyAux(hero, spawnSettlement.GatePosition, 0f, spawnSettlement);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00006A22 File Offset: 0x00004C22
		public static MobileParty SpawnLordParty(Hero hero, Vec2 position, float spawnRadius)
		{
			return MobilePartyHelper.SpawnLordPartyAux(hero, position, spawnRadius, null);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00006A2D File Offset: 0x00004C2D
		private static MobileParty SpawnLordPartyAux(Hero hero, Vec2 position, float spawnRadius, Settlement spawnSettlement)
		{
			return LordPartyComponent.CreateLordParty(hero.CharacterObject.StringId, hero, position, spawnRadius, spawnSettlement, hero);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00006A44 File Offset: 0x00004C44
		public static void CreateNewClanMobileParty(Hero partyLeader, Clan clan, out bool leaderCameFromMainParty)
		{
			leaderCameFromMainParty = PartyBase.MainParty.MemberRoster.Contains(partyLeader.CharacterObject);
			GiveGoldAction.ApplyBetweenCharacters(null, partyLeader, 3000, true);
			clan.CreateNewMobileParty(partyLeader).Ai.SetMoveModeHold();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00006A7C File Offset: 0x00004C7C
		public static void DesertTroopsFromParty(MobileParty party, int stackNo, int numberOfDeserters, int numberOfWoundedDeserters, ref TroopRoster desertedTroopList)
		{
			TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(stackNo);
			party.MemberRoster.AddToCounts(elementCopyAtIndex.Character, -(numberOfDeserters + numberOfWoundedDeserters), false, -numberOfWoundedDeserters, 0, true, -1);
			if (desertedTroopList == null)
			{
				desertedTroopList = TroopRoster.CreateDummyTroopRoster();
			}
			desertedTroopList.AddToCounts(elementCopyAtIndex.Character, numberOfDeserters + numberOfWoundedDeserters, false, numberOfWoundedDeserters, 0, true, -1);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00006ADC File Offset: 0x00004CDC
		public static bool IsHeroAssignableForScoutInParty(Hero hero, MobileParty party)
		{
			return hero.PartyBelongedTo == party && hero != party.GetRoleHolder(SkillEffect.PerkRole.Scout) && hero.GetSkillValue(DefaultSkills.Scouting) >= 0;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00006B05 File Offset: 0x00004D05
		public static bool IsHeroAssignableForEngineerInParty(Hero hero, MobileParty party)
		{
			return hero.PartyBelongedTo == party && hero != party.GetRoleHolder(SkillEffect.PerkRole.Engineer) && hero.GetSkillValue(DefaultSkills.Engineering) >= 0;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00006B2D File Offset: 0x00004D2D
		public static bool IsHeroAssignableForSurgeonInParty(Hero hero, MobileParty party)
		{
			return hero.PartyBelongedTo == party && hero != party.GetRoleHolder(SkillEffect.PerkRole.Surgeon) && hero.GetSkillValue(DefaultSkills.Medicine) >= 0;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006B55 File Offset: 0x00004D55
		public static bool IsHeroAssignableForQuartermasterInParty(Hero hero, MobileParty party)
		{
			return hero.PartyBelongedTo == party && hero != party.GetRoleHolder(SkillEffect.PerkRole.Quartermaster) && hero.GetSkillValue(DefaultSkills.Trade) >= 0;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006B80 File Offset: 0x00004D80
		public static Hero GetHeroWithHighestSkill(MobileParty party, SkillObject skill)
		{
			Hero result = null;
			int num = -1;
			for (int i = 0; i < party.MemberRoster.Count; i++)
			{
				CharacterObject characterAtIndex = party.MemberRoster.GetCharacterAtIndex(i);
				if (characterAtIndex.HeroObject != null && characterAtIndex.HeroObject.GetSkillValue(skill) > num)
				{
					num = characterAtIndex.HeroObject.GetSkillValue(skill);
					result = characterAtIndex.HeroObject;
				}
			}
			return result;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public static TroopRoster GetStrongestAndPriorTroops(MobileParty mobileParty, int maxTroopCount, bool includePlayer)
		{
			FlattenedTroopRoster flattenedTroopRoster = mobileParty.MemberRoster.ToFlattenedRoster();
			flattenedTroopRoster.RemoveIf((FlattenedTroopRosterElement x) => x.IsWounded);
			return MobilePartyHelper.GetStrongestAndPriorTroops(flattenedTroopRoster, maxTroopCount, includePlayer);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006C1C File Offset: 0x00004E1C
		public static TroopRoster GetStrongestAndPriorTroops(FlattenedTroopRoster roster, int maxTroopCount, bool includePlayer)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			List<CharacterObject> list = (from x in roster
			select x.Troop into x
			orderby x.Level descending
			select x).ToList<CharacterObject>();
			if (list.Any((CharacterObject x) => x.IsPlayerCharacter))
			{
				list.Remove(CharacterObject.PlayerCharacter);
				if (includePlayer)
				{
					troopRoster.AddToCounts(CharacterObject.PlayerCharacter, 1, false, 0, 0, true, -1);
					maxTroopCount--;
				}
			}
			List<CharacterObject> list2 = (from x in list
			where x.IsNotTransferableInPartyScreen && x.IsHero
			select x).ToList<CharacterObject>();
			int num = MathF.Min(list2.Count, maxTroopCount);
			for (int i = 0; i < num; i++)
			{
				troopRoster.AddToCounts(list2[i], 1, false, 0, 0, true, -1);
				list.Remove(list2[i]);
			}
			int count = list.Count;
			int num2 = num;
			while (num2 < maxTroopCount && num2 < count)
			{
				troopRoster.AddToCounts(list[num2], 1, false, 0, 0, true, -1);
				num2++;
			}
			return troopRoster;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006D6C File Offset: 0x00004F6C
		public static int GetMaximumXpAmountPartyCanGet(MobileParty party)
		{
			TroopRoster memberRoster = party.MemberRoster;
			int num = 0;
			for (int i = 0; i < memberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = memberRoster.GetElementCopyAtIndex(i);
				int num2;
				if (MobilePartyHelper.CanTroopGainXp(party.Party, elementCopyAtIndex.Character, out num2))
				{
					num += num2;
				}
			}
			return num;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public static void PartyAddSharedXp(MobileParty party, float xpToDistribute)
		{
			if (xpToDistribute > 0f)
			{
				TroopRoster memberRoster = party.MemberRoster;
				int num = 0;
				for (int i = 0; i < memberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = memberRoster.GetElementCopyAtIndex(i);
					int num2;
					if (MobilePartyHelper.CanTroopGainXp(party.Party, elementCopyAtIndex.Character, out num2))
					{
						num += num2;
					}
				}
				int num3 = 0;
				while (num3 < memberRoster.Count && xpToDistribute >= 1f && num > 0)
				{
					TroopRosterElement elementCopyAtIndex2 = memberRoster.GetElementCopyAtIndex(num3);
					int num4;
					if (MobilePartyHelper.CanTroopGainXp(party.Party, elementCopyAtIndex2.Character, out num4))
					{
						int num5 = MathF.Floor(MathF.Max(1f, xpToDistribute * (float)num4 / (float)num));
						memberRoster.AddXpToTroopAtIndex(num5, num3);
						xpToDistribute -= (float)num5;
						num -= num4;
					}
					num3++;
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00006E80 File Offset: 0x00005080
		public static bool CanTroopGainXp(PartyBase owner, CharacterObject character, out int gainableMaxXp)
		{
			bool result = false;
			gainableMaxXp = 0;
			int index = owner.MemberRoster.FindIndexOfTroop(character);
			int elementNumber = owner.MemberRoster.GetElementNumber(index);
			int elementXp = owner.MemberRoster.GetElementXp(index);
			for (int i = 0; i < character.UpgradeTargets.Length; i++)
			{
				int upgradeXpCost = character.GetUpgradeXpCost(owner, i);
				if (elementXp < upgradeXpCost * elementNumber)
				{
					result = true;
					int num = upgradeXpCost * elementNumber - elementXp;
					if (num > gainableMaxXp)
					{
						gainableMaxXp = num;
					}
				}
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006EFC File Offset: 0x000050FC
		public static Vec2 FindReachablePointAroundPosition(Vec2 centerPosition, float maxDistance, float minDistance = 0f)
		{
			Vec2 vec = new Vec2(centerPosition.x, centerPosition.y);
			PathFaceRecord faceIndex = Campaign.Current.MapSceneWrapper.GetFaceIndex(centerPosition);
			Vec2 vec2 = centerPosition;
			if (maxDistance > 0f)
			{
				int num = 0;
				do
				{
					num++;
					Vec2 vec3 = Vec2.One.Normalized();
					vec3.RotateCCW(MBRandom.RandomFloatRanged(0f, 6.2831855f));
					vec3 *= MBRandom.RandomFloatRanged(minDistance, maxDistance);
					vec = centerPosition + vec3;
					PathFaceRecord faceIndex2 = Campaign.Current.MapSceneWrapper.GetFaceIndex(vec);
					if (faceIndex2.IsValid() && Campaign.Current.MapSceneWrapper.AreFacesOnSameIsland(faceIndex2, faceIndex, false))
					{
						vec2 = vec;
					}
				}
				while (vec2 == centerPosition && num < 250);
			}
			return vec2;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006FC8 File Offset: 0x000051C8
		public static void TryMatchPartySpeedWithItemWeight(MobileParty party, float targetPartySpeed, ItemObject itemToUse = null)
		{
			targetPartySpeed = MathF.Max(1f, targetPartySpeed);
			ItemObject item = itemToUse ?? DefaultItems.HardWood;
			float speed = party.Speed;
			int num = MathF.Sign(speed - targetPartySpeed);
			int num2 = 0;
			while (num2 < 200 && MathF.Abs(speed - targetPartySpeed) >= 0.1f && MathF.Sign(speed - targetPartySpeed) == num)
			{
				if (speed >= targetPartySpeed)
				{
					party.ItemRoster.AddToCounts(item, 1);
				}
				else
				{
					if (party.ItemRoster.GetItemNumber(item) <= 0)
					{
						break;
					}
					party.ItemRoster.AddToCounts(item, -1);
				}
				speed = party.Speed;
				num2++;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00007060 File Offset: 0x00005260
		public static void UtilizePartyEscortBehavior(MobileParty escortedParty, MobileParty escortParty, ref bool isWaitingForEscortParty, float innerRadius, float outerRadius, MobilePartyHelper.ResumePartyEscortBehaviorDelegate onPartyEscortBehaviorResumed, bool showDebugSpheres = false)
		{
			if (!isWaitingForEscortParty)
			{
				if (escortParty.Position2D.DistanceSquared(escortedParty.Position2D) >= outerRadius * outerRadius)
				{
					escortedParty.Ai.SetMoveGoToPoint(escortedParty.Position2D);
					escortedParty.Ai.CheckPartyNeedsUpdate();
					isWaitingForEscortParty = true;
					return;
				}
			}
			else if (escortParty.Position2D.DistanceSquared(escortedParty.Position2D) <= innerRadius * innerRadius)
			{
				onPartyEscortBehaviorResumed();
				escortedParty.Ai.CheckPartyNeedsUpdate();
				isWaitingForEscortParty = false;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000070E0 File Offset: 0x000052E0
		public static Hero GetMainPartySkillCounsellor(SkillObject skill)
		{
			PartyBase mainParty = PartyBase.MainParty;
			Hero hero = null;
			int num = 0;
			for (int i = 0; i < mainParty.MemberRoster.Count; i++)
			{
				CharacterObject characterAtIndex = mainParty.MemberRoster.GetCharacterAtIndex(i);
				if (characterAtIndex.IsHero && !characterAtIndex.HeroObject.IsWounded)
				{
					int skillValue = characterAtIndex.GetSkillValue(skill);
					if (skillValue >= num)
					{
						num = skillValue;
						hero = characterAtIndex.HeroObject;
					}
				}
			}
			return hero ?? mainParty.LeaderHero;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00007158 File Offset: 0x00005358
		public static Settlement GetCurrentSettlementOfMobilePartyForAICalculation(MobileParty mobileParty)
		{
			Settlement result;
			if ((result = mobileParty.CurrentSettlement) == null)
			{
				if (mobileParty.LastVisitedSettlement == null || mobileParty.LastVisitedSettlement.Position2D.DistanceSquared(mobileParty.Position2D) >= 1f)
				{
					return null;
				}
				result = mobileParty.LastVisitedSettlement;
			}
			return result;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000071A0 File Offset: 0x000053A0
		public static TroopRoster GetPlayerPrisonersPlayerCanSell()
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			List<string> list = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyPrisonerLocks().ToList<string>();
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.PrisonRoster.GetTroopRoster())
			{
				if (!list.Contains(troopRosterElement.Character.StringId))
				{
					troopRoster.Add(troopRosterElement);
				}
			}
			return troopRoster;
		}

		// Token: 0x02000465 RID: 1125
		// (Invoke) Token: 0x0600415C RID: 16732
		public delegate void ResumePartyEscortBehaviorDelegate();
	}
}
