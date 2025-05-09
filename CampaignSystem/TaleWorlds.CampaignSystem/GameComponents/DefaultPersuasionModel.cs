﻿using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000127 RID: 295
	public class DefaultPersuasionModel : PersuasionModel
	{
		// Token: 0x060016EE RID: 5870 RVA: 0x0007104C File Offset: 0x0006F24C
		public override int GetSkillXpFromPersuasion(PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient)
		{
			return (difficulty - PersuasionDifficulty.VeryEasy) / 1 * 5 * argumentDifficultyBonusCoefficient;
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00071058 File Offset: 0x0006F258
		public override void GetChances(PersuasionOptionArgs optionArgs, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance, float difficultyMultiplier)
		{
			float defaultSuccessChance = this.GetDefaultSuccessChance(optionArgs, difficultyMultiplier);
			switch (optionArgs.ArgumentStrength)
			{
			case PersuasionArgumentStrength.ExtremelyHard:
				successChance = defaultSuccessChance * 0.05f;
				critFailChance = defaultSuccessChance * 0.15f;
				goto IL_C3;
			case PersuasionArgumentStrength.VeryHard:
				successChance = defaultSuccessChance * 0.15f;
				critFailChance = defaultSuccessChance * 0.2f;
				goto IL_C3;
			case PersuasionArgumentStrength.Hard:
				successChance = defaultSuccessChance * 0.35f;
				critFailChance = defaultSuccessChance * 0.15f;
				goto IL_C3;
			case PersuasionArgumentStrength.Easy:
				successChance = defaultSuccessChance * 0.7f;
				critFailChance = 0f;
				goto IL_C3;
			case PersuasionArgumentStrength.VeryEasy:
				successChance = defaultSuccessChance * 0.8f;
				critFailChance = 0f;
				goto IL_C3;
			case PersuasionArgumentStrength.ExtremelyEasy:
				successChance = defaultSuccessChance * 0.9f;
				critFailChance = 0f;
				goto IL_C3;
			}
			successChance = defaultSuccessChance * 0.55f;
			critFailChance = defaultSuccessChance * 0.15f;
			IL_C3:
			float bonusSuccessChance = this.GetBonusSuccessChance(optionArgs);
			successChance = MathF.Clamp(successChance * bonusSuccessChance, 0f, 1f);
			critFailChance = MathF.Clamp(critFailChance * (2f - bonusSuccessChance), 0f, 1f);
			critSuccessChance = 0f;
			if (optionArgs.GivesCriticalSuccess)
			{
				critSuccessChance = successChance;
				successChance = 0f;
			}
			if (critSuccessChance > 0f && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.MeaningfulFavors))
			{
				critSuccessChance = MathF.Clamp(critSuccessChance + critSuccessChance * DefaultPerks.Charm.MeaningfulFavors.PrimaryBonus, 0f, 1f);
			}
			failChance = 1f - critSuccessChance - successChance - critFailChance;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x000711CC File Offset: 0x0006F3CC
		private float GetDefaultSuccessChance(PersuasionOptionArgs optionArgs, float difficultyMultiplier)
		{
			int skillValue = Hero.MainHero.GetSkillValue(optionArgs.SkillUsed);
			float num = (float)((optionArgs.TraitEffect == TraitEffect.Positive) ? Hero.MainHero.GetTraitLevel(optionArgs.TraitUsed) : (-(float)Hero.MainHero.GetTraitLevel(optionArgs.TraitUsed)));
			num = MBMath.ClampFloat((optionArgs.TraitUsed != null) ? num : 0f, -1f, 2f);
			float num2;
			if (num <= 0f)
			{
				num2 = 0.2f + (num - -1f) * 0.4f;
			}
			else if (num <= 1f)
			{
				num2 = 0.6f + (num - 0f) * 0.6f;
			}
			else
			{
				num2 = 1.2f + (num - 1f) * 0.4f;
			}
			return MathF.Clamp((100f - difficultyMultiplier / (0.01f * (100f + (float)skillValue * num2))) / 100f, 0.1f, 1f);
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x000712BC File Offset: 0x0006F4BC
		private float GetBonusSuccessChance(PersuasionOptionArgs optionArgs)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			explainedNumber.AddFactor(MathF.Clamp((float)(Hero.MainHero.GetSkillValue(optionArgs.SkillUsed) / Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus) * 0.2f, 0f, 0.2f), optionArgs.SkillUsed.Name);
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Athletics.ImposingStature))
			{
				explainedNumber.AddFactor(DefaultPerks.Athletics.ImposingStature.PrimaryBonus, DefaultPerks.Athletics.ImposingStature.Name);
			}
			float persuasionBonusChance = Campaign.Current.Models.DifficultyModel.GetPersuasionBonusChance();
			if (persuasionBonusChance > 0f)
			{
				explainedNumber.AddFactor(persuasionBonusChance, GameTexts.FindText("str_game_difficulty", null));
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00071388 File Offset: 0x0006F588
		public override void GetEffectChances(PersuasionOptionArgs option, out float moveToNextStageChance, out float blockRandomOptionChance, float difficultyMultiplier)
		{
			moveToNextStageChance = 0f;
			blockRandomOptionChance = 0f;
			if (option.CanMoveToTheNextReservation || option.CanBlockOtherOption)
			{
				moveToNextStageChance = (option.CanMoveToTheNextReservation ? 1f : 0f);
				blockRandomOptionChance = (option.CanBlockOtherOption ? 1f : 0f);
			}
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x000713E0 File Offset: 0x0006F5E0
		public override PersuasionArgumentStrength GetArgumentStrengthBasedOnTargetTraits(CharacterObject character, Tuple<TraitObject, int>[] traitCorrelations)
		{
			float num = 0f;
			float num2 = 1f;
			for (int i = 0; i < traitCorrelations.Length; i++)
			{
				num2 += (float)(character.GetTraitLevel(traitCorrelations[i].Item1) * traitCorrelations[i].Item2);
				num += (float)MathF.Abs(traitCorrelations[i].Item2);
				num2 += (float)(CharacterObject.PlayerCharacter.GetTraitLevel(traitCorrelations[i].Item1) * traitCorrelations[i].Item2);
				num += (float)MathF.Abs(traitCorrelations[i].Item2);
			}
			if (num2 > num / 6f)
			{
				return PersuasionArgumentStrength.VeryEasy;
			}
			if (num2 > 0f)
			{
				return PersuasionArgumentStrength.ExtremelyEasy;
			}
			if (num2 < num / -6f)
			{
				return PersuasionArgumentStrength.VeryHard;
			}
			if (num2 < 0f)
			{
				return PersuasionArgumentStrength.Hard;
			}
			return PersuasionArgumentStrength.Normal;
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00071494 File Offset: 0x0006F694
		public override float CalculateInitialPersuasionProgress(CharacterObject character, float goalValue, float successValue)
		{
			float result;
			if (!character.IsHero)
			{
				result = 0f;
			}
			else
			{
				result = MathF.Max(0f, character.HeroObject.GetRelationWithPlayer() * successValue) / (float)Campaign.Current.Models.DiplomacyModel.MaxRelationLimit;
			}
			return result;
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x000714E0 File Offset: 0x0006F6E0
		public override float CalculatePersuasionGoalValue(CharacterObject oneToOneConversationCharacter, float baseGoalValue)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(baseGoalValue, false, null);
			if (CharacterObject.OneToOneConversationCharacter != null && CharacterObject.OneToOneConversationCharacter.IsHero)
			{
				if (CharacterObject.OneToOneConversationCharacter.HeroObject.MapFaction == Hero.MainHero.MapFaction && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.MoralLeader))
				{
					explainedNumber.Add(DefaultPerks.Charm.MoralLeader.PrimaryBonus, DefaultPerks.Charm.MoralLeader.Name, null);
				}
				else if (CharacterObject.OneToOneConversationCharacter.HeroObject.MapFaction != Hero.MainHero.MapFaction && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.NaturalLeader))
				{
					explainedNumber.Add(DefaultPerks.Charm.NaturalLeader.PrimaryBonus, DefaultPerks.Charm.NaturalLeader.Name, null);
				}
			}
			explainedNumber.LimitMin(1f);
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x000715B8 File Offset: 0x0006F7B8
		public override float GetDifficulty(PersuasionDifficulty difficulty)
		{
			switch (difficulty)
			{
			case PersuasionDifficulty.VeryEasy:
				return 0.9f;
			case PersuasionDifficulty.Easy:
				return 0.8f;
			case PersuasionDifficulty.EasyMedium:
				return 0.7f;
			case PersuasionDifficulty.Medium:
				return 0.6f;
			case PersuasionDifficulty.MediumHard:
				return 0.5f;
			case PersuasionDifficulty.Hard:
				return 0.4f;
			case PersuasionDifficulty.VeryHard:
				return 0.3f;
			case PersuasionDifficulty.UltraHard:
				return 0.2f;
			case PersuasionDifficulty.Impossible:
				return 0.1f;
			default:
				return 1f;
			}
		}
	}
}
