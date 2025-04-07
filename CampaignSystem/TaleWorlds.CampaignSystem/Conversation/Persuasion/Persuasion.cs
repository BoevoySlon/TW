using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Conversation.Persuasion
{
	// Token: 0x02000253 RID: 595
	public class Persuasion
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06001F5F RID: 8031 RVA: 0x00089B93 File Offset: 0x00087D93
		public float DifficultyMultiplier
		{
			get
			{
				return this._difficultyMultiplier;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x00089B9B File Offset: 0x00087D9B
		// (set) Token: 0x06001F61 RID: 8033 RVA: 0x00089BA3 File Offset: 0x00087DA3
		public float Progress { get; private set; }

		// Token: 0x06001F62 RID: 8034 RVA: 0x00089BAC File Offset: 0x00087DAC
		public Persuasion(float goalValue, float successValue, float failValue, float criticalSuccessValue, float criticalFailValue, float initialProgress, PersuasionDifficulty difficulty)
		{
			this._chosenOptions = new List<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>();
			this.GoalValue = Campaign.Current.Models.PersuasionModel.CalculatePersuasionGoalValue(CharacterObject.OneToOneConversationCharacter, goalValue);
			this.SuccessValue = successValue;
			this.FailValue = failValue;
			this.CriticalSuccessValue = criticalSuccessValue;
			this.CriticalFailValue = criticalFailValue;
			this._difficulty = difficulty;
			if (initialProgress < 0f)
			{
				this.Progress = Campaign.Current.Models.PersuasionModel.CalculateInitialPersuasionProgress(CharacterObject.OneToOneConversationCharacter, this.GoalValue, this.SuccessValue);
			}
			else
			{
				this.Progress = initialProgress;
			}
			this._difficultyMultiplier = Campaign.Current.Models.PersuasionModel.GetDifficulty(difficulty);
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x00089C6C File Offset: 0x00087E6C
		public void CommitProgress(PersuasionOptionArgs persuasionOptionArgs)
		{
			PersuasionOptionResult persuasionOptionResult = this.GetResult(persuasionOptionArgs);
			persuasionOptionResult = this.CheckPerkEffectOnResult(persuasionOptionResult);
			Tuple<PersuasionOptionArgs, PersuasionOptionResult> tuple = new Tuple<PersuasionOptionArgs, PersuasionOptionResult>(persuasionOptionArgs, persuasionOptionResult);
			persuasionOptionArgs.BlockTheOption(true);
			this._chosenOptions.Add(tuple);
			this.Progress = MathF.Clamp(this.Progress + this.GetPersuasionOptionResultValue(persuasionOptionResult), 0f, this.GoalValue);
			CampaignEventDispatcher.Instance.OnPersuasionProgressCommitted(tuple);
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x00089CD4 File Offset: 0x00087ED4
		private PersuasionOptionResult CheckPerkEffectOnResult(PersuasionOptionResult result)
		{
			PersuasionOptionResult result2 = result;
			if (result == PersuasionOptionResult.CriticalFailure && Hero.MainHero.GetPerkValue(DefaultPerks.Charm.ForgivableGrievances) && MBRandom.RandomFloat <= DefaultPerks.Charm.ForgivableGrievances.PrimaryBonus)
			{
				TextObject textObject = new TextObject("{=5IQriov5}You avoided critical failure because of {PERK_NAME}.", null);
				textObject.SetTextVariable("PERK_NAME", DefaultPerks.Charm.ForgivableGrievances.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), Color.White));
				result2 = PersuasionOptionResult.Failure;
			}
			return result2;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x00089D40 File Offset: 0x00087F40
		private float GetPersuasionOptionResultValue(PersuasionOptionResult result)
		{
			switch (result)
			{
			case PersuasionOptionResult.CriticalFailure:
				return -this.CriticalFailValue;
			case PersuasionOptionResult.Failure:
				return 0f;
			case PersuasionOptionResult.Success:
				return this.SuccessValue;
			case PersuasionOptionResult.CriticalSuccess:
				return this.CriticalSuccessValue;
			case PersuasionOptionResult.Miss:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x00089D90 File Offset: 0x00087F90
		private PersuasionOptionResult GetResult(PersuasionOptionArgs optionArgs)
		{
			float num;
			float num2;
			float num3;
			float num4;
			Campaign.Current.Models.PersuasionModel.GetChances(optionArgs, out num, out num2, out num3, out num4, this._difficultyMultiplier);
			float num5 = MBRandom.RandomFloat;
			if (num5 < num2)
			{
				return PersuasionOptionResult.CriticalSuccess;
			}
			num5 -= num2;
			if (num5 < num)
			{
				return PersuasionOptionResult.Success;
			}
			num5 -= num;
			if (num5 < num4)
			{
				return PersuasionOptionResult.Failure;
			}
			num5 -= num4;
			if (num5 < num3)
			{
				return PersuasionOptionResult.CriticalFailure;
			}
			return PersuasionOptionResult.Miss;
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x00089DF6 File Offset: 0x00087FF6
		public IEnumerable<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> GetChosenOptions()
		{
			return this._chosenOptions.AsReadOnly();
		}

		// Token: 0x040009FD RID: 2557
		public readonly float SuccessValue;

		// Token: 0x040009FE RID: 2558
		public readonly float FailValue;

		// Token: 0x040009FF RID: 2559
		public readonly float CriticalSuccessValue;

		// Token: 0x04000A00 RID: 2560
		public readonly float CriticalFailValue;

		// Token: 0x04000A01 RID: 2561
		private readonly float _difficultyMultiplier;

		// Token: 0x04000A02 RID: 2562
		private readonly PersuasionDifficulty _difficulty;

		// Token: 0x04000A03 RID: 2563
		private readonly List<Tuple<PersuasionOptionArgs, PersuasionOptionResult>> _chosenOptions;

		// Token: 0x04000A04 RID: 2564
		public readonly float GoalValue;
	}
}
