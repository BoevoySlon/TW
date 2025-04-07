using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000100 RID: 256
	public class DefaultDifficultyModel : DifficultyModel
	{
		// Token: 0x06001553 RID: 5459 RVA: 0x00062ABC File Offset: 0x00060CBC
		public override float GetPlayerTroopsReceivedDamageMultiplier()
		{
			switch (CampaignOptions.PlayerTroopsReceivedDamage)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.5f;
			case CampaignOptions.Difficulty.Easy:
				return 0.75f;
			case CampaignOptions.Difficulty.Realistic:
				return 1f;
			default:
				return 1f;
			}
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00062AFC File Offset: 0x00060CFC
		public override float GetDamageToPlayerMultiplier()
		{
			switch (CampaignOptions.PlayerReceivedDamage)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.25f;
			case CampaignOptions.Difficulty.Easy:
				return 0.5f;
			case CampaignOptions.Difficulty.Realistic:
				return 1f;
			default:
				return 1f;
			}
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00062B3C File Offset: 0x00060D3C
		public override int GetPlayerRecruitSlotBonus()
		{
			switch (CampaignOptions.RecruitmentDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 2;
			case CampaignOptions.Difficulty.Easy:
				return 1;
			case CampaignOptions.Difficulty.Realistic:
				return 0;
			default:
				return 0;
			}
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00062B6C File Offset: 0x00060D6C
		public override float GetPlayerMapMovementSpeedBonusMultiplier()
		{
			switch (CampaignOptions.PlayerMapMovementSpeed)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.1f;
			case CampaignOptions.Difficulty.Easy:
				return 0.05f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00062BAC File Offset: 0x00060DAC
		public override float GetCombatAIDifficultyMultiplier()
		{
			switch (CampaignOptions.CombatAIDifficulty)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.1f;
			case CampaignOptions.Difficulty.Easy:
				return 0.32f;
			case CampaignOptions.Difficulty.Realistic:
				return 0.96f;
			default:
				return 0.5f;
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00062BEC File Offset: 0x00060DEC
		public override float GetPersuasionBonusChance()
		{
			switch (CampaignOptions.PersuasionSuccessChance)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return 0.1f;
			case CampaignOptions.Difficulty.Easy:
				return 0.05f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00062C2C File Offset: 0x00060E2C
		public override float GetClanMemberDeathChanceMultiplier()
		{
			switch (CampaignOptions.ClanMemberDeathChance)
			{
			case CampaignOptions.Difficulty.VeryEasy:
				return -1f;
			case CampaignOptions.Difficulty.Easy:
				return -0.5f;
			case CampaignOptions.Difficulty.Realistic:
				return 0f;
			default:
				return 0f;
			}
		}
	}
}
