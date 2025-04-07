using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F4 RID: 244
	public class DefaultCharacterDevelopmentModel : CharacterDevelopmentModel
	{
		// Token: 0x060014CD RID: 5325 RVA: 0x0005DBBE File Offset: 0x0005BDBE
		public DefaultCharacterDevelopmentModel()
		{
			this.InitializeSkillsRequiredForLevel();
			this.InitializeXpRequiredForSkillLevel();
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0005DBF0 File Offset: 0x0005BDF0
		public override List<Tuple<SkillObject, int>> GetSkillsDerivedFromTraits(Hero hero = null, CharacterObject templateCharacter = null, bool isByNaturalGrowth = false)
		{
			List<Tuple<SkillObject, int>> list = new List<Tuple<SkillObject, int>>();
			Occupation occupation = (hero != null) ? hero.Occupation : templateCharacter.Occupation;
			if (templateCharacter == null)
			{
				templateCharacter = hero.CharacterObject;
			}
			int num = templateCharacter.GetTraitLevel(DefaultTraits.Commander);
			int num2 = templateCharacter.GetTraitLevel(DefaultTraits.Manager);
			int num3 = templateCharacter.GetTraitLevel(DefaultTraits.Trader);
			int num4 = templateCharacter.GetTraitLevel(DefaultTraits.Politician);
			int traitLevel = templateCharacter.GetTraitLevel(DefaultTraits.Siegecraft);
			int traitLevel2 = templateCharacter.GetTraitLevel(DefaultTraits.SergeantCommandSkills);
			int traitLevel3 = templateCharacter.GetTraitLevel(DefaultTraits.ScoutSkills);
			int traitLevel4 = templateCharacter.GetTraitLevel(DefaultTraits.Surgery);
			int traitLevel5 = templateCharacter.GetTraitLevel(DefaultTraits.Blacksmith);
			int num5 = templateCharacter.GetTraitLevel(DefaultTraits.RogueSkills);
			int num6 = templateCharacter.GetTraitLevel(DefaultTraits.Fighter);
			if (occupation == Occupation.Merchant)
			{
				num6 = 3;
				num2 = 6;
				num3 = 8;
				num4 = 5;
				num = 2;
			}
			else if (occupation == Occupation.GangLeader)
			{
				num6 = 6;
				num2 = 3;
				num3 = 3;
				num4 = 5;
				num = 3;
				num5 = 6;
			}
			else if (occupation == Occupation.RuralNotable || occupation == Occupation.Artisan || occupation == Occupation.Headman)
			{
				num6 = 4;
				num2 = 4;
				num3 = 2;
				num4 = 5;
			}
			else if (occupation == Occupation.Preacher)
			{
				num6 = 2;
				num4 = 7;
			}
			int item = MathF.Max(num * 10 + MBRandom.RandomInt(10), traitLevel2 * 5 + MBRandom.RandomInt(10));
			int item2 = MathF.Max(num * 5 + MBRandom.RandomInt(10), traitLevel2 * 10 + MBRandom.RandomInt(10));
			int num7 = num2 * 10 + MBRandom.RandomInt(10);
			int num8 = num3 * 10 + MBRandom.RandomInt(10);
			int item3 = traitLevel * 10 + MBRandom.RandomInt(10);
			int item4 = traitLevel3 * 10 + MBRandom.RandomInt(10);
			int item5 = num4 * 10 + MBRandom.RandomInt(10);
			int item6 = num5 * 10 + MBRandom.RandomInt(10);
			int item7 = traitLevel4 * 10 + MBRandom.RandomInt(10);
			int item8 = traitLevel5 * 10 + MBRandom.RandomInt(10);
			num8 = Math.Max(num7 - 20, num8);
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Steward, num7));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Trade, num8));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Crafting, item8));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Medicine, item7));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Roguery, item6));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Leadership, item));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Tactics, item2));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Engineering, item3));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Scouting, item4));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Charm, item5));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.KnightFightingSkills));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.CavalryFightingSkills));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.HorseArcherFightingSkills));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.HopliteFightingSkills));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.PeltastFightingSkills));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.HuscarlFightingSkills));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.ArcherFIghtingSkills));
			num6 = MathF.Max(num6, templateCharacter.GetTraitLevel(DefaultTraits.CrossbowmanStyle));
			int num9 = num6 * 30 + MBRandom.RandomInt(10);
			int num10 = num6 * 30 + MBRandom.RandomInt(10);
			int num11 = num6 * 30 + MBRandom.RandomInt(10);
			int num12 = num6 * 25 + MBRandom.RandomInt(10);
			int num13 = num6 * 20 + MBRandom.RandomInt(10);
			int num14 = num6 * 20 + MBRandom.RandomInt(10);
			int num15 = num6 * 20 + MBRandom.RandomInt(10);
			int num16 = num6 * 20 + MBRandom.RandomInt(10);
			if (templateCharacter.GetTraitLevel(DefaultTraits.KnightFightingSkills) > 0)
			{
				num16 += 50;
				num9 += 10;
				num11 += 20;
				num12 -= 30;
				num13 -= 30;
				num14 -= 30;
				num15 += 10;
			}
			if (templateCharacter.GetTraitLevel(DefaultTraits.CavalryFightingSkills) > 0)
			{
				num16 += 50;
				num11 += 10;
				num14 += 30;
				num12 -= 20;
				num13 -= 40;
				num10 -= 20;
				num15 -= 10;
			}
			if (templateCharacter.GetTraitLevel(DefaultTraits.HorseArcherFightingSkills) > 0)
			{
				num16 += 40;
				num12 += 30;
				num11 -= 10;
				num10 -= 30;
				num13 -= 10;
				num14 -= 10;
				num15 -= 10;
			}
			if (templateCharacter.GetTraitLevel(DefaultTraits.ArcherFIghtingSkills) > 0)
			{
				num10 -= 20;
				num11 -= 30;
				num16 -= 30;
				num13 -= 20;
				num14 -= 20;
				num12 += 60;
				num9 -= 10;
				num15 += 10;
			}
			if (templateCharacter.GetTraitLevel(DefaultTraits.HuscarlFightingSkills) > 0)
			{
				num10 += 50;
				num11 += 20;
				num12 -= 20;
				num13 -= 20;
				num14 -= 20;
				num15 += 10;
				num16 -= 20;
			}
			if (templateCharacter.GetTraitLevel(DefaultTraits.PeltastFightingSkills) > 0)
			{
				num14 += 30;
				num15 += 30;
				num9 += 10;
				num10 -= 20;
				num11 -= 20;
				num12 -= 20;
				num13 -= 20;
				num16 -= 10;
			}
			if (templateCharacter.GetTraitLevel(DefaultTraits.HopliteFightingSkills) > 0)
			{
				num11 += 20;
				num9 += 10;
				num10 -= 10;
				num15 += 10;
				num12 -= 20;
				num13 -= 20;
				num16 -= 10;
				num14 -= 20;
			}
			if (templateCharacter.GetTraitLevel(DefaultTraits.CrossbowmanStyle) > 0)
			{
				num13 += 60;
				num14 -= 20;
				num11 -= 20;
				num10 -= 10;
				num12 -= 20;
				num15 -= 10;
				num16 -= 20;
			}
			if (occupation == Occupation.Lord)
			{
				num16 += 20;
				num16 = MathF.Max(num16, 100);
			}
			if (occupation == Occupation.Wanderer)
			{
				if (num9 < num6 * 30)
				{
					num9 = MBRandom.RandomInt(5);
				}
				if (num10 < num6 * 30)
				{
					num10 = MBRandom.RandomInt(5);
				}
				if (num11 < num6 * 30)
				{
					num11 = MBRandom.RandomInt(5);
				}
				if (num12 < num6 * 25)
				{
					num12 = MBRandom.RandomInt(5);
				}
				if (num13 < num6 * 20)
				{
					num13 = MBRandom.RandomInt(5);
				}
				if (num14 < num6 * 20)
				{
					num14 = MBRandom.RandomInt(5);
				}
				if (num15 < num6 * 20)
				{
					num15 = MBRandom.RandomInt(5);
				}
				if (num16 < num6 * 20)
				{
					num16 = MBRandom.RandomInt(5);
				}
			}
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.OneHanded, num9));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.TwoHanded, num10));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Polearm, num11));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Bow, num12));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Crossbow, num13));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Throwing, num14));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Athletics, num15));
			list.Add(new Tuple<SkillObject, int>(DefaultSkills.Riding, num16));
			if (hero != null)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					SkillObject item9 = list[i].Item1;
					float item10 = (float)list[i].Item2;
					float skillScalingModifierForAge = Campaign.Current.Models.AgeModel.GetSkillScalingModifierForAge(hero, item9, isByNaturalGrowth);
					int item11 = MathF.Floor(item10 * skillScalingModifierForAge);
					list[i] = new Tuple<SkillObject, int>(item9, item11);
				}
			}
			return list;
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0005E33C File Offset: 0x0005C53C
		private void InitializeSkillsRequiredForLevel()
		{
			int num = 1000;
			int num2 = 1;
			this._skillsRequiredForLevel[0] = 0;
			this._skillsRequiredForLevel[1] = 1;
			for (int i = 2; i < this._skillsRequiredForLevel.Length; i++)
			{
				num2 += num;
				this._skillsRequiredForLevel[i] = num2;
				num += 1000 + num / 5;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0005E38F File Offset: 0x0005C58F
		public override int MaxFocusPerSkill
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x060014D1 RID: 5329 RVA: 0x0005E392 File Offset: 0x0005C592
		public override int MaxAttribute
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0005E396 File Offset: 0x0005C596
		public override int SkillsRequiredForLevel(int level)
		{
			if (level > 62)
			{
				return int.MaxValue;
			}
			return this._skillsRequiredForLevel[level];
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0005E3AB File Offset: 0x0005C5AB
		public override int GetMaxSkillPoint()
		{
			return int.MaxValue;
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0005E3B4 File Offset: 0x0005C5B4
		private void InitializeXpRequiredForSkillLevel()
		{
			int num = 30;
			this._xpRequiredForSkillLevel[0] = num;
			for (int i = 1; i < 1024; i++)
			{
				num += 10 + i;
				this._xpRequiredForSkillLevel[i] = this._xpRequiredForSkillLevel[i - 1] + num;
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x0005E3F8 File Offset: 0x0005C5F8
		public override int GetXpRequiredForSkillLevel(int skillLevel)
		{
			if (skillLevel > 1024)
			{
				skillLevel = 1024;
			}
			if (skillLevel <= 0)
			{
				return 0;
			}
			return this._xpRequiredForSkillLevel[skillLevel - 1];
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0005E41C File Offset: 0x0005C61C
		public override int GetSkillLevelChange(Hero hero, SkillObject skill, float skillXp)
		{
			int num = 0;
			int skillValue = hero.GetSkillValue(skill);
			for (int i = 0; i < 1024 - skillValue; i++)
			{
				int num2 = skillValue + i;
				if (num2 < 1023)
				{
					if (skillXp < (float)this._xpRequiredForSkillLevel[num2])
					{
						break;
					}
					num++;
				}
			}
			return num;
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0005E464 File Offset: 0x0005C664
		public override int GetXpAmountForSkillLevelChange(Hero hero, SkillObject skill, int skillLevelChange)
		{
			int skillValue = hero.GetSkillValue(skill);
			return this._xpRequiredForSkillLevel[skillValue + skillLevelChange] - this._xpRequiredForSkillLevel[skillValue];
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0005E48C File Offset: 0x0005C68C
		public override void GetTraitLevelForTraitXp(Hero hero, TraitObject trait, int xpValue, out int traitLevel, out int clampedTraitXp)
		{
			clampedTraitXp = xpValue;
			int num = (trait.MinValue < -1) ? -6000 : ((trait.MinValue == -1) ? -2500 : 0);
			int num2 = (trait.MaxValue > 1) ? 6000 : ((trait.MaxValue == 1) ? 2500 : 0);
			if (xpValue > num2)
			{
				clampedTraitXp = num2;
			}
			else if (xpValue < num)
			{
				clampedTraitXp = num;
			}
			traitLevel = ((clampedTraitXp <= -4000) ? -2 : ((clampedTraitXp <= -1000) ? -1 : ((clampedTraitXp < 1000) ? 0 : ((clampedTraitXp < 4000) ? 1 : 2))));
			if (traitLevel < trait.MinValue)
			{
				traitLevel = trait.MinValue;
				return;
			}
			if (traitLevel > trait.MaxValue)
			{
				traitLevel = trait.MaxValue;
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0005E555 File Offset: 0x0005C755
		public override int GetTraitXpRequiredForTraitLevel(TraitObject trait, int traitLevel)
		{
			if (traitLevel < -1)
			{
				return -4000;
			}
			if (traitLevel == -1)
			{
				return -1000;
			}
			if (traitLevel == 0)
			{
				return 0;
			}
			if (traitLevel != 1)
			{
				return 4000;
			}
			return 1000;
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x0005E57F File Offset: 0x0005C77F
		public override int AttributePointsAtStart
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x0005E583 File Offset: 0x0005C783
		public override int LevelsPerAttributePoint
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x0005E586 File Offset: 0x0005C786
		public override int FocusPointsPerLevel
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x0005E589 File Offset: 0x0005C789
		public override int FocusPointsAtStart
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x0005E58C File Offset: 0x0005C78C
		public override int MaxSkillRequiredForEpicPerkBonus
		{
			get
			{
				return 250;
			}
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x0005E593 File Offset: 0x0005C793
		public override int MinSkillRequiredForEpicPerkBonus
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0005E59C File Offset: 0x0005C79C
		public override ExplainedNumber CalculateLearningLimit(int attributeValue, int focusValue, TextObject attributeName, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(0f, includeDescriptions, null);
			result.Add((float)((attributeValue - 1) * 10), attributeName, null);
			result.Add((float)(focusValue * 30), DefaultCharacterDevelopmentModel._skillFocusText, null);
			result.LimitMin(0f);
			return result;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0005E5E8 File Offset: 0x0005C7E8
		public override float CalculateLearningRate(Hero hero, SkillObject skill)
		{
			int level = hero.Level;
			int attributeValue = hero.GetAttributeValue(skill.CharacterAttribute);
			int focus = hero.HeroDeveloper.GetFocus(skill);
			int skillValue = hero.GetSkillValue(skill);
			return this.CalculateLearningRate(attributeValue, focus, skillValue, level, skill.CharacterAttribute.Name, false).ResultNumber;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0005E640 File Offset: 0x0005C840
		public override ExplainedNumber CalculateLearningRate(int attributeValue, int focusValue, int skillValue, int characterLevel, TextObject attributeName, bool includeDescriptions = false)
		{
			ExplainedNumber result = new ExplainedNumber(1.25f, includeDescriptions, null);
			result.AddFactor(0.4f * (float)attributeValue, attributeName);
			result.AddFactor((float)focusValue * 1f, DefaultCharacterDevelopmentModel._skillFocusText);
			int num = MathF.Round(this.CalculateLearningLimit(attributeValue, focusValue, null, false).ResultNumber);
			if (skillValue > num)
			{
				int num2 = skillValue - num;
				result.AddFactor(-1f - 0.1f * (float)num2, DefaultCharacterDevelopmentModel._overLimitText);
			}
			result.LimitMin(0f);
			return result;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0005E6C8 File Offset: 0x0005C8C8
		public override SkillObject GetNextSkillToAddFocus(Hero hero)
		{
			SkillObject result = null;
			float num = float.MinValue;
			foreach (SkillObject skillObject in Skills.All)
			{
				if (hero.HeroDeveloper.CanAddFocusToSkill(skillObject))
				{
					int attributeValue = hero.GetAttributeValue(skillObject.CharacterAttribute);
					int focus = hero.HeroDeveloper.GetFocus(skillObject);
					float num2 = (float)hero.GetSkillValue(skillObject) - this.CalculateLearningLimit(attributeValue, focus, null, false).ResultNumber;
					if (num2 > num)
					{
						num = num2;
						result = skillObject;
					}
				}
			}
			return result;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0005E770 File Offset: 0x0005C970
		public override CharacterAttribute GetNextAttributeToUpgrade(Hero hero)
		{
			CharacterAttribute result = null;
			float num = float.MinValue;
			foreach (CharacterAttribute characterAttribute in Attributes.All)
			{
				int attributeValue = hero.GetAttributeValue(characterAttribute);
				if (attributeValue < this.MaxAttribute)
				{
					float num2 = 0f;
					if (attributeValue == 0)
					{
						num2 = float.MaxValue;
					}
					else
					{
						foreach (SkillObject skill in characterAttribute.Skills)
						{
							float num3 = MathF.Max(0f, (float)(75 + hero.GetSkillValue(skill)) - this.CalculateLearningLimit(attributeValue, hero.HeroDeveloper.GetFocus(skill), null, false).ResultNumber);
							num2 += num3;
						}
						int num4 = 1;
						foreach (CharacterAttribute characterAttribute2 in Attributes.All)
						{
							if (characterAttribute2 != characterAttribute)
							{
								int attributeValue2 = hero.GetAttributeValue(characterAttribute2);
								if (num4 < attributeValue2)
								{
									num4 = attributeValue2;
								}
							}
						}
						float num5 = MathF.Sqrt((float)num4 / (float)attributeValue);
						num2 *= num5;
					}
					if (num2 > num)
					{
						num = num2;
						result = characterAttribute;
					}
				}
			}
			return result;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0005E910 File Offset: 0x0005CB10
		public override PerkObject GetNextPerkToChoose(Hero hero, PerkObject perk)
		{
			PerkObject result = perk;
			if (perk.AlternativePerk != null && MBRandom.RandomFloat < 0.5f)
			{
				result = perk.AlternativePerk;
			}
			return result;
		}

		// Token: 0x0400072B RID: 1835
		private const int MaxCharacterLevels = 62;

		// Token: 0x0400072C RID: 1836
		private const int SkillPointsAtLevel1 = 1;

		// Token: 0x0400072D RID: 1837
		private const int SkillPointsGainNeededInitialValue = 1000;

		// Token: 0x0400072E RID: 1838
		private const int SkillPointsGainNeededIncreasePerLevel = 1000;

		// Token: 0x0400072F RID: 1839
		private readonly int[] _skillsRequiredForLevel = new int[63];

		// Token: 0x04000730 RID: 1840
		private const int FocusPointsPerLevelConst = 1;

		// Token: 0x04000731 RID: 1841
		private const int LevelsPerAttributePointConst = 4;

		// Token: 0x04000732 RID: 1842
		private const int FocusPointsAtStartConst = 5;

		// Token: 0x04000733 RID: 1843
		private const int AttributePointsAtStartConst = 15;

		// Token: 0x04000734 RID: 1844
		private const int MaxSkillLevels = 1024;

		// Token: 0x04000735 RID: 1845
		private readonly int[] _xpRequiredForSkillLevel = new int[1024];

		// Token: 0x04000736 RID: 1846
		private const int XpRequirementForFirstLevel = 30;

		// Token: 0x04000737 RID: 1847
		private const int MaxSkillPoint = 2147483647;

		// Token: 0x04000738 RID: 1848
		private const float BaseLearningRate = 1.25f;

		// Token: 0x04000739 RID: 1849
		private const int TraitThreshold2 = 4000;

		// Token: 0x0400073A RID: 1850
		private const int TraitMaxValue1 = 2500;

		// Token: 0x0400073B RID: 1851
		private const int TraitThreshold1 = 1000;

		// Token: 0x0400073C RID: 1852
		private const int TraitMaxValue2 = 6000;

		// Token: 0x0400073D RID: 1853
		private const int SkillLevelVariant = 10;

		// Token: 0x0400073E RID: 1854
		private static readonly TextObject _skillFocusText = new TextObject("{=MRktqZwu}Skill Focus", null);

		// Token: 0x0400073F RID: 1855
		private static readonly TextObject _overLimitText = new TextObject("{=bcA7ZuyO}Learning Limit Exceeded", null);
	}
}
