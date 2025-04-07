using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002E RID: 46
	public static class CampaignCheats
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x00012685 File Offset: 0x00010885
		public static bool CheckCheatUsage(ref string ErrorType)
		{
			if (Campaign.Current == null)
			{
				ErrorType = "Campaign was not started.";
				return false;
			}
			if (!Game.Current.CheatMode)
			{
				ErrorType = "Cheat mode is disabled!";
				return false;
			}
			ErrorType = "";
			return true;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x000126B4 File Offset: 0x000108B4
		public static bool CheckParameters(List<string> strings, int ParameterCount)
		{
			if (strings.Count == 0)
			{
				return ParameterCount == 0;
			}
			return strings.Count == ParameterCount;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000126CC File Offset: 0x000108CC
		public static bool CheckHelp(List<string> strings)
		{
			return strings.Count != 0 && strings[0].ToLower() == "help";
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000126F0 File Offset: 0x000108F0
		[CommandLineFunctionality.CommandLineArgumentFunction("set_hero_crafting_stamina", "campaign")]
		public static string SetCraftingStamina(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_hero_crafting_stamina [HeroName] | [Stamina]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			ICraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return "Can not found ICrafting Campaign Behavior!\n" + text;
			}
			int num = 0;
			if (!int.TryParse(separatedNames[1], out num) || num < 0 || num > 100)
			{
				return string.Concat(new object[]
				{
					"Please enter a valid number between 0-100 number is: ",
					num,
					"\n",
					text
				});
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			if (hero != null)
			{
				int value = (int)((float)(campaignBehavior.GetMaxHeroCraftingStamina(hero) * num) / 100f);
				campaignBehavior.SetHeroCraftingStamina(hero, value);
				return "Success";
			}
			return "Hero is not found\n" + text;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x000127E8 File Offset: 0x000109E8
		[CommandLineFunctionality.CommandLineArgumentFunction("set_hero_culture", "campaign")]
		public static string SetHeroCulture(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_hero_culture [HeroName] | [CultureName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string objectName = separatedNames[1].ToLower().Replace(" ", "");
			CultureObject @object = Campaign.Current.ObjectManager.GetObject<CultureObject>(objectName);
			if (@object == null)
			{
				return "Culture couldn't be found!\n" + text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (hero.Culture == @object)
			{
				return string.Format("Hero culture is already {0}", @object.Name);
			}
			hero.Culture = @object;
			return "Success";
		}

		// Token: 0x060002DA RID: 730 RVA: 0x000128C4 File Offset: 0x00010AC4
		[CommandLineFunctionality.CommandLineArgumentFunction("set_clan_culture", "campaign")]
		public static string SetClanCulture(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_clan_culture [ClanName] | [CultureName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> parameters = CampaignCheats.GetSeparatedNames(strings, "|");
			if (parameters.Count != 2)
			{
				return text;
			}
			string objectName = parameters[1].ToLower().Replace(" ", "");
			CultureObject @object = Campaign.Current.ObjectManager.GetObject<CultureObject>(objectName);
			if (@object == null)
			{
				return "Culture couldn't be found!\n" + text;
			}
			Clan clan = Campaign.Current.Clans.FirstOrDefault((Clan x) => string.Equals(x.Name.ToString().Replace(" ", ""), parameters[0].Replace(" ", ""), StringComparison.OrdinalIgnoreCase) && !x.IsEliminated);
			if (clan == null)
			{
				return "Clan couldn't be found\n" + text;
			}
			if (clan.Culture == @object)
			{
				return string.Format("Clan culture is already {0}", @object.Name);
			}
			clan.Culture = @object;
			return "Success";
		}

		// Token: 0x060002DB RID: 731 RVA: 0x000129C4 File Offset: 0x00010BC4
		[CommandLineFunctionality.CommandLineArgumentFunction("make_hero_wounded", "campaign")]
		public static string MakeHeroWounded(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.make_hero_wounded [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Hero hero = CampaignCheats.GetHero(CampaignCheats.ConcatenateString(strings));
			if (hero != null)
			{
				hero.MakeWounded(null, KillCharacterAction.KillCharacterActionDetail.None);
				return "Success";
			}
			return "Hero is not found\n" + text;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00012A24 File Offset: 0x00010C24
		[CommandLineFunctionality.CommandLineArgumentFunction("reset_player_skills_level_and_perks", "campaign")]
		public static string ResetPlayerSkillsLevelAndPerk(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0) && !CampaignCheats.CheckHelp(strings))
			{
				Hero.MainHero.CharacterObject.Level = 0;
				Hero.MainHero.HeroDeveloper.ClearHero();
				return "Success";
			}
			return "Format is \"campaign.reset_player_skills_level_and_perks\".";
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00012A80 File Offset: 0x00010C80
		[CommandLineFunctionality.CommandLineArgumentFunction("set_skills_of_hero", "campaign")]
		public static string SetSkillsOfGivenHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_skills_of_hero [HeroName] | [Level]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num = -1;
			Hero hero = null;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Level must be a number\n" + text;
			}
			hero = CampaignCheats.GetHero(separatedNames[0]);
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (num > 0 && num <= 300)
			{
				hero.CharacterObject.Level = 0;
				hero.HeroDeveloper.ClearHero();
				int num2 = MathF.Min(num / 25 + 1, 10);
				int maxFocusPerSkill = Campaign.Current.Models.CharacterDevelopmentModel.MaxFocusPerSkill;
				foreach (SkillObject skill in Skills.All)
				{
					if (hero.HeroDeveloper.GetFocus(skill) + num2 > maxFocusPerSkill)
					{
						num2 = maxFocusPerSkill;
					}
					hero.HeroDeveloper.AddFocus(skill, num2, false);
					hero.HeroDeveloper.SetInitialSkillLevel(skill, num);
				}
				hero.HeroDeveloper.UnspentFocusPoints = 0;
				return string.Format("{0}'s skills are set to level {1}.", hero.Name, num);
			}
			return string.Format("Level must be between 0 - {0}.", 300);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00012C14 File Offset: 0x00010E14
		[CommandLineFunctionality.CommandLineArgumentFunction("set_settlements_visible", "campaign")]
		public static string SetAllSettlementsVisible(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			int num;
			if (CampaignCheats.CheckParameters(strings, 1) && !CampaignCheats.CheckHelp(strings) && int.TryParse(strings[0], out num) && (num == 2 || num == 1 || num == 0))
			{
				foreach (Settlement settlement in Settlement.All)
				{
					bool flag = num != 0 && (!settlement.IsHideout || num == 1);
					settlement.IsVisible = flag;
					settlement.IsInspected = flag;
				}
				return "Success";
			}
			return "Format is \"campaign.set_settlements_visible [2(no hideouts)/1(all)/0(none)]\".";
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00012CCC File Offset: 0x00010ECC
		[CommandLineFunctionality.CommandLineArgumentFunction("set_skill_main_hero", "campaign")]
		public static string SetSkillMainHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_skill_main_hero [SkillName] | [LevelValue]\".";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2 || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			foreach (SkillObject skillObject in Skills.All)
			{
				if (string.Equals(skillObject.Name.ToString().Replace(" ", ""), separatedNames[0].Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					int num = 1;
					if (!int.TryParse(separatedNames[1], out num))
					{
						return "Please enter a number\n" + text;
					}
					if (num <= 0 || num > 300)
					{
						return string.Format("Level must be between 0 - {0}.", 300);
					}
					Hero.MainHero.HeroDeveloper.SetInitialSkillLevel(skillObject, num);
					Hero.MainHero.HeroDeveloper.InitializeSkillXp(skillObject);
					return "Success";
				}
			}
			return "Skill is not found.\n" + text;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00012E10 File Offset: 0x00011010
		[CommandLineFunctionality.CommandLineArgumentFunction("set_all_skills_main_hero", "campaign")]
		public static string SetAllSkillsMainHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			int num = 1;
			string text = "Format is \"campaign.set_all_skills_main_hero [LevelValue]\".";
			if (strings.IsEmpty<string>() || !int.TryParse(strings[0], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (num <= 0 || num > 300)
			{
				return string.Format("Level must be between 0 - {0}.", 300);
			}
			if (CampaignCheats.CheckParameters(strings, 1) && !CampaignCheats.CheckHelp(strings))
			{
				foreach (SkillObject skill in Skills.All)
				{
					Hero.MainHero.HeroDeveloper.SetInitialSkillLevel(skill, num);
					Hero.MainHero.HeroDeveloper.InitializeSkillXp(skill);
				}
				return "Success";
			}
			return text;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00012EF4 File Offset: 0x000110F4
		[CommandLineFunctionality.CommandLineArgumentFunction("set_skill_of_all_companions", "campaign")]
		public static string SetSkillCompanion(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_skill_of_all_companions [SkillName] | [LevelValue]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			foreach (SkillObject skillObject in Skills.All)
			{
				if (string.Equals(skillObject.Name.ToString().Replace(" ", ""), separatedNames[0].Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					int num = 1;
					if (!int.TryParse(separatedNames[1], out num))
					{
						return "Please enter a number\n" + text;
					}
					if (num <= 0 || num > 300)
					{
						return string.Format("Level must be between 0 - {0}.", 300);
					}
					foreach (Hero hero in Clan.PlayerClan.Companions)
					{
						hero.HeroDeveloper.SetInitialSkillLevel(skillObject, num);
						hero.HeroDeveloper.InitializeSkillXp(skillObject);
					}
					return "Success";
				}
			}
			return "Skill is not found.\n" + text;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00013088 File Offset: 0x00011288
		[CommandLineFunctionality.CommandLineArgumentFunction("set_all_companion_skills", "campaign")]
		public static string SetAllSkillsOfAllCompanions(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_all_companion_skills [LevelValue]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			foreach (SkillObject skill in Skills.All)
			{
				int num = 1;
				if (strings.Count == 0 || !int.TryParse(strings[0], out num))
				{
					return "Please enter a number\n" + text;
				}
				if (num <= 0 || num > 300)
				{
					return string.Format("Level must be between 0 - {0}.", 300);
				}
				foreach (Hero hero in Clan.PlayerClan.Companions)
				{
					hero.HeroDeveloper.SetInitialSkillLevel(skill, num);
					hero.HeroDeveloper.InitializeSkillXp(skill);
				}
			}
			return "Success";
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000131B4 File Offset: 0x000113B4
		[CommandLineFunctionality.CommandLineArgumentFunction("set_all_heroes_skills", "campaign")]
		public static string SetAllHeroSkills(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_all_heroes_skills [LevelValue]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num;
			if (strings.Count == 0 || !int.TryParse(strings[0], out num))
			{
				return "Please enter a positive number\n" + text;
			}
			foreach (Hero hero in (from x in Hero.AllAliveHeroes
			where x.IsActive && x.PartyBelongedTo != null
			select x).ToList<Hero>())
			{
				foreach (SkillObject skill in Skills.All)
				{
					if (num <= 0 || num > 300)
					{
						return string.Format("Level must be between 0 - {0}.", 300);
					}
					hero.HeroDeveloper.SetInitialSkillLevel(skill, num);
					hero.HeroDeveloper.InitializeSkillXp(skill);
				}
			}
			return "Success";
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000132FC File Offset: 0x000114FC
		[CommandLineFunctionality.CommandLineArgumentFunction("set_loyalty_of_settlement", "campaign")]
		public static string SetLoyaltyOfSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_loyalty_of_settlement [SettlementName] | [loyalty]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num = 0;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a positive number\n" + text;
			}
			if (num > 100 || num < 0)
			{
				return "Loyalty has to be in the range of 0 to 100";
			}
			string text2 = separatedNames[0];
			Settlement settlement = CampaignCheats.GetSettlement(text2);
			if (settlement == null)
			{
				return "Settlement is not found: " + text2 + "\n" + text;
			}
			if (settlement.IsVillage)
			{
				return "Settlement must be castle or town";
			}
			settlement.Town.Loyalty = (float)num;
			return "Success";
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000133CC File Offset: 0x000115CC
		[CommandLineFunctionality.CommandLineArgumentFunction("print_main_party_position", "campaign")]
		public static string PrintMainPartyPosition(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.print_main_party_position\".";
			}
			return MobileParty.MainParty.Position2D.x + " " + MobileParty.MainParty.Position2D.y;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00013434 File Offset: 0x00011634
		[CommandLineFunctionality.CommandLineArgumentFunction("start_world_war", "campaign")]
		public static string StartWorldWar(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.start_world_war\".";
			}
			foreach (Kingdom kingdom in Kingdom.All)
			{
				foreach (Kingdom kingdom2 in Kingdom.All)
				{
					if (kingdom != kingdom2 && !FactionManager.IsAtWarAgainstFaction(kingdom, kingdom2))
					{
						DeclareWarAction.ApplyByDefault(kingdom, kingdom2);
					}
				}
			}
			return "All kingdoms are at war with each other!";
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000134FC File Offset: 0x000116FC
		[CommandLineFunctionality.CommandLineArgumentFunction("start_world_peace", "campaign")]
		public static string StartWorldPeace(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.start_world_peace\".";
			}
			foreach (Kingdom kingdom in Kingdom.All)
			{
				foreach (Kingdom kingdom2 in Kingdom.All)
				{
					if (kingdom != kingdom2 && FactionManager.IsAtWarAgainstFaction(kingdom, kingdom2))
					{
						MakePeaceAction.Apply(kingdom, kingdom2, 0);
					}
				}
			}
			return "All kingdoms are at peace with each other!";
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000135C4 File Offset: 0x000117C4
		[CommandLineFunctionality.CommandLineArgumentFunction("add_modified_item", "campaign")]
		public static string AddModifiedItem(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_modified_item [ItemName] | [ModifierName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string itemObjectName = separatedNames[0];
			string itemModifierName = separatedNames[1];
			ItemObject itemObject = CampaignCheats.GetItemObject(itemObjectName);
			if (itemObject == null)
			{
				return "Cant find the item.\n" + text;
			}
			ItemModifier itemModifier = CampaignCheats.GetItemModifier(itemModifierName);
			if (itemModifier != null)
			{
				EquipmentElement rosterElement = new EquipmentElement(itemObject, itemModifier, null, false);
				MobileParty.MainParty.ItemRoster.AddToCounts(rosterElement, 5);
				return "Success";
			}
			return "Cant find the modifier.\n" + text;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00013684 File Offset: 0x00011884
		[CommandLineFunctionality.CommandLineArgumentFunction("set_player_name", "campaign")]
		public static string SetPlayerName(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.set_player_name [HeroName]\".";
			}
			string value = CampaignCheats.ConcatenateString(strings);
			TextObject textObject = GameTexts.FindText("str_generic_character_firstname", null);
			textObject.SetTextVariable("CHARACTER_FIRSTNAME", new TextObject(value, null));
			TextObject textObject2 = GameTexts.FindText("str_generic_character_name", null);
			textObject2.SetTextVariable("CHARACTER_NAME", new TextObject(value, null));
			Hero.MainHero.SetName(textObject2, textObject);
			return "Success";
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00013710 File Offset: 0x00011910
		[CommandLineFunctionality.CommandLineArgumentFunction("add_crafting_materials", "campaign")]
		public static string AddCraftingMaterials(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.add_crafting_materials\".";
			}
			for (int i = 0; i < 9; i++)
			{
				PartyBase.MainParty.ItemRoster.AddToCounts(Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem((CraftingMaterials)i), 100);
			}
			return "100 pieces for each crafting material is added to the player inventory.";
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00013780 File Offset: 0x00011980
		[CommandLineFunctionality.CommandLineArgumentFunction("add_hero_relation", "campaign")]
		public static string AddHeroRelation(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_hero_relation [HeroName]/All | [Value] \".\n";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int relation;
			if (!int.TryParse(separatedNames[1], out relation))
			{
				return "Please enter a number\n" + text;
			}
			string text2 = separatedNames[0];
			Hero hero = CampaignCheats.GetHero(text2);
			if (hero == Hero.MainHero)
			{
				return "Can not add relation with yourself.";
			}
			if (hero != null)
			{
				ChangeRelationAction.ApplyPlayerRelation(hero, relation, true, true);
				return "Success";
			}
			if (string.Equals(text2, "all", StringComparison.OrdinalIgnoreCase))
			{
				foreach (Hero hero2 in Hero.AllAliveHeroes)
				{
					if (!hero2.IsHumanPlayerCharacter)
					{
						ChangeRelationAction.ApplyPlayerRelation(hero2, relation, true, true);
					}
				}
				return "Success";
			}
			return "Hero is not found\n" + text;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001389C File Offset: 0x00011A9C
		[CommandLineFunctionality.CommandLineArgumentFunction("heal_main_party", "campaign")]
		public static string HealMainParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.heal_main_party\".";
			}
			if (MobileParty.MainParty.MapEvent == null)
			{
				for (int i = 0; i < PartyBase.MainParty.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = PartyBase.MainParty.MemberRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Character.IsHero)
					{
						elementCopyAtIndex.Character.HeroObject.Heal(elementCopyAtIndex.Character.HeroObject.MaxHitPoints, false);
					}
					else
					{
						MobileParty.MainParty.Party.AddToMemberRosterElementAtIndex(i, 0, -PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(i));
					}
				}
				return "Success";
			}
			return "Main party shouldn't be in a map event.";
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001396C File Offset: 0x00011B6C
		[CommandLineFunctionality.CommandLineArgumentFunction("declare_war", "campaign")]
		public static string DeclareWar(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.declare_war [Faction1] | [Faction2]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0].ToLower().Replace(" ", "");
			string text3 = separatedNames[1].ToLower().Replace(" ", "");
			IFaction faction = null;
			IFaction faction2 = null;
			foreach (IFaction faction3 in Campaign.Current.Factions)
			{
				string a = faction3.Name.ToString().ToLower().Replace(" ", "");
				if (a == text2)
				{
					faction = faction3;
				}
				else if (a == text3)
				{
					faction2 = faction3;
				}
			}
			if (faction == null)
			{
				return "Faction is not found: " + text2 + "\n" + text;
			}
			if (faction2 == null)
			{
				return "Faction is not found: " + text3 + "\n" + text;
			}
			if (faction == faction2 || faction.MapFaction == faction2.MapFaction)
			{
				return "Can't declare between same factions";
			}
			if (!faction.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			if (!faction2.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			DeclareWarAction.ApplyByDefault(faction, faction2);
			return string.Concat(new object[]
			{
				"War declared between ",
				faction.Name,
				" and ",
				faction2.Name
			});
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00013B3C File Offset: 0x00011D3C
		[CommandLineFunctionality.CommandLineArgumentFunction("declare_peace", "campaign")]
		public static string DeclarePeace(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "campaign.declare_peace [Faction1] | [Faction2]";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string text2 = separatedNames[0].ToLower().Replace(" ", "");
			string text3 = separatedNames[1].ToLower().Replace(" ", "");
			IFaction faction = null;
			IFaction faction2 = null;
			foreach (IFaction faction3 in Campaign.Current.Factions)
			{
				string a = faction3.Name.ToString().ToLower().Replace(" ", "");
				if (a == text2)
				{
					faction = faction3;
				}
				else if (a == text3)
				{
					faction2 = faction3;
				}
			}
			if (faction == null)
			{
				return "Faction is not found: " + text2 + "\n" + text;
			}
			if (faction2 == null)
			{
				return "Faction is not found: " + text3 + "\n" + text;
			}
			if (faction == faction2 || faction.MapFaction == faction2.MapFaction)
			{
				return "Can't declare between same factions";
			}
			if (!faction.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			if (!faction2.IsMapFaction)
			{
				return faction.Name + " is bound to a kingdom.";
			}
			if (faction.GetStanceWith(faction2).IsAtConstantWar)
			{
				return "There is constant war between factions, peace can't be declared";
			}
			MakePeaceAction.Apply(faction, faction2, 0);
			return string.Concat(new object[]
			{
				"Peace declared between ",
				faction.Name,
				" and ",
				faction2.Name
			});
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00013D24 File Offset: 0x00011F24
		[CommandLineFunctionality.CommandLineArgumentFunction("add_item_to_main_party", "campaign")]
		public static string AddItemToMainParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_item_to_main_party [ItemObject] | [Amount]\"\n If amount is not entered only 1 item will be given.";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			ItemObject itemObject = CampaignCheats.GetItemObject(separatedNames[0]);
			if (itemObject == null)
			{
				return "Item is not found\n" + text;
			}
			int num = 1;
			if (strings.Count == 1)
			{
				PartyBase.MainParty.ItemRoster.AddToCounts(itemObject, num);
				return itemObject.Name + " has been given to the main party.";
			}
			if (separatedNames.Count == 2 && (!int.TryParse(separatedNames[1], out num) || num < 1))
			{
				return "Please enter a positive number\n" + text;
			}
			PartyBase.MainParty.ItemRoster.AddToCounts(itemObject, num);
			return itemObject.Name + " has been given to the main party.";
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00013E00 File Offset: 0x00012000
		[CommandLineFunctionality.CommandLineArgumentFunction("add_all_crafting_materials_to_main_party", "campaign")]
		public static string AddCraftingMaterialItemsToMainParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_all_crafting_materials_to_main_party [Amount]\n If amount is not entered only 1 item per material will be given.\".";
			if (strings.Count > 1 || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			if (strings.Count == 1 && (!int.TryParse(strings[0], out num) || num < 1))
			{
				return "Please enter a positive number\n" + text;
			}
			for (CraftingMaterials craftingMaterials = CraftingMaterials.IronOre; craftingMaterials < CraftingMaterials.NumCraftingMats; craftingMaterials++)
			{
				ItemObject craftingMaterialItem = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem(craftingMaterials);
				PartyBase.MainParty.ItemRoster.AddToCounts(craftingMaterialItem, num);
			}
			return "Crafting materials have been given to the main party.";
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00013EA0 File Offset: 0x000120A0
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_capturer_party", "campaign")]
		public static string KillCapturerParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.kill_capturer_party\".";
			}
			if (!PlayerCaptivity.IsCaptive)
			{
				return "Player is not captive.";
			}
			if (PlayerCaptivity.CaptorParty.IsSettlement)
			{
				return "Can't destroy settlement";
			}
			GameMenu.SwitchToMenu("menu_captivity_end_by_party_removed");
			DestroyPartyAction.Apply(null, PlayerCaptivity.CaptorParty.MobileParty);
			return "Success";
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00013F14 File Offset: 0x00012114
		[CommandLineFunctionality.CommandLineArgumentFunction("add_influence", "campaign")]
		public static string AddInfluence(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.add_influence [Number]\". If Number is not entered, 100 influence will be added.";
			}
			int num = 100;
			bool flag = false;
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				flag = int.TryParse(strings[0], out num);
			}
			if (flag || CampaignCheats.CheckParameters(strings, 0))
			{
				float num2 = MBMath.ClampFloat(Hero.MainHero.Clan.Influence + (float)num, 0f, float.MaxValue);
				float num3 = num2 - Hero.MainHero.Clan.Influence;
				ChangeClanInfluenceAction.Apply(Clan.PlayerClan, num2);
				return string.Format("The influence of player is changed by {0}.", num3);
			}
			return "Please enter a positive number\nFormat is \"campaign.add_influence [Number]\".";
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00013FC0 File Offset: 0x000121C0
		[CommandLineFunctionality.CommandLineArgumentFunction("add_renown_to_clan", "campaign")]
		public static string AddRenown(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_renown [ClanName] | [PositiveNumber]\". \n If number is not specified, 100 will be added. \n If clan name is not specified, player clan will get the renown.";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 100;
			string text2 = "";
			Hero hero = Hero.MainHero;
			bool flag = false;
			if (CampaignCheats.CheckParameters(strings, 1))
			{
				if (!int.TryParse(strings[0], out num))
				{
					num = 100;
					text2 = CampaignCheats.ConcatenateString(strings);
					hero = CampaignCheats.GetClanLeader(text2);
					flag = true;
				}
			}
			else if (!CampaignCheats.CheckParameters(strings, 0))
			{
				List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
				if (separatedNames.Count == 2 && !int.TryParse(separatedNames[1], out num))
				{
					return "Please enter a positive number\n" + text;
				}
				text2 = separatedNames[0];
				hero = CampaignCheats.GetClanLeader(text2);
				flag = true;
			}
			if (hero != null)
			{
				if (num > 0)
				{
					GainRenownAction.Apply(hero, (float)num, false);
					return string.Format("Added {0} renown to ", num) + hero.Clan.Name;
				}
				return "Please enter a positive number\n" + text;
			}
			else
			{
				if (flag)
				{
					return "Clan: " + text2 + " not found.\n" + text;
				}
				return "Wrong Input.\n" + text;
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x000140E0 File Offset: 0x000122E0
		[CommandLineFunctionality.CommandLineArgumentFunction("add_gold_to_hero", "campaign")]
		public static string AddGoldToHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_gold_to_hero [HeroName] | [PositiveNumber]\".\n If number is not specified, 1000 will be added. \n If hero name is not specified, player's gold will change.";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1000;
			Hero hero = Hero.MainHero;
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				GiveGoldAction.ApplyBetweenCharacters(null, hero, num, true);
				return "Success";
			}
			if (CampaignCheats.CheckParameters(strings, 1) && !int.TryParse(strings[0], out num))
			{
				num = 1000;
				hero = CampaignCheats.GetHero(CampaignCheats.ConcatenateString(strings));
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count == 2)
			{
				if (!int.TryParse(separatedNames[1], out num))
				{
					return "Please enter a number\n" + text;
				}
				hero = CampaignCheats.GetHero(separatedNames[0]);
			}
			if (separatedNames.Count == 1 && !int.TryParse(separatedNames[0], out num))
			{
				hero = CampaignCheats.GetHero(separatedNames[0]);
			}
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (hero.Gold + num < 0 || hero.Gold + num > 100000000)
			{
				return "Hero's gold must be between 0-100000000.";
			}
			GiveGoldAction.ApplyBetweenCharacters(null, hero, num, true);
			return string.Format("{0}'s denars changed by {1}.", hero.Name, num);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00014210 File Offset: 0x00012410
		[CommandLineFunctionality.CommandLineArgumentFunction("add_gold_to_all_heroes", "campaign")]
		public static string AddGoldToAllHeroes(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_gold_to_all_heroes [PositiveNumber]\".\n If number is not specified, 100 will be added.";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1000;
			bool flag = false;
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				flag = int.TryParse(strings[0], out num);
			}
			if (!flag && !CampaignCheats.CheckParameters(strings, 0))
			{
				return "Wrong input.\nFormat is \"campaign.add_gold_to_all_heroes [Number]\".";
			}
			if (num < 1)
			{
				return "Please enter a positive number\n" + text;
			}
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero != null)
				{
					GiveGoldAction.ApplyBetweenCharacters(null, hero, num, true);
				}
			}
			return string.Format("All party's denars changed by {0}.", num);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x000142E0 File Offset: 0x000124E0
		[CommandLineFunctionality.CommandLineArgumentFunction("activate_all_policies_for_player_kingdom", "campaign")]
		public static string ActivateAllPolicies(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.activate_all_policies_for_player_kingdom";
			}
			if (Clan.PlayerClan.Kingdom != null)
			{
				Kingdom kingdom = Clan.PlayerClan.Kingdom;
				foreach (PolicyObject policyObject in PolicyObject.All)
				{
					if (!kingdom.ActivePolicies.Contains(policyObject))
					{
						kingdom.AddPolicy(policyObject);
					}
				}
				return "All policies are now active for player kingdom.";
			}
			return "Player is not in a kingdom.";
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0001438C File Offset: 0x0001258C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_building_level", "campaign")]
		public static string AddDevelopment(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_building_level [SettlementName] | [Building]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[0]);
			if (settlement != null && settlement.IsFortification)
			{
				BuildingType buildingType = null;
				foreach (BuildingType buildingType2 in BuildingType.All)
				{
					if (buildingType2.Name.ToString().ToLower().Replace(" ", "").Equals(separatedNames[1].ToString().ToLower().Replace(" ", "")))
					{
						if (buildingType2.BuildingLocation == BuildingLocation.Castle && settlement.IsCastle)
						{
							buildingType = buildingType2;
							break;
						}
						if (settlement.IsTown && (buildingType2.BuildingLocation == BuildingLocation.Settlement || buildingType2.BuildingLocation == BuildingLocation.Daily))
						{
							buildingType = buildingType2;
							break;
						}
					}
				}
				if (buildingType == null)
				{
					return "Development could not be found.\n" + text;
				}
				foreach (Building building in settlement.Town.Buildings)
				{
					if (building.BuildingType == buildingType)
					{
						if (building.CurrentLevel < 3)
						{
							Building building2 = building;
							int currentLevel = building2.CurrentLevel;
							building2.CurrentLevel = currentLevel + 1;
							return string.Concat(new object[]
							{
								buildingType.Name,
								" level increased to ",
								building.CurrentLevel,
								" at ",
								settlement.Name
							});
						}
						return buildingType.Name + " is already at top level!";
					}
				}
			}
			return "Settlement is not found\n" + text;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000145A8 File Offset: 0x000127A8
		[CommandLineFunctionality.CommandLineArgumentFunction("add_progress_to_current_building", "campaign")]
		public static string AddDevelopmentProgress(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_progress_to_current_building [SettlementName] | [Progress (0-100)]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a positive number\n" + text;
			}
			if (num > 100 || num < 0)
			{
				return "Progress must be between 0 and 100.";
			}
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[0]);
			if (settlement != null && settlement.IsFortification)
			{
				Building currentBuilding = settlement.Town.CurrentBuilding;
				if (currentBuilding != null)
				{
					if (!currentBuilding.BuildingType.IsDefaultProject)
					{
						settlement.Town.BuildingsInProgress.Peek().BuildingProgress += ((float)currentBuilding.GetConstructionCost() - currentBuilding.BuildingProgress) * (float)num / 100f;
						return string.Concat(new object[]
						{
							"Development progress increased to ",
							(int)(settlement.Town.BuildingsInProgress.Peek().BuildingProgress * 100f),
							" at ",
							settlement.Name
						});
					}
					return "Currently there are no buildings in queue.";
				}
			}
			return "Settlement is not found\n" + text;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00014700 File Offset: 0x00012900
		[CommandLineFunctionality.CommandLineArgumentFunction("set_current_building", "campaign")]
		public static string SetCurrentDevelopment(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_current_building [SettlementName] | [BuildingTypeName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[0]);
			if (settlement != null && settlement.IsFortification)
			{
				BuildingType buildingType = null;
				bool flag = true;
				foreach (BuildingType buildingType2 in BuildingType.All)
				{
					if (separatedNames[1].Replace(" ", "").Equals(buildingType2.Name.ToString().Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
					{
						buildingType = buildingType2;
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					foreach (Building building in settlement.Town.Buildings)
					{
						if (building.BuildingType == buildingType && building.CurrentLevel < 3)
						{
							BuildingHelper.ChangeCurrentBuilding(buildingType, settlement.Town);
							return string.Concat(new object[]
							{
								"Current building changed to ",
								building.BuildingType.Name,
								" at ",
								settlement.Name
							});
						}
					}
				}
				return "Building type is not found.\n" + text;
			}
			return "Settlement is not found\n" + text;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000148B8 File Offset: 0x00012AB8
		[CommandLineFunctionality.CommandLineArgumentFunction("add_skill_xp_to_hero", "campaign")]
		public static string AddSkillXpToHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			Hero hero = Hero.MainHero;
			int num = 100;
			string text = "Format is \"campaign.add_skill_xp_to_hero [HeroName] | [SkillName] | [PositiveNumber]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
				if (separatedNames.Count == 1)
				{
					string text2 = "";
					if (int.TryParse(separatedNames[0], out num))
					{
						if (num <= 0)
						{
							return "Please enter a positive number\n" + text;
						}
						foreach (SkillObject skillObject in Skills.All)
						{
							hero.HeroDeveloper.AddSkillXp(skillObject, (float)num, true, true);
							int num2 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject) * (float)num);
							text2 += string.Format("{0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill.\n", new object[]
							{
								num,
								num2,
								hero.Name,
								skillObject.Name
							});
						}
						return text2;
					}
					else
					{
						hero = CampaignCheats.GetHero(separatedNames[0]);
						num = 100;
						if (hero == null)
						{
							hero = Hero.MainHero;
							string text3 = separatedNames[0];
							foreach (SkillObject skillObject2 in Skills.All)
							{
								if (skillObject2.Name.ToString().Replace(" ", "").Equals(text3.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || skillObject2.StringId == text3.Replace(" ", ""))
								{
									if (hero.GetSkillValue(skillObject2) < 300)
									{
										hero.HeroDeveloper.AddSkillXp(skillObject2, (float)num, true, true);
										int num2 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject2) * (float)num);
										return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[]
										{
											num,
											num2,
											hero.Name,
											skillObject2.Name
										});
									}
									return string.Format("{0} value for {1} is already at max.. ", skillObject2, hero);
								}
							}
							return text;
						}
						foreach (SkillObject skillObject3 in Skills.All)
						{
							hero.HeroDeveloper.AddSkillXp(skillObject3, (float)num, true, true);
							int num2 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject3) * (float)num);
							text2 += string.Format("{0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill.\n", new object[]
							{
								num,
								num2,
								hero.Name,
								skillObject3.Name
							});
						}
						return text2;
					}
				}
				else
				{
					if (separatedNames.Count == 2)
					{
						hero = CampaignCheats.GetHero(separatedNames[0]);
						if (hero != null)
						{
							if (int.TryParse(separatedNames[1], out num))
							{
								if (num <= 0)
								{
									return "Please enter a positive number\n" + text;
								}
								using (List<SkillObject>.Enumerator enumerator = Skills.All.GetEnumerator())
								{
									if (!enumerator.MoveNext())
									{
										goto IL_706;
									}
									SkillObject skillObject4 = enumerator.Current;
									if (hero.GetSkillValue(skillObject4) < 300)
									{
										hero.HeroDeveloper.AddSkillXp(skillObject4, (float)num, true, true);
										int num3 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject4) * (float)num);
										return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[]
										{
											num,
											num3,
											hero.Name,
											skillObject4.Name
										});
									}
									return string.Format("{0} value for {1} is already at max.. ", skillObject4, hero);
								}
							}
							num = 100;
							string text4 = separatedNames[1];
							foreach (SkillObject skillObject5 in Skills.All)
							{
								if (skillObject5.Name.ToString().Replace(" ", "").Equals(text4.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || skillObject5.StringId == text4.Replace(" ", ""))
								{
									if (hero.GetSkillValue(skillObject5) < 300)
									{
										hero.HeroDeveloper.AddSkillXp(skillObject5, (float)num, true, true);
										int num4 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject5) * (float)num);
										return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[]
										{
											num,
											num4,
											hero.Name,
											skillObject5.Name
										});
									}
									return string.Format("{0} value for {1} is already at max.. ", skillObject5, hero);
								}
							}
							return "Skill not found.\n" + text;
						}
						hero = Hero.MainHero;
						if (!int.TryParse(separatedNames[1], out num))
						{
							return text;
						}
						if (num <= 0)
						{
							return "Please enter a positive number\n" + text;
						}
						string text5 = separatedNames[0];
						foreach (SkillObject skillObject6 in Skills.All)
						{
							if (skillObject6.Name.ToString().Replace(" ", "").Equals(text5.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || skillObject6.StringId == text5.Replace(" ", ""))
							{
								if (hero.GetSkillValue(skillObject6) < 300)
								{
									hero.HeroDeveloper.AddSkillXp(skillObject6, (float)num, true, true);
									int num5 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject6) * (float)num);
									return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[]
									{
										num,
										num5,
										hero.Name,
										skillObject6.Name
									});
								}
								return string.Format("{0} value for {1} is already at max.. ", skillObject6, hero);
							}
						}
						return "Skill not found.\n" + text;
					}
					IL_706:
					if (separatedNames.Count != 3)
					{
						return text;
					}
					if (!int.TryParse(separatedNames[2], out num) || num < 0)
					{
						return "Please enter a positive number\n" + text;
					}
					hero = CampaignCheats.GetHero(separatedNames[0]);
					if (hero == null)
					{
						return "Hero is not found\n" + text;
					}
					string text6 = separatedNames[1];
					foreach (SkillObject skillObject7 in Skills.All)
					{
						if (skillObject7.Name.ToString().Replace(" ", "").Equals(text6.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || skillObject7.StringId == text6.Replace(" ", ""))
						{
							if (hero.GetSkillValue(skillObject7) < 300)
							{
								hero.HeroDeveloper.AddSkillXp(skillObject7, (float)num, true, true);
								int num6 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject7) * (float)num);
								return string.Format("Input {0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill. ", new object[]
								{
									num,
									num6,
									hero.Name,
									skillObject7.Name
								});
							}
							return string.Format("{0} value for {1} is already at max.. ", skillObject7, hero);
						}
					}
					return "Skill not found.\n" + text;
				}
				string result;
				return result;
			}
			if (hero != null)
			{
				string text7 = "";
				foreach (SkillObject skillObject8 in Skills.All)
				{
					hero.HeroDeveloper.AddSkillXp(skillObject8, (float)num, true, true);
					int num7 = (int)(hero.HeroDeveloper.GetFocusFactor(skillObject8) * (float)num);
					text7 += string.Format("{0} xp is modified to {1} xp due to focus point factor \nand added to the {2}'s {3} skill.\n", new object[]
					{
						num,
						num7,
						hero.Name,
						skillObject8.Name
					});
				}
				return text7;
			}
			return "Wrong Input.\n" + text;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0001519C File Offset: 0x0001339C
		[CommandLineFunctionality.CommandLineArgumentFunction("print_prisoners", "campaign")]
		public static string PrintPrisoners(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.print_prisoners\".";
			}
			string text = "";
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsPrisoner)
				{
					text = string.Concat(new object[]
					{
						text,
						hero.Name,
						"    (captor: ",
						hero.PartyBelongedToAsPrisoner.Name,
						")\n"
					});
				}
			}
			return text + "\nSuccess";
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00015260 File Offset: 0x00013460
		[CommandLineFunctionality.CommandLineArgumentFunction("add_companions", "campaign")]
		public static string AddCompanions(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_companions [Number]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num;
			if (!int.TryParse(strings[0], out num))
			{
				return "Invalid number.\n" + text;
			}
			if (num <= 0)
			{
				return "Please enter a positive number\n" + text;
			}
			for (int i = 0; i < num; i++)
			{
				CampaignCheats.AddCompanion(new List<string>());
			}
			return "Success";
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000152E0 File Offset: 0x000134E0
		[CommandLineFunctionality.CommandLineArgumentFunction("add_companion", "campaign")]
		public static string AddCompanion(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.add_companion\".";
			}
			CharacterObject wanderer = CharacterObject.PlayerCharacter.Culture.NotableAndWandererTemplates.GetRandomElementWithPredicate((CharacterObject x) => x.Occupation == Occupation.Wanderer);
			Settlement randomElementWithPredicate = Settlement.All.GetRandomElementWithPredicate((Settlement settlement) => settlement.Culture == wanderer.Culture && settlement.IsTown);
			Hero hero = HeroCreator.CreateSpecialHero(wanderer, randomElementWithPredicate, null, null, -1);
			GiveGoldAction.ApplyBetweenCharacters(null, hero, 20000, true);
			hero.SetHasMet();
			hero.ChangeState(Hero.CharacterStates.Active);
			AddCompanionAction.Apply(Clan.PlayerClan, hero);
			AddHeroToPartyAction.Apply(hero, MobileParty.MainParty, true);
			return "companion has been added.";
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000153B4 File Offset: 0x000135B4
		[CommandLineFunctionality.CommandLineArgumentFunction("set_player_reputation_trait", "campaign")]
		public static string SetPlayerReputationTrait(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_player_reputation_trait [Trait] | [Number]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num;
			if (int.TryParse(separatedNames[1], out num))
			{
				string text2 = separatedNames[0];
				foreach (TraitObject traitObject in TraitObject.All)
				{
					if (traitObject.Name.ToString().Replace(" ", "").Equals(text2.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || traitObject.StringId == text2.Replace(" ", ""))
					{
						if (num >= traitObject.MinValue && num <= traitObject.MaxValue)
						{
							Hero.MainHero.SetTraitLevel(traitObject, num);
							Campaign.Current.PlayerTraitDeveloper.UpdateTraitXPAccordingToTraitLevels();
							return string.Format("Set {0} to {1}.", traitObject.Name, num);
						}
						return string.Format("Number must be between {0} and {1}.", traitObject.MinValue, traitObject.MaxValue);
					}
				}
				return "Trait not found\n" + text;
			}
			return "Please enter a number\n" + text;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001554C File Offset: 0x0001374C
		[CommandLineFunctionality.CommandLineArgumentFunction("print_player_traits", "campaign")]
		public static string PrintPlayerTrait(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.print_player_traits\".";
			}
			string text = "";
			foreach (TraitObject traitObject in TraitObject.All)
			{
				text = string.Concat(new object[]
				{
					text,
					traitObject.Name.ToString(),
					" Trait Level:  ",
					Hero.MainHero.GetTraitLevel(traitObject),
					" Trait Xp: ",
					Campaign.Current.PlayerTraitDeveloper.GetPropertyValue(traitObject),
					"\n"
				});
			}
			return text;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00015620 File Offset: 0x00013820
		[CommandLineFunctionality.CommandLineArgumentFunction("add_horse", "campaign")]
		public static string AddHorse(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_horse [Number]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			if (!int.TryParse(strings[0], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (num > 0)
			{
				ItemObject itemObject = Items.All.FirstOrDefault((ItemObject x) => x.IsMountable);
				PartyBase.MainParty.ItemRoster.AddToCounts(itemObject, num);
				return string.Format("Added {0} {1} to player inventory.", num, itemObject.Name);
			}
			return "Nothing added.";
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000156D4 File Offset: 0x000138D4
		[CommandLineFunctionality.CommandLineArgumentFunction("give_settlement_to_player", "campaign")]
		public static string GiveSettlementToPlayer(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.give_settlement_to_player [SettlementName/SettlementId]\nWrite \"campaign.give_settlement_to_player help\" to list available settlements.\nWrite \"campaign.give_settlement_to_player Calradia\" to give all settlements to player.";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			if (text2.ToLower() == "help")
			{
				string text3 = "";
				text3 += "\n";
				text3 += "Available settlements";
				text3 += "\n";
				text3 += "==============================";
				text3 += "\n";
				foreach (Settlement settlement in MBObjectManager.Instance.GetObjectTypeList<Settlement>())
				{
					text3 = string.Concat(new object[]
					{
						text3,
						"Id: ",
						settlement.StringId,
						" Name: ",
						settlement.Name,
						"\n"
					});
				}
				return text3;
			}
			string value = text2;
			MBReadOnlyList<Settlement> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			if (text2.ToLower().Replace(" ", "") == "calradia")
			{
				foreach (Settlement settlement2 in objectTypeList)
				{
					if (settlement2.IsCastle || settlement2.IsTown)
					{
						ChangeOwnerOfSettlementAction.ApplyByDefault(Hero.MainHero, settlement2);
					}
				}
				return "You own all of Calradia now!";
			}
			Settlement settlement3 = CampaignCheats.GetSettlement(text2);
			if (settlement3 == null)
			{
				foreach (Settlement settlement4 in objectTypeList)
				{
					if (settlement4.Name.ToString().Equals(value, StringComparison.InvariantCultureIgnoreCase))
					{
						settlement3 = settlement4;
						break;
					}
				}
			}
			if (settlement3 == null)
			{
				return "Given settlement name or id could not be found.\n" + text;
			}
			if (settlement3.IsVillage)
			{
				return "Settlement must be castle or town.";
			}
			ChangeOwnerOfSettlementAction.ApplyByDefault(Hero.MainHero, settlement3);
			return settlement3.Name + " has been given to the player.";
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00015918 File Offset: 0x00013B18
		[CommandLineFunctionality.CommandLineArgumentFunction("give_settlement_to_kingdom", "campaign")]
		public static string GiveSettlementToKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.give_settlement_to_kingdom [SettlementName] | [KingdomName]";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[0]);
			if (settlement == null)
			{
				return "Given settlement name could not be found.\n" + text;
			}
			if (settlement.IsVillage)
			{
				settlement = settlement.Village.Bound;
			}
			Kingdom kingdom = CampaignCheats.GetKingdom(separatedNames[1]);
			if (kingdom == null)
			{
				return "Given kingdom could not be found.\n" + text;
			}
			if (settlement.MapFaction == kingdom)
			{
				return "Kingdom already owns the settlement.";
			}
			if (settlement.IsVillage)
			{
				return "Settlement must be castle or town.";
			}
			ChangeOwnerOfSettlementAction.ApplyByDefault(kingdom.Leader, settlement);
			return settlement.Name + string.Format(" has been given to {0}.", kingdom.Leader.Name);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00015A08 File Offset: 0x00013C08
		[CommandLineFunctionality.CommandLineArgumentFunction("add_power_to_notable", "campaign")]
		public static string AddPowerToNotable(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.add_power_to_notable [HeroName] | [Number]";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num))
			{
				return "Please enter a positive number\n" + text;
			}
			if (num <= 0)
			{
				return "Please enter a positive number\n" + text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (!hero.IsNotable)
			{
				return "Hero is not a notable.";
			}
			hero.AddPower((float)num);
			return string.Format("{0} power is {1}", hero.Name, hero.Power);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00015ADC File Offset: 0x00013CDC
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_hero", "campaign")]
		public static string KillHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.kill_hero [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero = CampaignCheats.GetHero(text2);
			if (hero == null)
			{
				return "Hero is not found: " + text2.ToLower() + "\n" + text;
			}
			if (!hero.IsAlive)
			{
				return "Hero " + text2 + " is already dead.";
			}
			if (!hero.CanDie(KillCharacterAction.KillCharacterActionDetail.Murdered))
			{
				return "Hero cant die!";
			}
			if (hero == Hero.MainHero)
			{
				return "Hero " + text2 + " is main hero. Use [campaingn.make_main_hero_ill] to kill main hero.";
			}
			KillCharacterAction.ApplyByMurder(hero, null, true);
			return "Hero " + text2.ToLower() + " is killed.";
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00015B9C File Offset: 0x00013D9C
		[CommandLineFunctionality.CommandLineArgumentFunction("make_main_hero_ill", "campaign")]
		private static string KillMainHero(List<string> strings)
		{
			string result = "";
			if (!CampaignCheats.CheckCheatUsage(ref result))
			{
				return result;
			}
			Campaign.Current.MainHeroIllDays = 500;
			Hero.MainHero.HitPoints = Hero.MainHero.CharacterObject.MaxHitPoints();
			return "Success";
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00015BE8 File Offset: 0x00013DE8
		[CommandLineFunctionality.CommandLineArgumentFunction("print_character_feats", "campaign")]
		public static string PrintCharacterFeats(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.print_character_feats [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero = CampaignCheats.GetHero(text2);
			string text3 = "";
			if (hero != null)
			{
				foreach (FeatObject featObject in FeatObject.All)
				{
					text3 = string.Concat(new object[]
					{
						text3,
						"\n",
						featObject.Name,
						" :",
						hero.Culture.HasFeat(featObject).ToString()
					});
				}
				return text3;
			}
			return "Hero is not found: " + text2 + "\n" + text;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00015CD0 File Offset: 0x00013ED0
		[CommandLineFunctionality.CommandLineArgumentFunction("make_hero_fugitive", "campaign")]
		public static string MakeHeroFugitive(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.make_hero_fugitive [HeroName]";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero = CampaignCheats.GetHero(text2);
			if (hero == null)
			{
				return "Hero is not found: " + text2.ToLower() + "\n" + text;
			}
			if (!hero.IsAlive)
			{
				return "Hero " + text2 + " is dead.";
			}
			if (hero.PartyBelongedTo != null)
			{
				if (hero.PartyBelongedTo == MobileParty.MainParty)
				{
					return "You cannot be fugitive when you are in your main party.";
				}
				DestroyPartyAction.Apply(null, hero.PartyBelongedTo);
			}
			MakeHeroFugitiveAction.Apply(hero);
			return "Hero " + text2.ToLower() + " is now fugitive.";
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00015D8C File Offset: 0x00013F8C
		[CommandLineFunctionality.CommandLineArgumentFunction("leave_faction", "campaign")]
		public static string LeaveFaction(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.leave_faction\".";
			}
			if (Hero.MainHero.MapFaction == Clan.PlayerClan)
			{
				return "Function execution failed.";
			}
			if (Hero.MainHero.MapFaction.Leader == Hero.MainHero)
			{
				string objectName;
				if (Hero.MainHero.MapFaction.Name.ToString().ToLower() == "empire")
				{
					objectName = "lord_1_1";
				}
				else if (Hero.MainHero.MapFaction.Name.ToString().ToLower() == "sturgia")
				{
					objectName = "lord_2_1";
				}
				else if (Hero.MainHero.MapFaction.Name.ToString().ToLower() == "aserai")
				{
					objectName = "lord_3_1";
				}
				else if (Hero.MainHero.MapFaction.Name.ToString().ToLower() == "vlandia")
				{
					objectName = "lord_4_1";
				}
				else if (Hero.MainHero.MapFaction.Name.ToString().ToLower() == "battania")
				{
					objectName = "lord_5_1";
				}
				else if (Hero.MainHero.MapFaction.Name.ToString().ToLower() == "khuzait")
				{
					objectName = "lord_6_1";
				}
				else
				{
					objectName = "lord_1_1";
				}
				Hero heroObject = Game.Current.ObjectManager.GetObject<CharacterObject>(objectName).HeroObject;
				if (!Hero.MainHero.MapFaction.IsKingdomFaction)
				{
					(Hero.MainHero.MapFaction as Clan).SetLeader(heroObject);
				}
				else
				{
					ChangeRulingClanAction.Apply(Hero.MainHero.MapFaction as Kingdom, heroObject.Clan);
				}
			}
			ChangeKingdomAction.ApplyByLeaveKingdom(Hero.MainHero.Clan, true);
			return "Success";
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00015F7C File Offset: 0x0001417C
		[CommandLineFunctionality.CommandLineArgumentFunction("lead_your_faction", "campaign")]
		public static string LeadYourFaction(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.lead_your_faction\".";
			}
			if (Hero.MainHero.MapFaction.Leader != Hero.MainHero)
			{
				if (Hero.MainHero.MapFaction.IsKingdomFaction)
				{
					ChangeRulingClanAction.Apply(Hero.MainHero.MapFaction as Kingdom, Clan.PlayerClan);
				}
				else
				{
					(Hero.MainHero.MapFaction as Clan).SetLeader(Hero.MainHero);
				}
				return "Success";
			}
			return "Function execution failed.";
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00016010 File Offset: 0x00014210
		[CommandLineFunctionality.CommandLineArgumentFunction("print_heroes_suitable_for_marriage", "campaign")]
		public static string PrintHeroesSuitableForMarriage(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"print_heroes_suitable_for_marriage\".";
			}
			List<Hero> list = new List<Hero>();
			List<Hero> list2 = new List<Hero>();
			foreach (Kingdom kingdom in Kingdom.All)
			{
				foreach (Hero hero in kingdom.Lords)
				{
					if (hero.CanMarry())
					{
						if (hero.IsFemale)
						{
							list.Add(hero);
						}
						else
						{
							list2.Add(hero);
						}
					}
				}
			}
			string text = "Maidens:\n";
			string text2 = "Suitors:\n";
			foreach (Hero hero2 in list)
			{
				TextObject textObject = (hero2.PartyBelongedTo == null) ? TextObject.Empty : hero2.PartyBelongedTo.Name;
				text = string.Concat(new object[]
				{
					text,
					"Name: ",
					hero2.Name,
					" --- Clan: ",
					hero2.Clan,
					" --- Party:",
					textObject,
					"\n"
				});
			}
			foreach (Hero hero3 in list2)
			{
				TextObject textObject2 = (hero3.PartyBelongedTo == null) ? TextObject.Empty : hero3.PartyBelongedTo.Name;
				text2 = string.Concat(new object[]
				{
					text2,
					"Name: ",
					hero3.Name,
					" --- Clan: ",
					hero3.Clan,
					" --- Party:",
					textObject2,
					"\n"
				});
			}
			return text + "\n" + text2;
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00016240 File Offset: 0x00014440
		[CommandLineFunctionality.CommandLineArgumentFunction("marry_player_with_hero", "campaign")]
		public static string MarryPlayerWithHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.marry_player_with_hero [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			if (!Campaign.Current.Models.MarriageModel.IsSuitableForMarriage(Hero.MainHero))
			{
				return "Main hero is not suitable for marriage";
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero = CampaignCheats.GetHero(text2);
			if (hero == null)
			{
				return "Hero is not found: " + text2.ToLower() + "\n" + text;
			}
			MarriageModel marriageModel = Campaign.Current.Models.MarriageModel;
			if (marriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, hero))
			{
				MarriageAction.Apply(Hero.MainHero, hero, true);
				return "Success";
			}
			if (!marriageModel.IsSuitableForMarriage(hero))
			{
				return string.Format("Hero: {0} is not suitable for marriage.", hero.Name);
			}
			if (!marriageModel.IsClanSuitableForMarriage(hero.Clan))
			{
				return string.Format("{0}'s clan is not suitable for marriage.", hero.Name);
			}
			if (!marriageModel.IsClanSuitableForMarriage(Hero.MainHero.Clan))
			{
				return "Main hero's clan is not suitable for marriage.";
			}
			Clan clan = Hero.MainHero.Clan;
			if (((clan != null) ? clan.Leader : null) == Hero.MainHero)
			{
				Clan clan2 = hero.Clan;
				if (((clan2 != null) ? clan2.Leader : null) == hero)
				{
					return "Clan leaders are not suitable for marriage.";
				}
			}
			if (!hero.IsFemale)
			{
				return "Hero is not female.";
			}
			DefaultMarriageModel obj = new DefaultMarriageModel();
			if ((bool)typeof(DefaultMarriageModel).GetMethod("AreHeroesRelated", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, new object[]
			{
				Hero.MainHero,
				hero
			}))
			{
				return "Heroes are related.";
			}
			Hero courtedHeroInOtherClan = Romance.GetCourtedHeroInOtherClan(Hero.MainHero, hero);
			if (courtedHeroInOtherClan != null && courtedHeroInOtherClan != hero)
			{
				return string.Format("{0} has courted {1}.", courtedHeroInOtherClan.Name, Hero.MainHero.Name);
			}
			Hero courtedHeroInOtherClan2 = Romance.GetCourtedHeroInOtherClan(hero, Hero.MainHero);
			if (courtedHeroInOtherClan2 != null && courtedHeroInOtherClan2 != Hero.MainHero)
			{
				return string.Format("{0} has courted {1}.", courtedHeroInOtherClan2.Name, hero.Name);
			}
			return string.Concat(new object[]
			{
				"Marriage is not suitable between ",
				Hero.MainHero.Name,
				" and ",
				hero.Name,
				"\n"
			});
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00016474 File Offset: 0x00014674
		[CommandLineFunctionality.CommandLineArgumentFunction("is_hero_suitable_for_marriage_with_player", "campaign")]
		public static string IsHeroSuitableForMarriageWithPlayer(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.is_hero_suitable_for_marriage_with_player [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			Hero hero = CampaignCheats.GetHero(text2);
			if (hero == null)
			{
				return "Hero is not found: " + text2.ToLower() + "\n" + text;
			}
			if (Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, hero))
			{
				return string.Concat(new object[]
				{
					"Marriage is suitable between ",
					Hero.MainHero.Name,
					" and ",
					hero.Name,
					"\n"
				});
			}
			return string.Concat(new object[]
			{
				"Marriage is not suitable between ",
				Hero.MainHero.Name,
				" and ",
				hero.Name,
				"\n"
			});
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0001656C File Offset: 0x0001476C
		[CommandLineFunctionality.CommandLineArgumentFunction("start_player_vs_world_war", "campaign")]
		public static string StartPlayerVsWorldWar(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.start_player_vs_world_war\".";
			}
			foreach (IFaction faction in Campaign.Current.Factions)
			{
				if ((faction != Clan.PlayerClan || faction != Hero.MainHero.MapFaction) && !faction.IsEliminated && (faction.IsKingdomFaction || faction.IsMinorFaction))
				{
					DeclareWarAction.ApplyByDefault(faction, Clan.PlayerClan);
				}
			}
			return "Success";
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00016614 File Offset: 0x00014814
		[CommandLineFunctionality.CommandLineArgumentFunction("start_player_vs_world_truce", "campaign")]
		public static string StartPlayerVsWorldTruce(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.start_player_vs_world_truce\".";
			}
			foreach (IFaction faction in Campaign.Current.Factions)
			{
				if (faction != Clan.PlayerClan || faction != Hero.MainHero.MapFaction)
				{
					MakePeaceAction.Apply(faction, Clan.PlayerClan, 0);
				}
			}
			return "Success";
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000166A4 File Offset: 0x000148A4
		[CommandLineFunctionality.CommandLineArgumentFunction("create_player_kingdom", "campaign")]
		public static string CreatePlayerKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 0))
			{
				return "Format is \"campaign.create_player_kingdom\".";
			}
			Campaign.Current.KingdomManager.CreateKingdom(Clan.PlayerClan.Name, Clan.PlayerClan.InformalName, Clan.PlayerClan.Culture, Clan.PlayerClan, null, null, null, null);
			return "Success";
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00016718 File Offset: 0x00014918
		[CommandLineFunctionality.CommandLineArgumentFunction("create_random_clan", "campaign")]
		public static string CreateRandomClan(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.create_random_clan [KingdomName]\".";
			if (CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			Kingdom kingdom;
			if (strings.Count > 0)
			{
				kingdom = CampaignCheats.GetKingdom(CampaignCheats.ConcatenateString(strings));
			}
			else
			{
				kingdom = Kingdom.All.GetRandomElement<Kingdom>();
			}
			if (kingdom == null)
			{
				return "Kingdom is not valid!\n" + text;
			}
			CultureObject culture = kingdom.Culture;
			Settlement settlement;
			if ((settlement = kingdom.Settlements.FirstOrDefault((Settlement x) => x.IsTown)) == null)
			{
				settlement = (kingdom.Settlements.GetRandomElement<Settlement>() ?? Settlement.All.FirstOrDefault((Settlement x) => x.IsTown && x.Culture == culture));
			}
			Settlement settlement2 = settlement;
			TextObject name = NameGenerator.Current.GenerateClanName(culture, settlement2);
			Clan clan = Clan.CreateClan("test_clan_" + Clan.All.Count);
			clan.InitializeClan(name, new TextObject("{=!}informal", null), Kingdom.All.GetRandomElement<Kingdom>().Culture, Banner.CreateRandomClanBanner(-1), default(Vec2), false);
			CharacterObject characterObject = culture.LordTemplates.FirstOrDefault((CharacterObject x) => x.Occupation == Occupation.Lord);
			if (characterObject == null)
			{
				return "Can't find a proper lord template.\n" + text;
			}
			Settlement bornSettlement = kingdom.Settlements.GetRandomElement<Settlement>() ?? Settlement.All.FirstOrDefault((Settlement x) => x.IsTown && x.Culture == culture);
			Hero hero = HeroCreator.CreateSpecialHero(characterObject, bornSettlement, clan, null, MBRandom.RandomInt(18, 36));
			hero.HeroDeveloper.InitializeHeroDeveloper(false, null);
			hero.ChangeState(Hero.CharacterStates.Active);
			clan.SetLeader(hero);
			ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom, false);
			EnterSettlementAction.ApplyForCharacterOnly(hero, settlement2);
			GiveGoldAction.ApplyBetweenCharacters(null, hero, 15000, false);
			return string.Format("{0} is added to {1}. Its leader is: {2}", clan.Name, kingdom.Name, hero.Name);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0001692C File Offset: 0x00014B2C
		[CommandLineFunctionality.CommandLineArgumentFunction("join_kingdom", "campaign")]
		public static string JoinKingdom(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.join_kingdom[KingdomName / FirstTwoCharactersOfKingdomName]\".\nWrite \"campaign.join_kingdom help\" to list available Kingdoms.";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings).Replace(" ", "");
			if (text2.ToLower() == "help")
			{
				string text3 = "";
				text3 += "\n";
				text3 += "Format is \"campaign.join_kingdom [KingdomName/FirstTwoCharacterOfKingdomName]\".";
				text3 += "\n";
				text3 += "Available Kingdoms";
				text3 += "\n";
				foreach (Kingdom kingdom in Kingdom.All)
				{
					text3 = text3 + "Kingdom name " + kingdom.Name.ToString() + "\n";
				}
				return text3;
			}
			Kingdom kingdom2 = null;
			foreach (Kingdom kingdom3 in Kingdom.All)
			{
				if (kingdom3.Name.ToString().Equals(text2, StringComparison.OrdinalIgnoreCase))
				{
					kingdom2 = kingdom3;
					break;
				}
				if (text2.Length >= 2 && kingdom3.Name.ToString().ToLower().Substring(0, 2).Equals(text2.ToLower().Substring(0, 2)))
				{
					kingdom2 = kingdom3;
					break;
				}
			}
			if (kingdom2 == null)
			{
				return "Kingdom is not found: " + text2 + "\n" + text;
			}
			if (Hero.MainHero.Clan.Kingdom == kingdom2)
			{
				return "Already in kingdom";
			}
			ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, kingdom2, true);
			return "Success";
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00016B00 File Offset: 0x00014D00
		[CommandLineFunctionality.CommandLineArgumentFunction("join_kingdom_as_mercenary", "campaign")]
		public static string JoinKingdomAsMercenary(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.join_kingdom_as_mercenary[KingdomName / FirstTwoCharactersOfKingdomName]\".\nWrite \"campaign.join_kingdom_as_mercenary help\" to list available Kingdoms.";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings).Replace(" ", "");
			if (text2.ToLower() == "help")
			{
				string text3 = "";
				text3 += "\n";
				text3 += "Format is \"campaign.join_kingdom_as_mercenary [KingdomName/FirstTwoCharacterOfKingdomName]\".";
				text3 += "\n";
				text3 += "Available Kingdoms";
				text3 += "\n";
				foreach (Kingdom kingdom in Kingdom.All)
				{
					text3 = text3 + "Kingdom name " + kingdom.Name.ToString() + "\n";
				}
				return text3;
			}
			Kingdom kingdom2 = null;
			foreach (Kingdom kingdom3 in Kingdom.All)
			{
				if (kingdom3.Name.ToString().Equals(text2, StringComparison.OrdinalIgnoreCase))
				{
					kingdom2 = kingdom3;
					break;
				}
				if (text2.Length >= 2 && kingdom3.Name.ToString().ToLower().Substring(0, 2).Equals(text2.ToLower().Substring(0, 2)))
				{
					kingdom2 = kingdom3;
					break;
				}
			}
			if (kingdom2 == null)
			{
				return "Kingdom is not found: " + text2 + "\n" + text;
			}
			ChangeKingdomAction.ApplyByJoinFactionAsMercenary(Hero.MainHero.Clan, kingdom2, 50, true);
			return "Success";
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00016CBC File Offset: 0x00014EBC
		[CommandLineFunctionality.CommandLineArgumentFunction("set_criminal_rating", "campaign")]
		public static string SetCriminalRating(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			int num = 0;
			if (CampaignCheats.CheckHelp(strings))
			{
				string text = "";
				text += "\n";
				text += "Format is \"campaign.set_criminal_rating [Faction] | [Gold]\".";
				text += "\n";
				text += "Available Factions";
				text += "\n";
				foreach (Kingdom kingdom in Kingdom.All)
				{
					text = text + "Faction name " + kingdom.Name.ToString() + "\n";
				}
				foreach (Clan clan in Clan.NonBanditFactions)
				{
					text = text + "Faction name " + clan.Name.ToString() + "\n";
				}
				return text;
			}
			string text2 = "Format is \"campaign.set_criminal_rating [FactionName] | [Value]\".\nWrite \"campaign.set_criminal_rating help\" to list available Factions.";
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return text2;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text2;
			}
			if (!int.TryParse(separatedNames[1], out num))
			{
				return text2;
			}
			string text3 = separatedNames[0];
			foreach (Clan clan2 in Clan.NonBanditFactions)
			{
				if (clan2.Name.ToString().Replace(" ", "").Equals(text3.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					ChangeCrimeRatingAction.Apply(clan2, (float)num - clan2.MainHeroCrimeRating, true);
					return "Success";
				}
			}
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (kingdom2.Name.ToString().Replace(" ", "").Equals(text3.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					ChangeCrimeRatingAction.Apply(kingdom2, (float)num - kingdom2.MainHeroCrimeRating, true);
					return "Success";
				}
			}
			return "Faction is not found: " + text3 + "\n" + text2;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00016F5C File Offset: 0x0001515C
		[CommandLineFunctionality.CommandLineArgumentFunction("print_criminal_ratings", "campaign")]
		public static string PrintCriminalRatings(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.print_criminal_ratings";
			}
			string text = "";
			bool flag = true;
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (kingdom.MainHeroCrimeRating > 0f)
				{
					text = string.Concat(new object[]
					{
						text,
						kingdom.Name,
						"   criminal rating: ",
						kingdom.MainHeroCrimeRating,
						"\n"
					});
					flag = false;
				}
			}
			text += "-----------\n";
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				if (clan.MainHeroCrimeRating > 0f)
				{
					text = string.Concat(new object[]
					{
						text,
						clan.Name,
						"   criminal rating: ",
						clan.MainHeroCrimeRating,
						"\n"
					});
					flag = false;
				}
			}
			if (flag)
			{
				return "You don't have any criminal rating.";
			}
			return text;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000170B8 File Offset: 0x000152B8
		[CommandLineFunctionality.CommandLineArgumentFunction("set_main_hero_age", "campaign")]
		public static string SetMainHeroAge(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_main_hero_age [Age]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			if (!int.TryParse(strings[0], out num))
			{
				return "Please enter a number\n" + text;
			}
			if (num < Campaign.Current.Models.AgeModel.HeroComesOfAge || num > Campaign.Current.Models.AgeModel.MaxAge)
			{
				return string.Format("Age must be between {0} - {1}", Campaign.Current.Models.AgeModel.HeroComesOfAge, Campaign.Current.Models.AgeModel.MaxAge);
			}
			Hero.MainHero.SetBirthDay(HeroHelper.GetRandomBirthDayForAge((float)num));
			return "Success";
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0001718F File Offset: 0x0001538F
		public static bool MainPartyIsAttackable
		{
			get
			{
				return CampaignCheats._mainPartyIsAttackable;
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00017198 File Offset: 0x00015398
		[CommandLineFunctionality.CommandLineArgumentFunction("set_main_party_attackable", "campaign")]
		public static string SetMainPartyAttackable(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.set_main_party_attackable [1/0]\".";
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			if (strings[0] == "0" || strings[0] == "1")
			{
				bool flag = strings[0] == "1";
				CampaignCheats._mainPartyIsAttackable = flag;
				return "Main party is" + (flag ? " " : " NOT ") + "attackable.";
			}
			return "Wrong input.\n" + text;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00017238 File Offset: 0x00015438
		[CommandLineFunctionality.CommandLineArgumentFunction("add_morale_to_party", "campaign")]
		public static string AddMoraleToParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_morale_to_party [HeroName] | [Number]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 10;
			Hero hero = Hero.MainHero;
			string text2 = "";
			bool flag = false;
			if (CampaignCheats.CheckParameters(strings, 1))
			{
				if (!int.TryParse(strings[0], out num))
				{
					num = 10;
					text2 = strings[0];
					hero = CampaignCheats.GetHero(text2);
					flag = true;
				}
			}
			else
			{
				List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
				if (separatedNames.Count != 2)
				{
					return text;
				}
				if (!int.TryParse(separatedNames[1], out num))
				{
					num = 100;
					text2 = separatedNames[0];
					hero = CampaignCheats.GetHero(text2);
				}
				else
				{
					text2 = separatedNames[0];
					hero = CampaignCheats.GetHero(text2);
				}
				flag = true;
			}
			if (hero != null)
			{
				MobileParty partyBelongedTo = hero.PartyBelongedTo;
				if (partyBelongedTo != null)
				{
					float num2 = MBMath.ClampFloat(partyBelongedTo.RecentEventsMorale + (float)num, 0f, float.MaxValue);
					float num3 = num2 - partyBelongedTo.RecentEventsMorale;
					partyBelongedTo.RecentEventsMorale = num2;
					return string.Format("The base morale of {0}'s party changed by {1}.", hero.Name, num3);
				}
				return "Hero: " + text2 + " does not belonged to any party.\n" + text;
			}
			else
			{
				if (flag)
				{
					return "Hero: " + text2 + " not found.\n" + text;
				}
				return "Wrong input.\n" + text;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00017390 File Offset: 0x00015590
		[CommandLineFunctionality.CommandLineArgumentFunction("boost_cohesion_of_army", "campaign")]
		public static string BoostCohesionOfArmy(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"boost_cohesion_of_army [ArmyLeaderName]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Hero hero = Hero.MainHero;
			Army army = hero.PartyBelongedTo.Army;
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				string text2 = CampaignCheats.ConcatenateString(strings.GetRange(0, strings.Count));
				hero = CampaignCheats.GetHero(text2);
				if (hero == null)
				{
					return "Hero: " + text2 + " not found.\n" + text;
				}
				if (hero.PartyBelongedTo == null)
				{
					return "Hero: " + text2 + " does not belong to any army.";
				}
				army = hero.PartyBelongedTo.Army;
				if (army == null)
				{
					return "Hero: " + text2 + " does not belong to any army.";
				}
			}
			if (army != null)
			{
				army.Cohesion = 100f;
				return string.Format("{0}'s army cohesion is boosted.", hero.Name);
			}
			return "Wrong input.\n" + text;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00017474 File Offset: 0x00015674
		[CommandLineFunctionality.CommandLineArgumentFunction("boost_cohesion_of_all_armies", "campaign")]
		public static string BoostCohersionOfAllArmies(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"boost_cohersion_of_all_armies\".";
			}
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				foreach (MobileParty mobileParty in MobileParty.All)
				{
					if (mobileParty.Army != null)
					{
						mobileParty.Army.Cohesion = 100f;
					}
				}
				return "All armies cohesion are boosted.";
			}
			return "Wrong input.\nFormat is \"boost_cohesion_of_all_armies\".";
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001750C File Offset: 0x0001570C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_focus_points_to_hero", "campaign")]
		public static string AddFocusPointCheat(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_focus_points_to_hero [HeroName] | [PositiveNumber]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			bool flag = false;
			int num = 1;
			Hero hero;
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				Hero.MainHero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentFocusPoints + 1, 0, int.MaxValue);
				hero = Hero.MainHero;
				return string.Format("{0} focus points added to the {1}. ", num, hero.Name);
			}
			int num2 = 0;
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count == 1)
			{
				bool flag2 = int.TryParse(separatedNames[0], out num2);
				if (num2 <= 0 && flag2)
				{
					return "Please enter a positive number\n" + text;
				}
				Hero.MainHero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentFocusPoints + num2, 0, int.MaxValue);
				hero = Hero.MainHero;
				flag = true;
				num = num2;
			}
			else
			{
				if (separatedNames.Count != 2)
				{
					return text;
				}
				if (int.TryParse(separatedNames[1], out num2))
				{
					hero = CampaignCheats.GetHero(separatedNames[0]);
					if (hero != null)
					{
						hero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(hero.HeroDeveloper.UnspentFocusPoints + num2, 0, int.MaxValue);
						flag = true;
						num = num2;
					}
				}
				else
				{
					hero = CampaignCheats.GetHero(separatedNames[0]);
					if (hero != null)
					{
						hero.HeroDeveloper.UnspentFocusPoints = MBMath.ClampInt(hero.HeroDeveloper.UnspentFocusPoints + 1, 0, int.MaxValue);
						flag = true;
					}
				}
			}
			if (flag)
			{
				return string.Format("{0} focus points added to the {1}. ", num, hero.Name);
			}
			return "Hero is not found\n" + text;
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000176C4 File Offset: 0x000158C4
		[CommandLineFunctionality.CommandLineArgumentFunction("add_attribute_points_to_hero", "campaign")]
		public static string AddAttributePointsCheat(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_attribute_points_to_hero [HeroName] | [PositiveNumber]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			bool flag = false;
			int num = 1;
			Hero hero;
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				Hero.MainHero.HeroDeveloper.UnspentAttributePoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentAttributePoints + 1, 0, int.MaxValue);
				hero = Hero.MainHero;
				return string.Format("{0} attribute points added to the {1}. ", num, hero.Name);
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count == 1)
			{
				int num2;
				if (!int.TryParse(separatedNames[0], out num2) || num2 <= 0)
				{
					return "Please enter a positive number\n" + text;
				}
				Hero.MainHero.HeroDeveloper.UnspentAttributePoints = MBMath.ClampInt(Hero.MainHero.HeroDeveloper.UnspentAttributePoints + num2, 0, int.MaxValue);
				hero = Hero.MainHero;
				flag = true;
				num = num2;
			}
			else
			{
				if (separatedNames.Count != 2)
				{
					return text;
				}
				hero = CampaignCheats.GetHero(separatedNames[0]);
				if (hero != null)
				{
					flag = true;
					int num3;
					if (int.TryParse(separatedNames[1], out num3))
					{
						num = num3;
					}
					hero.HeroDeveloper.UnspentAttributePoints = MBMath.ClampInt(hero.HeroDeveloper.UnspentAttributePoints + num, 0, int.MaxValue);
				}
			}
			if (flag)
			{
				return string.Format("{0} attribute points added to the {1}. ", num, hero.Name);
			}
			return "Hero is not found\n" + text;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00017838 File Offset: 0x00015A38
		[CommandLineFunctionality.CommandLineArgumentFunction("print_tournaments", "campaign")]
		public static string PrintSettlementsWithTournament(List<string> strings)
		{
			string result = "";
			if (!CampaignCheats.CheckCheatUsage(ref result))
			{
				return result;
			}
			if (!Campaign.Current.IsDay)
			{
				return "Cant print tournaments. Wait day light.";
			}
			string text = "";
			foreach (Town town in Town.AllTowns)
			{
				if (Campaign.Current.TournamentManager.GetTournamentGame(town) != null)
				{
					text = text + town.Name + "\n";
				}
			}
			return text;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000178D4 File Offset: 0x00015AD4
		public static string ConvertListToMultiLine(List<string> strings)
		{
			string text = "";
			foreach (string str in strings)
			{
				text = text + str + "\n";
			}
			return text;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00017930 File Offset: 0x00015B30
		[CommandLineFunctionality.CommandLineArgumentFunction("print_all_issues", "campaign")]
		public static string PrintAllIssues(List<string> strings)
		{
			string result = "";
			if (!CampaignCheats.CheckCheatUsage(ref result))
			{
				return result;
			}
			string text = "Total issue count : " + Campaign.Current.IssueManager.Issues.Count + "\n";
			int num = 0;
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
			{
				text = string.Concat(new object[]
				{
					text,
					++num,
					") ",
					keyValuePair.Value.Title,
					", ",
					keyValuePair.Key,
					": ",
					keyValuePair.Value.IssueSettlement,
					"\n"
				});
			}
			return text;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00017A2C File Offset: 0x00015C2C
		[CommandLineFunctionality.CommandLineArgumentFunction("print_issues", "campaign")]
		public static string PrintIssues(List<string> strings)
		{
			string result = "";
			if (!CampaignCheats.CheckCheatUsage(ref result))
			{
				return result;
			}
			string text = "";
			Dictionary<Type, string> dictionary = new Dictionary<Type, string>();
			foreach (KeyValuePair<Hero, IssueBase> keyValuePair in Campaign.Current.IssueManager.Issues)
			{
				if (!dictionary.ContainsKey(keyValuePair.Value.GetType()))
				{
					dictionary.Add(keyValuePair.Value.GetType(), string.Concat(new object[]
					{
						keyValuePair.Value.Title,
						", ",
						keyValuePair.Key,
						": ",
						keyValuePair.Value.IssueSettlement,
						"\n"
					}));
				}
			}
			foreach (KeyValuePair<Type, string> keyValuePair2 in dictionary)
			{
				text += keyValuePair2.Value;
			}
			return text;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00017B5C File Offset: 0x00015D5C
		[CommandLineFunctionality.CommandLineArgumentFunction("give_workshop_to_player", "campaign")]
		public static string GiveWorkshopToPlayer(List<string> strings)
		{
			string text = "Format is \"campaign.give_workshop_to_player [SettlementName] | [workshop_no]\".";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return text;
			}
			if (CampaignCheats.CheckHelp(strings) || separatedNames.Count != 2)
			{
				if (Settlement.CurrentSettlement == null)
				{
					return "You need to be in a settlement to see the workshops available.";
				}
				if (Settlement.CurrentSettlement.Town == null)
				{
					return "Settlement should be town\n";
				}
				if (separatedNames.Count != 1)
				{
					string text2 = text;
					for (int i = 0; i < Settlement.CurrentSettlement.Town.Workshops.Length; i++)
					{
						Workshop workshop = Settlement.CurrentSettlement.Town.Workshops[i];
						text2 = string.Concat(new object[]
						{
							text2,
							"\n",
							i,
							" : ",
							workshop.Name,
							" - owner : ",
							(workshop.Owner != null) ? workshop.Owner.Name.ToString() : ""
						});
						if (workshop.WorkshopType.IsHidden)
						{
							text2 += "(hidden)";
						}
					}
					return text2;
				}
				int num;
				if (!int.TryParse(strings[0], out num))
				{
					return "Please enter a number\n" + text;
				}
				if (num > 0 && num < Settlement.CurrentSettlement.Town.Workshops.Length)
				{
					Workshop workshop2 = Settlement.CurrentSettlement.Town.Workshops[num];
					ChangeOwnerOfWorkshopAction.ApplyByPlayerBuying(workshop2);
					return string.Format("Gave {0} to {1}", workshop2.WorkshopType.Name, Hero.MainHero.Name);
				}
				return string.Format("There is no workshop with no {0}.", num);
			}
			else
			{
				Settlement settlement = CampaignCheats.GetSettlement(separatedNames[0]);
				if (settlement == null || !settlement.IsTown)
				{
					return "Settlement should be a town.";
				}
				int num2;
				if (!int.TryParse(separatedNames[1], out num2))
				{
					return "Please enter a number\n" + text;
				}
				if (num2 < 0 || num2 >= settlement.Town.Workshops.Length)
				{
					return string.Format("There is no workshop with no {0}.", num2);
				}
				Workshop workshop3 = settlement.Town.Workshops[num2];
				if (workshop3.WorkshopType.IsHidden)
				{
					return "Workshop is hidden.";
				}
				ChangeOwnerOfWorkshopAction.ApplyByPlayerBuying(workshop3);
				return "Success";
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00017D9C File Offset: 0x00015F9C
		[CommandLineFunctionality.CommandLineArgumentFunction("conceive_child", "campaign")]
		public static string MakePregnant(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (Hero.MainHero.Spouse == null)
			{
				Hero hero = Hero.AllAliveHeroes.FirstOrDefault((Hero t) => t != Hero.MainHero && Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(Hero.MainHero, t));
				if (hero == null)
				{
					return "error";
				}
				MarriageAction.Apply(Hero.MainHero, hero, true);
				if (Hero.MainHero.IsFemale ? (!Hero.MainHero.IsPregnant) : (!Hero.MainHero.Spouse.IsPregnant))
				{
					MakePregnantAction.Apply(Hero.MainHero.IsFemale ? Hero.MainHero : Hero.MainHero.Spouse);
					return "Success";
				}
				return "You are expecting a child already.";
			}
			else
			{
				if (Hero.MainHero.IsFemale ? (!Hero.MainHero.IsPregnant) : (!Hero.MainHero.Spouse.IsPregnant))
				{
					MakePregnantAction.Apply(Hero.MainHero.IsFemale ? Hero.MainHero : Hero.MainHero.Spouse);
					return "Success";
				}
				return "You are expecting a child already.";
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00017EC8 File Offset: 0x000160C8
		public static Hero GenerateChild(Hero hero, bool isFemale, CultureObject culture)
		{
			if (hero.Spouse == null)
			{
				List<Hero> list = Hero.AllAliveHeroes.ToList<Hero>();
				list.Shuffle<Hero>();
				Hero hero2 = list.FirstOrDefault((Hero t) => t != hero && Campaign.Current.Models.MarriageModel.IsCoupleSuitableForMarriage(hero, t));
				if (hero2 != null)
				{
					MarriageAction.Apply(hero, hero2, true);
					if (hero.IsFemale ? (!hero.IsPregnant) : (!hero.Spouse.IsPregnant))
					{
						MakePregnantAction.Apply(hero.IsFemale ? hero : hero.Spouse);
					}
				}
			}
			Hero hero3 = hero.IsFemale ? hero : hero.Spouse;
			Hero spouse = hero3.Spouse;
			Hero hero4 = HeroCreator.DeliverOffSpring(hero3, spouse, isFemale);
			hero4.Culture = culture;
			hero3.IsPregnant = false;
			return hero4;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00017FBC File Offset: 0x000161BC
		[CommandLineFunctionality.CommandLineArgumentFunction("add_prisoner_to_party", "campaign")]
		public static string AddPrisonerToParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_prisoner_to_party [PrisonerName] | [CapturerName]\".";
			if (CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			string heroName = separatedNames[0].Trim();
			string heroName2 = separatedNames[1].Trim();
			Hero hero = CampaignCheats.GetHero(heroName);
			Hero hero2 = CampaignCheats.GetHero(heroName2);
			if (hero == null || hero2 == null)
			{
				return "Can't find one of the heroes.\n" + text;
			}
			if (!hero2.IsActive || hero2.PartyBelongedTo == null)
			{
				return "Capturer hero is not active!";
			}
			if (!hero.IsActive || hero.IsHumanPlayerCharacter || (hero.Occupation != Occupation.Lord && hero.Occupation != Occupation.Wanderer))
			{
				return "Hero can't be taken as a prisoner!";
			}
			if (!FactionManager.IsAtWarAgainstFaction(hero.MapFaction, hero2.MapFaction))
			{
				return "Factions are not at war!";
			}
			if (hero.PartyBelongedTo != null)
			{
				if (hero.PartyBelongedTo.MapEvent != null)
				{
					return "prisoners party shouldn't be in a map event.";
				}
				if (hero.PartyBelongedTo.LeaderHero == hero)
				{
					DestroyPartyAction.Apply(null, hero.PartyBelongedTo);
				}
				else
				{
					hero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
				}
			}
			if (hero.IsPrisoner)
			{
				EndCaptivityAction.ApplyByEscape(hero, null);
			}
			if (hero.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForCharacterOnly(hero);
			}
			if (hero2.IsHumanPlayerCharacter)
			{
				hero.SetHasMet();
			}
			TakePrisonerAction.Apply(hero2.PartyBelongedTo.Party, hero);
			return "Success";
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00018148 File Offset: 0x00016348
		[CommandLineFunctionality.CommandLineArgumentFunction("add_random_prisoner_hero", "campaign")]
		public static string AddRandomPrisonerHeroCheat(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.add_random_prisoner_hero\".";
			}
			if (!Hero.MainHero.IsPartyLeader)
			{
				return string.Format("{0} is not a party leader.", Hero.MainHero.Name);
			}
			Hero randomElementWithPredicate = Hero.AllAliveHeroes.GetRandomElementWithPredicate((Hero x) => !x.CharacterObject.IsPlayerCharacter && x.IsActive && x.PartyBelongedTo == null && !x.IsPrisoner && x.CharacterObject.Occupation == Occupation.Lord);
			if (randomElementWithPredicate == null)
			{
				return "There is not any available heroes right now.";
			}
			if (randomElementWithPredicate.CurrentSettlement != null)
			{
				LeaveSettlementAction.ApplyForCharacterOnly(randomElementWithPredicate);
			}
			MobileParty partyBelongedTo = randomElementWithPredicate.PartyBelongedTo;
			bool flag = ((partyBelongedTo != null) ? partyBelongedTo.LeaderHero : null) == randomElementWithPredicate;
			MobileParty partyBelongedTo2 = randomElementWithPredicate.PartyBelongedTo;
			TakePrisonerAction.Apply(PartyBase.MainParty, randomElementWithPredicate);
			randomElementWithPredicate.SetHasMet();
			if (flag)
			{
				DisbandPartyAction.StartDisband(partyBelongedTo2);
				partyBelongedTo2.IsDisbanding = true;
			}
			return "Success";
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00018218 File Offset: 0x00016418
		[CommandLineFunctionality.CommandLineArgumentFunction("control_party_ai_by_cheats", "campaign")]
		public static string ControlPartyAIByCheats(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string result = "Format is \"campaign.control_party_ai_by_cheats [HeroName] | [0|1] \".";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings) || separatedNames.Count == 1)
			{
				return result;
			}
			if (separatedNames.Count != 2)
			{
				return result;
			}
			if (separatedNames[1] != "0" && separatedNames[1] != "1")
			{
				return result;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			bool enable = separatedNames[1] == "1";
			string result2;
			CampaignCheats.ControlPartyAIByCheatsInternal(hero, enable, out result2);
			return result2;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000182C0 File Offset: 0x000164C0
		[CommandLineFunctionality.CommandLineArgumentFunction("ai_siege_settlement", "campaign")]
		public static string AISiegeSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.ai_siege_settlement [HeroName] | [SettlementName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[1]);
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (settlement == null)
			{
				return "Settlement is not found\n" + text;
			}
			if (!settlement.IsFortification)
			{
				return "Settlement is not a fortification (Town or Castle)";
			}
			if (hero.MapFaction == settlement.MapFaction)
			{
				return string.Format("Hero Faction: {0} and Settlement Faction: {1} are the same", hero.MapFaction.Name, settlement.MapFaction.Name);
			}
			if (!FactionManager.IsAtWarAgainstFaction(hero.MapFaction, settlement.MapFaction))
			{
				return string.Format("Hero Faction: {0} and Settlement Faction: {1} are not at war, you can use \"campaign.declare_war\" cheat", hero.MapFaction.Name, settlement.MapFaction.Name);
			}
			string text2;
			if (CampaignCheats.ControlPartyAIByCheatsInternal(hero, true, out text2))
			{
				SetPartyAiAction.GetActionForBesiegingSettlement(hero.PartyBelongedTo, settlement);
				return text2 + "\nParty AI can be enabled again by using \"campaign.control_party_ai_by_cheats\"\nSuccess";
			}
			return text2;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000183E8 File Offset: 0x000165E8
		[CommandLineFunctionality.CommandLineArgumentFunction("ai_raid_village", "campaign")]
		public static string AIRaidVillage(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.ai_raid_village [HeroName] | [VillageName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[1]);
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (settlement == null)
			{
				return "Settlement is not found\n" + text;
			}
			if (!settlement.IsVillage)
			{
				return "Settlement is not a village.";
			}
			if (hero.MapFaction == settlement.MapFaction)
			{
				return string.Format("Hero Faction: {0} and Village Faction: {1} are the same", hero.MapFaction.Name, settlement.MapFaction.Name);
			}
			if (!FactionManager.IsAtWarAgainstFaction(hero.MapFaction, settlement.MapFaction))
			{
				return string.Format("Hero Faction: {0} and Village Faction: {1} are not at war, you can use \"campaign.declare_war\" cheat", hero.MapFaction.Name, settlement.MapFaction.Name);
			}
			if (settlement.IsUnderRaid)
			{
				return "Village is already under raid.";
			}
			string text2;
			if (CampaignCheats.ControlPartyAIByCheatsInternal(hero, true, out text2))
			{
				SetPartyAiAction.GetActionForRaidingSettlement(hero.PartyBelongedTo, settlement);
				return text2 + "\nParty AI can be enabled again by using \"campaign.control_party_ai_by_cheats\"\nSuccess";
			}
			return text2;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00018520 File Offset: 0x00016720
		[CommandLineFunctionality.CommandLineArgumentFunction("ai_attack_party", "campaign")]
		public static string AIAttackParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.ai_attack_party [AttackerHeroName] | [HeroName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			Hero hero2 = CampaignCheats.GetHero(separatedNames[1]);
			if (hero == null || hero2 == null)
			{
				return "Hero is not found\n" + text;
			}
			if (hero2.PartyBelongedTo == null)
			{
				return "Second hero is not in a party";
			}
			if (hero.MapFaction == hero2.MapFaction)
			{
				return string.Format("Attacker Hero Faction: {0} and Other Hero Faction: {1} are the same", hero.MapFaction.Name, hero2.MapFaction.Name);
			}
			if (!FactionManager.IsAtWarAgainstFaction(hero.MapFaction, hero2.MapFaction))
			{
				return string.Format("Attacker Hero Faction: {0} and Other Hero Faction: {1} are not at war, you can use \"campaign.declare_war\" cheat", hero.MapFaction.Name, hero2.MapFaction.Name);
			}
			string text2;
			if (CampaignCheats.ControlPartyAIByCheatsInternal(hero, true, out text2))
			{
				SetPartyAiAction.GetActionForEngagingParty(hero.PartyBelongedTo, hero2.PartyBelongedTo);
				return text2 + "\nParty AI can be enabled again by using \"campaign.control_party_ai_by_cheats\"\nSuccess";
			}
			return text2;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00018640 File Offset: 0x00016840
		[CommandLineFunctionality.CommandLineArgumentFunction("ai_defend_settlement", "campaign")]
		public static string AIDefendSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.ai_defend_settlement [HeroName] | [SettlementName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[1]);
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (settlement == null)
			{
				return "Settlement is not found\n" + text;
			}
			if (FactionManager.IsAtWarAgainstFaction(hero.MapFaction, settlement.MapFaction))
			{
				return string.Format("Hero Faction: {0} and Settlement Faction: {1} are at war.", hero.MapFaction.Name, settlement.MapFaction.Name);
			}
			if (!settlement.IsUnderRaid && !settlement.IsUnderSiege)
			{
				return "Settlement is not under siege nor raid";
			}
			string text2;
			if (CampaignCheats.ControlPartyAIByCheatsInternal(hero, true, out text2))
			{
				SetPartyAiAction.GetActionForDefendingSettlement(hero.PartyBelongedTo, settlement);
				return text2 + "\nParty AI can be enabled again by using \"campaign.control_party_ai_by_cheats\"\nSuccess";
			}
			return text2;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00018740 File Offset: 0x00016940
		[CommandLineFunctionality.CommandLineArgumentFunction("ai_goto_settlement", "campaign")]
		public static string AIGotoSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.ai_goto_settlement [HeroName] | [SettlementName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings) || CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 2)
			{
				return text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			Settlement settlement = CampaignCheats.GetSettlement(separatedNames[1]);
			if (hero == null)
			{
				return "Hero is not found\n" + text;
			}
			if (settlement == null)
			{
				return "Settlement is not found\n" + text;
			}
			if (FactionManager.IsAtWarAgainstFaction(hero.MapFaction, settlement.MapFaction))
			{
				return string.Format("Hero Faction: {0} and Settlement Faction: {1} are at war", hero.MapFaction.Name, settlement.MapFaction.Name);
			}
			string text2;
			if (CampaignCheats.ControlPartyAIByCheatsInternal(hero, true, out text2))
			{
				SetPartyAiAction.GetActionForVisitingSettlement(hero.PartyBelongedTo, settlement);
				return text2 + "\nParty AI can be enabled again by using \"campaign.control_party_ai_by_cheats\"\nSuccess";
			}
			return text2;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001882C File Offset: 0x00016A2C
		public static List<string> GetSeparatedNames(List<string> strings, string separator)
		{
			List<string> list = new List<string>();
			List<int> list2 = new List<int>(strings.Count);
			for (int i = 0; i < strings.Count; i++)
			{
				if (strings[i] == separator)
				{
					list2.Add(i);
				}
			}
			list2.Add(strings.Count);
			int num = 0;
			for (int j = 0; j < list2.Count; j++)
			{
				int num2 = list2[j];
				string item = CampaignCheats.ConcatenateString(strings.GetRange(num, num2 - num));
				num = num2 + 1;
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000188C0 File Offset: 0x00016AC0
		private static bool ControlPartyAIByCheatsInternal(Hero hero, bool enable, out string resultDescription)
		{
			if (hero == null)
			{
				resultDescription = "Hero is not found";
				return false;
			}
			if (hero == Hero.MainHero)
			{
				resultDescription = "Hero cannot be MainHero";
				return false;
			}
			MobileParty partyBelongedTo = hero.PartyBelongedTo;
			if (partyBelongedTo == null)
			{
				resultDescription = "Hero is not part of a party";
				return false;
			}
			if (partyBelongedTo.Army != null && partyBelongedTo.Army.LeaderParty != partyBelongedTo)
			{
				resultDescription = "Party AI cannot be changed while party is part of an army and not the leader of the army";
				return false;
			}
			partyBelongedTo.Ai.SetDoNotMakeNewDecisions(enable);
			resultDescription = (enable ? string.Format("Party AI of {0} is controlled by cheats", hero) : string.Format("Party AI of {0} isn't controlled by cheats", hero));
			return true;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00018948 File Offset: 0x00016B48
		[CommandLineFunctionality.CommandLineArgumentFunction("clear_settlement_defense", "campaign")]
		public static string ClearSettlementDefense(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.clear_settlement_defense [SettlementName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Settlement settlement = CampaignCheats.GetSettlement(CampaignCheats.ConcatenateString(strings.GetRange(0, strings.Count)));
			if (settlement == null)
			{
				return "Settlement is not found\n" + text;
			}
			settlement.Militia = 0f;
			MobileParty mobileParty = settlement.IsFortification ? settlement.Town.GarrisonParty : null;
			if (mobileParty != null)
			{
				DestroyPartyAction.Apply(null, mobileParty);
			}
			return "Success";
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000189D8 File Offset: 0x00016BD8
		[CommandLineFunctionality.CommandLineArgumentFunction("print_party_prisoners", "campaign")]
		public static string PrintPartyPrisoners(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.print_party_prisoners [PartyName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = CampaignCheats.ConcatenateString(strings);
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				if (string.Equals(text2.Replace(" ", ""), mobileParty.Name.ToString().Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					string text3 = "";
					foreach (TroopRosterElement troopRosterElement in mobileParty.PrisonRoster.GetTroopRoster())
					{
						text3 = string.Concat(new object[]
						{
							text3,
							troopRosterElement.Character.Name,
							" count: ",
							mobileParty.PrisonRoster.GetTroopCount(troopRosterElement.Character),
							"\n"
						});
					}
					if (string.IsNullOrEmpty(text3))
					{
						return "There is not any prisoners in the party right now.";
					}
					return text3;
				}
			}
			return "Party is not found: " + text2 + "\n" + text;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00018B50 File Offset: 0x00016D50
		[CommandLineFunctionality.CommandLineArgumentFunction("add_prisoners_xp", "campaign")]
		public static string AddPrisonersXp(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_prisoners_xp [Amount]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			if (!int.TryParse(strings[0], out num) || num < 1)
			{
				return "Please enter a positive number\n" + text;
			}
			for (int i = 0; i < MobileParty.MainParty.PrisonRoster.Count; i++)
			{
				MobileParty.MainParty.PrisonRoster.SetElementXp(i, MobileParty.MainParty.PrisonRoster.GetElementXp(i) + num);
				InformationManager.DisplayMessage(new InformationMessage(string.Concat(new object[]
				{
					"[DEBUG] ",
					num,
					" xp given to ",
					MobileParty.MainParty.PrisonRoster.GetElementCopyAtIndex(i).Character.Name
				})));
			}
			return "Success";
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00018C3C File Offset: 0x00016E3C
		[CommandLineFunctionality.CommandLineArgumentFunction("set_settlement_variable", "campaign")]
		public static string SetSettlementVariable(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_settlement_variable [SettlementName/SettlementID] | [VariableName] | [Value]\". Available variables:\nProsperity\nSecurity\nFood\nLoyalty\nMilitia\nHearth";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckParameters(strings, 2) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count != 3)
			{
				return text;
			}
			string text2 = separatedNames[1].ToLower();
			string s = separatedNames[2].ToLower();
			string settlementName = separatedNames[0];
			Settlement settlement = Settlement.FindFirst((Settlement x) => string.Compare(x.Name.ToString().Replace(" ", ""), settlementName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase) == 0);
			if (settlement == null)
			{
				return "Settlement is not found: " + settlementName + "\n" + text;
			}
			bool flag = false;
			if (settlement.IsVillage)
			{
				if (text2.Equals("hearth") || text2.Equals("militia"))
				{
					flag = true;
				}
			}
			else if (text2.Equals("prosperity") || text2.Equals("militia") || text2.Equals("security") || text2.Equals("loyalty") || text2.Equals("food"))
			{
				flag = true;
			}
			if (!flag)
			{
				return "Settlement don't have variable: " + text2;
			}
			float num = -333f;
			if (!float.TryParse(s, out num))
			{
				return "Please enter a number\n" + text;
			}
			string a = text2.Replace(" ", "");
			if (!(a == "prosperity"))
			{
				if (!(a == "militia"))
				{
					if (!(a == "hearth"))
					{
						if (!(a == "security"))
						{
							if (!(a == "loyalty"))
							{
								if (!(a == "food"))
								{
									return "Invalid variable: " + text2 + "\n" + text;
								}
								settlement.Town.FoodStocks = num;
							}
							else
							{
								settlement.Town.Loyalty = num;
							}
						}
						else
						{
							settlement.Town.Security = num;
						}
					}
					else
					{
						settlement.Village.Hearth = num;
					}
				}
				else
				{
					settlement.Militia = num;
				}
			}
			else
			{
				settlement.Town.Prosperity = num;
			}
			return "Success";
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00018E70 File Offset: 0x00017070
		[CommandLineFunctionality.CommandLineArgumentFunction("set_hero_trait", "campaign")]
		public static string SetHeroTrait(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.set_hero_trait [HeroName] | [Trait]  | [Value]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count < 3)
			{
				return text;
			}
			int num;
			if (!int.TryParse(separatedNames[2], out num))
			{
				return "Please enter a number\n" + text;
			}
			Hero hero = CampaignCheats.GetHero(separatedNames[0]);
			if (hero != null)
			{
				int num2;
				if (int.TryParse(separatedNames[2], out num2))
				{
					string text2 = separatedNames[1];
					foreach (TraitObject traitObject in TraitObject.All)
					{
						if (traitObject.Name.ToString().Replace(" ", "").Equals(text2.Replace(" ", ""), StringComparison.InvariantCultureIgnoreCase) || traitObject.StringId == text2.Replace(" ", ""))
						{
							int traitLevel = hero.GetTraitLevel(traitObject);
							if (num2 >= traitObject.MinValue && num2 <= traitObject.MaxValue)
							{
								hero.SetTraitLevel(traitObject, num2);
								if (hero == Hero.MainHero)
								{
									Campaign.Current.PlayerTraitDeveloper.UpdateTraitXPAccordingToTraitLevels();
									CampaignEventDispatcher.Instance.OnPlayerTraitChanged(traitObject, traitLevel);
								}
								Campaign.Current.PlayerTraitDeveloper.UpdateTraitXPAccordingToTraitLevels();
								CampaignEventDispatcher.Instance.OnPlayerTraitChanged(traitObject, traitLevel);
								return string.Format("{0} 's {1} trait has been set to {2}.", separatedNames[0], traitObject.Name, num2);
							}
							return string.Format("Number must be between {0} and {1}.", traitObject.MinValue, traitObject.MaxValue);
						}
					}
				}
				return "Trait not found.\n" + text;
			}
			return "Hero: " + separatedNames[0] + " not found.\n" + text;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00019084 File Offset: 0x00017284
		[CommandLineFunctionality.CommandLineArgumentFunction("print_hero_traits", "campaign")]
		public static string PrintHeroTraits(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return "Format is \"campaign.print_hero_traits [HeroName]\".";
			}
			string text = CampaignCheats.ConcatenateString(strings);
			Hero hero = CampaignCheats.GetHero(text);
			if (hero != null)
			{
				string text2 = null;
				foreach (TraitObject traitObject in TraitObject.All)
				{
					text2 = string.Concat(new object[]
					{
						text2,
						traitObject.Name,
						" ",
						hero.GetTraitLevel(traitObject).ToString(),
						"\n"
					});
				}
				return text2;
			}
			return "Hero: " + text + " not found.\nFormat is \"campaign.print_hero_traits [HeroName]\".";
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00019158 File Offset: 0x00017358
		[CommandLineFunctionality.CommandLineArgumentFunction("remove_militas_from_settlement", "campaign")]
		public static string RemoveMilitiasFromSettlement(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.remove_militas_from_settlement [SettlementName]\".";
			}
			string concatenated = CampaignCheats.ConcatenateString(strings);
			Settlement settlement = Settlement.FindFirst((Settlement x) => string.Compare(x.Name.ToString().Replace(" ", ""), concatenated, StringComparison.OrdinalIgnoreCase) == 0);
			if (settlement == null)
			{
				return "Settlement is not found: " + concatenated;
			}
			if (settlement.Party.MapEvent != null)
			{
				return "Settlement, " + concatenated + " is in a MapEvent, try later to remove them";
			}
			List<MobileParty> list = new List<MobileParty>();
			IEnumerable<MobileParty> all = MobileParty.All;
			Func<MobileParty, bool> <>9__1;
			Func<MobileParty, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = ((MobileParty x) => x.IsMilitia && x.CurrentSettlement == settlement));
			}
			foreach (MobileParty mobileParty in all.Where(predicate))
			{
				if (mobileParty.MapEvent != null)
				{
					return "Milita in " + concatenated + " are in a MapEvent, try later to remove them";
				}
				list.Add(mobileParty);
			}
			foreach (MobileParty mobileParty2 in list)
			{
				mobileParty2.RemoveParty();
			}
			return "Success";
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000192C8 File Offset: 0x000174C8
		[CommandLineFunctionality.CommandLineArgumentFunction("cancel_quest", "campaign")]
		public static string CancelQuestCheat(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.cancel_quest [quest name]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			string text2 = "";
			for (int i = 0; i < strings.Count; i++)
			{
				text2 += strings[i];
				if (i + 1 != strings.Count)
				{
					text2 += " ";
				}
			}
			if (text2.IsEmpty<char>())
			{
				return text;
			}
			QuestBase questBase = null;
			int num = 0;
			foreach (QuestBase questBase2 in Campaign.Current.QuestManager.Quests)
			{
				if (text2.Replace(" ", "").Equals(questBase2.Title.ToString().Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					num++;
					if (num == 1)
					{
						questBase = questBase2;
					}
				}
			}
			if (questBase == null)
			{
				return "Quest not found.\n" + text;
			}
			if (num > 1)
			{
				return "There are more than one quest with the name: " + text2;
			}
			questBase.CompleteQuestWithCancel(new TextObject("{=!}Quest is canceled by cheat.", null));
			return "Success";
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00019404 File Offset: 0x00017604
		[CommandLineFunctionality.CommandLineArgumentFunction("kick_companion", "campaign")]
		public static string KickAllCompanionsFromParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.kick_companion [CompanionName] or [all](kicks all companions) or [noargument](kicks first companion if any) \".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			IEnumerable<TroopRosterElement> enumerable = from h in MobileParty.MainParty.MemberRoster.GetTroopRoster()
			where h.Character != null && h.Character.IsHero && h.Character.HeroObject.IsWanderer
			select h;
			if (enumerable.IsEmpty<TroopRosterElement>())
			{
				return "There are no companions in your party.";
			}
			if (strings.IsEmpty<string>())
			{
				RemoveCompanionAction.ApplyByFire(Clan.PlayerClan, enumerable.First<TroopRosterElement>().Character.HeroObject);
				return "Success";
			}
			if (strings[0].ToLower() == "all")
			{
				foreach (TroopRosterElement troopRosterElement in enumerable)
				{
					RemoveCompanionAction.ApplyByFire(Clan.PlayerClan, troopRosterElement.Character.HeroObject);
				}
				return "Success";
			}
			foreach (TroopRosterElement troopRosterElement2 in enumerable)
			{
				if (troopRosterElement2.Character.Name.ToString().ToLower().Replace(" ", "").Contains(strings[0].ToLower().Replace(" ", "")))
				{
					RemoveCompanionAction.ApplyByFire(Clan.PlayerClan, troopRosterElement2.Character.HeroObject);
					return "Success";
				}
			}
			return "No companion named: " + strings[0] + " is found.\n" + text;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000195B8 File Offset: 0x000177B8
		[CommandLineFunctionality.CommandLineArgumentFunction("add_money_to_main_party", "campaign")]
		public static string AddMoneyToMainParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_money_to_main_party [Amount]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num;
			if (int.TryParse(strings[0], out num) && num > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, num, false);
				return "Main hero gained " + num + " gold.";
			}
			return "Please enter a positive number\n" + text;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00019634 File Offset: 0x00017834
		[CommandLineFunctionality.CommandLineArgumentFunction("add_troops_xp", "campaign")]
		public static string AddTroopsXp(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_troops_xp [Amount]\".";
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			if (!int.TryParse(strings[0], out num) || num < 1)
			{
				return "Please enter a positive number\n" + text;
			}
			for (int i = 0; i < MobileParty.MainParty.MemberRoster.Count; i++)
			{
				MobileParty.MainParty.MemberRoster.SetElementXp(i, MobileParty.MainParty.MemberRoster.GetElementXp(i) + num);
				InformationManager.DisplayMessage(new InformationMessage(string.Concat(new object[]
				{
					"[DEBUG] ",
					num,
					" xp given to ",
					MobileParty.MainParty.MemberRoster.GetElementCopyAtIndex(i).Character.Name
				})));
			}
			return "Success";
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00019720 File Offset: 0x00017920
		[CommandLineFunctionality.CommandLineArgumentFunction("toggle_information_restrictions", "campaign")]
		public static string ToggleInformationRestrictions(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.toggle_information_restrictions\".";
			}
			DefaultInformationRestrictionModel defaultInformationRestrictionModel = Campaign.Current.Models.InformationRestrictionModel as DefaultInformationRestrictionModel;
			if (defaultInformationRestrictionModel == null)
			{
				return "DefaultInformationRestrictionModel is missing.";
			}
			defaultInformationRestrictionModel.IsDisabledByCheat = !defaultInformationRestrictionModel.IsDisabledByCheat;
			return "Information restrictions are " + (defaultInformationRestrictionModel.IsDisabledByCheat ? "disabled" : "enabled") + ".";
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000197A0 File Offset: 0x000179A0
		[CommandLineFunctionality.CommandLineArgumentFunction("set_campaign_speed_multiplier", "campaign")]
		public static string SetCampaignSpeed(List<string> strings)
		{
			string result = "Format is \"campaign.set_campaign_speed_multiplier  [positive speedUp multiplier]\".";
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return result;
			}
			float num;
			if (!float.TryParse(strings[0], out num) || num <= 0f)
			{
				return result;
			}
			if (num <= 15f)
			{
				Campaign.Current.SpeedUpMultiplier = num;
				return "Success";
			}
			Campaign.Current.SpeedUpMultiplier = 15f;
			return "Campaign speed is set to " + 15f + ". which is the maximum value for speed up multiplier!";
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00019838 File Offset: 0x00017A38
		[CommandLineFunctionality.CommandLineArgumentFunction("print_gameplay_statistics", "campaign")]
		public static string PrintGameplayStatistics(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"statistics.print_gameplay_statistics\".";
			}
			IStatisticsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IStatisticsCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return "Can not find IStatistics Campaign Behavior!";
			}
			string text = "";
			text += "---------------------------GENERAL---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Played Time in Campaign Time(Days): ",
				campaignBehavior.GetTotalTimePlayed().ToDays,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Played Time in Real Time: ",
				campaignBehavior.GetTotalTimePlayedInSeconds(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of children born: ",
				campaignBehavior.GetNumberOfChildrenBorn(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total influence earned: ",
				campaignBehavior.GetTotalInfluenceEarned(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of issues solved: ",
				campaignBehavior.GetNumberOfIssuesSolved(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of tournaments won: ",
				campaignBehavior.GetNumberOfTournamentWins(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Best tournament rank: ",
				campaignBehavior.GetHighestTournamentRank(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of prisoners recruited: ",
				campaignBehavior.GetNumberOfPrisonersRecruited(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of troops recruited: ",
				campaignBehavior.GetNumberOfTroopsRecruited(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of enemy clans defected: ",
				campaignBehavior.GetNumberOfClansDefected(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total crime rating gained: ",
				campaignBehavior.GetTotalCrimeRatingGained(),
				"\n"
			});
			text += "---------------------------BATTLE---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Battles Won / Lost: ",
				campaignBehavior.GetNumberOfBattlesWon(),
				" / ",
				campaignBehavior.GetNumberOfBattlesLost(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Largest battle won as the leader: ",
				campaignBehavior.GetLargestBattleWonAsLeader(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Largest army formed by the player: ",
				campaignBehavior.GetLargestArmyFormedByPlayer(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of enemy clans destroyed: ",
				campaignBehavior.GetNumberOfEnemyClansDestroyed(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Heroes killed in battle: ",
				campaignBehavior.GetNumberOfHeroesKilledInBattle(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Troops killed or knocked out in person: ",
				campaignBehavior.GetNumberOfTroopsKnockedOrKilledByPlayer(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Troops killed or knocked out by player party: ",
				campaignBehavior.GetNumberOfTroopsKnockedOrKilledAsParty(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of hero prisoners taken: ",
				campaignBehavior.GetNumberOfHeroPrisonersTaken(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of troop prisoners taken: ",
				campaignBehavior.GetNumberOfTroopPrisonersTaken(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of captured towns: ",
				campaignBehavior.GetNumberOfTownsCaptured(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of captured castles: ",
				campaignBehavior.GetNumberOfCastlesCaptured(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of cleared hideouts: ",
				campaignBehavior.GetNumberOfHideoutsCleared(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of raided villages: ",
				campaignBehavior.GetNumberOfVillagesRaided(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of days spent as prisoner: ",
				campaignBehavior.GetTimeSpentAsPrisoner().ToDays,
				"\n"
			});
			text += "---------------------------FINANCES---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned: ",
				campaignBehavior.GetTotalDenarsEarned(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from caravans: ",
				campaignBehavior.GetDenarsEarnedFromCaravans(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from workshops: ",
				campaignBehavior.GetDenarsEarnedFromWorkshops(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from ransoms: ",
				campaignBehavior.GetDenarsEarnedFromRansoms(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from taxes: ",
				campaignBehavior.GetDenarsEarnedFromTaxes(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars earned from tributes: ",
				campaignBehavior.GetDenarsEarnedFromTributes(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Total denars paid in tributes: ",
				campaignBehavior.GetDenarsPaidAsTributes(),
				"\n"
			});
			text += "---------------------------CRAFTING---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Number of weapons crafted: ",
				campaignBehavior.GetNumberOfWeaponsCrafted(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Most expensive weapon crafted: ",
				campaignBehavior.GetMostExpensiveItemCrafted().Item1,
				" - ",
				campaignBehavior.GetMostExpensiveItemCrafted().Item2,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Numbere of crafting parts unlocked: ",
				campaignBehavior.GetNumberOfCraftingPartsUnlocked(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Number of crafting orders completed: ",
				campaignBehavior.GetNumberOfCraftingOrdersCompleted(),
				"\n"
			});
			text += "---------------------------COMPANIONS---------------------------\n";
			text = string.Concat(new object[]
			{
				text,
				"Number of hired companions: ",
				campaignBehavior.GetNumberOfCompanionsHired(),
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				"Companion with most issues solved: ",
				campaignBehavior.GetCompanionWithMostIssuesSolved().Item1,
				" - ",
				campaignBehavior.GetCompanionWithMostIssuesSolved().Item2,
				"\n"
			});
			return string.Concat(new object[]
			{
				text,
				"Companion with most kills: ",
				campaignBehavior.GetCompanionWithMostKills().Item1,
				" - ",
				campaignBehavior.GetCompanionWithMostKills().Item2,
				"\n"
			});
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0001A038 File Offset: 0x00018238
		[CommandLineFunctionality.CommandLineArgumentFunction("set_armies_and_parties_visible", "campaign")]
		public static string SetAllArmiesAndPartiesVisible(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.set_armies_and_parties_visible [1/0]\".";
			}
			Campaign.Current.TrueSight = (strings[0] == "1");
			return "Success";
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0001A090 File Offset: 0x00018290
		[CommandLineFunctionality.CommandLineArgumentFunction("print_strength_of_lord_parties", "campaign")]
		public static string PrintStrengthOfLordParties(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MobileParty mobileParty in MobileParty.AllLordParties)
			{
				stringBuilder.AppendLine(mobileParty.Name + " strength: " + mobileParty.Party.TotalStrength);
			}
			stringBuilder.AppendLine("Success");
			return stringBuilder.ToString();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0001A12C File Offset: 0x0001832C
		[CommandLineFunctionality.CommandLineArgumentFunction("print_strength_of_factions", "campaign")]
		public static string PrintStrengthOfFactions(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Clan clan in Clan.All)
			{
				stringBuilder.AppendLine(clan.Name + " strength: " + clan.TotalStrength);
			}
			stringBuilder.AppendLine("Success");
			return stringBuilder.ToString();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001A1C4 File Offset: 0x000183C4
		[CommandLineFunctionality.CommandLineArgumentFunction("print_influence_change_of_clan", "campaign")]
		public static string PrintInfluenceChangeOfClan(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string result = "Format is \"campaign.print_influence_change_of_clan [ClanName]\".";
			if (CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return result;
			}
			string clanName = CampaignCheats.ConcatenateString(strings);
			StringBuilder stringBuilder = new StringBuilder();
			Clan clan = Campaign.Current.Clans.FirstOrDefault((Clan x) => string.Equals(x.Name.ToString().Replace(" ", ""), clanName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase) && !x.IsEliminated);
			if (clan == null)
			{
				return result;
			}
			if (clan != null)
			{
				foreach (ValueTuple<string, float> valueTuple in Campaign.Current.Models.ClanPoliticsModel.CalculateInfluenceChange(clan, true).GetLines())
				{
					stringBuilder.AppendLine(valueTuple.Item1 + ": " + valueTuple.Item2);
				}
			}
			stringBuilder.AppendLine("Success");
			return stringBuilder.ToString();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0001A2C8 File Offset: 0x000184C8
		[CommandLineFunctionality.CommandLineArgumentFunction("add_supporters_for_main_hero", "campaign")]
		public static string AddSupportersForMainHero(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string str = "Usage : campaign.add_supporters_for_main_hero [Number]";
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 1))
			{
				return "" + "Usage : campaign.add_supporters_for_main_hero [Number]" + "\n";
			}
			string str2 = "";
			int num;
			if (int.TryParse(strings[0], out num) && num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					Hero randomElementWithPredicate = Hero.AllAliveHeroes.GetRandomElementWithPredicate((Hero x) => !x.IsChild && x.SupporterOf != Clan.PlayerClan);
					if (randomElementWithPredicate != null)
					{
						randomElementWithPredicate.SupporterOf = Clan.PlayerClan;
						str2 = str2 + "supporter added: " + randomElementWithPredicate.Name.ToString() + "\n";
					}
				}
				return str2 + "\nSuccess";
			}
			return "Please enter a positive number\n" + str;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001A3B0 File Offset: 0x000185B0
		[CommandLineFunctionality.CommandLineArgumentFunction("show_hideouts", "campaign")]
		public static string ShowHideouts(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			int num;
			if (!CampaignCheats.CheckParameters(strings, 1) || CampaignCheats.CheckHelp(strings) || !int.TryParse(strings[0], out num) || (num != 1 && num != 2))
			{
				return "Format is \"campaign.show_hideouts [1/2]\n 1: Show infested hideouts\n2: Show all hideouts\".";
			}
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsHideout && (num != 1 || settlement.Hideout.IsInfested))
				{
					Hideout hideout = settlement.Hideout;
					hideout.IsSpotted = true;
					hideout.Owner.Settlement.IsVisible = true;
				}
			}
			return ((num == 1) ? "Infested" : "All") + " hideouts is visible now.";
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0001A48C File Offset: 0x0001868C
		[CommandLineFunctionality.CommandLineArgumentFunction("hide_hideouts", "campaign")]
		public static string HideHideouts(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsHideout)
				{
					Hideout hideout = settlement.Hideout;
					hideout.IsSpotted = false;
					hideout.Owner.Settlement.IsVisible = false;
				}
			}
			return "All hideouts should be invisible now.";
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0001A514 File Offset: 0x00018714
		[CommandLineFunctionality.CommandLineArgumentFunction("unlock_all_crafting_pieces", "campaign")]
		public static string UnlockCraftingPieces(List<string> strings)
		{
			string result = "";
			if (!CampaignCheats.CheckCheatUsage(ref result))
			{
				return result;
			}
			if (!CampaignCheats.CheckParameters(strings, 0) || CampaignCheats.CheckHelp(strings))
			{
				return "Format is \"campaign.unlock_all_crafting_pieces\".";
			}
			CraftingCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<CraftingCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return "Can not find Crafting Campaign Behavior!";
			}
			Type typeFromHandle = typeof(CraftingCampaignBehavior);
			FieldInfo field = typeFromHandle.GetField("_openedPartsDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo field2 = typeFromHandle.GetField("_openNewPartXpDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
			Dictionary<CraftingTemplate, List<CraftingPiece>> dictionary = (Dictionary<CraftingTemplate, List<CraftingPiece>>)field.GetValue(campaignBehavior);
			Dictionary<CraftingTemplate, float> dictionary2 = (Dictionary<CraftingTemplate, float>)field2.GetValue(campaignBehavior);
			MethodInfo method = typeFromHandle.GetMethod("OpenPart", BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (CraftingTemplate craftingTemplate in CraftingTemplate.All)
			{
				if (!dictionary.ContainsKey(craftingTemplate))
				{
					dictionary.Add(craftingTemplate, new List<CraftingPiece>());
				}
				if (!dictionary2.ContainsKey(craftingTemplate))
				{
					dictionary2.Add(craftingTemplate, 0f);
				}
				foreach (CraftingPiece craftingPiece in craftingTemplate.Pieces)
				{
					object[] parameters = new object[]
					{
						craftingPiece,
						craftingTemplate,
						false
					};
					method.Invoke(campaignBehavior, parameters);
				}
			}
			field.SetValue(campaignBehavior, dictionary);
			field2.SetValue(campaignBehavior, dictionary2);
			return "Success";
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0001A6A0 File Offset: 0x000188A0
		[CommandLineFunctionality.CommandLineArgumentFunction("rebellion_enabled", "campaign")]
		public static string SetRebellionEnabled(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is campaign.rebellion_enabled [1/0]\".";
			if (CampaignCheats.CheckHelp(strings) || !CampaignCheats.CheckParameters(strings, 1))
			{
				return text;
			}
			if (!(strings[0] == "0") && !(strings[0] == "1"))
			{
				return "Wrong input.\n" + text;
			}
			RebellionsCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<RebellionsCampaignBehavior>();
			if (campaignBehavior != null)
			{
				FieldInfo field = typeof(RebellionsCampaignBehavior).GetField("_rebellionEnabled", BindingFlags.Instance | BindingFlags.NonPublic);
				field.SetValue(campaignBehavior, strings[0] == "1");
				return "Rebellion is" + (((bool)field.GetValue(campaignBehavior)) ? " enabled" : " disabled");
			}
			return "Rebellions Campaign behavior not found.";
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0001A778 File Offset: 0x00018978
		[CommandLineFunctionality.CommandLineArgumentFunction("add_troops", "campaign")]
		public static string AddTroopsToParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			if (CampaignCheats.CheckParameters(strings, 0))
			{
				return "Write \"campaign.add_troops help\" for help";
			}
			string text = "Usage : \"campaign.add_troops [TroopId] | [Number] | [PartyName]\". Party name is optional.";
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (CampaignCheats.CheckHelp(strings) || separatedNames.Count < 2)
			{
				string text2 = "";
				text2 += text;
				text2 += "\n";
				text2 += "\n";
				text2 += "Available troops";
				text2 += "\n";
				text2 += "==============================";
				text2 += "\n";
				foreach (CharacterObject characterObject in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
				{
					if (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Gangster)
					{
						text2 = string.Concat(new object[]
						{
							text2,
							"Id: ",
							characterObject.StringId,
							" Name: ",
							characterObject.Name,
							"\n"
						});
					}
				}
				return text2;
			}
			string text3 = separatedNames[0];
			CharacterObject characterObject2 = MBObjectManager.Instance.GetObject<CharacterObject>(text3);
			if (characterObject2 == null)
			{
				foreach (CharacterObject characterObject3 in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
				{
					if ((characterObject3.Occupation == Occupation.Soldier || characterObject3.Occupation == Occupation.Gangster) && characterObject3.StringId == text3.ToLower())
					{
						characterObject2 = characterObject3;
						break;
					}
				}
			}
			if (characterObject2 == null)
			{
				return "Troop with id \"" + text3 + "\" could not be found.\n" + text;
			}
			int num;
			if (!int.TryParse(separatedNames[1], out num) || num < 1)
			{
				return "Please enter a positive number\n" + text;
			}
			MobileParty mobileParty = PartyBase.MainParty.MobileParty;
			if (separatedNames.Count == 3)
			{
				foreach (MobileParty mobileParty2 in MobileParty.All)
				{
					if (string.Equals(separatedNames[2].Replace(" ", ""), mobileParty2.Name.ToString().Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
					{
						mobileParty = mobileParty2;
						break;
					}
				}
			}
			if (mobileParty.MapEvent != null)
			{
				return "Party shouldn't be in a map event.";
			}
			typeof(DefaultPartySizeLimitModel).GetField("_addAdditionalPartySizeAsCheat", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
			mobileParty.AddElementToMemberRoster(characterObject2, num, false);
			return string.Concat(new object[]
			{
				mobileParty.Name.ToString(),
				" gained ",
				num,
				" of ",
				characterObject2.Name,
				"."
			});
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0001AA9C File Offset: 0x00018C9C
		[CommandLineFunctionality.CommandLineArgumentFunction("add_random_hero_to_party", "campaign")]
		public static string AddRandomHeroToParty(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_random_hero_to_party [PartyLeaderName]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			Hero hero = Hero.MainHero;
			string text2 = hero.Name.ToString();
			Hero randomElement = Hero.AllAliveHeroes.GetRandomElement<Hero>();
			if (!CampaignCheats.CheckParameters(strings, 0))
			{
				text2 = CampaignCheats.ConcatenateString(strings.GetRange(0, strings.Count));
				hero = CampaignCheats.GetHero(text2);
			}
			if (hero == null)
			{
				return text2 + " is not found.\n" + text;
			}
			if (!hero.IsPartyLeader)
			{
				return text2 + " is not a party leader.";
			}
			int num = 0;
			while (num < 1000 && (randomElement.PartyBelongedTo != null || randomElement.PartyBelongedToAsPrisoner != null))
			{
				randomElement = Hero.AllAliveHeroes.GetRandomElement<Hero>();
				num++;
			}
			if (randomElement.PartyBelongedTo != null || randomElement.PartyBelongedToAsPrisoner != null)
			{
				return "There is not any suitable hero right now.";
			}
			if (hero.Equals(Hero.MainHero))
			{
				typeof(DefaultPartySizeLimitModel).GetField("_addAdditionalPartySizeAsCheat", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
			}
			hero.PartyBelongedTo.AddElementToMemberRoster(randomElement.CharacterObject, 1, false);
			return string.Format("{0} is added to {1}'s party.", randomElement.Name, hero.Name);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0001ABD4 File Offset: 0x00018DD4
		[CommandLineFunctionality.CommandLineArgumentFunction("add_prisoner", "campaign")]
		public static string AddPrisoner(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}
			string text = "Format is \"campaign.add_prisoner [TroopName] | [PositiveNumber]\".";
			if (CampaignCheats.CheckHelp(strings))
			{
				return text;
			}
			int num = 1;
			string text2 = "looter";
			CharacterObject characterObject = MBObjectManager.Instance.GetObject<CharacterObject>(text2);
			bool flag = false;
			List<string> separatedNames = CampaignCheats.GetSeparatedNames(strings, "|");
			if (separatedNames.Count == 1)
			{
				if (!int.TryParse(separatedNames[0], out num))
				{
					num = 1;
					text2 = separatedNames[0].Replace(" ", "");
					characterObject = MBObjectManager.Instance.GetObject<CharacterObject>(text2);
					flag = true;
				}
			}
			else if (separatedNames.Count == 2)
			{
				if (!int.TryParse(separatedNames[1], out num))
				{
					return "Please enter a positive number\n" + text;
				}
				text2 = separatedNames[0].Replace(" ", "");
				characterObject = MBObjectManager.Instance.GetObject<CharacterObject>(text2);
				flag = true;
			}
			if (characterObject == null)
			{
				foreach (CharacterObject characterObject2 in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
				{
					if (characterObject2.Occupation == Occupation.Soldier && string.Equals(characterObject2.Name.ToString(), text2, StringComparison.OrdinalIgnoreCase))
					{
						characterObject = characterObject2;
						break;
					}
				}
			}
			if (characterObject != null)
			{
				if (num > 0)
				{
					typeof(DefaultPartySizeLimitModel).GetField("_addAdditionalPartySizeAsCheat", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);
					PartyBase.MainParty.AddPrisoner(characterObject, num);
					return string.Concat(new object[]
					{
						"Main hero gained ",
						num,
						" of ",
						characterObject.Name,
						" as prisoner."
					});
				}
				return "Please enter a positive number\n" + text;
			}
			else
			{
				if (flag)
				{
					return "Troop: " + text2 + " not found.\n" + text;
				}
				return "Wrong input.\n" + text;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001ADC0 File Offset: 0x00018FC0
		public static Hero GetHero(string heroName)
		{
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (string.Equals(hero.Name.ToString().Replace(" ", ""), heroName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					return hero;
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (string.Equals(hero2.Name.ToString(), heroName, StringComparison.OrdinalIgnoreCase))
				{
					return hero2;
				}
			}
			return null;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001AE98 File Offset: 0x00019098
		public static ItemObject GetItemObject(string itemObjectName)
		{
			foreach (ItemObject itemObject in Campaign.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (string.Equals(itemObject.Name.ToString().Replace(" ", ""), itemObjectName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					return itemObject;
				}
			}
			return null;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0001AF28 File Offset: 0x00019128
		public static Clan GetClan(string clanName)
		{
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				if (string.Equals(clan.Name.ToString(), clanName, StringComparison.OrdinalIgnoreCase))
				{
					return clan;
				}
			}
			return null;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0001AF88 File Offset: 0x00019188
		public static Hero GetClanLeader(string clanName)
		{
			foreach (Clan clan in Clan.NonBanditFactions)
			{
				if (string.Equals(clan.Name.ToString().Replace(" ", ""), clanName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					return clan.Leader;
				}
			}
			return null;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001B00C File Offset: 0x0001920C
		public static Kingdom GetKingdom(string kingdomName)
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				if (string.Equals(kingdom.Name.ToString().Replace(" ", ""), kingdomName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					return kingdom;
				}
			}
			return null;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001B090 File Offset: 0x00019290
		public static Settlement GetSettlement(string settlementName)
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (string.Equals(settlement.Name.ToString(), settlementName, StringComparison.OrdinalIgnoreCase) || string.Equals(settlement.Name.ToString().Replace(" ", ""), settlementName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					return settlement;
				}
			}
			return null;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001B128 File Offset: 0x00019328
		public static Settlement GetDefaultSettlement()
		{
			Settlement settlement2 = Hero.MainHero.HomeSettlement;
			if (settlement2 == null)
			{
				settlement2 = Kingdom.All.GetRandomElement<Kingdom>().Settlements.GetRandomElementWithPredicate((Settlement settlement) => settlement.IsTown);
			}
			return settlement2;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001B178 File Offset: 0x00019378
		public static string ConcatenateString(List<string> strings)
		{
			if (strings == null || strings.IsEmpty<string>())
			{
				return string.Empty;
			}
			string text = strings[0];
			if (strings.Count > 1)
			{
				for (int i = 1; i < strings.Count; i++)
				{
					text = text + " " + strings[i];
				}
			}
			return text;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001B1CC File Offset: 0x000193CC
		public static ItemModifier GetItemModifier(string itemModifierName)
		{
			foreach (ItemModifier itemModifier in Campaign.Current.ObjectManager.GetObjectTypeList<ItemModifier>())
			{
				if (string.Equals(itemModifier.Name.ToString().Replace(" ", ""), itemModifierName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
				{
					return itemModifier;
				}
			}
			return null;
		}

		// Token: 0x040000D9 RID: 217
		public const string CampaignNotStarted = "Campaign was not started.";

		// Token: 0x040000DA RID: 218
		public const string CheatmodeDisabled = "Cheat mode is disabled!";

		// Token: 0x040000DB RID: 219
		public const string AchievementsAreDisabled = "Achievements are disabled!";

		// Token: 0x040000DC RID: 220
		public const string Help = "help";

		// Token: 0x040000DD RID: 221
		public const string EnterNumber = "Please enter a number";

		// Token: 0x040000DE RID: 222
		public const string EnterPositiveNumber = "Please enter a positive number";

		// Token: 0x040000DF RID: 223
		public const string SettlementNotFound = "Settlement is not found";

		// Token: 0x040000E0 RID: 224
		public const string HeroNotFound = "Hero is not found";

		// Token: 0x040000E1 RID: 225
		public const string KingdomNotFound = "Kingdom is not found";

		// Token: 0x040000E2 RID: 226
		public const string VillageNotFound = "Village is not found";

		// Token: 0x040000E3 RID: 227
		public const string FactionNotFound = "Faction is not found";

		// Token: 0x040000E4 RID: 228
		public const string PartyNotFound = "Party is not found";

		// Token: 0x040000E5 RID: 229
		public const string OK = "Success";

		// Token: 0x040000E6 RID: 230
		public const string CheatNameSeparator = "|";

		// Token: 0x040000E7 RID: 231
		public const string AiDisabledHelper = "Party AI can be enabled again by using \"campaign.control_party_ai_by_cheats\"";

		// Token: 0x040000E8 RID: 232
		public static string ErrorType = "";

		// Token: 0x040000E9 RID: 233
		public const int MaxSkillValue = 300;

		// Token: 0x040000EA RID: 234
		private static bool _mainPartyIsAttackable = true;
	}
}
