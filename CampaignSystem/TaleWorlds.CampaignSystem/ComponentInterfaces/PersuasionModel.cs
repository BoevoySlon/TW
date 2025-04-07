using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000175 RID: 373
	public abstract class PersuasionModel : GameModel
	{
		// Token: 0x0600195D RID: 6493
		public abstract int GetSkillXpFromPersuasion(PersuasionDifficulty difficulty, int argumentDifficultyBonusCoefficient);

		// Token: 0x0600195E RID: 6494
		public abstract void GetChances(PersuasionOptionArgs optionArgs, out float successChance, out float critSuccessChance, out float critFailChance, out float failChance, float difficultyMultiplier);

		// Token: 0x0600195F RID: 6495
		public abstract void GetEffectChances(PersuasionOptionArgs option, out float moveToNextStageChance, out float blockRandomOptionChance, float difficultyMultiplier);

		// Token: 0x06001960 RID: 6496
		public abstract PersuasionArgumentStrength GetArgumentStrengthBasedOnTargetTraits(CharacterObject character, Tuple<TraitObject, int>[] traitCorrelation);

		// Token: 0x06001961 RID: 6497
		public abstract float GetDifficulty(PersuasionDifficulty difficulty);

		// Token: 0x06001962 RID: 6498
		public abstract float CalculateInitialPersuasionProgress(CharacterObject character, float goalValue, float successValue);

		// Token: 0x06001963 RID: 6499
		public abstract float CalculatePersuasionGoalValue(CharacterObject oneToOneConversationCharacter, float successValue);
	}
}
