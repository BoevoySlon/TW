using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000084 RID: 132
	public static class HeroCreator
	{
		// Token: 0x0600106F RID: 4207 RVA: 0x0004AF90 File Offset: 0x00049190
		public static Hero CreateHeroAtOccupation(Occupation neededOccupation, Settlement forcedHomeSettlement = null)
		{
			Settlement settlement = forcedHomeSettlement ?? SettlementHelper.GetRandomTown(null);
			IEnumerable<CharacterObject> enumerable = from x in settlement.Culture.NotableAndWandererTemplates
			where x.Occupation == neededOccupation
			select x;
			int num = 0;
			foreach (CharacterObject characterObject in enumerable)
			{
				int num2 = characterObject.GetTraitLevel(DefaultTraits.Frequency) * 10;
				num += ((num2 > 0) ? num2 : 100);
			}
			if (!enumerable.Any<CharacterObject>())
			{
				return null;
			}
			CharacterObject template = null;
			int num3 = settlement.RandomIntWithSeed((uint)settlement.Notables.Count, 1, num);
			foreach (CharacterObject characterObject2 in enumerable)
			{
				int num4 = characterObject2.GetTraitLevel(DefaultTraits.Frequency) * 10;
				num3 -= ((num4 > 0) ? num4 : 100);
				if (num3 < 0)
				{
					template = characterObject2;
					break;
				}
			}
			Hero hero = HeroCreator.CreateSpecialHero(template, settlement, null, null, -1);
			if (hero.HomeSettlement.IsVillage && hero.HomeSettlement.Village.Bound != null && hero.HomeSettlement.Village.Bound.IsCastle)
			{
				float value = MBRandom.RandomFloat * 20f;
				hero.AddPower(value);
			}
			if (neededOccupation != Occupation.Wanderer)
			{
				hero.ChangeState(Hero.CharacterStates.Active);
			}
			if (neededOccupation != Occupation.Wanderer)
			{
				EnterSettlementAction.ApplyForCharacterOnly(hero, settlement);
			}
			if (neededOccupation != Occupation.Wanderer)
			{
				int amount = 10000;
				GiveGoldAction.ApplyBetweenCharacters(null, hero, amount, true);
			}
			CharacterObject template2 = hero.Template;
			if (((template2 != null) ? template2.HeroObject : null) != null && hero.Template.HeroObject.Clan != null && hero.Template.HeroObject.Clan.IsMinorFaction)
			{
				hero.SupporterOf = hero.Template.HeroObject.Clan;
			}
			else
			{
				hero.SupporterOf = HeroHelper.GetRandomClanForNotable(hero);
			}
			if (neededOccupation != Occupation.Wanderer)
			{
				HeroCreator.AddRandomVarianceToTraits(hero);
			}
			return hero;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0004B1C8 File Offset: 0x000493C8
		private static Hero CreateNewHero(CharacterObject template, int age = -1)
		{
			Debug.Print("creating hero from template with id: " + template.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
			CharacterObject newCharacter = CharacterObject.CreateFrom(template);
			Hero hero = Hero.CreateHero(newCharacter.StringId);
			hero.SetCharacterObject(newCharacter);
			newCharacter.HeroObject = hero;
			CampaignTime birthDay;
			if (age == -1)
			{
				birthDay = HeroHelper.GetRandomBirthDayForAge((float)(Campaign.Current.Models.AgeModel.HeroComesOfAge + MBRandom.RandomInt(30)));
			}
			else if (age == 0)
			{
				birthDay = CampaignTime.Now;
			}
			else if (hero.IsWanderer)
			{
				age = (int)template.Age;
				if (age < 20)
				{
					foreach (TraitObject trait in TraitObject.All)
					{
						int num = 12 + 4 * template.GetTraitLevel(trait);
						if (age < num)
						{
							age = num;
						}
					}
				}
				birthDay = HeroHelper.GetRandomBirthDayForAge((float)age);
			}
			else
			{
				birthDay = HeroHelper.GetRandomBirthDayForAge((float)age);
			}
			newCharacter.HeroObject.SetBirthDay(birthDay);
			Settlement settlement = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsTown && (newCharacter.Culture.StringId == "neutral_culture" || x.Culture == newCharacter.Culture));
			if (settlement == null)
			{
				settlement = SettlementHelper.FindRandomSettlement((Settlement x) => x.IsTown);
			}
			newCharacter.HeroObject.BornSettlement = settlement;
			newCharacter.HeroObject.StaticBodyProperties = BodyProperties.GetRandomBodyProperties(template.Race, template.IsFemale, template.GetBodyPropertiesMin(false), template.GetBodyPropertiesMax(), 0, MBRandom.RandomInt(), newCharacter.HairTags, newCharacter.BeardTags, newCharacter.TattooTags).StaticProperties;
			newCharacter.HeroObject.Weight = 0f;
			newCharacter.HeroObject.Build = 0f;
			if (newCharacter.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				newCharacter.HeroObject.HeroDeveloper.InitializeHeroDeveloper(false, null);
			}
			hero.PreferredUpgradeFormation = HeroCreator.GetRandomPreferredUpgradeFormation();
			return newCharacter.HeroObject;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0004B418 File Offset: 0x00049618
		public static Hero CreateSpecialHero(CharacterObject template, Settlement bornSettlement = null, Clan faction = null, Clan supporterOfClan = null, int age = -1)
		{
			Hero hero = HeroCreator.CreateNewHero(template, age);
			CultureObject culture = template.Culture;
			if (culture == null && bornSettlement != null)
			{
				culture = bornSettlement.Culture;
			}
			else if (culture == null && faction != null)
			{
				culture = faction.Culture;
			}
			if (faction != null)
			{
				hero.Clan = faction;
			}
			if (supporterOfClan != null)
			{
				hero.SupporterOf = supporterOfClan;
			}
			if (bornSettlement != null)
			{
				hero.BornSettlement = bornSettlement;
			}
			hero.CharacterObject.Culture = culture;
			TextObject firstName;
			TextObject fullName;
			NameGenerator.Current.GenerateHeroNameAndHeroFullName(hero, out firstName, out fullName, false);
			hero.SetName(fullName, firstName);
			HeroCreator.ModifyAppearanceByTraits(hero);
			CampaignEventDispatcher.Instance.OnHeroCreated(hero, false);
			return hero;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0004B4A8 File Offset: 0x000496A8
		public static Hero CreateRelativeNotableHero(Hero relative)
		{
			Hero hero = HeroCreator.CreateHeroAtOccupation(relative.CharacterObject.Occupation, relative.HomeSettlement);
			hero.Culture = relative.Culture;
			BodyProperties bodyPropertiesMin = relative.CharacterObject.GetBodyPropertiesMin(false);
			BodyProperties bodyPropertiesMin2 = hero.CharacterObject.GetBodyPropertiesMin(false);
			int defaultFaceSeed = relative.CharacterObject.GetDefaultFaceSeed(1);
			string hairTags = relative.HairTags;
			string tattooTags = relative.TattooTags;
			hero.StaticBodyProperties = BodyProperties.GetRandomBodyProperties(hero.CharacterObject.Race, hero.IsFemale, bodyPropertiesMin, bodyPropertiesMin2, 1, defaultFaceSeed, hairTags, relative.BeardTags, tattooTags).StaticProperties;
			return hero;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0004B544 File Offset: 0x00049744
		public static bool CreateBasicHero(CharacterObject character, out Hero hero, string stringId = "")
		{
			if (string.IsNullOrEmpty(stringId))
			{
				hero = HeroCreator.CreateNewHero(character, -1);
				CampaignEventDispatcher.Instance.OnHeroCreated(hero, false);
				return true;
			}
			hero = Campaign.Current.CampaignObjectManager.Find<Hero>(stringId);
			if (hero != null)
			{
				return false;
			}
			hero = Hero.CreateHero(stringId);
			hero.SetCharacterObject(character);
			hero.HeroDeveloper.InitializeHeroDeveloper(false, hero.Template);
			hero.StaticBodyProperties = character.GetBodyPropertiesMin(false).StaticProperties;
			hero.Weight = 0f;
			hero.Build = 0f;
			character.HeroObject = hero;
			hero.PreferredUpgradeFormation = HeroCreator.GetRandomPreferredUpgradeFormation();
			CampaignEventDispatcher.Instance.OnHeroCreated(hero, false);
			return true;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0004B600 File Offset: 0x00049800
		private static void ModifyAppearanceByTraits(Hero hero)
		{
			int num = MBRandom.RandomInt(0, 100);
			int num2 = MBRandom.RandomInt(0, 100);
			if (hero.Age >= 40f)
			{
				num -= 30;
				num2 += 30;
			}
			int hair = -1;
			int beard = -1;
			int tattoo = -1;
			bool flag = hero.HairTags.IsEmpty<char>() && hero.BeardTags.IsEmpty<char>();
			if (hero.GetTraitLevel(DefaultTraits.RomanHair) > 0 && !hero.IsFemale && flag)
			{
				if (num < 0)
				{
					hair = 0;
				}
				else if (num < 20)
				{
					hair = 13;
				}
				else if (num < 70)
				{
					hair = 8;
				}
				else
				{
					hair = 6;
				}
				if (num2 < 60)
				{
					beard = 0;
				}
				else if (num2 < 110)
				{
					beard = 13;
				}
				else
				{
					beard = 14;
				}
			}
			else if (hero.GetTraitLevel(DefaultTraits.CelticHair) > 0 && !hero.IsFemale && flag)
			{
				if (num < 0)
				{
					hair = 0;
				}
				else if (num < 20)
				{
					hair = 13;
				}
				else if (num < 40)
				{
					hair = 6;
				}
				else if (num < 60)
				{
					hair = 14;
				}
				else if (num < 85)
				{
					hair = 2;
				}
				else
				{
					hair = 7;
				}
				if (num2 < 40)
				{
					beard = 1;
				}
				else if (num2 < 60)
				{
					beard = 2;
				}
				else if (num2 < 110)
				{
					beard = 3;
				}
				else
				{
					beard = 5;
				}
			}
			else if (hero.GetTraitLevel(DefaultTraits.ArabianHair) > 0 && !hero.IsFemale && flag)
			{
				if (num < 0)
				{
					hair = 0;
				}
				else if (num < 20)
				{
					hair = 13;
				}
				else if (num < 40)
				{
					hair = 6;
				}
				else if (num < 60)
				{
					hair = 2;
				}
				else if (num < 85)
				{
					hair = 11;
				}
				else
				{
					hair = 7;
				}
				if (num2 < 40)
				{
					beard = 0;
				}
				else if (num2 < 50)
				{
					beard = 6;
				}
				else if (num2 < 60)
				{
					beard = 12;
				}
				else if (num2 < 70)
				{
					beard = 8;
				}
				else if (num2 < 80)
				{
					beard = 15;
				}
				else if (num2 < 100)
				{
					beard = 9;
				}
				else
				{
					beard = 17;
				}
			}
			else if (hero.GetTraitLevel(DefaultTraits.RusHair) > 0 && !hero.IsFemale && flag)
			{
				if (num < 0)
				{
					hair = 0;
				}
				else if (num < 40)
				{
					hair = 6;
				}
				else if (num < 60)
				{
					hair = 12;
				}
				else if (num < 85)
				{
					hair = 11;
				}
				else
				{
					hair = 2;
				}
				if (num2 < 30)
				{
					beard = 0;
				}
				else if (num2 < 60)
				{
					beard = 13;
				}
				else if (num2 < 70)
				{
					beard = 17;
				}
				else if (num2 < 90)
				{
					beard = 18;
				}
				else
				{
					beard = 19;
				}
			}
			hero.ModifyHair(hair, beard, tattoo);
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0004B844 File Offset: 0x00049A44
		private static void AddRandomVarianceToTraits(Hero hero)
		{
			foreach (TraitObject traitObject in TraitObject.All)
			{
				if (traitObject == DefaultTraits.Honor || traitObject == DefaultTraits.Mercy || traitObject == DefaultTraits.Generosity || traitObject == DefaultTraits.Valor || traitObject == DefaultTraits.Calculating)
				{
					int num = hero.CharacterObject.GetTraitLevel(traitObject);
					float num2 = MBRandom.RandomFloat;
					if (hero.IsPreacher && traitObject == DefaultTraits.Generosity)
					{
						num2 = 0.5f;
					}
					if (hero.IsMerchant && traitObject == DefaultTraits.Calculating)
					{
						num2 = 0.5f;
					}
					if ((double)num2 < 0.25)
					{
						num--;
						if (num < -1)
						{
							num = -1;
						}
					}
					if ((double)num2 > 0.75)
					{
						num++;
						if (num > 1)
						{
							num = 1;
						}
					}
					if (hero.IsGangLeader && (traitObject == DefaultTraits.Mercy || traitObject == DefaultTraits.Honor) && num > 0)
					{
						num = 0;
					}
					num = MBMath.ClampInt(num, traitObject.MinValue, traitObject.MaxValue);
					hero.SetTraitLevel(traitObject, num);
				}
			}
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0004B968 File Offset: 0x00049B68
		public static Hero DeliverOffSpring(Hero mother, Hero father, bool isOffspringFemale)
		{
			Debug.SilentAssert(mother.CharacterObject.Race == father.CharacterObject.Race, "", false, "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\HeroCreator.cs", "DeliverOffSpring", 484);
			Hero hero = HeroCreator.CreateNewHero(isOffspringFemale ? mother.CharacterObject : father.CharacterObject, 0);
			hero.ClearTraits();
			float randomFloat = MBRandom.RandomFloat;
			int val;
			if ((double)randomFloat < 0.1)
			{
				val = 0;
			}
			else if ((double)randomFloat < 0.5)
			{
				val = 1;
			}
			else if ((double)randomFloat < 0.9)
			{
				val = 2;
			}
			else
			{
				val = 3;
			}
			List<TraitObject> list = DefaultTraits.Personality.ToList<TraitObject>();
			list.Shuffle<TraitObject>();
			for (int i = 0; i < Math.Min(list.Count, val); i++)
			{
				int value = ((double)MBRandom.RandomFloat < 0.5) ? MBRandom.RandomInt(list[i].MinValue, 0) : MBRandom.RandomInt(1, list[i].MaxValue + 1);
				hero.SetTraitLevel(list[i], value);
			}
			foreach (TraitObject trait in TraitObject.All.Except(DefaultTraits.Personality))
			{
				hero.SetTraitLevel(trait, ((double)MBRandom.RandomFloat < 0.5) ? mother.GetTraitLevel(trait) : father.GetTraitLevel(trait));
			}
			hero.SetNewOccupation(isOffspringFemale ? mother.Occupation : father.Occupation);
			int becomeChildAge = Campaign.Current.Models.AgeModel.BecomeChildAge;
			hero.CharacterObject.IsFemale = isOffspringFemale;
			hero.Mother = mother;
			hero.Father = father;
			MBEquipmentRoster randomElementInefficiently = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentRostersForDeliveredOffspring(hero).GetRandomElementInefficiently<MBEquipmentRoster>();
			if (randomElementInefficiently != null)
			{
				Equipment randomElementInefficiently2 = randomElementInefficiently.GetCivilianEquipments().GetRandomElementInefficiently<Equipment>();
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, randomElementInefficiently2);
				Equipment equipment = new Equipment(false);
				equipment.FillFrom(randomElementInefficiently2, false);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipment);
			}
			else
			{
				Debug.FailedAssert("Equipment template not found", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\HeroCreator.cs", "DeliverOffSpring", 549);
			}
			TextObject firstName;
			TextObject fullName;
			NameGenerator.Current.GenerateHeroNameAndHeroFullName(hero, out firstName, out fullName, false);
			hero.SetName(fullName, firstName);
			hero.HeroDeveloper.InitializeHeroDeveloper(true, null);
			BodyProperties bodyProperties = mother.BodyProperties;
			BodyProperties bodyProperties2 = father.BodyProperties;
			int seed = MBRandom.RandomInt();
			string hairTags = isOffspringFemale ? mother.HairTags : father.HairTags;
			string tattooTags = isOffspringFemale ? mother.TattooTags : father.TattooTags;
			hero.StaticBodyProperties = BodyProperties.GetRandomBodyProperties(mother.CharacterObject.Race, isOffspringFemale, bodyProperties, bodyProperties2, 1, seed, hairTags, father.BeardTags, tattooTags).StaticProperties;
			hero.BornSettlement = HeroCreator.DecideBornSettlement(hero);
			if (mother == Hero.MainHero || father == Hero.MainHero)
			{
				hero.Clan = Clan.PlayerClan;
				hero.Culture = Hero.MainHero.Culture;
			}
			else
			{
				hero.Clan = father.Clan;
				hero.Culture = (((double)MBRandom.RandomFloat < 0.5) ? father.Culture : mother.Culture);
			}
			CampaignEventDispatcher.Instance.OnHeroCreated(hero, true);
			int heroComesOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
			if (hero.Age > (float)becomeChildAge || (hero.Age == (float)becomeChildAge && hero.BirthDay.GetDayOfYear < CampaignTime.Now.GetDayOfYear))
			{
				CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
			}
			if (hero.Age > (float)heroComesOfAge || (hero.Age == (float)heroComesOfAge && hero.BirthDay.GetDayOfYear < CampaignTime.Now.GetDayOfYear))
			{
				CampaignEventDispatcher.Instance.OnHeroComesOfAge(hero);
			}
			return hero;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0004BD44 File Offset: 0x00049F44
		private static Settlement DecideBornSettlement(Hero child)
		{
			Settlement settlement;
			if (child.Mother.CurrentSettlement != null && (child.Mother.CurrentSettlement.IsTown || child.Mother.CurrentSettlement.IsVillage))
			{
				settlement = child.Mother.CurrentSettlement;
			}
			else if (child.Mother.PartyBelongedTo != null || child.Mother.PartyBelongedToAsPrisoner != null)
			{
				IMapPoint toMapPoint;
				if (child.Mother.PartyBelongedToAsPrisoner != null)
				{
					IMapPoint mapPoint2;
					if (!child.Mother.PartyBelongedToAsPrisoner.IsMobile)
					{
						IMapPoint mapPoint = child.Mother.PartyBelongedToAsPrisoner.Settlement;
						mapPoint2 = mapPoint;
					}
					else
					{
						IMapPoint mapPoint = child.Mother.PartyBelongedToAsPrisoner.MobileParty;
						mapPoint2 = mapPoint;
					}
					toMapPoint = mapPoint2;
				}
				else
				{
					toMapPoint = child.Mother.PartyBelongedTo;
				}
				settlement = SettlementHelper.FindNearestTown(null, toMapPoint);
			}
			else
			{
				settlement = child.Mother.HomeSettlement;
			}
			if (settlement == null)
			{
				settlement = ((child.Mother.Clan.Settlements.Count > 0) ? child.Mother.Clan.Settlements.GetRandomElement<Settlement>() : Town.AllTowns.GetRandomElement<Town>().Settlement);
			}
			return settlement;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0004BE5C File Offset: 0x0004A05C
		private static FormationClass GetRandomPreferredUpgradeFormation()
		{
			int num = MBRandom.RandomInt(10);
			if (num < 4)
			{
				return (FormationClass)num;
			}
			return FormationClass.NumberOfAllFormations;
		}
	}
}
