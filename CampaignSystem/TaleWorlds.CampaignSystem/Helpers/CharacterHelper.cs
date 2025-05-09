﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000012 RID: 18
	public static class CharacterHelper
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00007880 File Offset: 0x00005A80
		public static DynamicBodyProperties GetDynamicBodyPropertiesBetweenMinMaxRange(CharacterObject character)
		{
			BodyProperties bodyPropertyMin = character.BodyPropertyRange.BodyPropertyMin;
			BodyProperties bodyPropertyMax = character.BodyPropertyRange.BodyPropertyMax;
			float minVal = (bodyPropertyMin.Age < bodyPropertyMax.Age) ? bodyPropertyMin.Age : bodyPropertyMax.Age;
			float maxVal = (bodyPropertyMin.Age > bodyPropertyMax.Age) ? bodyPropertyMin.Age : bodyPropertyMax.Age;
			float minVal2 = (bodyPropertyMin.Weight < bodyPropertyMax.Weight) ? bodyPropertyMin.Weight : bodyPropertyMax.Weight;
			float maxVal2 = (bodyPropertyMin.Weight > bodyPropertyMax.Weight) ? bodyPropertyMin.Weight : bodyPropertyMax.Weight;
			float minVal3 = (bodyPropertyMin.Build < bodyPropertyMax.Build) ? bodyPropertyMin.Build : bodyPropertyMax.Build;
			float maxVal3 = (bodyPropertyMin.Build > bodyPropertyMax.Build) ? bodyPropertyMin.Build : bodyPropertyMax.Build;
			float age = MBRandom.RandomFloatRanged(minVal, maxVal);
			float weight = MBRandom.RandomFloatRanged(minVal2, maxVal2);
			float build = MBRandom.RandomFloatRanged(minVal3, maxVal3);
			return new DynamicBodyProperties(age, weight, build);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00007994 File Offset: 0x00005B94
		public static TextObject GetReputationDescription(CharacterObject character)
		{
			TextObject textObject = new TextObject("{=!}{REPUTATION_SUMMARY}", null);
			TextObject textObject2 = Campaign.Current.ConversationManager.FindMatchingTextOrNull("reputation", character);
			StringHelpers.SetCharacterProperties("NOTABLE", character, textObject2, false);
			textObject.SetTextVariable("REPUTATION_SUMMARY", textObject2);
			return textObject;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000079E0 File Offset: 0x00005BE0
		[return: TupleElementNames(new string[]
		{
			"color1",
			"color2"
		})]
		public static ValueTuple<uint, uint> GetDeterministicColorsForCharacter(CharacterObject character)
		{
			Hero heroObject = character.HeroObject;
			CultureObject cultureObject = (((heroObject != null) ? heroObject.MapFaction : null) != null) ? character.HeroObject.MapFaction.Culture : character.Culture;
			if (!character.IsHero)
			{
				return new ValueTuple<uint, uint>(cultureObject.Color, cultureObject.Color2);
			}
			if (character.Occupation == Occupation.Lord)
			{
				IFaction mapFaction = character.HeroObject.MapFaction;
				uint item = (mapFaction != null) ? mapFaction.Color : 4291609515U;
				IFaction mapFaction2 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>(item, (mapFaction2 != null) ? mapFaction2.Color2 : 4291609515U);
			}
			string stringId = cultureObject.StringId;
			if (stringId == "empire")
			{
				IFaction mapFaction3 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>((mapFaction3 != null) ? mapFaction3.Color : 4291609515U, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, CampaignData.EmpireHeroClothColors));
			}
			if (stringId == "sturgia")
			{
				IFaction mapFaction4 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>((mapFaction4 != null) ? mapFaction4.Color : 4291609515U, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, CampaignData.SturgiaHeroClothColors));
			}
			if (stringId == "aserai")
			{
				IFaction mapFaction5 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>((mapFaction5 != null) ? mapFaction5.Color : 4291609515U, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, CampaignData.AseraiHeroClothColors));
			}
			if (stringId == "vlandia")
			{
				IFaction mapFaction6 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>((mapFaction6 != null) ? mapFaction6.Color : 4291609515U, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, CampaignData.VlandiaHeroClothColors));
			}
			if (stringId == "battania")
			{
				IFaction mapFaction7 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>((mapFaction7 != null) ? mapFaction7.Color : 4291609515U, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, CampaignData.BattaniaHeroClothColors));
			}
			if (!(stringId == "khuzait"))
			{
				IFaction mapFaction8 = character.HeroObject.MapFaction;
				return new ValueTuple<uint, uint>((mapFaction8 != null) ? mapFaction8.Color : 4291609515U, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, CampaignData.EmpireHeroClothColors));
			}
			IFaction mapFaction9 = character.HeroObject.MapFaction;
			return new ValueTuple<uint, uint>((mapFaction9 != null) ? mapFaction9.Color : 4291609515U, CharacterHelper.GetDeterministicColorFromListForHero(character.HeroObject, CampaignData.KhuzaitHeroClothColors));
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00007C38 File Offset: 0x00005E38
		private static uint GetDeterministicColorFromListForHero(Hero hero, uint[] colors)
		{
			return colors.ElementAt(hero.RandomIntWithSeed(39U) % colors.Length);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00007C4C File Offset: 0x00005E4C
		public static IFaceGeneratorCustomFilter GetFaceGeneratorFilter()
		{
			IFacegenCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IFacegenCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return null;
			}
			return campaignBehavior.GetFaceGenFilter();
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00007C64 File Offset: 0x00005E64
		public static string GetNonconversationPose(CharacterObject character)
		{
			if (character.HeroObject.IsGangLeader)
			{
				return "aggressive";
			}
			if (!character.HeroObject.IsNoncombatant && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) <= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
			{
				return "aggressive2";
			}
			if (!character.HeroObject.IsNoncombatant && character.HeroObject.IsLord && character.GetPersona() == DefaultTraits.PersonaCurt && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
			{
				return "warrior2";
			}
			if (character.HeroObject.Clan != null && character.HeroObject.Clan.IsNoble && character.GetPersona() == DefaultTraits.PersonaEarnest && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) >= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) >= 0)
			{
				return "hip2";
			}
			if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				return "demure";
			}
			if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaIronic)
			{
				return "confident3";
			}
			if (character.GetPersona() == DefaultTraits.PersonaCurt)
			{
				return "closed2";
			}
			if (character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				return "demure2";
			}
			if (character.GetPersona() == DefaultTraits.PersonaIronic)
			{
				return "confident";
			}
			if (character.GetPersona() == DefaultTraits.PersonaEarnest)
			{
				return "normal2";
			}
			return "normal";
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00007DDC File Offset: 0x00005FDC
		public static string GetNonconversationFacialIdle(CharacterObject character)
		{
			string result = "convo_normal";
			string result2 = "convo_bemused";
			string result3 = "convo_mocking_teasing";
			string result4 = "convo_mocking_revenge";
			string result5 = "convo_delighted";
			string result6 = "convo_approving";
			string result7 = "convo_thinking";
			string result8 = "convo_focused_happy";
			string result9 = "convo_calm_friendly";
			string result10 = "convo_annoyed";
			string result11 = "convo_undecided_closed";
			string result12 = "convo_bored";
			string result13 = "convo_grave";
			string result14 = "convo_predatory";
			string result15 = "convo_confused_annoyed";
			if (character.HeroObject.IsGangLeader)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) <= 0 && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
				{
					return result14;
				}
				return result15;
			}
			else if (character.GetPersona() == DefaultTraits.PersonaCurt)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0)
				{
					return result12;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
				{
					return result11;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
				{
					return result10;
				}
				return result13;
			}
			else if (character.GetPersona() == DefaultTraits.PersonaEarnest)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) > 0)
				{
					return result8;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0)
				{
					return result12;
				}
				return result5;
			}
			else if (character.IsFemale && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
			{
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) > 0)
				{
					return result9;
				}
				if (!character.HeroObject.IsNoncombatant)
				{
					return result7;
				}
				return result6;
			}
			else
			{
				if (character.GetPersona() != DefaultTraits.PersonaIronic)
				{
					return result;
				}
				if (!character.HeroObject.IsNoncombatant && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
				{
					return result4;
				}
				if (character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0)
				{
					return result3;
				}
				return result2;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00007F9C File Offset: 0x0000619C
		public static string GetStandingBodyIdle(CharacterObject character)
		{
			HeroHelper.WillLordAttack();
			string result = "normal";
			TraitObject persona = character.GetPersona();
			bool flag = Settlement.CurrentSettlement != null;
			if (character.IsHero)
			{
				if (character.HeroObject.IsWounded)
				{
					return (MBRandom.RandomFloat <= 0.7f) ? "weary" : "weary2";
				}
				bool flag2 = !character.HeroObject.IsHumanPlayerCharacter;
				int superiorityState = CharacterHelper.GetSuperiorityState(character);
				if (flag2)
				{
					int relation = Hero.MainHero.GetRelation(character.HeroObject);
					bool flag3 = CharacterHelper.MorePowerThanPlayer(character);
					if (character.IsFemale && character.HeroObject.IsNoncombatant)
					{
						if (relation < 0)
						{
							result = "closed";
						}
						else if (persona == DefaultTraits.PersonaIronic)
						{
							result = ((MBRandom.RandomFloat <= 0.5f) ? "confident" : "confident2");
						}
						else if (persona == DefaultTraits.PersonaCurt)
						{
							result = ((MBRandom.RandomFloat <= 0.5f) ? "closed" : "confident");
						}
						else if (persona == DefaultTraits.PersonaEarnest || persona == DefaultTraits.PersonaSoftspoken)
						{
							result = ((MBRandom.RandomFloat <= 0.7f) ? "demure" : "confident");
						}
					}
					else if (relation <= -20)
					{
						if (superiorityState >= 0)
						{
							if (persona == DefaultTraits.PersonaSoftspoken)
							{
								result = (character.IsFemale ? "closed" : "warrior2");
							}
							else if (persona == DefaultTraits.PersonaIronic)
							{
								result = (character.IsFemale ? "confident2" : "aggressive");
							}
							else
							{
								result = (character.IsFemale ? "confident2" : "warrior");
							}
						}
						else if (superiorityState == -1)
						{
							if (persona == DefaultTraits.PersonaSoftspoken)
							{
								if (flag3)
								{
									result = "closed";
								}
								else
								{
									result = (character.IsFemale ? "closed" : "normal");
								}
							}
							else if (persona == DefaultTraits.PersonaIronic)
							{
								if (flag3)
								{
									result = ((MBRandom.RandomFloat <= 0.5f) ? "closed" : "warrior");
								}
								else
								{
									result = "closed";
								}
							}
							else
							{
								result = (character.IsFemale ? "closed" : "warrior2");
							}
						}
					}
					else if (superiorityState >= 0)
					{
						if (persona == DefaultTraits.PersonaIronic)
						{
							if (flag)
							{
								if (flag3)
								{
									result = ((MBRandom.RandomFloat <= 0.7f) ? "confident2" : "normal");
								}
								else
								{
									result = ((MBRandom.RandomFloat <= 0.5f) ? "hip" : "normal");
								}
							}
							else
							{
								result = "confident2";
							}
						}
						else if (persona == DefaultTraits.PersonaSoftspoken)
						{
							if (flag)
							{
								if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
								{
									result = ((MBRandom.RandomFloat <= 0.5f) ? "normal2" : "demure2");
								}
								else if (flag3)
								{
									result = ((MBRandom.RandomFloat <= 0.5f) ? "normal" : "closed");
								}
								else
								{
									result = ((MBRandom.RandomFloat <= 0.5f) ? "normal" : "demure");
								}
							}
							else
							{
								result = "normal";
							}
						}
						else if (persona == DefaultTraits.PersonaCurt)
						{
							if (flag)
							{
								if (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0)
								{
									result = "demure2";
								}
								else if (flag3)
								{
									result = ((MBRandom.RandomFloat <= 0.6f) ? "normal" : "closed2");
								}
								else
								{
									result = ((MBRandom.RandomFloat <= 0.4f) ? "warrior2" : "closed");
								}
							}
							else
							{
								result = "normal";
							}
						}
						else if (persona == DefaultTraits.PersonaEarnest)
						{
							if (flag)
							{
								if (flag3)
								{
									result = ((MBRandom.RandomFloat <= 0.6f) ? "normal" : "confident");
								}
								else
								{
									result = ((MBRandom.RandomFloat <= 0.2f) ? "normal" : "confident");
								}
							}
							else
							{
								result = "normal";
							}
						}
					}
				}
			}
			if (character.Occupation == Occupation.Bandit || character.Occupation == Occupation.Gangster)
			{
				result = ((MBRandom.RandomFloat <= 0.7f) ? "aggressive" : "hip");
			}
			if (character.Occupation == Occupation.Guard || character.Occupation == Occupation.PrisonGuard || character.Occupation == Occupation.Soldier)
			{
				result = "normal";
			}
			return result;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000083E0 File Offset: 0x000065E0
		public static string GetDefaultFaceIdle(CharacterObject character)
		{
			string result = "convo_normal";
			string result2 = "convo_bemused";
			string result3 = "convo_mocking_aristocratic";
			string result4 = "convo_mocking_teasing";
			string result5 = "convo_mocking_revenge";
			string result6 = "convo_contemptuous";
			string result7 = "convo_delighted";
			string result8 = "convo_approving";
			string result9 = "convo_relaxed_happy";
			string result10 = "convo_nonchalant";
			string result11 = "convo_thinking";
			string result12 = "convo_undecided_closed";
			string result13 = "convo_bored";
			string result14 = "convo_bored2";
			string result15 = "convo_grave";
			string result16 = "convo_stern";
			string result17 = "convo_very_stern";
			string result18 = "convo_beaten";
			string result19 = "convo_predatory";
			string result20 = "convo_confused_annoyed";
			bool flag = false;
			bool flag2 = false;
			if (character.IsHero)
			{
				flag = (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) > 0);
				flag2 = (character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) + character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0);
			}
			bool flag3 = Hero.MainHero.Clan.Renown < 0f;
			bool flag4 = false;
			if (PlayerEncounter.Current != null && PlayerEncounter.Current.PlayerSide == BattleSideEnum.Defender && (PlayerEncounter.EncounteredMobileParty == null || PlayerEncounter.EncounteredMobileParty.Ai.DoNotAttackMainPartyUntil.IsPast) && PlayerEncounter.EncounteredParty.Owner != null && FactionManager.IsAtWarAgainstFaction(PlayerEncounter.EncounteredParty.MapFaction, Hero.MainHero.MapFaction))
			{
				flag4 = true;
			}
			if (Campaign.Current.CurrentConversationContext == ConversationContext.CapturedLord && character.IsHero && character.HeroObject.MapFaction == PlayerEncounter.EncounteredParty.MapFaction)
			{
				return result16;
			}
			if (character.HeroObject != null)
			{
				int relation = character.HeroObject.GetRelation(Hero.MainHero);
				if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaIronic)
				{
					if (relation > 4)
					{
						return result4;
					}
					if (relation < -10)
					{
						return result5;
					}
					if (character.Occupation == Occupation.GangLeader && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
					{
						return result10;
					}
					if (character.Occupation == Occupation.GangLeader && flag3)
					{
						return result10;
					}
					Clan clan = character.HeroObject.Clan;
					if (clan == null || !clan.IsNoble)
					{
						return result3;
					}
					if (character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) + character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0)
					{
						return result13;
					}
					return result2;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaCurt)
				{
					if (relation > 4)
					{
						return result7;
					}
					if (relation < -20)
					{
						return result4;
					}
					if (character.Occupation == Occupation.GangLeader && flag3)
					{
						return result19;
					}
					if (flag2)
					{
						return result15;
					}
					return result14;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaSoftspoken)
				{
					if (relation > 4)
					{
						return result7;
					}
					if (relation < -20)
					{
						return result20;
					}
					Clan clan2 = character.HeroObject.Clan;
					if ((clan2 == null || !clan2.IsNoble) && flag3 && !character.IsFemale && flag2)
					{
						return result6;
					}
					Clan clan3 = character.HeroObject.Clan;
					if (clan3 != null && clan3.IsNoble && flag3 && !character.IsFemale && flag2)
					{
						return result12;
					}
					if (flag)
					{
						return result8;
					}
					return result11;
				}
				else if (character.HeroObject != null && character.GetPersona() == DefaultTraits.PersonaEarnest)
				{
					if (relation > 4)
					{
						return result7;
					}
					if (relation < -40)
					{
						return result17;
					}
					if (relation < -20)
					{
						return result16;
					}
					Clan clan4 = character.HeroObject.Clan;
					if (clan4 != null && clan4.IsNoble && flag2)
					{
						return result10;
					}
					if (flag)
					{
						return result8;
					}
					return result;
				}
			}
			else if (character.Occupation == Occupation.Villager || character.Occupation == Occupation.Townsfolk)
			{
				int deterministicHashCode = character.StringId.GetDeterministicHashCode();
				if (Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.Town != null && Settlement.CurrentSettlement.Town.Prosperity < (float)(200 * (Settlement.CurrentSettlement.IsTown ? 5 : 1)) && deterministicHashCode % 2 == 0)
				{
					return result18;
				}
				if (deterministicHashCode % 2 == 1)
				{
					return result9;
				}
			}
			else if (flag4 && character.Occupation == Occupation.Bandit)
			{
				return result16;
			}
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000087F0 File Offset: 0x000069F0
		private static int GetSuperiorityState(CharacterObject character)
		{
			if (Hero.MainHero.MapFaction != null && Hero.MainHero.MapFaction.Leader == Hero.MainHero && character.HeroObject.MapFaction == Hero.MainHero.MapFaction)
			{
				return -1;
			}
			if (character.IsHero && character.HeroObject.MapFaction != null && character.HeroObject.MapFaction.IsKingdomFaction)
			{
				Clan clan = character.HeroObject.Clan;
				if (clan != null && clan.IsNoble)
				{
					return 1;
				}
			}
			if (character.Occupation == Occupation.Villager || character.Occupation == Occupation.Townsfolk || character.Occupation == Occupation.Bandit || character.Occupation == Occupation.Gangster || character.Occupation == Occupation.Wanderer)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000088B0 File Offset: 0x00006AB0
		private static bool MorePowerThanPlayer(CharacterObject otherCharacter)
		{
			float num;
			if (otherCharacter.HeroObject.PartyBelongedTo != null)
			{
				num = otherCharacter.HeroObject.PartyBelongedTo.Party.TotalStrength;
			}
			else
			{
				num = otherCharacter.HeroObject.Power;
			}
			float totalStrength = MobileParty.MainParty.Party.TotalStrength;
			return num > totalStrength;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000890C File Offset: 0x00006B0C
		public static CharacterObject FindUpgradeRootOf(CharacterObject character)
		{
			foreach (CharacterObject characterObject in CharacterObject.All)
			{
				if (characterObject.IsBasicTroop && CharacterHelper.UpgradeTreeContains(characterObject, characterObject, character))
				{
					return characterObject;
				}
			}
			return character;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00008970 File Offset: 0x00006B70
		private static bool UpgradeTreeContains(CharacterObject rootTroop, CharacterObject baseTroop, CharacterObject character)
		{
			if (baseTroop == character)
			{
				return true;
			}
			for (int i = 0; i < baseTroop.UpgradeTargets.Length; i++)
			{
				if (baseTroop.UpgradeTargets[i] == rootTroop)
				{
					return false;
				}
				if (CharacterHelper.UpgradeTreeContains(rootTroop, baseTroop.UpgradeTargets[i], character))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000089B8 File Offset: 0x00006BB8
		public static ItemObject GetDefaultWeapon(CharacterObject affectorCharacter)
		{
			for (int i = 0; i <= 4; i++)
			{
				EquipmentElement equipmentFromSlot = affectorCharacter.Equipment.GetEquipmentFromSlot((EquipmentIndex)i);
				ItemObject item = equipmentFromSlot.Item;
				if (((item != null) ? item.PrimaryWeapon : null) != null && equipmentFromSlot.Item.PrimaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.WeaponMask))
				{
					return equipmentFromSlot.Item;
				}
			}
			return null;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00008A18 File Offset: 0x00006C18
		public static bool CanUseItemBasedOnSkill(BasicCharacterObject currentCharacter, EquipmentElement itemRosterElement)
		{
			ItemObject item = itemRosterElement.Item;
			SkillObject relevantSkill = item.RelevantSkill;
			return (relevantSkill == null || currentCharacter.GetSkillValue(relevantSkill) >= item.Difficulty) && (!currentCharacter.IsFemale || !item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByFemale)) && (currentCharacter.IsFemale || !item.ItemFlags.HasAnyFlag(ItemFlags.NotUsableByMale));
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00008A84 File Offset: 0x00006C84
		public static int GetPartyMemberFaceSeed(PartyBase party, BasicCharacterObject character, int rank)
		{
			int num = party.Index * 171 + character.StringId.GetDeterministicHashCode() * 6791 + rank * 197;
			return ((num >= 0) ? num : (-num)) % 2000;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00008AC7 File Offset: 0x00006CC7
		public static int GetDefaultFaceSeed(BasicCharacterObject character, int rank)
		{
			return character.GetDefaultFaceSeed(rank);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public static bool SearchForFormationInTroopTree(CharacterObject baseTroop, FormationClass formation)
		{
			if (baseTroop.UpgradeTargets.Length == 0 && baseTroop.DefaultFormationClass == formation)
			{
				return true;
			}
			foreach (CharacterObject characterObject in baseTroop.UpgradeTargets)
			{
				if (characterObject.Level > baseTroop.Level && CharacterHelper.SearchForFormationInTroopTree(characterObject, formation))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00008B24 File Offset: 0x00006D24
		public static IEnumerable<CharacterObject> GetTroopTree(CharacterObject baseTroop, float minTier = -1f, float maxTier = 3.4028235E+38f)
		{
			MBQueue<CharacterObject> queue = new MBQueue<CharacterObject>();
			queue.Enqueue(baseTroop);
			while (queue.Count > 0)
			{
				CharacterObject character = queue.Dequeue();
				if ((float)character.Tier >= minTier && (float)character.Tier <= maxTier)
				{
					yield return character;
				}
				foreach (CharacterObject item in character.UpgradeTargets)
				{
					queue.Enqueue(item);
				}
				character = null;
			}
			yield break;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00008B44 File Offset: 0x00006D44
		public static void DeleteQuestCharacter(CharacterObject character, Settlement questSettlement)
		{
			if (questSettlement != null)
			{
				IList<LocationCharacter> listOfCharacters = questSettlement.LocationComplex.GetListOfCharacters();
				if (listOfCharacters.Any((LocationCharacter x) => x.Character == character))
				{
					LocationCharacter locationCharacter = listOfCharacters.First((LocationCharacter x) => x.Character == character);
					questSettlement.LocationComplex.RemoveCharacterIfExists(locationCharacter);
				}
			}
			Game.Current.ObjectManager.UnregisterObject(character);
		}
	}
}
