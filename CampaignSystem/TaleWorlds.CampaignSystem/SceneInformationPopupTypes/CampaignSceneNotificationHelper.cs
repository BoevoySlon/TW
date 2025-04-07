using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.SceneInformationPopupTypes
{
	// Token: 0x020000A6 RID: 166
	public static class CampaignSceneNotificationHelper
	{
		// Token: 0x060011C5 RID: 4549 RVA: 0x00052388 File Offset: 0x00050588
		public static SceneNotificationData.SceneNotificationCharacter GetBodyguardOfCulture(CultureObject culture)
		{
			string stringId = culture.StringId;
			string objectName;
			if (!(stringId == "battania"))
			{
				if (!(stringId == "khuzait"))
				{
					if (!(stringId == "vlandia"))
					{
						if (!(stringId == "aserai"))
						{
							if (!(stringId == "sturgia"))
							{
								if (!(stringId == "empire"))
								{
									objectName = "fighter_sturgia";
								}
								else
								{
									objectName = "imperial_legionary";
								}
							}
							else
							{
								objectName = "sturgian_veteran_warrior";
							}
						}
						else
						{
							objectName = "mamluke_palace_guard";
						}
					}
					else
					{
						objectName = "vlandian_banner_knight";
					}
				}
				else
				{
					objectName = "khuzait_khans_guard";
				}
			}
			else
			{
				objectName = "battanian_fian_champion";
			}
			return new SceneNotificationData.SceneNotificationCharacter(MBObjectManager.Instance.GetObject<CharacterObject>(objectName), null, default(BodyProperties), false, uint.MaxValue, uint.MaxValue, false);
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00052440 File Offset: 0x00050640
		public static void RemoveWeaponsFromEquipment(ref Equipment equipment, bool removeHelmet = false, bool removeShoulder = false)
		{
			for (int i = 0; i < 5; i++)
			{
				equipment[i] = EquipmentElement.Invalid;
			}
			if (removeHelmet)
			{
				equipment[5] = EquipmentElement.Invalid;
			}
			if (removeShoulder)
			{
				equipment[9] = EquipmentElement.Invalid;
			}
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00052488 File Offset: 0x00050688
		public static string GetChildStageEquipmentIDFromCulture(CultureObject childCulture)
		{
			string stringId = childCulture.StringId;
			if (stringId == "empire")
			{
				return "comingofage_kid_emp_cutscene_template";
			}
			if (stringId == "aserai")
			{
				return "comingofage_kid_ase_cutscene_template";
			}
			if (stringId == "battania")
			{
				return "comingofage_kid_bat_cutscene_template";
			}
			if (stringId == "khuzait")
			{
				return "comingofage_kid_khu_cutscene_template";
			}
			if (stringId == "sturgia")
			{
				return "comingofage_kid_stu_cutscene_template";
			}
			if (!(stringId == "vlandia"))
			{
				return "comingofage_kid_emp_cutscene_template";
			}
			return "comingofage_kid_vla_cutscene_template";
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00052518 File Offset: 0x00050718
		public static CharacterObject GetRandomTroopForCulture(CultureObject culture)
		{
			string objectName = "imperial_recruit";
			if (culture != null)
			{
				List<CharacterObject> list = new List<CharacterObject>();
				if (culture.BasicTroop != null)
				{
					list.Add(culture.BasicTroop);
				}
				if (culture.EliteBasicTroop != null)
				{
					list.Add(culture.EliteBasicTroop);
				}
				if (culture.MeleeMilitiaTroop != null)
				{
					list.Add(culture.MeleeMilitiaTroop);
				}
				if (culture.MeleeEliteMilitiaTroop != null)
				{
					list.Add(culture.MeleeEliteMilitiaTroop);
				}
				if (culture.RangedMilitiaTroop != null)
				{
					list.Add(culture.RangedMilitiaTroop);
				}
				if (culture.RangedEliteMilitiaTroop != null)
				{
					list.Add(culture.RangedEliteMilitiaTroop);
				}
				if (list.Count > 0)
				{
					return list[MBRandom.RandomInt(list.Count)];
				}
			}
			return Game.Current.ObjectManager.GetObject<CharacterObject>(objectName);
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x000525DA File Offset: 0x000507DA
		public static IEnumerable<Hero> GetMilitaryAudienceForHero(Hero hero, bool includeClanLeader = true, bool onlyClanMembers = false)
		{
			if (hero.Clan != null)
			{
				if (includeClanLeader)
				{
					Hero leader = hero.Clan.Leader;
					if (leader != null && leader.IsAlive && hero != hero.Clan.Leader)
					{
						yield return hero.Clan.Leader;
					}
				}
				IOrderedEnumerable<Hero> orderedEnumerable = from h in hero.Clan.Heroes
				orderby h.Level
				select h;
				foreach (Hero hero2 in orderedEnumerable)
				{
					if (hero2 != hero.Clan.Leader && hero2.IsAlive && !hero2.IsChild && hero2 != hero)
					{
						yield return hero2;
					}
				}
				IEnumerator<Hero> enumerator = null;
			}
			if (!onlyClanMembers)
			{
				IOrderedEnumerable<Hero> orderedEnumerable2 = from h in Hero.AllAliveHeroes
				orderby CharacterRelationManager.GetHeroRelation(hero, h)
				select h;
				foreach (Hero hero3 in orderedEnumerable2)
				{
					bool flag = hero3 != null && hero3.Clan != hero.Clan;
					if (hero3.IsFriend(hero3) && hero3.IsLord && !hero3.IsChild && hero3 != hero && !flag)
					{
						yield return hero3;
					}
				}
				IEnumerator<Hero> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000525F8 File Offset: 0x000507F8
		public static IEnumerable<Hero> GetMilitaryAudienceForKingdom(Kingdom kingdom, bool includeKingdomLeader = true)
		{
			if (includeKingdomLeader)
			{
				Hero leader = kingdom.Leader;
				if (leader != null && leader.IsAlive)
				{
					yield return kingdom.Leader;
				}
			}
			Hero leader2 = kingdom.Leader;
			IOrderedEnumerable<Hero> orderedEnumerable;
			if (leader2 == null)
			{
				orderedEnumerable = null;
			}
			else
			{
				orderedEnumerable = from h in leader2.Clan.Heroes.WhereQ((Hero h) => h != h.Clan.Kingdom.Leader)
				orderby h.GetRelationWithPlayer()
				select h;
			}
			IOrderedEnumerable<Hero> orderedEnumerable2 = orderedEnumerable;
			foreach (Hero hero in orderedEnumerable2)
			{
				if (!hero.IsChild && hero != Hero.MainHero && hero.IsAlive)
				{
					yield return hero;
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x00052610 File Offset: 0x00050810
		public static TextObject GetFormalDayAndSeasonText(CampaignTime time)
		{
			TextObject textObject = new TextObject("{=CpsPq6WD}the {DAY_ORDINAL} day of {SEASON_NAME}", null);
			TextObject variable = GameTexts.FindText("str_season_" + time.GetSeasonOfYear, null);
			TextObject variable2 = GameTexts.FindText("str_ordinal_number", (time.GetDayOfSeason + 1).ToString());
			textObject.SetTextVariable("SEASON_NAME", variable);
			textObject.SetTextVariable("DAY_ORDINAL", variable2);
			return textObject;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0005267C File Offset: 0x0005087C
		public static TextObject GetFormalNameForKingdom(Kingdom kingdom)
		{
			TextObject result;
			if (kingdom.Culture.StringId.Equals("empire", StringComparison.InvariantCultureIgnoreCase))
			{
				result = kingdom.Name;
			}
			else if (kingdom.Leader == Hero.MainHero)
			{
				result = kingdom.InformalName;
			}
			else
			{
				result = FactionHelper.GetFormalNameForFactionCulture(kingdom.Culture);
			}
			return result;
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x000526D0 File Offset: 0x000508D0
		public static SceneNotificationData.SceneNotificationCharacter CreateNotificationCharacterFromHero(Hero hero, Equipment overridenEquipment = null, bool useCivilian = false, BodyProperties overriddenBodyProperties = default(BodyProperties), uint overriddenColor1 = 4294967295U, uint overriddenColor2 = 4294967295U, bool useHorse = false)
		{
			if (overriddenColor1 == 4294967295U)
			{
				IFaction mapFaction = hero.MapFaction;
				overriddenColor1 = ((mapFaction != null) ? mapFaction.Color : hero.CharacterObject.Culture.Color);
			}
			if (overriddenColor2 == 4294967295U)
			{
				IFaction mapFaction2 = hero.MapFaction;
				overriddenColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : hero.CharacterObject.Culture.Color2);
			}
			if (overridenEquipment == null)
			{
				overridenEquipment = (useCivilian ? hero.CivilianEquipment : hero.BattleEquipment);
			}
			return new SceneNotificationData.SceneNotificationCharacter(hero.CharacterObject, overridenEquipment, overriddenBodyProperties, useCivilian, overriddenColor1, overriddenColor2, useHorse);
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00052757 File Offset: 0x00050957
		public static ItemObject GetDefaultHorseItem()
		{
			return Game.Current.ObjectManager.GetObjectTypeList<ItemObject>().First((ItemObject i) => i.ItemType == ItemObject.ItemTypeEnum.Horse && i.HasHorseComponent && i.HorseComponent.IsMount && i.HorseComponent.Monster.StringId == "horse");
		}
	}
}
